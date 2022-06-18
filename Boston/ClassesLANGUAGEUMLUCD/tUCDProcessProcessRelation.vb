Imports System.Reflection
Imports System.ComponentModel

Namespace UCD
    Public Class ProcessProcessRelation
        Inherits UML.ProcessProcessRelation
        Implements FBM.iPageObject

        Private WithEvents _CMMLProcessProcessRelation As CMML.ProcessProcessRelation
        Public Shadows Property CMMLProcessProcessRelation As CMML.ProcessProcessRelation
            Get
                Return Me._CMMLProcessProcessRelation
            End Get
            Set(value As CMML.ProcessProcessRelation)
                Me._CMMLProcessProcessRelation = value
            End Set
        End Property

        Private _IsExtends As Boolean = False
        <CategoryAttribute("Extends/Includes"),
        Browsable(True),
        [ReadOnly](False),
        DisplayName("Is Extends"),
        DescriptionAttribute("The first Process extends the functionality of the second Process.")>
        Public Shadows Property IsExtends As Boolean
            Get
                Return Me._IsExtends
            End Get
            Set(value As Boolean)
                Me._IsExtends = value
            End Set
        End Property

        Private _IsIncludes As Boolean = False
        <CategoryAttribute("Extends/Includes"),
        Browsable(True),
        [ReadOnly](False),
        DisplayName("Is Includes"),
        DescriptionAttribute("The first Process includes the functionality of the second Process.")>
        Public Shadows Property IsIncludes As Boolean
            Get
                Return Me._IsIncludes
            End Get
            Set(value As Boolean)
                Me._IsIncludes = value
            End Set
        End Property

        ''' <summary>
        ''' Parameterless Constructor
        ''' </summary>
        Public Sub New()
        End Sub

        Public Sub New(ByRef arUMLModel As UML.Model, ByRef arProcess1 As UML.Process, ByRef arProcess2 As UML.Process)

            Me.UMLModel = arUMLModel
            Me.Process1 = arProcess1
            Me.Process2 = Process2

        End Sub

        Private Sub CMMLProcessProcessRelation_IsIncludesChanged(abIsIncludes As Boolean) Handles _CMMLProcessProcessRelation.IsIncludesChanged

            Try
                Me.IsIncludes = abIsIncludes

                Call Me.RefreshShape()

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub


        Private Sub CMMLProcessProcessRelation_IsExtendsChanged(abIsExtends As Boolean) Handles _CMMLProcessProcessRelation.IsExtendsChanged

            Try
                Me.IsExtends = abIsExtends

                Call Me.RefreshShape()

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Public Sub RefreshShape(Optional ByVal aoChangedPropertyItem As PropertyValueChangedEventArgs = Nothing,
                                Optional ByVal asSelectedGridItemLabel As String = "")


            Try
                '------------------------------------------------
                'Set the values in the underlying Model.ValueType
                '------------------------------------------------

                'VM 20101218-Use the following and e above to fix the below
                ' -> "ValueConstraintValue", e.OldValue.ToString, e.ChangedItem.Value.ToString
                'was -> Optional ByVal asAttributeName As String = Nothing, Optional ByVal aoOldValue As Object = Nothing, Optional ByVal aoNewValue As Object = Nothing
                If IsSomething(aoChangedPropertyItem) Then
                    Select Case aoChangedPropertyItem.ChangedItem.PropertyDescriptor.Name
                        Case Is = "IsExtends"
                            Call Me.CMMLProcessProcessRelation.setIsExtends(Me.IsExtends)
                            Call Me.CMMLProcessProcessRelation.setIsIncludes(Not Me.IsExtends)
                            Me.IsIncludes = Not Me.IsExtends
                        Case Is = "IsIncludes"
                            Call Me.CMMLProcessProcessRelation.setIsIncludes(Me.IsIncludes)
                            Call Me.CMMLProcessProcessRelation.setIsExtends(Not Me.IsIncludes)
                            Me.IsExtends = Not Me.IsIncludes
                    End Select
                End If

                '---------------------------------------
                'Graphical elements.
                '---------------------------------------
                If Me.Link IsNot Nothing Then
                    If Me.IsExtends Then
                        Me.Link.Text = "<<Extends>>"
                    ElseIf Me.IsIncludes Then
                        Me.Link.Text = "<<Includes>>"
                    End If
                End If

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Private Sub _CMMLProcessProcessRelation_RemovedFromModel() Handles _CMMLProcessProcessRelation.RemovedFromModel

            Try
                Call Me.RemoveFromPage()

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try
        End Sub
    End Class

End Namespace
