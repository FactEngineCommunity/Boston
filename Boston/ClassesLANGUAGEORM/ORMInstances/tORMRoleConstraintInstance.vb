Imports MindFusion.Diagramming
Imports MindFusion.Drawing
Imports System.Xml.Serialization
Imports System.ComponentModel
Imports System.Drawing.Drawing2D
Imports System.Reflection

Namespace FBM
    <Serializable()> _
    Public Class RoleConstraintInstance
        Inherits FBM.RoleConstraint
        Implements FBM.iPageObject
        Implements IEquatable(Of FBM.RoleConstraintInstance)

        <XmlIgnore()>
        Public WithEvents RoleConstraint As FBM.RoleConstraint


        <CategoryAttribute("Constraint Type"),
        Browsable(True),
        [ReadOnly](True),
        BindableAttribute(True),
        DefaultValueAttribute(""),
        DesignOnly(False),
        DescriptionAttribute("The type of Role Constraint")>
        Public Overrides Property RoleConstraintType() As pcenumRoleConstraintType
            Get
                Return Me._RoleConstraintType
            End Get
            Set(ByVal value As pcenumRoleConstraintType)
                Me._RoleConstraintType = value
            End Set
        End Property

        <System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)>
        <XmlIgnore()>
        <DebuggerBrowsable(DebuggerBrowsableState.Never)>
        Public _IsDeontic As Boolean = False
        <CategoryAttribute("Modality"),
        DefaultValueAttribute(False),
        DescriptionAttribute("The Modality of the Role Constraint.")>
        Public Shadows Property IsDeontic() As Boolean
            Get
                Return Me._IsDeontic
            End Get
            Set(ByVal value As Boolean)
                Me._IsDeontic = value
            End Set
        End Property

        <XmlIgnore()>
        Public Shadows Role As New List(Of FBM.RoleInstance)

        <XmlIgnore()>
        Public Shadows RoleConstraintRole As New List(Of FBM.RoleConstraintRoleInstance)

        ''' <summary>
        ''' True if the user is in the process of creating an Argument for the RoleConstraint, else False.
        ''' </summary>
        ''' <remarks></remarks>
        Public CreatingArgument As Boolean = False 'NB See also: FBM.RoleContraint.CurrentArgument As FBM.RoleConstraintArgument
        'NB See: frmDiagramORM.Diagram.NodeClicked for when starting the argument by effectively dragging from the RoleConstraint/Instance

        <NonSerialized>
        <XmlIgnore()>
        Public Page As FBM.Page

        Public Property X As Integer Implements FBM.iPageObject.X
        Public Property Y As Integer Implements FBM.iPageObject.Y

        Public Height As Integer
        Public Width As Integer

        <NonSerialized()>
        <XmlIgnore()>
        Public Shape As New FBM.RoleConstraintShape

        Public Sub New()
            '-------------------
            'Parameterless New
            '-------------------
        End Sub

        Public Sub New(ByRef arRoleConstraint As FBM.RoleConstraint)

            Me.ConceptType = pcenumConceptType.RoleConstraint

            If IsSomething(arRoleConstraint) Then
                Me.Model = arRoleConstraint.Model
                Me.RoleConstraint = New FBM.RoleConstraint
                Me.RoleConstraint = arRoleConstraint
                Me.Id = arRoleConstraint.Id
            End If

        End Sub


        Public Sub New(ByVal aiRoleConstraintType As pcenumRoleConstraintType)

            Me.New()
            Me.RoleConstraint = New FBM.RoleConstraint
            Select Case aiRoleConstraintType
                Case Is = pcenumRoleConstraintType.ExclusiveORConstraint,
                          pcenumRoleConstraintType.ExternalUniquenessConstraint,
                          pcenumRoleConstraintType.ExclusionConstraint,
                          pcenumRoleConstraintType.FrequencyConstraint,
                          pcenumRoleConstraintType.InclusiveORConstraint,
                          pcenumRoleConstraintType.SubsetConstraint

                    Me.RoleConstraintType = aiRoleConstraintType

                Case Is = pcenumRoleConstraintType.InternalUniquenessConstraint
                    Me.ConceptType = pcenumConceptType.RoleConstraint
                    Me.RoleConstraintType = pcenumRoleConstraintType.InternalUniquenessConstraint
            End Select

        End Sub

        Public Overloads Function Clone(ByRef arPage As FBM.Page,
                                        Optional ByVal abIsMDAModelElement As Boolean = False,
                                        Optional abAddToPage As Boolean = False) As Object

            Dim lrRoleConstraintInstance As New FBM.RoleConstraintInstance
            Dim lrRoleConstraintRoleInstance As FBM.RoleConstraintRoleInstance
            Dim lrClonedRoleConstraintRoleInstance As FBM.RoleConstraintRoleInstance

            Try

                With Me
                    lrRoleConstraintInstance.Model = arPage.Model
                    lrRoleConstraintInstance.Page = arPage
                    lrRoleConstraintInstance.Symbol = .Symbol
                    lrRoleConstraintInstance.Id = .Id
                    If abIsMDAModelElement = False Then
                        lrRoleConstraintInstance.IsMDAModelElement = .IsMDAModelElement
                    Else
                        lrRoleConstraintInstance.IsMDAModelElement = abIsMDAModelElement
                    End If

                    If arPage.Model.RoleConstraint.Exists(AddressOf .RoleConstraint.Equals) Then
                        lrRoleConstraintInstance.RoleConstraint = arPage.Model.RoleConstraint.Find(AddressOf .RoleConstraint.Equals)
                    Else
                        Dim lrRoleConstraint As New FBM.RoleConstraint
                        lrRoleConstraint = .RoleConstraint.Clone(arPage.Model, False, abIsMDAModelElement)
                        arPage.Model.AddRoleConstraint(lrRoleConstraint)
                        lrRoleConstraintInstance.RoleConstraint = lrRoleConstraint
                    End If

                    lrRoleConstraintInstance.Name = .Name
                    lrRoleConstraintInstance.RoleConstraintType = .RoleConstraintType

                    For Each lrRoleConstraintRoleInstance In .RoleConstraintRole
                        lrClonedRoleConstraintRoleInstance = New FBM.RoleConstraintRoleInstance
                        lrClonedRoleConstraintRoleInstance = lrRoleConstraintRoleInstance.Clone(arPage)
                        lrClonedRoleConstraintRoleInstance.RoleConstraint = lrRoleConstraintInstance
                        lrRoleConstraintInstance.RoleConstraintRole.Add(lrClonedRoleConstraintRoleInstance)
                        lrRoleConstraintInstance.Role.Add(lrClonedRoleConstraintRoleInstance.Role)
                    Next

                    lrRoleConstraintInstance.IsDeontic = .IsDeontic
                    lrRoleConstraintInstance.IsPreferredIdentifier = .IsPreferredIdentifier
                    lrRoleConstraintInstance.Cardinality = .Cardinality
                    lrRoleConstraintInstance.CardinalityRangeType = .CardinalityRangeType
                    lrRoleConstraintInstance.LevelNr = .LevelNr
                    lrRoleConstraintInstance.ShortDescription = .ShortDescription
                    lrRoleConstraintInstance.LongDescription = .LongDescription
                    lrRoleConstraintInstance.MaximumFrequencyCount = .MaximumFrequencyCount
                    lrRoleConstraintInstance.MinimumFrequencyCount = .MinimumFrequencyCount

                    lrRoleConstraintInstance.X = .X
                    lrRoleConstraintInstance.Y = .Y

                    If abAddToPage Then
                        arPage.RoleConstraintInstance.AddUnique(lrRoleConstraintInstance)
                    End If
                End With

                Return lrRoleConstraintInstance

            Catch ex As Exception
                Dim lsMessage As String = ""

                lsMessage = "Error: FBM.RoleConstraintInstance.Clone: " & vbCrLf & vbCrLf & ex.Message
                Call prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)

                Return lrRoleConstraintInstance
            End Try

        End Function

        Public Overloads Function CloneFrequencyConstraintInstance(ByRef arPage As FBM.Page) As FBM.FrequencyConstraint

            Dim lrFrequencyConstraintInstance As New FBM.FrequencyConstraint
            Dim lrRoleConstraintRoleInstance As FBM.RoleConstraintRoleInstance
            Dim lrClonedRoleConstraintRoleInstance As FBM.RoleConstraintRoleInstance

            Try
                With Me
                    lrFrequencyConstraintInstance.Model = arPage.Model
                    lrFrequencyConstraintInstance.Page = arPage
                    lrFrequencyConstraintInstance.Symbol = .Symbol
                    lrFrequencyConstraintInstance.Id = .Id
                    lrFrequencyConstraintInstance.ConceptType = .ConceptType
                    lrFrequencyConstraintInstance.RoleConstraintType = pcenumRoleConstraintType.FrequencyConstraint
                    lrFrequencyConstraintInstance.RoleConstraint = New FBM.RoleConstraint
                    lrFrequencyConstraintInstance.RoleConstraint = arPage.Model.RoleConstraint.Find(AddressOf .RoleConstraint.Equals)
                    lrFrequencyConstraintInstance.Name = .Name
                    lrFrequencyConstraintInstance.IsDeontic = .IsDeontic
                    lrFrequencyConstraintInstance.IsPreferredIdentifier = .IsPreferredIdentifier
                    lrFrequencyConstraintInstance.Cardinality = .Cardinality
                    lrFrequencyConstraintInstance.CardinalityRangeType = .CardinalityRangeType
                    lrFrequencyConstraintInstance.LevelNr = .LevelNr
                    lrFrequencyConstraintInstance.ShortDescription = .ShortDescription
                    lrFrequencyConstraintInstance.LongDescription = .LongDescription
                    lrFrequencyConstraintInstance.MaximumFrequencyCount = .MaximumFrequencyCount
                    lrFrequencyConstraintInstance.MinimumFrequencyCount = .MinimumFrequencyCount
                    lrFrequencyConstraintInstance.X = .X
                    lrFrequencyConstraintInstance.Y = .Y

                    For Each lrRoleConstraintRoleInstance In .RoleConstraintRole
                        lrClonedRoleConstraintRoleInstance = New FBM.RoleConstraintRoleInstance
                        lrClonedRoleConstraintRoleInstance = lrRoleConstraintRoleInstance.Clone(arPage)
                        lrClonedRoleConstraintRoleInstance.RoleConstraint = lrFrequencyConstraintInstance
                        lrFrequencyConstraintInstance.RoleConstraintRole.Add(lrClonedRoleConstraintRoleInstance)
                        lrFrequencyConstraintInstance.Role.Add(lrClonedRoleConstraintRoleInstance.Role)
                    Next

                End With

                Return lrFrequencyConstraintInstance

            Catch ex As Exception

                Dim lsMessage As String
                lsMessage = "Error: tRoleConstraintInstance.CloneFrequencyConstraintInstance"
                lsMessage &= vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)

                Return lrFrequencyConstraintInstance
            End Try
        End Function

        Public Overloads Function CloneUniquenessConstraintInstance(ByRef arPage As FBM.Page, Optional ByVal abAddToPage As Boolean = False) As tUniquenessConstraint

            Dim lrRoleConstraintInstance As New tUniquenessConstraint
            Dim lrRoleConstraintRoleInstance As FBM.RoleConstraintRoleInstance
            Dim lrClonedRoleConstraintRoleInstance As FBM.RoleConstraintRoleInstance

            Try

                With Me
                    lrRoleConstraintInstance.Model = .Model
                    lrRoleConstraintInstance.Page = arPage
                    lrRoleConstraintInstance.Symbol = .Symbol
                    lrRoleConstraintInstance.Id = .Id
                    lrRoleConstraintInstance.ConceptType = .ConceptType
                    lrRoleConstraintInstance.RoleConstraintType = .RoleConstraintType
                    lrRoleConstraintInstance.RoleConstraint = arPage.Model.RoleConstraint.Find(AddressOf .RoleConstraint.Equals)
                    lrRoleConstraintInstance.Name = .Name
                    lrRoleConstraintInstance.IsDeontic = .IsDeontic
                    lrRoleConstraintInstance.IsPreferredIdentifier = .IsPreferredIdentifier
                    lrRoleConstraintInstance.Cardinality = .Cardinality
                    lrRoleConstraintInstance.CardinalityRangeType = .CardinalityRangeType
                    lrRoleConstraintInstance.LevelNr = .LevelNr
                    lrRoleConstraintInstance.ShortDescription = .ShortDescription
                    lrRoleConstraintInstance.LongDescription = .LongDescription
                    lrRoleConstraintInstance.MinimumFrequencyCount = .MinimumFrequencyCount
                    lrRoleConstraintInstance.MaximumFrequencyCount = .MaximumFrequencyCount
                    lrRoleConstraintInstance.X = .X
                    lrRoleConstraintInstance.Y = .Y

                    If abAddToPage Then
                        arPage.RoleConstraintInstance.AddUnique(lrRoleConstraintInstance)
                    End If

                    '-------------------------------------------------------------------------
                    'Associate the RoleInstances to which the RoleConstraintInstance relates
                    '-------------------------------------------------------------------------
                    For Each lrRoleConstraintRoleInstance In .RoleConstraintRole
                        lrClonedRoleConstraintRoleInstance = New FBM.RoleConstraintRoleInstance
                        lrClonedRoleConstraintRoleInstance = lrRoleConstraintRoleInstance.Clone(arPage)
                        lrClonedRoleConstraintRoleInstance.RoleConstraint = lrRoleConstraintInstance
                        lrRoleConstraintInstance.RoleConstraintRole.Add(lrClonedRoleConstraintRoleInstance)
                        lrRoleConstraintInstance.Role.Add(lrClonedRoleConstraintRoleInstance.Role)
                    Next

                End With

                Return lrRoleConstraintInstance

            Catch ex As Exception

                Dim lsMessage As String
                lsMessage = "Error: tRoleConstraintInstance.CloneUniquenessConstraintInstance"
                lsMessage &= vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)

                Return lrRoleConstraintInstance
            End Try

        End Function

        Public Overloads Function CloneRingConstraintInstance(ByRef arPage As FBM.Page) As RingConstraint

            Dim lrRingConstraintInstance As New RingConstraint
            Dim lrRoleConstraintRoleInstance As FBM.RoleConstraintRoleInstance
            Dim lrClonedRoleConstraintRoleInstance As FBM.RoleConstraintRoleInstance

            Try
                With Me
                    lrRingConstraintInstance.Model = arPage.Model
                    lrRingConstraintInstance.Page = arPage
                    lrRingConstraintInstance.Symbol = .Symbol
                    lrRingConstraintInstance.Id = .Id
                    lrRingConstraintInstance.ConceptType = .ConceptType
                    lrRingConstraintInstance.RoleConstraintType = pcenumRoleConstraintType.RingConstraint
                    lrRingConstraintInstance.RoleConstraint = arPage.Model.RoleConstraint.Find(AddressOf .RoleConstraint.Equals)
                    lrRingConstraintInstance.RingConstraintType = .RingConstraintType
                    lrRingConstraintInstance.Name = .Name
                    lrRingConstraintInstance.RoleConstraintType = .RoleConstraintType

                    For Each lrRoleConstraintRoleInstance In .RoleConstraintRole
                        lrClonedRoleConstraintRoleInstance = New FBM.RoleConstraintRoleInstance
                        lrClonedRoleConstraintRoleInstance = lrRoleConstraintRoleInstance.Clone(arPage)
                        lrRingConstraintInstance.RoleConstraintRole.Add(lrClonedRoleConstraintRoleInstance)
                        lrRingConstraintInstance.Role.Add(lrClonedRoleConstraintRoleInstance.Role)
                    Next

                    lrRingConstraintInstance.IsDeontic = .IsDeontic
                    lrRingConstraintInstance.IsPreferredIdentifier = .IsPreferredIdentifier
                    lrRingConstraintInstance.Cardinality = .Cardinality
                    lrRingConstraintInstance.CardinalityRangeType = .CardinalityRangeType
                    lrRingConstraintInstance.LevelNr = .LevelNr
                    lrRingConstraintInstance.ShortDescription = .ShortDescription
                    lrRingConstraintInstance.LongDescription = .LongDescription
                    lrRingConstraintInstance.MaximumFrequencyCount = .MaximumFrequencyCount
                    lrRingConstraintInstance.MinimumFrequencyCount = .MinimumFrequencyCount

                    lrRingConstraintInstance.X = .X
                    lrRingConstraintInstance.Y = .Y
                End With

                Return lrRingConstraintInstance

            Catch ex As Exception

                Dim lsMessage As String
                lsMessage = "Error: FBM.RoleConstraintInstance.CloneRingConstraintInstance"
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)

                Return lrRingConstraintInstance
            End Try
        End Function

        Public Function CloneConceptInstance() As FBM.ConceptInstance

            Dim lrConceptInstance As New FBM.ConceptInstance(Me.Model, Me.Page, Me.Id, Me.ConceptType)

            lrConceptInstance.X = Me.X
            lrConceptInstance.Y = Me.Y

            Return lrConceptInstance

        End Function

        Public Function JoinsSubtypingRelationship() As Boolean

            If Me.RoleConstraintRole.FindAll(Function(x) x.SubtypeConstraintInstance IsNot Nothing).Count > 0 Then
                Return True
            Else
                Return False
            End If

        End Function

        Public Sub MouseDown() Implements FBM.iPageObject.MouseDown

        End Sub

        Public Sub MouseMove() Implements FBM.iPageObject.MouseMove

        End Sub

        Public Sub MouseUp() Implements FBM.iPageObject.MouseUp

        End Sub

        Public Sub Moved() Implements FBM.iPageObject.Moved

        End Sub

        Public Sub NodeDeleting() Implements FBM.iPageObject.NodeDeleting

        End Sub

        Public Sub NodeModified() Implements FBM.iPageObject.NodeModified

        End Sub

        Public Sub NodeSelected() Implements FBM.iPageObject.NodeSelected

            Try

                If Me.Shape IsNot Nothing Then
                    Me.Shape.Pen.Color = Color.Blue
                    Me.Shape.Transparent = False
                    Me.Shape.AllowOutgoingLinks = True
                End If

                Call Me.RefreshRoleConstraintRoleTexts()

                'Dim lrRoleConstraintRole As FBM.RoleConstraintRole
                'Dim lrRoleInstance As FBM.RoleInstance

                'If Me.RoleConstraint.Argument.Count > 0 Then
                '    For Each lrRoleConstraintRole In Me.RoleConstraint.RoleConstraintRole
                '        lrRoleInstance = Me.Page.RoleInstance.Find(Function(x) x.Id = lrRoleConstraintRole.Role.Id)
                '        If lrRoleConstraintRole.RoleConstraintArgument IsNot Nothing Then
                '            lrRoleInstance.Shape.Text = lrRoleConstraintRole.RoleConstraintArgument.SequenceNr.ToString & _
                '            "." & _
                '            lrRoleConstraintRole.ArgumentSequenceNr.ToString
                '        End If
                '    Next
                '    Dim lrArgument As FBM.RoleConstraintArgument
                '    Dim lrRole As FBM.Role

                '    For Each lrArgument In Me.RoleConstraint.Argument
                '        If lrArgument.JoinPath IsNot Nothing Then
                '            For Each lrRole In lrArgument.JoinPath.RolePath
                '                lrRoleInstance = Me.Page.RoleInstance.Find(Function(x) x.Id = lrRole.Id)
                '                Dim Values() As Integer = CType([Enum].GetValues(GetType(pcenumColourWheel)), Integer())
                '                lrRoleInstance.Shape.Brush = New MindFusion.Drawing.SolidBrush(Color.FromArgb(Values(lrArgument.SequenceNr Mod 6)))
                '            Next
                '        End If
                '    Next
                'End If

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Public Sub RefreshRoleConstraintRoleTexts()

            Dim lrRoleInstance As FBM.RoleInstance
            'Colours for RoleInstances
            Dim Values() As Integer = CType([Enum].GetValues(GetType(pcenumColourWheel)), Integer())

            If Me.RoleConstraint.Argument.Count > 0 Then
                For Each lrRoleConstraintRole In Me.RoleConstraint.RoleConstraintRole
                    lrRoleInstance = Me.Page.RoleInstance.Find(Function(x) x.Id = lrRoleConstraintRole.Role.Id)
                    If lrRoleInstance IsNot Nothing And lrRoleConstraintRole.RoleConstraintArgument IsNot Nothing Then
                        lrRoleInstance.Shape.Text = lrRoleConstraintRole.RoleConstraintArgument.SequenceNr.ToString &
                        "." &
                        lrRoleConstraintRole.ArgumentSequenceNr.ToString
                    End If
                Next
                Dim lrArgument As FBM.RoleConstraintArgument
                Dim lrRole As FBM.Role

                For Each lrArgument In Me.RoleConstraint.Argument
                    If lrArgument.JoinPath IsNot Nothing Then
                        For Each lrRole In lrArgument.JoinPath.RolePath
                            lrRoleInstance = Me.Page.RoleInstance.Find(Function(x) x.Id = lrRole.Id)
                            If lrRoleInstance IsNot Nothing Then
                                lrRoleInstance.Shape.Brush = New MindFusion.Drawing.SolidBrush(Color.FromArgb(Values(lrArgument.SequenceNr Mod 6)))
                            End If
                        Next
                    End If
                Next

            End If

            If Me.CurrentArgument IsNot Nothing Then
                If Me.CurrentArgument.JoinPath IsNot Nothing Then
                    For Each lrRole In Me.CurrentArgument.JoinPath.RolePath
                        lrRoleInstance = Me.Page.RoleInstance.Find(Function(x) x.Id = lrRole.Id)
                        If lrRoleInstance IsNot Nothing Then
                            lrRoleInstance.Shape.Brush = New MindFusion.Drawing.SolidBrush(Color.FromArgb(Values(Me.CurrentArgument.SequenceNr Mod 6)))
                        End If
                    Next
                End If
            End If

        End Sub

        ''' <summary>
        ''' Clears the Shape.Brush of the Argument specified by its SequenceNr
        ''' </summary>
        ''' <param name="aiArgumentSequenceNr">The SequenceNr of the Argument for which the JoinPath will be cleared (of Brush colour)</param>
        ''' <remarks></remarks>
        Public Sub ClearJoinPathBrushesForArgument(ByVal aiArgumentSequenceNr As Integer)

            Try
                Dim lrRoleInstance As FBM.RoleInstance

                If Me.RoleConstraint.Argument.Count > 0 Then

                    Dim lrArgument As FBM.RoleConstraintArgument

                    lrArgument = Me.RoleConstraint.Argument.Find(Function(x) x.SequenceNr = aiArgumentSequenceNr)
                    If lrArgument.JoinPath IsNot Nothing Then
                        For Each lrRole In lrArgument.JoinPath.RolePath
                            lrRoleInstance = Me.Page.RoleInstance.Find(Function(x) x.Id = lrRole.Id)
                            If lrRoleInstance IsNot Nothing Then
                                lrRoleInstance.Shape.Text = ""
                                lrRoleInstance.Shape.Brush = New MindFusion.Drawing.SolidBrush(Color.White)
                            End If
                        Next
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


        Public Overridable Sub DisplayAndAssociate()

            Dim StringSize As New SizeF
            Dim G As Graphics
            Dim lsMessage As String = ""

            Dim loDroppedNode As FBM.RoleConstraintShape

            Try
                Select Case Me.RoleConstraintType
                    Case Is = pcenumRoleConstraintType.InternalUniquenessConstraint
                        '------------------------------
                        'InternalUniquenessConstraint
                        '------------------------------
                        If Me.Role.Count > 0 Then
                            Call Me.DisplayAndAssociateAsInternalUniquenessConstraint()
                        Else
                            lsMessage = "Tried to DisplayAndAssociate RoleConstraintInstance of RoleConstraintType: InternalUniquenessConstraint, where there are no Roles associated with the RoleConstraintInstance (i.e. RoleConstraint.Role.Count = 0):"
                            lsMessage &= vbCrLf & "RoleConstraint.Id: " & Me.Id
                            Throw New Exception(lsMessage)
                        End If
                    Case Is = pcenumRoleConstraintType.FrequencyConstraint
                        '---------------------
                        'FrequencyConstraint
                        '---------------------
                        Dim lrRoleInstance As New FBM.RoleInstance
                        Dim lsFrequencyConstraintText As String = ""

                        Select Case Me.CardinalityRangeType
                            Case Is = pcenumCardinalityRangeType.LessThanOREqual
                                lsFrequencyConstraintText = "<=" & Me.Cardinality
                            Case Is = pcenumCardinalityRangeType.Equal
                                lsFrequencyConstraintText = Me.Cardinality
                            Case Is = pcenumCardinalityRangeType.GreaterThanOREqual
                                lsFrequencyConstraintText = ">=" & Me.Cardinality
                        End Select

                        '------------------------------------------
                        'Must be associated with one or more Role
                        '------------------------------------------
                        If Me.RoleConstraintRole.Count = 0 Then
                            Throw New Exception("Cannot display and associate a FrequencyConstraint without first populating the RoleConstraintRole group.")
                        End If

                        lrRoleInstance = Me.RoleConstraintRole(0).Role

                        Dim myFont As New Font(lrRoleInstance.Shape.Parent.Font.FontFamily, lrRoleInstance.Shape.Parent.Font.Size, FontStyle.Regular, GraphicsUnit.Pixel)
                        G = frmDiagramORM.CreateGraphics
                        StringSize = lrRoleInstance.Shape.Parent.MeasureString(lsFrequencyConstraintText, lrRoleInstance.Shape.Parent.Font, 1000, System.Drawing.StringFormat.GenericDefault)

                        Dim liX, liY As Integer
                        liX = lrRoleInstance.Shape.Bounds.X
                        liY = lrRoleInstance.Shape.Bounds.Y - (StringSize.Height * 2)

                        loDroppedNode = New FBM.RoleConstraintShape(Me.Page.Diagram, Me)
                        loDroppedNode.Move(liX, liY)
                        loDroppedNode.Shape = MindFusion.Diagramming.Shapes.Ellipse
                        loDroppedNode.Resize(StringSize.Width + 2, StringSize.Height)

                        'Dim loShapeNode As ShapeNode = Me.Page.Diagram.Factory.CreateShapeNode(liX, liY, StringSize.Width + 2, StringSize.Height, Shapes.Rectangle)

                        loDroppedNode.Pen = New MindFusion.Drawing.Pen(Color.White)
                        loDroppedNode.EnabledHandles = AdjustmentHandles.Move
                        loDroppedNode.Transparent = True 'False has faint ring around external constraints, looks ugly.

                        loDroppedNode.HandlesStyle = HandlesStyle.InvisibleMove
                        loDroppedNode.Text = lsFrequencyConstraintText
                        'loDroppedNode.ResizeToFitText(FitSize.KeepHeight)
                        loDroppedNode.ShadowColor = Color.White
                        loDroppedNode.ShadowOffsetX = 0
                        loDroppedNode.ShadowOffsetY = 0
                        loDroppedNode.TextColor = Color.Black
                        'loDroppedNode.Transparent = False
                        loDroppedNode.Visible = True
                        loDroppedNode.ZTop()

                        Me.Page.Diagram.Nodes.Add(loDroppedNode)

                        '--------------------------------------------------------------------------------------
                        'Attach the FrequencyConstraint.Shape to the RoleInstance.Shape to which it joins.
                        '  This is so that if an 'AutoArrange' of the Shapes on the Page is called by
                        '  the user, then the FrequencyConstraint will stay next to its associated Role.
                        '--------------------------------------------------------------------------------------
                        loDroppedNode.AttachTo(lrRoleInstance.Shape, AttachToNode.TopCenter)

                        loDroppedNode.Tag = Me

                        '---------------------------------------------------------------------------
                        'Attach the Role.FrequencyConstraint ShapeNode to the Role ShapeGroup,                                    
                        '---------------------------------------------------------------------------
                        loDroppedNode.AttachTo(lrRoleInstance.Shape, AttachToNode.TopLeft)
                        Me.Shape = loDroppedNode

                        '--------------------------------------------------------------------------------
                        'Create the link between the FrequencyConstraint and the Role that it joins to.
                        '--------------------------------------------------------------------------------
                        Dim lo_link As DiagramLink = New DiagramLink(lrRoleInstance.Shape.Parent, lrRoleInstance.Shape, loDroppedNode)
                        lo_link.BaseShape = ArrowHead.None
                        lo_link.HeadShape = ArrowHead.None
                        lo_link.Pen.DashStyle = Drawing2D.DashStyle.Custom
                        ReDim lo_link.Pen.DashPattern(3)
                        lo_link.Pen.DashPattern(0) = 1
                        lo_link.Pen.DashPattern(1) = 2
                        lo_link.Pen.DashPattern(2) = 1
                        lo_link.Pen.DashPattern(3) = 2

                        lrRoleInstance.Shape.Parent.Links.Add(lo_link)
                    Case Else
                        '------------------------------------------------
                        'ExternalRoleConstraints with links on the Page
                        '------------------------------------------------
                        loDroppedNode = New FBM.RoleConstraintShape(Me.Page.Diagram, Me)
                        loDroppedNode.Move(Me.X, Me.Y)
                        Me.Page.Diagram.Nodes.Add(loDroppedNode)
                        loDroppedNode.Pen = New MindFusion.Drawing.Pen(Color.White)
                        loDroppedNode.HandlesStyle = HandlesStyle.Invisible
                        loDroppedNode.EnabledHandles = AdjustmentHandles.Move
                        loDroppedNode.Shape = MindFusion.Diagramming.Shapes.Ellipse
                        loDroppedNode.Transparent = True 'False has faint ring around external constraints, looks ugly.
                        loDroppedNode.Visible = False
                        loDroppedNode.ImageAlign = ImageAlign.Stretch

                        Select Case Me.RoleConstraintType
                            Case Is = pcenumRoleConstraintType.EqualityConstraint
                                loDroppedNode.Image = My.Resources.ORMShapes.equality
                                loDroppedNode.ToolTip = "Equality Constraint"
                            Case Is = pcenumRoleConstraintType.ExclusionConstraint
                                loDroppedNode.Image = My.Resources.ORMShapes.exclusion
                                loDroppedNode.ToolTip = "Exclusion Constraint"
                            Case Is = pcenumRoleConstraintType.ExclusiveORConstraint
                                loDroppedNode.Image = My.Resources.ORMShapes.exclusiveOr
                                loDroppedNode.ToolTip = "Exclusive OR Constraint"
                            Case Is = pcenumRoleConstraintType.ExternalUniquenessConstraint
                                If (Me.IsDeontic = False) And (Me.IsPreferredIdentifier = False) Then
                                    loDroppedNode.Image = My.Resources.ORMShapes.externalUniqueness
                                ElseIf Me.IsDeontic And (Me.IsPreferredIdentifier = False) Then
                                    loDroppedNode.Image = My.Resources.ORMShapes.deontic_external_uniqueness
                                ElseIf Me.IsDeontic And Me.IsPreferredIdentifier Then
                                    loDroppedNode.Image = My.Resources.ORMShapes.deontic_external_preferred_uniqueness
                                ElseIf (Me.IsDeontic = False) And Me.IsPreferredIdentifier Then
                                    loDroppedNode.Shape.Image = My.Resources.ORMShapes.preferred_uniqueness
                                End If
                                loDroppedNode.ToolTip = "External Uniqueness Constraint"
                            Case Is = pcenumRoleConstraintType.FrequencyConstraint
                                loDroppedNode.Image = Nothing
                                loDroppedNode.ToolTip = "Frequency Constraint"
                            Case Is = pcenumRoleConstraintType.InclusiveORConstraint
                                loDroppedNode.Image = My.Resources.ORMShapes.inclusive_or
                                loDroppedNode.ToolTip = "Inclusive-OR Constraint"
                            Case Is = pcenumRoleConstraintType.RingConstraint
                                Select Case Me.RingConstraintType
                                    Case Is = pcenumRingConstraintType.AcyclicIntransitive
                                        loDroppedNode.Image = My.Resources.ORMShapes.acyclic_intransitive
                                    Case Is = pcenumRingConstraintType.Acyclic
                                        loDroppedNode.Image = My.Resources.ORMShapes.acyclic
                                    Case Is = pcenumRingConstraintType.Antisymmetric
                                        loDroppedNode.Image = My.Resources.ORMShapes.Antisymmetric
                                    Case Is = pcenumRingConstraintType.Asymmetric
                                        loDroppedNode.Image = My.Resources.ORMShapes.Asymmetric
                                    Case Is = pcenumRingConstraintType.AsymmetricIntransitive
                                        loDroppedNode.Image = My.Resources.ORMShapes.asymmetric_intransitive
                                    Case Is = pcenumRingConstraintType.DeonticAcyclic
                                        loDroppedNode.Image = My.Resources.ORMShapes.deontic_acyclic
                                    Case Is = pcenumRingConstraintType.DeonticAcyclicIntransitive
                                        loDroppedNode.Image = My.Resources.ORMShapes.deontic_acyclic_intransitive
                                    Case Is = pcenumRingConstraintType.DeonticAntisymmetric
                                        loDroppedNode.Image = My.Resources.ORMShapes.deontic_antisymmetric
                                    Case Is = pcenumRingConstraintType.DeonticAssymmetric
                                        loDroppedNode.Image = My.Resources.ORMShapes.deontic_asymmetric
                                    Case Is = pcenumRingConstraintType.DeonticAssymmetricIntransitive
                                        loDroppedNode.Image = My.Resources.ORMShapes.deontic_asymmetric_intransitive
                                    Case Is = pcenumRingConstraintType.DeonticIntransitive
                                        loDroppedNode.Image = My.Resources.ORMShapes.deontic_intransitive
                                    Case Is = pcenumRingConstraintType.DeonticIrreflexive
                                        loDroppedNode.Image = My.Resources.ORMShapes.deontic_irreflexive
                                    Case Is = pcenumRingConstraintType.DeonticPurelyReflexive
                                        loDroppedNode.Image = My.Resources.ORMShapes.deontic_purely_reflexive
                                    Case Is = pcenumRingConstraintType.DeonticSymmetric
                                        loDroppedNode.Image = My.Resources.ORMShapes.deontic_symmetric
                                    Case Is = pcenumRingConstraintType.DeonticSymmetricIntransitive
                                        loDroppedNode.Image = My.Resources.ORMShapes.deontic_symmetric_intransitive
                                    Case Is = pcenumRingConstraintType.DeonticSymmetricIrreflexive
                                        loDroppedNode.Image = My.Resources.ORMShapes.deontic_symmetric_irreflexive
                                    Case Is = pcenumRingConstraintType.Intransitive
                                        loDroppedNode.Image = My.Resources.ORMShapes.deontic_intransitive
                                    Case Is = pcenumRingConstraintType.Irreflexive
                                        loDroppedNode.Image = My.Resources.ORMShapes.deontic_irreflexive
                                    Case Is = pcenumRingConstraintType.PurelyReflexive
                                        loDroppedNode.Image = My.Resources.ORMShapes.purely_reflexive
                                    Case Is = pcenumRingConstraintType.Symmetric
                                        loDroppedNode.Image = My.Resources.ORMShapes.symmetric
                                    Case Is = pcenumRingConstraintType.SymmetricIntransitive
                                        loDroppedNode.Image = My.Resources.ORMShapes.symmetric_intransitive
                                    Case Is = pcenumRingConstraintType.SymmetricIrreflexive
                                        loDroppedNode.Image = My.Resources.ORMShapes.symmetric_irreflexive
                                End Select
                                loDroppedNode.ToolTip = "Ring Constraint"
                            Case Is = pcenumRoleConstraintType.SubsetConstraint
                                loDroppedNode.Image = My.Resources.ORMShapes.subset
                                loDroppedNode.ToolTip = "Subset Constraint"
                        End Select


                        loDroppedNode.Resize(7, 7)
                        loDroppedNode.Pen = New MindFusion.Drawing.Pen(Color.White)
                        loDroppedNode.Visible = True
                        loDroppedNode.ImageAlign = ImageAlign.Stretch
                        loDroppedNode.AllowOutgoingLinks = True

                        loDroppedNode.Tag = New Object
                        loDroppedNode.Tag = Me
                        loDroppedNode.RoleConstraint = Me

                        Me.Shape = loDroppedNode

                        Dim lrRoleConstraintRoleInstance As FBM.RoleConstraintRoleInstance
                        Dim lrRoleInstance As FBM.RoleInstance
                        For Each lrRoleConstraintRoleInstance In Me.RoleConstraintRole
                            lrRoleInstance = lrRoleConstraintRoleInstance.Role

                            '-------------------------------------------------------------
                            'Create the link between the Role and the ORMObject that it 
                            '  joins to, unless is joined to a SubtypeConstraint Link
                            '-------------------------------------------------------------                                   
                            If lrRoleInstance.FactType.IsSubtypeRelationshipFactType Then
                                '20150803-Remove the following commented-out code if the new RoleInstanceLink code works
                                '  for Constraints that link Subtyping Constraints. No longer need link-to-link.
                                'Me.Page.CreateLinkToLink(Me.Shape, lrRoleInstance.FactType.SubtypeConstraintInstance.Link)
                                lrRoleConstraintRoleInstance.SubtypeConstraintInstance = lrRoleInstance.FactType.SubtypeConstraintInstance
                            Else
                                Dim lo_link As New DiagramLink(Me.Page.Diagram, Me.Shape, lrRoleInstance.Shape)
                                lo_link.Visible = True
                                lo_link.Pen.DashPattern = New Single() {2, 1, 2, 1}
                                lo_link.Pen.Width = 0.4
                                lo_link.Tag = lrRoleConstraintRoleInstance
                                lrRoleConstraintRoleInstance.Link = New DiagramLink(Me.Page.Diagram)
                                lrRoleConstraintRoleInstance.Link = lo_link

                                Me.Page.Diagram.Links.Add(lo_link)

                                Select Case Me.RoleConstraintType
                                    Case Is = pcenumRoleConstraintType.SubsetConstraint
                                        If lrRoleConstraintRoleInstance.IsExit Then
                                            lo_link.HeadShape = ArrowHead.PointerArrow
                                            lo_link.HeadShapeSize = 3
                                            lo_link.HeadPen.Color = Color.Purple
                                        End If
                                        If lrRoleConstraintRoleInstance.RoleConstraintRole.RoleConstraintArgument IsNot Nothing Then
                                            If lrRoleConstraintRoleInstance.RoleConstraintRole.RoleConstraintArgument.SequenceNr > 1 Then
                                                lo_link.HeadShape = ArrowHead.PointerArrow
                                                lo_link.HeadShapeSize = 3
                                                lo_link.HeadPen.Color = Color.Purple
                                            End If
                                        End If
                                End Select
                            End If

                            Me.Page.Diagram.Invalidate()
                        Next

                End Select
            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Private Sub DisplayAndAssociateAsInternalUniquenessConstraint()

            'Dim loDroppedNode As ShapeNode
            Dim lrRoleConstraintRole As New FBM.RoleConstraintRoleInstance
            Dim lrRoleInstance As New FBM.RoleInstance
            Dim lrFactTypeInstance As New FBM.FactTypeInstance
            Dim lsMessage As String = ""

            Try

                If Me.Role.Count > 0 Then
                    lrFactTypeInstance = Me.Role(0).FactType
                Else
                    lsMessage = "RoleConstraint.Role.Count = 0"
                    lsMessage &= vbCrLf & " RoleConstraint.Id: " & Me.Id
                    Throw New System.Exception(lsMessage)
                End If

                For Each lrRoleConstraintRole In Me.RoleConstraintRole

                    '-----------------------------------------------------------------------
                    'Get the RoleInstance to draw the InternalUniquenessConstraint against.
                    '-----------------------------------------------------------------------
                    lrFactTypeInstance = Me.RoleConstraintRole(0).Role.FactType

                    lrRoleInstance = lrFactTypeInstance.RoleGroup.Find(Function(x) x.Id = lrRoleConstraintRole.Role.Id)

                    '---------------------------------------------------------------------------------------------
                    'Create the ShapeNode/Shape for the actual line/s drawn for the InternalUniquenessConstraint
                    '---------------------------------------------------------------------------------------------
                    Dim loDroppedShapeNode As New ShapeNode
                    Dim lrShape As MindFusion.Diagramming.Shape
                    If Me.IsDeontic Then
                        lrShape = New Shape(
                                       New ElementTemplate() _
                                       {
                                            New LineTemplate(23, 0, 100, 0)
                                       },
                                       New ElementTemplate() _
                                       {
                                            New ArcTemplate(0, -23, 23, 48, 0, 360)
                                       },
                                        Nothing, FillMode.Winding, "test")
                    ElseIf Me.IsPreferredIdentifier Then
                        '-------------------------------------
                        'For preferred uniqueness constraint
                        '-------------------------------------
                        lrShape = New Shape(
                                        New ElementTemplate() _
                                        {
                                            New LineTemplate(0, 0, 100, 0),
                                            New LineTemplate(100, 0, 100, 100),
                                            New LineTemplate(100, 100, 0, 100),
                                            New LineTemplate(0, 100, 0, 0)
                                        },
                                            New ElementTemplate() _
                                        {
                                            New LineTemplate(0, 40, 100, 40),
                                            New LineTemplate(0, 70, 100, 70)
                                        },
                                            Nothing, System.Drawing.Drawing2D.FillMode.Winding, "test")

                        lrShape.Decorations(0).Color = Color.Purple
                        lrShape.Decorations(1).Color = Color.Purple
                    Else
                        lrShape = New Shape(
                                    New ElementTemplate() _
                                    {
                                        New LineTemplate(0, 0, 100, 0),
                                        New LineTemplate(100, 0, 100, 100),
                                        New LineTemplate(100, 100, 0, 100),
                                        New LineTemplate(0, 100, 0, 0)
                                    },
                                    New ElementTemplate() _
                                    {
                                        New LineTemplate(0, 50, 100, 50)
                                    },
                                    Nothing, System.Drawing.Drawing2D.FillMode.Winding, "test")

                        lrShape.Decorations(0).Color = Color.Purple
                    End If


                    loDroppedShapeNode = Me.Page.Diagram.Factory.CreateShapeNode(lrRoleInstance.Shape.Bounds.X,
                                                                                 (lrRoleInstance.Shape.Bounds.Y - 3.5) - ((Me.LevelNr - 1) * 2), 6, 2, lrShape)
                    loDroppedShapeNode.Pen = New MindFusion.Drawing.Pen(Color.White, 0.0002)
                    loDroppedShapeNode.ShadowColor = Color.White
                    'loDroppedShapeNode.Shape.Decorations(0).Color = Color.Green
                    loDroppedShapeNode.HandlesStyle = HandlesStyle.InvisibleMove
                    'loDroppedShapeNode.Transparent = True
                    loDroppedShapeNode.AllowOutgoingLinks = False
                    loDroppedShapeNode.AllowIncomingLinks = False
                    loDroppedShapeNode.ZBottom()

                    '-------------------------------------------------------------------------------
                    'Assign the RoleConstraint object to the Tag of the UniquenessConstraint Shape
                    '-------------------------------------------------------------------------------
                    loDroppedShapeNode.Tag = New Object
                    loDroppedShapeNode.Tag = lrRoleConstraintRole

                    '------------------------------------------------------------------------------------------------------------------
                    'Attach the UniquenessConstraint Shape to the RoleInstance.Shape
                    '  i.e. MindFusion attach so that when the RoleInstance is moved the UniquenessConstraint(Instance) will move too.
                    '------------------------------------------------------------------------------------------------------------------
                    loDroppedShapeNode.AttachTo(lrRoleInstance.Shape, AttachToNode.TopCenter)

                    '------------------------------------------------------------------------------------------------------------------
                    'Assign the UniquenessConstraint(Instance) Shape to the corresponding RoleConstraintRole object
                    '  NB For an InternalUniquenessConstraint there is no shape for the RoleConstraint...just the RoleConstraintRoles
                    '------------------------------------------------------------------------------------------------------------------
                    lrRoleConstraintRole.Shape = loDroppedShapeNode

                    If lrRoleConstraintRole.Role.FactType.isPreferredReferenceMode Then
                        lrRoleConstraintRole.Shape.Visible = False
                    Else
                        lrRoleConstraintRole.Shape.Visible = True
                    End If

                    '-------------------------------------------------
                    'Force the FactTypeInstance.Shape to the ZBottom
                    '-------------------------------------------------
                    If lrRoleInstance.FactType.Shape IsNot Nothing Then
                        lrRoleInstance.FactType.Shape.ZBottom()
                    End If

                    Me.Page.Diagram.Invalidate()

                Next

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub


        Public Function get_constraint_arity() As Integer
            '------------------------------------------------
            'Returns the arity of the 'RoleConstraint'
            ' defined by aiConstraint_id
            '------------------------------------------------
            Dim liInd As Integer = 0
            Dim li_count As Integer = 0

            'li_count = 0
            'For liInd = 1 To prApplication.workingpage.RoleConstraintInstance.Count
            '    If prApplication.workingpage.RoleConstraintInstance(liInd).constraint_type = 0 Then
            '        '-------------------------------------------
            '        'Found another InternalUniquenessConstraint
            '        '-------------------------------------------
            '        If (Me.MasterRole.Id = prApplication.workingpage.RoleConstraintInstance(liInd).MasterRole.Id) And (Me.LevelNr = prApplication.workingpage.RoleConstraintInstance(liInd).LevelNr) Then
            '            '------------------------------------------------------------------------------------------
            '            'Found another InternalUniquenessConstraint where the MasterRoleId and LevelNr is the same
            '            '------------------------------------------------------------------------------------------
            '            li_count = li_count + 1
            '        End If
            '    End If
            'Next liInd

            get_constraint_arity = li_count

        End Function

        ''' <summary>
        ''' Returns the Model level ModelObject for this Instance.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overrides Function getBaseModelObject() As FBM.ModelObject

            Return Me.RoleConstraint

        End Function

        Public Sub RefreshShape(Optional ByVal aoChangedPropertyItem As PropertyValueChangedEventArgs = Nothing,
                                Optional ByVal asSelectedGridItemLabel As String = "")


            Try
                Dim lrEntityType As FBM.EntityType

                If IsSomething(aoChangedPropertyItem) Then
                    Select Case aoChangedPropertyItem.ChangedItem.PropertyDescriptor.Name
                        Case Is = "Name"
                            '------------------------------------------------
                            'Set the values in the underlying Model.EntityType
                            '------------------------------------------------
                            If Me.RoleConstraint.Name = Me.Name Then
                                '------------------------------------------------------------
                                'Nothing to do. Name of the RoleConstraint has not been changed.
                                '------------------------------------------------------------
                            Else
                                Me.RoleConstraint.SetName(Me.Name)
                                Me.Id = Me.Name
                                Me.Symbol = Me.Name
                            End If
                        Case Is = "ShortDescription"
                            Call Me.RoleConstraint.SetShortDescription(Me.ShortDescription)
                            Me.Model.ModelDictionary.Find(Function(x) LCase(x.Symbol) = LCase(Me.Id)).ShortDescription = Me.ShortDescription
                        Case Is = "LongDescription"
                            Call Me.RoleConstraint.SetLongDescription(Me.LongDescription)
                            Me.Model.ModelDictionary.Find(Function(x) LCase(x.Symbol) = LCase(Me.Id)).LongDescription = Me.LongDescription
                        Case Is = "IsPreferredIdentifier"
                            If Me.IsPreferredIdentifier Then
                                Select Case Me.RoleConstraintType
                                    Case Is = pcenumRoleConstraintType.InternalUniquenessConstraint
                                        If Me.RoleConstraintRole.Count = 1 And
                                           Me.RoleConstraint.IsEachRoleFactTypeBinary Then
                                            If Me.RoleConstraintRole(0).Role.JoinedORMObject.ConceptType = pcenumConceptType.ValueType And
                                               Me.RoleConstraint.RoleConstraintRole(0).Role.FactType.GetOtherRoleOfBinaryFactType(Me.RoleConstraint.RoleConstraintRole(0).Role.Id).JoinedORMObject.ConceptType = pcenumConceptType.EntityType Then
                                                '-------------------------------------------------------------------------------------------------------------------------------------------------------------
                                                'Is a Internal Uniqueness RoleConstraint over an appropriate FactType to make the FactType/ValueType combination the ReferenceMode scheme for the EntityType
                                                '-------------------------------------------------------------------------------------------------------------------------------------------------------------
                                                lrEntityType = New FBM.EntityType
                                                Dim lrValueType As New FBM.ValueType

                                                lrValueType = Me.RoleConstraint.RoleConstraintRole(0).Role.JoinedORMObject
                                                lrEntityType = Me.RoleConstraint.RoleConstraintRole(0).Role.FactType.GetOtherRoleOfBinaryFactType(Me.RoleConstraint.RoleConstraintRole(0).Role.Id).JoinedORMObject

                                                '----------------------------------------------------------------------------------------------------------
                                                'Remove any CompoundReferenceScheme if there is one
                                                If lrEntityType.HasCompoundReferenceMode Then
                                                    Call lrEntityType.RemoveComplexReferenceScheme()
                                                End If

                                                Dim lsReferenceMode As String = lrValueType.GetReferenceModeFromName


                                                lrEntityType.ReferenceMode = lsReferenceMode
                                                lrEntityType.PreferredIdentifierRCId = Me.RoleConstraint.Id
                                                lrEntityType.ReferenceModeRoleConstraint = Me.RoleConstraint
                                                lrEntityType.ReferenceModeFactType = New FBM.FactType
                                                lrEntityType.ReferenceModeFactType = Me.RoleConstraint.RoleConstraintRole(0).Role.FactType
                                                lrEntityType.ReferenceModeValueType = New FBM.ValueType
                                                lrEntityType.ReferenceModeValueType = lrValueType

                                                lrEntityType.SetReferenceMode(lsReferenceMode, True)

                                            End If
                                        End If
                                    Case Is = pcenumRoleConstraintType.ExternalUniquenessConstraint
                                        If Me.RoleConstraintRole.Count > 1 And
                                            Me.RoleConstraint.IsEachRoleFactTypeBinary Then
                                            If Me.DoesEachRoleFactTypeOppositeRoleJoinSameModelObject Then
                                                '------------------------------------------------------------
                                                'Is Compound Reference Scheme identifier for an Entity Type
                                                '------------------------------------------------------------

                                                Me.Shape.Image = My.Resources.ORMShapes.preferred_uniqueness


                                                lrEntityType = Me.RoleConstraint.RoleConstraintRole(0).Role.FactType.GetOtherRoleOfBinaryFactType(Me.RoleConstraint.RoleConstraintRole(0).Role.Id).JoinedORMObject

                                                If lrEntityType.HasSimpleReferenceScheme Then
                                                    lrEntityType.RemoveSimpleReferenceScheme(False)
                                                End If

                                                'NB EntityTypeInstance take care of themselves, events called in RemoveSimpleReferenceScheme.

                                                '------------------------------------------------------------------
                                                'Model level triggers changes in the Instances
                                                Dim lrEntityTypeInstance As New FBM.EntityTypeInstance
                                                Dim lrRoleInstance As New FBM.RoleInstance


                                                Dim lrRole As FBM.Role
                                                For Each lrRoleConstraintRole In Me.RoleConstraint.RoleConstraintRole
                                                    lrRole = lrRoleConstraintRole.Role.FactType.GetOtherRoleOfBinaryFactType(lrRoleConstraintRole.Role.Id)
                                                    lrRole.SetMandatory(True, True)
                                                Next


                                                lrRoleInstance = Me.RoleConstraintRole(0).Role.FactType.GetOtherRoleOfBinaryFactType(Me.RoleConstraintRole(0).Role.Id)

                                                If lrRoleInstance.JoinedORMObject.ConceptType = pcenumConceptType.EntityType Then
                                                    lrEntityTypeInstance = lrRoleInstance.JoinedORMObject

                                                    Call lrEntityTypeInstance.EntityType.SetCompoundReferenceSchemeRoleConstraint(Me.RoleConstraint)
                                                End If

                                            End If
                                        End If
                                End Select
                            Else
                                'IsPreferredIdentifier = False
                                Select Case Me.RoleConstraintType
                                    Case Is = pcenumRoleConstraintType.InternalUniquenessConstraint
                                    Case Is = pcenumRoleConstraintType.ExternalUniquenessConstraint
                                        Me.Shape.Image = My.Resources.ORMShapes.externalUniqueness
                                        If Me.RoleConstraintRole.Count > 1 And
                                           Me.RoleConstraint.IsEachRoleFactTypeBinary Then
                                            If Me.DoesEachRoleFactTypeOppositeRoleJoinSameModelObject Then
                                                '------------------------------------------------------------
                                                'Is Compound Reference Scheme identifier for an Entity Type
                                                '------------------------------------------------------------

                                                Me.Shape.Image = My.Resources.ORMShapes.preferred_uniqueness

                                                lrEntityType = Me.RoleConstraint.RoleConstraintRole(0).Role.FactType.GetOtherRoleOfBinaryFactType(Me.RoleConstraint.RoleConstraintRole(0).Role.Id).JoinedORMObject
                                                Call lrEntityType.RemoveComplexReferenceScheme()
                                            End If
                                        End If

                                End Select
                            End If

                            Call Me.RoleConstraint.SetIsPreferredIdentifier(Me.IsPreferredIdentifier)
                        Case Is = "IsDeontic"
                            Me.RoleConstraint.IsDeontic = Me.IsDeontic
                            If Me.RoleConstraint.RoleConstraintType = pcenumRoleConstraintType.RingConstraint Then
                                If Me.IsDeontic Then
                                    Select Case Me.RingConstraintType
                                        Case Is = pcenumRingConstraintType.Acyclic
                                            Me.RingConstraintType = pcenumRingConstraintType.DeonticAcyclic
                                        Case Is = pcenumRingConstraintType.AcyclicIntransitive
                                            Me.RingConstraintType = pcenumRingConstraintType.DeonticAcyclicIntransitive
                                        Case Is = pcenumRingConstraintType.Antisymmetric
                                            Me.RingConstraintType = pcenumRingConstraintType.DeonticAntisymmetric
                                        Case Is = pcenumRingConstraintType.Asymmetric
                                            Me.RingConstraintType = pcenumRingConstraintType.DeonticAssymmetric
                                        Case Is = pcenumRingConstraintType.AsymmetricIntransitive
                                            Me.RingConstraintType = pcenumRingConstraintType.DeonticAssymmetricIntransitive
                                        Case Is = pcenumRingConstraintType.Intransitive
                                            Me.RingConstraintType = pcenumRingConstraintType.DeonticIntransitive
                                        Case Is = pcenumRingConstraintType.Irreflexive
                                            Me.RingConstraintType = pcenumRingConstraintType.DeonticIrreflexive
                                        Case Is = pcenumRingConstraintType.PurelyReflexive
                                            Me.RingConstraintType = pcenumRingConstraintType.DeonticPurelyReflexive
                                        Case Is = pcenumRingConstraintType.Symmetric
                                            Me.RingConstraintType = pcenumRingConstraintType.DeonticSymmetric
                                        Case Is = pcenumRingConstraintType.SymmetricIntransitive
                                            Me.RingConstraintType = pcenumRingConstraintType.DeonticSymmetricIntransitive
                                        Case Is = pcenumRingConstraintType.SymmetricIrreflexive
                                            Me.RingConstraintType = pcenumRingConstraintType.DeonticSymmetricIrreflexive
                                    End Select
                                Else
                                    Select Case Me.RingConstraintType
                                        Case Is = pcenumRingConstraintType.DeonticAcyclic
                                            Me.RingConstraintType = pcenumRingConstraintType.Acyclic
                                        Case Is = pcenumRingConstraintType.DeonticAcyclicIntransitive
                                            Me.RingConstraintType = pcenumRingConstraintType.AcyclicIntransitive
                                        Case Is = pcenumRingConstraintType.DeonticAntisymmetric
                                            Me.RingConstraintType = pcenumRingConstraintType.Antisymmetric
                                        Case Is = pcenumRingConstraintType.DeonticAssymmetric
                                            Me.RingConstraintType = pcenumRingConstraintType.Asymmetric
                                        Case Is = pcenumRingConstraintType.DeonticAssymmetricIntransitive
                                            Me.RingConstraintType = pcenumRingConstraintType.AsymmetricIntransitive
                                        Case Is = pcenumRingConstraintType.DeonticIntransitive
                                            Me.RingConstraintType = pcenumRingConstraintType.Intransitive
                                        Case Is = pcenumRingConstraintType.DeonticIrreflexive
                                            Me.RingConstraintType = pcenumRingConstraintType.Irreflexive
                                        Case Is = pcenumRingConstraintType.DeonticPurelyReflexive
                                            Me.RingConstraintType = pcenumRingConstraintType.PurelyReflexive
                                        Case Is = pcenumRingConstraintType.DeonticSymmetric
                                            Me.RingConstraintType = pcenumRingConstraintType.Symmetric
                                        Case Is = pcenumRingConstraintType.DeonticSymmetricIntransitive
                                            Me.RingConstraintType = pcenumRingConstraintType.SymmetricIntransitive
                                        Case Is = pcenumRingConstraintType.DeonticSymmetricIrreflexive
                                            Me.RingConstraintType = pcenumRingConstraintType.SymmetricIrreflexive
                                    End Select
                                End If
                            End If
                            Call Me.RoleConstraint.SetIsDeontic(Me.IsDeontic)
                    End Select

                    If IsSomething(Me.Page.Form) Then
                        Me.Page.Diagram.Invalidate()
                        Call Me.Page.Form.EnableSaveButton()
                    End If
                End If

                '=====================================================================
                'Shape drawing
                '=====================================================================
                Select Case Me.RoleConstraintType
                    Case Is = pcenumRoleConstraintType.ExternalUniquenessConstraint
                        If (Me.IsDeontic = False) And (Me.IsPreferredIdentifier = False) Then
                            Me.Shape.Image = My.Resources.ORMShapes.externalUniqueness
                        ElseIf Me.IsDeontic And (Me.IsPreferredIdentifier = False) Then
                            Me.Shape.Image = My.Resources.ORMShapes.deontic_external_uniqueness
                        ElseIf Me.IsDeontic And Me.IsPreferredIdentifier Then
                            Me.Shape.Image = My.Resources.ORMShapes.deontic_external_preferred_uniqueness
                        ElseIf (Me.IsDeontic = False) And Me.IsPreferredIdentifier Then
                            Me.Shape.Image = My.Resources.ORMShapes.preferred_uniqueness
                        End If
                    Case Is = pcenumRoleConstraintType.EqualityConstraint
                        Select Case Me.IsDeontic
                            Case Is = True
                                Me.Shape.Image = My.Resources.ORMShapes.deontic_equality
                            Case Else
                                Me.Shape.Image = My.Resources.ORMShapes.equality
                        End Select
                    Case Is = pcenumRoleConstraintType.ExclusionConstraint
                        Select Case Me.IsDeontic
                            Case Is = True
                                Me.Shape.Image = My.Resources.ORMShapes.deontic_exclusion
                            Case Else
                                Me.Shape.Image = My.Resources.ORMShapes.exclusion
                        End Select

                    Case Is = pcenumRoleConstraintType.ExclusiveORConstraint
                        Select Case Me.IsDeontic
                            Case Is = True
                                Me.Shape.Image = My.Resources.ORMShapes.deontic_exclusiveOr
                            Case Else
                                Me.Shape.Image = My.Resources.ORMShapes.exlusiveOR
                        End Select

                    Case Is = pcenumRoleConstraintType.InclusiveORConstraint
                        Select Case Me.IsDeontic
                            Case Is = True
                                Me.Shape.Image = My.Resources.ORMShapes.deontic_inclusive_or
                            Case Else
                                Me.Shape.Image = My.Resources.ORMShapes.inclusive_or
                        End Select

                    Case Is = pcenumRoleConstraintType.InternalUniquenessConstraint

                        If Me.RoleConstraintRole(0).Shape Is Nothing Then
                            Exit Sub
                        End If

                        Dim lrRoleConstraintRoleInstance As FBM.RoleConstraintRoleInstance
                        For Each lrRoleConstraintRoleInstance In Me.RoleConstraintRole

                            'lrRoleConstraintRoleInstance.Shape.Move(lrRoleConstraintRoleInstance.Shape.Bounds.X, _
                            '                                        (lrRoleConstraintRoleInstance.Role.Shape.Bounds.Y - 3.5) - ((Me.LevelNr - 1) * 1.5))

                        Next


                        If Me.IsDeontic Then
                            Me.RoleConstraintRole(0).Shape.Shape = New Shape(
                                                                   New ElementTemplate() _
                                                                   {
                                                                        New LineTemplate(23, 0, 100, 0)
                                                                   },
                                                                   New ElementTemplate() _
                                                                   {
                                                                        New ArcTemplate(0, -23, 23, 48, 0, 360)
                                                                   },
                                                                    Nothing, FillMode.Winding, "test")
                        ElseIf Me.IsPreferredIdentifier Then
                            '-------------------------------------
                            'For preferred uniqueness constraint
                            '-------------------------------------
                            Dim loRectangle As New Rectangle(Me.RoleConstraintRole(0).Shape.Bounds.X,
                                                             Me.RoleConstraintRole(0).Shape.Bounds.Y,
                                                             Me.RoleConstraintRole(0).Shape.Bounds.Width,
                                                             3)

                            'Me.RoleConstraintRole(0).Shape.SetRect(loRectangle, False)

                            Dim lrShape As Shape = New Shape(
                                                                New ElementTemplate() _
                                                                     {
                                                                        New LineTemplate(0, 0, 100, 0),
                                                                        New LineTemplate(100, 0, 100, 100),
                                                                        New LineTemplate(100, 100, 0, 100),
                                                                        New LineTemplate(0, 100, 0, 0)
                                                                    },
                                                                        New ElementTemplate() _
                                                                    {
                                                                        New LineTemplate(0, 40, 100, 40),
                                                                        New LineTemplate(0, 70, 100, 70)
                                                                    },
                                                                    Nothing, System.Drawing.Drawing2D.FillMode.Winding, "test")

                            lrShape.Decorations(0).Color = Color.Purple
                            lrShape.Decorations(1).Color = Color.Purple

                            For Each lrRoleConstraintRole In Me.RoleConstraintRole
                                lrRoleConstraintRole.Shape.Shape = lrShape
                            Next
                        Else
                            Dim loRectangle As New Rectangle(Me.RoleConstraintRole(0).Shape.Bounds.X,
                                                             Me.RoleConstraintRole(0).Shape.Bounds.Y,
                                                             Me.RoleConstraintRole(0).Shape.Bounds.Width, 3)
                            'Me.RoleConstraintRole(0).Shape.SetRect(loRectangle, False)
                            Dim lrShape As Shape = New Shape(
                                        New ElementTemplate() _
                                        {
                                            New LineTemplate(0, 0, 100, 0),
                                            New LineTemplate(100, 0, 100, 100),
                                            New LineTemplate(100, 100, 0, 100),
                                            New LineTemplate(0, 100, 0, 0)
                                        },
                                        New ElementTemplate() _
                                        {
                                            New LineTemplate(0, 50, 100, 50)
                                        },
                                        Nothing, System.Drawing.Drawing2D.FillMode.Winding, "test")
                            lrShape.Decorations(0).Color = Color.Purple

                            For Each lrRoleConstraintRole In Me.RoleConstraintRole
                                lrRoleConstraintRole.Shape.Shape = lrShape
                            Next
                        End If

                End Select

                If Me.IsDeontic Then
                    Dim lrRoleConstraintRoleInstance As FBM.RoleConstraintRoleInstance
                    Select Case Me.RoleConstraintType
                        Case Is = pcenumRoleConstraintType.ExternalUniquenessConstraint,
                                  pcenumRoleConstraintType.EqualityConstraint,
                                  pcenumRoleConstraintType.ExclusionConstraint,
                                  pcenumRoleConstraintType.ExclusiveORConstraint,
                                  pcenumRoleConstraintType.InclusiveORConstraint,
                                  pcenumRoleConstraintType.RingConstraint,
                                  pcenumRoleConstraintType.SubsetConstraint

                            For Each lrRoleConstraintRoleInstance In Me.RoleConstraintRole
                                If IsSomething(lrRoleConstraintRoleInstance.Link) Then
                                    lrRoleConstraintRoleInstance.Link.Pen.Color = Color.Blue
                                End If
                            Next
                    End Select
                Else
                    Dim lrRoleConstraintRoleInstance As FBM.RoleConstraintRoleInstance
                    Select Case Me.RoleConstraintType
                        Case Is = pcenumRoleConstraintType.ExternalUniquenessConstraint,
                                  pcenumRoleConstraintType.EqualityConstraint,
                                  pcenumRoleConstraintType.ExclusionConstraint,
                                  pcenumRoleConstraintType.ExclusiveORConstraint,
                                  pcenumRoleConstraintType.InclusiveORConstraint,
                                  pcenumRoleConstraintType.RingConstraint,
                                  pcenumRoleConstraintType.SubsetConstraint

                            For Each lrRoleConstraintRoleInstance In Me.RoleConstraintRole
                                If IsSomething(lrRoleConstraintRoleInstance.Link) Then
                                    lrRoleConstraintRoleInstance.Link.Pen.Color = Color.Purple
                                End If
                            Next
                    End Select
                End If

                If IsSomething(Me.Page) Then
                    Call Me.Page.Invalidate()
                    If IsSomething(Me.Page.Diagram) Then
                        Call Me.Page.Diagram.Invalidate()
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

        Public Overridable Sub RemoveFromPage(ByVal abBroadcastInterfaceEvent As Boolean)

            Try

                If Me.Page Is Nothing Then
                    Exit Sub
                End If

                Dim lrRoleConstraintRoleInstance As FBM.RoleConstraintRoleInstance

                Select Case Me.RoleConstraintType
                    Case Is = pcenumRoleConstraintType.InternalUniquenessConstraint
                        '----------------------------------------------------------------------------------
                        'CodeSafe: If there are no RoleConstraintRoles, don't want to abort at this stage
                        '----------------------------------------------------------------------------------
                        If Me.RoleConstraintRole.Count > 0 Then
                            If Me.RoleConstraintRole(0).Role IsNot Nothing Then
                                Me.RoleConstraintRole(0).Role.FactType.RemoveInternalUniquenessConstraint(Me)
                            End If
                        End If
                End Select

                For Each lrRoleConstraintRoleInstance In Me.RoleConstraintRole
                    If IsSomething(Me.Page.Diagram) Then
                        Me.Page.Diagram.Nodes.Remove(lrRoleConstraintRoleInstance.Shape)
                    End If
                    If lrRoleConstraintRoleInstance.Role IsNot Nothing Then
                        If lrRoleConstraintRoleInstance.Role.Shape IsNot Nothing Then
                            lrRoleConstraintRoleInstance.Role.Shape.Text = ""
                            lrRoleConstraintRoleInstance.Role.Shape.Brush = New MindFusion.Drawing.SolidBrush(Color.White)
                        End If
                    End If
                Next

                Call Me.Page.RemoveRoleConstraintInstance(Me, abBroadcastInterfaceEvent)

                Me.Page.MakeDirty()

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Public Overridable Overloads Sub Save(Optional ByVal abRapidSave As Boolean = False)

            Dim lrConceptInstance As New FBM.ConceptInstance(Me.Model, Me.Page, Me.Id)
            lrConceptInstance.ConceptType = pcenumConceptType.RoleConstraint
            lrConceptInstance.X = Me.X
            lrConceptInstance.Y = Me.Y

            '------------------------------------------------
            'Saves the RoleConstraintInstnce to the database
            '------------------------------------------------
            If abRapidSave Then
                Call TableConceptInstance.AddConceptInstance(lrConceptInstance)
            Else
                If TableConceptInstance.ExistsConceptInstance(lrConceptInstance) Then
                    Call TableConceptInstance.UpdateConceptInstance(lrConceptInstance)
                Else
                    Call TableConceptInstance.AddConceptInstance(lrConceptInstance)
                End If
            End If

        End Sub

        Private Sub RoleConstraint_CardinalityChanged(ByVal aiNewCardinality As Integer) Handles RoleConstraint.CardinalityChanged

            Me.Cardinality = aiNewCardinality
            Call Me.RefreshShape()

        End Sub

        Private Sub RoleConstraint_CardinalityRangeTypeChanged(ByVal aiNewCardinalityRangeType As publicConstants.pcenumCardinalityRangeType) Handles RoleConstraint.CardinalityRangeTypeChanged

            Me.CardinalityRangeType = aiNewCardinalityRangeType
            Call Me.RefreshShape()

        End Sub

        Private Sub RoleConstraint_IsDeonticChanged(ByVal abNewIsDeontic As Boolean) Handles RoleConstraint.IsDeonticChanged

            Me.IsDeontic = abNewIsDeontic
            Call Me.RefreshShape()

        End Sub

        Private Sub RoleConstraint_IsPreferredIdentifierChanged(ByVal abNewIsPreferredIdentifier As Boolean) Handles RoleConstraint.IsPreferredIdentifierChanged

            Try
                Me.IsPreferredIdentifier = abNewIsPreferredIdentifier

                If IsSomething(Me.Page) Then
                    Call Me.RefreshShape()
                End If

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Private Sub RoleConstraint_LevelNrChanged(ByVal aiNewLevelNr As Integer) Handles RoleConstraint.LevelNrChanged

            Me.LevelNr = aiNewLevelNr
            Call Me.RefreshShape()

        End Sub

        Private Sub RoleConstraint_LongDescriptionChanged(asLongDescription As String) Handles RoleConstraint.LongDescriptionChanged

            Me.LongDescription = asLongDescription

        End Sub

        Private Sub RoleConstraint_MaximumFrequencyCountChanged(ByVal aiNewMaximumFrequencyCount As Integer) Handles RoleConstraint.MaximumFrequencyCountChanged

            Me.MaximumFrequencyCount = aiNewMaximumFrequencyCount
            Call Me.RefreshShape()

        End Sub

        Private Sub RoleConstraint_MinimumFrequencyCountChanged(ByVal aiNewMinimumFrequencyCount As Integer) Handles RoleConstraint.MinimumFrequencyCountChanged

            Me.MinimumFrequencyCount = aiNewMinimumFrequencyCount
            Call Me.RefreshShape()

        End Sub

        Private Sub RoleConstraint_NameChanged() Handles RoleConstraint.NameChanged

            Dim lrConceptInstance As New FBM.ConceptInstance(Me.Model, Me.Page, Me.RoleConstraint.Id, pcenumConceptType.RoleConstraint)
            Call TableConceptInstance.UpdateConceptInstanceByModelPageConceptTypeRoleId(lrConceptInstance, Me.Id)

            Me.Id = Me.RoleConstraint.Id
            Me.Name = Me.RoleConstraint.Id
            Me.Symbol = Me.RoleConstraint.Id

            Me.Page.MakeDirty()

        End Sub

        Private Sub RoleConstraint_RemovedFromModel(ByVal abBroadcastInterfaceEvent As Boolean) Handles RoleConstraint.RemovedFromModel

            Call Me.RemoveFromPage(abBroadcastInterfaceEvent)

        End Sub

        Public Sub NodeDeselected() Implements FBM.iPageObject.NodeDeselected

            If Me.Shape IsNot Nothing Then
                Me.Shape.Transparent = True
            End If

        End Sub

        Private Sub RoleConstraint_RingConstraintTypeChanged(ByVal aiNewRingConstraintType As publicConstants.pcenumRingConstraintType) Handles RoleConstraint.RingConstraintTypeChanged

            Me.RingConstraintType = aiNewRingConstraintType
            Call Me.RefreshShape()

        End Sub

        Private Sub RoleConstraint_RoleConstraintRoleAdded(ByRef arRoleConstraintRole As RoleConstraintRole,
                                                           ByRef arSubtypeConstraintInstance As FBM.SubtypeRelationshipInstance) Handles RoleConstraint.RoleConstraintRoleAdded

            Try
                Dim lrRoleConstraintRoleInstance As FBM.RoleConstraintRoleInstance


                lrRoleConstraintRoleInstance = arRoleConstraintRole.CloneInstance(Me.Page)
                Me.RoleConstraintRole.Add(lrRoleConstraintRoleInstance)

                Dim lrRoleInstance As FBM.RoleInstance = lrRoleConstraintRoleInstance.Role

                If lrRoleInstance.FactType.IsSubtypeRelationshipFactType Then
                    lrRoleConstraintRoleInstance.SubtypeConstraintInstance = lrRoleInstance.FactType.SubtypeConstraintInstance
                End If
                'If arSubtypeConstraintInstance IsNot Nothing Then
                '    lrRoleConstraintRoleInstance.SubtypeConstraintInstance = arSubtypeConstraintInstance
                'End If

                Call Me.RefreshRoleConstraintRoleTexts()

                If Me.Page.Diagram Is Nothing Then Exit Sub

                Dim lo_link As DiagramLink

                Select Case Me.RoleConstraint.RoleConstraintType
                    Case Is = pcenumRoleConstraintType.InternalUniquenessConstraint

                        Call lrRoleConstraintRoleInstance.displayAndAssociateForInternalUniquenessConstraint()

                    Case Else
                        lo_link = New DiagramLink(Me.Page.Diagram, Me.Shape, lrRoleConstraintRoleInstance.Role.Shape)
                        lo_link.Visible = True
                        lo_link.Pen.DashPattern = New Single() {2, 1, 2, 1}
                        lo_link.Pen.Width = 0.4
                        lo_link.HeadPen.Color = Color.Purple
                        lo_link.Tag = lrRoleConstraintRoleInstance
                        lrRoleConstraintRoleInstance.Link = New DiagramLink(Me.Page.Diagram)
                        lrRoleConstraintRoleInstance.Link = lo_link

                        Me.Page.Diagram.Links.Add(lo_link)

                        Select Case Me.RoleConstraintType
                            Case Is = pcenumRoleConstraintType.SubsetConstraint
                                If lrRoleConstraintRoleInstance.IsExit Then
                                    lo_link.HeadShape = ArrowHead.PointerArrow
                                    lo_link.HeadShapeSize = 3
                                    lo_link.HeadPen.Color = Color.Purple
                                End If
                                If lrRoleConstraintRoleInstance.RoleConstraintRole.RoleConstraintArgument IsNot Nothing Then
                                    ' RoleConstraintRole.RoleConstraintArgument IsNot Nothing Then
                                    If lrRoleConstraintRoleInstance.RoleConstraintRole.RoleConstraintArgument.SequenceNr > 1 Then
                                        lo_link.HeadShape = ArrowHead.PointerArrow
                                        lo_link.HeadShapeSize = 3
                                        lo_link.HeadPen.Color = Color.Purple
                                    End If
                                End If
                        End Select
                End Select

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub


        Private Sub RoleConstraint_RoleConstraintRoleRemoved(ByVal arRoleConstraintRole As RoleConstraintRole) Handles RoleConstraint.RoleConstraintRoleRemoved

            Dim lrRoleConstraintRoleInstanceToRemove As FBM.RoleConstraintRoleInstance

            Try
                If Me.Page Is Nothing Then
                    Exit Sub
                End If

                lrRoleConstraintRoleInstanceToRemove = Me.RoleConstraintRole.Find(Function(x) x.Role.Id = arRoleConstraintRole.Role.Id)

                If lrRoleConstraintRoleInstanceToRemove Is Nothing Then
                    'Have probably already removed the RoleConstraintRole. i.e. e.g. RoleConstraint.RemoveArgumentBySequenceNumber removes RoleConstraintRoles and then the Argument.
                    Exit Sub
                End If

                If IsSomething(lrRoleConstraintRoleInstanceToRemove.Link) Then
                    Me.Page.Diagram.Links.Remove(lrRoleConstraintRoleInstanceToRemove.Link)
                End If

                If lrRoleConstraintRoleInstanceToRemove.RoleConstraint.RoleConstraintType = pcenumRoleConstraintType.InternalUniquenessConstraint Then
                    Me.Page.Diagram.Nodes.Remove(lrRoleConstraintRoleInstanceToRemove.Shape)
                End If

                Me.RoleConstraintRole.Remove(lrRoleConstraintRoleInstanceToRemove)

                For Each lrRoleConstraintRoleInstance In Me.RoleConstraintRole.FindAll(Function(x) x.SequenceNr >= lrRoleConstraintRoleInstanceToRemove.SequenceNr)
                    lrRoleConstraintRoleInstance.SequenceNr -= 1
                Next

                Me.Page.Diagram.Invalidate()

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try


        End Sub

        Private Sub RoleConstraint_RoleConstraintTypeChanged(ByVal aiNewRoleConstraintType As publicConstants.pcenumRoleConstraintType) Handles RoleConstraint.RoleConstraintTypeChanged

            Me.RoleConstraintType = aiNewRoleConstraintType

            Call Me.RefreshShape()

        End Sub

        Public Sub SetAppropriateColour() Implements iPageObject.SetAppropriateColour

            If Me.Shape IsNot Nothing Then
                If Me.Shape.Selected Then
                    Me.Shape.Pen.Color = Color.Blue
                Else
                    If Me.RoleConstraint.HasModelError Then
                        Me.Shape.Transparent = False
                        Me.Shape.Visible = True
                        Me.Shape.Pen.Color = Color.Red
                    Else
                        Me.Shape.Transparent = True
                        Me.Shape.Pen.Color = Color.White
                    End If
                End If
            End If

        End Sub

        Public Sub RepellNeighbouringPageObjects(ByVal aiDepth As Integer) Implements iPageObject.RepellNeighbouringPageObjects

        End Sub

        Public Sub Move(ByVal aiNewX As Integer, ByVal aiNewY As Integer, ByVal abBroadcastInterfaceEvent As Boolean) Implements iPageObject.Move

        End Sub

        Private Sub RoleConstraint_ShortDescriptionChanged(asShortDescription As String) Handles RoleConstraint.ShortDescriptionChanged

            Me.ShortDescription = asShortDescription

        End Sub

        Public Overloads Function Equals(other As RoleConstraintInstance) As Boolean Implements IEquatable(Of RoleConstraintInstance).Equals
            Return Me.Id = other.Id
        End Function
    End Class

End Namespace