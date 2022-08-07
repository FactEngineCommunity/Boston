Imports System.Reflection
Imports System.ComponentModel

Namespace ERD

    <Serializable()> _
    Public Class Attribute
        Inherits FBM.FactDataInstance
        Implements IEquatable(Of ERD.Attribute)

        Public Entity As Object

        <CategoryAttribute("Name"),
        [ReadOnly](False),
        DefaultValueAttribute(GetType(String), ""),
        DescriptionAttribute("A unique Name for the Attribute.")>
        Public Overrides Property Name() As String
            Get
                Return Me._AttributeName
            End Get
            Set(ByVal value As String)
                '------------------------------------------------------
                'See Me.SetName for management of Me.Id and Me.Symbol
                '------------------------------------------------------
                _AttributeName = value
            End Set
        End Property

        Public _AttributeName As String = ""
        Public Property AttributeName() As String
            Get
                Return Me._AttributeName
            End Get
            Set(ByVal value As String)
                Me._AttributeName = value
            End Set
        End Property

        Public Multiplicity As New UML.Multiplicity
        Public IsIdentityIdentifier As Boolean = False
        Public PreferredIdentityIdentifier As New List(Of String)

        Public WithEvents ModelFactType As FBM.FactType

        Public _DataType As String = ""
        <CategoryAttribute("Attribute"),
        [ReadOnly](True),
        DefaultValueAttribute(GetType(String), ""),
        DescriptionAttribute("The data type of the Attribute.")>
        Public Overridable Property DataType() As String
            Get
                If Me.Column IsNot Nothing Then
                    Return Me.Column.getMetamodelDataType.ToString '_DataType
                Else
                    Return "Unkown, Column not set"
                End If
            End Get
            Set(ByVal value As String)
                Me._DataType = value
            End Set
        End Property

        Private _IsDerivationParameter As Boolean = False
        <CategoryAttribute("Attribute"),
        [ReadOnly](False),
        DefaultValueAttribute(GetType(String), ""),
        DescriptionAttribute("True if the Attribute is a Parameter to a Derived Fact Type.")>
        Public Overridable Property IsDerivationParameter() As Boolean
            Get
                Return _IsDerivationParameter
            End Get
            Set(ByVal value As Boolean)
                Me._IsDerivationParameter = value
            End Set
        End Property

        Public _PartOfPrimaryKey As Boolean = False
        Public Overridable Property PartOfPrimaryKey() As Boolean
            Get
                If Me.Column IsNot Nothing Then
                    Return Me.Column.isPartOfPrimaryKey
                Else
                    Dim lbIsPartOfPrimaryKey = (From Index In Me.Column.Index
                                                Where Index.IsPrimaryKey
                                                Select Index).Count > 0
                    Return lbIsPartOfPrimaryKey
                End If

            End Get
            Set(ByVal value As Boolean)
                Me._PartOfPrimaryKey = value
            End Set
        End Property

        Public _Mandatory As Boolean = False
        Public Overridable Property Mandatory() As Boolean
            Get
                Return Me.Column.IsMandatory
            End Get
            Set(ByVal value As Boolean)
                Me._Mandatory = value
            End Set
        End Property

        ''' <summary>
        ''' Stores the (if any) Attribute referenced by the Attribute in the same or another Entity.
        ''' </summary>
        ''' <remarks></remarks>
        Public ReferencesAttribute As ERD.Attribute

        Public ReadOnly Property OrdinalPosition As Integer
            Get
                If Me.Column Is Nothing Then
                    Return 1
                Else
                    Return Me.Column.OrdinalPosition
                End If
            End Get
        End Property

        ''' <summary>
        ''' The FactType (at the ORMModel level) responsible for the existance of this Attribute.
        '''   NB If the FactType, at the ORMModel level, is removed from the ORMModel, then this Attribute must similarly be removed from its Entity.
        '''   Because this class/object is ultimately drawn on a Page.Diagram, the corresponding drawing object must reflect the removal of the Attribute.
        ''' </summary>
        ''' <remarks></remarks>
        Public WithEvents ResponsibleFactType As FBM.FactType

        ''' <summary>
        ''' The Role (at the ORMModel level) responsible for the existanceof this Attribute.
        ''' </summary>
        ''' <remarks></remarks>
        Public WithEvents ResponsibleRole As FBM.Role

        ''' <summary>
        ''' The Role that ultimately is responsible for the Attribute. Nested ObjectifiedFactTypes referenced by a Role in another ObjectifiedFactType are
        '''   sometime responsible for a Column/Property/Attribute that is not of the ResponsibleRole.
        '''   e.g. See the TimetableBookings Page of the University Model.
        ''' </summary>
        ''' <remarks></remarks>
        Public WithEvents ActiveRole As FBM.Role

        ''' <summary>
        ''' Is the Column from the Supertype Table that is represented by this Attribute/Column, if this Attribute/Column is inherited from a Supertype table.
        ''' </summary>
        Public WithEvents SupertypeColumn As RDS.Column

        ''' <summary>
        ''' The RDS Column that the Attribute relates to.
        ''' </summary>
        ''' <remarks></remarks>
        Public WithEvents Column As RDS.Column = Nothing


        Public Property IsPartOfRelation() As Boolean
            Get
                Return IsSomething(Me.Relation)
            End Get
            Set(ByVal value As Boolean)
                Throw New NotImplementedException("This property is Read Only.")
            End Set
        End Property

        ''' <summary>
        ''' If the Attribute is part of a Relation, the Relation that the Attribute is part of.
        ''' </summary>
        ''' <remarks></remarks>
        Public Relation As ERD.Relation

        Public Overrides Property ModelError() As System.Collections.Generic.List(Of FBM.ModelError)
            Get
                Dim larModelError As New List(Of FBM.ModelError)

                If Me.Column.ActiveRole.JoinsValueType IsNot Nothing Then
                    larModelError.AddRange(Me.Column.ActiveRole.JoinsValueType.ModelError)
                End If

                If Me.Column.getMetamodelDataType = pcenumORMDataType.DataTypeNotSet And larModelError.Count = 0 Then

                    Dim lsErrorMessage = "Data Type Not Specified Error - Value Type: '" & Me.Column.ActiveRole.JoinsValueType.Name & "'."

                    Dim lrModelError = New FBM.ModelError(pcenumModelErrors.DataTypeNotSpecifiedError,
                                                          lsErrorMessage,
                                                          Nothing,
                                                          Me.Column.ActiveRole.JoinsValueType)

                    larModelError.AddUnique(lrModelError)
                End If

                Return larModelError
            End Get
            Set(value As System.Collections.Generic.List(Of FBM.ModelError))
                'Nothing to do here. See getter above.
            End Set
        End Property

        Public Sub New()

            Me.Id = System.Guid.NewGuid.ToString
            Me.Name = Me.Id

        End Sub

        Public Sub New(ByVal asAttributeId As String)

            Me.Id = asAttributeId
            Me.Name = Me.Id

        End Sub

        Public Sub New(ByVal asAttributeId As String, ByRef arEntity As ERD.Entity)

            Me.Id = asAttributeId
            Me.Name = Me.Id
            Me.Entity = arEntity

        End Sub

        Public Shadows Function Equals(ByVal other As Attribute) As Boolean Implements System.IEquatable(Of Attribute).Equals

            If Me.Id = other.Id Then
                Return True
            Else
                Return False
            End If

        End Function

        ''' <summary>
        ''' Used to sort Attributes by Ordinal Position
        ''' </summary>
        ''' <param name="aoA"></param>
        ''' <param name="aoB"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function ComparerOrdinalPosition(ByVal aoA As ERD.Attribute, ByVal aoB As ERD.Attribute) As Integer

            Dim loa As New Object
            Dim lob As New Object

            Try
                Return aoA.OrdinalPosition - aoB.OrdinalPosition

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Function

        Public Overloads Function Clone() As ERD.Attribute

            Dim lrAttribute As New ERD.Attribute

            With Me
                lrAttribute.Name = .Name
                lrAttribute.Mandatory = .Mandatory
                lrAttribute.PartOfPrimaryKey = .PartOfPrimaryKey
            End With

            Return lrAttribute

        End Function

        Public Sub RefreshShape(Optional ByVal aoChangedPropertyItem As PropertyValueChangedEventArgs = Nothing,
                                Optional ByVal asSelectedGridItemLabel As String = "")

            Try
                Dim lsPartOfPrimaryKey As String = ""
                Dim lsMandatory As String = ""

                'CodeSafe
                If Me.Cell Is Nothing Then Exit Sub

                If Me.Entity.RDSTable.HasPrimaryKeyIndex Then
                    If Me.Column.isPartOfPrimaryKey Then
                        lsPartOfPrimaryKey = "#"
                        '20220315-VM-Commented out. If all seems okay then remove completely.
                        'ElseIf Me.Entity.RDSTable.Index.Find(Function(x) x.IsPrimaryKey).Column.Contains(Me.Column) Then
                        '    lsPartOfPrimaryKey = "#"
                    Else
                        lsPartOfPrimaryKey = ""
                    End If
                Else
                    lsPartOfPrimaryKey = ""
                    Try
                        Dim lrTable As RDS.Table = Me.Column.Role.FactType.getCorrespondingRDSTable(Nothing, True)

                        If Me.Column.isPartOfPrimaryKey(lrTable Is Nothing) Then
                            lsPartOfPrimaryKey = "#"
                        End If
                    Catch ex As Exception
                        lsPartOfPrimaryKey = "[Error]"
                    End Try

                End If

                If Me.Mandatory Then
                    lsMandatory = "*"
                Else
                    lsMandatory = "o"
                End If

                If Not Me.Entity.Attribute.Contains(Me) Then
                    Exit Sub
                End If

                Me.Cell.Text = " " & Trim(lsPartOfPrimaryKey & " " & lsMandatory & " " & Me.AttributeName)

                'RDSData
                If Me.Entity.DisplayRDSData Then
                    Me.Cell.Text &= " " & vbTab

                    'Index
                    Dim liInd As Integer = 0
                    For Each lrIndex In Me.Column.Index

                        If liInd > 0 Then Me.Cell.Text &= ","
                        Me.Cell.Text &= lrIndex.IndexQualifier

                        liInd += 1
                    Next

                    'ForeignKey
                    If Me.Column.Relation.FindAll(Function(x) x.OriginTable.Name = Me.Column.Table.Name).Count > 0 Then
                        Me.Cell.Text &= " FK "
                    End If

                    'DataType
                    Me.Cell.Text &= "  " & Me.Column.getMetamodelDataType.ToString

                    'Ordinal Position
                    'Me.Cell.Text &= "  " & Me.OrdinalPosition

                    'DataTypeLength
                    Select Case Me.Column.getMetamodelDataType
                        Case Is = pcenumORMDataType.TextFixedLength
                            Me.Cell.Text &= "(" & Me.Column.getMetamodelDataTypeLength.ToString & ")"
                    End Select
                End If

                If Me.Column.getMetamodelDataType = pcenumORMDataType.DataTypeNotSet Then
                    Me.Cell.TextColor = Color.Red
                Else
                    Me.Cell.TextColor = Color.Black
                End If

                If Me.Cell.Table.Rows.Count > 0 Then
                    Try
                        Me.Cell.Table.ResizeToFitText(True)
                    Catch
                    End Try
                End If

                If Me.Page.Diagram IsNot Nothing Then
                    Me.Page.Diagram.Invalidate()
                End If

                If IsSomething(aoChangedPropertyItem) Then
                    Select Case aoChangedPropertyItem.ChangedItem.PropertyDescriptor.Name
                        Case Is = "Name"
                            '-----------------------------------------------------------------------------------------------------------------
                            'If the Attribute is not part of a Relation, the the Attribute has a corresponding ValueType at the Model level.
                            '  The Name/Id of that ValueType must be updated as well.
                            '  This is done automatically when the Concept of the Attribute is updated/switchec.
                            '-----------------------------------------------------------------------------------------------------------------
                            If Me.Name.Length > 0 Then
                                Call Me.Column.setName(Me.Name)
                            Else
                                Me.AttributeName = Me.Column.Name
                                MsgBox("You can't have a zero length Attribute name.")
                            End If
                        Case Is = "IsDerivationParameter"
                            Call Me.Column.setIsDerivationParameter(Me.IsDerivationParameter)
                    End Select
                End If

                Try
                    Me.Entity.TableShape.ResizeToFitText(False)
                Catch
                    'Not a biggie.
                End Try


            Catch ex As Exception
                    Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Private Sub ResonsibleFactType_RemovedFromModel() Handles ResponsibleFactType.RemovedFromModel

            'CodeSafe
            If Me.Entity.TableShape Is Nothing Then Exit Sub

            Me.Entity.TableShape.DeleteRow(Me.OrdinalPosition - 1)

            'Call Me.Page.DeleteERDElementsByAttributeId(Me.Id)

        End Sub

        Private Sub ResponsibleRole_MandatoryChanged(abMandatoryStatus As Boolean) Handles ResponsibleRole.MandatoryChanged

            Dim lsSQLQuery As String = ""
            Dim lrORMRecordset As New ORMQL.Recordset
            Dim lrFact As New FBM.Fact

            Try
                Me.Mandatory = abMandatoryStatus

                '20220808-VM-Taken care of at the RDS level. Column.SetMandatory
                'If abMandatoryStatus = True Then

                '    lsSQLQuery = "SELECT COUNT(*)"
                '    lsSQLQuery &= " FROM CoreIsMandatory"
                '    lsSQLQuery &= " WHERE IsMandatory = '" & Me.Id & "'"

                '    lrORMRecordset = Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                '    If lrORMRecordset("Count").Data = 0 Then

                '        lsSQLQuery = "INSERT INTO CoreIsMandatory (IsMandatory)"
                '        lsSQLQuery &= " VALUES ("
                '        lsSQLQuery &= " '" & Me.Id & "'"
                '        lsSQLQuery &= " )"

                '        lrFact = Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                '        '20220808-VM-Removed. CMML Pages (E.g. ERD View) no longer use CMML data on the Page itself for Attributes.
                '        'lsSQLQuery = "ADD FACT '" & lrFact.Id & "'"
                '        'lsSQLQuery &= " TO " & pcenumCMMLRelations.CoreIsMandatory.ToString
                '        'lsSQLQuery &= " ON PAGE '" & Me.Page.Name & "'"

                '        'Call Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)
                '        'Else
                '        '    lsSQLQuery = "ADD FACT '" & lrORMRecordset.Facts(0).Id & "'"
                '        '    lsSQLQuery &= " TO " & pcenumCMMLRelations.CoreIsMandatory.ToString
                '        '    lsSQLQuery &= " ON PAGE '" & Me.Page.Name & "'"

                '        '    Call Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)
                '    End If

                'Else
                '    lsSQLQuery = "DELETE FROM CoreIsMandatory"
                '    lsSQLQuery &= " WHERE IsMandatory = '" & Me.Id & "'"

                '    Call Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)
                'End If

                Call Me.RefreshShape()

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Private Sub Column_ActiveRoleChanged() Handles Column.ActiveRoleChanged

            Dim lsSQLQuery As String = ""
            Dim lrRecordset As ORMQL.Recordset
            Dim lrRecordset1 As ORMQL.Recordset

            lsSQLQuery = "SELECT *"
            lsSQLQuery &= " FROM CorePropertyHasActiveRole" '(Property, Role)
            lsSQLQuery &= " WHERE Property = '" & Me.Column.Id & "'"

            lrRecordset = Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

            lsSQLQuery = " SELECT *"
            lsSQLQuery &= "  FROM " & pcenumCMMLRelations.CorePropertyHasActiveRole.ToString
            lsSQLQuery &= " ON PAGE '" & Me.Page.Name & "'"
            lsSQLQuery &= " WHERE Property = '" & Me.Column.Id & "'"

            lrRecordset1 = Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

            If lrRecordset1.EOF Then
                lsSQLQuery = "ADD FACT '" & lrRecordset.CurrentFact.Id & "'"
                lsSQLQuery &= " TO " & pcenumCMMLRelations.CorePropertyHasActiveRole.ToString
                lsSQLQuery &= " ON PAGE '" & Me.Name & "'"

                Call Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)
            End If

            Call Me.RefreshShape()

        End Sub

        Private Sub Column_ContributesToPrimaryKeyChanged(ByVal abContributesToPrimaryKey As Boolean) Handles Column.ContributesToPrimaryKeyChanged

            Me.PartOfPrimaryKey = abContributesToPrimaryKey

            Call Me.RefreshShape()
            Call Me.Entity.RefreshShape

        End Sub

        Private Sub Column_forceRefresh() Handles Column.forceRefresh

            Call Me.RefreshShape()

        End Sub

        Private Sub Column_IndexAdded(ByRef arIndex As RDS.Index) Handles Column.IndexAdded

            Call Me.RefreshShape()

        End Sub

        Private Sub Column_IndexRemoved(ByRef arIndex As RDS.Index) Handles Column.IndexRemoved

            If arIndex.IsPrimaryKey Then
                Me.PartOfPrimaryKey = False
            End If
            Call Me.RefreshShape()

        End Sub

        Private Sub Column_IsMandatoryChanged(ByRef abIsMandatory As Boolean) Handles Column.IsMandatoryChanged

            Me.Mandatory = abIsMandatory

            Call Me.RefreshShape()

        End Sub

        Private Sub Column_NameChanged(asNewName As String) Handles Column.NameChanged

            Me.AttributeName = asNewName

            If Me.Cell IsNot Nothing Then
                Call Me.RefreshShape()
            End If

        End Sub


        Private Sub Attribute_ConceptSwitched(ByRef arConcept As FBM.Concept) Handles Me.ConceptSwitched

            Me.AttributeName = Me.Concept.Symbol

            Call Me.RefreshShape()

        End Sub

        Private Sub Attribute_ConceptSymbolUpdated() Handles Me.ConceptSymbolUpdated
            Try
                Me.AttributeName = Me.Concept.Symbol

                Call Me.RefreshShape()
            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
            End Try
        End Sub

        Private Sub Column_OrdinalPositionChanged(aiNewOrdinalPosition As Integer) Handles Column.OrdinalPositionChanged

            Try
                'Me.OrdinalPosition = aiNewOrdinalPosition '20210422-VM-Removed because made ReadOnly Property returning Attribute.Column.OrdinalPosition

                If Me.Entity.Name = Me.Column.Table.Name Then
                    'Column may be moved to a new Table/Entity, so may not exist in Me.Entity.
                    Call Me.Entity.RefreshShape()
                Else
                    Try
                        Call Me.Entity.RefreshShape()
                    Catch ex As Exception
                        Dim lsMessage As String
                        Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                        lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                        lsMessage &= vbCrLf & vbCrLf & ex.Message
                        prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
                    End Try
                End If
            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Private Sub Column_IsDerivationParameterChanged(abIsDerivationParameter As Boolean) Handles Column.IsDerivationParameterChanged

            Try
                Me.IsDerivationParameter = abIsDerivationParameter

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Private Sub SupertypeColumn_OrdinalPositionChanged(aiNewOrdinalPosition As Integer) Handles SupertypeColumn.OrdinalPositionChanged

            Try
                'Me.OrdinalPosition = aiNewOrdinalPosition '20210422-VM-Removed because made ReadOnly Property returning Attribute.Column.OrdinalPosition

                If Me.Entity.Name = Me.Column.Table.Name Then
                    'Column may be moved to a new Table/Entity, so may not exist in Me.Entity.
                    Call Me.Entity.RefreshShape()
                Else
                    Try
                        Call Me.Entity.RefreshShape()
                    Catch ex As Exception
                        Dim lsMessage As String
                        Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                        lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                        lsMessage &= vbCrLf & vbCrLf & ex.Message
                        prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
                    End Try
                End If
            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Private Sub Column_DataTypeChanged() Handles Column.DataTypeChanged

            Try
                Call Me.RefreshShape()

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
