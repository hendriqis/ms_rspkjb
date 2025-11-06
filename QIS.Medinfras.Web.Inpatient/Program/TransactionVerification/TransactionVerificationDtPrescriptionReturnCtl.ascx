<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TransactionVerificationDtPrescriptionReturnCtl.ascx.cs" 
    Inherits="QIS.Medinfras.Web.Inpatient.Program.TransactionVerificationDtPrescriptionReturnCtl" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>

<script type="text/javascript" id="dxss_generatebilldtprescriptionctl">
    function onCbpProcessVerificationEndCallback(s) {
        hideLoadingPanel();
        cbpView.PerformCallback();
        pcRightPanelContent.Hide();
    }

    $('#chkSelectAllPrescriptionReturn').die('change');
    $('#chkSelectAllPrescriptionReturn').live('change', function () {
        var isChecked = $(this).is(":checked");
        $('.chkIsSelected').each(function () {
            $(this).find('input').prop('checked', isChecked);
        });
    });

    $(function () {
        $('.grdPrescription .chkIsSelected input').each(function () {
            if ($(this).closest('tr').find('.hdnCheckedValue').val() == 'False')
                $(this).prop('checked', false);
            else
                $(this).prop('checked', true);
        });
    });

    $('#<%=btnMPEntryPopupVerified.ClientID %>').click(function () {
        getCheckedMember();
        cbpProcessVerification.PerformCallback();
    });

    function getCheckedMember() {
        var lstSelectedMember = [];
        var lstUnselectedMember = [];

        var result = '';
        $('.grdPrescription .chkIsSelected input').each(function () {
            var isChecked = $(this).is(":checked");
            var key = $(this).closest('tr').find('.keyField').val();
            if (isChecked)
                lstSelectedMember.push(key);
            else
                lstUnselectedMember.push(key);
        });
        $('#<%=hdnSelectedMember.ClientID %>').val(lstSelectedMember.join(','));
        $('#<%=hdnUnselectedMember.ClientID %>').val(lstUnselectedMember.join(','));
    }
</script>
<input type="hidden" id="hdnSelectedMember" runat="server" value="" />
<input type="hidden" id="hdnUnselectedMember" runat="server" value="" />
<input type="hidden" id="hdnTransactionHdID" runat="server" value="" />
<div class="toolbarArea">
    <ul>
        <li runat="server" id="btnMPEntryPopupVerified"><img src='<%=ResolveUrl("~/Libs/Images/Icon/tbset.png")%>' alt="" /><div><%=GetLabel("Verifikasi")%></div></li>
    </ul>
</div>
<table class="tblContentArea">
    <colgroup>
        <col style="width:50%"/>
        <col style="width:50%"/>
    </colgroup>
    <tr>
        <td style="padding:5px;vertical-align:top">
            <table class="tblEntryContent" style="width:100%">
                <colgroup>
                    <col style="width:30%"/>
                    <col/>
                </colgroup>
                <tr>
                    <td class="tdLabel"><label><%=GetLabel("No Bukti")%></label></td>
                    <td><asp:TextBox ID="txtTransactionNo" Width="150px" ReadOnly="true" runat="server" /></td>
                </tr>
                <tr>
                    <td class="tdLabel"><%=GetLabel("Tanggal") %> - <%=GetLabel("Jam") %></td>
                    <td>
                    <table cellpadding="0" cellspacing="0">
                        <tr>
                            <td style="padding-right: 1px;width:145px"><asp:TextBox ID="txtTransactionDate" ReadOnly="true" Width="120px" CssClass="datepicker" runat="server" /></td>
                            <td style="width:5px">&nbsp;</td>
                            <td><asp:TextBox ID="txtTransactionTime" Width="80px" CssClass="time" runat="server" ReadOnly="true" Style="text-align:center" /></td>
                        </tr>
                    </table>
                </td>
                </tr>
            </table>
        </td>
        <td style="padding:5px;vertical-align:top">
            <table class="tblEntryContent" style="width:100%">
                <colgroup>
                    <col style="width:30%"/>
                    <col/>
                </colgroup>
                <tr>
                    <td class="tdLabel"><label id="Label1" class="lblNormal" runat="server"><%=GetLabel("No Referensi")%></label></td>
                    <td><asp:TextBox ID="txtReferenceNo" Width="100%" ReadOnly="true" runat="server" /></td>
                </tr>
                <tr>
                    <td class="tdLabel"><label id="Label2" class="lblNormal" runat="server"><%=GetLabel("Return Type")%></label></td>
                    <td><asp:TextBox ID="txtPrescriptionReturnType" Width="100%" ReadOnly="true" runat="server" /></td>
                </tr>
            </table>
        </td>
    </tr>
