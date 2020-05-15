Imports System.ComponentModel
Imports System.Xml.Serialization

Namespace FBM
    <Serializable()> _
    Public Class tUniquenessConstraint
        Inherits FBM.RoleConstraintInstance

        <CategoryAttribute("Identification"), _
        Browsable(True), _
        DefaultValueAttribute(False), _
        DescriptionAttribute("Is this the preferred identifier for the opposite Role?")> _
        Public Overrides Property IsPreferredIdentifier() As Boolean
            Get
                Return _IsPreferredIdentifier
            End Get
            Set(ByVal value As Boolean)
                _IsPreferredIdentifier = value
            End Set
        End Property

    End Class
End Namespace
