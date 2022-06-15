Imports System.Reflection
Imports System.Xml.Serialization
Imports MindFusion.Diagramming

Namespace FBM

    <Serializable()> _
    Public Class FactInstance
        Inherits FBM.Fact
        Implements FBM.iPageObject

        <XmlAttribute()> _
        Public Shadows ConceptType As pcenumConceptType = pcenumConceptType.Fact

        <XmlIgnore()> _
        Public WithEvents Fact As FBM.Fact 'Pointer to the Fact that this FactInstance relates to. 

        <XmlIgnore()>
        Public Shadows FactType As FBM.FactTypeInstance

        Private Shadows _Data As New List(Of FBM.FactDataInstance)
        <XmlIgnore()>
        Public Shadows Property Data As List(Of FBM.FactDataInstance)
            Get
                Return Me._Data
            End Get
            Set(value As List(Of FBM.FactDataInstance))
                Me._Data = value
                If Me.Model.Loaded And Me.Page.Loaded Then Call Me.makeDirty()
            End Set
        End Property

        Public Overrides Property Model As FBM.Model
            Get
                Return Me._Model
            End Get
            Set(value As FBM.Model)
                Me._Model = value
            End Set
        End Property

        <XmlIgnore()>
        Public FactInstance As FBM.FactInstance 'Used to refer to original object when cloned for convenience.
        'e.g. If working with a CMML.StartState object (as a clone of a tFactInstance object), this 'FactInstance'
        '  refers back to the FactInstance from where the clone was made. This is so that any changes made to the CMML.StartState object
        '  can be reflected back against the original tFactInstance object. The reason that you would do this is because
        '  CMML.StartState objects (for instance) refer to tFactInstance objects within the corresponding FactTypeInstance on a Page.
        '  If the user 'moves' a CMML.StartState object on the Page, then the x,y coordinates need to be updated within the FactInstance so that the
        '  Fact ConceptInstance (Fact on a Page) can be updated in the database when the Page is saved. Otherwise, x,y coordinates cannot be saved back to the database.
        '  - NB The option to always work with FactInstance objects would have negated the need to use this pattern, however it is more
        '  convenient from a programming perspective to work with CMML.StartState, CMML.EndState objects than with a genereric FactInstance object,
        '  which would be confusing over time.
        '  - See CloneStartState, CloneEndState
        '  - NB See also the similar functionality within the FBM.FactDataInstance class.
        '----------------------------------------------------------------------------------------------------------------------------------------

        <NonSerialized(),
        XmlIgnore()>
        Public _Shape As ShapeNode
        <XmlIgnore()>
        Public Property Shape As ShapeNode Implements iPageObject.Shape
            Get
                Return Me._Shape
            End Get
            Set(value As ShapeNode)
                Me._Shape = value
            End Set
        End Property

        <DebuggerBrowsable(DebuggerBrowsableState.Never)>
        Public _X As Integer
        Public Property X() As Integer Implements iPageObject.X
            Get
                Return Me._X
            End Get
            Set(ByVal value As Integer)
                Me._X = value
                If Me.FactInstance IsNot Nothing Then
                    Me.FactInstance._X = value
                End If
            End Set
        End Property

        <DebuggerBrowsable(DebuggerBrowsableState.Never)>
        Public _Y As Integer
        Public Property Y() As Integer Implements iPageObject.Y
            Get
                Return Me._Y
            End Get
            Set(ByVal value As Integer)
                Me._Y = value
                If IsSomething(Me.FactInstance) Then
                    Me.FactInstance._Y = value
                End If
            End Set
        End Property

        <XmlIgnore()> _
        Public Page As FBM.Page

        Public Sub New()
        End Sub

        Public Sub New(ByRef arFactTypeInstance As FBM.FactTypeInstance)

            Me.New()
            Me.Model = arFactTypeInstance.Model
            Me.Page = arFactTypeInstance.Page
            Me.FactType = arFactTypeInstance

        End Sub

        Public Sub New(ByRef as_fact_id As String, ByRef arFactTypeInstance As FBM.FactTypeInstance)

            Me.Symbol = Trim(as_fact_id)
            Me.Id = Me.Symbol
            Me.Model = arFactTypeInstance.Model
            Me.Page = arFactTypeInstance.Page
            Me.FactType = arFactTypeInstance

            '-----------------------------------------------------
            'Join the FactInstance to the Fact in the Model
            '-----------------------------------------------------
            Me.Fact = New FBM.Fact(as_fact_id, arFactTypeInstance.FactType)
            Me.Fact = Me.FactType.FactType.Fact.Find(AddressOf Me.Fact.EqualsById)

        End Sub

        Public Sub New(ByRef arFact As FBM.Fact, ByRef arFactTypeInstance As FBM.FactTypeInstance)

            Me.Id = arFact.Id
            Me.Model = arFact.Model
            Me.Symbol = Trim(arFact.Symbol)
            Me.Page = arFactTypeInstance.Page
            Me.FactType = arFactTypeInstance
            Me.Fact = arFact

        End Sub

        Public Shadows Function Equals(ByVal other As FBM.FactInstance) As Boolean

            Dim lr_data As FBM.FactDataInstance
            Dim lbFound As Boolean = False

            If Me.FactType.Id = other.FactType.Id Then
                For Each lr_data In other.Data
                    If Me.Data.Find(AddressOf lr_data.Equals) Is Nothing Then
                        lbFound = False
                        Return False
                    Else
                        lbFound = True
                    End If
                Next
                Return lbFound
            Else
                Return False
            End If

        End Function

        Public Shadows Function EqualsByFirstDataMatch(ByVal other As FBM.FactInstance) As Boolean

            Dim lr_data As FBM.FactDataInstance
            Dim lbFound As Boolean = False

            If Me.FactType.Id = other.FactType.Id Then
                For Each lr_data In other.Data
                    If Me.Data.Find(Function(x) (x.Role.Id = lr_data.Role.Id) And (x.Concept.Symbol = lr_data.Concept.Symbol)) Is Nothing Then 'AddressOf lr_data.Equals) Is Nothing Then
                        lbFound = False
                    Else
                        lbFound = True
                        Return lbFound
                    End If
                Next
                Return lbFound
            Else
                Return False
            End If

        End Function

        Default Public Overloads Property Item(ByVal asItemValue As String) As FBM.FactDataInstance
            Get
                Return Me.GetFactDataInstanceByRoleName(asItemValue)
            End Get
            Set(value As FBM.FactDataInstance)
                Throw New NotImplementedException("Not implemented")
            End Set
        End Property

        Public Overrides Function Clone() As Object

            Dim lrFactInstance As New FBM.FactInstance
            Dim lrFactDataInstance As FBM.FactDataInstance

            Try
                With Me                    
                    lrFactInstance.Model = .Model                    
                    lrFactInstance.Page = .Page
                    lrFactInstance.Id = .Id
                    lrFactInstance.Name = .Name
                    lrFactInstance.Symbol = .Symbol
                    lrFactInstance.FactType = Me.Page.FactTypeInstance.Find(AddressOf .FactType.Equals)
                    lrFactInstance.Fact = lrFactInstance.FactType.FactType.Fact.Find(AddressOf .Fact.EqualsById)
                    For Each lrFactDataInstance In .Data
                        lrFactInstance.Data.Add(lrFactDataInstance.Clone(Me.Page, lrFactInstance))
                    Next

                End With

                Return lrFactInstance

            Catch ex As Exception
                Dim lsMessage As String
                lsMessage = "Error: tFactInstance.Clone"
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)

                Return Nothing
            End Try

        End Function

        Public Overloads Function Clone(ByRef arPage As FBM.Page, ByRef arFactTypeInstance As FBM.FactTypeInstance) As Object

            Dim lrFactInstance As New FBM.FactInstance
            Dim lrFactDataInstance As FBM.FactDataInstance

            Try
                With Me
                    lrFactInstance.Model = New FBM.Model
                    lrFactInstance.Model = arPage.Model                    
                    lrFactInstance.Page = arPage
                    lrFactInstance.Id = .Id
                    lrFactInstance.Name = .Name
                    lrFactInstance.Symbol = .Symbol

                    lrFactInstance.FactType = arFactTypeInstance
                    lrFactInstance.Fact = arFactTypeInstance.FactType.Fact.Find(AddressOf .Fact.EqualsById)
                    For Each lrFactDataInstance In .Data
                        lrFactInstance.Data.Add(lrFactDataInstance.Clone(arPage, lrFactInstance))
                    Next

                End With

                Return lrFactInstance

            Catch ex As Exception
                Dim lsMessage As String
                lsMessage = "Error: tFactInstance.Clone"
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)

                Return Nothing
            End Try

        End Function

        Public Function CloneConceptInstance() As FBM.ConceptInstance

            Try
                Dim lrConceptInstance As New FBM.ConceptInstance(Me.Model, Me.Page, Me.Id, Me.ConceptType, Me.X, Me.Y)

                Return lrConceptInstance
            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try


        End Function

        Public Function CloneEndStateTransition(ByRef arPage As FBM.Page, ByRef arFromState As STD.State, ByRef arEndStateIndicator As STD.EndStateIndicator) As STD.EndStateTransition

            Dim lrEndStateTransition As New STD.EndStateTransition

            With Me
                lrEndStateTransition.Model = .Model
                lrEndStateTransition.Page = arPage
                lrEndStateTransition.Fact = Me.Fact
                lrEndStateTransition.FactInstance = Me
                lrEndStateTransition.Concept = .Concept
                lrEndStateTransition.X = .X
                lrEndStateTransition.Y = .Y
            End With

            lrEndStateTransition.FromState = arFromState
            lrEndStateTransition.EndStateIndicator = arEndStateIndicator

            Return lrEndStateTransition

        End Function

        Public Function CloneActorProcessRelation(ByRef arPage As FBM.Page, Optional ByRef arActor As UML.Actor = Nothing, Optional ByRef arProcess As UML.Process = Nothing) As UML.ActorProcessRelation

            Dim lrUMLActorProcessRelation As New UML.ActorProcessRelation(arPage.UMLDiagram, arActor, arProcess)
            Try
                With Me
                    lrUMLActorProcessRelation.Model = .Model
                    lrUMLActorProcessRelation.UMLModel = arPage.UMLDiagram
                    lrUMLActorProcessRelation.Page = arPage
                    lrUMLActorProcessRelation.Fact = Me.Fact
                    lrUMLActorProcessRelation.FactInstance = Me
                    lrUMLActorProcessRelation.Concept = .Concept
                    lrUMLActorProcessRelation.X = .X
                    lrUMLActorProcessRelation.Y = .Y
                End With

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

            Return lrUMLActorProcessRelation

        End Function


        Public Function CloneProcessProcessRelation(ByRef arPage As FBM.Page, Optional ByRef arProcess1 As UML.Process = Nothing, Optional ByRef arProcess2 As UML.Process = Nothing) As UML.ProcessProcessRelation

            Dim lrUMLProcessProcessRelation As New UML.ProcessProcessRelation(arPage.UMLDiagram, arProcess1, arProcess2)
            Try
                With Me
                    lrUMLProcessProcessRelation.Model = .Model
                    lrUMLProcessProcessRelation.UMLModel = arPage.UMLDiagram
                    lrUMLProcessProcessRelation.Page = arPage
                    lrUMLProcessProcessRelation.Fact = Me.Fact
                    lrUMLProcessProcessRelation.FactInstance = Me
                    lrUMLProcessProcessRelation.Concept = .Concept
                    lrUMLProcessProcessRelation.X = .X
                    lrUMLProcessProcessRelation.Y = .Y
                End With

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

            Return lrUMLProcessProcessRelation

        End Function


        ''' <summary>
        ''' Start State Indicator is a circle linking to the first State in a State Transition Diagram.
        '''   Generally an STD will only have on staring state
        ''' </summary>
        ''' <param name="arPage"></param>
        ''' <returns></returns>
        Public Function CloneStartStateIndicator(ByRef arPage As FBM.Page, Optional ByRef arState As STD.State = Nothing) As STD.StartStateIndicator

            '------------------------------------------------------------------
            'As in 'Start Status Indicator' within a State Transition Diagram
            '------------------------------------------------------------------

            Try
                If Not Me.FactType.Id = pcenumCMMLRelations.CoreValueTypeHasStartCoreElementState.ToString Then
                    Throw New Exception("Called CloneStartStateIndicator for the wrong type of FactType and Fact.")
                End If

                Dim lrStartStateIndicator As New STD.StartStateIndicator

                With Me
                    lrStartStateIndicator.Model = .Model
                    lrStartStateIndicator.Page = arPage
                    'lrStartStateIndicator.Name = .Concept.Symbol '20200113-VM-Not really needed.
                    lrStartStateIndicator.Fact = Me.Fact
                    lrStartStateIndicator.FactInstance = Me
                    lrStartStateIndicator.Concept = .Concept
                    lrStartStateIndicator.X = .X
                    lrStartStateIndicator.Y = .Y
                End With

                lrStartStateIndicator.State = arState

                Return lrStartStateIndicator

            Catch ex As Exception

                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)

                Return Nothing
            End Try

        End Function

        Public Function CloneStateTransition(ByRef arPage As FBM.Page, ByRef arFromState As STD.State, ByRef arToState As STD.State, ByVal asEventName As String)

            Dim lrStateTransition As New STD.StateTransition

            With Me
                lrStateTransition.Model = .Model
                lrStateTransition.Page = arPage
                lrStateTransition.Name = asEventName 'was .Concept.Symbol  [20210112-VM-wasn't working]
                lrStateTransition.EventName = asEventName
                lrStateTransition.Fact = Me.Fact
                lrStateTransition.FactInstance = Me
                lrStateTransition.Concept = .Concept
                lrStateTransition.X = .X
                lrStateTransition.Y = .Y
            End With

            lrStateTransition.FromState = arFromState
            lrStateTransition.ToState = arToState
            lrStateTransition.EventName = asEventName

            Return lrStateTransition

        End Function

        ''' <summary>
        ''' Enumerates the Data of the Fact as a key value
        ''' </summary>
        ''' <returns>String representing a key of the data within the Fact</returns>
        ''' <remarks>Used in processing ORMQL statements. e.g. Where DISTINCT keyword is used in a SELECT statement</remarks>
        Public Overrides Function EnumerateDataAsKey(ByVal aasKeyColumnOrder As List(Of String)) As String

            Dim lsColumnName As String
            Dim lsKey As String = ""

            For Each lsColumnName In aasKeyColumnOrder
                lsKey &= Me.GetFactDataInstanceByRoleName(lsColumnName).Data
            Next

            Return lsKey

        End Function

        Public Overloads Function EnumerateAsBracketedFact() As String

            Dim lrROle As FBM.RoleInstance
            Dim lrRoleGroup As New List(Of FBM.RoleInstance)
            Dim lrFactDataInstance As FBM.FactDataInstance
            Dim lrFactInstance As New FBM.FactInstance
            Dim lrReferencedFactInstance As New FBM.FactInstance
            Dim liInd As Integer = 0

            Try

                EnumerateAsBracketedFact = "{"

                For Each lrFactDataInstance In Me.Data
                    lrRoleGroup.Add(lrFactDataInstance.Role)
                Next

                lrRoleGroup.Sort(AddressOf FBM.FactTypeInstance.CompareRoleXPositions)

                For Each lrROle In lrRoleGroup
                    lrFactDataInstance = New FBM.FactDataInstance
                    lrFactDataInstance.Role = New FBM.RoleInstance
                    lrFactDataInstance.Role.Id = lrROle.Id
                    lrFactDataInstance = Me.Data.Find(AddressOf lrFactDataInstance.EqualsByRole)
                    If lrROle.TypeOfJoin = pcenumRoleJoinType.FactType Then
                        lrReferencedFactInstance = New FBM.FactInstance(lrFactDataInstance.Data, lrROle.JoinsFactType)
                        lrFactInstance = lrROle.JoinsFactType.Fact.Find(AddressOf lrReferencedFactInstance.EqualsById)
                        EnumerateAsBracketedFact &= lrFactInstance.EnumerateAsBracketedFact
                    Else
                        EnumerateAsBracketedFact &= lrFactDataInstance.Data
                    End If

                    liInd += 1
                    If (liInd > 0) And (liInd < Me.Data.Count) Then
                        EnumerateAsBracketedFact &= ","
                    End If
                Next

                EnumerateAsBracketedFact &= "}"

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)

                Return "{}"
            End Try

        End Function

        Public Function GetFactDataInstanceByRoleName(ByVal asRoleName As String) As FBM.FactDataInstance

            Try
                Dim lrFactDataInstance As New FBM.FactDataInstance

                Dim larFactDataInstance = From FactDataInstance In Me.Data
                                          Where FactDataInstance.Role.Name = asRoleName
                                          Distinct Select FactDataInstance

                For Each lrFactDataInstance In larFactDataInstance
                    Exit For
                Next

                Return lrFactDataInstance

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)

                Return Nothing
            End Try

        End Function

        Public Overrides Sub makeDirty()
            Me.FactType.isDirty = True
            Me.Model.IsDirty = True
            Me.Page.IsDirty = True
            Me.isDirty = True
        End Sub

        Public Shadows Sub Save(Optional ByVal abRapidSave As Boolean = False)

            Dim lrFactDataInstance As FBM.FactDataInstance
            Dim lrConceptInstance As New FBM.ConceptInstance
            Dim lrConcept As FBM.Concept
            Try
                lrConceptInstance.ModelId = Me.Fact.Model.ModelId
                lrConceptInstance.PageId = Me.Page.PageId
                lrConceptInstance.Symbol = Me.Fact.Symbol
                lrConceptInstance.X = Me.X
                lrConceptInstance.Y = Me.Y
                lrConceptInstance.ConceptType = pcenumConceptType.Fact

                If abRapidSave Then

                    lrConcept = New FBM.Concept(lrConceptInstance.Symbol, True)
                    Call lrConcept.Save()

                    Call TableConceptInstance.AddConceptInstance(lrConceptInstance)


                    For Each lrFactDataInstance In Me.Data

                        lrConceptInstance = New FBM.ConceptInstance
                        lrConceptInstance.ModelId = Me.FactType.Model.ModelId
                        lrConceptInstance.PageId = Me.Page.PageId
                        lrConceptInstance.Symbol = lrFactDataInstance.Data
                        lrConceptInstance.RoleId = lrFactDataInstance.Role.Id
                        lrConceptInstance.X = lrFactDataInstance.X
                        lrConceptInstance.Y = lrFactDataInstance.Y
                        lrConceptInstance.ConceptType = pcenumConceptType.Value

                        'NB Don't use abRapidSave here because more than one Fact can use the same ConceptInstance for its FactData.
                        If TableConceptInstance.ExistsConceptInstance(lrConceptInstance) Then
                            Call TableConceptInstance.UpdateConceptInstance(lrConceptInstance)
                        Else
                            lrConcept = New FBM.Concept(lrFactDataInstance.Data)
                            lrConcept.Save()
                            Call TableConceptInstance.AddConceptInstance(lrConceptInstance)
                        End If
                    Next
                ElseIf Me.isDirty Then

                    If TableConceptInstance.ExistsConceptInstance(lrConceptInstance) Then
                        Call TableConceptInstance.UpdateConceptInstance(lrConceptInstance)
                    Else
                        lrConcept = New FBM.Concept(lrConceptInstance.Symbol, True)
                        lrConcept.Save()
                        Call TableConceptInstance.AddConceptInstance(lrConceptInstance)
                    End If

                    For Each lrFactDataInstance In Me.Data

                        lrConceptInstance = New FBM.ConceptInstance
                        lrConceptInstance.ModelId = Me.FactType.Model.ModelId
                        lrConceptInstance.PageId = Me.Page.PageId
                        lrConceptInstance.Symbol = lrFactDataInstance.Data
                        lrConceptInstance.RoleId = lrFactDataInstance.Role.Id
                        lrConceptInstance.X = lrFactDataInstance.X
                        lrConceptInstance.Y = lrFactDataInstance.Y
                        lrConceptInstance.ConceptType = pcenumConceptType.Value

                        'NB Don't use abRapidSave here because more than one Fact can use the same ConceptInstance for its FactData.
                        If TableConceptInstance.ExistsConceptInstance(lrConceptInstance, False) Then
                            Call TableConceptInstance.UpdateConceptInstance(lrConceptInstance)
                        Else
                            lrConcept = New FBM.Concept(lrFactDataInstance.Data, True)
                            lrConcept.Save()
                            Call TableConceptInstance.AddConceptInstance(lrConceptInstance)
                        End If
                    Next

                    Me.isDirty = False

                End If 'IsDirty

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Public Sub MouseDown() Implements iPageObject.MouseDown
            Throw New NotImplementedException()
        End Sub

        Public Sub MouseMove() Implements iPageObject.MouseMove
            Throw New NotImplementedException()
        End Sub

        Public Sub MouseUp() Implements iPageObject.MouseUp
            Throw New NotImplementedException()
        End Sub

        Public Sub NodeDeleting() Implements iPageObject.NodeDeleting
            Throw New NotImplementedException()
        End Sub

        Public Sub NodeDeselected() Implements iPageObject.NodeDeselected
            Throw New NotImplementedException()
        End Sub

        Public Sub NodeModified() Implements iPageObject.NodeModified
            Throw New NotImplementedException()
        End Sub

        Public Sub NodeSelected() Implements iPageObject.NodeSelected
            Throw New NotImplementedException()
        End Sub

        Public Sub Move(aiNewX As Integer, aiNewY As Integer, abBroadcastInterfaceEvent As Boolean) Implements iPageObject.Move
            Throw New NotImplementedException()
        End Sub

        Public Sub Moved() Implements iPageObject.Moved
            Throw New NotImplementedException()
        End Sub

        Public Sub RepellNeighbouringPageObjects(aiDepth As Integer) Implements iPageObject.RepellNeighbouringPageObjects
            Throw New NotImplementedException()
        End Sub

        Public Sub SetAppropriateColour() Implements iPageObject.SetAppropriateColour
            Throw New NotImplementedException()
        End Sub

        Public Sub EnableSaveButton() Implements iPageObject.EnableSaveButton
            Throw New NotImplementedException()
        End Sub

        Private Sub Fact_ModelErrorAdded(ByRef arModelError As ModelError) Handles Fact.ModelErrorAdded


            Try
                If Me.Page IsNot Nothing Then
                    Try
                        If Me.FactType.FactTable IsNot Nothing Then
                            Me.FactType.FactTable.ResortFactTable()
                        End If
                    Catch
                    End Try
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