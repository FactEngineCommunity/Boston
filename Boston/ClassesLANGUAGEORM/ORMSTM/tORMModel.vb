Imports System.Reflection

Namespace FBM

    Partial Public Class Model

        Public Sub createCMMLEndStateIndicator(ByRef arEndStateIndicator As STM.EndStateIndicator)

            Dim lsSQLQuery As String

            lsSQLQuery = "INSERT INTO " & pcenumCMMLRelations.CoreElementHasElementType.ToString & " (Element, ElementType)"
            lsSQLQuery &= " VALUES ('" & arEndStateIndicator.EndStateId & "','EndStateIndicator'"
            lsSQLQuery &= ")"

            arEndStateIndicator.Fact = Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)

        End Sub

        Public Sub createCMMLEndStateTransition(ByRef arEndStateTransition As STM.EndStateTransition)

            Dim lsSQLQuery As String

            lsSQLQuery = "INSERT INTO " & pcenumCMMLRelations.CoreValueTypeHasEndCoreElementState.ToString & " (ValueType,EndState,CoreElement,Event)"
            lsSQLQuery &= " VALUES ('" & arEndStateTransition.ValueType.Id & "','" & arEndStateTransition.EndStateId & "','" & arEndStateTransition.State.Name & "','" & arEndStateTransition.Event & "'"
            lsSQLQuery &= ")"

            arEndStateTransition.Fact = Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)

        End Sub

        Public Function createCMMLState(ByRef arValueType As FBM.ValueType, ByVal asStateName As String) As STM.State

            Dim lrState = New STM.State
            lrState.ValueType = arValueType
            lrState.Name = asStateName
            lrState.Model = Me.STM

            Me.STM.State.AddUnique(lrState)

            Dim lsSQLQuery As String

            lsSQLQuery = "INSERT INTO " & pcenumCMMLRelations.CoreElementHasElementType.ToString & " (Element, ElementType)"
            lsSQLQuery &= " VALUES ('" & lrState.Name & "','State'"
            lsSQLQuery &= ")"

            lrState.setFact(Me.ORMQL.ProcessORMQLStatement(lsSQLQuery))

            Return lrState

        End Function

        Public Sub PopulateSTMStructureFromCoreMDAElements()

            Try
                Me.STMLoading = True
                Dim lsMessage As String = ""

                Dim lsSQLQuery As String = ""
                Dim lrORMRecordset As ORMQL.Recordset

                '==============================================================================================
                'StateTransitions
                lsSQLQuery = " SELECT *"
                lsSQLQuery &= "  FROM " & pcenumCMMLRelations.CoreStateTransition.ToString

                lrORMRecordset = Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                Dim lrState,
                    lrFromState,
                    lrToState As STM.State

                Dim lrStateTransition As STM.StateTransition
                Dim lrValueType As FBM.ValueType

                While Not lrORMRecordset.EOF

                    lrValueType = Me.ValueType.Find(Function(x) x.Id = lrORMRecordset("ValueType").Data)

                    lrFromState = New STM.State
                    lrFromState.ValueType = lrValueType
                    lrFromState.Name = lrORMRecordset("Concept1").Data
                    lrFromState.Model = Me.STM

                    lrToState = New STM.State
                    lrToState.ValueType = lrValueType
                    lrToState.Name = lrORMRecordset("Concept2").Data
                    lrToState.Model = Me.STM

                    Me.STM.State.AddUnique(lrFromState)
                    Me.STM.State.AddUnique(lrToState)

                    lrStateTransition = New STM.StateTransition
                    lrStateTransition.FromState = lrFromState
                    lrStateTransition.ToState = lrToState
                    lrStateTransition.Event = lrORMRecordset("Event").Data

                    lrStateTransition.Fact = lrORMRecordset.CurrentFact

                    Me.STM.StateTransition.AddUnique(lrStateTransition)

                    lrORMRecordset.MoveNext()
                End While

                '===============================================================================================
                'Orphan States
                lsSQLQuery = " SELECT *"
                lsSQLQuery &= "  FROM " & pcenumCMMLRelations.CoreElementHasElementType.ToString
                lsSQLQuery &= " WHERE ElementType = 'State'"

                lrORMRecordset = Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                While Not lrORMRecordset.EOF

                    If Me.STM.State.Find(Function(x) x.Name = lrORMRecordset("Element").Data) Is Nothing Then
                        lrState = New STM.State(Me.STM, Nothing, lrORMRecordset("Element").Data)
                        lrState.Fact = lrORMRecordset.CurrentFact

                        Me.STM.State.AddUnique(lrState)
                    End If

                    lrORMRecordset.MoveNext()
                End While

                '===============================================================================================
                'Start State Transition
                lsSQLQuery = " SELECT *"
                lsSQLQuery &= "  FROM " & pcenumCMMLRelations.CoreValueTypeHasStartCoreElementState.ToString

                lrORMRecordset = Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                While Not lrORMRecordset.EOF

                    lrValueType = Me.ValueType.Find(Function(x) x.Id = lrORMRecordset("ValueType").Data)

                    lrState = New STM.State(Me.STM, lrValueType, lrORMRecordset("CoreElement").Data)

                    lrState = Me.STM.State.Find(AddressOf lrState.Equals)
                    If lrState Is Nothing Then
                        lrToState = New STM.State
                        lrToState.ValueType = lrValueType
                        lrToState.Name = lrORMRecordset("CoreElement").Data
                        lrToState.Model = Me.STM

                        lrState = lrToState
                        Me.STM.State.AddUnique(lrToState)
                    Else
                        lrState.IsStart = True
                    End If

                    '-------------------------------------------------------------------------------
                    'StartStateTransition
                    Dim lrStartStateTransition = New STM.StartStateTransition(Me.STM, lrValueType, lrState, lrORMRecordset("Event").Data, lrORMRecordset.CurrentFact)
                    Me.STM.StartStateTransition.Add(lrStartStateTransition)

                    lrORMRecordset.MoveNext()
                End While

                '===============================================================================================
                'End State Transitions
                lsSQLQuery = " SELECT *"
                lsSQLQuery &= "  FROM " & pcenumCMMLRelations.CoreValueTypeHasEndCoreElementState.ToString

                lrORMRecordset = Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                While Not lrORMRecordset.EOF

                    lrValueType = Me.ValueType.Find(Function(x) x.Id = lrORMRecordset("ValueType").Data)

                    lrState = New STM.State(Me.STM, lrValueType, lrORMRecordset("CoreElement").Data)

                    lrState = Me.STM.State.Find(AddressOf lrState.Equals)
                    lrState.IsStop = True

                    Dim lrEndStateTransition = New STM.EndStateTransition(Me.STM, lrValueType, lrState, lrORMRecordset("Event").Data)
                    lrEndStateTransition.EndStateId = lrORMRecordset("EndState").Data
                    lrEndStateTransition.Fact = lrORMRecordset.CurrentFact

                    Me.STM.EndStateTransition.Add(lrEndStateTransition)

                    lrORMRecordset.MoveNext()
                End While

                Me.STMLoading = False

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)

                Me.STMLoading = False
            End Try
        End Sub

        Public Sub removeCMMLEndStateTransition(ByRef arEndStateTransition As STM.EndStateTransition)

            Dim lsSQLQuery As String

            'VM-Complete this
            lsSQLQuery = "DELETE FROM " & pcenumCMMLRelations.CoreValueTypeHasEndCoreElementState.ToString
            lsSQLQuery &= " WHERE ValueType = '" & arEndStateTransition.ValueType.Id & "'"
            lsSQLQuery &= " AND CoreElement = '" & arEndStateTransition.State.Name & "'"
            lsSQLQuery &= " AND EndState = '" & arEndStateTransition.EndStateId & "'"

            'NB Will automatically delete the corresponding FactInstances at the Page level as well.
            Call Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)

        End Sub

        Public Sub removeCMMLStartStateTransition(ByRef arStartStateTransition As STM.StartStateTransition)

            Dim lsSQLQuery As String

            'VM-Complete this
            lsSQLQuery = "DELETE FROM " & pcenumCMMLRelations.CoreValueTypeHasStartCoreElementState.ToString
            lsSQLQuery &= " WHERE ValueType = '" & arStartStateTransition.ValueType.Id & "'"
            lsSQLQuery &= " AND CoreElement = '" & arStartStateTransition.State.Name & "'"
            lsSQLQuery &= " AND Event = '" & arStartStateTransition.Event & "'"

            'NB Will automatically delete the corresponding FactInstances at the Page level as well.
            Call Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)

        End Sub

        Public Function setValueTypeAsStateTransitionBased(ByRef arValueType As FBM.ValueType) As FBM.Fact

            Dim lsSQLQuery As String

            Try
                lsSQLQuery = "SELECT *"
                lsSQLQuery &= " FROM " & pcenumCMMLRelations.CoreValueTypeIsStateTransitionBased.ToString
                lsSQLQuery &= " WHERE IsStateTransitionBased = '" & arValueType.Id & "'"

                Dim lrRecordset As ORMQL.Recordset = Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                If lrRecordset.Facts.Count > 0 Then
                    Return lrRecordset.CurrentFact
                Else
                    lsSQLQuery = "INSERT INTO " & pcenumCMMLRelations.CoreValueTypeIsStateTransitionBased.ToString
                    lsSQLQuery &= " (IsStateTransitionBased) VALUES ('" & arValueType.Id & "')"

                    Dim lrFact As FBM.Fact = Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                    Return lrFact
                End If

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)

                Return Nothing
            End Try


        End Function

    End Class

End Namespace
