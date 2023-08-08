Imports Newtonsoft.Json

Namespace FEKL
    Public Class FEKL4JSONObject

        Public Property Id As String
        Public Property DocumentName As String
        Public Property DocumentLocation As String
        Public Property DocumentLocationJson As String
        Public Property ObjectType As String
        Public Property FEKLStatement As String
        Public Property PageNumber As Integer
        Public Property LineNumber As Integer
        Public Property SectionId As String
        Public Property SectionName As String
        Public Property RequirementId As String
        Public Property StartOffset As Integer
        Public Property EndOffset As Integer

        Public Processing As Boolean = False

        Public ReadOnly Property InError As Boolean
            Get
                Return Me.ErrorType <> [Interface].publicConstants.pcenumErrorType.None
            End Get
        End Property

        Public ErrorType As [Interface].publicConstants.pcenumErrorType = [Interface].publicConstants.pcenumErrorType.None
        Public ErrorString As String = "No Error"

    End Class

    Public Class FEKL4JSON

        <JsonProperty("FEKL4JSON")>
        Public Property FEKLStatement As New List(Of FEKL4JSONObject)
    End Class

End Namespace