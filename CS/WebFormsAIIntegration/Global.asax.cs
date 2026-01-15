using Azure.AI.OpenAI;
using DevExpress.AIIntegration;
using DevExpress.Web.ASPxRichEdit.Internal;
using Microsoft.Extensions.AI;
using System;
using System.ClientModel;
using System.Threading.Tasks;

namespace WebFormsAIIntegration {

    public class Global_asax : System.Web.HttpApplication {
        void Application_Start(object sender, EventArgs e) {
            string azureOpenAIEndpoint = Environment.GetEnvironmentVariable("AZURE_OPENAI_ENDPOINT");
            string azureOpenAIKey = Environment.GetEnvironmentVariable("AZURE_OPENAI_API_KEY");
            var defaultAIContainer = new AIExtensionsContainerDefault();
            var azureOpenAIClient = new AzureOpenAIClient(
                new Uri(azureOpenAIEndpoint),
                new ApiKeyCredential(azureOpenAIKey));

            var chatClient = azureOpenAIClient.GetChatClient("gpt-4o-mini").AsIChatClient();
            defaultAIContainer.RegisterChatClient(chatClient);
            Application["AIService"] = defaultAIContainer;
        }
    }
}
