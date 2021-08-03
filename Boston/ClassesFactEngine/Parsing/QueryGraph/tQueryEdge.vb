Imports System.Reflection

Namespace FactEngine
    Public Class QueryEdge

        Public Id As String = System.Guid.NewGuid.ToString

        Public QueryGraph As FactEngine.QueryGraph = Nothing

        Public WhichClause As FEQL.WHICHCLAUSE

        Public BaseNode As FactEngine.QueryNode = Nothing
        Public TargetNode As FactEngine.QueryNode = Nothing
        Public Predicate As String = ""

        Public IdentifierList As New List(Of String)

        Public IsSubQueryLeader As Boolean = False
        Public IsPartOfSubQuery As Boolean = False
        Public SubQueryAlias As String = ""

        Public FBMFactType As FBM.FactType = Nothing

        Public FBMFactTypeReading As FBM.FactTypeReading = Nothing

        Public FBMPredicatePart As FBM.PredicatePart = Nothing

        Public IsRecursive As Boolean = False
        Public RecursiveNumber1 As String = Nothing
        Public RecursiveNumber2 As String = Nothing
        ''' <summary>
        ''' For graph queries looking for circular paths.
        ''' </summary>
        Public IsCircular As Boolean = False
        ''' <summary>
        ''' For graph queries looking for shortest path.
        ''' </summary>
        Public IsShortestPath As Boolean = False

        ''' <summary>
        ''' 'E.g. WHICH "Person was armed by WHICH Person 2", rather than "WHICH Person armed Person 2" and where the latter is the primary FactTypeReading predicate.
        ''' </summary>
        Public IsReciprocal As Boolean = False

        ''' <summary>
        ''' True if the Predicate is one of many (2 or more) PredicateParts of a Ternary or greater FactType.
        ''' E.g. WHICH Part is in (Bin:'H1')  where 'is in' is a PredicatePart of 'Part is in Bin in Warehouse'.
        ''' </summary>
        Public IsPartialPredicateEdge As Boolean = False

        ''' <summary>
        ''' Used when QueryEdge matches one of the many PredicateParts of a FTR for a FactType with Arity greater than 2
        ''' E.g. "Person visited (Country:'China')" within a larger ternary FactType, "Person visited (Country:'China') within the last 10 months"
        ''' </summary>
        Public FBMPossibleFactTypes As New List(Of FBM.FactType)

        ''' <summary>
        ''' True when the QueryEdge is a match for only part of the FactType.
        ''' E.g. When "Person visited (Country:'China')" is the partial match for a larger Fact Type (Reading)
        ''' 'Person visited (Country:'China') within the last 10 months' as in the query "WHICH Person visited (Country:'China') within the last 10 months"
        ''' </summary>
        Public IsPartialFactTypeMatch As Boolean = False

        ''' <summary>
        ''' Populated on initial parse if more than one Partial FactType Match
        ''' </summary>
        Public AmbiguousFactTypeMatches As New List(Of FBM.FactType)

        Public AmbiguousFactTypeReading As FBM.FactTypeReading

        ''' <summary>
        ''' Populated if Ambiguous FactType Match for Partial FactType Match type QueryEdges. I.e. Partial FactTypeReading is found.
        ''' </summary>
        Public ErrorMessage As String = Nothing

        ''' <summary>
        ''' TRUE if is the project of a select/project
        ''' </summary>
        Public IsProjectColumn As Boolean = False

        Private _Alias As String = Nothing
        ''' <summary>
        ''' As set when setting the QueryGraph.Nodes aliases
        ''' </summary>
        Public Property [Alias] As String
            Get
                If Me.FBMFactType.isRDSTable Then
                    Return Me._Alias
                Else
                    Return Me._Alias
                End If
            End Get
            Set(value As String)
                Me._Alias = value
            End Set
        End Property

        Public WhichClauseType As pcenumWhichClauseType = pcenumWhichClauseType.None
        Public WhichClauseSubType As pcenumWhichClauseType = pcenumWhichClauseType.None 'Used predominantly for IsPredicateNodePropertyIdentification so that the main Type is not changed

        ''' <summary>
        ''' A query such as "WHICH Seat has A Booking THAT is to watch (Film:'Rocky')" will inject a QueryEdge "Session is to watch Film"
        '''   where the model is "Seat has Booking THAT is for Session THAT is to watch (Film:'Rocky')" where Session is an ObjectifiedFactType over
        '''   Film, Cinema and DateTime and where 'Session is to watch Film' is for the corresponding LinkFactType between Session and Film.
        ''' </summary>
        Public InjectsQueryEdge As FactEngine.QueryEdge = Nothing

        ''' <summary>
        ''' Paremeterless New
        ''' </summary>
        Public Sub New()
        End Sub

        Public Sub New(ByRef arQueryGraph As FactEngine.QueryGraph,
                       ByVal arWhichClause As FEQL.WHICHCLAUSE)
            Me.QueryGraph = arQueryGraph
            Me.WhichClause = arWhichClause
        End Sub

        ''' <summary>
        ''' E.g. For the FactEngine query, "Which Lecturer is in which School", arTargetNode points to the 
        ''' QueryNode for the 'School' FBM ModelObject, and arFBMFactType is the FactType with FactType reading "Lecturer is in School"
        ''' </summary>
        ''' <param name="arTargetNode">The node on the far side of the directed graph</param>
        ''' <param name="arFBMFactType">The FBM FactType which has a FactTypeReading matching the FactEngineQL query clause.</param>
        Public Sub New(ByRef arTargetNode As FactEngine.QueryNode,
                       ByRef arFBMFactType As FBM.FactType)

            Me.TargetNode = arTargetNode
            Me.FBMFactType = arFBMFactType

        End Sub

        Public Function getAndSetFBMFactType(ByRef arBaseNode As FactEngine.QueryNode,
                                             ByRef arTargetNode As FactEngine.QueryNode,
                                             ByVal asPredicate As String,
                                             Optional ByVal arPreviousTargetNode As FactEngine.QueryNode = Nothing) As Exception

            Dim lsMessage As String = ""
            Try
                Dim larModelObject As New List(Of FBM.ModelObject)
                Dim larRole As New List(Of FBM.Role)
                Dim lrFactTypeReading As FBM.FactTypeReading
                If Me.WhichClauseSubType = pcenumWhichClauseType.IsPredicateNodePropertyIdentification Then
