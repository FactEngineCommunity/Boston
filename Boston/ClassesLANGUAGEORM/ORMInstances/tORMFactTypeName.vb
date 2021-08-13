Imports System.Xml.Serialization
Imports System.Reflection

Namespace FBM
    <Serializable()> _
    Public Class FactTypeName
        Inherits FBM.PageObject
        Implements FBM.iPageObject

        <XmlIgnore()> _
        Public FactType As New FBM.FactType

        <XmlIgnore()> _
        Public FactTypeInstance As FBM.FactTypeInstance

        Public Shadows Property X As Integer Implements FBM.iPageObject.X
        Public Shadows Property Y As Integer Implements FBM.iPageObject.Y

        Sub New()

            Me.ConceptType = pcenumConceptType.FactTypeName

        End Sub

        Sub New(ByVal arModel As FBM.Model, ByRef arPage As FBM.Page, ByRef arFactTypeInstance As FBM.FactTypeInstance, ByVal asName As String)

            Call Me.New()

            Me.Model = arModel
            Me.Page = arPage
            Me.Name = asName
            Me.FactTypeInstance = arFactTypeInstance
            Me.FactType = arFactTypeInstance.FactType

        End Sub

        Public Overloads Function Clone(ByRef arPage As FBM.Page, ByRef arFactTypeInstance As FBM.FactTypeInstance) As Object

            Dim lrFactTypeName As New FBM.FactTypeName

            With Me

                lrFactTypeName.Model = arPage.Model
                lrFactTypeName.Page = arPage
                lrFactTypeName.Name = .Name
                lrFactTypeName.FactTypeInstance = arFactTypeInstance
                lrFactTypeName.FactType = arFactTypeInstance.FactType
                lrFactTypeName.X = .X
                lrFactTypeName.Y = .Y

            End With

            Return lrFactTypeName

        End Function

        Public Overloads Sub RefreshShape(Optional ByVal aoChangedPropertyItem As PropertyValueChangedEventArgs = Nothing)

        End Sub

        Public Sub RemoveFromPage(ByVal abBroadcastInterfaceEvent As Boolean)

            Try
                Dim lrConceptInstance As New FBM.ConceptInstance(Me.Model, Me.Page, Me.FactTypeInstance.Name, pcenumConceptType.FactTypeName)

                If abBroadcastInterfaceEvent Then
                    Call TableConceptInstance.DeleteConceptInstance(lrConceptInstance)
                End If

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Public Overrides Sub Save(Optional ByVal abRapidSave As Boolean = False)

            '-----------------------------------------
            'Saves the EntityInstance to the database
            '-----------------------------------------
            Dim lrConceptInstance As New FBM.ConceptInstance

            Try
                lrConceptInstance.ModelId = Me.Model.ModelId
                lrConceptInstance.PageId = Me.Page.PageId
                lrConceptInstance.Symbol = Me.FactTypeInstance.Name
                lrConceptInstance.X = Me.X
                lrConceptInstance.Y = Me.Y
                lrConceptInstance.ConceptType = pcenumConceptType.FactTypeName
                lrConceptInstance.Visible = Me.FactTypeInstance.ShowFactTypeName

                '--------------------------------------------------
                'Make sure the new Symbol is in the Concept table
                '--------------------------------------------------
                Dim lrConcept As New FBM.Concept(Me.FactTypeInstance.Name)
                lrConcept.Save()

                If abRapidSave Then
                    Call TableConceptInstance.AddConceptInstance(lrConceptInstance)
                Else
                    If TableConceptInstance.ExistsConceptInstance(lrConceptInstance) Then
                        Call TableConceptInstance.UpdateConceptInstance(lrConceptInstance)
                    Else
                        Call TableConceptInstance.AddConceptInstance(lrConceptInstance)
                    End If
                End If

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Public Sub MouseDown() Implements iPageObject.MouseDown

        End Sub

        Public Sub MouseMove() Implements iPageObject.MouseMove

        End Sub

        Public Sub MouseUp() Implements iPageObject.MouseUp

        End Sub

        Public Sub Move(ByVal aiNewX As Integer, ByVal aiNewY As Integer, ByVal abBroadcastInterfaceEvent As Boolean) Implements iPageObject.Move

        End Sub

        Public Sub Moved() Implements iPageObject.Moved

        End Sub

        Public Sub NodeDeleting() Implements iPageObject.NodeDeleting

        End Sub

        Public Sub NodeDeselected() Implements iPageObject.NodeDeselected

        End Sub

        Public Sub NodeModified() Implements iPageObject.NodeModified

        End Sub

        Public Sub NodeSelected() Implements iPageObject.NodeSelected

        End Sub

        Public Sub RepellNeighbouringPageObjects(ByVal aiDepth As Integer) Implements iPageObject.RepellNeighbouringPageObjects

        End Sub

        Public Sub SetAppropriateColour() Implements iPageObject.SetAppropriateColour

        End Sub

        Public Overloads Sub EnableSaveButton() Implements iPageObject.EnableSaveButton
            Throw New NotImplementedException()
        End Sub
    End Class
End Namespace
