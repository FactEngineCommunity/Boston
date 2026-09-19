Imports System.Xml.Serialization
Imports Newtonsoft.Json


Namespace FBM

    ''' <summary>
    ''' Synonyms for Model Elements. Stored in the DataStore table, SQLite.
    ''' </summary>
    <Serializable()>
    Public Class Synonym


        <NonSerialized>
        <XmlIgnore>
        <JsonIgnore>
        Public _ModelId As String = Nothing

        <XmlAttribute>
        <JsonProperty>
        Public Property ModelId As String
            Get
                If Me.BaseTermModelElement IsNot Nothing Then
                    Return Me.BaseTermModelElement.Model.ModelId
                Else
                    Return Me._ModelId
                End If
            End Get
            Set(value As String)
                Me._ModelId = value
            End Set
        End Property


        <NonSerialized>
        <XmlIgnore>
        <JsonIgnore>
        Public BaseTermModelElement As FBM.ModelObject = Nothing

        <NonSerialized>
        <XmlIgnore>
        <JsonIgnore>
        Public _BaseTerm As String = Nothing

        <XmlAttribute>
        <JsonProperty>
        Public Property BaseTerm As String
            Get
                If Me.BaseTermModelElement IsNot Nothing Then
                    Return Me.BaseTermModelElement.Id
                Else
                    Return Me._BaseTerm
                End If
            End Get
            Set(value As String)
                Me._BaseTerm = value
            End Set
        End Property

        <NonSerialized>
        <XmlIgnore>
        <JsonIgnore>
        Private _Synonym As String = Nothing

        <XmlAttribute>
        <JsonProperty>
        Public Property Synonym As String
            Get
                Return Me._Synonym
            End Get
            Set(value As String)
                Me._Synonym = value
            End Set
        End Property

        ''' <summary>
        ''' Parameterless Constructor
        ''' </summary>
        Public Sub New()
        End Sub

        Public Sub New(ByRef arBaseTermModelElement As FBM.ModelObject, ByVal asSynonym As String)

            Me.BaseTermModelElement = arBaseTermModelElement
            Me._BaseTerm = arBaseTermModelElement.Id
            Me.Synonym = asSynonym

        End Sub

    End Class

End Namespace
