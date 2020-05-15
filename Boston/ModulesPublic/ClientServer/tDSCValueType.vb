Namespace DuplexServiceClient

    Partial Public Class DuplexServiceClient

        Private Sub HandleModelAddValueType(ByRef arModel As FBM.Model, ByVal arInterfaceModel As Viev.FBM.Interface.Model)

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

            Call arModel.AddValueType(lrValueType, True, False)

            If arInterfaceModel.Page IsNot Nothing Then
                Dim lrPage As FBM.Page = arModel.Page.Find(Function(x) x.PageId = arInterfaceModel.Page.Id)

                Dim lrPointF As New PointF(arInterfaceModel.Page.ConceptInstance.X, arInterfaceModel.Page.ConceptInstance.Y)
                Call lrPage.DropValueTypeAtPoint(lrValueType, lrPointF)
            End If

        End Sub

        Private Sub HandleModelDeleteValueType(ByRef arModel As FBM.Model, ByRef arInterfaceModel As Viev.FBM.Interface.Model)

            Dim lrInterfaceValueType As Viev.FBM.Interface.ValueType
            lrInterfaceValueType = arInterfaceModel.ValueType(0)

            Dim lrValueType As FBM.ValueType
            lrValueType = arModel.ValueType.Find(Function(x) x.Id = lrInterfaceValueType.Id)

            Call lrValueType.RemoveFromModel(False, True, False)

        End Sub

        Private Sub HandleModelUpdateValueType(ByRef arModel As FBM.Model, ByRef arInterfaceModel As Viev.FBM.Interface.Model)

            Dim lrInterfaceValueType As Viev.FBM.Interface.ValueType
            lrInterfaceValueType = arInterfaceModel.ValueType(0)

            Dim lrValueType As FBM.ValueType
            lrValueType = arModel.ValueType.Find(Function(x) x.Id = lrInterfaceValueType.Id)

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

        End Sub

    End Class

End Namespace
