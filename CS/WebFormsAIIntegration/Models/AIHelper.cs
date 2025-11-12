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

        public static async Task<string> GetResponseAsync(AIRequestData commandParameter) {
            if(AIService == null) {
                return "The AI service is not available";
            }
            string style = "";
            var commandParts = commandParameter.Command.Split('-');
            if(commandParts.Length == 2) {
                style = commandParts[1];
            }
            TextResponse result = commandParameter.Command switch {
                "Summarize" => await AIService.AbstractiveSummaryAsync(new AbstractiveSummaryRequest(commandParameter.Text)),
                "Explain" => await AIService.ExplainAsync(new ExplainRequest(commandParameter.Text)),
                "Proofread" => await AIService.ProofreadAsync(new ProofreadRequest(commandParameter.Text)),
                "Expand" => await AIService.ExpandAsync(new ExpandRequest(commandParameter.Text)),
                "Shorten" => await AIService.ShortenAsync(new ShortenRequest(commandParameter.Text)),
                string s when s.StartsWith("Translate") => await AIService.TranslateAsync(new TranslateRequest(commandParameter.Text, style)),
                string s when s.StartsWith("ChangeStyle") => await AIService.ChangeStyleAsync(new ChangeStyleRequest(commandParameter.Text, (WritingStyle)Enum.Parse(typeof(WritingStyle), style))),
                string s when s.StartsWith("ChangeTone") => await AIService.ChangeToneAsync(new ChangeToneRequest(commandParameter.Text, (ToneStyle)Enum.Parse(typeof(ToneStyle), style))),
                _ => null
            };
            string text = "";
            if(!ReferenceEquals(result, null) && result.IsCompleted)
                text = result.Response;
            else if(!result.IsRestrictedOrFailed) {
                text = result.Response;
                while(result.IsContinuationRequired) {
                    await result.ContinueAsync();
                    text += result.Response;
                }
            }
            else {
                text = "An error occurred while processing your request";
            }
            return text;
        }
    }
}
