<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UDDMedicationOrderCompoundEntryCtl1.ascx.cs" 
    Inherits="QIS.Medinfras.Web.Pharmacy.Program.UDDMedicationOrderCompoundEntryCtl1" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>

<style type="text/css">
    .trSelectedItem {background-color: #ecf0f1 !important;}
</style>

<script type="text/javascript" id="dxss_drugslogisticsquickpicksctl">
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

    $(function () {
        hideLoadingPanel();
        addItemFilterRow();
        setDatePicker('<%=txtStartDate.ClientID %>');
        $('#<%=txtStartDate.ClientID %>').datepicker('option', 'minDate', '0');
        $('#<%=txtStartDate.ClientID %>').val(getDateNowDatePickerFormat());

    });

    function onBeforeSaveRecord(errMessage) {
        getCheckedMember();
        if ($('#<%=hdnSelectedMember.ClientID %>').val() != '') {
            return true;
        }
        else {
            errMessage.text = 'Please Select Item First';
            return false;
        }
        return false;
    }

    function getCheckedMember() {
        var lstSelectedMember = [];
        var lstSelectedMemberStrength = [];
        var lstSelectedMemberQty = [];

        var result = '';
        $('#tblSelectedItem .trSelectedItem').each(function () {
            var key = $(this).find('.keyField').val();

            var isStrength = '0';
            if ($(this).find('.chkStrength').is(':checked')) {
                isStrength = 1;
            }

            var qty = $(this).find('.txtQty').val().replace(',','.');

            lstSelectedMember.push(key);
            lstSelectedMemberStrength.push(isStrength);
            lstSelectedMemberQty.push(qty);
        });
        $('#<%=hdnSelectedMember.ClientID %>').val(lstSelectedMember.join(','));
        $('#<%=hdnSelectedMemberStrength.ClientID %>').val(lstSelectedMemberStrength.join(','));
        $('#<%=hdnSelectedMemberQty.ClientID %>').val(lstSelectedMemberQty.join(','));
    }

    //#region Paging
    var pageCount = parseInt('<%=PageCount %>');

    $(function () {
        setPaging($("#pagingPopup"), pageCount, function (page) {
            getCheckedMember();
            cbpPopup.PerformCallback('changepage|' + page);
        });

        //#region Item Group
        $('#lblItemGroupDrugLogistic.lblLink').click(function () {
            var filterExpression = "GCItemType = 'X001^002' AND IsDeleted = 0";
            openSearchDialog('itemgroup', filterExpression, function (value) {
                $('#<%=txtItemGroupDrugLogisticCode.ClientID %>').val(value);
                onTxtItemGroupDrugLogisticCodeChanged(value);
            });
        });

        $('#<%=txtItemGroupDrugLogisticCode.ClientID %>').change(function () {
            onTxtItemGroupDrugLogisticCodeChanged($(this).val());
        });

        function onTxtItemGroupDrugLogisticCodeChanged(value) {
            var filterExpression = "ItemGroupCode = '" + value + "'";
            Methods.getObject('GetItemGroupMasterList', filterExpression, function (result) {
                if (result != null) {
                    $('#<%=hdnItemGroupDrugLogisticID.ClientID %>').val(result.ItemGroupID);
                    $('#<%=txtItemGroupDrugLogisticName.ClientID %>').val(result.ItemGroupName1);
                }
                else {
                    $('#<%=hdnItemGroupDrugLogisticID.ClientID %>').val('');
                    $('#<%=txtItemGroupDrugLogisticCode.ClientID %>').val('');
                    $('#<%=txtItemGroupDrugLogisticName.ClientID %>').val('');
                }
                getCheckedMember();
                cbpPopup.PerformCallback('refresh');
            });
        }
        //#endregion
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

    $('#<%=grdView.ClientID %> .chkIsSelected input').die('change');
    $('#<%=grdView.ClientID %> .chkIsSelected input').live('change', function () {
        if ($(this).is(':checked')) {
            $selectedTr = $(this).closest('tr');
            var dose = $selectedTr.find('.txtDose').val();
            var doseUnit = $selectedTr.find('.txtDoseUnit').val();
            var itemUnit = $selectedTr.find('.txtItemUnit').val();

            $newTr = $('#tmplSelectedTestItem').html();
            $newTr = $newTr.replace(/\$\{ItemName1}/g, $selectedTr.find('.tdItemName1').html());
            $newTr = $newTr.replace(/\$\{ItemID}/g, $selectedTr.find('.keyField').html());
            $newTr = $newTr.replace(/\$\{GCMedicationRoute}/g, $selectedTr.find('.hiddenColumn').html());
            $newTr = $newTr.replace(/\$\{ItemUnit}/g, itemUnit);
            $newTr = $newTr.replace(/\$\{Dose}/g, dose);
            $newTr = $newTr.replace(/\$\{DoseUnit}/g, doseUnit);
            $newTr = $newTr.replace(/\$\{ItemUnit}/g, itemUnit);
            $newTr = $($newTr);
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

    $('#tblSelectedItem .chkStrength').die('change');
    $('#tblSelectedItem .chkStrength').live('change', function () {
        $selectedTr = $(this).parent().parent().parent().closest('tr');
        var dose = $selectedTr.find('.txtDose').val();
        var doseUnit = $selectedTr.find('.txtDoseUnit').val();
        var itemUnit = $selectedTr.find('.itemUnit').val();
        if ($(this).is(':checked')) {
            $selectedTr.find('.txtQty').val(dose);
            $selectedTr.find('.txtItemUnit').val(doseUnit);
        }
        else {
            $selectedTr.find('.txtQty').val('1');
            $selectedTr.find('.txtItemUnit').val(itemUnit);
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

    function SetMedicationDefaultTime(frequency) {
        Methods.getMedicationSequenceTime(frequency, function (result) {
            if (result != null) {
                var medicationTimeInfo = result.split('|');
                $('#<%=txtStartTime.ClientID %>').val(medicationTimeInfo[0]);
                $('#<%=txtStartTime1.ClientID %>').val(medicationTimeInfo[0]);
                $('#<%=txtStartTime2.ClientID %>').val(medicationTimeInfo[1]);
                $('#<%=txtStartTime3.ClientID %>').val(medicationTimeInfo[2]);
                $('#<%=txtStartTime4.ClientID %>').val(medicationTimeInfo[3]);
                $('#<%=txtStartTime5.ClientID %>').val(medicationTimeInfo[4]);
                $('#<%=txtStartTime6.ClientID %>').val(medicationTimeInfo[5]);
            }
            else {
                $('#<%=txtStartTime.ClientID %>').val('-');
                $('#<%=txtStartTime1.ClientID %>').val('-');
                $('#<%=txtStartTime2.ClientID %>').val('-');
                $('#<%=txtStartTime3.ClientID %>').val('-');
                $('#<%=txtStartTime4.ClientID %>').val('-');
                $('#<%=txtStartTime5.ClientID %>').val('-');
                $('#<%=txtStartTime6.ClientID %>').val('-');
            }
        });
    }

    //#region calculate Dispense Qty
    $('#<%=txtFrequencyNumber.ClientID %>').live('change', function () {
        SetMedicationDefaultTime($('#<%=txtFrequencyNumber.ClientID %>').val());
        var dispQty = $('#<%=txtDispenseQty.ClientID %>').val();
        calculateDispenseQty();
    });

    $('#<%=txtDosingDose.ClientID %>').live('change', function () {
        var dispQty = $('#<%=txtDispenseQty.ClientID %>').val();
        calculateDispenseQty();
    });

    $('#<%=txtDosingDuration.ClientID %>').live('change', function () {
        var dispQty = $('#<%=txtDispenseQty.ClientID %>').val();
        calculateDispenseQty();
    });

    $('#<%=txtDispenseQty.ClientID %>').live('change', function () {
        var dispQty = $('#<%=txtDispenseQty.ClientID %>').val();
        $('#<%=txtEmbalaceQty.ClientID %>').val(dispQty);
        $('#<%=txtTakenQty.ClientID %>').val(dispQty);
        if (dispQty <= 0 || dispQty == "") {
            showToast('Error Message', 'Compound Quantity should be greater than 0 !');
            $('#<%=txtDispenseQty.ClientID %>').val('');
        }
    });

    function calculateDispenseQty() {
        var frequency = $('#<%=txtFrequencyNumber.ClientID %>').val();
        var dose = $('#<%=txtDosingDose.ClientID %>').val();
        var dosingDuration = $('#<%=txtDosingDuration.ClientID %>').val();

        var dispenseQty = dosingDuration * frequency * dose;
        $('#<%=txtDispenseQty.ClientID %>').val(dispenseQty);
        if (dispenseQty <= 0 || dispenseQty == "") {
            $('#<%=txtDispenseQty.ClientID %>').val('');
        }

        $('#<%=txtTakenQty.ClientID %>').val(dispenseQty);
    }
    //#endregion
</script>

<div style="padding:10px;">
    <script id="tmplSelectedTestItem" type="text/x-jquery-tmpl">
        <tr class="trSelectedItem">
            <td align="center">
                <input type="checkbox" class="chkIsSelected2" />
                <input type="hidden" class="keyField" value='${ItemID}' />
                <input type="hidden" class="hiddenColumn" id="medicationRoute" value='${GCMedicationRoute}' />
                <input type="hidden" class="itemUnit" id="itemUnit" value='${ItemUnit}' />
            </td>
            <td>${ItemName1}</td>
            <td> 
                 <table border="0" cellpadding="0" cellspacing="0" style="width:100%">
                    <tr>
                        <td><input type="checkbox" class="chkStrength" style="width:20px" /></td>
                        <td>
                            <input type="text" class="txtDose" value="${Dose}" readonly="readonly"  style="width:40px;height:20px;text-align:right" />
                        </td>
                        <td>
                            <input type="text" class="txtDoseUnit" value="${DoseUnit}" readonly="readonly" style="width:50px;height:20px" />
                        </td>
                    </tr>
                 </table>
            </td>
            <td><input type="text" validationgroup="mpDrugsQuickPicks" class="txtQty" value="1" style="width:40px;text-align:right" /></td>
            <td><input type="text" class="txtItemUnit" value="${ItemUnit}" readonly="readonly" style="width:60px;" /></td>
        </tr>
    </script>
    <input type="hidden" id="hdnSelectedMember" runat="server" value="" />
    <input type="hidden" id="hdnTransactionID" runat="server" value="" />
    <input type="hidden" id="hdnRegistrationID" runat="server" value="" />
    <input type="hidden" id="hdnVisitID" runat="server" value="" />
    <input type="hidden" id="hdnChargeClassID" runat="server" value="" />
    <input type="hidden" id="hdnDefaultGCMedicationRoute" runat="server" value="" />
    <input type="hidden" id="hdnParamedicID" runat="server" value="" />
    <input type="hidden" id="hdnParam" runat="server" value="" />    
    <input type="hidden" id="hdnFilterItem" runat="server" />
    <input type="hidden" id="hdnSelectedMemberStrength" runat="server" value="" />
    <input type="hidden" id="hdnSelectedMemberQty" runat="server" value="" />
    <input type="hidden" id="hdnLocationID" runat="server" value="" />
    <input type="hidden" value="" id="hdnPrescriptionFeeAmount" runat="server" />
    <input type="hidden" id="hdnItemGroupDrugLogisticID" value="" runat="server" />
    <table style="width:100%">
        <colgroup>
            <col style="width:40%"/>
            <col style="width:60%"/>
        </colgroup>
        <tr>
            <td style="padding:5px;vertical-align:top">
                <h4><%=GetLabel("Item yang tersedia di lokasi :")%></h4>
                <table cellspacing="0" width="100%">
                    <colgroup>
                        <col style="width:150px"/>
                        <col style="width:100px"/>
                        <col style="width:250px"/>
                        <col />
                    </colgroup>
                    <tr>
                        <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Lokasi")%></label></td>
                        <td colspan="2">
                            <dxe:ASPxComboBox ID="cboPopupLocation" ClientInstanceName="cboPopupLocation" Width="100%" runat="server" OnCallback="cboPopupLocation_Callback">
                                <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" 
                                  EndCallback="function(s,e){ hideLoadingPanel(); }" 
                                  ValueChanged="function(s,e){ onCboPopupLocationValueChanged(e) }" />
                            </dxe:ASPxComboBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblLink" id="lblItemGroupDrugLogistic"><%=GetLabel("Kelompok Barang")%></label></td>
                        <td><asp:TextBox ID="txtItemGroupDrugLogisticCode" Width="100%" runat="server" /></td>
                        <td><asp:TextBox ID="txtItemGroupDrugLogisticName" Width="100%" runat="server" ReadOnly="true"/></td>
                    </tr>
                    <tr>
                        <td></td>
                        <td></td>
                    </tr>
                </table>
                <dxcp:ASPxCallbackPanel ID="cbpPopup" runat="server" Width="100%" ClientInstanceName="cbpPopup"
                    ShowLoadingPanel="false" OnCallback="cbpPopup_Callback">
                    <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel();}"
                        EndCallback="function(s,e){ onCbpPopupEndCallback(s); }" />
                    <PanelCollection>
                        <dx:PanelContent ID="PanelContent1" runat="server">
                            <asp:Panel runat="server" ID="pnlView" Style="width: 100%; margin-left: auto; margin-right: auto; position: relative;font-size:0.95em;">
                                <asp:GridView ID="grdView" runat="server" CssClass="grdView notAllowSelect" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty"
                                OnRowDataBound="grdView_RowDataBound">
                                    <Columns>
                                        <asp:BoundField DataField="ItemID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField"/>
                                        <asp:BoundField DataField="GCMedicationRoute" HeaderStyle-CssClass="hiddenColumn" ItemStyle-CssClass="hiddenColumn" />
                                        <asp:TemplateField HeaderStyle-CssClass="hiddenColumn" ItemStyle-CssClass="hiddenColumn">
                                            <ItemTemplate>
                                                <table>
                                                    <td><input type="text" class="txtDose number" value="<%#:Eval("cfDose") %>" style="width:100%" readonly="readonly"/></td>
                                                    <td><input type="text" class="txtDoseUnit" value="<%#:Eval("DoseUnit") %>" style="width:100%" readonly="readonly"/></td>                                                
                                                    <td><input type="text" class="txtItemUnit" value="<%#:Eval("ItemUnit") %>" style="width:100%" readonly="readonly"/></td>                                                
                                                </table>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderStyle-Width="20px" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkIsSelected" runat="server" CssClass="chkIsSelected" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="ItemName1" HeaderText="Obat" ItemStyle-CssClass="tdItemName1" />
                                        <asp:BoundField DataField="QuantityEND" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" HeaderText="Stok" HeaderStyle-Width="60px" />
                                        <asp:BoundField DataField="ItemUnit" HeaderText="Satuan" ItemStyle-CssClass="tdItemUnit" HeaderStyle-Width="60px" />
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
                        <div id="pagingPopup"></div>
                    </div>
                </div>
            </td>
            <td style="padding:5px;vertical-align:top">
                <h4><%=GetLabel("Item yang telah dipilih : (Komposisi Racikan)")%></h4>
                <table cellspacing="0" width="100%">
                    <colgroup>
                        <col width="150px" />
                        <col />
                    </colgroup>
                    <tr>
                        <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Nama Racikan")%></label></td>
                        <td><asp:TextBox runat="server" ID="txtCompoundMedicationName" Width="300px" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel" style="vertical-align: top; padding-top: 2px;"><label class="lblNormal"><%=GetLabel("Tujuan Pengobatan")%></label></td>
                        <td><asp:TextBox ID="txtMedicationPurpose" Width="300px" runat="server"/></td>
                    </tr>
                    <tr>
                        <td></td>
                        <td></td>
                    </tr>
                </table>
                <fieldset id="fsDrugsQuickPicks">
                    <div style="height:300px; overflow-y:scroll;">
                        <table id="tblSelectedItem" class="grdView notAllowSelect" cellspacing="0" rules="all">
                        <tr id="trHeader2">
                            <th style="width: 20px">
                                &nbsp;
                            </th>
                            <th align="left">
                                <%=GetLabel("Obat")%>
                            </th>
                            <th align="center" style="width: 60px">
                                <%=GetLabel("Strength")%>
                            </th>
                            <th align="right" style="width: 40px">
                                <%=GetLabel("Dose")%>
                            </th>
                            <th style="width: 60px">
                                <%=GetLabel("Unit")%>
                            </th>
                        </tr>
                        <tr id="trFooter">
                        </tr>
                    </table>
                    </div>
                    <div>
                        <table style="width:100%">
                            <colgroup>
                                <col style="width:100%"/>
                            </colgroup>
                            <tr>
                                <td valign="top">
                                    <table style="width:100%" cellpadding="1" cellspacing="1">
                                        <colgroup>
                                            <col width="110px"/>
                                            <col width="40px" />
                                            <col width="60px" />
                                            <col width="40px" />
                                            <col width="50px" />
                                            <col width="90px"/>
                                            <col width="40px" />
                                            <col width="40px" />
                                            <col  />
                                        </colgroup>
                                        <tr>
                                            <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Frekuensi / Dosis")%></label></td>
                                            <td><asp:TextBox ID="txtFrequencyNumber" runat="server" Width="100%" CssClass="number" /></td>
                                            <td><dxe:ASPxComboBox ID="cboFrequencyTimelineCompoundCtl" runat="server" Width="100%" /></td>
                                            <td><asp:TextBox ID="txtDosingDose" runat="server" Width="100%" CssClass="number" /></td>
                                            <td><dxe:ASPxComboBox ID="cboDosingUnitCompoundCtl" ClientInstanceName="cboDosingUnitCompoundCtl" runat="server" Width="100%" /></td>
                                            <td class="tdLabel" style="padding-left:10px"><label class="lblMandatory"><%=GetLabel("Durasi (hari)")%></label></td>
                                            <td><asp:TextBox runat="server" ID="txtDosingDuration" Width="100%" CssClass="number" /></td>
                                        </tr>
                                        <tr>
                                            <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Rute Obat")%></label></td>
                                            <td colspan="2"><dxe:ASPxComboBox runat="server" ID="cboMedicationRouteCompoundCtl" ClientInstanceName="cboMedicationRouteCompoundCtl" Width="100%" /></td>
                                            <td class="tdLabel"><label class="lblNormal"><%=GetLabel("AC/DC/PC")%></label></td>
                                            <td><dxe:ASPxComboBox runat="server" ID="cboCoenamRuleCompoundCtl" ClientInstanceName="cboCoenamRuleCompoundCtl" Width="100%" /></td>
                                            <td><asp:CheckBox runat="server" ID="chkIsUsingSweetener" /><%=GetLabel(" slqs")%></td>
                                            <td><asp:CheckBox runat="server" ID="chkIsAsRequired" /><%=GetLabel(" PRN")%></td>
                                            <td><asp:CheckBox runat="server" ID="chkIsIMM" /><%=GetLabel(" IMM")%></td>
                                        </tr>
                                        <tr>
                                            <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Mulai Diberikan")%></label></td>
                                            <td colspan="7">
                                                <table cellpadding="0" cellspacing="0" width="100%">
                                                    <colgroup>
                                                        <col style="width:140px"/>
                                                        <col style="width:50px"/>
                                                        <col style="width:5px" />
                                                        <col />
                                                    </colgroup>
                                                    <tr>
                                                        <td><asp:TextBox runat="server" ID="txtStartDate" CssClass="datepicker" Width="110px" /></td>
                                                        <td style="display:none"><asp:TextBox runat="server" ID="txtStartTime" CssClass="time" Width="100%" /></td>
                                                        <td>&nbsp;</td>
                                                        <td style="display:none">
                                                            <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                                                <tr>
                                                                    <td style="width:26%"><asp:CheckBox ID="chkIsMorning" runat="server" Text = " Pagi" Checked="false" /></td>
                                                                    <td style="width:24%"><asp:CheckBox ID="chkIsNoon" runat="server" Text = " Siang" Checked="false" /></td>
                                                                    <td style="width:26%"><asp:CheckBox ID="chkIsEvening" runat="server" Text = " Sore" Checked="false" /></td>
                                                                    <td style="width:24%"><asp:CheckBox ID="chkIsNight" runat="server" Text = " Malam" Checked="false" /></td>
                                                                </tr>
                                                            </table>                                    
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="tdLabel">
                                                <label class="lblNormal">
                                                    <%=GetLabel("Waktu Pemberian")%></label>
                                            </td>
                                            <td colspan="6">
                                                <table border="0" cellpadding="0" cellspacing="0">
                                                    <tr>
                                                        <td style="width: 50px" align="center"">
                                                            <label class="lblNormal">1</label>
                                                        </td>
                                                        <td style="width: 50px" align="center"">
                                                            <label class="lblNormal">2</label>
                                                        </td>
                                                        <td style="width: 50px" align="center"">
                                                            <label class="lblNormal">3</label>
                                                        </td>
                                                        <td style="width: 50px" align="center"">
                                                            <label class="lblNormal">4</label>
                                                        </td>
                                                        <td style="width: 50px" align="center"">
                                                            <label class="lblNormal">5</label>
                                                        <td style="width: 50px" align="center"">
                                                            <label class="lblNormal">6</label>
                                                        </td>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 50px">
                                                            <asp:TextBox runat="server" ID="txtStartTime1" CssClass="time" Width="100%" Text="00:00" />
                                                        </td>
                                                        <td style="width: 50px">
                                                            <asp:TextBox runat="server" ID="txtStartTime2" CssClass="time" Width="100%" Text="00:00" />
                                                        </td>
                                                        <td style="width: 50px">
                                                            <asp:TextBox runat="server" ID="txtStartTime3" CssClass="time" Width="100%" Text="00:00" />
                                                        </td>
                                                        <td style="width: 50px">
                                                            <asp:TextBox runat="server" ID="txtStartTime4" CssClass="time" Width="100%" Text="00:00"/>
                                                        </td>
                                                        <td style="width: 50px">
                                                            <asp:TextBox runat="server" ID="txtStartTime5" CssClass="time" Width="100%" Text="00:00"/>
                                                        </td>
                                                        <td style="width: 50px">
                                                            <asp:TextBox runat="server" ID="txtStartTime6" CssClass="time" Width="100%" Text="00:00"/>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Jumlah Resep")%></label></td>
                                            <td><asp:TextBox runat="server" ID="txtDispenseQty" Width="100%" CssClass="number" /></td>
                                            <td />
                                            <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Diambil")%></label></td>
                                            <td><asp:TextBox runat="server" ID="txtTakenQty" Width="40px" CssClass="number" ReadOnly="true" /></td>
                                            <td class="tdLabel" style="padding-left:10px"><label class="lblNormal"><%=GetLabel("Embalase - Jumlah")%></label></td>
                                            <td>
                                                <table border="0" cellpadding="0" cellspacing="0">
                                                    <tr>
                                                        <td><asp:TextBox runat="server" ID="txtEmbalaceQty" Width="40px" CssClass="number" /></td>
                                                        <td><dxe:ASPxComboBox runat="server" ID="cboEmbalace" ClientInstanceName="cboEmbalace" Width="100%" /></td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="tdLabel" style="vertical-align: top; padding-top: 2px;"><label class="lblNormal"><%=GetLabel("Instruksi Khusus")%></label></td>
                                            <td colspan="7"><asp:TextBox ID="txtMedicationAdministration" Width="100%" runat="server" TextMode="MultiLine" Height="50px" /></td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </div>
                </fieldset>
            </td>
        </tr>
    </table>
        
    <div class="imgLoadingGrdView" id="containerImgLoadingView" >
        <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
    </div> 
</div>