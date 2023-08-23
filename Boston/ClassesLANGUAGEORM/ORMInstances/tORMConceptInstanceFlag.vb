Imports System.Xml.Serialization

Namespace FBM
    '-----------------------------------------------------------------------------------------------------
    'Everything that can be displayed on the screen is a 'Symbol', even if it is an ASCII text character
    '  or logic 'Symbol'
    '-----------------------------------------------------------------------------------------------------
    <Serializable()>
    Public Class ConceptInstanceFlag
        Inherits FBM.ConceptInstance
        Implements IEquatable(Of FBM.ConceptInstanceFlag)

        <XmlAttribute>
        Public Flag As pcenumConceptInstanceFlag

        Public Sub New()
            '-------------------
            'Parameterless New
            '-------------------
        End Sub

        Public Sub New(ByRef arModel As FBM.Model,
                       ByRef arPage As FBM.Page,
                       ByVal asSymbol As String,
                       ByVal aiConceptType As pcenumConceptType,
                       ByVal aiInstanceNumber As Integer,
                       ByVal aiConceptInstanceFlag As pcenumConceptInstanceFlag
                       )

            Me.ModelId = arModel.ModelId
            Me.PageId = arPage.PageId
            Me.Symbol = asSymbol
            Me.ConceptType = aiConceptType
            Me.InstanceNumber = aiInstanceNumber
            Me.Flag = aiConceptInstanceFlag

        End Sub

        Public Shadows Function Equals(other As ConceptInstanceFlag) As Boolean Implements IEquatable(Of ConceptInstanceFlag).Equals
            Return Me.ModelId = other.ModelId And
                   Me.PageId = other.PageId And
                   Me.Symbol = other.Symbol And
                   Me.ConceptType = other.ConceptType And
                   Me.InstanceNumber = other.InstanceNumber And
                   Me.Flag = other.Flag
        End Function

    End Class

End Namespace