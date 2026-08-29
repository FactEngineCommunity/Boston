Imports System.Xml.Serialization

Namespace FBM

    ''' <summary>
    ''' Wrapper for the set of path components that make up a derivation.
    '''
    ''' In practice this minimal importer only needs RolePath.
    ''' NORMA may extend PathComponents in future schema versions; this wrapper keeps
    ''' deserialization stable.
    ''' </summary>
    Public Class PathComponents

        ''' <summary>
        ''' All sibling RolePaths belonging to these PathComponents.
        ''' </summary>
        <XmlElement("RolePath")>
        Public Property RolePaths As New List(Of RolePath)

        ''' <summary>
        ''' Compatibility accessor for existing code that expects one RolePath.
        ''' </summary>
        <XmlIgnore>
        Public Property RolePath As RolePath
            Get
                If Me.RolePaths Is Nothing OrElse Me.RolePaths.Count = 0 Then
                    Return Nothing
                End If

                Return Me.RolePaths(0)
            End Get
            Set(ByVal value As RolePath)
                If Me.RolePaths Is Nothing Then
                    Me.RolePaths = New List(Of RolePath)
                Else
                    Me.RolePaths.Clear()
                End If

                If value IsNot Nothing Then
                    Me.RolePaths.Add(value)
                End If
            End Set
        End Property

    End Class

End Namespace
