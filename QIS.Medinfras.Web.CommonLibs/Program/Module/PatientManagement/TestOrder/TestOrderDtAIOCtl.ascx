<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TestOrderDtAIOCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.TestOrderDtAIOCtl" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<script type="text/javascript" id="dxss_itemlaboratoryfractionentryctl">
    setDatePicker('<%=txtRealizationDate.ClientID %>');
    setDatePicker('<%=txtRescheduleDate.ClientID %>');
    $('#<%=txtRealizationDate.ClientID %>').datepicker('option', 'maxDate', '0');
    $('#<%=txtRescheduleDate.ClientID %>').datepicker('option', 'minDate', '0');
    $('#<%=tdReschedule.ClientID %>').attr('style', 'display:none');
    $('#<%=tdVoidReason.ClientID %>').attr('style', 'display:none');

    function getCheckedMember() {
        var lstTestOrderDtID = '';
        var lstTestPartnerID = '';
        var lstTestParamedicID = '';
        var lstTestNotes = '';
        var lstIsCito = '';
        var lstQty = '';
        $('#<%=grdView.ClientID %> .chkIsSelected input:checked').each(function () {
            $tr = $(this).closest('tr');
            var testOrderDtID = $tr.find('.keyField').html();
            var testPartnerID = $tr.find('.hdnTestPartnerID').val();
            var testParamedicID = $tr.find('.hdnTestParamedicID').val();
            var isCito = 0;
            var qty = $tr.find('.txtQty').val();
            if ($tr.find('.chkIsCITO input').is(":checked")) {
                isCito = 1;
            };

            if (lstTestOrderDtID != '') {
                lstTestOrderDtID += ',';
                lstTestPartnerID += ',';
                lstTestParamedicID += ',';
                lstIsCito += ',';
                lstQty += ',';
            }
            lstTestOrderDtID += testOrderDtID;
            lstTestPartnerID += testPartnerID;
            lstTestParamedicID += testParamedicID;
            lstIsCito += isCito;
            lstQty += qty;
        });
        $('#<%=hdnListTestOrderDtID.ClientID %>').val(lstTestOrderDtID);
        $('#<%=hdnListTestPartnerID.ClientID %>').val(lstTestPartnerID);
        $('#<%=hdnListTestParamedicID.ClientID %>').val(lstTestParamedicID);
        $('#<%=hdnListIsCito.ClientID %>').val(lstIsCito);
        $('#<%=hdnListQty.ClientID %>').val(lstQty);
    }

    $('#btnTestOrderApprove').click(function (evt) {
        var displayOption = $('#<%=rblDataSource.ClientID %>').find(":checked").val();
        getCheckedMember();
        var lstOrderDt = $('#<%=hdnListTestOrderDtID.ClientID %>').val();
        var countDt = $('#<%=hdnCountDt.ClientID %>').val();

        if (countDt == "0") {
            if (displayOption == 0) { // DIKERJAKAN
                cbpEntryPopupView.PerformCallback('approve');
            }
            else if (displayOption == 1) { // BATAL
                var visitID = $('#<%=hdnVisitID.ClientID %>').val();
                var TestOrderID = $('#<%=hdnTestOrderID.ClientID %>').val();
                var filterExpression = "VisitID = " + visitID + " AND TestOrderID = " + TestOrderID + " AND IsDeleted = 0";
                Methods.getObject('GetPatientSurgeryList', filterExpression, function (result) {
                    if (result != null) {
                        showToast('Warning', "Nomor Order ini tidak dapat di void karena sudah ada Laporan Operasi");
                    }
                    else {
                        cbpEntryPopupView.PerformCallback('decline');
                    }
                });
            }
            else { // DIJADWALKAN
                cbpEntryPopupView.PerformCallback('reschedule');
            }
        } else {
            if (lstOrderDt != "" && lstOrderDt != "0") {
                if (displayOption == 0) { // DIKERJAKAN
                    if (IsValid(evt, 'fsMPPopupEntry', 'mpEntry')) {
                        cbpEntryPopupView.PerformCallback('approve');
                    }
                }
                else if (displayOption == 1) { // BATAL
                    if (IsValid(evt, 'fsMPEntry', 'mpEntry')) {
                        var visitID = $('#<%=hdnVisitID.ClientID %>').val();
                        var TestOrderID = $('#<%=hdnTestOrderID.ClientID %>').val();
                        var filterExpression = "VisitID = " + visitID + " AND TestOrderID = " + TestOrderID + " AND IsDeleted = 0";
                        Methods.getObject('GetPatientSurgeryList', filterExpression, function (result) {
                            if (result != null) {
                                showToast('Warning', "Nomor Order ini tidak dapat di void karena sudah ada Laporan Operasi");
                            }
                            else {
                                cbpEntryPopupView.PerformCallback('decline');
                            }
                        });
                    }
                }
                else { // DIJADWALKAN
                    if (IsValid(evt, 'fsMPPopupEntry', 'mpEntry')) {
                        cbpEntryPopupView.PerformCallback('reschedule');
                    }
                }
            } else {
                showToast('Failed', 'Error Message : Tidak ada order yang dipilih.');
            }
        }
    });

    $('#btnTestOrderClose').click(function (evt) {
        if ($('#<%=hdnTransactionID.ClientID %>').val() == '' || $('#<%=hdnTransactionID.ClientID %>').val() == '0') {
            showLoadingPanel();
            document.location = document.referrer;
        }
        else {
            pcRightPanelContent.Hide();
        }
    });

    function onCbpEntryPopupViewEndCallback(s) {
        hideLoadingPanel();
        var param = s.cpResult.split('|');
        if (param[0] == 'approve') {
            if (param[1] == 'fail')
                showToast('Approve Failed', 'Error Message : ' + param[2]);
            else {
                if ($('#<%=hdnTransactionID.ClientID %>').val() == '')
                    onAfterAddRecordAddRowCount();
                if (s.cpRetval != "") onLoadObject(s.cpRetval);
                pcRightPanelContent.Hide();
            }
        }
        else if (param[0] == 'decline') {
            if (param[1] == 'fail')
                showToast('Cancel Failed', 'Error Message : ' + param[2]);
        }
        else if (param[0] == 'reschedule') {
            if (param[1] == 'fail') {
                showToast('Reschedule Failed', 'Error Message : ' + param[2]);
            }
            else {
                showToast('Reschedule Success', param[2]);
                cbpEntryPopupView.PerformCallback('refresh');
                //////                showLoadingPanel();
                //////                document.location = document.referrer;
            }
        }
        else {
            pcRightPanelContent.Hide();
        }
    }

    function getTestOrderTransactionFilterExpression() {
        var filterExpression = "<%:GetTestOrderTransactionFilterExpression() %>";
        return filterExpression;
    }

    $('#lblTestOrderTransactionNo.lblLink').click(function () {
        openSearchDialog('patientchargeshd', getTestOrderTransactionFilterExpression(), function (value) {
            $('#<%=txtTransactionNo.ClientID %>').val(value);
            onTxtTestOrderTransactionNoChanged(value);
        });
    });

    $('#<%=txtTransactionNo.ClientID %>').live('change', function () {
        onTxtTestOrderTransactionNoChanged($(this).val());
    });

    function onTxtTestOrderTransactionNoChanged(value) {
        var filterExpression = getTestOrderTransactionFilterExpression() + " AND TransactionNo = '" + value + "'";
        Methods.getObject('GetPatientChargesHdList', filterExpression, function (result) {
            if (result != null) {
                $('#<%=hdnTransactionID.ClientID %>').val(result.TransactionID);
                $('#<%=txtTestOrderDate.ClientID %>').val(result.TransactionDateInDatePickerFormat);
                $('#<%=txtTestOrderTime.ClientID %>').val(result.TransactionTime);
            }
            else {
                $('#<%=hdnTransactionID.ClientID %>').val('');
                $('#<%=txtTransactionNo.ClientID %>').val('');
                $('#<%=txtTestOrderDate.ClientID %>').val('');
                $('#<%=txtTestOrderTime.ClientID %>').val('');
            }
        });
    }

    //#region Order Physician
    $('#lblFromOrderPhysician.lblLink').live('click', function () {
        var filterParamedic = "GCParamedicMasterType = 'X019^001' AND IsDeleted = 0";
        openSearchDialog('paramedic', filterParamedic, function (value) {
            $('#<%=txtFromOrderPhysicianCode.ClientID %>').val(value);
            ontxtFromOrderPhysicianCodeChanged(value);
        });
    });

    $('#<%=txtFromOrderPhysicianCode.ClientID %>').live('change', function () {
        ontxtFromOrderPhysicianCodeChanged($(this).val());
    });

    function ontxtFromOrderPhysicianCodeChanged(value) {
        var filterExpression = "ParamedicCode = '" + value + "'";
        Methods.getObject('GetvParamedicMasterList', filterExpression, function (result) {
            if (result != null) {
                $('#<%=hdnOrderPhysicianID.ClientID %>').val(result.ParamedicID);
                $('#<%=txtFromOrderPhysicianName.ClientID %>').val(result.ParamedicName);
            }
            else {
                $('#<%=hdnOrderPhysicianID.ClientID %>').val('');
                $('#<%=txtFromOrderPhysicianCode.ClientID %>').val('');
                $('#<%=txtFromOrderPhysicianName.ClientID %>').val('');
            }
        });
    }
    //#endregion

    //#region Physician
    $('#lblTestOrderPhysician.lblLink').live('click', function () {
        openSearchDialog('paramedic', onGetPhysicianFilterExpression(), function (value) {
            $('#<%=txtPhysicianCode.ClientID %>').val(value);
            onTxtTestOrderPhysicianCodeChanged(value);
        });
    });

    $('#<%=txtPhysicianCode.ClientID %>').live('change', function () {
        onTxtTestOrderPhysicianCodeChanged($(this).val());
    });

    function onTxtTestOrderPhysicianCodeChanged(value) {
        var filterExpression = onGetPhysicianFilterExpression() + " AND ParamedicCode = '" + value + "'";
        Methods.getObject('GetvParamedicMasterList', filterExpression, function (result) {
            if (result != null) {
                $('#<%=hdnPhysicianID.ClientID %>').val(result.ParamedicID);
                $('#<%=txtPhysicianName.ClientID %>').val(result.ParamedicName);
            }
            else {
                $('#<%=hdnPhysicianID.ClientID %>').val('');
                $('#<%=txtPhysicianCode.ClientID %>').val('');
                $('#<%=txtPhysicianName.ClientID %>').val('');
            }
        });
    }
    //#endregion

    //#region Test Partner
    $td = null;
    $('.lblTestPartner.lblLink').live('click', function () {
        $td = $(this).parent();
        openSearchDialog('testpartner', 'IsDeleted = 0', function (value) {
            onTxtTestPartnerChanged(value);
        });
    });

    function onTxtTestPartnerChanged(value) {
        var filterExpression = "BusinessPartnerCode = '" + value + "'";
        Methods.getObject('GetvTestPartnerList', filterExpression, function (result) {
            if (result != null) {
                $td.find('.hdnTestPartnerID').val(result.BusinessPartnerID);
                if (result.ShortName != "")
                    $td.find('.lblTestPartner').html(result.ShortName);
                else
                    $td.find('.lblTestPartner').html(result.BusinessPartnerName);
            }
            else {
                $td.find('.hdnTestPartnerID').val('0');
                $td.find('.lblTestPartner').html('');
            }
        });
    }
    //#endregion

    //#region Dokter Pelaksana Detail
    $td = null;
    $('.lblTestParamedicDetailCtl.lblLink').live('click', function () {
        $td = $(this).parent();
        var filterExpression = "ParamedicID IN (SELECT ParamedicID FROM ServiceUnitParamedic WHERE HealthcareServiceUnitID = '" + $('#<%=hdnHealthcareServiceUnitID.ClientID %>').val() + "') AND IsDeleted = 0";
        openSearchDialog('paramedic', filterExpression, function (value) {
            onTxtParamedicPerItemChanged(value);
        });
    });

    function onTxtParamedicPerItemChanged(value) {
        var filterExpression = "ParamedicCode = '" + value + "'";
        Methods.getObject('GetvParamedicMasterList', filterExpression, function (result) {
            if (result != null) {
                $td.find('.hdnTestParamedicID').val(result.ParamedicID);
                if (result.ParamedicName != "")
                    $td.find('.lblTestParamedicDetailCtl').html(result.ParamedicName);
            }
            else {
                $td.find('.hdnTestParamedicID').val('0');
                $td.find('.lblTestParamedicDetailCtl').html('');
            }
        });
    }
    //#endregion

    $('#chkSelectAll').die('change');
    $('#chkSelectAll').live('change', function () {
        var isChecked = $(this).is(":checked");
        $('.chkIsSelected input').each(function () {
            $(this).prop('checked', isChecked);
            $(this).change();
        });
    });

    function onCboVoidReasonValueChanged(s) {
        if (s.GetValue() == 'X129^999')
            $('#<%=txtVoidReason.ClientID %>').show();
        else
            $('#<%=txtVoidReason.ClientID %>').hide();
    }

    $('#<%=rblDataSource.ClientID %>').live('change', function () {
        var displayOption = $('#<%=rblDataSource.ClientID %>').find(":checked").val();

        if (displayOption == 0) {
            $('#<%=tdReschedule.ClientID %>').attr('style', 'display:none');
            $('#<%=tdVoidReason.ClientID %>').attr('style', 'display:none');
        }
        else if (displayOption == 1) {
            $('#<%=tdReschedule.ClientID %>').attr('style', 'display:none');
            $('#<%:tdVoidReason.ClientID %>').removeAttr('style');
        }
        else {
            $('#<%=tdVoidReason.ClientID %>').attr('style', 'display:none');
            $('#<%:tdReschedule.ClientID %>').removeAttr('style');
        }
    });

    $('.chkIsSelected input').die('change');
    $('.chkIsSelected input').live('change', function () {
        $tr = $(this).closest('tr');
        if ($(this).is(':checked')) {
            $tr.find('.txtQty').removeAttr('readonly');
        }
        else {
            $tr.find('.txtQty').attr('readonly', 'readonly');
        }
    });

    $('.txtQty').die('change');
    $('.txtQty').live('change', function () {
        var $tr = $(this).closest('tr');
        var maxQty = parseFloat($tr.find('.hdnQuantity').val());
        var value = $(this).val();
        var value = checkMinus(value);
        if (value > maxQty) {
            $(this).val(maxQty);
        }
        else {
            if (value == 0) {
                $(this).val(maxQty);
            }
        }
    });
