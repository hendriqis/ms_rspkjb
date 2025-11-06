<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ItemServiceItemCostEntryCtl.ascx.cs" 
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.ItemServiceItemCostEntryCtl" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>

<script type="text/javascript" id="dxss_serviceunithealthcareentryctl">
    $(function () {
        $('.txtCurrency').each(function () {
            $(this).trigger('changeValue');
        });

        $('.txtItemCostCurrent').change(function () {
            $prev = parseInt($(this).closest('tr').find('.txtItemCostPrev').attr('hiddenVal'));
            $current = parseInt($(this).val());
            $total = $prev + $current;
            $(this).closest('tr').find('.txtItemCostTotal').val($total).trigger('changeValue');
        });
    });
</script>

<input type="hidden" id="hdnItemCostID" value="" runat="server" />
<table class="tblContentArea">
    <colgroup>
        <col style="width:100%"/>
    </colgroup>
    <tr>            
        <td style="padding:5px;vertical-align:top">
            <fieldset id="fsEntryPopup" style="margin:0"> 
                <table class="tblEntryContent" >
                    <colgroup>
                        <col style="width:160px"/>
                        <col style="width:120px"/>
                        <col style="width:120px"/>
                        <col style="width:120px"/>
                    </colgroup>
                    <tr>
                        <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Item")%></label></td>
                        <td colspan="3"><asp:TextBox ID="txtItemName" ReadOnly="true" Width="100%" runat="server" /></td>
                    </tr>  
                    <tr>
                        <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Healthcare")%></label></td>
                        <td colspan="2"><asp:TextBox ID="txtHealthcareName" ReadOnly="true" Width="100%" runat="server" /></td>
                    </tr>                            
                    <tr>
                        <td>&nbsp;</td>
                        <td align="center"><div class="lblComponent"><%=GetLabel("PREVIOUS")%></div></td>
                        <td align="center"><div class="lblComponent"><%=GetLabel("CURRENT")%></div></td>
                        <td align="center"><div class="lblComponent"><%=GetLabel("TOTAL")%></div></td>
                    </tr>                          
                    <tr>
                        <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Material")%></label></td>
                        <td><asp:TextBox ID="txtMaterialPrev" CssClass="txtCurrency txtItemCostPrev" ReadOnly="true" runat="server" Width="100%" /></td>
                        <td><asp:TextBox ID="txtMaterialCurrent" CssClass="txtCurrency required txtItemCostCurrent" runat="server" Width="100%" /></td>
                        <td><asp:TextBox ID="txtMaterialTotal" CssClass="txtCurrency txtItemCostTotal" ReadOnly="true" runat="server" Width="100%" /></td>
                    </tr>                           
                    <tr>
                        <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Labor")%></label></td>
                        <td><asp:TextBox ID="txtLaborPrev" CssClass="txtCurrency txtItemCostPrev" ReadOnly="true" runat="server" Width="100%" /></td>
                        <td><asp:TextBox ID="txtLaborCurrent" CssClass="txtCurrency required txtItemCostCurrent" runat="server" Width="100%" /></td>
                        <td><asp:TextBox ID="txtLaborTotal" CssClass="txtCurrency txtItemCostTotal" ReadOnly="true" runat="server" Width="100%" /></td>
                    </tr>                           
                    <tr>
                        <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Overhead")%></label></td>
                        <td><asp:TextBox ID="txtOverheadPrev" CssClass="txtCurrency txtItemCostPrev" ReadOnly="true" runat="server" Width="100%" /></td>
                        <td><asp:TextBox ID="txtOverheadCurrent" CssClass="txtCurrency required txtItemCostCurrent" runat="server" Width="100%" /></td>
                        <td><asp:TextBox ID="txtOverheadTotal" CssClass="txtCurrency txtItemCostTotal" ReadOnly="true" runat="server" Width="100%" /></td>
                    </tr>                           
                    <tr>
                        <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Sub Contract")%></label></td>
                        <td><asp:TextBox ID="txtSubContractPrev" CssClass="txtCurrency txtItemCostPrev" ReadOnly="true" runat="server" Width="100%" /></td>
                        <td><asp:TextBox ID="txtSubContractCurrent" CssClass="txtCurrency required txtItemCostCurrent" runat="server" Width="100%" /></td>
                        <td><asp:TextBox ID="txtSubContractTotal" CssClass="txtCurrency txtItemCostTotal" ReadOnly="true" runat="server" Width="100%" /></td>
                    </tr>                           
                    <tr>
                        <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Burden")%></label></td>
                        <td><asp:TextBox ID="txtBurdenPrev" CssClass="txtCurrency txtItemCostPrev" ReadOnly="true" runat="server" Width="100%" /></td>
                        <td><asp:TextBox ID="txtBurdenCurrent" CssClass="txtCurrency required txtItemCostCurrent" runat="server" Width="100%" /></td>
                        <td><asp:TextBox ID="txtBurdenTotal" CssClass="txtCurrency txtItemCostTotal" ReadOnly="true" runat="server" Width="100%" /></td>
                    </tr>  
                </table>
            </fieldset>
        </td>
    </tr>
</table>

