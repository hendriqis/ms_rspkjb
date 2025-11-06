<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PPRAAlertNotificationCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.EMR.Program.PPRAAlertNotificationCtl" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
    <%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<style type="text/css">       
    .highlight    {  background-color:#FE5D15; color: White; }
    
    .druglist { font-weight: bold;}
</style>
<script type="text/javascript" id="dxss_patiententryctl">
    $(function () {

        if ($('#<%=hdnFormValues.ClientID %>').val() != '') {
            var oldData = $('#<%=hdnFormValues.ClientID %>').val().split(';');
            for (var i = 0; i < oldData.length; ++i) {
                var temp = oldData[i].split('=');
                $('#<%=divFormContent.ClientID %>').find('.ddlForm').each(function () {
                    if ($(this).attr('controlID') == temp[0])
                        $(this).val(temp[1]);
                });
                $('#<%=divFormContent.ClientID %>').find('.chkForm').each(function () {
                    if ($(this).attr('controlID') == temp[0] && temp[1] == "1") {
                        $(this).prop('checked', true);
                    }
                });
                $('#<%=divFormContent.ClientID %>').find('.chkNursingProblem').each(function () {
                    if ($(this).attr('controlID') == temp[0] && temp[1] == "1") {
                        $(this).prop('checked', true);
                    }
                });
                $('#<%=divFormContent.ClientID %>').find('.optForm').each(function () {
                    if ($(this).attr('controlID') == temp[0] && temp[1] == "1") {
                        $(this).prop('checked', true);
                    }
                });
                $('#<%=divFormContent.ClientID %>').find('.txtForm').each(function () {
                    if ($(this).attr('controlID') == temp[0])
                        $(this).val(temp[1]);
                });
            }
        }

        $('#<%=rblPPRAMicrobiologyTestStatus.ClientID %> input').die('change');
        $('#<%=rblPPRAMicrobiologyTestStatus.ClientID %> input').live('change', function () {
            if ($(this).val() != "0" && $(this).val() != "") {
                $('#<%=trMicrobiologyTestOrder1.ClientID %>').removeAttr("style");
                $('#<%=trMicrobiologyTestOrder2.ClientID %>').removeAttr("style");
                $('#<%=hdnTestOrderID.ClientID %>').val("0");
                $('#<%=txtTestOrderNo.ClientID %>').val("");
                $('#<%=txtTestOrderInfo.ClientID %>').val("");
            }
            else {
                $('#<%=trMicrobiologyTestOrder1.ClientID %>').attr("style", "display:none");
                $('#<%=trMicrobiologyTestOrder2.ClientID %>').attr("style", "display:none");
                $('#<%=hdnTestOrderID.ClientID %>').val("0");
                $('#<%=txtTestOrderNo.ClientID %>').val("");
                $('#<%=txtTestOrderInfo.ClientID %>').val("");
            }
        });
    });

    //#region Left Navigation Panel
    $('#leftPageNavPanel ul li').click(function () {
        $('#leftPageNavPanel ul li.selected').removeClass('selected');
        $(this).addClass('selected');
        var contentID = $(this).attr('contentID');

        showContent(contentID);
    });

    function showContent(contentID) {
        var i, x, tablinks;
        x = document.getElementsByClassName("divPageNavPanelContent");
        for (i = 0; i < x.length; i++) {
            x[i].style.display = "none";
        }
        document.getElementById(contentID).style.display = "block";
    }
    //#endregion

    $('#lblCopyFromSOAP').die('click');
    $('#lblCopyFromSOAP').live('click', function (evt) {
        var filterExpression = "VisitID = " + $('#<%=hdnPopupVisitID.ClientID %>').val() + " AND ParamedicID = " + $('#<%=hdnPopupParamedicID.ClientID %>').val() + " AND GCPatientNoteType IN ('X011^011','X011^002','X011^004') AND SubjectiveText IS NOT NULL";
        openSearchDialog('planningNote', filterExpression, function (value) {
            $('#<%=hdnPopupVisitNoteID.ClientID %>').val(value);
            onCopyFromSOAP(value);
        });
    });

    function onCopyFromSOAP(value) {
        var filterExpression = "ID = " + value;
        Methods.getObject('GetPatientVisitNoteList', filterExpression, function (result) {
            if (result != null) {
                $('#<%=hdnPopupVisitNoteID.ClientID %>').val(result.ID);
                $('#<%=txtPPRASubjectiveSummary.ClientID %>').val(result.SubjectiveText);
                $('#<%=txtPPRAObjectiveSummary.ClientID %>').val(result.ObjectiveText);
                $('#<%=txtPPRAAssessmentSummary.ClientID %>').val(result.AssessmentText);
                $('#<%=txtPPRAPlanningSummary.ClientID %>').val(result.PlanningText);
            }
            else {
                $('#<%=hdnPopupVisitNoteID.ClientID %>').val('0');
                $('#<%=txtPPRASubjectiveSummary.ClientID %>').val('');
                $('#<%=txtPPRAObjectiveSummary.ClientID %>').val('');
                $('#<%=txtPPRAAssessmentSummary.ClientID %>').val('');
                $('#<%=txtPPRAPlanningSummary.ClientID %>').val('');
            }
        });
    }

    $('#lblTestOrderNo.lblLink').live('click', function () {
        var filterExpression = "VisitID = " + $('#<%=hdnPopupVisitID.ClientID %>').val() + " AND IsLaboratoryUnit = 1";
        openSearchDialog('testorderhd', filterExpression, function (value) {
            $('#<%=txtTestOrderNo.ClientID %>').val(value);
            onTxtTestOrderNoChanged(value);
        });
    });

    $('#<%=txtTestOrderNo.ClientID %>').change(function () {
        onTxtTestOrderNoChanged($(this).val());
    });

    function onTxtTestOrderNoChanged(value) {
        var filterExpression = "TestOrderNo = '" + value + "'";
        Methods.getObject('GetTestOrderHdList', filterExpression, function (result) {
            if (result != null) {
                $('#<%=hdnTestOrderID.ClientID %>').val(result.TestOrderID);
            }
            else {
                $('#<%=hdnTestOrderID.ClientID %>').val('0');
            }
        });
    }

    function onCbpPopupProcesEndCallback(s) {
        hideLoadingPanel();
        var param = s.cpResult.split('|');
        var retval = s.cpRetval.split('|');
        if (param[0] == 'process') {
            if (param[1] == '0') {
                showToast("PROCESS : FAIL", "Error Message : <br/><span style='color:red'>" + param[2] + "</span>");
            }
            else {
                pcRightPanelContent.Hide();
            }
        }
    }

    function onCbpViewEndCallback(s) {
        hideLoadingPanel();
    }

    function onRefreshGrid() {
        cbpView.PerformCallback('refresh');
    }

    function onBeforeSaveRecord(errMessage) {
        return true;
    }

    function onAfterSaveAddRecordEntryPopup(result) {
        if (typeof onRefreshControl == 'function')
            onRefreshControl();
    }

    function onAfterSaveEditRecordEntryPopup(result) {
        if (typeof onRefreshControl == 'function')
            onRefreshControl();
    }

    function getFormValues() {
        var controlValues = '';
        $('#<%=divFormContent.ClientID %>').find('.ddlForm').each(function () {
            if (controlValues != '')
                controlValues += ";";
            controlValues += $(this).attr('controlID') + '=' + $(this).val();
        });
        $('#<%=divFormContent.ClientID %>').find('.txtForm').each(function () {
            if (controlValues != '')
                controlValues += ";";
            controlValues += $(this).attr('controlID') + '=' + $(this).val();
        });
        $('#<%=divFormContent.ClientID %>').find('.chkForm').each(function () {
            if (controlValues != '')
                controlValues += ";";
            if ($(this).is(':checked'))
                controlValues += $(this).attr('controlID') + '=1';
            else
                controlValues += $(this).attr('controlID') + '=0';
        });
        $('#<%=divFormContent.ClientID %>').find('.chkNursingProblem').each(function () {
            if (controlValues != '')
                controlValues += ";";
            if ($(this).is(':checked'))
                controlValues += $(this).attr('controlID') + '=1';
            else
                controlValues += $(this).attr('controlID') + '=0';
        });
        $('#<%=divFormContent.ClientID %>').find('.optForm').each(function () {
            if (controlValues != '')
                controlValues += ";";
            if ($(this).is(':checked'))
                controlValues += $(this).attr('controlID') + '=1';
            else
                controlValues += $(this).attr('controlID') + '=0';
        });
        $('#<%=hdnFormValues.ClientID %>').val(controlValues);

        return controlValues;
    }
    $('#leftPageNavPanel ul li').first().click();
</script>
<input type="hidden" runat="server" id="hdnSelectedID" value="" />
<input type="hidden" runat="server" id="hdnSelectedRemarks" value="" />
<input type="hidden" runat="server" id="hdnDispensaryServiceUnitID" value="" />
<input type="hidden" runat="server" id="hdnReferenceNo" value="" />
<input type="hidden" runat="server" id="hdnTransactionNo" value="" />
<input type="hidden" runat="server" id="hdnPrescriptionOrderID" value="" />
<input type="hidden" runat="server" id="hdnTransactionDate" value="" />   
<input type="hidden" runat="server" id="hdnFormLayout" value="" />
<input type="hidden" runat="server" id="hdnFormValues" value="" />
<input type="hidden" runat="server" id="hdnPopupVisitID" value="" />
<input type="hidden" runat="server" id="hdnPopupParamedicID" value="" />
<input type="hidden" runat="server" id="hdnPopupVisitNoteID" value="" />
<input type="hidden" runat="server" id="hdnTestOrderID" value="" />

<div>
    <table class="tblContentArea">
        <colgroup>
            <col style="width: 20%" />
            <col style="width: 80%" />
        </colgroup>
        <tr>
            <td style="vertical-align: top">
                <div id="leftPageNavPanel" class="w3-border">
                    <ul>
                        <li contentID="divPage1" contentIndex="1" title="Data Pasien" class="w3-hover-red">Data Pasien</li>
                        <li contentID="divPage2" contentIndex="2" title="Formulir PPRA" class="w3-hover-red">Formulir PPRA</li>
                    </ul>
                </div>
            </td>
            <td style="padding:5px;vertical-align:top;">
                <div id="divPage1" class="w3-border divPageNavPanelContent w3-animate-left" style="display: none">
                   <table class="tblEntryContent" style="width:99%">
                        <colgroup>
                            <col style="width: 150px" />
                            <col style="width: 150px" />
                            <col style="width: 5px" />
                            <col style="width: 100px" />
                            <col style="width: 150px" />
                        </colgroup>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal" id="Label5">
                                    <%=GetLabel("No. Registrasi")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtRegistrationNo" Width="150px" ReadOnly="true" runat="server" />
                            </td>
                            <td />
                            <td class="tdLabel">
                                <label class="lblNormal" id="lblTransactionNo">
                                    <%=GetLabel("No. Order")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtPrescriptionOrderNo" Width="100%" ReadOnly="true" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal" id="Label1">
                                    <%=GetLabel("No. RM")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtMedicalNo" Width="150px" ReadOnly="true" runat="server" />
                            </td>
                            <td />
                            <td class="tdLabel">
                                <label class="lblNormal" id="Label6">
                                    <%=GetLabel("Tanggal Lahir")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtDateOfBirth" Width="100%" ReadOnly="true" runat="server" />
                            </td>
                        </tr>
                        <tr>
                             <td class="tdLabel">
                                <label class="lblNormal" id="Label2">
                                    <%=GetLabel("Pasien")%></label>
                            </td>
                            <td colspan="4">
                                <asp:TextBox ID="txtPatientName" Width="100%" ReadOnly="true" runat="server" />
                            </td>
                        </tr>
                        <tr style="display : none">
                             <td class="tdLabel">
                                <label class="lblNormal" id="Label3">
                                    <%=GetLabel("Lokasi Pasien")%></label>
                            </td>
                            <td colspan="4">
                                <asp:TextBox ID="txtPatientLocation" Width="100%" ReadOnly="true" runat="server" />
                            </td>
                        </tr>
                        <tr style="display:none">
                             <td class="tdLabel" style="vertical-align:top">
                                <label class="lblNormal" id="Label7">
                                    <%=GetLabel("Diagnosis Pasien")%></label>
                            </td>
                            <td colspan="4">
                                <asp:TextBox ID="txtPatientDiagnosis" Width="100%" ReadOnly="true" runat="server" TextMode="MultiLine" Rows="5" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel" colspan="5">
                                <label class="lblNormal" id="Label4">
                                    <%=GetLabel("Daftar Item yang termasuk Kelompok Antimikroba Pengendalian Khusus (Reserved)")%></label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="5">
                                <asp:TextBox ID="txtItemList" Width="100%" ReadOnly="true" runat="server" TextMode="MultiLine" Rows="7" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel" colspan="5">
                                <label class="lblMandatory">
                                    <%=GetLabel("Lanjutkan dengan Pengisian Formulir PPRA")%></label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="5">
                                <asp:RadioButtonList ID="rblIsHasPPRAForm" runat="server" RepeatDirection="Horizontal">
                                    <asp:ListItem Text=" Ya" Value="1" Selected />
                                    <asp:ListItem Text=" Tidak" Value="0"  />
                                    <asp:ListItem Text=" Dilakukan di luar sistem (Formulir Manual)" Value="2"  />                                               
                                </asp:RadioButtonList>
                            </td>
                        </tr>
                    </table>
                </div>
                <div id="divPage2" class="w3-border divPageNavPanelContent w3-animate-left" style="display: none">
                   <table class="tblEntryContent" style="width:99%">
                        <colgroup>
                            <col style="width: 150px" />
                            <col style="width: 150px" />
                            <col style="width: 5px" />
                            <col style="width: 100px" />
                            <col style="width: 150px" />
                        </colgroup>
                        <tr>
                            <td class="tdLabel" style="vertical-align:top">
                                <div><label class="lblMandatory">
                                    <%=GetLabel("Riwayat Penyakit")%></label> </div>
                                <div><span class="lblLink" id="lblCopyFromSOAP"><%= GetLabel("Salin dari SOAP")%></span></div>
                            </td>
                            <td colspan="4">
                                <asp:TextBox ID="txtPPRASubjectiveSummary" Width="100%" runat="server" TextMode="MultiLine" Rows="3" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel" style="vertical-align:top">
                                <label class="lblMandatory">
                                    <%=GetLabel("Kondisi Pasien")%></label>
                            </td>
                            <td colspan="4">
                                <asp:TextBox ID="txtPPRAObjectiveSummary" Width="100%" runat="server" TextMode="MultiLine" Rows="3" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel" style="vertical-align:top">
                                <label class="lblMandatory">
                                    <%=GetLabel("Diagnosa Pasien")%></label>
                            </td>
                            <td colspan="4">
                                <asp:TextBox ID="txtPPRAAssessmentSummary" Width="100%" runat="server" TextMode="MultiLine" Rows="3" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblMandatory">
                                    <%=GetLabel("Indikasi")%></label>
                            </td>
                            <td colspan="4">
                                <asp:RadioButtonList ID="rblPPRAIndication" runat="server" RepeatDirection="Horizontal">
                                    <asp:ListItem Text=" Profilaxis" Value="1"  />
                                    <asp:ListItem Text=" Empirik" Value="2"  />
                                    <asp:ListItem Text=" Definitif" Value="3"  />                                               
                                </asp:RadioButtonList>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel" style="vertical-align:top">
                                <label class="lblMandatory">
                                    <%=GetLabel("Alasan Permintaan")%></label>
                            </td>
                            <td colspan="4">
                                <asp:TextBox ID="txtPPRAReason" Width="100%" runat="server" TextMode="MultiLine" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblMandatory">
                                    <%=GetLabel("Pemeriksaan Kultur")%></label>
                            </td>
                            <td colspan="4">
                                <asp:RadioButtonList ID="rblPPRAMicrobiologyTestStatus" runat="server" RepeatDirection="Horizontal">
                                    <asp:ListItem Text=" Tidak Ada" Value="0"  />
                                    <asp:ListItem Text=" Ada" Value="1"  />                                           
                                </asp:RadioButtonList>
                            </td>
                        </tr>
                        <tr id="trMicrobiologyTestOrder1" runat="server">
                            <td class="tdLabel" style="vertical-align:top">
                                <label class="lblLink" id="lblTestOrderNo">
                                    <%=GetLabel("No. Order")%></label>
                            </td>
                            <td colspan="2">
                                <asp:TextBox ID="txtTestOrderNo" Width="100%" runat="server" ReadOnly="true" />
                            </td>
                            <td colspan="2" style="display:none">
                                <asp:TextBox ID="txtTestOrderInfo" Width="100%" runat="server" ReadOnly="true" />
                            </td>
                        </tr>
                        <tr id="trMicrobiologyTestOrder2" runat="server">
                            <td class="tdLabel" style="vertical-align:top">
                                <label class="lblMandatory">
                                    <%=GetLabel("Hasil Kultur")%></label>
                            </td>
                            <td colspan="4">
                                <asp:RadioButtonList ID="rblPPRAMicrobiologyTestResultStatus" runat="server" RepeatDirection="Horizontal">
                                    <asp:ListItem Text=" Belum Ada/Menunggu Hasil" Value="0"  />
                                    <asp:ListItem Text=" Hasil sudah ada" Value="1"  />                                           
                                </asp:RadioButtonList>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel" colspan="5">
                                <label class="lblMandatory">
                                    <%=GetLabel("Pemeriksaan Radiologi/Penunjang Lainnya yang berhubungan dengan infeksi")%></label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="5">
                                <asp:TextBox ID="txtPPRAPlanningSummary" Width="100%" runat="server" TextMode="MultiLine" Rows="3" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel" colspan="5">
                                <label class="lblNormal">
                                    <%=GetLabel("Catatan Lainnya (Perjalanan Penyakit/Temuan saat operasi) (Jika ada)")%></label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="5">
                                <asp:TextBox ID="txtPPRARemarks" Width="100%" runat="server" TextMode="MultiLine" Rows="3" />
                            </td>
                        </tr>
                        <tr style="display:none">
                            <td colspan="2">
                                <div id="divFormContent" runat="server" style="height: 450px; overflow-y: auto;">
                                </div>
                            </td>
                        </tr>
                    </table>
                </div>
            </td>
        </tr>
    </table>
</div>
