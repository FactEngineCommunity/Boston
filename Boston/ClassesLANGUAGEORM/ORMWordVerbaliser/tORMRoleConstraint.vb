Imports System.Reflection

Namespace FBM

    Partial Public Class RoleConstraint

        Public Sub verbaliseEqualityConstraint(ByRef arORMWordVerbaliser As FBM.ORMWordVerbailser)

            Try
                If Me.RoleConstraintType <> pcenumRoleConstraintType.EqualityConstraint Then
                    Throw New Exception("Method called for Role Constraint that is actually a " & Me.RoleConstraintType.ToString & " constraint.")
                End If

                arORMWordVerbaliser.VerbaliseModelObject(Me)
                arORMWordVerbaliser.VerbaliseQuantifier(" is a Role Constraint")
                arORMWordVerbaliser.VerbaliseQuantifier(" (of type, 'Equality Constraint')")
                arORMWordVerbaliser.WriteLine()
                arORMWordVerbaliser.WriteLine()

                '--------------------------------------------------------------------------------
                'Get the list of ModelObjects in the RoleConstraints set of RoleConstraintRoles
                '--------------------------------------------------------------------------------
                Dim lrModelObject As FBM.ModelObject
                Dim larModelObject As New List(Of FBM.ModelObject)

                larModelObject = Me.GetCommonArgumentModelObjects()

                arORMWordVerbaliser.VerbaliseQuantifier("For each: ")

                Dim liInd As Integer = 1
                For Each lrModelObject In larModelObject
                    arORMWordVerbaliser.VerbaliseModelObject(lrModelObject)
                    If liInd < larModelObject.Count Then
                        arORMWordVerbaliser.VerbaliseQuantifier(" and ")
                    End If
                    liInd += 1
                Next

                arORMWordVerbaliser.WriteLine()

                liInd = 0
                For Each lrRoleConstraintArgument In Me.Argument
                    Call lrRoleConstraintArgument.ProjectArgumentReading(arORMWordVerbaliser, larModelObject)

                    If liInd = 0 Then
                        arORMWordVerbaliser.VerbaliseQuantifier(" if and only if ")
                    End If

                    arORMWordVerbaliser.WriteLine()
                    liInd += 1
                Next


            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Public Sub verbaliseExclusionConstraint(ByRef arORMWordVerbaliser As FBM.ORMWordVerbailser)

            Try
                '------------------------------------------------------------
                'Declare that the RoleConstraint(Name) is an RoleConstraint
                '------------------------------------------------------------
                arORMWordVerbaliser.VerbaliseModelObject(Me)
                arORMWordVerbaliser.VerbaliseQuantifier(" is a Role Constraint")
                arORMWordVerbaliser.VerbaliseQuantifier(" (of type, 'Exclusion Role Constraint')")
                arORMWordVerbaliser.WriteLine()
                arORMWordVerbaliser.WriteLine()

                If Me.Argument.Count < 2 Then
                    arORMWordVerbaliser.VerbaliseError("Error: An Exclusion Role Constraint requires at least 2 Role Constraint Arguments.")
                    arORMWordVerbaliser.WriteLine()
                    arORMWordVerbaliser.VerbaliseError("This Exclusion Role Constraint has " & Me.Argument.Count.ToString & " Role Constraint Arguments.")
                    arORMWordVerbaliser.WriteLine()
                    arORMWordVerbaliser.WriteLine()
                End If

                arORMWordVerbaliser.VerbaliseQuantifier("For each ")

                Dim lrModelObject As New FBM.ModelObject

                '-----------------------------
                'Find the common ModelObject
                '-----------------------------
                If Me.RoleConstraintRole.Count = 0 Then
                    Exit Sub
                End If

                Dim larModelObject As List(Of FBM.ModelObject)
                larModelObject = Me.GetCommonArgumentModelObjects

                lrModelObject = Me.RoleConstraintRole(0).Role.JoinedORMObject

                Dim liInd As Integer = 1
                For Each lrModelObject In larModelObject
                    arORMWordVerbaliser.VerbaliseModelObject(lrModelObject)
                    If liInd = larModelObject.Count - 1 Then
                        arORMWordVerbaliser.VerbaliseQuantifier(" and ")
                    ElseIf liInd = larModelObject.Count Then
                    Else
                        arORMWordVerbaliser.VerbaliseSeparator(",")
                    End If
                    liInd += 1
                Next
                arORMWordVerbaliser.VerbaliseQuantifier(" at most one of the following holds:")
                arORMWordVerbaliser.WriteLine()

                Dim lrRoleConstraintArgument As FBM.RoleConstraintArgument

                For Each lrRoleConstraintArgument In Me.Argument
                    Call lrRoleConstraintArgument.ProjectArgumentReading(arORMWordVerbaliser, larModelObject)
                    arORMWordVerbaliser.WriteLine()
                Next

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Public Sub verbaliseExclusiveORConstraint(ByRef arORMWordVerbaliser As FBM.ORMWordVerbailser)

            Try

                '------------------------------------------------------------
                'Declare that the RoleConstraint(Name) is an RoleConstraint
                '------------------------------------------------------------
                arORMWordVerbaliser.VerbaliseModelObject(Me)
                arORMWordVerbaliser.VerbaliseQuantifier(" is a Role Constraint (of type, 'Exclusive OR Constraint')")
                arORMWordVerbaliser.WriteLine()
                arORMWordVerbaliser.WriteLine()


                Dim liInd As Integer = 0
                Dim liReadingInd As Integer = 0
                Dim lrModelObject As New FBM.ModelObject
                Dim lrFactType As FBM.FactType
                Dim larFactTypeModelObjects As New List(Of FBM.ModelObject)

                '-----------------------------
                'Find the common ModelObject
                '-----------------------------
                If Me.RoleConstraintRole.Count = 0 Then
                    arORMWordVerbaliser.VerbaliseError("The Role Constraint has no arguments.")                    
                    Exit Sub
                End If
                lrModelObject = Me.RoleConstraintRole(0).Role.JoinedORMObject

                arORMWordVerbaliser.VerbaliseQuantifier("For each ")
                arORMWordVerbaliser.VerbaliseModelObject(lrModelObject)
                arORMWordVerbaliser.VerbaliseQuantifier(", at most one of the following holds:")
                arORMWordVerbaliser.WriteLine()

                For Each lrRoleConstraintRole In Me.RoleConstraintRole
                    lrFactType = lrRoleConstraintRole.Role.FactType
                    If lrFactType.FactTypeReading.Count > 0 Then
                        Dim lrFactTypeReading As FBM.FactTypeReading = lrFactType.FactTypeReading(0)
                        Call lrFactTypeReading.Verbalise(arORMWordVerbaliser, " that ", pcenumFollowingThatOrSome.Some)
                        arORMWordVerbaliser.WriteLine()
                    Else
                        arORMWordVerbaliser.VerbaliseError("<Provide a Fact Type Reading for Fact Type, '" & lrFactType.Name & "', to complete this verbalisation>")
                        arORMWordVerbaliser.WriteLine()
                    End If
                Next

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Public Sub verbaliseInclusiveORConstraint(ByRef arORMWordVerbaliser As FBM.ORMWordVerbailser)

            Try

                '------------------------------------------------------------
                'Declare that the RoleConstraint(Name) is an RoleConstraint
                '------------------------------------------------------------
                arORMWordVerbaliser.VerbaliseModelObject(Me)
                arORMWordVerbaliser.VerbaliseQuantifier(" is a Role Constraint")
                arORMWordVerbaliser.VerbaliseQuantifier(" (of type, 'Inclusive OR Constraint')")
                arORMWordVerbaliser.WriteLine()
                arORMWordVerbaliser.WriteLine()

                Dim liInd As Integer = 0
                Dim liReadingInd As Integer = 0
                Dim lrFactType As FBM.FactType
                Dim lrFactTypeReading As FBM.FactTypeReading
                Dim lrRoleConstraintRole As FBM.RoleConstraintRole
                Dim lrModelObject As FBM.ModelObject

                '-----------------------------
                'Find the common ModelObject
                '-----------------------------
                If Me.RoleConstraintRole.Count = 0 Then
                    arORMWordVerbaliser.VerbaliseError("The Role Constraint has no arguments.")

                    Exit Sub
                End If
                lrModelObject = Me.RoleConstraintRole(0).Role.JoinedORMObject

                arORMWordVerbaliser.VerbaliseQuantifier("Each ")
                arORMWordVerbaliser.VerbaliseModelObject(lrModelObject)
                arORMWordVerbaliser.WriteLine()

                For Each lrRoleConstraintRole In Me.RoleConstraintRole
                    lrFactType = lrRoleConstraintRole.Role.FactType
                    If lrFactType.FactTypeReading.Count > 0 Then

                        Dim lrRole As FBM.Role
                        Dim larRole = From Role In lrFactType.RoleGroup _
                                      Select Role

                        Dim larRoleList As New List(Of FBM.Role)
                        larRoleList.Add(lrRoleConstraintRole.Role)

                        For Each lrRole In larRole
                            If lrRole Is lrRoleConstraintRole.Role Then
                                '--------
                                'Ignore
                                '--------
                            Else
                                larRoleList.Add(lrRole)
                            End If
                        Next

                        lrFactTypeReading = lrFactType.FindSuitableFactTypeReadingByRoles(larRoleList)

                        If IsSomething(lrFactTypeReading) Then
                            Dim lrPredicatePart As FBM.PredicatePart
                            liReadingInd = 0
                            For Each lrPredicatePart In lrFactTypeReading.PredicatePart
                                If liReadingInd < lrFactTypeReading.PredicatePart.Count Then
                                    arORMWordVerbaliser.VerbalisePredicateText(" " & lrPredicatePart.PredicatePartText & " ")
                                End If
                                If liReadingInd > 0 Then
                                    arORMWordVerbaliser.VerbaliseQuantifier("some ")
                                    arORMWordVerbaliser.VerbaliseModelObject(lrPredicatePart.Role.JoinedORMObject)
                                End If
                                liReadingInd += 1
                            Next
                        Else
                            arORMWordVerbaliser.VerbaliseError("Error: Fact Type needs at least on Fact Type Reading.")
                        End If
                    Else
                        arORMWordVerbaliser.VerbaliseError("Error: Fact Type needs at least on Fact Type Reading.")
                    End If
                    If liInd < Me.RoleConstraintRole.Count - 1 Then
                        arORMWordVerbaliser.VerbaliseQuantifier(" or ")
                        arORMWordVerbaliser.WriteLine()
                    End If
                    liInd += 1
                Next

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Public Sub verbaliseUniquenessConstraint(ByRef arORMWordVerbaliser As FBM.ORMWordVerbailser)

            Try
                Dim lrFactType As FBM.FactType

                If Me.RoleConstraintRole.Count = 0 Then
                    arORMWordVerbaliser.VerbaliseError("<Provide links to Roles for Role Constraint, '" & Me.Name & "', to complete this verbalisation>")
                    arORMWordVerbaliser.WriteLine()
                    Exit Sub
                End If

                '------------------------------------------------------------
                'Declare that the RoleConstraint(Name) is an RoleConstraint
                '------------------------------------------------------------
                arORMWordVerbaliser.VerbaliseModelObject(Me)
                arORMWordVerbaliser.VerbaliseQuantifier(" is a Role Constraint")
                arORMWordVerbaliser.VerbaliseQuantifier(" (of type, 'Uniqueness Constraint')")
                arORMWordVerbaliser.WriteLine()
                arORMWordVerbaliser.WriteLine()


                Dim liInd As Integer = 0
                Dim lrModelObject As FBM.ModelObject
                Dim lrRoleConstraintRole As FBM.RoleConstraintRole

                arORMWordVerbaliser.VerbaliseQuantifier("For each ")

                Dim larModelObject = From RoleConstraintRole In Me.RoleConstraintRole _
                                     Select RoleConstraintRole.Role.JoinedORMObject

                Dim lsPreboundReadingText As String = ""
                For Each lrRoleConstraintRole In Me.RoleConstraintRole
                    lrFactType = lrRoleConstraintRole.Role.FactType
                    lrModelObject = lrRoleConstraintRole.Role.JoinedORMObject
                    If lrFactType.FactTypeReading.Count > 0 Then
                        lsPreboundReadingText = lrFactType.GetPreboundReadingTextForModelElementAtPosition(lrModelObject,
                                                                                                           0)
                    End If
                    arORMWordVerbaliser.VerbalisePredicateText(lsPreboundReadingText)
                    arORMWordVerbaliser.VerbaliseModelObject(lrModelObject)

                    If liInd < larModelObject.Count - 1 Then
                        arORMWordVerbaliser.VerbaliseQuantifier(" and ")
                    End If
                    liInd += 1
                Next

                arORMWordVerbaliser.WriteLine()
                arORMWordVerbaliser.VerbaliseQuantifier(" at most one ")

                Dim larListedFactType As New List(Of FBM.FactType)
                Dim lbSkippedFactType As Boolean = False

                liInd = 0

                Dim lrCommonModelObject As FBM.ModelObject
                Dim lbFoundCommonModelObject As Boolean = False
                Dim larArgumentCommonModelObjects As New List(Of FBM.ModelObject)

                If Me.IsEachRoleFactTypeBinary Then
                    If Me.DoesEachRoleFactTypeOppositeRoleJoinSameModelObject Then
                        lrFactType = Me.RoleConstraintRole(0).Role.FactType
                        lrCommonModelObject = lrFactType.GetOtherRoleOfBinaryFactType(Me.RoleConstraintRole(0).Role.Id).JoinedORMObject

                        larArgumentCommonModelObjects.Add(lrCommonModelObject)

                        lbFoundCommonModelObject = True
                    End If
                End If

                'FactTypeReading(0).GetReadingTextThatOrSome(Me.JoinPath.RolePath, arVerbaliser, larArgumentCommonModelObjects, larVerbalisedModelObject)
                Dim lrFactTypeReading As FBM.FactTypeReading
                Dim larRole As New List(Of FBM.Role)
                For Each lrRoleConstraintRole In Me.RoleConstraintRole

                    lrFactType = lrRoleConstraintRole.Role.FactType

                    larRole.Clear()
                    larRole.Add(lrRoleConstraintRole.Role.FactType.GetOtherRoleOfBinaryFactType(lrRoleConstraintRole.Role.Id))
                    larRole.Add(lrRoleConstraintRole.Role)
                    lrFactTypeReading = lrFactType.FindSuitableFactTypeReadingByRoles(larRole, True)

                    lbSkippedFactType = False

                    If (liInd >= 1) And Not larListedFactType.Exists(AddressOf lrFactType.Equals) Then
                        arORMWordVerbaliser.VerbaliseQuantifier(" and ")
                    End If

                    If lrFactTypeReading IsNot Nothing Then
                        lrFactTypeReading.GetReadingTextThatOrSome(larRole,
                                                                   arORMWordVerbaliser,
                                                                   larArgumentCommonModelObjects,
                                                                   New List(Of FBM.ModelObject),
                                                                   liInd > 0,
                                                                   Nothing,
                                                                   True)
                    Else
                        arORMWordVerbaliser.VerbaliseError("Provide a Fact Type Reading for Fact Type, '" & lrFactType.Name & "', to complete this verbalisation")
                        arORMWordVerbaliser.WriteLine()
                    End If

                    If Not lbSkippedFactType Then
                        arORMWordVerbaliser.WriteLine()
                    End If

                    liInd += 1
                Next

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Public Sub verbaliseRingConstraint(ByRef arORMWordVerbaliser As FBM.ORMWordVerbailser)

            Select Case Me.RingConstraintType
                Case Is = pcenumRingConstraintType.Acyclic
                    Call Me.VerbaliseRingConstraintAcyclic(arORMWordVerbaliser)

                Case Is = pcenumRingConstraintType.AcyclicIntransitive
                    Call Me.verbaliseRingConstraintAcyclicIntransitive(arORMWordVerbaliser)

                Case Is = pcenumRingConstraintType.Antisymmetric
                    Call Me.VerbaliseRingConstraintAntisymmetric(arORMWordVerbaliser)

                Case Is = pcenumRingConstraintType.Asymmetric
                    Call Me.verbaliseRingConstraintAsymmetric(arORMWordVerbaliser)

                Case Is = pcenumRingConstraintType.AsymmetricIntransitive
                    Call Me.verbaliseRingConstraintAsymmetricIntransitive(arORMWordVerbaliser)

                Case Is = pcenumRingConstraintType.DeonticAcyclic
                    Call Me.VerbaliseRingConstraintAcyclic(arORMWordVerbaliser)

                Case Is = pcenumRingConstraintType.DeonticAcyclicIntransitive
                    Call Me.verbaliseRingConstraintAcyclicIntransitive(arORMWordVerbaliser)

                Case Is = pcenumRingConstraintType.DeonticAntisymmetric
                    Call Me.VerbaliseRingConstraintAntisymmetric(arORMWordVerbaliser)

                Case Is = pcenumRingConstraintType.DeonticAssymmetric
                    Call Me.verbaliseRingConstraintAsymmetric(arORMWordVerbaliser)

                Case Is = pcenumRingConstraintType.DeonticAssymmetricIntransitive
                    Call Me.verbaliseRingConstraintAsymmetricIntransitive(arORMWordVerbaliser)

                Case Is = pcenumRingConstraintType.DeonticIntransitive
                    Call Me.VerbaliseRingConstraintIntransitive(arORMWordVerbaliser)

                Case Is = pcenumRingConstraintType.DeonticIrreflexive
                    Call Me.verbaliseRingConstraintIrreflexive(arORMWordVerbaliser)

                Case Is = pcenumRingConstraintType.DeonticPurelyReflexive
                    Call Me.verbaliseRingConstraintPurelyReflexive(arORMWordVerbaliser)

                Case Is = pcenumRingConstraintType.DeonticSymmetric
                    Call Me.verbaliseRingConstraintSymmetric(arORMWordVerbaliser)

                Case Is = pcenumRingConstraintType.DeonticSymmetricIntransitive
                    Call Me.verbaliseRingConstraintSymmetricIntransitive(arORMWordVerbaliser)

                Case Is = pcenumRingConstraintType.DeonticSymmetricIrreflexive
                    Call Me.verbaliseRingConstraintSymmetricIrreflexive(arORMWordVerbaliser)

                Case Is = pcenumRingConstraintType.Intransitive
                    Call Me.verbaliseRingConstraintIntransitive(arORMWordVerbaliser)

                Case Is = pcenumRingConstraintType.Irreflexive
                    Call Me.verbaliseRingConstraintIrreflexive(arORMWordVerbaliser)

                Case Is = pcenumRingConstraintType.PurelyReflexive
                    Call Me.verbaliseRingConstraintPurelyReflexive(arORMWordVerbaliser)

                Case Is = pcenumRingConstraintType.Symmetric
                    Call Me.verbaliseRingConstraintSymmetric(arORMWordVerbaliser)

                Case Is = pcenumRingConstraintType.SymmetricIntransitive
                    Call Me.verbaliseRingConstraintSymmetricIntransitive(arORMWordVerbaliser)

                Case Is = pcenumRingConstraintType.SymmetricIrreflexive
                    Call Me.verbaliseRingConstraintSymmetricIrreflexive(arORMWordVerbaliser)
            End Select

        End Sub

        Public Sub VerbaliseRingConstraintAcyclic(ByRef arORMWordVerbaliser As FBM.ORMWordVerbailser)

            Try
                If Me.RoleConstraintRole.Count = 0 Then
                    arORMWordVerbaliser.VerbaliseError("<Provide links to Roles for Role Constraint, '" & Me.Name & "', to complete this verbalisation>")
                    arORMWordVerbaliser.WriteLine()
                    Exit Sub
                End If

                '------------------------------------------------------------
                'Declare that the RoleConstraint(Name) is an RoleConstraint
                '------------------------------------------------------------
                arORMWordVerbaliser.VerbaliseModelObject(Me)
                arORMWordVerbaliser.VerbaliseQuantifier(" is a Role Constraint")
                arORMWordVerbaliser.VerbaliseQuantifier(" (of type, 'Ring Constraint')")
                arORMWordVerbaliser.WriteLine()
                arORMWordVerbaliser.WriteLine()

                If Me.IsDeontic Then
                    arORMWordVerbaliser.VerbaliseQuantifier("It is obligatory that no ")
                Else
                    arORMWordVerbaliser.VerbaliseQuantifier("No ")
                End If

                arORMWordVerbaliser.VerbaliseModelObject(Me.RoleConstraintRole(0).Role.JoinedORMObject)
                arORMWordVerbaliser.VerbaliseQuantifier(" may cycle back to itself via one or more traversals through ")

                Dim lrFactType As FBM.FactType
                lrFactType = Me.RoleConstraintRole(0).Role.FactType
                If lrFactType.FactTypeReading.Count > 0 Then
                    Dim lrFactTypeReading As FBM.FactTypeReading
                    lrFactTypeReading = lrFactType.FactTypeReading(0)
                    Call lrFactTypeReading.Verbalise(arORMWordVerbaliser, Nothing, pcenumFollowingThatOrSome.Either, False)
                Else
                    arORMWordVerbaliser.VerbaliseError("<Provide a Fact Type Reading for Fact Type, '" & lrFactType.Name & "', to complete this verbalisation>")
                    arORMWordVerbaliser.WriteLine()
                End If

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Public Sub verbaliseRingConstraintAcyclicIntransitive(ByRef arORMWordVerbaliser As FBM.ORMWordVerbailser)

            Try
                If Me.RoleConstraintRole.Count = 0 Then
                    arORMWordVerbaliser.VerbaliseError("<Provide links to Roles for Role Constraint, '" & Me.Name & "', to complete this verbalisation>")
                    arORMWordVerbaliser.WriteLine()
                    Exit Sub
                End If

                '------------------------------------------------------------
                'Declare that the RoleConstraint(Name) is an RoleConstraint
                '------------------------------------------------------------
                arORMWordVerbaliser.VerbaliseModelObject(Me)
                arORMWordVerbaliser.VerbaliseQuantifier(" is a Role Constraint")
                arORMWordVerbaliser.VerbaliseQuantifier(" (of type, 'Acyclic Intransitive Ring Constraint')")
                arORMWordVerbaliser.WriteLine()
                arORMWordVerbaliser.WriteLine()

                If Me.IsDeontic Then
                    arORMWordVerbaliser.VerbaliseQuantifier("It is obligatory that no ")
                Else
                    arORMWordVerbaliser.VerbaliseQuantifier("No ")
                End If

                arORMWordVerbaliser.VerbaliseModelObject(Me.RoleConstraintRole(0).Role.JoinedORMObject)
                arORMWordVerbaliser.VerbaliseQuantifier(" may cycle back to itself via one or more traversals through ")

                Dim lrFactType As FBM.FactType
                lrFactType = Me.RoleConstraintRole(0).Role.FactType

                If lrFactType.FactTypeReading.Count > 0 Then
                    Dim lrFactTypeReading As FBM.FactTypeReading = lrFactType.FactTypeReading(0)
                    Call lrFactTypeReading.Verbalise(arORMWordVerbaliser, Nothing, pcenumFollowingThatOrSome.Either, False)
                Else
                    arORMWordVerbaliser.VerbaliseError("<Provide a Fact Type Reading for Fact Type, '" & lrFactType.Name & "', to complete this verbalisation>")
                    arORMWordVerbaliser.WriteLine()
                End If

                arORMWordVerbaliser.WriteLine()
                arORMWordVerbaliser.WriteLine()
                arORMWordVerbaliser.VerbaliseQuantifier("If ")
                lrFactType = Me.RoleConstraintRole(0).Role.FactType
                If lrFactType.FactTypeReading.Count > 0 Then
                    Dim lrFactTypeReading As FBM.FactTypeReading = lrFactType.FactTypeReading(0)
                    Call lrFactTypeReading.Verbalise(arORMWordVerbaliser, Nothing, pcenumFollowingThatOrSome.Either, False, 1)
                Else
                    arORMWordVerbaliser.VerbaliseError("<Provide a Fact Type Reading for Fact Type, '" & lrFactType.Name & "', to complete this verbalisation>")
                    arORMWordVerbaliser.WriteLine()
                End If

                arORMWordVerbaliser.VerbaliseQuantifier(" and ")
                lrFactType = Me.RoleConstraintRole(0).Role.FactType
                If lrFactType.FactTypeReading.Count > 0 Then
                    Dim lrFactTypeReading As FBM.FactTypeReading = lrFactType.FactTypeReading(0)
                    Call lrFactTypeReading.Verbalise(arORMWordVerbaliser, "that ", pcenumFollowingThatOrSome.Either, False, 2)
                Else
                    arORMWordVerbaliser.VerbaliseError("<Provide a Fact Type Reading for Fact Type, '" & lrFactType.Name & "', to complete this verbalisation>")
                    arORMWordVerbaliser.WriteLine()
                End If

                'If Entity Type1 is subtype of Entity Type2 and Entity Type2 is subtype of Entity Type3

                If Me.IsDeontic Then
                    arORMWordVerbaliser.WriteLine()
                    arORMWordVerbaliser.VerbaliseQuantifier(" then it is forbidden that ")
                Else
                    arORMWordVerbaliser.WriteLine()
                    arORMWordVerbaliser.VerbaliseQuantifier(" then it is impossible that ")
                End If


                Dim lrSubscriptArray As New List(Of String)
                lrSubscriptArray.Add("1")
                lrSubscriptArray.Add("3")
                lrFactType = Me.RoleConstraintRole(0).Role.FactType
                If lrFactType.FactTypeReading.Count > 0 Then
                    Dim lrFactTypeReading As FBM.FactTypeReading = lrFactType.FactTypeReading(0)
                    Call lrFactTypeReading.Verbalise(arORMWordVerbaliser, "that ", pcenumFollowingThatOrSome.That, False, 1, lrSubscriptArray)
                Else
                    arORMWordVerbaliser.VerbaliseError("<Provide a Fact Type Reading for Fact Type, '" & lrFactType.Name & "', to complete this verbalisation>")
                    arORMWordVerbaliser.WriteLine()
                End If

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Public Sub VerbaliseRingConstraintAntisymmetric(ByRef arORMWordVerbaliser As FBM.ORMWordVerbailser)

            Try
                Dim lrFactType As FBM.FactType

                If Me.RoleConstraintRole.Count = 0 Then
                    arORMWordVerbaliser.VerbaliseQuantifier("<Provide links to Roles for Role Constraint, '" & Me.Name & "', to complete this verbalisation>")
                    arORMWordVerbaliser.WriteLine()
                    Exit Sub
                End If

                '------------------------------------------------------------
                'Declare that the RoleConstraint(Name) is an RoleConstraint
                '------------------------------------------------------------
                arORMWordVerbaliser.VerbaliseModelObject(Me)
                arORMWordVerbaliser.VerbaliseQuantifier(" is a Role Constraint")
                arORMWordVerbaliser.VerbaliseQuantifier(" (of type, 'Antisymmetric Ring Constraint')")
                arORMWordVerbaliser.WriteLine()
                arORMWordVerbaliser.WriteLine()

                arORMWordVerbaliser.VerbaliseQuantifier("If ")

                lrFactType = Me.RoleConstraintRole(0).Role.FactType
                If lrFactType.FactTypeReading.Count > 0 Then
                    Dim lrFactTypeReading As FBM.FactTypeReading = lrFactType.FactTypeReading(0)
                    Call lrFactTypeReading.Verbalise(arORMWordVerbaliser, Nothing, pcenumFollowingThatOrSome.Either, False, 1)
                Else
                    arORMWordVerbaliser.VerbaliseError("<Provide a Fact Type Reading for Fact Type, '" & lrFactType.Name & "', to complete this verbalisation>")
                    arORMWordVerbaliser.WriteLine()
                End If

                arORMWordVerbaliser.VerbaliseQuantifier(" and ")

                arORMWordVerbaliser.VerbaliseModelObject(Me.RoleConstraintRole(0).Role.JoinedORMObject)
                arORMWordVerbaliser.Write(" ")
                arORMWordVerbaliser.VerbaliseSubscript("1")
                arORMWordVerbaliser.Write(" ")

                arORMWordVerbaliser.VerbaliseQuantifier("is not ")

                arORMWordVerbaliser.VerbaliseModelObject(Me.RoleConstraintRole(0).Role.JoinedORMObject)
                arORMWordVerbaliser.Write(" ")
                arORMWordVerbaliser.VerbaliseSubscript("2")
                arORMWordVerbaliser.Write(" ")
                arORMWordVerbaliser.WriteLine()

                If Me.IsDeontic Then
                    arORMWordVerbaliser.VerbaliseQuantifier(" then it is forbidden that ")
                Else
                    arORMWordVerbaliser.VerbaliseQuantifier(" then it is impossible that ")
                End If

                lrFactType = Me.RoleConstraintRole(0).Role.FactType
                If lrFactType.FactTypeReading.Count > 0 Then
                    Dim lrFactTypeReading As FBM.FactTypeReading = lrFactType.FactTypeReading(0)
                    Call lrFactTypeReading.Verbalise(arORMWordVerbaliser, Nothing, pcenumFollowingThatOrSome.Either, False, 1, Nothing, True)
                Else
                    arORMWordVerbaliser.VerbaliseError("<Provide a Fact Type Reading for Fact Type, '" & lrFactType.Name & "', to complete this verbalisation>")
                    arORMWordVerbaliser.WriteLine()
                End If

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Public Sub verbaliseRingConstraintAsymmetric(ByRef arORMWordVerbaliser As FBM.ORMWordVerbailser)

            Try
                Dim lrFactType As FBM.FactType

                If Me.RoleConstraintRole.Count = 0 Then
                    arORMWordVerbaliser.VerbaliseError("<Provide links to Roles for Role Constraint, '" & Me.Name & "', to complete this verbalisation>")
                    arORMWordVerbaliser.WriteLine()
                    Exit Sub
                End If

                '------------------------------------------------------------
                'Declare that the RoleConstraint(Name) is an RoleConstraint
                '------------------------------------------------------------
                arORMWordVerbaliser.VerbaliseModelObject(Me)
                arORMWordVerbaliser.VerbaliseQuantifier(" is a Role Constraint")
                arORMWordVerbaliser.VerbaliseQuantifier(" (of type, 'Ring Constraint')")
                arORMWordVerbaliser.WriteLine()
                arORMWordVerbaliser.WriteLine()

                arORMWordVerbaliser.VerbaliseQuantifier("If ")
                lrFactType = Me.RoleConstraintRole(0).Role.FactType
                If lrFactType.FactTypeReading.Count > 0 Then
                    Dim lrFactTypeReading As FBM.FactTypeReading = lrFactType.FactTypeReading(0)
                    Call lrFactTypeReading.Verbalise(arORMWordVerbaliser, "", pcenumFollowingThatOrSome.None, False, 1)
                Else
                    arORMWordVerbaliser.VerbaliseError("<Provide a Fact Type Reading for Fact Type, '" & lrFactType.Name & "', to complete this verbalisation>")
                    arORMWordVerbaliser.WriteLine()
                End If

                If Me.IsDeontic Then
                    arORMWordVerbaliser.WriteLine()
                    arORMWordVerbaliser.VerbaliseQuantifier(" then it is forbidden that ")
                Else
                    arORMWordVerbaliser.WriteLine()
                    arORMWordVerbaliser.VerbaliseQuantifier(" then it is impossible that ")
                End If

                lrFactType = Me.RoleConstraintRole(0).Role.FactType
                If lrFactType.FactTypeReading.Count > 0 Then
                    Dim lrFactTypeReading As FBM.FactTypeReading = lrFactType.FactTypeReading(0)
                    Call lrFactTypeReading.Verbalise(arORMWordVerbaliser, "", pcenumFollowingThatOrSome.None, False, 2, Nothing, True)
                Else
                    arORMWordVerbaliser.VerbaliseError("<Provide a Fact Type Reading for Fact Type, '" & lrFactType.Name & "', to complete this verbalisation>")
                    arORMWordVerbaliser.WriteLine()
                End If

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Public Sub verbaliseRingConstraintAsymmetricIntransitive(ByRef arORMWordVerbaliser As FBM.ORMWordVerbailser)

            Try
                Dim lrFactType As FBM.FactType

                If Me.RoleConstraintRole.Count = 0 Then
                    arORMWordVerbaliser.VerbaliseError("<Provide links to Roles for Role Constraint, '" & Me.Name & "', to complete this verbalisation>")
                    arORMWordVerbaliser.WriteLine()
                    Exit Sub
                End If

                '------------------------------------------------------------
                'Declare that the RoleConstraint(Name) is an RoleConstraint
                '------------------------------------------------------------
                arORMWordVerbaliser.VerbaliseModelObject(Me)
                arORMWordVerbaliser.VerbaliseQuantifier(" is a Role Constraint")
                arORMWordVerbaliser.VerbaliseQuantifier(" (of type, 'Asymmetric Intransitive Ring Constraint')")
                arORMWordVerbaliser.WriteLine()
                arORMWordVerbaliser.WriteLine()

                arORMWordVerbaliser.VerbaliseQuantifier("If ")

                lrFactType = Me.RoleConstraintRole(0).Role.FactType
                If lrFactType.FactTypeReading.Count > 0 Then
                    Dim lrFactTypeReading As FBM.FactTypeReading = lrFactType.FactTypeReading(0)
                    Call lrFactTypeReading.Verbalise(arORMWordVerbaliser, "", pcenumFollowingThatOrSome.None, False, 1)
                Else
                    arORMWordVerbaliser.VerbaliseError("<Provide a Fact Type Reading for Fact Type, '" & lrFactType.Name & "', to complete this verbalisation>")
                    arORMWordVerbaliser.WriteLine()
                End If

                If Me.IsDeontic Then
                    arORMWordVerbaliser.WriteLine()
                    arORMWordVerbaliser.VerbaliseQuantifier(" then it is forbidden that ")
                Else
                    arORMWordVerbaliser.WriteLine()
                    arORMWordVerbaliser.VerbaliseQuantifier(" then it is impossible that ")
                End If

                lrFactType = Me.RoleConstraintRole(0).Role.FactType
                If lrFactType.FactTypeReading.Count > 0 Then
                    Dim lrFactTypeReading As FBM.FactTypeReading = lrFactType.FactTypeReading(0)
                    Call lrFactTypeReading.Verbalise(arORMWordVerbaliser, "", pcenumFollowingThatOrSome.None, False, 2, Nothing, True)
                Else
                    arORMWordVerbaliser.VerbaliseError("<Provide a Fact Type Reading for Fact Type, '" & lrFactType.Name & "', to complete this verbalisation>")
                    arORMWordVerbaliser.WriteLine()
                End If

                arORMWordVerbaliser.WriteLine()
                arORMWordVerbaliser.VerbaliseQuantifier("If ")

                lrFactType = Me.RoleConstraintRole(0).Role.FactType
                If lrFactType.FactTypeReading.Count > 0 Then
                    Dim lrFactTypeReading As FBM.FactTypeReading = lrFactType.FactTypeReading(0)
                    Call lrFactTypeReading.Verbalise(arORMWordVerbaliser, "", pcenumFollowingThatOrSome.None, False, 1)
                Else
                    arORMWordVerbaliser.VerbaliseError("<Provide a Fact Type Reading for Fact Type, '" & lrFactType.Name & "', to complete this verbalisation>")
                    arORMWordVerbaliser.WriteLine()
                End If

                arORMWordVerbaliser.VerbaliseQuantifier(" and ")

                lrFactType = Me.RoleConstraintRole(0).Role.FactType
                If lrFactType.FactTypeReading.Count > 0 Then
                    Dim lrFactTypeReading As FBM.FactTypeReading = lrFactType.FactTypeReading(0)
                    Call lrFactTypeReading.Verbalise(arORMWordVerbaliser, "", pcenumFollowingThatOrSome.None, False, 2)
                Else
                    arORMWordVerbaliser.VerbaliseError("<Provide a Fact Type Reading for Fact Type, '" & lrFactType.Name & "', to complete this verbalisation>")
                    arORMWordVerbaliser.WriteLine()
                End If

                'If Entity Type1 is subtype of Entity Type2 and Entity Type2 is subtype of Entity Type3
                If Me.IsDeontic Then
                    arORMWordVerbaliser.WriteLine()
                    arORMWordVerbaliser.VerbaliseQuantifier(" then it is forbidden that ")
                Else
                    arORMWordVerbaliser.WriteLine()
                    arORMWordVerbaliser.VerbaliseQuantifier(" then it is impossible that ")
                End If

                Dim lrSubscriptArray As New List(Of String)
                lrSubscriptArray.Add("1")
                lrSubscriptArray.Add("3")
                lrFactType = Me.RoleConstraintRole(0).Role.FactType
                If lrFactType.FactTypeReading.Count > 0 Then
                    Dim lrFactTypeReading As FBM.FactTypeReading = lrFactType.FactTypeReading(0)
                    Call lrFactTypeReading.Verbalise(arORMWordVerbaliser, "that ", pcenumFollowingThatOrSome.That, False, 1, lrSubscriptArray)
                Else
                    arORMWordVerbaliser.VerbaliseError("<Provide a Fact Type Reading for Fact Type, '" & lrFactType.Name & "', to complete this verbalisation>")
                    arORMWordVerbaliser.WriteLine()
                End If


            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Public Sub verbaliseRingConstraintIntransitive(ByRef arORMWordVerbaliser As FBM.ORMWordVerbailser)

            Try
                Dim lrFactType As FBM.FactType

                If Me.RoleConstraintRole.Count = 0 Then
                    arORMWordVerbaliser.VerbaliseError("<Provide links to Roles for Role Constraint, '" & Me.Name & "', to complete this verbalisation>")
                    arORMWordVerbaliser.WriteLine()
                    Exit Sub
                End If

                '------------------------------------------------------------
                'Declare that the RoleConstraint(Name) is an RoleConstraint
                '------------------------------------------------------------
                arORMWordVerbaliser.VerbaliseModelObject(Me)
                arORMWordVerbaliser.VerbaliseQuantifier(" is a Role Constraint")
                arORMWordVerbaliser.VerbaliseQuantifier(" (of type, 'Intransitive Ring Constraint')")
                arORMWordVerbaliser.WriteLine()
                arORMWordVerbaliser.WriteLine()

                arORMWordVerbaliser.VerbaliseQuantifier("If ")

                lrFactType = Me.RoleConstraintRole(0).Role.FactType
                If lrFactType.FactTypeReading.Count > 0 Then
                    Dim lrFactTypeReading As FBM.FactTypeReading = lrFactType.FactTypeReading(0)
                    Call lrFactTypeReading.Verbalise(arORMWordVerbaliser, "", pcenumFollowingThatOrSome.None, False, 1)
                Else
                    arORMWordVerbaliser.VerbaliseError("<Provide a Fact Type Reading for Fact Type, '" & lrFactType.Name & "', to complete this verbalisation>")
                    arORMWordVerbaliser.WriteLine()
                End If

                arORMWordVerbaliser.VerbaliseQuantifier(" and ")

                lrFactType = Me.RoleConstraintRole(0).Role.FactType
                If lrFactType.FactTypeReading.Count > 0 Then
                    Dim lrFactTypeReading As FBM.FactTypeReading = lrFactType.FactTypeReading(0)
                    Call lrFactTypeReading.Verbalise(arORMWordVerbaliser, "", pcenumFollowingThatOrSome.None, False, 2)
                Else
                    arORMWordVerbaliser.VerbaliseError("<Provide a Fact Type Reading for Fact Type, '" & lrFactType.Name & "', to complete this verbalisation>")
                    arORMWordVerbaliser.WriteLine()
                End If
                'If Entity Type1 is subtype of Entity Type2 and Entity Type2 is subtype of Entity Type3

                If Me.IsDeontic Then
                    arORMWordVerbaliser.WriteLine()
                    arORMWordVerbaliser.VerbaliseQuantifier(" then it is forbidden that ")
                Else
                    arORMWordVerbaliser.WriteLine()
                    arORMWordVerbaliser.VerbaliseQuantifier(" then it is impossible that ")
                End If


                Dim lrSubscriptArray As New List(Of String)
                lrSubscriptArray.Add("1")
                lrSubscriptArray.Add("3")
                lrFactType = Me.RoleConstraintRole(0).Role.FactType
                If lrFactType.FactTypeReading.Count > 0 Then
                    Dim lrFactTypeReading As FBM.FactTypeReading = lrFactType.FactTypeReading(0)
                    Call lrFactTypeReading.Verbalise(arORMWordVerbaliser, "that ", pcenumFollowingThatOrSome.That, False, 1, lrSubscriptArray)
                Else
                    arORMWordVerbaliser.VerbaliseError("<Provide a Fact Type Reading for Fact Type, '" & lrFactType.Name & "', to complete this verbalisation>")
                    arORMWordVerbaliser.WriteLine()
                End If

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Public Sub verbaliseRingConstraintIrreflexive(ByRef arORMWordVerbaliser As FBM.ORMWordVerbailser)

            Try
                Dim lrFactType As FBM.FactType

                If Me.RoleConstraintRole.Count = 0 Then
                    arORMWordVerbaliser.VerbaliseError("<Provide links to Roles for Role Constraint, '" & Me.Name & "', to complete this verbalisation>")
                    arORMWordVerbaliser.WriteLine()
                    Exit Sub
                End If

                '------------------------------------------------------------
                'Declare that the RoleConstraint(Name) is an RoleConstraint
                '------------------------------------------------------------
                arORMWordVerbaliser.VerbaliseModelObject(Me)
                arORMWordVerbaliser.VerbaliseQuantifier(" is a Role Constraint")
                arORMWordVerbaliser.VerbaliseQuantifier(" (of type, 'Ring Constraint')")
                arORMWordVerbaliser.WriteLine()
                arORMWordVerbaliser.WriteLine()

                If Me.IsDeontic Then
                    arORMWordVerbaliser.VerbaliseQuantifier("It is obligatory that no ")
                Else
                    arORMWordVerbaliser.VerbaliseQuantifier("No ")
                End If

                lrFactType = Me.RoleConstraintRole(0).Role.FactType
                If lrFactType.FactTypeReading.Count > 0 Then
                    Dim lrFactTypeReading As FBM.FactTypeReading = lrFactType.FactTypeReading(0)
                    Call lrFactTypeReading.Verbalise(arORMWordVerbaliser, "", pcenumFollowingThatOrSome.TheSame, False)
                Else
                    arORMWordVerbaliser.VerbaliseError("<Provide a Fact Type Reading for Fact Type, '" & lrFactType.Name & "', to complete this verbalisation>")
                    arORMWordVerbaliser.WriteLine()
                End If


            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Public Sub verbaliseRingConstraintPurelyReflexive(ByRef arORMWordVerbaliser As FBM.ORMWordVerbailser)

            Try
                Dim lrFactType As FBM.FactType

                If Me.RoleConstraintRole.Count = 0 Then
                    arORMWordVerbaliser.VerbaliseError("<Provide links to Roles for Role Constraint, '" & Me.Name & "', to complete this verbalisation>")
                    arORMWordVerbaliser.WriteLine()
                    Exit Sub
                End If

                '------------------------------------------------------------
                'Declare that the RoleConstraint(Name) is an RoleConstraint
                '------------------------------------------------------------
                arORMWordVerbaliser.VerbaliseModelObject(Me)
                arORMWordVerbaliser.VerbaliseQuantifier(" is a Role Constraint")
                arORMWordVerbaliser.VerbaliseQuantifier(" (of type, 'Purely Reflexive Ring Constraint')")
                arORMWordVerbaliser.WriteLine()
                arORMWordVerbaliser.WriteLine()

                If Me.IsDeontic Then
                    arORMWordVerbaliser.VerbaliseQuantifier("It is obligatory that if ")
                Else
                    arORMWordVerbaliser.VerbaliseQuantifier("If ")
                End If

                lrFactType = Me.RoleConstraintRole(0).Role.FactType
                If lrFactType.FactTypeReading.Count > 0 Then
                    Dim lrFactTypeReading As FBM.FactTypeReading = lrFactType.FactTypeReading(0)
                    Call lrFactTypeReading.Verbalise(arORMWordVerbaliser, Nothing, pcenumFollowingThatOrSome.None, False, 1)
                Else
                    arORMWordVerbaliser.VerbaliseError("<Provide a Fact Type Reading for Fact Type, '" & lrFactType.Name & "', to complete this verbalisation>")
                    arORMWordVerbaliser.WriteLine()
                End If

                arORMWordVerbaliser.VerbaliseQuantifier(" then ")
                arORMWordVerbaliser.VerbaliseModelObject(Me.RoleConstraintRole(0).Role.JoinedORMObject)
                arORMWordVerbaliser.VerbaliseSubscript("1")

                arORMWordVerbaliser.VerbaliseQuantifier("is ")
                arORMWordVerbaliser.VerbaliseModelObject(Me.RoleConstraintRole(0).Role.JoinedORMObject)
                arORMWordVerbaliser.VerbaliseSubscript("2")

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Public Sub verbaliseRingConstraintSymmetric(ByRef arORMWordVerbaliser As FBM.ORMWordVerbailser)

            Try
                Dim lrFactType As FBM.FactType

                If Me.RoleConstraintRole.Count = 0 Then
                    arORMWordVerbaliser.VerbaliseError("<Provide links to Roles for Role Constraint, '" & Me.Name & "', to complete this verbalisation>")
                    arORMWordVerbaliser.WriteLine()
                    Exit Sub
                End If

                '------------------------------------------------------------
                'Declare that the RoleConstraint(Name) is an RoleConstraint
                '------------------------------------------------------------
                arORMWordVerbaliser.VerbaliseModelObject(Me)
                arORMWordVerbaliser.VerbaliseQuantifier(" is a Role Constraint")
                arORMWordVerbaliser.VerbaliseQuantifier(" (of type, 'Symmetric Ring Constraint')")
                arORMWordVerbaliser.WriteLine()
                arORMWordVerbaliser.WriteLine()

                If Me.IsDeontic Then
                    arORMWordVerbaliser.VerbaliseQuantifier("It is obligatory that if ")
                Else
                    arORMWordVerbaliser.VerbaliseQuantifier("If ")
                End If

                lrFactType = Me.RoleConstraintRole(0).Role.FactType
                If lrFactType.FactTypeReading.Count > 0 Then
                    Dim lrFactTypeReading As FBM.FactTypeReading = lrFactType.FactTypeReading(0)
                    Call lrFactTypeReading.Verbalise(arORMWordVerbaliser, "", pcenumFollowingThatOrSome.None, False, 1)
                Else
                    arORMWordVerbaliser.VerbaliseError("<Provide a Fact Type Reading for Fact Type, '" & lrFactType.Name & "', to complete this verbalisation>")
                    arORMWordVerbaliser.WriteLine()
                End If

                arORMWordVerbaliser.WriteLine()
                arORMWordVerbaliser.VerbaliseQuantifier(" then ")

                lrFactType = Me.RoleConstraintRole(0).Role.FactType
                If lrFactType.FactTypeReading.Count > 0 Then
                    Dim lrFactTypeReading As FBM.FactTypeReading = lrFactType.FactTypeReading(0)
                    Call lrFactTypeReading.Verbalise(arORMWordVerbaliser, "", pcenumFollowingThatOrSome.None, False, 2, Nothing, True)
                Else
                    arORMWordVerbaliser.VerbaliseError("<Provide a Fact Type Reading for Fact Type, '" & lrFactType.Name & "', to complete this verbalisation>")
                    arORMWordVerbaliser.WriteLine()
                End If

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Public Sub verbaliseRingConstraintSymmetricIntransitive(ByRef arORMWordVerbaliser As FBM.ORMWordVerbailser)

            Try
                Dim lrFactType As FBM.FactType

                If Me.RoleConstraintRole.Count = 0 Then
                    arORMWordVerbaliser.VerbaliseError("<Provide links to Roles for Role Constraint, '" & Me.Name & "', to complete this verbalisation>")
                    arORMWordVerbaliser.WriteLine()
                    Exit Sub
                End If

                '------------------------------------------------------------
                'Declare that the RoleConstraint(Name) is an RoleConstraint
                '------------------------------------------------------------
                arORMWordVerbaliser.VerbaliseModelObject(Me)
                arORMWordVerbaliser.VerbaliseQuantifier(" is a Role Constraint")
                arORMWordVerbaliser.VerbaliseQuantifier(" (of type, 'Symmetric Intransitive Ring Constraint')")
                arORMWordVerbaliser.WriteLine()
                arORMWordVerbaliser.WriteLine()

                If Me.IsDeontic Then
                    arORMWordVerbaliser.VerbaliseQuantifier("It is obligatory that if ")
                Else
                    arORMWordVerbaliser.VerbaliseQuantifier("If ")
                End If

                lrFactType = Me.RoleConstraintRole(0).Role.FactType
                If lrFactType.FactTypeReading.Count > 0 Then
                    Dim lrFactTypeReading As FBM.FactTypeReading = lrFactType.FactTypeReading(0)
                    Call lrFactTypeReading.Verbalise(arORMWordVerbaliser, "", pcenumFollowingThatOrSome.None, False, 1)
                Else
                    arORMWordVerbaliser.VerbaliseError("<Provide a Fact Type Reading for Fact Type, '" & lrFactType.Name & "', to complete this verbalisation>")
                    arORMWordVerbaliser.WriteLine()
                End If

                arORMWordVerbaliser.WriteLine()
                arORMWordVerbaliser.VerbaliseQuantifier(" then ")

                If lrFactType.FactTypeReading.Count > 0 Then
                    Dim lrFactTypeReading As FBM.FactTypeReading = lrFactType.FactTypeReading(0)
                    Call lrFactTypeReading.Verbalise(arORMWordVerbaliser, "", pcenumFollowingThatOrSome.None, False, 2, Nothing, True)
                Else
                    arORMWordVerbaliser.VerbaliseError("<Provide a Fact Type Reading for Fact Type, '" & lrFactType.Name & "', to complete this verbalisation>")
                    arORMWordVerbaliser.WriteLine()
                End If

                arORMWordVerbaliser.WriteLine()
                arORMWordVerbaliser.WriteLine()
                arORMWordVerbaliser.VerbaliseQuantifier("If ")

                If lrFactType.FactTypeReading.Count > 0 Then
                    Dim lrFactTypeReading As FBM.FactTypeReading = lrFactType.FactTypeReading(0)
                    Call lrFactTypeReading.Verbalise(arORMWordVerbaliser, "", pcenumFollowingThatOrSome.None, False, 1)
                Else
                    arORMWordVerbaliser.VerbaliseError("<Provide a Fact Type Reading for Fact Type, '" & lrFactType.Name & "', to complete this verbalisation>")
                    arORMWordVerbaliser.WriteLine()
                End If

                arORMWordVerbaliser.VerbaliseQuantifier(" and ")

                If lrFactType.FactTypeReading.Count > 0 Then
                    Dim lrFactTypeReading As FBM.FactTypeReading = lrFactType.FactTypeReading(0)
                    Call lrFactTypeReading.Verbalise(arORMWordVerbaliser, "", pcenumFollowingThatOrSome.None, False, 2)
                Else
                    arORMWordVerbaliser.VerbaliseError("<Provide a Fact Type Reading for Fact Type, '" & lrFactType.Name & "', to complete this verbalisation>")
                    arORMWordVerbaliser.WriteLine()
                End If
                'If Entity Type1 is subtype of Entity Type2 and Entity Type2 is subtype of Entity Type3

                If Me.IsDeontic Then
                    arORMWordVerbaliser.WriteLine()
                    arORMWordVerbaliser.VerbaliseQuantifier(" then it is forbidden that ")
                Else
                    arORMWordVerbaliser.WriteLine()
                    arORMWordVerbaliser.VerbaliseQuantifier(" then it is impossible that ")
                End If


                Dim lrSubscriptArray As New List(Of String)
                lrSubscriptArray.Add("1")
                lrSubscriptArray.Add("3")
                lrFactType = Me.RoleConstraintRole(0).Role.FactType
                If lrFactType.FactTypeReading.Count > 0 Then
                    Dim lrFactTypeReading As FBM.FactTypeReading = lrFactType.FactTypeReading(0)
                    Call lrFactTypeReading.Verbalise(arORMWordVerbaliser, "that ", pcenumFollowingThatOrSome.That, False, 1, lrSubscriptArray)
                Else
                    arORMWordVerbaliser.VerbaliseError("<Provide a Fact Type Reading for Fact Type, '" & lrFactType.Name & "', to complete this verbalisation>")
                    arORMWordVerbaliser.WriteLine()
                End If

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Public Sub verbaliseRingConstraintSymmetricIrreflexive(ByRef arORMWordVerbaliser As FBM.ORMWordVerbailser)

            Try
                Dim lrFactType As FBM.FactType

                If Me.RoleConstraintRole.Count = 0 Then
                    arORMWordVerbaliser.VerbaliseError("<Provide links to Roles for Role Constraint, '" & Me.Name & "', to complete this verbalisation>")
                    arORMWordVerbaliser.WriteLine()
                    Exit Sub
                End If

                '------------------------------------------------------------
                'Declare that the RoleConstraint(Name) is an RoleConstraint
                '------------------------------------------------------------
                arORMWordVerbaliser.VerbaliseModelObject(Me)
                arORMWordVerbaliser.VerbaliseQuantifier(" is a Role Constraint")
                arORMWordVerbaliser.VerbaliseQuantifier(" (of type, 'Symmetric Irreflexive Ring Constraint')")
                arORMWordVerbaliser.WriteLine()
                arORMWordVerbaliser.WriteLine()

                If Me.IsDeontic Then
                    arORMWordVerbaliser.VerbaliseQuantifier("It is obligatory that if ")
                Else
                    arORMWordVerbaliser.VerbaliseQuantifier("If ")
                End If

                lrFactType = Me.RoleConstraintRole(0).Role.FactType
                If lrFactType.FactTypeReading.Count > 0 Then
                    Dim lrFactTypeReading As FBM.FactTypeReading = lrFactType.FactTypeReading(0)
                    Call lrFactTypeReading.Verbalise(arORMWordVerbaliser, "", pcenumFollowingThatOrSome.None, False, 1)
                Else
                    arORMWordVerbaliser.VerbaliseError("<Provide a Fact Type Reading for Fact Type, '" & lrFactType.Name & "', to complete this verbalisation>")
                    arORMWordVerbaliser.WriteLine()
                End If

                arORMWordVerbaliser.WriteLine()
                arORMWordVerbaliser.VerbaliseQuantifier(" then ")

                If lrFactType.FactTypeReading.Count > 0 Then
                    Dim lrFactTypeReading As FBM.FactTypeReading = lrFactType.FactTypeReading(0)
                    Call lrFactTypeReading.Verbalise(arORMWordVerbaliser, "", pcenumFollowingThatOrSome.None, False, 2, Nothing, True)
                Else
                    arORMWordVerbaliser.VerbaliseError("<Provide a Fact Type Reading for Fact Type, '" & lrFactType.Name & "', to complete this verbalisation>")
                    arORMWordVerbaliser.WriteLine()
                End If

                arORMWordVerbaliser.WriteLine()

                If Me.IsDeontic Then
                    arORMWordVerbaliser.WriteLine()
                    arORMWordVerbaliser.VerbaliseQuantifier("It is obligatory that no ")
                Else
                    arORMWordVerbaliser.WriteLine()
                    arORMWordVerbaliser.VerbaliseQuantifier("No ")
                End If

                If lrFactType.FactTypeReading.Count > 0 Then
                    Dim lrFactTypeReading As FBM.FactTypeReading = lrFactType.FactTypeReading(0)
                    Call lrFactTypeReading.Verbalise(arORMWordVerbaliser, "", pcenumFollowingThatOrSome.TheSame, False)
                Else
                    arORMWordVerbaliser.VerbaliseError("<Provide a Fact Type Reading for Fact Type, '" & lrFactType.Name & "', to complete this verbalisation>")
                    arORMWordVerbaliser.WriteLine()
                End If

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Public Sub verbaliseSubsetConstraint(ByRef arORMWordVerbaliser As FBM.ORMWordVerbailser)

            Try
                If Me.RoleConstraintType <> pcenumRoleConstraintType.SubsetConstraint Then
                    Throw New Exception("Method called for Role Constraint that is actually a " & Me.RoleConstraintType.ToString & " constraint.")
                End If

                If Me.RoleConstraintRole.Count = 0 Then
                    arORMWordVerbaliser.VerbaliseError("<Provide links to Roles for Role Constraint, '" & Me.Name & "', to complete this verbalisation>")
                    arORMWordVerbaliser.WriteLine()
                    Exit Sub
                End If

                '------------------------------------------------------------
                'Declare that the RoleConstraint(Name) is an RoleConstraint
                '------------------------------------------------------------
                arORMWordVerbaliser.VerbaliseModelObject(Me)
                arORMWordVerbaliser.VerbaliseQuantifier(" is a Role Constraint")
                arORMWordVerbaliser.VerbaliseQuantifier(" (of type, 'Subset Constraint')")
                arORMWordVerbaliser.WriteLine()
                arORMWordVerbaliser.WriteLine()

                Dim larFactTypeReading As New List(Of FBM.FactTypeReading)
                Dim larReadingObjectType As New List(Of FBM.ModelObject)
                Dim larExitPredicatePart As New List(Of FBM.PredicatePart)
                Dim lbSomeUsedInExit As Boolean = False
                Dim lsPredicatePart As String = ""

                Dim liEntryCount = From rcr In Me.RoleConstraintRole _
                                   Where rcr.IsEntry = True _
                                   Select rcr Distinct.Count

                Dim liEntriesProcessed As Integer = 0

                arORMWordVerbaliser.VerbaliseQuantifier("If ")

                Dim lrArgument As FBM.RoleConstraintArgument

                If Me.Argument.Count = 0 Then
                    arORMWordVerbaliser.VerbaliseError("Complete the creation of the arguments for this Role Constraint.")
                Else
                    lrArgument = Me.getArgument(1) ' Argument(0)

                    Dim larModelObject As List(Of FBM.ModelObject)
                    larModelObject = Me.GetCommonArgumentModelObjects

                    Call lrArgument.ProjectArgumentReading(arORMWordVerbaliser, New List(Of FBM.ModelObject))

                    arORMWordVerbaliser.VerbaliseQuantifier(", then")
                    arORMWordVerbaliser.WriteLine()

                    If Me.Argument.Count > 1 Then
                        lrArgument = Me.getArgument(2)

                        Call lrArgument.ProjectArgumentReading(arORMWordVerbaliser, larModelObject)
                    Else
                        arORMWordVerbaliser.VerbaliseError("A Subset Role Constraint needs more than one Argument.")
                    End If
                End If

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

    End Class

End Namespace