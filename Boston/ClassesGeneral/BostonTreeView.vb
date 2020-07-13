Imports System.Runtime.InteropServices
Public Class BostonTreeView
    Inherits TreeView

    Protected Overrides Sub OnHandleCreated(ByVal e As EventArgs)
        SendMessage(Me.Handle, TVM_SETEXTENDEDSTYLE, CType(TVS_EX_DOUBLEBUFFER, IntPtr), CType(TVS_EX_DOUBLEBUFFER, IntPtr))
        MyBase.OnHandleCreated(e)
    End Sub

    Private Const TVM_SETEXTENDEDSTYLE As Integer = &H1100 + 44
    Private Const TVM_GETEXTENDEDSTYLE As Integer = &H1100 + 45
    Private Const TVS_EX_DOUBLEBUFFER As Integer = &H4
    <DllImport("user32.dll")>
    Private Shared Function SendMessage(ByVal hWnd As IntPtr, ByVal msg As Integer, ByVal wp As IntPtr, ByVal lp As IntPtr) As IntPtr
    End Function

End Class
