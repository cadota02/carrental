<%@ Page Title="Contact" Language="C#" MasterPageFile="~/SitePublic.Master" AutoEventWireup="true" CodeBehind="Contact.aspx.cs" Inherits="CarRentalSystem.Contact" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder_header" runat="server">
   </asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder_body" runat="server">
      <div class="container mt-5">
            <div class="card shadow">
                <div class="card-header bg-primary text-white">
                    <h4>Contact Us</h4>
                </div>
                <div class="card-body">
                    <div class="row g-3">

                        <div class="col-md-6">
                            <label class="form-label">Full Name</label>
                            <asp:TextBox ID="txtFullName" runat="server" PlaceHolder="Enter your name" CssClass="form-control" required="true" />
                            <asp:RequiredFieldValidator ID="rfvFullName" runat="server" ControlToValidate="txtFullName"
                                ErrorMessage="Full Name is required" CssClass="text-danger" Display="Dynamic" />
                        </div>

                        <div class="col-md-6">
                            <label class="form-label">Email</label>
                            <asp:TextBox ID="txtEmail" runat="server"   PlaceHolder="Enter your email" CssClass="form-control" TextMode="Email" />
                            <asp:RequiredFieldValidator ID="rfvEmail" runat="server" ControlToValidate="txtEmail"
                                ErrorMessage="Email is required" CssClass="text-danger" Display="Dynamic" />
                        </div>

                        <div class="col-md-6">
                            <label class="form-label">Phone</label>
                            <asp:TextBox ID="txtPhone" runat="server"  PlaceHolder="Enter your phone" CssClass="form-control" />
                            <asp:RequiredFieldValidator ID="rfvPhone" runat="server" ControlToValidate="txtPhone"
                                ErrorMessage="Phone is required" CssClass="text-danger" Display="Dynamic" />
                        </div>

                        <div class="col-md-6">
                            <label class="form-label">Subject</label>
                            <asp:TextBox ID="txtSubject" runat="server"  PlaceHolder="Enter subject" CssClass="form-control" />
                            <asp:RequiredFieldValidator ID="rfvSubject" runat="server" ControlToValidate="txtSubject"
                                ErrorMessage="Subject is required" CssClass="text-danger" Display="Dynamic" />
                        </div>

                        <div class="col-md-12">
                            <label class="form-label">Message</label>
                            <asp:TextBox ID="txtMessage" runat="server"  PlaceHolder="Enter message" CssClass="form-control" TextMode="MultiLine" Rows="4" />
                            <asp:RequiredFieldValidator ID="rfvMessage" runat="server" ControlToValidate="txtMessage"
                                ErrorMessage="Message is required" CssClass="text-danger" Display="Dynamic" />
                        </div>

                        <div class="col-md-12 mt-5 text-center">
                            <asp:Button ID="btnSubmit" runat="server" CssClass="btn btn-success" Text="Submit" OnClick="btnSubmit_Click" />
                        </div>

                        <div class="col-md-12">
                            <asp:Label ID="lblResult" runat="server" CssClass="text-success fw-bold" />
                        </div>

                    </div>
                </div>
            </div>
        </div>
</asp:Content>
