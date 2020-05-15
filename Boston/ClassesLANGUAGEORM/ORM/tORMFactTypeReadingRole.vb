Imports System.Reflection
Imports System.Xml.Serialization

Namespace FBM
    <Serializable()> _
    Public Class FactTypeReadingRole
        Inherits FBM.ModelObject

        Public FactTypeReadingId As String 'ForeignKey Reference to the MetaModelFactType table.
        Public RoleId As String 'ForeignKey Reference to the MetaModelRole table.
        Public SequenceNr As Integer 'Redundantly stores the SequenceNr of the Role within the respective FactType.
        Public PreBoundText As String 'Prebound text, normally in the form "SSSS-", preceding the ModelElement(Name) referenced by the referenced Role.
        Public PostBoundText As String 'Postbound text, normally in the form "-SSSS", following the ModelElement(Name) referenced by the referenced Role.
        Public PredicatePart As String 'The PredicatePart. e.g. "drives a" in Predicate "Person drives a CarType".

    End Class

End Namespace
