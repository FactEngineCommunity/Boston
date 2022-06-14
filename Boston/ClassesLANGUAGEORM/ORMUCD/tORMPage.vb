Imports Boston.FBM.STM
Imports System.Reflection
Imports Newtonsoft.Json

Namespace FBM

    Partial Public Class Page
        <JsonIgnore()>
        <NonSerialized>
        Public WithEvents CMMLModel As CMML.Model

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
