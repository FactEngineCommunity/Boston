Imports System.Reflection

Namespace TableModel

    Public Module tableORMModel

        Public Sub add_model(ByVal ar_model As FBM.Model)

            Try
                Dim lsSQLQuery As String = ""

                lsSQLQuery = "INSERT INTO MetaModelModel"
                lsSQLQuery &= " VALUES ("
                lsSQLQuery &= " '" & Trim(ar_model.ModelId) & "'"
                lsSQLQuery &= " ,'" & Trim(ar_model.Name) & "'"
                lsSQLQuery &= " ,'" & Trim(ar_model.ShortDescription) & "'"
                lsSQLQuery &= " ,'" & Trim(ar_model.LongDescription) & "'"
                lsSQLQuery &= " ,'" & Viev.NullVal(ar_model.EnterpriseId, "") & "'"
                lsSQLQuery &= " ,'" & Viev.NullVal(ar_model.SubjectAreaId, "") & "'"
                lsSQLQuery &= " ,'" & Viev.NullVal(ar_model.ProjectId, "") & "'"
                lsSQLQuery &= " ," & ar_model.ProjectPhaseId
                lsSQLQuery &= " ,'" & Viev.NullVal(ar_model.SolutionId, "") & "'"
                lsSQLQuery &= " ," & ar_model.IsConceptualModel
                lsSQLQuery &= " ," & ar_model.IsPhysicalModel
                lsSQLQuery &= " ," & ar_model.IsNamespace
                lsSQLQuery &= " ," & ar_model.IsEnterpriseModel
                lsSQLQuery &= " ,'" & ar_model.TargetDatabaseType.ToString & "'"
                lsSQLQuery &= " ,'" & ar_model.TargetDatabaseConnectionString & "'"
                If ar_model.Namespace Is Nothing Then
                    lsSQLQuery &= " ,''"
                Else
                    lsSQLQuery &= " ,'" & ar_model.Namespace.Id & "'"
                End If
                lsSQLQuery &= " ,'" & ar_model.CreatedByUserId & "'"
                lsSQLQuery &= " ,'" & ar_model.CoreVersionNumber & "'"
                lsSQLQuery &= " ,'" & Trim(ar_model.Server) & "'"
                lsSQLQuery &= " ,'" & Trim(ar_model.Database) & "'"
                lsSQLQuery &= " ,'" & Trim(ar_model.Schema) & "'"
                lsSQLQuery &= " ,'" & Trim(ar_model.Warehouse) & "'"
                lsSQLQuery &= " ,'" & Trim(ar_model.DatabaseRole) & "'"
                lsSQLQuery &= " ,'" & Trim(ar_model.Port) & "'"
                lsSQLQuery &= " ," & ar_model.StoreAsXML
                lsSQLQuery &= ")"

                Call pdbConnection.Execute(lsSQLQuery)

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
            End Try

        End Sub

        Public Sub DeleteModel(ByVal arModel As FBM.Model)

            Dim lsSQLQuery As String = ""

            lsSQLQuery = "DELETE FROM MetaModelModel"
            lsSQLQuery &= " WHERE ModelId = '" & arModel.ModelId & "'"

            'pdbConnection.BeginTrans()
            pdbConnection.Execute(lsSQLQuery)
            'pdbConnection.CommitTrans()

        End Sub

        Public Function ExistsModelByName(ByVal asModelName As String) As Boolean

            Dim lsSQLQuery As String = ""
            Dim lREcordset As New ADODB.Recordset

            '------------------------
            'Initialise return value
            '------------------------
            ExistsModelByName = False

            lREcordset.ActiveConnection = pdbConnection
            lREcordset.CursorType = pcOpenStatic

            lsSQLQuery = "SELECT COUNT(*)"
            lsSQLQuery &= "  FROM MetaModelModel"
            lsSQLQuery &= " WHERE ModelName = '" & Trim(asModelName) & "'"


            lREcordset.Open(lsSQLQuery)

            If lREcordset(0).Value > 0 Then
                ExistsModelByName = True
            Else
                ExistsModelByName = False
            End If

            lREcordset.Close()

        End Function

        Public Function ExistsModelById(ByVal asModelId As String) As Boolean

            Dim lsSQLQuery As String = ""
            Dim lREcordset As New ADODB.Recordset

            '------------------------
            'Initialise return value
            '------------------------
            ExistsModelById = False

            lREcordset.ActiveConnection = pdbConnection
            lREcordset.CursorType = pcOpenStatic

            lsSQLQuery = "SELECT COUNT(*)"
            lsSQLQuery &= "  FROM MetaModelModel"
            lsSQLQuery &= " WHERE ModelId = '" & Trim(asModelId) & "'"


            lREcordset.Open(lsSQLQuery)

            If lREcordset(0).Value > 0 Then
                ExistsModelById = True
            Else
                ExistsModelById = False
            End If

            lREcordset.Close()

        End Function

        Public Sub GetModelDetails(ByRef arModel As FBM.Model)

            Try
                Dim lsSQLQuery As String = ""
                Dim lREcordset As New RecordsetProxy()
                Dim lsMessage As String

                lREcordset.ActiveConnection = pdbConnection
                lREcordset.CursorType = pcOpenStatic

                lsSQLQuery = "SELECT *"
                lsSQLQuery &= " FROM MetaModelModel"
                lsSQLQuery &= " WHERE ModelId = '" & Trim(arModel.ModelId) & "'"

                lREcordset.Open(lsSQLQuery)

                If Not lREcordset.EOF Then
                    arModel.ModelId = lREcordset("ModelId").Value
                    arModel.Name = lREcordset("ModelName").Value
                    arModel.ShortDescription = Viev.NullVal(lREcordset("ShortDescription").Value, "")
                    arModel.LongDescription = Viev.NullVal(lREcordset("LongDescription").Value, "")
                    arModel.EnterpriseId = arModel.EnterpriseId
                    arModel.SubjectAreaId = Viev.NullVal(lREcordset("SubjectAreaId").Value, "")
                    arModel.ProjectId = Viev.NullVal(lREcordset("ProjectId").Value, "")
                    arModel.ProjectPhaseId = lREcordset("ProjectPhaseId").Value
                    arModel.SolutionId = Viev.NullVal(lREcordset("SolutionId").Value, "")

                    arModel.IsConceptualModel = True 'Default
                    arModel.IsPhysicalModel = False
                    arModel.IsNamespace = False
                    arModel.IsEnterpriseModel = True 'By default for this function.
                    arModel.StoreAsXML = CBool(lREcordset("StoreAsXML").Value)

                    Try
                        arModel.TargetDatabaseType = CType([Enum].Parse(GetType(pcenumDatabaseType), Viev.NullVal(lREcordset("TargetDatabaseType").Value, pcenumDatabaseType.None)), pcenumDatabaseType)
                    Catch
                        arModel.TargetDatabaseType = pcenumDatabaseType.None
                    End Try
                    arModel.TargetDatabaseConnectionString = Viev.NullVal(lREcordset("TargetDatabaseConnectionString").Value, "")
                    arModel.CreatedByUserId = NullVal(lREcordset("CreatedByUserId").Value, "")

                    arModel.CoreVersionNumber = Trim(lREcordset("CoreVersionNumber").Value)

                    'ODBC etc
                    arModel.Server = Trim(NullVal(lREcordset("Server").Value, ""))
                    arModel.Database = Trim(NullVal(lREcordset("DatabaseName").Value, ""))
                    arModel.Schema = Trim(NullVal(lREcordset("Schema").Value, ""))
                    arModel.Warehouse = Trim(NullVal(lREcordset("Warehouse").Value, ""))
                    arModel.DatabaseRole = Trim(NullVal(lREcordset("Role").Value, ""))
                    arModel.Port = Trim(NullVal(lREcordset("Port").Value, ""))
                Else

                    lsMessage = "No Model returned for ModelId: '" & arModel.ModelId & "'"
                    Throw New System.Exception(lsMessage)
                End If

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)

            End Try
        End Sub

        Public Function GetModels(ByRef asCreatedByUserId As String,
                                  Optional ByVal asNamespaceId As String = Nothing) As List(Of FBM.Model)

            Dim lrModel As FBM.Model
            Dim lsSQLQuery As String = ""
            Dim lREcordset As New ADODB.Recordset

            '-----------------------------
            'Initialise the return value
            '-----------------------------
            GetModels = New List(Of FBM.Model)

            Try

                lREcordset.ActiveConnection = pdbConnection
                lREcordset.CursorType = pcOpenStatic

                lsSQLQuery = "SELECT *"
                lsSQLQuery &= " FROM MetaModelModel"
                If asCreatedByUserId IsNot Nothing Then
                    lsSQLQuery &= " WHERE CreatedByUserId = '" & asCreatedByUserId & "'"
                ElseIf asNamespaceId IsNot Nothing Then
                    lsSQLQuery &= "  WHERE NamespaceId = '" & asNamespaceId & "'"
                End If
                lsSQLQuery &= " ORDER BY ModelName"

                lREcordset.Open(lsSQLQuery)

                While Not lREcordset.EOF
                    lrModel = New FBM.Model
                    lrModel.ModelId = lREcordset("ModelId").Value
                    lrModel.Name = lREcordset("ModelName").Value
                    lrModel.ShortDescription = Viev.NullVal(lREcordset("ShortDescription").Value, "")
                    lrModel.LongDescription = Viev.NullVal(lREcordset("LongDescription").Value, "")
                    lrModel.EnterpriseId = lrModel.EnterpriseId
                    lrModel.SubjectAreaId = Viev.NullVal(lREcordset("SubjectAreaId").Value, "")
                    lrModel.ProjectId = Viev.NullVal(lREcordset("ProjectId").Value, "")
                    lrModel.ProjectPhaseId = lREcordset("ProjectPhaseId").Value
                    lrModel.SolutionId = Viev.NullVal(lREcordset("SolutionId").Value, "")

                    lrModel.IsConceptualModel = True 'Default
                    lrModel.IsPhysicalModel = False
                    lrModel.IsNamespace = False
                    lrModel.IsEnterpriseModel = True 'By default for this function.
                    lrModel.StoreAsXML = CBool(lREcordset("StoreAsXML").Value)

                    Try
                        lrModel.TargetDatabaseType = CType([Enum].Parse(GetType(pcenumDatabaseType), Viev.NullVal(lREcordset("TargetDatabaseType").Value, pcenumDatabaseType.None)), pcenumDatabaseType)
                    Catch
                        lrModel.TargetDatabaseType = pcenumDatabaseType.None
                    End Try
                    lrModel.TargetDatabaseConnectionString = Viev.NullVal(lREcordset("TargetDatabaseConnectionString").Value, "")

                    lrModel.CreatedByUserId = NullVal(lREcordset("CreatedByUserId").Value, "")

                    lrModel.CoreVersionNumber = Trim(lREcordset("CoreVersionNumber").Value)

                    'ODBC etc
                    lrModel.Server = Trim(NullVal(lREcordset("Server").Value, ""))
                    lrModel.Database = Trim(NullVal(lREcordset("DatabaseName").Value, ""))
                    lrModel.Schema = Trim(NullVal(lREcordset("Schema").Value, ""))
                    lrModel.Warehouse = Trim(NullVal(lREcordset("Warehouse").Value, ""))
                    lrModel.DatabaseRole = Trim(NullVal(lREcordset("Role").Value, ""))
                    lrModel.Port = Trim(NullVal(lREcordset("Port").Value, ""))

                    GetModels.Add(lrModel)
                    lREcordset.MoveNext()
                End While

                lREcordset.Close()

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
            End Try

        End Function

        Public Sub update_model(ByVal ar_model As FBM.Model)

            Dim lsSQLQuery As String

            Try

                '------------------------------------------------------
                'Code Safe: If the ModelName = "" then throw an error
                '------------------------------------------------------
                If ar_model.Name = "" Then
                    Dim lsMessage As String = ""
                    lsMessage = "Tried to update the Model.Name to '' (Blank)"
                    Throw New Exception(lsMessage)
                End If

                lsSQLQuery = " UPDATE MetaModelModel"
                lsSQLQuery &= "   SET ModelName = '" & Trim(ar_model.Name) & "'"
                lsSQLQuery &= "       ,ShortDescription = '" & Trim(ar_model.ShortDescription) & "'"
                lsSQLQuery &= "       ,LongDescription = '" & Trim(ar_model.LongDescription) & "'"
                lsSQLQuery &= "       ,EnterpriseId = '" & Viev.NullVal(ar_model.EnterpriseId, "") & "'"
                lsSQLQuery &= "       ,SubjectAreaId = '" & Viev.NullVal(ar_model.SubjectAreaId, "") & "'"
                lsSQLQuery &= "       ,ProjectId = '" & Viev.NullVal(ar_model.ProjectId, "") & "'"
                lsSQLQuery &= "       ,ProjectPhaseId = " & ar_model.ProjectPhaseId
                lsSQLQuery &= "       ,SolutionId = '" & Viev.NullVal(ar_model.SolutionId, "") & "'"
                lsSQLQuery &= "       ,IsConceptualModel = " & ar_model.IsConceptualModel
                lsSQLQuery &= "       ,IsPhysicalModel = " & ar_model.IsPhysicalModel
                lsSQLQuery &= "       ,IsNamespace = " & ar_model.IsNamespace
                lsSQLQuery &= "       ,TargetDatabaseType = '" & Trim(ar_model.TargetDatabaseType.ToString) & "'"
                lsSQLQuery &= "       ,TargetDatabaseConnectionString = '" & Trim(ar_model.TargetDatabaseConnectionString) & "'"
                If ar_model.Namespace IsNot Nothing Then
                    lsSQLQuery &= "       ,NamespaceId = '" & Trim(ar_model.Namespace.Id) & "'"
                End If
                lsSQLQuery &= "       ,CreatedByUserId = '" & NullVal(ar_model.CreatedByUserId, "") & "'"
                lsSQLQuery &= "       ,CoreVersionNumber = '" & Trim(ar_model.CoreVersionNumber) & "'"
                lsSQLQuery &= "       ,Server = '" & Trim(ar_model.Server) & "'"
                lsSQLQuery &= "       ,DatabaseName = '" & Trim(ar_model.Database) & "'"
                lsSQLQuery &= "       ,[Schema] = '" & Trim(ar_model.Schema) & "'"
                lsSQLQuery &= "       ,Warehouse = '" & Trim(ar_model.Warehouse) & "'"
                lsSQLQuery &= "       ,[Role] = '" & Trim(ar_model.DatabaseRole) & "'"
                lsSQLQuery &= "       ,Port = '" & Trim(ar_model.Port) & "'"
                lsSQLQuery &= "       ,StoreAsXML = " & ar_model.StoreAsXML.ToString
                lsSQLQuery &= " WHERE ModelId = '" & Trim(ar_model.ModelId) & "'"

                pdbConnection.BeginTrans()
                pdbConnection.Execute(lsSQLQuery)
                pdbConnection.CommitTrans()

            Catch ex As Exception
                Dim lsMessage As String
                lsMessage = "Error: tableORMMOdel.UpdateModel"
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
            End Try

        End Sub

    End Module

End Namespace