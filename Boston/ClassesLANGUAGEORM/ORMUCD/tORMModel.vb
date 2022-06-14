Imports System.Reflection

Namespace FBM

    Partial Public Class Model

        ''' <summary>
        ''' Creates a Process within the CMML layer of the 4-Layer Architecture. Also creates a UML.Process
        ''' </summary>
        ''' <param name="arCMMLProcess"></param>
        ''' <returns></returns>
        Public Function createCMMLProcess(ByRef arCMMLProcess As CMML.Process) As UML.Process

            Dim lrProcess = New UML.Process

            lrProcess.Id = arCMMLProcess.Id 'Ids are used for Processes
            lrProcess.CMMLProcess = arCMMLProcess
            lrProcess.Text = arCMMLProcess.Text
            lrProcess.Model = Me

            Dim lsSQLQuery As String

            lsSQLQuery = "INSERT INTO " & pcenumCMMLRelations.CoreElementHasElementType.ToString & " (Element, ElementType)"
            lsSQLQuery &= " VALUES ('" & lrProcess.Id & "','Process'"
            lsSQLQuery &= ")"

            Dim lrFact As FBM.Fact = Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)

            lsSQLQuery = "INSERT INTO " & pcenumCMMLRelations.CoreProcessHasProcessText.ToString & " (Process, ProcessText)"
            lsSQLQuery &= " VALUES ('" & lrProcess.Id & "','" & lrProcess.Text & "'"
            lsSQLQuery &= ")"

            Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)

            Return lrProcess

        End Function

        Public Sub updateCMMLProcessText(ByRef arProcess As CMML.Process)

            Try
                Dim lsSQLQuery As String

                lsSQLQuery = "UPDATE " & pcenumCMMLRelations.CoreProcessHasProcessText.ToString
                lsSQLQuery &= " SET ProcessText = '" & arProcess.Text & "'"
                lsSQLQuery &= " WHERE Process = '" & arProcess.Id & "'"

                Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub


        Public Sub updateCMMLProcessText(ByRef arProcess As UML.Process)

            Try
                Dim lsSQLQuery As String

                lsSQLQuery = "UPDATE " & pcenumCMMLRelations.CoreProcessHasProcessText.ToString
                lsSQLQuery &= " SET ProcessText = '" & arProcess.Text & "'"
                lsSQLQuery &= " WHERE Process = '" & arProcess.Id & "'"

                Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)

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
