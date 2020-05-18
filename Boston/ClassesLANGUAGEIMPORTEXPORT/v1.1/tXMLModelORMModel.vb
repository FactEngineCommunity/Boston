Imports System.Xml.Serialization

Namespace XMLModel11
    <Serializable()>
    Public Class ORMModel

        <XmlAttribute()>
        Public ModelId As String = ""

        <XmlAttribute()>
        Public Name As String = ""

        Public ValueTypes As New List(Of XMLModel11.ValueType)
        Public EntityTypes As New List(Of XMLModel11.EntityType)
        Public FactTypes As New List(Of XMLModel11.FactType)
        Public RoleConstraints As New List(Of XMLModel11.RoleConstraint)
        Public ModelNotes As New List(Of XMLModel11.ModelNote)

        ''' <summary>
        ''' Parameterless Constructor
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub New()
            '-------------------
            'Parameterless New
            '-------------------
        End Sub
    End Class
End Namespace