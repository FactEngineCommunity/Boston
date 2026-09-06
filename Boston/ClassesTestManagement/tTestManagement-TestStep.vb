Imports System.ComponentModel
Imports System.Xml.Serialization
Imports Newtonsoft.Json

Namespace TestManagement

    ' Class for steps within a test
    <Serializable>
    Public Class TestStep 'Inherits Identifiable
        Implements INotifyPropertyChanged

        <JsonIgnore>
        <XMLIgnore>
        <Browsable(False)>
        Public Property TestCase As TestCase

        <JsonIgnore>
        <XmlIgnore>
        Private _TestCaseIdentifier As String

        <JsonProperty>
        <XmlAttribute>
        <ForeignKeyReference(GetType(TestManagement.TestCase), NameOf(TestManagement.TestCase.Identifier), True)>
        <Browsable(False)>
        Public Property TestCaseIdentifier As String
            Get
                If Me.TestCase Is Nothing Then
                    Return Me._TestCaseIdentifier
                Else
                    Return Me.TestCase.Identifier
                End If
            End Get
            Set(value As String)
                Me._TestCaseIdentifier = value
            End Set
        End Property

        <JsonIgnore>
        <XmlIgnore>
        <Browsable(False)>
        Public Property TestSet As TestSet

        <JsonIgnore>
        <XmlIgnore>
        Private _TestSetIdentifier As String

        <NonSerialized>
        Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged

        <JsonProperty>
        <XmlIgnore>
        <Browsable(False)>
        Public Property TestSetIdentifier As String
            Get
                If Me.TestCase IsNot Nothing Then
                    Return Me.TestCase.TestSetIdentifier
                ElseIf Me.TestSet IsNot Nothing Then
                    Return Me.TestSet.Identifier
                Else
                    Return Me._TestSetIdentifier
                End If
            End Get
            Set(value As String)
                Me._TestSetIdentifier = value
            End Set
        End Property

        <JsonIgnore>
        <XmlIgnore>
        Private _TestStep As String = ""

        <JsonProperty>
        <XmlElement>
        Public Property TestStep As String
            Get
                Return Me._TestStep
            End Get
            Set(value As String)
                Me._TestStep = value
                RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(NameOf(TestStep)))
            End Set
        End Property

        <JsonIgnore>
        <XmlIgnore>
        Private _ExpectedResult As String = ""

        <JsonProperty>
        <XmlElement>
        Public Property ExpectedResult As String
            Get
                Return Me._ExpectedResult
            End Get
            Set(value As String)
                Me._ExpectedResult = value
                RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(NameOf(ExpectedResult)))
            End Set
        End Property

        ''' <summary>
        ''' Parameterless Constructor
        ''' </summary>
        Public Sub New()
            MyBase.New()
        End Sub

        Public Sub New(ByRef arTestCase As TestCase)

            Me.TestCase = arTestCase

        End Sub

    End Class

End Namespace