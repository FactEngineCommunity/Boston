'Imports WeifenLuo.WinFormsUI.Docking
Imports System.Drawing.Color
Imports System.Reflection

Public Class frmToolboxORMVerbalisation
    'Inherits DockContent

    Public zrModel As FBM.Model

    Public Function EqualsByName(ByVal other As Form) As Boolean
        If Me.Name = other.Name Then
            Return True
        Else
            Return False
        End If
    End Function

    Public Sub VerbaliseEntityType(ByVal arEntityType As FBM.EntityType)
        '------------------------------------------------------
        'PSEUDOCODE
        '  * Declare that the EntityType(Name) is an EntityType
        '  * Verbalise the ReferenceScheme
        '  * FOR EACH IncomingLink (from a Role)
        '      * Verbalise the FactType for the associated Role
        '  * LOOP 
        '------------------------------------------------------
        Dim lrVerbaliser As New FBM.ORMVerbailser

        Try
            Call lrVerbaliser.Reset()

            '------------------------------------------------------
            'Declare that the EntityType(Name) is an EntityType
            '------------------------------------------------------
            lrVerbaliser.VerbaliseModelObject(arEntityType)
            lrVerbaliser.VerbaliseQuantifier(" is an Entity Type.")
            lrVerbaliser.HTW.WriteBreak()
            If arEntityType.IsDerived Then
                lrVerbaliser.VerbaliseQuantifier("*")
                lrVerbaliser.VerbaliseQuantifier(arEntityType.DerivationText)
                lrVerbaliser.HTW.WriteBreak()
            End If
            lrVerbaliser.HTW.WriteBreak()

            If arEntityType.ShortDescription <> "" Or arEntityType.LongDescription <> "" Then

                lrVerbaliser.VerbaliseHeading("Informally:")
                lrVerbaliser.HTW.WriteBreak()
                lrVerbaliser.HTW.WriteBreak()

                If arEntityType.ShortDescription <> "" Then
                    lrVerbaliser.VerbaliseQuantifier("Short Description: ")
                    lrVerbaliser.VerbaliseQuantifierLight(arEntityType.ShortDescription)
                    lrVerbaliser.HTW.WriteBreak()
                    lrVerbaliser.HTW.WriteBreak()
                End If

                If arEntityType.LongDescription <> "" Then
                    lrVerbaliser.VerbaliseQuantifier("Long Description: ")
                    lrVerbaliser.VerbaliseQuantifierLight(arEntityType.LongDescription)
                    lrVerbaliser.HTW.WriteBreak()
                    lrVerbaliser.HTW.WriteBreak()
                End If

            End If

            '------------------------------
            'Verbalise the ReferenceScheme
            '------------------------------                   
            Dim lrTopmostSupertype As FBM.EntityType
            Dim lrFactType As FBM.FactType
            Dim liInd As Integer = 0

            If arEntityType.IsSubtype Then
                lrTopmostSupertype = arEntityType.GetTopmostSupertype(True)
            Else 'Is Not Subtype
                lrTopmostSupertype = arEntityType
            End If

            If arEntityType.HasCompoundReferenceMode Then
                lrVerbaliser.VerbaliseQuantifier("Reference Scheme: ")
                Dim lrEntityType As FBM.EntityType = arEntityType.GetTopmostNonAbsorbedSupertype(True)
                For Each lrRoleConstraintRole In lrEntityType.ReferenceModeRoleConstraint.RoleConstraintRole
                    lrFactType = lrRoleConstraintRole.Role.FactType
                    Dim larRole As New List(Of FBM.Role)
                    larRole.Add(lrFactType.GetOtherRoleOfBinaryFactType(lrRoleConstraintRole.Role.Id))
                    larRole.Add(lrRoleConstraintRole.Role)
                    Dim lrFactTypeReading As FBM.FactTypeReading
                    lrFactTypeReading = lrFactType.FindSuitableFactTypeReadingByRoles(larRole, True)
                    If lrFactTypeReading IsNot Nothing Then
                        Call lrFactTypeReading.GetReadingText(lrVerbaliser)
                    Else
                        lrVerbaliser.VerbaliseError("Provide a Fact Type Reading for the Fact Type:")
                        lrVerbaliser.VerbaliseModelObject(lrFactType)
                        lrVerbaliser.VerbaliseError(" with  " & lrRoleConstraintRole.Role.JoinedORMObject.Id & " at the last position in the reading.")
                    End If
                    If liInd < lrEntityType.ReferenceModeRoleConstraint.RoleConstraintRole.Count - 1 Then
                        lrVerbaliser.VerbaliseQuantifier(", ")
                    End If
                    liInd += 1
                Next
            ElseIf lrTopmostSupertype.HasSimpleReferenceScheme Then
                lrVerbaliser.VerbaliseQuantifier("Reference Scheme: ") ' & lrTopmostSupertype.Name & " has ")
                lrVerbaliser.VerbaliseModelObject(lrTopmostSupertype)
                lrVerbaliser.VerbaliseQuantifier(" has ")

                lrVerbaliser.VerbaliseModelObject(lrTopmostSupertype.ReferenceModeValueType)

                lrVerbaliser.HTW.WriteBreak()

                '----------------------------
                'Verbalise the ReferenceMode
                '----------------------------
                lrVerbaliser.VerbaliseQuantifier("Reference Mode: ")
                lrVerbaliser.VerbaliseQuantifier(lrTopmostSupertype.ReferenceMode)
            ElseIf lrTopmostSupertype.HasCompoundReferenceMode Then
                lrVerbaliser.VerbaliseQuantifier("Reference Scheme: ")
                For Each lrRoleConstraintRole In lrTopmostSupertype.ReferenceModeRoleConstraint.RoleConstraintRole
                    lrFactType = lrRoleConstraintRole.Role.FactType
                    Dim larRole As New List(Of FBM.Role)
                    larRole.Add(lrFactType.GetOtherRoleOfBinaryFactType(lrRoleConstraintRole.Role.Id))
                    larRole.Add(lrRoleConstraintRole.Role)
                    Dim lrFactTypeReading As FBM.FactTypeReading
                    lrFactTypeReading = lrFactType.FindSuitableFactTypeReadingByRoles(larRole, True)
                    If lrFactTypeReading IsNot Nothing Then
                        Call lrFactTypeReading.GetReadingText(lrVerbaliser)
                    Else
                        lrVerbaliser.VerbaliseError("Provide a Fact Type Reading for the Fact Type:")
                        lrVerbaliser.VerbaliseModelObject(lrFactType)
                        lrVerbaliser.VerbaliseError(" with  " & lrRoleConstraintRole.Role.JoinedORMObject.Id & " at the last position in the reading.")
                    End If
                    If liInd < lrTopmostSupertype.ReferenceModeRoleConstraint.RoleConstraintRole.Count - 1 Then
                        lrVerbaliser.VerbaliseQuantifier(", ")
                    End If
                    liInd += 1
                Next
            Else
                lrVerbaliser.VerbaliseError("Provide a Reference Mode for the Entity Type:")
                lrVerbaliser.VerbaliseModelObject(lrTopmostSupertype)
            End If

            lrVerbaliser.HTW.WriteBreak()
            lrVerbaliser.HTW.WriteBreak()

            '-------------------------------------------------
            'FOR EACH IncomingLink (from a Role)
            '  Verbalise the FactType for the associated Role
            'LOOP 
            '-------------------------------------------------

            lrVerbaliser.HTW.WriteBreak()
            lrVerbaliser.VerbaliseHeading("Fact Types:")
            lrVerbaliser.HTW.WriteBreak()
            lrVerbaliser.HTW.WriteBreak()

            '--------------------------------------
            'FactTypes
            Dim larFactType = zrModel.FactType.FindAll(Function(x) x.allRolesJoinSomething)

            Dim FactType = From ft In larFactType,
                                rl In ft.RoleGroup
                           Where rl.JoinedORMObject.Id = arEntityType.Id
                           Select ft Distinct

            For Each lrFactType In FactType

                lrVerbaliser.VerbaliseModelObjectLightGray(lrFactType)
                lrVerbaliser.HTW.WriteBreak()

                If lrFactType.FactTypeReading.Count > 0 Then
                    lrVerbaliser.HTW.Write(" (")
                    Call lrFactType.FactTypeReading(0).GetReadingText(lrVerbaliser)
                    lrVerbaliser.HTW.Write(") ")
                End If

                lrVerbaliser.HTW.WriteBreak()
            Next

            lrVerbaliser.HTW.WriteBreak()
            lrVerbaliser.VerbaliseQuantifier("Subtypes:")
            lrVerbaliser.HTW.WriteBreak()
            lrVerbaliser.HTW.WriteBreak()

            Dim lrModelObject As FBM.ModelObject
            For Each lrModelObject In arEntityType.childModelObjectList
                lrVerbaliser.VerbaliseModelObject(lrModelObject)
                lrVerbaliser.HTW.WriteBreak()
            Next

            lrVerbaliser.HTW.WriteBreak()
            lrVerbaliser.VerbaliseQuantifier("Supertypes:")
            lrVerbaliser.HTW.WriteBreak()
            lrVerbaliser.HTW.WriteBreak()

            For Each lrModelObject In arEntityType.parentModelObjectList
                lrVerbaliser.VerbaliseModelObject(lrModelObject)
                lrVerbaliser.HTW.WriteBreak()
            Next

            Me.WebBrowser.DocumentText = lrVerbaliser.Verbalise

            If Control.ModifierKeys = Keys.Control Then
                lrVerbaliser.HTW.WriteBreak()
                lrVerbaliser.VerbaliseQuantifier("Supertypes:")
                lrVerbaliser.HTW.WriteBreak()
                lrVerbaliser.HTW.WriteBreak()

                For Each lsInstance In arEntityType.Instance
                    lrVerbaliser.VerbalisePredicateText(lsInstance)
                    lrVerbaliser.HTW.WriteBreak()
                Next
            End If

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message & vbCrLf & ex.StackTrace
            lrVerbaliser.HTW.WriteBreak()
            lrVerbaliser.VerbaliseError(lsMessage)
            Me.WebBrowser.DocumentText = lrVerbaliser.Verbalise
        End Try

    End Sub

    Public Sub VerbaliseRoleConstraintInternalUniquenessConstraint(ByVal arRoleConstraint As FBM.RoleConstraint)

        Dim lrVerbaliser As New FBM.ORMVerbailser
        Call lrVerbaliser.Reset()
        Dim liInd As Integer
        Dim lrRole As FBM.Role
        Dim lrFactTypeReading As FBM.FactTypeReading

        Try
            Dim lrFactType As FBM.FactType = arRoleConstraint.Role(0).FactType

            '------------------------------------------------------------
            'Declare that the RoleConstraint(Name) is an RoleConstraint
            '------------------------------------------------------------
            lrVerbaliser.VerbaliseModelObject(arRoleConstraint)
            lrVerbaliser.VerbaliseQuantifier(" is a Role Constraint")
            lrVerbaliser.VerbaliseQuantifier(" (of type, 'Internal Uniqueness Constraint')")
            lrVerbaliser.HTW.WriteBreak()
            lrVerbaliser.HTW.WriteBreak()


            If lrFactType.HasTotalRoleConstraint Then

                lrVerbaliser.VerbaliseModelObject(arRoleConstraint)
                lrVerbaliser.VerbaliseQuantifier(" is a Total Internal Uniqueness Constraint.")
                lrVerbaliser.HTW.WriteBreak()
                lrVerbaliser.HTW.WriteBreak()
                lrVerbaliser.VerbaliseQuantifier("In each population of Fact Type, ")
                lrVerbaliser.VerbaliseModelObject(lrFactType)
                lrVerbaliser.VerbaliseQuantifier(", each ")

                liInd = 0
                For Each lrRole In lrFactType.RoleGroup
                    lrVerbaliser.VerbaliseModelObject(lrRole.JoinedORMObject)
                    liInd += 1
                    If liInd < lrFactType.Arity Then
                        lrVerbaliser.VerbaliseSeparator(", ")
                    End If
                Next

                lrVerbaliser.VerbaliseQuantifier(" combination occurs at most once.")
                lrVerbaliser.HTW.WriteBreak()
                lrVerbaliser.VerbaliseQuantifier("This association of ")

                liInd = 0
                For Each lrRole In lrFactType.RoleGroup
                    lrVerbaliser.HTW.Write(" ")
                    lrVerbaliser.VerbaliseModelObject(lrRole.JoinedORMObject)
                    If liInd < lrFactType.Arity Then
                        lrVerbaliser.VerbaliseSeparator(",")
                    End If
                Next

                lrVerbaliser.VerbaliseQuantifier(" provides the preferred identification scheme for ")
                lrVerbaliser.VerbaliseModelObject(lrFactType)
                lrVerbaliser.VerbaliseQuantifier(".")
                lrVerbaliser.HTW.WriteBreak()

            ElseIf lrFactType.Arity = 2 Then
                '--------------------
                'Is Binary FactType
                '--------------------
                Dim larRole As New List(Of FBM.Role)

                lrRole = arRoleConstraint.Role(0)
                larRole.Add(lrRole)

                    For Each lrRole In lrFactType.RoleGroup
                    If lrRole.Id <> arRoleConstraint.Role(0).Id Then
                        larRole.Add(lrRole)
                    End If
                Next

                    lrFactTypeReading = lrFactType.FindSuitableFactTypeReadingByRoles(larRole)

                    If IsSomething(lrFactTypeReading) Then
                        lrVerbaliser.VerbaliseIndent()
                        lrVerbaliser.VerbaliseQuantifier("Each ")
                        lrVerbaliser.VerbaliseModelObject(lrFactTypeReading.PredicatePart(0).Role.JoinedORMObject)

                    If arRoleConstraint.RoleConstraintRole(0).Role.Id = lrFactTypeReading.RoleList(0).Id Then
                        lrVerbaliser.VerbalisePredicateText(" " & lrFactTypeReading.PredicatePart(0).PredicatePartText)
                        If lrFactTypeReading.PredicatePart(0).Role.Mandatory Then
                            lrVerbaliser.VerbaliseQuantifier(" one ")
                        Else
                            lrVerbaliser.VerbaliseQuantifier(" at most one ")
                        End If
                        lrVerbaliser.VerbaliseModelObject(lrFactTypeReading.PredicatePart(1).Role.JoinedORMObject)
                    Else
                        If lrFactType.Is1To1BinaryFactType Then
                                lrVerbaliser.VerbalisePredicateText(" " & lrFactTypeReading.PredicatePart(0).PredicatePartText)
                                If lrFactTypeReading.PredicatePart(0).Role.Mandatory Then
                                    lrVerbaliser.VerbaliseQuantifier(" one ")
                                Else
                                    lrVerbaliser.VerbaliseQuantifier(" at most one ")
                                End If
                                lrVerbaliser.VerbaliseModelObject(lrFactTypeReading.PredicatePart(1).Role.JoinedORMObject)
                            Else
                                If lrFactTypeReading.PredicatePart(0).Role.Mandatory Then
                                    lrVerbaliser.VerbalisePredicateText(" " & lrFactTypeReading.PredicatePart(0).PredicatePartText)
                                    lrVerbaliser.VerbaliseQuantifier(" at least one ")
                                    lrVerbaliser.VerbaliseModelObject(lrFactTypeReading.PredicatePart(1).Role.JoinedORMObject)
                                Else
                                    lrVerbaliser.VerbaliseQuantifier(" possibly ")
                                    lrVerbaliser.VerbalisePredicateText(" " & lrFactTypeReading.PredicatePart(0).PredicatePartText)
                                    lrVerbaliser.VerbaliseQuantifier(" more than one ")
                                    lrVerbaliser.VerbaliseModelObject(lrFactTypeReading.PredicatePart(1).Role.JoinedORMObject)
                                End If
                            End If
                        End If
                    End If

                    If (lrFactType.FactTypeReading.Count = 2) Then
                        If Not lrFactType.Is1To1BinaryFactType Then
                            lrFactTypeReading = lrFactType.FactTypeReading.Find(Function(x) x.Id <> lrFactTypeReading.Id)
                            lrVerbaliser.HTW.WriteBreak()
                            lrVerbaliser.VerbaliseIndent()
                            lrVerbaliser.VerbaliseQuantifier("Each ")
                            lrVerbaliser.VerbaliseModelObject(lrFactTypeReading.PredicatePart(0).Role.JoinedORMObject)
                        If arRoleConstraint.RoleConstraintRole(0).Role.Id = lrFactTypeReading.RoleList(0).Id Then
                            lrVerbaliser.VerbalisePredicateText(" " & lrFactTypeReading.PredicatePart(0).PredicatePartText)
                            If lrFactTypeReading.PredicatePart(0).Role.Mandatory Then
                                lrVerbaliser.VerbaliseQuantifier(" one ")
                            Else
                                lrVerbaliser.VerbaliseQuantifier(" at most one ")
                            End If
                            lrVerbaliser.VerbaliseModelObject(lrFactTypeReading.PredicatePart(1).Role.JoinedORMObject)
                        Else
                            If lrFactTypeReading.PredicatePart(0).Role.Mandatory Then
                                    lrVerbaliser.VerbalisePredicateText(" " & lrFactTypeReading.PredicatePart(0).PredicatePartText)
                                    lrVerbaliser.VerbaliseQuantifier(" at least one ")
                                    lrVerbaliser.VerbaliseModelObject(lrFactTypeReading.PredicatePart(1).Role.JoinedORMObject)
                                Else
                                    lrVerbaliser.VerbaliseQuantifier(" possibly ")
                                    lrVerbaliser.VerbalisePredicateText(" " & lrFactTypeReading.PredicatePart(0).PredicatePartText)
                                    lrVerbaliser.VerbaliseQuantifier(" more than one ")
                                    lrVerbaliser.VerbaliseModelObject(lrFactTypeReading.PredicatePart(1).Role.JoinedORMObject)
                                End If
                            End If
                        End If
                    Else
                        lrVerbaliser.HTW.WriteBreak()
                        lrVerbaliser.VerbaliseIndent()
                        lrVerbaliser.VerbaliseError("Provide a reverse Fact Type Reading for this Fact Type")
                        lrVerbaliser.HTW.WriteBreak()
                        lrVerbaliser.HTW.WriteBreak()
                    End If

                lrVerbaliser.HTW.WriteBreak()
            Else
                '-------------------------------------------
                'Is Partial Internal Uniqueness Constraint
                '-------------------------------------------                
                lrVerbaliser.VerbaliseQuantifier("In each population of ")
                lrVerbaliser.VerbaliseModelObject(lrFactType)

                lrVerbaliser.VerbaliseQuantifier(" each")

                liInd = 0
                For Each lrRole In arRoleConstraint.Role
                    lrVerbaliser.HTW.Write(" ")
                    lrVerbaliser.VerbaliseModelObject(lrRole.JoinedORMObject)
                    liInd += 1
                    If liInd < arRoleConstraint.Role.Count Then
                        lrVerbaliser.VerbaliseSeparator(",")
                    End If
                Next
                lrVerbaliser.VerbaliseQuantifier(" combination ")

                    For Each lrRole In lrFactType.RoleGroup
                    If Not IsSomething(arRoleConstraint.Role.Find(AddressOf lrRole.Equals)) Then
                        lrVerbaliser.VerbaliseQuantifier("is unique and relates to exactly one instance of ")
                        lrVerbaliser.VerbaliseModelObject(lrRole.JoinedORMObject)
                    End If
                Next

                lrVerbaliser.HTW.WriteBreak()
            End If

            Me.WebBrowser.DocumentText = lrVerbaliser.Verbalise

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            lrVerbaliser.HTW.WriteBreak()
            lrVerbaliser.VerbaliseError(lsMessage)
            Me.WebBrowser.DocumentText = lrVerbaliser.Verbalise
        End Try


    End Sub

    Public Sub VerbaliseRoleConstraintUniquenessConstraint(ByVal arRoleConstraint As FBM.RoleConstraint)

        Dim lrFactType As FBM.FactType
        Dim lrVerbaliser As New FBM.ORMVerbailser
        Call lrVerbaliser.Reset()

        Try


            If arRoleConstraint.RoleConstraintRole.Count = 0 Then
                lrVerbaliser.VerbaliseError("<Provide links to Roles for Role Constraint, '" & arRoleConstraint.Name & "', to complete this verbalisation>")
                lrVerbaliser.HTW.WriteBreak()
                Exit Sub
            End If

            '------------------------------------------------------------
            'Declare that the RoleConstraint(Name) is an RoleConstraint
            '------------------------------------------------------------
            lrVerbaliser.VerbaliseModelObject(arRoleConstraint)
            lrVerbaliser.VerbaliseQuantifier(" is a Role Constraint")
            lrVerbaliser.VerbaliseQuantifier(" (of type, 'Uniqueness Constraint')")
            lrVerbaliser.HTW.WriteBreak()
            lrVerbaliser.HTW.WriteBreak()


            Dim liInd As Integer = 0
            Dim lrModelObject As FBM.ModelObject
            Dim lrRoleConstraintRole As FBM.RoleConstraintRole

            lrVerbaliser.VerbaliseQuantifier("For each ")

            Dim larModelObject = From RoleConstraintRole In arRoleConstraint.RoleConstraintRole
                                 Select RoleConstraintRole.Role.JoinedORMObject

            Dim lsPreboundReadingText As String = ""
            For Each lrRoleConstraintRole In arRoleConstraint.RoleConstraintRole
                lrFactType = lrRoleConstraintRole.Role.FactType
                lrModelObject = lrRoleConstraintRole.Role.JoinedORMObject
                If lrFactType.FactTypeReading.Count > 0 Then
                    lsPreboundReadingText = lrFactType.GetPreboundReadingTextForModelElementAtPosition(lrModelObject, 0)
                End If
                lrVerbaliser.VerbalisePredicateText(lsPreboundReadingText)
                lrVerbaliser.VerbaliseModelObject(lrModelObject)

                If liInd < larModelObject.Count - 1 Then
                    lrVerbaliser.VerbaliseQuantifier(" and ")
                End If
                liInd += 1
            Next

            lrVerbaliser.HTW.WriteBreak()
            lrVerbaliser.VerbaliseQuantifier(" at most one ")

            Dim larListedFactType As New List(Of FBM.FactType)
            Dim lbSkippedFactType As Boolean = False

            liInd = 0

            Dim lrCommonModelObject As FBM.ModelObject
            Dim lbFoundCommonModelObject As Boolean = False
            Dim larArgumentCommonModelObjects As New List(Of FBM.ModelObject)

            If arRoleConstraint.IsEachRoleFactTypeBinary Then
                If arRoleConstraint.DoesEachRoleFactTypeOppositeRoleJoinSameModelObject Then
                    lrFactType = arRoleConstraint.RoleConstraintRole(0).Role.FactType
                    lrCommonModelObject = lrFactType.GetOtherRoleOfBinaryFactType(arRoleConstraint.RoleConstraintRole(0).Role.Id).JoinedORMObject

                    larArgumentCommonModelObjects.Add(lrCommonModelObject)

                    lbFoundCommonModelObject = True
                End If
            Else
                For Each lrRole In arRoleConstraint.Role
                    If lrRole.FactType.IsBinaryFactType Then
                        lrCommonModelObject = lrRole.FactType.GetOtherRoleOfBinaryFactType(lrRole.Id).JoinedORMObject
                        larArgumentCommonModelObjects.Add(lrCommonModelObject)
                    ElseIf lrRole.FactType.IsObjectified Then
                        larArgumentCommonModelObjects.Add(lrRole.FactType)
                    End If
                Next

                If larArgumentCommonModelObjects.Count = 2 Then
                    Dim lasModelObjectId = From ModelObject In larArgumentCommonModelObjects
                                           Select ModelObject.Id Distinct

                    If lasModelObjectId.Count = 1 Then
                        lbFoundCommonModelObject = True
                        lrCommonModelObject = larArgumentCommonModelObjects(0)
                    End If
                End If
            End If

            'FactTypeReading(0).GetReadingTextThatOrSome(Me.JoinPath.RolePath, arVerbaliser, larArgumentCommonModelObjects, larVerbalisedModelObject)
            Dim lrFactTypeReading As FBM.FactTypeReading
            Dim larRole As New List(Of FBM.Role)
            For Each lrRoleConstraintRole In arRoleConstraint.RoleConstraintRole

                Dim lrRCRole As FBM.Role = lrRoleConstraintRole.Role
                larRole.Clear()

                If lrRoleConstraintRole.Role.FactType.IsObjectified Then

                    Dim lrLinkFactType = (From LinkFactType In arRoleConstraint.Model.FactType.FindAll(Function(x) x.IsLinkFactType)
                                          Where LinkFactType.LinkFactTypeRole Is lrRoleConstraintRole.Role
                                          Select LinkFactType).First
                    lrFactType = lrLinkFactType

                    Dim lrFirstRole As FBM.Role = lrLinkFactType.RoleGroup.Find(Function(x) x.JoinedORMObject Is lrRoleConstraintRole.Role.FactType)
                    larRole.Add(lrFirstRole)
                    larRole.Add(lrLinkFactType.GetOtherRoleOfBinaryFactType(lrFirstRole.Id))
                Else
                    lrFactType = lrRoleConstraintRole.Role.FactType
                    larRole.Add(lrRoleConstraintRole.Role.FactType.GetOtherRoleOfBinaryFactType(lrRoleConstraintRole.Role.Id))
                    larRole.Add(lrRoleConstraintRole.Role)
                End If


                lrFactTypeReading = lrFactType.FindSuitableFactTypeReadingByRoles(larRole, True)

                lbSkippedFactType = False

                If (liInd >= 1) And Not larListedFactType.Exists(AddressOf lrFactType.Equals) Then
                    lrVerbaliser.VerbaliseQuantifier(" and ")
                End If

                If lrFactTypeReading IsNot Nothing Then
                    lrFactTypeReading.GetReadingTextThatOrSome(larRole,
                                                               lrVerbaliser,
                                                               larArgumentCommonModelObjects,
                                                               New List(Of FBM.ModelObject),
                                                               liInd > 0,
                                                               Nothing,
                                                               True)
                Else
                    lrVerbaliser.VerbaliseError("Provide a Fact Type Reading for Fact Type, '" & lrFactType.Name & "', to complete this verbalisation")
                    lrVerbaliser.HTW.WriteBreak()
                End If

                If Not lbSkippedFactType Then
                    lrVerbaliser.HTW.WriteBreak()
                End If

                liInd += 1
            Next

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message

            lrVerbaliser.VerbaliseError(lsMessage)
        Finally
            Me.WebBrowser.DocumentText = lrVerbaliser.Verbalise
        End Try

    End Sub

    Public Sub VerbaliseRoleConstraintValueComparisonConstraint(ByVal arRoleConstraint As FBM.RoleConstraint)

        Dim lrFactType As FBM.FactType
        Dim lrVerbaliser As New FBM.ORMVerbailser
        Call lrVerbaliser.Reset()

        Try
            If arRoleConstraint.RoleConstraintRole.Count = 0 Then
                lrVerbaliser.VerbaliseError("<Provide links to Roles for Role Constraint, '" & arRoleConstraint.Name & "', to complete this verbalisation>")
                lrVerbaliser.HTW.WriteBreak()
                Exit Sub
            End If

            '------------------------------------------------------------
            'Declare that the RoleConstraint(Name) is an RoleConstraint
            '------------------------------------------------------------
            lrVerbaliser.VerbaliseModelObject(arRoleConstraint)
            lrVerbaliser.VerbaliseQuantifier(" is a Role Constraint")
            lrVerbaliser.VerbaliseQuantifier(" (of type, 'Value Comparison Constraint')")
            lrVerbaliser.HTW.WriteBreak()
            lrVerbaliser.HTW.WriteBreak()

            Dim liInd As Integer = 0
            Dim lrModelObject As FBM.ModelObject
            Dim lrRoleConstraintRole As FBM.RoleConstraintRole

            Dim lrCommonModelObject As FBM.ModelObject = Nothing
            Dim lbFoundCommonModelObject As Boolean = False
            Dim larArgumentCommonModelObjects As New List(Of FBM.ModelObject)

            If arRoleConstraint.IsEachRoleFactTypeBinary Then
                If arRoleConstraint.DoesEachRoleFactTypeOppositeRoleJoinSameModelObject Then
                    lrFactType = arRoleConstraint.RoleConstraintRole(0).Role.FactType
                    lrCommonModelObject = lrFactType.GetOtherRoleOfBinaryFactType(arRoleConstraint.RoleConstraintRole(0).Role.Id).JoinedORMObject

                    larArgumentCommonModelObjects.Add(lrCommonModelObject)

                    lbFoundCommonModelObject = True
                End If
            Else
                For Each lrRole In arRoleConstraint.Role
                    If lrRole.FactType.IsBinaryFactType Then
                        lrCommonModelObject = lrRole.FactType.GetOtherRoleOfBinaryFactType(lrRole.Id).JoinedORMObject
                        larArgumentCommonModelObjects.Add(lrCommonModelObject)
                    ElseIf lrRole.FactType.IsObjectified Then
                        larArgumentCommonModelObjects.Add(lrRole.FactType)
                    End If
                Next

                If larArgumentCommonModelObjects.Count = 2 Then
                    Dim lasModelObjectId = From ModelObject In larArgumentCommonModelObjects
                                           Select ModelObject.Id Distinct

                    If lasModelObjectId.Count = 1 Then
                        lbFoundCommonModelObject = True
                        lrCommonModelObject = larArgumentCommonModelObjects(0)
                    End If
                End If
            End If

            If lrCommonModelObject IsNot Nothing Then
                lrVerbaliser.VerbaliseQuantifier("For each ")
                lrVerbaliser.VerbaliseModelObject(lrCommonModelObject)
            Else
                'Nothing at this stage [20211120-VM].
            End If

            lrVerbaliser.HTW.WriteBreak()
            lrVerbaliser.VerbaliseQuantifier("If ")

            'Dim larModelObject = From RoleConstraintRole In arRoleConstraint.RoleConstraintRole
            '                     Select RoleConstraintRole.Role.JoinedORMObject

            'Dim lsPreboundReadingText As String = ""
            'For Each lrRoleConstraintRole In arRoleConstraint.RoleConstraintRole
            '    lrFactType = lrRoleConstraintRole.Role.FactType
            '    lrModelObject = lrRoleConstraintRole.Role.JoinedORMObject
            '    If lrFactType.FactTypeReading.Count > 0 Then
            '        lsPreboundReadingText = lrFactType.GetPreboundReadingTextForModelElementAtPosition(lrModelObject, 0)
            '    End If
            '    lrVerbaliser.VerbalisePredicateText(lsPreboundReadingText)
            '    lrVerbaliser.VerbaliseModelObject(lrModelObject)

            '    If liInd < larModelObject.Count - 1 Then
            '        lrVerbaliser.VerbaliseQuantifier(" and ")
            '    End If
            '    liInd += 1
            'Next

            'lrVerbaliser.HTW.WriteBreak()
            'lrVerbaliser.VerbaliseQuantifier(" at most one ")

            Dim larListedFactType As New List(Of FBM.FactType)
            Dim lbSkippedFactType As Boolean = False
            Dim lrFactTypeReading As FBM.FactTypeReading
            Dim larRole As New List(Of FBM.Role)
            liInd = 0

            For Each lrRoleConstraintRole In arRoleConstraint.RoleConstraintRole

                Dim lrRCRole As FBM.Role = lrRoleConstraintRole.Role
                larRole.Clear()

                If lrRoleConstraintRole.Role.FactType.IsObjectified Then

                    Dim lrLinkFactType = (From LinkFactType In arRoleConstraint.Model.FactType.FindAll(Function(x) x.IsLinkFactType)
                                          Where LinkFactType.LinkFactTypeRole Is lrRoleConstraintRole.Role
                                          Select LinkFactType).First
                    lrFactType = lrLinkFactType

                    Dim lrFirstRole As FBM.Role = lrLinkFactType.RoleGroup.Find(Function(x) x.JoinedORMObject Is lrRoleConstraintRole.Role.FactType)
                    larRole.Add(lrFirstRole)
                    larRole.Add(lrLinkFactType.GetOtherRoleOfBinaryFactType(lrFirstRole.Id))
                Else
                    lrFactType = lrRoleConstraintRole.Role.FactType
                    larRole.Add(lrRoleConstraintRole.Role.FactType.GetOtherRoleOfBinaryFactType(lrRoleConstraintRole.Role.Id))
                    larRole.Add(lrRoleConstraintRole.Role)
                End If


                lrFactTypeReading = lrFactType.FindSuitableFactTypeReadingByRoles(larRole, True)

                lbSkippedFactType = False

                If (liInd >= 1) And Not larListedFactType.Exists(AddressOf lrFactType.Equals) Then
                    lrVerbaliser.VerbaliseQuantifier(" and ")
                End If

                If lrFactTypeReading IsNot Nothing Then
                    lrFactTypeReading.GetReadingText(lrVerbaliser,
                                                     False,
                                                     False,
                                                     False,
                                                     lrRoleConstraintRole.Role,
                                                     liInd + 1)
                Else
                    lrVerbaliser.VerbaliseError("Provide a Fact Type Reading for Fact Type, '" & lrFactType.Name & "', to complete this verbalisation")
                    lrVerbaliser.HTW.WriteBreak()
                End If

                If Not lbSkippedFactType Then
                    lrVerbaliser.HTW.WriteBreak()
                End If

                liInd += 1
            Next

            lrVerbaliser.VerbaliseQuantifier("Then ")

            Dim lbRoleConstraintIsOverOneObjectType As Boolean = False
            If arRoleConstraint.Role(0).JoinedORMObject Is arRoleConstraint.Role(1).JoinedORMObject Then
                lbRoleConstraintIsOverOneObjectType = True
            End If

            lrVerbaliser.VerbaliseModelObject(arRoleConstraint.RoleConstraintRole(0).Role.JoinedORMObject)
            If lbRoleConstraintIsOverOneObjectType Then lrVerbaliser.VerbaliseSubscript("1")
            lrVerbaliser.VerbaliseQuantifier(" " & arRoleConstraint.ValueRangeType.DescriptionAttr & " ")
            lrVerbaliser.VerbaliseModelObject(arRoleConstraint.RoleConstraintRole(1).Role.JoinedORMObject)
            If lbRoleConstraintIsOverOneObjectType Then lrVerbaliser.VerbaliseSubscript("2")

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message

            lrVerbaliser.VerbaliseError(lsMessage)
        Finally
            Me.WebBrowser.DocumentText = lrVerbaliser.Verbalise
        End Try

    End Sub


    Public Sub VerbaliseRoleConstraintFrequencyConstraint(ByVal arRoleConstraint As FBM.RoleConstraint)

        Dim lrVerbaliser As New FBM.ORMVerbailser
        Call lrVerbaliser.Reset()

        Try

            '------------------------------------------------------------
            'Declare that the RoleConstraint(Name) is an RoleConstraint
            '------------------------------------------------------------
            lrVerbaliser.VerbaliseModelObject(arRoleConstraint)
            lrVerbaliser.VerbaliseQuantifier(" is a Role Constraint")
            lrVerbaliser.VerbaliseQuantifier(" (of type, 'Frequency Constraint')")
            lrVerbaliser.HTW.WriteBreak()
            lrVerbaliser.HTW.WriteBreak()

            Dim lrFactType As FBM.FactType = arRoleConstraint.Role(0).FactType

            Dim lrFactTypeReading As FBM.FactTypeReading = Nothing

            Dim larFactTypeReading = From FactTypeReading In lrFactType.FactTypeReading
                                     Where FactTypeReading.PredicatePart(0).Role.Id = arRoleConstraint.Role(0).Id
                                     Select FactTypeReading

            If larFactTypeReading.Count > 0 Then
                lrFactTypeReading = larFactTypeReading.First

                lrVerbaliser.VerbaliseQuantifier("Each ")

                Dim liInd As Integer = 1
                For Each lrPredicatePart In lrFactTypeReading.PredicatePart
                    lrVerbaliser.VerbaliseModelObject(lrPredicatePart.Role.JoinedORMObject)
                    lrVerbaliser.VerbalisePredicateText(" " & lrPredicatePart.PredicatePartText)
                    If liInd = 1 Then
                        lrVerbaliser.VerbaliseQuantifier(" from ")
                        lrVerbaliser.VerbaliseValue(arRoleConstraint.MinimumFrequencyCount)
                        lrVerbaliser.VerbaliseQuantifier(" to ")
                        lrVerbaliser.VerbaliseValue(arRoleConstraint.MaximumFrequencyCount)
                        lrVerbaliser.VerbaliseQuantifier(" instances of ")
                    End If
                    liInd += 1
                Next
            Else
                lrVerbaliser.VerbaliseError("Provide a Fact Type Reading that begins with " & arRoleConstraint.Role(0).JoinedORMObject.Id & ", for Fact Type: " & lrFactType.Id)
            End If

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        Finally
            Me.WebBrowser.DocumentText = lrVerbaliser.Verbalise
        End Try


    End Sub

    Public Sub VerbaliseRoleConstraintRingConstraintAntisymmetric(ByVal arRoleConstraint As FBM.RoleConstraint, Optional ByVal abDeontic As Boolean = False)

        Dim lrVerbaliser As New FBM.ORMVerbailser
        Call lrVerbaliser.Reset()

        Dim lrFactType As FBM.FactType

        If arRoleConstraint.RoleConstraintRole.Count = 0 Then
            lrVerbaliser.VerbaliseQuantifier("<Provide links to Roles for Role Constraint, '" & arRoleConstraint.Name & "', to complete this verbalisation>")
            lrVerbaliser.HTW.WriteBreak()
            Exit Sub
        End If

        '------------------------------------------------------------
        'Declare that the RoleConstraint(Name) is an RoleConstraint
        '------------------------------------------------------------
        lrVerbaliser.VerbaliseModelObject(arRoleConstraint)
        lrVerbaliser.VerbaliseQuantifier(" is a Role Constraint")
        lrVerbaliser.VerbaliseQuantifier(" (of type, 'Antisymmetric Ring Constraint')")
        lrVerbaliser.HTW.WriteBreak()
        lrVerbaliser.HTW.WriteBreak()

        lrVerbaliser.VerbaliseQuantifier("If ")

        lrFactType = arRoleConstraint.RoleConstraintRole(0).Role.FactType
        If lrFactType.FactTypeReading.Count > 0 Then            
            Call Me.VerbaliseFactTypePart(lrVerbaliser, lrFactType, Purple, Green, Nothing, pcenumFollowingThatOrSome.Either, False, 1)
        Else
            lrVerbaliser.VerbaliseError("<Provide a Fact Type Reading for Fact Type, '" & lrFactType.Name & "', to complete this verbalisation>")
            lrVerbaliser.HTW.WriteBreak()
        End If

        lrVerbaliser.VerbaliseQuantifier(" and ")

        lrVerbaliser.VerbaliseModelObject(arRoleConstraint.RoleConstraintRole(0).Role.JoinedORMObject)
        lrVerbaliser.HTW.Write(" ")
        lrVerbaliser.VerbaliseSubscript("1")
        lrVerbaliser.HTW.Write(" ")

        lrVerbaliser.VerbaliseQuantifier("is not ")

        lrVerbaliser.VerbaliseModelObject(arRoleConstraint.RoleConstraintRole(0).Role.JoinedORMObject)
        lrVerbaliser.HTW.Write(" ")
        lrVerbaliser.VerbaliseSubscript("2")
        lrVerbaliser.HTW.Write(" ")
        lrVerbaliser.HTW.WriteBreak()

        If abDeontic Then
            lrVerbaliser.VerbaliseQuantifier(" then it is forbidden that ")
        Else
            lrVerbaliser.VerbaliseQuantifier(" then it is impossible that ")
        End If

        lrFactType = arRoleConstraint.RoleConstraintRole(0).Role.FactType
        If lrFactType.FactTypeReading.Count > 0 Then            
            Call Me.VerbaliseFactTypePart(lrVerbaliser, lrFactType, Purple, Green, Nothing, pcenumFollowingThatOrSome.Either, False, 1, Nothing, True)
        Else
            lrVerbaliser.VerbaliseError("<Provide a Fact Type Reading for Fact Type, '" & lrFactType.Name & "', to complete this verbalisation>")
            lrVerbaliser.HTW.WriteBreak()
        End If

        Me.WebBrowser.DocumentText = lrVerbaliser.Verbalise

    End Sub

    Public Sub VerbaliseRoleConstraintRingConstraintAsymmetric(ByVal arRoleConstraint As FBM.RoleConstraint, Optional ByVal abDeontic As Boolean = False)

        Dim lrVerbaliser As New FBM.ORMVerbailser
        Call lrVerbaliser.Reset()

        Dim lrFactType As FBM.FactType

        If arRoleConstraint.RoleConstraintRole.Count = 0 Then
            lrVerbaliser.VerbaliseError("<Provide links to Roles for Role Constraint, '" & arRoleConstraint.Name & "', to complete this verbalisation>")
            lrVerbaliser.HTW.WriteBreak()
            Exit Sub
        End If

        '------------------------------------------------------------
        'Declare that the RoleConstraint(Name) is an RoleConstraint
        '------------------------------------------------------------
        lrVerbaliser.VerbaliseModelObject(arRoleConstraint)
        lrVerbaliser.VerbaliseQuantifier(" is a Role Constraint")
        lrVerbaliser.VerbaliseQuantifier(" (of type, 'Ring Constraint')")
        lrVerbaliser.HTW.WriteBreak()
        lrVerbaliser.HTW.WriteBreak()

        lrVerbaliser.VerbaliseQuantifier("If ")
        lrFactType = arRoleConstraint.RoleConstraintRole(0).Role.FactType
        If lrFactType.FactTypeReading.Count > 0 Then
            Call Me.VerbaliseFactTypePart(lrVerbaliser, lrFactType, Purple, Green, "", pcenumFollowingThatOrSome.None, False, 1)
        Else
            lrVerbaliser.VerbaliseError("<Provide a Fact Type Reading for Fact Type, '" & lrFactType.Name & "', to complete this verbalisation>")
            lrVerbaliser.HTW.WriteBreak()
        End If

        If abDeontic Then
            lrVerbaliser.HTW.WriteBreak()
            lrVerbaliser.VerbaliseQuantifier(" then it is forbidden that ")
        Else
            lrVerbaliser.HTW.WriteBreak()
            lrVerbaliser.VerbaliseQuantifier(" then it is impossible that ")
        End If

        lrFactType = arRoleConstraint.RoleConstraintRole(0).Role.FactType
        If lrFactType.FactTypeReading.Count > 0 Then            
            Call Me.VerbaliseFactTypePart(lrVerbaliser, lrFactType, Purple, Green, "", pcenumFollowingThatOrSome.None, False, 2, Nothing, True)
        Else
            lrVerbaliser.VerbaliseError("<Provide a Fact Type Reading for Fact Type, '" & lrFactType.Name & "', to complete this verbalisation>")
            lrVerbaliser.HTW.WriteBreak()
        End If

        Me.WebBrowser.DocumentText = lrVerbaliser.Verbalise

    End Sub

    Public Sub VerbaliseRoleConstraintRingConstraintAsymmetricIntransitive(ByVal arRoleConstraint As FBM.RoleConstraint, Optional ByVal abDeontic As Boolean = False)

        Dim lrVerbaliser As New FBM.ORMVerbailser
        Call lrVerbaliser.Reset()

        Dim lrFactType As FBM.FactType

        If arRoleConstraint.RoleConstraintRole.Count = 0 Then
            lrVerbaliser.VerbaliseError("<Provide links to Roles for Role Constraint, '" & arRoleConstraint.Name & "', to complete this verbalisation>")
            lrVerbaliser.HTW.WriteBreak()
            Exit Sub
        End If

        '------------------------------------------------------------
        'Declare that the RoleConstraint(Name) is an RoleConstraint
        '------------------------------------------------------------
        lrVerbaliser.VerbaliseModelObject(arRoleConstraint)
        lrVerbaliser.VerbaliseQuantifier(" is a Role Constraint")
        lrVerbaliser.VerbaliseQuantifier(" (of type, 'Asymmetric Intransitive Ring Constraint')")
        lrVerbaliser.HTW.WriteBreak()
        lrVerbaliser.HTW.WriteBreak()

        lrVerbaliser.VerbaliseQuantifier("If ")

        lrFactType = arRoleConstraint.RoleConstraintRole(0).Role.FactType
        If lrFactType.FactTypeReading.Count > 0 Then
            Call Me.VerbaliseFactTypePart(lrVerbaliser, lrFactType, Purple, Green, "", pcenumFollowingThatOrSome.None, False, 1)
        Else
            lrVerbaliser.VerbaliseError("<Provide a Fact Type Reading for Fact Type, '" & lrFactType.Name & "', to complete this verbalisation>")
            lrVerbaliser.HTW.WriteBreak()
        End If

        If abDeontic Then
            lrVerbaliser.HTW.WriteBreak()
            lrVerbaliser.VerbaliseQuantifier(" then it is forbidden that ")
        Else
            lrVerbaliser.HTW.WriteBreak()
            lrVerbaliser.VerbaliseQuantifier(" then it is impossible that ")
        End If

        lrFactType = arRoleConstraint.RoleConstraintRole(0).Role.FactType
        If lrFactType.FactTypeReading.Count > 0 Then
            Call Me.VerbaliseFactTypePart(lrVerbaliser, lrFactType, Purple, Green, "", pcenumFollowingThatOrSome.None, False, 2, Nothing, True)
        Else
            lrVerbaliser.VerbaliseError("<Provide a Fact Type Reading for Fact Type, '" & lrFactType.Name & "', to complete this verbalisation>")
            lrVerbaliser.HTW.WriteBreak()
        End If

        lrVerbaliser.HTW.WriteBreak()
        lrVerbaliser.VerbaliseQuantifier("If ")

        lrFactType = arRoleConstraint.RoleConstraintRole(0).Role.FactType
        If lrFactType.FactTypeReading.Count > 0 Then
            Call Me.VerbaliseFactTypePart(lrVerbaliser, lrFactType, Purple, Green, "", pcenumFollowingThatOrSome.None, False, 1)
        Else
            lrVerbaliser.VerbaliseError("<Provide a Fact Type Reading for Fact Type, '" & lrFactType.Name & "', to complete this verbalisation>")
            lrVerbaliser.HTW.WriteBreak()
        End If

        lrVerbaliser.VerbaliseQuantifier(" and ")

        lrFactType = arRoleConstraint.RoleConstraintRole(0).Role.FactType
        If lrFactType.FactTypeReading.Count > 0 Then
            Call Me.VerbaliseFactTypePart(lrVerbaliser, lrFactType, Purple, Green, "", pcenumFollowingThatOrSome.None, False, 2)
        Else
            lrVerbaliser.VerbaliseError("<Provide a Fact Type Reading for Fact Type, '" & lrFactType.Name & "', to complete this verbalisation>")
            lrVerbaliser.HTW.WriteBreak()
        End If

        'If Entity Type1 is subtype of Entity Type2 and Entity Type2 is subtype of Entity Type3
        If abDeontic Then
            lrVerbaliser.HTW.WriteBreak()
            lrVerbaliser.VerbaliseQuantifier(" then it is forbidden that ")
        Else
            lrVerbaliser.HTW.WriteBreak()
            lrVerbaliser.VerbaliseQuantifier(" then it is impossible that ")
        End If

        Dim lrSubscriptArray As New List(Of String)
        lrSubscriptArray.Add("1")
        lrSubscriptArray.Add("3")
        lrFactType = arRoleConstraint.RoleConstraintRole(0).Role.FactType
        If lrFactType.FactTypeReading.Count > 0 Then
            Call Me.VerbaliseFactTypePart(lrVerbaliser, lrFactType, Purple, Green, "that ", pcenumFollowingThatOrSome.That, False, 1, lrSubscriptArray)
        Else
            lrVerbaliser.VerbaliseError("<Provide a Fact Type Reading for Fact Type, '" & lrFactType.Name & "', to complete this verbalisation>")
            lrVerbaliser.HTW.WriteBreak()
        End If

        Me.WebBrowser.DocumentText = lrVerbaliser.Verbalise

    End Sub

    Public Sub VerbaliseRoleConstraintRingConstraintIntransitive(ByVal arRoleConstraint As FBM.RoleConstraint, Optional ByVal abDeontic As Boolean = False)

        Dim lrVerbaliser As New FBM.ORMVerbailser
        Call lrVerbaliser.Reset()

        Dim lrFactType As FBM.FactType

        If arRoleConstraint.RoleConstraintRole.Count = 0 Then
            lrVerbaliser.VerbaliseError("<Provide links to Roles for Role Constraint, '" & arRoleConstraint.Name & "', to complete this verbalisation>")
            lrVerbaliser.HTW.WriteBreak()
            Exit Sub
        End If

        '------------------------------------------------------------
        'Declare that the RoleConstraint(Name) is an RoleConstraint
        '------------------------------------------------------------
        lrVerbaliser.VerbaliseModelObject(arRoleConstraint)
        lrVerbaliser.VerbaliseQuantifier(" is a Role Constraint")
        lrVerbaliser.VerbaliseQuantifier(" (of type, 'Intransitive Ring Constraint')")
        lrVerbaliser.HTW.WriteBreak()
        lrVerbaliser.HTW.WriteBreak()

        lrVerbaliser.VerbaliseQuantifier("If ")

        lrFactType = arRoleConstraint.RoleConstraintRole(0).Role.FactType
        If lrFactType.FactTypeReading.Count > 0 Then
            Call Me.VerbaliseFactTypePart(lrVerbaliser, lrFactType, Purple, Green, "", pcenumFollowingThatOrSome.None, False, 1)
        Else
            lrVerbaliser.VerbaliseError("<Provide a Fact Type Reading for Fact Type, '" & lrFactType.Name & "', to complete this verbalisation>")
            lrVerbaliser.HTW.WriteBreak()
        End If

        lrVerbaliser.VerbaliseQuantifier(" and ")

        lrFactType = arRoleConstraint.RoleConstraintRole(0).Role.FactType
        If lrFactType.FactTypeReading.Count > 0 Then
            Call Me.VerbaliseFactTypePart(lrVerbaliser, lrFactType, Purple, Green, "", pcenumFollowingThatOrSome.None, False, 2)
        Else
            lrVerbaliser.VerbaliseError("<Provide a Fact Type Reading for Fact Type, '" & lrFactType.Name & "', to complete this verbalisation>")
            lrVerbaliser.HTW.WriteBreak()
        End If
        'If Entity Type1 is subtype of Entity Type2 and Entity Type2 is subtype of Entity Type3

        If abDeontic Then
            lrVerbaliser.HTW.WriteBreak()
            lrVerbaliser.VerbaliseQuantifier(" then it is forbidden that ")
        Else
            lrVerbaliser.HTW.WriteBreak()
            lrVerbaliser.VerbaliseQuantifier(" then it is impossible that ")
        End If


        Dim lrSubscriptArray As New List(Of String)
        lrSubscriptArray.Add("1")
        lrSubscriptArray.Add("3")
        lrFactType = arRoleConstraint.RoleConstraintRole(0).Role.FactType
        If lrFactType.FactTypeReading.Count > 0 Then
            Call Me.VerbaliseFactTypePart(lrVerbaliser, lrFactType, Purple, Green, "that ", pcenumFollowingThatOrSome.That, False, 1, lrSubscriptArray)
        Else
            lrVerbaliser.VerbaliseError("<Provide a Fact Type Reading for Fact Type, '" & lrFactType.Name & "', to complete this verbalisation>")
            lrVerbaliser.HTW.WriteBreak()
        End If

        Me.WebBrowser.DocumentText = lrVerbaliser.Verbalise

    End Sub

    Public Sub VerbaliseRoleConstraintRingConstraintIrreflexive(ByVal arRoleConstraint As FBM.RoleConstraint, Optional ByVal abDeontic As Boolean = False)

        Dim lrVerbaliser As New FBM.ORMVerbailser
        Call lrVerbaliser.Reset()

        Dim lrFactType As FBM.FactType

        If arRoleConstraint.RoleConstraintRole.Count = 0 Then
            lrVerbaliser.VerbaliseError("<Provide links to Roles for Role Constraint, '" & arRoleConstraint.Name & "', to complete this verbalisation>")
            lrVerbaliser.HTW.WriteBreak()
            Exit Sub
        End If

        '------------------------------------------------------------
        'Declare that the RoleConstraint(Name) is an RoleConstraint
        '------------------------------------------------------------
        lrVerbaliser.VerbaliseModelObject(arRoleConstraint)
        lrVerbaliser.VerbaliseQuantifier(" is a Role Constraint")
        lrVerbaliser.VerbaliseQuantifier(" (of type, 'Ring Constraint')")
        lrVerbaliser.HTW.WriteBreak()
        lrVerbaliser.HTW.WriteBreak()

        If abDeontic Then
            lrVerbaliser.VerbaliseQuantifier("It is obligatory that no ")
        Else
            lrVerbaliser.VerbaliseQuantifier("No ")
        End If

        lrFactType = arRoleConstraint.RoleConstraintRole(0).Role.FactType
        If lrFactType.FactTypeReading.Count > 0 Then
            Call Me.VerbaliseFactTypePart(lrVerbaliser, lrFactType, Purple, Green, "", pcenumFollowingThatOrSome.TheSame, False)
        Else
            lrVerbaliser.VerbaliseError("<Provide a Fact Type Reading for Fact Type, '" & lrFactType.Name & "', to complete this verbalisation>")
            lrVerbaliser.HTW.WriteBreak()
        End If

        Me.WebBrowser.DocumentText = lrVerbaliser.Verbalise

    End Sub

    Public Sub VerbaliseRoleConstraintRingConstraintPurelyReflexive(ByVal arRoleConstraint As FBM.RoleConstraint, Optional ByVal abDeontic As Boolean = False)

        'VxVy(xRy -> x = xy)

        Dim lrVerbaliser As New FBM.ORMVerbailser
        Call lrVerbaliser.Reset()

        Dim lrFactType As FBM.FactType

        If arRoleConstraint.RoleConstraintRole.Count = 0 Then
            lrVerbaliser.VerbaliseError("<Provide links to Roles for Role Constraint, '" & arRoleConstraint.Name & "', to complete this verbalisation>")
            lrVerbaliser.HTW.WriteBreak()
            Exit Sub
        End If

        '------------------------------------------------------------
        'Declare that the RoleConstraint(Name) is an RoleConstraint
        '------------------------------------------------------------
        lrVerbaliser.VerbaliseModelObject(arRoleConstraint)
        lrVerbaliser.VerbaliseQuantifier(" is a Role Constraint")
        lrVerbaliser.VerbaliseQuantifier(" (of type, 'Purely Reflexive Ring Constraint')")
        lrVerbaliser.HTW.WriteBreak()
        lrVerbaliser.HTW.WriteBreak()

        If abDeontic Then
            lrVerbaliser.VerbaliseQuantifier("It is obligatory that if ")
        Else
            lrVerbaliser.VerbaliseQuantifier("If ")
        End If

        lrFactType = arRoleConstraint.RoleConstraintRole(0).Role.FactType
        If lrFactType.FactTypeReading.Count > 0 Then
            Call Me.VerbaliseFactTypePart(lrVerbaliser, lrFactType, Purple, Green, Nothing, pcenumFollowingThatOrSome.None, False, 1)
        Else
            lrVerbaliser.VerbaliseError("<Provide a Fact Type Reading for Fact Type, '" & lrFactType.Name & "', to complete this verbalisation>")
            lrVerbaliser.HTW.WriteBreak()
        End If

        lrVerbaliser.VerbaliseQuantifier(" then ")
        lrVerbaliser.VerbaliseModelObject(arRoleConstraint.RoleConstraintRole(0).Role.JoinedORMObject)
        lrVerbaliser.VerbaliseSubscript("1")

        lrVerbaliser.VerbaliseQuantifier("is ")
        lrVerbaliser.VerbaliseModelObject(arRoleConstraint.RoleConstraintRole(0).Role.JoinedORMObject)
        lrVerbaliser.VerbaliseSubscript("2")

        Me.WebBrowser.DocumentText = lrVerbaliser.Verbalise
    End Sub

    Public Sub VerbaliseRoleConstraintRingConstraintSymmetric(ByVal arRoleConstraint As FBM.RoleConstraint, Optional ByVal abDeontic As Boolean = False)

        Dim lrFactType As FBM.FactType
        Dim lrVerbaliser As New FBM.ORMVerbailser

        Call lrVerbaliser.Reset()

        If arRoleConstraint.RoleConstraintRole.Count = 0 Then
            lrVerbaliser.VerbaliseError("<Provide links to Roles for Role Constraint, '" & arRoleConstraint.Name & "', to complete this verbalisation>")
            lrVerbaliser.HTW.WriteBreak()
            Exit Sub
        End If

        '------------------------------------------------------------
        'Declare that the RoleConstraint(Name) is an RoleConstraint
        '------------------------------------------------------------
        lrVerbaliser.VerbaliseModelObject(arRoleConstraint)
        lrVerbaliser.VerbaliseQuantifier(" is a Role Constraint")
        lrVerbaliser.VerbaliseQuantifier(" (of type, 'Symmetric Ring Constraint')")
        lrVerbaliser.HTW.WriteBreak()
        lrVerbaliser.HTW.WriteBreak()

        If abDeontic Then
            lrVerbaliser.VerbaliseQuantifier("It is obligatory that if ")
        Else
            lrVerbaliser.VerbaliseQuantifier("If ")
        End If

        lrFactType = arRoleConstraint.RoleConstraintRole(0).Role.FactType
        If lrFactType.FactTypeReading.Count > 0 Then
            Call Me.VerbaliseFactTypePart(lrVerbaliser, lrFactType, Purple, Green, "", pcenumFollowingThatOrSome.None, False, 1)
        Else
            lrVerbaliser.VerbaliseError("<Provide a Fact Type Reading for Fact Type, '" & lrFactType.Name & "', to complete this verbalisation>")
            lrVerbaliser.HTW.WriteBreak()
        End If

        lrVerbaliser.HTW.WriteBreak()
        lrVerbaliser.VerbaliseQuantifier(" then ")

        lrFactType = arRoleConstraint.RoleConstraintRole(0).Role.FactType
        If lrFactType.FactTypeReading.Count > 0 Then
            Call Me.VerbaliseFactTypePart(lrVerbaliser, lrFactType, Purple, Green, "", pcenumFollowingThatOrSome.None, False, 2, Nothing, True)
        Else
            lrVerbaliser.VerbaliseError("<Provide a Fact Type Reading for Fact Type, '" & lrFactType.Name & "', to complete this verbalisation>")
            lrVerbaliser.HTW.WriteBreak()
        End If

        Me.WebBrowser.DocumentText = lrVerbaliser.Verbalise

    End Sub

    Public Sub VerbaliseRoleConstraintRingConstraintSymmetricTransitive(ByVal arRoleConstraint As FBM.RoleConstraint, Optional ByVal abDeontic As Boolean = False)

        Try
            '            If InstructionType1 Is allowed Then To be used In conjunction With InstructionType2
            'then InstructionType2 Is allowed to be used in conjunction with InstructionType1.
            'If InstructionType1 Is allowed Then To be used In conjunction With InstructionType2 And InstructionType2 Is allowed To be used In conjunction With InstructionType3
            'then InstructionType1 Is allowed to be used in conjunction with InstructionType3.

            Dim lrFactType As FBM.FactType
            Dim lrVerbaliser As New FBM.ORMVerbailser

            Call lrVerbaliser.Reset()

            If arRoleConstraint.RoleConstraintRole.Count = 0 Then
                lrVerbaliser.VerbaliseError("<Provide links to Roles for Role Constraint, '" & arRoleConstraint.Name & "', to complete this verbalisation>")
                lrVerbaliser.HTW.WriteBreak()
                Exit Sub
            End If

            '------------------------------------------------------------
            'Declare that the RoleConstraint(Name) is an RoleConstraint
            '------------------------------------------------------------
            lrVerbaliser.VerbaliseModelObject(arRoleConstraint)
            lrVerbaliser.VerbaliseQuantifier(" is a Role Constraint")
            lrVerbaliser.VerbaliseQuantifier(" (of type, 'Symmetric Intransitive Ring Constraint')")
            lrVerbaliser.HTW.WriteBreak()
            lrVerbaliser.HTW.WriteBreak()

            If abDeontic Then
                lrVerbaliser.VerbaliseQuantifier("It is obligatory that if ")
            Else
                lrVerbaliser.VerbaliseQuantifier("If ")
            End If

            lrFactType = arRoleConstraint.RoleConstraintRole(0).Role.FactType
            If lrFactType.FactTypeReading.Count > 0 Then
                Call Me.VerbaliseFactTypePart(lrVerbaliser, lrFactType, Purple, Green, "", pcenumFollowingThatOrSome.None, False, 1)
            Else
                lrVerbaliser.VerbaliseError("<Provide a Fact Type Reading for Fact Type, '" & lrFactType.Name & "', to complete this verbalisation>")
                lrVerbaliser.HTW.WriteBreak()
            End If

            lrVerbaliser.HTW.WriteBreak()
            lrVerbaliser.VerbaliseQuantifier(" then ")

            If lrFactType.FactTypeReading.Count > 0 Then
                Call Me.VerbaliseFactTypePart(lrVerbaliser, lrFactType, Purple, Green, "", pcenumFollowingThatOrSome.None, False, 2, Nothing, True)
            Else
                lrVerbaliser.VerbaliseError("<Provide a Fact Type Reading for Fact Type, '" & lrFactType.Name & "', to complete this verbalisation>")
                lrVerbaliser.HTW.WriteBreak()
            End If

            lrVerbaliser.HTW.WriteBreak()
            lrVerbaliser.HTW.WriteBreak()
            lrVerbaliser.VerbaliseQuantifier("If ")

            If lrFactType.FactTypeReading.Count > 0 Then
                Call Me.VerbaliseFactTypePart(lrVerbaliser, lrFactType, Purple, Green, "", pcenumFollowingThatOrSome.None, False, 1)
            Else
                lrVerbaliser.VerbaliseError("<Provide a Fact Type Reading for Fact Type, '" & lrFactType.Name & "', to complete this verbalisation>")
                lrVerbaliser.HTW.WriteBreak()
            End If

            lrVerbaliser.VerbaliseQuantifier(" and ")

            If lrFactType.FactTypeReading.Count > 0 Then
                Call Me.VerbaliseFactTypePart(lrVerbaliser, lrFactType, Purple, Green, "", pcenumFollowingThatOrSome.None, False, 2)
            Else
                lrVerbaliser.VerbaliseError("<Provide a Fact Type Reading for Fact Type, '" & lrFactType.Name & "', to complete this verbalisation>")
                lrVerbaliser.HTW.WriteBreak()
            End If
            'If Entity Type1 is subtype of Entity Type2 and Entity Type2 is subtype of Entity Type3

            If abDeontic Then
                lrVerbaliser.HTW.WriteBreak()
                lrVerbaliser.VerbaliseQuantifier(" then ")
            Else
                lrVerbaliser.HTW.WriteBreak()
                lrVerbaliser.VerbaliseQuantifier(" then ")
            End If


            Dim lrSubscriptArray As New List(Of String)
            lrSubscriptArray.Add("1")
            lrSubscriptArray.Add("3")
            lrFactType = arRoleConstraint.RoleConstraintRole(0).Role.FactType
            If lrFactType.FactTypeReading.Count > 0 Then
                Call Me.VerbaliseFactTypePart(lrVerbaliser, lrFactType, Purple, Green, "that ", pcenumFollowingThatOrSome.That, False, 1, lrSubscriptArray)
            Else
                lrVerbaliser.VerbaliseError("<Provide a Fact Type Reading for Fact Type, '" & lrFactType.Name & "', to complete this verbalisation>")
                lrVerbaliser.HTW.WriteBreak()
            End If

            Me.WebBrowser.DocumentText = lrVerbaliser.Verbalise


        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message

        End Try

    End Sub

    Public Sub VerbaliseRoleConstraintRingConstraintSymmetricIntransitive(ByVal arRoleConstraint As FBM.RoleConstraint, Optional ByVal abDeontic As Boolean = False)

        Dim lrFactType As FBM.FactType
        Dim lrVerbaliser As New FBM.ORMVerbailser

        Call lrVerbaliser.Reset()

        If arRoleConstraint.RoleConstraintRole.Count = 0 Then
            lrVerbaliser.VerbaliseError("<Provide links to Roles for Role Constraint, '" & arRoleConstraint.Name & "', to complete this verbalisation>")
            lrVerbaliser.HTW.WriteBreak()
            Exit Sub
        End If

        '------------------------------------------------------------
        'Declare that the RoleConstraint(Name) is an RoleConstraint
        '------------------------------------------------------------
        lrVerbaliser.VerbaliseModelObject(arRoleConstraint)
        lrVerbaliser.VerbaliseQuantifier(" is a Role Constraint")
        lrVerbaliser.VerbaliseQuantifier(" (of type, 'Symmetric Intransitive Ring Constraint')")
        lrVerbaliser.HTW.WriteBreak()
        lrVerbaliser.HTW.WriteBreak()

        If abDeontic Then
            lrVerbaliser.VerbaliseQuantifier("It is obligatory that if ")
        Else
            lrVerbaliser.VerbaliseQuantifier("If ")
        End If

        lrFactType = arRoleConstraint.RoleConstraintRole(0).Role.FactType
        If lrFactType.FactTypeReading.Count > 0 Then
            Call Me.VerbaliseFactTypePart(lrVerbaliser, lrFactType, Purple, Green, "", pcenumFollowingThatOrSome.None, False, 1)
        Else
            lrVerbaliser.VerbaliseError("<Provide a Fact Type Reading for Fact Type, '" & lrFactType.Name & "', to complete this verbalisation>")
            lrVerbaliser.HTW.WriteBreak()
        End If

        lrVerbaliser.HTW.WriteBreak()
        lrVerbaliser.VerbaliseQuantifier(" then ")

        If lrFactType.FactTypeReading.Count > 0 Then
            Call Me.VerbaliseFactTypePart(lrVerbaliser, lrFactType, Purple, Green, "", pcenumFollowingThatOrSome.None, False, 2, Nothing, True)
        Else
            lrVerbaliser.VerbaliseError("<Provide a Fact Type Reading for Fact Type, '" & lrFactType.Name & "', to complete this verbalisation>")
            lrVerbaliser.HTW.WriteBreak()
        End If

        lrVerbaliser.HTW.WriteBreak()
        lrVerbaliser.HTW.WriteBreak()
        lrVerbaliser.VerbaliseQuantifier("If ")

        If lrFactType.FactTypeReading.Count > 0 Then
            Call Me.VerbaliseFactTypePart(lrVerbaliser, lrFactType, Purple, Green, "", pcenumFollowingThatOrSome.None, False, 1)
        Else
            lrVerbaliser.VerbaliseError("<Provide a Fact Type Reading for Fact Type, '" & lrFactType.Name & "', to complete this verbalisation>")
            lrVerbaliser.HTW.WriteBreak()
        End If

        lrVerbaliser.VerbaliseQuantifier(" and ")

        If lrFactType.FactTypeReading.Count > 0 Then
            Call Me.VerbaliseFactTypePart(lrVerbaliser, lrFactType, Purple, Green, "", pcenumFollowingThatOrSome.None, False, 2)
        Else
            lrVerbaliser.VerbaliseError("<Provide a Fact Type Reading for Fact Type, '" & lrFactType.Name & "', to complete this verbalisation>")
            lrVerbaliser.HTW.WriteBreak()
        End If
        'If Entity Type1 is subtype of Entity Type2 and Entity Type2 is subtype of Entity Type3

        If abDeontic Then
            lrVerbaliser.HTW.WriteBreak()
            lrVerbaliser.VerbaliseQuantifier(" then it is forbidden that ")
        Else
            lrVerbaliser.HTW.WriteBreak()
            lrVerbaliser.VerbaliseQuantifier(" then it is impossible that ")
        End If


        Dim lrSubscriptArray As New List(Of String)
        lrSubscriptArray.Add("1")
        lrSubscriptArray.Add("3")
        lrFactType = arRoleConstraint.RoleConstraintRole(0).Role.FactType
        If lrFactType.FactTypeReading.Count > 0 Then
            Call Me.VerbaliseFactTypePart(lrVerbaliser, lrFactType, Purple, Green, "that ", pcenumFollowingThatOrSome.That, False, 1, lrSubscriptArray)
        Else
            lrVerbaliser.VerbaliseError("<Provide a Fact Type Reading for Fact Type, '" & lrFactType.Name & "', to complete this verbalisation>")
            lrVerbaliser.HTW.WriteBreak()
        End If

        Me.WebBrowser.DocumentText = lrVerbaliser.Verbalise
    End Sub

    Public Sub VerbaliseRoleConstraintRingConstraintSymmetricIrreflexive(ByVal arRoleConstraint As FBM.RoleConstraint, Optional ByVal abDeontic As Boolean = False)

        Dim lrFactType As FBM.FactType
        Dim lrVerbaliser As New FBM.ORMVerbailser

        Call lrVerbaliser.Reset()

        If arRoleConstraint.RoleConstraintRole.Count = 0 Then
            lrVerbaliser.VerbaliseError("<Provide links to Roles for Role Constraint, '" & arRoleConstraint.Name & "', to complete this verbalisation>")
            lrVerbaliser.HTW.WriteBreak()
            Exit Sub
        End If

        '------------------------------------------------------------
        'Declare that the RoleConstraint(Name) is an RoleConstraint
        '------------------------------------------------------------
        lrVerbaliser.VerbaliseModelObject(arRoleConstraint)
        lrVerbaliser.VerbaliseQuantifier(" is a Role Constraint")
        lrVerbaliser.VerbaliseQuantifier(" (of type, 'Symmetric Irreflexive Ring Constraint')")
        lrVerbaliser.HTW.WriteBreak()
        lrVerbaliser.HTW.WriteBreak()

        If abDeontic Then
            lrVerbaliser.VerbaliseQuantifier("It is obligatory that if ")
        Else
            lrVerbaliser.VerbaliseQuantifier("If ")
        End If

        lrFactType = arRoleConstraint.RoleConstraintRole(0).Role.FactType
        If lrFactType.FactTypeReading.Count > 0 Then
            Call Me.VerbaliseFactTypePart(lrVerbaliser, arRoleConstraint.RoleConstraintRole(0).Role.FactType, Purple, Green, "", pcenumFollowingThatOrSome.None, False, 1)
        Else
            lrVerbaliser.VerbaliseError("<Provide a Fact Type Reading for Fact Type, '" & lrFactType.Name & "', to complete this verbalisation>")
            lrVerbaliser.HTW.WriteBreak()
        End If

        lrVerbaliser.HTW.WriteBreak()
        lrVerbaliser.VerbaliseQuantifier(" then ")

        If lrFactType.FactTypeReading.Count > 0 Then
            Call Me.VerbaliseFactTypePart(lrVerbaliser, lrFactType, Purple, Green, "", pcenumFollowingThatOrSome.None, False, 2, Nothing, True)
        Else
            lrVerbaliser.VerbaliseError("<Provide a Fact Type Reading for Fact Type, '" & lrFactType.Name & "', to complete this verbalisation>")
            lrVerbaliser.HTW.WriteBreak()
        End If

        lrVerbaliser.HTW.WriteBreak()

        If abDeontic Then
            lrVerbaliser.HTW.WriteBreak()
            lrVerbaliser.VerbaliseQuantifier("It is obligatory that no ")
        Else
            lrVerbaliser.HTW.WriteBreak()
            lrVerbaliser.VerbaliseQuantifier("No ")
        End If

        If lrFactType.FactTypeReading.Count > 0 Then
            Call Me.VerbaliseFactTypePart(lrVerbaliser, lrFactType, Purple, Green, "", pcenumFollowingThatOrSome.TheSame, False)
        Else
            lrVerbaliser.VerbaliseError("<Provide a Fact Type Reading for Fact Type, '" & lrFactType.Name & "', to complete this verbalisation>")
            lrVerbaliser.HTW.WriteBreak()
        End If

        Me.WebBrowser.DocumentText = lrVerbaliser.Verbalise

    End Sub

    Public Sub VerbaliseRoleConstraintRingConstraint(ByVal arRoleConstraint As FBM.RoleConstraint)

        Select Case arRoleConstraint.RingConstraintType
            Case Is = pcenumRingConstraintType.Acyclic
                Call Me.VerbaliseRoleConstraintRingConstraintAcyclic(arRoleConstraint)

            Case Is = pcenumRingConstraintType.AcyclicIntransitive
                Call Me.VerbaliseRoleConstraintRingConstraintAcyclicIntransitive(arRoleConstraint)

            Case Is = pcenumRingConstraintType.AcyclicStronglyIntransitive
                Call Me.VerbaliseRoleConstraintRingConstraintAcyclicStronglyIntransitive(arRoleConstraint)

            Case Is = pcenumRingConstraintType.Antisymmetric
                Call Me.VerbaliseRoleConstraintRingConstraintAntisymmetric(arRoleConstraint)

            Case Is = pcenumRingConstraintType.Asymmetric
                Call Me.VerbaliseRoleConstraintRingConstraintAsymmetric(arRoleConstraint)

            Case Is = pcenumRingConstraintType.AsymmetricIntransitive
                Call Me.VerbaliseRoleConstraintRingConstraintAsymmetricIntransitive(arRoleConstraint)

            Case Is = pcenumRingConstraintType.DeonticAcyclic
                Call Me.VerbaliseRoleConstraintRingConstraintAcyclic(arRoleConstraint, True)

            Case Is = pcenumRingConstraintType.DeonticAcyclicIntransitive
                Call Me.VerbaliseRoleConstraintRingConstraintAcyclicIntransitive(arRoleConstraint, True)

            Case Is = pcenumRingConstraintType.DeonticAntisymmetric
                Call Me.VerbaliseRoleConstraintRingConstraintAntisymmetric(arRoleConstraint, True)

            Case Is = pcenumRingConstraintType.DeonticAssymmetric
                Call Me.VerbaliseRoleConstraintRingConstraintAsymmetric(arRoleConstraint, True)

            Case Is = pcenumRingConstraintType.DeonticAssymmetricIntransitive
                Call Me.VerbaliseRoleConstraintRingConstraintAsymmetricIntransitive(arRoleConstraint, True)

            Case Is = pcenumRingConstraintType.DeonticIntransitive
                Call Me.VerbaliseRoleConstraintRingConstraintIntransitive(arRoleConstraint, True)

            Case Is = pcenumRingConstraintType.DeonticIrreflexive
                Call Me.VerbaliseRoleConstraintRingConstraintIrreflexive(arRoleConstraint, True)

            Case Is = pcenumRingConstraintType.DeonticPurelyReflexive
                Call Me.VerbaliseRoleConstraintRingConstraintPurelyReflexive(arRoleConstraint, True)

            Case Is = pcenumRingConstraintType.DeonticSymmetric
                Call Me.VerbaliseRoleConstraintRingConstraintSymmetric(arRoleConstraint, True)

            Case Is = pcenumRingConstraintType.DeonticSymmetricIntransitive
                Call Me.VerbaliseRoleConstraintRingConstraintSymmetricIntransitive(arRoleConstraint, True)

            Case Is = pcenumRingConstraintType.DeonticSymmetricIrreflexive
                Call Me.VerbaliseRoleConstraintRingConstraintSymmetricIrreflexive(arRoleConstraint, True)

            Case Is = pcenumRingConstraintType.Intransitive
                Call Me.VerbaliseRoleConstraintRingConstraintIntransitive(arRoleConstraint)

            Case Is = pcenumRingConstraintType.Irreflexive
                Call Me.VerbaliseRoleConstraintRingConstraintIrreflexive(arRoleConstraint)

            Case Is = pcenumRingConstraintType.PurelyReflexive
                Call Me.VerbaliseRoleConstraintRingConstraintPurelyReflexive(arRoleConstraint)

            Case Is = pcenumRingConstraintType.Symmetric
                Call Me.VerbaliseRoleConstraintRingConstraintSymmetric(arRoleConstraint)

            Case Is = pcenumRingConstraintType.SymmetricIntransitive
                Call Me.VerbaliseRoleConstraintRingConstraintSymmetricIntransitive(arRoleConstraint)

            Case Is = pcenumRingConstraintType.SymmetricIrreflexive
                Call Me.VerbaliseRoleConstraintRingConstraintSymmetricIrreflexive(arRoleConstraint)

            Case Is = pcenumRingConstraintType.SymmetricTransitive
                Call Me.VerbaliseRoleConstraintRingConstraintSymmetricTransitive(arRoleConstraint, True)
        End Select

    End Sub

    Public Sub VerbaliseRoleConstraintRingConstraintAcyclic(ByVal arRoleConstraint As FBM.RoleConstraint, Optional ByVal abDeontic As Boolean = False)

        Dim lrVerbaliser As New FBM.ORMVerbailser
        Call lrVerbaliser.Reset()

        If arRoleConstraint.RoleConstraintRole.Count = 0 Then
            lrVerbaliser.VerbaliseError("<Provide links to Roles for Role Constraint, '" & arRoleConstraint.Name & "', to complete this verbalisation>")
            lrVerbaliser.HTW.WriteBreak()
            Exit Sub
        End If

        '------------------------------------------------------------
        'Declare that the RoleConstraint(Name) is an RoleConstraint
        '------------------------------------------------------------
        lrVerbaliser.VerbaliseModelObject(arRoleConstraint)
        lrVerbaliser.VerbaliseQuantifier(" is a Role Constraint")
        lrVerbaliser.VerbaliseQuantifier(" (of type, 'Ring Constraint')")
        lrVerbaliser.HTW.WriteBreak()
        lrVerbaliser.HTW.WriteBreak()

        If abDeontic Then            
            lrVerbaliser.VerbaliseQuantifier("It is obligatory that no ")
        Else
            lrVerbaliser.VerbaliseQuantifier("No ")
        End If

        lrVerbaliser.VerbaliseModelObject(arRoleConstraint.RoleConstraintRole(0).Role.JoinedORMObject)
        lrVerbaliser.VerbaliseQuantifier(" may cycle back to itself via one or more traversals through ")

        Dim lrFactType As FBM.FactType
        lrFactType = arRoleConstraint.RoleConstraintRole(0).Role.FactType
        If lrFactType.FactTypeReading.Count > 0 Then
            Call Me.VerbaliseFactTypePart(lrVerbaliser, lrFactType, Purple, Green, Nothing, pcenumFollowingThatOrSome.Either, False)
        Else
            lrVerbaliser.VerbaliseError("<Provide a Fact Type Reading for Fact Type, '" & lrFactType.Name & "', to complete this verbalisation>")
            lrVerbaliser.HTW.WriteBreak()
        End If

        Me.WebBrowser.DocumentText = lrVerbaliser.Verbalise
    End Sub

    Public Sub VerbaliseRoleConstraintRingConstraintAcyclicIntransitive(ByVal arRoleConstraint As FBM.RoleConstraint, Optional ByVal abDeontic As Boolean = False)

        Dim lrVerbaliser As New FBM.ORMVerbailser
        Call lrVerbaliser.Reset()

        If arRoleConstraint.RoleConstraintRole.Count = 0 Then
            lrVerbaliser.VerbaliseError("<Provide links to Roles for Role Constraint, '" & arRoleConstraint.Name & "', to complete this verbalisation>")
            lrVerbaliser.HTW.WriteBreak()
            Exit Sub
        End If

        '------------------------------------------------------------
        'Declare that the RoleConstraint(Name) is an RoleConstraint
        '------------------------------------------------------------
        lrVerbaliser.VerbaliseModelObject(arRoleConstraint)
        lrVerbaliser.VerbaliseQuantifier(" is a Role Constraint")
        lrVerbaliser.VerbaliseQuantifier(" (of type, 'Acyclic Intransitive Ring Constraint')")
        lrVerbaliser.HTW.WriteBreak()
        lrVerbaliser.HTW.WriteBreak()

        If abDeontic Then
            lrVerbaliser.VerbaliseQuantifier("It is obligatory that no ")
        Else
            lrVerbaliser.VerbaliseQuantifier("No ")
        End If

        lrVerbaliser.VerbaliseModelObject(arRoleConstraint.RoleConstraintRole(0).Role.JoinedORMObject)
        lrVerbaliser.VerbaliseQuantifier(" may cycle back to itself via one or more traversals through ")

        Dim lrFactType As FBM.FactType
        lrFactType = arRoleConstraint.RoleConstraintRole(0).Role.FactType

        If lrFactType.FactTypeReading.Count > 0 Then
            Call Me.VerbaliseFactTypePart(lrVerbaliser, lrFactType, Purple, Green, Nothing, pcenumFollowingThatOrSome.Either, False)
        Else
            lrVerbaliser.VerbaliseError("<Provide a Fact Type Reading for Fact Type, '" & lrFactType.Name & "', to complete this verbalisation>")
            lrVerbaliser.HTW.WriteBreak()
        End If

        lrVerbaliser.HTW.WriteBreak()
        lrVerbaliser.HTW.WriteBreak()
        lrVerbaliser.VerbaliseQuantifier("If ")
        lrFactType = arRoleConstraint.RoleConstraintRole(0).Role.FactType
        If lrFactType.FactTypeReading.Count > 0 Then
            Call Me.VerbaliseFactTypePart(lrVerbaliser, lrFactType, Purple, Green, Nothing, pcenumFollowingThatOrSome.Either, False, 1)
        Else
            lrVerbaliser.VerbaliseError("<Provide a Fact Type Reading for Fact Type, '" & lrFactType.Name & "', to complete this verbalisation>")
            lrVerbaliser.HTW.WriteBreak()
        End If

        lrVerbaliser.VerbaliseQuantifier(" and ")
        lrFactType = arRoleConstraint.RoleConstraintRole(0).Role.FactType
        If lrFactType.FactTypeReading.Count > 0 Then
            Call Me.VerbaliseFactTypePart(lrVerbaliser, lrFactType, Purple, Green, "that ", pcenumFollowingThatOrSome.Either, False, 2)
        Else
            lrVerbaliser.VerbaliseError("<Provide a Fact Type Reading for Fact Type, '" & lrFactType.Name & "', to complete this verbalisation>")
            lrVerbaliser.HTW.WriteBreak()
        End If

        'If Entity Type1 is subtype of Entity Type2 and Entity Type2 is subtype of Entity Type3

        If abDeontic Then
            lrVerbaliser.HTW.WriteBreak()
            lrVerbaliser.VerbaliseQuantifier(" then it is forbidden that ")
        Else
            lrVerbaliser.HTW.WriteBreak()
            lrVerbaliser.VerbaliseQuantifier(" then it is impossible that ")
        End If


        Dim lrSubscriptArray As New List(Of String)
        lrSubscriptArray.Add("1")
        lrSubscriptArray.Add("3")
        lrFactType = arRoleConstraint.RoleConstraintRole(0).Role.FactType
        If lrFactType.FactTypeReading.Count > 0 Then
            Call Me.VerbaliseFactTypePart(lrVerbaliser, lrFactType, Purple, Green, "that ", pcenumFollowingThatOrSome.That, False, 1, lrSubscriptArray)
        Else
            lrVerbaliser.VerbaliseError("<Provide a Fact Type Reading for Fact Type, '" & lrFactType.Name & "', to complete this verbalisation>")
            lrVerbaliser.HTW.WriteBreak()
        End If

        Me.WebBrowser.DocumentText = lrVerbaliser.Verbalise

    End Sub

    Public Sub VerbaliseRoleConstraintRingConstraintAcyclicStronglyIntransitive(ByVal arRoleConstraint As FBM.RoleConstraint, Optional ByVal abDeontic As Boolean = False)

        Dim lrVerbaliser As New FBM.ORMVerbailser
        Call lrVerbaliser.Reset()

        Try

            If arRoleConstraint.RoleConstraintRole.Count = 0 Then
                lrVerbaliser.VerbaliseError("<Provide links to Roles for Role Constraint, '" & arRoleConstraint.Name & "', to complete this verbalisation>")
                lrVerbaliser.HTW.WriteBreak()
                Exit Sub
            End If

            '------------------------------------------------------------
            'Declare that the RoleConstraint(Name) is an RoleConstraint
            '------------------------------------------------------------
            lrVerbaliser.VerbaliseModelObject(arRoleConstraint)
            lrVerbaliser.VerbaliseQuantifier(" is a Role Constraint")
            lrVerbaliser.VerbaliseQuantifier(" (of type, 'Acyclic Strongly Intransitive Ring Constraint')")
            lrVerbaliser.HTW.WriteBreak()
            lrVerbaliser.HTW.WriteBreak()

            If abDeontic Then
                lrVerbaliser.VerbaliseQuantifier("It is obligatory that no ")
            Else
                lrVerbaliser.VerbaliseQuantifier("No ")
            End If

            lrVerbaliser.VerbaliseModelObject(arRoleConstraint.RoleConstraintRole(0).Role.JoinedORMObject)
            lrVerbaliser.VerbaliseQuantifier(" may cycle back to itself via one or more traversals through ")

            Dim lrFactType As FBM.FactType
            lrFactType = arRoleConstraint.RoleConstraintRole(0).Role.FactType

            If lrFactType.FactTypeReading.Count > 0 Then
                Call Me.VerbaliseFactTypePart(lrVerbaliser, lrFactType, Purple, Green, Nothing, pcenumFollowingThatOrSome.Either, False)
            Else
                lrVerbaliser.VerbaliseError("<Provide a Fact Type Reading for Fact Type, '" & lrFactType.Name & "', to complete this verbalisation>")
                lrVerbaliser.HTW.WriteBreak()
            End If

            lrVerbaliser.HTW.WriteBreak()
            lrVerbaliser.HTW.WriteBreak()
            lrVerbaliser.VerbaliseQuantifier("If ")
            lrFactType = arRoleConstraint.RoleConstraintRole(0).Role.FactType
            If lrFactType.FactTypeReading.Count > 0 Then
                Call Me.VerbaliseFactTypePart(lrVerbaliser, lrFactType, Purple, Green, Nothing, pcenumFollowingThatOrSome.Either, False, 1)
            Else
                lrVerbaliser.VerbaliseError("<Provide a Fact Type Reading for Fact Type, '" & lrFactType.Name & "', to complete this verbalisation>")
                lrVerbaliser.HTW.WriteBreak()
            End If

            'lrVerbaliser.VerbaliseQuantifier(" and ")
            lrFactType = arRoleConstraint.RoleConstraintRole(0).Role.FactType
            'If lrFactType.FactTypeReading.Count > 0 Then
            '    Call Me.VerbaliseFactTypePart(lrVerbaliser, lrFactType, Purple, Green, "that ", pcenumFollowingThatOrSome.Either, False, 2)
            'Else
            '    lrVerbaliser.VerbaliseError("<Provide a Fact Type Reading for Fact Type, '" & lrFactType.Name & "', to complete this verbalisation>")
            '    lrVerbaliser.HTW.WriteBreak()
            'End If

            'If Entity Type1 is subtype of Entity Type2 and Entity Type2 is subtype of Entity Type3

            If abDeontic Then
                lrVerbaliser.HTW.WriteBreak()
                lrVerbaliser.VerbaliseQuantifier(" then it is forbidden that ")
            Else
                lrVerbaliser.HTW.WriteBreak()
                lrVerbaliser.VerbaliseQuantifier(" then it is not true that ")
            End If


            lrVerbaliser.VerbaliseModelObject(lrFactType.RoleGroup(0).JoinedORMObject, "1")
            lrVerbaliser.VerbaliseQuantifier(" is indirectly related to ")
            lrVerbaliser.VerbaliseModelObject(lrFactType.RoleGroup(1).JoinedORMObject, "2")
            lrVerbaliser.VerbaliseQuantifier(" by repeatedly applying this fact type.")

            'Dim lrSubscriptArray As New List(Of String)
            'lrSubscriptArray.Add("1")
            'lrSubscriptArray.Add("3")
            'lrFactType = arRoleConstraint.RoleConstraintRole(0).Role.FactType
            'If lrFactType.FactTypeReading.Count > 0 Then
            '    Call Me.VerbaliseFactTypePart(lrVerbaliser, lrFactType, Purple, Green, "that ", pcenumFollowingThatOrSome.That, False, 1, lrSubscriptArray)
            'Else
            '    lrVerbaliser.VerbaliseError("<Provide a Fact Type Reading for Fact Type, '" & lrFactType.Name & "', to complete this verbalisation>")
            '    lrVerbaliser.HTW.WriteBreak()
            'End If

            Me.WebBrowser.DocumentText = lrVerbaliser.Verbalise

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            lrVerbaliser.HTW.WriteBreak()
            lrVerbaliser.VerbaliseError(lsMessage)
            Me.WebBrowser.DocumentText = lrVerbaliser.Verbalise
        End Try

    End Sub

    Public Sub VerbaliseRoleConstraintExclusiveORConstraint(ByVal arRoleConstraint As FBM.RoleConstraint)

        Dim lrVerbaliser As New FBM.ORMVerbailser
        Call lrVerbaliser.Reset()

        '------------------------------------------------------------
        'Declare that the RoleConstraint(Name) is an RoleConstraint
        '------------------------------------------------------------
        lrVerbaliser.VerbaliseModelObject(arRoleConstraint)
        lrVerbaliser.VerbaliseQuantifier(" is a Role Constraint (of type, 'Exclusive OR Constraint')")
        lrVerbaliser.HTW.WriteBreak()
        lrVerbaliser.HTW.WriteBreak()


        Dim liInd As Integer = 0
        Dim liReadingInd As Integer = 0
        Dim lrModelObject As New FBM.ModelObject
        Dim lrFactType As FBM.FactType
        Dim larFactTypeModelObjects As New List(Of FBM.ModelObject)

        '-----------------------------
        'Find the common ModelObject
        '-----------------------------
        If arRoleConstraint.RoleConstraintRole.Count = 0 Then
            lrVerbaliser.VerbaliseError("The Role Constraint has no arguments.")
            Me.WebBrowser.DocumentText = lrVerbaliser.Verbalise()
            Exit Sub
        End If
        lrModelObject = arRoleConstraint.RoleConstraintRole(0).Role.JoinedORMObject

        lrVerbaliser.VerbaliseQuantifier("For each ")
        lrVerbaliser.VerbaliseModelObject(lrModelObject)
        lrVerbaliser.VerbaliseQuantifier(", exactly one of the following holds:")
        lrVerbaliser.HTW.WriteBreak()

        For Each lrRoleConstraintRole In arRoleConstraint.RoleConstraintRole
            lrFactType = lrRoleConstraintRole.Role.FactType
            If lrFactType.FactTypeReading.Count > 0 Then
                Call Me.VerbaliseFactTypePart(lrVerbaliser, lrFactType, Color.Purple, Color.Green, " that ", pcenumFollowingThatOrSome.Some)
                lrVerbaliser.HTW.WriteBreak()
            Else
                lrVerbaliser.VerbaliseError("<Provide a Fact Type Reading for Fact Type, '" & lrFactType.Name & "', to complete this verbalisation>")
                lrVerbaliser.HTW.WriteBreak()
            End If
        Next

        Me.WebBrowser.DocumentText = lrVerbaliser.Verbalise

    End Sub

    Public Sub VerbaliseRoleConstraintInclusiveORConstraint(ByVal arRoleConstraint As FBM.RoleConstraint)

        Dim lrVerbaliser As New FBM.ORMVerbailser
        Call lrVerbaliser.Reset()

        '------------------------------------------------------------
        'Declare that the RoleConstraint(Name) is an RoleConstraint
        '------------------------------------------------------------
        lrVerbaliser.VerbaliseModelObject(arRoleConstraint)
        lrVerbaliser.VerbaliseQuantifier(" is a Role Constraint")
        lrVerbaliser.VerbaliseQuantifier(" (of type, 'Inclusive OR Constraint')")
        lrVerbaliser.HTW.WriteBreak()
        lrVerbaliser.HTW.WriteBreak()

        Dim liInd As Integer = 0
        Dim liReadingInd As Integer = 0
        Dim lrFactType As FBM.FactType
        Dim lrFactTypeReading As FBM.FactTypeReading
        Dim lrRoleConstraintRole As FBM.RoleConstraintRole
        Dim lrModelObject As FBM.ModelObject

        '-----------------------------
        'Find the common ModelObject
        '-----------------------------
        If arRoleConstraint.RoleConstraintRole.Count = 0 Then
            lrVerbaliser.VerbaliseError("The Role Constraint has no arguments.")
            Me.WebBrowser.DocumentText = lrVerbaliser.Verbalise()
            Exit Sub
        End If
        lrModelObject = arRoleConstraint.RoleConstraintRole(0).Role.JoinedORMObject

        lrVerbaliser.VerbaliseQuantifier("Each ")
        lrVerbaliser.VerbaliseModelObject(lrModelObject)
        lrVerbaliser.HTW.WriteBreak()

        For Each lrRoleConstraintRole In arRoleConstraint.RoleConstraintRole
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
                            lrVerbaliser.VerbalisePredicateText(" " & lrPredicatePart.PredicatePartText & " ")
                        End If
                        If liReadingInd > 0 Then
                            lrVerbaliser.VerbaliseQuantifier("some ")
                            lrVerbaliser.VerbaliseModelObject(lrPredicatePart.Role.JoinedORMObject)
                        End If
                        liReadingInd += 1
                    Next
                Else
                    lrVerbaliser.VerbaliseError("Error: Fact Type needs at least on Fact Type Reading.")
                End If
            Else                
                lrVerbaliser.VerbaliseError("Error: Fact Type needs at least on Fact Type Reading.")
            End If
            If liInd < arRoleConstraint.RoleConstraintRole.Count - 1 Then
                lrVerbaliser.VerbaliseQuantifier(" or ")
                lrVerbaliser.HTW.WriteBreak()
            End If
            liInd += 1
        Next

        Me.WebBrowser.DocumentText = lrVerbaliser.Verbalise

    End Sub

    Public Sub VerbaliseRoleConstraintExclusionConstraint(ByVal arRoleConstraint As FBM.RoleConstraint)

        Try
            Dim lrVerbaliser As New FBM.ORMVerbailser
            Call lrVerbaliser.Reset()

            '------------------------------------------------------------
            'Declare that the RoleConstraint(Name) is an RoleConstraint
            '------------------------------------------------------------
            lrVerbaliser.VerbaliseModelObject(arRoleConstraint)
            lrVerbaliser.VerbaliseQuantifier(" is a Role Constraint")
            lrVerbaliser.VerbaliseQuantifier(" (of type, 'Exclusion Role Constraint')")
            lrVerbaliser.HTW.WriteBreak()
            lrVerbaliser.HTW.WriteBreak()

            If arRoleConstraint.Argument.Count < 2 Then
                lrVerbaliser.VerbaliseError("Error: An Exclusion Role Constraint requires at least 2 Role Constraint Arguments.")
                lrVerbaliser.HTW.WriteBreak()
                lrVerbaliser.VerbaliseError("This Exclusion Role Constraint has " & arRoleConstraint.Argument.Count.ToString & " Role Constraint Arguments.")
                lrVerbaliser.HTW.WriteBreak()
                lrVerbaliser.HTW.WriteBreak()
            End If

            lrVerbaliser.VerbaliseQuantifier("For each ")

            Dim lrModelObject As New FBM.ModelObject

            '-----------------------------
            'Find the common ModelObject
            '-----------------------------
            If arRoleConstraint.RoleConstraintRole.Count = 0 Then
                Exit Sub
            End If

            Dim larModelObject As List(Of FBM.ModelObject)
            larModelObject = arRoleConstraint.GetCommonArgumentModelObjects

            lrModelObject = arRoleConstraint.RoleConstraintRole(0).Role.JoinedORMObject

            Dim liInd As Integer = 1
            For Each lrModelObject In larModelObject
                lrVerbaliser.VerbaliseModelObject(lrModelObject)
                If liInd = larModelObject.Count - 1 Then
                    lrVerbaliser.VerbaliseQuantifier(" and ")
                ElseIf liInd = larModelObject.Count Then
                Else
                    lrVerbaliser.VerbaliseSeparator(",")
                End If
                liInd += 1
            Next
            lrVerbaliser.VerbaliseQuantifier(" at most one of the following holds:")
            lrVerbaliser.HTW.WriteBreak()

            Dim lrRoleConstraintArgument As FBM.RoleConstraintArgument

            For Each lrRoleConstraintArgument In arRoleConstraint.Argument
                Call lrRoleConstraintArgument.ProjectArgumentReading(lrVerbaliser, larModelObject)
                lrVerbaliser.HTW.WriteBreak()
            Next

            Me.WebBrowser.DocumentText = lrVerbaliser.Verbalise

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Sub

    Public Sub VerbaliseRelation(ByVal arFactType As FBM.FactType,
                                 Optional ByRef arRelation As RDS.Relation = Nothing,
                                 Optional ByVal abShowMetaInformation As Boolean = False)

        Try
            Dim lrFactTypeReading As FBM.FactTypeReading
            Dim liInd As Integer = 0
            Dim lrFact As FBM.Fact
            Dim lrRole As FBM.Role
            Dim lrRoleConstraint As FBM.RoleConstraint

            Dim lrVerbaliser As New FBM.ORMVerbailser
            Call lrVerbaliser.Reset()

            '------------------------------------------------------
            'Declare that the EntityType(Name) is an EntityType
            '------------------------------------------------------
            lrVerbaliser.VerbalisePredicateText(arFactType.Id)
            lrVerbaliser.VerbaliseQuantifier(" is a Relation.")
            If abShowMetaInformation And arRelation IsNot Nothing Then
                lrVerbaliser.VerbaliseBlackText("Relation.Id: " & arRelation.Id)
            End If
            lrVerbaliser.HTW.WriteBreak()
            lrVerbaliser.HTW.WriteBreak()

            Select Case arFactType.Arity
                Case Is = 1
                Case Is = 2
                    If arFactType.FactTypeReading.Count > 0 Then
                        lrFactTypeReading = arFactType.FactTypeReading(0)
                        lrFactTypeReading.GetReadingText(lrVerbaliser)
                        lrVerbaliser.HTW.WriteBreak()

                        If arFactType.IsManyToOneByRoleOrder(lrFactTypeReading.RoleList) Then
                            lrVerbaliser.VerbaliseIndent()
                            lrVerbaliser.VerbaliseQuantifier("Each ")
                            lrVerbaliser.VerbaliseModelObject(lrFactTypeReading.RoleList(0).JoinedORMObject)
                            lrVerbaliser.VerbalisePredicateText(" " & lrFactTypeReading.PredicatePart(0).PredicatePartText)
                            lrVerbaliser.VerbaliseQuantifier(" at most one ")
                            lrVerbaliser.VerbalisePredicateText(lrFactTypeReading.PredicatePart(1).PreBoundText)
                            lrVerbaliser.VerbaliseModelObject(lrFactTypeReading.RoleList(1).JoinedORMObject)

                            lrVerbaliser.HTW.WriteBreak()
                            lrVerbaliser.VerbaliseIndent()

                            lrVerbaliser.VerbaliseQuantifier("It is possible that more than one ")
                            lrVerbaliser.VerbaliseModelObject(lrFactTypeReading.RoleList(0).JoinedORMObject)
                            lrVerbaliser.VerbalisePredicateText(" " & lrFactTypeReading.PredicatePart(0).PredicatePartText)
                            lrVerbaliser.VerbaliseQuantifier(" the same ")
                            lrVerbaliser.VerbalisePredicateText(lrFactTypeReading.PredicatePart(1).PreBoundText)
                            lrVerbaliser.VerbaliseModelObject(lrFactTypeReading.RoleList(1).JoinedORMObject)
                        End If

                        lrVerbaliser.VerbaliseSeparator(" ")
                        lrVerbaliser.VerbaliseModelObjectLightGray(arFactType)
                    End If

                Case Else

            End Select
            lrVerbaliser.HTW.WriteBreak()

            lrVerbaliser.HTW.WriteBreak()
            lrVerbaliser.VerbaliseHeading("Fact Type Readings:")
            lrVerbaliser.HTW.WriteBreak()
            lrVerbaliser.HTW.WriteBreak()

            If arFactType.IsLinkFactType Then
                Dim lrParentFactType As FBM.FactType = arFactType.LinkFactTypeRole.FactType

                If lrParentFactType.FactTypeReading.Count > 0 Then
                    lrFactTypeReading = lrParentFactType.FactTypeReading(0)
                    lrVerbaliser.VerbaliseQuantifier("If ")
                    lrVerbaliser.VerbaliseModelObject(lrParentFactType)
                    lrVerbaliser.VerbaliseQuantifier(" is defined by '")
                    lrFactTypeReading.GetReadingText(lrVerbaliser, False, True)
                    lrVerbaliser.VerbaliseQuantifier("'")
                    lrVerbaliser.HTW.WriteBreak()
                Else
                    lrVerbaliser.VerbaliseError("Error: Provide a Fact Type Reading for the Fact Type that this Link Fact Type belongs to.")
                    lrVerbaliser.HTW.WriteBreak()
                End If
            End If

            For Each lrFactTypeReading In arFactType.FactTypeReading
                lrFactTypeReading.GetReadingText(lrVerbaliser, False, True)
                lrVerbaliser.HTW.WriteBreak()
            Next

            lrVerbaliser.HTW.WriteBreak()
            lrVerbaliser.VerbaliseHeading("Sample Facts:")
            lrVerbaliser.HTW.WriteBreak()
            lrVerbaliser.HTW.WriteBreak()

            If arFactType.Fact.Count = 0 Then
                lrVerbaliser.VerbaliseQuantifierLight("There are no Sample Facts against the Fact Type.")
                lrVerbaliser.HTW.WriteBreak()
            Else
                For Each lrFact In arFactType.Fact
                    lrVerbaliser.HTW.Write(lrFact.GetReading)
                    lrVerbaliser.HTW.WriteBreak()
                Next
            End If

            '------------------------
            'Uniqueness Constraints
            '------------------------
            lrVerbaliser.HTW.WriteBreak()
            lrVerbaliser.VerbaliseHeading("Uniqueness Constraints:")
            lrVerbaliser.HTW.WriteBreak()
            lrVerbaliser.HTW.WriteBreak()


            If arFactType.HasTotalRoleConstraint Then
                lrRoleConstraint = arFactType.InternalUniquenessConstraint(0)

                lrVerbaliser.VerbaliseModelObject(arFactType)
                lrVerbaliser.VerbaliseQuantifier(" has a Total Internal Uniqueness Constraint.")
                lrVerbaliser.HTW.WriteBreak()
                lrVerbaliser.VerbaliseQuantifier("Role Constraint:")
                lrVerbaliser.VerbaliseModelObject(lrRoleConstraint)
                lrVerbaliser.HTW.WriteBreak()
                lrVerbaliser.VerbaliseQuantifier("In each population of ")
                lrVerbaliser.VerbaliseModelObject(arFactType)
                lrVerbaliser.VerbaliseQuantifier(" each")

                liInd = 0
                For Each lrRole In arFactType.RoleGroup
                    lrVerbaliser.VerbaliseModelObject(lrRole.JoinedORMObject)
                    liInd += 1
                    If liInd < arFactType.Arity Then
                        lrVerbaliser.VerbaliseSeparator(",")
                    End If
                Next

                lrVerbaliser.VerbaliseQuantifier(" combination occurs at most once.")
                lrVerbaliser.HTW.WriteBreak()
                lrVerbaliser.VerbaliseQuantifier("This association of ")

                liInd = 0
                For Each lrRole In arFactType.RoleGroup
                    lrVerbaliser.HTW.Write(" ")
                    lrVerbaliser.VerbaliseModelObject(lrRole.JoinedORMObject)
                    If liInd < arFactType.Arity Then
                        lrVerbaliser.VerbaliseSeparator(",")
                    End If
                Next

                lrVerbaliser.VerbaliseQuantifier(" provides the preferred identification scheme for ")
                lrVerbaliser.VerbaliseModelObject(arFactType)
                lrVerbaliser.VerbaliseQuantifier(".")
                lrVerbaliser.HTW.WriteBreak()

            ElseIf arFactType.Arity = 2 Then
                '--------------------
                'Is Binary FactType
                '--------------------
                Dim larRole As New List(Of FBM.Role)

                For Each lrRoleConstraint In arFactType.InternalUniquenessConstraint

                    lrVerbaliser.VerbaliseQuantifier("Role Constraint: ")
                    lrVerbaliser.VerbaliseModelObject(lrRoleConstraint)
                    lrVerbaliser.HTW.WriteBreak()

                    lrRole = lrRoleConstraint.Role(0)
                    larRole.Add(lrRole)

                    For Each lrRole In arFactType.RoleGroup
                        If lrRole.Id <> lrRoleConstraint.Role(0).Id Then
                            larRole.Add(lrRole)
                        End If
                    Next

                    lrFactTypeReading = arFactType.FindSuitableFactTypeReadingByRoles(larRole)

                    If IsSomething(lrFactTypeReading) Then
                        lrVerbaliser.VerbaliseIndent()
                        lrVerbaliser.VerbaliseQuantifier("Each ")
                        lrVerbaliser.VerbaliseModelObject(lrFactTypeReading.PredicatePart(0).Role.JoinedORMObject)

                        If lrRoleConstraint.RoleConstraintRole(0).Role.Id = lrFactTypeReading.RoleList(0).Id Then
                            lrVerbaliser.VerbalisePredicateText(" " & lrFactTypeReading.PredicatePart(0).PredicatePartText)
                            If lrFactTypeReading.PredicatePart(0).Role.Mandatory Then
                                lrVerbaliser.VerbaliseQuantifier(" one ")
                            Else
                                lrVerbaliser.VerbaliseQuantifier(" at most one ")
                            End If
                            lrVerbaliser.VerbaliseModelObject(lrFactTypeReading.PredicatePart(1).Role.JoinedORMObject)
                        Else
                            If arFactType.Is1To1BinaryFactType Then
                                lrVerbaliser.VerbalisePredicateText(" " & lrFactTypeReading.PredicatePart(0).PredicatePartText)
                                If lrFactTypeReading.PredicatePart(0).Role.Mandatory Then
                                    lrVerbaliser.VerbaliseQuantifier(" one ")
                                Else
                                    lrVerbaliser.VerbaliseQuantifier(" at most one ")
                                End If
                                lrVerbaliser.VerbaliseModelObject(lrFactTypeReading.PredicatePart(1).Role.JoinedORMObject)
                            Else
                                If lrFactTypeReading.PredicatePart(0).Role.Mandatory Then
                                    lrVerbaliser.VerbalisePredicateText(" " & lrFactTypeReading.PredicatePart(0).PredicatePartText)
                                    lrVerbaliser.VerbaliseQuantifier(" at least one ")
                                    lrVerbaliser.VerbaliseModelObject(lrFactTypeReading.PredicatePart(1).Role.JoinedORMObject)
                                Else
                                    lrVerbaliser.VerbaliseQuantifier(" possibly ")
                                    lrVerbaliser.VerbalisePredicateText(" " & lrFactTypeReading.PredicatePart(0).PredicatePartText)
                                    lrVerbaliser.VerbaliseQuantifier(" more than one ")
                                    lrVerbaliser.VerbaliseModelObject(lrFactTypeReading.PredicatePart(1).Role.JoinedORMObject)
                                End If
                            End If
                        End If
                    End If

                    If (arFactType.FactTypeReading.Count = 2) Then
                        If Not arFactType.Is1To1BinaryFactType Then
                            lrFactTypeReading = arFactType.FactTypeReading.Find(Function(x) x.Id <> lrFactTypeReading.Id)
                            lrVerbaliser.HTW.WriteBreak()
                            lrVerbaliser.VerbaliseIndent()
                            lrVerbaliser.VerbaliseQuantifier("Each ")
                            lrVerbaliser.VerbaliseModelObject(lrFactTypeReading.PredicatePart(0).Role.JoinedORMObject)
                            If lrRoleConstraint.RoleConstraintRole(0).Role.Id = lrFactTypeReading.RoleList(0).Id Then
                                lrVerbaliser.VerbalisePredicateText(" " & lrFactTypeReading.PredicatePart(0).PredicatePartText)
                                If lrFactTypeReading.PredicatePart(0).Role.Mandatory Then
                                    lrVerbaliser.VerbaliseQuantifier(" one ")
                                Else
                                    lrVerbaliser.VerbaliseQuantifier(" at most one ")
                                End If
                                lrVerbaliser.VerbaliseModelObject(lrFactTypeReading.PredicatePart(1).Role.JoinedORMObject)
                            Else
                                If lrFactTypeReading.PredicatePart(0).Role.Mandatory Then
                                    lrVerbaliser.VerbalisePredicateText(" " & lrFactTypeReading.PredicatePart(0).PredicatePartText)
                                    lrVerbaliser.VerbaliseQuantifier(" at least one ")
                                    lrVerbaliser.VerbaliseModelObject(lrFactTypeReading.PredicatePart(1).Role.JoinedORMObject)
                                Else
                                    lrVerbaliser.VerbaliseQuantifier(" possibly ")
                                    lrVerbaliser.VerbalisePredicateText(" " & lrFactTypeReading.PredicatePart(0).PredicatePartText)
                                    lrVerbaliser.VerbaliseQuantifier(" more than one ")
                                    lrVerbaliser.VerbaliseModelObject(lrFactTypeReading.PredicatePart(1).Role.JoinedORMObject)
                                End If
                            End If
                        End If
                    Else
                        lrVerbaliser.HTW.WriteBreak()
                        lrVerbaliser.VerbaliseIndent()
                        lrVerbaliser.VerbaliseError("Provide a reverse Fact Type Reading for this Fact Type")
                        lrVerbaliser.HTW.WriteBreak()
                        lrVerbaliser.HTW.WriteBreak()
                    End If

                    lrVerbaliser.HTW.WriteBreak()
                Next
            Else
                '-------------------------------------------
                'Is Partial Internal Uniqueness Constraint
                '-------------------------------------------
                For Each lrRoleConstraint In arFactType.InternalUniquenessConstraint

                    lrVerbaliser.VerbaliseQuantifier("Role Constraint: ")
                    lrVerbaliser.VerbaliseModelObject(lrRoleConstraint)
                    lrVerbaliser.HTW.WriteBreak()
                    lrVerbaliser.VerbaliseQuantifier("In each population of ")
                    lrVerbaliser.VerbaliseModelObject(arFactType)
                    lrVerbaliser.VerbaliseQuantifier(" each")

                    liInd = 0
                    For Each lrRole In lrRoleConstraint.Role
                        lrVerbaliser.HTW.Write(" ")
                        lrVerbaliser.VerbaliseModelObject(lrRole.JoinedORMObject)
                        liInd += 1
                        If liInd < lrRoleConstraint.Role.Count Then
                            lrVerbaliser.VerbaliseSeparator(",")
                        End If
                    Next
                    lrVerbaliser.VerbaliseQuantifier(" combination ")

                    For Each lrRole In arFactType.RoleGroup
                        If Not IsSomething(lrRoleConstraint.Role.Find(AddressOf lrRole.Equals)) Then
                            lrVerbaliser.VerbaliseQuantifier("is unique and relates to exactly one instance of ")
                            lrVerbaliser.VerbaliseModelObject(lrRole.JoinedORMObject)
                        End If
                    Next

                    lrVerbaliser.HTW.WriteBreak()
                Next
            End If

            '---------------------------
            'Mandatory RoleConstraints
            '---------------------------
            lrVerbaliser.HTW.WriteBreak()
            lrVerbaliser.HTW.WriteBreak()
            lrVerbaliser.VerbaliseHeading("Mandatory Role Constraints:")
            lrVerbaliser.HTW.WriteBreak()
            lrVerbaliser.HTW.WriteBreak()

            If arFactType.RoleGroup.FindAll(Function(x) x.Mandatory = True).Count = 0 Then
                lrVerbaliser.VerbaliseQuantifierLight("There are no Mandatory Role Constraints against Roles of the Fact Type.")
            Else
                For Each lrRole In arFactType.RoleGroup
                    If lrRole.Mandatory = True Then
                        lrVerbaliser.VerbaliseQuantifier("For each instance of ")
                        lrVerbaliser.VerbaliseModelObject(lrRole.JoinedORMObject)
                        lrVerbaliser.VerbaliseQuantifier(" it is mandatory that that instance participate in ")
                        lrVerbaliser.VerbaliseModelObject(arFactType)
                        lrVerbaliser.HTW.Write(" ")
                        lrVerbaliser.HTW.WriteBreak()
                    End If
                Next
            End If

            lrVerbaliser.HTW.WriteBreak()

            Me.WebBrowser.DocumentText = lrVerbaliser.Verbalise

        Catch ex As Exception
            Dim lsMessage1 As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage1 &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Sub


    Public Sub VerbaliseRoleConstraintEqualityConstraint(ByVal arRoleConstraint As FBM.RoleConstraint)

        Dim lrVerbaliser As New FBM.ORMVerbailser
        Call lrVerbaliser.Reset()

        '------------------------------------------------------------
        'Declare that the RoleConstraint(Name) is an RoleConstraint
        '------------------------------------------------------------
        lrVerbaliser.VerbaliseModelObject(arRoleConstraint)
        lrVerbaliser.VerbaliseQuantifier(" is a Role Constraint")
        lrVerbaliser.VerbaliseQuantifier(" (of type, 'Equality Constraint')")
        lrVerbaliser.HTW.WriteBreak()
        lrVerbaliser.HTW.WriteBreak()

        '--------------------------------------------------------------------------------
        'Get the list of ModelObjects in the RoleConstraints set of RoleConstraintRoles
        '--------------------------------------------------------------------------------
        Dim lrModelObject As FBM.ModelObject
        Dim larModelObject As New List(Of FBM.ModelObject)

        larModelObject = arRoleConstraint.GetCommonArgumentModelObjects()

        'For Each lrRoleConstraintRole In arRoleConstraint.RoleConstraintRole
        '    If larModelObject.Exists(AddressOf lrRoleConstraintRole.Role.JoinedORMObject.Equals) Then
        '        '---------------------------------------
        '        'Don't add the ModelObject to the List
        '        '---------------------------------------
        '    Else
        '        larModelObject.Add(lrRoleConstraintRole.Role.JoinedORMObject)
        '    End If
        'Next

        lrVerbaliser.VerbaliseQuantifier("For each: ")

        Dim liInd As Integer = 1
        For Each lrModelObject In larModelObject
            lrVerbaliser.VerbaliseModelObject(lrModelObject)
            If liInd < larModelObject.Count Then
                lrVerbaliser.VerbaliseQuantifier(" and ")
            End If
            liInd += 1
        Next

        lrVerbaliser.HTW.WriteBreak()

        liInd = 0
        For Each lrRoleConstraintArgument In arRoleConstraint.Argument
            Call lrRoleConstraintArgument.ProjectArgumentReading(lrVerbaliser, larModelObject)

            If liInd = 0 Then
                lrVerbaliser.VerbaliseQuantifier(" if and only if ")
            End If

            lrVerbaliser.HTW.WriteBreak()
            liInd += 1
        Next

        Me.WebBrowser.DocumentText = lrVerbaliser.Verbalise

    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="arFactType"></param>
    ''' <param name="arModelObjectColour"></param>
    ''' <param name="arPredicatePartColour"></param>
    ''' <param name="asIntitialThatOrSome"></param>
    ''' <param name="asFollowingThatOrSome"></param>
    ''' <param name="abDropIntialModelObject"></param>
    ''' <param name="aiStartingSubscriptInteger"></param>
    ''' <param name="aarSubscriptArray"></param>
    ''' <param name="abSubscriptNegativeOrder"></param>
    ''' <param name="aarListedFactType">List of FactTypes already processed</param>
    ''' <param name="abSkippedFactType"></param>
    ''' <remarks></remarks>
    Private Sub VerbaliseFactTypePart(ByRef arVerbaliser As FBM.ORMVerbailser, _
                                      ByVal arFactType As FBM.FactType, _
                                      ByVal arModelObjectColour As Color, _
                                      ByVal arPredicatePartColour As Color, _
                                      Optional ByVal asIntitialThatOrSome As String = Nothing, _
                                      Optional ByVal asFollowingThatOrSome As pcenumFollowingThatOrSome = Nothing, _
                                      Optional ByVal abDropIntialModelObject As Boolean = Nothing, _
                                      Optional ByVal aiStartingSubscriptInteger As Integer = Nothing, _
                                      Optional ByVal aarSubscriptArray As List(Of String) = Nothing, _
                                      Optional ByVal abSubscriptNegativeOrder As Boolean = False, _
                                      Optional ByRef aarListedFactType As List(Of FBM.FactType) = Nothing, _
                                      Optional ByRef abSkippedFactType As Boolean = False)

        Dim liInd As Integer = 0
        Dim liSubscriptInteger As Integer = 0
        Dim lrFactTypeReading As FBM.FactTypeReading
        Dim larFactTypeReading As New List(Of FBM.FactTypeReading)
        Dim lsPredicatePart As String = ""
        Dim liEntriesProcessed As Integer = 0
        Dim lrPredicatePart As FBM.PredicatePart

        If IsSomething(aiStartingSubscriptInteger) Then
            liSubscriptInteger = aiStartingSubscriptInteger
        End If

        If IsSomething(aarListedFactType) Then
            If aarListedFactType.Exists(AddressOf arFactType.Equals) Then
                '-----------------------------------------------------------------------------------
                'The FactType has already been verbalised and doesn't need to be verbalised again.
                '-----------------------------------------------------------------------------------
                abSkippedFactType = True
                Exit Sub
            End If
            aarListedFactType.Add(arFactType)
        End If

        If arFactType.FactTypeReading.Count > 0 Then

            lrFactTypeReading = arFactType.FactTypeReading(0)

            If larFactTypeReading.Exists(AddressOf lrFactTypeReading.Equals) Then
                '----------------------------------------
                'Have already processed that Reading
                '--------------------------------------
            Else
                larFactTypeReading.Add(lrFactTypeReading)
                liInd = 0

                If abSubscriptNegativeOrder Then
                    liSubscriptInteger = arFactType.RoleGroup.Count
                End If

                For Each lrPredicatePart In lrFactTypeReading.PredicatePart

                    If Me.zrModel.GetConceptTypeByNameFuzzy(lrPredicatePart.Role.JoinedORMObject.Id, lrPredicatePart.Role.JoinedORMObject.Id) = pcenumConceptType.FactType Then
                        If IsSomething(aarListedFactType) Then
                            aarListedFactType.Add(Me.zrModel.GetModelObjectByName(lrPredicatePart.Role.JoinedORMObject.Id))
                        End If
                    End If

                    If (liInd = 0) And IsSomething(asIntitialThatOrSome) Then                        
                        arVerbaliser.VerbaliseQuantifier(asIntitialThatOrSome)
                    ElseIf (liInd = 0) Then
                        arVerbaliser.VerbaliseQuantifier("some ")
                    Else
                        If IsSomething(asFollowingThatOrSome) Then
                            Select Case asFollowingThatOrSome
                                Case Is = pcenumFollowingThatOrSome.Some
                                    arVerbaliser.VerbaliseQuantifier("some ")
                                Case Is = pcenumFollowingThatOrSome.That
                                    arVerbaliser.VerbaliseQuantifier("that ")
                                Case Is = pcenumFollowingThatOrSome.Either
                                    arVerbaliser.VerbaliseQuantifier("some ") '20141218-For now, might change depending on the Internal Uniqueness Constraints of the FactType
                                Case Is = pcenumFollowingThatOrSome.TheSame
                                    arVerbaliser.VerbaliseQuantifier("the same ")
                            End Select
                        End If
                    End If
                    If (liInd = 0) And IsSomething(abDropIntialModelObject) Then
                        If abDropIntialModelObject Then
                            '--------------------------------------------------
                            'Don't add the initial ModelObject to the reading
                            '--------------------------------------------------
                            If IsSomething(aarListedFactType) Then
                                If Me.zrModel.GetConceptTypeByNameFuzzy(lrPredicatePart.Role.JoinedORMObject.Id, lrPredicatePart.Role.JoinedORMObject.Id) = pcenumConceptType.FactType Then
                                    If aarListedFactType.Exists(AddressOf Me.zrModel.GetModelObjectByName(lrPredicatePart.Role.JoinedORMObject.Id).Equals) Then
                                        '--------------------------------------------------
                                        'Definitely Drop he Initial Model Object
                                        '-----------------------------------------
                                    Else                                        
                                        arVerbaliser.VerbaliseQuantifier("(")
                                        arVerbaliser.VerbaliseModelObject(lrPredicatePart.Role.JoinedORMObject)
                                        arVerbaliser.VerbaliseQuantifier(")")
                                    End If
                                Else                                    
                                    arVerbaliser.VerbaliseQuantifier("(")
                                    arVerbaliser.VerbaliseModelObject(lrPredicatePart.Role.JoinedORMObject)
                                    arVerbaliser.VerbaliseQuantifier(")")
                                End If
                            Else                                
                                arVerbaliser.VerbaliseQuantifier("(")
                                arVerbaliser.VerbaliseModelObject(lrPredicatePart.Role.JoinedORMObject)
                                arVerbaliser.VerbaliseQuantifier(")")
                            End If
                        Else
                            If aiStartingSubscriptInteger > 0 Then                                
                                arVerbaliser.VerbaliseModelObject(lrPredicatePart.Role.JoinedORMObject)
                                If IsSomething(aarSubscriptArray) Then                                    
                                    arVerbaliser.VerbaliseSubscript(" " & aarSubscriptArray(liInd))
                                Else                                    
                                    arVerbaliser.VerbaliseSubscript(" " & liSubscriptInteger)
                                End If
                            Else
                                arVerbaliser.HTW.Write(" ")
                                arVerbaliser.VerbalisePredicateText(lrPredicatePart.PreBoundText)
                                arVerbaliser.VerbaliseModelObject(lrPredicatePart.Role.JoinedORMObject)
                                arVerbaliser.VerbalisePredicateText(lrPredicatePart.PostBoundText)
                                arVerbaliser.HTW.Write(" ")
                            End If
                        End If
                    Else
                        If aiStartingSubscriptInteger > 0 Then
                            arVerbaliser.VerbalisePredicateText(lrPredicatePart.PreBoundText)
                            arVerbaliser.VerbaliseModelObject(lrPredicatePart.Role.JoinedORMObject)
                            arVerbaliser.VerbalisePredicateText(lrPredicatePart.PostBoundText)
                            If IsSomething(aarSubscriptArray) Then
                                arVerbaliser.VerbaliseSubscript(" " & aarSubscriptArray(liInd))
                            Else
                                arVerbaliser.VerbaliseSubscript(" " & liSubscriptInteger)
                            End If
                        Else
                            arVerbaliser.HTW.Write(" ")
                            arVerbaliser.VerbalisePredicateText(lrPredicatePart.PreBoundText)
                            arVerbaliser.VerbaliseModelObject(lrPredicatePart.Role.JoinedORMObject)
                            arVerbaliser.VerbalisePredicateText(lrPredicatePart.PostBoundText)
                            arVerbaliser.HTW.Write(" ")
                        End If
                    End If

                    If liInd < lrFactTypeReading.PredicatePart.Count Then
                        lsPredicatePart = Trim(lrFactTypeReading.PredicatePart(liInd).PredicatePartText)
                        lsPredicatePart = lsPredicatePart & " "
                        lsPredicatePart = lsPredicatePart.Replace(" a ", " ")
                        lsPredicatePart = lsPredicatePart.Replace(" an ", " ")

                        arVerbaliser.VerbalisePredicateText(" " & lsPredicatePart)
                    End If
                    liInd += 1
                    If abSubscriptNegativeOrder Then
                        liSubscriptInteger -= 1
                    Else
                        liSubscriptInteger += 1
                    End If

                Next

                liEntriesProcessed += 1

            End If

        Else

        End If

    End Sub

    Public Sub VerbaliseRoleConstraintSubset(ByVal arRoleConstraintSubtype As FBM.RoleConstraint)

        Try
            Dim liInd As Integer = 0

            Dim lrVerbaliser As New FBM.ORMVerbailser
            Call lrVerbaliser.Reset()

            If arRoleConstraintSubtype.RoleConstraintRole.Count = 0 Then
                lrVerbaliser.VerbaliseError("<Provide links to Roles for Role Constraint, '" & arRoleConstraintSubtype.Name & "', to complete this verbalisation>")
                lrVerbaliser.HTW.WriteBreak()
                Exit Sub
            End If

            '------------------------------------------------------------
            'Declare that the RoleConstraint(Name) is an RoleConstraint
            '------------------------------------------------------------
            lrVerbaliser.VerbaliseModelObject(arRoleConstraintSubtype)
            lrVerbaliser.VerbaliseQuantifier(" is a Role Constraint")
            lrVerbaliser.VerbaliseQuantifier(" (of type, 'Subset Constraint')")
            lrVerbaliser.HTW.WriteBreak()
            lrVerbaliser.HTW.WriteBreak()

            Dim larFactTypeReading As New List(Of FBM.FactTypeReading)
            Dim larReadingObjectType As New List(Of FBM.ModelObject)
            Dim larExitPredicatePart As New List(Of FBM.PredicatePart)
            Dim lbSomeUsedInExit As Boolean = False
            Dim lsPredicatePart As String = ""

            Dim liEntryCount = From rcr In arRoleConstraintSubtype.RoleConstraintRole
                               Where rcr.IsEntry = True
                               Select rcr Distinct.Count

            Dim liEntriesProcessed As Integer = 0

            lrVerbaliser.VerbaliseQuantifier("If ")

            Dim lrArgument As FBM.RoleConstraintArgument

            If arRoleConstraintSubtype.Argument.Count = 0 Then
                lrVerbaliser.VerbaliseError("Complete the creation of the arguments for this Role Constraint.")
            Else
                lrArgument = arRoleConstraintSubtype.getArgument(1) ' Argument(0)

                Dim larModelObject As List(Of FBM.ModelObject)
                larModelObject = arRoleConstraintSubtype.GetCommonArgumentModelObjects

                Call lrArgument.ProjectArgumentReading(lrVerbaliser, New List(Of FBM.ModelObject))

                lrVerbaliser.VerbaliseQuantifier(", then")
                lrVerbaliser.HTW.WriteBreak()

                If arRoleConstraintSubtype.Argument.Count > 1 Then
                    lrArgument = arRoleConstraintSubtype.getArgument(2)

                    Call lrArgument.ProjectArgumentReading(lrVerbaliser, larModelObject)
                Else
                    lrVerbaliser.VerbaliseError("A Subset Role Constraint needs more than one Argument.")
                End If
            End If

            Me.WebBrowser.DocumentText = lrVerbaliser.Verbalise

        Catch ex As Exception
            Dim lsMessage1 As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage1 &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Sub

    Public Sub VerbaliseFactInstance(ByVal arFactInstance As FBM.FactInstance)

        Try
            Dim liInd As Integer = 0

            Dim lrVerbaliser As New FBM.ORMVerbailser
            Call lrVerbaliser.Reset()

            lrVerbaliser.VerbaliseQuantifier("Fact with Fact.Id: '" & arFactInstance.Id & "' is a Fact of FactType: '")
            lrVerbaliser.VerbaliseModelObject(arFactInstance.FactType)
            lrVerbaliser.VerbaliseQuantifier("'")
            lrVerbaliser.HTW.WriteBreak()
            lrVerbaliser.HTW.WriteBreak()

            lrVerbaliser.VerbaliseQuantifier("Fact Reading:")
            lrVerbaliser.HTW.WriteBreak()
            lrVerbaliser.HTW.WriteBreak()
            lrVerbaliser.HTW.Write(arFactInstance.Fact.GetReading)

            lrVerbaliser.HTW.WriteBreak()
            lrVerbaliser.HTW.WriteBreak()
            lrVerbaliser.VerbaliseQuantifier("Fact Data Instances:")
            lrVerbaliser.HTW.WriteBreak()
            For Each lrFactDataInstance In arFactInstance.Data
                lrVerbaliser.VerbaliseQuantifier(lrFactDataInstance.Data & ": ")
                lrVerbaliser.VerbaliseBlackText("X: " & lrFactDataInstance.X.ToString)
                lrVerbaliser.VerbaliseBlackText("Y: " & lrFactDataInstance.Y.ToString)
                lrVerbaliser.HTW.WriteBreak()
            Next

            Me.WebBrowser.DocumentText = lrVerbaliser.Verbalise

        Catch ex As Exception
            Dim lsMessage1 As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage1 &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Sub

    Public Sub VerbaliseFact(ByVal arFact As FBM.Fact)

        Try
            Dim liInd As Integer = 0

            Dim lrVerbaliser As New FBM.ORMVerbailser
            Call lrVerbaliser.Reset()

            lrVerbaliser.VerbaliseQuantifier("Fact with Fact.Id: '" & arFact.Id & "' is a Fact of FactType: '")
            lrVerbaliser.VerbaliseModelObject(arFact.FactType)
            lrVerbaliser.VerbaliseQuantifier("'")
            lrVerbaliser.HTW.WriteBreak()
            lrVerbaliser.HTW.WriteBreak()

            lrVerbaliser.VerbaliseQuantifier("Fact Reading:")
            lrVerbaliser.HTW.WriteBreak()
            lrVerbaliser.HTW.WriteBreak()
            lrVerbaliser.HTW.Write(arFact.GetReading)

            Me.WebBrowser.DocumentText = lrVerbaliser.Verbalise

        Catch ex As Exception
            Dim lsMessage1 As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage1 &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Sub

    Public Sub VerbaliseFactType(ByVal arFactType As FBM.FactType,
                                 Optional ByVal arRole As FBM.Role = Nothing)
        '------------------------------------------------------
        'PSEUDOCODE
        '  * Declare that the FactType(Name) is an FactType
        '------------------------------------------------------

        Dim lrVerbaliser As New FBM.ORMVerbailser

        Try
            Dim lrFactTypeReading As FBM.FactTypeReading
            Dim liInd As Integer = 0
            Dim lrFact As FBM.Fact
            Dim lrRole As FBM.Role
            Dim lrRoleConstraint As FBM.RoleConstraint

            Call lrVerbaliser.Reset()

            'CodeSafe
            If arFactType.GetType = GetType(FBM.FactTypeInstance) Then
                arFactType = CType(arFactType, FBM.FactTypeInstance).FactType
            End If

            If arRole IsNot Nothing Then
                lrVerbaliser.VerbaliseQuantifier("Role in Fact Type")
                lrVerbaliser.HTW.WriteBreak()
                lrVerbaliser.HTW.WriteBreak()
            End If

            '------------------------------------------------------
            'Declare that the EntityType(Name) is an EntityType
            '------------------------------------------------------
            lrVerbaliser.VerbaliseModelObject(arFactType)
            If arFactType.IsLinkFactType Then
                lrVerbaliser.VerbaliseQuantifier(" is a Link Fact Type.")
            Else
                lrVerbaliser.VerbaliseQuantifier(" is a Fact Type.")
            End If

            lrVerbaliser.HTW.WriteBreak()
            lrVerbaliser.HTW.WriteBreak()

            Select Case arFactType.Arity
                Case Is = 1
                Case Is = 2
                    If arFactType.FactTypeReading.Count > 0 Then
                        lrFactTypeReading = arFactType.FactTypeReading(0)
                        lrFactTypeReading.GetReadingText(lrVerbaliser)
                        lrVerbaliser.HTW.WriteBreak()

                        If arFactType.IsManyToOneByRoleOrder(lrFactTypeReading.RoleList) Then
                            lrVerbaliser.VerbaliseIndent()
                            lrVerbaliser.VerbaliseQuantifier("Each ")
                            lrVerbaliser.VerbaliseModelObject(lrFactTypeReading.RoleList(0).JoinedORMObject)
                            lrVerbaliser.VerbalisePredicateText(" " & lrFactTypeReading.PredicatePart(0).PredicatePartText)
                            If arFactType.RoleGroup(0).Mandatory Then
                                lrVerbaliser.VerbaliseQuantifier(" one ")
                            Else
                                lrVerbaliser.VerbaliseQuantifier(" at most one ")
                            End If

                            lrVerbaliser.VerbalisePredicateText(lrFactTypeReading.PredicatePart(1).PreBoundText)
                            lrVerbaliser.VerbaliseModelObject(lrFactTypeReading.RoleList(1).JoinedORMObject)

                            lrVerbaliser.HTW.WriteBreak()
                            lrVerbaliser.VerbaliseIndent()

                            lrVerbaliser.VerbaliseQuantifier("It is possible that more than one ")
                            lrVerbaliser.VerbaliseModelObject(lrFactTypeReading.RoleList(0).JoinedORMObject)
                            lrVerbaliser.VerbalisePredicateText(" " & lrFactTypeReading.PredicatePart(0).PredicatePartText)
                            lrVerbaliser.VerbaliseQuantifier(" the same ")
                            lrVerbaliser.VerbalisePredicateText(lrFactTypeReading.PredicatePart(1).PreBoundText)
                            lrVerbaliser.VerbaliseModelObject(lrFactTypeReading.RoleList(1).JoinedORMObject)
                            lrVerbaliser.HTW.WriteBreak()
                        End If
                        lrVerbaliser.HTW.WriteBreak()
                    End If

                Case Else
                    lrVerbaliser.HTW.WriteBreak()
            End Select

            lrVerbaliser.VerbaliseHeading("Fact Type Readings:")
            lrVerbaliser.HTW.WriteBreak()
            lrVerbaliser.HTW.WriteBreak()

            If arFactType.IsLinkFactType Then
                Dim lrParentFactType As FBM.FactType = arFactType.LinkFactTypeRole.FactType

                If lrParentFactType.FactTypeReading.Count > 0 Then
                    lrFactTypeReading = lrParentFactType.FactTypeReading(0)
                    lrVerbaliser.VerbaliseQuantifier("If ")
                    lrVerbaliser.VerbaliseModelObject(lrParentFactType)
                    lrVerbaliser.VerbaliseQuantifier(" is defined by '")
                    lrFactTypeReading.GetReadingText(lrVerbaliser, False, True)
                    lrVerbaliser.VerbaliseQuantifier("'")
                    lrVerbaliser.HTW.WriteBreak()
                Else
                    lrVerbaliser.VerbaliseError("Error: Provide a Fact Type Reading for the Fact Type that this Link Fact Type belongs to.")
                    lrVerbaliser.HTW.WriteBreak()
                End If
            End If

            For Each lrFactTypeReading In arFactType.FactTypeReading
                lrFactTypeReading.GetReadingText(lrVerbaliser, False, True)
                lrVerbaliser.HTW.WriteBreak()
            Next

            lrVerbaliser.HTW.WriteBreak()
            lrVerbaliser.VerbaliseHeading("Sample Facts:")
            lrVerbaliser.HTW.WriteBreak()
            lrVerbaliser.HTW.WriteBreak()

            If arFactType.Fact.Count = 0 Then
                lrVerbaliser.VerbaliseQuantifierLight("There are no Sample Facts against the Fact Type.")
                lrVerbaliser.HTW.WriteBreak()
            Else
                For Each lrFact In arFactType.Fact
                    Try
                        lrVerbaliser.HTW.Write(lrFact.GetReading)
                        lrVerbaliser.HTW.WriteBreak()
                    Catch ex As Exception
                        'Don't abort if you can't write the fact. This allows the user to delete the fact.
                    End Try

                Next
            End If

            '------------------------
            'Uniqueness Constraints
            '------------------------
            lrVerbaliser.HTW.WriteBreak()
            lrVerbaliser.VerbaliseHeading("Uniqueness Constraints:")
            lrVerbaliser.HTW.WriteBreak()
            lrVerbaliser.HTW.WriteBreak()


            If arFactType.HasTotalRoleConstraint Then
                lrRoleConstraint = arFactType.InternalUniquenessConstraint(0)

                lrVerbaliser.VerbaliseModelObject(arFactType)
                lrVerbaliser.VerbaliseQuantifier(" has a Total Internal Uniqueness Constraint.")
                lrVerbaliser.HTW.WriteBreak()
                lrVerbaliser.VerbaliseQuantifier("Role Constraint:")
                lrVerbaliser.VerbaliseModelObject(lrRoleConstraint)
                lrVerbaliser.HTW.WriteBreak()
                lrVerbaliser.VerbaliseQuantifier("In each population of ")
                lrVerbaliser.VerbaliseModelObject(arFactType)
                lrVerbaliser.VerbaliseQuantifier(" each")

                liInd = 0
                For Each lrRole In arFactType.RoleGroup
                    lrVerbaliser.VerbaliseModelObject(lrRole.JoinedORMObject)
                    liInd += 1
                    If liInd < arFactType.Arity Then
                        lrVerbaliser.VerbaliseSeparator(",")
                    End If
                Next

                lrVerbaliser.VerbaliseQuantifier(" combination occurs at most once.")
                lrVerbaliser.HTW.WriteBreak()
                lrVerbaliser.VerbaliseQuantifier("This association of ")

                liInd = 0
                For Each lrRole In arFactType.RoleGroup
                    lrVerbaliser.HTW.Write(" ")
                    lrVerbaliser.VerbaliseModelObject(lrRole.JoinedORMObject)
                    If liInd < arFactType.Arity Then
                        lrVerbaliser.VerbaliseSeparator(",")
                    End If
                Next

                lrVerbaliser.VerbaliseQuantifier(" provides the preferred identification scheme for ")
                lrVerbaliser.VerbaliseModelObject(arFactType)
                lrVerbaliser.VerbaliseQuantifier(".")
                lrVerbaliser.HTW.WriteBreak()

            ElseIf arFactType.Arity = 2 Then
                '--------------------
                'Is Binary FactType
                '--------------------
                Dim larRole As New List(Of FBM.Role)

                For Each lrRoleConstraint In arFactType.InternalUniquenessConstraint

                    lrVerbaliser.VerbaliseQuantifier("Role Constraint: ")
                    lrVerbaliser.VerbaliseModelObject(lrRoleConstraint)
                    lrVerbaliser.HTW.WriteBreak()

                    lrRole = lrRoleConstraint.Role(0)
                    larRole.Clear()
                    larRole.Add(lrRole)

                    For Each lrRole In arFactType.RoleGroup
                        If lrRole.Id <> lrRoleConstraint.Role(0).Id Then
                            larRole.Add(lrRole)
                        End If
                    Next

                    lrFactTypeReading = arFactType.FindSuitableFactTypeReadingByRoles(larRole)

                    If IsSomething(lrFactTypeReading) Then
                        lrVerbaliser.VerbaliseIndent()
                        lrVerbaliser.VerbaliseQuantifier("Each ")
                        lrVerbaliser.VerbaliseModelObject(lrFactTypeReading.PredicatePart(0).Role.JoinedORMObject)

                        If lrRoleConstraint.RoleConstraintRole(0).Role.Id = lrFactTypeReading.RoleList(0).Id Then
                            lrVerbaliser.VerbalisePredicateText(" " & lrFactTypeReading.PredicatePart(0).PredicatePartText)
                            If lrFactTypeReading.PredicatePart(0).Role.Mandatory Then
                                lrVerbaliser.VerbaliseQuantifier(" one ")
                            Else
                                lrVerbaliser.VerbaliseQuantifier(" at most one ")
                            End If
                            lrVerbaliser.VerbalisePredicateText(lrFactTypeReading.PredicatePart(1).PreBoundText)
                            lrVerbaliser.VerbaliseModelObject(lrFactTypeReading.PredicatePart(1).Role.JoinedORMObject)
                        Else
                            If arFactType.Is1To1BinaryFactType Then
                                lrVerbaliser.VerbalisePredicateText(" " & lrFactTypeReading.PredicatePart(0).PredicatePartText)
                                If lrFactTypeReading.PredicatePart(0).Role.Mandatory Then
                                    lrVerbaliser.VerbaliseQuantifier(" one ")
                                Else
                                    lrVerbaliser.VerbaliseQuantifier(" at most one ")
                                End If
                                lrVerbaliser.VerbalisePredicateText(lrFactTypeReading.PredicatePart(1).PreBoundText)
                                lrVerbaliser.VerbaliseModelObject(lrFactTypeReading.PredicatePart(1).Role.JoinedORMObject)
                            Else
                                If lrFactTypeReading.PredicatePart(0).Role.Mandatory Then
                                    lrVerbaliser.VerbalisePredicateText(" " & lrFactTypeReading.PredicatePart(0).PredicatePartText)
                                    lrVerbaliser.VerbaliseQuantifier(" at least one ")
                                    lrVerbaliser.VerbaliseModelObject(lrFactTypeReading.PredicatePart(1).Role.JoinedORMObject)
                                Else
                                    lrVerbaliser.VerbaliseQuantifier(" possibly ")
                                    lrVerbaliser.VerbalisePredicateText(" " & lrFactTypeReading.PredicatePart(0).PredicatePartText)
                                    lrVerbaliser.VerbaliseQuantifier(" more than one ")
                                    lrVerbaliser.VerbaliseModelObject(lrFactTypeReading.PredicatePart(1).Role.JoinedORMObject)
                                End If
                            End If
                        End If
                    End If

                    If (arFactType.FactTypeReading.Count = 2) Then
                        If Not arFactType.Is1To1BinaryFactType Then
                            lrFactTypeReading = arFactType.FactTypeReading.Find(Function(x) x.Id <> lrFactTypeReading.Id)
                            lrVerbaliser.HTW.WriteBreak()
                            lrVerbaliser.VerbaliseIndent()
                            lrVerbaliser.VerbaliseQuantifier("Each ")
                            lrVerbaliser.VerbaliseModelObject(lrFactTypeReading.PredicatePart(0).Role.JoinedORMObject)
                            If lrRoleConstraint.RoleConstraintRole(0).Role.Id = lrFactTypeReading.RoleList(0).Id Then
                                lrVerbaliser.VerbalisePredicateText(" " & lrFactTypeReading.PredicatePart(0).PredicatePartText)
                                If lrFactTypeReading.PredicatePart(0).Role.Mandatory Then
                                    lrVerbaliser.VerbaliseQuantifier(" one ")
                                Else
                                    lrVerbaliser.VerbaliseQuantifier(" at most one ")
                                End If
                                lrVerbaliser.VerbaliseModelObject(lrFactTypeReading.PredicatePart(1).Role.JoinedORMObject)
                            Else
                                If lrFactTypeReading.PredicatePart(0).Role.Mandatory Then
                                    lrVerbaliser.VerbalisePredicateText(" " & lrFactTypeReading.PredicatePart(0).PredicatePartText)
                                    lrVerbaliser.VerbaliseQuantifier(" at least one ")
                                    lrVerbaliser.VerbaliseModelObject(lrFactTypeReading.PredicatePart(1).Role.JoinedORMObject)
                                Else
                                    lrVerbaliser.VerbaliseQuantifier(" possibly ")
                                    lrVerbaliser.VerbalisePredicateText(" " & lrFactTypeReading.PredicatePart(0).PredicatePartText)
                                    lrVerbaliser.VerbaliseQuantifier(" more than one ")
                                    lrVerbaliser.VerbaliseModelObject(lrFactTypeReading.PredicatePart(1).Role.JoinedORMObject)
                                End If
                            End If
                        End If
                    Else
                        lrVerbaliser.HTW.WriteBreak()
                        lrVerbaliser.VerbaliseIndent()
                        lrVerbaliser.VerbaliseError("Provide a reverse Fact Type Reading for this Fact Type")
                        lrVerbaliser.HTW.WriteBreak()
                        lrVerbaliser.HTW.WriteBreak()
                    End If

                    lrVerbaliser.HTW.WriteBreak()
                    lrVerbaliser.HTW.WriteBreak()
                Next
            Else
                '-------------------------------------------
                'Is Partial Internal Uniqueness Constraint
                '-------------------------------------------
                For Each lrRoleConstraint In arFactType.InternalUniquenessConstraint

                    lrVerbaliser.VerbaliseQuantifier("Role Constraint: ")
                    lrVerbaliser.VerbaliseModelObject(lrRoleConstraint)
                    lrVerbaliser.HTW.WriteBreak()
                    lrVerbaliser.VerbaliseIndent()
                    lrVerbaliser.VerbaliseQuantifier("In each population of ")
                    lrVerbaliser.VerbaliseModelObject(arFactType)
                    lrVerbaliser.VerbaliseQuantifier(" each")

                    liInd = 0
                    For Each lrRole In lrRoleConstraint.Role
                        lrVerbaliser.HTW.Write(" ")
                        lrVerbaliser.VerbaliseModelObject(lrRole.JoinedORMObject)
                        liInd += 1
                        If liInd < lrRoleConstraint.Role.Count Then
                            lrVerbaliser.VerbaliseSeparator(",")
                        End If
                    Next
                    lrVerbaliser.VerbaliseQuantifier(" combination ")

                    For Each lrRole In arFactType.RoleGroup
                        If Not IsSomething(lrRoleConstraint.Role.Find(AddressOf lrRole.Equals)) Then
                            lrVerbaliser.VerbaliseQuantifier("is unique and relates to exactly one instance of ")
                            lrVerbaliser.VerbaliseModelObject(lrRole.JoinedORMObject)
                        End If
                    Next

                    lrVerbaliser.HTW.WriteBreak()
                    lrVerbaliser.HTW.WriteBreak()
                Next
            End If

            '---------------------------
            'Mandatory RoleConstraints
            '---------------------------
            lrVerbaliser.HTW.WriteBreak()
            lrVerbaliser.HTW.WriteBreak()
            lrVerbaliser.VerbaliseHeading("Mandatory Role Constraints:")
            lrVerbaliser.HTW.WriteBreak()
            lrVerbaliser.HTW.WriteBreak()

            If arFactType.RoleGroup.FindAll(Function(x) x.Mandatory = True).Count = 0 Then
                lrVerbaliser.VerbaliseQuantifierLight("There are no Mandatory Role Constraints against Roles of the Fact Type.")
            Else
                For Each lrRole In arFactType.RoleGroup
                    If lrRole.Mandatory = True Then
                        lrVerbaliser.VerbaliseQuantifier("For each instance of ")
                        lrVerbaliser.VerbaliseModelObject(lrRole.JoinedORMObject)
                        lrVerbaliser.VerbaliseQuantifier(" it is mandatory that that instance participate in ")
                        lrVerbaliser.VerbaliseModelObject(arFactType)
                        lrVerbaliser.HTW.Write(" ")
                        lrVerbaliser.HTW.WriteBreak()
                    End If
                Next
            End If

            lrVerbaliser.HTW.WriteBreak()
            lrVerbaliser.HTW.WriteBreak()

            lrVerbaliser.VerbaliseHeading("Associated Fact Types")
            lrVerbaliser.HTW.WriteBreak()

            If arFactType.allRolesJoinSomething Then

                Dim larFactType = From FactType In arFactType.Model.FactType
                                  From Role In FactType.RoleGroup
                                  Where Role.JoinedORMObject IsNot Nothing
                                  Where Role.JoinedORMObject.Id = arFactType.Id
                                  Where FactType.IsLinkFactType = False
                                  Select FactType

                For Each lrFactType In larFactType
                    lrVerbaliser.VerbaliseModelObject(lrFactType)
                    lrVerbaliser.HTW.WriteBreak()
                Next
            End If

            Me.WebBrowser.DocumentText = lrVerbaliser.Verbalise

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message

            lrVerbaliser.VerbaliseBlackText(lsMessage)

            Me.WebBrowser.DocumentText = lrVerbaliser.Verbalise
        End Try

    End Sub

    Public Sub verbaliseModelElement(ByRef arModelElement As FBM.ModelObject)

        Select Case arModelElement.ConceptType
            Case Is = pcenumConceptType.EntityType
                Call Me.VerbaliseEntityType(CType(arModelElement, FBM.EntityType))
            Case Is = pcenumConceptType.ValueType
                Call Me.VerbaliseValueType(CType(arModelElement, FBM.ValueType))
        End Select

    End Sub

    Public Sub VerbalisePage(ByRef arPage As FBM.Page)

        Try

            Dim lrVerbaliser As New FBM.ORMVerbailser
            Call lrVerbaliser.Reset()

            '------------------------------------------------------
            'Declare that Page is in a Model
            '------------------------------------------------------
            lrVerbaliser.VerbaliseQuantifier("'" & arPage.Name & "' is a Page in Model, '" & arPage.Model.Name & "'")

            Me.WebBrowser.DocumentText = lrVerbaliser.Verbalise

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Sub

    Public Sub VerbalisePGSNode(ByVal arNode As PGS.Node)

        Try

            Dim lrVerbaliser As New FBM.ORMVerbailser
            Call lrVerbaliser.Reset()

            '------------------------------------------------------
            'Declare that the EntityType(Name) is an EntityType
            '------------------------------------------------------
            lrVerbaliser.VerbalisePredicateText(arNode.Data)
            lrVerbaliser.VerbaliseQuantifier(" is a Node Type.")
            lrVerbaliser.HTW.WriteBreak()
            lrVerbaliser.HTW.WriteBreak()

            lrVerbaliser.VerbaliseHeading("Properties")
            lrVerbaliser.HTW.WriteBreak()
            For Each lrColumn In arNode.RDSTable.Column
                lrVerbaliser.VerbalisePredicateText(lrColumn.Name)
                lrVerbaliser.HTW.WriteBreak()
            Next

            Me.WebBrowser.DocumentText = lrVerbaliser.Verbalise

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Sub

    Public Sub VerbaliseTable(ByVal arTable As RDS.Table)

        Dim lrVerbaliser As New FBM.ORMVerbailser
        Call lrVerbaliser.Reset()

        '------------------------------------------------------
        'Declare that the EntityType(Name) is an EntityType
        '------------------------------------------------------
        lrVerbaliser.VerbalisePredicateText(arTable.Name)
        lrVerbaliser.VerbaliseQuantifier(" is an Entity.")
        lrVerbaliser.HTW.WriteBreak()
        lrVerbaliser.HTW.WriteBreak()

        lrVerbaliser.VerbaliseQuantifier("ORM Level: ")
        lrVerbaliser.VerbaliseModelObjectLight(arTable.FBMModelElement)
        lrVerbaliser.VerbaliseQuantifier(" (" & arTable.FBMModelElement.ConceptType.ToString & ")")
        lrVerbaliser.HTW.WriteBreak()
        lrVerbaliser.HTW.WriteBreak()

        lrVerbaliser.VerbaliseHeading("Constraints:")
        lrVerbaliser.HTW.WriteBreak()
        lrVerbaliser.HTW.WriteBreak()

        If arTable.Index.Count = 0 Then
            lrVerbaliser.VerbaliseBlackText("There are no Indexes for this Entity")
            lrVerbaliser.HTW.WriteBreak()
            lrVerbaliser.HTW.WriteBreak()
        Else
            For Each lrIndex In arTable.Index
                lrVerbaliser.VerbalisePredicateText(lrIndex.Name & " ")

                If lrIndex.IsPrimaryKey Then
                    lrVerbaliser.VerbaliseQuantifierLight("is a Primary Key over Columns, ")
                Else
                    lrVerbaliser.VerbaliseQuantifierLight("is a Unique Key over Columns, ")
                End If

                lrVerbaliser.VerbaliseBlackText("(")
                Dim liInd As Integer = 1
                For Each lrColumn In lrIndex.Column
                    lrVerbaliser.VerbaliseModelObject(lrColumn.ActiveRole.JoinedORMObject)
                    If liInd < lrIndex.Column.Count Then lrVerbaliser.VerbaliseBlackText(", ")
                    liInd += 1
                Next
                lrVerbaliser.VerbaliseBlackText(")")
                lrVerbaliser.HTW.WriteBreak()
            Next
        End If

        Me.WebBrowser.DocumentText = lrVerbaliser.Verbalise

    End Sub

    Public Sub VerbaliseValueType(ByVal arValueType As FBM.ValueType)
        '------------------------------------------------------
        'PSEUDOCODE
        '  * Declare that the ValueType(Name) is an ValueType
        '------------------------------------------------------
        Dim lrVerbaliser As New FBM.ORMVerbailser
        Call lrVerbaliser.Reset()

        '------------------------------------------------------
        'Declare that the EntityType(Name) is an EntityType
        '------------------------------------------------------
        lrVerbaliser.VerbaliseModelObject(arValueType)
        lrVerbaliser.VerbaliseQuantifier(" is a Value Type.")
        lrVerbaliser.HTW.WriteBreak()
        lrVerbaliser.HTW.WriteBreak()

        '-------------------------------------------------
        'FOR EACH IncomingLink (from a Role)
        '  Verbalise the FactType for the associated Role
        'LOOP 
        '-------------------------------------------------
        lrVerbaliser.VerbaliseQuantifier("Data Type: ")
        lrVerbaliser.HTW.Write(arValueType.DataType.ToString)
        lrVerbaliser.HTW.WriteBreak()


        If arValueType.DataTypeLength <> 0 Then
            lrVerbaliser.VerbaliseQuantifier("Data Type Length: ")
            lrVerbaliser.HTW.Write(arValueType.DataTypeLength.ToString)
            lrVerbaliser.HTW.WriteBreak()
        End If

        If arValueType.DataTypePrecision <> 0 Then
            lrVerbaliser.VerbaliseQuantifier("Data Type Precition: ")
            lrVerbaliser.HTW.Write(arValueType.DataTypePrecision.ToString)
            lrVerbaliser.HTW.WriteBreak()
        End If

        '-------------------------------------------------
        'FOR EACH IncomingLink (from a Role)
        '  Verbalise the FactType for the associated Role
        'LOOP 
        '-------------------------------------------------
        lrVerbaliser.HTW.WriteBreak()
        lrVerbaliser.HTW.WriteBreak()
        lrVerbaliser.VerbaliseQuantifier("Value Constraints:")
        lrVerbaliser.HTW.WriteBreak()
        lrVerbaliser.HTW.WriteBreak()


        Dim liCounter As Integer = 0
        Dim lrValueTypeConstraintString As String = ""

        If arValueType.ValueConstraint.Count > 0 Then
            lrVerbaliser.VerbaliseQuantifier("Possible Values for ")
            lrVerbaliser.VerbaliseModelObject(arValueType)
            lrVerbaliser.VerbaliseQuantifier(" are {")
            For Each lrValueTypeConstraintString In arValueType.ValueConstraint
                liCounter += 1
                If liCounter = 1 Then
                    lrVerbaliser.HTW.Write("'" & lrValueTypeConstraintString & "'")
                Else
                    lrVerbaliser.HTW.Write(", '" & lrValueTypeConstraintString & "'")
                End If
            Next
            lrVerbaliser.VerbaliseQuantifier("}")
        Else
            lrVerbaliser.VerbaliseQuantifier("There are no Value Constraints for this Value Type.")
        End If

        lrVerbaliser.HTW.WriteBreak()
        lrVerbaliser.HTW.WriteBreak()
        lrVerbaliser.VerbaliseQuantifier("Sample Values:")
        lrVerbaliser.HTW.WriteBreak()

        'LINQ
        Dim RoleData = From ft In zrModel.FactType,
                            rl In ft.RoleGroup,
                            fct In ft.Fact,
                            data In fct.Data
                       Where rl.JoinedORMObject.Id = arValueType.Id _
                            And data.Role.JoinedORMObject.Id = arValueType.Id
                       Select data Distinct

        Dim lrRoleData As FBM.FactData

        For Each lrRoleData In RoleData
            lrVerbaliser.HTW.Write("'" & lrRoleData.Data & "'")
        Next

        '-------------------------------------------------
        'FOR EACH IncomingLink (from a Role)
        '  Verbalise the FactType for the associated Role
        'LOOP 
        '-------------------------------------------------
        lrVerbaliser.HTW.WriteBreak()
        lrVerbaliser.HTW.WriteBreak()
        lrVerbaliser.VerbaliseQuantifier("Fact Types:")
        lrVerbaliser.HTW.WriteBreak()
        lrVerbaliser.HTW.WriteBreak()


        'LINQ
        Dim FactType = From ft In zrModel.FactType,
                            rl In ft.RoleGroup
                       Where rl.JoinedORMObject.Id = arValueType.Id
                       Select ft Distinct

        Dim lrFactType As FBM.FactType


        For Each lrFactType In FactType
            lrVerbaliser.VerbaliseModelObject(lrFactType)
            lrVerbaliser.VerbalisePredicateText(" (")
            If lrFactType.FactTypeReading.Count > 0 Then
                Call lrFactType.FactTypeReading(0).GetReadingText(lrVerbaliser)
            End If
            lrVerbaliser.VerbalisePredicateText(")")
            lrVerbaliser.HTW.WriteBreak()
        Next

        Me.WebBrowser.DocumentText = lrVerbaliser.Verbalise

    End Sub

    Public Sub verbaliseRole(ByRef arRole As FBM.Role)

        Dim lrVerbaliser As New FBM.ORMVerbailser
        Call lrVerbaliser.Reset()

        '------------------------------------------------------
        'Declare that the EntityType(Name) is an EntityType
        '------------------------------------------------------        
        lrVerbaliser.VerbaliseQuantifier("Role with Id: ")
        lrVerbaliser.VerbaliseBlackText(arRole.Id)
        lrVerbaliser.VerbaliseQuantifier(" is part of Fact Type, ")
        lrVerbaliser.VerbaliseModelObject(arRole.FactType)
        lrVerbaliser.HTW.WriteBreak()
        lrVerbaliser.HTW.WriteBreak()

        Me.WebBrowser.DocumentText = lrVerbaliser.Verbalise

    End Sub

    Public Sub FindTextAndColourInRichboxText(ByRef arRichTextBox As RichTextBox, ByVal arColour As Color, ByVal asString As String)

        Dim lFoundPos As Integer = 0

        If IsSomething(asString) Then
            lFoundPos = arRichTextBox.Find(asString, arRichTextBox.TextLength, RichTextBoxFinds.WholeWord)
        Else
            lFoundPos = -1
        End If
        While (lFoundPos >= 0)


            arRichTextBox.SelectionStart = lFoundPos
            'The SelLength property is set to 0 as
            'soon as you change SelStart
            arRichTextBox.SelectionLength = Len(asString)
            arRichTextBox.SelectionColor = arColour
            arRichTextBox.DeselectAll()

            'Attempt to find the next match
            lFoundPos = arRichTextBox.Find(asString, lFoundPos + Len(asString), RichTextBoxFinds.WholeWord)

            If lFoundPos = arRichTextBox.TextLength - Len(asString) Then
                arRichTextBox.DeselectAll()
                Exit While
            End If

        End While

    End Sub

    Public Sub FindTextAndUnderline(ByRef arRichTextBox As RichTextBox, ByVal asString As String)

        Dim lFoundPos As Integer = 0

        lFoundPos = arRichTextBox.Find(asString, arRichTextBox.TextLength, RichTextBoxFinds.WholeWord)
        While (lFoundPos >= 0)


            arRichTextBox.SelectionStart = lFoundPos
            'The SelLength property is set to 0 as
            'soon as you change SelStart
            arRichTextBox.SelectionLength = Len(asString)
            arRichTextBox.SelectionFont = New Font(arRichTextBox.Font, FontStyle.Underline)
            arRichTextBox.SelectionColor = Gray
            arRichTextBox.DeselectAll()

            'Attempt to find the next match
            lFoundPos = arRichTextBox.Find(asString, lFoundPos + Len(asString), RichTextBoxFinds.WholeWord)

            If lFoundPos = arRichTextBox.TextLength - Len(asString) Then
                arRichTextBox.DeselectAll()
                Exit While
            End If

        End While

    End Sub

    Private Sub frm_ORM_verbalisation_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing

        prApplication.ToolboxForms.RemoveAll(AddressOf Me.EqualsByName)
        'If IsSomething(frmMain) Then
        '    frmMain.zfrm_ORM_verbalisation = Nothing
        'End If

    End Sub

    Private Sub WebBrowser_Navigating(ByVal sender As Object, ByVal e As System.Windows.Forms.WebBrowserNavigatingEventArgs) Handles WebBrowser.Navigating

        Dim lasURLArgument() As String
        Dim lsModelObjectName As String

        lasURLArgument = e.Url.ToString.Split(":")

        If lasURLArgument(0) = "elementid" Then

            lsModelObjectName = lasURLArgument(1)
            Dim lrDiagramSpyPage As New FBM.DiagramSpyPage(Me.zrModel, "123", "Diagram Spy", pcenumLanguage.ORMModel)

            Dim lrModelObject As FBM.ModelObject

            lrModelObject = Me.zrModel.GetModelObjectByName(lsModelObjectName)

            If frmMain.IsDiagramSpyFormLoaded Then
                prApplication.ActivePages.Find(Function(x) x.Tag.GetType Is GetType(FBM.DiagramSpyPage)).Close()
            End If

            '------------------------------------------------------------------------------------------
            'Cancel the Navigation so that the new verbalisation isn't wiped out.
            '  i.e. Because the Navication (e.URL) isn't to an actual URL, an error WebPage is shown,
            '  rather than the new Verbalisation. Cancelling the Navigation fixes this.
            '------------------------------------------------------------------------------------------
            e.Cancel = True

            Select Case lrModelObject.ConceptType
                Case Is = pcenumConceptType.ValueType
                    Call Me.VerbaliseValueType(lrModelObject)
                Case Is = pcenumConceptType.EntityType
                    Call Me.VerbaliseEntityType(lrModelObject)
                Case Is = pcenumConceptType.FactType
                    Call Me.VerbaliseFactType(lrModelObject)
                Case Is = pcenumConceptType.RoleConstraint
                    'TBA
            End Select

            Call frmMain.LoadDiagramSpy(lrDiagramSpyPage, lrModelObject)

        End If

        'e.Cancel = True

    End Sub

    Public Sub VerbaliseEntity(ByVal arEntity As ERD.Entity)

        Dim lrVerbaliser As New FBM.ORMVerbailser
        Call lrVerbaliser.Reset()

        '------------------------------------------------------
        'Declare that the EntityType(Name) is an EntityType
        '------------------------------------------------------
        lrVerbaliser.VerbalisePredicateText(arEntity.Data)
        lrVerbaliser.VerbaliseQuantifier(" is an Entity.")
        lrVerbaliser.HTW.WriteBreak()
        lrVerbaliser.HTW.WriteBreak()

        If arEntity.isSubtype Then
            lrVerbaliser.VerbaliseModelObject(arEntity.RDSTable.FBMModelElement)
            For Each lrSupertypeTable In arEntity.RDSTable.getSupertypeTables
                lrVerbaliser.VerbaliseQuantifier(" is a kind of ")
                lrVerbaliser.VerbaliseModelObject(lrSupertypeTable.FBMModelElement)
                lrVerbaliser.HTW.WriteBreak()
            Next
            lrVerbaliser.HTW.WriteBreak()
        End If

        lrVerbaliser.VerbaliseHeading("Constraints:")
        lrVerbaliser.HTW.WriteBreak()
        lrVerbaliser.HTW.WriteBreak()

        If arEntity.RDSTable.Index.Count = 0 Then
            lrVerbaliser.VerbaliseBlackText("There are no Indexes for this Entity")
            lrVerbaliser.HTW.WriteBreak()
            lrVerbaliser.HTW.WriteBreak()
        Else
            For Each lrIndex In arEntity.RDSTable.Index
                lrVerbaliser.VerbalisePredicateText(lrIndex.Name & " ")

                If lrIndex.IsPrimaryKey Then
                    lrVerbaliser.VerbaliseQuantifierLight("is a Primary Key over Column/s, ")
                Else
                    lrVerbaliser.VerbaliseQuantifierLight("is a Unique Key over Column/s, ")
                End If

                lrVerbaliser.VerbaliseBlackText("(")
                Dim liInd As Integer = 1
                For Each lrColumn In lrIndex.Column
                    lrVerbaliser.VerbaliseBlackText(lrColumn.Name) '20220225-VM-Was ActiveRole.JoinedORMObject)
                    If liInd < lrIndex.Column.Count Then lrVerbaliser.VerbaliseBlackText(", ")
                    liInd += 1
                Next
                lrVerbaliser.VerbaliseBlackText(")")
                lrVerbaliser.HTW.WriteBreak()
            Next
        End If

        Me.WebBrowser.DocumentText = lrVerbaliser.Verbalise

    End Sub

    Public Sub VerbaliseAttribute(ByVal arAttribute As ERD.Attribute)

        Try
            Dim lrVerbaliser As New FBM.ORMVerbailser
            Call lrVerbaliser.Reset()

            '------------------------------------------------------
            'Declare that the EntityType(Name) is an EntityType
            '------------------------------------------------------
            lrVerbaliser.VerbalisePredicateText(arAttribute.AttributeName)
            lrVerbaliser.VerbaliseQuantifier(" is an Attribute.")
            lrVerbaliser.HTW.WriteBreak()
            lrVerbaliser.HTW.WriteBreak()

            lrVerbaliser.VerbaliseQuantifier("Data Type: ")
            Dim liDataType As pcenumORMDataType = arAttribute.Column.getMetamodelDataType
            If liDataType = pcenumORMDataType.DataTypeNotSet Then
                lrVerbaliser.VerbaliseError(liDataType.ToString)
            Else
                lrVerbaliser.VerbaliseBlackText(liDataType.ToString)
            End If

            lrVerbaliser.HTW.WriteBreak()

            Select Case liDataType
                Case Is = pcenumORMDataType.NumericFloatCustomPrecision, _
                          pcenumORMDataType.NumericDecimal, _
                          pcenumORMDataType.NumericMoney

                    lrVerbaliser.VerbaliseQuantifier("Data Precision: ")
                    lrVerbaliser.VerbaliseBlackText(arAttribute.Column.getMetamodelDataTypePrecision.ToString)
                    lrVerbaliser.HTW.WriteBreak()
                Case Is = pcenumORMDataType.RawDataFixedLength, _
                          pcenumORMDataType.RawDataLargeLength, _
                          pcenumORMDataType.RawDataVariableLength, _
                          pcenumORMDataType.TextFixedLength, _
                          pcenumORMDataType.TextLargeLength, _
                          pcenumORMDataType.TextVariableLength
                    lrVerbaliser.VerbaliseQuantifier("Data Length: ")
                    lrVerbaliser.VerbaliseBlackText(arAttribute.Column.getMetamodelDataTypeLength.ToString)
                    lrVerbaliser.HTW.WriteBreak()
                Case Else
                    'Do Nothing
            End Select

            Dim lrFactTypeReading As FBM.FactTypeReading
            Dim lrFactType As FBM.FactType = arAttribute.Column.Role.FactType

            '===========================================
            Dim lrRoleConstraint As FBM.RoleConstraint

            If lrFactType.HasTotalRoleConstraint Then
                lrVerbaliser.HTW.WriteBreak()
                lrRoleConstraint = lrFactType.InternalUniquenessConstraint(0)

                lrVerbaliser.VerbaliseModelObject(lrFactType)
                lrVerbaliser.VerbaliseQuantifier(" has a Total Internal Uniqueness Constraint.")
                lrVerbaliser.HTW.WriteBreak()
                lrVerbaliser.HTW.WriteBreak()
                lrVerbaliser.VerbaliseQuantifier("Role Constraint:")
                lrVerbaliser.VerbaliseModelObject(lrRoleConstraint)
                lrVerbaliser.HTW.WriteBreak()
                lrVerbaliser.VerbaliseQuantifier("In each population of ")
                lrVerbaliser.VerbaliseModelObject(lrFactType)
                lrVerbaliser.VerbaliseQuantifier(" each ")

                Dim liInd As Integer = 0

                For Each lrRole In lrFactType.RoleGroup
                    lrVerbaliser.VerbaliseModelObject(lrRole.JoinedORMObject)
                    liInd += 1
                    If liInd < lrFactType.Arity Then
                        lrVerbaliser.VerbaliseSeparator(",")
                    End If
                Next

                lrVerbaliser.VerbaliseQuantifier(" combination occurs at most once.")
                lrVerbaliser.HTW.WriteBreak()
                lrVerbaliser.VerbaliseQuantifier("This association of ")

                liInd = 0
                For Each lrRole In lrFactType.RoleGroup
                    lrVerbaliser.HTW.Write(" ")
                    lrVerbaliser.VerbaliseModelObject(lrRole.JoinedORMObject)
                    If liInd < lrFactType.Arity Then
                        lrVerbaliser.VerbaliseSeparator(",")
                    End If
                Next

                lrVerbaliser.VerbaliseQuantifier(" provides the preferred identification scheme for ")
                lrVerbaliser.VerbaliseModelObject(lrFactType)
                lrVerbaliser.VerbaliseQuantifier(".")
                lrVerbaliser.HTW.WriteBreak()

                '===========================================
            ElseIf lrFactType.Arity = 2 Then
                If lrFactType.FactTypeReading.Count > 0 Then

                    lrVerbaliser.HTW.WriteBreak()
                    lrVerbaliser.VerbaliseHeading("ORM Level")
                    lrVerbaliser.HTW.WriteBreak()
                    lrVerbaliser.HTW.WriteBreak()

                    lrFactTypeReading = arAttribute.Column.Role.FactType.FactTypeReading(0)
                    lrFactTypeReading.GetReadingText(lrVerbaliser)
                    lrVerbaliser.VerbaliseTextLightGray(" Fact Type: ")
                    lrVerbaliser.VerbaliseModelObjectLightGray(arAttribute.Column.Role.FactType)
                    lrVerbaliser.HTW.WriteBreak()

                    lrVerbaliser.VerbaliseIndent()
                    lrVerbaliser.VerbaliseQuantifier("Each ")
                    lrVerbaliser.VerbaliseModelObject(lrFactType.RoleGroup(0).JoinedORMObject)
                    lrVerbaliser.VerbalisePredicateText(" " & lrFactTypeReading.PredicatePart(0).PredicatePartText)
                    lrVerbaliser.VerbaliseQuantifier(" at most one ")
                    lrVerbaliser.VerbalisePredicateText(lrFactTypeReading.PredicatePart(1).PreBoundText)
                    lrVerbaliser.VerbaliseModelObject(lrFactType.RoleGroup(1).JoinedORMObject)

                    lrVerbaliser.HTW.WriteBreak()
                    lrVerbaliser.VerbaliseIndent()

                    If lrFactType.IsManyToOneByRoleOrder(lrFactTypeReading.RoleList) Then
                        lrVerbaliser.VerbaliseQuantifier("It is possible that more than one ")
                        lrVerbaliser.VerbaliseModelObject(lrFactType.RoleGroup(0).JoinedORMObject)
                        lrVerbaliser.VerbalisePredicateText(" " & lrFactTypeReading.PredicatePart(0).PredicatePartText)
                        lrVerbaliser.VerbaliseQuantifier(" the same ")
                        lrVerbaliser.VerbalisePredicateText(lrFactTypeReading.PredicatePart(1).PreBoundText)
                        lrVerbaliser.VerbaliseModelObject(lrFactType.RoleGroup(1).JoinedORMObject)
                    ElseIf lrFactType.Is1To1BinaryFactType Then
                        Dim larRole As New List(Of FBM.Role)
                        larRole.Add(lrFactType.RoleGroup(1))
                        larRole.Add(lrFactType.RoleGroup(0))
                        lrFactTypeReading = lrFactType.FindSuitableFactTypeReadingByRoles(larRole)
                        lrVerbaliser.VerbaliseQuantifier("Each ")
                        lrVerbaliser.VerbaliseModelObject(larRole(0).JoinedORMObject)
                        lrVerbaliser.VerbalisePredicateText(" " & lrFactTypeReading.PredicatePart(0).PredicatePartText)
                        lrVerbaliser.VerbaliseQuantifier(" at most one ")
                        lrVerbaliser.VerbalisePredicateText(lrFactTypeReading.PredicatePart(1).PreBoundText)
                        lrVerbaliser.VerbaliseModelObject(larRole(1).JoinedORMObject)
                    End If

                    If arAttribute.Column.Role.JoinedORMObject.ConceptType = pcenumConceptType.EntityType Then
                        Dim lrEntityType As FBM.EntityType = arAttribute.Column.Role.JoinedORMObject
                        Dim lrTopmostSupertype As FBM.EntityType = lrEntityType.GetTopmostSupertype

                        If lrTopmostSupertype.HasSimpleReferenceScheme Then

                            If lrTopmostSupertype.ReferenceModeRoleConstraint Is Nothing Then
                                lrVerbaliser.HTW.WriteBreak()
                                lrVerbaliser.VerbaliseError("Error: Entity Type, '" & lrEntityType.Id & ", has no Reference Mode Role Constraint.")
                            Else

                                If lrTopmostSupertype.ReferenceModeRoleConstraint.RoleConstraintRole(0).Role Is arAttribute.Column.ActiveRole Then

                                    lrVerbaliser.HTW.WriteBreak()
                                    lrVerbaliser.HTW.WriteBreak()
                                    lrVerbaliser.VerbaliseQuantifier("This association with ")
                                    lrVerbaliser.VerbaliseModelObject(lrEntityType.ReferenceModeValueType)
                                    lrVerbaliser.VerbaliseQuantifier(" provides the preferred reference scheme for ")
                                    lrVerbaliser.VerbaliseModelObject(lrEntityType)
                                End If
                            End If

                        End If
                    End If
                End If
            ElseIf lrFactType.Arity > 2 Then
                Select Case arAttribute.Column.Role.JoinedORMObject.ConceptType
                    Case Is = pcenumConceptType.EntityType
                        Dim lrEntityType As FBM.EntityType = arAttribute.Column.Role.JoinedORMObject
                        If lrEntityType.HasSimpleReferenceScheme Then
                            If lrEntityType.ReferenceModeValueType Is arAttribute.Column.ActiveRole.JoinedORMObject Then
                                lrVerbaliser.HTW.WriteBreak()
                                lrVerbaliser.VerbaliseHeading("ORM Level")
                                lrVerbaliser.HTW.WriteBreak()
                                lrVerbaliser.HTW.WriteBreak()
                                lrVerbaliser.VerbaliseQuantifier("This Attribute ultimately references the ORM Entity Type ")
                                lrVerbaliser.VerbaliseModelObject(lrEntityType)
                                lrVerbaliser.VerbaliseQuantifier(" with a Reference Mode Value Type with name ")
                                lrVerbaliser.VerbaliseModelObject(lrEntityType.ReferenceModeValueType)
                            End If
                        End If
                    Case Is = pcenumConceptType.FactType
                        Select Case arAttribute.Column.ActiveRole.JoinedORMObject.ConceptType
                            Case Is = pcenumConceptType.EntityType
                            Case Is = pcenumConceptType.ValueType
                                lrVerbaliser.HTW.WriteBreak()
                                lrVerbaliser.VerbaliseHeading("ORM Level")
                                lrVerbaliser.HTW.WriteBreak()
                                lrVerbaliser.HTW.WriteBreak()
                                lrVerbaliser.VerbaliseQuantifier("This Attribute ultimately references the ORM Value Type ")
                                lrVerbaliser.VerbaliseModelObject(arAttribute.Column.ActiveRole.JoinedORMObject)
                        End Select
                End Select
            Else
                'Is UnaryFactType Column/Attribute
                lrVerbaliser.HTW.WriteBreak()
                lrVerbaliser.VerbaliseHeading("ORM Level")
                lrVerbaliser.HTW.WriteBreak()

                lrFactTypeReading = arAttribute.Column.Role.FactType.FactTypeReading(0)
                lrVerbaliser.HTW.WriteBreak()

                lrFactTypeReading.GetReadingText(lrVerbaliser)
                'lrVerbaliser.VerbaliseModelObject(lrFactType.RoleGroup(0).JoinedORMObject)
                'lrVerbaliser.VerbalisePredicateText(" " & lrFactTypeReading.PredicatePart(0).PredicatePartText)


                lrVerbaliser.HTW.WriteBreak()

            End If

            lrVerbaliser.HTW.WriteBreak()
            lrVerbaliser.HTW.WriteBreak()
            lrVerbaliser.VerbaliseHeading("Value Constraints:")
            lrVerbaliser.HTW.WriteBreak()
            lrVerbaliser.HTW.WriteBreak()

            Dim liCounter As Integer = 0
            Dim lrResponsibleModelObject As FBM.ModelObject = Nothing
            If arAttribute.Column.getMetamodelValueContraintValues(lrResponsibleModelObject).Count > 0 Then

                If lrResponsibleModelObject Is Nothing Then
                    lrVerbaliser.VerbaliseError("Error: Column has no Active Role.")
                Else

                    lrVerbaliser.VerbaliseQuantifier("Possible Values for ")
                    lrVerbaliser.VerbaliseModelObject(lrResponsibleModelObject)
                    lrVerbaliser.VerbaliseQuantifier(" are {")
                    For Each lsString In arAttribute.Column.getMetamodelValueContraintValues(lrResponsibleModelObject)
                        liCounter += 1
                        If liCounter = 1 Then
                            lrVerbaliser.HTW.Write("'" & lsString & "'")
                        Else
                            lrVerbaliser.HTW.Write(", '" & lsString & "'")
                        End If
                    Next
                    lrVerbaliser.VerbaliseQuantifier("}")
                End If
            Else
                lrVerbaliser.VerbaliseQuantifier("There are no Value Constraints for this Attribute.")
            End If

            If My.Settings.SuperuserMode Then
                If arAttribute.ActiveRole.JoinedORMObject.GetType = GetType(FBM.EntityType) And Not arAttribute.ActiveRole.FactType.Arity = 1 Then
                    lrVerbaliser.HTW.WriteBreak()
                    lrVerbaliser.HTW.WriteBreak()
                    lrVerbaliser.VerbaliseError("Error: This Attribute is joined to an EntityType (" & arAttribute.ActiveRole.JoinedORMObject.Id & ") rather than the Entity Type's Reference Scheme's Value Type.")
                    lrVerbaliser.HTW.WriteBreak()
                    lrVerbaliser.VerbaliseError("Either set the Reference Scheme for the joined Entity Type or contact support for how to resolve this issue.")
                End If
            End If

            Me.WebBrowser.DocumentText = lrVerbaliser.Verbalise

        Catch ex As Exception
            Dim lsMessage1 As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage1 &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Sub

    Public Sub VerbaliseColumn(ByVal arColumn As RDS.Column)

        Dim lrVerbaliser As New FBM.ORMVerbailser
        Call lrVerbaliser.Reset()

        '------------------------------------------------------
        'Declare that the EntityType(Name) is an EntityType
        '------------------------------------------------------
        lrVerbaliser.VerbalisePredicateText(arColumn.Name)
        lrVerbaliser.VerbaliseQuantifier(" is an Attribute.")
        lrVerbaliser.HTW.WriteBreak()
        lrVerbaliser.HTW.WriteBreak()

        lrVerbaliser.VerbaliseQuantifier("Data Type: ")
        lrVerbaliser.VerbalisePredicateText(arColumn.getMetamodelDataType.ToString)
        Dim liDataTypeLength = arColumn.getMetamodelDataTypeLength
        Dim liDataTypePrecision = arColumn.getMetamodelDataTypePrecision
        Dim lbPrecisionDone = False
        If liDataTypeLength > 0 Then
            lrVerbaliser.VerbaliseBlackText("(" & liDataTypeLength.ToString)
            If liDataTypePrecision > 0 Then
                lrVerbaliser.VerbaliseBlackText("," & liDataTypePrecision.ToString)
            End If
            lrVerbaliser.VerbaliseBlackText(")")
        End If
        If liDataTypePrecision > 0 And Not lbPrecisionDone Then
            lrVerbaliser.VerbaliseBlackText("(" & liDataTypePrecision.ToString & ")")
        End If


        Me.WebBrowser.DocumentText = lrVerbaliser.Verbalise

    End Sub

End Class