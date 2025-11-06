<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PatientBillSummaryRecalculationBillProcessCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.PatientBillSummaryRecalculationBillProcessCtl" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Src="~/Libs/Program/Module/PatientManagement/TransactionDtView/TransactionDtServiceViewCtl.ascx"
    TagName="ServiceCtl" TagPrefix="uc1" %>
<%@ Register Src="~/Libs/Program/Module/PatientManagement/TransactionDtView/TransactionDtProductViewCtl.ascx"
    TagName="DrugMSCtl" TagPrefix="uc1" %>
<script type="text/javascript" id="dxss_patiententryctl">
    $('.chkSelectAll input').change(function () {
        var isChecked = $(this).is(":checked");
        $('.chkIsSelected').each(function () {
            $(this).find('input').prop('checked', isChecked);
        });
//        calculateTotal();
    });

    $(function () {
        var param = "<%=ResolveUrl("~/Libs/Images/Icon/tbrecalculate.png")%>";
        var param = '<li id="btnRecalculate"><img src="'+ param + '" alt="" /><div>Recalculate</div></li>';
        addToolbarButton(param);
    });

    $('#btnRecalculate').live('click', function () {
        if (confirm('Are You Sure?')) {
            var param = '';
            $('.chkIsSelected input:checked').each(function () {
                $tr = $(this).closest('tr');
                if (param != '')
                    param += '|';
                param += $tr.find('.hdnKeyField').val();
            });
            $('#<%=hdnParam.ClientID %>').val(param);
            cbpRecalculationPatientBillProcess.PerformCallback();
        }
    });

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

    function onBeforeSaveRecord(errMessage) {
        var param = '';
        $('.chkIsSelectedCtl input:checked').each(function () {
            var $td = $(this).parent().parent();
            var input = $td.find('.hdnKeyField').val();
            if (param != "") {
                param += "|";
            }
            param += input;
        });
        if (param != "") {
            $('#<%=hdnParam.ClientID %>').val(param);
            return true;
        } 
        else {
            errMessage.text = 'Pilih Item Terlebih Dahulu';
            return false;
        }
    }
</script>
<style type="text/css">
    .containerRecalculationProcess                   { height: 280px;overflow-y:auto; border: 1px solid #EAEAEA; }
</style>

<input type="hidden" value="" id="hdnParam" runat="server" />
<div>
    <input type="hidden" runat="server" id="hdnRegistrationID" value="" />
    <input type="hidden" runat="server" id="hdnLinkedRegistrationID" value="" />
    <div style="text-align: left; width: 100%; margin-bottom: 10px;">
        <table>
            <colgroup>
                <col style="width: 50%" />
                <col />
            </colgroup>
            <tr>
                <td valign="top" style="width:80%">
                    <table>
                        <colgroup>
                            <col style="width: 250px" />
                        </colgroup>
                        <tr>
                            <td>
                                <%=GetLabel("Include Variable Tariff")%>
                            </td>
                            <td>
                                <asp:CheckBox ID="chkIsIncludeVariableTariff" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <%=GetLabel("Hitung Ulang Berdasarkan Buku Tarif")%>
                            </td>
                            <td>
                                <asp:CheckBox ID="chkIsResetItemTariff" runat="server" />
                            </td>
                        </tr>
                    </table>
                </td>
                <td valign="top" style="width: 100%" align="right">
                    <div id="divWarningPendingSave" runat="server">
                        <table>
                            <tr>
                                <td align="center" style="vertical-align:middle"><img height="40" src='<%= ResolveUrl("~/Libs/Images/warning.png")%>' alt='' /></td>
                                <td align="right" style="vertical-align:middle"><label class="lblWarning"><%=GetLabel("DON'T FORGET TO SAVE AFTER RECALCULATE")%></label></td>
                            </tr>
                        </table>
                    </div>
                </td>
            </tr>
        </table>
        <%--<table>
            <tr>
                <td>
                    <input type="button" id="btnRecalculateOk" value='<%= GetLabel("Recalculate")%>' />
                </td>
                <td>
                    <input type="button" id="btnRecalculateClose" value='<%= GetLabel("Close")%>' onclick="pcRightPanelContent.Hide();" />
                </td>
            </tr>
        </table>--%>
    </div>
    <dxcp:ASPxCallbackPanel ID="cbpRecalculationPatientBillProcess" runat="server" Width="1200px"
        ClientInstanceName="cbpRecalculationPatientBillProcess" ShowLoadingPanel="false"
        OnCallback="cbpRecalculationPatientBillProcess_Callback">
        <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ onCbpRecalculationPatientBillProcessEndCallback(s); }" />
        <PanelCollection>
            <dx:PanelContent ID="PanelContent1" runat="server" s>
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
                    <uc1:ServiceCtl ID="ctlLaboratory" runat="server" />
                </div>
                <div id="containerImaging" style="display: none" class="containerRecalculationProcess">
                    <uc1:ServiceCtl ID="ctlImaging" runat="server" />
                </div>
                <div id="containerMedicalDiagnostic" style="display: none" class="containerRecalculationProcess">
                    <uc1:ServiceCtl ID="ctlMedicalDiagnostic" runat="server" />
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
