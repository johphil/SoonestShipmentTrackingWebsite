<%@ Page Language="C#" MasterPageFile="~/Views/Site.Master" %>
<%@ Import Namespace="Microsoft.AspNet.Identity" %>
<%@ Import Namespace="Microsoft.AspNet.Identity.Owin" %>
<%@ Import Namespace="SoonestShipmentTrackingWebsite.App_Start" %>
<%@ Import Namespace="SoonestShipmentTrackingWebsite.Helpers" %>
<script runat="server">
    protected void btnSend_Click(object sender, EventArgs e)
    {
        if (!Page.IsValid) return;

        var manager = Context.GetOwinContext().GetUserManager<ApplicationUserManager>();
        var user = manager.FindByEmail(txtEmail.Text.Trim());
        if (user != null && user.EmailConfirmed)
        {
            try
            {
                var token = manager.GeneratePasswordResetToken(user.Id);
                var url = Request.Url.GetLeftPart(UriPartial.Authority)
                    + ResolveUrl("~/Views/Account/ResetPassword.aspx")
                    + "?userId=" + HttpUtility.UrlEncode(user.Id)
                    + "&code=" + HttpUtility.UrlEncode(token);
                EmailHelper.SendPasswordResetEmail(user.Email, user.FullName, url);
            }
            catch (Exception)
            {
                litError.Text = "We could not send the email. Please try again later.";
                pnlError.Visible = true;
                return;
            }
        }

        pnlForm.Visible = false;
        pnlDone.Visible = true;
    }
</script>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">Forgot Password</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div class="section" style="padding-top:36px;">
    <div class="container" style="max-width:480px;">
        <div class="card-panel">
            <div class="page-heading">Forgot Password</div>
            <asp:Panel ID="pnlError" runat="server" Visible="false">
                <div class="validation-summary"><asp:Literal ID="litError" runat="server" /></div>
            </asp:Panel>
            <asp:Panel ID="pnlForm" runat="server">
                <div class="page-subheading">Enter your email and we will send you a reset link.</div>
                <div class="form-group">
                    <label for="txtEmail">Email</label>
                    <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" TextMode="Email" />
                    <asp:RequiredFieldValidator runat="server" ControlToValidate="txtEmail" Display="Dynamic" CssClass="field-error" ErrorMessage="Email is required." ValidationGroup="Forgot" />
                </div>
                <asp:Button ID="btnSend" runat="server" CssClass="btn btn-red btn-block" Text="Send Reset Link" OnClick="btnSend_Click" ValidationGroup="Forgot" />
            </asp:Panel>
            <asp:Panel ID="pnlDone" runat="server" Visible="false">
                <div class="alert-success">If an account with that email exists, a reset link has been sent.</div>
            </asp:Panel>
            <p style="text-align:center;margin-top:16px;font-size:13.5px;">
                <a runat="server" href="~/Views/Account/Login.aspx">Back to log in</a>
            </p>
        </div>
    </div>
</div>
</asp:Content>
