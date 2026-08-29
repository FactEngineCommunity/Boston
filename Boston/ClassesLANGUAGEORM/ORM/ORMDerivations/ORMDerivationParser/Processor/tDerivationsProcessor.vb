Imports System.Reflection

Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Text.RegularExpressions

Namespace Derivations

    'DERIVATION
    '└─ DERIVATION_SENTENCE
    '   ├─ DEFINITION_DERIVATION
    '   └─ BICONDITIONAL_DERIVATION
    '      ├─ DERIVATION_HEAD_READING
    '      └─ DERIVATION_EXPRESSION
    '         ├─ DERIVATION_CLAUSE
    '         │  └─ FACT_TYPE_PATH
    '         │     └─ FACT_TYPE_PATH_COMPONENT*
    '         └─ LOGICAL_CONNECTIVE*

    ''' <summary>
    ''' Takes a freeform text (NORMA style) Derivation, and using the TinyPG classes, creates a parallel to the ParseTree (for a Derivation), 
    '''   to map over to the effective AST FBM.(Derivations Classes), so that a freeform text Derivation written in Boston can be stored in a .fbm XML FBM Model file.
    ''' </summary>
    Public Class Processor

        Private Enum SurfaceTokenKind
            Literal
            ModelReference
        End Enum

        Private Class SurfaceToken
            Public Property Kind As SurfaceTokenKind
            Public Property Text As String = ""
            Public Property ModelReference As Derivations.ModelElementReference
            Public Property ModelObject As FBM.ModelObject
            Public Property BindingKey As String = ""
            Public Property IsNegativeQuantifier As Boolean = False
        End Class

        Private Class ExpectedReadingToken
            Public Property Kind As SurfaceTokenKind
            Public Property Text As String = ""
            Public Property Role As FBM.Role
        End Class

        Private Class ReadingMatch
            Public Property Reading As FBM.FactTypeReading
            Public Property NextTokenIndex As Integer
            Public Property Roles As New List(Of FBM.Role)
            Public Property BindingKeys As New List(Of String)
            Public Property IsNegated As Boolean = False
        End Class

        Private Class LinearPathSolution
            Public Property Occurrences As New List(Of ReadingMatch)
            Public Property NegatedOccurrenceIndexes As New HashSet(Of Integer)
        End Class

        Private Class BindingOccurrence
            Public Property IsPathRoot As Boolean
            Public Property BranchIndex As Integer = 0
            Public Property PathIndex As Integer = -1
            Public Property OccurrenceIndex As Integer = -1
            Public Property RoleIndex As Integer = -1
            Public Property OccurrenceRoleCount As Integer
            Public Property PathedRole As FBM.PathedRole
        End Class

        Private Enum CalculatedArgumentKind
            Binding
            Constant
            FunctionValue
        End Enum

        Private Class CalculatedArgumentPlan
            Public Property Kind As CalculatedArgumentKind
            Public Property BindingKey As String = ""
            Public Property ConstantValue As String = ""
            Public Property FunctionExpression As CalculatedExpressionPlan
            Public Property IsBagInput As Boolean = False
        End Class

        Private Class CalculatedExpressionPlan
            Public Property FunctionName As String = ""
            Public Property OperatorSymbol As String = ""
            Public Property Arguments As New List(Of CalculatedArgumentPlan)
            Public Property AggregationContextBindingKey As String = ""
        End Class

        Private Class CalculatedWherePlan
            Public Property TargetBindingKey As String = ""
            Public Property ComparisonOperatorSymbol As String = "="
            Public Property Expression As CalculatedExpressionPlan
            Public Property RightArgument As CalculatedArgumentPlan
            Public Property ScopeClauseIndex As Integer = -1
        End Class

        ''' <summary>
        ''' Gets the respective element of the ParseTree/Node.
        '''   NB A ParseTree is a Node.
        ''' </summary>
        ''' <param name="ao_object">An instance/object of the visitor class representing the type of Node within the ParseTree/Node.</param>
        ''' <param name="aoParseTreeNode">ParseTree/Node. A ParseTree is a Node.</param>
        Public Sub GetParseTreeTokensReflection(ByRef ao_object As Object, ByRef aoParseTreeNode As ORMDerivation.ParseNode)

            '-------------------------------
            'NB IMPORTANT: The TokenTypes in the ParseTree must be the same as established in aoObject (as hardcoded here in Richmond).
            'i.e. Richmond (the software) and the Parser (to the outside word) are linked.
            'If the Tokens in the Parser do not match the HardCoded tokens then Richmond will not be able to retrieve the tokens
            ' from the ParseText input from the user.
            'This isn't the falt of the user, this is a fault of using the ParserGenerator (TinyPG in Richmond's case) to set-up the Tokens.
            'i.e. The person setting up the Parser in TinyPG need be aware that 'Tokens' in TinyPG (when defining the ORMQL) need be the same
            ' as the Tokens in Richmond and as Richmond expects.
            'i.e. Establishing a Parser in TinyPG is actually a form of 'coding' (hard-coding), and where Richmond ostensibly has 2 parsers,
            '  VB.Net and TinyPG.
            '---------------------
            'Parameters
            'ao_object is of runtime generated type DynamicCollection.Entity
            '----------------------------------------------------------------------

            Dim loPropertyInfo() As System.Reflection.PropertyInfo = ao_object.GetType().GetProperties
            Dim loProperty As PropertyInfo
            Dim lr_bag As New Stack
            Dim loParseTreeNode As ORMDerivation.ParseNode
            Dim lrType As Type
            Try
                '--------------------------------------
                'Retrieve the list of required Tokens
                '--------------------------------------
                For Each loProperty In loPropertyInfo
                    lr_bag.Push(loProperty.Name)
                Next

                loParseTreeNode = aoParseTreeNode

                Dim lasListOfString As New List(Of String)

                If lr_bag.Contains(aoParseTreeNode.Token.Type.ToString) Then

                    lrType = ao_object.GetType.GetProperty(aoParseTreeNode.Token.Type.ToString).PropertyType
                    Dim piInstance As PropertyInfo = ao_object.GetType.GetProperty(aoParseTreeNode.Token.Type.ToString)

                    If lrType Is GetType(String) Then

                        piInstance.SetValue(ao_object, Trim(aoParseTreeNode.Token.Text))

                    ElseIf lrType Is lasListOfString.GetType Then


                        Dim liInstance As Object = ao_object.GetType.GetProperty(aoParseTreeNode.Token.Type.ToString).GetValue(ao_object)
                        Dim list As IList = CType(liInstance, IList)
                        list.Add(aoParseTreeNode.Token.Text)
                        ao_object.GetType.GetProperty(aoParseTreeNode.Token.Type.ToString).SetValue(ao_object, list, Nothing)

                    ElseIf lrType Is GetType(List(Of Object)) Then

                        Dim liInstance As Object = ao_object.GetType.GetProperty(aoParseTreeNode.Token.Type.ToString).GetValue(ao_object)
                        Dim list As IList = CType(liInstance, IList)
                        list.Add(aoParseTreeNode)
                        ao_object.GetType.GetProperty(aoParseTreeNode.Token.Type.ToString).SetValue(ao_object, list, Nothing)

                    ElseIf lrType Is GetType(Object) Then

                        Try
                            piInstance.SetValue(ao_object, aoParseTreeNode)
                        Catch ex As Exception
                            Call GetParseTreeTokensReflection(piInstance, loParseTreeNode)
                        End Try


                    ElseIf lrType.Name = "List`1" Then

                        Dim instance = Activator.CreateInstance(lrType.GenericTypeArguments(0))
                        Call GetParseTreeTokensReflection(instance, loParseTreeNode)
                        Dim liInstance As Object = ao_object.GetType.GetProperty(aoParseTreeNode.Token.Type.ToString).GetValue(ao_object)
                        Dim list As IList = CType(liInstance, IList)
                        list.Add(instance)
                        ao_object.GetType.GetProperty(aoParseTreeNode.Token.Type.ToString).SetValue(ao_object, list, Nothing)

                    Else
                        Dim instance = Activator.CreateInstance(lrType)
                        Call GetParseTreeTokensReflection(instance, loParseTreeNode)
                        piInstance.SetValue(ao_object, instance)
                    End If

                    'The matched property owns this ParseNode subtree. Its value or
                    'child object has already been populated above, so do not walk
                    'the same subtree again against the parent object.
                    Exit Sub

                End If

                For Each loParseTreeNode In aoParseTreeNode.Nodes.ToArray
                    Call GetParseTreeTokensReflection(ao_object, loParseTreeNode)
                Next

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()
                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub


        ''' <summary>
        ''' Generates a NORMA-compatible FBM derivation AST from freeform
        ''' derivation text, using the derived Fact Type's Boston model.
        ''' </summary>
        Public Function GenerateFBMDerivationRule(
            ByVal asDerivationText As String,
            ByRef arDerivedFactType As FBM.FactType) As FBM.DerivationRule

            Try
                If arDerivedFactType Is Nothing Then
                    Throw New Exception("No derived Fact Type was supplied.")
                End If

                Dim lrModel As FBM.Model = arDerivedFactType.Model
                Return GenerateFBMDerivationRule(asDerivationText,
                                                 lrModel,
                                                 arDerivedFactType)

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowMessage(lsMessage, pcenumErrorType.Warning, ex.StackTrace,,,,, True, ex)
                Return Nothing
            End Try

        End Function

        ''' <summary>
        ''' Generates a NORMA-compatible FBM derivation AST from freeform
        ''' derivation text. The first compiler slice accepts one continuous
        ''' biconditional RolePath. Unsupported logical branches, conditions
        ''' and calculations are rejected rather than inferred.
        ''' </summary>
        Public Function GenerateFBMDerivationRule(
            ByVal asDerivationText As String,
            ByRef arModel As FBM.Model,
            ByRef arDerivedFactType As FBM.FactType) As FBM.DerivationRule

            Try
                If arModel Is Nothing Then
                    Throw New Exception("No Boston model was supplied.")
                End If
                If arDerivedFactType Is Nothing Then
                    Throw New Exception("No derived Fact Type was supplied.")
                End If
                If String.IsNullOrWhiteSpace(asDerivationText) Then
                    Throw New Exception("Derivation text is required.")
                End If

                Dim lrStart As Derivations.Start =
                    ParseDerivationObjectTree(asDerivationText)
                Dim lrSentence As Derivations.DerivationSentence =
                    lrStart.DERIVATION.DERIVATION_SENTENCE

                If lrSentence Is Nothing OrElse
                   lrSentence.BICONDITIONAL_DERIVATION Is Nothing Then
                    Throw New Exception(
                        "The first FBM derivation compiler slice supports " &
                        "biconditional derivations only.")
                End If

                Dim lrBiconditional As Derivations.BiconditionalDerivation =
                    lrSentence.BICONDITIONAL_DERIVATION

                Dim lrCalculatedPlan As CalculatedWherePlan =
                    ExtractTrailingCalculatedWhere(
                        lrBiconditional.DERIVATION_EXPRESSION)
                Dim lbRootNegativeExistential As Boolean =
                    IsRootNegativeExistentialExpression(
                        lrBiconditional.DERIVATION_EXPRESSION)
                If Not lbRootNegativeExistential Then
                    ValidateInitialExpressionShape(
                        lrBiconditional.DERIVATION_EXPRESSION)
                End If

                Dim larHeadTokens As List(Of SurfaceToken) =
                    BuildHeadSurfaceTokens(
                        lrBiconditional.DERIVATION_HEAD_READING,
                        arModel)
                Dim lrHeadMatch As ReadingMatch =
                    BindDerivedHeadReading(arDerivedFactType,
                                           larHeadTokens)

                If lbRootNegativeExistential Then
                    Return BuildFBMRootNegativeExistentialDerivationRule(
                        arDerivedFactType,
                        lrHeadMatch,
                        lrBiconditional.DERIVATION_EXPRESSION,
                        arModel)
                End If

                Dim larSolutions As New List(Of LinearPathSolution)
                For Each lrClause As Derivations.DerivationClause In
                    lrBiconditional.DERIVATION_EXPRESSION.DERIVATION_CLAUSE

                    Dim larBodyTokens As List(Of SurfaceToken) =
                        BuildBodySurfaceTokens(lrClause.FACT_TYPE_PATH,
                                               arModel)

                    If larBodyTokens.Count = 0 OrElse
                       larBodyTokens(0).Kind <>
                           SurfaceTokenKind.ModelReference Then
                        Throw New Exception(
                            "Each RolePath branch must begin with an Object Type reference.")
                    End If

                    Dim lrSolution As LinearPathSolution =
                        BindLinearRolePath(larBodyTokens,
                                           0,
                                           larBodyTokens(0).ModelObject,
                                           larBodyTokens(0).BindingKey,
                                           arModel,
                                           True)

                    If lrSolution Is Nothing OrElse
                       lrSolution.Occurrences.Count = 0 Then
                        Throw New Exception(
                            "No complete maximal Fact Type Reading path matches: " &
                            SurfaceTokensText(larBodyTokens, 0))
                    End If
                    larSolutions.Add(lrSolution)
                Next

                Dim larConnectives As List(Of Derivations.LogicalConnective) =
                    lrBiconditional.DERIVATION_EXPRESSION.LOGICAL_CONNECTIVE.ToList()
                CollapseWhereNegatedClauses(larSolutions, larConnectives)

                If lrCalculatedPlan IsNot Nothing Then
                    If larConnectives.Any(
                        Function(x) Not String.IsNullOrWhiteSpace(x.KEYWD_WHERE_NEGATED)) Then
                        Throw New Exception(
                            "The first calculated projection compiler slice " &
                            "does not combine calculations with a negated " &
                            "'where' clause.")
                    End If

                    If larConnectives.Any(
                        Function(x) Not String.IsNullOrWhiteSpace(x.KEYWD_OR)) Then
                        Return BuildFBMScopedAndOrDerivationRule(
                            arDerivedFactType,
                            lrHeadMatch,
                            larSolutions,
                            larConnectives,
                            arModel,
                            lrCalculatedPlan)
                    End If

                    If larSolutions.Count = 1 Then
                        Return BuildFBMDerivationRule(arDerivedFactType,
                                                      lrHeadMatch,
                                                      larSolutions(0),
                                                      arModel,
                                                      lrCalculatedPlan)
                    End If

                    Return BuildFBMAndDerivationRule(arDerivedFactType,
                                                     lrHeadMatch,
                                                     larSolutions,
                                                     arModel,
                                                     lrCalculatedPlan)
                End If

                If larSolutions.Count = 1 Then
                    Return BuildFBMDerivationRule(arDerivedFactType,
                                                  lrHeadMatch,
                                                  larSolutions(0))
                End If

                If larConnectives.Any(Function(x) Not String.IsNullOrWhiteSpace(x.KEYWD_OR)) Then
                    Return BuildFBMAndOrDerivationRule(
                        arDerivedFactType,
                        lrHeadMatch,
                        larSolutions,
                        larConnectives)
                End If

                Return BuildFBMAndDerivationRule(arDerivedFactType,
                                                 lrHeadMatch,
                                                 larSolutions)

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowMessage(lsMessage, pcenumErrorType.Warning, ex.StackTrace,,,,, True, ex)
                Return Nothing
            End Try

        End Function

        Private Function ExtractTrailingCalculatedWhere(
            ByVal arExpression As Derivations.DerivationExpression) As CalculatedWherePlan

            If arExpression Is Nothing OrElse
               arExpression.LOGICAL_CONNECTIVE Is Nothing Then Return Nothing

            Dim larWhereIndexes As New List(Of Integer)
            For liIndex As Integer = 0 To arExpression.LOGICAL_CONNECTIVE.Count - 1
                Dim lrConnective As Derivations.LogicalConnective =
                    arExpression.LOGICAL_CONNECTIVE(liIndex)
                If lrConnective IsNot Nothing AndAlso
                   Not String.IsNullOrWhiteSpace(lrConnective.KEYWD_WHERE) AndAlso
                   liIndex + 1 < arExpression.DERIVATION_CLAUSE.Count AndAlso
                   IsCalculatedWhereClause(
                       arExpression.DERIVATION_CLAUSE(liIndex + 1)) Then
                    larWhereIndexes.Add(liIndex)
                End If
            Next

            If larWhereIndexes.Count = 0 Then Return Nothing
            If larWhereIndexes.Count > 1 Then
                Throw New Exception(
                    "The first calculated 'where' compiler slice supports " &
                    "one calculated clause only.")
            End If

            Dim liWhereIndex As Integer = larWhereIndexes(0)
            If liWhereIndex <> arExpression.LOGICAL_CONNECTIVE.Count - 1 OrElse
               liWhereIndex + 1 <> arExpression.DERIVATION_CLAUSE.Count - 1 Then
                Throw New Exception(
                    "A calculated 'where' clause must be the final clause " &
                    "of the derivation expression.")
            End If

            Dim lrClause As Derivations.DerivationClause =
                arExpression.DERIVATION_CLAUSE(liWhereIndex + 1)
            If lrClause Is Nothing OrElse lrClause.FACT_TYPE_PATH Is Nothing Then
                Throw New Exception("The calculated 'where' clause is empty.")
            End If

            Dim larComponents As List(Of Derivations.FactTypePathComponent) =
                lrClause.FACT_TYPE_PATH.FACT_TYPE_PATH_COMPONENT
            If larComponents Is Nothing OrElse larComponents.Count < 3 OrElse
               larComponents(0) Is Nothing OrElse
               larComponents(1) Is Nothing OrElse
               larComponents(0).MODEL_ELEMENT_REFERENCE Is Nothing OrElse
               Not IsSupportedComparisonOperator(
                   larComponents(1).COMPARISON_OPERATOR) Then
                Throw New Exception(
                    "The calculated condition must begin with " &
                    "'<Object Type> <comparison operator>'.")
            End If

            Dim lrPlan As New CalculatedWherePlan With {
                .TargetBindingKey = ModelReferenceBindingKey(
                    larComponents(0).MODEL_ELEMENT_REFERENCE),
                .ComparisonOperatorSymbol =
                    larComponents(1).COMPARISON_OPERATOR.Trim(),
                .ScopeClauseIndex = liWhereIndex
            }

            If larComponents.Count = 5 AndAlso
               IsArithmeticValueComponent(larComponents(2)) AndAlso
               Not String.IsNullOrWhiteSpace(
                   larComponents(3).ARITHMETIC_OPERATOR) AndAlso
               IsArithmeticValueComponent(larComponents(4)) Then

                Dim lsOperatorSymbol As String =
                    larComponents(3).ARITHMETIC_OPERATOR.Trim()
                lrPlan.Expression = New CalculatedExpressionPlan With {
                    .FunctionName = ArithmeticFunctionName(lsOperatorSymbol),
                    .OperatorSymbol = lsOperatorSymbol
                }
                lrPlan.Expression.Arguments.Add(
                    BuildArithmeticArgumentPlan(larComponents(2)))
                lrPlan.Expression.Arguments.Add(
                    BuildArithmeticArgumentPlan(larComponents(4)))

            ElseIf larComponents.Count = 3 AndAlso
                   larComponents(2).FUNCTION_CALL IsNot Nothing Then

                lrPlan.Expression = BuildFunctionExpressionPlan(
                    larComponents(2).FUNCTION_CALL)
            ElseIf larComponents.Count = 3 AndAlso
                   IsArithmeticValueComponent(larComponents(2)) Then

                lrPlan.RightArgument =
                    BuildArithmeticArgumentPlan(larComponents(2))
            Else
                Throw New Exception(
                    "The calculated expression compiler supports " &
                    "'<derived value> = <value> <arithmetic operator> <value>' and " &
                    "'<Object Type> <comparison operator> <value or function call>'.")
            End If

            arExpression.DERIVATION_CLAUSE.RemoveAt(liWhereIndex + 1)
            arExpression.LOGICAL_CONNECTIVE.RemoveAt(liWhereIndex)
            Return lrPlan

        End Function

        Private Function IsCalculatedWhereClause(
            ByVal arClause As Derivations.DerivationClause) As Boolean

            If arClause Is Nothing OrElse arClause.FACT_TYPE_PATH Is Nothing Then
                Return False
            End If

            Dim larComponents As List(Of Derivations.FactTypePathComponent) =
                arClause.FACT_TYPE_PATH.FACT_TYPE_PATH_COMPONENT
            Return larComponents IsNot Nothing AndAlso
                   larComponents.Count >= 2 AndAlso
                   larComponents(0) IsNot Nothing AndAlso
                   larComponents(1) IsNot Nothing AndAlso
                   larComponents(0).MODEL_ELEMENT_REFERENCE IsNot Nothing AndAlso
                   IsSupportedComparisonOperator(
                       larComponents(1).COMPARISON_OPERATOR)

        End Function

        Private Function IsSupportedComparisonOperator(
            ByVal asOperatorSymbol As String) As Boolean

            Select Case If(asOperatorSymbol, "").Trim()
                Case "=", "<>", ">", ">=", "<", "<="
                    Return True
            End Select
            Return False

        End Function

        Private Function ComparisonFunctionName(
            ByVal asOperatorSymbol As String) As String

            Select Case asOperatorSymbol
                Case "="
                    Return "equals"
                Case "<>"
                    Return "notEquals"
                Case ">"
                    Return "greaterThan"
                Case ">="
                    Return "greaterThanOrEqual"
                Case "<"
                    Return "lessThan"
                Case "<="
                    Return "lessThanOrEqual"
            End Select

            Throw New Exception(
                "Comparison operator '" & asOperatorSymbol & "' is not supported.")

        End Function

        Private Function IsRootNegativeExistentialExpression(
            ByVal arExpression As Derivations.DerivationExpression) As Boolean

            If arExpression Is Nothing OrElse
               arExpression.DERIVATION_CLAUSE.Count <> 2 OrElse
               arExpression.LOGICAL_CONNECTIVE.Count <> 1 Then Return False

            Dim lrConnective As Derivations.LogicalConnective =
                arExpression.LOGICAL_CONNECTIVE(0)
            If lrConnective Is Nothing OrElse
               String.IsNullOrWhiteSpace(lrConnective.KEYWD_WHERE) Then Return False

            Dim lrExistentialClause As Derivations.DerivationClause =
                arExpression.DERIVATION_CLAUSE(0)
            If lrExistentialClause Is Nothing OrElse
               lrExistentialClause.FACT_TYPE_PATH Is Nothing Then Return False

            Dim larComponents As List(Of Derivations.FactTypePathComponent) =
                lrExistentialClause.FACT_TYPE_PATH.FACT_TYPE_PATH_COMPONENT
            Return larComponents IsNot Nothing AndAlso
                   larComponents.Count = 3 AndAlso
                   larComponents(0) IsNot Nothing AndAlso
                   larComponents(1) IsNot Nothing AndAlso
                   larComponents(2) IsNot Nothing AndAlso
                   Not String.IsNullOrWhiteSpace(larComponents(0).KEYWD_NO) AndAlso
                   larComponents(1).MODEL_ELEMENT_REFERENCE IsNot Nothing AndAlso
                   String.Equals(larComponents(2).PREDICATE_WORD.Trim(),
                                 "exists",
                                 StringComparison.OrdinalIgnoreCase)

        End Function

        Private Function BuildFBMRootNegativeExistentialDerivationRule(
            ByVal arDerivedFactType As FBM.FactType,
            ByVal arHeadMatch As ReadingMatch,
            ByVal arExpression As Derivations.DerivationExpression,
            ByVal arModel As FBM.Model) As FBM.DerivationRule

            Dim larExistentialComponents As List(
                Of Derivations.FactTypePathComponent) =
                arExpression.DERIVATION_CLAUSE(0).
                    FACT_TYPE_PATH.FACT_TYPE_PATH_COMPONENT
            Dim lrExistentialReference As Derivations.ModelElementReference =
                larExistentialComponents(1).MODEL_ELEMENT_REFERENCE
            Dim lrExistentialObject As FBM.ModelObject =
                ResolveObjectType(arModel,
                                  lrExistentialReference.MODEL_ELEMENT_NAME)
            Dim lsExistentialBindingKey As String =
                ModelReferenceBindingKey(lrExistentialReference)

            Dim larRelationTokens As List(Of SurfaceToken) =
                BuildBodySurfaceTokens(
                    arExpression.DERIVATION_CLAUSE(1).FACT_TYPE_PATH,
                    arModel)
            If larRelationTokens.Count = 0 OrElse
               larRelationTokens(0).Kind <> SurfaceTokenKind.ModelReference Then
                Throw New Exception(
                    "The negative existential restriction must begin with an Object Type reference.")
            End If

            Dim lrForwardSolution As LinearPathSolution =
                BindLinearRolePath(larRelationTokens,
                                   0,
                                   larRelationTokens(0).ModelObject,
                                   larRelationTokens(0).BindingKey,
                                   arModel,
                                   True)
            If lrForwardSolution Is Nothing OrElse
               lrForwardSolution.Occurrences.Count = 0 Then
                Throw New Exception(
                    "No complete RolePath connects the negative existential restriction: " &
                    SurfaceTokensText(larRelationTokens, 0))
            End If

            If Not lrForwardSolution.Occurrences.Any(
                Function(x) x.BindingKeys.Any(
                    Function(y) String.Equals(y,
                                              lsExistentialBindingKey,
                                              StringComparison.OrdinalIgnoreCase))) Then
                Throw New Exception(
                    "The negative existential Object Type '" &
                    lrExistentialObject.Id &
                    "' is not bound by its relational 'where' clause.")
            End If

            Dim lrReversedSolution As LinearPathSolution =
                ReverseLinearPathSolution(lrForwardSolution)
            Dim lrFirstReversedMatch As ReadingMatch =
                lrReversedSolution.Occurrences(0)
            If Not String.Equals(lrFirstReversedMatch.BindingKeys(0),
                                 lsExistentialBindingKey,
                                 StringComparison.OrdinalIgnoreCase) Then
                Throw New Exception(
                    "The relational 'where' path does not terminate at the " &
                    "negative existential Object Type.")
            End If

            Dim lrMainRootObject As FBM.ModelObject =
                lrForwardSolution.Occurrences(0).Roles(0).JoinedORMObject
            Dim lsMainRootBindingKey As String =
                lrForwardSolution.Occurrences(0).BindingKeys(0)
            Dim lrRule As New FBM.DerivationRule
            Dim lrPath As New FBM.FactTypeDerivationPath With {
                .Id = NewDerivationId(),
                .Name = arDerivedFactType.Id,
                .PathComponents = New FBM.PathComponents
            }
            Dim lrRolePath As New FBM.RolePath With {
                .Id = NewDerivationId()
            }
            Dim lrMainRoot As New FBM.RootObjectType With {
                .id = NewDerivationId(),
                .ref = lrMainRootObject.Id,
                .BostonModelElement = lrMainRootObject,
                .IsNegated = False
            }
            lrRolePath.RootObjectType = lrMainRoot

            Dim ldictOccurrences As New Dictionary(
                Of String, List(Of BindingOccurrence))(
                StringComparer.OrdinalIgnoreCase)
            AddBindingOccurrence(ldictOccurrences,
                                 lsMainRootBindingKey,
                                 New BindingOccurrence With {
                                     .IsPathRoot = True
                                 })

            Dim lrSubPath As New FBM.RoleSubPath With {
                .Id = NewDerivationId(),
                .RootObjectType = New FBM.RootObjectType With {
                    .id = NewDerivationId(),
                    .ref = lrExistentialObject.Id,
                    .BostonModelElement = lrExistentialObject,
                    .IsNegated = True
                }
            }
            Dim liPathIndex As Integer = 0
            AddSolutionToSubPath(lrReversedSolution,
                                 lrSubPath,
                                 lrExistentialObject,
                                 lsExistentialBindingKey,
                                 0,
                                 liPathIndex,
                                 ldictOccurrences)
            lrRolePath.SubPath.Add(lrSubPath)

            AddRequiredObjectUnifiers(lrRolePath,
                                      lrMainRoot,
                                      ldictOccurrences)
            lrPath.PathComponents.RolePaths.Add(lrRolePath)
            AddDerivationProjection(lrPath,
                                    lrRolePath,
                                    lrMainRoot,
                                    arHeadMatch,
                                    ldictOccurrences)
            lrRule.FactTypeDerivationPath = lrPath
            Return lrRule

        End Function

        Private Function ReverseLinearPathSolution(
            ByVal arSolution As LinearPathSolution) As LinearPathSolution

            Dim lrReversed As New LinearPathSolution
            For liOriginalOccurrence As Integer =
                arSolution.Occurrences.Count - 1 To 0 Step -1

                Dim lrOriginal As ReadingMatch =
                    arSolution.Occurrences(liOriginalOccurrence)
                Dim lrMatch As New ReadingMatch With {
                    .Reading = lrOriginal.Reading,
                    .IsNegated = lrOriginal.IsNegated
                }
                For liRole As Integer = lrOriginal.Roles.Count - 1 To 0 Step -1
                    lrMatch.Roles.Add(lrOriginal.Roles(liRole))
                    lrMatch.BindingKeys.Add(lrOriginal.BindingKeys(liRole))
                Next
                lrReversed.Occurrences.Add(lrMatch)

                If arSolution.NegatedOccurrenceIndexes.Contains(
                    liOriginalOccurrence) Then
                    lrReversed.NegatedOccurrenceIndexes.Add(
                        arSolution.Occurrences.Count - 1 - liOriginalOccurrence)
                End If
            Next

            Return lrReversed

        End Function

        Private Function IsArithmeticValueComponent(
            ByVal arComponent As Derivations.FactTypePathComponent) As Boolean

            Return arComponent IsNot Nothing AndAlso
                   (arComponent.MODEL_ELEMENT_REFERENCE IsNot Nothing OrElse
                    Not String.IsNullOrWhiteSpace(arComponent.NUMBER_LITERAL) OrElse
                    arComponent.PARENTHESISED_DERIVATION_EXPRESSION IsNot Nothing)

        End Function

        Private Function BuildArithmeticArgumentPlan(
            ByVal arComponent As Derivations.FactTypePathComponent) As CalculatedArgumentPlan

            If arComponent.MODEL_ELEMENT_REFERENCE IsNot Nothing Then
                Return New CalculatedArgumentPlan With {
                    .Kind = CalculatedArgumentKind.Binding,
                    .BindingKey = ModelReferenceBindingKey(
                        arComponent.MODEL_ELEMENT_REFERENCE)
                }
            End If

            If Not String.IsNullOrWhiteSpace(arComponent.NUMBER_LITERAL) Then
                Return New CalculatedArgumentPlan With {
                    .Kind = CalculatedArgumentKind.Constant,
                    .ConstantValue = arComponent.NUMBER_LITERAL.Trim()
                }
            End If

            If arComponent.PARENTHESISED_DERIVATION_EXPRESSION IsNot Nothing Then
                Dim lrParenthesised As Derivations.ParenthesisedDerivationExpression =
                    arComponent.PARENTHESISED_DERIVATION_EXPRESSION
                If lrParenthesised.DERIVATION_EXPRESSION Is Nothing Then
                    Throw New Exception("The parenthesised arithmetic expression is empty.")
                End If

                Return New CalculatedArgumentPlan With {
                    .Kind = CalculatedArgumentKind.FunctionValue,
                    .FunctionExpression = BuildArithmeticExpressionPlan(
                        lrParenthesised.DERIVATION_EXPRESSION)
                }
            End If

            Throw New Exception("The arithmetic operand is not supported.")

        End Function

        Private Function BuildArithmeticExpressionPlan(
            ByVal arExpression As Derivations.DerivationExpression) As CalculatedExpressionPlan

            If arExpression Is Nothing OrElse
               arExpression.DERIVATION_CLAUSE.Count <> 1 OrElse
               arExpression.LOGICAL_CONNECTIVE.Count <> 0 Then
                Throw New Exception(
                    "A parenthesised arithmetic expression must contain one expression.")
            End If

            Dim lrClause As Derivations.DerivationClause =
                arExpression.DERIVATION_CLAUSE(0)
            If lrClause Is Nothing OrElse
               Not String.IsNullOrWhiteSpace(lrClause.KEYWD_NEGATED) OrElse
               lrClause.FACT_TYPE_PATH Is Nothing Then
                Throw New Exception(
                    "The parenthesised arithmetic expression is not supported.")
            End If

            Dim larComponents As List(Of Derivations.FactTypePathComponent) =
                lrClause.FACT_TYPE_PATH.FACT_TYPE_PATH_COMPONENT
            If larComponents.Count <> 3 OrElse
               Not IsArithmeticValueComponent(larComponents(0)) OrElse
               String.IsNullOrWhiteSpace(
                   larComponents(1).ARITHMETIC_OPERATOR) OrElse
               Not IsArithmeticValueComponent(larComponents(2)) Then
                Throw New Exception(
                    "A parenthesised arithmetic expression must have the form " &
                    "'(<value> <arithmetic operator> <value>)'.")
            End If

            Dim lsOperatorSymbol As String =
                larComponents(1).ARITHMETIC_OPERATOR.Trim()
            Dim lrPlan As New CalculatedExpressionPlan With {
                .FunctionName = ArithmeticFunctionName(lsOperatorSymbol),
                .OperatorSymbol = lsOperatorSymbol
            }
            lrPlan.Arguments.Add(
                BuildArithmeticArgumentPlan(larComponents(0)))
            lrPlan.Arguments.Add(
                BuildArithmeticArgumentPlan(larComponents(2)))
            Return lrPlan

        End Function

        Private Function ArithmeticFunctionName(
            ByVal asOperatorSymbol As String) As String

            Select Case asOperatorSymbol
                Case "+"
                    Return "add"
                Case "-"
                    Return "subtract"
                Case "*"
                    Return "multiply"
                Case "/"
                    Return "divide"
            End Select

            Throw New Exception(
                "Arithmetic operator '" & asOperatorSymbol & "' is not supported.")

        End Function

        Private Function BuildFunctionExpressionPlan(
            ByVal arFunctionCall As Derivations.FunctionCall) As CalculatedExpressionPlan

            If arFunctionCall Is Nothing OrElse
               String.IsNullOrWhiteSpace(arFunctionCall.FUNCTION_NAME) Then
                Throw New Exception("A calculated function call has no function name.")
            End If

            Dim lrPlan As New CalculatedExpressionPlan With {
                .FunctionName = arFunctionCall.FUNCTION_NAME.Trim()
            }
            Dim larArguments As List(Of List(Of Derivations.FactTypePathComponent)) =
                GetFunctionArgumentComponents(arFunctionCall)

            For Each larArgument As List(Of Derivations.FactTypePathComponent) In larArguments
                If IsAggregateArgument(larArgument) Then
                    If larArguments.Count <> 1 Then
                        Throw New Exception(
                            "An 'each <Object Type> for that <Object Type>' " &
                            "aggregate must be the function's only argument.")
                    End If

                    lrPlan.Arguments.Add(
                        New CalculatedArgumentPlan With {
                            .Kind = CalculatedArgumentKind.Binding,
                            .BindingKey = ModelReferenceBindingKey(
                                larArgument(1).MODEL_ELEMENT_REFERENCE),
                            .IsBagInput = True
                        })
                    lrPlan.AggregationContextBindingKey =
                        ModelReferenceBindingKey(
                            larArgument(4).MODEL_ELEMENT_REFERENCE)
                Else
                    lrPlan.Arguments.Add(
                        BuildOrdinaryFunctionArgumentPlan(larArgument))
                End If
            Next

            Return lrPlan

        End Function

        Private Function GetFunctionArgumentComponents(
            ByVal arFunctionCall As Derivations.FunctionCall) As List(
                Of List(Of Derivations.FactTypePathComponent))

            Dim larResult As New List(
                Of List(Of Derivations.FactTypePathComponent))
            If arFunctionCall.FUNCTION_ARGUMENT_LIST Is Nothing Then Return larResult

            For Each lrExpression As Derivations.DerivationExpression In
                arFunctionCall.FUNCTION_ARGUMENT_LIST.DERIVATION_EXPRESSION

                If lrExpression Is Nothing OrElse
                   lrExpression.DERIVATION_CLAUSE.Count <> 1 OrElse
                   lrExpression.LOGICAL_CONNECTIVE.Count <> 0 OrElse
                   lrExpression.DERIVATION_CLAUSE(0).FACT_TYPE_PATH Is Nothing Then
                    Throw New Exception(
                        "Function arguments must be calculated values or Object Type references.")
                End If

                Dim larCurrent As New List(Of Derivations.FactTypePathComponent)
                For Each lrComponent As Derivations.FactTypePathComponent In
                    lrExpression.DERIVATION_CLAUSE(0).FACT_TYPE_PATH.FACT_TYPE_PATH_COMPONENT

                    If Not String.IsNullOrWhiteSpace(lrComponent.COMMA_SEPARATOR) Then
                        If larCurrent.Count = 0 Then
                            Throw New Exception("A function argument is empty.")
                        End If
                        larResult.Add(larCurrent)
                        larCurrent = New List(Of Derivations.FactTypePathComponent)
                    Else
                        larCurrent.Add(lrComponent)
                    End If
                Next

                If larCurrent.Count > 0 Then larResult.Add(larCurrent)
            Next

            Return larResult

        End Function

        Private Function IsAggregateArgument(
            ByVal aarComponents As List(
                Of Derivations.FactTypePathComponent)) As Boolean

            Return aarComponents IsNot Nothing AndAlso
                   aarComponents.Count = 5 AndAlso
                   Not String.IsNullOrWhiteSpace(aarComponents(0).KEYWD_EACH) AndAlso
                   aarComponents(1).MODEL_ELEMENT_REFERENCE IsNot Nothing AndAlso
                   String.Equals(aarComponents(2).PREDICATE_WORD.Trim(),
                                 "for",
                                 StringComparison.OrdinalIgnoreCase) AndAlso
                   Not String.IsNullOrWhiteSpace(aarComponents(3).KEYWD_THAT) AndAlso
                   aarComponents(4).MODEL_ELEMENT_REFERENCE IsNot Nothing

        End Function

        Private Function BuildOrdinaryFunctionArgumentPlan(
            ByVal aarComponents As List(
                Of Derivations.FactTypePathComponent)) As CalculatedArgumentPlan

            If aarComponents Is Nothing OrElse aarComponents.Count <> 1 Then
                Throw New Exception(
                    "A function argument must be an Object Type reference, " &
                    "literal, or nested function call.")
            End If

            Dim lrComponent As Derivations.FactTypePathComponent = aarComponents(0)
            If lrComponent.MODEL_ELEMENT_REFERENCE IsNot Nothing Then
                Return New CalculatedArgumentPlan With {
                    .Kind = CalculatedArgumentKind.Binding,
                    .BindingKey = ModelReferenceBindingKey(
                        lrComponent.MODEL_ELEMENT_REFERENCE)
                }
            End If
            If lrComponent.FUNCTION_CALL IsNot Nothing Then
                Return New CalculatedArgumentPlan With {
                    .Kind = CalculatedArgumentKind.FunctionValue,
                    .FunctionExpression = BuildFunctionExpressionPlan(
                        lrComponent.FUNCTION_CALL)
                }
            End If
            If Not String.IsNullOrWhiteSpace(lrComponent.TEXT_LITERAL) Then
                Return New CalculatedArgumentPlan With {
                    .Kind = CalculatedArgumentKind.Constant,
                    .ConstantValue = lrComponent.TEXT_LITERAL.Trim()
                }
            End If
            If Not String.IsNullOrWhiteSpace(lrComponent.NUMBER_LITERAL) Then
                Return New CalculatedArgumentPlan With {
                    .Kind = CalculatedArgumentKind.Constant,
                    .ConstantValue = lrComponent.NUMBER_LITERAL.Trim()
                }
            End If

            Throw New Exception("The function argument is not supported.")

        End Function

        Private Function ParseDerivationObjectTree(
            ByVal asDerivationText As String) As Derivations.Start

            Dim lrParser As New ORMDerivation.Parser(
                New ORMDerivation.Scanner)
            Dim lrParseTree As ORMDerivation.ParseTree =
                lrParser.Parse(asDerivationText)

            If lrParseTree Is Nothing Then
                Throw New Exception(
                    "TinyPG returned no derivation ParseTree.")
            End If

            If lrParseTree.Errors IsNot Nothing AndAlso
               lrParseTree.Errors.Count > 0 Then
                Dim lrBuilder As New StringBuilder
                For Each lrError As ORMDerivation.ParseError In
                    lrParseTree.Errors
                    If lrBuilder.Length > 0 Then lrBuilder.Append(" ")
                    lrBuilder.Append(lrError.Message)
                Next
                Throw New Exception(
                    "The derivation could not be parsed. " &
                    lrBuilder.ToString())
            End If

            Dim lrStart As New Derivations.Start
            Dim loStart As Object = lrStart
            Dim lrRootNode As ORMDerivation.ParseNode = lrParseTree
            GetParseTreeTokensReflection(loStart,
                                         lrRootNode)
            lrStart = DirectCast(loStart, Derivations.Start)

            If lrStart.DERIVATION Is Nothing OrElse
               lrStart.DERIVATION.DERIVATION_SENTENCE Is Nothing Then
                Throw New Exception(
                    "The ParseTree did not populate the Derivations object graph.")
            End If

            Return lrStart

        End Function

        Private Sub ValidateInitialExpressionShape(
            ByVal arExpression As Derivations.DerivationExpression)

            If arExpression Is Nothing Then
                Throw New Exception(
                    "The biconditional derivation has no body expression.")
            End If
            If arExpression.DERIVATION_CLAUSE Is Nothing OrElse
               arExpression.DERIVATION_CLAUSE.Count = 0 Then
                Throw New Exception(
                    "The biconditional derivation has no derivation clause.")
            End If

            Dim liRequiredConnectives As Integer =
                arExpression.DERIVATION_CLAUSE.Count - 1
            Dim liActualConnectives As Integer =
                If(arExpression.LOGICAL_CONNECTIVE Is Nothing,
                   0,
                   arExpression.LOGICAL_CONNECTIVE.Count)
            If liActualConnectives <> liRequiredConnectives Then
                Throw New Exception(
                    "The derivation clause/connective sequence is incomplete.")
            End If

            If arExpression.LOGICAL_CONNECTIVE IsNot Nothing Then
                For Each lrConnective As Derivations.LogicalConnective In
                    arExpression.LOGICAL_CONNECTIVE
                    If lrConnective Is Nothing OrElse
                       (String.IsNullOrWhiteSpace(lrConnective.KEYWD_AND) AndAlso
                        String.IsNullOrWhiteSpace(lrConnective.KEYWD_OR) AndAlso
                        String.IsNullOrWhiteSpace(lrConnective.KEYWD_WHERE_NEGATED)) OrElse
                       Not String.IsNullOrWhiteSpace(lrConnective.KEYWD_WHERE) Then
                        Throw New Exception(
                            "The current FBM derivation compiler slice supports " &
                            "'and', 'or', and negated 'where' clauses only.")
                    End If
                Next
            End If

            For Each lrClause As Derivations.DerivationClause In
                arExpression.DERIVATION_CLAUSE
                If lrClause Is Nothing OrElse
                   lrClause.FACT_TYPE_PATH Is Nothing Then
                    Throw New Exception(
                        "A derivation clause has no Fact Type Path.")
                End If
                If Not String.IsNullOrWhiteSpace(
                    lrClause.KEYWD_NEGATED) Then
                    Throw New Exception(
                        "Negated derivation clauses are not yet supported by " &
                        "the current FBM derivation compiler slice.")
                End If
            Next

        End Sub

        Private Function BuildHeadSurfaceTokens(
            ByVal arHead As Derivations.DerivationHeadReading,
            ByVal arModel As FBM.Model) As List(Of SurfaceToken)

            If arHead Is Nothing OrElse
               arHead.MODEL_ELEMENT_REFERENCE Is Nothing Then
                Throw New Exception(
                    "The derivation head has no leading Object Type reference.")
            End If

            Dim larTokens As New List(Of SurfaceToken)
            AddModelReferenceToken(larTokens,
                                   arHead.MODEL_ELEMENT_REFERENCE,
                                   arModel)

            If arHead.FACT_TYPE_PATH_COMPONENT IsNot Nothing Then
                For Each lrComponent As Derivations.FactTypePathComponent In
                    arHead.FACT_TYPE_PATH_COMPONENT
                    AddComponentSurfaceTokens(larTokens,
                                              lrComponent,
                                              arModel,
                                              False)
                Next
            End If

            Return larTokens

        End Function

        Private Function BuildBodySurfaceTokens(
            ByVal arPath As Derivations.FactTypePath,
            ByVal arModel As FBM.Model) As List(Of SurfaceToken)

            If arPath Is Nothing OrElse
               arPath.FACT_TYPE_PATH_COMPONENT Is Nothing Then
                Throw New Exception(
                    "The derivation body has no Fact Type Path components.")
            End If

            Dim larTokens As New List(Of SurfaceToken)
            Dim lbNegateNextModelReference As Boolean = False
            For Each lrComponent As Derivations.FactTypePathComponent In
                arPath.FACT_TYPE_PATH_COMPONENT

                If lrComponent Is Nothing Then Continue For

                If Not String.IsNullOrWhiteSpace(lrComponent.KEYWD_NO) Then
                    If lbNegateNextModelReference Then
                        Throw New Exception(
                            "A 'no' quantifier has no intervening Object Type reference.")
                    End If
                    lbNegateNextModelReference = True
                    Continue For
                End If

                Dim liTokenCountBefore As Integer = larTokens.Count
                AddComponentSurfaceTokens(larTokens,
                                          lrComponent,
                                          arModel,
                                          True)
                If lbNegateNextModelReference Then
                    Dim lrNewModelToken As SurfaceToken =
                        larTokens.Skip(liTokenCountBefore).
                            FirstOrDefault(
                                Function(x) x.Kind =
                                    SurfaceTokenKind.ModelReference)
                    If lrNewModelToken IsNot Nothing Then
                        lrNewModelToken.IsNegativeQuantifier = True
                        lbNegateNextModelReference = False
                    End If
                End If
            Next

            If lbNegateNextModelReference Then
                Throw New Exception(
                    "The 'no' quantifier is not followed by an Object Type reference.")
            End If
            Return larTokens

        End Function

        Private Sub AddComponentSurfaceTokens(
            ByVal aarTokens As List(Of SurfaceToken),
            ByVal arComponent As Derivations.FactTypePathComponent,
            ByVal arModel As FBM.Model,
            ByVal abAllowRolePathQuantifiers As Boolean)

            If arComponent Is Nothing Then Exit Sub

            If arComponent.MODEL_ELEMENT_REFERENCE IsNot Nothing Then
                AddModelReferenceToken(aarTokens,
                                       arComponent.MODEL_ELEMENT_REFERENCE,
                                       arModel)
                Exit Sub
            End If

            If Not String.IsNullOrWhiteSpace(
                arComponent.PREDICATE_WORD) Then
                AddLiteralSurfaceTokens(aarTokens,
                                        arComponent.PREDICATE_WORD)
                Exit Sub
            End If

            If abAllowRolePathQuantifiers AndAlso
               (Not String.IsNullOrWhiteSpace(
                    arComponent.KEYWD_THAT) OrElse
                Not String.IsNullOrWhiteSpace(
                    arComponent.KEYWD_SOME)) Then
                'Variable identity in the RolePath lets the renderer derive
                '"that" versus "some"; these words are not FBM AST nodes.
                Exit Sub
            End If

            Dim lsUnsupported As String =
                FirstUnsupportedComponentText(arComponent)
            If lsUnsupported <> "" Then
                Throw New Exception(
                    "The first FBM derivation compiler slice does not yet " &
                    "support path component '" & lsUnsupported & "'.")
            End If

            Throw New Exception(
                "An empty Fact Type Path component was encountered.")

        End Sub

        Private Function FirstUnsupportedComponentText(
            ByVal arComponent As Derivations.FactTypePathComponent) As String

            If Not String.IsNullOrWhiteSpace(arComponent.KEYWD_FOR_SOME) Then Return arComponent.KEYWD_FOR_SOME
            If Not String.IsNullOrWhiteSpace(arComponent.KEYWD_EACH) Then Return arComponent.KEYWD_EACH
            If Not String.IsNullOrWhiteSpace(arComponent.KEYWD_NO) Then Return arComponent.KEYWD_NO
            If Not String.IsNullOrWhiteSpace(arComponent.KEYWD_POSSIBLE_VALUES_OF) Then Return arComponent.KEYWD_POSSIBLE_VALUES_OF
            If Not String.IsNullOrWhiteSpace(arComponent.KEYWD_BY_DEFINITION) Then Return arComponent.KEYWD_BY_DEFINITION
            If Not String.IsNullOrWhiteSpace(arComponent.KEYWD_AT_LEAST) Then Return arComponent.KEYWD_AT_LEAST
            If Not String.IsNullOrWhiteSpace(arComponent.KEYWD_AT_MOST) Then Return arComponent.KEYWD_AT_MOST
            If arComponent.FUNCTION_CALL IsNot Nothing Then Return "function call"
            If arComponent.PARENTHESISED_DERIVATION_EXPRESSION IsNot Nothing Then Return "parenthesised derivation expression"
            If Not String.IsNullOrWhiteSpace(arComponent.COMPARISON_OPERATOR) Then Return arComponent.COMPARISON_OPERATOR
            If Not String.IsNullOrWhiteSpace(arComponent.ARITHMETIC_OPERATOR) Then Return arComponent.ARITHMETIC_OPERATOR
            If Not String.IsNullOrWhiteSpace(arComponent.NUMBER_LITERAL) Then Return arComponent.NUMBER_LITERAL
            If Not String.IsNullOrWhiteSpace(arComponent.TEXT_LITERAL) Then Return arComponent.TEXT_LITERAL
            If Not String.IsNullOrWhiteSpace(arComponent.COMMA_SEPARATOR) Then Return arComponent.COMMA_SEPARATOR
            If Not String.IsNullOrWhiteSpace(arComponent.KEYWD_THAT) Then Return arComponent.KEYWD_THAT
            If Not String.IsNullOrWhiteSpace(arComponent.KEYWD_SOME) Then Return arComponent.KEYWD_SOME
            Return ""

        End Function

        Private Sub AddModelReferenceToken(
            ByVal aarTokens As List(Of SurfaceToken),
            ByVal arReference As Derivations.ModelElementReference,
            ByVal arModel As FBM.Model)

            If arReference Is Nothing OrElse
               String.IsNullOrWhiteSpace(
                   arReference.MODEL_ELEMENT_NAME) Then
                Throw New Exception(
                    "A Fact Type Path contains an empty Object Type reference.")
            End If

            Dim lrModelObject As FBM.ModelObject =
                ResolveObjectType(arModel,
                                  arReference.MODEL_ELEMENT_NAME)

            aarTokens.Add(New SurfaceToken With {
                .Kind = SurfaceTokenKind.ModelReference,
                .Text = lrModelObject.Id,
                .ModelReference = arReference,
                .ModelObject = lrModelObject,
                .BindingKey = ModelReferenceBindingKey(arReference)
            })

        End Sub

        Private Sub AddLiteralSurfaceTokens(
            ByVal aarTokens As List(Of SurfaceToken),
            ByVal asText As String)

            If String.IsNullOrWhiteSpace(asText) Then Exit Sub
            For Each lrMatch As Match In Regex.Matches(asText, "\S+")
                aarTokens.Add(New SurfaceToken With {
                    .Kind = SurfaceTokenKind.Literal,
                    .Text = NormaliseSurfaceToken(lrMatch.Value)
                })
            Next

        End Sub

        Private Function ResolveObjectType(
            ByVal arModel As FBM.Model,
            ByVal asObjectTypeId As String) As FBM.ModelObject

            Dim larCandidates As New List(Of FBM.ModelObject)

            If arModel.EntityType IsNot Nothing Then
                larCandidates.AddRange(
                    arModel.EntityType.
                        Where(Function(x) x IsNot Nothing AndAlso
                            String.Equals(x.Id,
                                          asObjectTypeId,
                                          StringComparison.OrdinalIgnoreCase)).
                        Cast(Of FBM.ModelObject)())
            End If
            If arModel.ValueType IsNot Nothing Then
                larCandidates.AddRange(
                    arModel.ValueType.
                        Where(Function(x) x IsNot Nothing AndAlso
                            String.Equals(x.Id,
                                          asObjectTypeId,
                                          StringComparison.OrdinalIgnoreCase)).
                        Cast(Of FBM.ModelObject)())
            End If
            If arModel.FactType IsNot Nothing Then
                larCandidates.AddRange(
                    arModel.FactType.
                        Where(Function(x) x IsNot Nothing AndAlso
                            String.Equals(x.Id,
                                          asObjectTypeId,
                                          StringComparison.OrdinalIgnoreCase)).
                        Cast(Of FBM.ModelObject)())
            End If

            If larCandidates.Count = 0 Then
                Throw New Exception(
                    "Object Type '" & asObjectTypeId &
                    "' does not exist in the supplied Boston model.")
            End If
            If larCandidates.Count > 1 Then
                Throw New Exception(
                    "Object Type id '" & asObjectTypeId &
                    "' is ambiguous in the supplied Boston model.")
            End If

            Return larCandidates(0)

        End Function

        Private Function BindDerivedHeadReading(
            ByVal arDerivedFactType As FBM.FactType,
            ByVal aarHeadTokens As List(Of SurfaceToken)) As ReadingMatch

            If arDerivedFactType.FactTypeReading Is Nothing Then
                Throw New Exception(
                    "Derived Fact Type '" & arDerivedFactType.Id &
                    "' has no Fact Type Readings.")
            End If
            If aarHeadTokens Is Nothing OrElse
               aarHeadTokens.Count = 0 OrElse
               aarHeadTokens(0).Kind <>
                   SurfaceTokenKind.ModelReference Then
                Throw New Exception(
                    "The derivation head is not a Fact Type Reading.")
            End If

            Dim larMatches As New List(Of ReadingMatch)
            For Each lrReading As FBM.FactTypeReading In
                arDerivedFactType.FactTypeReading
                Dim lrMatch As ReadingMatch =
                    TryMatchReading(lrReading,
                                    aarHeadTokens,
                                    0,
                                    aarHeadTokens(0).ModelObject,
                                    aarHeadTokens(0).BindingKey,
                                    False,
                                    True)
                If lrMatch IsNot Nothing AndAlso
                   lrMatch.NextTokenIndex = aarHeadTokens.Count Then
                    larMatches.Add(lrMatch)
                End If
            Next

            larMatches = CollapseEquivalentReadingMatches(larMatches)
            If larMatches.Count = 0 Then
                Throw New Exception(
                    "No Fact Type Reading of derived Fact Type '" &
                    arDerivedFactType.Id &
                    "' exactly matches the derivation head.")
            End If
            If larMatches.Count > 1 Then
                Throw New Exception(
                    "More than one Fact Type Reading of derived Fact Type '" &
                    arDerivedFactType.Id &
                    "' exactly matches the derivation head.")
            End If

            Return larMatches(0)

        End Function

        Private Function BindLinearRolePath(
            ByVal aarTokens As List(Of SurfaceToken),
            ByVal aiTokenIndex As Integer,
            ByVal arCurrentObject As FBM.ModelObject,
            ByVal asCurrentBindingKey As String,
            ByVal arModel As FBM.Model,
            ByVal abFirstOccurrence As Boolean) As LinearPathSolution

            If aiTokenIndex = aarTokens.Count Then
                Return New LinearPathSolution
            End If
            If aiTokenIndex > aarTokens.Count Then Return Nothing

            Dim larMatches As List(Of ReadingMatch) =
                FindReadingMatches(aarTokens,
                                   aiTokenIndex,
                                   arCurrentObject,
                                   asCurrentBindingKey,
                                   arModel,
                                   abFirstOccurrence)
            If larMatches.Count = 0 Then Return Nothing

            Dim larArities As List(Of Integer) =
                larMatches.Select(Function(x) x.Roles.Count).
                    Distinct().
                    OrderByDescending(Function(x) x).
                    ToList()

            For Each liArity As Integer In larArities
                Dim larSuccessful As New List(Of LinearPathSolution)

                For Each lrMatch As ReadingMatch In
                    larMatches.Where(
                        Function(x) x.Roles.Count = liArity)

                    If lrMatch.NextTokenIndex <= aiTokenIndex OrElse
                       lrMatch.Roles.Count = 0 Then Continue For

                    Dim lrLastRole As FBM.Role =
                        lrMatch.Roles.Last()
                    Dim lsLastBindingKey As String =
                        lrMatch.BindingKeys.Last()
                    Dim lrTail As LinearPathSolution =
                        BindLinearRolePath(aarTokens,
                                           lrMatch.NextTokenIndex,
                                           lrLastRole.JoinedORMObject,
                                           lsLastBindingKey,
                                           arModel,
                                           False)

                    If lrTail IsNot Nothing Then
                        Dim lrSuccessful As New LinearPathSolution
                        lrSuccessful.Occurrences.Add(lrMatch)
                        lrSuccessful.Occurrences.AddRange(
                            lrTail.Occurrences)
                        If lrMatch.IsNegated Then
                            lrSuccessful.NegatedOccurrenceIndexes.Add(0)
                        End If
                        For Each liNegatedIndex As Integer In
                            lrTail.NegatedOccurrenceIndexes
                            lrSuccessful.NegatedOccurrenceIndexes.Add(
                                liNegatedIndex + 1)
                        Next
                        larSuccessful.Add(lrSuccessful)
                    End If
                Next

                If larSuccessful.Count = 1 Then Return larSuccessful(0)
                If larSuccessful.Count > 1 Then
                    Throw New Exception(
                        "More than one maximal Fact Type Reading path " &
                        "matches near '" &
                        SurfaceTokensText(aarTokens,
                                          aiTokenIndex) & "'.")
                End If
            Next

            Return Nothing

        End Function

        Private Function FindReadingMatches(
            ByVal aarTokens As List(Of SurfaceToken),
            ByVal aiTokenIndex As Integer,
            ByVal arCurrentObject As FBM.ModelObject,
            ByVal asCurrentBindingKey As String,
            ByVal arModel As FBM.Model,
            ByVal abFirstOccurrence As Boolean) As List(Of ReadingMatch)

            Dim larMatches As New List(Of ReadingMatch)
            If arModel.FactType Is Nothing Then Return larMatches

            Dim larReadings =
                From lrFactType As FBM.FactType In arModel.FactType
                Where lrFactType IsNot Nothing AndAlso
                      lrFactType.FactTypeReading IsNot Nothing
                From lrReading As FBM.FactTypeReading In
                    lrFactType.FactTypeReading
                Where lrReading IsNot Nothing AndAlso
                      lrReading.PredicatePart IsNot Nothing AndAlso
                      lrReading.PredicatePart.Count > 0
                Let lrFirstPart = lrReading.PredicatePart.
                    Where(Function(x) x IsNot Nothing).
                    OrderBy(Function(x) x.SequenceNr).
                    FirstOrDefault()
                Where lrFirstPart IsNot Nothing AndAlso
                      lrFirstPart.Role IsNot Nothing AndAlso
                      SameModelObject(
                          lrFirstPart.Role.JoinedORMObject,
                          arCurrentObject)
                Order By lrReading.PredicatePart.Count Descending
                Select lrReading

            For Each lrReading As FBM.FactTypeReading In larReadings
                Dim lrMatch As ReadingMatch =
                    TryMatchReading(lrReading,
                                    aarTokens,
                                    aiTokenIndex,
                                    arCurrentObject,
                                    asCurrentBindingKey,
                                    Not abFirstOccurrence,
                                    abFirstOccurrence)
                If lrMatch IsNot Nothing Then larMatches.Add(lrMatch)
            Next

            Return CollapseEquivalentReadingMatches(larMatches).
                OrderByDescending(Function(x) x.Roles.Count).
                ToList()

        End Function

        Private Function TryMatchReading(
            ByVal arReading As FBM.FactTypeReading,
            ByVal aarActualTokens As List(Of SurfaceToken),
            ByVal aiActualStart As Integer,
            ByVal arCurrentObject As FBM.ModelObject,
            ByVal asCurrentBindingKey As String,
            ByVal abAllowImplicitFirstRole As Boolean,
            ByVal abRequireExplicitFirstRole As Boolean) As ReadingMatch

            Dim larExpected As List(Of ExpectedReadingToken) =
                BuildExpectedReadingTokens(arReading)
            If larExpected.Count = 0 Then Return Nothing

            Dim lrExplicit As ReadingMatch =
                TryMatchExpectedReading(arReading,
                                        larExpected,
                                        0,
                                        aarActualTokens,
                                        aiActualStart,
                                        arCurrentObject,
                                        asCurrentBindingKey,
                                        False)
            If lrExplicit IsNot Nothing Then Return lrExplicit

            If abRequireExplicitFirstRole OrElse
               Not abAllowImplicitFirstRole OrElse
               larExpected(0).Kind <>
                   SurfaceTokenKind.ModelReference OrElse
               larExpected(0).Role Is Nothing OrElse
               Not SameModelObject(
                   larExpected(0).Role.JoinedORMObject,
                   arCurrentObject) Then
                Return Nothing
            End If

            Return TryMatchExpectedReading(arReading,
                                           larExpected,
                                           1,
                                           aarActualTokens,
                                           aiActualStart,
                                           arCurrentObject,
                                           asCurrentBindingKey,
                                           True)

        End Function

        Private Function TryMatchExpectedReading(
            ByVal arReading As FBM.FactTypeReading,
            ByVal aarExpected As List(Of ExpectedReadingToken),
            ByVal aiExpectedStart As Integer,
            ByVal aarActual As List(Of SurfaceToken),
            ByVal aiActualStart As Integer,
            ByVal arCurrentObject As FBM.ModelObject,
            ByVal asCurrentBindingKey As String,
            ByVal abImplicitFirstRole As Boolean) As ReadingMatch

            Dim lrResult As New ReadingMatch With {
                .Reading = arReading
            }
            Dim liActual As Integer = aiActualStart
            Dim liRoleOrdinal As Integer = 0

            If abImplicitFirstRole Then
                lrResult.Roles.Add(aarExpected(0).Role)
                lrResult.BindingKeys.Add(asCurrentBindingKey)
                liRoleOrdinal = 1
            End If

            For liExpected As Integer = aiExpectedStart To aarExpected.Count - 1

                If liActual >= aarActual.Count Then Return Nothing

                Dim lrExpected As ExpectedReadingToken =
                    aarExpected(liExpected)
                Dim lrActual As SurfaceToken = aarActual(liActual)

                If lrExpected.Kind = SurfaceTokenKind.Literal Then
                    If lrActual.Kind <> SurfaceTokenKind.Literal OrElse
                       Not String.Equals(lrExpected.Text,
                                         lrActual.Text,
                                         StringComparison.OrdinalIgnoreCase) Then
                        Return Nothing
                    End If
                Else
                    If lrActual.Kind <>
                           SurfaceTokenKind.ModelReference OrElse
                       lrExpected.Role Is Nothing OrElse
                       Not SameModelObject(
                           lrExpected.Role.JoinedORMObject,
                           lrActual.ModelObject) Then
                        Return Nothing
                    End If

                    If liRoleOrdinal = 0 AndAlso
                       arCurrentObject IsNot Nothing AndAlso
                       (Not SameModelObject(lrActual.ModelObject,
                                            arCurrentObject) OrElse
                        Not String.Equals(lrActual.BindingKey,
                                          asCurrentBindingKey,
                                          StringComparison.OrdinalIgnoreCase)) Then
                        Return Nothing
                    End If

                    lrResult.Roles.Add(lrExpected.Role)
                    lrResult.BindingKeys.Add(lrActual.BindingKey)
                    If lrActual.IsNegativeQuantifier Then
                        lrResult.IsNegated = True
                    End If
                    liRoleOrdinal += 1
                End If

                liActual += 1
            Next

            If liActual <= aiActualStart Then Return Nothing
            If lrResult.Roles.Count = 0 OrElse
               lrResult.Roles.Any(
                   Function(x) x Is Nothing OrElse
                               x.JoinedORMObject Is Nothing) Then
                Return Nothing
            End If

            lrResult.NextTokenIndex = liActual
            Return lrResult

        End Function

        Private Function BuildExpectedReadingTokens(
            ByVal arReading As FBM.FactTypeReading) As List(Of ExpectedReadingToken)

            Dim larTokens As New List(Of ExpectedReadingToken)
            AddExpectedLiteralTokens(larTokens,
                                     arReading.FrontText)

            Dim larParts As List(Of FBM.PredicatePart) =
                arReading.PredicatePart.
                    Where(Function(x) x IsNot Nothing).
                    OrderBy(Function(x) x.SequenceNr).
                    ToList()

            For Each lrPart As FBM.PredicatePart In larParts
                AddExpectedLiteralTokens(larTokens,
                                         lrPart.PreBoundText)
                larTokens.Add(New ExpectedReadingToken With {
                    .Kind = SurfaceTokenKind.ModelReference,
                    .Role = lrPart.Role,
                    .Text = If(lrPart.Role Is Nothing OrElse
                               lrPart.Role.JoinedORMObject Is Nothing,
                               "",
                               lrPart.Role.JoinedORMObject.Id)
                })
                AddExpectedLiteralTokens(larTokens,
                                         lrPart.PostBoundText)
                AddExpectedLiteralTokens(larTokens,
                                         lrPart.PredicatePartText)
            Next

            AddExpectedLiteralTokens(larTokens,
                                     arReading.FollowingText)
            Return larTokens

        End Function

        Private Sub AddExpectedLiteralTokens(
            ByVal aarTokens As List(Of ExpectedReadingToken),
            ByVal asText As String)

            If String.IsNullOrWhiteSpace(asText) Then Exit Sub
            For Each lrMatch As Match In Regex.Matches(asText, "\S+")
                aarTokens.Add(New ExpectedReadingToken With {
                    .Kind = SurfaceTokenKind.Literal,
                    .Text = NormaliseSurfaceToken(lrMatch.Value)
                })
            Next

        End Sub

        Private Function CollapseEquivalentReadingMatches(
            ByVal aarMatches As List(Of ReadingMatch)) As List(Of ReadingMatch)

            Dim ldictMatches As New Dictionary(Of String, ReadingMatch)(
                StringComparer.OrdinalIgnoreCase)

            For Each lrMatch As ReadingMatch In aarMatches
                If lrMatch Is Nothing OrElse
                   lrMatch.Reading Is Nothing Then Continue For

                Dim lsSignature As String =
                    If(lrMatch.Reading.FactType Is Nothing,
                       "",
                       lrMatch.Reading.FactType.Id) & "|" &
                    String.Join(",",
                                lrMatch.Roles.Select(
                                    Function(x) If(x Is Nothing,
                                                   "",
                                                   x.Id))) & "|" &
                     lrMatch.NextTokenIndex.ToString() & "|" &
                     String.Join(",", lrMatch.BindingKeys) & "|" &
                     lrMatch.IsNegated.ToString()

                Dim lrExisting As ReadingMatch = Nothing
                If Not ldictMatches.TryGetValue(lsSignature,
                                                lrExisting) OrElse
                   ReadingPreference(lrMatch.Reading) >
                       ReadingPreference(lrExisting.Reading) Then
                    ldictMatches(lsSignature) = lrMatch
                End If
            Next

            Return ldictMatches.Values.ToList()

        End Function

        Private Function ReadingPreference(
            ByVal arReading As FBM.FactTypeReading) As Integer

            If arReading Is Nothing Then Return 0
            If arReading.IsPreferred Then Return 2
            If arReading.IsPreferredForPredicate Then Return 1
            Return 0

        End Function

        Private Function BuildFBMDerivationRule(
            ByVal arDerivedFactType As FBM.FactType,
            ByVal arHeadMatch As ReadingMatch,
            ByVal arSolution As LinearPathSolution,
            Optional ByVal arModel As FBM.Model = Nothing,
            Optional ByVal arCalculatedPlan As CalculatedWherePlan = Nothing) As FBM.DerivationRule

            Dim lrRule As New FBM.DerivationRule
            Dim lrPath As New FBM.FactTypeDerivationPath With {
                .Id = NewDerivationId(),
                .Name = arDerivedFactType.Id,
                .PathComponents = New FBM.PathComponents
            }
            Dim lrRolePath As New FBM.RolePath With {
                .Id = NewDerivationId()
            }

            Dim lrFirstMatch As ReadingMatch =
                arSolution.Occurrences(0)
            Dim lrRootObject As FBM.ModelObject =
                lrFirstMatch.Roles(0).JoinedORMObject
            Dim lsRootBindingKey As String =
                lrFirstMatch.BindingKeys(0)
            Dim lrRoot As New FBM.RootObjectType With {
                .id = NewDerivationId(),
                .ref = lrRootObject.Id,
                .BostonModelElement = lrRootObject,
                .IsNegated = False
            }
            lrRolePath.RootObjectType = lrRoot

            Dim ldictOccurrences As New Dictionary(
                Of String, List(Of BindingOccurrence))(
                    StringComparer.OrdinalIgnoreCase)
            AddBindingOccurrence(ldictOccurrences,
                                 lsRootBindingKey,
                                 New BindingOccurrence With {
                                     .IsPathRoot = True
                                 })

            Dim liPathIndex As Integer = 0
            For liOccurrence As Integer = 0 To arSolution.Occurrences.Count - 1

                Dim lrMatch As ReadingMatch =
                    arSolution.Occurrences(liOccurrence)

                For liRole As Integer = 0 To lrMatch.Roles.Count - 1
                    Dim lrPathedRole As New FBM.PathedRole With {
                        .id = NewDerivationId(),
                        .Ref = lrMatch.Roles(liRole).Id,
                        .IsNegated = liRole = 0 AndAlso
                                     arSolution.NegatedOccurrenceIndexes.Contains(liOccurrence),
                        .Purpose = If(liRole = 0,
                                      "PostInnerJoin",
                                      "SameFactType")
                    }
                    lrRolePath.PathedRole.Add(lrPathedRole)

                    AddBindingOccurrence(
                        ldictOccurrences,
                        lrMatch.BindingKeys(liRole),
                        New BindingOccurrence With {
                            .BranchIndex = 0,
                            .PathIndex = liPathIndex,
                            .OccurrenceIndex = liOccurrence,
                            .RoleIndex = liRole,
                            .OccurrenceRoleCount = lrMatch.Roles.Count,
                            .PathedRole = lrPathedRole
                        })
                    liPathIndex += 1
                Next
            Next

            AddRequiredObjectUnifiers(lrRolePath,
                                      lrRoot,
                                      ldictOccurrences)

            Dim ldictCalculatedProjectionSources As Dictionary(Of String, String) = Nothing
            If arCalculatedPlan IsNot Nothing Then
                ldictCalculatedProjectionSources =
                    AddCalculatedWhereClause(arModel,
                                             lrRolePath,
                                             arHeadMatch,
                                             ldictOccurrences,
                                             arCalculatedPlan)
            End If

            lrPath.PathComponents.RolePaths.Add(lrRolePath)
            AddDerivationProjection(lrPath,
                                    lrRolePath,
                                    lrRoot,
                                    arHeadMatch,
                                    ldictOccurrences,
                                    ldictCalculatedProjectionSources)
            lrRule.FactTypeDerivationPath = lrPath
            Return lrRule

        End Function

        Private Sub CollapseWhereNegatedClauses(
            ByRef aarSolutions As List(Of LinearPathSolution),
            ByRef aarConnectives As List(Of Derivations.LogicalConnective))

            If aarSolutions Is Nothing OrElse aarSolutions.Count = 0 Then Exit Sub
            If aarConnectives Is Nothing Then
                aarConnectives = New List(Of Derivations.LogicalConnective)
            End If
            If aarConnectives.Count <> aarSolutions.Count - 1 Then
                Throw New Exception(
                    "The derivation clause/connective sequence is incomplete.")
            End If

            Dim larCollapsedSolutions As New List(Of LinearPathSolution)
            Dim larCollapsedConnectives As New List(Of Derivations.LogicalConnective)
            larCollapsedSolutions.Add(aarSolutions(0))

            For liConnective As Integer = 0 To aarConnectives.Count - 1
                Dim lrConnective As Derivations.LogicalConnective =
                    aarConnectives(liConnective)
                Dim lrNextSolution As LinearPathSolution =
                    aarSolutions(liConnective + 1)

                If Not String.IsNullOrWhiteSpace(lrConnective.KEYWD_WHERE_NEGATED) Then
                    Dim lrPreviousSolution As LinearPathSolution =
                        larCollapsedSolutions.Last()
                    If lrPreviousSolution.NegatedOccurrenceIndexes.Count > 0 Then
                        Throw New Exception(
                            "Multiple negated 'where' clauses on one RolePath " &
                            "alternative are not yet supported.")
                    End If

                    Dim liNegatedStart As Integer =
                        lrPreviousSolution.Occurrences.Count
                    lrPreviousSolution.Occurrences.AddRange(
                        lrNextSolution.Occurrences)
                    lrPreviousSolution.NegatedOccurrenceIndexes.Add(
                        liNegatedStart)
                Else
                    larCollapsedConnectives.Add(lrConnective)
                    larCollapsedSolutions.Add(lrNextSolution)
                End If
            Next

            aarSolutions = larCollapsedSolutions
            aarConnectives = larCollapsedConnectives

        End Sub

        Private Function BuildFBMScopedAndOrDerivationRule(
            ByVal arDerivedFactType As FBM.FactType,
            ByVal arHeadMatch As ReadingMatch,
            ByVal aarSolutions As List(Of LinearPathSolution),
            ByVal aarConnectives As List(Of Derivations.LogicalConnective),
            ByVal arModel As FBM.Model,
            ByVal arCalculatedPlan As CalculatedWherePlan) As FBM.DerivationRule

            If aarSolutions Is Nothing OrElse
               aarConnectives Is Nothing OrElse
               arCalculatedPlan Is Nothing OrElse
               aarSolutions.Count < 2 OrElse
               aarConnectives.Count <> aarSolutions.Count - 1 Then
                Throw New Exception(
                    "The scoped mixed 'and'/'or' branch sequence is incomplete.")
            End If

            If arCalculatedPlan.ScopeClauseIndex < 0 OrElse
               arCalculatedPlan.ScopeClauseIndex >= aarSolutions.Count Then
                Throw New Exception(
                    "The calculated 'where' clause does not have a valid " &
                    "preceding RolePath clause.")
            End If

            Dim larGroups As New List(Of List(Of LinearPathSolution))
            Dim larGroupStarts As New List(Of Integer)
            Dim larCurrentGroup As New List(Of LinearPathSolution)
            larGroupStarts.Add(0)
            larCurrentGroup.Add(aarSolutions(0))

            For liConnective As Integer = 0 To aarConnectives.Count - 1
                If Not String.IsNullOrWhiteSpace(
                    aarConnectives(liConnective).KEYWD_OR) Then
                    larGroups.Add(larCurrentGroup)
                    larCurrentGroup = New List(Of LinearPathSolution)
                    larGroupStarts.Add(liConnective + 1)
                End If
                larCurrentGroup.Add(aarSolutions(liConnective + 1))
            Next
            larGroups.Add(larCurrentGroup)

            Dim liCalculatedGroup As Integer = -1
            For liGroup As Integer = 0 To larGroups.Count - 1
                Dim liGroupStart As Integer = larGroupStarts(liGroup)
                Dim liGroupEnd As Integer =
                    liGroupStart + larGroups(liGroup).Count - 1
                If arCalculatedPlan.ScopeClauseIndex >= liGroupStart AndAlso
                   arCalculatedPlan.ScopeClauseIndex <= liGroupEnd Then
                    liCalculatedGroup = liGroup
                    Exit For
                End If
            Next

            If liCalculatedGroup < 0 Then
                Throw New Exception(
                    "The calculated 'where' clause could not be associated " &
                    "with its preceding RolePath alternative.")
            End If

            Dim lrRule As New FBM.DerivationRule
            Dim lrPath As New FBM.FactTypeDerivationPath With {
                .Id = NewDerivationId(),
                .Name = arDerivedFactType.Id,
                .PathComponents = New FBM.PathComponents
            }

            For liGroup As Integer = 0 To larGroups.Count - 1
                Dim lrGroupPlan As CalculatedWherePlan = Nothing
                If liGroup = liCalculatedGroup Then
                    lrGroupPlan = arCalculatedPlan
                End If

                Dim lrGroupRule As FBM.DerivationRule
                If larGroups(liGroup).Count = 1 Then
                    lrGroupRule = BuildFBMDerivationRule(
                        arDerivedFactType,
                        arHeadMatch,
                        larGroups(liGroup)(0),
                        arModel,
                        lrGroupPlan)
                Else
                    lrGroupRule = BuildFBMAndDerivationRule(
                        arDerivedFactType,
                        arHeadMatch,
                        larGroups(liGroup),
                        arModel,
                        lrGroupPlan)
                End If

                If lrGroupRule Is Nothing OrElse
                   lrGroupRule.FactTypeDerivationPath Is Nothing Then
                    Throw New Exception(
                        "A scoped RolePath alternative could not be compiled.")
                End If

                For Each lrRolePath As FBM.RolePath In
                    lrGroupRule.FactTypeDerivationPath.PathComponents.RolePaths
                    lrPath.PathComponents.RolePaths.Add(lrRolePath)
                Next
                For Each lrProjection As FBM.DerivationProjection In
                    lrGroupRule.FactTypeDerivationPath.Projection
                    lrPath.Projection.Add(lrProjection)
                Next
            Next

            'Sibling RolePaths are alternative derivation projections. The
            'renderer takes their join operator from the first RolePath. Keep
            'that RolePath's own conjunctive branch intact by placing its
            'existing SubPaths beneath one AND wrapper before marking the
            'top-level sibling relationship as OR.
            Dim lrFirstRolePath As FBM.RolePath =
                lrPath.PathComponents.RolePaths(0)
            If lrFirstRolePath.SubPath IsNot Nothing AndAlso
               lrFirstRolePath.SubPath.Count > 1 Then
                Dim larFirstGroupSubPaths As List(Of FBM.RoleSubPath) =
                    lrFirstRolePath.SubPath.ToList()
                Dim lrFirstGroupWrapper As New FBM.RoleSubPath With {
                    .Id = NewDerivationId(),
                    .SplitCombinationOperator = "And"
                }
                lrFirstGroupWrapper.SubPath.AddRange(larFirstGroupSubPaths)
                lrFirstRolePath.SubPath.Clear()
                lrFirstRolePath.SubPath.Add(lrFirstGroupWrapper)
            End If
            lrFirstRolePath.SplitCombinationOperator = "Or"

            lrRule.FactTypeDerivationPath = lrPath
            Return lrRule

        End Function

        Private Function BuildFBMAndOrDerivationRule(
            ByVal arDerivedFactType As FBM.FactType,
            ByVal arHeadMatch As ReadingMatch,
            ByVal aarSolutions As List(Of LinearPathSolution),
            ByVal aarConnectives As List(Of Derivations.LogicalConnective)) As FBM.DerivationRule

            If aarSolutions Is Nothing OrElse
               aarConnectives Is Nothing OrElse
               aarSolutions.Count < 2 OrElse
               aarConnectives.Count <> aarSolutions.Count - 1 Then
                Throw New Exception(
                    "The mixed 'and'/'or' branch sequence is incomplete.")
            End If

            Dim larGroups As New List(Of List(Of LinearPathSolution))
            Dim larCurrentGroup As New List(Of LinearPathSolution)
            larCurrentGroup.Add(aarSolutions(0))

            For liConnective As Integer = 0 To aarConnectives.Count - 1
                If Not String.IsNullOrWhiteSpace(
                    aarConnectives(liConnective).KEYWD_OR) Then
                    larGroups.Add(larCurrentGroup)
                    larCurrentGroup = New List(Of LinearPathSolution)
                End If
                larCurrentGroup.Add(aarSolutions(liConnective + 1))
            Next
            larGroups.Add(larCurrentGroup)

            Dim lrRule As New FBM.DerivationRule
            Dim lrPath As New FBM.FactTypeDerivationPath With {
                .Id = NewDerivationId(),
                .Name = arDerivedFactType.Id,
                .PathComponents = New FBM.PathComponents
            }
            Dim lrRolePath As New FBM.RolePath With {
                .Id = NewDerivationId(),
                .SplitCombinationOperator = "Or"
            }

            Dim lrFirstMatch As ReadingMatch =
                aarSolutions(0).Occurrences(0)
            Dim lrRootObject As FBM.ModelObject =
                lrFirstMatch.Roles(0).JoinedORMObject
            Dim lsRootBindingKey As String =
                lrFirstMatch.BindingKeys(0)
            Dim lrRoot As New FBM.RootObjectType With {
                .id = NewDerivationId(),
                .ref = lrRootObject.Id,
                .BostonModelElement = lrRootObject,
                .IsNegated = False
            }
            lrRolePath.RootObjectType = lrRoot

            Dim ldictOccurrences As New Dictionary(
                Of String, List(Of BindingOccurrence))(
                    StringComparer.OrdinalIgnoreCase)
            AddBindingOccurrence(ldictOccurrences,
                                 lsRootBindingKey,
                                 New BindingOccurrence With {
                                     .IsPathRoot = True,
                                     .BranchIndex = -1
                                 })

            Dim liPathIndex As Integer = 0
            Dim liBranchIndex As Integer = 0
            For Each larGroup As List(Of LinearPathSolution) In larGroups
                Dim lrGroupSubPath As New FBM.RoleSubPath With {
                    .Id = NewDerivationId()
                }

                If larGroup.Count > 1 Then
                    lrGroupSubPath.SplitCombinationOperator = "And"
                    For liSolution As Integer = 0 To larGroup.Count - 1
                        Dim lrLeafSubPath As New FBM.RoleSubPath With {
                            .Id = NewDerivationId()
                        }
                        AddSolutionToSubPath(larGroup(liSolution),
                                             lrLeafSubPath,
                                             lrRootObject,
                                             lsRootBindingKey,
                                             liBranchIndex,
                                             liPathIndex,
                                             ldictOccurrences)
                        lrGroupSubPath.SubPath.Add(lrLeafSubPath)
                        liBranchIndex += 1
                    Next
                Else
                    AddSolutionToSubPath(larGroup(0),
                                         lrGroupSubPath,
                                         lrRootObject,
                                         lsRootBindingKey,
                                         liBranchIndex,
                                         liPathIndex,
                                         ldictOccurrences)
                    liBranchIndex += 1
                End If

                lrRolePath.SubPath.Add(lrGroupSubPath)
            Next

            AddRequiredObjectUnifiers(lrRolePath,
                                      lrRoot,
                                      ldictOccurrences)
            lrPath.PathComponents.RolePaths.Add(lrRolePath)
            AddDerivationProjection(lrPath,
                                    lrRolePath,
                                    lrRoot,
                                    arHeadMatch,
                                    ldictOccurrences)
            lrRule.FactTypeDerivationPath = lrPath
            Return lrRule

        End Function

        Private Sub AddSolutionToSubPath(
            ByVal arSolution As LinearPathSolution,
            ByVal arSubPath As FBM.RoleSubPath,
            ByVal arRootObject As FBM.ModelObject,
            ByVal asRootBindingKey As String,
            ByVal aiBranchIndex As Integer,
            ByRef aiPathIndex As Integer,
            ByVal adictOccurrences As Dictionary(
                Of String, List(Of BindingOccurrence)))

            If arSolution Is Nothing OrElse
               arSolution.Occurrences.Count = 0 Then
                Throw New Exception(
                    "An empty logical RolePath branch was encountered.")
            End If

            Dim lrFirstMatch As ReadingMatch = arSolution.Occurrences(0)
            If Not SameModelObject(lrFirstMatch.Roles(0).JoinedORMObject,
                                   arRootObject) OrElse
               Not String.Equals(lrFirstMatch.BindingKeys(0),
                                 asRootBindingKey,
                                 StringComparison.OrdinalIgnoreCase) Then
                Throw New Exception(
                    "All logical RolePath branches must begin from the " &
                    "same root variable.")
            End If

            For liOccurrence As Integer = 0 To arSolution.Occurrences.Count - 1
                Dim lrMatch As ReadingMatch =
                    arSolution.Occurrences(liOccurrence)
                For liRole As Integer = 0 To lrMatch.Roles.Count - 1
                    Dim lrPathedRole As New FBM.PathedRole With {
                        .id = NewDerivationId(),
                        .Ref = lrMatch.Roles(liRole).Id,
                        .IsNegated = liRole = 0 AndAlso
                                     arSolution.NegatedOccurrenceIndexes.Contains(liOccurrence),
                        .Purpose = If(liRole = 0,
                                      "PostInnerJoin",
                                      "SameFactType")
                    }
                    arSubPath.PathedRole.Add(lrPathedRole)

                    AddBindingOccurrence(
                        adictOccurrences,
                        lrMatch.BindingKeys(liRole),
                        New BindingOccurrence With {
                            .BranchIndex = aiBranchIndex,
                            .PathIndex = aiPathIndex,
                            .OccurrenceIndex = liOccurrence,
                            .RoleIndex = liRole,
                            .OccurrenceRoleCount = lrMatch.Roles.Count,
                            .PathedRole = lrPathedRole
                        })
                    aiPathIndex += 1
                Next
            Next

        End Sub

        Private Function BuildFBMAndDerivationRule(
            ByVal arDerivedFactType As FBM.FactType,
            ByVal arHeadMatch As ReadingMatch,
            ByVal aarSolutions As List(Of LinearPathSolution),
            Optional ByVal arModel As FBM.Model = Nothing,
            Optional ByVal arCalculatedPlan As CalculatedWherePlan = Nothing) As FBM.DerivationRule

            If aarSolutions Is Nothing OrElse aarSolutions.Count < 2 Then
                Throw New Exception(
                    "An 'and' derivation requires at least two RolePath branches.")
            End If

            Dim lrRule As New FBM.DerivationRule
            Dim lrPath As New FBM.FactTypeDerivationPath With {
                .Id = NewDerivationId(),
                .Name = arDerivedFactType.Id,
                .PathComponents = New FBM.PathComponents
            }
            Dim lrRolePath As New FBM.RolePath With {
                .Id = NewDerivationId(),
                .SplitCombinationOperator = "And"
            }

            Dim lrFirstMatch As ReadingMatch =
                aarSolutions(0).Occurrences(0)
            Dim lrRootObject As FBM.ModelObject =
                lrFirstMatch.Roles(0).JoinedORMObject
            Dim lsRootBindingKey As String =
                lrFirstMatch.BindingKeys(0)
            Dim lrRoot As New FBM.RootObjectType With {
                .id = NewDerivationId(),
                .ref = lrRootObject.Id,
                .BostonModelElement = lrRootObject,
                .IsNegated = False
            }
            lrRolePath.RootObjectType = lrRoot

            Dim ldictOccurrences As New Dictionary(
                Of String, List(Of BindingOccurrence))(
                    StringComparer.OrdinalIgnoreCase)
            AddBindingOccurrence(ldictOccurrences,
                                 lsRootBindingKey,
                                 New BindingOccurrence With {
                                     .IsPathRoot = True,
                                     .BranchIndex = -1
                                 })

            Dim liPathIndex As Integer = 0
            For liBranch As Integer = 0 To aarSolutions.Count - 1
                Dim lrSolution As LinearPathSolution = aarSolutions(liBranch)
                If lrSolution Is Nothing OrElse
                   lrSolution.Occurrences.Count = 0 Then
                    Throw New Exception(
                        "An empty 'and' RolePath branch was encountered.")
                End If

                Dim lrBranchFirst As ReadingMatch =
                    lrSolution.Occurrences(0)
                If Not SameModelObject(
                        lrBranchFirst.Roles(0).JoinedORMObject,
                        lrRootObject) OrElse
                   Not String.Equals(lrBranchFirst.BindingKeys(0),
                                     lsRootBindingKey,
                                     StringComparison.OrdinalIgnoreCase) Then
                    Throw New Exception(
                        "All 'and' RolePath branches must begin from the " &
                        "same root variable.")
                End If

                Dim lrSubPath As New FBM.RoleSubPath With {
                    .Id = NewDerivationId()
                }

                For liOccurrence As Integer = 0 To lrSolution.Occurrences.Count - 1

                    Dim lrMatch As ReadingMatch =
                        lrSolution.Occurrences(liOccurrence)
                    For liRole As Integer = 0 To lrMatch.Roles.Count - 1
                        Dim lrPathedRole As New FBM.PathedRole With {
                            .id = NewDerivationId(),
                            .Ref = lrMatch.Roles(liRole).Id,
                            .IsNegated = liRole = 0 AndAlso
                                         lrSolution.NegatedOccurrenceIndexes.Contains(liOccurrence),
                            .Purpose = If(liRole = 0,
                                          "PostInnerJoin",
                                          "SameFactType")
                        }
                        lrSubPath.PathedRole.Add(lrPathedRole)

                        AddBindingOccurrence(
                            ldictOccurrences,
                            lrMatch.BindingKeys(liRole),
                            New BindingOccurrence With {
                                .BranchIndex = liBranch,
                                .PathIndex = liPathIndex,
                                .OccurrenceIndex = liOccurrence,
                                .RoleIndex = liRole,
                                .OccurrenceRoleCount = lrMatch.Roles.Count,
                                .PathedRole = lrPathedRole
                            })
                        liPathIndex += 1
                    Next
                Next

                lrRolePath.SubPath.Add(lrSubPath)
            Next

            AddRequiredObjectUnifiers(lrRolePath,
                                      lrRoot,
                                      ldictOccurrences)

            Dim ldictCalculatedProjectionSources As Dictionary(Of String, String) = Nothing
            If arCalculatedPlan IsNot Nothing Then
                ldictCalculatedProjectionSources =
                    AddCalculatedWhereClause(arModel,
                                             lrRolePath,
                                             arHeadMatch,
                                             ldictOccurrences,
                                             arCalculatedPlan)
            End If

            lrPath.PathComponents.RolePaths.Add(lrRolePath)
            AddDerivationProjection(lrPath,
                                    lrRolePath,
                                    lrRoot,
                                    arHeadMatch,
                                    ldictOccurrences,
                                    ldictCalculatedProjectionSources)
            lrRule.FactTypeDerivationPath = lrPath
            Return lrRule

        End Function

        Private Sub AddDerivationProjection(
            ByVal arPath As FBM.FactTypeDerivationPath,
            ByVal arRolePath As FBM.RolePath,
            ByVal arRoot As FBM.RootObjectType,
            ByVal arHeadMatch As ReadingMatch,
            ByVal adictOccurrences As Dictionary(
                Of String, List(Of BindingOccurrence)),
            Optional ByVal adictCalculatedSources As Dictionary(Of String, String) = Nothing)

            Dim lrProjection As New FBM.DerivationProjection With {
                .Id = NewDerivationId(),
                .Ref = arRolePath.Id
            }

            For liRole As Integer = 0 To arHeadMatch.Roles.Count - 1
                Dim lsBindingKey As String =
                    arHeadMatch.BindingKeys(liRole)

                Dim lrSource As New FBM.DerivationSource
                Dim lsCalculatedValueId As String = Nothing
                If adictCalculatedSources IsNot Nothing AndAlso
                   adictCalculatedSources.TryGetValue(lsBindingKey,
                                                       lsCalculatedValueId) Then
                    lrSource.CalculatedValue =
                        New FBM.CalculatedValueReference With {
                            .Ref = lsCalculatedValueId
                        }
                Else
                    Dim larSources As List(Of BindingOccurrence) = Nothing
                    If Not adictOccurrences.TryGetValue(lsBindingKey,
                                                        larSources) Then
                        Throw New Exception(
                            "Derived head variable '" & lsBindingKey &
                            "' is not bound by the derivation RolePath.")
                    End If

                    If larSources.Any(Function(x) x.IsPathRoot) Then
                    lrSource.PathRoot =
                        New FBM.PathRootReference With {
                            .Ref = arRoot.id
                        }
                    Else
                        Dim lrSourceOccurrence As BindingOccurrence =
                            larSources.
                                Where(Function(x) x.PathedRole IsNot Nothing).
                                OrderByDescending(Function(x) x.PathIndex).
                                FirstOrDefault()
                        If lrSourceOccurrence Is Nothing Then
                            Throw New Exception(
                                "No RolePath source exists for derived head " &
                                "variable '" & lsBindingKey & "'.")
                        End If
                        lrSource.PathedRole =
                            New FBM.PathedRoleReference With {
                                .Ref = lrSourceOccurrence.PathedRole.id
                            }
                    End If
                End If

                lrProjection.RoleProjection.Add(
                    New FBM.RoleProjection With {
                        .Id = NewDerivationId(),
                        .Ref = arHeadMatch.Roles(liRole).Id,
                        .DerivationSource = lrSource
                    })
            Next

            arPath.Projection.Add(lrProjection)

        End Sub

        Private Function AddCalculatedWhereClause(
            ByVal arModel As FBM.Model,
            ByVal arRolePath As FBM.RolePath,
            ByVal arHeadMatch As ReadingMatch,
            ByVal adictOccurrences As Dictionary(Of String, List(Of BindingOccurrence)),
            ByVal arPlan As CalculatedWherePlan) As Dictionary(Of String, String)

            If arModel Is Nothing Then
                Throw New Exception(
                    "No Boston model was supplied for the calculated 'where' clause.")
            End If

            If Not String.Equals(arPlan.ComparisonOperatorSymbol,
                                 "=",
                                 StringComparison.Ordinal) AndAlso
               Not adictOccurrences.ContainsKey(arPlan.TargetBindingKey) Then
                Throw New Exception(
                    "Comparison input '" & arPlan.TargetBindingKey &
                    "' is not bound by the derivation RolePath.")
            End If

            If adictOccurrences.ContainsKey(arPlan.TargetBindingKey) Then
                AddCalculatedBooleanCondition(arModel,
                                              arRolePath,
                                              adictOccurrences,
                                              arPlan)
                Return Nothing
            End If

            Dim lrCalculatedValue As FBM.CalculatedValue =
                AddCalculatedProjectionValue(arModel,
                                             arRolePath,
                                             arHeadMatch,
                                             adictOccurrences,
                                             arPlan)
            Dim ldictSources As New Dictionary(Of String, String)(
                StringComparer.OrdinalIgnoreCase)
            ldictSources(arPlan.TargetBindingKey) = lrCalculatedValue.Id
            Return ldictSources

        End Function

        Private Sub AddCalculatedBooleanCondition(
            ByVal arModel As FBM.Model,
            ByVal arRolePath As FBM.RolePath,
            ByVal adictOccurrences As Dictionary(Of String, List(Of BindingOccurrence)),
            ByVal arPlan As CalculatedWherePlan)

            Dim larTargetOccurrences As List(Of BindingOccurrence) = Nothing
            If Not adictOccurrences.TryGetValue(arPlan.TargetBindingKey,
                                                larTargetOccurrences) Then
                Throw New Exception(
                    "Calculated condition input '" & arPlan.TargetBindingKey &
                    "' is not bound by the derivation RolePath.")
            End If

            Dim lrTargetOccurrence As BindingOccurrence =
                larTargetOccurrences.
                    Where(Function(x) x.PathedRole IsNot Nothing).
                    OrderByDescending(Function(x) x.PathIndex).
                    FirstOrDefault()
            If lrTargetOccurrence Is Nothing Then
                Throw New Exception(
                    "Calculated condition input '" & arPlan.TargetBindingKey &
                    "' has no PathedRole source.")
            End If

            Dim loRightSource As Object
            If arPlan.Expression IsNot Nothing Then
                Dim lrRightValue As FBM.CalculatedValue =
                    CreateCalculatedExpressionValue(arModel,
                                                    arRolePath,
                                                    adictOccurrences,
                                                    arPlan)
                loRightSource = New FBM.CalculatedValueRef With {
                    .Ref = lrRightValue.Id
                }
            ElseIf arPlan.RightArgument IsNot Nothing Then
                loRightSource = CreateCalculatedArgumentSource(
                    arModel,
                    arRolePath,
                    adictOccurrences,
                    arPlan.RightArgument)
            Else
                Throw New Exception(
                    "The calculated Boolean condition has no right operand.")
            End If

            Dim lrComparisonFunction As FBM.Function =
                EnsureBinaryFunction(arModel,
                                     arPlan.ComparisonOperatorSymbol,
                                     ComparisonFunctionName(
                                         arPlan.ComparisonOperatorSymbol),
                                     True)
            Dim lrComparisonValue As FBM.CalculatedValue =
                CreateBinaryCalculatedValue(
                    arRolePath,
                    lrComparisonFunction,
                    New FBM.PathedRoleRef With {
                        .Ref = lrTargetOccurrence.PathedRole.id
                    },
                    loRightSource)

            If arRolePath.Conditions Is Nothing Then
                arRolePath.Conditions = New FBM.Conditions
            End If
            arRolePath.Conditions.Items.Add(
                New FBM.CalculatedCondition With {
                    .Ref = lrComparisonValue.Id
                })

        End Sub

        Private Function AddCalculatedProjectionValue(
            ByVal arModel As FBM.Model,
            ByVal arRolePath As FBM.RolePath,
            ByVal arHeadMatch As ReadingMatch,
            ByVal adictOccurrences As Dictionary(Of String, List(Of BindingOccurrence)),
            ByVal arPlan As CalculatedWherePlan) As FBM.CalculatedValue

            If Not arHeadMatch.BindingKeys.Any(
                Function(x) String.Equals(x,
                                          arPlan.TargetBindingKey,
                                          StringComparison.OrdinalIgnoreCase)) Then
                Throw New Exception(
                    "Calculated target '" & arPlan.TargetBindingKey &
                    "' is not a variable in the derived Fact Type reading.")
            End If

            If Not String.Equals(arPlan.ComparisonOperatorSymbol,
                                 "=",
                                 StringComparison.Ordinal) Then
                Throw New Exception(
                    "A calculated projection must use '=' rather than '" &
                    arPlan.ComparisonOperatorSymbol & "'.")
            End If

            If arPlan.Expression Is Nothing Then
                Throw New Exception(
                    "A calculated projection requires an arithmetic or function expression.")
            End If

            Return CreateCalculatedExpressionValue(arModel,
                                                   arRolePath,
                                                   adictOccurrences,
                                                   arPlan)

        End Function

        Private Function CreateCalculatedExpressionValue(
            ByVal arModel As FBM.Model,
            ByVal arRolePath As FBM.RolePath,
            ByVal adictOccurrences As Dictionary(Of String, List(Of BindingOccurrence)),
            ByVal arPlan As CalculatedWherePlan) As FBM.CalculatedValue

            If arPlan Is Nothing OrElse arPlan.Expression Is Nothing Then
                Throw New Exception("The calculated expression is empty.")
            End If

            Return CreateCalculatedExpressionValue(arModel,
                                                   arRolePath,
                                                   adictOccurrences,
                                                   arPlan.Expression)

        End Function

        Private Function CreateCalculatedExpressionValue(
            ByVal arModel As FBM.Model,
            ByVal arRolePath As FBM.RolePath,
            ByVal adictOccurrences As Dictionary(Of String, List(Of BindingOccurrence)),
            ByVal arExpression As CalculatedExpressionPlan) As FBM.CalculatedValue

            If arExpression Is Nothing OrElse
               String.IsNullOrWhiteSpace(arExpression.FunctionName) Then
                Throw New Exception("The calculated expression has no function.")
            End If

            Dim liBagInputIndex As Integer = -1
            For liIndex As Integer = 0 To arExpression.Arguments.Count - 1
                If arExpression.Arguments(liIndex).IsBagInput Then
                    If liBagInputIndex >= 0 Then
                        Throw New Exception(
                            "A calculated function may have only one bag input.")
                    End If
                    liBagInputIndex = liIndex
                End If
            Next

            Dim lrFunction As FBM.Function =
                EnsureFunction(arModel,
                               arExpression.OperatorSymbol,
                               arExpression.FunctionName,
                               arExpression.Arguments.Count,
                               liBagInputIndex)
            Dim lrCalculatedValue As New FBM.CalculatedValue With {
                .Id = NewDerivationId(),
                .Function = New FBM.FunctionRef With {
                    .Ref = lrFunction.id
                },
                .Inputs = New FBM.Inputs
            }

            For liIndex As Integer = 0 To arExpression.Arguments.Count - 1
                Dim loSource As Object = CreateCalculatedArgumentSource(
                    arModel,
                    arRolePath,
                    adictOccurrences,
                    arExpression.Arguments(liIndex))
                Dim lrParameter As FBM.Parameter =
                    lrFunction.Parameters(liIndex)

                lrCalculatedValue.Inputs.Input.Add(
                    New FBM.Input With {
                        .Id = NewDerivationId(),
                        .Parameter = New FBM.ParameterRef With {
                            .Ref = lrParameter.id
                        },
                        .Source = New FBM.Source With {
                            .Item = loSource
                        }
                    })
            Next

            If Not String.IsNullOrWhiteSpace(
                arExpression.AggregationContextBindingKey) Then
                Dim larContextOccurrences As List(Of BindingOccurrence) = Nothing
                If Not adictOccurrences.TryGetValue(
                    arExpression.AggregationContextBindingKey,
                    larContextOccurrences) OrElse
                   Not larContextOccurrences.Any(Function(x) x.IsPathRoot) Then
                    Throw New Exception(
                        "Aggregation context '" &
                        arExpression.AggregationContextBindingKey &
                        "' is not the derivation RolePath root.")
                End If

                lrCalculatedValue.AggregationContext =
                    New FBM.AggregationContext With {
                        .PathRoot = New FBM.PathRootReference With {
                            .Ref = arRolePath.RootObjectType.id
                        }
                    }
            End If

            arRolePath.CalculatedValues.Add(lrCalculatedValue)
            Return lrCalculatedValue

        End Function

        Private Function CreateCalculatedArgumentSource(
            ByVal arModel As FBM.Model,
            ByVal arRolePath As FBM.RolePath,
            ByVal adictOccurrences As Dictionary(Of String, List(Of BindingOccurrence)),
            ByVal arArgument As CalculatedArgumentPlan) As Object

            Select Case arArgument.Kind
                Case CalculatedArgumentKind.Binding
                    Dim larOccurrences As List(Of BindingOccurrence) = Nothing
                    If Not adictOccurrences.TryGetValue(arArgument.BindingKey,
                                                       larOccurrences) Then
                        Throw New Exception(
                            "Calculated input '" & arArgument.BindingKey &
                            "' is not bound by the derivation RolePath.")
                    End If

                    Dim lrOccurrence As BindingOccurrence =
                        larOccurrences.
                            Where(Function(x) x.PathedRole IsNot Nothing).
                            OrderByDescending(Function(x) x.PathIndex).
                            FirstOrDefault()
                    If lrOccurrence Is Nothing Then
                        Throw New Exception(
                            "Calculated input '" & arArgument.BindingKey &
                            "' has no PathedRole source.")
                    End If

                    Return New FBM.PathedRoleRef With {
                        .Ref = lrOccurrence.PathedRole.id
                    }

                Case CalculatedArgumentKind.Constant
                    Return New FBM.Constant With {
                        .Id = NewDerivationId(),
                        .Value = arArgument.ConstantValue
                    }

                Case CalculatedArgumentKind.FunctionValue
                    Dim lrNestedValue As FBM.CalculatedValue =
                        CreateCalculatedExpressionValue(
                            arModel,
                            arRolePath,
                            adictOccurrences,
                            arArgument.FunctionExpression)
                    Return New FBM.CalculatedValueRef With {
                        .Ref = lrNestedValue.Id
                    }
            End Select

            Throw New Exception("The calculated function argument is not supported.")

        End Function

        Private Function CreateBinaryCalculatedValue(
            ByVal arRolePath As FBM.RolePath,
            ByVal arFunction As FBM.Function,
            ByVal aoLeftSource As Object,
            ByVal aoRightSource As Object) As FBM.CalculatedValue

            Dim lrLeftParameter As FBM.Parameter =
                ResolveFunctionParameter(arFunction, "left", 0)
            Dim lrRightParameter As FBM.Parameter =
                ResolveFunctionParameter(arFunction, "right", 1)
            Dim lrCalculatedValue As New FBM.CalculatedValue With {
                .Id = NewDerivationId(),
                .Function = New FBM.FunctionRef With {
                    .Ref = arFunction.id
                },
                .Inputs = New FBM.Inputs
            }

            lrCalculatedValue.Inputs.Input.Add(
                New FBM.Input With {
                    .Id = NewDerivationId(),
                    .Parameter = New FBM.ParameterRef With {
                        .Ref = lrLeftParameter.id
                    },
                    .Source = New FBM.Source With {
                        .Item = aoLeftSource
                    }
                })
            lrCalculatedValue.Inputs.Input.Add(
                New FBM.Input With {
                    .Id = NewDerivationId(),
                    .Parameter = New FBM.ParameterRef With {
                        .Ref = lrRightParameter.id
                    },
                    .Source = New FBM.Source With {
                        .Item = aoRightSource
                    }
                })

            arRolePath.CalculatedValues.Add(lrCalculatedValue)
            Return lrCalculatedValue

        End Function

        Private Function EnsureBinaryFunction(
            ByVal arModel As FBM.Model,
            ByVal asOperatorSymbol As String,
            ByVal asFunctionName As String,
            Optional ByVal abIsBoolean As Boolean = False) As FBM.Function

            Return EnsureFunction(arModel,
                                  asOperatorSymbol,
                                  asFunctionName,
                                  2,
                                  -1,
                                  abIsBoolean)

        End Function

        Private Function EnsureFunction(
            ByVal arModel As FBM.Model,
            ByVal asOperatorSymbol As String,
            ByVal asFunctionName As String,
            ByVal aiArgumentCount As Integer,
            Optional ByVal aiBagInputIndex As Integer = -1,
            Optional ByVal abIsBoolean As Boolean = False) As FBM.Function

            Dim lrFunction As FBM.Function = Nothing
            If Not String.IsNullOrWhiteSpace(asOperatorSymbol) Then
                lrFunction = arModel.Function.
                    Where(Function(x) x IsNot Nothing AndAlso
                                      String.Equals(x.OperatorSymbol,
                                                    asOperatorSymbol,
                                                    StringComparison.Ordinal)).
                    OrderBy(Function(x) x.id, StringComparer.OrdinalIgnoreCase).
                    FirstOrDefault()
            End If

            If lrFunction Is Nothing Then
                lrFunction = arModel.Function.
                    Where(Function(x) x IsNot Nothing AndAlso
                                      String.Equals(x.Name,
                                                    asFunctionName,
                                                    StringComparison.OrdinalIgnoreCase)).
                    OrderBy(Function(x) x.id, StringComparer.OrdinalIgnoreCase).
                    FirstOrDefault()
            End If

            If lrFunction Is Nothing Then
                lrFunction = New FBM.Function(arModel) With {
                    .id = NewDerivationId(),
                    .Name = asFunctionName,
                    .OperatorSymbol = asOperatorSymbol,
                    .IsBoolean = abIsBoolean
                }
                For liIndex As Integer = 0 To aiArgumentCount - 1
                    Dim lsParameterName As String
                    If aiArgumentCount = 1 Then
                        lsParameterName = "values"
                    ElseIf aiArgumentCount = 2 AndAlso liIndex = 0 Then
                        lsParameterName = "left"
                    ElseIf aiArgumentCount = 2 Then
                        lsParameterName = "right"
                    Else
                        lsParameterName = "argument" & (liIndex + 1).ToString()
                    End If

                    lrFunction.Parameters.Add(
                        New FBM.Parameter With {
                            .id = NewDerivationId(),
                            .Name = lsParameterName,
                            .BagInput = (liIndex = aiBagInputIndex)
                        })
                Next
                arModel.Function.Add(lrFunction)
            End If

            If abIsBoolean Then lrFunction.IsBoolean = True

            If lrFunction.Parameters Is Nothing OrElse
               lrFunction.Parameters.Count <> aiArgumentCount Then
                Throw New Exception(
                    "Function '" & lrFunction.Name &
                    "' defines " & lrFunction.Parameters.Count.ToString() &
                    " input parameter(s), but the derivation supplies " &
                    aiArgumentCount.ToString() & ".")
            End If

            If aiBagInputIndex >= 0 Then
                If aiBagInputIndex >= lrFunction.Parameters.Count Then
                    Throw New Exception(
                        "Function '" & lrFunction.Name &
                        "' has no parameter for the aggregate bag input.")
                End If
                lrFunction.Parameters(aiBagInputIndex).BagInput = True
            End If

            Return lrFunction

        End Function

        Private Function ResolveFunctionParameter(
            ByVal arFunction As FBM.Function,
            ByVal asParameterName As String,
            ByVal aiFallbackIndex As Integer) As FBM.Parameter

            Dim lrParameter As FBM.Parameter =
                arFunction.Parameters.FirstOrDefault(
                    Function(x) x IsNot Nothing AndAlso
                                String.Equals(x.Name,
                                              asParameterName,
                                              StringComparison.OrdinalIgnoreCase))
            If lrParameter Is Nothing Then
                lrParameter = arFunction.Parameters(aiFallbackIndex)
            End If
            Return lrParameter

        End Function

        Private Sub AddBindingOccurrence(
            ByVal adictOccurrences As Dictionary(
                Of String, List(Of BindingOccurrence)),
            ByVal asBindingKey As String,
            ByVal arOccurrence As BindingOccurrence)

            Dim larOccurrences As List(Of BindingOccurrence) = Nothing
            If Not adictOccurrences.TryGetValue(asBindingKey,
                                                larOccurrences) Then
                larOccurrences = New List(Of BindingOccurrence)
                adictOccurrences(asBindingKey) = larOccurrences
            End If
            larOccurrences.Add(arOccurrence)

        End Sub

        Private Sub AddRequiredObjectUnifiers(
            ByVal arRolePath As FBM.RolePath,
            ByVal arRoot As FBM.RootObjectType,
            ByVal adictOccurrences As Dictionary(
                Of String, List(Of BindingOccurrence)))

            For Each lrPair As KeyValuePair(
                Of String, List(Of BindingOccurrence)) In
                adictOccurrences

                Dim larOccurrences As List(Of BindingOccurrence) =
                    lrPair.Value
                If larOccurrences Is Nothing OrElse
                   larOccurrences.Count < 2 OrElse
                   OccurrencesAreImplicitlyContinuous(larOccurrences) Then
                    Continue For
                End If

                Dim lrUnifier As New FBM.ObjectUnifier With {
                    .id = NewDerivationId()
                }
                If larOccurrences.Any(Function(x) x.IsPathRoot) Then
                    lrUnifier.PathRoots.Add(
                        New FBM.PathRootReference With {
                            .Ref = arRoot.id
                        })
                End If

                For Each lrOccurrence As BindingOccurrence In
                    larOccurrences.
                        Where(Function(x) x.PathedRole IsNot Nothing).
                        OrderBy(Function(x) x.PathIndex)
                    lrUnifier.PathedRoles.Add(
                        New FBM.PathedRoleReference With {
                            .Ref = lrOccurrence.PathedRole.id
                        })
                Next

                If lrUnifier.PathRoots.Count +
                   lrUnifier.PathedRoles.Count >= 2 Then
                    arRolePath.ObjectUnifier.Add(lrUnifier)
                End If
            Next

        End Sub

        Private Function OccurrencesAreImplicitlyContinuous(
            ByVal aarOccurrences As List(Of BindingOccurrence)) As Boolean

            If aarOccurrences Is Nothing OrElse
               aarOccurrences.Count < 2 Then Return True

            Dim lsetVisited As New HashSet(Of Integer)
            Dim lqueuePending As New Queue(Of Integer)
            lsetVisited.Add(0)
            lqueuePending.Enqueue(0)

            While lqueuePending.Count > 0
                Dim liCurrent As Integer = lqueuePending.Dequeue()
                For liCandidate As Integer = 0 To aarOccurrences.Count - 1
                    If lsetVisited.Contains(liCandidate) Then Continue For
                    If OccurrencesHaveImplicitJoin(
                            aarOccurrences(liCurrent),
                            aarOccurrences(liCandidate)) Then
                        lsetVisited.Add(liCandidate)
                        lqueuePending.Enqueue(liCandidate)
                    End If
                Next
            End While

            Return lsetVisited.Count = aarOccurrences.Count

        End Function

        Private Function OccurrencesHaveImplicitJoin(
            ByVal arLeft As BindingOccurrence,
            ByVal arRight As BindingOccurrence) As Boolean

            If arLeft Is Nothing OrElse arRight Is Nothing Then Return False

            If arLeft.IsPathRoot Then
                Return arRight.OccurrenceIndex = 0 AndAlso
                       arRight.RoleIndex = 0
            End If
            If arRight.IsPathRoot Then
                Return arLeft.OccurrenceIndex = 0 AndAlso
                       arLeft.RoleIndex = 0
            End If
            If arLeft.BranchIndex <> arRight.BranchIndex Then Return False

            If arLeft.OccurrenceIndex + 1 = arRight.OccurrenceIndex AndAlso
               arLeft.RoleIndex = arLeft.OccurrenceRoleCount - 1 AndAlso
               arRight.RoleIndex = 0 Then Return True

            Return arRight.OccurrenceIndex + 1 = arLeft.OccurrenceIndex AndAlso
                   arRight.RoleIndex = arRight.OccurrenceRoleCount - 1 AndAlso
                   arLeft.RoleIndex = 0

        End Function

        Private Function SameModelObject(
            ByVal arLeft As FBM.ModelObject,
            ByVal arRight As FBM.ModelObject) As Boolean

            If arLeft Is Nothing OrElse arRight Is Nothing Then
                Return False
            End If
            Return String.Equals(arLeft.Id,
                                 arRight.Id,
                                 StringComparison.OrdinalIgnoreCase)

        End Function

        Private Function ModelReferenceBindingKey(
            ByVal arReference As Derivations.ModelElementReference) As String

            Return arReference.MODEL_ELEMENT_NAME.
                       Trim().ToUpperInvariant() &
                   "|" &
                   If(arReference.MODEL_ELEMENT_ALIAS_NUMBER,
                      "").Trim()

        End Function

        Private Function NormaliseSurfaceToken(
            ByVal asText As String) As String

            Return If(asText, "").Trim().ToLowerInvariant()

        End Function

        Private Function SurfaceTokensText(
            ByVal aarTokens As List(Of SurfaceToken),
            ByVal aiStart As Integer) As String

            If aarTokens Is Nothing OrElse
               aiStart >= aarTokens.Count Then Return ""
            Return String.Join(
                " ",
                aarTokens.Skip(aiStart).
                    Select(Function(x) x.Text))

        End Function

        Private Function NewDerivationId() As String
            Return "_" & Guid.NewGuid().ToString().ToUpperInvariant()
        End Function


    End Class

End Namespace
