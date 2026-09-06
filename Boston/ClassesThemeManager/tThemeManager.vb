Imports WeifenLuo.WinFormsUI.Docking
Imports WeifenLuo.WinFormsUI.Docking.VS2012Theme

Public Module ThemeManager

    Public Enum ThemeColor
        Primary
        Secondary
        Tertiary
        Text
    End Enum

    ' Dictionaries to hold theme colors
    Private LightTheme As Dictionary(Of ThemeColor, Color)
    Private NatureTheme As Dictionary(Of ThemeColor, Color)
    Private DarkTheme As Dictionary(Of ThemeColor, Color)
    Private BlueTheme As Dictionary(Of ThemeColor, Color)

    Public Sub InitializeThemes()
        LightTheme = New Dictionary(Of ThemeColor, Color) From {
            {ThemeColor.Primary, Color.WhiteSmoke},
            {ThemeColor.Secondary, Color.Silver},
            {ThemeColor.Tertiary, Color.White},
            {ThemeColor.Text, Color.Black}
        }

        NatureTheme = New Dictionary(Of ThemeColor, Color) From {
            {ThemeColor.Primary, Color.DarkSeaGreen},
            {ThemeColor.Secondary, Color.AliceBlue},
            {ThemeColor.Tertiary, Color.Honeydew},
            {ThemeColor.Text, Color.White} 'Color.FromArgb(0, 122, 204)}
        }

        DarkTheme = New Dictionary(Of ThemeColor, Color) From {
            {ThemeColor.Primary, Color.DimGray},
            {ThemeColor.Secondary, Color.DimGray},
            {ThemeColor.Tertiary, Color.DarkGray},
            {ThemeColor.Text, Color.White}
        }

        ' New Mid-Range Light Blue Theme
        BlueTheme = New Dictionary(Of ThemeColor, Color) From {
            {ThemeColor.Primary, Color.FromArgb(173, 216, 230)}, ' LightBlue
            {ThemeColor.Secondary, Color.FromArgb(135, 206, 235)}, ' SkyBlue
            {ThemeColor.Tertiary, Color.FromArgb(176, 224, 230)}, ' PowderBlue
            {ThemeColor.Text, Color.FromArgb(25, 25, 112)} ' MidnightBlue for text
        }

    End Sub

    Public Sub ApplyTheme(form As Form, themeType As ThemeType)

        Dim theme As Dictionary(Of ThemeColor, Color)

        Call InitializeThemes()

        ' Select the appropriate theme based on the enum value
        Select Case themeType
            Case ThemeType.Light
                theme = LightTheme
            Case ThemeType.Nature
                theme = NatureTheme
            Case ThemeType.Dark
                theme = DarkTheme
            Case ThemeType.Blue
                theme = BlueTheme
            Case Else
                ' Default to Light theme if an unknown theme is passed
                theme = LightTheme
        End Select

        ' Apply the selected theme based on the form type
        Select Case form.GetType()
            Case GetType(WeifenLuo.WinFormsUI.Docking.DockContent)
                Dim dockPanel As DockPanel = DirectCast(form, WeifenLuo.WinFormsUI.Docking.DockContent).DockPanel

                ' Apply the custom theme to the DockPanel
                'dockPanel.Theme = New CustomVS2012Theme(theme(ThemeColor.Primary), theme(ThemeColor.Secondary))

            Case Else
                ' Set the background color for other forms
                form.BackColor = theme(ThemeColor.Tertiary)
        End Select

        ' Apply the selected theme to the controls within the form
        ApplyThemeToControls(form, theme)


        'Child forms
        For Each childForm As Form In form.MdiChildren
            'Perform actions on each child form
            Call ApplyTheme(childForm, themeType)
        Next

        If form.Name = "frmMain" Then
            For Each dockContent As WeifenLuo.WinFormsUI.Docking.DockContent In CType(form, frmMain).DockPanel.Contents
                ' Perform actions on each docked content
                Call ApplyTheme(dockContent, themeType)
            Next
        End If

    End Sub

    Private Sub ApplyThemeToControls(container As Control, theme As Dictionary(Of ThemeColor, Color))
        For Each ctrl As Control In container.Controls
            If TypeOf ctrl Is Button Then
                ctrl.BackColor = theme(ThemeColor.Primary)
                ctrl.ForeColor = theme(ThemeColor.Text)
            ElseIf {GetType(Boston.BostonTreeView), GetType(TreeView)}.Contains(ctrl.GetType) Then
                ctrl.BackColor = Color.White 'theme(ThemeColor.Secondary)
                ctrl.ForeColor = Color.FromArgb(129, 129, 129) ' Lighter gray color
            ElseIf TypeOf ctrl Is Label Then
                ctrl.ForeColor = theme(ThemeColor.Text)
            ElseIf {GetType(WeifenLuo.WinFormsUI.Docking.DockPanel), GetType(WeifenLuo.WinFormsUI.Docking.DockContent)}.Contains(ctrl.GetType) Then
                ' Apply special theme customization for WeifenLuo DockPanel forms
                'Dim customTheme As New CustomVS2012Theme()
                'DirectCast(ctrl, WeifenLuo.WinFormsUI.Docking.DockPanel).Theme = customTheme
            End If

            ' Recursively apply the theme to child controls
            If ctrl.HasChildren Then
                ApplyThemeToControls(ctrl, theme)
            End If
        Next
        ' Apply the custom theme for WeifenLuo DockPanel top bars
        If {GetType(WeifenLuo.WinFormsUI.Docking.DockPanel), GetType(WeifenLuo.WinFormsUI.Docking.DockContent)}.Contains(container.GetType) Then
            'Dim customTheme As New CustomVS2012Theme()
            'DirectCast(container, WeifenLuo.WinFormsUI.Docking.DockPanel).Theme = customTheme
        End If
    End Sub


End Module
