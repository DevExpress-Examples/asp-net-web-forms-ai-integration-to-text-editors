<%@ Page Language="VB" AutoEventWireup="true" CodeBehind="RichEdit.aspx.vb" Inherits="WebFormsAIIntegration.RichEdit" Async="true" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script type="text/javascript">
        let pendingCommand = "";

        function OnCustomCommandExecuted(s, e) {
            if (e.commandName.startsWith('AI')) {
                const command = e.commandName.split(':')[1];
                if (s.selection.intervals.length > 0) {
                    const selectedInterval = s.selection.intervals[0];
                    if (selectedInterval.length > 0) {
                        const selectedText = s.document.activeSubDocument.getTextByInterval(selectedInterval);
                        richEdit.loadingPanel.show();
                        pendingCommand = command.split("-")[0];
                        callback.PerformCallback(JSON.stringify({ Command: command, Text: selectedText }));
                    } else {
                        alert('Please select some text');
                    }
                }
            }
        }

        function OnCallbackComplete(s, e) {
            richEdit.loadingPanel.hide();
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
            richEdit.commands.beginUpdate();
            richEdit.commands.delete.execute();
            richEdit.commands.insertText.execute(aiTextMemo.GetText());
            richEdit.commands.endUpdate();
            popup.Hide();
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <dx:ASPxCallback ID="ASPxCallback1" runat="server" ClientInstanceName="callback" OnCallback="ASPxCallback1_Callback">
            <ClientSideEvents CallbackComplete="OnCallbackComplete" />
        </dx:ASPxCallback>

        <dx:ASPxRichEdit runat="server" ID="ASPxRichEdit1" ClientInstanceName="richEdit" Width="100%" Height="700px" WorkDirectory="~/Documents">
            <ClientSideEvents CustomCommandExecuted="OnCustomCommandExecuted" />
        </dx:ASPxRichEdit>

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
    </form>
</body>
</html>
