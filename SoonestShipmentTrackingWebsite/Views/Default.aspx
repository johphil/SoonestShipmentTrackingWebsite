<%@ Page Language="C#" MasterPageFile="Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="SoonestShipmentTrackingWebsite.Views.Default" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">Home</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <section class="hero hero-split">
        <div class="container">
            <div class="hero-copy">
                <div class="eyebrow"><span class="eyebrow-bar"></span>FASTER, SAFER, GLOBAL</div>
                <h1>Logistics that move <span>at your speed.</span></h1>
                <p>Soonest Global Express Corporation delivers your packages across borders with speed, security, and care. From local shipments to international deliveries, we keep you connected to what matters.</p>
                <div class="hero-actions">
                    <a runat="server" href="~/Views/Account/Register.aspx" class="btn btn-light btn-lg">Create Account <img class="btn-arrow" src="../Assets/icons/arrow-right.svg" alt="" style="filter:none;" /></a>
                    <a runat="server" href="~/Views/Contact.aspx" class="btn btn-ghost btn-lg">Talk to Us</a>
                </div>
                <div class="hero-benefits">
                    <div><span class="benefit-icon"><img src="../Assets/icons/globe.svg" alt="" /></span><span>International Shipping</span></div>
                    <div><span class="benefit-icon"><img src="../Assets/icons/shield.svg" alt="" /></span><span>Safe &amp; Secure Handling</span></div>
                    <div><span class="benefit-icon"><img src="../Assets/icons/track.svg" alt="" /></span><span>Real-Time Tracking</span></div>
                    <div><span class="benefit-icon"><img src="../Assets/icons/support.svg" alt="" /></span><span>Dedicated Support</span></div>
                </div>
            </div>
            <div class="hero-photo">
                <img src="../Assets/about-1.png" alt="Warehouse package handling" />
                <div class="hero-photo-badge"><b>30</b><span>branches<br />nationwide</span></div>
            </div>
        </div>
    </section>

    <section class="track-strip">
        <div class="container">
            <div class="track-strip-card">
                <div class="track-strip-title">
                    <span class="track-card-icon"><img src="../Assets/icons/package.svg" alt="" /></span>
                    <div><h2>Track Your Shipment</h2><p>Enter your tracking number for real-time updates.</p></div>
                </div>
                <div class="track-box">
                    <asp:TextBox ID="txtControlNumber" runat="server" placeholder="Enter tracking number" />
                    <asp:Button ID="btnTrack" runat="server" CssClass="btn btn-track" Text="Track Shipment" OnClick="btnTrack_Click" />
                </div>
            </div>
        </div>
    </section>

    <section class="section services-reference">
        <div class="container">
            <div class="section-head">
                <div>
                    <div class="reference-kicker">WHAT WE DO</div>
                    <h2 class="section-heading">Logistics solutions <span>built around you.</span></h2>
                </div>
                <p class="section-lead">Everything you need to send, receive, and follow a shipment, handled by one dependable team.</p>
            </div>
            <div class="card-grid service-cards">
                <div class="speed-card"><div class="service-round"><img src="../Assets/icons/globe.svg" alt="" /></div><h4>International Shipping</h4><p>Move shipments across borders with dependable freight and customs support.</p></div>
                <div class="speed-card"><div class="service-round"><img src="../Assets/icons/package.svg" alt="" /></div><h4>Domestic Delivery</h4><p>Fast, secure delivery coverage across Metro Manila, Luzon, Visayas, and Mindanao.</p></div>
                <div class="speed-card"><div class="service-round"><img src="../Assets/icons/track.svg" alt="" /></div><h4>Real-Time Tracking</h4><p>Follow every milestone and stay informed from pickup to final delivery.</p></div>
                <div class="speed-card"><div class="service-round"><img src="../Assets/icons/support.svg" alt="" /></div><h4>Dedicated Support</h4><p>Get practical assistance from a team that keeps your shipment moving.</p></div>
            </div>
        </div>
    </section>

    <section class="section section-alt about-reference">
        <div class="container two-col">
            <div class="about-photo about-carousel" id="defaultAboutCarousel">
                <img class="about-carousel-slide is-active" src="../Assets/about-1.png" alt="Warehouse package handling" />
                <img class="about-carousel-slide" src="../Assets/about-2.png" alt="Courier carrying parcel for delivery" />
                <img class="about-carousel-slide" src="../Assets/about-3.png" alt="Logistics dispatch center operations" />
                <div class="about-carousel-dots" aria-hidden="true">
                    <span class="about-carousel-dot is-active"></span>
                    <span class="about-carousel-dot"></span>
                    <span class="about-carousel-dot"></span>
                </div>
            </div>
            <div>
                <div class="reference-kicker">ABOUT US</div>
                <h2 class="reference-heading">Moving Packages. <span>Connecting People.</span></h2>
                <p class="section-lead">Soonest Global Express Corporation provides dependable logistics for local and international shipments, with secure handling, clear communication, and reliable delivery.</p>
                <ul class="feature-list">
                    <li>Secure package handling</li>
                    <li>Branches nationwide</li>
                    <li>Step-by-step tracking</li>
                    <li>Responsive support team</li>
                </ul>
                <a runat="server" href="~/Views/About.aspx" class="btn btn-primary">Learn More <img class="btn-arrow" src="../Assets/icons/arrow-right.svg" alt="" /></a>
            </div>
        </div>
    </section>

    <section class="stats-band"><div class="container"><div class="card-grid">
        <div class="stat-item"><div class="stat-number">20,000+</div><div class="stat-label">Delivered Packages</div></div>
        <div class="stat-item"><div class="stat-number">500K+</div><div class="stat-label">KM Per Year</div></div>
        <div class="stat-item"><div class="stat-number">3,200+</div><div class="stat-label">Projects Done</div></div>
        <div class="stat-item"><div class="stat-number">30</div><div class="stat-label">Branches Nationwide</div></div>
    </div></div></section>

    <section class="section">
        <div class="container">
            <div class="reference-kicker">HOW IT WORKS</div>
            <h2 class="reference-heading">Ship in <span>three simple steps.</span></h2>
            <div class="steps-grid">
                <div class="step-card"><h4>Create your account</h4><p>Sign up and verify your email to manage all your shipments in one place.</p></div>
                <div class="step-card"><h4>Request a shipment</h4><p>Enter the sender, recipient, and package details, and we take it from there.</p></div>
                <div class="step-card"><h4>Track every milestone</h4><p>Follow your parcel from pickup to delivery with real-time status updates.</p></div>
            </div>
        </div>
    </section>

    <section class="cta-band"><div class="container">
        <div>
            <h2>Let&rsquo;s move what matters to you.</h2>
            <p>Create a customer account to manage and track every shipment in one place.</p>
        </div>
        <div class="hero-actions">
            <a runat="server" href="~/Views/Account/Register.aspx" class="btn btn-light btn-lg">Get Started</a>
            <a runat="server" href="~/Views/Track.aspx" class="btn btn-ghost btn-lg">Track a Shipment</a>
        </div>
    </div></section>

<script type="text/javascript">
    (function () {
        var carousel = document.getElementById('defaultAboutCarousel');
        if (!carousel) return;

        var slides = carousel.querySelectorAll('.about-carousel-slide');
        var dots = carousel.querySelectorAll('.about-carousel-dot');
        if (!slides.length) return;

        var index = 0;
        function showSlide(nextIndex) {
            for (var i = 0; i < slides.length; i++) {
                var active = i === nextIndex;
                slides[i].classList.toggle('is-active', active);
                if (dots[i]) dots[i].classList.toggle('is-active', active);
            }
            index = nextIndex;
        }

        setInterval(function () {
            showSlide((index + 1) % slides.length);
        }, 3000);
    })();
</script>
</asp:Content>
