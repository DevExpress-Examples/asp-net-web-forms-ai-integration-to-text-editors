Imports DevExpress.AIIntegration
Imports System.Threading.Tasks
Imports System.Web

Namespace WebFormsAIIntegration.Models

    Public Module AIHelper

        Public Class AIRequestData

            Public Property Command As String

            Public Property Text As String
        End Class

        Private ReadOnly Property AIService As AIExtensionsContainerDefault
            Get
                Return TryCast(HttpContext.Current.Application("AIService"), AIExtensionsContainerDefault)
            End Get
        End Property

        Public Async Function GetResponseAsync(ByVal commandParameter As AIRequestData) As Task(Of String)
            If AIService Is Nothing Then
                Return "The AI service is not available"
            End If

            Dim style As String = ""
            Dim commandParts = commandParameter.Command.Split("-"c)
            If commandParts.Length = 2 Then
                style = commandParts(1)
            End If

             ''' Cannot convert LocalDeclarationStatementSyntax, System.InvalidCastException: Unable to cast object of type 'Microsoft.CodeAnalysis.VisualBasic.Syntax.EmptyStatementSyntax' to type 'Microsoft.CodeAnalysis.VisualBasic.Syntax.ExpressionSyntax'.
'''    at ICSharpCode.CodeConverter.VB.CommonConversions.RemodelVariableDeclaration(VariableDeclarationSyntax declaration) in C:\builds\CS2VB\CodeConverter-master\CodeConverter\VB\CommonConversions.cs:line 478
'''    at ICSharpCode.CodeConverter.VB.MethodBodyExecutableStatementVisitor.VisitLocalDeclarationStatement(LocalDeclarationStatementSyntax node) in C:\builds\CS2VB\CodeConverter-master\CodeConverter\VB\MethodBodyExecutableStatementVisitor.cs:line 59
'''    at Microsoft.CodeAnalysis.CSharp.CSharpSyntaxVisitor`1.Visit(SyntaxNode node)
'''    at ICSharpCode.CodeConverter.VB.CommentConvertingMethodBodyVisitor.DefaultVisit(SyntaxNode node) in C:\builds\CS2VB\CodeConverter-master\CodeConverter\VB\CommentConvertingMethodBodyVisitor.cs:line 24
''' 
''' Input:
'''             DevExpress.AIIntegration.Extensions.TextResponse result = commandParameter.Command switch {
'''                 "Summarize" => await WebFormsAIIntegration.Models.AIHelper.AIService.AbstractiveSummaryAsync(new DevExpress.AIIntegration.Extensions.AbstractiveSummaryRequest(commandParameter.Text)),
'''                 "Explain" => await WebFormsAIIntegration.Models.AIHelper.AIService.ExplainAsync(new DevExpress.AIIntegration.Extensions.ExplainRequest(commandParameter.Text)),
'''                 "Proofread" => await WebFormsAIIntegration.Models.AIHelper.AIService.ProofreadAsync(new DevExpress.AIIntegration.Extensions.ProofreadRequest(commandParameter.Text)),
'''                 "Expand" => await WebFormsAIIntegration.Models.AIHelper.AIService.ExpandAsync(new DevExpress.AIIntegration.Extensions.ExpandRequest(commandParameter.Text)),
'''                 "Shorten" => await WebFormsAIIntegration.Models.AIHelper.AIService.ShortenAsync(new DevExpress.AIIntegration.Extensions.ShortenRequest(commandParameter.Text)),
'''                 string s when s.StartsWith("Translate") => await WebFormsAIIntegration.Models.AIHelper.AIService.TranslateAsync(new DevExpress.AIIntegration.Extensions.TranslateRequest(commandParameter.Text, style)),
'''                 string s when s.StartsWith("ChangeStyle") => await WebFormsAIIntegration.Models.AIHelper.AIService.ChangeStyleAsync(new DevExpress.AIIntegration.Extensions.ChangeStyleRequest(commandParameter.Text, (DevExpress.AIIntegration.Extensions.WritingStyle)System.Enum.Parse(typeof(DevExpress.AIIntegration.Extensions.WritingStyle), style))),
'''                 string s when s.StartsWith("ChangeTone") => await WebFormsAIIntegration.Models.AIHelper.AIService.ChangeToneAsync(new DevExpress.AIIntegration.Extensions.ChangeToneRequest(commandParameter.Text, (DevExpress.AIIntegration.Extensions.ToneStyle)System.Enum.Parse(typeof(DevExpress.AIIntegration.Extensions.ToneStyle), style))),
'''                 _ => null
'''             };
''' 
'''  Dim text As String = ""
            If Not Object.ReferenceEquals(result, Nothing) AndAlso result.IsCompleted Then
                text = result.Response
            ElseIf Not result.IsRestrictedOrFailed Then
                text = result.Response
                While result.IsContinuationRequired
                    Await result.ContinueAsync()
                    text += result.Response
                End While
            Else
                text = "An error occurred while processing your request"
            End If

            Return text
        End Function
    End Module
End Namespace
