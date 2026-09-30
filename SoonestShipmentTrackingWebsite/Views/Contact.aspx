<%@ Page Language="C#" MasterPageFile="Site.Master" AutoEventWireup="true" CodeBehind="Contact.aspx.cs" Inherits="SoonestShipmentTrackingWebsite.Views.Contact" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">Contact Us</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <section class="hero" style="padding:44px 0;">
        <div class="container" style="grid-template-columns:1fr;text-align:center;">
            <div>
                <div class="eyebrow" style="justify-content:center;"><span class="eyebrow-bar"></span>WE&rsquo;RE HERE TO HELP</div>
                <h1 style="margin:14px auto 0;max-width:560px;">Get In Touch <span>With Our Team</span></h1>
                <p style="margin:14px auto 0;text-align:center;">Questions about a shipment, a branch, or setting up an account? Send us a message and we&rsquo;ll get back to you shortly.</p>
            </div>
        </div>
    </section>

    <section class="section">
        <div class="container">
            <div class="two-col" style="grid-template-columns:.9fr 1.1fr;align-items:start;">

                <div>
                    <div class="kicker"><span class="kicker-bar"></span>CONTACT INFO</div>
                    <h2 class="section-heading" style="font-size:26px;">Reach us <span>directly.</span></h2>

                    <div class="contact-info-list" style="margin-top:20px;">
                        <div class="contact-info-item">
                            <div class="icon-round icon-blue"><svg viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="1.8" stroke-linecap="round" stroke-linejoin="round"><path d="M22 16.9v3a2 2 0 01-2.2 2 19.8 19.8 0 01-8.6-3 19.5 19.5 0 01-6-6 19.8 19.8 0 01-3-8.7A2 2 0 014.1 2h3a2 2 0 012 1.7c.1 1 .3 2 .7 3a2 2 0 01-.5 2L8 10a16 16 0 006 6l1.3-1.3a2 2 0 012-.5c1 .4 2 .6 3 .7a2 2 0 011.7 2z" /></svg></div>
                            <div><h5>Call Us</h5><p>0987 654 3210<br />Mon&ndash;Sat, 9:00 AM &ndash; 7:00 PM</p></div>
                        </div>
                        <div class="contact-info-item">
                            <div class="icon-round icon-blue"><svg viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="1.8" stroke-linecap="round" stroke-linejoin="round"><path d="M4 4h16v16H4z" /><path d="M4 6l8 7 8-7" /></svg></div>
                            <div><h5>Email Us</h5><p>info@soonestglobal.com</p></div>
                        </div>
                        <div class="contact-info-item">
                            <div class="icon-round icon-blue"><svg viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="1.8" stroke-linecap="round" stroke-linejoin="round"><path d="M12 22s7-7.4 7-12.5A7 7 0 105 9.5C5 14.6 12 22 12 22z" /><circle cx="12" cy="9.5" r="2.3" /></svg></div>
                            <div><h5>Visit Us</h5><p>Jerusalem Street, BF Martinville Manuyo Dos,<br />Las Pi&ntilde;as City, Philippines</p></div>
                        </div>
                        <div class="contact-info-item">
                            <div class="icon-round icon-blue"><svg viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="1.8" stroke-linecap="round" stroke-linejoin="round"><circle cx="12" cy="12" r="9" /><path d="M12 7v5l3.5 3.5" /></svg></div>
                            <div><h5>Office Hours</h5><p>Monday &ndash; Saturday, 9:00 AM &ndash; 7:00 PM<br />Closed on Sundays and public holidays</p></div>
                        </div>
                    </div>

                    <div class="contact-map">
                        <iframe src="https://www.google.com/maps/embed?pb=!1m18!1m12!1m3!1d3697.3050371882305!2d120.99718697484197!3d14.46708218033218!2m3!1f0!2f0!3f0!3m2!1i1024!2i768!4f13.1!3m3!1m2!1s0x3397ce6df296db7b%3A0x26d09e52a60ce058!2sSoonest%20Global%20Express%20Corp!5e1!3m2!1sen!2sph!4v1790752807036!5m2!1sen!2sph" width="600" height="450" style="border:0;" allowfullscreen="" loading="lazy" referrerpolicy="strict-origin-when-cross-origin" title="Soonest Global Express location"></iframe>
                    </div>
                </div>

                <div class="card-panel">
                    <h3 style="margin-top:0;color:#1f2937;font-size:19px;">Send Us a Message</h3>

                    <asp:Panel ID="pnlFormSuccess" runat="server" Visible="false" CssClass="alert-success" style="margin-bottom:16px;">
                        Thanks for reaching out! Our team will get back to you shortly.
                    </asp:Panel>

                    <asp:Panel ID="pnlFormError" runat="server" Visible="false" CssClass="validation-summary" style="margin-bottom:16px;">
                        <asp:Literal ID="litFormError" runat="server" />
                    </asp:Panel>

                    <asp:Panel ID="pnlContactForm" runat="server">
                        <div class="form-row2" style="display:grid;grid-template-columns:1fr 1fr;gap:16px;">
                            <div class="form-group">
                                <label>Full Name</label>
                                <asp:TextBox ID="txtName" runat="server" CssClass="form-control" placeholder="Juan Dela Cruz" />
                                <asp:RequiredFieldValidator runat="server" ControlToValidate="txtName" Display="Dynamic" CssClass="field-error" ErrorMessage="Enter your name." ValidationGroup="Contact" />
                            </div>
                            <div class="form-group">
                                <label>Email Address</label>
                                <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" placeholder="you@email.com" />
                                <asp:RequiredFieldValidator runat="server" ControlToValidate="txtEmail" Display="Dynamic" CssClass="field-error" ErrorMessage="Enter your email." ValidationGroup="Contact" />
                                <asp:RegularExpressionValidator runat="server" ControlToValidate="txtEmail" Display="Dynamic" CssClass="field-error" ErrorMessage="Enter a valid email address." ValidationExpression="^[^@\s]+@[^@\s]+\.[^@\s]+$" ValidationGroup="Contact" />
                            </div>
                        </div>
                        <div class="form-group">
                            <label>Phone Number <span style="font-weight:400;color:#64748b;">(optional)</span></label>
                            <asp:TextBox ID="txtPhone" runat="server" CssClass="form-control" placeholder="+63 900 000 0000" />
                        </div>
                        <div class="form-group">
                            <label>Subject</label>
                            <asp:TextBox ID="txtSubject" runat="server" CssClass="form-control" placeholder="What is this regarding?" />
                            <asp:RequiredFieldValidator runat="server" ControlToValidate="txtSubject" Display="Dynamic" CssClass="field-error" ErrorMessage="Enter a subject." ValidationGroup="Contact" />
                        </div>
                        <div class="form-group">
                            <label>Message</label>
                            <asp:TextBox ID="txtMessage" runat="server" CssClass="form-control" TextMode="MultiLine" Rows="5" placeholder="How can we help?" />
                            <asp:RequiredFieldValidator runat="server" ControlToValidate="txtMessage" Display="Dynamic" CssClass="field-error" ErrorMessage="Enter a message." ValidationGroup="Contact" />
                        </div>
                        <asp:Button ID="btnSend" runat="server" CssClass="btn btn-red" Text="Send Message" OnClick="btnSend_Click" ValidationGroup="Contact" />
                    </asp:Panel>
                </div>

            </div>
        </div>
    </section>

</asp:Content>
