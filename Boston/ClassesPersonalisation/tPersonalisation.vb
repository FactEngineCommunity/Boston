Imports Newtonsoft.Json

Namespace Personalisation
    Public Class Profile

        <JsonIgnore>
        Private _UserId As String

        <JsonProperty>
        Public Property UserId As String
            Get
                Return Me._UserId
            End Get
            Set(value As String)
                Me._UserId = value
            End Set
        End Property

        <JsonIgnore>
        Private _LogoFileLocation As String

        <JsonProperty>
        Public Property LogoFileLocation As String
            Get
                Return Me._LogoFileLocation
            End Get
            Set(value As String)
                Me._LogoFileLocation = value
            End Set
        End Property

        <JsonIgnore>
        Private _DefaultOSMTaskId As String

        <JsonProperty>
        Public Property DefaultOSMTaskId As String
            Get
                Return Me._DefaultOSMTaskId
            End Get
            Set(value As String)
                Me._DefaultOSMTaskId = value
            End Set
        End Property

        <JsonIgnore>
        Private _DefaultBostonApplicationId As String

        <JsonProperty>
        Public Property DefaultBostonApplicationId As String
            Get
                Return Me._DefaultBostonApplicationId
            End Get
            Set(value As String)
                Me._DefaultBostonApplicationId = value
            End Set
        End Property

        <JsonIgnore>
        Private _ShowEnterpriseExplorer As Boolean = True

        <JsonProperty>
        Public Property ShowEnterpriseExplorer As Boolean
            Get
                Return Me._ShowEnterpriseExplorer
            End Get
            Set(value As Boolean)
                Me._ShowEnterpriseExplorer = value
            End Set
        End Property

        <JsonIgnore>
        Private _ShowVirtualAnalyst As Boolean = True

        <JsonProperty>
        Public Property ShowVirtualAnalyst As Boolean
            Get
                Return Me._ShowVirtualAnalyst
            End Get
            Set(value As Boolean)
                Me._ShowVirtualAnalyst = value
            End Set
        End Property

        <JsonIgnore>
        Private _ShowTheBox As Boolean = True

        <JsonProperty>
        Public Property ShowTheBox As Boolean
            Get
                Return Me._ShowTheBox
            End Get
            Set(value As Boolean)
                Me._ShowTheBox = value
            End Set
        End Property

    End Class

End Namespace
