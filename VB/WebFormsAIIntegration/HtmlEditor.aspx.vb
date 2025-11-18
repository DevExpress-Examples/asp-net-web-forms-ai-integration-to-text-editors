Imports DevExpress.AIIntegration.Extensions
Imports DevExpress.Web
Imports WebFormsAIIntegration.Models
Imports System
Imports System.IO
Imports System.Linq
Imports System.Text.Json

Namespace WebFormsAIIntegration

    Public Partial Class HtmlEditor
        Inherits Web.UI.Page

        Protected Sub Page_Init(ByVal sender As Object, ByVal e As EventArgs)
            ASPxHtmlEditor1.CreateDefaultRibbonTabs(True)
            AddAIItem()
        End Sub

        Private Sub AddAIItem()
            Dim aiTab = New RibbonTab("AI Assistant")
            Dim group = aiTab.Groups.Add("")
            Dim basicItems = New String() {"Summarize", "Explain", "Proofread", "Expand", "Shorten"}.[Select](Function(x) New RibbonButtonItem("AI:" & x, x)).ToArray()
            Dim translateMenu = New RibbonDropDownButtonItem("Translate")
            Dim translateItems = New RibbonDropDownButtonItem() {New RibbonDropDownButtonItem("AI:Translate-en", "English"), New RibbonDropDownButtonItem("AI:Translate-de", "German"), New RibbonDropDownButtonItem("AI:Translate-fr", "French")}
            translateMenu.Items.Add(translateItems)
            Dim changeStyleMenu = New RibbonDropDownButtonItem("ChangeStyle", "Change Style")
            Dim changeStyleItems = [Enum].GetNames(GetType(WritingStyle)).[Select](Function(x) New RibbonDropDownButtonItem("AI:ChangeStyle-" & x, x)).ToArray()
            changeStyleMenu.Items.Add(changeStyleItems)
            Dim changeToneMenu = New RibbonDropDownButtonItem("ChangeTone", "Change Tone")
            Dim changeToneItems = [Enum].GetNames(GetType(ToneStyle)).[Select](Function(x) New RibbonDropDownButtonItem("AI:ChangeTone-" & x, x)).ToArray()
            changeToneMenu.Items.Add(changeToneItems)
            group.Items.Add(basicItems)
            group.Items.Add(translateMenu)
            group.Items.Add(changeStyleMenu)
            group.Items.Add(changeToneMenu)
            ASPxHtmlEditor1.RibbonTabs.Add(aiTab)
        End Sub

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs)
            If Not IsPostBack Then
                Dim htmlContent = File.ReadAllText(Server.MapPath("Documents/sample.html"))
                ASPxHtmlEditor1.Html = htmlContent
            End If
        End Sub

        Protected Async Sub ASPxCallback1_Callback(ByVal source As Object, ByVal e As CallbackEventArgs)
            Dim callback As ASPxCallback = CType(source, ASPxCallback)
            Dim aiRequestData = JsonSerializer.Deserialize(Of AIRequestData)(e.Parameter)
            Dim aiResponse = Await GetResponseAsync(aiRequestData)
            callback.JSProperties("cpText") = aiResponse
        End Sub
    End Class
End Namespace
