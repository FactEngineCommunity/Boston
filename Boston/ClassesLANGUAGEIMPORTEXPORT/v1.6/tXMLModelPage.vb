Imports System.Xml.Serialization
Imports System.Runtime.Serialization.Formatters.Binary

Namespace XMLModel16
    <Serializable()>
    Public Class Page

        <XmlAttribute()>
        Public Id As String

        <XmlAttribute()>
        Public Name As String = ""

        <XmlAttribute()>
        Public Language As pcenumLanguage

        <XmlAttribute()>
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
