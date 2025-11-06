<%@ Page Language="C#" MasterPageFile="~/Libs/MasterPage/PatientPage/MPBasePatientPageTrx5.master"
    AutoEventWireup="true" CodeBehind="NursingProcessEntry1.aspx.cs" Inherits="QIS.Medinfras.Web.CommonLibs.Program.NursingProcessEntry1" %>

<%@ Register Src="~/Libs/Program/Module/PatientManagement/TransactionPage/Nursing/Assessment/Process/NursingTransactionEntryDiagnosisItemCtl.ascx"
    TagName="DiagnoseItemCtl" TagPrefix="uc1" %>
<%@ Register Src="~/Libs/Program/Module/PatientManagement/TransactionPage/Nursing/Assessment/Process/NursingTransactionEntryInterventionCtl.ascx"
    TagName="InterventionCtl" TagPrefix="uc1" %>
<%@ Register Src="~/Libs/Program/Module/PatientManagement/TransactionPage/Nursing/Assessment/Process/NursingTransactionEntryOutcomeCtl.ascx"
    TagName="OutcomeCtl" TagPrefix="uc1" %>
<%@ Register Src="~/Libs/Program/Module/PatientManagement/TransactionPage/Nursing/Assessment/Process/NursingTransactionEntryImplementationCtl.ascx"
    TagName="ImplementationCtl" TagPrefix="uc1" %>
<%@ Register Src="~/Libs/Program/Module/PatientManagement/TransactionPage/Nursing/Assessment/Process/NursingTransactionEntryEvaluationCtl.ascx"
    TagName="EvaluationCtl" TagPrefix="uc1" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Src="~/Libs/Program/Module/PatientManagement/TransactionPage/TransactionPageToolbarCtl.ascx"
    TagName="ToolbarCtl" TagPrefix="uc1" %>
