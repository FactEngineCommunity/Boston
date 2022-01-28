Imports System.Reflection

Namespace TableFactTypeInstance

    Public Module Table_FactTypeInstance

        Public Sub AddFactTypeInstance(ByVal arFactTypeInstance As FBM.FactTypeInstance)

            Dim lrConceptInstance As New FBM.ConceptInstance(arFactTypeInstance.Model, arFactTypeInstance.Page, pcenumConceptType.FactType)

            lrConceptInstance.Symbol = arFactTypeInstance.Id
            lrConceptInstance.X = arFactTypeInstance.X
            lrConceptInstance.Y = arFactTypeInstance.Y
            lrConceptInstance.Orientation = 101
            lrConceptInstance.Visible = True
            lrConceptInstance.ConceptType = pcenumConceptType.FactType

            TableConceptInstance.AddConceptInstance(lrConceptInstance)

        End Sub

        Public Sub DeleteFactTypeInstance(ByVal arFactTypeInstance As FBM.FactTypeInstance)

            Try
                Dim lsSQLQuery As String = ""

                lsSQLQuery = "DELETE FROM ModelConceptInstance"
                lsSQLQuery &= " WHERE PageId = '" & Trim(arFactTypeInstance.Page.PageId) & "'"
                lsSQLQuery &= "   AND Symbol = '" & Trim(arFactTypeInstance.Id) & "'"
                lsSQLQuery &= "   AND ConceptType = '" & pcenumConceptType.FactType.ToString & "'"

                pdbConnection.BeginTrans()
                Call pdbConnection.Execute(lsSQLQuery)
                pdbConnection.CommitTrans()

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Public Function ExistsFactTypeInstance(ByVal arFactTypeInstance As FBM.FactTypeInstance) As Boolean

            Dim lsSQLQuery As String = ""
            Dim lREcordset As New ADODB.Recordset

            lREcordset.ActiveConnection = pdbConnection
            lREcordset.CursorType = pcOpenStatic

            lsSQLQuery = " SELECT COUNT(*)"
            lsSQLQuery &= "  FROM ModelConceptInstance"
            lsSQLQuery &= " WHERE PageId = '" & Trim(arFactTypeInstance.Page.PageId) & "'"
            lsSQLQuery &= "   AND Symbol = '" & Trim(arFactTypeInstance.Id) & "'"
            lsSQLQuery &= "   AND ConceptType = '" & pcenumConceptType.FactType.ToString & "'"

            lREcordset.Open(lsSQLQuery)

            If lREcordset(0).Value > 0 Then
                ExistsFactTypeInstance = True
            Else
                ExistsFactTypeInstance = False
            End If

            lREcordset.Close()

        End Function

        Public Function GetFactTypeInstanceCountByPage(ByVal ar_page As FBM.Page) As Integer

            '-------------------------------------------------------------------------------------------------
            'NB EnterpriseInstances never manifest within the database, they manifest within the database as
            '  SymbolInstances, so we must count the SymbolInstances that match the arguments.
            '-------------------------------------------------------------------------------------------------

            Dim lsSQLQuery As String = ""
            Dim lREcordset As New ADODB.Recordset

            lREcordset.ActiveConnection = pdbConnection
            lREcordset.CursorType = pcOpenStatic

            GetFactTypeInstanceCountByPage = 0

            lsSQLQuery = " SELECT COUNT(*)"
            lsSQLQuery &= "  FROM ModelConceptInstance ci,"
            lsSQLQuery &= "       MetaModelFactType ft"
            lsSQLQuery &= " WHERE ft.FactTypeId = ci.Symbol"
            lsSQLQuery &= "   AND ci.PageId = '" & Trim(ar_page.PageId) & "'"
            lsSQLQuery &= "   AND ci.ConceptType = '" & pcenumConceptType.FactType.ToString & "'"

            lREcordset.Open(lsSQLQuery)

            GetFactTypeInstanceCountByPage = lREcordset(0).Value

            lREcordset.Close()

        End Function

        ''' <summary>
        '''   NB Adds the FactTypeInstance to the Page.
        ''' </summary>
        ''' <param name="asFactTypeInstanceId"></param>
        ''' <param name="arPage"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetFactTypeInstanceByPage(ByVal asFactTypeInstanceId As String, ByRef arPage As FBM.Page) As FBM.FactTypeInstance

            Dim lrRoleInstance As FBM.RoleInstance
            Dim lsMessage As String = ""
            Dim lsSQLQuery As String = ""
            Dim lREcordset As New ADODB.Recordset
            Dim lrRole As FBM.Role
            Dim lrFactTypeInstance As FBM.FactTypeInstance

            Try

                '-----------------------------
                'Initialise the return value
                '-----------------------------
                lrFactTypeInstance = New FBM.FactTypeInstance

                lREcordset.ActiveConnection = pdbConnection
                lREcordset.CursorType = pcOpenStatic

                lsSQLQuery = " SELECT ft.*, ci.*"
                lsSQLQuery &= "  FROM MetaModelFactType ft,"
                lsSQLQuery &= "       ModelConceptInstance ci"
                lsSQLQuery &= " WHERE ft.ModelId = '" & Trim(arPage.Model.ModelId) & "'"
                lsSQLQuery &= "   AND ci.ModelId = '" & Trim(arPage.Model.ModelId) & "'"
                lsSQLQuery &= "   AND ci.PageId = '" & Trim(arPage.PageId) & "'"
                lsSQLQuery &= "   AND ci.Symbol = '" & asFactTypeInstanceId & "'"
                lsSQLQuery &= "   AND ci.ConceptType = '" & pcenumConceptType.FactType.ToString & "'"
                lsSQLQuery &= "   AND ci.Symbol = ft.FactTypeId"

                lREcordset.Open(lsSQLQuery)

                If Not lREcordset.EOF Then
                    While Not lREcordset.EOF

                        lrFactTypeInstance.Id = asFactTypeInstanceId
                        lrFactTypeInstance.Name = asFactTypeInstanceId 'Trim(lREcordset("FactTypeName").Value)
                        lrFactTypeInstance.Model = arPage.Model
                        lrFactTypeInstance.Page = arPage
                        lrFactTypeInstance.X = lREcordset("x").Value
                        lrFactTypeInstance.Y = lREcordset("y").Value
                        '2022-01-28-VM-Moved to below...now get from FactType.
                        'lrFactTypeInstance.IsDerived = CBool(lREcordset("IsDerived").Value)
                        'lrFactTypeInstance.IsStored = CBool(lREcordset("IsStored").Value)
                        'lrFactTypeInstance.IsIndependent = CBool(lREcordset("IsIndependent").Value)
                        'lrFactTypeInstance.IsSubtypeStateControlling = CBool(lREcordset("IsSubtypeStateControlling").Value)
                        'lrFactTypeInstance.DerivationText = Trim(NullVal(lREcordset("DerivationText").Value, ""))
                        'lrFactTypeInstance.IsLinkFactType = CBool(lREcordset("IsLinkFactType").Value)
                        'If lrFactTypeInstance.IsLinkFactType Then
                        '    lrFactTypeInstance.LinkFactTypeRole = arPage.RoleInstance.Find(Function(x) x.Id = NullVal(lREcordset("LinkFactTypeRoleId").Value, ""))
                        'End If

                        If arPage.FactTypeInstance.Exists(AddressOf lrFactTypeInstance.Equals) Then
                            '-------------------------------------------------------------------------------------------------------
                            'The FactTypeInstance has already been added to the Page by the recursive loading of FactTypeInstances
                            '-------------------------------------------------------------------------------------------------------
                            Dim lrExistingFactTypeInstance As FBM.FactTypeInstance
                            lrExistingFactTypeInstance = arPage.FactTypeInstance.Find(AddressOf lrFactTypeInstance.Equals)
                            lrExistingFactTypeInstance.X = lrFactTypeInstance.X
                            lrExistingFactTypeInstance.Y = lrFactTypeInstance.Y

                            For Each lrRoleInstance In lrExistingFactTypeInstance.RoleGroup
                                lrRoleInstance.RoleName = New FBM.RoleName(lrRoleInstance, lrRoleInstance.Name)
                                Call TableRoleName.GetRoleNameDetails(lrRoleInstance.RoleName)
                            Next

                            Return lrExistingFactTypeInstance
                        Else
                            lrFactTypeInstance.FactType = arPage.Model.FactType.Find(Function(x) x.Id = lrFactTypeInstance.Id)

                            lrFactTypeInstance.ShortDescription = lrFactTypeInstance.FactType.ShortDescription
                            lrFactTypeInstance.LongDescription = lrFactTypeInstance.FactType.LongDescription
                            lrFactTypeInstance.DBName = lrFactTypeInstance.FactType.DBName

                            lrFactTypeInstance.IsDerived = lrFactTypeInstance.FactType.IsDerived
                            lrFactTypeInstance.IsStored = lrFactTypeInstance.FactType.IsStored
                            lrFactTypeInstance.IsIndependent = lrFactTypeInstance.FactType.IsIndependent
                            lrFactTypeInstance.IsSubtypeStateControlling = lrFactTypeInstance.FactType.IsSubtypeStateControlling
                            lrFactTypeInstance.DerivationText = lrFactTypeInstance.FactType.DerivationText
                            lrFactTypeInstance.IsLinkFactType = lrFactTypeInstance.FactType.IsLinkFactType
                            If lrFactTypeInstance.IsLinkFactType Then
                                lrFactTypeInstance.LinkFactTypeRole = arPage.RoleInstance.Find(Function(x) x.Id = NullVal(lREcordset("LinkFactTypeRoleId").Value, ""))
                            End If

                            If lrFactTypeInstance.FactType Is Nothing Then
                                Throw New Exception("Cannot find FactType for FactTypeInstance with FactTypeId: " & lrFactTypeInstance.Id)
                            End If


                            lrFactTypeInstance.IsObjectified = lrFactTypeInstance.FactType.IsObjectified
                            lrFactTypeInstance.ShowFactTypeName = lrFactTypeInstance.IsObjectified

                            If lrFactTypeInstance.IsObjectified Then
                                Dim lrObjectifyingEntityTypeInstance As FBM.EntityTypeInstance

                                If IsSomething(lrFactTypeInstance.FactType.ObjectifyingEntityType) Then
                                    lrObjectifyingEntityTypeInstance = arPage.EntityTypeInstance.Find(Function(x) x.Id = lrFactTypeInstance.FactType.ObjectifyingEntityType.Id)
                                    If IsSomething(lrObjectifyingEntityTypeInstance) Then
                                        '---------------------------------------------
                                        'All okay. Found the EntityType on the Page.
                                        '---------------------------------------------
                                    Else
                                        lrObjectifyingEntityTypeInstance = lrFactTypeInstance.FactType.ObjectifyingEntityType.CloneInstance(arPage, True)
                                    End If
                                    lrObjectifyingEntityTypeInstance.IsObjectifyingEntityType = True
                                    lrObjectifyingEntityTypeInstance.ObjectifiedFactType = New FBM.FactTypeInstance
                                    lrObjectifyingEntityTypeInstance.ObjectifiedFactType = lrFactTypeInstance
                                    'lrFactTypeInstance.ObjectifyingEntityType = New FBM.EntityTypeInstance
                                    lrFactTypeInstance.ObjectifyingEntityType = lrObjectifyingEntityTypeInstance
                                Else
                                    lrFactTypeInstance.ObjectifyingEntityType = Nothing
                                End If
                            End If

                            lrFactTypeInstance.isPreferredReferenceMode = lrFactTypeInstance.FactType.IsPreferredReferenceMode
                            lrFactTypeInstance.IsSubtypeRelationshipFactType = lrFactTypeInstance.FactType.IsSubtypeRelationshipFactType

                            '-----------------------------------------
                            'Setup the FactTypeName for the FactType
                            '-----------------------------------------
                            lrFactTypeInstance.FactTypeName = New FBM.FactTypeName(arPage.Model, arPage, lrFactTypeInstance, lrFactTypeInstance.Name)

                            '==============================================================================================================
                            Call TableFactTypeName.GetFactTypeNameDetails(lrFactTypeInstance.FactTypeName)

                            If lrFactTypeInstance.DerivationText <> "" Then
                                lrFactTypeInstance.FactTypeDerivationText = New FBM.FactTypeDerivationText(arPage.Model,
                                                                                                        arPage,
                                                                                                        lrFactTypeInstance)
                                Call TableFactTypeDerivationText.GetFactTypeDerivationTextDetails(lrFactTypeInstance.FactTypeDerivationText)
                            End If

                            '-------------------------------------------------------------
                            'The Model must already have been loaded before loading the 
                            '  Page, so get the RoleGroup/RoleInstances from the model.
                            '-------------------------------------------------------------                        
                            For Each lrRole In lrFactTypeInstance.FactType.RoleGroup

                                lrRoleInstance = lrRole.CloneInstance(arPage, True)

                                'prApplication.ThrowErrorMessage("Loading Page:'" & arPage.Name & "' AND RoleInstance.Id:'" & lrRoleInstance.Id & "'", pcenumErrorType.Information)

                                lrRoleInstance.FactType = lrFactTypeInstance

                                If lrRoleInstance.TypeOfJoin = pcenumRoleJoinType.FactType Then
                                    If lrRoleInstance.JoinedORMObject Is Nothing Then
                                        lrRoleInstance.JoinedORMObject = TableFactTypeInstance.GetFactTypeInstanceByPage(lrRoleInstance.Role.JoinsFactType.Id, arPage)

                                        'Load facts from the database, because cloning the RoleInstance above loads the FactTypeInstance, but not the Facts.
                                        '20210826-VM-Can't see why this doesn't double up the FactInstances, because GetFactTypeInstanceByPage (above) should get the Fact Instances.
                                        lrRoleInstance.JoinsFactType.GetFactInstancesFromDatabase()
                                    End If
                                End If

                                '-----------------------------------------------------------------------------------------------
                                'CodeSafe; Make sure the RoleInstance is connected to something.
                                If lrRoleInstance.JoinedORMObject Is Nothing Then
                                    Select Case lrRoleInstance.TypeOfJoin
                                        Case Is = pcenumRoleJoinType.EntityType
                                            lrRoleInstance.JoinedORMObject = arPage.EntityTypeInstance.Find(Function(x) x.Id = lrRoleInstance.JoinsEntityType.Id)
                                        Case Is = pcenumRoleJoinType.ValueType
                                            lsMessage = "Cannot find ValueType for"
                                            lsMessage &= vbCrLf & "FactType.Id: " & lrFactTypeInstance.FactType.Id
                                            lsMessage &= vbCrLf & "FactType.Name: " & lrFactTypeInstance.FactType.Name
                                            lsMessage &= vbCrLf & " Role.Id: " & lrRoleInstance.Id
                                            lsMessage &= vbCrLf & " Page.Id: " & arPage.PageId
                                            lsMessage &= vbCrLf & " Page.Name: " & arPage.Name
                                            lsMessage &= vbCrLf & " ValueTypeId: (looking for) '" & lrRole.JoinedORMObject.Id & "'"
                                            If arPage.Model.GetModelObjectByName(lrRole.JoinedORMObject.Id) IsNot Nothing Then
                                                Call arPage.DropValueTypeAtPoint(lrRole.JoinedORMObject, New PointF(50, 50), True)
                                            Else
                                                Throw New Exception(lsMessage)
                                            End If
                                            lrRoleInstance.JoinedORMObject = arPage.ValueTypeInstance.Find(Function(x) x.Id = lrRoleInstance.JoinsValueType.Id)
                                        Case Is = pcenumRoleJoinType.FactType
                                            '---------------------------------------------------------------------------------------------------
                                            'See Below. Do not throw exception here because the FactTypeInstance may not have been loaded yet.
                                            '---------------------------------------------------------------------------------------------------
                                            lrRoleInstance.JoinedORMObject = TableFactTypeInstance.GetFactTypeInstanceByPage(lrRoleInstance.Role.JoinsFactType.Id, arPage)
                                            'Load facts from the database, because cloning the RoleInstance above loads the FactTypeInstance, but not the Factsf
                                            lrRoleInstance.JoinsFactType.GetFactInstancesFromDatabase()
                                    End Select
                                End If

                                '-----------------------------------------
                                'Setup the RoleName for the RoleInstance
                                '-----------------------------------------
                                lrRoleInstance.RoleName = New FBM.RoleName(lrRoleInstance, lrRoleInstance.Name)

                                Call TableRoleName.GetRoleNameDetails(lrRoleInstance.RoleName)

                                '--------------------------------------------------------------
                                'Add the RoleInstance to the RoleGroup of the FactTypeInstance
                                '--------------------------------------------------------------
                                lrFactTypeInstance.RoleGroup.Add(lrRoleInstance)

                                '----------------------------------------------------
                                'Add the RoleInstance to the Page.RoleInstance group
                                '----------------------------------------------------
                                'SyncLock arPage.RoleInstance                                    
                                arPage.RoleInstance.AddUnique(lrRoleInstance)
                                'End SyncLock

                                'prApplication.ThrowErrorMessage("Successfully loaded Page:'" & lrFactTypeInstance.Page.Name & "' AND RoleInstance.Id:'" & lrRoleInstance.Id & "'", pcenumErrorType.Information)
                            Next

                            '----------------------------------------------
                            'Setup the FactTable for the FactTypeInstance
                            '----------------------------------------------
                            lrFactTypeInstance.FactTable = New FBM.FactTable(arPage, lrFactTypeInstance)
                            Call TableFactTableInstance.GetFactTableDetails(lrFactTypeInstance.FactTable)

                            If lrFactTypeInstance.IsSubtypeRelationshipFactType Then
                                Dim lrEntityTypeInstance As FBM.EntityTypeInstance
                                Dim lrParentEntityTypeInstance As FBM.EntityTypeInstance
                                lrEntityTypeInstance = lrFactTypeInstance.RoleGroup(0).JoinsEntityType
                                lrParentEntityTypeInstance = lrFactTypeInstance.RoleGroup(1).JoinsEntityType

                                Dim lrSubtypeConstraintInstance As New FBM.SubtypeRelationshipInstance
                                lrSubtypeConstraintInstance.EntityType = lrEntityTypeInstance
                                lrSubtypeConstraintInstance.parentEntityType = lrParentEntityTypeInstance
                                lrFactTypeInstance.SubtypeConstraintInstance = lrEntityTypeInstance.SubtypeRelationship.Find(AddressOf lrSubtypeConstraintInstance.Equals)
                            End If

                            '----------------------------------------------
                            'Get the Facts (FactTypeData) for the FactType
                            '----------------------------------------------
                            If lrFactTypeInstance.FactType.Fact.Count > 0 Then
                                'Only get FactInstances if the FactTypeInstance's FactType has Facts.
                                Call tableORMFactInstance.GetFactsForFactTypeInstance(lrFactTypeInstance)
                            End If
                            'new threading below
                            'Dim loFactInstanceThread As New System.Threading.Thread(AddressOf lrFactTypeInstance.GetFactInstancesFromDatabase)
                            'loFactInstanceThread.Start()

                            'SyncLock arPage.FactTypeInstance
                            arPage.FactTypeInstance.AddUnique(lrFactTypeInstance)
                            'End SyncLock
                        End If

                        lREcordset.MoveNext()
                    End While

                    lREcordset.Close()
                Else
                    Return Nothing
                    Exit Function
                End If

                Return lrFactTypeInstance

            Catch ex As Exception
                lsMessage = "Error: TableFactTypeInstance.lrFactTypeInstance"
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)

                Return Nothing
            End Try

        End Function

        Function GetFactTypeInstancesByPage(ByRef arPage As FBM.Page) As List(Of FBM.FactTypeInstance)

            Dim lrFactTypeInstance As FBM.FactTypeInstance
            Dim lrRoleInstance As FBM.RoleInstance
            'Dim lrRole As FBM.Role
            Dim lsSQLQuery As String = ""
            Dim lREcordset As New ADODB.Recordset

            lREcordset.ActiveConnection = pdbConnection
            lREcordset.CursorType = pcOpenStatic

            lsSQLQuery = " SELECT ft.*, ci.*"
            lsSQLQuery &= "  FROM MetaModelFactType ft,"
            lsSQLQuery &= "       ModelConceptInstance ci"
            lsSQLQuery &= " WHERE ci.PageId = '" & Trim(arPage.PageId) & "'"
            lsSQLQuery &= "   AND ci.ModelId = '" & Trim(arPage.Model.ModelId) & "'"
            lsSQLQuery &= "   AND ci.Symbol = ft.FactTypeId"
            lsSQLQuery &= "   AND ci.ConceptType = '" & pcenumConceptType.FactType.ToString & "'"
            lsSQLQuery &= "   AND ft.ModelId = '" & Trim(arPage.Model.ModelId) & "'"
            lsSQLQuery &= " ORDER BY FT.IsObjectified ASC"

            lREcordset.Open(lsSQLQuery)

            '-----------------------------
            'Initialise the return value
            '-----------------------------
            GetFactTypeInstancesByPage = New List(Of FBM.FactTypeInstance)

            Try

                If Not lREcordset.EOF Then
                    While Not lREcordset.EOF
                        lrFactTypeInstance = TableFactTypeInstance.GetFactTypeInstanceByPage(Trim(lREcordset("FactTypeId").Value), arPage)

                        '-----------------------------------------------------------------------------------
                        'CodeSafe: Only add the FactTypeInstance to the Page if it was successfully loaded
                        '-----------------------------------------------------------------------------------
                        If lrFactTypeInstance IsNot Nothing Then
                            GetFactTypeInstancesByPage.Add(lrFactTypeInstance)
                            'prApplication.ThrowErrorMessage("Successfully loaded (Page:'" & lrFactTypeInstance.Page.Name & "' AND FactTypeInstance.Id:'" & lrFactTypeInstance.Id & "')", pcenumErrorType.Information)
                        End If

                        lREcordset.MoveNext()
                    End While

                    lREcordset.Close()

                    '--------------------------------------------------------------
                    'Go through each of the Roles in each of the FactTypes,
                    '  and for those Roles that join a FactType, make
                    '  sure that the JoinedORMObject is populated for the Role.
                    '
                    'The reason that this is required is because the (Joined) FactType
                    '  for a Role may not have been loaded at the time the Role
                    '  was loaded, so the 'find' function will have returned 'Nothing'
                    '--------------------------------------------------------------                                        
                    For Each lrFactTypeInstance In GetFactTypeInstancesByPage.FindAll(Function(x) x.RoleGroup.FindAll(Function(y) y.TypeOfJoin = pcenumRoleJoinType.FactType And y.JoinedORMObject Is Nothing).Count > 0).ToArray
                        For Each lrRoleInstance In lrFactTypeInstance.RoleGroup.FindAll(Function(p) p.TypeOfJoin = pcenumRoleJoinType.FactType And p.JoinedORMObject Is Nothing)
                            lrRoleInstance.JoinedORMObject = Nothing
                            lrRoleInstance.JoinedORMObject = arPage.FactTypeInstance.Find(Function(x) x.Id = lrRoleInstance.Role.JoinsFactType.Id)
                            If lrRoleInstance.JoinedORMObject Is Nothing Then
                                '-------------------------------------------------------------------
                                'Try and load the FactType if it is in the Model.
                                lrFactTypeInstance = arPage.DropFactTypeAtPoint(lrRoleInstance.Role.JoinsFactType, New PointF(0, 0), False, False, True)
                                lrRoleInstance.JoinedORMObject = lrFactTypeInstance
                                GetFactTypeInstancesByPage.Add(lrFactTypeInstance)
                            End If
                            If lrRoleInstance.JoinedORMObject Is Nothing Then
                                Throw New Exception("GetFactTypeInstancesByPage: Cannot find FactTypeInstance for RoleId: " & lrRoleInstance.Id & ", PageName: " & arPage.Name)
                            End If
                        Next
                        '---------------------------------------------------
                        'Do the same for the Facts of the FactTypeInstance
                        '---------------------------------------------------
                        For Each lrFactInstance In lrFactTypeInstance.Fact
                            For Each lrFactDataInstance In lrFactInstance.Data.FindAll(Function(x) x.Role.TypeOfJoin = pcenumRoleJoinType.FactType And x.JoinedObjectType Is Nothing)
                                lrFactDataInstance.JoinedObjectType = GetFactTypeInstancesByPage.Find(Function(x) x.Id = lrFactDataInstance.Role.JoinedORMObject.Id)
                            Next
                        Next
                    Next
                Else
                    GetFactTypeInstancesByPage = Nothing
                End If

            Catch ex As Exception
                Dim lsMessage As String
                lsMessage = "Error: GetFactTypeInstancesByPage:"
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Function

        Public Sub ModifyKey(ByVal arFactTypeInstance As FBM.FactTypeInstance, ByVal as_new_key As String)

            Dim lsSQLQuery As String = ""

            Try
                lsSQLQuery = " UPDATE ModelConceptInstance"
                lsSQLQuery &= "   SET Symbol = '" & Replace(Trim(as_new_key), "'", "`") & "'"
                lsSQLQuery &= " WHERE PageId = '" & Trim(arFactTypeInstance.Page.PageId) & "'"
                lsSQLQuery &= "   AND Symbol = '" & Replace(arFactTypeInstance.Id, "'", "`") & "'"
                lsSQLQuery &= "   AND ConceptType = '" & pcenumConceptType.FactType.ToString & "'"

                pdbConnection.BeginTrans()
                pdbConnection.Execute(lsSQLQuery)
                pdbConnection.CommitTrans()

            Catch ex As Exception
                Dim lsMessage As String
                lsMessage = "Error: TableFactTypeInstance.ModifyKey"
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                lsMessage &= vbCrLf & vbCrLf & "SQL: " & lsSQLQuery
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Public Sub update_FactTypeInstance(ByVal arFactTypeInstance As FBM.FactTypeInstance)

            Dim lsSQLQuery As String = ""

            Try
                lsSQLQuery = " UPDATE ModelConceptInstance"
                lsSQLQuery &= "   SET Orientation = " & arFactTypeInstance.FactTypeOrientation
                lsSQLQuery &= "       ,x = " & arFactTypeInstance.X
                lsSQLQuery &= "       ,y = " & arFactTypeInstance.Y
                lsSQLQuery &= " WHERE ModelId = '" & Trim(arFactTypeInstance.Model.ModelId) & "'"
                lsSQLQuery &= "   AND PageId = '" & Trim(arFactTypeInstance.Page.PageId) & "'"
                lsSQLQuery &= "   AND Symbol = '" & Replace(Trim(arFactTypeInstance.Id), "'", "`") & "'"
                lsSQLQuery &= "   AND ConceptType = '" & pcenumConceptType.FactType.ToString & "'"

                pdbConnection.BeginTrans()
                pdbConnection.Execute(lsSQLQuery)
                pdbConnection.CommitTrans()

            Catch ex As Exception
                Dim lsMessage As String
                lsMessage = "Error: TableFactTypeInstance.UpdateFactTypeInstance"
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                lsMessage &= vbCrLf & vbCrLf & "SQL: " & lsSQLQuery
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub


    End Module
End Namespace