<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PrescriptionEntryTemplateCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.Pharmacy.Program.PrescriptionEntryTemplateCtl" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<script type="text/javascript" id="dxss_templatePharmacyctl">
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
    });

    function onBeforeSaveRecord(errMessage) {
        if (IsValid(null, 'fsDrugsQuickPicks', 'mpDrugsQuickPicks')) {
            getCheckedMember();
            if ($('#<%=hdnSelectedMemberTemplate.ClientID %>').val() != '')
                return true;
            else {
                errMessage.text = 'Please Select Item First';
                return false;
            }
        }
    }

    function getCheckedMember() {
        var lstSelectedMember = [];
        var lstSelectedMemberSigna = [];
        var lstSelectedMemberCoenam = [];
        var lstSelectedMemberQty = [];
        var lstSelectedMemberPrn = [];
        var lstSelectedMemberQty = [];
        var lstSelectedMemberDispenseQty = [];
        var lstSelectedMemberRemarks = [];
        var lstSelectedMemberRoute = [];
        var lstSelectedMemberDosingUnit = [];
        var result = '';
        $('#tblSelectedItemTemplate .trSelectedItemTemplate').each(function () {
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

            lstSelectedMember.push(key);
            lstSelectedMemberSigna.push(signa);
            lstSelectedMemberCoenam.push(coenamRule);
            lstSelectedMemberPrn.push(isPRN);
            lstSelectedMemberQty.push(qty);
            lstSelectedMemberDosingUnit.push(dosingUnit);
            lstSelectedMemberDispenseQty.push(dispenseQty);
            lstSelectedMemberRemarks.push(medicationAdministration);
            lstSelectedMemberRoute.push(route);
        });
        $('#<%=hdnSelectedMemberTemplate.ClientID %>').val(lstSelectedMember.join(','));
        $('#<%=hdnSelectedMemberSigna.ClientID %>').val(lstSelectedMemberSigna.join(','));
        $('#<%=hdnSelectedMemberCoenam.ClientID %>').val(lstSelectedMemberCoenam.join(','));
        $('#<%=hdnSelectedMemberPRN.ClientID %>').val(lstSelectedMemberPrn.join(','));
        $('#<%=hdnSelectedMemberQty.ClientID %>').val(lstSelectedMemberQty.join(','));
        $('#<%=hdnSelectedMemberDosingUnit.ClientID %>').val(lstSelectedMemberDosingUnit.join(','));
        $('#<%=hdnSelectedMemberDispenseQty.ClientID %>').val(lstSelectedMemberDispenseQty.join(','));
        $('#<%=hdnSelectedMemberRemarks.ClientID %>').val(lstSelectedMemberRemarks.join('|'));
        $('#<%=hdnSelectedMemberRoute.ClientID %>').val(lstSelectedMemberRoute.join(','));
    }

    //#region Template
    $('#lblTemplatePharmacyPanel.lblLink').live('click', function () {
        var filter = "IsDeleted = 0";
        openSearchDialog('chargestemplatehd', filter, function (value) {
            $('#<%=txtTemplateCode.ClientID %>').val(value);
            onTxtTemplateCodeChanged(value);
        });
    });

    $('#<%=txtTemplateCode.ClientID %>').live('change', function () {
        onTxtTemplateCodeChanged($(this).val());
    });

    function onTxtTemplateCodeChanged(value) {
        var filterExpression = "ChargesTemplateCode = '" + value + "' AND ChargesIsDeletedHd = 0 AND  ChargesIsDeletedDt = 0";
        Methods.getObject('GetvChargesTemplateQuickPickList', filterExpression, function (result) {
            if (result != null) {
                $('#<%=hdnChargesTemplateID.ClientID %>').val(result.ChargesTemplateID);
                $('#<%=txtTemplateName.ClientID %>').val(result.ChargesTemplateName);
            }
            else {
                $('#<%=hdnChargesTemplateID.ClientID %>').val('');
                $('#<%=txtTemplateCode.ClientID %>').val('');
                $('#<%=txtTemplateName.ClientID %>').val('');
            }
        });
        cbpViewCtl.PerformCallback('refresh');
    }
    //#endregion

    //#region Paging
    var pageCount = parseInt('<%=PageCount %>');

    $(function () {
        setPaging($("#pagingPopup"), pageCount, function (page) {
            getCheckedMember();
            cbpPopup.PerformCallback('changepage|' + page);
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

    function oncbpViewCtlEndCallback(s) {
        hideLoadingPanel();
        var param = s.cpResult.split('|');
        if (param[0] == 'refresh') {
            var pageCount = parseInt(param[1]);
            setPaging($("#pagingPopup"), pageCount, function (page) {
                getCheckedMember();
                cbpViewCtl.PerformCallback('changepage|' + page);
            });
        }
        addItemFilterRow();
    }

    $('#<%=grdView.ClientID %> .chkIsSelected input').die('change');
    $('#<%=grdView.ClientID %> .chkIsSelected input').live('change', function () {
        if ($(this).is(':checked')) {
            $selectedTr = $(this).closest('tr');

            $newTr = $('#tmplSelectedItemTemplate').html();
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
            $('#tblSelectedItemTemplate tr').each(function () {
                if ($(this).find('.keyField').val() == id) {
                    $(this).remove();
                }
            });
        }
    });

    $('#tblSelectedItemTemplate .chkIsSelectedTemplate').die('change');
    $('#tblSelectedItemTemplate .chkIsSelectedTemplate').live('change', function () {
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
                var lstSelectedMember = $('#<%=hdnSelectedMemberTemplate.ClientID %>').val().split(',');
                lstSelectedMember.splice(lstSelectedMember.indexOf(id), 1);
                $('#<%=hdnSelectedMemberTemplate.ClientID %>').val(lstSelectedMember.join(','));
            }
            $(this).closest('tr').remove();
        }
    });
  
</script>
<div style="padding: 10px;">
    <script id="tmplSelectedItemTemplate" type="text/x-jquery-tmpl">
        <tr class="trSelectedItemTemplate">
            <td align="center">
                <input type="checkbox" class="chkIsSelectedTemplate" />
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
    <input type="hidden" id="hdnParam" runat="server" value="" />
    <input type="hidden" id="hdnTransactionID" runat="server" value="" />
    <input type="hidden" id="hdnLocationID" runat="server" value="" />
    <input type="hidden" id="hdnDefaultGCMedicationRoute" runat="server" value="" />
    <input type="hidden" id="hdnParamedicID" runat="server" value="" />
    <input type="hidden" id="hdnRegistrationID" runat="server" value="" />
    <input type="hidden" id="hdnVisitID" runat="server" value="" />
    <input type="hidden" id="hdnFilterItem" runat="server" value="" />
    <input type="hidden" id="hdnChargeClassID" runat="server" value="" />
    <input type="hidden" id="hdnIsDrugChargesJustDistributionQP" runat="server" value="0" />
    <input type="hidden" id="hdnBusinessPartnerIDCtl" runat="server" value="" />
    <input type="hidden" id="hdnPrescriptionFeeAmount" runat="server" value="" />
    <input type="hidden" id="hdnDefaultEmbalaceIDCtl" value="" runat="server" />
    <input type="hidden" id="hdnIsAutoInsertEmbalaceCtl" value="" runat="server" />
    <input type="hidden" id="hdnIsGenerateQueueLabel" value="0" runat="server" />
    <input type="hidden" id="hdnItemQtyWithSpecialQueuePrefix" value="0" runat="server" />
    <input type="hidden" id="hdnChargesTemplateID" value="0" runat="server" />
    <input type="hidden" id="hdnSelectedMemberTemplate" runat="server" value="" />
    <input type="hidden" id="hdnSelectedMemberStrength" runat="server" value="" />
    <input type="hidden" id="hdnSelectedMemberSigna" runat="server" value="" />
    <input type="hidden" id="hdnSelectedMemberCoenam" runat="server" value="" />
    <input type="hidden" id="hdnSelectedMemberPRN" runat="server" value="" />
    <input type="hidden" id="hdnSelectedMemberQty" runat="server" value="" />
    <input type="hidden" id="hdnSelectedMemberDosingUnit" runat="server" value="" />
    <input type="hidden" id="hdnSelectedMemberDispenseQty" runat="server" value="" />
    <input type="hidden" id="hdnSelectedMemberRemarks" runat="server" value="" />
    <input type="hidden" id="hdnSelectedMemberStartTime" runat="server" value="" />
    <input type="hidden" id="hdnSelectedMemberRoute" runat="server" value="" />
    <input type="hidden" id="hdnIsEndingAmountRoundingTo100" runat="server" value="0" />
    <input type="hidden" id="hdnIsEndingAmountRoundingTo1" runat="server" value="0" />
    <input type="hidden" value="" id="hdnHealthcareServiceUnitID" runat="server" />
    <table>
        <colgroup>
            <col style="width: 150px" />
            <col style="width: 400px" />
        </colgroup>
        <tr>
            <td class="tdLabel">
                <label class="lblMandatory">
                    <%=GetLabel("Lokasi")%></label>
            </td>
            <td>
                <dxe:ASPxComboBox ID="cboPopupLocation" ClientInstanceName="cboPopupLocation" Width="100%"
                    runat="server" OnCallback="cboPopupLocation_Callback">
                    <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ hideLoadingPanel(); }"
                        ValueChanged="function(s,e){ onCboPopupLocationValueChanged(e) }" />
                </dxe:ASPxComboBox>
            </td>
        </tr>
        <tr>
            <td class="tdLabel">
                <label class="lblLink" id="lblTemplatePharmacyPanel">
                    <%=GetLabel("Template")%></label>
            </td>
            <td>
                <table style="width: 100%" cellpadding="0" cellspacing="0">
                    <colgroup>
                        <col style="width: 30%" />
                        <col style="width: 3px" />
                        <col />
                    </colgroup>
                    <tr>
                        <td>
                            <asp:TextBox ID="txtTemplateCode" Width="100%" runat="server" />
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            <asp:TextBox ID="txtTemplateName" Width="100%" runat="server" ReadOnly="true" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <div style="height: 400px; overflow-y: scroll;">
        <table style="width: 100%">
            <colgroup>
                <col style="width: 50%" />
                <col style="width: 50%" />
            </colgroup>
            <tr>
                <td style="padding: 5px; vertical-align: top">
                    <dxcp:ASPxCallbackPanel ID="cbpViewCtl" runat="server" Width="100%" ClientInstanceName="cbpViewCtl"
                        ShowLoadingPanel="false" OnCallback="cbpViewCtl_Callback">
                        <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel();}" EndCallback="function(s,e){ oncbpViewCtlEndCallback(s); }" />
                        <PanelCollection>
                            <dx:PanelContent ID="PanelContent1" runat="server">
                                <asp:Panel runat="server" ID="pnlView" Style="width: 100%; margin-left: auto; margin-right: auto;
                                    position: relative;">
                                    <asp:GridView ID="grdView" runat="server" CssClass="grdView notAllowSelect" AutoGenerateColumns="false"
                                        ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty" OnRowDataBound="grdView_RowDataBound">
                                        <Columns>
                                            <asp:BoundField DataField="ItemID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                            <asp:TemplateField HeaderStyle-Width="20px" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chkIsSelected" runat="server" CssClass="chkIsSelected" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="ItemName1" HeaderText="Obat dan Persediaan Medis" ItemStyle-CssClass="tdItemName1" />
                                            <asp:BoundField DataField="QuantityEND" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right"
                                                HeaderText="Stok" HeaderStyle-Width="60px" />
                                            <asp:BoundField DataField="TotalQtyOnHand" ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right"
                                                HeaderText="Stok RS" HeaderStyle-Width="60px" />
                                            <asp:BoundField DataField="ItemUnit" HeaderText="Satuan" ItemStyle-CssClass="tdItemUnit"
                                                HeaderStyle-Width="60px" />
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
                    <fieldset id="fsDrugsQuickPicks">
                        <table id="tblSelectedItemTemplate" class="grdView notAllowSelect" cellspacing="0"
                            rules="all">
                            <tr id="trHeader2">
                                <th style="width: 20px">
                                    &nbsp;
                                </th>
                                <th align="left">
                                    <%=GetLabel("Obat dan Persediaan Medis")%>
                                    <br />
                                    <span style='font-style: italic'>
                                        <%=GetLabel("Instruksi Khusus")%></span>
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
                </td>
            </tr>
        </table>
    </div>
    <div class="imgLoadingGrdView" id="containerImgLoadingView">
        <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
    </div>
</div>
