Imports System.Reflection

Namespace FactEngine

    <Serializable>
    Public Class QueryEdge
        Implements IEquatable(Of FactEngine.QueryEdge)

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

        Public ReadOnly Property IsRDSTable As Boolean
            Get
                If Me.FBMFactType Is Nothing Then
                    Return False
                Else
                    Return Me.FBMFactType.isRDSTable
                End If
            End Get
        End Property

        Public ReadOnly Property IsDerived As Boolean
            Get
                If Me.FBMFactType Is Nothing Then
                    Return False
                Else
                    Return Me.FBMFactType.IsDerived
                End If
            End Get
        End Property

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

        Public Function Equals(other As QueryEdge) As Boolean Implements IEquatable(Of QueryEdge).Equals
            Return Me.Id = other.Id
        End Function

        ''' <summary>
        ''' Used for QueryEdge.getAndSetFBMFactType
        ''' </summary>
        ''' <param name="arBaseNode">The BaseNode for the FactTypeReading</param>
        ''' <param name="arTargetNode">The TargetNode for he FactTypeReading</param>
        ''' <param name="asPredicate">THe Predicate for the FactTypeReading</param>
        ''' <param name="aarModelObject">The created list of ModelObjects</param>
        ''' <param name="aarRole">The created list of Roles.</param>
        ''' <returns></returns>
        Private Function CreateFBMFactTypeReading(ByRef arBaseNode As FactEngine.QueryNode,
                                                  ByRef arTargetNode As FactEngine.QueryNode,
                                                  ByVal asPredicate As String,
                                                  ByRef aarModelObject As List(Of FBM.ModelObject),
                                                  ByRef aarRole As List(Of FBM.Role)) As FBM.FactTypeReading

            Try
                Dim larModelObject As New List(Of FBM.ModelObject)
                Dim larRole As New List(Of FBM.Role)

                larModelObject.Add(arBaseNode.FBMModelObject)

                Dim lasPredicatePart As New List(Of String)
                lasPredicatePart.Add(asPredicate)

                If arTargetNode IsNot Nothing Then
                    larModelObject.Add(arTargetNode.FBMModelObject)
                    lasPredicatePart.Add("")
                End If

                Dim lrDummyFactType As New FBM.FactType
                larRole.Add(New FBM.Role(lrDummyFactType, larModelObject(0)))
                larRole.Add(New FBM.Role(lrDummyFactType, larModelObject(1)))

                aarModelObject = larModelObject
                aarRole = larRole

                Return New FBM.FactTypeReading(lrDummyFactType, larRole, lasPredicatePart)

            Catch ex As Exception
                Throw New Exception(ex.Message)
            End Try


        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="arBaseNode"></param>
        ''' <param name="arTargetNode"></param>
        ''' <param name="asPredicate"></param>
        ''' <param name="arPreviousTargetNode"></param>
        ''' <param name="abUsePreviousFoundBaseNodeIfFound">Predominantly used from within the FactEngine form itself when retrieving Identifiers, before QueryGraph has been created.</param>
        ''' <returns></returns>
        Public Function getAndSetFBMFactType(ByRef arBaseNode As FactEngine.QueryNode,
                                             ByRef arTargetNode As FactEngine.QueryNode,
                                             ByVal asPredicate As String,
                                             Optional ByVal arPreviousTargetNode As FactEngine.QueryNode = Nothing,
                                             Optional ByVal abUsePreviousFoundBaseNodeIfFound As Boolean = False) As Exception

            Dim lsMessage As String = ""
            Try
                Dim lrFactTypeReading As FBM.FactTypeReading
                Dim lrOriginalFactTypeReading As FBM.FactTypeReading
                Dim larModelObject As New List(Of FBM.ModelObject)
                Dim larRole As New List(Of FBM.Role)
                Dim lrPredicatePart As FBM.PredicatePart
                Dim lrQueryEdge As New FactEngine.QueryEdge
                Dim larFactType As List(Of FBM.FactType)
                Dim lrFactType As FBM.FactType
                Dim lrBaseNode As FactEngine.QueryNode = Nothing

                lrFactTypeReading = Me.CreateFBMFactTypeReading(arBaseNode, arTargetNode, asPredicate, larModelObject, larRole)
                lrFactTypeReading.PredicatePart(1).PreBoundText = arTargetNode.PreboundText

                '========================================================
                Dim lrReturnFactTypeReading As FBM.FactTypeReading = Nothing
                Dim lrReturnPredicatePart As FBM.PredicatePart = Nothing

                Me.FBMFactType = Me.QueryGraph.Model.getFactTypeByModelObjectsFactTypeReading(larModelObject,
                                                                                              lrFactTypeReading, False,
                                                                                              lrReturnFactTypeReading,
                                                                                              lrReturnPredicatePart)

                '======================================================
                'Old code, trying to get rid of this.
