<%@ Page Title="Home" Language="C#" MasterPageFile="~/SiteCustomer.Master" AutoEventWireup="true" CodeBehind="CustomerHome.aspx.cs" Inherits="CarRentalSystem.Customer.CustomerHome" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder_header" runat="server">
     <script type="text/javascript">
         function openModal() {
             $('#registerModal').modal('show');
         }
         function getConfirmation_verify() {
             return confirm("Are you sure you want to delete?");
         }
        

     </script>
   
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder_body" runat="server">

     <div class="content-header">
      <div class="container">
        <div class="row mb-2">
                <div class="col-sm-6">
                   
                </div>
                <!-- /.col -->
               
                <!-- /.col -->
            </div>
            <!-- /.row -->
        </div>
        <!-- /.container-fluid -->
    </div>
    <!-- /.content-header -->


 <section class="content">
     <div class="container">
      <div class="row">
        <div class="col-md-12">
          <div class="card card-info">
            <div class="card-header">
              <h3 class="card-title">Reservation</h3>

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
                                              
                                                <div class="col-sm-6">
                                                 
                                                 <asp:LinkButton ID="btn_filter" CssClass=" btn btn-info" 
                                                         runat="server" onclick="btn_filter_Click" > Search</asp:LinkButton>
                                                          <asp:LinkButton ID="btn_reset" CssClass="btn btn-default" 
                                                         runat="server" onclick="btn_reset_Click">Refresh</asp:LinkButton>
                                          <asp:LinkButton ID="btn_add"  CssClass="btn btn-success" runat="server" 
                                    PostBackUrl="/AddReservation">Book a Car</asp:LinkButton>
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
                                         <asp:HyperLink ID="HyperLink1" runat="server"  Text='<%# Eval("bookingno") %>' Target="_blank" NavigateUrl='<%# "~/PrintReservation?id=" + Eval("booking_id") %> '></asp:HyperLink>

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
                                      <asp:LinkButton ID="btn_select" CssClass="btn btn-primary btn-xs "  CommandArgument='<%# Eval("booking_id") %>' Visible='<%# Eval("status").ToString().ToLower() == "approved" ? false : true %>' onclick="btn_select_Click"  runat="server" >Edit</asp:LinkButton>
                                      <asp:LinkButton ID="btn_delete" CssClass="btn btn-danger btn-xs " onclick="btn_delete_Click" runat="server"  Visible='<%# Eval("status").ToString().ToLower() == "approved" ? false : true %>'
                                     OnClientClick="return getConfirmation_verifys(this, 'Please confirm','Are you sure you want to Delete?');"
                                       >Remove</asp:LinkButton>
                                         <asp:HyperLink ID="HyperLink2" CssClass="btn btn-default btn-xs " Target="_blank" NavigateUrl='<%# "~/BillPrint?billid=" +Eval("billid") %>' Visible='<%# Eval("billid").ToString() == "0" ? false : true %>' runat="server">Print Bill</asp:HyperLink>
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
        <!-- /.container-fluid -->
    </section>
    
    <!-- Modal Container -->
</asp:Content>

