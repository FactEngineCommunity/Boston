Imports System.Reflection

Namespace FEQL

    Partial Public Class Processor

        ''' <summary>
        ''' Returns the SQL for the Derivation
        ''' </summary>
        ''' <param name="asDerivationText">The Derivation Text of the FactType</param>
        ''' <param name="arFactType">The FactType for the Derivation Text</param>
        ''' <returns></returns>
        Public Function processDerivationText(ByVal asDerivationText As String,
                                              ByRef arFactType As FBM.FactType,
                                              ByVal ParamArray aarParameterArray As FactEngine.QueryEdge()) As String

            Try

                Me.Parsetree = Me.Parser.Parse(asDerivationText)

                Dim lrDerivationClause As New FEQL.DERIVATIONCLAUSE


                If Me.Parsetree.Errors.Count > 0 Then
                    Throw New Exception("Error in Derivation Text for Fact Type, '" & arFactType.Id & "'.")
                End If

                Call Me.GetParseTreeTokensReflection(lrDerivationClause, Me.Parsetree.Nodes(0))

                'PSEUDOCODE
                'Get the FactTypes for the DerivationSubClauses
                'Analyse/Get the type of Derivation
                'Return the SQL for the Derivation

#Region "Get Fact Types For Each DerivationSubClause"

                For Each lrDerivationSubClause In lrDerivationClause.DERIVATIONSUBCLAUSE

                    If lrDerivationSubClause.isFactTypeOnly Then
#Region "FACTREADING"
                        'Get the FactType for the DerivationSubClause
                        Dim larModelObject As New List(Of FBM.ModelObject)
                        Dim larRole As New List(Of FBM.Role)
                        Dim lrFactTypeReading As New FBM.FactTypeReading
                        Dim lrFactType As New FBM.FactType("DummyFactType")

                        Dim lrSentence As New Language.Sentence("DummySentence")

                        Dim liInd = 0
                        For Each lsModelElementName In lrDerivationSubClause.FACTREADING.MODELELEMENTNAME
                            Dim lrModelObject = Me.Model.GetModelObjectByName(Trim(lsModelElementName).Replace(vbCr, ""))
                            larModelObject.Add(lrModelObject)

                            Dim lrRole = New FBM.Role(lrFactType, lrModelObject)
                            larRole.Add(lrRole)

                            Dim lrPredicatePart = New Language.PredicatePart()
                            Dim lsPredicatePartText = ""
                            If liInd < lrDerivationSubClause.FACTREADING.MODELELEMENTNAME.Count - 1 Then
                                For Each lsPredicatePart In lrDerivationSubClause.FACTREADING.PREDICATECLAUSE(liInd).PREDICATE
                                    lsPredicatePartText &= lsPredicatePart & " "
                                Next
                                lrPredicatePart.PredicatePartText = Trim(lsPredicatePartText)
                            Else
                                lrPredicatePart.PredicatePartText = ""
                            End If
                            lrSentence.PredicatePart.Add(lrPredicatePart)

                            liInd += 1
                        Next

                        lrFactTypeReading = New FBM.FactTypeReading(lrFactType, larRole, lrSentence)

                        lrDerivationSubClause.FBMFactType = Me.Model.getFactTypeByModelObjectsFactTypeReading(larModelObject, lrFactTypeReading)
#End Region
                    ElseIf lrDerivationSubClause.DERIVATIONFORMULA IsNot Nothing Then
                        'Nothing to do at this stage
                    End If

                Next

