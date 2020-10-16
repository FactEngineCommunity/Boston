
Partial Public Class tApplication

    Public Function getModelsByModelElementName(ByVal asModelElementName As String) As List(Of FBM.Model)

        Dim lsSQLQuery As String = ""
        Dim lREcordset As New ADODB.Recordset

        Dim larModel As New List(Of FBM.Model)

        Try
            lREcordset.ActiveConnection = pdbConnection
            lREcordset.CursorType = pcOpenStatic

            lsSQLQuery = "SELECT *"
            lsSQLQuery &= " FROM MetaModelModelDictionary"
            lsSQLQuery &= " WHERE Symbol = '" & Replace(asModelElementName, "'", "`") & "'"

            lREcordset.Open(lsSQLQuery)

            While Not lREcordset.EOF

                larModel.AddUnique(Me.Models.Find(Function(x) x.ModelId = lREcordset("ModelId").Value))

                lREcordset.MoveNext()
            End While

            lREcordset.Close()

            Return larModel

        Catch ex As Exception

            Dim lsMessage As String
            lsMessage = "Error: TableModelDictionary.DoesModelObjectExistInAntotherModel"
            lsMessage &= vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)

            Return larModel
        End Try

    End Function

    Public Function getModelsByPredicatePartText(ByVal asPredicatePartText As String) As List(Of FBM.Model)

        Dim lsSQLQuery As String = ""
        Dim lREcordset As New ADODB.Recordset

        Dim larModel As New List(Of FBM.Model)

        Try
            lREcordset.ActiveConnection = pdbConnection
            lREcordset.CursorType = pcOpenStatic

            lsSQLQuery = "SELECT *"
            lsSQLQuery &= " FROM MetaModelPredicatePart"
            lsSQLQuery &= " WHERE PredicatePartText = '" & Replace(asPredicatePartText, "'", "`") & "'"

            lREcordset.Open(lsSQLQuery)

            While Not lREcordset.EOF

                larModel.AddUnique(Me.Models.Find(Function(x) x.ModelId = lREcordset("ModelId").Value))

                lREcordset.MoveNext()
            End While

            lREcordset.Close()

            Return larModel

        Catch ex As Exception

            Dim lsMessage As String
            lsMessage = "Error: TableModelDictionary.DoesModelObjectExistInAntotherModel"
            lsMessage &= vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)

            Return larModel
        End Try

    End Function


End Class



