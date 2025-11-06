<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MedicationQuickPicksHistoryCtl1.ascx.cs" 
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.MedicationQuickPicksHistoryCtl1" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="QIS.Medinfras.Web.CustomControl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"
    Namespace="QIS.Medinfras.Web.CustomControl" TagPrefix="qis" %>

<style type="text/css">
    .trSelectedItem {background-color: #ecf0f1 !important;}
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

    function onBeforeSaveRecordEntryPopup() {
        getCheckedMember();
        if ($('#<%=hdnSelectedMember.ClientID %>').val() != '')
            return true;
        else {
            errMessage.text = 'Please select item First before save';
            return false;
        }
        return false;
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

        var result = '';
        $('.grdSelected .chkIsSelected input:checked').each(function () {
            var $tr = $(this).closest('tr');
            var key = $tr.find('.keyField').val();
            var frequency = $tr.find('.txtFrequency').val();
            var qty = $tr.find('.txtQty').val();
            var dispenseQty = $tr.find('.txtDispenseQty').val();

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

        $('#<%=rblVisitPeriod.ClientID %> input').change(function () {
            RefreshGrid(false, 1);
        });

        $('#<%=rblVisitType.ClientID %> input').change(function () {
            RefreshGrid(false, 1);
        });

        $('#<%=rblItemType.ClientID %> input').change(function () {
            RefreshGrid(false, 1);
        });

        $('#<%=grdView.ClientID %> > tbody > tr:gt(0):not(.trEmpty)').live('click', function () {
            if ($(this).attr('class') != 'selected') {
                $('#<%=grdView.ClientID %> tr.selected').removeClass('selected');
                $(this).addClass('selected');
                $('#<%=hdnSelectedVisitID.ClientID %>').val($(this).find('.keyField').html());

                if (typeof (grdHistory) != 'undefined' && typeof (cbpPopupViewDt) != 'undefined')
                    cbpPopupViewDt.PerformCallback('refresh');
                else
                    window.setTimeout("cbpPopupViewDt.PerformCallback('refresh');", 100);
            }
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
        var message = "You've have selected item to proceed. Your action will reset the selection. Do you want to continue ?";
        showToastConfirmation(message, function (result) {
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

<div style="padding:3px;">
    <input type="hidden" id="hdnSelectedMember" runat="server" value="" />
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
    <input type="hidden" id="hdnParam" runat="server" value="" />    
    <input type="hidden" id="hdnFilterItem" runat="server" />
    <input type="hidden" id="hdnSelectedMemberStrength" runat="server" value="" />
    <input type="hidden" id="hdnSelectedMemberFrequency" runat="server" value="" />
    <input type="hidden" id="hdnSelectedMemberPRN" runat="server" value="" />
    <input type="hidden" id="hdnSelectedMemberCoenam" runat="server" value="" />
    <input type="hidden" id="hdnSelectedMemberQty" runat="server" value="" />
    <input type="hidden" id="hdnSelectedMemberDispenseQty" runat="server" value="" />
    <input type="hidden" id="hdnSelectedMemberRemarks" runat="server" value="" />
    <input type="hidden" id="hdnSelectedMemberStartTime" runat="server" value="" />
    <input type="hidden" id="hdnSelectedMemberIsUsingUDD" runat="server" value="" />
    <input type="hidden" id="hdnSelectedMemberRoute" runat="server" value="" />
    <input type="hidden" id="hdnLocationID" runat="server" value="" />
    <input type="hidden" value="" id="hdnPrescriptionFeeAmount" runat="server" />
    <table style="width:100%">
        <colgroup>
            <col style="width:40%"/>
            <col style="width:60%"/>
        </colgroup>
        <tr>
            <td style="padding:2px;vertical-align:top">
                <h4><%=GetLabel("Riwayat Kunjungan Pasien :")%></h4>
                <table cellspacing="0" width="100%">
                    <colgroup>
                        <col style="width:120px"/>
                        <col style="width:100px"/>
                        <col style="width:250px"/>
                        <col />
                    </colgroup>
                    <tr>
                        <td class="tdLabel"><label class="lblNormal" id="lblVisitPeriod"><%=GetLabel("Periode")%></label></td>
                        <td colspan="2">
                            <asp:RadioButtonList ID="rblVisitPeriod" runat="server" RepeatDirection="Horizontal">
                                <asp:ListItem Text="3 Bulan Terakhir" Value="1" Selected="True" />
                                <asp:ListItem Text="6 Bulan Terakhir" Value="2"  />
                                <asp:ListItem Text="Seluruhnya" Value="3"  />
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblNormal" id="lblVisitType"><%=GetLabel("Display")%></label></td>
                        <td colspan="2">
                            <asp:RadioButtonList ID="rblVisitType" runat="server" RepeatDirection="Horizontal">
                                <asp:ListItem Text="All" Value="0" Selected="True" />
                                <asp:ListItem Text="Pasien Dokter" Value="1" />
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                    <tr>
                        <td></td>
                        <td colspan="2"></td>
                    </tr>
                </table>
                <dxcp:ASPxCallbackPanel ID="cbpPopup" runat="server" Width="100%" ClientInstanceName="cbpPopup"
                    ShowLoadingPanel="false" OnCallback="cbpPopup_Callback">
                    <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel();}"
                        EndCallback="function(s,e){ onCbpPopupEndCallback(s); }" />
                    <PanelCollection>
                        <dx:PanelContent ID="PanelContent1" runat="server" CssClass="pnlContainerGridPatientPage">
                            <asp:Panel runat="server" ID="pnlView" Style="width: 100%; margin-top:20px; margin-left: auto; margin-right: auto; position: relative;font-size:0.95em;">
                                <asp:GridView ID="grdView" runat="server" CssClass="grdSelected grdPatientPage" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty"
                                OnRowDataBound="grdView_RowDataBound">
                                    <Columns>
                                        <asp:BoundField DataField="VisitID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                <%=GetLabel("Informasi Kunjungan")%>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <div><b><%#: Eval("cfVisitDate")%></b> <span style="color:Blue"><%#: Eval("ServiceUnitName")%></span></div>
                                                <div> <%#: Eval("ParamedicName")%></div>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <EmptyDataTemplate>
                                        <%=GetLabel("Tidak ada informasi riwayat kunjungan untuk pasien")%>
                                    </EmptyDataTemplate>
                                </asp:GridView>
                            </asp:Panel>
                        </dx:PanelContent>
                    </PanelCollection>
                </dxcp:ASPxCallbackPanel>        
                <div class="imgLoadingGrdView" id="containerImgLoadingView1" >
                    <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                </div> 
                <div class="containerPaging">
                    <div class="wrapperPaging">
                        <div id="pagingPopup"></div>
                    </div>
                </div>
            </td>
            <td style="padding:2px;vertical-align:top">
                <h4><%=GetLabel("Item yang telah dipilih :")%></h4>
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
                                  EndCallback="function(s,e){ hideLoadingPanel(); }"  />
                            </dxe:ASPxComboBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblNormal" id="Label2"><%=GetLabel("Display")%></label></td>
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
                </table>
                <div style="position: relative;">
                    <dxcp:ASPxCallbackPanel ID="cbpPopupViewDt" runat="server" Width="100%" ClientInstanceName="cbpPopupViewDt"
                        ShowLoadingPanel="false" OnCallback="cbpViewDt_Callback">
                        <ClientSideEvents BeginCallback="function(s,e){$('#containerImgLoadingViewDt').show(); }"
                            EndCallback="function(s,e){ onCbpPopupViewDtEndCallback(s); }" />
                        <PanelCollection>
                            <dx:PanelContent ID="PanelContent2" runat="server">
                                <asp:Panel runat="server" ID="Panel1" CssClass="pnlContainerGridPatientPage">
                                    <asp:GridView ID="grdPopupViewDt" runat="server" CssClass="grdSelected grdPatientPage" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                        <Columns>
                                            <asp:TemplateField HeaderStyle-Width="40px" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                <HeaderTemplate>
                                                    <input id="chkSelectAll" type="checkbox" />
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <input type="hidden" class="keyField" value='<%#:Eval("PrescriptionOrderDetailID")%>' />
                                                    <asp:CheckBox ID="chkIsSelected" runat="server" CssClass="chkIsSelected" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Left">
                                                <HeaderTemplate>
                                                    <div><%=GetLabel("Item Name")%></div>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <div><%#: Eval("DrugName")%></div>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-Width="90px" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" HeaderText="Frekuensi" >
                                                <ItemTemplate>
                                                    <input type="hidden" value='<%#:Eval("GCDosingFrequency")%>' class="hdnGCItemUnit" />
                                                    <input type="number" value='<%#:Eval("Frequency")%>' validationgroup="mpMedicationOrderHistoryQty" class="txtFrequency" min="0" value="0" style="width:40px; text-align: right" /> <%#:Eval("DosingFrequency") %>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-Width="90px" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" HeaderText="Dosis" >
                                                <ItemTemplate>
                                                    <input type="text" value='<%#:Eval("cfNumberOfDosage")%>' validationgroup="mpMedicationOrderHistoryQty" class="txtQty" style="width:40px; text-align: right"  /> <%#:Eval("DosingUnit") %>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-Width="100px" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" HeaderText="Jumlah" >
                                                <ItemTemplate>
                                                    <input type="hidden" value='<%#:Eval("GCItemUnit")%>' class="hdnGCItemUnit" />
                                                    <input type="number" value='<%#:Eval("DispenseQty")%>' validationgroup="mpMedicationOrderHistoryQty" class="txtDispenseQty" min="0" value="0" style="width:40px; text-align: right" /> <%#:Eval("ItemUnit") %>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-Width="170px" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" HeaderText="Special Instruction" >
                                                <ItemTemplate>
                                                    <input type="text" value='<%#:Eval("MedicationAdministration")%>' validationgroup="mpMedicationOrderHistoryQty" class="txtMedicationAdministration" style="width:100%"  />
                                                </ItemTemplate>
                                            </asp:TemplateField>
<%--                                            <asp:TemplateField HeaderStyle-Width="170px" HeaderText="Special Instruction" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left">
                                                <ItemTemplate>
                                                    <div style="display:none" class="divSignaID" runat="server" id="divSignaID"></div>
                                                    <qis:QISQuickEntry runat="server" ClientInstanceName="txtMedicationAdministration" ID="txtMedicationAdministration"
                                                        Width="170px" Value='<%#:Eval("MedicationAdministration")%>' CssClass="txtMedicationAdministration">
                                                        <QuickEntryHints>
                                                            <qis:QISQuickEntryHint Text="Signa" ValueField="SignaLabel" TextField="SignaName1"
                                                                Description="Signa" FilterExpression="IsDeleted = 0" MethodName="GetvSignaList">
                                                                <Columns>
                                                                    <qis:QISQuickEntryHintColumn Caption="Signa Label" FieldName="SignaLabel" Width="80px" />
                                                                    <qis:QISQuickEntryHintColumn Caption="Signa Description" FieldName="SignaName1" Width="600px" />
                                                                </Columns>
                                                            </qis:QISQuickEntryHint>
                                                        </QuickEntryHints>
                                                    </qis:QISQuickEntry>
                                                    <input type="hidden" class="hdnDose" />
                                                    <input type="hidden" class="hdnFrequency" />
                                                </ItemTemplate>
                                            </asp:TemplateField>--%>
                                        </Columns>
                                        <EmptyDataTemplate>
                                            <%=GetLabel("No Data To Display")%>
                                        </EmptyDataTemplate>
                                    </asp:GridView>
                                </asp:Panel>
                            </dx:PanelContent>
                        </PanelCollection>
                    </dxcp:ASPxCallbackPanel>    
                    <div class="imgLoadingGrdView" id="containerImgLoadingViewDt" >
                        <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                    </div>
                    <div class="containerPaging">
                        <div class="wrapperPaging">
                            <div id="pagingDt1"></div>
                        </div>
                    </div> 
                </div>
            </td>
        </tr>
    </table>
</div>