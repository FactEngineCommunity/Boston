Imports Boston.Parser.Syntax.Constants
Imports Boston.Parser.Meta

Namespace Parser.Syntax

    Friend Class Strings

        Public Shared Function Tokenise(ByVal expression As String, ByVal LineNumber As Integer) As SyntaxTokenCollection
            Dim toks As New SyntaxTokenCollection()

            'Scan for spaces and control characters
            Dim sr As New System.IO.StringReader(expression)
            Try
                Dim currIdx As Integer = -1
                Dim inStr As Boolean = False
                Dim sb As New System.Text.StringBuilder()
                While sr.Peek > -1
                    Dim ch As String = Convert.ToChar(sr.Read()).ToString
                    currIdx += 1
                    If ch.Equals("""") Then
                        'If next char is a quote while still in string literals, it means quote character
                        If inStr And sr.Peek > -1 Then
                            Dim c As String = Convert.ToChar(sr.Peek()).ToString
                            If c.Equals("""") Then
                                'Add the quote, but still in string
                                sr.Read()
                                sb.Append("""")
                                Continue While
                            End If
                        End If

                        'In/out string mode
                        inStr = Not inStr

                        'New word if entering string
                        If inStr And sb.Length > 0 Then
                            toks.Add(New SyntaxToken(sb.ToString, LineNumber, SyntaxToken.ElementTypes.NotSet))
                            sb = New System.Text.StringBuilder()
                        End If

                        sb.Append(ch)

                    ElseIf ch.Equals(" ") Then
                        'End of word if not in string
                        If inStr Then
                            sb.Append(ch)
                            Continue While
                        End If
                        If sb.Length > 0 Then
                            toks.Add(New SyntaxToken(sb.ToString, LineNumber, SyntaxToken.ElementTypes.NotSet))
                            sb = New System.Text.StringBuilder()
                        End If

                    Else
                        'Check operator character occurances if not in string
                        If inStr Then
                            sb.Append(ch)
                            Continue While
                        End If
                        Dim charIdx As Integer = -1
                        For i As Integer = 0 To Constants.RESERVED_OPERATORS.Count - 1
                            'Skip periods, they'll be used in decimal values and variable attributes qualifiers
                            If RESERVED_OPERATORS(i).Equals(".") Then Continue For

                            If ch.Equals(Constants.RESERVED_OPERATORS(i)) Then
                                'Skip comment qualifier (this is hardcoded against comment as "//") for optimisation
                                'we may have to stick a 
                                'TODO here to make it work with another constant value
                                If Constants.RESERVED_OPERATORS(i).Equals("/") And sr.Peek > -1 Then
                                    Dim nextChar As Char = Convert.ToChar(sr.Peek())
                                    If nextChar.ToString.Equals("/") Then
                                        'Ignore past comments
                                        'toks.Add(New SyntaxToken("//", LineNumber, SyntaxToken.ElementTypes.NotSet))
                                        Exit While

                                    End If

                                ElseIf Constants.RESERVED_OPERATORS(i).Equals("<") Then
                                    Dim nextChar As Char = Convert.ToChar(sr.Peek())
                                    If nextChar.ToString.Equals(">") Then
                                        'Append not equal to
                                        sb.Append("<>")

                                        'Read next
                                        sr.Read()
                                        currIdx += 1
                                        Continue While

                                    End If

                                ElseIf Constants.RESERVED_OPERATORS(i).Equals("<") Then
                                    Dim nextChar As Char = Convert.ToChar(sr.Peek())
                                    If nextChar.ToString.Equals("=") Then
                                        'Append less than equal to
                                        sb.Append("<=")

                                        'Read next
                                        sr.Read()
                                        currIdx += 1
                                        Continue While

                                    End If

                                ElseIf Constants.RESERVED_OPERATORS(i).Equals(">") Then
                                    Dim nextChar As Char = Convert.ToChar(sr.Peek())
                                    If nextChar.ToString.Equals("=") Then
                                        'Append greater than equal to
                                        sb.Append(">=")

                                        'Read next
                                        sr.Read()
                                        currIdx += 1
                                        Continue While

                                    End If

                                End If

                                charIdx = currIdx
                                Exit For
                            End If
                        Next
                        If charIdx = -1 Then
                            'Keep appending
                            sb.Append(ch)
                        Else
                            'End of word
                            If sb.Length > 0 Then
                                toks.Add(New SyntaxToken(sb.ToString, LineNumber, SyntaxToken.ElementTypes.NotSet))
                                sb = New System.Text.StringBuilder()
                            End If
                            toks.Add(New SyntaxToken(ch, LineNumber, SyntaxToken.ElementTypes.NotSet))
                        End If
                    End If
                End While

                'Must enclose string literals
                If inStr Then
                    Throw New Exception("Syntax error. Line: " & LineNumber.ToString & ".")
                End If

                'Add remaining dangler
                If sb.Length > 0 Then toks.Add(New SyntaxToken(sb.ToString, LineNumber, SyntaxToken.ElementTypes.NotSet))

            Catch ex As Exception
                Throw ex

            Finally
                sr.Close()
                sr.Dispose()
                sr = Nothing

            End Try

            Return toks
        End Function

        Public Shared Function TokensToString(ByVal Tokens As SyntaxTokenCollection) As String
            Return TokensToString(Tokens, 0, Tokens.Count - 1)
        End Function

        Public Shared Function TokensToString(ByVal Tokens As SyntaxTokenCollection, _
                                              ByVal TokenIdxStart As Integer, ByVal TokenIdxEnd As Integer) As String
            Dim sb As New System.Text.StringBuilder()
            For i As Integer = TokenIdxStart To TokenIdxEnd
                sb.Append(Tokens(i).Text & " ")
            Next
            If sb.Length > 0 Then sb.Remove(sb.Length - 1, 1)
            Return sb.ToString
        End Function

        Public Shared Function RemQuotes(ByVal value As String) As String
            'trim and remove quotes
            value = value.Trim

            'Get first position
            Dim idx As Integer = value.IndexOf("""")

            'None to remove
            If idx = -1 Then Return value

            'One at the end
            If idx = value.Length Then Return SubStr(value, 0, idx)

            'Remove first instance
            If idx = 0 Then value = value.Substring(1)

            'Get again
            idx = value.LastIndexOf("""")

            'No more to remove
            If idx = -1 Then Return value

            If idx = value.Length - 1 Then value = SubStr(value, 0, idx)

            Return value
        End Function

        Public Shared Function SubStr(ByVal Line As String, ByVal StartIdx As Integer, ByVal EndIdx As Integer) As String
            Return Line.Substring(StartIdx, EndIdx - StartIdx)
        End Function

        'case-insensitive string compare
        Public Shared Function StrEq(ByVal value As String, ByVal CompareTo As String) As Boolean
            Return String.Compare(value, CompareTo, StringComparison.OrdinalIgnoreCase) = 0
        End Function

        Public Shared Function StrIdxOf(ByVal value As String, ByVal toFind As String) As Integer
            Return value.IndexOf(toFind, StringComparison.OrdinalIgnoreCase)
        End Function

        'From http://www.codeproject.com/KB/string/fastestcscaseinsstringrep.aspx
        Public Shared Function ReplaceInsensitive(ByVal original As String, ByVal pattern As String, ByVal replacement As String) As String
            Dim count As Integer = 0, position0 As Integer = 0, position1 As Integer = 0
            Dim upperString As String = original.ToUpper()
            Dim upperPattern As String = pattern.ToUpper()
            Dim inc As Integer = (original.Length \ pattern.Length) * (replacement.Length - pattern.Length)
            Dim chars As Char() = New Char(original.Length + (Math.Max(0, inc) - 1)) {}
            position1 = upperString.IndexOf(upperPattern, position0)
            While position1 <> -1
                For i As Integer = position0 To position1 - 1
                    chars(count) = original(i)
                    count += 1
                Next
                For i As Integer = 0 To replacement.Length - 1
                    chars(count) = replacement(i)
                    count += 1
                Next
                position0 = position1 + pattern.Length
                position1 = upperString.IndexOf(upperPattern, position0)
            End While
            If position0 = 0 Then
                Return original
            End If
            For i As Integer = position0 To original.Length - 1
                chars(count) = original(i)
                count += 1
            Next
            Return New String(chars, 0, count)
        End Function

    End Class

End Namespace