#Region "With NodePropertyIdentification"
                If 1 = 2 And Me.WhichClauseSubType = pcenumWhichClauseType.IsPredicateNodePropertyIdentification Then

                    Select Case Me.BaseNode.FBMModelObject.GetType
                        Case Is = GetType(FBM.FactType)

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

                            lrReturnFactTypeReading = Nothing
                            lrPredicatePart = Nothing

                            Me.FBMFactType = Me.QueryGraph.Model.getFactTypeByModelObjectsFactTypeReading(larModelObject,
                                                                                                          lrFactTypeReading, False, lrReturnFactTypeReading, lrPredicatePart)
                            If Me.FBMFactType Is Nothing Then
                                'Try Fastenshtein
                                Me.FBMFactType = Me.QueryGraph.Model.getFactTypeByModelObjectsFactTypeReading(larModelObject,
                                                                                                              lrFactTypeReading, True, lrReturnFactTypeReading, lrPredicatePart)
                            End If

                            Me.FBMFactTypeReading = lrReturnFactTypeReading
                            Me.FBMPredicatePart = lrPredicatePart

                            If Me.FBMFactType Is Nothing Then
                                Dim larFactTypeReading = From FactType In Me.QueryGraph.Model.FactType
                                                         From FactTypeReading In FactType.FactTypeReading
                                                         Where FactType.Arity = 2
                                                         Where FactTypeReading.PredicatePart.All(Function(x) larModelObject.Contains(x.Role.JoinedORMObject))
                                                         Select FactTypeReading

                                Dim larFinalFactTypeReading As New List(Of FBM.FactTypeReading)

                                For Each lrFactTypeReading2 In larFactTypeReading
                                    If lrFactTypeReading2.PredicatePart(0).Role.JoinedORMObject.Id = larRole(0).JoinedORMObject.Id And
                                   lrFactTypeReading2.PredicatePart(1).Role.JoinedORMObject.Id = larRole(1).JoinedORMObject.Id And
                                   lrFactTypeReading2.PredicatePart(1).PreBoundText = Viev.NullVal(arTargetNode.PreboundText, "") Then
                                        larFinalFactTypeReading.Add(lrFactTypeReading2)
                                    End If
                                Next

                                larFactType = From FactTypeReading In larFinalFactTypeReading
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
                                            If larFinalFactTypeReading.Count = 1 Then
                                                Me.FBMFactTypeReading = larFinalFactTypeReading(0)
                                            End If
                                            If Me.FBMFactType.Arity > 2 Then
                                                Me.IsPartialFactTypeMatch = True
                                            End If
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
                                        ElseIf Me.FBMPossibleFactTypes.Count > 1 And Me.FBMFactType Is Nothing Then
                                            lsMessage = "More than one Fact Type has a Predicate Part, '" & asPredicate & "', for Model Elements, '" & arBaseNode.FBMModelObject.Id & " and " & arTargetNode.FBMModelObject.Id & "."
                                            lsMessage &= vbCrLf & vbCrLf & "Try referencing the following Fact Types directly in your query:"
                                            lsMessage &= vbCrLf & vbCrLf
                                            For Each lrFactType In Me.FBMPossibleFactTypes
                                                lsMessage &= "- " & lrFactType.Id & vbCrLf
                                            Next
                                            Throw New Exception(lsMessage)
                                        Else
                                            Me.IsPartialFactTypeMatch = True
                                        End If

                                    End If

                                End If
                            End If

                    End Select

                End If
