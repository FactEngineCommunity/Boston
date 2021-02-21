Imports System.ComponentModel
Imports System.Xml.Serialization
Imports MindFusion.Diagramming
Imports System.Reflection
Imports Boston.FBM

Namespace STD

    <Serializable()>
    Public Class State
        '--------------------------------------------------------------------------------------------------------
        'Represents Data that represents a State (e.g. Within a StateTransitionDiagram)
        '  The Data is FactData within a Fact within a FactType.
        '  e.g. "Shipped" may be a 'State' of Merchant Goods in a StateTransitionDiagram that describes
        '  the 'State' of an Order of those Merchant Goods.
        '  - The MetaModel for StateTransitionDiagrams has a FactType, the Facts of which hold various 'States'.
        '  In the StateTransitionDiagram MetaModel, that FactType is called: 'StateTransitionRelation'.
        '--------------------------------------------------------------------------------------------------------
        Inherits FBM.FactDataInstance
        Implements IEquatable(Of STD.State)
        Implements FBM.iPageObject

        <XmlAttribute()>
        Public Shadows ConceptType As pcenumConceptType = pcenumConceptType.State

        <CategoryAttribute("State"),
         DefaultValueAttribute(GetType(String), ""),
         DescriptionAttribute("Name of the State (Value).")>
        Public Shadows _Name As String = ""
        <XmlAttribute()>
        <CategoryAttribute("State Name"),
         DescriptionAttribute("The 'Name' for the State")>
        Public Property StateName() As String
            Get
                Return Me._Name
            End Get
            Set(ByVal Value As String)
                Me._Name = Value
            End Set
        End Property

        Public ValueType As FBM.ValueType = Nothing

        Public ReadOnly Property IsEndState As Boolean
            Get
                Return Me.STMState.IsStop
            End Get
        End Property

        Public ReadOnly Property IsStartState As Boolean
            Get
                Return Me.STMState.IsStart
            End Get
        End Property

        Private Property iPageObject_X As Integer Implements iPageObject.X
            Get
                Throw New NotImplementedException()
            End Get
            Set(value As Integer)
                Throw New NotImplementedException()
            End Set
        End Property

        Private Property iPageObject_Y As Integer Implements iPageObject.Y
            Get
                Throw New NotImplementedException()
            End Get
            Set(value As Integer)
                Throw New NotImplementedException()
            End Set
        End Property

        Public WithEvents STMState As FBM.STM.State = Nothing

        Public Sub New(ByRef arPage As FBM.Page)
            Me.Page = arPage
            Me.FactDataInstance = New FBM.FactDataInstance(Me.Page)
        End Sub


        Public Overloads Function Equals(other As State) As Boolean Implements IEquatable(Of State).Equals

            Return Me.Id = other.Id
            'If (Me.Role.Id = other.Role.Id) And (Me.Concept.Symbol = other.Concept.Symbol) Then
            '    Return True
            'Else
            '    Return False
            'End If

        End Function

        Public Sub DisplayAndAssociate()

            Try
                Dim loDroppedNode As ShapeNode = Nothing
                Dim StringSize As New SizeF

                StringSize = Me.Page.Diagram.MeasureString(Trim(Me.Name), Me.Page.Diagram.Font, 1000, System.Drawing.StringFormat.GenericDefault)

                '=====================================================================
                '----------------------------------------------
                'Create a TableNode on the Page for the Entity
                '----------------------------------------------
                loDroppedNode = Me.Page.Diagram.Factory.CreateShapeNode(Me.X, Me.Y, 2, 2)
                loDroppedNode.Shape = Shapes.RoundRect
                loDroppedNode.Pen.Width = 0.4
                loDroppedNode.HandlesStyle = HandlesStyle.InvisibleMove
                loDroppedNode.Resize(20, 15)
                loDroppedNode.AllowIncomingLinks = True
                loDroppedNode.AllowOutgoingLinks = True
                loDroppedNode.Text = Me.StateName
                loDroppedNode.Transparent = False

                loDroppedNode.Tag = Me
                Me.Shape = loDroppedNode


                ''loDroppedNode = Me.Page.Diagram.Factory.CreateTableNode(Me.X, Me.Y, 20, 2, 1, 0)
                'loDroppedNode = New ERD.TableNode
                'Me.Page.Diagram.Nodes.Add(loDroppedNode)
                'loDroppedNode.Resize(20, 2)
                'loDroppedNode.ColumnCount = 1
                'loDroppedNode.RowCount = 0
                'Me.Page.Diagram.Nodes.Add(loDroppedNode)
                'loDroppedNode.Move(Me.X, Me.Y)
                'loDroppedNode.HandlesStyle = HandlesStyle.Invisible
                'loDroppedNode.Pen.Width = 0.5

                'If Me.Page.Language = pcenumLanguage.PropertyGraphSchema Then
                '    If Me.NodeType = pcenumPGSEntityType.Node Then
                '        loDroppedNode.Pen.Color = Color.Brown
                '    Else
                '        loDroppedNode.Pen.Color = Color.DarkGray
                '    End If
                'Else
                '    loDroppedNode.Pen.Color = Color.Black
                'End If

                'loDroppedNode.Brush = New SolidBrush(Color.White)
                'loDroppedNode.ShadowColor = Color.LightGray
                'loDroppedNode.EnableStyledText = True
                'loDroppedNode.CellFrameStyle = CellFrameStyle.None
                'loDroppedNode.ConnectionStyle = TableConnectionStyle.Both
                'loDroppedNode.Expandable = False
                'loDroppedNode.Obstacle = True
                'loDroppedNode.Style = TableStyle.RoundedRectangle
                'loDroppedNode.Resize(StringSize.Width + 5, 15)
                'loDroppedNode.AllowIncomingLinks = True
                'loDroppedNode.AllowOutgoingLinks = True
                'loDroppedNode.Caption = "<B>" & " " & Me.FactDataInstance.Data & " "
                'loDroppedNode.Tag = Me
                'loDroppedNode.ResizeToFitText(False)
                'Me.FactDataInstance.TableShape = loDroppedNode
                'Me.TableShape = loDroppedNode

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Public Sub RefreshShape(Optional ByVal aoChangedPropertyItem As PropertyValueChangedEventArgs = Nothing,
                                Optional ByVal asSelectedGridItemLabel As String = "")

            If IsSomething(aoChangedPropertyItem) Then
                Select Case aoChangedPropertyItem.ChangedItem.PropertyDescriptor.Name
                    Case Is = "StateName"
                        Call Me.STMState.setName(Me.StateName)
                End Select
            End If

        End Sub

        Public Sub setEndState(ByVal abEndStateStatus As Boolean)

            Call Me.STMState.setEndState(abEndStateStatus)

        End Sub

        Public Sub setStartState(ByVal abStartStateStatus As Boolean)

            If abStartStateStatus Then

                Call Me.STMState.makeStartState()

            End If

        End Sub

        Private Sub STMState_IsStartStateChanged(abIsStartState As Boolean) Handles STMState.IsStartStateChanged

            Dim lsSQLQuery As String

            If abIsStartState Then

                lsSQLQuery = "ADD FACT '" & Me.STMState.StartStateFact.Id & "'"
                lsSQLQuery &= " TO " & pcenumCMMLRelations.CoreValueTypeHasStartCoreElementState.ToString
                lsSQLQuery &= " ON PAGE '" & Me.Page.Name & "'"

                Call Me.Page.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)
            Else

            End If

        End Sub

        Public Sub MouseDown() Implements iPageObject.MouseDown
            Throw New NotImplementedException()
        End Sub

        Public Sub MouseMove() Implements iPageObject.MouseMove
            Throw New NotImplementedException()
        End Sub

        Public Sub MouseUp() Implements iPageObject.MouseUp
            Throw New NotImplementedException()
        End Sub

        Public Sub NodeDeleting() Implements iPageObject.NodeDeleting
            Throw New NotImplementedException()
        End Sub

        Public Sub NodeDeselected() Implements iPageObject.NodeDeselected
            Throw New NotImplementedException()
        End Sub

        Public Sub NodeModified() Implements iPageObject.NodeModified
            Throw New NotImplementedException()
        End Sub

        Public Sub NodeSelected() Implements iPageObject.NodeSelected
            Call Me.SetAppropriateColour()
        End Sub

        Public Sub Move(aiNewX As Integer, aiNewY As Integer, abBroadcastInterfaceEvent As Boolean) Implements iPageObject.Move

            Me.X = aiNewX
            Me.Y = aiNewY

            Me.FactDataInstance.X = aiNewX
            Me.FactDataInstance.Y = aiNewY

            Me.FactDataInstance.makeDirty()

        End Sub

        Public Sub Moved() Implements iPageObject.Moved
            Throw New NotImplementedException()
        End Sub

        Public Sub RepellNeighbouringPageObjects(aiDepth As Integer) Implements iPageObject.RepellNeighbouringPageObjects
            Throw New NotImplementedException()
        End Sub

        Public Sub SetAppropriateColour() Implements iPageObject.SetAppropriateColour

            If IsSomething(Me.Shape) Then
                If Me.Shape.Selected Then
                    Me.Shape.Pen.Color = Color.Blue
                Else
                    If Me.ValueType.HasModelError Then
                        Me.Shape.Pen.Color = Color.Red
                    Else
                        Me.Shape.Pen.Color = Color.Black
                    End If
                End If

                If Me.Page.Diagram IsNot Nothing Then
                    Me.Page.Diagram.Invalidate()
                End If

            End If

        End Sub

        Private Sub STMState_NameChanged(asNewName As String) Handles STMState.NameChanged

            Me.StateName = asNewName

            Me.Shape.Text = asNewName

        End Sub

        Private Sub STMState_FactChanged(ByRef arFact As Fact) Handles STMState.FactChanged

            Try
                'Remove the original Fact from the FactType(Instance)
                Me.Fact.FactType.RemoveFactById(Me.Fact.Fact)

                'Change the Fact
                Me.Fact = Me.Fact.FactType.AddFact(arFact)
                Me.FactData = arFact("Element")
                Me.FactDataInstance = Me.Fact("Element")

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Public Overrides Function RemoveFromPage() As Boolean

            Try

                Call Me.Fact.FactType.RemoveFact(Me.Fact)


            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Function

    End Class

End Namespace