Imports System.Reflection

Public Module tableDataLineageItem
    Public Sub addDataLineageItem(ByRef arDataLineageItem As DataLineage.DataLineageItem)

        Dim lsSQLQuery As String = ""

        Try
            lsSQLQuery = "INSERT INTO DataLineageItem"
            lsSQLQuery &= " VALUES ("
            lsSQLQuery &= " '" & Trim(Replace(arDataLineageItem.Model.ModelId, "'", "`")) & "'"
            lsSQLQuery &= " ,'" & Trim(Replace(arDataLineageItem.Name, "'", "`")) & "'"
            lsSQLQuery &= ")"

            pdbConnection.BeginTrans()
            Call pdbConnection.Execute(lsSQLQuery)
            pdbConnection.CommitTrans()

        Catch ex As Exception
            Dim lsMessage1 As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage1 &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)

            pdbConnection.RollbackTrans()
        End Try

    End Sub

    Public Function getDataLineageItemCount() As Integer

        Try
            Dim lsSQLQuery As String = ""
            Dim lREcordset As New ADODB.Recordset

            lREcordset.ActiveConnection = pdbConnection
            lREcordset.CursorType = pcOpenStatic

            lsSQLQuery = " SELECT COUNT(*)"
            lsSQLQuery &= "  FROM DataLineageItem"

            lREcordset.Open(lsSQLQuery)

            getDataLineageItemCount = lREcordset(0).Value

            lREcordset.Close()

        Catch ex As Exception
            Dim lsMessage1 As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage1 &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Function

    Public Function getDataLineageItemDetailsByName(ByVal asDataLineageItemName As String,
                                                    ByRef arDataLineageItem As DataLineage.DataLineageItem,
                                                    Optional abIgnoreErrors As Boolean = False) As DataLineage.DataLineageItem

        Dim lsSQLQuery As String = ""
        Dim lREcordset As New ADODB.Recordset

        Try
            lREcordset.ActiveConnection = pdbConnection
            lREcordset.CursorType = pcOpenStatic

            lsSQLQuery = "SELECT *"
            lsSQLQuery &= " FROM DataLineageItem"
            lsSQLQuery &= " WHERE Id = '" & Trim(asDataLineageItemName) & "'"

            lREcordset.Open(lsSQLQuery)

            If Not lREcordset.EOF Then
                If arDataLineageItem Is Nothing Then
                    arDataLineageItem = New DataLineage.DataLineageItem
                End If
                arDataLineageItem.Name = lREcordset("DataLineageItemName").Value
            Else
                If Not abIgnoreErrors Then
                    Dim lsMessage As String = "Error: getDataLineageItemDetailsByName: No DataLineageItem returned for DataLineageItemName: " & asDataLineageItemName
                    Throw New Exception(lsMessage)
                End If
            End If

            lREcordset.Close()

            Return arDataLineageItem

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)

            Return Nothing
        End Try

    End Function

    Public Sub updateDataLineageItem(ByRef arDataLineageItem As DataLineage.DataLineageItem, ByVal asNewName As String)

        Dim lsSQLQuery As String

        Try
            lsSQLQuery = " UPDATE DataLineageItem"
            lsSQLQuery &= "   SET DataLineageItemName = '" & Trim(Replace(asNewName, "'", "`")) & "'"
            lsSQLQuery &= " WHERE DataLineageItemName = '" & Trim(Replace(arDataLineageItem.Name, "'", "`")) & "'"

            pdbConnection.BeginTrans()
            pdbConnection.Execute(lsSQLQuery)
            pdbConnection.CommitTrans()

        Catch ex As Exception

            Dim lsMessage1 As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage1 &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)

            pdbConnection.RollbackTrans()
        End Try

    End Sub


End Module
