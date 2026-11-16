<%@ Page Language="C#" MasterPageFile="~/Views/Site.Master" AutoEventWireup="true" CodeBehind="CreateShipment.aspx.cs" Inherits="SoonestShipmentTrackingWebsite.Views.Admin.CreateShipment" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">New Shipment</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div class="section app-section" style="padding-top:44px;">
    <div class="container" style="max-width:760px;">
        <div class="card-panel">
            <div class="page-heading">Create Shipment</div>

            <asp:Panel ID="pnlError" runat="server" Visible="false">
                <div class="validation-summary"><asp:Literal ID="litError" runat="server" /></div>
            </asp:Panel>

            <div class="form-group">
                <label>Customer</label>
                <asp:DropDownList ID="ddlCustomer" runat="server" CssClass="form-control" />
                <asp:RequiredFieldValidator runat="server" ControlToValidate="ddlCustomer" InitialValue="" Display="Dynamic" CssClass="field-error" ErrorMessage="Please select a customer." ValidationGroup="CreateShipment" />
            </div>

            <div style="display:grid;grid-template-columns:1fr 1fr;gap:16px;">
                <div class="form-group">
                    <label>Sender Name</label>
                    <asp:TextBox ID="txtSenderName" runat="server" CssClass="form-control" />
                    <asp:RequiredFieldValidator runat="server" ControlToValidate="txtSenderName" Display="Dynamic" CssClass="field-error" ErrorMessage="Required." ValidationGroup="CreateShipment" />
                </div>
                <div class="form-group">
                    <label>Shipment Type</label>
                    <asp:DropDownList ID="ddlShipmentType" runat="server" CssClass="form-control" />
                    <asp:RequiredFieldValidator runat="server" ControlToValidate="ddlShipmentType" InitialValue="" Display="Dynamic" CssClass="field-error" ErrorMessage="Please select shipment type." ValidationGroup="CreateShipment" />
                </div>
            </div>

            <div id="domesticFields" class="form-group" style="display:none;">
                <label>Branch Origin</label>
                <asp:DropDownList ID="ddlBranchOrigin" runat="server" CssClass="form-control" />
            </div>

            <div id="internationalFields" style="display:none;">
                <div style="display:grid;grid-template-columns:1fr 1fr;gap:16px;">
                    <div class="form-group">
                        <label>Container Number</label>
                        <asp:TextBox ID="txtContainerNumber" runat="server" CssClass="form-control" MaxLength="100" />
                    </div>
                    <div class="form-group">
                        <label>Packing List Image (optional)</label>
                        <asp:FileUpload ID="fuPackingListImage" runat="server" CssClass="form-control" accept=".jpg,.jpeg,.png,.gif,.bmp,.webp" />
                    </div>
                </div>
            </div>

            <div class="form-group">
                <label>Recipient Name</label>
                <asp:TextBox ID="txtRecipientName" runat="server" CssClass="form-control" />
                <asp:RequiredFieldValidator runat="server" ControlToValidate="txtRecipientName" Display="Dynamic" CssClass="field-error" ErrorMessage="Required." ValidationGroup="CreateShipment" />
            </div>

            <div class="form-group">
                <label>Recipient Address</label>
                <asp:TextBox ID="txtRecipientAddress" runat="server" CssClass="form-control" />
                <asp:RequiredFieldValidator runat="server" ControlToValidate="txtRecipientAddress" Display="Dynamic" CssClass="field-error" ErrorMessage="Required." ValidationGroup="CreateShipment" />
            </div>

            <div class="form-group">
                <label>Region Area</label>
                <asp:DropDownList ID="ddlRegionArea" runat="server" CssClass="form-control" />
                <asp:RequiredFieldValidator runat="server" ControlToValidate="ddlRegionArea" InitialValue="" Display="Dynamic" CssClass="field-error" ErrorMessage="Please select a region area." ValidationGroup="CreateShipment" />
            </div>

            <div style="display:grid;grid-template-columns:1fr 1fr;gap:16px;">
                <div class="form-group">
                    <label>Recipient Mobile Number</label>
                    <div class="phone-input-group">
                        <span class="phone-prefix">+63</span>
                        <asp:TextBox ID="txtRecipientPhone" runat="server" CssClass="form-control phone-input" placeholder="9XX XXX XXXX" />
                    </div>
                    <asp:RequiredFieldValidator runat="server" ControlToValidate="txtRecipientPhone" Display="Dynamic" CssClass="field-error" ErrorMessage="Required." ValidationGroup="CreateShipment" />
                </div>
                <div class="form-group">
                    <label>Recipient Email</label>
                    <asp:TextBox ID="txtRecipientEmail" runat="server" CssClass="form-control" TextMode="Email" />
                    <asp:RequiredFieldValidator runat="server" ControlToValidate="txtRecipientEmail" Display="Dynamic" CssClass="field-error" ErrorMessage="Required." ValidationGroup="CreateShipment" />
                </div>
            </div>

            <div style="display:grid;grid-template-columns:1.4fr .8fr .8fr;gap:16px;">
                <div class="form-group">
                    <label>Package Description</label>
                    <asp:CheckBoxList ID="cblPackageDescription" runat="server" RepeatLayout="Flow" CssClass="package-checklist" />
                    <asp:CustomValidator ID="cvPackageDescription" runat="server" Display="Dynamic" CssClass="field-error" ErrorMessage="Please select at least one package description." ValidationGroup="CreateShipment" OnServerValidate="cvPackageDescription_ServerValidate" />
                </div>
                <div class="form-group">
                    <label>Weight (kg)</label>
                    <asp:TextBox ID="txtWeight" runat="server" CssClass="form-control" />
                    <asp:RequiredFieldValidator runat="server" ControlToValidate="txtWeight" Display="Dynamic" CssClass="field-error" ErrorMessage="Required." ValidationGroup="CreateShipment" />
                </div>
                <div class="form-group">
                    <label>Delivery Date</label>
                    <asp:TextBox ID="txtEstDelivery" runat="server" CssClass="form-control" TextMode="Date" />
                    <asp:RequiredFieldValidator runat="server" ControlToValidate="txtEstDelivery" Display="Dynamic" CssClass="field-error" ErrorMessage="Required." ValidationGroup="CreateShipment" />
                </div>
            </div>

            <asp:Button ID="btnCreate" runat="server" CssClass="btn btn-red" Text="Create Shipment" OnClick="btnCreate_Click" ValidationGroup="CreateShipment" />
            <a runat="server" href="~/Views/Admin/Shipments.aspx" class="btn btn-outline" style="border-color:#07233d;color:#07233d;">Cancel</a>
        </div>
    </div>
