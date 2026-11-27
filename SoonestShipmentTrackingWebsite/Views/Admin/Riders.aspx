<%@ Page Language="C#" MasterPageFile="~/Views/Site.Master" AutoEventWireup="true" CodeBehind="Riders.aspx.cs" Inherits="SoonestShipmentTrackingWebsite.Views.Admin.Riders" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">Manage Riders</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div class="section app-section">
    <div class="container">
        <div class="card-panel">
            <div class="page-head">
                <div>
                    <div class="page-heading">Manage Riders</div>
                    <div class="page-subheading">Maintain rider records separately from system user accounts.</div>
                </div>
                <a runat="server" href="~/Views/Admin/RiderEdit.aspx" class="btn btn-primary">+ Add Rider</a>
            </div>

            <div class="toolbar">
                <asp:TextBox ID="txtSearch" runat="server" CssClass="form-control" placeholder="Search name, email, mobile, license" />
                <asp:Button ID="btnSearch" runat="server" CssClass="btn btn-outline" Text="Search" OnClick="btnSearch_Click" />
                <asp:Button ID="btnClear" runat="server" CssClass="btn btn-outline" Text="Clear" OnClick="btnClear_Click" />
            </div>

            <asp:GridView ID="gvRiders" runat="server" AutoGenerateColumns="false" CssClass="data-grid" GridLines="None"
                AllowPaging="true" PageSize="10" OnPageIndexChanging="gvRiders_PageIndexChanging" OnRowCommand="gvRiders_RowCommand"
                EmptyDataText="No riders found.">
                <Columns>
                    <asp:BoundField DataField="RiderId" HeaderText="Rider ID" />
                    <asp:BoundField DataField="FullName" HeaderText="Full Name" />
                    <asp:BoundField DataField="Email" HeaderText="Email Address" />
                    <asp:BoundField DataField="MobileNumber" HeaderText="SMS / Mobile Number" />
                    <asp:BoundField DataField="LicenseNumber" HeaderText="License Number" />
                    <asp:BoundField DataField="Address" HeaderText="Address" />
                    <asp:BoundField DataField="DateCreated" HeaderText="Date Created" DataFormatString="{0:MMM d, yyyy h:mm tt}" />
                    <asp:BoundField DataField="DateModified" HeaderText="Date Modified" DataFormatString="{0:MMM d, yyyy h:mm tt}" />
                    <asp:TemplateField HeaderText="">
                        <ItemTemplate>
                            <asp:HyperLink runat="server" NavigateUrl='<%# "~/Views/Admin/RiderEdit.aspx?id=" + Eval("RiderId") %>'
                                CssClass="btn btn-outline" Text="Edit" />
                            <asp:LinkButton runat="server" CommandName='<%# (bool)Eval("IsActive") ? "DeactivateRider" : "ActivateRider" %>' CommandArgument='<%# Eval("RiderId") %>'
                                CssClass="btn btn-outline" Text='<%# (bool)Eval("IsActive") ? "Remove" : "Activate" %>'
                                OnClientClick='<%# (bool)Eval("IsActive") ? "return confirm(\"Remove this rider from active assignments?\");" : "return confirm(\"Activate this rider?\");" %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </div>
</div>
</asp:Content>
