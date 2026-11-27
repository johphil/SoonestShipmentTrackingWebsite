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
                        CssClass="btn btn-primary btn-submit-request"
                        Text="Submit Request"
                        OnClick="btnRequest_Click"
                        ValidationGroup="RequestShipment" />

                    <a
                        runat="server"
                        href="~/Views/Customer/MyShipments.aspx"
                        class="btn btn-outline btn-cancel-request">Cancel
                </a>

                </div>

            </div>

        </div>
    </div>

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