#End Region

                '========================================================================================================================
                'Straight clean find
#Region "Straight clean find"
                If Me.FBMFactType IsNot Nothing And lrReturnFactTypeReading IsNot Nothing And lrReturnPredicatePart IsNot Nothing Then

                    Me.FBMFactTypeReading = lrReturnFactTypeReading
                    Me.FBMPredicatePart = lrReturnPredicatePart

                    Return Nothing
                End If
#End Region

                '========================================================================================================================
                'Fastenstein - Straight
#Region "Fastenstein - Straight"
                Me.FBMFactType = Me.QueryGraph.Model.getFactTypeByModelObjectsFactTypeReading(larModelObject,
                                                                                              lrFactTypeReading, True,
                                                                                              lrReturnFactTypeReading,
                                                                                              lrReturnPredicatePart)

                If Me.FBMFactType IsNot Nothing And lrReturnFactTypeReading IsNot Nothing And lrReturnPredicatePart IsNot Nothing Then

                    Me.FBMFactTypeReading = lrReturnFactTypeReading
                    Me.FBMPredicatePart = lrReturnPredicatePart

                    Return Nothing
                End If
#End Region

                '====================================================================================================================
                'Partial FactType matches
#Region "Partial FactType matches"
                lrBaseNode = Nothing

                lrOriginalFactTypeReading = lrFactTypeReading

                Me.FBMPossibleFactTypes = Me.QueryGraph.Model.getFactTypeByPartialMatchModelObjectsFactTypeReading(larModelObject,
                                                                                                                   lrFactTypeReading)

                Dim lbAmbiguousPreviousEdgeToResolve As Boolean = False
                If Me.GetPreviousQueryEdge IsNot Nothing Then
                    If Me.GetPreviousQueryEdge.AmbiguousFactTypeMatches.Count > 0 And
                       Me.GetPreviousQueryEdge.IsPartialFactTypeMatch Then
                        lbAmbiguousPreviousEdgeToResolve = True
                    End If
                End If

                If Me.FBMPossibleFactTypes.Count = 1 Or (Me.FBMPossibleFactTypes.Count > 0 And lbAmbiguousPreviousEdgeToResolve) Then
                    '-----------------------------------------------------------------------
                    'Is a partial match. E.g. The following is for a ternary FactType:
                    '"WHICH Person visited (Country:'China') within the last 10 months"
                    'Check if is PartialPredicate of a FactTypeReading with more than 2 PredicateParts
                    Me.IsPartialFactTypeMatch = True

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

                    If Me.FBMPossibleFactTypes.Count = 1 And lbAmbiguousPreviousEdgeToResolve Then
                        'Resolve the previous QueryEdge FactType
                        Dim lrPreviousQueryEdge As FactEngine.QueryEdge = Me.GetPreviousQueryEdge
                        lrPreviousQueryEdge.FBMFactType = Me.FBMFactType
                        lrPreviousQueryEdge.FBMFactTypeReading = Me.FBMFactTypeReading
                        lrPreviousQueryEdge.FBMPredicatePart = (From FactTypeReading In Me.FBMPossibleFactTypes(0).FactTypeReading
                                                                From PredicatePart In FactTypeReading.PredicatePart
                                                                Where PredicatePart.Role.JoinedORMObject.Id = lrPreviousQueryEdge.BaseNode.FBMModelObject.Id
                                                                Where PredicatePart.PredicatePartText = lrPreviousQueryEdge.Predicate
                                                                Select PredicatePart
                                                                ).First
                        lrPreviousQueryEdge.AmbiguousFactTypeMatches = New List(Of FBM.FactType)
                        lrPreviousQueryEdge.ErrorMessage = Nothing
                    ElseIf Me.FBMPossibleFactTypes.Count > 1 Then
                        Me.AmbiguousFactTypeReading = lrFactTypeReading
                        Me.AmbiguousFactTypeMatches = Me.FBMPossibleFactTypes
                    End If

                ElseIf Me.FBMPossibleFactTypes.Count > 1 Then
                    '--------------------------------------------------------------------
                    'Is a partial match. E.g. The following is for a ternary FactType:
                    '"WHICH Person visited (Country:'China') within the last 10 months"
                    Me.IsPartialFactTypeMatch = True
                    Me.AmbiguousFactTypeReading = lrFactTypeReading
                    Me.AmbiguousFactTypeMatches = FBMPossibleFactTypes
                    lsMessage = "Ambiguous predicate reading, '" & arBaseNode.FBMModelObject.Id & " " & asPredicate & " " & arTargetNode.Name & "'."
                    lsMessage &= vbCrLf & vbCrLf & "There is more than one Fact Type with this predicate part."
                    Me.ErrorMessage = lsMessage
                    If Me.GetPreviousQueryEdge Is Nothing Then
                        'Ambiguous, but might be resolved by the next PartialFactType match.
                        Me.FBMFactType = Me.FBMPossibleFactTypes.First
                        Return Nothing
                    ElseIf Not Me.GetPreviousQueryEdge.IsPartialFactTypeMatch Then
                        'Ambiguous, but might be resolved by the next PartialFactType match.
                        Me.FBMFactType = Me.FBMPossibleFactTypes.First
                        Return Nothing
                    End If
                    'Throw New Exception(Me.ErrorMessage)
                End If

                    'Return if successful
                    If Me.FBMFactType IsNot Nothing And Me.FBMFactTypeReading IsNot Nothing And Me.FBMPredicatePart IsNot Nothing Then
                    Me.IsPartialFactTypeMatch = True
                    Return Nothing
                End If
