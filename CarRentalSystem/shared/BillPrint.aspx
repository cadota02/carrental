<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BillPrint.aspx.cs" Inherits="CarRentalSystem.shared.BillPrint" %>

<!DOCTYPE html>
<html>
<head>
    <title>Billing Statement Print</title>
    <link href="styles.css" rel="stylesheet" />
    <script>
        window.onload = function () {
            window.print();
        };
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div style="padding: 20px;">
      
            <asp:Literal ID="litInvoiceHtml" runat="server" />
        </div>
    </form>
</body>
</html>
