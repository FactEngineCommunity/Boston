Imports MindFusion.Diagramming
Imports System.ComponentModel
Imports System.Xml.Serialization
Imports System.Reflection

Namespace ERD
    <Serializable()> _
    Public Class Link
        Inherits FBM.FactDataInstance
        Implements ERD.iLinkObject

        <XmlAttribute()> _
        Public Shadows ConceptType As pcenumConceptType = pcenumConceptType.Link

        Public FactInstance As FBM.FactInstance
        Public Shadows WithEvents Fact As FBM.Fact

        Public OriginModelElement As ERD.Entity
        Public DestinationModelElement As ERD.Entity

        Public Relation As ERD.Relation

        Public SentData As New List(Of String)

        Public Link As ERD.tERDLink
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


            If IsSomething(aoOriginModelElement) Then
                Me.OriginModelElement = New ERD.Entity
                Me.OriginModelElement = aoOriginModelElement
            End If

            If IsSomething(aoDestinationModelElement) Then
                Me.DestinationModelElement = New ERD.Entity
                Me.DestinationModelElement = aoDestinationModelElement
            End If

            If IsSomething(arRelation) Then
                Me.Relation = New ERD.Relation
                Me.Relation = arRelation
                Me.Relation.Link = New ERD.Link
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

            Dim lrERDLink As New ERD.tERDLink(Me.Page.Diagram)

            lrERDLink.Origin = Me.OriginModelElement.TableShape
            lrERDLink.Destination = Me.DestinationModelElement.TableShape
            lrERDLink.OriginIndex = -1
            lrERDLink.DestinationIndex = -1
            lrERDLink.Style = LinkStyle.Polyline 'Changed to Cascading in ERD.tERDLink.Draw

            lrERDLink.SnapToNodeBorder = True
            lrERDLink.ShadowColor = Color.White
            lrERDLink.Style = LinkStyle.Bezier
            lrERDLink.SegmentCount = 1
            lrERDLink.HeadShape = ArrowHead.Arrow
            lrERDLink.HeadShapeSize = 2
            lrERDLink.Brush = New MindFusion.Drawing.SolidBrush(Color.Black)
            lrERDLink.Text = Me.SentData(0)
            lrERDLink.Visible = True

            lrERDLink.ERDLink = New ERD.Link
            lrERDLink.ERDLink = Me
            Me.Link = lrERDLink
            lrERDLink.Tag = Me

            Me.Page.Diagram.Links.Add(lrERDLink)

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

    End Class

End Namespace