Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.IO
Imports System.Data
Imports System.Management
Imports System.Reflection

Namespace CSD
    Public Class clsFileHandler

        Public Property mrFileInfo As FileInfo

        Private mvHeaderRow As Integer = -1

        Public Property HeaderRow As Integer
            Get
                Return mvHeaderRow
            End Get
            Set(ByVal value As Integer)
                mvHeaderRow = value
            End Set
        End Property

        Public Property DataRow1 As Integer
        Public Property msDelimiter As String
        Public Property MaxRows As Integer

        Public ReadOnly Property NameOnly As String
            Get
                Return mrFileInfo.Name.Substring(0, (mrFileInfo.Name.Length - mrFileInfo.Extension.Length))
            End Get
        End Property

        Public ReadOnly Property UNCPath As String
            Get
                Return GetUNCPath()
            End Get
        End Property

        Public Sub New()
        End Sub

        Public Sub New(ByVal sFilename As String)
            mrFileInfo = New FileInfo(sFilename)
        End Sub

        Private Function GetUNCPath() As String
            Dim sPath As StringBuilder = New StringBuilder()
            Dim sNetLtr As String
            Dim sLtr As String = mrFileInfo.FullName.Substring(0, 2)
            Dim query As SelectQuery = New SelectQuery("select name, ProviderName from win32_logicaldisk where drivetype=4")
            Dim searcher As ManagementObjectSearcher = New ManagementObjectSearcher(query)

            For Each mo As ManagementObject In searcher.[Get]()
                sNetLtr = Convert.ToString(mo("name"))

                If sNetLtr = sLtr Then
                    sPath.AppendFormat("{0}{1}", mo("ProviderName"), mrFileInfo.DirectoryName.Substring(2))
                End If
            Next

            Return sPath.ToString()
        End Function

        Public Function CSVToDataTable() As DataTable

            Try
                If mrFileInfo Is Nothing Then
                    Return Nothing
                End If

                Dim ldtData As DataTable = New DataTable()

                Dim oTR As TextReader = File.OpenText(mrFileInfo.FullName)
                Dim sLine As String = Nothing
                Dim arData As String()
                Dim drData As DataRow
                Dim iRows As Integer = 0

                If mvHeaderRow > -1 Then

                    For i As Integer = 0 To (mvHeaderRow + 1) - 1
                        sLine = CleanString(oTR.ReadLine())
                    Next
                Else
                    sLine = Me.CleanString(oTR.ReadLine())
                End If

                Me.CreateColumns(ldtData, sLine)

                If ldtData.Columns.Count = 0 Then
                    Return Nothing
                End If

                oTR.Close()
                oTR = File.OpenText(mrFileInfo.FullName)

                For i As Integer = 0 To (DataRow1 + 1) - 1
                    sLine = Me.CleanString(oTR.ReadLine())
                Next

                While True
                    arData = sLine.Split(New String() {msDelimiter}, StringSplitOptions.None)
                    drData = ldtData.NewRow()

                    For i As Integer = 0 To ldtData.Columns.Count - 1

                        If i < arData.Length Then
                            drData(i) = arData(i)
                        End If
                    Next

                    iRows += 1

                    If MaxRows > 0 AndAlso iRows > MaxRows Then
                        Exit While
                    End If

                    ldtData.Rows.Add(drData)
                    sLine = CleanString(oTR.ReadLine())

                    If sLine Is Nothing Then
                        Exit While
                    End If
                End While

                oTR.Close()
                oTR.Dispose()
                ldtData.AcceptChanges()

                Return ldtData

            Catch ex As Exception

                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)

                Return Nothing

            End Try

        End Function

        Private Function CleanString(ByVal asLine As String) As String

            Try
                If asLine Is Nothing Then
                    Return Nothing
                End If

                asLine = asLine.Replace("'", "''")
                asLine = asLine.Replace("""", "")
                Return asLine

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
            End Try

        End Function

        Private Sub CreateColumns(ByVal aoTable As DataTable, ByVal sLine As String)
            Try
                Dim loCol As DataColumn
                Dim lsTemp As String
                Dim liColumnCount As Integer = 0

                Dim lasData As String() = sLine.Split(New String() {msDelimiter}, StringSplitOptions.None)

                For i As Integer = 0 To lasData.Length - 1
                    lsTemp = String.Empty

                    If mvHeaderRow <> -1 Then
                        lsTemp = lasData(i)
                    End If

                    If (lsTemp.Trim()).Length = 0 Then
                        lsTemp = String.Format("ColName_{0}", i.ToString())
                    End If

                    liColumnCount = aoTable.Columns.Count + 100

                    While aoTable.Columns.Contains(lsTemp)
                        lsTemp = String.Format("ColName_{0}", liColumnCount.ToString())
                    End While

                    loCol = New DataColumn(lsTemp, System.Type.[GetType]("System.String"))

                    aoTable.Columns.Add(loCol)
                Next

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)

            End Try

        End Sub

        Public Function ExportORMQLRecordsetToCSV(ByRef arRecordset As ORMQL.Recordset, ByVal abExcludeTitles As Boolean) As Boolean

            Try
                If arRecordset Is Nothing Then
                    Return False
                End If

                Dim sLine As StringBuilder = New StringBuilder()

                If mrFileInfo.Exists Then
                    mrFileInfo.Delete()
                End If

                Dim oSW As StreamWriter = New StreamWriter(New FileStream(mrFileInfo.FullName, FileMode.Create))

                Dim liColumnCount As Integer = arRecordset.Columns.Count

                If Not abExcludeTitles Then

                    For Each lsColumnName As String In arRecordset.Columns
                        sLine.AppendFormat("{0}{1}", lsColumnName, msDelimiter)
                    Next

                    sLine.Length = sLine.Length - 1
                    oSW.WriteLine(sLine.ToString())
                End If

                For Each lrFact In arRecordset.Facts
                    sLine.Length = 0

                    For liColumnInd As Integer = 0 To arRecordset.Columns.Count - 1
                        sLine.AppendFormat("{0}{1}", Convert.ToString(lrFact.Data(liColumnInd).Data), If(liColumnInd + 1 < liColumnCount, msDelimiter, ""))
                    Next

                    oSW.WriteLine(sLine.ToString())
                Next

                oSW.Flush()
                oSW.Close()
                Return True

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)

                Return False
            End Try

        End Function

        Public Function ExportDataViewToCSV(ByVal advData As DataView, ByVal abExcludeTitles As Boolean) As Boolean
            Try

                If advData Is Nothing Then
                    Return False
                End If

                Dim sLine As StringBuilder = New StringBuilder()

                If mrFileInfo.Exists Then
                    mrFileInfo.Delete()
                End If

                Dim oSW As StreamWriter = New StreamWriter(New FileStream(mrFileInfo.FullName, FileMode.Create))

                If Not abExcludeTitles Then

                    For Each oCol As DataColumn In advData.Table.Columns
                        sLine.AppendFormat("{0}{1}", oCol.Caption, msDelimiter)
                    Next

                    sLine.Length = sLine.Length - 1
                    oSW.WriteLine(sLine.ToString())
                End If

                For i As Integer = 0 To advData.Count - 1
                    sLine.Length = 0

                    For j As Integer = 0 To advData.Table.Columns.Count - 1
                        sLine.AppendFormat("{0}{1}", Convert.ToString(advData(i)(j)), msDelimiter)
                    Next

                    oSW.WriteLine(sLine.ToString())
                Next

                oSW.Flush()
                oSW.Close()

                Return True

            Catch Exc As Exception
                Throw Exc
            End Try
        End Function
    End Class
End Namespace