#End Region

                '========================================================================================================================
                'Ambiguity
#Region "Ambiguity"
                larFactType = New List(Of FBM.FactType)

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
                    Me.AmbiguousFactTypeReading = lrFactTypeReading
                    Throw New Exception(lsMessage)
                ElseIf larFactType.Count = 1 Then
                    Me.FBMFactType = larFactType.First
                    'FactTypeReading, PredicatePart at end
                End If

                'Vic...put jump to end
                If Me.FBMFactType IsNot Nothing Then

                    GoTo FinalCleanup
                End If
#End Region


                '====================================================================================================================
                'Previous QueryEdge BaseNode
                '  [Model].getFactTypeReadingByPartialPredicateReading                                
                '  [querygraph].FindPreviousQueryEdgeBaseNodeByModelElementName
#Region "Previous QuerEdge BaseNode"
                Dim larPredicatePart As New List(Of FBM.PredicatePart)
                lrFactTypeReading = Me.QueryGraph.Model.getFactTypeReadingByPartialPredicateReading(asPredicate, Me.TargetNode.Name, larPredicatePart)

                If lrFactTypeReading IsNot Nothing Then
                    '-------------------------------------------------------------------------------------------
                    Me.FBMFactType = lrFactTypeReading.FactType
                    Me.FBMPredicatePart = larPredicatePart(0)
                    Me.FBMFactTypeReading = Me.FBMPredicatePart.FactTypeReading

                    '20210910-VM-Added isPartial... below. If this is wrong remove it.
                    Me.IsPartialFactTypeMatch = True
                    'Set the new BaseNode because the original one was fruitless.
                    lrBaseNode = Me.QueryGraph.FindPreviousQueryEdgeBaseNodeByModelElementName(Me.FBMPredicatePart.Role.JoinedORMObject.Id, Me.BaseNode)
                    If lrBaseNode IsNot Nothing Then
                        Me.BaseNode = lrBaseNode
                    End If
                    '-------------------------------------------------------------------------------------------
                End If
                'Return if successful
                If Me.FBMFactType IsNot Nothing And Me.FBMFactTypeReading IsNot Nothing And Me.FBMPredicatePart IsNot Nothing Then
                    Return Nothing
                End If
#End Region


                '==================================================================================================================
                'Far Side ModelElement predicate
