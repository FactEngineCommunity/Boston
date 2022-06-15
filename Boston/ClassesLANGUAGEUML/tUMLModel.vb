Namespace UML

    ''' <summary>
    ''' This UseCaseModel is for a Page, and is not the tCMMLModel which is at the Model level.
    ''' The UseCaseModel is for Instances Of Actor, Process And associated links between them, rather than the actual Model level objects.
    ''' NB It is under the Namespace UML rather than CMML. CMML is at the Model level, and UML objects are for Page Instances.
    ''' </summary>
    Public Class Model

        Public Model As FBM.Model

        Public Actor As New List(Of UML.Actor)
        Public Process As New List(Of UML.Process)

        Public ActorProcessRelation As New List(Of UML.ActorProcessRelation)
        Public ProcessProcessRelation As New List(Of UML.ProcessProcessRelation)

        Public ActorToProcessParticipationRelationFTI As FBM.FactTypeInstance
        Public PocessToProcessRelationFTI As FBM.FactTypeInstance

        ''' <summary>
        ''' Parameterless constructor.
        ''' </summary>
        Public Sub New()
        End Sub

        Public Sub New(ByRef arFBMModel As FBM.Model)

            Me.Model = arFBMModel
        End Sub

        ''' <summary>
        ''' Creates a unique Process Text for the Process Transition Diagram.
        '''   NB Not necessarily a unique Process Text for the Model/STModel, because more than one ValueType can have the same Process Text.
        ''' </summary>
        ''' <param name="asRootProcessText"></param>
        ''' <param name="aiCounter"></param>
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
