Imports System.Xml.Serialization

Namespace XMLModel15
    <Serializable()> _
    Public Class ORMModel

        <XmlAttribute()> _
        Public ModelId As String = ""

        <XmlAttribute()> _
        Public Name As String = ""

        Public ValueTypes As New List(Of XMLModel15.ValueType)
        Public EntityTypes As New List(Of XMLModel15.EntityType)
        Public FactTypes As New List(Of XMLModel15.FactType)
        Public RoleConstraints As New List(Of XMLModel15.RoleConstraint)
        Public ModelNotes As New List(Of XMLModel15.ModelNote)

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