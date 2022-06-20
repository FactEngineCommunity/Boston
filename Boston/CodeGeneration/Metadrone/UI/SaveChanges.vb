Imports Boston.Persistence

Namespace UI

    Friend Class SaveChanges

        'Check if tab control is dirty
        Public Shared Function TabIsDirty(ByVal TabPage As TabPage) As Boolean
            If TabPage.Controls.Count = 0 Then Return False

            If TypeOf TabPage.Controls(0) Is CodeEditor Then
                Dim editor As CodeEditor = CType(TabPage.Controls(0), CodeEditor)
                If TypeOf editor.Tag Is Template Then
                    Return CType(editor.Tag, Template).GetDirty

                ElseIf TypeOf editor.Tag Is Main Then
                    Return CType(editor.Tag, Main).GetDirty

                ElseIf TypeOf editor.Tag Is CodeDOM_VB Then
                    Return CType(editor.Tag, CodeDOM_VB).GetDirty

                ElseIf TypeOf editor.Tag Is CodeDOM_CS Then
                    Return CType(editor.Tag, CodeDOM_CS).GetDirty

                End If

            ElseIf TypeOf TabPage.Controls(0) Is ManageProjectProperties Then
                Dim props As ManageProjectProperties = CType(TabPage.Controls(0), ManageProjectProperties)
                If TypeOf props.Tag Is Persistence.Properties Then Return CType(props.Tag, Persistence.Properties).GetDirty

            ElseIf TypeOf TabPage.Controls(0) Is ManageSource Then
                Dim man As ManageSource = CType(TabPage.Controls(0), ManageSource)
                If TypeOf man.Tag Is Source Then Return CType(man.Tag, Source).GetDirty

            End If

            Return False
        End Function

        'Add dirty tab into list
        Public Function FillDirty(ByVal BaseNode As TreeNode, ByVal TabPage As TabPage) As Boolean
            If TabPage.Controls.Count = 0 Then Return False

            If TypeOf TabPage.Controls(0) Is CodeEditor Then
                Dim editor As CodeEditor = CType(TabPage.Controls(0), CodeEditor)

                If TypeOf editor.Tag Is Template Then
                    If CType(editor.Tag, Template).GetDirty Then Me.lstItems.Items.Add(editor.Description)

                ElseIf TypeOf editor.Tag Is Main Then
                    If CType(editor.Tag, Main).GetDirty Then Me.lstItems.Items.Add(editor.Description)

                ElseIf TypeOf editor.Tag Is CodeDOM_VB Then
                    If CType(editor.Tag, CodeDOM_VB).GetDirty Then Me.lstItems.Items.Add(editor.Description)

                ElseIf TypeOf editor.Tag Is CodeDOM_CS Then
                    If CType(editor.Tag, CodeDOM_CS).GetDirty Then Me.lstItems.Items.Add(editor.Description)

                End If

            ElseIf TypeOf TabPage.Controls(0) Is ManageProjectProperties Then
                Dim props As ManageProjectProperties = CType(TabPage.Controls(0), ManageProjectProperties)
                If TypeOf props.Tag Is Persistence.Properties Then
                    If CType(props.Tag, Persistence.Properties).GetDirty Then
                        Me.lstItems.Items.Add("Properties")
                    End If
                End If

            ElseIf TypeOf TabPage.Controls(0) Is ManageSource Then
                Dim man As ManageSource = CType(TabPage.Controls(0), ManageSource)

                If TypeOf man.Tag Is Source Then
                    If CType(man.Tag, Source).GetDirty Then
                        Me.lstItems.Items.Add(Parser.Syntax.Constants.GLOBALS_SOURCES & "." & CType(man.Tag, Source).Name)
                    End If
                End If

            End If

            Return Me.lstItems.Items.Count > 0
        End Function

        'Add dirty tabs into list
        Public Function FillDirty(ByVal BaseNode As TreeNode, ByVal TabControl As TabControl, ByVal projDirty As Boolean) As Boolean
            Dim items As New List(Of String)

            If projDirty Then
                'Assume basenode is the project node
                If BaseNode.Tag.ToString.Equals(Explorer.TAG_PROJECT) Then Me.lstItems.Items.Add(BaseNode.Text)
            End If

            For Each tp As TabPage In TabControl.TabPages
                If tp.Controls.Count = 0 Then Continue For
                If TypeOf tp.Controls(0) Is CodeEditor Then
                    Dim editor As CodeEditor = CType(tp.Controls(0), CodeEditor)

                    If TypeOf editor.Tag Is Template Then
                        If CType(editor.Tag, Template).GetDirty Then items.Add(editor.Description)

                    ElseIf TypeOf editor.Tag Is Main Then
                        If CType(editor.Tag, Main).GetDirty Then items.Add(editor.Description)

                    ElseIf TypeOf editor.Tag Is CodeDOM_VB Then
                        If CType(editor.Tag, CodeDOM_VB).GetDirty Then items.Add(editor.Description)

                    ElseIf TypeOf editor.Tag Is CodeDOM_CS Then
                        If CType(editor.Tag, CodeDOM_CS).GetDirty Then items.Add(editor.Description)

                    End If

                ElseIf TypeOf tp.Controls(0) Is ManageProjectProperties Then
                    Dim props As ManageProjectProperties = CType(tp.Controls(0), ManageProjectProperties)
                    If TypeOf props.Tag Is Persistence.Properties Then
                        If CType(props.Tag, Persistence.Properties).GetDirty Then
                            Me.lstItems.Items.Add("Properties")
                        End If
                    End If

                ElseIf TypeOf tp.Controls(0) Is ManageSource Then
                    Dim man As ManageSource = CType(tp.Controls(0), ManageSource)

                    If TypeOf man.Tag Is Source Then
                        If CType(man.Tag, Source).GetDirty Then
                            items.Add(Parser.Syntax.Constants.GLOBALS_SOURCES & "." & CType(man.Tag, Source).Name)
                        End If

                    End If

                End If
            Next

            items.Sort()

            For Each item In items
                Me.lstItems.Items.Add(item)
            Next

            Return Me.lstItems.Items.Count > 0
        End Function

    End Class

End Namespace