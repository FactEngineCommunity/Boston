Imports System
Imports System.Collections.Generic
Imports System.Xml.Serialization
Imports Newtonsoft.Json

Namespace FBM

    '==========================================================
    '  core:Functions
    '==========================================================
    ''' <summary>
    ''' Container for NORMA's function catalog (core:Functions).
    '''
    ''' NORMA stores built-in and user-defined functions/operators here.
    ''' Conditions and CalculatedValues may refer to these by id.
    '''
    ''' Boston usage:
    '''   - During first pass, functions may be ignored if you only need derivation text.
    '''   - During a later pass, functions can be mapped to Boston's own expression tree,
    '''     enabling richer rendering and validation.
    '''
    ''' Examples:
    '''   - StringLength(x)
    '''   - Abs(x)
    '''   - OperatorSymbol "!=" or "&lt;&gt;" (inequality)
    '''   
    ''' Storage:
    '''   - Stored in the DataStore table for the Model, and within the .fbm XML for models stored as XML.
    ''' </summary>
    <Serializable>
    <XmlRoot("Function", Namespace:="orm")>
    Public Class [Function]

        <XmlAttribute>
        <JsonProperty>
        Public Property id As String = System.Guid.NewGuid.ToString

        ''' <summary>
        ''' The FBM.Model that the Function belongs to.
        ''' </summary>
        <XmlIgnore>
        <JsonIgnore>
        Public Model As FBM.Model = Nothing

        <XmlIgnore>
        <JsonIgnore>
        Private _ModelId As String = ""

        <XmlAttribute("ModelId")>
        <JsonProperty>
        Public Property ModelId As String
            Get
                If Me.Model Is Nothing Then
                    Return Me._ModelId
                Else
                    Return Me.Model.ModelId
                End If
            End Get
            Set(value As String)
                Me._ModelId = value
            End Set
        End Property

        ''' <summary>
        ''' E.g. count, max, min, concat, subtract, add, etc.
        ''' </summary>
        ''' <returns></returns>
        <XmlAttribute("Name")>
        <JsonProperty>
        Public Property Name As String = Nothing

        ''' <summary>
        ''' E.g. "=", "&lt;&gt;"
        ''' </summary>
        ''' <returns></returns>
        <XmlAttribute("OperatorSymbol")>
        <JsonProperty>
        Public Property OperatorSymbol As String

        ''' <summary>
        ''' True if Function result is boolean.
        ''' </summary>
        <XmlAttribute>
        <JsonProperty>
        Public Property IsBoolean As Boolean = False

        ''' <summary>
        ''' NB count, max, min, have one Paramter where BagInput = True.
        ''' </summary>
        <XmlArray("Parameters")>
        <XmlArrayItem("Parameter")>
        <JsonProperty>
        Public Property Parameters As New List(Of Parameter)

        ''' <summary>
        ''' Parameterless Constructor
        ''' </summary>
        Public Sub New()
        End Sub

        Public Sub New(ByRef arModel As FBM.Model)
            Me.Model = arModel
        End Sub

    End Class

    <Serializable>
    <XmlRoot("Parameter", Namespace:="orm")>
    Public Class Parameter

        <XmlAttribute>
        <JsonProperty>
        Public Property id As String = System.Guid.NewGuid.ToString

        ''' <summary>
        ''' E.g. left, right, values, etc.
        ''' </summary>
        ''' <returns></returns>
        <XmlAttribute("Name")>
        <JsonProperty>
        Public Property Name As String = Nothing

        ''' <summary>
        ''' NB Functions count, max, min, have one Paramter where BagInput = True.
        ''' </summary>
        <XmlAttribute>
        <JsonProperty>
        Public Property BagInput As Boolean = False

        ''' <summary>
        ''' Parameterless Constructor
        ''' </summary>
        Public Sub New()
        End Sub

    End Class

End Namespace