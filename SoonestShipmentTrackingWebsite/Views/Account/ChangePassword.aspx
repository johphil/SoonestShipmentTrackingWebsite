<%@ Page Language="C#" MasterPageFile="~/Views/Site.Master" %>
<%@ Import Namespace="Microsoft.AspNet.Identity" %>
<%@ Import Namespace="Microsoft.AspNet.Identity.Owin" %>
<%@ Import Namespace="SoonestShipmentTrackingWebsite.App_Start" %>
<script runat="server">
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!User.Identity.IsAuthenticated)
            Response.Redirect("~/Views/Account/Login.aspx?ReturnUrl=" + HttpUtility.UrlEncode(Request.Url.AbsolutePath));
    }

    protected void btnChange_Click(object sender, EventArgs e)
    {
        if (!Page.IsValid) return;

        var manager = Context.GetOwinContext().GetUserManager<ApplicationUserManager>();
        var result = manager.ChangePassword(User.Identity.GetUserId(), txtCurrent.Text, txtNew.Text);
        if (result.Succeeded)
        {
            pnlError.Visible = false;
            pnlDone.Visible = true;
            return;
        }
        litError.Text = HttpUtility.HtmlEncode(string.Join(" ", result.Errors));
        pnlError.Visible = true;
    }
</script>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">Change Password</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div class="section" style="padding-top:36px;">
    <div class="container" style="max-width:480px;">
        <div class="card-panel">
            <div class="page-heading">Change Password</div>
            <asp:Panel ID="pnlError" runat="server" Visible="false">
                <div class="validation-summary"><asp:Literal ID="litError" runat="server" /></div>
            </asp:Panel>
            <asp:Panel ID="pnlDone" runat="server" Visible="false">
                <div class="alert-success">Your password has been changed.</div>
            </asp:Panel>
            <div class="form-group">
                <label for="txtCurrent">Current password</label>
                <asp:TextBox ID="txtCurrent" runat="server" CssClass="form-control" TextMode="Password" />
                <asp:RequiredFieldValidator runat="server" ControlToValidate="txtCurrent" Display="Dynamic" CssClass="field-error" ErrorMessage="Current password is required." ValidationGroup="Change" />
            </div>
            <div class="form-group">
                <label for="txtNew">New password</label>
                <asp:TextBox ID="txtNew" runat="server" CssClass="form-control" TextMode="Password" />
                <asp:RequiredFieldValidator runat="server" ControlToValidate="txtNew" Display="Dynamic" CssClass="field-error" ErrorMessage="New password is required." ValidationGroup="Change" />
            </div>
            <div class="form-group">
                <label for="txtConfirm">Confirm new password</label>
                <asp:TextBox ID="txtConfirm" runat="server" CssClass="form-control" TextMode="Password" />
                <asp:CompareValidator runat="server" ControlToValidate="txtConfirm" ControlToCompare="txtNew" Display="Dynamic" CssClass="field-error" ErrorMessage="Passwords do not match." ValidationGroup="Change" />
            </div>
            <asp:Button ID="btnChange" runat="server" CssClass="btn btn-red btn-block" Text="Change Password" OnClick="btnChange_Click" ValidationGroup="Change" />
            <p style="text-align:center;margin-top:16px;font-size:13.5px;">
                <a runat="server" href="~/Views/Account/Profile.aspx">Back to profile</a>
            </p>
        </div>
    </div>
</div>
</asp:Content>