</script>
<div style="height: 500px; overflow-y: auto; overflow-x: hidden">
    <input type="hidden" id="hdnTransactionID" runat="server" value="" />
    <input type="hidden" id="hdnGCTransactionStatus" runat="server" value="" />
    <input type="hidden" id="hdnIsAllowVoid" runat="server" value="" />
    <input type="hidden" id="hdnLinkedChargesID" value="" runat="server" />
    <input type="hidden" id="hdnHealthcareServiceUnitID" value="" runat="server" />
    <input type="hidden" id="hdnDepartmentID" value="" runat="server" />
    <input type="hidden" id="hdnVisitID" value="" runat="server" />
    <input type="hidden" id="hdnTestOrderID" value="" runat="server" />
    <input type="hidden" id="hdnOrderDate" value="" runat="server" />
    <input type="hidden" id="hdnClassID" value="" runat="server" />
    <input type="hidden" id="hdnOrderPhysicianID" value="" runat="server" />
    <input type="hidden" id="hdnPhysicianID" value="" runat="server" />
    <input type="hidden" id="hdnRegistrationID" value="" runat="server" />
    <input type="hidden" id="hdnHSUImagingID" value="" runat="server" />
    <input type="hidden" id="hdnHSULaboratoryID" value="" runat="server" />
    <input type="hidden" id="hdnDefaultParamedicID" value="" runat="server" />
    <input type="hidden" id="hdnListTestOrderDtID" runat="server" />
    <input type="hidden" id="hdnListTestPartnerID" runat="server" />
    <input type="hidden" id="hdnListTestParamedicID" runat="server" />
    <input type="hidden" id="hdnListIsCito" runat="server" />
    <input type="hidden" id="hdnListQty" runat="server" />
    <input type="hidden" id="hdnCountDt" runat="server" value="" />
    <table class="tblContentArea">
        <colgroup>
            <col style="width: 100%" />
        </colgroup>
        <tr>
            <td style="padding: 5px; vertical-align: top">
                <fieldset id="fsMPPopupEntry">
                    <table class="tblEntryContent" style="width: 100%">
                        <colgroup>
                            <col style="width: 200px" />
                            <col style="width: 120px" />
                            <col style="width: 60px" />
                            <col style="width: 150px; padding-left: 10px" />
                            <col style="width: 100px" />
                            <col />
                        </colgroup>
                        <tr id="trTransactionNo" runat="server">
                            <td class="tdLabel">
                                <label class="lblNormal" id="lblTestOrderTransactionNo">
                                    <%=GetLabel("No. Order")%></label>
                            </td>
                            <td colspan="2">
                                <asp:TextBox ID="txtTransactionNo" ReadOnly="true" Width="190px" runat="server" />
                            </td>
                        </tr>
                        <tr id="trDiorderOleh" runat="server">
                            <td class="tdLabel">
                                <label class="lblNormal" id="lblUserOrder">
                                    <%=GetLabel("Order dientry oleh")%></label>
                            </td>
                            <td colspan="2">
                                <asp:TextBox ID="txtOrderUser" ReadOnly="true" Width="190px" runat="server" />
                            </td>
                            <td>
                                <asp:CheckBox ID="chkIsCITO" runat="server" CssClass="chkIsCITO" Checked='<%#Eval("IsCito") %>' />
                                <label class="lblNormal" id="Label1">
                                    <%=GetLabel("CITO")%>
                                </label>
                            </td>
                            <%--<td class="tdLabel">
                                <label class="lblNormal" id="lblDokter">
                                    <%=GetLabel("Dokter Pengirim")%></label>
                            </td>
                            <td colspan="5">
                                <asp:TextBox ID="txtFromOrderPhysicianName" Width="100%" runat="server" ReadOnly="true" />
                            </td>--%>
                        </tr>
                        <tr id="trOrderPhysician" runat="server">
                            <td class="tdLabel">
                                <label class="lblLink lblNormal" id="lblFromOrderPhysician">
                                    <%=GetLabel("Dokter Pengirim")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtFromOrderPhysicianCode" Width="120px" runat="server" />
                            </td>
                            <td colspan="4">
                                <asp:TextBox ID="txtFromOrderPhysicianName" Width="100%" ReadOnly="true" runat="server" />
                            </td>
                        </tr>
                        <tr id="trOrderDateTime" runat="server">
                            <td class="tdLabel">
                                <%=GetLabel("Tanggal ") %>
                                /
                                <%=GetLabel("Jam Order") %>
                            </td>
                            <td>
                                <asp:TextBox ID="txtTestOrderDate" Width="120px" runat="server" Style="text-align: center"
                                    ReadOnly="true" />
                            </td>
                            <td>
                                <asp:TextBox ID="txtTestOrderTime" Width="60px" CssClass="time" runat="server" Style="text-align: center"
                                    ReadOnly="true" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <%=GetLabel("Tanggal ") %>
                                /
                                <%=GetLabel("Jam Rencana") %>
                            </td>
                            <td>
                                <asp:TextBox ID="txtScheduledDate" Width="120px" runat="server" Style="text-align: center"
                                    ReadOnly="true" />
                            </td>
                            <td>
                                <asp:TextBox ID="txtScheduledTime" Width="60px" CssClass="time" runat="server" Style="text-align: center"
                                    ReadOnly="true" />
                            </td>
                            <td colspan="2">
                                <asp:TextBox ID="txtToBePerformed" ReadOnly="true" Width="100%" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel" valign="top" style="padding-top: 5px;">
                                <label>
                                    <%=GetLabel("Catatan")%></label>
                            </td>
                            <td colspan="5">
                                <asp:TextBox ID="txtNotes" ReadOnly="true" Width="100%" TextMode="MultiLine" Height="70px"
                                    runat="server" />
                            </td>
                        </tr>
                        <tr id="trTransactionDateTime" runat="server">
                            <td class="tdLabel">
                                <%=GetLabel("Tanggal ") %>
                                /
                                <%=GetLabel("Jam Konfirmasi Order") %>
                            </td>
                            <td>
                                <asp:TextBox ID="txtRealizationDate" Width="100px" runat="server" CssClass="datepicker"
                                    Style="text-align: center" />
                            </td>
                            <td>
                                <asp:TextBox ID="txtRealizationTime" Width="60px" CssClass="time" runat="server"
                                    Style="text-align: center" />
                            </td>
                        </tr>
                        <tr id="trTransactionParamedic" runat="server">
                            <td class="tdLabel">
                                <label class="lblLink lblMandatory" id="lblTestOrderPhysician">
                                    <%=GetLabel("Dokter Pelaksana")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtPhysicianCode" Width="120px" runat="server" />
                            </td>
                            <td colspan="4">
                                <asp:TextBox ID="txtPhysicianName" Width="100%" ReadOnly="true" runat="server" />
                            </td>
                        </tr>
                    </table>
                </fieldset>
                <dxcp:ASPxCallbackPanel ID="cbpEntryPopupView" runat="server" Width="100%" ClientInstanceName="cbpEntryPopupView"
                    ShowLoadingPanel="false" OnCallback="cbpEntryPopupView_Callback">
                    <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ onCbpEntryPopupViewEndCallback(s); }" />
                    <PanelCollection>
                        <dx:PanelContent ID="PanelContent1" runat="server">
                            <asp:Panel runat="server" ID="pnlEntryPopupGrdView" Style="width: 100%; margin-left: auto;
                                margin-right: auto; position: relative; font-size: 0.95em;">
                                <input type="hidden" id="hdnAppointmentInfo" runat="server" value="" />
                                <asp:GridView ID="grdView" runat="server" CssClass="grdView notAllowSelect" AutoGenerateColumns="false"
                                    ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty" OnRowDataBound="grdView_RowDataBound">
                                    <Columns>
                                        <asp:BoundField DataField="ID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                        <asp:TemplateField HeaderStyle-Width="40px" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                            <HeaderTemplate>
                                                <input id="chkSelectAll" type="checkbox" />
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkIsSelected" runat="server" CssClass="chkIsSelected" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderStyle-Width="40px" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                            <HeaderTemplate>
                                                <div>
                                                    <%=GetLabel("CITO") %></div>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkIsCITO" runat="server" CssClass="chkIsCITO" Checked='<%#Eval("IsCito") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderStyle-HorizontalAlign="Left">
                                            <HeaderTemplate>
                                                <div>
                                                    <%=GetLabel("Pelayanan") %></div>
                                                <div style="color: Blue">
                                                    <%=GetLabel("Diagnosa") %></div>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <div>
                                                    <%#: Eval("ItemName1")%></div>
                                                <div style="color: Blue">
                                                    <%#: Eval("DiagnoseName") %></div>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderStyle-HorizontalAlign="Left">
                                            <HeaderTemplate>
                                                <div>
                                                    <%=GetLabel("Test Partner") %>
                                                </div>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <input type="hidden" value="0" class="hdnTestPartnerID" id="hdnTestPartnerID" runat="server" />
                                                <label id="lblTestPartner" class="lblTestPartner <%# Eval("IsSubContractItem").ToString() == "False" ? "lblDisabled" : Eval("GCTestOrderStatus").ToString() == "X126^001" ? "lblLink" : "lblDisabled" %>">
                                                    <%# Eval("TestPartner") %></label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderStyle-HorizontalAlign="Left">
                                            <HeaderTemplate>
                                                <div>
                                                    <%=GetLabel("Dokter Pelaksana") %>
                                                </div>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <input type="hidden" value="0" class="hdnTestParamedicID" id="hdnTestParamedicID"
                                                    runat="server" />
                                                <label id="lblTestParamedicDetailCtl" class="lblTestParamedicDetailCtl <%# Eval("GCTestOrderStatus").ToString() == "X126^001" ? "lblLink" : "lblDisabled" %>">
                                                    <%# Eval("DetailOrderParamedicName").ToString() != "" ? Eval("DetailOrderParamedicName").ToString() : Eval("DokterPemeriksa").ToString() %></label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderStyle-HorizontalAlign="Right">
                                            <HeaderTemplate>
                                                <div>
                                                    <%=GetLabel("Jumlah") %>
                                                </div>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                            <input type="hidden" id="hdnQuantity" class="hdnQuantity" runat="server" value='<%#: Eval("RemainingQty")%>' />
                                                <asp:TextBox ID="txtQty" ReadOnly="true" Width="99%" value="0" runat="server" min="0"
                                                    class="txtQty number min" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="Remarks" HeaderText="Catatan Klinis/Order" HeaderStyle-Width="200px"
                                            HeaderStyle-VerticalAlign="middle" />
                                        <asp:BoundField DataField="TestOrderStatus" HeaderText="Status" HeaderStyle-Width="100px"
                                            HeaderStyle-VerticalAlign="middle" />
                                    </Columns>
                                    <EmptyDataTemplate>
                                        <%=GetLabel("No Data To Display")%>
                                    </EmptyDataTemplate>
                                </asp:GridView>
                                <div class="imgLoadingGrdView" id="containerImgLoadingViewPopup">
                                    <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                                </div>
                            </asp:Panel>
                        </dx:PanelContent>
                    </PanelCollection>
                </dxcp:ASPxCallbackPanel>
            </td>
        </tr>
    </table>
    <div style="width: 100%; text-align: right; padding-top: 10px">
        <table id="tblProcess" runat="server" width="100%">
            <tr>
                <td style="width: 100%">
                    <asp:RadioButtonList ID="rblDataSource" runat="server" RepeatDirection="Horizontal">
                        <asp:ListItem Text="Dikerjakan" Value="0" Selected="True" />
