Imports MindFusion.Diagramming
Imports System.ComponentModel
Imports System.Xml.Serialization
Imports System.Collections.Specialized
Imports System.Drawing.Drawing2D
Imports System.Reflection
Imports Newtonsoft.Json

Namespace FBM
    <Serializable()> _
    Public Class ValueTypeInstance
        Inherits FBM.ValueType
        Implements ICloneable
        Implements FBM.iPageObject

        <XmlIgnore()>
        Private WithEvents _ValueType As FBM.ValueType 'The ValueType for which the ValueTypeIstance acts as View/Proxy.
        <XmlIgnore()> _
        <Browsable(False)> _
        Public Property ValueType() As FBM.ValueType
            Get
                Return Me._ValueType
            End Get
            Set(ByVal value As FBM.ValueType)
                Me._ValueType = value
                If value IsNot Nothing Then
                    Me.Concept = value.Concept
                End If
            End Set
        End Property

        Public Shadows Property Name() As String
            Get
                Return _Name
            End Get
            Set(ByVal value As String)
                '-----------------------------------------------------------
                'The surrogate key for the ValueType is about
                '  to change (to match the name of the ValueType)
                '  so update the ModelDictionary entry for the 
                '  Concept/Symbol (the nominalistic idenity of the ValueType)
                '-----------------------------------------------------------
                _Name = value
                Me.Symbol = value
                Me.Id = value
            End Set
        End Property

        <XmlIgnore()> _
        Public Shadows _ValueConstraint As New FBM.ValueConstraintInstance(Me)
        <XmlIgnore()>
        <CategoryAttribute("Value Type"),
        Browsable(True),
        [ReadOnly](False),
        DescriptionAttribute("The List of Values that Objects of this Value Type may take."),
        Editor(GetType(tStringCollectionEditor), GetType(System.Drawing.Design.UITypeEditor))>
        Public Shadows Property ValueConstraint() As Viev.Strings.StringCollection  'NB This is what is edited in the PropertyGrid
            Get
                Return Me._ValueConstraintList
            End Get
            Set(ByVal Value As Viev.Strings.StringCollection)
                Me._ValueConstraintList = Value
            End Set
        End Property

        Public Overloads Property Instances As Viev.Strings.StringCollection
            Get
                Return Me.ValueType.Instances
            End Get
            Set(value As Viev.Strings.StringCollection)
                Me.ValueType.Instances = value
            End Set
        End Property

        <XmlIgnore()>
        <Browsable(False),
        [ReadOnly](True),
        BindableAttribute(False)>
        Public Shadows WithEvents Concept As New FBM.Concept

        <XmlIgnore()>
        Public Shadows SubtypeRelationship As New List(Of FBM.SubtypeRelationshipInstance)

        <XmlIgnore()>
        Public Page As FBM.Page

        <JsonIgnore()>
        <NonSerialized(),
        XmlIgnore()>
        Public _Shape As ShapeNode
        <XmlIgnore()>
        Public Property Shape As ShapeNode Implements iPageObject.Shape
            Get
                Return Me._Shape
            End Get
            Set(value As ShapeNode)
                Me._Shape = value
            End Set
        End Property


        <XmlIgnore()> _
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

        <XmlIgnore()> _
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

        ''' <summary>
        ''' Only used, at this stage, with SubtypeRelationshipInstances (added to Outgoing links)
        ''' </summary>
        <NonSerialized(),
        XmlIgnore()>
        Public OutgoingLink As New List(Of DiagramLink)

        <XmlIgnore>
        <Browsable(True)>
        <CategoryAttribute("Target Database"),
        [ReadOnly](False),
        DescriptionAttribute("The data type for this Value Type in the target database.")>
        Public Shadows ReadOnly Property DBDataType As String
            Get
                Return Me.ValueType.DBDataType
            End Get
        End Property

        <JsonIgnore()>
        <XmlIgnore()>
        <DebuggerBrowsable(DebuggerBrowsableState.Never)>
        Public _HasBeenMoved As Boolean = False

        <JsonIgnore()>
        <XmlIgnore()> _
        Public Property HasBeenMoved() As Boolean  'Used in AutoLayout operations.
            Get
                Return Me._HasBeenMoved
            End Get
            Set(ByVal value As Boolean)
                Me._HasBeenMoved = value
                Me.X = Me.Shape.Bounds.X
                Me.Y = Me.Shape.Bounds.Y
            End Set
        End Property

        Private _InstanceNumber As Integer = 1
        Public Property InstanceNumber As Integer Implements iPageObject.InstanceNumber
            Get
                Return Me._InstanceNumber
            End Get
            Set(value As Integer)
                Me._InstanceNumber = value
            End Set
        End Property

        Public Sub New()

            Me.ValueType = New FBM.ValueType
            Me.m_dctd = DynamicTypeDescriptor.ProviderInstaller.Install(Me)

        End Sub

        Public Sub New(ByRef arModel As FBM.Model,
                       ByRef arPage As FBM.Page,
                       ByVal aiLanguageId As pcenumLanguage,
                       Optional ByVal as_entity_type_name As String = Nothing,
                       Optional ByVal ab_use_name_as_id As Boolean = False,
                       Optional ByVal aiX As Integer = 0,
                       Optional ByVal aiY As Integer = 0)

            Call Me.New()

            Me.Model = arModel
            Me.Page = arPage

            If ab_use_name_as_id Then
                Me.Id = Trim(Me.Name)
            Else
                Me.Id = System.Guid.NewGuid.ToString
            End If

            If IsSomething(as_entity_type_name) Then
                Me.Name = as_entity_type_name
            Else
                Me.Name = "New Value Type"
            End If

            Me.X = aiX
            Me.Y = aiY

        End Sub

        Public Overloads Function Clone(ByRef arPage As FBM.Page, _
                                        Optional ByVal abIsMDAModelElement As Boolean = False,
                                        Optional ByVal abAddToPage As Boolean = False) As Object

            Dim lrValueTypeInstance As New FBM.ValueTypeInstance

            Try

                If arPage.ValueTypeInstance.Find(Function(x) x.Id = Me.Id) IsNot Nothing Then
                    lrValueTypeInstance = arPage.ValueTypeInstance.Find(Function(x) x.Id = Me.Id)
                Else
                    With Me
                        lrValueTypeInstance.isDirty = True
                        lrValueTypeInstance.Model = arPage.Model
                        lrValueTypeInstance.Page = arPage
                        lrValueTypeInstance.Symbol = .Symbol
                        lrValueTypeInstance.Id = .Id
                        lrValueTypeInstance.Name = .Name
                        If abIsMDAModelElement = False Then
                            lrValueTypeInstance.IsMDAModelElement = .IsMDAModelElement
                        Else
                            lrValueTypeInstance.IsMDAModelElement = abIsMDAModelElement
                        End If

                        If arPage.Model.ValueType.Exists(AddressOf .ValueType.Equals) Then
                            lrValueTypeInstance.ValueType = arPage.Model.ValueType.Find(AddressOf .ValueType.Equals)
                        Else
                            Dim lrValueType As New FBM.ValueType
                            lrValueType = .ValueType.Clone(arPage.Model, False, abIsMDAModelElement)
                            arPage.Model.AddValueType(lrValueType,,,, True)
                            lrValueTypeInstance.ValueType = lrValueType
                        End If

                        'Call lrValueTypeInstance.SetName(.Name)
                        lrValueTypeInstance.ValueConstraint = .ValueConstraint

                        lrValueTypeInstance.X = .X
                        lrValueTypeInstance.Y = .Y
                    End With

                    If abAddToPage Then
                        arPage.ValueTypeInstance.AddUnique(lrValueTypeInstance)
                    End If

                End If

                Return lrValueTypeInstance

            Catch ex As Exception
                Dim lsMessage As String = ""

                lsMessage = "Error: tValueTypeInstance.Clone: " & vbCrLf & vbCrLf & ex.Message
                Call prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)

                Return lrValueTypeInstance
            End Try

        End Function

        Public Function CloneConceptInstance() As FBM.ConceptInstance

            Dim lrConceptInstance As New FBM.ConceptInstance(Me.Model, Me.Page, Me.Id, Me.ConceptType)

            lrConceptInstance.X = Me.X
            lrConceptInstance.Y = Me.Y

            Return lrConceptInstance

        End Function

        Public Function ClonePageObject() As FBM.PageObject

            Dim lrPageObject As New FBM.PageObject

            lrPageObject.Name = Me.Name
            lrPageObject.Shape = Me.Shape
            lrPageObject.X = Me.Shape.Bounds.X
            lrPageObject.Y = Me.Shape.Bounds.Y

            Return lrPageObject

        End Function

        Public Shadows Function Equals(ByVal other As FBM.ValueTypeInstance) As Boolean

            Return Me.Id = other.Id And Me.InstanceNumber = other.InstanceNumber

        End Function


        Public Sub DisplayAndAssociate(Optional ByVal abForceDisplay As Boolean = False)

            Dim loDroppedNode As ShapeNode
            Dim lo_value_constraint As ShapeNode

            Try
                '--------------------------------------------------------------------
                'Create a Shape for the ValueTypeInstance on the DiagramView object
                '--------------------------------------------------------------------            
                If Me.Shape IsNot Nothing Then
                    'Is already Displayed and Associated
                    Exit Sub
                End If
                If Me.Page.Diagram Is Nothing Then Exit Sub

                loDroppedNode = Me.Page.Diagram.Factory.CreateShapeNode(Me.X, Me.Y, 2, 2)
                loDroppedNode.Shape = Shapes.RoundRect
                loDroppedNode.Pen.DashPattern = New Single() {1, 1, 1, 1}
                loDroppedNode.HandlesStyle = HandlesStyle.InvisibleMove
                loDroppedNode.Text = Me.Name & Boston.returnIfTrue(Me.IsIndependent, " ! ", "")
                loDroppedNode.Resize(20, 12)
                loDroppedNode.ShadowOffsetX = 1
                loDroppedNode.ShadowOffsetY = 1
                loDroppedNode.ShadowColor = Color.LightGray
                loDroppedNode.Tag = Me
                loDroppedNode.Pen.Width = 0.5
                If Me.IsReferenceModeForFactTypeOnPage And (Not abForceDisplay) Then
                    loDroppedNode.Visible = False
                Else
                    loDroppedNode.Visible = True
                End If

                Me.Shape = loDroppedNode
                loDroppedNode.ToolTip = "Value Type"


                loDroppedNode.Pen.DashStyle = DashStyle.Custom

                ReDim loDroppedNode.Pen.DashPattern(3)
                loDroppedNode.Pen.DashPattern(0) = 4
                loDroppedNode.Pen.DashPattern(1) = 3
                loDroppedNode.Pen.DashPattern(2) = 4
                loDroppedNode.Pen.DashPattern(3) = 3
                loDroppedNode.Pen.Color = Color.Navy

                '----------------------------------------
                'Setup the ValueConstraint
                '----------------------------------------                        
                Dim StringSize As New SizeF
                Dim G As Graphics
                Dim ls_enumerated_value_constraint As String = Me.EnumerateValueConstraint 'The ValueTypeConstraint enumerated e.g. "1,2,3,4" or "value1, value2, value3" etc

                G = Me.Page.Form.CreateGraphics

                If ls_enumerated_value_constraint = "" Then
                    StringSize = Me.Page.Diagram.MeasureString("", Me.Page.Diagram.Font, 1000, System.Drawing.StringFormat.GenericDefault)
                Else
                    StringSize = Me.Page.Diagram.MeasureString("{" & Trim(ls_enumerated_value_constraint) & "}", Me.Page.Diagram.Font, 1000, System.Drawing.StringFormat.GenericDefault)
                End If

                lo_value_constraint = Me.Page.Diagram.Factory.CreateShapeNode(loDroppedNode.Bounds.X, loDroppedNode.Bounds.Y - (StringSize.Height * 2), StringSize.Width, StringSize.Height, MindFusion.Diagramming.Shapes.Rectangle)


                If ls_enumerated_value_constraint = "" Then
                    lo_value_constraint.Text = ""
                Else
                    lo_value_constraint.Text = "{" & Trim(ls_enumerated_value_constraint) & "}"
                    lo_value_constraint.HandlesStyle = HandlesStyle.InvisibleMove
                    lo_value_constraint.Resize(StringSize.Width, StringSize.Height)

                    lo_value_constraint.TextColor = Color.Maroon
                    lo_value_constraint.Transparent = True
                    lo_value_constraint.Visible = True
                    lo_value_constraint.ZTop()
                End If


                Dim lrValueConstraint As FBM.ValueConstraintInstance = New FBM.ValueConstraintInstance
                lrValueConstraint.ValueType = Me
                lo_value_constraint.Tag = lrValueConstraint

                '--------------------------------------------------------------
                'Attach the ValueConstraintInstance ShapeNode to the ValueType
                '--------------------------------------------------------------
                lo_value_constraint.AttachTo(loDroppedNode, AttachToNode.TopLeft)
                lrValueConstraint.Shape = lo_value_constraint
                Me._ValueConstraint = lrValueConstraint
                If Me.ValueType.IsReferenceMode Then
                    lrValueConstraint.Shape.Visible = False
                Else
                    lrValueConstraint.Shape.Visible = True
                End If
                Me._ValueConstraint = lrValueConstraint

                Dim liValueTypeNameStringSize As New SizeF
                '---------------------------------------
                'Set the size of the ValueTypeInstance
                '---------------------------------------
                G = Me.Page.Form.CreateGraphics
                liValueTypeNameStringSize = Me.Page.Diagram.MeasureString(Trim(Me.Name & Boston.returnIfTrue(Me.IsIndependent, " ! ", "")), Me.Page.Diagram.Font, 1000, System.Drawing.StringFormat.GenericDefault)
                Dim loRectangle As New Rectangle(Me.X, Me.Y, liValueTypeNameStringSize.Width + 4, liValueTypeNameStringSize.Height + 4)
                Me.Shape.SetRect(loRectangle, False)

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Public Function EnumerateValueConstraint() As String

            Dim liCounter As Integer = 0
            Dim lsConcept As String

            EnumerateValueConstraint = ""

            For Each lsConcept In Me.ValueType.ValueConstraint
                liCounter += 1
                If liCounter < Me.ValueType.ValueConstraint.Count Then
                    EnumerateValueConstraint &= lsConcept & ","
                Else
                    EnumerateValueConstraint &= lsConcept
                End If
            Next

        End Function

        Public Overrides Function HasSubTypes() As Boolean

            Return Me.ValueType.HasSubTypes

        End Function

        Public Overloads Function GetAdjoinedRoles(Optional abIgnoreReferenceModeFactTypes As Boolean = False) As List(Of FBM.RoleInstance)

            Try
                Dim larReturnRoleInstances As New List(Of FBM.RoleInstance)

                Dim larRoleInstance = From FactTypeInstance In Me.Page.FactTypeInstance.FindAll(Function(x) x.isPreferredReferenceMode = Not abIgnoreReferenceModeFactTypes)
                                      From RoleInstance In FactTypeInstance.RoleGroup
                                      Where RoleInstance.JoinedORMObject IsNot Nothing
                                      Where RoleInstance.JoinedORMObject.Id = Me.Id
                                      Select RoleInstance

                For Each lrRoleInstance In larRoleInstance
                    larReturnRoleInstances.Add(lrRoleInstance)
                Next
                Return larReturnRoleInstances

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)

                Return Nothing
            End Try

        End Function

        Public Sub getBlankCellCloseBy(ByRef aiX As Integer, ByRef aiY As Integer)

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
        ''' Returns the Model level ModelObject for this Instance.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overrides Function getBaseModelObject() As FBM.ModelObject

            Return Me.ValueType

        End Function

        ''' <summary>
        ''' Returns True if the ValueType represents the ReferenceMode/Scheme of a FactType(Instance) on the Page.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function IsReferenceModeForFactTypeOnPage() As Boolean

            IsReferenceModeForFactTypeOnPage = False

            Try
                Dim larGoodFactTypeInstances = From FactTypeInstance In Me.Page.FactTypeInstance _
                                          Where FactTypeInstance.HasNULLRole = False _
                                          Select FactTypeInstance

                Dim larFactTypeInstance = (From FactTypeInstance In larGoodFactTypeInstances _
                                           From RoleInstance In FactTypeInstance.RoleGroup _
                                          Where RoleInstance.JoinedORMObject.Id = Me.Id _
                                          And FactTypeInstance.isPreferredReferenceMode = True _
                                          Select FactTypeInstance).Count

                Return larFactTypeInstance > 0

                'Old code before .Count was introduced.
                'Dim lrFactTypeInstance As FBM.FactTypeInstance

                'For Each lrFactTypeInstance In larFactTypeInstance
                '    IsReferenceModeForFactTypeOnPage = True
                'Next

            Catch ex As Exception
                Dim lsMessage As String = ""
                lsMessage = "Error: tValueTypeInstance.IsReferenceModeForFactTypeOnPage: " & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
                Return False
            End Try

        End Function


        Public Function RemoveFromPage(ByVal abBroadcastInterfaceEvent As Boolean,
                                       Optional ByVal abIgnoreJoinedRoles As Boolean = False) As Boolean

            Dim lsMessage As String = ""
            Dim liFactTypeInstanceCount As Integer = 0

            Try
                RemoveFromPage = True

                If abIgnoreJoinedRoles Then GoTo RemoveAnyway

                liFactTypeInstanceCount = Aggregate FactType In Me.Page.FactTypeInstance
                                               From Role In FactType.RoleGroup
                                              Where Role.JoinedORMObject IsNot Nothing
                                              Where Role.JoinedORMObject.Id = Me.Id
                                              Where CType(Role.JoinedORMObject, Object).InstanceNumber = Me.InstanceNumber
                                              Into Count()


                If liFactTypeInstanceCount = 0 Then