#Region "With NodePropertyIdentification"
                    larModelObject.Add(arBaseNode.RelativeFBMModelObject)
                    larModelObject.Add(arTargetNode.FBMModelObject)
                    Dim lasPredicatePart As New List(Of String)
                    lasPredicatePart.Add(asPredicate)
                    lasPredicatePart.Add("")

                    Dim lrDummyFactType As New FBM.FactType
                    larRole.Add(New FBM.Role(lrDummyFactType, larModelObject(0)))
                    larRole.Add(New FBM.Role(lrDummyFactType, larModelObject(1)))

                    lrFactTypeReading = New FBM.FactTypeReading(lrDummyFactType, larRole, lasPredicatePart)
                    lrFactTypeReading.PredicatePart(1).PreBoundText = arTargetNode.PreboundText

                    Select Case Me.BaseNode.FBMModelObject.GetType
                        Case Is = GetType(FBM.FactType)

                            '========================================================
                            '20200802-VM-NB Can probably do the following regardless of whether is a FactType
                            Me.FBMFactType = Me.QueryGraph.Model.getFactTypeByModelObjectsFactTypeReading(larModelObject,
                                                                                                          lrFactTypeReading)
                            '========================================================

                            If Me.FBMFactType Is Nothing Then
                                'Thinks its an RDS Table query like "WHICH Lecturer likes WHICH Lecturer"
                                Dim lrRDSFactType As FBM.FactType
                                Select Case Me.BaseNode.FBMModelObject.ConceptType
                                    Case Is = pcenumConceptType.FactType
                                        lrRDSFactType = CType(Me.BaseNode.FBMModelObject, FBM.FactType)
                                    Case Else
                                        Throw New Exception("There is no Fact Type, '" & arBaseNode.FBMModelObject.Id & " " & asPredicate & " " & arTargetNode.FBMModelObject.Id & "', in the Model.")
                                End Select

                                'Still thinks it is a RDS Table
                                Dim liCount = (From FactTypeReading In lrRDSFactType.FactTypeReading
                                               Where FactTypeReading.PredicatePart(0).PredicatePartText = asPredicate
                                               Where FactTypeReading.PredicatePart(0).Role.JoinedORMObject Is larModelObject(0)
                                               Where FactTypeReading.PredicatePart(1).Role.JoinedORMObject Is larModelObject(1)
                                               Select FactTypeReading).Count

                                If liCount = 0 Then
                                    lsMessage = "There is no Fact Type, '" & arBaseNode.FBMModelObject.Id & " " & asPredicate & " " & arTargetNode.FBMModelObject.Id & "', in the Model."

                                    Dim larLinkFactTypeReading = From LinkFactType In lrRDSFactType.getLinkFactTypes
                                                                 From FactTypeReading In LinkFactType.FactTypeReading
                                                                 Where FactTypeReading.PredicatePart(0).Role.JoinedORMObject Is larModelObject(0)
                                                                 Where FactTypeReading.PredicatePart(1).Role.JoinedORMObject Is larModelObject(1)
                                                                 Select FactTypeReading

                                    If larLinkFactTypeReading.Count > 0 Then
                                        lsMessage &= vbCrLf & vbCrLf
                                        lsMessage &= "Did you mean to create a Which Select Clause based on the following predicate reading:" & vbCrLf

                                        For Each lrFactTypeReading In larLinkFactTypeReading.ToList
                                            lsMessage &= vbCrLf & vbTab & lrFactTypeReading.GetReadingTextCQL
                                        Next

                                    End If

                                    Throw New Exception(lsMessage)
                                Else
                                    'Is an RDS Table type query
                                    Me.FBMFactType = Me.BaseNode.FBMModelObject
                                End If
                            End If
                        Case Else
                            'Dim larModelObject As New List(Of FBM.ModelObject)
                            'larModelObject.Add(arBaseNode.FBMModelObject)
                            'larModelObject.Add(arTargetNode.FBMModelObject)
                            'Dim lasPredicatePart As New List(Of String)
                            'lasPredicatePart.Add(asPredicate)
                            'larRole = New List(Of FBM.Role)
                            'Dim lrDummyFactType As New FBM.FactType
                            'larRole.Add(New FBM.Role(lrDummyFactType, larModelObject(0)))
                            'larRole.Add(New FBM.Role(lrDummyFactType, larModelObject(1)))

                            Dim larFactTypeReading = From FactType In Me.QueryGraph.Model.FactType
                                                     From FactTypeReading In FactType.FactTypeReading
                                                     Where FactTypeReading.PredicatePart.Count > 1
                                                     Select FactTypeReading

                            Dim larFinalFactTypeReading As New List(Of FBM.FactTypeReading)

                            For Each lrFactTypeReading2 In larFactTypeReading
                                If lrFactTypeReading2.PredicatePart(0).Role.JoinedORMObject.Id = larRole(0).JoinedORMObject.Id And
                                   lrFactTypeReading2.PredicatePart(1).Role.JoinedORMObject.Id = larRole(1).JoinedORMObject.Id And
                                   lrFactTypeReading2.PredicatePart(1).PreBoundText = Viev.NullVal(arTargetNode.PreboundText, "") Then
                                    larFinalFactTypeReading.Add(lrFactTypeReading2)
                                End If
                            Next

                            Dim larFactType = From FactTypeReading In larFinalFactTypeReading
                                              Select FactTypeReading.FactType Distinct

                            Me.FBMPossibleFactTypes = Me.QueryGraph.Model.getFactTypeByPartialMatchModelObjectsFactTypeReading(larModelObject,
                                                                                                                               lrFactTypeReading)

                            If larFactType.Count > 1 Then
                                lsMessage = "More than one Fact Type has a Predicate Part, '" & asPredicate & "', for Model Elements, '" & arBaseNode.FBMModelObject.Id & " and " & arTargetNode.FBMModelObject.Id & "."
                                lsMessage &= vbCrLf & vbCrLf & "Try referencing the following Fact Types directly in your query:"
                                lsMessage &= vbCrLf & vbCrLf
                                For Each lrFactTypeReading In larFinalFactTypeReading
                                    lsMessage &= "- " & lrFactTypeReading.FactType.Id & vbCrLf
                                Next
                                Throw New Exception(lsMessage)
                            Else
                                If larFinalFactTypeReading.Count = 1 Then
                                    If Me.FBMPossibleFactTypes.Count > 0 Then
                                        Me.FBMFactType = Me.FBMPossibleFactTypes.First
                                        Me.IsPartialFactTypeMatch = True
                                    Else
                                        Me.FBMFactType = larFinalFactTypeReading(0).FactType
                                        If larFinalFactTypeReading(0).FactType.Arity > 2 Then
                                            Me.IsPartialFactTypeMatch = True
                                        End If
                                    End If
                                Else
                                    If Me.FBMPossibleFactTypes.Count = 0 Then
                                        lsMessage = "There is no Predicate Part for any Fact Type that has a Predicate Part, '" & asPredicate & "', for Model Elements, '" & arBaseNode.FBMModelObject.Id & " and " & arTargetNode.FBMModelObject.Id & ". Try revising your query."
                                        Throw New Exception(lsMessage)
                                    ElseIf Me.FBMPossibleFactTypes.Count = 1 Then
                                        Me.FBMFactType = Me.FBMPossibleFactTypes.First
                                        Me.IsPartialFactTypeMatch = True
                                    Else
                                        Me.IsPartialFactTypeMatch = True
                                    End If

                                End If

                            End If

                    End Select
