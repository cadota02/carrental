<%@ Page Title="Cars" Language="C#" MasterPageFile="~/SiteAdmin.Master" AutoEventWireup="true" validateRequest="false" CodeBehind="Cars.aspx.cs" Inherits="CarRentalSystem.Admin.Cars" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder_header" runat="server">
    <script src="/Scripts/ckeditor/ckeditor.js"></script>
<script type="text/javascript">
    function loadCKEditor() {
        var editorId = '<%= txtContent.ClientID %>';

         // Ensure the element exists before initializing CKEditor
         var editorElement = document.getElementById(editorId);
         if (editorElement) {
             // Destroy previous instance if any
             if (CKEDITOR.instances[editorId]) {
                 CKEDITOR.instances[editorId].destroy(true);
             }

             // Initialize CKEditor
             CKEDITOR.replace(editorId, {
                 on: {
                     instanceReady: function (evt) {
                         // Remove security warning if exists
                         var warningElement = CKEDITOR.document.getById('cke_warning');
                         if (warningElement) {
                             warningElement.remove();
                         }
                     }
                 }
             });
         } else {
             console.error('Element with ID "' + editorId + '" not found.');
         }
     }

    // Re-initialize CKEditor after every async postback
    Sys.Application.add_load(function () {
        loadCKEditor();
    });

    // Also init CKEditor on first page load
    window.onload = function () {
        loadCKEditor();
    };
  

</script>

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
                <div class="col-sm-6">
                    <ol class="breadcrumb float-sm-right">
                        <li class="breadcrumb-item"><a href="AdminHome">Home</a></li>
                        <li class="breadcrumb-item active">Cars</li>
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
              <h3 class="card-title">Manage Car</h3>

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
                                      OnClick="btn_add_Click">Add Car</asp:LinkButton>
                                                </div>
                                             
                                                  </div>
                       <div class="table-responsive">
                           <asp:GridView ID="gv_masterlist" runat="server" 
                              CssClass="table  table-bordered table-sm  table-hover align-middle" 
                              AutoGenerateColumns="false" AllowPaging="true"
                              OnPageIndexChanging="OnPaging" PageSize="10" 
                            GridLines="None" PagerSettings-Mode="NumericFirstLast"
                              HeaderStyle-HorizontalAlign="Center" HeaderStyle-VerticalAlign="Middle" Font-Size="Small">
                                  <Columns>
                                            
                          
                                     <asp:TemplateField ItemStyle-CssClass="text-center align-middle">
                                       <HeaderTemplate> # </HeaderTemplate>
                                        <ItemTemplate>
       
                                            <asp:Label ID="lbl_no" runat="server" Text="<%# Container.DataItemIndex + 1 %>  "></asp:Label>
                                         </ItemTemplate>
                                    </asp:TemplateField>
                                         <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="text-center align-middle">
                                       <HeaderTemplate> Image </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:Image ID="Image1" runat="server" ImageUrl='<%# Eval("image_path") %>' Width="70px" Height="70px" ImageAlign="Middle" />
                                            
                                         </ItemTemplate>
                                    </asp:TemplateField>
                                           <asp:BoundField DataField="car_name" HeaderText="Car Name" ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="text-center align-middle"/>
                                             <asp:TemplateField HeaderText="Description" ItemStyle-CssClass="align-middle">
                                                <ItemTemplate>
                                               <asp:Literal ID="ltContentPreview" Text='<%# Eval("description") %>' runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            
                                                 <asp:BoundField DataField="contact_person" HeaderText="Contact Person"    ItemStyle-HorizontalAlign="Left"  ItemStyle-CssClass="text-center align-middle" />
                                                <asp:BoundField DataField="contact_no" HeaderText="Contact No" ItemStyle-HorizontalAlign="Left"  ItemStyle-CssClass="text-center align-middle" />
                                                <asp:BoundField DataField="Status" HeaderText="Status" ItemStyle-HorizontalAlign="Center"  ItemStyle-CssClass="text-center align-middle" />

                                   <asp:TemplateField  ItemStyle-CssClass="text-center align-middle">
                                        <HeaderTemplate> Action </HeaderTemplate>
                                     <ItemTemplate>
                                      <asp:HiddenField ID="hd_id" Value='<%#Eval("car_id") %>' runat="server"></asp:HiddenField>
                           <asp:HiddenField ID="hd_status" Value='<%#Eval("is_available") %>' runat="server"></asp:HiddenField>
                                      <asp:HiddenField ID="hd_name" Value='<%#Eval("car_name") %>' runat="server"></asp:HiddenField>
                                      <asp:LinkButton ID="btn_select" CssClass="btn btn-primary btn-xs "  CommandArgument='<%# Eval("car_id") %>' onclick="btn_select_Click"  runat="server" >Edit</asp:LinkButton>
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
                                       <asp:Label ID="lbl_header" runat="server" Text="Add Car"></asp:Label>
                                             </ContentTemplate>
                                             </asp:UpdatePanel>
                                            </h4>
                                               <button type="button" data-dismiss="modal" aria-label="Close" class="close"><span aria-hidden="true">×</span></button>
                                        </div>

          <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                          <div class="modal-body">
       
        <asp:HiddenField ID="hd_carid" Value="0" runat="server" />
                         <div class="form-group row">
                        <div class="col-sm-6">
                            <label>Car Name
                              <asp:RequiredFieldValidator ControlToValidate="txtCarName" runat="server" ErrorMessage="*" ForeColor="Red" ValidationGroup="add" />
                            </label>
                            <asp:TextBox ID="txtCarName" runat="server" CssClass="form-control" />
                     
                        </div>
                      <div class="col-sm-6">
                              <label>Available?</label>
                             <asp:CheckBox ID="chkAvailable" CssClass="form-control"  runat="server" />

                        </div>
                       </div>
                         <div class="form-group row">
                        <div class="col-sm-6">
                            <label>Contact Person</label>
                            <asp:TextBox ID="txtContactPerson" runat="server" CssClass="form-control" />
                        </div>
                        <div class="col-sm-6">
                            <label>Contact No</label>
                            <asp:TextBox ID="txtContactNo" runat="server" CssClass="form-control" />
                        </div>
                             </div>
                               <div class="form-group row">
                        
                         <div class="col-sm-12">
                            <label>Upload Image (.jpeg/.png)</label>
                            <asp:FileUpload ID="fuImage" runat="server" CssClass="form-control" />
                            <asp:Image ID="imgPreview" runat="server" Width="100" Visible="false" CssClass="mt-2" />
                        </div>
                         </div>
                              <div>
                               <asp:TextBox ID="txtContent" runat="server" TextMode="MultiLine" Rows="10" Columns="60"></asp:TextBox>

                              </div>
      
                              </div>
                        <div class="modal-footer">
                        <asp:Button ID="btnSave" runat="server" OnClick="btnRegister_Click" ValidationGroup="add" Text="Save" CssClass="btn btn-success" />
                          <asp:Button ID="btnClose" runat="server" OnClick="btnClose_Click"  Text="Close" CssClass="btn btn-secondary" />
                         
                    </div>
                   </ContentTemplate>
                           <Triggers>
                    <asp:PostBackTrigger ControlID="btnSave" />
                </Triggers>
                    </asp:UpdatePanel> 
   
    </div>
   </div>
 
</div>
</asp:Content>
