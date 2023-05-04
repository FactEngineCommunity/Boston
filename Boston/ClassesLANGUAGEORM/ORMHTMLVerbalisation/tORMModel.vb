Namespace FBM
    Partial Public Class Model

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
        Public Sub VerbaliseFactTypePart(ByRef arVerbaliser As FBM.ORMVerbailser,
                                          ByVal arFactType As FBM.FactType,
                                          ByVal arModelObjectColour As Color,
                                          ByVal arPredicatePartColour As Color,
                                          Optional ByVal asIntitialThatOrSome As String = Nothing,
                                          Optional ByVal asFollowingThatOrSome As pcenumFollowingThatOrSome = Nothing,
                                          Optional ByVal abDropIntialModelObject As Boolean = Nothing,
                                          Optional ByVal aiStartingSubscriptInteger As Integer = Nothing,
                                          Optional ByVal aarSubscriptArray As List(Of String) = Nothing,
                                          Optional ByVal abSubscriptNegativeOrder As Boolean = False,
                                          Optional ByRef aarListedFactType As List(Of FBM.FactType) = Nothing,
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

                        If Me.GetConceptTypeByNameFuzzy(lrPredicatePart.Role.JoinedORMObject.Id, lrPredicatePart.Role.JoinedORMObject.Id) = pcenumConceptType.FactType Then
                            If IsSomething(aarListedFactType) Then
                                aarListedFactType.Add(Me.GetModelObjectByName(lrPredicatePart.Role.JoinedORMObject.Id))
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
                                    If Me.GetConceptTypeByNameFuzzy(lrPredicatePart.Role.JoinedORMObject.Id, lrPredicatePart.Role.JoinedORMObject.Id) = pcenumConceptType.FactType Then
                                        If aarListedFactType.Exists(AddressOf Me.GetModelObjectByName(lrPredicatePart.Role.JoinedORMObject.Id).Equals) Then
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

    End Class

End Namespace