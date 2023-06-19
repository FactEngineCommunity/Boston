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
	Public mrRecordset As ORMQL.Recordset

	'Returned to frmToolboxTableData if Importing CSV data.
	Public mdtData As DataTable

	Private Shared mrFileHandler As CSD.clsFileHandler = New CSD.clsFileHandler()


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

		Select Case Me.miFormFunction
			Case Is = pcenumCSVFormFunction.ExportCSVData

				' Create an instance of the SaveFileDialog
				Dim lrSaveFileDialog As New SaveFileDialog()

				' Set the initial directory and default file name
				lrSaveFileDialog.InitialDirectory = "C:\"
				lrSaveFileDialog.FileName = Me.mrTable.Name & ".csv"

				' Display the dialog box and wait for the user's response
				If lrSaveFileDialog.ShowDialog() = DialogResult.OK Then

					' Get the selected file name and path
					txtFileName.Text = lrSaveFileDialog.FileName
				End If
			Case Is = pcenumCSVFormFunction.ImportCSVData

				Dim lrOpenFileDialog As New OpenFileDialog()

				lrOpenFileDialog.Filter = "(*.csv)|*.csv|*.txt)|*.txt|All files (*.*)|*.*"
				lrOpenFileDialog.FilterIndex = 0

				If lrOpenFileDialog.ShowDialog(Me) = DialogResult.OK Then

					txtFileName.Text = lrOpenFileDialog.FileName

					Call Me.GetFileDetails(txtFileName.Text)

				End If
		End Select

	End Sub

	Private Sub GetFileDetails(ByVal asFilePath As String)

		mrFileHandler = New CSD.clsFileHandler(asFilePath)

		If mrFileHandler.mrFileInfo.Exists Then

			txtCreationTime.Text = Convert.ToString(mrFileHandler.mrFileInfo.CreationTime)
			txtDirectoryPath.Text = mrFileHandler.mrFileInfo.DirectoryName
			txtExists.Text = Convert.ToString(mrFileHandler.mrFileInfo.Exists)
			txtExt.Text = mrFileHandler.mrFileInfo.Extension
			txtFullName.Text = mrFileHandler.mrFileInfo.FullName
			txtLastAccessTime.Text = Convert.ToString(mrFileHandler.mrFileInfo.LastAccessTime)
			txtLastWriteTime.Text = Convert.ToString(mrFileHandler.mrFileInfo.LastWriteTime)
			txtLength.Text = Convert.ToString(mrFileHandler.mrFileInfo.Length)
			txtName.Text = mrFileHandler.mrFileInfo.Name
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
			mrFileHandler.msDelimiter = txtDelimiter.Text
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

		mrFileHandler.msDelimiter = txtDelimiterOUT.Text
		mrFileHandler.mrFileInfo = New FileInfo(lsFilePath)

		If mrFileHandler.ExportORMQLRecordsetToCSV(Me.mrRecordset, Not chkIncludeTitle.Checked) Then
			txtDestination.Text = lsFilePath
		Else
			txtDestination.Text = "Export failed"
		End If
	End Sub

	Private Sub ButtonFinish_Click(sender As Object, e As EventArgs) Handles ButtonFinish.Click

		Try

			Select Case Me.miFormFunction
				Case Is = pcenumCSVFormFunction.ExportCSVData

				Case Is = pcenumCSVFormFunction.ImportCSVData
					'DataView Returned to frmToolboxTableData
			End Select

		Catch ex As Exception
			Dim lsMessage As String
			Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

			lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
			lsMessage &= vbCrLf & vbCrLf & ex.Message
			prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
		End Try

	End Sub

End Class

