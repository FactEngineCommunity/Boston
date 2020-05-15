Public Class frmNotifications

    ''' <summary>
    ''' Called from the Notifications themselves as they close.
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub CloseIfEmpty()
        If Me.FlexibleListBox.Count = 0 Then
            Me.Hide()
            Me.Dispose()
            frmMain.zfrmNotifications = Nothing
            frmMain.ToolStripButtonNotifications.Visible = False
        End If
    End Sub

End Class