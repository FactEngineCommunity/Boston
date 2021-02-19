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
        Implements FBM.iFBMIndependence

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

        <XmlIgnore()>
        <DebuggerBrowsable(DebuggerBrowsableState.Never)>
        Public _DataType As pcenumORMDataType = pcenumORMDataType.DataTypeNotSet


        <XmlAttribute()>
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
        <CategoryAttribute("Value Type"),
        Browsable(False),
        [ReadOnly](False),
        BindableAttribute(True),
        DefaultValueAttribute(""),
        DesignOnly(False),
        DescriptionAttribute("The 'Data Type Precision' of the Data Type.")>
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
        Public _ValueConstraintList As New Viev.Strings.StringCollection
        '<XmlIgnore()> _
        <CategoryAttribute("Value Type"),
         Browsable(True),
         [ReadOnly](False),
         DescriptionAttribute("The List of Values that Objects of this Value Type may take."),
         Editor(GetType(tStringCollectionEditor), GetType(System.Drawing.Design.UITypeEditor))>
        Public Property ValueConstraint() As Viev.Strings.StringCollection 'StringCollection 
            '   DefaultValueAttribute(""), _
            '   BindableAttribute(True), _
            '   DesignOnly(False), _
            Get
                Return Me._ValueConstraintList
            End Get
            Set(ByVal Value As Viev.Strings.StringCollection)
                Me._ValueConstraintList = Value
                '----------------------------------------------------
                'Update the set of Concepts/Symbols/Values
                '  within the 'value_constraint' for this ValueType.
                '----------------------------------------------------
                Dim lsString As String
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

        <XmlIgnore()> _
        <DebuggerBrowsable(DebuggerBrowsableState.Never)> _
        Private _IsIndependent As Boolean = True
        <XmlAttribute()> _
        <CategoryAttribute("Model Object"), _
        DefaultValueAttribute(False),
        DescriptionAttribute("True if the Model Object is independent.")>
        Public Property IsIndependent As Boolean Implements iFBMIndependence.IsIndependent
            Get
                If Me.Model Is Nothing Then
                    Return Me._IsIndependent
                End If

                Dim laoRole = From Role In Me.Model.Role
                              Where Role.JoinedORMObject.Id = Me.Id
                              Select Role

                Return laoRole.Count = 0

            End Get
            Set(ByVal value As Boolean)
                Me._IsIndependent = value
            End Set
        End Property

        Public ReadOnly Property HasModelError() As Boolean Implements iValidationErrorHandler.HasModelError
            Get
                Return Me.ModelError.Count > 0
            End Get
        End Property

        Public Event DataTypeChanged(ByVal aiNewDataType As pcenumORMDataType)
        Public Event DataTypePrecisionChanged(ByVal aiNewDataTypePrecision As Integer)
        Public Event DataTypeLengthChanged(ByVal aiDataTypeLength As Integer)
        Public Event IsIndependentChanged(abNewIsIndependent As Boolean) Implements iFBMIndependence.IsIndependentChanged
        Public Event ValueConstraintAdded(ByVal asNewValueConstraint As String)
        Public Event ValueConstraintRemoved(ByVal asRemovedValueConstraint As String)
        Public Event ValueConstraintModified(ByVal asOldValue As String, ByVal asNewValue As String)
        Public Event ModelErrorAdded(ByRef arModelError As ModelError) Implements iValidationErrorHandler.ModelErrorAdded
        Public Event RemovedFromModel(ByVal abBroadcastInterfaceEvent As Boolean)

        Sub New()
            Me.ConceptType = pcenumConceptType.ValueType
            Me.isDirty = True
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

            Me.isDirty = True

        End Sub

        Public Sub New(ByRef arModel As FBM.Model, ByVal aiLanguageId As pcenumLanguage, ByVal as_value_type_name As String, ByVal as_ValueTypeId As String)

            Me.New()

            Me.Model = arModel
            Me.Name = as_value_type_name
            Me.Id = as_ValueTypeId
            Me.Symbol = as_ValueTypeId
            Me.isDirty = True

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

        Public Overrides Function EqualsBySignature(ByVal other As FBM.ModelObject) As Boolean

            If other.GetSignature = Me.GetSignature Then
                Return True
            Else
                Return False
            End If

        End Function

        Public Overloads Function Clone(ByRef arModel As FBM.Model, _
                                        Optional abAddToModel As Boolean = False, _
                                        Optional ByVal abIsMDAModelElement As Boolean = False) As Object

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
                    lrValueType.Model = arModel
                    lrValueType.Id = .Id
                    lrValueType.Name = .Name
                    lrValueType.Symbol = .Symbol
                    lrValueType.ConceptType = .ConceptType
                    lrValueType.ShortDescription = .ShortDescription
                    lrValueType.LongDescription = .LongDescription
                    lrValueType.isDirty = True
                    lrValueType.IsIndependent = .IsIndependent
                    If abIsMDAModelElement = False Then
                        lrValueType.IsMDAModelElement = .IsMDAModelElement
                    Else
                        lrValueType.IsMDAModelElement = abIsMDAModelElement
                    End If

                    lrValueType.ValueConstraint = .ValueConstraint
                    lrValueType.PrimativeType = .PrimativeType
                    lrValueType.DataType = .DataType
                    lrValueType.DataTypeLength = .DataTypeLength
                    lrValueType.DataTypePrecision = .DataTypePrecision

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
        Public Shadows Sub ChangeModel(ByRef arTargetModel As FBM.Model)

            Me.Model = arTargetModel

            If arTargetModel.ValueType.Exists(AddressOf Me.Equals) Then
                '----------------------------------------------
                'The ValueType is already in the TargetModel
                '----------------------------------------------
            Else
                arTargetModel.AddValueType(Me)
            End If

            Me.isDirty = True

        End Sub

        Public Sub AddValueConstraint(ByVal asValueConstraint As String)

            Me.ValueConstraint.Add(asValueConstraint)
            Call Me.makeDirty()

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
        Public Function IsRoleIndependent() As Boolean

            IsRoleIndependent = True

            Dim larRole = From Role In Me.Model.Role _
                          Where Role.JoinedORMObject Is Me _
                          Select Role

            If larRole.Count > 0 Then
                IsRoleIndependent = False
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
        ''' Returns the CQL for the ValueType
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetCQLText() As String

            Dim lsCQLStatement As String = ""

            lsCQLStatement = Me.Name & " is written as " & Me.DataType.ToString & ";"

            Return lsCQLStatement

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
                lsReferenceMode = "." & lsReferenceMode
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

        Public Overrides Function CloneInstance(ByRef arPage As FBM.Page, Optional ByVal abAddToPage As Boolean = False) As FBM.ModelObject

            Dim lrValueTypeInstance As New FBM.ValueTypeInstance

            If arPage.ValueTypeInstance.Find(Function(x) x.Id = Me.Id) IsNot Nothing Then
                'The ValueType already exists as an instance on the Page.
                lrValueTypeInstance = arPage.ValueTypeInstance.Find(Function(x) x.Id = Me.Id)
            Else
                With Me
                    lrValueTypeInstance.Model = arPage.Model
                    lrValueTypeInstance.Page = arPage
                    lrValueTypeInstance.Id = .Id
                    lrValueTypeInstance.Name = .Name
                    lrValueTypeInstance.Symbol = .Symbol
                    lrValueTypeInstance.ConceptType = .ConceptType
                    lrValueTypeInstance.ValueType = Me
                    lrValueTypeInstance.ShortDescription = .ShortDescription
                    lrValueTypeInstance.LongDescription = .LongDescription
                    lrValueTypeInstance.IsIndependent = .IsIndependent
                    lrValueTypeInstance.PrimativeType = .PrimativeType
                    lrValueTypeInstance.DataType = .DataType
                    lrValueTypeInstance.DataTypeLength = .DataTypeLength
                    lrValueTypeInstance.DataTypePrecision = .DataTypePrecision
                    lrValueTypeInstance.X = 0
                    lrValueTypeInstance.Y = 0

                    Dim lsValueConstraint As String

                    For Each lsValueConstraint In Me.ValueConstraint
                        lrValueTypeInstance.ValueConstraint.Add(lsValueConstraint)
                    Next

                    If abAddToPage Then
                        If Not arPage.ValueTypeInstance.Exists(AddressOf lrValueTypeInstance.Equals) Then
                            arPage.ValueTypeInstance.Add(lrValueTypeInstance)
                        End If
                    End If

                End With
            End If


            Return lrValueTypeInstance

        End Function

        Public Sub ModifyValueConstraint(ByVal asOldValueConstraint As String, asNewValueConstraint As String)

            Try

                '-------------------------------------------------------------
                'Update ValueConstraintValue of the particular
                '  Concept/Symbol/Value within the 'value_constraint'
                '  for the ValueType of this ValueTypeInstance
                '-------------------------------------------------------------
                If Me._ValueConstraintList.IndexOf(asOldValueConstraint) >= 0 Then

                    Me.ValueConstraint.Item(Me.ValueConstraint.IndexOf(asOldValueConstraint)) = asNewValueConstraint

                    'VM-20180401-Not sure what this does.
                    'Dim lrConcept As New FBM.Concept(aoChangedPropertyItem.OldValue)
                    'lrConcept = Me.ValueType._ValueConstraint.Find(AddressOf lrConcept.Equals)
                    'If IsSomething(lrConcept) Then
                    '    lrConcept.Symbol = aoChangedPropertyItem.ChangedItem.Value.ToString
                    'End If

                    RaiseEvent ValueConstraintModified(asOldValueConstraint, asNewValueConstraint)

                ElseIf Not Me.ValueConstraint.Contains(asNewValueConstraint) Then
                    Me.AddValueConstraint(asNewValueConstraint)
                End If

                Me.isDirty = True
                Me.Model.MakeDirty(False, False)

                '=============================================================================================================================================================
                '20180401-VM-Can probably reimplement the below with more sophisticated code. Take code from populating a cell in a FactTable when VT has a ValueConstraint.
                '20170126-VM-Commented out the below.
                'The "Instance" piece can probably disappear altogether. That doesn't look good. Especially if a Value constraint looks like "1..12" rather than "1", "2" etc
                '  Need to remove this (ValueTypeInstance) setting of Me.ValueType.ValueConstraint from within the ValueConstraint property,
                '  but rather have a SetValueConstraint method, which will call the lower ValueType method....which will trigger an event
                '  that will update all related ValueTypeInstances....etc.
                ' Basically, this whole section needs an overhall. 
                'Me.ValueConstraint.Clear()
                'Me.ValueConstraint = lasStringCollection
                'Me.ValueType.Instance.Clear()
                'For Each lsValueConstraint In lasStringCollection
                '    Me.ValueType.Instance.Add(lsValueConstraint)
                'Next

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub


        Public Overrides Function RemoveFromModel(Optional ByVal abForceRemoval As Boolean = False,
                                                  Optional ByVal abCheckForErrors As Boolean = True,
                                                  Optional ByVal abDoDatabaseProcessing As Boolean = True,
                                                  Optional ByVal abIncludeSubtypeRelationshipFactTypes As Boolean = True) As Boolean

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

                If abDoDatabaseProcessing Then
                    Call TableValueType.DeleteValueType(Me)
                End If

                Me.Model.RemoveValueType(Me, abDoDatabaseProcessing)

                RaiseEvent RemovedFromModel(abDoDatabaseProcessing)

                Return True

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
            End Try


        End Function

        Public Sub RemoveValueConstraint(ByVal asValueConstraint As String)

            Me.ValueConstraint.Remove(asValueConstraint)

            RaiseEvent ValueConstraintRemoved(asValueConstraint)

        End Sub

        Public Sub renameValueConstraint(ByVal asOldValueConstraint As String, ByVal asNewValueConstraint As String)

            Try
                Dim liIndex = Me.ValueConstraint.IndexOf(asOldValueConstraint)

                Me.ValueConstraint(liIndex) = asNewValueConstraint

                Me.makeDirty()

                RaiseEvent ValueConstraintModified(asOldValueConstraint, asNewValueConstraint)

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Public Overridable Overloads Sub Save(Optional ByVal abRapidSave As Boolean = False)

            '-------------------------------------
            'Saves the ValueType to the database
            '-------------------------------------

            Try
                Dim lrDictionaryEntry As New FBM.DictionaryEntry(Me.Model, Me.Id, pcenumConceptType.ValueType, Me.ShortDescription, Me.LongDescription)
                lrDictionaryEntry = Me.Model.AddModelDictionaryEntry(lrDictionaryEntry, True)
                lrDictionaryEntry.isValueType = True

                Call lrDictionaryEntry.Save()

                If abRapidSave Then
                    Call TableValueType.AddValueType(Me)

                ElseIf Me.isDirty Then

                    If TableValueType.ExistsValueType(Me) Then
                        Call TableValueType.UpdateValueType(Me)
                    Else
                        Try
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

                    Me.isDirty = False

                End If


                '-----------------------------------------------------
                'Save the ValueConstraints (if any) for the ValueType
                '-----------------------------------------------------
                Dim lrValue As String

                For Each lrValue In Me._ValueConstraintList

                    If abRapidSave Then
                        Dim lrConcept As New FBM.Concept(lrValue)
                        Call TableValueTypeValueConstraint.AddValueTypeValueConstraint(Me, lrConcept)
                    Else
                        Dim lrConcept As New FBM.Concept(lrValue)
                        If TableValueTypeValueConstraint.ExistsValueTypeValueConstraint(Me, lrConcept) Then
                            '----------------------------------------------------------------------------
                            'ValueConstraintValue already exists for this ValueType within the database
                            '----------------------------------------------------------------------------
                        Else
                            Call TableValueTypeValueConstraint.AddValueTypeValueConstraint(Me, lrConcept)
                        End If
                    End If
                Next

                'Removes ValueConstraintValues (from the database) that are no longer associated with this ValueType
                Call TableValueTypeValueConstraint.remove_unneeded_value_constraints(Me)

                Me.isDirty = False

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try


        End Sub

        Public Sub SetDataType(ByVal aiNewDataType As pcenumDataType,
                               Optional ByVal aiDataTypeLength As Integer = Nothing,
                               Optional ByVal aiDataTypePrecision As Integer = Nothing,
                               Optional ByVal abBroadcastInterfaceEvent As Boolean = True)

            Me.DataType = aiNewDataType

            If IsSomething(aiDataTypeLength) Then
                Me.DataTypeLength = aiDataTypeLength
            End If

            If IsSomething(aiDataTypePrecision) Then
                Me.DataTypePrecision = aiDataTypePrecision
            End If

            RaiseEvent DataTypeChanged(aiNewDataType)

            If My.Settings.UseClientServer And My.Settings.InitialiseClient And abBroadcastInterfaceEvent Then
                Call prDuplexServiceClient.BroadcastToDuplexService(Viev.FBM.Interface.pcenumBroadcastType.ModelUpdateValueType, Me, Nothing)
            End If

            Me.isDirty = True
            Me.Model.Save()

        End Sub

        Public Sub SetDataTypeLength(ByVal aiNewDataTypeLength As Integer, _
                                     Optional ByVal abBroadcastInterfaceEvent As Boolean = True)

            Me.DataTypeLength = aiNewDataTypeLength
            RaiseEvent DataTypeLengthChanged(aiNewDataTypeLength)

            If My.Settings.UseClientServer And My.Settings.InitialiseClient And abBroadcastInterfaceEvent Then
                Call prDuplexServiceClient.BroadcastToDuplexService(Viev.FBM.Interface.pcenumBroadcastType.ModelUpdateValueType, Me, Nothing)
            End If

            Me.isDirty = True
            Call Me.Model.MakeDirty()
        End Sub

        Public Sub SetDataTypePrecision(ByVal aiNewDataTypePrecision As Integer,
                                        Optional ByVal abBroadcastInterfaceEvent As Boolean = True)

            Me.DataTypePrecision = aiNewDataTypePrecision
            RaiseEvent DataTypePrecisionChanged(aiNewDataTypePrecision)

            If My.Settings.UseClientServer And My.Settings.InitialiseClient And abBroadcastInterfaceEvent Then
                Call prDuplexServiceClient.BroadcastToDuplexService(Viev.FBM.Interface.pcenumBroadcastType.ModelUpdateValueType, Me, Nothing)
            End If

            Me.isDirty = True
            Call Me.Model.MakeDirty()

        End Sub

        Public Overrides Sub SetName(ByVal asNewName As String, Optional ByVal abBroadcastInterfaceEvent As Boolean = True)

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

                    Call Me.SwitchConcept(New FBM.Concept(asNewName, True), pcenumConceptType.ValueType)

                    '------------------------------------------------------------------------------------------
                    'Update the Model(database) immediately. There is no choice. The reason why
                    '  is because the (in-memory) key is changing, so the database must be updated to 
                    '  reflect the new key, otherwise it will not be possible to Update an existing ValueType.
                    '------------------------------------------------------------------------------------------
                    Call TableValueType.ModifyKey(Me, asNewName)

                    If My.Settings.UseClientServer And My.Settings.InitialiseClient And abBroadcastInterfaceEvent Then
                        Call prDuplexServiceClient.BroadcastToDuplexService(Viev.FBM.Interface.pcenumBroadcastType.ModelUpdateValueType, Me, Nothing)
                    End If

                    Me.Id = asNewName
                    Me.Name = asNewName
                    Call TableValueType.UpdateValueType(Me) 'Sets the new Name, because Id is the same as Name and the database's Name hasn't been updated yet.

                    '-------------------------------------------------------
                    'Update all of the respective ValueConstraint records
                    '  in the database as well
                    '-------------------------------------------------------
                    Call TableValueTypeValueConstraint.ModifyKey(Me, asNewName)

                    Me.Model.MakeDirty()

                    RaiseEvent updated()
                    Call Me.RaiseEventNameChanged(asNewName)

                    '------------------------------------------------------------------------------------
                    'Must save the Model because Roles that reference the ValueType must be saved.
                    '  NB If Roles are saved, the FactType must be saved. If the FactType is saved,
                    '  the RoleGroup's references (per Role) must be saved. A Role within the RoleGroup 
                    '  may reference another FactType, so that FactType must be saved...etc.
                    '  i.e. It's easier and safer to simply save the whole model.
                    '------------------------------------------------------------------------------------
                    Dim larRole = From Role In Me.Model.Role _
                                  Where Role.JoinedORMObject Is Me _
                                  Select Role

                    For Each lrRole In larRole
                        lrRole.makeDirty()
                        lrRole.FactType.makeDirty()
                        lrRole.FactType.Save()
                    Next

                    '====================================================================================
                    'RDS
                    Dim lsColumnName As String = ""

                    For Each lrColumn In Me.Model.RDS.getColumnsThatReferenceValueType(Me)

                        If lrColumn.Role.FactType.Id = lrColumn.ActiveRole.FactType.Id Then
                            Call lrColumn.setName(lrColumn.Role.GetAttributeName)
                        Else
                            lsColumnName = Me.Id
                            lsColumnName = Viev.Strings.MakeCapCamelCase(Viev.Strings.RemoveWhiteSpace(lsColumnName))
                            Call lrColumn.setName(lsColumnName)
                        End If
                    Next

                    '-------------------------------------------------------------
                    'To make sure all the FactData and FactDataInstances/Pages are saved for RDS
                    Me.Model.Save()

                End If 'Me.Id <> asNewName



            Catch ex As Exception
                Dim lsMessage As String
                lsMessage = "Error: tValueType.SetName"
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Sub SetValueConstraint(ByVal arValueConstraint As Viev.Strings.StringCollection)

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

            'VM-20180319-Commented out, because was calling SetName twice/again when the Name of the EntityType was simply changed.
            'Me.SetName(Me.Concept.Symbol)

            'VM-20180319-Dummy Code to trap the debug stoppoint, to see if removing the above affects anything
            Dim liDummyCodeInd As Integer = 0

        End Sub


        Public Sub ClearModelErrors() Implements iValidationErrorHandler.ClearModelErrors
            Me.ModelError.Clear()
        End Sub

        Public Sub AddModelError(ByRef arModelError As ModelError) Implements iValidationErrorHandler.AddModelError

            Me.ModelError.Add(arModelError)
            RaiseEvent ModelErrorAdded(arModelError)

        End Sub

        Public Sub SetIsIndependent(abNewIsIndependent As Boolean, abBroadcastInterfaceEvent As Boolean) Implements iFBMIndependence.SetIsIndependent

            Me.IsIndependent = abNewIsIndependent

            Me.isDirty = True
            Me.Model.MakeDirty(False, False)

            RaiseEvent IsIndependentChanged(abNewIsIndependent)

        End Sub

        Public Sub setInstance(ByVal asOldValue As String, ByVal asNewValue As String)

            Try

                If Me.Instance.IndexOf(asOldValue) >= 0 Then

                    Me.Instance.Item(Me.Instance.IndexOf(asOldValue)) = asNewValue

                ElseIf Not Me.Instance.Contains(asNewValue) Then
                    Me.Instance.Add(asNewValue)
                End If

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

    End Class

End Namespace