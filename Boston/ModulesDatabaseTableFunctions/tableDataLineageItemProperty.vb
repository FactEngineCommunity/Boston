Imports System.Reflection

Public Module tableDataLineageItemProperty
    Public Sub addDataLineageItemProperty(ByRef arDataLineageItemProperty As DataLineage.DataLineageItemProperty)

        Dim lsSQLQuery As String = ""

        Try
            lsSQLQuery = "INSERT INTO DataLineageItemProperty"
            lsSQLQuery &= " VALUES ("
            lsSQLQuery &= " '" & Trim(Replace(arDataLineageItemProperty.Model.ModelId, "'", "`")) & "'"
            lsSQLQuery &= " ,'" & Trim(Replace(arDataLineageItemProperty.Name, "'", "`")) & "'"
            lsSQLQuery &= " ,'" & Trim(Replace(arDataLineageItemProperty.Category, "'", "`")) & "'"
            lsSQLQuery &= " ,'" & Trim(Replace(arDataLineageItemProperty.PropertyType, "'", "`")) & "'"
            lsSQLQuery &= " ,'" & Trim(Replace(arDataLineageItemProperty.Property, "'", "`")) & "'"
            lsSQLQuery &= " ,'" & Trim(Replace(arDataLineageItemProperty.LineageSetNumber, "'", "`")) & "'"
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

    Public Sub DeleteDataLineageItemProperty(ByVal arDataLineageItemProperty As DataLineage.DataLineageItemProperty)

        Dim lsSQLQuery As String = ""

        Try
            lsSQLQuery = "DELETE FROM DataLineageItemProperty"
            lsSQLQuery &= " WHERE ModelId = '" & arDataLineageItemProperty.Model.ModelId & "'"
            lsSQLQuery &= " AND DataLineageItemName = '" & Trim(arDataLineageItemProperty.Name) & "'"
            lsSQLQuery &= " AND DataLineageCategory = '" & Trim(arDataLineageItemProperty.Category) & "'"
            lsSQLQuery &= " AND DataLineagePropertyType = '" & Trim(arDataLineageItemProperty.PropertyType) & "'"

            pdbConnection.BeginTrans()
            pdbConnection.Execute(lsSQLQuery)
            pdbConnection.CommitTrans()

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    Public Function ExistsDataLineageItemProperty(ByVal arDataLineageItemProperty As DataLineage.DataLineageItemProperty) As Boolean

        Dim lsSQLQuery As String = ""
        Dim lREcordset As New RecordsetProxy

        Try
            '------------------------
            'Initialise return value
            '------------------------
            ExistsDataLineageItemProperty = False

            lREcordset.ActiveConnection = pdbConnection
            lREcordset.CursorType = pcOpenStatic

            lsSQLQuery = "SELECT COUNT(*)"
            lsSQLQuery &= "  FROM DataLineageItemProperty"
            lsSQLQuery &= " WHERE ModelId = '" & arDataLineageItemProperty.Model.ModelId & "'"
            lsSQLQuery &= " AND DataLineageItemName = '" & Trim(arDataLineageItemProperty.Name) & "'"
            lsSQLQuery &= " AND DataLineageCategory = '" & Trim(arDataLineageItemProperty.Category) & "'"
            lsSQLQuery &= " AND DataLineagePropertyType = '" & Trim(arDataLineageItemProperty.PropertyType) & "'"

            lREcordset.Open(lsSQLQuery)

            If lREcordset(0).Value > 0 Then
                ExistsDataLineageItemProperty = True
            Else
                ExistsDataLineageItemProperty = False
            End If

            lREcordset.Close()

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)

            Return False
        End Try

    End Function

    Public Function getDataLineageItemPropertyCount() As Integer

        Try
            Dim lsSQLQuery As String = ""
            Dim lREcordset As New RecordsetProxy

            lREcordset.ActiveConnection = pdbConnection
            lREcordset.CursorType = pcOpenStatic

            lsSQLQuery = " SELECT COUNT(*)"
            lsSQLQuery &= "  FROM DataLineageItemProperty"

            lREcordset.Open(lsSQLQuery)

            getDataLineageItemPropertyCount = lREcordset(0).Value

            lREcordset.Close()

        Catch ex As Exception
            Dim lsMessage1 As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage1 &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Function

    Public Function getHighestLineageSetNrForDataLineageItemCategory(ByRef arModel As FBM.Model,
                                                                     ByVal asDataLineageItemName As String,
                                                                     ByVal asDataLineageItemCategory As String) As Integer

        Dim lsSQLQuery As String = ""
        Dim lRecordset As New RecordsetProxy

        Try
            lRecordset.ActiveConnection = pdbConnection
            lRecordset.CursorType = pcOpenStatic

            lsSQLQuery = "SELECT MAX(LineageSetNumber)"
            lsSQLQuery &= " FROM DataLineageItemProperty"
            lsSQLQuery &= " WHERE ModelId = '" & arModel.ModelId & "'"
            lsSQLQuery &= " AND DataLineageItemName = '" & Trim(asDataLineageItemName) & "'"
            lsSQLQuery &= " AND DataLineageCategory = '" & Trim(asDataLineageItemCategory) & "'"

            lRecordset.Open(lsSQLQuery)

            If Not lRecordset.EOF Then
                getHighestLineageSetNrForDataLineageItemCategory = NullVal(lRecordset(0).Value, 0)
            Else
                Return 0
            End If

            lRecordset.Close()

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)

            Return 0
        End Try

    End Function

    Public Function getDataLineageItemPropertyDetails(ByRef arDataLineageItemProperty As DataLineage.DataLineageItemProperty,
                                                      Optional abIgnoreErrors As Boolean = False) As DataLineage.DataLineageItemProperty

        Dim lsSQLQuery As String = ""
        Dim lRecordset As New RecordsetProxy

        Try
            lRecordset.ActiveConnection = pdbConnection
            lRecordset.CursorType = pcOpenStatic

            lsSQLQuery = "SELECT *"
            lsSQLQuery &= " FROM DataLineageItemProperty"
            lsSQLQuery &= " WHERE ModelId = '" & arDataLineageItemProperty.Model.ModelId & "'"
            lsSQLQuery &= " AND DataLineageItemName = '" & Trim(arDataLineageItemProperty.Name) & "'"
            lsSQLQuery &= " AND DataLineageCategory = '" & Trim(arDataLineageItemProperty.Category) & "'"
            lsSQLQuery &= " AND DataLineagePropertyType = '" & Trim(arDataLineageItemProperty.PropertyType) & "'"
            lsSQLQuery &= " AND LineageSetNumber = " & Trim(arDataLineageItemProperty.LineageSetNumber)

            lRecordset.Open(lsSQLQuery)

            If Not lRecordset.EOF Then
                arDataLineageItemProperty.Property = lRecordset("DataLineageProperty").Value
            Else
                If Not abIgnoreErrors Then
                    Dim lsMessage As String = "No DataLineageItemProperty returned for DataLineageItemName: " & arDataLineageItemProperty.Name & ", Property Type: " & arDataLineageItemProperty.PropertyType
                    Throw New Exception(lsMessage)
                End If
            End If

            lRecordset.Close()

            Return arDataLineageItemProperty

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            lsMessage.AppendDoubleLineBreak(lsSQLQuery)
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)

            Return Nothing
        End Try

    End Function


    Public Function getDataLineageItemPropertyDetailsByNameCategoryPropertyType(ByRef arModel As FBM.Model,
                                                            ByVal asDataLineageItemName As String,
                                                            ByVal asCategory As String,
                                                            ByVal asPropertyType As String,
                                                            Optional abIgnoreErrors As Boolean = False) As DataLineage.DataLineageItemProperty

        Dim lsSQLQuery As String = ""
        Dim lREcordset As New RecordsetProxy

        Dim lrDataLineageItemProperty As DataLineage.DataLineageItemProperty = Nothing

        Try
            lREcordset.ActiveConnection = pdbConnection
            lREcordset.CursorType = pcOpenStatic

            lsSQLQuery = "SELECT *"
            lsSQLQuery &= " FROM DataLineageItemProperty"
            lsSQLQuery &= " WHERE ModelId = '" & arModel.ModelId & "'"
            lsSQLQuery &= " AND DataLineageItemName = '" & Trim(asDataLineageItemName) & "'"
            lsSQLQuery &= " AND DataLineageCategory = '" & Trim(asCategory) & "'"
            lsSQLQuery &= " AND DataLineagePropertyType = '" & Trim(asPropertyType) & "'"

            lREcordset.Open(lsSQLQuery)

            If lREcordset.EOF Then
                lrDataLineageItemProperty = New DataLineage.DataLineageItemProperty
                lrDataLineageItemProperty.Model = arModel
                lrDataLineageItemProperty.Name = asDataLineageItemName
                lrDataLineageItemProperty.Category = asCategory
                lrDataLineageItemProperty.PropertyType = asPropertyType
                lrDataLineageItemProperty.Property = lREcordset("Symbol").Value
            Else
                If Not abIgnoreErrors Then
                    Dim lsMessage As String = "Error: getDataLineageItemPropertyDetailsByNameCategoryPropertyType: No DataLineageItemProperty returned for DataLineageItemName: " & asDataLineageItemName
                    Throw New Exception(lsMessage)
                End If
            End If

            lREcordset.Close()

            Return lrDataLineageItemProperty

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)

            Return Nothing
        End Try

    End Function

    Public Sub updateDataLineageItemProperty(ByRef arDataLineageItemProperty As DataLineage.DataLineageItemProperty)

        Dim lsSQLQuery As String

        Try
            lsSQLQuery = " UPDATE DataLineageItemProperty"
            lsSQLQuery &= "   SET ModelId = '" & Trim(arDataLineageItemProperty.Model.ModelId) & "'"
            lsSQLQuery &= "      ,DataLineageItemName = '" & Trim(Replace(arDataLineageItemProperty.Name, "'", "`")) & "'"
            lsSQLQuery &= "      ,DataLineageCategory = '" & Trim(arDataLineageItemProperty.Category) & "'"
            lsSQLQuery &= "      ,DataLineagePropertyType = '" & Trim(arDataLineageItemProperty.PropertyType) & "'"
            lsSQLQuery &= "      ,DataLineageProperty = '" & Trim(Replace(arDataLineageItemProperty.Property, "'", "`")) & "'"
            lsSQLQuery &= " WHERE ModelId = '" & Trim(arDataLineageItemProperty.Model.ModelId) & "'"
            lsSQLQuery &= "   AND DataLineageItemName = '" & Trim(arDataLineageItemProperty.Name) & "'"
            lsSQLQuery &= "   AND DataLineageCategory = '" & Trim(arDataLineageItemProperty.Category) & "'"
            lsSQLQuery &= "   AND DataLineagePropertyType = '" & Trim(arDataLineageItemProperty.PropertyType) & "'"
            lsSQLQuery &= "   AND LineageSetNumber = " & Trim(arDataLineageItemProperty.LineageSetNumber)

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
