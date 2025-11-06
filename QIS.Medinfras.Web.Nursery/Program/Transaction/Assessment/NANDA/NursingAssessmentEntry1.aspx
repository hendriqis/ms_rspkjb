<%@ Page Language="C#" MasterPageFile="~/Libs/MasterPage/PatientPage/MPBasePatientPageTrx2.master" AutoEventWireup="true"
    CodeBehind="NursingAssessmentEntry1.aspx.cs" Inherits="QIS.Medinfras.Web.Nursing.Program.NursingAssessmentEntry1" %>

<%@ Register Src="~/Program/Transaction/NursingTransaction/NursingTransactionEntryDiagnosisItemCtl.ascx"
    TagName="DiagnoseItemCtl" TagPrefix="uc1" %>
<%@ Register Src="~/Program/Transaction/NursingTransaction/NursingTransactionEntryInterventionCtl.ascx"
    TagName="InterventionCtl" TagPrefix="uc1" %>
<%@ Register Src="~/Program/Transaction/NursingTransaction/NursingTransactionEntryOutcomeCtl.ascx"
    TagName="OutcomeCtl" TagPrefix="uc1" %>
<%@ Register Src="~/Program/Transaction/NursingTransaction/NursingTransactionEntryImplementationCtl.ascx"
    TagName="ImplementationCtl" TagPrefix="uc1" %>
<%@ Register Src="~/Program/Transaction/NursingTransaction/NursingTransactionEntryEvaluationCtl.ascx"
    TagName="EvaluationCtl" TagPrefix="uc1" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Src="~/Libs/Program/Module/PatientManagement/TransactionPage/TransactionPageToolbarCtl.ascx" TagName="ToolbarCtl" TagPrefix="uc1" %>

