Imports System.Reflection
Imports Newtonsoft.Json
Imports System.ComponentModel
Imports System.Xml.Serialization

Namespace Interlink

    ''' <summary>
    ''' An Interlink enables a Model to target other Models (databases) for Querying, and API call functions.
    ''' </summary>
    <Serializable>
    Public Class Interlink

        <XmlIgnore>
        <JsonIgnore()>
        Private _ModelId As String = Nothing
        <XmlAttribute>
        Public Property ModelId As String
            Get
                If Me.Model IsNot Nothing Then
                    Return Me.Model.ModelId
                ElseIf Me._ModelId IsNot Nothing Then
                    Return Me._ModelId
                Else
                    Return "<Error: There is no Interlink Model>"
                End If
            End Get
            Set(value As String)
                Me._ModelId = value
            End Set
        End Property

        <XmlIgnore>
        <JsonIgnore()>
        Public Model As FBM.Model 'The Model that owns the Inerlink

        <XmlIgnore>
        <JsonIgnore()>
        Private _TargetModelId As String = Nothing
        <XmlAttribute>
        Public Property TargetModelId As String
            Get
                If Me.TargetModel IsNot Nothing Then
                    Return Me.TargetModel.ModelId
                ElseIf Me._TargetModelId IsNot Nothing Then
                    Return Me._TargetModelId
                ElseIf Me.TargetModel Is Nothing Then
                    Return "<Error: There is no Target Model>"
                Else
                    Return "<Error: There is no Target Model>"
                End If
            End Get
            Set(value As String)
                Me._TargetModelId = value
            End Set
        End Property

        <XmlIgnore>
        <JsonIgnore()>
        Public TargetModel As FBM.Model 'The Model targetted by the Model Element

        <XmlIgnore>
        <JsonIgnore()>
        Private _ModelElementId As String = Nothing
        <XmlAttribute>
        Public Property ModelElementId As String
            Get
                If Me.ModelElement IsNot Nothing Then
                    Return Me.ModelElement.Id
                ElseIf Me._ModelElementId IsNot Nothing Then
                    Return Me._ModelElementId
                Else
                    Return "<Error: Model Element is Nothing>"
                End If
            End Get
            Set(value As String)
                Me._ModelElementId = value
            End Set
        End Property

        <XmlIgnore>
        <JsonIgnore()>
        Public ModelElement As FBM.ModelObject 'The Model Element that is Interlinked with an Element within the TargetModel

        <XmlIgnore>
        <JsonIgnore()>
        Private _TargetModelElementId As String = Nothing
        <XmlAttribute>
        Public Property TargetModelElementId As String
            Get
                If Me.TargetModelElement IsNot Nothing Then
                    Return Me.TargetModelElement.Id
                ElseIf Me._TargetModelElementId IsNot Nothing Then
                    Return Me._TargetModelElementId
                ElseIf Me.TargetModelElement Is Nothing Then
                    Return "<Error: Model Element is Nothing>"
                Else
                    Return "<Error: Model Element is Nothing>"
                End If
            End Get
            Set(value As String)
                Me._TargetModelElementId = value
            End Set
        End Property

        <XmlIgnore>
        <JsonIgnore()>
        Public TargetModelElement As FBM.ModelObject 'The Target Model Elment within the Target Model that the Interlink is for.

        <XmlIgnore>
        <JsonIgnore()>
        Private _IsInError As Boolean = False
        <XmlIgnore>
        Public Property IsInError As Boolean
            Get
                Return Model Is Nothing Or
                    TargetModel Is Nothing Or
                    ModelElement Is Nothing Or
                    TargetModelElement Is Nothing
            End Get
            Set(value As Boolean)
                Me._IsInError = value
            End Set
        End Property

        <XmlIgnore>
        <JsonIgnore()>
        Private _IsPrimaryInterlink As Boolean = False
        <XmlAttribute>
        Public Property IsPrimaryInterlink As Boolean
            Get
                Return Me._IsPrimaryInterlink
            End Get
            Set(value As Boolean)
                Me._IsPrimaryInterlink = value
            End Set
        End Property

        ''' <summary>
        ''' Parameterless Constructor
        ''' </summary>
        Public Sub New()
        End Sub

        Public Sub New(ByRef arModelElement As FBM.ModelObject)

            Try
                Me.Model = arModelElement.Model
                Me.ModelElement = arModelElement

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