#End Region
                Else
                    Dim lrDummyFactType As New FBM.FactType
                    Dim lasPredicatePart As New List(Of String)

                    larModelObject.Add(arBaseNode.RelativeFBMModelObject)

                    lasPredicatePart.Add(asPredicate)

                    larRole.Add(New FBM.Role(lrDummyFactType, larModelObject(0)))

                    If arTargetNode IsNot Nothing Then
                        larModelObject.Add(arTargetNode.FBMModelObject)
                        lasPredicatePart.Add("")
                        larRole.Add(New FBM.Role(lrDummyFactType, larModelObject(1)))
                    End If

                    lrFactTypeReading = New FBM.FactTypeReading(lrDummyFactType, larRole, lasPredicatePart)
                    lrFactTypeReading.PredicatePart(1).PreBoundText = arTargetNode.PreboundText

                    '---------------------------------------------------------------------------------------------
                    'Get the FactType
                    Me.FBMFactType = Me.QueryGraph.Model.getFactTypeByModelObjectsFactTypeReading(larModelObject,
                                                                                                  lrFactTypeReading)

                    If Me.FBMFactType Is Nothing Then
                        'Try Fastenshtein
                        Me.FBMFactType = Me.QueryGraph.Model.getFactTypeByModelObjectsFactTypeReading(larModelObject,
                                                                                                      lrFactTypeReading, True)
                    End If

                    If Me.FBMFactType Is Nothing Then

                        Dim larFactType As New List(Of FBM.FactType)

                        If larModelObject.Count > 1 Then

                            Dim larAltFactTypeReading = From FactType In Me.QueryGraph.Model.FactType
                                                        From FactTypeReading In FactType.FactTypeReading
                                                        Where FactTypeReading.PredicatePart.Count = 2
                                                        Select FactTypeReading

                            Dim larAltFactType = From FactTypeReading In larAltFactTypeReading
                                                 Where FactTypeReading.PredicatePart(0).Role.JoinedORMObject Is larModelObject(0)
                                                 Where FactTypeReading.PredicatePart(1).Role.JoinedORMObject Is larModelObject(1)
                                                 Where FactTypeReading.PredicatePart(0).PredicatePartText = asPredicate
                                                 Select FactTypeReading.FactType

                            larFactType = larAltFactType.ToList
                        Else
                            Dim larAltFactType = From FactType In Me.QueryGraph.Model.FactType
                                                 From FactTypeReading In FactType.FactTypeReading
                                                 From PredicatePart In FactTypeReading.PredicatePart
                                                 Where PredicatePart.Role.JoinedORMObject.Id = larModelObject(0).Id
                                                 Where PredicatePart.PredicatePartText = asPredicate
                                                 Select FactType

                            larFactType = larAltFactType.ToList
                        End If

                        If larFactType.Count > 1 Then
                            lsMessage = "There is more than one Fact Type Reading that is or starts, '" & arBaseNode.FBMModelObject.Id & " " & asPredicate & " " & arTargetNode.Name & "'"
                            lsMessage &= "or there is no such Fact Type Reading."
                            Throw New Exception(lsMessage)
                        ElseIf larFactType.Count = 0 Then

                            Me.FBMPossibleFactTypes = Me.QueryGraph.Model.getFactTypeByPartialMatchModelObjectsFactTypeReading(larModelObject,
                                                                                                                               lrFactTypeReading)

                            If Me.FBMPossibleFactTypes.Count = 0 Then
                                '----------------------------------------------------------------
                                'Try and get another BaseNode in the query using the followig:
                                '  [querygraph].FindPreviousQueryEdgeBaseNodeByModelElementName
                                '  [Model].getFactTypeReadingByPartialPredicateReading                                
                                Dim larPredicatePart As New List(Of FBM.PredicatePart)
                                lrFactTypeReading = Me.QueryGraph.Model.getFactTypeReadingByPartialPredicateReading(asPredicate, Me.TargetNode.Name, larPredicatePart)
                                If lrFactTypeReading Is Nothing Then
                                    lsMessage = "There is no Fact Type Reading in the Model, '" & arBaseNode.FBMModelObject.Id & " " & asPredicate & " " & arTargetNode.Name & "'"
                                    Throw New Exception(lsMessage)
                                Else
                                    Me.FBMFactType = lrFactTypeReading.FactType
                                    Me.FBMPredicatePart = larPredicatePart(0)
                                    Me.FBMFactTypeReading = Me.FBMPredicatePart.FactTypeReading
                                    'Set the new BaseNode because the original one was fruitless.
                                    Dim lrBaseNode = Me.QueryGraph.FindPreviousQueryEdgeBaseNodeByModelElementName(Me.FBMPredicatePart.Role.JoinedORMObject.Id)
                                    If lrBaseNode IsNot Nothing Then
                                        Me.BaseNode = lrBaseNode
                                    Else
                                        lsMessage = "There is no Fact Type Reading in the Model, '" & arBaseNode.FBMModelObject.Id & " " & asPredicate & " " & arTargetNode.Name & "'"
                                        Throw New Exception(lsMessage)
                                    End If
                                End If

                            ElseIf Me.FBMPossibleFactTypes.Count = 1 Then
                                'Check if is PartialPredicate of a FactTypeReading with more than 2 PredicateParts
                                If Me.FBMPossibleFactTypes.First.RoleGroup.Count > 2 Then
                                    Me.IsPartialFactTypeMatch = True
                                    Me.FBMFactType = Me.FBMPossibleFactTypes.First
                                    Dim lsModelElementId As String
                                    If arPreviousTargetNode Is Nothing Then
                                        lsModelElementId = larModelObject(0).Id
                                    Else
                                        lsModelElementId = arPreviousTargetNode.Name
                                    End If
                                    Me.FBMFactTypeReading = (From FactTypeReading In Me.FBMPossibleFactTypes(0).FactTypeReading
                                                             From PredicatePart In FactTypeReading.PredicatePart
                                                             Where PredicatePart.Role.JoinedORMObject.Id = lsModelElementId
                                                             Where PredicatePart.PredicatePartText = asPredicate
                                                             Select FactTypeReading
                                                            ).First

                                    Me.FBMPredicatePart = (From FactTypeReading In Me.FBMPossibleFactTypes(0).FactTypeReading
                                                           From PredicatePart In FactTypeReading.PredicatePart
                                                           Where PredicatePart.Role.JoinedORMObject.Id = lsModelElementId
                                                           Where PredicatePart.PredicatePartText = asPredicate
                                                           Select PredicatePart
                                                          ).First
                                End If
                            Else
                                Me.AmbiguousFactTypeMatches = FBMPossibleFactTypes
                                lsMessage = "Ambiguous predicate reading, '" & arBaseNode.FBMModelObject.Id & " " & asPredicate & " " & arTargetNode.Name & "'."
                                lsMessage &= vbCrLf & vbCrLf & "There is more than one Fact Type with this predicate part."
                                Me.ErrorMessage = lsMessage
                            End If
                        Else
                            Me.FBMFactType = larFactType.First
                        End If
                    End If

                    If Me.FBMFactType Is Nothing And Me.FBMPossibleFactTypes.Count = 0 Then

                        'See if it is a ReifiedFactType LinkFactType predicate requring an InjectedQueryEdge
                        Dim lrPredicatePart As FBM.PredicatePart
                        Dim lrQueryEdge As New FactEngine.QueryEdge
                        lrQueryEdge.QueryGraph = Me.QueryGraph
                        Dim lrFactType As FBM.FactType = Me.QueryGraph.Model.getFactTypeByPredicateFarSideModelElement(asPredicate, arTargetNode.FBMModelObject)
                        If lrFactType IsNot Nothing Then
                            Dim lrCandidateBaseNode As FactEngine.QueryNode
                            lrPredicatePart = (From FactTypeReading In lrFactType.FactTypeReading
                                               From PredicatePart In FactTypeReading.PredicatePart
                                               Where PredicatePart.PredicatePartText = asPredicate
                                               Select PredicatePart).First

                            lrCandidateBaseNode = New FactEngine.QueryNode(lrPredicatePart.Role.JoinedORMObject, lrQueryEdge)

                            If Me.QueryGraph.Model.hasCountFactTypesBetweenModelElements(arBaseNode.FBMModelObject, lrCandidateBaseNode.FBMModelObject) = 1 Then
                                lrQueryEdge.BaseNode = lrCandidateBaseNode
                                lrQueryEdge.TargetNode = New FactEngine.QueryNode(arTargetNode.FBMModelObject, lrQueryEdge, True)
                                lrQueryEdge.FBMFactType = lrFactType
                                'Change the TargetNode to the lrCandidateBaseNode
                                arTargetNode = New FactEngine.QueryNode(lrCandidateBaseNode.FBMModelObject, Me, True)
                                'get the FactType
                                lrFactTypeReading.PredicatePart.Last.Role.JoinedORMObject = arTargetNode.FBMModelObject
                                Me.FBMFactType = Me.QueryGraph.Model.getFactTypeByModelObjectsFactTypeReading(larModelObject, lrFactTypeReading)
                            Else
                                Throw New Exception("There is not Fact Type, '" & arBaseNode.FBMModelObject.Id & " " & asPredicate & " " & arTargetNode.FBMModelObject.Id & "', in the Model.")
                            End If
                        Else
                            Throw New Exception("There is not Fact Type, '" & arBaseNode.FBMModelObject.Id & " " & asPredicate & " " & arTargetNode.FBMModelObject.Id & "', in the Model.")
                        End If

                    ElseIf Me.FBMPossibleFactTypes.Count > 0 Then
                        'Is only a partial match
                        'E.g. The following is for a ternary FactType:
                        '"WHICH Person visited (Country:'China') within the last 10 months"

                        Me.IsPartialFactTypeMatch = True

                    End If

                    If Me.FBMFactTypeReading Is Nothing And Me.FBMFactType IsNot Nothing Then
                        Me.FBMFactTypeReading = Me.FBMFactType.FactTypeReading.Find(AddressOf lrFactTypeReading.EqualsByPredicatePartText)
                        If Me.FBMFactTypeReading Is Nothing Then
                            Me.FBMFactTypeReading = Me.FBMFactType.FactTypeReading.Find(Function(x) lrFactTypeReading.EqualsByPredicatePartText(x, True) And
                                                                                                    lrFactTypeReading.EqualsByRoleJoinedModelObjectSequence(x))
                        End If
                    ElseIf Me.AmbiguousFactTypeMatches.Count > 0 Then
                        Me.AmbiguousFactTypeReading = lrFactTypeReading
                    End If

                End If

            Catch ex As Exception
                Throw New Exception(ex.Message)
                'prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

            Return Nothing
        End Function

        Public Function GetNextQueryEdge() As FactEngine.QueryEdge

            Try

                If Me.QueryGraph.QueryEdges.IndexOf(Me) < Me.QueryGraph.QueryEdges.Count - 1 Then
                    Return Me.QueryGraph.QueryEdges(Me.QueryGraph.QueryEdges.IndexOf(Me) + 1)
                Else
                    Return Nothing
                End If

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)

                Return Nothing
            End Try

        End Function

        Public Function GetPreviousQueryEdge() As FactEngine.QueryEdge

            Try

                If Me.QueryGraph.QueryEdges.IndexOf(Me) > 0 Then
                    Return Me.QueryGraph.QueryEdges(Me.QueryGraph.QueryEdges.IndexOf(Me) - 1)
                Else
                    Return Nothing
                End If

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)

                Return Nothing
            End Try

        End Function

        Public Function HasPreviousQueryEdge() As Boolean

            Try

                Return Me.QueryGraph.QueryEdges.IndexOf(Me) > 0

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)

                Return Nothing
            End Try

        End Function

    End Class

End Namespace
