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
            Me.HTW.AddStyleAttribute("postition", "absolute")
            Me.HTW.AddStyleAttribute("top", "60px")
            Me.HTW.AddStyleAttribute("bottom", "7px")
            Me.HTW.AddStyleAttribute("width", "100%")
            Me.HTW.RenderBeginTag(HtmlTextWriterTag.Div)

            'Navcontainer
            Me.HTW.AddAttribute("id", "navcontainer")
            Me.HTW.AddStyleAttribute("top", "14px")
            Me.HTW.RenderBeginTag(HtmlTextWriterTag.Div)

            'Me.HTW.Write(vbCrLf)
            'Me.HTW.Write(Richmond.publicFunctions.ReadEmbeddedRessourceToString(Assembly.GetExecutingAssembly, "glossary.homelabellink.txt"))
            'Me.HTW.Write(vbCrLf)

            Me.HTW.RenderEndTag() 'navcontainer

            Me.HTW.Write(vbCrLf)
            Me.HTW.Write(Richmond.publicFunctions.ReadEmbeddedRessourceToString(Assembly.GetExecutingAssembly, "glossary.styles.txt"))
            Me.HTW.Write(vbCrLf)

            'LeftSidebar
            Me.HTW.Write("<div class=" & Chr(34) & " glossary-sidebar" & Chr(34) & ">")
            'Index controls
            'Me.HTW.Write(vbCrLf)
            'Me.HTW.Write(Richmond.publicFunctions.ReadEmbeddedRessourceToString(Assembly.GetExecutingAssembly, "glossary.div.controls.txt"))
            'Me.HTW.Write(vbCrLf)

            'Create the index
            Me.HTW.Write("<ol class=" & Chr(34) & "glossary-index" & Chr(34) & ">")
            Dim larModelObject = prApplication.WorkingModel.getModelObjects().OrderBy(Function(x) x.Id)
            For Each lrModelObject In larModelObject
                Call Me.addIndexEntry(lrModelObject)
            Next
            Me.HTW.Write("</ol>") 'Index llist
            Me.HTW.Write("</div>") 'LeftSidebar      

            '=============================================
            'Glossary
            Me.HTW.Write(vbCrLf)
            Me.HTW.AddAttribute("Class", "glossary")
            Me.HTW.AddAttribute("id", "glossary")
            Me.HTW.RenderBeginTag(HtmlTextWriterTag.Div)
            Me.HTW.RenderBeginTag(HtmlTextWriterTag.Dl)

            'Heading
            Me.HTW.RenderBeginTag(HtmlTextWriterTag.H1)
            Me.HTW.Write("Glossary for the " & prApplication.WorkingModel.Name & " model")
            Me.HTW.RenderEndTag() 'H1

            For Each lrModelObject In larModelObject
                Select Case lrModelObject.GetType
                    Case GetType(FBM.ValueType)
                        Call Me.addValueTypeTerm(lrModelObject)
                    Case GetType(FBM.EntityType)
                        Call Me.addEntityTypeTerm(lrModelObject)
                    Case GetType(FBM.FactType)
                        Call Me.addFactTypeEntry(CType(lrModelObject, FBM.FactType))
                End Select

                For Each lrFactTypeReading In lrModelObject.getOutgoingFactTypeReadings
                    Call Me.addFactTypeReadingEntry(lrFactTypeReading)
                Next

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

            Dim lsIndexEntry = "<li><a href=" & Chr(34) & "./Index.html#" & Trim(arModelObject.Id) & Chr(34) & " class=" & Chr(34) & "object_type" & Chr(34) & ">" & arModelObject.Id & "</a></li>"

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

            Me.HTW.AddAttribute(HtmlTextWriterAttribute.Class, "predicate")
            Me.HTW.RenderBeginTag(HtmlTextWriterTag.Span)
            Me.HTW.Write(" is written as ")
            Me.HTW.RenderEndTag() 'SPAN (is written as)

            Me.HTW.AddAttribute(HtmlTextWriterAttribute.Class, "object_type")
            Me.HTW.RenderBeginTag(HtmlTextWriterTag.A)
            Me.HTW.Write(CType(arModelObject, FBM.ValueType).DataType.ToString)
            Me.HTW.RenderEndTag() 'A (DataType)

            Dim liInd = 0
            For Each lsInstance In arModelObject.Instance
                If liInd = 0 Then
                    Me.HTW.WriteBreak()
                    Me.HTW.WriteBreak()
                    Call Me.VerbaliseHeading("Example/s")
                    Me.HTW.WriteBreak()
                End If
                Me.HTW.AddAttribute(HtmlTextWriterAttribute.Class, "glossary-example")
                Me.HTW.RenderBeginTag(HtmlTextWriterTag.Dd)
                Call Me.VerbaliseIndent()
                Me.HTW.Write(lsInstance)
                Me.HTW.RenderEndTag() 'DD
                liInd += 1
            Next

            Me.HTW.RenderEndTag() 'DT
            Me.HTW.WriteBreak()

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

            Me.HTW.AddAttribute(HtmlTextWriterAttribute.Class, "predicate")
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

        Public Sub addFactTypeEntry(ByRef arFactType As FBM.FactType)


            '  <dt><a name="ABN" class="object_type">ABN</a> <span class="predicate">is written as </span>
            '<a href = "./index.html#String" Class="object_type">String</a></dt>
            Me.HTW.RenderBeginTag(HtmlTextWriterTag.Dt)

            Me.HTW.AddAttribute(HtmlTextWriterAttribute.Name, arFactType.Id)
            Me.HTW.AddAttribute(HtmlTextWriterAttribute.Class, "object_type")
            Me.HTW.RenderBeginTag(HtmlTextWriterTag.A)
            Me.HTW.Write(arFactType.Id)
            Me.HTW.RenderEndTag() 'A (ModelObject)

            Me.HTW.Write(" is where ")

            If arFactType.FactTypeReading.Count > 0 Then


                For Each lrPredicatePart In arFactType.FactTypeReading(0).PredicatePart

                    Me.HTW.AddAttribute(HtmlTextWriterAttribute.Href, "./Index.html#" & lrPredicatePart.Role.JoinedORMObject.Id)
                    Me.HTW.AddAttribute(HtmlTextWriterAttribute.Class, "object_type")
                    Me.HTW.RenderBeginTag(HtmlTextWriterTag.A)
                    Me.HTW.Write(lrPredicatePart.Role.JoinedORMObject.Id)
                    Me.HTW.RenderEndTag() 'A (ModelObject1)

                    If lrPredicatePart.PredicatePartText <> "" Then
                        Me.HTW.Write(" " & lrPredicatePart.PredicatePartText & " ")
                    End If

                Next
            End If

            Me.HTW.RenderEndTag() 'DT


        End Sub

        Public Sub addFactTypeReadingEntry(ByRef arFactTypeReading As FBM.FactTypeReading)

            '  <dd>
            ' <div Class="glossary-facttype"><div class="glossary-reading"><span class="copula"><a href="./index.html#ABN" class="object_type">ABN</a>
            ' Is of <a href="./index.html#Company" class="object_type">Company</a></span>
            '</div><div class="glossary-alternates">(alternatively: <div Class="glossary-reading"><span class="copula"><a href="./index.html#Company" class="object_type">Company</a> has <a href="./index.html#ABN" class="object_type">ABN</a></span></div>)</div></div>
            '<ul Class="glossary-constraints">
            '<div Class="glossary-constraint"><span class="copula"><a href="./index.html#Company" class="object_type">Company</a> has <span class="keyword">one</span> <a href="./index.html#ABN" class="object_type">ABN</a></span></div>
            '<div Class="glossary-constraint"><span class="copula"><a href="./index.html#ABN" class="object_type">ABN</a> Is of <span class="keyword">at most one</span> <a href="./index.html#Company" class="object_type">Company</a></span></div>
            '</ul>
            ' </dd>

            Me.HTW.RenderBeginTag(HtmlTextWriterTag.Dd)

            Me.HTW.AddAttribute(HtmlTextWriterAttribute.Class, "glossary-facttype")
            Me.HTW.RenderBeginTag(HtmlTextWriterTag.Div)

            Me.HTW.AddAttribute(HtmlTextWriterAttribute.Class, "glossary-reading")
            Me.HTW.RenderBeginTag(HtmlTextWriterTag.Div)

            For Each lrPredicatePart In arFactTypeReading.PredicatePart

                Me.HTW.AddAttribute(HtmlTextWriterAttribute.Href, "./Index.html#" & lrPredicatePart.Role.JoinedORMObject.Id)
                Me.HTW.AddAttribute(HtmlTextWriterAttribute.Class, "object_type")
                Me.HTW.RenderBeginTag(HtmlTextWriterTag.A)
                Me.HTW.Write(lrPredicatePart.Role.JoinedORMObject.Id)
                Me.HTW.RenderEndTag() 'A (ModelObject1)

                If lrPredicatePart.PredicatePartText <> "" Then
                    Me.HTW.Write(" " & lrPredicatePart.PredicatePartText & " ")
                End If

            Next

            If arFactTypeReading.PredicatePart.Count > 2 Then
                Call Me.VerbaliseHeading(" is defined as a ")

                Me.HTW.AddAttribute(HtmlTextWriterAttribute.Href, "./Index.html#" & arFactTypeReading.FactType.Id)
                Me.HTW.AddAttribute(HtmlTextWriterAttribute.Class, "object_type")
                Me.HTW.RenderBeginTag(HtmlTextWriterTag.A)
                Me.HTW.Write(arFactTypeReading.FactType.Id)
                Me.HTW.RenderEndTag() 'A (ModelObject1)

            End If

            Me.HTW.RenderEndTag() 'Div glossary-reading
            Me.HTW.RenderEndTag() 'Div glossary-facttype

            Me.HTW.RenderEndTag() 'DD

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
