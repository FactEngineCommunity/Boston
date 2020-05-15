Imports System.Collections.Generic
Imports System.Xml

''' <summary>
''' 
''' </summary>
''' <remarks></remarks>
Public Class tRecentlyViewedNodes
    Dim RecentNodes As New List(Of String)            ' List of <TAB> delimited strings
    Dim MaxNodes As Integer = 4                 ' Maximum number of nodes to keep track of
    Dim ElementName As String = "RecentNode"    ' Element name to save the recent nodes

    ''' <summary>
    ''' Returns the list of recent nodes (TAB delimited strings)
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetNodes() As List(Of String)
        GetNodes = RecentNodes
    End Function

    ''' <summary>
    ''' This sub adds the TAB delimited path to TreeNode into the list of 
    ''' recent nodes
    ''' </summary>
    ''' <param name="node"></param>
    ''' <remarks></remarks>
    Public Sub AddNode(ByRef node As TreeNode)
        Dim s As String = ""

        ' Walk to the root of the TreeView and create full <TAB> delimited path to the node
        While Not node Is Nothing
            If s.Length = 0 Then
                s = node.Text
            Else
                s = node.Text + CChar(vbTab) + s
            End If
            node = node.Parent
        End While

        ' If node already exist move it to the end
        If RecentNodes.Contains(s) Then
            RecentNodes.Remove(s)
            RecentNodes.Add(s)
        Else
            RecentNodes.Add(s)
        End If
        If RecentNodes.Count > MaxNodes Then
            RecentNodes.RemoveAt(0)
        End If
    End Sub

    ''' <summary>
    ''' This function returns the Node in TreeView 
    ''' based on TAB delimited path to the node
    ''' </summary>
    ''' <param name="path">TAB delimited path to the node</param>
    ''' <param name="tv">TreeView control to find a node in</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function NavigateTo(ByVal path As String, ByRef tv As TreeView)
        Dim strings As String()
        Dim node As TreeNode = Nothing
        Dim nodes As TreeNodeCollection = tv.Nodes
        Dim foundNode As Boolean

        strings = path.Split(New [Char]() {CChar(vbTab)})
        For Each s As String In strings
            foundNode = False
            For Each n As TreeNode In nodes
                If n.Text = s Then
                    node = n
                    nodes = node.Nodes
                    foundNode = True
                End If
            Next
            If Not foundNode Then
                NavigateTo = Nothing
                Exit Function
            End If
        Next
        NavigateTo = node
    End Function

    ''' <summary>
    ''' Saves the list of recent nodes into XML file
    ''' </summary>
    ''' <param name="path"></param>
    ''' <remarks></remarks>
    Public Sub Serialize(ByVal path As String)
        Try
            Using writer As New XmlTextWriter(path, Nothing)
                writer.WriteStartDocument()
                writer.WriteStartElement("root")
                For Each s As String In RecentNodes
                    writer.WriteStartElement(ElementName)
                    writer.WriteString(s)
                    writer.WriteEndElement()
                Next
                writer.WriteEndElement()
                writer.WriteEndDocument()
            End Using
        Catch e As Exception
            MsgBox(e.Message, MsgBoxStyle.Critical)
        End Try
    End Sub

    ''' <summary>
    ''' Loads the list of recent nodes from the XML file
    ''' </summary>
    ''' <param name="path"></param>
    ''' <remarks></remarks>
    Public Sub Deserialize(ByVal path As String)
        RecentNodes.Clear()
        Try
            Using reader As New XmlTextReader(path)
                reader.Read()
                reader.ReadStartElement("root")
                While reader.IsStartElement(ElementName)
                    reader.ReadStartElement(ElementName)
                    RecentNodes.Add(reader.ReadString())
                    reader.ReadEndElement()
                End While
            End Using
        Catch e As Exception
            MsgBox(e.Message, MsgBoxStyle.Critical)
        End Try
    End Sub
End Class

