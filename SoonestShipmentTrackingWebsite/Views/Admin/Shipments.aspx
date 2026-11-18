<%@ Page Language="C#" MasterPageFile="~/Views/Site.Master" AutoEventWireup="true" CodeBehind="Shipments.aspx.cs" Inherits="SoonestShipmentTrackingWebsite.Views.Admin.Shipments" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">Manage Shipments</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div class="section app-section">
    <div class="container">
        <div class="card-panel">
            <div class="page-head">
                <div>
                    <div class="page-heading">All Shipments</div>
                    <div class="page-subheading">Filter by status using the chips below. Newest activity first.</div>
                </div>
            </div>
            <asp:Literal ID="litStatusChips" runat="server" />
            <div class="table-scroll">
            <asp:GridView ID="gvShipments" runat="server" AutoGenerateColumns="false" CssClass="data-grid data-grid-shipments" GridLines="None"
                            AllowPaging="true" PageSize="20" OnPageIndexChanging="gvShipments_PageIndexChanging" PagerStyle-CssClass="grid-pager"
                EmptyDataText="No shipments match this view.">
                <Columns>
                    <asp:TemplateField HeaderText="Shipment">
                        <ItemTemplate><%# SoonestShipmentTrackingWebsite.Helpers.StatusDisplayHelper.ShipmentCell(Eval("ControlNumber"), Eval("ShipmentType"), Eval("CreatedDate")) %></ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Customer">
                        <ItemTemplate><%# SoonestShipmentTrackingWebsite.Helpers.StatusDisplayHelper.PartyCell(Eval("CustomerName"), Eval("CustomerEmail")) %></ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Area">
                        <ItemTemplate><%# SoonestShipmentTrackingWebsite.Helpers.StatusDisplayHelper.RegionAreaCell(Eval("RegionArea")) %></ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Route / Container">
                        <ItemTemplate><%# SoonestShipmentTrackingWebsite.Helpers.StatusDisplayHelper.RouteCell(Eval("ShipmentType"), Eval("BranchOrigin"), Eval("ContainerNumber"), Eval("PackingList"), Eval("RegionArea"), Eval("RecipientAddress")) %></ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Cargo">
                        <ItemTemplate><%# SoonestShipmentTrackingWebsite.Helpers.StatusDisplayHelper.CargoCell(Eval("PackageDescription"), Eval("WeightKg")) %></ItemTemplate>
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
    </div>
</div>
</asp:Content>
