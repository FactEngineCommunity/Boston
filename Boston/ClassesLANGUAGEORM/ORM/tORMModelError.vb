Imports System.Xml.Serialization

Namespace FBM
    <Serializable()> _
    Public Class ModelError
        Implements IEquatable(Of ModelError)
        'Implements ICloneable

        <XmlIgnore()> _
        <DebuggerBrowsable(DebuggerBrowsableState.Never)> _
        Public _error_id As String = ""
        Public Property ErrorId() As pcenumModelErrors
            Get
                Return _error_id
            End Get
            Set(ByVal Value As pcenumModelErrors)
                _error_id = Value
            End Set
        End Property

        <XmlIgnore()> _
        <DebuggerBrowsable(DebuggerBrowsableState.Never)> _
        Public _error_description As String = ""
        Public Property Description() As String
            Get
                Return _error_description
            End Get
            Set(ByVal Value As String)
                _error_description = Value
            End Set
        End Property

        <XmlIgnore()>
        <DebuggerBrowsable(DebuggerBrowsableState.Never)>
        Public _SubError_id As pcenumModelSubErrorType = pcenumModelSubErrorType.None
        Public Property SubErrorId() As pcenumModelSubErrorType
            Get
                Return Me._SubError_id
            End Get
            Set(value As pcenumModelSubErrorType)
                Me._SubError_id = value
            End Set
        End Property

        Public DictionaryEntry As FBM.DictionaryEntry
        Public ModelObject As FBM.ModelObject
        Public CMMLModelElement As Object = Nothing

        Public Sub New()
            '-------------------
            'Parameterless New
            '-------------------
        End Sub

        Public Sub New(ByVal asErrorId As pcenumModelErrors,
                       ByRef arModelObject As FBM.ModelObject)

            Me.ErrorId = asErrorId
            Me.ModelObject = arModelObject

        End Sub

        Public Sub New(ByVal asErrorId As String,
                       ByVal asErrorDescription As String,
                       Optional ByRef arDictionaryEntry As FBM.DictionaryEntry = Nothing,
                       Optional ByRef arModelObject As FBM.ModelObject = Nothing,
                       Optional ByVal abAddToModelElementAndModel As Boolean = False,
                       Optional ByVal aiSubErrorId As pcenumModelSubErrorType = pcenumModelSubErrorType.None,
                       Optional ByRef arCMMLModelElement As Object = Nothing)

            Me._error_id = asErrorId
            Me._error_description = asErrorDescription
            Me._SubError_id = aiSubErrorId
            Me.CMMLModelElement = arCMMLModelElement

            '20220530-VM-Eventually can get rid of Me._error_description = asErrorDescription, above. Try/Catch for now
            Try
                Select Case asErrorId
                    Case Is = "105"
                        Me._error_description = "Entity Type Requires Reference Scheme Error - "
                        Me._error_description &= "Entity Type: '" & arModelObject.Id & "'."
                End Select
            Catch
            End Try

            If IsSomething(arDictionaryEntry) Then
                Me.DictionaryEntry = arDictionaryEntry
            End If

            If IsSomething(arModelObject) Then
                Me.ModelObject = arModelObject
            End If

            If abAddToModelElementAndModel And arModelObject IsNot Nothing Then
                arModelObject._ModelError.AddUnique(Me)
                arModelObject.Model._ModelError.AddUnique(Me)
            End If

        End Sub

        Public Shadows Function Equals(ByVal other As FBM.ModelError) As Boolean Implements System.IEquatable(Of FBM.ModelError).Equals

            If Me.Description = other.Description Then
                Return True
            Else
                Return False
            End If

        End Function

        Public Shadows Function EqualsByErrorIdModelElementId(ByVal other As FBM.ModelError) As Boolean

            Return Me.ErrorId = other.ErrorId And Me.ModelObject.Id = other.ModelObject.Id

        End Function

        Public Function Clone() As Object 'Implements System.ICloneable.Clone

            Dim lrModelerror As New FBM.ModelError

            With Me
                lrModelerror.Description = .Description
                lrModelerror.ErrorId = .ErrorId
            End With

            Return lrModelerror

        End Function

    End Class
End Namespace