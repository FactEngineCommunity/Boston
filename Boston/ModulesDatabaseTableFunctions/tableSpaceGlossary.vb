Imports System.Reflection
Imports System.Text.RegularExpressions

Namespace TableSpaceGlossary

    Public Module TableSpaceGlossary


        Public Function GetGlossaryEntryDetailsForModel(ByRef arModel As FBM.Model) As List(Of FBM.DictionaryEntry)

            Dim lsMessage As String

            Try
                Dim lrDictionaryEntry As FBM.DictionaryEntry
                Dim lsSQLQuery As String = ""
                Dim lRecordset As New ADODB.Recordset

                lRecordset.ActiveConnection = pdbConnection
                lRecordset.CursorType = pcOpenStatic

                '-----------------------------
                'Initialise the return value
                '-----------------------------
                GetGlossaryEntryDetailsForModel = New List(Of FBM.DictionaryEntry)


                '---------------------------------------------
                'First get EntityTypes with no ParentEntityId
                '---------------------------------------------
                lsSQLQuery = " SELECT *"
                lsSQLQuery &= "  FROM SpaceGlossary"

                lRecordset.Open(lsSQLQuery)

                If Not lRecordset.EOF Then
                    While Not lRecordset.EOF
                        lrDictionaryEntry = New FBM.DictionaryEntry
                        lrDictionaryEntry.Model = arModel
                        lrDictionaryEntry.Concept = New FBM.Concept(lRecordset("Term").Value)
                        lrDictionaryEntry.Realisations.Add(lrDictionaryEntry.Concept)
                        lrDictionaryEntry.Symbol = lrDictionaryEntry.Concept.Symbol
                        lrDictionaryEntry.ShortDescription = Trim(VievLibrary.NullVal(lRecordset("ShortDescription").Value, ""))
                        lrDictionaryEntry.LongDescription = Trim(VievLibrary.NullVal(lRecordset("LongDescription").Value, ""))
                        lrDictionaryEntry.isGeneralConcept = True

                        lrDictionaryEntry = arModel.AddModelDictionaryEntry(lrDictionaryEntry)

                        GetGlossaryEntryDetailsForModel.Add(lrDictionaryEntry)
                        lRecordset.MoveNext()
                    End While
                End If

                lRecordset.Close()

            Catch ex As Exception
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)

                Return Nothing
            End Try

        End Function

    End Module

End Namespace