Imports System.Xml.Serialization
Imports System.Reflection

Namespace FBM
    <Serializable()> _
    Public Class Role
        Inherits Viev.FBM.Role

        ''' <summary>
        ''' Clones an instance of the Role
        ''' </summary>
        ''' <param name="arPage"></param>
        ''' <param name="abAddToPage"></param>
        ''' <param name="abForceReferencingErrorThrowing">True if want to force error throwing when ModelObject referenced (joined) by the Role(Instance) does not exist, else False. _
        ''' The reason that you would want to suppress throwing of an error is because FactTypes with Roles referencing FactTypes can be recursive, and it is far easier to load _
        ''' all FactTypeInstances on a Page (using CloneInstance for the FactType/Roles, and then go back and populate the JoinedORMObject for those where Role.JoinedORMObject is Nothing. _
        ''' This is far easier to implement than a recursive loading of FactTypeInstances.</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shadows Function CloneInstance(ByRef arPage As FBM.Page, Optional ByVal abAddToPage As Boolean = False, Optional ByVal abForceReferencingErrorThrowing As Boolean = Nothing) As FBM.RoleInstance

            Dim lrEntityTypeInstance As FBM.EntityTypeInstance
            Dim lrRoleInstance As New FBM.RoleInstance
            Dim lsMessage As String = ""

            Try

                With Me
                    lrRoleInstance.Model = arPage.Model
                    lrRoleInstance.Page = arPage
                    lrRoleInstance.Role = Me

                    Dim lrFactTypeInstance As New FBM.FactTypeInstance(arPage.Model, arPage, pcenumLanguage.ORMModel, .FactType.Name, True)
                    lrFactTypeInstance.Id = .FactType.Id

                    lrRoleInstance.Id = .Id
                    lrRoleInstance.Name = .Name
                    lrRoleInstance.ConceptType = .ConceptType
                    lrRoleInstance.FactType = arPage.FactTypeInstance.Find(AddressOf lrFactTypeInstance.Equals)
                    If lrFactTypeInstance.FactType Is Nothing Then
                        Throw New ApplicationException("Error: No FactTypeInstance exists for Role with Role.Id: " & lrRoleInstance.Id & ", and for Role.FactType.Id: " & .FactType.Id)
                    End If
                    lrRoleInstance.Deontic = .Deontic
                    lrRoleInstance.TypeOfJoin = .TypeOfJoin
                    lrRoleInstance.Mandatory = .Mandatory
                    lrRoleInstance.SequenceNr = .SequenceNr

                    lrRoleInstance.RoleName = New FBM.RoleName(lrRoleInstance, lrRoleInstance.Name)

                    Select Case .TypeOfJoin
                        Case Is = pcenumRoleJoinType.EntityType
                            lrEntityTypeInstance = New FBM.EntityTypeInstance
                            lrEntityTypeInstance = arPage.EntityTypeInstance.Find(AddressOf .JoinedORMObject.EqualsByName)
                            lrRoleInstance.JoinsEntityType = New FBM.EntityTypeInstance
                            lrRoleInstance.JoinsEntityType = lrEntityTypeInstance
                            If Not lrRoleInstance.JoinsEntityType Is Nothing Then
                                lrRoleInstance.JoinedORMObject = New FBM.EntityTypeInstance
                                lrRoleInstance.JoinedORMObject = lrEntityTypeInstance 'lrRoleInstance.JoinsEntityType                                
                            Else
                                lrRoleInstance.JoinedORMObject = Nothing
                                lrRoleInstance.JoinsEntityType = .JoinedORMObject.CloneEntityTypeInstance(arPage)
                                lsMessage = "Error: No EntityTypeInstance found for:"
                                lsMessage &= vbCrLf & " Role with Role.Id: " & lrRoleInstance.Id & ", and"
                                lsMessage &= vbCrLf & " for Role.FactType.Id: " & .FactType.Id
                                lsMessage &= vbCrLf & " for Role.JoinsEntityTypeId: " & Me.JoinedORMObject.Id
                                lsMessage &= vbCrLf & " for Page.Name: " & arPage.Name
                                If IsSomething(abForceReferencingErrorThrowing) Then
                                    If abForceReferencingErrorThrowing Then
                                        Throw New ApplicationException(lsMessage)
                                    End If
                                End If
                            End If
                        Case Is = pcenumRoleJoinType.ValueType
                            lrRoleInstance.JoinsValueType = arPage.ValueTypeInstance.Find(AddressOf .JoinedORMObject.EqualsByName)
                            If Not lrRoleInstance.JoinsValueType Is Nothing Then
                                lrRoleInstance.JoinedORMObject = New FBM.ValueTypeInstance
                                lrRoleInstance.JoinedORMObject = lrRoleInstance.JoinsValueType
                                'lrRoleInstance.JoinsValueType = .JoinedORMObject.CloneInstance(arPage)
                            Else
                                lrRoleInstance.JoinedORMObject = Nothing
                                lrRoleInstance.JoinsValueType = .JoinedORMObject.CloneInstance(arPage)
                                lsMessage = "Error: No ValueTypeInstance found for "
                                lsMessage &= vbCrLf & " Role with Role.Id: " & lrRoleInstance.Id & ", and"
                                lsMessage &= vbCrLf & " for Role.FactType.Id: " & .FactType.Id
                                lsMessage &= vbCrLf & " for Role.JoinsValueTypeId: " & Me.JoinedORMObject.Id
                                lsMessage &= vbCrLf & " for Page.Name: " & arPage.Name

                                If IsSomething(abForceReferencingErrorThrowing) Then
                                    If abForceReferencingErrorThrowing Then
                                        Throw New ApplicationException(lsMessage)
                                    End If
                                End If
                            End If
                        Case Is = pcenumRoleJoinType.FactType
                            lrRoleInstance.JoinsFactType = arPage.FactTypeInstance.Find(AddressOf .JoinedORMObject.EqualsByName)
                            If lrRoleInstance.JoinsFactType IsNot Nothing Then
                                lrRoleInstance.JoinedORMObject = lrRoleInstance.JoinsFactType
                            Else
                                lrRoleInstance.JoinedORMObject = Nothing
                                lrRoleInstance.JoinsFactType = .JoinedORMObject.CloneInstance(arPage, abAddToPage)

                                lsMessage = "Error: No FactTypeInstance found for "
                                lsMessage &= vbCrLf & " Role with Role.Id: " & lrRoleInstance.Id & ", and"
                                lsMessage &= vbCrLf & " for Role.FactType.Id: " & .FactType.Id
                                lsMessage &= vbCrLf & " for Role.JoinsFactTypeId: " & Me.JoinedORMObject.Id
                                lsMessage &= vbCrLf & " for Page.Name: " & arPage.Name
                                If IsSomething(abForceReferencingErrorThrowing) Then
                                    If abForceReferencingErrorThrowing Then
                                        Throw New ApplicationException(lsMessage)
                                    End If
                                End If
                            End If
                    End Select

                    If abAddToPage Then
                        arPage.RoleInstance.Add(lrRoleInstance)
                    End If

                End With

                Return lrRoleInstance

            Catch ex As Exception
                lsMessage = "Error: tRole.CloneInstance: " & vbCrLf & vbCrLf
                lsMessage &= "Role.Id: " & lrRoleInstance.Id
                lsMessage &= vbCrLf & "Role.FactType.Id: " & Me.FactType.Id
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace) ' & vbCrLf & ex.InnerException.ToString)

                Return Nothing
            End Try

        End Function

        Public Overrides Sub Delete()
            MyBase.Delete()

            TableRole.DeleteRole(Me)

        End Sub

        ''' <summary>
        ''' Removes the Role from the Model if it is okay to do so.
        '''   i.e. Will not remove the Role if there is a RoleConstraint attached to the Role.
        ''' </summary>
        ''' <remarks></remarks>
        Public Overrides Function RemoveFromModel(Optional ByVal abForceRemoval As Boolean = False, _
                                                  Optional ByVal abCheckForErrors As Boolean = True) As Boolean

            Try
                Me.Model.Role.Remove(Me)
                Call TableRole.DeleteRole(Me)

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                Me.Model.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace, True)
            End Try

        End Function

        Public Overrides Sub Save()

            Try
                If TableRole.ExistsRole(Me) Then
                    Call TableRole.UdateRole(Me)
                Else
                    Call TableRole.AddRole(Me)
                End If
            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try
        End Sub


    End Class

End Namespace
