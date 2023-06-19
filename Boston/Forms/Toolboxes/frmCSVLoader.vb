Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Data
Imports System.Drawing
Imports System.Linq
Imports System.Text
Imports System.Windows.Forms
Imports System.IO
Imports System.Reflection

Public Class frmCSVLoader

	Public miFormFunction As pcenumCSVFormFunction = pcenumCSVFormFunction.ExportCSVData

	Public mrModel As FBM.Model
	Public mrTable As RDS.Table

	Private Shared mrFileHandler As CSD.clsFileHandler = New CSD.clsFileHandler()
	Private mdtData As DataTable

	Private Sub Form1_Load(sender As Object, e As EventArgs) Handles Me.Load

		Try
			Call Me.SetupForm()

		Catch ex As Exception
			Dim lsMessage As String
			Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

			lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
			lsMessage &= vbCrLf & vbCrLf & ex.Message
			prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
		End Try

	End Sub

	Private Sub SetupForm()

		Try
			Select Case Me.miFormFunction
				Case Is = pcenumCSVFormFunction.ExportCSVData
					Me.TabControl.TabPages.Remove(Me.TabPageImportCSV)
					Me.TabPageExportCSVData.Show()
				Case Is = pcenumCSVFormFunction.ImportCSVData
					Me.TabControl.TabPages.Remove(Me.TabPageExportCSVData)
					Me.TabPageImportCSV.Show()
			End Select

			Me.LabelModelName.Text = Me.mrModel.Name
			Me.LabelTableName.Text = Me.mrTable.Name

		Catch ex As Exception
			Dim lsMessage As String
			Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

			lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
			lsMessage &= vbCrLf & vbCrLf & ex.Message
			prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
		End Try

	End Sub

	Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click
		Me.Close()
	End Sub

	Private Sub btnGetFile_Click(sender As Object, e As EventArgs) Handles btnGetFile.Click

		Dim lrOpenFileDialog As New OpenFileDialog()

		lrOpenFileDialog.Filter = "(*.csv)|*.csv|*.txt)|*.txt|All files (*.*)|*.*"
		lrOpenFileDialog.FilterIndex = 0

		If lrOpenFileDialog.ShowDialog(Me) = DialogResult.OK Then

			txtFileName.Text = lrOpenFileDialog.FileName

			Call Me.GetFileDetails(txtFileName.Text)

		End If

	End Sub

	Private Sub GetFileDetails(ByVal asFilePath As String)

		mrFileHandler = New CSD.clsFileHandler(asFilePath)

		If mrFileHandler.FileInf.Exists Then

			txtCreationTime.Text = Convert.ToString(mrFileHandler.FileInf.CreationTime)
			txtDirectoryPath.Text = mrFileHandler.FileInf.DirectoryName
			txtExists.Text = Convert.ToString(mrFileHandler.FileInf.Exists)
			txtExt.Text = mrFileHandler.FileInf.Extension
			txtFullName.Text = mrFileHandler.FileInf.FullName
			txtLastAccessTime.Text = Convert.ToString(mrFileHandler.FileInf.LastAccessTime)
			txtLastWriteTime.Text = Convert.ToString(mrFileHandler.FileInf.LastWriteTime)
			txtLength.Text = Convert.ToString(mrFileHandler.FileInf.Length)
			txtName.Text = mrFileHandler.FileInf.Name
			txtNameOnly.Text = mrFileHandler.NameOnly
			txtUNCPath.Text = mrFileHandler.UNCPath

		End If


	End Sub

	Private Sub btnGetData_Click(sender As Object, e As EventArgs) Handles btnGetData.Click

		Try
			Call Me.GetDataFromCSV(Me.txtFileName.Text)

		Catch ex As Exception
			Dim lsMessage As String
			Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

			lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
			lsMessage &= vbCrLf & vbCrLf & ex.Message
			prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
		End Try
	End Sub

	Private Sub GetDataFromCSV(ByVal asFilePath As String)

		Try
			mrFileHandler.Delimiter = txtDelimiter.Text
			mrFileHandler.DataRow1 = Convert.ToInt32(numDataRow.Value)
			mrFileHandler.HeaderRow = Convert.ToInt32(numTitleRow.Value)
			mrFileHandler.MaxRows = CInt(Math.Truncate(numMax.Value))
			mdtData = mrFileHandler.CSVToDataTable()

			Me.DataGridView1.DataSource = mdtData

		Catch ex As Exception
			Dim lsMessage As String
			Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

			lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
			lsMessage &= vbCrLf & vbCrLf & ex.Message
			prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
		End Try

	End Sub

	Private Sub btnExport_Click(sender As Object, e As EventArgs) Handles btnExport.Click

		Dim lsFilePath As String = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)

		lsFilePath = Path.Combine(lsFilePath, txtFileNameOut.Text)

		mrFileHandler.Delimiter = txtDelimiterOUT.Text
		mrFileHandler.FileInf = New FileInfo(lsFilePath)

		If mrFileHandler.TableToCSV(mdtData.DefaultView, Not chkIncludeTitle.Checked) Then
			txtDestination.Text = lsFilePath
		Else
			txtDestination.Text = "Export failed"
		End If
	End Sub

End Class

