Imports System.Xml.Serialization

Namespace FBM
    <Serializable()> _
    Public Class FactTypeReading
        Inherits Viev.FBM.FactTypeReading

        Public Sub New(ByRef arFactType As FBM.FactType, ByRef aarRole As List(Of FBM.Role), ByVal arSentence As Language.Sentence)

            Me.New(arFactType)

            Try
                Dim liInd As Integer = 0
                Dim lsPredicatePart As String = ""
                Dim lrPredicatePart As FBM.PredicatePart

                Me.FrontText = arSentence.FrontText
                Me.FollowingText = arSentence.FollowingText

                For liInd = 1 To aarRole.Count
                    Me.RoleList.Add(aarRole(liInd - 1))
                    lrPredicatePart = New FBM.PredicatePart(Me.Model, Me)
                    lrPredicatePart.SequenceNr = liInd
                    lrPredicatePart.Role = aarRole(liInd - 1)
                    If arSentence.PredicatePart.Count = aarRole.Count - 1 Then
                        lrPredicatePart.PredicatePartText = ""
                        lrPredicatePart.PreBoundText = ""
                        lrPredicatePart.PostBoundText = ""
                    Else
                        lrPredicatePart.PredicatePartText = arSentence.PredicatePart(liInd - 1).PredicatePartText
                        lrPredicatePart.PreBoundText = arSentence.PredicatePart(liInd - 1).PreboundText
                        lrPredicatePart.PostBoundText = arSentence.PredicatePart(liInd - 1).PostboundText
                    End If
                    Me.PredicatePart.Add(lrPredicatePart)
                Next

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                Me.Model.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace, True)
            End Try

        End Sub

        Public Shadows Function CloneInstance(ByVal arPage As FBM.Page) As FBM.FactTypeReadingInstance

            Dim lrFactTypeReadingInstance As New FBM.FactTypeReadingInstance

            With Me
                lrFactTypeReadingInstance.Model = arPage.Model
                lrFactTypeReadingInstance.Page = arPage
                lrFactTypeReadingInstance.Id = .Id
                lrFactTypeReadingInstance.FactTypeReading = Me
                lrFactTypeReadingInstance.FrontText = .FrontText
                lrFactTypeReadingInstance.FollowingText = .FollowingText
                lrFactTypeReadingInstance.PredicatePart = .PredicatePart
                Dim lrFactTypeInstance As New FBM.FactTypeInstance(Me.Model, arPage, pcenumLanguage.ORMModel, Me.FactType.Name, True)
                lrFactTypeInstance = arPage.FactTypeInstance.Find(AddressOf lrFactTypeInstance.Equals)
                lrFactTypeReadingInstance.FactType = lrFactTypeInstance
            End With

            Return lrFactTypeReadingInstance

        End Function


        Public Overrides Function RemoveFromModel(Optional ByVal abForceRemoval As Boolean = False, _
                                                  Optional ByVal abCheckForErrors As Boolean = True) As Boolean

            Try
                Call Me.FactType.RemoveFactTypeReading(Me)

                Call TableFactTypeReading.DeleteFactTypeReading(Me)

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                Me.Model.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace, True)
            End Try

        End Function


        ''' <summary>
        ''' Removes PredicatePart/s from FactTypeReading for an ObjectType joined to a Role/s of the FactType of the FactTypeReading.
        ''' When a user removes a Role from a FactType, there is no option but to remove all 
        ''' PredicateParts (in FactTypeReadings for the FactType) that contain the ObjectType
        ''' joined by the Role being removed.
        ''' </summary>
        ''' <param name="arRole">The Role for which the associated PredicatePart will be removed</param>
        ''' <remarks></remarks>
        Public Overrides Sub RemovePredicatePartForRole(ByRef arRole As FBM.Role)

            Dim lrPredicatePart As FBM.PredicatePart
            Dim lrPredicatePartInd As FBM.PredicatePart
            Dim lrRoleToBeRemoved As FBM.Role

            Try
                lrRoleToBeRemoved = arRole

                lrPredicatePart = Me.PredicatePart.Find(Function(x) x.Role.Id = lrRoleToBeRemoved.Id)
                Dim liSequenceNrBeingRemoved As Integer = lrPredicatePart.SequenceNr

                '--------------------------------------------------------------
                'Remove the last PredicatePart for the FTR from the database.
                '--------------------------------------------------------------
                lrPredicatePart.SequenceNr = Me.PredicatePart.Count
                Call tableORMPredicatePart.DeletePredicatePart(lrPredicatePart)

                Me.PredicatePart.Remove(lrPredicatePart)

                For Each lrPredicatePartInd In Me.PredicatePart.FindAll(Function(x) x.SequenceNr >= liSequenceNrBeingRemoved)
                    lrPredicatePartInd.SequenceNr -= 1
                    tableORMPredicatePart.UpdatePredicatePart(lrPredicatePartInd)
                Next

                '---------------------------------------------------------------------------------------------------------
                'Make sure that the last PredicatePart of the FactTypeReading has a "" (empty string) PredicatePartText.
                '---------------------------------------------------------------------------------------------------------
                Me.PredicatePart(Me.PredicatePart.Count - 1).PredicatePartText = ""

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                Me.Model.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace, True)
            End Try

        End Sub

        Public Overloads Overrides Sub Save()

            Try
                '-------------------------------------
                'Saves the FactTypeReading to the database
                '-------------------------------------
                If TableFactTypeReading.ExistsFactTypeReading(Me) Then
                    Call TableFactTypeReading.UpdateFactTypeReading(Me)
                Else
                    Call TableFactTypeReading.AddFactTypeReading(Me)
                End If

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
