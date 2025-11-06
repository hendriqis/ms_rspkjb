<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PatientInformationTransactionDtCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Controls.PatientInformationTransactionDtCtl" %>
<%@ Register Src="~/Libs/Controls/InformationTransactionDt/InformationDtServiceCtl.ascx"
    TagName="ServiceCtl" TagPrefix="uc1" %>
<%@ Register Src="~/Libs/Controls/InformationTransactionDt/InformationDtDrugCtl.ascx"
    TagName="DrugCtl" TagPrefix="uc1" %>
<%@ Register Src="~/Libs/Controls/InformationTransactionDt/InformationDtDrugReturnCtl.ascx"
    TagName="DrugReturnCtl" TagPrefix="uc1" %>
<%@ Register Src="~/Libs/Controls/InformationTransactionDt/InformationDtLogisticCtl.ascx"
    TagName="LogisticCtl" TagPrefix="uc1" %>
<script type="text/javascript" id="dxss_transinfodtctl">
    $('#ulTabClinicTransaction li').click(function () {
        $('#ulTabClinicTransaction li.selected').removeAttr('class');
        $('.containerTransDt').filter(':visible').hide();
        $contentID = $(this).attr('contentid');
        $('#' + $contentID).show();
        $(this).addClass('selected');
    });

    if ($('#<%=hdnType.ClientID %>').val() == 'Prescription') {
        $('#ulTabClinicTransaction').find("[contentid='containerDrug']").click();
        $('#ulTabClinicTransaction').css('display', 'none');
    } else if ($('#<%=hdnType.ClientID %>').val() == 'PrescriptionReturn') {
        $('#ulTabClinicTransaction').find("[contentid='containerDrugReturn']").click();
        $('#ulTabClinicTransaction').css('display', 'none');
    }
</script>
<style type="text/css">
    .containerTransDt
    {
        height: 400px;
        overflow-y: auto;
        border: 1px solid #EAEAEA;
    }
</style>
<input type="hidden" value="" id="hdnType" runat="server" />
<table class="tblContentArea">
    <colgroup>
        <col style="width: 50%" />
        <col style="width: 50%" />
    </colgroup>
    <tr>
        <td style="padding: 5px; vertical-align: top">
            <table class="tblEntryContent" style="width: 100%">
                <colgroup>
                    <col style="width: 120px" />
                    <col />
                </colgroup>
                <tr>
                    <td class="tdLabel">
                        <label>
                            <%=GetLabel("No. Transaksi")%></label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtTransactionNo" Width="150px" ReadOnly="true" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td class="tdLabel">
                        <label>
                            <%=GetLabel("Tgl. Transaksi")%></label>
                    </td>
                    <td>
                        <table cellpadding="0" cellspacing="0">
                            <tr>
                                <td style="padding-right: 1px; width: 145px">
                                    <asp:TextBox ID="txtTransactionDate" ReadOnly="true" Width="120px" CssClass="datepicker"
                                        runat="server" />
                                </td>
                                <td style="width: 5px">
                                    &nbsp;
                                </td>
                                <td>
                                    <asp:TextBox ID="txtTransactionTime" Width="80px" CssClass="time" runat="server"
                                        ReadOnly="true" Style="text-align: center" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </td>
        <td style="padding: 5px; vertical-align: top">
            <table class="tblEntryContent" style="width: 100%">
                <colgroup>
                    <col style="width: 30%" />
                    <col />
                </colgroup>
                <tr>
                    <td class="tdLabel">
                        <label id="Label1" class="lblNormal" runat="server">
                            <%=GetLabel("No. Referensi")%></label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtReferenceNo" Width="100%" ReadOnly="true" runat="server" />
                    </td>
                </tr>
            </table>
        </td>
    </tr>
</table>
<div class="containerUlTabPage">
    <ul class="ulTabPage" id="ulTabClinicTransaction">
        <li class="selected" contentid="containerService">
            <%=GetLabel("Pelayanan") %></li>
        <li contentid="containerDrug">
            <%=GetLabel("Obat & Alkes") %></li>
        <li contentid="containerDrugReturn">
            <%=GetLabel("Retur Obat & Alkes") %></li>
        <li contentid="containerLogistic">
            <%=GetLabel("Barang Umum") %></li>
    </ul>
</div>
<div id="containerService" class="containerTransDt">
    <uc1:ServiceCtl ID="ctlService" runat="server" />
</div>
<div id="containerDrug" style="display: none" class="containerTransDt">
    <uc1:DrugCtl ID="ctlDrug" runat="server" />
</div>
<div id="containerDrugReturn" style="display: none" class="containerTransDt">
    <uc1:DrugReturnCtl ID="ctlDrugReturn" runat="server" />
</div>
<div id="containerLogistic" style="display: none" class="containerTransDt">
    <uc1:LogisticCtl ID="ctlLogistic" runat="server" />
</div>
<table style="width: 100%" cellpadding="0" cellspacing="0">
    <colgroup>
        <col style="width: 10%" />
        <col style="width: 35%" />
        <col style="width: 10%" />
        <col style="width: 35%" />
    </colgroup>
    <tr>
        <td>
            <div class="lblComponent" style="text-align: left; padding-left: 5px;">
                <%=GetLabel("Grand Total") %>
                :
            </div>
        </td>
        <td style="text-align: right; padding-right: 10px;">
            Rp.
            <asp:TextBox ID="txtTotal" ReadOnly="true" CssClass="number" runat="server" Width="200px" />
        </td>
    </tr>
    <tr>
        <td>
            <div class="lblComponent" style="text-align: left; padding-left: 5px;">
                <%=GetLabel("Grand Total Instansi") %>
                :
            </div>
        </td>
        <td style="text-align: right; padding-right: 10px;">
            Rp.
            <asp:TextBox ID="txtTotalPayer" ReadOnly="true" CssClass="number" runat="server"
                Width="200px" />
        </td>
        <td>
            <div class="lblComponent" style="text-align: left; padding-left: 5px;">
                <%=GetLabel("Grand Total Pasien") %>
                :
            </div>
        </td>
        <td style="text-align: right; padding-right: 10px;">
            Rp.
            <asp:TextBox ID="txtTotalPatient" ReadOnly="true" CssClass="number" runat="server"
                Width="200px" />
        </td>
    </tr>
</table>
