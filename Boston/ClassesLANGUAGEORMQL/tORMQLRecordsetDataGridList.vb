Imports System.ComponentModel
Imports System.Reflection
Imports DynamicClassLibrary.Factory

Namespace ORMQL
    Public Class RecordsetDataGridList
        Implements IBindingListView

        Public mrRecordset As ORMQL.Recordset
        Public mrTable As RDS.Table
        Private DynamicClass As tClass
        Public DynamicObject As Object


        Public Sub New(ByRef arRecordset As ORMQL.Recordset,
                       ByRef arTable As RDS.Table)

            Me.mrRecordset = arRecordset
            Me.mrTable = arTable

            Me.DynamicClass = New tClass
            For Each lsColumn In Me.mrRecordset.Columns

                Dim lsColumnName As String
                Try
                    lsColumnName = lsColumn.Substring(lsColumn.IndexOf(".") + 1)
                Catch ex As Exception
                    lsColumnName = Trim(lsColumn)
                End Try


                Select Case lsColumnName
                    Case Is = "Date"
                        Me.DynamicClass.add_attribute(New tAttribute("[" & lsColumnName & "]", GetType(Date)))
                    Case Else
                        Me.DynamicClass.add_attribute(New tAttribute(lsColumnName, GetType(String)))
                End Select
            Next
            Me.DynamicObject = Me.DynamicClass.clone()

        End Sub

        Public ReadOnly Property AllowNew As Boolean Implements IBindingList.AllowNew
            Get
                Return True
            End Get
        End Property

        Public ReadOnly Property AllowEdit As Boolean Implements IBindingList.AllowEdit
            Get
                Return True
            End Get
        End Property

        Public ReadOnly Property AllowRemove As Boolean Implements IBindingList.AllowRemove
            Get
                Return True
            End Get
        End Property

        Public ReadOnly Property SupportsChangeNotification As Boolean Implements IBindingList.SupportsChangeNotification
            Get
                Return True
            End Get
        End Property

        Public ReadOnly Property SupportsSearching As Boolean Implements IBindingList.SupportsSearching
            Get
                Return True
            End Get
        End Property

        Public ReadOnly Property SupportsSorting As Boolean Implements IBindingList.SupportsSorting
            Get
                Return False
            End Get
        End Property

        Public ReadOnly Property IsSorted As Boolean Implements IBindingList.IsSorted
            Get
                Return False
            End Get
        End Property

        Public ReadOnly Property SortProperty As PropertyDescriptor Implements IBindingList.SortProperty
            Get
                Return Nothing 'Throw New NotImplementedException()
            End Get
        End Property

        Public ReadOnly Property SortDirection As ListSortDirection Implements IBindingList.SortDirection
            Get
                Return ListSortDirection.Ascending
            End Get
        End Property

        Default Public Property Item(index As Integer) As Object Implements IList.Item
            Get
                Try
                    Dim liInd As Integer = 0
                    Dim lsDataAsString As String

                    For Each lrData In Me.mrRecordset.Facts(index).Data

                        Try
                            Dim lsColumn As String = Me.mrRecordset.Columns(liInd)
                            Dim lsColumnName As String
                            Try
                                lsColumnName = lsColumn.Substring(lsColumn.IndexOf(".") + 1)
                            Catch ex As Exception
                                lsColumnName = lsColumn
                            End Try

                            'Dim lsString = Me.mrRecordset.Columns(liInd)
                            Dim piInstance As PropertyInfo = Me.DynamicObject.GetType.GetProperty(lsColumnName)

                            Select Case Me.mrTable.Column.Find(Function(x) x.Name = lsColumnName).getMetamodelDataType 'Was lsString
                                Case Is = pcenumORMDataType.TemporalDate,
                                          pcenumORMDataType.TemporalDateAndTime
                                    Try
                                        'lsDataAsString = DateTime.ParseExact(lrData.Data,
                                        '                                     System.Threading.Thread.CurrentThread.CurrentUICulture.DateTimeFormat.FullDateTimePattern,
                                        '                                     System.Globalization.CultureInfo.InvariantCulture).ToString(System.Threading.Thread.CurrentThread.CurrentUICulture.DateTimeFormat.FullDateTimePattern)
                                        lsDataAsString = Convert.ToDateTime(lrData.Data).ToString(My.Settings.FactEngineUserDateTimeFormat)
                                    Catch ex As Exception
                                        lsDataAsString = lrData.Data
                                    End Try
                                Case Else
                                    lsDataAsString = lrData.Data
                            End Select

                            piInstance.SetValue(Me.DynamicObject, lsDataAsString)
                        Catch ex As Exception
                            'Not a biggie. Column's data won't be set,
                            ' but indicates to the user there is something wrong with the Column; possibly ColumnName mismatch between Model and Database Table/NodeType.
                        End Try

                        liInd += 1
                    Next
                    Return Me.DynamicObject.clone
                Catch ex As Exception
                    Return Me.DynamicObject
                End Try
            End Get
            Set(value As Object)
                Throw New NotImplementedException()
            End Set
        End Property

        Public ReadOnly Property IsReadOnly As Boolean Implements IList.IsReadOnly
            Get
                Return False
            End Get
        End Property

        Public ReadOnly Property IsFixedSize As Boolean Implements IList.IsFixedSize
            Get
                Return False
            End Get
        End Property

        Public ReadOnly Property Count As Integer Implements ICollection.Count
            Get
                Return Me.mrRecordset.Facts.Count
            End Get
        End Property

        Public ReadOnly Property SyncRoot As Object Implements ICollection.SyncRoot
            Get
                Throw New NotImplementedException()
            End Get
        End Property

        Public ReadOnly Property IsSynchronized As Boolean Implements ICollection.IsSynchronized
            Get
                Return True
            End Get
        End Property

        Public Property Filter As String Implements IBindingListView.Filter
            Get
                Throw New NotImplementedException()
            End Get
            Set(value As String)
                Throw New NotImplementedException()
            End Set
        End Property

        Public ReadOnly Property SortDescriptions As ListSortDescriptionCollection Implements IBindingListView.SortDescriptions
            Get
                Throw New NotImplementedException()
            End Get
        End Property

        Public ReadOnly Property SupportsAdvancedSorting As Boolean Implements IBindingListView.SupportsAdvancedSorting
            Get
                Return False
            End Get
        End Property

        Public ReadOnly Property SupportsFiltering As Boolean Implements IBindingListView.SupportsFiltering
            Get
                Return False
            End Get
        End Property

        Public Event ListChanged As ListChangedEventHandler Implements IBindingList.ListChanged

        Public Sub AddIndex([property] As PropertyDescriptor) Implements IBindingList.AddIndex
            Throw New NotImplementedException()
        End Sub

        Public Sub ApplySort([property] As PropertyDescriptor, direction As ListSortDirection) Implements IBindingList.ApplySort
            Throw New NotImplementedException()
        End Sub

        Public Sub RemoveIndex([property] As PropertyDescriptor) Implements IBindingList.RemoveIndex
            Throw New NotImplementedException()
        End Sub

        Public Sub RemoveSort() Implements IBindingList.RemoveSort
            Throw New NotImplementedException()
        End Sub

        Public Sub Clear() Implements IList.Clear
            'Throw New NotImplementedException()
        End Sub

        Public Sub Insert(index As Integer, value As Object) Implements IList.Insert
            Throw New NotImplementedException()
        End Sub

        Public Sub Remove(value As Object) Implements IList.Remove
            Throw New NotImplementedException()
        End Sub

        Public Sub RemoveAt(index As Integer) Implements IList.RemoveAt
            'Me.mrRecordset.Facts.RemoveAt(index)
        End Sub

        Public Sub CopyTo(array As Array, index As Integer) Implements ICollection.CopyTo
            Throw New NotImplementedException()
        End Sub

        Public Function AddNew() As Object Implements IBindingList.AddNew
            'Throw New NotImplementedException()
        End Function

        Public Function Find([property] As PropertyDescriptor, key As Object) As Integer Implements IBindingList.Find
            'Throw New NotImplementedException()
            Dim larFact = From Fact In Me.mrRecordset.Facts
                          From FactData In Fact.Data
                          Where FactData.Role.Name = [property].Name
                          Where FactData.Data = key
                          Select Me.mrRecordset.Facts.IndexOf(Fact)

            If larFact.Count > 0 Then
                Return larFact.First
            Else
                Return 0
            End If

        End Function

        Public Function Add(value As Object) As Integer Implements IList.Add
            Throw New NotImplementedException()
        End Function

        Public Function Contains(value As Object) As Boolean Implements IList.Contains
            Throw New NotImplementedException()
        End Function

        Public Function IndexOf(value As Object) As Integer Implements IList.IndexOf
            Throw New NotImplementedException()
        End Function

        Public Function GetEnumerator() As IEnumerator Implements IEnumerable.GetEnumerator
            Dim larItem As New List(Of Object)
            For liInd = 0 To Me.mrRecordset.Facts.Count - 1
                larItem.Add(Me.Item(liInd).clone)
            Next
            Return larItem.GetEnumerator
        End Function

        Public Sub ApplySort(sorts As ListSortDescriptionCollection) Implements IBindingListView.ApplySort
            Throw New NotImplementedException()
        End Sub

        Public Sub RemoveFilter() Implements IBindingListView.RemoveFilter
            Throw New NotImplementedException()
        End Sub
    End Class
End Namespace