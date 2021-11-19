Imports System.Reflection

Namespace TablePage

    Public Module TablePage

        Public Sub AddPage(ByVal ar_page As FBM.Page)

            Try
                Dim lsSQLQuery As String = ""

                lsSQLQuery = "INSERT INTO ModelPage"
                lsSQLQuery &= " VALUES ("
                lsSQLQuery &= " '" & Trim(ar_page.PageId) & "'"
                lsSQLQuery &= " ,'" & Trim(ar_page.Name) & "'"
                lsSQLQuery &= " ,'" & Trim(ar_page.Model.ModelId) & "'"
                lsSQLQuery &= " ,'" & Trim(ar_page.Language) & "'"
                lsSQLQuery &= " ," & ar_page.IsCoreModelPage
                lsSQLQuery &= " ," & ar_page.ShowFacts
                lsSQLQuery &= ")"

                pdbConnection.BeginTrans()
                Call pdbConnection.Execute(lsSQLQuery)
                pdbConnection.CommitTrans()

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Public Sub DeletePage(ByVal ar_page As FBM.Page)

            Try
                Dim lsSQLQuery As String = ""

                lsSQLQuery = "DELETE FROM ModelPage"
                lsSQLQuery &= " WHERE PageId = '" & Trim(ar_page.PageId) & "'"

                Call pdbConnection.Execute(lsSQLQuery)

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Public Sub DeletePagesByModel(ByVal arModel As FBM.Model)

            Dim lsSQLQuery As String = ""

            lsSQLQuery = "DELETE FROM ModelPage"
            lsSQLQuery &= " WHERE ModelId = '" & Trim(arModel.ModelId) & "'"

            Call pdbConnection.Execute(lsSQLQuery)

        End Sub


        Public Function ExistsPageById(ByVal asPageId As String) As Boolean

            Dim lsSQLQuery As String = ""
            Dim lREcordset As New ADODB.Recordset

            '------------------------
            'Initialise return value
            '------------------------
            ExistsPageById = False

            lREcordset.ActiveConnection = pdbConnection
            lREcordset.CursorType = pcOpenStatic

            lsSQLQuery = "SELECT COUNT(*)"
            lsSQLQuery &= "  FROM ModelPage"
            lsSQLQuery &= " WHERE PageId = '" & Trim(asPageId) & "'"

            lREcordset.Open(lsSQLQuery)

            If lREcordset(0).Value > 0 Then
                ExistsPageById = True
            Else
                ExistsPageById = False
            End If

            lREcordset.Close()

        End Function

        Public Function GetPageCountByModel(ByVal as_ModelId As String, Optional ByVal abIncludeCorePages As Boolean = False)

            Dim lsSQLQuery As String = ""
            Dim lREcordset As New ADODB.Recordset

            lREcordset.ActiveConnection = pdbConnection
            lREcordset.CursorType = pcOpenStatic

            lsSQLQuery = " SELECT COUNT(*)"
            lsSQLQuery &= "  FROM ModelPage"
            lsSQLQuery &= " WHERE ModelId = '" & Trim(as_ModelId) & "'"
            lsSQLQuery &= "   AND IsCoreModelPage = " & abIncludeCorePages.ToString


            lREcordset.Open(lsSQLQuery)

            GetPageCountByModel = lREcordset(0).Value

            lREcordset.Close()

        End Function


        Public Function GetPagesByModel(ByRef ar_model As FBM.Model,
                                        Optional ByVal abLoadPage As Boolean = False,
                                        Optional ByVal abUseThreading As Boolean = False,
                                        Optional ByRef aoBackgroundWorker As System.ComponentModel.BackgroundWorker = Nothing,
                                        Optional ByRef abThreadAfter30pages As Boolean = False) _
                                        As List(Of FBM.Page)

            Try
                Dim lrPage As FBM.Page
                Dim lsSQLQuery As String = ""
                Dim lREcordset As New ADODB.Recordset
                Dim loPageLoadThread As System.Threading.Thread
                Dim liPageLoadedInd As Integer = 0

                lREcordset.ActiveConnection = pdbConnection
                lREcordset.CursorType = pcOpenStatic

                lsSQLQuery = " SELECT *"
                lsSQLQuery &= "  FROM ModelPage"
                lsSQLQuery &= " WHERE ModelId = '" & Trim(ar_model.ModelId) & "'"
                lsSQLQuery &= " ORDER BY PageName"


                lREcordset.Open(lsSQLQuery)

                '-----------------------------
                'Initialise the return value
                '-----------------------------
                If ar_model.Page.Count = 0 Then
                    GetPagesByModel = New List(Of FBM.Page)
                Else
                    GetPagesByModel = ar_model.Page
                End If

                If Not lREcordset.EOF Then
                    While Not lREcordset.EOF
                        '----------------------------------------------------------------
                        'Check to see if the Page has already been loaded for the Model
                        '----------------------------------------------------------------
                        lrPage = New FBM.Page(ar_model, Trim(lREcordset("PageId").Value), Trim(lREcordset("PageName").Value), lREcordset("LanguageId").Value)
                        lrPage.IsDirty = False 'Because we haven't edited anything on the Page.

                        Richmond.WriteToStatusBar("Loading Page: '" & lrPage.Name & "'")
                        lrPage.IsCoreModelPage = lREcordset("IsCoreModelPage").Value
                        lrPage.ShowFacts = lREcordset("ShowFacts").Value

                        liPageLoadedInd += 1
                        If aoBackgroundWorker IsNot Nothing Then
                            aoBackgroundWorker.ReportProgress(60 + ((liPageLoadedInd / lREcordset.RecordCount) * 40))
                            Richmond.WriteToProgressBar(60 + ((liPageLoadedInd / lREcordset.RecordCount) * 40))
                        End If

                        If ar_model.Page.Contains(lrPage) Then

                            If abLoadPage Then
                                lrPage = ar_model.Page.Find(AddressOf lrPage.Equals)
                                If Not lrPage.Loaded Then
                                    If abUseThreading Or (ar_model.Page.FindAll(Function(x) x.Loaded).Count > 45 And abThreadAfter30pages) Then
                                        loPageLoadThread = New System.Threading.Thread(AddressOf lrPage.Load)
                                        loPageLoadThread.Start(False)
                                    Else
                                        lrPage.Load(False)
                                    End If
                                End If
                            End If
                        Else
                            ar_model.Page.AddUnique(lrPage)
                            If abLoadPage Then
                                '----------------------------------------------------------------
                                'Page adds itself to the Model. Needs to do this for threading.
                                '----------------------------------------------------------------
                                If abUseThreading Then
                                    loPageLoadThread = New System.Threading.Thread(AddressOf lrPage.Load)
                                    loPageLoadThread.Start(True)
                                Else
                                    lrPage.Load(True)
                                End If
                            End If
                            Richmond.WriteToStatusBar("Loaded Page: '" & lrPage.Name & "'")
                        End If

                        lREcordset.MoveNext()
                    End While
                End If
                lREcordset.Close()

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)

                Return Nothing
            End Try

        End Function

        Public Sub UpdatePage(ByVal ar_page As FBM.Page)

            Dim lsSQLQuery As String

            Try

                '------------------------------------------------------
                'Code Safe: If the Page.Name = "" then throw an error
                '------------------------------------------------------
                If ar_page.Name = "" Then
                    Dim lsMessage As String = ""
                    lsMessage = "Tried to update the Page.Name to '' (Blank)"
                    Throw New Exception(lsMessage)
                End If

                lsSQLQuery = " UPDATE ModelPage"
                lsSQLQuery &= "   SET PageName = '" & Trim(ar_page.Name) & "'"
                lsSQLQuery &= "       ,ShowFacts = " & ar_page.ShowFacts
                lsSQLQuery &= " WHERE ModelId = '" & Trim(ar_page.Model.ModelId) & "'"
                lsSQLQuery &= "   AND PageId = '" & Trim(ar_page.PageId) & "'"

                'NB ModelId need never be updated, because the attribute value never changes over the life of the Page
                'NB language_id need never be updated, because the attribute value never changes over the life of the Page
                'NB IsCoreModelPage need never be updated, because the attribute value never changes over the life of the Page
                'If ever this changes, then simply change this procedure.


                pdbConnection.BeginTrans()
                pdbConnection.Execute(lsSQLQuery)
                pdbConnection.CommitTrans()

            Catch ex As Exception
                Dim lsMessage As String
                lsMessage = "Error: TablePage.UpdatePage"
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

    End Module
End Namespace