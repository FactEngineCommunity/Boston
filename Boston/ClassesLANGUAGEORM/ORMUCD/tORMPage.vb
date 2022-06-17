Imports Boston.FBM.STM
Imports System.Reflection
Imports Newtonsoft.Json

Namespace FBM

    Partial Public Class Page
        <JsonIgnore()>
        <NonSerialized>
        Public WithEvents CMMLModel As CMML.Model

        Public Sub DropUMLActorAtPoint(ByRef arActor As UML.Actor, ByVal aoPointF As PointF)

            Dim lsSQLQuery As String
            Dim lrRecordset As ORMQL.Recordset
            Dim lsMessage As String

            Try

                arActor.X = aoPointF.X
                arActor.Y = aoPointF.Y

                lsSQLQuery = "SELECT *"
                lsSQLQuery &= " FROM " & pcenumCMMLRelations.CoreElementHasElementType.ToString
                lsSQLQuery &= " WHERE Element = '" & arActor.Name & "'"
                lsSQLQuery &= " AND ElementType = 'Actor'"

                lrRecordset = Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                If lrRecordset.EOF Then
                    lsMessage = "The Actor, '" & arActor.Name & "', does not seem to exist at the Model level."

                    Call prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Information, Nothing, False, False, True)
                    Exit Sub
                Else
                    lsSQLQuery = "ADD FACT '" & lrRecordset.CurrentFact.Id & "'"
                    lsSQLQuery &= " TO " & pcenumCMMLRelations.CoreElementHasElementType.ToString
                    lsSQLQuery &= " ON PAGE '" & Me.Name & "'"

                    Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                    Me.UMLDiagram.Actor.AddUnique(arActor)

                    Call arActor.DisplayAndAssociate()
                End If

            Catch ex As Exception
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Public Function DropCMMLActorAtPoint(ByRef arCMMLActor As CMML.Actor,
                                               ByVal aoPointF As PointF,
                                               Optional ByVal abBroadcastInterfaceEvent As Boolean = True) As UML.Actor

            Dim lsSQLQuery As String = ""
            Dim lrFactInstance As FBM.FactInstance
            Dim lrRecordset As ORMQL.Recordset
            Dim lsMessage As String = ""

            Try
                'Set the Actor' Page to Me                
                lsSQLQuery = "SELECT *"
                lsSQLQuery &= " FROM " & pcenumCMMLRelations.CoreElementHasElementType.ToString
                lsSQLQuery &= " WHERE Element = '" & arCMMLActor.Name & "'"
                lsSQLQuery &= " AND ElementType = 'Actor'"

                lrRecordset = Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                If lrRecordset.EOF Then
                    lsMessage = "The Actor, '" & arCMMLActor.Name & "', does not seem to exist at the Model level."

                    Call prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Information, Nothing, False, False, True)
                    Return Nothing
                Else
                    lsSQLQuery = "ADD FACT '" & lrRecordset.CurrentFact.Id & "'"
                    lsSQLQuery &= " TO " & pcenumCMMLRelations.CoreElementHasElementType.ToString
                    lsSQLQuery &= " ON PAGE '" & Me.Name & "'"

                    lrFactInstance = Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                    Dim lrUMLActor As UML.Actor = lrFactInstance.GetFactDataInstanceByRoleName(pcenumCMML.Element.ToString).CloneActor(Me)
                    lrUMLActor.Name = arCMMLActor.Name

                    Me.UMLDiagram.Actor.AddUnique(lrUMLActor)
                    '===================================================================================================================

                    Call lrUMLActor.Move(aoPointF.X, aoPointF.Y, abBroadcastInterfaceEvent)

                    Call Me.Save(False, False)

                    Call lrUMLActor.DisplayAndAssociate()

                    Return lrUMLActor
                End If

            Catch ex As Exception
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)

                Return Nothing
            End Try

        End Function


        Public Function DropCMMLProcessAtPoint(ByRef arCMMLProcess As CMML.Process,
                                               ByVal aoPointF As PointF,
                                               ByRef aoContainerNode As MindFusion.Diagramming.ContainerNode,
                                               Optional ByVal abBroadcastInterfaceEvent As Boolean = True,
                                               Optional ByVal aiLanguage As pcenumLanguage = pcenumLanguage.CMML) As Object

            Dim lsSQLQuery As String = ""
            Dim lrFactInstance As FBM.FactInstance
            Dim lrRecordset As ORMQL.Recordset
            Dim lsMessage As String = ""

            Try
                'Set the Process' Page to Me                
                lsSQLQuery = "SELECT *"
                lsSQLQuery &= " FROM " & pcenumCMMLRelations.CoreElementHasElementType.ToString
                lsSQLQuery &= " WHERE Element = '" & arCMMLProcess.Id & "'"

                lrRecordset = Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                If lrRecordset.EOF Then
                    lsMessage = "The Process, '" & arCMMLProcess.Text & "', does not seem to exist at the Model level."

                    Call prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Information, Nothing, False, False, True)
                    Return Nothing
                Else
                    lsSQLQuery = "ADD FACT '" & lrRecordset.CurrentFact.Id & "'"
                    lsSQLQuery &= " TO " & pcenumCMMLRelations.CoreElementHasElementType.ToString
                    lsSQLQuery &= " ON PAGE '" & Me.Name & "'"

                    lrFactInstance = Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                    Dim lrUMLProcess As UML.Process = lrFactInstance.GetFactDataInstanceByRoleName(pcenumCMML.Element.ToString).CloneProcess(Me)
                    lrUMLProcess.Id = arCMMLProcess.Id
                    lrUMLProcess.Text = arCMMLProcess.Text
                    lrUMLProcess.CMMLProcess = arCMMLProcess

                    Dim lrProcess As UML.Process = lrUMLProcess
                    Select Case aiLanguage
                        Case Is = pcenumLanguage.UMLUseCaseDiagram
                            lrProcess = lrUMLProcess.CloneUCDProcess(Me)
                    End Select

                    Me.UMLDiagram.Process.AddUnique(lrProcess)
                    '===================================================================================================================

                    Call lrProcess.Move(aoPointF.X, aoPointF.Y, abBroadcastInterfaceEvent)

                    Call Me.Save(False, False)

                    Call lrProcess.DisplayAndAssociate(aoContainerNode)

                    Return lrProcess
                End If

            Catch ex As Exception
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)

                Return Nothing
            End Try

        End Function


        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="arProcess">The Model Level Process</param>
        ''' <param name="aoPointF"></param>
        ''' <param name="aoContainerNode"></param>
        ''' <returns></returns>
        Public Function DropProcessAtPoint(ByRef arProcess As UML.Process,
                                           ByVal aoPointF As PointF,
                                           ByRef aoContainerNode As MindFusion.Diagramming.ContainerNode,
                                           Optional ByVal abBroadcastInterfaceEvent As Boolean = True) As UML.Process

            Dim lsSQLQuery As String = ""
            Dim lrFactInstance As FBM.FactInstance
            Dim lrRecordset As ORMQL.Recordset
            Dim lsMessage As String = ""

            Try
                'Set the Process' Page to Me
                arProcess.Page = Me

                lsSQLQuery = "SELECT *"
                lsSQLQuery &= " FROM " & pcenumCMMLRelations.CoreElementHasElementType.ToString
                lsSQLQuery &= " WHERE Element = '" & arProcess.Id & "'"

                lrRecordset = Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                If lrRecordset.EOF Then
                    lsMessage = "The Process, '" & arProcess.Text & "', does not seem to exist at the Model level."

                    Call prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Information, Nothing, False, False, True)
                    Return Nothing
                Else

                    lsSQLQuery = "ADD FACT '" & lrRecordset.CurrentFact.Id & "'"
                    lsSQLQuery &= " TO " & pcenumCMMLRelations.CoreElementHasElementType.ToString
                    lsSQLQuery &= " ON PAGE '" & Me.Name & "'"

                    lrFactInstance = Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                    Dim lrProcess As UML.Process = lrFactInstance.GetFactDataInstanceByRoleName(pcenumCMML.Element.ToString).CloneProcess(Me)
                    lrProcess.Text = arProcess.Text
                    Me.UMLDiagram.Process.AddUnique(lrProcess)
                    '===================================================================================================================

                    Call lrProcess.Move(aoPointF.X, aoPointF.Y, abBroadcastInterfaceEvent)

                    Call Me.Save(False, False)

                    Call lrProcess.DisplayAndAssociate(aoContainerNode)

                    Return lrProcess
                End If

            Catch ex As Exception
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)

                Return Nothing
            End Try

        End Function

    End Class

End Namespace
