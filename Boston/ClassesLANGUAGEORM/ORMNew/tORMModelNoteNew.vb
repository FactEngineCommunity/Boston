Imports System.Xml.Serialization

Namespace FBM
    <Serializable()> _
    Public Class ModelNote
        Inherits Viev.FBM.ModelNote

        Public Shadows Function CloneInstance(ByRef arPage As FBM.Page, Optional ByVal abAddToPage As Boolean = False) As FBM.ModelNoteInstance

            Dim lrModelNoteInstance As New FBM.ModelNoteInstance

            With Me
                lrModelNoteInstance.Id = .Id
                lrModelNoteInstance.Model = arPage.Model
                lrModelNoteInstance.Page = arPage
                lrModelNoteInstance.Text = .Text
                If IsSomething(.JoinedObjectType) Then
                    Select Case Me.JoinedObjectType.ConceptType
                        Case Is = pcenumConceptType.EntityType
                            lrModelNoteInstance.JoinedObjectType = arPage.EntityTypeInstance.Find(AddressOf Me.JoinedObjectType.Equals)
                        Case Is = pcenumConceptType.ValueType
                            lrModelNoteInstance.JoinedObjectType = arPage.ValueTypeInstance.Find(AddressOf Me.JoinedObjectType.Equals)
                        Case Is = pcenumConceptType.FactType
                            lrModelNoteInstance.JoinedObjectType = arPage.FactTypeInstance.Find(AddressOf Me.JoinedObjectType.Equals)
                    End Select
                Else
                    lrModelNoteInstance.JoinedObjectType = Nothing
                End If
            End With

            If abAddToPage Then
                arPage.ModelNote.Add(lrModelNoteInstance)
            End If

            Return lrModelNoteInstance

        End Function

        ''' <summary>
        ''' Saves the ModelNote to the database
        ''' </summary>
        ''' <remarks></remarks>
        Public Overloads Overrides Sub Save()

            Dim lrDictionaryEntry As New FBM.DictionaryEntry(Me.Model, Me.Id, pcenumConceptType.ModelNote, Me.ShortDescription, Me.LongDescription)
            lrDictionaryEntry = Me.Model.ModelDictionary.Find(AddressOf lrDictionaryEntry.Equals)
            lrDictionaryEntry.isModelNote = True

            If TableModelNote.ExistsModelNote(Me) Then
                Call TableModelNote.UpdateModelNote(Me)
                Call lrDictionaryEntry.Save()
            Else
                Try
                    pdbConnection.BeginTrans()
                    Call lrDictionaryEntry.Save()
                    If TableModelNote.ExistsModelNote(Me) Then
                        '----------------------------------------------------------------
                        'ModeNote already exists in Richmond DB, so no need to add.
                        '  i.e. EntityType did not exist in Model, but did in Richmond.
                        '  EntityTypes in Richmond are model independant.
                        '----------------------------------------------------------------
                    Else
                        Call TableModelNote.AddModelNote(Me)
                    End If
                    pdbConnection.CommitTrans()
                Catch ar_err As Exception
                    pdbConnection.RollbackTrans()
                    Throw New ApplicationException("Error: ModelNote.Save: " & ar_err.Message)
                End Try
            End If

        End Sub

    End Class

End Namespace
