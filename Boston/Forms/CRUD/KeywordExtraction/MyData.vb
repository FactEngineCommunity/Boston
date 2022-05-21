Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports System.Runtime.InteropServices
Imports System.Text.RegularExpressions
Imports System.Collections
Imports System.IO

Enum eCodeType
	CODE_TYPE_UNKNOWN
	'type unknown
	CODE_TYPE_ASCII
	'ASCII
	CODE_TYPE_GB
	'GB2312,GBK,GB10380
	CODE_TYPE_UTF8
	'UTF-8
	CODE_TYPE_BIG5
	'BIG5
End Enum

'处理结果类型定义
<StructLayout(LayoutKind.Explicit)> _
Public Structure result_t
	<FieldOffset(0)> _
	Public start As Integer
	<FieldOffset(4)> _
	Public length As Integer
	<FieldOffset(8)> _
	Public sPos As Integer
	<FieldOffset(12)> _
	Public sPosLow As Integer
	<FieldOffset(16)> _
	Public POS_id As Integer
	<FieldOffset(20)> _
	Public word_ID As Integer
	<FieldOffset(24)> _
	Public word_type As Integer
	<FieldOffset(28)> _
	Public weight As Integer
End Structure

Public Class WORDSFRE
	Public Word As String
	Public Frequency As Integer
	Public Position As Integer()
	Public Distance As Integer()
	Public ED As [Double]
	Public IsInModel As Boolean = False

	Public Function EntropyDifference_Max() As Boolean
		Try
			'计算mean值
			Dim DocLength As Integer = 0
			Dim mean As [Double] = 0
			For Each a As Integer In Distance
				DocLength += a
			Next
			mean = 1.0 * DocLength / Frequency

			'intrinsic mode 和 extrinsic mode
			Dim intrinsic As Integer()
			Dim extrinsic As Integer()
			Dim in_num As Integer = 0, ex_num As Integer = 0
			For Each a As Integer In Distance
				If a > mean Then
					ex_num += 1
				Else
					in_num += 1
				End If
			Next
			intrinsic = New Integer(in_num - 1) {}
			extrinsic = New Integer(ex_num - 1) {}
			in_num = 0
			ex_num = 0
			For Each a As Integer In Distance
				If a > mean Then
					extrinsic(ex_num) = a
					ex_num += 1
				Else
					intrinsic(in_num) = a
					in_num += 1
				End If
			Next

			Dim ha_intrinsic As New Hashtable()
			Dim ha_extrinsic As New Hashtable()
			For Each a As Integer In intrinsic
				If ha_intrinsic.Contains(a) Then
					ha_intrinsic(a) = CInt(ha_intrinsic(a)) + 1
				Else
					ha_intrinsic.Add(a, 1)
				End If
			Next
			For Each a As Integer In extrinsic
				If ha_extrinsic.Contains(a) Then
					ha_extrinsic(a) = CInt(ha_extrinsic(a)) + 1
				Else
					ha_extrinsic.Add(a, 1)
				End If
			Next

			'计算H(dI)和H(dE)
			Dim H_dI As [Double] = 0.0
			Dim H_dE As [Double] = 0.0
			H_dI = Math.Log(ha_intrinsic.Count, 2)
			H_dE = Math.Log(ha_extrinsic.Count, 2)

			'计算EDq(d)
			Dim EDq_d As [Double] = 0.0
			Dim q As Integer = 2
			EDq_d = Math.Pow(H_dI, q) - Math.Pow(H_dE, q)
			If EDq_d <= 0 Then
				ED = -1000

				Return True
			End If

			'计算EDgeoq(d)
			Dim EDgeoq_d As [Double] = 0.0
			Dim Hgeo_dI As [Double] = 0.0
			Dim pI As [Double] = 0.0
			Dim Hgeo_dE As [Double] = 0.0
			Dim pE As [Double] = 0.0
			For i As Integer = 1 To CInt(Math.Truncate(mean)) - 1
				pI += (1 / mean) * Math.Pow(1 - 1 / mean, i - 1)
			Next
			For i As Integer = 1 To CInt(Math.Truncate(mean)) - 1
				Hgeo_dI += -((1 / mean) * Math.Pow(1 - 1 / mean, i - 1)) / pI * Math.Log(((1 / mean) * Math.Pow(1 - 1 / mean, i - 1)) / pI, 2)
			Next
			pE = 1 - pI
			For i As Integer = CInt(Math.Truncate(mean)) To DocLength - 1
				Hgeo_dE += -((1 / mean) * Math.Pow(1 - 1 / mean, i - 1)) / pE * Math.Log(((1 / mean) * Math.Pow(1 - 1 / mean, i - 1)) / pE, 2)
			Next
			EDgeoq_d = Math.Pow(Hgeo_dI, q) - Math.Pow(Hgeo_dE, q)

			'计算EDnorq(d)
			Dim EDnorq_d As [Double] = 0.0
			If Math.Abs(EDgeoq_d) <> 0 Then
				EDnorq_d = EDq_d / Math.Abs(EDgeoq_d)
			Else
				EDnorq_d = -1000
			End If

			ED = EDnorq_d

			Return True
		Catch ex As System.Exception
			Return False
		End Try
	End Function

	Public Function EntropyDifference_Normal() As Boolean
		Try
			'计算mean值
			Dim DocLength As Integer = 0
			Dim mean As [Double] = 0

			For Each a As Integer In Distance
				DocLength += a
			Next
			mean = 1.0 * DocLength / Frequency

			'intrinsic mode 和 extrinsic mode
			Dim intrinsic As Integer()
			Dim extrinsic As Integer()
			Dim in_num As Integer = 0, ex_num As Integer = 0
			For Each a As Integer In Distance
				If a > mean Then
					ex_num += 1
				Else
					in_num += 1
				End If
			Next
			intrinsic = New Integer(in_num - 1) {}
			extrinsic = New Integer(ex_num - 1) {}
			in_num = 0
			ex_num = 0
			For Each a As Integer In Distance
				If a > mean Then
					extrinsic(ex_num) = a
					ex_num += 1
				Else
					intrinsic(in_num) = a
					in_num += 1
				End If
			Next

			Dim ha_intrinsic As New Hashtable()
			Dim ha_extrinsic As New Hashtable()

			For Each a As Integer In intrinsic
				If ha_intrinsic.Contains(a) Then
					ha_intrinsic(a) = CInt(ha_intrinsic(a)) + 1
				Else
					ha_intrinsic.Add(a, 1)
				End If
			Next
			For Each a As Integer In extrinsic
				If ha_extrinsic.Contains(a) Then
					ha_extrinsic(a) = CInt(ha_extrinsic(a)) + 1
				Else
					ha_extrinsic.Add(a, 1)
				End If
			Next

			'计算H(dI)和H(dE)
			Dim H_dI As [Double] = 0.0
			Dim H_dE As [Double] = 0.0

			Dim Complete_intrinsic As Integer() = New Integer(ha_intrinsic.Count - 1) {}
			Dim Complete_extrinsic As Integer() = New Integer(ha_extrinsic.Count - 1) {}
			Dim Complete_intrinsic_Num As Integer = 0
			Dim Complete_extrinsic_Num As Integer = 0
			Dim isComplete As Boolean = False
			For Each a As Integer In intrinsic
				For i As Integer = 0 To Complete_intrinsic_Num - 1
					If a = Complete_intrinsic(i) Then
						isComplete = True
						Exit For
					End If
				Next
				If Not isComplete Then
					Dim Pd As [Double] = 1.0 * CInt(ha_intrinsic(a)) / intrinsic.Length
					H_dI += -Pd * Math.Log(Pd, 2)
					Complete_intrinsic(Complete_intrinsic_Num) = a
					Complete_intrinsic_Num += 1
				End If
				isComplete = False
			Next
			For Each a As Integer In extrinsic
				For i As Integer = 0 To Complete_extrinsic_Num - 1
					If a = Complete_extrinsic(i) Then
						isComplete = True
						Exit For
					End If
				Next
				If Not isComplete Then
					Dim Pd As [Double] = 1.0 * CInt(ha_extrinsic(a)) / extrinsic.Length
					H_dE += -Pd * Math.Log(Pd, 2)
					Complete_extrinsic(Complete_extrinsic_Num) = a
					Complete_extrinsic_Num += 1
				End If
				isComplete = False
			Next

			'计算EDq(d)
			Dim EDq_d As [Double] = 0.0
			Dim q As Integer = 2
			EDq_d = Math.Pow(H_dI, q) - Math.Pow(H_dE, q)
			EDq_d = Math.Pow(H_dI, q) - Math.Pow(H_dE, q)
			If EDq_d <= 0 Then
				ED = -1000

				Return True
			End If

			'计算EDgeoq(d)
			Dim EDgeoq_d As [Double] = 0.0
			Dim Hgeo_dI As [Double] = 0.0
			Dim pI As [Double] = 0.0
			Dim Hgeo_dE As [Double] = 0.0
			Dim pE As [Double] = 0.0
			For i As Integer = 1 To CInt(Math.Truncate(mean)) - 1
				pI += (1 / mean) * Math.Pow(1 - 1 / mean, i - 1)
			Next
			For i As Integer = 1 To CInt(Math.Truncate(mean)) - 1
				Hgeo_dI += -((1 / mean) * Math.Pow(1 - 1 / mean, i - 1)) / pI * Math.Log(((1 / mean) * Math.Pow(1 - 1 / mean, i - 1)) / pI, 2)
			Next
			pE = 1 - pI
			For i As Integer = CInt(Math.Truncate(mean)) To DocLength - 1
				Hgeo_dE += -((1 / mean) * Math.Pow(1 - 1 / mean, i - 1)) / pE * Math.Log(((1 / mean) * Math.Pow(1 - 1 / mean, i - 1)) / pE, 2)
			Next
			EDgeoq_d = Math.Pow(Hgeo_dI, q) - Math.Pow(Hgeo_dE, q)

			'计算EDnorq(d)
			Dim EDnorq_d As [Double] = 0.0
			If Math.Abs(EDgeoq_d) <> 0 Then
				EDnorq_d = EDq_d / Math.Abs(EDgeoq_d)
			Else
				EDnorq_d = -1000
			End If

			ED = EDnorq_d

			Return True
		Catch ex As System.Exception
			Return False
		End Try
	End Function
