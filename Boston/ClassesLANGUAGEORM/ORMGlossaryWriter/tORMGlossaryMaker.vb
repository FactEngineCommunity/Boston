Imports System.IO
Imports System.Web.UI
Imports System.Reflection

Namespace FBM

    Public Class ORMGlossaryMaker

        Private SW As New StringWriter
        Public HTW As New HtmlTextWriter(Me.SW)

        ''' <summary>
        ''' Constructor.
        ''' </summary>
        Public Sub New()
            Call Me.Reset()
        End Sub

        ''' <summary>
        ''' Creates the HTML Glossary. See also Me.New and Me.Reset
        ''' </summary>
        ''' <returns></returns>
        Public Function Create() As String

            'Header
            Me.HTW.Write(vbCrLf)
            Me.HTW.Write(Richmond.publicFunctions.ReadEmbeddedRessourceToString(Assembly.GetExecutingAssembly, "glossary.div.header.txt"))
            Me.HTW.Write(vbCrLf)

            'Overall body below header
            Me.HTW.AddAttribute("postition", "absolute")
            Me.HTW.AddAttribute("top", "60px")
            Me.HTW.AddAttribute("bottom", "7px")
            Me.HTW.AddAttribute("width", "100%")
            Me.HTW.RenderBeginTag(HtmlTextWriterTag.Div)

            'Navcontainer
            Me.HTW.AddAttribute("id", "navcontainer")
            Me.HTW.AddStyleAttribute("top", "14px")
            Me.HTW.RenderBeginTag(HtmlTextWriterTag.Div)

            Me.HTW.Write(vbCrLf)
            Me.HTW.Write(Richmond.publicFunctions.ReadEmbeddedRessourceToString(Assembly.GetExecutingAssembly, "glossary.homelabellink.txt"))
            Me.HTW.Write(vbCrLf)

            Me.HTW.RenderEndTag() 'navcontainer

            Me.HTW.Write(vbCrLf)
            Me.HTW.Write(Richmond.publicFunctions.ReadEmbeddedRessourceToString(Assembly.GetExecutingAssembly, "glossary.styles.txt"))
            Me.HTW.Write(vbCrLf)

            'LeftSidebar
            Me.HTW.Write("<div class=" & Chr(34) & " glossary-sidebar" & Chr(34) & ">")
            'Index controls
            Me.HTW.Write(vbCrLf)
            Me.HTW.Write(Richmond.publicFunctions.ReadEmbeddedRessourceToString(Assembly.GetExecutingAssembly, "glossary.div.controls.txt"))
            Me.HTW.Write(vbCrLf)

            'Create the index
            Me.HTW.Write("<ol class=" & Chr(34) & " glossary-toc" & Chr(34) & ">")
            For Each lrModelObject In prApplication.WorkingModel.getModelObjects()
                Call Me.addIndexEntry(lrModelObject)
            Next
            Me.HTW.Write("</ol>") 'Index llist
            Me.HTW.Write("</div>") 'LeftSidebar      

            '=============================================
            'Glossary
            Me.HTW.Write(vbCrLf)
            Me.HTW.AddAttribute("Class", "glossary-doc hide-constraints hide-alternates hide-examples")
            Me.HTW.AddAttribute("id", "glossary-doc")
            Me.HTW.RenderBeginTag(HtmlTextWriterTag.Div)
            Me.HTW.RenderBeginTag(HtmlTextWriterTag.Dl)

            'Heading
            Me.HTW.RenderBeginTag(HtmlTextWriterTag.H1)
            Me.HTW.Write("Glossary")
            Me.HTW.RenderEndTag() 'H1

            For Each lrModelObject In prApplication.WorkingModel.getModelObjects()
                Select Case lrModelObject.GetType
                    Case GetType(FBM.ValueType)
                        Call Me.addValueTypeTerm(lrModelObject)
                    Case GetType(FBM.EntityType)
                        Call Me.addEntityTypeTerm(lrModelObject)
                End Select
                Me.HTW.WriteBreak()
                Me.HTW.WriteBreak()
            Next

            Me.HTW.RenderEndTag() 'DL
            Me.HTW.RenderEndTag() 'Div Glossary
            '=============================================

            Me.HTW.RenderEndTag() 'Overall body below header
            Me.HTW.RenderEndTag() '</body>
            Return Me.SW.ToString

        End Function

        ''' <summary>
        ''' Used for putting an entry in the index within the index list of the glossary.
        ''' Users click on the index entry to navigate to the relevant entry in the glossary.
        ''' </summary>
        ''' <param name="arModelObject"></param>
        Public Sub addIndexEntry(ByRef arModelObject As FBM.ModelObject)

            Dim lsIndexEntry = "<li><a href=" & Chr(34) & "./ Index.html#" & Trim(arModelObject.Id) & Chr(34) & " class=" & Chr(34) & "object_type" & Chr(34) & ">" & arModelObject.Id & "</a></li>"

            Me.HTW.Write(vbCrLf)
            Me.HTW.Write(lsIndexEntry)

        End Sub

        Public Sub addValueTypeTerm(ByRef arModelObject As FBM.ModelObject)

            '  <dt><a name="ABN" class="object_type">ABN</a> <span class="keyword">is written as </span><a href="./index.html#String" class="object_type">String</a></dt>
            Me.HTW.RenderBeginTag(HtmlTextWriterTag.Dt)

            Me.HTW.AddAttribute(HtmlTextWriterAttribute.Name, arModelObject.Id)
            Me.HTW.AddAttribute(HtmlTextWriterAttribute.Class, "object_type")
            Me.HTW.RenderBeginTag(HtmlTextWriterTag.A)
            Me.HTW.Write(arModelObject.Id)
            Me.HTW.RenderEndTag() 'A (ModelObject)

            Me.HTW.AddAttribute(HtmlTextWriterAttribute.Class, "keyword")
            Me.HTW.RenderBeginTag(HtmlTextWriterTag.Span)
            Me.HTW.Write(" is written as ")
            Me.HTW.RenderEndTag() 'SPAN (is written as)

            Me.HTW.AddAttribute(HtmlTextWriterAttribute.Class, "object_type")
            Me.HTW.RenderBeginTag(HtmlTextWriterTag.A)
            Me.HTW.Write(CType(arModelObject, FBM.ValueType).DataType.ToString)
            Me.HTW.RenderEndTag() 'A (DataType)

            Me.HTW.RenderEndTag() 'DT

        End Sub

        Public Sub addEntityTypeTerm(ByRef arModelObject As FBM.ModelObject)

            Dim lrEntityType = CType(arModelObject, FBM.EntityType)

            '  <dt><a name="ABN" class="object_type">ABN</a> <span class="keyword">is written as </span><a href="./index.html#String" class="object_type">String</a></dt>
            Me.HTW.RenderBeginTag(HtmlTextWriterTag.Dt)

            Me.HTW.AddAttribute(HtmlTextWriterAttribute.Name, arModelObject.Id)
            Me.HTW.AddAttribute(HtmlTextWriterAttribute.Class, "object_type")
            Me.HTW.RenderBeginTag(HtmlTextWriterTag.A)
            Me.HTW.Write(arModelObject.Id)
            Me.HTW.RenderEndTag() 'A (ModelObject)

            Me.HTW.AddAttribute(HtmlTextWriterAttribute.Class, "keyword")
            Me.HTW.RenderBeginTag(HtmlTextWriterTag.Span)
            Me.HTW.Write(" is identified by its ")
            Me.HTW.RenderEndTag() 'SPAN (is written as)

            If lrEntityType.ReferenceModeValueType IsNot Nothing Then
                Me.HTW.AddAttribute(HtmlTextWriterAttribute.Class, "object_type")
                Me.HTW.RenderBeginTag(HtmlTextWriterTag.A)
                Me.HTW.Write(lrEntityType.ReferenceModeValueType.Id)
                Me.HTW.RenderEndTag() 'A (ReferenceMode)
            End If

            Me.HTW.RenderEndTag() 'DT

        End Sub



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

        Public Sub VerbaliseModelObject(ByRef arModelObject As FBM.ModelObject)

            Me.HTW.AddAttribute(HtmlTextWriterAttribute.Class, "objectType")
            Me.HTW.AddAttribute(HtmlTextWriterAttribute.Href, "elementid:" & arModelObject.Id)
            Me.HTW.RenderBeginTag(HtmlTextWriterTag.A)
            Me.HTW.Write(arModelObject.Id)
            Me.HTW.RenderEndTag()

        End Sub

        Public Sub VerbaliseModelObjectLight(ByRef arModelObject As FBM.ModelObject)

            Me.HTW.AddAttribute(HtmlTextWriterAttribute.Class, "quantifierLight")
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

            Me.HTW.BeginRender()
            Me.HTW.Write(Richmond.publicFunctions.ReadEmbeddedRessourceToString(Assembly.GetExecutingAssembly, "glossary.head.txt"))
            Me.HTW.Write(vbCrLf)
            Me.HTW.AddStyleAttribute("text-align", "left")
            Me.HTW.AddStyleAttribute("heigth", "100%")
            Me.HTW.RenderBeginTag(HtmlTextWriterTag.Body)

        End Sub

    End Class

End Namespace
