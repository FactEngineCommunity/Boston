Imports System.Xml.Serialization

Namespace FBM
    <Serializable()> _
    Public Class RoleConstraintRole
        Inherits Viev.FBM.RoleConstraintRole

        Public Shadows Function CloneInstance(ByRef arPage As FBM.Page, Optional ByVal arRoleConstraintInstance As FBM.RoleConstraintInstance = Nothing) As FBM.RoleConstraintRoleInstance

            Dim lrRoleConstraintRoleInstance As New FBM.RoleConstraintRoleInstance

            Try
                With Me
                    lrRoleConstraintRoleInstance.Model = arPage.Model
                    lrRoleConstraintRoleInstance.Page = arPage
                    lrRoleConstraintRoleInstance.ConceptType = .ConceptType
                    lrRoleConstraintRoleInstance.Name = .Name
                    lrRoleConstraintRoleInstance.ShortDescription = .ShortDescription
                    lrRoleConstraintRoleInstance.LongDescription = .LongDescription
                    lrRoleConstraintRoleInstance.IsEntry = .IsEntry
                    lrRoleConstraintRoleInstance.IsExit = .IsExit
                    lrRoleConstraintRoleInstance.RoleConstraintRole = Me
                    lrRoleConstraintRoleInstance.SequenceNr = .SequenceNr
                    lrRoleConstraintRoleInstance.Symbol = .Symbol

                    Dim lrRoleInstance As New FBM.RoleInstance(.Model, arPage)
                    lrRoleInstance.Id = Me.Role.Id
                    lrRoleConstraintRoleInstance.Role = arPage.RoleInstance.Find(AddressOf lrRoleInstance.Equals)

                    lrRoleConstraintRoleInstance.Role.RoleConstraintRole.Add(lrRoleConstraintRoleInstance)

                    Dim lrRoleConstraintInstance As New FBM.RoleConstraintInstance()
                    lrRoleConstraintInstance.Id = .RoleConstraint.Id
                    If arRoleConstraintInstance Is Nothing Then
                        lrRoleConstraintRoleInstance.RoleConstraint = arPage.RoleConstraintInstance.Find(AddressOf lrRoleConstraintInstance.Equals)
                    Else
                        lrRoleConstraintRoleInstance.RoleConstraint = arRoleConstraintInstance
                    End If

                    If Nothing Is lrRoleConstraintRoleInstance.RoleConstraint Then
                        Throw New Exception("Could not find RoleConstraintInstance for RoleConstraintRoleInstance")
                    End If

                End With

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

            Return lrRoleConstraintRoleInstance

        End Function


        ''' <summary>
        ''' Creates an instance of the RoleConstraintRole in the database. Does nothing if the object of this class does not stem from an inherited class where this method is overridden.
        ''' </summary>
        ''' <remarks></remarks>
        Public Overrides Sub Create() Implements VievLibrary.Relational.iObjectRelationalMap(Of Viev.FBM.RoleConstraintRole).Create
            Call TableRoleConstraintRole.AddRoleConstraintRole(Me)
        End Sub

        ''' <summary>
        ''' Deletes the RoleConstraintRole from the database. Does nothing if the object of this class does not stem from an inherited class where this method is overridden.
        ''' </summary>
        ''' <remarks></remarks>
        Public Overrides Sub Delete() Implements iObjectRelationalMap(Of RoleConstraintRole).Delete
            Call TableRoleConstraintRole.DeleteRoleConstraintRole(Me)
        End Sub


        Public Overrides Shadows Sub Save() Implements VievLibrary.Relational.iObjectRelationalMap(Of RoleConstraintRole).Save

            If TableRoleConstraintRole.ExistsRoleConstraintRole(Me) Then
                TableRoleConstraintRole.UdateRoleConstraintRole(Me)
            Else
                TableRoleConstraintRole.AddRoleConstraintRole(Me)
            End If

        End Sub

    End Class

End Namespace
