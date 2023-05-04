Imports System.Reflection

Public Module tableDataLineageItemModelElement

    Public Sub addDataLineageItemModelElement(ByRef arDataLineageItemModelElement As DataLineage.DataLineageItemModelElement)

        Dim lsSQLQuery As String = ""

        Try
            lsSQLQuery = "INSERT INTO DataLineageItemModelElement"
            lsSQLQuery &= " VALUES ("
            lsSQLQuery &= " '" & Trim(Replace(arDataLineageItemModelElement.Model.ModelId, "'", "`")) & "'"
            lsSQLQuery &= " ,'" & Trim(Replace(arDataLineageItemModelElement.Name, "'", "`")) & "'"
            lsSQLQuery &= " ,'" & Trim(Replace(arDataLineageItemModelElement.ModelElementType, "'", "`")) & "'"
            lsSQLQuery &= " ,'" & Trim(Replace(arDataLineageItemModelElement.Symbol, "'", "`")) & "'"
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

    Public Function getDataLineageItemModelElementCount() As Integer

        Try
            Dim lsSQLQuery As String = ""
            Dim lREcordset As New ADODB.Recordset

            lREcordset.ActiveConnection = pdbConnection
            lREcordset.CursorType = pcOpenStatic

            lsSQLQuery = " SELECT COUNT(*)"
            lsSQLQuery &= "  FROM DataLineageItemModelElement"

            lREcordset.Open(lsSQLQuery)

            getDataLineageItemModelElementCount = lREcordset(0).Value

            lREcordset.Close()

        Catch ex As Exception
            Dim lsMessage1 As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage1 &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Function

    Public Function getDataLineageItemModelElementDetailsByName(ByRef arModel As FBM.Model,
                                                                ByVal asDataLineageItemName As String,
                                                                Optional abIgnoreErrors As Boolean = False) As List(Of DataLineage.DataLineageItemModelElement)

        Dim lsSQLQuery As String = ""
        Dim lREcordset As New ADODB.Recordset

        Dim lrDataLineageItemModelElement As DataLineage.DataLineageItemModelElement = Nothing
        Dim larDataLineageItemModelElement As New List(Of DataLineage.DataLineageItemModelElement)

        Try
            lREcordset.ActiveConnection = pdbConnection
            lREcordset.CursorType = pcOpenStatic

            lsSQLQuery = "SELECT *"
            lsSQLQuery &= " FROM DataLineageItemModelElement"
            lsSQLQuery &= " WHERE ModelId = '" & arModel.ModelId & "'"
            lsSQLQuery &= " AND DataLineageItemName = '" & Trim(asDataLineageItemName) & "'"

            lREcordset.Open(lsSQLQuery)

            While Not lREcordset.EOF
                lrDataLineageItemModelElement = New DataLineage.DataLineageItemModelElement
                lrDataLineageItemModelElement.Model = arModel
                lrDataLineageItemModelElement.Name = lREcordset("DataLineageItemName").Value
                lrDataLineageItemModelElement.ModelElementType = lREcordset("ModelElementType").Value
                lrDataLineageItemModelElement.Symbol = lREcordset("Symbol").Value

                larDataLineageItemModelElement.Add(lrDataLineageItemModelElement)

                lREcordset.MoveNext()
            End While

            lREcordset.Close()

            Return larDataLineageItemModelElement

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)

            Return Nothing
        End Try

    End Function

    Public Function getDataLineageItemModelElementsForModelElementDataLineageItemName(ByRef arModel As FBM.Model,
                                                                                      ByVal asModelElementType As String,
                                                                                      ByVal asDataLineageItemName As String) As List(Of DataLineage.DataLineageItemModelElement)

        Dim lsSQLQuery As String = ""
        Dim lREcordset As New ADODB.Recordset

        Dim lrDataLineageItemModelElement As DataLineage.DataLineageItemModelElement
        Dim larDataLineageItemModelElement As New List(Of DataLineage.DataLineageItemModelElement)

        Try
            lREcordset.ActiveConnection = pdbConnection
            lREcordset.CursorType = pcOpenStatic

            lsSQLQuery = "SELECT *"
            lsSQLQuery &= " FROM DataLineageItemModelElement"
            lsSQLQuery &= " WHERE ModelId = '" & Trim(arModel.ModelId) & "'"
            lsSQLQuery &= " AND  ModelElementType = '" & Trim(asModelElementType) & "'"
            lsSQLQuery &= " AND DataLineageItemName = '" & Trim(asDataLineageItemName) & "'"

            lREcordset.Open(lsSQLQuery)

            While Not lREcordset.EOF
                lrDataLineageItemModelElement = New DataLineage.DataLineageItemModelElement
                lrDataLineageItemModelElement.Model = arModel
                lrDataLineageItemModelElement.Name = lREcordset("DataLineageItemName").Value
                lrDataLineageItemModelElement.ModelElementType = lREcordset("ModelElementType").Value
                lrDataLineageItemModelElement.Symbol = lREcordset("Symbol").Value

                larDataLineageItemModelElement.Add(lrDataLineageItemModelElement)

                lREcordset.MoveNext()
            End While

            lREcordset.Close()

            Return larDataLineageItemModelElement
        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)

            Return Nothing
        End Try

    End Function

    Public Sub updateDataLineageItemModelElement(ByRef arModel As FBM.Model, ByRef arDataLineageItemModelElement As DataLineage.DataLineageItemModelElement)

        Dim lsSQLQuery As String

        Try
            lsSQLQuery = " UPDATE DataLineageItemModelElement"
            lsSQLQuery &= "   SET ModelId = '" & Trim(arDataLineageItemModelElement.Model.ModelId) & "'"
            lsSQLQuery &= "      ,DataLineageItemName = '" & Trim(Replace(arDataLineageItemModelElement.Name, "'", "`")) & "'"
            lsSQLQuery &= "      ,ModelElmentType = '" & Trim(Replace(arDataLineageItemModelElement.ModelElementType, "'", "`")) & "'"
            lsSQLQuery &= "      ,Symbol = '" & Trim(Replace(arDataLineageItemModelElement.Symbol, "'", "`")) & "'"
            lsSQLQuery &= " WHERE DataLineageItemName = '" & Trim(Replace(arDataLineageItemModelElement.Name, "'", "`")) & "'"

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
