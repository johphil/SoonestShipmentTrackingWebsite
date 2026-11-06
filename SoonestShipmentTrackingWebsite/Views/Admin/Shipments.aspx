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
            <asp:GridView ID="gvShipments" runat="server" AutoGenerateColumns="false" CssClass="data-grid" GridLines="None"
                            AllowPaging="true" PageSize="20" OnPageIndexChanging="gvShipments_PageIndexChanging" PagerStyle-CssClass="grid-pager"
                EmptyDataText="No shipments match this view.">
                <Columns>
                    <asp:TemplateField HeaderText="Control No.">
                        <ItemTemplate>
                            <span class="cell-main mono"><%# Eval("ControlNumber") %></span>
                            <span class="cell-sub">Created <%# Eval("CreatedDate", "{0:MMM d, yyyy}") %></span>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Customer">
                        <ItemTemplate>
                            <span class="cell-main"><%# Server.HtmlEncode(Convert.ToString(Eval("CustomerName"))) %></span>
                            <span class="cell-sub"><%# Server.HtmlEncode(Convert.ToString(Eval("CustomerEmail"))) %></span>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Recipient">
                        <ItemTemplate>
                            <span class="cell-main"><%# Server.HtmlEncode(Convert.ToString(Eval("RecipientName"))) %></span>
                            <span class="cell-sub"><%# Server.HtmlEncode(Convert.ToString(Eval("RecipientPhone"))) %></span>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Route">
                        <ItemTemplate>
                            <span class="cell-main"><%# Server.HtmlEncode(Convert.ToString(Eval("BranchOrigin"))) %></span>
                            <span class="cell-sub ellipsis" title='<%# Server.HtmlEncode(Convert.ToString(Eval("RecipientAddress"))) %>'>&rarr; <%# Server.HtmlEncode(Convert.ToString(Eval("RecipientAddress"))) %></span>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Package">
                        <ItemTemplate>
                            <span class="cell-main ellipsis"><%# SoonestShipmentTrackingWebsite.Helpers.StatusDisplayHelper.Enc(Convert.ToString(Eval("PackageDescription"))) %></span>
                            <span class="cell-sub"><%# SoonestShipmentTrackingWebsite.Helpers.StatusDisplayHelper.WeightText(Eval("WeightKg") as decimal?) %></span>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Status">
                        <ItemTemplate><%# SoonestShipmentTrackingWebsite.Helpers.StatusDisplayHelper.Badge((SoonestShipmentTrackingWebsite.Models.ShipmentStatus)Eval("CurrentStatus")) %></ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Issue">
                        <ItemTemplate><%# SoonestShipmentTrackingWebsite.Helpers.StatusDisplayHelper.IssueIndicator(Eval("IssueReported"), Eval("IsIssueResolved")) %></ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Rider">
                        <ItemTemplate><%# SoonestShipmentTrackingWebsite.Helpers.StatusDisplayHelper.RiderInfo(Eval("CurrentStatus"), Eval("RiderName")) %></ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Est. Delivery">
                        <ItemTemplate><span class="nowrap"><%# SoonestShipmentTrackingWebsite.Helpers.StatusDisplayHelper.EtaText(Eval("EstimatedDeliveryDate") as DateTime?, (SoonestShipmentTrackingWebsite.Models.ShipmentStatus)Eval("CurrentStatus")) %></span></ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Last Update">
                        <ItemTemplate>
                            <span class="nowrap"><%# Eval("UpdatedDate", "{0:MMM d, yyyy}") %></span>
                            <span class="cell-sub"><%# Eval("UpdatedDate", "{0:h:mm tt}") %></span>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="">
                        <ItemTemplate>
                            <asp:HyperLink runat="server" NavigateUrl='<%# "~/Views/Admin/UpdateStatus.aspx?id=" + Eval("Id") %>'
                                CssClass="btn btn-outline" Text="Update" style="border-color:#07233d;color:#07233d;" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
            </div>
        </div>
    </div>
</div>
</asp:Content>
