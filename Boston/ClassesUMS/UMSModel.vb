Imports System.Collections.Generic
Imports System.IO
Imports YamlDotNet.Serialization
Imports YamlDotNet.Serialization.NamingConventions

Namespace UMS


    ' ─── Type Definition ───────────────────────────────────────────────────────────

    Public Class Model

        Public Property Name As String

        ''' <summary>
        ''' The version of the Unified Modelling Schema Standard used to generate the .yaml file.
        ''' </summary>
        ''' <returns></returns>
        Public Property UMSVersionNr As String = "0.1"

        ''' <summary>
        ''' The Version Number of the Model, if one exists.
        ''' </summary>
        ''' <returns></returns>
        Public Property ModelVersionNr As String

        Public Property ModelElement As New List(Of UMS.ModelElement)

    End Class

End Namespace
