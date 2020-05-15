Imports System.Xml.Serialization

Namespace FBM
    <Serializable()> _
    Public Class ModelObject
        Inherits Viev.FBM.ModelObject

        Public Function CloneEntityTypeInstance(ByRef arPage As FBM.Page) As FBM.EntityTypeInstance

            Dim lrEntityTypeInstance As New FBM.EntityTypeInstance

            Try
                With Me
                    lrEntityTypeInstance.Model = arPage.Model
                    lrEntityTypeInstance.Page = arPage
                    lrEntityTypeInstance.Name = .Name
                    lrEntityTypeInstance.Id = Me.Name
                End With

                Return lrEntityTypeInstance

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                Me.Model.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace, True)

                Return lrEntityTypeInstance
            End Try

        End Function


        Public Function CloneRoleConstraintInstance(ByRef arPage As FBM.Page) As FBM.RoleConstraintInstance

            Dim lrRoleConstraintInstance As New FBM.RoleConstraintInstance

            With Me
                lrRoleConstraintInstance.Name = .Name
                lrRoleConstraintInstance.Id = Me.Name
                lrRoleConstraintInstance.Symbol = Me.Name
                lrRoleConstraintInstance.Page = arPage
                lrRoleConstraintInstance.Model = arPage.Model
            End With

            Return lrRoleConstraintInstance

        End Function


        Public Overrides Sub SwitchConcept(ByVal arNewConcept As Concept)

            '--------------------------------------------------------
            'See if the NewSymbol is already in the ModelDictionary
            '--------------------------------------------------------
            Dim lsOriginalSymbol As String = ""
            Dim lrOriginalDictionaryEntry As New FBM.DictionaryEntry(Me.Model, Me.Concept.Symbol, pcenumConceptType.Value)
            Dim lrNewDictionaryEntry As New FBM.DictionaryEntry(Me.Model, arNewConcept.Symbol, pcenumConceptType.Value)
            Dim lsDebugMessage As String = ""
            Dim lsMessage As String

            Try

                lsOriginalSymbol = Me.Concept.Symbol

                If (lrOriginalDictionaryEntry.Concept.Symbol = lrNewDictionaryEntry.Concept.Symbol) Then
                    '--------------------
                    'Nothing to do here
                    '--------------------
                Else
                    lrNewDictionaryEntry = Me.Model.ModelDictionary.Find(AddressOf lrNewDictionaryEntry.Equals)

                    If IsSomething(lrNewDictionaryEntry) Then
                        '----------------------------------------------------------------------------
                        'The NewConcept exists in the ModelDictionary
                        '  Substitute the existing Concept for a ModelDictionary entry (Concept) that
                        '  already exists in the Model
                        '----------------------------------------------------------------------------                     
                        ''-----------------------------------------------------------------------------------------------------------
                        ''Set the Symbol/Concept.Symbol values of the NewDictionaryEntry (existing entry), because
                        ''  .Equals may have matched a DictionaryEntry with the same Lowercase string value, but not the same value
                        ''-----------------------------------------------------------------------------------------------------------
                        'lrNewDictionaryEntry.Symbol = arNewConcept.Symbol
                        'lrNewDictionaryEntry.Concept.Symbol = arNewConcept.Symbol

                        lrOriginalDictionaryEntry = Me.Model.ModelDictionary.Find(AddressOf lrOriginalDictionaryEntry.Equals)

                        If IsSomething(lrOriginalDictionaryEntry) Then
                            Call Me.Model.DeprecateRealisationsForDictionaryEntry(lrOriginalDictionaryEntry)
                        Else
                            '----------------------------------------------------------------------------------------------------------------------------------
                            'Throw a warning message but do not interupt programme flow.
                            '  We're going to deprecate Realisations for the DictionaryEntry anyway.
                            '----------------------------------------------------------------------------------------------------------------------------------
                            lsMessage = "Original DictionaryEntry for FactData.Concept not found in the ModelDictionary"
                            Call Me.Model.ThrowErrorMessage(lsMessage, pcenumErrorType.Warning, "", False)
                        End If

                        Me.Concept = lrNewDictionaryEntry.Concept
                        lrNewDictionaryEntry.AddConceptType(Me.ConceptType)
                    Else
                        '-------------------------------------------------------------------------
                        'The NewConcept does not exist in the ModelDictionary.
                        '  Modify the existing Concept, effectively updating the ModelDictionary.
                        '-------------------------------------------------------------------------
                        '20150504-VM-Remove this commented out code if all is working well.
                        'Quite obviously causes error on Save if OriginalDictionaryEntry doesn't exist in the ModelDictionary
                        'Seems to serve no purpose...because OriginalDictionaryEntry was not in the dictionary
                        'So why Save it?
                        'lrOriginalDictionaryEntry = New FBM.DictionaryEntry(Me.Model, lsOriginalSymbol, pcenumConceptType.Value)
                        'lrOriginalDictionaryEntry = Me.Model.ModelDictionary.Find(AddressOf lrOriginalDictionaryEntry.Equals)
                        'lrOriginalDictionaryEntry.Save()
                        '---------------------------------------------------
                        'Make sure that the new Concept is in the database
                        '---------------------------------------------------
                        arNewConcept.Save()
                        Call TableModelDictionary.ModifySymbol(Me.Model, lrOriginalDictionaryEntry, arNewConcept.Symbol, Me.ConceptType)
                        Me.Concept.Symbol = arNewConcept.Symbol

                    End If

                    Call Me.Model.MakeDirty()
                End If

            Catch ex As Exception
                lsMessage = "Error: tFactData.Data.Set"
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                Me.Model.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace, True)
            End Try

        End Sub




    End Class
End Namespace
