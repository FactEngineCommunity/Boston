Imports System.ComponentModel
Imports System.Xml.Serialization
Imports System.Reflection

Namespace FBM
    <Serializable()> _
    Public Class FactData
        Inherits FBM.ModelObject
        Implements IEquatable(Of FBM.FactData)
        Implements ICloneable
        Implements IXmlSerializable
        Implements FBM.iValidationErrorHandler

        <XmlAttribute()> _
        Public Shadows ConceptType As pcenumConceptType = pcenumConceptType.RoleData

        ''' <summary>
        ''' The FactType of the Fact to which the RoleData belongs.
        ''' </summary>
        ''' <remarks></remarks>
        <XmlIgnore()> _
        Public FactType As FBM.FactType

        Public Overrides Property Model() As Model
            Get
                Return MyBase.Model
            End Get
            Set(ByVal value As Model)
                MyBase.Model = value
            End Set
        End Property

        ''' <summary>
        ''' The Fact to which the RoleData belongs.
        ''' </summary>
        ''' <remarks></remarks>
        <XmlIgnore()> _
        Public WithEvents Fact As New FBM.Fact 'The Fact within which this RoleData represents a Value. e.g. in Fact P={a,b,c} this RoleData may represent the value, b.

        ''' <summary>
        ''' The Role to which the 'Concept' (Value) is related within the Fact.
        ''' </summary>
        ''' <remarks></remarks>
        <XmlIgnore()> _
        Public Role As New FBM.Role

        ''' <summary>
        ''' The Data stored for the Role for the Fact.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overridable Property Data() As String
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

        Private _ModelError As New List(Of FBM.ModelError)
        Public Property ModelError() As System.Collections.Generic.List(Of ModelError) Implements iValidationErrorHandler.ModelError
            Get
                Return Me._ModelError
            End Get
            Set(ByVal value As System.Collections.Generic.List(Of ModelError))
                Me._ModelError = value
            End Set
        End Property


        Public Shadows Event ConceptSymbolUpdated()
        Public Event ConceptSwitched(ByRef arConcept As FBM.Concept)
        Public Event ModelErrorAdded(ByRef arModelError As ModelError) Implements iValidationErrorHandler.ModelErrorAdded

        Public Sub New()
            '------------------------------------
            'Default Parameterless Constructor
            '------------------------------------
        End Sub

        Public Sub New(ByRef arRole As FBM.Role, ByRef arConcept As FBM.Concept, Optional ByRef arFact As FBM.Fact = Nothing)

            Me.Model = arRole.Model
            Me.Role = arRole
            If IsSomething(arFact) Then
                Me.Fact = arFact
                Me.FactType = arFact.FactType
            End If

            Me.Concept = arConcept


        End Sub

        ''' <summary>
        ''' Returns a Clone of the FactData.
        ''' </summary>
        ''' <param name="arFact"></param>
        ''' <returns></returns>
        ''' <remarks>Needs to be managed carefully because does not clone the Role</remarks>
        Public Overloads Function Clone(Optional ByRef arFact As FBM.Fact = Nothing) As FBM.FactData

            Dim lrFactData As New FBM.FactData

            With Me
                lrFactData.Model = .Model
                lrFactData.Id = .Id
                lrFactData.Data = .Data
                lrFactData.FactType = .FactType
                If IsSomething(arFact) Then
                    lrFactData.Fact = arFact
                End If
                lrFactData.Concept = .Concept
                lrFactData.Name = .Name
                lrFactData.Symbol = .Symbol
                lrFactData.Role = .Role
                lrFactData.ShortDescription = .ShortDescription
                lrFactData.LongDescription = .LongDescription

            End With

            Return lrFactData

        End Function

        Public Shadows Function Equals(ByVal other As FBM.FactData) As Boolean Implements System.IEquatable(Of FBM.FactData).Equals

            If (Me.Fact.Id = other.Fact.Id) And (Me.Role.Id = other.Role.Id) Then 'And (Me.Concept.Symbol = other.Concept.Symbol) Then
                Return True
            Else
                Return False
            End If

        End Function

        Public Shadows Function EqualsByData(ByVal other As FBM.FactData) As Boolean

            If Me.Data = other.Data Then
                Return True
            Else
                Return False
            End If

        End Function

        Public Shadows Function EqualsByRoleIdData(ByVal other As FBM.FactData) As Boolean

            If (Me.Role.Id = other.Role.Id) And (Me.Data = other.Data) Then
                Return True
            Else
                Return False
            End If

        End Function

        Public Overridable Function EqualsByRoleNameData(ByVal other As FBM.FactData) As Boolean

            Select Case other.GetType.ToString
                Case Is = GetType(FBM.FactData).ToString
                    If (Me.Role.Name = other.Role.Name) And (Me.Concept.Symbol = other.Concept.Symbol) Then
                        Return True
                    Else
                        Return False
                    End If
            End Select


        End Function

        Public Function EqualsByRole(ByVal other As FBM.FactData) As Boolean

            If (Me.Role.Id = other.Role.Id) Then
                Return True
            Else
                Return False
            End If

        End Function

        ''' <summary>
        ''' Clones the RoleData struct. 
        ''' PRECONDITIONS: The ModelDictionary for the Model must be prepopulated.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overloads Function Clone(ByRef arModel As FBM.Model, ByRef arFact As FBM.Fact, ByRef arFactType As FBM.FactType) As Object

            Dim lrFactData As New FBM.FactData

            Try
                With Me
                    'Concept - See lrFactData.Data below
                    lrFactData.Model = arModel
                    lrFactData.ConceptType = .ConceptType
                    lrFactData.Data = .Data 'NB Sets the 'Concept' attribute

                    Dim lrDictionaryEntry As New FBM.DictionaryEntry(arModel, lrFactData.Data, pcenumConceptType.Value)
                    Call arModel.AddModelDictionaryEntry(lrDictionaryEntry)

                    If IsSomething(arFact) Then
                        lrFactData.Fact = arFact
                    End If
                    If IsSomething(arFactType) Then
                        lrFactData.FactType = arFactType
                    End If
                    lrFactData.Role = arFactType.RoleGroup.Find(AddressOf .Role.Equals)
                    lrFactData.Symbol = .Symbol
                End With

                Return lrFactData
            Catch ex As Exception
                Dim lsMessage As String = ""

                lsMessage = "Error: tRoleData.Clone: " & vbCrLf & vbCrLf & ex.Message
                Call Me.Model.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace, True)

                Return lrFactData
            End Try

        End Function


        Public Shared Function compare_role_SequenceNr(ByVal ao_a As FBM.FactData, ByVal ao_b As FBM.FactData) As Integer

            '------------------------------------------------------
            'Used as a delegate within 'sort'
            '------------------------------------------------------        
            Return ao_a.Role.SequenceNr - ao_b.Role.SequenceNr

        End Function

        Public Overrides Function CanSafelyRemoveFromModel() As Boolean

            Return False
        End Function

        Public Sub ClearModelErrors() Implements iValidationErrorHandler.ClearModelErrors
            Me.ModelError.Clear()
        End Sub

        Public ReadOnly Property HasModelError() As Boolean Implements iValidationErrorHandler.HasModelError
            Get
                Return Me.ModelError.Count > 0
            End Get
        End Property

        Public Overrides Function RemoveFromModel(Optional ByVal abForceRemoval As Boolean = False, _
                                                  Optional ByVal abCheckForErrors As Boolean = True) As Boolean '(ByRef arError As FBM.ModelError) As Boolean

            Me.Fact.Data.Remove(Me)

            Dim lrDictionaryEntry As New FBM.DictionaryEntry(Me.Model, Me.Data, pcenumConceptType.Value)
            Call Me.Model.DeprecateRealisationsForDictionaryEntry(lrDictionaryEntry)

        End Function

        ''' <summary>
        ''' Prototype for the Save function.
        ''' </summary>
        ''' <remarks></remarks>
        Public Overridable Shadows Sub Save()
        End Sub

        ''' <summary>
        ''' Sets the Concept.Symbol for the FactData.
        ''' Preconditions: The ModelDictionary is already loaded for the FactData.Model and the FactData.Concept is already linked to a DictionaryEntry in the ModelDitionary (link to DictionaryEntry.Concept).
        ''' Postconditions: The FactData.Concept is either updated to a new Symbol or switched to a new DictionaryEntry.Concept.
        ''' </summary>
        ''' <param name="value"></param>
        ''' <remarks>NB Initial setting of the FactData.Concept.Symbol should be done via the Set method of FactData.Data</remarks>
        Public Sub SetData(ByVal value As String)



        End Sub

        'Public Sub SwitchConcept(ByVal asNewInstance As String)

        '    Dim lrNewDictionaryEntry As New FBM.DictionaryEntry(Me.Model, asNewInstance, pcenumConceptType.Value)
        '    Dim lsDebugMessage As String = ""

        '    Try

        '        Call Me.Model.ThrowErrorMessage("....This is the fact being modified..." & Me.Fact.Symbol, pcenumErrorType.Information)

        '        '---------------------------------------------------------------------------------------------------------------------------------
        '        'Code Safe: If the new Concept (being switched to) already exists in the Model, then switch the Concept to that DictionaryEntry.
        '        '---------------------------------------------------------------------------------------------------------------------------------

        '        lrNewDictionaryEntry = Me.Model.ModelDictionary.Find(AddressOf lrNewDictionaryEntry.Equals)

        '        If IsSomething(lrNewDictionaryEntry) Then
        '            '----------------------------------------------------------------------------
        '            'The NewConcept exists in the ModelDictionary
        '            '  Substitute the existing Concept for a ModelDictionary entry (Concept) that
        '            '  already exists in the Model
        '            '----------------------------------------------------------------------------
        '            Me.Concept = lrNewDictionaryEntry.Concept

        '            lsDebugMessage = "FactData.Data.Set"
        '            If IsSomething(Me.FactType) Then
        '                lsDebugMessage &= vbCrLf & "FactType.Id: " & Me.FactType.Id
        '            Else
        '                lsDebugMessage &= vbCrLf & "FactType.Id: Nothing"
        '            End If
        '            If IsSomething(Me.Fact) Then
        '                lsDebugMessage &= vbCrLf & "Fact.Symbol: " & Me.Fact.Symbol
        '            Else
        '                lsDebugMessage &= vbCrLf & "Fact.Symbol: Nothing"
        '            End If
        '            lsDebugMessage &= vbCrLf & "Role.Id: " & Me.Role.Id
        '            lsDebugMessage &= vbCrLf & "Original Data/Concept.Symbol: " & Me.Data
        '            lsDebugMessage &= vbCrLf & "New Data/Concept.Symbol: '" & lrNewDictionaryEntry.Symbol & "' already exists in the ModelDictionary"
        '            Call Me.Model.ThrowErrorMessage(lsDebugMessage, pcenumErrorType.Information)

        '        Else
        '            '-------------------------------------------------------
        '            'The NewConcept does not exist in the ModelDictionary.
        '            '  Modify the existing Concept to a new Concept
        '            '-------------------------------------------------------
        '            '-------------------------------------------------
        '            'Create a new DictionaryEntry for the NewConcept
        '            '-------------------------------------------------
        '            Dim lrDictionaryEntry As New FBM.DictionaryEntry(Me.Model, asNewInstance, pcenumConceptType.Value)
        '            Me.Model.AddModelDictionaryEntry(lrDictionaryEntry)

        '            Me.Concept = lrDictionaryEntry.Concept

        '            lsDebugMessage = "Setting FactData.Concept.Symbol to new Concept/DictionaryEntry: " & asNewInstance
        '            Call Me.Model.ThrowErrorMessage(lsDebugMessage, pcenumErrorType.Information)
        '        End If
        '        RaiseEvent updated()
        '        Call Me.Model.MakeDirty()

        '    Catch ex As Exception
        '        Dim lsMessage As String
        '        lsMessage = "Error: FBM.tFactData.SwitchConcept"
        '        lsMessage &= vbCrLf & vbCrLf & ex.Message
        '        Me.Model.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace, True)
        '    End Try

        'End Sub

        Private Sub update_from_concept() Handles Concept.ConceptSymbolUpdated
            RaiseEvent ConceptSymbolUpdated()
        End Sub

        Public Function GetSchema() As System.Xml.Schema.XmlSchema Implements System.Xml.Serialization.IXmlSerializable.GetSchema
            Return Nothing
        End Function

        Public Sub ReadXml(ByVal reader As System.Xml.XmlReader) Implements System.Xml.Serialization.IXmlSerializable.ReadXml

        End Sub

        Public Sub WriteXml(ByVal writer As System.Xml.XmlWriter) Implements System.Xml.Serialization.IXmlSerializable.WriteXml

            writer.WriteAttributeString("ConceptType", Me.ConceptType.ToString)
            writer.WriteElementString("RoleId", Me.Role.Id)
            writer.WriteElementString("Value", Me.Data)
        End Sub

        Public Sub AddModelError(ByRef arModelError As ModelError) Implements iValidationErrorHandler.AddModelError

            Me.ModelError.Add(arModelError)
            RaiseEvent ModelErrorAdded(arModelError)

        End Sub

    End Class
End Namespace