Imports System.Reflection
Imports System.Text.RegularExpressions

Namespace TableValueType

    Public Module Table_value_type

        Sub AddValueType(ByVal arValueType As FBM.ValueType)

            Dim lsSQLQuery As String = ""

            Try
                lsSQLQuery = "INSERT INTO MetaModelValueType"
                lsSQLQuery &= " VALUES ("
                lsSQLQuery &= " #" & Now & "#"
                lsSQLQuery &= " ,#" & Now & "#"
                lsSQLQuery &= " ,'" & Trim(Replace(arValueType.Model.ModelId, "'", "`")) & "'"
                lsSQLQuery &= " ,'" & Trim(Replace(arValueType.Id, "'", "`")) & "'"
                lsSQLQuery &= " ,'" & Trim(Replace(arValueType.Name, "'", "`")) & "'"
                lsSQLQuery &= " ,'" & Trim(arValueType.DataType.ToString) & "'"
                lsSQLQuery &= " ," & arValueType.DataTypeLength
                lsSQLQuery &= " ," & arValueType.DataTypePrecision
                lsSQLQuery &= " ," & arValueType.IsMDAModelElement
                lsSQLQuery &= " ,'" & arValueType.GUID & "'"
                lsSQLQuery &= " ," & arValueType.IsIndependent
                lsSQLQuery &= ")"

                Call pdbConnection.Execute(lsSQLQuery)
            Catch ex As Exception
                Dim lsMessage As String
                lsMessage = "Error: TableValueType.AddValueType"
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try
        End Sub

        Public Sub DeleteValueType(ByVal arValueType As FBM.ValueType)

            Dim lsSQLQuery As String = ""

            lsSQLQuery = "DELETE FROM MetaModelValueType"
            lsSQLQuery &= " WHERE ValueTypeId = '" & Replace(arValueType.Id, "'", "`") & "'"
            lsSQLQuery &= "   AND ModelId = '" & Replace(arValueType.Model.ModelId, "'", "`") & "'"


            pdbConnection.BeginTrans()
            pdbConnection.Execute(lsSQLQuery)
            pdbConnection.CommitTrans()

        End Sub

        Public Function ExistsValueType(ByVal arValueType As FBM.ValueType) As Boolean

            Dim lsSQLQuery As String = ""
            Dim lREcordset As New ADODB.Recordset

            '------------------------
            'Initialise return value
            '------------------------
            ExistsValueType = False

            lREcordset.ActiveConnection = pdbConnection
            lREcordset.CursorType = pcOpenStatic

            lsSQLQuery = "SELECT COUNT(*)"
            lsSQLQuery &= "  FROM MetaModelValueType"
            lsSQLQuery &= " WHERE ValueTypeId = '" & Trim(Replace(arValueType.Id, "'", "`")) & "'"
            lsSQLQuery &= " AND ModelId = '" & Trim(Replace(arValueType.Model.ModelId, "'", "`")) & "'"


            lREcordset.Open(lsSQLQuery)

            If lREcordset(0).Value > 0 Then
                ExistsValueType = True
            Else
                ExistsValueType = False
            End If

            lREcordset.Close()

        End Function

        Public Function ExistsValueTypeByModel(ByVal arValueType As FBM.ValueType) As Boolean

            Dim lsSQLQuery As String = ""
            Dim lREcordset As New ADODB.Recordset

            '-------------------------
            'Initialise return value
            '-------------------------
            ExistsValueTypeByModel = False

            lREcordset.ActiveConnection = pdbConnection
            lREcordset.CursorType = pcOpenStatic

            lsSQLQuery = "SELECT COUNT(*)"
            lsSQLQuery &= "  FROM MetaModelValueType VT,"
            lsSQLQuery &= "       MetaModelModelDictionary MD"
            lsSQLQuery &= " WHERE VT.ValueTypeId = '" & Trim(Replace(arValueType.Id, "'", "`")) & "'"
            lsSQLQuery &= "   AND VT.ValueTypeId = MD.Symbol"
            lsSQLQuery &= "   AND MD.ModelId = '" & Trim(arValueType.Model.ModelId) & "'"
            lsSQLQuery &= "   AND MD.ModelId = VT.ModelId"


            lREcordset.Open(lsSQLQuery)

            If lREcordset(0).Value > 0 Then
                ExistsValueTypeByModel = True
            Else
                ExistsValueTypeByModel = False
            End If

            lREcordset.Close()

        End Function

        Function GetValueTypeCountByModel(ByVal asModelId As String) As Integer


            Dim lsSQLQuery As String = ""
            Dim lREcordset As New ADODB.Recordset

            lREcordset.ActiveConnection = pdbConnection
            lREcordset.CursorType = pcOpenStatic

            lsSQLQuery = " SELECT COUNT(*)"
            lsSQLQuery &= "  FROM MetaModelValueType VT,"
            lsSQLQuery &= "       MetaModelModelDictionary MD"
            lsSQLQuery &= " WHERE MD.ModelId = '" & Trim(asModelId) & "'"
            lsSQLQuery &= "   AND MD.Symbol = VT.ValueTypeId"
            lsSQLQuery &= "   AND MD.ModelId = VT.ModelId"


            lREcordset.Open(lsSQLQuery)

            GetValueTypeCountByModel = lREcordset(0).Value

            lREcordset.Close()

        End Function

        Sub GetValueTypeDetails(ByRef arValueType As FBM.ValueType)

            Dim lsSQLQuery As String = ""
            Dim lREcordset As New ADODB.Recordset

            Try
                lREcordset.ActiveConnection = pdbConnection
                lREcordset.CursorType = pcOpenStatic

                lsSQLQuery = "SELECT *"
                lsSQLQuery &= " FROM MetaModelValueType"
                lsSQLQuery &= " WHERE ValueTypeId = '" & Trim(arValueType.Id) & "'"
                lsSQLQuery &= " AND ModelId = '" & Trim(arValueType.Model.ModelId) & "'"

                lREcordset.Open(lsSQLQuery)

                If Not lREcordset.EOF Then
                    arValueType.Id = lREcordset("ValueTypeId").Value
                    arValueType.Name = lREcordset("ValueTypeName").Value
                    arValueType.DataType = CType([Enum].Parse(GetType(pcenumORMDataType), Trim(lREcordset("DataType").Value)), pcenumORMDataType)
                    arValueType.DataTypeLength = lREcordset("DataTypeLength").Value
                    arValueType.DataTypePrecision = lREcordset("DataTypePrecision").Value
                    arValueType.IsMDAModelElement = CBool(lREcordset("IsMDAModelElement").Value)
                    arValueType.GUID = lREcordset("GUID").Value
                    arValueType.IsIndependent = CBool(lREcordset("IsIndependent").Value)
                    arValueType.isDirty = False
                Else
                    Dim lsMessage As String = "Error: GetValueTypeDetailsById: No ValueType returned for ValueTypeId: " & arValueType.Id
                    Throw New Exception(lsMessage)
                End If

                lREcordset.Close()

            Catch ex As Exception
                Dim lsMessage As String
                lsMessage = "Error: TableValueType.GetValueTypeDetailsById"
                lsMessage &= vbCrLf & lsMessage
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Public Function GetValueTypesByModel(ByRef arModel As FBM.Model) As List(Of FBM.ValueType)

            Dim lsMessage As String
            Dim lrValueType As FBM.ValueType
            Dim lsSQLQuery As String = ""
            Dim lREcordset As New ADODB.Recordset

            lREcordset.ActiveConnection = pdbConnection
            lREcordset.CursorType = pcOpenStatic

            '-----------------------------
            'Initialise the return value
            '-----------------------------
            GetValueTypesByModel = New List(Of FBM.ValueType)

            Try
                '---------------------------------------------
                'First get EntityTypes with no ParentEntityId
                '---------------------------------------------
                lsSQLQuery = " SELECT VT.*, MD.*"
                lsSQLQuery &= "  FROM MetaModelValueType VT,"
                lsSQLQuery &= "       MetaModelModelDictionary MD"
                lsSQLQuery &= " WHERE MD.ModelId = '" & Trim(arModel.ModelId) & "'"
                lsSQLQuery &= "   AND MD.Symbol = VT.ValueTypeId"
                lsSQLQuery &= "   AND MD.ModelId = VT.ModelId"


                lREcordset.Open(lsSQLQuery)

                If Not lREcordset.EOF Then
                    While Not lREcordset.EOF
                        lrValueType = New FBM.ValueType
                        lrValueType.isDirty = False
                        lrValueType.Model = arModel
                        lrValueType.Id = lREcordset("ValueTypeId").Value
                        lrValueType.Name = lrValueType.Id '20210813-VM-Was lREcordset("ValueTypeName").Value  NB Can remove this after time, because Name must be Id in Boston. FOL.
                        lrValueType.ShortDescription = Trim(Viev.NullVal(lREcordset("ShortDescription").Value, ""))
                        lrValueType.LongDescription = Trim(Viev.NullVal(lREcordset("LongDescription").Value, ""))
                        lrValueType.ConceptType = pcenumConceptType.ValueType
                        lrValueType.DataType = CType([Enum].Parse(GetType(pcenumORMDataType), Trim(lREcordset("DataType").Value)), pcenumORMDataType)
                        lrValueType.DataTypeLength = lREcordset("DataTypeLength").Value
                        lrValueType.DataTypePrecision = lREcordset("DataTypePrecision").Value
                        lrValueType.IsMDAModelElement = CBool(lREcordset("IsMDAModelElement").Value)
                        lrValueType.GUID = lREcordset("GUID").Value
                        lrValueType.IsIndependent = CBool(lREcordset("IsIndependent").Value)
                        lrValueType.isDirty = False

                        Call TableValueTypeValueConstraint.GetValueConstraintsByValueType(lrValueType)

                        If lrValueType.ValueConstraint.Count > 0 Then
                            Dim lsConstraint As String
                            Dim loRegularExpression As Regex = New Regex("[0-9]*\.\.[0-9]+")
                            For Each lsConstraint In lrValueType.ValueConstraint
                                If loRegularExpression.IsMatch(lsConstraint) Then
                                    Dim lasRange() As String = Split(lsConstraint, "..")
                                    If CInt(lasRange(0)) < CInt(lasRange(1)) Then
                                        Dim liInd As Integer
                                        For liInd = CInt(lasRange(0)) To CInt(lasRange(1))
                                            lrValueType.Instance.Add(liInd.ToString)
                                        Next
                                    End If
                                Else
                                    lrValueType.Instance.Add(lsConstraint)
                                End If
                            Next
                        End If

                        '------------------------------------------------
                        'Link to the Concept within the ModelDictionary
                        '------------------------------------------------
                        Dim lrDictionaryEntry As New FBM.DictionaryEntry(arModel, lrValueType.Id, pcenumConceptType.ValueType, lrValueType.ShortDescription, lrValueType.LongDescription)
                        lrDictionaryEntry = arModel.AddModelDictionaryEntry(lrDictionaryEntry)


                        If lrDictionaryEntry Is Nothing Then
                            lsMessage = "Cannot find DictionaryEntry in the ModelDictionary for ValueType:"
                            lsMessage &= vbCrLf & "Model.Id: " & arModel.ModelId
                            lsMessage &= vbCrLf & "ValueType.Id: " & lrValueType.Id
                            Throw New Exception(lsMessage)
                        End If

                        lrValueType.Concept = lrDictionaryEntry.Concept


                        GetValueTypesByModel.Add(lrValueType)
                        lREcordset.MoveNext()
                    End While
                End If

                lREcordset.Close()

            Catch ex As Exception
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Function

        Public Sub ModifyKey(ByVal arValueType As FBM.ValueType, ByVal as_new_key As String)

            Dim lsSQLQuery As String
            Dim lrValueType As New FBM.ValueType

            Try

                lrValueType.Model = arValueType.Model
                lrValueType.Id = as_new_key
                lrValueType.Name = as_new_key

                If Table_value_type.ExistsValueType(lrValueType) Then
                    '-------------------------------------------------------------------
                    'A ValueType with the same Id already exists, so don't do anything
                    '-------------------------------------------------------------------
                Else
                    lsSQLQuery = " UPDATE MetaModelValueType"
                    lsSQLQuery &= "   SET ValueTypeId = '" & Trim(Replace(as_new_key, "'", "`")) & "'"
                    lsSQLQuery &= " WHERE ValueTypeId = '" & Trim(Replace(arValueType.Id, "'", "`")) & "'"
                    lsSQLQuery &= "   AND ModelId = '" & Trim(Replace(arValueType.Model.ModelId, "'", "`")) & "'"

                    pdbConnection.BeginTrans()
                    pdbConnection.Execute(lsSQLQuery)
                    pdbConnection.CommitTrans()
                End If

            Catch ex As Exception
                Dim lsMessage As String
                lsMessage = "Error: TableValueType.ModifyKey"
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Public Sub UpdateValueType(ByVal arValueType As FBM.ValueType)

            Dim lsSQLQuery As String

            Try
                lsSQLQuery = " UPDATE MetaModelValueType"
                lsSQLQuery &= "   SET ValueTypeName = '" & Trim(Replace(arValueType.Name, "'", "`")) & "'"
                lsSQLQuery &= "       , DataType = '" & Trim(arValueType.DataType.ToString) & "'"
                lsSQLQuery &= "       , DataTypeLength = " & arValueType.DataTypeLength
                lsSQLQuery &= "       , DataTypePrecision = " & arValueType.DataTypePrecision
                lsSQLQuery &= "       , IsMDAModelElement = " & arValueType.IsMDAModelElement
                lsSQLQuery &= "       , [GUID] = '" & arValueType.GUID & "'"
                lsSQLQuery &= "       , IsIndependent = " & arValueType.IsIndependent
                lsSQLQuery &= " WHERE ValueTypeId = '" & Trim(Replace(arValueType.Id, "'", "`")) & "'"
                lsSQLQuery &= "   AND ModelId = '" & Trim(Replace(arValueType.Model.ModelId, "'", "`")) & "'"

                pdbConnection.BeginTrans()
                pdbConnection.Execute(lsSQLQuery)
                pdbConnection.CommitTrans()

            Catch ex As Exception

                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)

                pdbConnection.RollbackTrans()
            End Try

        End Sub

    End Module
End Namespace