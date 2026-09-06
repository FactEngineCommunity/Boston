Imports System.ComponentModel
Imports Newtonsoft.Json

Namespace TestManagement

    ''' <summary>
    ''' Class for results of tests
    ''' </summary>
    Public Class ProjectRequirement
        Inherits Identifiable

        <JsonIgnore>
        Private Shadows _Name As String

        <JsonProperty>
        <DisplayName("Title")>
        Public Shadows Property Name As String
            Get
                Return Me._Name
            End Get
            Set(value As String)
                Me._Name = value
            End Set
        End Property

        <JsonIgnore>
        <Browsable(False)>
        Public Project As ClientServer.Project

        <JsonIgnore>
        Private _ProjectId As String

        <JsonProperty>
        <Browsable(False)>
        Public Property ProjectId As String
            Get
                If Me.Project Is Nothing Then
                    Return Me._ProjectId
                Else
                    Return Me.Project.Id
                End If
            End Get
            Set(value As String)
                Me._ProjectId = value
            End Set
        End Property

        ''' <summary>
        ''' Parameterless Constructor
        ''' </summary>
        Public Sub New()
            MyBase.New()
        End Sub

    End Class

End Namespace