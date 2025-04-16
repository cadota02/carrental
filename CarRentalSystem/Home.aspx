<%@ Page Title="Home" Language="C#" MasterPageFile="~/SitePublic.Master" AutoEventWireup="true" CodeBehind="Home.aspx.cs" Inherits="CarRentalSystem.Home" %>
<asp:Content ID="Content3" runat="server" contentplaceholderid="ContentPlaceHolder_header">
     
     <style>
         .banner {
                position: relative;
                background: url('uploads/banner/banner1.png') no-repeat center center;
                background-size: cover;
                min-height: 400px;
                padding: 120px 20px;
                color: white;
                text-align: center;
                display: flex;
                flex-direction: column;
                justify-content: center;
                align-items: center;
            }

    .banner::before {
        content: '';
        position: absolute;
        top: 0; left: 0;
        width: 100%;
        height: 100%;
        background-color: rgba(0, 0, 0, 0.5); /* semi-transparent black */
        z-index: 1;
    }

    .banner h1, .banner p, .banner a {
        z-index: 2;
        position: relative;
    }

    .banner h1 {
        color: #fff;
        text-shadow: 2px 2px 6px rgba(0,0,0,0.8);
    }

    .banner p {
        color: #eaeaea;
    }
    </style>
   
      </asp:Content>
<asp:Content ID="Content4" runat="server" contentplaceholderid="ContentPlaceHolder_body">
       <div class="banner d-flex flex-column justify-content-center align-items-center text-center">
    <h1 class="display-4 fw-bold">Reliable & Affordable Car Rentals in Cotabato City</h1>
    <p class="lead mb-4">Explore the region with confidence and comfort.</p>
    <div>
        <a href="Reservation.aspx" class="btn btn-info btn-lg me-2 mb-2">Reserve Now</a>
        <a href="Contact.aspx" class="btn btn-outline-light btn-lg mb-2">Contact Now</a>
    </div>
</div>

        <div class="container mt-5">
            <h2 class="text-center mb-4">Most Popular Services</h2>
            <div class="row g-4">
                <div class="col-md-4">
                    <div class="card service-card shadow-sm">
                        <img src="uploads/banner/davao1.jfif" class="card-img-top" alt="Ride to Davao">
                        <div class="card-body">
                            <h5 class="card-title">Ride to Davao</h5>
                            <p class="card-text">Enjoy a smooth and scenic drive to Davao City with our well-maintained vehicles and professional drivers.</p>
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="card service-card shadow-sm">
                        <img src="uploads/banner/gensan.jfif" class="card-img-top" alt="Ride to General Santos">
                        <div class="card-body">
                            <h5 class="card-title">Ride to General Santos</h5>
                            <p class="card-text">Travel comfortably to Gensan for business or leisure with our reliable car rental services.</p>
                        </div>
                    </div>
                </div>
                <div class="col-md-4">
                    <div class="card service-card shadow-sm">
                        <img src="uploads/banner/cagayan1.jfif" class="card-img-top" alt="Ride to Cagayan de Oro">
                        <div class="card-body">
                            <h5 class="card-title">Ride to Cagayan de Oro</h5>
                            <p class="card-text">Long-distance trips made easy — book a ride to CDO with affordable rates and exceptional service.</p>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    <!-- /.content -->
      </asp:Content>

