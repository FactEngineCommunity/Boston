Imports System.ServiceModel
Imports System.Xml.Serialization
Imports System.Reflection
Imports System.Xml
Imports System.Runtime.Serialization

<DataContractFormat()> _
Public Class ConceptInstance

    <DataMember()> _
    Public ModelElementId As String = ""

    <DataMember()> _
    Public X As String

    <DataMember()> _
    Public Y As String

End Class
