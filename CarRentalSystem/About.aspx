<%@ Page Title="About" Language="C#" MasterPageFile="~/SitePublic.Master" AutoEventWireup="true" CodeBehind="About.aspx.cs" Inherits="CarRentalSystem.About" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder_header" runat="server">
   </asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder_body" runat="server">
       <div class="container mt-5">

            <div class="card shadow-sm rounded mb-4">
                <div class="card-body text-center">
                    <h2 class="text-primary fw-bold">Cotabato Car Rentals</h2>
                    <p class="fs-5 fst-italic">"Your journey, our wheels!"</p>
                </div>
            </div>

            <div class="row">
                <div class="col-md-6 mb-4">
                    <div class="card h-100">
                        <div class="card-header bg-primary text-white">
                            <h5 class="mb-0">Our Vision</h5>
                        </div>
                        <div class="card-body">
                            <p>To become the most trusted and innovative car rental provider, delivering convenient, affordable, and reliable mobility solutions to every traveler.</p>
                        </div>
                    </div>
                </div>

                <div class="col-md-6 mb-4">
                    <div class="card h-100">
                        <div class="card-header bg-primary text-white">
                            <h5 class="mb-0">Contact Information</h5>
                        </div>
                        <div class="card-body">
                            <p><strong>Address:</strong> 123 Highway Road, Cotabato City, Philippines</p>
                            <p><strong>Phone:</strong> +63 912 345 6789</p>
                            <p><strong>Email:</strong> arshad03@gmail.com</p>
                        </div>
                    </div>
                </div>
            </div>

            <div class="card">
                <div class="card-header bg-primary text-white">
                    <h5 class="mb-0">Our Location</h5>
                </div>
                <div class="card-body p-0">
                    <iframe 
                         src="https://www.google.com/maps?q=7.216214170869474,124.2453633909754&hl=es;z=14&output=embed"
                        width="100%" 
                        height="450" 
                        style="border:0;" 
                        allowfullscreen="true" 
                        loading="lazy">
                    </iframe>
                </div>
            </div>

        </div>
 
</asp:Content>
