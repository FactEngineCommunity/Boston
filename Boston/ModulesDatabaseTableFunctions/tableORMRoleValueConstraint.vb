Imports System.Reflection

Namespace TableRoleValueConstraint

    Module TableRoleValueConstraint

        Public Sub AddRoleValueConstraint(ByVal arRoleConstraint As FBM.RoleConstraint, ByVal ar_concept As FBM.Concept)

            Dim lsSQLQuery As String = ""

            Try

                lsSQLQuery = "INSERT INTO MetaModelRoleValueConstraint"
                lsSQLQuery &= " VALUES ("
                lsSQLQuery &= " #" & Now & "#"
                lsSQLQuery &= " ,#" & Now & "#"
                lsSQLQuery &= " ,'" & Trim(arRoleConstraint.Model.ModelId) & "'"
                lsSQLQuery &= " ,'" & Trim(arRoleConstraint.Id) & "'"
                lsSQLQuery &= " ,'" & Database.MakeStringSafe(Trim(ar_concept.Symbol)) & "'"
                lsSQLQuery &= ")"

                Call pdbConnection.Execute(lsSQLQuery)

            Catch ex As Exception
                Dim lsMessage As String
                lsMessage = "Error: TableRoleValueConstraint.AddValueTypeValueConstraint"
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Public Sub DeleteValueConstraint(ByVal arRoleConstraint As FBM.RoleConstraint, ByVal asValueConstraint As String)

            Dim lsSQLQuery As String = ""

            lsSQLQuery = "DELETE FROM MetaModelRoleConstraintValueConstraint"
            lsSQLQuery &= " WHERE ModelId = '" & Replace(arRoleConstraint.Model.ModelId, "'", "`") & "'"
            lsSQLQuery &= "   AND RoleConstraintId = '" & Replace(arRoleConstraint.Id, "'", "`") & "'"
            lsSQLQuery &= "   AND Symbol = '" & asValueConstraint & "'"

            pdbConnection.BeginTrans()
            pdbConnection.Execute(lsSQLQuery)
            pdbConnection.CommitTrans()

        End Sub

        Public Function ExistsRoleValueConstraint(ByVal arRoleConstraint As FBM.RoleConstraint, ByVal ar_concept As FBM.Concept) As Boolean

            Dim lsSQLQuery As String = ""
            Dim lREcordset As New ADODB.Recordset

            Try
                '------------------------
                'Initialise return value
                '------------------------
                ExistsRoleValueConstraint = False

                lREcordset.ActiveConnection = pdbConnection
                lREcordset.CursorType = pcOpenStatic

                lsSQLQuery = "SELECT COUNT(*)"
                lsSQLQuery &= "  FROM MetaModelRoleValueConstraint RVC"
                lsSQLQuery &= " WHERE RoleConstraintId = '" & Trim(arRoleConstraint.Id) & "'"
                lsSQLQuery &= "   AND Symbol = '" & Database.MakeStringSafe(Trim(ar_concept.Symbol)) & "'"


                lREcordset.Open(lsSQLQuery)

                If lREcordset(0).Value > 0 Then
                    ExistsRoleValueConstraint = True
                Else
                    ExistsRoleValueConstraint = False
                End If

                lREcordset.Close()

            Catch ex As Exception
                Dim lsMessage As String
                lsMessage = "Error: TableRoleConstraintValueConstraint.ExistsRoleConstraintValueConstraint"
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Function

        Sub GetValueConstraintsByRoleConstraint(ByRef arRoleConstraint As FBM.RoleConstraint)

            Dim lsSQLQuery As String = ""
            Dim lREcordset As New ADODB.Recordset

            Try

                lREcordset.ActiveConnection = pdbConnection
                lREcordset.CursorType = pcOpenStatic

                lsSQLQuery = "SELECT *"
                lsSQLQuery &= "  FROM MetaModelRoleValueConstraint RVC"
                lsSQLQuery &= " WHERE RVC.ModelId = '" & arRoleConstraint.Model.ModelId & "'"
                lsSQLQuery &= "   AND RVC.RoleConstraintId = '" & Trim(arRoleConstraint.Id) & "'"


                lREcordset.Open(lsSQLQuery)

                If Not lREcordset.EOF Then
                    While Not lREcordset.EOF

                        '--------------------------------------------------------------------------
                        'Add the Concept/Symbol/Value to the value_constraint' for this RoleConstraint                        
                        arRoleConstraint.ValueConstraint.Add(lREcordset("Symbol").Value)

                        Dim lrDictionaryEntry As FBM.DictionaryEntry = New FBM.DictionaryEntry(arRoleConstraint.Model, lREcordset("Symbol").Value, pcenumConceptType.Value)

                        '--------------------------------------------
                        'Add the Concept/Symbol/Value to the Model if it does not already exist within the 
                        '  ModelDictionary (e.g. The same 'Value' may be within many ValueConstraints/Facts
                        '  NB Needs to be CaseInsensitive because MSAccess/Jet is not case sensitive.
                        '  This is because otherwise HORSE would override Horse on save back to database. This can be problematic for DBName where Horse is otherwise an Entity with a DBName and HORSE may just be a Value.
                        '--------------------------------------------
                        lrDictionaryEntry = arRoleConstraint.Model.AddModelDictionaryEntry(lrDictionaryEntry, True, True, False, False, False, True, True)

                        lREcordset.MoveNext()
                    End While
                End If

                lREcordset.Close()

            Catch ex As Exception
                Dim lsMessage As String
                lsMessage = "Error: TableRoleConstraintValueConstraint.GetValueConstraintsByRoleConstraint"
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Public Sub ModifyKey(ByVal arRoleConstraint As FBM.ValueType, ByVal asNewKey As String)

            Dim lsSQLQuery As String

            Try

                lsSQLQuery = " UPDATE MetaModelRoleValueConstraint"
                lsSQLQuery &= "   SET ValueTypeId = '" & Trim(Replace(asNewKey, "'", "`")) & "'"
                lsSQLQuery &= " WHERE ValueTypeId = '" & Replace(arRoleConstraint.Id, "'", "`") & "'"

                pdbConnection.BeginTrans()
                pdbConnection.Execute(lsSQLQuery)
                pdbConnection.CommitTrans()

            Catch ex As Exception
                Dim lsMessage As String
                lsMessage = "Error: TableValueTypeValueConstraint.ModifyKey"
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Public Sub remove_unneeded_value_constraints(ByVal arRoleConstraint As FBM.RoleConstraint)

            Try
                Dim liCounter As Integer = 0
                Dim ls_value As String = ""
                Dim ls_ValueSymbol_list As String = ""
                Dim lsSQLQuery As String = ""

                For Each ls_value In arRoleConstraint.ValueConstraint
                    If liCounter = 0 Then
                        ls_ValueSymbol_list = "'" & Database.MakeStringSafe(Trim(ls_value)) & "'"
                    Else
                        ls_ValueSymbol_list &= ",'" & Database.MakeStringSafe(Trim(ls_value)) & "'"
                    End If
                    liCounter += 1
                Next

                If ls_ValueSymbol_list <> "" Then
                    lsSQLQuery = "DELETE FROM MetaModelRoleValueConstraint"
                    lsSQLQuery &= " WHERE RoleConstraintId = '" & Trim(arRoleConstraint.Id) & "'"
                    lsSQLQuery &= "   AND Symbol NOT IN (" & ls_ValueSymbol_list & ")"

                    Call pdbConnection.Execute(lsSQLQuery)
                End If

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
            End Try


        End Sub

    End Module

End Namespace