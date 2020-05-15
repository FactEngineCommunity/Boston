Public Class frmToolboxKLTheoremWriter

    Public zrPage As FBM.Page
    Public zrKLProofGenerator As New tKLProofGenerator

    Public Function EqualsByName(ByVal other As Form) As Boolean
        If Me.Name = other.Name Then
            Return True
        Else
            Return False
        End If
    End Function

    Private Sub frm_kl_theorem_writer_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing

        Me.Hide()

    End Sub

    Private Sub frm_kl_theorem_writer_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.GotFocus

        Call Me.SetupForm()

    End Sub

    Private Sub frm_kl_theorem_writer_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Call Me.SetupForm()

    End Sub

    Private Sub SetupForm()

        Me.TextBox1.Height = Me.Height - Me.TextBox1.Top

        If IsSomething(prApplication.WorkingPage) Then
            Me.ButtonAnalyseCurrentPage.Enabled = True
            Me.zrPage = prApplication.WorkingPage
        Else
            Me.ButtonAnalyseCurrentPage.Enabled = False
        End If

    End Sub

    Private Sub ButtonAnalyseCurrentPage_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonAnalyseCurrentPage.Click

        Dim liInd As Integer = 0
        Dim li_analysis_step As Integer = 0
        Dim lrRole As New FBM.Role
        'Dim lrEntityTypeInstance As FBM.EntityTypeInstance
        'Dim lrValueTypeInstance As FBM.ValueTypeInstance
        Dim lrFactTypeInstance As FBM.FactTypeInstance
        Dim lrRoleConstraint As FBM.RoleConstraint
        Dim lrFact As FBM.FactInstance
        Dim li_FactType_ind As Integer = 0

        Me.TextBox1.Clear()

        Me.zrKLProofGenerator = New tKLProofGenerator

        Me.TextBox1.Text = ""

        If Not IsSomething(Me.zrPage) Then
            MsgBox("Page not set")
            Exit Sub
        End If

        'For Each lrEntityTypeInstance In Me.zrPage.EntityTypeInstance
        '    If lrEntityTypeInstance.KLLetter Is "" Then

        '    End If
        'Next

        'For Each lrValueTypeInstance In Me.zrPage.ValueTypeInstance

        'Next

        'For Each lrFactTypeInstance In Me.zrPage.FactTypeInstance

        'Next


        For Each lrFactTypeInstance In Me.zrPage.FactTypeInstance
            '------------------------------------------------------------------
            'Reserve the Free Variables for the Roles of the Function/FactType
            '------------------------------------------------------------------     
            liInd = 0
            For Each lrRole In lrFactTypeInstance.FactType.RoleGroup
                liInd += 1
                lrRole.KLFreeVariableLabel = [Enum].GetName(GetType(pcenumKLFreeVariable), liInd)
            Next

            '---------------------------------------------------
            'Assign a KL Function Label to the FactTypeInstance
            '---------------------------------------------------                
            If Me.zrKLProofGenerator.is_function_label_reserved(lrFactTypeInstance.Name.Substring(0, 1)) Then
                '------------------------------
                'Allocate a new Function Label
                '------------------------------
                li_FactType_ind += 1
                lrFactTypeInstance.KLFunctionLabel = [Enum].GetName(GetType(pcenumKLFunction), li_FactType_ind)
            Else
                '-------------------------------------------------------------------------------
                'Allocate the first character of the name of the FactType as the Function label
                '-------------------------------------------------------------------------------
                lrFactTypeInstance.KLFunctionLabel = lrFactTypeInstance.Name.Substring(0, 1)
            End If

            '-------------------------------------------------------
            'Define the relation
            '-------------------------------------------------------
            li_analysis_step += 1
            Me.TextBox1.AppendText(li_analysis_step & "." & vbTab)
            Me.TextBox1.AppendText(Chr(pcenumKL.ForAll))
            For Each lrRole In lrFactTypeInstance.FactType.RoleGroup
                Me.TextBox1.AppendText(lrRole.KLFreeVariableLabel)
            Next
            Me.TextBox1.AppendText("(R")
            For Each lrRole In lrFactTypeInstance.FactType.RoleGroup
                Me.TextBox1.AppendText(lrRole.KLFreeVariableLabel)
            Next
            Me.TextBox1.AppendText(" " & Chr(pcenumKL.ImpliesThat) & " ")
            liInd = 1
            For Each lrRole In lrFactTypeInstance.FactType.RoleGroup
                Me.TextBox1.AppendText(UCase(Microsoft.VisualBasic.Left(lrRole.JoinedORMObject.Id, 1)))
                Me.TextBox1.AppendText(lrRole.KLFreeVariableLabel)
                If liInd < lrFactTypeInstance.FactType.RoleGroup.Count Then
                    Me.TextBox1.AppendText(" & ")
                End If
                liInd += 1
            Next
            Me.TextBox1.AppendText(")")
            Me.TextBox1.AppendText(vbTab & "Relation: '" & lrFactTypeInstance.Name & "'")
            Me.TextBox1.AppendText(vbCrLf)
            '--------------------------------------------------------

            If lrFactTypeInstance.Arity = 2 Then


                '---------------------------
                'Reserve the Function Label
                '---------------------------
                Me.zrKLProofGenerator.reserve_function_label(lrFactTypeInstance.KLFunctionLabel)

                '------------------------
                'Populate the function
                '------------------------
                li_analysis_step += 1
                Me.TextBox1.Text &= li_analysis_step & "." & vbTab & Chr(pcenumKL.ThereExists) & "xy x" & lrFactTypeInstance.KLFunctionLabel & "y" & vbTab & vbTab & vbTab & "Populate " & lrFactTypeInstance.KLFunctionLabel & vbTab & lrFactTypeInstance.Name & vbCrLf

                '-------------------------------------------------------------------------------------------------
                'Use the Universal Instantiation rule to instantiate Fact Values for each Role in a FactType/Function
                '-------------------------------------------------------------------------------------------------
                For Each lrFact In lrFactTypeInstance.Fact
                    li_analysis_step += 1

                    Dim lrDictionaryEntry As New FBM.DictionaryEntry(Me.zrPage.Model, lrFact.Data(0).Concept.Symbol, pcenumConceptType.Value)
                    Dim lsKLLetter As FBM.DictionaryEntry = lrFactTypeInstance.Model.ModelDictionary.Find(AddressOf lrDictionaryEntry.Equals)
                    lrDictionaryEntry = New FBM.DictionaryEntry(Me.zrPage.Model, lrFact.Data(1).Concept.Symbol, pcenumConceptType.Value)
                    Dim lsKLLetter2 As FBM.DictionaryEntry = lrFactTypeInstance.Model.ModelDictionary.Find(AddressOf lrDictionaryEntry.Equals)

                    Me.TextBox1.Text &= li_analysis_step & "." & vbTab & lsKLLetter.KLIdentityLetter & lrFactTypeInstance.KLFunctionLabel & lsKLLetter2.KLIdentityLetter & vbTab & vbTab & vbTab & "UI" & vbTab & lrFact.Fact.GetReading & vbCrLf
                Next

                '------------------------------------------
                'Translate the UniquenessConstraint to KL
                '------------------------------------------
                For Each lrRoleConstraint In lrFactTypeInstance.InternalUniquenessConstraint
                    li_analysis_step += 1
                    Me.TextBox1.Text &= li_analysis_step & "." & vbTab & Chr(pcenumKL.ForAll) & "xyz (x" & lrFactTypeInstance.KLFunctionLabel & "y & x" & lrFactTypeInstance.KLFunctionLabel & "z " & Chr(pcenumKL.ImpliesThat) & " y=z)" & vbTab & lrRoleConstraint.Name & vbCrLf
                    li_analysis_step += 1
                    Me.TextBox1.Text &= li_analysis_step & "." & vbTab & "a" & lrFactTypeInstance.KLFunctionLabel & "b & a" & lrFactTypeInstance.KLFunctionLabel & "c " & Chr(pcenumKL.ImpliesThat) & " b=c" & vbTab & vbTab & li_analysis_step - 1 & vbCrLf
                Next

                '-----------------------------
                'Express FrequenceConstraints
                '-----------------------------
                For Each lrRole In lrFactTypeInstance.FactType.RoleGroup
                    If lrRole.FrequencyConstraint > 0 Then
                        li_analysis_step += 1
                        Me.TextBox1.Text &= li_analysis_step & "." & vbTab & Chr(pcenumKL.ForAll) & "xy [x" & lrFactTypeInstance.KLFunctionLabel & "y " & Chr(pcenumKL.ImpliesThat) & " " & Chr(pcenumKL.ThereExists) & "z(x" & lrFactTypeInstance.KLFunctionLabel & "z & y not= z)]" & vbTab & vbTab & vbTab & "Cx"
                    End If
                Next
            Else
                '------------------------
                'Populate the function
                '------------------------
                li_analysis_step += 1
                Me.TextBox1.AppendText(li_analysis_step & "." & vbTab)
                Me.TextBox1.AppendText(Chr(pcenumKL.ThereExists))
                For Each lrRole In lrFactTypeInstance.FactType.RoleGroup
                    Me.TextBox1.AppendText(lrRole.KLFreeVariableLabel)
                Next
                Me.TextBox1.AppendText(" " & lrFactTypeInstance.KLFunctionLabel)
                For Each lrRole In lrFactTypeInstance.FactType.RoleGroup
                    Me.TextBox1.AppendText(lrRole.KLFreeVariableLabel)
                Next
                Me.TextBox1.AppendText(vbTab & vbTab & vbTab & "Populate " & lrFactTypeInstance.KLFunctionLabel & vbTab & lrFactTypeInstance.Name & vbCrLf)
            End If
        Next

    End Sub

    Private Sub frm_kl_theorem_writer_Resize(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Resize

        Me.TextBox1.Width = Me.Width - 20

    End Sub

End Class