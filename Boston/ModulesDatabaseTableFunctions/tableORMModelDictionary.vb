Namespace TableModelDictionary

    Public Module TableModelDictionary

        Public Sub AddModelDictionaryEntry(ByVal ar_dictionary_entry As FBM.DictionaryEntry)

            Dim lsSQLQuery As String = ""

            Try
                lsSQLQuery = "INSERT INTO MetaModelModelDictionary"
                lsSQLQuery &= " VALUES ("
                lsSQLQuery &= " #" & Now & "#"
                lsSQLQuery &= " ,#" & Now & "#"
                lsSQLQuery &= ",'" & Trim(ar_dictionary_entry.Model.ModelId) & "'"
                lsSQLQuery &= ",'" & Trim(Replace(ar_dictionary_entry.Symbol, "'", "`")) & "'"
                lsSQLQuery &= ",'" & Trim(Replace(ar_dictionary_entry.ShortDescription, "'", "`")) & "'"
                lsSQLQuery &= ",'" & Trim(Replace(ar_dictionary_entry.LongDescription, "'", "`")) & "'"
                lsSQLQuery &= "," & ar_dictionary_entry.isEntityType
                lsSQLQuery &= "," & ar_dictionary_entry.isValueType
                lsSQLQuery &= "," & ar_dictionary_entry.isFactType
                lsSQLQuery &= "," & ar_dictionary_entry.isFact
                lsSQLQuery &= "," & ar_dictionary_entry.isValue
                lsSQLQuery &= "," & ar_dictionary_entry.isRoleConstraint
                lsSQLQuery &= "," & ar_dictionary_entry.isModelNote
                lsSQLQuery &= "," & ar_dictionary_entry.isGeneralConcept
                lsSQLQuery &= ")"

                pdbConnection.Execute(lsSQLQuery)

            Catch ex As Exception
                Dim lsMessage As String
                lsMessage = "Error: TableModelDictionary.AddModelDictionaryEntry"
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Public Sub DeleteModelDictionaryEntry(ByVal arModelDictionaryEntry As FBM.DictionaryEntry)

            Dim lsSQLQuery As String = ""

            Try
                lsSQLQuery = "DELETE FROM MetaModelModelDictionary"
                lsSQLQuery &= " WHERE ModelId = '" & arModelDictionaryEntry.Model.ModelId & "'"
                lsSQLQuery &= "   AND Symbol = '" & Replace(arModelDictionaryEntry.Symbol, "'", "`") & "'"

                ''-----------------------------------------------------------------------------------------
                ''CodeSafe: Only remove the DictionaryEntry if everything matches for the DictionaryEntry
                ''-----------------------------------------------------------------------------------------
                'lsSQLQuery &= "   AND IsEntityType = " & arModelDictionaryEntry.isEntityType
                'lsSQLQuery &= "   AND IsValueType = " & arModelDictionaryEntry.isValueType
                'lsSQLQuery &= "   AND IsFactType = " & arModelDictionaryEntry.isFactType
                'lsSQLQuery &= "   AND IsRoleConstraint = " & arModelDictionaryEntry.isRoleConstraint
                'lsSQLQuery &= "   AND IsModelNote = " & arModelDictionaryEntry.isModelNote
                'lsSQLQuery &= "   AND IsFact = " & arModelDictionaryEntry.isFact
                'lsSQLQuery &= "   AND IsValue = " & arModelDictionaryEntry.isValue

                pdbConnection.BeginTrans()
                pdbConnection.Execute(lsSQLQuery)
                pdbConnection.CommitTrans()

            Catch ex As Exception
                Dim lsMessage As String
                lsMessage = "Error: TableModelDictionary.DeleteModelDictionaryEntry"
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Public Sub DeleteModelDictionaryEntriesByModel(ByVal arModel As FBM.Model)

            Dim lsSQLQuery As String = ""

            Try
                lsSQLQuery = "DELETE FROM MetaModelModelDictionary"
                lsSQLQuery &= " WHERE ModelId = '" & arModel.ModelId & "'"

                pdbConnection.BeginTrans()
                pdbConnection.Execute(lsSQLQuery)
                pdbConnection.CommitTrans()

            Catch ex As Exception
                Dim lsMessage As String
                lsMessage = "Error: TableModelDictionary.DeleteModelDictionaryEntry"
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Function DoesModelObjectExistInAntotherModel(ByVal arModelObject As FBM.ModelObject) As Boolean


            Dim lsSQLQuery As String = ""
            Dim lREcordset As New ADODB.Recordset

            Try
                lREcordset.ActiveConnection = pdbConnection
                lREcordset.CursorType = pcOpenStatic

                lsSQLQuery = "SELECT COUNT(*)"
                lsSQLQuery &= " FROM MetaModelModelDictionary"
                lsSQLQuery &= " WHERE ModelId <> '" & arModelObject.Model.ModelId & "'"
                lsSQLQuery &= "   AND Symbol = '" & Replace(arModelObject.Id, "'", "`") & "'"

                lREcordset.Open(lsSQLQuery)

                If lREcordset(0).Value > 0 Then
                    DoesModelObjectExistInAntotherModel = True
                Else
                    DoesModelObjectExistInAntotherModel = False
                End If

                lREcordset.Close()
            Catch ex As Exception

                Dim lsMessage As String
                lsMessage = "Error: TableModelDictionary.DoesModelObjectExistInAntotherModel"
                lsMessage &= vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)

            End Try

        End Function

        Function ExistsModelDictionaryEntry(ByVal ar_model_concept As FBM.DictionaryEntry) As Boolean


            Dim lsSQLQuery As String = ""
            Dim lREcordset As New ADODB.Recordset

            Try

                lREcordset.ActiveConnection = pdbConnection
                lREcordset.CursorType = pcOpenStatic

                lsSQLQuery = "SELECT COUNT(*)"
                lsSQLQuery &= " FROM MetaModelModelDictionary"
                lsSQLQuery &= " WHERE ModelId = '" & ar_model_concept.Model.ModelId & "'"
                lsSQLQuery &= "   AND Symbol = '" & Replace(Trim(ar_model_concept.Symbol), "'", "`") & "'"

                lREcordset.Open(lsSQLQuery)

                If lREcordset(0).Value > 0 Then
                    ExistsModelDictionaryEntry = True
                Else
                    ExistsModelDictionaryEntry = False
                End If

                lREcordset.Close()

            Catch ex As Exception
                Dim lsMessage As String
                lsMessage = "Error: TableModelDictionary.ExistsModelDictionaryEntry"
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Function

        Function get_model_concept_count_by_type(Optional ByVal aiConceptType As pcenumConceptType = Nothing) As Integer

            Dim lsSQLQuery As String = ""
            Dim lREcordset As New ADODB.Recordset

            lREcordset.ActiveConnection = pdbConnection
            lREcordset.CursorType = pcOpenStatic

            lsSQLQuery = "SELECT COUNT(*)"
            lsSQLQuery &= " FROM MetaModelModelDictionary"
            If Not IsNothing(aiConceptType) Then
                Select Case aiConceptType
                    Case pcenumConceptType.EntityType
                        lsSQLQuery &= " WHERE IsEntity = " & True
                    Case pcenumConceptType.ValueType
                        lsSQLQuery &= " WHERE IsValueType = " & True
                End Select
            End If

            lREcordset.Open(lsSQLQuery)

            get_model_concept_count_by_type = lREcordset(0).Value

        End Function

        Public Function GetDictionaryEntriesByModel(ByVal ar_model As FBM.Model) As List(Of FBM.DictionaryEntry)

            Dim lrDictionaryEntry As FBM.DictionaryEntry
            Dim lsSQLQuery As String = ""
            Dim lREcordset As New ADODB.Recordset

            lREcordset.ActiveConnection = pdbConnection
            lREcordset.CursorType = pcOpenStatic

            '-----------------------------
            'Initialise the return value
            '-----------------------------
            ar_model.ModelDictionary = New List(Of FBM.DictionaryEntry)

            Try
                '---------------------------------------------
                'First get EntityTypes with no ParentEntityId
                '---------------------------------------------
                lsSQLQuery = " SELECT *"
                lsSQLQuery &= "  FROM MetaModelModelDictionary MD"
                lsSQLQuery &= " WHERE MD.ModelId = '" & Trim(ar_model.ModelId) & "'"

                lREcordset.Open(lsSQLQuery)

                If Not lREcordset.EOF Then
                    While Not lREcordset.EOF

                        'If lREcordset("IsEntityType").Value = True Then
                        '    liConceptType = pcenumConceptType.EntityType
                        'ElseIf lREcordset("IsValueType").Value = True Then
                        '    liConceptType = pcenumConceptType.ValueType
                        'ElseIf lREcordset("IsFactType").Value = True Then
                        '    liConceptType = pcenumConceptType.FactType
                        'ElseIf lREcordset("IsFact").Value = True Then
                        '    liConceptType = pcenumConceptType.Fact
                        'ElseIf lREcordset("IsValue").Value = True Then
                        '    liConceptType = pcenumConceptType.Value
                        'ElseIf lREcordset("IsRoleConstraint").Value = True Then
                        '    liConceptType = pcenumConceptType.RoleConstraint
                        'ElseIf lREcordset("IsModelNote").Value = True Then
                        '    liConceptType = pcenumConceptType.ModelNote
                        'ElseIf lREcordset("IsGeneralConcept").Value = True Then
                        '    liConceptType = pcenumConceptType.GeneralConcept
                        'End If
                        'CType([Enum].Parse(GetType(pcenumRoleConstraintType), lREcordset("RoleConstraintType").Value), pcenumRoleConstraintType)

                        lrDictionaryEntry = New FBM.DictionaryEntry(ar_model, Trim(lREcordset("Symbol").Value), pcenumConceptType.GeneralConcept, , , False)

                        lrDictionaryEntry.isEntityType = lREcordset("IsEntityType").Value
                        lrDictionaryEntry.isValueType = lREcordset("IsValueType").Value
                        lrDictionaryEntry.isFactType = lREcordset("IsFactType").Value
                        lrDictionaryEntry.isFact = lREcordset("IsFact").Value
                        lrDictionaryEntry.isValue = lREcordset("IsValue").Value
                        lrDictionaryEntry.isRoleConstraint = lREcordset("IsRoleConstraint").Value
                        lrDictionaryEntry.isModelNote = lREcordset("IsModelNote").Value
                        lrDictionaryEntry.isFact = lREcordset("IsFact").Value
                        lrDictionaryEntry.isGeneralConcept = lREcordset("IsGeneralConcept").Value

                        lrDictionaryEntry.ShortDescription = Trim(Viev.NullVal(lREcordset("ShortDescription").Value, ""))
                        lrDictionaryEntry.LongDescription = Trim(Viev.NullVal(lREcordset("LongDescription").Value, ""))

                        '--------------------------------------------------------------------------
                        'Create a KL Identity Letter for the Concept in the dictionary, in memory
                        '  such that KL Theorems can be written.
                        '--------------------------------------------------------------------------
                        If Not {pcenumConceptType.Value, pcenumConceptType.Fact}.Contains(lrDictionaryEntry.ConceptType) Then
                            'Call lrDictionaryEntry.GenerateKLIdentityLetter(GetDictionaryEntriesByModel, 1)
                        End If

                        ar_model.ModelDictionary.Add(lrDictionaryEntry)
                        ar_model.Dictionary.Add(lrDictionaryEntry.Symbol, ar_model.ModelDictionary.Count - 1)
                        lREcordset.MoveNext()
                    End While
                End If

                lREcordset.Close()

            Catch ex As Exception
                MsgBox("Error: GetConceptsByModel: " & ex.Message)
            End Try

        End Function

        Public Function GetModelDictionaryCountByModel(ByVal ar_model As FBM.Model) As Integer

            Dim lsSQLQuery As String = ""
            Dim lREcordset As New ADODB.Recordset

            lREcordset.ActiveConnection = pdbConnection
            lREcordset.CursorType = pcOpenStatic

            lsSQLQuery = " SELECT COUNT(*)"
            lsSQLQuery &= "  FROM MetaModelModelDictionary"
            lsSQLQuery &= " WHERE ModelId = '" & Trim(ar_model.ModelId) & "'"

            lREcordset.Open(lsSQLQuery)

            GetModelDictionaryCountByModel = lREcordset(0).Value

            lREcordset.Close()

        End Function

        Function get_PrimativeType_by_ValueTypeId(ByVal aiEntity_id As Integer) As String

            get_PrimativeType_by_ValueTypeId = ""

        End Function

        ''' <summary>
        ''' Changes the actual 'Symbol' (i.e. Concept.Symbol) referenced by the ModelDictionaryEntry.
        ''' See Also: UpdateModelDictionaryEntry function, which does not modify the Symbol.
        ''' </summary>
        ''' <param name="arModel">The Model that owns the Model Dictionary</param>
        ''' <param name="arDictionaryEntry">The Dictionary Entry to be update.</param>
        ''' <param name="asNewSymbol">The new Symbol for the Dictionary Entry.</param>
        ''' <param name="aiConceptType">The Concept Type of the Dictionary Entry</param>
        ''' <remarks></remarks>
        Public Sub ModifySymbol(ByVal arModel As FBM.Model, ByVal arDictionaryEntry As FBM.DictionaryEntry, ByVal asNewSymbol As String, ByVal aiConceptType As pcenumConceptType)

            Dim lsSQLQuery As String = ""

            Try
                lsSQLQuery = " UPDATE MetaModelModelDictionary"
                lsSQLQuery &= "   SET Symbol = '" & Trim(Replace(asNewSymbol, "'", "`")) & "'"
                lsSQLQuery &= " WHERE ModelId = '" & Trim(arModel.ModelId) & "'"
                lsSQLQuery &= "   AND Symbol = '" & Trim(Replace(arDictionaryEntry.Symbol, "'", "`")) & "'"
                Select Case aiConceptType
                    Case pcenumConceptType.EntityType
                        lsSQLQuery &= "   AND IsEntityType = " & True
                    Case pcenumConceptType.ValueType
                        lsSQLQuery &= "   AND IsValueType = " & True
                    Case pcenumConceptType.FactType
                        lsSQLQuery &= "   AND IsFactType = " & True
                    Case pcenumConceptType.Fact
                        lsSQLQuery &= "   AND IsFact = " & True
                    Case pcenumConceptType.Value
                        lsSQLQuery &= "   AND IsValue = " & True
                    Case pcenumConceptType.RoleConstraint
                        lsSQLQuery &= "   AND IsRoleConstraint = " & True
                    Case pcenumConceptType.ModelNote
                        lsSQLQuery &= "   AND IsModelNote = " & True
                End Select

                pdbConnection.BeginTrans()
                pdbConnection.Execute(lsSQLQuery)

                ''===============================================================================================
                ''ConceptInstance
                ''  NB ConceptInstances are not fully referentially tied to the ModelDictionary table,
                ''  because things like RoleNames are not stored in the ModelDictionary.
                ''  So have to literally do an update on the ConceptInstance table as well.
                'lsSQLQuery = " UPDATE ModelConceptInstance MCI"
                'lsSQLQuery &= "   SET Symbol = '" & Trim(Replace(asNewSymbol, "'", "`")) & "'"
                'lsSQLQuery &= " WHERE ModelId = '" & Trim(arModel.ModelId) & "'"
                'lsSQLQuery &= "   AND Symbol = '" & Trim(Replace(arDictionaryEntry.Symbol, "'", "`")) & "'"
                'lsSQLQuery &= "   AND ConceptType = '" & pcenumConceptType.Value.ToString & "'"
                'lsSQLQuery &= "   AND RoleId IN (SELECT RoleId "
                'lsSQLQuery &= "                    FROM MetaModelRole MMR,"
                'lsSQLQuery &= "                         MetaModelFactType MMFT"
                'lsSQLQuery &= "                   WHERE MMR.ModelId = MCIModelId"
                'lsSQLQuery &= "                     AND MMR.FactTypeId = MMFT.FactTypeId"

                pdbConnection.Execute(lsSQLQuery)
                pdbConnection.CommitTrans()


            Catch ex As Exception

                Dim lsMessage As String
                lsMessage = "Error: TableModelDictionary"
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                lsMessage &= vbCrLf & vbCrLf
                lsMessage &= "Original Symbol: '" & arDictionaryEntry.Symbol & "'"
                lsMessage &= vbCrLf & "New Symbol: '" & asNewSymbol & "'"
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)

                pdbConnection.RollbackTrans()
            End Try

        End Sub

        Public Sub UpdateModelDictionaryEntry(ByVal arModelDictionaryEntry As FBM.DictionaryEntry)

            Dim lsSQLQuery As String = ""

            Try
                lsSQLQuery = " UPDATE MetaModelModelDictionary"
                lsSQLQuery &= "   SET ShortDescription = '" & Trim(arModelDictionaryEntry.ShortDescription.Replace("'", "''")) & "'"
                lsSQLQuery &= "       ,LongDescription = '" & Trim(arModelDictionaryEntry.LongDescription.Replace("'", "''")) & "'"
                lsSQLQuery &= "   ,IsEntityType = " & arModelDictionaryEntry.isEntityType                
                lsSQLQuery &= "   ,IsValueType = " & arModelDictionaryEntry.isValueType
                lsSQLQuery &= "   ,IsFactType = " & arModelDictionaryEntry.isFactType
                lsSQLQuery &= "   ,IsFact = " & arModelDictionaryEntry.isFact
                lsSQLQuery &= "   ,IsValue = " & arModelDictionaryEntry.isValue                    
                lsSQLQuery &= "   ,IsRoleConstraint = " & arModelDictionaryEntry.isRoleConstraint
                lsSQLQuery &= "   ,IsModelNote = " & arModelDictionaryEntry.isModelNote
                lsSQLQuery &= "   ,IsGeneralConcept = " & arModelDictionaryEntry.isGeneralConcept
                lsSQLQuery &= "       ,StartDate = Now"
                lsSQLQuery &= "       ,EndDate = #31/12/9999#"
                lsSQLQuery &= " WHERE ModelId = '" & Trim(arModelDictionaryEntry.Model.ModelId) & "'"
                lsSQLQuery &= "   AND Symbol = '" & Trim(Replace(arModelDictionaryEntry.Symbol, "'", "`")) & "'"

                pdbConnection.BeginTrans()
                pdbConnection.Execute(lsSQLQuery)
                pdbConnection.CommitTrans()

            Catch ex As Exception
                Dim lsMessage As String
                lsMessage = "Error: TableModelDictionary.UpdateModelDictionaryEntry"
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

    End Module

End Namespace