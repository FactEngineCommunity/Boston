Imports WordNetClasses
Imports System.Reflection

Namespace Language

    Public Class LanguageGeneric
        Inherits WordNetClasses.WN

        Public Model As FBM.Model

        Public LanguagePhrase As New List(Of Language.LanguagePhrase)

        Public Sub New(ByVal asDictionaryPath)

            MyBase.New(asDictionaryPath)

        End Sub

        Public Function GetNounOverviewForWord(ByVal asWord As String) As String


            Dim lbIsNoun As Boolean = False
            Dim lrSearchSet As Wnlib.SearchSet = Nothing
            Dim larArrayList As New ArrayList

            Me.hasmatch = False

            Call Me.OverviewFor(asWord, "noun", lbIsNoun, lrSearchSet, larArrayList)

            If lbIsNoun Then
                Return larArrayList(0).word
            End If

            Return Nothing

        End Function



        Public Function WordIsNoun(ByVal asWord As String) As Boolean

            Dim lbIsNoun As Boolean = False
            Dim lrSearchSet As Wnlib.SearchSet = Nothing
            Dim larArrayList As New ArrayList

            Me.hasmatch = False

            Call Me.OverviewFor(asWord, "noun", lbIsNoun, lrSearchSet, larArrayList)

            Return lbIsNoun

        End Function

        Public Function WordIsVerb(ByVal asWord As String) As Boolean

            Dim lbIsVerb As Boolean = False
            Dim lrSearchSet As Wnlib.SearchSet = Nothing
            Dim larArrayList As New ArrayList
            Dim lsSQLQuery As String = ""
            Dim lrRecordset As ORMQL.Recordset

            Try
                Me.hasmatch = False


                Call Me.OverviewFor(asWord, "verb", lbIsVerb, lrSearchSet, larArrayList)

                If Not lbIsVerb Then

                    lsSQLQuery = "SELECT COUNT(*)"
                    lsSQLQuery &= " FROM WordIsVerb"
                    lsSQLQuery &= " WHERE Word = '" & Trim(asWord) & "'"

                    lrRecordset = Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                    If CInt(lrRecordset("Count").Data) > 0 Then
                        lbIsVerb = True
                    End If
                End If

                Return lbIsVerb

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)

                Return False
            End Try


        End Function

        Public Function WordIsAdjective(ByVal asWord As String) As Boolean

            Dim lbIsAdjective As Boolean = False
            Dim lrSearchSet As Wnlib.SearchSet = Nothing
            Dim larArrayList As New ArrayList

            Call Me.OverviewFor(asWord, "adj", lbIsAdjective, lrSearchSet, larArrayList)

            Return lbIsAdjective

        End Function

        Public Function WordIsAdverb(ByVal asWord As String) As Boolean

            Dim lbIsAdverb As Boolean = False
            Dim lrSearchSet As Wnlib.SearchSet = Nothing
            Dim larArrayList As New ArrayList

            Call Me.OverviewFor(asWord, "adv", lbIsAdverb, lrSearchSet, larArrayList)

            Return lbIsAdverb

        End Function

        Public Function WordIsArticle(ByVal asWord As String) As Boolean

            Dim lsSQLQuery As String = ""
            Dim lrRecordset As ORMQL.Recordset

            Try
                WordIsArticle = False

                lsSQLQuery = "SELECT COUNT(*)"
                lsSQLQuery &= " FROM WordIsArticle"
                lsSQLQuery &= " WHERE Word = '" & Trim(asWord) & "'"

                lrRecordset = Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                If CInt(lrRecordset("Count").Data) > 0 Then
                    WordIsArticle = True
                End If
            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Function

        Public Function WordIsPreposition(ByVal asWord As String) As Boolean

            Dim lsSQLQuery As String = ""
            Dim lrRecordset As ORMQL.Recordset

            Try
                WordIsPreposition = False

                lsSQLQuery = "SELECT COUNT(*)"
                lsSQLQuery &= " FROM WordIsPreposition"
                lsSQLQuery &= " WHERE Word = '" & Trim(asWord) & "'"

                lrRecordset = Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                If CInt(lrRecordset("Count").Data) > 0 Then
                    WordIsPreposition = True
                End If

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Function

        Public Function WordIsConjunction(ByVal asWord As String) As Boolean

            Dim lsSQLQuery As String = ""
            Dim lrRecordset As ORMQL.Recordset

            Try
                WordIsConjunction = False

                lsSQLQuery = "SELECT COUNT(*)"
                lsSQLQuery &= " FROM WordIsConjunction"
                lsSQLQuery &= " WHERE Word = '" & Trim(asWord) & "'"

                lrRecordset = Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                If CInt(lrRecordset("Count").Data) > 0 Then
                    WordIsConjunction = True
                End If

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Function

        Public Function WordIsDelimeter(ByVal asWord As String) As Boolean

            Dim lsSQLQuery As String = ""
            Dim lrRecordset As ORMQL.Recordset

            Try
                WordIsDelimeter = False

                lsSQLQuery = "SELECT COUNT(*)"
                lsSQLQuery &= " FROM WordIsDelimeter"
                lsSQLQuery &= " WHERE Word = '" & Trim(asWord) & "'"

                lrRecordset = Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                If CInt(lrRecordset("Count").Data) > 0 Then
                    WordIsDelimeter = True
                End If

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Function

        Public Function WordIsAlternativeAdditiveDeterminer(ByVal asWord As String) As Boolean

            Dim lsSQLQuery As String = ""
            Dim lrRecordset As ORMQL.Recordset

            Try
                WordIsAlternativeAdditiveDeterminer = False

                lsSQLQuery = "SELECT COUNT(*)"
                lsSQLQuery &= " FROM WordIsAlternativeAdditiveDeterminer"
                lsSQLQuery &= " WHERE Word = '" & Trim(asWord) & "'"

                lrRecordset = Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                If CInt(lrRecordset("Count").Data) > 0 Then
                    WordIsAlternativeAdditiveDeterminer = True
                End If

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Function

        Public Function WordIsCardinalNumber(ByVal asWord As String) As Boolean

            Dim lsSQLQuery As String = ""
            Dim lrRecordset As ORMQL.Recordset

            Try
                WordIsCardinalNumber = False

                lsSQLQuery = "SELECT COUNT(*)"
                lsSQLQuery &= " FROM WordIsCardinalNumber"
                lsSQLQuery &= " WHERE Word = '" & Trim(asWord) & "'"

                lrRecordset = Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                If CInt(lrRecordset("Count").Data) > 0 Then
                    WordIsCardinalNumber = True
                End If

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)                
            End Try

        End Function

        Public Function WordIsDegreeDeterminer(ByVal asWord As String) As Boolean

            Dim lsSQLQuery As String = ""
            Dim lrRecordset As ORMQL.Recordset

            Try
                WordIsDegreeDeterminer = False

                lsSQLQuery = "SELECT COUNT(*)"
                lsSQLQuery &= " FROM WordIsDegreeDeterminer"
                lsSQLQuery &= " WHERE Word = '" & Trim(asWord) & "'"

                lrRecordset = Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                If CInt(lrRecordset("Count").Data) > 0 Then
                    WordIsDegreeDeterminer = True
                End If

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Function

        Public Function WordIsDemonstrativeDeterminer(ByVal asWord As String) As Boolean

            Dim lsSQLQuery As String = ""
            Dim lrRecordset As ORMQL.Recordset

            Try
                WordIsDemonstrativeDeterminer = False

                lsSQLQuery = "SELECT COUNT(*)"
                lsSQLQuery &= " FROM WordIsDemonstrativeDeterminer"
                lsSQLQuery &= " WHERE Word = '" & Trim(asWord) & "'"

                lrRecordset = Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                If CInt(lrRecordset("Count").Data) > 0 Then
                    WordIsDemonstrativeDeterminer = True
                End If

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Function

        Public Function WordIsDisjunctiveDeterminer(ByVal asWord As String) As Boolean

            Dim lsSQLQuery As String = ""
            Dim lrRecordset As ORMQL.Recordset

            Try
                WordIsDisjunctiveDeterminer = False

                lsSQLQuery = "SELECT COUNT(*)"
                lsSQLQuery &= " FROM WordIsDisjunctiveDeterminer"
                lsSQLQuery &= " WHERE Word = '" & Trim(asWord) & "'"

                lrRecordset = Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                If CInt(lrRecordset("Count").Data) > 0 Then
                    WordIsDisjunctiveDeterminer = True
                End If

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Function

        Public Function WordIsDistributiveDeterminer(ByVal asWord As String) As Boolean

            Dim lsSQLQuery As String = ""
            Dim lrRecordset As ORMQL.Recordset

            Try
                WordIsDistributiveDeterminer = False

                lsSQLQuery = "SELECT COUNT(*)"
                lsSQLQuery &= " FROM WordIsDistributiveDeterminer"
                lsSQLQuery &= " WHERE Word = '" & Trim(asWord) & "'"

                lrRecordset = Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                If CInt(lrRecordset("Count").Data) > 0 Then
                    WordIsDistributiveDeterminer = True
                End If

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Function

        Public Function WordIsElectiveDeterminer(ByVal asWord As String) As Boolean

            Dim lsSQLQuery As String = ""
            Dim lrRecordset As ORMQL.Recordset

            Try
                WordIsElectiveDeterminer = False

                lsSQLQuery = "SELECT COUNT(*)"
                lsSQLQuery &= " FROM WordIsElectiveDeterminer"
                lsSQLQuery &= " WHERE Word = '" & Trim(asWord) & "'"

                lrRecordset = Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                If CInt(lrRecordset("Count").Data) > 0 Then
                    WordIsElectiveDeterminer = True
                End If

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Function

        Public Function WordIsEqualitiveDeterminer(ByVal asWord As String) As Boolean

            Dim lsSQLQuery As String = ""
            Dim lrRecordset As ORMQL.Recordset

            Try
                WordIsEqualitiveDeterminer = False

                lsSQLQuery = "SELECT COUNT(*)"
                lsSQLQuery &= " FROM WordIsEqualitiveDeterminer"
                lsSQLQuery &= " WHERE Word = '" & Trim(asWord) & "'"

                lrRecordset = Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                If CInt(lrRecordset("Count").Data) > 0 Then
                    WordIsEqualitiveDeterminer = True
                End If

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Function

        Public Function WordIsEvaluativeDeterminer(ByVal asWord As String) As Boolean

            Dim lsSQLQuery As String = ""
            Dim lrRecordset As ORMQL.Recordset

            Try
                WordIsEvaluativeDeterminer = False

                lsSQLQuery = "SELECT COUNT(*)"
                lsSQLQuery &= " FROM WordIsEvaluativeDeterminer"
                lsSQLQuery &= " WHERE Word = '" & Trim(asWord) & "'"

                lrRecordset = Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                If CInt(lrRecordset("Count").Data) > 0 Then
                    WordIsEvaluativeDeterminer = True
                End If

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Function

        Public Function WordIsExclamativeDeterminer(ByVal asWord As String) As Boolean

            Dim lsSQLQuery As String = ""
            Dim lrRecordset As ORMQL.Recordset

            Try
                WordIsExclamativeDeterminer = False

                lsSQLQuery = "SELECT COUNT(*)"
                lsSQLQuery &= " FROM WordIsExclamativeDeterminer"
                lsSQLQuery &= " WHERE Word = '" & Trim(asWord) & "'"

                lrRecordset = Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                If CInt(lrRecordset("Count").Data) > 0 Then
                    WordIsExclamativeDeterminer = True
                End If

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Function

        Public Function WordIsExistentialDeterminer(ByVal asWord As String) As Boolean

            Dim lsSQLQuery As String = ""
            Dim lrRecordset As ORMQL.Recordset

            Try
                WordIsExistentialDeterminer = False

                lsSQLQuery = "SELECT COUNT(*)"
                lsSQLQuery &= " FROM WordIsExistentialDeterminer"
                lsSQLQuery &= " WHERE Word = '" & Trim(asWord) & "'"

                lrRecordset = Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                If CInt(lrRecordset("Count").Data) > 0 Then
                    WordIsExistentialDeterminer = True
                End If

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Function

        Public Function WordIsInterrogativeDeterminer(ByVal asWord As String) As Boolean

            Dim lsSQLQuery As String = ""
            Dim lrRecordset As ORMQL.Recordset

            Try
                WordIsInterrogativeDeterminer = False

                lsSQLQuery = "SELECT COUNT(*)"
                lsSQLQuery &= " FROM WordIsInterrogativeDeterminer"
                lsSQLQuery &= " WHERE Word = '" & Trim(asWord) & "'"

                lrRecordset = Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                If CInt(lrRecordset("Count").Data) > 0 Then
                    WordIsInterrogativeDeterminer = True
                End If

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Function

        Public Function WordIsNegativeDeterminer(ByVal asWord As String) As Boolean

            Dim lsSQLQuery As String = ""
            Dim lrRecordset As ORMQL.Recordset

            Try
                WordIsNegativeDeterminer = False

                lsSQLQuery = "SELECT COUNT(*)"
                lsSQLQuery &= " FROM WordIsNegativeDeterminer"
                lsSQLQuery &= " WHERE Word = '" & Trim(asWord) & "'"

                lrRecordset = Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                If CInt(lrRecordset("Count").Data) > 0 Then
                    WordIsNegativeDeterminer = True
                End If

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Function

        Public Function WordIsPersonalDeterminer(ByVal asWord As String) As Boolean

            Dim lsSQLQuery As String = ""
            Dim lrRecordset As ORMQL.Recordset

            Try
                WordIsPersonalDeterminer = False

                lsSQLQuery = "SELECT COUNT(*)"
                lsSQLQuery &= " FROM WordIsPersonalDeterminer"
                lsSQLQuery &= " WHERE Word = '" & Trim(asWord) & "'"

                lrRecordset = Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                If CInt(lrRecordset("Count").Data) > 0 Then
                    WordIsPersonalDeterminer = True
                End If

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Function

        Public Function WordIsPositiveMultalDeterminer(ByVal asWord As String) As Boolean

            Dim lsSQLQuery As String = ""
            Dim lrRecordset As ORMQL.Recordset

            Try
                WordIsPositiveMultalDeterminer = False

                lsSQLQuery = "SELECT COUNT(*)"
                lsSQLQuery &= " FROM WordIsPositiveMultalDeterminer"
                lsSQLQuery &= " WHERE Word = '" & Trim(asWord) & "'"

                lrRecordset = Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                If CInt(lrRecordset("Count").Data) > 0 Then
                    WordIsPositiveMultalDeterminer = True
                End If

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Function

        Public Function WordIsPositivePaucal(ByVal asWord As String) As Boolean

            Dim lsSQLQuery As String = ""
            Dim lrRecordset As ORMQL.Recordset

            Try
                WordIsPositivePaucal = False

                lsSQLQuery = "SELECT COUNT(*)"
                lsSQLQuery &= " FROM WordIsPositivePaucal"
                lsSQLQuery &= " WHERE Word = '" & Trim(asWord) & "'"

                lrRecordset = Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                If CInt(lrRecordset("Count").Data) > 0 Then
                    WordIsPositivePaucal = True
                End If

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Function

        Public Function WordIsPossessiveDeterminer(ByVal asWord As String) As Boolean

            Dim lsSQLQuery As String = ""
            Dim lrRecordset As ORMQL.Recordset

            Try
                WordIsPossessiveDeterminer = False

                lsSQLQuery = "SELECT COUNT(*)"
                lsSQLQuery &= " FROM WordIsPossessiveDeterminer"
                lsSQLQuery &= " WHERE Word = '" & Trim(asWord) & "'"

                lrRecordset = Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                If CInt(lrRecordset("Count").Data) > 0 Then
                    WordIsPossessiveDeterminer = True
                End If

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Function

        Public Function WordIsQualitativeDeterminer(ByVal asWord As String) As Boolean

            Dim lsSQLQuery As String = ""
            Dim lrRecordset As ORMQL.Recordset

            Try
                WordIsQualitativeDeterminer = False

                lsSQLQuery = "SELECT COUNT(*)"
                lsSQLQuery &= " FROM WordIsQualitativeDeterminer"
                lsSQLQuery &= " WHERE Word = '" & Trim(asWord) & "'"

                lrRecordset = Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                If CInt(lrRecordset("Count").Data) > 0 Then
                    WordIsQualitativeDeterminer = True
                End If

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Function

        Public Function WordIsQuantifier(ByVal asWord As String) As Boolean

            Dim lsSQLQuery As String = ""
            Dim lrRecordset As ORMQL.Recordset

            Try
                WordIsQuantifier = False

                lsSQLQuery = "SELECT COUNT(*)"
                lsSQLQuery &= " FROM WordIsQuantifier"
                lsSQLQuery &= " WHERE Word = '" & Trim(asWord) & "'"

                lrRecordset = Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                If CInt(lrRecordset("Count").Data) > 0 Then
                    WordIsQuantifier = True
                End If

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Function

        Public Function WordIsRelativeDeterminer(ByVal asWord As String) As Boolean

            Dim lsSQLQuery As String = ""
            Dim lrRecordset As ORMQL.Recordset

            Try
                WordIsRelativeDeterminer = False

                lsSQLQuery = "SELECT COUNT(*)"
                lsSQLQuery &= " FROM WordIsRelativeDeterminer"
                lsSQLQuery &= " WHERE Word = '" & Trim(asWord) & "'"

                lrRecordset = Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                If CInt(lrRecordset("Count").Data) > 0 Then
                    WordIsRelativeDeterminer = True
                End If

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Function

        Public Function WordIsSufficiencyDeterminer(ByVal asWord As String) As Boolean

            Dim lsSQLQuery As String = ""
            Dim lrRecordset As ORMQL.Recordset

            Try
                WordIsSufficiencyDeterminer = False

                lsSQLQuery = "SELECT COUNT(*)"
                lsSQLQuery &= " FROM WordIsSufficiencyDeterminer"
                lsSQLQuery &= " WHERE Word = '" & Trim(asWord) & "'"

                lrRecordset = Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                If CInt(lrRecordset("Count").Data) > 0 Then
                    WordIsSufficiencyDeterminer = True
                End If

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Function

        Public Function WordIsSubordinateConjunction(ByVal asWord As String) As Boolean

            Dim lsSQLQuery As String = ""
            Dim lrRecordset As ORMQL.Recordset

            Try
                WordIsSubordinateConjunction = False

                lsSQLQuery = "SELECT COUNT(*)"
                lsSQLQuery &= " FROM WordIsSubordinateConjunction"
                lsSQLQuery &= " WHERE Word = '" & Trim(asWord) & "'"

                lrRecordset = Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                If CInt(lrRecordset("Count").Data) > 0 Then
                    WordIsSubordinateConjunction = True
                End If

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Function

        Public Function WordIsUniquitiveDeterminer(ByVal asWord As String) As Boolean

            Dim lsSQLQuery As String = ""
            Dim lrRecordset As ORMQL.Recordset

            Try
                WordIsUniquitiveDeterminer = False

                lsSQLQuery = "SELECT COUNT(*)"
                lsSQLQuery &= " FROM WordIsUniquitiveDeterminer"
                lsSQLQuery &= " WHERE Word = '" & Trim(asWord) & "'"

                lrRecordset = Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                If CInt(lrRecordset("Count").Data) > 0 Then
                    WordIsUniquitiveDeterminer = True
                End If

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Function

        Public Function WordIsUniversalDeterminer(ByVal asWord As String) As Boolean

            Dim lsSQLQuery As String = ""
            Dim lrRecordset As ORMQL.Recordset

            Try
                WordIsUniversalDeterminer = False

                lsSQLQuery = "SELECT COUNT(*)"
                lsSQLQuery &= " FROM WordIsUniversalDeterminer"
                lsSQLQuery &= " WHERE Word = '" & Trim(asWord) & "'"

                lrRecordset = Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                If CInt(lrRecordset("Count").Data) > 0 Then
                    WordIsUniversalDeterminer = True
                End If

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Function

    End Class

End Namespace