</div>

<style>
    .package-checklist label { display: block; font-weight: normal; padding: 5px 0; }
    .package-checklist input[type="checkbox"] { margin-right: 8px; }

    .phone-input-group {
        display: flex;
        align-items: center;
    }

    .phone-prefix {
        display: inline-flex;
        align-items: center;
        justify-content: center;
        min-width: 54px;
        height: 34px;
        padding: 0 12px;
        border: 1px solid #dbe6f0;
        border-right: 0;
        border-radius: 8px 0 0 8px;
        background: #f7fafd;
        color: #334d66;
        font-weight: 600;
    }

    .phone-input-group .phone-input {
        border-radius: 0 8px 8px 0;
    }
</style>
<script>
    (function () {
        var ddl = document.getElementById('<%= ddlShipmentType.ClientID %>');
        var domestic = document.getElementById('domesticFields');
        var international = document.getElementById('internationalFields');

        function updateShipmentTypeFields() {
            var v = ddl ? ddl.value : '';
            var isDomestic = v === 'DOMESTIC';
            var isInternational = v === 'INTERNATIONAL';
            if (domestic) domestic.style.display = isDomestic ? '' : 'none';
            if (international) international.style.display = isInternational ? '' : 'none';
        }

        if (ddl) {
            ddl.addEventListener('change', updateShipmentTypeFields);
            updateShipmentTypeFields();
        }
    })();
</script>
</asp:Content>
