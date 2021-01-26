Imports System.Reflection

Namespace FBM

    Partial Public Class Model

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

            Return lrState

        End Function

        Public Sub PopulateSTMStructureFromCoreMDAElements()

            Try
                Me.STMLoading = True
                Dim lsMessage As String = ""

                Dim lsSQLQuery As String = ""
                Dim lrORMRecordset,
                    lrORMRecordset2 As ORMQL.Recordset

                lsSQLQuery = " SELECT *"
                lsSQLQuery &= "  FROM " & pcenumCMMLRelations.CoreStateTransition.ToString

                lrORMRecordset = Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                Dim lrState,
                    lrFromState,
                    lrToState As STM.State

                Dim lrStateTransition As STM.StateTransition

                Dim lrValueType As FBM.ValueType

                '==============================================================================================
                'StateTransitions
                While Not lrORMRecordset.EOF

                    lsSQLQuery = "SELECT *"
                    lsSQLQuery &= " FROM " & pcenumCMMLRelations.CoreStateTransitionIsForValueType.ToString
                    lsSQLQuery &= " WHERE StateTransition = '" & lrORMRecordset.CurrentFact.Id & "'"

                    lrORMRecordset2 = Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                    lrValueType = Me.ValueType.Find(Function(x) x.Id = lrORMRecordset2("ValueType").Data)

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

                    Me.STM.StateTransition.AddUnique(lrStateTransition)

                    lrORMRecordset.MoveNext()
                End While

                '===============================================================================================
                'Start States
                lsSQLQuery = " SELECT *"
                lsSQLQuery &= "  FROM " & pcenumCMMLRelations.CoreValueTypeHasStartCoreElementState.ToString

                lrORMRecordset = Me.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                While Not lrORMRecordset.EOF

                    lrValueType = Me.ValueType.Find(Function(x) x.Id = lrORMRecordset("ValueType").Data)

                    lrState = New STM.State(Me.STM, lrValueType, lrORMRecordset("CoreElement").Data)

                    Me.STM.State.Find(AddressOf lrState.Equals).IsStart = True

                    lrORMRecordset.MoveNext()
                End While

                '===============================================================================================
                'Stop States
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

    End Class

End Namespace
