Imports MindFusion.Diagramming
Imports System.Xml.Serialization
Imports System.Reflection

Namespace FBM
    <Serializable()> _
    Public Class FactTable
        Inherits FBM.PageObject
        Implements FBM.iTableNodePageObject

        <XmlIgnore()> _
        Public FactTypeInstance As FBM.FactTypeInstance

        <XmlIgnore()> _
        Public SelectedRow As Integer = 0


        <NonSerialized(), _
        XmlIgnore()> _
        Public Shadows WithEvents Page As FBM.Page

        <NonSerialized()>
        Public Shadows WithEvents TableShape As New TableNode

        Public Shadows Property X As Integer Implements FBM.iPageObject.X
        Public Shadows Property Y As Integer Implements FBM.iPageObject.Y

        Public Visible As Boolean = False

        Public Sub New()
            '---------------------
            'Default Constructor
            '---------------------
            Me.ConceptType = pcenumConceptType.FactTable
            Me.SelectedRow = 0
        End Sub

        Public Sub New(ByRef arPage As FBM.Page, ByRef arFactTypeInstance As FBM.FactTypeInstance)

            Me.Model = arFactTypeInstance.Model
            Me.Page = arPage
            Me.ConceptType = pcenumConceptType.FactTable
            Me.FactTypeInstance = arFactTypeInstance
            Me.TableShape.ColumnCount = arFactTypeInstance.Arity
            Me.Id = Me.FactTypeInstance.Id
            Me.Symbol = Me.FactTypeInstance.Id
            Me.Name = Me.FactTypeInstance.Id
            Me.SelectedRow = 0

        End Sub

        Public Overloads Function Clone(ByRef arPage As FBM.Page) As Object

            Return New FBM.FactTable

        End Function

        Public Sub DisplayAndAssociate(ByRef arFactTypeInstance As FBM.FactTypeInstance, ByVal abDisplayFactTable As Boolean)

            '--------------------------------------------------
            'FactTable
            '--------------------------------------------------
            Dim liYDisplacement As Integer = 0
            Dim lsMessage As String

            Try

                Me.FactTypeInstance = New FBM.FactTypeInstance
                Me.FactTypeInstance = arFactTypeInstance

                If Me.FactTypeInstance.IsObjectified Then
                    liYDisplacement = 20
                Else
                    liYDisplacement = 15
                End If

                If Me.X = 0 Then
                    Me.X = Me.FactTypeInstance.Shape.Bounds.X
                End If
                If Me.Y = 0 Then
                    Me.Y = Me.FactTypeInstance.Shape.Bounds.Y + liYDisplacement
                End If
                Dim loFactTable As TableNode = Me.Page.Diagram.Factory.CreateTableNode(Me.X, Me.Y, Me.FactTypeInstance.Shape.Bounds.Width, 30)

                Call loFactTable.ZBottom()

                loFactTable.Tag = New FBM.FactTable
                loFactTable.Tag = Me
                Me.TableShape = loFactTable
                Me.X = Me.TableShape.Bounds.X
                Me.Y = Me.TableShape.Bounds.Y

                loFactTable.AllowIncomingLinks = False
                loFactTable.AllowOutgoingLinks = False
                loFactTable.ColumnCount = Me.FactTypeInstance.Arity
                If loFactTable.ColumnCount > 0 Then
                    loFactTable.Columns(0).ColumnStyle = ColumnStyle.AutoWidth
                End If
                loFactTable.RowCount = 3
                loFactTable.Style = TableStyle.Rectangle
                loFactTable.Caption = ""
                loFactTable.CellFrameStyle = CellFrameStyle.Simple
                loFactTable.Pen.Color = Color.LightGray
                loFactTable.Scrollable = True
                loFactTable.Brush = New MindFusion.Drawing.SolidBrush("#FFFDFDFD")
                loFactTable.AttachTo(Me.FactTypeInstance.Shape, AttachToNode.BottomCenter)
                loFactTable.ToolTip = "Fact Table"


                If Not Me.FactTypeInstance.isPreferredReferenceMode Then
                    loFactTable.Visible = abDisplayFactTable Or Me.Visible
                Else
                    loFactTable.Visible = False
                End If

                Dim liInd As Integer
                Dim lrFactInstance As FBM.FactInstance
                Dim liRowNr As Integer = 0

                loFactTable.RowCount = Me.FactTypeInstance.Fact.Count

                For Each lrFactInstance In Me.FactTypeInstance.Fact
                    For liInd = 0 To (Me.FactTypeInstance.Arity - 1)
                        'Dim lrRole_data As New FBM.FactData(Me.FactTypeInstance.RoleGroup(liInd), New tConcept  ("", pcenumConceptType.fact))        
                        'lrRole_data = lrFact.data.Find(AddressOf lrRole_data.Equals)

                        Dim lrFactDataInstance As New FBM.FactDataInstance '(me.Page,lrFactInstance.Data(liInd).FactDataInstance.Role,
                        lrFactDataInstance = lrFactInstance.Data(liInd)
                        lrFactDataInstance.TableShape = Me.TableShape
                        lrFactDataInstance.Cell = loFactTable.Item(liInd, liRowNr)
                        lrFactDataInstance.Fact = lrFactInstance

                        '---------------------------------------------------------------------------------------------------------------------------------
                        'If the FactDataInstance.TypeOfJoin is to a FactType then display the EnumeratedAsBracketedFact enumeration of the referred Fact
                        '  otherwise display simply the FactDataInstance.Data
                        '---------------------------------------------------------------------------------------------------------------------------------
                        Select Case lrFactDataInstance.Role.TypeOfJoin
                            Case Is = pcenumRoleJoinType.EntityType, pcenumRoleJoinType.ValueType
                                loFactTable.Item(liInd, liRowNr).Text = Trim(lrFactDataInstance.Concept.Symbol)
                            Case Is = pcenumRoleJoinType.FactType

                                Dim lrJoinedFactInstance As New FBM.FactInstance
                                Dim lrJoinedFactTypeInstance As New FBM.FactTypeInstance

                                lrJoinedFactTypeInstance = lrFactDataInstance.Role.JoinedORMObject 'was lrFactDataInstance.JoinedObjectType
                                lrJoinedFactInstance.Symbol = lrFactDataInstance.Data
                                lrJoinedFactInstance.Id = lrFactDataInstance.Data
                                lrJoinedFactInstance = lrJoinedFactTypeInstance.Fact.Find(AddressOf lrJoinedFactInstance.EqualsById)

                                If lrJoinedFactInstance Is Nothing Then
                                    lsMessage = "Cannot find joined FactInstance for FactDataInstance with FactDataInstance joining FactTypeInstance.FactInstance.Fact:"
                                    lsMessage &= vbCrLf & "Page.Name: " & arFactTypeInstance.Page.Name
                                    lsMessage &= vbCrLf & "Page.Id: " & arFactTypeInstance.Page.PageId
                                    lsMessage &= vbCrLf & "FactTypeInstance.Id: " & arFactTypeInstance.Id
                                    lsMessage &= vbCrLf & "FactInstance.Id: " & lrFactInstance.Id
                                    lsMessage &= vbCrLf & "FactDataInstance.Role.Id: " & lrFactDataInstance.Role.Id
                                    lsMessage &= vbCrLf & "FactDataInstance.Data: " & lrFactDataInstance.Data
                                    Throw New Exception(lsMessage)
                                End If

                                loFactTable.Item(liInd, liRowNr).Text = lrJoinedFactInstance.EnumerateAsBracketedFact
                        End Select

                        '-------------------------------------------------
                        'Set the Tag of the Cell to the FactDataInstance
                        '-------------------------------------------------
                        loFactTable.Item(liInd, liRowNr).Tag = lrFactDataInstance
                    Next liInd
                    liRowNr += 1
                Next

                If Me.FactTypeInstance.Fact.Count > 0 Then
                    loFactTable.ResizeToFitText(True)
                End If

            Catch ex As Exception
                lsMessage = "Error: tFactTable.DisplayAndAssociate"
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Public Sub DumpFactsToLogFile()

            '-------------------------------------------------
            'Dump the Facts of the FactTable to the Log flie
            '-------------------------------------------------
            'Dim liCol As Integer = 0
            'Dim liRow As Integer = 0

            'For liRow = 0 To e.Table.RowCount - 1
            '    lrFactDataInstance = e.Table.Item(0, liRow).Tag
            '    Dim lsDebugMessage As String = ""
            '    lsDebugMessage = liRow.ToString
            '    lsDebugMessage &= lrFactDataInstance.Fact.EnumerateAsBracketedFact & "     "
            '    lsDebugMessage &= lrFactDataInstance.FactData.Fact.EnumerateAsBracketedFact
            '    Call prApplication.ErrorLog.WriteToErrorLog(lsDebugMessage, "", "")
            'Next
        End Sub

        Public Sub GetFactsFromTableNode()

            'Dim li_row, li_col As Integer        
            'Dim lo_cell As MindFusion.Diagramming.TableNode.Cell
            'Dim lrFact As FBM.Fact

            'Me.FactTypeInstance.fact.Clear()

            'For li_row = 0 To (Me.table_shape.RowCount - 1)

            '    lrFact = New tFact(Me.FactType)


            '    '-------------------------------------------------
            '    'First check to see if the row actually contains 
            '    '  any/enough data. Abort on empty cell.
            '    '-------------------------------------------------
            '    For li_col = 0 To (Me.table_shape.ColumnCount - 1)
            '        lo_cell = Me.table_shape.Item(li_col, li_row)
            '        If Trim(lo_cell.Text) = "" Then
            '            Exit Sub
            '        End If
            '    Next li_col

            '    For li_col = 0 To (Me.table_shape.ColumnCount - 1)
            '        lo_cell = Me.table_shape.Item(li_col, li_row)
            '        Dim lrRole As FBM.Role = Me.FactTypeInstance.RoleGroup(li_col)
            '        lrFact.data.Add(New FBM.FactData(lrRole, New t_concept(Trim(lo_cell.Text), pcenumConceptType.entity)))
            '    Next li_col
            '    Me.FactTypeInstance.fact.Add(lrFact)
            'Next li_row

        End Sub

        Public Sub RemoveFromPage(ByVal abBroadcastInterfaceEvent As Boolean)

            Try
                'CodeSafe
                If Me.Model Is Nothing Or Me.Page Is Nothing Then Exit Sub

                Dim lrConceptInstance As New FBM.ConceptInstance(Me.Model, Me.Page, Me.FactTypeInstance.Id, pcenumConceptType.FactTable)

                If abBroadcastInterfaceEvent Then
                    Call TableConceptInstance.DeleteConceptInstance(lrConceptInstance)
                End If
            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try


        End Sub

        Public Sub ResetBlackCellText()

            Dim lrFactInstance As FBM.FactInstance
            Dim lrFactDataInstance As FBM.FactDataInstance

            Try

                'CodeSafe
                If Me.FactTypeInstance.Fact.Count > Me.FactTypeInstance.FactTable.TableShape.Rows.Count Then
                    Call Me.FactTypeInstance.FactTable.ResortFactTable()
                End If

                For Each lrFactInstance In Me.FactTypeInstance.Fact
                    For Each lrFactDataInstance In lrFactInstance.Data

                        'CodeSafe
                        If lrFactDataInstance.Cell Is Nothing Then GoTo SkipThat

                        If lrFactDataInstance.FactData.HasModelError Then
                            lrFactDataInstance.Cell.TextColor = Color.Red
                        Else
                            lrFactDataInstance.Cell.TextColor = Color.Black
                        End If

                        lrFactDataInstance.Cell.Brush = New MindFusion.Drawing.SolidBrush(Color.White)
