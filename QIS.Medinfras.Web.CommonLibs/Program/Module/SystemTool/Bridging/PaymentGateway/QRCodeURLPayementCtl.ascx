<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="QRCodeURLPayementCtl.ascx.cs" Inherits="QIS.Medinfras.Web.CommonLibs.Program.QRCodeURLPayementCtl" %>
<script type="text/javascript" id="dxss_QRCodeURLPayementCtl">
    $(function () {
        if (($('#<%=txtRegNo.ClientID %>').val() == "" || $('#<%=txtUnit.ClientID %>').val() == "" || $('#<%=txtKamar.ClientID %>').val() == "" || $('#<%=txtBed.ClientID %>').val() == "")) {
            $('#<%=trReg.ClientID %>').hide();
            $('#<%=txtUnit.ClientID %>').hide();
            $('#<%=txtKamar.ClientID %>').hide();
            $('#<%=txtBed.ClientID %>').hide();
            $('#<%=lblUnit.ClientID %>').hide();
            $('#<%=lblKamar.ClientID %>').hide();
            $('#<%=lblBed.ClientID %>').hide(); 
        }
    });

</script>
  <div align="center">
    <asp:PlaceHolder ID="plBarCode" runat="server" />
    <input type="hidden" value="" id="hdnUrlPayment" runat="server" />
    
</div>
