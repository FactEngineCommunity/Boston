Imports Newtonsoft.Json
Imports System.ComponentModel
Imports System.Linq.Expressions
Imports System.Reflection

Namespace BostonApplication
    Public Class MenuItem

        <JsonIgnore>
        Public BostonAppliation As BostonApplication.Application = Nothing

        <JsonIgnore>
        <DebuggerBrowsable(DebuggerBrowsableState.Never)>
        Private _BostonApplicationId As String = Nothing

        <JsonIgnore>
        Private _SequenceNr As Integer = 1

        <JsonProperty>
        Public Property SequenceNr As Integer
            Get
                Return Me._SequenceNr
            End Get
            Set(value As Integer)
                Me._SequenceNr = value
            End Set
        End Property

        <JsonProperty>
        <ForeignKeyReference(GetType(Application), NameOf(Application.BostonApplicationId), True)>
        Public Property BostonApplicationId As String
            Get
                If Me.BostonAppliation Is Nothing Then
                    Return Me._BostonApplicationId
                Else
                    Return Me.BostonAppliation.BostonApplicationId
                End If
            End Get
            Set(value As String)
                Me._BostonApplicationId = value
            End Set
        End Property

        <JsonIgnore>
        <DebuggerBrowsable(DebuggerBrowsableState.Never)>
        Private _MenuItemId As String = System.Guid.NewGuid.ToString

        ''' <summary>
        ''' The Unique Identifier of the MenuItem
        ''' </summary>
        ''' <returns></returns>
        <JsonProperty>
        Public Property MenuItemId As String
            Get
                Return Me._MenuItemId
            End Get
            Set(value As String)
                Me._MenuItemId = value
            End Set
        End Property

        <JsonIgnore>
        <DebuggerBrowsable(DebuggerBrowsableState.Never)>
        Private _ParentMenuItem As BostonApplication.MenuItem = Nothing

        ''' <summary>
        ''' The MenuItem that owns this MenuItem, if any.
        ''' </summary>
        ''' <returns></returns>
        <JsonIgnore>
        Public Property ParentMenuItem As BostonApplication.MenuItem
            Get
                Return Me._ParentMenuItem
            End Get
            Set(value As BostonApplication.MenuItem)
                Me._ParentMenuItem = value
                If value IsNot Nothing Then
                    Me._ParentMenuItemId = value.MenuItemId
                End If
            End Set
        End Property


        <JsonIgnore>
        Private _ParentMenuItemId As String = Nothing

        ''' <summary>
        ''' The MenuItem that owns this MenuItem. Is 'Nothing' if has no parent and is a root level MenuItem.
        ''' </summary>
        ''' <returns></returns>
        <JsonProperty>
        <ForeignKeyReference(GetType(BostonApplication.MenuItem), NameOf(BostonApplication.MenuItem.MenuItemId), True)>
        Public Property ParentMenuItemId As String
            Get
                If Me.ParentMenuItem Is Nothing Then
                    Return Me._ParentMenuItemId
                Else
                    Return Me.ParentMenuItem.MenuItemId
                End If
            End Get
            Set(value As String)
                Me._ParentMenuItemId = value
            End Set
        End Property

        <JsonIgnore>
        <DebuggerBrowsable(DebuggerBrowsableState.Never)>
        Private _Text As String = ""

        ''' <summary>
        ''' The Text of the Menu. E.g. As for menus such as [Employee], [Session], etc
        ''' </summary>
        <JsonProperty>
        <Category("Menu Properties")>
        Public Property Text As String
            Get
                Return Me._Text
            End Get
            Set(value As String)
                Me._Text = value
                If Me.TreeNode IsNot Nothing Then
                    Me.TreeNode.Text = value
                End If
            End Set
        End Property

        ''' <summary>
        ''' As when the MenuItem is displayed on a TreeView, used to reflect position in the menu tree and Text property above.
        ''' </summary>
        <JsonIgnore>
        Public TreeNode As TreeNode

        ''' <summary>
        ''' The Menu Items of the Menu, if any.
        ''' </summary>
        <JsonIgnore>
        Public MenuItem As New List(Of BostonApplication.MenuItem)

        <JsonIgnore>
        <DebuggerBrowsable(DebuggerBrowsableState.Never)>
        Private _MenuFunction As MenuFunction = MenuFunction.None
        ''' <summary>
        ''' Determiner for the Menu's function. E.g. Exit, AddItem, EditItem, DeleteItem. See also, ObjectType.
        ''' </summary>
        <JsonProperty>
        <Category("Menu Properties"),
        Browsable(True),
        [ReadOnly](False),
        DesignOnly(False),
        BindableAttribute(True),
        DefaultValueAttribute(MenuFunction.None),
        Description("Determiner for the Menu's function. E.g. Exit, AddItem, EditItem, DeleteItem."),
        TypeConverter(GetType(Enumeration.EnumDescConverter))>
        Public Property MenuFunction As MenuFunction
            Get
                Return Me._MenuFunction
            End Get
            Set(value As MenuFunction)
                Me._MenuFunction = value
            End Set
        End Property


        <JsonIgnore>
        <DebuggerBrowsable(DebuggerBrowsableState.Never)>
        Private _ObjectType As String = ""
        ''' <summary>
        ''' E.g. "Mission", "Person", "ConfigurationItem"
        ''' </summary>
        <Category("Menu Properties")>
        <JsonProperty>
        Public Property ObjectType As String
            Get
                Return Me._ObjectType
            End Get
            Set(value As String)
                Me._ObjectType = value
            End Set
        End Property

        ''' <summary>
        ''' Parameterless Constructor
        ''' </summary>
        Public Sub New()
        End Sub

        Public Sub New(ByRef arBostonApplication As BostonApplication.Application, Optional ByRef arParentMenuItem As BostonApplication.MenuItem = Nothing)

            Try
                Me.BostonAppliation = arBostonApplication

                If arParentMenuItem IsNot Nothing Then
                    Me.ParentMenuItemId = arParentMenuItem.MenuItemId
                End If
            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex, False)
            End Try

        End Sub

        Public Sub GetMenuItems()

            Try
                Dim lrDataStore As New DataStore.Store
                Dim whereClause As Expression(Of Func(Of BostonApplication.MenuItem, Boolean)) = Function(t) t.BostonApplicationId = Me.BostonApplicationId And t.ParentMenuItemId = Me.MenuItemId

                Dim larMenuItem = lrDataStore.Get(Of BostonApplication.MenuItem)(whereClause)

                For Each lrMenuItem In larMenuItem.OrderBy(Function(x) x.SequenceNr)
                    Me.MenuItem.Add(lrMenuItem)
                    Call lrMenuItem.GetMenuItems()
                Next

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex, False)
            End Try

        End Sub

        ''' <summary>
        ''' Saves the MenuItem and all its (sub)MenuItems to the DataStore
        ''' </summary>
        Public Sub Save()

            Try
                Dim lrDataStore As New DataStore.Store
                Dim whereClause As Expression(Of Func(Of BostonApplication.MenuItem, Boolean)) = Function(t) t.MenuItemId = Me.MenuItemId
                lrDataStore.Upsert(Me, whereClause)

                For Each lrMenuItem In Me.MenuItem
                    Call lrMenuItem.Save()
                Next

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex, False)
            End Try

        End Sub

    End Class

End Namespace

<TypeConverter(GetType(Enumeration.EnumDescConverter))>
Public Enum MenuFunction

    <Description("None")> None
    <Description("Exit Application")> [ExitApplication]
    <Description("Add Item")> AddItem
    <Description("Edit Item")> EditItem
    <Description("Delete Item")> DeleteItem

End Enum

