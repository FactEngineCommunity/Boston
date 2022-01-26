Imports System.ComponentModel
Imports MindFusion.Diagramming
Imports System.Drawing.Drawing2D
Imports System.Xml
Imports System.Xml.Serialization
Imports System.Xml.Schema
Imports System.Reflection

Namespace FBM
    <DefaultPropertyAttribute("Title")> _
    <Serializable()> _
    Public Class RoleInstance
        Inherits FBM.Role
        Implements IEquatable(Of FBM.RoleInstance)
        'Implements IXmlSerializable
        Implements FBM.iRolePageObject

        <XmlIgnore()> _
        Public Shadows Model As FBM.Model 'WithEvents

        <XmlIgnore()> _
        Public Page As FBM.Page

        <XmlIgnore()> _
        Public WithEvents Role As New FBM.Role

        Public Shadows JoinedORMObject As New FBM.ModelObject 'WithEvents

        <XmlAttribute()>
        Public Shadows ReadOnly Property TypeOfJoin As pcenumRoleJoinType
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
        Public Shadows ReadOnly Property JoinsEntityType As FBM.EntityTypeInstance
            Get
                If Me.TypeOfJoin = pcenumRoleJoinType.EntityType Then
                    Return CType(Me.JoinedORMObject, FBM.EntityTypeInstance)
                Else
                    Return Nothing
                End If
            End Get
        End Property

        <XmlIgnore()>
        Public Shadows ReadOnly Property JoinsValueType As FBM.ValueTypeInstance
            Get
                If Me.TypeOfJoin = pcenumRoleJoinType.ValueType Then
                    Return CType(Me.JoinedORMObject, FBM.ValueTypeInstance)
                Else
                    Return Nothing
                End If
            End Get
        End Property

        <XmlIgnore()>
        Public Shadows ReadOnly Property JoinsFactType As FBM.FactTypeInstance
            Get
                If Me.TypeOfJoin = pcenumRoleJoinType.FactType Then
                    Return CType(Me.JoinedORMObject, FBM.FactTypeInstance)
                Else
                    Return Nothing
                End If
            End Get
        End Property

        <XmlIgnore()> _
        Public Shadows FactType As FBM.FactTypeInstance 'The FactTypeInstance (including 'ShapeNode') to which this RoleInstance is associated.    
        <XmlIgnore()> _
        Public Shadows RoleConstraint As New List(Of FBM.RoleConstraintInstance)
        <XmlIgnore()> _
        Public Shadows RoleConstraintRole As New List(Of FBM.RoleConstraintRoleInstance)

        Public Shadows Property Mandatory() As Boolean
            Get
                Return _Mandatory
            End Get
            Set(ByVal Value As Boolean)
                _Mandatory = Value
                Me.Role.Mandatory = Me.Mandatory
            End Set
        End Property

#Region "Role Value Constraint"

        <XmlIgnore()>
        Public Shadows WithEvents RoleConstraintRoleValueConstraint As FBM.RoleValueConstraint = Nothing

        <XmlIgnore()>
        <CategoryAttribute("Role Constraint"),
        Browsable(True),
        [ReadOnly](False),
        DescriptionAttribute("The list of Values that Objects that play this Role may take."),
        Editor(GetType(tStringCollectionEditor), GetType(System.Drawing.Design.UITypeEditor))>
        Public Shadows Property ValueConstraint() As Viev.Strings.StringCollection  'NB This is what is edited in the PropertyGrid
            Get
                Return Me._ValueConstraintList
            End Get
            Set(ByVal Value As Viev.Strings.StringCollection)
                Me._ValueConstraintList = Value
            End Set
        End Property
