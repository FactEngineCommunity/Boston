Imports System.Xml.Serialization

Namespace NORMA.Model

#Region "DataTypes"

    <XmlType(AnonymousType:=True, [Namespace]:="http://schemas.neumont.edu/ORM/2006-04/ORMCore")>
    Partial Public Class AutoCounterNumericDataType

        '''<remarks/>
        <XmlAttribute(AttributeName:="id")>
        Public Property Id() As String

        Public Sub New()
            Me.Id = "_" & System.Guid.NewGuid.ToString.ToUpper
        End Sub

    End Class

    <XmlType(AnonymousType:=True, [Namespace]:="http://schemas.neumont.edu/ORM/2006-04/ORMCore")>
    Partial Public Class VariableLengthTextDataType

        '''<remarks/>
        <XmlAttribute(AttributeName:="id")>
        Public Property Id() As String

        Public Sub New()
            Me.Id = "_" & System.Guid.NewGuid.ToString.ToUpper
        End Sub

    End Class

    <XmlType(AnonymousType:=True, [Namespace]:="http://schemas.neumont.edu/ORM/2006-04/ORMCore")>
    Partial Public Class UnspecifiedDataType

        '''<remarks/>
        <XmlAttribute(AttributeName:="id")>
        Public Property Id() As String

        Public Sub New()
            Me.Id = "_" & System.Guid.NewGuid.ToString.ToUpper
        End Sub

    End Class

    <XmlType(AnonymousType:=True, [Namespace]:="http://schemas.neumont.edu/ORM/2006-04/ORMCore")>
    Partial Public Class FixedLengthTextDataType

        '''<remarks/>
        <XmlAttribute(AttributeName:="id")>
        Public Property Id() As String

        Public Sub New()
            Me.Id = "_" & System.Guid.NewGuid.ToString.ToUpper
        End Sub

    End Class

    <XmlType(AnonymousType:=True, [Namespace]:="http://schemas.neumont.edu/ORM/2006-04/ORMCore")>
    Partial Public Class LargeLengthTextDataType

        '''<remarks/>
        <XmlAttribute(AttributeName:="id")>
        Public Property Id() As String

        Public Sub New()
            Me.Id = "_" & System.Guid.NewGuid.ToString.ToUpper
        End Sub

    End Class

    <XmlType(AnonymousType:=True, [Namespace]:="http://schemas.neumont.edu/ORM/2006-04/ORMCore")>
    Partial Public Class SignedIntegerNumericDataType

        '''<remarks/>
        <XmlAttribute(AttributeName:="id")>
        Public Property Id() As String

        Public Sub New()
            Me.Id = "_" & System.Guid.NewGuid.ToString.ToUpper
        End Sub

    End Class

    <XmlType(AnonymousType:=True, [Namespace]:="http://schemas.neumont.edu/ORM/2006-04/ORMCore")>
    Partial Public Class UnsignedIntegerNumericDataType

        '''<remarks/>
        <XmlAttribute(AttributeName:="id")>
        Public Property Id() As String

        Public Sub New()
            Me.Id = "_" & System.Guid.NewGuid.ToString.ToUpper
        End Sub

    End Class

    <XmlType(AnonymousType:=True, [Namespace]:="http://schemas.neumont.edu/ORM/2006-04/ORMCore")>
    Partial Public Class VariableLengthRawDataDataType

        '''<remarks/>
        <XmlAttribute(AttributeName:="id")>
        Public Property Id() As String

        Public Sub New()
            Me.Id = "_" & System.Guid.NewGuid.ToString.ToUpper
        End Sub

    End Class

    <XmlType(AnonymousType:=True, [Namespace]:="http://schemas.neumont.edu/ORM/2006-04/ORMCore")>
    Partial Public Class TrueOrFalseLogicalDataType

        '''<remarks/>
        <XmlAttribute(AttributeName:="id")>
        Public Property Id() As String

        Public Sub New()
            Me.Id = "_" & System.Guid.NewGuid.ToString.ToUpper
        End Sub

    End Class

    Partial Public Class FloatingPointNumericDataType

        '''<remarks/>
        <XmlAttribute(AttributeName:="id")>
        Public Property Id() As String

        Public Sub New()
            Me.Id = "_" & System.Guid.NewGuid.ToString.ToUpper
        End Sub

    End Class

    Partial Public Class DateTemporalDataType

        '''<remarks/>
        <XmlAttribute(AttributeName:="id")>
        Public Property Id() As String

        Public Sub New()
            Me.Id = "_" & System.Guid.NewGuid.ToString.ToUpper
        End Sub

    End Class

    Partial Public Class SignedSmallIntegerNumericDataType
        '''<remarks/>
        <XmlAttribute(AttributeName:="id")>
        Public Property Id() As String

        Public Sub New()
            Me.Id = "_" & System.Guid.NewGuid.ToString.ToUpper
        End Sub

    End Class

    Partial Public Class SignedLargeIntegerNumericDataType
        '''<remarks/>
        <XmlAttribute(AttributeName:="id")>
        Public Property Id() As String

        Public Sub New()
            Me.Id = "_" & System.Guid.NewGuid.ToString.ToUpper
        End Sub

    End Class

    Partial Public Class UnsignedTinyIntegerNumericDataType
        '''<remarks/>
        <XmlAttribute(AttributeName:="id")>
        Public Property Id() As String

        Public Sub New()
            Me.Id = "_" & System.Guid.NewGuid.ToString.ToUpper
        End Sub

    End Class

    Partial Public Class UnsignedSmallIntegerNumericDataType
        '''<remarks/>
        <XmlAttribute(AttributeName:="id")>
        Public Property Id() As String

        Public Sub New()
            Me.Id = "_" & System.Guid.NewGuid.ToString.ToUpper
        End Sub

    End Class

    Partial Public Class SinglePrecisionFloatingPointNumericDataType
        '''<remarks/>
        <XmlAttribute(AttributeName:="id")>
        Public Property Id() As String

        Public Sub New()
            Me.Id = "_" & System.Guid.NewGuid.ToString.ToUpper
        End Sub

    End Class

    Partial Public Class DoublePrecisionFloatingPointNumericDataType
        '''<remarks/>
        <XmlAttribute(AttributeName:="id")>
        Public Property Id() As String

        Public Sub New()
            Me.Id = "_" & System.Guid.NewGuid.ToString.ToUpper
        End Sub

    End Class

    Partial Public Class DecimalNumericDataType
        '''<remarks/>
        <XmlAttribute(AttributeName:="id")>
        Public Property Id() As String

        Public Sub New()
            Me.Id = "_" & System.Guid.NewGuid.ToString.ToUpper
        End Sub

    End Class

    Partial Public Class MoneyNumericDataType
        '''<remarks/>
        <XmlAttribute(AttributeName:="id")>
        Public Property Id() As String

        Public Sub New()
            Me.Id = "_" & System.Guid.NewGuid.ToString.ToUpper
        End Sub

    End Class

    Partial Public Class FixedLengthRawDataDataType
        '''<remarks/>
        <XmlAttribute(AttributeName:="id")>
        Public Property Id() As String

        Public Sub New()
            Me.Id = "_" & System.Guid.NewGuid.ToString.ToUpper
        End Sub

    End Class

    Partial Public Class LargeLengthRawDataDataType
        '''<remarks/>
        <XmlAttribute(AttributeName:="id")>
        Public Property Id() As String

        Public Sub New()
            Me.Id = "_" & System.Guid.NewGuid.ToString.ToUpper
        End Sub

    End Class

    Partial Public Class PictureRawDataDataType
        '''<remarks/>
        <XmlAttribute(AttributeName:="id")>
        Public Property Id() As String

        Public Sub New()
            Me.Id = "_" & System.Guid.NewGuid.ToString.ToUpper
        End Sub

    End Class

    Partial Public Class OleObjectRawDataDataType
        '''<remarks/>
        <XmlAttribute(AttributeName:="id")>
        Public Property Id() As String

        Public Sub New()
            Me.Id = "_" & System.Guid.NewGuid.ToString.ToUpper
        End Sub

    End Class

    Partial Public Class AutoTimestampTemporalDataType
        '''<remarks/>
        <XmlAttribute(AttributeName:="id")>
        Public Property Id() As String

        Public Sub New()
            Me.Id = "_" & System.Guid.NewGuid.ToString.ToUpper
        End Sub

    End Class

    Partial Public Class TimeTemporalDataType
        '''<remarks/>
        <XmlAttribute(AttributeName:="id")>
        Public Property Id() As String

        Public Sub New()
            Me.Id = "_" & System.Guid.NewGuid.ToString.ToUpper
        End Sub

    End Class

    Partial Public Class DateAndTimeTemporalDataType
        '''<remarks/>
        <XmlAttribute(AttributeName:="id")>
        Public Property Id() As String

        Public Sub New()
            Me.Id = "_" & System.Guid.NewGuid.ToString.ToUpper
        End Sub

    End Class

    Partial Public Class YesOrNoLogicalDataType
        '''<remarks/>
        <XmlAttribute(AttributeName:="id")>
        Public Property Id() As String

        Public Sub New()
            Me.Id = "_" & System.Guid.NewGuid.ToString.ToUpper
        End Sub

    End Class

    Partial Public Class RowIdOtherDataType
        '''<remarks/>
        <XmlAttribute(AttributeName:="id")>
        Public Property Id() As String

        Public Sub New()
            Me.Id = "_" & System.Guid.NewGuid.ToString.ToUpper
        End Sub

    End Class

    Partial Public Class ObjectIdOtherDataType
        '''<remarks/>
        <XmlAttribute(AttributeName:="id")>
        Public Property Id() As String

        Public Sub New()
            Me.Id = "_" & System.Guid.NewGuid.ToString.ToUpper
        End Sub
    End Class

#End Region

End Namespace
