
#Region "Copyright © 2008 Ashu Fouzdar. All rights reserved."
'
'Copyright © 2008 Ashu Fouzdar. All rights reserved.
'
'Redistribution and use in source and binary forms, with or without
'modification, are permitted provided that the following conditions
'are met:
'
'1. Redistributions of source code must retain the above copyright
'   notice, this list of conditions and the following disclaimer.
'2. Redistributions in binary form must reproduce the above copyright
'   notice, this list of conditions and the following disclaimer in the
'   documentation and/or other materials provided with the distribution.
'3. The name of the author may not be used to endorse or promote products
'   derived from this software without specific prior written permission.
'
'THIS SOFTWARE IS PROVIDED BY THE AUTHOR "AS IS" AND ANY EXPRESS OR
'IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES
'OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED.
'IN NO EVENT SHALL THE AUTHOR BE LIABLE FOR ANY DIRECT, INDIRECT,
'INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT
'NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE,
'DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY
'THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
'(INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF
'THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
' 
#End Region

Imports System.Windows.Forms

Public Class frmTextboxFind

    Public mRichTextBox As RichTextBox = Nothing
    Public mScintillaTextBox As ScintillaNET.Scintilla = Nothing
    Private foundIndex As Integer
    Private foundWord As String

    Public Event Find(ByVal findWhat As String, ByVal findOption As RichTextBoxFinds)

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.DialogResult = DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub txtFindWhat_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtFindWhat.TextChanged
        If txtFindWhat.TextLength > 0 Then
            btnFind.Enabled = True
        Else
            btnFind.Enabled = False
        End If
    End Sub

    Private Sub btnFind_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFind.Click
        If txtFindWhat.TextLength > 0 Then
            Dim findOption As RichTextBoxFinds = RichTextBoxFinds.None
            If chkMatchCase.Checked Then findOption = RichTextBoxFinds.MatchCase
            If chkWholeWord.Checked Then findOption = (findOption Or RichTextBoxFinds.WholeWord)
            If optUp.Checked Then findOption = (findOption Or RichTextBoxFinds.Reverse)
            Call Me.FindText(txtFindWhat.Text, findOption)
        End If
    End Sub

    Private Sub FindText(ByVal findWhat As String, ByVal findOption As RichTextBoxFinds)

        ' 1. Determine our baseline starting search index
        Dim searchStartIndex As Integer = 0

        ' If we are searching for the same word again, start from our last tracked index
        If findWhat.Equals(foundWord) Then
            searchStartIndex = foundIndex
        End If

        Dim findIndex As Integer = -1
        Dim isReverse As Boolean = ((findOption And RichTextBoxFinds.Reverse) = RichTextBoxFinds.Reverse)

        ' --- RICH TEXT BOX PATH ---
        If Me.mRichTextBox IsNot Nothing Then
            If isReverse Then
                ' RichTextBox backward search requires: (text, startRange, endRange, options)
                findIndex = Me.mRichTextBox.Find(findWhat, 0, searchStartIndex, findOption)
            Else
                ' Forward search: (text, startRange, options)
                findIndex = Me.mRichTextBox.Find(findWhat, searchStartIndex, findOption)
            End If

            If findIndex <> -1 Then
                Me.mRichTextBox.Select(findIndex, findWhat.Length)
                Me.mRichTextBox.ScrollToCaret()
            End If

            ' --- SCINTILLA TEXT BOX PATH ---
        ElseIf Me.mScintillaTextBox IsNot Nothing Then
            ' Map RichTextBoxFinds over to Scintilla's native SearchFlags
            Dim scintillaFlags As ScintillaNET.SearchFlags = ScintillaNET.SearchFlags.None
            If (findOption And RichTextBoxFinds.MatchCase) = RichTextBoxFinds.MatchCase Then
                scintillaFlags = scintillaFlags Or ScintillaNET.SearchFlags.MatchCase
            End If
            If (findOption And RichTextBoxFinds.WholeWord) = RichTextBoxFinds.WholeWord Then
                scintillaFlags = scintillaFlags Or ScintillaNET.SearchFlags.WholeWord
            End If
            Me.mScintillaTextBox.SearchFlags = scintillaFlags

            ' Set boundaries based on direction
            If isReverse Then
                ' To search backward in Scintilla, make TargetStart greater than TargetEnd
                ' If we're at the beginning or haven't matched yet, default to the text length
                If searchStartIndex = 0 Then searchStartIndex = Me.mScintillaTextBox.TextLength
                Me.mScintillaTextBox.TargetStart = searchStartIndex
                Me.mScintillaTextBox.TargetEnd = 0
            Else
                ' Search forward: from current tracking point to the end of the file
                Me.mScintillaTextBox.TargetStart = searchStartIndex
                Me.mScintillaTextBox.TargetEnd = Me.mScintillaTextBox.TextLength
            End If

            ' Execute search
            findIndex = Me.mScintillaTextBox.SearchInTarget(findWhat)

            If findIndex <> -1 Then
                ' Select the matched text components set by TargetStart/TargetEnd bounds
                Me.mScintillaTextBox.SetSelection(Me.mScintillaTextBox.TargetStart, Me.mScintillaTextBox.TargetEnd)
                Me.mScintillaTextBox.ScrollCaret()
            End If
        End If

        ' --- TRACKING SEARCH STATE ---
        If findIndex <> -1 Then
            foundWord = findWhat
            If isReverse Then
                ' If going backward, the next search starts immediately before this match
                foundIndex = findIndex
            Else
                ' If going forward, the next search starts immediately after this match
                foundIndex = findIndex + findWhat.Length
            End If
        Else
            ' Reset state if nothing is found so a fresh click starts from the beginning/end again
            foundWord = String.Empty
            foundIndex = 0
            MessageBox.Show("Finished searching the document.", "Find", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If
    End Sub

End Class
