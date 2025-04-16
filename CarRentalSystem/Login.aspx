<%@ Page Title="Login" Language="C#" MasterPageFile="~/SitePublic.Master" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="CarRentalSystem.Login" %>


<asp:Content ID="Content5" runat="server" 
    contentplaceholderid="ContentPlaceHolder_header">
    <div class="content-header">
        <div class="container">
            <div class="row">
                <div class="col-sm-6">
                
                </div>
                <!-- /.col -->
                <div class="col-sm-6">
                
                    <ol class="breadcrumb float-sm-right">
                 
                        <li class="breadcrumb-item"><a href="Home">Home</a></li>
                    
                        <li class="breadcrumb-item active">Login</li>
                    </ol>
                </div>
                <!-- /.col -->
            </div>
            <!-- /.row -->
        </div>
        <!-- /.container-fluid -->
    </div>
    <!-- /.content-header -->
      </asp:Content>
<asp:Content ID="Content6" runat="server" 
    contentplaceholderid="ContentPlaceHolder_body">
    <div class="content">
        <div class="container">
            <div class="row">
              <div class="col-lg-4 ">
                  
              </div>
                <div class="col-lg-4">
                 
                  <div class="card">
                  
    <div class="card-body login-card-body">
       
      <p class="login-box-msg pt-5" style="font-size: small"> <b>Login User</b>  <br />
   
         Car Rental Reservation System
      </p>
     

    
    <asp:Login ID="Login2" runat="server" OnAuthenticate="ValidateUser_Auth" 
                          width="100%" >
                    <LayoutTemplate>
                          <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Always">
                 <ContentTemplate>
        <div class="input-group mb-3">
        
           <asp:TextBox ID="UserName" runat="server" CssClass="form-control" name="loginUsername"   PlaceHolder="Username" ToolTip="Enter Username" TabIndex="1"></asp:TextBox>
          <div class="input-group-append">
            <div class="input-group-text">
              <span class="fas fa-user"></span>
            </div>
          </div>
        </div>
        <div class="input-group mb-3">
         <asp:TextBox ID="Password" runat="server" AutoPostBack="false" CssClass="form-control" name="loginPassword" TextMode="Password" PlaceHolder="Password" ToolTip="Enter password" TabIndex="2"></asp:TextBox>
          <div class="input-group-append">
            <div class="input-group-text">
              <span class="fas fa-lock"></span>
            </div>
          </div>
        </div>
                        <div  class="row col-sm-12">
                            
                              <small style="color: #FC7367"> <asp:Literal ID="FailureText" runat="server" EnableViewState="False"></asp:Literal></small>
                        </div>
        <div class="row">
          <div class="col-sm-9">

                  <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server"  Display="None" ErrorMessage="Username is required!" Font-Size="Small" ForeColor="#CC3300" 
                        ControlToValidate="UserName" SetFocusOnError="True" ValidationGroup="login"></asp:RequiredFieldValidator>
                   <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server"  Display="None" ErrorMessage="Password is required!" Font-Size="Small" ForeColor="#CC3300" 
                        ControlToValidate="Password" SetFocusOnError="True" ValidationGroup="login"></asp:RequiredFieldValidator>
             
              <asp:ValidationSummary ID="vsSummary" Font-Size="Small" ForeColor="#CC3300" runat="server"
    ValidationGroup="login"  />
          </div>
          <!-- /.col -->
          <div class="col-sm-3  style="padding-top: 20px"">
         
            <asp:Button ID="LoginButton" runat="server" CommandName="Login" Text="Sign In" CssClass="btn btn-primary btn-block" TabIndex="4" ValidationGroup="login"/>
          </div>
          <!-- /.col -->
        </div>
     
                         </ContentTemplate>
                  </asp:UpdatePanel>
        </LayoutTemplate>
                 </asp:Login>
   <%-- 
      <p >
        <a href="#">I forgot my password</a>
      </p>--%>
      
    </div>
    <!-- /.login-card-body -->
  </div>
                </div>
          <!-- /.col-md-6 -->
                <div class="col-lg-4">
                    
                </div>
          <!-- /.col-md-6 -->
            </div>
        
        <!-- /.row -->
        </div>
        <!-- /.container-fluid -->
    </div>
 
      </asp:Content>
