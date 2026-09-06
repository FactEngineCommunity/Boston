Imports System
Imports System.Text
Imports System.IO
Imports System.Collections.Generic
Imports System.Linq
Imports System.Runtime.CompilerServices
Imports System.Reflection

Public Class CLIFGenerator

    ' ===== Class-level fields (NO prefixes) =====
    Private ReadOnly Model As FBM.Model
    Private ReadOnly Builder As New StringBuilder(64 * 1024)

    Public Sub New(aModel As FBM.Model)
        If aModel Is Nothing Then Throw New ArgumentNullException(NameOf(aModel))
        Model = aModel
    End Sub

    Public Function GenerateCLIF(Optional ByRef arModelElement As FBM.ModelObject = Nothing) As String

        Builder.Clear()
        Builder.AppendLine("(cl-text")
        Builder.AppendLine("")

        EmitObjectTypes(arModelElement)
        EmitSubtypes(arModelElement)
        EmitSimpleIdentifications(arModelElement)
        EmitAttributesAndFrequencies(arModelElement)
        'EmitBinaryAndOtherFacts(arModelElement) '20250831-VM-Complete or scavange from this method.
        ' Uniqueness (internal) and other RoleConstraints that weren’t covered yet
        EmitRemainingRoleConstraints(arModelElement)

        Builder.AppendLine(")")

        Return Builder.ToString()
    End Function

    Public Sub GenerateCLIF(ByVal asFilePathName As String)
        If String.IsNullOrWhiteSpace(asFilePathName) Then Throw New ArgumentException("Path required.", NameOf(asFilePathName))

        Builder.Clear()
        Builder.AppendLine("(cl-text")

        EmitSubtypes()
        EmitSimpleIdentifications()
        EmitAttributesAndFrequencies()
        'EmitBinaryAndOtherFactTypes() '20250831-VM-Complete or scavange from this method.
        ' Uniqueness (internal) and other RoleConstraints that weren’t covered yet
        EmitRemainingRoleConstraints()

        Builder.AppendLine(")")

        Dim lDir As String = Path.GetDirectoryName(asFilePathName)
        If Not String.IsNullOrWhiteSpace(lDir) AndAlso Not Directory.Exists(lDir) Then Directory.CreateDirectory(lDir)
        File.WriteAllText(asFilePathName, Builder.ToString(), Encoding.UTF8)
    End Sub

    ' ======================================================
    ' =============== Tiny CLIF builder helpers ============
    ' ======================================================

    Private Function Symbol(aName As String) As String
        If String.IsNullOrWhiteSpace(aName) Then Throw New ArgumentException("Empty symbol", NameOf(aName))
        Dim lSafe As String = New String(aName.Select(Function(c) If(Char.IsLetterOrDigit(c) OrElse c = "_"c OrElse c = "-"c, c, "_"c)).ToArray())
        If lSafe.StartsWith(":"c) Then Return lSafe
        Return ":" & lSafe
    End Function

    Private Function Variable(aName As String) As String
        If String.IsNullOrWhiteSpace(aName) Then Throw New ArgumentException("Empty var", NameOf(aName))
        Dim lSafe As String = New String(aName.Select(Function(c) If(Char.IsLetterOrDigit(c) OrElse c = "_"c, c, "_"c)).ToArray())
        If lSafe.StartsWith("?"c) Then Return lSafe
        Return "?" & lSafe
    End Function

    Private Function StringLiteral(aValue As String) As String
        Dim lText As String = If(aValue, String.Empty)
        lText = lText.Replace("\", "\\")
        lText = lText.Replace("""", "\""")
        Return """" & lText & """"
    End Function

    Private Function Atom(aPred As String, ParamArray arArgs() As String) As String
        Dim lSb As New StringBuilder()
        lSb.Append("("c).Append(aPred)
        For Each lA In arArgs
            lSb.Append(" "c).Append(lA)
        Next
        lSb.Append(")"c)
        Return lSb.ToString()
    End Function

    Private Function AndF(ParamArray arConjuncts() As String) As String
        If arConjuncts Is Nothing OrElse arConjuncts.Length = 0 Then Return ""
        If arConjuncts.Length = 1 Then Return arConjuncts(0)
        Return "(and " & String.Join(" ", arConjuncts) & ")"
    End Function

    Private Function ImpliesF(aAntecedent As String, aConsequent As String) As String
        Return "(=> " & aAntecedent & " " & aConsequent & ")"
    End Function

    Private Function ForallF(arVars As IEnumerable(Of String), aBody As String) As String
        Return "(forall (" & String.Join(" ", arVars) & ") " & aBody & ")"
    End Function

    Private Function ExistsF(arVars As IEnumerable(Of String), aBody As String) As String
        Return "(exists (" & String.Join(" ", arVars) & ") " & aBody & ")"
    End Function

    Private Function EqF(aLeft As String, aRight As String) As String
        Return "(= " & aLeft & " " & aRight & ")"
    End Function

    Private Sub EmitObjectTypes(Optional ByRef arModelElement As FBM.ModelObject = Nothing)

        Try
            Builder.AppendLine("; ==== Object Types ====")

            Dim lrModelElement = arModelElement

            Dim larModelElement As IEnumerable(Of Object)

            If arModelElement Is Nothing Then

                larModelElement = (From ModelElement In Model.getModelObjects(False).FindAll(Function(x) Not x.IsMDAModelElement)
                                   Where {GetType(FBM.EntityType), GetType(FBM.FactType)}.Contains(ModelElement.GetType)
                                   Select ModelElement).ToList

            Else

                larModelElement = (From ModelElement In Model.getModelObjects(False).FindAll(Function(x) Not x.IsMDAModelElement)
                                   Where ModelElement.Id = lrModelElement.Id
                                   Select ModelElement).ToList
            End If

            If larModelElement.Count = 0 Then
                Builder.AppendLine("; there are none.")
                Builder.AppendLine()
                Exit Sub
            End If

            For Each lrModelElement In larModelElement
                Builder.AppendLine($"; {lrModelElement.Id}")

                Select Case lrModelElement.GetType
                    Case Is = GetType(FBM.EntityType)
#Region "Entity Type"
                        Dim lrEntityType As FBM.EntityType = DirectCast(lrModelElement, FBM.EntityType)

                        ' Reified type constant and declaration: (EntityType tt:<Id>)
                        Dim lTypeTok As String = "tt:" & lrEntityType.Id
                        Builder.AppendLine(Atom("EntityType", lTypeTok))

                        ' Bridge between predicate et:<Id> and reified type via inst_of (no instances implied)
                        Dim lx As String = Variable("x")
                        Builder.AppendLine(ForallF({lx}, ImpliesF(Atom("et:" & lrEntityType.Id, lx), Atom("inst_of", lx, lTypeTok))))
                        Builder.AppendLine(ForallF({lx}, ImpliesF(Atom("inst_of", lx, lTypeTok), Atom("et:" & lrEntityType.Id, lx))))
#End Region
                    Case Is = GetType(FBM.FactType)
#Region "Objectified Fact Type"
                        Dim lFT As FBM.FactType = DirectCast(lrModelElement, FBM.FactType)
                        If Not lFT.IsObjectified Then Exit Select

                        Dim lObj As FBM.EntityType = lFT.ObjectifyingEntityType
                        If lObj Is Nothing Then Exit Select

                        ' Roles in stable order
                        Dim larRoles = lFT.RoleGroup.OrderBy(Function(r) r.SequenceNr).ToList()
                        If larRoles.Count = 0 Then Exit Select

                        ' Variables for the tuple (x1, x2, ..., xn)
                        Dim larVars As New List(Of String)
                        For i = 0 To larRoles.Count - 1
                            larVars.Add(Variable("x" & (i + 1).ToString()))
                        Next

                        ' Type atoms for each role player (vt: / et: decided by JoinedORMObject type)
                        Dim larTypeAtoms As New List(Of String)
                        For i = 0 To larRoles.Count - 1
                            Dim lJoined = larRoles(i).JoinedORMObject
                            If lJoined Is Nothing Then Continue For
                            Dim lPrefix As String = If(TypeOf lJoined Is FBM.ValueType, "vt:", "et:")
                            larTypeAtoms.Add(Atom(lPrefix & lJoined.Id, larVars(i)))
                        Next

                        ' Predicate names
                        Dim lUnderlying As String = lFT.Name.ToSnakeCase()                    ' underlying n-ary fact (e.g., showing)
                        Dim lBridge As String = lObj.Name.ToSnakeCase() & "_of"               ' bridge (e.g., session_of)
                        Dim lS As String = Variable("s")                                      ' object variable

                        ' typing for the underlying predicate
                        Builder.AppendLine(
                            ForallF(larVars.ToArray(),
                                ImpliesF(Atom(lUnderlying, larVars.ToArray()), AndF(larTypeAtoms.ToArray()))
                            )
                        )

                        ' each Session (objectified entity) is associated with some tuple (IS WHERE direction optional but included)
                        Builder.AppendLine(
                            ForallF({lS},
                                ImpliesF(
                                    Atom("et:" & lObj.Id, lS),
                                    ExistsF(larVars.ToArray(),
                                        AndF(
                                            Atom(lUnderlying, larVars.ToArray()),
                                            Atom(lBridge, (New List(Of String) From {lS}).Concat(larVars).ToArray())
                                        )
                                    )
                                )
                            )
                        )

                        ' every underlying tuple yields at least one Session (IS WHERE)
                        Builder.AppendLine(
                            ForallF(larVars.ToArray(),
                                ImpliesF(
                                    Atom(lUnderlying, larVars.ToArray()),
                                    ExistsF({lS},
                                        AndF(
                                            Atom("et:" & lObj.Id, lS),
                                            Atom(lBridge, (New List(Of String) From {lS}).Concat(larVars).ToArray())
                                        )
                                    )
                                )
                            )
                        )

                        ' typing for the bridge predicate
                        Dim larBridgeTyping As New List(Of String)
                        larBridgeTyping.Add(Atom("et:" & lObj.Id, lS))
                        larBridgeTyping.AddRange(larTypeAtoms)
                        Builder.AppendLine(
                            ForallF((New List(Of String) From {lS}).Concat(larVars).ToArray(),
                                ImpliesF(
                                    Atom(lBridge, (New List(Of String) From {lS}).Concat(larVars).ToArray()),
                                    AndF(larBridgeTyping.ToArray())
                                )
                            )
                        )
#End Region
                End Select
            Next

            Builder.AppendLine()

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try
    End Sub


    ' --- Subtypes: Teenager/Child/Adult ⊆ Person ---
    Private Sub EmitSubtypes(Optional ByRef arModelElement As FBM.ModelObject = Nothing)

        Try
            Builder.AppendLine("; ==== Subtypes ====")

            Dim lrModelElement = arModelElement

            Dim larModelElement As IEnumerable(Of Object)

            If arModelElement Is Nothing Then

                larModelElement = (From ModelElement In Model.getModelObjects(False)
                                   From Subtype In ModelElement.HasSubtype
                                   Select New With {ModelElement, Subtype}).ToList

            Else

                larModelElement = (From ModelElement In Model.getModelObjects(False)
                                   From Subtype In ModelElement.HasSubtype
                                   Where (ModelElement.Id = lrModelElement.Id Or Subtype.Id = lrModelElement.Id)
                                   Select New With {ModelElement, Subtype}).ToList
            End If

            If larModelElement.Count = 0 Then
                Builder.AppendLine("; there are none.")
                Builder.AppendLine()
                Exit Sub
            End If


            For Each lrSubtypeRelationship In larModelElement
                Builder.AppendLine(ForallF({Variable("x")}, ImpliesF(Atom($"et:{lrSubtypeRelationship.Subtype.Id}", Variable("x")), Atom($"et:{lrSubtypeRelationship.ModelElement.Id}", Variable("x")))))
            Next
            Builder.AppendLine()

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try
    End Sub

    ' --- Simple value identifications: Address/Adress/Cinema/Film/Person by Id; Section by Section_Name ---
    Private Sub EmitSimpleIdentifications(Optional ByRef arModelElement As FBM.ModelObject = Nothing)

        Try
            Builder.AppendLine("; ==== Unique Identifiers (Singular) ====")

            Dim larModelElement As New List(Of FBM.ModelObject)

            If arModelElement Is Nothing Then
                larModelElement = Model.getModelObjects(False).FindAll(Function(x) Not x.IsMDAModelElement)

                If larModelElement.Count = 0 Then
                    Builder.AppendLine("; there are none.")
                    Builder.AppendLine()
                    Exit Sub
                End If
            Else
                larModelElement.Add(arModelElement)
            End If

            For Each lrModelElement In larModelElement

                Select Case lrModelElement.GetType
                    Case Is = GetType(FBM.EntityType)

                        Dim lrEntityType = CType(lrModelElement, FBM.EntityType)

                        If lrEntityType.HasSimpleReferenceScheme Then
                            EmitIdByValue($"{lrModelElement.Id.ToPascalCase}", $"{lrEntityType.GetTopmostSupertype.ReferenceModeValueType.Id.ToPascalCase}", $"id:{lrEntityType.Id.ToPascalCase}")

                            ' add inverse uniqueness: shared id value implies same entity
                            Dim lsIdPred As String = $"id:{lrEntityType.Id.ToPascalCase}"
                            Dim lE1 As String = Variable("e1")
                            Dim lE2 As String = Variable("e2")
                            Dim lV As String = Variable("v")
                            Builder.AppendLine(
                                ForallF({lE1, lE2, lV},
                                    ImpliesF(
                                        AndF(Atom(lsIdPred, lE1, lV), Atom(lsIdPred, lE2, lV)),
                                        EqF(lE1, lE2)
                                    )
                                )
                            )
                            Builder.AppendLine()
                        End If

                End Select
            Next

            Builder.AppendLine()

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try
    End Sub

    Private Sub EmitIdByValue(aEntityName As String, aValueTypeName As String, aAttrPred As String)

        Try

            Dim lE As String = Variable("e")
            Dim lV As String = Variable("v")
            Dim lsEt As String = "et:" & aEntityName
            Dim lVt As String = "vt:" & aValueTypeName

            ' Total: every entity has some id
            Builder.AppendLine(ForallF({lE}, ImpliesF(Atom(lsEt, lE), ExistsF({lV}, AndF(Atom(lVt, lV), Atom(aAttrPred, lE, lV))))))

            ' Functional: at most one id per entity
            Dim lV1 As String = Variable("v1")
            Dim lV2 As String = Variable("v2")
            Builder.AppendLine(ForallF({lE, lV1, lV2}, ImpliesF(AndF(Atom(aAttrPred, lE, lV1), Atom(aAttrPred, lE, lV2)), EqF(lV1, lV2))))

            ' Typing
            Builder.AppendLine(ForallF({lE, lV}, ImpliesF(Atom(aAttrPred, lE, lV), AndF(Atom(lsEt, lE), Atom(lVt, lV)))))

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try
    End Sub

    ' --- Attributes and frequencies (ONE / AT MOST ONE / AT LEAST ONE) ---
    Private Sub EmitAttributesAndFrequencies(Optional ByRef arModelElement As FBM.ModelObject = Nothing)

        Builder.AppendLine("; ==== Properties ====")

        Try
            Dim lrModelElement = arModelElement
            Dim larPropertyFactTypes As IEnumerable(Of Object)

            If arModelElement Is Nothing Then
                larPropertyFactTypes = (From Table In Model.RDS.Table
                                        From Column In Table.Column
                                        Select New With {Table.FBMModelElement, Column.FactType, Column}).ToList
            Else
                larPropertyFactTypes = (From Table In Model.RDS.Table
                                        From Column In Table.Column
                                        Where Table.FBMModelElement.Id = lrModelElement.Id
                                        Select New With {Table.FBMModelElement, Column.FactType, Column}).ToList
            End If

            If larPropertyFactTypes.Count = 0 Then
                Builder.AppendLine("; there are none.")
                Builder.AppendLine()
                Exit Sub
            End If

            For Each lrPropertyFactType In larPropertyFactTypes

                If lrPropertyFactType.FactType Is Nothing Then
                    Builder.AppendLine($"; Error: {lrPropertyFactType.FBMModelElement.Id} has Column: {lrPropertyFactType.Column.Name} without a Fact Type.")
                    Continue For
                End If

                Dim lrResponsibleRole = CType(lrPropertyFactType.Column.Role, FBM.Role)
                Dim lrActiveRole = CType(lrPropertyFactType.Column.ActiveRole, FBM.Role)

                If (lrPropertyFactType.FactType.IsManyTo1BinaryFactType Or lrPropertyFactType.FactType.Is1to1BinaryFactType) And Not lrPropertyFactType.FactType.IsObjectified Then

                    If lrResponsibleRole.HasInternalUniquenessConstraint Then

                        If lrResponsibleRole.Mandatory Then
                            'e.g. Booking has ONE SeatCount
                            EmitOneAttribute($"{lrResponsibleRole.JoinedORMObject.Id}", $"{lrActiveRole.JoinedORMObject.Id}", lrActiveRole.JoinedORMObject.Id.ToSnakeCase)
                            '20250831-VM-From ChatGPT v5, given that we define the unique identifier (id:) in another method
                            'One caveat: since you also have the identifier id:Person, you should equate them or you’ll force two possibly different Person_Id values per person. Add:
                            '(forall (?x ?v) (=> (idPerson ?x ?v) (person_id ?x ?v)))
                            '(forall (?x ?v) (=> (person_id ?x ?v) (id:Person ?x ?v)))
                        Else
                            'e.g. Film has AT MOST ONE Film Name
                            EmitFunctionalAttribute($"{lrResponsibleRole.JoinedORMObject.Id}", $"{lrActiveRole.JoinedORMObject.Id}", lrActiveRole.JoinedORMObject.Id.ToSnakeCase)
                        End If

                        Builder.AppendLine()
                        Builder.AppendLine()

                    ElseIf lrActiveRole.HasInternalUniquenessConstraint Then

                    End If

                ElseIf lrPropertyFactType.FactType.IsObjectified And lrPropertyFactType.FBMModelElement.GetType = GetType(FBM.FactType) Then
#Region "Objectified Fact Type"
                    If lrPropertyFactType.FactType.Id = lrPropertyFactType.FBMModelElement.Id Then

                        'e.g. Booking has ONE SeatCount
                        EmitOneAttribute($"{lrResponsibleRole.JoinedORMObject.Id}", $"{lrActiveRole.JoinedORMObject.Id}", lrActiveRole.JoinedORMObject.Id.ToSnakeCase)

                        Builder.AppendLine()
                        Builder.AppendLine()

                    Else
                        If lrPropertyFactType.FactType.IsManyTo1BinaryFactType Then
                            If lrResponsibleRole.HasInternalUniquenessConstraint Then

                                If lrResponsibleRole.Mandatory Then
                                    'e.g. Booking has ONE SeatCount
                                    EmitOneAttribute($"{lrResponsibleRole.JoinedORMObject.Id}", $"{lrActiveRole.JoinedORMObject.Id}", lrActiveRole.JoinedORMObject.Id.ToSnakeCase)
                                Else
                                    'e.g. Film has AT MOST ONE Film Name
                                    EmitFunctionalAttribute($"{lrResponsibleRole.JoinedORMObject.Id}", $"{lrActiveRole.JoinedORMObject.Id}", lrActiveRole.JoinedORMObject.Id.ToSnakeCase)
                                End If

                                Builder.AppendLine()
                                Builder.AppendLine()

                            ElseIf lrActiveRole.HasInternalUniquenessConstraint Then

                            End If

                        End If
                    End If
#End Region
                End If
            Next

            '' Cinema has AT MOST ONE CinemaName
            'EmitFunctionalAttribute("Cinema", "CinemaName", "cinema_name")
            'Builder.AppendLine()

            '' Booking has ONE SeatCount
            'EmitOneAttribute("Booking", "SeatCount", "seat_count")
            'Builder.AppendLine()

            Builder.AppendLine()

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try
    End Sub

    Private Sub EmitFunctionalAttribute(aEntity As String, aValueType As String, aPred As String)
        Dim lX As String = Variable("x")
        Dim lV As String = Variable("v")
        Dim lV1 As String = Variable("v1")
        Dim lV2 As String = Variable("v2")
        Dim lsEt As String = "et:" & aEntity
        Dim lVt As String = "vt:" & aValueType

        ' typing
        Builder.AppendLine(ForallF({lX, lV}, ImpliesF(Atom(aPred, lX, lV), AndF(Atom(lsEt, lX), Atom(lVt, lV)))))
        ' functionality
        Builder.AppendLine(ForallF({lX, lV1, lV2}, ImpliesF(AndF(Atom(aPred, lX, lV1), Atom(aPred, lX, lV2)), EqF(lV1, lV2))))
    End Sub

    Private Sub EmitOneAttribute(aEntity As String, aValueType As String, aPred As String)
        Dim lsX As String = Variable("x")
        Dim lsV As String = Variable("v")
        Dim lsV1 As String = Variable("v1")
        Dim lsV2 As String = Variable("v2")
        Dim lsEt As String = "et:" & aEntity
        Dim lVt As String = "vt:" & aValueType

        ' typing
        Builder.AppendLine(ForallF({lsX, lsV}, ImpliesF(Atom(aPred, lsX, lsV), AndF(Atom(lsEt, lsX), Atom(lVt, lsV)))))
        ' totality
        Builder.AppendLine(ForallF({lsX}, ImpliesF(Atom(lsEt, lsX), ExistsF({lsV}, AndF(Atom(lVt, lsV), Atom(aPred, lsX, lsV))))))
        ' functionality
        Builder.AppendLine(ForallF({lsX, lsV1, lsV2}, ImpliesF(AndF(Atom(aPred, lsX, lsV1), Atom(aPred, lsX, lsV2)), EqF(lsV1, lsV2))))
    End Sub

    Private Sub EmitOneEntityRef(aSrcEntity As String, aTgtEntity As String, aPred As String)
        Dim lS As String = Variable("s")
        Dim lT As String = Variable("t")
        Dim lT1 As String = Variable("t1")
        Dim lT2 As String = Variable("t2")

        ' typing
        Builder.AppendLine(ForallF({lS, lT}, ImpliesF(Atom(aPred, lS, lT), AndF(Atom("et:" & aSrcEntity, lS), Atom("et:" & aTgtEntity, lT)))))
        ' totality
        Builder.AppendLine(ForallF({lS}, ImpliesF(Atom("et:" & aSrcEntity, lS), ExistsF({lT}, AndF(Atom("et:" & aTgtEntity, lT), Atom(aPred, lS, lT))))))
        ' functionality (at most one)
        Builder.AppendLine(ForallF({lS, lT1, lT2}, ImpliesF(AndF(Atom(aPred, lS, lT1), Atom(aPred, lS, lT2)), EqF(lT1, lT2))))
    End Sub

    ' --- Binary/other fact types directly from the narrative ---
    Private Sub EmitBinaryAndOtherFactTypes(Optional ByRef arModelElement As FBM.ModelObject = Nothing)

        ' Cinema contains AT LEAST ONE Row
        Dim lC As String = Variable("c")
        Dim lR As String = Variable("r")
        Builder.AppendLine(ForallF({lC}, ImpliesF(Atom("et:Cinema", lC), ExistsF({lR}, AndF(Atom("et:Row", lR), Atom("contains", lC, lR))))))

        ' PersonLikesFilm has AT MOST ONE Rating (attribute on the objectified relation)
        Dim lR1 As String = Variable("r1")
        Dim lR2 As String = Variable("r2")
        Dim lPlf As String = Variable("plf")
        Builder.AppendLine(ForallF({lPlf, lR1, lR2}, ImpliesF(AndF(Atom("plf_rating", lPlf, lR1), Atom("plf_rating", lPlf, lR2)), EqF(lR1, lR2))))
        Builder.AppendLine(ForallF({lPlf, lR1}, ImpliesF(Atom("plf_rating", lPlf, lR1), AndF(Atom("et:PersonLikesFilm", lPlf), Atom("vt:Rating", lR1)))))

        ' Booking is confirmed (unary property)
        Dim lB As String = Variable("b")
        Builder.AppendLine(ForallF({lB}, ImpliesF(Atom("et:Booking", lB), Atom("confirmed", lB))))
        Builder.AppendLine()
    End Sub


    ' --- Remaining role constraints (e.g., IU not already covered directly) ---
    Private Sub EmitRemainingRoleConstraints(Optional ByRef arModelElement As FBM.ModelObject = Nothing)

        Try

            If Model Is Nothing OrElse Model.RoleConstraint Is Nothing Then Return

            Builder.AppendLine("; ==== Role Constraints ====")

            Dim lrModelElement = arModelElement
            Dim larRoleConstraint As New List(Of FBM.RoleConstraint)

            If arModelElement Is Nothing Then
                larRoleConstraint = Model.RoleConstraint.FindAll(Function(x) Not x.IsMDAModelElement)
            Else
                larRoleConstraint = (From RoleConstraint In Model.RoleConstraint
                                     From Role In RoleConstraint.Role
                                     Where Role.JoinedORMObject.Id = lrModelElement.Id
                                     Select RoleConstraint).ToList
            End If

            If larRoleConstraint.Count = 0 Then
                Builder.AppendLine("; there are none.")
                Builder.AppendLine()
                Exit Sub
            End If

            If lrModelElement IsNot Nothing AndAlso lrModelElement.GetType = GetType(FBM.FactType) Then
                larRoleConstraint.AddRange(CType(lrModelElement, FBM.FactType).InternalUniquenessConstraint)
            End If

            For Each lRc In larRoleConstraint

                If lRc Is Nothing OrElse lRc.FirstRoleConstraintRole Is Nothing Then Continue For

                If lRc.RoleConstraintType = pcenumRoleConstraintType.InternalUniquenessConstraint Then

                    Dim lFt As FBM.FactType = lRc.FirstRoleConstraintRole.FactType
                    If lFt Is Nothing OrElse lFt.RoleGroup Is Nothing OrElse lRc.Role Is Nothing Then Continue For
                    EmitIUConstraint(lRc, GetFactPredName(lFt, lRc.Role), lFt.RoleGroup, lRc.Role)

                End If
            Next

        Catch ex As Exception

        End Try
    End Sub

    ' Emit general IU: two tuples agreeing on key roles must agree on those positions
    Private Sub EmitIUConstraint(ByRef arRoleConstraint As FBM.RoleConstraint, ByVal aRelName As String, arAllRoles As List(Of FBM.Role), aarKeyRoles As List(Of FBM.Role))
        If arAllRoles Is Nothing OrElse arAllRoles.Count = 0 Then Exit Sub
        If aarKeyRoles Is Nothing Then aarKeyRoles = New List(Of FBM.Role)

        Try

            If aarKeyRoles.Count = 1 Then
#Region "arKeyRoles.Count =1"
                Builder.AppendLine($"; {arRoleConstraint.Id}")

                ' Treat incoming list as KEY (not NON-KEY)
                Dim keyIdx As Integer = Enumerable.Range(0, 2).First(Function(i) aarKeyRoles.Contains(arAllRoles(i)))
                Dim nonIdx As Integer = 1 - keyIdx

                ' Build two tuples x..., y...
                Dim larX As New List(Of String)
                Dim larY As New List(Of String)
                For i = 0 To arAllRoles.Count - 1
                    Dim base As String = arAllRoles(i).JoinedORMObject.Id.ToSnakeCase()
                    larX.Add(Variable("x_" & base & "_" & (i + 1)))
                    larY.Add(Variable("y_" & base & "_" & (i + 1)))
                Next

                ' Antecedent: R(x...) ∧ R(y...) ∧ (= x_key y_key)
                Dim antecedent As String = AndF(
                    Atom(aRelName, larX.ToArray()),
                    Atom(aRelName, larY.ToArray()),
                    EqF(larX(keyIdx), larY(keyIdx))
                )

                ' Consequent: (= x_non y_non)
                Dim consequent As String = EqF(larX(nonIdx), larY(nonIdx))

                Dim allVars As IEnumerable(Of String) = larX.Concat(larY)
                Builder.AppendLine(ForallF(allVars, ImpliesF(antecedent, consequent)))
                Builder.AppendLine()
#End Region
            ElseIf aarKeyRoles.Count = 2 Then
#Region "arKeyRoles.Count <=2"
                Builder.AppendLine($"; {arRoleConstraint.Id}")
                ' ---- Interpret incoming list as NON-KEY and flip to get KEY ----
                Dim lListed As New HashSet(Of FBM.Role)(aarKeyRoles) ' likely NON-KEY roles in your data
                Dim lKeySet As New HashSet(Of FBM.Role)(arAllRoles.Where(Function(r) Not lListed.Contains(r)))
                Dim lNonKeySet As New HashSet(Of FBM.Role)(arAllRoles.Where(Function(r) Not lKeySet.Contains(r)))

                ' Degenerate: nothing to constrain
                'If lKeySet.Count = 0 OrElse lNonKeySet.Count = 0 Then Exit Sub 'Was getting rid of Many-to-Many's

                ' ---- Build DISTINCT variable names for two tuples ----
                Dim larX As New List(Of String)
                Dim larY As New List(Of String)
                For i = 0 To arAllRoles.Count - 1
                    Dim lBase As String = arAllRoles(i).JoinedORMObject.Id.ToSnakeCase()
                    larX.Add(Variable("x_" & lBase & "_" & (i + 1)))
                    larY.Add(Variable("y_" & lBase & "_" & (i + 1)))
                Next

                ' ---- Antecedent: R(x...) ∧ R(y...) ∧ ∧_{k∈KEY} (x_k = y_k) ----
                Dim larConj As New List(Of String) From {
                                                            Atom(aRelName, larX.ToArray()),
                                                            Atom(aRelName, larY.ToArray())
                                                        }
                For i = 0 To arAllRoles.Count - 1
                    If lKeySet.Contains(arAllRoles(i)) Then
                        larConj.Add(EqF(larX(i), larY(i)))
                    End If
                Next
                Dim lAnte As String = AndF(larConj.ToArray())

                ' ---- Consequent: ∧_{j∉KEY} (x_j = y_j)  (NON-KEY must match) ----
                Dim larEqNonKey As New List(Of String)
                For i = 0 To arAllRoles.Count - 1
                    If lNonKeySet.Contains(arAllRoles(i)) Then
                        larEqNonKey.Add(EqF(larX(i), larY(i)))
                    End If
                Next
                If larEqNonKey.Count = 0 Then Exit Sub

                Dim lCons As String = If(larEqNonKey.Count = 1, larEqNonKey(0), AndF(larEqNonKey.ToArray()))
                Dim larAllVars As IEnumerable(Of String) = larX.Concat(larY)

                Builder.AppendLine(ForallF(larAllVars, ImpliesF(lAnte, lCons)))

                If aarKeyRoles(0).FactType.Is1To1BinaryFactType Then
#Region "Inverse reading"
                    If aarKeyRoles(0).FactType.Is1To1BinaryFactType Then
                        ' Choose alternate surface reading (e.g., "has") if available
                        Dim ft = aarKeyRoles(0).FactType
                        Dim invRC = ft.InternalUniquenessConstraint.FirstOrDefault(Function(rc) Not aarKeyRoles.Contains(rc.Role(0)))
                        Dim lasPredicatePartText =
                            (From fr In ft.FactTypeReading
                             From pp In fr.PredicatePart
                             Where pp.SequenceNr = 1 AndAlso (invRC Is Nothing OrElse pp.Role.Id <> invRC.Role(0).Id)
                             Select pp.PredicatePartText).ToList()

                        Dim lsAltReading As String = aRelName

                        If lasPredicatePartText.Count > 0 Then
                            lsAltReading = lasPredicatePartText.First().ToSnakeCase()
                        End If

                        ' --- Force (VALUE, ENTITY) order for the inverse antecedent atoms ---
                        Dim idxVal As Integer = arAllRoles.FindIndex(Function(r) TypeOf r.JoinedORMObject Is FBM.ValueType)
                        Dim idxEnt As Integer = arAllRoles.FindIndex(Function(r) TypeOf r.JoinedORMObject Is FBM.EntityType)

                        ' Fallback via key/non-key if typing didn’t resolve uniquely
                        If idxVal = -1 OrElse idxEnt = -1 Then
                            Dim iNon As Integer = Enumerable.Range(0, arAllRoles.Count).First(Function(i) lNonKeySet.Contains(arAllRoles(i)))
                            Dim iKey As Integer = Enumerable.Range(0, arAllRoles.Count).First(Function(i) lKeySet.Contains(arAllRoles(i)))
                            idxVal = iNon : idxEnt = iKey
                        End If

                        Dim larXArgs As New List(Of String) From {larX(idxVal), larX(idxEnt)}
                        Dim larYArgs As New List(Of String) From {larY(idxVal), larY(idxEnt)}  ' => e.g., (?y_person_id_2 ?y_person_1)

                        ' Inverse: if NON-KEY equal then KEY equal (use SAME predicate name, args in (VALUE, ENTITY) order)
                        Dim larConjInv As New List(Of String) From {
                            Atom(lsAltReading, larXArgs.ToArray()),
                            Atom(lsAltReading, larYArgs.ToArray())
                        }
                        For i = 0 To arAllRoles.Count - 1
                            If lNonKeySet.Contains(arAllRoles(i)) Then
                                larConjInv.Add(EqF(larX(i), larY(i)))
                            End If
                        Next
                        Dim lAnteInv As String = AndF(larConjInv.ToArray())

                        Dim larEqKey As New List(Of String)
                        For i = 0 To arAllRoles.Count - 1
                            If lKeySet.Contains(arAllRoles(i)) Then
                                larEqKey.Add(EqF(larX(i), larY(i)))
                            End If
                        Next

                        If larEqKey.Count > 0 Then
                            Dim lConsInv As String = If(larEqKey.Count = 1, larEqKey(0), AndF(larEqKey.ToArray()))
                            Builder.AppendLine("; inverse")
                            Builder.AppendLine(ForallF(larAllVars, ImpliesF(lAnteInv, lConsInv)))
                            Builder.AppendLine()
                        End If
                    End If
#End Region
                End If

                Builder.AppendLine()
#End Region
            ElseIf aarKeyRoles.Count > 2 Then
#Region "arKeyRoles.Count > 2"

                ' Variables for role players
                Dim larVars As New List(Of String)

                Dim lObj As FBM.FactType = aarKeyRoles(0).FactType

                For i = 0 To aarKeyRoles.Count - 1
                    larVars.Add(Variable("x" & (i + 1).ToString()))
                Next

                ' Type atoms for role players
                Dim larTypeAtoms As New List(Of String)
                For i = 0 To aarKeyRoles.Count - 1
                    Dim lJoined = aarKeyRoles(i).JoinedORMObject
                    If lJoined Is Nothing Then Continue For
                    Dim lPrefix = If(TypeOf lJoined Is FBM.ValueType, "vt:", "et:")
                    larTypeAtoms.Add(Atom(lPrefix & lJoined.Id, larVars(i)))
                Next

                ' Predicate names
                Dim lUnderlying As String = aarKeyRoles(0).FactType.Id.ToSnakeCase()                              ' e.g., showing / booked / has_seat
                Dim lBridge As String = "_of" '20250831-VM-Was (lObj.Name).ToSnakeCase() & "_of"                       ' e.g., session_of / booking_of

                ' Objectified entity variable
                Dim lS As String = Variable("s")

                ' 1) Typing for the underlying n-ary predicate
                Builder.AppendLine(
                ForallF(larVars.ToArray(),
                    ImpliesF(Atom(lUnderlying, larVars.ToArray()), AndF(larTypeAtoms.ToArray()))
                )
            )

                ' 2) Bridge existence: et:Object -> exists tuple s.t. underlying(...) & bridge(s, tuple)
                Builder.AppendLine(
                ForallF({lS},
                    ImpliesF(
                        Atom("et:" & lObj.Id, lS),
                        ExistsF(larVars.ToArray(),
                            AndF(
                                Atom(lUnderlying, larVars.ToArray()),
                                Atom(lBridge, (New List(Of String) From {lS}).Concat(larVars).ToArray())
                            )
                        )
                    )
                )
            )

                ' 3) Bijection on key (uniqueness of object given key)
                Dim lS2 As String = Variable("s2")
                Builder.AppendLine(
                ForallF((New List(Of String) From {lS, lS2}).Concat(larVars).ToArray(),
                    ImpliesF(
                        AndF(
                            Atom(lBridge, (New List(Of String) From {lS}).Concat(larVars).ToArray()),
                            Atom(lBridge, (New List(Of String) From {lS2}).Concat(larVars).ToArray())
                        ),
                        EqF(lS, lS2)
                    )
                )
            )

                ' 4) Equality of key components given same object (injectivity)
                Dim larVars2 As New List(Of String)
                For i = 0 To aarKeyRoles.Count - 1
                    larVars2.Add(Variable("x" & (i + 1).ToString() & "b"))
                Next
                Dim lBoth As String = AndF(
                Atom(lBridge, (New List(Of String) From {lS}).Concat(larVars).ToArray()),
                Atom(lBridge, (New List(Of String) From {lS}).Concat(larVars2).ToArray())
            )
                Dim larEq As New List(Of String)
                For i = 0 To aarKeyRoles.Count - 1
                    larEq.Add(EqF(larVars(i), larVars2(i)))
                Next
                Builder.AppendLine(
                ForallF((New List(Of String) From {lS}).Concat(larVars).Concat(larVars2).ToArray(),
                    ImpliesF(lBoth, AndF(larEq.ToArray()))
                )
            )

                ' 5) Typing for the bridge predicate
                Dim larTyping2 As New List(Of String)
                larTyping2.Add(Atom("et:" & lObj.Id, lS))
                larTyping2.AddRange(larTypeAtoms)
                Builder.AppendLine(
                ForallF((New List(Of String) From {lS}).Concat(larVars).ToArray(),
                    ImpliesF(
                        Atom(lBridge, (New List(Of String) From {lS}).Concat(larVars).ToArray()),
                        AndF(larTyping2.ToArray())
                    )
                )
            )

                Builder.AppendLine()
                Builder.AppendLine()
#End Region
            End If

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Sub

    Private Function GetFactPredName(aFactType As FBM.FactType, ByVal aarKeyRole As List(Of FBM.Role)) As String

        ' Prefer a reading-derived name; else default to ft:<Id or Name>
        If aFactType IsNot Nothing AndAlso aFactType.FactTypeReading IsNot Nothing AndAlso aFactType.FactTypeReading.Count > 0 Then

            Dim lPref = aFactType.FactTypeReading.FirstOrDefault(Function(r) r IsNot Nothing AndAlso r.IsPreferred)
            If lPref Is Nothing Then lPref = aFactType.FactTypeReading(0)

            Dim larPreferredPredicate = From PredicatePart In lPref.PredicatePart
                                        Where PredicatePart.SequenceNr = 1
                                        Where PredicatePart.Role.Id = aarKeyRole(0).Id
                                        Select PredicatePart

            If larPreferredPredicate.Count > 0 Then

                If lPref IsNot Nothing AndAlso lPref.PredicatePart IsNot Nothing AndAlso lPref.PredicatePart.Count > 0 Then

                    Dim larTokens As New List(Of String)

                    For Each lPart In lPref.PredicatePart.OrderBy(Function(p) p.SequenceNr)
                        Dim lChunk As String = ((lPart.PreBoundText & " " & lPart.PredicatePartText & " " & lPart.PostBoundText).Trim())
                        If Not String.IsNullOrWhiteSpace(lChunk) Then
                            larTokens.Add(lChunk.ToLowerInvariant().Replace(" "c, "_"c))
                        End If
                    Next

                    Dim lName As String = String.Join("_", larTokens.Where(Function(t) t <> ""))

                    If Not String.IsNullOrWhiteSpace(lName) Then Return lName
                End If

            Else

                Dim larPredicatePart = (From FactTypeReading In aFactType.FactTypeReading
                                        From PredicatePart In FactTypeReading.PredicatePart
                                        Where PredicatePart.SequenceNr = 1
                                        Where PredicatePart.Role.Id = aarKeyRole(0).Id
                                        Select PredicatePart.PredicatePartText).ToList

                If larPredicatePart.Count > 0 Then

                    Return larPredicatePart.First.ToSnakeCase

                End If

            End If

        End If

        Dim lFallback As String = If(Not String.IsNullOrWhiteSpace(aFactType?.Name), aFactType.Name, aFactType?.Id)

        Return "ft:" & New String(lFallback.Select(Function(c) If(Char.IsLetterOrDigit(c) OrElse c = "_"c OrElse c = "-"c, c, "_"c)).ToArray())

    End Function

End Class