<asp:Content ID="Content3" ContentPlaceHolderID="plhMPPatientPageNavigationPane"
    runat="server">
    <uc1:ToolbarCtl ID="ctlToolbar" runat="server" />
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div class="menuTitle">
        <%=HttpUtility.HtmlEncode(GetPageTitle())%></div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="plhHeader" runat="server">
    <input type="hidden" value="" id="hdnGCRegistrationStatus" runat="server" />
    <input type="hidden" value="" id="hdnDepartmentID" runat="server" />
    <input type="hidden" value="" id="hdnRegistrationID" runat="server" />
    <input type="hidden" value="" id="hdnVisitID" runat="server" />
    <input type="hidden" value="" id="hdnUserID" runat="server" />
    <input type="hidden" value="" id="hdnBusinessPartnerID" runat="server" />
    <input type="hidden" value="" id="hdnKdGudang" runat="server" />
    <input type="hidden" value="" id="hdnLoadFirstRecord" runat="server" />
    <input type="hidden" value="" id="hdnDefaultLocation" runat="server" />
    <input type="hidden" value="" id="hdnHealthcareServiceUnitID" runat="server" />
    <input type="hidden" value="" id="hdnDefaultParamedicID" runat="server" />
    <input type="hidden" value="" id="hdnDefaultParamedicCode" runat="server" />
    <input type="hidden" value="" id="hdnDefaultParamedicName" runat="server" />
    <input type="hidden" value="" id="hdnDefaultChargeClassID" runat="server" />
    <input type="hidden" id="hdnPageTitle" runat="server" />
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript">
        function onLoad() {
            setDatePicker('<%=txtTransactionDate.ClientID %>');
            $('#ulTabWorkList li').click(function () {
                $('#ulTabWorkList li.selected').removeAttr('class');
                $('.containerTransaction').filter(':visible').hide();
                $contentID = $(this).attr('contentid');
                $('#<%=hdnContainerActive.ClientID %>').val($contentID);
                $('#' + $contentID).show();
                $(this).addClass('selected');
                if (typeof onRefreshParentGrid == 'function')
                    onRefreshParentGrid();
            });

            //#region Transaction No
            function onGetTransactionNoFilterExpression() {
                var filterExpression = "<%:GetTransactionFilterExpression() %>";
                return filterExpression;
            }

            $('#lblTransactionNo.lblLink').click(function () {
                openSearchDialog('nursingTransaction', onGetTransactionNoFilterExpression(), function (value) {
                    $('#<%=txtTransactionNo.ClientID %>').val(value);
                    onTxtTransactionNoChanged(value);
                });
            });

            $('#<%=txtTransactionNo.ClientID %>').change(function () {
                onTxtTransactionNoChanged($(this).val());
            });

            function onTxtTransactionNoChanged(value) {
                onLoadObject(value);
            }
            //#endregion

            //#region Patient Problem

            $('#<%=lblNursingProblem.ClientID %>.lblLink').live('click', function () {
                var filterExpression = "VisitID = " + $('#<%=hdnVisitID.ClientID %>').val() + " AND IsDeleted = 0";
                openSearchDialog('nursingPatientProblem', filterExpression, function (value) {
                    $('#<%=txtProblemCode.ClientID %>').val(value);
                    onTxtProblemCodeChanged(value);
                });
            });


            $('#<%=txtProblemCode.ClientID %>').change(function () {
                onTxtProblemCodeChanged($(this).val());
            });

            function onTxtProblemCodeChanged(value) {
                var filterExpression = " ProblemCode = '" + value + "' AND VisitID = " + $('#<%=hdnVisitID.ClientID %>').val() + " AND IsDeleted = 0";
                Methods.getObject('GetvNursingPatientProblemList', filterExpression, function (result) {
                    if (result != null) {
                        $('#<%=hdnNursingPatientProblemID.ClientID %>').val(result.ID);
                        $('#<%=txtProblemName.ClientID %>').val(result.ProblemName);
                    }
                    else {
                        $('#<%=hdnNursingPatientProblemID.ClientID %>').val('0');
                        $('#<%=txtProblemCode.ClientID %>').val('');
                        $('#<%=txtProblemName.ClientID %>').val('');
                    }
                });
            }

            //#endregion

            //#region Diagnose
            function onGetDiagnoseFilterExpression() {
                var filterExpression = "IsDeleted = 0";
                if ($('#<%=txtProblemCode.ClientID %>').val() != '') {
                    var filterExpression = filterExpression + " AND NurseDiagnoseID IN (SELECT NurseDiagnoseID FROM vNursingProblemDiagnose WITH (NOLOCK) WHERE ProblemCode = '" + $('#<%=txtProblemCode.ClientID %>').val() + "')";
                }
                return filterExpression;
            }


            $('#<%=lblNursingDiagnose.ClientID %>.lblLink').live('click', function () {
                openSearchDialog('nursingDiagnose', onGetDiagnoseFilterExpression(), function (value) {
                    $('#<%=txtDiagnoseCode.ClientID %>').val(value);
                    onTxtDiagnoseCodeChanged(value);
                });
            });


            $('#<%=txtDiagnoseCode.ClientID %>').change(function () {
                onTxtDiagnoseCodeChanged($(this).val());
            });

            function onTxtDiagnoseCodeChanged(value) {
                var filterExpression = onGetDiagnoseFilterExpression() + " AND NurseDiagnoseCode = '" + value + "'";
                Methods.getObject('GetNursingDiagnoseList', filterExpression, function (result) {
                    if (result != null) {
                        $('#<%=hdnNursingDiagnoseID.ClientID %>').val(result.NurseDiagnoseID);
                        $('#<%=txtDiagnoseName.ClientID %>').val(result.NurseDiagnoseName);
                    }
                    else {
                        $('#<%=hdnNursingDiagnoseID.ClientID %>').val('0');
                        $('#<%=txtDiagnoseCode.ClientID %>').val('');
                        $('#<%=txtDiagnoseName.ClientID %>').val('');
                    }
                });
                resetCheckedDiagnoseItem();
                resetCheckedOutcomeItem();
                resetCheckedInterventionItem();
                resetCheckedOutcomeEvaluationItem();
                cbpView.PerformCallback('refresh');
            }

            function onGetNursingDiagnoseID() {
                return $('#<%=hdnNursingDiagnoseID.ClientID %>').val(); ;
            }

            //#endregion

            //#region Import Transaction No

            $('#lblImportTransactionNo.lblLink').click(function () {
                openSearchDialog('nursingTransaction', onGetTransactionNoFilterExpression(), function (value) {
                    $('#<%=txtTemplateTransactionNo.ClientID %>').val(value);
                    onTxtTemplateTransactionNoChanged(value);
                });
            });

            $('#<%=txtTemplateTransactionNo.ClientID %>').change(function () {
                onTxtTemplateTransactionNoChanged($(this).val());
            });

            function onTxtTemplateTransactionNoChanged(value) {
                var filterExpression = onGetTransactionNoFilterExpression() + " AND TransactionNo = '" + value + "'";
                Methods.getObject('GetvNursingTransactionHdList', filterExpression, function (result) {
                    if (result != null) {
                        $('#<%=hdnTemplateTransactionID.ClientID %>').val(result.TransactionID);
                    }
                    else {
                        $('#<%=hdnTemplateTransactionID.ClientID %>').val('0');
                        $('#<%=txtTemplateTransactionNo.ClientID %>').val('');
                    }
                });
            }
            //#endregion

            $('#<%=txtTemplateTransactionNo.ClientID %>').change(function () {
                onTxtTemplateTransactionNoChanged($(this).val());
            });
        }

        function setRightPanelButtonEnabled() {
            var status = $('#<%=hdnTransactionStatus.ClientID %>').val();
            if ($('#<%=hdnTransactionID.ClientID %>').val() != '0') {
                if (status != Constant.TransactionStatus.OPEN) {
                    $('#btngenerateCPPT').attr('enabled', 'false');
                }
                else {
                    $('#btngenerateCPPT').removeAttr('enabled');
                }
            }
            else {
                $('#btngenerateCPPT').removeAttr('enabled');
            }
        }

        function onBeforeSaveRecord(errMessage) {
            getCheckedDiagnoseItem(errMessage);
            getCheckedOutcomeItem(errMessage);
            getCheckedInterventionItem(errMessage);
            getCheckedOutcomeEvaluationItem(errMessage);
            return true;
        }

        function onAfterSaveRecordDtSuccess(TransactionID) {
            if ($('#<%=hdnTransactionID.ClientID %>').val() == '0') {
                $('#<%=hdnTransactionID.ClientID %>').val(TransactionID);
                var filterExpression = 'TransactionID = ' + TransactionID;
                Methods.getObject('GetNursingTransactionHdList', filterExpression, function (result) {
                    $('#<%=txtTransactionNo.ClientID %>').val(result.TransactionNo);
                });
                onAfterCustomSaveSuccess();
            }
        }

        function onAfterProcessPopupEntry(param) {
            if (param != "") {
                onLoadObject(param);
            }
        }

        function onBeforeRightPanelPrint(code, filterExpression, errMessage) {
            var transactionID = $('#<%=hdnTransactionID.ClientID %>').val();
            var printStatus = $('#<%=hdnPrintStatus.ClientID %>').val();
            if (transactionID == '' || transactionID == '0') {
                errMessage.text = 'Nomor Asuhan Keperawatan harus dipilih untuk dicetak';
                return false;
            }
            else {
                var status = $('#<%=hdnTransactionStatus.ClientID %>').val();
                if (status != Constant.TransactionStatus.VOID) {
                    filterExpression.text = "TransactionID = " + transactionID;
                    return true;
                }
                else {
                    errMessage.text = 'Nomor Asuhan Keperawatan sudah dibatalkan';
                    return false;
                }
            }
        }

        function onValidateBeforeLoadRightPanelContent(code, errMessage) {
            if (code == 'calculateMayorMinor') {
                var transID = $('#<%:hdnTransactionID.ClientID %>').val();
                if (transID != '' && transID != '0') {
                    return true;
                }
                else {
                    errMessage.text = "Harap pilih no asuhan terlebih dahulu.";
                    return false;
                }
            }
            else {
                return true;
            }
        }

        function onBeforeLoadRightPanelContent(code) {
            if (code == 'generateCPPT') {
                var param = '0' + '|' + $('#<%:hdnTransactionID.ClientID %>').val();
                return param;
            }
            else if (code == 'calculateMayorMinor') {
                var param = $('#<%:hdnTransactionID.ClientID %>').val() + '|' + $('#<%=txtTransactionNo.ClientID %>').val() + '|' + $('#<%=hdnNursingDiagnoseID.ClientID %>').val();
                return param;
            }
            else {
                var param = '0' + '|' + $('#<%:hdnTransactionID.ClientID %>').val();
                return param;
            }
        }

        //#region Import Transaction
        $('.btnImportTransaction').die('click');
        $('.btnImportTransaction').live('click', function () {
            if ($('#<%=hdnTransactionID.ClientID %>').val() != '' && $('#<%=hdnTransactionID.ClientID %>').val() == '0') {
                cbpImportProcess.PerformCallback('import');
                $('#<%=hdnTemplateTransactionID.ClientID %>').val('');
                $('#<%=txtTemplateTransactionNo.ClientID %>').val('');
           
            }
        });
        //#endregion

        //#region Percentage Mayor Minor
        $('.imgPercentage').die('click');
        $('.imgPercentage').live('click', function () {
            if ($('#<%=hdnTransactionID.ClientID %>').val() != '' && $('#<%=hdnTransactionID.ClientID %>').val() != '0') {
                var url = ResolveUrl('~/Libs/Program/Module/PatientManagement/TransactionPage/Nursing/Assessment/Process/MayorMinorInfoCtl.ascx');
                var id = $('#<%:hdnTransactionID.ClientID %>').val() + '|' + $('#<%=txtTransactionNo.ClientID %>').val() + '|' + $('#<%=hdnNursingDiagnoseID.ClientID %>').val();
                openUserControlPopup(url, id, 'Informasi Persentase Mayor Minor', 300, 200);
            }
            else {
                showToast('Warning', "Harap Pilih No. Asuhan Terlebih Dahulu.");
            }
        });
        //#endregion

        function onCbpImportProcessEndCallback(s) {
            if (s.cpResult == "failed") {
                hideLoadingPanel();
                showToast('Warning', "Import failed");
            }
            else {
                var result = s.cpResult.split('|');
                onLoadObject(result[1]);
            }
        }

        function onGetTransactionNursingDiagnoseID() {
            return $('#<%=hdnNursingDiagnoseID.ClientID %>').val();
        }

        function onGetTransactionNumber() {
            return $('#<%=txtTransactionNo.ClientID %>').val();
        }

        function onLoadCurrentRecord() {
            onLoadObject($('#<%=txtTransactionNo.ClientID %>').val());
        }
    </script>
    <input type="hidden" value="" id="hdnTransactionID" runat="server" />
    <input type="hidden" value="" id="hdnWatermarkText" runat="server" />
    <input type="hidden" value="" id="hdnPrintStatus" runat="server" />
    <input type="hidden" value="" id="hdnTransactionStatus" runat="server" />
    <input type="hidden" value="" id="hdnParamedicID" runat="server" />
    <input type="hidden" value="" id="hdnLinkedRegistrationID" runat="server" />
    <input type="hidden" value="" id="hdnContainerActive" runat="server" />
    <div style="height: 435px; overflow-y: auto;">
        <table class="tblContentArea">
            <colgroup>
                <col width="50%" />
                <col width="50%" />
            </colgroup>
            <tr>
                <td style="padding: 5px; vertical-align: top">
                    <table class="tblEntryContent" style="width: 100%">
                        <colgroup>
                            <col width="200px" />
                            <col />
                        </colgroup>
                        <tr>
                            <td class="tdLabel">
                                <label id="lblTransactionNo" class="lblLink">
                                    <%=GetLabel("No. Asuhan")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtTransactionNo" Width="232px" runat="server" CssClass="txtTransactionNo" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <%=GetLabel("Tanggal") %>
                                /
                                <%=GetLabel("Waktu") %>
                                Asuhan
                            </td>
                            <td>
                                <table cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td style="padding-right: 1px; width: 145px">
                                            <asp:TextBox ID="txtTransactionDate" Width="120px" CssClass="datepicker" runat="server" />
                                        </td>
                                        <td style="width: 5px">
                                            &nbsp;
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtTransactionTime" Width="80px" CssClass="time" runat="server"
                                                Style="text-align: center" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblMandatory">
                                    <%=GetLabel("Perawat")%></label>
                            </td>
                            <td>
                                <dxe:ASPxComboBox ID="cboParamedicID" Width="300px" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <%=GetLabel("Diagnosa Medis") %>
                            </td>
                            <td>
                                <asp:TextBox ID="txtDiagnoseText" Width="400px" runat="server" />
                            </td>
                        </tr>
                    </table>
                </td>
                <td style="padding: 5px; vertical-align: top">
                    <table class="tblEntryContent" style="width: 100%">
                        <colgroup>
                            <col width="200px" />
                            <col />
                        </colgroup>
                        <tr>
                            <td class="tdLabel">
                                <label id="lblImportTransactionNo" class="lblLink">
                                    <%=GetLabel("Template Asuhan")%></label>
                            </td>
                            <td>
                                <table cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td class="tdLabel">
                                            <asp:TextBox ID="txtTemplateTransactionNo" Width="150px" runat="server" />
                                            <input type="hidden" value="" id="hdnTemplateTransactionID" runat="server" />
                                        </td>
                                        <td style="padding-left: 5px">
                                            <input type="button" value="Import" id="btnImportTransaction" class="btnImportTransaction w3-btn w3-hover-blue" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblLink" id="lblNursingProblem" runat="server">
                                    <%=GetLabel("Masalah Keperawatan")%></label>
                            </td>
                            <td>
                                <input type="hidden" value="" runat="server" id="hdnNursingPatientProblemID" />
                                <table style="width: 100%" cellpadding="0" cellspacing="0">
                                    <colgroup>
                                        <col style="width: 20%" />
                                        <col style="width: 5px" />
                                        <col />
                                    </colgroup>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtProblemCode" Width="100%" runat="server" />
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtProblemName" Width="87%" runat="server" />
                                            <img src='<%=ResolveUrl("~/Libs/Images/Button/Percentage_Icon.png")%>' id="imgPercentage"
                                                class="imgPercentage" alt="" width="23" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblLink" id="lblNursingDiagnose" runat="server">
                                    <%=GetLabel("Diagnosa Keperawatan")%></label>
                            </td>
                            <td>
                                <input type="hidden" value="" runat="server" id="hdnNursingDiagnoseID" />
                                <table style="width: 100%" cellpadding="0" cellspacing="0">
                                    <colgroup>
                                        <col style="width: 20%" />
                                        <col style="width: 5px" />
                                        <col />
                                    </colgroup>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtDiagnoseCode" Width="100%" runat="server" />
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtDiagnoseName" Width="87%" runat="server" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <table class="tblContentArea" style="display: none">
                                <colgroup>
                                    <col width="50%" />
                                    <col width="50%" />
                                </colgroup>
                                <tr>
                                    <td colspan="2">
                                        <h4 style="text-align: center">
                                            <%=GetLabel("Persentase Data Mayor")%></h4>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <div style="background-color: #EAEAEA; text-align: center; width: 100%">
                                            <b>
                                                <%=GetLabel("Subjective")%></b>
                                        </div>
                                        <div runat="server" id="divSubjectiveMayor" style="text-align: center; font-weight: bold;" />
                                    </td>
                                    <td>
                                        <div style="background-color: #EAEAEA; text-align: center; width: 100%">
                                            <b>
                                                <%=GetLabel("Objective")%></b>
                                        </div>
                                        <div runat="server" id="divObjectiveMayor" style="text-align: center; font-weight: bold;" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <h4 style="text-align: center">
                                            <%=GetLabel("Persentase Data Minor")%></h4>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <div style="background-color: #EAEAEA; text-align: center; width: 100%">
                                            <b>
                                                <%=GetLabel("Subjective")%></b>
                                        </div>
                                        <div runat="server" id="divSubjectiveMinor" style="text-align: center; font-weight: bold;" />
                                    </td>
                                    <td>
                                        <div style="background-color: #EAEAEA; text-align: center; width: 100%">
                                            <b>
                                                <%=GetLabel("Objective")%></b>
                                        </div>
                                        <div runat="server" id="divObjectiveMinor" style="text-align: center; font-weight: bold;" />
                                    </td>
                                </tr>
                            </table>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <div class="containerUlTabPage">
                        <ul class="ulTabPage" id="ulTabWorkList">
                            <li class="selected" contentid="containerDiagnoseItem">
                                <%=GetLabel("DIAGNOSA KEPERAWATAN")%></li>
                            <li contentid="containerOutcome">
                                <%=GetLabel("LUARAN")%></li>
                            <li contentid="containerIntervention">
                                <%=GetLabel("INTERVENSI")%></li>
                            <li contentid="containerImplementation">
                                <%=GetLabel("IMPLEMENTASI")%></li>
                            <li contentid="ctlEvaluation">
                                <%=GetLabel("EVALUASI")%></li>
                        </ul>
                    </div>
                    <div style="padding: 2px;" id="containerDiagnoseItem" class="containerTransaction">
                        <uc1:DiagnoseItemCtl ID="ctlDiagnoseItem" runat="server" />
                    </div>
                    <div style="padding: 2px; display: none" id="containerOutcome" class="containerTransaction">
                        <uc1:OutcomeCtl ID="ctlOutcome" runat="server" />
                    </div>
                    <div style="padding: 2px; display: none" id="containerIntervention" class="containerTransaction">
                        <uc1:InterventionCtl ID="ctlIntervention" runat="server" />
                    </div>
                    <div style="padding: 2px; display: none" id="containerImplementation" class="containerTransaction">
                        <uc1:ImplementationCtl ID="ctlImplementation" runat="server" />
                    </div>
                    <div style="padding: 2px; display: none" id="ctlEvaluation" class="containerTransaction">
                        <uc1:EvaluationCtl ID="ctlEvaluation" runat="server" />
                    </div>
                </td>
            </tr>
        </table>
        <div class="imgLoadingGrdView" id="containerImgLoadingView">
            <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
        </div>
        <dxcp:ASPxCallbackPanel ID="cbpImportProcess" runat="server" Width="100%" ClientInstanceName="cbpImportProcess"
            ShowLoadingPanel="false" OnCallback="cbpImportProcess_Callback">
            <ClientSideEvents BeginCallback="function(s,e) { showLoadingPanel(); }" EndCallback="function(s,e) { onCbpImportProcessEndCallback(s); }" />
        </dxcp:ASPxCallbackPanel>
    </div>
</asp:Content>
