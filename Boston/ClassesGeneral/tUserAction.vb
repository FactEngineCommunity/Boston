''' <summary>
''' Used for the Undo/Redo Logs
''' </summary>
''' <remarks></remarks>
Public Class tUserAction

    ''' <summary>
    ''' The unique Id by which one or more UserActions may be grouped.
    ''' </summary>
    ''' <remarks></remarks>
    Public TransactionId As String

    ''' <summary>
    ''' The ModelObject that the User Action was applied to.
    ''' </summary>
    ''' <remarks></remarks>
    Public ModelObject As New Object

    ''' <summary>
    ''' The User Action that was applied to ModelObject.
    ''' </summary>
    ''' <remarks></remarks>
    Public Action As pcenumUserAction

    ''' <summary>
    ''' The ModelObject (and properties) before the User Action.
    ''' </summary>
    ''' <remarks></remarks>
    Public PreActionModelObject As New Object

    ''' <summary>
    ''' The ModelObject (and properties) after the User Action.
    ''' </summary>
    ''' <remarks></remarks>
    Public PostActionModelObject As New Object

    ''' <summary>
    ''' The Page on which the User Action took place.
    ''' </summary>
    ''' <remarks></remarks>
    Public Page As FBM.Page

    ''' <summary>
    ''' Parameterless New
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub New()

    End Sub

    Public Sub New(ByRef arModelObject As FBM.ModelObject, ByVal aiUserAction As pcenumUserAction, ByRef arPage As FBM.Page, Optional ByVal asTransactionId As String = Nothing)

        If IsSomething(asTransactionId) Then
            Me.TransactionId = asTransactionId
        Else
            Me.TransactionId = System.Guid.NewGuid.ToString
        End If

        Me.ModelObject = arModelObject
        Me.Action = aiUserAction
        Me.Page = arPage

    End Sub

End Class
