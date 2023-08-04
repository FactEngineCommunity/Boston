Imports System.Data.SQLite
Imports System.Linq.Expressions
Imports Newtonsoft.Json
Imports Newtonsoft.Json.Serialization
Imports System.IO
Imports System.Text.RegularExpressions
Imports System.Reflection

Namespace DataStore
    Public Class [Store]

        'EXAMPLE
        ' Define the LINQ expression for the condition
        'Dim lrDataStore As New DataStore.Store
        'Dim whereClause As Expression(Of Func(Of FBM.DictionaryEntry, Boolean)) = Function(p) p.Symbol = "Satellite"
        'Dim larDictionaryEntry As List(Of FBM.DictionaryEntry) = lrDataStore.GetData(Of FBM.DictionaryEntry)(whereClause)

        ''' <summary>
        '''  AddObject method to add a new record to the DataStore table
        ''' </summary>
        ''' <typeparam name="T"></typeparam>
        ''' <param name="arObject"></param>
        Public Sub Add(arObject As Object)
            Try
                'Usage Example:
                'Dim lrEntityType As New FBM.EntityType(Nothing, pcenumLanguage.ORMModel, "Test Entity Type", Nothing, True)
                'Dim lrDataStore As New DataStore.Store
                'Call lrDataStore.AddObject(lrEntityType)

                ' Create the custom JSON serializer settings
                Dim settings As New JsonSerializerSettings With {
                    .Formatting = Formatting.Indented,
                   .TypeNameHandling = TypeNameHandling.Objects,
                   .ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                   }

                ' Serialize the object into JSON with custom settings
                Dim jsonData As String = JsonConvert.SerializeObject(arObject, settings)

                ' Get the type name of the object
                Dim typeName As String = arObject.GetType.FullName

                ' Create the SQL query to insert the new record into the DataStore table
                Dim lrData As New DataStore.Data(jsonData, typeName, Now, "", "", "", "", "")

                ' Execute the insert query
                Call tableDataStore.AddData(lrData)

            Catch ex As Exception
                ' Handle the exception appropriately
                ' ...
            End Try
        End Sub

        ''' <summary>
        ''' Delete method
        ''' </summary>
        ''' <typeparam name="T"></typeparam>
        ''' <param name="whereClause"></param>
        Public Sub Delete(Of T)(whereClause As Expression(Of Func(Of T, Boolean)))
            Try
                'Usage Examples
                'Dim whereClause As Expression(Of Func(Of FBM.DictionaryEntry, Boolean)) = Function(p) p.Symbol = "Satellite"
                'Dim larDictionaryEntry As List(Of FBM.DictionaryEntry) = lrDataStore.GetData(Of FBM.DictionaryEntry)(whereClause)
                'or
                '            Dim lrEntityType As New FBM.EntityType(Nothing, pcenumLanguage.ORMModel, "Test Entity Type", Nothing, True)
                'Dim whereClause As Expression(Of Func(Of FBM.EntityType, Boolean)) = Function(p) p.Id = "Test Entity Type"
                'Dim lrDataStore As New DataStore.Store
                'Call lrDataStore.DeleteData(Of FBM.EntityType)(whereClause)

                ' Get the record that matches the provided whereClause using GetData function
                Dim asID As String = Nothing
                Dim dataList As List(Of T) = Me.Get(Of T)(whereClause, asID)

                ' Assuming there should be only one record that matches the condition
                If dataList.Count >= 1 And asID IsNot Nothing Then
                    Dim recordToUpdate As T = dataList(0)

                    ' Now you can update the record back in the database using prConnection.Execute
                    ' Here's the SQL query to update the Data field for the specific record identified by ID
                    Dim lsSQLQuery As String = "DELETE FROM DataStore WHERE ID = '" & asID & "'"

                    ' Execute the update query
                    Call pdbConnection.Execute(lsSQLQuery)
                End If

            Catch ex As Exception
                ' Handle the exception appropriately
                ' ...
            End Try
        End Sub

        Public Function [Get](Of T)(Optional whereClause As Expression(Of Func(Of T, Boolean)) = Nothing, Optional ByRef asID As String = Nothing) As List(Of T)

            Dim dataList As New List(Of T)()

            Try
                'Usage Example:
                'Dim whereClause As Expression(Of Func(Of FBM.EntityType, Boolean)) = Function(p) p.Id = "Test Entity Type"
                'Dim lrDataStore As New DataStore.Store
                'Dim larDictionaryEntry As List(Of FBM.EntityType) = lrDataStore.GetData(Of FBM.EntityType)(whereClause)

                Dim typeName As String = GetType(T).FullName

                Dim lsSQLQuery As String = "SELECT ID, Data FROM DataStore WHERE json_extract(Data, '$.$type') = '" & typeName & ", " & Assembly.GetExecutingAssembly().GetName().Name & "'"

                Dim lrRecordset As New RecordsetProxy
                lrRecordset.ActiveConnection = pdbConnection
                lrRecordset.CursorType = pcOpenStatic

                Call lrRecordset.Open(lsSQLQuery)

                While Not lrRecordset.EOF
                    Dim jsonData As String = lrRecordset("Data").Value
                    Dim jsonReader As JsonTextReader = New JsonTextReader(New StringReader(jsonData))
                    Dim data As T = JsonSerializer.Create().Deserialize(Of T)(jsonReader)

                    If whereClause Is Nothing OrElse whereClause.Compile()(data) Then
                        dataList.Add(data)
                    End If

                    asID = lrRecordset("ID").value

                    lrRecordset.MoveNext()
                End While

                lrRecordset.Close()

                Return dataList

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
            End Try

        End Function

        ''' <summary>
        ''' Update function to replace the JSON in the Data field with Newtonsoft serialization
        ''' NB Only operates where one record is returned from inner Get. I.e. You should aim to update only one Document.
        ''' </summary>
        ''' <typeparam name="t"></typeparam>
        ''' <param name="arObject"></param>
        ''' <param name="whereClause"></param>
        Public Sub Update(Of t)(arObject As Object, whereClause As Expression(Of Func(Of t, Boolean)))
            Try
                'Usage examples
                'Dim whereClause As Expression(Of Func(Of FBM.DictionaryEntry, Boolean)) = Function(p) p.Symbol = "Satellite"
                'Dim larDictionaryEntry As List(Of FBM.DictionaryEntry) = lrDataStore.GetData(Of FBM.DictionaryEntry)(whereClause)
                'or
                'Dim lrEntityType As New FBM.EntityType(Nothing, pcenumLanguage.ORMModel, "Test Entity Type", Nothing, True)
                'Dim whereClause As Expression(Of Func(Of FBM.EntityType, Boolean)) = Function(p) p.Id = "Test Entity Type"
                'lrEntityType.Symbol = "Testaddb"
                'Dim lrDataStore As New DataStore.Store
                'Call lrDataStore.UpdateData(Of FBM.EntityType)(lrEntityType, whereClause)

                ' Get the record that matches the provided whereClause using GetData function
                Dim asID As String = Nothing

                Dim dataList As List(Of t) = Me.Get(Of t)(whereClause, asID) 'Id is byRef and Updated by Get.

                ' Assuming there should be only one record that matches the condition
                If dataList.Count = 1 AndAlso asID IsNot Nothing Then
                    Dim recordToUpdate = dataList(0)

                    ' Create the custom JSON serializer settings
                    Dim settings As New JsonSerializerSettings With {
                                .Formatting = Formatting.Indented,
                                .TypeNameHandling = TypeNameHandling.Objects,
                                .ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                            }

                    ' Serialize the object into JSON with custom settings
                    Dim jsonData As String = JsonConvert.SerializeObject(arObject, settings)

                    ' Now you can update the record back in the database using prConnection.Execute
                    ' Here's the SQL query to update the Data field for the specific record identified by ID
                    Dim lsSQLQuery As String = "UPDATE DataStore SET Data = '" & jsonData & "' WHERE ID = '" & asID & "'"

                    ' Execute the update query
                    Dim lrRecordset As ORMQL.Recordset = pdbConnection.Execute(lsSQLQuery)

                    If lrRecordset.ErrorReturned Then
                        Throw New Exception(lrRecordset.ErrorString)
                    End If

                End If

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
            End Try
        End Sub

        ''' <summary>
        ''' Upsert method to either update or insert a record in the DataStore table
        ''' </summary>
        ''' <typeparam name="T"></typeparam>
        ''' <param name="arObject"></param>
        ''' <param name="whereClause"></param>
        Public Sub Upsert(Of T)(arObject As Object, whereClause As Expression(Of Func(Of T, Boolean)))
            Try
                ' Get the record that matches the provided whereClause using GetData function
                Dim asID As String = Nothing
                Dim dataList As List(Of T) = Me.Get(Of T)(whereClause, asID)

                ' Serialize the object into JSON with custom settings
                Dim settings As New JsonSerializerSettings With {
                    .Formatting = Formatting.Indented,
                    .TypeNameHandling = TypeNameHandling.Objects,
                    .ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                }
                Dim jsonData As String = JsonConvert.SerializeObject(arObject, settings)

                If dataList.Count = 1 AndAlso asID IsNot Nothing Then
                    ' If the record exists, update it in the database
                    Dim lsSQLQuery As String = "UPDATE DataStore SET Data = '" & jsonData & "' WHERE ID = '" & asID & "'"
                    Dim lrRecordset As ORMQL.Recordset = pdbConnection.Execute(lsSQLQuery)

                    If lrRecordset.ErrorReturned Then
                        Throw New Exception(lrRecordset.ErrorString)
                    End If
                Else
                    ' If the record doesn't exist, insert it into the database
                    Dim lrData As New DataStore.Data(jsonData, GetType(T).FullName, Now, "", "", "", "", "")
                    Call tableDataStore.AddData(lrData)
                End If

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
            End Try
        End Sub

    End Class

End Namespace
