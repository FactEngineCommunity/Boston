Imports System.Reflection

Namespace FactEngine
    Public Class QueryEdge

        Public QueryGraph As FactEngine.QueryGraph = Nothing

        Public BaseNode As FactEngine.QueryNode = Nothing
        Public TargetNode As FactEngine.QueryNode = Nothing
        Public Predicate As String = ""

        Public IdentifierList As New List(Of String)

        Public FBMFactType As FBM.FactType = Nothing

        ''' <summary>
        ''' TRUE if is the project of a select/project
        ''' </summary>
        Public IsProjectColumn As Boolean = False

        Public WhichClauseType As pcenumWhichClauseType = pcenumWhichClauseType.None

        ''' <summary>
        ''' Paremeterless New
        ''' </summary>
        Public Sub New()
        End Sub

        Public Sub New(ByRef arQueryGraph As FactEngine.QueryGraph)
            Me.QueryGraph = arQueryGraph
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
                If Me.WhichClauseType = pcenumWhichClauseType.IsPredicateNodePropertyIdentification Then

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
                                Me.FBMFactType = Me.BaseNode.FBMModelObject
                                'Else
                                '    Throw New Exception("There is not Fact Type, '" & arBaseNode.FBMModelObject.Id & " " & asPredicate & " " & arTargetNode.FBMModelObject.Id & "', in the Model.")
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
                                   lrFactTypeReading.PredicatePart(0).PredicatePartText = asPredicate Then
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
