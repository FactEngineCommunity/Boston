Imports System.IO
Imports System.Web.UI
Imports Gios.Word
Imports System.Reflection

Namespace FBM

    Public Class ORMWordVerbailser

        Private Document As Gios.Word.WordDocument

        Public IncludePagesOfSameNameAsModelElements As Boolean = False
        Public PutPageDiagramsAtEndOfDocument As Boolean = False

        Dim FontHeading1 As New Font("Helvetica", 14, FontStyle.Bold)
        Dim FontModelElement As New Font("Helvetica", 12, FontStyle.Bold)

        Dim heading As New Font("Helvetica", 14, FontStyle.Bold)
        Dim bold As New Font("Helvetica", 10, FontStyle.Bold)
        Dim predicateText As New Font("Helvetica", 10, FontStyle.Regular)
        Dim primaryErrorReport As New Font("Helvetica", 10, FontStyle.Bold)
        Dim regular As New Font("Helvetica", 10, FontStyle.Regular)        
        Dim quantifier As New Font("Helvetica", 10, FontStyle.Bold)
        Dim quantifierLight As New Font("Helvetica", 10, FontStyle.Regular)
        Dim objectType As New Font("Helvetica", 10, FontStyle.Bold)
        Dim listSeparator As New Font("Helvetica", 10, FontStyle.Bold)
        Dim subscript As New Font("Helvetica", 4, FontStyle.Regular)
        Dim FontString As New Font("Helvetica", 10, FontStyle.Regular)

        Public Sub New(ByRef rd As Gios.Word.WordDocument)

            Me.Document = rd

        End Sub

        Public Sub VerbaliseBlackText(ByVal asText As String)

            Me.Document.SetFont(Me.regular)
            Me.Document.SetForegroundColor(Color.Black)
            Me.Document.Write(asText)

        End Sub

        Public Sub VerbaliseDataType(ByVal asDataType As String)

            Me.Document.SetFont(Me.quantifierLight)
            Me.Document.SetForegroundColor(Color.Gray)
            Me.Document.Write(asDataType)

        End Sub

        Public Sub VerbaliseError(ByVal asErrorText As String)

            Me.Document.SetFont(Me.primaryErrorReport)
            Me.Document.SetForegroundColor(Color.Red)
            Me.Document.Write(asErrorText)

        End Sub

        Public Sub VerbaliseHeading(ByVal asHeading As String)

            Me.Document.SetFont(Me.heading)
            Me.Document.SetForegroundColor(Color.Black)
            Me.Document.Write(asHeading)

        End Sub

        Public Sub VerbaliseIndent()

            Me.Document.Write(vbTab)

        End Sub

        Public Sub VerbalisePredicateText(ByVal asPredicateText As String)

            Me.Document.SetFont(Me.predicateText)
            Me.Document.SetForegroundColor(Color.DarkGreen)
            Me.Document.Write(asPredicateText)
            Me.Document.SetFont(Me.regular)
            Me.Document.SetForegroundColor(Color.Black)

        End Sub


        Public Sub VerbaliseQuantifier(ByVal asQuantifier As String)

            Me.Document.SetFont(Me.quantifier)
            Me.Document.SetForegroundColor(Color.MediumBlue)
            Me.Document.Write(asQuantifier)

        End Sub

        Public Sub VerbaliseQuantifierLight(ByVal asQuantifier As String)

            Me.Document.SetFont(Me.quantifierLight)
            Me.Document.SetForegroundColor(Color.Blue)
            Me.Document.Write(asQuantifier)

        End Sub

        Public Sub VerbaliseString(ByVal asString As String)

            Me.Document.SetFont(Me.FontString)
            Me.Document.SetForegroundColor(Color.Maroon)
            Me.Document.Write(asString)

        End Sub

        Public Sub VerbaliseModelObject(ByRef arModelObject As FBM.ModelObject)

            Me.Document.SetFont(Me.objectType)
            Me.Document.SetForegroundColor(Color.Purple)
            Me.Document.Write(arModelObject.Id)

        End Sub

        Public Sub VerbaliseModelObjectLight(ByRef arModelObject As FBM.ModelObject)

            Me.Document.SetFont(Me.quantifierLight)
            Me.Document.SetForegroundColor(Color.Blue)
            Me.Document.Write(arModelObject.Id)

        End Sub

        Public Sub VerbaliseSeparator(ByVal asSeparator As String)

            Me.Document.SetFont(Me.listSeparator)
            Me.Document.SetForegroundColor(Color.Black)
            Me.Document.Write(asSeparator)

        End Sub

        Public Sub VerbaliseSubscript(ByVal asSubscriptText As String)

            Me.Document.SetFont(Me.subscript)
            Me.Document.SetForegroundColor(Color.Purple)
            Me.Document.Write(asSubscriptText)

        End Sub

        Public Sub Write(ByVal asText As String)

            Me.Document.Write(asText)

        End Sub

        Public Sub WriteLine()

            Me.Document.WriteLine()

        End Sub

        Public Sub createWordDocument(ByVal arWorkingModel As FBM.Model)

            Try
                'Create the First Page.
                Call Me.createFirstPage(arWorkingModel)

                Me.Document.SetPageNumbering(1)

                'EntityTypes
                Me.Document.NewPage()
                Me.Document.WriteLine()
                Me.Document.SetFont(FontHeading1)
                Me.Document.SetForegroundColor(Color.Black)
                Me.Document.WriteLine("Entity Types")
                Me.Document.WriteLine()

                Call Me.verbaliseEntityTypes(arWorkingModel)

                'Objectified Fact Types
                Me.Document.SetFont(FontHeading1)
                Me.Document.SetForegroundColor(Color.Black)
                Me.Document.WriteLine("Objectified Fact Types")
                Call Me.verbaliseObjectifiedFactTypes(arWorkingModel)

                'Constraints
                Me.Document.NewPage()
                Me.Document.WriteLine()
                Me.Document.SetFont(FontHeading1)
                Me.Document.SetForegroundColor(Color.Black)
                Me.Document.WriteLine("Constraints")
                Me.Document.WriteLine()

                Call Me.verbaliseConstraints(arWorkingModel)

                'ValueTypes
                Me.Document.NewPage()
                Me.Document.WriteLine()
                Me.Document.SetFont(FontHeading1)
                Me.Document.SetForegroundColor(Color.Black)
                Me.Document.WriteLine("Value Types")
                Me.Document.WriteLine()

                Call Me.verbaliseValueTypes(arWorkingModel)

                'Model's Page Image processing
                If Me.PutPageDiagramsAtEndOfDocument Then
                    Me.Document.NewPage()
                    Call Me.createModelPageImages(arWorkingModel)
                End If


                'Headers and Footers
                Me.Document.HeaderStart()
                Me.Document.SetTextAlign(WordTextAlign.Center)
                Me.Document.SetFont(New Font("Verdana", 10, FontStyle.Bold))
                Me.Document.Write("Schema for the model, '" & arWorkingModel.Name & "'")
                Me.Document.WriteLine()
                Me.Document.HeaderEnd()

                Me.Document.FooterStart()
                Me.Document.SetTextAlign(WordTextAlign.Center)
                Me.Document.SetFont(New Font("Verdana", 10, FontStyle.Bold))
                Me.Document.Write("Generated by Boston - Conceptual Modelling at its finest - http://www.viev.com")
                Me.Document.FooterEnd()

                'Create the RTF/Word Document
                Dim lsFileName As String = ""
                Try
                    Dim saveFileDialog As New Windows.Forms.SaveFileDialog
                    saveFileDialog.Filter = "Word Document|*.doc"
                    saveFileDialog.Title = "Save model documentation"
                    saveFileDialog.CheckFileExists = False
                    saveFileDialog.CheckPathExists = False
                    saveFileDialog.CreatePrompt = False


                    Call saveFileDialog.ShowDialog(frmMain)

                    If saveFileDialog.FileName <> "" Then
                        lsFileName = saveFileDialog.FileName
                        Me.Document.SaveToFile(saveFileDialog.FileName) '"..\\..\\Example1.doc")
                    End If

                Catch ex As Exception
                    MsgBox("Couldn't save the document. Check to see if you already have the document open.")
                End Try

                'Open the document in WoMe.Document.
                Try
                    If lsFileName <> "" Then
                        System.Diagnostics.Process.Start(lsFileName) ' "..\..\Example1.doc")
                    End If
                Catch ex As Exception
                    MsgBox("Couldn't open the file.")
                End Try

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Public Sub createFirstPage(ByRef arWorkingModel As FBM.Model)

            Me.Document.SetFont(bold)
            Me.Document.SetTextAlign(WordTextAlign.Left)

            For liInd = 1 To 5
                Me.Document.WriteLine()
            Next

            Me.Document.SetFont(New Font("Helvetica", 18, FontStyle.Bold))
            Me.Document.SetTextAlign(WordTextAlign.Center)
            Me.Document.WriteLine("Model Documentation")
            Me.Document.WriteLine()
            Me.Document.WriteLine(arWorkingModel.Name)

            For liInd = 1 To 20
                Me.Document.WriteLine()
            Next

            Me.Document.WriteLine()

            '=================================================================================================================
            'Document Author and Date
            Dim rt As WordTable = Me.Document.NewTable(regular, Color.Black, 2, 2, 2)
            rt.SetColumnsWidth(New Integer() {850, 3000})

            'foreach (WordCell rc in rt.Cells) rc.SetBorders(Color.Black,1,true,true,true,true)

            rt.SetContentAlignment(ContentAlignment.TopLeft)
            rt.Rows(0).SetRowHeight(15)
            rt.Rows(1).SetRowHeight(15)
            rt.Rows(0)(0).SetFont(New Font("Helvetica", 12, FontStyle.Regular))
            rt.Rows(0)(0).Write("Author:")
            rt.Rows(1)(0).SetFont(New Font("Helvetica", 12, FontStyle.Regular))
            rt.Rows(1)(0).Write("Date:")
            rt.Rows(1)(1).Write(DateTime.Today.Date.ToString)

            rt.SaveToDocument(3920, 0) 'Writes the Table to the Document.
            '=================================================================================================================

            Me.Document.SetTextAlign(WordTextAlign.Left)

        End Sub

        Private Sub verbaliseEntityTypes(ByRef arWorkingModel As FBM.Model)

            Try
                Dim liInd As Integer = 0

                For Each lrEntityType In arWorkingModel.EntityType.FindAll(Function(x) x.IsMDAModelElement = False And x.IsObjectifyingEntityType = False)

                    Call lrEntityType.Verbalise(Me)

                    '================================================
                    If Me.IncludePagesOfSameNameAsModelElements Then
                        Me.WriteLine()
                        Me.WriteLine()

                        Dim lsEntityTypeId As String = lrEntityType.Id

                        Dim larPage = From Page In arWorkingModel.Page _
                                      Where Page.Name = lsEntityTypeId _
                                      Select Page

                        For Each lrPage In larPage
                            Dim lrForm As Object = Nothing

                            Select Case lrPage.Language
                                Case Is = pcenumLanguage.ORMModel
                                    lrForm = frmMain.loadORMModelPage(lrPage, Nothing, False)
                                Case Is = pcenumLanguage.EntityRelationshipDiagram
                                    lrForm = frmMain.loadERDiagramView(lrPage, Nothing)
                                Case Is = pcenumLanguage.PropertyGraphSchema
                                    lrForm = frmMain.load_PGS_diagram_view(lrPage, Nothing)
                            End Select

                            Dim lsProgramDataDirectory As String = My.Computer.FileSystem.SpecialDirectories.AllUsersApplicationData & "\documentationfiles"
                            Call Richmond.CreateDirectoryIfItDoesntExist(lsProgramDataDirectory)

                            lrForm.writeImageToFile(lsProgramDataDirectory & "\" & "entitytypeimage" & liInd & ".jpg") '"..\\..\\entitytypeimage" & liInd & ".jpg")
                            Me.Document.PutImage(lsProgramDataDirectory & "\" & "entitytypeimage" & liInd & ".jpg", 70) '"..\..\entitytypeimage" & liInd & ".jpg", 70)

                            Call lrForm.Close()

                            liInd += 1
                        Next
                    End If
                    '================================================

                    Me.Document.WriteLine()
                    Me.Document.NewPage()

                    liInd += 1

                Next 'EntityType

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Private Sub verbaliseObjectifiedFactTypes(ByVal arWorkingModel As FBM.Model)

            Try
                For Each lrFactType In arWorkingModel.FactType.FindAll(Function(x) x.IsObjectified = True And x.IsMDAModelElement = False)

                    Me.Document.WriteLine()
                    Me.VerbaliseModelObject(lrFactType)
                    Me.VerbaliseQuantifierLight(" is an Objectified Fact Type")
                    Me.Document.WriteLine()

                    Me.Document.WriteLine()
                    Call lrFactType.FactTypeReading(0).GetReadingText(Me, True, True, True)
                    Me.Document.WriteLine()
                    Me.Document.WriteLine()

                    Me.VerbaliseQuantifier("Fact Types")
                    Me.Document.WriteLine()
                    Me.Document.WriteLine()

                    Dim FactType = From ft In arWorkingModel.FactType, _
                                        rl In ft.RoleGroup _
                                        Where rl.JoinedORMObject.Id = lrFactType.Id _
                                        Select ft Distinct

                    For Each lrRelatedFactType In FactType

                        If lrRelatedFactType.FactTypeReading.Count > 0 Then
                            Call lrRelatedFactType.FactTypeReading(0).GetReadingText(Me, True, True, True)
                        End If

                        Me.Document.WriteLine()
                    Next

                    Me.Document.WriteLine()
                    Me.Document.NewPage()

                Next

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Private Sub verbaliseConstraints(ByVal arWorkingModel As FBM.Model)

            Try
                For Each lrRoleConstraint In arWorkingModel.RoleConstraint.FindAll(Function(x) x.IsMDAModelElement = False And Not x.RoleConstraintType = pcenumRoleConstraintType.InternalUniquenessConstraint)
                    Select Case lrRoleConstraint.RoleConstraintType
                        Case Is = pcenumRoleConstraintType.EqualityConstraint
                            Call lrRoleConstraint.verbaliseEqualityConstraint(Me)
                        Case Is = pcenumRoleConstraintType.ExclusionConstraint
                            Call lrRoleConstraint.verbaliseExclusionConstraint(Me)
                        Case Is = pcenumRoleConstraintType.ExclusiveORConstraint
                            Call lrRoleConstraint.verbaliseExclusiveORConstraint(Me)
                        Case Is = pcenumRoleConstraintType.RingConstraint
                            Call lrRoleConstraint.verbaliseRingConstraint(Me)
                        Case Is = pcenumRoleConstraintType.SubsetConstraint
                            Call lrRoleConstraint.verbaliseSubsetConstraint(Me)
                    End Select
                Next

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Private Sub verbaliseValueTypes(ByVal arWorkingModel As FBM.Model)

            Try

                For Each lrValueType In arWorkingModel.ValueType.FindAll(Function(x) x.IsMDAModelElement = False)
                    Call lrValueType.verbaliseCQL(Me)
                Next

                'Me.WriteLine()
                'Me.Document.NewPage()

                'Dim liInd As Integer = 1

                'Dim loTable As WordTable = Me.Document.NewTable(regular, Color.Black, arWorkingModel.ValueType.Count + 1, 4, 0)
                'loTable.SetColumnsWidth(New Integer() {35, 35, 20, 20})

                'Me.Document.SetFont(regular)

                'loTable.Rows(0)(0).SetTextAlign(WordTextAlign.Center)
                'loTable.Rows(0)(0).SetFont(Me.bold)
                'loTable.Rows(0)(0).Write("Value Type")
                'loTable.Rows(0)(1).SetFont(Me.bold)
                'loTable.Rows(0)(1).Write("Data Type")
                'loTable.Rows(0)(2).SetFont(Me.bold)
                'loTable.Rows(0)(2).Write("Length")
                'loTable.Rows(0)(3).SetFont(Me.bold)
                'loTable.Rows(0)(3).Write("Precision")

                'For Each lrValueType In arWorkingModel.ValueType.FindAll(Function(x) x.IsMDAModelElement = False)
                '    loTable.Rows(liInd)(0).SetTextAlign(WordTextAlign.Left)
                '    loTable.Rows(liInd)(0).Write(lrValueType.Id)

                '    'Data Type
                '    loTable.Rows(liInd)(1).SetTextAlign(WordTextAlign.Left)
                '    loTable.Rows(liInd)(1).Write(lrValueType.DataType.ToString)

                '    'Length
                '    loTable.Rows(liInd)(2).SetTextAlign(WordTextAlign.Left)
                '    loTable.Rows(liInd)(2).Write(lrValueType.DataTypeLength.ToString)

                '    'Precision
                '    loTable.Rows(liInd)(3).SetTextAlign(WordTextAlign.Left)
                '    loTable.Rows(liInd)(3).Write(lrValueType.DataTypePrecision.ToString)

                '    liInd += 1
                'Next

                'loTable.SaveToDocument(10000, 0)


            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Private Sub createModelPageImages(ByRef arWorkingModel As FBM.Model)

            Try
                Dim liInd As Integer = 0

                For Each lrPage In arWorkingModel.Page

                    Me.Document.SetFont(FontHeading1)
                    Me.Document.SetForegroundColor(Color.Black)
                    Me.Document.WriteLine(lrPage.Name)
                    Me.Document.WriteLine()
                    Me.Document.WriteLine()

                    Dim lrForm As Object = Nothing

                    Select Case lrPage.Language
                        Case Is = pcenumLanguage.ORMModel
                            lrForm = frmMain.loadORMModelPage(lrPage, Nothing, False)
                        Case Is = pcenumLanguage.EntityRelationshipDiagram
                            lrForm = frmMain.loadERDiagramView(lrPage, Nothing)
                        Case Is = pcenumLanguage.PropertyGraphSchema
                            lrForm = frmMain.load_PGS_diagram_view(lrPage, Nothing)
                    End Select

                    Dim lsProgramDataDirectory As String = My.Computer.FileSystem.SpecialDirectories.AllUsersApplicationData & "\documentationfiles"
                    Call Richmond.CreateDirectoryIfItDoesntExist(lsProgramDataDirectory)

                    lrForm.writeImageToFile(lsProgramDataDirectory & "\" & liInd.ToString & ".jpg") '"..\\..\\image" & liInd & ".jpg")
                    Me.Document.PutImage(lsProgramDataDirectory & "\" & liInd.ToString & ".jpg", 70) '"..\..\image" & liInd & ".jpg", 70)

                    Me.Document.NewPage()

                    Call lrForm.Close()
                    liInd += 1
                Next

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub


    End Class

End Namespace
