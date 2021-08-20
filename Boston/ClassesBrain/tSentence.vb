Imports System.Reflection

Namespace Language

    Public Class Sentence
        Implements IEquatable(Of Language.Sentence)

        Public OriginalSentence As String = "" 'e.g. 'Building has BuildingName'
        Public SentenceId As String = System.Guid.NewGuid.ToString
        Public Sentence As String = "" 'e.g. 'a part is in a bin in a warehouse'
        Public SentenceResolved As String = "" 'e.g. 'Part is in Bin in Warehouse'
        Public WordList As New List(Of String)
        Public WordListQualification As New List(Of Language.WordQualification)
        Public WordListResolved As New List(Of Language.WordResolved)
        Public AllWordsIdentified As New List(Of String) 'All the words within the sentence exist within the Language ontology
        Public SentenceType As New List(Of pcenumSentenceType)
        Public POStaggingResolved As Boolean = False 'As in each word of the sentence has its sense resolved.
        Public ModelElement As New List(Of String)
        Public PredicatePart As New List(Of Language.PredicatePart)
        Public IsProcessed As Boolean = False
        Public ResolutionType As pcenumSentenceResolutionType

        Public FrontText As String = "" 'For use in creating FactTypeReadings.
        Public FollowingText As String = "" 'For use in creating FactTypeReadings.


        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="asSentence">e.g. 'part is in bin in warehouse'. NB The Brain may have already converted the input to lowercase.</param>
        ''' <param name="asOriginalSentence">e.g. 'Part is in Bin in Warehouse'. i.e. The sentence before the Brain converted the sentence to lowercase.</param>
        ''' <remarks></remarks>
        Sub New(ByVal asSentence As String, Optional ByVal asOriginalSentence As String = "")

            Dim lasWord() As String
            Dim lasOriginalWord() As String
            Dim lsWord As String = ""
            Dim liInd As Integer = 0

            Try
                Me.Sentence = Me.ReduceSpaces(Trim(asSentence))
                Me.OriginalSentence = Me.ReduceSpaces(Trim(asOriginalSentence))

                lasWord = Split(Me.Sentence, " ", -1, CompareMethod.Text)

                '20200515-Added IfThenElse below. Remove and fix if all not okay. Put in because if lasOrdinalWord is empty the code below that crashes.
                If asOriginalSentence = "" Then
                    lasOriginalWord = lasWord
                Else
                    lasOriginalWord = Split(Me.OriginalSentence, " ", -1, CompareMethod.Text)
                End If

                Dim charsToRemove As String() = {"@", ",", ".", ";", "'"}

                'Could use string clean = Regex.Replace(dirty, "[^A-Za-z0-9 ]", "");

                For Each lsWord In lasWord

                    'Remove offending characters
                    For Each c In charsToRemove
                        lsWord = lsWord.Replace(c, String.Empty)
                    Next

                    Me.WordList.Add(lsWord)
                    Me.WordListQualification.Add(New Language.WordQualification(lsWord))
                    Me.WordListQualification(liInd).OriginalWord = lasOriginalWord(liInd)
                    liInd += 1
                Next

                Me.SentenceType.Add(pcenumSentenceType.Unknown)

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Public Sub addResolvedNounsByFBMModelObjectList(ByRef aarModelObject As List(Of FBM.ModelObject))

            Try
                For Each lrModelObject In aarModelObject
                    Dim lrWordResolved = New Language.WordResolved(lrModelObject.Id, pcenumWordSense.Noun)
                    Me.WordListResolved.Add(lrWordResolved)
                Next
            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try
        End Sub


        Public Sub ResetSentence()

            Dim lasWord() As String
            Dim lsWord As String = ""

            Me.Sentence = Me.ReduceSpaces(Me.Sentence)

            lasWord = Split(Me.Sentence, " ", -1, CompareMethod.Text)

            Me.WordList.Clear()
            Me.WordListQualification.Clear()
            Me.WordListResolved.Clear()

            For Each lsWord In lasWord
                Me.WordList.Add(lsWord)
                Me.WordListQualification.Add(New Language.WordQualification(lsWord))
            Next

            Me.SentenceType.Clear()
            Me.SentenceType.Add(pcenumSentenceType.Unknown)

        End Sub

        Public Shadows Function Equals(ByVal other As Language.Sentence) As Boolean Implements System.IEquatable(Of Language.Sentence).Equals

            If Me.SentenceId = other.SentenceId Then
                Return True
            Else
                Return False
            End If

        End Function

        Public Function EqualsBySentence(ByVal other As Language.Sentence) As Boolean

            If Me.Sentence = other.Sentence Then
                Return True
            Else
                Return False
            End If

        End Function



        Public Function AreAllWordsResolved() As Boolean

            Dim lbResolved As Boolean = True


            If Me.WordListQualification.Exists(Function(x) x.Sense.Exists(Function(y) y.Weighting > 0)) Then
                '------------------------------------------------------
                'At least one Word has a Sense that has been weighted
                '------------------------------------------------------
                If Me.WordListQualification.Exists(Function(x) x.HasMultipleHeighestWeightings) Then
                    lbResolved = False
                ElseIf Me.WordListQualification.Exists(Function(x) x.Sense.Count = 0) Then
                    lbResolved = False
                End If
            Else
                '--------------------------------------------
                'No Word has a Sense that has been Weighted
                '--------------------------------------------
                lbResolved = False
            End If

            Return lbResolved

        End Function

        Public Sub ClearWordListQualifications()

            Dim lrWordQualification As Language.WordQualification

            For Each lrWordQualification In Me.WordListQualification
                lrWordQualification.Sense.Clear()
            Next

        End Sub

        Sub move_word_from_position_to_position(ByVal aiPosition_a As Integer, ByVal aiPosition_b As Integer)

            Dim lo_Stack As New tStack
            Dim lsWord As String = ""
            Dim liCounter As Integer = 0
            Dim lsWordAtPostionA As String = ""

            For Each lsWord In Me.WordList
                liCounter += 1
                If liCounter = aiPosition_a Then
                    lsWordAtPostionA = lsWord
                Else
                    lo_Stack.Push(lsWord)
                End If
            Next

            Call lo_Stack.Stack.Reverse()

            Me.Sentence = ""
            Me.WordList.Clear()

            liCounter = 0
            For Each lsWord In lo_Stack.Stack
                liCounter += 1
                If liCounter = aiPosition_b Then
                    Me.WordList.Add(lsWordAtPostionA)
                Else
                    Me.WordList.Add(lsWord)
                End If
                Me.Sentence &= lsWord & " "
            Next

        End Sub

        ''' <summary>
        ''' Reduces any 'spaces' greater than a single space, to 1 space
        ''' </summary>
        ''' <param name="asSentence"></param>
        ''' <remarks></remarks>
        Private Function ReduceSpaces(ByVal asSentence As Object)

            Dim li_position_of_double_space As Integer = 0
            Dim lsNewSentence As String = ""

            If asSentence.Contains("  ") Then
                li_position_of_double_space = Me.Sentence.IndexOf("  ")
                lsNewSentence = asSentence.Substring(0, li_position_of_double_space)
                lsNewSentence &= asSentence.Substring(li_position_of_double_space + 1, (Me.Sentence.Length - lsNewSentence.Length) - 1)
                Me.Sentence = lsNewSentence
                If lsNewSentence.Contains("  ") Then
                    Me.ReduceSpaces(lsNewSentence)
                End If
            Else
                Return asSentence
            End If

            Return lsNewSentence

        End Function

        ''' <summary>
        ''' Returns True if the Sentence is a Statement
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function IsStatement() As Boolean

            Return Me.SentenceType.Contains(pcenumSentenceType.Statement)

        End Function

        ''' <summary>
        ''' Returns True if the Sentence is a Question
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function IsQuestion() As Boolean

            Return Me.SentenceType.Contains(pcenumSentenceType.Question)

        End Function

        ''' <summary>
        ''' Returns True if the Sentence is a Directive
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function IsDirective() As Boolean

            Return Me.SentenceType.Contains(pcenumSentenceType.Directive)

        End Function

        ''' <summary>
        ''' Returns True if every Word in the Sentence has a WordQalification.Sense Count of 1 (i.e. every word in the Sentence is qualified as to what Sense that word is within the Sentence)
        ''' AND The SentenceType is known.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function IsQualified() As Boolean

            Dim lrWordQualification As Language.WordQualification

            IsQualified = True

            For Each lrWordQualification In Me.WordListQualification
                If lrWordQualification.Sense.Count > 1 Then
                    IsQualified = False
                End If
            Next

            If Me.SentenceType.Contains(pcenumSentenceType.Unknown) Then
                IsQualified = False
            End If

        End Function

        Public Sub AddSentenceType(ByVal aiSentenceType As pcenumSentenceType)

            Call Me.SentenceType.Remove(pcenumSentenceType.Unknown)

            Call Me.SentenceType.Add(aiSentenceType)

        End Sub

    End Class

End Namespace