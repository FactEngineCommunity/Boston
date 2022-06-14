
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

    Public mRichTextBox As RichTextBox
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

        Dim findIndex As Integer = 0
        If findWhat.Equals(foundWord) Then findIndex = foundIndex

        If findOption = RichTextBoxFinds.Reverse Then
            findIndex = Me.mRichTextBox.Find(findWhat, 0, findIndex, findOption)
        Else
            findIndex = Me.mRichTextBox.Find(findWhat, findIndex, findOption)
        End If
        If findIndex > 0 Then
            foundWord = findWhat
            If findOption = RichTextBoxFinds.Reverse Then
                foundIndex = findIndex
            Else
                foundIndex = findIndex + findWhat.Length
            End If
        End If
    End Sub

End Class
