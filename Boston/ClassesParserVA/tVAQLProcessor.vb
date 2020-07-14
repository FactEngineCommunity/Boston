Imports System.Reflection

Namespace VAQL

    Public Class Processor

        Private Model As FBM.Model

        Private Parser As New VAQL.Parser(New VAQL.Scanner) 'Used to parse Text input into the Brain; especially for ORMQL.
        Private Parsetree As New VAQL.ParseTree 'Used with the Parser, is populated during the parsing of text input into the Brain; especially ORMQL

        Public ISACONCEPTStatement As New Object
        Public ISANENTITYTYPEStatement As New Object
        Public ATMOSTONEStatement As New Object
        Public PREDICATEPARTClause As New Object
        Public VALUETYPEISWRITTENASStatement As New Object
        Public ENTITYTYPEISIDENTIFIEDBYITSStatement As New Object
        Public FACTTYPEREADINGStatement As New Object
        Public MODELELEMENTClause As New Object

        Public Sub New()

        End Sub

        Public Sub New(ByRef arModel As FBM.Model)

            Call Me.New()
            Me.Model = arModel

        End Sub

        Public Sub setDynamicObjects()

            '====================================================================
            'Create the DynamicObject for ISACONCEPT Texts/Satements
            '====================================================================
            Dim lrIsAConceptStatement As New DynamicClassLibrary.Factory.tClass
            lrIsAConceptStatement.add_attribute(New DynamicClassLibrary.Factory.tAttribute("FRONTREADINGTEXT", GetType(String)))
            lrIsAConceptStatement.add_attribute(New DynamicClassLibrary.Factory.tAttribute("MODELELEMENTNAME", GetType(String)))

            Me.ISACONCEPTStatement = lrIsAConceptStatement.clone

            '====================================================================
            'Create the DynamicObject for IsAnEntityType Texts/Satements
            '====================================================================
            Dim lrIsAnEntityTypeStatement As New DynamicClassLibrary.Factory.tClass
            lrIsAnEntityTypeStatement.add_attribute(New DynamicClassLibrary.Factory.tAttribute("MODELELEMENTNAME", GetType(String)))
            lrIsAnEntityTypeStatement.add_attribute(New DynamicClassLibrary.Factory.tAttribute("KEYWDISANENTITYTYPE", GetType(String)))

            Me.ISANENTITYTYPEStatement = lrIsAnEntityTypeStatement.clone

            '====================================================================
            'Create the DynamicObject for FACTTYPEREADING Texts/Satements
            '====================================================================
            Dim lrFactTypeReadingStatement As New DynamicClassLibrary.Factory.tClass
            lrFactTypeReadingStatement.add_attribute(New DynamicClassLibrary.Factory.tAttribute("FRONTREADINGTEXT", GetType(String)))
            lrFactTypeReadingStatement.add_attribute(New DynamicClassLibrary.Factory.tAttribute("MODELELEMENT", GetType(List(Of Object))))
            lrFactTypeReadingStatement.add_attribute(New DynamicClassLibrary.Factory.tAttribute("PREDICATECLAUSE", GetType(List(Of Object))))
            lrFactTypeReadingStatement.add_attribute(New DynamicClassLibrary.Factory.tAttribute("UNARYPREDICATEPART", GetType(String)))
            lrFactTypeReadingStatement.add_attribute(New DynamicClassLibrary.Factory.tAttribute("FOLLOWINGREADINGTEXT", GetType(String)))

            Me.FACTTYPEREADINGStatement = lrFactTypeReadingStatement.clone

            '====================================================================
            'Create the DynamicObject for MODELELEMENT Clauses
            '====================================================================
            Dim lrModelElementClause As New DynamicClassLibrary.Factory.tClass
            lrModelElementClause.add_attribute(New DynamicClassLibrary.Factory.tAttribute("PREBOUNDREADINGTEXT", GetType(String)))
            lrModelElementClause.add_attribute(New DynamicClassLibrary.Factory.tAttribute("MODELELEMENTNAME", GetType(String)))
            lrModelElementClause.add_attribute(New DynamicClassLibrary.Factory.tAttribute("POSTBOUNDREADINGTEXT", GetType(String)))

            Me.MODELELEMENTClause = lrModelElementClause.clone

            '====================================================================
            'Create the DynamicObject for ENTITYTYPEISIDENTIFIEDBYITS Satements
            '====================================================================
            Dim lrEntityTypeIsIdentifiedByItsStatement As New DynamicClassLibrary.Factory.tClass
            lrEntityTypeIsIdentifiedByItsStatement.add_attribute(New DynamicClassLibrary.Factory.tAttribute("MODELELEMENTNAME", GetType(String)))
            lrEntityTypeIsIdentifiedByItsStatement.add_attribute(New DynamicClassLibrary.Factory.tAttribute("REFERENCEMODE", GetType(String)))

            Me.ENTITYTYPEISIDENTIFIEDBYITSStatement = lrEntityTypeIsIdentifiedByItsStatement.clone

            '==========================================================
            '==========================================================
            Dim lrValueTypeIsWrittenAsStatement As New DynamicClassLibrary.Factory.tClass
            lrValueTypeIsWrittenAsStatement.add_attribute(New DynamicClassLibrary.Factory.tAttribute("MODELELEMENTNAME", GetType(String)))
            lrValueTypeIsWrittenAsStatement.add_attribute(New DynamicClassLibrary.Factory.tAttribute("DATATYPE", GetType(Object)))
            lrValueTypeIsWrittenAsStatement.add_attribute(New DynamicClassLibrary.Factory.tAttribute("DATATYPELENGTH", GetType(Object)))
            lrValueTypeIsWrittenAsStatement.add_attribute(New DynamicClassLibrary.Factory.tAttribute("DATATYPEPRECISION", GetType(Object)))
            lrValueTypeIsWrittenAsStatement.add_attribute(New DynamicClassLibrary.Factory.tAttribute("NUMBER", GetType(String)))

            Me.VALUETYPEISWRITTENASStatement = lrValueTypeIsWrittenAsStatement.clone

            Dim lrATMOSTONEStatement As New DynamicClassLibrary.Factory.tClass
            lrATMOSTONEStatement.add_attribute(New DynamicClassLibrary.Factory.tAttribute("FRONTREADINGTEXT", GetType(String)))
            lrATMOSTONEStatement.add_attribute(New DynamicClassLibrary.Factory.tAttribute("MODELELEMENT", GetType(List(Of Object))))
            lrATMOSTONEStatement.add_attribute(New DynamicClassLibrary.Factory.tAttribute("MODELELEMENTNAME", GetType(List(Of String))))
            lrATMOSTONEStatement.add_attribute(New DynamicClassLibrary.Factory.tAttribute("PREDICATECLAUSE", GetType(List(Of Object))))
            lrATMOSTONEStatement.add_attribute(New DynamicClassLibrary.Factory.tAttribute("UNARYPREDICATEPART", GetType(String)))
            lrATMOSTONEStatement.add_attribute(New DynamicClassLibrary.Factory.tAttribute("FOLLOWINGREADINGTEXT", GetType(String)))

            Me.ATMOSTONEStatement = lrATMOSTONEStatement.clone

            '=========================================================
            'Create the DynamicObject for PREDICATE Clauses
            '=========================================================
            Dim lrORMQLPREDICATECLAUSE As New DynamicClassLibrary.Factory.tClass
            lrORMQLPREDICATECLAUSE.add_attribute(New DynamicClassLibrary.Factory.tAttribute("PREDICATEPART", GetType(List(Of String))))

            Me.PREDICATEPARTClause = lrORMQLPREDICATECLAUSE.clone


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
        Public Sub GetParseTreeTokens(ByRef ao_object As Object, ByRef aoParseTreeNode As VAQL.ParseNode)

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

            '======================================================================================
            'How to do this using System.Reflection
            'MsgBox(GetType(ClientServer.User).GetField("FirstName").FieldType.ToString)
            'Call GetType(ClientServer.User).GetField("FirstName").SetValue(Me.zrUser, "Hi there")
            'MsgBox(zrUser.FirstName)
            '======================================================================================

            Dim loPropertyInfo() As System.Reflection.PropertyInfo = ao_object.GetType().GetProperties
            Dim loProperty As PropertyInfo
            Dim lr_bag As New Stack
            Dim loParseTreeNode As VAQL.ParseNode

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


        Public Function ProcessVAQLStatement(ByVal as_ORMQL_statement As String, _
                                             ByRef aoTokenType As VAQL.TokenType, _
                                             ByRef aoParseTree As VAQL.ParseTree) As Object

            Dim lrFact As New FBM.Fact

            Try
                '---------------------------
                'Parse the ORMQR statement
                '---------------------------
                Me.Parsetree = Me.Parser.Parse(as_ORMQL_statement)

                If Me.Parsetree.Errors.Count > 0 Then
                    Return False
                Else
                    If Me.ParseTreeContainsTokenType(Me.Parsetree, TokenType.VALUETYPEISWRITTENASCLAUSE) Then
                        aoTokenType = TokenType.VALUETYPEISWRITTENASCLAUSE
                        aoParseTree = Me.Parsetree
                    ElseIf Me.ParseTreeContainsTokenType(Me.Parsetree, TokenType.ENTITYTYPEISIDENTIFIEDBYITSCLAUSE) Then
                        aoTokenType = TokenType.ENTITYTYPEISIDENTIFIEDBYITSCLAUSE
                        aoParseTree = Me.Parsetree

                    ElseIf Me.ParseTreeContainsTokenType(Me.Parsetree, TokenType.MODELELEMENTLEADINGSTMT) And
                           Me.ParseTreeContainsTokenType(Me.Parsetree, TokenType.FACTTYPECLAUSE) And
                           Not Me.ParseTreeContainsTokenType(Me.Parsetree, TokenType.KEYWDATMOSTONE) And
                           Not Me.ParseTreeContainsTokenType(Me.Parsetree, TokenType.KEYWDONE) Then
                        aoTokenType = TokenType.FACTTYPECLAUSE
                        aoParseTree = Me.Parsetree

                    ElseIf Me.ParseTreeContainsTokenType(Me.Parsetree, TokenType.MODELELEMENTLEADINGSTMT) And
                           Me.ParseTreeContainsTokenType(Me.Parsetree, TokenType.KEYWDATMOSTONE) Then
                        aoTokenType = TokenType.KEYWDATMOSTONE
                        aoParseTree = Me.Parsetree
                    ElseIf Me.ParseTreeContainsTokenType(Me.Parsetree, TokenType.MODELELEMENTLEADINGSTMT) And
                           Me.ParseTreeContainsTokenType(Me.Parsetree, TokenType.KEYWDONE) Then
                        aoTokenType = TokenType.KEYWDONE
                        aoParseTree = Me.Parsetree
                    ElseIf Me.ParseTreeContainsTokenType(Me.Parsetree, TokenType.MODELELEMENTLEADINGSTMT) And
                           (Me.ParseTreeContainsTokenType(Me.Parsetree, TokenType.BINARYPREDICATECLAUSE) Or
                           Me.ParseTreeContainsTokenType(Me.Parsetree, TokenType.UNARYPREDICATECLAUSE)) Then
                        aoTokenType = TokenType.FACTTYPECLAUSE
                        aoParseTree = Me.Parsetree
                    ElseIf Me.ParseTreeContainsTokenType(Me.Parsetree, TokenType.KEYWDISACONCEPT) Then
                        aoTokenType = TokenType.KEYWDISACONCEPT
                        aoParseTree = Me.Parsetree
                    ElseIf Me.ParseTreeContainsTokenType(Me.Parsetree, TokenType.KEYWDISANENTITYTYPE) Then
                        aoTokenType = TokenType.KEYWDISANENTITYTYPE
                        aoParseTree = Me.Parsetree
                    End If

                    Return True
                End If

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
