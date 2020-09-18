Imports System.Xml.Serialization

Namespace XMLModel
    <Serializable()> _
    Public Class ORMModel

        <XmlAttribute()> _
        Public ModelId As String = ""

        <XmlAttribute()>
        Public Name As String = ""

        <XmlAttribute()>
        Public CoreVersionNumber As String = ""

        Public ValueTypes As New List(Of XMLModel.ValueType)
        Public EntityTypes As New List(Of XMLModel.EntityType)
        Public FactTypes As New List(Of XMLModel.FactType)
        Public RoleConstraints As New List(Of XMLModel.RoleConstraint)
        Public ModelNotes As New List(Of XMLModel.ModelNote)

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