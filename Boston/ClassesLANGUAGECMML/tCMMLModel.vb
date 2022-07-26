Imports System.Reflection

Namespace CMML

    Public Class Model
        '20220614-Was. Not sure why it was a subtype of FBM.Model
        'Inherits FBM.Model

        Public WithEvents Model As FBM.Model

        Public Actor As New List(Of CMML.Actor)
        Public Process As New List(Of CMML.Process)

        Public ActorProcessRelation As New List(Of CMML.ActorProcessRelation)

        Public ProcessProcessRelation As New List(Of CMML.ProcessProcessRelation)

        Public Event ActorAdded(ByRef arActor As CMML.Actor)
        Public Event ActorRemoved(ByRef arTable As CMML.Actor)
        Public Event ActorProcessRelationAdded(ByRef arActorProcessRelation As CMML.ActorProcessRelation)
        Public Event ActorProcessRelationRemoved(ByRef arActorProcessRelation As CMML.ActorProcessRelation)
        Public Event ProcessProcessRelationAdded(ByRef arProcessProcessRelation As CMML.ProcessProcessRelation)
        Public Event ProcessProcessRelationRemoved(ByRef arProcesProcessRelation As CMML.ProcessProcessRelation)
        Public Event ProcessAdded(ByRef arProcess As CMML.Process)
        Public Event ProcessRemoved(ByVal arProcess As CMML.Process)

        Public Sub New(ByRef arModel As FBM.Model)

            Me.Model = arModel

        End Sub

        Public Sub addActor(ByRef arActor As CMML.Actor)

            Try
                Dim lrActor As CMML.Actor = arActor

                'CodeSafe - Check that the Actor doesn't already exist
                If Me.Actor.Find(Function(x) x.Name = lrActor.Name) IsNot Nothing Then
                    'CodeSafe: Set arActor to the existing Actor.
                    '20200725-VM-This might not be the right strategy. Need to revisit. Why is a Actor being created that already exists.
                    'See FBM.FactType.Objectify for ManyToOne FactType.
                    arActor = Me.Actor.Find(Function(x) x.Name = lrActor.Name)
                    Exit Sub
                    'Throw New Exception("Actor with name, " & arActor.Name & ", already exists in the Relational Data Structure.")
                End If

                Me.Actor.AddUnique(arActor)

                Call Me.Model.createCMMLActor(arActor)

                RaiseEvent ActorAdded(arActor)

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub


        Public Sub addProcess(ByRef arProcess As CMML.Process)

            Try
                Me.Process.Add(arProcess)

                Call Me.Model.createCMMLProcess(arProcess)

#Region "Uncomment for BPMN"
                'Call Me.Model.addCMMLProcessProcessType(arProcess, arProcess.ProcessType)

                '#Region "BPMN"
                '#Region "Activity - BPMN"
                '                Call Me.Model.addCMMLProcessActivityType(arProcess, arProcess.ActivityType)

                '                Call Me.Model.addCMMLProcessActivityMarker(arProcess, arProcess.ActivityMarker)

                '                Call Me.Model.addCMMLProcessActivityTaskType(arProcess, arProcess.ActivityTaskType)
                '#End Region

                '#Region "Conversation - BPMN"
                '                Call Me.Model.addCMMLProcessConversationType(arProcess, arProcess.ConversationType)
                '#End Region

                '#Region "Events - BPMN"

                '                Call Me.Model.addCMMLProcessEventPosition(arProcess, arProcess.EventPosition)

                '                Call Me.Model.addCMMLProcessEventType(arProcess, arProcess.EventType)

                '                Call Me.Model.addCMMLProcessEventSubType(arProcess, arProcess.EventSubType)
                '#End Region

                '#Region "Gateway - BPMN"

                '                Call Me.Model.addCMMLProcessGatewayType(arProcess, arProcess.GatewayType)
                '#End Region

                '#End Region
