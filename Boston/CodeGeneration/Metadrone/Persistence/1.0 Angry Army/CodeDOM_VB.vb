Namespace Persistence.AngryArmy_1_0

    Public Class CodeDOM_VB : Implements IEditorItem, IMDPersistenceItem
        Private Dirty As Boolean = False
        Private mEditorGUID As String = System.Guid.NewGuid.ToString()
        Private mOwnerGUID As String = ""
        Private mHiLightOn As Boolean = False

        Private strText As String = Nothing

        Public Name As String = Nothing

        Public Property EditorGUID() As String Implements IEditorItem.EditorGUID
            Get
                Return Me.mEditorGUID
            End Get
            Set(ByVal value As String)
                Me.mEditorGUID = value
            End Set
        End Property

        Public Property OwnerGUID() As String Implements IEditorItem.OwnerGUID
            Get
                Return Me.mOwnerGUID
            End Get
            Set(ByVal value As String)
                Me.mOwnerGUID = value
            End Set
        End Property

        Public Property Text() As String
            Get
                Return Me.strText
            End Get
            Set(ByVal value As String)
                Me.strText = value
            End Set
        End Property

        Public Function GetDirty() As Boolean
            Return Me.Dirty
        End Function

        Public Sub SetDirty(ByVal value As Boolean)
            Me.Dirty = value
        End Sub

        Public Function GetCopy() As IMDPersistenceItem Implements IMDPersistenceItem.GetCopy
            Dim Copy As New CodeDOM_VB()
            Copy.Name = Me.Name
            Copy.Text = Me.Text
            Copy.EditorGUID = Me.EditorGUID
            Copy.OwnerGUID = Me.OwnerGUID
            Return Copy
        End Function

    End Class

End Namespace