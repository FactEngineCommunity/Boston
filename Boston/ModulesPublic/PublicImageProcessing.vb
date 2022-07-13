Namespace Boston
    Module PublicImageProcessing

        Public Function CropImage(ByVal img As Image, ByVal backgroundColor As Color, Optional ByVal margin As Integer = 0) As Image

            Dim minX As Integer = img.Width
            Dim minY As Integer = img.Height
            Dim maxX As Integer = 0
            Dim maxY As Integer = 0

            Using bmp As New Bitmap(img)

                For y As Integer = 0 To bmp.Height - 1
                    For x As Integer = 0 To bmp.Width - 1
                        If bmp.GetPixel(x, y).ToArgb <> backgroundColor.ToArgb Then
                            If x < minX Then
                                minX = x
                            ElseIf x > maxX Then
                                maxX = x
                            End If
                            If y < minY Then
                                minY = y
                            ElseIf y > maxY Then
                                maxY = y
                            End If
                        End If
                    Next
                Next

                Dim rect As New Rectangle(minX - margin, minY - margin, maxX - minX + 2 * margin + 1, maxY - minY + 2 * margin + 1)
                Dim cropped As Bitmap = bmp.Clone(rect, bmp.PixelFormat)

                Return cropped

            End Using

        End Function

        Public Function CreateFramedImage(ByVal Source As Image, ByVal BorderColor As Color, ByVal BorderThickness As Integer) As Image

            Dim b As New Bitmap(Source.Width + BorderThickness * 2, Source.Height + BorderThickness * 2)
            Dim g As Graphics = Graphics.FromImage(b)
            g.Clear(BorderColor)
            g.DrawImage(Source, BorderThickness, BorderThickness)
            g.Dispose()
            Return b
        End Function


    End Module

End Namespace
