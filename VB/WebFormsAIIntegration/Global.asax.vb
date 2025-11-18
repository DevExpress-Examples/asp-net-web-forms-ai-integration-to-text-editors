Imports Azure.AI.OpenAI
Imports DevExpress.AIIntegration
Imports Microsoft.Extensions.AI
Imports System
Imports System.ClientModel

Namespace WebFormsAIIntegration

    Public Class Global_asax
        Inherits Web.HttpApplication

        Private Sub Application_Start(ByVal sender As Object, ByVal e As EventArgs)
            Dim credentials =(New ApiKeyCredential("DEMO"))
            Dim client As IChatClient =(New AzureOpenAIClient(New Uri("https://public-api.devexpress.com/demo-openai"), credentials)).GetChatClient("gpt-4o-mini").AsIChatClient()
            Dim defaultAIContainer = New AIExtensionsContainerDefault()
            defaultAIContainer.RegisterChatClient(client)
            Application("AIService") = defaultAIContainer
        End Sub
    End Class
End Namespace
