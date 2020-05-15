Imports System.Xml.Serialization
Imports System.Runtime.Serialization.Formatters.Binary

Namespace XMLModelv081
    <Serializable()> _
    Public Class Page

        <XmlAttribute()> _
        Public Id As String

        <XmlAttribute()> _
        Public Name As String = ""

        <XmlAttribute()> _
        Public Language As pcenumLanguage

        <XmlAttribute()> _
        Public IsCoreModelPage As Boolean = False

        '----------------------------------------------
        'A Page consists of a set of ConceptInstances
        '----------------------------------------------
        Public ConceptInstance As New List(Of FBM.ConceptInstance)

        'Public EntityTypeInstance As New List(Of FBM.EntityTypeInstance)
        'Public ValueTypeInstance As New List(Of FBM.ValueTypeInstance)
        '<XmlIgnore()> _
        'Public FactTypeInstance As New List(Of FBM.FactTypeInstance)
        '<XmlIgnore()> _
        'Public RoleConstraintInstance As New List(Of FBM.RoleConstraintInstance)

        Public Sub New()
            '-------------------
            'Parameterless New
            '-------------------
        End Sub

    End Class
End Namespace
