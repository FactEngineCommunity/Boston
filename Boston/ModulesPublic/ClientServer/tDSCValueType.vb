Imports System.Reflection

Namespace DuplexServiceClient

    Partial Public Class DuplexServiceClient

        Private Sub HandleModelAddValueType(ByRef arModel As FBM.Model, ByVal arInterfaceModel As Viev.FBM.Interface.Model)

            Try

                Dim lrValueType As New FBM.ValueType(arModel,
                                     pcenumLanguage.ORMModel,
                                     arInterfaceModel.ValueType(0).Id,
                                     True)

                '-----------------------------------------------------------------------------
                'Flip the Interface lrValueType.Datatype's enum from the Interface ENUM to the Boston ENUM
                lrValueType.DataType.GetByDescription(arInterfaceModel.ValueType(0).DataType.ToString)
                lrValueType.DataTypeLength = arInterfaceModel.ValueType(0).DataTypeLength
                lrValueType.DataTypePrecision = arInterfaceModel.ValueType(0).DataTypePrecision

                'Originally was (VM-20181318-Delete the below if all seems okay. Tested the above and seems to work fine).
                'Dim liDummyPcenum As pcenumORMDataType = pcenumORMDataType.DataTypeNotSet
                'lrValueType.DataType = liDummyPcenum.GetByDescription(lrInterfaceModel.ValueType(0).DataType.ToString)

                Call arModel.AddValueType(lrValueType, True, False,, True)

                If arInterfaceModel.Page IsNot Nothing Then
                    Dim lrPage As FBM.Page = arModel.Page.Find(Function(x) x.PageId = arInterfaceModel.Page.Id)

                    Dim lrPointF As New PointF(arInterfaceModel.Page.ConceptInstance.X, arInterfaceModel.Page.ConceptInstance.Y)
                    Call lrPage.DropValueTypeAtPoint(lrValueType, lrPointF)
                End If

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Private Sub HandleModelDeleteValueType(ByRef arModel As FBM.Model, ByRef arInterfaceModel As Viev.FBM.Interface.Model)

            Try

                Dim lrInterfaceValueType As Viev.FBM.Interface.ValueType
                lrInterfaceValueType = arInterfaceModel.ValueType(0)

                Dim lrValueType As FBM.ValueType
                lrValueType = arModel.ValueType.Find(Function(x) x.Id = lrInterfaceValueType.Id)

                'CodeSafe: Exit sub if nothing to remove.
                If lrValueType Is Nothing Then Exit Sub

                Call lrValueType.RemoveFromModel(False, True, False)

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Private Sub HandleModelUpdateValueType(ByRef arModel As FBM.Model, ByRef arInterfaceModel As Viev.FBM.Interface.Model)

            Try
                Dim lrInterfaceValueType As Viev.FBM.Interface.ValueType
                lrInterfaceValueType = arInterfaceModel.ValueType(0)

                Dim lrValueType As FBM.ValueType
                lrValueType = arModel.ValueType.Find(Function(x) x.Id = lrInterfaceValueType.Id)

                'CodeSafe: Exit sub if nothing to remove.
                If lrValueType Is Nothing Then Exit Sub

                If lrInterfaceValueType.Name <> lrValueType.Name Then
                    Call lrValueType.SetName(lrInterfaceValueType.Name, False)
                ElseIf lrInterfaceValueType.DataType.ToString <> lrValueType.DataType.ToString Then
                    Dim liORMDataType As pcenumORMDataType = pcenumORMDataType.DataTypeNotSet
                    Call liORMDataType.GetByDescription(lrInterfaceValueType.DataType.ToString)
                    Call lrValueType.SetDataType(liORMDataType, 0, 0, False)
                ElseIf lrInterfaceValueType.DataTypeLength <> lrValueType.DataTypeLength Then
                    Call lrValueType.SetDataTypeLength(lrInterfaceValueType.DataTypeLength, False)
                ElseIf lrInterfaceValueType.DataTypePrecision <> lrValueType.DataTypePrecision Then
                    Call lrValueType.SetDataTypePrecision(lrInterfaceValueType.DataTypePrecision, False)
                End If

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
