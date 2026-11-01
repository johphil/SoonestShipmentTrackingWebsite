<%@ Page Language="C#" MasterPageFile="~/Views/Site.Master" %>
<%@ Import Namespace="Microsoft.AspNet.Identity" %>
<%@ Import Namespace="Microsoft.AspNet.Identity.Owin" %>
<%@ Import Namespace="SoonestShipmentTrackingWebsite.App_Start" %>
<script runat="server">
    private ApplicationUserManager Manager
    {
        get { return Context.GetOwinContext().GetUserManager<ApplicationUserManager>(); }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!User.Identity.IsAuthenticated)
        {
            Response.Redirect("~/Views/Account/Login.aspx?ReturnUrl=" + HttpUtility.UrlEncode(Request.Url.AbsolutePath));
            return;
        }
        if (IsPostBack) return;

        var user = Manager.FindById(User.Identity.GetUserId());
        if (user == null) return;
        litEmail.Text = HttpUtility.HtmlEncode(user.Email);
        txtFullName.Text = user.FullName;
        txtAddress.Text = user.Address;
        txtPhone.Text = user.PhoneNumber;
    }

    protected void btnSave_Click(object sender, EventArgs e)
    {
        if (!Page.IsValid) return;

        var manager = Manager;
        var user = manager.FindById(User.Identity.GetUserId());
        user.FullName = txtFullName.Text.Trim();
        user.Address = txtAddress.Text.Trim();
        user.PhoneNumber = txtPhone.Text.Trim();
        var result = manager.Update(user);
        if (result.Succeeded)
        {
            pnlError.Visible = false;
            pnlSaved.Visible = true;
        }
        else
        {
            litError.Text = HttpUtility.HtmlEncode(string.Join(" ", result.Errors));
            pnlError.Visible = true;
        }
    }
</script>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">My Profile</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div class="section" style="padding-top:36px;">
    <div class="container" style="max-width:480px;">
        <div class="card-panel">
            <div class="page-heading">My Profile</div>
            <asp:Panel ID="pnlError" runat="server" Visible="false">
                <div class="validation-summary"><asp:Literal ID="litError" runat="server" /></div>
            </asp:Panel>
            <asp:Panel ID="pnlSaved" runat="server" Visible="false">
                <div class="alert-success">Profile updated.</div>
            </asp:Panel>
            <div class="form-group">
                <label>Email</label>
                <div><asp:Literal ID="litEmail" runat="server" /></div>
            </div>
            <div class="form-group">
                <label for="txtFullName">Full name</label>
                <asp:TextBox ID="txtFullName" runat="server" CssClass="form-control" />
                <asp:RequiredFieldValidator runat="server" ControlToValidate="txtFullName" Display="Dynamic" CssClass="field-error" ErrorMessage="Full name is required." ValidationGroup="Profile" />
            </div>
            <div class="form-group">
                <label for="txtPhone">Phone</label>
                <asp:TextBox ID="txtPhone" runat="server" CssClass="form-control" />
            </div>
            <div class="form-group">
                <label for="txtAddress">Address</label>
                <asp:TextBox ID="txtAddress" runat="server" CssClass="form-control" />
            </div>
            <asp:Button ID="btnSave" runat="server" CssClass="btn btn-red btn-block" Text="Save Changes" OnClick="btnSave_Click" ValidationGroup="Profile" />
            <p style="text-align:center;margin-top:16px;font-size:13.5px;">
                <a runat="server" href="~/Views/Account/ChangePassword.aspx">Change password</a>
            </p>
        </div>
    </div>
</div>
</asp:Content>
