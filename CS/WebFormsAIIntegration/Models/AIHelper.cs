using DevExpress.AIIntegration;
using DevExpress.AIIntegration.Extensions;
using System;
using System.Threading.Tasks;
using System.Web;

namespace WebFormsAIIntegration.Models {
    public static class AIHelper {
        public class AIRequestData {
            public string Command { get; set; }
            public string Text { get; set; }
        }

        private static AIExtensionsContainerDefault AIService {
            get {
                return HttpContext.Current.Application["AIService"] as AIExtensionsContainerDefault;
            }
        }

        public static async Task<string> GetResponseAsync(AIRequestData aiRequestData) {
            TextResponse result = null;
            string style = "";
            var commandParts = aiRequestData.Command.Split('-');
            if(commandParts.Length == 2) {
                style = commandParts[1];
            }
            result = aiRequestData.Command switch {
                "Summarize" => await AIService.AbstractiveSummaryAsync(new AbstractiveSummaryRequest(aiRequestData.Text)),
                "Explain" => await AIService.ExplainAsync(new ExplainRequest(aiRequestData.Text)),
                "Proofread" => await AIService.ProofreadAsync(new ProofreadRequest(aiRequestData.Text)),
                "Expand" => await AIService.ExpandAsync(new ExpandRequest(aiRequestData.Text)),
                "Shorten" => await AIService.ShortenAsync(new ShortenRequest(aiRequestData.Text)),
                string s when s.StartsWith("Translate") => await AIService.TranslateAsync(new TranslateRequest(aiRequestData.Text, style)),
                string s when s.StartsWith("ChangeStyle") => await AIService.ChangeStyleAsync(new ChangeStyleRequest(aiRequestData.Text, (WritingStyle)Enum.Parse(typeof(WritingStyle), style))),
                string s when s.StartsWith("ChangeTone") => await AIService.ChangeToneAsync(new ChangeToneRequest(aiRequestData.Text, (ToneStyle)Enum.Parse(typeof(ToneStyle), style))),
                _ => null
            };
            string text = "";
            if(result.IsCompleted)
                text = result.Response;
            else if(!result.IsRestrictedOrFailed) {
                string translatedText = result.Response;
                while(result.IsContinuationRequired) {
                    await result.ContinueAsync();
                    translatedText += result.Response;
                }
                text = translatedText;
            }
            else {
                text = "An error occurred while processing your request";
            }
            return text;
        }
    }
}
