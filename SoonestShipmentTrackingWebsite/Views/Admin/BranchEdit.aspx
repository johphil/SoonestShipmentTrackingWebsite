<%@ Page Language="C#" MasterPageFile="~/Views/Site.Master" AutoEventWireup="true" CodeBehind="BranchEdit.aspx.cs" Inherits="SoonestShipmentTrackingWebsite.Views.Admin.BranchEdit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">Branch</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div class="section app-section">
    <div class="container container-md">
        <div class="card-panel">
            <div class="page-heading"><asp:Literal ID="litHeading" runat="server" Text="Add Branch" /></div>

            <asp:Panel ID="pnlError" runat="server" Visible="false">
                <div class="validation-summary"><asp:Literal ID="litError" runat="server" /></div>
            </asp:Panel>

            <asp:HiddenField ID="hdnId" runat="server" />

            <div class="form-group">
                <label>Branch Name</label>
                <asp:TextBox ID="txtBranchName" runat="server" CssClass="form-control" />
                <asp:RequiredFieldValidator runat="server" ControlToValidate="txtBranchName" Display="Dynamic" CssClass="field-error" ErrorMessage="Branch name is required." ValidationGroup="Branch" />
            </div>

            <div class="form-group">
                <label>City</label>
                <asp:TextBox ID="txtCity" runat="server" CssClass="form-control" />
                <asp:RequiredFieldValidator runat="server" ControlToValidate="txtCity" Display="Dynamic" CssClass="field-error" ErrorMessage="City is required." ValidationGroup="Branch" />
            </div>

            <div class="form-group">
                <label>Address</label>
                <asp:TextBox ID="txtAddress" runat="server" CssClass="form-control" />
                <asp:RequiredFieldValidator runat="server" ControlToValidate="txtAddress" Display="Dynamic" CssClass="field-error" ErrorMessage="Address is required." ValidationGroup="Branch" />
            </div>

            <div class="form-grid-2">
                <div class="form-group">
                    <label>Contact Number</label>
                    <asp:TextBox ID="txtContactNumber" runat="server" CssClass="form-control" placeholder="0987 654 3210" />
                </div>
                <div class="form-group">
                    <label>Email</label>
                    <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" TextMode="Email" />
                </div>
            </div>

            <div class="form-group">
                <label class="check-row">
                    <asp:CheckBox ID="chkIsActive" runat="server" /> Active
                </label>
            </div>

            <asp:Button ID="btnSave" runat="server" CssClass="btn btn-primary" Text="Save Branch" OnClick="btnSave_Click" ValidationGroup="Branch" />
            <a runat="server" href="~/Views/Admin/Branches.aspx" class="btn btn-outline">Cancel</a>
        </div>
    </div>
</div>
</asp:Content>
