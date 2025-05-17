<%@ Page Title="Contacts" Language="C#" MasterPageFile="~/SiteAdmin.Master" AutoEventWireup="true" CodeBehind="Contacts.aspx.cs" Inherits="CarRentalSystem.Admin.Contacts" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder_header" runat="server">
     <script type="text/javascript">
         function openModal() {
             $('#registerModal').modal('show');
         }
         function getConfirmation_verify() {
             return confirm("Are you sure you want to delete?");
         }
         function getConfirmation_action(status) {
             let msg = "";
             if (status === "new") msg = "Mark this message as read?";
             else if (status === "read") msg = "Archive this message?";
             else msg = "Mark Read this message again?";

             return confirm(msg);
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
                <div class="col-sm-6">
                    <ol class="breadcrumb float-sm-right">
                        <li class="breadcrumb-item"><a href="AdminHome">Home</a></li>
                        <li class="breadcrumb-item active">Contacts</li>
                    </ol>
                </div>
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
              <h3 class="card-title">Manage Contacts</h3>

              <div class="card-tools">
   
              
              </div>
            </div>
            <div class="card-body">
                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                                 <ContentTemplate>
                                             <div class="form-group row">
                               
                                                <div class="col-sm-4">
                                       
                                                 <asp:TextBox ID="txt_search" CssClass="form-control form-control-sm" placeholder="Enter keyword" runat="server"></asp:TextBox>
                                                </div>
                                              
                                                <div class="col-sm-8">
                                                 
                                                 <asp:LinkButton ID="btn_filter" CssClass="btn btn-info btn-sm" 
                                                         runat="server" onclick="btn_filter_Click" > Search</asp:LinkButton>
                                                          <asp:LinkButton ID="btn_reset" CssClass="btn btn-default btn-sm" 
                                                         runat="server" onclick="btn_reset_Click" BackColor="#999999">Refresh</asp:LinkButton>
                                                     
                                                </div>
                                             
                                                  </div>
                       <div class="table-responsive">
                           <asp:GridView ID="gv_masterlist" runat="server" 
                              CssClass="table  table-bordered table-sm  table-hover" 
                              AutoGenerateColumns="false" AllowPaging="true"
                              OnPageIndexChanging="OnPaging" PageSize="10" 
                            GridLines="None" PagerSettings-Mode="NumericFirstLast"
                              HeaderStyle-HorizontalAlign="Center" HeaderStyle-VerticalAlign="Middle" Font-Size="Small">
                                  <Columns>
                                            
                          
                                     <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                       <HeaderTemplate> # </HeaderTemplate>
                                        <ItemTemplate>
       
                                            <asp:Label ID="lbl_no" runat="server" Text="<%# Container.DataItemIndex + 1 %>  "></asp:Label>
                                         </ItemTemplate>
                                    </asp:TemplateField>
                                           <asp:BoundField DataField="full_name" HeaderText="Name"  ItemStyle-HorizontalAlign="Center" />
                                               <asp:BoundField DataField="email" HeaderText="Email" ItemStyle-HorizontalAlign="Center" />
                                                 <asp:BoundField DataField="subject" HeaderText="Subject"    ItemStyle-HorizontalAlign="Left" />
                                                <asp:BoundField DataField="message" HeaderText="Message" ItemStyle-HorizontalAlign="Left" />
                                        <asp:BoundField DataField="phone" HeaderText="Phone" ItemStyle-HorizontalAlign="Center" />
                                            <asp:BoundField DataField="created_at" DataFormatString="{0:MMM dd, yyyy hh:mm tt}" HeaderText="Date/time" ItemStyle-HorizontalAlign="Center" />
                                      <asp:TemplateField HeaderText="Status" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:Label ID="lblStatus" runat="server">
                                            <i class='<%# Eval("status").ToString() == "new" 
                        ? "fas fa-eye-slash text-muted"   /* Eye-slash icon for new (unseen) */
                        : Eval("status").ToString() == "read" 
                            ? "fas fa-check-circle text-success"  /* Check-circle icon for read */
                            : "fas fa-archive text-warning"  /* Archive icon for archived */
                    %>' 
               title='<%# Eval("status") %>'></i>
                                        </asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                   <asp:TemplateField>
                                        <HeaderTemplate> Action </HeaderTemplate>
                                     <ItemTemplate>
                                      <asp:HiddenField ID="hd_id" Value='<%#Eval("contact_id") %>' runat="server"></asp:HiddenField>
                           <asp:HiddenField ID="hd_status" Value='<%#Eval("status") %>' runat="server"></asp:HiddenField>
                                      <asp:HiddenField ID="hd_name" Value='<%#Eval("full_name") %>' runat="server"></asp:HiddenField>

                                      <asp:LinkButton ID="btn_delete" CssClass="btn btn-danger btn-xs " onclick="btn_delete_Click" runat="server"
                                      OnClientClick="return getConfirmation_verifys(this, 'Please confirm','Are you sure you want to Delete?');"
                                       >Remove</asp:LinkButton>

                                         <asp:Button ID="btnAction" runat="server"
                            Text='<%# Eval("status").ToString() == "new" ? "Mark as Read" :
                                         Eval("status").ToString() == "read" ? "Archive" : "Read" %>'
                            CssClass='<%# Eval("status").ToString() == "new" ? "btn btn-xs btn-warning" :
                                              Eval("status").ToString() == "read" ? "btn btn-xs btn-secondary" : "btn btn-xs btn-success" %>'
                            CommandArgument='<%# Eval("contact_id") + "|" + Eval("status") %>'
                            OnClick="btnAction_Click"
                       
                            OnClientClick='<%# "return getConfirmation_action(\"" + Eval("status") + "\");" %>' />
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
