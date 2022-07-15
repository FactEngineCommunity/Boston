Namespace TableRoleConstraintInstance

    Public Module TableRoleConstraintInstance

        Public Sub DeleteRoleConstraintInstance(ByVal arRoleConstraintInstance As FBM.RoleConstraintInstance)

            Dim lsSQLQuery As String = ""
            Dim lsSQLQuery2 As String = ""

            lsSQLQuery = "DELETE FROM ModelConceptInstance"
            lsSQLQuery &= " WHERE PageId = '" & Trim(arRoleConstraintInstance.Page.PageId) & "'"
            lsSQLQuery &= "   AND Symbol = '" & Trim(arRoleConstraintInstance.Id) & "'"
            lsSQLQuery &= "   AND ConceptType = '" & pcenumConceptType.RoleConstraint.ToString & "'"
            lsSQLQuery &= "   AND InstanceNumber = " & arRoleConstraintInstance.InstanceNumber

            lsSQLQuery2 = "UPDATE ModelConceptInstance"
            lsSQLQUery2 &= " SET InstanceNumber = InstanceNumber - 1"
            lsSQLQuery2 &= " WHERE PageId = '" & Trim(arRoleConstraintInstance.Page.PageId) & "'"
            lsSQLQuery2 &= "    AND Symbol = '" & Trim(arRoleConstraintInstance.Id) & "'"
            lsSQLQuery2 &= "    AND ConceptType = '" & pcenumConceptType.RoleConstraint.ToString & "'"
            lsSQLQuery2 &= "    AND InstanceNumber > " & arRoleConstraintInstance.InstanceNumber

            pdbConnection.BeginTrans()
            Call pdbConnection.Execute(lsSQLQuery)
            Call pdbConnection.Execute(lsSQLQuery2)
            pdbConnection.CommitTrans()

        End Sub

        Public Function getRoleConstraintInstanceCountForPage(ByVal ar_page As FBM.Page)

            Dim lsSQLQuery As String = ""
            Dim lREcordset As New ADODB.Recordset

            Try
                lREcordset.ActiveConnection = pdbConnection
                lREcordset.CursorType = pcOpenStatic

                lsSQLQuery = " SELECT COUNT(*)"
                lsSQLQuery &= "  FROM ModelConceptInstance ci,"
                lsSQLQuery &= "       MetaModelRoleConstraint rc"
                lsSQLQuery &= " WHERE ci.ModelId = '" & Trim(ar_page.Model.ModelId) & "'"
                lsSQLQuery &= "   AND ci.PageId = '" & Trim(ar_page.PageId) & "'"
                lsSQLQuery &= "   AND ci.Symbol = rc.RoleConstraintId"


                lREcordset.Open(lsSQLQuery)

                getRoleConstraintInstanceCountForPage = lREcordset(0).Value

                lREcordset.Close()

            Catch ex As Exception
                Dim lsMessage As String
                lsMessage = "Error: TableRoleConstraintInstance.GetRoleConstraintInstanceCountForPage"
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)

                Return 0
            End Try

        End Function

        Function GetRoleConstraintInstancesByPage(ByRef arPage As FBM.Page) As List(Of FBM.RoleConstraintInstance)

            Dim lrRoleConstraintInstance As FBM.RoleConstraintInstance
            Dim lsSQLQuery As String = ""
            Dim lREcordset As New ADODB.Recordset
            Dim lsMessage As String = ""

            lREcordset.ActiveConnection = pdbConnection
            lREcordset.CursorType = pcOpenStatic

            '-----------------------------
            'Initialise the return value
            '-----------------------------
            GetRoleConstraintInstancesByPage = New List(Of FBM.RoleConstraintInstance)

            '---------------------------------------------------------------------------------------------------------------------------------------------
            'Establish a dummy RoleConstraintInstance for Try,Catch error processing if Method fails before lrRoleConstraintInstance is set to any value
            '---------------------------------------------------------------------------------------------------------------------------------------------
            lrRoleConstraintInstance = New FBM.RoleConstraintInstance(New FBM.RoleConstraint(arPage.Model, "DummyAtBeginningOfMethod", True, pcenumRoleConstraintType.InternalUniquenessConstraint))

            Try
                lsSQLQuery = " SELECT rc.*, ci.*"
                lsSQLQuery &= "  FROM MetaModelRoleConstraint rc,"
                lsSQLQuery &= "       ModelConceptInstance ci"
                lsSQLQuery &= " WHERE ci.PageId = '" & Trim(arPage.PageId) & "'"
                lsSQLQuery &= "   AND ci.Symbol = rc.RoleConstraintId"
                lsSQLQuery &= "   AND rc.ModelId = '" & Trim(arPage.Model.ModelId) & "'"
                lsSQLQuery &= "   AND ci.ConceptType = '" & pcenumConceptType.RoleConstraint.ToString & "'"

                lREcordset.Open(lsSQLQuery)

                If Not lREcordset.EOF Then
                    While Not lREcordset.EOF
                        Select Case CType([Enum].Parse(GetType(pcenumRoleConstraintType), lREcordset("RoleConstraintType").Value), pcenumRoleConstraintType)
                            Case Is = pcenumRoleConstraintType.InternalUniquenessConstraint,
                                      pcenumRoleConstraintType.ExternalUniquenessConstraint
                                lrRoleConstraintInstance = New FBM.tUniquenessConstraint
                            Case Else
                                lrRoleConstraintInstance = New FBM.RoleConstraintInstance
                                'See also Cloning below.
                        End Select
                        lrRoleConstraintInstance.Id = lREcordset("RoleConstraintId").Value

                        If arPage.RoleConstraintInstance.Exists(AddressOf lrRoleConstraintInstance.Equals) Then
                            '--------------------------------------------------------------------------------
                            'The RoleConstraintInstance was likely added to the Page within a recursive loading
                            '  of FactTypeInstances where a FactType has a Role that references another FactType
                            '-----------------------------------------------------------------------------------
                            GetRoleConstraintInstancesByPage.Add(arPage.RoleConstraintInstance.Find(AddressOf lrRoleConstraintInstance.Equals))
                        Else
                            lrRoleConstraintInstance.Model = arPage.Model
                            lrRoleConstraintInstance.Page = arPage
                            lrRoleConstraintInstance.RoleConstraint = arPage.Model.RoleConstraint.Find(Function(x) x.Id = lrRoleConstraintInstance.Id)
                            lrRoleConstraintInstance.RoleConstraintType = CType([Enum].Parse(GetType(pcenumRoleConstraintType), lREcordset("RoleConstraintType").Value), pcenumRoleConstraintType)
                            lrRoleConstraintInstance.RingConstraintType = CType([Enum].Parse(GetType(pcenumRingConstraintType), lREcordset("RingConstraintType").Value), pcenumRingConstraintType)
                            lrRoleConstraintInstance.InstanceNumber = lREcordset("InstanceNumber").Value

                            Select Case lrRoleConstraintInstance.RoleConstraintType
                                Case Is = pcenumRoleConstraintType.FrequencyConstraint
                                    lrRoleConstraintInstance = lrRoleConstraintInstance.CloneFrequencyConstraintInstance(arPage)
                                Case Is = pcenumRoleConstraintType.RoleValueConstraint
                                    lrRoleConstraintInstance = lrRoleConstraintInstance.CloneRoleValueConstraintInstance(arPage)
                                Case Is = pcenumRoleConstraintType.RingConstraint
                                    lrRoleConstraintInstance = lrRoleConstraintInstance.CloneRingConstraintInstance(arPage)
                            End Select

                            If Not IsSomething(lrRoleConstraintInstance.RoleConstraint) Then
                                lsMessage = "No RoleConstraint found for RoleConstraint.Id = '" & lrRoleConstraintInstance.Id & "'"
                                Throw New Exception(lsMessage)
                            End If

                            lrRoleConstraintInstance.Name = lREcordset("RoleConstraintName").Value
                            lrRoleConstraintInstance.LevelNr = lREcordset("LevelNr").Value
                            lrRoleConstraintInstance.IsPreferredIdentifier = lREcordset("IsPreferredUniqueness").Value
                            lrRoleConstraintInstance.IsDeontic = lREcordset("IsDeontic").Value
                            lrRoleConstraintInstance.Cardinality = lREcordset("Cardinality").Value
                            lrRoleConstraintInstance.MaximumValue = Trim(Viev.NullVal(lREcordset("MaximumValue").Value, ""))
                            lrRoleConstraintInstance.MinimumValue = Trim(Viev.NullVal(lREcordset("MinimumValue").Value, ""))
                            lrRoleConstraintInstance.MaximumFrequencyCount = lREcordset("MaximumFrequencyCount").Value
                            lrRoleConstraintInstance.MinimumFrequencyCount = lREcordset("MinimumFrequencyCount").Value
                            Select Case lREcordset("CardinalityRangeType").Value
                                Case Is = pcenumCardinalityRangeType.LessThanOREqual.ToString
                                    lrRoleConstraintInstance.CardinalityRangeType = pcenumCardinalityRangeType.LessThanOREqual
                                Case Is = pcenumCardinalityRangeType.Equal.ToString
                                    lrRoleConstraintInstance.CardinalityRangeType = pcenumCardinalityRangeType.Equal
                                Case Is = pcenumCardinalityRangeType.GreaterThanOREqual.ToString
                                    lrRoleConstraintInstance.CardinalityRangeType = pcenumCardinalityRangeType.GreaterThanOREqual
                                Case Is = pcenumCardinalityRangeType.Between.ToString
                                    lrRoleConstraintInstance.CardinalityRangeType = pcenumCardinalityRangeType.Between
                            End Select
                            lrRoleConstraintInstance.LongDescription = lrRoleConstraintInstance.RoleConstraint.LongDescription
                            lrRoleConstraintInstance.ShortDescription = lrRoleConstraintInstance.RoleConstraint.ShortDescription
                            lrRoleConstraintInstance.ValueRangeType = CType([Enum].Parse(GetType(pcenumValueRangeType), lREcordset("ValueRangeType").Value), pcenumValueRangeType)
                            lrRoleConstraintInstance.X = lREcordset("x").Value
                            lrRoleConstraintInstance.Y = lREcordset("y").Value

                            Dim lrRole As FBM.Role
                            Dim lrRoleInstance As FBM.RoleInstance
                            For Each lrRole In lrRoleConstraintInstance.RoleConstraint.Role
                                lrRoleInstance = arPage.RoleInstance.Find(AddressOf lrRole.Equals)

                                If lrRoleInstance Is Nothing Then
                                    If arPage.FactTypeInstance.FindAll(Function(x) x.Id = lrRole.FactType.Id).Count = 0 Then
                                        '--------------------------------------------------------------------------------------------
                                        'The FactType is not on the Page, and should be. So add the FactType to the Page.
                                        Call arPage.DropFactTypeAtPoint(lrRole.FactType, New PointF(10, 10), False, False, True)
                                        lrRoleInstance = arPage.RoleInstance.Find(AddressOf lrRole.Equals)
                                    End If
                                End If

                                If IsSomething(lrRoleInstance) Then
                                    lrRoleInstance.RoleConstraint.Add(lrRoleConstraintInstance)
                                    lrRoleConstraintInstance.Role.Add(lrRoleInstance)
                                    If lrRoleConstraintInstance.RoleConstraint.RoleConstraintType = pcenumRoleConstraintType.InternalUniquenessConstraint Then
                                        If Not IsSomething(lrRoleInstance.FactType.InternalUniquenessConstraint.Find(AddressOf lrRoleConstraintInstance.RoleConstraint.Equals)) Then
                                            '----------------------------------------------------------------------------
                                            'Add the RoleConstraintInstance to the set of InternalUniquenessConstraints 
                                            '  for the FactTypeInstance
                                            '----------------------------------------------------------------------------
                                            'lrRoleInstance.FactType.InternalUniquenessConstraint.Add(lrRoleConstraintInstance)
                                            lrRoleInstance.FactType.AddInternalUniquenessConstraint(lrRoleConstraintInstance)
                                        End If
                                    End If
                                Else
                                    lrRole = arPage.Model.Role.Find(AddressOf lrRole.Equals)
                                    lsMessage = "No RoleInstance found for Role: '" & lrRole.Id & "'"
                                    Throw New Exception(lsMessage)
                                End If
                            Next

                            '--------------------------------------------
                            'Add the RoleConstraintInstance to the Page
                            '--------------------------------------------
                            arPage.RoleConstraintInstance.Add(lrRoleConstraintInstance)

                            Dim lrRoleConstraintRole As FBM.RoleConstraintRole
                            Dim lrRoleConstraintRoleInstance As FBM.RoleConstraintRoleInstance

                            '-----------------------------------------
                            'Create the RoleConstraintRoleInstance/s
                            '-----------------------------------------
                            For Each lrRoleConstraintRole In lrRoleConstraintInstance.RoleConstraint.RoleConstraintRole
                                lrRoleConstraintRoleInstance = lrRoleConstraintRole.CloneInstance(arPage)
                                lrRoleConstraintInstance.RoleConstraintRole.Add(lrRoleConstraintRoleInstance)

                                If lrRoleConstraintInstance.RoleConstraintType = pcenumRoleConstraintType.RoleValueConstraint Then
                                    lrRoleConstraintRoleInstance.Role.RoleConstraintRoleValueConstraint = lrRoleConstraintInstance
                                End If
                            Next

                            GetRoleConstraintInstancesByPage.Add(lrRoleConstraintInstance)
                        End If
                        lREcordset.MoveNext()
                    End While
                End If

                lREcordset.Close()

            Catch ex As Exception
                lsMessage = "Error: TableRoleConstraintInstance.getRolesConstraintInstanceByPage: "
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                lsMessage &= vbCrLf & vbCrLf & "Page.Id: '" & arPage.PageId & "'"
                lsMessage &= vbCrLf & "Page.Name: '" & arPage.Name & "'"
                lsMessage &= vbCrLf & "RoleConstraint.Id: '" & lrRoleConstraintInstance.Id & "'"
                Call prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try
        End Function

    End Module

End Namespace