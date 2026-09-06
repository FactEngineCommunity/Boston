Imports YamlDotNet.Serialization
Imports YamlDotNet.Serialization.NamingConventions

Namespace UMS

    ''' <summary>
    ''' Example Property (that is also an array).
    ''' 
    ''' - Name: PhoneNumbers
    '     DataType: TextVariableLength
    '     IsArray: true
    '     ArrayConstraints:
    '       MinItems: 0
    '       MaxItems: 10
    '       Unique: false        # whether array elements must be distinct
    '     Readings:
    '       - Person has PhoneNumber
    ''' </summary>
    Public Class PropertyDefinition

        ''' <summary>
        ''' The property name e.g. Id, Name, CinemaId
        ''' </summary>
        Public Property Name As String

        ''' <summary>
        ''' The Fact-Based (Modelling) Alias/Name for the Property if there is one.
        ''' E.g. A Property, 'FirstName', (in the database model) may have a space delimited name/alias of 'First Name' in a Fact-Based Model.
        ''' </summary>
        ''' <returns></returns>
        Public Property FactBasedName As String = Nothing

        ''' <summary>
        ''' Optional alias / role name e.g. "Cinema Name", "Film Title"
        ''' </summary>
        Public Property [Alias] As String

        ''' <summary>
        ''' Data type: Integer, DateTime, TextFixedLength, TextVariableLength
        ''' </summary>
        Public Property DataType As UMS.DataType

        ''' <summary>
        ''' Length for properties with Data Types that have a Length. E.g. 100, 200, 1. E.g. TextFixedLength(100)
        ''' Null for all other data types
        ''' </summary>
        Public Property Length As Integer?

        ''' <summary>
        ''' E.g. Number of decimal places. E.g. Data Type Money(10,2) where 2 is the Precision and 10 is the length.
        ''' Null for all other data types
        ''' </summary>
        Public Property Precision As Integer?

        ''' <summary>
        ''' Constraints e.g. [NOT NULL, UNIQUE]
        ''' </summary>
        Public Property Constraints As List(Of UMS.Constraint)

        Public Property FactTypeReadings As List(Of FactTypeReadings)

        Public Property Facts As List(Of UMS.Facts)

        Public Property IsArray As Boolean?

        Public Property ArrayConstraints As UMS.ArrayConstraints

    End Class

    Public Class ArrayConstraints

        Public Property MinItems As Integer?

        Public Property MaxItems As Integer?

        Public Property Unique As Boolean?

    End Class

End Namespace