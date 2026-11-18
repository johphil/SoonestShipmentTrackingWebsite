
<%@ Page Language="C#" MasterPageFile="~/Views/Site.Master" AutoEventWireup="true" CodeBehind="RequestShipment.aspx.cs" Inherits="SoonestShipmentTrackingWebsite.Views.Customer.RequestShipment" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Request Shipment
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

<div class="section app-section request-shipment-section">
    <div class="container request-shipment-container">

        <div class="card-panel request-shipment-card">

            <!-- PAGE HEADER -->
            <div class="request-header">
                <div class="page-heading">Request Shipment</div>
                <p>
                    Complete the shipment details below. Your request will be
                    reviewed by our staff and updated once approved.
                </p>
            </div>

            <!-- ERROR MESSAGE -->
            <asp:Panel ID="pnlError" runat="server" Visible="false">
                <div class="validation-summary">
                    <asp:Literal ID="litError" runat="server" />
                </div>
            </asp:Panel>

            <!-- SENDER INFORMATION -->
            <div class="form-grid-2">

                <div class="form-group">
                    <label>Sender Name</label>
                    <asp:TextBox
                        ID="txtSenderName"
                        runat="server"
                        CssClass="form-control"
                        placeholder="Enter sender name" />

                    <asp:RequiredFieldValidator
                        runat="server"
                        ControlToValidate="txtSenderName"
                        Display="Dynamic"
                        CssClass="field-error"
                        ErrorMessage="Required."
                        ValidationGroup="RequestShipment" />
                </div>

                <div class="form-group">
                    <label>Shipment Type</label>
                    <asp:DropDownList
                        ID="ddlShipmentType"
                        runat="server"
                        CssClass="form-control" />

                    <asp:RequiredFieldValidator
                        runat="server"
                        ControlToValidate="ddlShipmentType"
                        InitialValue=""
                        Display="Dynamic"
                        CssClass="field-error"
                        ErrorMessage="Please select shipment type."
                        ValidationGroup="RequestShipment" />
                </div>

            </div>

            <!-- DOMESTIC SHIPMENT -->
            <div id="domesticFields" class="form-group shipment-conditional">
                <label>Branch Origin</label>

                <asp:DropDownList
                    ID="ddlBranchOrigin"
                    runat="server"
                    CssClass="form-control" />
            </div>

            <!-- INTERNATIONAL SHIPMENT -->
            <div id="internationalFields" class="shipment-conditional">

                <div class="form-grid-2">

                    <div class="form-group">
                        <label>Container Number</label>

                        <asp:TextBox
                            ID="txtContainerNumber"
                            runat="server"
                            CssClass="form-control"
                            MaxLength="100"
                            placeholder="Enter container number" />
                    </div>

                    <div class="form-group">
                        <label>Packing List Image <span class="optional-label">(Optional)</span></label>

                        <asp:FileUpload
                            ID="fuPackingListImage"
                            runat="server"
                            CssClass="form-control"
                            accept=".jpg,.jpeg,.png,.gif,.bmp,.webp" />
                    </div>

                </div>

            </div>

            <!-- RECIPIENT INFORMATION -->
            <div class="form-group">
                <label>Recipient Name</label>

                <asp:TextBox
                    ID="txtRecipientName"
                    runat="server"
                    CssClass="form-control"
                    placeholder="Enter recipient name" />

                <asp:RequiredFieldValidator
                    runat="server"
                    ControlToValidate="txtRecipientName"
                    Display="Dynamic"
                    CssClass="field-error"
                    ErrorMessage="Required."
                    ValidationGroup="RequestShipment" />
            </div>

            <div class="form-group">
                <label>Recipient Address</label>

                <asp:TextBox
                    ID="txtRecipientAddress"
                    runat="server"
                    CssClass="form-control"
                    placeholder="Enter complete delivery address" />

                <asp:RequiredFieldValidator
                    runat="server"
                    ControlToValidate="txtRecipientAddress"
                    Display="Dynamic"
                    CssClass="field-error"
                    ErrorMessage="Required."
                    ValidationGroup="RequestShipment" />
            </div>

            <div class="form-group">
                <label>Region Area</label>

                <asp:DropDownList
                    ID="ddlRegionArea"
                    runat="server"
                    CssClass="form-control" />

                <asp:RequiredFieldValidator
                    runat="server"
                    ControlToValidate="ddlRegionArea"
                    InitialValue=""
                    Display="Dynamic"
                    CssClass="field-error"
                    ErrorMessage="Please select a region area."
                    ValidationGroup="RequestShipment" />
            </div>

            <!-- CONTACT INFORMATION -->
            <div class="form-grid-2">

                <div class="form-group">
                    <label>Recipient Mobile Number</label>

                    <div class="phone-input-group">
                        <span class="phone-prefix">+63</span>

                        <asp:TextBox
                            ID="txtRecipientPhone"
                            runat="server"
                            CssClass="form-control phone-input"
                            placeholder="9XX XXX XXXX" />
                    </div>

                    <asp:RequiredFieldValidator
                        runat="server"
                        ControlToValidate="txtRecipientPhone"
                        Display="Dynamic"
                        CssClass="field-error"
                        ErrorMessage="Required."
                        ValidationGroup="RequestShipment" />
                </div>

                <div class="form-group">
                    <label>Recipient Email</label>

                    <asp:TextBox
                        ID="txtRecipientEmail"
                        runat="server"
                        CssClass="form-control"
                        TextMode="Email"
                        placeholder="Enter email address" />

                    <asp:RequiredFieldValidator
                        runat="server"
                        ControlToValidate="txtRecipientEmail"
                        Display="Dynamic"
                        CssClass="field-error"
                        ErrorMessage="Required."
                        ValidationGroup="RequestShipment" />
                </div>

            </div>

            <!-- PACKAGE INFORMATION -->
            <div class="package-information-section">

                <div class="package-section-header">
                    <div>
                        <h4>Package Information</h4>
                        <p>Select all applicable package descriptions and provide the package details.</p>
                    </div>
                </div>

                <div class="package-information-grid">

                    <!-- PACKAGE DESCRIPTION -->
                    <div class="form-group package-description-group">

                        <label>Package Description</label>

                        <div class="package-checklist-wrapper">

                            <asp:CheckBoxList
                                ID="cblPackageDescription"
                                runat="server"
                                RepeatLayout="Flow"
                                RepeatDirection="Horizontal"
                                CssClass="package-checklist" />

                        </div>

                        <asp:CustomValidator
                            ID="cvPackageDescription"
                            runat="server"
                            Display="Dynamic"
                            CssClass="field-error"
                            ErrorMessage="Please select at least one package description."
                            ValidationGroup="RequestShipment"
                            OnServerValidate="cvPackageDescription_ServerValidate" />

                    </div>

                    <!-- WEIGHT -->
                    <div class="form-group">
                        <label>Weight (kg)</label>

                        <asp:TextBox
                            ID="txtWeight"
                            runat="server"
                            CssClass="form-control"
                            placeholder="Enter weight" />

                        <asp:RequiredFieldValidator
                            runat="server"
                            ControlToValidate="txtWeight"
                            Display="Dynamic"
                            CssClass="field-error"
                            ErrorMessage="Required."
                            ValidationGroup="RequestShipment" />
                    </div>

                    <!-- DELIVERY DATE -->
                    <div class="form-group">
                        <label>Preferred Delivery Date</label>

                        <asp:TextBox
                            ID="txtEstDelivery"
                            runat="server"
                            CssClass="form-control"
                            TextMode="Date" />

                        <asp:RequiredFieldValidator
                            runat="server"
                            ControlToValidate="txtEstDelivery"
                            Display="Dynamic"
                            CssClass="field-error"
                            ErrorMessage="Required."
                            ValidationGroup="RequestShipment" />
                    </div>

                </div>

            </div>

            <!-- FORM ACTIONS -->
            <div class="request-form-actions">

                <asp:Button
                    ID="btnRequest"
                    runat="server"
                    CssClass="btn btn-red btn-submit-request"
                    Text="Submit Request"
                    OnClick="btnRequest_Click"
                    ValidationGroup="RequestShipment" />

                <a
                    runat="server"
                    href="~/Views/Customer/MyShipments.aspx"
                    class="btn btn-outline btn-cancel-request">
                    Cancel
                </a>

            </div>

        </div>

    </div>
