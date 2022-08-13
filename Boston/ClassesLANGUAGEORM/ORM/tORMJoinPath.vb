Imports System.Reflection

Namespace FBM

    ''' <summary>
    ''' A JoinPath is ultimately a sequence of Roles that are transversed from one Role to another Role, or a set of greater than 
    '''   two Roles.
    '''   * Each RoleConstraintArgument must have a valid JoinPath between the Roles of the RoleConstraintRoles associated
    '''     with the RoleConstraintArgument.
    '''   * Even though a JoinPath may traverse multiple FactTypes, and ultimately the ObjectTypes joined by the Roles of the
    '''     those FactTypes, it is the set of Roles that are traversed, to get from one Role to another Role, that form the
    '''     JoinPath.RolePath.
    '''   * The FactTypePath attribute of this class represents the set of unique FactTypes (1 or more) that are traversed within
    '''     the JoinPath.
    '''   * The first and last Roles within the JoinPath are always Roles of the RoleConstraintRoles of the RoleConstraintArgument
    '''     of the JoinPath. Intermediate Roles are either Roles merely traversed within the JoinPath OR a Role of a
    '''     RoleConstraintRole of the RoleConstraintArgument where the RoleConstraintArgument has more than two RoleConstraintRoles.
    ''' </summary>
    ''' <remarks></remarks>
    <Serializable()> _
    Public Class JoinPath

        ''' <summary>
        ''' The RoleConstraintArgument for which the JoinPath is created.
        ''' </summary>
        ''' <remarks></remarks>
        Public Argument As FBM.RoleConstraintArgument

        ''' <summary>
        ''' The set of Roles traversed in order to form the JoinPath.
        ''' </summary>
        ''' <remarks></remarks>
        Public RolePath As New List(Of FBM.Role)

        ''' <summary>
        ''' The set of unique FactTypes (1 or more) that are traversed within the JoinPath.
        ''' </summary>
        ''' <remarks></remarks>
        Public FactTypePath As New List(Of FBM.FactType)

        Public JoinPathError As pcenumJoinPathError = pcenumJoinPathError.None

        ''' <summary>
        ''' TRUE if the JoinPath has as many Roles as the Argument of the JoinPath,
        ''' ELSE FALSE
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property IsComplete As Boolean
            Get
                Return Me.Argument.RoleConstraint.RoleConstraintRole.Count = Me.RolePath.Count
            End Get
        End Property


        ''' <summary>
        ''' Parameterless New.
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub New()

        End Sub

        ''' <summary>
        ''' Constructor.
        ''' </summary>
        ''' <param name="arRoleConstraintArgument">The RoleConstraintArgument for which the JoinPath is constructed.</param>
        ''' <remarks></remarks>
        Public Sub New(ByVal arRoleConstraintArgument As FBM.RoleConstraintArgument)

            Me.Argument = arRoleConstraintArgument

        End Sub

        Public Shadows Function Clone(ByRef arModel As FBM.Model, ByRef arArgument As FBM.RoleConstraintArgument) As FBM.JoinPath

            Dim lrJoinPath As New FBM.JoinPath

            Try
                With Me

                    lrJoinPath.Argument = arArgument

                    lrJoinPath.RolePath = New List(Of FBM.Role)
                    For Each lrRole In .RolePath
                        lrJoinPath.RolePath.Add(lrRole.Clone(arModel, False))
                    Next
                End With

                Return lrJoinPath

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)

                Return lrJoinPath
            End Try

        End Function

        ''' <summary>
        ''' Appends a JoinPath to the JoinPath.
        ''' </summary>
        ''' <param name="arJoinPath"></param>
        ''' <remarks>Used when stepping between the Roles of a RoleConstraintArgument.</remarks>
        Public Sub AppendJoinPath(ByVal arJoinPath As FBM.JoinPath)

            Try
                Me.RolePath.AddRange(arJoinPath.RolePath)
                Me.ConstructFactTypePath()

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        ''' <summary>
        ''' Constructs the FactTypePath of the JoinPath, given the Roles of the RolePath of the JoinPath.
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub ConstructFactTypePath()

            Dim lrRole As FBM.Role

            Me.FactTypePath = New List(Of FBM.FactType)

            For Each lrRole In Me.RolePath
                If Not Me.FactTypePath.Exists(Function(x) x.Id = lrRole.FactType.Id) Then
                    Me.FactTypePath.Add(lrRole.FactType)
                End If
            Next

        End Sub

        ''' <summary>
        ''' 20220813-VM-Created for the Brain creating DerivedUnaryFactType intelligent questions.
        ''' </summary>
        ''' <returns></returns>
        Public Function GetReading() As String

            Try
                '--------------------------------------------------------------------------------------------------
                'Return a Reading that is a combination of the FactTypeReadings of the FactTypes of the JoinPath.
                '--------------------------------------------------------------------------------------------------
                Dim lrFactType As FBM.FactType
                Dim larRole As New List(Of FBM.Role)
                Dim liInd As Integer = 0

                Dim lrFactTypeReading As FBM.FactTypeReading
                Dim lrLastVerbalisedRole As FBM.Role = Nothing
                Dim lsVerbalisation As String = ""

                For Each lrFactType In Me.FactTypePath
                    larRole = Me.RolePath.FindAll(Function(x) x.FactType.Id = lrFactType.Id)

                    Dim larThatOrSomeRole As New List(Of FBM.Role) From {Me.RolePath(0), Me.RolePath(Me.RolePath.Count - 1)}
                    Dim larArgumentCommonModelObjects As New List(Of FBM.ModelObject) From {Me.RolePath(0).JoinedORMObject, Me.RolePath(Me.RolePath.Count - 1).JoinedORMObject}
                    Dim larVerbalisedModelObject As New List(Of FBM.ModelObject)

                    Select Case lrFactType.FactTypeReading.Count
                        Case Is = 0
                            lsVerbalisation = "No FactTypeReading exists for Fact Type, '" & Me.FactTypePath(0).Name & "'"
                            lrLastVerbalisedRole = Nothing
                        Case Is = 1
                            lrFactTypeReading = lrFactType.FactTypeReading(0)

                            Call lrFactTypeReading.GetReadingTextThatOrSome(larThatOrSomeRole,
                                                                            lsVerbalisation,
                                                                            larArgumentCommonModelObjects,
                                                                            larVerbalisedModelObject,
                                                                            liInd > 0,
                                                                            lrLastVerbalisedRole)

                            lrLastVerbalisedRole = lrFactTypeReading.PredicatePart(lrFactTypeReading.PredicatePart.Count - 1).Role
                        Case Else

                            lrFactTypeReading = lrFactType.FindSuitableFactTypeReadingByRoles(Me.RolePath)
                            If lrFactTypeReading Is Nothing Then
                                lsVerbalisation = "No suitable FactTypeReading exists for Fact Type, '" & lrFactType.Name & "'"
                                lrLastVerbalisedRole = Nothing
                            Else
                                Call lrFactTypeReading.GetReadingTextThatOrSome(larThatOrSomeRole,
                                                                                lsVerbalisation,
                                                                                larArgumentCommonModelObjects,
                                                                                larVerbalisedModelObject,
                                                                                liInd > 0,
                                                                                lrLastVerbalisedRole)
                                lrLastVerbalisedRole = lrFactTypeReading.PredicatePart(lrFactTypeReading.PredicatePart.Count - 1).Role
                            End If
                    End Select

                    lsVerbalisation &= " "
                    liInd += 1
                Next

                Return lsVerbalisation

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)

                Return ""
            End Try

        End Function

    End Class

End Namespace
