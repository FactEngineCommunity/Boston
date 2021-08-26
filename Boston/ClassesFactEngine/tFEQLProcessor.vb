Imports System.Reflection

Namespace FEQL
    Public Class ASSERTStatement

        Private _NODEPROPERTYNAMEIDENTIFICATION As New List(Of NODEPROPERTYNAMEIDENTIFICATION)
        Public Property NODEPROPERTYNAMEIDENTIFICATION As List(Of NODEPROPERTYNAMEIDENTIFICATION)
            Get
                Return Me._NODEPROPERTYNAMEIDENTIFICATION
            End Get
            Set(value As List(Of NODEPROPERTYNAMEIDENTIFICATION))
                Me._NODEPROPERTYNAMEIDENTIFICATION = value
            End Set
        End Property

        Private _PREDICATECLAUSE As FEQL.PREDICATECLAUSE
        Public Property PREDICATECLAUSE As FEQL.PREDICATECLAUSE
            Get
                Return Me._PREDICATECLAUSE
            End Get
            Set(value As FEQL.PREDICATECLAUSE)
                Me._PREDICATECLAUSE = value
            End Set
        End Property

        Private _NODEPROPERTYIDENTIFICATION As New List(Of FEQL.NODEPROPERTYIDENTIFICATION)
        Public Property NODEPROPERTYIDENTIFICATION As List(Of FEQL.NODEPROPERTYIDENTIFICATION)
            Get
                Return Me._NODEPROPERTYIDENTIFICATION
            End Get
            Set(value As List(Of FEQL.NODEPROPERTYIDENTIFICATION))
                Me._NODEPROPERTYIDENTIFICATION = value
            End Set
        End Property

    End Class

    Public Class CREATEStatement

        Private _NODEPROPERTYNAMEIDENTIFICATION As FEQL.NODEPROPERTYNAMEIDENTIFICATION
        Public Property NODEPROPERTYNAMEIDENTIFICATION As FEQL.NODEPROPERTYNAMEIDENTIFICATION
            Get
                Return Me._NODEPROPERTYNAMEIDENTIFICATION
            End Get
            Set(value As FEQL.NODEPROPERTYNAMEIDENTIFICATION)
                Me._NODEPROPERTYNAMEIDENTIFICATION = value
            End Set
        End Property

        Private _PREDICATENODEPROPERTYIDENTIFICATION As New List(Of FEQL.PREDICATENODEPROPERTYIDENTIFICATION)
        Public Property PREDICATENODEPROPERTYIDENTIFICATION As List(Of FEQL.PREDICATENODEPROPERTYIDENTIFICATION)
            Get
                Return Me._PREDICATENODEPROPERTYIDENTIFICATION
            End Get
            Set(value As List(Of FEQL.PREDICATENODEPROPERTYIDENTIFICATION))
                Me._PREDICATENODEPROPERTYIDENTIFICATION = value
            End Set
        End Property

    End Class

    Public Class CREATEDATABASEStatement

        Private _DATABASENAME As String
        Public Property DATABASENAME As String
            Get
                Return Me._DATABASENAME
            End Get
            Set(value As String)
                Me._DATABASENAME = value
            End Set
        End Property

        Private _FILELOCATIONNAME As String
        Public Property FILELOCATIONNAME As String
            Get
                Return Me._FILELOCATIONNAME
            End Get
            Set(value As String)
                Me._FILELOCATIONNAME = value
            End Set
        End Property

    End Class

    Public Class EntityTypeIsIdentifiedByItsStatement

        <DebuggerBrowsable(DebuggerBrowsableState.Never)>
        Private _KEYWDISIDENTIFIEDBYITS As String = Nothing
        Public Property KEYWDISIDENTIFIEDBYITS As String
            Get
                Return Me._KEYWDISIDENTIFIEDBYITS
            End Get
            Set(value As String)
                Me._KEYWDISIDENTIFIEDBYITS = value
            End Set
        End Property

        <DebuggerBrowsable(DebuggerBrowsableState.Never)>
        Private _MODELELEMENTNAME As String
        Public Property MODELELEMENTNAME As String
            Get
                Return Me._MODELELEMENTNAME
            End Get
            Set(value As String)
                Me._MODELELEMENTNAME = value
            End Set
        End Property

        <DebuggerBrowsable(DebuggerBrowsableState.Never)>
        Private _REFERENCEMODE As String
        Public Property REFERENCEMODE As String
            Get
                Return Me._REFERENCEMODE
            End Get
            Set(value As String)
                Me._REFERENCEMODE = value
            End Set
        End Property

        <DebuggerBrowsable(DebuggerBrowsableState.Never)>
        Private _KEYWDWRITTENAS As String = Nothing
        Public Property KEYWDWRITTENAS As String
            Get
                Return Me._KEYWDWRITTENAS
            End Get
            Set(value As String)
                Me._KEYWDWRITTENAS = value
            End Set
        End Property

        <DebuggerBrowsable(DebuggerBrowsableState.Never)>
        Private _VALUETYPEWRITTENASCLAUSE As New FEQL.ValueTypeWrittenAsClause
        Public Property VALUETYPEWRITTENASCLAUSE As FEQL.ValueTypeWrittenAsClause
            Get
                Return Me._VALUETYPEWRITTENASCLAUSE
            End Get
            Set(value As FEQL.ValueTypeWrittenAsClause)
                Me._VALUETYPEWRITTENASCLAUSE = value
            End Set
        End Property

    End Class

    Public Class PROPERTYIDENTIFIER

        Private _COLUMNNAMESTR As String = Nothing
        Public Property COLUMNNAMESTR As String
            Get
                Return Me._COLUMNNAMESTR
            End Get
            Set(value As String)
                Me._COLUMNNAMESTR = value
            End Set
        End Property

        Private _SINGLEQUOTE As String = ""
        Public Property SINGLEQUOTE As String
            Get
                Return Me._SINGLEQUOTE
            End Get
            Set(value As String)
                Me._SINGLEQUOTE = value
            End Set
        End Property

        Private _IDENTIFIER As String = Nothing
        Public Property IDENTIFIER As String
            Get
                Return Me._IDENTIFIER
            End Get
            Set(value As String)
                Me._IDENTIFIER = value
            End Set
        End Property

        Private _EMAILADDRESS As String = Nothing
        Public Property EMAILADDRESS As String
            Get
                Return Me._EMAILADDRESS
            End Get
            Set(value As String)
                Me._EMAILADDRESS = value
            End Set
        End Property

        Public ReadOnly Property ID As String
            Get
                If Me._IDENTIFIER IsNot Nothing Then
                    Return Me._IDENTIFIER
                Else
                    Return Me._EMAILADDRESS
                End If
            End Get
        End Property

    End Class

    Public Class PREDICATENODEPROPERTYIDENTIFICATION

        Private _PREDICATECLAUSE As FEQL.PREDICATECLAUSE
        Public Property PREDICATECLAUSE As FEQL.PREDICATECLAUSE
            Get
                Return Me._PREDICATECLAUSE
            End Get
            Set(value As FEQL.PREDICATECLAUSE)
                Me._PREDICATECLAUSE = value
            End Set
        End Property

        Private _NODE As FEQL.NODE 'PROPERTYIDENTIFICATION As NODEPROPERTYIDENTIFICATION
        Public Property NODE As FEQL.NODE 'PROPERTYIDENTIFICATION As NODEPROPERTYIDENTIFICATION
            Get
                Return Me._NODE 'PROPERTYIDENTIFICATION
            End Get
            Set(value As NODE) 'PROPERTYIDENTIFICATION)
                Me._NODE = value 'PROPERTYIDENTIFICATION = value
            End Set
        End Property

    End Class

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

    Public Class RECURSIVECLAUSE

        Private _NUMBER1 As String = Nothing
        Public Property NUMBER1 As String
            Get
                Return Me._NUMBER1
            End Get
            Set(value As String)
                Me._NUMBER1 = value
            End Set
        End Property

        Private _NUMBER2 As String = Nothing
        Public Property NUMBER2 As String
            Get
                Return Me._NUMBER2
            End Get
            Set(value As String)
                Me._NUMBER2 = value
            End Set
        End Property

        Private _NUMBER As New List(Of String)
        Public Property NUMBER As List(Of String)
            Get
                Return Me._NUMBER
            End Get
            Set(value As List(Of String))
                Me._NUMBER = value
            End Set
        End Property

        Private _KEYWDCIRCULAR As String = Nothing
        Public Property KEYWDCIRCULAR As String
            Get
                Return Me._KEYWDCIRCULAR
            End Get
            Set(value As String)
                Me._KEYWDCIRCULAR = value
            End Set
        End Property

        Private _KEYWDSHORTESTPATH As String = Nothing
        Public Property KEYWDSHORTESTPATH As String
            Get
                Return Me._KEYWDSHORTESTPATH
            End Get
            Set(value As String)
                Me._KEYWDSHORTESTPATH = value
            End Set
        End Property

    End Class

    Public Class RETURNCOLUMN

        Private _MODELELEMENTNAME As String = Nothing
        Public Property MODELELEMENTNAME As String
            Get
                Return Me._MODELELEMENTNAME
            End Get
            Set(value As String)
                Me._MODELELEMENTNAME = value
            End Set
        End Property

        Private _KEYWDCOUNTSTAR As String = Nothing
        Public Property KEYWDCOUNTSTAR As String
            Get
                Return Me._KEYWDCOUNTSTAR
            End Get
            Set(value As String)
                Me._KEYWDCOUNTSTAR = value
            End Set
        End Property

        Private _MODELELEMENTSUFFIX As String = Nothing
        Public Property MODELELEMENTSUFFIX As String
            Get
                Return Me._MODELELEMENTSUFFIX
            End Get
            Set(value As String)
                Me._MODELELEMENTSUFFIX = value
            End Set
        End Property

        Private _COLUMNNAMESTR As String = Nothing
        Public Property COLUMNNAMESTR As String
            Get
                Return Me._COLUMNNAMESTR
            End Get
            Set(value As String)
                Me._COLUMNNAMESTR = value
            End Set
        End Property

    End Class

    Public Class RETURNCLAUSE

        Private _KEYWDDISTINCT As String = Nothing
        Public Property KEYWDDISTINCT As String
            Get
                Return Me._KEYWDDISTINCT
            End Get
            Set(value As String)
                Me._KEYWDDISTINCT = value
            End Set
        End Property

        Private _RETURNCOLUMN As New List(Of RETURNCOLUMN)
        Public Property RETURNCOLUMN As List(Of RETURNCOLUMN)
            Get
                Return Me._RETURNCOLUMN
            End Get
            Set(value As List(Of RETURNCOLUMN))
                Me._RETURNCOLUMN = value
            End Set
        End Property

    End Class

    Public Class SHOWStatement

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

    Public Class MODELELEMENTClause

        Private _MODELELEMENTNAME As String = Nothing
        Public Property MODELELEMENTNAME As String
            Get
                Return Me._MODELELEMENTNAME
            End Get
            Set(value As String)
                Me._MODELELEMENTNAME = value
            End Set
        End Property

        Private _MODELELEMENTSUFFIX As String = Nothing
        Public Property MODELELEMENTSUFFIX As String
            Get
                Return Me._MODELELEMENTSUFFIX
            End Get
            Set(value As String)
                Me._MODELELEMENTSUFFIX = value
            End Set
        End Property

    End Class

    Public Class WITHClause

        Private _KEYWDWITH As String = Nothing
        Public Property KEYWDWITH As String
            Get
                Return Me._KEYWDWITH
            End Get
            Set(value As String)
                Me._KEYWDWITH = value
            End Set
        End Property

        Private _KEYWDWHAT As String = Nothing
        Public Property KEYWDWHAT As String
            Get
                Return Me._KEYWDWHAT
            End Get
            Set(value As String)
                Me._KEYWDWHAT = value
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

    Public Class WHICHSELECTStatement

        Private _NODE As New List(Of FEQL.NODE)
        Public Property NODE As List(Of FEQL.NODE)
            Get
                Return Me._NODE
            End Get
            Set(value As List(Of FEQL.NODE))
                Me._NODE = value
            End Set
        End Property

        ''' <summary>
        ''' If the WHICHSELECTSTMT begins with a NodePropertyIdentification, then is populated.
        ''' </summary>
        Private _NODEPROPERTYIDENTIFICATION As New List(Of NODEPROPERTYIDENTIFICATION)
        Public Property NODEPROPERTYIDENTIFICATION As List(Of NODEPROPERTYIDENTIFICATION)
            Get
                Return Me._NODEPROPERTYIDENTIFICATION
            End Get
            Set(value As List(Of NODEPROPERTYIDENTIFICATION))
                Me._NODEPROPERTYIDENTIFICATION = value
            End Set
        End Property

        Private _MODELELEMENT As New List(Of Object)
        Public Property MODELELEMENT As List(Of Object)
            Get
                Return Me._MODELELEMENT
            End Get
            Set(value As List(Of Object))
                Me._MODELELEMENT = value
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

        Private _WHICHCLAUSE As New List(Of Object)
        Public Property WHICHCLAUSE As List(Of Object)
            Get
                Return Me._WHICHCLAUSE
            End Get
            Set(value As List(Of Object))
                Me._WHICHCLAUSE = value
            End Set
        End Property

        Private _RETURNCLAUSE As RETURNCLAUSE
        Public Property RETURNCLAUSE As RETURNCLAUSE
            Get
                Return Me._RETURNCLAUSE
            End Get
            Set(value As RETURNCLAUSE)
                Me._RETURNCLAUSE = value
            End Set
        End Property

    End Class

    Public Class MATHCLAUSE

        Private _MATHFUNCTION As String = Nothing
        Public Property MATHFUNCTION As String
            Get
                Return Me._MATHFUNCTION
            End Get
            Set(value As String)
                Me._MATHFUNCTION = value
            End Set
        End Property

        Private _NUMBER As String = Nothing

        Public Property [NUMBER] As String
            Get
                Return Me._NUMBER
            End Get
            Set(value As String)
                Me._NUMBER = value
            End Set
        End Property


        Private _MODELELEMENT As MODELELEMENTClause = Nothing
        Public Property MODELELEMENT As MODELELEMENTClause
            Get
                Return Me._MODELELEMENT
            End Get
            Set(value As MODELELEMENTClause)
                Me._MODELELEMENT = value
            End Set
        End Property

    End Class

    Public Class ValueTypeWrittenAsClause

        Private _DATATYPE As Object
        Public Property DATATYPE As Object
            Get
                Return Me._DATATYPE
            End Get
            Set(value As Object)
                Me._DATATYPE = value
            End Set
        End Property

        Private _DATATYPELENGTH As Object
        Public Property DATATYPELENGTH As Object
            Get
                Return Me._DATATYPELENGTH
            End Get
            Set(value As Object)
                Me._DATATYPELENGTH = value
            End Set
        End Property

        Private _DATATYPEPRECISION As Object
        Public Property DATATYPEPRECISION As Object
            Get
                Return Me._DATATYPEPRECISION
            End Get
            Set(value As Object)
                Me._DATATYPEPRECISION = value
            End Set
        End Property

        Private _NUMBER As String
        Public Property NUMBER As String
            Get
                Return Me._NUMBER
            End Get
            Set(value As String)
                Me._NUMBER = value
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

        Private _KEYWDISNOT As String = Nothing
        Public Property KEYWDISNOT As String
            Get
                Return Me._KEYWDISNOT
            End Get
            Set(value As String)
                Me._KEYWDISNOT = value
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

        Private _KEYWDAN As String = Nothing
        Public Property KEYWDAN As String
            Get
                Return Me._KEYWDAN
            End Get
            Set(value As String)
                Me._KEYWDAN = value
            End Set
        End Property

        Private _KEYWDNO As String = Nothing
        Public Property KEYWDNO As String
            Get
                Return Me._KEYWDNO
            End Get
            Set(value As String)
                Me._KEYWDNO = value
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

        Private _KEYWDWHEREALSO As String = Nothing
        Public Property KEYWDWHEREALSO As String
            Get
                Return Me._KEYWDWHEREALSO
            End Get
            Set(value As String)
                Me._KEYWDWHEREALSO = value
                Me.KEYWDAND = "AND" 'Because the FEQL processing is based on KEYWDAND, and does not include checks for KEYWDALSO.
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

        Private _MODELELEMENT As New List(Of Object)
        Public Property MODELELEMENT As List(Of Object)
            Get
                Return Me._MODELELEMENT
            End Get
            Set(value As List(Of Object))
                Me._MODELELEMENT = value
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

        Private _NODE As New List(Of NODE)

        Public Property NODE As List(Of NODE)
            Get
                Return Me._NODE
            End Get
            Set(value As List(Of NODE))
                Me._NODE = value
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

        Private _WITHCLAUSE As Object = Nothing 'NB Is used to disambiguate where the THAT is in the WHICHCLAUSE
        Public Property WITHCLAUSE As Object
            Get
                Return Me._WITHCLAUSE
            End Get
            Set(value As Object)
                Me._WITHCLAUSE = value
            End Set
        End Property

        Private _MATHCLAUSE As MATHCLAUSE = Nothing
        Public Property MATHCLAUSE As MATHCLAUSE
            Get
                Return Me._MATHCLAUSE
            End Get
            Set(value As MATHCLAUSE)
                Me._MATHCLAUSE = value
            End Set
        End Property

        Private _RECURSIVECLAUSE As RECURSIVECLAUSE = Nothing
        Public Property RECURSIVECLAUSE As RECURSIVECLAUSE
            Get
                Return Me._RECURSIVECLAUSE
            End Get
            Set(value As RECURSIVECLAUSE)
                Me._RECURSIVECLAUSE = value
            End Set
        End Property

    End Class

    Public Class NODE

        Private _MODELELEMENT As MODELELEMENTClause
        Public Property MODELELEMENT As MODELELEMENTClause
            Get
                Return Me._MODELELEMENT
            End Get
            Set(value As MODELELEMENTClause)
                Me._MODELELEMENT = value
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

        Private _MODELELEMENTSUFFIX As String = Nothing
        Public Property MODELELEMENTSUFFIX As String
            Get
                Return Me._MODELELEMENTSUFFIX
            End Get
            Set(value As String)
                Me._MODELELEMENTSUFFIX = value
            End Set
        End Property

        Private _PREBOUNDREADINGTEXT As String = Nothing
        Public Property PREBOUNDREADINGTEXT As String
            Get
                Return Me._PREBOUNDREADINGTEXT
            End Get
            Set(value As String)
                Me._PREBOUNDREADINGTEXT = value
            End Set
        End Property

        Private _POSTBOUNDREADINGTEXT As String = Nothing
        Public Property POSTBOUNDREADINGTEXT As String
            Get
                Return Me._POSTBOUNDREADINGTEXT
            End Get
            Set(value As String)
                Me._POSTBOUNDREADINGTEXT = value
            End Set
        End Property

        Private _NODEPROPERTYIDENTIFICATION As NODEPROPERTYIDENTIFICATION
        Public Property NODEPROPERTYIDENTIFICATION As NODEPROPERTYIDENTIFICATION
            Get
                Return Me._NODEPROPERTYIDENTIFICATION
            End Get
            Set(value As NODEPROPERTYIDENTIFICATION)
                Me._NODEPROPERTYIDENTIFICATION = value
            End Set
        End Property

    End Class

    Public Class NODEPROPERTYIDENTIFICATION

        Public Function getComparitorType() As pcenumFEQLComparitor

            If Me.BANG IsNot Nothing Then
                Return pcenumFEQLComparitor.Bang
            ElseIf Me.CARRET IsNot Nothing Then
                Return pcenumFEQLComparitor.Carret
            ElseIf Me.COLON IsNot Nothing Then
                Return pcenumFEQLComparitor.Colon
            ElseIf Me.LIKECOMPARITOR IsNot Nothing Then
                Return pcenumFEQLComparitor.LikeComparitor
            End If

        End Function

        Private _BANG As String = Nothing
        Public Property BANG As String
            Get
                Return Me._BANG
            End Get
            Set(value As String)
                Me._BANG = value
            End Set
        End Property

        Private _CARRET As String = Nothing
        Public Property CARRET As String
            Get
                Return Me._CARRET
            End Get
            Set(value As String)
                Me._CARRET = value
            End Set
        End Property

        Private _COLON As String = Nothing
        Public Property COLON As String
            Get
                Return Me._COLON
            End Get
            Set(value As String)
                Me._COLON = value
            End Set
        End Property

        Private _LIKECOMPARITOR As String = Nothing
        Public Property LIKECOMPARITOR As String
            Get
                Return Me._LIKECOMPARITOR
            End Get
            Set(value As String)
                Me._LIKECOMPARITOR = value
            End Set
        End Property


        Private _MODELELEMENT As New Object
        Public Property MODELELEMENT As Object
            Get
                Return Me._MODELELEMENT
            End Get
            Set(value As Object)
                Me._MODELELEMENT = value
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

        Private _IDENTIFIER As New List(Of String)
        Public Property IDENTIFIER As List(Of String)
            Get
                Return Me._IDENTIFIER
            End Get
            Set(value As List(Of String))
                Me._IDENTIFIER = value
            End Set
        End Property

        Private _QUOTEDIDENTIFIERLIST As FEQL.QuotedIdentifierList
        Public Property QUOTEDIDENTIFIERLIST As FEQL.QuotedIdentifierList
            Get
                Return Me._QUOTEDIDENTIFIERLIST
            End Get
            Set(value As FEQL.QuotedIdentifierList)
                Me._QUOTEDIDENTIFIERLIST = value
            End Set
        End Property

        Private _MODELELEMENTSUFFIX As String = Nothing
        Public Property MODELELEMENTSUFFIX As String
            Get
                Return Me._MODELELEMENTSUFFIX
            End Get
            Set(value As String)
                Me._MODELELEMENTSUFFIX = value
            End Set
        End Property

    End Class

    Public Class NODEPROPERTYNAMEIDENTIFICATION

        Private _COLON As String = Nothing
        Public Property COLON As String
            Get
                Return Me._COLON
            End Get
            Set(value As String)
                Me._COLON = value
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

        Private _QUOTEDPROPERTYIDENTIFIERLIST As FEQL.QuotedPropertyIdentifierList
        Public Property QUOTEDPROPERTYIDENTIFIERLIST As FEQL.QuotedPropertyIdentifierList
            Get
                Return Me._QUOTEDPROPERTYIDENTIFIERLIST
            End Get
            Set(value As FEQL.QuotedPropertyIdentifierList)
                Me._QUOTEDPROPERTYIDENTIFIERLIST = value
            End Set
        End Property

        Private _QUOTEDIDENTIFIERLIST As FEQL.QuotedIdentifierList
        Public Property QUOTEDIDENTIFIERLIST As FEQL.QuotedIdentifierList
            Get
                Return Me._QUOTEDIDENTIFIERLIST
            End Get
            Set(value As FEQL.QuotedIdentifierList)
                Me._QUOTEDIDENTIFIERLIST = value
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

    Public Class QuotedIdentifier

        Private _IDENTIFIER As String = Nothing
        Public Property IDENTIFIER As String
            Get
                Return Me._IDENTIFIER
            End Get
            Set(value As String)
                Me._IDENTIFIER = value
            End Set
        End Property

        Private _EMAILADDRESS As String = Nothing
        Public Property EMAILADDRESS As String
            Get
                Return Me._EMAILADDRESS
            End Get
            Set(value As String)
                Me._EMAILADDRESS = value
            End Set
        End Property

        Public ReadOnly Property ActualIdentifier As String
            Get
                If Me.EMAILADDRESS IsNot Nothing Then
                    Return Me.EMAILADDRESS
                Else
                    Return Me.IDENTIFIER
                End If
            End Get
        End Property


        Private _SINGLEQUOTE As String = ""
        Public Property SINGLEQUOTE As String
            Get
                Return Me._SINGLEQUOTE
            End Get
            Set(value As String)
                Me._SINGLEQUOTE = value
            End Set
        End Property

    End Class

    Public Class QuotedIdentifierList

        Private _COLON As String
        Public Property COLON As String
            Get
                Return Me._COLON
            End Get
            Set(value As String)
                Me._COLON = value
            End Set
        End Property

        Private _QUOTEDIDENTIFIER As New List(Of QuotedIdentifier)
        Public Property QUOTEDIDENTIFIER As List(Of QuotedIdentifier)
            Get
                Return Me._QUOTEDIDENTIFIER
            End Get
            Set(value As List(Of QuotedIdentifier))
                Me._QUOTEDIDENTIFIER = value
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

        Private _SINGLEQUOTE As String = ""
        Public Property SINGLEQUOTE As String
            Get
                Return Me._SINGLEQUOTE
            End Get
            Set(value As String)
                Me._SINGLEQUOTE = value
            End Set
        End Property

    End Class

    Public Class QuotedPropertyIdentifierList

        Private _PROPERTYIDENTIFIER As New List(Of PROPERTYIDENTIFIER)
        Public Property PROPERTYIDENTIFIER As List(Of PROPERTYIDENTIFIER)
            Get
                Return Me._PROPERTYIDENTIFIER
            End Get
            Set(value As List(Of PROPERTYIDENTIFIER))
                Me._PROPERTYIDENTIFIER = value
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

    Public Class PREDICATECLAUSE

        Private _PREDICATE As New List(Of String)
        Public Property PREDICATE As List(Of String)
            Get
                Return Me._PREDICATE
            End Get
            Set(value As List(Of String))
                Me._PREDICATE = value
            End Set
        End Property

    End Class

    Public Class FACTTYPEPRODUCTION
        Private _MODELELEMENT As New List(Of Object)
        Public Property MODELELEMENT As List(Of Object)
            Get
                Return Me._MODELELEMENT
            End Get
            Set(value As List(Of Object))
                Me._MODELELEMENT = value
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

        Private _PREDICATECLAUSE As List(Of FEQL.PREDICATECLAUSE)
        Public Property PREDICATECLAUSE As List(Of FEQL.PREDICATECLAUSE)
            Get
                Return Me._PREDICATECLAUSE
            End Get
            Set(value As List(Of FEQL.PREDICATECLAUSE))
                Me._PREDICATECLAUSE = value
            End Set
        End Property

        'Private _DERIVATIONCLAUSE
    End Class

    Public Class DERIVATIONSTMT

        Private _FACTREADING As FEQL.FACTREADINGClause
        Public Property FACTREADING As FACTREADINGClause
            Get
                Return Me._FACTREADING
            End Get
            Set(value As FACTREADINGClause)
                Me._FACTREADING = value
            End Set
        End Property

        Private _KEYWDISWHERE As String = Nothing
        Public Property KEYWDISWHERE As String
            Get
                Return Me._KEYWDISWHERE
            End Get
            Set(value As String)
                Me._KEYWDISWHERE = value
            End Set
        End Property



        Private _KEYWDCOUNT As String = Nothing
        Public Property KEYWDCOUNT As String
            Get
                Return Me._KEYWDCOUNT
            End Get
            Set(value As String)
                Me._KEYWDCOUNT = value
            End Set
        End Property

        Private _DERIVATIONSUBCLAUSE As New List(Of DERIVATIONSUBCLAUSE)

        Public Property DERIVATIONSUBCLAUSE As List(Of DERIVATIONSUBCLAUSE)
            Get
                Return Me._DERIVATIONSUBCLAUSE
            End Get
            Set(value As List(Of DERIVATIONSUBCLAUSE))
                Me._DERIVATIONSUBCLAUSE = value
            End Set
        End Property

    End Class

    Public Class DERIVATIONSUBCLAUSE

        Public FBMFactType As FBM.FactType = Nothing

        Public Function isFactTypeOnly() As Boolean

            isFactTypeOnly = True

            Return Me.FACTREADING IsNot Nothing

            'If Me.KEYWDCOUNT IsNot Nothing Or
            '   Me.EXPRESSION.Count > 0 Or
            '   Me.MATHCLAUSE.Count > 0 Then
            '    isFactTypeOnly = False
            'End If

            ''for now
            'If Me.KEYWDCOUNT IsNot Nothing Then
            '    isFactTypeOnly = True
            'End If

        End Function

        Private _FACTREADING As FEQL.FACTREADINGClause
        Public Property FACTREADING As FACTREADINGClause
            Get
                Return Me._FACTREADING
            End Get
            Set(value As FACTREADINGClause)
                Me._FACTREADING = value
            End Set
        End Property

        Private _DERIVATIONFORMULA As Object
        Public Property DERIVATIONFORMULA As Object
            Get
                Return Me._DERIVATIONFORMULA
            End Get
            Set(value As Object)
                Me._DERIVATIONFORMULA = value
            End Set
        End Property


        'Private _MODELELEMENT As New List(Of MODELELEMENTClause)
        'Public Property MODELELEMENT As List(Of MODELELEMENTClause)
        '    Get
        '        Return Me._MODELELEMENT
        '    End Get
        '    Set(value As List(Of MODELELEMENTClause))
        '        Me._MODELELEMENT = value
        '    End Set
        'End Property

        'Private _MODELELEMENTNAME As New List(Of String)
        'Public Property MODELELEMENTNAME As List(Of String)
        '    Get
        '        Return Me._MODELELEMENTNAME
        '    End Get
        '    Set(value As List(Of String))
        '        Me._MODELELEMENTNAME = value
        '    End Set
        'End Property

        'Private _PREDICATECLAUSE As New List(Of FEQL.PREDICATECLAUSE)
        'Public Property PREDICATECLAUSE As List(Of FEQL.PREDICATECLAUSE)
        '    Get
        '        Return Me._PREDICATECLAUSE
        '    End Get
        '    Set(value As List(Of FEQL.PREDICATECLAUSE))
        '        Me._PREDICATECLAUSE = value
        '    End Set
        'End Property

        'Private _KEYWDCOUNT As String = Nothing
        'Public Property KEYWDCOUNT As String
        '    Get
        '        Return Me._KEYWDCOUNT
        '    End Get
        '    Set(value As String)
        '        Me._KEYWDCOUNT = value
        '    End Set
        'End Property

        'Private _KEYWDEQUALS As String = Nothing
        'Public Property KEYWDEQUALS As String
        '    Get
        '        Return Me._KEYWDEQUALS
        '    End Get
        '    Set(value As String)
        '        Me._KEYWDEQUALS = value
        '    End Set
        'End Property

        'Private _EXPRESSION As New List(Of FEQL.EXPRESSION)
        'Public Property EXPRESSION As List(Of FEQL.EXPRESSION)
        '    Get
        '        Return Me._EXPRESSION
        '    End Get
        '    Set(value As List(Of FEQL.EXPRESSION))
        '        Me._EXPRESSION = value
        '    End Set
        'End Property

        'Private _MATHCLAUSE As New List(Of FEQL.MATHCLAUSE)
        'Public Property MATHCLAUSE As List(Of MATHCLAUSE)
        '    Get
        '        Return Me._MATHCLAUSE
        '    End Get
        '    Set(value As List(Of MATHCLAUSE))
        '        Me._MATHCLAUSE = value
        '    End Set
        'End Property

    End Class

    Public Class EXPRESSION

        Private _EXPRESSIONSYMBOL As String
        Public Property EXPRESSIONSYMBOL As String
            Get
                Return Me._EXPRESSIONSYMBOL
            End Get
            Set(value As String)
                Me._EXPRESSIONSYMBOL = value
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

    End Class

    'VALUETYPEISWRITTENASSTMT 
    'REFERENCEMODECLAUSE                           
    'CONSTRAINTMANDATORY
    'KEYWDISWHERE FACTTYPESTMT(COMMA FACTTYPESTMT)*
    '(BINARYFACTTYPECLAUSE
    'BINARYFACTTYPEMANYTOONEDEFINITIONSTMT

    Public Class Processor

        Public Model As FBM.Model

        ''' <summary>
        ''' The database manager that handles connection and queries/commands to the database.
        ''' </summary>
        Public DatabaseManager As New FactEngine.DatabaseManager

        Private Parser As New FEQL.Parser(New FEQL.Scanner) 'Used to parse Text input into the Brain; especially for ORMQL.
        Private Parsetree As New FEQL.ParseTree 'Used with the Parser, is populated during the parsing of text input into the Brain; especially ORMQL

        Public CREATEStatement As New FEQL.CREATEStatement
        Public DESCRIBEStatement As New FEQL.DESCRIBEStatement
        Public ENUMMERATEStatement As New FEQL.ENUMERATEStatement
        Public SHOWStatement As New FEQL.SHOWStatement
        Public WHICHSELECTStatement As New FEQL.WHICHSELECTStatement
        Public WHICHCLAUSE As New FEQL.WHICHCLAUSE
        Public MODELELEMENTCLAUSE As New FEQL.MODELELEMENTClause
        'Public NODEPROPERTYIDENTIFICATION As New FEQL.NODEPROPERTYIDENTIFICATION '20200822-Changed to Node, and Node is automatically retrieved from the ParseNodes.
        Public WITHCLAUSE As New FEQL.WITHClause

        ''' <summary>
        ''' Parameterless NEw
        ''' </summary>
        Public Sub New()
        End Sub

        Public Sub New(ByRef arModel As FBM.Model)
            Me.Model = arModel
            Me.DatabaseManager.FBMModel = arModel
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

                    lrType = ao_object.GetType.GetProperty(aoParseTreeNode.Token.Type.ToString).PropertyType
                    Dim piInstance As PropertyInfo = ao_object.GetType.GetProperty(aoParseTreeNode.Token.Type.ToString)

                    If lrType Is GetType(String) Then

                        piInstance.SetValue(ao_object, Trim(aoParseTreeNode.Token.Text))

                    ElseIf lrType Is lasListOfString.GetType Then


                        Dim liInstance As Object = ao_object.GetType.GetProperty(aoParseTreeNode.Token.Type.ToString).GetValue(ao_object)
                        Dim list As IList = CType(liInstance, IList)
                        list.Add(Trim(aoParseTreeNode.Token.Text))
                        ao_object.GetType.GetProperty(aoParseTreeNode.Token.Type.ToString).SetValue(ao_object, list, Nothing)

                    ElseIf lrType Is GetType(List(Of Object)) Then

                        Dim liInstance As Object = ao_object.GetType.GetProperty(aoParseTreeNode.Token.Type.ToString).GetValue(ao_object)
                        Dim list As IList = CType(liInstance, IList)
                        list.Add(aoParseTreeNode)
                        ao_object.GetType.GetProperty(aoParseTreeNode.Token.Type.ToString).SetValue(ao_object, list, Nothing)

                    ElseIf lrType Is GetType(Object) Then

                        piInstance.SetValue(ao_object, aoParseTreeNode)

                    ElseIf lrType.Name = "List`1" Then

                        Dim instance = Activator.CreateInstance(lrType.GenericTypeArguments(0))
                        Call GetParseTreeTokensReflection(instance, loParseTreeNode)
                        Dim liInstance As Object = ao_object.GetType.GetProperty(aoParseTreeNode.Token.Type.ToString).GetValue(ao_object)
                        Dim list As IList = CType(liInstance, IList)
                        list.Add(instance)
                        ao_object.GetType.GetProperty(aoParseTreeNode.Token.Type.ToString).SetValue(ao_object, list, Nothing)

                    Else
                        Dim instance = Activator.CreateInstance(lrType)
                        Call GetParseTreeTokensReflection(instance, loParseTreeNode)
                        piInstance.SetValue(ao_object, instance)
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

        Public Function ParseTreeContainsTokenType(ByVal aoParseTree As FEQL.ParseTree, ByVal aoTokenType As FEQL.TokenType) As Boolean

            Dim loParseNode As FEQL.ParseNode

            ParseTreeContainsTokenType = False

            For Each loParseNode In aoParseTree.Nodes
                If Me.ParseNodeContainsTokenType(loParseNode, aoTokenType) = True Then
                    ParseTreeContainsTokenType = True
                    Exit For
                End If
            Next

        End Function

        Public Function ParseNodeContainsTokenType(ByVal aoParseNode As FEQL.ParseNode, ByVal aoTokenType As FEQL.TokenType) As Boolean

            Dim loParseNode As FEQL.ParseNode

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

        Public Function ProcessFEQLStatement(ByVal asFEQLStatement As String,
                                             ByRef aoTokenType As FEQL.TokenType,
                                             ByRef aoParseTree As FEQL.ParseTree
                                             ) As ORMQL.Recordset

            Dim lrFact As New FBM.Fact

            Try
                '---------------------------
                'Parse the ORMQR statement
                '---------------------------
                SyncLock Me.Parsetree
                    Me.Parsetree = Me.Parser.Parse(Trim(asFEQLStatement))

                    If Me.ParseTreeContainsTokenType(Me.Parsetree, FEQL.TokenType.VALUETYPEISWRITTENASSTMT) Then ' VALUETYPEISWRITTENASCLAUSE
                        aoTokenType = FEQL.TokenType.VALUETYPEISWRITTENASSTMT
                        aoParseTree = Me.Parsetree
                        Return Nothing

                    ElseIf Me.ParseTreeContainsTokenType(Me.Parsetree, FEQL.TokenType.KEYWDASSERT) Then
                        aoTokenType = FEQL.TokenType.ASSERTSTMT
                        aoParseTree = Me.Parsetree
                        Return Me.processASSERTStatement(asFEQLStatement)

                    ElseIf Me.ParseTreeContainsTokenType(Me.Parsetree, FEQL.TokenType.KEYWDCREATE) And
                       Me.ParseTreeContainsTokenType(Me.Parsetree, FEQL.TokenType.KEYWDDATABASE) Then
                        aoTokenType = FEQL.TokenType.CREATEDATABASESTMT
                        aoParseTree = Me.Parsetree
                        Return Me.processCREATEDATABASEStatement(asFEQLStatement)

                    ElseIf Me.ParseTreeContainsTokenType(Me.Parsetree, FEQL.TokenType.KEYWDISIDENTIFIEDBYITS) Then
                        aoTokenType = FEQL.TokenType.KEYWDISIDENTIFIEDBYITS
                        aoParseTree = Me.Parsetree
                        Return Nothing
                    ElseIf Me.ParseTreeContainsTokenType(Me.Parsetree, FEQL.TokenType.KEYWDISWHERE) Then
                        aoTokenType = FEQL.TokenType.KEYWDISWHERE
                        aoParseTree = Me.Parsetree
                        Return Nothing
                    ElseIf Me.ParseTreeContainsTokenType(Me.Parsetree, FEQL.TokenType.KEYWDISAKINDOF) Then
                        aoTokenType = FEQL.TokenType.KEYWDISAKINDOF
                        aoParseTree = Me.Parsetree
                        Return Nothing
                    ElseIf Me.ParseTreeContainsTokenType(Me.Parsetree, FEQL.TokenType.FACTTYPEPRODUCTION) Then
                        aoTokenType = FEQL.TokenType.FACTTYPEPRODUCTION
                        aoParseTree = Me.Parsetree
                        Return Nothing
                    End If

                    Select Case Me.Parsetree.Nodes(0).Nodes(0).Text
                        Case Is = "CREATESTMT"

                            Return Me.ProcessCREATEStatement(asFEQLStatement)

                        Case Is = "DIDSELECTSTMT"

                            '=============================================================
                            Return Me.ProcessWHICHSELECTStatementNew(asFEQLStatement)

                            '----------------------------------------------------------------------------------
                            'Exit the sub because have found what the User was trying to do, and have done it 
                            '----------------------------------------------------------------------------------
                            Exit Function


                        Case Is = "WHICHSELECTSTMT"

                            '=============================================================
                            Return Me.ProcessWHICHSELECTStatementNew(asFEQLStatement)

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

                        Case Is = "SHOWSTMT"

                            '=============================================================
                            Return Me.ProcessSHOWStatement(asFEQLStatement)

                        Case Is = "DELETEFACTSTMT"
                            '20200727-VM-Just here as an example.
                            'Return Me.ProcessDELETEFACTSTMTStatement
                            Return Nothing
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
                        Case Is = FEQL.TokenType.DIDSELECTSTMT
                            Return FactEngine.pcenumFEQLStatementType.DIDStatement
                    End Select
                End SyncLock
            Catch ex As Exception

            End Try

        End Function

    End Class

End Namespace