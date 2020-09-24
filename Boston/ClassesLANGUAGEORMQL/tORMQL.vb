Imports System.Reflection
Imports System.Xml.Serialization
Imports System.Xml.Linq
Imports <xmlns:ns="http://www.w3.org/2001/XMLSchema">

Namespace ORMQL


    Public Class AddFactStatement

        Private _USERTABLENAME As String
        Public Property USERTABLENAME As String
            Get
                Return Me._USERTABLENAME
            End Get
            Set(value As String)
                Me._USERTABLENAME = value
            End Set
        End Property

        Private _VALUE As String
        Public Property VALUE As String
            Get
                Return Me._VALUE
            End Get
            Set(value As String)
                Me._VALUE = value
            End Set
        End Property

        Private _PAGENAME As String
        Public Property PAGENAME As String
            Get
                Return Me._PAGENAME
            End Get
            Set(value As String)
                Me._PAGENAME = value
            End Set
        End Property

    End Class

    Public Class CreateFactTypeStatement

        Private _FACTTYPENAME As String
        Public Property FACTTYPENAME As String
            Get
                Return Me._FACTTYPENAME
            End Get
            Set(value As String)
                Me._FACTTYPENAME = value
            End Set
        End Property

        Private _MODELELEMENTNAME As List(Of String)
        Public Property MODELELEMENTNAME As List(Of String)
            Get
                Return Me._MODELELEMENTNAME
            End Get
            Set(value As List(Of String))
                Me._MODELELEMENTNAME = value
            End Set
        End Property

        Private _PREDICATE As List(Of String)
        Public Property PREDICATE As List(Of String)
            Get
                Return Me._PREDICATE
            End Get
            Set(value As List(Of String))
                Me._PREDICATE = value
            End Set
        End Property
    End Class

    Public Class DeleteStatement

        Dim _USERTABLENAME As String
        Public Property USERTABLENAME As String
            Get
                Return Me._USERTABLENAME
            End Get
            Set(value As String)
                Me._USERTABLENAME = value
            End Set
        End Property

        Dim _PAGENAME As String
        Public Property PAGENAME As String
            Get
                Return Me._PAGENAME
            End Get
            Set(value As String)
                Me._PAGENAME = value
            End Set
        End Property

        Dim _WHERECLAUSECOLUMNNAMESTR As New List(Of String)
        Public Property WHERECLAUSECOLUMNNAMESTR As List(Of String)
            Get
                Return Me._WHERECLAUSECOLUMNNAMESTR
            End Get
            Set(value As List(Of String))
                Me._WHERECLAUSECOLUMNNAMESTR = value
            End Set
        End Property

        Dim _VALUE As New List(Of String)
        Public Property VALUE As List(Of String)
            Get
                Return Me._VALUE
            End Get
            Set(value As List(Of String))
                Me._VALUE = value
            End Set
        End Property
    End Class

    Public Class DeleteFactStatement

        Dim _USERTABLENAME As String
        Public Property USERTABLENAME As String
            Get
                Return Me._USERTABLENAME
            End Get
            Set(value As String)
                Me._USERTABLENAME = value
            End Set
        End Property

        Dim _PAGENAME As String
        Public Property PAGENAME As String
            Get
                Return Me._PAGENAME
            End Get
            Set(value As String)
                Me._PAGENAME = value
            End Set
        End Property

        Dim _VALUE As String
        Public Property VALUE As String
            Get
                Return Me._VALUE
            End Get
            Set(value As String)
                Me._VALUE = value
            End Set
        End Property
    End Class

    Public Class InsertStatement

        Private _USERTABLENAME As String = Nothing
        Public Property USERTABLENAME As String
            Get
                Return Me._USERTABLENAME
            End Get
            Set(value As String)
                Me._USERTABLENAME = value
            End Set
        End Property

        Private _COLUMNNAMESTR As New List(Of String)
        Public Property COLUMNNAMESTR As List(Of String)
            Get
                Return Me._COLUMNNAMESTR
            End Get
            Set(value As List(Of String))
                Me._COLUMNNAMESTR = value
            End Set
        End Property

        Private _PAGENAME As String = Nothing
        Public Property PAGENAME As String
            Get
                Return Me._PAGENAME
            End Get
            Set(value As String)
                Me._PAGENAME = value
            End Set
        End Property

        Private _MODELID As String = Nothing
        Public Property MODELID As String
            Get
                Return Me._MODELID
            End Get
            Set(value As String)
                Me._MODELID = value
            End Set
        End Property

        Private _VALUE As New List(Of String)
        Public Property VALUE As List(Of String)
            Get
                Return Me._VALUE
            End Get
            Set(value As List(Of String))
                Me._VALUE = value
            End Set
        End Property

    End Class

    Public Class RemoveInstanceStatement

        Dim _MODELELEMENTNAME As String
        Public Property MODELELEMENTNAME As String
            Get
                Return Me._MODELELEMENTNAME
            End Get
            Set(value As String)
                Me._MODELELEMENTNAME = value
            End Set
        End Property

        Dim _VALUE As String
        Public Property VALUE As String
            Get
                Return Me._VALUE
            End Get
            Set(value As String)
                Me._VALUE = value
            End Set
        End Property

    End Class

    Public Class RenameInstanceStatement

        Dim _MODELELEMENTNAME As String
        Public Property MODELELEMENTNAME As String
            Get
                Return Me._MODELELEMENTNAME
            End Get
            Set(value As String)
                Me._MODELELEMENTNAME = value
            End Set
        End Property

        Dim _VALUE As List(Of String)
        Public Property VALUE As List(Of String)
            Get
                Return Me._VALUE
            End Get
            Set(value As List(Of String))
                Me._VALUE = value
            End Set
        End Property

    End Class

    Public Class SelectStatement

        Private _COLUMNLIST As New List(Of Object) 'Stores the ParseNode containing the list of Columns to be returned.
        Public Property COLUMNLIST As List(Of Object)
            Get
                Return Me._COLUMNLIST
            End Get
            Set(value As List(Of Object))
                Me._COLUMNLIST = value
            End Set
        End Property

        Private _COLUMNNAMESTR As New List(Of String) 'Stores the list of Columns to be Returned, OR EmptyList for *, or Count(*)
        Public Property COLUMNNAMESTR As List(Of String)
            Get
                Return Me._COLUMNNAMESTR
            End Get
            Set(value As List(Of String))
                Me._COLUMNNAMESTR = value
            End Set
        End Property

        Private _KEYWDDISTINCT As Object = Nothing 'SELECT can be 'DISTINCT".
        Public Property KEYWDDISTINCT As Object
            Get
                Return Me._KEYWDDISTINCT
            End Get
            Set(value As Object)
                Me._KEYWDDISTINCT = value
            End Set
        End Property

        Private _USERTABLENAME As String 'Stores the table being searched on
        Public Property USERTABLENAME As String
            Get
                Return Me._USERTABLENAME
            End Get
            Set(value As String)
                Me._USERTABLENAME = value
            End Set
        End Property

        Private _PAGENAME As String = Nothing 'Stores the Page being searched, if the search is for a table on a Page.
        Public Property PAGENAME As String
            Get
                Return Me._PAGENAME
            End Get
            Set(value As String)
                Me._PAGENAME = value
            End Set
        End Property

        Private _MODELID As String 'Stores the Model being searched on
        Public Property MODELID As String
            Get
                Return Me._MODELID
            End Get
            Set(value As String)
                Me._MODELID = value
            End Set
        End Property

        Private _WHERESTMT As Object = Nothing 'Stores a list of Where clauses
        Public Property WHERESTMT As Object
            Get
                Return Me._WHERESTMT
            End Get
            Set(value As Object)
                Me._WHERESTMT = value
            End Set
        End Property

    End Class

    Public Class WhereClauseTree
        Private _COMPARISON As New List(Of Object)
        Public Property COMPARISON As List(Of Object)
            Get
                Return Me._COMPARISON
            End Get
            Set(value As List(Of Object))
                Me._COMPARISON = value
            End Set
        End Property
    End Class

    Public Class UpdateStatement

        Private _USERTABLENAME As String
        Public Property USERTABLENAME As String
            Get
                Return Me._USERTABLENAME
            End Get
            Set(value As String)
                Me._USERTABLENAME = value
            End Set
        End Property

        Private _COLUMNNAMESTR As New List(Of String)
        Public Property COLUMNNAMESTR As List(Of String)
            Get
                Return Me._COLUMNNAMESTR
            End Get
            Set(value As List(Of String))
                Me._COLUMNNAMESTR = value
            End Set
        End Property

        Private _VALUE As New List(Of String)
        Public Property VALUE As List(Of String)
            Get
                Return Me._VALUE
            End Get
            Set(value As List(Of String))
                Me._VALUE = value
            End Set
        End Property

    End Class
    Public Class Processor

        Private Model As FBM.Model

        '------------------------------------------------------
        'The Parser and ParseTree are built into the Model
        '  such that ORMQL statements can be passed to 
        '  a Model for processing (on itself), in the same
        '  way that an SQL relational database has its own Parser
        ' for SQL statements.
        '------------------------------------------------------

        Private Parser As New TinyPG.Parser(New TinyPG.Scanner) 'Used to parse Text input into the Brain; especially for ORMQL.
        Private Parsetree As New TinyPG.ParseTree 'Used with the Parser, is populated during the parsing of text input into the Brain; especially ORMQL
        Public SelectStatement As New SelectStatement
        Public WhereClauseTree As New WhereClauseTree
        Public InsertStatement As New InsertStatement
        Public DeleteStatement As New DeleteStatement
        Public DeleteFactStatement As New DeleteFactStatement
        Public CreateFactTypeStatement As New ORMQL.CreateFactTypeStatement
        Public AddFactStatement As New AddFactStatement
        Public UpdateStatement As New UpdateStatement
        Public RemoveInstanceStatement As New RemoveInstanceStatement
        Public RenameInstanceStatement As New RenameInstanceStatement

        Public Sub New()

            Try
                Richmond.WriteToStatusBar("Creating Dynamic Classes", True)

                ''================================================
                ''Create the DynamicObject for Select Statements
                ''================================================
                'Dim lrORMQLSelectStatement As New DynamicClassLibrary.Factory.tClass
                'lrORMQLSelectStatement.add_attribute(New DynamicClassLibrary.Factory.tAttribute("COLUMNLIST", GetType(List(Of Object)))) 'Stores the ParseNode containing the list of Columns to be returned.
                'lrORMQLSelectStatement.add_attribute(New DynamicClassLibrary.Factory.tAttribute("COLUMNNAMESTR", GetType(List(Of String)))) 'Stores the list of Columns to be Returned, OR EmptyList for *, or Count(*)
                'lrORMQLSelectStatement.add_attribute(New DynamicClassLibrary.Factory.tAttribute("KEYWDDISTINCT", GetType(Object))) 'Stores the list of Columns to be Returned, OR EmptyList for *, or Count(*)
                'lrORMQLSelectStatement.add_attribute(New DynamicClassLibrary.Factory.tAttribute("USERTABLENAME", GetType(List(Of String)))) 'Stores the table being searched on
                'lrORMQLSelectStatement.add_attribute(New DynamicClassLibrary.Factory.tAttribute("PAGENAME", GetType(List(Of String)))) 'Stores the Page being searched, if the search is for a table on a Page.
                'lrORMQLSelectStatement.add_attribute(New DynamicClassLibrary.Factory.tAttribute("MODELID", GetType(List(Of String)))) 'Stores the Model being searched on
                'lrORMQLSelectStatement.add_attribute(New DynamicClassLibrary.Factory.tAttribute("WHERESTMT", GetType(Object))) 'Stores a list of Where clauses

                'Me.SelectStatement = lrORMQLSelectStatement.clone

                '================================================
                'Create the DynamicObject for Where Clauses
                '================================================
                'Dim lrWhereClauseTree As New DynamicClassLibrary.Factory.tClass
                'lrWhereClauseTree.add_attribute(New DynamicClassLibrary.Factory.tAttribute("COMPARISON", GetType(List(Of Object))))

                'Me.WhereClauseTree = lrWhereClauseTree.clone

                ''==========================================================
                'Dim lrORMQLInsertStatement As New DynamicClassLibrary.Factory.tClass
                'lrORMQLInsertStatement.add_attribute(New DynamicClassLibrary.Factory.tAttribute("USERTABLENAME", GetType(List(Of String))))
                'lrORMQLInsertStatement.add_attribute(New DynamicClassLibrary.Factory.tAttribute("COLUMNNAMESTR", GetType(List(Of String))))
                'lrORMQLInsertStatement.add_attribute(New DynamicClassLibrary.Factory.tAttribute("PAGENAME", GetType(List(Of String))))
                'lrORMQLInsertStatement.add_attribute(New DynamicClassLibrary.Factory.tAttribute("MODELID", GetType(List(Of String))))
                'lrORMQLInsertStatement.add_attribute(New DynamicClassLibrary.Factory.tAttribute("VALUE", GetType(List(Of String))))

                'Me.InsertStatement = lrORMQLInsertStatement.clone

                ''==========================================================
                'Dim lrORMQLDeleteStatement As New DynamicClassLibrary.Factory.tClass
                'lrORMQLDeleteStatement.add_attribute(New DynamicClassLibrary.Factory.tAttribute("USERTABLENAME", GetType(List(Of String))))
                'lrORMQLDeleteStatement.add_attribute(New DynamicClassLibrary.Factory.tAttribute("PAGENAME", GetType(List(Of String))))
                'lrORMQLDeleteStatement.add_attribute(New DynamicClassLibrary.Factory.tAttribute("WHERECLAUSECOLUMNNAMESTR", GetType(List(Of String))))
                'lrORMQLDeleteStatement.add_attribute(New DynamicClassLibrary.Factory.tAttribute("VALUE", GetType(List(Of String))))

                'Me.DeleteStatement = lrORMQLDeleteStatement.clone

                '==========================================================
                'Dim lrORMQLDeleteFactStatement As New DynamicClassLibrary.Factory.tClass
                'lrORMQLDeleteFactStatement.add_attribute(New DynamicClassLibrary.Factory.tAttribute("VALUE", GetType(List(Of String))))
                'lrORMQLDeleteFactStatement.add_attribute(New DynamicClassLibrary.Factory.tAttribute("USERTABLENAME", GetType(List(Of String))))
                'lrORMQLDeleteFactStatement.add_attribute(New DynamicClassLibrary.Factory.tAttribute("PAGENAME", GetType(List(Of String))))

                'Me.DeleteFactStatement = lrORMQLDeleteFactStatement.clone

                '================================================
                'Create the DynamicObject for Select Statements
                '================================================
                'Dim lrORMQLCreateFactTypeStatement As New DynamicClassLibrary.Factory.tClass
                'lrORMQLCreateFactTypeStatement.add_attribute(New DynamicClassLibrary.Factory.tAttribute("FACTTYPENAME", GetType(String))) '(Optional) Stores the FactTypeName for the new FactType.
                'lrORMQLCreateFactTypeStatement.add_attribute(New DynamicClassLibrary.Factory.tAttribute("MODELELEMENTNAME", GetType(List(Of String)))) 'Stores the list of ModelElements for which the FactType is being created.
                'lrORMQLCreateFactTypeStatement.add_attribute(New DynamicClassLibrary.Factory.tAttribute("PREDICATE", GetType(List(Of String)))) 'Stores the list of PredicateParts for the FactTypeReading for the FactType.

                'Me.CreateFactTypeStatement = lrORMQLCreateFactTypeStatement.clone

                '-----------------------------------------------
                'Create the DynamicClass for ADDFACT Statemnts
                '-----------------------------------------------
                'Dim lrClass As New DynamicClassLibrary.Factory.tClass
                'lrClass.add_attribute(New DynamicClassLibrary.Factory.tAttribute("USERTABLENAME", GetType(String)))
                'lrClass.add_attribute(New DynamicClassLibrary.Factory.tAttribute("VALUE", GetType(String)))
                'lrClass.add_attribute(New DynamicClassLibrary.Factory.tAttribute("PAGENAME", GetType(String)))

                'Me.AddFactStatement = lrClass.clone

                '-----------------------------------------------
                'Create the DynamicClass for UPDATE Statemnts
                '-----------------------------------------------
                'Dim lrORMQLUpdateStatement As New DynamicClassLibrary.Factory.tClass
                'lrORMQLUpdateStatement.add_attribute(New DynamicClassLibrary.Factory.tAttribute("USERTABLENAME", GetType(String)))
                'lrORMQLUpdateStatement.add_attribute(New DynamicClassLibrary.Factory.tAttribute("COLUMNNAMESTR", GetType(List(Of String))))
                'lrORMQLUpdateStatement.add_attribute(New DynamicClassLibrary.Factory.tAttribute("VALUE", GetType(List(Of String))))

                'Me.UpdateStatement = lrORMQLUpdateStatement.clone

                'REMOVE INSTANCE
                'Dim lrORMQLRemoveInstanceStatement As New DynamicClassLibrary.Factory.tClass
                'lrORMQLRemoveInstanceStatement.add_attribute(New DynamicClassLibrary.Factory.tAttribute("MODELELEMENTNAME", GetType(String)))
                'lrORMQLRemoveInstanceStatement.add_attribute(New DynamicClassLibrary.Factory.tAttribute("VALUE", GetType(String)))

                'Me.RemoveInstanceStatement = lrORMQLRemoveInstanceStatement.clone

                'RENAME INSTANCE
                'Dim lrORMQLRenameInstanceStatement As New DynamicClassLibrary.Factory.tClass
                'lrORMQLRenameInstanceStatement.add_attribute(New DynamicClassLibrary.Factory.tAttribute("MODELELEMENTNAME", GetType(String)))
                'lrORMQLRenameInstanceStatement.add_attribute(New DynamicClassLibrary.Factory.tAttribute("VALUE", GetType(List(Of String))))

                'Me.RenameInstanceStatement = lrORMQLRenameInstanceStatement.clone


                Richmond.WriteToStatusBar("Dynamic Classes Created", True)

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Public Sub New(ByRef arModel As FBM.Model)

            Me.Model = arModel

        End Sub

        ''' <summary>        
        '''ORMQL Mode. Gets the tokens from the Parse Tree.
        '''Walks the ParseTree and finds the tokens as per the Properties/Fields of the ao_object passed to the procedure.
        '''  i.e. Based on the type of token at the Root of the ParseTree, the software dynamically creates ao_object such that 
        '''  it contains the tokens that it wants returned.
        ''' </summary>
        ''' <param name="ao_object">is of runtime generated type DynamicCollection.Entity</param>
        ''' <param name="aoParseTreeNode">ParseNode as from TinyPG Parser</param>
        ''' <remarks></remarks>
        Public Sub GetParseTreeTokens(ByRef ao_object As Object, ByRef aoParseTreeNode As TinyPG.ParseNode)

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
            Dim loParseTreeNode As TinyPG.ParseNode

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

                For Each loParseTreeNode In aoParseTreeNode.Nodes.ToArray
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

        Public Sub GetParseTreeTokensReflection(ByRef ao_object As Object, ByRef aoParseTreeNode As TinyPG.ParseNode)

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
            Dim loParseTreeNode As TinyPG.ParseNode
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

        Private Function ProcessREMOVEINSTANCEStatement() As Boolean

            Try
                '-------------------------------------------
                'Create the DynamicClass within the Factory
                '-------------------------------------------
                'Dim lrClass As New DynamicClassLibrary.Factory.tClass
                'lrClass.add_attribute(New DynamicClassLibrary.Factory.tAttribute("MODELELEMENTNAME", GetType(String)))
                'lrClass.add_attribute(New DynamicClassLibrary.Factory.tAttribute("VALUE", GetType(String)))

                ''-------------------------
                ''Set the DynamicObject
                ''-------------------------
                'Dim lrRemoveInstanceStatement As New Object
                'lrRemoveInstanceStatement = prApplication.ORMQL.RemoveInstanceStatement

                'lrRemoveInstanceStatement.MODELELEMENTNAME = ""
                'lrRemoveInstanceStatement.VALUE = ""

                ''----------------------------------
                ''Get the Tokens from the ParseTree
                ''----------------------------------
                'Call Me.GetParseTreeTokens(lrRemoveInstanceStatement, Me.Parsetree.Nodes(0))

                '=============================================================
                Dim lrRemoveInstanceStatement As New ORMQL.RemoveInstanceStatement
                lrRemoveInstanceStatement = prApplication.ORMQL.RemoveInstanceStatement

                lrRemoveInstanceStatement.MODELELEMENTNAME = Nothing
                lrRemoveInstanceStatement.VALUE = Nothing

                '----------------------------------
                'Get the Tokens from the ParseTree
                '----------------------------------
                Call Me.GetParseTreeTokensReflection(lrRemoveInstanceStatement, Me.Parsetree.Nodes(0))
                '======================================================================

                Dim lrModelElement As FBM.ModelObject = Me.Model.GetModelObjectByName(lrRemoveInstanceStatement.MODELELEMENTNAME)

                Call lrModelElement.removeInstance(lrRemoveInstanceStatement.VALUE)

                Return True

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)

                Return False
            End Try

        End Function

        Private Function ProcessRENAMEINSTANCEStatement() As Boolean

            Try
                '-------------------------
                'Set the DynamicObject
                '-------------------------
                'Dim lrRenameInstanceStatement As New Object
                'lrRenameInstanceStatement = prApplication.ORMQL.RenameInstanceStatement

                'lrRenameInstanceStatement.MODELELEMENTNAME = ""
                'lrRenameInstanceStatement.VALUE = New List(Of String)

                ''----------------------------------
                ''Get the Tokens from the ParseTree
                ''----------------------------------
                'Call Me.GetParseTreeTokens(lrRenameInstanceStatement, Me.Parsetree.Nodes(0))

                '=============================================================
                Dim lrRenameInstanceStatement As New ORMQL.RenameInstanceStatement
                lrRenameInstanceStatement = prApplication.ORMQL.RenameInstanceStatement

                lrRenameInstanceStatement.MODELELEMENTNAME = Nothing
                lrRenameInstanceStatement.VALUE = New List(Of String)

                '----------------------------------
                'Get the Tokens from the ParseTree
                '----------------------------------
                Call Me.GetParseTreeTokensReflection(lrRenameInstanceStatement, Me.Parsetree.Nodes(0))
                '======================================================================

                Dim lrModelElement As FBM.ModelObject = Me.Model.GetModelObjectByName(lrRenameInstanceStatement.MODELELEMENTNAME)

                Call lrModelElement.renameInstance(lrRenameInstanceStatement.VALUE(0), lrRenameInstanceStatement.VALUE(1))

                Return True

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)

                Return False
            End Try

        End Function

        Private Function ProcessSelectINSTANCESStatement(ByVal asORMQLStatement As String) As Object

            Try
                '---------------------------------------------------------------------------------
                'Create a DynamicClass within the Factor, to store the ParseTreeTokens as they
                '  are collected from the ParseText
                '---------------------------------------------------------------------------------

                '=============================================================
                Dim lrselectStatement As ORMQL.SelectStatement
                lrselectStatement = prApplication.ORMQL.SelectStatement

                lrselectStatement.COLUMNLIST = New List(Of Object)
                lrselectStatement.KEYWDDISTINCT = New Object
                lrselectStatement.COLUMNNAMESTR = New List(Of String)
                lrselectStatement.MODELID = Nothing
                lrselectStatement.USERTABLENAME = Nothing
                lrselectStatement.PAGENAME = Nothing
                lrselectStatement.WHERESTMT = New Object

                '----------------------------------
                'Get the Tokens from the ParseTree
                '----------------------------------
                Call Me.GetParseTreeTokensReflection(lrselectStatement, Me.Parsetree.Nodes(0))
                '======================================================================

                'Facts to return
                Dim larFact As New List(Of FBM.Fact)
                Dim lrFact As FBM.Fact
                Dim lrFactType = New FBM.FactType(Me.Model, "DummyFactType", True)
                Dim lrRole = New FBM.Role(lrFactType, "INSTANCES", True, Nothing)
                lrFactType.RoleGroup.Add(lrRole)

                Dim lrModelElement = Me.Model.GetModelObjectByName(lrselectStatement.USERTABLENAME)

                Select Case lrModelElement.ConceptType
                    Case Is = pcenumConceptType.EntityType
                        Dim lrEntityType = CType(lrModelElement, FBM.EntityType)
                        For Each lsInstance In lrEntityType.Instance
                            lrFact = New FBM.Fact(lrFactType, False)
                            lrFact.Data.Add(New FBM.FactData(lrRole, New FBM.Concept(lsInstance), lrFact))
                            larFact.Add(lrFact)
                        Next
                End Select

                Dim lrORMQlREcordset As New ORMQL.Recordset

                lrORMQlREcordset.Facts = larFact
                lrORMQlREcordset.Columns = lrselectStatement.COLUMNNAMESTR

                Return lrORMQlREcordset

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)

                Return New ORMQL.Recordset
            End Try

        End Function

        Private Function ProcessSelectStatement(ByVal asORMQLStatement As String) As Object

            Dim lrFact As New FBM.Fact
            Dim lrFactType As FBM.FactType
            Dim xml As XDocument
            Dim lrCustomClass As TinyPG.ParseNode
            Dim lrSerializer As System.Xml.Serialization.XmlSerializer
            Dim lrWriter As System.Xml.XmlWriter
            Dim lsColumnName As String = ""

            Try
                '---------------------------------------------------------------------------------
                'Create a DynamicClass within the Factor, to store the ParseTreeTokens as they
                '  are collected from the ParseText
                '---------------------------------------------------------------------------------
                Dim lrFactList As New List(Of FBM.Fact)
                Dim larFactList As New List(Of FBM.FactInstance)
                Dim lrRoleData As FBM.FactData
                Dim lsSelectType As String = ""
                Dim lrPage As FBM.Page

                '=============================================================
                Dim lrselectStatement As ORMQL.SelectStatement
                lrselectStatement = prApplication.ORMQL.SelectStatement

                lrselectStatement.COLUMNLIST = New List(Of Object)
                lrselectStatement.KEYWDDISTINCT = New Object
                lrselectStatement.COLUMNNAMESTR = New List(Of String)
                lrselectStatement.MODELID = Nothing
                lrselectStatement.USERTABLENAME = Nothing
                lrselectStatement.PAGENAME = Nothing
                lrselectStatement.WHERESTMT = New Object

                '----------------------------------
                'Get the Tokens from the ParseTree
                '----------------------------------
                Call Me.GetParseTreeTokensReflection(lrselectStatement, Me.Parsetree.Nodes(0))
                '======================================================================

                ''-------------------------
                ''Create the DynamicObject
                ''-------------------------
                'Dim lrSelectStatement As New Object
                'lrSelectStatement = prApplication.ORMQL.SelectStatement

                'lrSelectStatement.COLUMNLIST.CLear()
                'lrSelectStatement.KEYWDDISTINCT = New Object
                'lrSelectStatement.COLUMNNAMESTR.Clear()
                'lrSelectStatement.MODELID.Clear()
                'lrSelectStatement.USERTABLENAME.Clear()
                'lrSelectStatement.PAGENAME.Clear()
                'lrSelectStatement.WHERESTMT = New Object

                ''----------------------------------
                ''Get the Tokens from the ParseTree
                ''----------------------------------

                'Call Me.GetParseTreeTokens(lrSelectStatement, Me.Parsetree.Nodes(0))

                '-------------------------------------------------------------
                'Check if the Select query is targeted at a particular Model
                '-------------------------------------------------------------
                If lrselectStatement.MODELID IsNot Nothing Then
                    If (String.Compare(lrselectStatement.MODELID, Me.Model.ModelId)) = 0 Then
                        '-------------------------------------------------------------
                        'User has elected to search the current model, so do nothing
                        '-------------------------------------------------------------
                    Else
                        '-----------------------------------------
                        'Send the query to the appropriate Model
                        '-----------------------------------------
                        Dim lsModelId As String = lrselectStatement.MODELID
                        Dim lrModel As FBM.Model '(pcenumLanguage.ORMModel, "", lsModelId)
                        lrModel = prApplication.Models.Find(Function(x) x.ModelId = lsModelId) 'AddressOf lrModel.Equals)

                        If IsSomething(lrModel) Then
                            Dim lrReturnObject As New Object
                            lrReturnObject = lrModel.ORMQL.ProcessORMQLStatement(asORMQLStatement)
                            Return lrReturnObject
                        End If
                    End If
                End If


                '----------------------------------------------------------------
                'Check to see if there are any Parse Errors in the ORMQL Statment
                '----------------------------------------------------------------
                If Me.Parsetree.Errors.Count > 0 Then
                    Dim lr_error As TinyPG.ParseError
                    For Each lr_error In Me.Parsetree.Errors
                        Throw New ApplicationException("Error: tModel.ProcessORMQLStatement: " & lr_error.Message)
                    Next
                    Return False
                    Exit Function
                End If

                '---------------------------------------------------------
                'Find the FactType that the SELECT statement is for.
                '---------------------------------------------------------
                If lrselectStatement.PAGENAME Is Nothing Then
                    'lrFactType = New FBM.FactType(Me.Model, lrSelectStatement.USERTABLENAME(0).ToString, False)
                    '20200504-VM-Faster as below. Remove above and comment below if all okay after a period of time
                    lrFactType = Me.Model.FactType.Find(Function(x) x.Id = lrselectStatement.USERTABLENAME) 'AddressOf lrFactType.EqualsByName)
                Else
                    lrPage = Me.Model.Page.Find(Function(x) x.Name = lrselectStatement.PAGENAME)
                    'lrFactType = New FBM.FactTypeInstance(Me.Model, lrPage, pcenumLanguage.ORMModel, lrSelectStatement.USERTABLENAME(0).ToString, True)
                    '20200504-VM-Faster as below. Remove above and comment below if all okay after a period of time
                    lrFactType = lrPage.FactTypeInstance.Find(Function(x) x.Id = lrselectStatement.USERTABLENAME) 'AddressOf lrFactType.EqualsByName)
                End If

                If lrFactType Is Nothing Then
                    Dim lsInterimMessage As String
                    lsInterimMessage = "Cannot find Fact Type Instance, '" & lrselectStatement.USERTABLENAME
                    If lrselectStatement.PAGENAME IsNot Nothing Then
                        lsInterimMessage &= "', on Page, '" & lrselectStatement.PAGENAME & "'."
                    End If
                    Throw New Exception(lsInterimMessage)
                End If

                If Not lrFactType.ExistsRoleNameForEveryRole Then
                    Throw New Exception("Error: The FactType, '" & lrFactType.Name & "', must have a RoleName for each Role before creating a SELECT statement.")
                End If

                If lrFactType Is Nothing Then
                    'Throw New ApplicationException("Error: tModel.ProcessORMQLStatement: Can't find FactType with Name: " & lrSelectStatement.USERTABLENAME(0).ToString)
                    Dim lr_error As New TinyPG.ParseError("Error: Can't find FactType with Name: " & lrselectStatement.USERTABLENAME, 101, Nothing)
                    Me.Parsetree.Errors.Add(lr_error)
                    Return Me.Parsetree.Errors
                    Exit Function
                End If

                ''-------------------------
                ''Create the DynamicObject
                ''-------------------------
                'Dim lrWhereClauseTree As Object = prApplication.ORMQL.WhereClauseTree
                'lrWhereClauseTree.COMPARISON.Clear()

                '=============================================================                
                Dim lrWhereClauseTree = New ORMQL.WhereClauseTree

                '----------------------------------
                'Get the Tokens from the ParseTree
                '----------------------------------
                Call Me.GetParseTreeTokensReflection(lrWhereClauseTree, Me.Parsetree.Nodes(0))
                '======================================================================


                '=============================================
                'Process the Where Clause (if there is one)
                '=============================================
                If lrselectStatement.WHERESTMT.GetType Is GetType(Object) Then
                    '----------------------
                    'No WHERECLAUSE found
                    '  Retrieve all the Facts from the FactType
                    '-----------------------------------------
                    Select Case lrFactType.GetType.ToString
                        Case Is = GetType(FBM.FactType).ToString
                            lrFactList = lrFactType.Fact
                        Case Is = GetType(FBM.FactTypeInstance).ToString
                            Dim lrReturnFactTypeInstance As FBM.FactTypeInstance = lrFactType
                            'lrReturnFactTypeInstance = lrFactType
                            larFactList = lrReturnFactTypeInstance.Fact
                            For Each lrReturnFactInstance In larFactList
                                lrFactList.Add(lrReturnFactInstance)
                            Next
                    End Select

                    '==================================================================
                    '20200504-VM-Was formerly the below which did not return the actual Facts.
                    'This might have been because they would otherwise get accidentally modified. I can't remember why.
                    'If the above (returning the actual facts) does not work, consider uncommenting out the below.
                    'lrFactList = lrFactType.CloneFacts
                Else
                    '-------------------------------------
                    'The ORMQL Statement has a WHERESTMT
                    '-------------------------------------
                    lrWhereClauseTree.COMPARISON.Clear()
                    Call Me.GetParseTreeTokensReflection(lrWhereClauseTree, lrselectStatement.WHERESTMT) 'Me.parsetree.Nodes(0))

                    '-----------------------------------------------------------------------------
                    'Create a FactPredicate. Used for Lambda search on the FactType.Fact
                    '  to retrieve the set of Facts that match the Predicates of the WHERESTMT
                    '-----------------------------------------------------------------------------
                    Dim lrFactPredicate As New FBM.FactPredicate

                    '-------------------------------
                    'Get the COMPARISON predicates
                    '-------------------------------
                    Dim lrComparison As New Object
                    For Each lrComparison In lrWhereClauseTree.COMPARISON
                        lrCustomClass = lrComparison

                        lrSerializer = New System.Xml.Serialization.XmlSerializer(lrCustomClass.GetType())
                        xml = New XDocument

                        lrWriter = xml.CreateWriter
                        lrSerializer.Serialize(lrWriter, lrCustomClass)
                        lrWriter.Close()

                        Dim lasColumnName As XElement = <Comparison><%= From p In xml.<ParseNode>.<Nodes>.<ParseNode>.<Token>
                                                                        Where p.@Type = "WHERECLAUSECOLUMNNAMESTR"
                                                                        Select p.<Text>.Value
                                                                    %>
                                                        </Comparison>

                        lsColumnName = lasColumnName.Value


                        Dim lsDataValue As String = ""


                        Dim lrDataValue As XElement = <Comparison><%= From p In xml.<ParseNode>.<Nodes>.<ParseNode>.<Nodes>.<ParseNode>.<Token>
                                                                      Where p.@Type = "VALUE"
                                                                      Select p.<Text>.Value
                                                                  %>
                                                      </Comparison>

                        lsDataValue = lrDataValue.Value

                        lrRoleData = New FBM.FactData(New FBM.Role(lrFactType, lsColumnName, True), New FBM.Concept(lsDataValue))

                        lrFactPredicate.data.Add(lrRoleData)
                    Next

                    '--------------------------------------------------------------------
                    'Retrieve all the Facts from the FactType that match the predicate.
                    '--------------------------------------------------------------------
                    Select Case lrFactType.GetType.ToString
                        Case Is = GetType(FBM.FactType).ToString
                            lrFactList = lrFactType.Fact.FindAll(AddressOf lrFactPredicate.Equals)
                        Case Is = GetType(FBM.FactTypeInstance).ToString
                            Dim lrReturnFactTypeInstance As FBM.FactTypeInstance = lrFactType
                            'lrReturnFactTypeInstance = lrFactType 'As above to make faster
                            larFactList = lrReturnFactTypeInstance.Fact.FindAll(AddressOf lrFactPredicate.Equals)
                            Dim lrReturnFactInstance As FBM.FactInstance
                            For Each lrReturnFactInstance In larFactList
                                lrFactList.Add(lrReturnFactInstance)
                            Next
                    End Select

                End If 'The ORMQL Statement has a WHERESTMT

                '=============================================================
                'If KEYWDDISTINCT then return only the distinct set of Facts
                '=============================================================
                If lrselectStatement.KEYWDDISTINCT.GetType Is GetType(Object) Then
                    '----------------------
                    'No DISTINCT required
                    '----------------------
                Else
                    '---------------------------------------
                    'Return only the distinct set of Facts
                    '---------------------------------------
                    Dim larReturnDictionarySet As New Dictionary(Of String, String)
                    Dim lsKey As String = ""

                    For Each lrFact In lrFactList.ToArray
                        '-----------------------------------
                        'Create the key for the Dictionary
                        '-----------------------------------
                        lsKey = lrFact.EnumerateDataAsKey(lrselectStatement.COLUMNNAMESTR)

                        If larReturnDictionarySet.ContainsKey(lsKey) Then
                            lrFactList.Remove(lrFact)
                        Else
                            larReturnDictionarySet.Add(lsKey, lsKey)
                        End If
                    Next

                End If


                '-----------------------------------------------------------
                'Construct the DictionarySet for the Facts in the FactList
                '-----------------------------------------------------------

                '-------------------------------------------------------------------------------------------
                'Get the type of SELECT type for each column. Can be either, Attribute, Count(*), or *
                '-------------------------------------------------------------------------------------------
                Dim lrColumn As New Object
                For Each lrColumn In lrselectStatement.COLUMNLIST

                    Dim customClass As TinyPG.ParseNode = lrColumn
                    'MsgBox(Richmond.IsSerializable(customClass).ToString)
                    Dim serializer As New System.Xml.Serialization.XmlSerializer(customClass.GetType())
                    '
                    xml = New XDocument

                    Dim writer As System.Xml.XmlWriter = xml.CreateWriter
                    serializer.Serialize(writer, customClass)
                    writer.Close()

                    Dim lasSelectType As XElement = <Comparison><%= From p In xml.<ParseNode>.<Nodes>.<ParseNode>.<Nodes>.<ParseNode>.<Token>
                                                                    Select p.@Type
                                                                %>
                                                    </Comparison>

                    lsSelectType = lasSelectType.Value

                    Select Case lsSelectType
                        Case Is = "KEYWDCOUNTSTAR"
                            lrFact = New FBM.Fact()

                            lrFact.DictionarySet.Add("Count", lrFactList.Count)
                            lrFact.Symbol = "Count"

                            Dim lrDummyFactType As New FBM.FactType(Me.Model, "DummyFactType", True)
                            lrRoleData = New FBM.FactData(New FBM.Role(lrDummyFactType, "Count", True), New FBM.Concept("Count"))
                            lrRoleData.setData(lrFactList.Count, pcenumConceptType.Value, False)

                            lrFact.Data.Add(lrRoleData)

                            lrFactList = New List(Of FBM.Fact)
                            lrFactList.Add(lrFact)
                            lrselectStatement.COLUMNNAMESTR.Add("Count")
                            Exit For
                        Case Is = "STAR"
                            If lrFactList.Count > 0 Then
                                lrselectStatement.COLUMNNAMESTR.Clear()
                                If lrFactList(0).GetType Is GetType(FBM.Fact) Then
                                    For Each lrRoleData In lrFactList(0).Data
                                        lrselectStatement.COLUMNNAMESTR.Add(lrRoleData.Role.Name)
                                    Next
                                Else
                                    Dim lrFactInstance As New FBM.FactInstance
                                    Dim lrFactDataInstance As FBM.FactDataInstance
                                    lrFactInstance = lrFactList(0)
                                    For Each lrFactDataInstance In lrFactInstance.Data
                                        lrselectStatement.COLUMNNAMESTR.Add(lrFactDataInstance.Role.Name)
                                    Next
                                End If
                            End If
                            For Each lrFact In lrFactList
                                For Each lrRoleData In lrFact.Data
                                    If StrComp(lrRoleData.Role.Name, "") = 0 Then
                                        If Not lrFact.DictionarySet.Keys.Contains(lrRoleData.Role.Id) Then
                                            lrFact.DictionarySet.Add(lrRoleData.Role.Id, lrRoleData.Data)
                                        End If
                                    Else
                                        If Not lrFact.DictionarySet.Keys.Contains(lrRoleData.Role.Name) Then
                                            lrFact.DictionarySet.Add(lrRoleData.Role.Name, lrRoleData.Data)
                                        End If
                                    End If
                                Next
                            Next
                        Case Is = "COLUMNNAMESTR"

                        Case Is = "COLUMNNAMESTRCOMMACOLUMNNAME"
                            If lrFactList.Count > 0 Then
                                'lrSelectStatement.COLUMNNAMESTR.Clear()
                                'For Each lrRoleData In lrFactList(0).Data
                                '    lrSelectStatement.COLUMNNAMESTR.Add(lrRoleData.Role.Name)
                                'Next
                            End If
                            For Each lrFact In lrFactList
                                For Each lrRoleData In lrFact.Data
                                    If StrComp(lrRoleData.Role.Name, "") = 0 Then
                                        If lrFact.DictionarySet.Keys.Contains(lrRoleData.Role.Id) Then
                                        Else
                                            lrFact.DictionarySet.Add(lrRoleData.Role.Id, lrRoleData.Data)
                                        End If
                                    Else
                                        If lrFact.DictionarySet.Keys.Contains(lrRoleData.Role.Name) Then
                                        Else
                                            lrFact.DictionarySet.Add(lrRoleData.Role.Name, lrRoleData.Data)
                                        End If
                                    End If
                                Next
                            Next
                    End Select

                Next

                Dim lrORMQlREcordset As New ORMQL.Recordset

                lrORMQlREcordset.Facts = lrFactList
                lrORMQlREcordset.Columns = lrselectStatement.COLUMNNAMESTR

                Return lrORMQlREcordset

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)

                Return New ORMQL.Recordset
            End Try

        End Function

        Private Function processUPDATEStatement() As Object

            Try
                '-------------------------
                'Create the DynamicObject
                '-------------------------
                'Dim lrUpdateStatement As New Object
                'lrUpdateStatement = prApplication.ORMQL.UpdateStatement

                'lrUpdateStatement.USERTABLENAME = ""
                'lrUpdateStatement.COLUMNNAMESTR = New List(Of String)
                'lrUpdateStatement.VALUE = New List(Of String)

                ''----------------------------------
                ''Get the Tokens from the ParseTree
                ''----------------------------------
                'Call Me.GetParseTreeTokens(lrUpdateStatement, Me.Parsetree.Nodes(0))

                '=============================================================
                Dim lrupdateStatement As New ORMQL.UpdateStatement
                lrupdateStatement = prApplication.ORMQL.UpdateStatement

                lrupdateStatement.USERTABLENAME = Nothing
                lrupdateStatement.COLUMNNAMESTR.Clear()
                lrupdateStatement.VALUE.Clear()

                '----------------------------------
                'Get the Tokens from the ParseTree
                '----------------------------------
                Call Me.GetParseTreeTokensReflection(lrupdateStatement, Me.Parsetree.Nodes(0))
                '======================================================================

                Dim lrFactType As FBM.FactType
                Dim lrFact As FBM.Fact

                lrFactType = Me.Model.FactType.Find(Function(x) x.Id = lrupdateStatement.USERTABLENAME)

                'Where Clause
                For Each lrFact In lrFactType.Fact.FindAll(Function(x) x.Data.Find(Function(y) y.Role.Name = lrupdateStatement.COLUMNNAMESTR(1)).Data = lrupdateStatement.VALUE(1))
                    lrFact = lrFactType.Fact.Find(Function(x) x.Data.Find(Function(y) y.Role.Name = lrupdateStatement.COLUMNNAMESTR(1)).Data = lrupdateStatement.VALUE(1))

                    'Update Data
                    If lrFact IsNot Nothing Then
                        Dim lrFactData As FBM.FactData
                        lrFactData = lrFact.Data.Find(Function(x) x.Role.Name = lrupdateStatement.COLUMNNAMESTR(0))
                        lrFactData.Data = lrupdateStatement.VALUE(0)
                        lrFact.isDirty = True
                        lrFactData.isDirty = True
                        lrFactType.isDirty = True
                        Me.Model.IsDirty = True
                    End If
                Next

                Return True

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)

                Return False
            End Try

        End Function


        Private Function ProcessINSERTStatement(ByVal asORMQLStatement As String) As Object

            Dim lrFact As New FBM.Fact
            Dim lrFactType As FBM.FactType
            Dim lsColumnName As String = ""

            Try

                '-------------------------
                'Create the DynamicObject
                '-------------------------
                'Dim lrInsertStatement As New Object
                'lrInsertStatement = prApplication.ORMQL.InsertStatement

                'lrInsertStatement.USERTABLENAME.Clear()
                'lrInsertStatement.COLUMNNAMESTR.Clear()
                'lrInsertStatement.PAGENAME.Clear()
                'lrInsertStatement.MODELID.Clear()
                'lrInsertStatement.VALUE.Clear()

                ''----------------------------------
                ''Get the Tokens from the ParseTree
                ''----------------------------------
                'Call Me.GetParseTreeTokens(lrInsertStatement, Me.Parsetree.Nodes(0))

                '=============================================================
                Dim lrinsertStatement As New ORMQL.InsertStatement
                'lrinsertStatement = prApplication.ORMQL.insertStatement

                lrinsertStatement.USERTABLENAME = Nothing
                lrinsertStatement.PAGENAME = Nothing
                lrinsertStatement.MODELID = Nothing
                lrinsertStatement.COLUMNNAMESTR = New List(Of String)
                lrinsertStatement.VALUE = New List(Of String)

                '----------------------------------
                'Get the Tokens from the ParseTree
                '----------------------------------
                Call Me.GetParseTreeTokensReflection(lrinsertStatement, Me.Parsetree.Nodes(0))
                '======================================================================

                If Me.Parsetree.Errors.Count > 0 Then
                    Return Me.Parsetree.Errors
                    Exit Function
                End If

                '-------------------------------------------------------------
                'Check if the Select query is targeted at a particular Model
                '-------------------------------------------------------------
                If lrinsertStatement.MODELID IsNot Nothing Then
                    If (String.Compare(lrinsertStatement.MODELID, Me.Model.ModelId)) = 0 Then
                        '-------------------------------------------------------------
                        'User has elected to search the current model, so do nothing
                        '-------------------------------------------------------------
                    Else
                        Dim lsModelId As String = lrinsertStatement.MODELID
                        Dim lrModel As FBM.Model '(pcenumLanguage.ORMModel, "", lsModelId)

                        lrModel = prApplication.Models.Find(Function(x) x.ModelId = lsModelId) 'AddressOf lrModel.Equals)

                        If IsSomething(lrModel) Then
                            Dim lrReturnObject As New Object
                            lrReturnObject = lrModel.ORMQL.ProcessORMQLStatement(asORMQLStatement)
                            Return lrReturnObject
                        Else
                            Me.Parsetree.Errors.Add(New TinyPG.ParseError("Error: tModel.ProcessORMQLStatement: Can't find Model with ModelId: '" & lsModelId & "'", 100, Nothing))
                            Return Me.Parsetree.Errors
                            Exit Function
                        End If
                    End If
                End If

                '---------------------------------------------------------
                'Find the FactType that the INSERT INTO statement is for.
                '---------------------------------------------------------
                'lrFactType = New FBM.FactType(Me.Model, lrInsertStatement.USERTABLENAME(0).ToString, False)
                lrFactType = Me.Model.FactType.Find(Function(x) x.Id = lrinsertStatement.USERTABLENAME) 'AddressOf lrFactType.EqualsByName)

                If lrFactType Is Nothing Then
                    Me.Parsetree.Errors.Add(New TinyPG.ParseError("Error: tModel.ProcessORMQLStatement: Can't find FactType with Name: " & lrinsertStatement.USERTABLENAME, 100, Nothing))
                    Return Me.Parsetree.Errors
                    Exit Function
                End If

                '----------------
                'Create the Fact
                '----------------
                lrFact = New FBM.Fact(lrFactType, True)
                Dim liInd As Integer = 0
                Dim lrFactData As FBM.FactData
                Dim lrModelDictionaryEntry As New FBM.DictionaryEntry

                Dim lrRole As FBM.Role
                For Each lsColumnName In lrinsertStatement.COLUMNNAMESTR
                    'lrRole.Name = lsColumnName
                    lrRole = lrFactType.RoleGroup.Find(Function(x) x.Name = lsColumnName) 'AddressOf lrRole.EqualsByName)
                    Dim lrConcept As FBM.Concept = New FBM.Concept(Trim(lrinsertStatement.VALUE(liInd).ToString))

                    '----------------------------------------------------------------------------------------
                    'Link the FactData.Concept to the corresponding ModelDictionary.DictionaryEntry.Concept
                    '----------------------------------------------------------------------------------------
                    lrModelDictionaryEntry = New FBM.DictionaryEntry(Me.Model, lrConcept.Symbol, pcenumConceptType.Value)

                    lrModelDictionaryEntry = Me.Model.AddModelDictionaryEntry(lrModelDictionaryEntry, True, True, False)

                    lrFactData = New FBM.FactData(lrRole, lrModelDictionaryEntry.Concept, lrFact)
                    lrFact.isDirty = True

                    lrFact.Data.Add(lrFactData)
                    lrFact.isDirty = True
                    liInd += 1
                Next

                lrFactType.AddFact(lrFact)
                lrFactType.isDirty = True

                '---------------------------------------------------
                'Check to see if the Fact is to be added to a Page
                '---------------------------------------------------
                If lrinsertStatement.PAGENAME IsNot Nothing Then
                    '--------------
                    'Get the Page
                    '--------------
                    Dim lrPage As FBM.Page
                    lrPage = Me.Model.Page.Find(Function(x) x.Name = lrinsertStatement.PAGENAME)

                    If IsSomething(lrPage) Then
                        '-----------------------------------------------------------
                        'Find the FactTypeInstance to add the new FactInstance to.
                        '-----------------------------------------------------------
                        Dim lrFactTypeInstance As FBM.FactTypeInstance '= lrFactType.CloneInstance(lrPage, False)
                        lrFactTypeInstance = lrPage.FactTypeInstance.Find(Function(x) x.Id = lrFactType.Id) 'AddressOf lrFactTypeInstance.Equals)

                        If IsSomething(lrFactTypeInstance) Then
                            Dim lrFactInstance As FBM.FactInstance
                            lrFactTypeInstance.FactTable.TableShape.AddRow()

                            lrFactInstance = lrPage.CreateFactInstance(lrFactTypeInstance, lrFact)

                            '--------------------------------------------------
                            'Add the new FactInstance to the FactTypeInstance
                            '--------------------------------------------------
                            lrFactTypeInstance.AddFactInstance(lrFactInstance)
                            lrFactTypeInstance.isDirty = True

                            lrFactTypeInstance.FactTable.ResortFactTable()

                            lrPage.MakeDirty()

                            Return lrFactInstance
                            Exit Function
                        Else
                            Throw New Exception("No FactTypeInstance found for FactType.Name: " & lrFactType.Name & " on Page.Name: " & lrPage.Name)
                        End If

                    Else
                        Throw New Exception("No Page found in the Model for :" & lrinsertStatement.PAGENAME(0))
                    End If
                Else
                    Return lrFact
                    Exit Function
                End If

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()
                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)

                Return Nothing
            End Try


        End Function

        Private Function ProcessADDFACTStatement() As Object

            Dim lrFact As New FBM.Fact
            Dim lrFactType As FBM.FactType
            Dim lsColumnName As String = ""

            'Dim lrAddFactStatement As New Object
            'lrAddFactStatement = prApplication.ORMQL.AddFactStatement

            'lrAddFactStatement.USERTABLENAME = ""
            'lrAddFactStatement.VALUE = ""
            'lrAddFactStatement.PAGENAME = ""

            ''----------------------------------
            ''Get the Tokens from the ParseTree
            ''----------------------------------
            'Call Me.GetParseTreeTokens(lrAddFactStatement, Me.Parsetree.Nodes(0))

            '=============================================================
            Dim lrAddFactStatement As ORMQL.AddFactStatement
            lrAddFactStatement = prApplication.ORMQL.AddFactStatement

            lrAddFactStatement.USERTABLENAME = Nothing
            lrAddFactStatement.VALUE = Nothing
            lrAddFactStatement.PAGENAME = Nothing

            '----------------------------------
            'Get the Tokens from the ParseTree
            '----------------------------------
            Call Me.GetParseTreeTokensReflection(lrAddFactStatement, Me.Parsetree.Nodes(0))
            '======================================================================

            '========================================
            '---------------------------------------------------------
            'Find the FactType that the ADD FACT statement is for.
            '---------------------------------------------------------
            'lrFactType = New FBM.FactType(lrAddFactStatement.USERTABLENAME, True)
            lrFactType = Me.Model.FactType.Find(Function(x) x.Id = lrAddFactStatement.USERTABLENAME) 'AddressOf lrFactType.EqualsByName)

            If lrFactType Is Nothing Then
                Me.Parsetree.Errors.Add(New TinyPG.ParseError("Error: tModel.ProcessORMQLStatement: Can't find FactType with Name: '" & lrAddFactStatement.USERTABLENAME & "' within the Model.", 100, Nothing))
                Return Me.Parsetree.Errors
                Exit Function
            End If

            '-----------------
            'Create the Fact
            '-----------------
            'lrFact = New FBM.Fact(lrAddFactStatement.VALUE, lrFactType)
            lrFact = lrFactType.Fact.Find(Function(x) x.Id = lrAddFactStatement.VALUE) 'AddressOf lrFact.EqualsById)

            If lrFact Is Nothing Then
                Dim lrNode As New TinyPG.ParseNode()
                lrNode.Token = New TinyPG.Token(0, 0)
                Me.Parsetree.Errors.Add(New TinyPG.ParseError("Error: tModel.ProcessORMQLStatement: Can't find Fact with Id: '" & lrAddFactStatement.VALUE & "' within the Model level FactType.Id :" & lrAddFactStatement.USERTABLENAME, 100, lrNode))
                Return Me.Parsetree.Errors
                Exit Function
            End If

            Dim lrFactInstance As FBM.FactInstance

            If lrAddFactStatement.PAGENAME IsNot Nothing Then
                Dim lrPage As FBM.Page
                lrPage = Me.Model.Page.Find(Function(x) x.Name = lrAddFactStatement.PAGENAME)

                If IsSomething(lrPage) Then
                    Dim lrFactTypeInstance As FBM.FactTypeInstance '= lrFactType.CloneInstance(lrPage, False)
                    lrFactTypeInstance = lrPage.FactTypeInstance.Find(Function(x) x.Id = lrFactType.Id) 'AddressOf lrFactTypeInstance.Equals)
                    If IsSomething(lrFactTypeInstance) Then
                        lrFactInstance = lrFactTypeInstance.AddFact(lrFact, False)
                        Return lrFactInstance
                    Else
                        Me.Parsetree.Errors.Add(New TinyPG.ParseError("Error: tModel.ProcessORMQLStatement: Can't find FactTypeInstance with Id: '" & lrFactType.Id & "'.", 100, Nothing))
                        Return Me.Parsetree.Errors
                    End If
                Else
                    Me.Parsetree.Errors.Add(New TinyPG.ParseError("Error: tModel.ProcessORMQLStatement: Can't find Page with Name: '" & lrAddFactStatement.PAGENAME & "'.", 100, Nothing))
                    Return Me.Parsetree.Errors
                End If
            Else
                Return lrFact
            End If

        End Function

        Private Function ProcessDELETEStatement() As Object

            Dim lrFact As New FBM.Fact
            Dim lrFactType As FBM.FactType
            Dim lsColumnName As String = ""

            ''-------------------------------------------
            ''Create the DynamicClass within the Factory
            ''-------------------------------------------
            'Dim lrClass As New DynamicClassLibrary.Factory.tClass
            'lrClass.add_attribute(New DynamicClassLibrary.Factory.tAttribute("USERTABLENAME", GetType(List(Of String))))
            'lrClass.add_attribute(New DynamicClassLibrary.Factory.tAttribute("PAGENAME", GetType(List(Of String))))
            'lrClass.add_attribute(New DynamicClassLibrary.Factory.tAttribute("WHERECLAUSECOLUMNNAMESTR", GetType(List(Of String))))
            'lrClass.add_attribute(New DynamicClassLibrary.Factory.tAttribute("VALUE", GetType(List(Of String))))

            ''-------------------------
            ''Create the DynamicObject
            ''-------------------------
            'Dim lr_object As New Object
            'lr_object = lrClass.clone

            '----------------------------------
            'Get the Tokens from the ParseTree
            '----------------------------------
            'Call Me.GetParseTreeTokens(lr_object, Me.Parsetree.Nodes(0))

            '=============================================================
            Dim lrDeleteStatement As New ORMQL.DeleteStatement
            lrDeleteStatement = prApplication.ORMQL.DeleteStatement

            lrDeleteStatement.USERTABLENAME = Nothing
            lrDeleteStatement.PAGENAME = Nothing
            lrDeleteStatement.WHERECLAUSECOLUMNNAMESTR.Clear()
            lrDeleteStatement.VALUE.Clear()

            '----------------------------------
            'Get the Tokens from the ParseTree
            '----------------------------------
            Call Me.GetParseTreeTokensReflection(lrDeleteStatement, Me.Parsetree.Nodes(0))
            '======================================================================

            If Me.Parsetree.Errors.Count > 0 Then
                Dim lr_error As TinyPG.ParseError
                For Each lr_error In Me.Parsetree.Errors
                    Throw New ApplicationException("Error: tModel.ProcessORMQLStatement: " & lr_error.Message)
                Next
                Return False
                Exit Function
            End If

            '---------------------------------------------------------
            'Find the FactType that the DELETESTMT statement is for.
            '---------------------------------------------------------
            'lrFactType = New FBM.FactType(Me.Model, lr_object.USERTABLENAME(0).ToString, True)
            lrFactType = Me.Model.FactType.Find(Function(x) x.Id = lrDeleteStatement.USERTABLENAME) 'AddressOf lrFactType.EqualsByName)

            If lrFactType Is Nothing Then
                Throw New ApplicationException("Error: tModel.ProcessORMQLStatement: Can't find FactType with Name: " & lrDeleteStatement.USERTABLENAME(0).ToString)
                Return False
                Exit Function
            End If

            lrFact = New FBM.Fact(lrFactType)
            Dim liInd As Integer = 0

            Dim lrRole As New FBM.Role
            For Each lsColumnName In lrDeleteStatement.WHERECLAUSECOLUMNNAMESTR
                'lrRole.Name = lsColumnName
                lrRole = lrFactType.RoleGroup.Find(Function(x) x.Name = lsColumnName) 'AddressOf lrRole.EqualsByName)
                lrFact.Data.Add(New FBM.FactData(lrRole, New FBM.Concept(Trim(lrDeleteStatement.VALUE(liInd).ToString))))
                liInd += 1
            Next

            '--------------------------------------------------------------------------
            'Check to see if the Delete Statmenent elects to remove Facts from a Page
            '--------------------------------------------------------------------------
            If lrDeleteStatement.PAGENAME IsNot Nothing Then
                '--------------
                'Get the Page
                '--------------
                Dim lrPage As FBM.Page = Me.Model.Page.Find(Function(x) x.Name = lrDeleteStatement.PAGENAME)

                If IsSomething(lrPage) Then
                    '---------------------------
                    'Find the FactTypeInstance 
                    '---------------------------
                    Dim lrFactTypeInstance As FBM.FactTypeInstance '= lrFactType.CloneInstance(lrPage, False)
                    lrFactTypeInstance = lrPage.FactTypeInstance.Find(Function(x) x.Id = lrFactType.Id) 'AddressOf lrFactTypeInstance.Equals)

                    If IsSomething(lrFactTypeInstance) Then
                        Dim lrFactInstance As FBM.FactInstance
                        lrFactInstance = lrFact.CloneInstance(lrPage)
                        '20200710-VM-Was the following. Remove if all okay.
                        'lrFactInstance = lrFactTypeInstance.Fact.Find(AddressOf lrFactInstance.EqualsByFirstDataMatch)
                        'Do While lrFactInstance IsNot Nothing
                        '    lrFactTypeInstance.RemoveFact(lrFactInstance)
                        '    lrFactInstance = lrFact.CloneInstance(lrPage)
                        '    lrFactInstance = lrFactTypeInstance.Fact.Find(AddressOf lrFactInstance.EqualsByFirstDataMatch)
                        'Loop 'Remove all Facts on the Page that match the WHERE clause.

                        Dim larFactInstance As List(Of FBM.FactInstance) = lrFactTypeInstance.Fact.FindAll(AddressOf lrFactInstance.EqualsByFirstDataMatch)

                        For Each lrFactInstance In larFactInstance
                            lrFactTypeInstance.RemoveFact(lrFactInstance)
                        Next 'Remove all Facts on the Page that match the WHERE clause.

                        If lrPage.Language = pcenumLanguage.ORMModel Then Call lrFactTypeInstance.FactTable.ResortFactTable()
                    End If
                End If
            Else
                lrFactType.RemoveFactByData(lrFact, True)
            End If

            Return True

        End Function

        Private Function ProcessDELETEFACTSTMTStatement() As Object

            Dim lrFactType As FBM.FactType
            Dim lrFact As FBM.Fact

            '-------------------------------------------
            'Create the DynamicClass within the Factory
            '-------------------------------------------
            'Dim lrClass As New DynamicClassLibrary.Factory.tClass
            'lrClass.add_attribute(New DynamicClassLibrary.Factory.tAttribute("VALUE", GetType(String)))
            'lrClass.add_attribute(New DynamicClassLibrary.Factory.tAttribute("USERTABLENAME", GetType(List(Of String))))
            'lrClass.add_attribute(New DynamicClassLibrary.Factory.tAttribute("PAGENAME", GetType(List(Of String))))

            '-------------------------
            'Create the DynamicObject
            '-------------------------
            'Dim lr_object As New Object
            'lr_object = lrClass.clone

            'lr_object.VALUE = ""

            '----------------------------------
            'Get the Tokens from the ParseTree
            '----------------------------------
            'Call Me.GetParseTreeTokens(lr_object, Me.Parsetree.Nodes(0))

            '=============================================================
            Dim lrDeleteFactStatement As New Object
            lrDeleteFactStatement = prApplication.ORMQL.DeleteFactStatement

            lrDeleteFactStatement.VALUE.Clear()
            lrDeleteFactStatement.USERTABLENAME.Clear()
            lrDeleteFactStatement.PAGENAME.Clear()

            '----------------------------------
            'Get the Tokens from the ParseTree
            '----------------------------------
            Call Me.GetParseTreeTokensReflection(lrDeleteFactStatement, Me.Parsetree.Nodes(0))
            '======================================================================

            If Me.Parsetree.Errors.Count > 0 Then
                Dim lr_error As TinyPG.ParseError
                For Each lr_error In Me.Parsetree.Errors
                    Throw New ApplicationException("Error: tModel.ProcessORMQLStatement: " & lr_error.Message)
                Next
                Return False
                Exit Function
            End If

            '---------------------------------------------------------
            'Find the FactType that the DELETESTMT statement is for.
            '---------------------------------------------------------
            'lrFactType = New FBM.FactType(Me.Model, lr_object.USERTABLENAME(0).ToString, True)
            lrFactType = Me.Model.FactType.Find(Function(x) x.Id = lrDeleteFactStatement.USERTABLENAME(0).ToString) 'AddressOf lrFactType.EqualsByName)

            If lrFactType Is Nothing Then
                Throw New ApplicationException("Error: tModel.ProcessORMQLStatement: Can't find FactType with Name: " & lrDeleteFactStatement.USERTABLENAME(0).ToString)
                Return False
                Exit Function
            End If

            'lrFact = New FBM.Fact(lr_object.VALUE, lrFactType)
            lrFact = lrFactType.Fact.Find(Function(x) x.Id = lrDeleteFactStatement.VALUE) 'AddressOf lrFact.EqualsById)

            If lrFact Is Nothing Then
                Throw New ApplicationException("Can't find Fact.Id: " & lrDeleteFactStatement.VALUE & " in FactType.Name: " & lrFactType.Name)
                Return False
                Exit Function
            End If

            '--------------------------------------------------------------------------
            'Check to see if the Delete Statmenent elects to remove Facts from a Page
            '--------------------------------------------------------------------------
            If lrDeleteFactStatement.PAGENAME.Count > 0 Then
                '--------------
                'Get the Page
                '--------------
                Dim lrPage As FBM.Page
                lrPage = Me.Model.Page.Find(Function(x) x.Name = lrDeleteFactStatement.PAGENAME(0))

                If IsSomething(lrPage) Then
                    '-----------------------------------------------------------
                    'Find the FactTypeInstance to delete the FactInstance from.
                    '-----------------------------------------------------------
                    Dim lrFactTypeInstance As FBM.FactTypeInstance '= lrFactType.CloneInstance(lrPage, False)
                    lrFactTypeInstance = lrPage.FactTypeInstance.Find(Function(x) x.Id = lrFactType.Id) 'AddressOf lrFactTypeInstance.Equals)

                    If IsSomething(lrFactTypeInstance) Then
                        Dim lrFactInstance As FBM.FactInstance
                        'lrFactInstance = lrFact.CloneInstance(lrPage)
                        lrFactInstance = lrFactTypeInstance.Fact.Find(Function(x) x.Id - lrFact.Id) 'AddressOf lrFactInstance.EqualsById)
                        lrFactTypeInstance.RemoveFact(lrFactInstance)
                        lrFactType.RemoveFactByData(lrFact, False)
                        Call lrFactTypeInstance.FactTable.ResortFactTable()
                    End If
                End If
            Else
                lrFactType.RemoveFactByData(lrFact, True)
            End If


            Return True

        End Function

        Private Function ProcessDELETEALLStatement() As Object

            Dim lrFactType As FBM.FactType
            Dim lrFact As FBM.Fact

            '-------------------------------------------
            'Create the DynamicClass within the Factory
            '-------------------------------------------
            Dim lrClass As New DynamicClassLibrary.Factory.tClass
            lrClass.add_attribute(New DynamicClassLibrary.Factory.tAttribute("USERTABLENAME", GetType(List(Of String))))


            '-------------------------
            'Create the DynamicObject
            '-------------------------
            Dim lr_object As New Object
            lr_object = lrClass.clone

            lr_object.USERTABLENAME = New List(Of String)

            '----------------------------------
            'Get the Tokens from the ParseTree
            '----------------------------------
            Call Me.GetParseTreeTokensReflection(lr_object, Me.Parsetree.Nodes(0))

            If Me.Parsetree.Errors.Count > 0 Then
                Dim lr_error As TinyPG.ParseError
                For Each lr_error In Me.Parsetree.Errors
                    Throw New ApplicationException("Error: tModel.ProcessORMQLStatement: " & lr_error.Message)
                Next
                Return False
                Exit Function
            End If

            '---------------------------------------------------------
            'Find the FactType that the INSERT INTO statement is for.
            '---------------------------------------------------------
            'lrFactType = New FBM.FactType(lr_object.USERTABLENAME(0).ToString)
            lrFactType = Me.Model.FactType.Find(Function(x) x.Id = lr_object.USERTABLENAME(0).ToString) 'AddressOf lrFactType.EqualsByName)

            If lrFactType Is Nothing Then
                Throw New ApplicationException("Error: tModel.ProcessORMQLStatement: Can't find FactType with Name: " & lr_object.USERTABLENAME(0).ToString)
                Return False
                Exit Function
            End If

            Dim lar_fact As New List(Of FBM.Fact)

            For Each lrFact In lrFactType.Fact.ToArray
                Call lrFactType.RemoveFactById(lrFact)
            Next

            Return True


        End Function

        Private Function ProcessCREATEOBJECTStatement() As Object

            Dim lrParseValuesObject As New Object
            Dim lrDynamicObject As New Object

            '-------------------------------------------
            'Create the DynamicClass within the Factory
            '-------------------------------------------
            Dim lrClass As New DynamicClassLibrary.Factory.tClass
            lrClass.add_attribute(New DynamicClassLibrary.Factory.tAttribute("USERTABLENAME", GetType(List(Of String))))
            lrClass.add_attribute(New DynamicClassLibrary.Factory.tAttribute("COLUMNNAME", GetType(List(Of String))))
            lrClass.add_attribute(New DynamicClassLibrary.Factory.tAttribute("VALUE", GetType(List(Of String))))

            '-------------------------
            'Create the DynamicObject
            '-------------------------
            lrParseValuesObject = lrClass.clone

            lrParseValuesObject.USERTABLENAME = New List(Of String)
            lrParseValuesObject.COLUMNNAME = New List(Of String)
            lrParseValuesObject.VALUE = New List(Of String)

            '----------------------------------
            'Get the Tokens from the ParseTree
            '----------------------------------
            Call Me.GetParseTreeTokensReflection(lrParseValuesObject, Me.Parsetree.Nodes(0))

            Dim lrDynamicClass As New DynamicClassLibrary.Factory.tClass
            Dim lsAttributeName As String
            For Each lsAttributeName In lrParseValuesObject.COLUMNNAME
                lrDynamicClass.add_attribute(New DynamicClassLibrary.Factory.tAttribute(lsAttributeName, GetType(String)))
            Next
            lrDynamicClass.add_attribute(New DynamicClassLibrary.Factory.tAttribute("ConceptType", GetType(Integer)))

            lrDynamicObject = lrDynamicClass.clone

            lrDynamicObject.ConceptType = pcenumConceptType.ObjectInstance

            'For Each lsAttributeName In lrParseValuesObject.COLUMNNAME
            '    lrDynamicObject.SetValue(lsAttributeName, "ABC")
            'Next

            Return lrDynamicObject


        End Function

        Public Function ProcessADDMODELELEMENTStatement()

            Select Case Me.Parsetree.Nodes(0).Nodes(0).Nodes(1).Text
                Case Is = "ADDFACTSTMT"

                    Return Me.ProcessADDFACTStatement()

                    ''-------------------------------------------
                    ''Create the DynamicClass within the Factory
                    ''-------------------------------------------
                    'Dim lrClass As New DynamicClassLibrary.Factory.tClass
                    'lrClass.add_attribute(New DynamicClassLibrary.Factory.tAttribute("USERTABLENAME", GetType(String)))
                    'lrClass.add_attribute(New DynamicClassLibrary.Factory.tAttribute("VALUE", GetType(String)))
                    'lrClass.add_attribute(New DynamicClassLibrary.Factory.tAttribute("PAGENAME", GetType(String)))

                    ''-------------------------
                    ''Create the DynamicObject
                    ''-------------------------
                    'Dim lr_object As New Object
                    'lr_object = lrClass.clone

                    'lr_object.USERTABLENAME = ""
                    'lr_object.VALUE = ""
                    'lr_object.PAGENAME = ""

                    ''----------------------------------
                    ''Get the Tokens from the ParseTree
                    ''----------------------------------
                    'Call Me.GetParseTreeTokens(lr_object, Me.Parsetree.Nodes(0))

                    ''---------------------------------------------------------
                    ''Find the FactType that the ADD FACT statement is for.
                    ''---------------------------------------------------------
                    'lrFactType = New FBM.FactType(lr_object.USERTABLENAME, True)
                    'lrFactType = Me.Model.FactType.Find(AddressOf lrFactType.EqualsByName)

                    'If lrFactType Is Nothing Then
                    '    Me.Parsetree.Errors.Add(New TinyPG.ParseError("Error: tModel.ProcessORMQLStatement: Can't find FactType with Name: '" & lr_object.USERTABLENAME(0).ToString & "' within the Model.", 100, Nothing))
                    '    Return Me.Parsetree.Errors
                    '    Exit Function
                    'End If

                    ''-----------------
                    ''Create the Fact
                    ''-----------------
                    'lrFact = New FBM.Fact(lr_object.VALUE, lrFactType)

                    'lrFact = lrFactType.Fact.Find(AddressOf lrFact.EqualsById)

                    'If lrFact Is Nothing Then
                    '    Me.Parsetree.Errors.Add(New TinyPG.ParseError("Error: tModel.ProcessORMQLStatement: Can't find Fact with Id: '" & lr_object.VALUE & "' within the Model level FactType.Id :" & lr_object.USERTABLENAME, 100, Nothing))
                    '    Return Me.Parsetree.Errors
                    '    Exit Function
                    'End If

                    'Dim lrPage As New FBM.Page(Me.Model, lr_object.PAGENAME, lr_object.PAGENAME, pcenumLanguage.ORMModel)
                    'lrPage = Me.Model.Page.Find(AddressOf lrPage.EqualsByName)

                    'If IsSomething(lrPage) Then
                    '    Dim lrFactTypeInstance As FBM.FactTypeInstance = lrFactType.CloneInstance(lrPage)
                    '    lrFactTypeInstance = lrPage.FactTypeInstance.Find(AddressOf lrFactTypeInstance.Equals)
                    '    If IsSomething(lrFactTypeInstance) Then
                    '        lrFactTypeInstance.AddFact(lrFact)
                    '    End If
                    'Else
                    '    Me.Parsetree.Errors.Add(New TinyPG.ParseError("Error: tModel.ProcessORMQLStatement: Can't find Page with Name: '" & lr_object.PageID(0).ToString & "'.", 100, Nothing))
                    '    Return Me.Parsetree.Errors
                    'End If

                    'Return lrFact
                Case Else
                    Return Nothing
            End Select


        End Function

        Public Function ProcessORMQLStatement(ByVal as_ORMQL_statement As String) As Object

            Dim lrFact As New FBM.Fact

            Try
                '---------------------------
                'Parse the ORMQR statement
                '---------------------------
                SyncLock Me.Parsetree
                    Me.Parsetree = Me.Parser.Parse(as_ORMQL_statement)

                    Select Case Me.Parsetree.Nodes(0).Nodes(0).Text
                        Case Is = "SELECTSTMT"

                            '=============================================================
                            Dim lrselectStatement As ORMQL.SelectStatement
                            lrselectStatement = prApplication.ORMQL.SelectStatement

                            lrselectStatement.COLUMNLIST = New List(Of Object)
                            lrselectStatement.KEYWDDISTINCT = New Object
                            lrselectStatement.COLUMNNAMESTR = New List(Of String)
                            lrselectStatement.MODELID = Nothing
                            lrselectStatement.USERTABLENAME = Nothing
                            lrselectStatement.PAGENAME = Nothing
                            lrselectStatement.WHERESTMT = New Object

                            '----------------------------------
                            'Get the Tokens from the ParseTree
                            '----------------------------------
                            Call Me.GetParseTreeTokensReflection(lrselectStatement, Me.Parsetree.Nodes(0))

                            If lrselectStatement.COLUMNLIST.Count = 1 And lrselectStatement.COLUMNNAMESTR.Count = 1 Then
                                If lrselectStatement.COLUMNNAMESTR(0) = "INSTANCES" Then
                                    Dim lrRecordset = Me.ProcessSelectINSTANCESStatement(as_ORMQL_statement)
                                    Return lrRecordset
                                Else
                                    Dim lrRecordset = Me.ProcessSelectStatement(as_ORMQL_statement)
                                    Return lrRecordset
                                End If
                            Else
                                Dim lrRecordset = Me.ProcessSelectStatement(as_ORMQL_statement)
                                Return lrRecordset
                            End If

                            '----------------------------------------------------------------------------------
                            'Exit the sub because have found what the User was trying to do, and have done it 
                            '----------------------------------------------------------------------------------
                            Exit Function
                        Case Is = "INSERTSTMT"

                            lrFact = Me.ProcessINSERTStatement(as_ORMQL_statement)

                            '---------------------------------------------------------------------------
                            'Exit the sub because have found what the User was
                            '  trying to do, and have done it (i.e. Inserted a new Fact in the FactType
                            '---------------------------------------------------------------------------
                            Return lrFact
                            Exit Function
                        Case Is = "ADDMODELELEMENTTOPAGESTMT"


                            Return True
                        Case Is = "ADDFACT"

                            Return Me.ProcessADDFACTStatement()

                        Case Is = "DELETESTMT"

                            Return Me.ProcessDELETEStatement

                        Case Is = "DELETEFACTSTMT"

                            Return Me.ProcessDELETEFACTSTMTStatement

                        Case Is = "DELETEALLSTMT"
                            '--------------------------------------------
                            'Delete all Facts from a FactType
                            '--------------------------------------------

                            Return Me.ProcessDELETEALLStatement

                        Case Is = "CREATEOBJSTMT"

                            Return Me.ProcessCREATEOBJECTStatement

                        Case Is = "ADDMODELELEMENTSTMT"

                            Return Me.ProcessADDMODELELEMENTStatement

                        Case Is = "REMOVEINSTANCESTMT"

                            Return Me.ProcessREMOVEINSTANCEStatement

                        Case Is = "RENAMEINSTANCESTMT"

                            Return Me.ProcessRENAMEINSTANCEStatement

                        Case Is = "UPDATESTMT"

                            Return Me.processUPDATEStatement

                        Case Else
                            Return False

                    End Select

                End SyncLock

            Catch ex As Exception
                'Me.parsetree.Errors.Add(New TinyPG.ParseError("Error: tModel.ProcessORMQLStatement: " & ex.Message, 100, lrCustomClass))
                'Return Me.parsetree.Errors
                If IsSomething(ex.InnerException) Then
                    Return ex.Message & vbCrLf & "Inner Exception" & vbCrLf & ex.InnerException.Message & vbCrLf & vbCrLf & "Stack Trace" & vbCrLf & ex.StackTrace
                Else
                    Return ex.Message & vbCrLf & "Inner Exception" & vbCrLf & vbCrLf & "Stack Trace" & vbCrLf & ex.StackTrace
                End If


            End Try

        End Function


    End Class

End Namespace
