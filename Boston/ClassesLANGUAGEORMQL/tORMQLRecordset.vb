Namespace ORMQL
    <Serializable()> _
    Public Class Recordset

        Public Facts As New List(Of FBM.Fact)
        Public Columns As New List(Of String)

        Public ErrorString As String = Nothing

        Public Query As String = Nothing

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
            End Get
            Set(ByVal value As FBM.FactData)
                Dim lrFactData As New FBM.FactData
                lrFactData = Me.CurrentFact.GetFactDataByRoleName(asItemValue)
                lrFactData = value
            End Set
        End Property

        Public Sub MoveFirst()
            Me.CurrentFactIndex = 0
        End Sub

        Public Sub MoveNext()
            If Me.CurrentFactIndex < Me.Facts.Count Then
                Me.CurrentFactIndex += 1
            Else
                '------------
                'Do nothing
                '------------
            End If
        End Sub

        Public Sub MoveLast()

            Me.CurrentFactIndex = Me.Facts.Count - 1
        End Sub

    End Class
End Namespace
