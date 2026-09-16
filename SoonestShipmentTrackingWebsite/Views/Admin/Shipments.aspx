<%@ Page Language="C#" MasterPageFile="~/Views/Site.Master" AutoEventWireup="true" CodeBehind="Shipments.aspx.cs" Inherits="SoonestShipmentTrackingWebsite.Views.Admin.Shipments" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">Manage Shipments</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div class="section" style="padding-top:36px;">
    <div class="container">
        <div class="card-panel">
            <div style="display:flex;justify-content:space-between;align-items:center;margin-bottom:16px;">
                <div class="page-heading" style="margin:0;">All Shipments</div>
                <a runat="server" href="~/Views/Admin/CreateShipment.aspx" class="btn btn-red">+ New Shipment</a>
            </div>

            <asp:GridView ID="gvShipments" runat="server" AutoGenerateColumns="false" CssClass="data-grid" GridLines="None"
                EmptyDataText="No shipments yet. Create one to get started.">
                <Columns>
                    <asp:BoundField DataField="ControlNumber" HeaderText="Control No." />
                    <asp:TemplateField HeaderText="Customer">
                        <ItemTemplate>
                            <%# Eval("CustomerName") %><br />
                            <span style="color:#6b7684;font-size:12px;"><%# Eval("CustomerEmail") %></span>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="RecipientName" HeaderText="Recipient" />
                    <asp:BoundField DataField="DestinationCity" HeaderText="Destination" />
                    <asp:TemplateField HeaderText="Status">
                        <ItemTemplate><%# SoonestShipmentTrackingWebsite.Helpers.StatusDisplayHelper.Badge((SoonestShipmentTrackingWebsite.Models.ShipmentStatus)Eval("CurrentStatus")) %></ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="UpdatedDate" HeaderText="Last Update" DataFormatString="{0:MMM d, h:mm tt}" />
                    <asp:TemplateField HeaderText="">
                        <ItemTemplate>
                            <asp:HyperLink runat="server" NavigateUrl='<%# "~/Views/Admin/UpdateStatus.aspx?id=" + Eval("Id") %>'
                                CssClass="btn btn-outline" Text="Update Status" style="padding:5px 12px;font-size:11.5px;border-color:#0b2540;color:#0b2540;" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </div>
</div>
</asp:Content>
