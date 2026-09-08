<%@ Page Language="C#" MasterPageFile="~/Views/Site.Master" AutoEventWireup="true" CodeBehind="MyShipments.aspx.cs" Inherits="SoonestShipmentTrackingWebsite.Views.Customer.MyShipments" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">My Shipments</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div class="section" style="padding-top:36px;">
    <div class="container">
        <div class="card-panel">
            <div class="page-heading">My Shipments</div>

            <asp:GridView ID="gvShipments" runat="server" AutoGenerateColumns="false" CssClass="data-grid" GridLines="None"
                EmptyDataText="You don't have any shipments yet. Once our team creates a shipment on your account, it will show up here.">
                <Columns>
                    <asp:BoundField DataField="ControlNumber" HeaderText="Control No." />
                    <asp:BoundField DataField="RecipientName" HeaderText="Recipient" />
                    <asp:BoundField DataField="DestinationCity" HeaderText="Destination" />
                    <asp:TemplateField HeaderText="Status">
                        <ItemTemplate><%# SoonestShipmentTrackingWebsite.Helpers.StatusDisplayHelper.Badge((SoonestShipmentTrackingWebsite.Models.ShipmentStatus)Eval("CurrentStatus")) %></ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Est. Delivery">
                        <ItemTemplate><%# Eval("EstimatedDeliveryDate") != null ? Eval("EstimatedDeliveryDate", "{0:MMM d, yyyy}") : "-" %></ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="UpdatedDate" HeaderText="Last Update" DataFormatString="{0:MMM d, yyyy h:mm tt}" />
                    <asp:TemplateField HeaderText="">
                        <ItemTemplate>
                            <asp:HyperLink runat="server" NavigateUrl='<%# "~/Views/Customer/ShipmentDetails.aspx?id=" + Eval("Id") %>'
                                CssClass="btn btn-outline" Text="View History" style="padding:5px 12px;font-size:11.5px;border-color:#0b2540;color:#0b2540;" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </div>
</div>
</asp:Content>
