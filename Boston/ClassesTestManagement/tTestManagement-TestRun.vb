Imports System.ComponentModel
Imports Newtonsoft.Json

Namespace TestManagement

    ''' <summary>
    ''' Class for running tests
    ''' </summary>
    Public Class TestRun
        Inherits Identifiable
        Implements INotifyPropertyChanged

        <JsonIgnore>
        <Browsable(False)>
        Public Overrides Property Name As String

        <JsonIgnore>
        <Browsable(False)>
        Public Overrides Property Description As String

        <JsonProperty>
        Public Property ProjectId As String

        <JsonProperty>
        Public Property TestPlanIdentifier As String

        <JsonProperty>
        Public Property TestSetIdentifier As String

        <JsonIgnore>
        Private _TestCycleIdentifier As String

        <JsonProperty>
        <Browsable(False)>
        <ForeignKeyReference(GetType(TestManagement.TestCycle), NameOf(TestManagement.TestCycle.Identifier), True)>
        Public Property TestCycleIdentifier As String
            Get
                Return Me._TestCycleIdentifier
            End Get
            Set(value As String)
                If value <> Me._TestCycleIdentifier Then
                    Me._TestCycleIdentifier = value
                    Me.OnPropertyChanged(NameOf(TestCycleIdentifier))
                End If
            End Set
        End Property

        <JsonIgnore>
        Private _TestCaseIdentifier As String

        <JsonProperty>
        <Browsable(False)>
        <ForeignKeyReference(GetType(TestManagement.TestCase), NameOf(TestManagement.TestCase.Identifier), True)>
        Public Property TestCaseIdentifier As String
            Get
                Return Me._TestCaseIdentifier
            End Get
            Set(value As String)
                If value <> Me._TestCaseIdentifier Then
                    Me._TestCaseIdentifier = value
                    Me.OnPropertyChanged(NameOf(TestCaseIdentifier))
                End If
            End Set
        End Property

        <JsonIgnore>
        Private _Verdict As String

        <JsonProperty>
        Public Property Verdict As String
            Get
                Return Me._Verdict
            End Get
            Set(value As String)
                If value <> Me._Verdict Then
                    Me._Verdict = value
                    Me.OnPropertyChanged(NameOf(Verdict))
                End If
            End Set
        End Property

        <JsonIgnore>
        Private _Notes As String

        <JsonProperty>
        Public Property Notes As String
            Get
                Return Me._Notes
            End Get
            Set(value As String)
                If value <> Me._Notes Then
                    Me._Notes = value
                    Me.OnPropertyChanged(NameOf(Notes))
                End If
            End Set
        End Property

        <JsonIgnore>
        Private _RunStartDateTime As DateTime?

        <JsonProperty>
        Public Property RunStartDateTime As DateTime?
            Get
                Return Me._RunStartDateTime
            End Get
            Set(value As DateTime?)
                If value <> Me._RunStartDateTime Or Me._RunStartDateTime Is Nothing Then
                    Me._RunStartDateTime = value
                    Me.OnPropertyChanged(NameOf(RunStartDateTime))
                End If
            End Set
        End Property

        <JsonIgnore>
        Private _RunCompleteDateTime As DateTime?

        <JsonProperty>
        Public Property RunCompleteDateTime As DateTime?
            Get
                Return Me._RunCompleteDateTime
            End Get
            Set(value As DateTime?)
                If value <> Me._RunCompleteDateTime Or Me._RunCompleteDateTime Is Nothing Then
                    Me._RunCompleteDateTime = value
                    Me.OnPropertyChanged(NameOf(RunCompleteDateTime))
                End If
            End Set
        End Property

        ''' <summary>
        ''' For when Test Run details cannot be editted in the GUI when ClosedComplete = True
        ''' </summary>
        ''' <returns></returns>
        <JsonProperty>
        Public Property ClosedComplete As Boolean

        <JsonIgnore>
        Public Property TestRunArtifact As New BindingList(Of TestManagement.TestRunArtifact)

        <JsonProperty>
        Public Property PerformedByUserId As String

        Public Sub New()
            MyBase.New()
        End Sub

        Public Shadows Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged

        Protected Overloads Sub OnPropertyChanged(ByVal asPropertyName As String)
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(asPropertyName))
        End Sub

    End Class

End Namespace