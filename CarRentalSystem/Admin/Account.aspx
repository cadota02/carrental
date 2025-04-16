<%@ Page Title="Account" Language="C#" MasterPageFile="~/SiteAdmin.Master" AutoEventWireup="true" CodeBehind="Account.aspx.cs" Inherits="CarRentalSystem.Admin.Account" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder_header" runat="server">
     <script type="text/javascript">
         function openModal() {
             $('#registerModal').modal('show');
         }
         function getConfirmation_verify() {
             return confirm("Are you sure you want to delete?");
         }
         function getConfirmation_activate(status) {
             var action = status === "Active" ? "deactivate" : "activate";
             return confirm("Are you sure you want to " + action + "?");
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
                        <li class="breadcrumb-item active">Account</li>
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
          <div class="card card-primary">
            <div class="card-header">
              <h3 class="card-title">Manage Account</h3>

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
                                                        <asp:LinkButton ID="btn_add"  CssClass="btn btn-success btn-sm" runat="server" 
                                      OnClick="btn_add_Click">Register User</asp:LinkButton>
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
                                           <asp:BoundField DataField="Username" HeaderText="Username"  ItemStyle-HorizontalAlign="Center" />
                                               <asp:BoundField DataField="Fullname" HeaderText="Fullname" ItemStyle-HorizontalAlign="Center" />
                                                 <asp:BoundField DataField="UserPosition" HeaderText="Position"    ItemStyle-HorizontalAlign="Left" />
                                                <asp:BoundField DataField="Email" HeaderText="Email" ItemStyle-HorizontalAlign="Left" />
                                        <asp:BoundField DataField="Role" HeaderText="Role" ItemStyle-HorizontalAlign="Center" />
                                                <asp:BoundField DataField="Status" HeaderText="Status" ItemStyle-HorizontalAlign="Center" />

                                   <asp:TemplateField>
                                        <HeaderTemplate> Action </HeaderTemplate>
                                     <ItemTemplate>
                                      <asp:HiddenField ID="hd_id" Value='<%#Eval("ID") %>' runat="server"></asp:HiddenField>
                           <asp:HiddenField ID="hd_status" Value='<%#Eval("IsApproved") %>' runat="server"></asp:HiddenField>
                                      <asp:HiddenField ID="hd_name" Value='<%#Eval("Fullname") %>' runat="server"></asp:HiddenField>
                                      <asp:LinkButton ID="btn_select" CssClass="btn btn-primary btn-xs "  CommandArgument='<%# Eval("ID") %>' onclick="btn_select_Click"  runat="server" >Edit</asp:LinkButton>
                                      <asp:LinkButton ID="btn_delete" CssClass="btn btn-danger btn-xs " onclick="btn_delete_Click" runat="server"
                                      OnClientClick="return getConfirmation_verifys(this, 'Please confirm','Are you sure you want to Delete?');"
                                       >Remove</asp:LinkButton>

                                           <asp:Button ID="btnAction" runat="server" 
                    Text='<%# Eval("Status").ToString() == "Active" ? "Deactivate" : "Activate" %>' 
                    CssClass='<%# Eval("Status").ToString() == "Active" ? "btn btn-xs btn-warning" : "btn btn-xs btn-info" %>' 
                    CommandArgument='<%# Eval("ID") %>'
                    OnClick="btnAction_Click"   OnClientClick='<%# "return getConfirmation_activate(\"" + Eval("Status") + "\");" %>'/>
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
<div id="registerModal" class="modal fade"  role="dialog">
     <div class="modal-dialog modal-lg">
   <div class="modal-content">
                         <div class="modal-header">
                          
                                            <h4 class="modal-title">
                                                 <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                                                 <ContentTemplate>
                                       <asp:Label ID="lbl_header" runat="server" Text="New User"></asp:Label>
                                             </ContentTemplate>
                                             </asp:UpdatePanel>
                                            </h4>
                                               <button type="button" data-dismiss="modal" aria-label="Close" class="close"><span aria-hidden="true">×</span></button>
                                        </div>

          <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                          <div class="modal-body">
        <asp:HiddenField ID="hd_userid" Value="0" runat="server" />
       
        <div class="form-group row">
        <div class="col-sm-4">
            <label class="form-label">First Name</label>
                <asp:TextBox ID="txtFirstName" runat="server" CssClass="form-control" placeholder="Enter first name"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvFirstName" runat="server" ControlToValidate="txtFirstName" ErrorMessage="First name is required." CssClass="text-danger" Display="Dynamic"  ValidationGroup="add"></asp:RequiredFieldValidator>
            </div>
           <div class="col-sm-4">
                <label class="form-label">Last Name</label>
                <asp:TextBox ID="txtLastName" runat="server" CssClass="form-control" placeholder="Enter last name"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvLastName" runat="server" ControlToValidate="txtLastName" ErrorMessage="Last name is required." CssClass="text-danger" Display="Dynamic"  ValidationGroup="add"></asp:RequiredFieldValidator>
            </div>
              <div class="col-sm-4">
                <label class="form-label">Middle Name</label>
                <asp:TextBox ID="txtMiddleName" runat="server" CssClass="form-control" placeholder="Enter middle name"></asp:TextBox>
            </div>
      </div>
           
       <div class="form-group row">
            <div class="col-sm-4">
                <label class="form-label">Position</label>
                 <asp:TextBox ID="txtPosition" runat="server" CssClass="form-control" placeholder="Enter Position"></asp:TextBox>
            </div>
           <div class="col-sm-4">
                <label class="form-label">Email</label>
                <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" TextMode="Email" placeholder="Enter email"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvEmail" runat="server" ControlToValidate="txtEmail" ErrorMessage="Email is required." CssClass="text-danger" Display="Dynamic"  ValidationGroup="add"></asp:RequiredFieldValidator>
            </div>
            <div class="col-sm-4">
                <label class="form-label">Contact No</label>
                <asp:TextBox ID="txtContactNo" runat="server" CssClass="form-control" placeholder="Enter contact number"></asp:TextBox>
            </div>
           
        </div>
           <div class="form-group row">
            <div class="col-sm-6">
                <label class="form-label">Address</label>
                <asp:TextBox ID="txtAddress" runat="server" CssClass="form-control" placeholder="Enter address"></asp:TextBox>
            </div>
                <div class="col-sm-3">
                <label class="form-label">User Role</label>
                <asp:DropDownList ID="dprole" runat="server" CssClass="form-control">
                    <asp:ListItem Text="Administrator" Value="Admin"></asp:ListItem>
                    <asp:ListItem Text="Client" Value="Client"></asp:ListItem>
             
                </asp:DropDownList>
            </div>
                <div class="col-sm-3">
                <label class="form-label">Status</label>
                <asp:DropDownList ID="dpstatus" runat="server" CssClass="form-control">
                    <asp:ListItem Text="Inactive" Value="0"></asp:ListItem>
                    <asp:ListItem Text="Active" Value="1"></asp:ListItem>
             
                </asp:DropDownList>
            </div>
           </div>
            <div class="form-group row">
              <div class="col-sm-4">
                    <label class="form-label">Username</label>
                    <asp:TextBox ID="txtUsername" runat="server" CssClass="form-control" placeholder="Enter username"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvUsername" runat="server" ControlToValidate="txtUsername" ErrorMessage="Username is required." CssClass="text-danger" Display="Dynamic"  ValidationGroup="add"></asp:RequiredFieldValidator>
                </div>
               <div class="col-sm-4">
                    <label class="form-label">Password</label>
                    <asp:TextBox ID="txtPassword" runat="server" CssClass="form-control" TextMode="Password" placeholder="Enter password"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvPassword" runat="server" ControlToValidate="txtPassword" ErrorMessage="Password is required." CssClass="text-danger" Display="Dynamic"  ValidationGroup="add"></asp:RequiredFieldValidator>
                </div>
              <div class="col-sm-4">
                    <label class="form-label">Confirm Password</label>
                    <asp:TextBox ID="txtConfirmPassword" runat="server" CssClass="form-control" TextMode="Password" placeholder="Confirm password"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvConfirmPassword" runat="server" ControlToValidate="txtConfirmPassword" ErrorMessage="Confirm password is required." CssClass="text-danger" Display="Dynamic"  ValidationGroup="add"></asp:RequiredFieldValidator>
                    <asp:CompareValidator ID="cvPasswordMatch" runat="server" ControlToValidate="txtConfirmPassword" ControlToCompare="txtPassword" ErrorMessage="Passwords do not match." CssClass="text-danger" Display="Dynamic"  ValidationGroup="add"></asp:CompareValidator>
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
