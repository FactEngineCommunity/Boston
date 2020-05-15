Imports System.ComponentModel

Public Class tMyConverter
    'This will act as a typeconverter and present 
    'our collection to the property grid
    Inherits StringConverter

    Public Overloads Overrides Function GetStandardValuesSupported(ByVal context As ITypeDescriptorContext) As Boolean
        Return True 'True tells the propertygrid to display a combobox
    End Function

    Public Overloads Overrides Function GetStandardValuesExclusive(ByVal context As ITypeDescriptorContext) As Boolean
        Return False 'True makes the combobox select only, False allows free text entry.
    End Function

    Public Overloads Overrides Function GetStandardValues(ByVal context As ITypeDescriptorContext) As StandardValuesCollection
        Return New StandardValuesCollection(tGlobalForTypeConverter.OptionStringArray)
        'Exports our global collection of options
    End Function

End Class

Public Class tDataTypeListConverter
    'This will act as a typeconverter and present 
    'our collection to the property grid
    Inherits StringConverter

    Public Overloads Overrides Function GetStandardValuesSupported(ByVal context As ITypeDescriptorContext) As Boolean
        Return True 'True tells the propertygrid to display a combobox
    End Function

    Public Overloads Overrides Function GetStandardValuesExclusive(ByVal context As ITypeDescriptorContext) As Boolean
        Return False 'True makes the combobox select only, False allows free text entry.
    End Function

    Public Overloads Overrides Function GetStandardValues(ByVal context As ITypeDescriptorContext) As StandardValuesCollection
        Return New StandardValuesCollection(tGlobalForTypeConverter.DataTypes)
        'Exports our global collection of options
    End Function

End Class

Public Class tDataStoreListConverter
    'This will act as a typeconverter and present 
    'our collection to the property grid
    Inherits StringConverter

    Public Overloads Overrides Function GetStandardValuesSupported(ByVal context As ITypeDescriptorContext) As Boolean
        Return True 'True tells the propertygrid to display a combobox
    End Function

    Public Overloads Overrides Function GetStandardValuesExclusive(ByVal context As ITypeDescriptorContext) As Boolean
        Return False 'True makes the combobox select only, False allows free text entry.
    End Function

    Public Overloads Overrides Function GetStandardValues(ByVal context As ITypeDescriptorContext) As StandardValuesCollection
        Return New StandardValuesCollection(tGlobalForTypeConverter.DataStores)
        'Exports our global collection of options
    End Function

End Class