SkipThat:
                    Next
                Next

                Me.SelectedRow = 0

            Catch ex As Exception
                Dim lsMessage As String
                lsMessage = "Error: tFactTable.ResetBlackCellText"
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        ''' <summary>
        ''' Redraws the FactTable based on the position of each Role in the FactTypeInstance.RoleGroup.
        '''   This is required because the positions of the Roles within the RoleGroup may change based
        '''   on whether the user rearranges the position of the ModelObjects attached to the Role.
        '''   If the RoleGroup order of Roles changes, so must the order of the Columns in the FactTable.
        ''' 
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub ResortFactTable(Optional ByVal abCellReflectsError As Boolean = False)

            Dim liInd As Integer
            Dim liRowNr As Integer = 0
            Dim lrFactInstance As FBM.FactInstance
            Dim lrFactDataInstance As New FBM.FactDataInstance
            Dim lsMessage As String = ""

            Try

                '---------------------------------------------------------------------
                'Make sure that the Row/Column dimentions of the FactTable match the
                '  Facts within the FactType
                '---------------------------------------------------------------------            
                Me.TableShape.ColumnCount = 0
                Me.TableShape.RowCount = 0
                Me.TableShape.ColumnCount = Me.FactTypeInstance.Arity
                Me.TableShape.RowCount = Me.FactTypeInstance.Fact.Count

                '=====================================================
                'CodeSafe: Make sure all Facts have Data count equal to the Arity of the FactTypeInstance
