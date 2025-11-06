<%@ Page Title="" Language="C#" MasterPageFile="~/libs/MasterPage/MPList.master"
    AutoEventWireup="true" CodeBehind="TemplatePrescriptionList.aspx.cs" Inherits="QIS.Medinfras.Web.CommonLibs.Program.TemplatePrescriptionList" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<asp:Content ID="Content1" ContentPlaceHolderID="plhList" runat="server">
    <script type="text/javascript" src='<%= ResolveUrl("~/Libs/Scripts/CustomGridViewList.js")%>'></script>
    <script type="text/javascript">

        function onRefresh() {
            var hdnID = $('#<%=hdnID.ClientID %>').val();
            if (hdnID == '') {
                $('#lblEntryPopupAddData').hide();
                $('#lblEntryPopupAddCompoundData').hide();
            }
            else {
                $('#lblEntryPopupAddData').show();
                $('#lblEntryPopupAddCompoundData').show();
            }
        }

        function onRefresh2() {
            var hdnID = $('#<%=hdnID.ClientID %>').val();
            if (hdnID == '') {
                $('#lblEntryPopupAddData').hide();
                $('#lblEntryPopupAddCompoundData').hide();
            }
        }

        function onAfterDeleteClickSuccess() {
            $('#<%=hdnID.ClientID %>').val('');
            cbpViewDt.PerformCallback('refresh');
            cbpView.PerformCallback('refresh');
            onRefresh();
        }

        $('#lblEntryPopupAddData').live('click', function () {
            var prescriptionTemplateID = $('#<%=hdnPrescriptionTemplateID.ClientID %>').val();
            $('#<%=hdnIsFlagAdd.ClientID %>').val('1');
            $('#lblItem').attr('class', 'lblLink lblMandatory');
            $('#<%=hdnItemID.ClientID %>').val('');
            $('#<%=txtItemCode.ClientID %>').removeAttr('readonly');
            $('#<%=txtItemCode.ClientID %>').val('');
            $('#<%=txtItemName.ClientID %>').val('');
            $('#<%=txtFrequencyNumber.ClientID %>').val('1');
            $('#<%=txtDosingDose.ClientID %>').val('1');
            $('#<%=txtDosingDuration.ClientID %>').val('1');

            $('#containerPopupEntryData').show();
        });

        $('#lblEntryPopupAddCompoundData').live('click', function () {
            $row = $(this).closest('tr');
            var entity = rowToObject($row);
            var prescriptionTemplateID = $('#<%=hdnPrescriptionTemplateID.ClientID %>').val();
            var value = "add";
            var queryString = value + "|" + prescriptionTemplateID;
            var url = ResolveUrl("~/Libs/Program/Master/PhysicianPrescription/TemplatePrescriptionCompoundEntryCtl.ascx");
            openUserControlPopup(url, queryString, 'Template Racikan', 900, 650);
        });

        $('#btnEntryPopupCancel').live('click', function () {
            $('#containerPopupEntryData').hide();
        });

        $(function () {
            var grd = new customGridView();
            grd.init('<%=grdView.ClientID %>', '<%=hdnID.ClientID %>', '<%=hdnPrescriptionTemplateID.ClientID %>', '<%=pnlView.ClientID %>', cbpView, 'paging');

            onRefresh2();
            $('#<%=grdView.ClientID %> > tbody > tr:gt(0):not(.trEmpty)').live('click', function () {
                if ($(this).attr('class') == 'selected') {
                    $('#<%=grdView.ClientID %> tr.selected').removeClass('selected');
                    $(this).addClass('selected');
                    $('#<%=hdnPrescriptionTemplateID.ClientID %>').val($(this).find('.keyField').html());
                    onRefresh()
                    cbpViewDt.PerformCallback('refresh');
                }
            });
            $('#<%=grdView.ClientID %> tr:eq(1)').click();
        });


        $('#<%=lvwView.ClientID %> tr:gt(0):not(.trEmpty)').live('click', function () {
            if ($(this).attr('class') != 'selected') {
                $('#<%=lvwView.ClientID %> tr.selected').removeClass('selected');
                $(this).addClass('selected');
                $('#<%=hdnID.ClientID %>').val($(this).find('.keyField').html());
            }
        });

        function onRefreshControl(filterExpression) {
            $('#<%=hdnFilterExpression.ClientID %>').val(filterExpression);
            cbpView.PerformCallback('refresh');
        }

        function onGetCurrID() {
            return $('#<%=hdnID.ClientID %>').val();
        }

        $('#btnEntryPopupSave').live('click', function (evt) {
            if (IsValid(evt, 'fsEntryPopup', 'mpEntryPopup')) {
                cbpViewDt.PerformCallback('save');
            }
            else {
                return false;
            }
        });

        $('#.imgDelete.imgLink').live('click', function () {
            if (confirm("Are You Sure Want To Delete This Data?")) {
                $row = $(this).closest('tr');
                var prescriptionTemplateDetailID = $row.find('.hdnPrescriptionTemplateDetailID').val();
                $('#<%=hdnPrescriptionTemplateDetailID.ClientID %>').val(prescriptionTemplateDetailID);
                cbpViewDt.PerformCallback('delete');
            }
        });

        $('.imgEdit.imgLink').die('click');
        $('.imgEdit.imgLink').live('click', function () {
            $row = $(this).closest('tr');
            var entity = rowToObject($row);
            var isCompound = $row.find('.hdnIsCompound').val();
            if (isCompound != 'True') {
                $('#containerPopupEntryData').show();
                var itemID = $row.find('.hdnItemID').val();
                var itemCode = $row.find('.hdnItemCode').val();
                var itemName = $row.find('.hdnDrugName').val();
                var strengthAmount = $row.find('.hdnStrengthAmount').val();
                var strengthUnit = $row.find('.hdnStrengthUnit').val();
                var doseUnit = $row.find('.hdnGCDosingUnit').val();
                var frequency = $row.find('.hdnFrequency').val();
                var dose = $row.find('.hdnDose').val();
                var dosingDuration = $row.find('.hdnDosingDuration').val();
                var numberOfDosage = $row.find('.hdnNumberOfDosage').val();
                var isMorning = $row.find('.hdnIsMorning').val();
                var isNoon = $row.find('.hdnIsNoon').val();
                var isEvening = $row.find('.hdnIsEvening').val();
                var isNight = $row.find('.hdnIsNight').val();
                var isAsRequired = $row.find('.hdnIsAsRequired').val();
                var medicationAdministration = $row.find('.hdnMedicationAdministration').val();
                var coenamRule = $row.find('.hdnGCCoenamRule').val();
                var prescriptionTemplateDetailID = $row.find('.hdnPrescriptionTemplateDetailID').val();
                var route = $row.find('.hdnGCMedicationRoute').val();

                $('#<%=hdnPrescriptionTemplateDetailID.ClientID %>').val(prescriptionTemplateDetailID);
                $('#<%=hdnIsFlagAdd.ClientID %>').val('0');
                $('#lblItem').attr('class', 'lblDisabled');
                $('#<%=hdnEntryID.ClientID %>').val(entity.PrescriptionTemplateDetailID);
                $('#<%=lvwView.ClientID %> tr.focus').addClass('selected');
                $('#<%=hdnItemID.ClientID %>').val(itemID);
                $('#<%=txtItemCode.ClientID %>').attr('readonly', 'readonly');
                $('#<%=txtItemCode.ClientID %>').val(itemCode);
                $('#<%=txtItemCode.ClientID %>').trigger('change');
                $('#<%=txtItemName.ClientID %>').val(itemName);
                $('#<%=hdnGCDoseUnit.ClientID %>').val(doseUnit);
                $('#<%=txtFrequencyNumber.ClientID %>').val(frequency);
                $('#<%=txtDosingDose.ClientID %>').val(numberOfDosage);
                $('#<%=txtDosingDuration.ClientID %>').val(dosingDuration);
                $('#<%=txtMedicationAdministration.ClientID %>').val(medicationAdministration);
                $('#<%=hdnGCCoenamRule.ClientID %>').val(coenamRule);
                $('#<%=hdnGCMedicationRoute.ClientID %>').val(route);
                if (isMorning == "True")
                    $('#<%=chkIsMorning.ClientID %>').prop('checked', true);
                else
                    $('#<%=chkIsMorning.ClientID %>').prop('checked', false);
                if (isNoon == "True")
                    $('#<%=chkIsNoon.ClientID %>').prop('checked', true);
                else
                    $('#<%=chkIsNoon.ClientID %>').prop('checked', false);
                if (isEvening == "True")
                    $('#<%=chkIsEvening.ClientID %>').prop('checked', true);
                else
                    $('#<%=chkIsEvening.ClientID %>').prop('checked', false);
                if (isNight == "True")
                    $('#<%=chkIsNight.ClientID %>').prop('checked', true);
                else
                    $('#<%=chkIsNight.ClientID %>').prop('checked', false);
                if (isAsRequired == "True")
                    $('#<%=chkIsAsRequired.ClientID %>').prop('checked', true);
                else
                    $('#<%=chkIsAsRequired.ClientID %>').prop('checked', false);

                cboDosingUnit.SetValue(doseUnit);
                cboMedicationRoute.SetValue(route);
                calculateDispenseQty();
            }
            else {
                $row = $(this).closest('tr');
                var entity = rowToObject($row);
                var prescriptionTemplateID = $row.find('.hdnPrescriptionTemplateID').val();
                var prescriptionTemplateDetailID = $row.find('.hdnPrescriptionTemplateDetailID').val();

                $('#<%=hdnPrescriptionTemplateID.ClientID %>').val(prescriptionTemplateID);
                $('#<%=hdnPrescriptionTemplateDetailID.ClientID %>').val(prescriptionTemplateDetailID);
                var value = "edit";
                var queryString = value + "|" + prescriptionTemplateID + '|' + prescriptionTemplateDetailID;
                var url = ResolveUrl("~/Libs/Program/Master/PhysicianPrescription/TemplatePrescriptionCompoundEntryCtl.ascx");
                openUserControlPopup(url, queryString, 'Template Racikan', 900, 650);
            }
        });

        //#region Paging
        var pageCount = parseInt('<%=PageCount %>');
        var currPage = parseInt('<%=CurrPage %>');
        $(function () {
            setPaging($("#paging"), pageCount, function (page) {
                cbpView.PerformCallback('changepage|' + page);
            }, null, currPage);
        });

        function onCbpViewEndCallback(s) {
            hideLoadingPanel();
            var param = s.cpResult.split('|');
            if (param[0] == 'refresh') {
                var pageCount = parseInt(param[1]);
                if (pageCount > 0)
                    $('#<%=grdView.ClientID %> tr:eq(1)').click();

                setPaging($("#paging"), pageCount, function (page) {
                    cbpView.PerformCallback('changepage|' + page);
                });
            }
            else
                $('#<%=grdView.ClientID %> tr:eq(1)').click();
        }
        //#endregion

        //#region Paging Dt
        var pageCount = parseInt('<%=PageCount %>');
        $(function () {
            setPagingDetailItem(pageCount);
        });

        function setPagingDetailItem(pageCount) {
            setPaging($("#pagingdetail"), pageCount, function (page) {
                cbpViewDt.PerformCallback('changepage|' + page);
            }, 10);
        }

        function onCbpViewDtEndCallback(s) {
            var param = s.cpResult.split('|');
            if (param[0] == 'save') {
                if (param[1] == 'fail')
                    showToast('Save Failed', 'Error Message : ' + param[2]);
                else {
                    $('#containerPopupEntryData').hide();
                }
            }
            else if (param[0] == 'delete') {
                if (param[1] == 'fail')
                    showToast('Delete Failed', 'Error Message : ' + param[2]);
                else {
                    var pageCount = parseInt(param[2]);
                    setPagingDetailItem(pageCount);
                }
            }
            else if (param[0] == 'refresh') {
                var pageCount = parseInt(param[1]);
                setPagingDetailItem(pageCount);
                if (pageCount > 0)
                    $('#<%=lvwView.ClientID %> tr:eq(1)').click();

                setPaging($("#pagingDt"), pageCount, function (page) {
                    cbpViewDt.PerformCallback('changepage|' + page);
                });
            }
            $('#<%=hdnPrescriptionTemplateDetailID.ClientID %>').val();
            $('#containerImgLoadingViewDt').hide();
            $('#containerPopupEntryData').hide();
        }
        //#endregion


        function onAfterPopupControlClosing() {
            cbpViewDt.PerformCallback('refresh');
        }

        //#region Item
        function onGetItemMasterFilterExpression() {
            var filterExpression = "<%:OnGetItemMasterFilterExpression() %>";
            return filterExpression;
        }

        $('#lblItem.lblLink').die('click');
        $('#lblItem.lblLink').live('click', function () {
            openSearchDialog('item', onGetItemMasterFilterExpression(), function (value) {
                $('#<%=txtItemCode.ClientID %>').val(value);
                onTxtItemCodeChanged(value);
            });
        });

        $('#<%=txtItemCode.ClientID %>').die('change');
        $('#<%=txtItemCode.ClientID %>').live('change', function () {
            onTxtItemCodeChanged($(this).val());
        });

        function onTxtItemCodeChanged(value) {
            var filterExpression = onGetItemMasterFilterExpression() + " AND ItemCode = '" + value + "'";
            Methods.getObject('GetvDrugInfoList', filterExpression, function (result) {
                if (result != null) {
                    $('#<%=hdnItemID.ClientID %>').val(result.ItemID);
                    $('#<%=txtItemCode.ClientID %>').val(result.ItemCode);
                    $('#<%=txtItemName.ClientID %>').val(result.ItemName1);
                    $('#<%=txtDispenseUnit.ClientID %>').val(result.ItemUnit);
                    $('#<%=txtStrengthUnit.ClientID %>').val(result.DoseUnit);
                    $('#<%=txtStrengthAmount.ClientID %>').val(result.Dose);
                    $('#<%=hdnGCItemUnit.ClientID %>').val(result.GCItemUnit);
                    $('#<%=hdnGCMedicationRoute.ClientID %>').val(result.GCMedicationRoute);

                    cboDosingUnit.SetValue(result.GCItemUnit);
                    cboMedicationRoute.SetValue(result.GCMedicationRoute);
                }
                else {
                    $('#<%=hdnItemID.ClientID %>').val('');
                    $('#<%=txtItemCode.ClientID %>').val('');
                    $('#<%=txtItemName.ClientID %>').val('');
                    $('#<%=txtDispenseUnit.ClientID %>').val('');
                    $('#<%=txtStrengthUnit.ClientID %>').val('');
                    $('#<%=txtStrengthAmount.ClientID %>').val('');
                    $('#<%=hdnGCItemUnit.ClientID %>').val('');
                    $('#<%=hdnGCMedicationRoute.ClientID %>').val('');

                    cboDosingUnit.SetValue('');
                    cboMedicationRoute.SetValue('');
                }
            });
        }
        //#endregion

        //#region Signa
        $('#lblSigna.lblLink').live('click', function () {
            var filterExpression = "IsDeleted = 0";
            openSearchDialog('signa', filterExpression, function (value) {
                $('#<%=txtSignaLabel.ClientID %>').val(value);
                txtSignaLabelChanged(value);
            });
        });

        $('#<%=txtSignaLabel.ClientID %>').live('change', function () {
            txtSignaLabelChanged($(this).val());
        });

        function txtSignaLabelChanged(value) {
            var filterExpression = "IsDeleted = 0 AND SignaLabel = '" + value + "'";
            Methods.getObject('GetvSignaList', filterExpression, function (result) {
                if (result != null) {
                    $('#<%=hdnSignaID.ClientID %>').val(result.SignaID);
                    $('#<%=txtSignaName1.ClientID %>').val(result.SignaName1);
                    cboCoenamRule.SetValue(result.GCCoenamRule);
                    $('#<%=txtFrequencyNumber.ClientID %>').val(result.Frequency);
                    $('#<%=txtFrequencyNumber.ClientID %>').change();
                    cboFrequencyTimeline.SetValue(result.GCDosingFrequency);
                    cboDosingUnit.SetValue(result.GCDoseUnit);
                    $('#<%=hdnGCDosingUnit.ClientID %>').val(result.GCDosingUnit);
                    $('#<%=txtDosingDose.ClientID %>').val(result.Dose);
                    $('#<%=txtDosingDose.ClientID %>').change();
                } else {
                    $('#<%=txtSignaLabel.ClientID %>').val('');
                    $('#<%=txtSignaName1.ClientID %>').val('');
                }
            });
        }
        //#endregion

        //#region calculate Dispense Qty
        $('#<%=txtFrequencyNumber.ClientID %>').live('change', function () {
            calculateDispenseQty();
        });

        $('#<%=txtFrequencyNumber.ClientID %>').live('input', function () {
            $('#<%=txtSignaName1.ClientID %>').val('');
            $('#<%=txtSignaLabel.ClientID %>').val('');
            $('#<%=hdnSignaID.ClientID %>').val('');
        });

        $('#<%=txtDosingDose.ClientID %>').live('change', function () {
            calculateDispenseQty();
        });

        $('#<%=txtDosingDose.ClientID %>').live('input', function () {
            $('#<%=txtSignaName1.ClientID %>').val('');
            $('#<%=txtSignaLabel.ClientID %>').val('');
            $('#<%=hdnSignaID.ClientID %>').val('');
        });

        $('#<%=txtDosingDuration.ClientID %>').live('change', function () {
            var dosingUnit = cboDosingUnit.GetText();
            var itemUnit = $('#<%=txtDispenseUnit.ClientID %>').val();
            if (dosingUnit == itemUnit) {
                calculateDispenseQty();
            }
        });

        function cboFrequencyTimelineChanged() {
            var frequencyTimeLine = cboFrequencyTimeline.GetText();
            $('#<%=txtSignaName1.ClientID %>').val('');
            $('#<%=txtSignaLabel.ClientID %>').val('');
            $('#<%=hdnSignaID.ClientID %>').val('');
        }

        function cboDosingUnitChanged(paramProcess) {
            if (paramProcess != "edit") {
                calculateDispenseQty();
                $('#<%=txtSignaName1.ClientID %>').val('');
                $('#<%=txtSignaLabel.ClientID %>').val('');
                $('#<%=hdnSignaID.ClientID %>').val('');
            }
        }

        function calculateDispenseQty() {
            var frequency = $('#<%=txtFrequencyNumber.ClientID %>').val();
            var frequencyTimeLine = cboFrequencyTimeline.GetValue();
            var dose = $('#<%=txtDosingDose.ClientID %>').val();
            var dosingDuration = $('#<%=txtDosingDuration.ClientID %>').val();
            var strengthAmount = $('#<%=txtStrengthAmount.ClientID %>').val();
            var dosingUnit = cboDosingUnit.GetText();
            var itemUnit = $('#<%=txtDispenseUnit.ClientID %>').val();

            var frequencyInt = parseInt(frequency);
            var doseInt = parseInt(dose);
            var dosingDurationInt = parseInt(dosingDuration);

            if (frequencyInt < 0) {
                $('#<%=txtFrequencyNumber.ClientID %>').val('0');
            }

            if (doseInt < 0) {
                $('#<%=txtDosingDose.ClientID %>').val('0');
            }

            if (dosingDurationInt < 0) {
                $('#<%=txtDosingDuration.ClientID %>').val('0');
            }

            var dispenseQty = 0;
            if (frequency != '' && dose != '' && dosingDuration != '' && frequencyInt > 0 && doseInt > 0 && dosingDurationInt > 0) {
                if (dosingUnit == itemUnit) {
                    dispenseQty = Math.ceil(dosingDuration * frequency * dose);
                }
                else {
                    if (strengthAmount != 0)
                        dispenseQty = Math.ceil((dosingDuration * frequency * dose) / strengthAmount);
                    else
                        dispenseQty = 1;
                }

                $('#<%=txtDispenseQty.ClientID %>').val(dispenseQty);
                $('#<%=txtDispenseQty.ClientID %>').change();
            }
            else {
                dispenseQty = 1;
                $('#<%=txtDispenseQty.ClientID %>').val(dispenseQty);
                $('#<%=txtDispenseQty.ClientID %>').change();
            }

        }
        //#endregion

        function onCboDosingUnitEndCallback() {
            if ($('#<%=hdnGCDosingUnit.ClientID %>').val() == '') {
                if ($('#<%=hdnGCItemUnit.ClientID %>').val() == '')
                    cboDosingUnit.SetValue($('#<%=hdnGCBaseUnit.ClientID %>').val());
                else
                    cboDosingUnit.SetValue($('#<%=hdnGCItemUnit.ClientID %>').val());
            }
            else {
                cboDosingUnit.SetValue($('#<%=hdnGCDosingUnit.ClientID %>').val());
            }

            cboDosingUnitChanged();
        }

    </script>
    <input type="hidden" value="" id="hdnID" runat="server" />
    <input type="hidden" value="" id="hdnPrescriptionTemplateID" runat="server" />
    <input type="hidden" id="hdnPrescriptionTemplateDetailID" value="" runat="server" />
    <input type="hidden" value="" id="hdnItemID" runat="server" />
    <input type="hidden" value="" id="hdnTemplateType" runat="server" />
    <input type="hidden" value="" id="hdnParam" runat="server" />
    <input type="hidden" value="" id="hdnRecordFilterExpression" runat="server" />
    <input type="hidden" id="hdnFilterExpression" runat="server" value="" />
    <input type="hidden" runat="server" id="hdnCurrentSessionID" value="" />
    <input type="hidden" value="0" id="hdnParamedicID" runat="server" />
    <input type="hidden" value="0" id="hdnEmbalanceID" runat="server" />
    <input type="hidden" value="" id="hdnSignaID" runat="server" />
    <input type="hidden" value="0" id="hdnIsFlagAdd" runat="server" />
    <input type="hidden" value="0" id="hdnGCCompoundUnit" runat="server" />
    <input type="hidden" value="0" id="hdnGetCompoundQty" runat="server" />
    <input type="hidden" value="" id="hdnStrengthUnit" runat="server" />
    <input type="hidden" value="0" id="hdnStrengthAmount" runat="server" />
    <input type="hidden" value="" id="hdnDrugID" runat="server" />
    <input type="hidden" value="" id="hdnDrugName" runat="server" />
    <input type="hidden" value="" id="hdnGCDosingUnit" runat="server" />
    <input type="hidden" value="" id="hdnGCItemUnit" runat="server" />
    <input type="hidden" value="" id="hdnGCBaseUnit" runat="server" />
    <input type="hidden" value="" id="hdnGCDoseUnit" runat="server" />
    <input type="hidden" value="" id="hdnEntryID" runat="server" />
    <input type="hidden" value="" id="hdnGCRoute" runat="server" />
    <input type="hidden" value="" id="hdnGCCoenamRule" runat="server" />
    <input type="hidden" value="" id="hdnGCMedicationRoute" runat="server" />
    <input type="hidden" value="" id="hdnIsCompound" runat="server" />
    <div style="position: relative">
        <tr>
            <table style="width: 100%">
                <colgroup>
                    <col style="width: 30%" />
                    <col style="width: 70%" />
                </colgroup>
                <tr>
                    <td style="vertical-align: top">
                        <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
                            ShowLoadingPanel="false" OnCallback="cbpView_Callback">
                            <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ onCbpViewEndCallback(s); }" />
                            <PanelCollection>
                                <dx:PanelContent ID="PanelContent1" runat="server">
                                    <asp:Panel runat="server" ID="pnlView" CssClass="pnlContainerGridProcessList">
                                        <asp:GridView ID="grdView" runat="server" CssClass="grdSelected" AutoGenerateColumns="false"
                                            ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                            <Columns>
                                                <asp:BoundField DataField="PrescriptionTemplateID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                                <asp:BoundField DataField="PrescriptionTemplateCode" HeaderText="KODE TEMPLATE" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="30px" />
                                                <asp:BoundField DataField="PrescriptionTemplateName" HeaderStyle-HorizontalAlign="Left" HeaderText="NAMA TEMPLATE" HeaderStyle-Width="150px" />
                                            </Columns>
                                            <EmptyDataTemplate>
                                                <%=GetLabel("Tidak ada Template Resep")%>
                                            </EmptyDataTemplate>
                                        </asp:GridView>
                                    </asp:Panel>
                                </dx:PanelContent>
                            </PanelCollection>
                        </dxcp:ASPxCallbackPanel>
                        <div class="imgLoadingGrdView" id="containerImgLoadingView">
                            <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                        </div>
                        <div class="containerPaging">
                            <div class="wrapperPaging">
                                <div id="paging">
                                </div>
                            </div>
                        </div>
                    </td>
                    <td style="vertical-align: top">
                    <div id="containerPopupEntryData" style="margin-top: 10px; display: none;">
                            <div class="pageTitle">
                                <%=GetLabel("Entry")%></div>
                            <fieldset id="fsEntryPopup" style="margin: 0">
                                <table class="tblEntryDetail" style="width: 100%">
                                    <colgroup>
                                        <col style="width: 15%"/>
                                    </colgroup>
                                    <tr>
                                        <td class="tdLabel">
                                            <label class="lblMandatory lblLink" id="lblItem">
                                                <%=GetLabel("Item")%></label>
                                        </td>
                                        <td>
                                            <table style="width: 100%" cellpadding="0" cellspacing="0">
                                                <colgroup>
                                                    <col style="width: 80px" />
                                                    <col style="width: 3px" />
                                                    <col style="width: 460px"/>
                                                    <col />
                                                </colgroup>
                                                <tr>
                                                    <td>
                                                       <asp:TextBox ID="txtItemCode" CssClass="required" Width="100%" runat="server" />
                                                    </td>
                                                    <td>
                                                        &nbsp;
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtItemName" ReadOnly="true" Width="100%" runat="server" />
                                                    </td>
                                                    <td>
                                                        &nbsp;
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tdLabel">
                                            <label id="lblConversion">
                                                <%=GetLabel("Strength Conversion")%></label>
                                        </td>
                                        <td>
                                            <table style="width: 100%" cellpadding="0" cellspacing="0">
                                                <colgroup>
                                                    <col style="width: 80px" />
                                                    <col style="width: 3px" />
                                                    <col style="width: 100px"/>
                                                    <col />
                                                </colgroup>
                                                <tr>
                                                    <td>
                                                       <asp:TextBox ID="txtStrengthAmount" ReadOnly="true" Width="104%" runat="server" />
                                                    </td>
                                                    <td>
                                                        &nbsp;
                                                    </td>
                                                    <td>
                                                        <asp:TextBox runat="server" ID="txtStrengthUnit" Width="100%" ReadOnly="true" TabIndex="999" />
                                                    </td>
                                                    <td>
                                                        &nbsp;
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tdLabel">
                                            <label class="lblNormal lblLink" id="lblSigna">
                                                <%=GetLabel("Signa Template")%></label>
                                        </td>
                                         <td>
                                            <table style="width: 100%" cellpadding="0" cellspacing="0">
                                                <colgroup>
                                                    <col style="width: 80px" />
                                                    <col style="width: 3px" />
                                                    <col style="width: 460px"/>
                                                    <col />
                                                </colgroup>
                                                <tr>
                                                    <td>
                                                        <asp:TextBox runat="server" ID="txtSignaLabel" Width="100%" />
                                                    </td>
                                                     <td>
                                                        &nbsp;
                                                    </td>
                                                    <td>
                                                        <asp:TextBox runat="server" ID="txtSignaName1" Width="100%" ReadOnly="true" TabIndex="999" />
                                                    </td>
                                                    <td>
                                                        &nbsp;
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tdLabel">
                                            <label class="lblMandatory">
                                                <%=GetLabel("Frekuensi dan Dosis")%></label>
                                        </td>
                                        <td>
                                            <table style="width: 100%" cellpadding="0" cellspacing="0">
                                                <tr>
                                                    <td style="width: 80px">
                                                        <asp:TextBox ID="txtFrequencyNumber" runat="server" Width="100%" CssClass="number" />
                                                    </td>
                                                    <td style="width: 3px">
                                                        &nbsp;
                                                    </td>
                                                    <td style="width: 20%">
                                                        <dxe:ASPxComboBox ID="cboFrequencyTimeline" ClientInstanceName="cboFrequencyTimeline"
                                                            runat="server" Width="100%">
                                                            <ClientSideEvents SelectedIndexChanged="function(s,e){cboFrequencyTimelineChanged()}" />
                                                        </dxe:ASPxComboBox>
                                                    </td>
                                                    <td style="width: 80px">
                                                        <asp:TextBox ID="txtDosingDose" runat="server" Width="100%" CssClass="number" />
                                                    </td>
                                                    <td style="width: 3px">
                                                        &nbsp;
                                                    </td>
                                                    <td style="width: 20%">
                                                        <dxe:ASPxComboBox ID="cboDosingUnit" ClientInstanceName="cboDosingUnit" runat="server"
                                                        Width="100%" OnCallback="cboDosingUnit_Callback">
                                                        <ClientSideEvents EndCallback="onCboDosingUnitEndCallback" SelectedIndexChanged="function(s,e){cboDosingUnitChanged()}" />
                                                        </dxe:ASPxComboBox>
                                                    </td>
                                                    <td>
                                                        &nbsp;
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tdLabel">
                                            <label class="lblMandatory">
                                                <%=GetLabel("Duration (Day)")%></label>
                                        </td>
                                        <td>
                                            <table style="width: 100%" cellpadding="0" cellspacing="0">
                                                <colgroup>
                                                    <col style="width: 9px" />
                                                    <col style="width: 100px" />
                                                    <col />
                                                </colgroup>
                                                <tr>
                                                    <td>
                                                        <asp:TextBox runat="server" ID="txtDosingDuration" Width="100%" CssClass="number" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tdLabel">
                                            <label class="lblNormal">
                                                <%=GetLabel("Quantity Total")%></label>
                                        </td>
                                        <td>
                                            <table style="width: 100%" cellpadding="0" cellspacing="0">
                                                <colgroup>
                                                    <col style="width: 80px" />
                                                    <col style="width: 3px" />
                                                    <col style="width: 100px" />
                                                    <col style="width: 20px" />
                                                    <col style="width: 20%" />
                                                    <col />
                                                </colgroup>
                                                <tr>
                                                    <td>
                                                        <asp:TextBox runat="server" ID="txtDispenseQty" Width="100%" CssClass="number" />
                                                    </td>
                                                    <td>
                                                        &nbsp;
                                                    </td>
                                                    <td>
                                                        <asp:TextBox runat="server" ID="txtDispenseUnit" Width="100%" ReadOnly="true" TabIndex="999" />
                                                    </td>
                                                    <td>
                                                        &nbsp;
                                                    </td>
                                                    <td style="padding-left: 5px">
                                                        <asp:CheckBox runat="server" ID="chkIsUsingSweetener" /><%=GetLabel("Sweetener")%>
                                                    </td>
                                                    <td>
                                                        <asp:CheckBox ID="chkIsAsRequired" runat="server" Text="As Required" Checked="false" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tdLabel">
                                            <label class="lblMandatory">
                                                <%=GetLabel("Medication Route")%></label>
                                        </td>
                                        <td>
                                            <table style="width: 100%" cellpadding="0" cellspacing="0">
                                                <colgroup>
                                                    <col style="width: 10%" />
                                                    <col style="width: 3%" />
                                                    <col style="width: 1%" />
                                                    <col style="width: 5%" />
                                                    <col style="width: 20%" />
                                                </colgroup>
                                                <tr>
                                                    <td>
                                                        <dxe:ASPxComboBox runat="server" ID="cboMedicationRoute" ClientInstanceName="cboMedicationRoute"
                                                            Width="100%" />
                                                    </td>
                                                    <td>
                                                        &nbsp;
                                                    </td>
                                                    <td style="padding-left: 5px" align="right">
                                                        <label class="lblNormal">
                                                         <%=GetLabel("AC/DC/PC")%></label>
                                                    </td>
                                                    <td style="padding-left: 5px" align="center">
                                                        <dxe:ASPxComboBox runat="server" ID="cboCoenamRule" ClientInstanceName="cboCoenamRule"
                                                            Width="100%" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr id="trTakenTime" runat="server">
                                        <td class="tdLabel">
                                            <label class="lblNormal">
                                                <%=GetLabel("Taken Time")%></label>
                                        </td>
                                        <td>
                                            <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                                <tr>
                                                    <td style="width: 15%">
                                                        <asp:CheckBox ID="chkIsMorning" runat="server" Text="Morning" Checked="false" />
                                                    </td>
                                                    <td style="width: 15%">
                                                        <asp:CheckBox ID="chkIsNoon" runat="server" Text="Noon" Checked="false" />
                                                    </td>
                                                    <td style="width: 15%">
                                                        <asp:CheckBox ID="chkIsEvening" runat="server" Text="Evening" Checked="false" />
                                                    </td>
                                                    <td>
                                                        <asp:CheckBox ID="chkIsNight" runat="server" Text="Night" Checked="false" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tdLabel" style="vertical-align: top; padding-top: 5px;">
                                            <label class="lblNormal">
                                                <%=GetLabel("Administration Instruction")%></label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtMedicationAdministration" Width="540px" runat="server" TextMode="MultiLine" />
                                        </td>
                                    </tr>
                                    <tr>
                                    <td>
                                        <table>
                                            <tr>
                                                <td colspan="3">
                                                    <table>
                                                        <tr>
                                                            <td>
                                                                <input style="width: 70px; text-align: center" type="button" id="btnEntryPopupSave" value='<%= GetLabel("Simpan")%>' />
                                                            </td>
                                                            <td>
                                                                <input style="width: 70px; text-align: center" type="button" id="btnEntryPopupCancel" value='<%= GetLabel("Batal")%>' />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </table>
                            </fieldset>
                        </div>
                        <div id="divlvwView" style="position: relative;">
                            <dxcp:ASPxCallbackPanel ID="cbpViewDt" runat="server" ClientInstanceName="cbpViewDt"
                                OnCallback="cbpViewDt_Callback" ShowLoadingPanel="false">
                                <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingViewDt').show(); }"
                                    EndCallback="function(s,e){ onCbpViewDtEndCallback(s); }" />
                                  <PanelCollection>
                                    <dx:PanelContent ID="PanelContent2" runat="server">
                                        <asp:Panel runat="server" ID="pnlTemplateDt" Style="width: 100%; margin-left: auto; margin-right: auto;
                                            position: relative; font-size: 0.95em;">
                                            <asp:ListView runat="server" ID="lvwView" OnItemDataBound="lvwView_ItemDataBound">
                                                <EmptyDataTemplate>
                                                    <table id="tblView" runat="server" class="grdView notAllowSelect grdTemplateDt"
                                                        cellspacing="0" rules="all">
                                                        <tr>
                                                            <th style="width: 70px">
                                                                &nbsp;
                                                            </th>
                                                            <th align="Left">
                                                                <%=GetLabel("NAMA OBAT")%>
                                                            </th>
                                                            <th style="width: 60px" align="center">
                                                                <%=GetLabel("FREQUENCY")%>
                                                            </th>
                                                            <th style="width: 60px" align="left">
                                                                <%=GetLabel("TIMELINE")%>
                                                            </th>
                                                            <th style="width: 60px" align="right">
                                                                <%=GetLabel("DOSE")%>
                                                            </th>
                                                            <th style="width: 60px" align="left">
                                                                <%=GetLabel("UNIT")%>
                                                            </th>
                                                            <th style="width: 60px" align="left">
                                                                <%=GetLabel("ROUTE")%>
                                                            </th>
                                                            <th align="left">
                                                                <%=GetLabel("SIGNA")%>
                                                            </th>
                                                            <th style="width: 60px" align="right">
                                                                <%=GetLabel("QUANTITY")%>
                                                            </th>
                                                        </tr>
                                                        <tr class="trEmpty">
                                                            <td colspan="11">
                                                                <%=GetLabel("Belum ada obat yang ditambahkan")%>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </EmptyDataTemplate>
                                                <LayoutTemplate>
                                                    <table id="tblView" runat="server" class="grdView notAllowSelect grdTemplateDt"
                                                        cellspacing="0" rules="all">
                                                        <tr>
                                                            <th style="width: 70px" >
                                                                &nbsp;
                                                            </th>
                                                            <th style="width: 25%" align="Left">
                                                                <%=GetLabel("NAMA OBAT")%>
                                                            </th>
                                                            <th style="width: 60px" align="center">
                                                                <%=GetLabel("FREQUENCY")%>
                                                            </th>
                                                            <th style="width: 60px" align="left">
                                                                <%=GetLabel("TIMELINE")%>
                                                            </th>
                                                            <th style="width: 60px"  align="right">
                                                                <%=GetLabel("DOSE")%>
                                                            </th>
                                                            <th style="width: 60px" align="left">
                                                                <%=GetLabel("UNIT")%>
                                                            </th>
                                                            <th style="width: 60px" align="left">
                                                                <%=GetLabel("ROUTE")%>
                                                            </th>
                                                            <th align="left">
                                                                <%=GetLabel("SIGNA")%>
                                                            </th>
                                                            <th style="width: 60px" align="right">
                                                                <%=GetLabel("QUANTITY")%>
                                                            </th>
                                                        </tr>
                                                        <tr runat="server" id="itemPlaceholder">
                                                        </tr>
                                                    </table>
                                                </LayoutTemplate>
                                                <ItemTemplate>
                                                    <tr>
                                                        <td align="center">
                                                            <img class="imgEdit imgLink" src='<%# ResolveUrl("~/Libs/Images/Button/edit.png")%>'
                                                                alt="" style="float: left; margin-left: 7px" />
                                                            <img class="imgDelete imgLink" src='<%# ResolveUrl("~/Libs/Images/Button/delete.png")%>'
                                                                alt="" style="margin-left: 2px" />
                                                            <input type="hidden" class="hdnPrescriptionTemplateID" value="<%#: Eval("PrescriptionTemplateID")%>" />
                                                            <input type="hidden" class="hdnPrescriptionTemplateDetailID" value="<%#: Eval("PrescriptionTemplateDetailID")%>" />
                                                            <input type="hidden" class="hdnItemID" value="<%#: Eval("ItemID")%>" />
                                                            <input type="hidden" class="hdnFrequency" value="<%#: Eval("Frequency")%>" />
                                                            <input type="hidden" class="hdnItemCode" value="<%#: Eval("ItemCode")%>" />
                                                            <input type="hidden" class="hdnDrugName" value="<%#: Eval("DrugName")%>" />
                                                            <input type="hidden" class="hdnTakenQty" value="<%#: Eval("TakenQty")%>" />
                                                            <input type="hidden" class="hdnItemUnit" value="<%#: Eval("ItemUnit")%>" />
                                                            <input type="hidden" class="hdnDosingDuration" value="<%#: Eval("DosingDuration")%>" />
                                                            <input type="hidden" class="hdnNumberOfDosage" value="<%#: Eval("cfNumberOfDosage")%>" />
                                                            <input type="hidden" class="hdnGCDosingUnit" value="<%#: Eval("GCDosingUnit")%>" />
                                                            <input type="hidden" class="hdnGCMedicationRoute" value="<%#: Eval("GCRoute")%>" />
                                                            <input type="hidden" class="hdnStrengthAmount" value="<%#: Eval("Dose")%>" />
                                                            <input type="hidden" class="hdnStrengthUnit" value="<%#: Eval("DoseUnit")%>" />
                                                            <input type="hidden" class="hdnIsMorning" value="<%#: Eval("IsMorning")%>" />
                                                            <input type="hidden" class="hdnIsNoon" value="<%#: Eval("IsNoon")%>" />
                                                            <input type="hidden" class="hdnIsEvening" value="<%#: Eval("IsEvening")%>" />
                                                            <input type="hidden" class="hdnIsNight" value="<%#: Eval("IsNight")%>" />
                                                            <input type="hidden" class="hdnIsAsRequired" value="<%#: Eval("IsAsRequired")%>" />
                                                            <input type="hidden" class="hdnMedicationAdministration" value="<%#: Eval("MedicationAdministration")%>" />
                                                            <input type="hidden" class="hdnGCCoenamRule" value="<%#: Eval("GCCoenamRule")%>" />
                                                            <input type="hidden" class="hdnIsCompound" value="<%#: Eval("IsCompound")%>" />
                                                        </td>
                                                        <td class="tdDrugName">
                                                             <div>
                                                                <%#: Eval("cfMedicationName")%></div>
                                                             <div style='<%# Eval("IsCompound").ToString() == "False" ? "display:none;": "white-space: pre-line;font-style:italic; padding-top:5px" %>'>
                                                                <%#: Eval("cfCompoundDetail")%></div>
                                                        </td>
                                                        <td class="tdFrequency" align="center">
                                                            <%#: Eval("Frequency")%>
                                                        </td>
                                                        <td class="tdTimeline">
                                                            <%#: Eval("DosingFrequency")%>
                                                        </td>
                                                        <td class="tdDose" align="center">
                                                            <%#: Eval("cfNumberOfDosage")%>
                                                        </td>
                                                        <td class="tdUnit" >
                                                            <%#: Eval("DosingUnit")%>
                                                        </td>
                                                        <td class="tdRoute" >
                                                            <%#: Eval("Route")%>
                                                        </td>
                                                        <td class="tdSigna" >
                                                            <div>
                                                                <%#: Eval("cfConsumeMethod2")%></div>
                                                            <div>
                                                                <%#: Eval("MedicationAdministration")%></div>
                                                        </td>
                                                        <td class="tdQuantity" align="center">
                                                            <%#: Eval("DispenseQtyInString")%>
                                                        </td>
                                                    </tr>
                                                </ItemTemplate>
                                            </asp:ListView>
                                        </asp:Panel>
                                    </dx:PanelContent>
                                </PanelCollection>
                            </dxcp:ASPxCallbackPanel>
                            <div style="width: 100%; text-align: center" id="divContainerAddData" runat="server">
                                <span class="lblLink" id="lblEntryPopupAddData" style="margin-right: 200px;">
                                    <%= GetLabel("Tambah Data")%></span>
                                <span class="lblLink" id="lblEntryPopupAddCompoundData">
                                    <%= GetLabel("Tambah Racikan")%></span>
                            </div>
                            <div class="imgLoadingGrdView" id="containerImgLoadingViewDt">
                                <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                            </div>
                            <div class="containerPaging">
                                <div class="wrapperPaging">
                                    <div id="pagingDt">
                                    </div>
                                </div>
                            </div>
                        </div>
                    </td>
                </tr>
            </table>
        </tr>
    </div>
</asp:Content>