End Class

Class MyFun
	Public Shared Function DocStandardization(TheDoc As String) As String
		Try
			Dim TheDoc_New As String = ""

			'删除换行和多个空格
			Dim replaceSpace As New Regex("\s{2,}", RegexOptions.IgnoreCase)
			TheDoc = Regex.Replace(TheDoc, vbLf, " ")
			TheDoc = replaceSpace.Replace(TheDoc, " ").Trim()

			'删除标点
			TheDoc = Regex.Replace(TheDoc, "[，。、：；！？…—“”（）【】《》(),.:""'';&#?!]", " ")
			TheDoc = replaceSpace.Replace(TheDoc, " ").Trim()

			'全文转换为小写
			TheDoc_New = TheDoc.ToLower()

			Return TheDoc_New
		Catch ex As System.Exception
			Return TheDoc
		End Try
	End Function

	Public Shared Function RemoveStop(TheDoc As String) As String

		Dim path_stops As String = My.Settings.KeywordExtractionStopListLocation
		Dim encode As Encoding = Encoding.GetEncoding("GB2312")
		Dim TheDoc_New As String = ""

		Dim words As String()
		Dim stops As String()
		Dim isStop As Boolean = False
		Dim WordNum As Integer = 0
		Dim StopNum As Integer = 0

		words = TheDoc.Split(" "C)

		Try
			stops = File.ReadAllLines(path_stops, encode)
		Catch ex As System.Exception
			Return "ERROR: The file " & My.Settings.KeywordExtractionStopListLocation & " can not be found."
		End Try

		Try
			For Each wd As String In words
				For Each sp As String In stops
					If wd = sp Then
						isStop = True
						StopNum += 1
						Exit For
					End If
				Next
				If Not isStop Then
					If TheDoc_New = "" Then
						TheDoc_New += wd
					Else
						TheDoc_New += " "
						TheDoc_New += wd
					End If
				End If
				isStop = False
				WordNum += 1
			Next

			Return TheDoc_New
		Catch ex As System.Exception
			Return TheDoc
		End Try
	End Function

	Public Shared Function StatisticsWords(TheDoc As String) As WORDSFRE()
		Try
			Dim WordsFre As WORDSFRE()
			Dim words As String()
			Dim ha As New Hashtable()

			'获取全部词
			words = TheDoc.Split(" "C)

			'统计词频
			For Each wd As String In words
				If ha.Contains(wd) Then
					ha(wd) = CInt(ha(wd)) + 1
				Else
					ha.Add(wd, 1)
				End If
			Next

			'统计位置信息
			WordsFre = New WORDSFRE(ha.Count - 1) {}
			Dim WordsFreID As Integer = 0
			Dim ExistID As Integer = 0
			Dim isExist As Boolean = False
			For i As Integer = 0 To words.Length - 1
				For j As Integer = 0 To WordsFreID - 1
					If words(i) = WordsFre(j).Word Then
						isExist = True
						ExistID = j
						Exit For
					End If
				Next
				If Not isExist Then
					WordsFre(WordsFreID) = New WORDSFRE()
					WordsFre(WordsFreID).Position = New Integer(CInt(ha(words(i))) - 1) {}
					WordsFre(WordsFreID).Distance = New Integer(CInt(ha(words(i))) - 1) {}
					WordsFre(WordsFreID).Word = words(i)
					WordsFre(WordsFreID).Frequency = 1
					WordsFre(WordsFreID).Position(0) = i
					WordsFreID += 1
				Else
					WordsFre(ExistID).Position(WordsFre(ExistID).Frequency) = i
					WordsFre(ExistID).Frequency += 1
					isExist = False
				End If
			Next

			For i As Integer = 0 To WordsFre.Length - 1
				For j As Integer = 0 To WordsFre(i).Position.Length - 1
					If j = 0 Then
						WordsFre(i).Distance(j) = WordsFre(i).Position(j) + words.Length - WordsFre(i).Position(WordsFre(i).Position.Length - 1)
					Else
						WordsFre(i).Distance(j) = WordsFre(i).Position(j) - WordsFre(i).Position(j - 1)
					End If
				Next
			Next

			Return WordsFre
		Catch ex As System.Exception
			Return Nothing
		End Try
	End Function

	Public Shared Function QuickSort(array As WORDSFRE(), left As Integer, right As Integer) As Boolean
		Try
			If left < right Then
				Dim middle As Integer = GetMiddleFroQuickSort(array, left, right)
				If middle = -1 Then
					Return False
				End If
				QuickSort(array, left, middle - 1)
				QuickSort(array, middle + 1, right)
			End If
			Return True
		Catch ex As System.Exception
			Return False
		End Try
	End Function

	Private Shared Function GetMiddleFroQuickSort(array As WORDSFRE(), left As Integer, right As Integer) As Integer
		Try
			Dim key As WORDSFRE = array(left)
			While left < right
				While left < right AndAlso key.ED.CompareTo(array(right).ED) > 0
					right -= 1
				End While
				If left < right Then
					Dim temp As WORDSFRE = array(left)
					array(left) = array(right)
					left += 1
				End If

				While left < right AndAlso key.ED.CompareTo(array(left).ED) < 0
					left += 1
				End While
				If left < right Then
					Dim temp As WORDSFRE = array(right)
					array(right) = array(left)
					right -= 1
				End If
				array(left) = key
			End While
			Return left
		Catch ex As System.Exception
			Return -1
		End Try
	End Function
End Class

Class MyData
	Public Shared TheDoc As String
	Public Shared WordsFre As WORDSFRE()
End Class
