Imports System.ComponentModel
Imports System.Collections.Specialized
Imports System.Xml.Serialization
Imports System.Reflection

Namespace FBM
    <Serializable()> _
    Public Class ValueType
        Inherits FBM.ModelObject
        Implements IEquatable(Of FBM.ValueType)
        Implements ICloneable
        Implements iMDAObject
        Implements FBM.iValidationErrorHandler

        <XmlIgnore()> _
        <DebuggerBrowsable(DebuggerBrowsableState.Never)> _
        Private _IsMDAModelElement As Boolean = False
        <XmlAttribute()> _
        Public Property IsMDAModelElement() As Boolean Implements iMDAObject.IsMDAModelElement
            Get
                Return Me._IsMDAModelElement
            End Get
            Set(ByVal value As Boolean)
                Me._IsMDAModelElement = value
            End Set
        End Property

        <XmlIgnore()> _
        Public Shadows Property Name() As String
            Get
                Return _Name
            End Get
            Set(ByVal value As String)
                _Name = value
                Me.Symbol = value
                Me.Id = value
            End Set
        End Property

        Public PrimativeType As String

        <XmlIgnore()> _
        Public NORMAIsUnaryFactTypeValueType As Boolean = False

        <XmlIgnore()> _
        <DebuggerBrowsable(DebuggerBrowsableState.Never)> _
        Public _DataType As pcenumORMDataType = pcenumORMDataType.DataTypeNotSet
        <XmlAttribute()> _
        <CategoryAttribute("Value Type"), _
        Browsable(True), _
        [ReadOnly](False), _
        BindableAttribute(True), _
        DefaultValueAttribute(""), _
        DesignOnly(False), _
        DescriptionAttribute("The 'Data Type' for the Domain of the Value Type"), _
        TypeConverter(GetType(Enumeration.EnumDescConverter))> _
        Public Property DataType() As pcenumORMDataType
            Get
                Return _DataType
            End Get
            Set(ByVal Value As pcenumORMDataType)
                _DataType = Value
            End Set
        End Property

        Private _DataTypePrecision As Integer
        <CategoryAttribute("Value Type"), _
        Browsable(False), _
        [ReadOnly](False), _
        BindableAttribute(True), _
        DefaultValueAttribute(""), _
        DesignOnly(False), _
        DescriptionAttribute("The 'Data Type Precision' of the Data Type.")> _
        Public Property DataTypePrecision() As Integer
            Get
                Return Me._DataTypePrecision
            End Get
            Set(ByVal value As Integer)
                Me._DataTypePrecision = value
            End Set
        End Property

        Private _DataTypeLength As Integer
        <CategoryAttribute("Value Type"), _
        Browsable(False), _
        [ReadOnly](False), _
        BindableAttribute(True), _
        DefaultValueAttribute(""), _
        DesignOnly(False), _
        DescriptionAttribute("The 'Data Type Length' of the Data Type.")> _
        Public Property DataTypeLength() As Integer
            Get
                Return Me._DataTypeLength
            End Get
            Set(ByVal value As Integer)
                Me._DataTypeLength = value
            End Set
        End Property

        <XmlIgnore()> _
        <DebuggerBrowsable(DebuggerBrowsableState.Never)> _
        Public _ValueConstraint As New List(Of FBM.Concept)

        <XmlIgnore()> _
        <DebuggerBrowsable(DebuggerBrowsableState.Never)> _
        Public _ValueConstraintList As New VievLibrary.Strings.StringCollection
        '<XmlIgnore()> _
        <CategoryAttribute("Value Type"), _
         Browsable(True), _
         [ReadOnly](False), _
         DescriptionAttribute("The List of Values that Objects of this Entity Type may take."), _
         Editor(GetType(tStringCollectionEditor), GetType(System.Drawing.Design.UITypeEditor))> _
        Public Property ValueConstraint() As VievLibrary.Strings.StringCollection 'StringCollection 
            '   DefaultValueAttribute(""), _
            '   BindableAttribute(True), _
            '   DesignOnly(False), _
            Get
                Return Me._ValueConstraintList
            End Get
            Set(ByVal Value As VievLibrary.Strings.StringCollection)
                Me._ValueConstraintList = Value
                '----------------------------------------------------
                'Update the set of Concepts/Symbols/Values
                '  within the 'value_constraint' for this ValueType.
                '----------------------------------------------------
                Dim lsString As String = ""
                For Each lsString In Me._ValueConstraintList
                    Dim lrConcept As New FBM.Concept(lsString)
                    If Me._ValueConstraint.Contains(lrConcept) Then
                        '-------------------------------------------------
                        'Nothing to do, because the Concept/Symbol/Value
                        '  already exists for the 'value_constraint'
                        '  for this ValueType.
                        '-------------------------------------------------
                    Else
                        '-------------------------------------------
                        'Add the Concept/Symbol/Value to the Model
                        '-----------------------------------------
                        Dim lrModelDictionaryEntry As New FBM.DictionaryEntry(Me.Model, lrConcept.Symbol, pcenumConceptType.Value)
                        Me.Model.AddModelDictionaryEntry(lrModelDictionaryEntry)
                        '-----------------------------------------
                        'Add the Concept/Symbol/Value to the
                        '  'value_constraint' for this ValueType
                        '-----------------------------------------
                        Me._ValueConstraint.Add(lrConcept)
                    End If
                Next
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

        Public ReadOnly Property HasModelError() As Boolean Implements iValidationErrorHandler.HasModelError
            Get
                Return Me.ModelError.Count > 0
            End Get
        End Property

        Public Event NameChanged()
        Public Event DataTypeChanged(ByVal aiNewDataType As pcenumORMDataType)
        Public Event DataTypePrecisionChanged(ByVal aiNewDataTypePrecision As Integer)
        Public Event DataTypeLengthChanged(ByVal aiDataTypeLength As Integer)
        Public Event ValueConstraintAdded(ByVal asNewValueConstraint As String)
        Public Event ValueConstraintRemoved(ByVal asRemovedValueConstraint As String)
        Public Event ValueConstraintModified(ByVal asOldValue As String, ByVal asNewValue As String)
        Public Event ModelErrorAdded(ByRef arModelError As ModelError) Implements iValidationErrorHandler.ModelErrorAdded
        Public Event RemovedFromModel()

        Sub New()
            Me.ConceptType = pcenumConceptType.ValueType

        End Sub

        Public Sub New(ByRef arModel As FBM.Model, ByVal aiLanguageId As pcenumLanguage, ByVal asValueTypeName As String, ByVal abUseValueTypeName As Boolean)

            Me.New()

            Me.Model = arModel

            If IsSomething(asValueTypeName) Then
                Me.Name = asValueTypeName
            Else
                Me.Name = "New Value Type"
            End If

            If abUseValueTypeName Then
                Me.Id = Trim(Me.Name)
            Else
                Me.Id = System.Guid.NewGuid.ToString
            End If

        End Sub

        Public Sub New(ByRef arModel As FBM.Model, ByVal aiLanguageId As pcenumLanguage, ByVal as_value_type_name As String, ByVal as_ValueTypeId As String)

            Me.New()

            Me.Model = arModel
            Me.Name = as_value_type_name
            Me.Id = as_ValueTypeId

        End Sub


        Public Shadows Function Equals(ByVal other As FBM.ValueType) As Boolean Implements System.IEquatable(Of FBM.ValueType).Equals

            If Me.Id = other.Id Then
                Return True
            Else
                Return False
            End If

        End Function

        Public Shadows Function EqualsByName(ByVal other As FBM.ValueType) As Boolean

            If Me.Name = other.Name Then
                Return True
            Else
                Return False
            End If

        End Function

        Public Shadows Function EqualsByNameLike(ByVal other As FBM.ValueType) As Boolean

            If other.Name Like (Me.Name & "*") Then
                Return True
            Else
                Return False
            End If

        End Function

        Public Overloads Function Clone(ByRef arModel As FBM.Model, Optional abAddToModel As Boolean = False) As Object

            Dim lrValueType As New FBM.ValueType

            If arModel.ValueType.Exists(AddressOf Me.Equals) Then
                '---------------------------------------------------------------------------------------------------------------------
                'The target ValueType already exists in the target Model, so return the existing ValueType (from the target Model)
                '  20150127-There seems no logical reason to clone an ValueType to a target Model if it already exists in the target
                '  Model. This method is used when copying/pasting from one Model to a target Model, and (in general) the ValueType
                '  won't exist in the target Model. If it does, then that's the ValueType that's needed.
                '  NB Testing to see if the Signature of the ValueType already exists in the target Model is already performed in the
                '  Paste proceedure before dropping the ValueType onto a target Page/Model. If there is/was any clashes, then the 
                '  ValueType being copied/pasted will have it's Id/Name/Symbol changed and will not be affected by this test to see
                '  if the ValueType already exists in the target Model.
                '---------------------------------------------------------------------------------------------------------------------
                lrValueType = arModel.ValueType.Find(AddressOf Me.Equals)
            Else
                With Me
                    lrValueType.Model = New FBM.Model
                    lrValueType.Model = arModel
                    lrValueType.Id = .Id
                    lrValueType.Name = .Name
                    lrValueType.Symbol = .Symbol
                    lrValueType.ConceptType = .ConceptType
                    lrValueType.ShortDescription = .ShortDescription
                    lrValueType.LongDescription = .LongDescription

                    lrValueType.ValueConstraint = .ValueConstraint
                    lrValueType.PrimativeType = .PrimativeType
                    lrValueType.DataType = .DataType

                    If abAddToModel Then
                        arModel.AddValueType(lrValueType)
                    End If
                End With
            End If

            Return lrValueType

        End Function

        ''' <summary>
        ''' Used to 'Sort' Enumerated lists of tValueType
        ''' </summary>
        ''' <param name="ao_a"></param>
        ''' <param name="ao_b"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function CompareValueTypeNames(ByVal ao_a As FBM.ValueType, ByVal ao_b As FBM.ValueType) As Integer

            Return StrComp(ao_a.Name, ao_b.Name, CompareMethod.Text)

        End Function

        Public Overrides Function CanSafelyRemoveFromModel() As Boolean

            If Me.ExistsRolesAssociatedWithValueType Then
                Return False
            Else
                Return True
            End If

        End Function

        ''' <summary>
        ''' Changes the Model of the ValueType to the target Model.
        ''' </summary>
        ''' <param name="arTargetModel">The Model to which the ValueType will be associated on completion of this method.</param>
        ''' <remarks></remarks>
        Public Sub ChangeModel(ByRef arTargetModel As FBM.Model)

            Me.Model = arTargetModel

            If arTargetModel.ValueType.Exists(AddressOf Me.Equals) Then
                '----------------------------------------------
                'The ValueType is already in the TargetModel
                '----------------------------------------------
            Else
                arTargetModel.AddValueType(Me)
            End If

        End Sub

        Public Sub AddValueConstraint(ByVal asValueConstraint As String)

            Me.ValueConstraint.Add(asValueConstraint)

            RaiseEvent ValueConstraintAdded(asValueConstraint)            

        End Sub

        ''' <summary>
        ''' Returns TRUE if there are any Roles (within FactTypes) that are associated with the ValueType, else returns FALSE
        ''' </summary>
        ''' <returns>TRUE if there are any Roles (within FactTypes) that are associated with the ValueType, else returns FALSE</returns>
        ''' <remarks></remarks>
        Public Function ExistsRolesAssociatedWithValueType() As Boolean

            ExistsRolesAssociatedWithValueType = False

            Dim larRoles = From FactType In Me.Model.FactType _
                              From Role In FactType.RoleGroup _
                              Where Role.JoinedORMObject Is Me _
                              Select Role

            Dim lrRole As New FBM.Role

            For Each lrRole In larRoles
                Return True
            Next

        End Function

        ''' <summary>
        ''' Returns True if there are no Roles that reference the ValueType, else returns False
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function IsIndependant() As Boolean

            IsIndependant = True

            Dim larRole = From Role In Me.Model.Role _
                          Where Role.JoinedORMObject Is Me _
                          Select Role

            If larRole.Count > 0 Then
                IsIndependant = False
            End If

        End Function

        ''' <summary>
        ''' Returns TRUE if is the ReferenceMode of an EntityType, else returns FALSE
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function IsReferenceMode() As Boolean

            IsReferenceMode = False

            Dim liFactTypeCount As Integer = 0

            liFactTypeCount = Aggregate FactType In Me.Model.FactType _
                              From Role In FactType.RoleGroup _
                              Where Role.JoinedORMObject Is Me _
                              And FactType.IsPreferredReferenceMode = True _
                              Into Count()

            If liFactTypeCount > 0 Then
                IsReferenceMode = True
            End If

        End Function

        ''' <summary>
        ''' Formulates a ReferenceMode (for an EntityType) from the Name of the ValueType.
        '''   e.g. "Id" from "Person_Id".
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetReferenceModeFromName() As String

            Dim lsReferenceMode As String = ""

            If InStr(Me.Name, "_") > 0 Then
                lsReferenceMode = Right(Me.Name, Me.Name.Length - InStr(Me.Name, "_"))
            Else
                lsReferenceMode = Me.Name
            End If

            Return lsReferenceMode

        End Function

        ''' <summary>
        ''' Returns the unique Signature of the ValueType
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overrides Function GetSignature() As String

            Dim lsSignature As String

            lsSignature = Me.Id
            lsSignature &= Me.DataType

            Return lsSignature

        End Function


        Public Sub ModifyValueConstraint(ByVal asOldValueConstraint As String, asNewValueConstraint As String)

            Try

                Me.ValueConstraint.Item(Me.ValueConstraint.IndexOf(asOldValueConstraint)) = asNewValueConstraint

                RaiseEvent ValueConstraintModified(asOldValueConstraint, asNewValueConstraint)

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                Me.Model.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace, True)
            End Try

        End Sub


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

        Public Sub RemoveValueConstraint(ByVal asValueConstraint As String)

            Me.ValueConstraint.Remove(asValueConstraint)

            RaiseEvent ValueConstraintRemoved(asValueConstraint)

        End Sub

        ''' <summary>
        ''' Prototype for the Save function.
        ''' </summary>
        ''' <remarks></remarks>
        Public Overridable Overloads Sub Save()
        End Sub

        Public Sub SetDataType(ByVal aiNewDataType As pcenumDataType, Optional ByVal aiDataTypeLength As Integer = Nothing, Optional ByVal aiDataTypePrecision As Integer = Nothing)

            Me.DataType = aiNewDataType

            If IsSomething(aiDataTypeLength) Then
                Me.DataTypeLength = aiDataTypeLength
            End If

            If IsSomething(aiDataTypePrecision) Then
                Me.DataTypePrecision = aiDataTypePrecision
            End If

            RaiseEvent DataTypeChanged(aiNewDataType)

            Call Me.Model.MakeDirty()

        End Sub

        Public Sub SetDataTypeLength(ByVal aiNewDataTypeLength As Integer)

            Me.DataTypeLength = aiNewDataTypeLength
            RaiseEvent DataTypeLengthChanged(aiNewDataTypeLength)

            Call Me.Model.MakeDirty()
        End Sub

        Public Sub SetDataTypePrecision(ByVal aiNewDataTypePrecision As Integer)

            Me.DataTypePrecision = aiNewDataTypePrecision
            RaiseEvent DataTypePrecisionChanged(aiNewDataTypePrecision)

            Call Me.Model.MakeDirty()

        End Sub

        Public Overridable Sub SetName(ByVal asNewName As String)

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

                    Me.Id = asNewName                    

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

        Sub SetValueConstraint(ByVal arValueConstraint As VievLibrary.Strings.StringCollection)

            Me._ValueConstraintList = arValueConstraint
            '----------------------------------------------------
            'Update the set of Concepts/Symbols/Values
            '  within the 'value_constraint' for this ValueType.
            '----------------------------------------------------
            Dim lsString As String = ""
            For Each lsString In Me._ValueConstraintList
                Dim lrConcept As New FBM.Concept(lsString)
                If Me._ValueConstraint.Contains(lrConcept) Then
                    '-------------------------------------------------
                    'Nothing to do, because the Concept/Symbol/Value
                    '  already exists for the 'value_constraint'
                    '  for this ValueType.
                    '-------------------------------------------------
                Else
                    '-------------------------------------------
                    'Add the Concept/Symbol/Value to the Model
                    '-----------------------------------------
                    Dim lrModelDictionaryEntry As New FBM.DictionaryEntry(Me.Model, lrConcept.Symbol, pcenumConceptType.Value)
                    Me.Model.AddModelDictionaryEntry(lrModelDictionaryEntry)
                    '-----------------------------------------
                    'Add the Concept/Symbol/Value to the
                    '  'value_constraint' for this ValueType
                    '-----------------------------------------
                    Me._ValueConstraint.Add(lrConcept)
                End If
            Next
            RaiseEvent updated()
            Call Me.Model.MakeDirty()
        End Sub

        'Public Sub ModelUpdated() Handles model.ModelUpdated
        '    RaiseEvent updated()
        'End Sub

        Public Shadows Event updated()

        Private Sub Concept_Updated() Handles Concept.ConceptSymbolUpdated

            Me.SetName(Me.Concept.Symbol)

        End Sub


        Public Sub ClearModelErrors() Implements iValidationErrorHandler.ClearModelErrors
            Me.ModelError.Clear()
        End Sub

        Public Sub AddModelError(ByRef arModelError As ModelError) Implements iValidationErrorHandler.AddModelError

            Me.ModelError.Add(arModelError)
            RaiseEvent ModelErrorAdded(arModelError)

        End Sub

    End Class
End Namespace