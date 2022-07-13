Namespace TableEntityTypeInstance
    Module ztable_entity_type_instance

        Public Sub delete_entity_type_instance(ByVal arEntityTypeInstance As FBM.EntityTypeInstance)

            Dim lsSQLQuery As String = ""
            Dim lsSQLQuery2 As String = ""

            lsSQLQuery = "DELETE FROM ModelConceptInstance"
            lsSQLQuery &= " WHERE PageId = '" & Trim(arEntityTypeInstance.Page.PageId) & "'"
            lsSQLQuery &= "   AND Symbol = '" & Trim(arEntityTypeInstance.Id) & "'"
            lsSQLQuery &= "   AND ConceptType = '" & pcenumConceptType.EntityType.ToString & "'"
            lsSQLQuery &= "   AND InstanceNumber = " & arEntityTypeInstance.InstanceNumber

            lsSQLQuery2 = "UPDATE ModelConceptInstance"
            lsSQLQUery2 &= " SET InstanceNumber = InstanceNumber - 1"
            lsSQLQuery2 &= " WHERE PageId = '" & Trim(arEntityTypeInstance.Page.PageId) & "'"
            lsSQLQuery2 &= "    AND Symbol = '" & Trim(arEntityTypeInstance.Id) & "'"
            lsSQLQuery2 &= "    AND ConceptType = '" & pcenumConceptType.EntityType.ToString & "'"
            lsSQLQuery2 &= "    AND InstanceNumber > " & arEntityTypeInstance.InstanceNumber

            pdbConnection.BeginTrans()
            Call pdbConnection.Execute(lsSQLQuery)
            Call pdbConnection.Execute(lsSQLQuery2)
            pdbConnection.CommitTrans()

        End Sub

        Function getEntityTypeInstance_count_by_page(ByVal ar_page As FBM.Page) As Integer

            Dim lsSQLQuery As String = ""
            Dim lREcordset As New ADODB.Recordset

            lREcordset.ActiveConnection = pdbConnection
            lREcordset.CursorType = pcOpenStatic

            lsSQLQuery = " SELECT COUNT(*)"
            lsSQLQuery &= "  FROM ModelConceptInstance CI,"
            lsSQLQuery &= "       MetaModelEntityType ET"
            lsSQLQuery &= " WHERE ci.ModelId = '" & Trim(ar_page.Model.ModelId) & "'"
            lsSQLQuery &= "   AND ci.PageId = '" & Trim(ar_page.PageId) & "'"
            lsSQLQuery &= "   AND ci.Symbol = et.EntityTypeId"
            lsSQLQuery &= "   AND ci.ConceptType = '" & pcenumConceptType.EntityType.ToString & "'"



            lREcordset.Open(lsSQLQuery)

            getEntityTypeInstance_count_by_page = lREcordset(0).Value

            lREcordset.Close()

        End Function

        Function GetEntityTypeInstancesByPage(ByRef arPage As FBM.Page) As List(Of FBM.EntityTypeInstance)

            Dim lrEntityTypeInstance As FBM.EntityTypeInstance
            Dim lrEntityType As FBM.EntityType
            Dim lsSQLQuery As String = ""
            Dim lREcordset As New ADODB.Recordset
            Dim lsId As String

            Try
                lREcordset.ActiveConnection = pdbConnection
                lREcordset.CursorType = pcOpenStatic

                lsSQLQuery = " SELECT et.*, ci.*"
                lsSQLQuery &= "  FROM MetaModelEntityType et,"
                lsSQLQuery &= "       ModelConceptInstance ci"
                lsSQLQuery &= " WHERE ci.PageId = '" & Trim(arPage.PageId) & "'"
                lsSQLQuery &= "   AND ci.Symbol = et.EntityTypeId"
                lsSQLQuery &= "   AND ci.ModelId = et.ModelId"
                lsSQLQuery &= "   AND ci.ConceptType = '" & pcenumConceptType.EntityType.ToString & "'"

                lREcordset.Open(lsSQLQuery)

                '-----------------------------
                'Initialise the return value
                '-----------------------------

                GetEntityTypeInstancesByPage = New List(Of FBM.EntityTypeInstance)

                If Not lREcordset.EOF Then
                    While Not lREcordset.EOF
                        lrEntityTypeInstance = New FBM.EntityTypeInstance
                        lrEntityTypeInstance.Model = arPage.Model
                        lrEntityTypeInstance.Page = arPage
                        lrEntityTypeInstance.Id = lREcordset("EntityTypeId").Value

                        'CodeSafe: Proceed only if EntityType is in the Model. On rare occation Page may contain artifacts that are no longer in the Model.
                        lrEntityType = arPage.Model.EntityType.Find(Function(x) x.Id = lrEntityTypeInstance.Id)
                        If lrEntityType Is Nothing Then GoTo SkipEntityTypeInstance

                        lrEntityTypeInstance.EntityType = lrEntityType
                        lrEntityTypeInstance._Name = lREcordset("EntityTypeName").Value
                        lrEntityTypeInstance.ReferenceMode = Viev.NullVal(lREcordset("ReferenceMode").Value, "")
                        lrEntityTypeInstance.ShortDescription = lrEntityTypeInstance.EntityType.ShortDescription
                        lrEntityTypeInstance.LongDescription = lrEntityTypeInstance.EntityType.LongDescription
                        If Viev.NullVal(lREcordset("ValueTypeId").Value, "") = "" Then
                            lrEntityTypeInstance.ReferenceModeValueType = Nothing
                        Else
                            lsId = Viev.NullVal(lREcordset("ValueTypeId").Value, "")
                            lrEntityTypeInstance.ReferenceModeValueType = arPage.ValueTypeInstance.Find(Function(x) x.Id = lsId)
                        End If

                        lrEntityTypeInstance.PreferredIdentifierRCId = Viev.NullVal(lREcordset("PreferredIdentifierRCId").Value, "")
                        lrEntityTypeInstance.IsMDAModelElement = CBool(lREcordset("IsMDAModelElement").Value)
                        lrEntityTypeInstance.IsIndependent = CBool(lREcordset("IsIndependent").Value)
                        lrEntityTypeInstance.IsPersonal = CBool(lREcordset("IsPersonal").Value)
                        lrEntityTypeInstance.IsAbsorbed = CBool(lREcordset("IsAbsorbed").Value)
                        lrEntityTypeInstance.IsDerived = CBool(lREcordset("IsDerived").Value)
                        lrEntityTypeInstance.DerivationText = lrEntityTypeInstance.EntityType.DerivationText
                        lrEntityTypeInstance.DBName = lrEntityTypeInstance.EntityType.DBName
                        lrEntityTypeInstance.InstanceNumber = lREcordset("InstanceNumber").Value

                        lrEntityTypeInstance.X = lREcordset("x").Value
                        lrEntityTypeInstance.Y = lREcordset("y").Value

                        GetEntityTypeInstancesByPage.AddUnique(lrEntityTypeInstance)
