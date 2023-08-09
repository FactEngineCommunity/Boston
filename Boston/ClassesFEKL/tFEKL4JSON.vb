Imports Newtonsoft.Json

Namespace FEKL
    Public Class FEKL4JSONObject
        Implements ICloneable
        Implements IEquatable(Of FEKL4JSONObject)

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
        Public Processed As Boolean = False

        Public ReadOnly Property InError As Boolean
            Get
                Return Me.ErrorType <> [Interface].publicConstants.pcenumErrorType.None
            End Get
        End Property

        Public ErrorType As [Interface].publicConstants.pcenumErrorType = [Interface].publicConstants.pcenumErrorType.None
        Public ErrorString As String = "No Error"

        Public Function Clone() As Object Implements ICloneable.Clone
            Dim clonedObj As New FEKL4JSONObject()

            clonedObj.Id = Me.Id
            clonedObj.DocumentName = Me.DocumentName
            clonedObj.DocumentLocation = Me.DocumentLocation
            clonedObj.DocumentLocationJson = Me.DocumentLocationJson
            clonedObj.ObjectType = Me.ObjectType
            clonedObj.FEKLStatement = Me.FEKLStatement
            clonedObj.PageNumber = Me.PageNumber
            clonedObj.LineNumber = Me.LineNumber
            clonedObj.SectionId = Me.SectionId
            clonedObj.SectionName = Me.SectionName
            clonedObj.RequirementId = Me.RequirementId
            clonedObj.StartOffset = Me.StartOffset
            clonedObj.EndOffset = Me.EndOffset

            clonedObj.Processing = Me.Processing
            clonedObj.Processed = Me.Processed

            clonedObj.ErrorType = Me.ErrorType
            clonedObj.ErrorString = Me.ErrorString

            Return clonedObj
        End Function

        Public Shadows Function Equals(other As FEKL4JSONObject) As Boolean Implements IEquatable(Of FEKL4JSONObject).Equals
            Return Me.Id = other.Id
        End Function
    End Class

    Public Class FEKL4JSON
        Implements ICloneable

        <JsonProperty("FEKL4JSON")>
        Public Property FEKLStatement As New List(Of FEKL4JSONObject)

        Public Function Clone() As Object Implements ICloneable.Clone
            Dim clonedObj As New FEKL4JSON()

            For Each item In FEKLStatement
                clonedObj.FEKLStatement.Add(DirectCast(item.Clone(), FEKL4JSONObject))
            Next

            Return clonedObj
        End Function
    End Class

End Namespace