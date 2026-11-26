<%@ Page Language="C#" MasterPageFile="~/Views/Site.Master" AutoEventWireup="true" CodeBehind="Dashboard.aspx.cs" Inherits="SoonestShipmentTrackingWebsite.Views.Admin.Dashboard" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">Admin Dashboard</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div class="section app-section">
    <div class="container">

        <div class="page-head">
            <div>
                <div class="page-heading">Dashboard</div>
                <div class="page-subheading">Operations overview &mdash; click a tile to open that filtered shipment list.</div>
            </div>
            <div class="page-head-actions">
                <a runat="server" href="~/Views/Admin/CreateShipment.aspx" class="btn btn-primary">+ New Shipment</a>
                <a runat="server" href="~/Views/Admin/Shipments.aspx" class="btn btn-outline">Manage All Shipments</a>
            </div>
        </div>

        <div class="stat-mini-grid">
            <a class="stat-mini" runat="server" href="~/Views/Admin/Shipments.aspx"><div class="num"><asp:Literal ID="litTotal" runat="server" /></div><div class="lbl">Total Shipments</div></a>
            <a class="stat-mini" runat="server" href="~/Views/Admin/Shipments.aspx?status=Pending"><div class="num"><asp:Literal ID="litPending" runat="server" /></div><div class="lbl">Pending</div></a>
            <a class="stat-mini" runat="server" href="~/Views/Admin/Shipments.aspx?status=InTransit"><div class="num"><asp:Literal ID="litInTransit" runat="server" /></div><div class="lbl">In Transit</div></a>
            <a class="stat-mini" runat="server" href="~/Views/Admin/Shipments.aspx?status=OutForDelivery"><div class="num"><asp:Literal ID="litOutForDelivery" runat="server" /></div><div class="lbl">Out for Delivery</div></a>
            <a class="stat-mini" runat="server" href="~/Views/Admin/Shipments.aspx?status=Delivered"><div class="num"><asp:Literal ID="litDelivered" runat="server" /></div><div class="lbl">Delivered</div></a>
            <a class="stat-mini" runat="server" href="~/Views/Admin/Shipments.aspx?issues=1"><div class="num"><asp:Literal ID="litIssues" runat="server" /></div><div class="lbl">Open Issues</div></a>
            <div class="stat-mini"><div class="num"><asp:Literal ID="litCustomers" runat="server" /></div><div class="lbl">Customers</div></div>
        </div>

        <div class="dash-layout">
            <div class="card-panel">
                <h4 class="panel-title">Recent Shipments</h4>
                <div class="table-scroll">
                <asp:GridView ID="gvRecent" runat="server" AutoGenerateColumns="false" CssClass="data-grid data-grid-shipments" GridLines="None"
                                AllowPaging="true" PageSize="15" OnPageIndexChanging="gvRecent_PageIndexChanging" PagerStyle-CssClass="grid-pager"
                    EmptyDataText="No shipments yet.">
                    <Columns>
                    <asp:TemplateField HeaderText="Shipment">
                        <ItemTemplate><%# SoonestShipmentTrackingWebsite.Helpers.StatusDisplayHelper.ShipmentCell(Eval("ControlNumber"), Eval("ShipmentType"), Eval("CreatedDate")) %></ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Customer / Recipient">
                        <ItemTemplate><%# SoonestShipmentTrackingWebsite.Helpers.StatusDisplayHelper.PartiesCell(Eval("CustomerName"), Eval("RecipientName"), Eval("RecipientPhone")) %></ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Route / Container">
                        <ItemTemplate><%# SoonestShipmentTrackingWebsite.Helpers.StatusDisplayHelper.RouteCell(Eval("ShipmentType"), Eval("BranchOrigin"), Eval("ContainerNumber"), Eval("PackingList"), Eval("RegionArea"), Eval("RecipientAddress")) %></ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Status">
                        <ItemTemplate><%# SoonestShipmentTrackingWebsite.Helpers.StatusDisplayHelper.StatusCell(Eval("CurrentStatus"), Eval("IssueReported"), Eval("IsIssueResolved")) %></ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Est. Delivery">
                        <ItemTemplate><%# SoonestShipmentTrackingWebsite.Helpers.StatusDisplayHelper.EtaCell(Eval("EstimatedDeliveryDate"), Eval("CurrentStatus")) %></ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Updated">
                        <ItemTemplate><%# SoonestShipmentTrackingWebsite.Helpers.StatusDisplayHelper.UpdatedCell(Eval("UpdatedDate")) %></ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="">
                        <ItemStyle CssClass="col-action" />
                        <ItemTemplate>
                            <asp:HyperLink runat="server" NavigateUrl='<%# "~/Views/Admin/UpdateStatus.aspx?id=" + Eval("Id") %>' CssClass="btn btn-sm" Text="Update" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
                </asp:GridView>
                </div>
            </div>

            <div class="dash-side">
                <div class="card-panel">
                    <h4 class="panel-title">Status Breakdown</h4>
                    <asp:Literal ID="litStatusBar" runat="server" />
                </div>
                <asp:Literal ID="litAttention" runat="server" />
            </div>
        </div>

    </div>
</div>
</asp:Content>
