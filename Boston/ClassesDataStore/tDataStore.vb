Imports System.Data.SQLite
Imports System.Linq.Expressions
Imports Newtonsoft.Json
Imports Newtonsoft.Json.Serialization
Imports System.IO
Imports System.Text.RegularExpressions
Imports System.Reflection
Imports System.Text
Imports System.Runtime.CompilerServices
Imports DocumentFormat.OpenXml.Wordprocessing
Imports System.ComponentModel.DataAnnotations.Schema
Imports System.Data.Entity.Infrastructure
Imports System.Threading
Imports Boston.DFD

Namespace DataStore
    Public Class [Store]

        'EXAMPLE
        ' Define the LINQ expression for the condition
        'Dim lrDataStore As New DataStore.Store
        'Dim whereClause As Expression(Of Func(Of FBM.DictionaryEntry, Boolean)) = Function(p) p.Symbol = "Satellite"
        'Dim larDictionaryEntry As List(Of FBM.DictionaryEntry) = lrDataStore.GetData(Of FBM.DictionaryEntry)(whereClause)

        ''' <summary>
        '''  AddObject method to add a new record to the DataStore table
        ''' </summary>
        ''' <param name="arObject"></param>
        Public Sub Add(arObject As Object)
            Try
                'Usage Example:
                'Dim lrEntityType As New FBM.EntityType(Nothing, pcenumLanguage.ORMModel, "Test Entity Type", Nothing, True)
                'Dim lrDataStore As New DataStore.Store
                'Call lrDataStore.AddObject(lrEntityType)

                ' Create the custom JSON serializer settings
                Dim settings As New JsonSerializerSettings With {
                    .Formatting = Formatting.Indented,
                   .TypeNameHandling = TypeNameHandling.Objects,
                   .ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                   }

                ' Serialize the object into JSON with custom settings
                Dim jsonData As String = JsonConvert.SerializeObject(arObject, settings)

                ' Get the type name of the object
                Dim typeName As String = arObject.GetType.FullName

                ' Create the SQL query to insert the new record into the DataStore table
                Dim lrData As New DataStore.Data(jsonData, typeName, Now, "", "", "", "", "")

                ' Execute the insert query
                Call tableDataStore.AddData(lrData)

            Catch ex As Exception
                ' Handle the exception appropriately
                ' ...
            End Try
        End Sub


        Public Sub Add(arData As DataStore.Data)

            Try
                ' Execute the insert query
                Call tableDataStore.AddData(arData)

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
            End Try

        End Sub

#Region "For cascade detele"

#Region "Original Code"
        'Private Shared ReadOnly FKMap As New Dictionary(Of Type, List(Of (DepType As Type, DepMember As MemberInfo, TargetMember As String, Cascade As Boolean)))()
        'Private Shared ReadOnly FKInitLock As New Object()

        'Private Shared Function GetDependents(targetType As Type) As List(Of (DepType As Type, DepMember As MemberInfo, TargetMember As String, Cascade As Boolean))
        '    SyncLock FKInitLock
        '        If Not FKMap.ContainsKey(targetType) Then
        '            Dim list As New List(Of (DepType As Type, DepMember As MemberInfo, TargetMember As String, Cascade As Boolean))

        '            For Each asm In AppDomain.CurrentDomain.GetAssemblies()
        '                For Each t In asm.GetTypes()
        '                    For Each m In t.GetMembers(BindingFlags.Public Or BindingFlags.Instance)
        '                        Dim fk = m.GetCustomAttribute(Of ForeignKeyReferenceAttribute)()
        '                        If fk Is Nothing Then Continue For
        '                        If fk.TargetType Is targetType Then
        '                            list.Add((DepType:=t, DepMember:=m, TargetMember:=fk.TargetMember, Cascade:=fk.CascadeDelete))
        '                        End If
        '                    Next
        '                Next
        '            Next

        '            FKMap(targetType) = list
        '        End If
        '        Return FKMap(targetType)
        '    End SyncLock
        'End Function

        'Private Shared Function GetMemberValue(obj As Object, memberName As String) As Object
        '    Dim pi = obj.GetType().GetProperty(memberName, BindingFlags.Public Or BindingFlags.Instance)
        '    If pi IsNot Nothing Then Return pi.GetValue(obj)
        '    Dim fi = obj.GetType().GetField(memberName, BindingFlags.Public Or BindingFlags.Instance)
        '    If fi IsNot Nothing Then Return fi.GetValue(obj)
        '    Throw New InvalidOperationException($"Member {memberName} not found on {obj.GetType().Name}")
        'End Function

        'Private Shared Function BuildEqLambdaHelper(depType As Type, member As MemberInfo, val As Object) As Object
        '    Dim p = Expression.Parameter(depType, "x")
        '    Dim left As Expression =
        'If(TypeOf member Is PropertyInfo,
        '   Expression.Property(p, DirectCast(member, PropertyInfo)),
        '   Expression.Field(p, DirectCast(member, FieldInfo)))
        '    Dim right = Expression.Constant(val, left.Type)
        '    Dim body = Expression.Equal(left, right)
        '    Dim lambdaType = GetType(Func(Of ,)).MakeGenericType(depType, GetType(Boolean))
        '    Return Expression.Lambda(lambdaType, body, p)
        'End Function

