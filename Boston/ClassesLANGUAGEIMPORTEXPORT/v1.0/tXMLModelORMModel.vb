Imports System.Xml.Serialization

Namespace XMLModel1
    <Serializable()>
    Public Class ORMModel

        <XmlAttribute()>
        Public ModelId As String = ""

        <XmlAttribute()>
        Public Name As String = ""

        Public ValueTypes As New List(Of XMLModel1.ValueType)
        Public EntityTypes As New List(Of XMLModel1.EntityType)
        Public FactTypes As New List(Of XMLModel1.FactType)
        Public RoleConstraints As New List(Of XMLModel1.RoleConstraint)
        Public ModelNotes As New List(Of XMLModel1.ModelNote)

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