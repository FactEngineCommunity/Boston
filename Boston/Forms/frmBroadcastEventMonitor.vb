Public Class frmBroadcastEventMonitor

    Public WithEvents Tube As Tube

    Private Sub frmBroadcastEventMonitor_Load(sender As Object, e As EventArgs) Handles Me.Load

        Me.Tube = prDuplexServiceClient.Tube

    End Sub

    Private Sub Tube_ObjectAdded(ByRef arObject As Object) Handles Tube.ObjectAdded

        'Expects 'Message' to be the object (as string)

        Dim lsMessage As String = CType(arObject, String)

        Me.ListBoxBroadcastEvents.Items.Insert(0, lsMessage)

        If Me.ListBoxBroadcastEvents.Items.Count > Me.Tube.Length Then
            Me.ListBoxBroadcastEvents.Items.RemoveAt(Me.Tube.Length)
        End If

    End Sub

End Class