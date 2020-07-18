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
        <XmlIgnore()>
        Public Role As FBM.Role 'New

        ''' <summary>
        ''' The Data stored for the Role for the Fact.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Data() As String
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
                'Dim lsMessage As String

                Try

                    lsOriginalSymbol = Me.Concept.Symbol
                    'Call prApplication.ThrowErrorMessage("....This is the fact being modified..." & Me.Fact.Symbol, pcenumErrorType.Information)

                    If lsOriginalSymbol = value Then Exit Property 'Nothing to do here

                    lrNewDictionaryEntry = New FBM.DictionaryEntry(Me.Model, value, pcenumConceptType.Value)
                    lrNewDictionaryEntry.isDirty = True

                    If Me.Model.ModelDictionary.Find(AddressOf lrNewDictionaryEntry.Equals) IsNot Nothing Then
                        '----------------------------------------------------------------------------
                        'The NewConcept exists in the ModelDictionary
                        '  Substitute the existing Concept for a ModelDictionary entry (Concept) that
                        '  already exists in the Model. i.e. Switch Concepts.
                        '----------------------------------------------------------------------------                     
                        ''lrOriginalDictionaryEntry = Me.Model.ModelDictionary.Find(AddressOf lrOriginalDictionaryEntry.Equals)

                        ''If lrOriginalDictionaryEntry IsNot Nothing Then
                        ''    If (lrOriginalDictionaryEntry.Concept.Symbol = lrNewDictionaryEntry.Concept.Symbol) Then
                        ''        '-------------------------------------------------------------------------------------------------------------------
                        ''        'Don't worry about removing the OriginalDictionaryEntry as the FactData.Data/Concept was set to its original value
                        ''        '-------------------------------------------------------------------------------------------------------------------
                        ''    Else
                        ''        If lrOriginalDictionaryEntry.Realisations.Count <= 1 Then
                        ''            '--------------------------------------------------------------
                        ''            'Remove the original DictionaryEnty if it is no longer needed
                        ''            '--------------------------------------------------------------
                        ''            Me.Model.RemoveDictionaryEntry(lrOriginalDictionaryEntry, True)
                        ''        Else
                        ''            Call Me.Model.DeprecateRealisationsForDictionaryEntry(lrOriginalDictionaryEntry)
                        ''        End If
                        ''    End If
                        ''Else
                        ''    '--------------------------------------------------------------------------------------------------------------
                        ''    'Throw a warning message but do not interupt programme flow. 
                        ''    ' We were going to remove the OriginalDictionaryEntry anyway.
                        ''    '--------------------------------------------------------------------------------------------------------------
                        ''    lsMessage = "Original DictionaryEntry for FactData.Concept not found in the ModelDictionary"
                        ''    Call prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Warning)
                        ''End If

                        ''Me.Concept = lrNewDictionaryEntry.Concept

                        ''lrNewDictionaryEntry.Realisations.Add(lrNewDictionaryEntry.Concept)
                        ''lrNewDictionaryEntry.AddConceptType(pcenumConceptType.Value)

                        ''RaiseEvent ConceptSwitched(Me.Concept)
                        If Me.Model.Loaded Then Call Me.makeDirty()
                        Call Me.SwitchConcept(lrNewDictionaryEntry.Concept, pcenumConceptType.Value)

                        'lsDebugMessage = "FactData.Data.Set"
                        'If IsSomething(Me.FactType) Then
                        '    lsDebugMessage &= vbCrLf & "FactType.Id: " & Me.FactType.Id
                        'Else
                        '    lsDebugMessage &= vbCrLf & "FactType.Id: Nothing"
                        'End If
                        'If IsSomething(Me.Fact) Then
                        '    lsDebugMessage &= vbCrLf & "Fact.Symbol: " & Me.Fact.Symbol
                        'Else
                        '    lsDebugMessage &= vbCrLf & "Fact.Symbol: Nothing"
                        'End If
                        'lsDebugMessage &= vbCrLf & "Role.Id: " & Me.Role.Id
                        'lsDebugMessage &= vbCrLf & "Original Data/Concept.Symbol: " & Me.Data
                        'lsDebugMessage &= vbCrLf & "New Data/Concept.Symbol: '" & lrNewDictionaryEntry.Symbol & "' already exists in the ModelDictionary"
                        'Call prApplication.ThrowErrorMessage(lsDebugMessage, pcenumErrorType.Information)

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
                            Me.Model.DeprecateRealisationsForDictionaryEntry(lrOriginalDictionaryEntry, pcenumConceptType.Value)
                        End If

                        Me.Model.AddModelDictionaryEntry(lrNewDictionaryEntry)

                        Me.Concept = lrNewDictionaryEntry.Concept                        
                        Call lrNewDictionaryEntry.Concept.Save()
                        lrNewDictionaryEntry.Save()

                        Me.makeDirty()

                        RaiseEvent ConceptSymbolUpdated()

                        lsDebugMessage = "Setting FactData.Concept.Symbol to new Concep/DictionaryEntry: " & value
                        'Call prApplication.ThrowErrorMessage(lsDebugMessage, pcenumErrorType.Information)
                    End If

                    Call Me.Model.MakeDirty(False, False)

                Catch ex As Exception
                    Dim lsMessage1 As String
                    Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                    lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                    lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                    prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
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
        'Public Shadows Event ConceptSwitched(ByRef arConcept As FBM.Concept)
        Public Event ModelErrorAdded(ByRef arModelError As ModelError) Implements iValidationErrorHandler.ModelErrorAdded
        Public Event RemovedFromModel(ByVal abDeleteAll As Boolean)

        Public Sub New()
            '------------------------------------
            'Default Parameterless Constructor
            '------------------------------------
            Me.ConceptType = pcenumConceptType.RoleData
        End Sub

        Public Sub New(ByRef arRole As FBM.Role, ByRef arConcept As FBM.Concept, Optional ByRef arFact As FBM.Fact = Nothing)

            Me.ConceptType = pcenumConceptType.RoleData
            Me.Model = arRole.Model
            Me.Role = arRole
            If IsSomething(arFact) Then
                Me.Fact = arFact
                Me.FactType = arFact.FactType
            End If

            Me.Concept = arConcept
            Me.Symbol = arConcept.Symbol


        End Sub

        ''' <summary>
        ''' Returns a Clone of the FactData.
        ''' </summary>
        ''' <param name="arFact"></param>
        ''' <returns></returns>
        ''' <remarks>Needs to be managed carefully because does not clone the Role</remarks>
        Public Overloads Function Clone(Optional ByRef arFact As FBM.Fact = Nothing,
                                        Optional ByVal abSetData As Boolean = False) As FBM.FactData

            Dim lrFactData As New FBM.FactData

            With Me
                lrFactData.Model = .Model
                lrFactData.Id = .Id
                If abSetData Then
                Else
                    lrFactData.Data = .Data
                End If
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
                lrFactData.isDirty = True

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

        Public Function EqualsByRoleNameData(ByVal other As FBM.FactData) As Boolean

            Select Case other.GetType.ToString
                Case Is = GetType(FBM.FactData).ToString
                    If (Me.Role.Name = other.Role.Name) And (Me.Concept.Symbol = other.Concept.Symbol) Then
                        Return True
                    Else
                        Return False
                    End If
                Case Is = GetType(FBM.FactDataInstance).ToString
                    Dim lrFactDataInstance As FBM.FactDataInstance = other
                    If (Me.Role.Name = lrFactDataInstance.Role.Name) And (Me.Concept.Symbol = lrFactDataInstance.Concept.Symbol) Then
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
                lrFactData.Concept = New FBM.Concept
                With Me
                    'Concept - See lrFactData.Data below
                    lrFactData.Model = arModel
                    lrFactData.ConceptType = .ConceptType
                    If IsSomething(arFactType) Then
                        lrFactData.FactType = arFactType
                    End If

                    If IsSomething(arFact) Then
                        lrFactData.Fact = arFact
                    End If

                    lrFactData.Data = .Data 'NB Sets the 'Concept' attribute

                    Dim lrDictionaryEntry As New FBM.DictionaryEntry(arModel, lrFactData.Data, pcenumConceptType.Value)
                    Call arModel.AddModelDictionaryEntry(lrDictionaryEntry)

                    lrFactData.Role = arFactType.RoleGroup.Find(AddressOf .Role.Equals)
                    lrFactData.Symbol = .Symbol
                End With

                Return lrFactData
            Catch ex As Exception
                Dim lsMessage As String = ""

                lsMessage = "Error: tRoleData.Clone: " & vbCrLf & vbCrLf & ex.Message
                Call prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)

                Return lrFactData
            End Try

        End Function

        ''' <summary>
        ''' Creates an (PageObject) Instance of a FactData object.
        ''' </summary>
        ''' <param name="arPage">The Page onto which the FactDataInstance will be allocated.</param>
        ''' <param name="arFactInstance">The FactInstance for the FactDataInstance. Provide if adding directly to the FactInstance when FactTypeInstance.Fact does not contain the FactInstance</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shadows Function CloneInstance(ByRef arPage As FBM.Page,
                                              Optional ByRef arFactInstance As FBM.FactInstance = Nothing,
                                              Optional ByVal abMakeFactDataDirty As Boolean = False) As FBM.FactDataInstance

            Dim lrFactDataInstance As New FBM.FactDataInstance
            Dim lrFactTypeInstance As FBM.FactTypeInstance

            Try
                With Me
                    lrFactDataInstance.Model = arPage.Model
                    lrFactDataInstance.Page = arPage
                    lrFactDataInstance.FactData = Me
                    lrFactDataInstance.Data = .Data
                    lrFactDataInstance.Symbol = .Symbol

                    Select Case Me.Role.TypeOfJoin
                        Case Is = pcenumRoleJoinType.EntityType
                            lrFactDataInstance.JoinedObjectType = arPage.EntityTypeInstance.Find(Function(x) x.Id = .Role.JoinsEntityType.Id)
                        Case Is = pcenumRoleJoinType.ValueType
                            lrFactDataInstance.JoinedObjectType = arPage.ValueTypeInstance.Find(Function(x) x.Id = .Role.JoinsValueType.Id)
                        Case Is = pcenumRoleJoinType.FactType
                            lrFactDataInstance.JoinedObjectType = arPage.FactTypeInstance.Find(Function(x) x.Id = .Role.JoinsFactType.Id)
                    End Select

                    lrFactDataInstance.Concept = .Concept

                    '----------------------------------------
                    'Find the RoleInstance for the FactData
                    '----------------------------------------
                    lrFactDataInstance.Role = arPage.RoleInstance.Find(Function(x) x.Id = Me.Role.Id)
                    'CodeSafe: Add the Clone and add the RoleInstance to the Page if it does not exist
                    If lrFactDataInstance.Role.Data.FindAll(Function(x) x.Role Is Nothing).Count > 0 Then
                        lrFactDataInstance.Role = Me.Role.CloneInstance(arPage, True)
                    End If

                    '--------------------------------------------
                    'Find the FactTypeInstance for the FactData
                    '--------------------------------------------
                    lrFactTypeInstance = arPage.FactTypeInstance.Find(Function(x) x.Id = .Role.FactType.Id)

                    If IsSomething(arFactInstance) Then
                        lrFactDataInstance.Fact = arFactInstance
                    Else
                        '--------------------------------
                        'Find the Fact for the FactData
                        '--------------------------------
                        lrFactDataInstance.Fact = lrFactTypeInstance.Fact.Find(Function(x) x.Id = .Fact.Id)
                    End If

                End With

                Return lrFactDataInstance

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)

                Return Nothing
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

        Public Overrides Sub makeDirty()
            Try
                Me.isDirty = True
                Me.Fact.isDirty = True
                Me.Fact.FactType.isDirty = True
            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
            End Try
        End Sub

        Public Shadows Function RemoveFromModel(Optional ByVal abForceRemoval As Boolean = False,
                                                  Optional ByVal abCheckForErrors As Boolean = True,
                                                  Optional ByVal abDoDatabaseProcessing As Boolean = True,
                                                  Optional ByVal abDeleteAll As Boolean = False) As Boolean '(ByRef arError As FBM.ModelError) As Boolean

            Me.Fact.Data.Remove(Me)

            If abDoDatabaseProcessing Then
                TableFactData.DeleteFactData(Me)
            End If

            Dim lrDictionaryEntry As New FBM.DictionaryEntry(Me.Model, Me.Data, pcenumConceptType.Value)
            Call Me.Model.DeprecateRealisationsForDictionaryEntry(lrDictionaryEntry, pcenumConceptType.Value)

            RaiseEvent RemovedFromModel(abDeleteAll)

        End Function

        Public Shadows Sub Save(Optional ByVal abRapidSave As Boolean = False)

            Dim lsMessage As String

            Try
                If abRapidSave Then
                    '------------------------------------------------------
                    'CodeSafe: If no dictionary entry exists, create one.
                    '------------------------------------------------------
                    Dim lrDictionaryEntry As New FBM.DictionaryEntry(Me.Model, Me.Data, pcenumConceptType.Value)
                    lrDictionaryEntry = Me.Model.AddModelDictionaryEntry(lrDictionaryEntry)
                    lrDictionaryEntry.isValue = True
                    Call lrDictionaryEntry.Save()

                    TableFactData.AddFactData(Me)

                ElseIf Me.isDirty Then
                    If TableFactData.ExistsFactData(Me) Then
                        TableFactData.UpdateFactData(Me)
                    Else
                        '------------------------------------------------------
                        'CodeSafe: If no dictionary entry exists, create one.
                        '------------------------------------------------------
                        Dim lrDictionaryEntry As New FBM.DictionaryEntry(Me.Model, Me.Data, pcenumConceptType.Value)
                        lrDictionaryEntry = Me.Model.AddModelDictionaryEntry(lrDictionaryEntry)
                        lrDictionaryEntry.isValue = True
                        Call lrDictionaryEntry.Save()

                        TableFactData.AddFactData(Me)
                    End If

                    Me.isDirty = False
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

        ''' <summary>
        ''' Sets the Concept.Symbol for the FactData.
        ''' Preconditions: The ModelDictionary is already loaded for the FactData.Model and the FactData.Concept is already linked to a DictionaryEntry in the ModelDitionary (link to DictionaryEntry.Concept).
        ''' Postconditions: The FactData.Concept is either updated to a new Symbol or switched to a new DictionaryEntry.Concept.
        ''' </summary>
        ''' <param name="value"></param>
        ''' <remarks>NB Initial setting of the FactData.Concept.Symbol should be done via the Set method of FactData.Data</remarks>
        Public Sub setData(ByVal value As String, ByVal aiConceptType As pcenumConceptType, ByVal abAddToModelDictionary As Boolean)

            Me.Concept = New FBM.Concept(value)

            If abAddToModelDictionary Then
                Dim lrDictionaryEntry = New FBM.DictionaryEntry(Me.Model, value, aiConceptType)
                Call Me.Model.AddModelDictionaryEntry(lrDictionaryEntry)
            End If

        End Sub

        'Public Sub SwitchConcept(ByVal asNewInstance As String)

        '    Dim lrNewDictionaryEntry As New FBM.DictionaryEntry(Me.Model, asNewInstance, pcenumConceptType.Value)
        '    Dim lsDebugMessage As String = ""

        '    Try

        '        Call prApplication.ThrowErrorMessage("....This is the fact being modified..." & Me.Fact.Symbol, pcenumErrorType.Information)

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
        '            Call prApplication.ThrowErrorMessage(lsDebugMessage, pcenumErrorType.Information)

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
        '            Call prApplication.ThrowErrorMessage(lsDebugMessage, pcenumErrorType.Information)
        '        End If
        '        RaiseEvent updated()
        '        Call Me.Model.MakeDirty()

        '    Catch ex As Exception
        '        Dim lsMessage As String
        '        lsMessage = "Error: FBM.tFactData.SwitchConcept"
        '        lsMessage &= vbCrLf & vbCrLf & ex.Message
        '        prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        '    End Try

        'End Sub

        Private Sub update_from_concept() Handles Concept.ConceptSymbolUpdated
            Try
                RaiseEvent ConceptSymbolUpdated()
                Me.makeDirty()
            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
            End Try
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