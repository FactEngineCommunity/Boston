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

        '<CategoryAttribute("Role"), _
        'DefaultValueAttribute(GetType(Integer), "0"), _
        'DescriptionAttribute("When populated, determines the number of times an instance plays the Role.")> _
        <XmlIgnore()> _        
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
        <CategoryAttribute("Role"),
             DefaultValueAttribute(False),
             DescriptionAttribute("True if the Role is a Mandatory Role, else False.")>
        Public Property Mandatory() As Boolean
            Get
                Return _Mandatory
            End Get
            Set(ByVal Value As Boolean)
                _Mandatory = Value
            End Set
        End Property

        <XmlIgnore()>
        Public ReadOnly Property DerivedRoleName As String
            Get
                If Me.Name = "" Then
                    If Me.FactType.allRolesJoinTheSameObject Then
                        Return Me.JoinedORMObject.Id & Me.FactType.Id & Me.FactType.RoleGroup.IndexOf(Me)

                    Else
                        If Me.FactType.getCorrespondingRDSTable(Nothing, True) IsNot Nothing Then
                            Return Me.JoinedORMObject.Id & Me.FactType.getCorrespondingRDSTable.DatabaseName
                        Else
                            Return Me.JoinedORMObject.Id & Me.FactType.Id
                        End If

                    End If
                Else
                    Return Me.Name
                End If
            End Get
        End Property

        <XmlIgnore()> _
        <DebuggerBrowsable(DebuggerBrowsableState.Never)> _
        Public _Value_Range As String
        '<CategoryAttribute("Role"), _
        'DefaultValueAttribute(""), _
        'DescriptionAttribute("The range of allowable Values for the Role.")> _
        <XmlIgnore()>
        Public Property ValueRange() As String
            Get
                Return _Value_Range
            End Get
            Set(ByVal Value As String)
                _Value_Range = Value
            End Set
        End Property


