Namespace UML
    Public Class UseCaseModel

        Public Actor As New List(Of CMML.Actor)
        Public Process As New List(Of CMML.Process)
        Public ActorToProcessParticipationRelation As FBM.FactTypeInstance
        Public PocessToProcessRelation As FBM.FactTypeInstance

        ''' <summary>
        ''' Creates a unique Process Text for the Process Transition Diagram.
        '''  NB Not necessarily a unique Process Text for the Model/STModel, because more than one ValueType can have the same Process Text.
        ''' </summary>
        ''' <param Text="asRootProcessText"></param>
        ''' <param Text="aiCounter"></param>
        ''' <returns></returns>
        Public Function CreateUniqueProcessText(ByVal asRootProcessText As String, Optional aiCounter As Integer = 0) As String

            Dim lsTrialProcessText As String

            If aiCounter = 0 Then
                lsTrialProcessText = asRootProcessText
            Else
                lsTrialProcessText = asRootProcessText & CStr(aiCounter)
            End If

            CreateUniqueProcessText = lsTrialProcessText

            If Me.Process.Find(Function(x) x.Text = lsTrialProcessText) IsNot Nothing Then
                CreateUniqueProcessText = Me.CreateUniqueProcessText(asRootProcessText, aiCounter + 1)
            Else
                Return lsTrialProcessText
            End If

        End Function


    End Class

End Namespace
