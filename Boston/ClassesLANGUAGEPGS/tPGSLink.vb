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

        Public WithEvents Relation As ERD.Relation

        Public RDSRelation As RDS.Relation

        Public SentData As New List(Of String)

        Public Link As MindFusion.Diagramming.DiagramLink
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
                Me.OriginModelElement = New PGS.Node
                Me.OriginModelElement = aoOriginModelElement
            End If

            If IsSomething(aoDestinationModelElement) Then
                Me.DestinationModelElement = New PGS.Node
                Me.DestinationModelElement = aoDestinationModelElement
            End If

            If IsSomething(arRelation) Then
                Me.Relation = New ERD.Relation
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
                Dim lrPGSLink As New DiagramLink(Me.Page.Diagram, Me.OriginModelElement.shape, Me.DestinationModelElement.shape)

                lrPGSLink.Style = LinkStyle.Bezier

                lrPGSLink.SnapToNodeBorder = True
                lrPGSLink.ShadowColor = Color.White
                lrPGSLink.Brush = New MindFusion.Drawing.SolidBrush(Drawing.Color.DeepSkyBlue)
                lrPGSLink.Pen.Color = Drawing.Color.DeepSkyBlue
                lrPGSLink.Text = Me.SentData(0)
                lrPGSLink.HeadPen.Color = Drawing.Color.DeepSkyBlue
                lrPGSLink.AutoRoute = False

                lrPGSLink.Tag = Me
                Me.Link = lrPGSLink

                If Me.RDSRelation IsNot Nothing Then
                    If Me.Relation.IsPGSRelationNode Then
                        Me.Link.BaseShape = ArrowHead.PointerArrow
                        Me.Link.BaseShapeSize = Me.Link.HeadShapeSize
                    End If
                End If

                Me.Page.Diagram.Links.Add(lrPGSLink)

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Private Sub UpdateFromModel() Handles FactData.ConceptSymbolUpdated

            Call Me.UpdateGUIFromModel()

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
                        '    Call Me.Page.Form.EnableSaveButton()
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

            Me.Link.Style = LinkStyle.Bezier

            If IsSomething(Me.Link) Then
                Me.Link.Pen.Color = Color.Blue
            End If
        End Sub

        Public Sub MouseDown() Implements iLinkObject.MouseDown

        End Sub

        Public Sub MouseMove() Implements iLinkObject.MouseMove

        End Sub

        Public Sub MouseUp() Implements iLinkObject.MouseUp

        End Sub

        Public Sub Moved() Implements iLinkObject.Moved

        End Sub

        Public Sub setPredicate()

            Try
                If Me.Relation.IsPGSRelationNode Then
                    '=================================================================
                    'Origin/Destination Predicates
                    Dim lsOriginPredicate As String = ""
                    Dim lsDestinationPredicate As String = ""

                    Dim larRole As New List(Of FBM.Role)
                    Dim lrFactTypeReading As FBM.FactTypeReading

                    larRole.Add(Me.RDSRelation.ResponsibleFactType.RoleGroup(0)) 'NB Is opposite to the way you would think, because ER Diagrams read predicates at the opposite end of the Relation
                    larRole.Add(Me.RDSRelation.ResponsibleFactType.RoleGroup(1))

                    lrFactTypeReading = Me.RDSRelation.ResponsibleFactType.FindSuitableFactTypeReadingByRoles(larRole, True)

                    If lrFactTypeReading IsNot Nothing Then
                        lsDestinationPredicate = lrFactTypeReading.PredicatePart(0).PredicatePartText & " " & lrFactTypeReading.PredicatePart(1).PreBoundText
                    Else
                        lsDestinationPredicate = "unknown predicate"
                    End If

                    larRole.Clear()
                    larRole.Add(Me.RDSRelation.ResponsibleFactType.RoleGroup(1)) 'NB Is opposite to the way you would think, because ER Diagrams read predicates at the opposite end of the Relation
                    larRole.Add(Me.RDSRelation.ResponsibleFactType.RoleGroup(0))

                    lrFactTypeReading = Me.RDSRelation.ResponsibleFactType.FindSuitableFactTypeReadingByRoles(larRole, True)

                    If lrFactTypeReading IsNot Nothing Then
                        lsOriginPredicate = lrFactTypeReading.PredicatePart(0).PredicatePartText & " " & lrFactTypeReading.PredicatePart(1).PreBoundText
                    Else
                        lsOriginPredicate = "unknown predicate"
                    End If

                    If Me.Link.Origin.Bounds.X < Me.Link.Destination.Bounds.X Then
                        Me.Link.Text = lsOriginPredicate & " / " & lsDestinationPredicate
                    Else
                        Me.Link.Text = lsDestinationPredicate & " / " & lsOriginPredicate
                    End If
                Else
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

    End Class

End Namespace