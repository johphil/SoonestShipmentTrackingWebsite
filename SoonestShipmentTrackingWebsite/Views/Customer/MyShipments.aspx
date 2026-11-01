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
                    <asp:LinkButton ID="btnRequestShipment" runat="server" CssClass="btn btn-red" OnClick="btnRequestShipment_Click" CausesValidation="false">+ Request Shipment</asp:LinkButton>
                </div>
            </div>
            <asp:Literal ID="litSummary" runat="server" />
            <asp:Panel ID="pnlVerifyError" runat="server" Visible="false">
                <div class="validation-summary">You need to verify your email address first before requesting a shipment. Please check your inbox for the verification email.</div>
            </asp:Panel>
            <div class="table-scroll">
            <asp:GridView ID="gvShipments" runat="server" AutoGenerateColumns="false" CssClass="data-grid" GridLines="None"
                            AllowPaging="true" PageSize="15" OnPageIndexChanging="gvShipments_PageIndexChanging" PagerStyle-CssClass="grid-pager"
                EmptyDataText="You don't have any shipments yet. Once our team creates a shipment on your account, it will show up here.">
                <Columns>
                    <asp:TemplateField HeaderText="Control No.">
                        <ItemTemplate>
                            <span class="cell-main mono"><%# Eval("ControlNumber") %></span>
                            <span class="cell-sub">Created <%# Eval("CreatedDate", "{0:MMM d, yyyy}") %></span>
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
                            <asp:HyperLink runat="server" NavigateUrl='<%# "~/Views/Customer/ShipmentDetails.aspx?id=" + Eval("Id") %>'
                                CssClass="btn btn-outline" Text="Details" style="border-color:#07233d;color:#07233d;" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
            </div>
        </div>
    </div>
</div>
</asp:Content>
