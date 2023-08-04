Imports System.Reflection
Imports Newtonsoft.Json
Imports System.ComponentModel

Namespace KnowledgeGraph
    Public Class ConceptClassificationValue

        <JsonIgnore>
        Public Model As FBM.Model

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

        <JsonIgnore>
        <Browsable(False)>
        Public ModelElement As FBM.ModelObject

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

        <JsonIgnore>
        Public _ClassificationValue As String = "" 'E.g. Q6 Abbreviation, Agent, Metadata
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
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
            End Try

        End Sub

    End Class

End Namespace
