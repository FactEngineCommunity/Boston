Imports System.Xml.Serialization

Namespace FBM

    <Serializable()> _
    Public Class ValueType
        Inherits Viev.FBM.ValueType

        Public Overrides Function RemoveFromModel(Optional ByVal abForceRemoval As Boolean = False, _
                                                  Optional ByVal abCheckForErrors As Boolean = True) As Boolean

            Dim lsMessage As String

            Try
                RemoveFromModel = True

                If Me.ExistsRolesAssociatedWithValueType Then
                    lsMessage = "You cannot remove Value Type, '" & Trim(Me.Name) & "' while there are Fact Types/Roles within the Model associated with the Value Type."
                    MsgBox(lsMessage)

                    Return False
                    Exit Function
                End If

                Dim lrModelDictionaryEntry As New FBM.DictionaryEntry(Me.Model, Me.Id, pcenumConceptType.ValueType)
                lrModelDictionaryEntry = Me.Model.ModelDictionary.Find(AddressOf lrModelDictionaryEntry.Equals)


                Call TableValueType.DeleteValueType(Me)
                Me.Model.RemoveValueType(Me)

                RaiseEvent RemovedFromModel()

                Return True

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                Me.Model.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace, True)
            End Try

        End Function

        Public Overloads Overrides Sub Save()

            '-------------------------------------
            'Saves the ValueType to the database
            '-------------------------------------

            Try
                Dim lrDictionaryEntry As New FBM.DictionaryEntry(Me.Model, Me.Id, pcenumConceptType.ValueType, Me.ShortDescription, Me.LongDescription)
                lrDictionaryEntry = Me.Model.AddModelDictionaryEntry(lrDictionaryEntry)
                lrDictionaryEntry.isValueType = True

                If TableValueType.ExistsValueType(Me) Then
                    Call lrDictionaryEntry.Save()
                    Call TableValueType.UpdateValueType(Me)
                Else
                    Try
                        pdbConnection.BeginTrans()
                        Call lrDictionaryEntry.Save()
                        pdbConnection.CommitTrans()
                        If TableValueType.ExistsValueType(Me) Then
                            Call TableValueType.UpdateValueType(Me)
                        Else
                            Call TableValueType.AddValueType(Me)
                        End If
                    Catch ar_err As Exception
                        MsgBox("Error: tValueType.Save: " & ar_err.Message & ": ValueTypeId: " & Me.Id)
                        pdbConnection.RollbackTrans()
                    End Try
                End If

                '-----------------------------------------------------
                'Save the ValueConstraints (if any) for the ValueType
                '-----------------------------------------------------
                Dim lrValue As String

                For Each lrValue In Me._ValueConstraintList
                    Dim lrConcept As New FBM.Concept(lrValue)
                    If TableValueTypeValueConstraint.ExistsValueTypeValueConstraint(Me, lrConcept) Then
                        '----------------------------------------------------------------------------
                        'ValueConstraintValue already exists for this ValueType within the database
                        '----------------------------------------------------------------------------
                    Else
                        Call TableValueTypeValueConstraint.AddValueTypeValueConstraint(Me, lrConcept)
                    End If

                    'Removes ValueConstraintValues (from the database) that are no longer associated with this ValueType
                    Call TableValueTypeValueConstraint.remove_unneeded_value_constraints(Me)
                Next

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub



        Public Overrides Sub SetName(ByVal asNewName As String)

            '-----------------------------------------------------------------------------------------------------------------
            'The following explains the logic and philosophy of Richmond.
            '  A ValueType.Id/Name represents the same thing accross all Models in Richmond, otherwise the Richmond 
            '  user would have a different ValueType.Id/Name for the differing Concepts (not excluding that in Richmond
            '  a FactType in one Model can have a wildly different RoleGroup (ModelObject associations) than the same
            '  named FactType in another Model).
            '  So, for example, 'Person' is the same Concept accross all Models.
            '  NB A Concept.Symbol/ConceptType combination is unique to a Model except for ConceptTypes, Fact and Value,
            '  where the Symbol of a Fact of one FactType may be a Value of FactData of Facts of another FactType.
            '  NB Concept.Symbols must be unique in a Model amoungst EntityTypes, ValueTypes, FactTypes and RoleConstraints.
            '-----------------------------------------------------------------------------------------------------------------
            '-----------------------------------------------------------
            'Set the name and Symbol of the ValueType to the new Value.
            '  The Id of the ValueType is modified later in this Set.
            '-----------------------------------------------------------
            Try
                _Name = asNewName
                Me.Symbol = asNewName

                '--------------------------------------------------------------------------------------------------------
                'The surrogate key for the ValueType is about to change (to match the name of the ValueType)
                '  so update the ModelDictionary entry for the Concept/Symbol (the nominalistic idenity of the VactType
                '--------------------------------------------------------------------------------------------------------
                If StrComp(Me.Id, asNewName) <> 0 Then
                    '------------------------------------------------------
                    'The new ValueType.Name does not match the ValueType.Id
                    '------------------------------------------------------

                    Call Me.SwitchConcept(New FBM.Concept(asNewName))
                    'Dim lrDictionaryEntry As New FBM.DictionaryEntry(Me.Model, Me.Id, pcenumConceptType.ValueType)

                    'If Me.Model.ModelDictionary.Exists(AddressOf lrDictionaryEntry.EqualsBySymbol) Then
                    '    Call Me.Model.UpdateDictionarySymbol(Me.Id, asNewName, pcenumConceptType.ValueType)
                    '    Call TableModelDictionary.ModifySymbol(Me.Model, lrDictionaryEntry, asNewName, pcenumConceptType.ValueType)
                    'Else
                    '    Dim lsMessage As String = ""
                    '    lsMessage = "Tried to modify the Name of a ValueType where no Dictionary Entry exists for that ValueType."
                    '    lsMessage &= vbCrLf & "Original DictionaryEntry.Symbol: " & Me.Id.ToString
                    '    lsMessage &= vbCrLf & "New DictionaryEntry.Symbol: " & asNewName
                    '    Throw New System.Exception(lsMessage)
                    'End If

                    '------------------------------------------------------------------------------------------
                    'Update the Model(database) immediately. There is no choice. The reason why
                    '  is because the (in-memory) key is changing, so the database must be updated to 
                    '  reflect the new key, otherwise it will not be possible to Update an existing ValueType.
                    '------------------------------------------------------------------------------------------
                    Call TableValueType.ModifyKey(Me, asNewName)
                    Me.Id = asNewName
                    Call TableValueType.UpdateValueType(Me) 'Sets the new Name

                    '-------------------------------------------------------
                    'Update all of the respective ValueConstraint records
                    '  in the database as well
                    '-------------------------------------------------------
                    Call TableValueTypeValueConstraint.ModifyKey(Me, asNewName)

                    Me.Model.MakeDirty()

                    RaiseEvent updated()
                    RaiseEvent NameChanged()

                    '------------------------------------------------------------------------------------
                    'Must save the Model because Roles that reference the ValueType must be saved.
                    '  NB If Roles are saved, the FactType must be saved. If the FactType is saved,
                    '  the RoleGroup's references (per Role) must be saved. A Role within the RoleGroup 
                    '  may reference another FactType, so that FactType must be saved...etc.
                    '  i.e. It's easier and safer to simply save the whole model.
                    '------------------------------------------------------------------------------------
                    Me.Model.Save()

                End If 'Me.Id <> asNewName
            Catch ex As Exception
                Dim lsMessage As String
                lsMessage = "Error: tValueType.SetName"
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                Me.Model.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace, True)
            End Try

        End Sub


    End Class

End Namespace
