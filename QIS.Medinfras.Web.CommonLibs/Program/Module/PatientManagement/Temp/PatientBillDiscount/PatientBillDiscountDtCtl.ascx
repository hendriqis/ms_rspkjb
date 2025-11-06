<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PatientBillDiscountDtCtl.ascx.cs" 
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.PatientBillDiscountDtCtl" %>

<%@ Register Src="~/Libs/Program/Module/PatientManagement/TransactionDtView/TransactionDtServiceViewCtl.ascx" TagName="ServiceCtl" TagPrefix="uc1" %>
<%@ Register Src="~/Libs/Program/Module/PatientManagement/TransactionDtView/TransactionDtProductViewCtl.ascx" TagName="DrugMSCtl" TagPrefix="uc1" %>

<script type="text/javascript" id="dxss_generatebilldtctl">
    $('#ulTabClinicBilling li').click(function () {
        $('#ulTabClinicBilling li.selected').removeAttr('class');
        $('.containerTransDt').filter(':visible').hide();
        $contentID = $(this).attr('contentid');
        $('#' + $contentID).show();
        $(this).addClass('selected');
    });
</script>
<style type="text/css">
    .containerTransDt                   { height: 300px;overflow-y:auto; border: 1px solid #EAEAEA; }
</style>
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
                    <td class="tdLabel"><label><%=GetLabel("No Bill")%></label></td>
                    <td><asp:TextBox ID="txtBillingNo" Width="150px" ReadOnly="true" runat="server" /></td>
                </tr>
                <tr>
                    <td class="tdLabel"><%=GetLabel("Tanggal") %> - <%=GetLabel("Jam") %></td>
                    <td>
                    <table cellpadding="0" cellspacing="0">
                        <tr>
                            <td style="padding-right: 1px;width:145px"><asp:TextBox ID="txtBillingDate" ReadOnly="true" Width="120px" CssClass="datepicker" runat="server" /></td>
                            <td style="width:5px">&nbsp;</td>
                            <td><asp:TextBox ID="txtBillingTime" Width="80px" CssClass="time" runat="server" ReadOnly="true" Style="text-align:center" /></td>
                        </tr>
                    </table>
                </td>
                </tr>
            </table>
        </td>
    </tr>
</table>
<div class="containerUlTabPage">
    <ul class="ulTabPage" id="ulTabClinicBilling">
        <li class="selected" contentid="containerService"><%=GetLabel("Pelayanan") %></li>
        <li contentid="containerDrugMS"><%=GetLabel("Obat & Alkes") %></li>
        <li contentid="containerLogistics"><%=GetLabel("Barang Umum") %></li>
        <li contentid="containerLaboratory"><%=GetLabel("Laboratorium") %></li>
        <li contentid="containerImaging"><%=GetLabel("Radiologi") %></li>
        <li contentid="containerMedicalDiagnostic"><%=GetLabel("Penunjang Medis") %></li>
    </ul>
</div>

<div id="containerService" class="containerTransDt">
    <uc1:ServiceCtl ID="ctlService" runat="server" />
</div>
<div id="containerDrugMS" style="display:none" class="containerTransDt">
    <uc1:DrugMSCtl ID="ctlDrugMS" runat="server" />
</div>
<div id="containerLogistics" style="display:none" class="containerTransDt">
    <uc1:DrugMSCtl ID="ctlLogistic" runat="server" />
</div>
<div id="containerLaboratory" style="display:none" class="containerTransDt">
    <uc1:ServiceCtl ID="ctlLaboratory" runat="server" />
</div>
<div id="containerImaging" style="display:none" class="containerTransDt">
    <uc1:ServiceCtl ID="ctlImaging" runat="server" />
</div>
<div id="containerMedicalDiagnostic" style="display:none" class="containerTransDt">
    <uc1:ServiceCtl ID="ctlMedicalDiagnostic" runat="server" />
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