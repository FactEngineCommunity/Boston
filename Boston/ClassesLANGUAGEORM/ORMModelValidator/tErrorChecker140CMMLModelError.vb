Imports System.Reflection

Namespace Validation

    Public Class ErrorCheckerCMMLModelError
        Inherits Validation.ErrorChecker

        Public Sub New(ByRef arModel As FBM.Model)
            Call MyBase.New(arModel)

        End Sub

        Public Overrides Sub CheckForErrors()
            MyBase.CheckForErrors()

            Dim lrFactType As FBM.FactType
            Dim lrFactData As FBM.FactData
            Dim lsErrorMessage As String = ""
            Dim lrModelError As FBM.ModelError

            Try

                '================================================================
                'RDS Column not in corresponding Database Table/NodeType/Entity
                '================================================================
                Me.Model.connectToDatabase(False)

                If Me.Model.DatabaseConnection IsNot Nothing Then

                    For Each lrTable In Me.Model.RDS.Table

                        Dim larDatabaseColumn = Me.Model.DatabaseConnection.getColumnsByTable(lrTable)

                        For Each lrColumn In lrTable.Column

                            If larDatabaseColumn.Find(Function(x) x.Name = lrColumn.Name Or x.Name = lrColumn.DBName) Is Nothing Then

                                lsErrorMessage = "Column/Property, " & lrColumn.Name & ", is not represented in the database, for the Table/Node Type/Entity, " & lrTable.Name & "."

                                lrModelError = New FBM.ModelError(pcenumModelErrors.CMMLModelError,
                                                                  lsErrorMessage,
                                                                  Nothing,
                                                                  lrTable.FBMModelElement, True, pcenumModelSubErrorType.RDSColumnNotInCorrespondingDatabaseEntity, lrTable)

                            End If
                        Next
                    Next
                End If



                'For Each lrFactType In Me.Model.FactType

                '    If lrFactType.Fact.Count > 0 Then

                '        Dim larModelErrorFactData = (From Fact In lrFactType.Fact
                '                                     From FactData In Fact.Data
                '                                     Where FactData.Data = ""
                '                                     Select FactData).ToList

                '        For Each lrFactData In larModelErrorFactData

                '            lsErrorMessage = "Population contains NULL value error - "
                '            lsErrorMessage &= " Values include, {"
                '            Dim liInd As Integer = 0
                '            For Each lrFactFactData In lrFactData.Fact.Data
                '                If liInd > 0 Then lsErrorMessage &= ", "
                '                lsErrorMessage &= lrFactFactData.Data
                '                liInd += 1
                '            Next
                '            lsErrorMessage &= "}"

                '            lrModelError = New FBM.ModelError(pcenumModelErrors.PopulationContainstNULLValueError,
                '                                              lsErrorMessage,
                '                                              Nothing,
                '                                              lrFactType)

                '            lrFactType._ModelError.AddUnique(lrModelError)
                '            lrFactData.Fact.AddModelError(lrModelError)
                '            lrFactData.AddModelError(lrModelError)
                '            Me.Model.AddModelError(lrModelError)

                '        Next

                '    End If
                'Next

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

    End Class

End Namespace
