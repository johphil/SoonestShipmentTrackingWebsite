<%@ Page Language="C#" MasterPageFile="~/Views/Site.Master" AutoEventWireup="true" CodeBehind="Reports.aspx.cs" Inherits="SoonestShipmentTrackingWebsite.Views.Admin.Reports" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">Reports</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div class="section app-section" style="padding-top:44px;"><div class="container">
    <div class="card-panel" style="margin-bottom:20px;">
        <div class="page-heading">Shipment Reports</div>
        <div class="page-subheading">Filter operational shipments, delivery exceptions, and customer feedback.</div>
        <div style="display:grid;grid-template-columns:1fr 1fr 1fr;gap:14px;align-items:end;">
            <div class="form-group"><label>Status</label><asp:DropDownList ID="ddlStatus" runat="server" CssClass="form-control" /></div>
            <div class="form-group"><label>From date</label><asp:TextBox ID="txtFromDate" runat="server" CssClass="form-control" TextMode="Date" /></div>
            <div class="form-group"><label>To date</label><asp:TextBox ID="txtToDate" runat="server" CssClass="form-control" TextMode="Date" /></div>
        </div>
        <asp:Button ID="btnGenerate" runat="server" Text="Generate Report" CssClass="btn btn-red" OnClick="btnGenerate_Click" />
        <asp:Button ID="btnExport" runat="server" Text="Export CSV" CssClass="btn btn-outline" OnClick="btnExport_Click" />
        <asp:Label ID="lblError" runat="server" CssClass="field-error" Visible="false" />
    </div>
    <div style="display:grid;grid-template-columns:repeat(4,1fr);gap:14px;margin-bottom:20px;">
        <div class="card-panel"><strong>Total shipments</strong><h2><asp:Literal ID="litTotal" runat="server" /></h2></div>
        <div class="card-panel"><strong>Delivered</strong><h2><asp:Literal ID="litDelivered" runat="server" /></h2></div>
        <div class="card-panel"><strong>Out for delivery</strong><h2><asp:Literal ID="litOutForDelivery" runat="server" /></h2></div>
        <div class="card-panel"><strong>Issues reported</strong><h2><asp:Literal ID="litIssues" runat="server" /></h2></div>
    </div>
    <div class="card-panel">
        <asp:GridView ID="gvReport" runat="server" AutoGenerateColumns="false" CssClass="data-grid" GridLines="None" EmptyDataText="No shipments match the selected filters.">
            <Columns>
                <asp:BoundField DataField="ControlNumber" HeaderText="Control No." />
                <asp:BoundField DataField="CustomerName" HeaderText="Customer" />
                <asp:BoundField DataField="BranchOrigin" HeaderText="Branch Origin" />
                <asp:BoundField DataField="RecipientAddress" HeaderText="Recipient Address" />
                <asp:BoundField DataField="CurrentStatus" HeaderText="Status" />
                <asp:BoundField DataField="RiderName" HeaderText="Rider" />
                <asp:BoundField DataField="IsOrderReceived" HeaderText="Received" />
                <asp:BoundField DataField="IssueReported" HeaderText="Issue" />
                <asp:BoundField DataField="UpdatedDate" HeaderText="Last update" DataFormatString="{0:MMM d, yyyy h:mm tt}" />
            </Columns>
        </asp:GridView>
    </div>
</div></div>
</asp:Content>
