<%@ Page Language="C#" MasterPageFile="~/Views/Site.Master" %>
<%@ Import Namespace="Microsoft.AspNet.Identity" %>
<%@ Import Namespace="Microsoft.AspNet.Identity.Owin" %>
<%@ Import Namespace="SoonestShipmentTrackingWebsite.App_Start" %>
<script runat="server">
    protected void btnReset_Click(object sender, EventArgs e)
    {
        if (!Page.IsValid) return;

        var userId = Request.QueryString["userId"];
        var code = Request.QueryString["code"];
        if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(code))
        {
            ShowError("Invalid reset link.");
            return;
        }

        var manager = Context.GetOwinContext().GetUserManager<ApplicationUserManager>();
        var result = manager.ResetPassword(userId, code, txtNew.Text);
        if (result.Succeeded)
        {
            pnlForm.Visible = false;
            pnlDone.Visible = true;
            return;
        }
        ShowError(string.Join(" ", result.Errors));
    }

    private void ShowError(string message)
    {
        litError.Text = HttpUtility.HtmlEncode(message);
        pnlError.Visible = true;
    }
</script>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">Reset Password</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div class="section" style="padding-top:36px;">
    <div class="container" style="max-width:480px;">
        <div class="card-panel">
            <div class="page-heading">Reset Password</div>
            <asp:Panel ID="pnlError" runat="server" Visible="false">
                <div class="validation-summary"><asp:Literal ID="litError" runat="server" /></div>
            </asp:Panel>
            <asp:Panel ID="pnlForm" runat="server">
                <div class="form-group">
                    <label for="txtNew">New password</label>
                    <asp:TextBox ID="txtNew" runat="server" CssClass="form-control" TextMode="Password" />
                    <asp:RequiredFieldValidator runat="server" ControlToValidate="txtNew" Display="Dynamic" CssClass="field-error" ErrorMessage="Password is required." ValidationGroup="Reset" />
                </div>
                <div class="form-group">
                    <label for="txtConfirm">Confirm password</label>
                    <asp:TextBox ID="txtConfirm" runat="server" CssClass="form-control" TextMode="Password" />
                    <asp:CompareValidator runat="server" ControlToValidate="txtConfirm" ControlToCompare="txtNew" Display="Dynamic" CssClass="field-error" ErrorMessage="Passwords do not match." ValidationGroup="Reset" />
                </div>
                <asp:Button ID="btnReset" runat="server" CssClass="btn btn-red btn-block" Text="Reset Password" OnClick="btnReset_Click" ValidationGroup="Reset" />
            </asp:Panel>
            <asp:Panel ID="pnlDone" runat="server" Visible="false">
                <div class="alert-success">Your password has been reset. <a runat="server" href="~/Views/Account/Login.aspx">Log in</a></div>
            </asp:Panel>
        </div>
    </div>
</div>
</asp:Content>
