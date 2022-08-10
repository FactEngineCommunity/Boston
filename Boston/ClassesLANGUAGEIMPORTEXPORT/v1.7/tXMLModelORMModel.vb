Imports System.Reflection
Imports System.Xml.Serialization

Namespace XMLModel
    <Serializable()> _
    Public Class ORMModel

        <XmlAttribute()> _
        Public ModelId As String = ""

        <XmlAttribute()>
        Public Name As String = ""

        <XmlAttribute()>
        Public CoreVersionNumber As String = ""

        Public ValueTypes As New List(Of XMLModel.ValueType)
        Public EntityTypes As New List(Of XMLModel.EntityType)
        Public FactTypes As New List(Of XMLModel.FactType)
        Public RoleConstraints As New List(Of XMLModel.RoleConstraint)
        Public ModelNotes As New List(Of XMLModel.ModelNote)

        ''' <summary>
        ''' Parameterless Constructor
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub New()
            '-------------------
            'Parameterless New
            '-------------------
        End Sub

        Public Function getModelElementById(ByVal asModelElementId As String) As Object

            Try

                Dim lrValueType = Me.ValueTypes.Find(Function(x) x.Id = asModelElementId)
                Dim lrEntityType = Me.EntityTypes.Find(Function(x) x.Id = asModelElementId)
                Dim lrFactType = Me.FactTypes.Find(Function(x) x.Id = asModelElementId)
                Dim lrRoleConstraint = Me.RoleConstraints.Find(Function(x) x.Id = asModelElementId)
                Dim lrModelNote = Me.ModelNotes.Find(Function(x) x.Id = asModelElementId)

                If lrValueType IsNot Nothing Then
                    Return lrValueType
                ElseIf lrEntityType IsNot Nothing Then
                    Return lrEntityType
                ElseIf lrFactType IsNot Nothing Then
                    Return lrFactType
                ElseIf lrRoleConstraint IsNot Nothing Then
                    Return lrRoleConstraint
                ElseIf lrModelNote IsNot Nothing Then
                    Return lrModelNote
                Else
                    Return Nothing
                End If

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)

                Return Nothing
            End Try

        End Function

    End Class
End Namespace