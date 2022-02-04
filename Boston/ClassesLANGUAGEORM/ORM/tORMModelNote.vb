Imports System.Xml.Serialization
Imports System.Reflection

Namespace FBM
    <Serializable()> _
    Public Class ModelNote
        Inherits FBM.ModelObject
        Implements ICloneable
        Implements iMDAObject

        Public Text As String = "" 'The text of the ModelNote
        Public JoinedObjectType As FBM.ModelObject

        <XmlIgnore()> _
        <DebuggerBrowsable(DebuggerBrowsableState.Never)> _
        Private _IsMDAModelElement As Boolean = False
        <XmlAttribute()> _
        Public Property IsMDAModelElement() As Boolean Implements iMDAObject.IsMDAModelElement
            Get
                Return Me._IsMDAModelElement
            End Get
            Set(ByVal value As Boolean)
                Me._IsMDAModelElement = value
            End Set
        End Property

        Public Event TextChanged(ByVal asNewText As String)
        Public Event JoinedObjectTypeChanged(ByVal arJoinedModelObject As FBM.ModelObject)
        Public Event RemovedFromModel(ByVal abBroadcastInterfaceEvent As Boolean)

        ''' <summary>
        ''' Parameterless constructor
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub New()

            Me.ConceptType = pcenumConceptType.ModelNote
            Me.Id = System.Guid.NewGuid.ToString

        End Sub

        Public Sub New(ByRef arModel As FBM.Model)

            Call Me.New()
            Me.Model = arModel

        End Sub

        Public Overloads Function Clone(ByRef arModel As FBM.Model) As Object

            Dim lrModelNote As New FBM.ModelNote

            With Me
                lrModelNote.Model = arModel
                lrModelNote.Text = .Text
                lrModelNote.isDirty = True
            End With

            Return lrModelNote

        End Function

        Public Shadows Function CloneInstance(ByRef arPage As FBM.Page, Optional ByVal abAddToPage As Boolean = False) As FBM.ModelNoteInstance

            Dim lrModelNoteInstance As New FBM.ModelNoteInstance

            With Me
                lrModelNoteInstance.Id = .Id
                lrModelNoteInstance.Model = arPage.Model
                lrModelNoteInstance.Page = arPage
                lrModelNoteInstance.Text = .Text
                If IsSomething(.JoinedObjectType) Then
                    Select Case Me.JoinedObjectType.ConceptType
                        Case Is = pcenumConceptType.EntityType
                            lrModelNoteInstance.JoinedObjectType = arPage.EntityTypeInstance.Find(AddressOf Me.JoinedObjectType.Equals)
                        Case Is = pcenumConceptType.ValueType
                            lrModelNoteInstance.JoinedObjectType = arPage.ValueTypeInstance.Find(AddressOf Me.JoinedObjectType.Equals)
                        Case Is = pcenumConceptType.FactType
                            lrModelNoteInstance.JoinedObjectType = arPage.FactTypeInstance.Find(AddressOf Me.JoinedObjectType.Equals)
                    End Select
                Else
                    lrModelNoteInstance.JoinedObjectType = Nothing
                End If
            End With

            If abAddToPage Then
                arPage.ModelNoteInstance.Add(lrModelNoteInstance)
            End If

            Return lrModelNoteInstance

        End Function

        Public Overrides Function CanSafelyRemoveFromModel() As Boolean
            Return False
        End Function

        ''' <summary>
        ''' Returns the unique Signature of the ModelNote
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overrides Function GetSignature() As String

            Dim lsSignature As String

            lsSignature = Me.Id
            lsSignature &= Me.JoinedObjectType.Id
            lsSignature &= Me.Text

            Return lsSignature

        End Function

        Public Overrides Function RemoveFromModel(Optional ByVal abForceRemoval As Boolean = False,
                                                  Optional ByVal abCheckForErrors As Boolean = True,
                                                  Optional ByVal abDoDatabaseProcessing As Boolean = True,
                                                  Optional ByVal abIncludeSubtypeRelationshipFactTypes As Boolean = True) As Boolean

            Try
                RemoveFromModel = True

                Dim lrModelDictionaryEntry As New FBM.DictionaryEntry(Me.Model, Me.Id, pcenumConceptType.ModelNote)
                lrModelDictionaryEntry = Me.Model.ModelDictionary.Find(AddressOf lrModelDictionaryEntry.Equals)

                If abDoDatabaseProcessing Then
                    Call TableModelNote.DeleteModelNote(Me)
                End If

                Me.Model.RemoveModelNote(Me, abDoDatabaseProcessing)

                RaiseEvent RemovedFromModel(abDoDatabaseProcessing)

                Return True

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Function

        ''' <summary>
        ''' Saves the ModelNote to the database
        ''' </summary>
        ''' <remarks></remarks>
        Public Overridable Overloads Sub Save(Optional ByVal abRapidSave As Boolean = False)

            Try
                '20220129-VM-Haven't seen this error for a long time. If after a time, not missed, then remove.
                'Dim lrDictionaryEntry As New FBM.DictionaryEntry(Me.Model, Me.Id, pcenumConceptType.ModelNote, Me.ShortDescription, Me.LongDescription)
                'lrDictionaryEntry = Me.Model.AddModelDictionaryEntry(lrDictionaryEntry, False, False)
                'Call lrDictionaryEntry.Save()

                If abRapidSave Then
                    pdbConnection.BeginTrans()
                    Call TableModelNote.AddModelNote(Me)
                    pdbConnection.CommitTrans()
                    Me.isDirty = False
                ElseIf Me.isDirty Then

                    If TableModelNote.ExistsModelNote(Me) Then
                        Call TableModelNote.UpdateModelNote(Me)
                    Else
                        Try
                            pdbConnection.BeginTrans()
                            Call TableModelNote.AddModelNote(Me)
                            pdbConnection.CommitTrans()
                        Catch ar_err As Exception
                            pdbConnection.RollbackTrans()
                            Throw New ApplicationException("Error: ModelNote.Save: " & ar_err.Message)
                        End Try
                    End If

                    Me.isDirty = False
                End If

            Catch ex As Exception
                Dim lsMessage1 As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage1 = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage1 &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage1, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Public Sub SetText(ByVal asText As String)

            Try
                Me.Text = asText

                Call Me.makeDirty()
                Call Me.Model.MakeDirty(False, False)

                RaiseEvent TextChanged(asText)
            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)
            End Try

        End Sub

        Public Sub SetJoinedModelObject(ByRef arModelObject As FBM.ModelObject)

            Me.JoinedObjectType = arModelObject

            Call Me.makeDirty()
            Call Me.Model.MakeDirty(False, False)

            RaiseEvent JoinedObjectTypeChanged(arModelObject)

        End Sub

    End Class
End Namespace
