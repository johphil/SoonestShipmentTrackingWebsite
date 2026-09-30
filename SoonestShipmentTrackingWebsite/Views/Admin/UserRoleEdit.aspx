<%@ Page Language="C#" MasterPageFile="~/Views/Site.Master" AutoEventWireup="true" CodeBehind="UserRoleEdit.aspx.cs" Inherits="SoonestShipmentTrackingWebsite.Views.Admin.UserRoleEdit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">Manage Access</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div class="section app-section" style="padding-top:44px;">
    <div class="container" style="max-width:520px;">
        <div class="card-panel">
            <div class="page-heading">Manage Access</div>
            <div class="page-subheading">
                <asp:Literal ID="litUserName" runat="server" /> &middot; <asp:Literal ID="litUserEmail" runat="server" />
            </div>

            <asp:HiddenField ID="hdnUserId" runat="server" />

            <div class="form-group">
                <label>Access Level</label>
                <div class="role-picker">
                    <asp:RadioButtonList ID="rblRole" runat="server" RepeatLayout="Flow" RepeatDirection="Horizontal" class="role-list">
                        <asp:ListItem Text="Admin" Value="Admin" />
                        <asp:ListItem Text="Staff" Value="Staff" />
                        <asp:ListItem Text="Rider" Value="Rider" />
                        <asp:ListItem Text="Customer" Value="Customer" />
                    </asp:RadioButtonList>
                </div>
                <asp:RequiredFieldValidator runat="server" ControlToValidate="rblRole" Display="Dynamic" CssClass="field-error" ErrorMessage="Select an access level." ValidationGroup="Role" />
            </div>

            <div class="form-group">
                <label>Designated Branch <span style="color:#c62828;">*</span> <small>(required for Staff and Rider)</small></label>
                <asp:DropDownList ID="ddlBranch" runat="server" CssClass="form-control" />
                <asp:CustomValidator ID="valBranch" runat="server" Display="Dynamic" CssClass="field-error" ValidationGroup="Role" OnServerValidate="valBranch_ServerValidate" ErrorMessage="Select the branch where this Staff/Rider member is designated." />
            </div>

            <asp:Button ID="btnSave" runat="server" CssClass="btn btn-red" Text="Save Access Level" OnClick="btnSave_Click" ValidationGroup="Role" />
            <a runat="server" href="~/Views/Admin/UserAccounts.aspx" class="btn btn-outline" style="border-color:#07233d;color:#07233d;">Cancel</a>
        </div>
    </div>
</div>
</asp:Content>
