<%@ Page Title="Reservation" Language="C#" MasterPageFile="~/SitePublic.Master" AutoEventWireup="true" CodeBehind="Reservation.aspx.cs" Inherits="CarRentalSystem.Reservation" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder_header" runat="server">
    <style>
    .scroll-box {
        position: relative;
    }

    .scroll-btn {
        position: absolute;
        top: 35%;
        z-index: 10;
        background-color: rgba(255, 255, 255, 0.8);
        border: none;
        border-radius: 50%;
        width: 40px;
        height: 40px;
        box-shadow: 0 2px 5px rgba(0, 0, 0, 0.2);
        cursor: pointer;
    }

    .scroll-btn:hover {
        background-color: #ddd;
    }

    .scroll-left {
        left: 0;
        transform: translateY(-50%);
    }

    .scroll-right {
        right: 0;
        transform: translateY(-50%);
    }

    .scroll-container {
        display: flex;
        overflow-x: auto;
        gap: 1rem;
        padding-bottom: 1rem;
        scroll-behavior: smooth;
    }

    .scroll-container::-webkit-scrollbar {
        height: 8px;
    }

    .scroll-container::-webkit-scrollbar-thumb {
        background: #aaa;
        border-radius: 4px;
    }
</style>
    
    <script>
    function scrollCars(direction) {
        const container = document.getElementById('carScroll');
        const scrollAmount = 500; // adjust if needed
        container.scrollBy({
            left: direction * scrollAmount,
            behavior: 'smooth'
        });
    }
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder_body" runat="server">
     <div class="container my-5">
        <div class="row justify-content-center">
            <div class="col-md-10">
                <div class="card card-primary shadow">
                    <div class="card-header">
                        <h5 class="card-title">Car Reservation</h5>
                        <div class="card-tools">
                         
                            <asp:HyperLink ID="HyperLink1" CssClass="btn btn-tool" runat="server" NavigateUrl="~/Reservation"><i class="fas fa-sync-alt"></i></asp:HyperLink>
                        </div>
                    </div>
                    <div></div>
                    <div class="card-body">
                        <!-- Car Selection -->

                <asp:Button ID="btnPrev" runat="server" Text="&laquo;" CssClass="scroll-btn scroll-left text-lg" OnClick="btnPrev_Click" />
    <!-- Scrollable Car List -->
                        <div class="row">
                        <asp:Repeater ID="rptCars" runat="server">
                            <ItemTemplate>
                                <div class="col-md-4 mb-2 d-flex align-items-stretch">
                                    <div class="card shadow-sm">
                                        <img src='<%# Eval("image_path").ToString().Substring(2) %>' class="card-img-top" alt="Car Image">
                                        <div class="card-body">
                                           <%-- <span class="badge bg-primary mb-2">test</span>--%>
                                            <h5 class="card-title font-weight-bold"><%# Eval("car_name") %></h5>
                                            <p class="card-text text-sm"><%# Eval("description") %></p>
                                            <asp:HiddenField ID="hdcarname" Value='<%# Eval("car_name") %>' runat="server" />
                                            <asp:Button ID="btnSelect" runat="server" CssClass="btn btn-outline-primary mt-2" Text="Select Car" CommandArgument='<%# Eval("car_id") %>' OnCommand="btnSelect_Command" />
                                        </div>
                                    </div>
                                </div>
                            </ItemTemplate>
                        </asp:Repeater>
                            </div>
                        <%--    </div>--%>


    <!-- Right Button -->
                    
          <asp:Button ID="btnNext" runat="server" Text="&raquo;" CssClass="scroll-btn scroll-right text-lg" OnClick="btnNext_Click" />

            <!-- Booking Form -->
                     
                            <div class="form-group row">
                                 <div class="col-sm-4">
                                    <label class="form-label">Booking Number</label>
                                    <asp:TextBox ID="txtBookingNo" runat="server" CssClass="form-control" ReadOnly="true" />
                                </div>
                                <div class="col-sm-4">
                                    <label class="form-label">Booking Date</label>
                                    <asp:TextBox ID="txtBookingDate" runat="server" TextMode="Date" CssClass="form-control" />
                                    <asp:RequiredFieldValidator ID="rfvBookingDate" runat="server" ControlToValidate="txtBookingDate" 
                                        ErrorMessage="Booking date is required" CssClass="text-danger" Display="Dynamic"  ValidationGroup="add" />
                                </div>

                           <div class="col-sm-4">
                                    <label class="form-label">Return Date</label>
                                    <asp:TextBox ID="txtReturnDate" runat="server" TextMode="Date" CssClass="form-control" />
                                    <asp:RequiredFieldValidator ID="rfvReturnDate" runat="server" ControlToValidate="txtReturnDate" 
                                        ErrorMessage="Return date is required" CssClass="text-danger" Display="Dynamic"  ValidationGroup="add" />
                                </div>
                           </div>
                           <div class="form-group row">
                                 <div class="col-sm-4">
                                    <label class="form-label">Client Name</label>
                                    <asp:TextBox ID="txtClientName" runat="server" PlaceHolder="Your Fullname" CssClass="form-control" />
                                    <asp:RequiredFieldValidator ID="rfvClientName" runat="server" ControlToValidate="txtClientName" 
                                        ErrorMessage="Name is required" CssClass="text-danger" Display="Dynamic"  ValidationGroup="add" />
                                </div>

                              <div class="col-sm-4">
                                    <label class="form-label">Client Address</label>
                                    <asp:TextBox ID="txtClientAddress" runat="server" PlaceHolder="Current Address" CssClass="form-control" />
                                    <asp:RequiredFieldValidator ID="rfvClientAddress" runat="server" ControlToValidate="txtClientAddress" 
                                        ErrorMessage="Address is required" CssClass="text-danger" Display="Dynamic"  ValidationGroup="add" />
                                </div>
                               
                              <div class="col-sm-4">
                                    <label class="form-label">Contact Number</label>
                                    <asp:TextBox ID="txtClientContact" runat="server" PlaceHolder="Contact Number" CssClass="form-control" />
                                    <asp:RequiredFieldValidator ID="rfvClientContact" runat="server" ControlToValidate="txtClientContact" 
                                        ErrorMessage="Contact number is required" CssClass="text-danger" Display="Dynamic"  ValidationGroup="add" />
                                </div>
                               </div>
                          <div class="form-group row">
                                     <div class="col-sm-4">
                                    <label class="form-label">Email Address</label>
                                    <asp:TextBox ID="txtClientEmail" runat="server" PlaceHolder="Email" CssClass="form-control" TextMode="Email" />
                                    <asp:RequiredFieldValidator ID="rfvClientEmail" runat="server" ControlToValidate="txtClientEmail" 
                                        ErrorMessage="Email is required" CssClass="text-danger" Display="Dynamic"  ValidationGroup="add" />
                                </div>

                                    <div class="col-sm-4">
                                    <label class="form-label">Message</label>
                                    <asp:TextBox ID="txtClientRemarks" runat="server" CssClass="form-control" PlaceHolder="Put your instruction here" TextMode="MultiLine" Rows="3"  />
                                </div>
                                 <div class="col-sm-4">
                                    <label class="form-label">Chosen Car</label>
                                    <asp:TextBox ID="txtCar" runat="server" ReadOnly="true" CssClass="form-control" PlaceHolder="Select a Car"  />
                                </div>
                              </div>
                     
                              
                
                   
                    </div>
                     <div class="card-footer">
                           <asp:HyperLink ID="linkprint" class="btn btn-default" runat="server"   Text="Print Reservation" Target="_blank" ></asp:HyperLink>
                           <asp:Button ID="btnReserve" runat="server" Text="Submit Reservation" CssClass="btn btn-primary float-right" OnClick="btnReserve_Click" ValidationGroup="add" />

                         </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
