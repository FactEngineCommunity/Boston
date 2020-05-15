Imports System.IO
Imports System.Web.UI

Namespace FBM

    Public Class ORMVerbailser

        Private SW As New StringWriter
        Public HTW As New HtmlTextWriter(Me.SW)

        Public Head As String = "<head>" & vbCrLf & _
                       "<title>ORM2 Verbalization</title>" & vbCrLf & _
                       "<style type=" & Chr(34) & "text/css" & Chr(34) & ">" & vbCrLf & _
                       "body, table { font-family: Tahoma; font-size: 9pt; color: DarkGreen; font-weight: normal; }" & vbCrLf & _
                       "body { padding: 0em .1em; }" & vbCrLf & _
                       "table.hidden, tr.hidden, td.hidden { margin: 0em; padding: 0em; border-collapse: collapse;}" & vbCrLf & _
                       "td.hidden { vertical-align: top; }" & vbCrLf & _
                       "table.hidden { display:inline; }" & vbCrLf & _
                       "a {text-decoration:none; }" & vbCrLf & _
                       "a:hover {background-color:infobackground; }" & vbCrLf & _
                       ".heading { color: Black; font-weight: bold; }" & vbCrLf & _
                       ".objectType { color: Purple; font-weight: normal; }" & vbCrLf & _
                       ".objectTypeMissing { color: Purple; font-weight: normal; }" & vbCrLf & _
                       ".referenceMode { color: Brown; font-weight: normal; }" & vbCrLf & _
                       ".predicateText { color: DarkGreen; font-weight: normal; }" & vbCrLf & _
                       ".quantifier { color: MediumBlue; font-weight: bold; }" & vbCrLf & _
                       ".quantifierLight { color: Blue; font-weight: notmal; }" & vbCrLf & _
                       ".primaryErrorReport { color: red; font-weight: bolder; }" & vbCrLf & _
                       ".secondaryErrorReport { color: red; }" & vbCrLf & _
                       ".verbalization { }" & vbCrLf & _
                       ".indent { margin.left: 20px; position: relative; }" & vbCrLf & _
                       ".smallIndent { margin.left: 8px; position: relative;}" & vbCrLf & _
                       ".listSeparator { color: windowtext; font-weight: 200;}" & vbCrLf & _
                       ".logicalOperator { color: MediumBlue; font-weight: bold;}" & vbCrLf & _
                       ".note { color: Black; font-style: italic; font-weight: normal; }" & vbCrLf & _
                       ".definition { color: Black; font-style: italic; font-weight: normal; }" & vbCrLf & _
                       ".notAvailable { font-style: italic; }" & vbCrLf & _
                       ".instance { color: Brown; font-weight: normal; }" & vbCrLf & _
                       "</style>" & vbCrLf & _
                       "</head>"

        Public Sub VerbaliseError(ByVal asPredicateText As String)

            Me.HTW.AddAttribute(HtmlTextWriterAttribute.Class, "primaryErrorReport")
            Me.HTW.RenderBeginTag(HtmlTextWriterTag.Span)
            Me.HTW.Write(asPredicateText)
            Me.HTW.RenderEndTag()

        End Sub

        Public Sub VerbaliseHeading(ByVal asHeading As String)

            Me.HTW.AddAttribute(HtmlTextWriterAttribute.Class, "heading")
            Me.HTW.RenderBeginTag(HtmlTextWriterTag.Span)
            Me.HTW.Write(asHeading)
            Me.HTW.RenderEndTag()

        End Sub

        Public Sub VerbaliseIndent()

            Me.HTW.AddAttribute(HtmlTextWriterAttribute.Class, "indent")
            Me.HTW.RenderBeginTag(HtmlTextWriterTag.Span)
            Me.HTW.RenderEndTag()

        End Sub

        Public Sub VerbalisePredicateText(ByVal asErrorText As String)

            Me.HTW.AddAttribute(HtmlTextWriterAttribute.Class, "predicateText")
            Me.HTW.RenderBeginTag(HtmlTextWriterTag.Span)
            Me.HTW.Write(asErrorText)
            Me.HTW.RenderEndTag()

        End Sub


        Public Sub VerbaliseQuantifier(ByVal asQuantifier As String)

            Me.HTW.AddAttribute(HtmlTextWriterAttribute.Class, "quantifier")
            Me.HTW.RenderBeginTag(HtmlTextWriterTag.Span)
            Me.HTW.Write(asQuantifier)
            Me.HTW.RenderEndTag()

        End Sub

        Public Sub VerbaliseQuantifierLight(ByVal asQuantifier As String)

            Me.HTW.AddAttribute(HtmlTextWriterAttribute.Class, "quantifierLight")
            Me.HTW.RenderBeginTag(HtmlTextWriterTag.Span)
            Me.HTW.Write(asQuantifier)
            Me.HTW.RenderEndTag()

        End Sub

        Public Sub VerbaliseModelObject(ByRef arModelObject As FBM.ModelObject)

            Me.HTW.AddAttribute(HtmlTextWriterAttribute.Class, "objectType")
            Me.HTW.AddAttribute(HtmlTextWriterAttribute.Href, "elementid:" & arModelObject.Id)
            Me.HTW.RenderBeginTag(HtmlTextWriterTag.A)
            Me.HTW.Write(arModelObject.Id)
            Me.HTW.RenderEndTag()

        End Sub

        Public Sub VerbaliseSeparator(ByVal asSeparator As String)

            Me.HTW.AddAttribute(HtmlTextWriterAttribute.Class, "listSeparator")
            Me.HTW.RenderBeginTag(HtmlTextWriterTag.Span)
            Me.HTW.Write(asSeparator)
            Me.HTW.RenderEndTag()

        End Sub

        Public Sub VerbaliseSubscript(ByVal asSubscriptText As String)

            Me.HTW.AddAttribute(HtmlTextWriterAttribute.Style, "font-size:smaller;")
            Me.HTW.RenderBeginTag(HtmlTextWriterTag.Sub)
            Me.HTW.Write(asSubscriptText)
            Me.HTW.RenderEndTag()

        End Sub

        Public Sub Reset()

            Me.SW = New StringWriter
            Me.HTW = New HtmlTextWriter(Me.SW)
            Me.HTW.Write(Me.Head)

        End Sub

        Public Function Verbalise() As String

            Return Me.SW.ToString

        End Function

    End Class

End Namespace