<asp:Content ID="Content3" ContentPlaceHolderID="plhMPPatientPageNavigationPane" runat="server">   
    <uc1:ToolbarCtl ID="ctlToolbar" runat="server" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="plhHeader" runat="server">
    <input type="hidden" value="" id="hdnGCRegistrationStatus" runat="server" />
    <input type="hidden" value="" id="hdnDepartmentID" runat="server" />
    <input type="hidden" value="" id="hdnRegistrationID" runat="server" />
    <input type="hidden" value="" id="hdnVisitID" runat="server" />
    <input type="hidden" value="" id="hdnBusinessPartnerID" runat="server" />
    <input type="hidden" value="" id="hdnKdGudang" runat="server" />
    <input type="hidden" value="" id="hdnDefaultLocation" runat="server" />
    <input type="hidden" value="" id="hdnHealthcareServiceUnitID" runat="server" />
    <input type="hidden" value="" id="hdnDefaultParamedicID" runat="server" />
    <input type="hidden" value="" id="hdnDefaultParamedicCode" runat="server" />
    <input type="hidden" value="" id="hdnDefaultParamedicName" runat="server" />
    <input type="hidden" value="" id="hdnDefaultChargeClassID" runat="server" />
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript">
        function onLoad() {
            setDatePicker('<%=txtTransactionDate.ClientID %>');
            $('#ulTabWorkList li').click(function () {
                $('#ulTabWorkList li.selected').removeAttr('class');
                $('.containerTransaction').filter(':visible').hide();
                $contentID = $(this).attr('contentid');
                $('#' + $contentID).show();
                $(this).addClass('selected');
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

            //#region Diagnose
            function onGetDiagnoseFilterExpression() {
                var filterExpression = "<%:OnGetDiagnoseFilterExpression() %>";
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
                resetCheckedEvaluation();
                cbpView.PerformCallback('refresh');
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
                Methods.getObject('GetNursingTransactionHdList', filterExpression, function (result) {
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

        function onBeforeSaveRecord(errMessage) {
            getCheckedDiagnoseItem(errMessage);
            getCheckedOutcomeItem(errMessage);
            getCheckedInterventionItem(errMessage);
            getCheckedEvaluation(errMessage);
            return true;
        }

        function onAfterSaveRecordDtSuccess(TransactionID) {
            if ($('#<%=hdnTransactionID.ClientID %>').val() == '0') {
                $('#<%=hdnTransactionID.ClientID %>').val(TransactionID);
                var filterExpression = 'TransactionID = ' + TransactionID;  
                Methods.getObject('GetNursingTransactionHdList', filterExpression, function (result) {
                    $('#<%=txtTransactionNo.ClientID %>').val(result.TransactionNo);
                    //cbpView.PerformCallback('refresh');
                });
            }
            //else
            //cbpView.PerformCallback('refresh');
        }

        function onBeforeRightPanelPrint(code, filterExpression, errMessage) {
            var transactionID = $('#<%=hdnTransactionID.ClientID %>').val();
            var printStatus = $('#<%=hdnPrintStatus.ClientID %>').val();
            if (transactionID == '' || transactionID == '0') {
                errMessage.text = 'Please Set Transaction First!';
                return false;
            }
            else {
                filterExpression.text = "TransactionID = " + transactionID;
                return true;
            }
        }

        //#region Import Transaction
        $('.btnImportTransaction').die('click');
        $('.btnImportTransaction').live('click', function () {
            if ($('#<%=hdnTemplateTransactionID.ClientID %>').val() != '' && $('#<%=hdnTransactionID.ClientID %>').val() == '0') {
                cbpImportProcess.PerformCallback('import');
                $('#<%=hdnTemplateTransactionID.ClientID %>').val('');
                $('#<%=txtTemplateTransactionNo.ClientID %>').val('');
            }
        });
        //#endregion


        function onCbpImportProcessEndCallback(s) {
            if (s.cpResult == "failed") {
                showToast('Warning', "Import failed");
            }
            else {
                var result = s.cpResult.split('|');
                onLoadObject(result[1]);
            }
        }
        

    </script>
    <input type="hidden" value="" id="hdnTransactionID" runat="server" />
    <input type="hidden" value="" id="hdnWatermarkText" runat="server" />
    <input type="hidden" value="" id="hdnPrintStatus" runat="server" />
    <div style="height: 435px; overflow-y: auto;">
        <div class="pageTitle">
            <div style="font-size: 1.1em">
                <%=GetLabel("Asuhan Keperawatan")%></div>
        </div>
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
                                    <%=GetLabel("No. Transaksi")%></label>
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
                                    <%=GetLabel("Template No Transaksi")%></label>
                            </td>
                            <td>
                                <table cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td class="tdLabel">
                                            <asp:TextBox ID="txtTemplateTransactionNo" Width="150px" runat="server" />
                                            <input type="hidden" value="" id="hdnTemplateTransactionID" runat="server" />
                                        </td>
                                        <td style="padding-left :5px">
                                            <input type="button" value="Import" id="btnImportTransaction" class="btnImportTransaction" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                &nbsp
                            </td>
                            <td>
                                &nbsp
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblLink" id="lblNursingDiagnose" runat="server">
                                    <%=GetLabel("NANDA-I")%></label>
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
                                            <asp:TextBox ID="txtDiagnoseName" Width="100%" runat="server" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <div class="containerUlTabPage">
                        <ul class="ulTabPage" id="ulTabWorkList">
                            <li class="selected" contentid="containerDiagnoseItem">
                                <%=GetLabel("Diagnosa Keperawatan")%></li>
                            <li contentid="containerOutcome">
                                <%=GetLabel("Nursing Outcome Classification (NOC)")%></li>
                            <li contentid="containerIntervention">
                                <%=GetLabel("Nursing Intervention Classification (NIC)")%></li>
                            <li contentid="containerImplementation">
                                <%=GetLabel("Implementasi")%></li>
                            <li contentid="containerSOAP">
                                <%=GetLabel("Evaluasi")%></li>
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
                    <div style="padding: 2px; display: none" id="containerSOAP" class="containerTransaction">
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