</div>

<style>

/* =========================================
   REQUEST SHIPMENT PAGE
========================================= */

.request-shipment-section {
    padding-top: 44px;
    padding-bottom: 50px;
}

.request-shipment-container {
    max-width: 900px;
}

.request-shipment-card {
    padding: 28px;
}

.request-header {
    margin-bottom: 25px;
}

.request-header .page-heading {
    margin-bottom: 7px;
}

.request-header p {
    color: #64748b;
    font-size: 13px;
    line-height: 1.6;
    margin: 0;
}

.optional-label {
    font-size: 11px;
    color: #94a3b8;
    font-weight: 400;
}

/* =========================================
   FORM GRID
========================================= */

.form-grid-2 {
    display: grid;
    grid-template-columns: repeat(2, minmax(0, 1fr));
    gap: 16px;
}

.form-group {
    min-width: 0;
    margin-bottom: 18px;
}

.form-group > label {
    display: block;
    margin-bottom: 7px;
    font-size: 13px;
    font-weight: 600;
    color: #334155;
}

.form-control {
    width: 100%;
    min-width: 0;
    box-sizing: border-box;
}

.shipment-conditional {
    margin-bottom: 18px;
}

/* =========================================
   PACKAGE INFORMATION
========================================= */

.package-information-section {
    margin-top: 8px;
    margin-bottom: 24px;
    padding: 20px;
    background: #f8fafc;
    border: 1px solid #e2e8f0;
    border-radius: 10px;
}

