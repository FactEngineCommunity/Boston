'From: http://www.codeproject.com/KB/vb/Generic_collection_sorter.aspx

Imports System.Reflection

Namespace Tools

    Friend Class ListSorter(Of T)
        Implements IComparer(Of T)

#Region "Private Variables"
        Private _sortColumn As String
        Private _reverse As Boolean
#End Region

#Region "Constructor"
        Public Sub New(ByVal sortEx As String)
            'find if we want to sort asc or desc
            If Not String.IsNullOrEmpty(sortEx) Then
                _reverse = sortEx.ToLowerInvariant().EndsWith(" desc")

                If _reverse Then
                    _sortColumn = sortEx.Substring(0, sortEx.Length - 5)
                Else
                    If sortEx.ToLowerInvariant().EndsWith(" asc") Then
                        _sortColumn = sortEx.Substring(0, sortEx.Length - 4)
                    Else
                        _sortColumn = sortEx
                    End If
                End If

            End If
        End Sub
#End Region

#Region "Interface Implementation"

        Public Function Compare(ByVal x As T, ByVal y As T) As Integer Implements System.Collections.Generic.IComparer(Of T).Compare
            'get the properties of the objects
            Dim propsx() As PropertyInfo = x.GetType().GetProperties(System.Reflection.BindingFlags.Instance Or System.Reflection.BindingFlags.Public)
            Dim propsy() As PropertyInfo = x.GetType().GetProperties(System.Reflection.BindingFlags.Instance Or System.Reflection.BindingFlags.Public)
            Dim retval As Integer

            'find the column we want to sort based on
            For i As Integer = 0 To propsx.Length - 1
                If _sortColumn.ToLower() = propsx(i).Name.ToLower() Then
                    'find the type of column so we know how to sort
                    Select Case propsx(i).PropertyType.Name
                        Case "String"
                            retval = CStr(propsx(i).GetValue(x, Nothing)).CompareTo(CStr(propsy(i).GetValue(y, Nothing)))
                        Case "Integer"
                            retval = CInt(propsx(i).GetValue(x, Nothing)).CompareTo(CInt(propsy(i).GetValue(y, Nothing)))
                        Case "Int32"
                            retval = CInt(propsx(i).GetValue(x, Nothing)).CompareTo(CInt(propsy(i).GetValue(y, Nothing)))
                        Case "Int16"
                            retval = CInt(propsx(i).GetValue(x, Nothing)).CompareTo(CInt(propsy(i).GetValue(y, Nothing)))
                        Case "DateTime"
                            retval = CDate(propsx(i).GetValue(x, Nothing)).CompareTo(CDate(propsy(i).GetValue(y, Nothing)))
                    End Select

                End If
            Next

            If _reverse Then
                Return -1 * retval
            Else
                Return retval
            End If

        End Function
#End Region

#Region "Equal Function"

        Public Shadows Function Equals(ByVal x As T, ByVal y As T) As Boolean
            Dim propsx() As PropertyInfo = x.GetType().GetProperties(System.Reflection.BindingFlags.Instance Or System.Reflection.BindingFlags.Public)
            Dim propsy() As PropertyInfo = y.GetType().GetProperties(System.Reflection.BindingFlags.Instance Or System.Reflection.BindingFlags.Public)
            Dim retval As Boolean
            For i As Integer = 0 To propsx.Length - 1
                If _sortColumn.ToLower() = propsx(i).Name.ToLower() Then
                    retval = propsx(i).GetValue(x, Nothing).Equals(propsy(i).GetValue(y, Nothing))
                End If
            Next
            Return retval
        End Function
#End Region

    End Class

End Namespace
