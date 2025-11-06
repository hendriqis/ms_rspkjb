<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TransactionVerificationDrugLogisticCtl.ascx.cs" 
    Inherits="QIS.Medinfras.Web.Inpatient.Program.TransactionVerificationDrugLogisticCtl" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>

<script type="text/javascript" id="dxss_transactionverificationservicectl">
    $('#chkSelectAllResultDrugLogistic').die('change');
    $('#chkSelectAllResultDrugLogistic').live('change', function () {
        var isChecked = $(this).is(":checked");
        $('.chkIsSelectedDrugLogistic').each(function () {
            $(this).find('input').prop('checked', isChecked);
        });
    });

    $(function () {
        $('#<%=pnlDrugMS.ClientID %> .chkIsSelectedDrugLogistic input').each(function () {
            if ($(this).closest('tr').find('.hdnCheckedValueDrugLogistic').val() == 'False') {
                $(this).prop('checked', false);
            }
            else
                $(this).prop('checked', true);
        });
    });
</script>

<asp:Panel ID="pnlDrugMS" runat="server">
    <asp:ListView ID="lvwDrugLogistic" runat="server" OnItemDataBound="lvwDrugLogistic_ItemDataBound">
        <LayoutTemplate>                                
            <table id="tblView" runat="server" class="grdDrugMS grdNormal notAllowSelect" cellspacing="0" rules="all" >
                <tr> 
                    <th style="width:70px" align="center" rowspan="2"><input id="chkSelectAllResultDrugLogistic" type="checkbox" /></th> 
                    <th style="width:70px" rowspan="2">
                        <div style="text-align:left;padding-left:3px">
                            <%=GetLabel("Kode")%>
                        </div>
                    </th>
                    <th rowspan="2">
                        <div style="text-align:left;padding-left:3px">
                            <%=GetLabel("Deskripsi")%>
                        </div>
                    </th>
                    <th rowspan="2">
                        <div style="text-align:left;padding-left:3px">
                            <%=GetLabel("Kelas Tagihan")%>
                        </div>
                    </th>
                    <th style="width:80px" rowspan="2">
                        <div style="text-align:right;padding-right:3px">
                            <%=GetLabel("Harga Satuan")%>
                        </div>
                    </th>
                    <th colspan="3" align="center"><%=GetLabel("Jumlah")%></th>
                    <th colspan="2" align="center"><%=GetLabel("Jumlah Satuan Kecil")%></th>
                    <th rowspan="2" style="width:70px">
                        <div style="text-align:right;padding-right:3px">
                            <%=GetLabel("Harga")%>
                        </div>
                    </th>
                    <th rowspan="2" style="width:70px">
                        <div style="text-align:right;padding-right:3px">
                            <%=GetLabel("Diskon")%>
                        </div>
                    </th>
                    <th colspan="3" align="center"><%=GetLabel("Total")%></th>
                    <th rowspan="2" style="width:90px">
                        <div style="text-align:right;padding-right:3px">
                            <%=GetLabel("Petugas")%>
                        </div>
                    </th>                                
                </tr>
                <tr>                                
                    <th style="width:50px">
                        <div style="text-align:center;padding-right:3px">
                            <%=GetLabel("Digunakan")%>
                        </div>
                    </th>
                    <th style="width:50px">
                        <div style="text-align:center;padding-right:3px">
                            <%=GetLabel("Dibebankan")%>
                        </div>
                    </th>
                    <th style="width:50px">
                        <div style="text-align:center;padding-right:3px">
                            <%=GetLabel("Satuan")%>
                        </div>
                    </th>
                    <th style="width:40px">
                        <div style="text-align:center;padding-right:3px">
                            <%=GetLabel("Jumlah")%>
                        </div>
                    </th>
                    <th style="width:140px">
                        <div style="text-align:center;padding-right:3px">
                            <%=GetLabel("Konversi")%>
                        </div>
                    </th>
                    <th style="width:80px">
                        <div style="text-align:right;padding-right:3px">
                            <%=GetLabel("Instansi")%>
                        </div>
                    </th>
                    <th style="width:80px">
                        <div style="text-align:right;padding-right:3px">
                            <%=GetLabel("Pasien")%>
                        </div>
                    </th>
                    <th style="width:80px">
                        <div style="text-align:right;padding-right:3px">
                            <%=GetLabel("Total")%>
                        </div>
                    </th>
                </tr>
                <tr runat="server" id="itemPlaceholder" ></tr>
                <tr id="Tr2" class="trFooter" runat="server">
                    <td colspan="12" align="right" style="padding-right:3px"><%=GetLabel("Total") %></td>
                    <td align="right" style="padding-right:3px" id="tdDrugMSTotalPayer" runat="server"></td>
                    <td align="right" style="padding-right:3px" id="tdDrugMSTotalPatient" runat="server"></td>
                    <td align="right" style="padding-right:3px" id="tdDrugMSTotal" runat="server"></td>
                    <td>&nbsp;</td>
                </tr>
            </table>
        </LayoutTemplate>
        <EmptyDataTemplate>
            <table id="tblView" runat="server" class="grdDrugMS grdNormal notAllowSelect" cellspacing="0" rules="all" >
                <tr>  
                    <th style="width:70px" align="center" rowspan="2"><input id="chkSelectAllResultDrugLogistic" type="checkbox" /></th>
                    <th style="width:70px" rowspan="2">
                        <div style="text-align:left;padding-left:3px">
                            <%=GetLabel("Kode")%>
                        </div>
                    </th>
                    <th rowspan="2">
                        <div style="text-align:left;padding-left:3px">
                            <%=GetLabel("Deskripsi")%>
                        </div>
                    </th>
                    <th rowspan="2">
                        <div style="text-align:left;padding-left:3px">
                            <%=GetLabel("Kelas Tagihan")%>
                        </div>
                    </th>
                    <th style="width:80px" rowspan="2">
                        <div style="text-align:right;padding-right:3px">
                            <%=GetLabel("Harga Satuan")%>
                        </div>
                    </th>
                    <th colspan="3" align="center"><%=GetLabel("Jumlah")%></th>
                    <th colspan="2" align="center"><%=GetLabel("Jumlah Satuan Kecil")%></th>
                    <th rowspan="2" style="width:70px">
                        <div style="text-align:right;padding-right:3px">
                            <%=GetLabel("Harga")%>
                        </div>
                    </th>
                    <th rowspan="2" style="width:70px">
                        <div style="text-align:right;padding-right:3px">
                            <%=GetLabel("Diskon")%>
                        </div>
                    </th>
                    <th colspan="3" align="center"><%=GetLabel("Total")%></th>
                    <th rowspan="2" style="width:90px">
                        <div style="text-align:right;padding-right:3px">
                            <%=GetLabel("Petugas")%>
                        </div>
                    </th>                                
                </tr>
                <tr>                                
                    <th style="width:50px">
                        <div style="text-align:center;padding-right:3px">
                            <%=GetLabel("Digunakan")%>
                        </div>
                    </th>
                    <th style="width:50px">
                        <div style="text-align:center;padding-right:3px">
                            <%=GetLabel("Dibebankan")%>
                        </div>
                    </th>
                    <th style="width:50px">
                        <div style="text-align:center;padding-right:3px">
                            <%=GetLabel("Satuan")%>
                        </div>
                    </th>
                    <th style="width:40px">
                        <div style="text-align:center;padding-right:3px">
                            <%=GetLabel("Jumlah")%>
                        </div>
                    </th>
                    <th style="width:140px">
                        <div style="text-align:center;padding-right:3px">
                            <%=GetLabel("Konversi")%>
                        </div>
                    </th>
                    <th style="width:80px">
                        <div style="text-align:right;padding-right:3px">
                            <%=GetLabel("Instansi")%>
                        </div>
                    </th>
                    <th style="width:80px">
                        <div style="text-align:right;padding-right:3px">
                            <%=GetLabel("Pasien")%>
                        </div>
                    </th>
                    <th style="width:80px">
                        <div style="text-align:right;padding-right:3px">
                            <%=GetLabel("Total")%>
                        </div>
                    </th>
                </tr>
                <tr class="trEmpty">
                    <td colspan="20">
                        <%=GetLabel("No Data To Display") %>
                    </td>
                </tr>
            </table>
        </EmptyDataTemplate>
        <ItemTemplate>
            <tr>
                <td align="center">
                    <asp:CheckBox ID="chkIsSelectedDrugLogistic" runat="server" CssClass="chkIsSelectedDrugLogistic" />
                    <input type="hidden" class="keyFieldDrugLogistic" id="keyFieldDrugLogistic" runat="server" value='<%#: Eval("ID")%>' />
                    <input type="hidden" class="hdnCheckedValueDrugLogistic" id="hdnCheckedValueDrugLogistic" value='<%#: Eval("IsVerified") %>' />             
                </td>
                <td>
                    <div style="padding:3px;">
                        <div class="divTransactionNo"><%#: Eval("ItemCode") %></div>                                         
                    </div>
                </td>
                <td>
                    <div style="padding:3px">
                        <div><%#: Eval("ItemName1")%></div>                                                  
                    </div>
                </td>
                <td>
                    <div style="padding:3px;">
                        <div><%#: Eval("ChargeClassName")%></div>                                                   
                    </div>
                </td>
                <td>
                    <div style="padding:3px;text-align:right;">
                        <div><%#: Eval("Tariff", "{0:N}")%></div>                                                   
                    </div>
                </td>
                <td>
                    <div style="padding:3px;text-align:right;">
                        <div><%#: Eval("UsedQuantity")%></div>                                                   
                    </div>
                </td>
                <td>
                    <div style="padding:3px;text-align:right;">
                        <div><%#: Eval("ChargedQuantity")%></div>                                                   
                    </div>
                </td>
                <td>
                    <div style="padding:3px;">
                        <div><%#: Eval("ItemUnit")%></div>                                                   
                    </div>
                </td>
                <td>
                    <div style="padding:3px;text-align:right;">
                        <div><%#: Eval("BaseQuantity")%></div>                                                   
                    </div>
                </td>
                <td>
                    <div style="padding:3px;">
                        <div><%#: Eval("Conversion")%></div>                                                   
                    </div>
                </td>
                <td>
                    <div style="padding:3px;text-align:right;">
                        <div><%#: Eval("GrossLineAmount", "{0:N}")%></div>                                                   
                    </div>
                </td>
                <td>
                    <div style="padding:3px;text-align:right;">
                        <div><%#: Eval("DiscountAmount", "{0:N}")%></div>                                                   
                    </div>
                </td>
                <td>
                    <div style="padding:3px;text-align:right;">
                        <div><%#: Eval("PayerAmount", "{0:N}")%></div>                                                   
                    </div>
                </td>
                <td>
                    <div style="padding:3px;text-align:right;">
                        <div><%#: Eval("PatientAmount", "{0:N}")%></div>                                                   
                    </div>
                </td>
                <td>
                    <div style="padding:3px;text-align:right;">
                        <div><%#: Eval("LineAmount", "{0:N}")%></div>                                                   
                    </div>
                </td>
                <td>
                    <div style="padding:3px;text-align:right;">
                        <div><%#: Eval("LastUpdatedByUserName")%></div>
                        <div><%#: Eval("LastUpdatedDateInString")%></div>                                                 
                    </div>
                </td>
            </tr>
        </ItemTemplate>
    </asp:ListView>
</asp:Panel>
