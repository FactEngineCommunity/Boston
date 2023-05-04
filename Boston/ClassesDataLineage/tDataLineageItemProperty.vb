Imports System.Reflection

Namespace DataLineage

    Public Class DataLineageItemProperty
        Implements IEquatable(Of DataLineageItemProperty)

        Public Model As FBM.Model

        Public Name As String 'DataLineageItemName

        Public Category As String 'DataLineageCategory

        Public PropertyType As String 'E.g. "Document", "Page Number", "Target", "Source"

        Public [Property] As String 'E.g. "Document", "Page Number", "Target", "Source""

        Public LineageSetNumber As Integer 'E.g. 1 (mostly). Counter for the Data Lineage Category allocated against a Data Lineage Item and where a Data Lineage Category can be against a Data Lineage Item more than once.

        Public [Control] As Control

        'Parameterless Constructor
        Public Sub New()
        End Sub

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="asDataLineageItemName"></param>
        ''' <param name="asPropertyType">E.g. "Document", "Page Number", "Target", "Source"</param>
        ''' <param name="asProperty">E.g. "Document", "Page Number", "Target", "Source"</param>
        ''' <param name="aiLineageSetNumber">E.g. 1 (mostly). Counter for the Data Lineage Category allocated against a Data Lineage Item and where a Data Lineage Category can be against a Data Lineage Item more than once.</param>
        Public Sub New(ByRef arModel As FBM.Model,
                       ByVal asDataLineageItemName As String,
                       ByVal asPropertyType As String,
                       ByVal asProperty As String,
                       ByVal aiLineageSetNumber As Integer)
            Try
                Me.Model = arModel
                Me.Name = asDataLineageItemName
                Me.PropertyType = asPropertyType
                Me.Property = asProperty
                Me.LineageSetNumber = aiLineageSetNumber

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
            End Try
        End Sub

        Public Shadows Function Equals(other As DataLineageItemProperty) As Boolean Implements IEquatable(Of DataLineageItemProperty).Equals
            Return Me.Name = other.Name And Me.PropertyType = other.PropertyType And Me.Property = other.Property And Me.LineageSetNumber = other.LineageSetNumber
        End Function


        Public Sub Save(Optional ByVal abIgnoreErrors As Boolean = False)

            Try
                If Me.Control Is Nothing Then
                    Exit Sub
                Else
                    Select Case Me.Control.GetType
                        Case Is = GetType(Windows.Forms.TextBox)
                            Me.Property = Trim(CType(Me.Control, Windows.Forms.TextBox).Text)
                    End Select
                End If

                If Trim(Me.Property) = "" Then
                    Call tableDataLineageItemProperty.DeleteDataLineageItemProperty(Me)
                Else
                    If tableDataLineageItemProperty.ExistsDataLineageItemProperty(Me) Then
                        Call tableDataLineageItemProperty.updateDataLineageItemProperty(Me)
                    Else
                        Call tableDataLineageItemProperty.addDataLineageItemProperty(Me)
                    End If
                End If


            Catch ex As Exception

                If abIgnoreErrors Then Exit Sub

                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
            End Try

        End Sub

    End Class

End Namespace