#Region "Value Constraints"

        ''' <summary>
        ''' If the Role has RoleValueConstraint the RoleConstraint that is that RoleValueConstraint.
        ''' </summary>
        <XmlIgnore()>
        Public WithEvents RoleConstraintRoleValueConstraint As FBM.RoleConstraint = Nothing

        <XmlIgnore()>
        <DebuggerBrowsable(DebuggerBrowsableState.Never)>
        Public _ValueConstraint As New List(Of FBM.Concept)

        <XmlIgnore()>
        <DebuggerBrowsable(DebuggerBrowsableState.Never)>
        Public _ValueConstraintList As New Viev.Strings.StringCollection

        '<XmlIgnore()> _
        <CategoryAttribute("Value Type"),
         Browsable(True),
         [ReadOnly](False),
         DescriptionAttribute("The List of Values that Objects of this Value Type may take."),
         Editor(GetType(tStringCollectionEditor), GetType(System.Drawing.Design.UITypeEditor))>
        Public Property ValueConstraint() As Viev.Strings.StringCollection 'StringCollection 
            '   DefaultValueAttribute(""), _
            '   BindableAttribute(True), _
            '   DesignOnly(False), _
            Get
                Return Me._ValueConstraintList
            End Get
            Set(ByVal Value As Viev.Strings.StringCollection)
                Me._ValueConstraintList = Value
                '----------------------------------------------------
                'Update the set of Concepts/Symbols/Values
                '  within the 'value_constraint' for this ValueType.
                '----------------------------------------------------
                Dim lsString As String
                For Each lsString In Me._ValueConstraintList
                    Dim lrConcept As New FBM.Concept(lsString)
                    If Me._ValueConstraint.Contains(lrConcept) Then
                        '-------------------------------------------------
                        'Nothing to do, because the Concept/Symbol/Value
                        '  already exists for the 'value_constraint'
                        '  for this ValueType.
                        '-------------------------------------------------
                    Else
                        '-------------------------------------------
                        'Add the Concept/Symbol/Value to the Model
                        '-----------------------------------------
                        Dim lrModelDictionaryEntry As New FBM.DictionaryEntry(Me.Model, lrConcept.Symbol, pcenumConceptType.Value)
                        Me.Model.AddModelDictionaryEntry(lrModelDictionaryEntry)
                        '-----------------------------------------
                        'Add the Concept/Symbol/Value to the
                        '  'value_constraint' for this ValueType
                        '-----------------------------------------
                        Me._ValueConstraint.Add(lrConcept)
                    End If
                Next
            End Set
        End Property
#End Region

        <XmlIgnore()> _
        Public SequenceNr As Integer 'The position withn the FactType/RoleGroup

        <XmlAttribute()>
        Public ReadOnly Property TypeOfJoin As pcenumRoleJoinType
            Get
                If Me.JoinedORMObject Is Nothing Then
                    Return pcenumRoleJoinType.None
                End If
                Select Case Me.JoinedORMObject.ConceptType
                    Case Is = pcenumConceptType.ValueType
                        Return pcenumRoleJoinType.ValueType
                    Case Is = pcenumConceptType.EntityType
                        Return pcenumRoleJoinType.EntityType
                    Case Is = pcenumConceptType.FactType
                        Return pcenumRoleJoinType.FactType
                End Select
            End Get
        End Property

        <XmlIgnore()>
        Public ReadOnly Property JoinsEntityType As FBM.EntityType
            Get
                If Me.TypeOfJoin = pcenumRoleJoinType.EntityType Then
                    Return CType(Me.JoinedORMObject, FBM.EntityType)
                Else
                    Return Nothing
                End If
            End Get
        End Property

        <XmlIgnore()>
        Public ReadOnly Property JoinsValueType As FBM.ValueType
            Get
                If Me.TypeOfJoin = pcenumRoleJoinType.ValueType Then
                    Return CType(Me.JoinedORMObject, FBM.ValueType)
                Else
                    Return Nothing
                End If
            End Get
        End Property

        <XmlIgnore()>
        Public ReadOnly Property JoinsFactType As FBM.FactType
            Get
                If Me.TypeOfJoin = pcenumRoleJoinType.FactType Then
                    Return CType(Me.JoinedORMObject, FBM.FactType)
                Else
                    Return Nothing
                End If
            End Get
        End Property

        <XmlIgnore()> _
        Public WithEvents JoinedORMObject As New FBM.ModelObject 'WithEvents

        <XmlIgnore()> _
        Public RoleConstraintRole As New List(Of FBM.RoleConstraintRole) 'RoleConstraints are serialized seperately so that the serialiser doesn't go into an infinite loop.

        <XmlIgnore()> _
        Public Data As New List(Of FBM.FactData) 'As Values ('Concepts'...or just 'Data') are added to a Fact within a FactType, within which this Role exists, this enumerationis populated.
        'At first, that seems strange, as the RoleData seems to be more a part of a Fact then it ever is by itself (e.g. what is b outside of {a,b,c}).
        'There is, however, a different perspective, which is that an Entity (e.g. b) is an object in its own right, and is merely associated with a Fact, AND a Role within that Fact/FactType.
        'This perspective allows LamdaCalculus type searching for Facts, knowing only the Role and the Data (tRoleData has a member, 'Fact').

        ''' <summary>
        ''' FactEngine specific. As in when operating over a MongoDB database. 'Lecturer likes CarType' may have CarType as an array on Lecturer collection.
        '''   The ODBC driver for MongoDB expects not to join Lecturer and CarType, but rather link Lecturer to Lecturer_CarType (array virtual table in ODBC).
        '''   Also lets the user know that the collection is an array.
        ''' </summary>
        Public IsArray As Boolean = False


        <XmlIgnore()> _
        Public KLFreeVariableLabel As String = "" 'When generating proofs in KL, a letter from pcenumKLFreeVariable gets assigned to each Role in a FactType.

        Public NORMALinksToUnaryFactTypeValueType As Boolean = False

        Public Event MandatoryChanged(ByVal abMandatoryStatus As Boolean)
        Public Event RoleJoinModified(ByRef arModelObject As FBM.ModelObject)
        Public Event RoleNameChanged(ByVal asNewRoleName As String)
        Public Event ValueRangeChanged(ByVal asNewValueRange As String)
        Public Event ValueConstraintAdded(ByVal asValueConstraint As String)
        Public Event ValueConstraintRemoved(ByVal asRemovedValueConstraint As String)
        Public Event ValueConstraintModified(ByVal asOldValue As String, ByVal asNewValue As String)

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

        End Sub

        Sub New(ByRef arFactType As FBM.FactType, ByRef arJoinedObject As Object)

            Call Me.New()

            Me.FactType = arFactType
            Me.Model = arFactType.Model

            Me.JoinedORMObject = arJoinedObject

            '----------------------------------------------------------
            'Add the Role to the RoleGroup of the FactType of the Role
            '----------------------------------------------------------            
            Me.SequenceNr = Me.FactType.Arity

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
        Public Shadows Sub ChangeModel(ByRef arTargetModel As FBM.Model, ByVal abAddToModel As Boolean)

            Me.Model = arTargetModel

            If abAddToModel Then
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
                        lrRole.SequenceNr = .SequenceNr
                        lrRole.Mandatory = .Mandatory
                        lrRole.isDirty = True

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
                                If arModel.EntityType.Exists(AddressOf lrRole.JoinedORMObject.Equals) Then
                                    'lrRole.JoinsEntityType = arModel.EntityType.Find(AddressOf lrRole.JoinedORMObject.Equals)
                                    lrRole.JoinedORMObject = arModel.EntityType.Find(AddressOf lrRole.JoinedORMObject.Equals)
                                ElseIf arModel.ModelId = .Model.ModelId Then
                                    arModel.AddEntityType(lrRole.JoinedORMObject)
                                    'lrRole.JoinsEntityType = .JoinedORMObject
                                    lrRole.JoinedORMObject = .JoinedORMObject
                                Else
                                    Dim lrEntityType As FBM.EntityType = .JoinedORMObject
                                    lrEntityType = lrEntityType.Clone(arModel, True, lrEntityType.IsMDAModelElement)
                                    'lrRole.JoinsEntityType = lrEntityType
                                    lrRole.JoinedORMObject = lrEntityType
                                End If                                
                            Case pcenumRoleJoinType.ValueType                                                                
                                If arModel.ValueType.Exists(AddressOf lrRole.JoinedORMObject.Equals) Then
                                    'lrRole.JoinsValueType = arModel.ValueType.Find(AddressOf lrRole.JoinedORMObject.Equals)
                                    lrRole.JoinedORMObject = arModel.ValueType.Find(AddressOf lrRole.JoinedORMObject.Equals)
                                ElseIf arModel.ModelId = .Model.ModelId Then
                                    arModel.AddValueType(.JoinedORMObject)
                                    'lrRole.JoinsValueType = .JoinedORMObject
                                    lrRole.JoinedORMObject = .JoinedORMObject
                                Else
                                    'Cloning to a new Model.
                                    Dim lrValueType As FBM.ValueType = .JoinedORMObject
                                    lrValueType = lrValueType.Clone(arModel, True, lrValueType.IsMDAModelElement)
                                    'lrRole.JoinsValueType = lrValueType
                                    lrRole.JoinedORMObject = lrValueType
                                End If
                            Case pcenumRoleJoinType.FactType
                                If arModel.FactType.Exists(AddressOf .JoinedORMObject.Equals) Then
                                    'lrRole.JoinsFactType = arModel.FactType.Find(AddressOf .JoinedORMObject.Equals)
                                    lrRole.JoinedORMObject = arModel.FactType.Find(AddressOf .JoinedORMObject.Equals)
                                ElseIf arModel.ModelId = .Model.ModelId Then
                                    arModel.AddFactType(.JoinedORMObject)
                                    'lrRole.JoinsFactType = .JoinedORMObject
                                    lrRole.JoinedORMObject = .JoinedORMObject
                                Else
                                    Dim lrFactType As FBM.FactType = lrRole.JoinedORMObject
                                    lrFactType = lrFactType.Clone(arModel, True, lrFactType.IsMDAModelElement)
                                    'lrRole.JoinsFactType = lrFactType
                                    lrRole.JoinedORMObject = lrFactType
                                End If
                        End Select
                    End With
                End If

                Return lrRole
            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)

                Return lrRole
            End Try

        End Function

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
                    lrRoleInstance.Mandatory = .Mandatory
                    lrRoleInstance.SequenceNr = .SequenceNr

                    lrRoleInstance.RoleName = New FBM.RoleName(lrRoleInstance, lrRoleInstance.Name)

                    Select Case .TypeOfJoin
                        Case Is = pcenumRoleJoinType.EntityType
                            'lrEntityTypeInstance = New FBM.EntityTypeInstance
                            lrEntityTypeInstance = arPage.EntityTypeInstance.Find(AddressOf .JoinedORMObject.EqualsByName)
                            'lrRoleInstance.JoinsEntityType = New FBM.EntityTypeInstance
                            lrRoleInstance.JoinedORMObject = lrEntityTypeInstance
                            If lrRoleInstance.JoinedORMObject Is Nothing Then
                                lrRoleInstance.JoinedORMObject = .JoinedORMObject.CloneEntityTypeInstance(arPage)
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
                            lrRoleInstance.JoinedORMObject = arPage.ValueTypeInstance.Find(AddressOf .JoinedORMObject.EqualsByName)
                            If lrRoleInstance.JoinedORMObject Is Nothing Then
                                lrRoleInstance.JoinedORMObject = .JoinedORMObject.CloneInstance(arPage)
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
                            lrRoleInstance.JoinedORMObject = arPage.FactTypeInstance.Find(AddressOf .JoinedORMObject.EqualsByName)
                            If lrRoleInstance.JoinedORMObject Is Nothing Then
                                lrRoleInstance.JoinedORMObject = .JoinedORMObject.CloneInstance(arPage, abAddToPage)

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

                    If Me.RoleConstraintRoleValueConstraint IsNot Nothing Then
                        lrRoleInstance.RoleConstraintRoleValueConstraint = Me.RoleConstraintRoleValueConstraint.CloneRoleValueConstraintInstance(arPage, False)

                        For Each lsConstraintValue In lrRoleInstance.RoleConstraintRoleValueConstraint.ValueConstraint
                            lrRoleInstance.ValueConstraint.Add(lsConstraintValue)
                        Next
                    End If

                    If abAddToPage Then
                        arPage.RoleInstance.AddUnique(lrRoleInstance)
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

        Public Sub AddValueConstraint(ByVal asValueConstraint As String)

            Dim lsMessage As String

            Try
                Me.ValueConstraint.Add(asValueConstraint)
                Try
                    If Me.RoleConstraintRoleValueConstraint Is Nothing Then
                        Call Me.CreateOwnRoleValueConstraint
                    End If

                    Call Me.RoleConstraintRoleValueConstraint.AddValueConstraint(asValueConstraint)
                Catch ex As Exception
                    Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                    lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                    lsMessage &= vbCrLf & vbCrLf & ex.Message
                    prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
                End Try

                Call Me.makeDirty()

                RaiseEvent ValueConstraintAdded(asValueConstraint)

            Catch ex As Exception
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Private Sub CreateOwnRoleValueConstraint()

            Try
                Dim larRole As New List(Of FBM.Role)
                Dim lsRoleConstraintName As String = Me.Model.CreateUniqueRoleConstraintName(pcenumRoleConstraintType.RoleValueConstraint.ToString, 0)
                Dim lrRoleConstraint As New FBM.RoleConstraint(Me.Model, lsRoleConstraintName, True,
                                                               pcenumRoleConstraintType.RoleValueConstraint, Nothing, True)


                lrRoleConstraint.RoleConstraintRole.Add(New FBM.RoleConstraintRole(Me, lrRoleConstraint,,,, True))
                lrRoleConstraint.makeDirty()

                Me.Model.AddRoleConstraint(lrRoleConstraint)

                Me.RoleConstraintRoleValueConstraint = lrRoleConstraint

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        ''' <summary>
        ''' Used when adding a new Role to a FactType. If the Role is a RDSForeignKeyRole, then must add the related Column (for the new Role) to the Relation, and the Relation to the Column.
        ''' PRECONDITION: See FBM.Role.isRDSForeignKeyRole. Role must be the ResponsibleRole of a Column. Relation must have existing Columns (obviously).
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function belongsToRelation() As RDS.Relation

            Try
                Dim larRelation = From Relation In Me.Model.RDS.Relation _
                                  From OriginColumn In Relation.OriginColumns _
                                  Where OriginColumn.Role Is Me _
                                  Select Relation Distinct

                If larRelation.Count > 0 Then
                    Return larRelation.First
                Else
                    Return Nothing 'Don't throw an error because Relation may not have been created yet. This function called when a Column is added to a Table.
                End If

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)

                Return Nothing
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
                            If Me.JoinsEntityType.IsObjectifyingEntityType Then
                                BelongsToTable = Me.JoinsEntityType.ObjectifiedFactType.Id
                            Else
                                BelongsToTable = Me.JoinsEntityType.Name
                            End If
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
                                BelongsToTable = Me.JoinedORMObject.Id ' "Dummy" 'role_record(role_id).role_name
                                Exit Function
                            End If
                        Else
                            If Me.FactType.GetOtherRoleOfBinaryFactType(Me.Id).Mandatory = False Then
                                If Me.FactType.GetOtherRoleOfBinaryFactType(Me.Id).JoinedORMObject.ConceptType = pcenumConceptType.ValueType Then
                                    BelongsToTable = Me.JoinedORMObject.Name
                                Else
                                    BelongsToTable = Me.JoinedORMObject.Name
                                End If
                            Else
                                BelongsToTable = Me.JoinedORMObject.Name
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

            Dim lsAttributeName As String = ""
            Dim lrFactTypeReading As FBM.FactTypeReading
            Dim larRole As List(Of FBM.Role)
            Dim lrModelObject As FBM.ModelObject

            Try

                Select Case Me.TypeOfJoin
                    Case Is = pcenumRoleJoinType.EntityType
                        If Me.FactType.Is1To1BinaryFactType Then
                            lrModelObject = Me.FactType.GetOtherRoleOfBinaryFactType(Me.Id).JoinedORMObject
                            lsAttributeName = lrModelObject.Name

                            Select Case lrModelObject.ConceptType
                                Case Is = pcenumConceptType.EntityType
                                    Dim lrEntityType As FBM.EntityType
                                    lrEntityType = lrModelObject
                                    If lrEntityType.HasSimpleReferenceScheme Then
                                        lsAttributeName &= lrEntityType.ReferenceMode.Replace(".", "")
                                    End If
                            End Select

                            larRole = New List(Of FBM.Role)
                            larRole.Add(Me)
                            larRole.Add(Me.FactType.GetOtherRoleOfBinaryFactType(Me.Id))
                            lrFactTypeReading = Me.FactType.FindSuitableFactTypeReadingByRoles(larRole, True)
                            If lrFactTypeReading IsNot Nothing Then
                                lsAttributeName = lrFactTypeReading.PredicatePart(1).PreBoundText.Replace("-", "") & lsAttributeName
                            End If

                        ElseIf Me.FactType.IsBinaryFactType Then
                            lrModelObject = Me.FactType.GetOtherRoleOfBinaryFactType(Me.Id).JoinedORMObject
                            lsAttributeName = lrModelObject.Name

                            Select Case lrModelObject.ConceptType
                                Case Is = pcenumConceptType.EntityType
                                    Dim lrEntityType As FBM.EntityType
                                    lrEntityType = lrModelObject
                                    If lrEntityType.HasSimpleReferenceScheme Then
                                        lsAttributeName &= lrEntityType.ReferenceMode.Replace(".", "")
                                    End If
                            End Select

                            larRole = New List(Of FBM.Role)
                            larRole.Add(Me)
                            larRole.Add(Me.FactType.GetOtherRoleOfBinaryFactType(Me.Id))
                            lrFactTypeReading = Me.FactType.FindSuitableFactTypeReadingByRoles(larRole, True)
                            If lrFactTypeReading IsNot Nothing Then
                                lsAttributeName = lrFactTypeReading.PredicatePart(1).PreBoundText.Replace("-", "") & lsAttributeName
                            Else
                                larRole = New List(Of FBM.Role)
                                larRole.Add(Me.FactType.GetOtherRoleOfBinaryFactType(Me.Id))
                                larRole.Add(Me)                                
                                lrFactTypeReading = Me.FactType.FindSuitableFactTypeReadingByRoles(larRole, True)
                                If lrFactTypeReading IsNot Nothing Then
                                    lsAttributeName = lrFactTypeReading.PredicatePart(1).PreBoundText.Replace("-", "") & lsAttributeName
                                End If
                            End If
                        ElseIf Me.FactType.IsUnaryFactType Then
                                If Me.FactType.FactTypeReading.Count > 0 Then
                                    lsAttributeName = Viev.Strings.RemoveWhiteSpace(Viev.Strings.MakeCapCamelCase(Me.FactType.FactTypeReading(0).PredicatePart(0).PredicatePartText))
                                Else
                                    lsAttributeName = "ErrorNeedFactTypeReadingForUnaryFactType"
                                End If
                        Else
                                Select Case Me.JoinedORMObject.ConceptType
                                    Case Is = pcenumConceptType.EntityType
                                        Dim lrEntityType As FBM.EntityType
                                        lrEntityType = Me.JoinedORMObject
                                        If lrEntityType.HasSimpleReferenceScheme Then
                                            lsAttributeName &= lrEntityType.ReferenceModeValueType.Id
                                        Else
                                            lsAttributeName = Me.JoinedORMObject.Name
                                        End If
                                    Case Else
                                        lsAttributeName = Me.JoinedORMObject.Name
                                End Select
                        End If
                    Case Is = pcenumRoleJoinType.FactType
                        If Me.FactType.Is1To1BinaryFactType Then
                            lsAttributeName = Me.FactType.GetOtherRoleOfBinaryFactType(Me.Id).JoinedORMObject.Name

                            larRole = New List(Of FBM.Role)
                            larRole.Add(Me)
                            larRole.Add(Me.FactType.GetOtherRoleOfBinaryFactType(Me.Id))
                            lrFactTypeReading = Me.FactType.FindSuitableFactTypeReadingByRoles(larRole, True)
                            If lrFactTypeReading IsNot Nothing Then
                                lsAttributeName = lrFactTypeReading.PredicatePart(1).PreBoundText.Replace("-", "") & lsAttributeName
                            End If

                        ElseIf Me.FactType.IsBinaryFactType Then
                            lsAttributeName = Me.FactType.GetOtherRoleOfBinaryFactType(Me.Id).JoinedORMObject.Name

                            larRole = New List(Of FBM.Role)
                            larRole.Add(Me)
                            larRole.Add(Me.FactType.GetOtherRoleOfBinaryFactType(Me.Id))
                            lrFactTypeReading = Me.FactType.FindSuitableFactTypeReadingByRoles(larRole, True)
                            If lrFactTypeReading IsNot Nothing Then
                                lsAttributeName = lrFactTypeReading.PredicatePart(1).PreBoundText.Replace("-", "") & lsAttributeName
                            End If
                        Else
                            lsAttributeName = Me.JoinedORMObject.Name
                        End If
                    Case Is = pcenumRoleJoinType.ValueType
                        lsAttributeName = Me.JoinedORMObject.Name
                        'Throw New Exception("Tried to get Attribute Name on Role joined to ValueType")
                End Select

                Return Viev.Strings.MakeCapCamelCase(Viev.Strings.RemoveWhiteSpace(lsAttributeName))

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)

                Return "Error getting Attribute"
            End Try


        End Function

        ''' <summary>
        ''' PRECONDITIONS: The Role represents only one Column, not many (as when a Role of an ObjectifiedFactType references another ObjectifiedFactType).
        ''' The Role is within a BinaryFactType or UnaryFactType, and the other Role (if Binary) does not join an ObjectifiedFactType.
        ''' </summary>
        ''' <param name="arTable"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetCorrespondingUnaryOrBinaryFactTypeColumn(ByRef arTable As RDS.Table) As RDS.Column

            Dim lsColumnName As String = "DummyAttribute"
            Dim lrFactTypeReading As FBM.FactTypeReading
            Dim larRole As List(Of FBM.Role)
            Dim lrActiveRole As FBM.Role = Me 'Likely to change (as below is processed).
            Dim lrModelElement As FBM.ModelObject
            Dim lbIsMandatory As Boolean = False

            Try
                'CodeSafe:
                If Me.FactType.Arity > 2 Then Throw New Exception("Function called for a Role within a FactType that has an Arity > 2.")
                If Me.FactType.Arity = 2 Then
                    If Me.FactType.GetOtherRoleOfBinaryFactType(Me.Id).JoinedORMObject.ConceptType = pcenumConceptType.FactType Then
                        Throw New Exception("Function called for a BinaryFactType where the other Role joins an ObjectifiedFactType.")
                    End If
                End If
                If (Me.FactType.HasTotalRoleConstraint Or Me.FactType.HasPartialButMultiRoleConstraint) And Me.FactType.Arity > 1 Then
                    Throw New Exception("Function called for a Role within a FactType that has a TotalRoleConstraint or has a PartialButMultiRoleConstriant.")
                End If

                Select Case Me.JoinedORMObject.ConceptType
                    Case Is = pcenumConceptType.EntityType,
                              pcenumConceptType.FactType
                        If (Me.FactType.Is1To1BinaryFactType Or Me.FactType.IsBinaryFactType) Then _
                            'And Not (Me.FactType.HasTotalRoleConstraint Or Me.FactType.HasPartialButMultiRoleConstraint) Then

                            lrActiveRole = Me.FactType.GetOtherRoleOfBinaryFactType(Me.Id)
                            lrModelElement = lrActiveRole.JoinedORMObject

                            Select Case lrModelElement.ConceptType
                                Case Is = pcenumConceptType.EntityType
                                    Dim lrEntityType As FBM.EntityType
                                    lrEntityType = lrModelElement

                                    If lrEntityType.HasSimpleReferenceScheme Then

                                        If lrEntityType.IsSubtype Then
                                            Dim lrModelObject As FBM.ModelObject = lrEntityType.GetTopmostSupertype
                                            If lrModelObject.ConceptType = pcenumConceptType.EntityType Then
                                                Dim lrTopMostSupertypeEntityType As FBM.EntityType = lrModelObject

                                                lrActiveRole = lrTopMostSupertypeEntityType.ReferenceModeFactType.RoleGroup(1)
                                                lsColumnName = lrTopMostSupertypeEntityType.ReferenceModeValueType.Id
                                            Else 'TopmostSupertype is an ObjectifiedFactType
                                                Throw New NotImplementedException("Called EntityType.GetCorrespondingUnaryOrBinaryFactTypeColumn for Entity Type that has a topmost Supertype that is a FactType. Not yet implmented.")
                                            End If
                                        Else
                                            lrActiveRole = lrEntityType.ReferenceModeFactType.RoleGroup(1)
                                            lsColumnName = lrEntityType.ReferenceModeValueType.Id
                                        End If

                                    Else
                                        lsColumnName = lrEntityType.Id
                                    End If

                                Case Is = pcenumConceptType.ValueType

                                    lsColumnName = lrModelElement.Id
                            End Select


                            larRole = New List(Of FBM.Role)
                            larRole.Add(Me)
                            larRole.Add(lrActiveRole)
                            lrFactTypeReading = Me.FactType.FindSuitableFactTypeReadingByRoles(larRole, True)
                            If lrFactTypeReading IsNot Nothing Then
                                lsColumnName = lrFactTypeReading.PredicatePart(1).PreBoundText.Replace("-", "") & lsColumnName
                            End If

                        ElseIf Me.FactType.IsUnaryFactType Then
                            lrActiveRole = Me
                            If arTable.FBMModelElement Is Me.FactType Then
                                Select Case Me.JoinedORMObject.GetType
                                    Case Is = GetType(FBM.EntityType)
                                        If Me.JoinsEntityType.HasSimpleReferenceScheme Then
                                            lsColumnName = CType(Me.JoinsEntityType.GetTopmostNonAbsorbedSupertype, FBM.EntityType).ReferenceModeValueType.Id
                                        ElseIf Me.JoinsEntityType.HasCompoundReferenceMode Then
                                            Throw New Exception("Called for Entity Type with Compound Reference Scheme.")
                                        Else
                                            lsColumnName = Me.JoinedORMObject.Id
                                        End If
                                    Case Is = GetType(FBM.FactType)
                                        Throw New Exception("Called for Role that joins a Fact Type.")
                                End Select
                            ElseIf Me.FactType.FactTypeReading.Count > 0 Then
                                lsColumnName = Viev.Strings.RemoveWhiteSpace(Viev.Strings.MakeCapCamelCase(Me.FactType.FactTypeReading(0).PredicatePart(0).PredicatePartText))
                            Else
                                lsColumnName = "ErrorNeedFactTypeReadingForUnaryFactType"
                            End If
                        Else
                                Throw New Exception("Not caterd for. Contact www.viev.com")
                        End If

                    Case Is = pcenumConceptType.ValueType
                        lrActiveRole = Me
                        lsColumnName = Me.JoinedORMObject.Name
                        Throw New Exception("Tried to get Column for a Role joined to ValueType")
                End Select

                lsColumnName = Viev.Strings.MakeCapCamelCase(Viev.Strings.RemoveWhiteSpace(lsColumnName))
                lsColumnName = arTable.createUniqueColumnName(Nothing, lsColumnName, 0)

                If Me.Mandatory Then
                    lbIsMandatory = True
                Else
                    If Me.FactType.HasTotalRoleConstraint Or Me.FactType.HasPartialButMultiRoleConstraint Then

                        Dim larRoleConstraint = From RoleConstraint In Me.FactType.InternalUniquenessConstraint _
                                                Where RoleConstraint.Role.Contains(Me)
                                                Select RoleConstraint

                        For Each lrRoleConstraint In larRoleConstraint
                            lbIsMandatory = True
                        Next
                    End If
                End If

                Return New RDS.Column(arTable, lsColumnName, Me, lrActiveRole, lbIsMandatory)

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)

                Return Nothing
            End Try

        End Function

        ''' <summary>
        ''' PRECONDITIONS: The Role represents only one Column, not many (as when a Role of an ObjectifiedFactType references another ObjectifiedFactType).
        ''' The Role is within a FactType/ObjectifiedFactType.
        ''' </summary>
        ''' <param name="arTable"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetCorrespondingFactTypeColumn(ByRef arTable As RDS.Table, Optional ByRef arResponsibleRole As FBM.Role = Nothing) As RDS.Column

            Dim lsColumnName As String = "DummyAttribute"
            'Dim lrFactTypeReading As FBM.FactTypeReading
            'Dim larRole As List(Of FBM.Role)
            Dim lrActiveRole As FBM.Role = Me 'Likely to change (as below is processed).
            Dim lrModelElement As FBM.ModelObject
            Dim lbIsMandatory As Boolean = False

            Try
                Dim lrResponsibleRole As FBM.Role
                If arResponsibleRole Is Nothing Then
                    lrResponsibleRole = Me
                Else
                    lrResponsibleRole = arResponsibleRole
                End If

                Select Case Me.JoinedORMObject.ConceptType
                    Case Is = pcenumConceptType.EntityType

                        lrActiveRole = Me
                        lrModelElement = lrActiveRole.JoinedORMObject

                        Select Case lrModelElement.ConceptType
                            Case Is = pcenumConceptType.EntityType
                                Dim lrEntityType As FBM.EntityType
                                lrEntityType = lrModelElement

                                If lrEntityType.HasSimpleReferenceScheme Then
                                    If lrEntityType.IsSubtype Then
                                        Dim lrModelObject As FBM.ModelObject = lrEntityType.GetTopmostSupertype
                                        Select Case lrModelElement.ConceptType
                                            Case Is = pcenumConceptType.EntityType
                                                Dim lrTopmostSupertypeEntityType As FBM.EntityType = lrModelObject

                                                lrActiveRole = lrTopmostSupertypeEntityType.ReferenceModeFactType.RoleGroup(1)
                                                lsColumnName = lrTopmostSupertypeEntityType.ReferenceModeValueType.Id
                                            Case Else
                                                Throw New NotImplementedException("Called EntityType.GetCorrespondingFactTypeColumn for an EntityType that is a Subtype of a FactType as TopmostSupertype. Not yet implemented.")
                                        End Select

                                    Else
                                        lrActiveRole = lrEntityType.ReferenceModeFactType.RoleGroup(1)
                                        lsColumnName = lrEntityType.ReferenceModeValueType.Id
                                    End If
                                Else
                                    lsColumnName = lrEntityType.Id
                                End If

                            Case Is = pcenumConceptType.ValueType

                                lsColumnName = lrModelElement.Id
                        End Select

                        'ToDo: Must actually get this information from the corresponding LinkFactType.
                        'larRole = New List(Of FBM.Role)
                        'larRole.Add(Me)
                        'larRole.Add(lrActiveRole)
                        'lrFactTypeReading = Me.FactType.FindSuitableFactTypeReadingByRoles(larRole, True)
                        'If lrFactTypeReading IsNot Nothing Then
                        '    lsColumnName = lrFactTypeReading.PredicatePart(1).PreBoundText.Replace("-", "") & lsColumnName
                        'End If

                    Case Is = pcenumConceptType.ValueType
                        lrActiveRole = Me
                        lsColumnName = Me.JoinedORMObject.Name

                        'ToDo: Must actually get this information from the corresponding LinkFactType.
                        'larRole = New List(Of FBM.Role)
                        'larRole.Add(Me)
                        'larRole.Add(lrActiveRole)
                        'lrFactTypeReading = Me.FactType.FindSuitableFactTypeReadingByRoles(larRole, True)
                        'If lrFactTypeReading IsNot Nothing Then
                        '    lsColumnName = lrFactTypeReading.PredicatePart(1).PreBoundText.Replace("-", "") & lsColumnName
                        'End If
                End Select

                lsColumnName = Viev.Strings.MakeCapCamelCase(Viev.Strings.RemoveWhiteSpace(lsColumnName))
                lsColumnName = arTable.createUniqueColumnName(Nothing, lsColumnName, 0)

                If Me.Mandatory Then
                    lbIsMandatory = True
                Else
                    If Me.FactType.HasTotalRoleConstraint Or Me.FactType.HasPartialButMultiRoleConstraint Then

                        Dim larRoleConstraint = From RoleConstraint In Me.FactType.InternalUniquenessConstraint
                                                Where RoleConstraint.Role.Contains(Me)
                                                Select RoleConstraint

                        For Each lrRoleConstraint In larRoleConstraint
                            lbIsMandatory = True
                        Next
                    End If
                End If

                Dim lrColumn As RDS.Column = New RDS.Column(arTable, lsColumnName, lrResponsibleRole, lrActiveRole, lbIsMandatory)

                '20210505-VM-No longer needed. IsPartOfPrimaryKey uses Table Indexes to determine.
                'If lrResponsibleRole.isPartOfFactTypesPreferredReferenceScheme Then
                '    lrColumn.ContributesToPrimaryKey = True
                'End If

                Return lrColumn

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)

                Return Nothing
            End Try

        End Function

        ''' <summary>
        ''' PRECONDITION: Role is a ResponsibeRole of a Column in a RDS Table.
        ''' </summary>
        ''' <returns></returns>
        Public Overrides Function getCorrespondingRDSTable() As RDS.Table

            Try
                Dim lasTableName = From Table In Me.Model.RDS.Table
                                   Select Table.Name

                If lasTableName.ToList.Contains(Me.FactType.Id) Then
                    Return Me.FactType.getCorrespondingRDSTable
                ElseIf lasTableName.ToList.Contains(Me.JoinedORMObject.Id) Then
                    Return Me.JoinedORMObject.getCorrespondingRDSTable()
                Else
                    If Me.TypeOfJoin = pcenumRoleJoinType.EntityType Then
                        If Me.JoinsEntityType.IsObjectifyingEntityType Then
                            Return Me.JoinsEntityType.ObjectifiedFactType.getCorrespondingRDSTable()
                        Else
                            Return Me.JoinedORMObject.getCorrespondingRDSTable()
                        End If
                    Else
                        Return Me.JoinedORMObject.getCorrespondingRDSTable()
                    End If
                End If

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)

                Return Nothing
            End Try

        End Function

        ''' <summary>
        ''' Used in, for instance, Me.getResponsibleColumns() to make sure that upstream Columns for a ReassignedRole are actually for the reassigned Role.
        ''' </summary>
        ''' <param name="arTargetRole"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function getDownstreamRolePaths(ByRef arTargetRole As FBM.Role, _
                                               ByRef aarCoveredRoles As List(Of FBM.Role)) As List(Of FBM.Role)

            Try
                Dim larRolesToReturn As New List(Of FBM.Role)

                If Me.FactType.IsManyTo1BinaryFactType Or Me.FactType.IsBinaryFactType Then

                    Dim lrOtherRole As FBM.Role = Me.FactType.GetOtherRoleOfBinaryFactType(Me.Id)

                    larRolesToReturn.AddUnique(lrOtherRole)

                    Select Case lrOtherRole.JoinedORMObject.ConceptType
                        Case Is = pcenumConceptType.ValueType
                            'Nothing to add
                        Case Is = pcenumConceptType.EntityType
                            'Nothing to add
                        Case Is = pcenumConceptType.FactType
                            For Each lrRole In lrOtherRole.JoinsFactType.RoleGroup
                                larRolesToReturn.AddUnique(lrRole)
                                If lrRole Is arTargetRole Then
                                    Return larRolesToReturn
                                End If

                                If aarCoveredRoles.Contains(lrRole) Then
                                    Return larRolesToReturn
                                Else
                                    aarCoveredRoles.Add(lrRole)
                                End If
                                larRolesToReturn.AddRange(lrRole.getDownstreamRolePaths(arTargetRole, aarCoveredRoles))
                            Next
                    End Select

                ElseIf Me.FactType.HasTotalRoleConstraint Or Me.FactType.HasPartialButMultiRoleConstraint Then

                    larRolesToReturn.AddUnique(Me)

                    'For Each lrRole In Me.FactType.RoleGroup
                    aarCoveredRoles.AddUnique(Me) 'Was lrRole



                    If Me Is arTargetRole Then 'Was lrRole
                        Return larRolesToReturn
                    End If

                    Select Case Me.JoinedORMObject.ConceptType
                        Case Is = pcenumConceptType.ValueType
                            'Nothing to add
                        Case Is = pcenumConceptType.EntityType
                            'Nothing to add
                        Case Is = pcenumConceptType.FactType

                            For Each lrDownstreamRole In Me.JoinsFactType.RoleGroup 'Was lrRole
                                larRolesToReturn.AddUnique(lrDownstreamRole)
                                If lrDownstreamRole Is arTargetRole Then
                                    Return larRolesToReturn
                                End If

                                If aarCoveredRoles.Contains(lrDownstreamRole) Then
                                    Return larRolesToReturn
                                Else
                                    aarCoveredRoles.Add(lrDownstreamRole)
                                End If

                                larRolesToReturn.AddRange(lrDownstreamRole.getDownstreamRolePaths(arTargetRole, aarCoveredRoles))
                            Next
                    End Select
                    'Next

                End If

                Return larRolesToReturn

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)

                Return New List(Of FBM.Role)
            End Try

        End Function


        ''' <summary>
        ''' Returns a list of the Columns that are ultimately responsible for an ActiveRole,
        '''   or the ReferenceModeFactType.RoleGroup(0) of the JoinedORMObject (EntityType) of that Role.
        '''   Most likely will return just one table, but will return all Columns of all Tables that fit the bill.
        '''   See the TimetableBookings page of the University model.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function getResponsibleColumns() As List(Of RDS.Column)

            Try
                Dim larReturnColumns As New List(Of RDS.Column)

                'Upstream Columns that reference any downstream Roles to this Role.
                If Me.JoinedORMObject Is Nothing Then Return larReturnColumns

                Select Case Me.JoinedORMObject.ConceptType
                    Case Is = pcenumConceptType.ValueType
                        'Upstream Columns that have an ActiveRole that is Me.
                        Dim larColumn = From Table In Me.Model.RDS.Table
                                        From Column In Table.Column
                                        Where Column.ActiveRole Is Me
                                        Select Column

                        larReturnColumns.AddRange(larColumn.ToList)

                    Case Is = pcenumConceptType.EntityType
                        Dim larDownstreamActiveRoles As New List(Of FBM.Role)
                        Dim larCoveredRoles As New List(Of FBM.Role)

                        'Not sure why this is here.
                        'larCoveredRoles.Add(Me)

                        If Me.JoinsEntityType.HasSimpleReferenceScheme Then
                            If Me.JoinsEntityType.ReferenceModeFactType IsNot Nothing Then
                                larCoveredRoles.Add(Me.JoinsEntityType.ReferenceModeFactType.RoleGroup(0))
                                larCoveredRoles.Add(Me.JoinsEntityType.ReferenceModeFactType.RoleGroup(1))
                                larDownstreamActiveRoles.Add(Me.JoinsEntityType.ReferenceModeFactType.RoleGroup(1))
                            End If
                        ElseIf Me.JoinsEntityType.HasCompoundReferenceMode Then

                            larDownstreamActiveRoles.AddRange(Me.JoinsEntityType.getDownstreamActiveRoles(larCoveredRoles))
                        Else
                            'Taken care of in initial collection of upstream Columns (above).
                        End If

                        Dim larFurtherUpstreamColumns = From Table In Me.Model.RDS.Table _
                                                        From Column In Table.Column _
                                                        Where Column.Role Is Me _
                                                        Or (larDownstreamActiveRoles.Contains(Column.ActiveRole) _
                                                        And Not larCoveredRoles.Contains(Column.Role)) _
                                                        Select Column

                        For Each lrColumn In larFurtherUpstreamColumns

                            larCoveredRoles = New List(Of FBM.Role) 'So that getDownstreamRolePaths can't be circular.

                            If lrColumn.Role.getDownstreamRolePaths(Me, larCoveredRoles).Contains(Me) _
                                Or lrColumn.Role Is Me Then
                                Call larReturnColumns.AddUnique(lrColumn)
                            End If
                        Next

                    Case Is = pcenumConceptType.FactType

                        Dim larDownstreamActiveRoles As New List(Of FBM.Role)
                        Dim larCoveredRoles As New List(Of FBM.Role)

                        If Not Me.JoinsFactType.IsMDAModelElement Then

                            'Dim lrTable As RDS.Table = Me.JoinsFactType.getCorrespondingRDSTable

                            'larReturnColumns.Add(lrTable.Column.Find(Function(x) x.Role Is Me))

                            'Returns all Roles joined ObjectifiedFactTypes and their Roles' JoinedORMObjects (recursively).
                            larDownstreamActiveRoles = Me.getDownstreamRoleActiveRoles(larCoveredRoles)

                            Dim larFurtherUpstreamColumns = From Table In Me.Model.RDS.Table
                                                            From Column In Table.Column
                                                            Where larDownstreamActiveRoles.Contains(Column.ActiveRole) _
                                                            And Column.Role.getDownstreamRolePaths(Me, larCoveredRoles).Contains(Me) _
                                                            And Not larCoveredRoles.Contains(Column.Role)
                                                            Select Column

                            Call larReturnColumns.AddRange(larFurtherUpstreamColumns.ToList)
                        End If


                End Select

                Return larReturnColumns

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)

                Return New List(Of RDS.Column)
            End Try

        End Function

        ''' <summary>
        ''' Returns a list of the Tables that are ultimately responsible for an ActiveRole of a Column. Most likely will return just one table.
        '''   See the TimetableBookings page of the University model.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function getResponsibleTables() As List(Of RDS.Table)

            Dim larTable = From Table In Me.Model.RDS.Table
                           From Column In Table.Column
                           Where Me.FactType.RoleGroup.Contains(Column.ActiveRole)
                           Select Table

            Return larTable.ToList

        End Function


        ''' <summary>
        ''' Returns the list of Attribute Names for a Role that references a Fact Type (recursive).
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function getAttributeNames() As List(Of String)

            Dim lasAttributeName As New List(Of String)

            Try
                If Me.JoinedORMObject.ConceptType = pcenumConceptType.FactType Then

                    Dim lrFactType As FBM.FactType = Me.JoinedORMObject

                    For Each lrRole In lrFactType.RoleGroup
                        Select Case lrRole.JoinedORMObject.ConceptType
                            Case Is = pcenumConceptType.ValueType,
                                      pcenumConceptType.EntityType
                                lasAttributeName.Add(lrRole.GetAttributeName)
                            Case Is = pcenumConceptType.FactType
                                lasAttributeName.AddRange(lrRole.getAttributeNames)
                        End Select
                    Next

                Else
                    Throw New Exception("Tried to get Attribute names (plural) for a Role that does not reference a Fact Type.")
                End If

                Return lasAttributeName

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)

                Return New List(Of String)
            End Try

        End Function

        ''' <summary>
        ''' Used, for example, when reassigning a Role to another ModelObject.
        ''' PRECONDITION: Role is part of the RoleGroup of a FactType with a TotalInternalUniquenessConstraint or a PartialButMultiInternalUniquenessConstraint
        ''' </summary>
        ''' <param name="aarCoveredRoles">Used for recursion. Start empty.</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function getDownstreamRoleActiveRoles(ByRef aarCoveredRoles As List(Of FBM.Role)) As List(Of FBM.Role)

            Try
                Dim larRolesToReturn As New List(Of FBM.Role)

                If (Me.FactType.IsManyTo1BinaryFactType Or Me.FactType.Is1To1BinaryFactType) And Me.HasInternalUniquenessConstraint Then

                    Dim lrOtherRoleInFactType = Me.FactType.GetOtherRoleOfBinaryFactType(Me.Id)

                    Select Case lrOtherRoleInFactType.JoinedORMObject.ConceptType
                        Case Is = pcenumConceptType.FactType
                            larRolesToReturn.AddRange(Me.FactType.GetOtherRoleOfBinaryFactType(Me.Id).getDownstreamRoleActiveRoles(aarCoveredRoles))
                        Case Is = pcenumConceptType.ValueType
                            larRolesToReturn.Add(lrOtherRoleInFactType)

                        Case Is = pcenumConceptType.EntityType

                            If lrOtherRoleInFactType.JoinsEntityType.HasSimpleReferenceScheme Then

                                aarCoveredRoles.AddUnique(lrOtherRoleInFactType.JoinsEntityType.ReferenceModeFactType.RoleGroup(0))
                                aarCoveredRoles.AddUnique(lrOtherRoleInFactType.JoinsEntityType.ReferenceModeFactType.RoleGroup(1))

                                larRolesToReturn.Add(lrOtherRoleInFactType.JoinsEntityType.ReferenceModeFactType.RoleGroup(1))

                            ElseIf Me.JoinsEntityType.HasCompoundReferenceMode Then
                                '20210824-VM-Was me.JoinsEntityType, which is wrong...want what is checked in Select above.
                                larRolesToReturn.AddRange(lrOtherRoleInFactType.JoinsEntityType.getDownstreamActiveRoles(aarCoveredRoles))
                            Else
                                larRolesToReturn.AddUnique(lrOtherRoleInFactType)
                            End If

                    End Select

                ElseIf Me.FactType.HasTotalRoleConstraint Or Me.FactType.HasPartialButMultiRoleConstraint _
                    Or Me.JoinedORMObject.ConceptType = pcenumConceptType.FactType Then

                    Select Case Me.JoinedORMObject.ConceptType
                        Case Is = pcenumConceptType.ValueType
                            larRolesToReturn.Add(Me)

                        Case Is = pcenumConceptType.EntityType

                            If Me.JoinsEntityType.HasSimpleReferenceScheme Then

                                Dim lrSupertype As FBM.ModelObject = Me.JoinsEntityType.GetTopmostNonAbsorbedSupertype
                                If lrSupertype IsNot Me.JoinsEntityType Then
                                    aarCoveredRoles.AddUnique(CType(Me.JoinsEntityType.GetTopmostNonAbsorbedSupertype, FBM.EntityType).ReferenceModeFactType.RoleGroup(0))
                                    aarCoveredRoles.AddUnique(CType(Me.JoinsEntityType.GetTopmostNonAbsorbedSupertype, FBM.EntityType).ReferenceModeFactType.RoleGroup(1))

                                    larRolesToReturn.Add(CType(Me.JoinsEntityType.GetTopmostNonAbsorbedSupertype, FBM.EntityType).ReferenceModeFactType.RoleGroup(1))
                                Else
                                    If Me.JoinsEntityType.SubtypeRelationship.Count > 0 Then
                                        lrSupertype = Me.JoinsEntityType.SubtypeRelationship.First.parentEntityType
                                        aarCoveredRoles.AddUnique(CType(lrSupertype, FBM.EntityType).ReferenceModeFactType.RoleGroup(0))
                                        aarCoveredRoles.AddUnique(CType(lrSupertype, FBM.EntityType).ReferenceModeFactType.RoleGroup(1))

                                        larRolesToReturn.Add(CType(lrSupertype, FBM.EntityType).ReferenceModeFactType.RoleGroup(1))
                                    Else
                                        aarCoveredRoles.AddUnique(Me.JoinsEntityType.ReferenceModeFactType.RoleGroup(0))
                                        aarCoveredRoles.AddUnique(Me.JoinsEntityType.ReferenceModeFactType.RoleGroup(1))

                                        larRolesToReturn.Add(Me.JoinsEntityType.ReferenceModeFactType.RoleGroup(1))
                                    End If

                                End If

                            ElseIf Me.JoinsEntityType.HasCompoundReferenceMode Then

                                larRolesToReturn.AddRange(Me.JoinsEntityType.getDownstreamActiveRoles(aarCoveredRoles))
                            Else
                                larRolesToReturn.AddUnique(Me)
                            End If


                        Case Is = pcenumConceptType.FactType

                            For Each lrRole In Me.JoinsFactType.RoleGroup

                                If aarCoveredRoles.Contains(Me) Then
                                    Return larRolesToReturn
                                End If

                                aarCoveredRoles.Add(lrRole)
                                larRolesToReturn.AddRange(lrRole.getDownstreamRoleActiveRoles(aarCoveredRoles))
                            Next

                    End Select

                End If

                Return larRolesToReturn

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)

                Return New List(Of FBM.Role)
            End Try

        End Function

        ''' <summary>
        ''' Returns the list of Attribute Names for a Role that references a Fact Type (recursive).
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function getColumns(ByRef arTable As RDS.Table,
                                   ByRef arResponsibleRole As FBM.Role) As List(Of RDS.Column)

            Dim larColumn As New List(Of RDS.Column)
            Dim lsColumnName As String = ""

            Try

                Dim lrActiveRole As FBM.Role
                If Me.JoinedORMObject.ConceptType = pcenumConceptType.FactType Then
