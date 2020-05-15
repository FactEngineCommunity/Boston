Namespace FBM
    <Serializable()> _
    Public Class JoinPathRole

        ''' <summary>
        ''' The RoleConstraintArgument to which the JoinPathRole belongs.
        ''' </summary>
        ''' <remarks></remarks>
        Public Argument As FBM.RoleConstraintArgument

        ''' <summary>
        ''' The Role of the JoinPath (i.e. for this JoinPathRole).
        ''' </summary>
        ''' <remarks></remarks>
        Public Role As FBM.Role

        ''' <summary>
        ''' The SequenceNr of the Role within the set of Roles that form the JoinPath for the RoleConstraintArgument.
        ''' </summary>
        ''' <remarks></remarks>
        Public SequenceNr As Integer = 1

        ''' <summary>
        ''' Parameterless New
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub New()
        End Sub

        Public Sub New(ByRef arRoleConstraintArgument As FBM.RoleConstraintArgument, ByRef arRole As FBM.Role, ByVal aiSequenceNr As Integer)

            Me.Argument = arRoleConstraintArgument
            Me.Role = arRole
            Me.SequenceNr = aiSequenceNr

        End Sub

    End Class

End Namespace