#End Region

        <NonSerialized()> _
        <XmlIgnore()> _
        Public Shape As New ShapeNode

        <XmlIgnore()> _
        Public RoleName As New FBM.RoleName

        <NonSerialized()> _
        <XmlIgnore()> _
        Public Link As DiagramLink

        Public height As Integer
        Public width As Integer = 6

        Public _X As Integer = 0
        Public Property X As Integer Implements FBM.iPageObject.X
            Get
                If Me.Shape IsNot Nothing Then
                    Return Me.Shape.Bounds.X
                Else
                    Return 0
                End If

            End Get
            Set(ByVal value As Integer)
                Me._X = value
            End Set
        End Property

        Public _Y As Integer = 0
        Public Property Y As Integer Implements FBM.iPageObject.Y
            Get
                If Me.Shape IsNot Nothing Then
                    Return Me.Shape.Bounds.Y
                Else
                    Return 0
                End If

            End Get
            Set(ByVal value As Integer)
                Me._Y = value
            End Set
        End Property


        Public Sub New()
            '-------------------
            'Parameterless New
            '-------------------
            Me.JoinedORMObject = New FBM.ModelObject
        End Sub

        Public Sub New(ByRef arModel As FBM.Model,
                       Optional ByRef arPage As FBM.Page = Nothing,
                       Optional ByVal aiX As Object = Nothing,
                       Optional ByVal aiY As Object = Nothing)

            If IsSomething(arModel) Then Me.Role.Model = arModel

            If IsSomething(arPage) Then
                Me.Page = arPage
                Me.RoleName.Page = arPage
            End If

            If IsSomething(aiX) Then Me.X = aiX
            If IsSomething(aiY) Then Me.Y = aiY

            Me.RoleName.RoleInstance = Me

        End Sub

        Public Sub New(ByRef arModel As FBM.Model,
                       ByRef arPage As FBM.Page,
                       ByRef arRole As FBM.Role)

            Me.Role.Model = arModel
            Me.Page = arPage
            Me.Id = arRole.Id
            Me.Role = arRole

            Me.RoleName.Page = arPage
            Me.RoleName.RoleInstance = Me

        End Sub

        Public Sub New(ByRef arFactTypeInstance As FBM.FactTypeInstance,
                       ByRef ar_joined_object As Object,
                       ByRef arRole As FBM.Role)

            '----------------------------------------------------------------------------------------------------
            'PRECONDITIONS:
            '  * The Role referenced by the RoleInstance already exists in the Model
            '  * The FactType referenced by the FactTypeInstance of the RoleInstance already exists in the Model
            '----------------------------------------------------------------------------------------------------

            MyBase.ConceptType = pcenumConceptType.Role
            Me.Id = arRole.Id
            Me.Role = arRole

            Me.FactType = arFactTypeInstance
            Me.Model = arFactTypeInstance.Model
            Me.Page = arFactTypeInstance.Page
            Me.JoinedORMObject = ar_joined_object
            Me.SequenceNr = Me.FactType.FactType.Arity 'The arity of the FactType of the FactTypeInstance

            '----------------------------------------------------------
            'Add the Role to the RoleGroup of the FactType of the Role
            '----------------------------------------------------------
            Me.FactType.RoleGroup.Add(Me)

            Me.RoleName.Page = Me.Page
            Me.RoleName.RoleInstance = Me

        End Sub

        Public Overloads Function Clone(ByRef arPage As FBM.Page, Optional ByVal abAddToPage As Boolean = False) As Object

            Dim lrRoleInstance As New FBM.RoleInstance

            Try

                With Me
                    lrRoleInstance.Model = arPage.Model
                    lrRoleInstance.Page = arPage
                    lrRoleInstance.Symbol = .Symbol
                    lrRoleInstance.Id = .Id
                    lrRoleInstance.Name = .Name
                    lrRoleInstance.Symbol = .Symbol
                    lrRoleInstance.Role = New FBM.Role
                    lrRoleInstance.Role = arPage.Model.Role.Find(AddressOf .Role.Equals)

                    Select Case .TypeOfJoin
                        Case Is = pcenumRoleJoinType.EntityType
                            Dim lrEntityTypeInstance As FBM.EntityTypeInstance = .JoinedORMObject
                            lrEntityTypeInstance = lrEntityTypeInstance.Clone(arPage, True, lrEntityTypeInstance.EntityType.IsMDAModelElement, True)
                            arPage.EntityTypeInstance.AddUnique(lrEntityTypeInstance)
                            lrRoleInstance.JoinedORMObject = lrEntityTypeInstance 'arPage.EntityTypeInstance.Find(AddressOf .JoinedORMObject.Equals)
                        Case Is = pcenumRoleJoinType.ValueType
                            Dim lrValueTypeInstance As FBM.ValueTypeInstance = .JoinedORMObject
                            lrValueTypeInstance = lrValueTypeInstance.Clone(arPage, lrValueTypeInstance.ValueType.IsMDAModelElement, True)
                            arPage.ValueTypeInstance.AddUnique(lrValueTypeInstance)
                            lrRoleInstance.JoinedORMObject = lrValueTypeInstance 'arPage.ValueTypeInstance.Find(AddressOf .JoinedORMObject.Equals)
                        Case Is = pcenumRoleJoinType.FactType
                            If arPage.FactTypeInstance.Exists(AddressOf .JoinedORMObject.Equals) Then
                                lrRoleInstance.JoinedORMObject = arPage.FactTypeInstance.Find(AddressOf .JoinedORMObject.Equals)
                            Else
                                Dim lrJoinedFactTypeInstance As New FBM.FactTypeInstance
                                lrJoinedFactTypeInstance = .JoinedORMObject
                                lrJoinedFactTypeInstance = lrJoinedFactTypeInstance.Clone(arPage, False)
                                If Not arPage.FactTypeInstance.Exists(AddressOf lrJoinedFactTypeInstance.Equals) Then
                                    arPage.FactTypeInstance.AddUnique(lrJoinedFactTypeInstance)
                                End If
                                lrRoleInstance.JoinedORMObject = lrJoinedFactTypeInstance
                            End If
                    End Select

                    lrRoleInstance.Deontic = .Deontic
                    lrRoleInstance.FactType = arPage.FactTypeInstance.Find(AddressOf .FactType.Equals)
                    lrRoleInstance.KLFreeVariableLabel = .KLFreeVariableLabel
                    lrRoleInstance.Id = .Id
                    lrRoleInstance.SequenceNr = .SequenceNr
                    lrRoleInstance.Mandatory = .Mandatory
                    lrRoleInstance.RoleName = .RoleName.Clone(arPage, lrRoleInstance)

                    lrRoleInstance.Shape = New ShapeNode
                    lrRoleInstance.X = .X
                    lrRoleInstance.Y = .Y

                    If abAddToPage Then
                        arPage.RoleInstance.AddUnique(lrRoleInstance)
                    End If
                End With

                Return lrRoleInstance
            Catch ex As Exception
                Dim lsMessage As String = ""

                lsMessage = "Error: tRoleInstance.Clone: " & vbCrLf & vbCrLf & ex.Message
                Call prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)

                Return lrRoleInstance
            End Try

        End Function

        Public Shadows Function Equals(other As RoleInstance) As Boolean Implements IEquatable(Of RoleInstance).Equals

            Return (Me.Id = other.Id)

        End Function

        ''' <summary>
        '''Serializes all public, private and public fields except the one 
        '''  which are the hidden fields for the eventhandlers
        ''' </summary>
        ''' <remarks></remarks>
        ''' 
        Private Shadows Sub WriteXML(ByVal writer As XmlWriter)

            '' Get the list of all events 
            'Dim EvtInfos() As EventInfo = Me.GetType.GetEvents()
            'Dim EvtInfo As EventInfo

            '' Get the list of all fields
            'Dim FldInfos() As FieldInfo = Me.GetType.GetFields(BindingFlags.NonPublic Or BindingFlags.Instance Or BindingFlags.Public)

            '' Loops in each field and decides wether to serialize it or not
            'Dim FldInfo As FieldInfo



            'MsgBox("hello")
            writer.WriteAttributeString("Hello", "World")
            writer.WriteElementString("Hello", "World")
            'For Each FldInfo In FldInfos
            '    ' Finds if the field is a eventhandler
            '    Dim Found As Boolean = False
            '    For Each EvtInfo In EvtInfos
            '        If EvtInfo.Name + "Event" = FldInfo.Name Then
            '            Found = True
            '            Exit For
            '        End If
            '    Next

            '    ' If field is not an eventhandler serializes it
            '    If Not Found Then
            '        info.AddValue(FldInfo.Name, FldInfo.GetValue(Me))
            '    End If

            'Next

        End Sub

        Public Shadows Sub readxml(ByVal reader As XmlReader)
            '       	reader.MoveToContent();
            'Name = reader.GetAttribute("Name");
            'Boolean isEmptyElement = reader.IsEmptyElement; // (1)
            'reader.ReadStartElement();
            'if (!isEmptyElement) // (1)
            '{
            '	Birthday = DateTime.ParseExact(reader.
            '		ReadElementString("Birthday"), "yyyy-MM-dd", null);
            '	reader.ReadEndElement();

        End Sub

        Public Shadows Function getschema() As XmlSchema
            Return Nothing
        End Function

        Public Overloads Function GetAttributeName() As String

            Dim lsAttributeName As String = "DummyAttribute"
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
                                    Dim lrEntityTypeInstance As FBM.EntityTypeInstance
                                    lrEntityTypeInstance = lrModelObject
                                    If lrEntityTypeInstance.HasSimpleReferenceScheme Then
                                        lsAttributeName &= lrEntityTypeInstance.ReferenceMode.Replace(".", "")
                                    End If
                            End Select

                            larRole = New List(Of FBM.Role)
                            larRole.Add(Me.Role)
                            larRole.Add(Me.FactType.FactType.GetOtherRoleOfBinaryFactType(Me.Id))
                            lrFactTypeReading = Me.FactType.FactType.FindSuitableFactTypeReadingByRoles(larRole, True)
                            If lrFactTypeReading IsNot Nothing Then
                                lsAttributeName = lrFactTypeReading.PredicatePart(1).PreBoundText.Replace("-", "") & lsAttributeName
                            End If

                        ElseIf Me.FactType.IsBinaryFactType Then
                            lrModelObject = Me.FactType.GetOtherRoleOfBinaryFactType(Me.Id).JoinedORMObject
                            lsAttributeName = lrModelObject.Name

                            Select Case lrModelObject.ConceptType
                                Case Is = pcenumConceptType.EntityType
                                    Dim lrEntityTypeInstance As FBM.EntityTypeInstance
                                    lrEntityTypeInstance = lrModelObject
                                    If lrEntityTypeInstance.HasSimpleReferenceScheme Then
                                        lsAttributeName &= lrEntityTypeInstance.ReferenceMode.Replace(".", "")
                                    End If
                            End Select

                            larRole = New List(Of FBM.Role)
                            larRole.Add(Me.Role)
                            larRole.Add(Me.FactType.FactType.GetOtherRoleOfBinaryFactType(Me.Id))
                            lrFactTypeReading = Me.FactType.FactType.FindSuitableFactTypeReadingByRoles(larRole, True)
                            If lrFactTypeReading IsNot Nothing Then
                                lsAttributeName = lrFactTypeReading.PredicatePart(1).PreBoundText.Replace("-", "") & lsAttributeName
                            End If

                        Else
                            lsAttributeName = Me.JoinedORMObject.Name
                        End If
                    Case Is = pcenumRoleJoinType.FactType
                        If Me.FactType.Is1To1BinaryFactType Then
                            lsAttributeName = Me.FactType.GetOtherRoleOfBinaryFactType(Me.Id).JoinedORMObject.Name
                        ElseIf Me.FactType.IsBinaryFactType Then
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
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)

                Return "Error getting Attribute"
            End Try


        End Function

        Public Overloads Function BelongsToTable() As String
            'Input  - Role_id
            'Output - The Table_Name to which the Role belongs            

            BelongsToTable = Nothing

            If Me.FactType.HasTotalRoleConstraint Or Me.FactType.HasPartialButMultiRoleConstraint Then
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
                Select Case Me.TypeOfJoin
                    Case Is = pcenumRoleJoinType.EntityType
                        'Role joins to entity
                        If Me.FactType.Is1To1BinaryFactType Then
                            'Is 1:1 binary fact type
                            'If Me.Mandatory = True Then
                            If Me.FactType.TypeOfBinaryFactType = 1 Then
                                'Part 1 of Rule 4
                                BelongsToTable = Me.JoinsEntityType.Name
                                Exit Function
                            ElseIf Me.FactType.TypeOfBinaryFactType = 2 Then
                                'Part 2 of Rule 4
                                BelongsToTable = Me.FactType.Name
                                Exit Function
                            Else
                                'FactType has no mandatory Roles
                                BelongsToTable = Me.JoinsEntityType.Name
                            End If
                            'End If
                        ElseIf Me.HasInternalUniquenessConstraint Then
                            BelongsToTable = Me.JoinsEntityType.Name
                            Exit Function                        
                        End If
                    Case Is = pcenumRoleJoinType.FactType
                        'Joins nested Fact Object
                        BelongsToTable = Me.JoinsFactType.Name
                        Exit Function
                    Case Is = pcenumRoleJoinType.ValueType
                        BelongsToTable = "Mystery"
                End Select

            End If

        End Function


        Sub delete_role(ByVal project_id As Integer, ByVal cluster_id As Integer, ByVal RoleId As Integer)
            '-------------------------------------------------------------------------------------
            'This process is only called from other delete procedures
            'eg from within delete_entity (if a role joins to an entity instance of the entity)
            'Deletes all
            'Roles
            'Role Constraints
            'with RoleId

            'role
            'for every role record
            'subject_area_frm.Cluster_Tbl.Refresh
            ' Find the record you want to delete.
            'subject_area_frm.Cluster_Tbl.Recordset.FindFirst "Cluster_Id = " & main_frm.Combo2.ItemData((main_frm.Combo2.ListIndex))
            'If Not subject_area_frm.Cluster_Tbl.Recordset.NoMatch Then ' Check if record found.
            '    subject_area_frm.Cluster_Tbl.Recordset.Delete  ' Delete the record.
            'End If
            'loop

            'identify all roles have this RoleId
            'and delete their instances
            'for all ValueTypeInstances
            'if ValueTypeInstance.Id = ValueTypeId then
            'call DeleteValueTypeInstance()
            'end if
            'loop

            'identify all RoleConstraints that have this RoleId
            'and delete their instances
            'for all RoleConstraints
            'if RoleConstraints.RoleId = ValueTypeId then
            'call delete_RoleConstraint()
            'end if
            'loop

        End Sub

        Sub DisplayAndAssociate(ByRef arFactTypeInstance As FBM.FactTypeInstance)
            '---------------------------------------------------------------------------------------------------
            'Creates the visual RoleInstance object within the model, attaches the object details of the Role(Instance)
            '  to the visual object and attaches the visual Role object to the visual FactTypeInstance (within
            '  which the RoleInstance resides).
            '---------------------------------------------------------------------------------------------------

            Dim loDroppedNode As New ShapeNode

            Try
                '-----------------------------------------------------------------------------
                'Establish the position of the RoleInstance relative to the FactTypeInstance
                '-----------------------------------------------------------------------------
                Me._X = Me.FactType.Shape.Bounds.X + 3
                Me._Y = Me.FactType.Shape.Bounds.Y + 4 + ((Me.FactType.FactType.GetHighestConstraintLevel - 1) * 1.6)

                If Me.Role.SequenceNr = 1 Then
                    '--------------------------------------------------------------------
                    'Is first Role in the RoleGroup/FactType, so setup a new ShapeGroup 
                    '--------------------------------------------------------------------
                    loDroppedNode = Me.Page.Diagram.Factory.CreateShapeNode(Me._X, Me._Y, 6, 4)

                    '---------------------------------------------------------------------------
                    'Attach the Role.ShapeNode to the FactType.ShapeNode ShapeGroup,
                    '  because this Role is effectively the MasterRole of the RoleGroup/FactType
                    '---------------------------------------------------------------------------                
                    loDroppedNode.AttachTo(Me.FactType.Shape, AttachToNode.MiddleLeft)
                Else
                    '----------------------------------------------
                    'Drop relative to other Roles in the FactType
                    '----------------------------------------------
                    loDroppedNode = Me.Page.Diagram.Factory.CreateShapeNode(Me._X + (arFactTypeInstance.RoleGroup(0).Shape.Bounds.Width * (Me.SequenceNr - 1)), Me._Y, 6, 4, Shapes.Rectangle)

                    '--------------------------------------------------------------------------------------
                    'Resize the FactType.Shape and reset the position of the RoleGroup within the factType
                    '--------------------------------------------------------------------------------------
                    Dim lo_rectangle As New Rectangle(Me.FactType.Shape.Bounds.X, Me.FactType.Shape.Bounds.Y, ((Me.FactType.RoleGroup(0).Shape.Bounds.Width * Me.FactType.Arity) + 6), 15)

                    Me.FactType.Shape.SetRect(lo_rectangle, False)

                    loDroppedNode.AttachTo(arFactTypeInstance.Shape, AttachToNode.BottomCenter)
                End If


                loDroppedNode.Tag = New FBM.RoleInstance
                loDroppedNode.Tag = Me
                loDroppedNode.Shape = Shapes.Rectangle
                loDroppedNode.HandlesStyle = HandlesStyle.InvisibleMove
                loDroppedNode.AllowOutgoingLinks = True
                loDroppedNode.ToolTip = "Role"
                If Me.FactType.isPreferredReferenceMode Then
                    loDroppedNode.Visible = False
                Else
                    loDroppedNode.Visible = True
                End If
                loDroppedNode.Pen.Width = 0.1
                If Me.FactType.IsLinkFactType Then
                    loDroppedNode.Pen.DashPattern = New Single() {4, 3, 4, 3}
                End If

                Me.Shape = loDroppedNode

                '----------------------------------------
                'Setup the RoleName
                '----------------------------------------                        
                'Me.RoleName = New FBM.RoleName(Me, Me.Name)
                Call Me.RoleName.DisplayAndAssociate(Me)

                '-------------------------------------------------------------
                'Find the ORMObject to which the Role/RoleInstance attaches
                '-------------------------------------------------------------
                Select Case Me.TypeOfJoin
                    Case Is = pcenumRoleJoinType.EntityType
                        Me.JoinedORMObject = Me.Page.EntityTypeInstance.Find(Function(x) x.Id = Me.JoinsEntityType.Id)
                    Case Is = pcenumRoleJoinType.ValueType
                        Me.JoinedORMObject = Me.Page.ValueTypeInstance.Find(Function(x) x.Id = Me.JoinsValueType.Id)
                    Case Is = pcenumRoleJoinType.FactType
                        Me.JoinedORMObject = Me.Page.FactTypeInstance.Find(Function(x) x.Id = Me.JoinedORMObject.Id)
                        If Me.JoinsFactType.IsDisplayedAssociated Then
                            '------------------------------------------------------------------------------
                            'That's good, the joined FactTypeInstance is already Displayed and Associated
                            '------------------------------------------------------------------------------
                        Else
                            Me.JoinsFactType.DisplayAndAssociate(False, False)
                        End If
                    Case Else
                        '--------------------------------------------------------------------------
                        'When dropping a new Role onto an existing FactType, there is no object joined yet
                        Me.JoinedORMObject = Nothing
                End Select

                '-------------------------------------------------------------
                'Create the link between the Role and the ORMObject that it 
                '  joins to.
                '-------------------------------------------------------------
                If IsSomething(Me.JoinedORMObject) Then
                    Dim lrJoinedORMObject As Object = Me.JoinedORMObject
                    Dim loNode As MindFusion.Diagramming.ShapeNode = lrJoinedORMObject.shape
                    Dim lo_link As New DiagramLink(Me.Page.Diagram, Me.Shape, loNode)
                    lo_link.Locked = False ' was originally True
                    lo_link.Tag = Me
                    Me.Link = lo_link
                    Me.Shape.OutgoingLinks.Add(lo_link)
                    If Me.FactType.isPreferredReferenceMode Then
                        Me.Link.Visible = False
                    Else
                        Me.Link.Visible = True
                    End If
                    Me.Link.Pen.Width = 0.3

                    '---------------------
                    'Mark Mandatory Roles
                    '---------------------
                    If Me.Mandatory Then
                        lo_link.BaseShape = ArrowHead.Circle
                    Else
                        lo_link.BaseShape = ArrowHead.None
                    End If

                    Me.Page.Diagram.Links.Add(lo_link)
                End If

                '--------------------------------------------------------------------------------------
                'Resize the FactType.Shape and reset the position of the RoleGroup within the factType
                '--------------------------------------------------------------------------------------
                'Dim lo_rectangle As New Rectangle(Me.FactType.Shape.Bounds.X, Me.FactType.Shape.Bounds.Y, ((Me.FactType.RoleGroup(0).Shape.Bounds.Width * Me.FactType.Arity) + 6), 15)

                'Me.FactType.Shape.SetRect(lo_rectangle, False)

                'Just to be sure.
                If Me.FactType.RoleGroup.Count > 0 Then
                    If Me.FactType.RoleGroup(0).Shape IsNot Nothing Then
                        Me.Shape.Move(Me.X, Me.FactType.RoleGroup(0).Shape.Bounds.Y)
                    End If
                End If



                '------------------------------------------
                'Set the AnchorPatterns for the RoleGroup
                '------------------------------------------
                Call Me.FactType.ResetAnchorsForRoleGroup()

                Me.Page.Diagram.Invalidate()

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Public Overloads Function HasInternalUniquenessConstraint() As Boolean
            '-------------------------------------------------------------------------------
            'Used whn Automatically generating Tables/Entities/Classes
            'Used predominantly to see which Entity of a binary FactType
            'owns the uniqueness constraint (Role can't be part of 1:1 binary fact type)
            '-------------------------------------------------------------------------------

            'Returns True if 'owns' a internal uniqueness constraint
            'else returns false
            Dim lrRoleConstraintRole As FBM.RoleConstraintRoleInstance

            HasInternalUniquenessConstraint = False

            For Each lrRoleConstraintRole In Me.RoleConstraintRole
                If lrRoleConstraintRole.RoleConstraint.RoleConstraintType = pcenumRoleConstraintType.InternalUniquenessConstraint Then
                    HasInternalUniquenessConstraint = True
                    Exit Function
                End If
            Next

        End Function

        Public Shadows Function identify_by_FactType_position(ByVal other As FBM.RoleInstance) As Boolean

            If (Object.Equals(Me.Role.FactType, other.Role.FactType)) And (Me.Role.SequenceNr = other.Role.SequenceNr) Then
                Return True
            Else
                Return False
            End If

        End Function

        Public Overloads Function IsAnAttributeRole() As Boolean

            IsAnAttributeRole = False

            Select Case Me.TypeOfJoin
                Case Is = pcenumRoleJoinType.EntityType
                    If Me.HasInternalUniquenessConstraint() And Me.FactType.Is1To1BinaryFactType Then
                        'Rule 4
                        If IsSomething(Me.JoinsEntityType.ReferenceModeFactType) Then
                            If Me.FactType.Id = Me.JoinsEntityType.ReferenceModeFactType.Id Then
                                '---------------------------------------------------
                                'Is Role on ReferenceModeFactType
                                '  This isn't generally enough to create an Entity
                                '---------------------------------------------------
                                IsAnAttributeRole = False
                            Else
                                IsAnAttributeRole = True
                            End If
                        Else
                            IsAnAttributeRole = True
                        End If
                    ElseIf Me.FactType.HasTotalRoleConstraint Then
                        'Rule 3
                        IsAnAttributeRole = True
                    ElseIf Me.HasInternalUniquenessConstraint() And Me.FactType.IsBinaryFactType Then
                        'Rule 2
                        IsAnAttributeRole = True
                    End If
                Case Is = pcenumRoleJoinType.ValueType
                    'ValueType                    
                    IsAnAttributeRole = False
                Case Is = pcenumRoleJoinType.FactType
                    'ObjectifiedFactType        
                    IsAnAttributeRole = True
            End Select

        End Function

        Public Overloads Overrides Function IsERDPropertyRole() As Boolean

            IsERDPropertyRole = False

            Select Case Me.TypeOfJoin
                Case Is = pcenumRoleJoinType.EntityType
                    If Me.HasInternalUniquenessConstraint() And Me.FactType.Is1To1BinaryFactType Then
                        'Rule 4
                        If IsSomething(Me.JoinsEntityType.ReferenceModeFactType) Then
                            If Me.FactType.Id = Me.JoinsEntityType.ReferenceModeFactType.Id Then
                                '---------------------------------------------------
                                'Is Role on ReferenceModeFactType
                                '  This isn't generally enough to create an Entity
                                '---------------------------------------------------
                                IsERDPropertyRole = False
                            Else
                                IsERDPropertyRole = True
                            End If
                        Else
                            IsERDPropertyRole = True
                        End If
                    ElseIf Me.FactType.HasTotalRoleConstraint Then
                        'Rule 3 for ERDs but different for PGS
                        IsERDPropertyRole = True
                    ElseIf Me.HasInternalUniquenessConstraint() And Me.FactType.IsBinaryFactType Then
                        'Rule 2
                        IsERDPropertyRole = True
                    End If
                Case Is = pcenumRoleJoinType.ValueType
                    'ValueType                    
                    IsERDPropertyRole = False
                Case Is = pcenumRoleJoinType.FactType
                    'ObjectifiedFactType        
                    IsERDPropertyRole = True
            End Select

        End Function

        Public Overloads Function IsPGSPropertyRole() As Boolean

            IsPGSPropertyRole = False

            Select Case Me.TypeOfJoin
                Case Is = pcenumRoleJoinType.EntityType
                    If Me.HasInternalUniquenessConstraint() And Me.FactType.Is1To1BinaryFactType Then
                        'Rule 4
                        If IsSomething(Me.JoinsEntityType.ReferenceModeFactType) Then
                            If Me.FactType.Id = Me.JoinsEntityType.ReferenceModeFactType.Id Then
                                '---------------------------------------------------
                                'Is Role on ReferenceModeFactType
                                '  This isn't generally enough to create an Entity
                                '---------------------------------------------------
                                IsPGSPropertyRole = False
                            Else
                                IsPGSPropertyRole = True
                            End If
                        Else
                            IsPGSPropertyRole = True
                        End If
                    ElseIf Me.FactType.HasTotalRoleConstraint Then
                        'Rule 3 for ERDs but different for PGS
                        IsPGSPropertyRole = False
                    ElseIf Me.HasInternalUniquenessConstraint() And Me.FactType.IsBinaryFactType Then
                        'Rule 2
                        IsPGSPropertyRole = True
                    End If
                Case Is = pcenumRoleJoinType.ValueType
                    'ValueType                    
                    IsPGSPropertyRole = False
                Case Is = pcenumRoleJoinType.FactType
                    'ObjectifiedFactType        
                    IsPGSPropertyRole = True
            End Select

        End Function


        Public Sub RemoveFromPage()

            If Me.Page.Diagram IsNot Nothing Then
                If Me.Shape IsNot Nothing Then
                    Me.Page.Diagram.Nodes.Remove(Me.Shape)
                End If

                If Me.Link IsNot Nothing Then
                    Me.Page.Diagram.Links.Remove(Me.Link)
                End If

                Me.Page.RoleInstance.Remove(Me)

                If Me.RoleName IsNot Nothing Then
                    If Me.RoleName.Shape IsNot Nothing Then
                        Call Me.Page.Diagram.Nodes.Remove(Me.RoleName.Shape)
                    End If
                End If

            End If

        End Sub

        Sub refresh_role_instance()


            Try

                If Me.Page.Diagram Is Nothing Then
                    Exit Sub
                End If

                Dim StringSize As New SizeF
                Dim G As Graphics
                Dim myFont As New Font(Me.Page.Diagram.Font.FontFamily, Me.Page.Diagram.Font.Size, FontStyle.Regular, GraphicsUnit.Pixel)

                Try
                    If (Me.JoinedORMObject IsNot Nothing) Then
                        If Me.Mandatory And (Me.Shape.OutgoingLinks.Count > 0) Then
                            Me.Shape.OutgoingLinks(0).BaseShape = ArrowHead.Circle
                        Else
                            Me.Shape.OutgoingLinks(0).BaseShape = ArrowHead.None
                        End If

                        If Me.Deontic And (Me.Shape.OutgoingLinks.Count > 0) Then
                            Me.Shape.OutgoingLinks(0).HeadShape = ArrowHead.Circle
                        Else
                            Me.Shape.OutgoingLinks(0).HeadShape = ArrowHead.None
                        End If
                    End If
                Catch ex As Exception
                    'Sometimes has no outgoing links.
                End Try

                If IsSomething(Me.RoleName) Then
                        '------------------------------------------------------
                        'RoleName is already displayed for the RoleInstance
                        '------------------------------------------------------
                        If Me.Role.Name <> "" Then
                            Me.RoleName.Shape.Text = "[" & Me.Role.Name & "]"
                            Me.RoleName.Shape.Resize(StringSize.Width, StringSize.Height)
                        End If
                    Else
                        '-----------------------------------------
                        'Setup the RoleName for the RoleInstance
                        '-----------------------------------------
                        Dim loRoleName As ShapeNode

                        G = Me.Page.Form.CreateGraphics

                        StringSize = Me.Page.Diagram.MeasureString(Trim(Me.Role.Name), Me.Page.Diagram.Font, 1000, System.Drawing.StringFormat.GenericDefault)

                        loRoleName = Me.Page.Diagram.Factory.CreateShapeNode(Me.Shape.Bounds.X, Me.Shape.Bounds.Y - StringSize.Height, StringSize.Width, StringSize.Height)
                        loRoleName.Shape = MindFusion.Diagramming.Shapes.Rectangle
                        loRoleName.HandlesStyle = HandlesStyle.InvisibleMove
                        If Trim(Me.Role.Name) <> "" Then
                            loRoleName.Text = "[" & Trim(Me.Role.Name) & "]"
                        Else
                            loRoleName.Text = ""
                        End If
                        loRoleName.TextColor = Color.Blue
                        loRoleName.Transparent = True

                        loRoleName.Visible = True
                        loRoleName.ZTop()
                        Dim lrRoleName As FBM.RoleName = New FBM.RoleName(Me, Me.Role.Name)
                        lrRoleName.RoleInstance = Me
                        loRoleName.Tag = lrRoleName
                        '---------------------------------------------------------------------------
                        'Attach the Role.RoleName ShapeNode to the Role ShapeGroup,                                    
                        '---------------------------------------------------------------------------
                        loRoleName.AttachTo(Me.Shape, AttachToNode.TopLeft)
                        lrRoleName.Shape = loRoleName
                        Me.RoleName = lrRoleName
                    End If

                    If Me.FactType.IsLinkFactType Then
                        Me.Shape.Pen.DashPattern = New Single() {4, 3, 4, 3}
                    End If

                    Me.Page.Diagram.Invalidate()

                Catch ex As Exception
                    Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub


        Public Sub RefreshShape(Optional ByVal aoChangedPropertyItem As PropertyValueChangedEventArgs = Nothing,
                                Optional ByVal asSelectedGridItemLabel As String = "")

            Try
                '-----------------------------
                'Update the underlying model
                '-----------------------------
                If IsSomething(aoChangedPropertyItem) Then
                    Select Case aoChangedPropertyItem.ChangedItem.PropertyDescriptor.Name
                        Case Is = "Name"
                            Call Me.Role.setName(Me.Name, True)
                        Case Is = "ValueRange"
                            Call Me.Role.SetValueRange(Me.ValueRange)
                        Case Is = "Value"
                            With New WaitCursor
                                Call Me.Role.ModifyValueConstraint(aoChangedPropertyItem.OldValue, aoChangedPropertyItem.ChangedItem.Value.ToString)

                                If Me.RoleConstraintRoleValueConstraint Is Nothing Then
                                    Me.RoleConstraintRoleValueConstraint = Me.Role.RoleConstraintRoleValueConstraint.CloneRoleValueConstraintInstance(Me.Page, True)
                                    Call Me.RoleConstraintRoleValueConstraint.DisplayAndAssociate()
                                End If
                            End With
                    End Select
                End If

                '-------------------------------------------------------------------------------------------------------------------------------
                'Removing an item using the UITypeEditor does not trigger a return of aoChangedPropertyItem (As PropertyValueChangedEventArgs).
                '  So we must check each time (back here) whether there is an item to remove from the ValueConstraint list for the ValueType/Instance.
                Dim lasValueConstraint(Me.Role.ValueConstraint.Count) As String
                Me.Role.ValueConstraint.CopyTo(lasValueConstraint, 0)

                For Each lsValueConstraint In lasValueConstraint
                    If lsValueConstraint IsNot Nothing Then
                        If Not Me.ValueConstraint.Contains(lsValueConstraint) Then
                            Call Me.Role.RemoveValueConstraint(lsValueConstraint)
                        End If
                    End If
                Next

                If IsSomething(Me.RoleName) Then
                    Me.RoleName.RefreshShape()
                End If

                Me.Role.FrequencyConstraint = Me.FrequencyConstraint

                Me.Page.Model.IsDirty = True

                Call Me.refresh_role_instance()

                If Me.Page.Diagram IsNot Nothing Then
                    Me.Page.Diagram.Invalidate()
                End If

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        ''' <summary>
        ''' NB See also: FactTypeInstance.ResetAnchorsForRoleGroup
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub ResetAnchors()

            Dim apat1 As AnchorPattern = Nothing
            Dim liPositionInRoleGroup As Integer

            If liPositionInRoleGroup = 1 Then
                apat1 = New AnchorPattern(New AnchorPoint() { _
                        New AnchorPoint(50, 0, True, True), _
                        New AnchorPoint(50, 100, True, True), _
                        New AnchorPoint(0, 50, True, True)})

            ElseIf (liPositionInRoleGroup > 1) And (liPositionInRoleGroup < Me.FactType.Arity) Then
                apat1 = New AnchorPattern(New AnchorPoint() { _
                        New AnchorPoint(50, 0, True, True), _
                        New AnchorPoint(50, 100, True, True)})
            ElseIf liPositionInRoleGroup = Me.FactType.Arity Then
                apat1 = New AnchorPattern(New AnchorPoint() { _
                        New AnchorPoint(50, 0, True, True), _
                        New AnchorPoint(100, 50, True, True), _
                        New AnchorPoint(50, 100, True, True)})
            End If

            '============================================================================================
            '20150612-VM-The following code is a prototype for links at the intersection of Roles
            '  Currently not implemented. e.g. For RingConstraints on Binary FactTypes
            'If liPositionInRoleGroup = 1 Then
            '    If Me.FactType.Arity = 2 Then
            '        apat1 = New AnchorPattern(New AnchorPoint() { _
            '                New AnchorPoint(50, 0, True, True), _
            '                New AnchorPoint(50, 100, True, True), _
            '                New AnchorPoint(0, 50, True, True), _
            '                New AnchorPoint(100, 0, True, True), _
            '                New AnchorPoint(100, 100, True, True)})
            '    Else
            '        apat1 = New AnchorPattern(New AnchorPoint() { _
            '                New AnchorPoint(50, 0, True, True), _
            '                New AnchorPoint(50, 100, True, True), _
            '                New AnchorPoint(0, 50, True, True)})
            '    End If
            'ElseIf (liPositionInRoleGroup > 1) And (liPositionInRoleGroup < Me.FactType.Arity) Then
            '    apat1 = New AnchorPattern(New AnchorPoint() { _
            '            New AnchorPoint(50, 0, True, True), _
            '            New AnchorPoint(50, 100, True, True)})
            'ElseIf liPositionInRoleGroup = Me.FactType.Arity Then
            '    If Me.FactType.Arity = 2 Then
            '        apat1 = New AnchorPattern(New AnchorPoint() { _
            '                New AnchorPoint(50, 0, True, True), _
            '                New AnchorPoint(100, 50, True, True), _
            '                New AnchorPoint(50, 100, True, True), _
            '                New AnchorPoint(0, 0, True, True), _
            '                New AnchorPoint(0, 100, True, True)})
            '    Else
            '        apat1 = New AnchorPattern(New AnchorPoint() { _
            '                New AnchorPoint(50, 0, True, True), _
            '                New AnchorPoint(100, 50, True, True), _
            '                New AnchorPoint(50, 100, True, True)})
            '    End If
            'End If
            Me.Shape.AnchorPattern = apat1
            If IsSomething(Me.Link) Then
                '-----------------------------------------------------------------------
                'Because this method may be called for Roles that do not yet have links
                '-----------------------------------------------------------------------
                'Reattach the Link to the AnchorPoints of the Role
                '---------------------------------------------------
                Call Me.Link.ReassignAnchorPoints()
            End If

        End Sub

        Public Sub ResetLink()

            Try
                If Me.JoinedORMObject IsNot Nothing Then
                    '-------------------------------------------------------------
                    'Create the link between the Role and the ORMObject that it 
                    '  joins to.
                    '-------------------------------------------------------------        
                    If Me.Page.Diagram Is Nothing Then Exit Sub

                    Me.Page.Diagram.Links.Remove(Me.Link)

                    Me.Page.Diagram.Invalidate()
                    '---------------------------------------------------------------------------------------------------------------------
                    'Create a ShapeNode object because Me.JoinedORMObject is generic FBM.ModelObject and does not have attribute 'Shape'
                    '---------------------------------------------------------------------------------------------------------------------
                    Dim lrJoinedORMObject As Object = Me.JoinedORMObject
                    Dim loShapeNode As MindFusion.Diagramming.ShapeNode = lrJoinedORMObject.Shape
                    loShapeNode.AllowIncomingLinks = True
                    Me.Shape.Locked = False
                    Me.Shape.AllowOutgoingLinks = True
                    Dim loLink As New DiagramLink(Me.Page.Diagram, Me.Shape, loShapeNode)
                    loLink.Locked = True
                    loLink.Pen.Color = Color.Black
                    loLink.Tag = Me
                    Me.Link = loLink
                    Me.Link.Visible = True
                    Me.Link.Pen.Width = 0.3


                    Me.Page.Diagram.Links.Add(loLink)
                    Me.Page.Diagram.Invalidate()

                End If
            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Private Sub Role_MandatoryChanged(ByVal abMandatoryStatus As Boolean) Handles Role.MandatoryChanged

            Try
                Me._Mandatory = abMandatoryStatus
                Call Me.RefreshShape()

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub


        'Private Overloads Sub update_from_model() Handles model.ModelUpdated

        '    If IsSomething(Me.JoinedORMObject) Then
        '        Select Case Me.TypeOfJoin
        '            Case pcenumRoleJoinType.EntityType
        '                Dim lrEntityType As Object = Me.JoinedORMObject
        '                If TypeOf (Me.JoinedORMObject) Is tEntityTypeInstance Then
        '                    Me.JoinsEntityTypeId = lrEntityType.Id
        '                End If
        '            Case pcenumRoleJoinType.ValueType
        '                Dim lrValueType As Object = Me.JoinedORMObject
        '                If TypeOf (Me.JoinedORMObject) Is tValueTypeInstance Then
        '                    Me.JoinsValueTypeId = lrValueType.Id
        '                End If
        '            Case pcenumRoleJoinType.FactType
        '                Dim lrFactType As Object = Me.JoinedORMObject
        '                If TypeOf (Me.JoinedORMObject) Is FBM.tFactTypeInstance Then
        '                    Me.JoinsNestedFactTypeId = lrFactType.Id
        '                End If
        '        End Select
        '    End If

        'End Sub

        'Private Overloads Sub JoinedORMObject_updated() Handles JoinedORMObject.updated

        '    MsgBox("tRoleInstance: JoinedORMObject.updated: FactType:" & Me.FactType.Name)

        '    If IsSomething(Me.JoinedORMObject) Then
        '        Select Case Me.TypeOfJoin
        '            Case pcenumRoleJoinType.EntityType
        '                Dim lrEntityType As Object = Me.JoinedORMObject
        '                If TypeOf (Me.JoinedORMObject) Is tEntityTypeInstance Then
        '                    Me.JoinsEntityTypeId = lrEntityType.Id
        '                End If
        '            Case pcenumRoleJoinType.ValueType
        '                Dim lrValueType As Object = Me.JoinedORMObject
        '                If TypeOf (Me.JoinedORMObject) Is tValueTypeInstance Then
        '                    Me.JoinsValueTypeId = lrValueType.Id
        '                End If
        '            Case pcenumRoleJoinType.FactType
        '                Dim lrFactType As Object = Me.JoinedORMObject
        '                If TypeOf (Me.JoinedORMObject) Is FBM.tFactTypeInstance Then
        '                    Me.JoinsNestedFactTypeId = lrFactType.Id
        '                End If
        '        End Select
        '    End If

        'End Sub

        ''' <summary>
        ''' Handles the event when the Join of a Role changes to a different ModelObject
        ''' </summary>
        ''' <param name="arModelObject"></param>
        ''' <remarks></remarks>
        Private Sub Role_RoleJoinModified(ByRef arModelObject As ModelObject) Handles Role.RoleJoinModified

            Try
                Select Case arModelObject.ConceptType
                    Case Is = pcenumConceptType.EntityType
                        Me.JoinedORMObject = Me.Page.EntityTypeInstance.Find(AddressOf arModelObject.Equals)

                        '---------------------------------------------------------------------------------------------------------
                        'The EntityTypeInstance might not be on the Page, especially if a ValueType has just been converted to
                        '  an EntityType. In that instance, load the EntityTypeInstance to the Page if the EntityType exists
                        '  in the Model.
                        '---------------------------------------------------------------------------------------------------------
                        If Me.JoinedORMObject Is Nothing Then
                            If Me.Model.ExistsModelElement(arModelObject.Id) Then
                                If Me.Model.GetModelObjectByName(arModelObject.Id).GetType Is GetType(FBM.EntityType) Then
                                    Dim lrEntityType As FBM.EntityType
                                    lrEntityType = Me.Model.GetModelObjectByName(arModelObject.Id)
                                    If IsSomething(Me.Page.Diagram) Then
                                        Dim loPointClient As Point
                                        Dim loPoint As PointF

                                        loPointClient = New Point(Me.width / 2, Me.height / 2)
                                        loPoint = Me.Page.DiagramView.ClientToDoc(New Point(loPointClient.X, loPointClient.Y))
                                        Me.JoinedORMObject = Me.Page.DropEntityTypeAtPoint(lrEntityType, loPoint, True)
                                    Else
                                        Dim lrEntityTypeInstance As FBM.EntityTypeInstance
                                        lrEntityTypeInstance = lrEntityType.CloneInstance(Me.Page, True)
                                        lrEntityTypeInstance.X = 5
                                        lrEntityTypeInstance.Y = 5
                                    End If
                                    If Me.Model.Page.Contains(Me.Page) Then
                                        Call Me.Page.Save()
                                    End If
                                End If
                            End If
                        End If

                    Case Is = pcenumConceptType.ValueType

                        If Me.JoinedORMObject IsNot Nothing Then
                            If Me.JoinedORMObject.GetType = GetType(FBM.ValueTypeInstance) Then

                                Dim lrValueType As FBM.ValueType = arModelObject
                                If lrValueType.IsReferenceMode Then
                                    Dim lrValueTypeInstance As FBM.ValueTypeInstance = Me.JoinedORMObject
                                    If lrValueTypeInstance.Shape IsNot Nothing Then
                                        lrValueTypeInstance.Shape.Move(10, 10)
                                        lrValueTypeInstance.Shape.Visible = True
                                    End If
                                End If
                            End If
                        End If

                        Me.JoinedORMObject = Me.Page.ValueTypeInstance.Find(AddressOf arModelObject.Equals)


                        '---------------------------------------------------------------------------------------------------------
                        'The EntityTypeInstance might not be on the Page, especially if a ValueType has just been converted to
                        '  an EntityType. In that instance, load the EntityTypeInstance to the Page if the EntityType exists
                        '  in the Model.
                        '---------------------------------------------------------------------------------------------------------
                        If Me.JoinedORMObject Is Nothing Then
                            If Me.Model.ExistsModelElement(arModelObject.Id) Then
                                If Me.Model.GetModelObjectByName(arModelObject.Id).GetType Is GetType(FBM.EntityType) Then
                                    Dim lrValueType As FBM.ValueType
                                    lrValueType = Me.Model.GetModelObjectByName(arModelObject.Id)
                                    If IsSomething(Me.Page.Diagram) Then
                                        Dim loPointClient As Point
                                        Dim loPoint As PointF

                                        loPointClient = New Point(Me.width / 2, Me.height / 2)
                                        loPoint = Me.Page.DiagramView.ClientToDoc(New Point(loPointClient.X, loPointClient.Y))
                                        Me.JoinedORMObject = Me.Page.DropValueTypeAtPoint(lrValueType, loPoint, True)
                                    Else
                                        Dim lrValueTypeInstance As FBM.ValueTypeInstance
                                        lrValueTypeInstance = lrValueType.CloneInstance(Me.Page, True)
                                        lrValueTypeInstance.X = 5
                                        lrValueTypeInstance.Y = 5
                                    End If
                                    If Me.Model.Page.Contains(Me.Page) Then
                                        Call Me.Page.Save()
                                    End If
                                End If
                            End If
                        End If

                    Case Is = pcenumConceptType.FactType
                        Me.JoinedORMObject = Me.Page.FactTypeInstance.Find(AddressOf arModelObject.Equals)
                End Select

                Call Me.ResetLink()
                Call Me.Page.MakeDirty()
                If Me.Page.Form IsNot Nothing Then
                    Call Me.EnableSaveButton()
                End If

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Public Overrides Sub makeDirty()
            Me.FactType.isDirty = True
            Me.isDirty = True
            Me.Page.MakeDirty()

        End Sub

        Public Sub MouseDown() Implements iPageObject.MouseDown

        End Sub

        Public Sub MouseMove() Implements iPageObject.MouseMove

        End Sub

        Public Sub MouseUp() Implements iPageObject.MouseUp

        End Sub

        Public Sub Moved() Implements iPageObject.Moved

        End Sub

        Public Sub NodeDeleting() Implements iPageObject.NodeDeleting

        End Sub

        Public Sub NodeModified() Implements iPageObject.NodeModified

        End Sub

        Public Sub NodeSelected() Implements iPageObject.NodeSelected

        End Sub

        Public Sub ValidateAnchorPoint() Implements iRolePageObject.ValidateAnchorPoint

        End Sub

        Public Sub NodeDeselected() Implements FBM.iPageObject.NodeDeselected

            Call Me.SetAppropriateColour()

        End Sub

        Public Sub SetAppropriateColour() Implements iPageObject.SetAppropriateColour

            If Me.Shape IsNot Nothing Then
                If Me.Shape.Selected Then
                    Me.Shape.Pen.Color = Color.Blue
                Else
                    Me.Shape.Pen.Color = Color.Black
                End If
            End If

        End Sub

        Public Overloads Sub SetMandatory(ByVal abMandatoryStatus As Boolean)

            Try
                Me._Mandatory = abMandatoryStatus

                Call Me.Role.SetMandatory(abMandatoryStatus, True)

                Call Me.refresh_role_instance()

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Private Sub Role_RoleNameChanged(ByVal asNewRoleName As String) Handles Role.RoleNameChanged

            Me.Name = asNewRoleName
            Call Me.RefreshShape()

        End Sub

        Public Sub RepellNeighbouringPageObjects(ByVal aiDepth As Integer) Implements iPageObject.RepellNeighbouringPageObjects

        End Sub

        Public Sub Move(ByVal aiNewX As Integer, ByVal aiNewY As Integer, ByVal abBroadcastInterfaceEvent As Boolean) Implements iPageObject.Move

        End Sub

        Private Sub Role_ValueRangeChanged(asNewValueRange As String) Handles Role.ValueRangeChanged

            Me.ValueRange = asNewValueRange

        End Sub

        Public Sub EnableSaveButton() Implements iPageObject.EnableSaveButton
            If Me.Page IsNot Nothing Then
                If Me.Page.Form IsNot Nothing Then
                    Call Me.Page.Form.EnableSaveButton()
                End If
            Else
                Try
                    frmMain.ToolStripButton_Save.Enabled = True
                Catch ex As Exception
                End Try
            End If
        End Sub

        Private Sub Role_ValueConstraintAdded(ByVal asValueConstraint As String) Handles Role.ValueConstraintAdded

            Try
                If Not Me.ValueConstraint.Contains(asValueConstraint) Then
                    Me.ValueConstraint.Add(asValueConstraint)
                End If

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Private Sub Role_ValueConstraintModified(asOldValue As String, asNewValue As String) Handles Role.ValueConstraintModified
            Try
                Me.ValueConstraint.Item(Me.ValueConstraint.IndexOf(asOldValue)) = asNewValue

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try
        End Sub

        Private Sub Role_ValueConstraintRemoved(asRemovedValueConstraint As String) Handles Role.ValueConstraintRemoved

            Try
                Me.ValueConstraint.Remove(asRemovedValueConstraint)

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Private Sub RoleConstraintRoleValueConstraint_RemovedFromModel() Handles RoleConstraintRoleValueConstraint.RemovedFromModel
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
    End Class

End Namespace