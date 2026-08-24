Imports System.Windows.Forms
Imports System.Windows.Forms.VisualStyles
Imports System.Drawing

Namespace UI

    Friend Class ThemedColours

#Region "    Variables and Constants "

        Private Const NormalColor As String = "NormalColor"
        Private Const HomeStead As String = "HomeStead"
        Private Const Metallic As String = "Metallic"
        Private Const NoTheme As String = "NoTheme"

        Private Shared _toolBorder As Color()
#End Region

#Region "    Properties "

        Public Shared ReadOnly Property CurrentThemeIndex() As Integer
            Get
                Return ThemedColours.GetCurrentThemeIndex
            End Get
        End Property

        Public Shared ReadOnly Property CurrentThemeName() As String
            Get
                Try
                    Return ThemedColours.GetCurrentThemeName
                Catch ex As Exception
                    Return ""
                End Try

            End Get
        End Property

        Public Shared ReadOnly Property ToolBorder() As Color
            Get
                Return ThemedColours._toolBorder(ThemedColours.CurrentThemeIndex)
            End Get
        End Property

#End Region

#Region "    Constructors "

        Private Sub New()
        End Sub

        Shared Sub New()
            Dim colorArray1 As Color()
            colorArray1 = New Color() {Color.FromArgb(127, 157, 185), Color.FromArgb(164, 185, 127), Color.FromArgb(165, 172, 178), Color.FromArgb(132, 130, 132)}
            ThemedColours._toolBorder = colorArray1
        End Sub

#End Region

        Private Shared Function GetCurrentThemeIndex() As Integer
            Try
                If VisualStyleInformation.IsSupportedByOS AndAlso
                           VisualStyleInformation.IsEnabledByUser AndAlso
                           Application.RenderWithVisualStyles Then

                    Select Case VisualStyleInformation.ColorScheme
                        Case NormalColor : Return ColorScheme.NormalColor
                        Case HomeStead : Return ColorScheme.HomeStead
                        Case Metallic : Return ColorScheme.Metallic
                    End Select
                End If
            Catch
                Return ColorScheme.NoTheme
            End Try

            Return ColorScheme.NoTheme

        End Function

        Private Shared Function GetCurrentThemeName() As String

            Dim theme As String = "NoTheme"

            If VisualStyleInformation.IsSupportedByOS _
                AndAlso VisualStyleInformation.IsEnabledByUser _
                AndAlso Application.RenderWithVisualStyles Then

                theme = VisualStyleInformation.ColorScheme
            End If

            Return theme
        End Function


        Public Enum ColorScheme
            NormalColor = 0
            HomeStead = 1
            Metallic = 2
            NoTheme = 3
        End Enum

    End Class

End Namespace