Imports System.Reflection

Namespace DataLineage

    Public Class DataLineageItem
        Implements IEquatable(Of DataLineageItem)

        Public Model As FBM.Model

        Public Name As String 'DataLineageItemName

        Public DataLineageCategory As New DataLineage.DataLineageCategory

        Public DataLineageItemProperty As New List(Of DataLineage.DataLineageItemProperty)

        'Parameterless Constructor
        Public Sub New()
        End Sub

        Public Sub New(ByRef arModel As FBM.Model, ByVal asName As String)
            Try
                Me.Model = arModel
                Me.Name = asName
            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
            End Try
        End Sub

        Public Shadows Function Equals(other As DataLineageItem) As Boolean Implements IEquatable(Of DataLineageItem).Equals
            Return Me.Name = other.Name
        End Function

        Public Sub SaveProperties(ByVal arFEKLLineageObject As FEKL.FEKL4JSONObject,
                                  Optional ByRef abIgnoreErrors As Boolean = False)

            Try
#Region "Metadata Lineage"
                'NB More than one Lineage Item Property can be stored for a Model Element, even if the Model Element already exists.
                '  I.e. Process Metadata Lineage before checking to see if the Model Element already exists.


                'Lineage Category
                Dim lrDataLineageCategory = New DataLineage.DataLineageCategory(Me.Model, "Metadata Lineage", 1)
                Me.DataLineageCategory = lrDataLineageCategory

                'Lineage Property/ies
                Dim liLineageSetNumber = tableDataLineageItemProperty.getHighestLineageSetNrForDataLineageItemCategory(Me.Model,
                                                                                                                        Me.Name,
                                                                                                                        lrDataLineageCategory.Name)

                liLineageSetNumber += 1

                'Document Name
                Dim lrDataLineageProperty As New DataLineage.DataLineageItemProperty(Me.Model, Me.Name, "Specification Document Name", arFEKLLineageObject.DocumentName, liLineageSetNumber)
                lrDataLineageProperty.Category = Me.DataLineageCategory.Name
                Me.DataLineageItemProperty.Add(lrDataLineageProperty)
                'Document Location
                lrDataLineageProperty = New DataLineage.DataLineageItemProperty(Me.Model, Me.Name, "Document Location", arFEKLLineageObject.DocumentLocation, liLineageSetNumber)
                lrDataLineageProperty.Category = Me.DataLineageCategory.Name
                Me.DataLineageItemProperty.Add(lrDataLineageProperty)
                'Document Location JSON
                lrDataLineageProperty = New DataLineage.DataLineageItemProperty(Me.Model, Me.Name, "Document Location JSON", arFEKLLineageObject.DocumentLocationJson, liLineageSetNumber)
                lrDataLineageProperty.Category = Me.DataLineageCategory.Name
                Me.DataLineageItemProperty.Add(lrDataLineageProperty)
                'Page Number
                lrDataLineageProperty = New DataLineage.DataLineageItemProperty(Me.Model, Me.Name, "Page Number", arFEKLLineageObject.PageNumber, liLineageSetNumber)
                lrDataLineageProperty.Category = Me.DataLineageCategory.Name
                Me.DataLineageItemProperty.Add(lrDataLineageProperty)
                'Line Number
                lrDataLineageProperty = New DataLineage.DataLineageItemProperty(Me.Model, Me.Name, "Line Number", arFEKLLineageObject.LineNumber, liLineageSetNumber)
                lrDataLineageProperty.Category = Me.DataLineageCategory.Name
                Me.DataLineageItemProperty.Add(lrDataLineageProperty)
                'Section Id
                lrDataLineageProperty = New DataLineage.DataLineageItemProperty(Me.Model, Me.Name, "Section Id", arFEKLLineageObject.SectionId, liLineageSetNumber)
                lrDataLineageProperty.Category = Me.DataLineageCategory.Name
                Me.DataLineageItemProperty.Add(lrDataLineageProperty)
                'Section Name
                lrDataLineageProperty = New DataLineage.DataLineageItemProperty(Me.Model, Me.Name, "Section Name", arFEKLLineageObject.SectionName, liLineageSetNumber)
                lrDataLineageProperty.Category = Me.DataLineageCategory.Name
                Me.DataLineageItemProperty.Add(lrDataLineageProperty)
                'Requirement Id
                lrDataLineageProperty = New DataLineage.DataLineageItemProperty(Me.Model, Me.Name, "Requirement Id", arFEKLLineageObject.RequirementId, liLineageSetNumber)
                lrDataLineageProperty.Category = Me.DataLineageCategory.Name
                Me.DataLineageItemProperty.Add(lrDataLineageProperty)
                'Start Offset
                lrDataLineageProperty = New DataLineage.DataLineageItemProperty(Me.Model, Me.Name, "Start Offset", arFEKLLineageObject.StartOffset, liLineageSetNumber)
                lrDataLineageProperty.Category = Me.DataLineageCategory.Name
                Me.DataLineageItemProperty.Add(lrDataLineageProperty)
                'End Offset
                lrDataLineageProperty = New DataLineage.DataLineageItemProperty(Me.Model, Me.Name, "End Offset", arFEKLLineageObject.EndOffset, liLineageSetNumber)
                lrDataLineageProperty.Category = Me.DataLineageCategory.Name
                Me.DataLineageItemProperty.Add(lrDataLineageProperty)

                For Each lrDataLineageProperty In Me.DataLineageItemProperty
                    Call lrDataLineageProperty.Save(abIgnoreErrors)
                Next


SkipLineage:
#End Region

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
