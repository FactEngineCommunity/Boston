
<AttributeUsage(AttributeTargets.Property Or AttributeTargets.Field, AllowMultiple:=False)>
Public Class ForeignKeyReferenceAttribute
    Inherits Attribute
    Public ReadOnly Property TargetType As Type
    Public ReadOnly Property TargetMember As String
    Public ReadOnly Property CascadeDelete As Boolean
    Public Sub New(targetType As Type, targetMember As String, Optional cascadeDelete As Boolean = False)
        Me.TargetType = targetType
        Me.TargetMember = targetMember
        Me.CascadeDelete = cascadeDelete
    End Sub
End Class