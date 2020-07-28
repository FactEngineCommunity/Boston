Imports System.Xml.Serialization
Imports System.Reflection

Namespace FBM
    <Serializable()> _
    Public Class Fact
        Inherits FBM.ModelObject
        Implements IEquatable(Of FBM.Fact)
        Implements ICloneable
        Implements FBM.iValidationErrorHandler
        'Implements IXmlSerializable

        <XmlIgnore()> _
        Public FactType As FBM.FactType

        Private _Data As New List(Of FBM.FactData)
        Public Property Data As List(Of FBM.FactData)
            Get
                Return Me._Data
            End Get
            Set(value As List(Of FBM.FactData))
                Me._Data = value
                If Me.Model.Loaded Then Call Me.makeDirty()
            End Set
        End Property

        <XmlIgnore()> _
        Public DictionarySet As New Dictionary(Of String, String)

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

        Public Event ModelErrorAdded(ByRef arModelError As ModelError) Implements iValidationErrorHandler.ModelErrorAdded

        Public Event Deleted()

        Public Sub New()
            '--------------------
            'Default Constructor
            '--------------------
            Me.Symbol = System.Guid.NewGuid.ToString
            Me.Id = Me.Symbol
            Me.ConceptType = pcenumConceptType.Fact
        End Sub

        Public Sub New(ByRef arFactType As FBM.FactType, Optional ByVal abMakeDirty As Boolean = False)

            Me.New()
            Me.Model = arFactType.Model
            Me.FactType = arFactType
            Me.isDirty = abMakeDirty

        End Sub

        Public Sub New(ByVal asFactId As String, ByRef arFactType As FBM.FactType)

            Call Me.New()
            Me.Model = arFactType.Model
            Me.Id = Trim(asFactId)
            Me.Symbol = Me.Id
            Me.FactType = arFactType

        End Sub

        Public Shadows Function Equals(ByVal other As FBM.Fact) As Boolean Implements System.IEquatable(Of FBM.Fact).Equals

            Dim lr_data As FBM.FactData
            Dim lbFound As Boolean = False

            If Me.FactType.Id = other.FactType.Id Then
                For Each lr_data In Me.Data
                    If other.Data.Find(AddressOf lr_data.EqualsByRoleIdData) Is Nothing Then
                        lbFound = False
                        Exit For
                    Else
                        lbFound = True
                    End If
                Next
                Return lbFound
            Else
                Return False
            End If

        End Function

        Public Shadows Function EqualsByData(ByVal other As FBM.Fact) As Boolean

            Dim lrFactData As FBM.FactData
            Dim lbFound As Boolean = False

            If Me.FactType.Id = other.FactType.Id Then
                For Each lrFactData In other.Data
                    If Me.Data.Find(AddressOf lrFactData.EqualsByRoleIdData) Is Nothing Then
                        lbFound = False
                        Return False
                    Else
                        lbFound = True
                    End If
                Next
                Return lbFound
            Else
                Return False
            End If

        End Function

        Public Shadows Function EqualsById(ByVal other As FBM.Fact) As Boolean

            If Me.Id = other.Id Then
                Return True
            Else
                Return False
            End If

        End Function

        Public Shadows Function EqualsBySymbol(ByVal other As FBM.Fact) As Boolean

            If Me.Symbol = other.Symbol Then
                Return True
            Else
                Return False
            End If

        End Function

        Public Overridable Overloads Function Clone(ByRef arFactType As FBM.FactType, ByVal abSetData As Boolean) As Object

            Dim lrFact As New FBM.Fact
            Dim lrFactData As FBM.FactData

            With Me
                lrFact.Model = .Model
                lrFact.Id = .Id
                lrFact.Name = .Name
                lrFact.Symbol = .Symbol
                lrFact.FactType = New FBM.FactType
                lrFact.FactType = arFactType
                lrFact.isDirty = True

                For Each lrFactData In Me.Data
                    lrFact.Data.Add(lrFactData.Clone(lrFact, abSetData))
                Next

            End With

            Return lrFact

        End Function

        Public Overloads Function Clone(ByRef arModel As FBM.Model, ByRef arFactType As FBM.FactType) As Object

            Dim lrFact As New FBM.Fact
            Dim lrFactData As FBM.FactData

            Try

                With Me                    
                    lrFact.Model = arModel
                    lrFact.Id = .Id
                    lrFact.Name = .Name
                    lrFact.Symbol = .Symbol
                    lrFact.FactType = New FBM.FactType
                    lrFact.FactType = arFactType
                    lrFact.isDirty = True

                    Dim lrDictionaryEntry As New FBM.DictionaryEntry(arModel, lrFact.Id, pcenumConceptType.Value)
                    Call arModel.AddModelDictionaryEntry(lrDictionaryEntry)

                    For Each lrFactData In .Data
                        Dim lrClonedFactData As New FBM.FactData
                        lrClonedFactData = lrFactData.Clone(arModel, lrFact, arFactType)
                        lrFact.Data.Add(lrClonedFactData)
                        lrClonedFactData.Role.Data.Add(lrClonedFactData)
                    Next
                End With

                Return lrFact
            Catch ex As Exception
                Dim lsMessage As String = ""

                lsMessage = "Error: tFact.Clone: " & vbCrLf & vbCrLf & ex.Message
                Call prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)

                Return lrFact
            End Try

        End Function

        Public Shadows Function CloneInstance(ByRef arPage As FBM.Page,
                                              Optional ByVal abAddToPage As Boolean = False,
                                              Optional ByVal abMakeFactDataDirty As Boolean = False) As FBM.FactInstance

            Dim lrFactInstance As New FBM.FactInstance
            Dim lrFactData As FBM.FactData

            Try
                With Me
                    lrFactInstance.Model = arPage.Model
                    lrFactInstance.Page = arPage
                    lrFactInstance.Fact = Me
                    lrFactInstance.Id = .Id
                    lrFactInstance.FactType = arPage.FactTypeInstance.Find(Function(x) x.Id = .FactType.Id)

                    For Each lrFactData In .Data
                        lrFactInstance.Data.Add(lrFactData.CloneInstance(arPage, lrFactInstance, abMakeFactDataDirty))
                    Next

                    If abAddToPage Then
                        arPage.FactInstance.Add(lrFactInstance)
                    End If

                End With

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

            Return lrFactInstance

        End Function

        '''' <summary>
        ''''Serializes all public, private and public fields except the one 
        ''''  which are the hidden fields for the eventhandlers
        '''' </summary>
        '''' <remarks></remarks>
        '''' 
        'Private Sub WriteXML(ByVal writer As XmlWriter) Implements IXmlSerializable.WriteXml

        '    Dim lrRoleData As FBM.FactData

        '    writer.WriteAttributeString("ConceptType", Me.ConceptType.ToString)
        '    writer.WriteAttributeString("Id", Me.Id)

        '    For Each lrRoleData In Me.Data
        '        writer.WriteElementString("RoleData" , 
        '    Next
        'End Sub

        'Public Function GetSchema() As System.Xml.Schema.XmlSchema Implements System.Xml.Serialization.IXmlSerializable.GetSchema
        '    Return Nothing
        'End Function

        'Public Sub ReadXml(ByVal reader As System.Xml.XmlReader) Implements System.Xml.Serialization.IXmlSerializable.ReadXml

        'End Sub

        Public Overrides Function CanSafelyRemoveFromModel() As Boolean

            Return False
        End Function

        Public Function DoesFactMeetRoleConstraintsOfFactType(ByRef arModelErrors As List(Of FBM.ModelError)) As Boolean


            If 1 = 2 Then
                Return False
            End If

            Return True

        End Function

        Public Function EnumerateAsBracketedFact() As String

            Dim lrRole As FBM.Role
            Dim lrRoleGroup As New List(Of FBM.Role)
            Dim lrFactData As FBM.FactData
            Dim liInd As Integer = 0

            Try
                EnumerateAsBracketedFact = "{"

                For Each lrRole In Me.FactType.RoleGroup
                    lrFactData = New FBM.FactData
                    lrFactData.Role = lrRole
                    lrFactData = Me.Data.Find(AddressOf lrFactData.EqualsByRole)
                    Select Case lrRole.TypeOfJoin
                        Case Is = pcenumRoleJoinType.EntityType
                            Dim lrJoinedEntityType As FBM.EntityType
                            lrJoinedEntityType = lrRole.JoinedORMObject
                            If lrJoinedEntityType.HasCompoundReferenceMode Then
                                For Each lsEntityTypeInstance In lrJoinedEntityType.Instance

                                    Dim lsEnumeratedIdentity As String
                                    lsEnumeratedIdentity = lrJoinedEntityType.EnumerateInstance(lsEntityTypeInstance)
                                    EnumerateAsBracketedFact &= lsEnumeratedIdentity
                                Next
                            Else
                                EnumerateAsBracketedFact &= lrFactData.Data
                            End If
                        Case Else
                            EnumerateAsBracketedFact &= lrFactData.Data
                    End Select

                    liInd += 1
                    If (liInd > 0) And (liInd < Me.Data.Count) Then
                        EnumerateAsBracketedFact &= ","
                    End If
                Next

                EnumerateAsBracketedFact &= "}"
            Catch ex As Exception
                Dim lsMessage As String = ""

                lsMessage = "Error: tFact.EnumerateAsBracketedFact: " & vbCrLf & vbCrLf & ex.Message
                Call prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
                Return ""
            End Try

        End Function

        ''' <summary>
        ''' Enumerates the Data of the Fact as a key value
        ''' </summary>
        ''' <returns>String representing a key of the data within the Fact</returns>
        ''' <remarks>Used in processing ORMQL statements. e.g. Where DISTINCT keyword is used in a SELECT statement</remarks>
        Public Overridable Function EnumerateDataAsKey(ByVal aasKeyColumnOrder As List(Of String)) As String

            Dim lsColumnName As String
            Dim lsKey As String = ""

            For Each lsColumnName In aasKeyColumnOrder
                lsKey &= Me.GetFactDataByRoleName(lsColumnName).Data
            Next

            Return lsKey

        End Function

        ''' <summary>
        ''' Returns a Reading for the Fact.
        ''' e.g. "Person has a FirstName"
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetReading() As String

            Dim lsFactReading As String = ""
            Dim lrRole As FBM.Role
            Dim lrFactTypeReading As New FBM.FactTypeReading(Me.FactType)            
            Dim liInd As Integer = 0
            Dim lrRoleData As FBM.FactData
            Dim lrFactType As FBM.FactType
            Dim lrFact As FBM.Fact
            Dim lsRoleData As String = ""

            lrFactTypeReading = Me.FactType.FindSuitableFactTypeReading

            For Each lrRole In Me.FactType.RoleGroup
                lsFactReading &= " " & lrRole.JoinedORMObject.Id & " ('"
                lrRoleData = New FBM.FactData(lrRole, New FBM.Concept(Me.Data(liInd).Data), Me)
                lsRoleData = Me.Data.Find(AddressOf lrRoleData.EqualsByRole).Data
                Select Case lrRole.JoinedORMObject.ConceptType
                    Case Is = pcenumConceptType.FactType
                        lrFactType = lrRole.JoinedORMObject
                        lrFact = New FBM.Fact(lsRoleData, lrFactType)
                        lsFactReading &= lrFactType.Fact.Find(AddressOf lrFact.EqualsById).GetReading
                    Case Else
                        lrRoleData = New FBM.FactData(lrRole, New FBM.Concept(Me.Data(liInd).Data), Me)
                        lsFactReading &= Me.Data.Find(AddressOf lrRoleData.EqualsByRole).Data & "')"
                End Select

                If (liInd = 0) Or (liInd < (Me.FactType.RoleGroup.Count - 1)) Then
                    If IsSomething(lrFactTypeReading) Then
                        lsFactReading &= " " & lrFactTypeReading.PredicatePart(liInd).PredicatePartText
                    End If
                End If
                liInd += 1
            Next

            Return lsFactReading

        End Function

        Public Overridable Shadows Sub makeDirty()

            Me.FactType.isDirty = True
            For Each lrFactData In Me.Data
                lrFactData.isDirty = True
            Next
            Me.isDirty = True
        End Sub

        Public Function GetFactDataByRoleId(ByVal asRoleId As String) As FBM.FactData

            Dim lrFactData As FBM.FactData = Nothing
            Dim larFactData() As FBM.FactData
            Dim liInd As Integer = 0
            Dim lbFoundFactData As Boolean = False

            Try
                'larFactData = Me.Data.ToArray
                lrFactData = Me.Data.Find(Function(x) x.Role.Id = asRoleId) ' ToArray

                'For liInd = 1 To larFactData.Count
                '    If larFactData(liInd - 1).Role.Id = asRoleId Then
                '        lrFactData = larFactData(liInd - 1)
                '        lbFoundFactData = True
                '        Exit For
                '    End If
                'Next

                If lrFactData Is Nothing Then 'Not lbFoundFactData Then
                    Throw New Exception("Function called for Fact with no matching Role.")
                End If

                Return lrFactData

            Catch ex As Exception
                Dim lsMessage As String
                lsMessage = "Error: tFact.GetFactDataByRoleId"
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)

                Return Nothing
            End Try

        End Function

        Public Function GetFactDataByRoleName(ByVal asRoleName As String) As FBM.FactData

            Dim lrFactData As New FBM.FactData
            Dim larFactData() As FBM.FactData
            Dim liInd As Integer = 0
            Dim lbFoundFactData As Boolean = False

            Try

                larFactData = Me.Data.ToArray

                For liInd = 1 To larFactData.Count
                    If larFactData(liInd - 1).Role.Name = asRoleName Then
                        lrFactData = larFactData(liInd - 1)
                        lbFoundFactData = True
                        Exit For
                    End If
                Next

                If Not lbFoundFactData Then
                    Throw New Exception("Function called for Fact with no matching Role.")
                End If

                Return lrFactData

            Catch ex As Exception
                Dim lsMessage As String
                lsMessage = "Error: tFact.GetFactDataByRoleName"
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)

                Return Nothing
            End Try

        End Function

        ''' <summary>
        ''' If not DeleteAll, then the ConceptInstance/s for the Fact need to be removed from the database elsewhere.
        '''   The reason for this is that some ConceptInstances are Values for a Role, and may not belong to the Fact being deleted.
        ''' </summary>
        ''' <param name="abForceRemoval"></param>
        ''' <param name="abCheckForErrors"></param>
        ''' <param name="abDoDatabaseProcessing"></param>
        ''' <param name="abDeleteAll"></param>
        ''' <returns></returns>
        Public Shadows Function RemoveFromModel(Optional ByVal abForceRemoval As Boolean = False,
                                                  Optional ByVal abCheckForErrors As Boolean = True,
                                                  Optional ByVal abDoDatabaseProcessing As Boolean = True,
                                                  Optional ByVal abDeleteAll As Boolean = False) As Boolean
            Dim lrFactData As FBM.FactData
            Dim lrFactType As New FBM.FactType
            Dim larFactData As New System.Collections.Generic.List(Of FBM.FactData)

            For Each lrFactData In Me.Data
                larFactData.Add(lrFactData)
            Next

            lrFactType = Me.FactType
            lrFactType.Fact.Remove(Me)

            If abDoDatabaseProcessing Then
                Call TableFact.DeleteFact(Me)
            End If

            For Each lrFactData In larFactData
                Call lrFactData.RemoveFromModel(False, False,, abDeleteAll)
            Next

            Dim lrDictionaryEntry As New FBM.DictionaryEntry(Me.Model, Me.Symbol, pcenumConceptType.Fact)
            Me.Model.RemoveDictionaryEntry(lrDictionaryEntry, abDoDatabaseProcessing)

        End Function

        Public Overloads Sub Save(Optional ByVal abRapidSave As Boolean = False)

            Dim lrFactData As FBM.FactData

            Try
                If abRapidSave Then
                    '-----------
                    'Fact
                    Call TableFact.AddFact(Me)

                ElseIf Me.isDirty Then
                    '-----------
                    'Fact                
                    If TableFact.ExistsFact(Me) Then
                        Call TableFact.UpdateFact(Me)
                    Else
                        Call TableFact.AddFact(Me)
                    End If

                    Me.isDirty = False
                End If

                '-----------
                'FactData
                For Each lrFactData In Me.Data
                    lrFactData.Save(abRapidSave)
                Next

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

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
