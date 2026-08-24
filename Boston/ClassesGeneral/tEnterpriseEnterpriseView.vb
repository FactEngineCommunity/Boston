Imports System.Reflection

''' <summary>
''' This class is used on every TreeNode in the EnterpriseView navigation tree.
''' -----------------------------------------------------------------------------------
''' </summary>
''' <remarks></remarks>
Public Class tEnterpriseEnterpriseView
    Inherits EventArgs
    Implements IEquatable(Of tEnterpriseEnterpriseView)

    Public MenuType As pcenumMenuType
    Public WithEvents Tag As Object 'If Tree/Menu node represents an EnterpriseObject, then is the actual object or the key to retrieve the object.

    Public ModelId As String = Nothing
    Public PageId As String = Nothing
    Public TreeNode As New TreeNode 'Pointer back to the TreeNode that contains this EnterpriseView navigation menu item.

    Public LanguageId As pcenumLanguage 'Reserved for LeafNodes that are Models and especially where the Model is 
    'not represented on the TreeView as an ORMModel. For example, even though all UseCaseDiagrams in Richmond are
    'stored as ORM Models, the LeafNode on the TreeView representeding the UseCaseModel (while having a .Tag directly to the
    'respective ORM Model) will have a 'language_id' of UseCaseModel.
    '  This just makes it so much easier to make sure that Pages loaded under the Model node are of the same language as intended
    'by the Model node. i.e. The Page knows what Language it is, but the Model doesn't...so we must tell it. There is no other
    'choice...other than to have multiple models per ORM model...which defeats the purpose of Boston. 1 model...multiple model views.
    'That's what Richmond is about...multiple interpretations (ORM, Use Case Diagram, Event Trace Diagram etc) of the same model.
    '----------------------------------------------------------------------------------------------------------------------------

    Public FocusModelElement As FBM.ModelObject

    Sub New()
        '----------------------------------
        'Default Parameterless Constructor
        '----------------------------------
    End Sub

    Sub New(ByVal aiMenuObjectType As pcenumMenuType,
            Optional ByVal aoObject As Object = Nothing,
            Optional ByVal asModelId As String = Nothing,
            Optional ByVal aiLanguageId As pcenumLanguage = pcenumLanguage.ORMModel,
            Optional ByVal aoTreeNode As TreeNode = Nothing,
            Optional ByVal as_PageId As String = Nothing
            )
        '----------------------------------------------------------------------------------
        'Parameters
        '   aiMenuObjectType: Enumerated integer representing the
        '                        type of Object represented by the menu (TreeNode) object
        '   ao_object: If supplied, is the actual object represented by the node in the 
        '                        TreeView()
        '----------------------------------------------------------------------------------
        Me.MenuType = aiMenuObjectType

        If aoObject IsNot Nothing Then
            Me.Tag = aoObject

            Select Case aoObject.GetType
                Case Is = GetType(FBM.Model)
                    AddHandler DirectCast(Tag, FBM.Model).NameChanged, AddressOf FBMModelNameChangedHandler
            End Select

        End If

        If asModelId IsNot Nothing Then
            Me.ModelId = asModelId
        End If

        Me.LanguageId = aiLanguageId

        If aoTreeNode IsNot Nothing Then
            Me.TreeNode = aoTreeNode
        End If

        If as_PageId IsNot Nothing Then
            Me.PageId = as_PageId
        End If

    End Sub

    Private Sub FBMModelNameChangedHandler(asNewName As String)

        Me.TreeNode.Text = asNewName

    End Sub

    Public Shadows Function Equals(ByVal other As tEnterpriseEnterpriseView) As Boolean Implements System.IEquatable(Of tEnterpriseEnterpriseView).Equals

        Try
            If (Me.LanguageId = other.LanguageId) And
               (Me.ModelId = other.ModelId) And
               (Me.PageId = other.PageId) Then
                Return True
            Else
                Return False
            End If

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Function
End Class
