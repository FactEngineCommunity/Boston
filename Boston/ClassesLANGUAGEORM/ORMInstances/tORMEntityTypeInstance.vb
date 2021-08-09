Imports MindFusion.Diagramming
Imports System.ComponentModel
Imports System.Xml.Serialization
Imports System.Reflection
Imports System.Linq

Namespace FBM
    <DefaultPropertyAttribute("Title")> _
    <Serializable()> _
    Public Class EntityTypeInstance
        Inherits FBM.EntityType
        Implements IEquatable(Of FBM.EntityTypeInstance)
        Implements ICloneable
        Implements FBM.iPageObject

        <XmlAttribute()> _
        Public Shadows ConceptType As pcenumConceptType = pcenumConceptType.EntityType

        <XmlIgnore()> _
        Private WithEvents _EntityType As New FBM.EntityType 'The EntityType for which the EntityTypeIstance acts as View/Proxy.
        <XmlIgnore()>
        <Browsable(False)>
        Public Property EntityType() As FBM.EntityType
            Get
                Return Me._EntityType
            End Get
            Set(ByVal value As FBM.EntityType)
                Me._EntityType = value
                Me.Concept = value.Concept
            End Set
        End Property

        Public Overloads Property Instances As Viev.Strings.StringCollection
            Get
                Return Me.EntityType.Instances
            End Get
            Set(value As Viev.Strings.StringCollection)
                Me.EntityType.Instances = value
            End Set
        End Property

        <XmlElementAttribute()>
        <CategoryAttribute("Name"),
         Browsable(True),
         [ReadOnly](False),
         BindableAttribute(True),
         DefaultValueAttribute(""),
         DesignOnly(False),
         DescriptionAttribute("The name of the Entity Type")>
        Public Shadows Property Name() As String
            Get
                Return _Name
            End Get
            Set(ByVal value As String)
                '-----------------------------------------------------------
                'The surrogate key for the EntityType is about
                '  to change (to match the name of the EntityType)
                '  so update the ModelDictionary entry for the 
                '  Concept/Symbol (the nominalistic idenity of the EntityType
                '-----------------------------------------------------------
                _Name = value
                Me.Id = value
                Me.Symbol = value
            End Set
        End Property

        Private _IsDatabaseReservedWord As Boolean = False

        '<XmlElementAttribute()>
        '<CategoryAttribute("FactEngine"),
        ' Browsable(True),
        ' [ReadOnly](False),
        ' BindableAttribute(True),
        ' DefaultValueAttribute("False"),
        ' DesignOnly(False),
        ' DescriptionAttribute("True if the Name of the Entity Type is a reserved word in the underlying database.")>
        'Public Shadows Property IsDatabaseReservedWord As Boolean
        '    Get
        '        Return Me._IsDatabaseReservedWord
        '    End Get
        '    Set(value As Boolean)
        '        Me._IsDatabaseReservedWord = value
        '    End Set
        'End Property

        <XmlIgnore()> _
        Public Shadows ReferenceModeFactType As FBM.FactTypeInstance = Nothing
        <XmlIgnore()> _
        Public Shadows ReferenceModeValueType As FBM.ValueTypeInstance = Nothing

        <XmlIgnore()> _
        Public Shadows ReferenceModeRoleConstraint As FBM.RoleConstraintInstance = Nothing

        <XmlIgnore()> _
        Public Shadows ObjectifiedFactType As FBM.FactTypeInstance = Nothing

        <XmlIgnore()> _
        Public Shadows SubtypeRelationship As New List(Of FBM.SubtypeRelationshipInstance)

        Public ValueConstraintInstance As ValueConstraintInstance

        <XmlIgnore()> _
        <XmlElementAttribute()> _
        <DebuggerBrowsable(DebuggerBrowsableState.Never)> _
        Public _ExpandReferenceMode As Boolean = False
        <XmlAttribute()> _
        <CategoryAttribute("Entity Type"), _
         DescriptionAttribute("The 'Reference Mode' for the Entity Type")> _
         Public Property ExpandReferenceMode() As Boolean
            Get
                Return _ExpandReferenceMode
            End Get
            Set(ByVal value As Boolean)
                _ExpandReferenceMode = value
            End Set
        End Property

        <XmlIgnore()> _
        <DebuggerBrowsable(DebuggerBrowsableState.Never)> _
        Public Shadows _ReferenceMode As String
        <XmlAttribute()> _
        <CategoryAttribute("Entity Type"), _
         DescriptionAttribute("The 'Reference Mode' for the Entity Type"), _
         TypeConverter(GetType(tMyConverter))> _
        Public Shadows Property ReferenceMode() As String
            Get
                Dim TempString As String = ""
                'Holds our selected option for return

                If _ReferenceMode = Nothing Then
                    'If an option has not already been selected
                    If tGlobalForTypeConverter.OptionStringArray.GetUpperBound(0) > 0 Then
                        'If there is more than 1 option
                        'Sort them alphabetically
                        Array.Sort(tGlobalForTypeConverter.OptionStringArray)
                    End If
                    TempString = tGlobalForTypeConverter.OptionStringArray(0)
                    'Choose the first option (or the empty one)
                Else 'Otherwise, if the option is already selected
                    'Choose the already selected value                    
                    TempString = _ReferenceMode
                End If

                Return TempString
            End Get
            Set(ByVal Value As String)
                Me._ReferenceMode = Value                
            End Set
        End Property

        <XmlIgnore()> _
        Public _DataType As pcenumORMDataType
        <XmlIgnore()> _
        <CategoryAttribute("Entity Type"), _
         Browsable(False), _
         [ReadOnly](False), _
         BindableAttribute(True), _
         DefaultValueAttribute(""), _
         DesignOnly(False), _
         DescriptionAttribute("The 'Data Type' of Entities of this Entity Type."), _
         TypeConverter(GetType(Enumeration.EnumDescConverter))> _
        Public Property DataType() As pcenumORMDataType
            Get
                If Me.EntityType.HasSimpleReferenceScheme Then
                    Try
                        Dim lrTopmostEntityType As FBM.EntityType = Me.EntityType.GetTopmostSupertype
                        Return lrTopmostEntityType.ReferenceModeValueType.DataType
                    Catch ex As Exception
                        prApplication.ThrowErrorMessage(ex.Message, pcenumErrorType.Critical)
                    End Try
                Else
                    Return pcenumORMDataType.DataTypeNotSet
                End If
            End Get
            Set(value As pcenumORMDataType)
                Me._DataType = value
            End Set
        End Property

        <XmlIgnore()> _
        Private _DataTypePrecision As Integer
        <XmlIgnore()> _
        <CategoryAttribute("Entity Type"), _
         Browsable(True), _
         [ReadOnly](False), _
         BindableAttribute(True), _
         DefaultValueAttribute(""), _
         DesignOnly(False), _
         DescriptionAttribute("The 'Data Type Precision' of the Data Type.")> _
        Public Property DataTypePrecision() As Integer
            Get
                If Me.EntityType.HasSimpleReferenceScheme Then
                    Dim lrTopmostEntityType As FBM.EntityType = Me.EntityType.GetTopmostSupertype
                    Return lrTopmostEntityType.ReferenceModeValueType.DataTypePrecision
                Else
                    Return 0
                End If
            End Get
            Set(value As Integer)
                Me._DataTypePrecision = value
            End Set
        End Property

        <XmlIgnore()> _
        Private _DataTypeLength As Integer
        <XmlIgnore()> _
        <CategoryAttribute("Entity Type"), _
         Browsable(True), _
         [ReadOnly](False), _
         BindableAttribute(True), _
         DefaultValueAttribute(""), _
         DesignOnly(False), _
         DescriptionAttribute("The 'Data Type Length' of the Data Type.")> _
        Public Property DataTypeLength() As Integer
            Get
                If Me.EntityType.HasSimpleReferenceScheme Then
                    Dim lrTopmostEntityType As FBM.EntityType = Me.EntityType.GetTopmostSupertype
                    Return lrTopmostEntityType.ReferenceModeValueType.DataTypeLength
                Else
                    Return 0
                End If
            End Get
            Set(value As Integer)
                Me._DataTypeLength = value
            End Set
        End Property

        <XmlIgnore()>
        <Browsable(False),
        [ReadOnly](True),
        BindableAttribute(False)>
        Public Shadows WithEvents Concept As New FBM.Concept

        <XmlIgnore()> _
        Public Page As FBM.Page

        <NonSerialized(), _
        XmlIgnore()> _
        Public Shape As tORMDiagrammingShapeNode ' ShapeNode

        <NonSerialized(), _
        XmlIgnore()> _
        Public EntityTypeNameShape As ShapeNode

        <NonSerialized(), _
        XmlIgnore()> _
        Public ReferenceModeShape As ShapeNode

        <NonSerialized(), _
        XmlIgnore()> _
        Public TableShape As TableNode 'Used in ER Diagrams
        Public _X As Integer
        Public Property X As Integer Implements FBM.iPageObject.X
            Get
                Return Me._X
            End Get
            Set(ByVal value As Integer)
                Me._X = value
                If IsSomething(Me.Shape) Then
                    Dim loRectangle As New Rectangle(Me.X, Me.Shape.Bounds.Y, Me.Shape.Bounds.Width, Me.Shape.Bounds.Height)
                    Me.Shape.SetRect(loRectangle, False)
                End If
            End Set
        End Property

        Public _Y As Integer
        Public Property Y As Integer Implements FBM.iPageObject.Y
            Get
                Return Me._Y
            End Get
            Set(ByVal value As Integer)
                Me._Y = value
                If IsSomething(Me.Shape) Then
                    Dim loRectangle As New Rectangle(Me.Shape.Bounds.X, Me.Y, Me.Shape.Bounds.Width, Me.Shape.Bounds.Height)
                    Me.Shape.SetRect(loRectangle, False)
                End If
            End Set
        End Property

        <XmlIgnore()> _
        <DebuggerBrowsable(DebuggerBrowsableState.Never)> _
        Public _HasBeenMoved As Boolean = False
        <XmlIgnore()> _
        Public Property HasBeenMoved() As Boolean  'Used in AutoLayout operations.
            Get
                Return Me._HasBeenMoved
            End Get
            Set(ByVal value As Boolean)
                Me._HasBeenMoved = value
                If Me.Shape IsNot Nothing Then
                    Me.X = Me.Shape.Bounds.X
                    Me.Y = Me.Shape.Bounds.Y
                End If
            End Set
        End Property

        <NonSerialized(), _
        XmlIgnore()> _
        Public OutgoingLink As New List(Of DiagramLink) 'For when the EntityType is an Actor in a UseCaseDiagram etc

        <NonSerialized(), _
        XmlIgnore()> _
        Public IncomingLink As New List(Of DiagramLink) 'For when the EntityType is a Process in a UseCaseDiagram etc

        Sub New()

            Me.ConceptType = pcenumConceptType.EntityType

            'Call Me.SetPropertyAttributes()

            Me.m_dctd = DynamicTypeDescriptor.ProviderInstaller.Install(Me)

        End Sub

        Public Sub New(ByRef arModel As FBM.Model, ByVal aiLanguageId As pcenumLanguage, Optional ByVal as_entity_type_name As String = Nothing, Optional ByVal ab_use_name_as_id As Boolean = False, Optional ByVal aiX As Integer = 0, Optional ByVal aiY As Integer = 0)

            Call Me.New()

            Me.Model = arModel

            If ab_use_name_as_id Then
                Me.Id = Trim(Me.Name)
            Else
                Me.Id = System.Guid.NewGuid.ToString
            End If

            If IsSomething(as_entity_type_name) Then
                Me.Name = as_entity_type_name
            Else
                Me.Name = "New Entity Type"
            End If

            Me.X = aiX
            Me.Y = aiY

            'Call Me.SetPropertyAttributes()


        End Sub

        Public Shadows Function Equals(ByVal other As FBM.EntityTypeInstance) As Boolean Implements System.IEquatable(Of FBM.EntityTypeInstance).Equals

            If Me.Id = other.Id Then
                Return True
            Else
                Return False
            End If

        End Function

        Public Function EqualsActor(ByVal ar_entity_type_instance As FBM.EntityTypeInstance) As Boolean
            '------------------------------------------------------------
            'Use the EntityType referenced from the EntityTypeInstance
            '  because we do not 'show' the 'Process' EntityType in
            '  a UseCaseModel (for instance).
            '  In an ORM model, it wouldn't matter, but it is safer
            '  to use the Model.
            '
            'This obviously necessitates that when a User creates a 
            '  SubType relationship...that it is imediately reflected
            '  in the Model as well as the Page. This is the case
            '  with just about all Richmond modeling. 'Undo' issues are
            '  seperate from this and not really a concern. If the User
            '  is unhappy with what they just did...they should not save
            '  the results.
            '------------------------------------------------------------
            If ar_entity_type_instance.EntityType.parentModelObjectList.Count > 0 Then
                If ar_entity_type_instance.EntityType.parentModelObjectList(0).Id = pcenumCoreEntityType.Actor.ToString Then
                    Return True
                Else
                    Return False
                End If
            Else
                Return False
            End If


        End Function

        Public Function EqualsProcess(ByVal ar_entity_type_instance As FBM.EntityTypeInstance) As Boolean
            '------------------------------------------------------------
            'Use the EntityType referenced from the EntityTypeInstance
            '  because we do not 'show' the 'Process' EntityType in
            '  a UseCaseModel (for instance).
            '  In an ORM model, it wouldn't matter, but it is safer
            '  to use the Model.
            '
            'This obviously necessitates that when a User creates a 
            '  SubType relationship...that it is imediately reflected
            '  in the Model as well as the Page. This is the case
            '  with just about all Richmond modeling. 'Undo' issues are
            '  seperate from this and not really a concern. If the User
            '  is unhappy with what they just did...they should not save
            '  the results.
            '------------------------------------------------------------
            If ar_entity_type_instance.EntityType.parentModelObjectList.Count > 0 Then
                If ar_entity_type_instance.EntityType.parentModelObjectList(0).Id = pcenumCoreEntityType.Process.ToString Then
                    Return True
                Else
                    Return False
                End If
            Else
                Return False
            End If

        End Function

        Public Shared Function CompareTotalLinks(ByVal ao_a As FBM.EntityTypeInstance, ByVal ao_b As FBM.EntityTypeInstance) As Integer

            Return StrComp(ao_a.TotalLinks, ao_b.TotalLinks)

        End Function

        Public Overloads Function Clone(ByRef arPage As FBM.Page, _
                                        Optional ByVal abAddToModel As Boolean = False, _
                                        Optional ByVal abIsMDAModelElement As Boolean = False,
                                        Optional ByVal abAddToPage As Boolean = False) As Object

            Dim lrEntityTypeInstance As New FBM.EntityTypeInstance

            Try
                If arPage.EntityTypeInstance.Find(Function(x) x.Id = Me.Id) IsNot Nothing Then
                    lrEntityTypeInstance = arPage.EntityTypeInstance.Find(Function(x) x.Id = Me.Id)
                Else
                    With Me
                        lrEntityTypeInstance.isDirty = True
                        lrEntityTypeInstance.Model = arPage.Model
                        lrEntityTypeInstance.Page = arPage
                        lrEntityTypeInstance.Symbol = .Symbol
                        lrEntityTypeInstance.Id = .Id
                        lrEntityTypeInstance.Name = .Name
                        If abIsMDAModelElement = False Then
                            lrEntityTypeInstance.IsMDAModelElement = .IsMDAModelElement
                        Else
                            lrEntityTypeInstance.IsMDAModelElement = abIsMDAModelElement
                        End If

                        lrEntityTypeInstance.EntityType = .EntityType.Clone(arPage.Model, abAddToModel, abIsMDAModelElement)

                        If arPage.Model.EntityType.Exists(AddressOf .EntityType.Equals) Then
                            lrEntityTypeInstance.EntityType = arPage.Model.EntityType.Find(AddressOf .EntityType.Equals)
                        Else
                            Dim lrEntityType As New FBM.EntityType
                            lrEntityType = .EntityType.Clone(arPage.Model)
                            arPage.Model.AddEntityType(lrEntityType)
                            lrEntityTypeInstance.EntityType = lrEntityType
                        End If

                        lrEntityTypeInstance.ReferenceMode = .ReferenceMode
                        lrEntityTypeInstance.ExpandReferenceMode = .ExpandReferenceMode
                        lrEntityTypeInstance.ValueConstraint = .ValueConstraint

                        lrEntityTypeInstance.IsObjectifyingEntityType = .IsObjectifyingEntityType

                        If .ReferenceModeValueType IsNot Nothing Then
                            lrEntityTypeInstance.ReferenceModeValueType = .ReferenceModeValueType.Clone(arPage, .ReferenceModeValueType.ValueType.IsMDAModelElement, True)
                        End If

                        If abAddToPage Then 'Do this here otherwise cloning the ReferenceModeFactType (below) will recursively clone the EntityType.
                            arPage.EntityTypeInstance.AddUnique(lrEntityTypeInstance)
                        End If

                        If .ReferenceModeFactType IsNot Nothing Then
                            lrEntityTypeInstance.ReferenceModeFactType = .ReferenceModeFactType.Clone(arPage,
                                                                                                      True,
                                                                                                      .ReferenceModeFactType.FactType.IsMDAModelElement)

                            For Each lrRoleConstraintInstance In .ReferenceModeFactType.InternalUniquenessConstraint
                                Call lrRoleConstraintInstance.Clone(arPage, lrRoleConstraintInstance.IsMDAModelElement, True)
                            Next
                        End If

                        lrEntityTypeInstance.OutgoingLink = New List(Of DiagramLink)

                        lrEntityTypeInstance.X = .X
                        lrEntityTypeInstance.Y = .Y
                    End With
                End If

                Return lrEntityTypeInstance

            Catch ex As Exception
                Dim lsMessage As String = ""

                lsMessage = "Error: tEntityTypeInstance.Clone: " & vbCrLf & vbCrLf & ex.Message
                Call prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)

                Return lrEntityTypeInstance
            End Try

        End Function

        Public Function ClonePageObject() As FBM.PageObject

            Dim lrPageObject As New FBM.PageObject

            lrPageObject.Name = Me.Name
            lrPageObject.Shape = Me.Shape

            Return lrPageObject

        End Function

        Public Function CloneConceptInstance() As FBM.ConceptInstance

            Dim lrConceptInstance As New FBM.ConceptInstance(Me.Model, Me.Page, Me.Id, Me.ConceptType)

            lrConceptInstance.X = Me.X
            lrConceptInstance.Y = Me.Y

            Return lrConceptInstance

        End Function

        Public Sub MouseDown() Implements FBM.iPageObject.MouseDown

        End Sub

        Public Sub MouseMove() Implements FBM.iPageObject.MouseMove

        End Sub

        Public Sub MouseUp() Implements FBM.iPageObject.MouseUp

        End Sub

        Public Sub Moved() Implements FBM.iPageObject.Moved

            Try
                Me.X = Me.Shape.Bounds.X
                Me.Y = Me.Shape.Bounds.Y

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try


        End Sub

        Public Sub NodeDeleting() Implements FBM.iPageObject.NodeDeleting

        End Sub

        Public Sub NodeModified() Implements FBM.iPageObject.NodeModified

        End Sub

        Public Sub NodeSelected() Implements FBM.iPageObject.NodeSelected

            Call Me.SetAppropriateColour()

        End Sub

        ''' <summary>
        ''' AutoLayouts to FactTypes associated with the ModelObject and (optionally) recursively all ModelObjects at the other end of Binary FactTypes associated with the ModelObject.
        ''' </summary>
        ''' <param name="abJustThisModelObject">If True, only the FactTypes associated with the ModelObject are AutoLayedout</param>
        ''' <remarks></remarks>
        Public Sub AutoLayout(Optional ByVal abJustThisModelObject As Boolean = False, Optional ByVal abRepellNeighbouringObjects As Boolean = False)

            Dim lrLink As MindFusion.Diagramming.DiagramLink
            Dim liX As Integer
            Dim liY As Integer
            Dim liDegrees As Integer = 315

            Try
                For Each lrLink In Me.Shape.IncomingLinks
                    Dim lrFactTypeInstance As FBM.FactTypeInstance

                    If lrLink.Tag.ConceptType = pcenumConceptType.Role Then

                        Dim lrRoleInstance As FBM.RoleInstance
                        lrRoleInstance = lrLink.Tag
                        lrFactTypeInstance = lrRoleInstance.FactType

                        If Not lrFactTypeInstance.HasBeenMoved Then
                            liX = Viev.Greater(10, Me.Shape.Bounds.X + Math.Cos(liDegrees * (Math.PI / 180)) * 20)
                            liY = Viev.Greater(10, Me.Shape.Bounds.Y + Math.Sin(liDegrees * (Math.PI / 180)) * 20)
                            lrFactTypeInstance.Shape.Move(liX, liY)
                            lrFactTypeInstance.HasBeenMoved = True
                        End If

                        If abJustThisModelObject Then
                            '------------------------------
                            'Don't recursively AutoLayout
                            '------------------------------
                            If lrFactTypeInstance.IsBinaryFactType Then
                                Dim lrObject As Object
                                lrObject = lrFactTypeInstance.GetOtherRoleOfBinaryFactType(lrRoleInstance.Id).JoinedORMObject

                                liX = Viev.Lesser(Me.Shape.Bounds.X, lrObject.Shape.Bounds.X) + Math.Abs(Me.Shape.Bounds.X - lrObject.Shape.Bounds.X) / 2
                                liY = Viev.Lesser(Me.Shape.Bounds.Y, lrObject.Shape.Bounds.y) + Math.Abs(Me.Shape.Bounds.Y - lrObject.Shape.Bounds.Y) / 2

                                lrFactTypeInstance.Shape.Move(liX, liY)

                            End If
                        Else
                            If lrFactTypeInstance.IsBinaryFactType Then
                                Dim lrObject As Object
                                lrObject = lrFactTypeInstance.GetOtherRoleOfBinaryFactType(lrRoleInstance.Id).JoinedORMObject
                                If Not lrObject.HasBeenMoved Then
                                    liX = Viev.Greater(40, Me.Shape.Bounds.X + Math.Cos(liDegrees * (Math.PI / 180)) * 80)
                                    liY = Viev.Greater(40, Me.Shape.Bounds.Y + Math.Sin(liDegrees * (Math.PI / 180)) * 80)
                                    lrObject.Shape.Move(liX, liY)
                                    lrObject.HasBeenMoved = True
                                    If (lrObject.ConceptType = pcenumConceptType.EntityType) Then

                                        Dim lrChildEntityTypeInstance As FBM.EntityTypeInstance
                                        lrChildEntityTypeInstance = lrObject

                                        Call lrChildEntityTypeInstance.AutoLayout()

                                    End If
                                End If

                            End If
                        End If 'abJustThisModelObject
                        lrFactTypeInstance.SortRoleGroup()
                        liDegrees += 45
                    End If
                Next

                If abRepellNeighbouringObjects Then
                    Call Me.RepellNeighbouringPageObjects(1)
                End If

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Public Shadows Sub AddDataInstance(ByVal asDataInstance As String)

            Me.Instance.AddUnique(asDataInstance)

            If IsSomething(Me.SubtypeRelationship) Then
                Dim lrSubtypeInstance As FBM.SubtypeRelationshipInstance
                For Each lrSubtypeInstance In Me.SubtypeRelationship
                    lrSubtypeInstance.parentEntityType.AddDataInstance(asDataInstance)
                Next
            End If

        End Sub

        Public Overridable Overloads Sub AddSubtypeConstraint(ByVal arParentEntityTypeInstance As FBM.EntityTypeInstance)

            Dim lrSubtypeConstraint As New FBM.tSubtypeRelationship

            '----------------------------------------
            'Create a Model level SubtypeConstraint
            '----------------------------------------
            With New WaitCursor
                lrSubtypeConstraint = Me.EntityType.CreateSubtypeRelationship(arParentEntityTypeInstance.EntityType)
                Me.Model.Save(False)
            End With
        End Sub

        Public Sub DisplayAndAssociate()

            Dim loDroppedNode As New ShapeNode
            Dim loDroppedNameNode As New ShapeNode
            Dim loDroppedReferenceModeNode As New ShapeNode
            Dim loEntityTypeNameStringSize As New SizeF
            Dim loReferenceModeStringSize As New SizeF
            Dim G As Graphics
            Dim loEntityWidth As New SizeF
            Dim loEntityNameWidth As New SizeF
            Dim loReferenceModeWidth As New SizeF
            Dim liGreaterWidth As Integer = 0

            Try
                If Me.EntityType.IsObjectifyingEntityType Then
                    '--------------------------------------------------------
                    'Objectifying EntityTypes are hidden and not displayed.
                    '--------------------------------------------------------
                Else
                    '---------------------------------------------------------------------
                    'Create a Shape for the EntityTypeInstance on the DiagramView object
                    '---------------------------------------------------------------------           
                    loDroppedNode = New tORMDiagrammingShapeNode() ' Me.Page.Diagram.Factory.CreateShapeNode(Me.X, Me.Y, 2, 2)
                    loDroppedNode.Move(Me.X, Me.Y)
                    loDroppedNode.Shape = Shapes.RoundRect
                    loDroppedNode.HandlesStyle = HandlesStyle.InvisibleMove
                    loDroppedNode.AllowOutgoingLinks = True
                    loDroppedNode.AllowIncomingLinks = True
                    loDroppedNode.Resize(20, 12)
                    'loDroppedNode.Tag = New FBM.EntityTypeInstance
                    loDroppedNode.Tag = Me
                    loDroppedNode.ShadowColor = Color.White
                    'loDroppedNode.ShadowOffsetX = 1
                    'loDroppedNode.ShadowOffsetY = 1
                    'loDroppedNode.ShadowColor = Color.LightGray
                    loDroppedNode.Pen.Width = 0.5 '0.4
                    loDroppedNode.Pen.Color = Color.Navy
                    loDroppedNode.ToolTip = "Entity Type"

                    Me.Page.Diagram.Nodes.Add(loDroppedNode)

                    Me.Shape = loDroppedNode

                    '---------------------------------------------
                    'Create the ShapeNode for the EntityTypeName
                    '---------------------------------------------
                    G = Me.Page.Form.CreateGraphics
                    Dim lsETNameText As String = Me.Name
                    If Me.EntityType.IsIndependent Or Me.EntityType.IsDerived Then
                        lsETNameText &= "M"
                    End If

                    If Me.EntityType.IsDerived Then
                        lsETNameText &= "*"
                    End If

                    If Me.IsIndependent Then
                        lsETNameText &= "!"
                    End If

                    loEntityTypeNameStringSize = Me.Page.Diagram.MeasureString(Trim(lsETNameText), Me.Page.Diagram.Font, 1000, System.Drawing.StringFormat.GenericDefault)

                    loDroppedNameNode = Me.Page.Diagram.Factory.CreateShapeNode(Me.X + 2, Me.Y + 1, loEntityTypeNameStringSize.Width + 9, loEntityTypeNameStringSize.Height + 1, Shapes.Rectangle)
                    loDroppedNameNode.HandlesStyle = HandlesStyle.InvisibleMove
                    loDroppedNameNode.AllowOutgoingLinks = False
                    loDroppedNameNode.AllowIncomingLinks = True
                    loDroppedNameNode.Pen = New MindFusion.Drawing.Pen(Color.White, 0.0002)
                    loDroppedNameNode.Visible = True
                    loDroppedNameNode.Tag = New FBM.EntityTypeName(Me)
                    loDroppedNameNode.Text = lsETNameText

                    Me.EntityTypeNameShape = loDroppedNameNode

                    Dim loRectangle As New Rectangle(Me.X, Me.Y, Me.EntityTypeNameShape.Bounds.Width + 4, Me.Shape.Bounds.Height)
                    Me.Shape.SetRect(loRectangle, False)

                    loDroppedNameNode.AttachTo(Me.Shape, AttachToNode.TopCenter)
                    Me.Page.Diagram.Nodes.Add(loDroppedNameNode)

                    '--------------------------------------------------------------
                    'Create the ShapeNode for the ReferenceMode of the EntityType
                    '--------------------------------------------------------------            
                    Dim lsReferenceMode As String = ""

                    liGreaterWidth = Viev.Greater(Me.Name.Length, Me.ReferenceMode.Length)

                    If IsSomething(Me.ReferenceModeValueType) Then
                        lsReferenceMode = "(" & Me.ReferenceMode & ")"
                    Else
                        lsReferenceMode = ""
                    End If

                    loReferenceModeStringSize = Me.Page.Diagram.MeasureString(lsReferenceMode + "M", Me.Page.Diagram.Font, 1000, System.Drawing.StringFormat.GenericDefault)

                    loDroppedReferenceModeNode = Me.Page.Diagram.Factory.CreateShapeNode(Me.X + (Me.Shape.Bounds.Width / 2) - (loReferenceModeStringSize.Width / 2), Me.Y + 6, loReferenceModeStringSize.Width, loReferenceModeStringSize.Height, Shapes.Rectangle)
                    loDroppedReferenceModeNode.Move(Me.X + (liGreaterWidth / 2) - (loDroppedReferenceModeNode.Bounds.Width / 2), Me.Y + 6)
                    loDroppedReferenceModeNode.HandlesStyle = HandlesStyle.Invisible
                    loDroppedReferenceModeNode.AllowOutgoingLinks = False
                    loDroppedReferenceModeNode.AllowIncomingLinks = False
                    loDroppedReferenceModeNode.Pen = New MindFusion.Drawing.Pen(Color.White, 0.0002)
                    loDroppedReferenceModeNode.Visible = True
                    loDroppedReferenceModeNode.Tag = Me
                    loDroppedReferenceModeNode.Text = lsReferenceMode

                    Me.ReferenceModeShape = loDroppedReferenceModeNode

                    If loEntityTypeNameStringSize.Width > loReferenceModeStringSize.Width Then
                        '------------------------------------------------------------
                        'All good, the Width of the EntityType.Snape(Node) is okay.
                        '------------------------------------------------------------
                    Else
                        loRectangle = New Rectangle(Me.X, Me.Y, Me.ReferenceModeShape.Bounds.Width + 4, Me.Shape.Bounds.Height)
                        Me.Shape.SetRect(loRectangle, False)
                    End If

                    loDroppedReferenceModeNode.AttachTo(Me.Shape, AttachToNode.TopCenter)
                    loDroppedReferenceModeNode.Locked = True

                    If IsSomething(Me.ReferenceModeValueType) Then

                        If Me.ReferenceModeValueType.ValueConstraint.Count > 0 Then

                            Me.ValueConstraintInstance = New FBM.ValueConstraintInstance(Me.ReferenceModeValueType, Me)

                            Me.ValueConstraintInstance.Text = Me.ReferenceModeValueType.EnumerateValueConstraint
                            Call Me.ValueConstraintInstance.DisplayAndAssociate(Me)

                        End If

                    End If

                    '---------------------------------------------------------------------------------------------------------------
                    'Set the size of the EnityTypeInstance.Shape based on whether a ReferenceMode exists for the EntityType or not
                    '---------------------------------------------------------------------------------------------------------------
                    liGreaterWidth = Greater(Me.Name.Length, lsReferenceMode.Length)
                    If Me.Name.Length = liGreaterWidth Then
                        liGreaterWidth = Me.EntityTypeNameShape.Bounds.Width + 4
                        loEntityWidth = Me.Page.Diagram.MeasureString(Trim(Me.Name) + "MMMI", Me.Page.Diagram.Font, 1000, System.Drawing.StringFormat.GenericDefault)
                    Else
                        liGreaterWidth = Me.ReferenceModeShape.Bounds.Width + 4
                        loEntityWidth = Me.Page.Diagram.MeasureString(Trim(Me.ReferenceMode) + "MMMI", Me.Page.Diagram.Font, 1000, System.Drawing.StringFormat.GenericDefault)
                    End If
                    loEntityNameWidth = Me.Page.Diagram.MeasureString(Trim(lsETNameText) + ".", Me.Page.Diagram.Font, 1000, System.Drawing.StringFormat.GenericDefault)
                    loReferenceModeWidth = Me.Page.Diagram.MeasureString(Trim(Me.ReferenceMode) + "M",
                                                                         Me.Page.Diagram.Font,
                                                                         1000,
                                                                         System.Drawing.StringFormat.GenericDefault)

                    '-----------------------------------------------------------------------------------
                    'Set the Rectangle for the whole EntityType and the ReferenceModeShape (if needed)
                    '-----------------------------------------------------------------------------------
                    If Me.HasSimpleReferenceScheme And Not Me.EntityType.IsSubtype Then
                        loRectangle = New Rectangle(Me.X, Me.Y, loEntityWidth.Width, 12)
                        Me.Shape.SetRect(loRectangle, False)
                        loRectangle = New Rectangle(Me.X + 2, Me.ReferenceModeShape.Bounds.Y, loReferenceModeWidth.Width, loReferenceModeWidth.Height + 1)
                        Me.ReferenceModeShape.SetRect(loRectangle, False)
                        Me.ReferenceModeShape.Visible = True
                        Me.ReferenceModeShape.Text = "(" & Me.ReferenceMode & ")"
                        Me.ReferenceModeShape.Move(Me.X + (Me.Shape.Bounds.Width / 2) - (Me.ReferenceModeShape.Bounds.Width / 2), _
                                                   Me.Y + 6)
                    Else
                        loRectangle = New Rectangle(Me.X, Me.Y, loEntityWidth.Width, 8)
                        Me.Shape.SetRect(loRectangle, False)
                        Me.ReferenceModeShape.Visible = False
                    End If

                    '------------------------------------------------
                    'Set the rectangle for the EntityTypeNameShape.
                    '------------------------------------------------
                    loRectangle = New Rectangle(Me.X + 2, Me.EntityTypeNameShape.Bounds.Y, loEntityNameWidth.Width, Me.EntityTypeNameShape.Bounds.Height)
                    Me.EntityTypeNameShape.SetRect(loRectangle, False)


                End If 'IsObjectifyingEntityType

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try


        End Sub

        Public Overrides Function HasPrimaryReferenceScheme() As Boolean

            If Me.EntityType.HasPrimaryReferenceScheme Then
                Return True
            Else
                Return False
            End If

        End Function

        Public Overrides Function HasSimpleReferenceScheme() As Boolean

            Return Me.EntityType.HasSimpleReferenceScheme

        End Function

        Public Function HasSubTypes() As Boolean

            HasSubTypes = False

            HasSubTypes = Me.Model.EntityType.Exists(Function(x) x.parentModelObjectList.Contains(Me))

        End Function

        Public Sub HideTheReferenceScheme()

            Try
                Dim loRectangle As Rectangle

                Dim larFactTypeInstance = From FactTypeInstance In Me.Page.FactTypeInstance _
                                          From Role In FactTypeInstance.FactType.RoleGroup _
                                          Where Role.JoinedORMObject.Id = Me.Id _
                                          And FactTypeInstance.isPreferredReferenceMode = True _
                                          Select FactTypeInstance Distinct


                Me.ReferenceModeShape.Visible = True

                '---------------------------------------------------------------------------------------------------------------
                'Set the size of the EnityTypeInstance.Shape based on whether a ReferenceMode exists for the EntityType or not
                '---------------------------------------------------------------------------------------------------------------                                    
                If IsSomething(Me.EntityType.ReferenceModeValueType) Then
                    loRectangle = New Rectangle(Me.X, Me.Y, Me.EntityTypeNameShape.Bounds.Width + 4, 12)
                    Me.Shape.SetRect(loRectangle, False)
                Else
                    loRectangle = New Rectangle(Me.X, Me.Y, Me.EntityTypeNameShape.Bounds.Width + 4, 8)
                    Me.Shape.SetRect(loRectangle, False)
                End If

                '----------------------------------------------------------------------------------------
                'Hide the FactType/ValueType representing the ReferenceMode for the EntityType
                '----------------------------------------------------------------------------------------
                For Each lrFactTypeInstance In larFactTypeInstance
                    lrFactTypeInstance.Shape.Visible = False
                    lrFactTypeInstance.FactTable.TableShape.Visible = False
                    lrFactTypeInstance.FactTypeNameShape.Visible = False
                    If IsSomething(lrFactTypeInstance.FactTypeReadingShape.Shape) Then
                        lrFactTypeInstance.FactTypeReadingShape.Shape.Visible = False
                    End If
                    Dim lrRoleInstance As FBM.RoleInstance
                    Dim lrRoleConstraintInstance As FBM.RoleConstraintInstance
                    Dim lrRoleConstraintRoleInstance As FBM.RoleConstraintRoleInstance
                    For Each lrRoleInstance In lrFactTypeInstance.RoleGroup
                        lrRoleInstance.Shape.Visible = False
                        lrRoleInstance.Link.Visible = False
                        For Each lrRoleConstraintInstance In lrRoleInstance.RoleConstraint
                            For Each lrRoleConstraintRoleInstance In lrRoleConstraintInstance.RoleConstraintRole
                                lrRoleConstraintRoleInstance.Shape.Visible = False
                            Next
                        Next
                        If lrRoleInstance.TypeOfJoin = pcenumRoleJoinType.ValueType Then
                            Dim lrValueTypeInstance As FBM.ValueTypeInstance = lrRoleInstance.JoinedORMObject
                            lrValueTypeInstance.Shape.Visible = False
                            lrValueTypeInstance._ValueConstraint.Shape.Visible = False
                        End If
                    Next
                    For Each lrRoleConstraintInstance In lrFactTypeInstance.InternalUniquenessConstraint
                        For Each lrRoleConstraintRoleInstance In lrRoleConstraintInstance.RoleConstraintRole
                            lrRoleConstraintRoleInstance.Shape.Visible = False
                        Next
                    Next
                Next

                Me.ExpandReferenceMode = False
                Call Me.RefreshShape()

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Public Sub ExpandTheReferenceScheme()

            Try
                Dim loRectangle As Rectangle

                Dim larFactTypeInstance = From FactTypeInstance In Me.Page.FactTypeInstance _
                                          From Role In FactTypeInstance.FactType.RoleGroup _
                                          Where Role.JoinedORMObject.Id = Me.Id _
                                          And FactTypeInstance.isPreferredReferenceMode = True _
                                          Select FactTypeInstance Distinct

                Me.ReferenceModeShape.Visible = False

                loRectangle = New Rectangle(Me.X, Me.Y, Me.EntityTypeNameShape.Bounds.Width + 4, 8)
                Me.Shape.SetRect(loRectangle, False)

                '----------------------------------------------------------------------------------------
                'Expand (show) the FactType/ValueType representing the ReferenceMode for the EntityType
                '----------------------------------------------------------------------------------------
                For Each lrFactTypeInstance In larFactTypeInstance
                    If lrFactTypeInstance.IsObjectified Then
                        lrFactTypeInstance.Shape.Visible = True
                    End If
                    lrFactTypeInstance.X = Me.X + (2 * Me.Shape.Bounds.Width) - (lrFactTypeInstance.Shape.Bounds.Width / 2)
                    lrFactTypeInstance.Y = Me.Y - Me.Shape.Bounds.Height
                    lrFactTypeInstance.Shape.Visible = True
                    lrFactTypeInstance.FactTable.TableShape.Visible = False
                    lrFactTypeInstance.SetAppropriateColour()

                    loRectangle = New Rectangle(lrFactTypeInstance.X - 20, _
                                                lrFactTypeInstance.Y - 7, _
                                                lrFactTypeInstance.FactTypeNameShape.Bounds.Width + 4, _
                                                lrFactTypeInstance.FactTypeNameShape.Bounds.Height)

                    If IsSomething(lrFactTypeInstance.FactTypeReadingShape.Shape) Then
                        lrFactTypeInstance.FactTypeReadingShape.Shape.Visible = True
                    End If
                    Dim lrRoleInstance As FBM.RoleInstance
                    Dim lrRoleConstraintInstance As FBM.RoleConstraintInstance
                    Dim lrRoleConstraintRoleInstance As FBM.RoleConstraintRoleInstance
                    For Each lrRoleInstance In lrFactTypeInstance.RoleGroup
                        lrRoleInstance.Shape.Visible = True
                        lrRoleInstance.Link.Visible = True
                        For Each lrRoleConstraintInstance In lrRoleInstance.RoleConstraint
                            For Each lrRoleConstraintRoleInstance In lrRoleConstraintInstance.RoleConstraintRole
                                lrRoleConstraintRoleInstance.Shape.Visible = True
                            Next
                        Next
                        If lrRoleInstance.TypeOfJoin = pcenumRoleJoinType.ValueType Then
                            Dim lrValueTypeInstance As FBM.ValueTypeInstance = lrRoleInstance.JoinedORMObject
                            lrValueTypeInstance.X = Me.X + (3 * Me.Shape.Bounds.Width)
                            lrValueTypeInstance.Y = Me.Y

                            Call Me.getBlankCellCloseBy(lrValueTypeInstance.X, lrValueTypeInstance.Y, 40)

                            lrValueTypeInstance.Shape.Visible = True
                            lrValueTypeInstance._ValueConstraint.Shape.Visible = True

                            '------------------------------------------------------------------------------------
                            'Code Safe: If the EntityType/Instance.PreferredIdentifierRCId is not set, then set it
                            '------------------------------------------------------------------------------------
                            If Trim(Me.PreferredIdentifierRCId) = "" Then
                                If lrRoleInstance.RoleConstraint.Count > 0 Then
                                    Me.PreferredIdentifierRCId = lrRoleInstance.RoleConstraint(0).Id
                                    Me.EntityType.PreferredIdentifierRCId = lrRoleInstance.RoleConstraint(0).Id
                                End If
                            End If
                        End If
                    Next
                    For Each lrRoleConstraintInstance In lrFactTypeInstance.InternalUniquenessConstraint
                        For Each lrRoleConstraintRoleInstance In lrRoleConstraintInstance.RoleConstraintRole
                            lrRoleConstraintRoleInstance.Shape.Visible = True
                        Next
                    Next

                    Me.SetAdjoinedFactTypesBetweenModelElements(lrFactTypeInstance)

                    lrFactTypeInstance.SortRoleGroup()
                    Me.Page.Invalidate()
                Next

                Me.ExpandReferenceMode = True
                Call Me.RefreshShape()

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Public Overrides Function GetAdjoinedRoles(Optional abIgnoreReferenceModeFactTypes As Boolean = False) As List(Of FBM.Role)

            Try
                Dim lrRole As FBM.Role
                Dim larReturnRoles As New List(Of FBM.Role)

                Dim larRoles = From FactType In Me.Page.FactTypeInstance.FindAll(Function(x) x.isPreferredReferenceMode = Not abIgnoreReferenceModeFactTypes) _
                               From Role In FactType.RoleGroup _
                              Where Role.JoinedORMObject.Id = Me.Id _
                             Select Role

                For Each lrRole In larRoles
                    larReturnRoles.Add(lrRole)
                Next
                Return larReturnRoles

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
        ''' Returns the Model level ModelObject for this Instance.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overrides Function getBaseModelObject() As FBM.ModelObject

            Return Me.EntityType

        End Function

        Public Sub getBlankCellCloseBy(ByRef aiX As Integer, ByRef aiY As Integer, Optional aiDistance As Integer = 30)

            Dim laiArray(2, 2) As Integer
            Dim liRangeX1, liRangeX2 As Integer
            Dim liRangeY1, liRangeY2 As Integer

            For liX = 0 To 2
                For liY = 0 To 2

                    Select Case liX
                        Case Is = 0
                            liRangeX1 = Me.X - 70
                            liRangeX2 = Me.X
                        Case Is = 1
                            liRangeX1 = Me.X + 1
                            liRangeX2 = Me.X + 29
                        Case Is = 2
                            liRangeX1 = Me.X + 30
                            liRangeX2 = Me.X + 90
                    End Select

                    Select Case liY
                        Case Is = 0
                            liRangeY1 = Me.Y - 50
                            liRangeY2 = Me.Y
                        Case Is = 1
                            liRangeY1 = Me.Y + 1
                            liRangeY2 = Me.Y + 15
                        Case Is = 2
                            liRangeY1 = Me.Y + 16
                            liRangeY2 = Me.Y + 80
                    End Select

                    Dim larObjectsInCell = From ModelElement In Me.Page.GetAllPageObjects _
                                           Where (ModelElement.X > liRangeX1) And (ModelElement.X < liRangeX2) _
                                           And (ModelElement.Y > liRangeY1) And (ModelElement.Y < liRangeY2) _
                                           And ModelElement.Id <> Me.Id
                                           Select ModelElement

                    laiArray(liX, liY) = larObjectsInCell.ToList.Count

                Next
            Next

            If Me.Y < 20 Then
                For liBlank = 0 To 2
                    laiArray(0, liBlank) = 1000
                Next
            End If

            Dim liLowestCount As Integer = 0
            Dim liLowestX, liLowestY As Integer
            liLowestCount = laiArray(0, 0)
            For liX = 0 To 2
                For liY = 0 To 2
                    If (liX = 1) And (liY = 1) Then
                        'Skip the centre cell
                    Else
                        If laiArray(liX, liY) < liLowestCount Then
                            liLowestCount = laiArray(liX, liY)
                            liLowestX = liX
                            liLowestY = liY
                        End If

                        If liLowestCount = 0 Then
                            Exit For
                        End If
                    End If
                Next
                If liLowestCount = 0 Then
                    Exit For
                End If
            Next

            Dim liNewX, liNewY As Integer
            Select Case liLowestX
                Case Is = 0
                    liNewX = Me.X - aiDistance
                Case Is = 1
                    liNewX = Me.X + 2
                Case Is = 2
                    liNewX = Me.X + 10 + aiDistance
            End Select

            Select Case liLowestY
                Case Is = 0
                    liNewY = Me.Y - aiDistance - 10
                Case Is = 1
                    liNewY = Me.Y + 2
                Case Is = 2
                    liNewY = Me.Y + aiDistance + 1
            End Select

            aiX = Viev.Greater(liNewX, 1)
            aiY = Viev.Greater(liNewY, 1)

        End Sub

        Public Overloads Function GetSubTypes() As List(Of FBM.EntityTypeInstance)

            Dim lrEntityTypeInstance As FBM.EntityTypeInstance

            GetSubTypes = New List(Of FBM.EntityTypeInstance)

            For Each lrEntityTypeInstance In Me.Page.EntityTypeInstance

                If lrEntityTypeInstance.EntityType.parentModelObjectList.Contains(Me) Then
                    GetSubTypes.Add(lrEntityTypeInstance)
                End If

            Next

        End Function

        Public Overrides Sub Save(Optional ByVal abRapidSave As Boolean = False)

            '-----------------------------------------
            'Saves the EntityInstance to the database
            '-----------------------------------------
            Dim lrConceptInstance As New FBM.ConceptInstance

            lrConceptInstance.ModelId = Me.Model.ModelId
            lrConceptInstance.PageId = Me.Page.PageId
            lrConceptInstance.Symbol = Me.Id
            lrConceptInstance.X = Me.X
            lrConceptInstance.Y = Me.Y
            lrConceptInstance.ConceptType = pcenumConceptType.EntityType

            If abRapidSave Then
                Call TableConceptInstance.AddConceptInstance(lrConceptInstance)
            Else
                If TableConceptInstance.ExistsConceptInstance(lrConceptInstance) Then
                    Call TableConceptInstance.UpdateConceptInstance(lrConceptInstance)
                Else
                    Call TableConceptInstance.AddConceptInstance(lrConceptInstance)
                End If
            End If

        End Sub

        'Private Overloads Sub SetName(ByVal asNewName As String)

        '    If StrComp(Me.Id, asNewName) <> 0 Then
        '        '------------------------------------------------------------------------------------------
        '        'Update the Model(database) immediately. There is no choice. The reason that you do this,
        '        '  is because the (in-memory) key is changing, so if the database is not updated to 
        '        '  reflect the new key, it is not possible to Update an existing EntityType.
        '        '------------------------------------------------------------------------------------------
        '        '--------------------------------------------------
        '        'Make sure the new Symbol is in the Concept table
        '        '--------------------------------------------------
        '        Dim lrConcept As New FBM.Concept(asNewName)
        '        lrConcept.Save()

        '        Call TableConceptInstance.ModifyKey(Me, asNewName)

        '        Me.Id = asNewName
        '        If IsSomething(Me.Page) Then
        '            Me.Page.MakeDirty()
        '        End If
        '    End If
        'End Sub

        Public Overloads Sub SetReferenceModeObjects()

            Dim lsMessage As String

            Try
                If Me.PreferredIdentifierRCId = "" Then
                    Me.ReferenceModeFactType = Nothing
                    Me.ReferenceModeRoleConstraint = Nothing
                    Me.ReferenceModeValueType = Nothing
                Else
                    If Me.EntityType.ReferenceModeRoleConstraint Is Nothing Then
                        Exit Sub
                    End If
                    Me.ReferenceModeRoleConstraint = Me.Page.RoleConstraintInstance.Find(Function(x) x.Id = Me.EntityType.ReferenceModeRoleConstraint.Id)

                    If IsSomething(Me.ReferenceModeRoleConstraint) And Me.HasSimpleReferenceScheme Then
                        Me.ReferenceModeFactType = New FBM.FactTypeInstance
                        Me.ReferenceModeFactType = Me.ReferenceModeRoleConstraint.RoleConstraintRole(0).Role.FactType
                    End If

                End If
            Catch ex As Exception

                lsMessage = "Error: FBM.EntityType.SetReferenceModeObjects"
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                lsMessage &= vbCrLf & vbCrLf & "EntityType.Id: " & Me.Id
                lsMessage &= vbCrLf & vbCrLf & "Page.Id: " & Me.Page.PageId
                lsMessage &= vbCrLf & "Page.Name: " & Me.Page.Name
                lsMessage &= vbCrLf & "PreferredIdentifierRCId: " & Me.PreferredIdentifierRCId
                If IsSomething(Me.ReferenceModeRoleConstraint) Then
                    lsMessage &= vbCrLf & "FactType.Id: " & Me.ReferenceModeRoleConstraint.RoleConstraintRole(0).Role.FactType.Id
                End If
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub


        Public Function TotalLinks() As Integer

            Return Me.Shape.OutgoingLinks.Count + Me.Shape.IncomingLinks.Count

        End Function

        Public Sub RemoveFromPage(ByVal abBroadcastInterfaceEvent As Boolean)

            Dim lsMessage As String

            Try
                Dim liFactTypeInstanceCount As Integer = 0

                For Each lrSubtypeRelationshipInstance In Me.SubtypeRelationship
                    Call lrSubtypeRelationshipInstance.RemoveFromPage()
                Next

                If Me.HasSimpleReferenceScheme And Me.ReferenceModeFactType IsNot Nothing Then
                    liFactTypeInstanceCount = Aggregate FactType In Me.Page.FactTypeInstance
                                                   From Role In FactType.RoleGroup
                                                  Where Role.JoinedORMObject.Id = Me.Id _
                                                    And FactType.Id <> Me.EntityType.ReferenceModeFactType.Id
                                                   Into Count()
                Else
                    liFactTypeInstanceCount = Aggregate FactType In Me.Page.FactTypeInstance
                                                   From Role In FactType.RoleGroup
                                                  Where Role.JoinedORMObject.Id = Me.Id
                                                   Into Count()
                End If

                If liFactTypeInstanceCount = 0 Then
                    'EntityTypeInstances have 3 shapes

                    If Me.Shape IsNot Nothing Then
                        Me.Page.Diagram.Nodes.Remove(Me.Shape)
                        Me.Page.Diagram.Nodes.Remove(Me.EntityTypeNameShape)
                        Me.Page.Diagram.Nodes.Remove(Me.ReferenceModeShape)
                        Me.Page.EntityTypeInstance.Remove(Me)
                        Call TableEntityTypeInstance.delete_entity_type_instance(Me)

                        If Me.HasSimpleReferenceScheme _
                        And Me.ReferenceModeFactType IsNot Nothing _
                        And abBroadcastInterfaceEvent Then

                            Call TableFactTypeInstance.DeleteFactTypeInstance(Me.ReferenceModeFactType)
                            Call TableValueTypeInstance.DeleteValueTypeInstance(Me.ReferenceModeValueType)
                            Dim lrRoleConstraintInstance As FBM.RoleConstraintInstance
                            For Each lrRoleConstraintInstance In Me.ReferenceModeFactType.InternalUniquenessConstraint
                                TableRoleConstraintInstance.DeleteRoleConstraintInstance(lrRoleConstraintInstance)
                                Me.Page.RoleConstraintInstance.Remove(lrRoleConstraintInstance)
                            Next
                            Call TableFactTableInstance.DeleteFactTableInstance(Me.ReferenceModeFactType.FactTable)
                            Call TableFactTypeName.DeleteFactTypeNameInstance(Me.ReferenceModeFactType.FactTypeName)
                            Me.ReferenceModeFactType.RemoveFromPage(abBroadcastInterfaceEvent)
                            Me.ReferenceModeValueType.RemoveFromPage(abBroadcastInterfaceEvent)
                        End If
                    End If
                Else
                        lsMessage = "You cannot remove the Entity Type, '" & Trim(Me.Name) & "' until all Fact Types with Roles assigned to the Entity Type have been removed from the Page."
                    Throw New Exception(lsMessage)
                End If

                Call Me.Page.RemoveEntityTypeInstance(Me, abBroadcastInterfaceEvent)

                Call Me.Page.MakeDirty()

            Catch ex As Exception
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Public Overrides Sub removeInstance(ByVal asInstance As String)

            Try
                Dim larFactTypeInstance = From FactTypeInstance In Me.Page.FactTypeInstance
                                          From RoleInstance In FactTypeInstance.RoleGroup
                                          Where RoleInstance.JoinedORMObject.Id = Me.Id
                                          Select FactTypeInstance

                For Each lrFactTypeInstance In larFactTypeInstance.ToArray
                    Dim lrFactInstance As New FBM.FactInstance(lrFactTypeInstance)
                    Dim lrRoleInstance As FBM.RoleInstance

                    lrRoleInstance = lrFactTypeInstance.RoleGroup.Find(Function(x) x.JoinedORMObject.Id = Me.Id)

                    Dim lrFact As FBM.Fact = New FBM.Fact(lrFactTypeInstance.FactType)
                    lrFact.Data.Add(New FBM.FactData(lrRoleInstance.Role, New FBM.Concept(Trim(asInstance))))
                    lrFactInstance = lrFact.CloneInstance(Me.Page)

                    Dim lrActualFactTypeInstance As FBM.FactTypeInstance = Me.Page.FactTypeInstance.Find(Function(x) x.Id = lrFactTypeInstance.Id)

                    lrFactInstance = lrActualFactTypeInstance.Fact.Find(AddressOf lrFactInstance.EqualsByFirstDataMatch)

                    If lrFactInstance IsNot Nothing Then
                        lrActualFactTypeInstance.RemoveFact(lrFactInstance)
                    End If

                    Call lrActualFactTypeInstance.FactTable.ResortFactTable()
                Next

                For Each lrEntityType In Me.GetSubTypes
                    Call lrEntityType.removeInstance(asInstance)
                Next

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Private Sub RemoveReferenceMode(ByVal abBroadcastInterfaceEvent As Boolean)

            Try

                If Me.ReferenceModeFactType.FactTable IsNot Nothing Then Me.ReferenceModeFactType.FactTable.RemoveFromPage(abBroadcastInterfaceEvent)
                Me.ReferenceModeFactType.FactTypeName.RemoveFromPage(abBroadcastInterfaceEvent)

                Me.ReferenceModeFactType.RemoveFromPage(abBroadcastInterfaceEvent)
                Me.ReferenceModeValueType.RemoveFromPage(abBroadcastInterfaceEvent)

                Me.PreferredIdentifierRCId = ""
                Me.ReferenceModeValueType = Nothing
                Me.ReferenceModeFactType = Nothing

                If Me.ReferenceModeShape IsNot Nothing Then
                    Me.ReferenceModeShape.Text = ""
                    Me.ReferenceModeShape.Resize(1, 1)
                End If

                Me.ExpandReferenceMode = False

                If Me.Page.Diagram Is Nothing Then Exit Sub
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

            Dim loRectangle As Rectangle
            Dim loEntityWidth As New SizeF
            Dim loEntityNameWidth As New SizeF
            Dim loReferenceModeWidth As New SizeF

            Try
                '------------------
                'Update the Model
                '------------------
                If IsSomething(aoChangedPropertyItem) Then
                    Select Case aoChangedPropertyItem.ChangedItem.PropertyDescriptor.Name
                        Case Is = "DataType"
                            Select Case Me._DataType
                                Case Is = pcenumORMDataType.NumericFloatCustomPrecision,
                                          pcenumORMDataType.NumericDecimal,
                                          pcenumORMDataType.NumericMoney
                                    Call Me.SetPropertyAttributes(Me, "DataTypePrecision", True)
                                    Call Me.SetPropertyAttributes(Me, "DataTypeLength", False)
                                Case Is = pcenumORMDataType.RawDataFixedLength,
                                          pcenumORMDataType.RawDataLargeLength,
                                          pcenumORMDataType.RawDataVariableLength,
                                          pcenumORMDataType.TextFixedLength,
                                          pcenumORMDataType.TextLargeLength,
                                          pcenumORMDataType.TextVariableLength
                                    Call Me.SetPropertyAttributes(Me, "DataTypeLength", True)
                                    Call Me.SetPropertyAttributes(Me, "DataTypePrecision", False)
                                Case Else
                                    Call Me.SetPropertyAttributes(Me, "DataTypePrecision", False)
                                    Call Me.SetPropertyAttributes(Me, "DataTypeLength", False)
                            End Select
                            If Me.EntityType.HasSimpleReferenceScheme Then
                                Me.EntityType.ReferenceModeValueType.SetDataType(Me._DataType)
                            End If
                        Case Is = "DataTypeLength"
                            If Me.EntityType.HasSimpleReferenceScheme Then
                                Me.EntityType.ReferenceModeValueType.SetDataTypeLength(Me._DataTypeLength)
                            End If
                        Case Is = "DataTypePrecision"
                            If Me.EntityType.HasSimpleReferenceScheme Then
                                Me.EntityType.ReferenceModeValueType.SetDataTypePrecision(Me._DataTypePrecision)
                            End If
                        Case Is = "DBName"
                            Call Me.EntityType.SetDBName(Me.DBName)
                        Case Is = "DerivationText"
                            Call Me.EntityType.SetDerivationText(Me.DerivationText, True)
                        Case Is = "IsDatabaseReservedWord"
                            Call Me.EntityType.setIsDatabaseReservedWord(Me.IsDatabaseReservedWord)
                        Case Is = "IsDerived"
                            If Me.IsDerived Then
                                'Call Me.SetPropertyAttributes(Me, "IsStored", True)
                                Call Me.SetPropertyAttributes(Me, "DerivationText", True)
                            Else
                                'Call Me.SetPropertyAttributes(Me, "IsStored", False)
                                Call Me.SetPropertyAttributes(Me, "DerivationText", False)
                            End If
                            Call Me.EntityType.SetIsDerived(Me.IsDerived, True)
                        Case Is = "IsIndependent"
                            Call Me.EntityType.SetIsIndependent(Me.IsIndependent, True)
                            Call Me.EnableSaveButton()
                        Case Is = "IsPersonal"
                            Call Me.EntityType.SetIsPersonal(Me.IsPersonal)
                            Call Me.EnableSaveButton()
                        Case Is = "IsAbsorbed"

                            If Me.IsAbsorbed Then
                                For Each lrModelObject In Me.EntityType.HasSubtype
                                    If Not lrModelObject.IsAbsorbed Then
                                        Dim lsMessage = "The model element, " & Me.Id & ", has subtypes that are not absorbed. It only makes sense for this model element to be absorbed if its subtypes are absorbed."
                                        MsgBox(lsMessage, MsgBoxStyle.Exclamation)
                                        Me.IsAbsorbed = False
                                        Exit Sub
                                    End If
                                Next
                            End If

                            With New WaitCursor
                                Call Me.EntityType.SetIsAbsorbed(Me.IsAbsorbed)
                            End With
                            Call Me.EnableSaveButton()
                        Case Is = "ShortDescription"
                            Call Me.EntityType.SetShortDescription(Me.ShortDescription)
                            Me.Model.ModelDictionary.Find(Function(x) LCase(x.Symbol) = LCase(Me.Id)).ShortDescription = Me.ShortDescription
                            Call Me.EnableSaveButton()
                        Case Is = "LongDescription"
                            Call Me.EntityType.SetLongDescription(Me.LongDescription)
                            Me.Model.ModelDictionary.Find(Function(x) LCase(x.Symbol) = LCase(Me.Id)).LongDescription = Me.LongDescription
                            Call Me.EnableSaveButton()
                        Case Is = "ExpandReferenceMode" 'The name of the Property on the EntityType class related to this EntityTypeInstance

                            Dim larFactTypeInstance = From FactTypeInstance In Me.Page.FactTypeInstance
                                                      From Role In FactTypeInstance.FactType.RoleGroup
                                                      Where Role.JoinedORMObject.Id = Me.Id _
                                                      And FactTypeInstance.isPreferredReferenceMode = True
                                                      Select FactTypeInstance Distinct

                            If IsSomething(larFactTypeInstance) Then
                                If Me.ExpandReferenceMode Then
                                    Call Me.ExpandTheReferenceScheme()
                                Else
                                    '---------------------------
                                    'Hide the ReferenceScheme.                                    
                                    Call Me.HideTheReferenceScheme()
                                End If
                            Else
                                '------------------------------------------------------------------------------------
                                'The FactType/ValueType representing the ReferenceMode for the EntityType
                                '  is not currently displayed on the Page (but should exist within the Model).
                                '  Load/Add the FactType/ValueType represnting the ReferenceMode for the EntityType
                                '  to the Page.
                                '------------------------------------------------------------------------------------
                                Dim lrValueType As New FBM.ValueType(Me.Model, pcenumLanguage.ORMModel, Me.ReferenceMode, True)
                                '-------------------------------------
                                'Find the ValueType within the Model
                                '-------------------------------------
                                lrValueType = Me.Model.ValueType.Find(AddressOf lrValueType.Equals)
                                '----------------------------------------------------------------------------------
                                'Determine an X,Y coordinate for the ValueTyep to be placed on the Page
                                '  (on which the EntityTypeInstance is drawn) and drop the ValueType on the Page.
                                '----------------------------------------------------------------------------------
                                'Call Me.Page.drop_value_type_at_point(lrValueType, lo_pt)

                                '----------------------------------------------------------------------------------
                                'Find the FactType for the ReferenceMode of the EntityType (i.e. The FactType
                                '  between the EntityType and the ValueType that represents the ReferenceMode for 
                                '  the EntityType.
                                '----------------------------------------------------------------------------------
                                Dim lrFactType As New FBM.FactType
                                Me.Model.FactType.Find(AddressOf lrFactType.Equals)

                                '----------------------------------------------------------------------------------
                                'Determine an X,Y coordinate for the FactType to be placed on the Page
                                '  (on which the EntityTypeInstance is drawn) and drop the FactType on the Page.
                                '  NB Automatically links to the ModelObjects (EntityType/ValueType) associated
                                '  with the Roles of the FactType.
                                '----------------------------------------------------------------------------------
                                'Call Me.Page.DropFactTypeAtPoint(lrValueType, lo_pt)

                            End If
                        Case Is = "ReferenceMode" 'The name of the Property on the EntityType class related to this EntityTypeInstance                            
                            If Me.EntityType.GetTopmostNonAbsorbedSupertype Is Me.EntityType Then
                                With New WaitCursor
                                    Call Me.EntityType.SetReferenceMode(Trim(Me.ReferenceMode))
                                End With
                            Else
                                Dim lsMessage = "It makes no sense to have a Primary Reference Schema for a Model Element that is is absorbed into a supertype."
                                lsMessage &= vbCrLf & vbCrLf & "Reverting Reference Model for this Entity Type to ' '."
                                Me.ReferenceMode = " "
                                MsgBox(lsMessage)
                            End If
                            If Me.ReferenceMode = " " Then
                                Call Me.SetPropertyAttributes(Me, "DataType", False)
                            Else
                                'Call Me.SetPropertyAttributes(Me, "DataType", True)
                            End If
                        Case Is = "Name"
                            If Me.EntityType.Name = Me.Name Then
                                '------------------------------------------------------------
                                'Nothing to do. Name of the EntityType has not been changed.
                                '------------------------------------------------------------
                            Else
                                Dim lrEntityTypeDictionaryEntry As New FBM.DictionaryEntry(Me.Model, Me.Name, pcenumConceptType.EntityType)
                                Dim lrValueTypeDictionaryEntry As New FBM.DictionaryEntry(Me.Model, Me.Name, pcenumConceptType.ValueType)
                                Dim lrFactTypeDictionaryEntry As New FBM.DictionaryEntry(Me.Model, Me.Name, pcenumConceptType.FactType)
                                Dim lrRoleConstraintDictionaryEntry As New FBM.DictionaryEntry(Me.Model, Me.Name, pcenumConceptType.RoleConstraint)

                                If Me.Model.ModelDictionary.Exists(AddressOf lrEntityTypeDictionaryEntry.Equals) Then
                                    MsgBox("An Entity Type with the name, '" & lrEntityTypeDictionaryEntry.Symbol & "', already exists in the Model, '" & Me.Model.Name & "'.", MsgBoxStyle.Exclamation, "Model Object Conflict")
                                    Me.Name = Me.EntityType.Name
                                ElseIf Me.Model.ModelDictionary.Exists(AddressOf lrValueTypeDictionaryEntry.Equals) Then
                                    MsgBox("The name, '" & lrEntityTypeDictionaryEntry.Symbol & "', conflicts with a Value Type of the same name in the Model, '" & Me.Model.Name & "'.", MsgBoxStyle.Exclamation, "Model Object Conflict")
                                    Me.Name = Me.EntityType.Name
                                ElseIf Me.Model.ModelDictionary.Exists(AddressOf lrFactTypeDictionaryEntry.Equals) Then
                                    MsgBox("The name, '" & lrEntityTypeDictionaryEntry.Symbol & "', conflicts with a Fact Type of the same name in the Model, '" & Me.Model.Name & "'.", MsgBoxStyle.Exclamation, "Model Object Conflict")
                                    Me.Name = Me.EntityType.Name
                                ElseIf Me.Model.ModelDictionary.Exists(AddressOf lrRoleConstraintDictionaryEntry.Equals) Then
                                    MsgBox("The name, '" & lrEntityTypeDictionaryEntry.Symbol & "', conflicts with a Role Constraint of the same name in the Model, '" & Me.Model.Name & "'.", MsgBoxStyle.Exclamation, "Model Object Conflict")
                                    Me.Name = Me.EntityType.Name
                                ElseIf lrEntityTypeDictionaryEntry.Symbol.Contains("'") Then
                                    MsgBox("The name of an Entity Type cannot contain a ' (single quote).")
                                    Me.Name = Me.EntityType.Name
                                ElseIf Not FBM.IsAcceptableObjectTypeName(lrEntityTypeDictionaryEntry.Symbol) Then
                                    MsgBox("The name of an Entity Type can only contain the characters [a-zA-Z0-9].")
                                    Me.Name = Me.EntityType.Name
                                Else
                                    Me.EntityType.SetName(Me.Name)
                                    Me.Id = Me.Name
                                    Me.Symbol = Me.Name
                                End If
                            End If
                    End Select

                    If IsSomething(Me.Page.Form) Then
                        Call Me.EnableSaveButton()
                        Me.Page.Diagram.Invalidate()
                    End If

                    Call Me.Page.MakeDirty()

                    If Me.Page.Form IsNot Nothing Then
                        Call Me.Page.Form.ResetPropertiesGridToolbox(Me)
                    End If

                End If

                '=============================
                'Do Shape display processing
                '=============================
                Dim liGreaterWidth As Integer = 0

                If IsSomething(Me.Page.Diagram) Then
                    '------------------
                    'Diagram is set.
                    '------------------
                    If IsSomething(Me.EntityTypeNameShape) Then
                        '-------------------------------------------------------------------------------
                        'ShapeNode does not exist for an EntityTypeInstance when cloning an EntityType
                        '-------------------------------------------------------------------------------
                        Dim lsETNameText As String = Trim(Me.Name)

                        If Me.IsDerived Or Me.IsIndependent Then
                            lsETNameText &= " "
                        End If
                        If Me.IsDerived Then
                            lsETNameText &= "*"
                        End If
                        If Me.IsIndependent Then
                            lsETNameText &= "!"
                        End If

                        Me.EntityTypeNameShape.Text = lsETNameText
                        Me.Page.Diagram.Invalidate()

                        '---------------------------------------------------------------------------------------------------------------
                        'Set the size of the EnityTypeInstance.Shape based on whether a ReferenceMode exists for the EntityType or not
                        '---------------------------------------------------------------------------------------------------------------
                        Dim lsReferenceMode As String = ""
                        lsReferenceMode = "(" & Me.ReferenceMode & ")"
                        liGreaterWidth = Viev.Greater(Me.Name.Length, lsReferenceMode.Length)

                        If Me.Name.Length = liGreaterWidth Then
                            liGreaterWidth = Me.EntityTypeNameShape.Bounds.Width + 4
                            loEntityWidth = Me.Page.Diagram.MeasureString(Trim(Me.Name) + "MMMI", Me.Page.Diagram.Font, 1000, System.Drawing.StringFormat.GenericDefault)
                            loEntityNameWidth = Me.Page.Diagram.MeasureString(Trim(lsETNameText) + ".", Me.Page.Diagram.Font, 1000, System.Drawing.StringFormat.GenericDefault)
                            loReferenceModeWidth = Me.Page.Diagram.MeasureString(Trim(Me.ReferenceMode) + "M", Me.Page.Diagram.Font, 1000, System.Drawing.StringFormat.GenericDefault)
                        Else
                            liGreaterWidth = Me.ReferenceModeShape.Bounds.Width + 4
                            loEntityWidth = Me.Page.Diagram.MeasureString(Trim(Me.ReferenceMode) + "MMMI", Me.Page.Diagram.Font, 1000, System.Drawing.StringFormat.GenericDefault)
                            loEntityNameWidth = Me.Page.Diagram.MeasureString(Trim(lsETNameText) + ".", Me.Page.Diagram.Font, 1000, System.Drawing.StringFormat.GenericDefault)
                            loReferenceModeWidth = Me.Page.Diagram.MeasureString(Trim(Me.ReferenceMode) + "M", Me.Page.Diagram.Font, 1000, System.Drawing.StringFormat.GenericDefault)
                        End If

                        If IsSomething(Me.EntityType.ReferenceModeValueType) And Not Me.ExpandReferenceMode Then
                            loRectangle = New Rectangle(Me.X, Me.Y, loEntityWidth.Width, 12)
                            Me.Shape.SetRect(loRectangle, False)
                            loRectangle = New Rectangle(Me.X + 2, Me.EntityTypeNameShape.Bounds.Y, loEntityNameWidth.Width, Me.EntityTypeNameShape.Bounds.Height)
                            Me.EntityTypeNameShape.SetRect(loRectangle, False)
                            loRectangle = New Rectangle(Me.X + 2, Me.ReferenceModeShape.Bounds.Y, loReferenceModeWidth.Width, loReferenceModeWidth.Height + 1)
                            Me.ReferenceModeShape.SetRect(loRectangle, False)
                            Me.ReferenceModeShape.Visible = True
                            Me.ReferenceModeShape.Text = "(" & Me.ReferenceMode & ")"
                            Me.ReferenceModeShape.Move(Me.X + (liGreaterWidth / 2) - (Me.ReferenceModeShape.Bounds.Width / 2),
                                                       Me.Y + 6)
                        Else
                            loRectangle = New Rectangle(Me.X, Me.Y, loEntityWidth.Width, 8)
                            Me.Shape.SetRect(loRectangle, False)
                            loRectangle = New Rectangle(Me.X + 2, Me.EntityTypeNameShape.Bounds.Y, loEntityNameWidth.Width, Me.EntityTypeNameShape.Bounds.Height)
                            Me.EntityTypeNameShape.SetRect(loRectangle, False)
                            Me.ReferenceModeShape.Visible = False
                        End If
                    End If
                End If

                Call Me.SetAppropriateColour()

                '---------------------------------------------------------------
                'Some properties of the EntityType/Instance may now be hidden.
                '---------------------------------------------------------------
                If IsSomething(Me.Page.Form) Then
                    'Call Me.Page.Form.ResetPropertiesGridToolbox(Me)
                End If

            Catch ex As Exception
                Dim lsMessage As String
                lsMessage = "Error: tEntityTypeInstance.RefreshShape:"
                lsMessage &= vbCrLf & vbCrLf & "EntityTypeName: " & Me.Name
                lsMessage &= vbCrLf & ", PageId:" & Me.Page.PageId
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        ''' <summary>
        ''' Sets the ReferenceMode objects for a SimpleReferenceScheme.
        '''   20150910-VM-NB May need to change the name of this method to SetReferenceModeObjects and delete the existing SetReferenceModeObjects.
        '''   The existing SetReferenceModeObjects is called when the Page loads. This method also seems to fill that role.
        ''' </summary>
        ''' <remarks></remarks>
        Public Overloads Sub SetReferenceMode()

            Dim lsMessage As String = ""

            Try

                If Trim(Me.ReferenceMode) = "" Then
                    '-----------------------------------------------------------------------------------
                    'No processing to do.
                    '  Throw an error if this method is called for EntityTypes without a ReferenceMode
                    '-----------------------------------------------------------------------------------
                    lsMessage = "tEntityTypeInstance.SetReferenceMode called for EntityType with no ReferenceMode"
                    Throw New Exception(lsMessage)
                Else
                    '-------------------------------------------------------------------------------------------------------------
                    'User has elected to give the EntityType(Instance) a ReferenceMode or has changed the existing ReferenceMode
                    '-------------------------------------------------------------------------------------------------------------
                    If IsSomething(Me.ReferenceModeValueType) Then
                        '--------------------------------------------------------------------------------------
                        'All good, the EntityType(Instance) already has a ReferenceMode
                        '--------------------------------------------------------------------------------------

                        '-------------------------------------------------------------------------------------------------------------
                        '20160322-Either move this down to the model level or get rid of it. 
                        'Occurs rarely and might not be appropriate at the Page level.
                        'Check to see if the ReferenceModeValueType is used by more than one FactType.
                        '  NB It shouldn't be, but the user may have reused the ValueType, referencing the ValueType from a Role in
                        '  a FactType other than the EntityType/Instances ReferenceModeFactType.
                        '-------------------------------------------------------------------------------------------------------------
                        'Dim liReferenceModeFactTypeCount = Aggregate FactType In Me.Page.FactTypeInstance _
                        '                                   From Role In FactType.RoleGroup _
                        '                                   Where Role.JoinedORMObject.Id = Me.ReferenceModeValueType.Id _
                        '                                   Into Count()

                        'If liReferenceModeFactTypeCount = 1 Then
                        '    '------------------------------------------------------------------------------------------------------
                        '    'Can safely change the Name/Id of the ValueType because only 1 FactType/Role references the Valuetype
                        '    '------------------------------------------------------------------------------------------------------
                        '    Call Me.ReferenceModeValueType.RefreshShape()
                        'Else
                        '    '------------------------------------------------------------------------------------------------------
                        '    'Must create a new ValueType(Instance) so that the other FactType(Instance)s referencing the ValueType
                        '    '  are not effected by the change of ReferenceMode for this EntityType(Instance)
                        '    '------------------------------------------------------------------------------------------------------
                        '    Dim lrValueType As New FBM.ValueType
                        '    lrValueType = Me.EntityType.ReferenceModeValueType
                        '    Me.EntityType.ReferenceModeFactType.FindFirstRoleByModelObject(Me.EntityType.ReferenceModeValueType).JoinsValueType = lrValueType
                        '    Me.EntityType.ReferenceModeFactType.FindFirstRoleByModelObject(Me.EntityType.ReferenceModeValueType).JoinedORMObject = lrValueType
                        '    Me.EntityType.ReferenceModeValueType = lrValueType
                        '    Dim lrValueTypeInstance As New FBM.ValueTypeInstance
                        '    lrValueTypeInstance = lrValueType.CloneInstance(Me.Page, True)
                        '    Dim liX, liY As Integer
                        '    liX = Me.ReferenceModeValueType.X - 5
                        '    liY = Me.ReferenceModeValueType.Y - 5
                        '    Me.ReferenceModeFactType.FindFirstRoleByModelObject(Me.ReferenceModeValueType).JoinsValueType = lrValueTypeInstance
                        '    Me.ReferenceModeFactType.FindFirstRoleByModelObject(Me.ReferenceModeValueType).JoinedORMObject = lrValueTypeInstance
                        '    Me.ReferenceModeValueType = lrValueTypeInstance
                        '    Me.ReferenceModeValueType.DisplayAndAssociate(True)
                        '    Me.ReferenceModeValueType.Shape.Move(liX, liY)
                        '    Me.ReferenceModeFactType.FindFirstRoleByModelObject(Me.ReferenceModeValueType).ResetLink()
                        'End If

                    Else 'If Me.ReferenceModeValueType (Instance) exists 
                        '(below is ReferenceModeValueType (Instance) does not exist)
                        '---------------------------------------------------------------------------------------------
                        'Me.ReferenceModeValueType does not yet exist.
                        '  This happens when the ReferenceMode was "" and the user set the ReferenceMode to a value.
                        '---------------------------------------------------------------------------------------------
                        Me.ExpandReferenceMode = False

                        '---------------------------------------------------------------------------
                        'The ValueTypeInstance may already be on the Page. 
                        '  i.e. e.g. When the user sets a ReferenceMode via VAQL and the Brain.
                        '---------------------------------------------------------------------------
                        Dim lrValueTypeInstance As New FBM.ValueTypeInstance
                        lrValueTypeInstance.Id = Me.EntityType.ReferenceModeValueType.Id

                        If Me.Page.ValueTypeInstance.Exists(AddressOf lrValueTypeInstance.Equals) Then
                            '-----------------------------------------------------------------------------------------------
                            'The ValueTypeInstance is already on the Page.
                            '-----------------------------------------------------------------------------------------------
                            Me.ReferenceModeValueType = Me.Page.ValueTypeInstance.Find(AddressOf lrValueTypeInstance.Equals)
                            Me.ReferenceModeValueType.Shape.Visible = False
                        Else
                            '----------------------------------------
                            'Need to add the ValueType to the Page.
                            '----------------------------------------
                            Me.ReferenceModeValueType = Me.EntityType.ReferenceModeValueType.CloneInstance(Me.Page, True)
                            '------------------------------------------------------------------------------------------
                            '20170322-If all seems fine without this code commented out (just below)...remove it.
                            'Call Me.ReferenceModeValueType.SetName(Me.MakeReferenceModeName)
                            If IsSomething(Me.Shape) Then
                                Me.ReferenceModeValueType.DisplayAndAssociate()
                                Me.ReferenceModeValueType.Shape.Visible = False
                            End If
                        End If

                        '-------------------------------------------------------------------------------------------------------------
                        'The FactTypeInstance may already be on the Page.
                        '--------------------------------------------------
                        Dim lrFactTypeInstance As New FBM.FactTypeInstance
                        Dim lrRoleConstraintInstance As New FBM.RoleConstraintInstance

                        lrFactTypeInstance.Id = Me.EntityType.ReferenceModeFactType.Id

                        If Me.Page.FactTypeInstance.Exists(AddressOf lrFactTypeInstance.Equals) Then
                            Me.ReferenceModeFactType = Me.Page.FactTypeInstance.Find(AddressOf lrFactTypeInstance.Equals)
                            Me.ReferenceModeFactType.isPreferredReferenceMode = True
                            Me.ReferenceModeFactType.Shape.Visible = False
                        Else
                            Me.ReferenceModeFactType = Me.EntityType.ReferenceModeFactType.CloneInstance(Me.Page, True)
                            If IsSomething(Me.Shape) Then
                                Me.ReferenceModeFactType.DisplayAndAssociate()
                                Me.ReferenceModeFactType.Shape.Visible = False
                                For Each lrRoleConstraintInstance In Me.ReferenceModeFactType.InternalUniquenessConstraint
                                    lrRoleConstraintInstance.DisplayAndAssociate()
                                Next
                            End If
                        End If

                        '-------------------------------------------------------------------------------------------------------------
                        'The ReferenceModeRoleConstraint may already be on the Page.
                        '-------------------------------------------------------------

                        lrRoleConstraintInstance.Id = Me.EntityType.PreferredIdentifierRCId
                        Me.PreferredIdentifierRCId = Me.EntityType.PreferredIdentifierRCId

                        If Me.Page.RoleConstraintInstance.Exists(AddressOf lrRoleConstraintInstance.Equals) Then
                            Me.ReferenceModeRoleConstraint = Me.Page.RoleConstraintInstance.Find(AddressOf lrRoleConstraintInstance.Equals)
                        Else
                            '-----------------------------------------------------------------------------------------------------------------------------
                            'Should never really reach this line of code, because the ReferenceModeFactType is loaded onto the Page one way or the other
                            '-----------------------------------------------------------------------------------------------------------------------------
                            Me.ReferenceModeRoleConstraint = Me.EntityType.ReferenceModeRoleConstraint.CloneInstance(Me.Page, True)
                        End If

                        If Me.ReferenceModeShape IsNot Nothing Then
                            Call Me.HideTheReferenceScheme()
                        End If


                    End If
                    End If

                Me.Page.Invalidate()

            Catch ex As Exception
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Private Sub _EntityType_IsAbsorbedChanged(abNewIsAbsorbed As Boolean) Handles _EntityType.IsAbsorbedChanged

            Me.IsAbsorbed = abNewIsAbsorbed


        End Sub

        Private Sub _EntityType_IsIndependentChanged(abNewIsIndependent As Boolean) Handles _EntityType.IsIndependentChanged
            Me.IsIndependent = abNewIsIndependent
            Call Me.RefreshShape()
        End Sub

        Private Sub _EntityType_IsPersonalChanged(abNewIsPersonal As Boolean) Handles _EntityType.IsPersonalChanged
            Me.IsPersonal = abNewIsPersonal
        End Sub

        Private Sub _EntityType_LongDescriptionChanged(asLongDescription As String) Handles _EntityType.LongDescriptionChanged

            Me.LongDescription = asLongDescription

        End Sub

        Private Sub _EntityType_NameChanged() Handles _EntityType.NameChanged

            Dim lrConceptInstance As New FBM.ConceptInstance(Me.Model, Me.Page, Me.EntityType.Id, pcenumConceptType.EntityType)
            Call TableConceptInstance.UpdateConceptInstanceByModelPageConceptTypeRoleId(lrConceptInstance, Me.Id)

            Me.Name = Me.EntityType.Id
            Me.Id = Me.EntityType.Id
            Me.Symbol = Me.EntityType.Id

            Me.Page.MakeDirty()

        End Sub

        Private Sub _EntityType_PreferredIdentifierRCIdChanged(ByVal asNewPreferredIndentifierRCId As String) Handles _EntityType.PreferredIdentifierRCIdChanged

            Me.PreferredIdentifierRCId = asNewPreferredIndentifierRCId

        End Sub

        Private Sub _EntityType_ReferenceModeChanged(ByVal asNewReferenceMode As String,
                                                     ByVal abSimpleAssignment As Boolean,
                                                     ByVal abBroadcastInterfaceEvent As Boolean) Handles _EntityType.ReferenceModeChanged

            Try
                '-----------------------------
                'Do ReferenceMode processing
                '-----------------------------
                Me.ReferenceMode = Me.EntityType.ReferenceMode
                If (Trim(Me.ReferenceMode) = "") And IsSomething(Me.ReferenceModeFactType) Then
                    '--------------------------------------------------------
                    'Is an EntityType already with a SimpleReferenceScheme.
                    '--------------------------------------------------------
                    Call Me.RemoveReferenceMode(abBroadcastInterfaceEvent)
                ElseIf Me.EntityType.ReferenceMode = "" Then
                    '-------------------
                    'Nothing happening
                    '-------------------
                ElseIf abSimpleAssignment Then
                    Me.ReferenceMode = asNewReferenceMode
                    Call Me.SetReferenceMode()
                Else
                    Call Me.SetReferenceMode()
                End If

                '------------------------------------------------------------------------------------------------------------------
                'Refresh the FactTables for FactTypes that join to the EntityTypeInstance, to reflect the CompoundReferenceScheme
                Dim larFactTypeInstance = From FactTypeInstance In Me.Page.FactTypeInstance _
                                          From Role In FactTypeInstance.RoleGroup _
                                          Where Role.JoinedORMObject.Id = Me.Id _
                                          Select FactTypeInstance

                For Each lrFactTypeInstance In larFactTypeInstance.ToArray
                    Call lrFactTypeInstance.FactTable.ResortFactTable()
                Next

                Call Me.RefreshShape()

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Private Sub _EntityType_ReferenceModeRoleConstraintChanged(ByRef arNewReferenceModeRoleConstraint As RoleConstraint) Handles _EntityType.ReferenceModeRoleConstraintChanged

            Try
                If IsSomething(arNewReferenceModeRoleConstraint) Then
                    Dim lrRoleConstraintInstance As New FBM.RoleConstraintInstance
                    lrRoleConstraintInstance.Id = arNewReferenceModeRoleConstraint.Id
                    If Me.Page.RoleConstraintInstance.Exists(AddressOf lrRoleConstraintInstance.Equals) Then
                        '--------------------------------------------------------------------------
                        'The RoleConstraintInstance is on the same page as the EntityTypeInstance
                        '--------------------------------------------------------------------------
                        Me.ReferenceModeRoleConstraint = Me.Page.RoleConstraintInstance.Find(AddressOf arNewReferenceModeRoleConstraint.Equals)
                    Else
                        Me.ReferenceModeRoleConstraint = Nothing
                    End If

                    '------------------------------------------------------------------------------------------------------------------
                    'Refresh the FactTables for FactTypes that join to the EntityTypeInstance, to reflect the CompoundReferenceScheme
                    Dim larFactTypeInstance = From FactTypeInstance In Me.Page.FactTypeInstance _
                                              From Role In FactTypeInstance.RoleGroup _
                                              Where Role.JoinedORMObject.Id = Me.Id _
                                              Select FactTypeInstance

                    For Each lrFactTypeInstance In larFactTypeInstance.ToArray
                        Call lrFactTypeInstance.FactTable.ResortFactTable()
                    Next

                Else
                    '-------------------------------------------------------------
                    'Likely just removed a ReferenceScheme from the EntityType
                    '-----------------------------------------------------------
                    Me.ReferenceModeRoleConstraint = Nothing
                End If

                Call Me.SetAppropriateColour()

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Private Sub _EntityType_RemovedFromModel(ByVal abBroadcastInterfaceEvent As Boolean) Handles _EntityType.RemovedFromModel

            Call Me.RemoveFromPage(abBroadcastInterfaceEvent)

        End Sub


        Private Sub _EntityType_ShortDescriptionChanged(asShortDescription As String) Handles _EntityType.ShortDescriptionChanged

            Me.ShortDescription = asShortDescription
        End Sub

        Private Sub _EntityType_SimpleReferenceSchemeRemoved() Handles _EntityType.SimpleReferenceSchemeRemoved

            Dim lsMessage As String = ""

            Try
                '-------------------------------------------------------------------------
                'Check that this EntityTypeInstance actually has a SimpleReferenceScheme
                '-------------------------------------------------------------------------
                If Trim(Me.ReferenceMode) = "" Then

                    '--------------------------------------------------------------------------------------------------------------------------
                    'CodeSafe: Extra Checks
                    '------------------------
                    If Me.ReferenceModeFactType Is Nothing And _
                       Me.ReferenceModeValueType Is Nothing Then
                        lsMessage = "Tried to remove a Simple Reference Scheme from an EntityTypeInstance that doesn't have one."
                        lsMessage &= vbCrLf & "EntityType.Id: " & Me.Id
                        'Throw New Exception(lsMessage)
                    Else
                        '------------------------------------------------------------------------------------------------------------------------------
                        'Are in the process of actually removing the SimpleReferenceScheme, by the User selecting " " (nothing) for the ReferenceMode
                        '  in the PropertiesGrid for the EntityType/Instance.
                        '  In this case, the code (below) that 'shows' the hidden FactType/ValueType is not required to be called.
                        '------------------------------------------------------------------------------------------------------------------------------
                    End If
                Else
                    '-----------------------------
                    'Has a SimpleReferenceScheme
                    '-----------------------------
                    Me.ExpandReferenceMode = True

                    If IsSomething(Me.ReferenceModeValueType) Then
                        If IsSomething(Me.ReferenceModeValueType.Shape) Then
                            Me.ReferenceModeValueType.Shape.Visible = True
                            Call Me.getBlankCellCloseBy(Me.ReferenceModeValueType.X, Me.ReferenceModeValueType.Y)
                        End If
                        Me.ReferenceModeValueType = Nothing
                    End If

                    If IsSomething(Me.ReferenceModeFactType) Then
                        Me.ReferenceModeFactType.isPreferredReferenceMode = False
                        Me.ReferenceModeFactType.Visible = True

                        Call Me.SetAdjoinedFactTypesBetweenModelElements(Me.ReferenceModeFactType)
                        Me.ReferenceModeFactType = Nothing
                    End If

                    If IsSomething(Me.ReferenceModeRoleConstraint) Then
                        Me.ReferenceModeRoleConstraint.IsPreferredIdentifier = False
                        Me.ReferenceModeRoleConstraint.RefreshShape()
                        Me.ReferenceModeRoleConstraint = Nothing
                        Me.PreferredIdentifierRCId = ""
                    End If

                    Me.ReferenceMode = " "
                    Me.isDirty = True

                    Call Me.RefreshShape()
                End If

            Catch ex As Exception
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Private Sub _EntityType_SubtypeConstraintAdded(ByRef arSubtypeRelationship As FBM.tSubtypeRelationship) Handles _EntityType.SubtypeRelationshipAdded

            Try
                '-----------------------------------------------------------------------------------------------------------
                'Check to see whether the Supertype is on the Page
                Dim lsParentEntityTypeId As String = arSubtypeRelationship.parentEntityType.Id
                Dim lrParentEntityType As FBM.EntityType = Me.Page.EntityTypeInstance.Find(Function(x) x.Id = lsParentEntityTypeId)
                If lrParentEntityType Is Nothing Then
                    '---------------------------------------------------------------------------------------------------------------
                    'The Parent EntityType is not on the Page, so abort
                    Exit Sub
                End If

                Dim lrSubtypeRelationshipInstance As New FBM.SubtypeRelationshipInstance
                lrSubtypeRelationshipInstance = arSubtypeRelationship.CloneInstance(Me.Page)

                '-------------------------------------------------------------
                'CodeSafe
                If Me.Page IsNot Nothing Then
                    Dim lrFactType = lrSubtypeRelationshipInstance.SubtypeRelationship.FactType
                    Dim lrFactTypeInstance = Me.Page.DropFactTypeAtPoint(lrFactType, New PointF(0, 0), False,, True, False)
                    lrFactTypeInstance.SubtypeConstraintInstance = lrSubtypeRelationshipInstance
                    Call lrSubtypeRelationshipInstance.DisplayAndAssociate()
                End If

                Me.SubtypeRelationship.AddUnique(lrSubtypeRelationshipInstance)

                Call Me.Page.MakeDirty()

                If Me.Page.Form IsNot Nothing Then
                    Call Me.Page.Form.EnableSaveButton()
                End If

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
            End Try


        End Sub

        Private Sub _EntityType_SubtypeConstraintRemoved(ByRef arSubtypeConstraint As FBM.tSubtypeRelationship) Handles _EntityType.SubtypeConstraintRemoved

            Try
                Dim lrSubtypeConstraintInstance As FBM.SubtypeRelationshipInstance

                lrSubtypeConstraintInstance = arSubtypeConstraint.CloneInstance(Me.Page, False)
                lrSubtypeConstraintInstance = Me.SubtypeRelationship.Find(AddressOf lrSubtypeConstraintInstance.Equals)

                If IsSomething(lrSubtypeConstraintInstance) Then
                    Call lrSubtypeConstraintInstance.RemoveFromPage()
                    Me.SubtypeRelationship.Remove(lrSubtypeConstraintInstance)
                    If Me.Page.Form IsNot Nothing Then
                        Call Me.Page.Form.EnableSaveButton()
                    End If
                End If

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Private Sub _EntityType_updated(ByVal aiConceptType As publicConstants.pcenumConceptType) Handles _EntityType.updated

            Dim loEntityWidth As New SizeF
            Dim loEntityNameWidth As New SizeF
            Dim loReferenceModeWidth As New SizeF
            '---------------------------------------------------------------------
            'Linked by Delegate in New to the 'update' event of the ModelObject 
            '  referenced by Objects of this Class
            '---------------------------------------------------------------------
            Try
                Me.Name = Me.EntityType.Name

                'Me.ShortDescription = Me.EntityType.ShortDescription
                'Me.LongDescription = Me.EntityType.LongDescription

                Call Me.RefreshShape()

                Me.Page.MakeDirty()

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                lsMessage &= vbCrLf & vbCrLf & "EntityTypeName: " & Me.Name
                lsMessage &= vbCrLf & ", PageId:" & Me.Page.PageId
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try
        End Sub

        Public Sub NodeDeselected() Implements FBM.iPageObject.NodeDeselected

            Call Me.SetAppropriateColour()
        End Sub

        Public Sub SetAppropriateColour() Implements iPageObject.SetAppropriateColour

            Try
                If IsSomething(Me.Shape) Then
                    If Me.Shape.Selected Then
                        Me.Shape.Pen.Color = Color.Blue
                    Else
                        If Me.EntityType.HasModelError Then
                            Me.Shape.Pen.Color = Color.Red
                        ElseIf Me.EntityType.HasSimpleReferenceScheme Then
                            If Me.EntityType.GetTopmostSupertype.ConceptType = pcenumConceptType.EntityType Then
                                Dim lrEntityType As FBM.EntityType = Me.EntityType.GetTopmostSupertype
                                If lrEntityType.ReferenceModeValueType.HasModelError Then
                                    Me.Shape.Pen.Color = Color.Red
                                Else
                                    Me.Shape.Pen.Color = Color.Navy
                                End If
                            Else
                                Me.Shape.Pen.Color = Color.Navy
                            End If
                        Else
                            Me.Shape.Pen.Color = Color.Navy
                        End If
                    End If
                End If

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        ''' <summary>
        ''' Sets the position of Binary FactTypes adjoined to the EntityType/Instance between the EntityType/Instance
        '''   and the other ModelElement of the Binary FactType
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub SetAdjoinedFactTypesBetweenModelElements(Optional arFactTypeInstance As FBM.FactTypeInstance = Nothing)

            Dim lrFactTypeInstance As FBM.FactTypeInstance
            Dim lrLink As DiagramLink
            Dim liX As Integer
            Dim liY As Integer
            Dim lbDoMove As Boolean

            Try
                '------------------------------------------------------------------
                'CodeSafe: Abort if the EntityTypeInstance does not have a Shape.
                '------------------------------------------------------------------
                If Me.Shape Is Nothing Then
                    Exit Sub
                End If

                For Each lrLink In Me.Shape.IncomingLinks

                    If lrLink.Tag.ConceptType = pcenumConceptType.Role Then

                        Dim lrRoleInstance As FBM.RoleInstance
                        lrRoleInstance = lrLink.Tag
                        lrFactTypeInstance = lrRoleInstance.FactType

                        If arFactTypeInstance Is Nothing Then
                            lbDoMove = True
                        Else
                            lbDoMove = (lrFactTypeInstance.Id = arFactTypeInstance.Id)
                        End If

                        If lbDoMove And lrFactTypeInstance.IsBinaryFactType Then

                            Dim lrObject As Object
                            lrObject = lrFactTypeInstance.GetOtherRoleOfBinaryFactType(lrRoleInstance.Id).JoinedORMObject

                            If lrObject IsNot Me Then
                                liX = (Me.X + lrObject.X) / 2
                                liY = (Me.Y + lrObject.Y) / 2

                                lrFactTypeInstance.Move(liX, liY, True)
                            End If
                        End If
                    End If
                Next

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Public Sub RepellFromNeighbouringPageObjects(ByVal aiDepth As Integer, ByVal abBroadcastInterfaceEvent As Boolean)

            Dim liRepellDistance As Integer = 25
            Dim liNewX, liNewY As Integer

            '----------------------------------
            'CodeSafe:
            If aiDepth > 20 Then
                Exit Sub
            Else
                aiDepth += 1
            End If

            Try
                Dim larEntityTypeInstance = From PageObject In Me.Page.GetAllPageObjects _
                                            Where PageObject.Id <> Me.Id _
                                            And (Math.Abs(Me.X - PageObject.X) < liRepellDistance _
                                            And Math.Abs(Me.Y - PageObject.Y) < liRepellDistance) _
                                            And PageObject.Shape IsNot Nothing _
                                            Select PageObject

                For Each lrPageObject In larEntityTypeInstance

                    If (Me.X - lrPageObject.X >= 0) And (Math.Abs(Me.X - lrPageObject.X) < liRepellDistance) Then
                        liNewX = Me.X + 1
                    Else
                        liNewX = Me.X - 1
                    End If

                    If Me.Y - lrPageObject.Y >= 0 And (Math.Abs(Me.Y - lrPageObject.Y) < liRepellDistance) Then
                        liNewY = Me.Y + 1
                    Else
                        liNewY = Me.Y - 1
                    End If

                    Call Me.Move(liNewX, liNewY, abBroadcastInterfaceEvent)
                Next

                Call Me.RepellFromNeighbouringPageObjects(aiDepth, abBroadcastInterfaceEvent)

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Public Sub RepellNeighbouringPageObjects(ByVal aiDepth As Integer) Implements iPageObject.RepellNeighbouringPageObjects

            Dim lrEntityTypeInstance As FBM.EntityTypeInstance
            Dim lrValueTypeInstance As FBM.ValueTypeInstance
            Dim lrFactTypeInstance As FBM.FactTypeInstance
            Dim liRepellDistance As Integer
            Dim liNewX As Integer
            Dim liNewY As Integer

            liRepellDistance = 25

            '----------------------------------
            'CodeSafe:
            If aiDepth > 20 Then
                Exit Sub
            Else
                aiDepth += 1
            End If

            Try
                Dim larEntityTypeInstance = From EntityTypeInstance In Me.Page.EntityTypeInstance _
                                            Where EntityTypeInstance.Id <> Me.Id _
                                            And (Math.Abs(Me.X - EntityTypeInstance.X) < liRepellDistance _
                                            And Math.Abs(Me.Y - EntityTypeInstance.Y) < liRepellDistance) _
                                            And EntityTypeInstance.Shape IsNot Nothing _
                                            Select EntityTypeInstance

                For Each lrEntityTypeInstance In larEntityTypeInstance

                    If (Me.X - lrEntityTypeInstance.X > 0) And (Math.Abs(Me.X - lrEntityTypeInstance.X) < liRepellDistance) Then
                        liNewX = lrEntityTypeInstance.X - 1
                    Else
                        liNewX = lrEntityTypeInstance.X + 1
                    End If

                    If Me.Y - lrEntityTypeInstance.Y > 0 And (Math.Abs(Me.Y - lrEntityTypeInstance.Y) < liRepellDistance) Then
                        liNewY = lrEntityTypeInstance.Y - 1
                    Else
                        liNewY = lrEntityTypeInstance.Y + 1
                    End If

                    Call lrEntityTypeInstance.Move(liNewX, liNewY, True)
                    Call lrEntityTypeInstance.RepellNeighbouringPageObjects(aiDepth)

                Next

                Dim larValueTypeInstance = From ValueTypeInstance In Me.Page.ValueTypeInstance _
                                Where (Math.Abs(Me.X - ValueTypeInstance.X) < liRepellDistance _
                                And Math.Abs(Me.Y - ValueTypeInstance.Y) < liRepellDistance) _
                                And ValueTypeInstance.Shape IsNot Nothing _
                                Select ValueTypeInstance

                For Each lrValueTypeInstance In larValueTypeInstance
                    If (Me.X - lrValueTypeInstance.X > 0) And (Math.Abs(Me.X - lrValueTypeInstance.X) < liRepellDistance) Then
                        liNewX = lrValueTypeInstance.X - 1
                    Else
                        liNewX = lrValueTypeInstance.X + 1
                    End If

                    If Me.Y - lrValueTypeInstance.Y > 0 And (Math.Abs(Me.Y - lrValueTypeInstance.Y) < liRepellDistance) Then
                        liNewY = lrValueTypeInstance.Y - 1
                    Else
                        liNewY = lrValueTypeInstance.Y + 1
                    End If

                    lrValueTypeInstance.Move(liNewX, liNewY, True)
                    Call lrValueTypeInstance.RepellNeighbouringPageObjects(aiDepth)
                Next

                If Me.Shape IsNot Nothing Then
                    Call Me.SetAdjoinedFactTypesBetweenModelElements()
                End If


                '=========================================================
                'FactTypes
                '=========================================================
                Dim larFactTypeInstance = From FactTypeInstance In Me.Page.FactTypeInstance _
                                          Where (Math.Abs(Me.X - FactTypeInstance.X) < liRepellDistance _
                                          And Math.Abs(Me.Y - FactTypeInstance.Y) < liRepellDistance) _
                                          And FactTypeInstance.Shape IsNot Nothing _
                                          Select FactTypeInstance

                For Each lrFactTypeInstance In larFactTypeInstance
                    If (Me.X - lrFactTypeInstance.X > 0) And (Math.Abs(Me.X - lrFactTypeInstance.X) < liRepellDistance) Then
                        liNewX = lrFactTypeInstance.X - 1
                    Else
                        liNewX = lrFactTypeInstance.X + 1
                    End If

                    If Me.Y - lrFactTypeInstance.Y > 0 And (Math.Abs(Me.Y - lrFactTypeInstance.Y) < liRepellDistance) Then
                        liNewY = lrFactTypeInstance.Y - 1
                    Else
                        liNewY = lrFactTypeInstance.Y + 1
                    End If

                    lrFactTypeInstance.Move(liNewX, liNewY, True)
                    Call lrFactTypeInstance.RepellNeighbouringPageObjects(aiDepth)
                Next

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try


        End Sub

        Public Sub Move(ByVal aiNewX As Integer, ByVal aiNewY As Integer, ByVal abBroadcastInterfaceEvent As Boolean) Implements iPageObject.Move

            Try
                If aiNewX < 0 Then aiNewX = 0
                If aiNewY < 0 Then aiNewY = 0

                If Me.Shape IsNot Nothing Then
                    Me.Shape.Move(aiNewX, aiNewY)
                End If

                Me.X = aiNewX
                Me.Y = aiNewY

                '==============================================================================
                'Client/Server: Broadcast the moving of the Object
                '  NB See also: SelectionMoved. Need this code in both places for some reason VM-20180316
                If My.Settings.UseClientServer And My.Settings.InitialiseClient And abBroadcastInterfaceEvent Then

                    Dim lrModel As New Viev.FBM.Interface.Model
                    Dim lrPage As New Viev.FBM.Interface.Page()

                    lrModel.ModelId = Me.Page.Model.ModelId
                    lrPage.Id = Me.Page.PageId
                    lrPage.ConceptInstance = New Viev.FBM.Interface.ConceptInstance
                    lrPage.ConceptInstance.X = Me.Shape.Bounds.X
                    lrPage.ConceptInstance.Y = Me.Shape.Bounds.Y
                    lrPage.ConceptInstance.ModelElementId = Me.Id
                    lrModel.Page = lrPage

                    Dim lrBroadcast As New Viev.FBM.Interface.Broadcast
                    lrBroadcast.Model = lrModel
                    Call prDuplexServiceClient.SendBroadcast([Interface].pcenumBroadcastType.PageMovePageObject, lrBroadcast)

                End If
                '==============================================================================


            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Public Sub showSubtypeRelationships()

            For Each lrSubtypeRelationship In Me.SubtypeRelationship
                Call lrSubtypeRelationship.DisplayAndAssociate()
            Next

        End Sub

        Private Sub _EntityType_IsDatabaseReservedWordChanged(abIsDatabaseReservedWord As Boolean) Handles _EntityType.IsDatabaseReservedWordChanged
            Me.IsDatabaseReservedWord = abIsDatabaseReservedWord
        End Sub

        Public Sub EnableSaveButton() Implements iPageObject.EnableSaveButton
            If Me.Page IsNot Nothing Then
                If Me.Page.Form IsNot Nothing Then
                    Call Me.Page.Form.EnableSaveButton
                End If
            Else
                Try
                    frmMain.ToolStripButton_Save.Enabled = True
                Catch ex As Exception
                End Try
            End If
        End Sub

        Private Sub _EntityType_DBNameChanged(asDBName As String) Handles _EntityType.DBNameChanged

            Me._DBName = asDBName

        End Sub
    End Class

End Namespace