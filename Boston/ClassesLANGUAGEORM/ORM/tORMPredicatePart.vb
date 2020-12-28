Imports System.Xml.Serialization
Imports System.Reflection

Namespace FBM
    <Serializable()> _
    Public Class PredicatePart
        Implements IEquatable(Of FBM.PredicatePart)

        <NonSerialized()> _
        <XmlIgnore()> _
        Public Model As FBM.Model

        <XmlIgnore()> _
        Public FactTypeReading As FBM.FactTypeReading

        <XmlIgnore()> _
        Public Role As FBM.Role

        <XmlIgnore()> _
        Public ModelObjectSubscriptNumber As Integer = 0

        <XmlAttribute()> _
        Public ReadOnly Property RoleId() As String
            Get
                If Me.Role Is Nothing Then
                    Return Nothing
                ElseIf Me.Role.Id Is Nothing Then
                    Return Nothing
                Else
                    Return Me.Role.Id
                End If
            End Get
        End Property

        <XmlAttribute()> _
        Public SequenceNr As Integer = 1 'Has to at least be one
        'Public ObjectType1 As FBM.ModelObject
        'Public ObjectType2 As FBM.ModelObject

        'New model
        Public PreBoundText As String = "" 'Prebound text, normally in the form "SSSS-", preceding the ModelElement(Name) referenced by the referenced Role.
        Public PostBoundText As String = "" 'Postbound text, normally in the form "-SSSS", following the ModelElement(Name) referenced by the referenced Role.        

        ''' <summary>
        ''' The full text of the PredicatePart. i.e. Not 'split' into individual words/Symbols.
        ''' </summary>
        ''' <remarks></remarks>
        Public PredicatePartText As String = ""

        ''' <summary>
        ''' Is a 'split' of the PredicatePartText. i.e. Is the individual words of the PredicatePartText.
        ''' </summary>
        ''' <remarks></remarks>
        Public PredicatePart() As String

        Public isDirty As Boolean = False

        Public Sub New()
            '---------
            'Default
            '---------
        End Sub

        Public Sub New(ByRef arModel As FBM.Model, _
                       ByRef arFactTypeReading As FBM.FactTypeReading, _
                       Optional ByRef arRole As FBM.Role = Nothing,
                       Optional ByVal abMakeDirty As Boolean = False)

            Me.Model = arModel
            Me.FactTypeReading = arFactTypeReading
            Me.Role = arRole
            Me.isDirty = abMakeDirty

        End Sub

        Sub New(ByVal aiSequenceNr As Integer, ByRef arRole As FBM.Role, ByVal as_PredicatePartText As String)

            Me.SequenceNr = aiSequenceNr
            Me.Role = arRole
            Me.PredicatePartText = as_PredicatePartText
            Me.PredicatePart = as_PredicatePartText.Split

        End Sub

        Public Shadows Function Equals(ByVal other As FBM.PredicatePart) As Boolean Implements System.IEquatable(Of FBM.PredicatePart).Equals

            If (Me.FactTypeReading.Id = other.FactTypeReading.Id) And (Me.RoleId = other.RoleId) Then                
                Return True
            Else
                Return False
            End If

        End Function

        Public Overloads Function Clone(ByRef arModel As FBM.Model, ByRef arFactTypeReading As FBM.FactTypeReading, Optional abAddToModel As Boolean = False) As FBM.PredicatePart

            Dim lrPredicatePart As New FBM.PredicatePart

            Try
                With Me

                    lrPredicatePart.Model = arModel
                    lrPredicatePart.FactTypeReading = arFactTypeReading
                    lrPredicatePart.SequenceNr = .SequenceNr
                    lrPredicatePart.Role = .Role.Clone(arModel, abAddToModel)
                    lrPredicatePart.PredicatePart = .PredicatePart
                    lrPredicatePart.PredicatePartText = .PredicatePartText
                    lrPredicatePart.isDirty = True
                End With

                Return lrPredicatePart

            Catch ex As Exception
                Dim lsMessage As String
                Dim mb As MethodBase = MethodInfo.GetCurrentMethod()

                lsMessage = "Error: " & mb.ReflectedType.Name & "." & mb.Name
                lsMessage &= vbCrLf & vbCrLf & ex.Message
                prApplication.ThrowErrorMessage(lsMessage, pcenumErrorType.Critical, ex.StackTrace)

                Return lrPredicatePart
            End Try


        End Function

        Public Sub makeDirty()
            Me.isDirty = True
        End Sub

        ''' <summary>
        ''' Saves the PredicatePart to the database
        ''' </summary>
        ''' <param name="abRapidSave"></param>
        ''' <remarks></remarks>
        Public Sub Save(Optional ByVal abRapidSave As Boolean = False)

            Try

                If abRapidSave Then
                    Call tableORMPredicatePart.AddPredicatePart(Me)
                ElseIf Me.isDirty Then

                    If tableORMPredicatePart.ExistsPredicatePart(Me) Then
                        Call tableORMPredicatePart.UpdatePredicatePart(Me)
                    Else
                        Call tableORMPredicatePart.AddPredicatePart(Me)
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