Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text

Public Module BostonDerivationRenderer

    Private Const HtmlBreak As String = "<br/>"

    Private NotInheritable Class ModelObjectReferenceComparer
        Implements IEqualityComparer(Of FBM.ModelObject)

        Public Shared ReadOnly Instance As New ModelObjectReferenceComparer

        Private Sub New()
        End Sub

        Public Overloads Function Equals(
            ByVal arLeft As FBM.ModelObject,
            ByVal arRight As FBM.ModelObject) As Boolean _
            Implements IEqualityComparer(Of FBM.ModelObject).Equals

            Return Object.ReferenceEquals(arLeft,
                                          arRight)
        End Function

        Public Overloads Function GetHashCode(
            ByVal arObject As FBM.ModelObject) As Integer _
            Implements IEqualityComparer(Of FBM.ModelObject).GetHashCode

            If arObject Is Nothing Then Return 0
            Return System.Runtime.CompilerServices.RuntimeHelpers.
                GetHashCode(arObject)
        End Function
    End Class

    ' A derivation is rendered in three stages:
    '   1. Resolve the ordered PathedRoles to Boston Roles.
    '   2. Partition each role string into fact occurrences and resolve the exact
    '      FactTypeReading for every occurrence.
    '   3. Verbalise the resulting plan while tracking logical object identity.
    '
    ' No text is used to infer joins. Object identity comes from path continuity,
    ' projections and ObjectUnifiers. Predicate wording comes only from FTRs.

    Private Class RenderContext
        Public Property Model As FBM.Model
        Public Property DerivedFactType As FBM.FactType
        Public Property DerivationPath As FBM.FactTypeDerivationPath
        Public Property RolePath As FBM.RolePath
        Public ReadOnly Property RolePaths As New List(Of FBM.RolePath)

        Public ReadOnly Property RootById As New Dictionary(Of String, FBM.RootObjectType)(StringComparer.OrdinalIgnoreCase)
        Public ReadOnly Property PathedRoleById As New Dictionary(Of String, FBM.PathedRole)(StringComparer.OrdinalIgnoreCase)
        Public ReadOnly Property RoleByPathedRoleId As New Dictionary(Of String, FBM.Role)(StringComparer.OrdinalIgnoreCase)
        Public ReadOnly Property ParentByKey As New Dictionary(Of String, String)(StringComparer.OrdinalIgnoreCase)
        Public ReadOnly Property HeadSourceByRoleId As New Dictionary(Of String, FBM.DerivationSource)(StringComparer.OrdinalIgnoreCase)
        Public ReadOnly Property AllHeadSources As New List(Of FBM.DerivationSource)
        Public ReadOnly Property CalculatedValueById As New Dictionary(Of String, FBM.CalculatedValue)(StringComparer.OrdinalIgnoreCase)
        Public ReadOnly Property HeadBindingKeys As New HashSet(Of String)(StringComparer.OrdinalIgnoreCase)
        Public ReadOnly Property DisplayNameByKey As New Dictionary(Of String, String)(StringComparer.OrdinalIgnoreCase)
        Public ReadOnly Property AliasNumberByKey As New Dictionary(Of String, Integer)(StringComparer.OrdinalIgnoreCase)
        Public ReadOnly Property SyntheticKeysInOrder As New List(Of String)
        Public ReadOnly Property CorrelationRootByKey As New Dictionary(Of String, Object)(StringComparer.OrdinalIgnoreCase)
        Public ReadOnly Property VariableKeyByCorrelationRoot As New Dictionary(Of Object, Dictionary(Of FBM.ModelObject, String))
        Public ReadOnly Property VariableKeysByCorrelationRootInOrder As New Dictionary(Of Object, List(Of String))
        Public ReadOnly Property JoinedToKeyByKey As New Dictionary(Of String, String)(StringComparer.OrdinalIgnoreCase)
    End Class

    Private Class FactOccurrence
        Public Property FactType As FBM.FactType
        Public Property Reading As FBM.FactTypeReading
        Public Property PathedRoles As New List(Of FBM.PathedRole)
        Public Property Roles As New List(Of FBM.Role)
        Public Property PathBindingKeys As New List(Of String)
        Public Property BindingKeys As New List(Of String)
        Public Property EntryBindingKey As String = ""
        Public Property ExitBindingKey As String = ""
        Public Property ErrorText As String = ""
        Public Property IsNegated As Boolean
        Public Property RestrictsPreviousFactType As Boolean
        Public Property TrailingTransitionPlan As RoleStringPlan
    End Class

    Private Class RoleStringPlan
        Public Property Root As FBM.RootObjectType
        Public Property Occurrences As New List(Of FactOccurrence)
        Public Property StartsRolePath As Boolean
        Public Property ContextTransitionPlan As RoleStringPlan
        Public Property SkipParentContextBinding As Boolean
        Public Property BranchJoinOperator As String = ""
        Public Property ChildSplitOperator As String = "and"
        Public Property ChildPlans As New List(Of RoleStringPlan)
        Public Property NegatesEntireBranch As Boolean
        Public Property NegatedRootExists As Boolean
    End Class

    Private Class RolePathPlan
        Public Property RolePath As FBM.RolePath
        Public Property LeadTransitionPlan As RoleStringPlan
        Public Property MainPlan As RoleStringPlan
        Public Property TransitionPlan As RoleStringPlan
        Public Property ContextBindingKey As String = ""
        Public Property ContextObject As FBM.ModelObject
        Public Property SplitOperator As String = "and"
        Public Property SubPlans As New List(Of RoleStringPlan)
    End Class

    Public Function RenderDerivationEnglish(ByVal arModel As FBM.Model,
                                            ByVal arDerivedFactType As FBM.FactType) As String

        If arModel Is Nothing Then Return "[ERROR: No Boston model was supplied.]"
        If arDerivedFactType Is Nothing Then Return "[ERROR: No derived Fact Type was supplied.]"

        If arDerivedFactType.DerivationRule Is Nothing OrElse
           arDerivedFactType.DerivationRule.FactTypeDerivationPath Is Nothing OrElse
           arDerivedFactType.DerivationRule.FactTypeDerivationPath.PathComponents Is Nothing OrElse
           arDerivedFactType.DerivationRule.FactTypeDerivationPath.PathComponents.RolePaths Is Nothing OrElse
           Not arDerivedFactType.DerivationRule.FactTypeDerivationPath.PathComponents.RolePaths.
               Any(Function(x) x IsNot Nothing) Then
            Return RenderUndevelopedFactType(arDerivedFactType)
        End If

        Try
            Dim lrContext As RenderContext = BuildContext(arModel, arDerivedFactType)
            Dim larPlans As List(Of RolePathPlan) = BuildRolePathPlans(lrContext)

            EstablishHeadBindings(lrContext)
            PrepareDisplayNames(lrContext)

            Dim lsHead As String = RenderHead(lrContext)
            Dim lsConditions As String = RenderConditions(lrContext)
            Dim lsBody As String = RenderBody(lrContext, larPlans, lsConditions)

            Dim lrResult As New StringBuilder
            lrResult.Append("*")
            lrResult.Append(lsHead)

            If lsBody <> "" OrElse lsConditions <> "" Then
                lrResult.Append(" if and only if")
                lrResult.Append(HtmlBreak)

                If lsBody <> "" Then lrResult.Append(lsBody)
                If lsConditions <> "" Then
                    If lsBody <> "" Then lrResult.Append(HtmlBreak)
                    lrResult.Append("where ")
                    lrResult.Append(lsConditions)
                End If
            End If

            Dim lsResult As String = lrResult.ToString().Trim()
            If Not lsResult.EndsWith(".", StringComparison.Ordinal) Then lsResult &= "."
            Return lsResult

        Catch ex As Exception
            Return "*[ERROR: Derivation verbalisation failed for '" &
                   SafeFactTypeName(arDerivedFactType) & "': " & CleanError(ex.Message) & "]."
        End Try

    End Function

    Private Function BuildContext(ByVal arModel As FBM.Model,
                                  ByVal arDerivedFactType As FBM.FactType) As RenderContext

        Dim lrPath As FBM.FactTypeDerivationPath =
            arDerivedFactType.DerivationRule.FactTypeDerivationPath

        Dim larRolePaths As List(Of FBM.RolePath) =
            lrPath.PathComponents.RolePaths.
                Where(Function(x) x IsNot Nothing).
                ToList()

        Dim lrContext As New RenderContext With {
            .Model = arModel,
            .DerivedFactType = arDerivedFactType,
            .DerivationPath = lrPath,
            .RolePath = larRolePaths(0)
        }

        lrContext.RolePaths.AddRange(larRolePaths)

        For Each lrRolePath As FBM.RolePath In lrContext.RolePaths
            RegisterRoot(lrContext, lrRolePath.RootObjectType)
            RegisterPathedRoles(lrContext, lrRolePath.PathedRole)

            If lrRolePath.SubPath IsNot Nothing Then
                For Each lrSubPath As FBM.RoleSubPath In lrRolePath.SubPath
                    RegisterSubPath(lrContext, lrSubPath)
                Next
            End If
        Next

        For Each lrRolePath As FBM.RolePath In lrContext.RolePaths
            RegisterCalculatedValues(lrContext, lrRolePath)
            ApplyObjectUnifiers(lrContext, lrRolePath)
        Next
        RegisterProjections(lrContext)
        Return lrContext
    End Function

    Private Sub RegisterSubPath(ByVal arContext As RenderContext,
                                ByVal arSubPath As FBM.RoleSubPath)
        If arSubPath Is Nothing Then Exit Sub

        RegisterRoot(arContext, arSubPath.RootObjectType)
        RegisterPathedRoles(arContext, arSubPath.PathedRole)

        If arSubPath.SubPath IsNot Nothing Then
            For Each lrChildSubPath As FBM.RoleSubPath In arSubPath.SubPath
                RegisterSubPath(arContext, lrChildSubPath)
            Next
        End If
    End Sub

    Private Sub RegisterRoot(ByVal arContext As RenderContext,
                             ByVal arRoot As FBM.RootObjectType)
        If arRoot Is Nothing OrElse String.IsNullOrWhiteSpace(arRoot.id) Then Exit Sub
        arContext.RootById(arRoot.id) = arRoot
        EnsureKey(arContext, RootKey(arRoot.id))
    End Sub

    Private Sub RegisterPathedRoles(ByVal arContext As RenderContext,
                                    ByVal aarPathedRoles As List(Of FBM.PathedRole))
        If aarPathedRoles Is Nothing Then Exit Sub

        For Each lrPathedRole As FBM.PathedRole In aarPathedRoles
            If lrPathedRole Is Nothing OrElse String.IsNullOrWhiteSpace(lrPathedRole.id) Then Continue For

            arContext.PathedRoleById(lrPathedRole.id) = lrPathedRole
            EnsureKey(arContext, PathedKey(lrPathedRole.id))

            Dim lrRole As FBM.Role = ResolveRoleById(arContext.Model, lrPathedRole.Ref)
            If lrRole IsNot Nothing Then arContext.RoleByPathedRoleId(lrPathedRole.id) = lrRole
        Next
    End Sub

    Private Sub RegisterProjections(ByVal arContext As RenderContext)
        If arContext.DerivationPath.Projection Is Nothing Then Exit Sub

        For Each lrProjection As FBM.DerivationProjection In arContext.DerivationPath.Projection
            If lrProjection Is Nothing OrElse lrProjection.RoleProjection Is Nothing Then Continue For

            For Each lrRoleProjection As FBM.RoleProjection In lrProjection.RoleProjection
                If lrRoleProjection Is Nothing OrElse
                   String.IsNullOrWhiteSpace(lrRoleProjection.Ref) OrElse
                   lrRoleProjection.DerivationSource Is Nothing Then Continue For

                Dim lsHeadKey As String = HeadRoleKey(lrRoleProjection.Ref)
                EnsureKey(arContext, lsHeadKey)

                Dim lsSourceKey As String =
                    SourceBindingKey(lrRoleProjection.DerivationSource)
                If lsSourceKey <> "" Then
                    Dim lrDerivedRole As FBM.Role =
                        ResolveRoleInFactType(arContext.DerivedFactType,
                                              lrRoleProjection.Ref)
                    Dim lrSourceObject As FBM.ModelObject =
                        SourceBindingObject(arContext,
                                            lrRoleProjection.DerivationSource)

                    If lrDerivedRole IsNot Nothing AndAlso
                       lrDerivedRole.JoinedORMObject IsNot Nothing AndAlso
                       lrSourceObject IsNot Nothing Then

                        ' NORMA registers a projection as two typed variables at
                        ' one correlation root. Equal actual Object Types collapse;
                        ' compatible but different types remain partnered so the
                        ' verbalizer can say, for example:
                        '   some ObjectType 2 that is that ValueType 2
                        Dim loCorrelationRoot As Object =
                            CorrelationRootForKey(arContext,
                                                  lsSourceKey)
                        If loCorrelationRoot Is Nothing Then
                            loCorrelationRoot =
                                SourceCorrelationRoot(
                                    arContext,
                                    lrRoleProjection.DerivationSource)
                        End If

                        If SameModelObject(
                            lrDerivedRole.JoinedORMObject,
                            lrSourceObject) Then

                            ' The projected Role and source are two uses of the
                            ' same Boston model element. They are one variable,
                            ' even when an imported PathRoot holds a different
                            ' CLR instance for that model element.
                            UnionKeys(arContext,
                                      lsHeadKey,
                                      lsSourceKey)

                            If loCorrelationRoot IsNot Nothing Then
                                arContext.CorrelationRootByKey(lsSourceKey) =
                                    loCorrelationRoot
                                RegisterVariableAtCorrelationRoot(
                                    arContext,
                                    loCorrelationRoot,
                                    lsSourceKey,
                                    lrSourceObject)
                            End If
                        ElseIf loCorrelationRoot IsNot Nothing Then
                            arContext.CorrelationRootByKey(lsSourceKey) =
                                loCorrelationRoot
                            RegisterVariableAtCorrelationRoot(
                                arContext,
                                loCorrelationRoot,
                                lsSourceKey,
                                lrSourceObject)
                            RegisterVariableAtCorrelationRoot(
                                arContext,
                                loCorrelationRoot,
                                lsHeadKey,
                                lrDerivedRole.JoinedORMObject)
                        End If
                    End If
                End If

                arContext.HeadSourceByRoleId(lrRoleProjection.Ref) = lrRoleProjection.DerivationSource
                arContext.AllHeadSources.Add(lrRoleProjection.DerivationSource)
            Next
        Next
    End Sub

    Private Sub RegisterCalculatedValues(ByVal arContext As RenderContext,
                                         ByVal arRolePath As FBM.RolePath)
        If arRolePath Is Nothing OrElse arRolePath.CalculatedValues Is Nothing Then Exit Sub

        For Each lrValue As FBM.CalculatedValue In arRolePath.CalculatedValues
            If lrValue IsNot Nothing AndAlso Not String.IsNullOrWhiteSpace(lrValue.Id) Then
                arContext.CalculatedValueById(lrValue.Id) = lrValue
            End If
        Next
    End Sub

    Private Sub ApplyObjectUnifiers(ByVal arContext As RenderContext,
                                    ByVal arRolePath As FBM.RolePath)
        If arRolePath Is Nothing OrElse arRolePath.ObjectUnifier Is Nothing Then Exit Sub

        For Each lrUnifier As FBM.ObjectUnifier In arRolePath.ObjectUnifier
            If lrUnifier Is Nothing Then Continue For

            Dim larKeys As New List(Of String)
            Dim larObjects As New List(Of FBM.ModelObject)

            If lrUnifier.PathRoots IsNot Nothing Then
                For Each lrRootReference As FBM.PathRootReference In lrUnifier.PathRoots
                    If lrRootReference Is Nothing OrElse String.IsNullOrWhiteSpace(lrRootReference.Ref) Then Continue For

                    Dim lrRoot As FBM.RootObjectType = Nothing
                    If arContext.RootById.TryGetValue(lrRootReference.Ref,
                                                      lrRoot) AndAlso
                       lrRoot IsNot Nothing AndAlso
                       lrRoot.BostonModelElement IsNot Nothing Then
                        larKeys.Add(RootKey(lrRootReference.Ref))
                        larObjects.Add(lrRoot.BostonModelElement)
                    End If
                Next
            End If

            If lrUnifier.PathedRoles IsNot Nothing Then
                For Each lrRoleReference As FBM.PathedRoleReference In lrUnifier.PathedRoles
                    If lrRoleReference Is Nothing OrElse String.IsNullOrWhiteSpace(lrRoleReference.Ref) Then Continue For

                    Dim lrPathedRole As FBM.PathedRole = Nothing
                    If arContext.PathedRoleById.TryGetValue(lrRoleReference.Ref,
                                                           lrPathedRole) Then
                        Dim lrRole As FBM.Role =
                            ResolvePathedRole(arContext,
                                              lrPathedRole)
                        If lrRole IsNot Nothing AndAlso
                           lrRole.JoinedORMObject IsNot Nothing Then
                            larKeys.Add(PathedKey(lrRoleReference.Ref))
                            larObjects.Add(lrRole.JoinedORMObject)
                        End If
                    End If
                Next
            End If

            Dim loCorrelationRoot As Object = lrUnifier

            ' NORMA registers one variable for each pair of:
            '   normalized correlation root + actual role-player Object Type.
            ' Members of different Object Types remain correlated variables;
            ' they are not collapsed into one display identity.
            For liIndex As Integer = 0 To larKeys.Count - 1
                Dim lsKey As String = larKeys(liIndex)
                Dim lrObject As FBM.ModelObject = larObjects(liIndex)

                EnsureKey(arContext, lsKey)
                arContext.CorrelationRootByKey(lsKey) = loCorrelationRoot
                RegisterVariableAtCorrelationRoot(arContext,
                                                  loCorrelationRoot,
                                                  lsKey,
                                                  lrObject)
            Next
        Next
    End Sub

    Private Sub RegisterVariableAtCorrelationRoot(
        ByVal arContext As RenderContext,
        ByVal aoCorrelationRoot As Object,
        ByVal asBindingKey As String,
        ByVal arObject As FBM.ModelObject)

        If aoCorrelationRoot Is Nothing OrElse
           String.IsNullOrWhiteSpace(asBindingKey) OrElse
           arObject Is Nothing Then Exit Sub

        Dim ldictVariablesByObjectType As Dictionary(Of FBM.ModelObject, String) = Nothing
        Dim lsExistingKey As String = ""

        If Not arContext.VariableKeyByCorrelationRoot.
            TryGetValue(aoCorrelationRoot,
                        ldictVariablesByObjectType) Then
            ldictVariablesByObjectType =
                New Dictionary(Of FBM.ModelObject, String)(
                    ModelObjectReferenceComparer.Instance)
            arContext.VariableKeyByCorrelationRoot(aoCorrelationRoot) =
                ldictVariablesByObjectType
        End If

        If ldictVariablesByObjectType.TryGetValue(arObject,
                                                  lsExistingKey) Then
            UnionKeys(arContext,
                      lsExistingKey,
                      asBindingKey)
        Else
            ldictVariablesByObjectType(arObject) = asBindingKey

            Dim larVariableKeys As List(Of String) = Nothing
            If Not arContext.VariableKeysByCorrelationRootInOrder.
                TryGetValue(aoCorrelationRoot,
                            larVariableKeys) Then
                larVariableKeys = New List(Of String)
                arContext.VariableKeysByCorrelationRootInOrder(
                    aoCorrelationRoot) = larVariableKeys
            End If
            larVariableKeys.Add(asBindingKey)
        End If
    End Sub

    Private Function BuildRolePathPlans(
        ByVal arContext As RenderContext) As List(Of RolePathPlan)

        Dim larRolePathPlans As New List(Of RolePathPlan)

        For Each lrRolePath As FBM.RolePath In arContext.RolePaths
            Dim lrRolePathPlan As New RolePathPlan With {
                .RolePath = lrRolePath,
                .SplitOperator =
                    If(String.Equals(lrRolePath.SplitCombinationOperator,
                                     "Or",
                                     StringComparison.OrdinalIgnoreCase),
                       "or",
                       "and")
            }

            If lrRolePath.PathedRole IsNot Nothing AndAlso
               lrRolePath.PathedRole.Count > 0 Then
                lrRolePathPlan.MainPlan =
                    BuildRoleStringPlan(arContext,
                                        lrRolePath.RootObjectType,
                                        lrRolePath.PathedRole)
                lrRolePathPlan.MainPlan.StartsRolePath = True
                lrRolePathPlan.ContextBindingKey =
                    PlanExitBindingKey(lrRolePathPlan.MainPlan)
                lrRolePathPlan.ContextObject =
                    PathedRolePlayer(arContext,
                                     lrRolePath.PathedRole.LastOrDefault())
            ElseIf lrRolePath.RootObjectType IsNot Nothing Then
                lrRolePathPlan.ContextBindingKey =
                    RootKey(lrRolePath.RootObjectType.id)
                lrRolePathPlan.ContextObject =
                    lrRolePath.RootObjectType.BostonModelElement
            End If

            lrRolePathPlan.LeadTransitionPlan =
                BuildProjectedRootTransitionPlan(arContext, lrRolePathPlan)

            If lrRolePath.SubPath IsNot Nothing Then
                For Each lrSubPath As FBM.RoleSubPath In lrRolePath.SubPath
                    AddSubPathPlans(arContext,
                                    lrRolePathPlan,
                                    lrSubPath)
                Next
            End If

            BindSubPlansToContext(arContext, lrRolePathPlan)
            larRolePathPlans.Add(lrRolePathPlan)
        Next

        Return larRolePathPlans
    End Function

    Private Sub AddSubPathPlans(ByVal arContext As RenderContext,
                                ByVal arRolePathPlan As RolePathPlan,
                                ByVal arSubPath As FBM.RoleSubPath)
        If arSubPath Is Nothing Then Exit Sub

        If arSubPath.PathedRole IsNot Nothing AndAlso
           arSubPath.PathedRole.Count > 0 Then
            arRolePathPlan.SubPlans.Add(
                BuildSubPathRolePlan(arContext,
                                     arSubPath,
                                     True))
            Exit Sub
        End If

        If arSubPath.SubPath Is Nothing OrElse
           arSubPath.SubPath.Count = 0 Then Exit Sub

        ' An empty SubPath with children is a logical branch group. Bind its
        ' children together before adding them to the surrounding RolePath so
        ' the shared type transition is rendered only on the first child.
        Dim larGroupPlans As New List(Of RoleStringPlan)

        For Each lrChildSubPath As FBM.RoleSubPath In arSubPath.SubPath
            CollectSubPathRolePlans(arContext,
                                    lrChildSubPath,
                                    larGroupPlans)
        Next

        If larGroupPlans.Count = 0 Then Exit Sub

        Dim lsGroupOperator As String =
            If(String.Equals(arSubPath.SplitCombinationOperator,
                             "Or",
                             StringComparison.OrdinalIgnoreCase),
               "or",
               "and")

        For liIndex As Integer = 0 To larGroupPlans.Count - 1
            larGroupPlans(liIndex).SkipParentContextBinding = True
            If liIndex > 0 Then
                larGroupPlans(liIndex).BranchJoinOperator = lsGroupOperator
            End If
        Next

        BindNestedSubPlansToContext(arContext,
                                    larGroupPlans,
                                    arRolePathPlan.ContextBindingKey,
                                    arRolePathPlan.ContextObject)
        arRolePathPlan.SubPlans.AddRange(larGroupPlans)
    End Sub

    Private Function BuildSubPathRolePlan(
        ByVal arContext As RenderContext,
        ByVal arSubPath As FBM.RoleSubPath,
        ByVal abTopLevelSubPath As Boolean) As RoleStringPlan

        Dim lrPlan As RoleStringPlan =
            BuildRoleStringPlan(arContext,
                                arSubPath.RootObjectType,
                                arSubPath.PathedRole)

        lrPlan.ChildSplitOperator =
            If(String.Equals(arSubPath.SplitCombinationOperator,
                             "Or",
                             StringComparison.OrdinalIgnoreCase),
               "or",
               "and")

        lrPlan.NegatedRootExists =
            arSubPath.RootObjectType IsNot Nothing AndAlso
            arSubPath.RootObjectType.IsNegated

        ' At the outer SubPath level NORMA scopes a negated lead PathedRole
        ' around the complete branch, including its child SubPaths. Nested
        ' negated roles remain occurrence-level restrictions (for example,
        ' "belongs to no FactType").
        lrPlan.NegatesEntireBranch =
            abTopLevelSubPath AndAlso
            Not lrPlan.NegatedRootExists AndAlso
            arSubPath.PathedRole IsNot Nothing AndAlso
            arSubPath.PathedRole.Count > 0 AndAlso
            arSubPath.PathedRole(0) IsNot Nothing AndAlso
            arSubPath.PathedRole(0).IsNegated

        If lrPlan.NegatesEntireBranch AndAlso
           lrPlan.Occurrences IsNot Nothing AndAlso
           lrPlan.Occurrences.Count > 0 Then
            lrPlan.Occurrences(0).IsNegated = False
        End If

        If lrPlan.NegatedRootExists Then
            ' A negated RootObjectType introduces "no X exists"; it is not an
            ' implicit transition from the surrounding RolePath context.
            lrPlan.SkipParentContextBinding = True
        End If

        If arSubPath.SubPath IsNot Nothing AndAlso
           arSubPath.SubPath.Count > 0 Then

            For Each lrChildSubPath As FBM.RoleSubPath In arSubPath.SubPath
                If lrChildSubPath Is Nothing Then Continue For

                If lrChildSubPath.PathedRole IsNot Nothing AndAlso
                   lrChildSubPath.PathedRole.Count > 0 Then
                    lrPlan.ChildPlans.Add(
                        BuildSubPathRolePlan(arContext,
                                             lrChildSubPath,
                                             False))
                Else
                    CollectSubPathRolePlans(arContext,
                                            lrChildSubPath,
                                            lrPlan.ChildPlans)
                End If
            Next

            For liIndex As Integer = 1 To lrPlan.ChildPlans.Count - 1
                If String.IsNullOrWhiteSpace(
                    lrPlan.ChildPlans(liIndex).BranchJoinOperator) Then
                    lrPlan.ChildPlans(liIndex).BranchJoinOperator =
                        lrPlan.ChildSplitOperator
                End If
            Next

            BindNestedSubPlansToContext(
                arContext,
                lrPlan.ChildPlans,
                PlanExitBindingKey(lrPlan),
                PathedRolePlayer(arContext,
                                 arSubPath.PathedRole.LastOrDefault()))
        End If

        Return lrPlan
    End Function

    Private Sub CollectSubPathRolePlans(
        ByVal arContext As RenderContext,
        ByVal arSubPath As FBM.RoleSubPath,
        ByVal aarPlans As List(Of RoleStringPlan))

        If arSubPath Is Nothing Then Exit Sub

        If arSubPath.PathedRole IsNot Nothing AndAlso
           arSubPath.PathedRole.Count > 0 Then
            aarPlans.Add(
                BuildRoleStringPlan(arContext,
                                    arSubPath.RootObjectType,
                                    arSubPath.PathedRole))
            Exit Sub
        End If

        If arSubPath.SubPath IsNot Nothing Then
            For Each lrChildSubPath As FBM.RoleSubPath In arSubPath.SubPath
                CollectSubPathRolePlans(arContext,
                                        lrChildSubPath,
                                        aarPlans)
            Next
        End If
    End Sub

    Private Sub BindNestedSubPlansToContext(
        ByVal arContext As RenderContext,
        ByVal aarPlans As List(Of RoleStringPlan),
        ByVal asContextBindingKey As String,
        ByVal arContextObject As FBM.ModelObject)

        If aarPlans Is Nothing OrElse aarPlans.Count = 0 OrElse
           String.IsNullOrWhiteSpace(asContextBindingKey) OrElse
           arContextObject Is Nothing Then Exit Sub

        Dim larEntryKeys As New List(Of String)
        Dim larEntryObjects As New List(Of FBM.ModelObject)
        Dim larEntryPlans As New List(Of RoleStringPlan)
        Dim lrCommonEntryObject As FBM.ModelObject = Nothing
        Dim lbCommonEntryObject As Boolean = True

        For Each lrPlan As RoleStringPlan In aarPlans
            Dim lsEntryKey As String = PlanEntryBindingKey(lrPlan)
            Dim lrEntryObject As FBM.ModelObject = PlanEntryObject(lrPlan)
            If lsEntryKey = "" OrElse lrEntryObject Is Nothing Then Continue For

            If lrCommonEntryObject Is Nothing Then
                lrCommonEntryObject = lrEntryObject
            ElseIf Not SameModelObject(lrCommonEntryObject, lrEntryObject) Then
                lbCommonEntryObject = False
            End If

            larEntryKeys.Add(lsEntryKey)
            larEntryObjects.Add(lrEntryObject)
            larEntryPlans.Add(lrPlan)
        Next

        If lrCommonEntryObject Is Nothing OrElse larEntryKeys.Count = 0 Then
            Exit Sub
        End If

        If Not lbCommonEntryObject Then
            For liIndex As Integer = 0 To larEntryPlans.Count - 1
                If SameModelObject(arContextObject,
                                   larEntryObjects(liIndex)) Then
                    UnionKeys(arContext,
                              asContextBindingKey,
                              larEntryKeys(liIndex))
                Else
                    larEntryPlans(liIndex).ContextTransitionPlan =
                        BuildTransitionPlan(arContext,
                                            asContextBindingKey,
                                            arContextObject,
                                            larEntryKeys(liIndex),
                                            larEntryObjects(liIndex))
                End If
            Next
            Exit Sub
        End If

        Dim lsCommonEntryKey As String = larEntryKeys(0)
        For liIndex As Integer = 1 To larEntryKeys.Count - 1
            UnionKeys(arContext,
                      lsCommonEntryKey,
                      larEntryKeys(liIndex))
        Next

        If SameModelObject(arContextObject,
                           lrCommonEntryObject) Then
            UnionKeys(arContext,
                      asContextBindingKey,
                      lsCommonEntryKey)
        Else
            larEntryPlans(0).ContextTransitionPlan =
                BuildTransitionPlan(arContext,
                                    asContextBindingKey,
                                    arContextObject,
                                    lsCommonEntryKey,
                                    lrCommonEntryObject)
        End If
    End Sub

    Private Function BuildProjectedRootTransitionPlan(
        ByVal arContext As RenderContext,
        ByVal arRolePathPlan As RolePathPlan) As RoleStringPlan

        If arRolePathPlan Is Nothing OrElse
           arRolePathPlan.RolePath Is Nothing OrElse
           arRolePathPlan.RolePath.RootObjectType Is Nothing OrElse
           arRolePathPlan.MainPlan Is Nothing OrElse
           arContext.DerivationPath.Projection Is Nothing Then Return Nothing

        Dim lrRolePath As FBM.RolePath = arRolePathPlan.RolePath
        Dim lrRoot As FBM.RootObjectType = lrRolePath.RootObjectType
        If String.IsNullOrWhiteSpace(lrRolePath.Id) OrElse
           String.IsNullOrWhiteSpace(lrRoot.id) OrElse
           lrRoot.BostonModelElement Is Nothing Then Return Nothing

        Dim larRootProjections As List(Of FBM.RoleProjection) =
            arContext.DerivationPath.Projection.
                Where(
                    Function(x) x IsNot Nothing AndAlso
                                String.Equals(x.Ref,
                                              lrRolePath.Id,
                                              StringComparison.OrdinalIgnoreCase) AndAlso
                                x.RoleProjection IsNot Nothing).
                SelectMany(Function(x) x.RoleProjection).
                Where(
                    Function(x) x IsNot Nothing AndAlso
                                Not String.IsNullOrWhiteSpace(x.Ref) AndAlso
                                x.DerivationSource IsNot Nothing AndAlso
                                x.DerivationSource.PathRoot IsNot Nothing AndAlso
                                String.Equals(x.DerivationSource.PathRoot.Ref,
                                              lrRoot.id,
                                              StringComparison.OrdinalIgnoreCase)).
                ToList()

        ' A single projected root gives one unambiguous head-to-root type
        ' transition. Ring projections and other multi-projection cases retain
        ' the established renderer path until their NORMA structure is handled
        ' explicitly.
        If larRootProjections.Count <> 1 Then Return Nothing

        Dim lrRoleProjection As FBM.RoleProjection = larRootProjections(0)
        Dim lrDerivedRole As FBM.Role =
            ResolveRoleInFactType(arContext.DerivedFactType,
                                  lrRoleProjection.Ref)
        If lrDerivedRole Is Nothing OrElse
           lrDerivedRole.JoinedORMObject Is Nothing OrElse
           SameModelObject(lrDerivedRole.JoinedORMObject,
                           lrRoot.BostonModelElement) Then Return Nothing

        ' The projection transition belongs at the lead only when the actual
        ' RolePath starts at the projected root type.
        Dim lrMainEntryObject As FBM.ModelObject =
            PlanEntryObject(arRolePathPlan.MainPlan)
        If Not SameModelObject(lrMainEntryObject,
                               lrRoot.BostonModelElement) Then Return Nothing

        ' Keep this type label separate from the canonical projected-variable
        ' key. The head role and PathRoot denote the same logical value, but
        ' NORMA verbalises both compatible types (for example,
        ' CanonicalDataValue is some DataValue).
        Dim lsTypedRootKey As String =
            ProjectionTypeKey(lrRolePath.Id,
                              lrRoleProjection.Ref)
        Dim lrTransitionPlan As RoleStringPlan =
            BuildTransitionPlan(arContext,
                                HeadRoleKey(lrRoleProjection.Ref),
                                lrDerivedRole.JoinedORMObject,
                                lsTypedRootKey,
                                lrRoot.BostonModelElement)

        ' Do not broaden behavior for unresolved or ambiguous type changes.
        ' Existing rendering remains unchanged unless Boston has one definite
        ' subtype Fact Type Reading for this projection boundary.
        If Not HasSuccessfulTransition(lrTransitionPlan) Then Return Nothing

        EnsureKey(arContext, lsTypedRootKey)
        If Not arContext.DisplayNameByKey.ContainsKey(lsTypedRootKey) Then
            arContext.DisplayNameByKey(lsTypedRootKey) =
                RootObjectTypeName(lrRoot)
            arContext.SyntheticKeysInOrder.Add(lsTypedRootKey)
        End If

        Return lrTransitionPlan
    End Function

    Private Function BuildRoleStringPlan(ByVal arContext As RenderContext,
                                         ByVal arRoot As FBM.RootObjectType,
                                         ByVal aarPathedRoles As List(Of FBM.PathedRole)) As RoleStringPlan

        Dim lrPlan As New RoleStringPlan With {.Root = arRoot}
        Dim lrCurrent As FactOccurrence = Nothing

        For Each lrPathedRole As FBM.PathedRole In aarPathedRoles
            Dim lrRole As FBM.Role = ResolvePathedRole(arContext, lrPathedRole)
            lrRole = ResolveSameFactTypeLinkRole(lrCurrent,
                                                 lrPathedRole,
                                                 lrRole)

            If lrRole Is Nothing OrElse lrRole.FactType Is Nothing Then
                FinaliseOccurrence(arContext, lrPlan, lrCurrent)
                lrCurrent = Nothing

                Dim lsReference As String = If(lrPathedRole Is Nothing, "", lrPathedRole.Ref)
                lrPlan.Occurrences.Add(New FactOccurrence With {
                    .ErrorText = "[ERROR: PathedRole '" & CleanError(lsReference) &
                                 "' does not resolve to a Boston Role with a FactType.]"
                })
                Continue For
            End If

            If lrCurrent Is Nothing OrElse StartsNewFactOccurrence(lrCurrent, lrRole, lrPathedRole) Then
                FinaliseOccurrence(arContext, lrPlan, lrCurrent)
                lrCurrent = New FactOccurrence With {.FactType = lrRole.FactType}
            End If

            lrCurrent.PathedRoles.Add(lrPathedRole)
            lrCurrent.Roles.Add(lrRole)
            Dim lsPathBindingKey As String = PathedKey(lrPathedRole.id)
            lrCurrent.PathBindingKeys.Add(lsPathBindingKey)
            If lrCurrent.EntryBindingKey = "" Then lrCurrent.EntryBindingKey = lsPathBindingKey
            lrCurrent.ExitBindingKey = lsPathBindingKey
            lrCurrent.IsNegated = lrCurrent.IsNegated OrElse lrPathedRole.IsNegated
        Next

        FinaliseOccurrence(arContext, lrPlan, lrCurrent)
        ApplyPathContinuity(arContext, lrPlan)
        Return lrPlan
    End Function

    Private Sub BindSubPlansToContext(ByVal arContext As RenderContext,
                                      ByVal arRolePathPlan As RolePathPlan)
        If arRolePathPlan Is Nothing OrElse
           arRolePathPlan.SubPlans Is Nothing OrElse
           arRolePathPlan.SubPlans.Count = 0 OrElse
           String.IsNullOrWhiteSpace(arRolePathPlan.ContextBindingKey) OrElse
           arRolePathPlan.ContextObject Is Nothing Then Exit Sub

        Dim larEntryKeys As New List(Of String)
        Dim larEntryObjects As New List(Of FBM.ModelObject)
        Dim larEntryPlans As New List(Of RoleStringPlan)
        Dim lrCommonEntryObject As FBM.ModelObject = Nothing
        Dim lbCommonEntryObject As Boolean = True

        For Each lrSubPlan As RoleStringPlan In arRolePathPlan.SubPlans
            If lrSubPlan.SkipParentContextBinding Then Continue For

            Dim lsEntryKey As String = PlanEntryBindingKey(lrSubPlan)
            Dim lrEntryObject As FBM.ModelObject = PlanEntryObject(lrSubPlan)
            If lsEntryKey = "" OrElse lrEntryObject Is Nothing Then Continue For

            If lrCommonEntryObject Is Nothing Then
                lrCommonEntryObject = lrEntryObject
            ElseIf Not SameModelObject(lrCommonEntryObject, lrEntryObject) Then
                lbCommonEntryObject = False
            End If
            larEntryKeys.Add(lsEntryKey)
            larEntryObjects.Add(lrEntryObject)
            larEntryPlans.Add(lrSubPlan)
        Next

        If lrCommonEntryObject Is Nothing OrElse larEntryKeys.Count = 0 Then Exit Sub

        ' A scoped top-level branch owns its transition from the surrounding
        ' RolePath context. Hoisting one shared transition ahead of separately
        ' negated sibling branches changes the logical scope and produces
        ' fragments such as "that is some ObjectType that it is not true...".
        If larEntryPlans.Any(
            Function(x) x IsNot Nothing AndAlso
                        (x.NegatesEntireBranch OrElse
                         x.NegatedRootExists)) Then

            For liIndex As Integer = 0 To larEntryPlans.Count - 1
                If SameModelObject(arRolePathPlan.ContextObject,
                                   larEntryObjects(liIndex)) Then
                    UnionKeys(arContext,
                              arRolePathPlan.ContextBindingKey,
                              larEntryKeys(liIndex))
                Else
                    larEntryPlans(liIndex).ContextTransitionPlan =
                        BuildTransitionPlan(arContext,
                                            arRolePathPlan.ContextBindingKey,
                                            arRolePathPlan.ContextObject,
                                            larEntryKeys(liIndex),
                                            larEntryObjects(liIndex))
                End If
            Next
            Exit Sub
        End If

        If Not lbCommonEntryObject Then
            ' Alternative SubPaths can leave the same context through different
            ' subtype Fact Types. Each branch then needs its own transition:
            ' for example Constraint is ValueConstraint, Constraint is
            ' CardinalityConstraint, or Constraint is GeneralConstraint.
            For liIndex As Integer = 0 To larEntryPlans.Count - 1
                If SameModelObject(arRolePathPlan.ContextObject,
                                   larEntryObjects(liIndex)) Then
                    UnionKeys(arContext,
                              arRolePathPlan.ContextBindingKey,
                              larEntryKeys(liIndex))
                Else
                    larEntryPlans(liIndex).ContextTransitionPlan =
                        BuildTransitionPlan(arContext,
                                            arRolePathPlan.ContextBindingKey,
                                            arRolePathPlan.ContextObject,
                                            larEntryKeys(liIndex),
                                            larEntryObjects(liIndex))
                End If
            Next
            Exit Sub
        End If

        'The sibling SubPaths are alternative predicates for the same path variable.
        'Correlate their entry roles before aliases and quantifiers are prepared.
        Dim lsCommonEntryKey As String = larEntryKeys(0)
        For liIndex As Integer = 1 To larEntryKeys.Count - 1
            UnionKeys(arContext, lsCommonEntryKey, larEntryKeys(liIndex))
        Next

        If SameModelObject(arRolePathPlan.ContextObject, lrCommonEntryObject) Then
            UnionKeys(arContext,
                      arRolePathPlan.ContextBindingKey,
                      lsCommonEntryKey)
            Exit Sub
        End If

        'NORMA permits a RolePath to move from the current object to a compatible
        'SubPath object without listing the connecting Fact Type's roles as
        'PathedRoles. Resolve that boundary through Boston's ordinary Fact Types;
        'the predicate wording still comes exclusively from the selected FTR.
        arRolePathPlan.TransitionPlan =
            BuildTransitionPlan(arContext,
                                arRolePathPlan.ContextBindingKey,
                                arRolePathPlan.ContextObject,
                                lsCommonEntryKey,
                                lrCommonEntryObject)
    End Sub

    Private Function BuildTransitionPlan(
        ByVal arContext As RenderContext,
        ByVal asFromBindingKey As String,
        ByVal arFromObject As FBM.ModelObject,
        ByVal asToBindingKey As String,
        ByVal arToObject As FBM.ModelObject) As RoleStringPlan

        Dim lrPlan As New RoleStringPlan
        Dim lrSelectedFactType As FBM.FactType = Nothing
        Dim lrSelectedReading As FBM.FactTypeReading = Nothing
        Dim larSelectedRoles As List(Of FBM.Role) = Nothing
        Dim liMatchingReadings As Integer = 0

        'The adjacent RolePath roles identify the two role players. A change of
        'compatible type at this boundary is represented by their actual subtype
        'relationship Fact Type. Do not search unrelated model Fact Types merely
        'because they happen to contain the same two role players.
        Dim larRelationships As New List(Of FBM.SubtypeRelationship)

        If arToObject.SubtypeRelationship IsNot Nothing Then
            larRelationships.AddRange(
                arToObject.SubtypeRelationship.Where(
                    Function(x) x IsNot Nothing AndAlso
                                SameModelObject(x.ModelElement,
                                                arToObject) AndAlso
                                SameModelObject(x.parentModelElement,
                                                arFromObject)))
        End If

        If arFromObject.SubtypeRelationship IsNot Nothing Then
            larRelationships.AddRange(
                arFromObject.SubtypeRelationship.Where(
                    Function(x) x IsNot Nothing AndAlso
                                SameModelObject(x.ModelElement,
                                                arFromObject) AndAlso
                                SameModelObject(x.parentModelElement,
                                                arToObject)))
        End If

        Dim larFactTypes As List(Of FBM.FactType) =
            larRelationships.
                Where(Function(x) x.FactType IsNot Nothing).
                Select(Function(x) x.FactType).
                GroupBy(Function(x) If(x.Id, ""),
                        StringComparer.OrdinalIgnoreCase).
                Select(Function(x) x.First()).
                ToList()

        If larFactTypes IsNot Nothing Then
            For Each lrFactType As FBM.FactType In larFactTypes
                If lrFactType Is Nothing OrElse
                   lrFactType.RoleGroup Is Nothing Then Continue For

                Dim lrFromRole As FBM.Role =
                    lrFactType.RoleGroup.FirstOrDefault(
                        Function(x) x IsNot Nothing AndAlso
                                    SameModelObject(x.JoinedORMObject,
                                                    arFromObject))
                Dim lrToRole As FBM.Role =
                    lrFactType.RoleGroup.FirstOrDefault(
                        Function(x) x IsNot Nothing AndAlso
                                    SameModelObject(x.JoinedORMObject,
                                                    arToObject))
                If lrFromRole Is Nothing OrElse lrToRole Is Nothing Then
                    Continue For
                End If

                Dim larRoles As New List(Of FBM.Role) From {
                    lrFromRole,
                    lrToRole
                }
                Dim lrReading As FBM.FactTypeReading =
                    lrFactType.getFactTypeReadingByRoleSequence(larRoles)
                If lrReading Is Nothing Then Continue For

                liMatchingReadings += 1
                If liMatchingReadings = 1 Then
                    lrSelectedFactType = lrFactType
                    lrSelectedReading = lrReading
                    larSelectedRoles = larRoles
                End If
            Next
        End If

        If liMatchingReadings <> 1 Then
            Dim lsProblem As String =
                If(liMatchingReadings = 0,
                   "No RolePath-compatible transition Fact Type Reading",
                   "More than one RolePath-compatible transition Fact Type Reading")
            lrPlan.Occurrences.Add(
                New FactOccurrence With {
                    .ErrorText =
                        "[ERROR: " & lsProblem & " from '" &
                        CleanError(arFromObject.Id) & "' to '" &
                        CleanError(arToObject.Id) & "'.]"
                })
            Return lrPlan
        End If

        RegisterCompatibleVariable(arContext,
                                   asFromBindingKey,
                                   asToBindingKey,
                                   arToObject)

        Dim lrOccurrence As New FactOccurrence With {
            .FactType = lrSelectedFactType,
            .Reading = lrSelectedReading,
            .EntryBindingKey = asFromBindingKey,
            .ExitBindingKey = asToBindingKey
        }
        lrOccurrence.Roles.AddRange(larSelectedRoles)
        lrOccurrence.BindingKeys.Add(asFromBindingKey)
        lrOccurrence.BindingKeys.Add(asToBindingKey)
        lrPlan.Occurrences.Add(lrOccurrence)

        Return lrPlan
    End Function

    Private Sub RegisterCompatibleVariable(
        ByVal arContext As RenderContext,
        ByVal asJoinedToBindingKey As String,
        ByVal asVariableBindingKey As String,
        ByVal arVariableObject As FBM.ModelObject)

        If String.IsNullOrWhiteSpace(asJoinedToBindingKey) OrElse
           String.IsNullOrWhiteSpace(asVariableBindingKey) OrElse
           arVariableObject Is Nothing Then Exit Sub

        Dim loCorrelationRoot As Object =
            CorrelationRootForKey(arContext,
                                  asJoinedToBindingKey)

        If loCorrelationRoot Is Nothing Then
            loCorrelationRoot =
                "JOIN:" &
                CanonicalKey(arContext,
                             asJoinedToBindingKey)
        End If

        arContext.CorrelationRootByKey(asVariableBindingKey) =
            loCorrelationRoot
        arContext.JoinedToKeyByKey(asVariableBindingKey) =
            asJoinedToBindingKey
        RegisterVariableAtCorrelationRoot(arContext,
                                          loCorrelationRoot,
                                          asVariableBindingKey,
                                          arVariableObject)
    End Sub

    Private Function CorrelationRootForKey(
        ByVal arContext As RenderContext,
        ByVal asBindingKey As String) As Object

        If String.IsNullOrWhiteSpace(asBindingKey) Then Return Nothing

        Dim loCorrelationRoot As Object = Nothing
        If arContext.CorrelationRootByKey.TryGetValue(
            asBindingKey,
            loCorrelationRoot) Then
            Return loCorrelationRoot
        End If

        Dim lsCanonical As String =
            CanonicalKey(arContext,
                         asBindingKey)

        For Each lrPair As KeyValuePair(Of String, Object) In
            arContext.CorrelationRootByKey

            If String.Equals(
                CanonicalKey(arContext,
                             lrPair.Key),
                lsCanonical,
                StringComparison.OrdinalIgnoreCase) Then
                Return lrPair.Value
            End If
        Next

        Return Nothing
    End Function

    Private Function PlanEntryBindingKey(ByVal arPlan As RoleStringPlan) As String
        If arPlan Is Nothing OrElse arPlan.Occurrences Is Nothing Then Return ""
        Dim lrOccurrence As FactOccurrence =
            arPlan.Occurrences.FirstOrDefault(
                Function(x) x IsNot Nothing AndAlso
                            x.ErrorText = "" AndAlso
                            x.PathBindingKeys IsNot Nothing AndAlso
                            x.PathBindingKeys.Count > 0)
        If lrOccurrence Is Nothing Then Return ""
        Return lrOccurrence.PathBindingKeys(0)
    End Function

    Private Function PlanLogicalEntryBindingKey(
        ByVal arPlan As RoleStringPlan) As String

        Dim lsEntryKey As String =
            PlanEntryBindingKey(arPlan)
        If lsEntryKey <> "" Then Return lsEntryKey

        If arPlan Is Nothing OrElse
           arPlan.Occurrences Is Nothing Then Return ""

        Dim lrOccurrence As FactOccurrence =
            arPlan.Occurrences.FirstOrDefault(
                Function(x) x IsNot Nothing AndAlso
                            Not String.IsNullOrWhiteSpace(
                                x.EntryBindingKey))
        If lrOccurrence Is Nothing Then Return ""
        Return lrOccurrence.EntryBindingKey
    End Function

    Private Function PlanExitBindingKey(ByVal arPlan As RoleStringPlan) As String
        If arPlan Is Nothing OrElse arPlan.Occurrences Is Nothing Then Return ""
        Dim lrOccurrence As FactOccurrence =
            arPlan.Occurrences.LastOrDefault(
                Function(x) x IsNot Nothing AndAlso
                            Not String.IsNullOrWhiteSpace(x.ExitBindingKey))
        If lrOccurrence Is Nothing Then Return ""
        Return lrOccurrence.ExitBindingKey
    End Function

    Private Function PlanEntryObject(ByVal arPlan As RoleStringPlan) As FBM.ModelObject
        If arPlan Is Nothing OrElse arPlan.Occurrences Is Nothing Then Return Nothing
        Dim lrOccurrence As FactOccurrence =
            arPlan.Occurrences.FirstOrDefault(
                Function(x) x IsNot Nothing AndAlso
                            x.Roles IsNot Nothing AndAlso
                            x.Roles.Count > 0)
        If lrOccurrence Is Nothing Then Return Nothing
        Return OccurrencePathEntryObject(lrOccurrence)
    End Function

    Private Function PathedRolePlayer(ByVal arContext As RenderContext,
                                      ByVal arPathedRole As FBM.PathedRole) As FBM.ModelObject
        Dim lrRole As FBM.Role = ResolvePathedRole(arContext, arPathedRole)
        If lrRole Is Nothing Then Return Nothing
        Return lrRole.JoinedORMObject
    End Function

    Private Function SameModelObject(ByVal arLeft As FBM.ModelObject,
                                     ByVal arRight As FBM.ModelObject) As Boolean
        If arLeft Is Nothing OrElse arRight Is Nothing Then Return False
        Return Object.ReferenceEquals(arLeft, arRight) OrElse
               String.Equals(arLeft.Id,
                             arRight.Id,
                             StringComparison.OrdinalIgnoreCase)
    End Function

    Private Function StartsNewFactOccurrence(ByVal arCurrent As FactOccurrence,
                                             ByVal arNextRole As FBM.Role,
                                             ByVal arNextPathedRole As FBM.PathedRole) As Boolean
        If arCurrent Is Nothing OrElse arCurrent.FactType Is Nothing Then Return True
        If Not Object.ReferenceEquals(arCurrent.FactType, arNextRole.FactType) Then Return True

        ' Role.FactType is authoritative. Purpose is used only to distinguish a
        ' repeated occurrence of the same FactType from continuation within it.
        If arCurrent.Roles.Count > 0 AndAlso
           String.Equals(arNextPathedRole.Purpose, "PostInnerJoin", StringComparison.OrdinalIgnoreCase) Then
            Return True
        End If

        If arCurrent.FactType.RoleGroup IsNot Nothing AndAlso
           arCurrent.Roles.Count >= arCurrent.FactType.RoleGroup.Count Then
            Return True
        End If

        Return False
    End Function

    Private Sub FinaliseOccurrence(ByVal arContext As RenderContext,
                                   ByVal arPlan As RoleStringPlan,
                                   ByVal arOccurrence As FactOccurrence)
        If arOccurrence Is Nothing Then Exit Sub

        If arOccurrence.ErrorText = "" Then
            Dim larRoleSequence As New List(Of FBM.Role)(arOccurrence.Roles)
            arOccurrence.Reading =
                arOccurrence.FactType.getFactTypeReadingByRoleSequence(larRoleSequence)

            If arOccurrence.Reading Is Nothing Then
                Dim lsPartialError As String = ""
                arOccurrence.Reading =
                    FindReadingByPartialRoleSequence(arOccurrence.FactType,
                                                     larRoleSequence,
                                                     lsPartialError)
                If arOccurrence.Reading Is Nothing Then
                    arOccurrence.ErrorText =
                        "[ERROR: No Fact Type Reading for '" &
                        SafeFactTypeName(arOccurrence.FactType) &
                        "' with role sequence " & RoleSequenceText(arOccurrence.Roles) &
                        If(lsPartialError = "", "", ". " & lsPartialError) & ".]"
                End If
            End If

            If arOccurrence.Reading IsNot Nothing Then
                AlignOccurrenceBindings(arContext, arOccurrence)
            End If
        End If

        arPlan.Occurrences.Add(arOccurrence)
    End Sub

    Private Function FindReadingByPartialRoleSequence(
        ByVal arFactType As FBM.FactType,
        ByVal aarSuppliedRoles As List(Of FBM.Role),
        ByRef asError As String) As FBM.FactTypeReading

        asError = ""
        If arFactType Is Nothing OrElse aarSuppliedRoles Is Nothing OrElse
           aarSuppliedRoles.Count = 0 OrElse arFactType.FactTypeReading Is Nothing Then
            Return Nothing
        End If

        Dim larCandidates As List(Of FBM.FactTypeReading) =
            arFactType.FactTypeReading.
                Where(Function(x) ReadingContainsOrderedRoleSubset(x, aarSuppliedRoles)).
                ToList()

        If larCandidates.Count = 0 Then
            ' NORMA's final GetMatchingReading fallback uses AllowAnyOrder:
            ' first try to retain the pathed entry role as the lead while allowing
            ' the remaining supplied roles to be reordered into the FTR's order.
            larCandidates =
                arFactType.FactTypeReading.
                    Where(Function(x) ReadingStartsWithEntryAndContainsRoles(
                                         x,
                                         aarSuppliedRoles)).
                    ToList()
        End If

        If larCandidates.Count = 0 Then
            ' A RolePath can traverse a Fact Type in the reverse direction from
            ' its only available reading. NORMA's AllowAnyOrder ultimately
            ' returns an available matching reading even when its lead role is
            ' not the pathed entry role. Keep this as the final, least-specific
            ' tier and still require every supplied Role to belong to the FTR.
            larCandidates =
                arFactType.FactTypeReading.
                    Where(Function(x) ReadingContainsRolesInAnyOrder(
                                         x,
                                         aarSuppliedRoles)).
                    ToList()
        End If

        If larCandidates.Count = 0 Then Return Nothing

        Dim larRoleOrders =
            larCandidates.GroupBy(Function(x) ReadingRoleSequenceKey(x),
                                  StringComparer.OrdinalIgnoreCase).ToList()

        If larRoleOrders.Count = 1 Then
            Return PreferredReadingFromCandidates(larCandidates)
        End If

        Dim larPreferred As List(Of FBM.FactTypeReading) =
            larCandidates.Where(Function(x) x IsNot Nothing AndAlso x.IsPreferred).ToList()
        If larPreferred.Count = 1 Then Return larPreferred(0)

        Dim larPredicatePreferred As List(Of FBM.FactTypeReading) =
            larCandidates.Where(
                Function(x) x IsNot Nothing AndAlso x.IsPreferredForPredicate).ToList()
        If larPredicatePreferred.Count = 1 Then Return larPredicatePreferred(0)

        asError = "The supplied roles match more than one full FTR role order"
        Return Nothing
    End Function

    Private Function ReadingStartsWithEntryAndContainsRoles(
        ByVal arReading As FBM.FactTypeReading,
        ByVal aarSuppliedRoles As List(Of FBM.Role)) As Boolean

        Dim larReadingRoles As List(Of FBM.Role) = ReadingRoles(arReading)
        If larReadingRoles.Count = 0 OrElse aarSuppliedRoles Is Nothing OrElse
           aarSuppliedRoles.Count = 0 OrElse
           larReadingRoles.Count < aarSuppliedRoles.Count Then Return False

        If Not SameRole(larReadingRoles(0), aarSuppliedRoles(0)) Then Return False

        For Each lrSuppliedRole As FBM.Role In aarSuppliedRoles
            If Not larReadingRoles.Any(
                Function(x) SameRole(x, lrSuppliedRole)) Then Return False
        Next

        Return True
    End Function

    Private Function ReadingContainsRolesInAnyOrder(
        ByVal arReading As FBM.FactTypeReading,
        ByVal aarSuppliedRoles As List(Of FBM.Role)) As Boolean

        Dim larReadingRoles As List(Of FBM.Role) = ReadingRoles(arReading)
        If larReadingRoles.Count = 0 OrElse aarSuppliedRoles Is Nothing OrElse
           aarSuppliedRoles.Count = 0 OrElse
           larReadingRoles.Count < aarSuppliedRoles.Count Then Return False

        For Each lrSuppliedRole As FBM.Role In aarSuppliedRoles
            If Not larReadingRoles.Any(
                Function(x) SameRole(x, lrSuppliedRole)) Then Return False
        Next

        Return True
    End Function

    Private Function ReadingContainsOrderedRoleSubset(
        ByVal arReading As FBM.FactTypeReading,
        ByVal aarSuppliedRoles As List(Of FBM.Role)) As Boolean

        Dim larReadingRoles As List(Of FBM.Role) = ReadingRoles(arReading)
        If larReadingRoles.Count <= aarSuppliedRoles.Count Then Return False
        If Not SameRole(larReadingRoles(0), aarSuppliedRoles(0)) Then Return False

        Dim liReadingIndex As Integer = 0
        For Each lrSuppliedRole As FBM.Role In aarSuppliedRoles
            Dim lbFound As Boolean = False
            While liReadingIndex < larReadingRoles.Count
                If SameRole(larReadingRoles(liReadingIndex), lrSuppliedRole) Then
                    lbFound = True
                    liReadingIndex += 1
                    Exit While
                End If
                liReadingIndex += 1
            End While
            If Not lbFound Then Return False
        Next

        Return True
    End Function

    Private Function ReadingRoles(ByVal arReading As FBM.FactTypeReading) As List(Of FBM.Role)
        If arReading Is Nothing Then Return New List(Of FBM.Role)
        Return SafePredicateParts(arReading).
            Where(Function(x) x IsNot Nothing AndAlso x.Role IsNot Nothing).
            Select(Function(x) x.Role).
            ToList()
    End Function

    Private Function ReadingRoleSequenceKey(ByVal arReading As FBM.FactTypeReading) As String
        Return String.Join("|",
                           ReadingRoles(arReading).
                               Select(Function(x) If(x.Id, "")).ToArray())
    End Function

    Private Function PreferredReadingFromCandidates(
        ByVal aarCandidates As List(Of FBM.FactTypeReading)) As FBM.FactTypeReading

        Dim lrReading As FBM.FactTypeReading =
            aarCandidates.FirstOrDefault(Function(x) x IsNot Nothing AndAlso x.IsPreferred)
        If lrReading IsNot Nothing Then Return lrReading

        lrReading =
            aarCandidates.FirstOrDefault(
                Function(x) x IsNot Nothing AndAlso x.IsPreferredForPredicate)
        If lrReading IsNot Nothing Then Return lrReading

        Return aarCandidates.FirstOrDefault(Function(x) x IsNot Nothing)
    End Function

    Private Function SameRole(ByVal arLeft As FBM.Role,
                              ByVal arRight As FBM.Role) As Boolean
        If arLeft Is Nothing OrElse arRight Is Nothing Then Return False
        Return Object.ReferenceEquals(arLeft, arRight) OrElse
               String.Equals(arLeft.Id, arRight.Id, StringComparison.OrdinalIgnoreCase)
    End Function

    Private Sub AlignOccurrenceBindings(ByVal arContext As RenderContext,
                                        ByVal arOccurrence As FactOccurrence)
        Dim ldictAvailableKeys As New Dictionary(Of String, Queue(Of String))(StringComparer.OrdinalIgnoreCase)

        For liIndex As Integer = 0 To arOccurrence.Roles.Count - 1
            Dim lrRole As FBM.Role = arOccurrence.Roles(liIndex)
            If lrRole Is Nothing OrElse String.IsNullOrWhiteSpace(lrRole.Id) OrElse
               liIndex >= arOccurrence.PathBindingKeys.Count Then Continue For

            Dim lqKeys As Queue(Of String) = Nothing
            If Not ldictAvailableKeys.TryGetValue(lrRole.Id, lqKeys) Then
                lqKeys = New Queue(Of String)
                ldictAvailableKeys(lrRole.Id) = lqKeys
            End If
            lqKeys.Enqueue(arOccurrence.PathBindingKeys(liIndex))
        Next

        arOccurrence.BindingKeys = New List(Of String)
        Dim liSlot As Integer = 0

        For Each lrPart As FBM.PredicatePart In SafePredicateParts(arOccurrence.Reading)
            Dim lsBindingKey As String = ""
            Dim lqKeys As Queue(Of String) = Nothing

            If lrPart IsNot Nothing AndAlso lrPart.Role IsNot Nothing AndAlso
               ldictAvailableKeys.TryGetValue(lrPart.Role.Id, lqKeys) AndAlso
               lqKeys.Count > 0 Then
                lsBindingKey = lqKeys.Dequeue()
            Else
                Dim lsOccurrenceId As String =
                    If(arOccurrence.PathedRoles.Count = 0,
                       SafeFactTypeName(arOccurrence.FactType),
                       arOccurrence.PathedRoles(0).id)
                Dim lsRoleId As String =
                    If(lrPart Is Nothing OrElse lrPart.Role Is Nothing,
                       liSlot.ToString(),
                       lrPart.Role.Id)
                lsBindingKey = "SLOT:" & lsOccurrenceId & ":" & lsRoleId & ":" & liSlot.ToString()
                EnsureKey(arContext, lsBindingKey)

                If Not arContext.DisplayNameByKey.ContainsKey(lsBindingKey) Then
                    arContext.DisplayNameByKey(lsBindingKey) =
                        If(lrPart Is Nothing, "", RolePlayerName(lrPart.Role))
                    arContext.SyntheticKeysInOrder.Add(lsBindingKey)
                End If
            End If

            arOccurrence.BindingKeys.Add(lsBindingKey)
            liSlot += 1
        Next
    End Sub

    Private Sub ApplyPathContinuity(ByVal arContext As RenderContext,
                                    ByVal arPlan As RoleStringPlan)
        Dim larUsable As List(Of FactOccurrence) =
            arPlan.Occurrences.Where(Function(x) x IsNot Nothing AndAlso x.BindingKeys.Count > 0).ToList()

        If larUsable.Count = 0 Then Exit Sub

        Dim lrFirstOccurrence As FactOccurrence = larUsable(0)
        lrFirstOccurrence.EntryBindingKey =
            OccurrencePathEntryBindingKey(lrFirstOccurrence)

        If arPlan.Root IsNot Nothing AndAlso
           Not String.IsNullOrWhiteSpace(arPlan.Root.id) AndAlso
           arPlan.Root.BostonModelElement IsNot Nothing Then

            Dim lsRootKey As String =
                RootKey(arPlan.Root.id)
            Dim lrEntryObject As FBM.ModelObject =
                OccurrencePathEntryObject(lrFirstOccurrence)
            Dim lrFirstPathedRole As FBM.PathedRole =
                lrFirstOccurrence.PathedRoles.FirstOrDefault()
            Dim lbPostJoinFromRoot As Boolean =
                lrFirstPathedRole IsNot Nothing AndAlso
                (String.Equals(lrFirstPathedRole.Purpose,
                               "PostInnerJoin",
                               StringComparison.OrdinalIgnoreCase) OrElse
                 String.Equals(lrFirstPathedRole.Purpose,
                               "PostOuterJoin",
                               StringComparison.OrdinalIgnoreCase))

            If lbPostJoinFromRoot AndAlso
               lrEntryObject IsNot Nothing Then

                ' NORMA normalizes a leading PostInnerJoin/PostOuterJoin to
                ' the preceding PathRoot. Register each actual Object Type as
                ' a separate variable at that root; equal types collapse,
                ' while compatible different types remain correlated.
                Dim loCorrelationRoot As Object =
                    CorrelationRootForKey(arContext,
                                          lsRootKey)
                If loCorrelationRoot Is Nothing Then
                    loCorrelationRoot = arPlan.Root
                End If
                arContext.CorrelationRootByKey(lsRootKey) =
                    loCorrelationRoot
                RegisterVariableAtCorrelationRoot(
                    arContext,
                    loCorrelationRoot,
                    lsRootKey,
                    arPlan.Root.BostonModelElement)

                arContext.CorrelationRootByKey(
                    lrFirstOccurrence.EntryBindingKey) =
                    loCorrelationRoot
                arContext.JoinedToKeyByKey(
                    lrFirstOccurrence.EntryBindingKey) =
                    lsRootKey
                RegisterVariableAtCorrelationRoot(
                    arContext,
                    loCorrelationRoot,
                    lrFirstOccurrence.EntryBindingKey,
                    lrEntryObject)

            ElseIf SameModelObject(
                arPlan.Root.BostonModelElement,
                lrEntryObject) Then
                UnionKeys(arContext,
                          lsRootKey,
                          lrFirstOccurrence.EntryBindingKey)
            End If
        End If

        For liIndex As Integer = 1 To larUsable.Count - 1
            Dim lrCurrent As FactOccurrence = larUsable(liIndex)
            lrCurrent.EntryBindingKey =
                OccurrencePathEntryBindingKey(lrCurrent)

            Dim lrEntryObject As FBM.ModelObject =
                OccurrencePathEntryObject(lrCurrent)
            Dim lsJoinSourceKey As String =
                FindPriorBindingKeyForObject(larUsable,
                                             liIndex - 1,
                                             lrEntryObject)

            If lsJoinSourceKey <> "" Then
                UnionKeys(arContext,
                          lsJoinSourceKey,
                          lrCurrent.EntryBindingKey)

                Dim lrPrevious As FactOccurrence = larUsable(liIndex - 1)

                ' RolePath order above determines which logical objects join.
                ' The prose separator is different: NORMA can continue with
                ' "that" only when the selected FTR's trailing role flows into
                ' the next path entry. Otherwise this is a local restriction
                ' introduced with "where".
                Dim lrPreviousExitObject As FBM.ModelObject =
                    OccurrenceBindingObject(
                        lrPrevious,
                        lrPrevious.BindingKeys.Count - 1)
                lrCurrent.RestrictsPreviousFactType =
                    Not SameModelObject(lrPreviousExitObject,
                                        lrEntryObject)
            Else
                Dim lrPrevious As FactOccurrence = larUsable(liIndex - 1)
                Dim lrPreviousExitObject As FBM.ModelObject =
                    OccurrencePathExitObject(lrPrevious)

                If lrPreviousExitObject IsNot Nothing AndAlso
                   lrEntryObject IsNot Nothing AndAlso
                   Not SameModelObject(lrPreviousExitObject,
                                       lrEntryObject) Then

                    ' The RolePath can cross a subtype boundary without listing
                    ' the subtype Fact Type's Roles. If the next FTR is oriented
                    ' away from the prior path variable, NORMA renders it as a
                    ' new restriction and correlates its path-entry object back
                    ' to the previous path-exit object after the predicate.
                    Dim lrTransition As RoleStringPlan =
                        BuildTransitionPlan(arContext,
                                            lrCurrent.EntryBindingKey,
                                            lrEntryObject,
                                            lrPrevious.ExitBindingKey,
                                            lrPreviousExitObject)

                    If HasSuccessfulTransition(lrTransition) Then
                        lrCurrent.TrailingTransitionPlan = lrTransition
                        lrCurrent.RestrictsPreviousFactType = True
                    End If
                End If
            End If
        Next
    End Sub

    Private Function HasSuccessfulTransition(
        ByVal arPlan As RoleStringPlan) As Boolean

        Return arPlan IsNot Nothing AndAlso
               arPlan.Occurrences IsNot Nothing AndAlso
               arPlan.Occurrences.Count > 0 AndAlso
               Not arPlan.Occurrences.Any(
                   Function(x) x IsNot Nothing AndAlso x.ErrorText <> "")
    End Function

    Private Function OccurrencePathEntryBindingKey(
        ByVal arOccurrence As FactOccurrence) As String

        If arOccurrence Is Nothing OrElse
           arOccurrence.PathBindingKeys Is Nothing OrElse
           arOccurrence.PathBindingKeys.Count = 0 Then Return ""

        Return arOccurrence.PathBindingKeys(0)
    End Function

    Private Function OccurrencePathEntryObject(
        ByVal arOccurrence As FactOccurrence) As FBM.ModelObject

        If arOccurrence Is Nothing OrElse arOccurrence.Roles Is Nothing OrElse
           arOccurrence.Roles.Count = 0 OrElse
           arOccurrence.Roles(0) Is Nothing Then Return Nothing

        Return arOccurrence.Roles(0).JoinedORMObject
    End Function

    Private Function OccurrencePathExitObject(
        ByVal arOccurrence As FactOccurrence) As FBM.ModelObject

        If arOccurrence Is Nothing OrElse arOccurrence.Roles Is Nothing OrElse
           arOccurrence.Roles.Count = 0 OrElse
           arOccurrence.Roles(arOccurrence.Roles.Count - 1) Is Nothing Then
            Return Nothing
        End If

        Return arOccurrence.Roles(arOccurrence.Roles.Count - 1).JoinedORMObject
    End Function

    Private Function FindPriorBindingKeyForObject(
        ByVal aarOccurrences As List(Of FactOccurrence),
        ByVal aiLastOccurrenceIndex As Integer,
        ByVal arObject As FBM.ModelObject) As String

        If aarOccurrences Is Nothing OrElse arObject Is Nothing Then Return ""

        For liOccurrence As Integer = aiLastOccurrenceIndex To 0 Step -1
            Dim lrOccurrence As FactOccurrence = aarOccurrences(liOccurrence)
            If lrOccurrence Is Nothing OrElse
               lrOccurrence.BindingKeys Is Nothing Then Continue For

            For liBinding As Integer =
                Math.Min(lrOccurrence.BindingKeys.Count,
                         SafePredicateParts(lrOccurrence.Reading).Count) - 1 To 0 Step -1

                If SameModelObject(OccurrenceBindingObject(lrOccurrence,
                                                           liBinding),
                                   arObject) Then
                    Return lrOccurrence.BindingKeys(liBinding)
                End If
            Next
        Next

        Return ""
    End Function

    Private Function OccurrenceBindingObject(
        ByVal arOccurrence As FactOccurrence,
        ByVal aiBindingIndex As Integer) As FBM.ModelObject

        If arOccurrence Is Nothing OrElse arOccurrence.Reading Is Nothing Then
            Return Nothing
        End If

        Dim larParts As List(Of FBM.PredicatePart) =
            SafePredicateParts(arOccurrence.Reading)
        If aiBindingIndex < 0 OrElse aiBindingIndex >= larParts.Count Then
            Return Nothing
        End If

        Dim lrPart As FBM.PredicatePart = larParts(aiBindingIndex)
        If lrPart Is Nothing OrElse lrPart.Role Is Nothing Then Return Nothing
        Return lrPart.Role.JoinedORMObject
    End Function

    Private Sub EstablishHeadBindings(ByVal arContext As RenderContext)
        For Each lsRoleId As String In arContext.HeadSourceByRoleId.Keys
            arContext.HeadBindingKeys.Add(
                CanonicalKey(arContext, HeadRoleKey(lsRoleId)))
        Next
    End Sub

    Private Sub PrepareDisplayNames(ByVal arContext As RenderContext)
        Dim larOrderedKeys As New List(Of String)

        ' Head variables are named and ordered from the derived Fact Type
        ' Reading, not from whichever alternative projection happens to be
        ' encountered last. This gives stable ObjectType 1/ObjectType 2 aliases
        ' before existential variables introduced by the RolePaths.
        Dim lrHeadReading As FBM.FactTypeReading =
            PreferredReading(arContext.DerivedFactType)
        If lrHeadReading IsNot Nothing Then
            For Each lrPart As FBM.PredicatePart In SafePredicateParts(lrHeadReading)
                If lrPart Is Nothing OrElse lrPart.Role Is Nothing OrElse
                   Not arContext.HeadSourceByRoleId.ContainsKey(lrPart.Role.Id) Then
                    Continue For
                End If

                AddDisplayCandidate(arContext,
                                    larOrderedKeys,
                                    HeadRoleKey(lrPart.Role.Id),
                                    RolePlayerName(lrPart.Role))
            Next
        End If

        ' NORMA reserves existential aliases in the order in which the RolePath
        ' AST is verbalised. Dictionary registration order is not semantic and
        ' can otherwise give a later negated root "FactType 1" ahead of earlier
        ' FactType variables in preceding branches.
        For Each lrRolePath As FBM.RolePath In arContext.RolePaths
            AddRolePathDisplayCandidates(arContext,
                                         larOrderedKeys,
                                         lrRolePath)
        Next

        For Each lsSyntheticKey As String In arContext.SyntheticKeysInOrder
            Dim lsCanonical As String = CanonicalKey(arContext, lsSyntheticKey)
            If lsCanonical <> "" AndAlso
               arContext.DisplayNameByKey.ContainsKey(lsCanonical) Then
                larOrderedKeys.Add(lsCanonical)
            End If
        Next

        Dim larGroups = larOrderedKeys.
            Distinct(StringComparer.OrdinalIgnoreCase).
            GroupBy(Function(x) arContext.DisplayNameByKey(x), StringComparer.OrdinalIgnoreCase)

        For Each lrGroup In larGroups
            Dim larKeys As List(Of String) = lrGroup.ToList()
            If larKeys.Count < 2 Then Continue For

            For liIndex As Integer = 0 To larKeys.Count - 1
                arContext.AliasNumberByKey(larKeys(liIndex)) = liIndex + 1
            Next
        Next
    End Sub

    Private Sub AddRolePathDisplayCandidates(
        ByVal arContext As RenderContext,
        ByVal aarOrderedKeys As List(Of String),
        ByVal arRolePath As FBM.RolePath)

        If arRolePath Is Nothing Then Exit Sub

        AddRootDisplayCandidate(arContext,
                                aarOrderedKeys,
                                arRolePath.RootObjectType)
        AddPathedRoleDisplayCandidates(arContext,
                                       aarOrderedKeys,
                                       arRolePath.PathedRole)

        If arRolePath.SubPath IsNot Nothing Then
            For Each lrSubPath As FBM.RoleSubPath In arRolePath.SubPath
                AddSubPathDisplayCandidates(arContext,
                                            aarOrderedKeys,
                                            lrSubPath)
            Next
        End If
    End Sub

    Private Sub AddSubPathDisplayCandidates(
        ByVal arContext As RenderContext,
        ByVal aarOrderedKeys As List(Of String),
        ByVal arSubPath As FBM.RoleSubPath)

        If arSubPath Is Nothing Then Exit Sub

        AddRootDisplayCandidate(arContext,
                                aarOrderedKeys,
                                arSubPath.RootObjectType)
        AddPathedRoleDisplayCandidates(arContext,
                                       aarOrderedKeys,
                                       arSubPath.PathedRole)

        If arSubPath.SubPath IsNot Nothing Then
            For Each lrChild As FBM.RoleSubPath In arSubPath.SubPath
                AddSubPathDisplayCandidates(arContext,
                                            aarOrderedKeys,
                                            lrChild)
            Next
        End If
    End Sub

    Private Sub AddRootDisplayCandidate(
        ByVal arContext As RenderContext,
        ByVal aarOrderedKeys As List(Of String),
        ByVal arRoot As FBM.RootObjectType)

        If arRoot Is Nothing OrElse
           String.IsNullOrWhiteSpace(arRoot.id) Then Exit Sub

        AddDisplayCandidate(arContext,
                            aarOrderedKeys,
                            RootKey(arRoot.id),
                            RootObjectTypeName(arRoot))
    End Sub

    Private Sub AddPathedRoleDisplayCandidates(
        ByVal arContext As RenderContext,
        ByVal aarOrderedKeys As List(Of String),
        ByVal aarPathedRoles As List(Of FBM.PathedRole))

        If aarPathedRoles Is Nothing Then Exit Sub

        For Each lrPathedRole As FBM.PathedRole In aarPathedRoles
            If lrPathedRole Is Nothing OrElse
               String.IsNullOrWhiteSpace(lrPathedRole.id) Then Continue For

            Dim lrRole As FBM.Role =
                ResolvePathedRole(arContext,
                                  lrPathedRole)
            AddDisplayCandidate(arContext,
                                aarOrderedKeys,
                                PathedKey(lrPathedRole.id),
                                RolePlayerName(lrRole))
        Next
    End Sub

    Private Sub AddDisplayCandidate(ByVal arContext As RenderContext,
                                    ByVal aarOrderedKeys As List(Of String),
                                    ByVal asKey As String,
                                    ByVal asName As String)
        Dim lsCanonical As String = CanonicalKey(arContext, asKey)
        If lsCanonical = "" OrElse String.IsNullOrWhiteSpace(asName) Then Exit Sub
        If Not arContext.DisplayNameByKey.ContainsKey(lsCanonical) Then
            arContext.DisplayNameByKey(lsCanonical) = asName.Trim()
            aarOrderedKeys.Add(lsCanonical)
        End If
    End Sub

    Private Function RenderHead(ByVal arContext As RenderContext) As String
        Dim lrReading As FBM.FactTypeReading = PreferredReading(arContext.DerivedFactType)
        If lrReading Is Nothing Then
            Return "[ERROR: No Fact Type Reading exists for derived Fact Type '" &
                   SafeFactTypeName(arContext.DerivedFactType) & "']"
        End If

        Dim lrBuilder As New StringBuilder
        AppendRaw(lrBuilder, lrReading.FrontText)

        For Each lrPart As FBM.PredicatePart In SafePredicateParts(lrReading)
            If lrPart Is Nothing OrElse lrPart.Role Is Nothing Then
                AppendWord(lrBuilder, "[ERROR: Head reading contains a Predicate Part without a Role.]")
            Else
                Dim lsRolePlayerName As String = RolePlayerName(lrPart.Role)
                Dim lrSource As FBM.DerivationSource = Nothing

                If arContext.HeadSourceByRoleId.TryGetValue(lrPart.Role.Id, lrSource) Then
                    Dim lsBindingKey As String = HeadRoleKey(lrPart.Role.Id)
                    If lsBindingKey <> "" Then
                        lsRolePlayerName =
                            DisplayName(arContext,
                                        CanonicalKey(arContext, lsBindingKey),
                                        lrPart.Role)
                    End If
                End If

                AppendWord(lrBuilder, BoundRolePlayer(lrPart, lsRolePlayerName))
            End If
            AppendWord(lrBuilder, lrPart.PredicatePartText)
        Next

        AppendWord(lrBuilder, lrReading.FollowingText)
        Return lrBuilder.ToString().Trim()
    End Function

    Private Function RenderBody(ByVal arContext As RenderContext,
                                ByVal aarPlans As List(Of RolePathPlan),
                                ByRef asConditions As String) As String
        If aarPlans Is Nothing OrElse aarPlans.Count = 0 Then Return ""

        Dim lsOperator As String =
            aarPlans(0).SplitOperator

        Dim lsetInitial As New HashSet(Of String)(arContext.HeadBindingKeys, StringComparer.OrdinalIgnoreCase)
        Dim lsetShared As New HashSet(Of String)(lsetInitial, StringComparer.OrdinalIgnoreCase)
        Dim larRendered As New List(Of String)

        For liPlan As Integer = 0 To aarPlans.Count - 1
            Dim lrPlan As RolePathPlan = aarPlans(liPlan)
            Dim lsetMentions As HashSet(Of String)
            If lsOperator = "or" Then
                lsetMentions = New HashSet(Of String)(lsetInitial, StringComparer.OrdinalIgnoreCase)
            Else
                lsetMentions = lsetShared
            End If

            Dim lsLocalConditions As String = ""
            If liPlan > 0 Then
                lsLocalConditions =
                    RenderExplicitConditions(arContext,
                                             lrPlan.RolePath)
            End If

            Dim lsRendered As String
            If lsLocalConditions <> "" Then
                lsRendered =
                    RenderRolePathPlan(arContext,
                                       lrPlan,
                                       lsetMentions,
                                       lsLocalConditions)
                If lsLocalConditions <> "" Then
                    lsRendered &= HtmlBreak &
                                  "where " &
                                  lsLocalConditions
                End If
            Else
                lsRendered =
                    RenderRolePathPlan(arContext,
                                       lrPlan,
                                       lsetMentions,
                                       asConditions)
            End If
            If lsRendered <> "" Then larRendered.Add(lsRendered)
        Next

        Return String.Join(HtmlBreak & lsOperator & " ", larRendered)
    End Function

    Private Function RenderRolePathPlan(
        ByVal arContext As RenderContext,
        ByVal arPlan As RolePathPlan,
        ByVal aMentionedKeys As HashSet(Of String),
        ByRef asConditions As String) As String

        If arPlan Is Nothing Then Return ""

        Dim lrBuilder As New StringBuilder

        Dim lbHasLeadTransition As Boolean =
            HasSuccessfulTransition(arPlan.LeadTransitionPlan)
        Dim lsLeadTransition As String = ""

        If lbHasLeadTransition Then
            lsLeadTransition =
                RenderRoleString(arContext,
                                 arPlan.LeadTransitionPlan,
                                 aMentionedKeys,
                                 False,
                                 asConditions)
            If lsLeadTransition <> "" Then
                lrBuilder.Append(lsLeadTransition)
            End If
        End If

        If arPlan.MainPlan IsNot Nothing Then
            Dim lsMainPlan As String =
                RenderRoleString(arContext,
                                 arPlan.MainPlan,
                                 aMentionedKeys,
                                 lbHasLeadTransition,
                                 asConditions)
            If lsMainPlan <> "" Then
                If lsLeadTransition <> "" Then lrBuilder.Append(" that ")
                lrBuilder.Append(lsMainPlan)
            End If
        End If

        If arPlan.TransitionPlan IsNot Nothing AndAlso
           arPlan.TransitionPlan.Occurrences.Count > 0 Then

            Dim lsTransition As String =
                RenderRoleString(arContext,
                                 arPlan.TransitionPlan,
                                 aMentionedKeys,
                                 True,
                                 asConditions)
            If lsTransition <> "" Then
                If Not arPlan.TransitionPlan.Occurrences.Any(
                    Function(x) x IsNot Nothing AndAlso x.ErrorText <> "") Then
                    lsTransition = "that " & lsTransition
                End If
                If lrBuilder.Length > 0 Then lrBuilder.Append(" ")
                lrBuilder.Append(lsTransition)
            End If
        End If

        If arPlan.SubPlans Is Nothing OrElse arPlan.SubPlans.Count = 0 Then
            If lrBuilder.Length = 0 AndAlso
               Not Object.ReferenceEquals(arPlan.RolePath, arContext.RolePath) Then
                Return RenderExplicitConditions(arContext, arPlan.RolePath)
            End If
            Return lrBuilder.ToString().Trim()
        End If

        Dim larRenderedSubPlans As New List(Of String)
        Dim larRenderedSubPlanOperators As New List(Of String)
        Dim lsetShared As HashSet(Of String) = aMentionedKeys

        For liSubPlan As Integer = 0 To arPlan.SubPlans.Count - 1
            Dim lrSubPlan As RoleStringPlan = arPlan.SubPlans(liSubPlan)
            Dim lsetMentions As HashSet(Of String)

            If arPlan.SplitOperator = "or" Then
                lsetMentions =
                    New HashSet(Of String)(aMentionedKeys,
                                           StringComparer.OrdinalIgnoreCase)
            Else
                lsetMentions = lsetShared
            End If

            Dim lbCollapseLeadRole As Boolean =
                arPlan.TransitionPlan IsNot Nothing OrElse
                lrSubPlan.ContextTransitionPlan IsNot Nothing OrElse
                liSubPlan > 0

            Dim lsRendered As String =
                RenderRoleStringBranch(arContext,
                                       lrSubPlan,
                                       lsetMentions,
                                       lbCollapseLeadRole,
                                       asConditions)

            If lsRendered <> "" Then
                If liSubPlan = 0 AndAlso
                   arPlan.TransitionPlan IsNot Nothing Then
                    lsRendered = "that " & lsRendered
                End If
                larRenderedSubPlans.Add(lsRendered)
                larRenderedSubPlanOperators.Add(
                    If(String.IsNullOrWhiteSpace(lrSubPlan.BranchJoinOperator),
                       arPlan.SplitOperator,
                       lrSubPlan.BranchJoinOperator))
            End If
        Next

        If larRenderedSubPlans.Count > 0 Then
            If lrBuilder.Length > 0 Then
                If arPlan.TransitionPlan IsNot Nothing Then
                    lrBuilder.Append(" ")
                Else
                    lrBuilder.Append(HtmlBreak)
                End If
            End If
            For liIndex As Integer = 0 To larRenderedSubPlans.Count - 1
                If liIndex > 0 Then
                    lrBuilder.Append(HtmlBreak)
                    lrBuilder.Append(larRenderedSubPlanOperators(liIndex))
                    lrBuilder.Append(" ")
                End If
                lrBuilder.Append(larRenderedSubPlans(liIndex))
            Next
        End If

        Return lrBuilder.ToString().Trim()
    End Function

    Private Function RenderRoleStringBranch(
        ByVal arContext As RenderContext,
        ByVal arPlan As RoleStringPlan,
        ByVal aMentionedKeys As HashSet(Of String),
        ByVal abCollapseLeadRole As Boolean,
        ByRef asConditions As String,
        Optional ByVal abCollapseContextTransition As Boolean = False) As String

        If arPlan Is Nothing Then Return ""

        Dim lrBuilder As New StringBuilder
        Dim lsBranchTransition As String = ""
        Dim lsetBranchMentions As HashSet(Of String) =
            If(arPlan.NegatesEntireBranch OrElse
               arPlan.NegatedRootExists,
               New HashSet(Of String)(
                   aMentionedKeys,
                   StringComparer.OrdinalIgnoreCase),
               aMentionedKeys)

        If Not arPlan.NegatedRootExists AndAlso
           arPlan.ContextTransitionPlan IsNot Nothing AndAlso
           arPlan.ContextTransitionPlan.Occurrences.Count > 0 Then
            lsBranchTransition =
                RenderRoleString(arContext,
                                 arPlan.ContextTransitionPlan,
                                 lsetBranchMentions,
                                 abCollapseContextTransition,
                                 asConditions,
                                 abCollapseContextTransition)
        End If

        If arPlan.NegatedRootExists AndAlso
           arPlan.Root IsNot Nothing AndAlso
           Not String.IsNullOrWhiteSpace(arPlan.Root.id) Then

            Dim lsRootKey As String =
                CanonicalKey(arContext,
                             RootKey(arPlan.Root.id))
            Dim lsRootName As String =
                DisplayName(arContext,
                            lsRootKey,
                            Nothing)
            If lsRootName = "" Then
                lsRootName = RootObjectTypeName(arPlan.Root)
            End If

            lsetBranchMentions.Add(lsRootKey)
            lrBuilder.Append("no ")
            lrBuilder.Append(lsRootName)
            lrBuilder.Append(" exists")
            lrBuilder.Append(HtmlBreak)
            lrBuilder.Append("where ")
        End If

        Dim lbCollapseMain As Boolean =
            Not arPlan.NegatedRootExists AndAlso
            (abCollapseLeadRole OrElse
             lsBranchTransition <> "")

        Dim lsMain As String =
            RenderRoleString(arContext,
                             arPlan,
                             lsetBranchMentions,
                             lbCollapseMain,
                             asConditions)

        If lsBranchTransition <> "" Then
            lrBuilder.Append(lsBranchTransition)
            If lsMain <> "" Then
                If Not arPlan.ContextTransitionPlan.Occurrences.Any(
                    Function(x) x IsNot Nothing AndAlso x.ErrorText <> "") Then
                    lrBuilder.Append(" that ")
                Else
                    lrBuilder.Append(" ")
                End If
            End If
        End If
        If lsMain <> "" Then lrBuilder.Append(lsMain)

        If arPlan.ChildPlans IsNot Nothing AndAlso
           arPlan.ChildPlans.Count > 0 Then

            Dim lsPreviousExitKey As String =
                CanonicalKey(arContext,
                             PlanExitBindingKey(arPlan))

            For liChild As Integer = 0 To arPlan.ChildPlans.Count - 1
                Dim lrChild As RoleStringPlan =
                    arPlan.ChildPlans(liChild)
                Dim lsChildEntryKey As String =
                    CanonicalKey(arContext,
                                 PlanEntryBindingKey(lrChild))

                Dim lbCollapseChildLead As Boolean =
                    lrChild.ContextTransitionPlan IsNot Nothing OrElse
                    (lsPreviousExitKey <> "" AndAlso
                     String.Equals(lsPreviousExitKey,
                                   lsChildEntryKey,
                                   StringComparison.OrdinalIgnoreCase))

                Dim lbCollapseChildContextTransition As Boolean =
                    lrChild.ContextTransitionPlan IsNot Nothing AndAlso
                    lsPreviousExitKey <> "" AndAlso
                    String.Equals(
                        lsPreviousExitKey,
                        CanonicalKey(
                            arContext,
                            PlanLogicalEntryBindingKey(
                                lrChild.ContextTransitionPlan)),
                        StringComparison.OrdinalIgnoreCase)

                Dim lsChild As String =
                    RenderRoleStringBranch(arContext,
                                           lrChild,
                                           lsetBranchMentions,
                                           lbCollapseChildLead,
                                           asConditions,
                                           lbCollapseChildContextTransition)

                If lsChild <> "" Then
                    lrBuilder.Append(HtmlBreak)
                    If liChild > 0 Then
                        lrBuilder.Append(
                            If(String.IsNullOrWhiteSpace(
                                   lrChild.BranchJoinOperator),
                               arPlan.ChildSplitOperator,
                               lrChild.BranchJoinOperator))
                        lrBuilder.Append(" ")
                    ElseIf lbCollapseChildLead AndAlso
                       lrChild.ContextTransitionPlan Is Nothing Then
                        lrBuilder.Append("that ")
                    End If
                    lrBuilder.Append(lsChild)
                End If

                lsPreviousExitKey =
                    CanonicalKey(arContext,
                                 PlanExitBindingKey(lrChild))
            Next
        End If

        Dim lsResult As String =
            lrBuilder.ToString().Trim()

        If arPlan.NegatesEntireBranch AndAlso lsResult <> "" Then
            lsResult = "it is not true that (" & lsResult & ")"
        End If

        Return lsResult
    End Function

    Private Function RenderRoleString(ByVal arContext As RenderContext,
                                      ByVal arPlan As RoleStringPlan,
                                      ByVal aMentionedKeys As HashSet(Of String),
                                      ByVal abCollapseLeadRole As Boolean,
                                      ByRef asConditions As String,
                                      Optional ByVal abPreserveCollapsedLeadConnector As Boolean = False) As String
        Dim lrBuilder As New StringBuilder
        Dim lsPreviousExitKey As String = ""
        Dim lbInNegatedChain As Boolean = False
        Dim lbFirstRenderedOccurrence As Boolean = True

        For Each lrOccurrence As FactOccurrence In arPlan.Occurrences
            If lrOccurrence Is Nothing Then Continue For

            If lrOccurrence.ErrorText <> "" Then
                AppendWord(lrBuilder, lrOccurrence.ErrorText)
                lsPreviousExitKey = ""
                Continue For
            End If

            Dim lsNegativeQuantifierKey As String =
                NegativeQuantifierBindingKey(arContext,
                                             lrOccurrence,
                                             aMentionedKeys)

            Dim lbEmitsNegatedWhere As Boolean =
                lrOccurrence.IsNegated AndAlso
                lsNegativeQuantifierKey = "" AndAlso
                Not lbInNegatedChain

            If Not lbFirstRenderedOccurrence AndAlso
               lrOccurrence.RestrictsPreviousFactType AndAlso
               Not lbEmitsNegatedWhere Then
                If lrBuilder.Length > 0 Then lrBuilder.Append(HtmlBreak)
                lrBuilder.Append("where ")
                lsPreviousExitKey = ""
            End If

            If lbEmitsNegatedWhere Then
                If lrBuilder.Length > 0 Then lrBuilder.Append(HtmlBreak)
                lrBuilder.Append("where it is not true that (")
                lbInNegatedChain = True
                lsPreviousExitKey = ""
            End If

            Dim lbCollapseFirst As Boolean = False
            If lbFirstRenderedOccurrence AndAlso abCollapseLeadRole AndAlso
               Not lbInNegatedChain Then
                lbCollapseFirst = True
            ElseIf lsPreviousExitKey <> "" AndAlso lrOccurrence.BindingKeys.Count > 0 Then
                lbCollapseFirst =
                    String.Equals(CanonicalKey(arContext, lsPreviousExitKey),
                                  CanonicalKey(arContext, lrOccurrence.BindingKeys(0)),
                                  StringComparison.OrdinalIgnoreCase)
            End If

            AppendWord(lrBuilder,
                       RenderFactOccurrence(arContext,
                                            lrOccurrence,
                                            aMentionedKeys,
                                            lbCollapseFirst,
                                            lsNegativeQuantifierKey,
                                            lbFirstRenderedOccurrence AndAlso
                                            arPlan.StartsRolePath))

            Dim lsValueRestrictions As String =
                RenderOccurrenceValueRestrictions(arContext,
                                                  lrOccurrence)
            If lsValueRestrictions <> "" Then
                AppendWord(lrBuilder,
                           "where " & lsValueRestrictions)
            End If

            If HasSuccessfulTransition(lrOccurrence.TrailingTransitionPlan) Then
                Dim lsTransition As String =
                    RenderRoleString(arContext,
                                     lrOccurrence.TrailingTransitionPlan,
                                     aMentionedKeys,
                                     True,
                                     asConditions)
                If lsTransition <> "" Then
                    AppendWord(lrBuilder, "that " & lsTransition)
                End If
            End If

            lbFirstRenderedOccurrence = False

            If lrOccurrence.ExitBindingKey <> "" Then
                lsPreviousExitKey = lrOccurrence.ExitBindingKey
            End If
        Next

        If lbInNegatedChain Then
            If asConditions <> "" Then
                lrBuilder.Append(HtmlBreak)
                lrBuilder.Append("where ")
                lrBuilder.Append(asConditions)
                asConditions = ""
            End If
            lrBuilder.Append(")")
        End If

        Dim lsResult As String = lrBuilder.ToString().Trim()
        If abCollapseLeadRole AndAlso
           Not abPreserveCollapsedLeadConnector AndAlso
           lsResult.StartsWith("that ", StringComparison.OrdinalIgnoreCase) Then
            lsResult = lsResult.Substring(5)
        End If
        Return lsResult
    End Function

    Private Function NegativeQuantifierBindingKey(
        ByVal arContext As RenderContext,
        ByVal arOccurrence As FactOccurrence,
        ByVal aMentionedKeys As HashSet(Of String)) As String

        If arOccurrence Is Nothing OrElse
           Not arOccurrence.IsNegated OrElse
           arOccurrence.BindingKeys Is Nothing OrElse
           arOccurrence.BindingKeys.Count < 2 Then Return ""

        ' A negated traversal from an already-bound object to a new object is
        ' verbalised by NORMA with a negative existential ("belongs to no
        ' FactType"). If every role is already bound, retain the explicit
        ' "it is not true that (...)" form.
        Dim lsLeadKey As String =
            CanonicalKey(arContext,
                         arOccurrence.BindingKeys(0))
        If lsLeadKey = "" OrElse
           Not aMentionedKeys.Contains(lsLeadKey) Then Return ""

        For liIndex As Integer = 1 To arOccurrence.BindingKeys.Count - 1
            Dim lsCandidate As String =
                CanonicalKey(arContext,
                             arOccurrence.BindingKeys(liIndex))
            If lsCandidate <> "" AndAlso
               Not aMentionedKeys.Contains(lsCandidate) Then
                Return lsCandidate
            End If
        Next

        Return ""
    End Function

    Private Function RenderOccurrenceValueRestrictions(
        ByVal arContext As RenderContext,
        ByVal arOccurrence As FactOccurrence) As String

        If arOccurrence Is Nothing OrElse
           arOccurrence.PathedRoles Is Nothing OrElse
           arOccurrence.PathBindingKeys Is Nothing Then Return ""

        Dim larRestrictions As New List(Of String)

        For liIndex As Integer = 0 To Math.Min(arOccurrence.PathedRoles.Count, arOccurrence.PathBindingKeys.Count) - 1

            Dim lrPathedRole As FBM.PathedRole =
                arOccurrence.PathedRoles(liIndex)
            If lrPathedRole Is Nothing OrElse
               lrPathedRole.ValueRestriction Is Nothing OrElse
               lrPathedRole.ValueRestriction.
                   PathedRoleConditionValueConstraint Is Nothing OrElse
               lrPathedRole.ValueRestriction.
                   PathedRoleConditionValueConstraint.ValueRange Is Nothing Then
                Continue For
            End If

            Dim larRanges As List(Of FBM.PathedRoleValueRange) =
                lrPathedRole.ValueRestriction.
                    PathedRoleConditionValueConstraint.ValueRange

            If larRanges.Count <> 1 Then
                larRestrictions.Add(
                    "[ERROR: Unsupported PathedRole value restriction with " &
                    larRanges.Count.ToString() & " ranges.]")
                Continue For
            End If

            Dim lrRange As FBM.PathedRoleValueRange =
                larRanges(0)
            If lrRange Is Nothing OrElse
               Not String.Equals(lrRange.MinValue,
                                 lrRange.MaxValue,
                                 StringComparison.Ordinal) Then
                larRestrictions.Add(
                    "[ERROR: Unsupported non-single-value PathedRole restriction.]")
                Continue For
            End If

            Dim lrRole As FBM.Role =
                ResolvePathedRole(arContext,
                                  lrPathedRole)
            Dim lsName As String =
                DisplayName(
                    arContext,
                    CanonicalKey(arContext,
                                 arOccurrence.PathBindingKeys(liIndex)),
                    lrRole)

            larRestrictions.Add(
                "the possible value of that " & lsName &
                " is " & FormatRestrictedValue(lrRole,
                                                lrRange))
        Next

        Return String.Join(" and ", larRestrictions)
    End Function

    Private Function FormatRestrictedValue(
        ByVal arRole As FBM.Role,
        ByVal arRange As FBM.PathedRoleValueRange) As String

        If arRange Is Nothing Then Return "''"

        If Not String.IsNullOrWhiteSpace(
            arRange.InvariantMinValue) AndAlso
           String.Equals(arRange.InvariantMinValue,
                         arRange.InvariantMaxValue,
                         StringComparison.Ordinal) Then
            Return arRange.InvariantMinValue
        End If

        Dim lsValue As String =
            If(arRange.MinValue, "")

        If arRole IsNot Nothing AndAlso
           TypeOf arRole.JoinedORMObject Is FBM.ValueType AndAlso
           DirectCast(arRole.JoinedORMObject,
                      FBM.ValueType).DataTypeIsNumeric Then
            Return lsValue
        End If

        If arRole IsNot Nothing AndAlso
           TypeOf arRole.JoinedORMObject Is FBM.EntityType Then
            Dim lrEntityType As FBM.EntityType =
                DirectCast(arRole.JoinedORMObject,
                           FBM.EntityType)
            If lrEntityType.ReferenceModeValueType IsNot Nothing AndAlso
               lrEntityType.ReferenceModeValueType.DataTypeIsNumeric Then
                Return lsValue
            End If
        End If

        Return "'" & lsValue.Replace("'", "''") & "'"
    End Function

    Private Function CanCollapsePlanLead(ByVal arContext As RenderContext,
                                         ByVal arPlan As RoleStringPlan) As Boolean
        If arPlan Is Nothing OrElse arPlan.Root Is Nothing OrElse
           String.IsNullOrWhiteSpace(arPlan.Root.id) OrElse
           arPlan.Occurrences Is Nothing OrElse arPlan.Occurrences.Count = 0 Then
            Return False
        End If

        Dim lrFirstOccurrence As FactOccurrence =
            arPlan.Occurrences.FirstOrDefault(
                Function(x) x IsNot Nothing AndAlso x.BindingKeys IsNot Nothing AndAlso
                            x.BindingKeys.Count > 0)
        If lrFirstOccurrence Is Nothing Then Return False

        Dim lsRootKey As String =
            CanonicalKey(arContext, RootKey(arPlan.Root.id))
        Dim lsLeadKey As String =
            CanonicalKey(arContext, lrFirstOccurrence.EntryBindingKey)

        Return lsRootKey <> "" AndAlso
               String.Equals(lsRootKey, lsLeadKey, StringComparison.OrdinalIgnoreCase) AndAlso
               arContext.HeadBindingKeys.Contains(lsRootKey)
    End Function

    Private Function RenderFactOccurrence(ByVal arContext As RenderContext,
                                          ByVal arOccurrence As FactOccurrence,
                                          ByVal aMentionedKeys As HashSet(Of String),
                                          ByVal abCollapseFirstRole As Boolean,
                                          Optional ByVal asNegativeQuantifierKey As String = "",
                                          Optional ByVal abBasicLeadRole As Boolean = False) As String
        Dim larParts As List(Of FBM.PredicatePart) = SafePredicateParts(arOccurrence.Reading)
        If larParts.Count = 0 Then
            Return "[ERROR: Fact Type Reading for '" &
                   SafeFactTypeName(arOccurrence.FactType) &
                   "' has no Predicate Parts.]"
        End If

        Dim lrBuilder As New StringBuilder
        Dim lsetOccurrenceKeys As New HashSet(Of String)(
            StringComparer.OrdinalIgnoreCase)
        For Each lsBindingKey As String In arOccurrence.BindingKeys
            Dim lsCanonicalBindingKey As String =
                CanonicalKey(arContext,
                             lsBindingKey)
            If lsCanonicalBindingKey <> "" Then
                lsetOccurrenceKeys.Add(
                    lsCanonicalBindingKey)
            End If
        Next

        AppendRaw(lrBuilder, arOccurrence.Reading.FrontText)

        For liIndex As Integer = 0 To larParts.Count - 1
            Dim lrPart As FBM.PredicatePart = larParts(liIndex)

            If liIndex = 0 AndAlso abCollapseFirstRole Then
                AppendWord(lrBuilder, "that")
                If arOccurrence.BindingKeys.Count > 0 Then
                    aMentionedKeys.Add(CanonicalKey(arContext, arOccurrence.BindingKeys(0)))
                End If
            Else
                Dim lsKey As String =
                    If(liIndex < arOccurrence.BindingKeys.Count,
                       arOccurrence.BindingKeys(liIndex),
                       "")
                AppendWord(lrBuilder,
                           RenderRoleMention(arContext,
                                             lrPart,
                                             lsKey,
                                             aMentionedKeys,
                                             asNegativeQuantifierKey,
                                             lsetOccurrenceKeys,
                                             abBasicLeadRole AndAlso
                                             liIndex = 0))
            End If

            If lrPart IsNot Nothing Then AppendWord(lrBuilder, lrPart.PredicatePartText)
        Next

        AppendWord(lrBuilder, arOccurrence.Reading.FollowingText)
        Return lrBuilder.ToString().Trim()
    End Function

    Private Function RenderRoleMention(ByVal arContext As RenderContext,
                                       ByVal arPart As FBM.PredicatePart,
                                       ByVal asBindingKey As String,
                                       ByVal aMentionedKeys As HashSet(Of String),
                                       Optional ByVal asNegativeQuantifierKey As String = "",
                                       Optional ByVal aOccurrenceKeys As HashSet(Of String) = Nothing,
                                       Optional ByVal abBasicLeadRole As Boolean = False) As String
        If arPart Is Nothing OrElse arPart.Role Is Nothing Then
            Return "[ERROR: Predicate Part has no Role.]"
        End If

        Dim lsCanonical As String = CanonicalKey(arContext, asBindingKey)
        Dim lsName As String = DisplayName(arContext, lsCanonical, arPart.Role)
        Dim lsBarePhrase As String = BoundRolePlayer(arPart, lsName)

        If asNegativeQuantifierKey <> "" AndAlso
           String.Equals(lsCanonical,
                         asNegativeQuantifierKey,
                         StringComparison.OrdinalIgnoreCase) Then
            If lsCanonical <> "" Then aMentionedKeys.Add(lsCanonical)
            Return "no " & lsBarePhrase
        End If

        If lsCanonical <> "" AndAlso aMentionedKeys.Contains(lsCanonical) Then
            Return "that " & lsBarePhrase
        End If

        Dim lsPartnerKey As String =
            CorrelatedPartnerKey(arContext,
                                 asBindingKey,
                                 aMentionedKeys,
                                 aOccurrenceKeys)

        If lsCanonical <> "" Then aMentionedKeys.Add(lsCanonical)

        If lsPartnerKey <> "" Then
            Dim lsPartnerName As String =
                DisplayName(arContext,
                            lsPartnerKey,
                            Nothing)
            If lsPartnerName <> "" Then
                If abBasicLeadRole Then
                    Dim lsLeadConnector As String = "that"
                    If TypeOf arPart.Role.JoinedORMObject Is FBM.EntityType AndAlso
                       DirectCast(arPart.Role.JoinedORMObject,
                                  FBM.EntityType).IsPersonal Then
                        lsLeadConnector = "who"
                    End If

                    Return "that " & lsPartnerName &
                           " is some " & lsBarePhrase &
                           " " & lsLeadConnector
                End If

                Dim lsIdentityConnector As String = " that is that "
                If TypeOf arPart.Role.JoinedORMObject Is FBM.EntityType AndAlso
                   DirectCast(arPart.Role.JoinedORMObject,
                              FBM.EntityType).IsPersonal Then
                    lsIdentityConnector = " who is that "
                End If

                Return "some " & lsBarePhrase &
                       lsIdentityConnector &
                       lsPartnerName
            End If
        End If

        Return "some " & lsBarePhrase
    End Function

    Private Function CorrelatedPartnerKey(
        ByVal arContext As RenderContext,
        ByVal asBindingKey As String,
        ByVal aMentionedKeys As HashSet(Of String),
        ByVal aOccurrenceKeys As HashSet(Of String)) As String

        If String.IsNullOrWhiteSpace(asBindingKey) OrElse
           aMentionedKeys Is Nothing Then Return ""

        Dim loCorrelationRoot As Object =
            CorrelationRootForKey(arContext,
                                  asBindingKey)
        If loCorrelationRoot Is Nothing Then Return ""

        Dim lsCurrentKey As String =
            CanonicalKey(arContext,
                         asBindingKey)
        Dim larCandidateKeys As List(Of String) = Nothing
        If Not arContext.VariableKeysByCorrelationRootInOrder.
            TryGetValue(loCorrelationRoot,
                        larCandidateKeys) Then Return ""

        ' NORMA gives the directly joined variable first priority.
        Dim lsJoinedToKey As String = ""
        If Not arContext.JoinedToKeyByKey.TryGetValue(
            asBindingKey,
            lsJoinedToKey) Then

            For Each lrPair As KeyValuePair(Of String, String) In
                arContext.JoinedToKeyByKey

                If String.Equals(
                    CanonicalKey(arContext,
                                 lrPair.Key),
                    lsCurrentKey,
                    StringComparison.OrdinalIgnoreCase) Then
                    lsJoinedToKey = lrPair.Value
                    Exit For
                End If
            Next
        End If

        Dim lsPartnerKey As String =
            EligibleCorrelatedPartnerKey(arContext,
                                         lsCurrentKey,
                                         lsJoinedToKey,
                                         aMentionedKeys,
                                         aOccurrenceKeys)
        If lsPartnerKey <> "" Then Return lsPartnerKey

        ' Next prefer a head variable, exactly as NORMA does.
        For Each lsCandidateKey As String In larCandidateKeys
            Dim lsCandidateCanonical As String =
                EligibleCorrelatedPartnerKey(arContext,
                                             lsCurrentKey,
                                             lsCandidateKey,
                                             aMentionedKeys,
                                             aOccurrenceKeys)
            If lsCandidateCanonical <> "" AndAlso
               arContext.HeadBindingKeys.Contains(
                   lsCandidateCanonical) Then
                Return lsCandidateCanonical
            End If
        Next

        ' Finally use the first already-used variable in registration order.
        For Each lsCandidateKey As String In larCandidateKeys
            lsPartnerKey =
                EligibleCorrelatedPartnerKey(arContext,
                                             lsCurrentKey,
                                             lsCandidateKey,
                                             aMentionedKeys,
                                             aOccurrenceKeys)
            If lsPartnerKey <> "" Then Return lsPartnerKey
        Next

        Return ""
    End Function

    Private Function EligibleCorrelatedPartnerKey(
        ByVal arContext As RenderContext,
        ByVal asCurrentCanonicalKey As String,
        ByVal asCandidateKey As String,
        ByVal aMentionedKeys As HashSet(Of String),
        ByVal aOccurrenceKeys As HashSet(Of String)) As String

        If String.IsNullOrWhiteSpace(asCandidateKey) Then Return ""

        Dim lsCandidateCanonical As String =
            CanonicalKey(arContext,
                         asCandidateKey)
        If lsCandidateCanonical = "" OrElse
           String.Equals(lsCandidateCanonical,
                         asCurrentCanonicalKey,
                         StringComparison.OrdinalIgnoreCase) OrElse
           (aOccurrenceKeys IsNot Nothing AndAlso
            aOccurrenceKeys.Contains(
                lsCandidateCanonical)) OrElse
           Not aMentionedKeys.Contains(
               lsCandidateCanonical) Then Return ""

        Return lsCandidateCanonical
    End Function

    Private Function RenderConditions(ByVal arContext As RenderContext) As String
        Dim larConditions As New List(Of String)

        Dim lsExplicitConditions As String =
            RenderExplicitConditions(arContext, arContext.RolePath)
        If lsExplicitConditions <> "" Then larConditions.Add(lsExplicitConditions)

        If larConditions.Count = 0 Then
            larConditions.AddRange(RenderUnprojectedBooleanCalculations(arContext))
        End If

        larConditions.AddRange(RenderProjectedCalculations(arContext))

        Return String.Join(" and ",
                           larConditions.
                               Where(Function(x) Not String.IsNullOrWhiteSpace(x)).
                               Distinct(StringComparer.OrdinalIgnoreCase))
    End Function

    Private Function RenderExplicitConditions(ByVal arContext As RenderContext,
                                              ByVal arRolePath As FBM.RolePath) As String
        If arRolePath Is Nothing OrElse
           arRolePath.Conditions Is Nothing OrElse
           arRolePath.Conditions.Items Is Nothing Then Return ""

        Dim larConditions As New List(Of String)

        For Each lrCondition As FBM.ConditionNode In arRolePath.Conditions.Items
            Dim lsCondition As String =
                RenderConditionNode(arContext,
                                    lrCondition,
                                    New HashSet(Of String)(StringComparer.OrdinalIgnoreCase))
            If lsCondition <> "" Then larConditions.Add(lsCondition)
        Next

        Return String.Join(" and ",
                           larConditions.
                               Where(Function(x) Not String.IsNullOrWhiteSpace(x)).
                               Distinct(StringComparer.OrdinalIgnoreCase))
    End Function

    Private Function RenderProjectedCalculations(
        ByVal arContext As RenderContext) As List(Of String)

        Dim larProjections As New List(Of String)

        For Each lrPair As KeyValuePair(Of String, FBM.DerivationSource) In
            arContext.HeadSourceByRoleId

            Dim lrSource As FBM.DerivationSource = lrPair.Value
            If lrSource Is Nothing OrElse lrSource.CalculatedValue Is Nothing OrElse
               String.IsNullOrWhiteSpace(lrSource.CalculatedValue.Ref) Then Continue For

            Dim lrDerivedRole As FBM.Role =
                ResolveRoleInFactType(arContext.DerivedFactType, lrPair.Key)
            If lrDerivedRole Is Nothing Then
                larProjections.Add("[ERROR: Projected derived Role '" &
                                   CleanError(lrPair.Key) & "' was not found.]")
                Continue For
            End If

            Dim lsExpression As String =
                RenderCalculatedValue(
                    arContext,
                    lrSource.CalculatedValue.Ref,
                    New HashSet(Of String)(StringComparer.OrdinalIgnoreCase))

            larProjections.Add(RolePlayerName(lrDerivedRole) & " = " & lsExpression)
        Next

        Return larProjections
    End Function

    Private Function RenderUnprojectedBooleanCalculations(
        ByVal arContext As RenderContext) As List(Of String)

        Dim larConditions As New List(Of String)
        If arContext.RolePath.CalculatedValues Is Nothing Then Return larConditions

        Dim lsetProjectedIds As New HashSet(Of String)(StringComparer.OrdinalIgnoreCase)
        For Each lrSource As FBM.DerivationSource In arContext.HeadSourceByRoleId.Values
            If lrSource IsNot Nothing AndAlso lrSource.CalculatedValue IsNot Nothing AndAlso
               Not String.IsNullOrWhiteSpace(lrSource.CalculatedValue.Ref) Then
                lsetProjectedIds.Add(lrSource.CalculatedValue.Ref)
            End If
        Next

        For Each lrValue As FBM.CalculatedValue In arContext.RolePath.CalculatedValues
            If lrValue Is Nothing OrElse String.IsNullOrWhiteSpace(lrValue.Id) OrElse
               lsetProjectedIds.Contains(lrValue.Id) OrElse
               Not IsBooleanFunction(arContext.Model, lrValue.Function) Then Continue For

            Dim lsCondition As String =
                RenderCalculatedValue(arContext,
                                      lrValue.Id,
                                      New HashSet(Of String)(StringComparer.OrdinalIgnoreCase))
            If lsCondition <> "" Then larConditions.Add(lsCondition)
        Next

        Return larConditions
    End Function

    Private Function IsBooleanFunction(ByVal arModel As FBM.Model,
                                       ByVal arFunctionReference As FBM.FunctionRef) As Boolean
        If arModel Is Nothing OrElse arFunctionReference Is Nothing OrElse
           String.IsNullOrWhiteSpace(arFunctionReference.Ref) OrElse
           arModel.Function Is Nothing Then Return False

        Dim lrFunction As FBM.Function =
            arModel.Function.FirstOrDefault(
                Function(x) x IsNot Nothing AndAlso
                            String.Equals(x.id,
                                          arFunctionReference.Ref,
                                          StringComparison.OrdinalIgnoreCase))
        Return lrFunction IsNot Nothing AndAlso lrFunction.IsBoolean
    End Function

    Private Function RenderConditionNode(ByVal arContext As RenderContext,
                                         ByVal arCondition As FBM.ConditionNode,
                                         ByVal aCalculationStack As HashSet(Of String)) As String
        If arCondition Is Nothing Then Return ""

        If TypeOf arCondition Is FBM.ConditionAnd Then
            Return RenderConditionList(arContext,
                                       DirectCast(arCondition, FBM.ConditionAnd).Items,
                                       " and ",
                                       aCalculationStack)
        End If

        If TypeOf arCondition Is FBM.ConditionOr Then
            Return RenderConditionList(arContext,
                                       DirectCast(arCondition, FBM.ConditionOr).Items,
                                       " or ",
                                       aCalculationStack)
        End If

        If TypeOf arCondition Is FBM.ConditionNot Then
            Dim lsNested As String =
                RenderConditionNode(arContext,
                                    DirectCast(arCondition, FBM.ConditionNot).Item,
                                    aCalculationStack)
            If lsNested = "" Then Return ""
            Return "not (" & lsNested & ")"
        End If

        If TypeOf arCondition Is FBM.CalculatedCondition Then
            Return RenderCalculatedValue(arContext,
                                         DirectCast(arCondition, FBM.CalculatedCondition).Ref,
                                         aCalculationStack)
        End If

        If TypeOf arCondition Is FBM.ConditionBinaryComparison Then
            Dim lrComparison As FBM.ConditionBinaryComparison =
                DirectCast(arCondition, FBM.ConditionBinaryComparison)
            Return RenderConditionValue(arContext, lrComparison.Left, aCalculationStack) &
                   " " & ComparisonOperator(arCondition) & " " &
                   RenderConditionValue(arContext, lrComparison.Right, aCalculationStack)
        End If

        Return "[ERROR: Unsupported derivation condition '" &
               CleanError(arCondition.GetType().Name) & "'.]"
    End Function

    Private Function RenderConditionList(ByVal arContext As RenderContext,
                                         ByVal aarConditions As List(Of FBM.ConditionNode),
                                         ByVal asJoiner As String,
                                         ByVal aCalculationStack As HashSet(Of String)) As String
        If aarConditions Is Nothing Then Return ""

        Dim larText As New List(Of String)
        For Each lrCondition As FBM.ConditionNode In aarConditions
            Dim lsText As String = RenderConditionNode(arContext, lrCondition, aCalculationStack)
            If lsText <> "" Then larText.Add(lsText)
        Next
        Return String.Join(asJoiner, larText)
    End Function

    Private Function RenderConditionValue(ByVal arContext As RenderContext,
                                          ByVal arValue As FBM.ConditionValueContainer,
                                          ByVal aCalculationStack As HashSet(Of String)) As String
        If arValue Is Nothing OrElse arValue.Item Is Nothing Then
            Return "[ERROR: Missing condition operand.]"
        End If

        If TypeOf arValue.Item Is FBM.PathedRoleRef Then
            Return RenderPathedRoleValue(arContext,
                                         DirectCast(arValue.Item, FBM.PathedRoleRef).Ref)
        End If

        If TypeOf arValue.Item Is FBM.CalculatedValueRef Then
            Return RenderCalculatedValue(arContext,
                                         DirectCast(arValue.Item, FBM.CalculatedValueRef).Ref,
                                         aCalculationStack)
        End If

        If TypeOf arValue.Item Is FBM.Constant Then
            Return DirectCast(arValue.Item, FBM.Constant).Value
        End If

        Return "[ERROR: Unsupported condition operand.]"
    End Function

    Private Function RenderCalculatedValue(ByVal arContext As RenderContext,
                                           ByVal asCalculatedValueId As String,
                                           ByVal aCalculationStack As HashSet(Of String)) As String
        If String.IsNullOrWhiteSpace(asCalculatedValueId) Then
            Return "[ERROR: Missing CalculatedValue reference.]"
        End If

        If aCalculationStack.Contains(asCalculatedValueId) Then
            Return "[ERROR: Circular CalculatedValue '" & CleanError(asCalculatedValueId) & "'.]"
        End If

        Dim lrValue As FBM.CalculatedValue = Nothing
        If Not arContext.CalculatedValueById.TryGetValue(asCalculatedValueId, lrValue) Then
            Return "[ERROR: CalculatedValue '" & CleanError(asCalculatedValueId) & "' was not found.]"
        End If

        aCalculationStack.Add(asCalculatedValueId)
        Dim larInputs As New List(Of String)
        Dim lsFunctionName As String =
            ResolveFunctionName(arContext.Model, lrValue.Function)

        If lrValue.Inputs IsNot Nothing AndAlso lrValue.Inputs.Input IsNot Nothing Then
            For Each lrInput As FBM.Input In lrValue.Inputs.Input
                larInputs.Add(RenderCalculatedInput(arContext,
                                                    lrInput,
                                                    aCalculationStack,
                                                    lsFunctionName))
            Next
        End If

        aCalculationStack.Remove(asCalculatedValueId)

        If lrValue.AggregationContext IsNot Nothing AndAlso
           lrValue.AggregationContext.PathRoot IsNot Nothing AndAlso
           larInputs.Count = 1 AndAlso
           FunctionHasBagInput(arContext.Model, lrValue.Function) Then

            Dim lsRootReference As String = lrValue.AggregationContext.PathRoot.Ref
            Dim lsRootName As String = ""
            Dim lrRoot As FBM.RootObjectType = Nothing

            If arContext.RootById.TryGetValue(lsRootReference, lrRoot) Then
                lsRootName =
                    DisplayName(arContext,
                                CanonicalKey(arContext, RootKey(lsRootReference)),
                                Nothing)
                If lsRootName = "" Then lsRootName = RootObjectTypeName(lrRoot)
            End If

            Return lsFunctionName & "(each " & larInputs(0) &
                   " for that " & lsRootName & ")"
        End If

        If IsInfixFunction(lsFunctionName) AndAlso larInputs.Count = 2 Then
            Return larInputs(0) & " " & lsFunctionName & " " & larInputs(1)
        End If

        Return lsFunctionName & "(" & String.Join(", ", larInputs) & ")"
    End Function

    Private Function RenderCalculatedInput(ByVal arContext As RenderContext,
                                           ByVal arInput As FBM.Input,
                                           ByVal aCalculationStack As HashSet(Of String),
                                           ByVal asParentFunctionName As String) As String
        If arInput Is Nothing OrElse arInput.Source Is Nothing OrElse
           arInput.Source.Item Is Nothing Then
            Return "[ERROR: Missing CalculatedValue input.]"
        End If

        If TypeOf arInput.Source.Item Is FBM.PathedRoleRef Then
            Return RenderPathedRoleValue(arContext,
                                         DirectCast(arInput.Source.Item, FBM.PathedRoleRef).Ref)
        End If

        If TypeOf arInput.Source.Item Is FBM.PathRootReference Then
            Return RenderPathRootValue(arContext,
                                       DirectCast(arInput.Source.Item, FBM.PathRootReference).Ref)
        End If

        If TypeOf arInput.Source.Item Is FBM.CalculatedValueRef Then
            Dim lsChildCalculatedValueId As String =
                DirectCast(arInput.Source.Item, FBM.CalculatedValueRef).Ref
            Dim lsRenderedChild As String =
                RenderCalculatedValue(arContext,
                                      lsChildCalculatedValueId,
                                      aCalculationStack)
            Dim lrChildValue As FBM.CalculatedValue = Nothing
            If IsArithmeticFunction(asParentFunctionName) AndAlso
               arContext.CalculatedValueById.TryGetValue(lsChildCalculatedValueId,
                                                         lrChildValue) AndAlso
               lrChildValue IsNot Nothing AndAlso
               IsArithmeticFunction(
                   ResolveFunctionName(arContext.Model,
                                       lrChildValue.Function)) Then
                Return "(" & lsRenderedChild & ")"
            End If

            Return lsRenderedChild
        End If

        If TypeOf arInput.Source.Item Is FBM.Constant Then
            Return DirectCast(arInput.Source.Item, FBM.Constant).Value
        End If

        Return "[ERROR: Unsupported CalculatedValue input.]"
    End Function

    Private Function FunctionHasBagInput(ByVal arModel As FBM.Model,
                                         ByVal arFunctionReference As FBM.FunctionRef) As Boolean
        If arModel Is Nothing OrElse arFunctionReference Is Nothing OrElse
           String.IsNullOrWhiteSpace(arFunctionReference.Ref) OrElse
           arModel.Function Is Nothing Then Return False

        Dim lrFunction As FBM.Function =
            arModel.Function.FirstOrDefault(
                Function(x) x IsNot Nothing AndAlso
                            String.Equals(x.id,
                                          arFunctionReference.Ref,
                                          StringComparison.OrdinalIgnoreCase))

        Return lrFunction IsNot Nothing AndAlso lrFunction.Parameters IsNot Nothing AndAlso
               lrFunction.Parameters.Any(Function(x) x IsNot Nothing AndAlso x.BagInput)
    End Function

    Private Function RenderPathedRoleValue(ByVal arContext As RenderContext,
                                           ByVal asPathedRoleId As String) As String
        Dim lrRole As FBM.Role = Nothing
        arContext.RoleByPathedRoleId.TryGetValue(asPathedRoleId, lrRole)
        If lrRole Is Nothing Then
            Return "[ERROR: PathedRole '" & CleanError(asPathedRoleId) & "' was not found.]"
        End If

        Return DisplayName(arContext,
                           CanonicalKey(arContext, PathedKey(asPathedRoleId)),
                           lrRole)
    End Function

    Private Function RenderPathRootValue(ByVal arContext As RenderContext,
                                         ByVal asPathRootId As String) As String
        If String.IsNullOrWhiteSpace(asPathRootId) Then
            Return "[ERROR: Missing PathRoot reference.]"
        End If

        Dim lrRoot As FBM.RootObjectType = Nothing
        If Not arContext.RootById.TryGetValue(asPathRootId, lrRoot) Then
            Return "[ERROR: PathRoot '" & CleanError(asPathRootId) & "' was not found.]"
        End If

        Dim lsRootName As String =
            DisplayName(arContext,
                        CanonicalKey(arContext, RootKey(asPathRootId)),
                        Nothing)
        If lsRootName = "" Then lsRootName = RootObjectTypeName(lrRoot)

        Return lsRootName
    End Function

    Private Function ResolveFunctionName(ByVal arModel As FBM.Model,
                                         ByVal arFunctionReference As FBM.FunctionRef) As String
        If arFunctionReference Is Nothing OrElse
           String.IsNullOrWhiteSpace(arFunctionReference.Ref) Then
            Return "[ERROR: Missing Function reference.]"
        End If

        If arModel.Function IsNot Nothing Then
            Dim lrFunction As FBM.Function =
                arModel.Function.FirstOrDefault(
                    Function(x) x IsNot Nothing AndAlso
                                String.Equals(x.id,
                                              arFunctionReference.Ref,
                                              StringComparison.OrdinalIgnoreCase))
            If lrFunction IsNot Nothing Then
                If Not String.IsNullOrWhiteSpace(lrFunction.OperatorSymbol) Then
                    Return lrFunction.OperatorSymbol.Trim()
                End If
                If Not String.IsNullOrWhiteSpace(lrFunction.Name) Then Return lrFunction.Name.Trim()
            End If
        End If

        Return "[ERROR: Function '" & CleanError(arFunctionReference.Ref) & "' was not found.]"
    End Function

    Private Function ComparisonOperator(ByVal arCondition As FBM.ConditionNode) As String
        If TypeOf arCondition Is FBM.ConditionEquals Then Return "="
        If TypeOf arCondition Is FBM.ConditionNotEquals Then Return "<>"
        If TypeOf arCondition Is FBM.ConditionGreaterThan Then Return ">"
        If TypeOf arCondition Is FBM.ConditionGreaterThanOrEqual Then Return ">="
        If TypeOf arCondition Is FBM.ConditionLessThan Then Return "<"
        If TypeOf arCondition Is FBM.ConditionLessThanOrEqual Then Return "<="
        Return "[ERROR: Unknown comparison operator.]"
    End Function

    Private Function IsInfixFunction(ByVal asFunctionName As String) As Boolean
        Select Case asFunctionName
            Case "=", "<>", ">", ">=", "<", "<=", "+", "-", "*", "/"
                Return True
        End Select
        Return False
    End Function

    Private Function IsArithmeticFunction(ByVal asFunctionName As String) As Boolean
        Select Case asFunctionName
            Case "+", "-", "*", "/"
                Return True
        End Select
        Return False
    End Function

    Private Function RenderUndevelopedFactType(ByVal arFactType As FBM.FactType) As String
        Dim lrReading As FBM.FactTypeReading = PreferredReading(arFactType)
        If lrReading Is Nothing Then
            Return "[ERROR: No Fact Type Reading exists for '" &
                   SafeFactTypeName(arFactType) & "']"
        End If
        Return lrReading.GetReadingText()
    End Function

    Private Function PreferredReading(ByVal arFactType As FBM.FactType) As FBM.FactTypeReading
        If arFactType Is Nothing OrElse arFactType.FactTypeReading Is Nothing Then Return Nothing

        Dim lrReading As FBM.FactTypeReading =
            arFactType.FactTypeReading.FirstOrDefault(
                Function(x) x IsNot Nothing AndAlso x.IsPreferred)
        If lrReading IsNot Nothing Then Return lrReading

        lrReading =
            arFactType.FactTypeReading.FirstOrDefault(
                Function(x) x IsNot Nothing AndAlso x.IsPreferredForPredicate)
        If lrReading IsNot Nothing Then Return lrReading

        Return arFactType.FactTypeReading.FirstOrDefault(Function(x) x IsNot Nothing)
    End Function

    Private Function ResolveRoleById(ByVal arModel As FBM.Model,
                                     ByVal asRoleId As String) As FBM.Role
        If arModel Is Nothing OrElse String.IsNullOrWhiteSpace(asRoleId) OrElse
           arModel.FactType Is Nothing Then Return Nothing

        For Each lrFactType As FBM.FactType In arModel.FactType
            If lrFactType Is Nothing OrElse lrFactType.RoleGroup Is Nothing Then Continue For
            For Each lrRole As FBM.Role In lrFactType.RoleGroup
                If lrRole IsNot Nothing AndAlso
                   String.Equals(lrRole.Id, asRoleId, StringComparison.OrdinalIgnoreCase) Then
                    Return lrRole
                End If
            Next
        Next
        Return Nothing
    End Function

    Private Function ResolveRoleInFactType(ByVal arFactType As FBM.FactType,
                                           ByVal asRoleId As String) As FBM.Role
        If arFactType Is Nothing OrElse arFactType.RoleGroup Is Nothing OrElse
           String.IsNullOrWhiteSpace(asRoleId) Then Return Nothing

        Return arFactType.RoleGroup.FirstOrDefault(
            Function(x) x IsNot Nothing AndAlso
                        String.Equals(x.Id, asRoleId, StringComparison.OrdinalIgnoreCase))
    End Function

    Private Function ResolvePathedRole(ByVal arContext As RenderContext,
                                       ByVal arPathedRole As FBM.PathedRole) As FBM.Role
        If arPathedRole Is Nothing OrElse String.IsNullOrWhiteSpace(arPathedRole.id) Then Return Nothing
        Dim lrRole As FBM.Role = Nothing
        arContext.RoleByPathedRoleId.TryGetValue(arPathedRole.id, lrRole)
        Return lrRole
    End Function

    Private Function ResolveSameFactTypeLinkRole(
        ByVal arCurrentOccurrence As FactOccurrence,
        ByVal arPathedRole As FBM.PathedRole,
        ByVal arResolvedRole As FBM.Role) As FBM.Role

        If arCurrentOccurrence Is Nothing OrElse
           arCurrentOccurrence.FactType Is Nothing OrElse
           Not arCurrentOccurrence.FactType.IsLinkFactType OrElse
           arCurrentOccurrence.FactType.LinkFactTypeRole Is Nothing OrElse
           arCurrentOccurrence.FactType.RoleGroup Is Nothing OrElse
           arPathedRole Is Nothing OrElse
           Not String.Equals(arPathedRole.Purpose,
                             "SameFactType",
                             StringComparison.OrdinalIgnoreCase) OrElse
           arResolvedRole Is Nothing OrElse
           Not SameRole(arCurrentOccurrence.FactType.LinkFactTypeRole,
                        arResolvedRole) Then
            Return arResolvedRole
        End If

        ' NORMA's implied objectification Fact Type contains a RoleProxy for
        ' an original role of the objectified Fact Type. Boston represents the
        ' same construct as a Link Fact Type whose LinkFactTypeRole points to
        ' that original role. For a SameFactType path step, use the corresponding
        ' role inside the active Link Fact Type so both PathedRoles are verbalised
        ' by one implied-Fact-Type reading.
        Dim larCorrespondingRoles As List(Of FBM.Role) =
            arCurrentOccurrence.FactType.RoleGroup.
                Where(
                    Function(x) x IsNot Nothing AndAlso
                                SameModelObject(x.JoinedORMObject,
                                                arResolvedRole.JoinedORMObject)).
                ToList()

        If larCorrespondingRoles.Count = 1 Then
            Return larCorrespondingRoles(0)
        End If

        Return arResolvedRole
    End Function

    Private Function SourceBindingKey(ByVal arSource As FBM.DerivationSource) As String
        If arSource Is Nothing Then Return ""
        If arSource.PathRoot IsNot Nothing Then Return RootKey(arSource.PathRoot.Ref)
        If arSource.PathedRole IsNot Nothing Then Return PathedKey(arSource.PathedRole.Ref)
        Return ""
    End Function

    Private Function SourceBindingObject(
        ByVal arContext As RenderContext,
        ByVal arSource As FBM.DerivationSource) As FBM.ModelObject

        If arSource Is Nothing Then Return Nothing

        If arSource.PathRoot IsNot Nothing Then
            Dim lrRoot As FBM.RootObjectType = Nothing
            If arContext.RootById.TryGetValue(arSource.PathRoot.Ref,
                                              lrRoot) AndAlso
               lrRoot IsNot Nothing Then
                Return lrRoot.BostonModelElement
            End If
        ElseIf arSource.PathedRole IsNot Nothing Then
            Dim lrPathedRole As FBM.PathedRole = Nothing
            If arContext.PathedRoleById.TryGetValue(
                arSource.PathedRole.Ref,
                lrPathedRole) Then
                Dim lrRole As FBM.Role =
                    ResolvePathedRole(arContext,
                                      lrPathedRole)
                If lrRole IsNot Nothing Then
                    Return lrRole.JoinedORMObject
                End If
            End If
        End If

        Return Nothing
    End Function

    Private Function SourceCorrelationRoot(
        ByVal arContext As RenderContext,
        ByVal arSource As FBM.DerivationSource) As Object

        If arSource Is Nothing Then Return Nothing

        If arSource.PathRoot IsNot Nothing Then
            Dim lrRoot As FBM.RootObjectType = Nothing
            If arContext.RootById.TryGetValue(arSource.PathRoot.Ref,
                                              lrRoot) Then
                Return lrRoot
            End If
        ElseIf arSource.PathedRole IsNot Nothing Then
            Dim lrPathedRole As FBM.PathedRole = Nothing
            If arContext.PathedRoleById.TryGetValue(
                arSource.PathedRole.Ref,
                lrPathedRole) Then
                Return lrPathedRole
            End If
        End If

        Return Nothing
    End Function

    Private Function DisplayName(ByVal arContext As RenderContext,
                                 ByVal asCanonicalKey As String,
                                 ByVal arFallbackRole As FBM.Role) As String
        Dim lsName As String = ""
        If asCanonicalKey <> "" Then arContext.DisplayNameByKey.TryGetValue(asCanonicalKey, lsName)
        If lsName = "" Then lsName = RolePlayerName(arFallbackRole)

        Dim liAlias As Integer = 0
        If asCanonicalKey <> "" AndAlso
           arContext.AliasNumberByKey.TryGetValue(asCanonicalKey, liAlias) Then
            lsName &= " " & liAlias.ToString()
        End If
        Return lsName
    End Function

    Private Function RootObjectTypeName(ByVal arRoot As FBM.RootObjectType) As String
        If arRoot Is Nothing Then Return ""
        If arRoot.BostonModelElement IsNot Nothing Then Return arRoot.BostonModelElement.Id
        Return arRoot.ref
    End Function

    Private Function RolePlayerName(ByVal arRole As FBM.Role) As String
        If arRole Is Nothing OrElse arRole.JoinedORMObject Is Nothing Then Return ""
        Return arRole.JoinedORMObject.Id
    End Function

    Private Function BoundRolePlayer(ByVal arPart As FBM.PredicatePart,
                                     ByVal asRolePlayerName As String) As String
        If arPart Is Nothing Then Return asRolePlayerName
        Return NormalisePreBoundText(arPart.PreBoundText) &
               If(asRolePlayerName, "") &
               NormalisePostBoundText(arPart.PostBoundText)
    End Function

    Private Function NormalisePreBoundText(ByVal asText As String) As String
        Dim lsText As String = If(asText, "")
        If lsText.EndsWith("-", StringComparison.Ordinal) Then
            lsText = lsText.Substring(0, lsText.Length - 1)
            If lsText <> "" AndAlso Not Char.IsWhiteSpace(lsText(lsText.Length - 1)) Then
                lsText &= " "
            End If
        End If
        Return lsText
    End Function

    Private Function NormalisePostBoundText(ByVal asText As String) As String
        Dim lsText As String = If(asText, "")
        If lsText.StartsWith("-", StringComparison.Ordinal) Then
            lsText = lsText.Substring(1)
            If lsText <> "" AndAlso Not Char.IsWhiteSpace(lsText(0)) Then
                lsText = " " & lsText
            End If
        End If
        Return lsText
    End Function

    Private Function RoleSequenceText(ByVal aarRoles As List(Of FBM.Role)) As String
        If aarRoles Is Nothing Then Return "[]"
        Return "[" &
               String.Join(", ",
                           aarRoles.Select(
                               Function(x) If(x Is Nothing,
                                              "(missing)",
                                              If(x.Id, "(no id)")))) &
               "]"
    End Function

    Private Function SafePredicateParts(ByVal arReading As FBM.FactTypeReading) As List(Of FBM.PredicatePart)
        If arReading Is Nothing OrElse arReading.PredicatePart Is Nothing Then
            Return New List(Of FBM.PredicatePart)
        End If
        Return arReading.PredicatePart
    End Function

    Private Function SafeFactTypeName(ByVal arFactType As FBM.FactType) As String
        If arFactType Is Nothing OrElse String.IsNullOrWhiteSpace(arFactType.Name) Then
            Return "(unnamed Fact Type)"
        End If
        Return arFactType.Name
    End Function

    Private Sub AppendRaw(ByVal arBuilder As StringBuilder,
                          ByVal asText As String)
        If String.IsNullOrEmpty(asText) Then Exit Sub
        arBuilder.Append(asText)
    End Sub

    Private Sub AppendWord(ByVal arBuilder As StringBuilder,
                           ByVal asText As String)
        If String.IsNullOrWhiteSpace(asText) Then Exit Sub
        Dim lsText As String = asText.Trim()
        If arBuilder.Length > 0 AndAlso
           arBuilder(arBuilder.Length - 1) <> "("c AndAlso
           Not arBuilder.ToString().EndsWith(HtmlBreak, StringComparison.OrdinalIgnoreCase) AndAlso
           Not Char.IsWhiteSpace(arBuilder(arBuilder.Length - 1)) Then
            arBuilder.Append(" ")
        End If
        arBuilder.Append(lsText)
    End Sub

    Private Function CleanError(ByVal asText As String) As String
        Return If(asText, "").Replace(vbCr, " ").Replace(vbLf, " ").Trim()
    End Function

    Private Function RootKey(ByVal asId As String) As String
        If String.IsNullOrWhiteSpace(asId) Then Return ""
        Return "ROOT:" & asId
    End Function

    Private Function HeadRoleKey(ByVal asRoleId As String) As String
        If String.IsNullOrWhiteSpace(asRoleId) Then Return ""
        Return "HEADROLE:" & asRoleId
    End Function

    Private Function ProjectionTypeKey(ByVal asRolePathId As String,
                                       ByVal asRoleId As String) As String
        If String.IsNullOrWhiteSpace(asRolePathId) OrElse
           String.IsNullOrWhiteSpace(asRoleId) Then Return ""
        Return "PROJECTIONTYPE:" & asRolePathId & ":" & asRoleId
    End Function

    Private Function PathedKey(ByVal asId As String) As String
        If String.IsNullOrWhiteSpace(asId) Then Return ""
        Return "PATHED:" & asId
    End Function

    Private Sub EnsureKey(ByVal arContext As RenderContext,
                          ByVal asKey As String)
        If asKey = "" Then Exit Sub
        If Not arContext.ParentByKey.ContainsKey(asKey) Then
            arContext.ParentByKey(asKey) = asKey
        End If
    End Sub

    Private Function CanonicalKey(ByVal arContext As RenderContext,
                                  ByVal asKey As String) As String
        If asKey = "" Then Return ""
        EnsureKey(arContext, asKey)

        Dim lsParent As String = arContext.ParentByKey(asKey)
        If String.Equals(lsParent, asKey, StringComparison.OrdinalIgnoreCase) Then Return asKey

        lsParent = CanonicalKey(arContext, lsParent)
        arContext.ParentByKey(asKey) = lsParent
        Return lsParent
    End Function

    Private Sub UnionKeys(ByVal arContext As RenderContext,
                          ByVal asLeftKey As String,
                          ByVal asRightKey As String)
        If asLeftKey = "" OrElse asRightKey = "" Then Exit Sub
        Dim lsLeft As String = CanonicalKey(arContext, asLeftKey)
        Dim lsRight As String = CanonicalKey(arContext, asRightKey)
        If Not String.Equals(lsLeft, lsRight, StringComparison.OrdinalIgnoreCase) Then
            arContext.ParentByKey(lsRight) = lsLeft
        End If
    End Sub

End Module
