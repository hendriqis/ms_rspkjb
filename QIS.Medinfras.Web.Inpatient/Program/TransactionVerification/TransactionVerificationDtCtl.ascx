<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TransactionVerificationDtCtl.ascx.cs" 
    Inherits="QIS.Medinfras.Web.Inpatient.Program.TransactionVerificationDtCtl" %>

<%@ Register Src="~/Program/TransactionVerification/TransactionVerificationServiceCtl.ascx" TagName="ServiceCtl" TagPrefix="uc1" %>
<%@ Register Src="~/Program/TransactionVerification/TransactionVerificationDrugLogisticCtl.ascx" TagName="DrugLogisticCtl" TagPrefix="uc1" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>

<script type="text/javascript" id="dxss_generatebilldtctl">
    $('#ulTabClinicTransaction li').click(function () {
        $('#ulTabClinicTransaction li.selected').removeAttr('class');
        $('.containerTransDt').filter(':visible').hide();
        $contentID = $(this).attr('contentid');
        $('#' + $contentID).show();
        $(this).addClass('selected');
    });

    function onCbpProcessVerificationEndCallback(s) {
        hideLoadingPanel();
        cbpView.PerformCallback();
        pcRightPanelContent.Hide();
    }

    $('#<%=btnMPEntryPopupVerified.ClientID %>').click(function () {
        getCheckedMember();
        cbpProcessVerification.PerformCallback();
    });

    function getCheckedMember() {
        var lstSelectedMember = [];
        var lstUnselectedMember = [];

        var result = '';
        $('.grdService .chkIsSelected input').each(function () {
            var isChecked = $(this).is(":checked");
            var key = $(this).closest('tr').find('.keyField').val();

            if (isChecked)
                lstSelectedMember.push(key);
            else
                lstUnselectedMember.push(key);
        });
        $('.grdDrugMS .chkIsSelectedDrugLogistic input').each(function () {
            var isChecked = $(this).is(":checked");
            var key = $(this).closest('tr').find('.keyFieldDrugLogistic').val();

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
<style type="text/css">
    .containerTransDt                   { height: 300px;overflow-y:auto; border: 1px solid #EAEAEA; }
</style>
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
            </table>
        </td>
    </tr>
</table>
<div class="containerUlTabPage">
    <ul class="ulTabPage" id="ulTabClinicTransaction">
        <li class="selected" contentid="containerService"><%=GetLabel("Pelayanan") %></li>
        <li contentid="containerDrugMS"><%=GetLabel("Obat & Alkes") %></li>
        <li contentid="containerLogistics"><%=GetLabel("Barang Umum") %></li>
    </ul>
</div>

<div id="containerService" class="containerTransDt">
    <uc1:ServiceCtl ID="ctlService1" runat="server" />
</div>
<div id="containerDrugMS" style="display:none" class="containerTransDt">
    <uc1:DrugLogisticCtl ID="ctlDrugMS1" runat="server" />
</div>
<div id="containerLogistics" style="display:none" class="containerTransDt">
    <uc1:DrugLogisticCtl ID="ctlLogistic1" runat="server" />
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