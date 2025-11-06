<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TestOrderItemQuickPicksCtl1.ascx.cs"
    Inherits="QIS.Medinfras.Web.EMR.Program.TestOrderItemQuickPicksCtl1" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<style type="text/css">
    .trSelectedItem
    {
        background-color: #ecf0f1 !important;
    }
</style>
<script type="text/javascript" id="dxss_drugslogisticsquickpicksctl">
    function addItemFilterRow() {
        $trHeader = $('#<%=grdView.ClientID %> tr:eq(0)');
        $trFilter = $("<tr><td></td><td></td><td></td></tr>");

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

    $(function () {
        hideLoadingPanel();
        addItemFilterRow();
        $('#<%=txtDefaultStartDate.ClientID %>').datepicker('option', 'minDate', '0');
        $('#<%=txtDefaultStartDate.ClientID %>').datepicker('option', 'maxDate', '0');
        $('#<%=txtDefaultStartDate.ClientID %>').val(getDateNowDatePickerFormat());
    });

    $('#chkSelectAll').die('change');
    $('#chkSelectAll').live('change', function () {
        var isChecked = $(this).is(":checked");
        $('.chkIsSelected').each(function () {
            $(this).find('input').prop('checked', isChecked);
            $(this).find('input').trigger("change");
        });
    });

    function onBeforeSaveRecordEntryPopup() {
        //        if (IsValid(null, 'fsDrugsQuickPicks', 'mpDrugsQuickPicks')) {
        getCheckedMember();
        if ($('#<%=hdnSelectedMember.ClientID %>').val() != '')
            return true;
        else {
            displayErrorMessageBox("Order Pemeriksaan", "Belum ada item pemeriksaan yang dipilih");
            return false;
        }
        //        }
        return false;
    }

    function getCheckedMember() {
        var lstSelectedMember = [];
        var lstSelectedMemberName = [];
        var lstSelectedMemberRemarks = [];
        var lstSelectedMemberIsCITO = [];

        var result = '';
        $('#tblSelectedItem .trSelectedItem').each(function () {

            var key = $(this).find('.keyField').val();
            var name = $(this).find('.ItemName').html();
            var isCITO = '0';
            if ($(this).find('.chkCITO').is(':checked')) {
                isCITO = '1';
            }
            var detailRemarks = $(this).find('.txtDetailRemarks').val();

            lstSelectedMember.push(key);
            lstSelectedMemberName.push(name);
            lstSelectedMemberRemarks.push(detailRemarks);
            lstSelectedMemberIsCITO.push(isCITO);
        });
        $('#<%=hdnSelectedMember.ClientID %>').val(lstSelectedMember.join(','));
        $('#<%=hdnSelectedMemberName.ClientID %>').val(lstSelectedMemberName.join('^'));
        $('#<%=hdnSelectedMemberRemarks.ClientID %>').val(lstSelectedMemberRemarks.join('^'));
        $('#<%=hdnSelectedMemberIsCITO.ClientID %>').val(lstSelectedMemberIsCITO.join(','));
    }

    $("#<%:chkIsCITOHeader.ClientID %>").on("change", function (e) {
        if ($('#<%:chkIsCITOHeader.ClientID %>').is(":checked")) {
            $(".chkCITO").removeAttr("disabled");
            $(".chkCITO").prop('checked', true);
        } else {
            $(".chkCITO").removeAttr("disabled");
            $(".chkCITO").prop('checked', false);
        }
    });

    //#region Paging
    var pageCount = parseInt('<%=PageCount %>');

    $(function () {
        setPaging($("#pagingPopup"), pageCount, function (page) {
            getCheckedMember();
            cbpPopup.PerformCallback('changepage|' + page);
        });

        //#region Item Group
        $('#lblItemGroup.lblLink').click(function () {
            var filterExpression = "GCItemType = '" + $('#<%=hdnGCItemType.ClientID %>').val() + "' AND IsDeleted = 0";
            openSearchDialog('itemgroup', filterExpression, function (value) {
                $('#<%=txtItemGroupCode.ClientID %>').val(value);
                onTxtItemGroupCodeChanged(value);
            });
        });

        $('#<%=txtItemGroupCode.ClientID %>').change(function () {
            onTxtItemGroupCodeChanged($(this).val());
        });

        function onTxtItemGroupCodeChanged(value) {
            var filterExpression = "ItemGroupCode = '" + value + "'";
            Methods.getObject('GetItemGroupMasterList', filterExpression, function (result) {
                if (result != null) {
                    $('#<%=hdnItemGroupID.ClientID %>').val(result.ItemGroupID);
                    $('#<%=txtItemGroupName.ClientID %>').val(result.ItemGroupName1);
                }
                else {
                    $('#<%=hdnItemGroupID.ClientID %>').val('');
                    $('#<%=txtItemGroupCode.ClientID %>').val('');
                    $('#<%=txtItemGroupName.ClientID %>').val('');
                }
                getCheckedMember();
                cbpPopup.PerformCallback('refresh');
            });
        }
        //#endregion

        //#region Test Panel
        $('#lblTestPanel.lblLink').click(function () {
            var filterExpression = "GCItemType = '" + $('#<%=hdnGCItemType.ClientID %>').val() + "' AND IsDeleted = 0";
            openSearchDialog('templatepanel', filterExpression, function (value) {
                $('#<%=txtTemplateCode.ClientID %>').val(value);
                onTxtTemplateCodeChanged(value);
            });
        });

        $('#<%=txtTemplateCode.ClientID %>').change(function () {
            onTxtTemplateCodeChanged($(this).val());
        });

        function onTxtTemplateCodeChanged(value) {
            var filterExpression = "TestTemplateCode = '" + value + "'";
            Methods.getObject('GetTestTemplateHdList', filterExpression, function (result) {
                if (result != null) {
                    $('#<%=hdnTestTemplateID.ClientID %>').val(result.TestTemplateID);
                    $('#<%=txtTemplateName.ClientID %>').val(result.TestTemplateName);
                }
                else {
                    $('#<%=hdnTestTemplateID.ClientID %>').val('');
                    $('#<%=txtTemplateCode.ClientID %>').val('');
                    $('#<%=txtTemplateName.ClientID %>').val('');
                }
                getCheckedMember();
                cbpPopup.PerformCallback('refresh');
            });
        }
        //#endregion

        $('#<%=rblItemSource.ClientID %> input').change(function () {
            getCheckedMember();
            cbpPopup.PerformCallback('refresh');
        });

        $('#<%=rblItemType.ClientID %> input').change(function () {
            getCheckedMember();
            cbpPopup.PerformCallback('refresh');
        });
    });

    function onCbpPopupEndCallback(s) {
        hideLoadingPanel();

        var param = s.cpResult.split('|');
        if (param[0] == 'refresh') {
            var pageCount = parseInt(param[1]);
            setPaging($("#pagingPopup"), pageCount, function (page) {
                getCheckedMember();
                cbpPopup.PerformCallback('changepage|' + page);
            });
        }
        addItemFilterRow();
    }
    //#endregion

    $('#<%=txtRemarks.ClientID %>').die('change');
    $('#<%=txtRemarks.ClientID %>').live('change', function () {
        $('#<%=hdnRemarks.ClientID %>').val($(this).val());
    });

    $('#<%=grdView.ClientID %> .chkIsSelected input').die('change');
    $('#<%=grdView.ClientID %> .chkIsSelected input').live('change', function () {
        if ($(this).is(':checked')) {
            $selectedTr = $(this).closest('tr');
            var defaultStartDate = $('#<%=txtDefaultStartDate.ClientID %>').val();
            var defaultStartTime = '08:00';

            var isAllowCito = $selectedTr.find('.IsAllowCito').html();

            $newTr = $('#tmplSelectedTestItem').html();
            $newTr = $newTr.replace(/\$\{ItemName1}/g, $selectedTr.find('.tdItemName1').html());
            $newTr = $newTr.replace(/\$\{ItemID}/g, $selectedTr.find('.keyField').html());
            $newTr = $newTr.replace(/\$\{PreconditionNotes}/g, $selectedTr.find('.hiddenColumn').html());
            $newTr = $($newTr);

            if (isAllowCito == "True")
                $newTr.find(".chkCITO").removeAttr("disabled");
            else
                $newTr.find(".chkCITO").attr("disabled", "disabled");

            $newTr.insertBefore($('#trFooter'));
        }
        else {
            var id = $(this).closest('tr').find('.keyField').html();
            $('#tblSelectedItem tr').each(function () {
                if ($(this).find('.keyField').val() == id) {
                    $(this).remove();
                }
            });
        }
    });

    $('#tblSelectedItem .chkIsSelected2').die('change');
    $('#tblSelectedItem .chkIsSelected2').live('change', function () {
        if ($(this).is(':checked')) {
            $selectedTr = $(this).closest('tr');
            var id = $selectedTr.find('.keyField').val();
            var isFound = false;
            $('#<%=grdView.ClientID %> tr').each(function () {
                if (id == $(this).find('.keyField').html()) {
                    $(this).find('.chkIsSelected').find('input').prop('checked', false);
                    isFound = true;
                }
            });
            if (!isFound) {
                var lstSelectedMember = $('#<%=hdnSelectedMember.ClientID %>').val().split(',');
                lstSelectedMember.splice(lstSelectedMember.indexOf(id), 1);
                $('#<%=hdnSelectedMember.ClientID %>').val(lstSelectedMember.join(','));
            }
            $(this).closest('tr').remove();
        }
    });

    function onAfterSaveRecordPatientPageEntry(value) {
        if ($('#<%=hdnGCItemType.ClientID %>').val() == 'X001^004') {
            if (typeof onRefreshLaboratoryGrid == 'function')
                onRefreshLaboratoryGrid();
            if (value != "") {
                var param = value.split("|");
                if (typeof onAfterAddLabTestOrder == 'function')
                    onAfterAddLabTestOrder(param[1]);

                if (typeof onAfterSaveRecord == 'function')
                    onAfterSaveRecord(value);
            }
        }
        else {
            if (typeof onRefreshImagingGrid == 'function')
                onRefreshImagingGrid();
            if (value != "") {
                var param = value.split("|");
                if (typeof onAfterAddImagingTestOrder == 'function')
                    onAfterAddImagingTestOrder(param[1]);

                if (typeof onAfterSaveRecord == 'function')
                    onAfterSaveRecord(value);
            }
        }
    }

    //#region Physician
    $('#lblQuickPicksPhysician.lblLink').live('click', function () {
        openSearchDialog('paramedic', " GCParamedicMasterType = 'X019^001' AND IsDeleted = 0", function (value) {
            $('#<%=txtParamedicCode.ClientID %>').val(value);
            onTxtQuickPicksPhysicianCodeChanged(value);
        });
    });

    $('#<%=txtParamedicCode.ClientID %>').live('change', function () {
        onTxtQuickPicksPhysicianCodeChanged($(this).val());
    });

    function onTxtQuickPicksPhysicianCodeChanged(value) {
        var filterExpression = "ParamedicCode = '" + value + "'";
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
    function onCboServiceUnitValueChanged() {
        $('#<%=hdnHealthcareServiceUnitID.ClientID %>').val(cboServiceUnit.GetValue());
    }

    $('#<%=txtDefaultStartDate.ClientID %>').live('change', function () {
        var dateOrderItem1 = $('#<%=txtDefaultStartDate.ClientID %>').val();
        var dateToday = $('#<%=hdnDateToday.ClientID %>').val();

        if (dateOrderItem1 != '') {
            var from = dateOrderItem1.split("-");
            var f = new Date(from[2], from[1] - 1, from[0]);

            var to = dateToday.split("-");
            var t = new Date(to[2], to[1] - 1, to[0]);

            if (f < t) {
                $('#<%=txtDefaultStartDate.ClientID %>').val(dateToday);
            }
        }
        else {
            $('#<%=txtDefaultStartDate.ClientID %>').val(dateToday);
        }
    });

    $('#<%=txtDefaultStartTime.ClientID %>').live('change', function () {
        var dateOrderItem1 = $('#<%=txtDefaultStartTime.ClientID %>').val();
        var timeToday = $('#<%=hdnTimeToday.ClientID %>').val();

        if (dateOrderItem1 == '') {
            $('#<%=txtDefaultStartTime.ClientID %>').val(timeToday);
        }
        else {
            var isValid = /^([0-1]?[0-9]|2[0-4]):([0-5][0-9])(:[0-5][0-9])?$/.test(dateOrderItem1);

            if (!isValid) {
                $('#<%=txtDefaultStartTime.ClientID %>').val(timeToday);
            }
        }
    });
</script>
<div style="padding: 10px;">
    <script id="tmplSelectedTestItem" type="text/x-jquery-tmpl">
        <tr class="trSelectedItem">
            <td align="center">
                <input type="checkbox" class="chkIsSelected2" />
                <input type="hidden" class="keyField" value='${ItemID}' />
                <input type="hidden" class="hiddenColumn" value='${PreconditionNotes}' />
            </td>
            <td class="ItemName">${ItemName1}</td>
            <td><input type="text" class="txtDetailRemarks" style="width:200px" value='' /></td>
            <td><input type="checkbox" class="chkCITO" style="width:20px" /></td>
        </tr>
    </script>
    <input type="hidden" id="hdnDateToday" runat="server" value="" />
    <input type="hidden" id="hdnTimeToday" runat="server" value="" />
    <input type="hidden" id="hdnHealthcareServiceUnitID" runat="server" value="" />
    <input type="hidden" id="hdnSelectedMember" runat="server" value="" />
    <input type="hidden" id="hdnSelectedMemberName" runat="server" value="" />
    <input type="hidden" id="hdnSelectedMemberRemarks" runat="server" value="" />
    <input type="hidden" value="" id="hdnDefaultDiagnosa" runat="server" />
    <input type="hidden" value="" id="hdnDefaultChiefComplaint" runat="server" />
    <input type="hidden" value="" id="hdnIsNotesCopyDiagnose" runat="server" />
    <input type="hidden" value="" id="hdnRemarks" runat="server" />
    <input type="hidden" value="" id="hdnDefaultVisitParamedicID" runat="server" />
    <input type="hidden" id="hdnOrderID" runat="server" value="" />
    <input type="hidden" id="hdnTestOrderID" runat="server" value="" />
    <input type="hidden" id="hdnRegistrationID" runat="server" value="" />
    <input type="hidden" id="hdnVisitID" runat="server" value="" />
    <input type="hidden" id="hdnChargeClassID" runat="server" value="" />
    <input type="hidden" id="hdnDefaultGCMedicationRoute" runat="server" value="" />
    <input type="hidden" id="hdnParamedicID" runat="server" value="" />
    <input type="hidden" id="hdnImagingServiceUnitID" runat="server" value="" />
    <input type="hidden" id="hdnLaboratoryServiceUnitID" runat="server" value="" />
    <input type="hidden" id="hdnDispensaryUnitID" runat="server" value="" />
    <input type="hidden" id="hdnPrescriptionType" runat="server" value="" />
    <input type="hidden" id="hdnPrescriptionDate" value="" runat="server" />
    <input type="hidden" id="hdnPrescriptionTime" value="" runat="server" />
    <input type="hidden" id="hdnParam" runat="server" value="" />
    <input type="hidden" id="hdnGCItemType" runat="server" value="" />
    <input type="hidden" id="hdnTestOrderType" runat="server" value="" />
    <input type="hidden" id="hdnFilterItem" runat="server" />
    <input type="hidden" id="hdnSelectedMemberIsCITO" runat="server" value="" />
    <input type="hidden" id="hdnSelectedMemberIsCITODetail" runat="server" value="" />
    <input type="hidden" id="hdnLocationID" runat="server" value="" />
    <input type="hidden" id="hdnItemGroupID" value="" runat="server" />
    <input type="hidden" id="hdnTestTemplateID" value="" runat="server" />
    <input type="hidden" id="hdnBusinessPartnerJKN" value="0" runat="server" />
    <input type="hidden" id="hdnOPMaxLBTestItem" value="0" runat="server" />
    <input type="hidden" id="hdnOPMaxISTestItem" value="0" runat="server" />
    <input type="hidden" id="hdnOPMaxMDTestItem" value="0" runat="server" />
    <input type="hidden" id="hdnDiagnosisSummary" value="0" runat="server" />
    <input type="hidden" id="hdnChiefComplaint" value="0" runat="server" />
    <input type="hidden" id="hdnGCToBePerformed" runat="server" value="" />
    <input type="hidden" id="hdnPostSurgeryInstructionID" runat="server" value="" />
    <input type="hidden" id="hdnIsLimitedCPOEItemForBPJSLab" runat="server" value="" />
    <input type="hidden" id="hdnIsLimitedCPOEItemForBPJSRad" runat="server" value="" />
    <table style="width: 100%">
        <colgroup>
            <col style="width: 40%" />
            <col style="width: 60%" />
        </colgroup>
        <tr>
            <td style="padding: 2px; vertical-align: top">
                <h4>
                    <%=GetLabel("Item Pemeriksaan yang tersedia:")%></h4>
                <table cellspacing="0" width="100%" style="padding-bottom: 5px">
                    <colgroup>
                        <col style="width: 120px" />
                        <col style="width: 100px" />
                        <col style="width: 250px" />
                        <col />
                    </colgroup>
                    <tr>
                        <td class="tdLabel">
                            <%=GetLabel("Unit Pelayanan ") %>
                        </td>
                        <td colspan="3">
                            <dxe:ASPxComboBox ID="cboServiceUnit" ClientInstanceName="cboServiceUnit" Width="100%"
                                runat="server">
                                <ClientSideEvents ValueChanged="function(s,e){ onCboServiceUnitValueChanged(); }"
                                    Init="function(s,e){ onCboServiceUnitValueChanged(); }" />
                            </dxe:ASPxComboBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblLink" id="lblItemGroup">
                                <%=GetLabel("Kelompok")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtItemGroupCode" Width="100%" runat="server" />
                        </td>
                        <td>
                            <asp:TextBox ID="txtItemGroupName" Width="100%" runat="server" ReadOnly="true" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblLink" id="lblTestPanel">
                                <%=GetLabel("Panel")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtTemplateCode" Width="100%" runat="server" />
                        </td>
                        <td>
                            <asp:TextBox ID="txtTemplateName" Width="100%" runat="server" ReadOnly="true" />
                        </td>
                    </tr>
                    <tr style="display: none">
                        <td class="tdLabel">
                            <label class="lblNormal" id="Label1">
                                <%=GetLabel("Source")%></label>
                        </td>
                        <td colspan="2">
                            <asp:RadioButtonList ID="rblItemSource" runat="server" RepeatDirection="Horizontal">
                                <asp:ListItem Text=" Master" Value="1" Selected="True" />
                                <asp:ListItem Text=" History" Value="2" Enabled="false" />
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                    <tr style="display: none">
                        <td class="tdLabel">
                            <label class="lblNormal" id="Label2">
                                <%=GetLabel("Display")%></label>
                        </td>
                        <td colspan="2">
                            <asp:RadioButtonList ID="rblItemType" runat="server" RepeatDirection="Horizontal">
                                <asp:ListItem Text=" All" Value="0" Selected="True" />
                                <%--<asp:ListItem Text="Diagnosa" Value="1" Enabled ="false" />--%>
                                <asp:ListItem Text=" BPJS" Value="2" />
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                </table>
                <dxcp:ASPxCallbackPanel ID="cbpPopup" runat="server" Width="100%" ClientInstanceName="cbpPopup"
                    ShowLoadingPanel="false" OnCallback="cbpPopup_Callback">
                    <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel();}" EndCallback="function(s,e){ onCbpPopupEndCallback(s); }" />
                    <PanelCollection>
                        <dx:PanelContent ID="PanelContent1" runat="server">
                            <asp:Panel runat="server" ID="pnlView" Style="width: 100%; margin-left: auto; margin-right: auto;
                                position: relative; font-size: 0.95em;">
                                <asp:GridView ID="grdView" runat="server" CssClass="grdView notAllowSelect" AutoGenerateColumns="false"
                                    ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty" OnRowDataBound="grdView_RowDataBound">
                                    <Columns>
                                        <asp:BoundField DataField="ItemID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                        <asp:TemplateField HeaderStyle-Width="20px" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                            <HeaderTemplate>
                                                <input id="chkSelectAll" type="checkbox" />
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkIsSelected" runat="server" CssClass="chkIsSelected" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="ItemName1" HeaderText="Nama Pemeriksaan" ItemStyle-CssClass="tdItemName1" />
                                        <asp:TemplateField HeaderStyle-Width="20px" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                            <HeaderTemplate>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <div <%# Eval("PreconditionNotes").ToString() == "" ? "Style='display:none'":"" %>>
                                                    <img id="imgInfo" runat="server" class="blink-icon" src='<%# ResolveUrl("~/Libs/Images/Status/info.png") %>'
                                                        title='<%#Eval("PreconditionNotes") %>' alt="" style="height: 24px; width: 24px;" />
                                                </div>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="IsAllowCito" HeaderStyle-CssClass="IsAllowCito hiddenColumn"
                                            ItemStyle-CssClass="IsAllowCito hiddenColumn" />
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
                        <div id="pagingPopup">
                        </div>
                    </div>
                </div>
            </td>
            <td style="padding: 5px; vertical-align: top">
                <h4>
                    <%=GetLabel("Item Pemeriksaan yang telah dipilih :")%></h4>
                <table cellspacing="0" width="100%">
                    <colgroup>
                        <col width="160px" />
                        <col width="100px" />
                        <col width="170px" />
                        <col width="180px" />
                        <col />
                    </colgroup>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Tanggal - Jam Order")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtDefaultStartDate" runat="server" Width="120px" CssClass="datepicker" />
                        </td>
                        <td>
                            <asp:TextBox ID="txtDefaultStartTime" runat="server" Width="50px" CssClass="time" />
                        </td>
                        <td id="tdIsCITOHeader" runat="server" colspan="2">
                            <asp:CheckBox ID="chkIsCITOHeader" Width="150px" runat="server" Text=" CITO" />
                        </td>
                        <td id="tdPATest" runat="server" colspan="2">
                            <asp:CheckBox ID="chkIsPathologicalAnatomyTest" Width="150px" runat="server" Text=" Pemeriksaan PA" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblLink" id="lblQuickPicksPhysician">
                                <%=GetLabel("Dokter")%></label>
                        </td>
                        <td colspan="5">
                            <table style="width: 100%" cellpadding="0" cellspacing="0">
                                <colgroup>
                                    <col style="width: 120px" />
                                    <col style="width: 3px" />
                                    <col style="width: 600px" />
                                </colgroup>
                                <tr>
                                    <td>
                                        <asp:TextBox ID="txtParamedicCode" Width="100%" runat="server" />
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtParamedicName" Width="100%" runat="server" ReadOnly="true" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <%=GetLabel("Waktu Pengerjaan Order") %>
                        </td>
                        <td>
                            <asp:TextBox ID="txtToBePerformed" runat="server" Width="130px" />
                        </td>
                        <td class="tdLabel">
                            <%=GetLabel("Tanggal - Jam Pengerjaan") %>
                        </td>
                        <td>
                            <table style="width: 100%" cellpadding="0" cellspacing="0">
                                <colgroup>
                                    <col style="width: 110px" />
                                    <col style="width: 9px" />
                                </colgroup>
                                <tr>
                                    <td>
                                        <asp:TextBox ID="txtPerformDateCtl" runat="server" Width="100%" CssClass="datepicker" />
                                    </td>
                                    <td style="width: 5px">
                                        &nbsp
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtPerformTimeCtl" runat="server" Width="100%" CssClass="time" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel" valign="top" style="padding-top: 5px;">
                            <label class="lblMandatory">
                                <%=GetLabel("Catatan Klinis") %></label>
                        </td>
                        <td colspan="5">
                            <asp:TextBox ID="txtRemarks" runat="server" Width="100%" TextMode="Multiline" Rows="2"
                                onkeyUp="return MaxCount(this,event,500);" />
                        </td>
                    </tr>
                </table>
                <div style="height: 400px; overflow-y: scroll; padding-top: 5px">
                    <fieldset id="fsDrugsQuickPicks">
                        <table id="tblSelectedItem" class="grdView notAllowSelect" cellspacing="0" rules="all">
                            <tr id="trHeader2">
                                <th style="width: 20px">
                                    &nbsp;
                                </th>
                                <th align="left">
                                    <%=GetLabel("Nama Pemeriksaan")%>
                                </th>
                                <th align="left" style="width: 200px">
                                    <%=GetLabel("Catatan Tambahan")%>
                                </th>
                                <th align="center" style="width: 20px">
                                    <%=GetLabel("CITO")%>
                                </th>
                            </tr>
                            <tr id="trFooter">
                            </tr>
                        </table>
                    </fieldset>
                </div>
            </td>
        </tr>
    </table>
    <div class="imgLoadingGrdView" id="containerImgLoadingView">
        <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
    </div>
</div>
