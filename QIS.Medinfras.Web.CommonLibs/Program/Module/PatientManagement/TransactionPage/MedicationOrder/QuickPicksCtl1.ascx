<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="QuickPicksCtl1.ascx.cs" 
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.QuickPicksCtl1" %>

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
        setDatePicker('<%=txtDefaultStartDate.ClientID %>');
        $('#<%=txtDefaultStartDate.ClientID %>').datepicker('option', 'minDate', '0');
        $('#<%=txtDefaultStartDate.ClientID %>').val(getDateNowDatePickerFormat());
    });

    function onBeforeSaveRecord() {
        //        if (IsValid(null, 'fsDrugsQuickPicks', 'mpDrugsQuickPicks')) {
        getCheckedMember();
        if ($('#<%=hdnSelectedMember.ClientID %>').val() != '')
            return true;
        else {
            errMessage.text = 'Belum ada item yang dipilih untuk diproses';
            return false;
        }
        //        }
        return false;
    }

    function getCheckedMember() {
        var lstSelectedMember = [];
        var lstSelectedMemberSigna = [];
        var lstSelectedMemberCoenam = [];
        var lstSelectedMemberQty = [];
        var lstSelectedMemberPrn = [];
        var lstSelectedMemberDispenseQty = [];
        var lstSelectedMemberRemarks = [];
        var lstSelectedMemberIsUsingUDD = [];
        var lstSelectedMemberRoute = [];
        var lstSelectedMemberDosingUnit = [];

        var result = '';
        $('#tblSelectedItem .trSelectedItem').each(function () {
            var key = $(this).find('.keyField').val();
            var route = $(this).find('.hiddenColumn').val();
            var frequencyNo = $(this).find('.txtFrequencyNo').val();
            var frequencyType = $(this).find('.ddlFrequency').val();
            var signa = frequencyNo + frequencyType;
            var dosingUnit = $(this).find('.ddlDosingUnit').val();
            var coenamRule = $(this).find('.ddlCoenamRule').val();
            var dosingUnit = $(this).find('.ddlDosingUnit').val();

            var isPRN = '0';
            if ($(this).find('.chkPrn').is(':checked')) {
                isPRN = '1';
            }
            var qty = $(this).find('.txtQty').val();
            var dispenseQty = $(this).find('.txtDispenseQty').val();

            var medicationAdministration = $(this).find('.txtMedicationAdministration').val();

            var isUsingUDD = '0';

            lstSelectedMember.push(key);
            lstSelectedMemberSigna.push(signa);
            lstSelectedMemberCoenam.push(coenamRule);
            lstSelectedMemberPrn.push(isPRN);
            lstSelectedMemberQty.push(qty);
            lstSelectedMemberDosingUnit.push(dosingUnit);
            lstSelectedMemberDispenseQty.push(dispenseQty);
            lstSelectedMemberRemarks.push(medicationAdministration);
            lstSelectedMemberIsUsingUDD.push(isUsingUDD);
            lstSelectedMemberRoute.push(route);
        });
        $('#<%=hdnSelectedMember.ClientID %>').val(lstSelectedMember.join(','));
        $('#<%=hdnSelectedMemberSigna.ClientID %>').val(lstSelectedMemberSigna.join(','));
        $('#<%=hdnSelectedMemberCoenam.ClientID %>').val(lstSelectedMemberCoenam.join(','));
        $('#<%=hdnSelectedMemberPRN.ClientID %>').val(lstSelectedMemberPrn.join(','));
        $('#<%=hdnSelectedMemberQty.ClientID %>').val(lstSelectedMemberQty.join(','));
        $('#<%=hdnSelectedMemberDosingUnit.ClientID %>').val(lstSelectedMemberDosingUnit.join(','));
        $('#<%=hdnSelectedMemberDispenseQty.ClientID %>').val(lstSelectedMemberDispenseQty.join(','));
        $('#<%=hdnSelectedMemberRemarks.ClientID %>').val(lstSelectedMemberRemarks.join('|'));
        $('#<%=hdnSelectedMemberIsUsingUDD.ClientID %>').val(lstSelectedMemberIsUsingUDD.join(','));
        $('#<%=hdnSelectedMemberRoute.ClientID %>').val(lstSelectedMemberRoute.join(','));
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

    $('#<%=grdView.ClientID %> .chkIsSelected input').die('change');
    $('#<%=grdView.ClientID %> .chkIsSelected input').live('change', function () {
        if ($(this).is(':checked')) {
            $selectedTr = $(this).closest('tr');

            $newTr = $('#tmplSelectedTestItem').html();
            $newTr = $newTr.replace(/\$\{ItemName1}/g, $selectedTr.find('.tdItemName1').html());
            $newTr = $newTr.replace(/\$\{ItemID}/g, $selectedTr.find('.keyField').html());
            $newTr = $newTr.replace(/\$\{GCMedicationRoute}/g, $selectedTr.find('.hiddenColumn').html());
            $newTr = $newTr.replace(/\$\{MedicationAdministration}/g, '');
            $newTr = $newTr.replace(/\$\{ItemUnit}/g, $selectedTr.find('.tdItemUnit').html());

            // Populate Item Unit Drop Down List
            var unitCodes = ('<%=GetItemUnitListCode() %>').split(",");
            var unitText = ('<%=GetItemUnitListText() %>').split(",");
            var htmlText = '<select class="ddlDosingUnit" style="width:100px">';
            for (var i = 0; i < unitCodes.length; i++) {
                if (unitText[i] == $selectedTr.find('.tdItemUnit').html()) {
                    htmlText += '<option selected value="' + unitCodes[i] + '">' + unitText[i] + '</option>';
                }
                else {
                    htmlText += '<option value="' + unitCodes[i] + '">' + unitText[i] + '</option>';
                }
            }
            htmlText += '</select>';
            $newTr = $newTr.replace(/\$\{ddlItemUnit}/g, htmlText);

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
        if (typeof onAfterSaveDetail == 'function')
            onAfterSaveDetail(value);
    }



</script>

<div style="padding:10px;">
    <script id="tmplSelectedTestItem" type="text/x-jquery-tmpl">
        <tr class="trSelectedItem">
            <td align="center">
                <input type="checkbox" class="chkIsSelected2" />
                <input type="hidden" class="keyField" value='${ItemID}' />
                <input type="hidden" class="hiddenColumn" value='${GCMedicationRoute}' />
            </td>
            <td>
                <table border="0" cellpadding="0" cellspacing="1" style="width:100%">
                    <tr>
                        <td>
                            ${ItemName1}                    
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <input type="text" validationgroup="mpDrugsQuickPicks" class="txtMedicationAdministration" style="width:100%" value="${MedicationAdministration}" />
                        </td>
                    </tr>
                </table>
            </td>
            <td> 
                 <table border="0" cellpadding="0" cellspacing="1" style="width:100%">
                    <tr>
                        <td>
                            <input type="text" validationgroup="mpDrugsQuickPicks" class="txtFrequencyNo number min" min="1" value="1" style="width:20px;height:20px" />
                        </td>
                        <td>
                             <select class="ddlFrequency" style="width:40px">
                                  <option value="dd">dd</option>
                                  <option value="qh">qh</option>
                             </select>
                        </td>
                    </tr>
                 </table>
            </td>
            <td style="text-align:right">
                <table border="0" cellpadding="0" cellspacing="1" style="width:100%">
                    <tr>
                        <td>
                            <input type="text" validationgroup="mpDrugsQuickPicks" class="txtQty number min" value="1" style="width:30px;" />
                        </td>
                        <td>
                             ${ddlItemUnit}   
                        </td>
                    </tr>
                </table>
            </td>
            <td> <select class="ddlCoenamRule" style="width:40px">
                      <option value="-"> </option>
                      <option value="ac">ac</option>
                      <option value="dc">dc</option>
                      <option value="pc">pc</option>
                 </select> 
            </td>
            <td><input type="checkbox" class="chkPrn" style="width:20px" /></td>
            <td style="text-align:right">
                <table border="0" cellpadding="0" cellspacing="1" style="width:100%">
                    <td>
                        <input type="text" validationgroup="mpDrugsQuickPicks" class="txtDispenseQty number min" min="1" value="1" style="width:40px" />
                    </td>
                    <td>
                        ${ItemUnit}
                    </td>
                </table>
            </td>
        </tr>
    </script>
    <input type="hidden" id="hdnSelectedMember" runat="server" value="" />
    <input type="hidden" id="hdnOrderID" runat="server" value="" />
    <input type="hidden" id="hdnTransactionID" runat="server" value="" />
    <input type="hidden" id="hdnRegistrationID" runat="server" value="" />
    <input type="hidden" id="hdnVisitID" runat="server" value="" />
    <input type="hidden" id="hdnChargeClassID" runat="server" value="" />
    <input type="hidden" id="hdnDefaultGCMedicationRoute" runat="server" value="" />
    <input type="hidden" id="hdnParamedicID" runat="server" value="" />
    <input type="hidden" id="hdnDispensaryUnitID" runat="server" value="" />
    <input type="hidden" id="hdnPrescriptionType" runat="server" value="" />
    <input type="hidden" id="hdnPrescriptionDate" value="" runat="server" />
    <input type="hidden" id="hdnPrescriptionTime" value="" runat="server" />
    <input type="hidden" id="hdnParam" runat="server" value="" />    
    <input type="hidden" id="hdnFilterItem" runat="server" />
    <input type="hidden" id="hdnSelectedMemberStrength" runat="server" value="" />
    <input type="hidden" id="hdnSelectedMemberSigna" runat="server" value="" />
    <input type="hidden" id="hdnSelectedMemberPRN" runat="server" value="" />
    <input type="hidden" id="hdnSelectedMemberCoenam" runat="server" value="" />
    <input type="hidden" id="hdnSelectedMemberQty" runat="server" value="" />
    <input type="hidden" id="hdnSelectedMemberDosingUnit" runat="server" value="" />
    <input type="hidden" id="hdnSelectedMemberDispenseQty" runat="server" value="" />
    <input type="hidden" id="hdnSelectedMemberRemarks" runat="server" value="" />
    <input type="hidden" id="hdnSelectedMemberStartTime" runat="server" value="" />
    <input type="hidden" id="hdnSelectedMemberIsUsingUDD" runat="server" value="" />
    <input type="hidden" id="hdnSelectedMemberRoute" runat="server" value="" />
    <input type="hidden" id="hdnLocationID" runat="server" value="" />
    <input type="hidden" value="" id="hdnPrescriptionFeeAmount" runat="server" />
    <input type="hidden" id="hdnItemGroupDrugLogisticID" value="" runat="server" />
    <input type="hidden" value="" id="hdnMenuType" runat="server" />
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
                        <col style="width:120px"/>
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
                        <td class="tdLabel"><label class="lblLink" id="lblItemGroupDrugLogistic"><%=GetLabel("Group")%></label></td>
                        <td><asp:TextBox ID="txtItemGroupDrugLogisticCode" Width="100%" runat="server" /></td>
                        <td><asp:TextBox ID="txtItemGroupDrugLogisticName" Width="100%" runat="server" ReadOnly="true"/></td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblNormal" id="Label1"><%=GetLabel("Source")%></label></td>
                        <td colspan="2">
                            <asp:RadioButtonList ID="rblItemSource" runat="server" RepeatDirection="Horizontal">
                                <asp:ListItem Text="Master" Value="1" Selected="True" />
                                <asp:ListItem Text="History (Not Compound Only)" Value="2"  />
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3" class="tdLabel"><label class="lblNormal" id="Label2"><%=GetLabel("Display")%></label></td>
                    </tr>
                    <tr>
                        <td colspan="3">
                            <asp:RadioButtonList ID="rblItemType" runat="server" RepeatDirection="Horizontal">
                                <asp:ListItem Text="All" Value="0" Selected="True" />
                                <asp:ListItem Text="Formularium" Value="1" />
                                <asp:ListItem Text="BPJS" Value="2" />
                                <asp:ListItem Text="Inhealth" Value="3" />
                                <asp:ListItem Text="Obat Karyawan" Value="4" />
                            </asp:RadioButtonList>
                        </td>
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
                                        <asp:BoundField DataField="GCMedicationRoute" HeaderStyle-CssClass="hiddenColumn" ItemStyle-CssClass="hiddenColumn"/>
                                        <asp:TemplateField HeaderStyle-Width="20px" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkIsSelected" runat="server" CssClass="chkIsSelected" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="ItemName1" HeaderText="Obat dan Persediaan Medis" ItemStyle-CssClass="tdItemName1" />
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
                <h4><%=GetLabel("Item yang telah dipilih :")%></h4>
                <table cellspacing="0" width="100%">
                    <colgroup>
                        <col width="150px" />
                        <col />
                    </colgroup>
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
                            <%=GetLabel("Keterangan Order") %>
                        </td>
                        <td>
                            <asp:TextBox ID="txtRemarks" runat="server" Width="100%" TextMode="Multiline"
                                Rows="2" />
                        </td>
                    </tr>
                </table>
                <div style="height:400px; overflow-y:scroll;">
                    <fieldset id="fsDrugsQuickPicks">
                        <table id="tblSelectedItem" class="grdView notAllowSelect" cellspacing="0" rules="all">
                            <tr id="trHeader2">
                                <th style="width: 20px">
                                    &nbsp;
                                </th>
                                <th align="left">
                                    <%=GetLabel("Obat dan Persediaan Medis")%>
                                    <br />
                                    <span style='font-style:italic'><%=GetLabel("Instruksi Khusus")%></span>
                                </th>
                                <th align="right" style="width: 60px">
                                    <%=GetLabel("Frekuensi")%>
                                </th>
                                <th align="right" style="width: 40px">
                                    <%=GetLabel("Dosis")%>
                                </th>
                                <th align="right" style="width: 40px">
                                    ac/dc/
                                    <br />
                                    pc
                                </th>
                                <th align="center" style="width: 20px">
                                    <%=GetLabel("prn")%>
                                </th>
                                <th align="right" style="width: 40px">
                                    <%=GetLabel("Jumlah R/")%>
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
        
    <div class="imgLoadingGrdView" id="containerImgLoadingView" >
        <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
    </div> 
</div>