.package-section-header {
    margin-bottom: 18px;
}

.package-section-header h4 {
    font-size: 15px;
    font-weight: 700;
    color: #1e293b;
    margin: 0 0 5px;
}

.package-section-header p {
    font-size: 12px;
    line-height: 1.5;
    color: #64748b;
    margin: 0;
}

.package-information-grid {
    display: grid;
    grid-template-columns:
        minmax(0, 1.5fr)
        minmax(100px, .65fr)
        minmax(155px, 1fr);
    gap: 16px;
    align-items: start;
}

.package-description-group {
    min-width: 0;
}

.package-checklist-wrapper {
    max-height: 240px;
    overflow-y: auto;
    overflow-x: hidden;
    padding: 12px;
    background: #ffffff;
    border: 1px solid #dbe4ed;
    border-radius: 9px;
    scrollbar-width: thin;
    scrollbar-color: #94a3b8 #f1f5f9;
}

/* Custom scrollbar - Chrome, Edge, Safari */
.package-checklist-wrapper::-webkit-scrollbar {
    width: 7px;
}

.package-checklist-wrapper::-webkit-scrollbar-track {
    background: #f1f5f9;
    border-radius: 10px;
}

.package-checklist-wrapper::-webkit-scrollbar-thumb {
    background: #94a3b8;
    border-radius: 10px;
}

/* CHECKBOX GRID */

.package-checklist {
    display: grid;
    grid-template-columns: repeat(2, minmax(0, 1fr));
    gap: 8px;
}

.package-checklist > span {
    display: block;
    min-width: 0;
    position: relative;
}

/* Hide default checkbox */

.package-checklist input[type="checkbox"] {
    position: absolute;
    opacity: 0;
    width: 1px;
    height: 1px;
    margin: 0;
}

/* Checkbox card */

.package-checklist label {
    display: flex;
    align-items: center;
    position: relative;
    box-sizing: border-box;
    min-height: 38px;
    padding: 8px 8px 8px 30px;
    margin: 0;
    border: 1px solid #e2e8f0;
    border-radius: 7px;
    background: #ffffff;
    color: #475569;
    font-size: 12px;
    font-weight: 500;
    line-height: 1.35;
    cursor: pointer;
    transition: all .15s ease;
}

/* Checkbox indicator */

