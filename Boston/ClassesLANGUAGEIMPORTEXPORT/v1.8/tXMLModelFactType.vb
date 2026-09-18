Imports System.Xml.Serialization

Namespace XMLModel
    <Serializable()> _
    Public Class FactType
        Implements IEquatable(Of XMLModel.FactType)

        <DebuggerBrowsable(DebuggerBrowsableState.Never)> _
        Private _Id As String
        <XmlAttribute()> _
        Public Property Id() As String
            Get
                Return Me._Id
            End Get
            Set(ByVal value As String)
                Me._Id = value
            End Set
        End Property

        <DebuggerBrowsable(DebuggerBrowsableState.Never)> _
        Private _GUID As String
        <XmlAttribute()> _
        Public Property GUID As String
            Get
                Return Me._GUID
            End Get
            Set(ByVal value As String)
                Me._GUID = value
            End Set
        End Property

        <DebuggerBrowsable(DebuggerBrowsableState.Never)> _
        Private _Name As String
        <XmlAttribute()>
        Public Property Name() As String
            Get
                Return Me._Name
            End Get
            Set(ByVal value As String)
                Me._Name = value
            End Set
        End Property

        <DebuggerBrowsable(DebuggerBrowsableState.Never)>
        Private _DBName As String = ""
        <XmlAttribute()>
        Public Property DBName() As String
            Get
                Return Me._DBName
            End Get
            Set(ByVal value As String)
                Me._DBName = value
            End Set
        End Property

        <XmlArray("Aliases")>
        <XmlArrayItem("Alias")>
        Public Property [Alias] As List(Of XMLModel.Alias)

        <DebuggerBrowsable(DebuggerBrowsableState.Never)>
        Private _GraphLabel As New List(Of String)
        Public Property GraphLabel() As List(Of String)
            Get
                Return Me._GraphLabel
            End Get
            Set(ByVal value As List(Of String))
                Me._GraphLabel = value
            End Set
        End Property

#Region "Property Graph Schema Specific"

        <DebuggerBrowsable(DebuggerBrowsableState.Never)>
        Private _Source As String
        <XmlAttribute>
        Public Property Source() As String
            Get
                Return Me._Source
            End Get
            Set(ByVal value As String)
                Me._Source = value
            End Set
        End Property
        Public Function ShouldSerializeSource() As Boolean
            Return Not String.IsNullOrWhiteSpace(Source)
        End Function

        <DebuggerBrowsable(DebuggerBrowsableState.Never)>
        Private _Target As String
        <XmlAttribute>
        Public Property Target() As String
            Get
                Return Me._Target
            End Get
            Set(ByVal value As String)
                Me._Target = value
            End Set
        End Property

        Public Function ShouldSerializeTarget() As Boolean
            Return Not String.IsNullOrWhiteSpace(Target)
        End Function

