
Public Class WaitCursor
    Implements IDisposable


    Private m_cursorOld As Cursor
    Private _disposedValue As Boolean = False

    Public Sub New()
        m_cursorOld = Cursor.Current
        Cursor.Current = Cursors.WaitCursor
    End Sub

    Protected Overridable Sub Dispose(ByVal disposing As Boolean)
        If Not _disposedValue Then Cursor.Current = m_cursorOld
        _disposedValue = True
    End Sub

    Public Sub Dispose1() Implements IDisposable.Dispose
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub
End Class

