Imports System.ComponentModel
Imports System.Xml.Serialization
Imports System.Runtime.Serialization
Imports System.Reflection
Imports System.Security.Permissions
Imports System.Xml.Schema

Namespace FBM
    <Serializable()> _
    Public Class Role
        Inherits FBM.ModelObject
        Implements IEquatable(Of FBM.Role)
        'Implements IXmlSerializable
        Implements ICloneable

        <XmlIgnore()> _
        Public FactType As FBM.FactType 'The FactType to which the role belongs

        <XmlIgnore()> _
        <DebuggerBrowsable(DebuggerBrowsableState.Never)> _
        Public _role_cardinality As Integer = 0 'The maximum number of times that the Entity/ValueType/NestedFactType can be associated with the Role.

        <XmlIgnore()> _
        Public part_of_key As Boolean = False

        <XmlIgnore()> _
        <CategoryAttribute("Role"), _
             DefaultValueAttribute(GetType(Integer), "0"), _
             DescriptionAttribute("When populated, determines the number of times an instance plays the Role.")> _
        Public Property FrequencyConstraint() As Integer
            Get
                Return _role_cardinality
            End Get
            Set(ByVal Value As Integer)
                _role_cardinality = Value
            End Set
        End Property

        Public Deontic As Boolean = False

        <XmlIgnore()> _
        <DebuggerBrowsable(DebuggerBrowsableState.Never)> _
        Public _Mandatory As Boolean = False 'True, False
        <CategoryAttribute("Role"), _
             DefaultValueAttribute(False), _
             DescriptionAttribute("True if the Role is a Mandatory Role, else False.")> _
        Public Property Mandatory() As Boolean
            Get
                Return _Mandatory
            End Get
            Set(ByVal Value As Boolean)
                _Mandatory = Value
            End Set
        End Property

        <XmlIgnore()> _
        <DebuggerBrowsable(DebuggerBrowsableState.Never)> _
        Public _Value_Range As String
        <CategoryAttribute("Role"), _
             DefaultValueAttribute(""), _
             DescriptionAttribute("The range of allowable Values for the Role.")> _
        Public Property ValueRange() As String
            Get
                Return _Value_Range
            End Get
            Set(ByVal Value As String)
                _Value_Range = Value
            End Set
        End Property

        <XmlIgnore()> _
        Public SequenceNr As Integer 'The position withn the FactType/RoleGroup

        <XmlAttribute()> _
        Public TypeOfJoin As pcenumRoleJoinType

        <XmlIgnore()> _
        Public JoinsEntityType As FBM.EntityType
        <XmlIgnore()> _
        Public JoinsValueType As FBM.ValueType
        <XmlIgnore()> _
        Public JoinsFactType As FBM.FactType

        <XmlIgnore()> _
        Public WithEvents JoinedORMObject As New FBM.ModelObject 'WithEvents

        <XmlIgnore()> _
        Public RoleConstraintRole As New List(Of FBM.RoleConstraintRole) 'RoleConstraints are serialized seperately so that the serialiser doesn't go into an infinite loop.

        <XmlIgnore()> _
        Public Data As New List(Of FBM.FactData) 'As Values ('Concepts'...or just 'Data') are added to a Fact within a FactType, within which this Role exists, this enumerationis populated.
        'At first, that seems strange, as the RoleData seems to be more a part of a Fact then it ever is by itself (e.g. what is b outside of {a,b,c}).
        'There is, however, a different perspective, which is that an Entity (e.g. b) is an object in its own right, and is merely associated with a Fact, AND a Role within that Fact/FactType.
        'This perspective allows LamdaCalculus type searching for Facts, knowing only the Role and the Data (tRoleData has a member, 'Fact').

        <XmlIgnore()> _
        Public KLFreeVariableLabel As String = "" 'When generating proofs in KL, a letter from pcenumKLFreeVariable gets assigned to each Role in a FactType.

        Public NORMALinksToUnaryFactTypeValueType As Boolean = False

        Public Event MandatoryChanged(ByVal abMandatoryStatus As Boolean)
        Public Event RoleJoinModified(ByRef arModelObject As FBM.ModelObject)
        Public Event RoleNameChanged(ByVal asNewRoleName As String)

        Sub New()

            MyBase.ConceptType = pcenumConceptType.Role
            Me.Id = System.Guid.NewGuid.ToString()

        End Sub

        Sub New(ByRef arFactType As FBM.FactType, ByVal asRoleId As String, Optional ByVal abUseIdAsName As Boolean = False, Optional ByVal arJoinedModelObject As Object = Nothing)

            Call Me.New()
            Me.Model = arFactType.Model
            Me.Id = asRoleId
            Me._Symbol = asRoleId
            Me.FactType = arFactType

            If abUseIdAsName Then
                Me.Name = Me.Id
            End If

            Me.JoinedORMObject = arJoinedModelObject

            If arJoinedModelObject IsNot Nothing Then
                Select Case arJoinedModelObject.ConceptType
                    Case Is = pcenumConceptType.EntityType
                        Me.TypeOfJoin = pcenumRoleJoinType.EntityType
                        Me.JoinsEntityType = arJoinedModelObject
                    Case Is = pcenumConceptType.ValueType
                        Me.TypeOfJoin = pcenumRoleJoinType.ValueType
                        Me.JoinsValueType = arJoinedModelObject
                    Case Is = pcenumConceptType.FactType
                        Me.TypeOfJoin = pcenumRoleJoinType.FactType
                        Me.JoinsFactType = arJoinedModelObject
                End Select
            End If

        End Sub

        Sub New(ByRef arFactType As FBM.FactType, ByRef arJoinedObject As Object)

            Call Me.New()

            Me.FactType = arFactType
            Me.Model = arFactType.Model

            Me.JoinedORMObject = arJoinedObject

            If IsSomething(arJoinedObject) Then
                Select Case arJoinedObject.ConceptType
                    Case Is = pcenumConceptType.EntityType
                        Me.TypeOfJoin = pcenumRoleJoinType.EntityType
                        Me.JoinsEntityType = arJoinedObject
                    Case Is = pcenumConceptType.ValueType
                        Me.TypeOfJoin = pcenumRoleJoinType.ValueType
                        Me.JoinsValueType = arJoinedObject
                    Case Is = pcenumConceptType.FactType
                        Me.TypeOfJoin = pcenumRoleJoinType.FactType
                        Me.JoinsFactType = arJoinedObject
                End Select
            End If

            '----------------------------------------------------------
            'Add the Role to the RoleGroup of the FactType of the Role
            '----------------------------------------------------------            
            Me.SequenceNr = Me.FactType.Arity

        End Sub

        ''' <summary>
        ''' Deletes the Role from the Database.
        ''' </summary>
        ''' <remarks></remarks>
        Public Overridable Sub Delete()

        End Sub

        Public Shadows Function Equals(ByVal other As FBM.Role) As Boolean Implements System.IEquatable(Of FBM.Role).Equals

            If Me.Id = other.Id Then
                Return True
            Else
                Return False
            End If

        End Function

        Public Shadows Function EqualsByJoinedModelObjectId(ByVal other As FBM.Role) As Boolean

            If Trim(Me.JoinedORMObject.Id) = Trim(other.JoinedORMObject.Id) Then
                Return True
            Else
                Return False
            End If

        End Function

        Public Shadows Function EqualsByName(ByVal other As FBM.Role) As Boolean

            If Trim(Me.Name) = Trim(other.Name) Then
                Return True
            Else
                Return False
            End If

        End Function

        '''' <summary>
        ''''Serializes all public, private and public fields except the one 
        ''''  which are the hidden fields for the eventhandlers
        '''' </summary>
        '''' <remarks></remarks>
        '''' 
        'Private Sub WriteXML(ByVal writer As XmlWriter) Implements IXmlSerializable.WriteXml

        '    '' Get the list of all events 
        '    'Dim EvtInfos() As EventInfo = Me.GetType.GetEvents()
        '    'Dim EvtInfo As EventInfo

        '    '' Get the list of all fields
        '    'Dim FldInfos() As FieldInfo = Me.GetType.GetFields(BindingFlags.NonPublic Or BindingFlags.Instance Or BindingFlags.Public)

        '    '' Loops in each field and decides wether to serialize it or not
        '    'Dim FldInfo As FieldInfo
        '    'MsgBox("hello")

        '    writer.WriteAttributeString("ConceptType", Me.ConceptType.ToString)
        '    writer.WriteAttributeString("Id", Me.Id)
        '    writer.WriteAttributeString("Name", Me.Name)
        '    writer.WriteAttributeString("TypeOfJoin", Me.TypeOfJoin.ToString)
        '    writer.WriteAttributeString("JoinedObjectId", Me.JoinedORMObject.Id.ToString)
        '    'Select Case Me.TypeOfJoin
        '    '    Case Is = pcenumRoleJoinType.EntityType
        '    '        writer.WriteElementString("JoinedEntityTypeId", Me.JoinsEntityType.Id)
        '    '    Case Is = pcenumRoleJoinType.ValueType
        '    '        writer.WriteElementString("JoinedValueTypeId", Me.JoinsValueType.Id)
        '    '    Case Is = pcenumRoleJoinType.ValueType
        '    '        writer.WriteElementString("JoinedFactTypeId", Me.JoinsFactType.Id)
        '    'End Select
        '    'For Each FldInfo In FldInfos
        '    '    ' Finds if the field is a eventhandler
        '    '    Dim Found As Boolean = False
        '    '    For Each EvtInfo In EvtInfos
        '    '        If EvtInfo.Name + "Event" = FldInfo.Name Then
        '    '            Found = True
        '    '            Exit For
        '    '        End If
        '    '    Next

        '    '    ' If field is not an eventhandler serializes it
        '    '    If Not Found Then
        '    '        info.AddValue(FldInfo.Name, FldInfo.GetValue(Me))
        '    '    End If

        '    'Next

        'End Sub

        'Public Sub readxml(ByVal reader As XmlReader) Implements IXmlSerializable.ReadXml
        '    '       	reader.MoveToContent();
        '    'Name = reader.GetAttribute("Name");
        '    'Boolean isEmptyElement = reader.IsEmptyElement; // (1)
        '    'reader.ReadStartElement();
        '    'if (!isEmptyElement) // (1)
        '    '{
        '    '	Birthday = DateTime.ParseExact(reader.
        '    '		ReadElementString("Birthday"), "yyyy-MM-dd", null);
        '    '	reader.ReadEndElement();

        'End Sub

        Public Overrides Function CanSafelyRemoveFromModel() As Boolean
            Return False
        End Function

        ''' <summary>
        ''' Changes the Model of the Role to the target Model.
        ''' </summary>
        ''' <param name="arTargetModel">The Model to which the Role will be associated on completion of this method.</param>
        ''' <remarks></remarks>
        Public Sub ChangeModel(ByRef arTargetModel As FBM.Model)

            Me.Model = arTargetModel

            If arTargetModel.Role.Exists(AddressOf Me.Equals) Then
                '----------------------------------------------
                'The Role is already in the TargetModel
                '----------------------------------------------
            Else
                arTargetModel.AddRole(Me)
            End If

        End Sub

        'Public Function getschema() As XmlSchema Implements IXmlSerializable.GetSchema
        '    Return Nothing
        'End Function


        Public Overloads Function Clone(ByRef arModel As FBM.Model, Optional abAddToModel As Boolean = False) As Object

            Dim lrRole As New FBM.Role

            Try

                If arModel.Role.Exists(AddressOf Me.Equals) Then
                    '---------------------------------------------------------------------------------------------------------------------
                    'The target Role already exists in the target Model, so return the existing Role (from the target Model)
                    '  20150127-There seems no logical reason to clone an Role to a target Model if it already exists in the target
                    '  Model. This method is used when copying/pasting from one Model to a target Model, and (in general) the Role
                    '  won't exist in the target Model. If it does, then that's the Role that's needed.
                    '  NB Testing to see if the Signature of the FactType already exists in the target Model is already performed in the
                    '  Paste proceedure before dropping the FactType onto a target Page/Model. If there is/was any clashes, then the 
                    '  FactType being copied/pasted will have it's Id/Name/Symbol changed and will not be affected by this test to see
                    '  if the FactType already exists in the target Model.
                    '---------------------------------------------------------------------------------------------------------------------
                    lrRole = arModel.Role.Find(AddressOf Me.Equals)
                Else
                    With Me
                        lrRole.Model = arModel
                        lrRole.Id = .Id
                        lrRole.Name = .Name
                        lrRole.Symbol = .Symbol
                        lrRole.ConceptType = .ConceptType
                        lrRole.Deontic = .Deontic
                        lrRole.TypeOfJoin = .TypeOfJoin
                        lrRole.SequenceNr = .SequenceNr
                        lrRole.Mandatory = .Mandatory

                        If abAddToModel Then
                            arModel.AddRole(lrRole)
                        End If

                        'lrRole.Data (20150127-Currently set when cloning FactData)
                        lrRole.FactType = New FBM.FactType
                        lrRole.FactType.Id = .FactType.Id
                        If Me.Model.FactType.Exists(AddressOf lrRole.FactType.Equals) Then
                            lrRole.FactType = Me.Model.FactType.Find(AddressOf lrRole.FactType.Equals)
                        Else
                            lrRole.FactType = .FactType.Clone(arModel)
                        End If

                        lrRole.FrequencyConstraint = .FrequencyConstraint
                        lrRole.LongDescription = .LongDescription
                        lrRole.ShortDescription = .ShortDescription
                        'lrRole.RoleConstraintRole (20150202-Currently set when cloning a RoleConstraint.RoleConstraintRole)
                        lrRole.ValueRange = .ValueRange

                        Select Case .TypeOfJoin
                            Case pcenumRoleJoinType.EntityType
                                lrRole.JoinedORMObject = .JoinedORMObject.CloneEntityType(arModel)
                                lrRole.JoinsEntityType = .JoinedORMObject
                                If arModel.EntityType.Exists(AddressOf lrRole.JoinedORMObject.Equals) Then
                                    lrRole.JoinsEntityType = arModel.EntityType.Find(AddressOf lrRole.JoinedORMObject.Equals)
                                Else
                                    arModel.AddEntityType(lrRole.JoinedORMObject)
                                End If
                            Case pcenumRoleJoinType.ValueType
                                lrRole.JoinedORMObject = .JoinedORMObject.CloneValueType(arModel)
                                lrRole.JoinsValueType = .JoinedORMObject
                                If arModel.ValueType.Exists(AddressOf lrRole.JoinedORMObject.Equals) Then
                                    lrRole.JoinsValueType = arModel.ValueType.Find(AddressOf lrRole.JoinedORMObject.Equals)
                                Else
                                    arModel.AddValueType(lrRole.JoinedORMObject)
                                End If
                            Case pcenumRoleJoinType.FactType
                                lrRole.JoinedORMObject = .JoinedORMObject.CloneFactType(arModel)
                                lrRole.JoinsFactType = .JoinedORMObject
                                If arModel.FactType.Exists(AddressOf lrRole.JoinedORMObject.Equals) Then
                                    lrRole.JoinsFactType = arModel.FactType.Find(AddressOf lrRole.JoinedORMObject.Equals)
                                Else
                                    arModel.AddFactType(lrRole.JoinedORMObject)
                                End If
                        End Select
                    End With
                End If


                Return lrRole
            Catch ex As Exception
                Dim lsMessage As String = ""

                lsMessage = "Error: tORMRole.Clone: " & vbCrLf & vbCrLf & ex.Message
                Call Me.Model.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace, True)

                Return lrRole
            End Try

        End Function

        Public Function BelongsToTable() As String
            'Input  - Role_id
            'Output - The Table_Name to which the Role belongs            

            BelongsToTable = Nothing

            If Me.FactType.HasTotalRoleConstraint Then
                'Rule 3 and part of 1
                BelongsToTable = Me.FactType.Name
                Exit Function
            ElseIf Me.FactType.HasPartialButMultiRoleConstraint Then
                'Rule 1
                BelongsToTable = Me.FactType.Name
                Exit Function
            ElseIf Me.FactType.IsUnaryFactType Then
                If Me.TypeOfJoin = pcenumRoleJoinType.EntityType Then
                    'Then role joins to entity
                    BelongsToTable = Me.JoinsEntityType.Name
                    Exit Function
                ElseIf Me.TypeOfJoin = pcenumRoleJoinType.FactType Then
                    BelongsToTable = Me.JoinsFactType.Name
                End If
            ElseIf Me.FactType.IsBinaryFactType Then
                'Rules 2 and 4
                'For ind = 1 To role_count
                If Me.TypeOfJoin = pcenumRoleJoinType.EntityType Then
                    'Then role joins to entity
                    If Not Me.FactType.Is1To1BinaryFactType Then
                        If Me.HasInternalUniquenessConstraint Then
                            BelongsToTable = Me.JoinsEntityType.Name
                            Exit Function
                        End If
                    Else 'is 1:1 binary fact type
                        If Me.Mandatory = True Then
                            If Me.FactType.TypeOfBinaryFactType = 1 Then
                                'Part 1 of Rule 4
                                BelongsToTable = Me.JoinsEntityType.Name
                                Exit Function
                            ElseIf Me.FactType.TypeOfBinaryFactType = 2 Then
                                'Part 2 of Rule 4
                                BelongsToTable = "Dummy" 'role_record(role_id).role_name
                                Exit Function
                            End If
                        End If
                    End If
                ElseIf Me.TypeOfJoin = pcenumRoleJoinType.FactType Then
                    'Joins nested Fact Object

                    BelongsToTable = Me.JoinsFactType.Name
                    Exit Function
                End If
            End If

        End Function

        Public Function GetAttributeName() As String

            Dim lsAttributeName As String = "DummyAttribute"

            Try
                Select Case Me.TypeOfJoin
                    Case Is = pcenumRoleJoinType.EntityType
                        If Me.FactType.Is1To1BinaryFactType Then
                            lsAttributeName = Me.FactType.GetOtherRoleOfBinaryFactType(Me.Id).JoinedORMObject.Name
                        Else
                            lsAttributeName = Me.JoinedORMObject.Name
                        End If
                    Case Is = pcenumRoleJoinType.FactType
                        If Me.FactType.Is1To1BinaryFactType Then
                            lsAttributeName = Me.FactType.GetOtherRoleOfBinaryFactType(Me.Id).JoinedORMObject.Name
                        Else
                            lsAttributeName = Me.JoinedORMObject.Name
                        End If
                    Case Is = pcenumRoleJoinType.ValueType
                        Throw New Exception("Tried to get Attribute Name on Role joined to ValueType")
                End Select

                Return lsAttributeName

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                Me.Model.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace, True)

                Return "Error getting Attribute"
            End Try


        End Function

        Function HasInternalUniquenessConstraint() As Boolean
            '-------------------------------------------------------------------------------
            'Used whn Automatically generating Tables/Entities/Classes
            'Used predominantly to see which Entity of a binary FactType
            'owns the uniqueness constraint (Role can't be part of 1:1 binary fact type)
            '-------------------------------------------------------------------------------

            'Returns True if 'owns' a internal uniqueness constraint
            'else returns false
            Dim lrRoleConstraintRole As FBM.RoleConstraintRole

            HasInternalUniquenessConstraint = False

            For Each lrRoleConstraintRole In Me.RoleConstraintRole
                If lrRoleConstraintRole.RoleConstraint.RoleConstraintType = pcenumRoleConstraintType.InternalUniquenessConstraint Then
                    HasInternalUniquenessConstraint = True
                    Exit Function
                End If
            Next

        End Function

        Function IsAnAttributeRole() As Boolean

            IsAnAttributeRole = False

            Select Case Me.TypeOfJoin
                Case Is = pcenumRoleJoinType.EntityType
                    If Me.HasInternalUniquenessConstraint() And Me.FactType.Is1To1BinaryFactType Then
                        IsAnAttributeRole = True
                    ElseIf Me.FactType.HasTotalRoleConstraint Then
                        IsAnAttributeRole = True
                    End If
                Case Is = pcenumRoleJoinType.ValueType
                    'ValueType
                    If Me.HasInternalUniquenessConstraint() And Me.FactType.Is1To1BinaryFactType Then
                        IsAnAttributeRole = True
                    ElseIf Me.FactType.HasTotalRoleConstraint Then
                        IsAnAttributeRole = True
                    End If
                Case Is = pcenumRoleJoinType.FactType
                    'ObjectifiedFactType        
                    IsAnAttributeRole = True
            End Select

        End Function


        Function IsPartOfNestedFactType() As Integer
            '---------------------------------------------------------------
            'Returns true if the FactType/RoleGroup to which RoleId belongs
            'is a Nested_Fact type else returns false
            '---------------------------------------------------------------

            IsPartOfNestedFactType = False

            If Me.FactType.IsObjectified Then
                IsPartOfNestedFactType = True
            End If

        End Function


        'Public Overridable Sub update_from_model() Handles model.ModelUpdated


        '    If IsSomething(Me.FactType) Then
        '        'MsgBox("tRole: Model.ModelUpdated: " & Me.FactType.Name)
        '    End If
        '    If IsSomething(Me.JoinedORMObject) Then

        '        Select Case Me.TypeOfJoin
        '            Case pcenumRoleJoinType.EntityType
        '                Dim lrEntityType As Object = Me.JoinedORMObject
        '                If TypeOf (Me.JoinedORMObject) Is tEntityType Then
        '                    Me.JoinsEntityTypeId = lrEntityType.Id
        '                End If
        '            Case pcenumRoleJoinType.ValueType
        '                Dim lrValueType As Object = Me.JoinedORMObject
        '                If TypeOf (Me.JoinedORMObject) Is tValueType Then
        '                    Me.JoinsValueTypeId = lrValueType.Id
        '                End If
        '            Case pcenumRoleJoinType.FactType
        '                Dim lrFactType As Object = Me.JoinedORMObject
        '                If TypeOf (Me.JoinedORMObject) Is FBM.tFactType Then
        '                    Me.JoinsNestedFactTypeId = lrFactType.Id
        '                End If
        '        End Select
        '    End If

        'End Sub

        ''' <summary>
        ''' Reassigns the joined ModelObject of the Role
        ''' </summary>
        ''' <param name="arNewJoinedModelObject"></param>
        ''' <remarks></remarks>
        Public Sub ReassignJoinedModelObject(ByRef arNewJoinedModelObject As FBM.ModelObject)

            Try
                If arNewJoinedModelObject Is Me.JoinedORMObject Then
                    Exit Sub
                Else
                    Me.JoinedORMObject = arNewJoinedModelObject

                    Select Case arNewJoinedModelObject.ConceptType
                        Case Is = pcenumConceptType.EntityType
                            Me.TypeOfJoin = pcenumRoleJoinType.EntityType
                            Me.JoinsEntityType = Me.JoinedORMObject
                            Me.JoinsValueType = Nothing
                            Me.JoinsFactType = Nothing
                        Case Is = pcenumConceptType.ValueType
                            Me.TypeOfJoin = pcenumRoleJoinType.ValueType
                            Me.JoinsValueType = Me.JoinedORMObject
                            Me.JoinsEntityType = Nothing
                            Me.JoinsFactType = Nothing
                        Case Is = pcenumConceptType.FactType
                            Me.TypeOfJoin = pcenumRoleJoinType.FactType
                            Me.JoinsFactType = Me.JoinedORMObject
                            Me.JoinsEntityType = Nothing
                            Me.JoinsValueType = Nothing
                    End Select

                    RaiseEvent RoleJoinModified(arNewJoinedModelObject)

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
        ''' Removes the Role from the Model if it is okay to do so.
        '''   i.e. Will not remove the Role if there is a RoleConstraint attached to the Role.
        ''' </summary>
        ''' <remarks></remarks>
        Public Overrides Function RemoveFromModel(Optional ByVal abForceRemoval As Boolean = False, _
                                                  Optional ByVal abCheckForErrors As Boolean = True) As Boolean

            Try
                Me.Model.Role.Remove(Me)

                Return True

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                Me.Model.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace, True)

                Return False
            End Try

        End Function

        Public Overrides Sub Save()
        End Sub

        Public Sub SetMandatory(ByVal abMandatoryStatus As Boolean)

            Me._Mandatory = abMandatoryStatus
            Me.Model.IsDirty = True
            RaiseEvent MandatoryChanged(abMandatoryStatus)

        End Sub

        ''' <summary>
        ''' Sets the Name of the Role
        ''' </summary>
        ''' <param name="asName"></param>
        ''' <remarks></remarks>
        Public Sub SetName(ByVal asName As String)

            Me.Name = asName
            Me.Model.IsDirty = True
            RaiseEvent RoleNameChanged(asName)

        End Sub

        'Private Sub JoinedORMObject_updated() Handles JoinedORMObject.updated

        '    MsgBox("tRole: JoinedORMObject.updated: FactType:" & Me.FactType.Name)

        '    If IsSomething(Me.JoinedORMObject) Then
        '        Select Case Me.TypeOfJoin
        '            Case pcenumRoleJoinType.EntityType
        '                Dim lrEntityType As Object = Me.JoinedORMObject
        '                If TypeOf (Me.JoinedORMObject) Is tEntityType Then
        '                    Me.JoinsEntityTypeId = lrEntityType.Id
        '                End If
        '            Case pcenumRoleJoinType.ValueType
        '                Dim lrValueType As Object = Me.JoinedORMObject
        '                If TypeOf (Me.JoinedORMObject) Is tValueType Then
        '                    Me.JoinsValueTypeId = lrValueType.Id
        '                End If
        '            Case pcenumRoleJoinType.FactType
        '                Dim lrFactType As Object = Me.JoinedORMObject
        '                If TypeOf (Me.JoinedORMObject) Is FBM.tFactType Then
        '                    Me.JoinsNestedFactTypeId = lrFactType.Id
        '                End If
        '        End Select
        '    End If

        'End Sub
    End Class
End Namespace