Imports System.Reflection

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

        Public Sub Save()

            Try

                If TableJoinPathRole.ExistsJoinPathRole(Me) Then
                    '-----------------------------------------------------------------
                    'Nothing to do. Nothing to Update on the existing record either.
                    '-----------------------------------------------------------------
                Else
                    TableJoinPathRole.AddJoinPathRole(Me)
                End If

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

    End Class

End Namespace
