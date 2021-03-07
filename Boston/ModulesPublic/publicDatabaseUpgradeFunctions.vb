Imports System.Reflection
Imports System.Xml.Serialization
Imports System.IO

Namespace DatabaseUpgradeFunctions
    Public Module publicDatabaseUpgradeFunctions

        ''' <summary>
        ''' For release of the new data model for PredicateParts of FactTypeReadings.
        ''' Introduced the following Fields to the MetaModelPredicatePart table:
        '''   RowId
        '''   PreboundText
        '''   PostboundText
        ''' This function sets the appropriate RowId for each row of the MetaModelPredicatePart table.
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub UpgradePredicatePartRoleIds()
            '--------------------------------------------------------------------------------------------------------------
            'PSEUDOCODE
            '  * For each PredicatePart record in MetaModelPredicatePart
            '    * Match Object1 (of the PredicatePart) 
            '        to the Role of the FactType of the FactTypeReading (of the PredicatePart)
            '        that has the same JoinedObjectId
            '        AND Set the RoleId of the PredicatePart to that Role's Id
            '  * Next (record in MetaModelPredicatePart)
            '--------------------------------------------------------------------------------------------------------------

            Dim lsSQLQuery As String = ""
            Dim lrRecordset As New ADODB.Recordset
            Dim lrRecordset1 As New ADODB.Recordset

            Try
                lrRecordset.ActiveConnection = pdbConnection
                lrRecordset.CursorType = ADODB.CursorTypeEnum.adOpenDynamic

                lrRecordset1.ActiveConnection = pdbConnection
                lrRecordset1.CursorType = pcOpenStatic

                lsSQLQuery = "SELECT * FROM MetaModelPredicatePart"

                lrRecordset.Open(lsSQLQuery, , ADODB.CursorTypeEnum.adOpenDynamic, ADODB.LockTypeEnum.adLockOptimistic)

                While Not lrRecordset.EOF

                    lsSQLQuery = "SELECT RoleId"
                    lsSQLQuery &= " FROM MetaModelRole"
                    lsSQLQuery &= " WHERE FactTypeId = (SELECT FactTypeId "
                    lsSQLQuery &= "                       FROM MetaModelFactTypeReading"
                    lsSQLQuery &= "                      WHERE FactTypeReadingId = '" & lrRecordset("FactTypeReadingId").Value & "'"
                    lsSQLQuery &= "                     )"
                    lsSQLQuery &= " AND (JoinsEntityTypeId = '" & lrRecordset("Symbol1").Value & "'"
                    lsSQLQuery &= "   OR JoinsValueTypeId = '" & lrRecordset("Symbol1").Value & "'"
                    lsSQLQuery &= "   OR JoinsNestedFactTypeId = '" & lrRecordset("Symbol1").Value & "'"
                    lsSQLQuery &= " )"

                    lrRecordset1.Open(lsSQLQuery)

                    lrRecordset("RoleId").Value = lrRecordset1("RoleId").Value

                    lrRecordset1.Close()

                    lrRecordset.Update()
                    lrRecordset.MoveNext()
                End While

                lrRecordset.Close()

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Public Sub InsertNewPredicatePartRecordsForRoleIds()

            Dim lsSQLQuery As String = ""
            Dim lrRecordset As New ADODB.Recordset
            Dim lrRecordset1 As New ADODB.Recordset

            Try
                lrRecordset.ActiveConnection = pdbConnection
                lrRecordset.CursorType = ADODB.CursorTypeEnum.adOpenDynamic

                lrRecordset1.ActiveConnection = pdbConnection
                lrRecordset1.CursorType = pcOpenStatic

                lsSQLQuery = "SELECT * FROM MetaModelPredicatePart"

                lrRecordset.Open(lsSQLQuery, , ADODB.CursorTypeEnum.adOpenDynamic, ADODB.LockTypeEnum.adLockOptimistic)

                While Not lrRecordset.EOF

                    lsSQLQuery = "SELECT RoleId"
                    lsSQLQuery &= " FROM MetaModelRole"
                    lsSQLQuery &= " WHERE FactTypeId = (SELECT FactTypeId "
                    lsSQLQuery &= "                       FROM MetaModelFactTypeReading"
                    lsSQLQuery &= "                      WHERE FactTypeReadingId = '" & lrRecordset("FactTypeReadingId").Value & "'"
                    lsSQLQuery &= "                     )"
                    lsSQLQuery &= " AND (JoinsEntityTypeId = '" & lrRecordset("Symbol2").Value & "'"
                    lsSQLQuery &= "   OR JoinsValueTypeId = '" & lrRecordset("Symbol2").Value & "'"
                    lsSQLQuery &= "   OR JoinsNestedFactTypeId = '" & lrRecordset("Symbol2").Value & "'"
                    lsSQLQuery &= " )"

                    lrRecordset1.Open(lsSQLQuery)

                    '----------------------------------------------
                    'Insert the new MetaModelPredicatePart record
                    '----------------------------------------------
                    If Trim(NullVal(lrRecordset("Symbol2").Value, "")) <> "" Then
                        lsSQLQuery = "INSERT INTO MetaModelPredicatePart"
                        lsSQLQuery &= " VALUES ("
                        lsSQLQuery &= " #" & Now & "#"
                        lsSQLQuery &= " ,#" & Now & "#"
                        lsSQLQuery &= " ,'" & Trim(lrRecordset("ModelId").Value) & "'"
                        lsSQLQuery &= " ,'" & Trim(lrRecordset("FactTypeReadingId").Value) & "'"
                        lsSQLQuery &= " ," & lrRecordset("SequenceNr").Value + 1
                        lsSQLQuery &= " ,'" & Trim(lrRecordset("Symbol2").Value) & "'"
                        lsSQLQuery &= " ,''" 'Symbol2
                        lsSQLQuery &= " ,''" 'PredicatePartText
                        lsSQLQuery &= " ,'" & Trim(lrRecordset1("RoleId").Value) & "'"
                        lsSQLQuery &= " ,''" 'PreBoundText
                        lsSQLQuery &= " ,''" 'PostBoundText
                        lsSQLQuery &= ")"

                        Call pdbConnection.Execute(lsSQLQuery)

                    End If

                    lrRecordset1.Close()

                    lrRecordset.MoveNext()
                End While

                lrRecordset.Close()

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try


        End Sub

        ''' <summary>
        ''' Sets the initial value of the GUID field on the MetaModelValueType table.
        ''' Database Version: 1.16
        ''' Boston Release: 2.6
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub AddValueTypeGUIDs()

            Dim lsSQLQuery As String = ""
            Dim lrRecordset As New ADODB.Recordset

            Try
                lrRecordset.ActiveConnection = pdbConnection
                lrRecordset.CursorType = ADODB.CursorTypeEnum.adOpenForwardOnly
                lrRecordset.LockType = ADODB.LockTypeEnum.adLockOptimistic

                lsSQLQuery = "SELECT * FROM MetaModelValueType"

                lrRecordset.Open(lsSQLQuery)

                Dim liInd As Integer = 0
                While Not lrRecordset.EOF
                    liInd += 1
                    lrRecordset("GUID").Value = "VT" & liInd.ToString
                    lrRecordset.Update()
                    lrRecordset.MoveNext()
                End While

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        ''' <summary>
        ''' Sets the initial value of the GUID field on the MetaModelFactType table.
        ''' Database Version: 1.16
        ''' Boston Release: 2.6
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub AddFactTypeGUIDs()

            Dim lsSQLQuery As String = ""
            Dim lrRecordset As New ADODB.Recordset
            Dim lrRecordset1 As New ADODB.Recordset

            Try
                lrRecordset.ActiveConnection = pdbConnection
                lrRecordset.CursorType = ADODB.CursorTypeEnum.adOpenForwardOnly
                lrRecordset.LockType = ADODB.LockTypeEnum.adLockOptimistic


                lsSQLQuery = "SELECT * FROM MetaModelFactType"

                lrRecordset.Open(lsSQLQuery)

                Dim liInd As Integer = 0
                While Not lrRecordset.EOF
                    liInd += 1
                    lrRecordset("GUID").Value = "FT" & liInd.ToString
                    lrRecordset.Update()
                    lrRecordset.MoveNext()
                End While

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        ''' <summary>
        ''' Sets the initial value of the GUID field on the MetaModelRoleConstraint table.
        ''' Database Version: 1.16
        ''' Boston Release: 2.6
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub AddRoleConstraintGUIDs()

            Dim lsSQLQuery As String = ""
            Dim lrRecordset As New ADODB.Recordset

            Try
                lrRecordset.ActiveConnection = pdbConnection
                lrRecordset.CursorType = ADODB.CursorTypeEnum.adOpenForwardOnly
                lrRecordset.LockType = ADODB.LockTypeEnum.adLockOptimistic


                lsSQLQuery = "SELECT * FROM MetaModelRoleConstraint"

                lrRecordset.Open(lsSQLQuery)

                Dim liInd As Integer = 0
                While Not lrRecordset.EOF
                    liInd += 1
                    lrRecordset("GUID").Value = "RC" & liInd.ToString
                    lrRecordset.Update()
                    lrRecordset.MoveNext()
                End While

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Public Sub SetInitialFactTypeReadingTypedPredicateIds()

            Try

                Dim lsSQLQuery As String = ""
                Dim lrRecordset As New ADODB.Recordset

                lrRecordset.ActiveConnection = pdbConnection
                lrRecordset.CursorType = ADODB.CursorTypeEnum.adOpenForwardOnly
                lrRecordset.LockType = ADODB.LockTypeEnum.adLockOptimistic

                lsSQLQuery = "SELECT * "
                lsSQLQuery &= " FROM MetaModelFactTypeReading"

                lrRecordset.Open(lsSQLQuery)

                While Not lrRecordset.EOF

                    lrRecordset("TypedPredicateId").Value = System.Guid.NewGuid.ToString
                    lrRecordset.Update()
                    lrRecordset.MoveNext()
                End While

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        ''' <summary>
        ''' For Boston v4.1; replaces the existing University model with a Core model distributed as a .fbm (XML) model.
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub ReplaceUniversityModel()

            Dim lrSerializer As XmlSerializer = Nothing
            Dim lrNewUniversityModel As New FBM.Model

            Try
                'Load the Core.fbm file from <Application Directory>/coremodel/Core.fbm
                Dim lsNewUniversityModelFile As String = Richmond.MyPath & "\coremodel\University.fbm"
                Dim objStreamReader As New StreamReader(lsNewUniversityModelFile)

                lrSerializer = New XmlSerializer(GetType(XMLModel.Model))
                Dim lrXMLModel As New XMLModel.Model
                lrXMLModel = lrSerializer.Deserialize(objStreamReader)
                objStreamReader.Close()

                lrNewUniversityModel = lrXMLModel.MapToFBMModel

                'Delete the existing University model from the database
                Dim lrUniversityModel As New FBM.Model("University", "3810f0c1-9253-4d44-ad7a-960cd273a225")
                Call lrUniversityModel.RemoveFromDatabase()

                Call lrNewUniversityModel.Save(True)

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Public Sub BackupDatabase()

            Try
                'Dim lsDBLocation As String = ""
                Dim liInd As Integer = 0
                'Dim larConnectionString() As String
                Dim lsNewDatabaseAndPath As String

                Dim lrSQLConnectionStringBuilder As New System.Data.Common.DbConnectionStringBuilder(True)
                lrSQLConnectionStringBuilder.ConnectionString = Trim(My.Settings.DatabaseConnectionString)

                Dim lsDatabaseLocation = lrSQLConnectionStringBuilder("Data Source")

                'larConnectionString = Split(My.Settings.DatabaseConnectionString, ";")

                'For liInd = 0 To larConnectionString.Length - 1
                '    lsDBLocation = larConnectionString(liInd)
                '    lsDBLocation = Trim(lsDBLocation)
                '    If lsDBLocation.StartsWith("Data Source=") Then
                '        lsDBLocation = lsDBLocation.Replace("Data Source=", "")
                '        Exit For
                '    End If
                'Next

                '------------------------------------------------
                'Define the location of database
                '------------------------------------------------
                lsNewDatabaseAndPath = Path.GetDirectoryName(lsDatabaseLocation)
                lsNewDatabaseAndPath = String.Format(lsNewDatabaseAndPath & "\Boston{0:yyyyMMddHHmmss}.mdb", Now)

                File.Copy(lsDatabaseLocation, lsNewDatabaseAndPath, True)

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        ''' <summary>
        ''' For Boston v4.1; replaces the existing Core model with a Core model distributed as a .fbm (XML) model.
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub ReplaceCoreModel()

            '--------------------------------------------------------------------------------
            'PSEUDOCODE
            '* Load the Core.fbm file from <Application Directory>/coremodel/Core.fbm
            '* Delete the Core model from the database
            '* Replace the in-memory Core model (prApplication.CMML.Core) with the newly loaded Core model.
            '* Save the in-memory Core model (prApplication.CMML.Core) to the database.
            '* Job done
            '* Now when Users load their existing Models they will be converted to the new Core model.
            '--------------------------------------------------------------------------------

            Dim lrSerializer As XmlSerializer = Nothing
            Dim lrNewCoreModel As New FBM.Model

            Try
                'Backup the database
                Call BackupDatabase()

                'Load the Core.fbm file from <Application Directory>/coremodel/Core.fbm
                Dim lsNewCoreModelFile As String = Richmond.MyPath & "\coremodel\Core.fbm"
                Dim objStreamReader As New StreamReader(lsNewCoreModelFile)

                lrSerializer = New XmlSerializer(GetType(XMLModel.Model))
                Dim lrXMLModel As New XMLModel.Model
                lrXMLModel = lrSerializer.Deserialize(objStreamReader)
                objStreamReader.Close()

                lrNewCoreModel = lrXMLModel.MapToFBMModel

                'Delete the existing Core model from the database
                Dim lrCoreModel As New FBM.Model("Core", "Core")
                Call TableModel.DeleteModel(lrCoreModel)

                'Replace the in-memory Core model (prApplication.CMML.Core) with the newly loaded Core model.
                prApplication.CMML.Core = lrNewCoreModel

                '* Save the in-memory Core model (prApplication.CMML.Core) to the database.
                Call prApplication.CMML.Core.Save()

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
