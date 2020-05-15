Imports System.Xml.Serialization

Namespace ClientServer

    <Serializable()>
    Public Class [Namespace]

        Public Id As String = System.Guid.NewGuid.ToString
        Public Name As String = ""
        Public Number As String = ""
        Public Project As ClientServer.Project

        ''' <summary>
        ''' Paremeterless New for Serialisation.
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub New()
        End Sub

        Public Sub New(ByVal asId As String, ByVal asName As String, ByRef arProject As ClientServer.Project, Optional asNumber As String = Nothing)

            Me.Id = asId
            Me.Name = asName
            Me.Project = arProject

            If asNumber IsNot Nothing Then
                Me.Number = asNumber
            End If

        End Sub


    End Class

End Namespace
