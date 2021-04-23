Imports System.Reflection

Public Class tCMML

    Public Core As FBM.Model

    Public Function getORMDiagramPagesForEntityType(ByVal arEntityType As FBM.EntityType) As List(Of FBM.Page)

        GetORMDiagramPagesForEntityType = New List(Of FBM.Page)

        Dim lrModel As FBM.Model
        Dim lrPage As FBM.Page

        lrModel = arEntityType.Model

        Dim larPage = From Page In lrModel.Page _
                     From EntityTypeInstance In Page.EntityTypeInstance _
                     Where Page.Language = pcenumLanguage.ORMModel _
                     And EntityTypeInstance.EntityType.Id = arEntityType.Id _
                     Select Page Distinct _
                     Order By Page.Name

        For Each lrPage In larPage
            GetORMDiagramPagesForEntityType.Add(lrPage)
        Next

    End Function

    Public Function getORMDiagramPagesForModelElementName(ByRef arModel As FBM.Model, ByVal asModelElementName As String) As List(Of FBM.Page)

        getORMDiagramPagesForModelElementName = New List(Of FBM.Page)

        Dim larPage = From Page In arModel.Page _
                     From EntityTypeInstance In Page.EntityTypeInstance _
                     Where Page.Language = pcenumLanguage.ORMModel _
                     And EntityTypeInstance.EntityType.Id = asModelElementName _
                     Select Page Distinct _
                     Order By Page.Name

        For Each lrPage In larPage
            getORMDiagramPagesForModelElementName.Add(lrPage)
        Next

        larPage = From Page In arModel.Page _
                  From FactTypeInstance In Page.FactTypeInstance _
                  Where Page.Language = pcenumLanguage.ORMModel _
                  And FactTypeInstance.FactType.Id = asModelElementName _
                  Select Page Distinct _
                  Order By Page.Name

        For Each lrPage In larPage
            getORMDiagramPagesForModelElementName.Add(lrPage)
        Next

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

    End Function

    Public Function getSTDDiagramPagesForValueType(ByRef arValueType As FBM.ValueType) As List(Of FBM.Page)

        getSTDDiagramPagesForValueType = New List(Of FBM.Page)

        Dim lrModel As FBM.Model = arValueType.Model
        Dim lrValueType As FBM.ValueType = arValueType

        Dim larPage = arValueType.Model.Page.FindAll(Function(x) x.Language = pcenumLanguage.StateTransitionDiagram)

        Dim lsSQLQuery As String = ""
        Dim lrORMRecordset As ORMQL.Recordset

        For Each lrPage In larPage

            lsSQLQuery = "SELECT *"
            lsSQLQuery &= " FROM " & pcenumCMMLRelations.CoreStateTransition.ToString
            lsSQLQuery &= " ON PAGE '" & lrPage.Name & "'"
            lsSQLQuery &= " WHERE ValueType = '" & arValueType.Id & "'"

            lrORMRecordset = arValueType.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

            If lrORMRecordset.Facts.Count > 0 Then
                getSTDDiagramPagesForValueType.Add(lrPage)
            End If
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

    End Function


    Public Function get_orm_diagram_pages_for_value_type(ByVal ar_entity_type As FBM.ValueType) As List(Of FBM.Page)

        get_orm_diagram_pages_for_value_type = New List(Of FBM.Page)

        Dim lr_model As FBM.Model
        Dim lr_page As FBM.Page

        lr_model = ar_entity_type.Model

        Dim lrPage = From Page In lr_model.Page _
                     From ValueTypeInstance In Page.ValueTypeInstance _
                     Where Page.Language = pcenumLanguage.ORMModel _
                     And ValueTypeInstance.ValueType.Id = ar_entity_type.Id _
                     Select Page Distinct _
                     Order By Page.Name

        For Each lr_page In lrPage
            get_orm_diagram_pages_for_value_type.Add(lr_page)
        Next

    End Function

    Public Function getORMDiagramPagesForEntity(ByVal ar_entity As ERD.Entity) As List(Of FBM.Page)

        getORMDiagramPagesForEntity = New List(Of FBM.Page)

        Dim lr_model As FBM.Model
        Dim lr_page As FBM.Page
        'Dim lr_use_case_page As New FBM.Page(ar_entity.Model, Nothing, "", pcenumLanguage.UseCaseDiagram)

        lr_model = ar_entity.Model

        '-------------------------------------
        'Pages where Entity is an EntityType
        '-------------------------------------
        Dim lrPage = From Page In lr_model.Page _
                     From EntityTypeInstance In Page.EntityTypeInstance _
                     Where Page.Language = pcenumLanguage.ORMModel _
                     And EntityTypeInstance.Id = ar_entity.Data _
                     Select Page Distinct _
                     Order By Page.Name

        For Each lr_page In lrPage
            getORMDiagramPagesForEntity.Add(lr_page)
        Next

        '-------------------------------------
        'Pages where Entity is an EntityType
        '-------------------------------------
        lrPage = From Page In lr_model.Page _
                 From FactTypeInstance In Page.FactTypeInstance _
                 Where Page.Language = pcenumLanguage.ORMModel _
                 And FactTypeInstance.Id = ar_entity.Data _
                 Select Page Distinct _
                 Order By Page.Name

        For Each lr_page In lrPage
            getORMDiagramPagesForEntity.Add(lr_page)
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

    End Function


    Public Function get_orm_diagram_pages_for_FactType(ByVal arFactTypeype As FBM.FactType) As List(Of FBM.Page)

        Dim lrModel As FBM.Model
        Dim lrPage As FBM.Page

        Try
            get_orm_diagram_pages_for_FactType = New List(Of FBM.Page)

            lrModel = arFactTypeype.Model

            Dim larPage = From Page In lrModel.Page _
                         From FactTypeInstance In Page.FactTypeInstance _
                         Where Page.Language = pcenumLanguage.ORMModel _
                         And FactTypeInstance.FactType.Id = arFactTypeype.Id _
                         Select Page Distinct _
                         Order By Page.Name

            For Each lrPage In larPage
                get_orm_diagram_pages_for_FactType.Add(lrPage)
            Next

        Catch ex As Exception
            Dim lsMessage As String
            lsMessage = "Error: tCMML.GetOrmDiagramPagesForFactType"
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)

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

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)

            Return New List(Of FBM.Page)
        End Try


    End Function

    Public Function getORMDiagramPagesForState(ByRef arState As STD.State) As List(Of FBM.Page)

        Try
            getORMDiagramPagesForState = New List(Of FBM.Page)

            Dim lr_model As FBM.Model
            Dim lsStateName = arState.StateName
            Dim lsValueTypeId = arState.ValueType.Id

            lr_model = arState.Model

            Dim larPage = From Page In lr_model.Page
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

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)

            Return New List(Of FBM.Page)
        End Try

    End Function

End Class