#End Region

        ' Assembly allowlist – adjust to your solution's prefixes or add/remove as needed.
        Private Shared ReadOnly OurAssemblyPrefixes As String() = {"FBM.", "Boston", "FactEngine", "DataStore", "TestManagement"}

        ' Build-once, shared index: TargetType → dependents
        Private Shared ReadOnly FKIndex As New Lazy(Of Dictionary(Of Type, List(Of (DepType As Type, DepMember As MemberInfo, TargetMember As String, Cascade As Boolean))))(
                            Function() BuildFkIndex(),
                            System.Threading.LazyThreadSafetyMode.ExecutionAndPublication)

        Private Shared Function IsOurAssembly(a As Assembly) As Boolean
            If a Is Nothing OrElse a.IsDynamic Then Return False
            Dim n As String = a.GetName().Name
            For Each pfx In OurAssemblyPrefixes
                If n.StartsWith(pfx, StringComparison.Ordinal) Then Return True
            Next
            Return False
        End Function

        Private Shared Function BuildFkIndex() As Dictionary(Of Type, List(Of (DepType As Type, DepMember As MemberInfo, TargetMember As String, Cascade As Boolean)))
            Dim index As New Dictionary(Of Type, List(Of (Type, MemberInfo, String, Boolean)))()

            For Each asm In AppDomain.CurrentDomain.GetAssemblies()
                If Not IsOurAssembly(asm) Then Continue For

                Dim types As Type()
                Try
                    types = asm.GetTypes()
                Catch ex As ReflectionTypeLoadException
                    types = ex.Types
                End Try
                If types Is Nothing Then Continue For

                For Each t In types
                    If t Is Nothing Then Continue For

                    Const flags As BindingFlags = BindingFlags.Instance Or BindingFlags.Public Or BindingFlags.DeclaredOnly

                    ' Only properties and fields – skip methods/events for speed
                    Dim members As New List(Of MemberInfo)
                    members.AddRange(t.GetProperties(flags))
                    members.AddRange(t.GetFields(flags))

                    For Each m In members
                        ' Fast path: inspect CustomAttributeData without constructing the attribute
                        Dim hasFk As Boolean = False
                        Dim targetType As Type = Nothing
                        Dim targetMember As String = Nothing
                        Dim cascade As Boolean = False

                        For Each cad In CustomAttributeData.GetCustomAttributes(m)
                            If cad.AttributeType Is GetType(ForeignKeyReferenceAttribute) Then
                                hasFk = True

                                ' Try read from ctor args (in case your attribute uses constructor parameters)
                                ' Adjust indexes to match your actual attribute signature if different.
                                If cad.ConstructorArguments IsNot Nothing Then
                                    If cad.ConstructorArguments.Count >= 1 AndAlso TypeOf cad.ConstructorArguments(0).Value Is Type Then
                                        targetType = DirectCast(cad.ConstructorArguments(0).Value, Type)
                                    End If
                                    If cad.ConstructorArguments.Count >= 2 AndAlso cad.ConstructorArguments(1).ArgumentType Is GetType(String) Then
                                        targetMember = TryCast(cad.ConstructorArguments(1).Value, String)
                                    End If
                                End If

                                ' Named args fallback / override
                                If cad.NamedArguments IsNot Nothing Then
                                    For Each na In cad.NamedArguments
                                        If na.MemberName = NameOf(ForeignKeyReferenceAttribute.TargetType) AndAlso TypeOf na.TypedValue.Value Is Type Then
                                            targetType = DirectCast(na.TypedValue.Value, Type)
                                        ElseIf na.MemberName = NameOf(ForeignKeyReferenceAttribute.TargetMember) Then
                                            targetMember = TryCast(na.TypedValue.Value, String)
                                        ElseIf na.MemberName = NameOf(ForeignKeyReferenceAttribute.CascadeDelete) Then
                                            cascade = Convert.ToBoolean(na.TypedValue.Value)
                                        End If
                                    Next
                                End If
                                Exit For
                            End If
                        Next

                        ' Slow fallback only if the quick path saw the attribute but couldn't read values
                        If hasFk AndAlso (targetType Is Nothing OrElse String.IsNullOrEmpty(targetMember)) Then
                            Dim fk = m.GetCustomAttribute(Of ForeignKeyReferenceAttribute)()
                            If fk IsNot Nothing Then
                                If targetType Is Nothing Then targetType = fk.TargetType
                                If String.IsNullOrEmpty(targetMember) Then targetMember = fk.TargetMember
                                cascade = fk.CascadeDelete
                            End If
                        End If

                        If hasFk AndAlso targetType IsNot Nothing AndAlso Not String.IsNullOrEmpty(targetMember) Then
                            Dim list As List(Of (Type, MemberInfo, String, Boolean)) = Nothing
                            If Not index.TryGetValue(targetType, list) Then
                                list = New List(Of (Type, MemberInfo, String, Boolean))()
                                index(targetType) = list
                            End If
                            list.Add((t, m, targetMember, cascade))
                        End If
                    Next
                Next
            Next

            Return index
        End Function

        Private Shared Function GetDependents(targetType As Type) As List(Of (DepType As Type, DepMember As MemberInfo, TargetMember As String, Cascade As Boolean))
            Dim map = FKIndex.Value
            Dim list As List(Of (Type, MemberInfo, String, Boolean)) = Nothing
            If map.TryGetValue(targetType, list) Then
                Return list
            End If
            Return New List(Of (Type, MemberInfo, String, Boolean))()
        End Function

        Private Shared Function GetMemberValue(obj As Object, memberName As String) As Object
            Dim pi = obj.GetType().GetProperty(memberName, BindingFlags.Public Or BindingFlags.Instance)
            If pi IsNot Nothing Then Return pi.GetValue(obj)
            Dim fi = obj.GetType().GetField(memberName, BindingFlags.Public Or BindingFlags.Instance)
            If fi IsNot Nothing Then Return fi.GetValue(obj)
            Throw New InvalidOperationException($"Member {memberName} not found on {obj.GetType().Name}")
        End Function

        Private Shared Function BuildEqLambdaHelper(depType As Type, member As MemberInfo, val As Object) As Object
            Dim p = Expression.Parameter(depType, "x")
            Dim left As Expression =
        If(TypeOf member Is PropertyInfo,
           Expression.Property(p, DirectCast(member, PropertyInfo)),
           Expression.Field(p, DirectCast(member, FieldInfo)))
            Dim right = Expression.Constant(val, left.Type)
            Dim body = Expression.Equal(left, right)
            Dim lambdaType = GetType(Func(Of ,)).MakeGenericType(depType, GetType(Boolean))
            Return Expression.Lambda(lambdaType, body, p)
        End Function