#End Region

                Select Case Me.getDerivationType(lrDerivationClause)
                    Case Is = FactEngine.pcenumFEQLDerivationType.TransitiveRingConstraintJoin

                        Return Me.getTransitiveRingConstraintJoinSQL(lrDerivationClause, arFactType)

                    Case Is = FactEngine.pcenumFEQLDerivationType.Count
                        arFactType.DerivationType = FactEngine.Constants.pcenumFEQLDerivationType.Count
                        Return Me.getCountDerivationSQL(lrDerivationClause, arFactType)

                    Case Else
                        Return Me.getStraightDerivationSQL(lrDerivationClause, arFactType, aarParameterArray)

                End Select

                Return ""

            Catch ex As Exception
                Throw New Exception(ex.Message)
            End Try

        End Function

        Private Function getStraightDerivationSQL(ByRef arDerivationClause As FEQL.DERIVATIONCLAUSE,
                                                  ByRef arFactType As FBM.FactType,
                                                  ByVal ParamArray aarParameterArray As FactEngine.QueryEdge()) As String

            Dim lsSQL As String = ""

            Try
                lsSQL = "(" & vbCrLf

                Dim lsColumnNames As String = ""

                Dim larFactReading = From DerivationClause In arDerivationClause.DERIVATIONSUBCLAUSE.FindAll(Function(x) x.FACTREADING IsNot Nothing)
                                     Select DerivationClause.FACTREADING

                Dim liInd As Integer = 0
                For Each lrFactReading In larFactReading
                    For Each lsModelElementName In lrFactReading.MODELELEMENTNAME
                        If liInd > 0 Then lsColumnNames &= ", "
                        Dim lrModelElement = arFactType.Model.GetModelObjectByName(lsModelElementName)
                        Select Case lrModelElement.GetType
                            Case GetType(FBM.ValueType)
                                lsColumnNames &= lrModelElement.Id
                            Case Else
                                Dim liInd2 As Integer = 0
                                For Each lrColumn In lrModelElement.getCorrespondingRDSTable.getPrimaryKeyColumns
                                    If liInd2 > 0 Then lsColumnNames &= ", "
                                    lsColumnNames &= lrColumn.Name
                                    liInd2 += 1
                                Next
                        End Select
                        liInd += 1
                    Next
                Next

                lsSQL &= "SELECT " & lsColumnNames & vbCrLf
                lsSQL &= " FROM "
                liInd = 0
                For Each lrDerivationSubClause In arDerivationClause.DERIVATIONSUBCLAUSE.FindAll(Function(x) x.isFactTypeOnly)
                    If liInd > 0 Then lsSQL &= "," & vbCrLf
                    Dim lrTable = lrDerivationSubClause.FBMFactType.getCorrespondingRDSTable
                    If lrTable IsNot Nothing Then
                        lsSQL &= lrTable.Name
                    End If
                    liInd += 1
                Next

                lsSQL &= vbCrLf & "WHERE "

                For Each lrDerivationSubClause In arDerivationClause.DERIVATIONSUBCLAUSE.FindAll(Function(x) Not x.isFactTypeOnly)
                    Dim lrDerivationFormula = lrDerivationSubClause.DERIVATIONFORMULA
                    lsSQL &= Me.walkDerivationFormulaTree(lrDerivationFormula, aarParameterArray)
                Next

                lsSQL &= ") AS " & arFactType.Name & vbCrLf

                Return lsSQL

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)

                Return lsSQL
            End Try

        End Function

        Public Function walkDerivationFormulaTree(ByRef arDerivationFormula As FEQL.ParseNode,
                                                  ByVal ParamArray aarParameterArray As FactEngine.QueryEdge())

            Dim lsSQL As String = ""
            Dim lsTokenText As String = Nothing
            Try
                Dim larBaseNodeParameter = From QueryEdge In aarParameterArray
                                           Where QueryEdge.BaseNode.HasIdentifier
                                           Select QueryEdge.BaseNode

                Dim larTargetNodeParameter = From QueryEdge In aarParameterArray
                                             Where QueryEdge.TargetNode.HasIdentifier
                                             Select QueryEdge.TargetNode

                Dim larQueryNodeParameters As List(Of FactEngine.QueryNode) = larBaseNodeParameter.ToList
                larQueryNodeParameters.AddRange(larTargetNodeParameter.ToList)

                lsSQL &= Me.processNode(arDerivationFormula, larQueryNodeParameters)

                For Each lrParseNode In arDerivationFormula.Nodes
                    lsSQL &= walkDerivationFormulaTree(lrParseNode, aarParameterArray)
                Next

                Return lsSQL
            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)

                Return lsSQL
            End Try

        End Function

        Public Function processNode(ByRef arParseNode As FEQL.ParseNode,
                                    ByRef aarQueryNodeParameters As List(Of FactEngine.QueryNode)) As String

            Dim lsSQL As String = ""
            Dim lsTokenText As String
            Try
                lsTokenText = Nothing

                Select Case arParseNode.Token.Type
                    Case Is = FEQL.TokenType.MODELELEMENTNAME,
                              FEQL.TokenType.EQUALS,
                              FEQL.TokenType.KEYWDLESSTHAN,
                              FEQL.TokenType.KEYWDGREATERTHAN,
                              FEQL.TokenType.PLUS,
                              FEQL.TokenType.MINUS,
                              FEQL.TokenType.TIMES,
                              FEQL.TokenType.DIVIDE,
                              FEQL.TokenType.NUMBER,
                              FEQL.TokenType.BROPEN,
                              FEQL.TokenType.BRCLOSE
                        lsTokenText = Trim(arParseNode.Token.Text)
                    Case Is = FEQL.TokenType.KEYWDTODAY 'Reserved Words
                        lsTokenText = Trim(arParseNode.Token.Text)
                    Case Else
                        Return ""
                End Select

                Dim lrModelElement As FBM.ModelObject
                Select Case arParseNode.Token.Type
                    Case Is = FEQL.TokenType.MODELELEMENTNAME
                        lrModelElement = Me.Model.GetModelObjectByName(lsTokenText)
                        Select Case lrModelElement.GetType
                            Case GetType(FBM.ValueType)
                                Select Case CType(lrModelElement, FBM.ValueType).DataType
                                    Case Is = pcenumORMDataType.TemporalDate
                                        lsTokenText = "julianday(" & lsTokenText & ")"
                                End Select
                        End Select
                End Select

                If lsTokenText IsNot Nothing Then

                    Dim lasParameter = From QueryNode In aarQueryNodeParameters
                                       Where QueryNode.FBMModelObject.Id = lsTokenText
                                       Select QueryNode.IdentifierList(0)
                    'Where NullVal(QueryNode.Alias,"") = 'ToDo

                    If lasParameter.Count > 0 Then
                        lsTokenText = lasParameter.First
                    End If

                    Select Case LCase(lsTokenText)
                        Case Is = "today" 'Reserved word
                            lsTokenText = "julianday('now') "
                        Case Else
                            lsTokenText &= " "
                    End Select
                End If

                lsSQL &= lsTokenText

                Return lsSQL
            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)

                Return lsSQL
            End Try

        End Function

        Private Function getCountDerivationSQL(ByRef arDerivationClause As FEQL.DERIVATIONCLAUSE,
                                               ByRef arFactType As FBM.FactType) As String

            Dim lsSQL As String = ""

            '(SELECT O2.Order_Nr, Count(*) As Arity 
            'From OrderItem O2,
            'Group By O2.Order_Nr) As OrderArity

            lsSQL = "(" & vbCrLf

            Dim lsBaseTableName = arDerivationClause.DERIVATIONSUBCLAUSE(0).FACTREADING.MODELELEMENTNAME(0)
            Dim lrColumn = arDerivationClause.DERIVATIONSUBCLAUSE(0).FBMFactType.getCorrespondingRDSTable.Column.Find(Function(x) x.Role.JoinedORMObject.Id = lsBaseTableName)

            lsSQL &= "SELECT " & lrColumn.Name
            lsSQL &= ", COUNT(*) AS " & arFactType.RoleGroup(1).JoinedORMObject.Id & vbCrLf
            lsSQL &= "FROM " & arDerivationClause.DERIVATIONSUBCLAUSE(0).FBMFactType.getCorrespondingRDSTable.Name & vbCrLf
            lsSQL &= "GROUP BY " & lrColumn.Name
            lsSQL &= ") AS " & arFactType.Name
            Return lsSQL

        End Function

        Private Function getTransitiveRingConstraintJoinSQL(ByRef arDerivationClause As FEQL.DERIVATIONCLAUSE,
                                                            ByRef arFactType As FBM.FactType) As String

            ''(
            ''With RECURSIVE my_tree As (
            ''   Select Case PPA.Protein_Id, PPA.Protein_Id1, PDA.Disease_Id As Disease_Id
            ''   From ProteinProteinAssociation PPA,
            ''        ProteinDiseaseAssociation PDA
            ''   Where PPA.Protein_Id1 = PDA.Protein_Id

            ''                    UNION ALL

            ''   Select Case c.Protein_Id, c.Protein_Id1, p.Disease_Id
            ''   From ProteinProteinAssociation c
            ''     Join my_tree p           
            ''       On p.Protein_Id = c.Protein_Id1
            '') 
            ''Select Case Protein_Id, Disease_Id
            ''From my_tree
            '') PPDA

            Dim lsSQL As String = ""

            lsSQL = "(" & vbCrLf
            lsSQL &= "With RECURSIVE my_tree As (" & vbCrLf
            lsSQL &= "SELECT "

            Dim liInd = 0
            For Each lrColumn In arDerivationClause.DERIVATIONSUBCLAUSE(0).FBMFactType.getCorrespondingRDSTable.Column
                If liInd > 0 Then lsSQL &= ","
                lsSQL &= lrColumn.Table.Name & "." & lrColumn.Name
                liInd += 1
            Next
            lsSQL &= ","
            lsSQL &= arDerivationClause.DERIVATIONSUBCLAUSE(1).FBMFactType.getCorrespondingRDSTable.Name & "."
            lsSQL &= arDerivationClause.DERIVATIONSUBCLAUSE(1).FBMFactType.getCorrespondingRDSTable.Column(1).Name & vbCrLf

            lsSQL &= "FROM "

            lsSQL &= arDerivationClause.DERIVATIONSUBCLAUSE(0).FBMFactType.Id & "," & vbCrLf
            lsSQL &= arDerivationClause.DERIVATIONSUBCLAUSE(1).FBMFactType.Id & vbCrLf

            lsSQL &= "WHERE " & arDerivationClause.DERIVATIONSUBCLAUSE(0).FBMFactType.Id & "."
            lsSQL &= arDerivationClause.DERIVATIONSUBCLAUSE(0).FBMFactType.getCorrespondingRDSTable.Column(1).Name & " = "
            lsSQL &= arDerivationClause.DERIVATIONSUBCLAUSE(1).FBMFactType.Id & "."
            lsSQL &= arDerivationClause.DERIVATIONSUBCLAUSE(1).FBMFactType.getCorrespondingRDSTable.Column(0).Name & vbCrLf

            lsSQL &= "UNION ALL "

            lsSQL &= "SELECT "

            liInd = 0
            For Each lrColumn In arDerivationClause.DERIVATIONSUBCLAUSE(0).FBMFactType.getCorrespondingRDSTable.Column
                If liInd > 0 Then lsSQL &= ","
                lsSQL &= lrColumn.Table.Name & "." & lrColumn.Name
                liInd += 1
            Next
            lsSQL &= ", "
            lsSQL &= "p."
            lsSQL &= arDerivationClause.DERIVATIONSUBCLAUSE(1).FBMFactType.getCorrespondingRDSTable.Column(1).Name & vbCrLf

            lsSQL &= "FROM " & arDerivationClause.DERIVATIONSUBCLAUSE(0).FBMFactType.Id & vbCrLf

            lsSQL &= "JOIN my_tree p" & vbCrLf
            lsSQL &= "ON p." & arDerivationClause.DERIVATIONSUBCLAUSE(0).FBMFactType.getCorrespondingRDSTable.Column(0).Name & " = "

            lsSQL &= arDerivationClause.DERIVATIONSUBCLAUSE(0).FBMFactType.Id & "."
            lsSQL &= arDerivationClause.DERIVATIONSUBCLAUSE(0).FBMFactType.getCorrespondingRDSTable.Column(1).Name & vbCrLf

            lsSQL &= ")" & vbCrLf
            lsSQL &= "SELECT " '&  Protein_Id, Disease_Id

            liInd = 0
            For Each lrColumn In arFactType.getCorrespondingRDSTable.Column
                If liInd > 0 Then lsSQL &= ","
                lsSQL &= lrColumn.Name
                liInd += 1
            Next

            lsSQL &= vbCrLf & "FROM my_tree" & vbCrLf
            lsSQL &= ") " & arFactType.Name

            Return lsSQL


        End Function



        ''' <summary>
        ''' Returns the type of Derivation for the given DerivationClause. E.g. TransitiveRingConstraintJoin
        ''' </summary>
        ''' <param name="arDerivationClause"></param>
        ''' <returns></returns>
        Public Function getDerivationType(ByRef arDerivationClause As FEQL.DERIVATIONCLAUSE) As FactEngine.pcenumFEQLDerivationType

            If arDerivationClause.DERIVATIONSUBCLAUSE.Count = 2 And
               arDerivationClause.DERIVATIONSUBCLAUSE(0).FBMFactType.hasTransitiveRingConstraint Then

                Return FactEngine.pcenumFEQLDerivationType.TransitiveRingConstraintJoin

            ElseIf arDerivationClause.DERIVATIONSUBCLAUSE.Count = 1 And
                   arDerivationClause.KEYWDCOUNT IsNot Nothing Then

                arDerivationClause.DERIVATIONSUBCLAUSE(0).FBMFactType.DerivationType = FactEngine.Constants.pcenumFEQLDerivationType.Count
                Return FactEngine.pcenumFEQLDerivationType.Count

            End If

            Return FactEngine.pcenumFEQLDerivationType.None

        End Function

    End Class

End Namespace

