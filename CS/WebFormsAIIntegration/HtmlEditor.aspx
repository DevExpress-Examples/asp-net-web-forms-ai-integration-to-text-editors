<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="HtmlEditor.aspx.cs" Inherits="WebFormsAIIntegration.HtmlEditor" Async="true" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script type="text/javascript">
        let pendingCommand = "";

        function OnCustomCommand(s, e) {
            if (e.commandName.startsWith('AI')) {
                const command = e.commandName.split(':')[1];
                const selectedText = s.GetSelection().GetText();
                if (selectedText !== '') {
                    htmlEditor.ShowLoadingPanel();
                    pendingCommand = command.split("-")[0];
                    callback.PerformCallback(JSON.stringify({ Command: command, Text: selectedText }));
                }
                else {
                    alert('Please select some text');
                }
            }
        }

        function OnCallbackComplete(s, e) {
            htmlEditor.HideLoadingPanel();
            popup.SetHeaderText(pendingCommand);
            aiTextMemo.SetText(s.cpText);
            popup.Show();
            pendingCommand = "";
        }

        function OnCopyClick(s, e) {
            if (navigator.clipboard.writeText) {
                navigator.clipboard.writeText(aiTextMemo.GetText());
            }
            popup.Hide();
        }

        function OnReplaceClick(s, e) {
            htmlEditor.GetSelection().SetHtml(aiTextMemo.GetText(), true);
            popup.Hide();
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <dx:ASPxCallback ID="ASPxCallback1" runat="server" ClientInstanceName="callback" OnCallback="ASPxCallback1_Callback">
                <ClientSideEvents CallbackComplete="OnCallbackComplete" />
            </dx:ASPxCallback>

            <dx:ASPxHtmlEditor ID="ASPxHtmlEditor1" runat="server" ClientInstanceName="htmlEditor" Width="100%" Height="700px" ToolbarMode="Ribbon">
                <ClientSideEvents CustomCommand="OnCustomCommand" />
            </dx:ASPxHtmlEditor>

            <dx:ASPxPopupControl ID="ASPxPopupControl1" ClientInstanceName="popup" ShowFooter="true" runat="server" Modal="true" Width="700px" PopupHorizontalAlign="WindowCenter" PopupVerticalAlign="WindowCenter">
                <ContentCollection>
                    <dx:PopupControlContentControl runat="server">
                        <dx:ASPxMemo ID="AITextMemo" runat="server" ClientInstanceName="aiTextMemo" Height="300px" Width="100%" ReadOnly="true"></dx:ASPxMemo>
                    </dx:PopupControlContentControl>
                </ContentCollection>
                <FooterContentTemplate>
                    <dx:ASPxButton ID="CopyBtn" runat="server" Text="Copy" AutoPostBack="false">
                        <ClientSideEvents Click="OnCopyClick" />
                    </dx:ASPxButton>
                    <dx:ASPxButton ID="CloseBtn" runat="server" Text="Replace" AutoPostBack="false">
                        <ClientSideEvents Click="OnReplaceClick" />
                    </dx:ASPxButton>
                </FooterContentTemplate>
            </dx:ASPxPopupControl>
        </div>
    </form>
</body>
</html>
