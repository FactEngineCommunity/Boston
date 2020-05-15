Namespace PGS

    <Serializable()> _
    Public Class Relation

        Public Model As FBM.Model

        Public Id As String = ""

        Public OriginEntity As PGS.Node
        Public OriginAttribute As New List(Of ERD.Attribute)
        Public OriginMandatory As Boolean = False
        Public OriginMultiplicity As pcenumCMMLMultiplicity = pcenumCMMLMultiplicity.One
        Public OriginContributesToPrimaryKey As Boolean = False
        Public OriginPredicate As String = ""

        Public DestinationEntity As PGS.Node
        Public DestinationAttribute As List(Of ERD.Attribute)
        Public DestinationMandatory As Boolean = False
        Public DestinationMultiplicty As pcenumCMMLMultiplicity = pcenumCMMLMultiplicity.One
        Public DestinationPredicate As String = ""

        Public WithEvents RelationFactType As FBM.FactType

        Public Link As ERD.Link


        Public Sub New()
            '---------------------------
            'Parameterless Constructor
            '---------------------------
        End Sub

        Public Sub New(ByRef arModel As FBM.Model, _
                       ByVal asRelationId As String, _
                       ByRef arOriginNode As PGS.Node, _
                       ByVal aiOriginMultiplicity As pcenumCMMLMultiplicity, _
                       ByVal abOriginMandatory As Boolean, _
                       ByVal abOriginContributesToPrimaryKey As Boolean, _
                       ByRef arDestinationNode As PGS.Node, _
                       ByVal aiDestinationMultiplicity As pcenumCMMLMultiplicity, _
                       ByVal abDestinationMandatory As Boolean)

            Me.Model = arModel
            Me.Id = asRelationId

            Me.OriginEntity = arOriginNode
            Me.OriginMultiplicity = aiOriginMultiplicity
            Me.OriginMandatory = abOriginMandatory
            Me.OriginContributesToPrimaryKey = abOriginContributesToPrimaryKey

            Me.DestinationEntity = arDestinationNode
            Me.DestinationMultiplicty = aiDestinationMultiplicity
            Me.DestinationMandatory = abDestinationMandatory

        End Sub

        Private Sub RelationFactType_FactTypeReadingAdded(ByRef arFactTypeReading As FBM.FactTypeReading) Handles RelationFactType.FactTypeReadingAdded

            Call Me.UpdatePredicatePartsFromRelationFactTypeFactTypeReading(arFactTypeReading)

        End Sub

        ''' <summary>
        ''' VM-20160524-Needs to be written (only just started) for the 1.13 database model.
        ''' Updates the OriginPredicatePart and the DestinationPreicatePart of a Relation based on the FactTypeReading.
        ''' FactType must be binary.
        ''' </summary>
        ''' <param name="arFactTypeReading"></param>
        ''' <remarks></remarks>
        Private Sub UpdatePredicatePartsFromRelationFactTypeFactTypeReading(ByRef arFactTypeReading As FBM.FactTypeReading)

            Dim lsSQLQuery As String = ""
            Dim lsPredicate As String = ""
            Dim lrRecordset As ORMQL.Recordset

            If Not arFactTypeReading.FactType.IsBinaryFactType Then
                Exit Sub
            End If

            If arFactTypeReading.FactType.IsManyTo1BinaryFactType Then

                lsPredicate = arFactTypeReading.PredicatePart(0).PredicatePartText & " " & arFactTypeReading.PredicatePart(1).PreBoundText

                If arFactTypeReading.PredicatePart(0).Role.JoinedORMObject.Id = Me.OriginEntity.Name Then

                    lsSQLQuery = "SELECT COUNT(*) "
                    lsSQLQuery &= " FROM " & pcenumCMMLRelations.CoreDestinationPredicate.ToString
                    lsSQLQuery &= " WHERE Relation = '" & Me.Id & "'"

                    lrRecordset = Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                    If lrRecordset("Count").Data = 1 Then
                        'UPDATE doesn't exist as implemented in ORMQL yet
                        'lsSQLQuery = "UPDATE " & pcenumCMMLRelations.CoreDestinationPredicate.ToString
                        'lsSQLQuery &= " SET Predicate = '" & lsPredicate & "'"
                        'lsSQLQuery &= " WHERE Relation = '" & Me.Id & "'"

                        lsSQLQuery = "SELECT *"
                        lsSQLQuery &= " FROM " & pcenumCMMLRelations.CoreDestinationPredicate.ToString
                        lsSQLQuery &= " WHERE Relation = '" & Me.Id & "'"

                        lrRecordset = Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                        lrRecordset("Predicate").Data = lsPredicate
                    Else
                        lsSQLQuery = "INSERT INTO " & pcenumCMMLRelations.CoreDestinationPredicate.ToString
                        lsSQLQuery &= " (Relation, Predicate)"
                        lsSQLQuery &= " VALUES ("
                        lsSQLQuery &= "'" & Me.Id & "'"
                        lsSQLQuery &= ",'" & lsPredicate & "'"
                        lsSQLQuery &= " )"

                        Call Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)
                    End If

                    Me.DestinationPredicate = lsPredicate

                Else
                    lsSQLQuery = "SELECT COUNT(*) "
                    lsSQLQuery &= " FROM " & pcenumCMMLRelations.CoreOriginPredicate.ToString
                    lsSQLQuery &= " WHERE Relation = '" & Me.Id & "'"

                    lrRecordset = Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                    If lrRecordset("Count").Data = 1 Then
                        'UPDATE doesn't exist as implemented in ORMQL yet
                        'lsSQLQuery = "UPDATE " & pcenumCMMLRelations.CoreOriginPredicate.ToString
                        'lsSQLQuery &= " SET Predicate = '" & lsPredicate & "'"
                        'lsSQLQuery &= " WHERE Relation = '" & Me.Id & "'"

                        lsSQLQuery = "SELECT *"
                        lsSQLQuery &= " FROM " & pcenumCMMLRelations.CoreOriginPredicate.ToString
                        lsSQLQuery &= " WHERE Relation = '" & Me.Id & "'"

                        lrRecordset = Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                        lrRecordset("Predicate").Data = lsPredicate
                    Else
                        lsSQLQuery = "INSERT INTO " & pcenumCMMLRelations.CoreOriginPredicate.ToString
                        lsSQLQuery &= " (Relation, Predicate)"
                        lsSQLQuery &= " VALUES ("
                        lsSQLQuery &= "'" & Me.Id & "'"
                        lsSQLQuery &= ",'" & lsPredicate & "'"
                        lsSQLQuery &= " )"

                        Call Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)
                    End If

                    Me.OriginPredicate = lsPredicate

                End If

            End If

        End Sub

        Private Sub RelationFactType_FactTypeReadingModified(ByRef arFactTypeReading As FBM.FactTypeReading) Handles RelationFactType.FactTypeReadingModified

            Call Me.UpdatePredicatePartsFromRelationFactTypeFactTypeReading(arFactTypeReading)

        End Sub

    End Class

End Namespace