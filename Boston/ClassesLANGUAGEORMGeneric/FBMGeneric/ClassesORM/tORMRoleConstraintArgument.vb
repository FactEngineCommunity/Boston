Imports System.Web.UI
Imports System.Xml.Serialization
Imports System.Reflection

Namespace FBM
    <Serializable()> _
    Public Class RoleConstraintArgument
        Inherits FBM.ModelObject

        <XmlIgnore()> _
        Public RoleConstraint As FBM.RoleConstraint = Nothing

        <XmlAttribute()> _
        Public SequenceNr As Integer = 1

        <XmlIgnore()> _
        Public RoleConstraintRole As New List(Of FBM.RoleConstraintRole)

        Public JoinPath As New FBM.JoinPath(Me)

        Public Sub New(ByRef arRoleConstraint As FBM.RoleConstraint, _
                       ByVal aiSequenceNr As Integer, _
                       Optional ByVal asArgumentId As String = Nothing)

            Call MyBase.New(System.Guid.NewGuid.ToString)

            If asArgumentId IsNot Nothing Then
                Me.Id = asArgumentId
                Me.Symbol = asArgumentId
                Me.Name = asArgumentId
            End If

            Me.Model = arRoleConstraint.Model
            Me.RoleConstraint = arRoleConstraint
            Me.SequenceNr = aiSequenceNr

        End Sub

        Public Sub AddRoleConstraintRole(ByRef arRoleConstraintRole As FBM.RoleConstraintRole, _
                                         Optional ByVal abForceAddition As Boolean = False)

            Try
                '--------------------------------------------------------
                'Construct the JoinPath for the RoleConstraintArgument.
                '--------------------------------------------------------
                Dim liJoinPathError As pcenumJoinPathError = pcenumJoinPathError.None

                '-------------------------------------------------------------------
                'Associate the RoleConstraintRole with the RoleConstraintArgument.
                '-------------------------------------------------------------------
                arRoleConstraintRole.RoleConstraintArgument = Me
                arRoleConstraintRole.ArgumentSequenceNr = Me.RoleConstraintRole.Count

                If (Me.RoleConstraintRole.Count = 0) Or abForceAddition Then

                    '-----------------------------------------------------------------------
                    'Add the RoleConstraintRole to the set for the RoleConstraintArgument.
                    '-----------------------------------------------------------------------
                    Me.RoleConstraintRole.Add(arRoleConstraintRole)

                    Call Me.ConstructJoinPathForAssociatedRoleConstraintRoles()

                ElseIf Me.ExistsJoinPathForRoleConstraintRoles(liJoinPathError) Then
                    '-----------------------------------------------------------------------
                    'Add the RoleConstraintRole to the set for the RoleConstraintArgument.
                    '-----------------------------------------------------------------------
                    Me.RoleConstraintRole.Add(arRoleConstraintRole)

                    Call Me.ConstructJoinPathForAssociatedRoleConstraintRoles()
                Else
                    '----------------------------------------------------------------------------------------------------
                    ' Tried to add a RoleConstraintRole to the Argument of a RoleConstraint, 
                    '  where there is no JoinPath between the existing Roles of the Argument, and the Role being added.
                    '----------------------------------------------------------------------------------------------------
                    Me.JoinPath.JoinPathError = liJoinPathError
                End If

            Catch ex As Exception
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()
                Dim lsMessage As String

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                Me.Model.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace, True)
            End Try

        End Sub

        ''' <summary>
        ''' RETURNS TRUE if all the Roles of the RoleConstraintRoles of the RoleConstraintArgument are of the same FactType, 
        ''' ELSE RETURNS FALSE
        ''' </summary>
        ''' <returns>RETURNS TRUE if all the Roles of the RoleConstraintRoles of the RoleConstraintArgument are of the same FactType, 
        ''' ELSE RETURNS FALSE</returns>
        ''' <remarks></remarks>
        Public Function AllRolesAreForTheSameFactType() As Boolean

            Try
                Dim larFactType As New List(Of FBM.FactType)
                Dim lrRoleConstraintRole As FBM.RoleConstraintRole

                If Me.RoleConstraintRole.Count = 0 Then
                    Throw New Exception("Function called for RoleConstraintArgument with no associated RoleConstraintRoles.")
                End If

                For Each lrRoleConstraintRole In Me.RoleConstraint.RoleConstraintRole
                    If Not larFactType.Exists(Function(x) x.Id = lrRoleConstraintRole.Role.FactType.Id) Then
                        larFactType.Add(lrRoleConstraintRole.Role.FactType)
                    End If
                Next

                Return larFactType.Count = 1

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                Me.Model.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace, True)
            End Try

        End Function


        ''' <summary>
        ''' Constructs the JoinPath for the RoleConstraintArgument based on the RoleConstraintRoles of the RoleConstraintArgument.
        ''' </summary>
        ''' <remarks>
        ''' PRECONDITIONS:
        ''' 1. A join path exists for the Roles of the RoleConstraintRoles of the RoleConstraintArgumen.
        '''     See Me.ExistsJoinPathForRoleConstraintRoles
        '''     NB It isn't necessary to check this first, but (of course) if there is no JoinPath then this process will fail.
        ''' POSTCONDITIONS:
        ''' 1. The JoinPath object for this RoleConstraintArgument is populated.</remarks>
        Public Sub ConstructJoinPathForAssociatedRoleConstraintRoles()

            Try
                If Me.RoleConstraintRole.Count = 0 Then
                    Throw New Exception("Process called for RoleConstraintArgument with no associated RoleConstraintRoles")
                End If

                '----------------------------------------------------------------------------------------------------------
                'First check to see whether the JoinPath is relatively straight forward and is for Roles of one FactType.
                '----------------------------------------------------------------------------------------------------------
                If Me.AllRolesAreForTheSameFactType Then
                    Dim lrRoleConstraintRole As FBM.RoleConstraintRole

                    Me.JoinPath.RolePath = New List(Of FBM.Role)

                    For Each lrRoleConstraintRole In Me.RoleConstraintRole
                        Me.JoinPath.RolePath.Add(lrRoleConstraintRole.Role)
                    Next
                Else
                    '----------------------------------------------------------------------------------------
                    'Traversal of multiple FactTypes/ObjectTypes is required in order to form the JoinPath.
                    '----------------------------------------------------------------------------------------
                    Dim lbSussessful As Boolean = False
                    Dim liJoinPathError As pcenumJoinPathError = pcenumJoinPathError.None
                    Dim larJoinPathCovered As New List(Of FBM.Role)
                    Me.JoinPath = Me.Model.GetJoinPathBetweenRoles(Me.RoleConstraintRole(0).Role, _
                                                                   Me.RoleConstraintRole(Me.RoleConstraintRole.Count - 1).Role, _
                                                                   lbSussessful, _
                                                                   liJoinPathError, _
                                                                   larJoinPathCovered)
                End If

                Call Me.JoinPath.ConstructFactTypePath()

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                Me.Model.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace, True)
            End Try

        End Sub

        ''' <summary>
        ''' Returns TRUE if there is a JoinPath for all of the Roles associated with the RoleConstraintArgument, else returns FALSE.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function ExistsJoinPathForRoleConstraintRoles(ByRef aiJoinPathError As pcenumJoinPathError) As Boolean

            Try
                '----------------------------------------------------------------------------------------------------------
                'First check to see whether the JoinPath is relatively straight forward and is for Roles of one FactType.
                '----------------------------------------------------------------------------------------------------------
                If Me.RoleConstraintRole.Count = 1 Then
                    '------------------------------------------------------------------
                    'Simplest. Of course a JoinPath exists between a Role and itself.
                    '------------------------------------------------------------------
                    Return True
                ElseIf Me.RoleConstraintRole.Count > 0 Then
                    If Me.AllRolesAreForTheSameFactType Then
                        '-------------------------------------------
                        'Simple and straight forward, return True.
                        '-------------------------------------------
                        Return True
                    End If
                Else
                    Dim lrFirstRoleConstraintRole As FBM.RoleConstraintRole
                    Dim lrRoleConstraintRole As FBM.RoleConstraintRole

                    lrFirstRoleConstraintRole = Me.RoleConstraintRole(0)
                    Me.JoinPath = New FBM.JoinPath
                    Dim lbSuccessful As Boolean = False
                    Dim larJoinPathCovered As New List(Of FBM.Role)
                    For Each lrRoleConstraintRole In Me.RoleConstraintRole.FindAll(Function(x) x.Role.Id <> lrFirstRoleConstraintRole.Role.Id)
                        Me.JoinPath.AppendJoinPath(Me.Model.GetJoinPathBetweenRoles(lrFirstRoleConstraintRole.Role, _
                                                                           lrRoleConstraintRole.Role, _
                                                                           lbSuccessful, _
                                                                           aiJoinPathError, _
                                                                           larJoinPathCovered))
                    Next
                    Return lbSuccessful
                End If

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                Me.Model.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace, True)
            End Try

        End Function

        ''' <summary>
        ''' Projects a Reading of the combined FactTypeReadings for the FactTypes of the JoinPath of the Argument.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function ProjectArgumentReading() As String

            Dim lsArgumentReading As String = ""
            Dim larArgumentCommonModelObjects As New List(Of FBM.ModelObject)

            Try
                If Me.JoinPath.FactTypePath.Count = 1 Then
                    '---------------------------------------------------------
                    'Simply return a FactTypeReading of the single FactType.
                    '---------------------------------------------------------
                    Select Case Me.JoinPath.FactTypePath(0).FactTypeReading.Count
                        Case Is = 0
                            lsArgumentReading = "No FactTypeReading exists for Fact Type, '" & Me.JoinPath.FactTypePath(0).Name & "'"
                        Case Is = 1
                            lsArgumentReading = Me.JoinPath.FactTypePath(0).FactTypeReading(0).GetReadingTextThatOrSome(Me.JoinPath.RolePath, larArgumentCommonModelObjects)
                        Case Else
                            Dim lrFactTypeReading As FBM.FactTypeReading
                            lrFactTypeReading = Me.JoinPath.FactTypePath(0).FindSuitableFactTypeReadingByRoles(Me.JoinPath.RolePath)
                            If lrFactTypeReading Is Nothing Then
                                lsArgumentReading = "No suitable FactTypeReading exists for Fact Type, '" & Me.JoinPath.FactTypePath(0).Name & "'"
                            Else
                                lsArgumentReading = lrFactTypeReading.GetReadingTextThatOrSome(Me.JoinPath.RolePath, larArgumentCommonModelObjects)
                            End If
                    End Select
                Else
                    '--------------------------------------------------------------------------------------------------
                    'Return a Reading that is a combination of the FactTypeReadings of the FactTypes of the JoinPath.
                    '--------------------------------------------------------------------------------------------------
                    Dim lrFactType As FBM.FactType
                    Dim larRole As New List(Of FBM.Role)
                    Dim liInd As Integer = 0

                    For Each lrFactType In Me.JoinPath.FactTypePath
                        larRole = Me.JoinPath.RolePath.FindAll(Function(x) x.FactType.Id = lrFactType.Id)
                        Dim larThatOrSomeRole As New List(Of FBM.Role)

                        Dim lrRoleConstraintRole As FBM.RoleConstraintRole
                        For Each lrRoleConstraintRole In Me.RoleConstraintRole
                            larThatOrSomeRole.Add(lrRoleConstraintRole.Role)
                        Next

                        Select Case lrFactType.FactTypeReading.Count
                            Case Is = 0
                                lsArgumentReading &= "No FactTypeReading exists for Fact Type, '" & Me.JoinPath.FactTypePath(0).Name & "'"
                            Case Is = 1
                                lsArgumentReading &= lrFactType.FactTypeReading(0).GetReadingTextThatOrSome(larThatOrSomeRole, larArgumentCommonModelObjects, liInd > 0)
                            Case Else
                                Dim lrFactTypeReading As FBM.FactTypeReading
                                lrFactTypeReading = lrFactType.FindSuitableFactTypeReadingByRoles(Me.JoinPath.RolePath)
                                If lrFactTypeReading Is Nothing Then
                                    lsArgumentReading &= "No suitable FactTypeReading exists for Fact Type, '" & lrFactType.Name & "'"
                                Else
                                    lsArgumentReading &= lrFactTypeReading.GetReadingTextThatOrSome(larThatOrSomeRole, larArgumentCommonModelObjects, liInd > 0)
                                End If
                        End Select

                        lsArgumentReading &= " "
                        liInd += 1
                    Next
                End If

                Return Trim(lsArgumentReading)

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                Me.Model.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace, True)

                Return ""
            End Try

        End Function

        ''' <summary>
        ''' Projects a Reading of the combined FactTypeReadings for the FactTypes of the JoinPath of the Argument,
        '''   using the given HTMLTextWriter
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub ProjectArgumentReading(ByRef arVerbaliser As FBM.ORMVerbailser, ByRef larArgumentCommonModelObjects As List(Of FBM.ModelObject))

            Try
                If Me.JoinPath.FactTypePath.Count = 1 Then
                    '---------------------------------------------------------
                    'Simply return a FactTypeReading of the single FactType.
                    '---------------------------------------------------------
                    Select Case Me.JoinPath.FactTypePath(0).FactTypeReading.Count
                        Case Is = 0
                            arVerbaliser.VerbaliseError("No FactTypeReading exists for Fact Type, '" & Me.JoinPath.FactTypePath(0).Name & "'")
                        Case Is = 1
                            Call Me.JoinPath.FactTypePath(0).FactTypeReading(0).GetReadingTextThatOrSome(Me.JoinPath.RolePath, arVerbaliser, larArgumentCommonModelObjects)
                        Case Else
                            Dim lrFactTypeReading As FBM.FactTypeReading
                            lrFactTypeReading = Me.JoinPath.FactTypePath(0).FindSuitableFactTypeReadingByRoles(Me.JoinPath.RolePath)
                            If lrFactTypeReading Is Nothing Then
                                arVerbaliser.VerbaliseError("No suitable FactTypeReading exists for Fact Type, '" & Me.JoinPath.FactTypePath(0).Name & "'")
                            Else
                                Call lrFactTypeReading.GetReadingTextThatOrSome(Me.JoinPath.RolePath, arVerbaliser, larArgumentCommonModelObjects)
                            End If
                    End Select
                Else
                    '--------------------------------------------------------------------------------------------------
                    'Return a Reading that is a combination of the FactTypeReadings of the FactTypes of the JoinPath.
                    '--------------------------------------------------------------------------------------------------
                    Dim lrFactType As FBM.FactType
                    Dim larRole As New List(Of FBM.Role)
                    Dim liInd As Integer = 0

                    For Each lrFactType In Me.JoinPath.FactTypePath
                        larRole = Me.JoinPath.RolePath.FindAll(Function(x) x.FactType.Id = lrFactType.Id)
                        Dim larThatOrSomeRole As New List(Of FBM.Role)

                        Dim lrRoleConstraintRole As FBM.RoleConstraintRole
                        For Each lrRoleConstraintRole In Me.RoleConstraintRole
                            larThatOrSomeRole.Add(lrRoleConstraintRole.Role)
                        Next

                        Select Case lrFactType.FactTypeReading.Count
                            Case Is = 0
                                arVerbaliser.VerbaliseError("No FactTypeReading exists for Fact Type, '" & Me.JoinPath.FactTypePath(0).Name & "'")
                            Case Is = 1
                                Call lrFactType.FactTypeReading(0).GetReadingTextThatOrSome(larThatOrSomeRole, arVerbaliser, larArgumentCommonModelObjects, liInd > 0)
                            Case Else
                                Dim lrFactTypeReading As FBM.FactTypeReading
                                lrFactTypeReading = lrFactType.FindSuitableFactTypeReadingByRoles(Me.JoinPath.RolePath)
                                If lrFactTypeReading Is Nothing Then
                                    arVerbaliser.VerbaliseError("No suitable FactTypeReading exists for Fact Type, '" & lrFactType.Name & "'")
                                Else
                                    Call lrFactTypeReading.GetReadingTextThatOrSome(larThatOrSomeRole, arVerbaliser, larArgumentCommonModelObjects, liInd > 0)
                                End If
                        End Select

                        arVerbaliser.HTW.Write(" ")
                        liInd += 1
                    Next
                End If

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                Me.Model.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace, True)
            End Try

        End Sub


        ''' <summary>
        ''' Prototype for the Save function.
        ''' </summary>
        ''' <remarks></remarks>
        Public Overridable Overloads Sub Save()
        End Sub

    End Class


End Namespace
