Namespace CMML

    Public Class Model
        Inherits FBM.Model

        Public Actor As New List(Of CMML.tActor)
        Public Process As New List(Of CMML.Process)

        Public Sub AddActor()

        End Sub

        Public Sub AddProcess()

        End Sub


        ''' <summary>
        ''' 20220613-Obsolete
        ''' </summary>
        Public Sub CreateActor()

            '----------------------------------------------------------
            'Establish a new Actor(EntityType) for the dropped object.
            '  i.e. Establish the EntityType within the Model as well
            '  as creating a new object for the Actor.
            '----------------------------------------------------------
            Dim lrEntityType As FBM.EntityType = Me.CreateEntityType

            lrEntityType.SetName("New Actor")
            Dim liCount As Integer
            liCount = Me.EntityType.FindAll(AddressOf lrEntityType.EqualsByNameLike).Count
            lrEntityType.Name &= " " & (CStr(liCount) + 1)

            '--------------------------------------------
            'Set the ParentEntityType for the new Actor
            '--------------------------------------------
            Dim lrParentEntityType As FBM.EntityType = New FBM.EntityType(Me, pcenumLanguage.ORMModel, "Actor", "Actor")
            lrParentEntityType = Me.EntityType.Find(AddressOf lrParentEntityType.Equals)

            lrEntityType.parentModelObjectList.Add(lrParentEntityType)

            '---------------------------------------------------
            'Find the Core Page that lists Actor (EntityTypes)
            '---------------------------------------------------
            Dim lrPage As New FBM.Page(Me, "CoreActorEntityTypes", "CoreActorEntityTypes", pcenumLanguage.ORMModel)
            lrPage = Me.Page.Find(AddressOf lrPage.EqualsByName)
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

            lrRecordset = Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)

            If CInt(lrRecordset("Count").Data) > 0 Then

                lsUniqueDataStoreName = Me.CreateUniqueDataStoreName(arDataStore, aiStartingInd + 1)

            End If

            Return lsUniqueDataStoreName

        End Function

        Public Function CreateUniqueProcessName(ByVal arProcess As CMML.Process, Optional ByVal aiStartingInd As Integer = 0) As String

            Dim lsUniqueProcessName As String = ""
            Dim lsSQLQuery As String = ""
            Dim lrRecordset As ORMQL.Recordset

            If aiStartingInd = 0 Then
                lsUniqueProcessName = arProcess.Name
            Else
                lsUniqueProcessName = arProcess.Name & aiStartingInd.ToString
            End If


            lsSQLQuery = "SELECT COUNT(*)"
            lsSQLQuery &= " FROM " & pcenumCMMLRelations.CoreElementHasElementType.ToString
            lsSQLQuery &= " WHERE " & pcenumCMML.Element.ToString & " = '" & lsUniqueProcessName & "'"

            lrRecordset = Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)

            If CInt(lrRecordset("Count").Data) > 0 Then

                lsUniqueProcessName = Me.CreateUniqueProcessName(arProcess, aiStartingInd + 1)

            End If

            Return lsUniqueProcessName

        End Function

        Public Sub CreateProcess()

        End Sub

    End Class

End Namespace
