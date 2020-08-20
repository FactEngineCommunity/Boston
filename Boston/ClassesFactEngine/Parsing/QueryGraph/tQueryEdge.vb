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

        Public FBMFactType As FBM.FactType = Nothing

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
                                        ByVal asPredicate As String) As Exception

            Dim lsMessage As String = ""
            Try
                If Me.WhichClauseSubType = pcenumWhichClauseType.IsPredicateNodePropertyIdentification Then

                    Select Case Me.BaseNode.FBMModelObject.GetType
                        Case Is = GetType(FBM.FactType)

                            '========================================================
                            '20200802-VM-NB Can probably do the following regardless of whether is a FactType
                            Dim larModelObject As New List(Of FBM.ModelObject)
                            larModelObject.Add(arBaseNode.FBMModelObject)
                            larModelObject.Add(arTargetNode.FBMModelObject)
                            Dim lasPredicatePart As New List(Of String)
                            lasPredicatePart.Add(asPredicate)
                            lasPredicatePart.Add("")
                            Dim larRole As New List(Of FBM.Role)
                            Dim lrDummyFactType As New FBM.FactType
                            larRole.Add(New FBM.Role(lrDummyFactType, larModelObject(0)))
                            larRole.Add(New FBM.Role(lrDummyFactType, larModelObject(1)))
                            Dim lrFactTypeReading As New FBM.FactTypeReading(lrDummyFactType, larRole, lasPredicatePart)
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
                                        Throw New Exception("There is not Fact Type, '" & arBaseNode.FBMModelObject.Id & " " & asPredicate & " " & arTargetNode.FBMModelObject.Id & "', in the Model.")
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
                            Dim larModelObject As New List(Of FBM.ModelObject)
                            larModelObject.Add(arBaseNode.FBMModelObject)
                            larModelObject.Add(arTargetNode.FBMModelObject)
                            Dim lasPredicatePart As New List(Of String)
                            lasPredicatePart.Add(asPredicate)
                            Dim larRole As New List(Of FBM.Role)
                            Dim lrDummyFactType As New FBM.FactType
                            larRole.Add(New FBM.Role(lrDummyFactType, larModelObject(0)))
                            larRole.Add(New FBM.Role(lrDummyFactType, larModelObject(1)))

                            Dim larFactTypeReading = From FactType In Me.QueryGraph.Model.FactType
                                                     From FactTypeReading In FactType.FactTypeReading
                                                     Where FactTypeReading.PredicatePart.Count > 1
                                                     Select FactTypeReading

                            Dim larFinalFactTypeReading As New List(Of FBM.FactTypeReading)
                            For Each lrFactTypeReading In larFactTypeReading
                                If lrFactTypeReading.PredicatePart(0).Role.JoinedORMObject.Id = larRole(0).JoinedORMObject.Id And
                                   lrFactTypeReading.PredicatePart(1).Role.JoinedORMObject.Id = larRole(1).JoinedORMObject.Id And
                                   lrFactTypeReading.PredicatePart(1).PreBoundText = Viev.NullVal(arTargetNode.PreboundText, "") Then
                                    larFinalFactTypeReading.Add(lrFactTypeReading)
                                End If
                            Next

                            If larFinalFactTypeReading.Count > 1 Then
                                lsMessage = "More than one Fact Type has a Predicate Part, '" & asPredicate & "', for Model Elements, '" & arBaseNode.FBMModelObject.Id & " and " & arTargetNode.FBMModelObject.Id & "."
                                lsMessage &= vbCrLf & vbCrLf & "Try referencing the following Fact Types directly in your query:"
                                lsMessage &= vbCrLf & vbCrLf
                                For Each lrFactTypeReading In larFinalFactTypeReading
                                    lsMessage &= "- " & lrFactTypeReading.FactType.Id & vbCrLf
                                Next
                                Throw New Exception(lsMessage)
                            Else
                                If larFinalFactTypeReading.Count = 1 Then
                                    Me.FBMFactType = larFinalFactTypeReading(0).FactType
                                Else
                                    lsMessage = "There is no Predicate Part for any Fact Type that has a Predicate Part, '" & asPredicate & "', for Model Elements, '" & arBaseNode.FBMModelObject.Id & " and " & arTargetNode.FBMModelObject.Id & ". Try revising your query."
                                    Throw New Exception(lsMessage)
                                End If
                            End If

                    End Select

                Else

                    Dim larModelObject As New List(Of FBM.ModelObject)
                    larModelObject.Add(arBaseNode.FBMModelObject)
                    larModelObject.Add(arTargetNode.FBMModelObject)
                    Dim lasPredicatePart As New List(Of String)
                    lasPredicatePart.Add(asPredicate)
                    lasPredicatePart.Add("")
                    Dim larRole As New List(Of FBM.Role)
                    Dim lrDummyFactType As New FBM.FactType
                    larRole.Add(New FBM.Role(lrDummyFactType, larModelObject(0)))
                    larRole.Add(New FBM.Role(lrDummyFactType, larModelObject(1)))
                    Dim lrFactTypeReading As New FBM.FactTypeReading(lrDummyFactType, larRole, lasPredicatePart)
                    Me.FBMFactType = Me.QueryGraph.Model.getFactTypeByModelObjectsFactTypeReading(larModelObject,
                                                                                                lrFactTypeReading)

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
                            lsMessage = "There is no Fact Type Reading in the Model, '" & arBaseNode.FBMModelObject.Id & " " & asPredicate & " " & arTargetNode.Name & "'"
                            Throw New Exception(lsMessage)
                        Else
                            Me.FBMFactType = larFactType.First
                        End If
                    End If

                    If Me.FBMFactType Is Nothing Then
                        Throw New Exception("There is not Fact Type, '" & arBaseNode.FBMModelObject.Id & " " & asPredicate & " " & arTargetNode.FBMModelObject.Id & "', in the Model.")
                    End If

                End If

            Catch ex As Exception
                Throw New Exception(ex.Message)
                'prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

            Return Nothing
        End Function

    End Class

End Namespace
