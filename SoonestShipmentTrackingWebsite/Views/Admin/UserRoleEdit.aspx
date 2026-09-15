<%@ Page Language="C#" MasterPageFile="~/Views/Site.Master" AutoEventWireup="true" CodeBehind="UserRoleEdit.aspx.cs" Inherits="SoonestShipmentTrackingWebsite.Views.Admin.UserRoleEdit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">Manage Access</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div class="section" style="padding-top:36px;">
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
                    <asp:RadioButtonList ID="rblRole" runat="server" RepeatLayout="Flow" RepeatDirection="Vertical">
                        <asp:ListItem Text="Admin — full access to shipments, branches, and user accounts" Value="Admin" />
                        <asp:ListItem Text="Staff — internal team member" Value="Staff" />
                        <asp:ListItem Text="Customer — can register shipments and track their own parcels" Value="Customer" />
                    </asp:RadioButtonList>
                </div>
                <asp:RequiredFieldValidator runat="server" ControlToValidate="rblRole" Display="Dynamic" CssClass="field-error" ErrorMessage="Select an access level." ValidationGroup="Role" />
            </div>

            <asp:Button ID="btnSave" runat="server" CssClass="btn btn-red" Text="Save Access Level" OnClick="btnSave_Click" ValidationGroup="Role" />
            <a runat="server" href="~/Views/Admin/UserAccounts.aspx" class="btn btn-outline" style="border-color:#0b2540;color:#0b2540;">Cancel</a>
        </div>
    </div>
</div>
</asp:Content>
