Imports System.Reflection

Namespace FEQL
    Public Class DESCRIBEStatement

        Private _MODELELEMENTNAME As String = Nothing
        Public Property MODELELEMENTNAME As String
            Get
                Return Me._MODELELEMENTNAME
            End Get
            Set(value As String)
                Me._MODELELEMENTNAME = value
            End Set
        End Property

    End Class

    Public Class ENUMERATEStatement

        Private _MODELELEMENTNAME As String = Nothing
        Public Property MODELELEMENTNAME As String
            Get
                Return Me._MODELELEMENTNAME
            End Get
            Set(value As String)
                Me._MODELELEMENTNAME = value
            End Set
        End Property

    End Class

    Public Class WHICHSELECTStatement

        Private _MODELELEMENTNAME As New List(Of String)
        Public Property MODELELEMENTNAME As List(Of String)
            Get
                Return Me._MODELELEMENTNAME
            End Get
            Set(value As List(Of String))
                Me._MODELELEMENTNAME = value
            End Set
        End Property

        Private _WHICHCLAUSE As New List(Of Object)
        Public Property WHICHCLAUSE As List(Of Object)
            Get
                Return Me._WHICHCLAUSE
            End Get
            Set(value As List(Of Object))
                Me._WHICHCLAUSE = value
            End Set
        End Property

    End Class

    Public Class WHICHCLAUSE

        Private _KEYWDIS As String = Nothing
        Public Property KEYWDIS As String
            Get
                Return Me._KEYWDIS
            End Get
            Set(value As String)
                Me._KEYWDIS = value
            End Set
        End Property

        Private _KEYWDA As String = Nothing
        Public Property KEYWDA As String
            Get
                Return Me._KEYWDA
            End Get
            Set(value As String)
                Me._KEYWDA = value
            End Set
        End Property

        Private _KEYWDAND As String = Nothing
        Public Property KEYWDAND As String
            Get
                Return Me._KEYWDAND
            End Get
            Set(value As String)
                Me._KEYWDAND = value
            End Set
        End Property

        Private _WHICHTHATCLAUSE As Object = Nothing 'NB Is used to disambiguate where the THAT is in the WHICHCLAUSE
        Public Property WHICHTHATCLAUSE As Object
            Get
                Return Me._WHICHTHATCLAUSE
            End Get
            Set(value As Object)
                Me._WHICHTHATCLAUSE = value
            End Set
        End Property

        Private _KEYWDTHAT As New List(Of String)
        Public Property KEYWDTHAT As List(Of String)
            Get
                Return Me._KEYWDTHAT
            End Get
            Set(value As List(Of String))
                Me._KEYWDTHAT = value
            End Set
        End Property

        Private _PREDICATE As New List(Of String)
        Public Property PREDICATE As List(Of String)
            Get
                Return Me._PREDICATE
            End Get
            Set(value As List(Of String))
                Me._PREDICATE = value
            End Set
        End Property

        Private _KEYWDWHICH As String = Nothing
        Public Property KEYWDWHICH As String
            Get
                Return Me._KEYWDWHICH
            End Get
            Set(value As String)
                Me._KEYWDWHICH = value
            End Set
        End Property

        Private _MODELELEMENTNAME As New List(Of String)
        Public Property MODELELEMENTNAME As List(Of String)
            Get
                Return Me._MODELELEMENTNAME
            End Get
            Set(value As List(Of String))
                Me._MODELELEMENTNAME = value
            End Set
        End Property

        Private _NODEPROPERTYIDENTIFICATION As Object = Nothing
        Public Property NODEPROPERTYIDENTIFICATION As Object
            Get
                Return Me._NODEPROPERTYIDENTIFICATION
            End Get
            Set(value As Object)
                Me._NODEPROPERTYIDENTIFICATION = value
            End Set
        End Property

    End Class

    Public Class NODEPROPERTYIDENTIFICATION

        Private _MODELELEMENTNAME As String
        Public Property MODELELEMENTNAME As String
            Get
                Return Me._MODELELEMENTNAME
            End Get
            Set(value As String)
                Me._MODELELEMENTNAME = value
            End Set
        End Property

        Private _IDENTIFIER As New List(Of String)
        Public Property IDENTIFIER As List(Of String)
            Get
                Return Me._IDENTIFIER
            End Get
            Set(value As List(Of String))
                Me._IDENTIFIER = value
            End Set
        End Property

    End Class

    Public Class Processor

        Public Model As FBM.Model

        Private Parser As New FEQL.Parser(New FEQL.Scanner) 'Used to parse Text input into the Brain; especially for ORMQL.
        Private Parsetree As New FEQL.ParseTree 'Used with the Parser, is populated during the parsing of text input into the Brain; especially ORMQL

        Public DESCRIBEStatement As New FEQL.DESCRIBEStatement
        Public ENUMMERATEStatement As New FEQL.ENUMERATEStatement
        Public WHICHSELECTStatement As New FEQL.WHICHSELECTStatement
        Public WHICHCLAUSE As New FEQL.WHICHCLAUSE
        Public NODEPROPERTYIDENTIFICATION As New FEQL.NODEPROPERTYIDENTIFICATION

        ''' <summary>
        ''' Parameterless NEw
        ''' </summary>
        Public Sub New()
        End Sub

        Public Sub New(ByRef arModel As FBM.Model)
            Me.Model = arModel
        End Sub

        Public Sub GetParseTreeTokensReflection(ByRef ao_object As Object, ByRef aoParseTreeNode As FEQL.ParseNode)

            '-------------------------------
            'NB IMPORTANT: The TokenTypes in the ParseTree must be the same as established in aoObject (as hardcoded here in Richmond).
            'i.e. Richmond (the software) and the Parser (to the outside word) are linked.
            'If the Tokens in the Parser do not match the HardCoded tokens then Richmond will not be able to retrieve the tokens
            ' from the ParseText input from the user.
            'This isn't the falt of the user, this is a fault of using the ParserGenerator (TinyPG in Richmond's case) to set-up the Tokens.
            'i.e. The person setting up the Parser in TinyPG need be aware that 'Tokens' in TinyPG (when defining the ORMQL) need be the same
            ' as the Tokens in Richmond and as Richmond expects.
            'i.e. Establishing a Parser in TinyPG is actually a form of 'coding' (hard-coding), and where Richmond ostensibly has 2 parsers,
            '  VB.Net and TinyPG.
            '---------------------
            'Parameters
            'ao_object is of runtime generated type DynamicCollection.Entity
            '----------------------------------------------------------------------

            Dim loPropertyInfo() As System.Reflection.PropertyInfo = ao_object.GetType().GetProperties
            Dim loProperty As PropertyInfo
            Dim lr_bag As New Stack
            Dim loParseTreeNode As FEQL.ParseNode
            Dim lrType As Type
            Try
                '--------------------------------------
                'Retrieve the list of required Tokens
                '--------------------------------------
                For Each loProperty In loPropertyInfo
                    lr_bag.Push(loProperty.Name)
                Next

                loParseTreeNode = aoParseTreeNode

                Dim lasListOfString As New List(Of String)

                If lr_bag.Contains(aoParseTreeNode.Token.Type.ToString) Then
                    'lrType = ao_object.GetAttributeMember(aoParseTreeNode.Token.Type.ToString).GetType
                    lrType = ao_object.GetType.GetProperty(aoParseTreeNode.Token.Type.ToString).PropertyType
                    Dim piInstance As PropertyInfo = ao_object.GetType.GetProperty(aoParseTreeNode.Token.Type.ToString)

                    If lrType Is GetType(String) Then
                        'ao_object.SetAttributeMember(aoParseTreeNode.Token.Type.ToString, aoParseTreeNode.Token.Text)
                        piInstance.SetValue(ao_object, aoParseTreeNode.Token.Text)

                    ElseIf lrType Is lasListOfString.GetType Then
                        'ao_object.GetAttributeMember(aoParseTreeNode.Token.Type.ToString).Add(aoParseTreeNode.Token.Text)

                        Dim liInstance As Object = ao_object.GetType.GetProperty(aoParseTreeNode.Token.Type.ToString).GetValue(ao_object)
                        'Dim instance As Object = Activator.CreateInstance(ao_object.GetType.GetProperty(aoParseTreeNode.Token.Type.ToString).PropertyType)
                        Dim list As IList = CType(liInstance, IList)
                        list.Add(aoParseTreeNode.Token.Text)
                        ao_object.GetType.GetProperty(aoParseTreeNode.Token.Type.ToString).SetValue(ao_object, list, Nothing)

                        '==================================
                        'piInstance.SetValue(ao_object, aoParseTreeNode.Token.Text)
                        'ElseIf lrType Is GetType(List(Of String)) Then
                        '    'ao_object.GetAttributeMember(aoParseTreeNode.Token.Type.ToString).Add(aoParseTreeNode)
                        '    piInstance.SetValue(ao_object, aoParseTreeNode.Token.Text)
                    ElseIf lrType Is GetType(List(Of Object)) Then

                        Dim liInstance As Object = ao_object.GetType.GetProperty(aoParseTreeNode.Token.Type.ToString).GetValue(ao_object)
                        'Dim instance As Object = Activator.CreateInstance(ao_object.GetType.GetProperty(aoParseTreeNode.Token.Type.ToString).PropertyType)
                        Dim list As IList = CType(liInstance, IList)
                        list.Add(aoParseTreeNode)
                        ao_object.GetType.GetProperty(aoParseTreeNode.Token.Type.ToString).SetValue(ao_object, list, Nothing)

                        'ao_object.GetAttributeMember(aoParseTreeNode.Token.Type.ToString).Add(aoParseTreeNode)
                        'piInstance.SetValue(ao_object, aoParseTreeNode.Token.Text)
                    ElseIf lrType Is GetType(Object) Then
                        'ao_object.SetAttributeMember(aoParseTreeNode.Token.Type.ToString, aoParseTreeNode)
                        piInstance.SetValue(ao_object, aoParseTreeNode)
                    End If
                End If

                For Each loParseTreeNode In aoParseTreeNode.Nodes.ToArray
                    Call GetParseTreeTokensReflection(ao_object, loParseTreeNode)
                Next

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()
                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Public Function ProcessFEQLStatement(ByVal asFEQLStatement As String) As ORMQL.Recordset

            Dim lrFact As New FBM.Fact

            Try
                '---------------------------
                'Parse the ORMQR statement
                '---------------------------
                SyncLock Me.Parsetree
                    Me.Parsetree = Me.Parser.Parse(asFEQLStatement)

                    Select Case Me.Parsetree.Nodes(0).Nodes(0).Text
                        Case Is = "WHICHSELECTSTMT"

                            '=============================================================
                            Return Me.ProcessWHICHSELECTStatement(asFEQLStatement)

                            '----------------------------------------------------------------------------------
                            'Exit the sub because have found what the User was trying to do, and have done it 
                            '----------------------------------------------------------------------------------
                            Exit Function

                        Case Is = "ENUMERATESTMT"

                            '=============================================================
                            Return Me.ProcessENUMERATEStatement(asFEQLStatement)

                        Case Is = "DESCRIBESTMT"

                            '=============================================================
                            Return Me.ProcessDESCRIBEStatement(asFEQLStatement)

                        Case Is = "DELETEFACTSTMT"
                            '20200727-VM-Just here as an example.
                            'Return Me.ProcessDELETEFACTSTMTStatement

                        Case Else
                            Dim lrRecordset As New ORMQL.Recordset
                            lrRecordset.ErrorString = "Unknown Query/Command"
                            Return lrRecordset

                    End Select

                End SyncLock

            Catch ex As Exception
                'Me.parsetree.Errors.Add(New TinyPG.ParseError("Error: tModel.ProcessORMQLStatement: " & ex.Message, 100, lrCustomClass))
                'Return Me.parsetree.Errors
                Dim lrRecordset As New ORMQL.Recordset
                If IsSomething(ex.InnerException) Then
                    lrRecordset.ErrorString = ex.Message & vbCrLf & "Inner Exception" & vbCrLf & ex.InnerException.Message & vbCrLf & vbCrLf & "Stack Trace" & vbCrLf & ex.StackTrace
                    Return lrRecordset
                Else
                    lrRecordset.ErrorString = ex.Message & vbCrLf & "Inner Exception" & vbCrLf & vbCrLf & "Stack Trace" & vbCrLf & ex.StackTrace
                    Return lrRecordset
                End If

            End Try

        End Function

        Public Function getWhichStatementType(ByVal asFEQLStatement As String,
                                              Optional ByVal abParse As Boolean = False) As FactEngine.Constants.pcenumFEQLStatementType

            Try
                SyncLock Me.Parsetree
                    If abParse Then
                        Me.Parsetree = Me.Parser.Parse(asFEQLStatement)
                    End If

                    If Me.Parsetree Is Nothing Then
                        Return FactEngine.pcenumFEQLStatementType.None
                    Else
                        If Me.Parsetree.Nodes.Count = 0 Then Return FactEngine.Constants.pcenumFEQLStatementType.None
                    End If

                    Select Case Me.Parsetree.Nodes(0).Nodes(0).Token.Type
                        Case Is = FEQL.TokenType.WHICHSELECTSTMT   '"WHICHSELECTSTMT"
                            Return FactEngine.pcenumFEQLStatementType.WHICHSELECTStatement
                        Case Is = FEQL.TokenType.ENUMERATESTMT  '"ENUMERATESTMT"
                            Return FactEngine.pcenumFEQLStatementType.ENUMERATEStatement
                        Case Is = FEQL.TokenType.DESCRIBESTMT
                            Return FactEngine.pcenumFEQLStatementType.DESCRIBEStatement
                    End Select
                End SyncLock
            Catch ex As Exception

            End Try

        End Function

    End Class

End Namespace