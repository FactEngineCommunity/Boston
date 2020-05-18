Imports System.Xml.Serialization

Namespace XMLModel16
    <Serializable()>
    Public Class ORMModel

        <XmlAttribute()>
        Public ModelId As String = ""

        <XmlAttribute()>
        Public Name As String = ""

        Public ValueTypes As New List(Of XMLModel16.ValueType)
        Public EntityTypes As New List(Of XMLModel16.EntityType)
        Public FactTypes As New List(Of XMLModel16.FactType)
        Public RoleConstraints As New List(Of XMLModel16.RoleConstraint)
        Public ModelNotes As New List(Of XMLModel16.ModelNote)

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