Imports Newtonsoft.Json
Imports System.ComponentModel
Imports System.Linq.Expressions
Imports System.Reflection

Public Module BostonApplicationConstants

    <Serializable()>
    Public Enum pcenumBostonApplicationType
        <Description("Windows Forms Application")> WindowsFormsApplication
        <Description("Natural Language BI Form")> NaturalLanguageBusinessIntelligenceForm
    End Enum

End Module

Namespace BostonApplication

    Public Class Application

        <JsonIgnore>
        <DebuggerBrowsable(DebuggerBrowsableState.Never)>
        Private _BostonApplicationId As String = System.Guid.NewGuid.ToString

        <JsonProperty>
        Public Property BostonApplicationId As String
            Get
                Return Me._BostonApplicationId
            End Get
            Set(value As String)
                Me._BostonApplicationId = value
            End Set
        End Property


        <JsonIgnore>
        <DebuggerBrowsable(DebuggerBrowsableState.Never)>
        Private _Name As String

        ''' <summary>
        ''' The name of the Application
        ''' </summary>
        <JsonProperty>
        Public Property Name As String
            Get
                Return Me._Name
            End Get
            Set(value As String)
                Me._Name = value
            End Set
        End Property

        <JsonIgnore>
        <DebuggerBrowsable(DebuggerBrowsableState.Never)>
        Private _ApplicationType As pcenumBostonApplicationType

        <JsonProperty>
        Public Property ApplicationType As pcenumBostonApplicationType
            Get
                Return Me._ApplicationType
            End Get
            Set(value As pcenumBostonApplicationType)
                Me._ApplicationType = value
            End Set
        End Property

        <JsonIgnore>
        Public Model As FBM.Model = Nothing

        <JsonIgnore>
        <DebuggerBrowsable(DebuggerBrowsableState.Never)>
        Private _DefaultModelId As String

        <JsonProperty>
        Public Property DefaultModelId As String
            Get
                Return Me._DefaultModelId
            End Get
            Set(value As String)
                Me._DefaultModelId = value
            End Set
        End Property

        <JsonIgnore>
        <DebuggerBrowsable(DebuggerBrowsableState.Never)>
        Private _ImageFilePath As String = Nothing

        <JsonProperty>
        Public Property ImageFilePath As String
            Get
                Return Me._ImageFilePath
            End Get
            Set(value As String)
                Me._ImageFilePath = value
            End Set
        End Property


        <JsonIgnore>
        Public MenuItem As New List(Of BostonApplication.MenuItem)

        ''' <summary>
        ''' Parameterless Constructor
        ''' </summary>
        Public Sub New()
        End Sub


        ''' <summary>
        ''' Gets the MenuItems for the Application from the DataStore
        ''' </summary>
        Public Sub GetMenuItems()

            Try
                Dim lrDataStore As New DataStore.Store
                Dim whereClause As Expression(Of Func(Of BostonApplication.MenuItem, Boolean)) = Function(t) t.BostonApplicationId = Me.BostonApplicationId And t.ParentMenuItemId Is Nothing

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
        ''' Saves the BostonApplication to the DataStore
        ''' </summary>
        Public Sub Save()

            Try
                Dim lrDataStore As New DataStore.Store
                Dim whereClause As Expression(Of Func(Of BostonApplication.Application, Boolean)) = Function(t) t.BostonApplicationId = Me.BostonApplicationId
                lrDataStore.Upsert(Me, whereClause)

                Call Me.SaveMenuItems()

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex, False)
            End Try

        End Sub

        ''' <summary>
        ''' Saves te MenuItems for the BostonApplication.
        ''' </summary>
        Public Sub SaveMenuItems()

            Try
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
