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

        Public Model As FBM.Model

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
                Me.Model.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace, True)
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

    End Class

End Namespace