SkipEntityTypeInstance:
                        lREcordset.MoveNext()
                    End While

                    lREcordset.Close()

                    '20200513-VM-Remove if all seems okay
                    ''------------------------------------------------------
                    ''Now Link the EntityTypes to their Parent EntityTypes
                    ''------------------------------------------------------
                    'lsSQLQuery = " SELECT *"
                    'lsSQLQuery &= "  FROM MetaModelParentEntityType"
                    'lsSQLQuery &= " WHERE ModelId = '" & Trim(arPage.Model.ModelId) & "'"

                    'lREcordset.Open(lsSQLQuery)

                    'Dim lr_parentEntityTypeInstance As FBM.EntityTypeInstance

                    'If Not lREcordset.EOF Then
                    '    While Not lREcordset.EOF
                    '        lrEntityTypeInstance = New FBM.EntityTypeInstance
                    '        lrEntityTypeInstance.Id = Trim(lREcordset("EntityTypeId").Value)
                    '        lrEntityTypeInstance = GetEntityTypeInstancesByPage.Find(AddressOf lrEntityTypeInstance.Equals)

                    '        If IsSomething(lrEntityTypeInstance) Then
                    '            '----------------------------------------------------------------------
                    '            'The ParentEntityType is at least part of the model under review
                    '            '  i.e. If currently looking at an ORM model...is within the ORM model
                    '            '----------------------------------------------------------------------
                    '            lr_parentEntityTypeInstance = New FBM.EntityTypeInstance
                    '            lr_parentEntityTypeInstance.Id = Trim(lREcordset("ParentEntityTypeId").Value)
                    '            lr_parentEntityTypeInstance = GetEntityTypeInstancesByPage.Find(AddressOf lr_parentEntityTypeInstance.Equals)

                    '            Dim lrSubtypeConstraint As New FBM.tSubtypeRelationship
                    '            lrSubtypeConstraint.EntityType = lrEntityTypeInstance.EntityType
                    '            lrSubtypeConstraint.parentEntityType = lr_parentEntityTypeInstance.EntityType

                    '            lrSubtypeConstraint = lrSubtypeConstraint.EntityType.SubtypeConstraint.Find(AddressOf lrSubtypeConstraint.Equals)

                    '            Dim lrSubtypeConstraintInstance As FBM.SubtypeRelationshipInstance

                    '            lrSubtypeConstraintInstance = lrSubtypeConstraint.CloneInstance(arPage, True)

                    '            lrEntityTypeInstance.SubtypeConstraint.Add(lrSubtypeConstraintInstance)
                    '        End If
                    '        lREcordset.MoveNext()
                    '    End While
                    'End If
                End If

            Catch ex As Exception
                Dim lsMessage As String
                lsMessage = "Error: TableEntityTypeInstance.GetEntityTypeInstancesByPage"
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)

                Return Nothing
            End Try

        End Function

    End Module
End Namespace