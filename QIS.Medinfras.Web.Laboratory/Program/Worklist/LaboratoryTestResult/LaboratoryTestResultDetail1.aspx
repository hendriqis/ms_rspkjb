<%@ Page Language="C#" MasterPageFile="~/Libs/MasterPage/MPTrx2.master" AutoEventWireup="true"
    CodeBehind="LaboratoryTestResultDetail1.aspx.cs" Inherits="QIS.Medinfras.Web.Laboratory.Program.LaboratoryTestResultDetail1" %>

<%@ Register Src="~/Libs/Controls/PatientBannerCtl.ascx" TagName="PatientBannerCtl"
    TagPrefix="uc1" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<asp:Content ID="Content4" ContentPlaceHolderID="plhCustomButtonToolbarLeft" runat="server">
    <li id="btnLaboratoryResultBack" runat="server" crudmode="R">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/back.png")%>' alt="" /><div>
            <%=GetLabel("Back")%></div>
    </li>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="plhCustomButtonToolbar" runat="server">
    <li id="btnVoidResult" runat="server" crudmode="R">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbdelete.png")%>' alt="" /><div>
            <%=GetLabel("Void Result")%></div>
    </li>
    <li id="btnReopenResult" runat="server" crudmode="R">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbreopen.png")%>' alt="" /><div>
            <%=GetLabel("Re-Open Result")%></div>
    </li>
    <li id="btnImportResult" runat="server" crudmode="R">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbprocess.png")%>' alt="" /><div>
            <%=GetLabel("Import Result")%></div>
    </li>
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
    <input type="hidden" value="" id="hdnClassID" runat="server" />
    <input type="hidden" value="" id="hdnBusinessPartnerID" runat="server" />
    <input type="hidden" value="" id="hdnKdGudang" runat="server" />
    <input type="hidden" value="" id="hdnDefaultLocation" runat="server" />
    <input type="hidden" value="" id="hdnHealthcareServiceUnitID" runat="server" />
    <input type="hidden" value="" id="hdnPhysicianID" runat="server" />
    <input type="hidden" value="" id="hdnPhysicianCode" runat="server" />
    <input type="hidden" value="" id="hdnPageTitle" runat="server" />
    <input type="hidden" value="" id="hdnResultGCTransactionStatus" runat="server" />
    <uc1:PatientBannerCtl ID="ctlPatientBanner" runat="server" />
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript">
        function getPhysicianID() {
            return $('#<%=hdnPhysicianID.ClientID %>').val();
        }
        function getBusinessPartnerID() {
            return $('#<%=hdnBusinessPartnerID.ClientID %>').val();
        }
        function getClassID() {
            return $('#<%=hdnClassID.ClientID %>').val();
        }
        function getRegistrationID() {
            return $('#<%=hdnRegistrationID.ClientID %>').val();
        }
        function getVisitID() {
            return $('#<%=hdnVisitID.ClientID %>').val();
        }

        $(function () {
            setRightPanelButtonEnabled();
        });

        function onLoad() {
            $('#ulTabLabResult li').click(function () {
                $('#ulTabLabResult li.selected').removeAttr('class');
                $('.containerDaftarDt').filter(':visible').hide();
                $contentID = $(this).attr('contentid');
                $('#' + $contentID).show();
                $(this).addClass('selected');
                if ($contentID == "containerHasil") {
                    cbpViewHasil.PerformCallback('refresh');
                }
                
            });

            setDatePicker('<%=txtResultDate.ClientID %>');

            if ($('#<%=hdnIsBridgingLIS.ClientID %>').val() == "1") {
                $('#<%=btnVoidResult.ClientID %>').hide();
            }

            if ($('#<%=hdnResultGCTransactionStatus.ClientID %>').val() != Constant.TransactionStatus.WAIT_FOR_APPROVAL) {
                $('#<%=btnReopenResult.ClientID %>').hide();
            }

            $('#<%=btnLaboratoryResultBack.ClientID %>').click(function () {
                showLoadingPanel();
                if ($('#<%=hdnType.ClientID %>').val() == 'to')
                    document.location = ResolveUrl('~/Program/Worklist/LaboratoryTestResult/LaboratoryTestResultList1.aspx');
                else
                    document.location = ResolveUrl('~/Program/Worklist/LaboratoryTestResult/LaboratoryTestResultHistoryList.aspx');
            });

            $('#<%=btnReopenResult.ClientID %>').click(function () {
                onCustomButtonClick('reopen_result');
            });

            $('#<%=btnVoidResult.ClientID %>').click(function () {
                showDeleteConfirmation(function (data) {
                    var param = 'delete_result;' + data.GCDeleteReason + ';' + data.Reason;
                    onCustomButtonClick(param);
                });
            });

            $('#<%=btnImportResult.ClientID %>').click(function () {
                selectedItemID = $(this).closest('tr').find('.keyField').html();
                var labResultID = $('#<%=hdnLabResultID.ClientID %>').val();
                var transactionID = $('#<%=hdnTransactionHdID.ClientID %>').val();
                var isResultExists = $('#<%=hdnIsResultExists.ClientID %>').val();

                var visitID = $('#<%=hdnVisitID.ClientID %>').val();
                var paramResultTest = visitID + "|" + selectedItemID + "|" + labResultID + "|" + transactionID;
                var url = ResolveUrl("~/Program/WorkList/LaboratoryTestResult/ImportLISResultCtl.ascx");

                openUserControlPopup(url, paramResultTest, 'Import Laboratory Result', 1200, 500);
            });

            if ($('#<%=hdnIsStatusOpen.ClientID %>').val() == '0') {
                $('#ulTabLabResult li:eq(1)').click();
                $('#ulTabLabResult li:eq(0)').hide();
                hideToolbar();
                showWatermark($('#<%=hdnWatermarkText.ClientID %>').val());
            }
        }

        $('.lblResultAttachment.lblLink').live('click', function (evt) {
            var resultAttachment = $('#<%=hdnResultAttachment.ClientID %>').val();
            if (resultAttachment != null && resultAttachment != "") {
                window.open("data:application/pdf;base64, " + resultAttachment, "popupWindow", "width=600, height=600,scrollbars=yes");
            } else {
                showToast('INFORMATION', 'No data to display.');
            }
        });

        $('.lblViewPDF.lblLink').live('click', function (evt) {
            $tr = $(this).closest('tr').parent().closest('tr');
            var textResultValue = $tr.find('.textResultValue').html();
            window.open("data:application/pdf;base64, " + textResultValue, "popupWindow", "width=600, height=600,scrollbars=yes");
        });

        function onAfterCustomClickSuccess(type, retval) {
            var callback = type.split(';');
            if (type == 'saveparamedic') {
                cbpView.PerformCallback('refresh');
                $('#btnCancel').trigger('click');
            }
            else if (callback[0] == 'delete_result') {
                $('#<%=btnLaboratoryResultBack.ClientID %>').click();
            }
            else if (type == 'reopen_result') {
                $('#<%=btnLaboratoryResultBack.ClientID %>').click();
            }
        }

        $('#btnSave').live('click', function (evt) {
            if (IsValid(evt, 'fsTrxPopup', 'mpTrxPopup'))
                onCustomButtonClick('saveparamedic');
        });

        $('#btnCancel').live('click', function () {
            $('#containerEntry').hide();
        });

        $('#<%=grdView.ClientID %> .imgEdit.imgLink').live('click', function () {
            $row = $(this).closest('tr').parent().closest('tr');
            var entity = rowToObject($row);
            $('#<%=hdnChargesDtID.ClientID %>').val(entity.ID);
            $('#<%=hdnParamedicID.ClientID %>').val(entity.ParamedicID);
            $('#<%=txtParamedicCode.ClientID %>').val(entity.ParamedicCode);
            $('#<%=txtParamedicName.ClientID %>').val(entity.ParamedicName);
            $('#containerEntry').show();
        });

        //#region Physician
        function onGetPhysicianFilterExpression() {
            var filterExpression = "ParamedicID IN (SELECT ParamedicID FROM ServiceUnitParamedic WHERE HealthcareServiceUnitID = '" + $('#<%=hdnHealthcareServiceUnitID.ClientID %>').val() + "') AND IsDeleted = 0";
            return filterExpression;
        }

        $('#lblParamedic.lblLink').live('click', function () {
            openSearchDialog('paramedic', onGetPhysicianFilterExpression(), function (value) {
                $('#<%=txtParamedicCode.ClientID %>').val(value);
                onTxtServicePhysicianCodeChanged(value);
            });
        });

        $('#<%=txtParamedicCode.ClientID %>').live('change', function () {
            onTxtServicePhysicianCodeChanged($(this).val());
        });

        function onTxtServicePhysicianCodeChanged(value) {
            var filterExpression = onGetPhysicianFilterExpression() + " AND ParamedicCode = '" + value + "'";
            Methods.getObject('GetvParamedicMasterList', filterExpression, function (result) {
                if (result != null) {
                    $('#<%=hdnParamedicID.ClientID %>').val(result.ParamedicID);
                    $('#<%=txtParamedicName.ClientID %>').val(result.ParamedicName);
                }
                else {
                    $('#<%=hdnParamedicID.ClientID %>').val('');
                    $('#<%=txtParamedicCode.ClientID %>').val('');
                    $('#<%=txtParamedicName.ClientID %>').val('');
                }
            });
        }
        //#endregion

        var selectedItemID = "";
        var selectedParamedicID = "";
        $('.lnkHasil a').live('click', function (evt) {
            if (IsValid(evt, 'fsMPEntry', 'mpEntry')) {
                $row = $(this).closest('tr');
                var entity = rowToObject($row);
                selectedItemID = $(this).closest('tr').find('.keyField').html();
                selectedParamedicID = entity.ParamedicID;

                var labResultID = $('#<%=hdnLabResultID.ClientID %>').val();
                var transactionID = $('#<%=hdnTransactionHdID.ClientID %>').val();
                var visitID = $('#<%=hdnVisitID.ClientID %>').val();

                var paramResultTest = visitID + "|" + selectedItemID + "|" + labResultID + "|" + transactionID + "|" + selectedParamedicID;
                var url = ResolveUrl("~/Program/WorkList/LaboratoryTestResult/LaboratoryTestResultEntryCtl1.ascx");
                openUserControlPopup(url, paramResultTest, 'Entry Hasil Pemeriksaan', 800, 500);
            }
        });

        function onShowWatermarkSetCustomToolbarVisibility() {
            $('#<%=btnImportResult.ClientID %>').hide();
            $('#<%=btnVoidResult.ClientID %>').hide();
        }

        function onHideWatermarkSetCustomToolbarVisibility() {
        }

        function onAfterSaveAddRecordEntryPopup(param) {
            var paramInfo = param.split('|');
            $('#<%=hdnLabResultID.ClientID %>').val(paramInfo[0]);
            $('#<%=txtReferenceNo.ClientID %>').val(paramInfo[1]);
            cbpView.PerformCallback('refresh');
            setTimeout(function () { cbpViewHasil.PerformCallback('refresh'); }, 1000);
        }

        function onAfterSaveEditRecordEntryPopup(param) {
            cbpView.PerformCallback('refresh');
            setTimeout(function () { cbpViewHasil.PerformCallback('refresh'); }, 1000);
        }

        function onCbpProcessEndCallback(s) {
            var param = s.cpResult.split('|');
            if (param[0] == 'fail')
                showToast('Save Failed', 'Error Message : ' + param[1]);
            else {
                var visitID = $('#<%=hdnVisitID.ClientID %>').val();
                var labResultID = s.cpRetval;
                var transactionID = $('#<%=hdnTransactionHdID.ClientID %>').val();
                $('#<%=hdnLabResultID.ClientID %>').val(labResultID);

                var paramResultTest = visitID + "|" + selectedItemID + "|" + labResultID + "|" + transactionID;
                var url = ResolveUrl("~/Program/WorkList/LaboratoryTestResult/LaboratoryTestResultEntryCtl1.ascx");
                openUserControlPopup(url, paramResultTest, 'Hasil Pemeriksaan', 800, 500);
            }
            hideLoadingPanel();
        }

        function oncboResultDeliveryPlanValueChanged(s) {
            var oValue = cboResultDeliveryPlan.GetValue();
            if (oValue == "X546^999") {
                $('#<%:txtResultDeliveryPlanOthers.ClientID %>').val("");
                $('#<%=txtResultDeliveryPlanOthers.ClientID %>').removeAttr('readonly');
            } else {
                $('#<%:txtResultDeliveryPlanOthers.ClientID %>').val("");
                $('#<%:txtResultDeliveryPlanOthers.ClientID %>').attr('readonly', 'true');
            }
        }

        function onBeforeLoadRightPanelContent(code) {
            if (code == 'pathologyAnatomyInfoEntry') {
                var visitID = $('#<%:hdnVisitID.ClientID %>').val();
                var testOrderID = "0";
                var serviceunitID = $('#<%:hdnHealthcareServiceUnitID.ClientID %>').val();
                var transactionID = $('#<%:hdnTransactionHdID.ClientID %>').val();
                return visitID + "|" + testOrderID + "|" + "" + "|" + serviceunitID + "|" + transactionID;
            }
            else if (code == 'btnCalculateGFR') {
                return $('#<%:hdnTransactionHdID.ClientID %>').val();
            }
        }

        function onBeforeRightPanelPrint(code, filterExpression, errMessage) {
            var labresultID = $('#<%=hdnLabResultID.ClientID %>').val();
            var visitID = $('#<%=hdnVisitID.ClientID %>').val();
            var transactionID = $('#<%=hdnTransactionHdID.ClientID %>').val();
            if (labresultID != "0") {
                if (code == 'PM-00734') {
                    filterExpression.text = transactionID;
                    return true;
                } 
                if (code == 'LB-00004' || code == 'LB-00015' || code == 'LB-00017') {
                    filterExpression.text = "TransactionID = " + transactionID;
                    return true;
                } else {
                    if ($('#<%=hdnIsAllowByPassPrint.ClientID %>').val() == '1') {
                        var isVerified = $('#<%=hdnIsVerified.ClientID %>').val();
                        if (isVerified == '0') {
                            errMessage.text = 'Hasil tidak dapat dicetak karena belum diverifikasi!';
                            return false;
                        }
                        else {
                            if (code == 'LB-00001' || code == 'LB-00005' || code == 'LB-00006' || code == 'LB-00008' || code == 'LB-00010' || code == 'LB-00024'
                                || code == 'LB-00012' || code == 'LB-00016' || code == 'LB-00018' || code == 'LB-00019' || code == 'LB-00020' || code == 'LB-00024'
                                || code == 'LB-00026' || code == 'LB-00027' || code == 'LB-00028' || code == 'LB-00030' || code == 'LB-00031' || code == 'LB-00032'
                                || code == 'LB-00037' || code == 'LB-00039' || code == 'LB-00038') {
                                filterExpression.text = "ID = " + labresultID;
                            }
                            return true;
                        }
                    } else {
                        if (code == 'LB-00001' || code == 'LB-00005' || code == 'LB-00006' || code == 'LB-00008' || code == 'LB-00010' || code == 'LB-00024'
                        || code == 'LB-00012' || code == 'LB-00016' || code == 'LB-00018' || code == 'LB-00019' || code == 'LB-00020' || code == 'LB-00024'
                        || code == 'LB-00026' || code == 'LB-00027' || code == 'LB-00028' || code == 'LB-00030' || code == 'LB-00031' || code == 'LB-00032'
                        || code == 'LB-00037' || code == 'LB-00039' || code == 'LB-00038') {
                            filterExpression.text = "ID = " + labresultID;
                        }
                        return true;
                    }
                }
            } else {
                errMessage.text = 'Tidak ada hasil pemeriksaan yang dapat dicetak!';
                return false;
            }
        }

        $('#<%=grdView2.ClientID %> .lblCommentDetail.lblLink').die('click');
        $('#<%=grdView2.ClientID %> .lblCommentDetail.lblLink').live('click', function () {
            $row = $(this).parent().closest('tr');
            var entity = rowToObject($row);
            var transactionID = $('#<%=hdnTransactionHdID.ClientID %>').val();

            var filterExpressionOrder = "ChargeTransactionID = '" + transactionID + "' AND ItemID = '" + entity.ItemID + "' AND FractionID = '" + entity.FractionID + "'";
            Methods.getObject('GetvLaboratoryResultDtList', filterExpressionOrder, function (result1) {
                if (result1 != null) {
                    showToast('Comment', result1.Remarks);
                }
                else {
                    var filterExpressionChargesInfo = "ID = (SELECT ID FROM PatientChargesDt WHERE TransactionID = " + transactionID + " AND ItemID = " + entity.ItemID + " AND IsDeleted = 0)";
                    Methods.getObject('GetPatientChargesDtInfoList', filterExpressionChargesInfo, function (result2) {
                        if (result2 != null) {
                            showToast('Comment', result2.Remarks);
                        }
                    });
                }
            });
        });

        function setRightPanelButtonEnabled() {
            var transactionID = $('#<%=hdnTransactionHdID.ClientID %>').val();
            var itemKreatininSerum = 0;
            var filterParam = "ParameterCode = 'LB0038'";
            var isHasItem = false;
            Methods.getObject('GetSettingParameterDtList', filterParam, function (result2) {
                if (result2 != null) {
                    itemKreatininSerum = result2.ParameterValue;
                }
            });

            var filterExpression = "TransactionID = '" + transactionID + "' AND IsDeleted = 0 AND ItemID = '" + itemKreatininSerum + "' AND ISNULL(GCTransactionDetailStatus,'') != 'X121^999'";
            Methods.getObject('GetPatientChargesDtList', filterExpression, function (result) {
                if (result != null) {
                    isHasItem = true;
                }
            });

            if (!isHasItem) {
                $('#btnCalculateGFR').attr('enabled', 'false');
            }
        }

        $('.lblReffRange.lblLink').die('click');
        $('.lblReffRange.lblLink').live('click', function () {
            $row = $(this).parent().closest('tr');
            var entity = rowToObject($row);
            showToast('Nilai Referensi', entity.cfReferenceRange);
        });
    </script>
    <input type="hidden" value="" id="hdnChargesDtID" runat="server" />
    <input type="hidden" value="" id="hdnTransactionHdID" runat="server" />
    <input type="hidden" value="" id="hdnLabResultID" runat="server" />
    <input type="hidden" value="" id="hdnIsStatusOpen" runat="server" />
    <input type="hidden" value="" id="hdnWatermarkText" runat="server" />
    <input type="hidden" value="" id="hdnType" runat="server" />
    <input type="hidden" value="" id="hdnIsVerified" runat="server" />
    <input type="hidden" value="" id="hdnIsAllowByPassPrint" runat="server" />
    <input type="hidden" value="0" id="hdnIsBridgingLIS" runat="server" />
    <input type="hidden" value="0" id="hdnIsResultExists" runat="server" />
    <input type="hidden" value="" id="hdnTransactionNo" runat="server" />
    <input type="hidden" value="0" id="hdnIsPathologicalAnatomyTest" runat="server" />
    <div style="height: 435px; overflow-y: auto;">
        <table class="tblContentArea">
            <colgroup>
                <col style="width: 50%" />
                <col style="width: 50%" />
            </colgroup>
            <tr>
                <td style="padding: 5px; vertical-align: top">
                    <table class="tblEntryContent" style="width: 100%">
                        <colgroup>
                            <col style="width: 30%" />
                            <col />
                        </colgroup>
                        <tr>
                            <td class="tdLabel">
                                <label id="lblTransactionNo" class="lblNormal" runat="server">
                                    <%=GetLabel("No. Transaksi")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtTransactionNo" Width="231px" runat="server" ReadOnly="true" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <%=GetLabel("Tanggal ") %>
                                /
                                <%=GetLabel("Jam ") %>
                                Hasil
                            </td>
                            <td>
                                <table cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td style="padding-right: 1px; width: 145px">
                                            <asp:TextBox ID="txtResultDate" Width="120px" CssClass="datepicker" runat="server" />
                                        </td>
                                        <td style="width: 5px">
                                            &nbsp;
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtResultTime" Width="80px" CssClass="time" runat="server" Style="text-align: center" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel" valign="top" style="padding-top: 5px">
                                <%=GetLabel("Catatan Hasil")%>
                            </td>
                            <td colspan="2">
                                <asp:TextBox ID="txtNotes" Width="100%" runat="server" TextMode="MultiLine" Rows="2" />
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
                                <%=GetLabel("Nomor Order")%>
                            </td>
                            <td>
                                <asp:TextBox ID="txtOrderNumber" ReadOnly="true" Width="100%" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <%=GetLabel("Tanggal ")%>
                                /
                                <%=GetLabel("Jam ") %>
                                Order
                            </td>
                            <td>
                                <input type="hidden" runat="server" id="hdnTestOrderID" />
                                <table cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td style="padding-right: 1px; width: 120px">
                                            <asp:TextBox ID="txtOrderDate" Width="100%" runat="server" ReadOnly="true" Style="text-align: center" />
                                        </td>
                                        <td style="width: 5px">
                                            &nbsp;
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtOrderTime" Width="80px" CssClass="time" runat="server" Style="text-align: center"
                                                ReadOnly="true" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <%=GetLabel("Diorder Oleh")%>
                            </td>
                            <td>
                                <asp:TextBox ID="txtOrderBy" ReadOnly="true" Width="100%" runat="server" />
                            </td>
                        </tr>
                        <tr id="trPA" runat="server" style="display: none;">
                            <td class="tdLabel">
                                <label id="Label3" class="lblNormal" runat="server">
                                    <%=GetLabel("No. PA")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtPAReferenceNo" Width="207px" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label id="Label1" class="lblNormal" runat="server">
                                    <%=GetLabel("No. Referensi")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtReferenceNo" Width="207px" runat="server" />
                            </td>
                        </tr>
                        <tr id="trResultDeliveryPlan" runat="server">
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%:GetLabel("Rencana Pengambilan Hasil")%></label>
                            </td>
                            <td colspan="2">
                                <dxe:ASPxComboBox ID="cboResultDeliveryPlan" ClientInstanceName="cboResultDeliveryPlan"
                                    Width="100%" runat="server">
                                    <ClientSideEvents ValueChanged="function(s){ oncboResultDeliveryPlanValueChanged(s); }" />
                                </dxe:ASPxComboBox>
                            </td>
                            <td>
                                <asp:TextBox ID="txtResultDeliveryPlanOthers" Width="100%" runat="server" ReadOnly="true" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label id="Label2" class="lblNormal" runat="server">
                                    <%=GetLabel("Download Attachment")%></label>
                            </td>
                            <td>
                                <input type="hidden" runat="server" id="hdnResultAttachment" />
                                <div>
                                    <asp:Label ID="lblResultAttachment" class="lblResultAttachment lblLink" runat="server"><%=GetLabel("Preview")%></asp:Label>
                                </div>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <div class="containerUlTabPage">
                        <ul class="ulTabPage" id="ulTabLabResult">
                            <li class="selected" contentid="containerDaftar">
                                <%=GetLabel("DAFTAR PEMERIKSAAN") %></li>
                            <li contentid="containerHasil">
                                <%=GetLabel("HASIL PEMERIKSAAN") %></li>
                        </ul>
                    </div>
                    <div id="containerDaftar" class="containerDaftarDt">
                        <div id="containerEntry" style="margin-top: 4px; display: none;">
                            <div class="pageTitle">
                                <%=GetLabel("Ubah Dokter")%></div>
                            <fieldset id="fsTrxPopup" style="margin: 0">
                                <input type="hidden" value="" id="hdnEntryID" runat="server" />
                                <table style="width: 100%" class="tblEntryDetail">
                                    <colgroup>
                                        <col style="width: 50%" />
                                    </colgroup>
                                    <tr>
                                        <td valign="top">
                                            <table style="width: 50%">
                                                <colgroup>
                                                    <col style="width: 150px" />
                                                </colgroup>
                                                <tr>
                                                    <td class="tdLabel">
                                                        <label class="lblLink lblMandatory" id="lblParamedic">
                                                            <%=GetLabel("Dokter Pembaca")%></label>
                                                    </td>
                                                    <td>
                                                        <input type="hidden" value="" id="hdnParamedicID" runat="server" />
                                                        <table cellpadding="0" cellspacing="0" width="100%">
                                                            <colgroup>
                                                                <col style="width: 120px" />
                                                                <col style="width: 3px" />
                                                                <col />
                                                            </colgroup>
                                                            <tr>
                                                                <td>
                                                                    <asp:TextBox ID="txtParamedicCode" Width="100%" runat="server" />
                                                                </td>
                                                                <td>
                                                                    &nbsp;
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtParamedicName" ReadOnly="true" Width="100%" runat="server" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <table style="width: 100%">
                                                <tr>
                                                    <td>
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <input type="button" id="btnSave" value='<%= GetLabel("Save")%>' />
                                                                </td>
                                                                <td>
                                                                    <input type="button" id="btnCancel" value='<%= GetLabel("Cancel")%>' />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </fieldset>
                        </div>
                        <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
                            ShowLoadingPanel="false" OnCallback="cbpView_Callback">
                            <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ hideLoadingPanel(); }" />
                            <PanelCollection>
                                <dx:PanelContent ID="PanelContent2" runat="server">
                                    <asp:Panel runat="server" ID="Panel1" CssClass="pnlContainerGrid" Style="height: 200px">
                                        <asp:GridView ID="grdView" runat="server" CssClass="grdSelected" AutoGenerateColumns="false"
                                            ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                            <Columns>
                                                <asp:BoundField DataField="ItemID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                                <asp:TemplateField HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <table cellpadding="0" cellspacing="0">
                                                            <tr>
                                                                <td>
                                                                    <img class="imgEdit <%# Eval("IsVerified").ToString() == "True" ? "imgDisabled" : Eval("ResultGCTransactionStatus").ToString() == "X121^001" ? "imgLink" : "imgDisabled" %>"
                                                                        title='<%=GetLabel("Edit")%>' src='<%# Eval("IsVerified").ToString() == "True" ? ResolveUrl("~/Libs/Images/Button/edit_disabled.png") : Eval("ResultGCTransactionStatus").ToString() == "X121^001" ? ResolveUrl("~/Libs/Images/Button/edit.png") : ResolveUrl("~/Libs/Images/Button/edit_disabled.png")%>'
                                                                        alt="" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                        <input type="hidden" value="<%#:Eval("ID") %>" bindingfield="ID" />
                                                        <input type="hidden" value="<%#:Eval("ParamedicID") %>" bindingfield="ParamedicID" />
                                                        <input type="hidden" value="<%#:Eval("ParamedicCode") %>" bindingfield="ParamedicCode" />
                                                        <input type="hidden" value="<%#:Eval("ParamedicName") %>" bindingfield="ParamedicName" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="ItemName1" HeaderText="Jenis Pemeriksaan" HeaderStyle-HorizontalAlign="Left" />
                                                <asp:BoundField DataField="ParamedicName" HeaderText="Dokter" HeaderStyle-Width="300px"
                                                    HeaderStyle-HorizontalAlign="Left" />
                                                <asp:HyperLinkField HeaderText="" Text="Hasil Pemeriksaan" ItemStyle-HorizontalAlign="Center"
                                                    ItemStyle-CssClass="lnkHasil" HeaderStyle-Width="150px" HeaderStyle-HorizontalAlign="Left" />
                                            </Columns>
                                            <EmptyDataTemplate>
                                                <%=GetLabel("Tidak ada Data Pemeriksaan / Pelayanan Pasien")%>
                                            </EmptyDataTemplate>
                                        </asp:GridView>
                                    </asp:Panel>
                                </dx:PanelContent>
                            </PanelCollection>
                        </dxcp:ASPxCallbackPanel>
                    </div>
                    <div id="containerHasil" style="display: none" class="containerDaftarDt">
                        <dxcp:ASPxCallbackPanel ID="cbpViewHasil" runat="server" Width="100%" ClientInstanceName="cbpViewHasil"
                            ShowLoadingPanel="false" OnCallback="cbpViewHasil_Callback">
                            <ClientSideEvents BeginCallback="function(s,e){showLoadingPanel(); }" EndCallback="function(s,e){ hideLoadingPanel(); }" />
                            <PanelCollection>
                                <dx:PanelContent ID="PanelContent1" runat="server">
                                    <asp:Panel runat="server" ID="Panel2" CssClass="pnlContainerGrid" Style="height: 200px">
                                        <asp:GridView ID="grdView2" runat="server" CssClass="grdSelected" AutoGenerateColumns="false"
                                            ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                            <Columns>
                                                <asp:TemplateField HeaderText="Pemeriksaan" HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="350px">
                                                    <ItemTemplate>
                                                        <div>
                                                            <%#: Eval("ItemName1") %></div>
                                                        <input type="hidden" value="<%#:Eval("cfReferenceRange") %>" bindingfield="cfReferenceRange" />
                                                        <input type="hidden" value="<%#:Eval("ID") %>" bindingfield="ID" />
                                                        <input type="hidden" value="<%#:Eval("ItemID") %>" bindingfield="ItemID" />
                                                        <input type="hidden" value="<%#:Eval("FractionID") %>" bindingfield="FractionID" />
                                                        <input type="hidden" value="<%#:Eval("Remarks") %>" bindingfield="Remarks" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Artikel Pemeriksaan" HeaderStyle-HorizontalAlign="Left"
                                                    ItemStyle-Width="250px">
                                                    <ItemTemplate>
                                                        <div class='<%#: Eval("IsNormal").ToString() == "False" ? "isAbnormalColor" : "" %>'>
                                                            <%#: Eval("FractionName1") %></div>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Pending Hasil" HeaderStyle-HorizontalAlign="Center"
                                                    ItemStyle-HorizontalAlign="Center" ItemStyle-Width="60px">
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chkIsPendingResult" runat="server" CssClass="chkIsPendingResult"
                                                            Checked='<%# Eval("IsPendingResult")%>' Enabled="false" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="TextValue" HeaderStyle-CssClass="hiddenColumn" ItemStyle-CssClass="textResultValue hiddenColumn" />
                                                <asp:TemplateField HeaderText="Hasil" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                    ItemStyle-Width="100px">
                                                    <ItemTemplate>
                                                        <div class='<%#: Eval("IsNormal").ToString() == "False" ? "isAbnormalColor" : "" %>'
                                                            <%# Eval("cfIsResultInPDF").ToString() == "True" ? "Style='display:none'" : "Style='text-align: left'" %>>
                                                            <asp:Literal ID="literal" Text='<%# GetISBridgingToLIS() == "True" ? Eval("cfTestResultValueNew") : Eval("MetricResultValue") %>' Mode="PassThrough"
                                                                runat="server" /></div>
                                                        <div <%# Eval("cfIsResultInPDF").ToString() == "False" ? "Style='display:none'" : "" %>>
                                                            <asp:Label class="lblViewPDF lblLink" runat="server"><%#: Eval("cfTestReferenceValue") %></asp:Label>
                                                        </div>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Flag" ItemStyle-Width="40px" HeaderStyle-HorizontalAlign="Center"
                                                    ItemStyle-HorizontalAlign="Center">
                                                    <ItemTemplate>
                                                        <div class='<%#: Eval("IsNormal").ToString() == "False" ? "isAbnormalColor" : "" %>'
                                                            style="text-align: right">
                                                            <%#: Eval("ResultFlag") %></div>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Satuan" ItemStyle-Width="60px" HeaderStyle-HorizontalAlign="Left"
                                                    ItemStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <div class='<%#: Eval("IsNormal").ToString() == "False" ? "isAbnormalColor" : "" %>'>
                                                            <%#: Eval("MetricUnit") %></div>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Nilai Referensi" ItemStyle-Width="140px" HeaderStyle-HorizontalAlign="Left"
                                                    ItemStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <div class='<%#: Eval("cfIsReferenceAsLink").ToString() == "True" ? "lblLink lblReffRange" : "" %>'>
                                                            <asp:Literal ID="literalRefRange" Text='<%# GetISBridgingToLIS() == "True" ? Eval("cfReferenceRangeCustom") : Eval("MetricUnitLabel") %>' Mode="PassThrough"
                                                                runat="server" /></div>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="" ItemStyle-Width="50px" HeaderStyle-HorizontalAlign="Left"
                                                    ItemStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <div class='lblLink lblCommentDetail'>
                                                            <%=GetLabel("Comment")%>
                                                        </div>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Verifikasi" ItemStyle-Width="120px" HeaderStyle-HorizontalAlign="Left"
                                                    ItemStyle-HorizontalAlign="Left">
                                                    <ItemTemplate>
                                                        <div>
                                                            <%#: Eval("VerifiedUserName") %></div>
                                                        <div>
                                                            <%#: Eval("VerifiedDateInString") %></div>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                            <EmptyDataTemplate>
                                                <%=GetLabel("Tidak ada Data Pemeriksaan / Pelayanan Pasien")%>
                                            </EmptyDataTemplate>
                                        </asp:GridView>
                                    </asp:Panel>
                                </dx:PanelContent>
                            </PanelCollection>
                        </dxcp:ASPxCallbackPanel>
                    </div>
                </td>
            </tr>
        </table>
        <div class="imgLoadingGrdView" id="containerImgLoadingView">
            <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
        </div>
    </div>
</asp:Content>
