Imports Newtonsoft.Json
Imports System.ComponentModel
Imports System.Xml.Serialization
Imports System.Reflection

Namespace KnowledgeGraph

    <Serializable()>
    Public Class ConceptClassificationValue

        <XmlIgnore>
        <JsonIgnore>
        Public Model As FBM.Model

        <XmlAttribute>
        <JsonProperty>
        Public Property Identifier As String = System.Guid.NewGuid.ToString

        <XmlAttribute>
        <JsonIgnore>
        Private _ModelId As String = ""
        Public Property ModelId As String
            Get
                If Me.Model IsNot Nothing Then
                    Return Me.Model.ModelId
                Else
                    Return Me._ModelId
                End If
            End Get
            Set(value As String)
                Me._ModelId = value
            End Set
        End Property

        <XmlIgnore>
        <JsonIgnore>
        Private _Concept As String = ""
        Public Property Concept As String
            Get
                Try
                    Return Me.ModelElement.Id
                Catch ex As Exception
                    Return Me._Concept
                End Try
            End Get
            Set(value As String)
                Me._Concept = value
            End Set
        End Property

        <XmlIgnore>
        <JsonIgnore>
        <Browsable(False)>
        Public ModelElement As FBM.ModelObject

        <XmlAttribute>
        <JsonIgnore>
        Private _ClassificationType As String = ""
        Public Property ClassificationType As String  'E.g. Q6 Facet, Type, Level
            Get
                Return Me._ClassificationType
            End Get
            Set(value As String)
                Me._ClassificationType = value
            End Set
        End Property

        <XmlIgnore>
        <JsonIgnore>
        Public _ClassificationValue As String = "" 'E.g. Q6 Abbreviation, Agent, Metadata

        <XmlAttribute>
        Public Property ClassificationValue As String
            Get
                Return Me._ClassificationValue
            End Get
            Set(value As String)
                Me._ClassificationValue = value
            End Set
        End Property

        ''' <summary>
        ''' Parameterless Constructor
        ''' </summary>
        Public Sub New()
        End Sub

        Public Sub New(ByRef arModel As FBM.Model, ByRef arModelElement As FBM.ModelObject, ByVal asClassificationType As String, ByVal asClassificationValue As String)

            Try
                Me.Model = arModel
                Me.ModelElement = arModelElement
                Me.ClassificationType = asClassificationType
                Me.ClassificationValue = asClassificationValue
            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
            End Try

        End Sub

    End Class

End Namespace
