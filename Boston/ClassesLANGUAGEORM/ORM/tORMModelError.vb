Imports System.Xml.Serialization

Namespace FBM
    <Serializable()> _
    Public Class ModelError
        Implements IEquatable(Of ModelError)
        'Implements ICloneable

        <XmlIgnore()> _
        <DebuggerBrowsable(DebuggerBrowsableState.Never)> _
        Public _error_id As String = ""
        Public Property ErrorId() As String
            Get
                Return _error_id
            End Get
            Set(ByVal Value As String)
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

        Public DictionaryEntry As FBM.DictionaryEntry
        Public ModelObject As FBM.ModelObject

        Public Sub New()
            '-------------------
            'Parameterless New
            '-------------------
        End Sub

        Public Sub New(ByVal asErrorId As String, _
                       ByRef arModelObject As FBM.ModelObject)

            Me.ErrorId = asErrorId
            Me.ModelObject = arModelObject

        End Sub

        Public Sub New(ByVal asErrorId As String, _
                       ByVal asErrorDescription As String, _
                       Optional ByRef arDictionaryEntry As FBM.DictionaryEntry = Nothing, _
                       Optional ByRef arModelObject As FBM.ModelObject = Nothing)

            Me._error_id = asErrorId
            Me._error_description = asErrorDescription

            If IsSomething(arDictionaryEntry) Then
                Me.DictionaryEntry = arDictionaryEntry
            End If

            If IsSomething(arModelObject) Then
                Me.ModelObject = arModelObject
            End If

        End Sub

        Public Shadows Function Equals(ByVal other As FBM.ModelError) As Boolean Implements System.IEquatable(Of FBM.ModelError).Equals

            If Me.Description = other.Description Then
                Return True
            Else
                Return False
            End If

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