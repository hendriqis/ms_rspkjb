<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MedicationQuickPicksTemplateCtl1.ascx.cs"
    Inherits="QIS.Medinfras.Web.EMR.Program.MedicationQuickPicksTemplateCtl1" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="QIS.Medinfras.Web.CustomControl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"
    Namespace="QIS.Medinfras.Web.CustomControl" TagPrefix="qis" %>
<style type="text/css">
    .trSelectedItem
    {
        background-color: #ecf0f1 !important;
    }
</style>
<script type="text/javascript" id="dxss_drugsquickpicksHistoryCtl1">
    function addItemFilterRow() {
        $trHeader = $('#<%=grdView.ClientID %> tr:eq(0)');
        $trFilter = $("<tr><td></td><td></td></tr>");

        $input = $("<input type='text' id='txtFilterItem' style='width:100%;height:20px' />").val($('#<%=hdnFilterItem.ClientID %>').val());
        $trFilter.find('td').eq(1).append($input);
        $trFilter.insertAfter($trHeader);
    }

    $('#txtFilterItem').live('keypress', function (e) {
        var code = (e.keyCode ? e.keyCode : e.which);
        if (code == 13) {
            getCheckedMember();
            $('#<%=hdnFilterItem.ClientID %>').val($(this).val());
            e.preventDefault();
            cbpPopup.PerformCallback('refresh');
        }
    });

    function onBeforeSaveRecord(errMessage) {
        if (IsValid(null, 'fsDrugsQuickPicks', 'mpDrugsQuickPicks')) {
            getCheckedMember();
            if ($('#<%=hdnSelectedMember.ClientID %>').val() != '') {
                var lstItemNoStock = $('#<%=hdnSelectedItemNoStockTemplate.ClientID %>').val();
                var isValidate = $('#<%=hdnValidationEmptyStockCtlTemplate.ClientID %>').val();
                if (isValidate == '0' && lstItemNoStock != '') {
                    var lstItemNoStockSplit = lstItemNoStock.Split('<BR>');
                    var lstItemNoStock1 = [];
                    var lstItemNoStock2 = [];
                    for (var i = 0; i < lstItemNoStock.length; i++) {
                        var countData = 0;

                        for (var x = 0; x < lstItemNoStock.length; x++) {
                            if (lstItemNoStock[i] == lstItemNoStock[x]) {
                                countData += 1;
                            }
                        }

                        if (countData > 1) {
                            lstItemNoStock1.push(lstItemNoStock[i]);
                        }
                        else {
                            lstItemNoStock2.push(lstItemNoStock[i]);
                        }
                    }

                    lstItemNoStock1.concat(lstItemNoStock2)
                    $('#<%=hdnSelectedItemNoStockTemplate.ClientID %>').val(lstItemNoStock1.join('<BR>'));
                    lstItemNoStock = $('#<%=hdnSelectedItemNoStockTemplate.ClientID %>').val();

                    errMessage.text = 'showconfirm|<b>Item - item berikut ini tidak memiliki stok : </b><br>' + lstItemNoStock + '<br><b>Lanjutkan proses ?</b>';
                    return false;
                }
                else {
                    return true;
                }
            }
            else {
                errMessage.text = 'Please Select Item First';
                return false;
            }
        }
    }

    function onBeforeSaveRecordEntryPopup(errMessage) {
        getCheckedMember();
        if ($('#<%=hdnSelectedMember.ClientID %>').val() != '') {
            var lstItemNoStock = $('#<%=hdnSelectedItemNoStockTemplate.ClientID %>').val();
            var isValidate = $('#<%=hdnValidationEmptyStockCtlTemplate.ClientID %>').val();
            if (isValidate == '0' && lstItemNoStock != '') {
                var lstItemNoStockSplit = lstItemNoStock.split('<BR>');
                var lstItemNoStock1 = [];
                var lstItemNoStock2 = [];
                for (var i = 0; i < lstItemNoStockSplit.length; i++) {
                    var countData = 0;

                    for (var x = 0; x < lstItemNoStockSplit.length; x++) {
                        if (lstItemNoStockSplit[i] == lstItemNoStockSplit[x]) {
                            countData += 1;
                        }
                    }

                    if (countData > 1) {
                        var ishasData = 0;
                        for (var x = 0; x < lstItemNoStock1.length; x++) {
                            if (lstItemNoStock1[x] == lstItemNoStockSplit[i]) {
                                ishasData = 1;
                            }
                        }

                        if (ishasData == 0) {
                            lstItemNoStock1.push(lstItemNoStockSplit[i]);
                        }
                    }
                    else {
                        lstItemNoStock2.push(lstItemNoStockSplit[i]);
                    }
                }

                $('#<%=hdnSelectedItemNoStockTemplate.ClientID %>').val(lstItemNoStock1.concat(lstItemNoStock2).join('<BR>'));
                lstItemNoStock = $('#<%=hdnSelectedItemNoStockTemplate.ClientID %>').val();

                errMessage.text = 'showconfirm|<b>Item - item berikut ini tidak memiliki stok : </b><br>' + lstItemNoStock + '<br><b>Lanjutkan proses ?</b>';
                return false;
            }
            else {
                return true;
            }
        }
        else {
            errMessage.text = 'Please Select Item First';
            return false;
        }
    }

    $('#chkSelectAll').die('change');
    $('#chkSelectAll').live('change', function () {
        var isChecked = $(this).is(":checked");
        $('.chkIsSelected').each(function () {
            $(this).find('input').prop('checked', isChecked);
        });
    });

    function getCheckedMember() {
        var lstSelectedMember = [];
        var lstSelectedMemberFrequency = [];
        var lstSelectedMemberCoenam = [];
        var lstSelectedMemberQty = [];
        var lstSelectedMemberPrn = [];
        var lstSelectedMemberDispenseQty = [];
        var lstSelectedMemberRemarks = [];
        var lstSelectedMemberIsUsingUDD = [];
        var lstSelectedMemberRoute = [];
        var lstSelectedItemNoStock = [];

        var result = '';
        $('.grdSelected .chkIsSelected input:checked').each(function () {
            var $tr = $(this).closest('tr');
            var key = $tr.find('.keyField').val();
            var frequency = $tr.find('.txtFrequency').val();
            var qty = $tr.find('.txtQty').val();
            var dispenseQty = $tr.find('.txtDispenseQty').val();
            var itemName1 = $tr.find('.itemName').val();

            var filter = "PrescriptionTemplateDetailID IN ('" + key + "') OR ParentID IN ('" + key + "')";
            Methods.getListObject('GetPrescriptionTemplateDtList', filter, function (result) {
                for (i = 0; i < result.length; i++) {
                    var locationID = cboPopupLocation.GetValue();
                    var filterDetail = "ItemID = '" + result[i].ItemID + "' AND LocationID = '" + locationID + "' AND IsDeleted = 0 AND QuantityEND > 0";
                    Methods.getObject('GetItemBalanceList', filterDetail, function (resultDetail) {
                        if (resultDetail == null) {
                            lstSelectedItemNoStock.push(itemName1);
                        }
                    });
                }
            });

            //            var isPRN = '0';
            //            if ($(this).find('.chkPrn').is(':checked')) {
            //                isPRN = '1';
            //            }

            var medicationAdministration = $tr.find('.txtMedicationAdministration').val();

            lstSelectedMember.push(key);
            lstSelectedMemberFrequency.push(frequency);
            //lstSelectedMemberPrn.push(isPRN);
            lstSelectedMemberQty.push(qty);
            lstSelectedMemberDispenseQty.push(dispenseQty);
            lstSelectedMemberRemarks.push(medicationAdministration);
        });
        $('#<%=hdnSelectedMember.ClientID %>').val(lstSelectedMember.join(','));
        $('#<%=hdnSelectedMemberFrequency.ClientID %>').val(lstSelectedMemberFrequency.join(','));
        $('#<%=hdnSelectedMemberQty.ClientID %>').val(lstSelectedMemberQty.join(','));
        $('#<%=hdnSelectedMemberDispenseQty.ClientID %>').val(lstSelectedMemberDispenseQty.join(','));
        $('#<%=hdnSelectedMemberRemarks.ClientID %>').val(lstSelectedMemberRemarks.join('|'));
        $('#<%=hdnSelectedItemNoStockTemplate.ClientID %>').val(lstSelectedItemNoStock.join('<BR>'));
    }

    //#region Paging
    var pageCount = parseInt('<%=PageCount %>');

    $(function () {
        hideLoadingPanel();
        setDatePicker('<%=txtDefaultStartDate.ClientID %>');
        $('#<%=txtDefaultStartDate.ClientID %>').datepicker('option', 'minDate', '0');
        $('#<%=txtDefaultStartDate.ClientID %>').val(getDateNowDatePickerFormat());

        setPaging($("#pagingPopup"), pageCount, function (page) {
            RefreshGrid(true, page);
        });


        $('#<%=grdView.ClientID %> > tbody > tr:gt(0):not(.trEmpty)').live('click', function () {
            if ($(this).attr('class') != 'selected') {
                $('#<%=grdView.ClientID %> tr.selected').removeClass('selected');
                $(this).addClass('selected');
                $('#<%=hdnSelectedVisitID.ClientID %>').val($(this).find('.keyField').html());

                if (typeof (grdPopupViewDt) != 'undefined' && typeof (cbpPopupViewDt) != 'undefined')
                    cbpPopupViewDt.PerformCallback('refresh');
                else
                    window.setTimeout("cbpPopupViewDt.PerformCallback('refresh');", 100);
            }
        });

        $('#<%=rblItemType.ClientID %> input').change(function () {
            RefreshGrid(true, 1);
        });

        $('#<%=grdView.ClientID %> tr:eq(1)').click();
    });

    function RefreshGrid(mode, pageNo) {
        getCheckedMember();
        if ($('#<%=hdnSelectedMember.ClientID %>').val() != '')
            PromptUserBeforeRefresh(mode, pageNo);
        else
            cbpPopup.PerformCallback('changepage|' + pageNo);
    }

    function PromptUserBeforeRefresh(mode, pageNo) {
        displayConfirmationMessageBox('QUICK PICKS : ORDER', 'Ada item yang telah dipilih, aksi anda akan mereset pilihan tersebut, dilanjutkan ?', function (result) {
            if (result) {
                if (mode == false)
                    cbpPopup.PerformCallback('refresh');
                else
                    cbpPopup.PerformCallback('changepage|' + pageNo);
            }
        });
    }

    function onCbpPopupEndCallback(s) {
        hideLoadingPanel();

        var param = s.cpResult.split('|');
        if (param[0] == 'refresh') {
            var pageCount = parseInt(param[1]);
            setPaging($("#pagingPopup"), pageCount, function (page) {
                RefreshGrid(true, page);
            });
            $('#<%=grdView.ClientID %> tr:eq(1)').click();
        }
    }
    //#endregion

    function onRefreshGrid() {
        $('#<%=hdnFilterExpressionQuickSearch.ClientID %>').val(txtSearchView.GenerateFilterExpression());
        cbpPopup.PerformCallback('refresh');
    }

    function onTxtSearchViewSearchClick(s) {
        setTimeout(function () {
            s.SetBlur();
            onRefreshGrid();
        }, 0);
    }

    //#region Paging Dt
    function onCbpPopupViewDtEndCallback(s) {
        $('#containerImgLoadingViewDt').hide();

        var param = s.cpResult.split('|');
        if (param[0] == 'refresh') {
            var pageCount = parseInt(param[1]);
            if (pageCount > 0)
                $('#<%=grdPopupViewDt.ClientID %> tr:eq(1)').click();

            setPaging($("#pagingDt1"), pageCount, function (page) {
                cbpViewDt.PerformCallback('changepage|' + page);
            });
        }
        else
            $('#<%=grdPopupViewDt.ClientID %> tr:eq(1)').click();
    }
    //#endregion

    function onAfterSaveRecordPatientPageEntry(value) {
        if (typeof onAfterSaveDetail == 'function')
            onAfterSaveDetail(value);
    }
