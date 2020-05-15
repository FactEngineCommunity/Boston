Imports System.ServiceModel
Imports System.Xml.Serialization
Imports System.Reflection
Imports System.Xml
Imports System.Runtime.Serialization

<DataContractFormat()> _
Public Class Page

    <DataMember()> _
    Public Id As String = ""

    <DataMember()> _
    Public Name As String = ""

    <DataMember()> _
    Public ConceptInstance As ConceptInstance

End Class
