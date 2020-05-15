
Namespace Conversion
    Public Module Conversion

        ' <summary>
        ' Converts polar coordinates to the corresponding dekart coordinates,
        ' using the specified point as a center of the coordinate system.
        ' </summary>
        Public Sub PolarToDekart(ByVal coordCenter As PointF, ByVal a As Double, ByVal r As Double, ByRef dekart As PointF)

            If r = 0 Then

                dekart = coordCenter
                Return
            End If

            dekart.X = CType(coordCenter.X + Math.Cos(a * Math.PI / 180) * r, Double)
            dekart.Y = CType(coordCenter.Y - Math.Sin(a * Math.PI / 180) * r, Double)
        End Sub

        ' <summary>
        ' Converts dekart coordinates to the corresponding polar coordinates,
        ' using the specified point as a center of the coordinate system.
        ' </summary>
        Public Sub DekartToPolar(ByVal coordCenter As PointF, ByVal dekart As PointF, ByRef a As Double, ByRef r As Double)

            If Object.Equals(coordCenter, dekart) Then
                a = 0
                r = 0
                Return
            End If

            Dim dx As Double = dekart.X - coordCenter.X
            Dim dy As Double = dekart.Y - coordCenter.Y
            r = CType(Math.Sqrt(Math.Pow(dx, 2) + Math.Pow(dy, 2)), Double)

            a = CType(Math.Atan(-dy / dx) * 180 / Math.PI, Double)
            If (dx < 0) Then
                a += 180
            End If

        End Sub


    End Module
End Namespace
