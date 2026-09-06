Imports Newtonsoft.Json

Namespace TestManagement

    Public Class TestPlan
        Inherits TestManagement.Identifiable 'Name,Description,DateLastUpdated
        Implements IEquatable(Of TestManagement.TestPlan)

        <JsonIgnore>
        Private _ProjectId As String

        ''' <summary>
        ''' The Project for which the Test Plan is created for.
        ''' </summary>
        ''' <returns></returns>
        <JsonProperty>
        Public Property ProjectId As String
            Get
                Return Me._ProjectId
            End Get
            Set(value As String)
                Me._ProjectId = value
            End Set
        End Property

        <JsonIgnore>
        Private _PlannedStartDate As Nullable(Of Date)

        <JsonProperty>
        Public Property PlannedStartDate As Nullable(Of Date)
            Get
                Return Me._PlannedStartDate
            End Get
            Set(value As Nullable(Of Date))
                Me._PlannedStartDate = value
            End Set
        End Property

        <JsonIgnore>
        Private _PlannedEndDate As Nullable(Of Date)

        <JsonProperty>
        Public Property PlannedEndDate As Nullable(Of Date)
            Get
                Return Me._PlannedEndDate
            End Get
            Set(value As Nullable(Of Date))
                Me._PlannedEndDate = value
            End Set
        End Property

        <JsonIgnore>
        Public Property TestManager As ClientServer.User = Nothing

        <JsonIgnore>
        Private _TestManagerId As String

        <JsonProperty>
        Public Property TestManagerId As String
            Get
                If Me.TestManager Is Nothing Then
                    Return Me._TestManagerId
                Else
                    Return Me.TestManager.Id
                End If
            End Get
            Set(value As String)
                Me._TestManagerId = value
            End Set
        End Property

        <JsonIgnore>
        Private _ActualStartDate As Nullable(Of Date)

        <JsonProperty>
        Public Property ActualStartDate As Nullable(Of Date)
            Get
                Return Me._ActualStartDate
            End Get
            Set(value As Nullable(Of Date))
                Me._ActualStartDate = value
            End Set
        End Property

        <JsonIgnore>
        Private _ActualEndDate As Nullable(Of Date)

        <JsonProperty>
        Public Property ActualEndDate As Nullable(Of Date)
            Get
                Return Me._ActualEndDate
            End Get
            Set(value As Nullable(Of Date))
                Me._ActualEndDate = value
            End Set
        End Property

        <JsonIgnore>
        Public Staff As New List(Of ClientServer.User)

        Public Shadows Function Equals(other As TestPlan) As Boolean Implements IEquatable(Of TestPlan).Equals
            Return Me.Identifier = other.Identifier
        End Function
    End Class

End Namespace