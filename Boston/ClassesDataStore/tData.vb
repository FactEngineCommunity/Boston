Namespace DataStore
    Public Class [Data]

        ''' <summary>
        ''' Unique ID for a Document/Text
        ''' </summary>
        ''' <returns></returns>
        Public Property ID As String = System.Guid.NewGuid.ToString
        Public Property Data As String 'JSON or Text
        Public Property DataClass As String 'E.g "Person" or a class in Boston
        Public Property EntryDate As DateTime 'Date of Entry.
        Public Property Source As String 'Whatever Source information you want
        Public Property Author As String 'Who contributed the Data
        Public Property Tags As String 'JSON set of Tags if required
        Public Property Location As String 'Where the data is from
        Public Property Notes As String 'Any notes

        Public Sub New()
            ' Default constructor
        End Sub

        Public Sub New(ByVal data As String, ByVal dataClass As String, ByVal entryDate As DateTime, ByVal source As String, ByVal author As String, ByVal tags As String, ByVal location As String, ByVal notes As String)

            Me.Data = data
            Me.DataClass = dataClass
            Me.EntryDate = entryDate
            Me.Source = source
            Me.Author = author
            Me.Tags = tags
            Me.Location = location
            Me.Notes = notes
        End Sub

    End Class

End Namespace
