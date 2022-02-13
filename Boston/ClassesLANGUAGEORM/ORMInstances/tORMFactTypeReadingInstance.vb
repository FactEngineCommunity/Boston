Imports System.ComponentModel
Imports MindFusion.Diagramming
Imports System.Xml.Serialization
Imports System.Reflection

Namespace FBM
    <Serializable()> _
    Public Class FactTypeReadingInstance
        Inherits FBM.FactTypeReading
        Implements FBM.iPageObject

        'The FactTypeReading for which the FactTypeReadingInstance acts as View/Proxy.
        <XmlIgnore()> _
        Private WithEvents _FactTypeReading As New FBM.FactTypeReading
        <XmlIgnore()> _
        <Browsable(False)> _
        Public Property FactTypeReading() As FBM.FactTypeReading
            Get
                Return Me._FactTypeReading
            End Get
            Set(ByVal value As FBM.FactTypeReading)
                Me._FactTypeReading = value
            End Set
        End Property

        <XmlIgnore()> _
        Public Shadows FactType As FBM.FactTypeInstance   'The FactType to which the FactTypeReading relates

        <XmlIgnore()> _
        Public Page As FBM.Page

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


        <XmlIgnore()>
        Private _X As Integer
        Public Property X As Integer Implements FBM.iPageObject.X
            Get
                If Me.Shape IsNot Nothing Then
                    Return Me.Shape.Bounds.X
                Else
                    Return Me._X
                End If
            End Get
            Set(value As Integer)
                Me._X = value
            End Set
        End Property

        <XmlIgnore()>
        Private _Y As Integer
        Public Property Y As Integer Implements FBM.iPageObject.Y
            Get
                If Me.Shape IsNot Nothing Then
                    Return Me.Shape.Bounds.Y
                Else
                    Return Me._Y
                End If
            End Get
            Set(value As Integer)
                Me._Y = value
            End Set
        End Property

        Public Sub New()

        End Sub

        Sub New(ByRef arFactTypeInstance As FBM.FactTypeInstance, _
                Optional ByRef arFactTypeReading As FBM.FactTypeReading = Nothing)

            Try
                Me.Model = arFactTypeInstance.Model
                Me.Page = arFactTypeInstance.Page
                Me.FactType = arFactTypeInstance

                If arFactTypeReading Is Nothing Then
                    Me.Id = System.Guid.NewGuid.ToString
                Else
                    Me.Id = arFactTypeReading.Id
                End If

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Public Overloads Function Clone(ByRef arPage As FBM.Page) As Object

            Return New FBM.FactTypeReadingInstance

        End Function

        Public Sub DisplayAndAssociate()

            Try
                Dim lsDottedReadingText As String = ""
                Dim loDroppedNode As ShapeNode

                Dim StringSize As New SizeF
                Dim G As Graphics

                'CodeSafe
                If Me.FactType Is Nothing Then Exit Sub
                If Me.FactType.Shape Is Nothing Then Exit Sub
                If Me.Page.Form Is Nothing Then Exit Sub

                G = Me.Page.Form.CreateGraphics
                lsDottedReadingText = Me.GetDottedReadingText

                StringSize = Me.Page.Diagram.MeasureString(Trim(lsDottedReadingText), Me.Page.Diagram.Font, 1000, System.Drawing.StringFormat.GenericDefault)

                Dim liX, liY As Integer
                liX = Me.FactType.Shape.Bounds.X
                liY = Me.FactType.Shape.Bounds.Y

                loDroppedNode = Me.Page.Diagram.Factory.CreateShapeNode(liX, liY + (StringSize.Height + 5), StringSize.Width, StringSize.Height, MindFusion.Diagramming.Shapes.Rectangle)
                loDroppedNode.HandlesStyle = HandlesStyle.InvisibleMove
                loDroppedNode.Text = Trim(lsDottedReadingText)
                loDroppedNode.TextColor = Color.Blue
                loDroppedNode.Transparent = True
                loDroppedNode.ZTop()
                loDroppedNode.Tag = Me

                '---------------------------------------------------------------------------
                'Attach the FactTypeName ShapeNode to the FactTypeInstance ShapeNode
                '---------------------------------------------------------------------------
                loDroppedNode.AttachTo(Me.FactType.Shape, AttachToNode.MiddleRight)

                If Me.FactType.isPreferredReferenceMode Then
                    loDroppedNode.Visible = False
                Else
                    loDroppedNode.Visible = True
                End If

                Me.Shape = loDroppedNode

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Public Overrides Function GetDottedReadingText() As String

            Try
                '----------------------------------------------------
                'Create the dotted reading from the PredicateParts
                '----------------------------------------------------
                Dim liSequenceNr As Integer = 0
                Dim lrPredicatePart As FBM.PredicatePart

                GetDottedReadingText = ""

                If Me.PredicatePart.Count = 0 Then Return ""

                If Me.FactType Is Nothing Then Return ""

                If (Me.FactType.Arity = 2) And Me.FactType.FactType.FactTypeReading.Count = 2 Then

                    Dim lsReadingText As String = ""
                    Dim lsFirstReadingText As String = ""
                    Dim lsSecondReadingText As String = ""

                    For Each lrFactTypeReading In Me.FactType.FactType.FactTypeReading
                        lsReadingText = ""

                        lsReadingText &= lrFactTypeReading.FrontText
                        For Each lrPredicatePart In lrFactTypeReading.PredicatePart
                            liSequenceNr += 1
                            lsReadingText &= lrPredicatePart.PreBoundText
                            lsReadingText &= lrPredicatePart.PostBoundText
                            lsReadingText &= Trim(lrPredicatePart.PredicatePartText)
                            If liSequenceNr >= 1 Then
                                lsReadingText &= " "
                            End If
                        Next
                        If lrFactTypeReading.FollowingText <> "" Then
                            lsReadingText &= "... " & lrFactTypeReading.FollowingText
                        End If

                        If Me.FactType.RoleGroup(0).Id = lrFactTypeReading.PredicatePart(0).RoleId Then
                            lsFirstReadingText = lsReadingText
                        Else
                            lsSecondReadingText = lsReadingText
                        End If
                    Next

                    GetDottedReadingText = lsFirstReadingText & " / " & lsSecondReadingText
                Else
                    If (Me.FactType.RoleGroup(0).Id <> Me.PredicatePart(0).RoleId) Then
                        GetDottedReadingText = Chr(171)
                    End If

                    GetDottedReadingText &= Me.FrontText

                    For Each lrPredicatePart In Me.PredicatePart
                        liSequenceNr += 1
                        GetDottedReadingText &= lrPredicatePart.PreBoundText
                        GetDottedReadingText &= "..."
                        GetDottedReadingText &= lrPredicatePart.PostBoundText
                        GetDottedReadingText &= Trim(lrPredicatePart.PredicatePartText)
                        If liSequenceNr >= 1 Then
                            GetDottedReadingText &= " "
                        End If
                    Next

                    GetDottedReadingText &= Me.FollowingText
                End If

                If Me.FactType.IsDerived Then
                    GetDottedReadingText &= " *"
                End If

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)

                Return Nothing
            End Try

        End Function

        Public Sub RefreshShape(Optional ByVal aoChangedPropertyItem As PropertyValueChangedEventArgs = Nothing,
                                Optional ByVal asSelectedGridItemLabel As String = "")

            Try
                If IsSomething(Me.Shape) Then
                    Me.Shape.Text = ""
                    If (Me.FactType.Arity = 2) And Me.PredicatePart.Count > 0 Then
                        If (Me.FactType.RoleGroup(0).Id <> Me.PredicatePart(0).RoleId) Then
                            Me.Shape.Text = Chr(171)
                        End If
                    End If

                    Me.Shape.Text &= Me.GetDottedReadingText
                    Me.Shape.ResizeToFitText(FitSize.KeepRatio)
                    Me.Page.Diagram.Invalidate()
                End If

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Public Sub MouseDown() Implements iPageObject.MouseDown

        End Sub

        Public Sub MouseMove() Implements iPageObject.MouseMove

        End Sub

        Public Sub MouseUp() Implements iPageObject.MouseUp

        End Sub

        Public Sub Moved() Implements iPageObject.Moved

        End Sub

        Public Sub NodeDeleting() Implements iPageObject.NodeDeleting

        End Sub

        Public Sub NodeDeselected() Implements iPageObject.NodeDeselected

        End Sub

        Public Sub NodeModified() Implements iPageObject.NodeModified

        End Sub

        Public Sub NodeSelected() Implements iPageObject.NodeSelected

        End Sub

        Public Sub RepellNeighbouringPageObjects(ByVal aiDepth As Integer) Implements iPageObject.RepellNeighbouringPageObjects

        End Sub

        Public Sub SetAppropriateColour() Implements iPageObject.SetAppropriateColour

        End Sub

        Public Overrides Sub makeDirty()
            MyBase.makeDirty()
            Me.isDirty = True
            Me.FactType.isDirty = True
            Me.Page.IsDirty = True
            Me.Model.IsDirty = True
        End Sub

        Public Sub Move(ByVal aiNewX As Integer, ByVal aiNewY As Integer, ByVal abBroadcastInterfaceEvent As Boolean) Implements iPageObject.Move

            Me.X = aiNewX
            Me.Y = aiNewY
            Me.FactType.FactTypeReadingPoint = New Point(Me.X, Me.Y)

        End Sub

        Public Sub EnableSaveButton() Implements iPageObject.EnableSaveButton
            Throw New NotImplementedException()
        End Sub

        Public Overloads Sub Save(Optional ByVal abRapidSave As Boolean = False)

            '-----------------------------------------
            'Saves the EntityInstance to the database
            '-----------------------------------------
            Dim lrConceptInstance As New FBM.ConceptInstance

            Try
                lrConceptInstance.ModelId = Me.Model.ModelId
                lrConceptInstance.PageId = Me.Page.PageId
                lrConceptInstance.Symbol = Me.FactType.Id
                lrConceptInstance.X = Me.X
                lrConceptInstance.Y = Me.Y
                lrConceptInstance.ConceptType = pcenumConceptType.FactTypeReading

                '--------------------------------------------------
                'Make sure the new Symbol is in the Concept table
                '--------------------------------------------------
                Dim lrConcept As New FBM.Concept(Me.FactType.Id)
                lrConcept.Save()

                If abRapidSave Then
                    Call TableConceptInstance.AddConceptInstance(lrConceptInstance)
                Else
                    If TableConceptInstance.ExistsConceptInstance(lrConceptInstance, False) Then
                        Call TableConceptInstance.UpdateConceptInstance(lrConceptInstance)
                    Else
                        Call TableConceptInstance.AddConceptInstance(lrConceptInstance)
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


    End Class

End Namespace