Imports System.Reflection

Namespace VAQL

    Public Class AddObjectTypesRelatedToObjectTypeOnPage

        Private _KEYWDADDOBJECTTYPESRELATEDTO As String
        Public Property KEYWDADDOBJECTTYPESRELATEDTO As String
            Get
                Return Me._KEYWDADDOBJECTTYPESRELATEDTO
            End Get
            Set(value As String)
                Me._KEYWDADDOBJECTTYPESRELATEDTO = value
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

        Private _PAGENAME As String
        Public Property PAGENAME As String
            Get
                Return Me._PAGENAME
            End Get
            Set(value As String)
                Me._PAGENAME = value
            End Set
        End Property

        Private _KEYWDANYFACTTYPE As String
        Public Property KEYWDANYFACTTYPE As String
            Get
                Return Me._KEYWDANYFACTTYPE
            End Get
            Set(value As String)
                Me._KEYWDANYFACTTYPE = value
            End Set
        End Property

    End Class

    Public Class AddObjectTypeToPageStatement

        Private _KEYWDADDOBJECTTYPE As String
        Public Property KEYWDADDOBJECTTYPE As String
            Get
                Return Me._KEYWDADDOBJECTTYPE
            End Get
            Set(value As String)
                Me._KEYWDADDOBJECTTYPE = value
            End Set
        End Property

        Private _KEYWDTOPAGE As String
        Public Property KEYWDTOPAGE As String
            Get
                Return Me._KEYWDTOPAGE
            End Get
            Set(value As String)
                Me._KEYWDTOPAGE = value
            End Set
        End Property

        Private _KEYWDSTANDALONE As String
        Public Property KEYWDSTANDALONE As String
            Get
                Return Me._KEYWDSTANDALONE
            End Get
            Set(value As String)
                Me._KEYWDSTANDALONE = value
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

    Public Class CreatePageStatement

        Private _KEYWDCREATE As String
        Public Property KEYWDCREATE As String
            Get
                Return Me._KEYWDCREATE
            End Get
            Set(value As String)
                Me._KEYWDCREATE = value
            End Set
        End Property

        Private _KEYWDPAGE As String
        Public Property KEYWDPAGE As String
            Get
                Return Me._KEYWDPAGE
            End Get
            Set(value As String)
                Me._KEYWDPAGE = value
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


    Public Class IsAConceptStatement

        Private _FRONTREADINGTEXT As String
        Public Property FRONTREADINGTEXT As String
            Get
                Return Me._FRONTREADINGTEXT
            End Get
            Set(value As String)
                Me._FRONTREADINGTEXT = value
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

    End Class

    Public Class IsAnEntityTypeStatement

        Private _MODELELEMENTNAME As String
        Public Property MODELELEMENTNAME As String
            Get
                Return Me._MODELELEMENTNAME
            End Get
            Set(value As String)
                Me._MODELELEMENTNAME = value
            End Set
        End Property

        Private _KEYWDISANENTITYTYPE As String
        Public Property KEYWDISANENTITYTYPE As String
            Get
                Return Me._KEYWDISANENTITYTYPE
            End Get
            Set(value As String)
                Me._KEYWDISANENTITYTYPE = value
            End Set
        End Property
    End Class

    Public Class LongDescriptionStatement

        Private _MODELELEMENTNAME As String
        Public Property MODELELEMENTNAME As String
            Get
                Return Me._MODELELEMENTNAME
            End Get
            Set(value As String)
                Me._MODELELEMENTNAME = value
            End Set
        End Property

        Private _DESCRIPTIONCONTENT As String
        Public Property DESCRIPTIONCONTENT As String
            Get
                Return Me._DESCRIPTIONCONTENT
            End Get
            Set(value As String)
                Me._DESCRIPTIONCONTENT = value
            End Set
        End Property

    End Class

    Public Class IsAValueTypeStatement

        Private _MODELELEMENTNAME As String
        Public Property MODELELEMENTNAME As String
            Get
                Return Me._MODELELEMENTNAME
            End Get
            Set(value As String)
                Me._MODELELEMENTNAME = value
            End Set
        End Property

        Private _KEYWDISAVALUETYPE As String
        Public Property KEYWDISAVALUETYPE As String
            Get
                Return Me._KEYWDISAVALUETYPE
            End Get
            Set(value As String)
                Me._KEYWDISAVALUETYPE = value
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
        Private _VALUETYPEWRITTENASCLAUSE As New VAQL.ValueTypeWrittenAsClause
        Public Property VALUETYPEWRITTENASCLAUSE As VAQL.ValueTypeWrittenAsClause
            Get
                Return Me._VALUETYPEWRITTENASCLAUSE
            End Get
            Set(value As VAQL.ValueTypeWrittenAsClause)
                Me._VALUETYPEWRITTENASCLAUSE = value
            End Set
        End Property
    End Class

    Public Class IsAKindOfStatement

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

    Public Class IsWhereStatement

        Private _MODELELEMENT As New List(Of VAQL.ModelElementClause)
        Public Property MODELELEMENT As List(Of VAQL.ModelElementClause)
            Get
                Return Me._MODELELEMENT
            End Get
            Set(value As List(Of VAQL.ModelElementClause))
                Me._MODELELEMENT = value
            End Set
        End Property

        Private _FACTTYPESTMT As New List(Of VAQL.FactTypeStatement)
        Public Property FACTTYPESTMT As List(Of VAQL.FactTypeStatement)
            Get
                Return Me._FACTTYPESTMT
            End Get
            Set(value As List(Of VAQL.FactTypeStatement))
                Me._FACTTYPESTMT = value
            End Set
        End Property

    End Class

    Public Class IdentifierModelElementClause

        Private _MODELELEMENT As ModelElementClause = Nothing
        Public Property MODELELEMENT As ModelElementClause
            Get
                Return Me._MODELELEMENT
            End Get
            Set(value As ModelElementClause)
                Me._MODELELEMENT = value
            End Set
        End Property

        Private _VALUE As String = Nothing
        Public Property VALUE As String
            Get
                Return Me._VALUE
            End Get
            Set(value As String)
                Me._VALUE = value
            End Set
        End Property

        Private _VALUELIST As ValueListClause = Nothing

        Public Property VALUELIST As ValueListClause
            Get
                Return Me._VALUELIST
            End Get
            Set(value As ValueListClause)
                Me._VALUELIST = value
            End Set
        End Property

    End Class

    Public Class ValueListClause

        Private _MODELELEMENT As ModelElementClause = Nothing
        Public Property MODELELEMENT As ModelElementClause
            Get
                Return Me._MODELELEMENT
            End Get
            Set(value As ModelElementClause)
                Me._MODELELEMENT = value
            End Set
        End Property

        Private _VALUE As String = Nothing
        Public Property VALUE As String
            Get
                Return Me._VALUE
            End Get
            Set(value As String)
                Me._VALUE = value
            End Set
        End Property

    End Class

    Public Class FactStatement

        <DebuggerBrowsable(DebuggerBrowsableState.Never)>
        Private _IDENTIFIERMODELELEMENT As New List(Of IdentifierModelElementClause)
        Public Property IDENTIFIERMODELELEMENT As List(Of IdentifierModelElementClause)
            Get
                Return Me._IDENTIFIERMODELELEMENT
            End Get
            Set(value As List(Of IdentifierModelElementClause))
                Me._IDENTIFIERMODELELEMENT = value
            End Set
        End Property

        Private _PREDICATECLAUSE As New List(Of PredicateClause)
        Public Property PREDICATECLAUSE As List(Of PredicateClause)
            Get
                Return Me._PREDICATECLAUSE
            End Get
            Set(value As List(Of PredicateClause))
                Me._PREDICATECLAUSE = value
            End Set
        End Property

    End Class

    Public Class FactTypeReadingStatement

        Private _FRONTREADINGTEXT As String
        Public Property FRONTREADINGTEXT As String
            Get
                Return Me._FRONTREADINGTEXT
            End Get
            Set(value As String)
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

    Public Class FactTypeStatement

        Private _FRONTREADINGTEXT As String
        Public Property FRONTREADINGTEXT As String
            Get
                Return Me._FRONTREADINGTEXT
            End Get
            Set(value As String)
                Me._FRONTREADINGTEXT = value
            End Set
        End Property


        Private _MODELELEMENT As New List(Of VAQL.ModelElementClause)
        Public Property MODELELEMENT As List(Of VAQL.ModelElementClause)
            Get
                Return Me._MODELELEMENT
            End Get
            Set(value As List(Of VAQL.ModelElementClause))
                Me._MODELELEMENT = value
            End Set
        End Property

        Private _MODELELEMENTTYPE As New List(Of VAQL.ModelElementTypeClause)
        Public Property MODELELEMENTTYPE As List(Of VAQL.ModelElementTypeClause)
            Get
                Return Me._MODELELEMENTTYPE
            End Get
            Set(value As List(Of VAQL.ModelElementTypeClause))
                Me._MODELELEMENTTYPE = value
            End Set
        End Property

        Private _PREDICATECLAUSE As New List(Of VAQL.PredicateClause)
        Public Property PREDICATECLAUSE As List(Of VAQL.PredicateClause)
            Get
                Return Me._PREDICATECLAUSE
            End Get
            Set(value As List(Of VAQL.PredicateClause))
                Me._PREDICATECLAUSE = value
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

        Public Function makeSentence() As String

            Dim lsSentence As String = ""

            Dim liInd = 0
            For Each lrModelElement In Me.MODELELEMENT
                lsSentence &= lrModelElement.MODELELEMENTNAME & " "

                If liInd < Me.MODELELEMENT.Count - 1 Then
                    For Each lsPredicatePart In Me.PREDICATECLAUSE(liInd).PREDICATEPART
                        lsSentence &= lsPredicatePart
                    Next
                End If
                liInd += 1
            Next

            Return Trim(lsSentence)

        End Function

    End Class

    Public Class PredicateClause

        Private _PREDICATEPART As New List(Of String)
        Public Property PREDICATEPART As List(Of String)
            Get
                Return Me._PREDICATEPART
            End Get
            Set(value As List(Of String))
                Me._PREDICATEPART = value
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

    Public Class ModelElementTypeClause

        <DebuggerBrowsable(DebuggerBrowsableState.Never)>
        Private _MODELELEMENT As VAQL.ModelElementClause
        Public Property MODELELEMENT As VAQL.ModelElementClause
            Get
                Return Me._MODELELEMENT
            End Get
            Set(value As VAQL.ModelElementClause)
                Me._MODELELEMENT = value
            End Set
        End Property

        <DebuggerBrowsable(DebuggerBrowsableState.Never)>
        Private _KEYWDWRITTENAS As String
        Public Property KEYWDWRITTENAS As String
            Get
                Return Me._KEYWDWRITTENAS
            End Get
            Set(value As String)
                Me._KEYWDWRITTENAS = value
            End Set
        End Property

        <DebuggerBrowsable(DebuggerBrowsableState.Never)>
        Private _KEYWDIDENTIFIEDBYITS As String
        Public Property KEYWDIDENTIFIEDBYITS As String
            Get
                Return Me._KEYWDIDENTIFIEDBYITS
            End Get
            Set(value As String)
                Me._KEYWDIDENTIFIEDBYITS = value
            End Set
        End Property

        <DebuggerBrowsable(DebuggerBrowsableState.Never)>
        Private _VALUETYPEWRITTENASCLAUSE As New VAQL.ValueTypeWrittenAsClause
        Public Property VALUETYPEWRITTENASCLAUSE As VAQL.ValueTypeWrittenAsClause
            Get
                Return Me._VALUETYPEWRITTENASCLAUSE
            End Get
            Set(value As VAQL.ValueTypeWrittenAsClause)
                Me._VALUETYPEWRITTENASCLAUSE = value
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

    End Class

    Public Class EntityTypeIsIdentifiedByItsStatement

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
        Private _VALUETYPEWRITTENASCLAUSE As New VAQL.ValueTypeWrittenAsClause
        Public Property VALUETYPEWRITTENASCLAUSE As VAQL.ValueTypeWrittenAsClause
            Get
                Return Me._VALUETYPEWRITTENASCLAUSE
            End Get
            Set(value As VAQL.ValueTypeWrittenAsClause)
                Me._VALUETYPEWRITTENASCLAUSE = value
            End Set
        End Property

    End Class

    Public Class ObjectifiedFactTypeIsIdentifiedByItsStatement

        <DebuggerBrowsable(DebuggerBrowsableState.Never)>
        Private _MODELELEMENT As New List(Of ModelElementClause)
        Public Property MODELELEMENT As List(Of ModelElementClause)
            Get
                Return Me._MODELELEMENT
            End Get
            Set(value As List(Of ModelElementClause))
                Me._MODELELEMENT = value
            End Set
        End Property

    End Class

    Public Class ValueConstraintClause

        Private _MODELELEMENTNAME As String
        Public Property MODELELEMENTNAME As String
            Get
                Return Me._MODELELEMENTNAME
            End Get
            Set(value As String)
                Me._MODELELEMENTNAME = value
            End Set
        End Property

        Private _VALUECONSTRAINTVALUE As New List(Of String)
        Public Property VALUECONSTRAINTVALUE As List(Of String)
            Get
                Return Me._VALUECONSTRAINTVALUE
            End Get
            Set(value As List(Of String))
                Me._VALUECONSTRAINTVALUE = value
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

    Public Class ValueTypeIsWrittenAsStatement

        Private _MODELELEMENTNAME As String
        Public Property MODELELEMENTNAME As String
            Get
                Return Me._MODELELEMENTNAME
            End Get
            Set(value As String)
                Me._MODELELEMENTNAME = value
            End Set
        End Property

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

    Public Class AtMostOneStatement

        Private _FRONTREADINGTEXT As String
        Public Property FRONTREADINGTEXT As String
            Get
                Return Me._FRONTREADINGTEXT
            End Get
            Set(value As String)
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

        Private _MODELELEMENTTYPE As New List(Of VAQL.ModelElementTypeClause)
        Public Property MODELELEMENTTYPE As List(Of VAQL.ModelElementTypeClause)
            Get
                Return Me._MODELELEMENTTYPE
            End Get
            Set(value As List(Of VAQL.ModelElementTypeClause))
                Me._MODELELEMENTTYPE = value
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
        Private _VALUETYPEWRITTENASCLAUSE As VAQL.ValueTypeWrittenAsClause = Nothing
        Public Property VALUETYPEWRITTENASCLAUSE As VAQL.ValueTypeWrittenAsClause
            Get
                Return Me._VALUETYPEWRITTENASCLAUSE
            End Get
            Set(value As VAQL.ValueTypeWrittenAsClause)
                Me._VALUETYPEWRITTENASCLAUSE = value
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

        Private Parser As New VAQL.Parser(New VAQL.Scanner) 'Used to parse Text input into the Brain; especially for ORMQL.
        Private Parsetree As New VAQL.ParseTree 'Used with the Parser, is populated during the parsing of text input into the Brain; especially ORMQL

        Public ADDOBJECTTYPETOPAGEStatement As New VAQL.AddObjectTypeToPageStatement
        Public ADDOBJECTTYPESRELATEDTOOBJECTTYPEONPAGEStatement As New VAQL.AddObjectTypesRelatedToObjectTypeOnPage
        Public CREATEPAGEStatement As New VAQL.CreatePageStatement
        Public FACTStatement As New VAQL.FactStatement
        Public ISACONCEPTStatement As New VAQL.IsAConceptStatement
        Public ISANENTITYTYPEStatement As New VAQL.IsAnEntityTypeStatement
        Public ISAVALUETYPEStatement As New VAQL.IsAValueTypeStatement
        Public ISAKINDOFStatement As New VAQL.IsAKindOfStatement
        Public ISWHEREStatement As New VAQL.IsWhereStatement
        Public ATMOSTONEStatement As New VAQL.AtMostOneStatement
        Public PREDICATEPARTClause As New VAQL.PredicatePartClause
        Public ENTITYTYPEISIDENTIFIEDBYITSStatement As New VAQL.EntityTypeIsIdentifiedByItsStatement
        Public OBJECTIFIEDFACTTYPEISIDENTIFIEDBYITSStatement As New VAQL.ObjectifiedFactTypeIsIdentifiedByItsStatement
        Public FACTTYPEREADINGStatement As New VAQL.FactTypeReadingStatement
        Public LONGDESCRIPTIONSTMT As New VAQL.LongDescriptionStatement
        Public MODELELEMENTClause As New VAQL.ModelElementClause
        Public MODELELEMENTTYPEClause As New VAQL.ModelElementTypeClause
        Public VALUETYPEISWRITTENASStatement As New VAQL.ValueTypeIsWrittenAsStatement
        Public VALUETYPEWRITTENASClause As New VAQL.ValueTypeWrittenAsClause
        Public VALUECONSTRAINTClause As New VAQL.ValueConstraintClause

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
            'Dim lrIsAConceptStatement As New DynamicClassLibrary.Factory.tClass
            'lrIsAConceptStatement.add_attribute(New DynamicClassLibrary.Factory.tAttribute("FRONTREADINGTEXT", GetType(String)))
            'lrIsAConceptStatement.add_attribute(New DynamicClassLibrary.Factory.tAttribute("MODELELEMENTNAME", GetType(String)))

            'Me.ISACONCEPTStatement = lrIsAConceptStatement.clone

            '====================================================================
            'Create the DynamicObject for IsAnEntityType Texts/Satements
            '====================================================================
            'Dim lrIsAnEntityTypeStatement As New DynamicClassLibrary.Factory.tClass
            'lrIsAnEntityTypeStatement.add_attribute(New DynamicClassLibrary.Factory.tAttribute("MODELELEMENTNAME", GetType(String)))
            'lrIsAnEntityTypeStatement.add_attribute(New DynamicClassLibrary.Factory.tAttribute("KEYWDISANENTITYTYPE", GetType(String)))

            'Me.ISANENTITYTYPEStatement = lrIsAnEntityTypeStatement.clone

            '====================================================================
            'Create the DynamicObject for FACTTYPEREADING Texts/Satements
            '====================================================================
            'Dim lrFactTypeReadingStatement As New DynamicClassLibrary.Factory.tClass
            'lrFactTypeReadingStatement.add_attribute(New DynamicClassLibrary.Factory.tAttribute("FRONTREADINGTEXT", GetType(String)))
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
            'Create the DynamicObject for ENTITYTYPEISIDENTIFIEDBYITS Satements
            '====================================================================
            'Dim lrEntityTypeIsIdentifiedByItsStatement As New DynamicClassLibrary.Factory.tClass
            'lrEntityTypeIsIdentifiedByItsStatement.add_attribute(New DynamicClassLibrary.Factory.tAttribute("MODELELEMENTNAME", GetType(String)))
            'lrEntityTypeIsIdentifiedByItsStatement.add_attribute(New DynamicClassLibrary.Factory.tAttribute("REFERENCEMODE", GetType(String)))

            'Me.ENTITYTYPEISIDENTIFIEDBYITSStatement = lrEntityTypeIsIdentifiedByItsStatement.clone

            '==========================================================
            '==========================================================
            'Dim lrValueTypeIsWrittenAsStatement As New DynamicClassLibrary.Factory.tClass
            'lrValueTypeIsWrittenAsStatement.add_attribute(New DynamicClassLibrary.Factory.tAttribute("MODELELEMENTNAME", GetType(String)))
            'lrValueTypeIsWrittenAsStatement.add_attribute(New DynamicClassLibrary.Factory.tAttribute("DATATYPE", GetType(Object)))
            'lrValueTypeIsWrittenAsStatement.add_attribute(New DynamicClassLibrary.Factory.tAttribute("DATATYPELENGTH", GetType(Object)))
            'lrValueTypeIsWrittenAsStatement.add_attribute(New DynamicClassLibrary.Factory.tAttribute("DATATYPEPRECISION", GetType(Object)))
            'lrValueTypeIsWrittenAsStatement.add_attribute(New DynamicClassLibrary.Factory.tAttribute("NUMBER", GetType(String)))

            'Me.VALUETYPEISWRITTENASStatement = lrValueTypeIsWrittenAsStatement.clone

            'Dim lrATMOSTONEStatement As New DynamicClassLibrary.Factory.tClass
            'lrATMOSTONEStatement.add_attribute(New DynamicClassLibrary.Factory.tAttribute("FRONTREADINGTEXT", GetType(String)))
            'lrATMOSTONEStatement.add_attribute(New DynamicClassLibrary.Factory.tAttribute("MODELELEMENT", GetType(List(Of Object))))
            'lrATMOSTONEStatement.add_attribute(New DynamicClassLibrary.Factory.tAttribute("MODELELEMENTNAME", GetType(List(Of String))))
            'lrATMOSTONEStatement.add_attribute(New DynamicClassLibrary.Factory.tAttribute("PREDICATECLAUSE", GetType(List(Of Object))))
            'lrATMOSTONEStatement.add_attribute(New DynamicClassLibrary.Factory.tAttribute("UNARYPREDICATEPART", GetType(String)))
            'lrATMOSTONEStatement.add_attribute(New DynamicClassLibrary.Factory.tAttribute("FOLLOWINGREADINGTEXT", GetType(String)))

            'Me.ATMOSTONEStatement = lrATMOSTONEStatement.clone

            '=========================================================
            'Create the DynamicObject for PREDICATE Clauses
            '=========================================================
            'Dim lrORMQLPREDICATECLAUSE As New DynamicClassLibrary.Factory.tClass
            'lrORMQLPREDICATECLAUSE.add_attribute(New DynamicClassLibrary.Factory.tAttribute("PREDICATEPART", GetType(List(Of String))))

            'Me.PREDICATEPARTClause = lrORMQLPREDICATECLAUSE.clone


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

        Public Sub GetParseTreeTokensReflection(ByRef ao_object As Object, ByRef aoParseTreeNode As VAQL.ParseNode)

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
            Dim loParseTreeNode As VAQL.ParseNode
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
                        list.Add(aoParseTreeNode.Token.Text)
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


        'Public Sub GetParseTreeTokensReflection(ByRef ao_object As Object, ByRef aoParseTreeNode As VAQL.ParseNode)

        '    '-------------------------------
        '    'NB IMPORTANT: The TokenTypes in the ParseTree must be the same as established in aoObject (as hardcoded here in Richmond).
        '    'i.e. Richmond (the software) and the Parser (to the outside word) are linked.
        '    'If the Tokens in the Parser do not match the HardCoded tokens then Richmond will not be able to retrieve the tokens
        '    ' from the ParseText input from the user.
        '    'This isn't the falt of the user, this is a fault of using the ParserGenerator (TinyPG in Richmond's case) to set-up the Tokens.
        '    'i.e. The person setting up the Parser in TinyPG need be aware that 'Tokens' in TinyPG (when defining the ORMQL) need be the same
        '    ' as the Tokens in Richmond and as Richmond expects.
        '    'i.e. Establishing a Parser in TinyPG is actually a form of 'coding' (hard-coding), and where Richmond ostensibly has 2 parsers,
        '    '  VB.Net and TinyPG.
        '    '---------------------
        '    'Parameters
        '    'ao_object is of runtime generated type DynamicCollection.Entity
        '    '----------------------------------------------------------------------

        '    Dim loPropertyInfo() As System.Reflection.PropertyInfo = ao_object.GetType().GetProperties
        '    Dim loProperty As PropertyInfo
        '    Dim lr_bag As New Stack
        '    Dim loParseTreeNode As VAQL.ParseNode
        '    Dim lrType As Type
        '    Try
        '        '--------------------------------------
        '        'Retrieve the list of required Tokens
        '        '--------------------------------------
        '        For Each loProperty In loPropertyInfo
        '            lr_bag.Push(loProperty.Name)
        '        Next

        '        loParseTreeNode = aoParseTreeNode

        '        Dim lasListOfString As New List(Of String)

        '        If lr_bag.Contains(aoParseTreeNode.Token.Type.ToString) Then
        '            'lrType = ao_object.GetAttributeMember(aoParseTreeNode.Token.Type.ToString).GetType
        '            lrType = ao_object.GetType.GetProperty(aoParseTreeNode.Token.Type.ToString).PropertyType
        '            Dim piInstance As PropertyInfo = ao_object.GetType.GetProperty(aoParseTreeNode.Token.Type.ToString)

        '            If lrType Is GetType(String) Then
        '                'ao_object.SetAttributeMember(aoParseTreeNode.Token.Type.ToString, aoParseTreeNode.Token.Text)
        '                piInstance.SetValue(ao_object, aoParseTreeNode.Token.Text)

        '            ElseIf lrType Is lasListOfString.GetType Then
        '                'ao_object.GetAttributeMember(aoParseTreeNode.Token.Type.ToString).Add(aoParseTreeNode.Token.Text)

        '                Dim liInstance As Object = ao_object.GetType.GetProperty(aoParseTreeNode.Token.Type.ToString).GetValue(ao_object)
        '                'Dim instance As Object = Activator.CreateInstance(ao_object.GetType.GetProperty(aoParseTreeNode.Token.Type.ToString).PropertyType)
        '                Dim list As IList = CType(liInstance, IList)
        '                list.Add(aoParseTreeNode.Token.Text)
        '                ao_object.GetType.GetProperty(aoParseTreeNode.Token.Type.ToString).SetValue(ao_object, list, Nothing)

        '                '==================================
        '                'piInstance.SetValue(ao_object, aoParseTreeNode.Token.Text)
        '                'ElseIf lrType Is GetType(List(Of String)) Then
        '                '    'ao_object.GetAttributeMember(aoParseTreeNode.Token.Type.ToString).Add(aoParseTreeNode)
        '                '    piInstance.SetValue(ao_object, aoParseTreeNode.Token.Text)
        '            ElseIf lrType Is GetType(List(Of Object)) Then

        '                Dim liInstance As Object = ao_object.GetType.GetProperty(aoParseTreeNode.Token.Type.ToString).GetValue(ao_object)
        '                'Dim instance As Object = Activator.CreateInstance(ao_object.GetType.GetProperty(aoParseTreeNode.Token.Type.ToString).PropertyType)
        '                Dim list As IList = CType(liInstance, IList)
        '                list.Add(aoParseTreeNode)
        '                ao_object.GetType.GetProperty(aoParseTreeNode.Token.Type.ToString).SetValue(ao_object, list, Nothing)

        '                'ao_object.GetAttributeMember(aoParseTreeNode.Token.Type.ToString).Add(aoParseTreeNode)
        '                'piInstance.SetValue(ao_object, aoParseTreeNode.Token.Text)
        '            ElseIf lrType Is GetType(Object) Then
        '                'ao_object.SetAttributeMember(aoParseTreeNode.Token.Type.ToString, aoParseTreeNode)
        '                piInstance.SetValue(ao_object, aoParseTreeNode)
        '            End If
        '        End If

        '        For Each loParseTreeNode In aoParseTreeNode.Nodes.ToArray
        '            Call Me.GetParseTreeTokensReflection(ao_object, loParseTreeNode)
        '        Next

        '    Catch ex As Exception
        '        Dim lsMessage As String
        '        Dim mb As MethodBase = MethodInfo.GetCurrentMethod()
        '        lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
        '        lsMessage &= vbCrLf & vbCrLf & ex.Message
        '        prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        '    End Try

        'End Sub


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


        Public Function ProcessVAQLStatement(ByVal as_ORMQL_statement As String,
                                             ByRef aoTokenType As VAQL.TokenType,
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
                    If Me.ParseNodeContainsTokenType(Me.Parsetree, TokenType.KEYWDADDOBJECTTYPE) And
                        Me.ParseNodeContainsTokenType(Me.Parsetree, TokenType.KEYWDTOPAGE) Then
                        aoTokenType = TokenType.ADDOBJECTTYPETOPAGESTMT
                        aoParseTree = Me.Parsetree

                    ElseIf Me.ParseTreeContainsTokenType(Me.Parsetree, TokenType.ADDOBJECTTYPESRELATEDTOMODELELEMENTTOPAGESTMT) Then
                        aoTokenType = TokenType.ADDOBJECTTYPESRELATEDTOMODELELEMENTTOPAGESTMT
                        aoParseTree = Me.Parsetree

                    ElseIf Me.ParseNodeContainsTokenType(Me.Parsetree, TokenType.KEYWDCREATE) And
                           Me.ParseNodeContainsTokenType(Me.Parsetree, TokenType.KEYWDPAGE) Then
                        aoTokenType = TokenType.CREATEPAGESTMT
                        aoParseTree = Me.Parsetree

                    ElseIf Me.ParseNodeContainsTokenType(Me.Parsetree, TokenType.KEYWDHASLONGDESCRIPTION) Then
                        aoTokenType = TokenType.KEYWDHASLONGDESCRIPTION
                        aoParseTree = Me.Parsetree

                    ElseIf Me.ParseTreeContainsTokenType(Me.Parsetree, TokenType.VALUETYPEISWRITTENASCLAUSE) Then
                        aoTokenType = TokenType.VALUETYPEISWRITTENASCLAUSE
                        aoParseTree = Me.Parsetree

                    ElseIf Me.ParseTreeContainsTokenType(Me.Parsetree, TokenType.FACTSTMT) Then
                        aoTokenType = TokenType.FACTSTMT
                        aoParseTree = Me.Parsetree

                    ElseIf Me.ParseTreeContainsTokenType(Me.Parsetree, TokenType.VALUECONSTRAINTCLAUSE) Then
                        aoTokenType = TokenType.VALUECONSTRAINTCLAUSE
                        aoParseTree = Me.Parsetree

                    ElseIf Me.ParseTreeContainsTokenType(Me.Parsetree, TokenType.KEYWDISAKINDOF) Then
                        aoTokenType = TokenType.KEYWDISAKINDOF
                        aoParseTree = Me.Parsetree

                    ElseIf Me.ParseTreeContainsTokenType(Me.Parsetree, TokenType.OBJECTIFIEDFACTTYPEISIDENTIFIEDBYITSCLAUSE) Then
                        aoTokenType = TokenType.OBJECTIFIEDFACTTYPEISIDENTIFIEDBYITSCLAUSE
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
                           Me.ParseTreeContainsTokenType(Me.Parsetree, TokenType.KEYWDANYNUMBEROF) Then
                        aoTokenType = TokenType.KEYWDANYNUMBEROF
                        aoParseTree = Me.Parsetree

                    ElseIf Me.ParseTreeContainsTokenType(Me.Parsetree, TokenType.MODELELEMENTLEADINGSTMT) And
                           Me.ParseTreeContainsTokenType(Me.Parsetree, TokenType.KEYWDATLEASTONE) Then
                        aoTokenType = TokenType.KEYWDATLEASTONE
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
                    ElseIf Me.ParseTreeContainsTokenType(Me.Parsetree, TokenType.KEYWDISAVALUETYPE) Then
                        aoTokenType = TokenType.KEYWDISAVALUETYPE
                        aoParseTree = Me.Parsetree
                    ElseIf Me.ParseTreeContainsTokenType(Me.Parsetree, TokenType.KEYWDISWHERE) And
                           Me.ParseTreeContainsTokenType(Me.Parsetree, TokenType.FACTTYPESTMT) Then
                        aoTokenType = TokenType.KEYWDISWHERE
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
