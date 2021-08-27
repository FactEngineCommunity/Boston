Imports System.Reflection

Public Module tableORMPredicatePart

    Public Sub AddPredicatePart(ByVal arPredicatePart As FBM.PredicatePart)

        Dim lsSQLQuery As String = ""

        Try
            lsSQLQuery = "INSERT INTO MetaModelPredicatePart"
            lsSQLQuery &= " VALUES ("
            lsSQLQuery &= " #" & Now & "#"
            lsSQLQuery &= " ,#" & Now & "#"
            lsSQLQuery &= " ,'" & Trim(arPredicatePart.Model.ModelId) & "'"
            lsSQLQuery &= " ,'" & Trim(arPredicatePart.FactTypeReading.Id) & "'"
            lsSQLQuery &= " ," & arPredicatePart.SequenceNr            
            'lsSQLQuery &= " ,'" & Trim(arPredicatePart.ObjectType1.Symbol) & "'"
            'If IsSomething(arPredicatePart.ObjectType2) Then
            '    '----------------------------------
            '    'FactType with more than one Role
            '    '----------------------------------
            '    lsSQLQuery &= " ,'" & Trim(Replace(arPredicatePart.ObjectType2.Symbol, "'", "`")) & "'"
            'Else
            '    '----------------
            '    'Unary FactType
            '    '----------------
            '    lsSQLQuery &= " ,''"
            'End If
            lsSQLQuery &= " ,'',''" ' Remove when Symbol1 and Symbol2 fields have been removed from the table.
            lsSQLQuery &= " ,'" & Trim(Replace(arPredicatePart.PredicatePartText, "'", "`")) & "'"
            lsSQLQuery &= " ,'" & Trim(arPredicatePart.RoleId) & "'"
            lsSQLQuery &= " ,'" & Trim(Replace(arPredicatePart.PreBoundText, "'", "`")) & "'"
            lsSQLQuery &= " ,'" & Trim(Replace(arPredicatePart.PostBoundText, "'", "`")) & "'"
            lsSQLQuery &= ")"

            Call pdbConnection.Execute(lsSQLQuery)

        Catch ex As Exception
            Dim lsMessage As String
            lsMessage = "Error: TablePredicatePart.AddPredicatePart"
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Sub

    Public Function ExistsPredicatePart(ByVal arPredicatePart As FBM.PredicatePart) As Boolean

        Try
            Dim lsSQLQuery As String = ""
            Dim lREcordset As New ADODB.Recordset

            lREcordset.ActiveConnection = pdbConnection
            lREcordset.CursorType = pcOpenStatic

            lsSQLQuery = "SELECT COUNT(*)"
            lsSQLQuery &= "  FROM MetaModelPredicatePart"
            lsSQLQuery &= " WHERE ModelId = '" & Trim(arPredicatePart.Model.ModelId) & "'"
            lsSQLQuery &= "   AND FactTypeReadingId = '" & Trim(arPredicatePart.FactTypeReading.Id) & "'"
            lsSQLQuery &= "   AND SequenceNr = " & arPredicatePart.SequenceNr

            lREcordset.Open(lsSQLQuery)

            If lREcordset(0).Value > 0 Then
                ExistsPredicatePart = True
            Else
                ExistsPredicatePart = False
            End If

            lREcordset.Close()

        Catch ex As Exception
            Dim lsMessage As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Function

    Public Sub DeletePredicatePart(ByVal arPredicatePart As FBM.PredicatePart)

        Try
            Dim lsSQLQuery As String

            lsSQLQuery = " DELETE FROM MetaModelPredicatePart"
            lsSQLQuery &= " WHERE ModelId = '" & Trim(arPredicatePart.Model.ModelId) & "'"
            lsSQLQuery &= "   AND FactTypeReadingId = '" & Trim(arPredicatePart.FactTypeReading.Id) & "'"
            lsSQLQuery &= "   AND SequenceNr = " & arPredicatePart.SequenceNr

            pdbConnection.Execute(lsSQLQuery)

        Catch ex As Exception
            Dim lsMessage1 As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage1 &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Sub

    Public Sub DeletePredicatePartByRole(ByVal arPredicatePart As FBM.PredicatePart)

        Try
            Dim lsSQLQuery As String

            lsSQLQuery = " DELETE FROM MetaModelPredicatePart"
            lsSQLQuery &= " WHERE ModelId = '" & Trim(arPredicatePart.Model.ModelId) & "'"
            lsSQLQuery &= "   AND FactTypeReadingId = '" & Trim(arPredicatePart.FactTypeReading.Id) & "'"
            lsSQLQuery &= "   AND RoleId = '" & arPredicatePart.Role.Id & "'"

            pdbConnection.Execute(lsSQLQuery)

        Catch ex As Exception
            Dim lsMessage1 As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage1 &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Sub

    Public Function GetPredicatePartsByFactTypeReading(ByVal arFactTypeReading As FBM.FactTypeReading) As List(Of FBM.PredicatePart)

        Dim lrPredicatePart As FBM.PredicatePart
        Dim lsSQLQuery As String = ""
        Dim lREcordset As New ADODB.Recordset
        Dim lsMessage As String = ""

        lREcordset.ActiveConnection = pdbConnection
        lREcordset.CursorType = pcOpenStatic

        lsSQLQuery = "SELECT *"
        lsSQLQuery &= "  FROM MetaModelPredicatePart"
        lsSQLQuery &= " WHERE ModelId = '" & Trim(arFactTypeReading.Model.ModelId) & "'"
        lsSQLQuery &= "   AND FactTypeReadingId = '" & Trim(arFactTypeReading.Id) & "'"
        lsSQLQuery &= " ORDER BY SequenceNr"

        GetPredicatePartsByFactTypeReading = New List(Of FBM.PredicatePart)

        Try
            lREcordset.Open(lsSQLQuery)

            If Not lREcordset.EOF Then
                While Not lREcordset.EOF
                    lrPredicatePart = New FBM.PredicatePart(arFactTypeReading.Model, arFactTypeReading)
                    lrPredicatePart.isDirty = False
                    lrPredicatePart.SequenceNr = lREcordset("SequenceNr").Value
                    lrPredicatePart.Role = arFactTypeReading.FactType.GetRoleById(lREcordset("RoleId").Value)
                    '----------------------------------------------------------------------------------------------------                    
                    'lrPredicatePart.ObjectType1 = arFactTypeReading.Model.GetModelObjectByName(Trim(lREcordset("Symbol1").Value))
                    'lrPredicatePart.ObjectType2 = arFactTypeReading.Model.GetModelObjectByName(Trim(lREcordset("Symbol2").Value))
                    lrPredicatePart.PreBoundText = Trim(NullVal(lREcordset("PreboundText").Value, ""))
                    lrPredicatePart.PostBoundText = Trim(NullVal(lREcordset("PostboundText").Value, ""))
                    lrPredicatePart.PredicatePartText = Trim(lREcordset("PredicatePartText").Value)
                    lrPredicatePart.isDirty = False


                    GetPredicatePartsByFactTypeReading.Add(lrPredicatePart)

                    lREcordset.MoveNext()
                End While
            Else
                GetPredicatePartsByFactTypeReading = Nothing
            End If

            lREcordset.Close()

        Catch ex As Exception
            Dim lsMessage1 As String
            Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

            lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
            lsMessage1 &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Function

    Public Sub UpdatePredicatePart(ByVal arPredicatePart As FBM.PredicatePart)

        Dim lsSQLQuery As String

        Try

            lsSQLQuery = " UPDATE MetaModelPredicatePart"
            lsSQLQuery &= "   SET RoleId = '" & Trim(arPredicatePart.RoleId) & "'"
            lsSQLQuery &= "      ,PreboundText = '" & Replace(arPredicatePart.PreBoundText, "'", "`") & "'"
            lsSQLQuery &= "      ,PostboundText = '" & Replace(arPredicatePart.PostBoundText, "'", "`") & "'"
            lsSQLQuery &= "      ,PredicatePartText = '" & Replace(arPredicatePart.PredicatePartText, "'", "`") & "'"
            lsSQLQuery &= " WHERE ModelId = '" & Trim(arPredicatePart.Model.ModelId) & "'"
            lsSQLQuery &= "   AND FactTypeReadingId = '" & Trim(arPredicatePart.FactTypeReading.Id) & "'"
            lsSQLQuery &= "   AND SequenceNr = " & arPredicatePart.SequenceNr

            pdbConnection.Execute(lsSQLQuery)

        Catch ex As Exception
            Dim lsMessage As String
            lsMessage = "Error: TablePredicatePart.UpdatePredicatePart"
            lsMessage &= vbCrLf & vbCrLf & ex.Message
            prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
        End Try

    End Sub

End Module
