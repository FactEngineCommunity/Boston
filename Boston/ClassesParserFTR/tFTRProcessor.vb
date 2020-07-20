Imports System.Reflection

Namespace FTR


    Public Class FactTypeReadingStatement

        Private _FRONTREADINGTEXT As List(Of String)
        Public Property FRONTREADINGTEXT As List(Of String)
            Get
                Return Me._FRONTREADINGTEXT
            End Get
            Set(value As List(Of String))
                Me._FRONTREADINGTEXT = value
            End Set
        End Property

        Private _MODELELEMENT As List(Of Object)
        Public Property MODELELEMENT As List(Of Object)
            Get
                Return Me._MODELELEMENT
            End Get
            Set(value As List(Of Object))
                Me._MODELELEMENT = value
            End Set
        End Property

        Private _PREDICATECLAUSE As List(Of Object)
        Public Property PREDICATECLAUSE As List(Of Object)
            Get
                Return Me._PREDICATECLAUSE
            End Get
            Set(value As List(Of Object))
                Me._PREDICATECLAUSE = value
            End Set
        End Property

        Private _UNARYPREDICATEPART As String
        Public Property UNARYPREDICATEPART As String
            Get
                Return Me._UNARYPREDICATEPART
            End Get
            Set(value As String)
                Me._UNARYPREDICATEPART = value
            End Set
        End Property

        Private _FOLLOWINGREADINGTEXT As String
        Public Property FOLLOWINGREADINGTEXT As String
            Get
                Return Me._FOLLOWINGREADINGTEXT
            End Get
            Set(value As String)
                Me._FOLLOWINGREADINGTEXT = value
            End Set
        End Property

    End Class

    Public Class ModelElementClause

        Private _PREBOUNDREADINGTEXT As String
        Public Property PREBOUNDREADINGTEXT As String
            Get
                Return Me._PREBOUNDREADINGTEXT
            End Get
            Set(value As String)
                Me._PREBOUNDREADINGTEXT = value
            End Set
        End Property

        Private _MODELELEMENTNAME As String
        Public Property MODELELEMENTNAME As String
            Get
                Return Me._MODELELEMENTNAME
            End Get
            Set(value As String)
                Me._MODELELEMENTNAME = value
            End Set
        End Property

        Private _POSTBOUNDREADINGTEXT As String
        Public Property POSTBOUNDREADINGTEXT As String
            Get
                Return Me._POSTBOUNDREADINGTEXT
            End Get
            Set(value As String)
                Me._POSTBOUNDREADINGTEXT = value
            End Set
        End Property
    End Class

    Public Class PredicatePartClause

        Private _PREDICATEPART As List(Of String)
        Public Property PREDICATEPART As List(Of String)
            Get
                Return Me._PREDICATEPART
            End Get
            Set(value As List(Of String))
                Me._PREDICATEPART = value
            End Set
        End Property
    End Class

    Public Class Processor

        Private Model As FBM.Model

        Private Parser As New FTR.Parser(New FTR.Scanner) 'Used to parse Text input into the Brain; especially for ORMQL.
        Private Parsetree As New FTR.ParseTree 'Used with the Parser, is populated during the parsing of text input into the Brain; especially ORMQL

        Public FACTTYPEREADINGStatement As New FTR.FactTypeReadingStatement
        Public MODELELEMENTClause As New FTR.ModelElementClause
        Public PREDICATEPARTClause As New FTR.PredicatePartClause

        Public Sub New()

            '====================================================================
            'Create the DynamicObject for FACTTYPEREADING Texts/Satements
            '====================================================================
            'Dim lrFactTypeReadingStatement As New DynamicClassLibrary.Factory.tClass
            'lrFactTypeReadingStatement.add_attribute(New DynamicClassLibrary.Factory.tAttribute("FRONTREADINGTEXT", GetType(List(Of String))))
            'lrFactTypeReadingStatement.add_attribute(New DynamicClassLibrary.Factory.tAttribute("MODELELEMENT", GetType(List(Of Object))))
            'lrFactTypeReadingStatement.add_attribute(New DynamicClassLibrary.Factory.tAttribute("PREDICATECLAUSE", GetType(List(Of Object))))            
            'lrFactTypeReadingStatement.add_attribute(New DynamicClassLibrary.Factory.tAttribute("UNARYPREDICATEPART", GetType(String)))
            'lrFactTypeReadingStatement.add_attribute(New DynamicClassLibrary.Factory.tAttribute("FOLLOWINGREADINGTEXT", GetType(String)))

            'Me.FACTTYPEREADINGStatement = lrFactTypeReadingStatement.clone

            '====================================================================
            'Create the DynamicObject for MODELELEMENT Clauses
            '====================================================================
            'Dim lrModelElementClause As New DynamicClassLibrary.Factory.tClass
            'lrModelElementClause.add_attribute(New DynamicClassLibrary.Factory.tAttribute("PREBOUNDREADINGTEXT", GetType(String)))
            'lrModelElementClause.add_attribute(New DynamicClassLibrary.Factory.tAttribute("MODELELEMENTNAME", GetType(String)))
            'lrModelElementClause.add_attribute(New DynamicClassLibrary.Factory.tAttribute("POSTBOUNDREADINGTEXT", GetType(String)))

            'Me.MODELELEMENTClause = lrModelElementClause.clone

            '====================================================================
            'Create the DynamicObject for PREDICATEPART Clauses
            '====================================================================
            'Dim lrPredicatePartClause As New DynamicClassLibrary.Factory.tClass
            'lrPredicatePartClause.add_attribute(New DynamicClassLibrary.Factory.tAttribute("PREDICATEPART", GetType(List(Of String))))

            'Me.PREDICATEPARTClause = lrPredicatePartClause.clone

        End Sub

        Public Sub New(ByRef arModel As FBM.Model)

            Call Me.New()
            Me.Model = arModel

        End Sub

        ''' <summary>        
        '''ORMQL Mode. Gets the tokens from the Parse Tree.
        '''Walks the ParseTree and finds the tokens as per the Properties/Fields of the ao_object passed to the procedure.
        '''  i.e. Based on the type of token at the Root of the ParseTree, the software dynamically creates ao_object such that 
        '''  it contains the tokens that it wants returned.
        ''' </summary>
        ''' <param name="ao_object">is of runtime generated type DynamicCollection.Entity</param>
        ''' <param name="aoParseTreeNode">ParseNode as from VAQL Parser</param>
        ''' <remarks></remarks>
        Public Sub GetParseTreeTokens(ByRef ao_object As Object, ByRef aoParseTreeNode As FTR.ParseNode)

            '-------------------------------
            'NB IMPORTANT: The TokenTypes in the ParseTree must be the same as established in aoObject (as hardcoded here in Richmond).
            'i.e. Richmond (the software) and the Parser (to the outside word) are linked.
            'If the Tokens in the Parser do not match the HardCoded tokens then Richmond will not be able to retrieve the tokens
            ' from the ParseText input from the user.
            'This isn't the falt of the user, this is a fault of using the ParserGenerator (VAQL in Richmond's case) to set-up the Tokens.
            'i.e. The person setting up the Parser in VAQL need be aware that 'Tokens' in VAQL (when defining the ORMQL) need be the same
            ' as the Tokens in Richmond and as Richmond expects.
            'i.e. Establishing a Parser in VAQL is actually a form of 'coding' (hard-coding), and where Richmond ostensibly has 2 parsers,
            '  VB.Net and VAQL.
            '---------------------
            'Parameters
            'ao_object is of runtime generated type DynamicCollection.Entity
            '----------------------------------------------------------------------

            Dim loPropertyInfo() As System.Reflection.PropertyInfo = ao_object.GetType().GetProperties
            Dim loProperty As PropertyInfo
            Dim lr_bag As New Stack
            Dim loParseTreeNode As FTR.ParseNode

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
                    Dim lrType As Type
                    lrType = ao_object.GetAttributeMember(aoParseTreeNode.Token.Type.ToString).GetType
                    If lrType Is lasListOfString.GetType Then
                        ao_object.GetAttributeMember(aoParseTreeNode.Token.Type.ToString).Add(aoParseTreeNode.Token.Text)
                    ElseIf lrType Is GetType(List(Of Object)) Then
                        ao_object.GetAttributeMember(aoParseTreeNode.Token.Type.ToString).Add(aoParseTreeNode)
                    ElseIf lrType Is GetType(Object) Then
                        ao_object.SetAttributeMember(aoParseTreeNode.Token.Type.ToString, aoParseTreeNode)
                    ElseIf lrType Is GetType(String) Then
                        ao_object.SetAttributeMember(aoParseTreeNode.Token.Type.ToString, aoParseTreeNode.Token.Text)
                    End If
                End If

                For Each loParseTreeNode In aoParseTreeNode.Nodes
                    Call GetParseTreeTokens(ao_object, loParseTreeNode)
                Next

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()
                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Public Sub GetParseTreeTokensReflection(ByRef ao_object As Object, ByRef aoParseTreeNode As FTR.ParseNode)

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
            Dim loParseTreeNode As FTR.ParseNode
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
                    Call Me.GetParseTreeTokensReflection(ao_object, loParseTreeNode)
                Next

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()
                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Public Function ParseTreeContainsTokenType(ByVal aoParseTree As VAQL.ParseTree, ByVal aoTokenType As VAQL.TokenType) As Boolean


            Dim loParseNode As VAQL.ParseNode

            ParseTreeContainsTokenType = False

            For Each loParseNode In aoParseTree.Nodes
                If Me.ParseNodeContainsTokenType(loParseNode, aoTokenType) = True Then
                    ParseTreeContainsTokenType = True
                    Exit For
                End If
            Next

        End Function

        Public Function ParseNodeContainsTokenType(ByVal aoParseNode As VAQL.ParseNode, ByVal aoTokenType As VAQL.TokenType) As Boolean

            Dim loParseNode As VAQL.ParseNode

            ParseNodeContainsTokenType = False

            If aoParseNode.Token.Type = aoTokenType Then
                Return True
            Else
                For Each loParseNode In aoParseNode.Nodes
                    If Me.ParseNodeContainsTokenType(loParseNode, aoTokenType) = True Then
                        ParseNodeContainsTokenType = True
                        Exit For
                    End If
                Next
            End If

        End Function

        Public Function ProcessFTR(ByVal asFTRText As String, ByRef aoParseTree As FTR.ParseTree) As Object

            Dim lrFact As New FBM.Fact

            Try
                '---------------------------
                'Parse the ORMQR statement
                '---------------------------
                Me.Parsetree = Me.Parser.Parse(asFTRText)

                aoParseTree = Me.Parsetree

                Return False

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)

                Return False
            End Try

        End Function


    End Class

End Namespace
