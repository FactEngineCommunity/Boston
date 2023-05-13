Imports System.Reflection
Imports ADODB
Imports LinFu.DynamicProxy
Imports Microsoft.VisualBasic.Compatibility.VB6

<Microsoft.VisualBasic.ComClass()>
Public Class RecordsetProxy
    Inherits DispatchProxy
    Implements _Recordset


    Private _innerRecordset As Object

    Public Property ActiveConnection As Object Implements _Recordset.ActiveConnection
        Get
            Return Me._innerRecordset.ActiveConnection
        End Get
        Set(value As Object)
            Select Case value.GetType()
                Case GetType(FactEngine.SQLiteConnection)
                    _innerRecordset = New SQLite.Recordset
                Case Else
                    _innerRecordset = New ADODB.Recordset()
            End Select
            Me._innerRecordset.ActiveConnection = value
        End Set
    End Property

    Private _CursorType As Integer = 0
    Public Property CursorType As CursorTypeEnum Implements _Recordset.CursorType
        Get
            Return Me._CursorType
        End Get
        Set(value As CursorTypeEnum)
            Me._CursorType = value
        End Set
    End Property

    'Public Overridable Property ActiveConnection As Object Implements _Recordset.ActiveConnection
    '    Get
    '        Return Me.Invoke(Sub() Return _innerRecordset.ActiveConnection End Sub)
    '    End Get
    '    Set(value As Object)
    '        Me.Invoke(Sub() _innerRecordset.ActiveConnection = value)
    '    End Set
    'End Property

    Default Public Overridable Property Item(ByVal asItemValue As String) As Object
        Get
            Try
                If Not asItemValue.IsNumeric Then
                    Return Me._innerRecordset(asItemValue)

                Else
                    Return Me._innerRecordset(CInt(asItemValue))
                End If
            Catch ex As Exception
                Return Nothing
            End Try


        End Get
        Set(value As Object)
        End Set
    End Property

    ''' <summary>
    ''' Paramerless Constructor
    ''' </summary>
    Public Sub New()
    End Sub

    Public Sub New(ByVal innerRecordset As ADODB.Recordset)
        Me._innerRecordset = innerRecordset
    End Sub

    Protected Overrides Function Invoke(targetMethod As MethodInfo, args() As Object) As Object

        'remove "set_" or "get_" 
        Dim propertyName As String = targetMethod.Name.Substring(4)

        If propertyName = "ActiveConnection" AndAlso args IsNot Nothing AndAlso args.Length > 0 Then
            Dim connection = args(0)
            Select Case connection.GetType()
                Case GetType(FactEngine.SQLiteConnection)
                    _innerRecordset = New SQLite.Recordset
                Case Else
                    _innerRecordset = New ADODB.Recordset()
            End Select

            Return targetMethod.Invoke(_innerRecordset, args)
        Else
            Return targetMethod.Invoke(_innerRecordset, args)
        End If

    End Function

    Public Shared Shadows Function Create(innerRecordset As ADODB.Recordset) As Object

        Dim proxy As ADODB.Recordset = DispatchProxy.Create(Of ADODB.Recordset, RecordsetProxy)()
        Dim proxyObject As Object = proxy
        Dim innerObject As Object = innerRecordset
        Dim proxyRecordset As ADODB.Recordset = CType(proxyObject, ADODB.Recordset)
        Dim innerRecordsetObject As ADODB.Recordset = CType(innerObject, ADODB.Recordset)

        ' Set the inner recordset for the proxy
        proxyRecordset.Source = innerRecordsetObject

        Return proxyRecordset
    End Function

    Public Sub SetRecordset(recordset As ADODB.Recordset)
        Me._innerRecordset = recordset
    End Sub

    Public Sub let_ActiveConnection(pvar As Object) Implements _Recordset.let_ActiveConnection
        Throw New NotImplementedException()
    End Sub

    Public Sub let_Source(pvSource As String) Implements _Recordset.let_Source
        Throw New NotImplementedException()
    End Sub

    Public Sub AddNew(Optional FieldList As Object = Nothing, Optional Values As Object = Nothing) Implements _Recordset.AddNew
        Throw New NotImplementedException()
    End Sub

    Public Sub CancelUpdate() Implements _Recordset.CancelUpdate
        Throw New NotImplementedException()
    End Sub

    Public Sub Close() Implements _Recordset.Close
        Call Me._innerRecordset.Close
    End Sub

    Public Sub Delete(Optional AffectRecords As AffectEnum = AffectEnum.adAffectCurrent) Implements _Recordset.Delete
        Throw New NotImplementedException()
    End Sub

    Public Function GetRows(Optional Rows As Integer = -1, Optional Start As Object = Nothing, Optional Fields As Object = Nothing) As Object Implements _Recordset.GetRows
        Throw New NotImplementedException()
    End Function

    Public Sub Move(NumRecords As Integer, Optional Start As Object = Nothing) Implements _Recordset.Move
        Throw New NotImplementedException()
    End Sub

    Public Sub MoveNext() Implements _Recordset.MoveNext
        Call Me._innerRecordset.MoveNext
    End Sub

    Public Sub MovePrevious() Implements _Recordset.MovePrevious
        Throw New NotImplementedException()
    End Sub

    Public Sub MoveFirst() Implements _Recordset.MoveFirst
        Call Me._innerRecordset.MoveFirst
    End Sub

    Public Sub MoveLast() Implements _Recordset.MoveLast
        Throw New NotImplementedException()
    End Sub

    Public Sub Open(Optional Source As Object = Nothing, Optional ActiveConnection As Object = Nothing, Optional CursorType As CursorTypeEnum = CursorTypeEnum.adOpenUnspecified, Optional LockType As LockTypeEnum = LockTypeEnum.adLockUnspecified, Optional Options As Integer = -1) Implements _Recordset.Open
        Try
            Me._innerRecordset.Open(Source)
        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
        End Try
    End Sub

    Public Sub Requery(Optional Options As Integer = -1) Implements _Recordset.Requery
        Throw New NotImplementedException()
    End Sub

    Public Sub _xResync(Optional AffectRecords As AffectEnum = AffectEnum.adAffectAll) Implements _Recordset._xResync
        Throw New NotImplementedException()
    End Sub

    Public Sub Update(Optional Fields As Object = Nothing, Optional Values As Object = Nothing) Implements _Recordset.Update
        Throw New NotImplementedException()
    End Sub

    Public Function _xClone() As Recordset Implements _Recordset._xClone
        Throw New NotImplementedException()
    End Function

    Public Sub UpdateBatch(Optional AffectRecords As AffectEnum = AffectEnum.adAffectAll) Implements _Recordset.UpdateBatch
        Throw New NotImplementedException()
    End Sub

    Public Sub CancelBatch(Optional AffectRecords As AffectEnum = AffectEnum.adAffectAll) Implements _Recordset.CancelBatch
        Throw New NotImplementedException()
    End Sub

    Public Function NextRecordset(ByRef Optional RecordsAffected As Object = Nothing) As Recordset Implements _Recordset.NextRecordset
        Throw New NotImplementedException()
    End Function

    Public Function Supports(CursorOptions As CursorOptionEnum) As Boolean Implements _Recordset.Supports
        Throw New NotImplementedException()
    End Function

    Public Sub Find(Criteria As String, Optional SkipRecords As Integer = 0, Optional SearchDirection As SearchDirectionEnum = SearchDirectionEnum.adSearchForward, Optional Start As Object = Nothing) Implements _Recordset.Find
        Throw New NotImplementedException()
    End Sub

    Public Sub Cancel() Implements _Recordset.Cancel
        Throw New NotImplementedException()
    End Sub

    Public Sub _xSave(Optional FileName As String = "", Optional PersistFormat As PersistFormatEnum = PersistFormatEnum.adPersistADTG) Implements _Recordset._xSave
        Throw New NotImplementedException()
    End Sub

    Public Function GetString(Optional StringFormat As StringFormatEnum = StringFormatEnum.adClipString, Optional NumRows As Integer = -1, Optional ColumnDelimeter As String = "", Optional RowDelimeter As String = "", Optional NullExpr As String = "") As String Implements _Recordset.GetString
        Throw New NotImplementedException()
    End Function

    Public Function CompareBookmarks(Bookmark1 As Object, Bookmark2 As Object) As CompareEnum Implements _Recordset.CompareBookmarks
        Throw New NotImplementedException()
    End Function

    Public Function Clone(Optional LockType As LockTypeEnum = LockTypeEnum.adLockUnspecified) As Recordset Implements _Recordset.Clone
        Throw New NotImplementedException()
    End Function

    Public Sub Resync(Optional AffectRecords As AffectEnum = AffectEnum.adAffectAll, Optional ResyncValues As ResyncEnum = ResyncEnum.adResyncAllValues) Implements _Recordset.Resync
        Throw New NotImplementedException()
    End Sub

    Public Sub Seek(KeyValues As Object, Optional SeekOption As SeekEnum = SeekEnum.adSeekFirstEQ) Implements _Recordset.Seek
        Throw New NotImplementedException()
    End Sub

    Public Sub Save(Optional Destination As Object = Nothing, Optional PersistFormat As PersistFormatEnum = PersistFormatEnum.adPersistADTG) Implements _Recordset.Save
        Throw New NotImplementedException()
    End Sub

    Public ReadOnly Property Properties As ADODB.Properties Implements _Recordset.Properties
        Get
            Throw New NotImplementedException()
        End Get
    End Property

    Public Property AbsolutePosition As PositionEnum Implements _Recordset.AbsolutePosition
        Get
            Throw New NotImplementedException()
        End Get
        Set(value As PositionEnum)
            Throw New NotImplementedException()
        End Set
    End Property

    Public ReadOnly Property BOF As Boolean Implements _Recordset.BOF
        Get
            Throw New NotImplementedException()
        End Get
    End Property

    Public Property Bookmark As Object Implements _Recordset.Bookmark
        Get
            Throw New NotImplementedException()
        End Get
        Set(value As Object)
            Throw New NotImplementedException()
        End Set
    End Property

    Public Property CacheSize As Integer Implements _Recordset.CacheSize
        Get
            Throw New NotImplementedException()
        End Get
        Set(value As Integer)
            Throw New NotImplementedException()
        End Set
    End Property


    Public ReadOnly Property EOF As Boolean Implements _Recordset.EOF
        Get
            Return Me._innerRecordset.EOF
        End Get
    End Property

    Public ReadOnly Property Fields As Fields Implements _Recordset.Fields
        Get
            Return Me._innerRecordset.Fields
        End Get
    End Property

    Public Property LockType As LockTypeEnum Implements _Recordset.LockType
        Get
            Throw New NotImplementedException()
        End Get
        Set(value As LockTypeEnum)
            Throw New NotImplementedException()
        End Set
    End Property

    Public Property MaxRecords As Integer Implements _Recordset.MaxRecords
        Get
            Throw New NotImplementedException()
        End Get
        Set(value As Integer)
            Throw New NotImplementedException()
        End Set
    End Property

    Public ReadOnly Property RecordCount As Integer Implements _Recordset.RecordCount
        Get
            Throw New NotImplementedException()
        End Get
    End Property

    Public Property Source As Object Implements _Recordset.Source
        Get
            Throw New NotImplementedException()
        End Get
        Set(value As Object)
            Throw New NotImplementedException()
        End Set
    End Property

    Public Property AbsolutePage As PositionEnum Implements _Recordset.AbsolutePage
        Get
            Throw New NotImplementedException()
        End Get
        Set(value As PositionEnum)
            Throw New NotImplementedException()
        End Set
    End Property

    Public ReadOnly Property EditMode As EditModeEnum Implements _Recordset.EditMode
        Get
            Throw New NotImplementedException()
        End Get
    End Property

    Public Property Filter As Object Implements _Recordset.Filter
        Get
            Throw New NotImplementedException()
        End Get
        Set(value As Object)
            Throw New NotImplementedException()
        End Set
    End Property

    Public ReadOnly Property PageCount As Integer Implements _Recordset.PageCount
        Get
            Throw New NotImplementedException()
        End Get
    End Property

    Public Property PageSize As Integer Implements _Recordset.PageSize
        Get
            Throw New NotImplementedException()
        End Get
        Set(value As Integer)
            Throw New NotImplementedException()
        End Set
    End Property

    Public Property Sort As String Implements _Recordset.Sort
        Get
            Throw New NotImplementedException()
        End Get
        Set(value As String)
            Throw New NotImplementedException()
        End Set
    End Property

    Public ReadOnly Property Status As Integer Implements _Recordset.Status
        Get
            Throw New NotImplementedException()
        End Get
    End Property

    Public ReadOnly Property State As Integer Implements _Recordset.State
        Get
            Throw New NotImplementedException()
        End Get
    End Property

    Public Property CursorLocation As CursorLocationEnum Implements _Recordset.CursorLocation
        Get
            Throw New NotImplementedException()
        End Get
        Set(value As CursorLocationEnum)
            Throw New NotImplementedException()
        End Set
    End Property

    Public Property Collect(Index As Object) As Object Implements _Recordset.Collect
        Get
            Throw New NotImplementedException()
        End Get
        Set(value As Object)
            Throw New NotImplementedException()
        End Set
    End Property

    Public Property MarshalOptions As MarshalOptionsEnum Implements _Recordset.MarshalOptions
        Get
            Throw New NotImplementedException()
        End Get
        Set(value As MarshalOptionsEnum)
            Throw New NotImplementedException()
        End Set
    End Property

    Public Property DataSource As Object Implements _Recordset.DataSource
        Get
            Throw New NotImplementedException()
        End Get
        Set(value As Object)
            Throw New NotImplementedException()
        End Set
    End Property

    Public ReadOnly Property ActiveCommand As Object Implements _Recordset.ActiveCommand
        Get
            Throw New NotImplementedException()
        End Get
    End Property

    Public Property StayInSync As Boolean Implements _Recordset.StayInSync
        Get
            Throw New NotImplementedException()
        End Get
        Set(value As Boolean)
            Throw New NotImplementedException()
        End Set
    End Property

    Public Property DataMember As String Implements _Recordset.DataMember
        Get
            Throw New NotImplementedException()
        End Get
        Set(value As String)
            Throw New NotImplementedException()
        End Set
    End Property

    Public Property Index As String Implements _Recordset.Index
        Get
            Throw New NotImplementedException()
        End Get
        Set(value As String)
            Throw New NotImplementedException()
        End Set
    End Property

    Private Sub Recordset21_let_ActiveConnection(pvar As Object) Implements Recordset21.let_ActiveConnection
        Throw New NotImplementedException()
    End Sub

    Private Sub Recordset21_let_Source(pvSource As String) Implements Recordset21.let_Source
        Throw New NotImplementedException()
    End Sub

    Private Sub Recordset21_AddNew(Optional FieldList As Object = Nothing, Optional Values As Object = Nothing) Implements Recordset21.AddNew
        Throw New NotImplementedException()
    End Sub

    Private Sub Recordset21_CancelUpdate() Implements Recordset21.CancelUpdate
        Throw New NotImplementedException()
    End Sub

    Private Sub Recordset21_Close() Implements Recordset21.Close
        Throw New NotImplementedException()
    End Sub

    Private Sub Recordset21_Delete(Optional AffectRecords As AffectEnum = AffectEnum.adAffectCurrent) Implements Recordset21.Delete
        Throw New NotImplementedException()
    End Sub

    Private Function Recordset21_GetRows(Optional Rows As Integer = -1, Optional Start As Object = Nothing, Optional Fields As Object = Nothing) As Object Implements Recordset21.GetRows
        Throw New NotImplementedException()
    End Function

    Private Sub Recordset21_Move(NumRecords As Integer, Optional Start As Object = Nothing) Implements Recordset21.Move
        Throw New NotImplementedException()
    End Sub

    Private Sub Recordset21_MoveNext() Implements Recordset21.MoveNext
        Throw New NotImplementedException()
    End Sub

    Private Sub Recordset21_MovePrevious() Implements Recordset21.MovePrevious
        Throw New NotImplementedException()
    End Sub

    Private Sub Recordset21_MoveFirst() Implements Recordset21.MoveFirst
        Throw New NotImplementedException()
    End Sub

    Private Sub Recordset21_MoveLast() Implements Recordset21.MoveLast
        Throw New NotImplementedException()
    End Sub

    Private Sub Recordset21_Open(Optional Source As Object = Nothing, Optional ActiveConnection As Object = Nothing, Optional CursorType As CursorTypeEnum = CursorTypeEnum.adOpenUnspecified, Optional LockType As LockTypeEnum = LockTypeEnum.adLockUnspecified, Optional Options As Integer = -1) Implements Recordset21.Open
        Throw New NotImplementedException()
    End Sub

    Private Sub Recordset21_Requery(Optional Options As Integer = -1) Implements Recordset21.Requery
        Throw New NotImplementedException()
    End Sub

    Private Sub Recordset21__xResync(Optional AffectRecords As AffectEnum = AffectEnum.adAffectAll) Implements Recordset21._xResync
        Throw New NotImplementedException()
    End Sub

    Private Sub Recordset21_Update(Optional Fields As Object = Nothing, Optional Values As Object = Nothing) Implements Recordset21.Update
        Throw New NotImplementedException()
    End Sub

    Private Function Recordset21__xClone() As Recordset Implements Recordset21._xClone
        Throw New NotImplementedException()
    End Function

    Private Sub Recordset21_UpdateBatch(Optional AffectRecords As AffectEnum = AffectEnum.adAffectAll) Implements Recordset21.UpdateBatch
        Throw New NotImplementedException()
    End Sub

    Private Sub Recordset21_CancelBatch(Optional AffectRecords As AffectEnum = AffectEnum.adAffectAll) Implements Recordset21.CancelBatch
        Throw New NotImplementedException()
    End Sub

    Private Function Recordset21_NextRecordset(ByRef Optional RecordsAffected As Object = Nothing) As Recordset Implements Recordset21.NextRecordset
        Throw New NotImplementedException()
    End Function

    Private Function Recordset21_Supports(CursorOptions As CursorOptionEnum) As Boolean Implements Recordset21.Supports
        Throw New NotImplementedException()
    End Function

    Private Sub Recordset21_Find(Criteria As String, Optional SkipRecords As Integer = 0, Optional SearchDirection As SearchDirectionEnum = SearchDirectionEnum.adSearchForward, Optional Start As Object = Nothing) Implements Recordset21.Find
        Throw New NotImplementedException()
    End Sub

    Private Sub Recordset21_Cancel() Implements Recordset21.Cancel
        Throw New NotImplementedException()
    End Sub

    Private Sub Recordset21__xSave(Optional FileName As String = "", Optional PersistFormat As PersistFormatEnum = PersistFormatEnum.adPersistADTG) Implements Recordset21._xSave
        Throw New NotImplementedException()
    End Sub

    Private Function Recordset21_GetString(Optional StringFormat As StringFormatEnum = StringFormatEnum.adClipString, Optional NumRows As Integer = -1, Optional ColumnDelimeter As String = "", Optional RowDelimeter As String = "", Optional NullExpr As String = "") As String Implements Recordset21.GetString
        Throw New NotImplementedException()
    End Function

    Private Function Recordset21_CompareBookmarks(Bookmark1 As Object, Bookmark2 As Object) As CompareEnum Implements Recordset21.CompareBookmarks
        Throw New NotImplementedException()
    End Function

    Private Function Recordset21_Clone(Optional LockType As LockTypeEnum = LockTypeEnum.adLockUnspecified) As Recordset Implements Recordset21.Clone
        Throw New NotImplementedException()
    End Function

    Private Sub Recordset21_Resync(Optional AffectRecords As AffectEnum = AffectEnum.adAffectAll, Optional ResyncValues As ResyncEnum = ResyncEnum.adResyncAllValues) Implements Recordset21.Resync
        Throw New NotImplementedException()
    End Sub

    Private Sub Recordset21_Seek(KeyValues As Object, Optional SeekOption As SeekEnum = SeekEnum.adSeekFirstEQ) Implements Recordset21.Seek
        Throw New NotImplementedException()
    End Sub

    Private ReadOnly Property Recordset21_Properties As ADODB.Properties Implements Recordset21.Properties
        Get
            Throw New NotImplementedException()
        End Get
    End Property

    Private Property Recordset21_AbsolutePosition As PositionEnum Implements Recordset21.AbsolutePosition
        Get
            Throw New NotImplementedException()
        End Get
        Set(value As PositionEnum)
            Throw New NotImplementedException()
        End Set
    End Property

    Private Property Recordset21_ActiveConnection As Object Implements Recordset21.ActiveConnection
        Get
            Throw New NotImplementedException()
        End Get
        Set(value As Object)
            Throw New NotImplementedException()
        End Set
    End Property

    Private ReadOnly Property Recordset21_BOF As Boolean Implements Recordset21.BOF
        Get
            Throw New NotImplementedException()
        End Get
    End Property

    Private Property Recordset21_Bookmark As Object Implements Recordset21.Bookmark
        Get
            Throw New NotImplementedException()
        End Get
        Set(value As Object)
            Throw New NotImplementedException()
        End Set
    End Property

    Private Property Recordset21_CacheSize As Integer Implements Recordset21.CacheSize
        Get
            Throw New NotImplementedException()
        End Get
        Set(value As Integer)
            Throw New NotImplementedException()
        End Set
    End Property

    Private Property Recordset21_CursorType As CursorTypeEnum Implements Recordset21.CursorType
        Get
            Throw New NotImplementedException()
        End Get
        Set(value As CursorTypeEnum)
            Throw New NotImplementedException()
        End Set
    End Property

    Private ReadOnly Property Recordset21_EOF As Boolean Implements Recordset21.EOF
        Get
            Throw New NotImplementedException()
        End Get
    End Property

    Private ReadOnly Property Recordset21_Fields As Fields Implements Recordset21.Fields
        Get
            Throw New NotImplementedException()
        End Get
    End Property

    Private Property Recordset21_LockType As LockTypeEnum Implements Recordset21.LockType
        Get
            Throw New NotImplementedException()
        End Get
        Set(value As LockTypeEnum)
            Throw New NotImplementedException()
        End Set
    End Property

    Private Property Recordset21_MaxRecords As Integer Implements Recordset21.MaxRecords
        Get
            Throw New NotImplementedException()
        End Get
        Set(value As Integer)
            Throw New NotImplementedException()
        End Set
    End Property

    Private ReadOnly Property Recordset21_RecordCount As Integer Implements Recordset21.RecordCount
        Get
            Throw New NotImplementedException()
        End Get
    End Property

    Private Property Recordset21_Source As Object Implements Recordset21.Source
        Get
            Throw New NotImplementedException()
        End Get
        Set(value As Object)
            Throw New NotImplementedException()
        End Set
    End Property

    Private Property Recordset21_AbsolutePage As PositionEnum Implements Recordset21.AbsolutePage
        Get
            Throw New NotImplementedException()
        End Get
        Set(value As PositionEnum)
            Throw New NotImplementedException()
        End Set
    End Property

    Private ReadOnly Property Recordset21_EditMode As EditModeEnum Implements Recordset21.EditMode
        Get
            Throw New NotImplementedException()
        End Get
    End Property

    Private Property Recordset21_Filter As Object Implements Recordset21.Filter
        Get
            Throw New NotImplementedException()
        End Get
        Set(value As Object)
            Throw New NotImplementedException()
        End Set
    End Property

    Private ReadOnly Property Recordset21_PageCount As Integer Implements Recordset21.PageCount
        Get
            Throw New NotImplementedException()
        End Get
    End Property

    Private Property Recordset21_PageSize As Integer Implements Recordset21.PageSize
        Get
            Throw New NotImplementedException()
        End Get
        Set(value As Integer)
            Throw New NotImplementedException()
        End Set
    End Property

    Private Property Recordset21_Sort As String Implements Recordset21.Sort
        Get
            Throw New NotImplementedException()
        End Get
        Set(value As String)
            Throw New NotImplementedException()
        End Set
    End Property

    Private ReadOnly Property Recordset21_Status As Integer Implements Recordset21.Status
        Get
            Throw New NotImplementedException()
        End Get
    End Property

    Private ReadOnly Property Recordset21_State As Integer Implements Recordset21.State
        Get
            Throw New NotImplementedException()
        End Get
    End Property

    Private Property Recordset21_CursorLocation As CursorLocationEnum Implements Recordset21.CursorLocation
        Get
            Throw New NotImplementedException()
        End Get
        Set(value As CursorLocationEnum)
            Throw New NotImplementedException()
        End Set
    End Property

    Private Property Recordset21_Collect(Index As Object) As Object Implements Recordset21.Collect
        Get
            Throw New NotImplementedException()
        End Get
        Set(value As Object)
            Throw New NotImplementedException()
        End Set
    End Property

    Private Property Recordset21_MarshalOptions As MarshalOptionsEnum Implements Recordset21.MarshalOptions
        Get
            Throw New NotImplementedException()
        End Get
        Set(value As MarshalOptionsEnum)
            Throw New NotImplementedException()
        End Set
    End Property

    Private Property Recordset21_DataSource As Object Implements Recordset21.DataSource
        Get
            Throw New NotImplementedException()
        End Get
        Set(value As Object)
            Throw New NotImplementedException()
        End Set
    End Property

    Private ReadOnly Property Recordset21_ActiveCommand As Object Implements Recordset21.ActiveCommand
        Get
            Throw New NotImplementedException()
        End Get
    End Property

    Private Property Recordset21_StayInSync As Boolean Implements Recordset21.StayInSync
        Get
            Throw New NotImplementedException()
        End Get
        Set(value As Boolean)
            Throw New NotImplementedException()
        End Set
    End Property

    Private Property Recordset21_DataMember As String Implements Recordset21.DataMember
        Get
            Throw New NotImplementedException()
        End Get
        Set(value As String)
            Throw New NotImplementedException()
        End Set
    End Property

    Private Property Recordset21_Index As String Implements Recordset21.Index
        Get
            Throw New NotImplementedException()
        End Get
        Set(value As String)
            Throw New NotImplementedException()
        End Set
    End Property

    Private Sub Recordset20_let_ActiveConnection(pvar As Object) Implements Recordset20.let_ActiveConnection
        Throw New NotImplementedException()
    End Sub

    Private Sub Recordset20_let_Source(pvSource As String) Implements Recordset20.let_Source
        Throw New NotImplementedException()
    End Sub

    Private Sub Recordset20_AddNew(Optional FieldList As Object = Nothing, Optional Values As Object = Nothing) Implements Recordset20.AddNew
        Throw New NotImplementedException()
    End Sub

    Private Sub Recordset20_CancelUpdate() Implements Recordset20.CancelUpdate
        Throw New NotImplementedException()
    End Sub

    Private Sub Recordset20_Close() Implements Recordset20.Close
        Throw New NotImplementedException()
    End Sub

    Private Sub Recordset20_Delete(Optional AffectRecords As AffectEnum = AffectEnum.adAffectCurrent) Implements Recordset20.Delete
        Throw New NotImplementedException()
    End Sub

    Private Function Recordset20_GetRows(Optional Rows As Integer = -1, Optional Start As Object = Nothing, Optional Fields As Object = Nothing) As Object Implements Recordset20.GetRows
        Throw New NotImplementedException()
    End Function

    Private Sub Recordset20_Move(NumRecords As Integer, Optional Start As Object = Nothing) Implements Recordset20.Move
        Throw New NotImplementedException()
    End Sub

    Private Sub Recordset20_MoveNext() Implements Recordset20.MoveNext
        Throw New NotImplementedException()
    End Sub

    Private Sub Recordset20_MovePrevious() Implements Recordset20.MovePrevious
        Throw New NotImplementedException()
    End Sub

    Private Sub Recordset20_MoveFirst() Implements Recordset20.MoveFirst
        Throw New NotImplementedException()
    End Sub

    Private Sub Recordset20_MoveLast() Implements Recordset20.MoveLast
        Throw New NotImplementedException()
    End Sub

    Private Sub Recordset20_Open(Optional Source As Object = Nothing, Optional ActiveConnection As Object = Nothing, Optional CursorType As CursorTypeEnum = CursorTypeEnum.adOpenUnspecified, Optional LockType As LockTypeEnum = LockTypeEnum.adLockUnspecified, Optional Options As Integer = -1) Implements Recordset20.Open
        Throw New NotImplementedException()
    End Sub

    Private Sub Recordset20_Requery(Optional Options As Integer = -1) Implements Recordset20.Requery
        Throw New NotImplementedException()
    End Sub

    Private Sub Recordset20__xResync(Optional AffectRecords As AffectEnum = AffectEnum.adAffectAll) Implements Recordset20._xResync
        Throw New NotImplementedException()
    End Sub

    Private Sub Recordset20_Update(Optional Fields As Object = Nothing, Optional Values As Object = Nothing) Implements Recordset20.Update
        Throw New NotImplementedException()
    End Sub

    Private Function Recordset20__xClone() As Recordset Implements Recordset20._xClone
        Throw New NotImplementedException()
    End Function

    Private Sub Recordset20_UpdateBatch(Optional AffectRecords As AffectEnum = AffectEnum.adAffectAll) Implements Recordset20.UpdateBatch
        Throw New NotImplementedException()
    End Sub

    Private Sub Recordset20_CancelBatch(Optional AffectRecords As AffectEnum = AffectEnum.adAffectAll) Implements Recordset20.CancelBatch
        Throw New NotImplementedException()
    End Sub

    Private Function Recordset20_NextRecordset(ByRef Optional RecordsAffected As Object = Nothing) As Recordset Implements Recordset20.NextRecordset
        Throw New NotImplementedException()
    End Function

    Private Function Recordset20_Supports(CursorOptions As CursorOptionEnum) As Boolean Implements Recordset20.Supports
        Throw New NotImplementedException()
    End Function

    Private Sub Recordset20_Find(Criteria As String, Optional SkipRecords As Integer = 0, Optional SearchDirection As SearchDirectionEnum = SearchDirectionEnum.adSearchForward, Optional Start As Object = Nothing) Implements Recordset20.Find
        Throw New NotImplementedException()
    End Sub

    Private Sub Recordset20_Cancel() Implements Recordset20.Cancel
        Throw New NotImplementedException()
    End Sub

    Private Sub Recordset20__xSave(Optional FileName As String = "", Optional PersistFormat As PersistFormatEnum = PersistFormatEnum.adPersistADTG) Implements Recordset20._xSave
        Throw New NotImplementedException()
    End Sub

    Private Function Recordset20_GetString(Optional StringFormat As StringFormatEnum = StringFormatEnum.adClipString, Optional NumRows As Integer = -1, Optional ColumnDelimeter As String = "", Optional RowDelimeter As String = "", Optional NullExpr As String = "") As String Implements Recordset20.GetString
        Throw New NotImplementedException()
    End Function

    Private Function Recordset20_CompareBookmarks(Bookmark1 As Object, Bookmark2 As Object) As CompareEnum Implements Recordset20.CompareBookmarks
        Throw New NotImplementedException()
    End Function

    Private Function Recordset20_Clone(Optional LockType As LockTypeEnum = LockTypeEnum.adLockUnspecified) As Recordset Implements Recordset20.Clone
        Throw New NotImplementedException()
    End Function

    Private Sub Recordset20_Resync(Optional AffectRecords As AffectEnum = AffectEnum.adAffectAll, Optional ResyncValues As ResyncEnum = ResyncEnum.adResyncAllValues) Implements Recordset20.Resync
        Throw New NotImplementedException()
    End Sub

    Private ReadOnly Property Recordset20_Properties As ADODB.Properties Implements Recordset20.Properties
        Get
            Throw New NotImplementedException()
        End Get
    End Property

    Private Property Recordset20_AbsolutePosition As PositionEnum Implements Recordset20.AbsolutePosition
        Get
            Throw New NotImplementedException()
        End Get
        Set(value As PositionEnum)
            Throw New NotImplementedException()
        End Set
    End Property

    Private Property Recordset20_ActiveConnection As Object Implements Recordset20.ActiveConnection
        Get
            Throw New NotImplementedException()
        End Get
        Set(value As Object)
            Throw New NotImplementedException()
        End Set
    End Property

    Private ReadOnly Property Recordset20_BOF As Boolean Implements Recordset20.BOF
        Get
            Throw New NotImplementedException()
        End Get
    End Property

    Private Property Recordset20_Bookmark As Object Implements Recordset20.Bookmark
        Get
            Throw New NotImplementedException()
        End Get
        Set(value As Object)
            Throw New NotImplementedException()
        End Set
    End Property

    Private Property Recordset20_CacheSize As Integer Implements Recordset20.CacheSize
        Get
            Throw New NotImplementedException()
        End Get
        Set(value As Integer)
            Throw New NotImplementedException()
        End Set
    End Property

    Private Property Recordset20_CursorType As CursorTypeEnum Implements Recordset20.CursorType
        Get
            Throw New NotImplementedException()
        End Get
        Set(value As CursorTypeEnum)
            Throw New NotImplementedException()
        End Set
    End Property

    Private ReadOnly Property Recordset20_EOF As Boolean Implements Recordset20.EOF
        Get
            Throw New NotImplementedException()
        End Get
    End Property

    Private ReadOnly Property Recordset20_Fields As Fields Implements Recordset20.Fields
        Get
            Throw New NotImplementedException()
        End Get
    End Property

    Private Property Recordset20_LockType As LockTypeEnum Implements Recordset20.LockType
        Get
            Throw New NotImplementedException()
        End Get
        Set(value As LockTypeEnum)
            Throw New NotImplementedException()
        End Set
    End Property

    Private Property Recordset20_MaxRecords As Integer Implements Recordset20.MaxRecords
        Get
            Throw New NotImplementedException()
        End Get
        Set(value As Integer)
            Throw New NotImplementedException()
        End Set
    End Property

    Private ReadOnly Property Recordset20_RecordCount As Integer Implements Recordset20.RecordCount
        Get
            Throw New NotImplementedException()
        End Get
    End Property

    Private Property Recordset20_Source As Object Implements Recordset20.Source
        Get
            Throw New NotImplementedException()
        End Get
        Set(value As Object)
            Throw New NotImplementedException()
        End Set
    End Property

    Private Property Recordset20_AbsolutePage As PositionEnum Implements Recordset20.AbsolutePage
        Get
            Throw New NotImplementedException()
        End Get
        Set(value As PositionEnum)
            Throw New NotImplementedException()
        End Set
    End Property

    Private ReadOnly Property Recordset20_EditMode As EditModeEnum Implements Recordset20.EditMode
        Get
            Throw New NotImplementedException()
        End Get
    End Property

    Private Property Recordset20_Filter As Object Implements Recordset20.Filter
        Get
            Throw New NotImplementedException()
        End Get
        Set(value As Object)
            Throw New NotImplementedException()
        End Set
    End Property

    Private ReadOnly Property Recordset20_PageCount As Integer Implements Recordset20.PageCount
        Get
            Throw New NotImplementedException()
        End Get
    End Property

    Private Property Recordset20_PageSize As Integer Implements Recordset20.PageSize
        Get
            Throw New NotImplementedException()
        End Get
        Set(value As Integer)
            Throw New NotImplementedException()
        End Set
    End Property

    Private Property Recordset20_Sort As String Implements Recordset20.Sort
        Get
            Throw New NotImplementedException()
        End Get
        Set(value As String)
            Throw New NotImplementedException()
        End Set
    End Property

    Private ReadOnly Property Recordset20_Status As Integer Implements Recordset20.Status
        Get
            Throw New NotImplementedException()
        End Get
    End Property

    Private ReadOnly Property Recordset20_State As Integer Implements Recordset20.State
        Get
            Throw New NotImplementedException()
        End Get
    End Property

    Private Property Recordset20_CursorLocation As CursorLocationEnum Implements Recordset20.CursorLocation
        Get
            Throw New NotImplementedException()
        End Get
        Set(value As CursorLocationEnum)
            Throw New NotImplementedException()
        End Set
    End Property

    Private Property Recordset20_Collect(Index As Object) As Object Implements Recordset20.Collect
        Get
            Throw New NotImplementedException()
        End Get
        Set(value As Object)
            Throw New NotImplementedException()
        End Set
    End Property

    Private Property Recordset20_MarshalOptions As MarshalOptionsEnum Implements Recordset20.MarshalOptions
        Get
            Throw New NotImplementedException()
        End Get
        Set(value As MarshalOptionsEnum)
            Throw New NotImplementedException()
        End Set
    End Property

    Private Property Recordset20_DataSource As Object Implements Recordset20.DataSource
        Get
            Throw New NotImplementedException()
        End Get
        Set(value As Object)
            Throw New NotImplementedException()
        End Set
    End Property

    Private ReadOnly Property Recordset20_ActiveCommand As Object Implements Recordset20.ActiveCommand
        Get
            Throw New NotImplementedException()
        End Get
    End Property

    Private Property Recordset20_StayInSync As Boolean Implements Recordset20.StayInSync
        Get
            Throw New NotImplementedException()
        End Get
        Set(value As Boolean)
            Throw New NotImplementedException()
        End Set
    End Property

    Private Property Recordset20_DataMember As String Implements Recordset20.DataMember
        Get
            Throw New NotImplementedException()
        End Get
        Set(value As String)
            Throw New NotImplementedException()
        End Set
    End Property

    Private Sub Recordset15_let_ActiveConnection(pvar As Object) Implements Recordset15.let_ActiveConnection
        Throw New NotImplementedException()
    End Sub

    Private Sub Recordset15_let_Source(pvSource As String) Implements Recordset15.let_Source
        Throw New NotImplementedException()
    End Sub

    Private Sub Recordset15_AddNew(Optional FieldList As Object = Nothing, Optional Values As Object = Nothing) Implements Recordset15.AddNew
        Throw New NotImplementedException()
    End Sub

    Private Sub Recordset15_CancelUpdate() Implements Recordset15.CancelUpdate
        Throw New NotImplementedException()
    End Sub

    Private Sub Recordset15_Close() Implements Recordset15.Close
        Throw New NotImplementedException()
    End Sub

    Private Sub Recordset15_Delete(Optional AffectRecords As AffectEnum = AffectEnum.adAffectCurrent) Implements Recordset15.Delete
        Throw New NotImplementedException()
    End Sub

    Private Function Recordset15_GetRows(Optional Rows As Integer = -1, Optional Start As Object = Nothing, Optional Fields As Object = Nothing) As Object Implements Recordset15.GetRows
        Throw New NotImplementedException()
    End Function

    Private Sub Recordset15_Move(NumRecords As Integer, Optional Start As Object = Nothing) Implements Recordset15.Move
        Throw New NotImplementedException()
    End Sub

    Private Sub Recordset15_MoveNext() Implements Recordset15.MoveNext
        Throw New NotImplementedException()
    End Sub

    Private Sub Recordset15_MovePrevious() Implements Recordset15.MovePrevious
        Throw New NotImplementedException()
    End Sub

    Private Sub Recordset15_MoveFirst() Implements Recordset15.MoveFirst
        Throw New NotImplementedException()
    End Sub

    Private Sub Recordset15_MoveLast() Implements Recordset15.MoveLast
        Throw New NotImplementedException()
    End Sub

    Private Sub Recordset15_Open(Optional Source As Object = Nothing, Optional ActiveConnection As Object = Nothing, Optional CursorType As CursorTypeEnum = CursorTypeEnum.adOpenUnspecified, Optional LockType As LockTypeEnum = LockTypeEnum.adLockUnspecified, Optional Options As Integer = -1) Implements Recordset15.Open
        Throw New NotImplementedException()
    End Sub

    Private Sub Recordset15_Requery(Optional Options As Integer = -1) Implements Recordset15.Requery
        Throw New NotImplementedException()
    End Sub

    Private Sub Recordset15__xResync(Optional AffectRecords As AffectEnum = AffectEnum.adAffectAll) Implements Recordset15._xResync
        Throw New NotImplementedException()
    End Sub

    Private Sub Recordset15_Update(Optional Fields As Object = Nothing, Optional Values As Object = Nothing) Implements Recordset15.Update
        Throw New NotImplementedException()
    End Sub

    Private Function Recordset15__xClone() As Recordset Implements Recordset15._xClone
        Throw New NotImplementedException()
    End Function

    Private Sub Recordset15_UpdateBatch(Optional AffectRecords As AffectEnum = AffectEnum.adAffectAll) Implements Recordset15.UpdateBatch
        Throw New NotImplementedException()
    End Sub

    Private Sub Recordset15_CancelBatch(Optional AffectRecords As AffectEnum = AffectEnum.adAffectAll) Implements Recordset15.CancelBatch
        Throw New NotImplementedException()
    End Sub

    Private Function Recordset15_NextRecordset(ByRef Optional RecordsAffected As Object = Nothing) As Recordset Implements Recordset15.NextRecordset
        Throw New NotImplementedException()
    End Function

    Private Function Recordset15_Supports(CursorOptions As CursorOptionEnum) As Boolean Implements Recordset15.Supports
        Throw New NotImplementedException()
    End Function

    Private Sub Recordset15_Find(Criteria As String, Optional SkipRecords As Integer = 0, Optional SearchDirection As SearchDirectionEnum = SearchDirectionEnum.adSearchForward, Optional Start As Object = Nothing) Implements Recordset15.Find
        Throw New NotImplementedException()
    End Sub

    Private ReadOnly Property Recordset15_Properties As ADODB.Properties Implements Recordset15.Properties
        Get
            Throw New NotImplementedException()
        End Get
    End Property

    Private Property Recordset15_AbsolutePosition As PositionEnum Implements Recordset15.AbsolutePosition
        Get
            Throw New NotImplementedException()
        End Get
        Set(value As PositionEnum)
            Throw New NotImplementedException()
        End Set
    End Property

    Private Property Recordset15_ActiveConnection As Object Implements Recordset15.ActiveConnection
        Get
            Throw New NotImplementedException()
        End Get
        Set(value As Object)
            Throw New NotImplementedException()
        End Set
    End Property

    Private ReadOnly Property Recordset15_BOF As Boolean Implements Recordset15.BOF
        Get
            Throw New NotImplementedException()
        End Get
    End Property

    Private Property Recordset15_Bookmark As Object Implements Recordset15.Bookmark
        Get
            Throw New NotImplementedException()
        End Get
        Set(value As Object)
            Throw New NotImplementedException()
        End Set
    End Property

    Private Property Recordset15_CacheSize As Integer Implements Recordset15.CacheSize
        Get
            Throw New NotImplementedException()
        End Get
        Set(value As Integer)
            Throw New NotImplementedException()
        End Set
    End Property

    Private Property Recordset15_CursorType As CursorTypeEnum Implements Recordset15.CursorType
        Get
            Throw New NotImplementedException()
        End Get
        Set(value As CursorTypeEnum)
            Throw New NotImplementedException()
        End Set
    End Property

    Private ReadOnly Property Recordset15_EOF As Boolean Implements Recordset15.EOF
        Get
            Throw New NotImplementedException()
        End Get
    End Property

    Private ReadOnly Property Recordset15_Fields As Fields Implements Recordset15.Fields
        Get
            Throw New NotImplementedException()
        End Get
    End Property

    Private Property Recordset15_LockType As LockTypeEnum Implements Recordset15.LockType
        Get
            Throw New NotImplementedException()
        End Get
        Set(value As LockTypeEnum)
            Throw New NotImplementedException()
        End Set
    End Property

    Private Property Recordset15_MaxRecords As Integer Implements Recordset15.MaxRecords
        Get
            Throw New NotImplementedException()
        End Get
        Set(value As Integer)
            Throw New NotImplementedException()
        End Set
    End Property

    Private ReadOnly Property Recordset15_RecordCount As Integer Implements Recordset15.RecordCount
        Get
            Throw New NotImplementedException()
        End Get
    End Property

    Private Property Recordset15_Source As Object Implements Recordset15.Source
        Get
            Throw New NotImplementedException()
        End Get
        Set(value As Object)
            Throw New NotImplementedException()
        End Set
    End Property

    Private Property Recordset15_AbsolutePage As PositionEnum Implements Recordset15.AbsolutePage
        Get
            Throw New NotImplementedException()
        End Get
        Set(value As PositionEnum)
            Throw New NotImplementedException()
        End Set
    End Property

    Private ReadOnly Property Recordset15_EditMode As EditModeEnum Implements Recordset15.EditMode
        Get
            Throw New NotImplementedException()
        End Get
    End Property

    Private Property Recordset15_Filter As Object Implements Recordset15.Filter
        Get
            Throw New NotImplementedException()
        End Get
        Set(value As Object)
            Throw New NotImplementedException()
        End Set
    End Property

    Private ReadOnly Property Recordset15_PageCount As Integer Implements Recordset15.PageCount
        Get
            Throw New NotImplementedException()
        End Get
    End Property

    Private Property Recordset15_PageSize As Integer Implements Recordset15.PageSize
        Get
            Throw New NotImplementedException()
        End Get
        Set(value As Integer)
            Throw New NotImplementedException()
        End Set
    End Property

    Private Property Recordset15_Sort As String Implements Recordset15.Sort
        Get
            Throw New NotImplementedException()
        End Get
        Set(value As String)
            Throw New NotImplementedException()
        End Set
    End Property

    Private ReadOnly Property Recordset15_Status As Integer Implements Recordset15.Status
        Get
            Throw New NotImplementedException()
        End Get
    End Property

    Private ReadOnly Property Recordset15_State As Integer Implements Recordset15.State
        Get
            Throw New NotImplementedException()
        End Get
    End Property

    Private Property Recordset15_CursorLocation As CursorLocationEnum Implements Recordset15.CursorLocation
        Get
            Throw New NotImplementedException()
        End Get
        Set(value As CursorLocationEnum)
            Throw New NotImplementedException()
        End Set
    End Property

    Private Property Recordset15_Collect(Index As Object) As Object Implements Recordset15.Collect
        Get
            Throw New NotImplementedException()
        End Get
        Set(value As Object)
            Throw New NotImplementedException()
        End Set
    End Property

    Private Property Recordset15_MarshalOptions As MarshalOptionsEnum Implements Recordset15.MarshalOptions
        Get
            Throw New NotImplementedException()
        End Get
        Set(value As MarshalOptionsEnum)
            Throw New NotImplementedException()
        End Set
    End Property

    Private ReadOnly Property _ADO_Properties As ADODB.Properties Implements _ADO.Properties
        Get
            Throw New NotImplementedException()
        End Get
    End Property

End Class