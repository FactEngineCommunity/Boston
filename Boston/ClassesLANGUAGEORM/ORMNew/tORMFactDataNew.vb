Imports System.Xml.Serialization

Namespace FBM
    <Serializable()> _
    Public Class FactData
        Inherits Viev.FBM.FactData

        ''' <summary>
        ''' The Data stored for the Role for the Fact.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overrides Property Data() As String
            Get
                Return Me.Concept.Symbol
            End Get
            Set(ByVal value As String)
                '--------------------------------------------------------
                'See if the NewSymbol is already in the ModelDictionary
                '--------------------------------------------------------
                Dim lsOriginalSymbol As String = ""
                Dim lrOriginalDictionaryEntry As New FBM.DictionaryEntry(Me.Model, Me.Concept.Symbol, pcenumConceptType.Value)
                Dim lrNewDictionaryEntry As New FBM.DictionaryEntry(Me.Model, value, pcenumConceptType.Value)
                Dim lsDebugMessage As String = ""
                Dim lsMessage As String

                Try

                    lsOriginalSymbol = Me.Concept.Symbol
                    Call Me.Model.ThrowErrorMessage("....This is the fact being modified..." & Me.Fact.Symbol, pcenumErrorType.Information, "", False)

                    lrNewDictionaryEntry = Me.Model.ModelDictionary.Find(AddressOf lrNewDictionaryEntry.Equals)

                    If lrNewDictionaryEntry IsNot Nothing Then
                        '----------------------------------------------------------------------------
                        'The NewConcept exists in the ModelDictionary
                        '  Substitute the existing Concept for a ModelDictionary entry (Concept) that
                        '  already exists in the Model. i.e. Switch Concepts.
                        '----------------------------------------------------------------------------                     
                        lrOriginalDictionaryEntry = Me.Model.ModelDictionary.Find(AddressOf lrOriginalDictionaryEntry.Equals)

                        If lrOriginalDictionaryEntry IsNot Nothing Then
                            If (lrOriginalDictionaryEntry.Concept.Symbol = lrNewDictionaryEntry.Concept.Symbol) Then
                                '-------------------------------------------------------------------------------------------------------------------
                                'Don't worry about removing the OriginalDictionaryEntry as the FactData.Data/Concept was set to its original value
                                '-------------------------------------------------------------------------------------------------------------------
                            Else
                                If lrOriginalDictionaryEntry.Realisations.Count <= 1 Then
                                    '--------------------------------------------------------------
                                    'Remove the original DictionaryEnty if it is no longer needed
                                    '--------------------------------------------------------------
                                    Me.Model.RemoveDictionaryEntry(lrOriginalDictionaryEntry)
                                Else
                                    Call Me.Model.DeprecateRealisationsForDictionaryEntry(lrOriginalDictionaryEntry)
                                End If
                            End If
                        Else
                            '--------------------------------------------------------------------------------------------------------------
                            'Throw a warning message but do not interupt programme flow. 
                            ' We were going to remove the OriginalDictionaryEntry anyway.
                            '--------------------------------------------------------------------------------------------------------------
                            lsMessage = "Original DictionaryEntry for FactData.Concept not found in the ModelDictionary"
                            Call Me.Model.ThrowErrorMessage(lsMessage, pcenumErrorType.Warning, "", False)
                        End If

                        Me.Concept = lrNewDictionaryEntry.Concept

                        lrNewDictionaryEntry.Realisations.Add(lrNewDictionaryEntry.Concept)
                        lrNewDictionaryEntry.AddConceptType(pcenumConceptType.Value)

                        RaiseEvent ConceptSwitched(Me.Concept)

                        lsDebugMessage = "FactData.Data.Set"
                        If IsSomething(Me.FactType) Then
                            lsDebugMessage &= vbCrLf & "FactType.Id: " & Me.FactType.Id
                        Else
                            lsDebugMessage &= vbCrLf & "FactType.Id: Nothing"
                        End If
                        If IsSomething(Me.Fact) Then
                            lsDebugMessage &= vbCrLf & "Fact.Symbol: " & Me.Fact.Symbol
                        Else
                            lsDebugMessage &= vbCrLf & "Fact.Symbol: Nothing"
                        End If
                        lsDebugMessage &= vbCrLf & "Role.Id: " & Me.Role.Id
                        lsDebugMessage &= vbCrLf & "Original Data/Concept.Symbol: " & Me.Data
                        lsDebugMessage &= vbCrLf & "New Data/Concept.Symbol: '" & lrNewDictionaryEntry.Symbol & "' already exists in the ModelDictionary"
                        Call Me.Model.ThrowErrorMessage(lsDebugMessage, pcenumErrorType.Information, "", False)

                    Else
                        '-------------------------------------------------------------------------
                        'The NewConcept does not exist in the ModelDictionary.
                        '  Modify the existing Concept, effectively updating the ModelDictionary.
                        '  NB If the OriginalDictionaryEntry is not in the ModelDictionary, then
                        '  the Data/Value must be a new Data/Value altogether, so create a new
                        '  DictionaryEntry in the ModelDictionary.
                        '-------------------------------------------------------------------------
                        lrOriginalDictionaryEntry = New FBM.DictionaryEntry(Me.Model, lsOriginalSymbol, pcenumConceptType.Value)
                        If IsSomething(Me.Model.ModelDictionary.Find(AddressOf lrOriginalDictionaryEntry.Equals)) Then
                            lrOriginalDictionaryEntry = Me.Model.ModelDictionary.Find(AddressOf lrOriginalDictionaryEntry.Equals)
                            Me.Model.DeprecateRealisationsForDictionaryEntry(lrOriginalDictionaryEntry)
                        Else
                            lrNewDictionaryEntry = New FBM.DictionaryEntry(Me.Model, value, pcenumConceptType.Value)
                            Me.Model.AddModelDictionaryEntry(lrNewDictionaryEntry)
                            Me.Concept = lrNewDictionaryEntry.Concept
                        End If

                        Dim lrConcept As New FBM.Concept(value)
                        Call lrConcept.Save()

                        Call TableModelDictionary.ModifySymbol(Me.Model, lrOriginalDictionaryEntry, value, pcenumConceptType.Value)
                        Me.Concept.Symbol = value
                        RaiseEvent ConceptSymbolUpdated()

                        lsDebugMessage = "Setting FactData.Concept.Symbol to new Concep/DictionaryEntry: " & value
                        Call Me.Model.ThrowErrorMessage(lsDebugMessage, pcenumErrorType.Information, "", False)
                    End If

                    Call Me.Model.MakeDirty()

                Catch ex As Exception
                    lsMessage = "Error: tFactData.Data.Set"
                    lsMessage &= vbCrLf & vbCrLf & ex.Message
                    Me.Model.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace, True)
                End Try
            End Set
        End Property


        Public Overrides Function RemoveFromModel(Optional ByVal abForceRemoval As Boolean = False, _
                                                  Optional ByVal abCheckForErrors As Boolean = True) As Boolean '(ByRef arError As FBM.ModelError) As Boolean

            Me.Fact.Data.Remove(Me)
            TableFactData.DeleteFactData(Me)

            Dim lrDictionaryEntry As New FBM.DictionaryEntry(Me.Model, Me.Data, pcenumConceptType.Value)
            Call Me.Model.DeprecateRealisationsForDictionaryEntry(lrDictionaryEntry)

        End Function

        Public Overrides Sub Save()

            Dim lsMessage As String

            Try
                If TableFactData.ExistsFactData(Me) Then
                    TableFactData.UpdateFactData(Me)
                Else
                    Dim lrDictionaryEntry As New FBM.DictionaryEntry(Me.Model, Me.Data, pcenumConceptType.Value)
                    lrDictionaryEntry = Me.Model.ModelDictionary.Find(AddressOf lrDictionaryEntry.Equals)

                    '------------------------------------------------------
                    'CodeSafe: If no dictionary entry exists, create one.
                    '------------------------------------------------------
                    If lrDictionaryEntry Is Nothing Then
                        lsMessage = "No DictionaryEntry exists for FactData. Creating a DictionaryEntry for the FactData"

                        lrDictionaryEntry = New FBM.DictionaryEntry(Me.Model, Me.Data, pcenumConceptType.Value)
                        lrDictionaryEntry = Me.Model.AddModelDictionaryEntry(lrDictionaryEntry)
                    End If

                    lrDictionaryEntry.isValue = True

                    Call lrDictionaryEntry.Save()
                    TableFactData.AddFactData(Me)
                End If
            Catch ex As Exception
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                lsMessage &= vbCrLf
                lsMessage &= vbCrLf & "FactTypeId: " & Me.FactType.Id
                lsMessage &= vbCrLf & "RoleId: " & Me.Role.Id
                lsMessage &= vbCrLf & ". FactSymbol(Id): " & Me.Fact.Symbol
                lsMessage &= vbCrLf & ". Value: " & Me.Data
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub


    End Class

End Namespace
