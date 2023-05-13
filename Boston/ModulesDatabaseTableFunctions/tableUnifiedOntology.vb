Imports System.Reflection
Imports System.Text.RegularExpressions

Namespace TableUnifiedOntology

    Public Module TableUnifiedOntology

        Sub AddUnifiedOntology(ByVal arUnifiedOntology As Ontology.UnifiedOntology)

            Dim lsSQLQuery As String = ""

            Try
                lsSQLQuery = "INSERT INTO UnifiedOntology"
                lsSQLQuery &= " VALUES ("
                lsSQLQuery &= " '" & Trim(Replace(arUnifiedOntology.Id, "'", "`")) & "'"
                lsSQLQuery &= ",'" & Trim(Replace(arUnifiedOntology.Name, "'", "`")) & "'"
                lsSQLQuery &= ",'" & Trim(Replace(arUnifiedOntology.ImageFileLocationName, "'", "`")) & "'"
                lsSQLQuery &= ")"

                Call pdbConnection.Execute(lsSQLQuery)
            Catch ex As Exception
                Dim lsMessage As String
                lsMessage = "Error: TableUnifiedOntology.AddUnifiedOntology"
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try
        End Sub

        Public Sub DeleteUnifiedOntology(ByVal arUnifiedOntology As Ontology.UnifiedOntology)

            Dim lsSQLQuery As String = ""

            lsSQLQuery = "DELETE FROM UnifiedOntology"
            lsSQLQuery &= " WHERE Id = '" & Replace(arUnifiedOntology.Id, "'", "`") & "'"

            pdbConnection.BeginTrans()
            pdbConnection.Execute(lsSQLQuery)
            pdbConnection.CommitTrans()

        End Sub

        Public Function ExistsUnifiedOntology(ByVal arUnifiedOntology As Ontology.UnifiedOntology) As Boolean

            Dim lsSQLQuery As String = ""
            Dim lREcordset As New RecordsetProxy

            '------------------------
            'Initialise return value
            '------------------------
            ExistsUnifiedOntology = False

            lREcordset.ActiveConnection = pdbConnection
            lREcordset.CursorType = pcOpenStatic

            lsSQLQuery = "SELECT COUNT(*)"
            lsSQLQuery &= "  FROM UnifiedOntology"
            lsSQLQuery &= " WHERE Id = '" & Trim(Replace(arUnifiedOntology.Id, "'", "`")) & "'"

            lREcordset.Open(lsSQLQuery)

            If lREcordset(0).Value > 0 Then
                ExistsUnifiedOntology = True
            Else
                ExistsUnifiedOntology = False
            End If

            lREcordset.Close()

        End Function

        Function GetUnifiedOntologyCount() As Integer


            Dim lsSQLQuery As String = ""
            Dim lREcordset As New RecordsetProxy

            lREcordset.ActiveConnection = pdbConnection
            lREcordset.CursorType = pcOpenStatic

            lsSQLQuery = " SELECT COUNT(*)"
            lsSQLQuery &= "  FROM UnifiedOntology"

            lREcordset.Open(lsSQLQuery)

            GetUnifiedOntologyCount = lREcordset(0).Value

            lREcordset.Close()

        End Function

        Sub GetUnifiedOntologyDetails(ByRef arUnifiedOntology As Ontology.UnifiedOntology)

            Dim lsSQLQuery As String = ""
            Dim lREcordset As New RecordsetProxy

            Try
                lREcordset.ActiveConnection = pdbConnection
                lREcordset.CursorType = pcOpenStatic

                lsSQLQuery = "SELECT *"
                lsSQLQuery &= " FROM UnifiedOntology"
                lsSQLQuery &= " WHERE Id = '" & Trim(arUnifiedOntology.Id) & "'"

                lREcordset.Open(lsSQLQuery)

                If Not lREcordset.EOF Then
                    arUnifiedOntology.Id = lREcordset("Id").Value
                    arUnifiedOntology.Name = lREcordset("UnifiedOntologyName").Value
                    arUnifiedOntology.ImageFileLocationName = lREcordset("ImageFileLocationName").Value

                    Call tableUnifiedOntologyModel.getModelsForUnifiedOntology(arUnifiedOntology)

                    For Each lrModel In arUnifiedOntology.Model
                        TableModelDictionary.GetDictionaryEntriesByModel(lrModel, True)
                    Next

                    'Using myConnection As New System.Data.OleDb.OleDbConnection(My.Settings.DatabaseConnectionString)
                    '    Dim SQL As String = "SELECT [Image] FROM [UnifiedOntology] WHERE Id = '" & Trim(arUnifiedOntology.Id) & "'"
                    '    Using myCommand As New System.Data.OleDb.OleDbCommand(SQL, myConnection)
                    '        myConnection.Open()
                    '        Using myReader As System.Data.OleDb.OleDbDataReader = myCommand.ExecuteReader
                    '            If myReader.Read Then

                    '                Dim barray() As Byte = myReader("Image")

                    '                Using mStream As New System.IO.MemoryStream()
                    '                    mStream.Write(barray, 78, barray.Length - 78)

                    '                    arUnifiedOntology.Image = System.Drawing.Image.FromStream(mStream, True)

                    '                    'OR TRY
                    '                    'Dim tc As TypeConverter = TypeDescriptor.GetConverter(GetType(Bitmap))
                    '                    'Dim bitmap1 As Bitmap = CType(tc.ConvertFrom(byteArray), Bitmap)

                    '                End Using

                    '                'arUnifiedOntology.Image = myReader("Image")
                    '            End If
                    '            myReader.Close()
                    '        End Using

                    '    End Using
                    '    myConnection.Close()
                    'End Using
                Else
                    Dim lsMessage As String = "Error: GetUnifiedOntologyDetails: No UnifiedOntology returned for UnifiedOntologyName: " & arUnifiedOntology.Name
                    Throw New Exception(lsMessage)
                End If

                lREcordset.Close()

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Public Function GetUnifiedOntologies() As List(Of Ontology.UnifiedOntology)

            Dim lsMessage As String
            Dim lrUnifiedOntology As Ontology.UnifiedOntology
            Dim lsSQLQuery As String = ""
            Dim lREcordset As New RecordsetProxy

            lREcordset.ActiveConnection = pdbConnection
            lREcordset.CursorType = pcOpenStatic

            '-----------------------------
            'Initialise the return value
            '-----------------------------
            GetUnifiedOntologies = New List(Of Ontology.UnifiedOntology)

            Try
                '---------------------------------------------
                'First get EntityTypes with no ParentEntityId
                '---------------------------------------------
                lsSQLQuery = " SELECT *"
                lsSQLQuery &= "  FROM UnifiedOntology"

                lREcordset.Open(lsSQLQuery)

                If Not lREcordset.EOF Then
                    While Not lREcordset.EOF
                        lrUnifiedOntology = New Ontology.UnifiedOntology
                        lrUnifiedOntology.Id = lREcordset("Id").Value
                        lrUnifiedOntology.Name = lREcordset("UnifiedOntologyName").Value

                        GetUnifiedOntologies.Add(lrUnifiedOntology)
                        lREcordset.MoveNext()
                    End While
                End If

                lREcordset.Close()

            Catch ex As Exception
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Function

        Public Function UpdateUnifiedOntology(ByVal arUnifiedOntology As Ontology.UnifiedOntology) As Boolean

            Dim lsSQLQuery As String

            Try
                lsSQLQuery = " UPDATE UnifiedOntology"
                lsSQLQuery &= "   SET UnifiedOntologyName = '" & Trim(Replace(arUnifiedOntology.Name, "'", "`")) & "'"
                lsSQLQuery &= " WHERE Id = '" & Trim(Replace(arUnifiedOntology.Id, "'", "`")) & "'"

                pdbConnection.BeginTrans()
                pdbConnection.Execute(lsSQLQuery)
                pdbConnection.CommitTrans()

                Return True

            Catch ex As Exception

                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Warning, ex.StackTrace, False, False, True)

                pdbConnection.RollbackTrans()

                Return False
            End Try

        End Function

    End Module
End Namespace