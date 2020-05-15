Imports System.Drawing
Imports System.Drawing.Drawing2D

Imports MindFusion.Diagramming
Imports MindFusion.Diagramming.WinForms
Imports MindFusion.Drawing
Imports MindFusion.Diagramming.Layout

Public Class frmUMLClassDiagram

    Public zrPage As FBM.Page = Nothing
    Public zoTreeNode As TreeNode = Nothing
    Public UMLClassDiagram As New UML.UMLSuperstructure

    Private zo_containernode As ContainerNode

    Private morph_vector As tmorphvector
    Private morph_shape As New ShapeNode


    Private Sub frm_UseCaseModel_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing

        '-------------------------------------------
        'Process the page associated with the form.
        '-------------------------------------------
        If IsSomething(Me.zrPage) Then
            If Me.zrPage.IsDirty Then
                Select Case MsgBox("Changes have been made to the Page. Would you like to save those changes?", MsgBoxStyle.YesNoCancel)
                    Case Is = MsgBoxResult.Yes
                        Me.zrPage.Save()
                    Case Is = MsgBoxResult.Cancel
                        e.Cancel = True
                        Exit Sub
                End Select
            End If
            Me.zrPage.Form = Nothing
            Me.zrPage.ReferencedForm = Nothing
        End If

        '----------------------------------------------
        'Reset the PageLoaded flag on the Page so
        '  that the User can open the Page again
        '  if they want.
        '----------------------------------------------        
        Me.zrPage.FormLoaded = False

        prApplication.WorkingModel = Nothing
        prApplication.WorkingPage = Nothing

        '------------------------------------------------
        'If the 'Properties' window is open, reset the
        '  SelectedObject
        '------------------------------------------------
        If Not IsNothing(frmMain.zfrm_properties) Then
            frmMain.zfrm_properties.PropertyGrid.SelectedObject = Nothing
        End If

        Me.Hide()

        frmMain.ToolStripButton_Save.Enabled = False


    End Sub


    Private Sub frm_UseCaseModel_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Call Me.SetupForm()

    End Sub

    Sub SetupForm()


    End Sub


    Private Sub frm_UseCaseModel_Activated(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Activated

        Call SetToolbox()


    End Sub

    Private Sub frmUMLClassDiagram_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Enter

        If IsSomething(Me.zoTreeNode) Then
            If IsSomething(frmMain.zfrmModelExplorer) Then
                frmMain.zfrmModelExplorer.TreeView.SelectedNode = Me.zoTreeNode
            End If

            If IsSomething(frmMain.zfrm_model_dictionary) Then
                Call frmMain.zfrm_model_dictionary.LoadToolboxModelDictionary(Me.zrPage.Language)
            End If
        End If

        If IsSomething(frmMain.zfrm_KL_theorem_writer) Then
            frmMain.zfrm_KL_theorem_writer.zrPage = Me.zrPage
        End If

        Call SetToolbox()

    End Sub


    Private Sub DiagramView_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DiagramView.Click

        Call SetToolbox()

        'Me.DiagramView.Behavior = Behavior.Modify
        Me.Diagram.Invalidate()
        Me.zrPage.IsDirty = True

    End Sub

    Sub SetToolbox()

        Dim lsl_shape_library As ShapeLibrary = ShapeLibrary.LoadFrom(My.Settings.ShapeLibraryClassDiagram)

        Dim child As New frmToolbox

        If prApplication.RightToolboxForms.FindAll(AddressOf child.EqualsByName).Count > 0 Then

            'End If
            child = prApplication.RightToolboxForms.Find(AddressOf child.EqualsByName)
            'If IsSomething(frmMain.zfrm_toolbox) Then
            'frmMain.zfrm_toolbox
            child.ShapeListBox.Shapes = lsl_shape_library.Shapes

            Dim lo_shape As Shape
            'frmMain.zfrm_toolbox.
            For Each lo_shape In child.ShapeListBox.Shapes
                Select Case lo_shape.DisplayName
                    Case Is = "Actor"
                        lo_shape.Image = My.Resources.actor
                End Select
            Next
        End If

    End Sub

    Private Sub DiagramView_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles DiagramView.DoubleClick

        '---------------------------------------
        'Only allow 'InPlaceEdit' on Processes
        '---------------------------------------
        If Me.Diagram.Selection.Items.Count = 1 Then
            If Me.Diagram.Selection.Items(0).Tag.ConceptType = pcenumConceptType.Process Then
                Me.DiagramView.AllowInplaceEdit = True
            Else
                Me.DiagramView.AllowInplaceEdit = False
            End If
        Else
            Me.DiagramView.AllowInplaceEdit = False
        End If

    End Sub

    Private Sub DiagramView_DragDrop(ByVal sender As Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles DiagramView.DragDrop

        Dim liInd As Integer = 0
        Dim ls_message As String = ""
        Dim loNode As New ShapeNode


        If e.Data.GetDataPresent(tShapeNodeDragItem.DraggedItemObjectType) Then

            Dim lnode_dragged_node As tShapeNodeDragItem = e.Data.GetData(tShapeNodeDragItem.DraggedItemObjectType)

            If lnode_dragged_node.Index >= 0 Then
                If lnode_dragged_node.Index < frmMain.zfrm_toolbox.ShapeListBox.ShapeCount Then
                    Dim p As Point = Me.DiagramView.PointToClient(New Point(e.X, e.Y))
                    Dim pt As PointF = Me.DiagramView.ClientToDoc(New Point(p.X, p.Y))

                    Select Case frmMain.zfrm_toolbox.ShapeListBox.Shapes(lnode_dragged_node.Index).Id
                        Case Is = "Actor"
                        Case Is = "Process"
                    End Select
                End If
            End If
        End If


    End Sub

    Sub LoadUMLClassDiagramPage(ByRef arPage As FBM.Page, ByRef aoTreeNode As TreeNode)

        Dim loDroppedNode As TableNode = Nothing
        Dim lrFactTypeInstance_pt As PointF = Nothing
        Dim lrFactInstance As New FBM.FactInstance
        Dim lrAttributeFactInstance As New FBM.FactInstance
        Dim lsIdentityIdentifier As String = "" '<<i>>
        Dim lsSQLQuery As String = ""
        Dim lrORMQlREcordset As New ORMQL.Recordset

        frmMain.Cursor = Cursors.WaitCursor

        '-------------------------------------------------------
        'Set the Caption/Title of the Page to the PageName
        '-------------------------------------------------------
        Me.zrPage = arPage
        Me.TabText = arPage.Name

        Me.zoTreeNode = aoTreeNode

        '------------------------------------------------------------------------------
        'Display the UML Class Diagram by logically associating Shape objects
        '   with the corresponding 'object' within the ORMModelPage object
        '------------------------------------------------------------------------------
        Dim lrFactTypeInstance As New FBM.FactTypeInstance
        lrFactTypeInstance = arPage.FactTypeInstance.Find(Function(p) p.Id = pcenumCMMLRelations.ElementHasElementType.ToString)
        If IsSomething(lrFactTypeInstance) Then
            '------------------------------------------------------------------
            'At least one Actor/Process relation has already been established
            '------------------------------------------------------------------
            Me.UMLClassDiagram.ElementHasElementType = lrFactTypeInstance
        Else
        End If


        lrFactTypeInstance = New FBM.FactTypeInstance
        lrFactTypeInstance = arPage.FactTypeInstance.Find(Function(p) p.Id = pcenumCMMLRelations.ClassHasAttribute.ToString)
        Me.UMLClassDiagram.ClassHasAttribute = lrFactTypeInstance

        lrFactTypeInstance = New FBM.FactTypeInstance
        lrFactTypeInstance = arPage.FactTypeInstance.Find(Function(p) p.Id = pcenumCMMLRelations.Generalisation.ToString)
        Me.UMLClassDiagram.Generalisation = lrFactTypeInstance

        lrFactTypeInstance = New FBM.FactTypeInstance
        lrFactTypeInstance = arPage.FactTypeInstance.Find(Function(p) p.Id = pcenumCMMLRelations.EnumerationHasEnumerationName.ToString)
        Me.UMLClassDiagram.EnumerationHasEnumerationName = lrFactTypeInstance

        lrFactTypeInstance = New FBM.FactTypeInstance
        lrFactTypeInstance = arPage.FactTypeInstance.Find(Function(p) p.Id = pcenumCMMLRelations.EnumerationHasEnumerationLiteral.ToString)
        Me.UMLClassDiagram.EnumerationHasEnumerationLiteral = lrFactTypeInstance

        Dim lrFactDataInstance As New FBM.FactDataInstance(Me.zrPage)
        Dim lrFact As FBM.Fact

        '--------------------
        'Load the Classes.
        '--------------------
        Richmond.WriteToStatusBar("Loading Classes")
        If IsSomething(Me.UMLClassDiagram.ElementHasElementType) Then
            For Each lrFactInstance In Me.UMLClassDiagram.ElementHasElementType.Fact
                '--------------------------------------------------------------------------
                'Qualify the Fact is for 'Element's that have an 'ElementType' of 'Class'
                '--------------------------------------------------------------------------
                Dim lrClass As New UML.Class
                If lrFactInstance.Data(1).Data = "Class" Then
                    lrFactDataInstance = lrFactInstance.Data(0)


                    lrClass = lrFactDataInstance.CloneClass(arPage)

                    '----------------------------------------------
                    'Create a TableNode on the Page for the Class
                    '----------------------------------------------
                    loDroppedNode = Diagram.Factory.CreateTableNode(lrClass.X, lrClass.Y, 20, 2, 1, 0)
                    loDroppedNode.HandlesStyle = HandlesStyle.InvisibleMove
                    loDroppedNode.Brush = New MindFusion.Drawing.SolidBrush(Color.White)
                    loDroppedNode.CellFrameStyle = CellFrameStyle.None
                    loDroppedNode.Resize(20, 15)
                    loDroppedNode.ShadowColor = Color.White
                    loDroppedNode.AllowIncomingLinks = True
                    loDroppedNode.AllowOutgoingLinks = True
                    loDroppedNode.Caption = lrFactDataInstance.Data
                    loDroppedNode.Tag = New ERD.Entity
                    loDroppedNode.Tag = lrClass
                    loDroppedNode.ConnectionStyle = TableConnectionStyle.Table
                    loDroppedNode.Expandable = False
                    loDroppedNode.Pen = New MindFusion.Drawing.Pen(Color.Black, 0.4)
                    lrClass.TableShape = loDroppedNode
                    lrClass.FactDataInstance.TableShape = loDroppedNode
                    lrClass.TableShape.Obstacle = True

                    Me.UMLClassDiagram.Class.Add(lrClass)

                    '==================================
                    'Load the attributes of the Class
                    '==================================
                    lsSQLQuery = "SELECT *"
                    lsSQLQuery &= " FROM " & pcenumCMMLRelations.ClassHasAttribute.ToString
                    lsSQLQuery &= " WHERE Class = '" & lrFactDataInstance.Data & "'"

                    lrORMQlREcordset = Me.zrPage.Model.processORMQLStatement(lsSQLQuery)

                    If lrORMQlREcordset.Facts.Count > 0 Then

                        For Each lrFact In lrORMQlREcordset.Facts.ToArray

                            Dim lrERAttribute As New CMML.tActor
                            Dim lsPropertyName As String = ""
                            Dim lsAttributeName As String = ""

                            lsAttributeName = lrFact.GetFactDataByRoleName("Attribute").Data

                            lsSQLQuery = "SELECT *"
                            lsSQLQuery &= " FROM " & pcenumCMMLRelations.PropertyHasPropertyName.ToString
                            lsSQLQuery &= " WHERE Property = '" & lsAttributeName & "'"

                            Dim lrRecordset2 As New ORMQL.Recordset
                            lrRecordset2 = Me.zrPage.Model.processORMQLStatement(lsSQLQuery)

                            lsPropertyName = lrRecordset2.Facts(0).GetFactDataByRoleName("PropertyName").Data

                            Dim lrAttribute As New UML.Attribute(lsAttributeName)

                            Richmond.WriteToStatusBar("Loading Attribute: " & lsAttributeName)

                            lsIdentityIdentifier = ""

                            'Dim lrAttributeDictionary As New Dictionary(Of String, String)
                            'For Each lrFactDataInstance In lrFactInstance.Data
                            '    lrAttributeDictionary.Add(lrFactDataInstance.Role.Id, lrFactDataInstance.Data)
                            'Next


                            lrERAttribute = lrFactDataInstance.CloneActor(arPage)

                            '-----------------------------------------------------------------------
                            'Check if the attribute forms part of the Identification for the Class
                            '-----------------------------------------------------------------------
                            Dim lsIdentityConstraints As String = ""

                            lsSQLQuery = "SELECT *"
                            lsSQLQuery &= " FROM " & pcenumCMMLRelations.IdentificationMakesUseOfProperty.ToString
                            lsSQLQuery &= " WHERE Property = '" & lsAttributeName & "'"

                            lrORMQlREcordset = Me.zrPage.Model.processORMQLStatement(lsSQLQuery)

                            If lrORMQlREcordset.Facts.Count > 0 Then

                                Richmond.WriteToStatusBar("Setting Identification for Attribute: " & lsAttributeName)
                                'Dim lrIdentificationFactInstance As New FBM.FactInstance

                                Dim liInd As Integer = 0

                                Dim lrIdentificationFact As FBM.Fact
                                For Each lrIdentificationFact In lrORMQlREcordset.Facts
                                    If lrIdentificationFact.GetFactDataByRoleName("Property").Data = lsAttributeName Then
                                        lsIdentityIdentifier = "<<i>>"

                                        lrAttribute.IsIdentityIdentifier = True

                                        If liInd = 0 Then
                                            lsIdentityConstraints = lrIdentificationFact.GetFactDataByRoleName("Identification").Data
                                        Else
                                            lsIdentityConstraints &= "," & lrIdentificationFact.GetFactDataByRoleName("Identification").Data
                                        End If

                                        lrAttribute.PreferredIdentityIdentifier.Add(lrIdentificationFact.GetFactDataByRoleName("Identification").Data)

                                        liInd += 1
                                    End If
                                Next
                            End If

                            '================================================================================                              
                            '-----------------------------------
                            'Get the DataType of the Attribute
                            '-----------------------------------
                            Richmond.WriteToStatusBar("Loading DataType for Attribute: " & lsAttributeName)
                            Dim lsDataType As String = ""


                            lsSQLQuery = "SELECT Type "
                            lsSQLQuery &= " FROM " & pcenumCMMLRelations.PropertyHasType.ToString
                            lsSQLQuery &= " WHERE Property = '" & lsAttributeName & "'"
                            lrORMQlREcordset = Me.zrPage.Model.processORMQLStatement(lsSQLQuery)

                            lsDataType = lrORMQlREcordset.Facts(0).GetFactDataByRoleName("Type").Data
                            lrAttribute.DataType = lsDataType
                            '================================================================================

                            '---------------------------------------
                            'Get the Multiplicity of the Attribute
                            '---------------------------------------
                            Richmond.WriteToStatusBar("Loading the Multiplicity for Attribute: " & lsAttributeName)
                            Dim lsMultiplicityId As String = ""
                            Dim lsMultiplicity As String = ""
                            Dim lsMultiplicityUpperBound As String = ""
                            Dim lsMultiplicityLowerBound As String = ""

                            lsSQLQuery = "SELECT Multiplicty"
                            lsSQLQuery &= " FROM " & pcenumCMMLRelations.PropertyHasMultiplicity.ToString
                            lsSQLQuery &= " WHERE Property = '" & lsAttributeName & "'"

                            lrORMQlREcordset = Me.zrPage.Model.processORMQLStatement(lsSQLQuery)

                            lsMultiplicityId = lrORMQlREcordset.Facts(0).GetFactDataByRoleName("Multiplicity").Data

                            If lrORMQlREcordset.Facts.Count > 0 Then

                                '------------------------------------------------
                                'Check to see if the Multiplicity is Unbounded.
                                '------------------------------------------------
                                lsSQLQuery = "SELECT *"
                                lsSQLQuery &= " FROM " & pcenumCMMLRelations.MultiplicityIsUnbounded.ToString
                                lsSQLQuery &= " WHERE IsUnbounded = '" & lsMultiplicityId & "'"

                                lrORMQlREcordset = Me.zrPage.Model.processORMQLStatement(lsSQLQuery)

                                If lrORMQlREcordset.Facts.Count > 0 Then

                                    lsMultiplicityLowerBound = "0"
                                    lsMultiplicityUpperBound = "*"

                                    lsSQLQuery = "SELECT Bound"
                                    lsSQLQuery &= " FROM " & pcenumCMMLRelations.MultiplicityHasLowerBound.ToString
                                    lsSQLQuery &= " WHERE Multiplicity = '" & lsMultiplicityId & "'"

                                    lrORMQlREcordset = Me.zrPage.Model.processORMQLStatement(lsSQLQuery)

                                    If lrORMQlREcordset.Facts.Count > 0 Then
                                        lsMultiplicityLowerBound = lrORMQlREcordset.Facts(0).GetFactDataByRoleName("Bound").Data
                                    End If

                                Else
                                    lsSQLQuery = "SELECT Bound"
                                    lsSQLQuery &= " FROM " & pcenumCMMLRelations.MultiplicityHasUpperBound.ToString
                                    lsSQLQuery &= " WHERE Multiplicity = '" & lsMultiplicityId & "'"

                                    lrORMQlREcordset = Me.zrPage.Model.processORMQLStatement(lsSQLQuery)

                                    lsMultiplicityUpperBound = lrORMQlREcordset.Facts(0).GetFactDataByRoleName("Bound").Data


                                    lsSQLQuery = "SELECT Bound"
                                    lsSQLQuery &= " FROM " & pcenumCMMLRelations.MultiplicityHasLowerBound.ToString
                                    lsSQLQuery &= " WHERE Multiplicity = '" & lsMultiplicityId & "'"

                                    lrORMQlREcordset = Me.zrPage.Model.processORMQLStatement(lsSQLQuery)

                                    lsMultiplicityLowerBound = lrORMQlREcordset.Facts(0).GetFactDataByRoleName("Bound").Data
                                End If

                                Dim lrMultiplicity As New UML.Multiplicity(lsMultiplicityId, lrAttribute.Id, lsMultiplicityLowerBound, lsMultiplicityUpperBound)
                                lrAttribute.Multiplicity = lrMultiplicity

                                Me.UMLClassDiagram.Multiplicity.Add(lrMultiplicity)

                            End If

                            If lsMultiplicityLowerBound <> lsMultiplicityUpperBound Then
                                lsMultiplicity = lsMultiplicityLowerBound & ".." & lsMultiplicityUpperBound
                            Else
                                lsMultiplicity = lsMultiplicityUpperBound
                            End If

                            If lsIdentityConstraints = "" Then
                                '-----------------------------------
                                'Don't modify lsIdentifyIdentifier
                                '-----------------------------------
                            Else
                                lsIdentityIdentifier &= "(" & lsIdentityConstraints & ")"
                            End If


                            lrClass.TableShape.RowCount += 1
                            lrClass.TableShape.Item(0, lrClass.TableShape.RowCount - 1).Text = lsIdentityIdentifier & " " & lsPropertyName & " [" & lsMultiplicity & "] : " & lsDataType
                            lrClass.TableShape.Item(0, lrClass.TableShape.RowCount - 1).Tag = lrAttribute
                            lrClass.TableShape.ResizeToFitText(False)

                        Next 'For Each lrFactDataInstance In lrFactInstance.data
                    End If
                End If
            Next
        End If


        '==============================
        'Link the Subtypes/Supertypes
        '==============================
        Richmond.WriteToStatusBar("Linking Subtypes")
        Dim lrEREntityOrigin As New FBM.FactDataInstance
        Dim lrEREntityDestination As New FBM.FactDataInstance
        If IsSomething(Me.UMLClassDiagram.Generalisation) Then
            For Each lrFactInstance In Me.UMLClassDiagram.Generalisation.Fact

                Dim lrOriginEntity As New ERD.Entity
                Dim lrDestinationEntity As New ERD.Entity
                Dim lrRelationDictionary As New Dictionary(Of String, String)

                For Each lrFactDataInstance In lrFactInstance.Data
                    lrRelationDictionary.Add(lrFactDataInstance.Role.Id, lrFactDataInstance.Data)
                Next

                '--------------------------------------------
                'Find the Subtype Class to add the link to.
                '--------------------------------------------
                lsSQLQuery = "SELECT *"
                lsSQLQuery &= " FROM " & pcenumCMMLRelations.Generalisation.ToString
                lsSQLQuery &= " WHERE directSubtype = '" & lrFactInstance.GetFactDataInstanceByRoleName("directSubtype").Data & "'"

                lrORMQlREcordset = Me.zrPage.Model.processORMQLStatement(lsSQLQuery)

                Dim lrOriginClass As New UML.Class
                lrOriginClass.Page = Me.zrPage
                lrOriginClass.Model = Me.zrPage.Model
                lrOriginClass.FactData.Model = Me.zrPage.Model
                lrOriginClass.Name = lrORMQlREcordset.Facts(0).GetFactDataByRoleName("directSubtype").Data

                lrOriginClass = Me.UMLClassDiagram.Class.Find(AddressOf lrOriginClass.Equals)

                Dim loOriginTableNode As TableNode = lrOriginClass.TableShape

                '-----------------------------
                'Find the Destination Entity
                '-----------------------------
                Dim lrDestinationClass As New UML.Class
                lrDestinationClass.Page = Me.zrPage
                lrDestinationClass.Model = Me.zrPage.Model
                lrDestinationClass.FactData.Model = Me.zrPage.Model
                lrDestinationClass.Name = lrORMQlREcordset.Facts(0).GetFactDataByRoleName("directSupertype").Data

                lrDestinationClass = Me.UMLClassDiagram.Class.Find(AddressOf lrDestinationClass.Equals)

                Dim loDestinationTableNode As TableNode = lrDestinationClass.TableShape

                Dim loEntityLink As DiagramLink
                loEntityLink = Me.Diagram.Factory.CreateDiagramLink(loOriginTableNode, loDestinationTableNode)
                loEntityLink.SnapToNodeBorder = True
                loEntityLink.ShadowColor = Color.White
                loEntityLink.Style = LinkStyle.Cascading
                loEntityLink.SegmentCount = 3
                loEntityLink.HeadShape = ArrowHead.Triangle
                loEntityLink.HeadShapeSize = 5
                loEntityLink.Brush = New MindFusion.Drawing.SolidBrush(Color.White)

                '----------------------------------------------------------
                'Create the Generalisation for the Subtype/Supertype pair
                '----------------------------------------------------------
                Dim lrGeneralisation As New UML.Generalisation
                lrGeneralisation.Subtype = lrOriginClass
                lrGeneralisation.Supertype = lrDestinationClass
                lrGeneralisation.Link = loEntityLink
                lrGeneralisation.FactId = lrFactInstance.Id
                loEntityLink.Tag = lrGeneralisation
                loEntityLink.AutoRoute = True

                '-----------------------------------------------------------
                'Find the Origin and Destination points from the last save  
                '-----------------------------------------------------------
                '----------------------------------------
                'Get the actual OriginFactDataReference
                '----------------------------------------
                Dim lrFactPredicate As New tFactPredicate
                Dim lrOriginFactDataPredicate As New FBM.FactData
                Dim lrDestinationFactDataPredicate As New FBM.FactData
                Dim lrConcept As New FBM.Concept(lrFactInstance.Id)
                Dim lrGeneralisationEndsFactTypeInstance As FBM.FactTypeInstance
                lrGeneralisationEndsFactTypeInstance = arPage.FactTypeInstance.Find(Function(p) p.Id = pcenumCMMLRelations.GeneralisationEnds.ToString)
                lrOriginFactDataPredicate = New FBM.FactData(New FBM.Role(lrGeneralisationEndsFactTypeInstance, "OriginReference", True), lrConcept)
                lrFactPredicate.data.Add(lrOriginFactDataPredicate)
                lrGeneralisation.OriginFactDataInstance = lrGeneralisationEndsFactTypeInstance.Fact.Find(AddressOf lrFactPredicate.Equals).GetFactDataInstanceByRoleName("OriginReference")

                '----------------------------------------
                'Get the actual OriginFactDataReference
                '----------------------------------------
                lrDestinationFactDataPredicate = New FBM.FactData(New FBM.Role(lrGeneralisationEndsFactTypeInstance, "DestinationReference", True), lrConcept)
                lrFactPredicate.data.Add(lrDestinationFactDataPredicate)
                lrGeneralisation.DestinationFactDataInstance = lrGeneralisationEndsFactTypeInstance.Fact.Find(AddressOf lrFactPredicate.Equals).GetFactDataInstanceByRoleName("DestinationReference")

                Dim lrPointF As New PointF(lrGeneralisation.OriginFactDataInstance.X, lrGeneralisation.OriginFactDataInstance.Y)
                lrGeneralisation.Link.ControlPoints(0) = lrPointF
                lrPointF = New PointF(lrGeneralisation.DestinationFactDataInstance.X, lrGeneralisation.DestinationFactDataInstance.Y)
                lrGeneralisation.Link.ControlPoints(lrGeneralisation.Link.ControlPoints.Count - 1) = lrPointF
                lrGeneralisation.Link.UpdateFromPoints()

                Me.UMLClassDiagram.Generalisations.Add(lrGeneralisation)

                '-------------------------------------------------------------
                'Check if the Genreralisation is part of a GeneralisationSet
                '-------------------------------------------------------------
                lsSQLQuery = "SELECT *"
                lsSQLQuery &= " FROM " & pcenumCMMLRelations.GeneralisationIsPartOfGeneralisationSet.ToString
                lsSQLQuery &= " WHERE Generalisation = '" & lrFactInstance.Fact.Id & "'"

                lrORMQlREcordset = Me.zrPage.Model.processORMQLStatement(lsSQLQuery)

                If lrORMQlREcordset.Facts.Count > 0 Then
                    '---------------------------------------------------
                    'The Generalisation is part of a GeneralisationSet
                    '---------------------------------------------------
                    Dim lrGeneralisationSet As New UML.GeneralisationSet
                    lrGeneralisationSet.GeneralisationSetName = lrORMQlREcordset.Facts(0).GetFactDataByRoleName("GeneralisationSet").Data
                    lrGeneralisationSet.Generalisation.Add(lrGeneralisation)

                    If Me.UMLClassDiagram.GeneralisationSet.Exists(AddressOf lrGeneralisationSet.Equals) Then
                        '--------------------------------------------------------------------------------
                        'Add the Generalisation to the existing GeneralisationSet in Me.UMLClassDiagram
                        '--------------------------------------------------------------------------------
                        Me.UMLClassDiagram.GeneralisationSet.Find(AddressOf lrGeneralisationSet.Equals).Generalisation.Add(lrGeneralisation)

                        '---------------------------------------------------------------------------------------------------
                        'The Generalisation is part of an existing GeneralisationSet, so make the Link.Destination the same
                        '  as the first Generalisation in the GeneralisationSet
                        '---------------------------------------------------------------------------------------------------
                        Dim lrExistingGeneralisationSet As UML.GeneralisationSet = Me.UMLClassDiagram.GeneralisationSet.Find(AddressOf lrGeneralisationSet.Equals)
                        lrPointF = New PointF
                        lrPointF = lrExistingGeneralisationSet.Generalisation(0).Link.ControlPoints(lrExistingGeneralisationSet.Generalisation(0).Link.ControlPoints.Count - 1)
                        lrGeneralisation.Link.ControlPoints(lrGeneralisation.Link.ControlPoints.Count - 1) = lrPointF
                        lrGeneralisation.Link.UpdateFromPoints()

                    Else
                        '------------------------------------------------------------------------------
                        'Add the Generalisation to the set of Generalisations for the UMLClassDiagram
                        '------------------------------------------------------------------------------
                        Me.UMLClassDiagram.GeneralisationSet.Add(lrGeneralisationSet)

                        '---------------------------------------------------------------------------
                        'Position the Link.Destination towards the bottom of the Destination Class
                        '---------------------------------------------------------------------------
                        lrPointF = New PointF
                        lrPointF.X = lrDestinationClass.TableShape.Bounds.X + (lrDestinationClass.TableShape.Bounds.Width / 2)
                        lrPointF.Y = lrDestinationClass.TableShape.Bounds.Y + lrDestinationClass.TableShape.Bounds.Height
                        lrGeneralisation.Link.ControlPoints(lrGeneralisation.Link.ControlPoints.Count - 1) = lrPointF
                        lrGeneralisation.Link.UpdateFromPoints()


                        '---------------------------------------------------------------------------------
                        'Check to see if GeneralisationSet is either or both of Complete/Overlapping
                        '---------------------------------------------------------------------------------
                        Dim lbComplete As Boolean = False
                        Dim lbOverlapping As Boolean = False


                        lsSQLQuery = "SELECT *"
                        lsSQLQuery &= " FROM GeneralisationSetIsCovering"
                        lsSQLQuery &= " WHERE IsCovering = '" & lrGeneralisationSet.GeneralisationSetName & "'"

                        Dim lrORMQLRecordset2 As New ORMQL.Recordset

                        lrORMQLRecordset2 = Me.zrPage.Model.processORMQLStatement(lsSQLQuery)

                        If lrORMQLRecordset2.Facts.Count > 0 Then
                            '-----------------------------------------------------------------------------
                            'Is a Covering GeneralisationSet so add a comment to the Generalisation.Link
                            '-----------------------------------------------------------------------------
                            lbComplete = True
                        End If

                        lsSQLQuery = "SELECT *"
                        lsSQLQuery &= " FROM GeneralisationSetIsDisjoint"
                        lsSQLQuery &= " WHERE IsDisjoint = '" & lrGeneralisationSet.GeneralisationSetName & "'"

                        lrORMQLRecordset2 = Me.zrPage.Model.processORMQLStatement(lsSQLQuery)

                        If lrORMQLRecordset2.Facts.Count > 0 Then
                            '-----------------------------------------------------------------------------
                            'Is a Covering GeneralisationSet so add a comment to the Generalisation.Link
                            '-----------------------------------------------------------------------------
                            lbOverlapping = True
                        End If

                        Dim lsLinkText As String = "{"

                        If lbComplete Then lsLinkText &= "Complete"
                        If lbOverlapping Then
                            '-----------------
                            'Not overlapping
                            '-----------------
                        Else
                            If lsLinkText <> "" Then
                                lsLinkText &= ","
                            End If
                            lsLinkText &= "Overlapping"
                        End If

                        If lbComplete Or lbOverlapping Then
                            lsLinkText &= "}"
                        Else
                            lsLinkText = ""
                        End If

                        lrGeneralisation.Link.Text = lsLinkText

                    End If
                Else

                End If

                Me.Diagram.LinksSnapToBorders = True
            Next
        End If


        '=========================================
        'Create enumeration Entities on the Page
        '=========================================
        Richmond.WriteToStatusBar("Loading Enumeration Classes")
        If IsSomething(Me.UMLClassDiagram.EnumerationHasEnumerationName) Then
            For Each lrFactInstance In Me.UMLClassDiagram.EnumerationHasEnumerationName.Fact
                '--------------------------------------------------------------------------
                'Qualify the Fact is for 'Element's that have an 'ElementType' of 'Class'
                '--------------------------------------------------------------------------

                Dim lsEnumerationName As String = ""

                lsEnumerationName = lrFactInstance.GetFactDataInstanceByRoleName("EnumerationName").Data

                Dim lrClass As New UML.Class
                lrClass = lrFactInstance.GetFactDataInstanceByRoleName("Enumeration").CloneClass(arPage)

                '----------------------------------------------------------
                'Create a TableNode on the Page for the Enumeration Class
                '----------------------------------------------------------
                loDroppedNode = Diagram.Factory.CreateTableNode(lrClass.X, lrClass.Y, 20, 2, 1, 0)
                loDroppedNode.HandlesStyle = HandlesStyle.InvisibleMove
                loDroppedNode.Brush = New MindFusion.Drawing.SolidBrush(Color.White)
                loDroppedNode.CellFrameStyle = CellFrameStyle.None
                loDroppedNode.Resize(20, 15)
                loDroppedNode.ShadowColor = Color.White
                loDroppedNode.AllowIncomingLinks = True
                loDroppedNode.AllowOutgoingLinks = True
                loDroppedNode.Caption = "<<enumeration>>" & vbCrLf & lrClass.Symbol
                loDroppedNode.CaptionHeight = 2 * loDroppedNode.CaptionHeight
                loDroppedNode.Tag = New ERD.Entity
                loDroppedNode.Tag = lrClass
                lrClass.TableShape = loDroppedNode
                lrClass.FactDataInstance.TableShape = loDroppedNode

                Dim lrEREntity As New FBM.FactDataInstance
                Dim lrEnumerationLiteralFactInstance As New FBM.FactInstance
                If IsSomething(Me.UMLClassDiagram.EnumerationHasEnumerationLiteral) Then
                    For Each lrEnumerationLiteralFactInstance In Me.UMLClassDiagram.EnumerationHasEnumerationLiteral.Fact

                        Dim lrERAttribute As New ERD.Entity
                        Dim lsAttributeName As String = ""

                        lsEnumerationName = lrEnumerationLiteralFactInstance.GetFactDataInstanceByRoleName("Enumeration").Data
                        lsAttributeName = lrEnumerationLiteralFactInstance.GetFactDataInstanceByRoleName("EnumerationLiteral").Data

                        lrClass.TableShape.RowCount += 1
                        lrClass.TableShape.Item(0, lrClass.TableShape.RowCount - 1).Text = lsAttributeName
                        lrClass.TableShape.ResizeToFitText(False)
                    Next
                End If 'Enumeration has Enumeration Literal

            Next
        End If

        '=============================================================
        'Create the Associations on the Page (links between Classes)
        '=============================================================
        Dim lsAssociationName As String = ""

        lsSQLQuery = "SELECT DISTINCT Association"
        lsSQLQuery &= " FROM " & pcenumCMMLRelations.AssociationHasAssociationEnd.ToString

        lrORMQlREcordset = Me.zrPage.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

        For Each lrFact In lrORMQlREcordset.Facts
            '----------------------------------------------
            'Find the AssociationEnds of the Association.
            '----------------------------------------------
            Dim lsAssociationEndName As String = ""
            Dim lsClassName As String = ""
            Dim lasAssociationEndClassName As New List(Of String)

            lsAssociationName = lrFact.GetFactDataByRoleName("Association").Data

            lsSQLQuery = "SELECT *"
            lsSQLQuery &= " FROM " & pcenumCMMLRelations.AssociationHasAssociationEnd.ToString
            lsSQLQuery &= " WHERE Association = '" & lsAssociationName & "'"

            Dim lrORMQlREcordset2 As ORMQL.Recordset = Me.zrPage.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

            Dim lrAssociationEndFact As FBM.Fact

            For Each lrAssociationEndFact In lrORMQlREcordset2.Facts

                lsSQLQuery = "SELECT *"
                lsSQLQuery &= " FROM " & pcenumCMMLRelations.AssociationEndIsAttachedToClass.ToString
                lsSQLQuery &= " WHERE AssociationEnd = '" & lrAssociationEndFact.GetFactDataByRoleName("AssociationEnd").Data & "'"

                Dim lrORMQlREcordset3 As ORMQL.Recordset = Me.zrPage.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                Dim lrAssociationEndClass As FBM.Fact
                For Each lrAssociationEndClass In lrORMQlREcordset3.Facts
                    lasAssociationEndClassName.Add(lrORMQlREcordset3.Facts(0).GetFactDataByRoleName("Class").Data)
                Next
            Next

            Dim lrOriginEntity As New ERD.Entity
            Dim lrDestinationEntity As New ERD.Entity

            '------------------------------------------
            'Find the Class shape to add the Link to.
            '------------------------------------------
            Dim lrOriginClass As New UML.Class
            lrOriginClass.Page = Me.zrPage
            lrOriginClass.Model = Me.zrPage.Model
            lrOriginClass.FactData.Model = Me.zrPage.Model
            lrOriginClass.Name = lasAssociationEndClassName(0).ToString

            lrOriginClass = Me.UMLClassDiagram.Class.Find(AddressOf lrOriginClass.Equals)

            Dim loOriginTableNode As TableNode = lrOriginClass.TableShape

            '------------------------------------------
            'Find the Class shape to add the Link to.
            '------------------------------------------
            Dim lrDestinationClass As New UML.Class
            lrDestinationClass.Page = Me.zrPage
            lrDestinationClass.Model = Me.zrPage.Model
            lrDestinationClass.FactData.Model = Me.zrPage.Model
            lrDestinationClass.Name = lasAssociationEndClassName(1).ToString

            lrDestinationClass = Me.UMLClassDiagram.Class.Find(AddressOf lrDestinationClass.Equals)

            Dim loDestinationTableNode As TableNode = lrDestinationClass.TableShape

            Dim loEntityLink As DiagramLink
            loEntityLink = Me.Diagram.Factory.CreateDiagramLink(loOriginTableNode, loDestinationTableNode)
            loEntityLink.SnapToNodeBorder = True
            loEntityLink.ShadowColor = Color.White
            loEntityLink.Style = LinkStyle.Cascading
            loEntityLink.SegmentCount = 3
            loEntityLink.HeadShape = ArrowHead.None
            loEntityLink.AllowMoveEnd = True
            loEntityLink.AllowMoveStart = True


            Dim loOriginRoleNameShape As ShapeNode
            Dim loOriginPointF As New Point(loEntityLink.Bounds.X, loEntityLink.Bounds.Y)
            loOriginRoleNameShape = Me.Diagram.Factory.CreateShapeNode(loOriginPointF.X, loOriginPointF.Y, 35, 6)
            loOriginRoleNameShape.Text = loDestinationTableNode.Caption
            loOriginRoleNameShape.Tag = New UML.AssociationEndRole
            loOriginRoleNameShape.ShadowColor = Color.White
            loOriginRoleNameShape.Transparent = True
            loOriginRoleNameShape.AttachTo(loOriginTableNode, AttachToNode.BottomCenter)
            loOriginRoleNameShape.AllowIncomingLinks = False
            loOriginRoleNameShape.AllowOutgoingLinks = False
            loOriginRoleNameShape.Obstacle = False

            Dim loDestinationRoleNameShape As ShapeNode
            Dim loDestinationPointF As New Point(loEntityLink.Bounds.X + loEntityLink.Bounds.Width, loEntityLink.Bounds.Y + loEntityLink.Bounds.Height - 4)
            loDestinationRoleNameShape = Me.Diagram.Factory.CreateShapeNode(loDestinationPointF.X, loDestinationPointF.Y, 35, 6)
            loDestinationRoleNameShape.Text = loOriginTableNode.Caption
            loDestinationRoleNameShape.Tag = New UML.AssociationEndRole
            loDestinationRoleNameShape.ShadowColor = Color.White
            loDestinationRoleNameShape.Transparent = True
            loDestinationRoleNameShape.AttachTo(loDestinationTableNode, AttachToNode.TopCenter)
            loDestinationRoleNameShape.AllowIncomingLinks = False
            loDestinationRoleNameShape.AllowOutgoingLinks = False
            loDestinationRoleNameShape.Obstacle = False

            Dim loAssociationNameShape As ShapeNode
            Dim loAssociationPoint As New Point(loEntityLink.Bounds.X + (loEntityLink.Bounds.Width / 2), loEntityLink.Bounds.Y + ((loEntityLink.Bounds.Height - 4) / 2))
            loAssociationNameShape = Me.Diagram.Factory.CreateShapeNode(loAssociationPoint.X, loAssociationPoint.Y, 35, 6)
            loAssociationNameShape.Text = lsAssociationName
            loAssociationNameShape.Tag = New UML.AssociationEndRole
            loAssociationNameShape.ShadowColor = Color.White
            loAssociationNameShape.Transparent = True
            loAssociationNameShape.AllowIncomingLinks = False
            loAssociationNameShape.AllowOutgoingLinks = False
            loAssociationNameShape.Obstacle = False
            'loAssociationNameShape.AttachTo(loEntityLink, AttachToLink.Segment, 1)

            '-------------------------------------------
            'Find the Multiplicity for the Association
            '-------------------------------------------
            lsSQLQuery = "SELECT *"
            lsSQLQuery &= " FROM " & pcenumCMMLRelations.AssociationIsForAttribute.ToString
            lsSQLQuery &= " WHERE Association = '" & lsAssociationName & "'"

            lrORMQlREcordset2 = Me.zrPage.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

            Dim lsAttributeId As String = lrORMQlREcordset2.Facts(0).GetFactDataByRoleName("Attribute").Data

            Dim lrMultiplicty As New UML.Multiplicity("Dummy", lsAttributeId)

            lrMultiplicty = Me.UMLClassDiagram.Multiplicity.Find(AddressOf lrMultiplicty.EqualsByAttributeId)

            loOriginRoleNameShape.Text &= "   " & lrMultiplicty.LowerBound
            If lrMultiplicty.UpperBound = "n" Then
                loDestinationRoleNameShape.Text &= "   " & "*"
            Else
                loDestinationRoleNameShape.Text &= "   " & lrMultiplicty.UpperBound
            End If


        Next 'Association

        '==============================
        'Add the Comments to the Page
        '==============================
        lsSQLQuery = "SELECT Element, Comment"
        lsSQLQuery &= " FROM " & pcenumCMMLRelations.ElementHasComment.ToString
        lsSQLQuery &= " ON PAGE '" & Me.zrPage.Name & "'"

        lrORMQlREcordset = Me.zrPage.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

        For Each lrFact In lrORMQlREcordset.Facts

            Dim loCommentShape As ShapeNode
            Dim StringSize As New SizeF
            Dim lsComment As String = ""

            Dim lrCommentFactInstance As New FBM.FactInstance
            lrCommentFactInstance = lrORMQlREcordset.Facts(0)
            Dim lrCommentFactDataInstance As New FBM.FactDataInstance
            lsComment = lrCommentFactInstance.GetFactDataInstanceByRoleName("Comment").Data
            lrCommentFactDataInstance = lrCommentFactInstance.GetFactDataInstanceByRoleName("Comment").FactDataInstance

            StringSize = Me.Diagram.MeasureString(lsComment, Me.Diagram.Font, 1000, System.Drawing.StringFormat.GenericDefault)
            Dim lrComment As UML.Comment = lrCommentFactDataInstance.CloneUMLComment(arPage)
            Dim lr_rectanglef As New RectangleF(lrComment.X, lrComment.Y, StringSize.Width, StringSize.Height + 10)
            Dim lrShape As MindFusion.Diagramming.Shape
            lrShape = New Shape( _
                           New ElementTemplate() _
                           { _
                                New LineTemplate(0, 0, 78, 0), _
                                New LineTemplate(78, 0, 100, 21), _
                                New LineTemplate(100, 21, 100, 100), _
                                New LineTemplate(100, 100, 0, 100), _
                                New LineTemplate(0, 100, 0, 0) _
                           }, _
                           New ElementTemplate() _
                           { _
                                New LineTemplate(78, 0, 78, 20), _
                                New LineTemplate(78, 20, 100, 20) _
                           }, _
                     Nothing, FillMode.Winding, "test")

            loCommentShape = Me.Diagram.Factory.CreateShapeNode(lr_rectanglef, lrShape)
            loCommentShape.Text = lsComment
            loCommentShape.ShadowColor = Color.White
            loCommentShape.AllowIncomingLinks = False
            loCommentShape.AllowOutgoingLinks = True
            loCommentShape.Obstacle = False
            loCommentShape.Tag = lrComment

            '------------------------------------------
            'Find the Class shape to add the Link to.
            '------------------------------------------
            Dim lrDestinationClass As New UML.Class
            lrDestinationClass.Page = Me.zrPage
            lrDestinationClass.Model = Me.zrPage.Model
            lrDestinationClass.FactData.Model = Me.zrPage.Model
            lrDestinationClass.Name = lrCommentFactInstance.GetFactDataInstanceByRoleName("Element").Data

            lrDestinationClass = Me.UMLClassDiagram.Class.Find(AddressOf lrDestinationClass.Equals)

            Dim loDestinationTableNode As TableNode = lrDestinationClass.TableShape

            Dim loCommentLink As DiagramLink
            loCommentLink = Me.Diagram.Factory.CreateDiagramLink(loCommentShape, loDestinationTableNode)
            loCommentLink.SnapToNodeBorder = True
            loCommentLink.ShadowColor = Color.White
            loCommentLink.Style = LinkStyle.Cascading
            loCommentLink.SegmentCount = 3
            loCommentLink.HeadShape = ArrowHead.None
            loCommentLink.AllowMoveEnd = True
            loCommentLink.AllowMoveStart = True
            loCommentLink.Pen.DashStyle = Drawing2D.DashStyle.Custom
            ReDim loCommentLink.Pen.DashPattern(3)
            loCommentLink.Pen.DashPattern(0) = 1
            loCommentLink.Pen.DashPattern(1) = 2
            loCommentLink.Pen.DashPattern(2) = 1
            loCommentLink.Pen.DashPattern(3) = 2
            loCommentLink.Tag = lrComment
        Next

        frmMain.Cursor = Cursors.Default
        Me.Diagram.RouteAllLinks()
        Me.Diagram.Invalidate()
        Me.zrPage.FormLoaded = True

    End Sub



    Sub reset_node_and_link_colors()

        Dim liInd As Integer = 0

        '------------------------------------------------------------------------------------
        'Reset the border colors of the ShapeNodes (to what they were before being selected
        '------------------------------------------------------------------------------------
        For liInd = 1 To Diagram.Nodes.Count
            Select Case Diagram.Nodes(liInd - 1).Tag.ConceptType
                Case Is = pcenumConceptType.EntityType
                    Diagram.Nodes(liInd - 1).Pen.Color = Color.Black
                Case Is = pcenumConceptType.FactType
                    If Diagram.Nodes(liInd - 1).Tag.IsObjectified Then
                        Diagram.Nodes(liInd - 1).Visible = True
                        Diagram.Nodes(liInd - 1).Pen.Color = Color.Black
                    Else
                        Diagram.Nodes(liInd - 1).Visible = False
                        Diagram.Nodes(liInd - 1).Pen.Color = Color.Pink
                    End If
                Case Is = pcenumConceptType.RoleName
                    Diagram.Nodes(liInd - 1).Pen.Color = Color.Blue
                Case Is = pcenumConceptType.RoleConstraint
                    Select Case Diagram.Nodes(liInd - 1).Tag.RoleConstraintType
                        Case Is = pcenumRoleConstraintType.InternalUniquenessConstraint
                            Diagram.Nodes(liInd - 1).Pen.Color = Color.Maroon
                        Case Is = pcenumRoleConstraintType.ExclusionConstraint, _
                                  pcenumRoleConstraintType.ExternalUniquenessConstraint
                            Diagram.Nodes(liInd - 1).Pen.Color = Color.White
                        Case Else
                            Diagram.Nodes(liInd - 1).Pen.Color = Color.Black
                    End Select
                Case Is = pcenumConceptType.FactTable
                    Diagram.Nodes(liInd - 1).Pen.Color = Color.LightGray
                Case Else
                    Diagram.Nodes(liInd - 1).Pen.Color = Color.Black
            End Select
        Next

        For liInd = 1 To Diagram.Links.Count
            Diagram.Links(liInd - 1).Pen.Color = Color.Black
        Next

        Me.Diagram.Invalidate()

    End Sub

    Private Sub DiagramView_DragOver(ByVal sender As Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles DiagramView.DragOver

        Dim p As Point = Me.DiagramView.PointToClient(New Point(e.X, e.Y))
        Dim lo_point As PointF = Me.DiagramView.ClientToDoc(New Point(p.X, p.Y))
        Dim loNode As MindFusion.Diagramming.DiagramNode


        If IsSomething(Diagram.GetNodeAt(lo_point)) Then
            loNode = Diagram.GetNodeAt(lo_point)
            If TypeOf loNode Is MindFusion.Diagramming.ShapeNode Then
                loNode.Pen = New MindFusion.Drawing.Pen(Color.Brown)
            End If
        Else
            If e.Data.GetDataPresent(tShapeNodeDragItem.DraggedItemObjectType) Then
                Dim lnode_dragged_node As tShapeNodeDragItem = e.Data.GetData(tShapeNodeDragItem.DraggedItemObjectType)
                If lnode_dragged_node.Index = 1 Then
                    'Is Process and should be dropped only within the SystemBoundary (ContainerNode)
                    e.Effect = DragDropEffects.None
                    Exit Sub
                End If
            End If
        End If


        If e.Data.GetDataPresent(tShapeNodeDragItem.DraggedItemObjectType) Then
            e.Effect = DragDropEffects.Copy
        Else
            e.Effect = DragDropEffects.None
        End If

    End Sub

    Private Sub Diagram_LinkModified(ByVal sender As Object, ByVal e As MindFusion.Diagramming.LinkEventArgs) Handles Diagram.LinkModified


        Dim lsSqlQuery As String = ""
        Dim lrORMQLRecordset As New ORMQL.Recordset

        e.Link.AutoRoute = True

        Select Case e.Link.Tag.ConceptType
            Case Is = pcenumConceptType.Generalisation
                Dim lrGeneralisationSetGeneralisation As UML.Generalisation
                Dim lrGeneralisation As New UML.Generalisation
                lrGeneralisation.Link = e.Link

                lrGeneralisation = Me.UMLClassDiagram.Generalisations.Find(AddressOf lrGeneralisation.EqualsByLink)

                '-----------------------------------------------------------------
                'Set the X/Y Coordinates of the Origin/Destination of the Link
                '-----------------------------------------------------------------
                lrGeneralisation.OriginFactDataInstance.X = e.Link.ControlPoints(0).X
                lrGeneralisation.OriginFactDataInstance.Y = e.Link.ControlPoints(0).Y
                lrGeneralisation.DestinationFactDataInstance.X = e.Link.ControlPoints(e.Link.ControlPoints.Count - 1).X
                lrGeneralisation.DestinationFactDataInstance.Y = e.Link.ControlPoints(e.Link.ControlPoints.Count - 1).Y

                Dim lrDestinationClass As UML.Class = lrGeneralisation.Supertype

                '-------------------------------------------------------------
                'Check if the Genreralisation is part of a GeneralisationSet
                '-------------------------------------------------------------
                'GeneralisationIsPartOfGeneralisationSet
                lsSqlQuery = "SELECT *"
                lsSqlQuery &= " FROM " & pcenumCMMLRelations.GeneralisationIsPartOfGeneralisationSet.ToString
                lsSqlQuery &= " WHERE Generalisation = '" & lrGeneralisation.FactId & "'"

                lrORMQLRecordset = Me.zrPage.Model.ORMQL.ProcessORMQLStatement(lsSqlQuery)

                If lrORMQLRecordset.Facts.Count > 0 Then
                    '--------------------------------------------------------------------------------------------------------------------------------------
                    'The Generalisation is part of a GeneralisationSet, so move the destination end of the other Generalisations in the GeneralisationSet
                    '--------------------------------------------------------------------------------------------------------------------------------------
                    Dim lrGeneralisationSet As New UML.GeneralisationSet
                    lrGeneralisationSet.GeneralisationSetName = lrORMQLRecordset.Facts(0).GetFactDataByRoleName("GeneralisationSet").Data
                    lrGeneralisationSet = Me.UMLClassDiagram.GeneralisationSet.Find(AddressOf lrGeneralisationSet.Equals)

                    For Each lrGeneralisationSetGeneralisation In lrGeneralisationSet.Generalisation

                        If lrGeneralisationSetGeneralisation.FactId <> lrGeneralisation.FactId Then
                            Dim lrPointF As PointF = lrGeneralisation.Link.ControlPoints(lrGeneralisation.Link.ControlPoints.Count - 1)
                            lrGeneralisationSetGeneralisation.Link.ControlPoints(lrGeneralisationSetGeneralisation.Link.ControlPoints.Count - 1) = lrPointF
                            lrGeneralisation.DestinationFactDataInstance.X = lrPointF.X
                            lrGeneralisation.DestinationFactDataInstance.Y = lrPointF.Y
                            lrGeneralisationSetGeneralisation.Link.UpdateFromPoints()
                        End If
                    Next
                End If

        End Select

    End Sub

    Private Sub Diagram_NodeModified(ByVal sender As Object, ByVal e As MindFusion.Diagramming.NodeEventArgs) Handles Diagram.NodeModified

        Dim lrTableNode As TableNode
        Dim lrShapeNode As ShapeNode
        Dim lrShape As ShapeNode
        Me.zrPage.MakeDirty()
        frmMain.ToolStripButton_Save.Enabled = True

        '-------------------------------------------------------------------------------------------
        'The user has clicked/moved a ShapeNode, so update the X and Y coordinates of the ShapeNode
        '-------------------------------------------------------------------------------------------            
        Dim lrLink As DiagramLink
        Dim lrORMObject As New Object
        Dim lrFactInstance As New FBM.FactInstance

        If TypeOf (e.Node) Is MindFusion.Diagramming.ShapeNode Then



        End If

        Select Case e.Node.Tag.ConceptType
            Case Is = pcenumConceptType.Class
                lrTableNode = e.Node
                Dim lrFactDataInstance As New FBM.FactDataInstance(Me.zrPage)

                lrFactDataInstance = lrTableNode.Tag.FactDataInstance
                lrFactDataInstance.X = e.Node.Bounds.X
                lrFactDataInstance.Y = e.Node.Bounds.Y
            Case Is = pcenumConceptType.Comment
                lrShapeNode = e.Node
                Dim lrFactDataInstance As New FBM.FactDataInstance(Me.zrPage)

                lrFactDataInstance = lrShapeNode.Tag.FactDataInstance
                lrFactDataInstance.X = e.Node.Bounds.X
                lrFactDataInstance.Y = e.Node.Bounds.Y
            Case Else
                lrShape = e.Node

                If IsSomething(e.Node.Tag) Then
                    For Each lrLink In lrShape.GetAllLinks
                        If lrLink.Origin Is lrShape Then
                            lrORMObject = lrLink.Origin.Tag
                        End If
                        If lrLink.Destination Is lrShape Then
                            lrORMObject = lrLink.Destination.Tag
                        End If
                        lrORMObject.X = e.Node.Bounds.X
                        lrORMObject.Y = e.Node.Bounds.Y
                        '---------------------------------------
                        'Get the Fact associated with the Link
                        '---------------------------------------
                        lrFactInstance = lrLink.Tag
                        Dim lrFactDataInstance As New FBM.FactDataInstance(Me.zrPage)
                        Dim lrRole As New FBM.Role
                        Select Case lrORMObject.ConceptType
                            Case Is = pcenumConceptType.Entity
                                lrRole.Id = pcenumCMML.Actor.ToString
                        End Select
                        lrRole.Name = lrRole.Id
                        lrFactDataInstance.Role = lrRole.CloneInstance(Me.zrPage)
                        lrFactDataInstance.Data = lrORMObject.Data
                        'lrFactDataInstance = lrFactInstance.Data.Find(AddressOf lrFactDataInstance.Equals)
                        'lrFactDataInstance.X = e.Node.Bounds.X
                        'lrFactDataInstance.Y = e.Node.Bounds.Y
                    Next
                End If

        End Select

        Me.Diagram.RouteAllLinks()

    End Sub


    Private Sub Diagram_NodeSelected(ByVal sender As Object, ByVal e As MindFusion.Diagramming.NodeEventArgs) Handles Diagram.NodeSelected

        '------------------------------------------------------------------------------------------
        'NB IMPORTANT: DiagramView.MouseDown gets processed before Diagram.NodeSelected, so management of which object
        '  is displayed in the PropertyGrid is performed in ORMDiagram.MouseDown
        '------------------------------------------------------------------------------------------

        Select Case e.Node.Tag.ConceptType
            Case Is = pcenumConceptType.Entity
                e.Node.Pen.Color = Color.Blue
            Case Else
                'Do nothing
        End Select

        '----------------------------------------------------
        'Set the ContextMenuStrip menu for the selected item
        '----------------------------------------------------
        Select Case Me.Diagram.Selection.Items(0).Tag.ConceptType
            Case Is = pcenumConceptType.Entity
                Me.DiagramView.ContextMenuStrip = ContextMenuStrip_Actor
            Case Else
                Me.DiagramView.ContextMenuStrip = ContextMenuStrip_Diagram
        End Select

        Me.zrPage.SelectedObject.Add(e.Node.Tag)

    End Sub

    Private Sub DiagramView_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles DiagramView.GotFocus

        Call SetToolbox()

        Call frmMain.hide_unnecessary_forms(Me.zrPage)

    End Sub

    Private Sub DiagramView_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles DiagramView.MouseDown

        Dim lo_point As System.Drawing.PointF
        Dim loNode As DiagramNode

        lo_point = Me.DiagramView.ClientToDoc(e.Location)

        Me.DiagramView.SmoothingMode = SmoothingMode.AntiAlias

        '--------------------------------------------------
        'Just to be sure...set the Richmond.WorkingProject
        '--------------------------------------------------
        prApplication.WorkingPage = Me.zrPage

        If IsSomething(Diagram.GetNodeAt(lo_point)) Then
            '----------------------------
            'Mouse is over an ShapeNode
            '----------------------------
            'Me.Diagram.AllowUnconnectedLinks = True

            '--------------------------------------------
            'Turn on the TimerLinkSwitch.
            '  After the user has held down the mouse button for 1second over a object,
            '  then link creation will be allowed
            '--------------------------------------------
            TimerLinkSwitch.Enabled = True

            '----------------------------------------------------
            'Get the Node/Shape under the mouse cursor.
            '----------------------------------------------------
            loNode = Diagram.GetNodeAt(lo_point)
            Me.DiagramView.DrawLinkCursor = Cursors.Hand
            Cursor.Show()

            '---------------------------------------------------------------------------------------------------------------------------
            'If the PropertiesForm is loaded, set the 'SelectedObject' property of the PropertyGrid to the UseCaseModel object selected
            '---------------------------------------------------------------------------------------------------------------------------
            If Not IsNothing(frmMain.zfrm_properties) Then
                frmMain.zfrm_properties.PropertyGrid.SelectedObject = loNode.Tag
            End If

            'If IsSomething(Diagram.GetNodeAt(lo_point)) Then
            '    '----------------------------
            '    'Mouse is over an ShapeNode
            '    '----------------------------

            '    '----------------------------------------------------
            '    'Get the Node/Shape under the mouse cursor.
            '    '----------------------------------------------------
            '    If TypeOf Diagram.GetNodeAt(lo_point) Is MindFusion.Diagramming.TableNode Then
            '        Exit Sub
            '    End If
            '    loNode = Diagram.GetNodeAt(lo_point)

            '    If Control.ModifierKeys And Keys.Control Then
            '        '------------------------------------
            '        'Use is holding down the CtrlKey so
            '        '  enforce the selection of the object
            '        '------------------------------------                
            '        loNode.Selected = True
            '        loNode.Pen.Color = Color.Blue
            '        prApplication.workingpage.MultiSelectionPerformed = True

            '        Select Case loNode.Tag.ConceptType
            '            Case Is = pcenumConceptType.role
            '                '----------------------------------------------------------------------
            '                'This stops a Role AND its FactType being selected at the same time
            '                '----------------------------------------------------------------------
            '                prApplication.workingpage.SelectedObject.Remove(loNode.Tag.FactType)
            '        End Select

            '        Exit Sub
            '    Else
            '        If prApplication.workingpage.MultiSelectionPerformed Then
            '            If Diagram.Selection.Nodes.Contains(loNode) Then
            '                '--------------------------------------------------------------------
            '                'Don't clear the SelectedObjects if the ShapeNode selected/clicked on 
            '                '  is within the Diagram.Selection because the user has just performed
            '                '  a MultiSelection, ostensibly (one would assume) to then 'move'
            '                '  or 'delete' the selection of objects.
            '                '--------------------------------------------------------------------                    
            '                '------------------------------------------------------------------------------
            '                'Unless the Shape.Tag is a FactType, then just select it
            '                '  The reason for this, is because of MindFusion.Groups.
            '                '  When a User selects a Role...the whole RoleGroup (all Roles in the FactType)
            '                '  are selected, so it is a MultiSelection by default.
            '                '------------------------------------------------------------------------------
            '                Select Case loNode.Tag.ConceptType
            '                    Case Is = pcenumConceptType.FactType
            '                        me.zrPage.SelectedObject.Clear()
            '                        Diagram.Selection.Clear()
            '                        '-----------------------------------------------
            '                        'Select the ShapeNode/ORMObject just clicked on
            '                        '-----------------------------------------------                    
            '                        loNode.Selected = True
            '                        loNode.Pen.Color = Color.Blue

            '                        '-------------------------------------------------------------------
            '                        'Reset the MultiSelectionPerformed flag on the ORMModel
            '                        '-------------------------------------------------------------------
            '                        me.zrPage.MultiSelectionPerformed = False
            '                End Select
            '            Else
            '                '---------------------------------------------------------------------------
            '                'Clear the SelectedObjects because the user neither did a MultiSelection
            '                '  nor held down the [Ctrl] key before clicking on the ShapeNode.
            '                '  Clearing the SelectedObject groups, allows for new objects to be selected
            '                '  starting with the ShapeNode/ORMObject just clicked on.
            '                '---------------------------------------------------------------------------
            '                me.zrPage.SelectedObject.Clear()
            '                Diagram.Selection.Clear()
        ElseIf IsSomething(Diagram.GetLinkAt(lo_point, 2)) Then
            '-------------------------
            'User clicked on a link
            '-------------------------
        Else
            '------------------------------------------------
            'Use clicked on the Canvas
            '------------------------------------------------

            '---------------------------
            'Clear the SelectedObjects
            '---------------------------
            Me.zrPage.SelectedObject.Clear()
            Me.Diagram.Selection.Clear()

            Me.DiagramView.ContextMenuStrip = ContextMenuStrip_Diagram

            Me.Diagram.AllowUnconnectedLinks = False

            '------------------------------------------------------------------------------
            'Clear the 'InPlaceEdit' on principal.
            '  i.e. Is only allowed for 'Processes' and the user clicked on the Canvas,
            '  so disable the 'InPlaceEdit'.
            '  NB See Diagram.DoubleClick where if a 'Process' is DoubleClicked on,
            '  then 'InPlaceEdit' is temporarily allowed.
            '------------------------------------------------------------------------------
            Me.DiagramView.AllowInplaceEdit = False

            '-----------------------------------------------------------------------------------------------------------
            'If the PropertiesForm is loaded, set the 'SelectedObject' property of the PropertyGrid to the UseCaseModel
            '-----------------------------------------------------------------------------------------------------------
            If Not IsNothing(frmMain.zfrm_properties) Then
                frmMain.zfrm_properties.PropertyGrid.SelectedObject = Me.UMLClassDiagram
            End If

            Call Me.reset_node_and_link_colors()
        End If

    End Sub


    Private Sub Diagram_NodeTextEdited(ByVal sender As Object, ByVal e As MindFusion.Diagramming.EditNodeTextEventArgs) Handles Diagram.NodeTextEdited

        If e.Node.Tag.ConceptType = pcenumConceptType.Process Then
            e.Node.Tag.name = e.NewText
        End If

    End Sub


    Private Sub CopyToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CopyToolStripMenuItem.Click

        Dim image As Image = Diagram.CreateImage()
        Windows.Forms.Clipboard.SetImage(image)

    End Sub

    Private Sub DiagramView_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles DiagramView.MouseUp

        Dim liInd As Integer = 0
        Dim lo_point As System.Drawing.PointF
        Dim loObject As Object
        Dim loNode As DiagramNode

        DiagramView.SmoothingMode = SmoothingMode.AntiAlias

        '----------------------------------------------------
        'Check to see if the user has used the Control key to
        '  do a multi-select
        '----------------------------------------------------
        If Control.ModifierKeys And Keys.Control Then
            Exit Sub
        End If

        '-------------------------------------------------------
        'Check to see if the user was clicking over a ShapeNode
        '-------------------------------------------------------
        lo_point = Diagram.PixelToUnit(e.Location)

        If IsSomething(Diagram.GetItemAt(lo_point, False)) Then
            '----------------------------------------------
            'Mouse is over a DiagramItem
            '----------------------------------------------
            loObject = Diagram.GetItemAt(lo_point, False)

            If Not (TypeOf (loObject) Is MindFusion.Diagramming.DiagramNode) Then
                Exit Sub
            Else
                loNode = Diagram.GetItemAt(lo_point, False)
            End If


            '----------------------------------------------
            'Reset the cursor to a hand
            '----------------------------------------------
            'DiagramView.DrawLinkCursor = Cursors.Hand
            Cursor.Show()

            '------------------------------------------------------------------------------------
            'Reset ShapeNode.Colors to Black. e.g. User 'may' have just moved a whole selection.
            '------------------------------------------------------------------------------------
            For liInd = 1 To Diagram.Nodes.Count
                'Select Case Diagram.Nodes(liInd - 1).Tag.ConceptType
                '    Case Else
                '        Diagram.Nodes(liInd - 1).Pen.Color = Color.Black
                'End Select
            Next

            For liInd = 1 To Diagram.Links.Count
                Diagram.Links(liInd - 1).Pen.Color = Color.Black
            Next

            '-------------------------------------------------------------------------------------------
            'The user has clicked/moved a ShapeNode, so update the X and Y coordinates of the ShapeNode
            '-------------------------------------------------------------------------------------------            
            If Not IsNothing(loNode.Tag) Then
                loNode.Tag.x = loNode.Bounds.X
                loNode.Tag.y = loNode.Bounds.Y
            End If

            '-----------------------------------------------------------------------------------
            'Set/Reset the color of the ShapeNode under the mouse cursor
            '-----------------------------------------------------------------------------------
            loNode = Diagram.GetItemAt(lo_point, False)

            'Select Case loNode.Tag.ConceptType
            '    Case Is = pcenumConceptType.Actor
            '        loNode.Pen.Color = Color.Blue
            '    Case Else
            '        loNode.Pen.Color = Color.Blue
            'End Select

        Else
            '------------------------------------------------------------------
            'Mouse is over the canvas and a MultiSelection may have taken place
            '------------------------------------------------------------------
            Me.Diagram.AllowUnconnectedLinks = False
        End If

        '---------------------------------------------
        'Refresh the Diagram drawing (ShapeNode/Links)
        '---------------------------------------------
        Diagram.Invalidate()

    End Sub

    Private Sub TimerLinkSwitch_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TimerLinkSwitch.Tick

        'Me.DiagramView.Behavior = Behavior.DrawLinks

        TimerLinkSwitch.Enabled = False

    End Sub



    Private Sub mnuOption_ProcessProperties_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PropertiesToolStripMenuItem2.Click

        Call frmMain.LoadToolboxPropertyWindow(Me.DockPanel.ActivePane)

        If Not IsNothing(frmMain.zfrm_properties) Then
            If Me.Diagram.Selection.Items.Count > 0 Then
                frmMain.zfrm_properties.PropertyGrid.SelectedObject = Me.Diagram.Selection.Items(0).Tag
            Else
                frmMain.zfrm_properties.PropertyGrid.SelectedObject = Me.UMLClassDiagram
            End If
        End If

    End Sub

    Private Sub DiagramView_MouseWheel(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles DiagramView.MouseWheel

        Select Case e.Delta
            Case Is = 0
                'Do Nothing
            Case Is < 0
                If frmMain.ToolStripComboBox_zoom.SelectedIndex > 0 Then
                    frmMain.ToolStripComboBox_zoom.SelectedIndex -= 1
                End If
            Case Is > 0
                If frmMain.ToolStripComboBox_zoom.SelectedIndex < frmMain.ToolStripComboBox_zoom.Items.Count Then
                    If frmMain.ToolStripComboBox_zoom.SelectedIndex < frmMain.ToolStripComboBox_zoom.Items.Count - 1 Then
                        frmMain.ToolStripComboBox_zoom.SelectedIndex += 1
                    End If
                End If
        End Select
    End Sub

    Private Sub ModelDictionaryToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ModelDictionaryToolStripMenuItem.Click

        Call frmMain.LoadToolboxModelDictionary()

    End Sub



    Private Sub morph_to_StateTransitionDiagram(ByVal sender As Object, ByVal e As EventArgs)

        Dim item As ToolStripItem = CType(sender, ToolStripItem)

        Me.Diagram1.Nodes.Clear()
        Call Me.DiagramView.SendToBack()
        Call Me.HiddenDiagramView.BringToFront()

        '--------------------------------------------------------------
        'Paste the selected Actor/EntityType to the HiddenDiagramView
        '  (for animated morphing)
        '--------------------------------------------------------------
        Dim lr_shape_node As ShapeNode
        lr_shape_node = Me.zrPage.SelectedObject(0).Shape

        Dim lrProcess As New CMML.Process
        lrProcess = Me.zrPage.SelectedObject(0)

        lr_shape_node = Me.Diagram1.Factory.CreateShapeNode(lr_shape_node.Bounds.X, lr_shape_node.Bounds.Y, 70, 5) ' lr_shape_node.Shape = Shapes.RoundRect
        lr_shape_node.Shape = Shapes.Rectangle
        lr_shape_node.Text = lrProcess.Name
        lr_shape_node.Pen.Color = Color.Black
        lr_shape_node.Visible = True
        lr_shape_node.Brush = New MindFusion.Drawing.SolidBrush(Color.White)

        Me.morph_shape = lr_shape_node

        Me.Diagram1.Invalidate()


        If IsSomething(frmMain.zfrmModelExplorer) Then
            Dim lr_enterprise_view As tEnterpriseEnterpriseView
            lr_enterprise_view = item.Tag

            prApplication.WorkingPage = lr_enterprise_view.Tag

            '------------------------------------------------------------------
            'Get the X,Y co-ordinates of the ValueType being morphed
            '------------------------------------------------------------------
            Dim lr_page As New FBM.Page(lr_enterprise_view.Tag.Model)
            lr_page = lr_enterprise_view.Tag

            Me.MorphTimer.Enabled = True
            Me.MorphStepTimer.Enabled = True
            Me.morph_vector = New tMorphVector(Me.morph_vector.StartPoint.X, Me.morph_vector.StartPoint.Y, 5, 5, 40)
            Me.MorphStepTimer.Tag = lr_enterprise_view.TreeNode
            Me.MorphStepTimer.Start()
            Me.MorphTimer.Start()

        End If

    End Sub

    Public Sub morph_to_UseCase_diagram(ByVal sender As Object, ByVal e As EventArgs)

        Dim item As ToolStripItem = CType(sender, ToolStripItem)

        '---------------------------------------------
        'Take a copy of the selected Actor/EntityType
        '---------------------------------------------
        'Me.ORMDiagramView.CopyToClipboard(False)

        Me.Diagram1.Nodes.Clear()
        Call Me.DiagramView.SendToBack()
        Call Me.HiddenDiagramView.BringToFront()

        '--------------------------------------------------------------
        'Paste the selected Actor/EntityType to the HiddenDiagramView
        '  (for animated morphing)
        '--------------------------------------------------------------
        Dim lr_shape_node As ShapeNode
        lr_shape_node = Me.zrPage.SelectedObject(0).Shape
        lr_shape_node = Me.Diagram1.Factory.CreateShapeNode(lr_shape_node.Bounds.X, lr_shape_node.Bounds.Y, lr_shape_node.Bounds.Width, lr_shape_node.Bounds.Height)
        lr_shape_node.Shape = Shapes.Rectangle
        lr_shape_node.Text = ""
        lr_shape_node.Pen.Color = Color.White
        lr_shape_node.Transparent = True
        lr_shape_node.Image = My.Resources.resource_file.actor
        lr_shape_node.Visible = True

        Me.morph_shape = lr_shape_node

        Me.Diagram1.Invalidate()

        If IsSomething(frmMain.zfrmModelExplorer) Then
            Dim lr_enterprise_view As tEnterpriseEnterpriseView
            lr_enterprise_view = item.Tag
            frmMain.zfrmModelExplorer.TreeView.SelectedNode = lr_enterprise_view.TreeNode
            prApplication.WorkingPage = lr_enterprise_view.Tag

            '------------------------------------------------------------------
            'Get the X,Y co-ordinates of the Actor/EntityType being morphed
            '------------------------------------------------------------------
            Dim lr_page As New FBM.Page(lr_enterprise_view.Tag.Model)
            lr_page = lr_enterprise_view.Tag
            Dim lrActor = From FactType In lr_page.FactTypeInstance _
                          From Fact In FactType.Fact _
                          From RoleData In Fact.Data _
                          Where RoleData.Role.JoinedORMObject.Name = pcenumCMML.Actor.ToString _
                          Select New FBM.FactDataInstance(Me.zrPage, Fact, RoleData.Role, RoleData.Concept, RoleData.X, RoleData.Y)

            Dim lrFactDataInstance As New Object
            For Each lrFactDataInstance In lrActor
                Exit For
            Next

            Me.morph_vector = New tMorphVector(Me.morph_vector.StartPoint.X, Me.morph_vector.StartPoint.Y, lrFactDataInstance.x, lrFactDataInstance.y, 40)
            Me.MorphTimer.Enabled = True
            Me.MorphStepTimer.Enabled = True

        End If

    End Sub

    Private Sub MorphTimer_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MorphTimer.Tick

        Call Me.HiddenDiagramView.SendToBack()

        Me.MorphTimer.Stop()

    End Sub

    Private Sub MorphStepTimer_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MorphStepTimer.Tick

        Dim lr_point As New Point
        Dim lr_rect As New Rectangle

        lr_point = Me.morph_vector.getNextMorphVectorStepPoint

        Me.morph_shape.Move(lr_point.X, lr_point.Y)
        Me.Diagram1.Invalidate()

        If Me.morph_vector.VectorStep > Me.morph_vector.VectorSteps Then
            Me.MorphStepTimer.Stop()
            Me.MorphStepTimer.Enabled = False
            frmMain.zfrmModelExplorer.TreeView.SelectedNode = Me.MorphStepTimer.Tag
            Call frmMain.zfrmModelExplorer.EditPageToolStripMenuItem_Click(sender, e)
            Me.DiagramView.BringToFront()
            Me.Diagram.Invalidate()
            Me.MorphTimer.Enabled = False
        End If

    End Sub

    Private Sub frm_UseCaseModel_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.GotFocus

        If IsSomething(Me.zoTreeNode) Then
            If IsSomething(frmMain.zfrmModelExplorer) Then
                frmMain.zfrmModelExplorer.TreeView.SelectedNode = Me.zoTreeNode
            End If
        End If

        If IsSomething(frmMain.zfrm_KL_theorem_writer) Then
            frmMain.zfrm_KL_theorem_writer.zrPage = Me.zrPage
        End If

        If IsSomething(Me.zrPage) Then
            Me.zrPage.SelectedObject.Clear()
        End If

        Call Me.SetToolbox()

    End Sub

    Public Sub EnableSaveButton()

        '-------------------------------------
        'Raised from ModelObjects themselves
        '-------------------------------------
        frmMain.ToolStripButton_Save.Enabled = True
    End Sub

    Private Sub ToolboxToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolboxToolStripMenuItem.Click

        Call frmMain.LoadToolbox()
        Call SetToolbox()

    End Sub


    Private Sub frm_UseCaseModel_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles DiagramView.MouseDown

        Dim lo_point As System.Drawing.PointF
        Dim loTableNode As New TableNode

        Me.DiagramView.SmoothingMode = SmoothingMode.Default

        prApplication.WorkingPage = Me.zrPage

        lo_point = Me.DiagramView.ClientToDoc(e.Location)

        Dim lrShape As MindFusion.Diagramming.DiagramNode

        If IsSomething(Diagram.GetNodeAt(lo_point)) Then

            lrShape = Diagram.GetNodeAt(lo_point)

            Select Case lrShape.Tag.ConceptType
                Case Is = pcenumConceptType.AssociationEndRole

                Case Is = pcenumConceptType.Class

                    loTableNode = Diagram.GetNodeAt(lo_point)

                    '-----------------------------------------------------------------------------------------------------------------------
                    'If the PropertiesForm is loaded, set the 'SelectedObject' property of the PropertyGrid to the ORMModel object selected
                    '-----------------------------------------------------------------------------------------------------------------------
                    Dim lrPropertyGridForm As frmToolboxProperties
                    lrPropertyGridForm = prApplication.GetToolboxForm(frmToolboxProperties.Name)


                    If IsSomething(lrPropertyGridForm) And IsSomething(loTableNode) Then
                        Dim lrModelObject As FBM.ModelObject
                        lrModelObject = loTableNode.Tag
                        lrPropertyGridForm.PropertyGrid.BrowsableAttributes = Nothing
                        lrPropertyGridForm.PropertyGrid.HiddenAttributes = Nothing
                        Select Case lrModelObject.ConceptType
                            Case Is = pcenumConceptType.Class
                                lrPropertyGridForm.PropertyGrid.SelectedObject = lrModelObject
                        End Select

                    End If
                Case Is = pcenumConceptType.Comment

            End Select
        Else
            '---------------------------------------------------
            'MouseDown is on canvas (not on object).
            'If any objects are already highlighted as blue, 
            '  then change the outline to black/originalcolour
            '---------------------------------------------------
            Dim lrPropertyGridForm As frmToolboxProperties

            lrPropertyGridForm = prApplication.GetToolboxForm(frmToolboxProperties.Name)
            If IsSomething(lrPropertyGridForm) Then

                Dim myfilterattribute As Attribute = New System.ComponentModel.CategoryAttribute("Page")
                ' And you pass it to the PropertyGrid,
                ' via its BrowsableAttributes property :
                lrPropertyGridForm.PropertyGrid.BrowsableAttributes = New System.ComponentModel.AttributeCollection(New System.Attribute() {myfilterattribute})
                lrPropertyGridForm.PropertyGrid.HiddenAttributes = Nothing
                lrPropertyGridForm.PropertyGrid.SelectedObject = Me.zrPage

            End If
        End If

    End Sub

    Private Sub mnuOption_ViewGrid_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuOption_ViewGrid.Click

        mnuOption_ViewGrid.Checked = Not mnuOption_ViewGrid.Checked
        Me.Diagram.ShowGrid = mnuOption_ViewGrid.Checked

    End Sub

    Private Sub PropertiesToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PropertiesToolStripMenuItem.Click


        Call frmMain.LoadToolboxPropertyWindow(Me.DockPanel.ActivePane)

        Dim lrPropertyGridForm As frmToolboxProperties

        If IsSomething(prApplication.GetToolboxForm(frmToolboxProperties.Name)) Then
            lrPropertyGridForm = prApplication.GetToolboxForm(frmToolboxProperties.Name)
            lrPropertyGridForm.PropertyGrid.HiddenAttributes = Nothing
            If Me.Diagram.Selection.Items.Count > 0 Then
                lrPropertyGridForm.PropertyGrid.SelectedObject = Me.Diagram.Selection.Items(0).Tag
            Else
                lrPropertyGridForm.PropertyGrid.SelectedObject = Me.zrPage.Model
            End If
        End If

    End Sub

    Private Sub PageAsORMMetaModelToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PageAsORMMetaModelToolStripMenuItem.Click

        Me.zrPage.Language = pcenumLanguage.ORMModel
        Me.zrPage.FormLoaded = False

        Call frmMain.zfrmModelExplorer.EditPageToolStripMenuItem_Click(sender, e)

    End Sub
End Class