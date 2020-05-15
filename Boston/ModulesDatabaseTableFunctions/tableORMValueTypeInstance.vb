Imports System.Reflection

Namespace TableValueTypeInstance

    Module zTable_ValueTypeInstance

        Public Sub DeleteValueTypeInstance(ByVal arValueTypeInstance As FBM.ValueTypeInstance)

            Dim lsSQLQuery As String = ""

            lsSQLQuery = "DELETE FROM ModelConceptInstance"
            lsSQLQuery &= " WHERE PageId = '" & Trim(arValueTypeInstance.Page.PageId) & "'"
            lsSQLQuery &= "   AND Symbol = '" & Trim(arValueTypeInstance.Id) & "'"
            lsSQLQuery &= "   AND ConceptType = '" & pcenumConceptType.ValueType.ToString & "'"

            pdbConnection.BeginTrans()
            Call pdbConnection.Execute(lsSQLQuery)
            pdbConnection.CommitTrans()

        End Sub

        Function getValueTypeInstance_count_by_page(ByVal ar_page As FBM.Page) As Integer

            '-------------------------------------------------------------------------------------------------
            'NB EnterpriseInstances never manifest within the database, they manifest within the database as
            '  SymbolInstances, so we must count the SymbolInstances that match the arguments.
            '-------------------------------------------------------------------------------------------------

            Dim lsSQLQuery As String = ""
            Dim lREcordset As New ADODB.Recordset

            lREcordset.ActiveConnection = pdbConnection
            lREcordset.CursorType = pcOpenStatic

            lsSQLQuery = " SELECT COUNT(*)"
            lsSQLQuery &= "  FROM ModelConceptInstance ci,"
            lsSQLQuery &= "       MetaModelValueType et"
            lsSQLQuery &= " WHERE ci.ModelId = '" & Trim(ar_page.Model.ModelId) & "'"
            lsSQLQuery &= "   AND ci.PageId = '" & Trim(ar_page.PageId) & "'"
            lsSQLQuery &= "   AND ci.Symbol = et.ValueTypeId"


            lREcordset.Open(lsSQLQuery)

            getValueTypeInstance_count_by_page = lREcordset(0).Value

            lREcordset.Close()

        End Function

        Function getValueTypeInstances_by_page(ByRef lrPage As FBM.Page) As List(Of FBM.ValueTypeInstance)

            Dim lrValueTypeInstance As FBM.ValueTypeInstance
            Dim lsSQLQuery As String = ""
            Dim lREcordset As New ADODB.Recordset

            '-----------------------------
            'Initialise the return value
            '-----------------------------
            getValueTypeInstances_by_page = New List(Of FBM.ValueTypeInstance)

            Try
                lREcordset.ActiveConnection = pdbConnection
                lREcordset.CursorType = pcOpenStatic

                lsSQLQuery = " SELECT vt.*, ci.*"
                lsSQLQuery &= "  FROM MetaModelValueType vt,"
                lsSQLQuery &= "       ModelConceptInstance ci"
                lsSQLQuery &= " WHERE ci.PageId = '" & Trim(lrPage.PageId) & "'"
                lsSQLQuery &= "   AND ci.Symbol = vt.ValueTypeId"
                lsSQLQuery &= "   AND vt.ModelId = '" & Trim(lrPage.Model.ModelId) & "'"
                lsSQLQuery &= "   AND ci.ConceptType = '" & pcenumConceptType.ValueType.ToString & "'"

                lREcordset.Open(lsSQLQuery)

                If Not lREcordset.EOF Then
                    While Not lREcordset.EOF
                        lrValueTypeInstance = New FBM.ValueTypeInstance
                        lrValueTypeInstance.Model = lrPage.Model
                        lrValueTypeInstance.Page = lrPage
                        lrValueTypeInstance.Id = lREcordset("ValueTypeId").Value
                        lrValueTypeInstance.ValueType.Id = lrValueTypeInstance.Id
                        lrValueTypeInstance.ValueType = lrPage.Model.ValueType.Find(AddressOf lrValueTypeInstance.ValueType.Equals)

                        '-------------------------------------------------------------------------------------------
                        'CodeSafe: Remove the ValueTypeInstance if it references a ValueType that no longer exists
                        If lrValueTypeInstance.ValueType Is Nothing Then
                            Call TableValueTypeInstance.DeleteValueTypeInstance(lrValueTypeInstance)
                        Else
                            lrValueTypeInstance.DataType = CType([Enum].Parse(GetType(pcenumORMDataType), Trim(lREcordset("DataType").Value)), pcenumORMDataType)
                            lrValueTypeInstance.DataTypeLength = lREcordset("DataTypeLength").Value
                            lrValueTypeInstance.DataTypePrecision = lREcordset("DataTypePrecision").Value
                            lrValueTypeInstance.ValueConstraint = lrValueTypeInstance.ValueType.ValueConstraint.Clone
                            lrValueTypeInstance.ShortDescription = lrValueTypeInstance.ValueType.ShortDescription
                            lrValueTypeInstance.LongDescription = lrValueTypeInstance.ValueType.LongDescription
                            lrValueTypeInstance.GUID = lrValueTypeInstance.ValueType.GUID
                            lrValueTypeInstance.IsIndependent = lrValueTypeInstance.ValueType.IsIndependent

                            lrValueTypeInstance.Name = lREcordset("ValueTypeName").Value
                            lrValueTypeInstance.X = lREcordset("x").Value
                            lrValueTypeInstance.Y = lREcordset("y").Value

                            getValueTypeInstances_by_page.Add(lrValueTypeInstance)
                        End If

                        lREcordset.MoveNext()
                    End While
                Else
                    '----------------------------------------
                    'Nothing to add
                    '-------------------
                End If

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Function

    End Module

End Namespace