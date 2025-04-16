<%@ Page Title="Home" Language="C#" Async="true" MasterPageFile="~/SiteAdmin.Master" AutoEventWireup="true" CodeBehind="AdminHome.aspx.cs" Inherits="CarRentalSystem.Admin.AdminHome" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder_header" runat="server">
       <script type="text/javascript">
           function openModal() {
               $('#registerModal').modal('show');
           }
           function getConfirmation_verify() {
               return confirm("Are you sure you want to delete?");
           }
         
           function closeModal1() {
               $('#registerModal').modal('hide'); // For Bootstrap modals
           }

           function getConfirmation_activate(status) {
               var action = status === "Active" ? "deactivate" : "activate";
               return confirm("Are you sure you want to " + action + "?");
           }

     </script>
      <div class="content-header">
      <div class="container">
        <div class="row mb-2">
                <div class="col-sm-6">
                   
                </div>
                <!-- /.col -->
                <div class="col-sm-6">
                    <ol class="breadcrumb float-sm-right">
                        <li class="breadcrumb-item active"><a href="AdminHome">Home</a></li>
                    
                    </ol>
                </div>
                <!-- /.col -->
            </div>
            <!-- /.row -->
        </div>
        <!-- /.container-fluid -->
    </div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder_body" runat="server">


     <section class="content">
     <div class="container">
           <asp:UpdatePanel ID="UpdatePanel4" runat="server">
               <ContentTemplate>
           <div class="row">
                
          <div class="col-md-3 col-sm-6 col-12">
            <div class="info-box">
              <span class="info-box-icon bg-info"><i class="fa fa-list"></i></span>

              <div class="info-box-content">
                <span class="info-box-text">Total Reservation</span>
                <span class="info-box-number"><%= totalCount %></span>
              </div>
              <!-- /.info-box-content -->
            </div>
            <!-- /.info-box -->
          </div>
          <!-- /.col -->
          <div class="col-md-3 col-sm-6 col-12">
            <div class="info-box">
              <span class="info-box-icon bg-success"><i class="fa fa-thumbs-up"></i></span>

              <div class="info-box-content">
                <span class="info-box-text">Approved</span>
                <span class="info-box-number"><%= approvedCount %></span>
              </div>
              <!-- /.info-box-content -->
            </div>
            <!-- /.info-box -->
          </div>
          <!-- /.col -->
          <div class="col-md-3 col-sm-6 col-12">
            <div class="info-box">
              <span class="info-box-icon bg-warning"><i class="fa fa-hourglass-half"></i></span>

              <div class="info-box-content">
                <span class="info-box-text">Pending</span>
                <span class="info-box-number"><%= pendingCount %></span>
              </div>
              <!-- /.info-box-content -->
            </div>
            <!-- /.info-box -->
          </div>
          <!-- /.col -->
          <div class="col-md-3 col-sm-6 col-12">
            <div class="info-box">
              <span class="info-box-icon bg-danger"><i class="fa fa-thumbs-down"></i></span>

              <div class="info-box-content">
                <span class="info-box-text">Rejected</span>
                <span class="info-box-number"><%= rejectedCount %></span>
              </div>
              <!-- /.info-box-content -->
            </div>
            <!-- /.info-box -->
          </div>
          <!-- /.col -->
            
        </div>
               </ContentTemplate>
            </asp:UpdatePanel>
        <!-- /.row -->

      <div class="row">
        <div class="col-md-12">
          <div class="card card-primary">
            <div class="card-header">
              <h3 class="card-title">Manage Reservation</h3>

              <div class="card-tools">
        
              
              </div>
            </div>
            <div class="card-body">
                 <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                    <ContentTemplate>
                                             <div class="form-group row">
                               
                                                <div class="col-sm-4">
                                       
                                                 <asp:TextBox ID="txt_search" CssClass="form-control form-control" placeholder="Enter keyword" runat="server"></asp:TextBox>
                                                </div>
                                                 <div class="col-sm-2">

                                                      <asp:DropDownList ID="dpfilterstatus" runat="server" CssClass="form-control">
                                            <asp:ListItem Text="Pending" Value="Pending"></asp:ListItem>
                                            <asp:ListItem Text="Approved" Value="Approved"></asp:ListItem>
                                        <asp:ListItem Text="Rejected" Value="Rejected"></asp:ListItem>
                                        </asp:DropDownList>
                                                 </div>
                                              
                                                <div class="col-sm-6">
                                                 
                                                 <asp:LinkButton ID="btn_filter" CssClass=" btn btn-info" 
                                                         runat="server" onclick="btn_filter_Click" > Search</asp:LinkButton>
                                                          <asp:LinkButton ID="btn_reset" CssClass="btn btn-default" 
                                                         runat="server" onclick="btn_reset_Click">Refresh</asp:LinkButton>
                                       
                                                </div>
                                             
                                                  </div>
                       <div class="table-responsive">
                           <asp:GridView ID="gv_masterlist" runat="server" 
                              CssClass="table  table-bordered table-sm  table-hover" 
                              AutoGenerateColumns="false" AllowPaging="true"
                              OnPageIndexChanging="OnPaging" PageSize="10" 
                            GridLines="None" PagerSettings-Mode="NumericFirstLast"
                              HeaderStyle-HorizontalAlign="Center" HeaderStyle-VerticalAlign="Middle" Font-Size="Smaller">
                                  <Columns>
                           <asp:TemplateField>
                                        <HeaderTemplate> Reserve No </HeaderTemplate>
                                     <ItemTemplate>
                                         <asp:HyperLink ID="HyperLink1" runat="server"  Text='<%# Eval("bookingno") %>' Target="_blank" NavigateUrl='<%# "~/reports/booking/" + Eval("bookingno") + "_"+ Eval("booking_id") +".pdf" %> '></asp:HyperLink>

                                           </ItemTemplate>
                                   </asp:TemplateField>
                                     
                                               <asp:BoundField DataField="client_name" HeaderText="Fullname"  />
                                       <asp:BoundField DataField="client_contactno" HeaderText="Contact"   />
                                                <asp:BoundField DataField="client_address" HeaderText="Address"   />
                                      <asp:BoundField DataField="car_name" HeaderText="Vehicle"    ItemStyle-HorizontalAlign="Left" />
                                                 <asp:BoundField DataField="booking_date"   DataFormatString="{0:MMM d, yyyy}" HtmlEncode="False" HeaderText="Date Booking"    ItemStyle-HorizontalAlign="Left" />
                                           <asp:BoundField DataField="return_date"   DataFormatString="{0:MMM d, yyyy}" HtmlEncode="False" HeaderText="Date Return"    ItemStyle-HorizontalAlign="Left" />
                                       <asp:BoundField DataField="client_remarks" HeaderText="Message"    ItemStyle-HorizontalAlign="Left" />
                                       <asp:BoundField DataField="admin_remarks" HeaderText="Remarks"    ItemStyle-HorizontalAlign="Left" />
                                      
                                        <asp:BoundField DataField="status" HeaderText="Status" ItemStyle-HorizontalAlign="Center" />
                                       <asp:BoundField DataField="time_ago" HeaderText="Time Ago" ItemStyle-HorizontalAlign="Center" />
                                   <asp:TemplateField>
                                        <HeaderTemplate> Action </HeaderTemplate>
                                     <ItemTemplate>
                                      <asp:HiddenField ID="hd_id" Value='<%#Eval("booking_id") %>' runat="server"></asp:HiddenField>
                           <asp:HiddenField ID="hd_status" Value='<%#Eval("status") %>' runat="server"></asp:HiddenField>
                                      <asp:HiddenField ID="hd_name" Value='<%#Eval("client_name") %>' runat="server"></asp:HiddenField>
                                      <asp:LinkButton ID="btn_select" CssClass="btn btn-primary btn-xs "  CommandArgument='<%# Eval("booking_id") %>' onclick="btn_select_Click"  runat="server" >Manage</asp:LinkButton>
                                      <asp:LinkButton ID="btn_delete" CssClass="btn btn-danger btn-xs " onclick="btn_delete_Click" runat="server"
                                     OnClientClick="return getConfirmation_verifys(this, 'Please confirm','Are you sure you want to Delete?');"
                                       >Remove</asp:LinkButton>
                                     </ItemTemplate>
                                   </asp:TemplateField>
                                 </Columns>
                               </asp:GridView>
                                         <asp:Label ID="lbl_item" runat="server" Text="" CssClass="form-control-label"></asp:Label>
                             </div>
                    </ContentTemplate>
                    </asp:UpdatePanel>

            </div>

         </div>
       </div>
      </div>
    </div>
  </section>

