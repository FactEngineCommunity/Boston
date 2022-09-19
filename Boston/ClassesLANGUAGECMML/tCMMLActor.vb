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

        Public WithEvents FBMModelElement As FBM.ModelObject

        ''' <summary>
        ''' The SequenceNr assigned to the Actor in (say) an EventTraceDiagram.
        ''' </summary>
        ''' <remarks></remarks>
        Public SequenceNr As Single = 1

        ''' <summary>
        ''' Processes participated with by the Actor
        ''' </summary>
        Public Process As New List(Of CMML.Process)

        <NonSerialized(),
        XmlIgnore()>
        Public NameShape As ShapeNode

        Public Event NameChanged(ByVal asNewName As String)
        Public Event RemovedFromModel()

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
                prApplication.ThrowErrorMessage(ex.Message, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
            End Try

        End Function

        Public Sub RemoveFromModel()

            Try
                Dim lrModelElement As FBM.ModelObject = Me.Model.Model.GetModelObjectByName(Me.Name)

                If lrModelElement IsNot Nothing Then
                    Select Case lrModelElement.GetType
                        Case Is = GetType(FBM.EntityType)
                            CType(lrModelElement, FBM.EntityType).setIsActor(False)
                        Case Is = GetType(FBM.FactType)
                            Throw New NotImplementedException("Not implemented for FactTypes that are Actors.")
                    End Select
                End If

                Call Me.Model.RemoveActor(Me)

                RaiseEvent RemovedFromModel()

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
            End Try

        End Sub

        Public Sub setName(ByVal asNewName As String)

            Try
                Dim lsOldName As String = Me.Name

                Me.Name = asNewName

                Me.FBMModelElement.setName(asNewName)

                'CMML
                Me.Model.Model.updateCMMLActorName(lsOldName, asNewName)

                RaiseEvent NameChanged(asNewName)

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
            End Try

        End Sub

        Private Sub FBMModelElement_NameChanged(asOldName As String, asNewName As String) Handles FBMModelElement.NameChanged

            Try
                Dim lsOldName = Me.Name

                Me.Name = asNewName

                'CMML
                Me.Model.Model.updateCMMLActorName(lsOldName, asNewName)

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
            End Try

        End Sub

    End Class

End Namespace