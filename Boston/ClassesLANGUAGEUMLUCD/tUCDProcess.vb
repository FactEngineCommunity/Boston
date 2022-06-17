Imports MindFusion.Diagramming
Imports System.ComponentModel
Imports System.Xml.Serialization
Imports System.Reflection

Namespace UCD
    <Serializable()>
    Public Class Process
        Inherits UML.Process
        Implements FBM.iPageObject

        <XmlAttribute()>
        Public Shadows ConceptType As pcenumConceptType = pcenumConceptType.Process

        Public Shadows WithEvents CMMLProcess As CMML.Process

        <XmlIgnore()>
        <CategoryAttribute("Process"),
        Browsable(True),
        [ReadOnly](True),
        DescriptionAttribute("The unique Process Id of the Process.")>
        Public Overloads ReadOnly Property ProcessId As String
            Get
                Return Me.Id
            End Get
        End Property

        '20220617-VM-Remove if not needed.
        '''' <summary>
        '''' The text of the Process
        '''' </summary>
        'Public Text As String

        'Public Shadows Page As FBM.Page

        'Public include_process As List(Of CMML.Process)
        'Public included_by_process As List(Of CMML.Process)
        'Public extend_to_process As List(Of CMML.Process)
        'Public extended_by_process As List(Of CMML.Process)

        '''' <summary>
        '''' The SequenceNr assigned to the Process in a sequence of Processes in (say) a FlowChart or EventTraceDiagram
        '''' </summary>
        '''' <remarks></remarks>
        'Public SequenceNr As Single = 1

        'Public Shadows IsDecision As Boolean = False
        'Public IsStart As Boolean = False
        'Public IsStop As Boolean = False

        '''' <summary>
        '''' The Actor responsible for the process, as in (say) an EventTraceDiagram
        '''' </summary>
        '''' <remarks></remarks>
        'Public ResponsibleActor As CMML.Actor

        '20220617-VM-Not really used.
        '<CategoryAttribute("Process"),
        '     DefaultValueAttribute(GetType(String), ""),
        '     DescriptionAttribute("Name of the Process.")>
        'Public Property ProcessName() As String
        '    Get
        '        Return Me.Name
        '    End Get
        '    Set(ByVal Value As String)
        '        Me.Name = Value
        '    End Set
        'End Property

        Public Shadows Event FactChanged(ByRef arFact As FBM.Fact)

        Public Sub New()
            Me.Id = System.Guid.NewGuid.ToString
        End Sub

        Public Sub New(ByRef arPage As FBM.Page, ByVal asGUID As String, ByVal asProcessId As String, ByVal asProcessText As String)
            Call MyBase.New

            Me.Page = arPage
            Me.Model = arPage.Model
            Me.FactData.Model = arPage.Model
            Me.Text = asProcessText

        End Sub

        Public Sub New(ByRef arPage As FBM.Page, ByRef arRoleInstance As FBM.RoleInstance, ByRef ar_concept As FBM.Concept)
            '---------------------------------------------------
            'NB Arguments are by Ref, because need to point to 
            '  actual objects on a Page.
            '---------------------------------------------------
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
            Me.X = aiX
            Me.Y = aiY

        End Sub

        Public Shadows Function EqualsByName(ByVal other As CMML.Process) As Boolean

            If other.Name Like (Me.Name) Then
                Return True
            Else
                Return False
            End If

        End Function

        Public Shared Function CompareSequenceNrs(ByVal aoA As CMML.Process, ByVal aoB As CMML.Process) As Integer

            '------------------------------------------------------
            'Used as a delegate within 'SortRoleGroup'
            '------------------------------------------------------
            Dim loa As New Object
            Dim lob As New Object

            Try
                Return aoA.SequenceNr - aoB.SequenceNr

            Catch ex As Exception
                prApplication.ThrowErrorMessage(ex.Message, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Function

        Public Sub DisplayAndAssociate(Optional ByRef aoContainerNode As ContainerNode = Nothing)

            Dim loDroppedNode As New ShapeNode

            Try
                '--------------------------------------------------------------------
                'Create a Shape for the EntityTypeInstance on the DiagramView object
                '--------------------------------------------------------------------            
                loDroppedNode = Me.Page.Diagram.Factory.CreateShapeNode(Me.X, Me.Y, 2, 2)
                loDroppedNode.HandlesStyle = HandlesStyle.InvisibleMove ''HatchHandles3 is a very professional look, or SquareHandles2
                loDroppedNode.ToolTip = "process"
                loDroppedNode.AllowOutgoingLinks = True
                loDroppedNode.AllowIncomingLinks = True
                loDroppedNode.Text = Me.Text 'arProcessInstance.Name            
                loDroppedNode.Tag = New FBM.EntityTypeInstance 'loDroppedNode.Tag = New Object
                loDroppedNode.Tag = Me
                loDroppedNode.Obstacle = True
                Me.Shape = loDroppedNode

                Select Case Me.Page.Language
                    Case Is = pcenumLanguage.ORMModel
                        'Can delete this later. At present, if the MetaModel is shown as an ORM diagram, the Page.Language changes to ORMModel
                        ' then if you drop a Process on the Page, the Process has no default shape
                        loDroppedNode.Shape = Shapes.Ellipse
                        loDroppedNode.Resize(20, 15)
                        loDroppedNode.Brush = New MindFusion.Drawing.SolidBrush(Color.Beige)
                    Case Is = pcenumLanguage.DataFlowDiagram
                        loDroppedNode.Shape = Shapes.Ellipse
                        loDroppedNode.Resize(20, 15)
                        loDroppedNode.Brush = New MindFusion.Drawing.SolidBrush(Color.Beige)
                    Case Is = pcenumLanguage.UMLUseCaseDiagram
                        loDroppedNode.Shape = Shapes.Ellipse
                        loDroppedNode.HandlesStyle = HandlesStyle.Invisible
                        loDroppedNode.Resize(40, 12)
                    Case Is = pcenumLanguage.FlowChart
                        If Me.IsDecision Then
                            loDroppedNode.Shape = Shapes.Decision
                        ElseIf Me.IsStart Then
                            loDroppedNode.Shape = Shapes.Ellipse
                        ElseIf Me.IsStop Then
                            loDroppedNode.Shape = Shapes.Ellipse
                        End If
                        loDroppedNode.Resize(20, 15)
                End Select

                If IsSomething(aoContainerNode) Then
                    aoContainerNode.Add(loDroppedNode)
                End If

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Public Overloads Sub Move(ByVal aiNewX As Integer, ByVal aiNewY As Integer, ByVal abBroadcastInterfaceEvent As Boolean) Implements FBM.iPageObject.Move

            Me.X = aiNewX
            Me.Y = aiNewY

            Me.FactDataInstance.X = aiNewX
            Me.FactDataInstance.Y = aiNewY

            Me.FactDataInstance.Fact.FactType.isDirty = True
            Me.FactDataInstance.Fact.isDirty = True
            Me.FactDataInstance.isDirty = True

            Try
                Me.FactDataInstance.Page.MakeDirty()
            Catch ex As Exception

            End Try

        End Sub

        Public Overrides Function RemoveFromPage() As Boolean

            Try
                Dim lsSQLQuery As String

                '----------------------------------------------------------------------------------------------------------
                'Remove the Process that represents the Process from the Diagram on the Page.
                '-------------------------------------------------------------------------------
                Me.Page.UMLDiagram.Process.Remove(Me)

                Dim larLinkToRemove As New List(Of DiagramLink)

                If Me.Page.Diagram IsNot Nothing Then

                    Me.Page.Diagram.Nodes.Remove(Me.Shape)

                    For Each lrLink In Me.Shape.IncomingLinks
                        larLinkToRemove.Add(lrLink)
                    Next

                    For Each lrLink In Me.Shape.OutgoingLinks
                        larLinkToRemove.Add(lrLink)
                    Next

                    For Each lrLink In larLinkToRemove
                        Me.Page.Diagram.Links.Remove(lrLink)
                    Next
                End If

                '-------------------------------------------------------------------------
                'Remove the Process from the Page
                '---------------------------------
#Region "CMML"
                'Likely already deleted when deleted at the Model level.
                lsSQLQuery = " DELETE FROM " & pcenumCMMLRelations.CoreElementHasElementType.ToString
                lsSQLQuery &= " ON PAGE '" & Me.Page.Name & "'"
                lsSQLQuery &= " WHERE Element = '" & Me.Id & "'"
                lsSQLQuery &= "   AND ElementType = 'Process'"

                Call Me.Page.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)
#End Region

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Function


        Public Overloads Sub RefreshShape(Optional ByVal aoChangedPropertyItem As PropertyValueChangedEventArgs = Nothing)

            Dim lsMessage As String = ""

            Try
                If IsSomething(aoChangedPropertyItem) Then
                    Select Case aoChangedPropertyItem.ChangedItem.PropertyDescriptor.Name
                        Case Is = "Name"

                            If Me.Page.ExistsDataStore(Me.Name) Then
                                lsMessage = "You cannot set the name of a Process as the same as the name of a DataStore in the Model."
                                lsMessage &= vbCrLf & vbCrLf
                                lsMessage &= "A DataStore with the name, '" & Me.Name & "', already exists in the Model"

                                Me.Name = Me.Data

                                MsgBox(lsMessage, MsgBoxStyle.Exclamation)
                            Else
                                Me.Shape.Text = Me.Name
                                Call Me.setName(Me.Name)
                            End If
                    End Select
                End If

            Catch ex As Exception
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Public Sub SetProcessText(ByVal asNewProcessText As String)

            Try
                Me.Text = asNewProcessText

                'CMML
                Call Me.Model.updateCMMLProcessText(Me)

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        ''' <summary>
        ''' Sets the size of the Process proportional to the number of incomming Links.
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub SetSizeProportionalToInputs(Optional ByRef aoContainerNode As ContainerNode = Nothing)

            Select Case Me.Shape.IncomingLinks.Count
                Case Is < 2
                    Me.Shape.Resize(20, 15)
                Case 3 To 5
                    Me.Shape.Resize(30, 20)
                Case 6 To 10
                    Me.Shape.Resize(40, 30)
                Case Is >= 10
                    Me.Shape.Resize(60, 60)
            End Select

            If IsSomething(aoContainerNode) Then
                aoContainerNode.AutoShrink = True
                aoContainerNode.Resize(Viev.Greater(Me.Shape.Bounds.Width + 20, aoContainerNode.Bounds.Width), Viev.Greater(Me.Shape.Bounds.Height + 20, aoContainerNode.Bounds.Height))
            End If

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

        Friend Shadows Sub UpdateGUIFromModel()

            '---------------------------------------------------------------------
            'Linked by Delegate in New to the 'update' event of the ModelObject 
            '  referenced by Objects of this Class
            '---------------------------------------------------------------------
            Try

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
                            Me.Shape.Text = Trim(Me.FactData.Data)
                            Call Me.EnableSaveButton()
                            Me.Page.Diagram.Invalidate()
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

        Public Sub setFact(ByRef arFact As FBM.Fact)

            Try

                Me.Fact = arFact

                RaiseEvent FactChanged(arFact)

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Public Overloads Sub NodeDeselected() Implements FBM.iPageObject.NodeDeselected

            Call Me.SetAppropriateColour()
        End Sub

        Public Overloads Sub SetAppropriateColour() Implements FBM.iPageObject.SetAppropriateColour

            Try
                If IsSomething(Me.Shape) Then
                    If Me.Shape.Selected Then
                        Me.Shape.Pen.Color = Color.Blue
                    Else
                        Me.Shape.Pen.Color = Color.Black
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

        Private Sub CMMLProcess_RemovedFromModel() Handles CMMLProcess.RemovedFromModel

            Try
                Call Me.RemoveFromPage()

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