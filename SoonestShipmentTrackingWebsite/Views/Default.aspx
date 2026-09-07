<%@ Page Language="C#" MasterPageFile="Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="SoonestShipmentTrackingWebsite.Views.Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">Home</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <!-- Hero -->
    <div class="hero">
        <div class="container">
            <div class="eyebrow">WE FIND SOLUTIONS..... TODAY!</div>
            <h1>Fast, Reliable Logistics Across Land, Air, and Sea.</h1>
            <p>
                Soonest Global Express Corporation delivers cargo and personal shipments
                nationwide and overseas, with a personalized, one-stop-shop service from
                pickup to final delivery.
            </p>

            <div class="track-box">
                <asp:TextBox ID="txtControlNumber" runat="server" placeholder="Enter your Control No. to track a shipment" />
                <asp:Button ID="btnTrack" runat="server" CssClass="btn btn-red" Text="Track" OnClick="btnTrack_Click" />
            </div>
        </div>
    </div>

    <!-- Info strip -->
    <div class="container">
        <div class="info-strip">
            <div class="info-item">
                <div class="info-label">Call Center</div>
                <div class="info-value">+632 249 8970</div>
            </div>
            <div class="info-item">
                <div class="info-label">Working Hours</div>
                <div class="info-value">Mon - Sat, 09:00 - 19:00</div>
            </div>
            <div class="info-item">
                <div class="info-label">Our Location</div>
                <div class="info-value">Jerusalem St., BF Martinville, Las Piñas City</div>
            </div>
            <div class="info-item">
                <div class="info-label">Branches</div>
                <div class="info-value">30 Nationwide Branches</div>
            </div>
        </div>
    </div>

    <!-- Welcome / About -->
    <div class="section">
        <div class="container">
            <div class="two-col">
                <div>
                    <div class="section-title">Welcome to Soonest Global Express</div>
                    <div class="section-title-bar"></div>
                    <p class="section-lead">
                        A Filipino-owned company engaged in complete logistics solutions, both
                        international and domestic, by sea, air, and land. With 25 years in the
                        industry and a team of over 125 experienced professionals, we deliver
                        swift, efficient, and innovative freight forwarding.
                    </p>
                    <p class="section-lead">
                        We are a Securities and Exchange Commission registered corporation,
                        accredited and licensed to handle both commercial cargo and personal
                        effect (balikbayan) shipments for local and international clients.
                    </p>
                    <p class="section-lead">
                        Our branch network spans key cities across the Philippines, supported by
                        partners across all seven continents — because international trade only
                        works with strong global relationships.
                    </p>
                    <a runat="server" href="~/Account/Register.aspx" class="btn btn-navy">Create a Customer Account</a>
                </div>
                <div>
                    <div class="card-panel text-center">
                        <div style="font-size:64px;">🚚</div>
                        <h4 style="color:#0b2540;margin:10px 0 4px;">Door-to-Door Delivery</h4>
                        <p style="color:#6b7684;font-size:13.5px;">Serving Banking, Electronics, Power Plant, Fashion, Entertainment, and Construction industries nationwide.</p>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <!-- Delivery speed cards -->
    <div class="section section-alt">
        <div class="container">
            <div class="section-title">Quality and On-Time Delivery</div>
            <div class="section-title-bar"></div>
            <p class="section-lead">Guaranteed space availability and reliable transit times across every region we serve.</p>

            <div class="card-grid">
                <div class="speed-card">
                    <div class="speed-icon">1D</div>
                    <h4>1 Day Delivery</h4>
                    <p>Within Metro Manila and nearby areas</p>
                </div>
                <div class="speed-card">
                    <div class="speed-icon">2D</div>
                    <h4>1–2 Days</h4>
                    <p>Luzon Area</p>
                </div>
                <div class="speed-card">
                    <div class="speed-icon">3D</div>
                    <h4>2–3 Days</h4>
                    <p>Visayas Area</p>
                </div>
                <div class="speed-card">
                    <div class="speed-icon">3D</div>
                    <h4>2–3 Days</h4>
                    <p>Mindanao Area</p>
                </div>
            </div>
        </div>
    </div>

    <!-- Stats band -->
    <div class="stats-band">
        <div class="container">
            <div class="card-grid">
                <div class="stat-item">
                    <div class="stat-number">20,000+</div>
                    <div class="stat-label">Delivered Packages</div>
                </div>
                <div class="stat-item">
                    <div class="stat-number">500K+</div>
                    <div class="stat-label">KM Per Year</div>
                </div>
                <div class="stat-item">
                    <div class="stat-number">3,200+</div>
                    <div class="stat-label">Projects Done</div>
                </div>
                <div class="stat-item">
                    <div class="stat-number">30</div>
                    <div class="stat-label">Branches</div>
                </div>
            </div>
        </div>
    </div>

    <!-- CTA -->
    <div class="section text-center">
        <div class="container">
            <div class="section-title" style="justify-content:center;">Ready to Ship With Us?</div>
            <p class="section-lead" style="margin:0 auto 22px;">Register a free customer account to manage and track every shipment tied to you in one place.</p>
            <a runat="server" href="~/Account/Register.aspx" class="btn btn-red">Get Started</a>
            <a runat="server" href="~/Track.aspx" class="btn btn-outline" style="border-color:#0b2540;color:#0b2540;">Track a Shipment</a>
        </div>
    </div>

</asp:Content>

