Imports System.Reflection

Namespace UML
    Public Class ProcessProcessRelation
        Inherits FBM.FactInstance
        Implements FBM.iPageObject

        Public UMLModel As UML.Model

        Public Process1 As UML.Process
        Public Process2 As UML.Process

        Public WithEvents CMMLProcessProcessRelation As CMML.ProcessProcessRelation

        Public Link As MindFusion.Diagramming.DiagramLink

        Public Event RemovedFromModel()

        ''' <summary>
        ''' Parameterless Constructor
        ''' </summary>
        Public Sub New()
        End Sub

        Public Sub New(ByRef arUMLModel As UML.Model, ByRef arProcess1 As UML.Process, ByRef arProcess2 As UML.Process)

            Me.UMLModel = arUMLModel
            Me.Process1 = arProcess1
            Me.Process2 = Process2

        End Sub

        Public Shadows Function RemoveFromModel(Optional ByVal abForceRemoval As Boolean = False,
                                                  Optional ByVal abCheckForErrors As Boolean = True,
                                                  Optional ByVal abDoDatabaseProcessing As Boolean = True,
                                                  Optional ByVal abDeleteAll As Boolean = False) As Boolean

            Try
                Call Me.Model.UML.RemoveProcessProcessRelation(Me.CMMLProcessProcessRelation)

                RaiseEvent RemovedFromModel()

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Function

        Public Sub RemoveFromPage()

            Try
                Call Me.UMLModel.PocessToProcessRelationFTI.RemoveFact(Me.FactInstance)

                If Me.Page.Diagram IsNot Nothing And Me.Link IsNot Nothing Then
                    Call Me.Page.Diagram.Links.Remove(Me.Link)
                End If


            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Private Sub CMMLProcessProcessRelation_RemovedFromModel() Handles CMMLProcessProcessRelation.RemovedFromModel

            Try
                Call Me.RemoveFromPage()

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

    End Class

End Namespace
