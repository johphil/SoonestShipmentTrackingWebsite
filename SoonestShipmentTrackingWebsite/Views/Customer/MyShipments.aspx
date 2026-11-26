<%@ Page Language="C#" MasterPageFile="~/Views/Site.Master" AutoEventWireup="true" CodeBehind="MyShipments.aspx.cs" Inherits="SoonestShipmentTrackingWebsite.Views.Customer.MyShipments" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">My Shipments</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div class="section app-section">
    <div class="container">
        <div class="card-panel">
            <div class="page-head">
                <div>
                    <div class="page-heading">My Shipments</div>
                    <div class="page-subheading">All shipments on your account, newest activity first.</div>
                </div>
                <div class="page-head-actions">
                    <a runat="server" href="~/Views/Track.aspx" class="btn btn-outline">Track by Control No.</a>
                    <asp:LinkButton ID="btnRequestShipment" runat="server" CssClass="btn btn-primary" OnClick="btnRequestShipment_Click" CausesValidation="false">+ Request Shipment</asp:LinkButton>
                </div>
            </div>
            <asp:Literal ID="litSummary" runat="server" />
            <asp:Panel ID="pnlVerifyError" runat="server" Visible="false">
                <div class="validation-summary">You need to verify your email address first before requesting a shipment. Please check your inbox for the verification email.</div>
            </asp:Panel>
            <div class="table-scroll">
            <asp:GridView ID="gvShipments" runat="server" AutoGenerateColumns="false" CssClass="data-grid data-grid-shipments" GridLines="None"
                            AllowPaging="true" PageSize="15" OnPageIndexChanging="gvShipments_PageIndexChanging" PagerStyle-CssClass="grid-pager"
                EmptyDataText="You don't have any shipments yet. Once our team creates a shipment on your account, it will show up here.">
                <Columns>
                    <asp:TemplateField HeaderText="Shipment">
                        <ItemTemplate><%# SoonestShipmentTrackingWebsite.Helpers.StatusDisplayHelper.ShipmentCell(Eval("ControlNumber"), Eval("ShipmentType"), Eval("CreatedDate")) %></ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Recipient">
                        <ItemTemplate><%# SoonestShipmentTrackingWebsite.Helpers.StatusDisplayHelper.RecipientCell(Eval("RecipientName"), Eval("RecipientPhone")) %></ItemTemplate>
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
                            <asp:HyperLink runat="server" NavigateUrl='<%# "~/Views/Customer/ShipmentDetails.aspx?id=" + Eval("Id") %>' CssClass="btn btn-sm" Text="Details" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
            </div>
        </div>
    </div>
</div>
</asp:Content>
