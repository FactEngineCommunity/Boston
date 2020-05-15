Imports System.Xml.Serialization

Namespace FBM
    <Serializable()> _
    Public Class FactPredicate
        Inherits Viev.FBM.FactPredicate

        Public Overrides Shadows Function Equals(ByVal other As FBM.Fact) As Boolean Implements System.IEquatable(Of FBM.Fact).Equals

            Dim lr_data As FBM.FactData
            Dim lbFound As Boolean = False

            For Each lr_data In Me.data
                Select Case other.GetType.ToString
                    Case Is = GetType(FBM.Fact).ToString
                        If other.Data.Find(AddressOf lr_data.EqualsByRoleNameData) Is Nothing Then
                            lbFound = False
                            Return False
                        Else
                            lbFound = True
                        End If
                    Case Is = GetType(FBM.FactInstance).ToString
                        Dim lrFactInstance As FBM.FactInstance = other
                        If lrFactInstance.Data.Find(AddressOf lr_data.EqualsByRoleNameData) Is Nothing Then
                            lbFound = False
                            Return False
                        Else
                            lbFound = True
                            Exit For
                        End If
                End Select
            Next
            Return lbFound

        End Function


        Public Overrides Shadows Function EqualsByRoleIdData(ByVal other As FBM.Fact) As Boolean

            Dim lr_data As FBM.FactData
            Dim lbFound As Boolean = False

            For Each lr_data In Me.data
                Select Case other.GetType.ToString
                    Case Is = GetType(FBM.Fact).ToString
                        If other.Data.Find(AddressOf lr_data.EqualsByRoleIdData) Is Nothing Then
                            lbFound = False
                            Return False
                        Else
                            lbFound = True
                        End If
                    Case Is = GetType(FBM.FactInstance).ToString
                        Dim lrFactInstance As FBM.FactInstance = other
                        If lrFactInstance.Data.Find(AddressOf lr_data.EqualsByRoleIdData) Is Nothing Then
                            lbFound = False
                            Return False
                        Else
                            lbFound = True
                            Exit For
                        End If
                End Select
            Next
            Return lbFound

        End Function

    End Class



End Namespace