</table>
<div class="containerUlTabPage" style="height:300px;background-color:White;">
    <asp:ListView ID="lvwPrescription" runat="server">
        <EmptyDataTemplate>
            <table id="Table1" runat="server" class="grdNormal grdPrescription notAllowSelect" cellspacing="0" rules="all">
                <tr>
                    <th style="width:70px" align="center" rowspan="2"><input id="chkSelectAllPrescriptionReturn" type="checkbox" /></th>
                    <th rowspan="2"><%=GetLabel("Item Name") %></th>
                    <th colspan="2" align="center"><%=GetLabel("Dikembalikan") %></th>
                    <th colspan="3" align="center"><%=GetLabel("Jumlah") %></th>
                </tr>
                <tr>
                    <th style="width:80px" align="center"><%=GetLabel("Jumlah") %></th>
                    <th style="width:80px" align="center"><%=GetLabel("Satuan") %></th>
                    <th style="width:150px" align="center"><%=GetLabel("Instansi") %></th>
                    <th style="width:150px" align="center"><%=GetLabel("Pasien") %></th>
                    <th style="width:150px" align="center"><%=GetLabel("Total") %></th>
                </tr>
                <tr align="center" style="height:50px; vertical-align:middle;">
                    <td colspan="8"><%=GetLabel("No Data To Display")%></td>
                </tr>
            </table>
        </EmptyDataTemplate>
        <LayoutTemplate>
            <table id="tblView" runat="server" class="grdNormal grdPrescription notAllowSelect" cellspacing="0" rules="all">
                <tr>
                    <th style="width:70px" align="center" rowspan="2"><input id="chkSelectAllPrescriptionReturn" type="checkbox" /></th>
                    <th rowspan="2"><%=GetLabel("Item Name") %></th>
                    <th colspan="2" align="center"><%=GetLabel("Dikembalikan") %></th>
                    <th colspan="3" align="center"><%=GetLabel("Jumlah") %></th>
                </tr>
                <tr>
                    <th style="width:80px" align="center"><%=GetLabel("Jumlah") %></th>
                    <th style="width:80px" align="center"><%=GetLabel("Satuan") %></th>
                    <th style="width:150px" align="center"><%=GetLabel("Instansi") %></th>
                    <th style="width:150px" align="center"><%=GetLabel("Pasien") %></th>
                    <th style="width:150px" align="center"><%=GetLabel("Total") %></th>
                </tr>
                <tr runat="server" id="itemPlaceholder" ></tr>
            </table>
        </LayoutTemplate>
        <ItemTemplate>
            <tr>
                <td align="center">
                    <asp:CheckBox ID="chkIsSelected" runat="server" CssClass="chkIsSelected" />
                    <input type="hidden" class="keyField" id="keyField" runat="server" value='<%#: Eval("ID")%>' />
                    <input type="hidden" class="hdnCheckedValue" id="hdnCheckedValue" value='<%#: Eval("IsVerified") %>' />
                </td>
                <td align="left"><%#:Eval("ItemName1") %></td>
                <td align="right"><%#:Eval("ItemQty") %></td>
                <td align="center"><%#:Eval("ItemUnit") %></td>
                <td align="right"><%#:Eval("PayerAmount", "{0:N}") %></td>
                <td align="right"><%#:Eval("PatientAmount", "{0:N}") %></td>
                <td align="right"><%#:Eval("LineAmount", "{0:N}") %></td>
            </tr>
        </ItemTemplate>
    </asp:ListView>
</div> 
<table style="width:100%" cellpadding="0" cellspacing="0">
    <colgroup>
        <col style="width:15%"/>
        <col style="width:35%"/>
        <col style="width:15%"/>
        <col style="width:35%"/>
    </colgroup>
    <tr>
        <td><div class="lblComponent" style="text-align: left; padding-left: 5px;"><%=GetLabel("Grand Total") %> : </div></td>
        <td style="text-align:right;padding-right: 10px;">
            Rp. <asp:TextBox ID="txtTotal" ReadOnly="true" CssClass="number" runat="server" Width="200px" />
        </td>
    </tr>
    <tr>
        <td><div class="lblComponent" style="text-align: left; padding-left: 5px;"><%=GetLabel("Grand Total Instansi") %> : </div></td>
        <td style="text-align:right;padding-right: 10px;">
            Rp. <asp:TextBox ID="txtTotalPayer" ReadOnly="true" CssClass="number" runat="server" Width="200px" />
        </td>
        <td><div class="lblComponent" style="text-align: left; padding-left: 5px;"><%=GetLabel("Grand Total Pasien") %>  : </div></td>
        <td style="text-align:right;padding-right: 10px;">
            Rp. <asp:TextBox ID="txtTotalPatient" ReadOnly="true" CssClass="number" runat="server" Width="200px" />
        </td>
    </tr>
</table>
<div style="display: none">
    <dxcp:ASPxCallbackPanel ID="cbpProcessVerification" runat="server" Width="100%" ClientInstanceName="cbpProcessVerification"
        ShowLoadingPanel="false" OnCallback="cbpProcessVerification_Callback">
            <ClientSideEvents BeginCallback="function(s,e) { showLoadingPanel(); }"
        EndCallback="function(s,e) { onCbpProcessVerificationEndCallback(s); }" />
    </dxcp:ASPxCallbackPanel>
</div>
