Imports System.Reflection

Public Module tableDataLineageItemProperty
    Public Sub addDataLineageItemProperty(ByRef arDataLineageItemProperty As DataLineage.DataLineageItemProperty,
                                          Optional ByVal abIgnoreErrors As Boolean = False)

        Dim lsSQLQuery As String = ""

        Try
            lsSQLQuery = "INSERT INTO DataLineageItemProperty"
            lsSQLQuery &= " VALUES ("
            lsSQLQuery &= " '" & Trim(Replace(arDataLineageItemProperty.Model.ModelId, "'", "`")) & "'"
            lsSQLQuery &= " ,'" & Trim(Replace(arDataLineageItemProperty.Name, "'", "`")) & "'"
            lsSQLQuery &= " ,'" & Trim(Replace(arDataLineageItemProperty.Category, "'", "`")) & "'"
            lsSQLQuery &= " ,'" & Trim(Replace(arDataLineageItemProperty.PropertyType, "'", "`")) & "'"
            lsSQLQuery &= " ,'" & Trim(Replace(arDataLineageItemProperty.Property, "'", "`")) & "'"
            lsSQLQuery &= " ," & Trim(Replace(arDataLineageItemProperty.LineageSetNumber, "'", "`"))
            lsSQLQuery &= ")"

            pdbConnection.BeginTrans()
            Dim lrRecordset As ORMQL.Recordset = pdbConnection.Execute(lsSQLQuery, abIgnoreErrors)
            If lrRecordset.ErrorReturned Then
                pdbConnection.RollbackTrans
                Throw New Exception(lrRecordset.ErrorString)
            End If
            pdbConnection.CommitTrans()

        Catch ex As Exception

            If abIgnoreErrors Then Exit Sub

            Dim lsMessage1 As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage1 &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)

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
            lsSQLQuery &= " AND LineageSetNumber = " & Trim(arDataLineageItemProperty.LineageSetNumber)

            pdbConnection.BeginTrans()
            Dim lrRecordset As ORMQL.Recordset = pdbConnection.Execute(lsSQLQuery)
            If lrRecordset.ErrorReturned Then
                pdbConnection.RollbackTrans
                Throw New Exception(lrRecordset.ErrorString)
            End If
            pdbConnection.CommitTrans()

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    Public Sub DeleteDataLineageItemPropertisByCategorySetNumber(ByRef arModel As FBM.Model,
                                                                 ByVal asDataLineageItemName As String,
                                                                 ByVal arDataLineageCategory As DataLineage.DataLineageCategory,
                                                                 ByVal aiSetNumber As Integer)

        Dim lsSQLQuery As String = ""

        Try
            lsSQLQuery = "DELETE FROM DataLineageItemProperty"
            lsSQLQuery &= " WHERE ModelId = '" & arModel.ModelId & "'"
            lsSQLQuery &= " AND DataLineageItemName = '" & asDataLineageItemName.Trim & "'"
            lsSQLQuery &= " AND DataLineageCategory = '" & Trim(arDataLineageCategory.Name) & "'"
            lsSQLQuery &= " AND LineageSetNumber = " & aiSetNumber.ToString

            pdbConnection.BeginTrans()
            Dim lrRecordset As ORMQL.Recordset = pdbConnection.Execute(lsSQLQuery)
            If lrRecordset.ErrorReturned Then
                pdbConnection.RollbackTrans
                Throw New Exception(lrRecordset.ErrorString)
            End If
            pdbConnection.CommitTrans()

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    Public Function ExistsDataLineageItemPropertySet(ByVal arDataLineageItem As DataLineage.DataLineageItem,
                                                     ByRef arDataLineageItemProperty As DataLineage.DataLineageItemProperty) As Boolean

        Dim lsSQLQuery As String = ""
        Dim lREcordset As New RecordsetProxy

        Try
            '------------------------
            'Initialise return value
            '------------------------
            ExistsDataLineageItemPropertySet = False

            lREcordset.ActiveConnection = pdbConnection
            lREcordset.CursorType = pcOpenStatic

            lsSQLQuery = "SELECT *"
            lsSQLQuery &= "  FROM DataLineageItemProperty"
            lsSQLQuery &= " WHERE ModelId = '" & arDataLineageItem.Model.ModelId & "'"
            lsSQLQuery &= " AND DataLineageItemName = '" & Trim(arDataLineageItem.Name) & "'"
            lsSQLQuery &= " AND DataLineageCategory = '" & Trim(arDataLineageItem.DataLineageCategory.Name) & "'"
            lsSQLQuery &= " AND DataLineageProperty = '" & Trim(arDataLineageItem.SetId) & "'"

            lREcordset.Open(lsSQLQuery)

            If Not lREcordset.EOF Then
                ExistsDataLineageItemPropertySet = True
                arDataLineageItemProperty.LineageSetNumber = lREcordset("LineageSetNumber").Value
            Else
                ExistsDataLineageItemPropertySet = False
            End If

            lREcordset.Close()

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)

            Return False
        End Try

    End Function


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
            lsSQLQuery &= " AND LineageSetNumber = " & arDataLineageItemProperty.LineageSetNumber.ToString

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
            prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)

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
            prApplication.ThrowMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Function

    Public Function getHighestLineageSetNrForDataLineageItemCategory(ByRef arModel As FBM.Model,
                                                                     ByVal asDataLineageItemName As String,
                                                                     ByVal asDataLineageItemCategory As String,
                                                                     Optional ByVal asFilterDocumentName As String = Nothing) As Integer

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

            If asFilterDocumentName IsNot Nothing Then
                lsSQLQuery &= " AND DataLineagePropertyType = 'Specification Document Name'"
                lsSQLQuery &= " AND DataLineageProperty = '" & asFilterDocumentName.Trim & "'"
            End If

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
            prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)

            Return 0
        End Try

    End Function

    Public Function getDataLineageDocumentsByModel(ByRef arModel As FBM.Model,
                                                   Optional abIgnoreErrors As Boolean = False) As List(Of DataLineage.Document)

        Dim lsSQLQuery As String = ""
        Dim lRecordset As New RecordsetProxy

        Dim larDataLineageDocument As New List(Of DataLineage.Document)
        Try
            lRecordset.ActiveConnection = pdbConnection
            lRecordset.CursorType = pcOpenStatic

            lsSQLQuery = "SELECT DISTINCT DataLineageProperty"
            lsSQLQuery &= " FROM DataLineageItemProperty"
            lsSQLQuery &= " WHERE ModelId = '" & arModel.ModelId & "'"
            lsSQLQuery &= " AND DataLineageCategory = 'Metadata Lineage'"
            lsSQLQuery &= " AND DataLineagePropertyType = 'Specification Document Name'"
            lsSQLQuery &= " AND DataLineageProperty <> ''"

            lRecordset.Open(lsSQLQuery)

            While Not lRecordset.EOF

                Dim lrDataLineageDocument As New DataLineage.Document

                lrDataLineageDocument.Name = lRecordset("DataLineageProperty").Value

                larDataLineageDocument.Add(lrDataLineageDocument)
                lRecordset.MoveNext()
            End While

            lRecordset.Close()

            Return larDataLineageDocument

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            lsMessage.AppendDoubleLineBreak(lsSQLQuery)
            prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)

            Return Nothing
        End Try

    End Function

    Public Function getDataLineageDocumentsByDataLineageItemName(ByRef arModel As FBM.Model,
                                                                 ByVal asDataLineageItemName As String,
                                                                 Optional abIgnoreErrors As Boolean = False) As List(Of DataLineage.Document)

        Dim lsSQLQuery As String = ""
        Dim lRecordset As New RecordsetProxy

        Dim larDataLineageDocument As New List(Of DataLineage.Document)
        Try
            lRecordset.ActiveConnection = pdbConnection
            lRecordset.CursorType = pcOpenStatic

            lsSQLQuery = "SELECT DISTINCT DataLineageProperty"
            lsSQLQuery &= " FROM DataLineageItemProperty"
            lsSQLQuery &= " WHERE ModelId = '" & arModel.ModelId & "'"
            lsSQLQuery &= " AND DataLineageCategory = 'Metadata Lineage'"
            lsSQLQuery &= " AND DataLineageItemName = '" & Trim(asDataLineageItemName) & "'"
            lsSQLQuery &= " AND DataLineagePropertyType = 'Specification Document Name'"
            lsSQLQuery &= " AND DataLineageProperty <> ''"

            lRecordset.Open(lsSQLQuery)

            While Not lRecordset.EOF

                Dim lrDataLineageDocument As New DataLineage.Document

                lrDataLineageDocument.Name = lRecordset("DataLineageProperty").Value
                lrDataLineageDocument.Location = tableDataLineageItemProperty.getDataLineageDocumentLocationByDataLineageItemName(arModel, asDataLineageItemName)

                larDataLineageDocument.Add(lrDataLineageDocument)
                lRecordset.MoveNext()
            End While

            lRecordset.Close()

            Return larDataLineageDocument

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            lsMessage.AppendDoubleLineBreak(lsSQLQuery)
            prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)

            Return Nothing
        End Try

    End Function

    Public Function getDataLineageDocumentLocationByDataLineageItemName(ByRef arModel As FBM.Model,
                                                                        ByVal asDataLineageItemName As String,
                                                                        Optional abIgnoreErrors As Boolean = False) As String

        Dim lsSQLQuery As String = ""
        Dim lRecordset As New RecordsetProxy

        Try
            lRecordset.ActiveConnection = pdbConnection
            lRecordset.CursorType = pcOpenStatic

            lsSQLQuery = "SELECT DISTINCT DataLineageProperty"
            lsSQLQuery &= " FROM DataLineageItemProperty"
            lsSQLQuery &= " WHERE ModelId = '" & arModel.ModelId & "'"
            lsSQLQuery &= " AND DataLineageCategory = 'Metadata Lineage'"
            lsSQLQuery &= " AND DataLineageItemName = '" & Trim(asDataLineageItemName) & "'"
            lsSQLQuery &= " AND DataLineagePropertyType = 'Document Location'"
            lsSQLQuery &= " AND DataLineageProperty <> ''"

            lRecordset.Open(lsSQLQuery)

            If lRecordset.EOF Then
                lRecordset.Close()
                Return ""
            Else
                Dim lsDocumentLocation = lRecordset("DataLineageProperty").Value
                lRecordset.Close()
                Return lsDocumentLocation
            End If

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            lsMessage.AppendDoubleLineBreak(lsSQLQuery)
            prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)

            Return Nothing
        End Try

    End Function


    Public Function getMetaDataLineageSetNumbersByDocumentFilter(ByRef arModel As FBM.Model,
                                                                 ByVal asDataLineageItemName As String,
                                                                 ByVal asDataLineageItemCategory As String,
                                                                 ByVal asFilterDocumentName As String) As List(Of Integer)

        Dim laiMetaDataLineageSetNumber As New List(Of Integer)
        Dim lsSQLQuery As String = ""
        Dim lRecordset As New RecordsetProxy

        Try
            lRecordset.ActiveConnection = pdbConnection
            lRecordset.CursorType = pcOpenStatic

            lsSQLQuery = "SELECT LineageSetNumber"
            lsSQLQuery &= " FROM DataLineageItemProperty"
            lsSQLQuery &= " WHERE ModelId = '" & arModel.ModelId & "'"
            lsSQLQuery &= " AND DataLineageItemName = '" & Trim(asDataLineageItemName) & "'"
            lsSQLQuery &= " AND DataLineageCategory = '" & Trim(asDataLineageItemCategory) & "'"
            lsSQLQuery &= " AND DataLineagePropertyType = 'Specification Document Name'"
            lsSQLQuery &= " AND DataLineageProperty = '" & asFilterDocumentName.Trim & "'"

            lRecordset.Open(lsSQLQuery)

            While Not lRecordset.EOF

                laiMetaDataLineageSetNumber.Add(lRecordset("LineageSetNumber").Value)

                lRecordset.MoveNext()
            End While

            Return laiMetaDataLineageSetNumber

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)

            Return laiMetaDataLineageSetNumber

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
            prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)

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
            prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)

            Return Nothing
        End Try

    End Function

    Public Sub ModifyKeyDataLineageItemProperty(ByRef arModel As FBM.Model,
                                                ByRef asOldDataLineageItemName As String,
                                                ByRef asNewDataLineageItemName As String)

        Dim lsSQLQuery As String

        Try
            lsSQLQuery = " UPDATE DataLineageItemProperty"
            lsSQLQuery &= "   SET ModelId = '" & Trim(arModel.ModelId) & "'"
            lsSQLQuery &= "      ,DataLineageItemName = '" & Trim(Replace(asNewDataLineageItemName, "'", "`")) & "'"
            lsSQLQuery &= " WHERE ModelId = '" & Trim(arModel.ModelId) & "'"
            lsSQLQuery &= "   AND DataLineageItemName = '" & Trim(asOldDataLineageItemName) & "'"

            pdbConnection.BeginTrans()
            pdbConnection.Execute(lsSQLQuery)
            pdbConnection.CommitTrans()

        Catch ex As Exception

            Dim lsMessage1 As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage1 &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)

            pdbConnection.RollbackTrans()
        End Try

    End Sub


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
            prApplication.ThrowMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)

            pdbConnection.RollbackTrans()
        End Try

    End Sub

End Module
