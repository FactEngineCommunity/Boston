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

        Public Property FileInf As FileInfo
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
        Public Property Delimiter As String
        Public Property MaxRows As Integer

        Public ReadOnly Property NameOnly As String
            Get
                Return FileInf.Name.Substring(0, (FileInf.Name.Length - FileInf.Extension.Length))
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
            FileInf = New FileInfo(sFilename)
        End Sub

        Private Function GetUNCPath() As String
            Dim sPath As StringBuilder = New StringBuilder()
            Dim sNetLtr As String
            Dim sLtr As String = FileInf.FullName.Substring(0, 2)
            Dim query As SelectQuery = New SelectQuery("select name, ProviderName from win32_logicaldisk where drivetype=4")
            Dim searcher As ManagementObjectSearcher = New ManagementObjectSearcher(query)

            For Each mo As ManagementObject In searcher.[Get]()
                sNetLtr = Convert.ToString(mo("name"))

                If sNetLtr = sLtr Then
                    sPath.AppendFormat("{0}{1}", mo("ProviderName"), FileInf.DirectoryName.Substring(2))
                End If
            Next

            Return sPath.ToString()
        End Function

        Public Function CSVToDataTable() As DataTable

            Try
                If FileInf Is Nothing Then
                    Return Nothing
                End If

                Dim dtData As DataTable = New DataTable()
                Dim oTR As TextReader = File.OpenText(FileInf.FullName)
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

                Me.CreateColumns(dtData, sLine)

                If dtData.Columns.Count = 0 Then
                    Return Nothing
                End If

                oTR.Close()
                oTR = File.OpenText(FileInf.FullName)

                For i As Integer = 0 To (DataRow1 + 1) - 1
                    sLine = Me.CleanString(oTR.ReadLine())
                Next

                While True
                    arData = sLine.Split(New String() {Delimiter}, StringSplitOptions.None)
                    drData = dtData.NewRow()

                    For i As Integer = 0 To dtData.Columns.Count - 1

                        If i < arData.Length Then
                            drData(i) = arData(i)
                        End If
                    Next

                    iRows += 1

                    If MaxRows > 0 AndAlso iRows > MaxRows Then
                        Exit While
                    End If

                    dtData.Rows.Add(drData)
                    sLine = CleanString(oTR.ReadLine())

                    If sLine Is Nothing Then
                        Exit While
                    End If
                End While

                oTR.Close()
                oTR.Dispose()
                dtData.AcceptChanges()

                Return dtData

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

                Dim lasData As String() = sLine.Split(New String() {Delimiter}, StringSplitOptions.None)

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

        Public Function TableToCSV(ByVal dvData As DataView, ByVal bExcludeTitles As Boolean) As Boolean
            Try

                If dvData Is Nothing Then
                    Return False
                End If

                Dim sLine As StringBuilder = New StringBuilder()

                If FileInf.Exists Then
                    FileInf.Delete()
                End If

                Dim oSW As StreamWriter = New StreamWriter(New FileStream(FileInf.FullName, FileMode.Create))

                If Not bExcludeTitles Then

                    For Each oCol As DataColumn In dvData.Table.Columns
                        sLine.AppendFormat("{0}{1}", oCol.Caption, Delimiter)
                    Next

                    sLine.Length = sLine.Length - 1
                    oSW.WriteLine(sLine.ToString())
                End If

                For i As Integer = 0 To dvData.Count - 1
                    sLine.Length = 0

                    For j As Integer = 0 To dvData.Table.Columns.Count - 1
                        sLine.AppendFormat("{0}{1}", Convert.ToString(dvData(i)(j)), Delimiter)
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

