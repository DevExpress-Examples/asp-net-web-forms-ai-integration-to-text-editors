using DevExpress.AIIntegration.Extensions;
using DevExpress.Web;
using DevExpress.Web.ASPxRichEdit;
using WebFormsAIIntegration.Models;
using System;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace WebFormsAIIntegration {
    public partial class HtmlEditor : System.Web.UI.Page {

        protected void Page_Init(object sender, EventArgs e) {
            ASPxHtmlEditor1.CreateDefaultRibbonTabs(true);
            AddAIItem();
        }

        void AddAIItem() {
            var aiTab = new RibbonTab("AI Assistant");
            var group = aiTab.Groups.Add("");

            var basicItems = new string[] { "Summarize", "Explain", "Proofread", "Expand", "Shorten" }
                            .Select(x => new RibbonButtonItem("AI:" + x, x)).ToArray();

            var translateMenu = new RibbonDropDownButtonItem("Translate");
            var translateItems = new RibbonDropDownButtonItem[] { new RibbonDropDownButtonItem("AI:Translate-en", "English"),
                new RibbonDropDownButtonItem("AI:Translate-de", "German"), new RibbonDropDownButtonItem("AI:Translate-fr", "French") };
            translateMenu.Items.Add(translateItems);

            var changeStyleMenu = new RibbonDropDownButtonItem("ChangeStyle", "Change Style");
            var changeStyleItems = Enum.GetNames(typeof(WritingStyle)).Select(x => new RibbonDropDownButtonItem("AI:ChangeStyle-" + x, x)).ToArray();
            changeStyleMenu.Items.Add(changeStyleItems);

            var changeToneMenu = new RibbonDropDownButtonItem("ChangeTone", "Change Tone");
            var changeToneItems = Enum.GetNames(typeof(ToneStyle)).Select(x => new RibbonDropDownButtonItem("AI:ChangeTone-" + x, x)).ToArray();
            changeToneMenu.Items.Add(changeToneItems);

            group.Items.Add(basicItems);
            group.Items.Add(translateMenu);
            group.Items.Add(changeStyleMenu);
            group.Items.Add(changeToneMenu);

            ASPxHtmlEditor1.RibbonTabs.Add(aiTab);
        }

        protected void Page_Load(object sender, EventArgs e) {
            if(!IsPostBack) {
                var htmlContent = File.ReadAllText(Server.MapPath("Documents/sample.html"));
                ASPxHtmlEditor1.Html = htmlContent;
            }
        }

        protected async void ASPxCallback1_Callback(object source, CallbackEventArgs e) {
            ASPxCallback callback = (ASPxCallback)source;
            var aiRequestData = JsonSerializer.Deserialize<AIHelper.AIRequestData>(e.Parameter);
            var aiResponse = await AIHelper.GetResponseAsync(aiRequestData);

            callback.JSProperties["cpText"] = aiResponse;
        }
    }
}
