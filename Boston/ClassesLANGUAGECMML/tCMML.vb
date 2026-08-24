Imports System.Reflection

Public Class tCMML

    Public Core As FBM.Model

    Public Function getORMDiagramPagesForActor(ByVal arActor As UML.Actor) As List(Of FBM.Page)

        getORMDiagramPagesForActor = New List(Of FBM.Page)

        Dim lrModel As FBM.Model

        lrModel = arActor.Model

        '---------------------------------------------------------------
        'Pages where Actor is on the Page as an EntityType or FactType
        '-------------------------------------
        Dim larPage = From Page In lrModel.Page
                      From ModelElement In Page.GetAllPageObjects
                      Where Page.Language = pcenumLanguage.ORMModel _
                      And ModelElement.Id = arActor.Data 'Could be either EntityType or FactType
                      Select Page Distinct
                      Order By Page.Name

        getORMDiagramPagesForActor = larPage.ToList


        Dim larConceptInstancePage = (From Page In lrModel.Page
                                      From ConceptInstance In Page.ConceptInstance
                                      Where Page.Language = pcenumLanguage.ORMModel
                                      Where (ConceptInstance.ConceptType = pcenumConceptType.EntityType Or
                                             ConceptInstance.ConceptType = pcenumConceptType.FactType)
                                      Where ConceptInstance.Symbol = arActor.Name
                                      Select Page Distinct
                                      Order By Page.Name).ToList

        For Each lrPage In larConceptInstancePage
            getORMDiagramPagesForActor.AddUnique(lrPage)
        Next

    End Function


    Public Function getORMDiagramPagesForEntityType(ByVal arEntityType As FBM.EntityType) As List(Of FBM.Page)

        Try
            getORMDiagramPagesForEntityType = New List(Of FBM.Page)

            Dim lrModel As FBM.Model
            Dim lrPage As FBM.Page

            lrModel = arEntityType.Model

            Dim larPage = (From Page In lrModel.Page
                           From EntityTypeInstance In Page.EntityTypeInstance
                           Where Page.Language = pcenumLanguage.ORMModel
                           Where EntityTypeInstance.EntityType IsNot Nothing
                           Where EntityTypeInstance.EntityType.Id = arEntityType.Id
                           Select Page Distinct
                           Order By Page.Name).ToList

            For Each lrPage In larPage
                getORMDiagramPagesForEntityType.Add(lrPage)
            Next

            Dim larConceptInstancePage = (From Page In lrModel.Page
                                          From ConceptInstance In Page.ConceptInstance
                                          Where Page.Language = pcenumLanguage.ORMModel
                                          Where ConceptInstance.ConceptType = pcenumConceptType.EntityType
                                          Where ConceptInstance.Symbol = arEntityType.Id
                                          Select Page Distinct
                                          Order By Page.Name).ToList

            For Each lrPage In larConceptInstancePage
                getORMDiagramPagesForEntityType.AddUnique(lrPage)
            Next

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowMessage(lsMessage, pcenumErrorType.Warning, ex.StackTrace, True,, True)

            Return New List(Of FBM.Page)
        End Try

    End Function

    Public Function getORMDiagramPagesForModelElementName(ByRef arModel As FBM.Model, ByVal asModelElementName As String) As List(Of FBM.Page)

        getORMDiagramPagesForModelElementName = New List(Of FBM.Page)

        Dim larPage = From Page In arModel.Page
                      From EntityTypeInstance In Page.EntityTypeInstance
                      Where Page.Language = pcenumLanguage.ORMModel _
                     And EntityTypeInstance.EntityType.Id = asModelElementName
                      Select Page Distinct
                      Order By Page.Name

        For Each lrPage In larPage
            getORMDiagramPagesForModelElementName.Add(lrPage)
        Next

        larPage = From Page In arModel.Page
                  From FactTypeInstance In Page.FactTypeInstance
                  Where Page.Language = pcenumLanguage.ORMModel _
                  And FactTypeInstance.FactType.Id = asModelElementName
                  Select Page Distinct
                  Order By Page.Name

        For Each lrPage In larPage
            getORMDiagramPagesForModelElementName.Add(lrPage)
        Next

        Dim larConceptInstancePage = (From Page In arModel.Page
                                      From ConceptInstance In Page.ConceptInstance
                                      Where Page.Language = pcenumLanguage.ORMModel
                                      Where (ConceptInstance.ConceptType = pcenumConceptType.EntityType Or
                                             ConceptInstance.ConceptType = pcenumConceptType.FactType)
                                      Where ConceptInstance.Symbol = asModelElementName
                                      Select Page Distinct
                                      Order By Page.Name).ToList

        For Each lrPage In larConceptInstancePage
            getORMDiagramPagesForModelElementName.AddUnique(lrPage)
        Next

    End Function

    Public Function getDataFlowDiagramPagesForActor(ByVal arActor As UML.Actor) As List(Of FBM.Page)

        getDataFlowDiagramPagesForActor = New List(Of FBM.Page)

        Dim lrModel As FBM.Model

        lrModel = arActor.Model

        Dim larPage = From Page In lrModel.Page
                      From FactTypeInstance In Page.FactTypeInstance
                      From Fact In FactTypeInstance.FactType.Fact
                      From FactData In Fact.Data
                      Where Page.Language = pcenumLanguage.DataFlowDiagram _
                      And FactTypeInstance.Name = pcenumCMMLRelations.CoreElementHasElementType.ToString _
                      And FactData.Role.Name = "Element" _
                      And FactData.Concept.Symbol = arActor.Name
                      Select Page Distinct
                      Order By Page.Name

        getDataFlowDiagramPagesForActor = larPage.ToList

    End Function

    Public Function getUseCaseDiagramPagesForProcess(ByVal arProcess As UML.Process, ByVal arExcludePage As FBM.Page) As List(Of FBM.Page)

        '================================================
        getUseCaseDiagramPagesForProcess = New List(Of FBM.Page)

        Dim lrModel As FBM.Model = arProcess.Model

        Dim larPage = From Page In lrModel.Page
                      From FactTypeInstance In Page.FactTypeInstance
                      From FactInstance In FactTypeInstance.Fact
                      From FactData In FactInstance.Data
                      Where Page.Language = pcenumLanguage.UMLUseCaseDiagram _
                      And FactTypeInstance.Name = pcenumCMMLRelations.CoreElementHasElementType.ToString _
                      And FactData.Role.Name = "Element" _
                      And FactData.Data = arProcess.Id
                      Where Page IsNot arExcludePage
                      Select Page Distinct
                      Order By Page.Name

        getUseCaseDiagramPagesForProcess = larPage.ToList


    End Function


    Public Function getDataFlowDiagramPagesForProcess(ByVal arProcess As UML.Process) As List(Of FBM.Page)

        '================================================
        getDataFlowDiagramPagesForProcess = New List(Of FBM.Page)

        Dim lrModel As FBM.Model

        lrModel = arProcess.Model

        Dim larPage = From Page In lrModel.Page
                      From FactTypeInstance In Page.FactTypeInstance
                      From Fact In FactTypeInstance.FactType.Fact
                      From FactData In Fact.Data
                      Where Page.Language = pcenumLanguage.DataFlowDiagram _
                      And FactTypeInstance.Name = pcenumCMMLRelations.CoreElementHasElementType.ToString _
                      And FactData.Role.Name = "Element" _
                      And FactData.Concept.Symbol = arProcess.Name
                      Select Page Distinct
                      Order By Page.Name

        getDataFlowDiagramPagesForProcess = larPage.ToList


    End Function


    Public Function GetERDiagramPagesForEntity(ByVal arEntity As ERD.Entity) As List(Of FBM.Page)

        GetERDiagramPagesForEntity = New List(Of FBM.Page)

        Dim lrModel As FBM.Model
        Dim lrPage As FBM.Page

        lrModel = arEntity.Model

        Dim larPage = From Page In lrModel.Page _
                      From FactType In Page.FactTypeInstance _
                      From Fact In FactType.Fact _
                      From RoleData In Fact.Data _
                      Where Page.Language = pcenumLanguage.EntityRelationshipDiagram _
                      And FactType.Name = pcenumCMMLRelations.CoreElementHasElementType.ToString _
                      And RoleData.Role.Name = pcenumCMML.Element.ToString _
                      And RoleData.Concept.Symbol = arEntity.Name _
                      Select Page Distinct _
                      Order By Page.Name

        For Each lrPage In larPage
            GetERDiagramPagesForEntity.Add(lrPage)
        Next

        Dim larConceptInstancePage = (From Page In lrModel.Page
                                      From ConceptInstance In Page.ConceptInstance
                                      Where Page.Language = pcenumLanguage.EntityRelationshipDiagram
                                      Where ConceptInstance.ConceptType = pcenumConceptType.Value
                                      Where ConceptInstance.Symbol = arEntity.Name
                                      Where ConceptInstance.RoleId = "e6ca6889-db16-470f-9106-d20387a59c3b"
                                      Select Page Distinct
                                      Order By Page.Name).ToList

        For Each lrPage In larConceptInstancePage
            GetERDiagramPagesForEntity.AddUnique(lrPage)
        Next

    End Function

    Public Function getERDiagramPagesForModelElementName(ByVal arModel As FBM.Model, ByVal asModelElementName As String) As List(Of FBM.Page)

        getERDiagramPagesForModelElementName = New List(Of FBM.Page)

        Dim larPage = From Page In arModel.Page _
                      From FactType In Page.FactTypeInstance _
                      From Fact In FactType.Fact _
                      From RoleData In Fact.Data _
                      Where Page.Language = pcenumLanguage.EntityRelationshipDiagram _
                      And FactType.Name = pcenumCMMLRelations.CoreElementHasElementType.ToString _
                      And RoleData.Role.Name = pcenumCMML.Element.ToString _
                      And RoleData.Concept.Symbol = asModelElementName _
                      Select Page Distinct _
                      Order By Page.Name

        For Each lrPage In larPage
            getERDiagramPagesForModelElementName.Add(lrPage)
        Next

        Dim larConceptInstancePage = (From Page In arModel.Page
                                      From ConceptInstance In Page.ConceptInstance
                                      Where Page.Language = pcenumLanguage.EntityRelationshipDiagram
                                      Where ConceptInstance.ConceptType = pcenumConceptType.Value
                                      Where ConceptInstance.Symbol = asModelElementName
                                      Where ConceptInstance.RoleId = "e6ca6889-db16-470f-9106-d20387a59c3b"
                                      Select Page Distinct
                                      Order By Page.Name).ToList

        For Each lrPage In larConceptInstancePage
            getERDiagramPagesForModelElementName.AddUnique(lrPage)
        Next

    End Function

    Public Function getERDiagramPagesForNode(ByRef arNode As PGS.Node) As List(Of FBM.Page)

        Return Me.getERDiagramPagesForModelElementName(arNode.Model, arNode.Id)

    End Function

    Public Function getERDiagramPagesForEntityType(ByVal arEntityType As FBM.EntityType) As List(Of FBM.Page)

        getERDiagramPagesForEntityType = New List(Of FBM.Page)

        Dim lrModel As FBM.Model
        Dim lrPage As FBM.Page

        lrModel = arEntityType.Model

        Dim larPage = From Page In lrModel.Page _
                      From FactType In Page.FactTypeInstance _
                      From Fact In FactType.Fact _
                      From RoleData In Fact.Data _
                      Where Page.Language = pcenumLanguage.EntityRelationshipDiagram _
                      And FactType.Name = pcenumCMMLRelations.CoreElementHasElementType.ToString _
                      And RoleData.Role.Name = pcenumCMML.Element.ToString _
                      And RoleData.Concept.Symbol = arEntityType.Name _
                      Select Page Distinct _
                      Order By Page.Name

        For Each lrPage In larPage
            getERDiagramPagesForEntityType.Add(lrPage)
        Next

        Dim larConceptInstancePage = (From Page In lrModel.Page
                                      From ConceptInstance In Page.ConceptInstance
                                      Where Page.Language = pcenumLanguage.EntityRelationshipDiagram
                                      Where ConceptInstance.ConceptType = pcenumConceptType.Value
                                      Where ConceptInstance.Symbol = arEntityType.Id
                                      Where ConceptInstance.RoleId = "e6ca6889-db16-470f-9106-d20387a59c3b"
                                      Select Page Distinct
                                      Order By Page.Name).ToList

        For Each lrPage In larConceptInstancePage
            getERDiagramPagesForEntityType.AddUnique(lrPage)
        Next

    End Function

    Public Function getPGSDiagramPagesForModelElementName(ByVal arModel As FBM.Model, asModelElementName As String) As List(Of FBM.Page)

        getPGSDiagramPagesForModelElementName = New List(Of FBM.Page)

        Dim larPage = From Page In arModel.Page
                      From FactType In Page.FactTypeInstance
                      From Fact In FactType.Fact
                      From RoleData In Fact.Data
                      Where Page.Language = pcenumLanguage.PropertyGraphSchema _
                      And FactType.Name = pcenumCMMLRelations.CoreElementHasElementType.ToString _
                      And RoleData.Role.Name = pcenumCMML.Element.ToString _
                      And RoleData.Concept.Symbol = asModelElementName
                      Select Page Distinct
                      Order By Page.Name

        For Each lrPage In larPage
            getPGSDiagramPagesForModelElementName.Add(lrPage)
        Next

        Dim larConceptInstancePage = (From Page In arModel.Page
                                      From ConceptInstance In Page.ConceptInstance
                                      Where Page.Language = pcenumLanguage.PropertyGraphSchema
                                      Where ConceptInstance.ConceptType = pcenumConceptType.Value
                                      Where ConceptInstance.Symbol = asModelElementName
                                      Where ConceptInstance.RoleId = "e6ca6889-db16-470f-9106-d20387a59c3b"
                                      Select Page Distinct
                                      Order By Page.Name).ToList

        For Each lrPage In larConceptInstancePage
            getPGSDiagramPagesForModelElementName.AddUnique(lrPage)
        Next

    End Function

    Public Function getStateTransitionDiagramPagesForProcess(ByVal arProcess As UML.Process) As List(Of FBM.Page)

        getStateTransitionDiagramPagesForProcess = New List(Of FBM.Page)

        Dim lrModel As FBM.Model

        lrModel = arProcess.Model

        Dim larPage = From Page In lrModel.Page
                      From FactType In Page.FactTypeInstance
                      From Fact In FactType.FactType.Fact
                      From RoleData In Fact.Data
                      Where Page.Language = pcenumLanguage.StateTransitionDiagram _
                      And FactType.Name = pcenumCMMLRelations.CoreStateTransition.ToString _
                      And RoleData.Role.Name = pcenumCMML.Event.ToString _
                      And RoleData.Concept.Symbol = arProcess.Name
                      Select Page Distinct
                      Order By Page.Name

        getStateTransitionDiagramPagesForProcess = larPage.ToList

    End Function

    Public Function getStateTransitionDiagramPagesForValueType(ByRef arValueType As FBM.ValueType) As List(Of FBM.Page)

        getStateTransitionDiagramPagesForValueType = New List(Of FBM.Page)

        Dim lrModel As FBM.Model = arValueType.Model
        Dim lrValueType As FBM.ValueType = arValueType

        Dim larPage = arValueType.Model.Page.FindAll(Function(x) x.Language = pcenumLanguage.StateTransitionDiagram And x.Loaded)

        Dim lsSQLQuery As String = ""
        Dim lrORMRecordset As ORMQL.Recordset

        For Each lrPage In larPage

            lsSQLQuery = "SELECT *"
            lsSQLQuery &= " FROM " & pcenumCMMLRelations.CoreStateTransition.ToString
            lsSQLQuery &= " ON PAGE '" & lrPage.Name & "'"
            lsSQLQuery &= " WHERE ValueType = '" & arValueType.Id & "'"

            lrORMRecordset = arValueType.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

            If lrORMRecordset.Facts.Count > 0 Then
                getStateTransitionDiagramPagesForValueType.Add(lrPage)
            End If
        Next

        Dim larConceptInstancePage = (From Page In lrModel.Page
                                      From ConceptInstance In Page.ConceptInstance
                                      Where Page.Language = pcenumLanguage.StateTransitionDiagram
                                      Where ConceptInstance.ConceptType = pcenumConceptType.Value
                                      Where ConceptInstance.Symbol = lrValueType.Id
                                      Where ConceptInstance.RoleId = "9665f3e0-8083-4a49-ad61-c8e4330ab451"
                                      Select Page Distinct
                                      Order By Page.Name).ToList

        For Each lrPage In larConceptInstancePage
            getStateTransitionDiagramPagesForValueType.AddUnique(lrPage)
        Next

    End Function

    Public Function getPGSDiagramPagesForEntityType(ByVal arEntityType As FBM.EntityType) As List(Of FBM.Page)

        getPGSDiagramPagesForEntityType = New List(Of FBM.Page)

        Dim lrModel As FBM.Model
        Dim lrPage As FBM.Page

        lrModel = arEntityType.Model

        Dim larPage = From Page In lrModel.Page _
                      From FactType In Page.FactTypeInstance _
                      From Fact In FactType.Fact _
                      From RoleData In Fact.Data _
                      Where Page.Language = pcenumLanguage.PropertyGraphSchema _
                      And FactType.Name = pcenumCMMLRelations.CoreElementHasElementType.ToString _
                      And RoleData.Role.Name = pcenumCMML.Element.ToString _
                      And RoleData.Concept.Symbol = arEntityType.Name _
                      Select Page Distinct _
                      Order By Page.Name

        For Each lrPage In larPage
            getPGSDiagramPagesForEntityType.Add(lrPage)
        Next

        Dim larConceptInstancePage = (From Page In lrModel.Page
                                      From ConceptInstance In Page.ConceptInstance
                                      Where Page.Language = pcenumLanguage.PropertyGraphSchema
                                      Where ConceptInstance.ConceptType = pcenumConceptType.Value
                                      Where ConceptInstance.Symbol = arEntityType.Id
                                      Where ConceptInstance.RoleId = "e6ca6889-db16-470f-9106-d20387a59c3b"
                                      Select Page Distinct
                                      Order By Page.Name).ToList

        For Each lrPage In larConceptInstancePage
            getPGSDiagramPagesForEntityType.AddUnique(lrPage)
        Next

    End Function


    Public Function getORMDiagramPagesForValueType(ByVal arValueType As FBM.ValueType) As List(Of FBM.Page)

        getORMDiagramPagesForValueType = New List(Of FBM.Page)

        Dim lrModel As FBM.Model
        Dim lrPage As FBM.Page

        lrModel = arValueType.Model

        Dim larPage = (From Page In lrModel.Page
                       From ValueTypeInstance In Page.ValueTypeInstance
                       Where Page.Language = pcenumLanguage.ORMModel _
                     And ValueTypeInstance.ValueType.Id = arValueType.Id
                       Select Page Distinct
                       Order By Page.Name).ToList

        For Each lrPage In larPage
            getORMDiagramPagesForValueType.Add(lrPage)
        Next

        Dim larConceptInstancePage = (From Page In lrModel.Page
                                      From ConceptInstance In Page.ConceptInstance
                                      Where Page.Language = pcenumLanguage.ORMModel
                                      Where ConceptInstance.ConceptType = pcenumConceptType.ValueType
                                      Where ConceptInstance.Symbol = arValueType.Id
                                      Select Page Distinct
                                      Order By Page.Name).ToList

        For Each lrPage In larConceptInstancePage
            getORMDiagramPagesForValueType.AddUnique(lrPage)
        Next


    End Function

    Public Function getORMDiagramPagesForEntity(ByVal arEntity As ERD.Entity) As List(Of FBM.Page)

        getORMDiagramPagesForEntity = New List(Of FBM.Page)

        Dim lrModel As FBM.Model
        Dim lrPage As FBM.Page
        'Dim lr_use_case_page As New FBM.Page(ar_entity.Model, Nothing, "", pcenumLanguage.UseCaseDiagram)

        lrModel = arEntity.Model

        '-------------------------------------
        'Pages where Entity is an EntityType
        '-------------------------------------
        Dim larPage = From Page In lrModel.Page
                      From EntityTypeInstance In Page.EntityTypeInstance
                      Where Page.Language = pcenumLanguage.ORMModel _
                            And EntityTypeInstance.Id = arEntity.Data
                      Select Page Distinct
                      Order By Page.Name

        For Each lr_page In larPage
            getORMDiagramPagesForEntity.Add(lr_page)
        Next

        '-------------------------------------
        'Pages where Entity is an EntityType
        '-------------------------------------
        larPage = From Page In lrModel.Page
                  From FactTypeInstance In Page.FactTypeInstance
                  Where Page.Language = pcenumLanguage.ORMModel _
                  And FactTypeInstance.Id = arEntity.Data
                  Select Page Distinct
                  Order By Page.Name

        For Each lr_page In larPage
            getORMDiagramPagesForEntity.Add(lr_page)
        Next

        Dim larConceptInstancePage = (From Page In lrModel.Page
                                      From ConceptInstance In Page.ConceptInstance
                                      Where Page.Language = pcenumLanguage.ORMModel
                                      Where (ConceptInstance.ConceptType = pcenumConceptType.EntityType Or
                                             ConceptInstance.ConceptType = pcenumConceptType.FactType)
                                      Where ConceptInstance.Symbol = arEntity.Id
                                      Select Page Distinct
                                      Order By Page.Name).ToList

        For Each lrPage In larConceptInstancePage
            getORMDiagramPagesForEntity.AddUnique(lrPage)
        Next

    End Function

    Public Function getORMDiagramPagesForPGSNode(ByVal arNode As PGS.Node) As List(Of FBM.Page)

        getORMDiagramPagesForPGSNode = New List(Of FBM.Page)

        Dim lrModel As FBM.Model
        lrModel = arNode.Model

        '-------------------------------------
        'Pages where Entity is an EntityType
        '-------------------------------------
        Dim larPage = From Page In lrModel.Page
                      Where (Page.EntityTypeInstance.FindAll(Function(x) x.Id = arNode.Name).Count > 0 _
                      Or Page.FactTypeInstance.FindAll(Function(x) x.Id = arNode.Name).Count > 0) And Page.Language = pcenumLanguage.ORMModel
                      Select Page Distinct
                      Order By Page.Name

        Return larPage.ToList

        Dim larConceptInstancePage = (From Page In lrModel.Page
                                      From ConceptInstance In Page.ConceptInstance
                                      Where Page.Language = pcenumLanguage.ORMModel
                                      Where (ConceptInstance.ConceptType = pcenumConceptType.EntityType Or
                                             ConceptInstance.ConceptType = pcenumConceptType.FactType)
                                      Where ConceptInstance.Symbol = arNode.Name
                                      Select Page Distinct
                                      Order By Page.Name).ToList

        For Each lrPage In larConceptInstancePage
            getORMDiagramPagesForPGSNode.AddUnique(lrPage)
        Next

    End Function


    Public Function getORMDiagramPagesForFactType(ByVal arFactType As FBM.FactType) As List(Of FBM.Page)

        Dim lrModel As FBM.Model
        Dim lrPage As FBM.Page

        Try
            getORMDiagramPagesForFactType = New List(Of FBM.Page)

            lrModel = arFactType.Model

            Dim larPage = From Page In lrModel.Page
                          From FactTypeInstance In Page.FactTypeInstance
                          Where Page.Language = pcenumLanguage.ORMModel _
                         And FactTypeInstance.FactType.Id = arFactType.Id
                          Select Page Distinct
                          Order By Page.Name

            For Each lrPage In larPage
                getORMDiagramPagesForFactType.Add(lrPage)
            Next

            Dim larConceptInstancePage = (From Page In lrModel.Page
                                          From ConceptInstance In Page.ConceptInstance
                                          Where Page.Language = pcenumLanguage.ORMModel
                                          Where ConceptInstance.ConceptType = pcenumConceptType.FactType
                                          Where ConceptInstance.Symbol = arFactType.Id
                                          Select Page Distinct
                                          Order By Page.Name).ToList

            For Each lrPage In larConceptInstancePage
                getORMDiagramPagesForFactType.AddUnique(lrPage)
            Next


        Catch ex As Exception
            Dim lsMessage As String
            lsMessage = "Error: tCMML.GetOrmDiagramPagesForFactType"
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)

            Return Nothing
        End Try

    End Function

    Public Function GetORMDiagramPagesForRoleConstraint(ByVal arRoleConstraint As FBM.RoleConstraint) As List(Of FBM.Page)

        Try
            GetORMDiagramPagesForRoleConstraint = New List(Of FBM.Page)

            Dim lrModel As FBM.Model
            Dim lrPage As FBM.Page

            lrModel = arRoleConstraint.Model

            Dim larPage = From Page In lrModel.Page
                          From RoleConstraintInstance In Page.RoleConstraintInstance
                          Where Page.Language = pcenumLanguage.ORMModel _
                          And RoleConstraintInstance.RoleConstraint.Id = arRoleConstraint.Id
                          Select Page Distinct
                          Order By Page.Name

            For Each lrPage In larPage
                GetORMDiagramPagesForRoleConstraint.Add(lrPage)
            Next

            Dim larConceptInstancePage = (From Page In lrModel.Page
                                          From ConceptInstance In Page.ConceptInstance
                                          Where Page.Language = pcenumLanguage.ORMModel
                                          Where ConceptInstance.ConceptType = pcenumConceptType.RoleConstraint
                                          Where ConceptInstance.Symbol = arRoleConstraint.Id
                                          Select Page Distinct
                                          Order By Page.Name).ToList

            For Each lrPage In larConceptInstancePage
                GetORMDiagramPagesForRoleConstraint.AddUnique(lrPage)
            Next

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)

            Return New List(Of FBM.Page)
        End Try


    End Function

    Public Function getORMDiagramPagesForState(ByRef arState As STD.State) As List(Of FBM.Page)

        Try
            getORMDiagramPagesForState = New List(Of FBM.Page)

            Dim lrModel As FBM.Model
            Dim lsStateName = arState.StateName
            Dim lsValueTypeId = arState.ValueType.Id

            lrModel = arState.Model

            Dim larPage = From Page In lrModel.Page
                          From ValueTypeInstance In Page.ValueTypeInstance
                          From ValueConstraint In ValueTypeInstance.ValueType.ValueConstraint
                          Where ValueTypeInstance.Id = lsValueTypeId
                          Where Page.Language = pcenumLanguage.ORMModel _
                          And ValueConstraint = lsStateName
                          Select Page Distinct
                          Order By Page.Name

            For Each lrPage In larPage
                getORMDiagramPagesForState.Add(lrPage)
            Next


            Dim larConceptInstancePage = (From Page In lrModel.Page
                                          From ConceptInstance In Page.ConceptInstance
                                          Where Page.Language = pcenumLanguage.ORMModel
                                          Where ConceptInstance.ConceptType = pcenumConceptType.ValueType
                                          Where ConceptInstance.Symbol = lsValueTypeId
                                          Select Page Distinct
                                          Order By Page.Name).ToList

            For Each lrPage In larConceptInstancePage
                getORMDiagramPagesForState.AddUnique(lrPage)
            Next

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)

            Return New List(Of FBM.Page)
        End Try

    End Function

    Public Function getUseCaseDiagramPagesForActor(ByVal arActor As UML.Actor) As List(Of FBM.Page)


        getUseCaseDiagramPagesForActor = New List(Of FBM.Page)

        Try

            Dim lrModel As FBM.Model

            lrModel = arActor.Model

            Dim larPage = From Page In lrModel.Page
                          From FactTypeInstance In Page.FactTypeInstance
                          From FactInstance In FactTypeInstance.Fact
                          From FactDataInstance In FactInstance.Data
                          Where Page.Language = pcenumLanguage.UMLUseCaseDiagram
                          Where FactTypeInstance.Name = pcenumCMMLRelations.CoreElementHasElementType.ToString
                          Where FactDataInstance.Role IsNot Nothing
                          Where FactDataInstance.Role.Name = pcenumCMML.Element.ToString
                          Where FactDataInstance.Data = arActor.Name
                          Select Page Distinct
                          Order By Page.Name

            getUseCaseDiagramPagesForActor = larPage.ToList

            Dim larConceptInstancePage = (From Page In lrModel.Page
                                          From ConceptInstance In Page.ConceptInstance
                                          Where Page.Language = pcenumLanguage.UMLUseCaseDiagram
                                          Where ConceptInstance.ConceptType = pcenumConceptType.Value
                                          Where ConceptInstance.Symbol = arActor.Name
                                          Where ConceptInstance.RoleId = "e6ca6889-db16-470f-9106-d20387a59c3b"
                                          Select Page Distinct
                                          Order By Page.Name).ToList

            For Each lrPage In larConceptInstancePage
                getUseCaseDiagramPagesForActor.AddUnique(lrPage)
            Next

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try

    End Function


End Class