#Region "Joins FactType"
                    Dim lrFactType As FBM.FactType = Me.JoinedORMObject

                    Dim larPrimaryKeyRoles As New List(Of FBM.Role)

                    If lrFactType.InternalUniquenessConstraint.Count = 1 Then
                        larPrimaryKeyRoles = lrFactType.InternalUniquenessConstraint(0).Role

                    ElseIf lrFactType.InternalUniquenessConstraint.Count > 1 Then
                        'ToDo: Should be the InternalUniquenessConstraint which is a PreferedReferenceScheme for the FactType.
                        'Use the lowest levelNr at this stage. 
                        Dim lrFirstInternalUniquenessConstraint As FBM.RoleConstraint
                        lrFirstInternalUniquenessConstraint = lrFactType.InternalUniquenessConstraint.Find(Function(x) x.LevelNr = 1)
                    End If

                    For Each lrRole In lrFactType.RoleGroup.FindAll(Function(x) larPrimaryKeyRoles.contains(x))
                        Select Case lrRole.JoinedORMObject.ConceptType
                            Case Is = pcenumConceptType.ValueType,
                                      pcenumConceptType.EntityType

                                lsColumnName = lrRole.JoinedORMObject.Id
                                lsColumnName = Viev.Strings.MakeCapCamelCase(Viev.Strings.RemoveWhiteSpace(lsColumnName))

                                lrActiveRole = New FBM.Role
                                lrActiveRole = lrRole

                                Select Case lrRole.JoinedORMObject.ConceptType
                                    Case Is = pcenumConceptType.EntityType

                                        If lrRole.JoinsEntityType.HasSimpleReferenceScheme Then
                                            lrActiveRole = CType(lrRole.JoinsEntityType.GetTopmostSupertype, FBM.EntityType).ReferenceModeFactType.RoleGroup(1)
                                            lsColumnName = lrActiveRole.JoinedORMObject.Name
                                        End If
                                    Case Else
                                End Select

                                lsColumnName = Viev.Strings.RemoveUnderscores(lsColumnName)

                                Dim lrColumn As RDS.Column = New RDS.Column(arTable, lsColumnName, arResponsibleRole, lrActiveRole, True)
                                larColumn.Add(lrColumn)

                            Case Is = pcenumConceptType.FactType
                                'Recursive
                                larColumn.AddRange(lrRole.getColumns(arTable, arResponsibleRole))
                        End Select
                    Next
