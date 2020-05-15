Imports System.ComponentModel
Imports System.Reflection
Imports System.Xml.Serialization

Namespace FBM
    <Serializable()> _
    Public Class FactTypeReading
        Inherits FBM.ModelObject
        Implements IEquatable(Of FBM.FactTypeReading)

        <XmlIgnore()> _
        Public _FactType As FBM.FactType
        <XmlIgnore()> _
        Public Overridable Property FactType() As FBM.FactType
            Get
                Return Me._FactType
            End Get
            Set(ByVal value As FBM.FactType)
                Me._FactType = value
            End Set
        End Property

        ''' <summary>
        ''' Used only in searching a set of FactTypeReadings for one that has a matching RoleList sequence.
        ''' </summary>
        ''' <remarks></remarks>
        Public _RoleList As New List(Of FBM.Role)
        Public Property RoleList As List(Of FBM.Role)
            Get
                If _RoleList.Count = 0 Then
                    Dim larRole As New List(Of FBM.Role)
                    Dim lrPredicatePart As FBM.PredicatePart
                    For Each lrPredicatePart In Me.PredicatePart
                        larRole.Add(lrPredicatePart.Role)
                    Next
                    Return larRole
                Else
                    Return Me._RoleList
                End If
            End Get
            Set(value As List(Of FBM.Role))
                Me._RoleList = value
            End Set
        End Property

        Public _PredicatePart As New List(Of FBM.PredicatePart)
        Public Overridable Property PredicatePart() As List(Of FBM.PredicatePart)
            Get
                Return Me._PredicatePart
            End Get
            Set(ByVal value As List(Of FBM.PredicatePart))
                Me._PredicatePart = value
            End Set
        End Property

        'New model
        Public FactTypeReadingRole As List(Of FBM.FactTypeReadingRole)


        Public FrontText As String = "" 'Text before the Predicate. e.g. "Sometimes" in "Sometimes Person drives CarType".
        Public FollowingText As String = "" 'Text after the last ModelElement of a binary or greater Fact Type. e.g. "sometimes" in "Person drives Car sometimes".


        Delegate Function MatchDelegate(ByVal arFactTypeReading As FBM.FactTypeReading) '20150201-No longer used. Can likely delete this line. Was referenced with...See 'matches' function below....but no reference in Matches.. methods found.

        Public Sub New()

            Me.ConceptType = pcenumConceptType.FactTypeReading

        End Sub

        Public Sub New(ByRef arFactType As FBM.FactType, Optional ByVal asFactTypeReadingId As String = "")

            Try
                Me.Model = arFactType.Model

                If asFactTypeReadingId = "" Then
                    Me.Id = System.Guid.NewGuid.ToString
                Else
                    Me.Id = asFactTypeReadingId
                End If

                Me.FactType = arFactType

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                Me.Model.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace, True)
            End Try

        End Sub

        Public Sub New(ByRef arFactType As FBM.FactType, ByRef aarRole As List(Of FBM.Role), ByVal aasPredicatePart As List(Of String))


            Me.New(arFactType)

            Try
                Dim liInd As Integer = 0
                Dim lsPredicatePart As String = ""
                Dim lrPredicatePart As FBM.PredicatePart

                For liInd = 1 To aarRole.Count
                    Me.RoleList.Add(aarRole(liInd - 1))
                    lrPredicatePart = New FBM.PredicatePart(Me.Model, Me)
                    lrPredicatePart.SequenceNr = liInd
                    lrPredicatePart.Role = aarRole(liInd - 1)
                    lrPredicatePart.PredicatePartText = aasPredicatePart(liInd - 1)
                    lrPredicatePart.PreBoundText = ""
                    lrPredicatePart.PostBoundText = ""
                    Me.PredicatePart.Add(lrPredicatePart)
                Next

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                Me.Model.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace, True)
            End Try


        End Sub

        Public Shadows Function Equals(ByVal other As FBM.FactTypeReading) As Boolean Implements System.IEquatable(Of FBM.FactTypeReading).Equals

            If Me.Id = other.Id Then
                Return True
            Else
                Return False
            End If

        End Function

        Public Function EqualsByRoleSequence(ByVal other As FBM.FactTypeReading) As Boolean

            Dim liInd As Integer = 0
            Dim lrPredicatePart As FBM.PredicatePart

            If Me.PredicatePart.Count <> other.PredicatePart.Count Then
                Return False
            End If

            For Each lrPredicatePart In Me.PredicatePart
                If lrPredicatePart.RoleId <> other.PredicatePart(liInd).RoleId Then
                    Return False
                End If
                liInd += 1
            Next

            Return True

        End Function

        Public Function EqualsByRoleJoinedModelObjectSequence(ByVal other As FBM.FactTypeReading) As Boolean

            Dim liInd As Integer = 0
            Dim lrRole As FBM.Role

            If Me.RoleList.Count <> other.RoleList.Count Then
                Return False
            End If

            For Each lrRole In Me.RoleList
                If lrRole.JoinedORMObject IsNot other.RoleList(liInd).JoinedORMObject Then
                    Return False
                End If
                liInd += 1
            Next

            Return True

        End Function

        ''' <summary>
        ''' RETURNS True if the PredicatePart/Role sequence matches that of the FactType's RoleGroup.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function MatchesByFactTypesRoles() As Boolean

            Dim liSequenceNr As Integer = 0
            Dim lrPredicatePart As New FBM.PredicatePart

            Try
                MatchesByFactTypesRoles = False

                For Each lrPredicatePart In Me.PredicatePart
                    If Me.PredicatePart(liSequenceNr).RoleId = Me.FactType.RoleGroup(liSequenceNr).Id Then
                        MatchesByFactTypesRoles = True
                    Else
                        Return False
                        Exit Function
                    End If
                    liSequenceNr += 1
                Next
            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                Me.Model.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace, True)
            End Try

        End Function



        Public Function MatchesByRoles(ByVal other As FBM.FactTypeReading) As Boolean

            Try
                Dim liSequenceNr As Integer = 0
                Dim lrPredicatePart As New FBM.PredicatePart

                MatchesByRoles = False

                If other.PredicatePart.Count <> Me.PredicatePart.Count Then
                    Return False
                    Exit Function
                End If

                For Each lrPredicatePart In other.PredicatePart
                    If Me.PredicatePart(liSequenceNr).RoleId = lrPredicatePart.RoleId Then
                        MatchesByRoles = True
                    Else
                        Return False
                        Exit Function
                    End If
                    liSequenceNr += 1
                Next

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                Me.Model.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace, True)
            End Try

        End Function

        ''' <summary>
        ''' Used to check (for Binary FactTypes) whether the FactTypeReading Role order matches that of the supplied RoleConstraint
        ''' PRECONDITIONS: The FactType for the reading must be a Binary FactType.
        ''' </summary>
        ''' <param name="arRoleConstraint"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function MatchesRoleConstraintRoleOrder(ByRef arRoleConstraint As FBM.RoleConstraint) As Boolean

            Dim liInd As Integer = 0

            Try
                MatchesRoleConstraintRoleOrder = True

                If Me.FactType.Arity <> 2 Then
                    Throw New Exception("This function is only to be used for FactTypeReadings that belong to a Binary FactType.")
                End If

                For liInd = 1 To arRoleConstraint.RoleConstraintRole.Count
                    If Me.RoleList(liInd - 1).Id <> arRoleConstraint.RoleConstraintRole(liInd - 1).Role.Id Then
                        MatchesRoleConstraintRoleOrder = False
                    End If
                Next

                Return MatchesRoleConstraintRoleOrder

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                Me.Model.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace, True)

                Return False
            End Try

        End Function

        '''' <summary>
        '''' VM-20160525-Removed as part of moving to v1.13 of the database structure.
        '''' </summary>
        '''' <param name="other"></param>
        '''' <returns>TRUE if
        '''' ELSE FALSE</returns>
        '''' <remarks></remarks>
        'Public Function MatchesByObjectTypeList(ByVal other As FBM.FactTypeReading) As Boolean

        '    Dim liSequenceNr As Integer = 0
        '    Dim lrPredicatePart As New FBM.PredicatePart
        '    Dim lsWord As String = ""

        '    MatchesByObjectTypeList = False

        '    If other.ObjectTypeList.Count <> Me.ObjectTypeList.Count Then
        '        Return False
        '        Exit Function
        '    End If

        '    '------------------------------------------------------------------------------
        '    'In a FactTypeReading we are only interested in the 
        '    '  order of the Symbols that represent the ORMObjectTypes (i.e. their names)
        '    '  not the actual ORMObjectTypes themselves.
        '    '  In this respect, we can dereference the ORMObjectTypes linked by the FactType
        '    '  and just match the FactTypeReading by the sorted list of Symbols representing
        '    '  the ORMObjectTypes.
        '    '------------------------------------------------------------------------------
        '    Dim lrORMObjectType As FBM.ModelObject
        '    Dim larObjectTypeList As New List(Of String)
        '    Dim larOtherObjectTypeList As New List(Of String)

        '    For Each lrORMObjectType In Me.ObjectTypeList
        '        larObjectTypeList.Add(lrORMObjectType.Name)
        '    Next

        '    For Each lrORMObjectType In other.ObjectTypeList
        '        larOtherObjectTypeList.Add(lrORMObjectType.Symbol)
        '    Next

        '    liSequenceNr = 0
        '    For Each lsWord In larOtherObjectTypeList
        '        If lsWord <> larObjectTypeList(liSequenceNr).ToString Then
        '            Return False
        '        End If
        '        liSequenceNr += 1
        '        MatchesByObjectTypeList = True 'Originally set to False. If gets to end of list and all elements the same, then Objects are equivalent.
        '    Next

        'End Function

        Public Sub AddPredicatePart(ByRef arPredicatePart As FBM.PredicatePart)

            arPredicatePart.SequenceNr = Me.PredicatePart.Count

            Me.PredicatePart.Add(arPredicatePart)

        End Sub

        Public Function GetPredicatePart(ByVal asRoleId As String) As FBM.PredicatePart

            Dim lrPredicatePart As New FBM.PredicatePart(Me.Model, Me)

            asRoleId = lrPredicatePart.RoleId

            Return Me.PredicatePart.Find(AddressOf lrPredicatePart.Equals)

        End Function

        Public Overloads Function Clone(ByRef arModel As FBM.Model, ByRef arFactType As FBM.FactType, Optional abAddToModel As Boolean = False) As FBM.FactTypeReading

            Dim lrPredicatePart As FBM.PredicatePart
            Dim lrFactTypeReading As New FBM.FactTypeReading

            Try
                With Me

                    lrFactTypeReading.Model = arModel
                    lrFactTypeReading.FactType = arFactType
                    lrFactTypeReading.Id = System.Guid.NewGuid.ToString
                    lrFactTypeReading.Name = lrFactTypeReading.Id
                    lrFactTypeReading.Symbol = lrFactTypeReading.Id

                    '------------------------------------------------------------------------------
                    'No Longer supported (v1.13 of the database Model).
                    'For Each lrObjectType In .ObjectTypeList
                    '    lrFactTypeReading.ObjectTypeList.Add(lrObjectType.Clone(arModel))
                    'Next

                    For Each lrPredicatePart In .PredicatePart
                        lrFactTypeReading.PredicatePart.Add(lrPredicatePart.Clone(arModel, lrFactTypeReading, abAddToModel))
                    Next

                End With

                Return lrFactTypeReading

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                Me.Model.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace, True)

                Return lrFactTypeReading
            End Try

        End Function

        Public Overrides Function CanSafelyRemoveFromModel() As Boolean
            Return False
        End Function

        Public Overridable Function GetDottedReadingText() As String

            Try
                '----------------------------------------------------
                'Create the dotted reading from the PredicateParts
                '----------------------------------------------------
                Dim liSequenceNr As Integer = 0
                Dim lrPredicatePart As FBM.PredicatePart

                If (Me.FactType.Arity = 2) And _
                   (Me.FactType.RoleGroup(0).Id <> Me.PredicatePart(0).RoleId) Then
                    GetDottedReadingText = Chr(171)
                    'chr(187) right '>>'
                    'chr(171) Left '<<'
                End If

                GetDottedReadingText = Me.FrontText & " "

                For Each lrPredicatePart In Me.PredicatePart
                    liSequenceNr += 1
                    GetDottedReadingText &= lrPredicatePart.PreBoundText
                    GetDottedReadingText &= "..."
                    GetDottedReadingText &= lrPredicatePart.PostBoundText
                    GetDottedReadingText &= Trim(lrPredicatePart.PredicatePartText)
                    If liSequenceNr >= 1 Then
                        GetDottedReadingText &= " "
                    End If
                Next

                GetDottedReadingText &= Me.FollowingText

                If Me.FactType.IsDerived Then
                    GetDottedReadingText &= " *"
                End If

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                Me.Model.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace, True)

                Return Nothing
            End Try

        End Function

        Public Function GetReadingText() As String

            '----------------------------------------------------
            'Create the dotted reading from the PredicateParts
            '----------------------------------------------------        
            Dim liSequenceNr As Integer = 0
            Dim lrPredicatePart As FBM.PredicatePart

            GetReadingText = ""

            GetReadingText &= Me.FrontText & " "

            For Each lrPredicatePart In Me.PredicatePart
                liSequenceNr += 1

                GetReadingText &= lrPredicatePart.PreBoundText
                GetReadingText &= lrPredicatePart.Role.JoinedORMObject.Symbol
                GetReadingText &= lrPredicatePart.PostBoundText
                If (liSequenceNr < Me.PredicatePart.Count) Or (lrPredicatePart.PredicatePartText <> "") Then
                    GetReadingText &= " "
                End If
                GetReadingText &= lrPredicatePart.PredicatePartText

                If liSequenceNr < Me.PredicatePart.Count Then
                    GetReadingText &= " "
                End If

            Next

            If Me.FollowingText <> "" Then
                GetReadingText &= " " & Me.FollowingText
            End If

        End Function

        ''' <summary>
        ''' Adds the FactTypeReading text of the FactTypeReading to the Verbaliser.
        ''' </summary>
        ''' <param name="arVerbaliser"></param>
        ''' <remarks></remarks>
        Public Sub GetReadingText(ByRef arVerbaliser As FBM.ORMVerbailser, Optional ByVal abIncludeRoleConstraintInformation As Boolean = False)

            '----------------------------------------------------
            'Create the dotted reading from the PredicateParts
            '----------------------------------------------------        
            Dim liSequenceNr As Integer = 0
            Dim lrPredicatePart As FBM.PredicatePart

            arVerbaliser.VerbalisePredicateText(Me.FrontText)

            For Each lrPredicatePart In Me.PredicatePart
                liSequenceNr += 1

                arVerbaliser.VerbalisePredicateText(lrPredicatePart.PreBoundText)
                arVerbaliser.VerbaliseModelObject(lrPredicatePart.Role.JoinedORMObject)
                arVerbaliser.VerbalisePredicateText(lrPredicatePart.PostBoundText)
                If (liSequenceNr < Me.PredicatePart.Count) Or (lrPredicatePart.PredicatePartText <> "") Then
                    arVerbaliser.HTW.Write(" ")
                End If

                arVerbaliser.VerbalisePredicateText(lrPredicatePart.PredicatePartText)

                If (liSequenceNr = 1) And (Me.FactType.Arity = 2) And abIncludeRoleConstraintInformation Then
                    If Me.FactType.IsManyTo1BinaryFactType Or Me.FactType.Is1To1BinaryFactType Then '  InternalUniquenessConstraint.Count = 1 Then
                        If Me.MatchesRoleConstraintRoleOrder(Me.FactType.InternalUniquenessConstraint(0)) Then
                            If Me.FactType.GetRoleById(Me.RoleList(0).Id).Mandatory = True Then
                                arVerbaliser.VerbaliseQuantifierLight(" one ")
                            Else
                                arVerbaliser.VerbaliseQuantifierLight(" at most one ")
                            End If
                        Else
                            If Me.FactType.GetRoleById(Me.RoleList(0).Id).Mandatory = True Then
                                arVerbaliser.VerbaliseQuantifierLight(" one ")
                            Else
                                arVerbaliser.VerbaliseQuantifierLight(" at most one ")
                            End If
                        End If
                    End If
                End If

                If liSequenceNr < Me.PredicatePart.Count Then
                    arVerbaliser.HTW.Write(" ")
                End If

            Next

            If Me.FollowingText <> "" Then
                arVerbaliser.HTW.Write(" ")
                arVerbaliser.VerbalisePredicateText(Me.FollowingText)
            End If

        End Sub

        ''' <summary>
        ''' Returns the ReadingText but with either 'that' or 'some' in front of the ObjectType names.
        ''' </summary>
        ''' <param name="aarRole">The set of Roles that determines whether 'that' or 'some' precedes the respective ObjectType's name.</param>
        ''' <param name="abDropFirstRole">If TRUE, then the first Role/ObjectType.Name is dropped and replaced by 'that'</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetReadingTextThatOrSome(ByVal aarRole As List(Of FBM.Role), _
                                                 ByRef aarArgumentCommonModelObjects As List(Of FBM.ModelObject), _
                                                 Optional ByVal abDropFirstRole As Boolean = False) As String

            '----------------------------------------------------
            'Create the dotted reading from the PredicateParts
            '----------------------------------------------------        
            Dim liSequenceNr As Integer = 0
            Dim lrPredicatePart As FBM.PredicatePart
            Dim lsReturnText As String = ""

            lsReturnText &= Me.FrontText

            For Each lrPredicatePart In Me.PredicatePart
                liSequenceNr += 1

                If (lrPredicatePart.Role.Id = aarRole(liSequenceNr - 1).Id) Or (abDropFirstRole And (liSequenceNr = 1)) Then
                    lsReturnText &= "that "
                Else
                    lsReturnText &= "some "
                End If

                If abDropFirstRole And (liSequenceNr = 1) Then
                Else
                    lsReturnText &= lrPredicatePart.PreBoundText
                    lsReturnText &= lrPredicatePart.Role.JoinedORMObject.Symbol
                    lsReturnText &= lrPredicatePart.PostBoundText
                End If

                If ((liSequenceNr < Me.PredicatePart.Count) Or _
                   (lrPredicatePart.PredicatePartText <> "")) And _
                   (lsReturnText(lsReturnText.Length - 1) <> " ") Then
                    lsReturnText &= " "
                End If

                lsReturnText &= lrPredicatePart.PredicatePartText

                If liSequenceNr < Me.PredicatePart.Count Then
                    lsReturnText &= " "
                End If
            Next

            If Me.FollowingText <> "" Then
                lsReturnText &= " " & Me.FollowingText
            End If

            Return lsReturnText

        End Function

        ''' <summary>
        ''' Returns the ReadingText but with either 'that' or 'some' in front of the ObjectType names.
        ''' </summary>
        ''' <param name="aarRole">The set of Roles that determines whether 'that' or 'some' precedes the respective ObjectType's name.</param>
        ''' <param name="abDropFirstRole">If TRUE, then the first Role/ObjectType.Name is dropped and replaced by 'that'</param>
        ''' <remarks></remarks>
        Public Sub GetReadingTextThatOrSome(ByVal aarRole As List(Of FBM.Role), _
                                            ByRef arVerbaliser As FBM.ORMVerbailser, _
                                            ByRef aarArgumentCommonModelObjects As List(Of FBM.ModelObject), _
                                            Optional ByVal abDropFirstRole As Boolean = False)

            Try
                '----------------------------------------------------
                'Create the dotted reading from the PredicateParts
                '----------------------------------------------------        
                Dim liSequenceNr As Integer = 0
                Dim lrPredicatePart As FBM.PredicatePart

                arVerbaliser.VerbaliseQuantifier(Me.FrontText)

                For Each lrPredicatePart In Me.PredicatePart
                    liSequenceNr += 1

                    If (abDropFirstRole And (liSequenceNr = 1)) Then
                        arVerbaliser.VerbaliseQuantifier("that ")

                        'arVerbaliser.VerbalisePredicateText(lrPredicatePart.PreBoundText)
                        'arVerbaliser.VerbaliseModelObject(lrPredicatePart.Role.JoinedORMObject)
                        'arVerbaliser.VerbalisePredicateText(lrPredicatePart.PostBoundText)

                    ElseIf aarRole.Contains(lrPredicatePart.Role) Then
                        If aarArgumentCommonModelObjects.FindAll(Function(x) x.Model.ModelObjectIsSubtypeOfModelObject(lrPredicatePart.Role.JoinedORMObject, x) = True).Count > 0 Then
                            arVerbaliser.VerbaliseQuantifier("some ")

                            arVerbaliser.VerbalisePredicateText(lrPredicatePart.PreBoundText)
                            arVerbaliser.VerbaliseModelObject(lrPredicatePart.Role.JoinedORMObject)
                            arVerbaliser.VerbalisePredicateText(lrPredicatePart.PostBoundText)

                            arVerbaliser.VerbaliseQuantifier(" that is that ")
                            arVerbaliser.VerbaliseModelObject(aarArgumentCommonModelObjects.Find(Function(x) Me.Model.ModelObjectIsSubtypeOfModelObject(lrPredicatePart.Role.JoinedORMObject, x) = True))
                        Else
                            arVerbaliser.VerbaliseQuantifier("that ")

                            arVerbaliser.VerbalisePredicateText(lrPredicatePart.PreBoundText)
                            arVerbaliser.VerbaliseModelObject(lrPredicatePart.Role.JoinedORMObject)
                            arVerbaliser.VerbalisePredicateText(lrPredicatePart.PostBoundText)
                        End If
                    Else
                        arVerbaliser.VerbaliseQuantifier("some ")

                        arVerbaliser.VerbalisePredicateText(lrPredicatePart.PreBoundText)
                        arVerbaliser.VerbaliseModelObject(lrPredicatePart.Role.JoinedORMObject)
                        arVerbaliser.VerbalisePredicateText(lrPredicatePart.PostBoundText)
                    End If

                    If ((liSequenceNr < Me.PredicatePart.Count) Or _
                       (lrPredicatePart.PredicatePartText <> "")) And _
                       (Not (abDropFirstRole And (liSequenceNr = 1))) Then
                        arVerbaliser.HTW.Write(" ")
                    End If

                    arVerbaliser.VerbalisePredicateText(lrPredicatePart.PredicatePartText)

                    If liSequenceNr < Me.PredicatePart.Count Then
                        arVerbaliser.HTW.Write(" ")
                    End If
                Next

                If Me.FollowingText <> "" Then
                    arVerbaliser.VerbalisePredicateText(" " & Me.FollowingText)
                End If

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                Me.Model.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace, True)
            End Try

        End Sub

        ''' <summary>
        ''' Removes PredicatePart/s from FactTypeReading for an ObjectType joined to a Role/s of the FactType of the FactTypeReading.
        ''' When a user removes a Role from a FactType, there is no option but to remove all 
        ''' PredicateParts (in FactTypeReadings for the FactType) that contain the ObjectType
        ''' joined by the Role being removed.
        ''' </summary>
        ''' <param name="arRole">The Role for which the associated PredicatePart will be removed</param>
        ''' <remarks></remarks>
        Public Overridable Sub RemovePredicatePartForRole(ByRef arRole As FBM.Role)

            Dim lrPredicatePart As FBM.PredicatePart
            Dim lrPredicatePartInd As FBM.PredicatePart
            Dim lrRoleToBeRemoved As FBM.Role

            Try
                lrRoleToBeRemoved = arRole

                lrPredicatePart = Me.PredicatePart.Find(Function(x) x.Role.Id = lrRoleToBeRemoved.Id)
                Dim liSequenceNrBeingRemoved As Integer = lrPredicatePart.SequenceNr

                '--------------------------------------------------------------
                'Remove the last PredicatePart for the FTR from the database.
                '--------------------------------------------------------------
                lrPredicatePart.SequenceNr = Me.PredicatePart.Count

                Me.PredicatePart.Remove(lrPredicatePart)

                For Each lrPredicatePartInd In Me.PredicatePart.FindAll(Function(x) x.SequenceNr >= liSequenceNrBeingRemoved)
                    lrPredicatePartInd.SequenceNr -= 1
                Next

                '---------------------------------------------------------------------------------------------------------
                'Make sure that the last PredicatePart of the FactTypeReading has a "" (empty string) PredicatePartText.
                '---------------------------------------------------------------------------------------------------------
                Me.PredicatePart(Me.PredicatePart.Count - 1).PredicatePartText = ""

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                Me.Model.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace, True)
            End Try


        End Sub

        Public Overrides Function RemoveFromModel(Optional ByVal abForceRemoval As Boolean = False, _
                                                  Optional ByVal abCheckForErrors As Boolean = True) As Boolean

            Try
                Call Me.FactType.RemoveFactTypeReading(Me)

                Return True

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                Me.Model.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace, True)

                Return False
            End Try

        End Function

        ''' <summary>
        ''' Prototype for the Save function.
        ''' </summary>
        ''' <remarks></remarks>
        Public Overridable Overloads Sub Save()
        End Sub

    End Class
End Namespace