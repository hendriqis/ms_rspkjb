<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TransactionVerificationServiceCtl.ascx.cs" 
    Inherits="QIS.Medinfras.Web.Inpatient.Program.TransactionVerificationServiceCtl" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>

<script type="text/javascript" id="dxss_transactionverificationservicectl">
    $('#chkSelectAllResult').die('change');
    $('#chkSelectAllResult').live('change', function () {
        var isChecked = $(this).is(":checked");
        $('.chkIsSelected').each(function () {
            $(this).find('input').prop('checked', isChecked);
        });
    });

    $(function () {
        $('.grdService .chkIsSelected input').each(function () {
            if ($(this).closest('tr').find('.hdnCheckedValue').val() == 'False')
                $(this).prop('checked', false);
            else
                $(this).prop('checked', true);
        });
    });
</script>

<asp:ListView ID="lvwService" runat="server">
    <EmptyDataTemplate>
        <table id="tblView" runat="server" class="grdNormal grdService" cellspacing="0" rules="all" >
            <tr>  
                <th style="width:70px" align="center" rowspan="2"><input id="chkSelectAllResult" type="checkbox" /></th>
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
                <th rowspan="2" style="width:80px">
                    <div style="text-align:right;padding-right:3px">
                        <%=GetLabel("Harga Satuan")%>
                    </div>
                </th>
                <th rowspan="2" style="width:40px">
                    <div style="text-align:right;padding-right:3px">
                        <%=GetLabel("Jumlah")%>
                    </div>
                </th>
                <th colspan="3" align="center"><%=GetLabel("Harga")%></th>
                <th colspan="3" align="center"><%=GetLabel("Total")%></th>
                <th rowspan="2" style="width:90px">
                    <div style="text-align:right;padding-right:3px">
                        <%=GetLabel("Petugas")%>
                    </div>
                </th>                                
            </tr>
            <tr>
                <th style="width:70px">
                    <div style="text-align:right;padding-right:3px">
                        <%=GetLabel("Harga")%>
                    </div>
                </th>
                <th style="width:70px">
                    <div style="text-align:right;padding-right:3px">
                        <%=GetLabel("CITO")%>
                    </div>
                </th>
                <th style="width:70px">
                    <div style="text-align:right;padding-right:3px">
                        <%=GetLabel("Diskon")%>
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
                <td colspan="21">
                    <%=GetLabel("No Data To Display") %>
                </td>
            </tr>
        </table>
    </EmptyDataTemplate>
    <LayoutTemplate>                                
        <table id="tblView" runat="server" class="grdService grdNormal" cellspacing="0" rules="all" >
            <tr>
                <th style="width:70px" align="center" rowspan="2"><input id="chkSelectAllResult" type="checkbox" /></th>
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
                <th rowspan="2" style="width:80px">
                    <div style="text-align:right;padding-right:3px">
                        <%=GetLabel("Harga Satuan")%>
                    </div>
                </th>
                <th rowspan="2" style="width:40px">
                    <div style="text-align:right;padding-right:3px">
                        <%=GetLabel("Jumlah")%>
                    </div>
                </th>
                <th colspan="3" align="center"><%=GetLabel("Harga")%></th>
                <th colspan="3" align="center"><%=GetLabel("Total")%></th>
                <th rowspan="2" style="width:90px">
                    <div style="text-align:right;padding-right:3px">
                        <%=GetLabel("Petugas")%>
                    </div>
                </th>                                
            </tr>
            <tr>
                <th style="width:70px">
                    <div style="text-align:right;padding-right:3px">
                        <%=GetLabel("Harga")%>
                    </div>
                </th>
                <th style="width:70px">
                    <div style="text-align:right;padding-right:3px">
                        <%=GetLabel("CITO")%>
                    </div>
                </th>
                <th style="width:70px">
                    <div style="text-align:right;padding-right:3px">
                        <%=GetLabel("Diskon")%>
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
            <tr id="Tr1" class="trFooter" runat="server">
                <td colspan="9" align="right" style="padding-right:3px"><%=GetLabel("Total") %></td>
                <td align="right" style="padding-right:3px" id="tdServiceTotalPayer" runat="server"></td>
                <td align="right" style="padding-right:3px" id="tdServiceTotalPatient" runat="server"></td>
                <td align="right" style="padding-right:3px" id="tdServiceTotal" runat="server"></td>
                <td>&nbsp;</td>
            </tr>
        </table>
    </LayoutTemplate>
    <ItemTemplate>
        <tr>
            <td align="center">
                <asp:CheckBox ID="chkIsSelected" runat="server" CssClass="chkIsSelected" />
                <input type="hidden" class="keyField" id="keyField" runat="server" value='<%#: Eval("ID")%>' />
                <input type="hidden" class="hdnCheckedValue" id="hdnCheckedValue" value='<%#: Eval("IsVerified") %>' />
                
            </td>
            <td>
                <div style="padding:3px;">
                    <div class="divTransactionNo"><%#: Eval("ItemCode") %></div>                                         
                </div>
            </td>
            <td>
                <div style="padding:3px">
                    <div><%#: Eval("ItemName1")%></div>
                    <div><%#: Eval("ParamedicName")%></div>                                                    
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
                    <div><%#: Eval("ChargedQuantity")%></div>                                                   
                </div>
            </td>
            <td>
                <div style="padding:3px;text-align:right;">
                    <div><%#: Eval("GrossLineAmount", "{0:N}")%></div>                                                   
                </div>
            </td>
            <td>
                <div style="padding:3px;text-align:right;">
                    <div><%#: Eval("CITOAmount", "{0:N}")%></div>                                                   
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
