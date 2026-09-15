<%@ Page Language="C#" MasterPageFile="~/Views/Site.Master" Async="true" AutoEventWireup="true" CodeBehind="UpdateStatus.aspx.cs" Inherits="SoonestShipmentTrackingWebsite.Views.Admin.UpdateStatus" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">Update Status</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div class="section" style="padding-top:36px;">
    <div class="container" style="max-width:760px;">

        <div class="card-panel" style="margin-bottom:20px;">
            <div style="display:flex;justify-content:space-between;align-items:flex-start;margin-bottom:14px;">
                <div>
                    <h2 class="mt-0" style="color:#0b2540;margin-bottom:4px;"><asp:Literal ID="litControlNumber" runat="server" /></h2>
                    <p style="color:#6b7684;margin:0;">To <asp:Literal ID="litRecipientName" runat="server" />, <asp:Literal ID="litDestinationCity" runat="server" /></p>
                </div>
                <asp:Literal ID="litCurrentBadge" runat="server" />
            </div>
            
            <asp:Panel ID="pnlError" runat="server" Visible="false">
                <div class="validation-summary"><asp:Literal ID="litError" runat="server" /></div>
            </asp:Panel>

            <div class="form-group">
                <label>New Status</label>
                <asp:DropDownList ID="ddlNewStatus" runat="server" CssClass="form-control" />
            </div>

            <div class="form-group">
                <label>Current Location</label>
                <asp:TextBox ID="txtLocation" runat="server" CssClass="form-control" placeholder="e.g. Las Piñas Sorting Hub" />
            </div>

            <div class="form-group">
                <label>Notes</label>
                <asp:TextBox ID="txtNotes" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="2" />
            </div>

            <asp:Button ID="btnSave" runat="server" CssClass="btn btn-red" Text="Save Status Update" OnClick="btnSave_Click" />
            <a runat="server" href="~/Views/Admin/Shipments.aspx" class="btn btn-outline" style="border-color:#0b2540;color:#0b2540;">Cancel</a>
        </div>

        <div class="card-panel">
            <h4 style="margin-top:0;color:#0b2540;">History</h4>
            <asp:Repeater ID="rptHistory" runat="server">
                <HeaderTemplate><table class="data-grid"><thead><tr><th>Status</th><th>Location</th><th>Notes</th><th>Date</th></tr></thead><tbody></HeaderTemplate>
                <ItemTemplate>
                    <tr>
                        <td><%# SoonestShipmentTrackingWebsite.Helpers.StatusDisplayHelper.Badge((SoonestShipmentTrackingWebsite.Models.ShipmentStatus)Eval("Status")) %></td>
                        <td><%# Eval("Location") %></td>
                        <td><%# Eval("Notes") %></td>
                        <td><%# Eval("Timestamp", "{0:MMM d, yyyy h:mm tt}") %></td>
                    </tr>
                </ItemTemplate>
                <FooterTemplate></tbody></table></FooterTemplate>
            </asp:Repeater>
            <asp:Label ID="lblNoHistory" runat="server" Text="No tracking events yet." Visible="false" style="color:#6b7684;" />
        </div>

    </div>
</div>
</asp:Content>
