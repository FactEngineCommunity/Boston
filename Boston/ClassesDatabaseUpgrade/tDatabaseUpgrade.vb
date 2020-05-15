Imports System.Reflection

Namespace DatabaseUpgrade
    Public Class Upgrade
        Implements iObjectRelationalMap(Of DatabaseUpgrade.Upgrade)

        Public UpgradeId As Integer
        Public FromVersionNr As String
        Public ToVersionNr As String
        Public SuccessfulImplementation As Boolean

        Public Sub Create() Implements iObjectRelationalMap(Of Upgrade).Create

            Call tableDatabaseUpgrade.AddUpgrade(Me)

        End Sub

        Public Sub Delete() Implements iObjectRelationalMap(Of Upgrade).Delete

            'Not implemented

        End Sub

        Public Function Load() As DatabaseUpgrade.Upgrade Implements iObjectRelationalMap(Of Upgrade).Load
            'Not implemented
            Return Me
        End Function

        Public Sub Save(Optional ByRef abRapidSave As Boolean = False) Implements iObjectRelationalMap(Of Upgrade).Save

            Try
                If tableDatabaseUpgrade.ExistsDatabaseUpgradeInRichmond(Me.FromVersionNr, Me.ToVersionNr) Then
                    '---------------------------------------------------
                    'Is already at verions required. Nothing to update
                    '---------------------------------------------------
                Else
                    Call tableDatabaseUpgrade.AddUpgrade(Me)
                End If

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
            End Try
        End Sub

    End Class
End Namespace
