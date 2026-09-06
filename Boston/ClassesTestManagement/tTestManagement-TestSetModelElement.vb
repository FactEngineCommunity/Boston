Imports Newtonsoft.Json

Namespace TestManagement

    ''' <summary>
    ''' For the ModelElements being tested as part of a TestSet.
    ''' </summary>
    Public Class TestSetModelElement

        <JsonIgnore>
        Public Property TestSet As TestSet

        <JsonIgnore>
        Private _TestSetIdentifier As String

        <JsonProperty>
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

        <JsonIgnore>
        Public ModelElement As FBM.ModelObject

        <JsonIgnore>
        Private _ModelElementId As String

        <JsonProperty>
        Public Property ModelElementId As String
            Get
                If Me.ModelElement Is Nothing Then
                    Return Me.ModelElement.Id
                Else
                    Return Me._ModelElementId
                End If
            End Get
            Set(value As String)
                Me._ModelElementId = value
            End Set
        End Property


        ''' <summary>
        ''' Paraemeterless Constructor
        ''' </summary>
        Public Sub New()
        End Sub

    End Class

End Namespace