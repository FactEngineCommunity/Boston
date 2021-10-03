Public Class tGenericSelection

    Public Type As pcenumGenericSelectionType = pcenumGenericSelectionType.SelectFromDatabase

    Public SelectField As String = ""  'The field to select
    Public SelectIndex As String = ""  'The identifier of the tuple selected
    Public SelectValue As String = ""  'The value (e.g. text value ) of the select_field, as selected by the user
    Public SelectedTag As Object = Nothing
    Public WhereClause As String = ""  'The where clause (extension) to apply to the database to retrieve the values from which to select
    Public OrderByFields As String = "" 'The set of fields to order by
    Public IndexField As String = ""
    Public TableName As String = ""    'The name of the table from which the values (for selection) are retrieved
    Public FormTitle As String = ""    'The title to display in the GenericSelectFrm    
    Public FieldList As String = "" 'Comma separated field list (e.g. "Name, Username" where SelectField might be "FirstName + ' ' + LastName as Name, Username")

    ''' <summary>
    ''' If the MultiColumn Combobox GenericSelect form is used, is the Column from which a return value is returned.
    ''' </summary>
    ''' <remarks></remarks>
    Public SelectColumn As Integer = 0

    Public ColumnWidthString As String = "100" 'e.g. "120;40;100" for a 3 column combobox

    Public TupleList As New List(Of tComboboxItem)

End Class

Public Enum pcenumGenericSelectionType
    SelectFromList
    SelectFromDatabase
End Enum
