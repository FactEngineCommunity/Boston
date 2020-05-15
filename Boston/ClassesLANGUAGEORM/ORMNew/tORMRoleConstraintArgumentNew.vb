Imports System.Xml.Serialization

Namespace FBM
    <Serializable()> _
    Public Class RoleConstraintArgument
        Inherits Viev.FBM.RoleConstraintArgument

        Public Overloads Overrides Sub Save()

            If TableRoleConstraintArgument.ExistsRoleConstraintArgument(Me) Then
                Call TableRoleConstraintArgument.UpdateRoleConstraintArgument(Me)
            Else
                Call TableRoleConstraintArgument.AddRoleConstraintArgument(Me)
            End If

            If Me.JoinPath.RolePath.Count > 0 Then
                '------------------------------------------
                'Must save the JoinPath for the Argument.
                '------------------------------------------
                Dim lrRole As FBM.Role
                Dim lrJoinPathRole As FBM.JoinPathRole
                Dim liSequenceNr As Integer = 1

                For Each lrRole In Me.JoinPath.RolePath
                    lrJoinPathRole = New FBM.JoinPathRole(Me, lrRole, liSequenceNr)
                    If TableJoinPathRole.ExistsJoinPathRole(lrJoinPathRole) Then
                        '-----------------------------------------------------------------
                        'Nothing to do. Nothing to Update on the existing record either.
                        '-----------------------------------------------------------------
                    Else
                        TableJoinPathRole.AddJoinPathRole(lrJoinPathRole)
                    End If
                    liSequenceNr += 1
                Next

            End If

        End Sub

    End Class

End Namespace