#End Region
                ElseIf Me.JoinedORMObject.ConceptType = pcenumConceptType.EntityType Then '
                    'Joins EntityType
                    If Me.FactType.isRDSTable Then
                        Dim lrEntityType As FBM.EntityType = Me.JoinsEntityType
                        If lrEntityType.HasSimpleReferenceScheme Then
                            larColumn.Add(Me.GetCorrespondingFactTypeColumn(arTable, arResponsibleRole))
                        Else
                            Call lrEntityType.getCompoundReferenceSchemeColumns(arTable, Me, larColumn)
                        End If
                    Else
                        larColumn.Add(Me.GetCorrespondingUnaryOrBinaryFactTypeColumn(arTable))
                    End If
                ElseIf Me.JoinedORMObject.ConceptType = pcenumConceptType.ValueType Then
                    'Joins ValueType
                    If Me.FactType.isRDSTable Then
                        larColumn.Add(Me.GetCorrespondingFactTypeColumn(arTable, arResponsibleRole))
                    Else
                        larColumn.Add(Me.GetCorrespondingUnaryOrBinaryFactTypeColumn(arTable))
                    End If
                End If

                Return larColumn

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)

                Return New List(Of RDS.Column)
            End Try

        End Function

        ''' <summary>
        ''' Returns True if the Role 'owns' an InternalUniquenessConstraint, else returns False            
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function HasInternalUniquenessConstraint() As Boolean
            '-------------------------------------------------------------------------------
            'Used when Automatically generating Tables/Entities/Classes
            'Used predominantly to see which Entity of a binary FactType
            'owns the uniqueness constraint (Role can't be part of 1:1 binary fact type)
            '-------------------------------------------------------------------------------
            
            HasInternalUniquenessConstraint = False

            Dim larRoleConstraintRole = From RoleConstraint In Me.Model.RoleConstraint
                                        From RoleConstraintRole In RoleConstraint.RoleConstraintRole _
                                        Where RoleConstraint.RoleConstraintType = pcenumRoleConstraintType.InternalUniquenessConstraint _
                                        And RoleConstraintRole.Role.Id = Me.Id _
                                        Select RoleConstraintRole

            Return larRoleConstraintRole.Count > 0

            'Dim lrRoleConstraintRole As FBM.RoleConstraintRole
            'For Each lrRoleConstraintRole In Me.RoleConstraintRole
            '    If lrRoleConstraintRole.RoleConstraint.RoleConstraintType = pcenumRoleConstraintType.InternalUniquenessConstraint Then
            '        HasInternalUniquenessConstraint = True
            '        Exit Function
            '    End If
            'Next

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

        ''' <summary>
        ''' Returns True if the Role represents a PK on the (ERD) Entity, and because the Role is part of the FactType that represents the SimpleReferenceScheme of the associated ModelObject (EntityType, FactType).
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function IsERDSimpleReferenceSchemePKRole() As Boolean

            Try


                If Me.FactType.IsPreferredReferenceMode _
                    And Me.RoleConstraintRole.FindAll(Function(x) x.RoleConstraint.IsPreferredIdentifier = False).Count > 0 Then
                    Return True
                Else
                    Return False
                End If

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Function

        ''' <summary>
        ''' Returns True if the Role is part of the PK of an ObjectifiedFactType.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function IsERDPKRoleOfObjectifiedFactType() As Boolean

            If Me.FactType.IsObjectified _
                And Me.RoleConstraintRole.FindAll(Function(x) x.RoleConstraint.IsPreferredIdentifier = True).Count > 0 Then
                Return True
            ElseIf Me.FactType.IsObjectified _
                And Me.FactType.InternalUniquenessConstraint.Count = 1 Then
                Return True
            Else
                Return False
            End If

        End Function

        Public Overridable Function IsERDPropertyRole() As Boolean

            IsERDPropertyRole = False

            Select Case Me.TypeOfJoin
                Case Is = pcenumRoleJoinType.EntityType
                    If Me.HasInternalUniquenessConstraint() And Me.FactType.Is1To1BinaryFactType Then
                        'Rule 4
                        If IsSomething(Me.JoinsEntityType.ReferenceModeFactType) Then
                            If Me.FactType.Id = Me.JoinsEntityType.ReferenceModeFactType.Id Then
                                '---------------------------------------------------
                                'Is Role on ReferenceModeFactType
                                '---------------------------------------------------
                                If Me.JoinsEntityType.ReferenceModeRoleConstraint.Role(0).Id = Me.Id Then
                                    IsERDPropertyRole = False
                                Else
                                    IsERDPropertyRole = True
                                End If
                            Else
                                IsERDPropertyRole = True
                            End If
                        Else
                            IsERDPropertyRole = True
                        End If
                    ElseIf Me.FactType.HasTotalRoleConstraint Then
                        'Rule 3 for ERDs but different for PGS
                        IsERDPropertyRole = True
                    ElseIf Me.FactType.IsObjectified Then
                        IsERDPropertyRole = True
                    ElseIf Me.HasInternalUniquenessConstraint() And Me.FactType.IsBinaryFactType Then
                        'Rule 2
                        IsERDPropertyRole = True
                    ElseIf Me.FactType.Arity = 1 Then
                        'Is Unary Role, so must be a Property
                        IsERDPropertyRole = True
                    End If
                Case Is = pcenumRoleJoinType.ValueType 'ValueType                    
                    If Me.FactType.IsObjectified Or Me.FactType.HasPartialButMultiRoleConstraint Then
                        IsERDPropertyRole = True
                    Else
                        IsERDPropertyRole = False
                    End If
                Case Is = pcenumRoleJoinType.FactType
                    'ObjectifiedFactType        
                    IsERDPropertyRole = True
            End Select

        End Function


        ''' <summary>
        ''' True if the Role belongs to a FactType that has a Role that is part of a CompoundReferenceScheme for an EntityType
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function isForFactTypeOfCompoundReferenceScheme() As Boolean

            If Me.JoinedORMObject.ConceptType = pcenumConceptType.EntityType Then

                Dim lrEntityType As FBM.EntityType = Me.JoinedORMObject

                If lrEntityType.HasCompoundReferenceMode Then

                    For Each lrRoleConstraintRole In lrEntityType.ReferenceModeRoleConstraint.RoleConstraintRole
                        If lrRoleConstraintRole.Role.FactType.Id = Me.FactType.Id Then
                            Return True
                        End If
                    Next
                Else
                    Return False
                End If
            Else
                Return False
            End If

            Return False

        End Function

        ''' <summary>
        ''' TRUE if the Role is an Active Role (or should be) of a Column, where the Column's ResponsibleRole is in a different FactType that references (eventually) the FactType of this Role.
        '''   See the TimetableBookings table of the University model.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function hasResponsibleColumns() As Boolean

            Return  Me.getResponsibleColumns.Count > 0 

        End Function

        Public Function IsPartOfNestedFactType() As Integer
            '---------------------------------------------------------------
            'Returns true if the FactType/RoleGroup to which RoleId belongs
            'is a Nested_Fact type else returns false
            '---------------------------------------------------------------

            IsPartOfNestedFactType = False

            If Me.FactType.IsObjectified Then
                IsPartOfNestedFactType = True
            End If

        End Function

        Public Function isPartOfFactTypesPreferredReferenceScheme() As Boolean

            Try
                If Me.FactType.InternalUniquenessConstraint.Count = 0 Then
                    Return False

                ElseIf (Me.FactType.IsManyTo1BinaryFactType Or Me.FactType.Is1To1BinaryFactType) And _
                    Not Me.FactType.IsObjectified Then

                    Return False

                ElseIf Me.FactType.isRDSTable _
                    And Me.FactType.InternalUniquenessConstraint.Count = 1 _
                    And Me.HasInternalUniquenessConstraint Then

                    Return True

                ElseIf Me.FactType.isRDSTable _
                    And Me.FactType.getPreferredInternalUniquenessConstraint.Role.Contains(Me) Then

                    Return True

                End If

                Return False

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)

                Return False
            End Try

        End Function


        ''' <summary>
        ''' PRECONDITION: Only use on Roles that are the ResponsibleRole for a Column.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function isRDSForeignKeyRole() As Boolean

            Try
                If Me.FactType.isRDSTable Then
                    'ToDo:
                    Return False
                Else
                    If Me.FactType.IsManyTo1BinaryFactType Or Me.FactType.Is1To1BinaryFactType Then
                        If Me.HasInternalUniquenessConstraint Then
                            Return True
                        End If
                    End If
                End If

                Return False

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)

                Return False
            End Try

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

        Public Overrides Sub makeDirty()
            Me.FactType.isDirty = True
            Me.isDirty = True
        End Sub

        Public Sub ModifyValueConstraint(ByVal asOldValueConstraint As String, asNewValueConstraint As String)

            Try

                '-------------------------------------------------------------
                'Update ValueConstraintValue of the particular
                '  Concept/Symbol/Value within the 'value_constraint'
                '  for the ValueType of this ValueTypeInstance
                '-------------------------------------------------------------
                If Me._ValueConstraintList.IndexOf(asOldValueConstraint) >= 0 Then

                    Me.ValueConstraint.Item(Me.ValueConstraint.IndexOf(asOldValueConstraint)) = asNewValueConstraint

                    'VM-20180401-Not sure what this does.
                    'Dim lrConcept As New FBM.Concept(aoChangedPropertyItem.OldValue)
                    'lrConcept = Me.ValueType._ValueConstraint.Find(AddressOf lrConcept.Equals)
                    'If IsSomething(lrConcept) Then
                    '    lrConcept.Symbol = aoChangedPropertyItem.ChangedItem.Value.ToString
                    'End If

                    RaiseEvent ValueConstraintModified(asOldValueConstraint, asNewValueConstraint)

                ElseIf Not Me.ValueConstraint.Contains(asNewValueConstraint) Then
                    Me.AddValueConstraint(asNewValueConstraint)
                End If

                Call Me.RoleConstraintRoleValueConstraint.ModifyValueConstraint(asOldValueConstraint, asNewValueConstraint)

                Me.isDirty = True
                Me.Model.MakeDirty(False, False)

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub



        ''' <summary>
        ''' Reassigns the joined ModelObject of the Role.
        ''' NB The name of the FactType of the Role must be modified to reflect the new relation.
        ''' </summary>
        ''' <param name="arNewJoinedModelObject"></param>
        ''' <remarks></remarks>
        Public Sub ReassignJoinedModelObject(ByRef arNewJoinedModelObject As FBM.ModelObject,
                                             Optional ByVal abBroadcastInterfaceEvent As Boolean = True,
                                             Optional arConceptInstance As FBM.ConceptInstance = Nothing)

            Try
                '==========================================================================================
                'PSEUDOCODE
                '  * Get the originally joined ModelObject
                '  * Get thr originally joined Table
                '  * Get the Column that has the Role as the Responsible Role
                '  * 

                '------------------------------------------------------------

                'CodeSafe
                If arNewJoinedModelObject Is Me.FactType Then Exit Sub

                'Get the originally joined ModelObject
                Dim lrOriginallyJoinedModelObject As FBM.ModelObject = Me.JoinedORMObject

                Dim lrOriginallyJoinedTable As RDS.Table = Nothing
                Dim larOriginalColumn As New List(Of RDS.Column)

                If lrOriginallyJoinedModelObject IsNot Nothing Then
                    lrOriginallyJoinedTable = lrOriginallyJoinedModelObject.getCorrespondingRDSTable
                    If lrOriginallyJoinedTable IsNot Nothing Then
                        larOriginalColumn = lrOriginallyJoinedTable.Column.FindAll(Function(x) x.Role Is Me)
                    End If
                End If


                If arNewJoinedModelObject Is Me.JoinedORMObject Then
                    Exit Sub
                Else
                    If Me.JoinedORMObject IsNot Nothing Then
                        Select Case Me.JoinedORMObject.GetType
                            Case Is = GetType(FBM.ValueType)
                                Dim lrValueType As FBM.ValueType = Me.JoinedORMObject

                                If lrValueType.isReferenceModeValueType Then
                                    Dim lrEntityType As FBM.EntityType = lrValueType.getReferringEntityType
                                    If Not arNewJoinedModelObject.GetType = GetType(FBM.ValueType) Then
                                        lrEntityType.RemoveSimpleReferenceScheme()
                                    Else
                                        lrEntityType.ReferenceModeValueType = arNewJoinedModelObject
                                    End If

                                End If

                        End Select
                    End If

                    Me.JoinedORMObject = arNewJoinedModelObject

                    If lrOriginallyJoinedTable IsNot Nothing Then

                        '==============================================================================================
                        'Simple case first, for where Role is Many on ManyToOne FactType linked
                        '  to a ModelObject that is a table. I.e. Linked to an EntityType or an Objectified Fact Type
                        '===================================
#Region "RDS Processing"
                        If Me.HasInternalUniquenessConstraint And (Me.FactType.IsManyTo1BinaryFactType Or Me.FactType.Is1To1BinaryFactType) Then
                            Select Case arNewJoinedModelObject.GetType
                                Case Is = GetType(FBM.ValueType)

                                    Dim lrOriginalJoinedORMObject = Me.JoinedORMObject

                                    Me.JoinedORMObject = arNewJoinedModelObject
                                    Me.makeDirty()

                                    Dim lrOtherRole As FBM.Role = Me.FactType.GetOtherRoleOfBinaryFactType(Me.Id)
                                    Dim lrTable As RDS.Table = lrOtherRole.JoinedORMObject.getCorrespondingRDSTable
                                    Dim larColumn = From Column In lrTable.Column
                                                    Where Column.ActiveRole Is Me
                                                    Select Column

                                    Dim lrColumn As RDS.Column = larColumn.First

                                    Dim lsColumnName As String = lrTable.createUniqueColumnName(lrColumn, Me.JoinedORMObject.Id, 0)

                                    Call lrColumn.setName(lsColumnName)

                                    '-----------------------------------------------------------------------------------------------
                                    'Pages
                                    Dim larPage = From Page In Me.Model.Page
                                                  From FactTypeInstance In Page.FactTypeInstance
                                                  Where FactTypeInstance.Id = Me.FactType.Id
                                                  Select Page

                                    For Each lrPage In larPage
                                        Dim asNewJoinedModelObjectId As String = arNewJoinedModelObject.Id
                                        If lrPage.ValueTypeInstance.FindAll(Function(x) x.Id = asNewJoinedModelObjectId).Count = 0 Then
                                            lrPage.DropValueTypeAtPoint(arNewJoinedModelObject, New PointF(10, 10), True)
                                        End If
                                    Next

                                Case Else
                                    '=========================================================
                                    'PSEUDOCODE
                                    '  * Create the new Column on the newly joined Table
                                    '  * Get the Original Relation
                                    '  * Remove the OriginalRelation from the Original Table                        
                                    '  * Reassign the Role to the newly joined ModelObject
                                    '  * Create the New Relation and add to New Table
                                    '  * Remove the Original Column
                                    '---------------------------------------------------------

                                    Dim lrNewTable = arNewJoinedModelObject.getCorrespondingRDSTable()

                                    Dim larCoveredRoles As New List(Of FBM.Role)
                                    Dim larDownstreamActiveRoles = Me.getDownstreamRoleActiveRoles(larCoveredRoles) 'Returns all Roles joined ObjectifiedFactTypes and their Roles' JoinedORMObjects (recursively).

                                    'Create the new Column/s in the newly joined Table
                                    Dim lrNewColumn As New RDS.Column
                                    For Each lrActiveRole In larDownstreamActiveRoles
                                        'Dim lrOriginalColumn = larOriginalColumn.Find(Function(x) x.ActiveRole Is lrActiveRole)
                                        lrNewColumn = New RDS.Column(lrNewTable,
                                                             lrActiveRole.JoinedORMObject.Id,
                                                             Me,
                                                             lrActiveRole,
                                                             Me.Mandatory)
                                        If lrNewTable.Column.Find(Function(x) x.Role.Id = Me.Id) Is Nothing Then
                                            lrNewTable.addColumn(lrNewColumn)
                                        End If
                                    Next

                                    'Remove the Original Relation
                                    If larOriginalColumn(0).Relation.Count > 0 Then
                                        Dim lrRelation = larOriginalColumn(0).Relation(0)
                                        Call Me.Model.RDS.removeRelation(lrRelation)
                                    End If

                                    'Reassign the Role
                                    Me.JoinedORMObject = arNewJoinedModelObject

                                    Me.makeDirty()


                                    'Create a Relation for the reassigned Role
                                    Call Me.Model.generateRelationForReassignedRole(Me)

                                    'Remove the orginal Column from the Originally Joined Table
                                    For Each lrColumn In larOriginalColumn
                                        Call lrOriginallyJoinedTable.removeColumn(lrColumn)
                                    Next
                            End Select

                        Else
                            '==========================================================================
                            'RDS - NB See below for RDS Processing propper. Must get the responsible Columns before the move.                    
                            Dim larColumn As List(Of RDS.Column)

                            larColumn = Me.getResponsibleColumns()
                            '======================================

                            Me.JoinedORMObject = arNewJoinedModelObject

                            Me.isDirty = True

                            If Me.FactType.IsObjectified Then
                                'Modify the JoinedORMObject of the appropriate LinkFactType
                                Dim larLinkFactTypeRole = From FactType In Me.Model.FactType
                                                          Where FactType.IsLinkFactType = True _
                                                  And FactType.LinkFactTypeRole Is Me
                                                          Select FactType.RoleGroup(1)

                                Dim lrLinkFactTypeRole As FBM.Role = larLinkFactTypeRole.First

                                Call lrLinkFactTypeRole.ReassignJoinedModelObject(arNewJoinedModelObject)

                                'Select Case arNewJoinedModelObject.ConceptType
                                '    Case Is = pcenumConceptType.EntityType
                                '        lrLinkFactTypeRole.TypeOfJoin = pcenumRoleJoinType.EntityType
                                '        lrLinkFactTypeRole.JoinsEntityType = lrLinkFactTypeRole.JoinedORMObject
                                '        lrLinkFactTypeRole.JoinsValueType = Nothing
                                '        lrLinkFactTypeRole.JoinsFactType = Nothing
                                '    Case Is = pcenumConceptType.ValueType
                                '        lrLinkFactTypeRole.TypeOfJoin = pcenumRoleJoinType.ValueType
                                '        lrLinkFactTypeRole.JoinsValueType = lrLinkFactTypeRole.JoinedORMObject
                                '        lrLinkFactTypeRole.JoinsEntityType = Nothing
                                '        lrLinkFactTypeRole.JoinsFactType = Nothing
                                '    Case Is = pcenumConceptType.FactType
                                '        lrLinkFactTypeRole.TypeOfJoin = pcenumRoleJoinType.FactType
                                '        lrLinkFactTypeRole.JoinsFactType = lrLinkFactTypeRole.JoinedORMObject
                                '        lrLinkFactTypeRole.JoinsEntityType = Nothing
                                '        lrLinkFactTypeRole.JoinsValueType = Nothing
                                'End Select
                            End If

                            If My.Settings.UseClientServer And My.Settings.InitialiseClient And abBroadcastInterfaceEvent Then
                                Call prDuplexServiceClient.BroadcastToDuplexService(Viev.FBM.Interface.pcenumBroadcastType.RoleReassignJoinedModelObject, Me, Nothing)
                            End If

                            '==========================================================================
                            'RDS - NB See Above. Must get the responsible Columns before the move.                    

                            '------------------------------------------------------------
                            'Relations - Remove existing Relations
                            If lrOriginallyJoinedModelObject IsNot Nothing Then
                                Select Case lrOriginallyJoinedModelObject.ConceptType
                                    Case Is = pcenumConceptType.EntityType,
                                      pcenumConceptType.FactType

                                        Dim larOriginTable = From Column In larColumn
                                                             Select Column.Table Distinct

                                        For Each lrOriginTable In larOriginTable
                                            Dim larOriginColumn = From Column In lrOriginTable.Column
                                                                  Where larColumn.Contains(Column)
                                                                  Select Column Distinct


                                            Dim larRelationToRemove As New List(Of RDS.Relation)
                                            larRelationToRemove = Me.Model.RDS.getRelationsByOriginTableOriginColumns(lrOriginTable, larOriginColumn.ToList)

                                            For Each lrRelation In larRelationToRemove.ToArray

                                                For Each lrColumn In lrRelation.OriginColumns
                                                    lrColumn.Relation.Remove(lrRelation)
                                                Next

                                                Call Me.Model.RDS.removeRelation(lrRelation)
                                            Next
                                        Next

                                End Select

                                'Remove the existing Column/s
                                For Each lrColumn In larColumn.ToList
                                    Me.Model.RDS.Table.Find(Function(x) x.Name = lrColumn.Table.Name).removeColumn(lrColumn)
                                Next

                            End If

                            '=======================================================================================================
                            'Create the New Columns
                            Dim larTable = From lrColumn In larColumn
                                           Select lrColumn.Table Distinct


                            Dim lrNewColumn As RDS.Column
                            For Each lrTable In larTable

                                Dim lrResponsibleRole As FBM.Role = larColumn.Find(Function(x) x.Table.Name = lrTable.Name).Role

                                Select Case Me.JoinedORMObject.ConceptType
                                    Case Is = pcenumConceptType.ValueType
                                        If Me.FactType.IsUnaryFactType Then
                                            Throw New Exception("Can't have a UnaryFactType against a ValueType.")

                                        ElseIf Me.FactType.IsManyTo1BinaryFactType Or Me.FactType.Is1To1BinaryFactType Then
                                            lrNewColumn = lrResponsibleRole.GetCorrespondingUnaryOrBinaryFactTypeColumn(lrTable)
                                            lrTable.addColumn(lrNewColumn)

                                        ElseIf Me.FactType.HasTotalRoleConstraint Or Me.FactType.HasPartialButMultiRoleConstraint Then
                                            lrNewColumn = Me.GetCorrespondingFactTypeColumn(lrTable)
                                            lrNewColumn.Role = lrResponsibleRole

                                            If lrResponsibleRole.isPartOfFactTypesPreferredReferenceScheme Then
                                                lrNewColumn.IsMandatory = True
                                                '20210505-VM-No longer needed. IsPartOfPrimaryKey uses Table Indexes to determine.
                                                'lrNewColumn.ContributesToPrimaryKey = True
                                            End If

                                            Call lrTable.addColumn(lrNewColumn)
                                        Else
                                            Throw New Exception("Don't know how we got here")
                                        End If
                                    Case Is = pcenumConceptType.EntityType

                                        Dim lrEntityType As FBM.EntityType = Me.JoinedORMObject

                                        If lrResponsibleRole.FactType.IsUnaryFactType Then

                                            lrNewColumn = lrResponsibleRole.GetCorrespondingUnaryOrBinaryFactTypeColumn(lrTable)
                                            lrTable.addColumn(lrNewColumn)

                                        ElseIf lrResponsibleRole.FactType.IsManyTo1BinaryFactType _
                                        Or lrResponsibleRole.FactType.Is1To1BinaryFactType Then

                                            If lrResponsibleRole.FactType.RoleGroup.Contains(Me) Then
                                                Dim lrRoleConstraint = Me.FactType.InternalUniquenessConstraint.Find(Function(x) x.RoleConstraintRole(0).Role.Id = lrResponsibleRole.Id)
                                                Call Me.Model.generateAttributesForRoleConstraint(lrRoleConstraint)

                                                '20180805-Removed because created new Column on the wrong Table for 1:1 BinaryFactType reassigned Role.
                                                'lrNewColumn = lrResponsibleRole.GetCorrespondingUnaryOrBinaryFactTypeColumn(lrTable)
                                            Else
                                                lrNewColumn = Me.GetCorrespondingFactTypeColumn(lrTable)
                                                lrNewColumn.Role = lrResponsibleRole

                                                '20210505-VM-No longer needed. IsPartOfPrimaryKey uses Table Indexes to determine.
                                                'lrNewColumn.ContributesToPrimaryKey = lrResponsibleRole.isPartOfFactTypesPreferredReferenceScheme

                                                lrTable.addColumn(lrNewColumn)
                                            End If

                                        ElseIf lrResponsibleRole.FactType.HasTotalRoleConstraint Or lrResponsibleRole.FactType.HasPartialButMultiRoleConstraint Then

                                            If lrEntityType.HasCompoundReferenceMode Then
                                                Dim larNewColumn As New List(Of RDS.Column)

                                                Call lrEntityType.getCompoundReferenceSchemeColumns(lrTable, lrResponsibleRole, larColumn)

                                                For Each lrColumn In larColumn
                                                    lrTable.addColumn(lrColumn)
                                                Next
                                            Else
                                                lrNewColumn = Me.GetCorrespondingFactTypeColumn(lrTable)
                                                lrNewColumn.Role = lrResponsibleRole

                                                If Me.isPartOfFactTypesPreferredReferenceScheme Then
                                                    lrNewColumn.IsMandatory = True
                                                    '20210505-VM-No longer needed. IsPartOfPrimaryKey uses Table Indexes to determine.
                                                    'lrNewColumn.ContributesToPrimaryKey = True
                                                    Dim lrPrimaryIndex As RDS.Index = lrTable.getPrimaryKeyIndex
                                                    Call lrPrimaryIndex.addColumn(lrNewColumn)
                                                End If

                                                Call lrTable.addColumn(lrNewColumn)
                                            End If

                                        ElseIf lrResponsibleRole.FactType.Is1To1BinaryFactType And lrResponsibleRole Is Me Then

                                            Dim lrRoleConstraint = Me.FactType.InternalUniquenessConstraint.Find(Function(x) x.RoleConstraintRole(0).Role.Id = Me.Id)
                                            Call Me.Model.generateAttributesForRoleConstraint(lrRoleConstraint)

                                        End If

                                    Case Is = pcenumConceptType.FactType

                                        Dim larNewColumn As New List(Of RDS.Column)

                                        larNewColumn = Me.getColumns(lrTable, lrResponsibleRole)

                                        For Each lrNewColumn In larNewColumn

                                            '20210505-VM-No longer needed. IsPartOfPrimaryKey uses Table Indexes to determine.
                                            'lrNewColumn.ContributesToPrimaryKey = lrResponsibleRole.isPartOfFactTypesPreferredReferenceScheme
                                            lrNewColumn.IsMandatory = lrResponsibleRole.Mandatory Or lrNewColumn.isPartOfPrimaryKey '20210505-VM-Was ContributesToPrimaryKey

                                            lrTable.addColumn(lrNewColumn)
                                        Next

                                End Select 'Me.JoinedORMObject.ConceptType

                            Next 'Table

                            '===End-Create new Columns===========================================================================

                            'RDS-Relationships
                            If Not Me.FactType.IsLinkFactType Then
                                Call Me.Model.generateRelationForReassignedRole(Me)
                            End If

                            '------------------------------------------------------------------------
                            'Rename the FactType of the Role (me) that has been modified.
                            Dim lsNewName As String = Me.FactType.MakeNameFromFactTypeReadings()
                            Dim lsOldName As String = Me.FactType.Id
                            If lsNewName <> lsOldName Then
                                If Me.FactType.getRolesReferencingNothingCount = 0 Then
                                    Call Me.FactType.setName(lsNewName, True)
                                End If
                            End If

                            Me.Model.MakeDirty(False, False)

                        End If 'Whether a Role in a ManyToOne Binary FactType or is a Role in a Ternary/Greater FactType.
                    ElseIf Me.FactType.HasPartialButMultiRoleConstraint Then
                        '==========================================================================
                        'RDS - NB See below for RDS Processing propper. Must get the responsible Columns before the move.                    

#Region "Create Column/s and Relation if need be"
                        '=======================================================================================================
                        'Create the New Columns

                        Dim lrTable As RDS.Table = Me.FactType.getCorrespondingRDSTable
                        Dim lrColumn As RDS.Column

                        If lrTable IsNot Nothing Then

                            Select Case Me.JoinedORMObject.ConceptType
                                Case Is = pcenumConceptType.ValueType

                                    If Not lrTable.Column.Exists(Function(x) x.Role.Id = Me.Id) Then
                                        'There is no Column in the Table for the Role.
                                        lrColumn = Me.GetCorrespondingFactTypeColumn(lrTable)
                                        '20210505-VM-No longer needed. IsPartOfPrimaryKey uses Table Indexes to determine.
                                        'If arRoleConstraint.Role.Contains(lrRole) And lrFactType.InternalUniquenessConstraint.Count = 1 Then
                                        '    lrColumn.ContributesToPrimaryKey = True
                                        'End If
                                        'If arRoleConstraint.Role.Contains(lrRole) Then  20210523-VM-Removed, because e.g. if a FT is ternary, and a two role IUC/RC is being made, the third role/Column is none the less mandatory.
                                        lrColumn.IsMandatory = True
                                        'End If
                                        lrTable.addColumn(lrColumn, Me.Model.IsDatabaseSynchronised)
                                    End If

                                Case Is = pcenumConceptType.EntityType

                                    If Not lrTable.Column.Exists(Function(x) x.Role.Id = Me.Id) Then
                                        'There is no Column in the Table for the Role.
                                        Dim lrEntityType As FBM.EntityType = Me.JoinedORMObject

                                        If lrEntityType.HasCompoundReferenceMode Then

                                            Dim larColumn As New List(Of RDS.Column)
                                            Call lrEntityType.getCompoundReferenceSchemeColumns(lrTable, Me, larColumn)

                                            For Each lrColumn In larColumn
                                                lrTable.addColumn(lrColumn, Me.Model.IsDatabaseSynchronised)
                                            Next
                                        Else
                                            lrColumn = Me.GetCorrespondingFactTypeColumn(lrTable)
                                            'If arRoleConstraint.Role.Contains(lrRole) Then '20210523-VM-Removed, because e.g. if a FT is ternary, and a two role IUC/RC is being made, the third role/Column is none the less mandatory.
                                            lrColumn.IsMandatory = Me.Mandatory
                                            lrTable.addColumn(lrColumn, Me.Model.IsDatabaseSynchronised)
                                        End If

                                    End If

                                Case Else 'Joins a FactType.

                                    Dim larColumn As New List(Of RDS.Column)

                                    larColumn = Me.getColumns(lrTable, Me)

                                    For Each lrColumn In larColumn

                                        If Not lrTable.Column.Exists(Function(x) x.Role.Id = Me.Id And x.ActiveRole.Id = lrColumn.ActiveRole.Id) Then
                                            'There is no existing Column in the Table for lrColumn.
                                            lrColumn.Name = lrTable.createUniqueColumnName(lrColumn, lrColumn.Name, 0)
                                            lrTable.addColumn(lrColumn, Me.Model.IsDatabaseSynchronised)
                                        End If
                                    Next
                            End Select
                            '===End-Create new Columns===========================================================================

                            'RDS-Relationships
#Region "Create the Relationship if need be"

                            'Relation  
                            If Me.FactType.IsObjectified Then
                                Dim larLinkFactTypeRole = From FactType In Me.Model.FactType
                                                          Where FactType.IsLinkFactType = True _
                                                             And FactType.LinkFactTypeRole Is Me
                                                          Select FactType.RoleGroup(0)

                                If larLinkFactTypeRole.Count > 0 Then
                                    For Each lrLinkFactTypeRole In larLinkFactTypeRole
                                        Call Me.Model.generateRelationForManyTo1BinaryFactType(lrLinkFactTypeRole)
                                    Next
                                Else
                                    Select Case Me.JoinedORMObject.ConceptType
                                        Case Is = pcenumConceptType.EntityType, pcenumConceptType.FactType
                                            Call Me.Model.generateRelationForManyToManyFactTypeRole(Me)
                                    End Select
                                End If
                            Else
                                Select Case Me.JoinedORMObject.ConceptType
                                    Case Is = pcenumConceptType.EntityType, pcenumConceptType.FactType
                                        Call Me.Model.generateRelationForManyToManyFactTypeRole(Me)
                                End Select

                            End If
#End Region
                        End If
#End Region


#End Region
                    End If

                        RaiseEvent RoleJoinModified(Me.JoinedORMObject)

                        If Me.FactType.RoleGroup.FindAll(Function(x) x.JoinedORMObject Is Nothing).Count > 0 Then
                            'Likely creating a new binary FactType from the Toolbox, and still has a Role that is unjoined.
                        Else
                            Call Me.Model.Save()
                        End If


                    End If 'Not joined back to what it originally joined to.

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try


        End Sub

        ''' <summary>
        ''' Removes the Role from the Model if it is okay to do so.
        '''   i.e. Will not remove the Role if there is a RoleConstraint attached to the Role.
        ''' </summary>
        ''' <remarks></remarks>
        Public Overrides Function RemoveFromModel(Optional ByVal abForceRemoval As Boolean = False,
                                                  Optional ByVal abCheckForErrors As Boolean = True,
                                                  Optional ByVal abDoDatabaseProcessing As Boolean = True,
                                                  Optional ByVal abIncludeSubtypeRelationshipFactTypes As Boolean = True
                                                  ) As Boolean

            Try
                Me.Model.Role.Remove(Me)

                If abDoDatabaseProcessing Then
                    Call TableRole.DeleteRole(Me)
                End If

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Function

        Public Sub RemoveValueConstraint(ByVal asValueConstraint As String)

            Dim lsMessage As String
            Try
                Me.ValueConstraint.Remove(asValueConstraint)

                Call TableRoleValueConstraint.DeleteValueConstraint(Me.RoleConstraintRoleValueConstraint, asValueConstraint)

                Try
                    Call Me.RoleConstraintRoleValueConstraint.RemoveValueConstraint(asValueConstraint)
                Catch ex As Exception
                    Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                    lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                    lsMessage &= vbCrLf & vbCrLf & ex.Message
                    prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
                End Try

                RaiseEvent ValueConstraintRemoved(asValueConstraint)

            Catch ex As Exception
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Public Overrides Sub Save(Optional ByVal abRapidSave As Boolean = False)

            Try
                If abRapidSave Then
                    Call TableRole.AddRole(Me)

                ElseIf Me.isDirty Then

                    If TableRole.ExistsRole(Me) Then
                        Call TableRole.updateRole(Me)
                    Else
                        Call TableRole.AddRole(Me)
                    End If

                    Me.isDirty = False
                End If

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try
        End Sub

        Public Sub SetMandatory(ByVal abMandatoryStatus As Boolean, ByVal abBroadcastInterfaceEvent As Boolean)

            Try
                Me._Mandatory = abMandatoryStatus

                Call Me.makeDirty()
                Call Me.Model.MakeDirty(False, False)

                RaiseEvent MandatoryChanged(abMandatoryStatus)

                If My.Settings.UseClientServer And My.Settings.InitialiseClient And abBroadcastInterfaceEvent Then
                    Call prDuplexServiceClient.BroadcastToDuplexService(Viev.FBM.Interface.pcenumBroadcastType.ModelUpdateRole, Me, Nothing)
                End If

                '===============================================================
                'RDS

                'Relations
                Dim larOriginRelation = From Relation In Me.Model.RDS.Relation _
                                        From OriginColumn In Relation.OriginColumns _
                                        Where OriginColumn.Role.Id = Me.Id _
                                        Select Relation

                For Each lrRelation As RDS.Relation In larOriginRelation
                    lrRelation.setOriginMandatory(abMandatoryStatus)
                Next

                Dim larDestinationRelation = From Relation In Me.Model.RDS.Relation _
                                        From DestinationColumn In Relation.DestinationColumns _
                                        Where DestinationColumn.Role.Id = Me.Id _
                                        Select Relation

                For Each lrRelation As RDS.Relation In larDestinationRelation
                    lrRelation.setDestinationMandatory(abMandatoryStatus)
                Next

                If Not Me.HasInternalUniquenessConstraint And Me.FactType.IsManyTo1BinaryFactType Then
                    Dim larRelation = From Relation In Me.Model.RDS.Relation _
                                      Where Relation.ResponsibleFactType.Id = Me.FactType.Id _
                                      Select Relation

                    For Each lrRelation In larRelation
                        lrRelation.setDestinationMandatory(abMandatoryStatus)
                    Next
                End If

                If Me.FactType.Is1To1BinaryFactType Then
                    Dim larRelation = From Relation In Me.Model.RDS.Relation _
                                      Where Relation.ResponsibleFactType.Id = Me.FactType.Id _
                                      And Relation.OriginTable.Name = Me.JoinedORMObject.Id _
                                      Select Relation

                    For Each lrRelation In larRelation
                        lrRelation.setOriginMandatory(abMandatoryStatus)
                    Next

                    larRelation = From Relation In Me.Model.RDS.Relation _
                                  Where Relation.ResponsibleFactType.Id = Me.FactType.Id _
                                  And Relation.DestinationTable.Name = Me.JoinedORMObject.Id _
                                  Select Relation

                    For Each lrRelation In larRelation
                        lrRelation.setDestinationMandatory(abMandatoryStatus)
                    Next
                End If

                'Columns
                Dim larColumn = From Table In Me.Model.RDS.Table _
                                From Column In Table.Column _
                                Where Column.Role.Id = Me.Id _
                                Select Column

                For Each lrColumn In larColumn
                    Call lrColumn.setMandatory(abMandatoryStatus)
                Next

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        ''' <summary>
        ''' Sets the Name of the Role
        ''' </summary>
        ''' <param name="asName"></param>
        ''' <remarks></remarks>
        Public Overrides Function setName(ByVal asName As String,
                                          Optional ByVal abBroadcastInterfaceEvent As Boolean = True,
                                          Optional ByVal abSuppressModelSave As Boolean = False) As Boolean

            Try

                Me.Name = asName

                Call Me.makeDirty()
                Call Me.Model.MakeDirty(False, False)

                RaiseEvent RoleNameChanged(asName)

                If My.Settings.UseClientServer And My.Settings.InitialiseClient And abBroadcastInterfaceEvent Then
                    Call prDuplexServiceClient.BroadcastToDuplexService(Viev.FBM.Interface.pcenumBroadcastType.ModelUpdateRole, Me, Nothing)
                End If

                Return True

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
                Return False
            End Try

        End Function

        Public Sub SetValueRange(ByVal asNewValueRange As String)

            Me.ValueRange = asNewValueRange

            Call Me.makeDirty()
            Call Me.Model.MakeDirty(False, True)

            RaiseEvent ValueRangeChanged(asNewValueRange)

        End Sub

        Private Sub RoleConstraintRoleValueConstraint_RemovedFromModel(abBroadcastInterfaceEvent As Boolean) Handles RoleConstraintRoleValueConstraint.RemovedFromModel

            Try
                Me.RoleConstraintRoleValueConstraint = Nothing
            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

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