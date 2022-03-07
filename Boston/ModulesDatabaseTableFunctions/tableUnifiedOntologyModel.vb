Imports System.Reflection

Public Module tableUnifiedOntologyModel

    Public Sub AddModelToUnifiedOntology(ByRef arModel As FBM.Model, ByRef arUnifiedOntology As Ontology.UnifiedOntology)

        Try

            Dim lsSQLQuery As String = ""

            lsSQLQuery = " INSERT INTO UnifiedOntologyModel"
            lsSQLQuery &= "  VALUES ("
            lsSQLQuery &= "  '" & arUnifiedOntology.Id & "'"
            lsSQLQuery &= "  ,'" & arModel.ModelId & "'"
            lsSQLQuery &= " )"

            pdbConnection.Execute(lsSQLQuery)

        Catch ex As Exception
            Dim lsMessage1 As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage1 &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Sub

    ''' <summary>
    ''' Returns the count of Models included in the UnifiedOntology.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function getModelCountByUnifiedOntology(ByRef arUnifiedOntology As Ontology.UnifiedOntology) As Integer

        Try
            Dim lsSQLQuery As String = ""
            Dim lREcordset As New ADODB.Recordset

            lREcordset.ActiveConnection = pdbConnection
            lREcordset.CursorType = pcOpenStatic

            lsSQLQuery = " SELECT COUNT(*)"
            lsSQLQuery &= "  FROM UnifiedOntologyModel"
            lsSQLQuery &= "  WHERE UnifiedOntologyId = '" & arUnifiedOntology.Id & "'"

            lREcordset.Open(lsSQLQuery)

            Return lREcordset(0).Value

            lREcordset.Close()

        Catch ex As Exception
            Dim lsMessage1 As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage1 &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Function

    Public Function getModelsForUnifiedOntology(ByRef arUnifiedOntology As Ontology.UnifiedOntology) As List(Of FBM.Model)

        Dim lsMessage As String
        Dim lsSQLQuery As String = ""
        Dim lREcordset As New ADODB.Recordset

        Dim lrModel As FBM.Model
        Dim larModel As New List(Of FBM.Model)

        lREcordset.ActiveConnection = pdbConnection
        lREcordset.CursorType = pcOpenStatic

        Try
            lsSQLQuery = " SELECT *"
            lsSQLQuery &= "  FROM UnifiedOntologyModel"
            lsSQLQuery &= " WHERE UnifiedOntologyId = '" & arUnifiedOntology.Id & "'"

            lREcordset.Open(lsSQLQuery)

            If Not lREcordset.EOF Then
                While Not lREcordset.EOF
                    lrModel = New FBM.Model
                    lrModel.ModelId = lREcordset("ModelId").Value
                    Call TableModel.GetModelDetails(lrModel)
                    larModel.Add(lrmodel)
                    arUnifiedOntology.Model.Add(lrModel)

                    'Get the Pages for the Model (but don't load the Pages)
                    TablePage.GetPagesByModel(lrModel, False)

                    lREcordset.MoveNext()
                End While
            End If

            lREcordset.Close()

            Return larModel

        Catch ex As Exception
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)

            Return larModel
        End Try

    End Function

    Public Function isModelInUnifiedOntology(ByRef arModel As FBM.Model, ByRef arUnifiedOntology As Ontology.UnifiedOntology) As Boolean

        Try
            Dim lsSQLQuery As String = ""
            Dim lREcordset As New ADODB.Recordset

            lREcordset.ActiveConnection = pdbConnection
            lREcordset.CursorType = pcOpenStatic

            lsSQLQuery = " SELECT COUNT(*)"
            lsSQLQuery &= "  FROM UnifiedOntologyModel"
            lsSQLQuery &= " WHERE UnifiedOntologyId = '" & arUnifiedOntology.Id & "'"
            lsSQLQuery &= "   AND ModelId = '" & arModel.ModelId & "'"

            lREcordset.Open(lsSQLQuery)

            Return lREcordset(0).Value > 0

            lREcordset.Close()

        Catch ex As Exception
            Dim lsMessage1 As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage1 &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Function

    Public Sub removeModelFromUnifiedOntology(ByRef arModel As FBM.Model, ByRef arUnifiedOntology As Ontology.UnifiedOntology)

        Try

            Dim lsSQLQuery As String = ""

            lsSQLQuery = " DELETE FROM UnifiedOntologyModel"
            lsSQLQuery &= " WHERE UnifiedOntologyId = '" & arUnifiedOntology.Id & "'"
            lsSQLQuery &= "   AND ModelId = '" & arModel.ModelId & "'"

            pdbConnection.Execute(lsSQLQuery)

        Catch ex As Exception
            Dim lsMessage1 As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage1 &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Sub

End Module
