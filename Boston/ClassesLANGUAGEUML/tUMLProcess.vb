Imports MindFusion.Diagramming
Imports System.ComponentModel
Imports System.Xml.Serialization
Imports System.Reflection

Namespace UML
    <Serializable()>
    Public Class Process
        Inherits FBM.FactDataInstance
        Implements FBM.iPageObject

        <XmlAttribute()>
        Public Shadows ConceptType As pcenumConceptType = pcenumConceptType.Process

        Public UMLModel As UML.Model

        Public Shadows WithEvents CMMLProcess As CMML.Process

        <XmlIgnore()>
        <CategoryAttribute("Process"),
        Browsable(True),
        [ReadOnly](True),
        DescriptionAttribute("The unique Process Id of the Process.")>
        Public ReadOnly Property ProcessId As String
            Get
                Return Me.Id
            End Get
        End Property

        ''' <summary>
        ''' The text of the Process
        ''' </summary>
        Public _Text As String
        <CategoryAttribute("Process"),
         DefaultValueAttribute(GetType(String), ""),
         DescriptionAttribute("Text of the Process.")>
        Public Property Text() As String
            Get
                Return Me._Text
            End Get
            Set(ByVal Value As String)
                Me._Text = Value
            End Set
        End Property

        Public Shadows Page As FBM.Page

        Public include_process As List(Of CMML.Process)
        Public included_by_process As List(Of CMML.Process)
        Public extend_to_process As List(Of CMML.Process)
        Public extended_by_process As List(Of CMML.Process)

        ''' <summary>
        ''' The SequenceNr assigned to the Process in a sequence of Processes in (say) a FlowChart or EventTraceDiagram
        ''' </summary>
        ''' <remarks></remarks>
        Public SequenceNr As Single = 1

        Public Shadows IsDecision As Boolean = False
        Public IsStart As Boolean = False
        Public IsStop As Boolean = False

        ''' <summary>
        ''' The Actor responsible for the process, as in (say) an EventTraceDiagram
        ''' </summary>
        ''' <remarks></remarks>
        Public ResponsibleActor As UML.Actor

        Public Event FactChanged(ByRef arFact As FBM.Fact)

        Public Sub New()
            Me.Id = System.Guid.NewGuid.ToString
        End Sub

        Public Sub New(ByRef arPage As FBM.Page, ByVal asProcessId As String, ByVal asProcessText As String)
            Call MyBase.New

            Me.Id = asProcessId
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

        Public Overloads Function CloneUCDProcess(ByRef arPage As FBM.Page) As UCD.Process

            Dim lrProcess As New UCD.Process

            Try
                With Me
                    lrProcess.Model = .Model
                    lrProcess.UMLModel = .UMLModel
                    lrProcess.Page = arPage
                    lrProcess.Id = .Id
                    lrProcess.Text = .Text
                    lrProcess.CMMLProcess = .CMMLProcess
                    lrProcess.ConceptType = pcenumConceptType.Process 'While this is redundant, it seems that it is required for Polymorphic use under tEntity
                    lrProcess.FactData = Me.FactData
                    lrProcess.Name = .Concept.Symbol
                    lrProcess.Symbol = .Data
                    lrProcess.FactDataInstance = Me.FactDataInstance
                    lrProcess.JoinedObjectType = Me.Role.JoinedORMObject
                    lrProcess.Concept = .Concept
                    lrProcess.Role = .Role
                    lrProcess.X = .X
                    lrProcess.Y = .Y
                End With

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

            Return lrProcess

        End Function



        Public Shadows Function EqualsByName(ByVal other As UML.Process) As Boolean

            If other.Name Like (Me.Name) Then
                Return True
            Else
                Return False
            End If

        End Function

        Public Shared Function CompareSequenceNrs(ByVal aoA As UML.Process, ByVal aoB As UML.Process) As Integer

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
            '--------------------------------------------------------------------
            'Create a Shape for the EntityTypeInstance on the DiagramView object
            '--------------------------------------------------------------------            
            loDroppedNode = Me.Page.Diagram.Factory.CreateShapeNode(Me.X, Me.Y, 2, 2)
            loDroppedNode.HandlesStyle = HandlesStyle.Invisible ''HatchHandles3 is a very professional look, or SquareHandles2
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

        End Sub

        Public Overloads Sub Move(ByVal aiNewX As Integer, ByVal aiNewY As Integer, ByVal abBroadcastInterfaceEvent As Boolean) Implements FBM.iPageObject.Move

            Me.X = aiNewX
            Me.Y = aiNewY

            Try

                Me.FactDataInstance.X = aiNewX
                Me.FactDataInstance.Y = aiNewY

                Me.FactDataInstance.Fact.FactType.isDirty = True
                Me.FactDataInstance.Fact.isDirty = True
                Me.FactDataInstance.isDirty = True
                Me.isDirty = True
                Me.Model.MakeDirty(False, False)

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

            Try
                Me.FactDataInstance.Page.MakeDirty()
            Catch ex As Exception
            End Try

        End Sub

        Public Sub RefreshShape(Optional ByVal aoChangedPropertyItem As PropertyValueChangedEventArgs = Nothing,
                                Optional ByVal asSelectedGridItemLabel As String = "")

            Dim lsMessage As String = ""

            Try
                If IsSomething(aoChangedPropertyItem) Then
                    Select Case aoChangedPropertyItem.ChangedItem.PropertyDescriptor.Name
                        Case Is = "Text"

                            If Me.Model.UML.Process.Find(Function(x) x.Id <> Me.Id And Trim(x.Text) = Trim(Me.Text)) IsNot Nothing Then
                                lsMessage = "You cannot set the name of a Process as the same as the name of another Process in the model."
                                lsMessage &= vbCrLf & vbCrLf
                                lsMessage &= "A Process with the tex, '" & Me.Text & "', already exists in the model."

                                Me.Text = Me.CMMLProcess.Text

                                MsgBox(lsMessage, MsgBoxStyle.Exclamation)
                            Else
                                Call Me.CMMLProcess.SetProcessText(Me.Text)
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

        Private Sub CMMLProcess_TextChanged(asNewText As String) Handles CMMLProcess.TextChanged

            Try
                Me.Text = asNewText

                Call Me.RefreshShape()

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