#Region "Far Side ModelElement predicate"
                lrFactType = Me.QueryGraph.Model.getFactTypeByPredicateFarSideModelElement(asPredicate, arTargetNode.FBMModelObject, True, Me.QueryGraph.getNodeModelElementList)

                If lrFactType IsNot Nothing Then
                    Dim lrModelElement As FBM.ModelObject = arTargetNode.FBMModelObject
                    Dim larFactTypeReading = From FactTypeReading In lrFactType.FactTypeReading
                                             Where FactTypeReading.PredicatePart(1).Role.JoinedORMObject.Id = lrModelElement.Id
                                             Select FactTypeReading

                    lrFactTypeReading = larFactTypeReading.First
                    Me.FBMFactType = lrFactTypeReading.FactType
                    Me.FBMFactTypeReading = lrFactTypeReading
                    Me.FBMPredicatePart = Me.FBMFactTypeReading.PredicatePart(0)
                    lrBaseNode = Me.QueryGraph.FindPreviousQueryEdgeBaseNodeByModelElementName(Me.FBMPredicatePart.Role.JoinedORMObject.Id, Me.BaseNode)
                    If lrBaseNode IsNot Nothing Then
                        Me.BaseNode = lrBaseNode
                    ElseIf lrBaseNode Is Nothing And abUsePreviousFoundBaseNodeIfFound Then
                        Me.BaseNode = New FactEngine.QueryNode(Me.FBMFactTypeReading.PredicatePart(0).Role.JoinedORMObject, Me, False)
                    Else
                        Me.FBMFactType = Nothing
                        Me.FBMFactTypeReading = Nothing
                        'Me.FBMPredicatePart = Nothing
                        lrFactTypeReading = Nothing
                    End If

                    Try
                        lrBaseNode = Me.QueryGraph.FindPreviousQueryEdgeBaseNodeByModelElementName(Me.FBMPredicatePart.Role.JoinedORMObject.Id, Me.BaseNode)
                    Catch
                        lrBaseNode = Nothing
                    End Try

                    If lrBaseNode IsNot Nothing Then
                        Me.BaseNode = lrBaseNode
                    End If
                End If

                'Return if successful
                If Me.FBMFactType IsNot Nothing And Me.FBMFactTypeReading IsNot Nothing And Me.FBMPredicatePart IsNot Nothing Then
                    Return Nothing
                End If
#End Region

                '===================================================================================================================================
                'ReifiedFactType LinkFactType predicate requring an InjectedQueryEdge
