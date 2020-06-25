Imports Boston.Parser.Syntax.Strings

Namespace Parser.Syntax

    Friend Class Variables
        'List of variables by variable name as key
        Private Class HashedVars
            Public List As New Dictionary(Of String, Variable)(StringComparer.OrdinalIgnoreCase)

            Public Sub New()

            End Sub

            Public Sub New(ByVal key As String, ByVal value As Variable)
                Me.List.Add(key, value)
            End Sub

            Public Function ContainsKey(ByVal key As String) As Boolean
                Return Me.List.ContainsKey(key)
            End Function

            Public Sub Add(ByVal key As String, ByVal value As Variable)
                Dim item As Variable = Nothing
                If Me.List.TryGetValue(key, item) Then
                    Throw New Exception("Variable '" & key & "' already declared in the current scope.")
                End If

                Me.List.Add(key, value)
            End Sub

            Public Function Item(ByVal key As String) As Variable
                Return Me.List.Item(key)
            End Function
        End Class

        'List of variables by scope number as the first key, and then by variable name within the scope
        Private Class ScopedHashedVars
            Public List As New Dictionary(Of Integer, HashedVars)

            Public Sub New()

            End Sub

            Public Sub New(ByVal scopedDepth As Integer, ByVal variableKey As String, ByVal value As Variable)
                Dim vars As New HashedVars(variableKey, value)
                Me.List.Add(scopedDepth, vars)
            End Sub

            Public Function ContainsKey(ByVal key As Integer) As Boolean
                Return Me.List.ContainsKey(key)
            End Function

            Public Sub Add(ByVal key As Integer, ByVal value As HashedVars)
                Me.List.Add(key, value)
            End Sub

            Public Function Item(ByVal key As Integer) As HashedVars
                Return Me.List.Item(key)
            End Function
        End Class


        'Variables in main
        'Multi key hash:
        '1: the scope
        '2: the variable name
        Private MainVars As New ScopedHashedVars()

        'Variables in templates
        'Multi key hash:
        '1: the template name
        '2: the scope
        '3: the variable name
        '
        'When templates are called within templates, the scope depth is incremented from the calling template.
        'Therefore recursively called templates should not interfere with each others scope
        Private TemplateVars As New Dictionary(Of String, ScopedHashedVars)(StringComparer.OrdinalIgnoreCase)



        Public Sub Add(ByVal TemplateName As String, ByVal Name As String, ByVal ScopeDepth As Integer, ByVal Variable As Variable)
            If String.IsNullOrEmpty(TemplateName) Then
                'Intended for main
                Dim vars As HashedVars = Nothing
                If Me.MainVars.List.TryGetValue(ScopeDepth, vars) Then
                    vars.Add(Name, Variable)
                Else
                    Me.MainVars.Add(ScopeDepth, New HashedVars(Name, Variable))
                End If
            Else
                'Template variable
                Dim scvars As ScopedHashedVars = Nothing
                If Me.TemplateVars.TryGetValue(TemplateName, scvars) Then
                    Dim vars As HashedVars = Nothing
                    If scvars.List.TryGetValue(ScopeDepth, vars) Then
                        'Add to current depth
                        vars.Add(Name, Variable)
                    Else
                        'New depth
                        scvars.Add(ScopeDepth, New HashedVars(Name, Variable))
                    End If
                Else
                    'Add new template and variable
                    Me.TemplateVars.Add(TemplateName, New ScopedHashedVars(ScopeDepth, Name, Variable))
                End If
            End If
        End Sub

        Public Function Item(ByVal TemplateName As String, ByVal Name As String, ByVal ScopeDepth As Integer) As Variable
            If String.IsNullOrEmpty(TemplateName) Then
                'Look each scope upwards

                While ScopeDepth > -1
                    If Me.MainVars.ContainsKey(ScopeDepth) AndAlso _
                       Me.MainVars.Item(ScopeDepth).ContainsKey(Name) Then
                        Return Me.MainVars.Item(ScopeDepth).Item(Name)
                    End If
                    ScopeDepth -= 1
                End While

                Throw New Exception("Variable '" & Name & "' is not declared.")

            Else
                If Me.TemplateVars.ContainsKey(TemplateName) Then
                    'Look each scope upwards
                    While ScopeDepth > -1
                        If Me.TemplateVars.Item(TemplateName).ContainsKey(ScopeDepth) AndAlso _
                           Me.TemplateVars.Item(TemplateName).Item(ScopeDepth).ContainsKey(Name) Then
                            Return Me.TemplateVars.Item(TemplateName).Item(ScopeDepth).Item(Name)
                        End If

                        ScopeDepth -= 1
                    End While

                    'Try main
                    Return Me.Item(Name)

                Else
                    'Check only in main
                    Return Me.Item(Name)
                End If

            End If
        End Function

        'Check in main - only in the base scope of zero (not in deeper scopes within blocks)
        Private Function Item(ByVal Name As String) As Variable
            If Me.MainVars.ContainsKey(0) AndAlso Me.MainVars.Item(0).ContainsKey(Name) Then
                Return Me.MainVars.Item(0).Item(Name)
            Else
                Throw New Exception("Variable '" & Name & "' is not declared.")
            End If
        End Function

        Public Function Exists(ByVal TemplateName As String, ByVal Name As String, ByVal ScopeDepth As Integer) As Boolean
            If String.IsNullOrEmpty(TemplateName) Then
                'Look each scope upwards
                While ScopeDepth > -1
                    If Me.MainVars.ContainsKey(ScopeDepth) AndAlso Me.MainVars.Item(ScopeDepth).ContainsKey(Name) Then
                        Return True
                    End If
                    ScopeDepth -= 1
                End While

                Return False

            Else
                If Not Me.TemplateVars.ContainsKey(TemplateName) Then
                    'Check in main - only in the base scope of zero (not in deeper scopes within blocks)
                    If Me.MainVars.ContainsKey(0) AndAlso Me.MainVars.Item(0).ContainsKey(Name) Then
                        Return True
                    End If
                    Return False
                End If

                'Look each scope upwards
                While ScopeDepth > -1
                    If Me.TemplateVars.Item(TemplateName).ContainsKey(ScopeDepth) AndAlso _
                       Me.TemplateVars.Item(TemplateName).Item(ScopeDepth).ContainsKey(Name) Then
                        Return True
                    End If
                    ScopeDepth -= 1
                End While

                'Check in main - only in the base scope of zero (not in deeper scopes within blocks)
                If Me.MainVars.ContainsKey(0) AndAlso Me.MainVars.Item(0).ContainsKey(Name) Then
                    Return True
                End If

                Return False

            End If
        End Function

        'Decommission inside a scope depth
        Public Sub Decommission(ByVal TemplateName As String, ByVal ScopeDepth As Integer)
            If String.IsNullOrEmpty(TemplateName) Then
                'Clear this scope
                If Me.MainVars.ContainsKey(ScopeDepth) Then Me.MainVars.Item(ScopeDepth).List.Clear()

                'Try one scope in to be sure
                If Me.MainVars.ContainsKey(ScopeDepth + 1) Then Me.MainVars.Item(ScopeDepth + 1).List.Clear()

            Else
                If Not Me.TemplateVars.ContainsKey(TemplateName) Then Exit Sub

                'Clear this scope
                If Me.TemplateVars.Item(TemplateName).ContainsKey(ScopeDepth) Then
                    Me.TemplateVars.Item(TemplateName).Item(ScopeDepth).List.Clear()
                End If

                'Try one scope in to be sure
                If Me.TemplateVars.Item(TemplateName).ContainsKey(ScopeDepth + 1) Then
                    Me.TemplateVars.Item(TemplateName).Item(ScopeDepth + 1).List.Clear()
                End If
            End If
        End Sub

        'Decommission individual variable
        Public Sub Decommission(ByVal TemplateName As String, ByVal Name As String, ByVal ScopeDepth As Integer)
            If String.IsNullOrEmpty(TemplateName) Then
                If Not Me.MainVars.ContainsKey(ScopeDepth) Then Exit Sub
                If Not Me.MainVars.Item(ScopeDepth).ContainsKey(Name) Then Exit Sub

                Me.MainVars.Item(ScopeDepth).List.Remove(Name)
            Else
                If Not Me.TemplateVars.ContainsKey(TemplateName) Then Exit Sub
                If Not Me.TemplateVars.Item(TemplateName).ContainsKey(ScopeDepth) Then Exit Sub
                If Not Me.TemplateVars.Item(TemplateName).Item(ScopeDepth).ContainsKey(Name) Then Exit Sub

                Me.TemplateVars.Item(TemplateName).Item(ScopeDepth).List.Remove(Name)
            End If
        End Sub

    End Class

End Namespace