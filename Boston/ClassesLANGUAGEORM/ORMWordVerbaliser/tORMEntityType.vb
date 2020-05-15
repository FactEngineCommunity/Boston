Imports System.Reflection

Namespace FBM

    Partial Public Class EntityType

        Public Sub Verbalise(ByRef arORMWordVerbaliser As FBM.ORMWordVerbailser)

            Try
                '------------------------------------------------------
                'Declare that the EntityType(Name) is an EntityType
                '------------------------------------------------------
                arORMWordVerbaliser.VerbaliseModelObject(Me)
                arORMWordVerbaliser.VerbaliseQuantifier(" is an Entity Type.")
                arORMWordVerbaliser.WriteLine()
                If Me.IsDerived Then
                    arORMWordVerbaliser.VerbaliseQuantifier("*")
                    arORMWordVerbaliser.VerbaliseQuantifier(Me.DerivationText)
                    arORMWordVerbaliser.WriteLine()
                End If
                arORMWordVerbaliser.WriteLine()

                If Me.ShortDescription <> "" Or Me.LongDescription <> "" Then

                    arORMWordVerbaliser.VerbaliseHeading("Informally:")
                    arORMWordVerbaliser.WriteLine()
                    arORMWordVerbaliser.WriteLine()

                    If Me.ShortDescription <> "" Then
                        arORMWordVerbaliser.VerbaliseQuantifier("Short Description: ")
                        arORMWordVerbaliser.VerbaliseQuantifierLight(Me.ShortDescription)
                        arORMWordVerbaliser.WriteLine()
                        arORMWordVerbaliser.WriteLine()
                    End If

                    If Me.LongDescription <> "" Then
                        arORMWordVerbaliser.VerbaliseQuantifier("Long Description: ")
                        arORMWordVerbaliser.VerbaliseQuantifierLight(Me.LongDescription)
                        arORMWordVerbaliser.WriteLine()
                        arORMWordVerbaliser.WriteLine()
                    End If

                End If

                '------------------------------
                'Verbalise the ReferenceScheme
                '------------------------------                   
                Dim lrTopmostSupertype As FBM.EntityType
                Dim lrFactType As FBM.FactType
                Dim liInd As Integer = 0

                If Me.IsSubtype Then
                    lrTopmostSupertype = Me.GetTopmostSupertype
                Else 'Is Not Subtype
                    lrTopmostSupertype = Me
                End If

                If lrTopmostSupertype.HasSimpleReferenceScheme Then
                    arORMWordVerbaliser.VerbaliseQuantifier("Reference Scheme: ")
                    arORMWordVerbaliser.VerbaliseModelObject(lrTopmostSupertype)
                    arORMWordVerbaliser.VerbaliseQuantifier(" has ")
                    arORMWordVerbaliser.VerbaliseModelObject(lrTopmostSupertype.ReferenceModeValueType)

                    arORMWordVerbaliser.WriteLine()

                    '----------------------------
                    'Verbalise the ReferenceMode
                    '----------------------------
                    arORMWordVerbaliser.VerbaliseQuantifier("Reference Mode: ")
                    arORMWordVerbaliser.VerbaliseQuantifier(lrTopmostSupertype.ReferenceMode)

                ElseIf lrTopmostSupertype.HasCompoundReferenceMode Then
                    arORMWordVerbaliser.VerbaliseQuantifier("Reference Scheme: ")
                    For Each lrRoleConstraintRole In lrTopmostSupertype.ReferenceModeRoleConstraint.RoleConstraintRole
                        lrFactType = lrRoleConstraintRole.Role.FactType
                        Dim larRole As New List(Of FBM.Role)
                        larRole.Add(lrFactType.GetOtherRoleOfBinaryFactType(lrRoleConstraintRole.Role.Id))
                        larRole.Add(lrRoleConstraintRole.Role)
                        Dim lrFactTypeReading As FBM.FactTypeReading
                        lrFactTypeReading = lrFactType.FindSuitableFactTypeReadingByRoles(larRole, True)
                        If lrFactTypeReading IsNot Nothing Then
                            Call lrFactTypeReading.GetReadingText(arORMWordVerbaliser)
                        Else
                            arORMWordVerbaliser.VerbaliseError("Provide a Fact Type Reading for the Fact Type:")
                            arORMWordVerbaliser.VerbaliseModelObject(lrFactType)
                            arORMWordVerbaliser.VerbaliseError(" with  " & lrRoleConstraintRole.Role.JoinedORMObject.Id & " at the last position in the reading.")
                        End If
                        If liInd < lrTopmostSupertype.ReferenceModeRoleConstraint.RoleConstraintRole.Count - 1 Then
                            arORMWordVerbaliser.VerbaliseQuantifier(", ")
                        End If
                        liInd += 1
                    Next
                Else
                    arORMWordVerbaliser.VerbaliseError("Provide a Reference Mode for the Entity Type:")
                    arORMWordVerbaliser.VerbaliseModelObject(lrTopmostSupertype)
                End If

                arORMWordVerbaliser.WriteLine()
                arORMWordVerbaliser.WriteLine()

                '-------------------------------------------------
                'FOR EACH IncomingLink (from a Role)
                '  Verbalise the FactType for the associated Role
                'LOOP 
                '-------------------------------------------------

                arORMWordVerbaliser.WriteLine()
                arORMWordVerbaliser.VerbaliseQuantifier("Fact Types:")
                arORMWordVerbaliser.WriteLine()
                arORMWordVerbaliser.WriteLine()

                'LINQ
                Dim FactType = From ft In Me.Model.FactType, _
                                    rl In ft.RoleGroup _
                                    Where rl.JoinedORMObject.Id = Me.Id _
                                    Select ft Distinct

                For Each lrFactType In FactType
                    If lrFactType.FactTypeReading.Count > 0 Then
                        Call lrFactType.FactTypeReading(0).GetReadingText(arORMWordVerbaliser, True, True, True)
                    End If

                    arORMWordVerbaliser.WriteLine()
                Next

                arORMWordVerbaliser.WriteLine()
                arORMWordVerbaliser.VerbaliseQuantifier("Subtypes:")
                arORMWordVerbaliser.WriteLine()                

                If Me.childModelObjectList.Count = 0 Then
                    arORMWordVerbaliser.VerbaliseQuantifierLight("There are no Subtypes of this Entity Type.")
                Else
                    arORMWordVerbaliser.WriteLine()
                    For Each lrModelObject In Me.childModelObjectList
                        arORMWordVerbaliser.VerbaliseModelObject(lrModelObject)
                        arORMWordVerbaliser.WriteLine()
                    Next
                End If

                arORMWordVerbaliser.WriteLine()
                arORMWordVerbaliser.WriteLine()
                arORMWordVerbaliser.VerbaliseQuantifier("Supertypes:")
                arORMWordVerbaliser.WriteLine()                

                If Me.parentModelObjectList.Count = 0 Then
                    arORMWordVerbaliser.VerbaliseQuantifierLight("There are no Supertypes of this Entity Type.")
                Else
                    arORMWordVerbaliser.WriteLine()
                    For Each lrModelObject In Me.parentModelObjectList
                        arORMWordVerbaliser.VerbaliseModelObject(lrModelObject)
                        arORMWordVerbaliser.WriteLine()
                    Next
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
