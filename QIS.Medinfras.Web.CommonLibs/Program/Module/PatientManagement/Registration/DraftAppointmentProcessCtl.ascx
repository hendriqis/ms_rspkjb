<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DraftAppointmentProcessCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.DraftAppointmentProcessCtl" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Src="~/Libs/Program/Module/PatientManagement/TransactionDtView/DraftTransactionDtServiceViewCtl.ascx"
    TagName="ServiceCtl" TagPrefix="uc1" %>
<%@ Register Src="~/Libs/Program/Module/PatientManagement/TransactionDtView/DraftTransactionDtProductViewCtl.ascx"
    TagName="DrugMSCtl" TagPrefix="uc1" %>
<%@ Register Src="~/Libs/Program/Module/PatientManagement/TransactionDtView/DraftTestOrderDtServiceViewCtl.ascx"
    TagName="TestOrderCtl" TagPrefix="uc1" %>
<%@ Register Src="~/Libs/Program/Module/PatientManagement/TransactionDtView/DraftPrescriptionOrderDtServiceViewCtl.ascx"
    TagName="PrescriptionOrderCtl" TagPrefix="uc1" %>
<script type="text/javascript" id="dxss_patiententryctl">
    $('.chkSelectAll input').change(function () {
        var isChecked = $(this).is(":checked");

        $('.chkIsSelected').each(function () {
            $(this).find('input').prop('checked', isChecked);
        });

        $('.chkIsSelectedTestOrder').each(function () {
            $(this).find('input').prop('checked', isChecked);
        });

        $('.chkIsSelectedPrescriptionOrder').each(function () {
            $(this).find('input').prop('checked', isChecked);
        });
        //        calculateTotal();
    });

    function onBeforeSaveRecord(errMessage) {
        var paramCharges = '';
        var paramOrder = '';
        var paramPrescription = '';

        $('.chkIsSelected input:checked').each(function () {
            $tr = $(this).closest('tr');
            if (paramCharges != '')
                paramCharges += '|';
            paramCharges += $tr.find('.hdnKeyField').val();
        });
        $('#<%=hdnParamCharges.ClientID %>').val(paramCharges);

        $('.chkIsSelectedTestOrder input:checked').each(function () {
            $tr = $(this).closest('tr');
            if (paramOrder != '')
                paramOrder += '|';
            paramOrder += $tr.find('.hdnKeyField').val();
        });
        $('#<%=hdnParamTestOrder.ClientID %>').val(paramOrder);

        $('.chkIsSelectedPrescriptionOrder input:checked').each(function () {
            $tr = $(this).closest('tr');
            if (paramPrescription != '')
                paramPrescription += '|';
            paramPrescription += $tr.find('.hdnKeyField').val();
        });
        $('#<%=hdnParamPrescriptionOrder.ClientID %>').val(paramPrescription);        

        return true;
    }

    function onCbpRecalculationPatientBillProcessEndCallback(s) {
        $('#ulTabRecalculatePatientBillProcess li').click(function () {
            $('#ulTabRecalculatePatientBillProcess li.selected').removeAttr('class');
            $('.containerRecalculationProcess').filter(':visible').hide();
            $contentID = $(this).attr('contentid');
            $('#' + $contentID).show();
            $(this).addClass('selected');
        });
        hideLoadingPanel();
    }

    $('#ulTabRecalculatePatientBillProcess li').click(function () {
        $('#ulTabRecalculatePatientBillProcess li.selected').removeAttr('class');
        $('.containerRecalculationProcess').filter(':visible').hide();
        $contentID = $(this).attr('contentid');
        $('#' + $contentID).show();
        $(this).addClass('selected');
    });

</script>
<style type="text/css">
    .containerRecalculationProcess
    {
        height: 280px;
        overflow-y: auto;
        border: 1px solid #EAEAEA;
    }
</style>
<input type="hidden" value="" id="hdnParamCharges" runat="server" />
<input type="hidden" value="" id="hdnParamTestOrder" runat="server" />
<input type="hidden" value="" id="hdnParamPrescriptionOrder" runat="server" />
<div>
    <input type="hidden" runat="server" id="hdnAppointmentID" value="" />
    <input type="hidden" runat="server" id="hdnRegistrationID" value="" />
    <input type="hidden" runat="server" id="hdnLinkedRegistrationID" value="" />
    <input type="hidden" runat="server" id="hdnHealthcareServiceUnitID" value="" />
    <input type="hidden" runat="server" id="hdnHealthcareServiceUnitIDIS" value="" />
    <input type="hidden" runat="server" id="hdnHealthcareServiceUnitIDLB" value="" />
    <div style="text-align: left; width: 100%; margin-bottom: 10px;">
    </div>
    <dxcp:ASPxCallbackPanel ID="cbpRecalculationPatientBillProcess" runat="server" Width="1200px"
        ClientInstanceName="cbpRecalculationPatientBillProcess" ShowLoadingPanel="false"
        OnCallback="cbpRecalculationPatientBillProcess_Callback">
        <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ onCbpRecalculationPatientBillProcessEndCallback(s); }" />
        <PanelCollection>
            <dx:PanelContent ID="PanelContent1" runat="server">
                <div class="containerUlTabPage">
                    <ul class="ulTabPage" id="ulTabRecalculatePatientBillProcess">
                        <li class="selected" contentid="containerService">
                            <%=GetLabel("Pelayanan") %></li>
                        <li contentid="containerDrugMS">
                            <%=GetLabel("Obat & Alkes") %></li>
                        <li contentid="containerLogistics">
                            <%=GetLabel("Barang Umum") %></li>
                        <li contentid="containerLaboratory">
                            <%=GetLabel("Laboratorium") %></li>
                        <li contentid="containerImaging">
                            <%=GetLabel("Radiologi") %></li>
                        <li contentid="containerMedicalDiagnostic">
                            <%=GetLabel("Penunjang Medis") %></li>
                        <li contentid="containerPharmacy">
                            <%=GetLabel("Farmasi") %></li>
                    </ul>
                </div>
                <div id="containerService" class="containerRecalculationProcess">
                    <uc1:ServiceCtl ID="ctlService" runat="server" />
                </div>
                <div id="containerDrugMS" style="display: none" class="containerRecalculationProcess">
                    <uc1:DrugMSCtl ID="ctlDrugMS" runat="server" />
                </div>
                <div id="containerLogistics" style="display: none" class="containerRecalculationProcess">
                    <uc1:DrugMSCtl ID="ctlLogistic" runat="server" />
                </div>
                <div id="containerLaboratory" style="display: none" class="containerRecalculationProcess">
                    <uc1:TestOrderCtl ID="ctlLaboratory" runat="server" />
                </div>
                <div id="containerImaging" style="display: none" class="containerRecalculationProcess">
                    <uc1:TestOrderCtl ID="ctlImaging" runat="server" />
                </div>
                <div id="containerMedicalDiagnostic" style="display: none" class="containerRecalculationProcess">
                    <uc1:TestOrderCtl ID="ctlMedicalDiagnostic" runat="server" />
                </div>
                <div id="containerPharmacy" style="display: none" class="containerRecalculationProcess">
                    <uc1:PrescriptionOrderCtl ID="ctlPharmacy" runat="server" />
                </div>
                <table style="width: 100%" cellpadding="0" cellspacing="0">
                    <colgroup>
                        <col style="width: 15%" />
                        <col style="width: 35%" />
                        <col style="width: 15%" />
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
            </dx:PanelContent>
        </PanelCollection>
    </dxcp:ASPxCallbackPanel>
</div>
