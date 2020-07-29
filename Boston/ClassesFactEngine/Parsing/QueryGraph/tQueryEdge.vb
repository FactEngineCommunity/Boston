Namespace FactEngine

    Public Class QueryEdge

        Public QueryGraph As FactEngine.QueryGraph = Nothing

        Public BaseNode As FactEngine.QueryNode = Nothing
        Public TargetNode As FactEngine.QueryNode = Nothing
        Public Predicate As String = ""

        Public IdentifierList As New List(Of String)

        Public FBMFactType As FBM.FactType = Nothing

        ''' <summary>
        ''' TRUE if is the project of a select/project
        ''' </summary>
        Public IsProjectColumn As Boolean = False

        Public WhichClauseType As pcenumWhichClauseType = pcenumWhichClauseType.None

        ''' <summary>
        ''' Paremeterless New
        ''' </summary>
        Public Sub New()
        End Sub

        Public Sub New(ByRef arQueryGraph As FactEngine.QueryGraph)
            Me.QueryGraph = arQueryGraph
        End Sub

        ''' <summary>
        ''' E.g. For the FactEngine query, "Which Lecturer is in which School", arTargetNode points to the 
        ''' QueryNode for the 'School' FBM ModelObject, and arFBMFactType is the FactType with FactType reading "Lecturer is in School"
        ''' </summary>
        ''' <param name="arTargetNode">The node on the far side of the directed graph</param>
        ''' <param name="arFBMFactType">The FBM FactType which has a FactTypeReading matching the FactEngineQL query clause.</param>
        Public Sub New(ByRef arTargetNode As FactEngine.QueryNode,
                       ByRef arFBMFactType As FBM.FactType)

            Me.TargetNode = arTargetNode
            Me.FBMFactType = arFBMFactType

        End Sub

        Public Sub getAndSetFBMFactType(ByRef arBaseNode As FactEngine.QueryNode,
                                        ByRef arTargetNode As FactEngine.QueryNode,
                                        ByVal asPredicate As String)

            Dim larModelObject As New List(Of FBM.ModelObject)
            larModelObject.Add(arBaseNode.FBMModelObject)
            larModelObject.Add(arTargetNode.FBMModelObject)
            Dim lasPredicatePart As New List(Of String)
            lasPredicatePart.Add(asPredicate)
            lasPredicatePart.Add("")
            Dim larRole As New List(Of FBM.Role)
            Dim lrDummyFactType As New FBM.FactType
            larRole.Add(New FBM.Role(lrDummyFactType, larModelObject(0)))
            larRole.Add(New FBM.Role(lrDummyFactType, larModelObject(1)))
            Dim lrFactTypeReading As New FBM.FactTypeReading(lrDummyFactType, larRole, lasPredicatePart)
            Me.FBMFactType = Me.QueryGraph.Model.getFactTypeByModelObjectsFactTypeReading(larModelObject,
                                                                                        lrFactTypeReading)
            If Me.FBMFactType Is Nothing Then
                Throw New Exception("There is not Fact Type, '" & arBaseNode.FBMModelObject.Id & " " & asPredicate & " " & arTargetNode.FBMModelObject.Id & "', in the Model.")
            End If

        End Sub

    End Class

End Namespace
