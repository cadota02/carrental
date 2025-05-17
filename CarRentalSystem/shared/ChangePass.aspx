  <%@ Page   Title="Change Password"Language="C#" AutoEventWireup="true" CodeBehind="ChangePass.aspx.cs" Inherits="CarRentalSystem.shared.ChangePass" %>
<%@ MasterType VirtualPath="~/Site.master" %>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder_header" runat="server">
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
<div class="content">
        <div class="container">
            <div class="row">
              <div class="col-lg-4 ">
                  
              </div>
                <div class="col-lg-4">
                 
                  <div class="card">
                  
    <div class="card-body login-card-body" style="border-radius: 20px;">

          <h5 class="text-center text-gray">Change Password</h5>
        <hr>
     <div class="form-group">
    <asp:Label ID="lblOldPass" runat="server" Text="Current Password: " />
    <asp:TextBox ID="txtOldPassword" CssClass="form-control" placeholder="Enter Old password" runat="server" TextMode="Password" />
    <asp:RequiredFieldValidator ID="rfvOldPass" runat="server"
        ControlToValidate="txtOldPassword"
        ErrorMessage="Old password is required"
        CssClass="text-danger"
        ValidationGroup="changepass"
        Display="Dynamic" />
</div>

<div class="form-group">
    <asp:Label ID="lblNewPass" runat="server" Text="New Password: " />
    <asp:TextBox ID="txtNewPassword" CssClass="form-control" placeholder="Enter New password" runat="server" TextMode="Password" />
    <asp:RequiredFieldValidator ID="rfvNewPass" runat="server"
        ControlToValidate="txtNewPassword"
        ErrorMessage="New password is required"
        CssClass="text-danger"
        ValidationGroup="changepass"
        Display="Dynamic" />
</div>

<div class="form-group">
    <asp:Label ID="lblConfirmPass" runat="server" Text="Confirm Password: " />
    <asp:TextBox ID="txtConfirmPassword" CssClass="form-control" placeholder="Enter Confirm password" runat="server" TextMode="Password" />
    <asp:RequiredFieldValidator ID="rfvConfirmPass" runat="server"
        ControlToValidate="txtConfirmPassword"
        ErrorMessage="Confirm password is required"
        CssClass="text-danger"
        ValidationGroup="changepass"
        Display="Dynamic" />
    <asp:CompareValidator ID="cvConfirmPass" runat="server"
        ControlToCompare="txtNewPassword"
        ControlToValidate="txtConfirmPassword"
        ErrorMessage="Passwords do not match"
        CssClass="text-danger"
        ValidationGroup="changepass"
        Display="Dynamic" />
</div>

<div class="row">
    <div class="col-sm-12 text-center">
        <asp:Label ID="lblMessage" runat="server" ForeColor="Red" Visible="false" />
        <asp:Button ID="btnChangePassword" CssClass="btn btn-primary btn-block" runat="server"
            Text="Change Password"
            ValidationGroup="changepass"
            OnClick="btnChangePassword_Click" />
    </div>
</div>
  
           
    </div>
   </div>
  </div>
  </div>
 </div>
 </div>
</asp:Content>

