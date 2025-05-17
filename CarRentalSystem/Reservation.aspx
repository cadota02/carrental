<%@ Page Title="Reservation" Language="C#" MasterPageFile="~/SitePublic.Master" AutoEventWireup="true" CodeBehind="Reservation.aspx.cs" Inherits="CarRentalSystem.Reservation" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder_header" runat="server">
    <script src="Shared/calendar/index.global.js"></script>
    <script src="Shared/calendar/index.global.min.js"></script>
    <style>
           .highlight {
      border: 6px solid #007bff !important;
        box-shadow: 0 0 12px rgba(0, 123, 255, 0.5);
    }
        #calendar {
  min-height: 500px;
  width: 100%;
}
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
            <div class="col-md-12">
                <div class="card card-info shadow">
                    <div class="card-header">
                        <h5 class="card-title">Car Reservation</h5>
                        <div class="card-tools">
                          <div class="input-group input-group-sm" style="width: 300px;">
                               <asp:TextBox ID="txtsearch" CssClass="form-control float-right" PlaceHolder="Search Keyword" runat="server"></asp:TextBox>
                           <div class="input-group-append">
                               <asp:LinkButton ID="btnsearch" CssClass="btn btn-primary" runat="server"  OnClick="btnsearch_Click"><i class="fas fa-search"></i></asp:LinkButton>
                               <asp:HyperLink ID="HyperLink1"  CssClass="btn btn-info pt-1" runat="server" NavigateUrl="~/Reservation"><i class="fas fa-sync-alt"></i></asp:HyperLink>
                       
                           
                           </div>
                       </div>
                        
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
                                <div class="col-md-4 mb-2 d-flex align-items-stretch" id='car_<%# Eval("car_id") %>'>
                                    <div class="card shadow-sm">
                                        <img src='<%# Eval("image_path").ToString().Substring(2) %>' class="card-img-top" alt="Car Image">
                                        <div class="card-body">
                                           <%-- <span class="badge bg-info mb-2">test</span>--%>
                                            <div class="row"><h5 class="card-title font-weight-bold"><%# Eval("car_name") %>
                                                <br /> Daily Rate: ₱<%# String.Format("{0:N2}", Eval("RatePerDay")) %>
                                            </h5>
                                          </div>
                                              <div class="accordion" id='accordion<%# Eval("car_id") %>'>
                                            <div class="mb-3">
                                                <div class="p-1" id='heading<%# Eval("car_id") %>'>
                                                    <h2 class="mb-0">
                                                        <button class="btn btn-link btn-sm text-left p-0" type="button"
                                                                data-toggle="collapse"
                                                                data-target='#collapse<%# Eval("car_id") %>'
                                                                aria-expanded="false"
                                                                aria-controls='collapse<%# Eval("car_id") %>'>
                                                            Read Description
                                                        </button>
                                                    </h2>
                                                </div>

                                                <div id='collapse<%# Eval("car_id") %>' class="collapse"
                                                     aria-labelledby='heading<%# Eval("car_id") %>'
                                                     data-parent='#accordion<%# Eval("car_id") %>'>
         
                                                        <span class="text-sm" style="font-weight:lighter"><%# Eval("description") %></span>
           
                                                </div>
                                            </div>
                                        </div>
                                            <asp:HiddenField ID="hdcarname" Value='<%# Eval("car_name") %>' runat="server" />
                                                   <asp:HiddenField ID="hd_id" Value='<%# Eval("car_id") %>' runat="server" />
                                            <asp:Button ID="btnSelect" runat="server" CssClass="btn btn-outline-primary btn-sm" Text="Select Car" CommandArgument='<%# Eval("car_id") %>' OnCommand="btnSelect_Command" />
                                             
                                            <asp:LinkButton 
                                                    ID="LinkButton1" 
                                                    runat="server" 
                                                    CssClass="btn btn-info btn-sm" 
                                                    OnClientClick='<%# "showAvailabilityModal(" + Eval("car_id") + "); return false;" %>' 
                                                    Text="Check Availability" />
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
                                 <div class="col-sm-3">
                                    <label class="form-label">Booking Number</label>
                                    <asp:TextBox ID="txtBookingNo" runat="server" CssClass="form-control" ReadOnly="true" />
                                </div>
                                <div class="col-sm-3">
                                    <label class="form-label">Booking Date</label>
                                    <asp:TextBox ID="txtBookingDate" runat="server" TextMode="Date" CssClass="form-control" />
                                    <asp:RequiredFieldValidator ID="rfvBookingDate" runat="server" ControlToValidate="txtBookingDate" 
                                        ErrorMessage="Booking date is required" CssClass="text-danger" Display="Dynamic"  ValidationGroup="add" />
                                </div>

                           <div class="col-sm-3">
                                    <label class="form-label">Return Date</label>
                                    <asp:TextBox ID="txtReturnDate" runat="server" TextMode="Date" CssClass="form-control" />
                                    <asp:RequiredFieldValidator ID="rfvReturnDate" runat="server" ControlToValidate="txtReturnDate" 
                                        ErrorMessage="Return date is required" CssClass="text-danger" Display="Dynamic"  ValidationGroup="add" />
                                </div>
                                <div class="col-sm-3">
                                    <label class="form-label">Chosen Car</label>
                                    <asp:TextBox ID="txtCar" runat="server" ReadOnly="true" CssClass="form-control" PlaceHolder="Select a Car"  />
                                </div>
                           </div>
                           <div class="form-group row">
                                 <div class="col-sm-3">
                                    <label class="form-label">Firstname</label>
                                    <asp:TextBox ID="txt_firstname" runat="server" PlaceHolder="Your Firstname" CssClass="form-control" />
                                    <asp:RequiredFieldValidator ID="rfvClientName" runat="server" ControlToValidate="txt_firstname" 
                                        ErrorMessage="Firstname is required" CssClass="text-danger" Display="Dynamic"  ValidationGroup="add" />
                                </div>
                                 <div class="col-sm-3">
                                    <label class="form-label">Lastname</label>
                                    <asp:TextBox ID="txt_lastname" runat="server" PlaceHolder="Your Lastname" CssClass="form-control" />
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txt_lastname" 
                                        ErrorMessage="Lastname is required" CssClass="text-danger" Display="Dynamic"  ValidationGroup="add" />
                                </div>
                                 <div class="col-sm-3">
                                    <label class="form-label">Middlename</label>
                                    <asp:TextBox ID="txt_mi" runat="server" PlaceHolder="Your Middle Name" CssClass="form-control" />
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txt_mi" 
                                        ErrorMessage="Middlename is required" CssClass="text-danger" Display="Dynamic"  ValidationGroup="add" />
                                </div>
                                 <div class="col-sm-3">
                                    <label class="form-label">Contact Number</label>
                                    <asp:TextBox ID="txtClientContact" runat="server" PlaceHolder="Contact Number" CssClass="form-control" />
                                    <asp:RequiredFieldValidator ID="rfvClientContact" runat="server" ControlToValidate="txtClientContact" 
                                        ErrorMessage="Contact number is required" CssClass="text-danger" Display="Dynamic"  ValidationGroup="add" />
                                </div>
                             
                               
                            
                               </div>
                          <div class="form-group row">
                               <div class="col-sm-4">
                                    <label class="form-label">Address</label>
                                    <asp:TextBox ID="txtClientAddress" runat="server" PlaceHolder="Current Address" CssClass="form-control" />
                                    <asp:RequiredFieldValidator ID="rfvClientAddress" runat="server" ControlToValidate="txtClientAddress" 
                                        ErrorMessage="Address is required" CssClass="text-danger" Display="Dynamic"  ValidationGroup="add" />
                                </div>
                                     <div class="col-sm-4">
                                    <label class="form-label">Email Address</label>
                                    <asp:TextBox ID="txtClientEmail" runat="server" PlaceHolder="Email" CssClass="form-control" TextMode="Email" />
                                    <asp:RequiredFieldValidator ID="rfvClientEmail" runat="server" ControlToValidate="txtClientEmail" 
                                        ErrorMessage="Email is required" CssClass="text-danger" Display="Dynamic"  ValidationGroup="add" />
                                </div>

                                    <div class="col-sm-4">
                                    <label class="form-label">Message</label>
                                    <asp:TextBox ID="txtClientRemarks" runat="server" CssClass="form-control" PlaceHolder="Put your instruction here"  />
                                </div>
                                 
                              </div>
                       <div class="container text-center">
                 <hr />
            <h6>Register a new account or fill in your existing account details:</h6>
                   <hr />
                        </div>
                       <div class="form-group row">
                               <div class="col-sm-4">
                                    <label class="form-label">Username</label>
                                    <asp:TextBox ID="txt_username" runat="server" PlaceHolder="Username" CssClass="form-control" />
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txt_username" 
                                        ErrorMessage="Username is required" CssClass="text-danger" Display="Dynamic"  ValidationGroup="add" />
                                </div>
                                     <div class="col-sm-4">
                                    <label class="form-label">Password</label>
                                    <asp:TextBox ID="txt_password" runat="server" PlaceHolder="Password" CssClass="form-control" TextMode="Password" />
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txt_password" 
                                        ErrorMessage="Password is required" CssClass="text-danger" Display="Dynamic"  ValidationGroup="add" />
                                </div>

                                    <div class="col-sm-4">
                                    <label class="form-label">Confirm Password</label>
                                    <asp:TextBox ID="txt_confirmpassword" runat="server" CssClass="form-control" PlaceHolder="Confirm password"  TextMode="Password"  />
                                  <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="txt_confirmpassword" 
                                        ErrorMessage="Confirm password is required" CssClass="text-danger" Display="Dynamic"  TextMode="Password" ValidationGroup="add" />
                                   <asp:CompareValidator ID="CompareValidator1" runat="server"
                                    ControlToCompare="txt_password"
                                    ControlToValidate="txt_confirmpassword"
                                    Operator="Equal"
                                    Type="String"
                                    ErrorMessage="Confirm password do not match."
                                    CssClass="text-danger"
                                    ValidationGroup="add"
                                    Display="Dynamic" />
                                        </div>
                                 
                              </div>
                     
                              
                
                   
                    </div>
                     <div class="card-footer">
                         <small>Once your account is registered, you’ll gain access after it’s approved by the administrator.</small>
                           <asp:HyperLink ID="linkprint" class="btn btn-default float-right" runat="server"   Text="Print Reservation" Target="_blank" ></asp:HyperLink>
                           <asp:Button ID="btnReserve" runat="server" Text="Submit Reservation" CssClass="btn btn-primary float-right" OnClick="btnReserve_Click" ValidationGroup="add" />

                         </div>
                </div>
            </div>
        </div>
    </div>

        <!-- Modal Container -->
