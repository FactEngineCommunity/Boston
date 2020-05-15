Imports MindFusion.Diagramming

Namespace ERD

    ''' <summary>
    ''' Inherits Mindfusion.Diagramming.TableNode
    ''' </summary>
    ''' <remarks></remarks>
    Public Class TableNode
        Inherits MindFusion.Diagramming.TableNode


        Public Sub New()
            Me.Font = New Font("Arial", 8, FontStyle.Regular)
        End Sub

        Public Overloads Sub ResizeToFitText(ByVal ignoreCation As Boolean)

            Me.ResizeToFitText(False, False)

            Me.CaptionHeight = 6

            Dim format1 As New StringFormat(StringFormatFlags.NoClip)
            format1.LineAlignment = StringAlignment.Center
            format1.Alignment = StringAlignment.Center

            Me.CaptionFormat = New StringFormat(format1)
            Me.Resize(Me.Bounds.Width + 5, Me.Bounds.Height + 4)

        End Sub



    End Class
End Namespace
