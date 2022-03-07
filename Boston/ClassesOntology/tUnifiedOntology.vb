Imports System.Reflection

Namespace Ontology

    Public Class UnifiedOntology

        Public Id As String = System.Guid.NewGuid.ToString

        ''' <summary>
        ''' The name of the Unified Ontology
        ''' </summary>
        Public Name As String

        Public Model As New List(Of FBM.Model)

        Public Image As System.Drawing.Image


        ''' <summary>
        ''' Parameterless Constructor
        ''' </summary>
        Public Sub New()
        End Sub

        ''' <summary>
        ''' Constructor
        ''' </summary>
        ''' <param name="asName"></param>
        Public Sub New(ByVal asId As String, ByVal asName As String)

            Me.Id = asId
            Me.Name = asName

        End Sub

        ''' <summary>
        ''' Saves the UnifiedOntology to the database.
        ''' </summary>
        Public Sub Save()

            Try
                If Not TableUnifiedOntology.ExistsUnifiedOntology(Me) Then
                    TableUnifiedOntology.AddUnifiedOntology(Me)
                Else
                    Call TableUnifiedOntology.UpdateUnifiedOntology(Me)
                End If

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

    End Class

End Namespace
