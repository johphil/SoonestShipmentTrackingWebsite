<%@ Page Language="C#" MasterPageFile="Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="SoonestShipmentTrackingWebsite.Views.Default" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">Home</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <section class="hero">
        <div class="container">
            <div class="hero-copy">
                <div class="eyebrow"><span class="eyebrow-bar"></span>FASTER, SAFER, GLOBAL</div>
                <h1>Logistics that move<br /><span>at your speed.</span></h1>
                <p>Soonest Global Express Corporation delivers your packages across borders with speed, security, and care. From local shipments to international deliveries, we keep you connected to what matters.</p>
                <div class="hero-benefits">
                    <div><span class="benefit-icon"><img src="../Assets/icons/globe.svg" alt="" /></span><span>International<br />Shipping</span></div>
                    <div><span class="benefit-icon"><img src="../Assets/icons/shield.svg" alt="" /></span><span>Safe &amp; Secure<br />Handling</span></div>
                    <div><span class="benefit-icon"><img src="../Assets/icons/track.svg" alt="" /></span><span>Real-Time<br />Tracking</span></div>
                    <div><span class="benefit-icon"><img src="../Assets/icons/support.svg" alt="" /></span><span>Dedicated<br />Support</span></div>
                </div>
            </div>
            <div class="hero-track-card">
                <div class="track-card-heading"><span class="track-card-icon"><img src="../Assets/icons/package.svg" alt="" /></span><div><h2>Track Your Shipment</h2><p>Enter your tracking number to get real-time updates.</p></div></div>
                <div class="track-box">
                    <asp:TextBox ID="txtControlNumber" runat="server" placeholder="Enter tracking number" />
                    <asp:Button ID="btnTrack" runat="server" CssClass="btn btn-track" Text="Track Shipment" OnClick="btnTrack_Click" />
                </div>
            </div>
        </div>
    </section>

    <section class="section about-reference">
        <div class="container two-col">
            <div class="about-photo"><img src="../Assets/soonesttruck_400.png" alt="Warehouse logistics operations" /></div>
            <div>
                <div class="reference-kicker">ABOUT US</div>
                <h2 class="reference-heading">Moving Packages.<br /><span>Connecting People.</span></h2>
                <p class="section-lead">Soonest Global Express Corporation provides dependable logistics solutions for local and international shipments. Our goal is to make shipping easier through secure handling, clear communication, and reliable delivery.</p>
                <p class="section-lead">With experienced teams, trusted partners, and branches across the Philippines, we connect businesses and families to the people and places that matter most.</p>
                <a runat="server" href="~/Views/Account/Register.aspx" class="btn btn-red btn-sm">Learn More <img class="btn-arrow" src="../Assets/icons/arrow-right.svg" alt="" /></a>
            </div>
        </div>
    </section>

    <section class="section section-alt services-reference">
        <div class="container">
            <div class="reference-kicker">WHAT WE DO</div>
            <h2 class="reference-heading">Logistics solutions<br /><span>built around you.</span></h2>
            <div class="card-grid service-cards">
                <div class="speed-card"><div class="service-round"><img src="../Assets/icons/globe.svg" alt="" /></div><h4>International Shipping</h4><p>Move shipments across borders with dependable freight and customs support.</p></div>
                <div class="speed-card"><div class="service-round"><img src="../Assets/icons/package.svg" alt="" /></div><h4>Domestic Delivery</h4><p>Fast, secure delivery coverage across Metro Manila, Luzon, Visayas, and Mindanao.</p></div>
                <div class="speed-card"><div class="service-round"><img src="../Assets/icons/track.svg" alt="" /></div><h4>Real-Time Tracking</h4><p>Follow every milestone and stay informed from pickup to final delivery.</p></div>
                <div class="speed-card"><div class="service-round"><img src="../Assets/icons/support.svg" alt="" /></div><h4>Dedicated Support</h4><p>Get practical assistance from a team that keeps your shipment moving.</p></div>
            </div>
        </div>
    </section>

    <section class="stats-band"><div class="container"><div class="card-grid">
        <div class="stat-item"><div class="stat-number">20,000+</div><div class="stat-label">Delivered Packages</div></div>
        <div class="stat-item"><div class="stat-number">500K+</div><div class="stat-label">KM Per Year</div></div>
        <div class="stat-item"><div class="stat-number">3,200+</div><div class="stat-label">Projects Done</div></div>
        <div class="stat-item"><div class="stat-number">30</div><div class="stat-label">Branches Nationwide</div></div>
    </div></div></section>

    <section class="section text-center"><div class="container cta-reference">
        <div class="reference-kicker">READY WHEN YOU ARE</div>
        <h2 class="reference-heading">Let's move what<br /><span>matters to you.</span></h2>
        <p class="section-lead">Create a customer account to manage and track every shipment in one place.</p>
        <a runat="server" href="~/Views/Account/Register.aspx" class="btn btn-red btn-sm">Get Started <img class="btn-arrow" src="../Assets/icons/arrow-right.svg" alt="" /></a>
        <a runat="server" href="~/Views/Track.aspx" class="btn btn-navy btn-sm">Track a Shipment</a>
    </div></section>
</asp:Content>
