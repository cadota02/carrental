function compute_item(c, val) {
    var num2_id = c.id;

    var stockid = num2_id.replace(val, "hd_remaining");
    var stock_value = document.getElementById(stockid).value;

    var qtyid = num2_id.replace(val, "txt_qtydelivered");

    var qty_value = document.getElementById(qtyid).value;


   
    if ((parseFloat(qty_value || 0) > parseFloat(stock_value || 0))) {

     
        MessageShow('Stock request exceeded!', '', 'Warning');
        document.getElementById(qtyid).value = "";
        return;
    }
 
    if (parseFloat(stock_value || 0) <= 0) {

        MessageShow("No available stock!", '', 'Warning')
        document.getElementById(qtyid).value = "";
        return;
    }
    if (qty_value.indexOf('-') > -1) {
        document.getElementById(qtyid).value = "";
        return;
    }
    
    if (parseFloat(qty_value || 0) <= 0 && parseFloat(stock_value || 0) < 0) {

        MessageShow("Qty must be none negative number!", '', 'Warning')
       
        document.getElementById(qtyid).value = "";
        return;
    }

    if (qty_value.indexOf('.') > -1) {
      
        document.getElementById(qtyid).value = "";
    }
  
 

}




function isNumberKey(txt, evt) {
    var charCode = (evt.which) ? evt.which : evt.keyCode;
    if (charCode == 46) {
        //Check if the text already contains the . character
        if (txt.value.indexOf('.') === -1) {
            return false;
        } else {
            return false;
        }
    } else {
        if (charCode > 31 &&
          (charCode < 48 || charCode > 57))
            return false;
    }
    return true;
}
   