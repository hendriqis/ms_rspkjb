<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MedicationOrderQuickPicksCtl.ascx.cs" 
    Inherits="QIS.Medinfras.Web.Pharmacy.Program.MedicationOrderQuickPicksCtl" %>

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

    function onBeforeSaveRecord(errMessage) {
        //        if (IsValid(null, 'fsDrugsQuickPicks', 'mpDrugsQuickPicks')) {
        getCheckedMember();
        if ($('#<%=hdnSelectedMember.ClientID %>').val() != '')
            return true;
        else {
            errMessage.text = 'Please Select Item First';
            return false;
        }
        //        }
        return false;
    }

    function getCheckedMember() {
        var lstSelectedMember = [];
        var lstSelectedMemberStrength = [];
        var lstSelectedMemberSigna = [];
        var lstSelectedMemberCoenam = [];
        var lstSelectedMemberQty = [];
        var lstSelectedMemberPrn = [];
        var lstSelectedMemberDuration = [];
        var lstSelectedMemberStartDate = [];
        var lstSelectedMemberStartTime = [];
        var lstSelectedMemberIsUsingUDD = [];
        var lstSelectedMemberRoute = [];

        var result = '';
        $('#tblSelectedItem .trSelectedItem').each(function () {
            var key = $(this).find('.keyField').val();
            var route = $(this).find('.hiddenColumn').val();
            var frequencyNo = $(this).find('.txtFrequencyNo').val();
            var frequencyType = $(this).find('.ddlFrequency').val();
            var signa = frequencyNo + frequencyType;
            var coenamRule = $(this).find('.ddlCoenamRule').val();

            var isStrength = '0';
            if ($(this).find('.chkStrength').is(':checked')) {
                isStrength = 1;
            }

            var isPRN = '0';
            if ($(this).find('.chkPrn').is(':checked')) {
                isPRN = '1';
            }
            var qty = $(this).find('.txtQty').val();
            var duration = $(this).find('.txtDuration').val();

            var startDate = $(this).find('.txtStartDate').val();
            var startTime = $(this).find('.txtStartTime').val();

            var isUsingUDD = '0';
            if ($(this).find('.chkIsUsingUDD').is(':checked')) {
                isUsingUDD = '1';
            }
            lstSelectedMember.push(key);
            lstSelectedMemberStrength.push(isStrength);
            lstSelectedMemberSigna.push(signa);
            lstSelectedMemberCoenam.push(coenamRule);
            lstSelectedMemberPrn.push(isPRN);
            lstSelectedMemberQty.push(qty);
            lstSelectedMemberDuration.push(duration);
            lstSelectedMemberStartDate.push(startDate);
            lstSelectedMemberStartTime.push(startTime);
            lstSelectedMemberIsUsingUDD.push(isUsingUDD);
            lstSelectedMemberRoute.push(route);
        });
        $('#<%=hdnSelectedMember.ClientID %>').val(lstSelectedMember.join(','));
        $('#<%=hdnSelectedMemberStrength.ClientID %>').val(lstSelectedMemberStrength.join(','));
        $('#<%=hdnSelectedMemberSigna.ClientID %>').val(lstSelectedMemberSigna.join(','));
        $('#<%=hdnSelectedMemberCoenam.ClientID %>').val(lstSelectedMemberCoenam.join(','));
        $('#<%=hdnSelectedMemberPRN.ClientID %>').val(lstSelectedMemberPrn.join(','));
        $('#<%=hdnSelectedMemberQty.ClientID %>').val(lstSelectedMemberQty.join(','));
        $('#<%=hdnSelectedMemberDuration.ClientID %>').val(lstSelectedMemberDuration.join(','));
        $('#<%=hdnSelectedMemberStartDate.ClientID %>').val(lstSelectedMemberStartDate.join(','));
        $('#<%=hdnSelectedMemberStartTime.ClientID %>').val(lstSelectedMemberStartTime.join(','));
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
            var defaultStartDate = $('#<%=txtDefaultStartDate.ClientID %>').val();
            var defaultStartTime = '08:00';

            $newTr = $('#tmplSelectedTestItem').html();
            $newTr = $newTr.replace(/\$\{ItemName1}/g, $selectedTr.find('.tdItemName1').html());
            $newTr = $newTr.replace(/\$\{ItemID}/g, $selectedTr.find('.keyField').html());
            $newTr = $newTr.replace(/\$\{GCMedicationRoute}/g, $selectedTr.find('.hiddenColumn').html());
            $newTr = $newTr.replace(/\$\{StartDate}/g, defaultStartDate);
            $newTr = $newTr.replace(/\$\{StartTime}/g, defaultStartTime);
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
</script>

<div style="padding:10px;">
    <script id="tmplSelectedTestItem" type="text/x-jquery-tmpl">
        <tr class="trSelectedItem">
            <td align="center">
                <input type="checkbox" class="chkIsSelected2" />
                <input type="hidden" class="keyField" value='${ItemID}' />
                <input type="hidden" class="hiddenColumn" value='${GCMedicationRoute}' />
            </td>
            <td>${ItemName1}</td>
            <td><input type="checkbox" class="chkIsUsingUDD" style="width:20px" /></td>
            <td><input type="checkbox" class="chkStrength" style="width:20px" /></td>
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
            <td style="text-align:right"><input type="text" validationgroup="mpDrugsQuickPicks" class="txtQty number min" value="1" style="width:40px;" /></td>
            <td> <select class="ddlCoenamRule" style="width:50px">
                      <option value="-"> </option>
                      <option value="ac">ac</option>
                      <option value="dc">dc</option>
                      <option value="pc">pc</option>
                 </select> 
            </td>
            <td><input type="checkbox" class="chkPrn" style="width:20px" /></td>
            <td style="text-align:right"><input type="text" validationgroup="mpDrugsQuickPicks" class="txtDuration number min" min="1" value="1" style="width:40px" /></td>
            <td style="text-align:center"><input type="text" validationgroup="mpDrugsQuickPicks" class="txtStartDate datepicker" style="width:80px" value="${StartDate}" /></td>
            <td style="text-align:center"><input type="text" validationgroup="mpDrugsQuickPicks" class="txtStartTime time" style="width:40px" value="${StartTime}" /></td>
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
    <input type="hidden" id="hdnSelectedMemberSigna" runat="server" value="" />
    <input type="hidden" id="hdnSelectedMemberPRN" runat="server" value="" />
    <input type="hidden" id="hdnSelectedMemberCoenam" runat="server" value="" />
    <input type="hidden" id="hdnSelectedMemberQty" runat="server" value="" />
    <input type="hidden" id="hdnSelectedMemberDuration" runat="server" value="" />
    <input type="hidden" id="hdnSelectedMemberStartDate" runat="server" value="" />
    <input type="hidden" id="hdnSelectedMemberStartTime" runat="server" value="" />
    <input type="hidden" id="hdnSelectedMemberIsUsingUDD" runat="server" value="" />
    <input type="hidden" id="hdnSelectedMemberRoute" runat="server" value="" />
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
                                </th>
                                <th align="center" style="width: 20px">
                                    <%=GetLabel("UDD")%>
                                </th>
                                <th align="center" style="width: 20px">
                                    <%=GetLabel("Kadar")%>
                                </th>
                                <th align="right" style="width: 60px">
                                    <%=GetLabel("Frekuensi")%>
                                </th>
                                <th align="right" style="width: 40px">
                                    <%=GetLabel("Dosis")%>
                                </th>
                                <th align="right" style="width: 40px">
                                    <%=GetLabel("ac/dc/pc")%>
                                </th>
                                <th align="center" style="width: 20px">
                                    <%=GetLabel("prn")%>
                                </th>
                                <th align="right" style="width: 40px">
                                    <%=GetLabel("Durasi")%>
                                </th>
                                <th align="center" style="width: 80px">
                                    <%=GetLabel("Mulai")%>
                                </th>
                                <th align="center" style="width: 40px">
                                    <%=GetLabel("Waktu")%>
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