Imports System.Xml.Serialization

Namespace NORMA.ORMDiagram

    <XmlType(AnonymousType:=True, [Namespace]:="http://schemas.neumont.edu/ORM/2006-04/ORMDiagram"),
        XmlRoot([Namespace]:="http://schemas.neumont.edu/ORM/2006-04/ORMDiagram", IsNullable:=False)>
    Public Class ORMDiagram

        '''<remarks/>
        Public Property Shapes() As ORMDiagramShapes

        '''<remarks/>
        Public Property Subject() As Subject

        '''<remarks/>
        <XmlAttribute(AttributeName:="id")>
        Public Property Id() As String

        '''<remarks/>
        <XmlAttribute()>
        Public Property IsCompleteView() As Boolean

        '''<remarks/>
        <XmlAttribute()>
        Public Property Name() As String

        '''<remarks/>
        <XmlAttribute()>
        Public Property BaseFontName() As String

        '''<remarks/>
        <XmlAttribute()>
        Public Property BaseFontSize() As Decimal

        Public Sub New()
            Me.Shapes = New ORMDiagramShapes
        End Sub

    End Class

    <XmlType(AnonymousType:=True, [Namespace]:="http://schemas.neumont.edu/ORM/2006-04/ORMDiagram")>
    Partial Public Class Subject

        <XmlAttribute(AttributeName:="ref")>
        Public Property Ref() As String

    End Class

    <XmlType(AnonymousType:=True, [Namespace]:="http://schemas.neumont.edu/ORM/2006-04/ORMDiagram")>
    Partial Public Class ORMDiagramShapes

        '''<remarks/>
        <XmlElement("ExternalConstraintShape", GetType(ExternalConstraintShape)),
         XmlElement("FactTypeShape", GetType(FactTypeShape)),
         XmlElement("FrequencyConstraintShape", GetType(FrequencyConstraintShape)),
         XmlElement("ModelNoteShape", GetType(ModelNoteShape)),
         XmlElement("ObjectTypeShape", GetType(ObjectTypeShape)),
         XmlElement("RingConstraintShape", GetType(RingConstraintShape))>
        Public Property Items() As Object() = {}

    End Class

End Namespace

