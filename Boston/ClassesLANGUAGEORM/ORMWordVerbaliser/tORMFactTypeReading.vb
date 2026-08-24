Imports System.Reflection

Namespace FBM

    Partial Public Class FactTypeReading

        Public Sub Verbalise(ByRef arORMWordVerbaliser As FBM.ORMWordVerbailser,
                             Optional ByVal asIntitialThatOrSome As String = Nothing,
                             Optional ByVal asFollowingThatOrSome As pcenumFollowingThatOrSome = Nothing,
                             Optional ByVal abDropIntialModelObject As Boolean = False,
                             Optional ByVal aiStartingSubscriptInteger As Integer = Nothing,
                             Optional ByVal aarSubscriptArray As List(Of String) = Nothing,
                             Optional ByVal abSubscriptNegativeOrder As Boolean = False,
                             Optional ByRef aarListedFactType As List(Of FBM.FactType) = Nothing,
                             Optional ByRef abSkippedFactType As Boolean = False)

            Try

                Dim liInd As Integer = 0
                Dim liSubscriptInteger As Integer
                Dim lsPredicatePart As String = ""

                If abSubscriptNegativeOrder Then
                    liSubscriptInteger = Me.FactType.RoleGroup.Count
                End If

                For Each lrPredicatePart In Me.PredicatePart

                    If Me.Model.GetConceptTypeByNameFuzzy(lrPredicatePart.Role.JoinedORMObject.Id, lrPredicatePart.Role.JoinedORMObject.Id) = pcenumConceptType.FactType Then
                        If aarListedFactType IsNot Nothing Then
                            aarListedFactType.Add(Me.Model.GetModelObjectByName(lrPredicatePart.Role.JoinedORMObject.Id))
                        End If
                    End If

                    If (liInd = 0) And asIntitialThatOrSome IsNot Nothing Then
                        arORMWordVerbaliser.VerbaliseQuantifier(asIntitialThatOrSome)
                    ElseIf (liInd = 0) Then
                        arORMWordVerbaliser.VerbaliseQuantifier("some ")
                    Else
                        Select Case asFollowingThatOrSome
                            Case Is = pcenumFollowingThatOrSome.Some
                                arORMWordVerbaliser.VerbaliseQuantifier("some ")
                            Case Is = pcenumFollowingThatOrSome.That
                                arORMWordVerbaliser.VerbaliseQuantifier("that ")
                            Case Is = pcenumFollowingThatOrSome.Either
                                arORMWordVerbaliser.VerbaliseQuantifier("some ") '20141218-For now, might change depending on the Internal Uniqueness Constraints of the FactType
                            Case Is = pcenumFollowingThatOrSome.TheSame
                                arORMWordVerbaliser.VerbaliseQuantifier("the same ")
                        End Select
                    End If
                    If (liInd = 0) And abDropIntialModelObject Then
                        If abDropIntialModelObject Then
                            '--------------------------------------------------
                            'Don't add the initial ModelObject to the reading
                            '--------------------------------------------------
                            If aarListedFactType IsNot Nothing Then
                                If Me.Model.GetConceptTypeByNameFuzzy(lrPredicatePart.Role.JoinedORMObject.Id, lrPredicatePart.Role.JoinedORMObject.Id) = pcenumConceptType.FactType Then
                                    If aarListedFactType.Exists(AddressOf Me.Model.GetModelObjectByName(lrPredicatePart.Role.JoinedORMObject.Id).Equals) Then
                                        '--------------------------------------------------
                                        'Definitely Drop he Initial Model Object
                                        '-----------------------------------------
                                    Else
                                        arORMWordVerbaliser.VerbaliseQuantifier("(")
                                        arORMWordVerbaliser.VerbaliseModelObject(lrPredicatePart.Role.JoinedORMObject)
                                        arORMWordVerbaliser.VerbaliseQuantifier(")")
                                    End If
                                Else
                                    arORMWordVerbaliser.VerbaliseQuantifier("(")
                                    arORMWordVerbaliser.VerbaliseModelObject(lrPredicatePart.Role.JoinedORMObject)
                                    arORMWordVerbaliser.VerbaliseQuantifier(")")
                                End If
                            Else
                                arORMWordVerbaliser.VerbaliseQuantifier("(")
                                arORMWordVerbaliser.VerbaliseModelObject(lrPredicatePart.Role.JoinedORMObject)
                                arORMWordVerbaliser.VerbaliseQuantifier(")")
                            End If
                        Else
                            If aiStartingSubscriptInteger > 0 Then
                                arORMWordVerbaliser.VerbaliseModelObject(lrPredicatePart.Role.JoinedORMObject)
                                If aarSubscriptArray IsNot Nothing Then
                                    arORMWordVerbaliser.VerbaliseSubscript(" " & aarSubscriptArray(liInd))
                                Else
                                    arORMWordVerbaliser.VerbaliseSubscript(" " & liSubscriptInteger)
                                End If
                            Else
                                arORMWordVerbaliser.Write(" ")
                                arORMWordVerbaliser.VerbalisePredicateText(lrPredicatePart.PreBoundText)
                                arORMWordVerbaliser.VerbaliseModelObject(lrPredicatePart.Role.JoinedORMObject)
                                arORMWordVerbaliser.VerbalisePredicateText(lrPredicatePart.PostBoundText)
                                arORMWordVerbaliser.Write(" ")
                            End If
                        End If
                    Else
                        If aiStartingSubscriptInteger > 0 Then
                            arORMWordVerbaliser.VerbalisePredicateText(lrPredicatePart.PreBoundText)
                            arORMWordVerbaliser.VerbaliseModelObject(lrPredicatePart.Role.JoinedORMObject)
                            arORMWordVerbaliser.VerbalisePredicateText(lrPredicatePart.PostBoundText)
                            If aarSubscriptArray IsNot Nothing Then
                                arORMWordVerbaliser.VerbaliseSubscript(" " & aarSubscriptArray(liInd))
                            Else
                                arORMWordVerbaliser.VerbaliseSubscript(" " & liSubscriptInteger)
                            End If
                        Else
                            arORMWordVerbaliser.Write(" ")
                            arORMWordVerbaliser.VerbalisePredicateText(lrPredicatePart.PreBoundText)
                            arORMWordVerbaliser.VerbaliseModelObject(lrPredicatePart.Role.JoinedORMObject)
                            arORMWordVerbaliser.VerbalisePredicateText(lrPredicatePart.PostBoundText)
                            arORMWordVerbaliser.Write(" ")
                        End If
                    End If

                    If liInd < Me.PredicatePart.Count Then
                        lsPredicatePart = Trim(Me.PredicatePart(liInd).PredicatePartText)
                        lsPredicatePart = lsPredicatePart & " "
                        lsPredicatePart = lsPredicatePart.Replace(" a ", " ")
                        lsPredicatePart = lsPredicatePart.Replace(" an ", " ")

                        arORMWordVerbaliser.VerbalisePredicateText(lsPredicatePart)
                    End If
                    liInd += 1
                    If abSubscriptNegativeOrder Then
                        liSubscriptInteger -= 1
                    Else
                        liSubscriptInteger += 1
                    End If

                Next


            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
            End Try
        End Sub

    End Class

End Namespace
