Friend Class tGlobalForTypeConverter

    'This class is read by the OptionConverter     
    'Holds the options available in the dropdown for the PropertyGrid
    Friend Shared OptionStringArray() As String = {" ", _
                                                   ".#", _
                                                   ".code", _
                                                   ".Code", _
                                                   ".Id", _
                                                   ".id", _
                                                   ".ID", _
                                                   ".name", _
                                                   ".Name", _
                                                   ".Nr", _
                                                   ".nr", _
                                                   ".Title", _
                                                   ".title", _
                                                   "AUD:", _
                                                   "CE:", _
                                                   "Celsius:", _
                                                   "cm:", _
                                                   "EUR:", _
                                                   "Fahrenheit:", _
                                                   "kg:", _
                                                   "km:", _
                                                   "mile:", _
                                                   "mm:", _
                                                   "USD:"}

    Friend Shared DataTypes() As String = {"<Data Type Not Set>", _
                                           "Logical: True or False", _
                                           "Logical: Yes or No", _
                                           "Numeric: Auto Counter", _
                                           "Numeric: Decimal", _
                                           "Numeric: Float (Custom Precision)", _
                                           "Numeric: Float (Double Precision)", _
                                           "Numeric: Float (Single Precision)", _
                                           "Numeric: Money", _
                                           "Numeric: Signed Big Integer", _
                                           "Numeric: Signed Integer", _
                                           "Numeric: Signed Small Integer", _
                                           "Numeric: Unsigned Big Integer", _
                                           "Numeric: Unsigned Integer", _
                                           "Numeric: Unsigned Small Integer", _
                                           "Numeric: Unsigned Tiny Integer", _
                                           "Other: Object ID", _
                                           "Other: Row ID", _
                                           "Raw Data: Fixed Length", _
                                           "Raw Data: Large Length", _
                                           "Raw Data: OLE Object", _
                                           "Raw Data: Picture", _
                                           "Raw Data: Variable Length", _
                                           "Temporal: Auto Timestamp", _
                                           "Temporal: Date", _
                                           "Temporal: Date & Time", _
                                           "Temporal: Time", _
                                           "Text: Fixed Length", _
                                           "Text: Large Length", _
                                           "Text: Variable Length"}

    ''' <summary>
    ''' Used for DataFlowDiagram links etc. Dynamic for the DataStores of a Model.
    ''' </summary>
    ''' <remarks></remarks>
    Friend Shared DataStores() As String = {"None" _
                                          }

End Class