RemoveAnyway:
                    Call Me.Page.RemoveValueTypeInstance(Me, abBroadcastInterfaceEvent)

                    Call Me.Page.MakeDirty()
                Else
                    lsMessage = "You cannot remove the Value Type, '" & Trim(Me.Name) & "' until all Fact Types with Roles assigned to the Value Type have been removed from the Page."
                    prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Warning, Nothing, False, False, True)
                    Return False
                End If

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)

                Return False
            End Try


        End Function

        Public Overrides Sub Save(Optional ByVal abRapidSave As Boolean = False)

            '---------------------------------------------
            'Saves the ValueTypeInstance to the database
            '---------------------------------------------
            Dim lrConceptInstance As New FBM.ConceptInstance

            lrConceptInstance.ModelId = Me.Model.ModelId
            lrConceptInstance.PageId = Me.Page.PageId
            lrConceptInstance.Symbol = Me.Id
            lrConceptInstance.X = Me.X
            lrConceptInstance.Y = Me.Y
            lrConceptInstance.ConceptType = pcenumConceptType.ValueType
            lrConceptInstance.InstanceNumber = Me.InstanceNumber

            If abRapidSave Then
                Call TableConceptInstance.AddConceptInstance(lrConceptInstance)
            Else
                If TableConceptInstance.ExistsConceptInstance(lrConceptInstance, False) Then
                    Call TableConceptInstance.UpdateConceptInstance(lrConceptInstance)
                Else
                    Call TableConceptInstance.AddConceptInstance(lrConceptInstance)
                End If
            End If

        End Sub

        Public Overloads Sub SetDataType(ByVal aiDataType As pcenumORMDataType, Optional ByVal aiDataTypeLength As Integer = -1, Optional ByVal aiDataTypePrecision As Integer = -1)

            Me.DataType = aiDataType
            Call Me.ValueType.SetDataType(aiDataType)

            If aiDataTypeLength >= 0 Then
                Me.DataTypeLength = aiDataTypeLength
                Call Me.ValueType.SetDataTypeLength(aiDataTypeLength)
            End If

            If aiDataTypePrecision >= 0 Then
                Me.DataTypePrecision = aiDataTypePrecision
                Call Me.ValueType.SetDataTypePrecision(aiDataTypePrecision)
            End If

        End Sub

        Public Overloads Sub SetName(ByVal asName As String)

            Me.Name = asName
            Me.Id = asName
            Me.Symbol = asName

            Call Me.ValueType.SetName(asName)

            Call TableConceptInstance.ModifyKey(Me, asName)

            Me.Page.IsDirty = True

        End Sub

        Private Sub _ValueType_DataTypeChanged(ByVal aiNewDataType As pcenumORMDataType) Handles _ValueType.DataTypeChanged

            Me.DataType = aiNewDataType
            Me.DataTypeLength = Me.ValueType.DataTypeLength
            Me.DataTypePrecision = Me.ValueType.DataTypePrecision

            Dim lrModelError As New FBM.ModelError("127", Me.ValueType)
            lrModelError.Description = "Data Type Not Specified Error - Value Type: '" & Me.ValueType.Name & "'."

            Me.ValueType.ModelError.Remove(lrModelError)

            Call Me.SetAppropriateColour()

        End Sub

        Private Sub _ValueType_DataTypeLengthChanged(ByVal aiDataTypeLength As Integer) Handles _ValueType.DataTypeLengthChanged

            Me.DataTypeLength = aiDataTypeLength

        End Sub

        Private Sub _ValueType_DataTypePrecisionChanged(ByVal aiNewDataTypePrecision As Integer) Handles _ValueType.DataTypePrecisionChanged

            Me.DataTypePrecision = aiNewDataTypePrecision

        End Sub

        Private Sub _ValueType_IsIndependentChanged(abNewIsIndependent As Boolean) Handles _ValueType.IsIndependentChanged

            Me.IsIndependent = abNewIsIndependent

            Call Me.RefreshShape()

        End Sub

        Private Sub _ValueType_LongDescriptionChanged(asLongDescription As String) Handles _ValueType.LongDescriptionChanged

            Me.LongDescription = asLongDescription

        End Sub

        Private Sub _ValueType_NameChanged() Handles _ValueType.NameChanged

            '---------------------------------------
            'Set the size of the ValueTypeInstance
            '---------------------------------------
            Try
                Dim lrConceptInstance As New FBM.ConceptInstance(Me.Model, Me.Page, Me.ValueType.Id, pcenumConceptType.ValueType)
                lrConceptInstance.X = Me.X
                lrConceptInstance.Y = Me.Y
                If Not TableConceptInstance.ExistsConceptInstanceByModelPageConceptTypeRoleId(lrConceptInstance) Then
                    Call TableConceptInstance.UpdateConceptInstanceByModelPageConceptTypeRoleId(lrConceptInstance, Me.Id)
                End If

                Me.Id = Me.ValueType.Id
                Me.Symbol = Me.ValueType.Id
                Me.Name = Me.ValueType.Id

                If IsSomething(Me.Shape) Then
                    Dim G As Graphics
                    Dim liValueTypeNameStringSize As New SizeF
                    G = Me.Page.Form.CreateGraphics
                    liValueTypeNameStringSize = Me.Page.Diagram.MeasureString(Trim(Me.Name), _
                                                                                Me.Page.Diagram.Font, _
                                                                                1000, _
                                                                                System.Drawing.StringFormat.GenericDefault)
                    Dim loRectangle As New Rectangle(Me.X, Me.Y, liValueTypeNameStringSize.Width + 4, liValueTypeNameStringSize.Height + 4)
                    Me.Shape.SetRect(loRectangle, False)
                End If

                'Me.Page.MakeDirty()

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Private Sub _ValueType_RemovedFromModel(ByVal abBroadcastInterfaceEvent As Boolean) Handles _ValueType.RemovedFromModel

            Try
                Call Me.RemoveFromPage(abBroadcastInterfaceEvent)

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Private Sub _ValueType_ShortDescriptionChanged(asShortDescription As String) Handles _ValueType.ShortDescriptionChanged

            Me.ShortDescription = asShortDescription

        End Sub


        Private Sub update_from_model() Handles _ValueType.updated

            '---------------------------------------------------------------------
            'Linked by Delegate in New to the 'update' event of the ModelObject 
            '  referenced by Objects of this Class
            '---------------------------------------------------------------------
            Try
                Me.Name = Me.ValueType.Name

                If IsSomething(Me.Page.Diagram) Then
                    '------------------
                    'Diagram is set.
                    '------------------
                    If IsSomething(Me.Shape) Then
                        If Me.Shape.Text <> "" Then
                            '---------------------------------------------------------------------------------
                            'Is the type of EntityTypeInstance that 
                            '  shows the EntityTypeName within the
                            '  ShapeNode itself and not a separate
                            '  ShapeNode attached to it (e.g. An Actor EntityTypeInstance has two ShapeNodes, 
                            ' 1 for the stickfigure, the other for the name of the Actor.
                            '---------------------------------------------------------------------------------
                            Me.Shape.Text = Trim(Me.Name)

                            Dim lrConcept As FBM.Concept
                            Me.ValueConstraint.Clear()
                            For Each lrConcept In Me.ValueType._ValueConstraint
                                Me.ValueConstraint.Add(lrConcept.Symbol)
                            Next

                            Call Me._ValueConstraint.RefreshShape()
                        End If

                    End If
                    Me.Page.Diagram.Invalidate()
                End If
            Catch lo_err As Exception
                MsgBox("class_ORM_entity_type_instance.update_from_model: " & lo_err.Message & "EntityTypeName: " & Me.Name & ", PageId:" & Me.Page.PageId)
            End Try

        End Sub

        Public Sub RefreshShape(Optional ByVal aoChangedPropertyItem As PropertyValueChangedEventArgs = Nothing,
                                Optional ByVal asSelectedGridItemLabel As String = "")


            Try
                '------------------------------------------------
                'Set the values in the underlying Model.ValueType
                '------------------------------------------------

                'VM 20101218-Use the following and e above to fix the below
                ' -> "ValueConstraintValue", e.OldValue.ToString, e.ChangedItem.Value.ToString
                'was -> Optional ByVal asAttributeName As String = Nothing, Optional ByVal aoOldValue As Object = Nothing, Optional ByVal aoNewValue As Object = Nothing
                If IsSomething(aoChangedPropertyItem) Then
                    Select Case aoChangedPropertyItem.ChangedItem.PropertyDescriptor.Name
                        Case Is = "DataType"
                            Select Case Me.DataType
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
                            With New WaitCursor
                                Call Me.ValueType.SetDataType(Me.DataType)
                            End With
                        Case Is = "DataTypePrecision"
                            With New WaitCursor
                                Call Me.ValueType.SetDataTypePrecision(Me.DataTypePrecision)
                            End With
                        Case Is = "DataTypeLength"
                            With New WaitCursor
                                Call Me.ValueType.SetDataTypeLength(Me.DataTypeLength)
                            End With
                        Case Is = "IsIndependent"
                            With New WaitCursor
                                Call Me.ValueType.SetIsIndependent(Me.IsIndependent, True)
                            End With
                        Case Is = "ShortDescription"
                            With New WaitCursor
                                Call Me.ValueType.SetShortDescription(Me.ShortDescription)
                                Me.Model.ModelDictionary.Find(Function(x) LCase(x.Symbol) = LCase(Me.Id)).ShortDescription = Me.ShortDescription
                            End With
                        Case Is = "LongDescription"
                            With New WaitCursor
                                Call Me.ValueType.SetLongDescription(Me.LongDescription)
                                Me.Model.ModelDictionary.Find(Function(x) LCase(x.Symbol) = LCase(Me.Id)).LongDescription = Me.LongDescription
                            End With
                        Case Is = "Value"
                            With New WaitCursor
                                Select Case asSelectedGridItemLabel
                                    Case Is = "Instances"
                                        Call Me.ValueType.setInstance(aoChangedPropertyItem.OldValue, aoChangedPropertyItem.ChangedItem.Value.ToString)
                                    Case Else
                                        Call Me.ValueType.ModifyValueConstraint(aoChangedPropertyItem.OldValue, aoChangedPropertyItem.ChangedItem.Value.ToString)
                                End Select
                            End With
                        Case Is = "Name"
                            If Me.ValueType.Name = Me.Name Then
                                '------------------------------------------------------------
                                'Nothing to do. Name of the ValueType has not been changed.
                                '------------------------------------------------------------
                            Else
                                Dim lrEntityTypeDictionaryEntry As New FBM.DictionaryEntry(Me.Model, Me.Name, pcenumConceptType.EntityType)
                                Dim lrValueTypeDictionaryEntry As New FBM.DictionaryEntry(Me.Model, Me.Name, pcenumConceptType.ValueType)
                                Dim lrFactTypeDictionaryEntry As New FBM.DictionaryEntry(Me.Model, Me.Name, pcenumConceptType.FactType)
                                Dim lrRoleConstraintDictionaryEntry As New FBM.DictionaryEntry(Me.Model, Me.Name, pcenumConceptType.RoleConstraint)

                                If LCase(Me.Name) = LCase(Me.ValueType.Name) Then
                                    Me.ValueType.SetName(Me.Name)
                                    Me.Id = Me.Name
                                    Me.Symbol = Me.Name
                                Else
                                    If Me.Model.ModelDictionary.Exists(AddressOf lrValueTypeDictionaryEntry.EqualsBySymbolConceptType) Then
                                        MsgBox("An Value Type with the name, '" & lrValueTypeDictionaryEntry.Symbol & "', already exists in the Model, '" & Me.Model.Name & "'.", MsgBoxStyle.Exclamation, "Model Object Conflict")
                                        Me.Name = Me.ValueType.Name
                                    ElseIf Me.Model.ModelDictionary.Exists(AddressOf lrEntityTypeDictionaryEntry.EqualsBySymbolConceptType) Then
                                        MsgBox("The name, '" & lrValueTypeDictionaryEntry.Symbol & "', conflicts with a Entity Type of the same name in the Model, '" & Me.Model.Name & "'.", MsgBoxStyle.Exclamation, "Model Object Conflict")
                                        Me.Name = Me.ValueType.Name
                                    ElseIf Me.Model.ModelDictionary.Exists(AddressOf lrFactTypeDictionaryEntry.EqualsBySymbolConceptType) Then
                                        MsgBox("The name, '" & lrValueTypeDictionaryEntry.Symbol & "', conflicts with a Fact Type of the same name in the Model, '" & Me.Model.Name & "'.", MsgBoxStyle.Exclamation, "Model Object Conflict")
                                        Me.Name = Me.ValueType.Name
                                    ElseIf Me.Model.ModelDictionary.Exists(AddressOf lrRoleConstraintDictionaryEntry.EqualsBySymbolConceptType) Then
                                        MsgBox("The name, '" & lrValueTypeDictionaryEntry.Symbol & "', conflicts with a Role Constraint of the same name in the Model, '" & Me.Model.Name & "'.", MsgBoxStyle.Exclamation, "Model Object Conflict")
                                        Me.Name = Me.ValueType.Name
                                    ElseIf Me.Name.Contains("'") Then
                                        MsgBox("The name of an Value Type cannot contain a ' (single quote).")
                                        Me.Name = Me.ValueType.Name
                                    ElseIf Not FBM.IsAcceptableObjectTypeName(Me.Name) Then
                                        MsgBox("The name of an Value Type can only contain the characters [a-zA-Z0-9].")
                                        Me.Name = Me.ValueType.Name
                                    Else
                                        With New WaitCursor
                                            Me.ValueType.SetName(Me.Name)
                                            Me.Id = Me.Name
                                            Me.Symbol = Me.Name
                                        End With
                                    End If
                                End If

                                If IsSomething(Me.Shape) Then
                                    Me.Shape.Text = Trim(Me.Name)
                                End If
                            End If
                    End Select
                End If

                '-------------------------------------------------------------------------------------------------------------------------------
                'Removing an item using the UITypeEditor does not trigger a return of aoChangedPropertyItem (As PropertyValueChangedEventArgs).
                '  So we must check each time (back here) whether there is an item to remove from the ValueConstraint list for the ValueType/Instance.
                Dim lasValueConstraint(Me.ValueType.ValueConstraint.Count) As String
                Me.ValueType.ValueConstraint.CopyTo(lasValueConstraint, 0)

                For Each lsValueConstraint In lasValueConstraint
                    If lsValueConstraint IsNot Nothing Then
                        If Not Me.ValueConstraint.Contains(lsValueConstraint) Then
                            Call Me.ValueType.RemoveValueConstraint(lsValueConstraint)
                        End If
                    End If
                Next

                '---------------------------------------
                'Set the size of the ValueTypeInstance
                '---------------------------------------
                Dim G As Graphics
                Dim liValueTypeNameStringSize As New SizeF

                If IsSomething(Me.Shape) And Me.Page.Form IsNot Nothing Then
                    G = Me.Page.Form.CreateGraphics
                    liValueTypeNameStringSize = Me.Page.Diagram.MeasureString(Trim(Me.Name), Me.Page.Diagram.Font, 1000, System.Drawing.StringFormat.GenericDefault)
                    Dim loRectangle As New Rectangle(Me.X, Me.Y, liValueTypeNameStringSize.Width + 4, liValueTypeNameStringSize.Height + 4)

                    Me.Shape.SetRect(loRectangle, False)

                    If Me.ValueType.HasModelError Then
                        Me.Shape.Pen.Color = Color.Red
                    Else
                        Me.Shape.Pen.Color = Color.Navy
                    End If
                End If

                If Me.IsIndependent Then
                    Me.Shape.Text = Me.Id & " !"
                End If

                If Me.Model IsNot Nothing Then
                    Call Me.Model.MakeDirty(True, False)
                End If

                If Me.Page.Form IsNot Nothing Then
                    Call Me.Page.Form.ResetPropertiesGridToolbox(Me)
                End If

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Private Sub tValueTypeInstance_updated() Handles Me.updated

        End Sub

        Public Sub MouseDown() Implements iPageObject.MouseDown

        End Sub

        Public Sub MouseMove() Implements iPageObject.MouseMove

        End Sub

        Public Sub MouseUp() Implements iPageObject.MouseUp

        End Sub

        Public Sub Moved() Implements iPageObject.Moved

        End Sub

        Public Function MoveToClearSpace() As Boolean

            Dim abInClearSpace As Boolean

            abInClearSpace = True

            Dim liClashCount = Aggregate ValueTypeInstance In Me.Page.ValueTypeInstance _
                                   Where ValueTypeInstance.Id <> Me.Id _
                                     And ValueTypeInstance.X = Me.X _
                                     And ValueTypeInstance.Y = Me.Y _
                                    Into Count()

            If liClashCount > 0 Then
                Me.X = Me.X + 5
                Me.Y = Me.Y + 5
                Call Me.RefreshShape()
                abInClearSpace = False
            End If

            If Not abInClearSpace Then
                Call Me.MoveToClearSpace()
            End If

            Return abInClearSpace

        End Function

        Public Sub NodeDeleting() Implements iPageObject.NodeDeleting

        End Sub

        Public Sub NodeModified() Implements iPageObject.NodeModified

        End Sub

        Public Sub NodeSelected() Implements iPageObject.NodeSelected

            Call Me.SetAppropriateColour()
        End Sub

        Public Sub NodeDeselected() Implements FBM.iPageObject.NodeDeselected

            Call Me.SetAppropriateColour()
        End Sub

        Public Sub SetAppropriateColour() Implements iPageObject.SetAppropriateColour

            If IsSomething(Me.Shape) Then
                If Me.Shape.Selected Then
                    Me.Shape.Pen.Color = Color.Blue
                Else
                    If Me.ValueType.HasModelError Then
                        Me.Shape.Pen.Color = Color.Red
                    Else
                        Me.Shape.Pen.Color = Color.Navy
                    End If
                End If

                If Me.Page.Diagram IsNot Nothing Then
                    Me.Page.Diagram.Invalidate()
                End If

            End If

        End Sub

        Public Sub RepellFromNeighbouringPageObjects(ByVal aiDepth As Integer, ByVal abBroadcastInterfaceEvent As Boolean)

            Dim liRepellDistance As Integer = 35
            Dim liNewX, liNewY As Integer

            '----------------------------------
            'CodeSafe:
            If aiDepth > 35 Then
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
            Dim liRepellDistance As Integer
            Dim liNewX As Integer
            Dim liNewY As Integer

            If aiDepth > 20 Then
                Exit Sub
            End If

            liRepellDistance = 25

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
                            Where ValueTypeInstance.Id <> Me.Id _
                            And (Math.Abs(Me.X - ValueTypeInstance.X) < liRepellDistance _
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
        End Sub

        Public Sub Move(ByVal aiNewX As Integer, ByVal aiNewY As Integer, ByVal abBroadcastInterfaceEvent As Boolean) Implements iPageObject.Move

            If aiNewX < 0 Then aiNewX = 0
            If aiNewY < 0 Then aiNewY = 0

            Me.Shape.Move(aiNewX, aiNewY)

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

        End Sub

        Private Sub _ValueType_ValueConstraintAdded(asNewValueConstraint As String) Handles _ValueType.ValueConstraintAdded

            Try
                If Not Me.ValueConstraint.Contains(asNewValueConstraint) Then
                    Me.ValueConstraint.Add(asNewValueConstraint)
                End If

                Call Me._ValueConstraint.RefreshShape()

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Private Sub _ValueType_ValueConstraintModified(asOldValue As String, asNewValue As String) Handles _ValueType.ValueConstraintModified

            Try
                Me.ValueConstraint.Item(Me.ValueConstraint.IndexOf(asOldValue)) = asNewValue

                Call Me._ValueConstraint.RefreshShape()

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Private Sub _ValueType_ValueConstraintRemoved(ByVal asRemovedValueConstraint As String) Handles _ValueType.ValueConstraintRemoved

            Try
                Me.ValueConstraint.Remove(asRemovedValueConstraint)

                Call Me._ValueConstraint.RefreshShape()

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
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

        Private Sub _ValueType_ChangedToEntityType(ByRef arEntityType As EntityType) Handles _ValueType.ChangedToEntityType

            Try
                Dim lrEntityTypeInstance As FBM.EntityTypeInstance = arEntityType.CloneInstance(Me.Page, True)
                lrEntityTypeInstance.x = Me.X
                lrEntityTypeInstance.y = Me.Y

                Call lrEntityTypeInstance.DisplayAndAssociate()

                Call Me.RemoveFromPage(True, True)

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