#Region "ReifiedFactType LinkFactType predicate requring an InjectedQueryEdge"
                lrQueryEdge.QueryGraph = Me.QueryGraph
                lrQueryEdge.WhichClause = Me.WhichClause
                lrQueryEdge.WhichClauseType = Me.WhichClauseType
                lrFactType = Me.QueryGraph.Model.getFactTypeByPredicateFarSideModelElement(asPredicate, arTargetNode.FBMModelObject)
                If lrFactType Is Nothing Then
                    lrFactType = Me.QueryGraph.Model.getFactTypeByPredicateFarSideModelElement(asPredicate, arTargetNode.FBMModelObject, True)
                End If

                If lrFactType IsNot Nothing Then
                    Dim lrCandidateBaseNode As FactEngine.QueryNode
                    lrPredicatePart = (From FactTypeReading In lrFactType.FactTypeReading
                                       From PredicatePart In FactTypeReading.PredicatePart
                                       Where PredicatePart.PredicatePartText = asPredicate Or
                                                                 Fastenshtein.Levenshtein.Distance(PredicatePart.PredicatePartText, asPredicate) < 4
                                       Select PredicatePart).First

                    lrCandidateBaseNode = New FactEngine.QueryNode(lrPredicatePart.Role.JoinedORMObject, lrQueryEdge)

                    If Me.QueryGraph.Model.hasCountFactTypesBetweenModelElements(arBaseNode.FBMModelObject, lrCandidateBaseNode.FBMModelObject) = 0 Then
                        If Me.QueryGraph.Model.hasCountFactTypesBetweenModelElements(arPreviousTargetNode.FBMModelObject, lrCandidateBaseNode.FBMModelObject) = 1 Then
                            arBaseNode = arPreviousTargetNode
                        End If
                    End If

                    If Me.QueryGraph.Model.hasCountFactTypesBetweenModelElements(arBaseNode.FBMModelObject, lrCandidateBaseNode.FBMModelObject) = 1 Then
                        lrQueryEdge.BaseNode = lrCandidateBaseNode
                        lrQueryEdge.TargetNode = New FactEngine.QueryNode(arTargetNode.FBMModelObject, lrQueryEdge, True)
                        If arTargetNode.HasIdentifier Then
                            lrQueryEdge.TargetNode.IdentifierList = arTargetNode.IdentifierList.ToList
                            lrQueryEdge.TargetNode.HasIdentifier = True
                        End If
                        lrQueryEdge.FBMFactType = lrFactType
                        lrQueryEdge.WhichClause = New FEQL.WHICHCLAUSE
                        If Me.IdentifierList.Count > 0 Then
                            lrQueryEdge.IdentifierList = Me.IdentifierList.ToList
                            Me.IdentifierList.Clear()
                        End If
                        'Change the TargetNode to the lrCandidateBaseNode
                        Me.TargetNode = New FactEngine.QueryNode(lrCandidateBaseNode.FBMModelObject, Me, True)
                        'get the FactType
                        lrOriginalFactTypeReading.PredicatePart.Last.Role.JoinedORMObject = arTargetNode.FBMModelObject
                        larModelObject(1) = arTargetNode.FBMModelObject

                        'Because is only one FactType
                        Me.FBMFactTypeReading = (From FactTypeReading In Me.BaseNode.FBMModelObject.getOutgoingFactTypeReadings
                                                 Where FactTypeReading.PredicatePart(0).Role.JoinedORMObject Is Me.BaseNode.FBMModelObject
                                                 Where FactTypeReading.PredicatePart(1).Role.JoinedORMObject Is Me.TargetNode.FBMModelObject
                                                 Select FactTypeReading).First

                        Me.FBMFactType = Me.FBMFactTypeReading.FactType

                        'SubQuery
                        If Me.QueryGraph.QueryEdges.Count > 0 Then
                            If Me.QueryGraph.QueryEdges.Last.IsPartOfSubQuery Or Me.QueryGraph.QueryEdges.Last.IsSubQueryLeader Then
                                lrQueryEdge.IsPartOfSubQuery = True
                                lrQueryEdge.SubQueryAlias = lrQueryEdge.QueryGraph.QueryEdges.Last.SubQueryAlias
                            End If
                        End If

                        Me.InjectsQueryEdge = lrQueryEdge
                    End If
                End If

                'Return if successful
                If Me.FBMFactType IsNot Nothing And Me.FBMFactTypeReading IsNot Nothing And Me.FBMPredicatePart IsNot Nothing Then
                    Return Nothing
                End If
#End Region

                '====================================================================================================================
                'Relative BaseNode - Uses GetTopmostNonAbsorbedSupertype
                'Not yet implemented
                '20210829-VM-Use the below for SubType predicates.
                'FBM.ModelObject.hasModelElementAsDownstreamSubtype

                larModelObject.Add(arBaseNode.RelativeFBMModelObject)

                '====================================================================================================================
                'Tidy up and final chance
FinalCleanup:
#Region "Tidy up and final chance"
                If Me.FBMFactType Is Nothing Then
                    Throw New Exception("There is no Fact Type, '" & arBaseNode.FBMModelObject.Id & " " & asPredicate & " " & arTargetNode.FBMModelObject.Id & "', in the Model.")
                End If

                If Me.FBMFactType IsNot Nothing And Me.FBMFactTypeReading Is Nothing Then
                    Me.FBMFactTypeReading = Me.FBMFactType.FactTypeReading.Find(AddressOf lrFactTypeReading.EqualsByPredicatePartText)
                    If Me.FBMFactTypeReading Is Nothing Then
                        Me.FBMFactTypeReading = Me.FBMFactType.FactTypeReading.Find(Function(x) lrFactTypeReading.EqualsByPredicatePartText(x, True) And
                                                                                                        lrFactTypeReading.EqualsByRoleJoinedModelObjectSequence(x))
                    End If
                    If Me.FBMPredicatePart Is Nothing And Me.FBMFactTypeReading IsNot Nothing Then
                        larPredicatePart = From PredicatePart In Me.FBMFactTypeReading.PredicatePart
                                           Where PredicatePart.PredicatePartText = asPredicate
                                           Select PredicatePart

                        If larPredicatePart.Count > 0 Then
                            Me.FBMPredicatePart = larPredicatePart.First
                        End If
                    End If
                End If
