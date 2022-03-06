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
                lsSQLQuery &= " '" & Trim(Replace(arUnifiedOntology.Name, "'", "`")) & "'"
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
            Dim lREcordset As New ADODB.Recordset

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
            Dim lREcordset As New ADODB.Recordset

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
            Dim lREcordset As New ADODB.Recordset

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

                    Call tableUnifiedOntologyModel.getModelsForUnifiedOntology(arUnifiedOntology)

                    For Each lrModel In arUnifiedOntology.Model
                        TableModelDictionary.GetDictionaryEntriesByModel(lrModel, True)
                    Next
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
            Dim lREcordset As New ADODB.Recordset

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

        Public Sub UpdateUnifiedOntology(ByVal arUnifiedOntology As Ontology.UnifiedOntology)

            Dim lsSQLQuery As String

            Try
                lsSQLQuery = " UPDATE ModelUnifiedOntology"
                lsSQLQuery &= "   SET UnifiedOntologyName = '" & Trim(Replace(arUnifiedOntology.Name, "'", "`")) & "'"
                lsSQLQuery &= " WHERE Id = '" & Trim(Replace(arUnifiedOntology.Id, "'", "`")) & "'"

                pdbConnection.BeginTrans()
                pdbConnection.Execute(lsSQLQuery)
                pdbConnection.CommitTrans()

            Catch ex As Exception

                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)

                pdbConnection.RollbackTrans()
            End Try

        End Sub

    End Module
End Namespace