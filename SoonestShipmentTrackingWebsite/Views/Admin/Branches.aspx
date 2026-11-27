<%@ Page Language="C#" MasterPageFile="~/Views/Site.Master" AutoEventWireup="true" CodeBehind="Branches.aspx.cs" Inherits="SoonestShipmentTrackingWebsite.Views.Admin.Branches" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">Manage Branches</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div class="section app-section">
    <div class="container">
        <div class="card-panel">
            <div class="page-head">
                <div class="page-heading">Manage Branches</div>
                <a runat="server" href="~/Views/Admin/BranchEdit.aspx" class="btn btn-primary">+ Add Branch</a>
            </div>

            <asp:GridView ID="gvBranches" runat="server" AutoGenerateColumns="false" CssClass="data-grid" GridLines="None"
                EmptyDataText="No branches yet. Add one to get started." OnRowCommand="gvBranches_RowCommand">
                <Columns>
                    <asp:BoundField DataField="BranchName" HeaderText="Branch Name" />
                    <asp:BoundField DataField="City" HeaderText="City" />
                    <asp:BoundField DataField="Address" HeaderText="Address" />
                    <asp:BoundField DataField="ContactNumber" HeaderText="Contact No." />
                    <asp:TemplateField HeaderText="Status">
                        <ItemTemplate>
                            <span class='<%# "status-badge " + ((bool)Eval("IsActive") ? "badge-success" : "badge-pending") %>'>
                                <%# (bool)Eval("IsActive") ? "Active" : "Inactive" %>
                            </span>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="">
                        <ItemTemplate>
                            <asp:HyperLink runat="server" NavigateUrl='<%# "~/Views/Admin/BranchEdit.aspx?id=" + Eval("Id") %>'
                                CssClass="btn btn-outline" Text="Edit" />
                            <asp:LinkButton runat="server" CommandName="DeleteBranch" CommandArgument='<%# Eval("Id") %>'
                                CssClass="btn btn-outline" Text="Delete"
                                OnClientClick="return confirm('Delete this branch? This cannot be undone.');" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </div>
</div>
</asp:Content>
