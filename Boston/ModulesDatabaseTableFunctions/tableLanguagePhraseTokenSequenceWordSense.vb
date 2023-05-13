Imports System.Reflection

Namespace Language

    Public Module TableLanguagePhraseTokenSequenceWordSense

        Sub AddLPTSWS(ByVal arLPTSWS As Language.LanguagePhraseTokenSequenceWordSense)

            Dim lsSQLQuery As String = ""

            Try
                lsSQLQuery = "INSERT INTO LanguagePhraseTokenSequenceWordSense"
                lsSQLQuery &= " VALUES ("
                lsSQLQuery &= " '" & Trim(Replace(arLPTSWS.Phrase.PhraseId, "'", "`")) & "'"
                lsSQLQuery &= " ,'" & Trim(Replace(arLPTSWS.Token, "'", "`")) & "'"
                lsSQLQuery &= " ,'" & Trim(Replace(arLPTSWS.SequenceNr.ToString, "'", "`")) & "'"
                lsSQLQuery &= " ,'" & Trim(arLPTSWS.WordSense.ToString) & "'"
                lsSQLQuery &= ")"

                Call pdbConnection.Execute(lsSQLQuery)

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try
        End Sub

        Public Sub DeleteValueType(ByVal arLPTSWS As Language.LanguagePhraseTokenSequenceWordSense)

            Dim lsSQLQuery As String = ""

            lsSQLQuery = "DELETE FROM LanguagePhraseTokenSequenceWordSense"
            lsSQLQuery &= " WHERE PhraseId = " & Replace(arLPTSWS.Phrase.PhraseId.ToString, "'", "`")
            lsSQLQuery &= "   AND Token = '" & Replace(arLPTSWS.Token, "'", "`") & "'"
            lsSQLQuery &= "   AND SequenceNr = '" & Replace(arLPTSWS.SequenceNr.ToString, "'", "`") & "'"
            lsSQLQuery &= "   AND WordSense = '" & Replace(arLPTSWS.WordSense.ToString, "'", "`") & "'"


            pdbConnection.BeginTrans()
            pdbConnection.Execute(lsSQLQuery)
            pdbConnection.CommitTrans()

        End Sub

        Public Function ExistsLPTSWS(ByRef arLPTSWS As Language.LanguagePhraseTokenSequenceWordSense) As Boolean

            Dim lsSQLQuery As String = ""
            Dim lREcordset As New RecordsetProxy

            '------------------------
            'Initialise return value
            '------------------------
            ExistsLPTSWS = False

            lREcordset.ActiveConnection = pdbConnection
            lREcordset.CursorType = pcOpenStatic

            lsSQLQuery = "SELECT COUNT(*)"
            lsSQLQuery &= "  FROM LanguagePhraseTokenSequenceWordSense"
            lsSQLQuery &= " WHERE PhraseId = " & arLPTSWS.Phrase.PhraseId.ToString
            lsSQLQuery &= "   AND Token = '" & Trim(Replace(arLPTSWS.Token, "'", "`")) & "'"
            lsSQLQuery &= "   AND SequenceNr = " & arLPTSWS.SequenceNr.ToString
            lsSQLQuery &= "   AND WordSense = '" & Trim(Replace(arLPTSWS.WordSense.ToString, "'", "`")) & "'"

            lREcordset.Open(lsSQLQuery)

            If lREcordset(0).Value > 0 Then
                ExistsLPTSWS = True
            Else
                ExistsLPTSWS = False
            End If

            lREcordset.Close()

        End Function

        Public Function GetLPTSWSByPhrase(ByRef arLanguagePhrase As Language.LanguagePhrase) As List(Of Language.LanguagePhraseTokenSequenceWordSense)

            Dim lsMessage As String
            Dim lrLPTSWS As Language.LanguagePhraseTokenSequenceWordSense
            Dim lsSQLQuery As String = ""
            Dim lREcordset As New RecordsetProxy

            lREcordset.ActiveConnection = pdbConnection
            lREcordset.CursorType = pcOpenStatic

            '-----------------------------
            'Initialise the return value
            '-----------------------------
            GetLPTSWSByPhrase = New List(Of Language.LanguagePhraseTokenSequenceWordSense)

            Try
                '---------------------------------------------
                'First get EntityTypes with no ParentEntityId
                '---------------------------------------------
                lsSQLQuery = " SELECT LPTSWS.*"
                lsSQLQuery &= "  FROM LanguagePhraseTokenSequenceWordSense LPTSWS"
                lsSQLQuery &= " WHERE LPTSWS.PhraseId = " & Trim(arLanguagePhrase.PhraseId.ToString)
                lsSQLQuery &= " ORDER BY SequenceNr"

                lREcordset.Open(lsSQLQuery)

                If Not lREcordset.EOF Then
                    While Not lREcordset.EOF
                        lrLPTSWS = New Language.LanguagePhraseTokenSequenceWordSense
                        lrLPTSWS.Phrase = arLanguagePhrase
                        lrLPTSWS.Token = lREcordset("Token").Value
                        lrLPTSWS.SequenceNr = CInt(lREcordset("SequenceNr").Value)
                        lrLPTSWS.WordSense = CType([Enum].Parse(GetType(pcenumWordSense), lREcordset("WordSense").Value), pcenumWordSense)

                        If lrLPTSWS.SequenceNr > arLanguagePhrase.TokenSequence.Count Then
                            arLanguagePhrase.TokenSequence.Add(New Language.LanguagePhraseTokenSequence(lrLPTSWS.Token, lrLPTSWS.SequenceNr))
                        End If

                        arLanguagePhrase.TokenSequence(lrLPTSWS.SequenceNr - 1).LPTSWS.Add(lrLPTSWS)

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

    End Module
End Namespace