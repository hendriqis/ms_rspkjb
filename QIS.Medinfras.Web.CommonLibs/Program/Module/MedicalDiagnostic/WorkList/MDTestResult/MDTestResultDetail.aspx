<%@ Page Title="" Language="C#" MasterPageFile="~/libs/MasterPage/MPTrx2.master"
    AutoEventWireup="true" CodeBehind="MDTestResultDetail.aspx.cs" Inherits="QIS.Medinfras.Web.CommonLibs.Program.MDTestResultDetail" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Src="~/Libs/Controls/PatientBannerCtl.ascx" TagName="PatientBannerCtl"
    TagPrefix="uc1" %>
<asp:Content ID="Content4" ContentPlaceHolderID="plhCustomButtonToolbarLeft" runat="server">
    <li id="btnMDTestResultBack" runat="server" crudmode="R">
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
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div class="menuTitle">
        <%=HttpUtility.HtmlEncode(GetPageTitle())%></div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="plhHeader" runat="server">
    <input type="hidden" value="" id="hdnResultGCTransactionStatus" runat="server" />
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
    <input type="hidden" value="" id="hdnImagingServiceUnitID" runat="server" />
    <input type="hidden" value="" id="hdnIsBridgingToRIS" runat="server" />
    <input type="hidden" value="" id="hdnPageTitle" runat="server" />
    <uc1:PatientBannerCtl ID="ctlPatientBanner" runat="server" />
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript">
        var selectedItemID = "";
        var Verified = "";
        $('.lnkHasil a').live('click', function (evt) {
            if (IsValid(evt, 'fsMPEntry', 'mpEntry')) {
                $row = $(this).closest('tr');
                var obj = rowToObject($row);

                selectedItemID = $(this).closest('tr').find('.keyField').html();
                selectedItemCode = obj.ItemCode;
                selectedItemName = obj.ItemName1;
                selectedParamedicID = obj.ParamedicID;
                selectedParamedicCode = obj.ParamedicCode;
                selectedParamedicName = obj.ParamedicName1;

                var imagingID = $('#<%=hdnID.ClientID %>').val();
                var isImagingResult = $('#<%=hdnIsImagingResult.ClientID %>').val();

                var paramResultTest = isImagingResult + "|" + selectedItemID + "|" + imagingID + "|" + selectedItemName + "|" + selectedItemCode + "|" + selectedParamedicID + "|" + selectedParamedicName + "|" + selectedParamedicCode;
                var isVerified = $(this).closest('tr').find('.hdnIsVerified').val();
                if (isVerified == "True" || $('#<%=hdnIsStatusOpen.ClientID %>').val() == '0') {
                    paramResultTest = selectedItemID + "|" + imagingID;
                    var url = ResolveUrl("~/Libs/Program/Module/MedicalDiagnostic/WorkList/MDTestResult/MDTestResultDetailVerifiedCtl.ascx");
                    openUserControlPopup(url, paramResultTest, 'Hasil Pemeriksaan', 1000, 600);
                }
                else {
                    var url = ResolveUrl("~/Libs/Program/Module/MedicalDiagnostic/WorkList/MDTestResult/MDTestResultDetailCtl.ascx");
                    openUserControlPopup(url, paramResultTest, 'Hasil Pemeriksaan', 1000, 600);
                }
            }
        });

        function onAfterSaveEditRecord(param) {
            $('#<%=hdnID.ClientID %>').val(param);
        }

        function onAfterSaveAddRecordEntryPopup(param) {
            $('#<%=hdnID.ClientID %>').val(param);
            cbpView.PerformCallback('refresh');
        }

        function onAfterSaveEditRecordEntryPopup(param) {
            cbpView.PerformCallback('refresh');
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

        $('.imgPrintIndo').live('click', function (evt) {
            if (IsValid(evt, 'fsMPEntry', 'mpEntry')) {
                var imagingID = $('#<%=hdnID.ClientID %>').val();
                selectedItemID = $(this).closest('tr').find('.keyField').html();
                var isVerified = $(this).closest('tr').find('.hdnIsVerified').val();
                var isControlPrint = $('#<%=hdnIsAllowPrintAfterVerified.ClientID %>').val();
                if (isControlPrint == '1' && isVerified == "0") {
                    errMessage.text = 'Hasil tidak dapat dicetak karena belum diverifikasi!';
                    return false;
                }
                else {
                    var filter = selectedItemID + "|" + imagingID;
                    var reportCode = "IS-00001"; //IS0001
                    var filterExpression = "ItemID = '" + selectedItemID + "' AND ImagingID='" + imagingID + "'";
                    openReportViewer(reportCode, filterExpression);
                }
            }
        });

        $('.imgPrintEng').live('click', function (evt) {
            if (IsValid(evt, 'fsMPEntry', 'mpEntry')) {
                selectedItemID = $(this).closest('tr').find('.keyField').html();

                var imagingID = $('#<%=hdnID.ClientID %>').val();
                var reportCode = "IS-00002"; //IS-00002
                var filterExpression = "ItemID = '" + selectedItemID + "' AND ImagingID = '" + imagingID + "'";
                openReportViewer(reportCode, filterExpression);
            }
        });

        function onCbpProcessEndCallback(s) {
            var param = s.cpResult.split('|');
            if (param[0] == 'fail')
                showToast('Save Failed', 'Error Message : ' + param[1]);
            else {
                var imagingID = s.cpRetval;
                $('#<%=hdnID.ClientID %>').val(imagingID);
                var paramResultTest = selectedItemID + "|" + imagingID;
                var url = ResolveUrl("~/Libs/Program/Module/MedicalDiagnostic/WorkList/MDTestResult/MDTestResultDetailCtl.ascx");
                openUserControlPopup(url, paramResultTest, 'Test Result Detail', 1000, 600);
            }
            hideLoadingPanel();
        }

        function onLoad() {
            setDatePicker('<%=txtResultDate.ClientID %>');

            $('#<%=btnMDTestResultBack.ClientID %>').click(function () {
                showLoadingPanel();
                if ($('#<%=hdnType.ClientID %>').val() == 'hs')
                    document.location = ResolveUrl("~/Libs/Program/Module/MedicalDiagnostic/WorkList/MDTestResult/MDTestResultHistoryList.aspx");
                else if ($('#<%=hdnIsImagingResult.ClientID %>').val() == "1")
                    document.location = ResolveUrl("~/Program/PatientList/ImagingTestResultList.aspx");
                else
                    document.location = ResolveUrl("~/Libs/Program/Module/MedicalDiagnostic/WorkList/MDTestResult/MDTestResultList.aspx");
            });

            $('#<%=btnVoidResult.ClientID %>').click(function () {
                showDeleteConfirmation(function (data) {
                    var param = 'delete_result;' + data.GCDeleteReason + ';' + data.Reason;
                    onCustomButtonClick(param);
                });
            });

            $('#<%=btnReopenResult.ClientID %>').click(function () {
                onCustomButtonClick('reopen_result');
            });

            if ($('#<%=hdnIsStatusOpen.ClientID %>').val() == '0') {
                hideToolbar();
                $('#<%=btnVoidResult.ClientID %>').hide();
                showWatermark($('#<%=hdnWatermarkText.ClientID %>').val());
            }

            if ($('#<%=hdnIsBridgingToRIS.ClientID %>').val() == "1") {
                $('#<%=btnVoidResult.ClientID %>').hide();
            }

            if ($('#<%=hdnResultGCTransactionStatus.ClientID %>').val() != Constant.TransactionStatus.WAIT_FOR_APPROVAL) {
                $('#<%=btnReopenResult.ClientID %>').hide();
            }
        }

        function onAfterCustomClickSuccess(type, retval) {
            var callback = type.split(';');
            if (type == 'saveparamedic') {
                cbpView.PerformCallback('refresh');
                $('#btnCancel').trigger('click');
            }
            else if (callback[0] == 'delete_result') {
                $('#<%=btnMDTestResultBack.ClientID %>').trigger('click');
            }
            else if (type == 'reopen_result') {
                $('#<%=btnMDTestResultBack.ClientID %>').click();
            }
        }

        $('.imgServiceParamedic.imgLink').live('click', function () {
            $row = $(this).closest('tr').parent().closest('tr');
            var entity = rowToObject($row);
            var id = entity.ID;
            var url = ResolveUrl("~/Libs/Program/Module/PatientManagement/Transaction/ParamedicTeamCtl.ascx");
            openUserControlPopup(url, id, 'Tim Medis', 600, 500);
        });

        function onGetPhysicianFilterExpression() {
            var filterExpression = "ParamedicID IN (SELECT ParamedicID FROM ServiceUnitParamedic WHERE HealthcareServiceUnitID = '" + $('#<%=hdnHealthcareServiceUnitID.ClientID %>').val() + "') AND IsDeleted = 0";
            return filterExpression;
        }

        //#region Physician
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
            if (code == "printLabelCover") 
            {
                return $('#<%:hdnTransactionHdID.ClientID %>').val();
            }
        }
        function onBeforeRightPanelPrint(code, filterExpression, errMessage) {
            var resultID = $('#<%=hdnID.ClientID %>').val();
            var visitID = $('#<%=hdnVisitID.ClientID %>').val();
            var transactionID = $('#<%=hdnTransactionHdID.ClientID %>').val();
            var iscontrolPrint = $('#<%=hdnIsAllowPrintAfterVerified.ClientID %>').val();

            if (resultID == '') {
                errMessage.text = 'Tidak ada hasil pemeriksaan yang dapat dicetak!';
                return false;
            }
            else {
                var hdnIsResultVerified = $('#<%=hdnIsResultVerified.ClientID %>').val();
                if (hdnIsResultVerified == '0' && iscontrolPrint == "1") {
                    errMessage.text = 'Hasil tidak dapat dicetak karena belum diverifikasi!';
                    return false;
                }
                else {
                    if (code == 'IS-00003' || code == 'IS-00007' || code == 'MD-00004' || code == 'IS-00023') {
                        filterExpression.text = "VisitID = " + visitID;
                        return true;
                    }
                    else if (code == 'MD-00007' || code == 'MD-00008' || code == 'IS-00015' || code == 'IS-00016' || code == 'IS-00014' || code == 'MD-00011') {
                        filterExpression.text = "TransactionID = " + transactionID;
                        return true;
                    }
                    else if (code == 'PM-00138' || code == 'PM-00189' || code == 'PM-00725' || code == 'PM-00726') {
                        filterExpression.text = transactionID;
                        return true;
                    }
                    else {
                        filterExpression.text = "ImagingID = " + resultID;
                    }
                    return true;
                }
            }
        }
    </script>
    <input type="hidden" value="" id="hdnID" runat="server" />
    <input type="hidden" value="" id="hdnChargesDtID" runat="server" />
    <input type="hidden" value="" id="hdnTransactionHdID" runat="server" />
    <input type="hidden" value="" id="hdnIsStatusOpen" runat="server" />
    <input type="hidden" value="" id="hdnWatermarkText" runat="server" />
    <input type="hidden" value="" id="hdnType" runat="server" />
    <input type="hidden" value="0" id="hdnIsImagingResult" runat="server" />
    <input type="hidden" value="1" id="hdnIsAllowPrintAfterVerified" runat="server" />
    <input type="hidden" value="1" id="hdnIsResultVerified" runat="server" />
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
                                <label id="Label1" class="lblNormal" runat="server">
                                    <%=GetLabel("No. Transaksi")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtTransactionNo" Width="231px" runat="server" ReadOnly="true" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <%=GetLabel("Tanggal") %>
                                /
                                <%=GetLabel("Jam") %>
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
                            <col style="width: 40%" />
                            <col style="width: 30%" />
                            <col />
                        </colgroup>
                        <tr>
                            <td class="tdLabel">
                                <%=GetLabel("Nomor Order")%>
                            </td>
                            <td colspan="2">
                                <asp:TextBox ID="txtOrderNo" ReadOnly="true" Width="205px" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <%=GetLabel("Tanggal")%>
                                /
                                <%=GetLabel("Jam") %>
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
                            <td colspan="2">
                                <asp:TextBox ID="txtOrderBy" ReadOnly="true" Width="300px" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <%=GetLabel("Catatan Klinis / Diagnosa")%>
                            </td>
                            <td colspan="2">
                                <asp:TextBox ID="txtDiagnosa" ReadOnly="false" Width="300px" runat="server" TextMode="MultiLine"
                                    Rows="2" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal" id="lblTransactionNo">
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
                            <td>
                                <dxe:ASPxComboBox ID="cboResultDeliveryPlan" ClientInstanceName="cboResultDeliveryPlan"
                                    Width="100%" runat="server">
                                    <ClientSideEvents ValueChanged="function(s){ oncboResultDeliveryPlanValueChanged(s); }" />
                                </dxe:ASPxComboBox>
                            </td>
                            <td>
                                <asp:TextBox ID="txtResultDeliveryPlanOthers" Width="100%" runat="server" ReadOnly="true" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <input type="hidden" id="hdnFilterExpression" runat="server" value="" />
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
                                                        <%=GetLabel("Dokter/Paramedis")%></label>
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
                            <dx:PanelContent ID="PanelContent1" runat="server">
                                <asp:Panel runat="server" ID="pnlView" CssClass="pnlContainerGrid">
                                    <asp:GridView ID="grdView" runat="server" CssClass="grdSelected" AutoGenerateColumns="false"
                                        ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                        <Columns>
                                            <asp:TemplateField HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <table cellpadding="0" cellspacing="0">
                                                        <tr>
                                                            <td>
                                                                <img class="imgServiceParamedic imgLink" <%# IsEditable.ToString() == "True" ?  "" : "style='display:none'" %>
                                                                    title='<%=GetLabel("Tim Dokter")%>' src='<%# ResolveUrl("~/Libs/Images/Button/paramedic_team.png")%>'
                                                                    alt="" style="margin-right: 2px" />
                                                            </td>
                                                            <td>
                                                                <img class="imgEdit <%# Eval("IsVerified").ToString() == "True" ? "imgDisabled" : Eval("ResultGCTransactionStatus").ToString() == "X121^001" ? "imgLink" : "imgDisabled" %>"
                                                                    title='<%=GetLabel("Edit")%>' src='<%# Eval("IsVerified").ToString() == "True" ? ResolveUrl("~/Libs/Images/Button/edit_disabled.png") : Eval("ResultGCTransactionStatus").ToString() == "X121^001" ? ResolveUrl("~/Libs/Images/Button/edit.png") : ResolveUrl("~/Libs/Images/Button/edit_disabled.png")%>'
                                                                    alt="" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                    <input type="hidden" value="<%#:Eval("ID") %>" bindingfield="ID" />
                                                    <input type="hidden" value="<%#:Eval("ItemCode") %>" bindingfield="ItemCode" />
                                                    <input type="hidden" value="<%#:Eval("ItemName1") %>" bindingfield="ItemName1" />
                                                    <input type="hidden" value="<%#:Eval("ParamedicID") %>" bindingfield="ParamedicID" />
                                                    <input type="hidden" value="<%#:Eval("ParamedicCode") %>" bindingfield="ParamedicCode" />
                                                    <input type="hidden" value="<%#:Eval("ParamedicName") %>" bindingfield="ParamedicName" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="ItemID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                            <asp:BoundField DataField="ItemName1" HeaderText="Nama Pelayanan" HeaderStyle-HorizontalAlign="Left" />
                                            <asp:BoundField DataField="ParamedicName" HeaderText="Dokter" HeaderStyle-Width="300px"
                                                HeaderStyle-HorizontalAlign="Left" />
                                            <asp:HyperLinkField HeaderText="Hasil Pemeriksaan" Text="Hasil Pemeriksaan" ItemStyle-HorizontalAlign="Center"
                                                ItemStyle-CssClass="lnkHasil" HeaderStyle-Width="120px" />
                                            <asp:TemplateField HeaderStyle-Width="150px" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                HeaderText="Verifikasi">
                                                <ItemTemplate>
                                                    <div>
                                                        <%#: Eval("IsVerified").ToString() == "True" ?  Eval("VerifiedUserName") : " "%></div>
                                                    <div>
                                                        <%#: Eval("IsVerified").ToString() == "True" ? Eval("CustomVerifiedDate") : " "%></div>
                                                    <input type="hidden" class="hdnIsVerified" value='<%# Eval("IsVerified")%>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <EmptyDataTemplate>
                                            <%=GetLabel("No Data To Display")%>
                                        </EmptyDataTemplate>
                                    </asp:GridView>
                                </asp:Panel>
                            </dx:PanelContent>
                        </PanelCollection>
                    </dxcp:ASPxCallbackPanel>
                    <div class="containerPaging">
                        <div class="wrapperPaging">
                            <div id="paging">
                            </div>
                        </div>
                    </div>
                </td>
            </tr>
        </table>
    </div>
    <dxcp:ASPxCallbackPanel ID="cbpProcess" runat="server" Width="100%" ClientInstanceName="cbpProcess"
        ShowLoadingPanel="false" OnCallback="cbpProcess_Callback">
        <ClientSideEvents BeginCallback="function(s,e) { showLoadingPanel(); }" EndCallback="function(s,e) { onCbpProcessEndCallback(s); }" />
    </dxcp:ASPxCallbackPanel>
</asp:Content>
