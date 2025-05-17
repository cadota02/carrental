<%@ Page Title="Billing Record" Language="C#" MasterPageFile="~/SiteAdmin.Master" AutoEventWireup="true" CodeBehind="BillingRecord.aspx.cs" Inherits="CarRentalSystem.Admin.BillingRecord" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder_header" runat="server">
     <script type="text/javascript">
     
         function getConfirmation_verify() {
             return confirm("Are you sure you want to delete?");
         }
    

     </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder_body" runat="server">
<section class="content">
     <div class="container mt-5">
      <div class="row">
        <div class="col-md-12">
          <div class="card card-info">
            <div class="card-header">
              <h3 class="card-title">Billing Records</h3>

              <div class="card-tools">
   
              
              </div>
            </div>
            <div class="card-body">
        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
            <ContentTemplate>
    

                    <div class="form-group row mb-2">
                               
                                                <div class="col-sm-4">
                                       
                                                 <asp:TextBox ID="txt_search" CssClass="form-control form-control-sm" placeholder="Enter keyword" runat="server"></asp:TextBox>
                                                </div>
                                              
                                                <div class="col-sm-8">
                                                 
                                                 <asp:LinkButton ID="btn_filter" CssClass="btn btn-info btn-sm" 
                                                         runat="server" onclick="btn_filter_Click" > Search</asp:LinkButton>
                                                          <asp:LinkButton ID="btn_reset" CssClass="btn btn-default btn-sm" 
                                                         runat="server" onclick="btn_reset_Click" BackColor="#999999">Refresh</asp:LinkButton>
                                               <%--<asp:LinkButton ID="btn_add" CssClass="btn btn-success btn-sm" 
                                                         runat="server"  PostBackUrl="~/ManageBill"> Add Bill</asp:LinkButton>--%>
                                                </div>
                                      
                                                  </div>
        <!-- GridView -->
                           <div class="table-responsive">
        <asp:GridView ID="gvCharges" runat="server" AutoGenerateColumns="False" CssClass="table table-sm table-bordered table-hover"
           AllowPaging="true" OnPageIndexChanging="OnPaging" PageSize="10" PagerSettings-Mode="NumericFirstLast"
             HeaderStyle-HorizontalAlign="Center" HeaderStyle-VerticalAlign="Middle" >
            <Columns>
                <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                       <HeaderTemplate>Billing No </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:HyperLink ID="HyperLink1" runat="server" Text='<%# Eval("billno") %>' Target="_blank" NavigateUrl='<%# "~/BillPrint?billid="+ Eval("billid") %>'></asp:HyperLink>
                                         
                                         </ItemTemplate>
                                    </asp:TemplateField>
              
                <asp:BoundField DataField="fullname" HeaderText="Customer" />
                                <asp:BoundField DataField="bookinfo" HeaderText="Booking Details" />
                <asp:BoundField DataField="amount" HeaderText="Amount" />
                  <asp:BoundField DataField="cash" HeaderText="Cash" />
                     <asp:BoundField DataField="change" HeaderText="Change" />
                  <asp:BoundField DataField="billdate" HeaderText="Date Bill"  DataFormatString="{0:MMM dd, yyyy}" />
                 <asp:BoundField DataField="remarks" HeaderText="Remarks" />
                 <asp:TemplateField>
                                        <HeaderTemplate> Action </HeaderTemplate>
                                     <ItemTemplate>
                                      <asp:HiddenField ID="hd_id" Value='<%#Eval("billid") %>' runat="server"></asp:HiddenField>
                        
                                      <asp:HiddenField ID="hd_name" Value='<%#Eval("billno") %>' runat="server"></asp:HiddenField>
                                      <asp:LinkButton ID="btn_select" CssClass="btn btn-primary btn-xs "  onclick="btn_select_Click"  runat="server" >Edit</asp:LinkButton>
                                      
                                         <asp:LinkButton ID="btn_delete" CssClass="btn btn-danger btn-xs " onclick="btn_delete_Click" runat="server"
                                      OnClientClick="return getConfirmation_verifys(this, 'Please confirm','Are you sure you want to Delete?');"
                                       >Remove</asp:LinkButton>

                                       
                                     </ItemTemplate>
                                   </asp:TemplateField>
            </Columns>
        </asp:GridView>
               </div>
            </ContentTemplate>
         </asp:UpdatePanel>
    </div>
       </div>
       </div>
    </div>
   </div>
 </section>

   
</asp:Content>

