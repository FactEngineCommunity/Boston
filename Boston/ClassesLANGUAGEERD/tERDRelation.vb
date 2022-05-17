Imports System.Reflection

Namespace ERD


    <Serializable()>
    Public Class Relation
        Implements IEquatable(Of ERD.Relation)

        Public ConceptType As pcenumConceptType = pcenumConceptType.Relation

        Public Model As FBM.Model
        Public Page As FBM.Page

        Public Id As String = ""

        Public OriginEntity As FBM.FactDataInstance
        Public OriginAttribute As New List(Of ERD.Attribute)
        Public OriginMandatory As Boolean = False
        Public OriginMultiplicity As pcenumCMMLMultiplicity = pcenumCMMLMultiplicity.One
        Public OriginContributesToPrimaryKey As Boolean = False
        Public OriginPredicate As String = ""

        Public DestinationEntity As FBM.FactDataInstance
        Public DestinationAttribute As List(Of ERD.Attribute)
        Public DestinationMandatory As Boolean = False
        Public DestinationMultiplicity As pcenumCMMLMultiplicity = pcenumCMMLMultiplicity.One
        Public DestinationPredicate As String = ""

        Public WithEvents RelationFactType As FBM.FactType

        Public IsPGSRelationNode As Boolean = False
        Public ActualPGSNode As PGS.Node

        Public Link As Object 'ERD.Link

        Public WithEvents RDSRelation As RDS.Relation

        Public WithEvents RDSTable As RDS.Table 'If the Relation is a PGSRelation then has the ability to switch to a Node, and visa-versa, 
        ' if And when there are changes to Table as a result of changes to the Table's responsible FactType. 
        ' NB The Relation/Table will only have a responsible FactType is the Relation (as a PGSRelation) is a result of an ObjectifiedFactType.

        Public Sub New()
            '---------------------------
            'Parameterless Constructor
            '---------------------------
        End Sub

        ''' <summary>
        ''' Object constructor.
        ''' </summary>
        ''' <param name="arModel"></param>
        ''' <param name="arPage"></param>
        ''' <param name="asRelationId"></param>
        ''' <param name="arOriginEntity"></param>
        ''' <param name="aiOriginMultiplicity"></param>
        ''' <param name="abOriginMandatory"></param>
        ''' <param name="abOriginContributesToPrimaryKey"></param>
        ''' <param name="arDestinationEntity"></param>
        ''' <param name="aiDestinationMultiplicity"></param>
        ''' <param name="abDestinationMandatory"></param>
        ''' <param name="abCorrespondingTable">If the Relation is a PGSRelation, then has a corresponding Table.</param>
        Public Sub New(ByRef arModel As FBM.Model,
                       ByRef arPage As FBM.Page,
                       ByVal asRelationId As String,
                       ByRef arOriginEntity As FBM.FactDataInstance,
                       ByVal aiOriginMultiplicity As pcenumCMMLMultiplicity,
                       ByVal abOriginMandatory As Boolean,
                       ByVal abOriginContributesToPrimaryKey As Boolean,
                       ByRef arDestinationEntity As FBM.FactDataInstance,
                       ByVal aiDestinationMultiplicity As pcenumCMMLMultiplicity,
                       ByVal abDestinationMandatory As Boolean,
                       Optional ByRef abCorrespondingTable As RDS.Table = Nothing)

            Me.Model = arModel
            Me.Page = arPage
            Me.Id = asRelationId

            Me.OriginEntity = arOriginEntity
            Me.OriginMultiplicity = aiOriginMultiplicity
            Me.OriginMandatory = abOriginMandatory
            Me.OriginContributesToPrimaryKey = abOriginContributesToPrimaryKey

            Me.DestinationEntity = arDestinationEntity
            Me.DestinationMultiplicity = aiDestinationMultiplicity
            Me.DestinationMandatory = abDestinationMandatory

        End Sub

        Public Function ClonePageObject() As FBM.PageObject

            Dim lrPageObject As New FBM.PageObject

            Select Case Me.RelationFactType.IsLinkFactType
                Case = True
                    lrPageObject.Name = Me.RelationFactType.RoleGroup(0).JoinedORMObject.Id
                Case Else
                    lrPageObject.Name = Me.RelationFactType.Id
            End Select

            lrPageObject.Shape = New MindFusion.Diagramming.ShapeNode
            Select Case Me.OriginEntity.GetType
                Case = GetType(PGS.Node)
                    lrPageObject.X = CType(Me.OriginEntity, PGS.Node).X
                    lrPageObject.Y = CType(Me.OriginEntity, PGS.Node).Y
                Case Else
                    lrPageObject.X = Me.OriginEntity.X
                    lrPageObject.Y = Me.OriginEntity.Y
            End Select

            Return lrPageObject

        End Function

        Public Shadows Function Equals(other As Relation) As Boolean Implements IEquatable(Of Relation).Equals
            Return Me.Id = other.Id
        End Function

        Private Sub RelationFactType_FactTypeReadingAdded(ByRef arFactTypeReading As FBM.FactTypeReading) Handles RelationFactType.FactTypeReadingAdded

        End Sub


        Private Sub RelationFactType_FactTypeReadingModified(ByRef arFactTypeReading As FBM.FactTypeReading) Handles RelationFactType.FactTypeReadingModified

            Try

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
            End Try


        End Sub

        Private Sub RDSRelation_DestinationMandatoryChanged(abDestinationIsMandatory As Boolean) Handles RDSRelation.DestinationMandatoryChanged

            Me.DestinationMandatory = abDestinationIsMandatory

            Call Me.Page.Diagram.Invalidate()

        End Sub

        Private Sub RDSRelation_DestinationMultiplicityChanged(aiDestinationMultiplicity As pcenumCMMLMultiplicity) Handles RDSRelation.DestinationMultiplicityChanged

            Me.DestinationMultiplicity = aiDestinationMultiplicity
            Call Me.Page.Diagram.Invalidate()

        End Sub

        Private Sub RDSRelation_DestinationPredicateChanged(asPredicate As String) Handles RDSRelation.DestinationPredicateChanged

            Try
                Me.DestinationPredicate = asPredicate

                If Me.Page IsNot Nothing Then
                    If Me.Page.Diagram IsNot Nothing Then
                        Call Me.Page.Diagram.Invalidate()
                    End If
                End If

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Private Sub RDSRelation_OriginMandatoryChanged(abOriginIsMandatory As Boolean) Handles RDSRelation.OriginMandatoryChanged

            Me.OriginMandatory = abOriginIsMandatory
            Call Me.Page.Diagram.Invalidate()

        End Sub

        Private Sub RDSRelation_OriginMultiplicityChanged(aiOriginMultiplicity As pcenumCMMLMultiplicity) Handles RDSRelation.OriginMultiplicityChanged

            Me.OriginMultiplicity = aiOriginMultiplicity
            Call Me.Page.Diagram.Invalidate()

        End Sub

        Private Sub RDSRelation_OriginPredicateChanged(asPredicate As String) Handles RDSRelation.OriginPredicateChanged

            Me.OriginPredicate = asPredicate

        End Sub

        Private Sub RDSRelation_RemovedFromModel() Handles RDSRelation.RemovedFromModel

            If Me.Link Is Nothing Then Exit Sub
            Me.Page.Diagram.Links.Remove(Me.Link.Link)
            Me.Page.ERDiagram.Relation.Remove(Me)

            Dim lrDiagramingLink As MindFusion.Diagramming.DiagramLink = Me.Link.Link
            lrDiagramingLink.Dispose()

            Me.Page.Diagram.Invalidate()

        End Sub


        Private Sub RDSRelation_ResponsibleFactTypeChanged(ByRef arNewResponsibleFactType As FBM.FactType) Handles RDSRelation.ResponsibleFactTypeChanged

            Me.RelationFactType = arNewResponsibleFactType

        End Sub


        Public Sub removeFromPage()

            Try
                Me.Page.Diagram.Links.Remove(Me.Link.Link)

                Dim lrDiagramingLink As MindFusion.Diagramming.DiagramLink = Me.Link.Link

                If lrDiagramingLink IsNot Nothing Then
                    lrDiagramingLink.Dispose()
                End If

                Me.Page.ERDiagram.Relation.Remove(Me)

                Me.Page.Diagram.Invalidate()


            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
            End Try
        End Sub

        Private Sub RDSRelation_OriginTableChanged(ByRef arTable As RDS.Table) Handles RDSRelation.OriginTableChanged

            Try
                If Me.Page.Diagram IsNot Nothing Then

                    If Me.Link IsNot Nothing Then
                        Dim lrTable As RDS.Table = arTable
                        Dim lrERDLink As ERD.Link = Me.Link
                        Dim lrEntity As ERD.Entity = Me.Page.ERDiagram.Entity.Find(Function(x) x.Name = lrTable.Name)

                        If lrEntity IsNot Nothing Then
                            lrERDLink.OriginModelElement = lrEntity
                            lrERDLink.Link.Origin = lrERDLink.OriginModelElement.TableShape
                        Else
                            Me.Page.Diagram.Links.Remove(lrERDLink.Link)
                        End If

                    End If
                End If


            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Private Sub RDSRelation_DestinationTableChanged(ByRef arTable As RDS.Table) Handles RDSRelation.DestinationTableChanged

            Try
                If Me.Page.Diagram IsNot Nothing Then

                    If Me.Link IsNot Nothing Then
                        Dim lrTable As RDS.Table = arTable
                        Dim lrERDLink As ERD.Link = Me.Link
                        Dim lrEntity As ERD.Entity = Me.Page.ERDiagram.Entity.Find(Function(x) x.Name = lrTable.Name)

                        If lrEntity IsNot Nothing Then
                            lrERDLink.DestinationModelElement = lrEntity
                            lrERDLink.Link.Destination = lrERDLink.DestinationModelElement.TableShape
                        Else
                            Me.Page.Diagram.Links.Remove(lrERDLink.Link)
                        End If

                    End If
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