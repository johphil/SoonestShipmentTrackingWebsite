<%@ Page Language="C#" MasterPageFile="~/Views/Site.Master" AutoEventWireup="true" CodeBehind="UserAccounts.aspx.cs" Inherits="SoonestShipmentTrackingWebsite.Views.Admin.UserAccounts" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">Manage User Accounts</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="section" style="padding-top: 36px;">
        <div class="container">
            <div class="card-panel">
                <div class="page-heading">Manage User Accounts</div>
                <div class="page-subheading">Assign each registered user an access level: Admin, Staff, or Customer.</div>
                
                <asp:GridView ID="gvUsers" runat="server" AutoGenerateColumns="false" CssClass="data-grid" GridLines="None"
                    EmptyDataText="No user accounts found.">
                    <Columns>
                        <asp:BoundField DataField="FullName" HeaderText="Name" />
                        <asp:BoundField DataField="Email" HeaderText="Email" />
                        <asp:BoundField DataField="PhoneNumber" HeaderText="Phone" />
                        <asp:TemplateField HeaderText="Current Access">
                            <ItemTemplate>
                                <span class='<%# "status-badge " + GetRoleBadgeClass(Eval("CurrentRole").ToString()) %>'><%# Eval("CurrentRole") %></span>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="">
                            <ItemTemplate>
                                <asp:HyperLink runat="server" NavigateUrl='<%# "~/Views/Admin/UserRoleEdit.aspx?id=" + Eval("Id") %>'
                                    CssClass="btn btn-outline" Text="Manage Access" Visible='<%# !(bool)Eval("IsCurrentUser") %>'
                                    Style="padding: 5px 12px; font-size: 11.5px; border-color: #0b2540; color: #0b2540;" />
                                <asp:Label runat="server" Text="This is you" CssClass="form-hint" Style="margin: 0;"
                                    Visible='<%# (bool)Eval("IsCurrentUser") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
        </div>
    </div>
</asp:Content>
