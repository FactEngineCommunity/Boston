Imports System.Xml.Serialization

Namespace FBM
    <Serializable()> _
    Public Class Fact
        Inherits Viev.FBM.Fact

        Public Overloads Function CloneInstance(ByRef arPage As FBM.Page) As FBM.FactInstance

            Dim lrFactInstance As New FBM.FactInstance
            Dim lrFactData As FBM.FactData

            Try
                With Me
                    lrFactInstance.Model = arPage.Model
                    lrFactInstance.Page = arPage
                    lrFactInstance.Fact = Me
                    lrFactInstance.Id = .Id

                    Dim lrFactTypeInstance As New FBM.FactTypeInstance(arPage.Model, arPage, pcenumLanguage.ORMModel, .FactType.Id, .FactType.Name)
                    lrFactInstance.FactType = arPage.FactTypeInstance.Find(AddressOf lrFactTypeInstance.Equals)

                    For Each lrFactData In .Data
                        lrFactInstance.Data.Add(lrFactData.CloneInstance(arPage, lrFactInstance))
                    Next
                End With

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

            Return lrFactInstance

        End Function


        Public Overrides Function RemoveFromModel(Optional ByVal abForceRemoval As Boolean = False, _
                                                  Optional ByVal abCheckForErrors As Boolean = True) As Boolean

            Dim lrFactData As FBM.FactData
            Dim lrFactType As New FBM.FactType
            Dim larFactData As New System.Collections.Generic.List(Of FBM.FactData)

            For Each lrFactData In Me.Data
                larFactData.Add(lrFactData)
            Next

            lrFactType = Me.FactType
            lrFactType.Fact.Remove(Me)
            Call TableFact.DeleteFact(Me)

            For Each lrFactData In larFactData
                Call lrFactData.RemoveFromModel()
            Next

            Dim lrDictionaryEntry As New FBM.DictionaryEntry(Me.Model, Me.Symbol, pcenumConceptType.Fact)
            Me.Model.RemoveDictionaryEntry(lrDictionaryEntry)

        End Function

        Public Overloads Overrides Sub Save()

            Dim lrFactData As FBM.FactData

            Try
                '-----------
                'Save Fact
                '-----------
                If TableFact.ExistsFact(Me) Then
                    Call TableFact.UpdateFact(Me)
                Else
                    Call TableFact.AddFact(Me)
                End If

                For Each lrFactData In Me.Data
                    lrFactData.Save()
                Next

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
