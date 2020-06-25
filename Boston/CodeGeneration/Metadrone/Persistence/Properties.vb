Namespace Persistence

    Public Class Properties : Implements IEditorItem, IMDPersistenceItem
        Private Dirty As Boolean = False
        Private mEditorGUID As String = System.Guid.NewGuid.ToString()
        Private mOwnerGUID As String = ""

        Private strBeginTag As String
        Private strEndTag As String
        Private strSuperMain As String

        Public Sub New()
            Me.BeginTag = Parser.Syntax.Constants.TAG_BEGIN_DEFAULT
            Me.EndTag = Parser.Syntax.Constants.TAG_END_DEFAULT
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
                Return Nothing
            End Get
            Set(ByVal value As String)

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

        Public Property SuperMain() As String
            Get
                Return Me.strSuperMain
            End Get
            Set(ByVal value As String)
                Me.strSuperMain = value
            End Set
        End Property

        Public Function GetCopy() As IMDPersistenceItem Implements IMDPersistenceItem.GetCopy
            Return Me
        End Function

        Public Function GetDirty() As Boolean
            Return Me.Dirty
        End Function

        Public Sub SetDirty(ByVal value As Boolean)
            Me.Dirty = value
        End Sub

    End Class

End Namespace