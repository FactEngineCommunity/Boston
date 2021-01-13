Imports System.ComponentModel
Imports System.Xml.Serialization
Imports MindFusion.Diagramming
Imports System.Reflection

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

        <XmlAttribute()>
        Public Shadows ConceptType As pcenumConceptType = pcenumConceptType.State

        <CategoryAttribute("State"),
         DefaultValueAttribute(GetType(String), ""),
         DescriptionAttribute("Name of the State (Value).")>
        Public Shadows _Name As String = ""
        Public Property StateName() As String
            Get
                Return Me._Name
            End Get
            Set(ByVal Value As String)
                Me._Name = Value
            End Set
        End Property

        Public ValueType As FBM.ValueType = Nothing

        Public STMState As FBM.STM.State = Nothing

        Public Overloads Function Equals(other As State) As Boolean Implements IEquatable(Of State).Equals

            If (Me.Role.Id = other.Role.Id) And (Me.Concept.Symbol = other.Concept.Symbol) Then
                Return True
            Else
                Return False
            End If

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
                loDroppedNode.HandlesStyle = HandlesStyle.InvisibleMove
                loDroppedNode.Resize(20, 15)
                loDroppedNode.AllowIncomingLinks = True
                loDroppedNode.AllowOutgoingLinks = True
                loDroppedNode.Text = Me.Data
                loDroppedNode.Transparent = False

                loDroppedNode.Tag = New STD.State
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

    End Class

End Namespace