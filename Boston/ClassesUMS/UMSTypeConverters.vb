Imports YamlDotNet.Core
Imports YamlDotNet.Serialization
Imports YamlDotNet.Serialization.NamingConventions

Namespace UMS

    ' ─── Custom Type Converters ───────────────────────────────────────────────────

    Public Class DataTypeConverter
        Implements IYamlTypeConverter

        Public Function Accepts(type As Type) As Boolean Implements IYamlTypeConverter.Accepts
            Return type = GetType(DataType)
        End Function

        Public Function ReadYaml(parser As YamlDotNet.Core.IParser, type As Type, rootDeserializer As ObjectDeserializer) As Object Implements IYamlTypeConverter.ReadYaml
            Dim scalar = parser.Consume(Of YamlDotNet.Core.Events.Scalar)()
            Select Case scalar.Value
                Case "Integer" : Return DataType.Integer
                Case "DateTime" : Return DataType.DateTime
                Case "TextFixedLength" : Return DataType.TextFixedLength
                Case "TextVariableLength" : Return DataType.TextVariableLength
                Case Else
                    Throw New YamlDotNet.Core.YamlException($"Unknown DataType: '{scalar.Value}'")
            End Select
        End Function

        Public Sub WriteYaml(emitter As YamlDotNet.Core.IEmitter, value As Object, type As Type, serializer As ObjectSerializer) Implements IYamlTypeConverter.WriteYaml
            Dim dt = CType(value, DataType)
            Dim text = dt.ToString()
            emitter.Emit(New YamlDotNet.Core.Events.Scalar(text))
        End Sub
    End Class


    Public Class ConstraintConverter
        Implements IYamlTypeConverter

        Public Function Accepts(type As Type) As Boolean Implements IYamlTypeConverter.Accepts
            Return type = GetType(Constraint)
        End Function

        Public Function ReadYaml(parser As IParser, type As Type, rootDeserializer As ObjectDeserializer) As Object Implements IYamlTypeConverter.ReadYaml
            Dim scalar = parser.Consume(Of YamlDotNet.Core.Events.Scalar)()
            Select Case scalar.Value
                Case "NOT NULL" : Return Constraint.NotNull
                Case "UNIQUE" : Return Constraint.Unique
                Case Else
                    Throw New YamlDotNet.Core.YamlException($"Unknown Constraint: '{scalar.Value}'")
            End Select
        End Function

        Public Sub WriteYaml(emitter As IEmitter, value As Object, type As Type, serializer As ObjectSerializer) Implements IYamlTypeConverter.WriteYaml
            Dim c = CType(value, Constraint)
            Dim text = If(c = Constraint.NotNull, "NOT NULL", "UNIQUE")
            emitter.Emit(New YamlDotNet.Core.Events.Scalar(text))
        End Sub
    End Class

End Namespace