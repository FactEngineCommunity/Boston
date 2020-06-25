Imports Boston.Parser.Syntax.Constants

Namespace Persistence

    Public Class MDPackageTag : Implements IMDPersistenceItem

        Private mGUID As String = Nothing

        Public Sub New()

        End Sub

        Public Sub CreateNewGUID()
            Me.mGUID = System.Guid.NewGuid.ToString()
        End Sub

        Public Property GUID() As String
            Get
                Return Me.mGUID
            End Get
            Set(ByVal value As String)
                Me.mGUID = value
            End Set
        End Property

        Public Function GetCopy() As IMDPersistenceItem Implements IMDPersistenceItem.GetCopy
            Dim Copy As New MDPackageTag()
            Copy.GUID = Me.GUID
            Return Copy
        End Function

    End Class

End Namespace