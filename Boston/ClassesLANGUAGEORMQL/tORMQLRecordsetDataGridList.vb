Imports System.ComponentModel
Imports System.Reflection

Namespace ORMQL
    Public Class RecordsetDataGridList
        Implements IBindingList

        Public mrRecordset As ORMQL.Recordset
        Public mrTable As RDS.Table
        Private DynamicClass As DynamicClassLibrary.Factory.tClass
        Public DynamicObject As Object


        Public Sub New(ByRef arRecordset As ORMQL.Recordset,
                       ByRef arTable As RDS.Table)

            Me.mrRecordset = arRecordset
            Me.mrTable = arTable

            Me.DynamicClass = New DynamicClassLibrary.Factory.tClass
            For Each lsColumn In Me.mrRecordset.Columns
                Select Case lsColumn
                    Case Is = "Date"
                        Me.DynamicClass.add_attribute(New DynamicClassLibrary.Factory.tAttribute("[" & lsColumn & "]", GetType(String)))
                    Case Else
                        Me.DynamicClass.add_attribute(New DynamicClassLibrary.Factory.tAttribute(lsColumn, GetType(String)))
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

                    For Each lrData In Me.mrRecordset.Facts(index).Data
                        Dim lsString = Me.mrRecordset.Columns(liInd)
                        Dim piInstance As PropertyInfo = Me.DynamicObject.GetType.GetProperty(lsString)
                        piInstance.SetValue(Me.DynamicObject, lrData.Data)
                        liInd += 1
                    Next
                    Return Me.DynamicObject
                Catch ex As Exception
                    Return Nothing
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
            Throw New NotImplementedException()
        End Sub

        Public Sub CopyTo(array As Array, index As Integer) Implements ICollection.CopyTo
            Throw New NotImplementedException()
        End Sub

        Public Function AddNew() As Object Implements IBindingList.AddNew
            'Throw New NotImplementedException()
        End Function

        Public Function Find([property] As PropertyDescriptor, key As Object) As Integer Implements IBindingList.Find
            Throw New NotImplementedException()
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
            Throw New NotImplementedException()
        End Function
    End Class
End Namespace