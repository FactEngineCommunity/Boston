Imports System.Math
Imports MindFusion.Diagramming
Imports System.Reflection

Public Class tMorphVector

    Public StartPoint As Point
    Public EndPoint As Point

    Public StartSize As Rectangle
    Public EndSize As Rectangle

    ''' <summary>
    ''' The count of steps to be taken along the vector
    ''' </summary>
    ''' <remarks></remarks>
    Public VectorSteps As Integer

    ''' <summary>
    ''' The current step in the number of steps taken along the vector
    ''' </summary>
    ''' <remarks></remarks>
    Public VectorStep As Integer

    Public InitialZoomFactor As Integer = 100
    Public TargetZoomFactor As Integer = 100

    Public TargetImage As Image = Nothing
    Public TargetShape As pcenumTargetMorphShape = pcenumTargetMorphShape.RoundRectangle
    Public TargetText As String = ""

    Public ModelElementId As String = ""

    ''' <summary>
    ''' The Shape on the original Page being morphed to the location of the same representative Shape on the destination Page.
    ''' </summary>
    ''' <remarks></remarks>
    Public Shape As ShapeNode

    Public EnterpriseTreeView As tEnterpriseEnterpriseView

    Public Sub New(ByVal aiStart_x As Integer, _
                   ByVal aiStart_y As Integer, _
                   ByVal aiEnd_x As Integer, _
                   ByVal aiEnd_y As Integer, _
                   ByVal aiVector_steps As Integer, _
                   Optional ByRef aoShape As MindFusion.Diagramming.ShapeNode = Nothing)

        Me.StartPoint = New Point(aiStart_x, aiStart_y)
        Me.EndPoint = New Point(aiEnd_x, aiEnd_y)
        Me.VectorSteps = aiVector_steps
        Me.Shape = aoShape

    End Sub


    Public Function getNextMorphVectorStepPoint() As Point

        Try
            Dim lr_point As New Point
            Dim li_vector_x As Integer = Abs(Me.EndPoint.X - Me.StartPoint.X)
            Dim li_vector_y As Integer = Abs(Me.EndPoint.Y - Me.StartPoint.Y)

            Dim liHypotenuse = Math.Sqrt(li_vector_x ^ 2 + li_vector_y ^ 2)

            Me.VectorStep += Viev.Greater(liHypotenuse / Me.VectorSteps, 1)

            li_vector_x = Int((li_vector_x / Me.VectorSteps) * Me.VectorStep)
            li_vector_y = Int((li_vector_y / Me.VectorSteps) * Me.VectorStep)

            If Me.EndPoint.X > Me.StartPoint.X Then
                li_vector_x = Me.StartPoint.X + li_vector_x
            Else
                li_vector_x = Me.StartPoint.X - li_vector_x
            End If

            If Me.EndPoint.Y > Me.StartPoint.Y Then
                li_vector_y = Me.StartPoint.Y + li_vector_y
            Else
                li_vector_y = Me.StartPoint.Y - li_vector_y
            End If

            lr_point = New Point(li_vector_x, li_vector_y)
            Return lr_point

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Function

    Public Function getNextMorphVectorRectangle() As Rectangle

        Try
            Dim lrRectangle As Rectangle

            Dim liVectorX As Integer = Abs(Me.EndSize.Width - Me.StartSize.Width)
            Dim liVectorY As Integer = Abs(Me.EndSize.Height - Me.StartSize.Height)

            liVectorX = Int((liVectorX / Me.VectorSteps) * Me.VectorStep)
            liVectorY = Int((liVectorY / Me.VectorSteps) * Me.VectorStep)

            If Me.EndSize.Width > Me.StartSize.Width Then
                liVectorX = Me.StartSize.Width + liVectorX
            Else
                liVectorX = Me.StartSize.Width - liVectorX
            End If

            If Me.EndSize.Height > Me.StartSize.Height Then
                liVectorY = Me.StartSize.Height + liVectorY
            Else
                liVectorY = Me.StartSize.Height - liVectorY
            End If

            lrRectangle = New Rectangle(0, 0, liVectorX, liVectorY)
            Return lrRectangle

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Function



End Class
