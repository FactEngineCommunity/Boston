Imports System.Xml.Serialization
Imports System.Reflection

Namespace FBM
    <Serializable()> _
    Public Class RoleConstraintRole
        Inherits FBM.ModelObject
        Implements VievLibrary.Relational.iObjectRelationalMap(Of FBM.RoleConstraintRole)
        Implements IEquatable(Of FBM.RoleConstraintRole)
        Implements ICloneable

        Public Role As FBM.Role

        <XmlIgnore()> _
        Public RoleConstraint As FBM.RoleConstraint

        <XmlAttribute()> _
        Public IsEntry As Boolean = False

        <XmlAttribute()> _
        Public IsExit As Boolean = False

        <XmlAttribute()> _
        Public SequenceNr As Integer = 1

        ''' <summary>
        ''' Populated if the RoleConstraintRole belongs to a RoleConsrtaint with a set of Arguments
        ''' </summary>
        ''' <remarks></remarks>
        Public RoleConstraintArgument As FBM.RoleConstraintArgument

        ''' <summary>
        ''' Poplated if the RoleConstraintRole belongs to a RoleConstraint with a set of Arguments. Is the Sequential poition of the RoleConstaintRole within the Argument to which it belongs. See RoleConstraintRole.RoleConstraintArgument.
        ''' </summary>
        ''' <remarks></remarks>
        <XmlAttribute()> _
        Public ArgumentSequenceNr As Integer = 1


        Public Sub New()
            '---------
            'Default
            '---------
            Me.ConceptType = pcenumConceptType.RoleConstraintRole
        End Sub

        Public Sub New(ByRef arRole As FBM.Role, ByRef arRoleConstraint As FBM.RoleConstraint, Optional ByVal ab_IsEntry As Boolean = Nothing, Optional ByVal ab_IsExit As Boolean = Nothing, Optional ByVal aiSequenceNr As Integer = Nothing)

            Call Me.new()

            Try
                Me.Model = arRole.Model
                Me.Role = arRole
                Me.RoleConstraint = arRoleConstraint

                If IsSomething(ab_IsEntry) Then
                    Me.IsEntry = ab_IsEntry
                End If

                If IsSomething(ab_IsExit) Then
                    Me.IsExit = ab_IsExit
                End If

                If IsSomething(aiSequenceNr) Then
                    Me.SequenceNr = aiSequenceNr
                End If

            Catch lrErr As Exception
                MsgBox("Error: RoleConstraintRole.New: " & lrErr.Message)
            End Try

        End Sub

        Public Shadows Function Equals(ByVal other As FBM.RoleConstraintRole) As Boolean Implements System.IEquatable(Of FBM.RoleConstraintRole).Equals

            If (Me.Role.Id = other.Role.Id) And _
               (Me.RoleConstraint.Id = other.RoleConstraint.Id) Then
                Return True
            Else
                Return False
            End If

        End Function

        Public Function EqualsByRole(ByVal other As FBM.RoleConstraintRole) As Boolean

            If Me.Role.Id = other.Role.Id Then
                Return True
            Else
                Return False
            End If

        End Function

        Public Overloads Function Clone(ByRef arModel As FBM.Model, ByRef arRoleConstraint As FBM.RoleConstraint, Optional abAddToModel As Boolean = False) As Object

            Dim lrRoleConstraintRole As New FBM.RoleConstraintRole

            Try
                With Me
                    lrRoleConstraintRole.Model = arModel
                    lrRoleConstraintRole.Symbol = .Symbol

                    lrRoleConstraintRole.RoleConstraint = arRoleConstraint

                    If arModel.Role.Exists(AddressOf .Role.Equals) Then
                        lrRoleConstraintRole.Role = arModel.Role.Find(AddressOf .Role.Equals)
                    Else
                        lrRoleConstraintRole.Role = .Role.Clone(arModel, abAddToModel)
                    End If


                    lrRoleConstraintRole.Role.RoleConstraintRole.Add(lrRoleConstraintRole)

                    lrRoleConstraintRole.IsEntry = .IsEntry
                    lrRoleConstraintRole.IsExit = .IsExit

                    lrRoleConstraintRole.SequenceNr = .SequenceNr

                    lrRoleConstraintRole.ShortDescription = .ShortDescription
                    lrRoleConstraintRole.LongDescription = .LongDescription
                End With

                Return lrRoleConstraintRole
            Catch ex As Exception
                Dim lsMessage As String = ""

                lsMessage = "Error: FBM.RoleConstraintRole.Clone: " & vbCrLf & vbCrLf & ex.Message
                Call Me.Model.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace, True)

                Return lrRoleConstraintRole
            End Try

        End Function


        Public Overrides Function CanSafelyRemoveFromModel() As Boolean
            Return False
        End Function

        ''' <summary>
        ''' Saves the RoleConstraintRole to the database. Does nothing if the object of this class does not stem from an inherited class where this method if overridden.
        ''' </summary>
        ''' <remarks></remarks>
        Public Overridable Shadows Sub Save() Implements VievLibrary.Relational.iObjectRelationalMap(Of RoleConstraintRole).Save

        End Sub

        ''' <summary>
        ''' Creates an instance of the RoleConstraintRole in the database. Does nothing if the object of this class does not stem from an inherited class where this method is overridden.
        ''' </summary>
        ''' <remarks></remarks>
        Public Overridable Sub Create() Implements VievLibrary.Relational.iObjectRelationalMap(Of Viev.FBM.RoleConstraintRole).Create

        End Sub

        ''' <summary>
        ''' Deletes the RoleConstraintRole from the database. Does nothing if the object of this class does not stem from an inherited class where this method is overridden.
        ''' </summary>
        ''' <remarks></remarks>
        Public Overridable Sub Delete() Implements iObjectRelationalMap(Of RoleConstraintRole).Delete

        End Sub

        Public Function Load() As RoleConstraintRole Implements iObjectRelationalMap(Of RoleConstraintRole).Load
            Throw New NotImplementedException("This functn is not yet implemented")
            Return Me
        End Function

    End Class
End Namespace