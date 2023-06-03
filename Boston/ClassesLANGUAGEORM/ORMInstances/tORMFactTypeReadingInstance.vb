Imports System.ComponentModel
Imports MindFusion.Diagramming
Imports System.Xml.Serialization
Imports System.Reflection

Namespace FBM
    <Serializable()> _
    Public Class FactTypeReadingInstance
        Inherits FBM.FactTypeReading
        Implements FBM.iPageObject

        <XmlAttribute()>
        Public Shadows ConceptType As pcenumConceptType = pcenumConceptType.FactTypeReading

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

        <XmlIgnore()>
        Public Page As FBM.Page

        Private _InstanceNumber As Integer = 1
        Public Property InstanceNumber As Integer Implements iPageObject.InstanceNumber
            Get
                Return Me._InstanceNumber
            End Get
            Set(value As Integer)
                Me._InstanceNumber = value
            End Set
        End Property

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

        Private _Visible As Boolean = True
        Public Property Visible As Boolean Implements iPageObject.Visible
            Get
                Return Me._Visible
            End Get
            Set(value As Boolean)
                Me._Visible = value
                If Me.Shape IsNot Nothing Then
                    Me.Shape.Visible = value
                End If
            End Set
        End Property

        ''' <summary>
        ''' Parameterless Constructor
        ''' </summary>
        Public Sub New()
        End Sub

        Sub New(ByRef arFactTypeInstance As FBM.FactTypeInstance, _
                Optional ByRef arFactTypeReading As FBM.FactTypeReading = Nothing)

            Try
                Me.Model = arFactTypeInstance.Model
                Me.Page = arFactTypeInstance.Page
                Me.FactType = arFactTypeInstance
                Me.InstanceNumber = arFactTypeInstance.InstanceNumber

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
                liX = Me.FactType.FactTypeReadingPoint.X 'Shape.Bounds.X
                liY = Me.FactType.FactTypeReadingPoint.Y 'Shape.Bounds.Y

                'For Y = liY + (StringSize.Height + 8)
                loDroppedNode = Me.Page.Diagram.Factory.CreateShapeNode(liX, liY, StringSize.Width, StringSize.Height, MindFusion.Diagramming.Shapes.Rectangle)
                loDroppedNode.HandlesStyle = HandlesStyle.InvisibleMove
                loDroppedNode.Text = Trim(lsDottedReadingText)
                loDroppedNode.TextColor = Color.Black ' 20230603-VM-Was Color.Blue
                loDroppedNode.Transparent = True
                loDroppedNode.ZTop()
                loDroppedNode.Tag = Me

                '---------------------------------------------------------------------------
                'Attach the FactTypeName ShapeNode to the FactTypeInstance ShapeNode
                '---------------------------------------------------------------------------
                loDroppedNode.AttachTo(Me.FactType.Shape, AttachToNode.MiddleRight)

                If Me.FactType.isPreferredReferenceMode Then
                    Dim lbReferenceSchemeVisible As Boolean = False
                    Try
                        Dim lrEntityType As FBM.EntityType = Me.RoleList.Find(Function(x) x.JoinedORMObject.GetType = GetType(FBM.EntityType)).JoinedORMObject
                        Dim lrEntityTypeInstance As FBM.EntityTypeInstance = Me.Page.EntityTypeInstance.Find(Function(x) x.Id = lrEntityType.Id)
                        lbReferenceSchemeVisible = lrEntityTypeInstance.ExpandReferenceMode
                    Catch ex As Exception
                        'Not a biggie
                    End Try

                    loDroppedNode.Visible = lbReferenceSchemeVisible
                Else
                    loDroppedNode.Visible = True
                End If

                Me.Shape = loDroppedNode

                'CodeSafe
                If Not Me.FactType.ShapeIsWithinRadius(Me.ShapeMidPoint, 40) And Me.FactType.Visible Then
                    Call Me.Move(Me.FactType.X, Me.FactType.Y + StringSize.Height + 8, False)
                End If

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

                    GetDottedReadingText = Trim(lsFirstReadingText) & " / " & Trim(lsSecondReadingText)
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

                    If Me.Page.Diagram IsNot Nothing Then Me.Page.Diagram.Invalidate()
                End If

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
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

        Public Shadows Sub Move(ByVal aiNewX As Integer, ByVal aiNewY As Integer, ByVal abBroadcastInterfaceEvent As Boolean) Implements iPageObject.Move

            Me.X = aiNewX
            Me.Y = aiNewY
            Me.FactType.FactTypeReadingPoint = New Point(aiNewX, aiNewY)

            If Me.Shape IsNot Nothing Then
                Me.Shape.Move(aiNewX, aiNewY)
            End If

            Me.makeDirty()
            Me.Page.MakeDirty()

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
                lrConceptInstance.InstanceNumber = Me.InstanceNumber

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

        Public Function ShapeMidPoint() As Point Implements iPageObject.ShapeMidPoint
            If Me.Shape IsNot Nothing Then
                Dim liMidX As Integer = Me.Shape.Bounds.X + (Me.Shape.Bounds.Width / 2)
                Dim liMidY As Integer = Me.Shape.Bounds.Y + (Me.Shape.Bounds.Height / 2)
                Return New Point(liMidX, liMidY)
            Else
                Return New Point(0, 0)
            End If
        End Function

    End Class

End Namespace