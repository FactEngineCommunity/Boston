Namespace BackCast

    Public Class Voice
        Public voice_id As String

        Public Property VoiceId As String
            Get
                Return Me.voice_id
            End Get
            Set(value As String)
                Me.voice_id = value
            End Set
        End Property

        Public Stability As Double = 0.27
        Public SimilarityBoost As Double = 0.64

        Public name As String
        Public samples As Object
        Public category As String = "generated" '"cloned" 
        Public fine_tuning As FineTuning
        Public labels As Labels
        Public description As Object
        Public preview_url As String
        Public available_for_tiers As List(Of Object)
        Public settings As Object
        Public sharing As Object
    End Class

    Public Class FineTuning
        Public model_id As Object
        Public is_allowed_to_fine_tune As Boolean
        Public fine_tuning_requested As Boolean
        Public finetuning_state As String
        Public verification_attempts As Object
        Public verification_failures As List(Of Object)
        Public verification_attempts_count As Integer
        Public slice_ids As Object
    End Class

    Public Class Labels
        Public accent As String
        Public age As String
        Public gender As String
    End Class


    ''' <summary>
    ''' For the JSON returned from ElevenLabs
    ''' </summary>
    Public Class RootObjectWrapper
        Public Property Voices As List(Of Voice)
    End Class

End Namespace