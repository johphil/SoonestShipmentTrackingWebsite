<%@ Page Language="C#" MasterPageFile="Site.Master" AutoEventWireup="true" CodeBehind="About.aspx.cs" Inherits="SoonestShipmentTrackingWebsite.Views.About" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">About Us</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <section class="hero" style="padding:44px 0;">
        <div class="container" style="grid-template-columns:1fr;text-align:center;">
            <div>
                <div class="eyebrow" style="justify-content:center;"><span class="eyebrow-bar"></span>ABOUT SOONEST GLOBAL EXPRESS</div>
                <h1 style="margin:14px auto 0;max-width:620px;">Moving Packages. <span>Connecting People.</span></h1>
            </div>
        </div>
    </section>

    <section class="section">
        <div class="container two-col">
            <div class="about-photo"><img src="../Assets/soonesttruck_400.png" alt="Warehouse logistics operations" /></div>
            <div>
                <div class="kicker"><span class="kicker-bar"></span>OUR STORY</div>
                <h2 class="section-heading">Built on trust, <span>delivered on time.</span></h2>
                <p class="section-lead">Soonest Global Express Corporation provides dependable logistics solutions for local and international shipments. What started as a small freight-forwarding team in Las Pinas has grown into a nationwide network trusted by thousands of families and businesses.</p>
                <p class="section-lead">Our goal is simple: make shipping easier through secure handling, clear communication, and reliable delivery &mdash; every time, no exceptions.</p>
            </div>
        </div>
    </section>

    <section class="section section-alt">
        <div class="container">
            <div class="card-grid" style="grid-template-columns:1fr 1fr;gap:24px;">
                <div class="card-panel">
                    <div class="icon-round icon-blue"><svg viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="1.8" stroke-linecap="round" stroke-linejoin="round"><path d="M22 12h-6l-2 3h-4l-2-3H2" /><path d="M5.5 5h13L22 12v6a2 2 0 01-2 2H4a2 2 0 01-2-2v-6L5.5 5z" /></svg></div>
                    <h4 style="font-size:18px;color:#1f2937;margin:0 0 8px;">Our Mission</h4>
                    <p class="section-lead" style="margin:0;">To connect people and businesses across the Philippines and the world through fast, secure, and dependable logistics &mdash; treating every package as if it were our own.</p>
                </div>
                <div class="card-panel">
                    <div class="icon-round"><svg viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="1.8" stroke-linecap="round" stroke-linejoin="round"><circle cx="12" cy="12" r="9" /><path d="M12 7v5l3.5 3.5" /></svg></div>
                    <h4 style="font-size:18px;color:#1f2937;margin:0 0 8px;">Our Vision</h4>
                    <p class="section-lead" style="margin:0;">To be the most trusted logistics partner in Southeast Asia, known for our reliability, transparency, and the care we bring to every delivery.</p>
                </div>
            </div>
        </div>
    </section>

    <section class="section">
        <div class="container page-title-banner">
            <div class="kicker"><span class="kicker-bar"></span>WHY CHOOSE US</div>
            <h2 class="section-heading">What sets us <span>apart.</span></h2>
        </div>
        <div class="container">
            <div class="card-grid" style="margin-top:34px;">
                <div class="speed-card">
                    <div class="icon-round"><svg viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="1.8" stroke-linecap="round" stroke-linejoin="round"><path d="M12 3l7 3v6c0 4.5-3 7-7 9-4-2-7-4.5-7-9V6l7-3z" /></svg></div>
                    <h4>25 Years of Trust</h4><p>A quarter century of experience keeping shipments safe from pickup to doorstep.</p>
                </div>
                <div class="speed-card">
                    <div class="icon-round"><svg viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="1.8" stroke-linecap="round"><circle cx="12" cy="12" r="9" /><ellipse cx="12" cy="12" rx="4" ry="9" /><path d="M3 12h18" /></svg></div>
                    <h4>30 Branches Nationwide</h4><p>A branch network across the Philippines means your shipment is never far from home.</p>
                </div>
                <div class="speed-card">
                    <div class="icon-round"><svg viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="1.8" stroke-linecap="round" stroke-linejoin="round"><circle cx="12" cy="12" r="9" /><path d="M12 7v5l3.5 3.5" /></svg></div>
                    <h4>Real-Time Tracking</h4><p>Every shipment is logged step-by-step, so you always know where it is.</p>
                </div>
                <div class="speed-card">
                    <div class="icon-round"><svg viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="1.8" stroke-linecap="round" stroke-linejoin="round"><path d="M4 13v-1a8 8 0 0116 0v1" /><rect x="2.5" y="13" width="4" height="6" rx="1.5" /><rect x="17.5" y="13" width="4" height="6" rx="1.5" /><path d="M20 19a4 4 0 01-4 3h-3" /></svg></div>
                    <h4>Dedicated Support</h4><p>A real team you can call, backed by fast responses when you need help.</p>
                </div>
            </div>
        </div>
    </section>

    <section class="stats-band"><div class="container"><div class="card-grid">
        <div class="stat-item" style="text-align:center;"><div class="stat-number">20,000+</div><div class="stat-label">Delivered Packages</div></div>
        <div class="stat-item" style="text-align:center;"><div class="stat-number">500K+</div><div class="stat-label">KM Per Year</div></div>
        <div class="stat-item" style="text-align:center;"><div class="stat-number">3,200+</div><div class="stat-label">Projects Done</div></div>
        <div class="stat-item" style="text-align:center;"><div class="stat-number">30</div><div class="stat-label">Branches Nationwide</div></div>
    </div></div></section>

    <section class="section text-center"><div class="container" style="max-width:600px;">
        <h2 class="section-heading">Ready to ship <span>with us?</span></h2>
        <p class="section-lead">Create a free account and start tracking your shipments today.</p>
        <div style="margin-top:18px;">
            <a runat="server" href="~/Views/Account/Register.aspx" class="btn btn-red">Create Account</a>
            <a runat="server" href="~/Views/Contact.aspx" class="btn btn-outline">Contact Us</a>
        </div>
    </div></section>

</asp:Content>
