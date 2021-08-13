Imports System.ComponentModel
Imports System.Xml.Serialization
Imports MindFusion.Diagramming
Imports MindFusion.Drawing
Imports System.Reflection
Imports Boston.FBM

Namespace ERD
    ''' <summary>
    ''' Used to draw/store ERD Entities when drawing an ERDDiagram.
    ''' </summary>
    ''' <remarks></remarks>
    <Serializable()>
    Public Class Entity
        Inherits FBM.FactDataInstance
        Implements IEquatable(Of FBM.EntityTypeInstance)
        Implements FBM.iTableNodePageObject

        <XmlAttribute()>
        Public NodeType As pcenumPGSEntityType = pcenumPGSEntityType.Node

        Public Shadows WithEvents FactData As New FBM.FactData

        <CategoryAttribute("Entity"),
         Browsable(True),
         [ReadOnly](False),
         BindableAttribute(True),
         DefaultValueAttribute(""),
         DesignOnly(False),
         DescriptionAttribute("The name of the Entity")>
        Public Overrides Property Name() As String
            Get
                Return Me._Name
            End Get
            Set(ByVal Value As String)
                Me._Name = Value
                Me._Symbol = Value
                Me.Id = Value
            End Set
        End Property


        Public Attribute As New List(Of ERD.Attribute)
        Public Relation As New List(Of ERD.Relation)

        Public PrimaryKey As New List(Of ERD.Attribute)

        Public Overrides ReadOnly Property isSubtype As Boolean
            Get
                Return Me.RDSTable.isSubtype
            End Get
        End Property

        Public DisplayRDSData As Boolean = False

        Public Shadows TableShape As ERD.TableNode

        '----------------------------------------------------------------
        'RDS
        ''' <summary>
        ''' The Table at the RDS layer of the Model, and as represented by this Entity.
        ''' </summary>
        ''' <remarks></remarks>
        Public WithEvents RDSTable As RDS.Table

        Public Shadows Property X As Integer Implements FBM.iPageObject.X
            Get
                Return Me._X
            End Get
            Set(value As Integer)
                Me._X = value
            End Set
        End Property

        Public Shadows Property Y As Integer Implements FBM.iPageObject.Y
            Get
                Return Me._Y
            End Get
            Set(value As Integer)
                Me._Y = value
            End Set
        End Property

        Public Sub New()

            Me.ConceptType = pcenumConceptType.Entity

        End Sub

        Public Sub New(ByRef arPage As FBM.Page, ByVal asEntityName As String)

            Me.ConceptType = pcenumConceptType.Entity
            Me.Page = arPage
            Me.Model = arPage.Model
            Me.FactData.Model = arPage.Model
            Me.FactData.Name = asEntityName
            Me.FactData.setData(asEntityName, pcenumConceptType.Value, False) 'Was Me.FactData.Data = asEntityName
            Me.Name = asEntityName

            Dim lrDictionaryEntry As New FBM.DictionaryEntry(Me.Model, asEntityName, pcenumConceptType.Value)

            Me.Concept = Me.Model.AddModelDictionaryEntry(lrDictionaryEntry).Concept
            Me.FactData.Concept = Me.Concept

        End Sub

        Public Sub New(ByRef arPage As FBM.Page, ByRef arTable As RDS.Table)

            Me.ConceptType = pcenumConceptType.Entity
            Me.Page = arPage
            Me.Model = arPage.Model
            Me.FactData.Model = arPage.Model
            Me.Name = arTable.Name

            Me.RDSTable = arTable

            Dim lrDictionaryEntry As New FBM.DictionaryEntry(Me.Model, arTable.Name, pcenumConceptType.Value)

            Me.Concept = Me.Model.AddModelDictionaryEntry(lrDictionaryEntry).Concept
            Me.FactData.Concept = Me.Concept

        End Sub

        Public Sub New(ByRef arPage As FBM.Page, ByRef arRoleInstance As FBM.RoleInstance, ByRef ar_concept As FBM.Concept)
            '---------------------------------------------------
            'NB Arguments are by Ref, because need to point to 
            '  actual objects on a Page.
            '---------------------------------------------------
            Me.ConceptType = pcenumConceptType.Entity
            Me.Page = arPage
            Me.Role = arRoleInstance
            Me.Concept = ar_concept

            '------------------------------------
            'link the RoleData back to the Model
            '------------------------------------
            Dim lrRole As FBM.Role = arRoleInstance.Role
            Dim lrRole_data As New FBM.FactData(arRoleInstance.Role, ar_concept)
            lrRole_data = lrRole.Data.Find(AddressOf lrRole_data.Equals)
            Me.FactData = lrRole_data

        End Sub

        Public Sub New(ByRef arPage As FBM.Page, ByRef arRoleInstance As FBM.RoleInstance, ByRef ar_concept As FBM.Concept, ByVal aiX As Integer, ByVal aiY As Integer)
            '---------------------------------------------------
            'NB Arguments are by Ref, because need to point to 
            '  actual objects on a Page.
            '---------------------------------------------------            
            Call Me.New(arPage, arRoleInstance, ar_concept)
            Me.ConceptType = pcenumConceptType.Entity
            Me.X = aiX
            Me.Y = aiY

        End Sub

        Public Function ClonePageObject() As FBM.PageObject

            Dim lrPageObject As New FBM.PageObject

            lrPageObject.Name = Me.Name
            lrPageObject.Shape = New ShapeNode()
            lrPageObject.X = Me.X
            lrPageObject.Y = Me.Y

            Return lrPageObject

        End Function


        Public Shadows Function Equals(ByVal other As FBM.EntityTypeInstance) As Boolean Implements System.IEquatable(Of FBM.EntityTypeInstance).Equals


        End Function


        Public Sub DisplayAndAssociate()

            Try
                Dim loDroppedNode As ERD.TableNode = Nothing
                Dim StringSize As New SizeF

                StringSize = Me.Page.Diagram.MeasureString(Trim(Me.Name), Me.Page.Diagram.Font, 1000, System.Drawing.StringFormat.GenericDefault)

                '=====================================================================
                '----------------------------------------------
                'Create a TableNode on the Page for the Entity
                '----------------------------------------------
                'loDroppedNode = Me.Page.Diagram.Factory.CreateTableNode(Me.X, Me.Y, 20, 2, 1, 0)
                loDroppedNode = New ERD.TableNode
                Me.Page.Diagram.Nodes.Add(loDroppedNode)
                loDroppedNode.Resize(20, 2)
                loDroppedNode.ColumnCount = 1
                loDroppedNode.RowCount = 0
                Me.Page.Diagram.Nodes.Add(loDroppedNode)
                loDroppedNode.Move(Me.X, Me.Y)
                loDroppedNode.HandlesStyle = HandlesStyle.Invisible
                loDroppedNode.Pen.Width = 0.5

                If Me.Page.Language = pcenumLanguage.PropertyGraphSchema Then
                    If Me.NodeType = pcenumPGSEntityType.Node Then
                        loDroppedNode.Pen.Color = Color.Brown
                    Else
                        loDroppedNode.Pen.Color = Color.DarkGray
                    End If
                Else
                    loDroppedNode.Pen.Color = Color.Black
                End If

                loDroppedNode.Brush = New SolidBrush(Color.White)
                loDroppedNode.ShadowColor = Color.LightGray
                loDroppedNode.EnableStyledText = True
                loDroppedNode.CellFrameStyle = CellFrameStyle.None
                loDroppedNode.ConnectionStyle = TableConnectionStyle.Both
                loDroppedNode.Expandable = False
                loDroppedNode.Obstacle = True
                loDroppedNode.Style = TableStyle.RoundedRectangle
                loDroppedNode.Resize(StringSize.Width + 5, 15)
                loDroppedNode.AllowIncomingLinks = True
                loDroppedNode.AllowOutgoingLinks = True
                loDroppedNode.Caption = "<B>" & " " & Me.FactDataInstance.Data & " "
                loDroppedNode.Tag = Me
                loDroppedNode.ResizeToFitText(False)
                Me.FactDataInstance.TableShape = loDroppedNode
                Me.TableShape = loDroppedNode

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Public Function CreateUniqueAttributeName(ByVal asAttributeName As String, ByVal aiCounter As Integer) As String

            Dim lsAttributeName As String = ""

            If aiCounter = 0 Then
                lsAttributeName = asAttributeName
            Else
                lsAttributeName = asAttributeName & aiCounter.ToString
            End If

            Dim lrAttribute As New ERD.Attribute(lsAttributeName)

            If IsSomething(Me.Attribute.Find(AddressOf lrAttribute.EqualsByName)) Then
                lsAttributeName = Me.CreateUniqueAttributeName(asAttributeName, aiCounter + 1)
            End If

            Return lsAttributeName

        End Function

        Public Sub ResetAttributeCellColours()

            Dim lrCell As MindFusion.Diagramming.TableNode.Cell
            Dim liInd As Integer = 0

            For liInd = 0 To Me.TableShape.RowCount - 1
                lrCell = Me.TableShape.Item(0, liInd)
                lrCell.Brush = New MindFusion.Drawing.SolidBrush(Color.White)
                lrCell.TextColor = Color.Black
            Next

        End Sub

        Public Overrides Function SetName(ByVal asNewName As String, Optional ByVal abBroadcastInterfaceEvent As Boolean = True) As Boolean

            '----------------------------------------------------------------------------------------------
            'Modify the FactData referenced by the FactData instance.
            '  The FactDataInstance event handler (FactData.Updated) manages(the) changes 
            '  to Me.Id, Me.Name, Me.Data etc
            '----------------------------------------------------------------------------------------------            
            Try
                Me.FactData.Name = asNewName
                Me.FactData.Data = asNewName
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

        Public Sub RefreshShape(Optional ByVal aoChangedPropertyItem As PropertyValueChangedEventArgs = Nothing,
                                Optional ByVal asSelectedGridItemLabel As String = "")

            Try

                If IsSomething(aoChangedPropertyItem) Then
                    Select Case aoChangedPropertyItem.ChangedItem.PropertyDescriptor.Name
                        Case Is = "Name"
                            '-----------------------------------------------------------------------------
                            'Update the Model.
                            '  GUI is updated via the event triggered, (Me)FactData.ConceptSymbolUpdated
                            '-----------------------------------------------------------------------------
                            'Me.FactData.Data = Me.Name
                            Call Me.RDSTable.FBMModelElement.setName(Me.Name)
                    End Select
                End If

                Me.Attribute.Sort(AddressOf ERD.Attribute.ComparerOrdinalPosition)

                Dim liInd As Integer
                Dim larSupertypeTable = Me.RDSTable.getSupertypeTables
                If larSupertypeTable.Count > 0 Then
                    larSupertypeTable.Reverse()
                    larSupertypeTable.Add(Me.RDSTable)
                    liInd = 0
                    For Each lrSupertypeTable In larSupertypeTable
                        For Each lrAttrubute In Me.Attribute.FindAll(Function(x) x.Column.Role.JoinedORMObject.Id = lrSupertypeTable.Name)
                            Me.Attribute.Remove(lrAttrubute)
                            Me.Attribute.Insert(liInd, lrAttrubute)
                            liInd += 1
                        Next
                    Next
                End If


                Me.TableShape.RowCount = Me.Attribute.Count

                'CodeSafe: Remove Attributes where Column is Nothing
                Me.Attribute.RemoveAll(Function(x) x.Column Is Nothing)

                liInd = 0
                For Each lrERAttribute In Me.Attribute
                    lrERAttribute.Cell = Me.TableShape.Item(0, liInd) 'lrERAttribute.Column.OrdinalPosition - 1)
                    Me.TableShape.Item(0, liInd).Tag = lrERAttribute 'lrERAttribute.Column.OrdinalPosition - 1
                    Call lrERAttribute.RefreshShape()

                    Me.TableShape.ResizeToFitText(False)
                    liInd += 1
                Next

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Private Sub FactData_ConceptSwitched(ByRef arConcept As FBM.Concept) Handles FactData.ConceptSwitched

            Me.Concept = arConcept
            Call Me.UpdateGUIFromModel()

        End Sub

        Private Sub UpdateFromModel() Handles FactData.ConceptSymbolUpdated

            Try
                Call Me.UpdateGUIFromModel()

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Friend Sub UpdateGUIFromModel()

            '---------------------------------------------------------------------
            'Linked by Delegate in New to the 'update' event of the ModelObject 
            '  referenced by Objects of this Class
            '---------------------------------------------------------------------
            Try

                If IsSomething(Me.Page.Diagram) Then
                    '------------------
                    'Diagram is set.
                    '------------------
                    If IsSomething(Me.TableShape) Then
                        If Me.TableShape.Caption <> "" Then
                            '---------------------------------------------------------------------------------
                            'Is the type of EntityTypeInstance that 
                            '  shows the EntityTypeName within the
                            '  ShapeNode itself and not a separate
                            '  ShapeNode attached to it (e.g. An Actor EntityTypeInstance has two ShapeNodes, 
                            ' 1 for the stickfigure, the other for the name of the Actor.
                            '---------------------------------------------------------------------------------
                            Me.TableShape.Caption = "<B>" & " " & Trim(Me.FactData.Data) & " "
                            Call Me.EnableSaveButton()
                            Me.Page.Diagram.Invalidate()
                        End If
                    End If
                End If
            Catch lo_err As Exception
                MsgBox("t_Entity.update_from_model: " & lo_err.Message) '& "FactTypeId: " & Me.role.FactType.FactTypeId & ", ValueSymbol:" & Me.concept.Symbol & ", PageId:") ' & Me.Page.PageId)
            End Try

        End Sub

        Public Overrides Function getCorrespondingRDSTable() As RDS.Table
            Return Me.RDSTable
        End Function

        Public Sub GetAttributesFromRDSColumns(Optional ByVal abAddToPage As Boolean = False)

            Try
                If Me.Attribute.Count > 0 Then
                    Throw New Exception("Method called for Entity, '" & Me.Name & "', that already has Attributes loaded.")
                ElseIf Me.Page Is Nothing Then
                    Throw New Exception("Method called for Entity, '" & Me.Name & "', that has no Page.")
                End If

                Dim lrERAttribute As ERD.Attribute

                For Each lrColumn In Me.RDSTable.Column

                    lrERAttribute = New ERD.Attribute
                    lrERAttribute.Column = lrColumn
                    lrERAttribute.Model = Me.Page.Model
                    lrERAttribute.Id = lrColumn.Id
                    lrERAttribute.Entity = Me
                    lrERAttribute.AttributeName = lrColumn.Name
                    lrERAttribute.ResponsibleRole = lrColumn.Role
                    lrERAttribute.ActiveRole = lrColumn.ActiveRole
                    lrERAttribute.ResponsibleFactType = lrERAttribute.ResponsibleRole.FactType
                    lrERAttribute.Mandatory = lrColumn.IsMandatory
                    'lrERAttribute.OrdinalPosition = lrColumn.OrdinalPosition
                    lrERAttribute.PartOfPrimaryKey = lrColumn.isPartOfPrimaryKey
                    lrERAttribute.Page = Me.Page

                    lrERAttribute.Column = lrColumn

                    Me.Attribute.Add(lrERAttribute)

                    If abAddToPage Then
                        Me.Page.ERDiagram.Attribute.Add(lrERAttribute)
                    End If
                Next

                Dim liInd As Integer
                Dim larSupertypeTable = Me.RDSTable.getSupertypeTables
                If larSupertypeTable.Count > 0 Then
                    larSupertypeTable.Reverse()
                    larSupertypeTable.Add(Me.RDSTable)
                    liInd = 0
                    For Each lrSupertypeTable In larSupertypeTable
                        For Each lrAttrubute In Me.Attribute.FindAll(Function(x) x.Column.Role.JoinedORMObject.Id = lrSupertypeTable.Name).OrderBy(Function(x) x.OrdinalPosition)
                            Me.Attribute.Remove(lrAttrubute)
                            Me.Attribute.Insert(liInd, lrAttrubute)
                            liInd += 1
                        Next
                    Next
                End If

            Catch ex As Exception
                Debugger.Break()
            End Try

        End Sub

        Public Sub MouseDown() Implements FBM.iPageObject.MouseDown

        End Sub

        Public Sub MouseMove() Implements FBM.iPageObject.MouseMove

        End Sub

        Public Sub MouseUp() Implements FBM.iPageObject.MouseUp

        End Sub

        Public Sub Moved() Implements FBM.iPageObject.Moved

        End Sub

        Public Sub NodeDeleting() Implements FBM.iPageObject.NodeDeleting

        End Sub

        Public Sub NodeModified() Implements FBM.iPageObject.NodeModified

        End Sub

        Public Sub NodeSelected() Implements FBM.iPageObject.NodeSelected

            Me.TableShape.Pen.Color = Color.Blue

        End Sub

        Public Sub CellClicked() Implements FBM.iTableNodePageObject.CellClicked

        End Sub

        Public Sub NodeDeselected() Implements FBM.iTableNodePageObject.NodeDeselected

            Me.TableShape.Pen.Color = Color.Black

        End Sub


        Public Sub NodeDoubleClicked() Implements FBM.iTableNodePageObject.NodeDoubleClicked

        End Sub

        Public Sub SetAppropriateColour() Implements FBM.iPageObject.SetAppropriateColour

        End Sub

        Public Sub RepellNeighbouringPageObjects(ByVal aiDepth As Integer) Implements FBM.iPageObject.RepellNeighbouringPageObjects

        End Sub

        Public Sub Move(ByVal aiNewX As Integer, ByVal aiNewY As Integer, ByVal abBroadcastInterfaceEvent As Boolean) Implements FBM.iPageObject.Move

            Me.X = aiNewX
            Me.Y = aiNewY

            Me.FactDataInstance.X = aiNewX
            Me.FactDataInstance.Y = aiNewY

            Me.FactDataInstance.Fact.FactType.isDirty = True
            Me.FactDataInstance.Fact.isDirty = True
            Me.FactDataInstance.isDirty = True

        End Sub

        Private Sub RDSTable_ColumnAdded(ByRef arColumn As RDS.Column) Handles RDSTable.ColumnAdded

            Try
                If arColumn.Role.JoinedORMObject.Id = Me.Name Then
                    Call Me.Page.AddAttributeToEntity(arColumn)
                End If

                Dim lrERAttribute As ERD.Attribute
                Dim lsSQLQuery As String = ""
                Dim lrRecordset As ORMQL.Recordset
                Dim lrFactDataInstance As FBM.FactDataInstance

                lsSQLQuery = "SELECT *"
                lsSQLQuery &= " FROM CoreERDAttribute"
                'lsSQLQuery &= " ON PAGE '" & Me.Page.Name & "'"
                lsSQLQuery &= " WHERE ModelObject = '" & Me.RDSTable.Name & "'"
                lsSQLQuery &= "   AND Attribute = '" & arColumn.Id & "'"

                lrRecordset = Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                lrFactDataInstance = lrRecordset("Attribute").CloneInstance(Me.Page)

                lrERAttribute = lrFactDataInstance.CloneAttribute(Me.Page)
                lrERAttribute.Id = arColumn.Id
                lrERAttribute.Column = arColumn
                '---------------------------------------------------
                'Find the ER Entity to add the Attribute to.
                '---------------------------------------------------
                lrERAttribute.Entity = Me

                '-------------------------------
                'Get the Name of the Attribute
                '-------------------------------
                lrERAttribute.AttributeName = arColumn.Name

                '----------------------------------------------------
                'Get the Role and FactType of the Attribute
                lrERAttribute.ResponsibleRole = arColumn.Role
                lrERAttribute.ResponsibleFactType = lrERAttribute.ResponsibleRole.FactType

                lrERAttribute.ActiveRole = arColumn.ActiveRole

                '-------------------------------------------------
                'Check to see whether the Attribute is Mandatory
                '-------------------------------------------------
                lrERAttribute.Mandatory = arColumn.IsMandatory

                '--------------------------------------------------------
                'Check to see whether the Entity has a PrimaryKey
                '--------------------------------------------------------
                lrERAttribute.PartOfPrimaryKey = arColumn.isPartOfPrimaryKey 'ContributesToPrimaryKey 'Should also be reflected in the Index for the Column etc.

                Me.Attribute.Add(lrERAttribute)
                Me.Page.ERDiagram.Attribute.Add(lrERAttribute)

                '-----------------------------------------
                'Refresh the table shape.
                If Me.TableShape IsNot Nothing Then 'CodeSafe
                    Me.TableShape.RowCount += 1
                    lrERAttribute.Cell = Me.TableShape.Item(0, Me.TableShape.RowCount - 1)
                    Me.TableShape.Item(0, Me.TableShape.RowCount - 1).Tag = lrERAttribute
                    Call lrERAttribute.RefreshShape()

                    Me.TableShape.ResizeToFitText(False)
                End If

                Call Me.RefreshShape()

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Private Sub RDSTable_ColumnRemoved(ByVal arColumn As RDS.Column) Handles RDSTable.ColumnRemoved

            Try
                Dim lrAttribute As ERD.Attribute

                lrAttribute = Me.Attribute.Find(Function(x) x.Id = arColumn.Id)

                Me.Attribute.Remove(lrAttribute)
                Me.Page.ERDiagram.Attribute.RemoveAll(Function(x) x.Id = lrAttribute.Id)

                If Me.TableShape IsNot Nothing Then
                    Me.TableShape.DeleteRow(lrAttribute.OrdinalPosition - 1)
                End If

                Call Me.RefreshShape()

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Private Sub RDSTable_IndexAdded(ByRef arIndex As RDS.Index) Handles RDSTable.IndexAdded

            For Each lrColumn In arIndex.Column

                Dim larAttribute = From Attribute In Me.Attribute
                                   Where Attribute.Column.Id = lrColumn.Id
                                   Select Attribute

                For Each lrAttribute In larAttribute
                    If arIndex.IsPrimaryKey Then
                        lrAttribute.PartOfPrimaryKey = True
                        Call lrAttribute.RefreshShape()
                    End If
                Next
            Next

        End Sub

        Private Sub RDSTable_IndexRemoved(ByRef arIndex As RDS.Index) Handles RDSTable.IndexRemoved

            For Each lrColumn In arIndex.Column

                Dim larAttribute = From Attribute In Me.Attribute
                                   Where Attribute.Column.Id = lrColumn.Id
                                   Select Attribute

                For Each lrAttribute In larAttribute
                    If arIndex.IsPrimaryKey Then
                        lrAttribute.PartOfPrimaryKey = False
                        Call lrAttribute.RefreshShape()
                    End If
                Next
            Next

        End Sub

        Private Sub RDSTable_NameChanged(asNewName As String) Handles RDSTable.NameChanged

            Me.TableShape.Caption = asNewName
            Call Me.Page.Diagram.Invalidate()

        End Sub

        Private Sub RDSTable_SubtypeRelationshipAdded() Handles RDSTable.SubtypeRelationshipAdded

            If Me.Page IsNot Nothing And Me.Page.Diagram IsNot Nothing Then

                For Each lrEntity In Me.Page.ERDiagram.Entity.FindAll(Function(x) CType(x, ERD.Entity).RDSTable.getSubtypeTables.Contains(Me.RDSTable))
                    Dim lo_link As New DiagramLink(Me.Page.Diagram, Me.TableShape, CType(lrEntity, ERD.Entity).TableShape)
                    lo_link.HeadShape = ArrowHead.Arrow
                    lo_link.Pen.Color = Color.Gray
                    lo_link.Locked = True
                    Me.Page.Diagram.Links.Add(lo_link)
                    Me.TableShape.OutgoingLinks.Add(lo_link)
                    Call lo_link.Route()
                Next
                Me.Page.Diagram.RouteAllLinks()
            End If

        End Sub

        Private Sub RDSTable_SubtypeRelationshipRemoved() Handles RDSTable.SubtypeRelationshipRemoved

            Dim larLink = New List(Of MindFusion.Diagramming.DiagramLink)
            For Each loLink In Me.TableShape.OutgoingLinks
                If loLink.GetType Is GetType(MindFusion.Diagramming.DiagramLink) Then
                    larLink.Add(loLink)
                End If
            Next
            For Each lrLink In larLink
                Call Me.Page.Diagram.Links.Remove(lrLink)
            Next

            For Each lrEntity In Me.Page.ERDiagram.Entity.FindAll(Function(x) CType(x, ERD.Entity).RDSTable.getSubtypeTables.Contains(Me.RDSTable))
                Dim lo_link As New DiagramLink(Me.Page.Diagram, Me.TableShape, CType(lrEntity, ERD.Entity).TableShape)
                lo_link.HeadShape = ArrowHead.Arrow
                lo_link.Pen.Color = Color.Gray
                lo_link.Locked = True
                Me.Page.Diagram.Links.Add(lo_link)
            Next

        End Sub

        Public Sub EnableSaveButton() Implements iPageObject.EnableSaveButton
            If Me.Page IsNot Nothing Then
                If Me.Page.Form IsNot Nothing Then
                    Call Me.EnableSaveButton()
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