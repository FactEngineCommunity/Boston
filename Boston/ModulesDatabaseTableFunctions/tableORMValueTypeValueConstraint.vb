Imports System.Reflection

Namespace TableValueTypeValueConstraint

    Module TableValueTypeValueConstraint

        Public Sub AddValueTypeValueConstraint(ByVal arValueType As FBM.ValueType, ByVal ar_concept As FBM.Concept)

            Dim lsSQLQuery As String = ""

            Try

                lsSQLQuery = "INSERT INTO MetaModelValueTypeValueConstraint"
                lsSQLQuery &= " VALUES ("
                lsSQLQuery &= pdbConnection.DateWrap(Now.ToString("yyyy/MM/dd HH:mm:ss"))
                lsSQLQuery &= "," & pdbConnection.DateWrap(Now.ToString("yyyy/MM/dd HH:mm:ss"))
                lsSQLQuery &= " ,'" & Trim(arValueType.Model.ModelId) & "'"
                lsSQLQuery &= " ,'" & Trim(arValueType.Id) & "'"
                lsSQLQuery &= " ,'" & Database.MakeStringSafe(Trim(ar_concept.Symbol)) & "'"
                lsSQLQuery &= ")"

                Call pdbConnection.Execute(lsSQLQuery)

            Catch ex As Exception
                Dim lsMessage As String
                lsMessage = "Error: TableValueTypeValueConstraint.AddValueTypeValueConstraint"
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Public Sub DeleteValueConstraint(ByVal arValueType As FBM.ValueType, ByVal asValueConstraint As String)

            Dim lsSQLQuery As String = ""

            lsSQLQuery = "DELETE FROM MetaModelValueTypeValueConstraint"
            lsSQLQuery &= " WHERE ModelId = '" & Replace(arValueType.Model.ModelId, "'", "`") & "'"
            lsSQLQuery &= "   AND ValueTypeId = '" & Replace(arValueType.Id, "'", "`") & "'"
            lsSQLQuery &= "   AND Symbol = '" & asValueConstraint & "'"

            pdbConnection.BeginTrans()
            pdbConnection.Execute(lsSQLQuery)
            pdbConnection.CommitTrans()

        End Sub

        Public Function ExistsValueTypeValueConstraint(ByVal arValueType As FBM.ValueType, ByVal ar_concept As FBM.Concept) As Boolean

            Dim lsSQLQuery As String = ""
            Dim lREcordset As New RecordsetProxy

            Try
                '------------------------
                'Initialise return value
                '------------------------
                ExistsValueTypeValueConstraint = False

                lREcordset.ActiveConnection = pdbConnection
                lREcordset.CursorType = pcOpenStatic

                lsSQLQuery = "SELECT COUNT(*)"
                lsSQLQuery &= "  FROM MetaModelValueTypeValueConstraint VTVC"
                lsSQLQuery &= " WHERE ValueTypeId = '" & Trim(arValueType.Id) & "'"
                lsSQLQuery &= "   AND Symbol = '" & Database.MakeStringSafe(Trim(ar_concept.Symbol)) & "'"


                lREcordset.Open(lsSQLQuery)

                If lREcordset(0).Value > 0 Then
                    ExistsValueTypeValueConstraint = True
                Else
                    ExistsValueTypeValueConstraint = False
                End If

                lREcordset.Close()

            Catch ex As Exception
                Dim lsMessage As String
                lsMessage = "Error: TableValueTypeValueConstraint.ExistsValueTypeValueConstraint"
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Function

        Sub GetValueConstraintsByValueType(ByRef arValueType As FBM.ValueType)

            Dim lsSQLQuery As String = ""
            Dim lREcordset As New RecordsetProxy

            Try

                lREcordset.ActiveConnection = pdbConnection
                lREcordset.CursorType = pcOpenStatic

                lsSQLQuery = "SELECT *"
                lsSQLQuery &= "  FROM MetaModelValueTypeValueConstraint VTVC"
                lsSQLQuery &= " WHERE VTVC.ModelId = '" & arValueType.Model.ModelId & "'"
                lsSQLQuery &= "   AND VTVC.ValueTypeId = '" & Trim(arValueType.Id) & "'"


                lREcordset.Open(lsSQLQuery)

                If Not lREcordset.EOF Then
                    While Not lREcordset.EOF

                        '--------------------------------------------------------------------------
                        'Add the Concept/Symbol/Value to the value_constraint' for this ValueType                        
                        arValueType.ValueConstraint.Add(lREcordset("Symbol").Value)

                        Dim lrDictionaryEntry As FBM.DictionaryEntry = New FBM.DictionaryEntry(arValueType.Model, lREcordset("Symbol").Value, pcenumConceptType.Value)

                        '--------------------------------------------
                        'Add the Concept/Symbol/Value to the Model if it does not already exist within the 
                        '  ModelDictionary (e.g. The same 'Value' may be within many ValueConstraints/Facts
                        '  NB Needs to be CaseInsensitive because MSAccess/Jet is not case sensitive.
                        '  This is because otherwise HORSE would override Horse on save back to database. This can be problematic for DBName where Horse is otherwise an Entity with a DBName and HORSE may just be a Value.
                        '--------------------------------------------
                        lrDictionaryEntry = arValueType.Model.AddModelDictionaryEntry(lrDictionaryEntry, True, True, False, False, False, True, True)

                        lREcordset.MoveNext()
                    End While
                End If

                lREcordset.Close()

            Catch ex As Exception
                Dim lsMessage As String
                lsMessage = "Error: TableValueTypeValueConstraint.GetValueConstraintsByValueType"
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try




        End Sub

        Public Sub ModifyKey(ByVal arValueType As FBM.ValueType, ByVal as_new_key As String)

            Dim lsSQLQuery As String

            Try

                lsSQLQuery = " UPDATE MetaModelValueTypeValueConstraint"
                lsSQLQuery &= "   SET ValueTypeId = '" & Trim(Replace(as_new_key, "'", "`")) & "'"
                lsSQLQuery &= " WHERE ValueTypeId = '" & Replace(arValueType.Id, "'", "`") & "'"

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

        Public Sub remove_unneeded_value_constraints(ByVal arValueType As FBM.ValueType)

            Try
                Dim liCounter As Integer = 0
                Dim ls_value As String = ""
                Dim ls_ValueSymbol_list As String = ""
                Dim lsSQLQuery As String = ""

                For Each ls_value In arValueType.ValueConstraint
                    If liCounter = 0 Then
                        ls_ValueSymbol_list = "'" & Database.MakeStringSafe(Trim(ls_value)) & "'"
                    Else
                        ls_ValueSymbol_list &= ",'" & Database.MakeStringSafe(Trim(ls_value)) & "'"
                    End If
                    liCounter += 1
                Next

                If ls_ValueSymbol_list <> "" Then
                    lsSQLQuery = "DELETE FROM MetaModelValueTypeValueConstraint"
                    lsSQLQuery &= " WHERE ValueTypeId = '" & Trim(arValueType.Id) & "'"
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