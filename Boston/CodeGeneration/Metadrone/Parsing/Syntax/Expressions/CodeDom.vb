Imports Microsoft.VisualBasic
Imports Microsoft.CSharp
Imports System
Imports System.Text
Imports System.CodeDom.Compiler
Imports System.Reflection
Imports System.IO
Imports System.Diagnostics

Namespace Parser.Syntax

    Friend Class CodeDom
        Private VB_CR As CompilerResults = Nothing
        Private CS_CR As CompilerResults = Nothing

        Private mCompilerErrors As New CompilerErrorCollection()

        Private Class InstanceMethodInfo
            Public Instance As String
            Public Method As String
            Public Params As New List(Of Object)

            Public Sub New(ByVal expression As String)
                'Split class and method
                Dim s() As String = expression.Split(".".ToCharArray)
                If s.Length < 2 Then Throw New Exception("Invalid expression.")

                'Instance first, all until last dot
                Dim sb As New System.Text.StringBuilder()
                For i As Integer = 0 To s.Length - 2
                    sb.Append(s(i))
                    If i < s.Length - 2 Then sb.Append(".")
                Next

                Me.Instance = sb.ToString


                'Eval method and params
                Dim sr As New System.IO.StringReader(s(s.Length - 1))
                sb = New System.Text.StringBuilder()
                Dim psb As New System.Text.StringBuilder()
                Dim parms As New List(Of String)
                Dim bracketCount As Integer = 0
                Try
                    While sr.Peek > -1
                        Dim ch As String = Convert.ToChar(sr.Read()).ToString
                        If ch.Equals("(") Then
                            If bracketCount = 0 Then
                                'Found method, now get params
                                If sb Is Nothing Then
                                    'expression like this()(), just throw
                                    Throw New Exception("Invalid expression.")
                                Else
                                    Me.Method = sb.ToString : sb = Nothing
                                End If
                                bracketCount = 1
                            Else
                                bracketCount += 1
                                psb.Append(ch) 'keep adding to current param
                            End If

                        ElseIf ch.Equals(")") Then
                            If bracketCount = 0 Then Throw New Exception("Invalid expression.")
                            bracketCount -= 1
                            If bracketCount = 0 Then
                                'finished, as to params
                                If psb.Length = 0 And parms.Count > 0 Then Throw New Exception("Argument expected.")
                                If psb.Length > 0 Then parms.Add(psb.ToString)
                            Else
                                psb.Append(ch) 'keep adding to current param
                            End If

                        ElseIf ch.Equals(",") Then
                            'add to params
                            If psb.Length = 0 Then Throw New Exception("Argument expected.")
                            parms.Add(psb.ToString)

                        Else
                            If sb IsNot Nothing Then
                                'keep adding to method
                                sb.Append(ch)
                            Else
                                'or to current param
                                psb.Append(ch)
                            End If

                        End If
                    End While

                Catch ex As Exception
                    Throw ex

                Finally
                    sr.Close()
                    sr.Dispose()
                    sr = Nothing

                End Try
                If sb IsNot Nothing Then Me.Method = sb.ToString

                'Cast params appropriately
                For Each p In parms
                    Dim st As New Syntax.SyntaxToken(p, 0, SyntaxToken.ElementTypes.NotSet)
                    Select Case st.Type
                        Case SyntaxToken.ElementTypes.Variable
                            Throw New Exception("Invalid argument '" & st.Text & "' for method '" & Me.Method & "'.")
                        Case Else
                            'send in the "metadrone'd" value
                            Me.Params.Add(st.Value.ToString)
                    End Select
                Next
            End Sub

        End Class

        Public Property CompilerErrors() As CompilerErrorCollection
            Get
                Return Me.mCompilerErrors
            End Get
            Set(ByVal Value As CompilerErrorCollection)
                Me.mCompilerErrors = Value
            End Set
        End Property

        'Refer http://www.vbforums.com/showthread.php?t=397265
        Public Sub New(ByVal vbSource As String, ByVal csSource As String)
            '' Setup the Compiler Parameters, add any referenced assemblies
            'Dim params As CompilerParameters = New CompilerParameters
            'params.ReferencedAssemblies.Add("system.dll")
            'params.CompilerOptions = "/t:library"
            'params.GenerateInMemory = True

            'Build VB code
            If vbSource.Length > 0 Then
                Dim provider As New VBCodeProvider   'Create a new VB Code Compiler
                Dim cp As New CompilerParameters     'Create a new Compiler parameter object.
                cp.GenerateExecutable = False        'Don't create an object on disk
                cp.GenerateInMemory = True           'But do create one in memory.
                'If cp.OutputAssembly is used with a VBCodeProvider, it seems to want to read before it is executed.  
                'See C# CodeBank example for explanation of why it was used.

                'Create a compiler output results object and compile the source code.
                Me.VB_CR = provider.CompileAssemblyFromSource(cp, vbSource)
                If Me.VB_CR.Errors.Count > 0 Then
                    'Compiler error
                    Dim sb As New System.Text.StringBuilder("VB compilation error(s):")
                    sb.AppendLine()
                    For i As Integer = 0 To Me.VB_CR.Errors.Count - 1
                        If i = 100 Then Exit For
                        sb.AppendLine("  " & Me.VB_CR.Errors(i).ErrorText)
                    Next
                    Throw New Exception(sb.ToString)
                End If
            End If

            'Build C Sharp code
            If csSource.Length > 0 Then
                Dim provider As New CSharpCodeProvider  'Create a new VB Code Compiler
                Dim cp As New CompilerParameters        'Create a new Compiler parameter object.
                cp.GenerateExecutable = False           'Don't create an object on disk
                cp.GenerateInMemory = True              'But do create one in memory.
                cp.OutputAssembly = "TempModule"        'This is not necessary, however, if used repeatedly, causes the CLR to not need to load a new assembly each time the function is run.

                'Create a compiler output results object and compile the source code.
                Me.CS_CR = provider.CompileAssemblyFromSource(cp, csSource)
                If Me.CS_CR.Errors.Count > 0 Then
                    'Compiler error
                    Dim sb As New System.Text.StringBuilder("C# compilation error(s):")
                    sb.AppendLine()
                    For i As Integer = 0 To Me.CS_CR.Errors.Count - 1
                        If i = 100 Then Exit For
                        sb.AppendLine("  " & Me.CS_CR.Errors(i).ErrorText)
                    Next
                    Throw New Exception(sb.ToString)
                End If
            End If
        End Sub

        Public Function RunVB(ByVal expression As String) As Object
            If Me.VB_CR Is Nothing Then Throw New Exception("No VB.Net code compiled.")

            'Get class and method
            Dim instinf As New InstanceMethodInfo(expression)
            Dim instance As Object = Me.VB_CR.CompiledAssembly.CreateInstance(instinf.Instance)
            If instance Is Nothing Then Throw New Exception("Could not instantiate '" & instinf.Instance & "'.")

            Dim methinf As MethodInfo = instance.GetType.GetMethod(instinf.Method)
            If methinf Is Nothing Then Throw New Exception("Could not invoke '" & instinf.Method & "'.")

            Dim parinf() As System.Reflection.ParameterInfo = methinf.GetParameters()

            Dim parms As New List(Of Object)
            For i As Integer = 0 To instinf.Params.Count - 1
                If parinf.Count <= i Then Throw New Exception("Too many arguments.")

                parms.Add(Convert.ChangeType(instinf.Params(i), parinf(i).ParameterType))
            Next

            Return methinf.Invoke(instance, parms.ToArray)
        End Function

        Public Function RunCS(ByVal expression As String) As Object
            If Me.CS_CR Is Nothing Then Throw New Exception("No C# code compiled.")

            'Get class and method
            Dim instinf As New InstanceMethodInfo(expression)
            Dim instance As Object = Me.CS_CR.CompiledAssembly.CreateInstance(instinf.Instance)
            If instance Is Nothing Then Throw New Exception("Could not instantiate '" & instinf.Instance & "'.")

            Dim methinf As MethodInfo = instance.GetType.GetMethod(instinf.Method)
            If methinf Is Nothing Then Throw New Exception("Could not invoke '" & instinf.Method & "'.")

            Dim parinf() As System.Reflection.ParameterInfo = methinf.GetParameters()

            Dim parms As New List(Of Object)
            For i As Integer = 0 To instinf.Params.Count - 1
                If parinf.Count <= i Then Throw New Exception("Too many arguments.")

                parms.Add(Convert.ChangeType(instinf.Params(i), parinf(i).ParameterType))
            Next

            Return methinf.Invoke(instance, parms.ToArray)
        End Function



        'Public Function Eval(ByVal Expression As String) As Object
        '    Dim oCodeProvider As VBCodeProvider = New VBCodeProvider
        '    ' Obsolete in 2.0 framework
        '    ' Dim oICCompiler As ICodeCompiler = oCodeProvider.CreateCompiler


        '    ' Compile and get results 
        '    ' 2.0 Framework - Method called from Code Provider
        '    Dim oCResults As CompilerResults = oCodeProvider.CompileAssemblyFromSource(Me.oCParams, Expression)
        '    ' 1.1 Framework - Method called from CodeCompiler Interface
        '    ' cr = oICCompiler.CompileAssemblyFromSource (cp, sb.ToString)

        '    ' Check for compile time errors 
        '    If oCResults.Errors.Count <> 0 Then
        '        Me.CompilerErrors = oCResults.Errors
        '        Dim sbErr As New System.Text.StringBuilder("Evaluation failed.")
        '        For Each comperr As CompilerError In Me.CompilerErrors
        '            sbErr.AppendLine(comperr.ToString)
        '        Next
        '        Throw New Exception(sbErr.ToString)

        '    Else
        '        ' No Errors On Compile, so continue to process...
        '        Dim oAssy As System.Reflection.Assembly = oCResults.CompiledAssembly
        '        Dim oExecInstance As Object = oAssy.CreateInstance("dValuate.EvalRunTime")

        '        Dim oType As Type = oExecInstance.GetType
        '        Dim oMethodInfo As MethodInfo = oType.GetMethod("EvaluateIt")
        '        Return oMethodInfo.Invoke(oExecInstance, Nothing)

        '    End If
        'End Function

    End Class

End Namespace