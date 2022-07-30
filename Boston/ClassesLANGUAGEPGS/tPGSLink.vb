Imports MindFusion.Diagramming
Imports System.ComponentModel
Imports System.Xml.Serialization
Imports System.Reflection
Imports Boston.ERD

Namespace PGS
    <Serializable()> _
    Public Class Link
        Inherits FBM.FactDataInstance
        Implements ERD.iLinkObject

        <XmlAttribute()> _
        Public Shadows ConceptType As pcenumConceptType = pcenumConceptType.Link

        Public FactInstance As FBM.FactInstance
        Public Shadows WithEvents Fact As FBM.Fact

        Public OriginModelElement As PGS.Node
        Public DestinationModelElement As PGS.Node

        Public Shadows WithEvents Relation As ERD.Relation

        Public WithEvents RDSRelation As RDS.Relation

        Public SentData As New List(Of String)

        Private Shadows _Link As MindFusion.Diagramming.DiagramLink
        Public Shadows Property Link As MindFusion.Diagramming.DiagramLink
            Get
                Return Me._Link
            End Get
            Set(value As MindFusion.Diagramming.DiagramLink)
                Me._Link = value
            End Set
        End Property

        Public Color As System.Drawing.Color = Color.Black

        Public HasBeenMoved As Boolean = False 'Used in AutoLayout (see DFD ResetNodeAndLinkColours)

        Public Sub New()

        End Sub

        Public Sub New(ByRef arPage As FBM.Page, _
                       ByRef arFactInstance As FBM.FactInstance, _
                       Optional ByRef aoOriginModelElement As FBM.FactDataInstance = Nothing, _
                       Optional ByVal aoDestinationModelElement As FBM.FactDataInstance = Nothing, _
                       Optional ByVal asSentData As String = "", _
                       Optional ByRef aoLink As ERD.tERDLink = Nothing, _
                       Optional ByRef arRelation As ERD.Relation = Nothing)

            Me.Page = arPage
            Me.Model = arPage.Model
            Me.FactData.Model = arPage.Model
            Me.FactInstance = arFactInstance
            Me.Fact = arFactInstance
            Me.RDSRelation = arRelation.RDSRelation

            If IsSomething(aoOriginModelElement) Then
                Me.OriginModelElement = aoOriginModelElement
            End If

            If IsSomething(aoDestinationModelElement) Then
                Me.DestinationModelElement = aoDestinationModelElement
            End If

            If IsSomething(arRelation) Then
                Me.Relation = arRelation
                Me.Relation.Link = Me
            End If

            Me.SentData.Add(asSentData)

            If IsSomething(aoLink) Then
                aoLink.Text = asSentData
                aoLink.Tag = Me
                Me.Link = aoLink
            End If

        End Sub

        Public Shadows Function EqualsByName(ByVal other As CMML.Process) As Boolean

            If other.Name Like (Me.Name) Then
                Return True
            Else
                Return False
            End If

        End Function

        Public Sub DisplayAndAssociate()

            Try
                If Me.OriginModelElement.Shape Is Nothing Then Exit Sub
                If Me.Link IsNot Nothing Then Exit Sub

                Dim lrDiagramLink As New DiagramLink(Me.Page.Diagram, Me.OriginModelElement.Shape, Me.DestinationModelElement.Shape)

                lrDiagramLink.Style = LinkStyle.Bezier
                lrDiagramLink.Pen.Width = 0.1
                lrDiagramLink.HeadPen.Width = 0.1

                lrDiagramLink.SnapToNodeBorder = True
                lrDiagramLink.ShadowColor = Color.White
                lrDiagramLink.Brush = New MindFusion.Drawing.SolidBrush(Drawing.Color.DeepSkyBlue)
                lrDiagramLink.Pen.Color = Drawing.Color.DeepSkyBlue
                lrDiagramLink.Text = Me.SentData(0)
                lrDiagramLink.HeadPen.Color = Drawing.Color.DeepSkyBlue
                lrDiagramLink.AutoRoute = False

                lrDiagramLink.Tag = Me
                Me.Relation.Link = Me
                Me.Relation.Link.Link = lrDiagramLink
                Me.Link = lrDiagramLink

                Call Me.setHeadShapes()

                Me.Page.Diagram.Links.Add(lrDiagramLink)

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Private Sub UpdateFromModel() Handles FactData.ConceptSymbolUpdated
            Try
                Call Me.UpdateGUIFromModel()
            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
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
                    If IsSomething(Me.Link) Then
                        'If Me.Link.Text <> "" Then
                        '    Me.Link.Text = Trim(Me.FactData.Data)
                        '    Call Me.EnableSaveButton()
                        '    Me.Page.Diagram.Invalidate()
                        'End If
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


        Private Sub Fact_Deleted() Handles Fact.Deleted

            If IsSomething(Me.Page.Diagram) Then
                Me.Page.Diagram.Links.Remove(Me.Link)
                Me.FactInstance.FactType.Fact.Remove(Me.FactInstance)
            End If

        End Sub

        Public Sub LinkDeleting() Implements iLinkObject.LinkDeleting

        End Sub

        Public Sub LinkDeselected() Implements iLinkObject.LinkDeslected

            If IsSomething(Me.Link) Then
                Me.Link.Pen.Color = Color.Black
            End If

        End Sub

        Public Sub LinkModified() Implements iLinkObject.LinkModified

        End Sub

        Public Overridable Sub LinkSelected() Implements iLinkObject.LinkSelected

            Try

                If IsSomething(Me.Link) Then
                    Me.Link.Style = LinkStyle.Bezier
                    Me.Link.Pen.Color = Color.Blue
                End If

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try
        End Sub

        Public Overloads Sub MouseDown() Implements iLinkObject.MouseDown

        End Sub

        Public Overloads Sub MouseMove() Implements iLinkObject.MouseMove

        End Sub

        Public Overloads Sub MouseUp() Implements iLinkObject.MouseUp

        End Sub

        Public Overloads Sub Moved() Implements iLinkObject.Moved

        End Sub

        ''' <summary>
        ''' Puts the arrows on the Link depending on various scenarios.
        ''' </summary>
        Public Sub setHeadShapes()

            Try
                'CodeSaf
                If Me.Link Is Nothing Then Exit Sub

                Me.Link.HeadShapeSize = 4
                Me.Link.BaseShapeSize = Me.Link.HeadShapeSize

                If Me.RDSRelation IsNot Nothing Then

                    If Me.RDSRelation.ResponsibleFactType.IsObjectified Or Me.RDSRelation.ResponsibleFactType.IsLinkFactType Then
                        'Set both head shapes to None to start with
                        Me.Link.HeadShape = ArrowHead.None
                        Me.Link.BaseShape = ArrowHead.None

                        Dim lrFactType As FBM.FactType
                        If Me.RDSRelation.ResponsibleFactType.LinkFactTypeRole Is Nothing Then
                            lrFactType = Me.RDSRelation.ResponsibleFactType
                        ElseIf Me.RDSRelation.ResponsibleFactType.LinkFactTypeRole.FactType.Arity = 2 And
                            Me.RDSRelation.ResponsibleFactType.LinkFactTypeRole.FactType.getCorrespondingRDSTable.isPGSRelation Then
                            'Must be binary and a PGS Relation
                            lrFactType = Me.RDSRelation.ResponsibleFactType.LinkFactTypeRole.FactType
                        Else
                            lrFactType = Me.RDSRelation.ResponsibleFactType
                        End If

                        If lrFactType.FactTypeReading.Count = 1 Then
                            Dim lrFactTypeReading = lrFactType.FactTypeReading(0)

                            Dim lrOriginModelObject = Me.Model.GetModelObjectByName(lrFactTypeReading.PredicatePart(0).Role.JoinedORMObject.Id)
                            Dim lrOriginNode = Me.Page.ERDiagram.Entity.Find(Function(x) x.Name = lrOriginModelObject.Id)

                            Dim lrDestinationNode As PGS.Node = Nothing
                            If lrFactType.Arity > 1 Then
                                Dim lrDestinationModelObject = Me.Model.GetModelObjectByName(lrFactTypeReading.PredicatePart(1).Role.JoinedORMObject.Id)
                                lrDestinationNode = Me.Page.ERDiagram.Entity.Find(Function(x) x.Name = lrDestinationModelObject.Id)
                            End If


                            If lrOriginNode Is Nothing Or lrDestinationNode Is Nothing Then Exit Sub

                            If lrFactType.HasTotalRoleConstraint Then
                                Me.Link.BaseShape = ArrowHead.PointerArrow
                                Me.Link.HeadShape = ArrowHead.PointerArrow

                            ElseIf Me.OriginModelElement.Name = lrOriginNode.Name Then
                                Me.Link.BaseShape = ArrowHead.None
                                Me.Link.HeadShape = ArrowHead.PointerArrow
                            Else
                                Me.Link.BaseShape = ArrowHead.PointerArrow
                                Me.Link.HeadShape = ArrowHead.None
                            End If
                        Else
                            If lrFactType.HasTotalRoleConstraint Then
                                Me.Link.BaseShapeSize = Me.Link.HeadShapeSize
                            Else
                                Me.Link.BaseShapeSize = 1.5
                            End If
                            Me.Link.BaseShape = ArrowHead.PointerArrow
                            Me.Link.HeadShape = ArrowHead.PointerArrow

                        End If

                    ElseIf Me.Relation.IsPGSRelationNode Then
                        If Me.RDSRelation.ResponsibleFactType.FactTypeReading.Count > 1 Then
                            Me.Link.BaseShape = ArrowHead.PointerArrow
                            Me.Link.BaseShapeSize = Me.Link.HeadShapeSize
                        End If
                    Else
                        If Me.RDSRelation.ResponsibleFactType.FactTypeReading.Count > 1 Then
                            Me.Link.BaseShape = ArrowHead.PointerArrow
                            Me.Link.BaseShapeSize = 2
                        Else
                            Me.Link.HeadShape = ArrowHead.PointerArrow
                            If Me.RDSRelation.ResponsibleFactType.Is1To1BinaryFactType Then
                                Me.Link.HeadShape = ArrowHead.None
                            End If
                            Me.Link.BaseShape = ArrowHead.None
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

        Public Sub setPredicate()

            Try
                'CodeSafe
                If Me.Link Is Nothing Then Exit Sub

                If Me.Link.Visible = False Then Exit Sub

                '20200902-VM-This should not happen, but if it does then at least get the predicate from the Relation.
                ' NB Has happened on a ternary FactType that is not Objectified.
                If Me.RDSRelation IsNot Nothing Then
                    If Me.RDSRelation.ResponsibleFactType Is Nothing Then
                        Me.Link.Text = Me.Relation.DestinationPredicate
                        Exit Sub
                    End If
                Else
                    Exit Sub
                End If

                If Me.Relation.IsPGSRelationNode Or Me.RDSRelation.ResponsibleFactType.isRDSTable Then
                    '=================================================================
                    'Origin/Destination Predicates
                    Dim lsOriginPredicate As String = ""
                    Dim lsDestinationPredicate As String = ""

                    Dim larRole As New List(Of FBM.Role)
                    Dim lrFactType As FBM.FactType = Nothing
                    Dim lrFactTypeReading As FBM.FactTypeReading = Nothing
                    Dim lsPredicate As String = Nothing

                    Try
                        If Me.RDSRelation.ResponsibleFactType.IsObjectified Or Me.RDSRelation.ResponsibleFactType.IsLinkFactType Then
                            If Me.RDSRelation.ResponsibleFactType.IsLinkFactType Then
                                lrFactType = Me.RDSRelation.ResponsibleFactType.LinkFactTypeRole.FactType
                                If Not lrFactType.getCorrespondingRDSTable().isPGSRelation Then
                                    GoTo SimplePredicate
                                End If
                            Else
                                lrFactType = Me.RDSRelation.ResponsibleFactType
                            End If
                        Else
                            lrFactType = Me.RDSRelation.ResponsibleFactType
                        End If
                    Catch ex As Exception
                        lsPredicate = "Error finding Fact Type for Relation"
                        GoTo SetPredicateNoMatterWhat
                    End Try

                    If lrFactType.FactTypeReading.Count = 0 Then
                        'There is no predicate to set.
                        lsPredicate = "Set a predicate"
                        Me.Link.TextColor = Color.Red
                    ElseIf lrFactType.FactTypeReading.Count = 1 Then
                        lrFactTypeReading = lrFactType.FactTypeReading(0)

                        lsPredicate = lrFactTypeReading.PredicatePart(0).PredicatePartText
                    Else
                        lrFactTypeReading = lrFactType.FactTypeReading(0)
                        Dim lsPredicate1 = lrFactTypeReading.PredicatePart(0).PredicatePartText
                        Dim lsPredicate2 As String = Nothing
                        If lrFactType.FactTypeReading.Count = 2 Then
                            lrFactTypeReading = lrFactType.FactTypeReading(1)
                            lsPredicate2 &= lrFactTypeReading.PredicatePart(0).PredicatePartText
                        End If

                        If lsPredicate2 Is Nothing Then
                            lsPredicate = lsPredicate1
                        Else
                            If Me.Link.Origin.Bounds.X > Me.Link.Destination.Bounds.X Then
                                lsPredicate = lsPredicate1 & " / " & lsPredicate2
                            Else
                                lsPredicate = lsPredicate2 & " / " & lsPredicate1
                            End If

                        End If

                        '20200714-VM-Not yet implemented.
                    End If

                    Dim lrRDSTable As RDS.Table
                    If Me.Page.ERDiagram.Entity.Find(Function(x) x.Name = lrFactType.Id) Is Nothing Then
                        Try
                            lrRDSTable = lrFactTypeReading.FactType.getCorrespondingRDSTable(Nothing, True)
                        Catch ex As Exception
                            lsPredicate = ""
                            Exit Sub
                        End Try
                        If lrRDSTable Is Nothing Then
                            lsPredicate = ""
                            Exit Sub
                        End If

                    Else
                        lrRDSTable = Me.Page.ERDiagram.Entity.Find(Function(x) x.Name = lrFactType.Id).getCorrespondingRDSTable
                    End If

                    For Each lrColumn In lrRDSTable.Column.FindAll(Function(x) Not x.isPartOfPrimaryKey Or x.Role.JoinedORMObject.GetType = GetType(FBM.ValueType))
                        '============================================================
                        'If lrColumn.ContributesToPrimaryKey And lrRDSTable.Column.Count > 1 Then
                        '    'Don't show the Column
                        'Else
                        If lrColumn.Relation.FindAll(Function(x) x.OriginTable.Name = lrColumn.Table.Name).Count > 0 Then
                            'ForeignKey. Don't show the Column
                        Else
                            lsPredicate &= " {" & lrColumn.Name
                            If lrRDSTable.Column.Count > 1 Then lsPredicate &= ",..."
                            lsPredicate &= "}"
                            Exit For
                        End If
                        '============================================================
                    Next
SetPredicateNoMatterWhat:
                    Me.Link.Text = lsPredicate


                    'larRole.Add(Me.RDSRelation.ResponsibleFactType.RoleGroup(0)) 'NB Is opposite to the way you would think, because ER Diagrams read predicates at the opposite end of the Relation
                    'larRole.Add(Me.RDSRelation.ResponsibleFactType.RoleGroup(1))

                    'lrFactTypeReading = Me.RDSRelation.ResponsibleFactType.FindSuitableFactTypeReadingByRoles(larRole, True)

                    'If lrFactTypeReading IsNot Nothing Then
                    '    lsDestinationPredicate = lrFactTypeReading.PredicatePart(0).PredicatePartText & " " & lrFactTypeReading.PredicatePart(1).PreBoundText
                    'Else
                    '    lsDestinationPredicate = "unknown predicate"
                    'End If

                    'larRole.Clear()
                    'larRole.Add(Me.RDSRelation.ResponsibleFactType.RoleGroup(1)) 'NB Is opposite to the way you would think, because ER Diagrams read predicates at the opposite end of the Relation
                    'larRole.Add(Me.RDSRelation.ResponsibleFactType.RoleGroup(0))

                    'lrFactTypeReading = Me.RDSRelation.ResponsibleFactType.FindSuitableFactTypeReadingByRoles(larRole, True)

                    'If lrFactTypeReading IsNot Nothing Then
                    '    lsOriginPredicate = lrFactTypeReading.PredicatePart(0).PredicatePartText & " " & lrFactTypeReading.PredicatePart(1).PreBoundText
                    'Else
                    '    lsOriginPredicate = "unknown predicate"
                    'End If

                    'Dim lasUnknownPredicate = {"unknown predicate"}

                    'If Me.Link.Origin.Bounds.X < Me.Link.Destination.Bounds.X Then
                    '    If lasUnknownPredicate.Contains(lsOriginPredicate) And lasUnknownPredicate.Contains(lsDestinationPredicate) Then
                    '        Me.Link.Text = "<create an ORM Fact Type Reading>"
                    '    ElseIf lasUnknownPredicate.Contains(lsOriginPredicate) Then
                    '        Me.Link.Text = lsDestinationPredicate
                    '    ElseIf lasUnknownPredicate.Contains(lsDestinationPredicate) Then
                    '        Me.Link.Text = lsOriginPredicate
                    '    Else
                    '        Me.Link.Text = lsOriginPredicate & " / " & lsDestinationPredicate
                    '    End If
                    'Else
                    '    If lasUnknownPredicate.Contains(lsOriginPredicate) And lasUnknownPredicate.Contains(lsDestinationPredicate) Then
                    '        Me.Link.Text = "<create an ORM Fact Type Reading>"
                    '    ElseIf lasUnknownPredicate.Contains(lsDestinationPredicate) Then
                    '        Me.Link.Text = lsOriginPredicate
                    '    ElseIf lasUnknownPredicate.Contains(lsOriginPredicate) Then
                    '        Me.Link.Text = lsDestinationPredicate
                    '    Else
                    '        Me.Link.Text = lsDestinationPredicate & " / " & lsOriginPredicate
                    '    End If
                    'End If
                Else
SimplePredicate:
                    Me.Link.Text = Me.RDSRelation.DestinationPredicate
                End If

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Private Sub RDSRelation_RemovedFromModel() Handles RDSRelation.RemovedFromModel

            Try
                'CodeSafe
                If Me.Page Is Nothing Then Exit Sub
                If Me.Page.Diagram Is Nothing Then Exit Sub

                Me.Page.Diagram.Links.Remove(Me.Link)

                Dim lrDiagramingLink As MindFusion.Diagramming.DiagramLink = Me.Link
                If lrDiagramingLink IsNot Nothing Then
                    Me.Page.Diagram.Links.Remove(lrDiagramingLink)
                    lrDiagramingLink.Dispose()
                End If

                Me.Page.Diagram.Invalidate()

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Private Sub RDSRelation_ResponsibleFactTypeFactTypeReadingModified() Handles RDSRelation.ResponsibleFactTypeFactTypeReadingModified

            Try
                Call Me.setPredicate()
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