#End Region
                RaiseEvent ProcessAdded(arProcess)

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Public Sub addActorProcessRelation(ByRef arCMMLActorProcessRelation As CMML.ActorProcessRelation)

            Try
                Me.ActorProcessRelation.AddUnique(arCMMLActorProcessRelation)
                arCMMLActorProcessRelation.Actor.Process.Add(arCMMLActorProcessRelation.Process)

                arCMMLActorProcessRelation.Fact = Me.Model.createCMMLActorProcessRelation(arCMMLActorProcessRelation)

                RaiseEvent ActorProcessRelationAdded(arCMMLActorProcessRelation)

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub


        Public Sub addProcessProcessRelation(ByRef arCMMLProcessProcessRelation As CMML.ProcessProcessRelation)

            Try
                Me.ProcessProcessRelation.AddUnique(arCMMLProcessProcessRelation)

                'CMML
                arCMMLProcessProcessRelation.Fact = Me.Model.createCMMLProcessProcessRelation(arCMMLProcessProcessRelation)

                RaiseEvent ProcessProcessRelationAdded(arCMMLProcessProcessRelation)

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub


        Public Function CreateUniqueProcessText(ByVal asRootProcessText As String, Optional aiCounter As Integer = 0) As String

            Dim lsTrialProcessText As String

            If aiCounter = 0 Then
                lsTrialProcessText = asRootProcessText
            Else
                lsTrialProcessText = asRootProcessText & CStr(aiCounter)
            End If

            CreateUniqueProcessText = lsTrialProcessText

            If Me.Process.Find(Function(x) x.Text = lsTrialProcessText) IsNot Nothing Then
                CreateUniqueProcessText = Me.CreateUniqueProcessText(asRootProcessText, aiCounter + 1)
            Else
                Return lsTrialProcessText
            End If

        End Function





        ''' <summary>
        ''' 20220613-Obsolete
        ''' </summary>
        Public Sub CreateActor()

            '----------------------------------------------------------
            'Establish a new Actor(EntityType) for the dropped object.
            '  i.e. Establish the EntityType within the Model as well
            '  as creating a new object for the Actor.
            '----------------------------------------------------------
            Dim lrEntityType As FBM.EntityType = Me.Model.CreateEntityType

            lrEntityType.SetName("New Actor")
            Dim liCount As Integer
            liCount = Me.Model.EntityType.FindAll(AddressOf lrEntityType.EqualsByNameLike).Count
            lrEntityType.Name &= " " & (CStr(liCount) + 1)

            '--------------------------------------------
            'Set the ParentEntityType for the new Actor
            '--------------------------------------------
            Dim lrParentEntityType As FBM.EntityType = New FBM.EntityType(Me.Model, pcenumLanguage.ORMModel, "Actor", "Actor")
            lrParentEntityType = Me.Model.EntityType.Find(AddressOf lrParentEntityType.Equals)

            lrEntityType.parentModelObjectList.Add(lrParentEntityType)

            '---------------------------------------------------
            'Find the Core Page that lists Actor (EntityTypes)
            '---------------------------------------------------
            Dim lrPage As New FBM.Page(Me.Model, "CoreActorEntityTypes", "CoreActorEntityTypes", pcenumLanguage.ORMModel)
            lrPage = Me.Model.Page.Find(AddressOf lrPage.EqualsByName)
            Dim lrEntityTypeInstance As FBM.EntityTypeInstance
            lrEntityTypeInstance = lrEntityType.CloneInstance(lrPage)
            lrEntityTypeInstance.X = 10
            lrEntityTypeInstance.Y = 10
            lrPage.EntityTypeInstance.Add(lrEntityTypeInstance)
            lrPage.MakeDirty()

            Call lrPage.ClearAndRefresh()

        End Sub

        Public Function CreateUniqueDataStoreName(ByVal arDataStore As DFD.DataStore, Optional ByVal aiStartingInd As Integer = 0) As String

            Dim lsUniqueDataStoreName As String = ""
            Dim lsSQLQuery As String = ""
            Dim lrRecordset As ORMQL.Recordset

            If aiStartingInd = 0 Then
                lsUniqueDataStoreName = arDataStore.Name
            Else
                lsUniqueDataStoreName = arDataStore.Name & aiStartingInd.ToString
            End If


            lsSQLQuery = "SELECT COUNT(*)"
            lsSQLQuery &= " FROM " & pcenumCMMLRelations.CoreElementHasElementType.ToString
            lsSQLQuery &= " WHERE " & pcenumCMML.Element.ToString & " = '" & lsUniqueDataStoreName & "'"

            lrRecordset = Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

            If CInt(lrRecordset("Count").Data) > 0 Then

                lsUniqueDataStoreName = Me.CreateUniqueDataStoreName(arDataStore, aiStartingInd + 1)

            End If

            Return lsUniqueDataStoreName

        End Function

        Public Function CreateUniqueProcessText(ByVal arProcess As CMML.Process, Optional ByVal aiStartingInd As Integer = 0) As String

            Dim lsUniqueProcessText As String = ""
            Dim lsSQLQuery As String = ""
            Dim lrRecordset As ORMQL.Recordset

            If aiStartingInd = 0 Then
                lsUniqueProcessText = arProcess.Text
            Else
                lsUniqueProcessText = arProcess.Text & aiStartingInd.ToString
            End If

            If Me.Process.Find(Function(x) x.Text = lsUniqueProcessText) IsNot Nothing Then
                lsUniqueProcessText = Me.CreateUniqueProcessText(arProcess, aiStartingInd + 1)
            End If

            Return lsUniqueProcessText

        End Function

        Public Sub RemoveActor(ByRef arActor As CMML.Actor)

            Try
                Me.Actor.Remove(arActor)

                'Remove all associated ActorProcessRelations
                Dim lsActorName = arActor.Name
                Dim larActorProcessRelation = From ActorProcessRelation In Me.ActorProcessRelation
                                              Where ActorProcessRelation.Actor.Name = lsActorName
                                              Select ActorProcessRelation

                For Each lrActorProcessRelation In larActorProcessRelation.ToList
                    Call lrActorProcessRelation.RemoveFromModel()
                Next

                'CMML
                Call Me.Model.removeCMMLActor(arActor)

                RaiseEvent ActorRemoved(arActor)

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Public Sub RemoveProcess(ByRef arProcess As CMML.Process)

            Try
                Me.Process.Remove(arProcess)

                'CMML
                Call Me.Model.removeCMMLProcess(arProcess)

                RaiseEvent ProcessRemoved(arProcess)

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Public Sub RemoveActorProcessRelation(ByRef arActorProcessRelation As CMML.ActorProcessRelation)

            Try
                Me.ActorProcessRelation.Remove(arActorProcessRelation)

                'CMML
                Call Me.Model.removeCMMLActorProcessRelation(arActorProcessRelation)

                RaiseEvent ActorProcessRelationRemoved(arActorProcessRelation)

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Public Sub RemoveProcessProcessRelation(ByRef arProcessProcessRelation As CMML.ProcessProcessRelation)

            Try
                Me.ProcessProcessRelation.Remove(arProcessProcessRelation)

                'CMML
                Call Me.Model.removeCMMLProcessProcessRelation(arProcessProcessRelation)

                RaiseEvent ProcessProcessRelationRemoved(arProcessProcessRelation)

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