</script>
<div style="padding: 1px;">
    <input type="hidden" id="hdnSelectedMember" runat="server" value="" />
    <input type="hidden" value="" id="hdnFilterExpressionQuickSearch" runat="server" />
    <input type="hidden" id="hdnSelectedItemNoStockTemplate" runat="server" value="" />
    <input type="hidden" id="hdnOrderID" runat="server" value="" />
    <input type="hidden" id="hdnTransactionID" runat="server" value="" />
    <input type="hidden" id="hdnRegistrationID" runat="server" value="" />
    <input type="hidden" id="hdnVisitID" runat="server" value="" />
    <input type="hidden" id="hdnSelectedVisitID" runat="server" value="" />
    <input type="hidden" id="hdnChargeClassID" runat="server" value="" />
    <input type="hidden" id="hdnDefaultGCMedicationRoute" runat="server" value="" />
    <input type="hidden" id="hdnParamedicID" runat="server" value="" />
    <input type="hidden" id="hdnDispensaryUnitID" runat="server" value="" />
    <input type="hidden" id="hdnPrescriptionType" runat="server" value="" />
    <input type="hidden" id="hdnPrescriptionDate" value="" runat="server" />
    <input type="hidden" id="hdnPrescriptionTime" value="" runat="server" />
    <input type="hidden" id="hdnPrescriptionValidateStockAllRS" value="" runat="server" />
    <input type="hidden" id="hdnParam" runat="server" value="" />
    <input type="hidden" id="hdnFilterItem" runat="server" />
    <input type="hidden" id="hdnSelectedMemberOrderInfo" runat="server" value="" />
    <input type="hidden" id="hdnSelectedMemberStrength" runat="server" value="" />
    <input type="hidden" id="hdnSelectedMemberFrequency" runat="server" value="" />
    <input type="hidden" id="hdnSelectedMemberPRN" runat="server" value="" />
    <input type="hidden" id="hdnSelectedMemberCoenam" runat="server" value="" />
    <input type="hidden" id="hdnSelectedMemberQty" runat="server" value="" />
    <input type="hidden" id="hdnSelectedMemberDispenseQty" runat="server" value="" />
    <input type="hidden" id="hdnSelectedMemberTakenQty" runat="server" value="" />
    <input type="hidden" id="hdnSelectedMemberRemarks" runat="server" value="" />
    <input type="hidden" id="hdnSelectedMemberStartTime" runat="server" value="" />
    <input type="hidden" id="hdnSelectedMemberIsUsingUDD" runat="server" value="" />
    <input type="hidden" id="hdnSelectedMemberRoute" runat="server" value="" />
    <input type="hidden" id="hdnLocationID" runat="server" value="" />
    <input type="hidden" value="" id="hdnPrescriptionFeeAmount" runat="server" />
    <input type="hidden" id="hdnIsAutoGenerateReferenceNo" value="0" runat="server" />
    <input type="hidden" id="hdnIsGenerateQueueLabel" value="0" runat="server" />
    <input type="hidden" id="hdnItemQtyWithSpecialQueuePrefix" value="0" runat="server" />
    <input type="hidden" id="hdnValidationEmptyStockCtlTemplate" value="" runat="server" />
    <table style="width: 100%">
        <colgroup>
            <col style="width: 35%" />
            <col style="width: 65%" />
        </colgroup>
        <tr>
            <td style="padding: 2px; vertical-align: top">
                <table cellspacing="0" width="100%">
                    <colgroup>
                        <col style="width: 120px" />
                        <col style="width: 100px" />
                        <col style="width: 250px" />
                        <col />
                    </colgroup>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblMandatory">
                                <%=GetLabel("Lokasi Farmasi")%></label>
                        </td>
                        <td colspan="2">
                            <dxe:ASPxComboBox ID="cboPopupLocation" ClientInstanceName="cboPopupLocation" Width="100%"
                                runat="server" OnCallback="cboPopupLocation_Callback">
                                <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ hideLoadingPanel(); }" />
                            </dxe:ASPxComboBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal" id="Label2">
                                <%=GetLabel("Filter Obat")%></label>
                        </td>
                        <td colspan="2">
                            <asp:RadioButtonList ID="rblItemType" runat="server" RepeatDirection="Horizontal">
                                <asp:ListItem Text="All" Value="0" Selected="True" />
                                <asp:ListItem Text="Formularium" Value="1" />
                                <asp:ListItem Text="BPJS" Value="2" />
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Tanggal Dimulai")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtDefaultStartDate" runat="server" Width="120px" CssClass="datepicker" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel" valign="top" style="padding-top: 5px;">
                            <label id="lblAllergy" runat="server">
                                <%=GetLabel("Alergi Pasien") %></label>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtAllergyInfo" runat="server" Width="100%" TextMode="Multiline"
                                Rows="2" ReadOnly="true" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label>
                                <%=GetLabel("Quick Filter")%></label>
                        </td>
                        <td colspan="2">
                            <qis:QISIntellisenseTextBox runat="server" ClientInstanceName="txtSearchView" ID="txtSearchView"
                                Width="100%" Watermark="Search">
                                <ClientSideEvents SearchClick="function(s){ onTxtSearchViewSearchClick(s); }" />
                                <IntellisenseHints>
                                    <qis:QISIntellisenseHint Text="Nama Template" FieldName="PrescriptionTemplateName" />
                                    <qis:QISIntellisenseHint Text="Kode Template" FieldName="PrescriptionTemplateCode" />
                                </IntellisenseHints>
                            </qis:QISIntellisenseTextBox>
                        </td>
                    </tr>
                </table>
                <dxcp:ASPxCallbackPanel ID="cbpPopup" runat="server" Width="100%" ClientInstanceName="cbpPopup"
                    ShowLoadingPanel="false" OnCallback="cbpPopup_Callback">
                    <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel();}" EndCallback="function(s,e){ onCbpPopupEndCallback(s); }" />
                    <PanelCollection>
                        <dx:PanelContent ID="PanelContent1" runat="server" CssClass="pnlContainerGridPatientPage">
                            <asp:Panel runat="server" ID="pnlView" Style="width: 100%; margin-top: 20px; margin-left: auto;
                                margin-right: auto; position: relative; font-size: 0.95em;">
                                <asp:GridView ID="grdView" runat="server" CssClass="grdSelected grdPatientPage" AutoGenerateColumns="false"
                                    ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty" OnRowDataBound="grdView_RowDataBound">
                                    <Columns>
                                        <asp:BoundField DataField="PrescriptionTemplateID" HeaderStyle-CssClass="keyField"
                                            ItemStyle-CssClass="keyField" />
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                <%=GetLabel("TEMPLATE")%>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <div>
                                                    <b><span style="color: Blue">
                                                        <%#: Eval("PrescriptionTemplateCode")%></span></b></div>
                                                <div>
                                                    <%#: Eval("PrescriptionTemplateName")%></div>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <EmptyDataTemplate>
                                        <%=GetLabel("Tidak ada informasi template peresepan untuk dokter")%>
                                    </EmptyDataTemplate>
                                </asp:GridView>
                            </asp:Panel>
                        </dx:PanelContent>
                    </PanelCollection>
                </dxcp:ASPxCallbackPanel>
                <div class="imgLoadingGrdView" id="containerImgLoadingView1">
                    <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                </div>
                <div class="containerPaging">
                    <div class="wrapperPaging">
                        <div id="pagingPopup">
                        </div>
                    </div>
                </div>
            </td>
            <td style="padding: 2px; vertical-align: top">
                <div style="position: relative;">
                    <dxcp:ASPxCallbackPanel ID="cbpPopupViewDt" runat="server" Width="100%" ClientInstanceName="cbpPopupViewDt"
                        ShowLoadingPanel="false" OnCallback="cbpViewDt_Callback">
                        <ClientSideEvents BeginCallback="function(s,e){$('#containerImgLoadingViewDt').show(); }"
                            EndCallback="function(s,e){ onCbpPopupViewDtEndCallback(s); }" />
                        <PanelCollection>
                            <dx:PanelContent ID="PanelContent2" runat="server">
                                <asp:Panel runat="server" ID="Panel1" CssClass="pnlContainerGridPatientPage">
                                    <asp:GridView ID="grdPopupViewDt" runat="server" CssClass="grdSelected grdPatientPage"
                                        AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty" OnRowDataBound="grdPopupViewDt_RowDataBound" >
                                        <Columns>
                                            <asp:TemplateField HeaderStyle-Width="40px" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                <HeaderTemplate>
                                                    <input id="chkSelectAll" type="checkbox" />
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <input type="hidden" class="keyField" value='<%#:Eval("PrescriptionTemplateDetailID")%>' />
                                                    <input type="hidden" class="itemName" value='<%#:Eval("cfMedicationName")%>' />
                                                    <asp:CheckBox ID="chkIsSelected" runat="server" CssClass="chkIsSelected" />
                                                    <div><img id="imgIsNotAvailable" runat="server" class="blink-icon" src='<%# ResolveUrl("~/Libs/Images/Status/alert.png") %>' title='Obat/Komponen racikan obat statusnya sudah tidak aktif atau diadakan di RS' alt="" style ="height:24px; width:24px;" /></div>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Left">
                                                <HeaderTemplate>
                                                    <div>
                                                        <%=GetLabel("Item Name")%></div>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <div id="divItemName" runat="server" style="font-weight: bold">
                                                        <span class="itemName">
                                                            <%#: Eval("cfMedicationName")%></span></div>
                                                    <div style='<%# Eval("IsCompound").ToString() == "False" ? "display:none;": "white-space: pre-line;font-style:italic; padding-top:5px" %>'>
                                                        <%#: Eval("cfCompoundDetail")%></div>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-Width="90px" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                HeaderText="Frekuensi">
                                                <ItemTemplate>
                                                    <input type="hidden" value='<%#:Eval("GCDosingFrequency")%>' class="hdnGCItemUnit" />
                                                    <input type="number" value='<%#:Eval("Frequency")%>' validationgroup="mpMedicationOrderHistoryQty"
                                                        class="txtFrequency" min="0" value="0" style="width: 40px; text-align: right" />
                                                    <%#:Eval("DosingFrequency") %>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-Width="90px" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                HeaderText="Dosis">
                                                <ItemTemplate>
                                                    <input type="text" value='<%#:Eval("cfNumberOfDosage")%>' validationgroup="mpMedicationOrderHistoryQty"
                                                        class="txtQty" style="width: 40px; text-align: right" />
                                                    <%#:Eval("DosingUnit") %>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-Width="100px" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                HeaderText="Jumlah Diorder">
                                                <ItemTemplate>
                                                    <input type="hidden" value='<%#:Eval("GCItemUnit")%>' class="hdnGCItemUnit" />
                                                    <input type="number" value='<%#:Eval("DispenseQty")%>' validationgroup="mpMedicationOrderHistoryQty"
                                                        class="txtDispenseQty" min="0" value="0" style="width: 40px; text-align: right" />
                                                    <%#:Eval("ItemUnit") %>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-Width="170px" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                HeaderText="Special Instruction">
                                                <ItemTemplate>
                                                    <input type="text" value='<%#:Eval("MedicationAdministration")%>' validationgroup="mpMedicationOrderHistoryQty"
                                                        class="txtMedicationAdministration" style="width: 100%" />
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
                    <div class="imgLoadingGrdView" id="containerImgLoadingViewDt">
                        <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                    </div>
                    <div class="containerPaging">
                        <div class="wrapperPaging">
                            <div id="pagingDt1">
                            </div>
                        </div>
                    </div>
                </div>
            </td>
        </tr>
    </table>
</div>
