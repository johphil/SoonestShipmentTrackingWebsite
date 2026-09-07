<%@ Page Language="C#" MasterPageFile="Site.Master" AutoEventWireup="true" CodeBehind="Track.aspx.cs" Inherits="SoonestShipmentTrackingWebsite.Views.Track" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">Track Shipment</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div class="section" style="padding-top:36px;">
    <div class="container">

        <div class="card-panel" style="margin-bottom:20px;">
            <div class="page-heading">Track Your Shipment</div>
            <div class="page-subheading">Enter your Control No. to see the current status and full history.</div>

            <div style="display:flex;gap:10px;">
                <asp:TextBox ID="txtControlNumber" runat="server" CssClass="form-control" placeholder="Enter Control No." />
                <asp:Button ID="btnSearch" runat="server" CssClass="btn btn-red" Text="Track" OnClick="btnSearch_Click" style="white-space:nowrap;" />
            </div>

            <asp:Panel ID="pnlNotFound" runat="server" Visible="false">
                <div class="alert-warning" style="margin-top:16px;">No shipment found for that Control No. Please check the number and try again.</div>
            </asp:Panel>
        </div>

        <asp:Panel ID="pnlResult" runat="server" Visible="false">
            <div class="card-panel" style="margin-bottom:20px;">
                <div style="display:flex;justify-content:space-between;align-items:flex-start;">
                    <div>
                        <h2 class="mt-0" style="color:#0b2540;margin-bottom:4px;"><asp:Literal ID="litControlNumber" runat="server" /></h2>
                        <p style="color:#6b7684;margin:0;">To <asp:Literal ID="litRecipientName" runat="server" />, <asp:Literal ID="litDestinationCity" runat="server" /></p>
                    </div>
                    <asp:Literal ID="litStatusBadge" runat="server" />
                </div>
                <hr style="border:none;border-top:1px solid #eceef1;margin:16px 0;" />
                <p class="mb-0"><strong>Estimated Delivery:</strong> <asp:Literal ID="litEta" runat="server" /></p>
            </div>

            <div class="card-panel">
                <h4 style="color:#0b2540;margin-top:0;">Shipment History</h4>
                <asp:Repeater ID="rptHistory" runat="server">
                    <HeaderTemplate><table class="data-grid"><thead><tr><th>Status</th><th>Location</th><th>Date</th></tr></thead><tbody></HeaderTemplate>
                    <ItemTemplate>
                        <tr>
                            <%--<td><%# SoonestGlobalExpress.Helpers.StatusDisplayHelper.Badge((SoonestGlobalExpress.Models.ShipmentStatus)Eval("Status")) %></td>--%>
                            <td><%# Eval("Location") %></td>
                            <td><%# Eval("Timestamp", "{0:MMM d, yyyy h:mm tt}") %></td>
                        </tr>
                    </ItemTemplate>
                    <FooterTemplate></tbody></table></FooterTemplate>
                </asp:Repeater>
                <asp:Label ID="lblNoHistory" runat="server" Text="No tracking events recorded yet." Visible="false" style="color:#6b7684;" />
            </div>
        </asp:Panel>

    </div>
</div>
</asp:Content>