#Region "CodeSafe"
                Dim larFactInstance = From FactInstance In Me.FactTypeInstance.Fact
                                      Where FactInstance.Data.Count <> Me.FactTypeInstance.Arity
                                      Select FactInstance

                For Each lrFactInstance In larFactInstance.ToArray
                    MsgBox("Found a FactInstance in FactTypeInstance, " & Me.FactTypeInstance.Id & ", where Fact.Data.Count <> the Arity of the FactTypeInstance. Removing the FactInstance")
                    Call Me.FactTypeInstance.Fact.Remove(lrFactInstance)
                Next
#End Region
                '=====================================================

                For Each lrFactInstance In Me.FactTypeInstance.Fact
                    For liInd = 0 To (Me.FactTypeInstance.Arity - 1)
                        '---------------------------------------------------------------------------------------------
                        'Find the RoleData based on the Role at the liInd position in the FactTypeInstance.RoleGroup
                        '---------------------------------------------------------------------------------------------
                        lrFactDataInstance = New FBM.FactDataInstance(Me.Page, lrFactInstance, Me.FactTypeInstance.RoleGroup(liInd), New FBM.Concept(lrFactInstance.Data(liInd).Data))

                        lsMessage = "ResortFactTable:"
                        lsMessage &= vbCrLf & "Finding FactDataInstance"
                        lsMessage &= vbCrLf & "FactInstance.Symbol: " & lrFactInstance.Symbol
                        lsMessage &= vbCrLf & "FactDataInstance.Fact.Id:" & lrFactDataInstance.Fact.Id
                        lsMessage &= vbCrLf & "FactDataInstance.Role.Id:" & lrFactDataInstance.Role.Id

                        lrFactDataInstance = lrFactInstance.Data.Find(AddressOf lrFactDataInstance.EqualsByRole)

                        If IsSomething(lrFactDataInstance) Then
                            lsMessage &= vbCrLf & "Found"
                        Else
                            lsMessage &= vbCrLf & "Not Found"
                            Call prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Information)
                            Throw New System.Exception(lsMessage)
                        End If
                        Call prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Information)

                        lrFactDataInstance.TableShape = Me.TableShape
                        lrFactDataInstance.Cell = Me.TableShape.Item(liInd, liRowNr)
                        lrFactDataInstance.Cell.TextColor = Color.Black
                        Select Case lrFactDataInstance.Role.TypeOfJoin
                            Case Is = pcenumRoleJoinType.EntityType
                                If lrFactDataInstance.Role.JoinsEntityType.EntityType.HasCompoundReferenceMode Then
                                    lrFactDataInstance.Cell.Text = lrFactDataInstance.Role.JoinsEntityType.EntityType.EnumerateInstance(lrFactDataInstance.Data)
                                Else
                                    lrFactDataInstance.Cell.Text = Trim(lrFactDataInstance.Data)
                                End If
                            Case Is = pcenumRoleJoinType.ValueType
                                lrFactDataInstance.Cell.Text = Trim(lrFactDataInstance.Data)
                            Case Is = pcenumRoleJoinType.FactType
                                '--------------------------------------------------------------------
                                'If the cell represents data of a Role joined to a FactType,
                                '  then the cell must display an enumerated/bracketed fact based on 
                                '  the Fact that it joins within that FactType. e.g. {1,2,3}
                                '--------------------------------------------------------------------
                                Dim lrJoinedFact As New FBM.FactInstance
                                Dim lrJoinedFactType As New FBM.FactTypeInstance
                                lrJoinedFact.Symbol = lrFactDataInstance.Data
                                lrJoinedFact.Id = lrFactDataInstance.Data
                                lrJoinedFactType = lrFactDataInstance.Role.JoinedORMObject  ' was lrFactDataInstance.JoinedObjectType
                                lrJoinedFact = lrJoinedFactType.Fact.Find(AddressOf lrJoinedFact.EqualsById)

                                If IsSomething(lrJoinedFact) Then
                                    lrFactDataInstance.Cell.Text = lrJoinedFact.EnumerateAsBracketedFact
                                Else
                                    If abCellReflectsError Then
                                        lrFactDataInstance.Cell.Text = "Error: No matching Fact."
                                    Else
                                        lsMessage = "Error: Could not find Fact for Role/FactData referencing Facts in a Fact Type"
                                        lsMessage &= vbCrLf & "Role.JoinsFactType.Id: " & lrFactDataInstance.Role.JoinedORMObject.Id
                                        lsMessage &= vbCrLf & "Joined Fact.Id: " & lrFactDataInstance.Data
                                        Throw New System.Exception(lsMessage)
                                    End If
                                End If

                        End Select
                        If lrFactDataInstance.FactData.HasModelError Then
                            lrFactDataInstance.Cell.TextColor = Color.Red
                        Else
                            lrFactDataInstance.Cell.TextColor = Color.Black
                        End If
                        lrFactDataInstance.Cell.Tag = lrFactDataInstance
                    Next liInd

                    '20220617-VM-Removed. Not really using.
                    'lsMessage = "ResortFactTable:"
                    'lsMessage &= vbCrLf & "FactDataInstance.Fact.Id:" & lrFactDataInstance.Fact.Id
                    'lsMessage &= vbCrLf & "FactDataInstance.FactData.FactId:" & lrFactDataInstance.FactData.Fact.Id
                    'Call prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Information)

                    liRowNr += 1
                Next

                Me.TableShape.ResizeToFitText(True)

                If IsSomething(Me.Page.Diagram) Then
                    Me.Page.Diagram.Invalidate()
                End If

            Catch loErr As Exception
                Dim lsMessage1 As String
                lsMessage1 = "Error: tFactTable.ResortFactTable"
                lsMessage1 &= vbCrLf & vbCrLf & loErr.Message
                lsMessage1 &= vbCrLf & vbCrLf & "FactTypeName: " & Me.FactTypeInstance.Name
                lsMessage1 &= vbCrLf & vbCrLf & "PageId: " & Me.FactTypeInstance.Page.PageId
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, loErr.StackTrace)
            End Try

        End Sub

        Public Overrides Sub Save(Optional ByVal abRapidSave As Boolean = False)

            '---------------------------------------------
            'Saves the FactTableInstance to the database
            '---------------------------------------------
            Dim lrConceptInstance As New FBM.ConceptInstance

            Try
                lrConceptInstance.ModelId = Me.Model.ModelId
                lrConceptInstance.PageId = Me.Page.PageId
                lrConceptInstance.Symbol = Me.FactTypeInstance.Id
                lrConceptInstance.X = Me.X
                lrConceptInstance.Y = Me.Y
                lrConceptInstance.ConceptType = pcenumConceptType.FactTable

                If abRapidSave Then
                    Call TableConceptInstance.AddConceptInstance(lrConceptInstance)
                Else
                    If TableConceptInstance.ExistsConceptInstance(lrConceptInstance) Then
                        Call TableConceptInstance.UpdateConceptInstance(lrConceptInstance)
                    Else
                        Call TableConceptInstance.AddConceptInstance(lrConceptInstance)
                    End If
                End If


            Catch ex As Exception
                Dim lsMessage As String
                lsMessage = "Error: tFactTable.Save"
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Public Sub MouseDown() Implements iPageObject.MouseDown

        End Sub

        Public Sub MouseMove() Implements iPageObject.MouseMove

        End Sub

        Public Sub MouseUp() Implements iPageObject.MouseUp

        End Sub

        Public Sub Moved() Implements iPageObject.Moved

        End Sub

        Public Sub NodeDeleting() Implements iPageObject.NodeDeleting

        End Sub

        Public Sub NodeModified() Implements iPageObject.NodeModified

        End Sub

        Public Sub NodeSelected() Implements iPageObject.NodeSelected

        End Sub

        Public Sub CellClicked() Implements iTableNodePageObject.CellClicked

        End Sub

        Public Sub NodeDeselected() Implements iTableNodePageObject.NodeDeselected

        End Sub

        Public Sub NodeDoubleClicked() Implements iTableNodePageObject.NodeDoubleClicked

        End Sub

        Public Sub SetAppropriateColour() Implements iPageObject.SetAppropriateColour

        End Sub

        Public Sub RepellNeighbouringPageObjects(ByVal aiDepth As Integer) Implements iPageObject.RepellNeighbouringPageObjects

        End Sub

        Public Sub Move(ByVal aiNewX As Integer, ByVal aiNewY As Integer, ByVal abBroadcastInterfaceEvent As Boolean) Implements iPageObject.Move

        End Sub

        Public Overloads Sub EnableSaveButton() Implements iPageObject.EnableSaveButton
            Throw New NotImplementedException()
        End Sub
    End Class

End Namespace