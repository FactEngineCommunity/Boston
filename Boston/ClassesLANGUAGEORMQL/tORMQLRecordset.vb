Imports System.Reflection

Namespace ORMQL

    <Serializable()>
    Public Class Recordset
        Implements IEnumerator

        Public _Facts As New List(Of FBM.Fact)

        Public Property Facts As List(Of FBM.Fact)
            Get
                Return Me._Facts
            End Get
            Set(value As List(Of FBM.Fact))
                Me._Facts = value
            End Set
        End Property


        Public ColumnNames As New List(Of String)
        Public Columns As New List(Of RDS.Column)

        ''' <summary>
        ''' True if an error was returned when creating the recordset. See ErrorString for error details.
        ''' </summary>
        ''' <returns></returns>
        Public ReadOnly Property ErrorReturned As Boolean
            Get
                Return Me.ErrorString IsNot Nothing
            End Get
        End Property

        Public ErrorString As String = Nothing

        ''' <summary>
        ''' The original FEQL query if one was used. Appended by FactEngine.
        ''' </summary>
        Public FEQLQuery As String = Nothing

        ''' <summary>
        ''' The Actual Query ran against the database.
        ''' </summary>
        Public Query As String = Nothing

        ''' <summary>
        ''' Query sent for processing, but that was translated to the Actual Query (Query member), to run Query against the database.
        ''' </summary>
        Public IntermediateQuery As String = Nothing

        ''' <summary>
        ''' If a Natural Language Query is converted to FactEngine Query Language query. Appended by FactEngine.
        ''' </summary>
        Public NaturalLanguageQuery As String = Nothing

        Public Warning As New List(Of String) 'For if there are any Warnings/Recommendations in a QueryGraph in FactEngine.

        ''' <summary>
        ''' The type of statement made by the User. E.g. a DESCRIBEStatement.
        ''' </summary>
        Public StatementType As FactEngine.pcenumFEQLStatementType = FactEngine.Constants.pcenumFEQLStatementType.None

        Public QueryGraph As FactEngine.QueryGraph

        ''' <summary>
        ''' As used in a DESCRIBE Statement, is pushed back to the client to process.
        ''' </summary>
        Public ModelElement As FBM.ModelObject

        Private _CurrentFact As FBM.Fact
        Public Property CurrentFact() As FBM.Fact
            Get
                If Me.Facts.Count > 0 Then
                    If Me.CurrentFactIndex < 0 Then Return Nothing
                    Return Me.Facts(Me.CurrentFactIndex)
                Else
                    Return Nothing
                End If
            End Get
            Set(ByVal value As FBM.Fact)
                Me._CurrentFact = value
            End Set
        End Property

        Public CurrentFactIndex As Integer = 0

        Private _EOF As Boolean
        Public Property EOF() As Boolean
            Get
                If (Me.CurrentFactIndex < Me.Facts.Count) And Me.Facts.Count > 0 Then
                    Return False
                Else
                    Return True
                End If
            End Get
            Set(ByVal value As Boolean)
                Me._EOF = value
            End Set
        End Property

        Public NumberOfRowsUpdated As Integer = 0

        Public [ApplicationException] As ApplicationException = Nothing

        ''' <summary>
        ''' Parameterless New
        ''' </summary>
        Public Sub New()
        End Sub

        ''' <summary>
        ''' Constructor
        ''' </summary>
        ''' <param name="aiStatementType">The type of Statement raised by the User.</param>
        Public Sub New(ByVal aiStatementType As FactEngine.pcenumFEQLStatementType)
            Me.StatementType = aiStatementType
        End Sub


        Default Public Property Item(ByVal asItemValue As String) As FBM.FactData
            Get
                Try
                    Me.CurrentFact = Me.Facts(Me.CurrentFactIndex)

                    If asItemValue.IsNumeric Then
                        Select Case Me.CurrentFact.GetType.ToString
                            Case Is = GetType(FBM.Fact).ToString
                                Return Me.CurrentFact.Data(CInt(asItemValue))
                            Case Is = GetType(FBM.FactInstance).ToString
                                Dim lrFactInstance As New FBM.FactInstance
                                lrFactInstance = Me.CurrentFact
                                Return lrFactInstance.Data(CInt(asItemValue))
                            Case Else
                                Return Nothing
                        End Select
                    Else
                        Select Case Me.CurrentFact.GetType.ToString
                            Case Is = GetType(FBM.Fact).ToString
                                Return Me.CurrentFact.GetFactDataByRoleName(asItemValue)
                            Case Is = GetType(FBM.FactInstance).ToString
                                Dim lrFactInstance As New FBM.FactInstance
                                lrFactInstance = Me.CurrentFact
                                Return lrFactInstance.GetFactDataInstanceByRoleName(asItemValue)
                            Case Else
                                Return Nothing
                        End Select
                    End If

                Catch ex As Exception
                    'Effectively a NullValue. NB If there are no Facts against a FactType, this does not mean the FactType does not exist in the Model.
                    'It merely means that the effective value for the FactType is nothing. Nothing is "" in Boston.
                    Dim lrFactData = New FBM.FactData(New FBM.Concept("", False))
                    Return lrFactData
                End Try
            End Get
            Set(ByVal value As FBM.FactData)
                Dim lrFactData As New FBM.FactData
                lrFactData = Me.CurrentFact.GetFactDataByRoleName(asItemValue)
                lrFactData = value
            End Set
        End Property

        Public ReadOnly Property Current As Object Implements IEnumerator.Current
            Get
                Return Me.CurrentFact
            End Get
        End Property

        Public Sub MoveFirst()
            Me.CurrentFactIndex = 0
        End Sub

        Public Function MoveNext() As Boolean Implements IEnumerator.MoveNext
            If Me.CurrentFactIndex < Me.Facts.Count Then
                Me.CurrentFactIndex += 1
            Else
                '------------
                'Do nothing
                '------------
                Return False
            End If
            Return True
        End Function

        Public Sub MoveLast()

            Me.CurrentFactIndex = Me.Facts.Count - 1
        End Sub

        Public Sub Reset() Implements IEnumerator.Reset

            Select Case Me.Facts.Count
                Case Is = 0
                    Me.CurrentFactIndex = -1
                Case Else
                    Me.CurrentFactIndex = 0
            End Select

        End Sub

        Public Function generateCSVText() As String

            Dim lsCSVText As String = ""
            Dim liInd = 0

            Try
                For Each lrFact In Me.Facts

                    liInd = 0
                    For Each lrFactData In lrFact.Data
                        If liInd > 0 Then lsCSVText &= ","
                        lsCSVText &= lrFactData.Data
                        liInd += 1
                    Next
                    lsCSVText &= vbCrLf
                Next

                Return lsCSVText

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)

                Return ""
            End Try

        End Function

    End Class

End Namespace