<div id="availabilityModal" class="modal fade"  tabindex="-1" role="dialog">
     <div class="modal-dialog modal-lg" role="document">
   <div class="modal-content">
                         <div class="modal-header">
                          
                                            <h4 class="modal-title">
                                              
                                       <asp:Label ID="lbl_header" runat="server" Text="Availability of Car"></asp:Label>
                                      
                                            </h4>
                                               <button type="button" data-dismiss="modal" aria-label="Close" class="close"><span aria-hidden="true">×</span></button>
                                        </div>

        
                          <div class="modal-body">
       
        <asp:HiddenField ID="hd_carid" Value="0" runat="server" />
                         <div id="calendar" style="min-height: 500px;"></div>
                              </div>
                        <div class="modal-footer">
                    
                          <asp:Button ID="btnClose" runat="server" OnClick="btnClose_Click"  Text="Close" CssClass="btn btn-secondary" />
                         
                    </div>
                  
   
    </div>
   </div>
    </div>
     <script>
         var calendar;

         function showAvailabilityModal(carid) {
             var calendarEl = document.getElementById('calendar');

             // Destroy old calendar if exists
             if (calendar) {
                 calendar.destroy();
             }

             // Create a new calendar instance
             calendar = new FullCalendar.Calendar(calendarEl, {
                 initialDate: new Date().toISOString().split('T')[0],
                 initialView: 'dayGridMonth',
                 nowIndicator: true,
                 headerToolbar: {
                     left: 'prev,next today',
                     center: 'title',
                     right: 'dayGridMonth,timeGridWeek,timeGridDay,listWeek'
                 },
                 events: 'shared/AvailiabityCarsHandler.ashx?carid=' + carid,
                 eventDidMount: function (info) {
                     info.el.setAttribute('title', info.event.extendedProps.tooltip);
                     if (info.view.type.startsWith("list")) {
                         var dotEl = info.el.querySelector('.fc-list-event-dot');
                         if (dotEl) {
                             dotEl.style.borderColor = info.event.backgroundColor;
                         }
                     }
                 }
             });

             // Show the modal using jQuery (AdminLTE 3 style)
             $('#availabilityModal').modal('show');
         }

         // Bind calendar render to Bootstrap 4 modal event (jQuery version)
         $(document).ready(function () {
             $('#availabilityModal').on('shown.bs.modal', function () {
               
                 if (calendar) {
                     calendar.render();
                 }
             });
         });
</script>
    <script>
        $(document).ready(function () {
            var carId = $('#<%= hd_carid.ClientID %>').val();
            if (carId !== "0") {
                console.log("Selected Car ID: " + carId);

                // ✅ Highlight the corresponding card if needed
                $('#car_' + carId).addClass('border border-primary shadow-lg');

                // or perform any action here
            }
        });
</script>
</asp:Content>