<%--                        <asp:ListItem Text="Batal" Value="1" />
                        <asp:ListItem Text="Dijadwalkan" Value="2" />--%>
                    </asp:RadioButtonList>
                </td>
            </tr>
        </table>
    </div>
    <div style="width: 100%; text-align: right; padding-top: 10px">
        <table id="tblApproveDecline" runat="server" width="100%">
            <tr>
                <td style="width: 80px; text-align: left">
                    <input type="button" id="btnTestOrderApprove" value='<%= GetLabel("Process")%>' style="width: 100px;
                        height: 30px" />
                </td>
                <td id="tdReschedule" runat="server">
                    <table id="tblReschedule" runat="server">
                        <tr>
                            <td style="padding-left: 10px">
                                <asp:TextBox ID="txtRescheduleDate" Width="100px" runat="server" CssClass="datepicker"
                                    Style="text-align: center" />
                            </td>
                        </tr>
                    </table>
                </td>
                <td id="tdVoidReason" runat="server">
                    <table id="tblVoidReason" runat="server">
                        <tr>
                            <td class="tdLabel">
                                <%=GetLabel("Alasan Batal") %>
                            </td>
                            <td style="padding-left: 10px">
                                <dxe:ASPxComboBox ID="cboVoidReason" runat="server" Width="200px">
                                    <ClientSideEvents Init="function(s,e){ onCboVoidReasonValueChanged(s); }" ValueChanged="function(s,e){ onCboVoidReasonValueChanged(s); }" />
                                </dxe:ASPxComboBox>
                            </td>
                            <td>
                                <asp:TextBox ID="txtVoidReason" runat="server" Width="150px" Style="display: none" />
                            </td>
                        </tr>
                    </table>
                </td>
                <td style="width: 80px;">
                    <input type="button" value='<%= GetLabel("Close")%>' id="btnTestOrderClose" style="width: 100px;
                        height: 30px" />
                </td>
            </tr>
        </table>
    </div>
    <%--    <div style="width: 100%; text-align: right; padding-top: 1px">
        <table id="tblDecline" runat="server" width="100%">
            <tr>
                <td style="width: 80px;">
                    <table id="tblVoidReason" runat="server">
                        <tr>
                            <td style="width: 80px;">
                                <input type="button" id="btnTestOrderDecline" value='<%= GetLabel("Void")%>' style="width: 100px" />
                            </td>
                            <td class="tdLabel">
                                <%=GetLabel("Alasan Batal") %>
                            </td>
                            <td style="padding-left: 10x">
                                <dxe:ASPxComboBox ID="cboVoidReason" runat="server" Width="200px">
                                    <ClientSideEvents Init="function(s,e){ onCboVoidReasonValueChanged(s); }" ValueChanged="function(s,e){ onCboVoidReasonValueChanged(s); }" />
                                </dxe:ASPxComboBox>
                            </td>
                            <td>
                                <asp:TextBox ID="txtVoidReason" runat="server" Width="150px" Style="display: none" />
                            </td>
                        </tr>
                    </table>
                </td>
                <td style="width: 80px;">
                    <input type="button" value='<%= GetLabel("Close")%>' id="btnTestOrderClose" style="width: 100px" />
                </td>
            </tr>
        </table>
    </div>--%>
</div>
