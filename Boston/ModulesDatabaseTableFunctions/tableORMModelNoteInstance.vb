Imports System.Reflection


Namespace TableModelNoteInstance

    Module tableORMModelNoteInstance

        Public Sub DeleteModelNoteInstance(ByVal arModelNoteInstance As FBM.ModelNoteInstance)

            Try
                Dim lsSQLQuery As String = ""

                lsSQLQuery = "DELETE FROM ModelConceptInstance"
                lsSQLQuery &= " WHERE PageId = '" & Trim(arModelNoteInstance.Page.PageId) & "'"
                lsSQLQuery &= "   AND Symbol = '" & Trim(arModelNoteInstance.Id) & "'"
                lsSQLQuery &= "   AND ConceptType = '" & pcenumConceptType.ModelNote.ToString & "'"

                pdbConnection.BeginTrans()
                Call pdbConnection.Execute(lsSQLQuery)
                pdbConnection.CommitTrans()

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Function getModelNoteInstanceCountByPage(ByRef arPage As FBM.Page) As Integer

            '-------------------------------------------------------------------------------------------------
            'NB EnterpriseInstances never manifest within the database, they manifest within the database as
            '  SymbolInstances, so we must count the SymbolInstances that match the arguments.
            '-------------------------------------------------------------------------------------------------
            Try
                Dim lsSQLQuery As String = ""
                Dim lREcordset As New ADODB.Recordset

                lREcordset.ActiveConnection = pdbConnection
                lREcordset.CursorType = pcOpenStatic

                lsSQLQuery = " SELECT COUNT(*)"
                lsSQLQuery &= "  FROM ModelConceptInstance ci,"
                lsSQLQuery &= "       MetaModelModelNote mn"
                lsSQLQuery &= " WHERE ci.ModelId = '" & Trim(arPage.Model.ModelId) & "'"
                lsSQLQuery &= "   AND ci.PageId = '" & Trim(arPage.PageId) & "'"
                lsSQLQuery &= "   AND ci.Symbol = mn.ModelNoteId"


                lREcordset.Open(lsSQLQuery)

                getModelNoteInstanceCountByPage = lREcordset(0).Value

                lREcordset.Close()

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Function

        Function getModelNoteInstancesByPage(ByRef arPage As FBM.Page) As List(Of FBM.ModelNoteInstance)

            '-----------------------------
            'Initialise the return value
            '-----------------------------
            getModelNoteInstancesByPage = New List(Of FBM.ModelNoteInstance)

            Try
                Dim lrModelNoteInstance As FBM.ModelNoteInstance
                Dim lsSQLQuery As String = ""
                Dim lREcordset As New ADODB.Recordset

                lREcordset.ActiveConnection = pdbConnection
                lREcordset.CursorType = pcOpenStatic

                lsSQLQuery = " SELECT mn.*, ci.*"
                lsSQLQuery &= "  FROM MetaModelModelNote mn,"
                lsSQLQuery &= "       ModelConceptInstance ci"
                lsSQLQuery &= " WHERE ci.PageId = '" & Trim(arPage.PageId) & "'"
                lsSQLQuery &= "   AND ci.Symbol = mn.ModelNoteId"
                lsSQLQuery &= "   AND ci.ConceptType = '" & pcenumConceptType.ModelNote.ToString & "'"

                lREcordset.Open(lsSQLQuery)


                While Not lREcordset.EOF
                    lrModelNoteInstance = New FBM.ModelNoteInstance
                    lrModelNoteInstance.Model = arPage.Model
                    lrModelNoteInstance.Page = arPage
                    lrModelNoteInstance.Id = lREcordset("ModelNoteId").Value
                    lrModelNoteInstance.NoteText = Trim(lREcordset("Note").Value)
                    lrModelNoteInstance.X = lREcordset("x").Value
                    lrModelNoteInstance.Y = lREcordset("y").Value

                    lrModelNoteInstance.ModelNote = arPage.Model.ModelNote.Find(Function(x) x.Id = lrModelNoteInstance.Id)

                    If lrModelNoteInstance.ModelNote Is Nothing Then GoTo SkipModelNote


                    If IsSomething(lrModelNoteInstance.ModelNote.JoinedObjectType) Then
                        lrModelNoteInstance.JoinedObjectType = New FBM.ModelObject
                        Select Case lrModelNoteInstance.ModelNote.JoinedObjectType.ConceptType
                            Case Is = pcenumConceptType.EntityType
                                lrModelNoteInstance.JoinedObjectType = arPage.EntityTypeInstance.Find(AddressOf lrModelNoteInstance.ModelNote.JoinedObjectType.Equals)
                            Case Is = pcenumConceptType.ValueType
                                lrModelNoteInstance.JoinedObjectType = arPage.ValueTypeInstance.Find(AddressOf lrModelNoteInstance.ModelNote.JoinedObjectType.Equals)
                            Case Is = pcenumConceptType.FactType
                                lrModelNoteInstance.JoinedObjectType = arPage.FactTypeInstance.Find(AddressOf lrModelNoteInstance.ModelNote.JoinedObjectType.Equals)
                        End Select
                    Else
                        lrModelNoteInstance.JoinedObjectType = Nothing
                    End If

                    getModelNoteInstancesByPage.Add(lrModelNoteInstance)
SkipModelNote:
                    lREcordset.MoveNext()
                End While

                lREcordset.Close()

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Function

    End Module

End Namespace
