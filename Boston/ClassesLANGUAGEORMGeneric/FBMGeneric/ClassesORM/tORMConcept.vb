Imports System.ComponentModel
Imports System.Xml.Serialization

Namespace FBM
    <Serializable()> _
    Public Class Concept
        Implements IEquatable(Of FBM.Concept)

        <XmlIgnore()> _
        <System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)> _
        <DebuggerBrowsable(DebuggerBrowsableState.Never)> _
        Public _Symbol As String = ""
        <XmlAttribute()> _
        <Browsable(False)> _
        Public Property Symbol() As String
            Get
                Return Me._Symbol
            End Get
            Set(ByVal value As String)
                Me._Symbol = value
                RaiseEvent ConceptSymbolUpdated()
            End Set
        End Property


        Public Event ConceptSymbolUpdated() 'Used to let Proxy 'Instances' in the MVC (Model View Controler) attach
        ' delegates to the object they represent such that if the ModelObject is 'updated'
        ' (e.g. an EntityType has its Name changed) from one Page (e.g. on a 'Use Case Diagram'), then the 
        ' 'Instances' of that same ModelObject will be notifed immediately (e.g. in a DataFlowDiagram 
        ' that shows the same ModelObject by MVC Proxy).


        Public Sub New()
            '---------
            'Default
            '---------
            Me.Symbol = System.Guid.NewGuid.ToString
        End Sub

        Public Sub New(ByVal asSymbol As String)

            Me.Symbol = asSymbol

        End Sub

        Public Shadows Function Equals(ByVal other As FBM.Concept) As Boolean Implements System.IEquatable(Of FBM.Concept).Equals

            If LCase(Me.Symbol) = LCase(other.Symbol) Then
                Return True
            Else
                Return False
            End If

        End Function

        Public Function Clone() As FBM.Concept

            Dim lrConcept As New FBM.Concept(Me.Symbol)

            Return lrConcept

        End Function

        Public Function CloneModelObject(ByRef arModel As FBM.Model) As FBM.ModelObject

            Dim lrModelObject As New FBM.ModelObject

            lrModelObject.Model = arModel
            lrModelObject.Concept = Me.Clone
            'lrModelObject.ConceptType = TBA
            lrModelObject.Id = Me.Symbol
            lrModelObject.Name = Me.Symbol
            lrModelObject.Symbol = Me.Symbol
            lrModelObject.Instance.Add(Me.Symbol)
            lrModelObject.LongDescription = ""
            lrModelObject.ShortDescription = ""

            Return lrModelObject

        End Function

        ''' <summary>
        ''' Prototype for the Save function.
        ''' </summary>
        ''' <remarks></remarks>
        Public Overridable Sub Save()
        End Sub

    End Class
End Namespace
