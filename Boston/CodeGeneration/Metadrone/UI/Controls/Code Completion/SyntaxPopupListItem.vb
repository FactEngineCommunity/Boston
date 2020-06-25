Namespace UI

    Friend Class SyntaxPopupListItem
        
        Public Enum Icons
            SYSTEM = 0
            METHOD = 1
            [PROPERTY] = 2
            OBJECT_SOURCE = 3
            OBJECT_TABLE = 4
            OBJECT_COLUMN = 5
            OBJECT_VIEW = 6
            OBJECT_FILE = 7
            OBJECT_PROC = 8
            OBJECT_FUNC = 9
            OBJECT_PARAM = 10
            OBJECT_INPARAM = 11
            OBJECT_OUTPARAM = 12
            OBJECT_INOUTPARAM = 13
            VAR_MAIN = 14
            VAR_TEMPLATE = 15
            SOURCE = 16
            TEMPLATE = 17
            TEMPLATE_PARAM = 18
            TRANSFORMATION = 19
        End Enum

        Public Item As String = Nothing
        Public Description As String = Nothing
        Public DefaultCompletion As String = Nothing
        Public Icon As Icons = Icons.SYSTEM

        Public Sub New(ByVal Item As String, ByVal Description As String, _
                       ByVal DefaultCompletion As String, ByVal Icon As Icons)
            Me.Item = Item
            Me.Description = Description
            Me.DefaultCompletion = DefaultCompletion
            Me.Icon = Icon
        End Sub
    End Class

End Namespace