Imports System.Linq.Expressions
Imports System.Reflection
Imports System.Text

Namespace DataStore

    Public Class SqlExpressionVisitor
        Inherits ExpressionVisitor

        Private ReadOnly predicate As Func(Of Object, Boolean)

        Public Sub New(predicate As Func(Of Object, Boolean))
            Me.predicate = predicate
        End Sub

        Public Function GetLinqFunction(expression As Expression) As Func(Of Object, Boolean)
            Dim whereClauseExpression = DirectCast(Visit(expression), Expression(Of Func(Of Object, Boolean)))
            Dim parameter = Expression.Parameter(GetType(Object), "obj")
            Dim combinedExpression = Expression.Lambda(Of Func(Of Object, Boolean))(Expression.AndAlso(Expression.Invoke(Expression.Constant(predicate), parameter), Expression.Invoke(whereClauseExpression, parameter)), parameter)
            Return combinedExpression.Compile()
        End Function

        Protected Overrides Function VisitMember(node As MemberExpression) As Expression
            Dim parameter = Expression.Parameter(GetType(Object), "obj")
            Dim propertyValue = Expression.PropertyOrField(Expression.Convert(parameter, node.Expression.Type), node.Member.Name)
            Dim lambda = Expression.Lambda(Of Func(Of Object, Object))(propertyValue, parameter)
            Dim value = lambda.Compile()
            Return Expression.Lambda(Of Func(Of Object, Boolean))(Expression.Equal(propertyValue, Expression.Constant(value)), parameter)
        End Function

        Protected Overrides Function VisitBinary(node As BinaryExpression) As Expression
            Dim left = Visit(node.Left)
            Dim right = Visit(node.Right)

            Dim parameter = Expression.Parameter(GetType(Object), "obj")

            Return Expression.Lambda(Of Func(Of Object, Boolean))(EvaluateBinaryExpression(node.NodeType, left, right, parameter), parameter)
        End Function

        Private Function EvaluateBinaryExpression(nodeType As ExpressionType, leftExpr As Expression, rightExpr As Expression, parameter As ParameterExpression) As Expression
            Dim left = Expression.Invoke(leftExpr, parameter)
            Dim right = Expression.Invoke(rightExpr, parameter)

            Select Case nodeType
                Case ExpressionType.Equal
                    Return Expression.Equal(left, right)
                Case ExpressionType.NotEqual
                    Return Expression.NotEqual(left, right)
                Case ExpressionType.GreaterThan
                    Return Expression.GreaterThan(left, right)
                Case ExpressionType.GreaterThanOrEqual
                    Return Expression.GreaterThanOrEqual(left, right)
                Case ExpressionType.LessThan
                    Return Expression.LessThan(left, right)
                Case ExpressionType.LessThanOrEqual
                    Return Expression.LessThanOrEqual(left, right)
                Case Else
                    Throw New NotSupportedException($"Unsupported binary operator: {nodeType}")
            End Select
        End Function
    End Class
    'Public Class SqlExpressionVisitor
    '    Inherits ExpressionVisitor

    '    Private ReadOnly parameters As List(Of SqlParameterInfo)

    '    Public Sub New()
    '        Me.parameters = New List(Of SqlParameterInfo)()
    '    End Sub

    '    Public Function GetSqlExpression(expression As Expression) As String
    '        Try
    '            Visit(expression)

    '            Dim sqlExpression As New StringBuilder()

    '            For Each param In parameters
    '                If sqlExpression.Length > 0 Then
    '                    sqlExpression.Append(" AND ")
    '                End If
    '                sqlExpression.Append(param.SqlComparison)
    '            Next

    '            Return sqlExpression.ToString()

    '        Catch ex As Exception
    '            Dim lsMessage As String
    '            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

    '            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
    '            lsMessage &= vbCrLf & vbCrLf & ex.Message
    '            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)

    '            Return ""
    '        End Try
    '    End Function

    '    Protected Overrides Function VisitMember(node As MemberExpression) As Expression
    '        Try
    '            If node.Expression IsNot Nothing AndAlso node.Expression.NodeType = ExpressionType.Parameter Then
    '                Dim paramName As String = $"@param_{parameters.Count}"
    '                Dim sqlComparison As String = $"{node.Member.Name} = {paramName}"
    '                parameters.Add(New SqlParameterInfo(paramName, sqlComparison))
    '            End If
    '            Return MyBase.VisitMember(node)

    '        Catch ex As Exception
    '            Dim lsMessage As String
    '            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

    '            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
    '            lsMessage &= vbCrLf & vbCrLf & ex.Message
    '            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)

    '            Return Nothing
    '        End Try
    '    End Function

    '    Protected Overrides Function VisitConstant(node As ConstantExpression) As Expression
    '        Try
    '            Dim paramName As String = $"@param_{parameters.Count}"
    '            Dim sqlComparison As String = $"= {paramName}"
    '            parameters.Add(New SqlParameterInfo(paramName, sqlComparison))
    '            Return Expression.Parameter(node.Type, paramName)

    '        Catch ex As Exception
    '            Dim lsMessage As String
    '            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

    '            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
    '            lsMessage &= vbCrLf & vbCrLf & ex.Message
    '            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)

    '            Return Nothing
    '        End Try
    '    End Function

    '    Protected Overrides Function VisitBinary(node As BinaryExpression) As Expression
    '        Try
    '            Dim left = Visit(node.Left)
    '            Dim right = Visit(node.Right)

    '            If TypeOf left Is ParameterExpression AndAlso TypeOf right Is ParameterExpression Then
    '                ' Both sides are parameter expressions, return the binary expression as it is
    '                Return Expression.MakeBinary(node.NodeType, left, right)
    '            Else
    '                ' At least one side is a constant value or member, generate a comparison expression
    '                Dim comparisonExpression = Expression.MakeBinary(node.NodeType, left, right)
    '                Dim sqlComparison = GetSqlComparison(comparisonExpression)
    '                parameters.Add(New SqlParameterInfo(sqlComparison))
    '                Return Expression.Constant(sqlComparison)
    '            End If

    '        Catch ex As Exception
    '            Dim lsMessage As String
    '            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

    '            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
    '            lsMessage &= vbCrLf & vbCrLf & ex.Message
    '            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)

    '            Return Nothing
    '        End Try
    '    End Function

    '    Private Function GetSqlComparison(expression As Expression) As String
    '        If TypeOf expression Is BinaryExpression Then
    '            Dim binaryExpression = DirectCast(expression, BinaryExpression)
    '            Dim left = GetSqlValue(binaryExpression.Left)
    '            Dim right = GetSqlValue(binaryExpression.Right)
    '            Dim [operator] = GetSqlOperator(binaryExpression.NodeType)
    '            Return $"{left} {[operator]} {right}"
    '        ElseIf TypeOf expression Is MemberExpression Then
    '            Dim memberExpression = DirectCast(expression, MemberExpression)
    '            Dim memberName = memberExpression.Member.Name

    '            If memberExpression.Expression IsNot Nothing AndAlso memberExpression.Expression.NodeType = ExpressionType.Parameter Then
    '                ' If the member is accessed on a parameter (i.e., property or field of the entity),
    '                ' we need to create a parameterized SQL comparison expression.
    '                Dim paramName As String = $"@param_{parameters.Count}"
    '                Dim sqlComparison As String = $"{memberName} = {paramName}"
    '                parameters.Add(New SqlParameterInfo(paramName, sqlComparison))
    '                Return sqlComparison
    '            Else
    '                ' If the member is accessed on a constant or member of a constant, we need to get the value
    '                ' and create a SQL comparison directly.
    '                Dim value = Expression.Lambda(memberExpression).Compile().DynamicInvoke()
    '                Return $"{memberName} = {GetSqlValue(value)}"
    '            End If
    '        ElseIf TypeOf expression Is ConstantExpression Then
    '            Dim constantExpression = DirectCast(expression, ConstantExpression)
    '            Dim value = constantExpression.Value
    '            Return GetSqlValue(value)
    '        Else
    '            Throw New NotSupportedException($"Unsupported expression type: {expression.NodeType}")
    '        End If
    '    End Function

    '    Private Function GetSqlOperator(nodeType As ExpressionType) As String
    '        Select Case nodeType
    '            Case ExpressionType.Equal
    '                Return "="
    '            Case ExpressionType.NotEqual
    '                Return "<>"
    '            Case ExpressionType.GreaterThan
    '                Return ">"
    '            Case ExpressionType.GreaterThanOrEqual
    '                Return ">="
    '            Case ExpressionType.LessThan
    '                Return "<"
    '            Case ExpressionType.LessThanOrEqual
    '                Return "<="
    '            Case Else
    '                Throw New NotSupportedException($"Unsupported binary operator: {nodeType}")
    '        End Select
    '    End Function

    '    Private Function GetSqlValue(value As Object) As String
    '        ' Helper method to format the parameter value as a string for SQL query
    '        If value Is Nothing Then
    '            Return "NULL"
    '        ElseIf TypeOf value Is String Then
    '            Return $"'{EscapeSqlString(value.ToString())}'"
    '        ElseIf TypeOf value Is DateTime Then
    '            Dim dateTimeValue As DateTime = DirectCast(value, DateTime)
    '            Return $"'{dateTimeValue.ToString("yyyy/MM/dd HH:mm:ss")}'"
    '        ElseIf TypeOf value Is Boolean Then
    '            Return If(DirectCast(value, Boolean), "1", "0")
    '        Else
    '            Return value.ToString()
    '        End If
    '    End Function

    '    Private Function EscapeSqlString(value As String) As String
    '        ' Handle single quotes in the string by escaping them with another single quote.
    '        ' E.g., "John's car" becomes "John''s car".
    '        Return value.Replace("'", "''")
    '    End Function

    '    Private Class SqlParameterInfo
    '        Public Property Name As String
    '        Public Property SqlComparison As String

    '        Public Sub New(name As String, sqlComparison As String)
    '            Me.Name = name
    '            Me.SqlComparison = sqlComparison
    '        End Sub

    '        Public Sub New(sqlComparison As String)
    '            ' The constructor for constants where name doesn't matter
    '            Me.Name = Nothing
    '            Me.SqlComparison = sqlComparison
    '        End Sub
    '    End Class
    'End Class
End Namespace