<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="QRCodeCtl.ascx.cs" Inherits="QIS.Medinfras.Web.CommonLibs.Program.QRCodeCtl" %>
<script type="text/javascript" id="dxss_patientbannerctl">
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
  <div align="center" style="padding:25px">
    <asp:PlaceHolder ID="plBarCode" runat="server" />
    <input type="hidden" value="" id="hdnRegIDTransfer" runat="server" />
    <table class="tblEntryContent" style="width: 100%">
            <colgroup>
                <col style="width: 220px" />
                <col />
            </colgroup>
            <tr id="trReg" runat="server">
                <td class="tdLabel">
                    <label class="lblNormal" runat="server" id="lblNoReg">
                        <%:GetLabel("No. Registrasi Transfer")%></label>
                </td>
                <td>
                    <table style="width: 100%" cellpadding="0" cellspacing="0">
                        <colgroup>
                            <col style="width: 175px" />
                            <col />
                        </colgroup>
                        <tr>
                            <td>
                                <asp:TextBox ID="txtRegNo" Width="250px" runat="server" ReadOnly="true" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr id="trUnit" runat="server">
                <td class="tdLabel">
                    <label class="lblNormal" runat="server" id="lblUnit">
                        <%:GetLabel("Unit")%></label>
                </td>
                <td>
                    <table style="width: 100%" cellpadding="0" cellspacing="0">
                        <colgroup>
                            <col style="width: 175px" />
                            <col />
                        </colgroup>
                        <tr>
                            <td>
                                <asp:TextBox ID="txtUnit" Width="250px" runat="server" ReadOnly="true" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>        
            <tr id="trKamar" runat="server">
                <td class="tdLabel">
                    <label class="lblNormal" runat="server" id="lblKamar">
                        <%:GetLabel("Kamar ")%></label>
                </td>
                <td>
                    <table style="width: 100%" cellpadding="0" cellspacing="0">
                        <colgroup>
                            <col style="width: 100px" />
                            <col style="width: 100px" />
                        </colgroup>
                        <tr id="trBed" runat="server">
                            <td>
                                <asp:TextBox ID="txtKamar" Width="100px" runat="server" Style="margin-right: 3px;
                                    text-align: left" ReadOnly="true"  />
                            </td>
                            <td style="padding-left: 5px; padding-right: 5px">
                             <label class="lblNormal" runat="server" id="lblBed">
                                 <%:GetLabel("Bed ")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtBed" Width="100px" runat="server" Style="margin-right: 3px;
                                    text-align: left" ReadOnly="true"  />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
</div>
