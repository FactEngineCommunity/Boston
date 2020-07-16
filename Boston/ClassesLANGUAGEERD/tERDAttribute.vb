Imports System.Reflection
Imports System.ComponentModel

Namespace ERD

    <Serializable()> _
    Public Class Attribute
        Inherits FBM.FactDataInstance
        Implements IEquatable(Of ERD.Attribute)

        Public Entity As ERD.Entity

        <CategoryAttribute("Name"), _
        [ReadOnly](True), _
        DefaultValueAttribute(GetType(String), ""), _
        DescriptionAttribute("A unique Name for the model object.")> _
        Public Overrides Property Name() As String
            Get
                Return Me._AttributeName
            End Get
            Set(ByVal value As String)
                '------------------------------------------------------
                'See Me.SetName for management of Me.Id and Me.Symbol
                '------------------------------------------------------
                _Name = value
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
        Public Overridable Property DataType() As String
            Get
                Return Me._DataType
            End Get
            Set(ByVal value As String)
                Me._DataType = value
            End Set
        End Property

        Public _PartOfPrimaryKey As Boolean = False
        Public Overridable Property PartOfPrimaryKey() As Boolean
            Get
                Dim lbIsPartOfPrimaryKey = (From Index In Me.Column.Index
                                            Where Index.IsPrimaryKey
                                            Select Index).Count > 0
                Return lbIsPartOfPrimaryKey
            End Get
            Set(ByVal value As Boolean)
                Me._PartOfPrimaryKey = value
            End Set
        End Property

        Public _Mandatory As Boolean = False
        Public Overridable Property Mandatory() As Boolean
            Get
                Return Me._Mandatory
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

        Public OrdinalPosition As Integer = 1 'The Ordinal Position of the Attribute in the set of Attributes for an Entity

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

        Public Sub RefreshShape(Optional ByVal aoChangedPropertyItem As PropertyValueChangedEventArgs = Nothing)

            Try
                Dim lsPartOfPrimaryKey As String = ""
                Dim lsMandatory As String = ""

                If Me.PartOfPrimaryKey Then
                    lsPartOfPrimaryKey = "#"
                Else
                    lsPartOfPrimaryKey = ""
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

                Me.Cell.Table.ResizeToFitText(True)


                Me.Page.Diagram.Invalidate()

                If IsSomething(aoChangedPropertyItem) Then
                    Select Case aoChangedPropertyItem.ChangedItem.PropertyDescriptor.Name
                        Case Is = "Name"

                            '-----------------------------------------------------------------------------------------------------------------
                            'If the Attribute is not part of a Relation, the the Attribute has a corresponding ValueType at the Model level.
                            '  The Name/Id of that ValueType must be updated as well.
                            '  This is done automatically when the Concept of the Attribute is updated/switchec.
                            '-----------------------------------------------------------------------------------------------------------------

                    End Select
                End If

                Me.Entity.TableShape.ResizeToFitText(False)

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Private Sub ResonsibleFactType_RemovedFromModel() Handles ResponsibleFactType.RemovedFromModel

            Me.Entity.TableShape.DeleteRow(Me.OrdinalPosition - 1)

            'Call Me.Page.DeleteERDElementsByAttributeId(Me.Id)

        End Sub

        Private Sub ResponsibleRole_MandatoryChanged(abMandatoryStatus As Boolean) Handles ResponsibleRole.MandatoryChanged

            Dim lsSQLQuery As String = ""
            Dim lrORMRecordset As New ORMQL.Recordset
            Dim lrFact As New FBM.Fact

            Try
                Me.Mandatory = abMandatoryStatus

                If abMandatoryStatus = True Then

                    lsSQLQuery = "SELECT COUNT(*)"
                    lsSQLQuery &= " FROM CoreIsMandatory"
                    lsSQLQuery &= " WHERE IsMandatory = '" & Me.Id & "'"

                    lrORMRecordset = Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                    If lrORMRecordset("Count").Data = 0 Then

                        lsSQLQuery = "INSERT INTO CoreIsMandatory (IsMandatory)"
                        lsSQLQuery &= " VALUES ("
                        lsSQLQuery &= " '" & Me.Data & "'"
                        lsSQLQuery &= " )"

                        lrFact = Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)

                        lsSQLQuery = "ADD FACT '" & lrFact.Id & "'"
                        lsSQLQuery &= " TO " & pcenumCMMLRelations.CoreIsMandatory.ToString
                        lsSQLQuery &= " ON PAGE '" & Me.Page.Name & "'"

                        Call Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)
                    Else
                        lsSQLQuery = "ADD FACT '" & lrORMRecordset.Facts(0).Id & "'"
                        lsSQLQuery &= " TO " & pcenumCMMLRelations.CoreIsMandatory.ToString
                        lsSQLQuery &= " ON PAGE '" & Me.Page.Name & "'"

                        Call Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)
                    End If

                Else
                    lsSQLQuery = "DELETE FROM CoreIsMandatory"
                    lsSQLQuery &= " WHERE IsMandatory = '" & Me.Id & "'"

                    Call Me.Model.ORMQL.ProcessORMQLStatement(lsSQLQuery)
                End If

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

            Call Me.RefreshShape()

        End Sub


        Private Sub Attribute_ConceptSwitched(ByRef arConcept As FBM.Concept) Handles Me.ConceptSwitched

            Me.AttributeName = Me.Concept.Symbol

            Call Me.RefreshShape()

        End Sub

        Private Sub Attribute_ConceptSymbolUpdated() Handles Me.ConceptSymbolUpdated

            Me.AttributeName = Me.Concept.Symbol

            Call Me.RefreshShape()

        End Sub

    End Class

End Namespace
