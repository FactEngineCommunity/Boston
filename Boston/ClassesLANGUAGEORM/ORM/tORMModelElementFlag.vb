Imports Newtonsoft.Json
Imports System.ComponentModel
Imports System.Xml.Serialization
Imports System.Reflection

Namespace FBM

    <Serializable()>
    Public Class ModelElementFlag
        Implements IEquatable(Of FBM.ModelElementFlag)

        <XmlIgnore>
        <JsonIgnore>
        <NonSerialized>
        Public Model As FBM.Model

        <XmlIgnore>
        <JsonIgnore>
        <NonSerialized>
        Private _ModelId As String = ""

        <XmlAttribute>
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
        <NonSerialized>
        Private _Concept As String = ""

        <XmlAttribute>
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
        <NonSerialized>
        <Browsable(False)>
        Public ModelElement As FBM.ModelObject

        <XmlAttribute>
        <JsonIgnore>
        <NonSerialized>
        Private _ModelElementFlagType As pcenumModelElementFlagType

        <XmlAttribute>
        Public Property ModelElementFlagType As pcenumModelElementFlagType  'E.g. Q6 Facet, Type, Level
            Get
                Return Me._ModelElementFlagType
            End Get
            Set(value As pcenumModelElementFlagType)
                Me._ModelElementFlagType = value
            End Set
        End Property

        <XmlIgnore>
        <JsonIgnore>
        <NonSerialized>
        Public _Value As Object = "" 'E.g. Q6 Abbreviation, Agent, Metadata

        <XmlElement>
        Public Property Value As Object
            Get
                Return Me._Value
            End Get
            Set(value As Object)
                Me._Value = value
            End Set
        End Property

        ''' <summary>
        ''' Parameterless Constructor
        ''' </summary>
        Public Sub New()
        End Sub

        Public Sub New(ByRef arModel As FBM.Model, ByRef arModelElement As FBM.ModelObject, ByVal aiModelElementFlagType As String, ByVal asValue As String)

            Try
                Me.Model = arModel
                Me.ModelElement = arModelElement
                Me.ModelElementFlagType = aiModelElementFlagType
                Me.Value = asValue

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
            End Try

        End Sub

        Public Shadows Function Equals(other As ModelElementFlag) As Boolean Implements IEquatable(Of ModelElementFlag).Equals

            Try
                Return Me.ModelId = other.ModelId And Me.Concept = other.Concept And Me.ModelElementFlagType = other.ModelElementFlagType

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
            End Try

        End Function

    End Class

End Namespace
