Imports System.Xml.Serialization
Imports System.Reflection

Namespace FBM
    <Serializable()> _
    Public Class RoleConstraintRole
        Inherits FBM.ModelObject
        Implements iObjectRelationalMap(Of FBM.RoleConstraintRole)
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

        Public Sub New(ByRef arRole As FBM.Role,
                       ByRef arRoleConstraint As FBM.RoleConstraint,
                       Optional ByVal ab_IsEntry As Boolean = Nothing,
                       Optional ByVal ab_IsExit As Boolean = Nothing,
                       Optional ByVal aiSequenceNr As Integer = Nothing,
                       Optional ByVal abMakeDirty As Boolean = False)

            Call Me.New()

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

                Me.isDirty = abMakeDirty

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
                    lrRoleConstraintRole.isDirty = True

                End With

                Return lrRoleConstraintRole
            Catch ex As Exception
                Dim lsMessage As String = ""

                lsMessage = "Error: FBM.RoleConstraintRole.Clone: " & vbCrLf & vbCrLf & ex.Message
                Call prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)

                Return lrRoleConstraintRole
            End Try

        End Function

        Public Shadows Function CloneInstance(ByRef arPage As FBM.Page,
                                              Optional ByRef arRoleConstraintInstance As FBM.RoleConstraintInstance = Nothing,
                                              Optional ByVal abAddToPage As Boolean = False,
                                              Optional ByRef arFactTypeInstance As FBM.FactTypeInstance = Nothing) As FBM.RoleConstraintRoleInstance

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

                    If abAddToPage Then
                        lrRoleConstraintRoleInstance.Role = arPage.RoleInstance.Find(AddressOf lrRoleInstance.Equals)
                        lrRoleConstraintRoleInstance.Role.RoleConstraintRole.Add(lrRoleConstraintRoleInstance)
                    ElseIf arFactTypeInstance IsNot Nothing Then
                        lrRoleConstraintRoleInstance.Role = arFactTypeInstance.RoleGroup.Find(Function(x) x.Id = lrRoleInstance.Id)
                        lrRoleConstraintRoleInstance.Role.RoleConstraintRole.Add(lrRoleConstraintRoleInstance)
                    Else
                        'Last Resort
                        lrRoleConstraintRoleInstance.Role = arPage.RoleInstance.Find(AddressOf lrRoleInstance.Equals)
                        lrRoleConstraintRoleInstance.Role.RoleConstraintRole.Add(lrRoleConstraintRoleInstance)
                    End If

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

        Public Overrides Function CanSafelyRemoveFromModel() As Boolean
            Return False
        End Function

        Public Shadows Sub Save(Optional ByRef abRapidSave As Boolean = False) Implements iObjectRelationalMap(Of RoleConstraintRole).Save

            If abRapidSave Then
                TableRoleConstraintRole.AddRoleConstraintRole(Me)
                Me.isDirty = False
            ElseIf Me.isDirty Then

                If TableRoleConstraintRole.ExistsRoleConstraintRole(Me) Then
                    TableRoleConstraintRole.UdateRoleConstraintRole(Me)
                Else
                    TableRoleConstraintRole.AddRoleConstraintRole(Me)
                End If
            End If
            Me.isDirty = False

        End Sub

        Public Sub Create() Implements iObjectRelationalMap(Of RoleConstraintRole).Create
            Call TableRoleConstraintRole.AddRoleConstraintRole(Me)
        End Sub

        Public Sub Delete() Implements iObjectRelationalMap(Of RoleConstraintRole).Delete
            Call TableRoleConstraintRole.DeleteRoleConstraintRole(Me)
        End Sub

        Public Function Load() As RoleConstraintRole Implements iObjectRelationalMap(Of RoleConstraintRole).Load
            Throw New NotImplementedException("This functn is not yet implemented")
            Return Me
        End Function

    End Class
End Namespace