#End Region

                Return Nothing


            Catch ex As Exception
                Dim lrApplicationException As New ApplicationException(ex.Message)
                lrApplicationException.Data.Add("QueryEdgeGetFBMFactTypeFail", Me)
                Throw lrApplicationException
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
                    If Not Me.QueryGraph.QueryEdges.Contains(Me) Then
                        'QueryEdge possibly hasn't been added to the QueryGraph yet
                        If Me.QueryGraph.QueryEdges.Count > 0 Then
                            Return Me.QueryGraph.QueryEdges.Last
                        Else
                            Return Nothing
                        End If
                    Else
                        Return Nothing
                    End If
                End If

                Return Nothing

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)

                Return Nothing
            End Try

        End Function

        Public Function getTargetSQLComparator() As String

            Try

                Select Case Me.TargetNode.Comparitor
                    Case Is = FEQL.pcenumFEQLComparitor.Bang
                        Return " <> "
                    Case Is = FEQL.pcenumFEQLComparitor.Colon,
                              FEQL.pcenumFEQLComparitor.Carret
                        Return " = "
                    Case Is = FEQL.pcenumFEQLComparitor.LikeComparitor
                        Return " LIKE "
                End Select


            Catch ex As Exception
                Throw New Exception(ex.Message)
            End Try

            Return " = "
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

        Public Function RDSColumn() As RDS.Column

            Try
                Dim lrColumn As RDS.Column = Nothing
                Dim lrFactType As FBM.FactType = Nothing

                Select Case Me.BaseNode.FBMModelObject.GetType
                    Case GetType(FBM.FactType)
                        If Me.WhichClauseType = pcenumWhichClauseType.WithClause Then
                            lrFactType = Me.FBMFactType
                        Else
                            lrFactType = CType(Me.BaseNode.FBMModelObject, FBM.FactType)
                        End If

                    Case GetType(FBM.EntityType)
                        lrFactType = Me.FBMFactType
                    Case GetType(FBM.ValueType)
                        If Me.IsPartialFactTypeMatch Then
                            lrFactType = Me.FBMFactType
                        Else
                            Throw New NotImplementedException("Unknown Conditional type in query. Contact support.")
                        End If
                End Select

                Dim lrPredicatePart As FBM.PredicatePart = Nothing

                If Me.FBMPredicatePart IsNot Nothing Then
                    lrPredicatePart = Me.FBMPredicatePart
                Else

                    Dim larPredicatePart As List(Of FBM.PredicatePart)
                    If Me.Predicate = "" Then
                        'Likely a "WITH WHAT Rating" or "WITH (Rating:'8')" as in "WHICH Lecturer likes WHICH Lecturer WITH WHAT RATING"
                        larPredicatePart = (From FactTypeReading In lrFactType.FactTypeReading
                                            Select FactTypeReading.PredicatePart(0)).ToList
                    Else
                        larPredicatePart = (From FactTypeReading In lrFactType.FactTypeReading
                                            From PredicatePart In FactTypeReading.PredicatePart
                                            Where PredicatePart.PredicatePartText = Trim(Me.Predicate)
                                            Select PredicatePart).ToList
                    End If

                            If larPredicatePart.Count = 0 Then

                        larPredicatePart = (From FactType In Me.QueryGraph.Model.FactType
                                            From FactTypeReading In FactType.FactTypeReading
                                            From PredicatePart In FactTypeReading.PredicatePart
                                            Where FactType.RoleGroup.FindAll(Function(x) x.JoinedORMObject.Id = Me.BaseNode.Name _
                                                           Or x.JoinedORMObject.Id = Me.TargetNode.Name).Count = 2
                                            Where PredicatePart.PredicatePartText = Me.Predicate
                                            Where Me.Predicate <> ""
                                            Select PredicatePart).ToList

                        If larPredicatePart.Count > 0 Then
                                    lrPredicatePart = larPredicatePart.First
                                    lrFactType = lrPredicatePart.FactTypeReading.FactType
                                Else
                                    If lrFactType.IsObjectified Then
                                Dim larFactTypeReading = From FactType In lrFactType.getLinkFactTypes
                                                         From FactTypeReading In FactType.FactTypeReading
                                                         Where FactTypeReading.PredicatePart(0).Role.JoinedORMObject.Id = Me.BaseNode.Name
                                                         Where FactTypeReading.PredicatePart(1).Role.JoinedORMObject.Id = Me.TargetNode.Name
                                                         Select FactTypeReading
                                If larFactTypeReading.Count > 0 Then
                                    lrFactType = larFactTypeReading.First.FactType
                                End If
                                    End If
                                End If
                            Else
                        If Me.FBMFactTypeReading IsNot Nothing Then
                            lrPredicatePart = larPredicatePart.Find(Function(x) x.FactTypeReading Is Me.FBMFactTypeReading)
                        Else
                            lrPredicatePart = larPredicatePart.First 'For now...need to consider PreboundReadingText/s
                                End If
                            End If
                        End If
                        Dim lrResponsibleRole As FBM.Role

                If lrPredicatePart.Role.JoinedORMObject Is Me.BaseNode.FBMModelObject Then
                    'Nothing to do here, because is the Predicate joined to the BaseNode that we want the Table for
                ElseIf Not lrPredicatePart.Role.JoinedORMObject Is Me.TargetNode.FBMModelObject Then

                    'lrQueryEdge.Predicate = "is " & lrQueryEdge.Predicate
                    '20200808-VM-Leave this breakpoint here. If hasn't been hit in years, get rid of this ElseIf
                    lrPredicatePart = (From FactTypeReading In lrFactType.FactTypeReading
                                       From PredicatePart In FactTypeReading.PredicatePart
                                       Where PredicatePart.PredicatePartText = Trim(Me.Predicate)
                                       Select PredicatePart).First
                End If

                If lrPredicatePart Is Nothing Then
                    Throw New Exception("There is no Predicate (Part) of Fact Type, '" & Me.FBMFactType.Id & "', that is '" & Me.Predicate & "'.")
                Else
                    If lrFactType.IsLinkFactType Then
                        'Want the Role from the actual FactType
                        lrResponsibleRole = lrFactType.LinkFactTypeRole
                    ElseIf Me.IsPartialFactTypeMatch Or Me.FBMFactType.isRDSTable Then
                        lrResponsibleRole = lrPredicatePart.FactTypeReading.PredicatePart(lrPredicatePart.SequenceNr).Role
                    Else
                        lrResponsibleRole = lrPredicatePart.Role
                    End If
                End If

                Dim lrTable As RDS.Table
                If Me.IsPartialFactTypeMatch Or Me.FBMFactType.isRDSTable Then
                    lrTable = Me.FBMFactType.getCorrespondingRDSTable

                    lrColumn = (From Column In lrTable.Column
                                Where Column.Role Is lrResponsibleRole
                                Where Column.ActiveRole.JoinedORMObject Is Me.TargetNode.FBMModelObject
                                Select Column).First

                Else
                    lrTable = Me.BaseNode.RDSTable

                    Dim larColumn = From Column In lrTable.Column
                                    Where Column.Role Is lrResponsibleRole
                                    Where Column.ActiveRole.JoinedORMObject Is Me.TargetNode.FBMModelObject
                                    Select Column

                    If larColumn.count > 0 Then
                        lrColumn = larColumn.First
                    End If
                End If

                Return lrColumn

            Catch ex As Exception
                Throw New Exception(ex.Message)
            End Try

        End Function

    End Class

End Namespace
