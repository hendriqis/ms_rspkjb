<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="VoidChargesMCUGroup.ascx.cs"
    Inherits="QIS.Medinfras.Web.MedicalCheckup.Program.VoidChargesMCUGroup" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<script type="text/javascript" id="dxss_mcudetailcustomctl">
    $(function () {
        setDatePicker('<%=txtRegistrationDate.ClientID %>');
        $('#<%=txtRegistrationDate.ClientID %>').datepicker('option', 'maxDate', '0');
    });

    $('#<%=txtRegistrationDate.ClientID %>').change(function (evt) {
        cbpViewPopup.PerformCallback("refresh");
    });

    $('#btnVoid').click(function (evt) {
        getCheckedMemberDt();
        var selected = $('#<%=hdnSelectedMemberDtRegID.ClientID %>').val();
        if (selected != '') {
            displayConfirmationMessageBox("ERROR", "Proses ini akan membatalkan seluruh order dan transaksi pasien. Melanjutkan proses?", function (result) {
                if (result) {
                    cbpViewPopup.PerformCallback("void");
                }
            });
        }
        else {
            displayMessageBox("WARNING", "Harap pilih registrasi yang akan diproses pembatalan order dan transaksi");
        }
    });

    $('.imgEdit.imgLink').die('click');
    $('.imgEdit.imgLink').live('click', function () {
        $row = $(this).closest('tr');
        var entity = rowToObject($row);
        $('#<%=hdnSelectedOrderID.ClientID %>').val(entity.OrderID);
        $('#<%=hdnSelectedOrderDtID.ClientID %>').val(entity.OrderDtID);
        $('#<%=hdnSelectedTransactionID.ClientID %>').val(entity.TransactionID);
        $('#<%=hdnSelectedTransactionDtID.ClientID %>').val(entity.TransactionDtID); 
        $('#<%=hdnSelectedHealthcareServiceUnitID.ClientID %>').val(entity.HealthcareServiceUnitID);
        $('#<%=hdnSelectedDepartmentID.ClientID %>').val(entity.DepartmentID);
        $('#<%=hdnItemID.ClientID %>').val(entity.ItemID);
        $('#<%=txtItemCode.ClientID %>').val(entity.ItemCode);
        $('#<%=txtItemName1.ClientID %>').val(entity.ItemName1);
        $('#<%=hdnParamedicID.ClientID %>').val(entity.ParamedicID);
        $('#<%=txtParamedicCode.ClientID %>').val(entity.ParamedicCode);
        $('#<%=txtParamedicName.ClientID %>').val(entity.ParamedicName);

        $('#containerPopupEntryData').show();
    });

    //#region Item Master
    function onGetItemMasterFilterExpression() {
        var filterExpression = "HealthcareServiceUnitID = '" + $('#<%=hdnSelectedHealthcareServiceUnitID.ClientID %>').val() + "' AND IsDeleted = 0";
        return filterExpression;
    }

    $('#lblItemMaster.lblLink').live('click', function () {
        openSearchDialog('serviceunititem', onGetItemMasterFilterExpression(), function (value) {
            $('#<%=txtItemCode.ClientID %>').val(value);
            onTxtServiceItemCodeChanged(value);
        });
    });

    $('#<%=txtItemCode.ClientID %>').live('change', function () {
        onTxtServiceItemCodeChanged($(this).val());
    });

    function onTxtServiceItemCodeChanged(value) {
        var filterExpression = onGetItemMasterFilterExpression() + " AND ItemCode = '" + value + "'";
        Methods.getObject('GetvServiceUnitItemList', filterExpression, function (result) {
            if (result != null) {
                $('#<%=hdnItemID.ClientID %>').val(result.ItemID);
                $('#<%=txtItemName1.ClientID %>').val(result.ItemName1);
            }
            else {
                $('#<%=hdnItemID.ClientID %>').val('');
                $('#<%=txtItemCode.ClientID %>').val('');
                $('#<%=txtItemName1.ClientID %>').val('');
            }
        });
    }
    //#endregion

    //#region Physician
    function onGetPhysicianFilterExpression() {
        var filterExpression = "ParamedicID IN (SELECT ParamedicID FROM ServiceUnitParamedic WHERE HealthcareServiceUnitID = '" + $('#<%=hdnSelectedHealthcareServiceUnitID.ClientID %>').val() + "') AND IsDeleted = 0";
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

    $('#btnEntryPopupCancel').live('click', function () {
        $('#containerPopupEntryData').hide();
    });

    $('#btnEntryPopupSave').click(function (evt) {
        if (IsValid(evt, 'fsEntryPopup', 'mpEntryPopup'))
            cbpViewPopup.PerformCallback('save');
        return false;
    });

    function onCbpViewPopupEndCallback(s) {
        var param = s.cpResult.split('|');
        if (param[0] == 'save') {
            if (param[1] == 'fail')
                showToast('Save Failed', 'Error Message : ' + param[2]);
            else
                $('#containerPopupEntryData').hide();
        }
        else if (param[0] == 'void') {
            if (param[1] == 'fail')
                showToast('Save Failed', 'Error Message : ' + param[2]);
            else
                $('#containerPopupEntryData').hide();
        }
        hideLoadingPanel();
    }

    //#region Check Box
    $('#chkSelectAllDt').die('change');
    $('#chkSelectAllDt').live('change', function () {
        var isChecked = $(this).is(":checked");
        $('.chkIsSelectedDt input').each(function () {
            $(this).prop('checked', isChecked);
            $(this).change();
        });
    });

    function getCheckedMemberDt() {
        var lstParam = '';
        var lstRegID = '';
        var lstVisitID = '';
        $('.chkIsSelectedDt input:checked').each(function () {
            var trxID = $(this).closest('tr').find('.hdnKeyField').val();
            var regID = $(this).closest('tr').find('.hdnRegistrationID').val();
            var visitID = $(this).closest('tr').find('.hdnVisitID').val();
            if (lstParam != '')
                lstParam += ',';
            lstParam += trxID;
            if (lstRegID != '')
                lstRegID += ',';
            lstRegID += regID;
            if (lstVisitID != '')
                lstVisitID += ',';
            lstVisitID += visitID;
        });
        $('#<%=hdnSelectedMemberDt.ClientID %>').val(lstParam);
        $('#<%=hdnSelectedMemberDtRegID.ClientID %>').val(lstRegID);
        $('#<%=hdnSelectedMemberDtVisitID.ClientID %>').val(lstVisitID);
    }
</script>
<div style="padding: 10px;">
    <input type="hidden" id="hdnVisitIDCtl" runat="server" />
    <input type="hidden" id="hdnRegistrationNoCtl" runat="server" />
    <input type="hidden" id="hdnPatientNameCtl" runat="server" />
    <input type="hidden" id="hdnItemIDCtl" runat="server" />
    <input type="hidden" id="hdnItemCodeCtl" runat="server" />
    <input type="hidden" id="hdnItemNameCtl" runat="server" />
    <input type="hidden" id="hdnSelectedOrderID" runat="server" />
    <input type="hidden" id="hdnSelectedOrderDtID" runat="server" />
    <input type="hidden" id="hdnSelectedTransactionID" runat="server" />
    <input type="hidden" id="hdnSelectedTransactionDtID" runat="server" />
    <input type="hidden" id="hdnSelectedHealthcareServiceUnitID" runat="server" />
    <input type="hidden" id="hdnSelectedDepartmentID" runat="server" />
    <input type="hidden" id="hdnRegistrationDate" runat="server" />
    <input type="hidden" id="hdnSelectedMemberDt" runat="server" value="" />
    <input type="hidden" id="hdnSelectedMemberDtRegID" runat="server" value="" />
    <input type="hidden" id="hdnSelectedMemberDtVisitID" runat="server" value="" />
    <table width="100%">
        <colgroup>
            <col width="20px" />
            <col width="250px" />
            <col width="50%" />
            <col />
        </colgroup>
        <tr>
            <td class="tdLabel">
                <label class="lblNormal">
                    <%=GetLabel("Tanggal Registrasi")%></label>
            </td>
            <td>
                <asp:TextBox ID="txtRegistrationDate" Width="120px" runat="server" CssClass="datepicker" />
            </td>
        </tr>
        <tr>
            <td>
                <input type="button" id="btnVoid" value="V O I D" class="btnVoid w3-button w3-red w3-border w3-border-red w3-round-large" />
            </td>
        </tr>
        <tr style="display:none">
            <td class="tdLabel">
                <label class="lblNormal">
                    <%=GetLabel("Nama Paket")%></label>
            </td>
            <td>
                <asp:TextBox ID="txtItemServiceName" ReadOnly="true" Width="100%" runat="server" />
            </td>
        </tr>
        <tr>
            <td colspan="3">
                <div id="containerPopupEntryData" style="margin-top: 10px; display: none;">
                    <input type="hidden" id="hdnVisitTypeID" runat="server" value="" />
                    <div class="pageTitle">
                        <%=GetLabel("Edit Detail Paket MCU")%></div>
                    <fieldset id="fsEntryPopup" style="margin: 0">
                        <table class="tblEntryDetail" style="width: 100%">
                            <colgroup>
                                <col style="width: 150px" />
                            </colgroup>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblLink lblMandatory" id="lblItemMaster">
                                        <%=GetLabel("Item")%></label>
                                </td>
                                <td>
                                    <input type="hidden" value="" id="hdnItemID" runat="server" />
                                    <table cellpadding="0" cellspacing="0" width="100%">
                                        <colgroup>
                                            <col style="width: 120px" />
                                            <col style="width: 3px" />
                                            <col />
                                        </colgroup>
                                        <tr>
                                            <td>
                                                <asp:TextBox ID="txtItemCode" Width="90%" runat="server" />
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtItemName1" ReadOnly="true" Width="90%" runat="server" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblLink lblMandatory" id="lblParamedic">
                                        <%=GetLabel("Paramedic")%></label>
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
                                                <asp:TextBox ID="txtParamedicCode" Width="90%" runat="server" />
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtParamedicName" ReadOnly="true" Width="90%" runat="server" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3">
                                    <table>
                                        <tr>
                                            <td>
                                                <input type="button" id="btnEntryPopupSave" value='<%= GetLabel("Simpan")%>' />
                                            </td>
                                            <td>
                                                <input type="button" id="btnEntryPopupCancel" value='<%= GetLabel("Batal")%>' />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                </div>
                <div>
                    <dxcp:ASPxCallbackPanel ID="cbpViewPopup" runat="server" Width="100%" ClientInstanceName="cbpViewPopup"
                        ShowLoadingPanel="false" OnCallback="cbpViewPopup_Callback">
                        <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ onCbpViewPopupEndCallback(s); }" />
                        <PanelCollection>
                            <dx:PanelContent ID="PanelContent1" runat="server">
                                <asp:Panel runat="server" ID="pnlView" Style="width: 100%; margin-left: auto; margin-right: auto;
                                    position: relative; font-size: 0.95em; max-height: 420px; overflow-y: auto">
                                    <asp:ListView runat="server" ID="lvwViewDt" OnItemDataBound="lvwViewDt_ItemDataBound">
                                       <EmptyDataTemplate>
                                            <table id="tblView" runat="server" class="grdControlOrder grdSelected" cellspacing="0"
                                                rules="all">
                                                <tr>
                                                    <th class="keyField">
                                                        &nbsp;
                                                    </th>
                                                        <th style="width: 10px" align="center">
                                                        <input id="chkSelectAllDt" type="checkbox" />
                                                    </th>
                                                    <th align="left">
                                                        <%=GetLabel("No Rekam Medis")%>
                                                    </th>
                                                    <th align="left">
                                                        <%=GetLabel("No Registrasi")%>
                                                    </th>
                                                    <th align="left">
                                                        <%=GetLabel("Nama Pasien")%>
                                                    </th>
                                                    <th align="left">
                                                        <%=GetLabel("Paket MCU")%>
                                                    </th>
                                                    <th align="left">
                                                        <%=GetLabel("Pembayar")%>
                                                    </th>
                                                </tr>
                                                <tr class="trEmpty">
                                                    <td colspan="20">
                                                        <%=GetLabel("Tidak ada data.")%>
                                                    </td>
                                                </tr>
                                            </table>
                                        </EmptyDataTemplate>
                                        <LayoutTemplate>
                                            <table id="tblView" runat="server" class="grdMCUOrder grdSelected" cellspacing="0"
                                                rules="all">
                                                <tr>
                                                    <th class="keyField">
                                                        &nbsp;
                                                    </th>
                                                        <th style="width: 10px" align="center">
                                                        <input id="chkSelectAllDt" type="checkbox" />
                                                    </th>
                                                    <th align="left">
                                                        <%=GetLabel("No Rekam Medis")%>
                                                    </th>
                                                    <th align="left">
                                                        <%=GetLabel("No Registrasi")%>
                                                    </th>
                                                    <th align="left">
                                                        <%=GetLabel("Nama Pasien")%>
                                                    </th>
                                                    <th align="left">
                                                        <%=GetLabel("Paket MCU")%>
                                                    </th>
                                                    <th align="left">
                                                        <%=GetLabel("Pembayar")%>
                                                    </th>
                                                </tr>
                                                <tr runat="server" id="itemPlaceholder">
                                                </tr>
                                            </table>
                                        </LayoutTemplate>
                                        <ItemTemplate>
                                            <tr>
                                                <td class="keyField">
                                                    <%#: Eval("ID")%>
                                                </td>
                                                <td align="center">
                                                    <div style="padding: 3px">
                                                        <asp:CheckBox ID="chkIsSelectedDt" CssClass="chkIsSelectedDt" runat="server" />
                                                        <input type="hidden" class="hdnKeyField" value="<%#: Eval("RegistrationID")%>" />
                                                    </div>
                                                </td>
                                                <td align="left">
                                                    <input type="hidden" value="" class="hdnKey" id="hdnKey" runat="server" />
                                                    <%#: Eval("MedicalNo")%>
                                                </td>
                                                <td align="left">
                                                    <%#: Eval("RegistrationNo")%>
                                                </td>
                                                <td align="left">
                                                    <%#: Eval("PatientName")%>
                                                </td>
                                                <td class="lblLink lblMCUItem" runat="server" id="lblMCUItem" style="display:none">
                                                    <%#: Eval("cfItemComparison")%>
                                                </td>
                                                <td>
                                                    <%#: Eval("cfItemComparison")%>
                                                </td>
                                                <td align="left">
                                                    <%#: Eval("BusinessPartnerName")%>
                                                </td>
                                                <td align="left" style="display:none">
                                                    <input type="hidden" value="" class="hdnRegistrationID" id="hdnRegistrationID" runat="server" />
                                                    <input type="hidden" value="" class="hdnVisitID" id="hdnVisitID" runat="server" />
                                                    <input type="hidden" value="" class="hdnRegistrationNo" id="hdnRegistrationNo" runat="server" />
                                                    <input type="hidden" value="" class="hdnPatientName" id="hdnPatientName" runat="server" />
                                                    <input type="hidden" value="" class="hdnItemID" id="hdnItemID" runat="server" />
                                                    <input type="hidden" value="" class="hdnItemCode" id="hdnItemCode" runat="server" />
                                                    <input type="hidden" value="" class="hdnItemName1" id="hdnItemName1" runat="server" />
                                                </td>
                                            </tr>
                                        </ItemTemplate>
                                    </asp:ListView>
                                </asp:Panel>
                            </dx:PanelContent>
                        </PanelCollection>
                    </dxcp:ASPxCallbackPanel>
                </div>
            </td>
        </tr>
    </table>
</div>
