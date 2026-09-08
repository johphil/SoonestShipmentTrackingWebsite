<%@ Page Language="C#" MasterPageFile="~/Views/Site.Master" AutoEventWireup="true" CodeBehind="Dashboard.aspx.cs" Inherits="SoonestShipmentTrackingWebsite.Views.Admin.Dashboard" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">Admin Dashboard</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div class="section" style="padding-top:36px;">
    <div class="container">

        <div class="stat-mini-grid">
            <div class="stat-mini"><div class="num"><asp:Literal ID="litTotal" runat="server" /></div><div class="lbl">Total Shipments</div></div>
            <div class="stat-mini"><div class="num"><asp:Literal ID="litPending" runat="server" /></div><div class="lbl">Pending</div></div>
            <div class="stat-mini"><div class="num"><asp:Literal ID="litInTransit" runat="server" /></div><div class="lbl">In Transit</div></div>
            <div class="stat-mini"><div class="num"><asp:Literal ID="litOutForDelivery" runat="server" /></div><div class="lbl">Out for Delivery</div></div>
            <div class="stat-mini"><div class="num"><asp:Literal ID="litDelivered" runat="server" /></div><div class="lbl">Delivered</div></div>
            <div class="stat-mini"><div class="num"><asp:Literal ID="litCustomers" runat="server" /></div><div class="lbl">Customers</div></div>
        </div>

        <div style="margin-bottom:22px;">
            <a runat="server" href="~/Views/Admin/CreateShipment.aspx" class="btn btn-red">+ New Shipment</a>
            <a runat="server" href="~/Views/Admin/Shipments.aspx" class="btn btn-outline" style="border-color:#0b2540;color:#0b2540;">Manage All Shipments</a>
        </div>

        <div class="card-panel">
            <h4 style="margin-top:0;color:#0b2540;">Recent Shipments</h4>
            <asp:GridView ID="gvRecent" runat="server" AutoGenerateColumns="false" CssClass="data-grid" GridLines="None"
                EmptyDataText="No shipments yet.">
                <Columns>
                    <asp:BoundField DataField="ControlNumber" HeaderText="Control No." />
                    <asp:BoundField DataField="CustomerName" HeaderText="Customer" />
                    <asp:BoundField DataField="RecipientName" HeaderText="Recipient" />
                    <asp:BoundField DataField="DestinationCity" HeaderText="Destination" />
                    <asp:TemplateField HeaderText="Status">
                        <ItemTemplate><%# SoonestShipmentTrackingWebsite.Helpers.StatusDisplayHelper.Badge((SoonestShipmentTrackingWebsite.Models.ShipmentStatus)Eval("CurrentStatus")) %></ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="UpdatedDate" HeaderText="Last Update" DataFormatString="{0:MMM d, h:mm tt}" />
                    <asp:TemplateField HeaderText="">
                        <ItemTemplate>
                            <asp:HyperLink runat="server" NavigateUrl='<%# "~/Views/Admin/UpdateStatus.aspx?id=" + Eval("Id") %>'
                                CssClass="btn btn-outline" Text="Update" style="padding:5px 12px;font-size:11.5px;border-color:#0b2540;color:#0b2540;" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>

    </div>
</div>
</asp:Content>
