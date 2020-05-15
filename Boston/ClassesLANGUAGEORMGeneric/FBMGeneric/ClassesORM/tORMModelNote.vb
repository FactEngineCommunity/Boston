Imports System.Xml.Serialization

Namespace FBM
    <Serializable()> _
    Public Class ModelNote
        Inherits FBM.ModelObject
        Implements ICloneable
        Implements iMDAObject

        Public Text As String = "" 'The text of the ModelNote
        Public JoinedObjectType As FBM.ModelObject

        <XmlIgnore()> _
        <DebuggerBrowsable(DebuggerBrowsableState.Never)> _
        Private _IsMDAModelElement As Boolean = False
        <XmlAttribute()> _
        Public Property IsMDAModelElement() As Boolean Implements iMDAObject.IsMDAModelElement
            Get
                Return Me._IsMDAModelElement
            End Get
            Set(ByVal value As Boolean)
                Me._IsMDAModelElement = value
            End Set
        End Property

        Public Event TextChanged(ByVal asNewText As String)
        Public Event JoinedObjectTypeChanged(ByVal arJoinedModelObject As FBM.ModelObject)

        ''' <summary>
        ''' Parameterless constructor
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub New()

            Me.ConceptType = pcenumConceptType.ModelNote
            Me.Id = System.Guid.NewGuid.ToString

        End Sub

        Public Sub New(ByRef arModel As FBM.Model)

            Call Me.New()
            Me.Model = arModel

        End Sub

        Public Overloads Function Clone(ByRef arModel As FBM.Model) As Object

            Dim lrModelNote As New FBM.ModelNote

            With Me
                lrModelNote.Model = arModel
                lrModelNote.Text = .Text
            End With

            Return lrModelNote

        End Function

        Public Overrides Function CanSafelyRemoveFromModel() As Boolean
            Return False
        End Function

        ''' <summary>
        ''' Returns the unique Signature of the ModelNote
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overrides Function GetSignature() As String

            Dim lsSignature As String

            lsSignature = Me.Id
            lsSignature &= Me.JoinedObjectType.Id
            lsSignature &= Me.Text

            Return lsSignature

        End Function

        ''' <summary>
        ''' Prototype for the Save function.
        ''' </summary>
        ''' <remarks></remarks>
        Public Overridable Overloads Sub Save()
        End Sub

        Public Sub SetText(ByVal asText As String)

            Me.Text = asText
            RaiseEvent TextChanged(asText)

        End Sub

        Public Sub SetJoinedModelObject(ByRef arModelObject As FBM.ModelObject)

            Me.JoinedObjectType = arModelObject
            RaiseEvent JoinedObjectTypeChanged(arModelObject)

        End Sub

    End Class
End Namespace
