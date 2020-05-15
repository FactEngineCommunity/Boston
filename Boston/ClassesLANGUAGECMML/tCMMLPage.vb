Imports System.Reflection
Imports MindFusion.Diagramming

Namespace CMML
    Public Class Page
        Inherits FBM.Page

        Sub DropActorAtPoint(ByRef arActor As CMML.tActor, ByVal aoPointF As PointF)

            Dim lsSQLQuery As String = ""
            Dim lrFactInstance As FBM.FactInstance

            Try
                lsSQLQuery = "INSERT INTO " & pcenumCMMLRelations.CoreElementHasElementType.ToString
                lsSQLQuery &= " (" & pcenumCMML.Element.ToString
                lsSQLQuery &= " , " & pcenumCMML.ElementType.ToString & ")"
                lsSQLQuery &= " ON PAGE '" & Me.Name & "'"
                lsSQLQuery &= " VALUES ("
                lsSQLQuery &= "'" & arActor.Name & "'"
                lsSQLQuery &= ",'" & pcenumCMML.Actor.ToString & "'"
                lsSQLQuery &= ")"

                lrFactInstance = Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                arActor = lrFactInstance.GetFactDataInstanceByRoleName(pcenumCMML.Element.ToString).CloneActor(Me)

                arActor.X = aoPointF.X
                arActor.Y = aoPointF.Y

                Call arActor.DisplayAndAssociate()

                Me.MakeDirty()

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Sub DropDataStoreAtPoint(ByRef arDataStoreInstance As DFD.DataStore, ByVal aoPointF As PointF, Optional ByRef aoContainerNode As ContainerNode = Nothing)

            Dim lsSQLQuery As String = ""
            Dim lrFactInstance As FBM.FactInstance

            Try

                lsSQLQuery = "INSERT INTO " & pcenumCMMLRelations.CoreElementHasElementType.ToString
                lsSQLQuery &= " (" & pcenumCMML.Element.ToString
                lsSQLQuery &= " , " & pcenumCMML.ElementType.ToString & ")"
                lsSQLQuery &= " ON PAGE '" & Me.Name & "'"
                lsSQLQuery &= " VALUES ("
                lsSQLQuery &= "'" & arDataStoreInstance.Name & "'"
                lsSQLQuery &= ",'" & pcenumCMML.DataStore.ToString & "'"
                lsSQLQuery &= ")"

                lrFactInstance = Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                arDataStoreInstance = lrFactInstance.GetFactDataInstanceByRoleName(pcenumCMML.Element.ToString).CloneDataStore(Me)

                arDataStoreInstance.X = aoPointF.X
                arDataStoreInstance.Y = aoPointF.Y

                Call arDataStoreInstance.DisplayAndAssociate(aoContainerNode)

                Me.MakeDirty()

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub 'DropDataStoreAtPoint


        Public Shadows Sub DropExistingEntityAtPoint(ByRef arEntityInstance As ERD.Entity, ByVal aoPointF As PointF)

            Try
                Dim lsSQLQuery As String = ""
                Dim lrRecordset As ORMQL.Recordset
                Dim lrFactInstance As FBM.FactInstance
                Dim lrRDSTable As RDS.Table

                lrRDSTable = arEntityInstance.RDSTable


                '--------------------
                'Load the Entity.
                '==================================================================================================================
                lsSQLQuery = "SELECT *"
                lsSQLQuery &= " FROM " & pcenumCMMLRelations.CoreElementHasElementType.ToString
                lsSQLQuery &= " WHERE Element = '" & arEntityInstance.Name & "'"

                lrRecordset = Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                lsSQLQuery = "ADD FACT '" & lrRecordset.CurrentFact.Id & "'"
                lsSQLQuery &= " TO " & pcenumCMMLRelations.CoreElementHasElementType.ToString
                lsSQLQuery &= " ON PAGE '" & Me.Name & "'"

                lrFactInstance = Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                arEntityInstance = lrFactInstance.GetFactDataInstanceByRoleName(pcenumCMML.Element.ToString).CloneEntity(Me)

                arEntityInstance.X = aoPointF.X
                arEntityInstance.Y = aoPointF.Y
                '==================================================================================================================

                Dim lrEntity As New ERD.Entity
                Dim lrFactDataInstance As FBM.FactDataInstance
                lrEntity = arEntityInstance.CloneEntity(Me)
                lrEntity.RDSTable = lrRDSTable

                '----------------------------------------------
                'Create a TableNode on the Page for the Entity
                '----------------------------------------------
                lrEntity.DisplayAndAssociate()

                Me.ERDiagram.Entity.Add(lrEntity)

                '=====================
                'Load the Attributes
                '=====================
                lsSQLQuery = "SELECT *"
                lsSQLQuery &= " FROM CoreERDAttribute"
                lsSQLQuery &= " WHERE ModelObject = '" & lrEntity.Name & "'"

                lrRecordset = Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                Dim lrERAttribute As ERD.Attribute
                Dim lrRecordset1 As ORMQL.Recordset

                While Not lrRecordset.EOF

                    Dim lsMandatory As String = ""

                    lrERAttribute = New ERD.Attribute

                    lsSQLQuery = "ADD FACT '" & lrRecordset.CurrentFact.Id & "'"
                    lsSQLQuery &= " TO " & pcenumCMMLRelations.CoreERDAttribute.ToString
                    lsSQLQuery &= " ON PAGE '" & Me.Name & "'"

                    lrFactInstance = Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery) 'lrRecordset("Attribute")

                    lrFactDataInstance = lrFactInstance.GetFactDataInstanceByRoleName("Attribute")
                    lrERAttribute = lrFactDataInstance.CloneAttribute(Me)

                    '-------------------------------
                    'Get the Name of the Attribute
                    '-------------------------------
                    lsSQLQuery = "SELECT *"
                    lsSQLQuery &= " FROM " & pcenumCMMLRelations.CorePropertyHasPropertyName.ToString
                    lsSQLQuery &= " WHERE Property = '" & lrRecordset("Attribute").Data & "'"

                    lrRecordset1 = Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                    lrERAttribute.AttributeName = lrRecordset1("PropertyName").Data

                    lsSQLQuery = "ADD FACT '" & lrRecordset1.CurrentFact.Id & "'"
                    lsSQLQuery &= " TO " & pcenumCMMLRelations.CorePropertyHasPropertyName.ToString
                    lsSQLQuery &= " ON PAGE '" & Me.Name & "'"

                    Call Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                    '-------------------------------------------------
                    'Check to see whether the Attribute is Mandatory
                    '-------------------------------------------------
                    lsSQLQuery = "SELECT *"
                    lsSQLQuery &= " FROM CoreIsMandatory"
                    lsSQLQuery &= " WHERE IsMandatory = '" & lrRecordset("Attribute").Data & "'"

                    lrRecordset1 = Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                    If lrRecordset1.Facts.Count = 1 Then
                        lrERAttribute.Mandatory = True
                        lsMandatory = "*"

                        lsSQLQuery = "ADD FACT '" & lrRecordset1.CurrentFact.Id & "'"
                        lsSQLQuery &= " TO " & pcenumCMMLRelations.CoreIsMandatory.ToString
                        lsSQLQuery &= " ON PAGE '" & Me.Name & "'"

                        Call Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)
                    End If

                    lsSQLQuery = "SELECT * FROM " & pcenumCMMLRelations.CorePropertyHasOrdinalPosition.ToString
                    lsSQLQuery &= " WHERE Attribute = '" & lrRecordset("Property").Data & "'" '& lrERAttribute.FactDataInstance.Fact.Id & "'"

                    lrRecordset1 = Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)
                    lrERAttribute.OrdinalPosition = CInt(lrRecordset1("Position").Data)

                    lsSQLQuery = "ADD FACT '" & lrRecordset1.CurrentFact.Id & "'"
                    lsSQLQuery &= " TO " & pcenumCMMLRelations.CorePropertyHasOrdinalPosition.ToString
                    lsSQLQuery &= " ON PAGE '" & Me.Name & "'"

                    Call Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                    '=============
                    'Role
                    lsSQLQuery = "SELECT * FROM " & pcenumCMMLRelations.CorePropertyIsForRole.ToString
                    lsSQLQuery &= " WHERE Property = '" & lrRecordset("Attribute").Data & "'" '& lrERAttribute.FactDataInstance.Fact.Id & "'"

                    lrRecordset1 = Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                    lsSQLQuery = "ADD FACT '" & lrRecordset1.CurrentFact.Id & "'"
                    lsSQLQuery &= " TO " & pcenumCMMLRelations.CorePropertyIsForRole.ToString
                    lsSQLQuery &= " ON PAGE '" & Me.Name & "'"

                    Call Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)
                    '=============

                    '=============
                    'FactType
                    lsSQLQuery = "SELECT * FROM " & pcenumCMMLRelations.CorePropertyIsForFactType.ToString
                    lsSQLQuery &= " WHERE Property = '" & lrRecordset("Attribute").Data & "'" '& lrERAttribute.FactDataInstance.Fact.Id & "'"

                    lrRecordset1 = Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                    lsSQLQuery = "ADD FACT '" & lrRecordset1.CurrentFact.Id & "'"
                    lsSQLQuery &= " TO " & pcenumCMMLRelations.CorePropertyIsForFactType.ToString
                    lsSQLQuery &= " ON PAGE '" & Me.Name & "'"

                    Call Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)
                    '=============

                    '---------------------------------------------------
                    'Add the Attribute to the ER Entity
                    '---------------------------------------------------
                    lrERAttribute.Entity = lrEntity

                    '--------------------------------------------------------
                    'Check to see whether the Entity has a PrimaryKey
                    '--------------------------------------------------------
                    lsSQLQuery = "SELECT *"
                    lsSQLQuery &= " FROM " & pcenumCMMLRelations.CoreIndexIsForEntity.ToString
                    lsSQLQuery &= " WHERE Entity = '" & lrEntity.Id & "'"

                    lrRecordset1 = Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                    If lrRecordset1.Facts.Count > 0 Then
                        '-------------------------
                        'Entity has a PrimaryKey
                        '-------------------------
                        lsSQLQuery = "ADD FACT '" & lrRecordset1.CurrentFact.Id & "'"
                        lsSQLQuery &= " TO " & pcenumCMMLRelations.CoreIndexIsForEntity.ToString
                        lsSQLQuery &= " ON PAGE '" & Me.Name & "'"

                        Call Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                        '=============================================================================================
                        Dim lsIndexName As String = ""
                        lsIndexName = Viev.Strings.RemoveWhiteSpace(lrEntity.Id & "PK")

                        '-------------------------------------
                        'Add the Attribute to the PrimaryKey
                        '-------------------------------------
                        lsSQLQuery = "SELECT * FROM "
                        lsSQLQuery &= pcenumCMMLRelations.CoreIndexMakesUseOfProperty.ToString
                        lsSQLQuery &= " WHERE Index = '" & lsIndexName & "'"
                        lsSQLQuery &= " AND Property = '" & lrERAttribute.Name & "'"

                        lrRecordset1 = Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                        If lrRecordset1.Facts.Count > 0 Then
                            lrERAttribute.PartOfPrimaryKey = True
                            lrEntity.PrimaryKey.Add(lrERAttribute)

                            lsSQLQuery = "ADD FACT '" & lrRecordset1.CurrentFact.Id & "'"
                            lsSQLQuery &= " TO " & pcenumCMMLRelations.CoreIndexMakesUseOfProperty.ToString
                            lsSQLQuery &= " ON PAGE '" & Me.Name & "'"

                            Call Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)
                        End If
                        '=============================================================================================
                    End If

                    lrEntity.Attribute.Add(lrERAttribute)
                    Me.ERDiagram.Attribute.Add(lrERAttribute)

                    lrRecordset.MoveNext()
                End While

                '-------------------------------------------------------------------
                'Paint the sorted Attributes (By Ordinal Position) for each Entity
                '-------------------------------------------------------------------
                lrEntity.Attribute.Sort(AddressOf ERD.Attribute.ComparerOrdinalPosition)

                For Each lrERAttribute In lrEntity.Attribute
                    lrEntity.TableShape.RowCount += 1
                    lrERAttribute.Cell = lrEntity.TableShape.Item(0, lrEntity.TableShape.RowCount - 1)
                    lrEntity.TableShape.Item(0, lrEntity.TableShape.RowCount - 1).Tag = lrERAttribute
                    Call lrERAttribute.RefreshShape()
                Next

                lrEntity.TableShape.ResizeToFitText(False)
                arEntityInstance = lrEntity 'NB IMPORTANT: Leave this at this point in the code. Otherwise (somehow) the EntityType ends up with no TableShape. See also frmERDDiagram.DiagramView.DragDrop.

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Sub DropProcessAtPoint(ByRef arProcessInstance As CMML.Process, ByVal aoPointF As PointF, Optional ByRef aoContainerNode As ContainerNode = Nothing)

            Dim lsSQLQuery As String = ""
            Dim lrFactInstance As FBM.FactInstance

            Try

                lsSQLQuery = "INSERT INTO " & pcenumCMMLRelations.CoreElementHasElementType.ToString
                lsSQLQuery &= " (" & pcenumCMML.Element.ToString
                lsSQLQuery &= " , " & pcenumCMML.ElementType.ToString & ")"
                lsSQLQuery &= " ON PAGE '" & Me.Name & "'"
                lsSQLQuery &= " VALUES ("
                lsSQLQuery &= "'" & arProcessInstance.Name & "'"
                lsSQLQuery &= ",'" & pcenumCMML.Process.ToString & "'"
                lsSQLQuery &= ")"

                lrFactInstance = Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                arProcessInstance = lrFactInstance.GetFactDataInstanceByRoleName(pcenumCMML.Element.ToString).CloneProcess(Me)

                arProcessInstance.X = aoPointF.X
                arProcessInstance.Y = aoPointF.Y

                Call arProcessInstance.DisplayAndAssociate(aoContainerNode)


            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Public Function GetModelDataStores() As List(Of String)

            Dim lsSQLQuey As String = ""
            Dim lrRecordset As ORMQL.Recordset

            GetModelDataStores = New List(Of String)

            lsSQLQuey = "SELECT *"
            lsSQLQuey &= " FROM " & pcenumCMMLRelations.CoreElementHasElementType.ToString
            lsSQLQuey &= " WHERE ElementType = 'DataStore'"

            lrRecordset = Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuey)

            While Not lrRecordset.EOF
                GetModelDataStores.Add(lrRecordset("Element").Data)
                lrRecordset.MoveNext()
            End While

        End Function

        Public Function ExistsActor(ByVal asProcessName As String) As Boolean

            Dim lsSQLQuery As String = ""
            Dim lrRecordset As ORMQL.Recordset

            ExistsActor = False

            lsSQLQuery = "SELECT COUNT(*)"
            lsSQLQuery &= " FROM " & pcenumCMMLRelations.CoreElementHasElementType.ToString
            lsSQLQuery &= " WHERE Element = '" & asProcessName & "'"
            lsSQLQuery &= "   AND ElementType = 'Actor'"

            lrRecordset = Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

            If CInt(lrRecordset("Count").Data) > 0 Then
                ExistsActor = True
            End If

        End Function

        Public Function ExistsProcess(ByVal asProcessName As String) As Boolean

            Dim lsSQLQuery As String = ""
            Dim lrRecordset As ORMQL.Recordset

            ExistsProcess = False

            lsSQLQuery = "SELECT COUNT(*)"
            lsSQLQuery &= " FROM " & pcenumCMMLRelations.CoreElementHasElementType.ToString
            lsSQLQuery &= " WHERE Element = '" & asProcessName & "'"
            lsSQLQuery &= "   AND ElementType = 'Process'"

            lrRecordset = Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

            If CInt(lrRecordset("Count").Data) > 0 Then
                ExistsProcess = True
            End If

        End Function


    End Class

End Namespace
