Imports System.Xml.Serialization

Namespace FBM
    '-----------------------------------------------------------------------------------------------------
    'Everything that can be displayed on the screen is a 'Symbol', even if it is an ASCII text character
    '  or logic 'Symbol'
    '-----------------------------------------------------------------------------------------------------
    <Serializable()> _
    Public Class ConceptInstance
        Inherits FBM.Concept

        '--------------------------------------------------------------------------------------------------------------------------
        'This class is predominantly used to store instance (usage) of a Symbol within a diagram (e.g. ORMDiagram, UseCaseDiagram).
        '  e.g. If an EntityType is used within an ORM Diagram, this class is used to reflect within which diagram and at which coordinate
        '  the EntityType was displayed within the diagram.
        '--------------------------------------------------------------------------------------------------------------------------
        <XmlIgnore()> _
        Public ModelId As String = ""

        <XmlAttribute()> _
        Public ConceptType As pcenumConceptType

        <XmlIgnore()> _
        Public PageId As String = ""

        <XmlAttribute()> _
        Public RoleId As String = "NotUsed" 'Only used for ConceptInstances where ConceptType is 'Value' otherwise is set to 'NotUsed'.

        <XmlAttribute()> _
        Public X As Integer

        <XmlAttribute()> _
        Public Y As Integer

        <XmlAttribute()> _
        Public Orientation As Integer

        <XmlAttribute()>
        Public Visible As Boolean = False

        <XmlAttribute()>
        Public InstanceNumber As Integer = 1

        <XmlElement()>
        Public ConceptInstanceFlag As New List(Of FBM.ConceptInstanceFlag)

        Public Sub New()
            '-------------------
            'Parameterless New
            '-------------------
        End Sub

        Public Sub New(ByRef arModel As FBM.Model,
                       ByRef arPage As FBM.Page,
                       ByVal asSymbol As String,
                       Optional aiInstanceNumber As Integer = 1)

            Me.ModelId = arModel.ModelId
            Me.PageId = arPage.PageId
            Me.Symbol = asSymbol
            Me.InstanceNumber = aiInstanceNumber

        End Sub

        Public Sub New(ByRef arModel As FBM.Model,
                       ByRef arPage As FBM.Page,
                       ByVal asSymbol As String,
                       ByVal aiConceptType As pcenumConceptType,
                       ByVal aiInstanceNumber As Integer,
                       Optional ByVal aiX As Integer = 0,
                       Optional ByVal aiY As Integer = 0)

            Me.ModelId = arModel.ModelId
            Me.PageId = arPage.PageId
            Me.Symbol = asSymbol
            Me.ConceptType = aiConceptType
            Me.InstanceNumber = aiInstanceNumber
            Me.X = aiX
            Me.Y = aiY

        End Sub

        Public Function EqualsBySymbolRoleId(ByVal other As FBM.ConceptInstance) As Boolean

            If Me.Symbol = other.Symbol And Me.RoleId = other.RoleId Then
                Return True
            Else
                Return False
            End If

        End Function

    End Class
End Namespace