Imports System.Xml.Serialization
Imports System.ComponentModel
Imports System.Reflection

Namespace FBM

    '-----------------------------------------------------------------------------------------------------
    'Everything that can be displayed on the screen is a 'Symbol', even if it is an ASCII text character
    '  or logic 'Symbol'
    '-----------------------------------------------------------------------------------------------------
    <Serializable()> _
    Public Class DictionaryEntry
        Implements IEquatable(Of DictionaryEntry)
        Implements IComparer(Of DictionaryEntry)
        Implements Relational.iObjectRelationalMap(Of DictionaryEntry)

        '--------------------------------------------------------------------------------------------------------------------------
        'This class is predominantly used to store instance (usage) of a Symbol within a diagram (e.g. ORMDiagram, UseCaseDiagram).
        '  e.g. If an EntityType is used within an ORM Diagram, this class is used to reflect within which diagram and at which coordinate
        '  the EntityType was displayed within the diagram.
        '--------------------------------------------------------------------------------------------------------------------------

        <XmlIgnore()>
        Public Model As FBM.Model

        ''' <summary>
        ''' Me.Symbol cannot be linked to for WithEvents in FactData instances, so need an instance of a Concept from which 'Public WithEvents Concept as FBM.Concept' can be declared in FactData/Instance
        ''' </summary>
        ''' <remarks></remarks>
        <XmlIgnore()>
        Public WithEvents Concept As FBM.Concept

        <XmlIgnore()>
        Public Realisations As New List(Of pcenumConceptType) '(Of FBM.Concept)

        ''' <summary>
        ''' See Me.EqualsByOtherConceptType
        ''' Only used to check if a DictionaryEntry has other ConceptTypes than the one specified.
        ''' Used to check if a DictionaryEnty should be removed from the Model.ModelDictionary and the database.         
        ''' </summary>
        ''' <remarks></remarks>
        <XmlIgnore()>
        Public ConceptType As pcenumConceptType

        <XmlAttribute()>
        Public Symbol As String = ""

        <CategoryAttribute("Description (Informal)"),
                [ReadOnly](True)>
        Public ReadOnly Property Term As String
            Get
                Return Me.Symbol
            End Get
        End Property

        <XmlIgnore()>
        Public _ShortDescription As String = ""
        <XmlElement()>
        <CategoryAttribute("Description (Informal)"),
             Browsable(True),
             [ReadOnly](False),
             BindableAttribute(True),
             DefaultValueAttribute(""),
             DesignOnly(False),
             DescriptionAttribute("Enter a description."),
             Editor(GetType(System.ComponentModel.Design.MultilineStringEditor), GetType(System.Drawing.Design.UITypeEditor))>
        Public Property ShortDescription As String
            Get
                Return Me._ShortDescription
            End Get
            Set(ByVal value As String)
                Me._ShortDescription = value
            End Set
        End Property

        <XmlIgnore()>
        Public _LongDescription As String = ""
        <XmlElement()>
        <CategoryAttribute("Description (Informal)"),
             Browsable(True),
             [ReadOnly](False),
             BindableAttribute(True),
             DefaultValueAttribute(""),
             DesignOnly(False),
             DescriptionAttribute("Enter a description."),
             Editor(GetType(System.ComponentModel.Design.MultilineStringEditor), GetType(System.Drawing.Design.UITypeEditor))>
        Public Property LongDescription As String
            Get
                Return Me._LongDescription
            End Get
            Set(value As String)
                Me._LongDescription = value
            End Set
        End Property

        <XmlAttribute()>
        Public DBName As String = ""

        <XmlIgnore()>
        Public KLIdentityLetter As String = ""

        <XmlAttribute()>
        Public isEntityType As Boolean = False

        <XmlAttribute()>
        Public isGeneralConcept As Boolean = False

        <XmlAttribute()>
        Public isValueType As Boolean = False

        <XmlAttribute()>
        Public isFactType As Boolean = False

        <XmlAttribute()>
        Public isFact As Boolean = False

        <XmlAttribute()>
        Public isValue As Boolean = False

        <XmlAttribute()>
        Public isRoleConstraint As Boolean = False

        <XmlAttribute()>
        Public isModelNote As Boolean = False

        <XmlIgnore()>
        Private _isDirty As Boolean = False

        <XmlIgnore()>
        Public Property isDirty As Boolean
            Get
                Return Me._isDirty
            End Get
            Set(value As Boolean)
                Me._isDirty = value
            End Set
        End Property

        Public Sub New()
            '-------------
            'Default
            '-------------
        End Sub


        Public Sub New(ByVal arModel As FBM.Model,
                       ByVal asSymbol As String,
                       ByVal aiConceptType As pcenumConceptType,
                       Optional ByVal asShortDescription As String = "",
                       Optional ByVal asLongDescription As String = "",
                       Optional ByVal abMakeConceptDirty As Boolean = True,
                       Optional ByVal abMakeDictionaryEntryDirty As Boolean = False,
                       Optional ByVal asDBName As String = "")

            Me.Model = arModel
            Me.Symbol = asSymbol
            Me.DBName = asDBName

            '-------------------------------------------------------------------------------
            'NB Only used in Model.AddModelDictionaryEntry and Me.EqualsByOtherConceptType
            '  A DictionaryEntry can have many ConceptTypes
            '-------------------------------------------------------------------------------
            Me.ConceptType = aiConceptType

            Select Case aiConceptType
                Case Is = pcenumConceptType.GeneralConcept
                    Me.isGeneralConcept = True
                Case Is = pcenumConceptType.EntityType
                    Me.isEntityType = True
                Case Is = pcenumConceptType.ValueType
                    Me.isValueType = True
                Case Is = pcenumConceptType.FactType
                    Me.isFactType = True
                Case Is = pcenumConceptType.RoleConstraint
                    Me.isRoleConstraint = True
                Case Is = pcenumConceptType.Fact
                    Me.isFact = True
                Case Is = pcenumConceptType.Value
                    Me.isValue = True
                Case Is = pcenumConceptType.ModelNote
                    Me.isModelNote = True
            End Select

            Me.Concept = New FBM.Concept(asSymbol, abMakeConceptDirty)

            Me.ShortDescription = asShortDescription
            Me.LongDescription = asLongDescription

            '--------------------------------------------------------
            'NB Me.isDirty = False by default for DictionaryEntries
            If Me.Model.Loaded Then
                If Me.Model.Page.FindAll(Function(x) x.Loaded = False) IsNot Nothing Then Me.isDirty = abMakeDictionaryEntryDirty
            End If

        End Sub

        Public Shadows Function Equals(ByVal other As FBM.DictionaryEntry) As Boolean Implements System.IEquatable(Of FBM.DictionaryEntry).Equals

            Return LCase(Me.Symbol) = LCase(other.Symbol)

        End Function

        Public Shadows Function EqualsCase(ByVal other As FBM.DictionaryEntry) As Boolean

            Return Me.Symbol = other.Symbol

        End Function

        Public Function EqualsLowercase(ByVal other As FBM.DictionaryEntry) As Boolean

            If LCase(Me.Symbol) = LCase(other.Symbol) Then
                Return True
            Else
                Return False
            End If

        End Function

        Public Shadows Function EqualsBySymbol(ByVal other As FBM.DictionaryEntry) As Boolean

            If (Me.Symbol = other.Symbol) Then
                Return True
            Else
                Return False
            End If

        End Function

        Public Function EqualsBySymbolConceptType(ByVal other As FBM.DictionaryEntry) As Boolean

            If (Me.Symbol = other.Symbol) And (Me.ConceptType = other.ConceptType) Then
                Return True
            Else
                Return False
            End If

        End Function

        Public Shadows Function EqualsByConceptTypeOnly(ByVal other As FBM.DictionaryEntry) As Boolean

            If (Me.Symbol = other.Symbol) Then
                Select Case Me.ConceptType
                    Case Is = pcenumConceptType.EntityType
                        If other.isEntityType Then
                            Return True
                        End If
                    Case Is = pcenumConceptType.ValueType
                        If other.isValueType Then
                            Return True
                        End If
                    Case Is = pcenumConceptType.FactType
                        If other.isFactType Then
                            Return True
                        End If
                    Case Is = pcenumConceptType.RoleConstraint
                        If other.isRoleConstraint Then
                            Return True
                        End If
                    Case Is = pcenumConceptType.ModelNote
                        If other.isModelNote Then
                            Return True
                        End If
                    Case Is = pcenumConceptType.Fact
                        If other.isFact Then
                            Return True
                        End If
                    Case Is = pcenumConceptType.Value
                        If other.isValue Then
                            Return True
                        End If
                End Select
            Else
                Return False
            End If

            Return False

        End Function

        ''' <summary>
        ''' Used to see if a DictionaryEntry exists but for a ConceptType other than the provided DictionaryEntry.ConceptType
        ''' </summary>
        ''' <param name="other"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shadows Function EqualsByOtherConceptType(ByVal other As FBM.DictionaryEntry) As Boolean

            If (Me.Symbol = other.Symbol) Then
                Select Case Me.ConceptType
                    Case Is = pcenumConceptType.EntityType
                        If other.isValueType Or
                           other.isFactType Or
                           other.isRoleConstraint Or
                           other.isModelNote Or
                           other.isFact Or
                           other.isValue Or
                           other.isGeneralConcept Then
                            Return True
                        End If
                    Case Is = pcenumConceptType.ValueType
                        If other.isEntityType Or
                           other.isFactType Or
                           other.isRoleConstraint Or
                           other.isModelNote Or
                           other.isFact Or
                           other.isValue Or
                           other.isGeneralConcept Then
                            Return True
                        End If
                    Case Is = pcenumConceptType.FactType
                        If other.isEntityType Or
                           other.isValueType Or
                           other.isRoleConstraint Or
                           other.isModelNote Or
                           other.isFact Or
                           other.isValue Or
                           other.isGeneralConcept Then
                            Return True
                        End If
                    Case Is = pcenumConceptType.RoleConstraint
                        If other.isEntityType Or
                           other.isValueType Or
                           other.isFactType Or
                           other.isModelNote Or
                           other.isFact Or
                           other.isValue Or
                           other.isGeneralConcept Then
                            Return True
                        End If
                    Case Is = pcenumConceptType.ModelNote
                        If other.isEntityType Or
                           other.isValueType Or
                           other.isFactType Or
                           other.isRoleConstraint Or
                           other.isFact Or
                           other.isValue Or
                           other.isGeneralConcept Then
                            Return True
                        End If
                    Case Is = pcenumConceptType.Fact
                        If other.isEntityType Or
                           other.isValueType Or
                           other.isFactType Or
                           other.isRoleConstraint Or
                           other.isModelNote Or
                           other.isValue Or
                           other.isGeneralConcept Then
                            Return True
                        End If
                    Case Is = pcenumConceptType.Value
                        If other.isEntityType Or
                           other.isValueType Or
                           other.isFactType Or
                           other.isRoleConstraint Or
                           other.isModelNote Or
                           other.isFact Or
                           other.isGeneralConcept Then
                            Return True
                        End If
                End Select
            Else
                Return False
            End If

        End Function

        Public Function Clone(ByRef arModel As FBM.Model) As FBM.DictionaryEntry

            Dim lrDictionaryEntry As New FBM.DictionaryEntry

            With Me
                lrDictionaryEntry.Model = arModel
                lrDictionaryEntry.Symbol = .Symbol
                lrDictionaryEntry.Concept = .Concept.Clone
                lrDictionaryEntry.ConceptType = Nothing
                lrDictionaryEntry.isEntityType = .isEntityType
                lrDictionaryEntry.isValueType = .isValueType
                lrDictionaryEntry.isFactType = .isFactType
                lrDictionaryEntry.isRoleConstraint = .isRoleConstraint
                lrDictionaryEntry.isFact = .isFact
                lrDictionaryEntry.isValue = .isValue
                lrDictionaryEntry.KLIdentityLetter = .KLIdentityLetter
                lrDictionaryEntry.ShortDescription = .ShortDescription
                lrDictionaryEntry.LongDescription = .LongDescription
                lrDictionaryEntry.Realisations = New List(Of pcenumConceptType)
                lrDictionaryEntry.isDirty = True
            End With

            Return lrDictionaryEntry

        End Function

        ''' <summary>
        ''' Adds a ConceptType to a DictionaryEntry.
        '''   NB A DictionaryEntry may have many ConceptTypes.
        ''' </summary>
        ''' <param name="aiConceptType"></param>
        ''' <remarks></remarks>
        Public Sub AddConceptType(ByVal aiConceptType As pcenumConceptType)

            Select Case aiConceptType
                Case Is = pcenumConceptType.GeneralConcept
                    Me.isGeneralConcept = True
                Case Is = pcenumConceptType.EntityType
                    Me.isEntityType = True
                Case Is = pcenumConceptType.ValueType
                    Me.isValueType = True
                Case Is = pcenumConceptType.FactType
                    Me.isFactType = True
                Case Is = pcenumConceptType.RoleConstraint
                    Me.isRoleConstraint = True
                Case Is = pcenumConceptType.ModelNote
                    Me.isModelNote = True
                Case Is = pcenumConceptType.Fact
                    Me.isFact = True
                Case Is = pcenumConceptType.Value, pcenumConceptType.RoleData
                    Me.isValue = True
            End Select

            Me.Realisations.AddUnique(aiConceptType)
            '20220215-VM-Was. Was too slow.
            'If Me.Model.Loaded Then
            '    If Me.Model.Page.FindAll(Function(x) x.Loaded = False) IsNot Nothing Then Me.isDirty = True
            'End If
        End Sub

        Public Sub AddRealisation(ByVal aiConceptType As pcenumConceptType, Optional ByVal abUnique As Boolean = False)

            If abUnique Then
                Me.Realisations.AddUnique(aiConceptType)
            Else
                Me.Realisations.Add(aiConceptType)
            End If

        End Sub


        Public Function GenerateKLIdentityLetter(ByRef aarModelDictionary As List(Of FBM.DictionaryEntry), ByVal aiStartingPosition As Integer, Optional ByVal asKLIdentityLetter As String = "")


            Dim KLIdentityLetters As Array = System.[Enum].GetValues(GetType(pcenumKLDataLetter))
            Dim KLIdentityLetter As pcenumKLDataLetter
            Dim lbFoundKLIdentityLetter As Boolean = False

            For Each KLIdentityLetter In KLIdentityLetters
                Me.KLIdentityLetter = asKLIdentityLetter & KLIdentityLetter.ToString
                If IsSomething(aarModelDictionary.Find(AddressOf Me.MatchesKLIdentityLetter)) Then
                    '-----------------------------------------------------------------------------
                    'Do nothing, as the KL Identity Letter already exists in the ModelDictionary
                    '  Move to the next KLIdentityLetter.
                    '-----------------------------------------------------------------------------
                Else
                    '------------------------------------------------------------------------------------
                    'Found a unique KL Identity Letter that is not already in the ModelDictionary
                    '  so use that as the KL Identity Letter for this Concept (in the Model Dictionary)
                    '------------------------------------------------------------------------------------                    
                    lbFoundKLIdentityLetter = True
                    Exit For
                End If
                aiStartingPosition += 1
            Next

            If lbFoundKLIdentityLetter Then
                '-------------------------------------------------------------------
                'All good, found a unique KLIdentityLetter for the DictionaryEntry
                '-------------------------------------------------------------------
            Else
                Dim lsStartingPrefix As String = ""
                lsStartingPrefix = Richmond.ConvertNumberToLetters(aiStartingPosition)
                Me.KLIdentityLetter = Me.GenerateKLIdentityLetter(aarModelDictionary, aiStartingPosition + 1, lsStartingPrefix)
            End If

            Return Me.KLIdentityLetter

        End Function

        ''' <summary>
        ''' Returns the ConceptType of the DictionaryEntry.
        '''   NB Should only be used in Model.AddModelDictionaryEntry
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetConceptType() As pcenumConceptType

            Return Me.ConceptType
        End Function

        ''' <summary>
        ''' Returns TRUE if the DictionaryEntry is only of ConceptType, "GeneralConcept" and no other ConceptType, else returns FALSE
        ''' </summary>
        ''' <returns>Returns TRUE if the DictionaryEntry is only of ConceptType, "GeneralConcept" and no other ConceptType, else returns FALSE</returns>
        ''' <remarks></remarks>
        Public Function isGeneralConceptOnly() As Boolean

            If Not Me.isValueType _
                And Not Me.isEntityType _
                And Not Me.isFactType _
                And Not Me.isRoleConstraint _
                And Not Me.isModelNote _
                And Me.isGeneralConcept Then

                Return True
            Else
                Return False
            End If

        End Function

        Public Function MatchesKLIdentityLetter(ByVal other As FBM.DictionaryEntry) As Boolean

            If Me.KLIdentityLetter = other.KLIdentityLetter Then
                Return True
            Else
                Return False
            End If

        End Function

        Public Function ModelElementTypeCount() As Integer

            Dim liModelElementTypeCount As Integer = 0

            If Me.isValueType Then liModelElementTypeCount += 1
            If Me.isEntityType Then liModelElementTypeCount += 1
            If Me.isFactType Then liModelElementTypeCount += 1
            If Me.isRoleConstraint Then liModelElementTypeCount += 1
            If Me.isFact Then liModelElementTypeCount += 1
            If Me.isValue Then liModelElementTypeCount += 1
            If Me.isModelNote Then liModelElementTypeCount += 1

            Return liModelElementTypeCount

        End Function

        Public Sub MorphTo(ByRef ar_enterprise_object As System.Object)

            Select Case ar_enterprise_object.ConceptType
                Case pcenumConceptType.EntityType
                    'ar_enterprise_object = Me.Symbol_object
                    ar_enterprise_object.ConceptType = pcenumConceptType.EntityType
            End Select

        End Sub

        Public Sub RefreshShape(Optional ByVal aoChangedPropertyItem As PropertyValueChangedEventArgs = Nothing,
                                Optional ByVal asSelectedGridItemLabel As String = "")

            Me.Model.MakeDirty(True, False)

        End Sub


        Public Overridable Overloads Sub Save(Optional ByRef abRapidSave As Boolean = False) Implements iObjectRelationalMap(Of DictionaryEntry).Save

            '-------------------------------------------
            'Saves the DictionaryEntry to the database
            '-------------------------------------------
            If abRapidSave Then
                '---------------------------------------------------------
                'Make sure an entry exists in the MetaModelConcept table
                '---------------------------------------------------------
                Dim lrConcept As New FBM.Concept(Me.Symbol, True)
                lrConcept.Save()
                Call TableModelDictionary.AddModelDictionaryEntry(Me)
                Me.isDirty = False
            ElseIf Me.isDirty Then

                'CodeSafe: If is only Value, then limit to 100 characters.
                If Me.Realisations.FindAll(Function(x) x <> pcenumConceptType.Value).Count = 0 Then
                    Me.Symbol = Strings.Left(Me.Symbol, 100)
                End If

                If TableModelDictionary.ExistsModelDictionaryEntry(Me) Then
                    Call TableModelDictionary.UpdateModelDictionaryEntry(Me)
                Else
                    '---------------------------------------------------------
                    'Make sure an entry exists in the MetaModelConcept table
                    '---------------------------------------------------------
                    Dim lrConcept As New FBM.Concept(Me.Symbol, True)
                    lrConcept.Save()
                    Call TableModelDictionary.AddModelDictionaryEntry(Me)
                End If

                Me.isDirty = False
            End If

        End Sub

        Public Shadows Event updated()

        Private Sub Concept_Updated() Handles Concept.ConceptSymbolUpdated

            Try
                Call TableModelDictionary.ModifySymbol(Me.Model, Me, Me.Concept.Symbol, pcenumConceptType.EntityType)

                Me.Symbol = Me.Concept.Symbol

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Public Sub removeConceptType(ByVal aiConceptType As pcenumConceptType, Optional abRemoveDictionaryEntryIfNoRealisations As Boolean = False)

            Select Case aiConceptType
                Case Is = pcenumConceptType.ValueType
                    Me.isValueType = False
                Case Is = pcenumConceptType.EntityType
                    Me.isEntityType = False
                Case Is = pcenumConceptType.FactType
                    Me.isFactType = False
                Case Is = pcenumConceptType.RoleConstraint
                    Me.isRoleConstraint = False
                Case Is = pcenumConceptType.ModelNote
                    Me.isModelNote = False
                Case Is = pcenumConceptType.Value
                    Me.isValue = False
            End Select

            Me.Realisations.RemoveAll(Function(x) x = aiConceptType)

            '2021004-VM-Added the following. Particularly for when removing ETs, VTs, RCs and FTs from the Model.
            If abRemoveDictionaryEntryIfNoRealisations Then
                If Me.Realisations.Count = 0 Then
                    Call TableModelDictionary.DeleteModelDictionaryEntry(Me)
                    Me.Model.ModelDictionary.Remove(Me)
                    Me.Model.Dictionary.Remove(Me.Symbol)
                End If
            End If

            Me.isDirty = True

        End Sub

        Public Function representsModelObject() As Boolean

            If Me.isValueType Or Me.isEntityType Or Me.isFactType Or Me.isRoleConstraint Then
                Return True
            Else
                Return False
            End If

        End Function

        Public Sub Create() Implements iObjectRelationalMap(Of DictionaryEntry).Create
            Throw New NotImplementedException()
        End Sub

        Public Sub Delete() Implements iObjectRelationalMap(Of DictionaryEntry).Delete
            Throw New NotImplementedException()
        End Sub

        Public Function Load() As DictionaryEntry Implements iObjectRelationalMap(Of DictionaryEntry).Load
            Throw New NotImplementedException()
        End Function

        Public Function Compare(x As DictionaryEntry, y As DictionaryEntry) As Integer Implements IComparer(Of DictionaryEntry).Compare
            Return StrComp(x.Symbol, y.Symbol)
        End Function

        Public Function RealisationsOnlyContainsConceptType(ByVal aiConceptType As pcenumConceptType) As Boolean

            Try
                Dim laiOtherConceptTypes = From Realisation In Me.Realisations
                                           Where Realisation <> aiConceptType
                                           Select Realisation

                Return laiOtherConceptTypes.Count > 0

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
                Return False
            End Try

        End Function

    End Class
End Namespace