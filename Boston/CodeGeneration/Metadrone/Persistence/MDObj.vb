Namespace Persistence

    Public Class MDObj : Implements IMDPersistenceItem

        Friend Bin As New Parser.Bin.BinObj()

        Public obj() As Byte = Nothing

        Public Sub New()

        End Sub

        Friend Sub AddPackage(ByVal Package As Boston.Parser.Bin.CompiledPackage)
            If Not My.Application.EXEC_SaveBin Then
                Me.Bin = New Parser.Bin.BinObj()
                Me.obj = Nothing
                Exit Sub
            End If

            If Package Is Nothing Then Exit Sub

            'Look for to update
            For i As Integer = 0 To Me.Bin.pkgs.Count - 1
                If Me.Bin.pkgs(i).GUID.Equals(Package.GUID) Then
                    'Clear variable values
                    Call Me.ClearTokenVals(Package)

                    'Clear param vals in package
                    Call Me.ClearParamVals(Package)

                    'Update package
                    Me.Bin.pkgs(i) = Package

                    'Update obj, serialised and compressed
                    Me.obj = Tools.Compression.Compress(Tools.Reflection.ConvertObjectToBytes(Me.Bin))

                    Exit Sub
                End If
            Next

            'Clear token values in sources
            Call Me.ClearSourceTokenVals()

            'Clear variable values
            Call Me.ClearTokenVals(Package)

            'Clear param vals in package
            Call Me.ClearParamVals(Package)

            'Add
            Me.Bin.pkgs.Add(Package)

            'Update obj, serialised and compressed
            Me.obj = Tools.Compression.Compress(Tools.Reflection.ConvertObjectToBytes(Me.Bin))
        End Sub



        'Clear source token values that are of type variable.
        'XML serialisation may not like some of these values, and they are not really needed as a compilation
        Private Sub ClearSourceTokenVals()
            For Each src In Me.Bin.srces
                Call Me.ClearSourceTokenVals(src.Transforms.TransformInstructions)
            Next
        End Sub

        Private Sub ClearSourceTokenVals(ByVal node As Parser.Syntax.SyntaxNode)
            For i As Integer = 0 To node.Tokens.Count - 1
                If node.Tokens(i).Type = Parser.Syntax.SyntaxToken.ElementTypes.Variable Then
                    node.Tokens(i).Value = Nothing
                End If
                For j As Integer = 0 To node.Tokens(i).QualifiedExpressionTokens.Count - 1
                    If node.Tokens(i).QualifiedExpressionTokens(j).Type = Parser.Syntax.SyntaxToken.ElementTypes.Variable Then
                        node.Tokens(i).QualifiedExpressionTokens(j).Value = Nothing
                    End If
                Next
            Next
            For Each n In node.Nodes
                Call Me.ClearSourceTokenVals(n)
            Next
        End Sub



        'Clear token values in a package that are of type variable.
        'XML serialisation may not like some of these values, and they are not really needed as a compilation
        Private Sub ClearTokenVals(ByVal pkg As Parser.Bin.CompiledPackage)
            For Each tmp In pkg.CompiledTemplates
                Call Me.ClearTokenVals(tmp.BaseNode)
            Next
        End Sub

        Private Sub ClearTokenVals(ByVal node As Parser.Syntax.SyntaxNode)
            For i As Integer = 0 To node.Tokens.Count - 1
                If node.Tokens(i).Type = Parser.Syntax.SyntaxToken.ElementTypes.Variable Then
                    node.Tokens(i).Value = Nothing
                End If
                For j As Integer = 0 To node.Tokens(i).QualifiedExpressionTokens.Count - 1
                    If node.Tokens(i).QualifiedExpressionTokens(j).Type = Parser.Syntax.SyntaxToken.ElementTypes.Variable Then
                        node.Tokens(i).QualifiedExpressionTokens(j).Value = Nothing
                    End If
                Next
            Next
            For Each n In node.Nodes
                Call Me.ClearTokenVals(n)
            Next
        End Sub



        'Clear parameter values in a package.
        'XML serialisation may not like some of these values, and they are not really needed as a compilation
        Private Sub ClearParamVals(ByVal pkg As Parser.Bin.CompiledPackage)
            For Each tmp In pkg.CompiledTemplates
                Call Me.ClearParamVals(tmp.BaseNode)
            Next
        End Sub

        Private Sub ClearParamVals(ByVal node As Parser.Syntax.SyntaxNode)
            For i As Integer = 0 To node.Owner_TemplateParamValues.Count - 1
                node.Owner_TemplateParamValues(i) = Nothing
            Next
            For i As Integer = 0 To node.TemplateParameters.Count - 1
                node.TemplateParameters(i).Value = Nothing
            Next
            For Each n In node.Nodes
                Call Me.ClearParamVals(n)
            Next
        End Sub

        Friend Sub SetCompiledSources(ByVal srces As List(Of Boston.Parser.Bin.CompiledSource))
            Me.Bin.srces.Clear()
            For Each src In srces
                Me.Bin.srces.Add(src)
            Next

            'Update obj, serialised and compressed
            Me.obj = Tools.Compression.Compress(Tools.Reflection.ConvertObjectToBytes(Me.Bin))
        End Sub

        Friend Function GetCompiledPackage(ByVal GUID As String) As Boston.Parser.Bin.CompiledPackage
            For Each p In Me.Bin.pkgs
                If p.GUID.Equals(GUID) Then Return p
            Next
            Return Nothing
        End Function

        Public Function GetCopy() As IMDPersistenceItem Implements IMDPersistenceItem.GetCopy
            Dim Copy As New MDObj()
            Array.Copy(Me.obj, Copy.obj, Me.obj.Length)
            Return Copy
        End Function

    End Class

End Namespace