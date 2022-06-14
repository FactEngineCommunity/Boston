Imports System.ComponentModel
Imports MindFusion.Diagramming
Imports System.Xml.Serialization
Imports System.Reflection

Namespace CMML
    <Serializable()>
    Public Class Actor
        Implements IEquatable(Of CMML.Actor)

        Public Model As CMML.Model

        <XmlAttribute()>
        Public Shadows ConceptType As pcenumConceptType = pcenumConceptType.Actor

        <CategoryAttribute("Actor"),
             DefaultValueAttribute(GetType(String), ""),
             DescriptionAttribute("Name of the Actor.")>
        Public Shadows _name As String = ""
        Public Property Name() As String
            Get
                Return Me._name
            End Get
            Set(ByVal Value As String)
                Me._name = Value
            End Set
        End Property

        Public FBMModelElement As FBM.ModelObject

        ''' <summary>
        ''' The SequenceNr assigned to the Actor in (say) an EventTraceDiagram.
        ''' </summary>
        ''' <remarks></remarks>
        Public SequenceNr As Single = 1

        Public process As List(Of CMML.Process)

        <NonSerialized(),
        XmlIgnore()>
        Public NameShape As ShapeNode

        Public Sub New()
            '-----------------------------------
            'Default Parameterless constructor
            '-----------------------------------
        End Sub

        Public Sub New(ByRef arModel As CMML.Model, ByVal asActorName As String, Optional ByRef arModelElement As FBM.ModelObject = Nothing)

            Me.Model = arModel
            Me.Name = asActorName
            Me.FBMModelElement = arModelElement

        End Sub

        Public Shadows Function Equals(ByVal other As CMML.Actor) As Boolean Implements System.IEquatable(Of CMML.Actor).Equals

            Return Me.Name = other.Name

        End Function

        Public Shadows Function EqualsByName(ByVal other As CMML.Actor) As Boolean

            If other.Name Like (Me.Name) Then
                Return True
            Else
                Return False
            End If

        End Function

        Public Shadows Function EqualsByNameLike(ByVal other As FBM.EntityType) As Boolean

            If other.Name Like (Me.Name & "*") Then
                Return True
            Else
                Return False
            End If

        End Function

        Public Shared Function CompareName(x As CMML.Actor, y As CMML.Actor) As Integer

            Return String.Compare(x.Name, y.Name)

        End Function

        Public Shared Function CompareSequenceNrs(ByVal aoA As UML.Actor, ByVal aoB As UML.Actor) As Integer

            '------------------------------------------------------
            'Used as a delegate within 'SortRoleGroup'
            '------------------------------------------------------
            Dim loa As New Object
            Dim lob As New Object

            Try
                Return aoA.SequenceNr - aoB.SequenceNr

            Catch ex As Exception
                prApplication.ThrowErrorMessage(ex.Message, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Function

    End Class

End Namespace