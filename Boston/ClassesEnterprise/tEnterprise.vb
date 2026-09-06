Imports Boston.DataStore

Namespace Enterprise

    Public Class Enterprise
        Inherits DataStore.DataStorePrototype

        <PrimaryKeyField>
        Public Property EnterpriseId As String = System.Guid.NewGuid.ToString

        Public EnterpriseName As String = ""

        Public DefaultBusinessRuleLanguageId As pcenumLanguage = pcenumLanguage.BusinessRulesNaturalLanguage

    End Class

End Namespace
