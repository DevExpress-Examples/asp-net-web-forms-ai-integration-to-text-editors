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
            var credentials = (new ApiKeyCredential("DEMO"));

            IChatClient client = (new AzureOpenAIClient(
                                    new Uri("https://public-api.devexpress.com/demo-openai"),
                                    credentials
                                )).GetChatClient("gpt-4o-mini").AsIChatClient();
            var defaultAIContainer = new AIExtensionsContainerDefault();
            defaultAIContainer.RegisterChatClient(client);


            Application["AIService"] = defaultAIContainer;
        }
    }
}
