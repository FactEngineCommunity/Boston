Imports System.Reflection

Namespace TableModelNote

    Module tableORMModelNote

        Public Sub AddModelNote(ByVal arModelNote As FBM.ModelNote)

            Try
                Dim lsSQLQuery As String = ""

                lsSQLQuery &= "INSERT INTO MetaModelModelNote"
                lsSQLQuery &= "  VALUES("
                lsSQLQuery &= "'" & Trim(arModelNote.Id) & "'"
                lsSQLQuery &= ",'" & Trim(Replace(arModelNote.Text, "'", "`")) & "'"
                If IsSomething(arModelNote.JoinedObjectType) Then
                    lsSQLQuery &= ",'" & Trim(arModelNote.JoinedObjectType.Id) & "'"
                Else
                    lsSQLQuery &= ",''"
                End If
                lsSQLQuery &= "," & arModelNote.IsMDAModelElement
                lsSQLQuery &= ",'" & arModelNote.Model.ModelId & "'"
                lsSQLQuery &= ")"

                pdbConnection.BeginTrans()
                pdbConnection.Execute(lsSQLQuery)
                pdbConnection.CommitTrans()

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Public Sub DeleteModelNote(ByVal arModelNote As FBM.ModelNote)

            Dim lsSQLQuery As String

            Try
                lsSQLQuery = "DELETE FROM MetaModelModelNote"
                lsSQLQuery &= " WHERE ModelNoteId = '" & Replace(arModelNote.Id, "'", "`") & "'"
                lsSQLQuery &= "   AND ModelId = '" & Replace(arModelNote.Model.ModelId, "'", "`") & "'"

                pdbConnection.BeginTrans()
                pdbConnection.Execute(lsSQLQuery)
                pdbConnection.CommitTrans()

            Catch ex As Exception
                pdbConnection.RollbackTrans()

                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
            End Try


        End Sub

        Public Function ExistsModelNote(ByVal arModelNote As FBM.ModelNote) As Boolean

            Try
                Dim lsSQLQuery As String = ""
                Dim lREcordset As New RecordsetProxy

                lREcordset.ActiveConnection = pdbConnection
                lREcordset.CursorType = pcOpenStatic

                lsSQLQuery = " SELECT COUNT(*)"
                lsSQLQuery &= "  FROM MetaModelModelNote"
                lsSQLQuery &= " WHERE ModelNoteId = '" & Trim(arModelNote.Id) & "'"
                lsSQLQuery &= " AND ModelId = '" & Trim(arModelNote.Model.ModelId) & "'"

                lREcordset.Open(lsSQLQuery)

                If lREcordset(0).Value = 0 Then
                    ExistsModelNote = False
                Else
                    ExistsModelNote = True
                End If

                lREcordset.Close()

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Function

        Function getModelNoteCountByModel(ByVal arModel As FBM.Model) As Integer

            Try
                Dim lsSQLQuery As String = ""
                Dim lREcordset As New RecordsetProxy

                lREcordset.ActiveConnection = pdbConnection
                lREcordset.CursorType = pcOpenStatic

                lsSQLQuery = " SELECT COUNT(*)"
                lsSQLQuery &= "  FROM MetaModelModelNote MN,"
                lsSQLQuery &= "       MetaModelModelDictionary MD"
                lsSQLQuery &= " WHERE MD.ModelId = '" & Trim(arModel.ModelId) & "'"
                lsSQLQuery &= "   AND MD.Symbol = MN.ModelNoteId"
                lsSQLQuery &= "   AND MD.ModelId = MN.ModelId"

                lREcordset.Open(lsSQLQuery)

                getModelNoteCountByModel = lREcordset(0).Value

                lREcordset.Close()

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Function

        Public Function getModelNoteDetailsByModel(ByRef arModelNote As FBM.ModelNote, ByVal abAddToModel As Boolean) As FBM.ModelNote

            Try
                Dim lsSQLQuery As String = ""
                Dim lRecordset As New RecordsetProxy

                Dim lsMessage As String

                lRecordset.ActiveConnection = pdbConnection
                lRecordset.CursorType = pcOpenStatic

                '---------------------------------------------
                'First get EntityTypes with no ParentEntityId
                '---------------------------------------------
                lsSQLQuery = " SELECT MN.*, MD.*"
                lsSQLQuery &= "  FROM MetaModelModelNote MN,"
                lsSQLQuery &= "       MetaModelModelDictionary MD"
                lsSQLQuery &= " WHERE MD.ModelId = '" & Trim(arModelNote.Model.ModelId) & "'"
                lsSQLQuery &= "   AND MD.Symbol = MN.ModelNoteId"
                lsSQLQuery &= "   AND MD.ModelId = MN.ModelId"
                lsSQLQuery &= "   AND MN.ModelNoteId = '" & arModelNote.Id & "'"

                lRecordset.Open(lsSQLQuery)

                If Not lRecordset.EOF Then
                    arModelNote.isDirty = False
                    arModelNote.ShortDescription = Trim(Viev.NullVal(lRecordset("ShortDescription").Value, ""))
                    arModelNote.LongDescription = Trim(Viev.NullVal(lRecordset("LongDescription").Value, ""))
                    arModelNote.ConceptType = pcenumConceptType.ModelNote
                    arModelNote.Text = Trim(Viev.NullVal(lRecordset("Note").Value, ""))
                    arModelNote.IsMDAModelElement = CBool(lRecordset("IsMDAModelElement").Value)
                    arModelNote.isDirty = False

                    If lRecordset("JoinedObjectTypeId").Value = "" Then
                        arModelNote.JoinedObjectType = Nothing
                    Else
                        arModelNote.JoinedObjectType = New FBM.ModelObject
                        arModelNote.JoinedObjectType.Id = Trim(Viev.NullVal(lRecordset("JoinedObjectTypeId").Value, ""))
                        If IsSomething(arModelNote.Model.EntityType.Find(AddressOf arModelNote.JoinedObjectType.Equals)) Then
                            arModelNote.JoinedObjectType = arModelNote.Model.EntityType.Find(AddressOf arModelNote.JoinedObjectType.Equals)
                        ElseIf IsSomething(arModelNote.Model.ValueType.Find(AddressOf arModelNote.JoinedObjectType.Equals)) Then
                            arModelNote.JoinedObjectType = arModelNote.Model.ValueType.Find(AddressOf arModelNote.JoinedObjectType.Equals)
                        ElseIf IsSomething(arModelNote.Model.FactType.Find(AddressOf arModelNote.JoinedObjectType.Equals)) Then
                            arModelNote.JoinedObjectType = arModelNote.Model.FactType.Find(AddressOf arModelNote.JoinedObjectType.Equals)
                        End If
                    End If

                    '------------------------------------------------
                    'Link to the Concept within the ModelDictionary
                    '------------------------------------------------
                    Dim lrDictionaryEntry As New FBM.DictionaryEntry(arModelNote.Model, arModelNote.Id, pcenumConceptType.ModelNote, arModelNote.ShortDescription, arModelNote.LongDescription)
                    lrDictionaryEntry = arModelNote.Model.AddModelDictionaryEntry(lrDictionaryEntry)

                    If lrDictionaryEntry Is Nothing Then
                        lsMessage = "Cannot find DictionaryEntry in the ModelDictionary for ModelNote:"
                        lsMessage &= vbCrLf & "Model.Id: " & arModelNote.Model.ModelId
                        lsMessage &= vbCrLf & "ModelNote.Id: " & arModelNote.Id
                        Throw New Exception(lsMessage)
                    End If

                    arModelNote.Concept = lrDictionaryEntry.Concept

                    If abAddToModel Then
                        Call arModelNote.Model.AddModelNote(arModelNote, True)
                    End If
                Else
                    Throw New Exception("No Model Note exists in the database for ModelNoteId: " & arModelNote.Id)
                End If

                lRecordset.Close()

                Return arModelNote

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)

                Return arModelNote
            End Try

        End Function

        Public Function getModelNotesByModel(ByRef arModel As FBM.Model) As List(Of FBM.ModelNote)

            Dim lrModelNote As FBM.ModelNote
            Dim lsSQLQuery As String = ""
            Dim lRecordset As New RecordsetProxy

            Dim lsMessage As String

            lRecordset.ActiveConnection = pdbConnection
            lRecordset.CursorType = pcOpenStatic

            '-----------------------------
            'Initialise the return value
            '-----------------------------
            getModelNotesByModel = New List(Of FBM.ModelNote)

            Try
                '---------------------------------------------
                'First get EntityTypes with no ParentEntityId
                '---------------------------------------------
                lsSQLQuery = " SELECT MN.*, MD.*"
                lsSQLQuery &= "  FROM MetaModelModelNote MN,"
                lsSQLQuery &= "       MetaModelModelDictionary MD"
                lsSQLQuery &= " WHERE MD.ModelId = '" & Trim(arModel.ModelId) & "'"
                lsSQLQuery &= "   AND MD.Symbol = MN.ModelNoteId"
                lsSQLQuery &= "   AND MD.ModelId = MN.ModelId"

                lRecordset.Open(lsSQLQuery)

                If Not lRecordset.EOF Then
                    While Not lRecordset.EOF
                        lrModelNote = New FBM.ModelNote
                        lrModelNote.isDirty = False
                        lrModelNote.Model = arModel
                        lrModelNote.Id = lRecordset("ModelNoteId").Value
                        lrModelNote.ShortDescription = Trim(Viev.NullVal(lRecordset("ShortDescription").Value, ""))
                        lrModelNote.LongDescription = Trim(Viev.NullVal(lRecordset("LongDescription").Value, ""))
                        lrModelNote.ConceptType = pcenumConceptType.ModelNote
                        lrModelNote.Text = Trim(Viev.NullVal(lRecordset("Note").Value, ""))
                        lrModelNote.IsMDAModelElement = CBool(lRecordset("IsMDAModelElement").Value)
                        lrModelNote.isDirty = False

                        If lRecordset("JoinedObjectTypeId").Value = "" Then
                            lrModelNote.JoinedObjectType = Nothing
                        Else
                            lrModelNote.JoinedObjectType = New FBM.ModelObject
                            lrModelNote.JoinedObjectType.Id = Trim(Viev.NullVal(lRecordset("JoinedObjectTypeId").Value, ""))
                            If IsSomething(arModel.EntityType.Find(AddressOf lrModelNote.JoinedObjectType.Equals)) Then
                                lrModelNote.JoinedObjectType = arModel.EntityType.Find(AddressOf lrModelNote.JoinedObjectType.Equals)
                            ElseIf IsSomething(arModel.ValueType.Find(AddressOf lrModelNote.JoinedObjectType.Equals)) Then
                                lrModelNote.JoinedObjectType = arModel.ValueType.Find(AddressOf lrModelNote.JoinedObjectType.Equals)
                            ElseIf IsSomething(arModel.FactType.Find(AddressOf lrModelNote.JoinedObjectType.Equals)) Then
                                lrModelNote.JoinedObjectType = arModel.FactType.Find(AddressOf lrModelNote.JoinedObjectType.Equals)
                            End If
                        End If

                        '------------------------------------------------
                        'Link to the Concept within the ModelDictionary
                        '------------------------------------------------
                        Dim lrDictionaryEntry As New FBM.DictionaryEntry(arModel, lrModelNote.Id, pcenumConceptType.ModelNote, lrModelNote.ShortDescription, lrModelNote.LongDescription)
                        lrDictionaryEntry = arModel.AddModelDictionaryEntry(lrDictionaryEntry)

                        If lrDictionaryEntry Is Nothing Then
                            lsMessage = "Cannot find DictionaryEntry in the ModelDictionary for ModelNote:"
                            lsMessage &= vbCrLf & "Model.Id: " & arModel.ModelId
                            lsMessage &= vbCrLf & "ModelNote.Id: " & lrModelNote.Id
                            Throw New Exception(lsMessage)
                        End If

                        lrModelNote.Concept = lrDictionaryEntry.Concept

                        getModelNotesByModel.Add(lrModelNote)
                        lRecordset.MoveNext()
                    End While
                End If

                lRecordset.Close()

            Catch ex As Exception                
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Function

        Public Sub UpdateModelNote(ByVal arModelNote As FBM.ModelNote)

            Try
                Dim lsSQLQuery As String = ""

                lsSQLQuery = "UPDATE MetaModelModelNote"
                lsSQLQuery &= " SET [Note] = '" & Trim(Replace(arModelNote.Text, "'", "`")) & "'"
                If IsSomething(arModelNote.JoinedObjectType) Then
                    lsSQLQuery &= " ,JoinedObjectTypeId = '" & arModelNote.JoinedObjectType.Id & "'"
                Else
                    lsSQLQuery &= " ,JoinedObjectTypeId = ''"
                End If
                lsSQLQuery &= "       ,IsMDAModelElement = " & arModelNote.IsMDAModelElement
                lsSQLQuery &= " WHERE ModelNoteId = '" & Trim(arModelNote.Id) & "'"
                lsSQLQuery &= "   AND ModelId = '" & Trim(arModelNote.Model.ModelId) & "'"

                pdbConnection.BeginTrans()
                pdbConnection.Execute(lsSQLQuery)
                pdbConnection.CommitTrans()

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

    End Module

End Namespace