<div id="registerModal" class="modal fade"  role="dialog">
     <div class="modal-dialog modal-lg">
   <div class="modal-content">
                        <div class="modal-header">
                          
                                            <h4 class="modal-title">
                                                 <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                                 <ContentTemplate>
                                       <asp:Label ID="lbl_header" runat="server" Text="Manage Booking"></asp:Label>
                                             </ContentTemplate>
                                             </asp:UpdatePanel>
                                            </h4>
                                               <button type="button" data-dismiss="modal" aria-label="Close" class="close"><span aria-hidden="true">×</span></button>
                                        </div>

          <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                         <div class="modal-body">
                                <asp:HiddenField ID="hd_appointmentid" Value="0" runat="server" />
                              
                                                <div class="row">
                                                    <div class="col-sm-12">
                                                <p><strong>Client Name: </strong> <asp:Label ID="lblFullname" runat="server" Text=""></asp:Label></p>
                                                    <p><strong>Message: </strong> <asp:Label ID="lblremarks" runat="server" Text=""></asp:Label> </p>
                                                    <p><strong>Date of Booking: </strong> <asp:Label ID="lblDateOfAppointment" runat="server" Text=""></asp:Label></p>
                                                 <p><strong>Car: </strong> <asp:Label ID="lblcarname" runat="server" Text=""></asp:Label></p>
                                                        </div>
                                                    </div>
                                <div class="form-group">
                                <div class="col-sm-12">
                                    <label class="form-label">Action Taken/Remarks</label>
                                        <asp:TextBox ID="txtremarks" runat="server" CssClass="form-control" placeholder="Action Taken/Remarks" TextMode="MultiLine"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="rfvFirstName" runat="server"  ControlToValidate="txtremarks" ErrorMessage="Remarks is required." CssClass="text-danger" Display="Dynamic"  ValidationGroup="add"></asp:RequiredFieldValidator>
                                    </div>
          
                                      <div class="col-sm-12">
                                        <label class="form-label">Status</label>
                                      <asp:DropDownList ID="dpstatus" runat="server" CssClass="form-control" OnSelectedIndexChanged="dpstatus_SelectedIndexChanged" AutoPostBack="True">
                                            <asp:ListItem Text="Pending" Value="pending"></asp:ListItem>
                                            <asp:ListItem Text="Approved" Value="approved"></asp:ListItem>
                                        <asp:ListItem Text="Rejected" Value="rejected"></asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
         
                              </div>
           
   
           
                                <div style="text-align: center">
                                <asp:Button ID="btnRegister" OnClick="btnRegister_Click" runat="server" Text="Submit" CssClass="btn btn-success" ValidationGroup="add" />
                              </div>
                             </div>
                           </ContentTemplate>
                    </asp:UpdatePanel>
      </div>
    </div>
</div>

</asp:Content>