#End Region

        ''' <summary>
        ''' Delete method
        ''' </summary>
        ''' <typeparam name="T"></typeparam>
        ''' <param name="whereClause"></param>
        Public Sub Delete(Of T)(whereClause As Expression(Of Func(Of T, Boolean)))
            Try
                'Usage Examples
                'Dim whereClause As Expression(Of Func(Of FBM.DictionaryEntry, Boolean)) = Function(p) p.Symbol = "Satellite"
                'Dim larDictionaryEntry As List(Of FBM.DictionaryEntry) = lrDataStore.GetData(Of FBM.DictionaryEntry)(whereClause)
                'or
                '            Dim lrEntityType As New FBM.EntityType(Nothing, pcenumLanguage.ORMModel, "Test Entity Type", Nothing, True)
                'Dim whereClause As Expression(Of Func(Of FBM.EntityType, Boolean)) = Function(p) p.Id = "Test Entity Type"
                'Dim lrDataStore As New DataStore.Store
                'Call lrDataStore.DeleteData(Of FBM.EntityType)(whereClause)

                ' Get the record that matches the provided whereClause using GetData function
                ' 1. Get the record(s) that match
                Dim asID As String = Nothing
                Dim dataList As List(Of T) = Me.Get(Of T)(whereClause, asID)
                'CodeSafe
                If dataList Is Nothing OrElse dataList.Count = 0 Then Exit Sub

                ' 2. For each root object, cascade delete children first
                For Each root In dataList
                    For Each dep In GetDependents(GetType(T))
                        Dim rootVal = GetMemberValue(root, dep.TargetMember)
                        Dim whereChild = BuildEqLambdaHelper(dep.DepType, dep.DepMember, rootVal)
                        Dim delMethod = Me.GetType().GetMethod("Delete").MakeGenericMethod(dep.DepType)
                        delMethod.Invoke(Me, {whereChild})
                    Next
                Next

                ' Assuming there should be only one record that matches the condition
                If dataList.Count >= 1 And asID IsNot Nothing Then
                    Dim lsSQLQuery As String
                    Select Case dataList.Count
                        Case Is = 1
                            lsSQLQuery = "DELETE FROM DataStore WHERE ID = '" & asID & "'"

                        Case Else

                            Dim typeName As String = GetType(T).FullName

                            lsSQLQuery = $"DELETE FROM DataStore WHERE json_extract(Data, '$.$type') = '{typeName}, {Assembly.GetExecutingAssembly().GetName().Name}'"
                            lsSQLQuery &= " AND " & Me.GenerateJsonWhereClause(whereClause)

                    End Select

                    Call pdbConnection.Execute(lsSQLQuery)
                End If

            Catch ex As Exception
                ' Handle the exception appropriately
                ' ...
            End Try
        End Sub

        Private Shared Function TryEval(e As Expression) As (Ok As Boolean, Rhs As Object)
            Dim c = TryCast(e, ConstantExpression)
            If c IsNot Nothing Then Return (True, c.Value)
            Try
                Dim box As Expression = If(e.Type.IsValueType, Expression.Convert(e, GetType(Object)), e)
                Dim lam = Expression.Lambda(Of Func(Of Object))(box)
                Return (True, lam.Compile(preferInterpretation:=True).Invoke())
            Catch
                Return (False, Nothing)
            End Try
        End Function

        Private Shared Function GetMemberType(m As MemberInfo) As Type
            If TypeOf m Is PropertyInfo Then Return DirectCast(m, PropertyInfo).PropertyType
            If TypeOf m Is FieldInfo Then Return DirectCast(m, FieldInfo).FieldType
            Return GetType(Object)
        End Function

        Private Shared Function ConvertTo(val As Object, target As Type) As Object
            If val Is Nothing Then Return Nothing
            If target.IsInstanceOfType(val) Then Return val
            Return System.Convert.ChangeType(val, If(Nullable.GetUnderlyingType(target), target), Globalization.CultureInfo.InvariantCulture)
        End Function

        Private Shared Iterator Function CartesianEnumerate(sets As IList(Of IList)) As IEnumerable(Of Object())
            Dim n = sets.Count
            If n = 0 Then Return
            Dim idx(n - 1) As Integer
            Dim cur(n - 1) As Object

            While True
                For i = 0 To n - 1
                    cur(i) = If(sets(i).Count > 0, sets(i)(idx(i)), Nothing)
                Next
                Yield DirectCast(cur.Clone(), Object())

                Dim k = n - 1
                While k >= 0
                    idx(k) += 1
                    If idx(k) < sets(k).Count Then Exit While
                    idx(k) = 0 : k -= 1
                End While
                If k < 0 Then Exit While
            End While
        End Function

#Region "Getter - Variable number of virtual tables"

        ' --- Variadic N-ary getter: takes a multi-parameter predicate and returns raw tuples (Object()) ---
        Public Function [Get](ByVal arWhere As LambdaExpression) As List(Of Object())

            ' 0) Validate
            If arWhere Is Nothing Then Throw New ArgumentNullException(NameOf(arWhere))

            Dim larParameters = arWhere.Parameters.ToArray()

            ' 1) Build per-type predicates aligned with parameters (Nothing when unconstrained)
            Dim larPerTypePredicates As LambdaExpression() = BuildPerTypePredicatesVariadic(arWhere)

            ' 2) Fetch each side using Get(Of T)(whereClause, ByRef asID)
            Dim larSourceSets As New List(Of IList)(larParameters.Length)

            For i = 0 To larParameters.Length - 1
                Dim lrType As Type = larParameters(i).Type
                Dim lrPredicate As LambdaExpression = larPerTypePredicates(i) ' may be Nothing
                Dim lsId As String = Nothing

                ' Pick the 2-parameter generic Get(Of T)(whereClause, ByRef asID)
                Dim lrMethod As MethodInfo = Me.GetType().GetMethods(BindingFlags.Public Or BindingFlags.Instance) _
                                                .Where(Function(m) m.Name = "Get" AndAlso m.IsGenericMethodDefinition) _
                                                .First(Function(m) m.GetParameters().Length = 2)

                Dim lrGeneric As MethodInfo = lrMethod.MakeGenericMethod(lrType)
                Dim lrArgs As Object() = {lrPredicate, lsId}   ' Nothing ⇒ no WHERE
                Dim lrResult As Object = lrGeneric.Invoke(Me, lrArgs)

                larSourceSets.Add(DirectCast(lrResult, IList))
            Next

            ' 3) Compile the multi-parameter predicate (in-process)
            Dim lrWhereDelegate As [Delegate] = arWhere.Compile()

            ' 4) Enumerate cartesian product, filter by where, return raw tuples
            Dim larOut As New List(Of Object())()
            For Each lrTuple As Object() In CartesianEnumerate(larSourceSets)
                If CBool(lrWhereDelegate.DynamicInvoke(lrTuple)) Then
                    larOut.Add(lrTuple)
                End If
            Next

            Return larOut
        End Function


        ' --- Build per-parameter simple predicates from an N-ary where (x1,…,xN) => … ---
        ' Produces, for each parameter xi, either:
        '   • xi => xi.Member1 = v1 AndAlso xi.Member2 = v2 ...    (when constraints found)
        '   • Nothing                                              (when unconstrained)
        Private Function BuildPerTypePredicatesVariadic(ByVal arWhere As LambdaExpression) As LambdaExpression()
            Dim apar = arWhere.Parameters.ToArray()

            ' Quick map: ParameterExpression → index
            Dim lParamIndex As New Dictionary(Of ParameterExpression, Integer)(apar.Length)
            For i = 0 To apar.Length - 1
                lParamIndex(apar(i)) = i
            Next

            ' Collect: for each parameter index → list of (Member, Value) constraints
            Dim alGroups As New List(Of List(Of (Member As MemberInfo, Value As Object)))(apar.Length)
            For i = 0 To apar.Length - 1
                alGroups.Add(New List(Of (MemberInfo, Object))())
            Next

            ' Harvest equality constraints from the body
            HarvestEqualities(arWhere.Body, Sub(idx, m, v) alGroups(idx).Add((m, v)), lParamIndex)

            ' Build xi => eq1 AndAlso eq2 ... OR Nothing if no constraints
            Dim aOut(apar.Length - 1) As LambdaExpression
            For i = 0 To apar.Length - 1
                If alGroups(i).Count = 0 Then
                    aOut(i) = Nothing
                Else
                    Dim p As ParameterExpression = Expression.Parameter(apar(i).Type, "x")
                    Dim lBody As Expression = Nothing
                    For Each cond In alGroups(i)
                        Dim lAccess As MemberExpression = Expression.MakeMemberAccess(p, cond.Member)
                        Dim lConst As ConstantExpression = Expression.Constant(ConvertTo(cond.Value, GetMemberType(cond.Member)), GetMemberType(cond.Member))
                        Dim lEq As Expression = Expression.Equal(lAccess, lConst)
                        lBody = If(lBody Is Nothing, lEq, Expression.AndAlso(lBody, lEq))
                    Next
                    Dim funType = GetType(Func(Of ,)).MakeGenericType(apar(i).Type, GetType(Boolean))
                    aOut(i) = Expression.Lambda(funType, lBody, p)
                End If
            Next

            Return aOut
        End Function


        ' --- Walk the N-ary predicate body and report normalized equalities: xi.Member = constant ---
        ' Handles:
        '   • (member == constant) and (constant == member)
        '   • VB: CompareString(member, constant, …) == 0
        '   • .Equals: member.Equals(constant)  (optionally compared to True)
        Private Sub HarvestEqualities(ByVal arExpr As Expression,
                              ByVal arReport As Action(Of Integer, MemberInfo, Object),
                              ByVal arParamIndex As Dictionary(Of ParameterExpression, Integer))

            Select Case arExpr.NodeType
                Case ExpressionType.AndAlso, ExpressionType.And
                    Dim lAnd = DirectCast(arExpr, BinaryExpression)
                    HarvestEqualities(lAnd.Left, arReport, arParamIndex)
                    HarvestEqualities(lAnd.Right, arReport, arParamIndex)

                Case ExpressionType.Equal
                    Dim lBe = DirectCast(arExpr, BinaryExpression)

                    ' Case 1: member == constant  (or swapped)
                    Dim lLeftMem = TryCast(StripConvert(lBe.Left), MemberExpression)
                    Dim lRightConst = TryCast(StripConvert(lBe.Right), ConstantExpression)
                    If lLeftMem IsNot Nothing AndAlso lRightConst IsNot Nothing Then
                        Dim p = TryCast(StripConvert(lLeftMem.Expression), ParameterExpression)
                        If p IsNot Nothing AndAlso arParamIndex.ContainsKey(p) Then
                            arReport(arParamIndex(p), lLeftMem.Member, lRightConst.Value)
                            Exit Sub
                        End If
                    End If
                    Dim lRightMem = TryCast(StripConvert(lBe.Right), MemberExpression)
                    Dim lLeftConst = TryCast(StripConvert(lBe.Left), ConstantExpression)
                    If lRightMem IsNot Nothing AndAlso lLeftConst IsNot Nothing Then
                        Dim p = TryCast(StripConvert(lRightMem.Expression), ParameterExpression)
                        If p IsNot Nothing AndAlso arParamIndex.ContainsKey(p) Then
                            arReport(arParamIndex(p), lRightMem.Member, lLeftConst.Value)
                            Exit Sub
                        End If
                    End If

                    ' Case 2: CompareString(member, constant, …) == 0
                    Dim lCallLeft = TryCast(StripConvert(lBe.Left), MethodCallExpression)
                    Dim lConstRight = TryCast(StripConvert(lBe.Right), ConstantExpression)
                    If lCallLeft IsNot Nothing AndAlso lConstRight IsNot Nothing AndAlso
               lConstRight.Value IsNot Nothing AndAlso lConstRight.Value.Equals(0) AndAlso
               lCallLeft.Method.Name = "CompareString" AndAlso lCallLeft.Arguments.Count >= 2 Then

                        Dim lm = TryCast(StripConvert(lCallLeft.Arguments(0)), MemberExpression)
                        If lm IsNot Nothing Then
                            Dim p = TryCast(StripConvert(lm.Expression), ParameterExpression)
                            If p IsNot Nothing AndAlso arParamIndex.ContainsKey(p) Then
                                Dim rv = TryEval(lCallLeft.Arguments(1))
                                If rv.Ok Then arReport(arParamIndex(p), lm.Member, rv.Rhs)
                                Exit Sub
                            End If
                        End If
                    End If

                    ' Case 3: member.Equals(constant)  [optionally == True on the outside]
                    Dim lCallLeftEq = TryCast(StripConvert(lBe.Left), MethodCallExpression)
                    If lCallLeftEq IsNot Nothing AndAlso lCallLeftEq.Method.Name = "Equals" AndAlso lCallLeftEq.Arguments.Count = 1 Then
                        Dim lm = TryCast(StripConvert(lCallLeftEq.Object), MemberExpression)
                        If lm IsNot Nothing Then
                            Dim p = TryCast(StripConvert(lm.Expression), ParameterExpression)
                            If p IsNot Nothing AndAlso arParamIndex.ContainsKey(p) Then
                                Dim rv = TryEval(lCallLeftEq.Arguments(0))
                                If rv.Ok Then arReport(arParamIndex(p), lm.Member, rv.Rhs)
                                Exit Sub
                            End If
                        End If
                    End If

                Case Else
                    ' ignore other node types (>, <, Contains, etc.) in the variadic splitter;
                    ' your single-type generator will still see them when lrPredicate IsNot Nothing.
            End Select
        End Sub

#End Region

        Public Function [Get](Of T)(Optional whereClause As Expression(Of Func(Of T, Boolean)) = Nothing,
                                    Optional ByRef asID As String = Nothing) As List(Of T)

            Dim dataList As New List(Of T)()

            Try
                'Usage Example:
                'Dim whereClause As Expression(Of Func(Of FBM.EntityType, Boolean)) = Function(p) p.Id = "Test Entity Type"
                'Dim lrDataStore As New DataStore.Store
                'Dim larDictionaryEntry As List(Of FBM.EntityType) = lrDataStore.GetData(Of FBM.EntityType)(whereClause)

                Dim typeName As String = GetType(T).FullName

                Dim lsSQLQuery As String
                If whereClause Is Nothing Then
                    lsSQLQuery = "SELECT ID, Data FROM DataStore WHERE json_extract(Data, '$.$type') = '" & typeName & ", " & Assembly.GetExecutingAssembly().GetName().Name & "'"
                Else
                    If GetType(T) = GetType(DataStore.Data) Then
                        lsSQLQuery = $"SELECT ID, Data FROM DataStore"
                        lsSQLQuery &= $" WHERE {Me.GenerateJsonWhereClause(whereClause)}"
                    Else
                        lsSQLQuery = "SELECT ID, Data FROM DataStore WHERE json_extract(Data, '$.$type') = '" & typeName & ", " & Assembly.GetExecutingAssembly().GetName().Name & "'"
                        lsSQLQuery &= " AND " & Me.GenerateJsonWhereClause(whereClause)
                    End If
                End If

                Dim lrRecordset As New RecordsetProxy
                lrRecordset.ActiveConnection = pdbConnection
                lrRecordset.CursorType = pcOpenStatic

                Call lrRecordset.Open(lsSQLQuery)

                While Not lrRecordset.EOF
                    Dim jsonData As String = lrRecordset("Data").Value
                    Dim jsonReader As JsonTextReader = New JsonTextReader(New StringReader(jsonData))
                    Dim data As T = JsonSerializer.Create().Deserialize(Of T)(jsonReader)

                    If whereClause Is Nothing OrElse whereClause.Compile()(data) Then
                        dataList.Add(data)
                    End If

                    asID = lrRecordset("ID").value

                    lrRecordset.MoveNext()
                End While

                lrRecordset.Close()

                Return dataList

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
            End Try

        End Function

        Private Function GenerateJsonWhereClause(Of T)(whereClause As Expression(Of Func(Of T, Boolean))) As String
            Dim jsonWhereBuilder As New StringBuilder()

            Dim conditions = ExtractConditions(whereClause.Body)
            For Each condition In conditions
                If jsonWhereBuilder.Length > 0 Then
                    jsonWhereBuilder.Append(" AND ")
                End If
                AppendCondition(condition, jsonWhereBuilder)
            Next

            Return jsonWhereBuilder.ToString()
        End Function

        Private Function ExtractConditions(expression As Expression) As IEnumerable(Of BinaryExpression)
            Dim conditions As New List(Of BinaryExpression)
            CollectConditions(expression, conditions)
            Return conditions
        End Function

        Private Sub CollectConditions(expression As Expression, conditions As List(Of BinaryExpression))
            If TypeOf expression Is BinaryExpression Then
                Dim binaryExpression = DirectCast(expression, BinaryExpression)
                If binaryExpression.NodeType = ExpressionType.And OrElse binaryExpression.NodeType = ExpressionType.AndAlso Then
                    CollectConditions(binaryExpression.Left, conditions)
                    CollectConditions(binaryExpression.Right, conditions)
                ElseIf binaryExpression.NodeType = ExpressionType.Equal OrElse binaryExpression.NodeType = ExpressionType.GreaterThan Then
                    conditions.Add(binaryExpression)
                End If
            End If
            ' Add handling for other types of expressions if needed
        End Sub

        Private Sub AppendCondition(binaryExpression As BinaryExpression, jsonWhereBuilder As StringBuilder)
            If binaryExpression.Left.NodeType = ExpressionType.MemberAccess AndAlso binaryExpression.Right.NodeType = ExpressionType.Constant Then
                AppendMemberAccessCondition(binaryExpression, jsonWhereBuilder)
            ElseIf binaryExpression.Left.NodeType = ExpressionType.Call AndAlso binaryExpression.Right.NodeType = ExpressionType.Constant Then
                AppendMethodCallCondition(binaryExpression, jsonWhereBuilder)
            ElseIf binaryExpression.Left.NodeType = ExpressionType.MemberAccess AndAlso binaryExpression.Right.NodeType = ExpressionType.Convert Then
                AppendConvertCondition(binaryExpression, jsonWhereBuilder)
            ElseIf binaryExpression.Left.NodeType = ExpressionType.Convert AndAlso binaryExpression.Right.NodeType = ExpressionType.Convert Then
                AppendMemberToMemberCondition(binaryExpression, jsonWhereBuilder)
            ElseIf binaryExpression.Left.NodeType = ExpressionType.Convert AndAlso binaryExpression.Right.NodeType = ExpressionType.Constant Then
                AppendMemberToConstantCondition(binaryExpression, jsonWhereBuilder)
            End If
            ' Add handling for other types of expressions if needed
        End Sub

        Private Sub AppendConvertCondition(binaryExpression As BinaryExpression, jsonWhereBuilder As StringBuilder)
            Dim memberExpression = DirectCast(binaryExpression.Left, MemberExpression)
            Dim memberName = memberExpression.Member.Name
            Dim unaryExpression = DirectCast(binaryExpression.Right, UnaryExpression)


            Dim constantValue As String
            If TypeOf unaryExpression.Operand Is MemberExpression Then
                Dim memberOperand = DirectCast(unaryExpression.Operand, MemberExpression)
                Dim lambda = Expression.Lambda(memberOperand)
                Dim compiled = lambda.Compile()
                constantValue = compiled.DynamicInvoke().ToString
            Else
                Throw New InvalidOperationException("Unsupported operand type for conversion.")
            End If

            Dim lsComparitor As String = "="
            Select Case binaryExpression.NodeType
                Case Is = ExpressionType.GreaterThan
                    lsComparitor = ">"
            End Select

            jsonWhereBuilder.Append("json_extract(Data, '$.")
            jsonWhereBuilder.Append(memberName)
            jsonWhereBuilder.Append($"') {lsComparitor} '")
            jsonWhereBuilder.Append(constantValue)
            jsonWhereBuilder.Append("'")
        End Sub

        Private Sub AppendMemberAccessCondition(binaryExpression As BinaryExpression, jsonWhereBuilder As StringBuilder)
            Dim memberExpression = DirectCast(binaryExpression.Left, MemberExpression)
            Dim memberName = memberExpression.Member.Name
            Dim constExpr = DirectCast(binaryExpression.Right, ConstantExpression)
            Dim constantValue = constExpr.Value

            jsonWhereBuilder.Append("json_extract(Data, '$.")
            jsonWhereBuilder.Append(memberName)
            jsonWhereBuilder.Append("') = ")

            If TypeOf constantValue Is Boolean Then
                jsonWhereBuilder.Append(If(CBool(constantValue), "1", "0"))
            Else
                jsonWhereBuilder.Append("'"c)
                jsonWhereBuilder.Append(constantValue.ToString())
                jsonWhereBuilder.Append("'"c)
            End If
        End Sub

        Private Sub AppendMemberToMemberCondition(binaryExpression As BinaryExpression, jsonWhereBuilder As StringBuilder)
            ' Extract the Convert expressions and retrieve the underlying MemberExpressions
            Try

                ' Handle the left expression (as a MemberExpression, not a closure)
                Dim leftConvertExpression = DirectCast(binaryExpression.Left, UnaryExpression)
                Dim leftMemberExpression = DirectCast(leftConvertExpression.Operand, MemberExpression)
                Dim leftMemberName As String = leftMemberExpression.Member.Name ' The actual member name (like UserId)

                ' Handle the right expression (which is a closure containing the constant value)
                Dim rightConvertExpression = DirectCast(binaryExpression.Right, UnaryExpression)
                Dim rightMemberExpression = DirectCast(rightConvertExpression.Operand, MemberExpression)
                Dim closureConstant = DirectCast(DirectCast(rightMemberExpression.Expression, ConstantExpression).Value, Object)
                Dim rightLiteralValue = closureConstant.GetType().GetField(rightMemberExpression.Member.Name).GetValue(closureConstant).ToString()

                ' Build the JSON condition comparing the left member to the right literal value
                jsonWhereBuilder.Append("json_extract(Data, '$.")
                jsonWhereBuilder.Append(leftMemberName) ' This is the left side member (e.g., UserId)
                jsonWhereBuilder.Append("') = '")
                jsonWhereBuilder.Append(rightLiteralValue) ' This is the right side literal value (e.g., lsUserId from the closure)
                jsonWhereBuilder.Append("'")

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
            End Try
        End Sub

        Private Sub AppendMemberToConstantCondition(binaryExpression As BinaryExpression, jsonWhereBuilder As StringBuilder)
            ' Extract the Convert expressions and retrieve the underlying MemberExpressions
            Try

                ' Handle the left expression (as a MemberExpression, not a closure)
                Dim leftConvertExpression = DirectCast(binaryExpression.Left, UnaryExpression)
                Dim leftMemberExpression = DirectCast(leftConvertExpression.Operand, MemberExpression)
                Dim leftMemberName As String = leftMemberExpression.Member.Name ' The actual member name (like UserId)

                Dim constantValue = DirectCast(DirectCast(binaryExpression.Right, ConstantExpression).Value, String)


                ' Build the JSON condition comparing the left member to the right literal value
                jsonWhereBuilder.Append("json_extract(Data, '$.")
                jsonWhereBuilder.Append(leftMemberName) ' This is the left side member (e.g., UserId)

                Select Case constantValue
                    Case Is = Nothing
                        jsonWhereBuilder.Append(constantValue)
                        jsonWhereBuilder.Append("') IS NULL")
                    Case Else
                        jsonWhereBuilder.Append(constantValue)
                        jsonWhereBuilder.Append("') = '")
                        jsonWhereBuilder.Append(constantValue) ' This is the right side literal value (e.g., lsUserId from the closure)
                        jsonWhereBuilder.Append("'")
                End Select

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
            End Try
        End Sub

        Public Function GetNestedPropertyValue(obj As Object, propertyPath As String) As Object
            Dim parts As String() = propertyPath.Split("."c)
            Dim currentObj As Object = obj
            Dim lasParts = parts.ToList
            lasParts.RemoveAt(parts.Count - 1)
            parts = lasParts.ToArray

            For Each part As String In parts
                If currentObj Is Nothing Then Return Nothing
                Dim propInfo As PropertyInfo = currentObj.GetType().GetProperty(part, BindingFlags.Public Or BindingFlags.NonPublic Or BindingFlags.Instance)
                If propInfo Is Nothing Then Return currentObj
                currentObj = propInfo.GetValue(currentObj, Nothing)
            Next

            Return currentObj
        End Function

        Public Shared Function GetMemberExpressionValue(expression As MemberExpression) As Object
            Dim dependencyChain As New List(Of MemberExpression)()
            Dim pointingExpression As MemberExpression = expression
            While pointingExpression IsNot Nothing
                dependencyChain.Add(pointingExpression)
                pointingExpression = TryCast(pointingExpression.Expression, MemberExpression)
            End While

            Dim baseExpression As ConstantExpression = TryCast(dependencyChain.Last().Expression, ConstantExpression)
            If baseExpression Is Nothing Then
                Throw New Exception($"Last expression {dependencyChain.Last().Expression} of dependency chain of {expression} is not a constant." & "Thus the expression value cannot be found.")
            End If

            Dim resolvedValue As Object = baseExpression.Value

            For i As Integer = dependencyChain.Count To 1 Step -1
                Dim expr As MemberExpression = dependencyChain(i - 1)
                resolvedValue = New PropOrField(expr.Member).GetValue(resolvedValue)
            Next

            Return resolvedValue
        End Function

#Region "NewAppendMethodCallCondition"

        Private Sub AppendMethodCallCondition(binaryExpression As BinaryExpression, jsonWhereBuilder As StringBuilder)

            Dim callExpr = DirectCast(binaryExpression.Left, MethodCallExpression)

            ' Find the target member: either obj.Method(...) or Method(target, ...)
            Dim target As MemberExpression = TryCast(StripConvert(callExpr.Object), MemberExpression)
            If target Is Nothing AndAlso callExpr.Arguments.Count > 0 Then
                target = TryCast(StripConvert(callExpr.Arguments(0)), MemberExpression)
            End If
            If target Is Nothing Then Exit Sub

            Dim leftName As String = GetTopLevelMemberName(target)

            Dim op As String
            Dim rhs As Object

            Select Case callExpr.Method.Name
                Case "Equals", "CompareString"
                    rhs = EvalToObject(callExpr.Arguments(If(callExpr.Object Is Nothing, 1, 0)))
                    op = "="
                Case "Contains"
                    rhs = "%" & ToInvariantString(EvalToObject(callExpr.Arguments(0))) & "%"
                    op = "LIKE"
                Case "StartsWith"
                    rhs = ToInvariantString(EvalToObject(callExpr.Arguments(0))) & "%"
                    op = "LIKE"
                Case "EndsWith"
                    rhs = "%" & ToInvariantString(EvalToObject(callExpr.Arguments(0)))
                    op = "LIKE"
                Case Else
                    Exit Sub
            End Select

            jsonWhereBuilder.
                    Append("json_extract(Data, '$.").
                    Append(leftName).
                    Append("') ").
                    Append(op).
                    Append(" '").
                    Append(SafeSql(rhs)).
                    Append("'")

        End Sub

        ' --- helpers ---

        Private Shared Function StripConvert(e As Expression) As Expression
            Dim u = TryCast(e, UnaryExpression)
            If u IsNot Nothing AndAlso (u.NodeType = ExpressionType.Convert OrElse u.NodeType = ExpressionType.ConvertChecked) Then
                Return StripConvert(u.Operand)
            End If
            Return e
        End Function

        Private Shared Function GetTopLevelMemberName(m As MemberExpression) As String
            Dim cur As Expression = m
            Dim last As MemberExpression = m
            While TypeOf cur Is MemberExpression
                last = DirectCast(cur, MemberExpression)
                cur = last.Expression
            End While
            ' last is the member whose Expression is the parameter (top-level on the entity)
            Return last.Member.Name
        End Function

        Private Shared Function EvalToObject(e As Expression) As Object
            Dim expr = StripConvert(e)
            Dim c = TryCast(expr, ConstantExpression)
            If c IsNot Nothing Then Return c.Value
            Dim boxed As Expression = If(expr.Type.IsValueType, Expression.Convert(expr, GetType(Object)), expr)
            Dim lam = Expression.Lambda(Of Func(Of Object))(boxed)
            Return lam.Compile().Invoke()
        End Function

        Private Shared Function ToInvariantString(o As Object) As String
            Return If(o Is Nothing, "", Convert.ToString(o, Globalization.CultureInfo.InvariantCulture))
        End Function

        Private Shared Function SafeSql(o As Object) As String
            Return ToInvariantString(o).Replace("'", "''")
        End Function

#End Region

#Region "Old AppendMethodCallCondition"

        '        Private Sub AppendMethodCallCondition(binaryExpression As BinaryExpression, jsonWhereBuilder As StringBuilder)

        '            Try

        '                Dim methodCallExpression = DirectCast(binaryExpression.Left, MethodCallExpression)
        '                Dim visitor = New ClosureExtractorVisitor()
        '                visitor.Visit(methodCallExpression)

        '                ' Extract the member name and value
        '                Dim extractedMemberName As String = Nothing
        '                Dim extractedMemberValue As Object = Nothing
        '                Dim SecondMemberName As String = "<Error>"
        '                Dim SecondMemberType As Object = Nothing
        '                Dim SecondMemberValue As Object = Nothing

        '                Dim constantValue As String = "<Error>"

        '                If visitor.ExtractedValues.Count > 0 Then
        '                    ' Assuming the first extracted value is the one we need
        '                    Dim firstExtractedValue = visitor.ExtractedValues.First()
        '                    If TypeOf firstExtractedValue.Key Is MemberExpression Then
        '                        Dim memberExpr = DirectCast(firstExtractedValue.Key, MemberExpression)
        '                        extractedMemberName = memberExpr.Member.Name

        '                        extractedMemberValue = visitor.ExtractedValues.ToList(1).Value

        '                        SecondMemberType = visitor.ExtractedValues.ToList(1).Key.Type

        '                        SecondMemberValue = Me.GetMemberExpressionValue(visitor.ExtractedValues.ToList(1).Key)

        '                    End If

        '                    If binaryExpression.Right.NodeType = ExpressionType.Constant Then
        '                        If SecondMemberValue IsNot Nothing Then
        '                            Select Case SecondMemberValue.GetType
        '                                Case Is = GetType(System.String),
        '                                          GetType(System.Int16)
        '                                    constantValue = SecondMemberValue
        '                                    GoTo FoundValues
        '                                Case Else

        '                            End Select
        '                        End If
        '                    End If

        '                    Dim lsFullSecondMemberPath As String = Nothing
        '                    Dim input As String = binaryExpression.ToString
        '                    Dim pattern As String = ".*\.([A-Za-z0-9_]+)" 'Was "\.(?<MemberName>[A-Za-z0-9_]+)(?![^.]*\.[A-Za-z0-9_])"
        '                    Dim match As Match = Regex.Match(input, pattern)

        '                    If match.Success Then
        '                        SecondMemberName = match.Groups(1).Value 'Was "MemberName").Value
        '                    End If


        '                    Try
        '                        lsFullSecondMemberPath = input.Split(",")(2).Split(":")(1)
        '                        Dim splitParts As String() = lsFullSecondMemberPath.Split("."c)
        '                        lsFullSecondMemberPath = String.Join(".", splitParts, 1, splitParts.Length - 1)
        '                    Catch ex As Exception
        '                        lsFullSecondMemberPath = SecondMemberName
        '                    End Try



        '                    Try
        '                        If SecondMemberName <> lsFullSecondMemberPath And lsFullSecondMemberPath.Split(".").Count > 2 Then
        '                            SecondMemberName = lsFullSecondMemberPath
        '                        End If
        '                    Catch ex As Exception
        '                        'We tried
        '                    End Try


        '                End If

        '                If extractedMemberName IsNot Nothing Then

        '                    ' First try to get it as a property
        '                    Dim memberInfo As MemberInfo = extractedMemberValue.GetType().GetProperty(SecondMemberName, BindingFlags.Public Or BindingFlags.NonPublic Or BindingFlags.Instance)

        '                    ' If not found as a property, try to get it as a field
        '                    If memberInfo Is Nothing Then
        '                        memberInfo = extractedMemberValue.GetType().GetField(SecondMemberName, BindingFlags.Public Or BindingFlags.NonPublic Or BindingFlags.Instance)
        '                    End If

        '                    If memberInfo Is Nothing And SecondMemberName.Contains(".") Then
        '                        memberInfo = GetNestedPropertyValue(extractedMemberValue, SecondMemberName)
        '                    End If

        '                    ' If the member is found (either as a property or field), get its value
        '                    If memberInfo IsNot Nothing Then
        '                        Dim value As Object = Nothing
        '                        If TypeOf memberInfo Is PropertyInfo Then
        '                            value = DirectCast(memberInfo, PropertyInfo).GetValue(extractedMemberValue, Nothing)
        '                        ElseIf TypeOf memberInfo Is FieldInfo Then
        '                            value = DirectCast(memberInfo, FieldInfo).GetValue(extractedMemberValue)
        '                        End If

        '                        If value IsNot Nothing Then
        '                            constantValue = value.ToString()
        '                        End If
        '                    Else
        '                        Try
        '                            memberInfo = SecondMemberValue.GetType().GetProperty(SecondMemberName, BindingFlags.Public Or BindingFlags.NonPublic Or BindingFlags.Instance)

        '                            If memberInfo Is Nothing Then
        '                                Try
        '                                    memberInfo = SecondMemberValue.GetType().GetField(SecondMemberName)
        '                                Catch ex As Exception
        '                                    'We tried
        '                                End Try
        '                            End If

        '                            If memberInfo IsNot Nothing Then
        '                                Dim value As Object = Nothing
        '                                If TypeOf memberInfo Is PropertyInfo Then
        '                                    value = DirectCast(memberInfo, PropertyInfo).GetValue(SecondMemberValue, Nothing)
        '                                ElseIf TypeOf memberInfo Is FieldInfo Then
        '                                    value = DirectCast(memberInfo, FieldInfo).GetValue(SecondMemberValue)
        '                                End If

        '                                If value IsNot Nothing Then
        '                                    constantValue = value.ToString()
        '                                End If
        '                            End If

        '                        Catch ex As Exception
        '                            'We tried
        '                        End Try
        '                    End If
        'FoundValues:
        '                    jsonWhereBuilder.Append("json_extract(Data, '$.")
        '                    jsonWhereBuilder.Append(extractedMemberName)
        '                    jsonWhereBuilder.Append("') = '")
        '                    jsonWhereBuilder.Append(constantValue)
        '                    jsonWhereBuilder.Append("'")
        '                Else
        '                    ' Handle the case where the member name could not be extracted
        '                    ' This may involve logging an error or throwing an exception
        '                End If

        '            Catch ex As Exception
        '                Dim lsMessage As String
        '                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

        '                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
        '                lsMessage &= vbCrLf & vbCrLf & ex.Message
        '                prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        '            End Try
        '        End Sub
#End Region
        Private Function ExtractMemberNameFromMethodCall(methodCallExpression As MethodCallExpression) As String
            ' Assuming the method call is structured in a way where one of the arguments
            ' is a MemberExpression which contains the member name we need.
            For Each arg As Expression In methodCallExpression.Arguments
                If TypeOf arg Is MemberExpression Then
                    Dim memberExpr = DirectCast(arg, MemberExpression)
                    Return memberExpr.Member.Name
                End If
                ' Depending on the structure, you might need to check for other types of expressions here
            Next

            ' If the member name isn't found, return a default value or handle accordingly
            Return String.Empty
        End Function

        ''' <summary>
        ''' Update function to replace the JSON in the Data field with Newtonsoft serialization
        ''' NB Only operates where one record is returned from inner Get. I.e. You should aim to update only one Document.
        ''' </summary>
        ''' <typeparam name="t"></typeparam>
        ''' <param name="arObject"></param>
        ''' <param name="whereClause"></param>
        Public Sub Update(Of t)(arObject As Object, whereClause As Expression(Of Func(Of t, Boolean)))
            Try
                'Usage examples
                'Dim whereClause As Expression(Of Func(Of FBM.DictionaryEntry, Boolean)) = Function(p) p.Symbol = "Satellite"
                'Dim larDictionaryEntry As List(Of FBM.DictionaryEntry) = lrDataStore.GetData(Of FBM.DictionaryEntry)(whereClause)
                'or
                'Dim lrEntityType As New FBM.EntityType(Nothing, pcenumLanguage.ORMModel, "Test Entity Type", Nothing, True)
                'Dim whereClause As Expression(Of Func(Of FBM.EntityType, Boolean)) = Function(p) p.Id = "Test Entity Type"
                'lrEntityType.Symbol = "Testaddb"
                'Dim lrDataStore As New DataStore.Store
                'Call lrDataStore.UpdateData(Of FBM.EntityType)(lrEntityType, whereClause)

                ' Get the record that matches the provided whereClause using GetData function
                Dim asID As String = Nothing

                Dim dataList As List(Of t) = Me.Get(Of t)(whereClause, asID) 'asId is byRef and Updated by Get.

                ' Assuming there should be only one record that matches the condition
                For Each recordToUpdate In dataList '.Count = 1 AndAlso asID IsNot Nothing Then


                    ' Create the custom JSON serializer settings
                    Dim settings As New JsonSerializerSettings With {
                                .Formatting = Formatting.Indented,
                                .TypeNameHandling = TypeNameHandling.Objects,
                                .ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                            }

                    ' Serialize the object into JSON with custom settings
                    Dim jsonData As String = JsonConvert.SerializeObject(arObject, settings)

                    ' Now you can update the record back in the database using prConnection.Execute
                    ' Here's the SQL query to update the Data field for the specific record identified by ID
                    Dim lsSQLQuery As String = "UPDATE DataStore SET Data = '" & jsonData & "' WHERE ID = '" & asID & "'"

                    ' Execute the update query
                    Dim lrRecordset As ORMQL.Recordset = pdbConnection.Execute(lsSQLQuery)

                    If lrRecordset.ErrorReturned Then
                        Throw New Exception(lrRecordset.ErrorString)
                    End If

                    Exit For

                Next

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
            End Try
        End Sub

        Public Sub UpsertWrapper(arObject As Object, whereClause As Object)
            Dim objectType = arObject.GetType()
            Dim upsertMethod = Me.GetType().GetMethod("Upsert").MakeGenericMethod(objectType)
            upsertMethod.Invoke(Me, {arObject, whereClause})
        End Sub

        ''' <summary>
        ''' Upsert method to either update or insert a record in the DataStore table
        ''' </summary>
        ''' <typeparam name="T"></typeparam>
        ''' <param name="arObject"></param>
        ''' <param name="whereClause"></param>
        Public Sub Upsert(Of T)(arObject As Object, whereClause As Expression(Of Func(Of T, Boolean)))
            Try
                ' Get the record that matches the provided whereClause using GetData function
                Dim asID As String = Nothing
                Dim dataList As List(Of T) = Me.Get(Of T)(whereClause, asID)

                ' Serialize the object into JSON with custom settings
                Dim settings As New JsonSerializerSettings With {
                    .Formatting = Formatting.Indented,
                    .TypeNameHandling = TypeNameHandling.Objects,
                    .ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                }
                Dim jsonData As String = JsonConvert.SerializeObject(arObject, settings)
                jsonData = Database.MakeStringSafe(jsonData)

                If dataList.Count >= 1 AndAlso asID IsNot Nothing Then
                    ' If the record exists, update it in the database
                    Dim lsSQLQuery As String = "UPDATE DataStore SET Data = '" & jsonData & "' WHERE ID = '" & asID & "'"
                    Dim lrRecordset As ORMQL.Recordset = pdbConnection.Execute(lsSQLQuery)

                    If lrRecordset.ErrorReturned Then
                        If lrRecordset.ErrorString.Contains("unique") Then
                            'Ignore 
                        Else
                            Throw New Exception(lrRecordset.ErrorString)
                        End If
                    End If
                Else
                    ' If the record doesn't exist, insert it into the database
                    Dim lrData As New DataStore.Data(jsonData, GetType(T).FullName, Now, "", "", "", "", "")
                    Call tableDataStore.AddData(lrData)
                End If

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
            End Try
        End Sub

    End Class

    ' Use a custom ExpressionVisitor to extract values from closures
    Public Class ClosureExtractorVisitor
        Inherits ExpressionVisitor

        Private _extractedValues As New Dictionary(Of Expression, Object)

        Public ReadOnly Property ExtractedValues As Dictionary(Of Expression, Object)
            Get
                Return _extractedValues
            End Get
        End Property

        Protected Overrides Function VisitMember(node As MemberExpression) As Expression
            If TypeOf node.Expression Is ConstantExpression Then
                ' Extracting the value from a constant object
                Dim constantExpr = DirectCast(node.Expression, ConstantExpression)
                Dim closureInstance = constantExpr.Value
                Dim memberInfo = TryCast(node.Member, PropertyInfo)
                If memberInfo IsNot Nothing Then
                    ' If the member is a property, extract the property value
                    Dim value = memberInfo.GetValue(closureInstance, Nothing)
                    _extractedValues(node) = value
                Else
                    _extractedValues(node) = constantExpr.Value
                    ' If it's not a property, handle accordingly (e.g., fields)
                    ' Implementation depends on your specific scenario
                End If
            ElseIf TypeOf node.Expression Is ParameterExpression Then
                ' Handling parameter expressions (Note: actual value cannot be determined here)
                Dim parameterExpr = DirectCast(node.Expression, ParameterExpression)
                _extractedValues(node) = parameterExpr
            Else
                ' Recursively handle other types of expressions
                Visit(node.Expression)
            End If

            Return MyBase.VisitMember(node)
        End Function

    End Class

    Public Class PropOrField
        Public ReadOnly MemberInfo As MemberInfo

        Public Sub New(memberInfo As MemberInfo)
            If Not (TypeOf memberInfo Is PropertyInfo) AndAlso Not (TypeOf memberInfo Is FieldInfo) Then
                Throw New Exception($"{NameOf(memberInfo)} must either be {NameOf(PropertyInfo)} or {NameOf(FieldInfo)}")
            End If

            Me.MemberInfo = memberInfo
        End Sub

        Public Function GetValue(source As Object) As Object
            If TypeOf MemberInfo Is PropertyInfo Then
                Return CType(MemberInfo, PropertyInfo).GetValue(source)
            ElseIf TypeOf MemberInfo Is FieldInfo Then
                Return CType(MemberInfo, FieldInfo).GetValue(source)
            End If

            Return Nothing
        End Function

        Public Sub SetValue(target As Object, source As Object)
            If TypeOf MemberInfo Is PropertyInfo Then
                CType(MemberInfo, PropertyInfo).SetValue(target, source)
            ElseIf TypeOf MemberInfo Is FieldInfo Then
                CType(MemberInfo, FieldInfo).SetValue(target, source)
            End If
        End Sub

        Public Function GetMemberType() As Type
            If TypeOf MemberInfo Is PropertyInfo Then
                Return CType(MemberInfo, PropertyInfo).PropertyType
            ElseIf TypeOf MemberInfo Is FieldInfo Then
                Return CType(MemberInfo, FieldInfo).FieldType
            End If

            Return Nothing
        End Function
    End Class

End Namespace
