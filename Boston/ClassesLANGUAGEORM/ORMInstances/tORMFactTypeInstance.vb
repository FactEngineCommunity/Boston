Imports System.ComponentModel
Imports MindFusion.Diagramming
Imports MindFusion.Diagramming.WinForms
Imports MindFusion.Drawing
Imports MindFusion.Diagramming.Layout
Imports System.Xml.Serialization
Imports System.Reflection

Namespace FBM
    <Serializable()> _
    Public Class FactTypeInstance
        Inherits FBM.FactType
        Implements ICloneable
        Implements IEquatable(Of FBM.FactTypeInstance)
        Implements FBM.iPageObject

        <XmlIgnore()>
        Public WithEvents Page As FBM.Page

        'The FactType for which the FactTypeIstance acts as View/Proxy.
        <XmlIgnore()> _
        Private WithEvents _FactType As New FBM.FactType
        <XmlIgnore()> _
        <Browsable(False)> _
        Public Property FactType() As FBM.FactType
            Get
                Return Me._FactType
            End Get
            Set(ByVal value As FBM.FactType)
                Me._FactType = value
                Me.Concept = value.Concept
            End Set
        End Property

        <XmlIgnore()> _
        Public FactTypeName As New FBM.FactTypeName

        <XmlIgnore()> _
        Public Shadows Property Name() As String
            Get
                Return _Name
            End Get
            Set(ByVal value As String)
                _Name = value
            End Set
        End Property        

        <XmlIgnore()> _
        <CategoryAttribute("Fact Type"), _
             DefaultValueAttribute(GetType(Integer), "0"), _
             DescriptionAttribute("The Cardinality of the Fact Type."), _
             ReadOnlyAttribute(True),
             Browsable(False)>
        Public ReadOnly Overrides Property Arity() As Integer
            Get
                Return Me.RoleGroup.Count
            End Get
        End Property

        <XmlIgnore()>
        Public Shadows Property IsObjectified() As Boolean
            Get
                Return Me._IsObjectified
            End Get
            Set(ByVal value As Boolean)
                Me._IsObjectified = value
            End Set
        End Property

        <XmlIgnore()>
        Public Shadows Property IsSubtypeStateControllin() As Boolean
            Get
                Return Me._IsSubtypeStateControlling
            End Get
            Set(ByVal value As Boolean)
                Me._IsSubtypeStateControlling = value
                Me.FactType.IsSubtypeStateControlling = value
            End Set
        End Property

        Public Shadows _IsPreferredReferenceMode As Boolean = False
        <CategoryAttribute("Reference Mode"), _
         DescriptionAttribute("Whether the Fact Type is for the RefernceMode of an ENtity type."), _
         BindableAttribute(True), _
         DefaultValueAttribute(False), _
         [ReadOnly](False), _
         Browsable(False)> _
        Public Overrides Property isPreferredReferenceMode() As Boolean
            Get
                Return _IsPreferredReferenceMode
            End Get
            Set(ByVal value As Boolean)
                Me._IsPreferredReferenceMode = value
                Me.FactType.IsPreferredReferenceMode = value
            End Set
        End Property

        Public Shadows LinkFactTypeRole As FBM.RoleInstance

        <XmlIgnore()> _
        <DebuggerBrowsable(DebuggerBrowsableState.Never)> _
        Private Shadows _RoleGroup As New List(Of FBM.RoleInstance)
        <XmlIgnore()> _
        Public Shadows Property RoleGroup() As List(Of FBM.RoleInstance)
            Get
                Return Me._RoleGroup
            End Get
            Set(ByVal value As List(Of FBM.RoleInstance))
                Me._RoleGroup = value
            End Set
        End Property
        'As New List(Of tRoleInstance)

        <XmlIgnore()> _
        Public Shadows InternalUniquenessConstraint As New List(Of FBM.RoleConstraintInstance)

        <XmlIgnore()> _
        Public Shadows Fact As New List(Of FBM.FactInstance)

        Public _Visible As Boolean = False
        Public Property Visible() As Boolean
            Get
                Return Me._Visible
            End Get
            Set(ByVal value As Boolean)
                Me._Visible = value
                If Me._Visible Then
                    Call Me.MakeVisible()
                Else
                End If
            End Set
        End Property

        <XmlIgnore()> _
        <Browsable(False), _
        [ReadOnly](True), _
        BindableAttribute(False)> _
        Public Shadows WithEvents Concept As New FBM.Concept

        <NonSerialized(), _
        XmlIgnore()> _
        Public Shape As ShapeNode

        <XmlIgnore()> _
        Public _X As Integer
        Public Property X As Integer Implements FBM.iPageObject.X
            Get
                Return Me._X
            End Get
            Set(ByVal value As Integer)
                Me._X = value
                'If IsSomething(Me.Shape) Then
                '    Dim loRectangle As New Rectangle(Me.X, Me.Shape.Bounds.Y, Me.Shape.Bounds.Width, Me.Shape.Bounds.Height)
                '    Me.Shape.SetRect(loRectangle, False)
                'End If
            End Set
        End Property

        <XmlIgnore()> _
        Public _Y As Integer
        Public Property Y As Integer Implements FBM.iPageObject.Y
            Get
                Return Me._Y
            End Get
            Set(ByVal value As Integer)
                Me._Y = value
                'If IsSomething(Me.Shape) Then
                '    Dim loRectangle As New Rectangle(Me.Shape.Bounds.X, Me.Y, Me.Shape.Bounds.Width, Me.Shape.Bounds.Height)
                '    Me.Shape.SetRect(loRectangle, False)
                'End If
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
                If IsSomething(Me.Shape) Then
                    Me.X = Me.Shape.Bounds.X
                    Me.Y = Me.Shape.Bounds.Y
                End If
            End Set
        End Property

        <NonSerialized(), _
        XmlIgnore()> _
        Public FactTable As New FBM.FactTable

        <NonSerialized(), _
        XmlIgnore()> _
        Public FactTypeNameShape As New ShapeNode

        <NonSerialized(), _
        XmlIgnore()> _
        Public FactTypeDerivationText As FBM.FactTypeDerivationText

        <NonSerialized(), _
        XmlIgnoreAttribute()> _
        Public FactTypeReadingShape As New FBM.FactTypeReadingInstance(Me) 'Removed 'Shadows FactTypeReading and replaced with FactTypeReadingShape'

        ''' <summary>
        ''' If the FactType represents a SubtypeConstraint, is the SubtypeConstraintInstance that this FactType represents.
        ''' Used for joining RoleConstraints to SubtypeConstraints on a Page.Diagram. The RoleConstraint.RoleConstraintRole.Link joins the FactType.SubtypeConstraintInstance.Link
        ''' </summary>
        ''' <remarks></remarks>
        <XmlIgnore()> _
        Public SubtypeConstraintInstance As FBM.SubtypeRelationshipInstance

        Public IsDisplayedAssociated As Boolean = False

        Public Sub New()
            '---------------------------
            'Parameterless Constructor
            '---------------------------
            MyBase.ConceptType = pcenumConceptType.FactType

            Me.m_dctd = DynamicTypeDescriptor.ProviderInstaller.Install(Me)
        End Sub

        Public Sub New(ByRef arModel As FBM.Model, _
                       ByRef arPage As FBM.Page, _
                       ByVal aiLanguageId As pcenumLanguage, _
                       Optional ByVal asFactTypeName As String = Nothing, _
                       Optional ByVal ab_use_name_as_id As Boolean = False, _
                       Optional ByVal aiX As Integer = 0, _
                       Optional ByVal aiY As Integer = 0)

            Call Me.New()

            Me.Model = arModel
            Me.Page = arPage

            '-----------------------------------------------
            'Create the FactTable for the FactTypeInstance
            '-----------------------------------------------
            Me.FactTable = New FBM.FactTable(Me.Page, Me)

            Me.FactTypeName.Model = arModel
            Me.FactTypeName.Page = arPage
            Me.FactTypeName.FactTypeInstance = Me
            Me.FactTypeName.FactType = Me.FactType

            If ab_use_name_as_id Then
                Me.Id = Trim(asFactTypeName)
            Else
                Me.Id = System.Guid.NewGuid.ToString
            End If

            If IsSomething(asFactTypeName) Then
                Me.Name = asFactTypeName
            Else
                Me.Name = "New Fact Type"
            End If

            Me.X = aiX
            Me.Y = aiY

        End Sub

        Public Sub New(ByRef arModel As FBM.Model, _
                       ByRef arPage As FBM.Page, _
                       ByVal aiLanguageId As pcenumLanguage, _
                       ByVal asFactTypeId As String, _
                       ByVal asFactTypeName As String)

            Call Me.New()

            Me.Model = arModel
            Me.Page = arPage

            '-----------------------------------------------
            'Create the FactTable for the FactTypeInstance
            '-----------------------------------------------
            Me.FactTable = New FBM.FactTable(Me.Page, Me)

            Me.FactTypeName.Model = arModel
            Me.FactTypeName.Page = arPage
            Me.FactTypeName.FactTypeInstance = Me
            Me.FactTypeName.FactType = Me.FactType


            Me.Id = Trim(asFactTypeId)
            Me.Name = asFactTypeName


        End Sub

        Public Shadows Function Equals(ByVal other As FBM.FactTypeInstance) As Boolean Implements System.IEquatable(Of FBM.FactTypeInstance).Equals

            If Me.Id = other.Id Then
                Return True
            Else
                Return False
            End If

        End Function

        Public Shadows Function EqualsByName(ByVal other As FBM.FactType) As Boolean

            If Trim(Me.Name) = Trim(other.Name) Then
                Return True
            Else
                Return False
            End If

        End Function

        Public Overloads Function Clone(ByRef arPage As FBM.Page, _
                                        Optional ByVal abAddToPage As Boolean = False, _
                                        Optional ByVal abIsMDAmodelElement As Boolean = False) As FBM.FactTypeInstance

            Dim lrFactTypeInstance As New FBM.FactTypeInstance
            Dim lrRoleInstance As FBM.RoleInstance
            Dim lrClonedRoleInstance As New FBM.RoleInstance
            Dim lrFactInstance As FBM.FactInstance

            Try

                If arPage.FactTypeInstance.Find(Function(x) x.Id = Me.Id) IsNot Nothing Then
                    lrFactTypeInstance = arPage.FactTypeInstance.Find(Function(x) x.Id = Me.Id)
                Else
                    With Me
                        lrFactTypeInstance.isDirty = True
                        lrFactTypeInstance.Model = arPage.Model
                        lrFactTypeInstance.Page = arPage
                        lrFactTypeInstance.FactTypeName.Page = arPage
                        lrFactTypeInstance.Id = .Id
                        lrFactTypeInstance.Name = .Name
                        lrFactTypeInstance.Symbol = .Symbol
                        lrFactTypeInstance.ShowFactTypeName = .ShowFactTypeName
                        lrFactTypeInstance.IsSubtypeRelationshipFactType = .IsSubtypeRelationshipFactType
                        lrFactTypeInstance.isPreferredReferenceMode = .isPreferredReferenceMode
                        lrFactTypeInstance.IsLinkFactType = .IsLinkFactType
                        If lrFactTypeInstance.IsLinkFactType Then
                            lrFactTypeInstance.LinkFactTypeRole = .LinkFactTypeRole.Clone(arPage, True)
                        End If

                        If abIsMDAmodelElement = False Then
                            lrFactTypeInstance.IsMDAModelElement = .IsMDAModelElement
                        Else
                            lrFactTypeInstance.IsMDAModelElement = abIsMDAmodelElement
                        End If

                        lrFactTypeInstance.IsDisplayedAssociated = False

                        lrFactTypeInstance._IsObjectified = ._IsObjectified

                        If arPage.Model.FactType.Exists(AddressOf .FactType.Equals) Then
                            lrFactTypeInstance.FactType = arPage.Model.FactType.Find(AddressOf .FactType.Equals)
                        Else
                            Dim lrFactType As New FBM.FactType
                            lrFactType = .FactType.Clone(arPage.Model, abAddToPage, abIsMDAmodelElement)
                            lrFactTypeInstance.FactType = lrFactType

                            If .IsObjectified Then
                                For Each lrLinkFactType In .FactType.getLinkFactTypes
                                    Call lrLinkFactType.Clone(arPage.Model, abAddToPage, lrLinkFactType.IsMDAModelElement)
                                Next

                                '20200805-VM-I don't know why this code was here at all. Remove if all okay.
                                'Move Relations that reference this FactType to their respective LinkFactType
                                'If Not abIsMDAmodelElement Then
                                '    Call arPage.Model.moveRelationsOfFactTypeToRespectiveLinkFactTypes(lrFactType)
                                'End If
                            End If
                        End If

                        lrFactTypeInstance.FactTypeName = .FactTypeName.Clone(arPage, lrFactTypeInstance)

                        lrFactTypeInstance.Shape = New ShapeNode
                        lrFactTypeInstance.X = .X
                        lrFactTypeInstance.Y = .Y

                        For Each lrRoleInstance In .RoleGroup
                            lrClonedRoleInstance = lrRoleInstance.Clone(arPage, abAddToPage)
                            lrClonedRoleInstance.FactType = lrFactTypeInstance
                            lrFactTypeInstance.RoleGroup.Add(lrClonedRoleInstance)
                        Next

                        '------------------------------------------------------------
                        'Needs to be after RoleInstances have been added to the FactTypeInstance 
                        '  so that the TableNode has the right ColumnCount
                        lrFactTypeInstance.FactTable = New FBM.FactTable(arPage, lrFactTypeInstance)

                        For Each lrFactInstance In .Fact
                            lrFactTypeInstance.Fact.Add(lrFactInstance.Clone(arPage, lrFactTypeInstance))
                        Next

                        If abAddToPage Then
                            If Not arPage.FactTypeInstance.Exists(AddressOf lrFactTypeInstance.Equals) Then
                                arPage.FactTypeInstance.AddUnique(lrFactTypeInstance)
                            End If
                        End If

                    End With
                End If
                Return lrFactTypeInstance

            Catch ex As Exception
                Dim lsMessage As String = ""

                lsMessage = "Error: FBM.tFactTypeInstance.Clone: " & vbCrLf & vbCrLf & ex.Message
                Call prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)

                Return lrFactTypeInstance
            End Try

        End Function

        Public Function CloneConceptInstance() As FBM.ConceptInstance

            Dim lrConceptInstance As New FBM.ConceptInstance(Me.Model, Me.Page, Me.Id, Me.ConceptType)

            lrConceptInstance.X = Me.X
            lrConceptInstance.Y = Me.Y
            lrConceptInstance.Visible = True
            lrConceptInstance.ConceptType = pcenumConceptType.FactType

            Return lrConceptInstance

        End Function

        ''' <summary>
        ''' Creates a cloned list of the Facts in the FactType
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overrides Function CloneFacts() As List(Of FBM.Fact)

            Dim lrFactInstance As FBM.Fact
            Dim larFactInstance As New List(Of FBM.Fact)

            For Each lrFactInstance In Me.Fact
                larFactInstance.Add(lrFactInstance.Clone())
            Next

            Return larFactInstance

        End Function

        Public Sub MouseDown() Implements FBM.iPageObject.MouseDown

            Me.Page.SelectedObject.Add(Me)
            Me.Shape.Pen.Color = Color.Blue
            Me.Shape.Selected = True

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

            '---------------------------------------------------------------------------------------------
            'NB Normally for a PageObject the following code would appear in this method.
            '  But for FactTypeInstances, the FactTypeInstance is only added to the SelectedObject list
            '  for the Page when the MouseDown method is invoked.
            '  This is so that FactTypeInstances aren't added to the SelectedObject list when only their
            '  Role has been selected (and who's Role.NodeSelected selects the FactTypeInstance.Shape
            '  also so that if the user moves the Role, the FactTypeInstance moves as well).
            '
            'Me.Page.SelectedObject.Add(Me)
            '
            'See MouseDown method in this class
            'See NodeSelected method for a RoleInstance.
            '
            'See also Me.Selected for setting appropriate color
            '----------------------------------------------------------------------------------------------

        End Sub

        Public Sub AddRoleInstance(ByRef arRoleInstance As FBM.RoleInstance)

            Me.RoleGroup.Add(arRoleInstance)
            Me.Page.RoleInstance.AddUnique(arRoleInstance)
        End Sub

        Public Sub BringStrandedJoinedObjectsCloser()



            Dim liNewX, liNewY As Integer

            If Me.isPreferredReferenceMode Then Exit Sub

            Dim laiConceptType = {pcenumConceptType.ValueType, pcenumConceptType.EntityType}

            If Me.Arity = 2 Then Exit Sub

            For Each lrRoleInstance In Me.RoleGroup.FindAll(Function(x) laiConceptType.Contains(x.JoinedORMObject.ConceptType))

                Call Me.getBlankCellCloseBy(liNewX, liNewY)

                If lrRoleInstance.JoinedORMObject.GetAdjoinedRoles(True).Count = 1 Then
                    'That ModelElement is only linked to this FactTypeInstance
                    Select Case lrRoleInstance.JoinedORMObject.ConceptType
                        Case Is = pcenumConceptType.ValueType
                            Dim lrValueTypeInstance As FBM.ValueTypeInstance = lrRoleInstance.JoinedORMObject
                            lrValueTypeInstance.Move(liNewX, liNewY, True)
                        Case Is = pcenumConceptType.EntityType
                            Dim lrEntityTypeInstance As FBM.EntityTypeInstance = lrRoleInstance.JoinedORMObject
                            lrEntityTypeInstance.Move(liNewX, liNewY, True)
                    End Select
                End If
            Next

        End Sub

        Private Sub getBlankCellCloseBy(ByRef aiX As Integer, ByRef aiY As Integer)

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
                    liNewX = Me.X - 30
                Case Is = 1
                    liNewX = Me.X + 2
                Case Is = 2
                    liNewX = Me.X + 40
            End Select

            Select Case liLowestY
                Case Is = 0
                    liNewY = Me.Y - 20
                Case Is = 1
                    liNewY = Me.Y + 2
                Case Is = 2
                    liNewY = Me.Y + 31
            End Select

            aiX = liNewX
            aiY = liNewY

        End Sub

        ''' <summary>
        ''' Removes the FactTypeInstance from the Page on which it is placed.
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub RemoveFromPage(ByVal abBroadcastInterfaceEvent As Boolean)

            Dim lrRoleInstance As FBM.RoleInstance

            Try
                For Each lrRoleInstance In Me.RoleGroup.ToArray
                    Call lrRoleInstance.RemoveFromPage()
                Next

                'CodeSafe
                For Each lrRoleInstance In Me.Page.RoleInstance.FindAll(Function(x) x.FactType.Id = Me.Id).ToArray
                    Call lrRoleInstance.RemoveFromPage()
                Next

                Dim lrRoleConstraintInstance As FBM.RoleConstraintInstance

                For Each lrRoleConstraintInstance In Me.InternalUniquenessConstraint.ToArray
                    Call lrRoleConstraintInstance.RemoveFromPage(abBroadcastInterfaceEvent)
                Next

                Call Me.FactTypeName.RemoveFromPage(abBroadcastInterfaceEvent)

                Call Me.Page.RemoveFactTypeInstance(Me, abBroadcastInterfaceEvent)

                Call Me.Page.MakeDirty()

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try
        End Sub

        Public Function ClonePageObject() As FBM.PageObject

            Dim lrPageObject As New FBM.PageObject

            lrPageObject.Name = Me.Name
            lrPageObject.Shape = Me.Shape

            Return lrPageObject

        End Function

        Public Shadows Sub AddInternalUniquenessConstraint(ByRef arRoleConstraintInstance As FBM.RoleConstraintInstance)

            Me.InternalUniquenessConstraint.Add(arRoleConstraintInstance)

        End Sub

        Public Overloads Sub CreateInternalUniquenessConstraint(ByRef aarRole As List(Of FBM.Role))

            'Dim lrRoleInstance As FBM.RoleInstance
            Dim lrRoleConstraint As FBM.RoleConstraint

            Try
                '--------------------------------------------------
                'Create the RoleConstraint within the WorkingModel
                '--------------------------------------------------
                lrRoleConstraint = Me.FactType.CreateInternalUniquenessConstraint(aarRole)

                '-------------------------------------------------------
                'Assign the UniquenessConstraint(Instance) Shape to the 
                '  corresponding RoleConstraint object
                '-------------------------------------------------------
                'lrRoleConstraintInstance = lrRoleConstraint.CloneUniquenessConstraintInstance(Me.Page)

                Call Me.Page.MakeDirty()

            Catch ex As Exception
                MsgBox("AddInternalUniquenessConstraint: " & ex.Message)
            End Try

        End Sub


        Public Shadows Function AddFact(ByVal arFact As FBM.Fact, Optional ByVal abResortFactTable As Boolean = False) As FBM.FactInstance

            Dim lrFactInstance As FBM.FactInstance

            lrFactInstance = arFact.CloneInstance(Me.Page, False, True)

            lrFactInstance.isDirty = True
            Me.isDirty = True
            Me.Page.IsDirty = True
            Me.Fact.Add(lrFactInstance)

            Me.Page.FactInstance.Add(lrFactInstance)

            If IsSomething(Me.FactTable) And abResortFactTable Then
                Call Me.FactTable.ResortFactTable()
                'Me.FactTable.TableShape.ResizeToFitText(True)
            End If

            Return lrFactInstance

        End Function

        Public Sub AddFactInstance(ByRef arFactInstance As FBM.FactInstance)

            Me.Fact.Add(arFactInstance)
            Me.Page.FactInstance.Add(arFactInstance)

            Me.isDirty = True
            Me.Page.IsDirty = True
        End Sub


        Public Shadows Function AddRole(ByRef arRole As FBM.Role, ByRef aoJoinedObject As Object) As FBM.RoleInstance

            Dim lrRoleInstance As FBM.RoleInstance

            '----------------------------------------------------------------------
            'Increment the Arity of this FactType because are adding an extra Role
            '----------------------------------------------------------------------
            lrRoleInstance = New FBM.RoleInstance(Me, aoJoinedObject, arRole)

            Call lrRoleInstance.DisplayAndAssociate(Me)

            '-------------------------------------------------------------------
            'Increment the Column Count of the FactTable of the FactType
            '-------------------------------------------------------------------
            Me.FactTable.TableShape.AddColumn()
            Me.FactTable.TableShape.Columns(Me.FactTable.TableShape.ColumnCount - 1).Width = 20

            '------------------------------------------------------------------------------------------
            'Sort the RoleInstances so that the Roles within the FactType/RoleGroup are in the correct
            '  visual order (in line with the objects to which they join)
            '------------------------------------------------------------------------------------------
            Me.SortRoleGroup()

            AddRole = lrRoleInstance

        End Function

        ''' <summary>
        ''' Adjusts the border height of the FactType in relation to the (number of) internal uniqueness constraints associated with the FactType.
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub AdjustBorderHeight()

            Try
                '-----------------------------------------------------------
                'CodeSafe: Sometimes gets called when removing a FactType.
                If Me.RoleGroup.Count = 0 Then Exit Sub

                Dim lrRoleInstance As FBM.RoleInstance

                If Me.Shape Is Nothing Then Exit Sub

                'VM-20180330-ToDo: This next line might never allow anything through. Test and remove if necessary
                If Me.Shape.Parent Is Nothing Then Exit Sub

                For Each lrRoleInstance In Me.RoleGroup
                    lrRoleInstance.Shape.Detach()
                Next

                Me.Y = Me.RoleGroup(0).Y - 3 - (Me.GetHighestConstraintLevel * 2)
                Me.Shape.Resize(Me.Shape.Bounds.Width, 12 + 4 + ((Me.GetHighestConstraintLevel - 1) * 2))

                Me.Shape.Move(Me.X, Me.Y)
                Call Me.Shape.ZBottom()

                If Me.FactTypeReadingShape.Shape IsNot Nothing Then
                    Me.FactTypeReadingShape.Shape.Move(((Me.Shape.Bounds.Width / 2) + Me.Shape.Bounds.X) - (Me.FactTypeReadingShape.Shape.Bounds.Width / 2), (Me.Shape.Bounds.Y + Me.Shape.Bounds.Height) - 6) 'FactTypeReadingShape.Shape.Bounds.Y)
                End If

                For Each lrRoleInstance In Me.RoleGroup
                    lrRoleInstance.Shape.AttachTo(Me.Shape, AttachToNode.MiddleLeft)
                Next

                If Me.FactTypeName IsNot Nothing Then
                    If Me.FactTypeName.Shape IsNot Nothing Then
                        If Me.FactTypeName.Shape.Bounds.Y.isBetween(Me.Y - 3, Me.Y + 3) Then
                            Me.FactTypeName.Shape.Move(Me.FactTypeName.Shape.Bounds.X, Me.Y - 8)
                        End If
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

        Public Sub Delete()
            '--------------------------------------------------------------------------------
            'Deletes the FactTypeInstance from the database.
            '  NB Does not delete any of the following associated with the FactTypeInstance
            '  a. Roles;
            '  b. RoleConstraints
            '
            '  i.e. Only deletes at the 'Page' level, rather than at the Model level.
            '--------------------------------------------------------------------------------
            Call TableFactTypeInstance.DeleteFactTypeInstance(Me)

        End Sub

        ''' <summary>
        ''' Deletes a FactInstance from a FactTypeInstance.
        '''   NB Does not remove the Fact from the Model level FactType.
        ''' </summary>
        ''' <param name="arFactInstance">The FactInstance to be deleted.</param>        
        ''' <remarks></remarks>
        Public Shadows Sub RemoveFact(ByRef arFactInstance As FBM.FactInstance)

            Dim lrFactInstance_list As New List(Of FBM.FactInstance)
            Dim lrFactInstance As FBM.FactInstance

            lrFactInstance = Me.Fact.Find(AddressOf arFactInstance.Equals)

            If IsSomething(lrFactInstance) Then
                '---------------------------------------------------
                'Remove the FactInstance from the FactTypeInstance
                '---------------------------------------------------
                Me.Fact.Remove(lrFactInstance)

                '------------------------------------------------------------------------------------------------
                'Delete the ConceptInstance for the FactInstance from the ConceptInstance table in the database
                '------------------------------------------------------------------------------------------------
                Dim lrConceptInstance As New FBM.ConceptInstance(Me.Model, Me.Page, arFactInstance.Symbol, pcenumConceptType.Fact)
                If TableConceptInstance.ExistsConceptInstance(lrConceptInstance) Then
                    TableConceptInstance.DeleteConceptInstance(lrConceptInstance)
                End If
            End If

            If IsSomething(Me.FactTable) Then
                Call Me.FactTable.ResortFactTable()
            End If

            Me.Page.MakeDirty()
            RaiseEvent updated()

        End Sub

        Public Shadows Sub RemoveFactById(ByRef arFact As FBM.Fact)

            Dim lrFactInstance_list As New List(Of FBM.FactInstance)
            Dim lrFactInstance As New FBM.FactInstance
            Dim lrFact As FBM.Fact = arFact

            lrFactInstance = Me.Fact.Find(Function(x) x.Id = lrFact.Id)

            If IsSomething(lrFactInstance) Then
                '---------------------------------------------------
                'Remove the FactInstance from the FactTypeInstance
                '---------------------------------------------------
                Me.Fact.Remove(lrFactInstance)

                '------------------------------------------------------------------------------------------------
                'Delete the ConceptInstance for the FactInstance from the ConceptInstance table in the database
                '------------------------------------------------------------------------------------------------
                Dim lrConceptInstance As New FBM.ConceptInstance(Me.Model, Me.Page, arFact.Symbol, pcenumConceptType.Fact)
                If TableConceptInstance.ExistsConceptInstance(lrConceptInstance) Then
                    TableConceptInstance.DeleteConceptInstance(lrConceptInstance)
                End If
            End If

            If IsSomething(Me.FactTable) And Me.Page.Language = pcenumLanguage.ORMModel Then
                Call Me.FactTable.ResortFactTable()
            End If

            Me.Page.MakeDirty()
            RaiseEvent updated()

        End Sub

        Public Function HasNULLRole() As Boolean

            Return Me.RoleGroup.FindAll(Function(x) x.JoinedORMObject Is Nothing).Count > 0

        End Function

        Public Overloads Function HasPartialButMultiRoleConstraint() As Boolean
            '---------------------------------------------------------
            'RETURNS TRUE if the FactType has a partial role constraint
            ' (but more than unary and less than total)
            'ELSE RETURNS FALSE
            '---------------------------------------------------------
            'This Function is used to see if a RoleGroup
            'has an internal uniqueness constraint spanning
            'more than 1 role but not a total_RoleConstraint
            '---------------------------------------------------------

            'Dim liInd As Integer

            HasPartialButMultiRoleConstraint = False

            If Me.HasTotalRoleConstraint() Then
                HasPartialButMultiRoleConstraint = False
                Exit Function
            End If

            Dim lrRoleConstraint As FBM.RoleConstraintInstance

            If Me.InternalUniquenessConstraint.Count > 0 Then
                For Each lrRoleConstraint In Me.InternalUniquenessConstraint
                    If (Me.Arity > 2) And (lrRoleConstraint.RoleConstraintRole.Count >= 2) Then
                        Return True
                    End If
                Next
            End If
            'For liInd = 1 To prApplication.workingpage.RoleConstraintInstance_count
            '    If (prApplication.workingpage.RoleConstraintInstance(liInd).constraint_type = 0) And (prApplication.workingmodel.getRole_by_id(prApplication.workingpage.RoleConstraintInstance(liInd).joins_RoleId).MasterRole.Id = Me.MasterRole.Id) Then
            '        'then is internal uniqueness constraint and in this RoleGroup
            '        If prApplication.workingpage.RoleConstraintInstance(liInd).get_constraint_arity > 1 Then
            '            HasPartialButMultiRoleConstraint = True
            '            Exit Function
            '        End If
            '    End If
            'Next liInd

        End Function

        Public Overloads Function HasTotalRoleConstraint() As Boolean

            '------------------------------------------------------------------------
            'RETURNS TRUE if a Total Role Constraint exists within
            'the FactType
            'Else returns False
            '------------------------------------------------------------------------
            'PSEUDOCODE
            '  * IF the RoleGroup.Constraint.Count < FactType.Cardinality THEN
            '      * RETURN FALSE
            '    ELSE IF the RoleGroup.Constraint.Count = FactType.Cardinality THEN
            '      * RETURN TRUE
            '    ELSE
            '      * IF the count of InternalUniquenessConstraints at level 1 = 
            '           the FactType.Cardinality for the RoleGroup THEN
            '         * RETURN TRUE
            '    END IF
            '------------------------------------------------------------------------

            If Me.InternalUniquenessConstraint.Count = 1 Then
                If Me.InternalUniquenessConstraint(0).RoleConstraintRole.Count = Me.Arity Then
                    '-------------------------------------------------
                    'Then must be a TotalInternalUniquenessConstraint
                    '-------------------------------------------------
                    Return True
                Else
                    Return False
                End If
            Else
                Return False
            End If

        End Function

        ''' <summary>
        ''' Hides the FactTypeInstance if if is displayed on the Page.
        '''   NB Predominantly used for hiding a SubtypeRelationship FactTypeInstance after it has been shown.
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub Hide()

            If Me.Page.Diagram IsNot Nothing Then
                If Me.Shape IsNot Nothing Then
                    Call Me.Shape.ZBottom()
                    Me.Shape.Visible = False
                    For Each lrRoleInstance In Me.RoleGroup
                        Call lrRoleInstance.Link.ZBottom()
                        lrRoleInstance.Shape.Visible = False
                        lrRoleInstance.Link.Visible = False
                    Next

                    If Me.FactTypeReadingShape.Shape IsNot Nothing Then
                        Call Me.FactTypeReadingShape.Shape.ZBottom()
                        Me.FactTypeReadingShape.Shape.Visible = False
                    End If

                    For Each lrRoleConstraint In Me.InternalUniquenessConstraint
                        Call lrRoleConstraint.Shape.ZBottom()
                        For Each lrRoleConstraintRole In lrRoleConstraint.RoleConstraintRole
                            lrRoleConstraintRole.Shape.Visible = False
                        Next
                    Next

                    For Each lrRole In Me.RoleGroup
                        If lrRole.Shape IsNot Nothing Then
                            lrRole.RoleName.Shape.ZBottom()
                            lrRole.RoleName.Shape.Visible = False
                        End If
                    Next

                    Dim larRoleConstraintRole = From RoleConstraintInstance In Me.Page.RoleConstraintInstance _
                                                From RoleConstraintRoleInstance In RoleConstraintInstance.RoleConstraintRole _
                                                Where Me.RoleGroup.FindAll(Function(x) x.Id = RoleConstraintRoleInstance.Role.Id).Count > 0
                                                Select RoleConstraintRoleInstance

                    For Each lrRoleConstraintRoleInstance In larRoleConstraintRole
                        If lrRoleConstraintRoleInstance.Link IsNot Nothing Then
                            Me.Page.Diagram.Links.Remove(lrRoleConstraintRoleInstance.Link)
                        End If
                    Next

                End If
            End If

        End Sub

        Public Overloads Function FindFirstRoleByModelObject(ByRef arModelObject As FBM.ModelObject) As FBM.RoleInstance

            '-----------------------------
            'Initialise the Return value
            '-----------------------------
            FindFirstRoleByModelObject = Nothing

            Dim lrRole As FBM.RoleInstance

            For Each lrRole In Me.RoleGroup
                If lrRole.JoinedORMObject.Id = arModelObject.Id Then
                    FindFirstRoleByModelObject = lrRole
                End If
            Next



        End Function

        Public Shadows Function FindSuitableFactTypeReading() As FBM.FactTypeReadingInstance
            '-----------------------------------------------------------------------------------
            'Finds a FactTypeReading within the FactType based on the order of 
            '  the Roles/JoinedModelObjects withn the RoleGroup
            '
            '  * The reason for this function is that when a user moves an ObjectType associated
            '  with the FactTypeInstance within a Page, the RoleGroup within the 
            '  FactTypeInstance is sorted. When this happens, the FactTypeReading (as it appears)
            '  may no longer match the RoleGroup. This function looks up the FactTypeReadings and
            '  tries to find a FactTypeReading with the ORMObjectTypes in the same sequential order
            '  as the argument list.
            '  e.g. If a FactType is for three Entity Types and is in the order Part, Bin, Warehouse
            '  this function will look for a FactTypeReading with the same sequential order.
            '  (e.g. 'A Part is in a Bin in a Warehouse'.
            '-----------------------------------------------------------------------------------
            Try
                Dim lrFactTypeReading As New FBM.FactTypeReading(Me.FactType, Me.Id)
                Dim lar_ORM_object_type As New List(Of FBM.ModelObject)

                Dim lrPredicatePart As FBM.PredicatePart
                For Each lrRoleInstance In Me.RoleGroup
                    lrPredicatePart = New FBM.PredicatePart(Me.Model, lrFactTypeReading, lrRoleInstance.Role)
                    lrFactTypeReading.PredicatePart.Add(lrPredicatePart)
                Next

                If Me.FactType.FactTypeReading.Count > 0 Then

                    lrFactTypeReading = Me.FactType.FactTypeReading.Find(AddressOf lrFactTypeReading.MatchesByRoles)
                    If IsSomething(lrFactTypeReading) Then

                        If Me.Page.Diagram IsNot Nothing Then
                            Me.Page.Diagram.Nodes.Remove(Me.FactTypeReadingShape.Shape)
                            Me.FactTypeReadingShape = lrFactTypeReading.CloneInstance(Me.Page)
                            If Me.FactTypeReadingShape.FactType IsNot Nothing Then
                                Call Me.FactTypeReadingShape.DisplayAndAssociate()
                                Call Me.FactTypeReadingShape.RefreshShape()
                            End If
                        End If

                        FindSuitableFactTypeReading = lrFactTypeReading.CloneInstance(Me.Page)
                    Else
                        If Me.FactType.FactTypeReading.Count > 0 Then
                            If Me.FactType.Arity = 2 Then
                                If Me.FactTypeReadingShape.Shape IsNot Nothing Then
                                    Me.Page.Diagram.Nodes.Remove(Me.FactTypeReadingShape.Shape)
                                End If
                                Me.FactTypeReadingShape = Me.FactType.FactTypeReading(0).CloneInstance(Me.Page)
                                'CodeSafe: Add the FactTypeIntsance to the FactTypeReadingShape, because the FactTypeInstance may have been removed from the Page
                                'This happens when deleting an EntityType with a ReferenceMode from the Model/Page.
                                Me.FactTypeReadingShape.FactType = Me
                                Call Me.FactTypeReadingShape.DisplayAndAssociate()
                                Call Me.FactTypeReadingShape.RefreshShape()

                                FindSuitableFactTypeReading = Me.FactTypeReadingShape

                            Else
                                FindSuitableFactTypeReading = Nothing
                            End If
                        Else
                            FindSuitableFactTypeReading = Nothing
                        End If

                    End If
                Else
                    FindSuitableFactTypeReading = Nothing
                End If

                If FindSuitableFactTypeReading Is Nothing _
                    And Me.FactTypeReadingShape IsNot Nothing Then
                    If Me.FactTypeReadingShape.Shape IsNot Nothing Then
                        Me.Page.Diagram.Nodes.Remove(Me.FactTypeReadingShape.Shape)
                        Me.FactTypeReadingShape = New FBM.FactTypeReadingInstance(Me, Nothing)
                        Call Me.FactTypeReadingShape.DisplayAndAssociate()
                        Call Me.FactTypeReadingShape.RefreshShape()
                        If Me.FactTypeReadingShape.Shape IsNot Nothing Then 'Can be nothing when emptying a Model.                            
                            Me.FactTypeReadingShape.Shape.Text = ""
                        End If
                    End If
                End If

                If Me.Shape IsNot Nothing Then
                    Me.Shape.ZBottom()

                    Me.Page.Diagram.Invalidate()
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

        ''' <summary>
        ''' Returns the Model level ModelObject for this Instance.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overrides Function getBaseModelObject() As FBM.ModelObject

            Return Me.FactType

        End Function

        ''' <summary>
        ''' Returns a count of the Roles within the RoleGroup of the FactType, where those Roles join a FactType.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks>Example of use: Sorting FactTypes.</remarks>
        Public Overrides Function GetCountRolesJoiningFactTypes() As Integer

            Dim liCount As Integer = 0
            Dim lrRole As FBM.RoleInstance

            For Each lrRole In Me.RoleGroup
                If lrRole.TypeOfJoin = pcenumRoleJoinType.FactType Then
                    liCount += 1
                    liCount += lrRole.JoinsFactType.GetCountRolesJoiningFactTypes
                End If
            Next

            Return liCount

        End Function

        Sub GetFactInstancesFromDatabase()

            If Not Me.FactType.IsMDAModelElement Or Me.Id = pcenumCMMLRelations.CoreElementHasElementType.ToString Then
                Call GetFactsForFactTypeInstance(Me)
            End If

        End Sub

        Public Overloads Overrides Function GetHighestConstraintLevel() As Integer

            '------------------------------------------------------------------------
            'RETURNS the highest level of InternalUniquenessConstraint on a FactType
            '------------------------------------------------------------------------
            GetHighestConstraintLevel = Me.InternalUniquenessConstraint.Count

        End Function

        Public Function GetObjectTypeList() As List(Of FBM.ModelObject)

            Dim lrRoleInstance As FBM.RoleInstance

            GetObjectTypeList = New List(Of FBM.ModelObject)

            For Each lrRoleInstance In Me.RoleGroup
                GetObjectTypeList.Add(lrRoleInstance.JoinedORMObject)
            Next

        End Function

        ''' <summary>
        ''' RETURNS the complimentory Role of the binary FactType in which asRoleId is involved 
        ''' </summary>
        ''' <param name="asRoleId">The id of the Role for which the complimentory Role is to be retrieved.</param>
        ''' <returns></returns>
        ''' <remarks>Assumes that the role_record group is populated returns an error if called with a l_RoleId that belongs to other than a binary group</remarks>
        Public Overloads Function GetOtherRoleOfBinaryFactType(ByVal asRoleId As String) As FBM.RoleInstance

            Dim lrRole As FBM.RoleInstance

            If Not Me.IsBinaryFactType() Then
                Throw New System.Exception("Error: FBM.tFactType.GetOtherRoleOfBinaryFactType: Non binary FactType for Role: aiRoleId: " & asRoleId)
            End If

            GetOtherRoleOfBinaryFactType = Nothing

            For Each lrRole In Me.RoleGroup
                If Not (lrRole.Id = asRoleId) Then
                    GetOtherRoleOfBinaryFactType = lrRole
                    Exit For
                End If
            Next

        End Function

        Public Sub IncrementArity()

            Me.FactTable.TableShape.AddColumn()
            Me.FactTable.TableShape.Columns(Me.FactTable.TableShape.Columns.Count - 1).ColumnStyle = ColumnStyle.AutoWidth
            '--------------------------------------------------------------
            'Adjust the FactTypeInstance.FactTable TableShape.Bounds.Width
            '--------------------------------------------------------------
            Dim lo_rectangle As New Rectangle(Me.Shape.Bounds.X, (Me.RoleGroup(0).Shape.Bounds.Y + Me.RoleGroup(0).Shape.Bounds.Height + 2), Me.Shape.Bounds.Width, 20)
            Me.FactTable.TableShape.SetRect(lo_rectangle, False)
            Me.FactTable.TableShape.CaptionHeight = 4

        End Sub

        ''' <summary>
        ''' Removes a RoleInstance from the RoleGroup of the FactTypeInstance
        ''' </summary>
        ''' <param name="arRoleInstance"></param>
        ''' <remarks></remarks>
        Shadows Sub RemoveRole(ByRef arRoleInstance As FBM.RoleInstance)

            Dim lrRoleInstance As FBM.RoleInstance
            Dim liInd As Integer = 0

            Try

                Dim lsRoleId As String = arRoleInstance.Id
                Dim larFactDataInstance = From f In Me.Fact, _
                                             fdi In f.Data _
                                            Where fdi.Role.Id = lsRoleId _
                                            Select fdi

                Me.Page.RemoveRoleInstance(arRoleInstance)
                Me.RoleGroup.Remove(arRoleInstance)

                '-------------------------------------------------------------------------------
                'Remove the Role from any FactTypeReading associated with the FactTypeInstance
                '-------------------------------------------------------------------------------
                Dim lrFactTypeReadingInstance As FBM.FactTypeReadingInstance
                For Each lrFactTypeReadingInstance In Me.FactTypeReading
                    lrFactTypeReadingInstance.RemovePredicatePartForRole(arRoleInstance)
                Next

                '----------------------------------------------------------------------------------------
                'Remove the RoleInstance from any FactTypeReading associated with the FactTypeInstance.
                '  NB Because the FactTypeReading is 'Shadowed' in the FactTypeInstance
                '  from the parent FactType, need to call remove_PredicateParts...here as well.
                '----------------------------------------------------------------------------------------
                'Me.FactTypeReading.RemovePredicatePartsForObjectType(arRoleInstance.JoinedORMObject)
                Call Me.FactTypeReadingShape.RefreshShape()

                '-------------------
                'Page is now Dirty
                '-------------------
                Me.Page.IsDirty = True

                If Me.Arity > 0 Then
                    'NB Case when Arity=0 (i.e. Remove the FactTypeInstance, should be taken care of at the Model.FactType level.
                    '----------------------------------------------------
                    'Resize the FactType because a Role has been removed
                    '  i.e. Need to shorten the FactTypeInstance.Shape
                    '----------------------------------------------------
                    Call Me.Resize()

                    Call Me.ResetAnchorsForRoleGroup()

                    Call Me.SortRoleGroupByRoleSequenceNr()
                    Call Me.SortRoleGroup()

                    '-----------------------------------------------------------------
                    'Display and Associate the RoleInstances of the FactTypeInstance
                    '-----------------------------------------------------------------        
                    For Each lrRoleInstance In Me.RoleGroup
                        '---------------------------------------------------------------------
                        'Associate the Role/RoleInstance to the FactType/FactTypeInstance
                        '---------------------------------------------------------------------                        
                        Me.Page.RemoveRoleInstance(lrRoleInstance)
                        If Me.Shape IsNot Nothing Then
                            Call lrRoleInstance.DisplayAndAssociate(Me)
                        End If
                    Next

                    Call Me.RefreshShape()
                End If


                If Me.Shape IsNot Nothing Then
                    Call Me.Shape.ZBottom()

                    liInd = 0
                    For Each lrRoleInstance In Me.RoleGroup
                        If liInd = 0 Then
                            lrRoleInstance.Shape.AttachTo(Me.Shape, AttachToNode.MiddleLeft)
                        Else
                            lrRoleInstance.Shape.AttachTo(Me.Shape, AttachToNode.BottomCenter)
                        End If
                        liInd += 1
                    Next

                    For Each lrInternalUniquenessConstraint In Me.InternalUniquenessConstraint
                        For Each lrRoleConstraintRole In lrInternalUniquenessConstraint.RoleConstraintRole
                            lrRoleConstraintRole.Shape.AttachTo(lrRoleConstraintRole.Role.Shape, AttachToNode.TopCenter)
                        Next
                    Next
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
        ''' Saves the FactTypeInstance to the database
        ''' </summary>
        ''' <param name="abRapidSave"></param>
        ''' <remarks></remarks>
        Public Overloads Sub Save(Optional ByVal abRapidSave As Boolean = False)

            Dim lrFactInstance As FBM.FactInstance

            If abRapidSave Then
                Call TableFactTypeInstance.AddFactTypeInstance(Me)
            Else
                If TableConceptInstance.ExistsConceptInstance(Me.CloneConceptInstance) Then
                    Call TableFactTypeInstance.update_FactTypeInstance(Me)
                Else
                    Call TableFactTypeInstance.AddFactTypeInstance(Me)
                End If
            End If

            '--------------------------------------------------------------------------
            'Save the FactTypeName (ConceptInstance) to the Database.
            '  The (X,Y) Position and 'Visible' values of a FactTypeName is stored 
            '  as a ModelConceptInstance tuple.
            '--------------------------------------------------------------------------
            Call Me.FactTypeName.Save(abRapidSave)

            If Me.FactType.IsDerived And Me.FactTypeDerivationText IsNot Nothing Then
                Call Me.FactTypeDerivationText.Save(abRapidSave)
            End If

            '--------------------------------------------
            'Save any Facts within the FactTypeInstance
            '--------------------------------------------
            For Each lrFactInstance In Me.Fact.FindAll(Function(x) x.isDirty)
                Try
                    lrFactInstance.Save(abRapidSave)
                Catch arErr As Exception
                    Dim lsMessage As String
                    lsMessage = "Error: FBM.tFactTypeInstance.Save: "
                    lsMessage &= vbCrLf & "FactTypeId: " & Me.Id
                    lsMessage &= vbCrLf & vbCrLf & arErr.Message
                    prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, arErr.StackTrace)
                End Try
            Next

            Me.isDirty = False

        End Sub

        Public Sub Selected()

            Try
                Me.Shape.Pen.Color = Color.Blue

                If IsSomething(Me.FactTable.TableShape) Then
                    Me.FactTable.TableShape.Pen.Color = Color.Black
                    Me.FactTable.TableShape.ZTop()
                End If

                If Me.IsLinkFactType Then
                    Me.Page.RoleInstance.Find(Function(x) x.Id = Me.LinkFactTypeRole.Id).Shape.Brush = _
                        New MindFusion.Drawing.SolidBrush(Color.FromArgb( _
                                                          [Enum].Parse(GetType(pcenumColourWheel), [Enum].GetName(GetType(pcenumColourWheel), pcenumColourWheel.LightPurple)) _
                                                          ))
                End If

                For Each lrRoleInstance In Me.RoleGroup
                    lrRoleInstance.Shape.Text = ""
                    lrRoleInstance.Shape.Brush = New MindFusion.Drawing.SolidBrush(Color.White)
                Next

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        ''' <summary>
        ''' Shows the FactTypeInstance if it has already been DisplayedAndAssociated. e.g. After the 'Hide' method has been called.
        '''   Predominantly used when showing/hiding a FactType/Instance for a SubtypingRelationship.
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub Show()

            Try
                If Me.Page.Diagram IsNot Nothing Then
                    If Me.Shape IsNot Nothing Then
                        Me.Shape.Visible = True
                        Call Me.Shape.ZBottom()
                        For Each lrRoleInstance In Me.RoleGroup
                            lrRoleInstance.Shape.Visible = True
                            lrRoleInstance.Link.Visible = True

                            Select Case lrRoleInstance.TypeOfJoin
                                Case Is = pcenumRoleJoinType.ValueType
                                    lrRoleInstance.JoinsValueType.Shape.Visible = True
                            End Select

                        Next

                        If Me.FactTypeReadingShape.Shape IsNot Nothing Then                        
                            Me.FactTypeReadingShape.Shape.Visible = True
                        End If

                        For Each lrRoleConstraint In Me.InternalUniquenessConstraint
                            For Each lrRoleConstraintRole In lrRoleConstraint.RoleConstraintRole
                                lrRoleConstraintRole.Shape.Visible = True
                            Next
                        Next
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
        ''' Resorts the RoleGroup of the FactType with the associated ObjectTypes of each Role for asthetic reasons.
        ''' i.e. So the Links from the Roles in the FactType are visually appealling on the Page.
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub SortRoleGroup()

            Dim lrRoleInstance As New FBM.RoleInstance
            Dim liCounter As Integer = 0
            Dim lo_orm_object As New Object

            Try
                If Me.Arity = 1 Then Exit Sub

                If IsSomething(Me.Shape) Then
                    If Me.FactType.RoleGroup.FindAll(Function(x) x.JoinedORMObject Is Nothing).Count > 0 Or (Me.RoleGroup.Count = 0) Then
                        '----------------------------------------------------------------------------------------------
                        'Likely that the user has dragged a multiRole FactType onto the canvas and hasn't
                        '  joined at least one of the Roles to a ModelObject yet.
                        '------------------------------------------------------------------------------------
                    ElseIf Me.DoAllRolesLinkToSameModelObject Then
                        '----------------------------------------------------------------------
                        'Don't sort the RoleGroup of the FactType, because
                        '  all of the Roles link to the same ObjectType and it makes 
                        '  it easier for the user if the sequence of the Roles within
                        '  the RoleGroup remain the same.
                        '  NB Also, the RoleGroup.sort (below) is quasi random
                        '  when all Roles link the same ObjectGroup (as ends up
                        '  sorting list of 0s as difference of x position of same ObjectType.
                        '----------------------------------------------------------------------            
                    Else
                        Me.RoleGroup.Sort(AddressOf CompareRoleXPositions)
                    End If

                    Dim lbIsVisible As Boolean = False

                    For Each lrRoleInstance In Me.RoleGroup
                        liCounter += 1
                        lbIsVisible = lrRoleInstance.Shape.Visible
                        lrRoleInstance.Shape.Detach()
                        lrRoleInstance.Shape.Move((Me.Shape.Bounds.X + 3) + ((liCounter - 1) * 6), Me.Shape.Bounds.Y + 4 + ((Me.FactType.GetHighestConstraintLevel - 1) * 1.6))
                        lrRoleInstance.Shape.AttachTo(Me.Shape, AttachToNode.BottomCenter)
                        lrRoleInstance.Shape.Visible = lbIsVisible
                        lrRoleInstance.SequenceNr = liCounter
                    Next

                    Call Me.ResetAnchorsForRoleGroup()
                    If IsSomething(Me.FactTable.TableShape) And IsSomething(Me.FactTable.FactTypeInstance) Then
                        Call Me.FactTable.ResortFactTable()
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

        Public Sub SortRoleGroupByRoleSequenceNr()

            Call Me.RoleGroup.Sort(AddressOf CompareRoleSequenceNrs)

        End Sub

        Public Overloads Function TypeOfBinaryFactType() As Integer
            'This Function returns one of the following values
            '0- If the FactType has no mandatory roles
            '1- if the FactType only has one manatory role
            '2- if the FactType has two mandatory roles

            Dim lrRoleInstance As FBM.RoleInstance

            TypeOfBinaryFactType = 0

            For Each lrRoleInstance In Me.RoleGroup
                If lrRoleInstance.Mandatory Then
                    TypeOfBinaryFactType += 1
                End If
            Next

        End Function

        Public Function DoAllRolesLinkToSameModelObject() As Boolean

            '------------------------------------------------------------------
            'Returns True if all the Roles in the FactType join/link to the
            '  same ObjectType within the model, 
            '  ELSE returns FALSE
            '------------------------------------------------------------------

            Dim lrRole As FBM.Role
            Dim ls_first_object_type_id As String = ""


            Try

                '-------------------------------------------------------------------------------------------
                'Optimistic TRUE, as using trigger to switch to false if any Role.JoinedObjectTypeId
                '  does not match the first Role.JoinedObjectTypeId (within the RoleGroup of the FactType)
                '-------------------------------------------------------------------------------------------
                DoAllRolesLinkToSameModelObject = True

                'Select Case Me.RoleGroup(0).TypeOfJoin
                '    Case Is = pcenumRoleJoinType.EntityType
                '        ls_first_object_type_id = Me.RoleGroup(0).JoinsEntityType.Id
                '    Case Is = pcenumRoleJoinType.FactType
                '        ls_first_object_type_id = Me.RoleGroup(0).JoinsFactType.Id
                '    Case Is = pcenumRoleJoinType.ValueType
                '        ls_first_object_type_id = Me.RoleGroup(0).JoinsValueType.Id
                'End Select                

                If Me.FactType.RoleGroup.FindAll(Function(x) x.JoinedORMObject Is Nothing).Count > 0 Then
                    Return False
                End If

                ls_first_object_type_id = Me.RoleGroup(0).JoinedORMObject.Id

                For Each lrRole In Me.FactType.RoleGroup
                    If lrRole.JoinedORMObject.Id <> ls_first_object_type_id Then
                        DoAllRolesLinkToSameModelObject = False
                        Exit For
                    End If
                Next

            Catch ex As Exception
                Dim lsMessage As String
                lsMessage = "Error: tFactTypeInstance.DoAllRolesLinkToSameModelObject"
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)

                DoAllRolesLinkToSameModelObject = False
            End Try

        End Function

        Public Shared Function CompareRoleXPositions(ByVal aoA As FBM.RoleInstance, ByVal aoB As FBM.RoleInstance) As Integer

            '------------------------------------------------------
            'Used as a delegate within 'SortRoleGroup'
            '------------------------------------------------------
            Dim loa As New Object
            Dim lob As New Object

            Try
                loa = aoA.JoinedORMObject
                lob = aoB.JoinedORMObject

                '--------------------------------------------------------------------------------------------
                'JoinedORMObject may be nothing when dropping a new Role onto an existing FactType(Instance)
                '  through the GUI.
                '--------------------------------------------------------------------------------------------
                If (loa Is Nothing) And (lob Is Nothing) Then
                    Throw New Exception("Error: Two RoleInstances with JoinedORMObject = Nothing.")
                ElseIf loa Is Nothing Then
                    Return lob.x
                ElseIf lob Is Nothing Then
                    Return loa.x
                Else
                    Return loa.x - lob.x
                End If


            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(ex.Message, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Function

        Public Shared Function CompareArity(ByVal aoA As FBM.FactTypeInstance, ByVal aoB As FBM.FactTypeInstance) As Integer

            Return aoA.Arity + aoB.Arity

        End Function

        Private Shared Function CompareRoleSequenceNrs(ByVal aoA As FBM.RoleInstance, ByVal aoB As FBM.RoleInstance) As Integer

            Return aoA.SequenceNr - aoB.SequenceNr

        End Function

        Public Shadows Sub RemoveInternalUniquenessConstraint(ByRef arInternalUniquenessConstraint As FBM.RoleConstraintInstance)

            Try
                Me.InternalUniquenessConstraint.Remove(arInternalUniquenessConstraint)

                Dim liInd As Integer = 1
                For liInd = 1 To Me.InternalUniquenessConstraint.Count
                    Me.InternalUniquenessConstraint(liInd - 1).LevelNr = liInd
                    Call Me.InternalUniquenessConstraint(liInd - 1).RefreshShape()
                Next

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub


        Public Sub ResetAnchorsForRoleGroup()

            '--------------------------------------------------------
            'Set the AnchorPatterns for the RoleGroup
            '--------------------------------------------------------
            Dim liInd As Integer
            Dim apat1 As AnchorPattern = Nothing

            Try
                For liInd = 1 To Me.Arity
                    If liInd = 1 Then
                        apat1 = New AnchorPattern(New AnchorPoint() { _
                                New AnchorPoint(50, 0, True, True), _
                                New AnchorPoint(50, 100, True, True), _
                                New AnchorPoint(0, 50, True, True)})
                    ElseIf (liInd > 1) And (liInd < Me.Arity) Then
                        apat1 = New AnchorPattern(New AnchorPoint() { _
                                New AnchorPoint(50, 0, True, True), _
                                New AnchorPoint(50, 100, True, True)})
                    ElseIf liInd = Me.Arity Then

                        apat1 = New AnchorPattern(New AnchorPoint() { _
                                New AnchorPoint(50, 0, True, True), _
                                New AnchorPoint(100, 50, True, True), _
                                New AnchorPoint(50, 100, True, True)})
                    End If

                    '============================================================================================
                    '20150612-VM-The following code is a prototype for links at the intersection of Roles
                    '  Currently not implemented. e.g. For RingConstraints on Binary FactTypes
                    'If liInd = 1 Then
                    '    If Me.Arity = 2 Then
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
                    'ElseIf (liInd > 1) And (liInd < Me.Arity) Then
                    '    apat1 = New AnchorPattern(New AnchorPoint() { _
                    '            New AnchorPoint(50, 0, True, True), _
                    '            New AnchorPoint(50, 100, True, True)})
                    'ElseIf liInd = Me.Arity Then
                    '    If Me.Arity = 2 Then
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

                    Dim lrRoleInstance As FBM.RoleInstance = Me.RoleGroup(liInd - 1)

                    If IsSomething(lrRoleInstance.Shape) Then
                        lrRoleInstance.Shape.AnchorPattern = apat1
                    End If

                    If IsSomething(lrRoleInstance.Link) Then
                        '-----------------------------------------------------------------------
                        'Because this method may be called for Roles that do not yet have links
                        '-----------------------------------------------------------------------
                        'Reattach the Link to the AnchorPoints of the Role
                        '---------------------------------------------------
                        Call lrRoleInstance.Link.ReassignAnchorPoints()
                    End If
                Next liInd

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try


        End Sub

        Public Sub ResetIsDisplayedAndAssociated()

            Me.IsDisplayedAssociated = False

        End Sub

        Sub Resize()

            Try
                'CodeSafe
                If Me.Shape Is Nothing Then Exit Sub

                '--------------------------------------------------------------------------------------
                'Resize the FactType.Shape and reset the position of the RoleGroup within the factType
                '--------------------------------------------------------------------------------------
                Dim lo_rectangle As New Rectangle(Me.Shape.Bounds.X, Me.Shape.Bounds.Y, ((Me.RoleGroup(0).Shape.Bounds.Width * Me.Arity) + 6), 15)
                Me.Shape.SetRect(lo_rectangle, False)

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try


        End Sub

        Public Sub DisplayAndAssociate(Optional ByVal abDisplayFactTable As Boolean = False, _
                                       Optional ByVal abDisplayFactTypeName As Boolean = False, _
                                       Optional ByVal abDisplaySubtypeFactType As Boolean = False)

            Dim loFactTypeNode As ShapeNode
            Dim loFactTypeName As ShapeNode

            Try

                If Me.IsDisplayedAssociated Or (Me.IsSubtypeRelationshipFactType And Not abDisplaySubtypeFactType) Then
                    '----------------------------------------------------------------
                    'Is already Displayed and Associated on the Page.
                    '  Used when recursively loading FactTypeInstances onto a Page.
                    'OR
                    'Is a SubtypeConstraintFactType, which are hidden.
                    '----------------------------------------------------------------
                Else
                    loFactTypeNode = Me.Page.Diagram.Factory.CreateShapeNode(Me.X, Me.Y, 12, 12 + ((Me.GetHighestConstraintLevel - 1) * 1.6), Shapes.RoundRect)

                    loFactTypeNode.HandlesStyle = HandlesStyle.InvisibleMove
                    loFactTypeNode.Pen.Color = Color.LightGray
                    loFactTypeNode.ToolTip = "Fact Type"
                    loFactTypeNode.AllowOutgoingLinks = False
                    If Me.isPreferredReferenceMode Then
                        loFactTypeNode.Visible = False
                    Else
                        loFactTypeNode.Visible = True 'VM just for initial testing. Set this back to false when Roles are implemented.
                    End If
                    If Me.IsObjectified Then
                        loFactTypeNode.ShadowOffsetX = 1
                        loFactTypeNode.ShadowOffsetY = 1
                        loFactTypeNode.ShadowColor = Color.LightGray
                    End If
                    loFactTypeNode.Pen.Width = 0.5

                    loFactTypeNode.Tag = New FBM.FactTypeInstance
                    loFactTypeNode.Tag = Me
                    Me.Shape = loFactTypeNode

                    '------------------------------------------------------------------------
                    'Setup a ShapeGroup for the FactType (to which to attach Role ShapeNodes)
                    '------------------------------------------------------------------------
                    Me.Page.Diagram.Groups.Add(Me.Page.Diagram.Factory.CreateGroup(Me.Shape))

                    '----------------------------------------
                    'Setup the FactTypeName
                    '----------------------------------------                    
                    Dim StringSize As New SizeF
                    Dim G As Graphics
                    Dim lsFactTypeName As String = Chr(34) & Me.Name & Chr(34)
                    G = Me.Page.Form.CreateGraphics
                    StringSize = Me.Page.Diagram.MeasureString(Trim(lsFactTypeName), Me.Page.Diagram.Font, 1000, System.Drawing.StringFormat.GenericDefault)
                    StringSize.Height += 5


                    loFactTypeName = Me.Page.Diagram.Factory.CreateShapeNode(Me.X, Me.Y - 7, StringSize.Width, StringSize.Height) 'Me.FactTypeName.X, Me.FactTypeName.Y, StringSize.Width, StringSize.Height)
                    loFactTypeName.Shape = MindFusion.Diagramming.Shapes.Rectangle
                    loFactTypeName.HandlesStyle = HandlesStyle.InvisibleMove
                    loFactTypeName.Text = lsFactTypeName
                    loFactTypeName.TextColor = Color.Blue
                    loFactTypeName.Transparent = True
                    loFactTypeName.ZTop()

                    Me.FactTypeName.Model = Me.Model
                    Me.FactTypeName.FactTypeInstance = Me
                    Me.FactTypeNameShape = loFactTypeName
                    Me.FactTypeName.Shape = loFactTypeName
                    Me.Page.Diagram.Nodes.Add(Me.FactTypeName.Shape)
                    loFactTypeName.Tag = Me.FactTypeName


                    '---------------------------------------------------------------------------
                    'Attach the FactTypeName ShapeNode to the FactTypeInstance ShapeNode
                    '---------------------------------------------------------------------------
                    loFactTypeName.AttachTo(loFactTypeNode, AttachToNode.TopLeft)

                    '------------------------------------------------------------------------------
                    'Hide the FactTypeName if the FactType is not Objectified.
                    '  NB This needs to happen after the attachement of the ShapeNode to the 
                    '  FactType.ShapeNodeGroup because the act of 'attaching' seems to 
                    '  reset the Visible attribute back to True (for the ShapeNode being attached)
                    '------------------------------------------------------------------------------
                    If (Me.FactType.IsObjectified Or abDisplayFactTypeName) And Not Me.isPreferredReferenceMode Then
                        loFactTypeName.Visible = True
                    Else
                        loFactTypeName.Visible = False
                    End If

                    loFactTypeName.Visible = Me.ShowFactTypeName


                    '==========================================================================================================
                    'FactTypeDerivationText
                    If Me.FactType.IsDerived Then
                        Dim loFactTypeDerivationTextShape As ShapeNode
                        Dim lsDerivationText As String = ""


                        If Me.FactType.IsManyTo1BinaryFactType Then
                            '-----------------------------------------------------------------------------------------------
                            'That's good, because needs to be at least that for Derived Fact Type.
                            Dim lrRole As FBM.Role
                            lrRole = Me.FactType.GetFirstRoleWithInternalUniquenessConstraint
                            lsDerivationText = "* <b>For each</b> " & lrRole.JoinedORMObject.Name
                            lsDerivationText &= vbCrLf & Me.DerivationText
                        Else
                            lsDerivationText = "* " & Me.DerivationText
                        End If

                        StringSize = Me.Page.Diagram.MeasureString(Trim(lsDerivationText), Me.Page.Diagram.Font, 1000, System.Drawing.StringFormat.GenericDefault)
                        StringSize.Height += 2

                        If StringSize.Width > 70 Then
                            StringSize = New SizeF(70, (StringSize.Height + 2) * lsDerivationText.Length / 100)
                        End If

                        loFactTypeDerivationTextShape = Me.Page.Diagram.Factory.CreateShapeNode(Me.X, Me.Y + Me.Shape.Bounds.Height + 5, StringSize.Width, StringSize.Height)
                        loFactTypeDerivationTextShape.Shape = MindFusion.Diagramming.Shapes.Rectangle
                        loFactTypeDerivationTextShape.HandlesStyle = HandlesStyle.MoveOnly
                        loFactTypeDerivationTextShape.EnableStyledText = True
                        loFactTypeDerivationTextShape.Locked = False
                        loFactTypeDerivationTextShape.TextFormat.Alignment = StringAlignment.Near
                        loFactTypeDerivationTextShape.Text = lsDerivationText
                        Call loFactTypeDerivationTextShape.ResizeToFitText(FitSize.KeepWidth)
                        loFactTypeDerivationTextShape.TextColor = Color.Black
                        loFactTypeDerivationTextShape.Transparent = True
                        loFactTypeDerivationTextShape.AllowIncomingLinks = False
                        loFactTypeDerivationTextShape.AllowOutgoingLinks = False
                        loFactTypeDerivationTextShape.ZTop()

                        If Me.FactTypeDerivationText Is Nothing Then
                            Me.FactTypeDerivationText = New FBM.FactTypeDerivationText(Me.Model, Me.Page, Me)
                        End If

                        Me.FactTypeDerivationText.Shape = loFactTypeDerivationTextShape
                        loFactTypeDerivationTextShape.Tag = Me.FactTypeDerivationText

                        If Me.FactTypeDerivationText.X = 0 Then Me.FactTypeDerivationText.X = Me.X
                        If Me.FactTypeDerivationText.Y = 0 Then Me.FactTypeDerivationText.Y = Me.Y + Me.Shape.Bounds.Height + 5

                        Me.FactTypeDerivationText.Shape.Move(Me.FactTypeDerivationText.X,
                                                             Me.FactTypeDerivationText.Y)

                        Me.Page.Diagram.Nodes.Add(Me.FactTypeDerivationText.Shape)

                        Me.FactTypeDerivationText.Shape.Visible = Me.IsDerived
                        Call Me.FactTypeDerivationText.Shape.ZBottom()
                    End If

                    '----------------------------------------------------------------
                    'Display and associatte the FactTable for the FactTypeInstance
                    '----------------------------------------------------------------                
                    Me.FactTable.Model = Me.Model
                    Me.FactTable.Page = Me.Page
                    Call Me.FactTable.DisplayAndAssociate(Me, abDisplayFactTable)


                    '--------------------------------------------------------------------------
                    'Sort the FactTypeInstance.RoleGroup by SequenceNr before displaying the Roles.
                    '  NB Roles have SequenceNrs so that FactTypes with multiple Roles
                    '  referencing the same ModelObject are maintained in a consistent
                    '  display sequence every time they are displayed on a Page.
                    '  Logically it makes no diference in which order Roles are displayed
                    '  but this may become confusing for the User if RoleGroups are displayed
                    '  differently on successive refreshes of a Page.
                    '--------------------------------------------------------------------------
                    Call Me.SortRoleGroupByRoleSequenceNr()

                    Dim lrRoleInstance As FBM.RoleInstance

                    '-----------------------------------------------------------------
                    'Display and Associate the RoleInstances of the FactTypeInstance
                    '-----------------------------------------------------------------
                    For Each lrRoleInstance In Me.RoleGroup
                        '---------------------------------------------------------------------
                        'Associate the Role/RoleInstance to the FactType/FactTypeInstance
                        '---------------------------------------------------------------------
                        Call lrRoleInstance.DisplayAndAssociate(Me)
                    Next

                    '------------------------------------------------------------------------------------------
                    'Sort the RoleInstances so that the Roles within the FactType/RoleGroup are in the correct
                    '  visual order (in line with the objects to which they join)
                    '------------------------------------------------------------------------------------------
                    Me.SortRoleGroup()

                    For Each lrRoleInstance In Me.RoleGroup
                        '-----------------------------------------------------------
                        'Attach the Role.RoleName ShapeNode to the Role ShapeGroup,                                    
                        '-----------------------------------------------------------
                        lrRoleInstance.RoleName.Shape.AttachTo(lrRoleInstance.Shape, AttachToNode.TopLeft)

                        If Me.isPreferredReferenceMode Then
                            If lrRoleInstance.JoinedORMObject.ConceptType = pcenumConceptType.ValueType Then
                                Dim lrValueTypeInstance As FBM.ValueTypeInstance = lrRoleInstance.JoinedORMObject
                                If lrValueTypeInstance.Shape IsNot Nothing Then
                                    If lrValueTypeInstance.Shape.OutgoingLinks.Count = 0 Then
                                        lrValueTypeInstance.Shape.Visible = False
                                    End If
                                End If
                            End If
                        End If

                    Next

                    '---------------------------------
                    'Find a suitable FactTypeReading
                    '---------------------------------
                    Dim lrFactTypeReading As New FBM.FactTypeReading
                    Dim lrFactTypeReadingInstance As New FBM.FactTypeReadingInstance

                    'Does DisplayingAndAssociating when found.
                    lrFactTypeReading = Me.FindSuitableFactTypeReading

                    '------------------------------------------
                    'Set the AnchorPatterns for the RoleGroup
                    '------------------------------------------
                    Call Me.ResetAnchorsForRoleGroup()

                    Me.IsDisplayedAssociated = True

                End If 'Displayed/Associated

            Catch ex As Exception

                Dim lsMessage As String
                lsMessage = "Error: tFactTypeInstance.DisplayAndAssociate"
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)

            End Try

        End Sub

        Private Sub MakeVisible()

            Try
                If IsSomething(Me.Shape) Then
                    Me.Shape.Visible = True                    

                    Me.FactTypeNameShape.Visible = True

                    If IsSomething(Me.FactTypeReadingShape.shape) Then
                        Me.FactTypeReadingShape.shape.Visible = True
                    End If
                    Dim lrRoleInstance As FBM.RoleInstance
                    Dim lrRoleConstraintInstance As FBM.RoleConstraintInstance
                    Dim lrRoleConstraintRoleInstance As FBM.RoleConstraintRoleInstance
                    For Each lrRoleInstance In Me.RoleGroup
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
                            lrValueTypeInstance.Shape.Visible = True
                        End If
                    Next
                    For Each lrRoleConstraintInstance In Me.InternalUniquenessConstraint
                        For Each lrRoleConstraintRoleInstance In lrRoleConstraintInstance.RoleConstraintRole
                            lrRoleConstraintRoleInstance.Shape.Visible = True
                        Next
                    Next
                End If

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Overloads Sub RefreshShape(Optional ByVal aoChangedPropertyItem As PropertyValueChangedEventArgs = Nothing,
                                Optional ByVal asSelectedGridItemLabel As String = "")
            '------------------------------------------------
            'Called by the Properties form when the value
            '  of an attribute is changed.
            '------------------------------------------------
            Dim lsMessage As String

            Try
                If IsSomething(aoChangedPropertyItem) Then
                    Select Case aoChangedPropertyItem.ChangedItem.PropertyDescriptor.Name
                        Case Is = "Name"
                            If Me.FactType.Name = Me.Name Then
                                '------------------------------------------------------------
                                'Nothing to do. Name of the FactType has not been changed.
                                '------------------------------------------------------------
                            Else
                                Dim lrEntityTypeDictionaryEntry As New FBM.DictionaryEntry(Me.Model, Me.Name, pcenumConceptType.EntityType)
                                Dim lrValueTypeDictionaryEntry As New FBM.DictionaryEntry(Me.Model, Me.Name, pcenumConceptType.ValueType)
                                Dim lrFactTypeDictionaryEntry As New FBM.DictionaryEntry(Me.Model, Me.Name, pcenumConceptType.FactType)
                                Dim lrRoleConstraintDictionaryEntry As New FBM.DictionaryEntry(Me.Model, Me.Name, pcenumConceptType.RoleConstraint)

                                If Me.Model.ModelDictionary.Exists(AddressOf lrFactTypeDictionaryEntry.EqualsBySymbolConceptType) Then
                                    MsgBox("A Fact Type with the name, '" & lrFactTypeDictionaryEntry.Symbol & "', already exists in the Model, '" & Me.Model.Name & "'.", MsgBoxStyle.Exclamation, "Model Object Conflict")
                                    Me.Name = Me.FactType.Name
                                ElseIf Me.Model.ModelDictionary.Exists(AddressOf lrValueTypeDictionaryEntry.EqualsBySymbolConceptType) Then
                                    MsgBox("The name, '" & lrEntityTypeDictionaryEntry.Symbol & "', conflicts with a Value Type of the same name in the Model, '" & Me.Model.Name & "'.", MsgBoxStyle.Exclamation, "Model Object Conflict")
                                    Me.Name = Me.FactType.Name
                                ElseIf Me.Model.ModelDictionary.Exists(AddressOf lrEntityTypeDictionaryEntry.EqualsBySymbolConceptType) Then
                                    MsgBox("The name, '" & lrEntityTypeDictionaryEntry.Symbol & "', conflicts with an Entity Type of the same name in the Model, '" & Me.Model.Name & "'.", MsgBoxStyle.Exclamation, "Model Object Conflict")
                                    Me.Name = Me.FactType.Name
                                ElseIf Me.Model.ModelDictionary.Exists(AddressOf lrRoleConstraintDictionaryEntry.EqualsBySymbolConceptType) Then
                                    MsgBox("The name, '" & lrEntityTypeDictionaryEntry.Symbol & "', conflicts with a Role Constraint of the same name in the Model, '" & Me.Model.Name & "'.", MsgBoxStyle.Exclamation, "Model Object Conflict")
                                    Me.Name = Me.FactType.Name
                                ElseIf lrEntityTypeDictionaryEntry.Symbol.Contains("'") Then
                                    MsgBox("The name of an Entity Type cannot contain a ' (single quote).")
                                    Me.Name = Me.FactType.Name
                                ElseIf Not FBM.IsAcceptableObjectTypeName(lrEntityTypeDictionaryEntry.Symbol) Then
                                    MsgBox("The name of a Fact Type can only contain the characters [a-zA-Z0-9].")
                                    Me.Name = Me.FactType.Name
                                Else
                                    Me.FactType.setName(Me.Name)
                                    Me.Id = Me.Name
                                    Me.Symbol = Me.Name
                                End If
                            End If
                        Case Is = "DBName"
                            Call Me.FactType.SetDBName(Me.DBName)
                        Case Is = "IsObjectified"
                            If Not Me.IsObjectified And Me.FactType.IsObjectified And Me.FactType.hasAssociatedFactTypes Then
                                lsMessage = "Remove all links to Fact Types linked to this Fact Type before removing the objectification of this Fact Type."
                                lsMessage &= vbCrLf & vbCrLf
                                lsMessage &= "Hint: See the list of associated Fact Types in the ORM Verbalisation toolbox to choose which Fact Types to remove or modify in the model before removing the objectification of this Fact Type."
                                MsgBox(lsMessage)
                                Me.IsObjectified = Me.FactType.IsObjectified
                            Else
                                If Me.IsObjectified Then
                                    Call Me.FactType.Objectify()
                                Else
                                    Call Me.FactType.RemoveObjectification(True)
                                End If

                            End If

                        Case Is = "ShowFactTypeName"
                            Call Me.FactType.SetShowFactTypeName(Me.ShowFactTypeName)
                        Case Is = "IsDerived"
                            If Me.IsDerived Then
                                Call Me.SetPropertyAttributes(Me, "IsStored", True)
                                Call Me.SetPropertyAttributes(Me, "DerivationText", True)
                            Else
                                Call Me.SetPropertyAttributes(Me, "IsStored", False)
                                Call Me.SetPropertyAttributes(Me, "DerivationText", False)
                            End If
                            Call Me.FactType.SetIsDerived(Me.IsDerived, True)
                            '=================================================================================================
                        Case Is = "IsIndependent"
                            Call Me.FactType.SetIsIndependent(Me.IsIndependent, True)
                        Case Is = "IsStored"
                            Call Me.FactType.SetIsStored(Me.IsStored, True)
                        Case Is = "DerivationText"
                            Call Me.FactType.SetDerivationText(Me.DerivationText, True)
                        Case Is = "ShortDescription"
                            Call Me.FactType.SetShortDescription(Me.ShortDescription)
                            Me.Model.ModelDictionary.Find(Function(x) LCase(x.Symbol) = LCase(Me.Id)).ShortDescription = Me.ShortDescription
                        Case Is = "LongDescription"
                            Call Me.FactType.SetLongDescription(Me.LongDescription)
                            Me.Model.ModelDictionary.Find(Function(x) LCase(x.Symbol) = LCase(Me.Id)).LongDescription = Me.LongDescription
                        Case Is = "IsSubtypeStateControlling"
                            Call Me.FactType.SetIsSubtypeStateControlling(Me.IsSubtypeStateControlling, True)
                    End Select

                    Call Me.EnableSaveButton()
                End If

                '--------------------------------------------------
                'Refresh the visible FactTypeInstance (on the Page)
                '--------------------------------------------------
                Me.FactTypeNameShape.Text = Chr(34) & Me.Name & Chr(34)
                Me.FactTypeNameShape.Visible = Me.ShowFactTypeName

                If Me.Shape IsNot Nothing Then

                    If Me.ShowFactTypeName Then
                        If Math.Abs(Me.FactTypeNameShape.Bounds.X - Me.Shape.Bounds.X) > 20 Or Math.Abs(Me.FactTypeNameShape.Bounds.Y - Me.Shape.Bounds.Y) > 20 Then
                            Me.FactTypeNameShape.Move(Me.Shape.Bounds.X - 5, Me.Shape.Bounds.Y - 10)
                        End If
                    End If

                    If Me.IsObjectified Then
                        Me.Shape.Visible = True
                        Me.Shape.Pen.Color = Color.Black
                    Else
                        Me.Shape.Pen.Color = Color.White
                    End If

                    If Me.FactTypeReadingShape IsNot Nothing Then
                        Call Me.FactTypeReadingShape.RefreshShape()
                    End If

                End If

                If Me.Page.Diagram IsNot Nothing Then
                    Me.Page.Diagram.Invalidate()
                    Me.Page.DiagramView.Invalidate()
                    Call Me.Page.Form.ResetPropertiesGridToolbox(Me)
                End If

            Catch ex As Exception
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub


        'Public Shadows Sub SetName(ByVal asNewName As String)

        '    Try
        '        Me.FactTypeName.Name = asNewName
        '        '-----------------------------------------------------------
        '        'The surrogate key for the FactType is about
        '        '  to change (to match the name of the FactType)
        '        '  so update the ModelDictionary entry for the 
        '        '  Concept/Symbol (the nominalistic idenity of the FactType
        '        '-----------------------------------------------------------
        '        Me.Symbol = asNewName

        '        If StrComp(Me.Id, asNewName) <> 0 Then
        '            ''------------------------------------------------------------------------------------------
        '            ''Update the Model(database) immediately. There is no choice. The reason that you do this,
        '            ''  is because the (in-memory) key is changing, so if the database is not updated to 
        '            ''  reflect the new key, it is not possible to Update an existing FactType.
        '            ''------------------------------------------------------------------------------------------
        '            Call TableFactTypeInstance.ModifyKey(Me, asNewName)

        '            Dim lrFactTable As New FBM.FactTable(Me.Page, Me)
        '            Call TableFactTableInstance.ModifyKey(lrFactTable, asNewName)

        '            Dim lrFactTypeName As New FBM.FactTypeName(Me.Model, Me.Page, Me, asNewName)
        '            lrFactTypeName.FactTypeInstance = Me
        '            Call TableFactTypeName.ModifyKey(lrFactTypeName, asNewName)

        '            Me.Id = asNewName
        '            Me.Page.IsDirty = True
        '        End If

        '    Catch ex As Exception
        '        Dim lsMessage As String
        '        Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

        '        lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
        '        lsMessage &= vbCrLf & vbCrLf & ex.Message
        '        prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        '    End Try

        'End Sub

        Private Sub _FactType_DerivationTextChanged(asDerivationText As String) Handles _FactType.DerivationTextChanged

            Me.DerivationText = asDerivationText

            If Me.FactTypeDerivationText Is Nothing Then
                Me.FactTypeDerivationText = New FBM.FactTypeDerivationText(Me.Model, Me.Page, Me)
                Me.FactTypeDerivationText.X = Me.X
                Me.FactTypeDerivationText.Y = Me.Y + Me.Shape.Bounds.Height + 5

                If Me.Shape IsNot Nothing And Me.Page.Diagram IsNot Nothing Then
                    Call Me.FactTypeDerivationText.DisplayAndAssociate()
                End If
            Else
                Me.FactTypeDerivationText.Shape.Text = asDerivationText
            End If

        End Sub

        Private Sub _FactType_FactRemoved(ByRef arFact As Fact) Handles _FactType.FactRemoved

            'Dim lrFactInstance As New FBM.FactInstance(arFact.Id, Me)
            'lrFactInstance = arFact.CloneInstance(Me.Page)

            Call Me.RemoveFactById(arFact)

            '-----------------------------------------------------------
            'Update the FactTable within the FactTypeInstance on the
            '  current page
            '-----------------------------------------------------------
            If Me.Page.Loaded And Me.Page.Language = pcenumLanguage.ORMModel Then
                If IsSomething(Me.FactTable.TableShape) And IsSomething(Me.FactTable.FactTypeInstance) Then
                    Call Me.FactTable.ResortFactTable()
                End If
            End If

        End Sub

        Private Sub _FactType_FactTypeReadingAdded(ByRef arFactTypeReading As FactTypeReading) Handles _FactType.FactTypeReadingAdded

            Try


                Call Me.FindSuitableFactTypeReading()

                Dim lrModelError As New FBM.ModelError(115, Me.FactType)
                lrModelError = Me.FactType.ModelError.Find(Function(x) x.ErrorId = lrModelError.ErrorId And x.ModelObject.Id = Me.FactType.Id)
                Me.FactType.ModelError.Remove(lrModelError)
                Call Me.Model.RemoveModelError(lrModelError)

                Call Me.SetAppropriateColour()

                '-------------------------------------------------------------------------------------------------------
                'VM-20180403-Old code, can remove
                'Dim lrFactTypeReadingInstance As FBM.FactTypeReadingInstance = arFactTypeReading.CloneInstance(Me.Page)
                'Dim lrSuitableFactTypeReadingInstance As FBM.FactTypeReadingInstance

                ''---------------------------------------------------------------------------
                ''The reading may not be suitable for the current RoleGroup sequence/layout
                ''  so check to see if it is. If it is not, then the reading should not
                ''  be displayed on the Page.
                ''---------------------------------------------------------------------------
                'Dim larRoles As New List(Of FBM.Role)
                'For Each lrRole In Me.FactType.RoleGroup
                '    larRoles.Add(lrRole)
                'Next
                
                'Dim lrSuitableFactTypeReading As FBM.FactTypeReading
                'lrSuitableFactTypeReading = Me.FactType.FindSuitableFactTypeReadingByRoles(larRoles)

                'If Me.FactTypeReadingShape Is Nothing Then
                '    Me.FactTypeReadingShape = New FBM.FactTypeReadingInstance(Me, Nothing)
                'End If
                'If Me.FactTypeReadingShape.Page Is Nothing Then
                '    Me.FactTypeReadingShape.Page = Me.Page
                'End If

                'If Me.FactTypeReadingShape.Page.Form IsNot Nothing Then
                '    If IsSomething(lrSuitableFactTypeReading) Then
                '        lrSuitableFactTypeReadingInstance = lrSuitableFactTypeReading.CloneInstance(Me.Page)
                '        If lrSuitableFactTypeReadingInstance.Equals(lrFactTypeReadingInstance) Then
                '            If IsSomething(Me.FactTypeReadingShape.Shape) Then
                '                lrFactTypeReadingInstance.Shape = Me.FactTypeReadingShape.Shape
                '                Me.FactTypeReadingShape = lrFactTypeReadingInstance
                '                Me.FactTypeReadingShape.RefreshShape()
                '            Else
                '                Call lrFactTypeReadingInstance.DisplayAndAssociate()
                '                Me.FactTypeReadingShape = lrFactTypeReadingInstance
                '            End If
                '        Else
                '            If IsSomething(Me.FactTypeReadingShape.Shape) Then
                '                Me.FactTypeReadingShape.RefreshShape()
                '            End If
                '        End If
                '    Else
                '        If IsSomething(Me.FactTypeReadingShape.Shape) Then
                '            Me.FactTypeReadingShape.Shape.Text = ""
                '        End If
                '    End If
                'End If

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Private Sub _FactType_FactTypeReadingModified(ByRef arFactTypeReading As FactTypeReading) Handles _FactType.FactTypeReadingModified

            Try
                '------------------------------------------------------------------------------------------------
                'Replace the existing FactTypeReadingShape if it matches the FactTypeReading that was modified.
                '------------------------------------------------------------------------------------------------
                If Me.FactTypeReadingShape.Equals(arFactTypeReading) Then
                    Me.Page.Diagram.Nodes.Remove(Me.FactTypeReadingShape.shape)
                    Me.FactTypeReadingShape = arFactTypeReading.CloneInstance(Me.Page)
                    Me.FactTypeReadingShape.DisplayAndAssociate()
                End If

                Me.RefreshShape()

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Private Sub _FactType_FactTypeReadingRemoved(ByRef arFactTypeReading As FactTypeReading) Handles _FactType.FactTypeReadingRemoved

            Dim lrFactTypeReadingInstance As New FBM.FactTypeReadingInstance

            Try
                Call Me.FindSuitableFactTypeReading()

                Call Me.SetAppropriateColour()

                'VM-20180328-Remove this below if all seems fine. FactTypeInstances don't store FactTypeReadings
                'lrFactTypeReadingInstance = arFactTypeReading.CloneInstance(Me.Page)

                'lrFactTypeReadingInstance = Me.FactTypeReading.Find(AddressOf lrFactTypeReadingInstance.Equals)

                'If IsSomething(lrFactTypeReadingInstance) Then
                '    If Me.FactTypeReadingShape.Shape.Tag Is lrFactTypeReadingInstance Then
                '        '------------------------------------------------------------------------------------------------------------------------------------
                '        'Remove the text from the FactTypeReadingInstance/FactTypeReadingShape
                '        '  Don't remove the actual object (FactTypeReadingShape) because it is a static object
                '        '  that gets set to the most appropriate FactTypeReading/Instance when either:
                '        '    a) The FactTypeInstance is Displayed/Associated
                '        '    b) The user moves ModelElements on a Page which changes the Role order of a FactType, changing the appropriate FactTypeReading
                '        '    c) A new FactTypeReading is created using the FactTypeReading editor.
                '        '
                '        'FactTypeReadingInstances are not saved to the database and are ostensibly part of the FactType/Instance.
                '        'In this manner, only the most appropriate FactTypeReading need be displayed to the user at any one time,
                '        ' and the FactTypeReadings need only be managed at the FactType/Model level (not that Page/Instance level).
                '        '------------------------------------------------------------------------------------------------------------------------------------
                '        Me.FactTypeReadingShape.Shape.Text = ""
                '    End If
                'End If

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub


        Private Sub _FactType_IsDerivedChanged(abIsDerived As Boolean) Handles _FactType.IsDerivedChanged

            Me.IsDerived = abIsDerived

        End Sub

        Private Sub _FactType_IsIndependentChanged(abNewIsIndependent As Boolean) Handles _FactType.IsIndependentChanged

            Me.IsIndependent = abNewIsIndependent

        End Sub

        Private Sub _FactType_IsObjectifiedChanged(ByVal abNewIsObjectified As Boolean) Handles _FactType.IsObjectifiedChanged

            Me.IsObjectified = abNewIsObjectified
            Call Me.RefreshShape()

        End Sub

        Private Sub _FactType_IsStoredChanged(abIsStored As Boolean) Handles _FactType.IsStoredChanged

            Me.IsStored = abIsStored

        End Sub

        Private Sub _FactType_IUConstraintAdded(ByRef arFactType As FactType, ByRef arRoleConstraint As FBM.RoleConstraint) Handles _FactType.IUConstraintAdded

            Try
                '----------------------------------------------------------
                'Must add the new RoleConstraint to this FactTypeInstance
                '----------------------------------------------------------
                Dim lrRoleConstraintInstance As New FBM.RoleConstraintInstance

                'CodeSafe
                'If the FactType of the IUC is not on the Page, do not add to Page.
                Dim lbAddToPage As Boolean = Me.Page.FactTypeInstance.Contains(Me)

                lrRoleConstraintInstance = arRoleConstraint.CloneUniquenessConstraintInstance(Me.Page, lbAddToPage)

                Me.InternalUniquenessConstraint.Add(lrRoleConstraintInstance)

                If IsSomething(Me.Shape) Then
                    lrRoleConstraintInstance.DisplayAndAssociate()
                    Call Me.AdjustBorderHeight()
                End If

                Dim lrModelError As FBM.ModelError = Me.FactType.ModelError.Find(Function(x) x.ErrorId = 106)
                Call Me.FactType.ModelError.Remove(lrModelError)
                Call Me.Model.RemoveModelError(lrModelError)

                Call Me.SetAppropriateColour()

                Call Me.Page.MakeDirty()



            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Private Sub _FactType_IUConstraintRemoved(ByRef arFactType As FactType, ByVal arRoleConstraint As FBM.RoleConstraint) Handles _FactType.IUConstraintRemoved

            Try

                If arFactType.Id = Me.Id Then
                    '----------------------------------------------------------
                    'Must remove the RoleConstraint from this FactTypeInstance
                    '----------------------------------------------------------
                    Dim lrRoleConstraintInstance As New FBM.RoleConstraintInstance
                    Dim lrRoleConstraintRoleInstance As FBM.RoleConstraintRoleInstance

                    lrRoleConstraintInstance = Me.InternalUniquenessConstraint.Find(Function(x) x.Id = arRoleConstraint.Id)

                    '----------------------------------------------------------------------------------------------------
                    'Code Safe: Check to see if the RoleConstraintInstance actually exists against the FactTypeInstance.
                    '----------------------------------------------------------------------------------------------------
                    If lrRoleConstraintInstance IsNot Nothing Then
                        For Each lrRoleConstraintRoleInstance In lrRoleConstraintInstance.RoleConstraintRole
                            If Me.Page.Diagram IsNot Nothing Then
                                Me.Page.Diagram.Nodes.Remove(lrRoleConstraintRoleInstance.BackingShape)
                                Me.Page.Diagram.Nodes.Remove(lrRoleConstraintRoleInstance.Shape)
                            End If
                        Next
                    End If


                    Me.Page.RoleConstraintInstance.Remove(lrRoleConstraintInstance)
                    Me.InternalUniquenessConstraint.Remove(lrRoleConstraintInstance)

                    Dim liInd As Integer = 1
                    For liInd = 1 To Me.InternalUniquenessConstraint.Count
                        Me.InternalUniquenessConstraint(liInd - 1).LevelNr = liInd
                        Call Me.InternalUniquenessConstraint(liInd - 1).RefreshShape()
                    Next

                    Call Me.AdjustBorderHeight()

                End If

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try


        End Sub

        Private Sub _FactType_LongDescriptionChanged(asLongDescription As String) Handles _FactType.LongDescriptionChanged

            Me.LongDescription = asLongDescription

        End Sub

        Private Sub _FactType_ModelErrorAdded(ByRef arModelError As ModelError) Handles _FactType.ModelErrorAdded

            Call Me.SetAppropriateColour()

        End Sub

        Private Sub _FactType_NameChanged(ByVal asNewName As String) Handles _FactType.NameChanged

            Dim lrConceptInstance As New FBM.ConceptInstance(Me.Model, Me.Page, Me.FactType.Id, pcenumConceptType.FactType)
            Call TableConceptInstance.UpdateConceptInstanceByModelPageConceptTypeRoleId(lrConceptInstance, Me.Id)

            lrConceptInstance = New FBM.ConceptInstance(Me.Model, Me.Page, Me.FactType.Id, pcenumConceptType.FactTable)
            Call TableConceptInstance.UpdateConceptInstanceByModelPageConceptTypeRoleId(lrConceptInstance, Me.Id)

            lrConceptInstance = New FBM.ConceptInstance(Me.Model, Me.Page, Me.FactType.Id, pcenumConceptType.FactTypeName)
            Call TableConceptInstance.UpdateConceptInstanceByModelPageConceptTypeRoleId(lrConceptInstance, Me.Id)

            Me.Id = Me.FactType.Id
            Me.Name = Me.FactType.Id
            Me.Symbol = Me.FactType.Id

            Me.Page.MakeDirty()

        End Sub

        Private Sub _FactType_ObjectificationRemoved() Handles _FactType.ObjectificationRemoved

            Try
                Me.IsObjectified = False

                If Me.ObjectifyingEntityType IsNot Nothing Then
                    Me.ObjectifyingEntityType.IsObjectifyingEntityType = False
                End If

                If IsSomething(Me.Shape) Then
                    Me.Shape.Pen.Color = Color.White
                End If

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try


        End Sub

        Private Sub _FactType_Objectified() Handles _FactType.Objectified

            Me.IsObjectified = True

            Dim lrEntityTypeInstance As New FBM.EntityTypeInstance

            If Me.Page.EntityTypeInstance.Exists(Function(x) x.Id = Me.FactType.ObjectifyingEntityType.Id) Then
                '-------------------------------------------------------------------------------------------
                'The Objectifying EntityType is already on the Page.
                '-----------------------------------------------------
            Else
                lrEntityTypeInstance = Me.FactType.ObjectifyingEntityType.CloneInstance(Me.Page, True)

                Me.ObjectifyingEntityType = lrEntityTypeInstance
                Me.ObjectifyingEntityType.IsObjectifyingEntityType = True

                Me.Page.MakeDirty()
                Me.Model.Save()
            End If

            If IsSomething(Me.Shape) Then
                Me.Shape.Pen.Color = Color.Black

                '----------------------------------
                'Position the name of the FactType
                '----------------------------------
                Me.FactTypeNameShape.Move(Me.Shape.Bounds.X - 15, Me.Shape.Bounds.Top - CInt(1.5 * Me.FactTypeNameShape.Bounds.Height))
                Me.FactTypeNameShape.Visible = Me.IsObjectified
                Me.ShowFactTypeName = True


                Me.Shape.ShadowOffsetX = 1
                Me.Shape.ShadowOffsetY = 1
                Me.Shape.ShadowColor = Color.LightGray


                '--------------------------------------------------------------
                'Push any RoleName that are within the bounds of the FactType
                '  outside the bounds of the FactType
                '--------------------------------------------------------------
                Dim lo_role_instance As FBM.RoleInstance
                For Each lo_role_instance In Me.RoleGroup
                    If (lo_role_instance.RoleName.Shape.Bounds.Y > Me.Shape.Bounds.Top) And (lo_role_instance.RoleName.Shape.Bounds.Y < Me.Shape.Bounds.Bottom) Then
                        '------------------------------------------------------
                        'RoleName is within verticle bounds of FactType shape
                        '------------------------------------------------------
                        lo_role_instance.RoleName.Shape.Move(lo_role_instance.RoleName.Shape.Bounds.X, Me.Shape.Bounds.Top - CInt(1.2 * lo_role_instance.RoleName.Shape.Bounds.Height))
                    End If
                Next
            End If

        End Sub


        Private Sub _FactType_RemovedFromModel(ByVal abBroadcastIntefaceEvent As Boolean) Handles _FactType.RemovedFromModel

            Call Me.RemoveFromPage(abBroadcastIntefaceEvent)

        End Sub

        Private Sub _FactType_RoleAdded(ByRef arRole As FBM.Role) Handles _FactType.RoleAdded

            Try
                Dim lrRoleInstance As FBM.RoleInstance

                lrRoleInstance = arRole.CloneInstance(Me.Page, True)
                Me.AddRoleInstance(lrRoleInstance)

                '----------------------------------------------------------------------------------------------------
                'Clone a New FBM.FactDataInstance for each (if any) FactData in each Fact for the FactType of the Role
                '----------------------------------------------------------------------------------------------------                        
                Dim lrFact As New FBM.Fact
                Dim lrFactData As New FBM.FactData
                Dim lrFactInstance As New FBM.FactInstance
                Dim lrFactDataInstance As New FBM.FactDataInstance
                For Each lrFact In Me.FactType.Fact
                    lrFactData = New FBM.FactData
                    lrFactData.Fact = lrFact
                    lrFactData.Role = New FBM.Role(Me.FactType, lrRoleInstance.Role.JoinedORMObject)
                    lrFactData.Role.Id = arRole.Id
                    lrFactData.Fact.Symbol = lrFact.Symbol
                    lrFactData = lrFact.Data.Find(AddressOf lrFactData.EqualsByRole)
                    lrFactDataInstance = lrFactData.CloneInstance(Me.Page)
                    lrFactInstance = New FBM.FactInstance
                    lrFactInstance.Id = lrFact.Id
                    lrFactInstance.Symbol = lrFact.Symbol
                    lrFactInstance = Me.Fact.Find(AddressOf lrFactInstance.EqualsById)
                    If IsSomething(lrFactInstance) Then
                        lrFactInstance.Data.Add(lrFactDataInstance)
                    End If
                Next

                '-------------------------------------------------------------------------
                ' Create a new RoleInstance.Shape to be attached to the FactTypeInstance
                '  of the selected RoleInstance.
                '-------------------------------------------------------------------------
                If Me.Shape IsNot Nothing Then
                    Call lrRoleInstance.DisplayAndAssociate(Me)

                    If Me.RoleGroup.FindAll(Function(x) x.JoinedORMObject Is Nothing).Count = 0 Then
                        Call Me.SortRoleGroup()
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

        Private Sub _FactType_RoleRemoved(ByRef arRole As Role) Handles _FactType.RoleRemoved

            Try
                Dim lrRoleInstance As New FBM.RoleInstance

                lrRoleInstance.Id = arRole.Id
                lrRoleInstance = Me.RoleGroup.Find(AddressOf lrRoleInstance.Equals)

                If IsSomething(lrRoleInstance) Then
                    Call Me.RemoveRole(lrRoleInstance)
                End If

                Me.Page.RoleInstance.Remove(lrRoleInstance)

                Call Me.SortRoleGroup()

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Private Sub _FactType_ShortDescriptionChanged(asShortDescription As String) Handles _FactType.ShortDescriptionChanged

            Me.ShortDescription = asShortDescription

        End Sub


        Private Sub _FactType_ShowFactTypeNameChanged(ByVal abNewShowFactTypeName As Boolean) Handles _FactType.ShowFactTypeNameChanged

            Me.ShowFactTypeName = abNewShowFactTypeName
            Call Me.RefreshShape()

        End Sub

        Private Sub update_from_model() Handles _FactType.Updated

            Try
                Me.Id = Me.FactType.Id
                Me.Name = Me.FactType.Id
                Me.Symbol = Me.FactType.Id

                Me.FactTypeNameShape.Text = Me.FactType.Name

                '-----------------------------------------------------------
                'Update the FactTable within the FactTypeInstance on the
                '  current page
                '-----------------------------------------------------------
                If Me.Page.Loaded Then
                    If IsSomething(Me.FactTable.TableShape) And IsSomething(Me.FactTable.FactTypeInstance) Then
                        Call Me.FactTable.ResortFactTable()
                    End If
                End If

                '-----------------------------------------------------------------------
                'Check to see if any of the Facts have been removed at the Model level
                '-----------------------------------------------------------------------
                Dim lrFactInstance As FBM.FactInstance
                For Each lrFactInstance In Me.Fact.ToArray
                    Dim lrFact As New FBM.Fact(Me.FactType)
                    Dim lrFactDataInstance As FBM.FactDataInstance
                    Dim lrFactData As New FBM.FactData
                    For Each lrFactDataInstance In lrFactInstance.Data
                        lrFactData = lrFactDataInstance.CloneFactData
                        lrFact.Data.Add(lrFactData)
                    Next
                    If Me.FactType.Fact.Exists(AddressOf lrFact.EqualsByData) Then
                        '------------------------------------------------
                        'Good, the Fact still exists at the Model level
                        '------------------------------------------------
                    Else
                        Call Me.RemoveFact(lrFactInstance)
                        Call Me.FactTable.ResortFactTable()
                    End If
                Next

                Me.Page.MakeDirty()

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        'Private Sub ModelUpdated() Handles Model.ModelUpdated

        '    If IsSomething(Me.Page) Then
        '        '----------------------------------------------------------------
        '        'The FactTypeInstance is loaded onto a Page.
        '        '  NB FactTypeInstances may be loaded that are not on a Page.
        '        '  The reason for this is that other ModelObjects may reference
        '        '  a FactTypeInstance (within the model, but not be on a Page.
        '        '----------------------------------------------------------------
        '        If IsSomething(Me.Page.diagram) Then
        '            '---------------------------------------------
        '            'Instance is on a Page that is loaded 
        '            '  and displayed on a Form/Diagram
        '            '---------------------------------------------
        '            Call Me.ResortFactTable()
        '        End If
        '    End If

        'End Sub

        Private Sub GetFactsForDropdownCombo(ByVal sender As Object, ByVal e As MindFusion.Diagramming.CellEventArgs) Handles Page.TableCellClicked

            If e.MouseButton = MouseButton.Right Then
                Exit Sub
            End If

            If Me.FactTable.TableShape Is e.Table Then
                '----------------------------------------------------------------
                'Found the Table that was clicked on.
                '  This event triggered for every FactTypeInstance on the Page.
                '----------------------------------------------------------------
            Else
                Exit Sub
            End If

            '-----------------------------------------------
            'Variable declaration put down here for speed.
            '-----------------------------------------------
            Dim lrRoleInstance As New FBM.RoleInstance
            Dim lrJoinedFactTypeInstance As New FBM.FactTypeInstance
            Dim lrJoinedEntityTypeInstance As New FBM.EntityTypeInstance
            Dim lrJoinedValueTypeInstance As New FBM.ValueTypeInstance
            Dim lrFactInstance As FBM.FactInstance

            lrRoleInstance = Me.RoleGroup(e.Column)

            Dim r As RectangleF = Me.FactTable.TableShape.Bounds
            Dim p1 As Point = Me.Page.DiagramView.DocToClient(New PointF(r.Left, r.Top))
            Dim p2 As Point = Me.Page.DiagramView.DocToClient(New PointF(r.Right, r.Bottom))

            Dim lrGraphics As Graphics

            Dim lrMillimetersPerInch As Single = 25.4

            lrGraphics = Me.Page.Form.CreateGraphics

            Dim liCaptionHeight As Integer = e.Table.CaptionHeight '* (lrGraphics.DpiX / lrMillimetersPerInch)
            Dim liTotalRowHeight As Integer = (e.Table.RowCount * (e.Table.RowHeight - 1))

            Dim cr As Rectangle = New Rectangle(p1.X, p1.Y, e.Table.Columns(e.Column).Width * (lrGraphics.DpiX / lrMillimetersPerInch) + 10, (liTotalRowHeight - liCaptionHeight)) ' p2.Y - p1.Y) 
            Dim i As Integer = 0

            'cr.Offset(Me.Page.Diagram.Bounds.X, Me.Page.Diagram.Bounds.Y)

            ' valid only for tables that have all rows of the same height

            Dim liYOffset As Single = liCaptionHeight + Me.Page.DiagramView.ScrollY  '+ (e.Table.Bounds.Y + liCaptionHeight) + _            
            'Dim liActualPenWidth As Single = e.Table.RowHeight - ((e.Table.Bounds.Height - e.Table.CaptionHeight) / e.Table.RowCount)
            Dim liCellHeight As Single = e.Table.RowHeight - 1.5
            ''liYOffset += e.Row * ((liTotalRowHeight - 2) / e.Table.RowCount)
            liYOffset += e.Row * liCellHeight

            Dim liXOffset As Single = 0
            Dim liColumnCounter As Integer = 0
            If e.Column > 0 Then
                For liColumnCounter = 0 To e.Column - 1
                    liXOffset += e.Table.Columns(liColumnCounter).Width
                Next
            End If

            Dim p3 As Point = Me.Page.DiagramView.DocToClient(New PointF(liXOffset, liYOffset))
            cr.Offset(p3.X, p3.Y)

            If cr.Width < 30 Then cr.Width = 30

            Me.Page.Form._table = Nothing
            Me.Page.Form._row = e.Row
            Me.Page.Form._col = e.Column
            Me.Page.Form._table = e.Table

            Select Case lrRoleInstance.TypeOfJoin
                Case Is = pcenumRoleJoinType.FactType
                    '---------------------------------------------------
                    'Get the FactTypeIntance joined by the RoleInstance
                    '---------------------------------------------------
                    lrJoinedFactTypeInstance = lrRoleInstance.JoinedORMObject

                    Me.Page.Form._comboBoxFactInstance.Bounds = cr
                    Me.Page.Form._comboBoxFactInstance.Visible = True

                    '-----------------------
                    'Populate the ComboBox
                    '-----------------------
                    Me.Page.Form._comboBoxFactInstance.Items.clear()
                    For Each lrFactInstance In lrJoinedFactTypeInstance.Fact
                        Dim lo_combobox_item As tComboboxItem = New tComboboxItem(lrFactInstance.Symbol, lrFactInstance.EnumerateAsBracketedFact, lrFactInstance)
                        If Not Me.Page.Form._comboBoxFactInstance.Items.Contains(lo_combobox_item) Then
                            Me.Page.Form._comboBoxFactInstance.Items.Add(lo_combobox_item)
                        End If
                    Next

                    For i = 0 To Me.Page.Form._comboBoxFactInstance.Items.Count - 1
                        If Me.Page.Form._comboBoxFactInstance.Items(i).ToString() = e.Table(Me.Page.Form._col, Me.Page.Form._row).Text Then
                            Me.Page.Form._comboBoxFactInstance.SelectedIndex = i
                            Exit For
                        End If
                    Next

                    Me.Page.Form._ComboBoxFactInstance.focus()

                Case Is = pcenumRoleJoinType.EntityType

                    '------------------------------------------------------
                    'Get the EntityTypeIntance joined by the RoleInstance
                    '------------------------------------------------------
                    lrJoinedEntityTypeInstance = lrRoleInstance.JoinedORMObject

                    Me.Page.Form._comboBoxEntityTypeInstance.Bounds = cr
                    Me.Page.Form._comboBoxEntityTypeInstance.Visible = True

                    '-----------------------
                    'Populate the ComboBox
                    '-----------------------
                    Me.Page.Form._comboBoxEntityTypeInstance.Items.clear()
                    Dim lsEntityTypeInstance As String
                    If lrJoinedEntityTypeInstance.EntityType.HasCompoundReferenceMode Then

                        Dim loCombobox As ComboBox = Me.Page.Form._comboBoxEntityTypeInstance
                        loCombobox.DropDownStyle = ComboBoxStyle.DropDownList
                        For Each lsEntityTypeInstance In lrJoinedEntityTypeInstance.EntityType.Instance

                            Dim lsEnumeratedIdentity As String
                            lsEnumeratedIdentity = lrJoinedEntityTypeInstance.EntityType.EnumerateInstance(lsEntityTypeInstance)

                            Dim lo_combobox_item As tComboboxItem = New tComboboxItem(lsEntityTypeInstance, _
                                                                                      lsEnumeratedIdentity, _
                                                                                      lsEntityTypeInstance)

                            If Not Me.Page.Form._comboBoxEntityTypeInstance.Items.Contains(lo_combobox_item) Then
                                Me.Page.Form._comboBoxEntityTypeInstance.Items.Add(lo_combobox_item)
                            End If
                        Next
                    Else
                        Dim loCombobox As ComboBox = Me.Page.Form._comboBoxEntityTypeInstance
                        loCombobox.DropDownStyle = ComboBoxStyle.DropDown
                        For Each lsEntityTypeInstance In lrJoinedEntityTypeInstance.EntityType.Instance
                            Dim lo_combobox_item As tComboboxItem = New tComboboxItem(lsEntityTypeInstance, lsEntityTypeInstance, lsEntityTypeInstance)
                            If Not Me.Page.Form._comboBoxEntityTypeInstance.Items.Contains(lo_combobox_item) And _
                               Not lsEntityTypeInstance = e.Table(Me.Page.Form._col, Me.Page.Form._row).Text Then
                                Me.Page.Form._comboBoxEntityTypeInstance.Items.Add(lo_combobox_item)
                            End If
                        Next
                    End If

                    Me.Page.Form._comboBoxEntityTypeInstance.Text = e.Table(Me.Page.Form._col, Me.Page.Form._row).Text
                    Me.Page.Form._ComboBoxEntityTypeInstance.focus()

                    Dim lsCellText = e.Table(Me.Page.Form._col, Me.Page.Form._row).Text
                    Dim lrSizeF = lrGraphics.MeasureString(lsCellText, Me.Page.Form._comboBoxEntityTypeInstance.Font)
                    Me.Page.Form._comboBoxEntityTypeInstance.Width = Viev.Greater(lrSizeF.Width + 30, 60)

                Case Is = pcenumRoleJoinType.ValueType

                    '------------------------------------------------------
                    'Get the ValueTypeIntance joined by the RoleInstance
                    '------------------------------------------------------
                    lrJoinedValueTypeInstance = lrRoleInstance.JoinedORMObject

                    Me.Page.Form._comboBoxValueTypeInstance.Bounds = cr
                    Me.Page.Form._comboBoxValueTypeInstance.Visible = True

                    '-----------------------
                    'Populate the ComboBox
                    '-----------------------
                    Me.Page.Form._comboBoxValueTypeInstance.Items.clear()
                    Dim lsValueTypeInstance As String
                    For Each lsValueTypeInstance In lrJoinedValueTypeInstance.ValueType.Instance
                        Dim lo_combobox_item As tComboboxItem = New tComboboxItem(lsValueTypeInstance, lsValueTypeInstance, lsValueTypeInstance)
                        If Not Me.Page.Form._comboBoxValueTypeInstance.Items.Contains(lo_combobox_item) Then
                            Me.Page.Form._comboBoxValueTypeInstance.Items.Add(lo_combobox_item)
                        End If
                    Next

                    Dim lsCellText = e.Table(Me.Page.Form._col, Me.Page.Form._row).Text
                    Me.Page.Form._comboBoxValueTypeInstance.text = lsCellText

                    Dim lrSizeF = lrGraphics.MeasureString(lsCellText, Me.Page.Form._comboBoxValueTypeInstance.Font)
                    Me.Page.Form._comboBoxValueTypeInstance.Width = Viev.Greater(lrSizeF.Width + 30, 60)

                    If lrJoinedValueTypeInstance.ValueConstraint.Count > 0 Then
                        Me.Page.Form._comboBoxValueTypeInstance.Items.clear()
                        For Each lsValueConstraint In lrJoinedValueTypeInstance.ValueConstraint
                            If lsValueConstraint.Contains("..") Then
                                Dim liPosition As Integer = lsValueConstraint.IndexOf(".")
                                Dim lsFirstValue As String = lsValueConstraint.Substring(0, liPosition)
                                liPosition = lsValueConstraint.LastIndexOf(".")
                                Dim lsLastValue As String = lsValueConstraint.Substring(liPosition + 1, (lsValueConstraint.Length - liPosition) - 1)
                                Dim liDummyInteger As Integer
                                If Integer.TryParse(lsFirstValue, liDummyInteger) And Integer.TryParse(lsLastValue, liDummyInteger) Then
                                    Dim liFirstInteger, liLastInteger As Integer
                                    liFirstInteger = Integer.Parse(lsFirstValue)
                                    liLastInteger = Integer.Parse(lsLastValue)
                                    For liInd = liFirstInteger To liLastInteger
                                        Me.Page.Form._comboBoxValueTypeInstance.Items.Add(liInd.ToString)
                                    Next
                                End If
                            End If
                        Next
                        '----------------------------------------------------------------------------------------------------
                        'VM-20180331-Change back to DropDownList if all seems okay. The above range code was created today.
                        '----------------------------------------------------------------------------------------------------
                        'Me.Page.Form._comboBoxValueTypeInstance.DropDownStyle = ComboBoxStyle.DropDownList
                        Me.Page.Form._comboBoxValueTypeInstance.DropDownStyle = ComboBoxStyle.DropDown
                    Else
                        Me.Page.Form._comboBoxValueTypeInstance.DropDownStyle = ComboBoxStyle.DropDown
                    End If

                    Me.Page.Form._ComboBoxValueTypeInstance.focus()

            End Select

        End Sub

        'Private Sub UpdateFromFactTypeFactTableUpdated() Handles _FactType.FactTableUpdated

        '    Dim liInd As Integer = 0
        '    Dim lrFactInstance As New FBM.FactInstance

        '    '------------------------------------------------------
        '    'Allign the set of Facts in the FactTypeInstance with 
        '    '  the set of Facts in the FactType
        '    '------------------------------------------------------
        '    If Me.Page.IsCoreModelPage Then
        '        '-------------------------------------------------------------
        '        'Only add Facts to FactTypeInstances within a Page that is a
        '        '  CoreModelPage. The reason why is because every Page is
        '        '  a different view of the underlying Model, so not 'all' 
        '        '  pages have the same view of the Facts within a FactType.
        '        '  The only 'view' of a FactType that represents the 
        '        '  a reflection of the Model is a CoreModelPage containing
        '        '  that FactType.
        '        '-------------------------------------------------------------
        '        If Me.Fact.Count < Me.FactType.Fact.Count Then
        '            '------------------------------------------------
        '            'There are Facts to add to the FactTypeInstance
        '            '------------------------------------------------
        '            For liInd = (Me.Fact.Count + 1) To Me.FactType.Fact.Count
        '                lrFactInstance = Me.FactType.Fact(liInd - 1).CloneInstance(Me.Page)
        '                Me.Fact.Add(lrFactInstance)
        '            Next
        '            If IsSomething(Me.FactTable) And Me.Page.loaded Then
        '                Me.SortRoleGroup()
        '                Me.FactTable.TableShape.ResizeToFitText(True)
        '            End If
        '        ElseIf Me.Fact.Count > Me.FactType.Fact.Count Then
        '            '-----------------------------------------------------
        '            'There are Facts to remove from the FactTypeInstance
        '            '-----------------------------------------------------
        '            'PSEUDOCODE
        '            '  * FOR EACH Fact in Me (FactTypeInstance)
        '            '      * IF the Fact is not in Me.FactType.Fact(List) THEN
        '            '          * Remove the Fact from Me
        '            '        END IF
        '            '    LOOP
        '            '-----------------------------------------------------
        '            Dim larFactInstanceRemovalList As New List(of tFactInstance)
        '            For Each lrFactInstance In Me.Fact
        '                Dim lrFact As New tFact(lrFactInstance.Symbol, Me.FactType)
        '                Dim lrRoleData As FBM.FactData
        '                Dim lrFactDataInstance As FBM.FactDataInstance
        '                For Each lrFactDataInstance In lrFactInstance.data
        '                    lrRoleData = New FBM.FactData(lrFactDataInstance.role.role, lrFactDataInstance.Concept, lrFact)
        '                    lrFact.data.Add(lrRoleData)
        '                Next
        '                If Not Me.FactType.Fact.Contains(lrFact) Then
        '                    larFactInstanceRemovalList.Add(lrFactInstance)
        '                End If
        '            Next
        '            For Each lrFactInstance In larFactInstanceRemovalList
        '                Me.Fact.Remove(lrFactInstance)
        '            Next
        '            If IsSomething(Me.FactTable) And Me.Page.loaded Then
        '                Me.SortRoleGroup()
        '                Me.FactTable.TableShape.ResizeToFitText(True)
        '            End If
        '        End If
        '    End If

        'End Sub

        Public Sub NodeDeselected() Implements FBM.iPageObject.NodeDeselected

        End Sub


        Private Shadows Event updated()

        Private Sub FactTypeInstanceUpdated() Handles Me.updated

        End Sub

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
        End Sub

        Public Sub SetAppropriateColour() Implements iPageObject.SetAppropriateColour

            If IsSomething(Me.Shape) Then
                If Me.Shape.Selected Then
                    Me.Shape.Pen.Color = Color.Blue
                Else
                    If Me.FactType.HasModelError Then
                        Me.Shape.Visible = True
                        Me.Shape.Pen.Color = Color.Red
                    Else
                        If Me.IsObjectified Then
                            Me.Shape.Pen.Color = Color.Navy
                        Else
                            Me.Shape.Pen.Color = Color.White
                        End If
                    End If
                End If
            End If

        End Sub

        ''' <summary>
        ''' Finds a suitable FactTypeReading for the FactTypeInstance and displays that FactTypeReading.
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub SetSuitableFactTypeReading()

            Dim lrFactTypeReading As FBM.FactTypeReading
            Dim lrFactTypeReadingInstance As FBM.FactTypeReadingInstance

            Call Me.SortRoleGroup()
            Dim larRole As New List(Of FBM.Role)
            Dim lrRoleInstance As FBM.RoleInstance

            For Each lrRoleInstance In Me.RoleGroup
                larRole.Add(lrRoleInstance.Role)
            Next
            lrFactTypeReading = Me.FactType.FindSuitableFactTypeReadingByRoles(larRole)

            If IsSomething(lrFactTypeReading) Then
                lrFactTypeReadingInstance = lrFactTypeReading.CloneInstance(Me.Page)
                lrFactTypeReadingInstance.shape = Me.FactTypeReadingShape.shape
                Me.FactTypeReadingShape = lrFactTypeReadingInstance
                Me.FactTypeReadingShape.RefreshShape()
            Else
                If IsSomething(Me.FactTypeReadingShape) Then
                    If IsSomething(Me.FactTypeReadingShape.shape) Then
                        Me.FactTypeReadingShape.shape.Text = ""
                    End If
                End If
            End If


        End Sub

        Public Sub RepellFromNeighbouringPageObjects(ByVal aiDepth As Integer, ByVal abBroadcastInterfaceEvent As Boolean)

            Dim liRepellDistance As Integer = 25
            Dim liNewX, liNewY As Integer

            Try
                '----------------------------------
                'CodeSafe:
                If aiDepth > 25 Then
                    Exit Sub
                Else
                    aiDepth += 1
                End If


                Dim larPageObject = From PageObject In Me.Page.GetAllPageObjects _
                                            Where PageObject.Id <> Me.Id _
                                            And (Math.Abs(Me.X - PageObject.X) < liRepellDistance _
                                            And Math.Abs(Me.Y - PageObject.Y) < liRepellDistance) _
                                            And PageObject.Shape IsNot Nothing _
                                            Select PageObject

                For Each lrPageObject In larPageObject

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

            If aiDepth > 20 Then
                Exit Sub
            Else
                aiDepth += 1
            End If

            liRepellDistance = 10

            Dim larEntityTypeInstance = From EntityTypeInstance In Me.Page.EntityTypeInstance _
                                        Where (Math.Abs(Me.X - EntityTypeInstance.X) < liRepellDistance _
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

                lrEntityTypeInstance.Move(liNewX, liNewY, True)
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

            '=========================================================
            'FactTypes
            '=========================================================
            Dim larFactTypeInstance = From FactTypeInstance In Me.Page.FactTypeInstance _
                                      Where FactTypeInstance.Id <> Me.Id _
                                      And (Math.Abs(Me.X - FactTypeInstance.X) < liRepellDistance _
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

        End Sub

        Public Sub Move(ByVal aiNewX As Integer, ByVal aiNewY As Integer, ByVal abBroadcastInterfaceEvent As Boolean) Implements iPageObject.Move

            If aiNewX < 0 Then aiNewX = 0
            If aiNewY < 0 Then aiNewY = 0

            If Me.Shape IsNot Nothing Then
                Me.Shape.Move(aiNewX, aiNewY)
            End If

            Me.X = aiNewX
            Me.Y = aiNewY

            '==============================================================================
            'Broadcast the moving of the Object
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
            Me.isDirty = True
            Me.Page.IsDirty = True
            Me.Model.IsDirty = True


        End Sub

        ''' <summary>
        ''' Moves the FactTypeInstance to a position between the ModelElements joined by the Roles of the FactType.
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub MoveToBetweenAssociatedModelObjects(ByVal abBroadcastInterfaceEvent As Boolean)

            Try

                Dim liTotalX, liTotalY As Integer
                Dim liAverageX, liAverageY As Integer

                For Each lrRoleInstance In Me.RoleGroup
                    Dim lrPageObject As iPageObject
                    lrPageObject = lrRoleInstance.JoinedORMObject
                    liTotalX += lrPageObject.X
                    liTotalY += lrPageObject.Y
                Next

                liAverageX = liTotalX / Me.RoleGroup.Count
                liAverageY = liTotalY / Me.RoleGroup.Count

                Call Me.Move(liAverageX, liAverageY, abBroadcastInterfaceEvent)

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
            End Try

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
    End Class

End Namespace