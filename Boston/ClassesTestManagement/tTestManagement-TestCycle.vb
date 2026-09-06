Imports Newtonsoft.Json

Namespace TestManagement

    ''' <summary>
    ''' Class for running tests
    ''' </summary>
    Public Class TestCycle
        Inherits Identifiable

        <JsonIgnore>
        Public TestSet As TestManagement.TestSet

        <JsonIgnore>
        Private _TestSetIdentifier As String

        <JsonProperty>
        <ForeignKeyReference(GetType(TestManagement.TestSet), NameOf(TestManagement.TestSet.Identifier), True)>
        Public Property TestSetIdentifier As String
            Get
                If Me.TestSet Is Nothing Then
                    Return Me._TestSetIdentifier
                Else
                    Return Me.TestSet.Identifier
                End If
            End Get
            Set(value As String)
                Me._TestSetIdentifier = value
            End Set
        End Property

        <JsonProperty>
        Public Property StartDateTime As DateTime?

        <JsonProperty>
        Public Property CompleteDateTime As DateTime?

        Public Property ClosedComplete As Boolean = False

        ''' <summary>
        ''' Parameterless Constructor
        ''' </summary>
        Public Sub New()
            MyBase.New()
        End Sub

    End Class

End Namespace