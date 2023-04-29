Imports System.Reflection

Namespace FBM
    Partial Public Class RoleConstraint

        Public Sub GenerateReadingVerbalisation(ByRef arVerbaliser As FBM.ORMVerbailser)

            Try
                Select Case Me.RoleConstraintType
                    Case Is = pcenumRoleConstraintType.ExclusionConstraint
                        Call Me.verbaliseHTMLExclusionConstraint(arVerbaliser)
                    Case Is = pcenumRoleConstraintType.SubsetConstraint
                        Call Me.VerbaliseHTMLSubset(arVerbaliser)

                End Select

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
            End Try

        End Sub

        Private Sub verbaliseHTMLExclusionConstraint(ByRef arVerbaliser As FBM.ORMVerbailser)

            Try
                arVerbaliser.VerbaliseModelObject(Me)
                arVerbaliser.VerbaliseQuantifier(" is a Role Constraint")
                arVerbaliser.VerbaliseQuantifier(" (of type, 'Exclusion Role Constraint')")
                arVerbaliser.HTW.WriteBreak()
                arVerbaliser.HTW.WriteBreak()

                If Me.Argument.Count < 2 Then
                    arVerbaliser.VerbaliseError("Error: An Exclusion Role Constraint requires at least 2 Role Constraint Arguments.")
                    arVerbaliser.HTW.WriteBreak()
                    arVerbaliser.VerbaliseError("This Exclusion Role Constraint has " & Me.Argument.Count.ToString & " Role Constraint Arguments.")
                    arVerbaliser.HTW.WriteBreak()
                    arVerbaliser.HTW.WriteBreak()
                End If

                arVerbaliser.VerbaliseQuantifier("For each ")

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
                    arVerbaliser.VerbaliseModelObject(lrModelObject)
                    If liInd = larModelObject.Count - 1 Then
                        arVerbaliser.VerbaliseQuantifier(" and ")
                    ElseIf liInd = larModelObject.Count Then
                    Else
                        arVerbaliser.VerbaliseSeparator(",")
                    End If
                    liInd += 1
                Next
                arVerbaliser.VerbaliseQuantifier(" at most one of the following holds:")
                arVerbaliser.HTW.WriteBreak()

                Dim lrRoleConstraintArgument As FBM.RoleConstraintArgument

                For Each lrRoleConstraintArgument In Me.Argument
                    Call lrRoleConstraintArgument.ProjectArgumentReading(arVerbaliser, larModelObject)
                    arVerbaliser.HTW.WriteBreak()
                Next

                arVerbaliser.HTW.WriteBreak()
                arVerbaliser.HTW.WriteBreak()

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
            End Try
        End Sub

        Public Sub VerbaliseHTMLSubset(ByRef arVerbaliser As FBM.ORMVerbailser)

            Try

                If Me.RoleConstraintRole.Count = 0 Then
                    arVerbaliser.VerbaliseError("<Provide links to Roles for Role Constraint, '" & Me.Name & "', to complete this verbalisation>")
                    arVerbaliser.HTW.WriteBreak()
                    Exit Sub
                End If

                '------------------------------------------------------------
                'Declare that the RoleConstraint(Name) is an RoleConstraint
                '------------------------------------------------------------
                arVerbaliser.VerbaliseModelObject(Me)
                arVerbaliser.VerbaliseQuantifier(" is a Role Constraint")
                arVerbaliser.VerbaliseQuantifier(" (of type, 'Subset Constraint')")
                arVerbaliser.HTW.WriteBreak()
                arVerbaliser.HTW.WriteBreak()

                Dim larFactTypeReading As New List(Of FBM.FactTypeReading)
                Dim larReadingObjectType As New List(Of FBM.ModelObject)
                Dim larExitPredicatePart As New List(Of FBM.PredicatePart)
                Dim lbSomeUsedInExit As Boolean = False
                Dim lsPredicatePart As String = ""

                Dim liEntryCount = From rcr In Me.RoleConstraintRole
                                   Where rcr.IsEntry = True
                                   Select rcr Distinct.Count

                Dim liEntriesProcessed As Integer = 0

                arVerbaliser.VerbaliseQuantifier("If ")

                Dim lrArgument As FBM.RoleConstraintArgument

                If Me.Argument.Count = 0 Then
                    arVerbaliser.VerbaliseError("Complete the creation of the arguments for this Role Constraint.")
                Else
                    lrArgument = Me.getArgument(1) ' Argument(0)

                    Dim larModelObject As List(Of FBM.ModelObject)
                    larModelObject = Me.GetCommonArgumentModelObjects

                    Call lrArgument.ProjectArgumentReading(arVerbaliser, New List(Of FBM.ModelObject))

                    arVerbaliser.VerbaliseQuantifier(", then")
                    arVerbaliser.HTW.WriteBreak()

                    If Me.Argument.Count > 1 Then
                        lrArgument = Me.getArgument(2)

                        Call lrArgument.ProjectArgumentReading(arVerbaliser, larModelObject)
                    Else
                        arVerbaliser.VerbaliseError("A Subset Role Constraint needs more than one Argument.")
                    End If
                End If

                arVerbaliser.HTW.WriteBreak()
                arVerbaliser.HTW.WriteBreak()

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
