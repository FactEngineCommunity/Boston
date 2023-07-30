Imports System.ComponentModel
Imports System.Reflection
Imports System.Xml.Serialization
Imports Gios.Word
Imports Newtonsoft.Json

Namespace FBM
    <Serializable()> _
    Public Class FactTypeReading
        Inherits FBM.ModelObject
        Implements IEquatable(Of FBM.FactTypeReading)

        <XmlAttribute()>
        Public Shadows ConceptType As pcenumConceptType = pcenumConceptType.FactTypeReading

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
        <JsonIgnore()>
        Public _RoleList As New List(Of FBM.Role)

        <JsonIgnore()>
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

        <JsonIgnore()>
        Public Overridable Property PredicatePart() As List(Of FBM.PredicatePart)
            Get
                Return Me._PredicatePart
            End Get
            Set(ByVal value As List(Of FBM.PredicatePart))
                Me._PredicatePart = value
            End Set
        End Property

        <JsonIgnore()>
        Public ReadOnly Property ModelObjects() As List(Of FBM.ModelObject)
            Get
                Return Me.PredicatePart.Select(Function(x) x.Role.JoinedORMObject).ToList
            End Get
        End Property

        'New model
        Public FactTypeReadingRole As List(Of FBM.FactTypeReadingRole)


        Public FrontText As String = "" 'Text before the Predicate. e.g. "Sometimes" in "Sometimes Person drives CarType".
        Public FollowingText As String = "" 'Text after the last ModelElement of a binary or greater Fact Type. e.g. "sometimes" in "Person drives Car sometimes".

        ''' <summary>
        ''' The Id of the TypedPredicate to which the FactTypeReading belongs. A TypedPredicate is an ordered set of Roles of a FactType.
        ''' The order of the Roles is always the same as the FactTypeReading. There may be more than one FactTypeReading with the same TypedPredicateId
        ''' </summary>
        ''' <remarks></remarks>
        Public TypedPredicateId As String = System.Guid.NewGuid.ToString

        ''' <summary>
        ''' True if the FactTypeReading is preferred for the FactType that it belongs to.
        ''' </summary>
        ''' <remarks></remarks>
        Public IsPreferred As Boolean = False

        ''' <summary>
        ''' If there is more than one FactTypeReading for a FactType with the same TypedPredicateId,
        ''' one of them may be 'Preferred'.
        ''' </summary>
        ''' <remarks></remarks>
        Public IsPreferredForPredicate As Boolean = True

        Public ReverseFactTypeReading As FBM.FactTypeReading = Nothing 'Used (at this stage) only when in the ORM Reading Editor, AutoComplete....to add a forward reading and a reverse reading in one go. See AutoComplete and frmToolboxORMReadingEditor.

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
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
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
                    lrPredicatePart = New FBM.PredicatePart(Me.Model, Me, Nothing, True)
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
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try


        End Sub

        Public Sub New(ByRef arFactType As FBM.FactType, ByRef aarRole As List(Of FBM.Role), ByVal arSentence As Language.Sentence)

            Me.New(arFactType)

            Try
                Dim liInd As Integer = 0
                Dim lsPredicatePart As String = ""
                Dim lrPredicatePart As FBM.PredicatePart

                Me.FrontText = arSentence.FrontText
                Me.FollowingText = arSentence.FollowingText

                For liInd = 1 To aarRole.Count
                    Me.RoleList.Add(aarRole(liInd - 1))
                    lrPredicatePart = New FBM.PredicatePart(Me.Model, Me, Nothing, True)
                    lrPredicatePart.SequenceNr = liInd
                    lrPredicatePart.Role = aarRole(liInd - 1)
                    If liInd = 1 And Me.FactType.Arity = 1 Then
                        'Unary FactType
                        lrPredicatePart.PredicatePartText = arSentence.PredicatePart(liInd - 1).PredicatePartText
                        lrPredicatePart.PreBoundText = ""
                        lrPredicatePart.PostBoundText = ""
                    ElseIf liInd = aarRole.Count Then
                        lrPredicatePart.PredicatePartText = ""
                        lrPredicatePart.PreBoundText = arSentence.PredicatePart(liInd - 1).PreboundText
                        lrPredicatePart.PostBoundText = arSentence.PredicatePart(liInd - 1).PostboundText
                        '20210814-VM-Was the below. Not sure why. "Street has ONE nick-Name" definitely has a PreboundReadingText.
                        'lrPredicatePart.PreBoundText = ""
                        'lrPredicatePart.PostBoundText = ""
                    Else
                        lrPredicatePart.PredicatePartText = arSentence.PredicatePart(liInd - 1).PredicatePartText
                        lrPredicatePart.PreBoundText = arSentence.PredicatePart(liInd - 1).PreboundText
                        lrPredicatePart.PostBoundText = arSentence.PredicatePart(liInd - 1).PostboundText
                    End If
                    Me.PredicatePart.Add(lrPredicatePart)
                Next

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
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

        Public Function EqualsByPredicatePartText(ByVal other As FBM.FactTypeReading,
                                                  Optional abUseFastenshtein As Boolean = False) As Boolean

            Dim liInd As Integer = 0

            Try
                If Me.PredicatePart.Count <> other.PredicatePart.Count Then
                    Return False
                End If

                For Each lrPredicatePart In Me.PredicatePart
                    If abUseFastenshtein Then
                        If (Fastenshtein.Levenshtein.Distance(lrPredicatePart.PredicatePartText, other.PredicatePart(liInd).PredicatePartText) >= 4) Or
                         (lrPredicatePart.PreBoundText <> other.PredicatePart(liInd).PreBoundText) Or
                         (lrPredicatePart.PostBoundText <> other.PredicatePart(liInd).PostBoundText) Then
                            Return False
                        End If
                    Else
                        If (lrPredicatePart.PredicatePartText <> other.PredicatePart(liInd).PredicatePartText) Or
                         (lrPredicatePart.PreBoundText <> other.PredicatePart(liInd).PreBoundText) Or
                         (lrPredicatePart.PostBoundText <> other.PredicatePart(liInd).PostBoundText) Then
                            Return False
                        End If
                    End If
                    liInd += 1
                Next

                Return True

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)

                Return False
            End Try

        End Function

        Public Function EqualsByPredicatePartTextModelElements(ByVal other As FBM.FactTypeReading,
                                                               Optional abUseFastenshtein As Boolean = False) As Boolean

            Dim liInd As Integer = 0

            Try
                If Me.PredicatePart.Count <> other.PredicatePart.Count Then
                    Return False
                End If

                For Each lrPredicatePart In Me.PredicatePart

                    If other.PredicatePart(liInd).Role.JoinedORMObject Is Nothing Then
                        Return False
                    ElseIf lrPredicatePart.Role.JoinedORMObject.Id <> other.PredicatePart(liInd).Role.JoinedORMObject.Id Then
                        Return False
                    End If

                    If abUseFastenshtein Then
                        If (Fastenshtein.Levenshtein.Distance(lrPredicatePart.PredicatePartText, other.PredicatePart(liInd).PredicatePartText) > 4) Or
                         (lrPredicatePart.PreBoundText <> other.PredicatePart(liInd).PreBoundText) Or
                         (lrPredicatePart.PostBoundText <> other.PredicatePart(liInd).PostBoundText) Then
                            Return False
                        End If
                    Else
                        If (lrPredicatePart.PredicatePartText <> other.PredicatePart(liInd).PredicatePartText) Or
                         (lrPredicatePart.PreBoundText <> other.PredicatePart(liInd).PreBoundText) Or
                         (lrPredicatePart.PostBoundText <> other.PredicatePart(liInd).PostBoundText) Then
                            Return False
                        End If
                    End If
                    liInd += 1
                Next

                Return True

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)

                Return False
            End Try

        End Function

        ''' <summary>
        ''' Used in FactEngine queries, to find FTRs for partial matches within a larger FT.Arity and FTR.
        ''' E.g. "Person visited (Country:'China')" within a larger ternary FT
        ''' "Person visited (Country:'China') within the last 10 months"
        ''' </summary>
        ''' <param name="other"></param>
        ''' <returns></returns>
        Public Function EqualsPartiallyByPredicatePartText(ByVal other As FBM.FactTypeReading,
                                                           Optional ByVal abUseFastenstein As Boolean = False) As Boolean

            Dim liInd As Integer = 0

            For Each lrPredicatePart In other.PredicatePart
                If abUseFastenstein Then
                    If (Fastenshtein.Levenshtein.Distance(lrPredicatePart.PredicatePartText, Me.PredicatePart(0).PredicatePartText) < 4) And
                         (lrPredicatePart.PreBoundText = Me.PredicatePart(0).PreBoundText) And
                         (lrPredicatePart.PostBoundText = Me.PredicatePart(0).PostBoundText) Then
                        'Only need one match for this to function to return True.
                        Return True
                    End If
                Else
                    If (lrPredicatePart.PredicatePartText = Me.PredicatePart(0).PredicatePartText) And
                         (lrPredicatePart.PreBoundText = Me.PredicatePart(0).PreBoundText) And
                         (lrPredicatePart.PostBoundText = Me.PredicatePart(0).PostBoundText) Then
                        'Only need one match for this to function to return True.
                        Return True
                    End If
                End If

                liInd += 1
            Next

            Return False

        End Function


        Public Function EqualsByRoleJoinedModelObjectSequence(ByVal other As FBM.FactTypeReading) As Boolean

            Dim liInd As Integer = 0
            Dim lrRole As FBM.Role

            If Me.RoleList.Count <> other.RoleList.Count Then
                Return False
            End If

            For Each lrRole In Me.RoleList
                If lrRole.JoinedORMObject.Id <> other.RoleList(liInd).JoinedORMObject.Id Then
                    Return False
                End If
                liInd += 1
            Next

            Return True

        End Function

        ''' <summary>
        ''' For QueryEdges in FactEngine queries where the QueryEdge is part of a larger FactType.
        ''' E.g. Where the FactType is ternary, but the QueryEdge is binary and where all QueryEdges are binary.
        ''' Other can be longer than Me
        ''' </summary>
        ''' <param name="other"></param>
        ''' <returns></returns>
        Public Function EqualsPartiallyByRoleJoinedModelObjectSequence(ByVal other As FBM.FactTypeReading) As Boolean

            Dim liMatchPosition = 0

            'NB Other can be longer than Me
            Try
                For Each lrPredicatePart In other.PredicatePart

                    If lrPredicatePart.Role.JoinedORMObject Is Me.RoleList(liMatchPosition).JoinedORMObject Then
                        liMatchPosition += 1
                        If lrPredicatePart.SequenceNr < other.PredicatePart.Count Then
                            If other.PredicatePart(lrPredicatePart.SequenceNr).Role.JoinedORMObject.Id = Me.RoleList(liMatchPosition).JoinedORMObject.Id Then
                                Return True
                            End If
                        End If
                    End If

                    liMatchPosition = 0
                Next

            Catch ex As Exception
                Return False
            End Try

            Return False

        End Function

        ''' <summary>
        ''' RETURNS True if the PredicatePart/Role sequence matches that of the FactType's RoleGroup.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function MatchesByFactTypesRoles(ByVal other As FBM.FactTypeReading) As Boolean

            Dim liSequenceNr As Integer = 0
            Dim lrPredicatePart As New FBM.PredicatePart

            Try
                MatchesByFactTypesRoles = False

                For Each lrPredicatePart In other.PredicatePart
                    If other.PredicatePart(liSequenceNr).RoleId = Me.FactType.RoleGroup(liSequenceNr).Id Then
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
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Function

        Public Function MatchesByRoles(ByVal other As FBM.FactTypeReading) As Boolean

            Try
                Dim liSequenceNr As Integer = 0
                Dim lrPredicatePart As New FBM.PredicatePart

                MatchesByRoles = False

                If other.PredicatePart.Count <> Me.PredicatePart.Count Then

                    For Each lrPredicatePart In other.PredicatePart.FindAll(Function(x) x.SequenceNr <= Me.PredicatePart.Count)
                        If Me.PredicatePart(liSequenceNr).RoleId = lrPredicatePart.RoleId Then
                            MatchesByRoles = True
                        Else
                            Return False
                            Exit Function
                        End If
                        liSequenceNr += 1
                    Next

                    'Return False
                    'Exit Function
                Else
                    For Each lrPredicatePart In other.PredicatePart
                        If Me.PredicatePart(liSequenceNr).RoleId = lrPredicatePart.RoleId Then
                            MatchesByRoles = True
                        Else
                            Return False
                            'Exit Function
                        End If
                        liSequenceNr += 1
                    Next
                End If

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
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
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)

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

            arPredicatePart.isDirty = True
            arPredicatePart.SequenceNr = Me.PredicatePart.Count + 1

            Call Me.ReSequencePredicateParts

            Me.makeDirty()
            Me.FactType.makeDirty()

            Me.PredicatePart.Add(arPredicatePart)

        End Sub

        Public Sub ReSequencePredicateParts()

            Try
                Me.PredicatePart = Me.PredicatePart.OrderBy(Function(x) x.SequenceNr)

                Dim liInd As Integer = 1
                For Each lrPredicatePart In Me.PredicatePart
                    lrPredicatePart.SequenceNr = liInd
                    liInd += 1
                Next

            Catch ex As Exception

            End Try
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
                    lrFactTypeReading.isDirty = True

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
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)

                Return lrFactTypeReading
            End Try

        End Function

        Public Shadows Function CloneInstance(ByRef arPage As FBM.Page, Optional arFactTypeInstance As FBM.FactTypeInstance = Nothing) As FBM.FactTypeReadingInstance

            Try
                Dim lrFactTypeReadingInstance As New FBM.FactTypeReadingInstance
                Dim lrFactTypeInstance As FBM.FactTypeInstance = arFactTypeInstance

                With Me
                    lrFactTypeReadingInstance.Model = arPage.Model
                    lrFactTypeReadingInstance.Page = arPage
                    lrFactTypeReadingInstance.Id = .Id
                    lrFactTypeReadingInstance.FactTypeReading = Me
                    lrFactTypeReadingInstance.FrontText = .FrontText
                    lrFactTypeReadingInstance.FollowingText = .FollowingText
                    lrFactTypeReadingInstance.PredicatePart = .PredicatePart
                    If lrFactTypeInstance Is Nothing Then
                        lrFactTypeInstance = arPage.FactTypeInstance.Find(Function(x) x.Id = Me.FactType.Id)
                    End If
                    lrFactTypeReadingInstance.FactType = lrFactTypeInstance
                End With

                Return lrFactTypeReadingInstance

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)

                Return Nothing
            End Try

        End Function

        Public Overrides Function CanSafelyRemoveFromModel() As Boolean
            Return False
        End Function

        ''' <summary>
        ''' Changes the Model of the FactTypeReading to the target Model.
        ''' </summary>
        ''' <param name="arTargetModel">The Model to which the FactTypeReading will be associated on completion of this method.</param>
        ''' <remarks></remarks>
        Public Shadows Sub ChangeModel(ByRef arTargetModel As FBM.Model)

            Me.Model = arTargetModel

            For Each lrPredicatePart In Me.PredicatePart
                lrPredicatePart.Model = arTargetModel
            Next

        End Sub

        Public Function CreateFactTypeReadingSentence(ByRef aarRole As List(Of FBM.Role), ByVal aasPredicate As List(Of String)) As Language.Sentence

            Try
                Dim liInd = 1
                Dim lsSentence As String = ""

                Dim lrSentence As New Language.Sentence("", "")

                lrSentence.FrontText = ""
                lrSentence.FollowingText = ""
                Dim lsPredicatePart As String = ""

                For Each lrRole As FBM.Role In aarRole

                    lsSentence &= lrRole.JoinedORMObject.Id

                    Dim lrPredicatePart As New Language.PredicatePart

                    lrPredicatePart.PreboundText = ""
                    lrPredicatePart.PostboundText = ""
                    lrPredicatePart.ObjectName = lrRole.JoinedORMObject.Id
                    lrSentence.ModelElement.Add(lrPredicatePart.ObjectName)

                    For Each lsPredicatePartPart In aasPredicate
                        lsPredicatePart &= lsPredicatePartPart
                    Next

                    lsSentence &= " " & lsPredicatePart & " "

                    lrPredicatePart.PredicatePartText = lsPredicatePart
                    lrPredicatePart.SequenceNr = liInd
                    lrSentence.PredicatePart.Add(lrPredicatePart)

                    liInd += 1
                Next

                lsSentence = Trim(lsSentence)

                lrSentence.Sentence = lsSentence
                lrSentence.OriginalSentence = lsSentence

                Return lrSentence

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)

                Return Nothing
            End Try

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
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)

                Return Nothing
            End Try

        End Function

        Public Function GetReadingText(Optional ByRef abIncludeCardinality As Boolean = False) As String

            '----------------------------------------------------
            'Create the dotted reading from the PredicateParts
            '----------------------------------------------------        
            Dim liSequenceNr As Integer = 0
            Dim lrPredicatePart As FBM.PredicatePart

            Try
                GetReadingText = ""

                GetReadingText &= Me.FrontText & " "

                For Each lrPredicatePart In Me.PredicatePart
                    liSequenceNr += 1

                    GetReadingText &= lrPredicatePart.PreBoundText
                    Try
                        GetReadingText &= lrPredicatePart.Role.JoinedORMObject.Id
                    Catch
                        'Can happen when loading a NORMA file for instance.
                        GetReadingText &= "[Missing Model Element]"
                    End Try

                    GetReadingText &= lrPredicatePart.PostBoundText
                    If (liSequenceNr < Me.PredicatePart.Count) Or (lrPredicatePart.PredicatePartText <> "") Then
                        GetReadingText &= " "
                    End If

                    GetReadingText &= lrPredicatePart.PredicatePartText


#Region "Cardinality"
                    If abIncludeCardinality Then

                        If liSequenceNr = 1 Then
                            If Me.FactType.Arity = 2 Then

                                Dim lrOtherRoleOfRoleGroup As FBM.Role = Me.FactType.GetOtherRoleOfBinaryFactType(lrPredicatePart.Role.Id)

                                If Me.FactType.HasTotalRoleConstraint Then
                                    'do nothing
                                Else
                                    If lrPredicatePart.Role.HasInternalUniquenessConstraint Then
                                        If lrPredicatePart.Role.Mandatory And Not lrOtherRoleOfRoleGroup.HasInternalUniquenessConstraint Then
                                            GetReadingText &= " ONE "
                                        Else
                                            GetReadingText &= " AT MOST ONE"
                                        End If
                                    ElseIf lrOtherRoleOfRoleGroup.HasInternalUniquenessConstraint Then
                                        GetReadingText &= " AT LEAST ONE "
                                    End If
                                End If
                            End If
                        End If
                    End If
#End Region

                    If liSequenceNr < Me.PredicatePart.Count Then
                        GetReadingText &= " "
                    End If

                Next

                If Me.FollowingText <> "" Then
                    GetReadingText &= " " & Me.FollowingText
                End If

                GetReadingText = Trim(GetReadingText)

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)

                Return "Error"
            End Try

        End Function

        Public Function GetPredicateText() As String

            '----------------------------------------------------
            'Create the dotted reading from the PredicateParts
            '----------------------------------------------------        
            Dim liSequenceNr As Integer = 0
            Dim lrPredicatePart As FBM.PredicatePart

            Try
                GetPredicateText = ""
                GetPredicateText &= Me.FrontText & " "

                For Each lrPredicatePart In Me.PredicatePart
                    liSequenceNr += 1

                    If (liSequenceNr < Me.PredicatePart.Count) Or (lrPredicatePart.PredicatePartText <> "") Then
                        GetPredicateText &= " "
                    End If
                    GetPredicateText &= lrPredicatePart.PredicatePartText

                    If liSequenceNr < Me.PredicatePart.Count Then
                        GetPredicateText &= " "
                    End If

                Next

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)

                Return "Error"
            End Try

        End Function

        ''' <summary>
        ''' Adds the FactTypeReading text of the FactTypeReading to the Verbaliser.
        ''' </summary>
        ''' <param name="arVerbaliser"></param>
        ''' <param name="arSubscriptPredicateRole">The Role to provide a Subcript number for. See parameter aiPredicateSubscriptNumber</param>
        ''' <param name="aiPredicateSubscriptNumber">The subscript number for the arSubscriptPredicateRole parameter</param>
        ''' <remarks></remarks>
        Public Sub GetReadingText(ByRef arVerbaliser As FBM.ORMVerbailser,
                                  Optional ByVal abIncludeRoleConstraintInformation As Boolean = False,
                                  Optional ByVal abIncludeSubscriptModelObjectNumbers As Boolean = False,
                                  Optional ByVal abThrowErrorToVerbaliser As Boolean = True,
                                  Optional ByVal arSubscriptPredicateRole As FBM.Role = Nothing,
                                  Optional ByVal aiPredicateSubscriptNumber As Integer = 0)

            Try
                '----------------------------------------------------
                'Create the dotted reading from the PredicateParts
                '----------------------------------------------------        
                Dim liSequenceNr As Integer = 0
                Dim lrPredicatePart As FBM.PredicatePart

                arVerbaliser.VerbalisePredicateText(Me.FrontText)

                If abIncludeSubscriptModelObjectNumbers Then
                    If Not Me.FactType.IsLinkFactType Then
                        If Me.FactType.HasMoreThanOneRoleReferencingTheSameModelObject() Then
                            Dim lrModelObjectCountDictionary As New Dictionary(Of String, Integer)
                            Call Me.FactType.GetReferencedModelObjectCounts(lrModelObjectCountDictionary)

                            For Each lrPredicatePart In Me.PredicatePart.AsEnumerable.Reverse
                                If lrModelObjectCountDictionary.ContainsKey(lrPredicatePart.Role.JoinedORMObject.Id) Then
                                    lrPredicatePart.ModelObjectSubscriptNumber = lrModelObjectCountDictionary.Item(lrPredicatePart.Role.JoinedORMObject.Id)
                                    lrModelObjectCountDictionary.Item(lrPredicatePart.Role.JoinedORMObject.Id) -= 1
                                End If
                            Next
                        End If
                    End If
                End If

                For Each lrPredicatePart In Me.PredicatePart

                    liSequenceNr += 1

                    Dim liIndex As Integer
                    Dim lsPredicatePartText As String = lrPredicatePart.PredicatePartText
                    Dim lsEndingAdjectives As String = Boston.ExtractLastAdjectiveFromSentence(lsPredicatePartText, liIndex)
                    If lsEndingAdjectives IsNot Nothing Then
                        lsPredicatePartText = lsPredicatePartText.Substring(0, liIndex)
                    End If

                    arVerbaliser.VerbalisePredicateText(lrPredicatePart.PreBoundText)

                    If lrPredicatePart.Role.JoinedORMObject IsNot Nothing Then
                        arVerbaliser.VerbaliseModelObject(lrPredicatePart.Role.JoinedORMObject)
                    End If

                    If Me.FactType.IsLinkFactType Then
                        Dim larRole = From Role In Me.FactType.LinkFactTypeRole.FactType.RoleGroup
                                      Where Role.JoinedORMObject.Id = lrPredicatePart.Role.JoinedORMObject.Id
                                      Select Role

                        If larRole.Count > 0 Then
                            If Me.FactType.LinkFactTypeRole.FactType.FactTypeReading.Count > 0 Then
                                lrPredicatePart.ModelObjectSubscriptNumber =
                                    Me.FactType.LinkFactTypeRole.FactType.FactTypeReading(0).PredicatePart.Find(Function(x) x.Role.Id = lrPredicatePart.Role.FactType.LinkFactTypeRole.Id).ModelObjectSubscriptNumber
                            End If
                        End If
                    End If

                    If arSubscriptPredicateRole IsNot Nothing Then
                        If arSubscriptPredicateRole Is lrPredicatePart.Role Then
                            arVerbaliser.VerbaliseSubscript(aiPredicateSubscriptNumber.ToString)
                        End If
                    Else
                        If lrPredicatePart.ModelObjectSubscriptNumber > 0 Then
                            arVerbaliser.VerbaliseSubscript(lrPredicatePart.ModelObjectSubscriptNumber)
                        End If
                    End If


                    arVerbaliser.VerbalisePredicateText(lrPredicatePart.PostBoundText)
                    If (liSequenceNr < Me.PredicatePart.Count) Or (lsPredicatePartText <> "") Then
                        arVerbaliser.HTW.Write(" ")
                    End If

                    If (liSequenceNr = 1) And (Me.FactType.Arity = 2) And abIncludeRoleConstraintInformation Then
                        If Me.FactType.IsManyTo1BinaryFactType Then
                            If Me.MatchesRoleConstraintRoleOrder(Me.FactType.InternalUniquenessConstraint(0)) Then

                                arVerbaliser.VerbalisePredicateText(lsPredicatePartText)

                                If Me.FactType.GetRoleById(Me.RoleList(0).Id).Mandatory = True Then
                                    arVerbaliser.VerbaliseQuantifierLight(" one ")
                                Else
                                    arVerbaliser.VerbaliseQuantifierLight(" at most one ")
                                End If
                            Else
                                If Me.FactType.GetRoleById(Me.RoleList(0).Id).Mandatory = True Then
                                    arVerbaliser.VerbalisePredicateText(lsPredicatePartText)

                                    arVerbaliser.VerbaliseQuantifierLight(" more than one ")
                                Else
                                    arVerbaliser.VerbaliseQuantifierLight(" possibly ")
                                    arVerbaliser.VerbalisePredicateText(lsPredicatePartText)
                                    arVerbaliser.VerbaliseQuantifierLight(" more than one ")

                                End If
                            End If
                        ElseIf Me.FactType.Is1To1BinaryFactType Then '  InternalUniquenessConstraint.Count = 1 Then..

                            arVerbaliser.VerbalisePredicateText(lsPredicatePartText)

                            If Me.MatchesRoleConstraintRoleOrder(Me.FactType.InternalUniquenessConstraint(0)) Then
                                If Me.FactType.GetRoleById(Me.RoleList(0).Id).Mandatory = True Then
                                    arVerbaliser.VerbaliseQuantifierLight(" one ")
                                Else
                                    arVerbaliser.VerbaliseQuantifierLight(" one ")
                                End If
                            Else
                                If Me.FactType.GetRoleById(Me.RoleList(0).Id).Mandatory = True Then
                                    arVerbaliser.VerbaliseQuantifierLight(" one ")
                                Else
                                    arVerbaliser.VerbaliseQuantifierLight(" one ")
                                End If
                            End If
                        Else
                            arVerbaliser.VerbalisePredicateText(lsPredicatePartText)
                        End If
                    Else
                        arVerbaliser.VerbalisePredicateText(lsPredicatePartText)
                    End If

                    If lsEndingAdjectives IsNot Nothing Then
                        arVerbaliser.VerbalisePredicateText(lsEndingAdjectives)
                    End If

                    If liSequenceNr < Me.PredicatePart.Count Then
                        arVerbaliser.HTW.Write(" ")
                    End If

                Next

                If Me.FollowingText <> "" Then
                    arVerbaliser.HTW.Write(" ")
                    arVerbaliser.VerbalisePredicateText(Me.FollowingText)
                End If

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message

                If abThrowErrorToVerbaliser Then
                    arVerbaliser.VerbaliseError(lsMessage)
                Else
                    prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
                End If

            End Try

        End Sub

        ''' <summary>
        ''' Adds the FactTypeReading text of the FactTypeReading to the Word Document verbaliser.
        ''' </summary>
        ''' <param name="arWordDocumentVerbaliser">The Object for the Gios.Word.WordDocument document being written to.</param>
        ''' <remarks></remarks>
        Public Sub GetReadingText(ByRef arWordDocumentVerbaliser As FBM.ORMWordVerbailser, _
                                  Optional ByVal abIncludeRoleConstraintInformation As Boolean = False, _
                                  Optional ByVal abIncludeSubscriptModelObjectNumbers As Boolean = False, _
                                  Optional ByVal abIncludeOFTIsWhereClause As Boolean = False)

            Try
                '----------------------------------------------------
                'Create the dotted reading from the PredicateParts
                '----------------------------------------------------        
                Dim liSequenceNr As Integer = 0
                Dim lrPredicatePart As FBM.PredicatePart

                If Me.FactType.IsObjectified And abIncludeOFTIsWhereClause Then
                    arWordDocumentVerbaliser.VerbaliseModelObject(Me.FactType)
                    arWordDocumentVerbaliser.VerbaliseQuantifierLight(" is where ")
                End If

                arWordDocumentVerbaliser.VerbalisePredicateText(Me.FrontText)

                If abIncludeSubscriptModelObjectNumbers Then
                    If Not Me.FactType.IsLinkFactType Then
                        If Me.FactType.HasMoreThanOneRoleReferencingTheSameModelObject() Then
                            Dim lrModelObjectCountDictionary As New Dictionary(Of String, Integer)
                            Call Me.FactType.GetReferencedModelObjectCounts(lrModelObjectCountDictionary)

                            For Each lrPredicatePart In Me.PredicatePart.AsEnumerable.Reverse
                                If lrModelObjectCountDictionary.ContainsKey(lrPredicatePart.Role.JoinedORMObject.Id) Then
                                    lrPredicatePart.ModelObjectSubscriptNumber = lrModelObjectCountDictionary.Item(lrPredicatePart.Role.JoinedORMObject.Id)
                                    lrModelObjectCountDictionary.Item(lrPredicatePart.Role.JoinedORMObject.Id) -= 1
                                End If
                            Next
                        End If
                    End If
                End If

                For Each lrPredicatePart In Me.PredicatePart
                    liSequenceNr += 1

                    arWordDocumentVerbaliser.VerbalisePredicateText(lrPredicatePart.PreBoundText)
                    arWordDocumentVerbaliser.VerbaliseModelObject(lrPredicatePart.Role.JoinedORMObject)

                    If Me.FactType.IsLinkFactType Then
                        Dim larRole = From Role In Me.FactType.LinkFactTypeRole.FactType.RoleGroup _
                                      Where Role.JoinedORMObject.Id = lrPredicatePart.Role.JoinedORMObject.Id _
                                      Select Role

                        If larRole.Count > 0 Then
                            If Me.FactType.LinkFactTypeRole.FactType.FactTypeReading.Count > 0 Then
                                lrPredicatePart.ModelObjectSubscriptNumber = _
                                    Me.FactType.LinkFactTypeRole.FactType.FactTypeReading(0).PredicatePart.Find(Function(x) x.Role.Id = lrPredicatePart.Role.FactType.LinkFactTypeRole.Id).ModelObjectSubscriptNumber
                            End If
                        End If
                    End If

                    If lrPredicatePart.ModelObjectSubscriptNumber > 0 Then
                        arWordDocumentVerbaliser.VerbaliseSubscript(lrPredicatePart.ModelObjectSubscriptNumber)
                    End If

                    arWordDocumentVerbaliser.VerbalisePredicateText(lrPredicatePart.PostBoundText)
                    If (liSequenceNr < Me.PredicatePart.Count) Or (lrPredicatePart.PredicatePartText <> "") Then
                        arWordDocumentVerbaliser.Write(" ")
                    End If

                    If (liSequenceNr = 1) And (Me.FactType.Arity = 2) And abIncludeRoleConstraintInformation Then
                        If Me.FactType.IsManyTo1BinaryFactType Then
                            If Me.MatchesRoleConstraintRoleOrder(Me.FactType.InternalUniquenessConstraint(0)) Then

                                arWordDocumentVerbaliser.VerbalisePredicateText(lrPredicatePart.PredicatePartText)

                                If Me.FactType.GetRoleById(Me.RoleList(0).Id).Mandatory = True Then
                                    arWordDocumentVerbaliser.VerbaliseQuantifierLight(" one")
                                Else
                                    arWordDocumentVerbaliser.VerbaliseQuantifierLight(" at most one")
                                End If
                            Else
                                If Me.FactType.GetRoleById(Me.RoleList(0).Id).Mandatory = True Then
                                    arWordDocumentVerbaliser.VerbalisePredicateText(lrPredicatePart.PredicatePartText)

                                    arWordDocumentVerbaliser.VerbaliseQuantifierLight(" more than one")
                                Else
                                    arWordDocumentVerbaliser.VerbaliseQuantifierLight(" possibly ")
                                    arWordDocumentVerbaliser.VerbalisePredicateText(lrPredicatePart.PredicatePartText)
                                    arWordDocumentVerbaliser.VerbaliseQuantifierLight(" more than one")

                                End If
                            End If
                        ElseIf Me.FactType.Is1To1BinaryFactType Then '  InternalUniquenessConstraint.Count = 1 Then

                            arWordDocumentVerbaliser.VerbalisePredicateText(lrPredicatePart.PredicatePartText)

                            If Me.MatchesRoleConstraintRoleOrder(Me.FactType.InternalUniquenessConstraint(0)) Then
                                If Me.FactType.GetRoleById(Me.RoleList(0).Id).Mandatory = True Then
                                    arWordDocumentVerbaliser.VerbaliseQuantifierLight(" one")
                                Else
                                    arWordDocumentVerbaliser.VerbaliseQuantifierLight(" at most one")
                                End If
                            Else
                                If Me.FactType.GetRoleById(Me.RoleList(0).Id).Mandatory = True Then
                                    arWordDocumentVerbaliser.VerbaliseQuantifierLight(" one")
                                Else
                                    arWordDocumentVerbaliser.VerbaliseQuantifierLight(" at most one")
                                End If
                            End If
                        Else
                            arWordDocumentVerbaliser.VerbalisePredicateText(lrPredicatePart.PredicatePartText)
                        End If
                    Else
                        arWordDocumentVerbaliser.VerbalisePredicateText(lrPredicatePart.PredicatePartText)
                    End If

                    If liSequenceNr < Me.PredicatePart.Count Then
                        arWordDocumentVerbaliser.Write(" ")
                    End If

                Next

                If Me.FollowingText <> "" Then
                    arWordDocumentVerbaliser.Write(" ")
                    arWordDocumentVerbaliser.VerbalisePredicateText(Me.FollowingText)
                End If

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Public Function GetReadingTextCQL(Optional ByVal abIncludeRoleConstraintInformation As Boolean = False, _
                                     Optional ByVal abIncludeSubscriptModelObjectNumbers As Boolean = False) As String

            Try
                '----------------------------------------------------
                'Create the dotted reading from the PredicateParts
                '----------------------------------------------------        
                Dim liSequenceNr As Integer = 0
                Dim lrPredicatePart As FBM.PredicatePart
                Dim lsVerbalisation As String = ""

                lsVerbalisation.AppendString(Me.FrontText)

                If abIncludeSubscriptModelObjectNumbers Then
                    If Not Me.FactType.IsLinkFactType Then
                        If Me.FactType.HasMoreThanOneRoleReferencingTheSameModelObject() Then
                            Dim lrModelObjectCountDictionary As New Dictionary(Of String, Integer)
                            Call Me.FactType.GetReferencedModelObjectCounts(lrModelObjectCountDictionary)

                            For Each lrPredicatePart In Me.PredicatePart.AsEnumerable.Reverse
                                If lrModelObjectCountDictionary.ContainsKey(lrPredicatePart.Role.JoinedORMObject.Id) Then
                                    lrPredicatePart.ModelObjectSubscriptNumber = lrModelObjectCountDictionary.Item(lrPredicatePart.Role.JoinedORMObject.Id)
                                    lrModelObjectCountDictionary.Item(lrPredicatePart.Role.JoinedORMObject.Id) -= 1
                                End If
                            Next
                        End If
                    End If
                End If

                For Each lrPredicatePart In Me.PredicatePart
                    liSequenceNr += 1

                    lsVerbalisation.AppendString(lrPredicatePart.PreBoundText)
                    lsVerbalisation.AppendString(lrPredicatePart.Role.JoinedORMObject.Id)

                    If Me.FactType.IsLinkFactType Then
                        Dim larRole = From Role In Me.FactType.LinkFactTypeRole.FactType.RoleGroup _
                                      Where Role.JoinedORMObject.Id = lrPredicatePart.Role.JoinedORMObject.Id _
                                      Select Role

                        If larRole.Count > 0 Then
                            If Me.FactType.LinkFactTypeRole.FactType.FactTypeReading.Count > 0 Then
                                lrPredicatePart.ModelObjectSubscriptNumber = _
                                    Me.FactType.LinkFactTypeRole.FactType.FactTypeReading(0).PredicatePart.Find(Function(x) x.Role.Id = lrPredicatePart.Role.FactType.LinkFactTypeRole.Id).ModelObjectSubscriptNumber
                            End If
                        End If
                    End If

                    If lrPredicatePart.ModelObjectSubscriptNumber > 0 Then
                        lsVerbalisation.AppendString(lrPredicatePart.ModelObjectSubscriptNumber)
                    End If

                    lsVerbalisation.AppendString(lrPredicatePart.PostBoundText)
                    If (liSequenceNr < Me.PredicatePart.Count) Or (lrPredicatePart.PredicatePartText <> "") Then
                        lsVerbalisation.AppendString(" ")
                    End If

                    If (liSequenceNr = 1) And (Me.FactType.Arity = 2) And abIncludeRoleConstraintInformation Then
                        If Me.FactType.IsManyTo1BinaryFactType Then
                            If Me.MatchesRoleConstraintRoleOrder(Me.FactType.InternalUniquenessConstraint(0)) Then

                                lsVerbalisation.AppendString(lrPredicatePart.PredicatePartText)

                                If Me.FactType.GetRoleById(Me.RoleList(0).Id).Mandatory = True Then
                                    lsVerbalisation.AppendString(" one ")
                                Else
                                    lsVerbalisation.AppendString(" at most one ")
                                End If
                            Else
                                If Me.FactType.GetRoleById(Me.RoleList(0).Id).Mandatory = True Then
                                    lsVerbalisation.AppendString(lrPredicatePart.PredicatePartText)

                                    lsVerbalisation.AppendString(" more than one ")
                                Else
                                    lsVerbalisation.AppendString(" possibly ")
                                    lsVerbalisation.AppendString(lrPredicatePart.PredicatePartText)
                                    lsVerbalisation.AppendString(" more than one ")

                                End If
                            End If
                        ElseIf Me.FactType.Is1To1BinaryFactType Then '  InternalUniquenessConstraint.Count = 1 Then

                            lsVerbalisation.AppendString(lrPredicatePart.PredicatePartText)

                            If Me.MatchesRoleConstraintRoleOrder(Me.FactType.InternalUniquenessConstraint(0)) Then
                                If Me.FactType.GetRoleById(Me.RoleList(0).Id).Mandatory = True Then
                                    lsVerbalisation.AppendString(" one ")
                                Else
                                    lsVerbalisation.AppendString(" at most one ")
                                End If
                            Else
                                If Me.FactType.GetRoleById(Me.RoleList(0).Id).Mandatory = True Then
                                    lsVerbalisation.AppendString(" one ")
                                Else
                                    lsVerbalisation.AppendString(" at most one ")
                                End If
                            End If
                        End If
                    Else
                        lsVerbalisation.AppendString(lrPredicatePart.PredicatePartText)
                    End If

                    If liSequenceNr < Me.PredicatePart.Count Then
                        lsVerbalisation.AppendString(" ")
                    End If

                Next

                If Me.FollowingText <> "" Then
                    lsVerbalisation.AppendString(" ")
                    lsVerbalisation.AppendString(Me.FollowingText)
                End If

                Return lsVerbalisation.Replace("  ", " ")

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)

                Return "Error generateing CQL for FactTypeReading: " & Me.Id
            End Try

        End Function

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
        ''' <param name="arVerbaliser">The HTML verbaliser to write to.</param> 
        ''' <param name="aarArgumentCommonModelObjects">The actual terms of the argument of the Role Constraint.</param>
        ''' <param name="abDropFirstRole">If TRUE, then the first Role/ObjectType.Name is dropped and replaced by 'that'</param>
        ''' <param name="arLastVerbalisedRole">Used to determine how to structure (next) reading based on previous reading.</param>
        ''' <remarks></remarks>
        Public Sub GetReadingTextThatOrSome(ByVal aarRole As List(Of FBM.Role),
                                            ByRef arVerbaliser As FBM.ORMVerbailser,
                                            ByRef aarArgumentCommonModelObjects As List(Of FBM.ModelObject),
                                            ByRef aarVerbalisedModelObjects As List(Of FBM.ModelObject),
                                            Optional ByVal abDropFirstRole As Boolean = False,
                                            Optional ByVal arLastVerbalisedRole As FBM.Role = Nothing,
                                            Optional ByVal abDropInitalThatOrSome As Boolean = False)

            Try
                '----------------------------------------------------
                'Create the dotted reading from the PredicateParts
                '----------------------------------------------------        
                Dim liSequenceNr As Integer = 0
                Dim lrPredicatePart As FBM.PredicatePart

                'Subscript Numbers
                If Me.FactType.HasMoreThanOneRoleReferencingTheSameModelObject() Then
                    Dim lrModelObjectCountDictionary As New Dictionary(Of String, Integer)
                    Call Me.FactType.GetReferencedModelObjectCounts(lrModelObjectCountDictionary)

                    For Each lrPredicatePart In Me.PredicatePart.AsEnumerable.Reverse
                        If lrModelObjectCountDictionary.ContainsKey(lrPredicatePart.Role.JoinedORMObject.Id) Then
                            lrPredicatePart.ModelObjectSubscriptNumber = lrModelObjectCountDictionary.Item(lrPredicatePart.Role.JoinedORMObject.Id)
                            lrModelObjectCountDictionary.Item(lrPredicatePart.Role.JoinedORMObject.Id) -= 1
                        End If
                    Next
                End If

                arVerbaliser.VerbaliseQuantifier(Me.FrontText)

                For Each lrPredicatePart In Me.PredicatePart
                    liSequenceNr += 1

                    If (abDropFirstRole And (liSequenceNr = 1)) Then

                        If arLastVerbalisedRole IsNot Nothing Then
                            If arLastVerbalisedRole.JoinedORMObject.Id = lrPredicatePart.Role.JoinedORMObject.Id Then
                                arVerbaliser.VerbaliseQuantifier("that ")
                            Else
                                If aarVerbalisedModelObjects.Contains(lrPredicatePart.Role.JoinedORMObject) Then
                                    arVerbaliser.VerbaliseQuantifier("and that ")
                                Else
                                    arVerbaliser.VerbaliseQuantifier("and some ")
                                End If

                                arVerbaliser.VerbalisePredicateText(lrPredicatePart.PreBoundText)
                                arVerbaliser.VerbaliseModelObject(lrPredicatePart.Role.JoinedORMObject)
                                arVerbaliser.VerbalisePredicateText(lrPredicatePart.PostBoundText)
                                arVerbaliser.VerbaliseQuantifier(" ")
                            End If
                        ElseIf abDropInitalThatOrSome Then
                            'Do nothing at this stage.
                        Else
                            arVerbaliser.VerbaliseQuantifier("that ")
                        End If

                    ElseIf aarRole.Contains(lrPredicatePart.Role) Then
                        If aarArgumentCommonModelObjects.Count = 0 Then
                            arVerbaliser.VerbaliseQuantifier("some ")

                            arVerbaliser.VerbalisePredicateText(lrPredicatePart.PreBoundText)
                            arVerbaliser.VerbaliseModelObject(lrPredicatePart.Role.JoinedORMObject)
                            arVerbaliser.VerbalisePredicateText(lrPredicatePart.PostBoundText)

                        ElseIf aarArgumentCommonModelObjects.FindAll(Function(x) x.Model.ModelObjectIsSubtypeOfModelObject(lrPredicatePart.Role.JoinedORMObject, x) = True).Count > 0 Then
                            arVerbaliser.VerbaliseQuantifier("some ")

                            arVerbaliser.VerbalisePredicateText(lrPredicatePart.PreBoundText)
                            arVerbaliser.VerbaliseModelObject(lrPredicatePart.Role.JoinedORMObject)
                            arVerbaliser.VerbalisePredicateText(lrPredicatePart.PostBoundText)

                            arVerbaliser.VerbaliseQuantifier(" that is that ")
                            arVerbaliser.VerbaliseModelObject(aarArgumentCommonModelObjects.Find(Function(x) Me.Model.ModelObjectIsSubtypeOfModelObject(lrPredicatePart.Role.JoinedORMObject, x) = True))
                        Else
                            If liSequenceNr = 1 And abDropInitalThatOrSome Then
                            Else
                                arVerbaliser.VerbaliseQuantifier("that ")
                            End If

                            arVerbaliser.VerbalisePredicateText(lrPredicatePart.PreBoundText)
                            arVerbaliser.VerbaliseModelObject(lrPredicatePart.Role.JoinedORMObject)
                            arVerbaliser.VerbalisePredicateText(lrPredicatePart.PostBoundText)
                        End If
                    Else
                        If aarVerbalisedModelObjects.Contains(lrPredicatePart.Role.JoinedORMObject) Then
                            arVerbaliser.VerbaliseQuantifier("that ")
                        Else
                            arVerbaliser.VerbaliseQuantifier("some ")
                        End If

                        arVerbaliser.VerbalisePredicateText(lrPredicatePart.PreBoundText)
                        arVerbaliser.VerbaliseModelObject(lrPredicatePart.Role.JoinedORMObject)
                        arVerbaliser.VerbalisePredicateText(lrPredicatePart.PostBoundText)
                    End If

                    If lrPredicatePart.ModelObjectSubscriptNumber > 0 Then
                        arVerbaliser.VerbaliseSubscript(lrPredicatePart.ModelObjectSubscriptNumber)
                    End If

                    If ((liSequenceNr < Me.PredicatePart.Count) Or
                       (lrPredicatePart.PredicatePartText <> "")) And
                       (Not (abDropFirstRole And (liSequenceNr = 1))) Then
                        arVerbaliser.HTW.Write(" ")
                    End If

                    arVerbaliser.VerbalisePredicateText(lrPredicatePart.PredicatePartText)

                    If liSequenceNr < Me.PredicatePart.Count Then
                        arVerbaliser.HTW.Write(" ")
                    End If

                    aarVerbalisedModelObjects.AddUnique(lrPredicatePart.Role.JoinedORMObject)
                Next

                If Me.FollowingText <> "" Then
                    arVerbaliser.VerbalisePredicateText(" " & Me.FollowingText)
                End If

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        ''' <summary>
        ''' Returns the ReadingText but with either 'that' or 'some' in front of the ObjectType names.
        ''' </summary>
        ''' <param name="aarRole">The set of Roles that determines whether 'that' or 'some' precedes the respective ObjectType's name.</param>
        ''' <param name="asVerbalisation">The verbalised string.</param> 
        ''' <param name="aarArgumentCommonModelObjects">The actual terms of the argument of the Role Constraint.</param>
        ''' <param name="abDropFirstRole">If TRUE, then the first Role/ObjectType.Name is dropped and replaced by 'that'</param>
        ''' <param name="arLastVerbalisedRole">Used to determine how to structure (next) reading based on previous reading.</param>
        ''' <remarks></remarks>
        Public Sub GetReadingTextThatOrSome(ByVal aarRole As List(Of FBM.Role),
                                            ByRef asVerbalisation As String,
                                            ByRef aarArgumentCommonModelObjects As List(Of FBM.ModelObject),
                                            ByRef aarVerbalisedModelObjects As List(Of FBM.ModelObject),
                                            Optional ByVal abDropFirstRole As Boolean = False,
                                            Optional ByVal arLastVerbalisedRole As FBM.Role = Nothing,
                                            Optional ByVal abDropInitalThatOrSome As Boolean = False)

            Try
                '----------------------------------------------------
                'Create the dotted reading from the PredicateParts
                '----------------------------------------------------        
                Dim liSequenceNr As Integer = 0
                Dim lrPredicatePart As FBM.PredicatePart

                'Subscript Numbers
                If Me.FactType.HasMoreThanOneRoleReferencingTheSameModelObject() Then
                    Dim lrModelObjectCountDictionary As New Dictionary(Of String, Integer)
                    Call Me.FactType.GetReferencedModelObjectCounts(lrModelObjectCountDictionary)

                    For Each lrPredicatePart In Me.PredicatePart.AsEnumerable.Reverse
                        If lrModelObjectCountDictionary.ContainsKey(lrPredicatePart.Role.JoinedORMObject.Id) Then
                            lrPredicatePart.ModelObjectSubscriptNumber = lrModelObjectCountDictionary.Item(lrPredicatePart.Role.JoinedORMObject.Id)
                            lrModelObjectCountDictionary.Item(lrPredicatePart.Role.JoinedORMObject.Id) -= 1
                        End If
                    Next
                End If

                asVerbalisation &= Me.FrontText

                For Each lrPredicatePart In Me.PredicatePart
                    liSequenceNr += 1

                    If (abDropFirstRole And (liSequenceNr = 1)) Then

                        If arLastVerbalisedRole IsNot Nothing Then
                            If arLastVerbalisedRole.JoinedORMObject.Id = lrPredicatePart.Role.JoinedORMObject.Id Then
                                asVerbalisation &= "that "
                            Else
                                If aarVerbalisedModelObjects.Contains(lrPredicatePart.Role.JoinedORMObject) Then
                                    asVerbalisation &= "and that "
                                Else
                                    asVerbalisation &= "and some "
                                End If

                                asVerbalisation &= lrPredicatePart.PreBoundText
                                asVerbalisation &= lrPredicatePart.Role.JoinedORMObject.Id
                                asVerbalisation &= lrPredicatePart.PostBoundText
                                asVerbalisation &= " "
                            End If
                        ElseIf abDropInitalThatOrSome Then
                            'Do nothing at this stage.
                        Else
                            asVerbalisation &= "that "
                        End If

                    ElseIf aarRole.Contains(lrPredicatePart.Role) Then
                        If aarArgumentCommonModelObjects.Count = 0 Then
                            asVerbalisation &= "some "

                            asVerbalisation &= lrPredicatePart.PreBoundText
                            asVerbalisation &= lrPredicatePart.Role.JoinedORMObject.Id
                            asVerbalisation &= lrPredicatePart.PostBoundText

                        ElseIf aarArgumentCommonModelObjects.FindAll(Function(x) x.Model.ModelObjectIsSubtypeOfModelObject(lrPredicatePart.Role.JoinedORMObject, x) = True).Count > 0 Then
                            asVerbalisation &= "some "

                            asVerbalisation &= lrPredicatePart.PreBoundText
                            asVerbalisation &= lrPredicatePart.Role.JoinedORMObject.Id
                            asVerbalisation &= lrPredicatePart.PostBoundText

                            asVerbalisation &= " that is that "
                            asVerbalisation &= aarArgumentCommonModelObjects.Find(Function(x) Me.Model.ModelObjectIsSubtypeOfModelObject(lrPredicatePart.Role.JoinedORMObject, x) = True).Id
                        Else
                            If liSequenceNr = 1 And abDropInitalThatOrSome Then
                            Else
                                asVerbalisation &= "that "
                            End If

                            asVerbalisation &= lrPredicatePart.PreBoundText
                            asVerbalisation &= lrPredicatePart.Role.JoinedORMObject.Id
                            asVerbalisation &= lrPredicatePart.PostBoundText
                        End If
                    Else
                        If aarVerbalisedModelObjects.Contains(lrPredicatePart.Role.JoinedORMObject) Then
                            asVerbalisation &= "that "
                        Else
                            asVerbalisation &= "some "
                        End If

                        asVerbalisation &= lrPredicatePart.PreBoundText
                        asVerbalisation &= lrPredicatePart.Role.JoinedORMObject.Id
                        asVerbalisation &= lrPredicatePart.PostBoundText
                    End If

                    If lrPredicatePart.ModelObjectSubscriptNumber > 0 Then
                        asVerbalisation &= lrPredicatePart.ModelObjectSubscriptNumber
                    End If

                    If ((liSequenceNr < Me.PredicatePart.Count) Or
                       (lrPredicatePart.PredicatePartText <> "")) And
                       (Not (abDropFirstRole And (liSequenceNr = 1))) Then
                        asVerbalisation &= " "
                    End If

                    asVerbalisation &= lrPredicatePart.PredicatePartText

                    If liSequenceNr < Me.PredicatePart.Count Then
                        asVerbalisation &= " "
                    End If

                    aarVerbalisedModelObjects.AddUnique(lrPredicatePart.Role.JoinedORMObject)
                Next

                If Me.FollowingText <> "" Then
                    asVerbalisation &= " " & Me.FollowingText
                End If

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub


        ''' <summary>
        ''' Returns the ReadingText but with either 'that' or 'some' in front of the ObjectType names.
        ''' </summary>
        ''' <param name="aarRole">The set of Roles that determines whether 'that' or 'some' precedes the respective ObjectType's name.</param>
        ''' <param name="arVerbaliser">The HTML verbaliser to write to.</param> 
        ''' <param name="aarArgumentCommonModelObjects">The actual terms of the argument of the Role Constraint.</param>
        ''' <param name="abDropFirstRole">If TRUE, then the first Role/ObjectType.Name is dropped and replaced by 'that'</param>
        ''' <param name="arLastVerbalisedRole">Used to determine how to structure (next) reading based on previous reading.</param>
        ''' <remarks></remarks>
        Public Sub GetReadingTextThatOrSome(ByVal aarRole As List(Of FBM.Role), _
                                            ByRef arVerbaliser As FBM.ORMWordVerbailser, _
                                            ByRef aarArgumentCommonModelObjects As List(Of FBM.ModelObject), _
                                            ByRef aarVerbalisedModelObjects As List(Of FBM.ModelObject),
                                            Optional ByVal abDropFirstRole As Boolean = False,
                                            Optional ByVal arLastVerbalisedRole As FBM.Role = Nothing,
                                            Optional ByVal abDropInitalThatOrSome As Boolean = False)

            Try
                '----------------------------------------------------
                'Create the dotted reading from the PredicateParts
                '----------------------------------------------------        
                Dim liSequenceNr As Integer = 0
                Dim lrPredicatePart As FBM.PredicatePart

                'Subscript Numbers
                If Me.FactType.HasMoreThanOneRoleReferencingTheSameModelObject() Then
                    Dim lrModelObjectCountDictionary As New Dictionary(Of String, Integer)
                    Call Me.FactType.GetReferencedModelObjectCounts(lrModelObjectCountDictionary)

                    For Each lrPredicatePart In Me.PredicatePart.AsEnumerable.Reverse
                        If lrModelObjectCountDictionary.ContainsKey(lrPredicatePart.Role.JoinedORMObject.Id) Then
                            lrPredicatePart.ModelObjectSubscriptNumber = lrModelObjectCountDictionary.Item(lrPredicatePart.Role.JoinedORMObject.Id)
                            lrModelObjectCountDictionary.Item(lrPredicatePart.Role.JoinedORMObject.Id) -= 1
                        End If
                    Next
                End If

                arVerbaliser.VerbaliseQuantifier(Me.FrontText)

                For Each lrPredicatePart In Me.PredicatePart
                    liSequenceNr += 1

                    If (abDropFirstRole And (liSequenceNr = 1)) Then

                        If arLastVerbalisedRole IsNot Nothing Then
                            If arLastVerbalisedRole.JoinedORMObject.Id = lrPredicatePart.Role.JoinedORMObject.Id Then
                                arVerbaliser.VerbaliseQuantifier("that ")
                            Else
                                If aarVerbalisedModelObjects.Contains(lrPredicatePart.Role.JoinedORMObject) Then
                                    arVerbaliser.VerbaliseQuantifier("and that ")
                                Else
                                    arVerbaliser.VerbaliseQuantifier("and some ")
                                End If

                                arVerbaliser.VerbalisePredicateText(lrPredicatePart.PreBoundText)
                                arVerbaliser.VerbaliseModelObject(lrPredicatePart.Role.JoinedORMObject)
                                arVerbaliser.VerbalisePredicateText(lrPredicatePart.PostBoundText)
                                arVerbaliser.VerbaliseQuantifier(" ")
                            End If
                        ElseIf abDropInitalThatOrSome Then
                            'Do nothing at this stage.
                        Else
                            arVerbaliser.VerbaliseQuantifier("that ")
                        End If

                    ElseIf aarRole.Contains(lrPredicatePart.Role) Then
                        If aarArgumentCommonModelObjects.Count = 0 Then
                            arVerbaliser.VerbaliseQuantifier("some ")

                            arVerbaliser.VerbalisePredicateText(lrPredicatePart.PreBoundText)
                            arVerbaliser.VerbaliseModelObject(lrPredicatePart.Role.JoinedORMObject)
                            arVerbaliser.VerbalisePredicateText(lrPredicatePart.PostBoundText)

                        ElseIf aarArgumentCommonModelObjects.FindAll(Function(x) x.Model.ModelObjectIsSubtypeOfModelObject(lrPredicatePart.Role.JoinedORMObject, x) = True).Count > 0 Then
                            arVerbaliser.VerbaliseQuantifier("some ")

                            arVerbaliser.VerbalisePredicateText(lrPredicatePart.PreBoundText)
                            arVerbaliser.VerbaliseModelObject(lrPredicatePart.Role.JoinedORMObject)
                            arVerbaliser.VerbalisePredicateText(lrPredicatePart.PostBoundText)

                            arVerbaliser.VerbaliseQuantifier(" that is that ")
                            arVerbaliser.VerbaliseModelObject(aarArgumentCommonModelObjects.Find(Function(x) Me.Model.ModelObjectIsSubtypeOfModelObject(lrPredicatePart.Role.JoinedORMObject, x) = True))
                        Else
                            If liSequenceNr = 1 And abDropInitalThatOrSome Then
                            Else
                                arVerbaliser.VerbaliseQuantifier("that ")
                            End If

                            arVerbaliser.VerbalisePredicateText(lrPredicatePart.PreBoundText)
                            arVerbaliser.VerbaliseModelObject(lrPredicatePart.Role.JoinedORMObject)
                            arVerbaliser.VerbalisePredicateText(lrPredicatePart.PostBoundText)
                        End If
                    Else
                        If aarVerbalisedModelObjects.Contains(lrPredicatePart.Role.JoinedORMObject) Then
                            arVerbaliser.VerbaliseQuantifier("that ")
                        Else
                            arVerbaliser.VerbaliseQuantifier("some ")
                        End If

                        arVerbaliser.VerbalisePredicateText(lrPredicatePart.PreBoundText)
                        arVerbaliser.VerbaliseModelObject(lrPredicatePart.Role.JoinedORMObject)
                        arVerbaliser.VerbalisePredicateText(lrPredicatePart.PostBoundText)
                    End If

                    If lrPredicatePart.ModelObjectSubscriptNumber > 0 Then
                        arVerbaliser.VerbaliseSubscript(lrPredicatePart.ModelObjectSubscriptNumber)
                    End If

                    If ((liSequenceNr < Me.PredicatePart.Count) Or _
                       (lrPredicatePart.PredicatePartText <> "")) And _
                       (Not (abDropFirstRole And (liSequenceNr = 1))) Then
                        arVerbaliser.Write(" ")
                    End If

                    arVerbaliser.VerbalisePredicateText(lrPredicatePart.PredicatePartText)

                    If liSequenceNr < Me.PredicatePart.Count Then
                        arVerbaliser.Write(" ")
                    End If

                    aarVerbalisedModelObjects.AddUnique(lrPredicatePart.Role.JoinedORMObject)
                Next

                If Me.FollowingText <> "" Then
                    arVerbaliser.VerbalisePredicateText(" " & Me.FollowingText)
                End If

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
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
        Public Sub RemovePredicatePartForRole(ByRef arRole As FBM.Role)

            Dim lrPredicatePart As FBM.PredicatePart
            Dim lrPredicatePartInd As FBM.PredicatePart
            Dim lrRoleToBeRemoved As FBM.Role

            Try
                lrRoleToBeRemoved = arRole

                lrPredicatePart = Me.PredicatePart.Find(Function(x) x.Role.Id = lrRoleToBeRemoved.Id)

                If lrPredicatePart Is Nothing Then
                    'Nothing to remove
                    Exit Sub
                End If

                Dim liSequenceNrBeingRemoved As Integer = lrPredicatePart.SequenceNr

                '--------------------------------------------------------------
                'Remove the PredicatePart (by Role) for the FTR from the database.
                '--------------------------------------------------------------                
                Call tableORMPredicatePart.DeletePredicatePartByRole(lrPredicatePart)

                Me.PredicatePart.Remove(lrPredicatePart)

                For Each lrPredicatePartInd In Me.PredicatePart.FindAll(Function(x) x.SequenceNr >= liSequenceNrBeingRemoved)
                    lrPredicatePartInd.SequenceNr -= 1
                    tableORMPredicatePart.UpdatePredicatePart(lrPredicatePartInd)
                Next

                '---------------------------------------------------------------------------------------------------------
                'Make sure that the last PredicatePart of the FactTypeReading has a "" (empty string) PredicatePartText.
                '---------------------------------------------------------------------------------------------------------
                If Me.PredicatePart.Count > 0 Then
                    Me.PredicatePart(Me.PredicatePart.Count - 1).PredicatePartText = ""
                End If

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace,,,,,, ex)
            End Try


        End Sub

        Public Overrides Function RemoveFromModel(Optional ByVal abForceRemoval As Boolean = False,
                                                  Optional ByVal abCheckForErrors As Boolean = True,
                                                  Optional ByVal abDoDatabaseProcessing As Boolean = True,
                                                  Optional ByVal abIncludeSubtypeRelationshipFactTypes As Boolean = True,
                                                  Optional ByVal abRemoveIndex As Boolean = True,
                                                  Optional ByVal abIsPartOfSimpleReferenceScheme As Boolean = False) As Boolean

            Try
                Call Me.FactType.RemoveFactTypeReading(Me, abDoDatabaseProcessing)

                If abDoDatabaseProcessing Then
                    Call TableFactTypeReading.DeleteFactTypeReading(Me)

                    '-----------------------------------------------------------------------------------------------------
                    'Models Stored as XML need to be saved to remove the appropriate ModelElements, and is a quick save.
                    If Me.Model.StoreAsXML Then Me.Model.Save()
                End If

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Function

        ''' <summary>
        ''' Saves the FactTypeReading to the database
        ''' </summary>
        ''' <param name="abRapidSave"></param>
        ''' <remarks></remarks>
        Public Overloads Sub Save(Optional ByVal abRapidSave As Boolean = False)

            Try
                If abRapidSave Then
                    Call TableFactTypeReading.AddFactTypeReading(Me)
                ElseIf Me.isDirty Then

                    If TableFactTypeReading.ExistsFactTypeReading(Me) Then
                        Call TableFactTypeReading.UpdateFactTypeReading(Me)
                    Else
                        Call TableFactTypeReading.AddFactTypeReading(Me)
                    End If

                    Me.isDirty = False

                End If

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try
        End Sub

    End Class

End Namespace