Namespace Persistence.Beta11

    Public Class Properties : Implements IEditorItem, IMDPersistenceItem
        Private Dirty As Boolean = False
        Private mEditorGUID As String = System.Guid.NewGuid.ToString()
        Private mOwnerGUID As String = ""

        Private strBeginTag As String
        Private strEndTag As String

        Private strBeginSafe As String
        Private strEndSafe As String

        Public Sub New()

        End Sub

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

        Public Property BeginTag() As String
            Get
                Return Me.strBeginTag
            End Get
            Set(ByVal value As String)
                Me.strBeginTag = value
            End Set
        End Property

        Public Property EndTag() As String
            Get
                Return Me.strEndTag
            End Get
            Set(ByVal value As String)
                Me.strEndTag = value
            End Set
        End Property

        Public Property BeginSafe() As String
            Get
                Return Me.strBeginSafe
            End Get
            Set(ByVal value As String)
                Me.strBeginSafe = value
            End Set
        End Property

        Public Property EndSafe() As String
            Get
                Return Me.strEndSafe
            End Get
            Set(ByVal value As String)
                Me.strEndSafe = value
            End Set
        End Property

        Public Sub SetupDefaults()
            Me.BeginTag = Parser.Syntax.Constants.TAG_BEGIN_DEFAULT
            Me.EndTag = Parser.Syntax.Constants.TAG_END_DEFAULT
            Me.BeginSafe = ""
            Me.EndSafe = ""
        End Sub

        Public Function GetCopy() As IMDPersistenceItem Implements IMDPersistenceItem.GetCopy
            Dim Copy As New Properties()
            Copy.BeginTag = Me.BeginTag
            Copy.EndTag = Me.EndTag

            Copy.BeginSafe = Me.BeginSafe
            Copy.EndSafe = Me.EndSafe

            Copy.EditorGUID = Me.EditorGUID
            Copy.OwnerGUID = Me.OwnerGUID

            Return Copy
        End Function

        Public Function GetDirty() As Boolean
            Return Me.Dirty
        End Function

        Public Sub SetDirty(ByVal value As Boolean)
            Me.Dirty = value
        End Sub

    End Class

End Namespace