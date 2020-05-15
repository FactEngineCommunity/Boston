Imports System.Reflection

Namespace Language

    Public Module TableLanguagePhrase

        Sub AddLanguagePhrase(ByVal arLanguagePhrase As Language.LanguagePhrase)

            Dim lsSQLQuery As String = ""

            Try
                lsSQLQuery = "INSERT INTO LanguagePhrase"
                lsSQLQuery &= " VALUES ("
                lsSQLQuery &= " " & Trim(Replace(arLanguagePhrase.PhraseId.ToString, "'", "`"))
                lsSQLQuery &= " ,'" & Trim(Replace(arLanguagePhrase.PhraseType.ToString, "'", "`")) & "'"                
                lsSQLQuery &= " ," & NullVal(arLanguagePhrase.ResolvesToLanguagePhraseId, "Null")
                lsSQLQuery &= " ,'" & Database.MakeStringSafe(arLanguagePhrase.Example) & "'"
                lsSQLQuery &= ")"

                Call pdbConnection.Execute(lsSQLQuery)
            Catch ex As Exception
                Dim lsMessage As String
                lsMessage = "Error: TableLanguagePhrase.AddLanguagePhrase"
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try
        End Sub

        Public Sub DeleteLanguagePhrase(ByVal arLanguagePhrase As Language.LanguagePhrase)

            Dim lsSQLQuery As String = ""

            lsSQLQuery = "DELETE FROM LanguagePhrase"
            lsSQLQuery &= " WHERE PhraseId = " & Replace(arLanguagePhrase.PhraseId.ToString, "'", "`")

            pdbConnection.BeginTrans()
            pdbConnection.Execute(lsSQLQuery)
            pdbConnection.CommitTrans()

        End Sub

        Public Function ExistsLanguagePhrase(ByVal arLanguagePhrase As Language.LanguagePhrase) As Boolean

            Dim lsSQLQuery As String = ""
            Dim lREcordset As New ADODB.Recordset

            '------------------------
            'Initialise return value
            '------------------------
            ExistsLanguagePhrase = False

            lREcordset.ActiveConnection = pdbConnection
            lREcordset.CursorType = pcOpenStatic

            lsSQLQuery = "SELECT COUNT(*)"
            lsSQLQuery &= "  FROM LanguagePhrase"
            lsSQLQuery &= " WHERE PhraseId = " & Trim(Replace(arLanguagePhrase.PhraseId.ToString, "'", "`"))

            lREcordset.Open(lsSQLQuery)

            If lREcordset(0).Value > 0 Then
                ExistsLanguagePhrase = True
            Else
                ExistsLanguagePhrase = False
            End If

            lREcordset.Close()

        End Function


        Sub GetLanguagePhraseDetails(ByRef arLanguagePhrase As Language.LanguagePhrase)

            Dim lsSQLQuery As String = ""
            Dim lREcordset As New ADODB.Recordset

            Try
                lREcordset.ActiveConnection = pdbConnection
                lREcordset.CursorType = pcOpenStatic

                lsSQLQuery = "SELECT *"
                lsSQLQuery &= " FROM LanguagePhrase"
                lsSQLQuery &= " WHERE PhraseId = " & Trim(arLanguagePhrase.PhraseId.ToString)

                lREcordset.Open(lsSQLQuery)

                If Not lREcordset.EOF Then
                    arLanguagePhrase.PhraseId = lREcordset("PhraseId").Value
                    arLanguagePhrase.PhraseType = CType([Enum].Parse(GetType(pcenumPhraseType), lREcordset("PhraseType").Value), pcenumPhraseType)
                    arLanguagePhrase.ResolvesToPhrase = New Language.LanguagePhrase
                    If lREcordset("ResolvesToPhraseId").Value.GetType Is GetType(DBNull) Then
                        arLanguagePhrase.ResolvesToPhrase.PhraseId = 0
                    Else
                        arLanguagePhrase.ResolvesToPhrase.PhraseId = lREcordset("ResolvesToPhraseId").Value
                    End If


                    Call TableLanguagePhraseTokenSequenceWordSense.GetLPTSWSByPhrase(arLanguagePhrase)

                    If arLanguagePhrase.PhraseType = pcenumPhraseType.Resolving Then
                        TableLanguagePhrase.GetLanguagePhraseDetails(arLanguagePhrase.ResolvesToPhrase)
                    End If

                Else
                    Dim lsMessage As String = "No LanguagePhrase returned for LanguagePhraseId: " & arLanguagePhrase.PhraseId
                    Throw New Exception(lsMessage)
                End If

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()
                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Public Function GetLanguagePhrasesByLanguage() As List(Of Language.LanguagePhrase)

            Dim lsMessage As String
            Dim lrLanguagePhrase As Language.LanguagePhrase
            Dim lsSQLQuery As String = ""
            Dim lREcordset As New ADODB.Recordset

            lREcordset.ActiveConnection = pdbConnection
            lREcordset.CursorType = pcOpenStatic

            '-----------------------------
            'Initialise the return value
            '-----------------------------
            GetLanguagePhrasesByLanguage = New List(Of Language.LanguagePhrase)

            Try
                '---------------------------------------------
                'First get EntityTypes with no ParentEntityId
                '---------------------------------------------
                lsSQLQuery = " SELECT LP.*"
                lsSQLQuery &= "  FROM LanguagePhrase LP"
                'lsSQLQuery &= " WHERE MD.ModelId = '" & Trim(arModel.ModelId) & "'"

                lREcordset.Open(lsSQLQuery)

                If Not lREcordset.EOF Then
                    While Not lREcordset.EOF
                        lrLanguagePhrase = New Language.LanguagePhrase

                        lrLanguagePhrase.PhraseId = lREcordset("PhraseId").Value.ToString

                        Call TableLanguagePhrase.GetLanguagePhraseDetails(lrLanguagePhrase)

                        GetLanguagePhrasesByLanguage.Add(lrLanguagePhrase)

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

        Public Sub UpdateLanguagePhrase(ByVal arLanguagePhrase As Language.LanguagePhrase)

            Dim lsSQLQuery As String

            Try
                lsSQLQuery = " UPDATE LanguagePhrase"
                lsSQLQuery &= "   SET PhraseType = '" & Trim(Replace(arLanguagePhrase.PhraseType.toString, "'", "`")) & "',"
                lsSQLQuery &= "       ResolvesToPhraseId = " & NullVal(Trim(arLanguagePhrase.ResolvesToLanguagePhraseId), "Null") & ","
                lsSQLQuery &= "       Example = '" & Database.MakeStringSafe(arLanguagePhrase.Example) & "'"
                lsSQLQuery &= " WHERE PhraseId = " & Trim(Replace(arLanguagePhrase.PhraseId.ToString, "'", "`"))

                pdbConnection.BeginTrans()
                pdbConnection.Execute(lsSQLQuery)
                pdbConnection.CommitTrans()

            Catch ex As Exception
                Dim lsMessage As String
                lsMessage = "Error: TableLanguagePhrase.UpdateLanguagePhrase"
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

    End Module
End Namespace