<%@ Page Language="C#" MasterPageFile="Site.Master" AutoEventWireup="true" CodeBehind="About.aspx.cs" Inherits="SoonestShipmentTrackingWebsite.Views.About" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">About Us</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <section class="hero hero-compact">
        <div class="container">
            <div>
                <div class="eyebrow"><span class="eyebrow-bar"></span>ABOUT SOONEST GLOBAL EXPRESS</div>
                <h1>Moving Packages. <span>Connecting People.</span></h1>
                <div class="crumbs"><a runat="server" href="~/Views/Default.aspx">Home</a><span>/</span><span>About</span></div>
            </div>
        </div>
    </section>

    <section class="section">
        <div class="container two-col">
            <div class="story-photo">
                <div class="about-photo about-carousel" id="aboutCarousel">
                    <img class="about-carousel-slide is-active" src="../Assets/about-1.png" alt="Warehouse package handling" />
                    <img class="about-carousel-slide" src="../Assets/about-2.png" alt="Courier carrying parcel for delivery" />
                    <img class="about-carousel-slide" src="../Assets/about-3.png" alt="Logistics dispatch center operations" />
                    <div class="about-carousel-dots" aria-hidden="true">
                        <span class="about-carousel-dot is-active"></span>
                        <span class="about-carousel-dot"></span>
                        <span class="about-carousel-dot"></span>
                    </div>
                </div>
                <div class="story-badge"><b>25</b><span>Years of trust</span></div>
            </div>
            <div>
                <div class="kicker"><span class="kicker-bar"></span>OUR STORY</div>
                <h2 class="section-heading">Built on trust, <span>delivered on time.</span></h2>
                <p class="section-lead">Soonest Global Express Corporation provides dependable logistics solutions for local and international shipments. What started as a small freight-forwarding team in Las Pi&ntilde;as has grown into a nationwide network trusted by thousands of families and businesses.</p>
                <p class="section-lead">Our goal is simple: make shipping easier through secure handling, clear communication, and reliable delivery &mdash; every time, no exceptions.</p>
                <ul class="feature-list">
                    <li>Secure package handling</li>
                    <li>Clear communication</li>
                    <li>Reliable delivery</li>
                    <li>Nationwide branch network</li>
                </ul>
            </div>
        </div>
    </section>

    <section class="stats-band"><div class="container"><div class="card-grid">
        <div class="stat-item"><div class="stat-number">20,000+</div><div class="stat-label">Delivered Packages</div></div>
        <div class="stat-item"><div class="stat-number">500K+</div><div class="stat-label">KM Per Year</div></div>
        <div class="stat-item"><div class="stat-number">3,200+</div><div class="stat-label">Projects Done</div></div>
        <div class="stat-item"><div class="stat-number">30</div><div class="stat-label">Branches Nationwide</div></div>
    </div></div></section>

    <section class="section section-alt">
        <div class="container">
            <div class="mv-grid">
                <div class="mv-card">
                    <div class="icon-round"><svg viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="1.8" stroke-linecap="round" stroke-linejoin="round" aria-hidden="true"><path d="M22 12h-6l-2 3h-4l-2-3H2" /><path d="M5.5 5h13L22 12v6a2 2 0 01-2 2H4a2 2 0 01-2-2v-6L5.5 5z" /></svg></div>
                    <div><h4>Our Mission</h4><p>To connect people and businesses across the Philippines and the world through fast, secure, and dependable logistics &mdash; treating every package as if it were our own.</p></div>
                </div>
                <div class="mv-card">
                    <div class="icon-round"><svg viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="1.8" stroke-linecap="round" stroke-linejoin="round" aria-hidden="true"><circle cx="12" cy="12" r="9" /><path d="M12 7v5l3.5 3.5" /></svg></div>
                    <div><h4>Our Vision</h4><p>To be the most trusted logistics partner in Southeast Asia, known for our reliability, transparency, and the care we bring to every delivery.</p></div>
                </div>
            </div>
        </div>
    </section>

    <section class="section">
        <div class="container">
            <div class="kicker"><span class="kicker-bar"></span>WHY CHOOSE US</div>
            <h2 class="section-heading">What sets us <span>apart.</span></h2>
            <div class="why-grid">
                <div class="why-item"><div class="icon-round"><svg viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="1.8" stroke-linecap="round" stroke-linejoin="round" aria-hidden="true"><path d="M12 3l7 3v6c0 4.5-3 7-7 9-4-2-7-4.5-7-9V6l7-3z" /></svg></div><div><h4>25 Years of Trust</h4><p>A quarter century of experience keeping shipments safe from pickup to doorstep.</p></div></div>
                <div class="why-item"><div class="icon-round"><svg viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="1.8" stroke-linecap="round" stroke-linejoin="round" aria-hidden="true"><circle cx="12" cy="12" r="9" /><ellipse cx="12" cy="12" rx="4" ry="9" /><path d="M3 12h18" /></svg></div><div><h4>30 Branches Nationwide</h4><p>A branch network across the Philippines means your shipment is never far from home.</p></div></div>
                <div class="why-item"><div class="icon-round"><svg viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="1.8" stroke-linecap="round" stroke-linejoin="round" aria-hidden="true"><circle cx="12" cy="12" r="9" /><path d="M12 7v5l3.5 3.5" /></svg></div><div><h4>Real-Time Tracking</h4><p>Every shipment is logged step-by-step, so you always know where it is.</p></div></div>
                <div class="why-item"><div class="icon-round"><svg viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="1.8" stroke-linecap="round" stroke-linejoin="round" aria-hidden="true"><path d="M4 13v-1a8 8 0 0116 0v1" /><rect x="2.5" y="13" width="4" height="6" rx="1.5" /><rect x="17.5" y="13" width="4" height="6" rx="1.5" /><path d="M20 19a4 4 0 01-4 3h-3" /></svg></div><div><h4>Dedicated Support</h4><p>A real team you can call, backed by fast responses when you need help.</p></div></div>
            </div>
        </div>
    </section>

    <section class="cta-band"><div class="container">
        <div>
            <h2>Ready to ship with us?</h2>
            <p>Create a free account and start tracking your shipments today.</p>
        </div>
        <div class="hero-actions">
            <a runat="server" href="~/Views/Account/Register.aspx" class="btn btn-light btn-lg">Create Account</a>
            <a runat="server" href="~/Views/Contact.aspx" class="btn btn-ghost btn-lg">Contact Us</a>
        </div>
    </div></section>

<script type="text/javascript">
    (function () {
        var carousel = document.getElementById('aboutCarousel');
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
