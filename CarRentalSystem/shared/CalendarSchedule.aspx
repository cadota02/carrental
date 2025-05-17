<%@ Page Title="Calendar"  Language="C#" AutoEventWireup="true" CodeBehind="CalendarSchedule.aspx.cs"  Inherits="CarRentalSystem.shared.CalendarSchedule" %>
<%@ MasterType VirtualPath="~/Site.master" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder_header" runat="server">
    <!-- FullCalendar JS -->
<script src="shared/calendar/index.global.js"></script>
    <script src="shared/calendar/index.global.min.js"></script>
     <div class="content-header">
        <div class="container">
            <div class="row">
                <div class="col-sm-6">
                
                </div>
                <!-- /.col -->
                <div class="col-sm-6">
                
                    <ol class="breadcrumb float-sm-right">
                 
                     <%--   <li class="breadcrumb-item"><a href="Home">Home</a></li>
                    
                        <li class="breadcrumb-item active">Login</li>--%>
                    </ol>
                </div>
                <!-- /.col -->
            </div>
            <!-- /.row -->
        </div>
        <!-- /.container-fluid -->
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder_body" runat="server">


 <section class="content">
     <div class="container">
      <div class="row">
        <div class="col-md-12">
          <div class="card card-info">
            <div class="card-header">
              <h3 class="card-title">Car Schedule</h3>

              <div class="card-tools">
   
              
              </div>
            </div>
            <div class="card-body">
                <div class="form-group row">
                    <div class="col-sm-2">
                        <label>Select Car:</label> </div>  
                    <div class="col-sm-4">
                    <asp:DropDownList ID="ddlCars" runat="server" CssClass="form-control" AutoPostBack="false" runat="server"></asp:DropDownList>
                        </div>
                </div>
                <div id='calendar'></div>
                </div>
   </div>
          
            <%-- <script>
                 document.addEventListener('DOMContentLoaded', function () {
                     var calendarEl = document.getElementById('calendar');

                     var calendar = new FullCalendar.Calendar(calendarEl, {
                         initialDate: new Date().toISOString().split('T')[0],
                         initialView: 'dayGridMonth',
                         nowIndicator: true,
                         headerToolbar: {
                             left: 'prev,next today',
                             center: 'title',
                             right: 'dayGridMonth,timeGridWeek,timeGridDay,listWeek'
                         },
                         events: 'shared/CarSchedule.ashx', // your .ashx file
                         eventDidMount: function (info) {
                             info.el.setAttribute('title', info.event.extendedProps.tooltip);
                             // Set color of dot in list view manually
                             if (info.view.type.startsWith("list")) {
                                 var dotEl = info.el.querySelector('.fc-list-event-dot');
                                 if (dotEl) {
                                     dotEl.style.borderColor = info.event.backgroundColor;
                                 }
                             }
                         }
                     });

                     calendar.render();
                 });
        </script>
  --%>
       </div>
     
    </div>
    </div>
 </section>
    <script>
        var calendar;

        document.addEventListener('DOMContentLoaded', function () {
            var calendarEl = document.getElementById('calendar');
            var ddlCars = document.getElementById('<%= ddlCars.ClientID %>');

        // Initialize FullCalendar
        calendar = new FullCalendar.Calendar(calendarEl, {
            initialDate: new Date().toISOString().split('T')[0],
            initialView: 'dayGridMonth',
            nowIndicator: true,
            headerToolbar: {
                left: 'prev,next today',
                center: 'title',
                right: 'dayGridMonth,timeGridWeek,timeGridDay,listWeek'
            },
            events: function (info, successCallback, failureCallback) {
                let carid = ddlCars.value;
                fetch('shared/CarSchedule.ashx?carid=' + carid)
                    .then(response => response.json())
                    .then(events => successCallback(events))
                    .catch(error => failureCallback(error));
            },
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

        calendar.render();

        // Reload events when dropdown changes
        ddlCars.addEventListener('change', function () {
            calendar.refetchEvents();
        });
    });
</script>
</asp:Content>