.package-checklist label::before {
    content: "";
    position: absolute;
    left: 9px;
    top: 50%;
    transform: translateY(-50%);
    width: 14px;
    height: 14px;
    border: 1.5px solid #aebdce;
    border-radius: 4px;
    background: #ffffff;
    transition: all .15s ease;
}

/* Selected state */

.package-checklist input[type="checkbox"]:checked + label {
    background: #eff6ff;
    border-color: #3b82f6;
    color: #1d4ed8;
}

.package-checklist input[type="checkbox"]:checked + label::before {
    background: #2563eb;
    border-color: #2563eb;
}

/* Checkmark */

.package-checklist input[type="checkbox"]:checked + label::after {
    content: "";
    position: absolute;
    left: 14px;
    top: 50%;
    width: 4px;
    height: 8px;
    border: solid #ffffff;
    border-width: 0 2px 2px 0;
    transform: translateY(-65%) rotate(45deg);
}

/* Hover state */

.package-checklist label:hover {
    border-color: #93b4df;
    background: #f1f7ff;
}

/* Keyboard accessibility */

.package-checklist input[type="checkbox"]:focus-visible + label {
    outline: 3px solid rgba(37, 99, 235, .2);
    outline-offset: 2px;
}

/* =========================================
   PHONE INPUT
========================================= */

.phone-input-group {
    display: flex;
    align-items: stretch;
}

.phone-prefix {
    display: inline-flex;
    align-items: center;
    justify-content: center;
    min-width: 54px;
    height: 34px;
    padding: 0 12px;
    box-sizing: border-box;
    border: 1px solid #dbe6f0;
    border-right: 0;
    border-radius: 8px 0 0 8px;
    background: #f7fafd;
    color: #334d66;
    font-size: 13px;
    font-weight: 600;
}

.phone-input-group .phone-input {
    border-radius: 0 8px 8px 0;
}

/* =========================================
   FORM ACTIONS
========================================= */

.request-form-actions {
    display: flex;
    justify-content: flex-end;
    align-items: center;
    gap: 10px;
    padding-top: 20px;
    border-top: 1px solid #e5eaf0;
}

.btn-submit-request,
.btn-cancel-request {
    min-height: 38px;
    padding: 9px 20px;
    box-sizing: border-box;
    font-size: 13px;
    font-weight: 600;
    text-align: center;
}

.btn-cancel-request {
    border-color: #07233d !important;
    color: #07233d !important;
    text-decoration: none;
}

/* =========================================
   RESPONSIVE LAYOUT
========================================= */

@media (max-width: 700px) {

    .request-shipment-container {
        padding-left: 12px;
        padding-right: 12px;
    }

    .request-shipment-card {
        padding: 20px;
    }

    .package-information-grid {
        grid-template-columns: repeat(2, minmax(0, 1fr));
    }

    .package-description-group {
        grid-column: 1 / -1;
    }

}

@media (max-width: 520px) {

    .request-shipment-section {
        padding-top: 20px;
    }

    .request-shipment-card {
        padding: 16px;
    }

    .form-grid-2 {
        grid-template-columns: 1fr;
        gap: 0;
    }

    .package-information-section {
        padding: 14px;
    }

    .package-information-grid {
        grid-template-columns: 1fr;
        gap: 0;
    }

    .package-description-group {
        grid-column: auto;
    }

    .package-checklist {
        grid-template-columns: repeat(2, minmax(0, 1fr));
    }

    .request-form-actions {
        flex-direction: column-reverse;
        align-items: stretch;
    }

    .btn-submit-request,
    .btn-cancel-request {
        width: 100%;
    }

}

@media (max-width: 350px) {

    .package-checklist {
        grid-template-columns: 1fr;
    }

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

            if (domestic) {
                domestic.style.display = isDomestic ? '' : 'none';
            }

            if (international) {
                international.style.display = isInternational ? '' : 'none';
            }

        }

        if (ddl) {
            ddl.addEventListener('change', updateShipmentTypeFields);
            updateShipmentTypeFields();
        }

    })();
</script>

</asp:Content>