#End Region

        <DebuggerBrowsable(DebuggerBrowsableState.Never)> _
        Private _ObjectifyingEntityTypeId As String = ""
        <XmlAttribute()> _
        Public Property ObjectifyingEntityTypeId() As String
            Get
                Return Me._ObjectifyingEntityTypeId
            End Get
            Set(ByVal value As String)
                Me._ObjectifyingEntityTypeId = value
            End Set
        End Property

        <DebuggerBrowsable(DebuggerBrowsableState.Never)> _
        Private _RoleGroup As New List(Of XMLModel.Role)
        Public Property RoleGroup() As List(Of XMLModel.Role)
            Get
                Return Me._RoleGroup
            End Get
            Set(ByVal value As List(Of XMLModel.Role))
                Me._RoleGroup = value
            End Set
        End Property

        <DebuggerBrowsable(DebuggerBrowsableState.Never)> _
        Private _Facts As New List(Of XMLModel.Fact)
        Public Property Facts() As List(Of XMLModel.Fact)
            Get
                Return Me._Facts
            End Get
            Set(ByVal value As List(Of XMLModel.Fact))
                Me._Facts = value
            End Set
        End Property

        <DebuggerBrowsable(DebuggerBrowsableState.Never)> _
        Private _FactTypeReadings As New List(Of XMLModel.FactTypeReading)
        Public Property FactTypeReadings() As List(Of XMLModel.FactTypeReading)
            Get
                Return Me._FactTypeReadings
            End Get
            Set(ByVal value As List(Of XMLModel.FactTypeReading))
                Me._FactTypeReadings = value
            End Set
        End Property

        <DebuggerBrowsable(DebuggerBrowsableState.Never)> _
        Private _IsLinkFactType As Boolean
        <XmlAttribute()> _
        Public Property IsLinkFactType As Boolean
            Get
                Return Me._IsLinkFactType
            End Get
            Set(ByVal value As Boolean)
                Me._IsLinkFactType = value
            End Set
        End Property

        <DebuggerBrowsable(DebuggerBrowsableState.Never)> _
        Private _LinkFactTypeRoleId As String
        <XmlAttribute()> _
        Public Property LinkFactTypeRoleId As String
            Get
                Return Me._LinkFactTypeRoleId
            End Get
            Set(ByVal value As String)
                Me._LinkFactTypeRoleId = value
            End Set
        End Property

        <DebuggerBrowsable(DebuggerBrowsableState.Never)> _
        Private _IsMDAModelElement As Boolean
        <XmlAttribute()> _
        Public Property IsMDAModelElement As Boolean
            Get
                Return Me._IsMDAModelElement
            End Get
            Set(ByVal value As Boolean)
                Me._IsMDAModelElement = value
            End Set
        End Property

        <DebuggerBrowsable(DebuggerBrowsableState.Never)> _
        Private _IsSubtypeRelationshipFactType As Boolean
        <XmlAttribute()> _
        Public Property IsSubtypeRelationshipFactType() As Boolean
            Get
                Return Me._IsSubtypeRelationshipFactType
            End Get
            Set(ByVal value As Boolean)
                Me._IsSubtypeRelationshipFactType = value
            End Set
        End Property

        <DebuggerBrowsable(DebuggerBrowsableState.Never)> _
        Private _IsObjectified As Boolean
        <XmlAttribute()> _
        Public Property IsObjectified() As Boolean
            Get
                Return Me._IsObjectified
            End Get
            Set(ByVal value As Boolean)
                Me._IsObjectified = value
            End Set
        End Property

        <DebuggerBrowsable(DebuggerBrowsableState.Never)> _
        Private _IsPreferredReferenceSchemeFT As Boolean
        <XmlAttribute()> _
        Public Property IsPreferredReferenceSchemeFT() As Boolean
            Get
                Return Me._IsPreferredReferenceSchemeFT
            End Get
            Set(ByVal value As Boolean)
                Me._IsPreferredReferenceSchemeFT = value
            End Set
        End Property

        <DebuggerBrowsable(DebuggerBrowsableState.Never)> _
        Private _IsDerived As Boolean
        <XmlAttribute()> _
        Public Property IsDerived As Boolean
            Get
                Return Me._IsDerived
            End Get
            Set(ByVal value As Boolean)
                Me._IsDerived = value
            End Set
        End Property

        <DebuggerBrowsable(DebuggerBrowsableState.Never)> _
        Private _IsStored As Boolean
        <XmlAttribute()> _
        Public Property IsStored As Boolean
            Get
                Return Me._IsStored
            End Get
            Set(ByVal value As Boolean)
                Me._IsStored = value
            End Set
        End Property

        <DebuggerBrowsable(DebuggerBrowsableState.Never)> _
        Private _DerivationText As String
        <XmlAttribute()> _
        Public Property DerivationText As String
            Get
                Return Me._DerivationText
            End Get
            Set(ByVal value As String)
                Me._DerivationText = value
            End Set
        End Property


        <DebuggerBrowsable(DebuggerBrowsableState.Never)> _
        Private _LongDescription As String = ""
        <XmlAttribute()> _
        Public Property LongDescription() As String
            Get
                Return Me._LongDescription
            End Get
            Set(ByVal value As String)
                Me._LongDescription = value
            End Set
        End Property

        <DebuggerBrowsable(DebuggerBrowsableState.Never)> _
        Private _ShortDescription As String = ""
        <XmlAttribute()> _
        Public Property ShortDescription() As String
            Get
                Return Me._ShortDescription
            End Get
            Set(ByVal value As String)
                Me._ShortDescription = value
            End Set
        End Property

        <DebuggerBrowsable(DebuggerBrowsableState.Never)> _
        Private _IsIndependent As Boolean
        <XmlAttribute()>
        Public Property IsIndependent As Boolean
            Get
                Return Me._IsIndependent
            End Get
            Set(ByVal value As Boolean)
                Me._IsIndependent = value
            End Set
        End Property

        <DebuggerBrowsable(DebuggerBrowsableState.Never)>
        Private _IsSubtypeStateControlling As Boolean
        <XmlAttribute()>
        Public Property IsSubtypeStateControlling As Boolean
            Get
                Return Me._IsSubtypeStateControlling
            End Get
            Set(ByVal value As Boolean)
                Me._IsSubtypeStateControlling = value
            End Set
        End Property

        Private _SubtypeRelationships As New List(Of XMLModel.SubtypeRelationship)
        Public Property SubtypeRelationships() As List(Of XMLModel.SubtypeRelationship)
            Get
                Return Me._SubtypeRelationships
            End Get
            Set(ByVal value As List(Of XMLModel.SubtypeRelationship))
                Me._SubtypeRelationships = value
            End Set
        End Property

        <DebuggerBrowsable(DebuggerBrowsableState.Never)>
        Private _StoreFactCoordinates As Boolean
        <XmlAttribute()>
        Public Property StoreFactCoordinates As Boolean
            Get
                Return Me._StoreFactCoordinates
            End Get
            Set(ByVal value As Boolean)
                Me._StoreFactCoordinates = value
            End Set
        End Property

        <DebuggerBrowsable(DebuggerBrowsableState.Never)>
        Private _ModelElementFlags As New List(Of FBM.ModelElementFlag)
        <XmlElement()>
        Public Property ModelElementFlags() As List(Of FBM.ModelElementFlag)
            Get
                Return Me._ModelElementFlags
            End Get
            Set(ByVal value As List(Of FBM.ModelElementFlag))
                Me._ModelElementFlags = value
            End Set
        End Property

        ''' <summary>
        ''' At this stage uses exactly the same classes as in the FBM.Model.
        ''' </summary>
        ''' <returns></returns>
        <XmlElement>
        Public Property DerivationRule As DerivationRule

        Public Shadows Function Equals(ByVal other As FactType) As Boolean Implements System.IEquatable(Of FactType).Equals

            If Me.Id = other.Id Then
                Return True
            Else
                Return False
            End If

        End Function
    End Class

End Namespace
