Imports System.IO
Imports System.Web.UI

Namespace FBM

    Public Class ORMVerbailser

        Private SW As New StringWriter
        Public HTW As New HtmlTextWriter(Me.SW)

        Public Head As String = "<head>" & vbCrLf &
                       "<title>ORM2 Verbalization</title>" & vbCrLf &
                       "<style type=" & Chr(34) & "text/css" & Chr(34) & ">" & vbCrLf &
                       "body, table { font-family: Tahoma; font-size: 9pt; color: DarkGreen; font-weight: normal; }" & vbCrLf &
                       "body { background-color: #F5F5F5; padding: 0em .1em; }" & vbCrLf &
                       "FTR { padding: 9; }" & vbCrLf &
                       "table.hidden, tr.hidden, td.hidden { margin: 0em; padding: 0em; border-collapse: collapse;}" & vbCrLf &
                       "td.hidden { vertical-align: top; }" & vbCrLf &
                       "table.hidden { display:inline; }" & vbCrLf &
                       "a {text-decoration:none; }" & vbCrLf &
                       "a:hover {background-color:infobackground; }" & vbCrLf &
                       ".heading { color: Black; font-weight: bold; }" & vbCrLf &
                       ".objectType { color: Purple; font-weight: normal; margin-bottom: 16px;}" & vbCrLf &
                       ".objectTypeLight { color: #D5D5D5;; font-weight: normal; }" & vbCrLf &
                       ".objectTypeMissing { color: Purple; font-weight: normal; }" & vbCrLf &
                       ".referenceMode { color: Brown; font-weight: normal; }" & vbCrLf &
                       ".predicateText { color: DarkGreen; font-weight: normal; }" & vbCrLf &
                       ".blacktext { color: Black; font-weight: normal; }" & vbCrLf &
                       ".quantifier { color: MediumBlue; font-weight: bold; }" & vbCrLf &
                       ".quantifierLight { color: Blue; font-weight: normal; }" & vbCrLf &
                       ".textLightGray { color: LightGray; font-weight: normal; }" & vbCrLf &
                       ".primaryErrorReport { color: red; font-weight: bolder; }" & vbCrLf &
                       ".secondaryErrorReport { color: red; }" & vbCrLf &
                       ".verbalization { }" & vbCrLf &
                       ".indent { margin.left: 20px; position: relative; }" & vbCrLf &
                       ".smallIndent { margin.left: 8px; position: relative;}" & vbCrLf &
                       ".listSeparator { color: windowtext; font-weight: 200;}" & vbCrLf &
                       ".logicalOperator { color: MediumBlue; font-weight: bold;}" & vbCrLf &
                       ".note { color: Black; font-style: italic; font-weight: normal; }" & vbCrLf &
                       ".definition { color: Black; font-style: italic; font-weight: normal; }" & vbCrLf &
                       ".notAvailable { font-style: italic; }" & vbCrLf &
                       ".instance { color: Brown; font-weight: normal; }" & vbCrLf &
                       ".value { color: Purple; font-weight: normal; margin-bottom: 16px;}" & vbCrLf &
                       "</style>" & vbCrLf &
                       "</head>"

        Public Sub VerbaliseBlackText(ByVal asText As String)

            Me.HTW.AddAttribute(HtmlTextWriterAttribute.Class, "blacktext")
            Me.HTW.RenderBeginTag(HtmlTextWriterTag.Span)
            Me.HTW.Write(asText)
            Me.HTW.RenderEndTag()

        End Sub

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

        Public Sub VerbaliseTextLightGray(ByVal asText As String)

            Me.HTW.AddAttribute(HtmlTextWriterAttribute.Class, "textLightGray")
            Me.HTW.RenderBeginTag(HtmlTextWriterTag.Span)
            Me.HTW.Write(asText)
            Me.HTW.RenderEndTag()

        End Sub

        Public Sub VerbaliseModelObject(ByRef arModelObject As FBM.ModelObject,
                                        Optional ByVal asSubscriptText As String = Nothing)

            If arModelObject IsNot Nothing Then
                Me.HTW.AddAttribute(HtmlTextWriterAttribute.Class, "objectType")
                Me.HTW.AddAttribute(HtmlTextWriterAttribute.Href, "elementid:" & arModelObject.Id)
                Me.HTW.RenderBeginTag(HtmlTextWriterTag.A)
                Me.HTW.Write(arModelObject.Id)
                Me.HTW.RenderEndTag()
                If asSubscriptText IsNot Nothing Then
                    Me.HTW.AddAttribute(HtmlTextWriterAttribute.Style, "font-size:smaller;")
                    Me.HTW.RenderBeginTag(HtmlTextWriterTag.Sub)
                    Me.HTW.Write(asSubscriptText)
                    Me.HTW.RenderEndTag()
                End If
            Else
                Me.HTW.Write("''")
            End If


        End Sub

        Public Sub VerbaliseModelObjectLight(ByRef arModelObject As FBM.ModelObject)

            Me.HTW.AddAttribute(HtmlTextWriterAttribute.Class, "quantifierLight")
            Me.HTW.AddAttribute(HtmlTextWriterAttribute.Href, "elementid:" & arModelObject.Id)
            Me.HTW.RenderBeginTag(HtmlTextWriterTag.A)
            Me.HTW.Write(arModelObject.Id)
            Me.HTW.RenderEndTag()

        End Sub

        Public Sub VerbaliseModelObjectLightGray(ByRef arModelObject As FBM.ModelObject)

            Me.HTW.AddAttribute(HtmlTextWriterAttribute.Class, "objectTypeLight")
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

        Public Sub VerbaliseValue(ByVal asText As String)

            Me.HTW.AddAttribute(HtmlTextWriterAttribute.Class, "value")
            Me.HTW.RenderBeginTag(HtmlTextWriterTag.Span)
            Me.HTW.Write(asText)
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
