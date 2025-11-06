<%@ Page Language="C#" MasterPageFile="~/MasterPage/MPPatientPageListEntry.master" AutoEventWireup="true" 
    CodeBehind="SurgeryOrderEntry.aspx.cs" Inherits="QIS.Medinfras.Web.EMR.Program.SurgeryOrderEntry" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="QIS.Medinfras.Web.CustomControl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null" 
    Namespace="QIS.Medinfras.Web.CustomControl" TagPrefix="qis" %>

<asp:Content ID="Content2" ContentPlaceHolderID="plhCustomButtonToolbar" runat="server">
    <li id="btnBackTestOrderList" runat="server" CRUDMode="R"><img src='<%=ResolveUrl("~/Libs/Images/Icon/back.png")%>' alt="" /><div><%=GetLabel("Back to Order List")%></div></li>
    <li id="btnSendOrder" runat="server" crudmode="R">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbProcess.png")%>' alt="" /><div>
            <%=GetLabel("Send Order")%></div>
    </li>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhRightButtonToolbar" runat="server">
    <li id="btnOpenTestOrderEntryQuickPicks" runat="server" CRUDMode="C"><img src='<%=ResolveUrl("~/Libs/Images/Icon/tbitems.png")%>' alt="" /><div><%=GetLabel("Quick Picks")%></div></li>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="plhScript" runat="server">
    <script type="text/javascript">
        $(function () {
            $('#<%=grdView.ClientID %> tr:gt(0):not(.trEmpty)').live('click', function () {
                $('#<%=grdView.ClientID %> tr.selected').removeClass('selected');
                $(this).addClass('selected');
                $('#<%=hdnID.ClientID %>').val($(this).find('.keyField').html());
            });
            $('#<%=grdView.ClientID %> tr:eq(1)').click();

            setDatePicker('<%=txtPerformDate.ClientID %>');
            $('#<%=txtPerformDate.ClientID %>').datepicker('option', 'minDate', '0');
        });

        function onAfterSaveRecordPatientPageEntry(value) {
            if ($('#<%=hdnTestOrderID.ClientID %>').val() == '') {
                $('#<%=hdnTestOrderID.ClientID %>').val(value);
                cboParamedicID.SetEnabled(false);
                cboServiceUnit.SetEnabled(false);
            }
            cbpView.PerformCallback('refresh');
        }

        //#region Paging
        var pageCount = parseInt('<%=PageCount %>');
        $(function () {
            setPaging($("#paging"), pageCount, function (page) {
                cbpView.PerformCallback('changepage|' + page);
            });
        });

        function onCbpViewEndCallback(s) {
            $('#containerImgLoadingView').hide();

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

        function onSelectedRowChanged(value) {
            var idx = $('#<%=grdView.ClientID %> tr').index($('#<%=grdView.ClientID %> tr.focus'));
            idx += value;
            if (idx < 1)
                idx = 1;
            if (idx == $('#<%=grdView.ClientID %> tr').length)
                idx--;
            $('#<%=grdView.ClientID %> tr:eq(' + idx + ')').click();
        }

        function getSelectedRow() {
            return $('#<%=grdView.ClientID %> tr.selected');
        }

        function onButtonCancelClick() {
            $('#<%=grdView.ClientID %> tr.selected').removeClass('selected');
        }

        //#region Entity To Control
        function entityToControl(entity) {
            onCboToBePerformedChanged();
            cboServiceUnit.SetEnabled(false);
            ledItem.SetFocus();
            $('#<%=grdView.ClientID %> tr.selected').removeClass('selected');
            if (entity != null) {
                $('#<%=grdView.ClientID %> tr.focus').addClass('selected');
                $('#<%=hdnEntryID.ClientID %>').val(entity.ID);
                $('#<%=hdnItemID.ClientID %>').val(entity.ItemID);
                cboDiagnose.SetValue(entity.DiagnoseID);
                $('#<%=txtRemarks.ClientID %>').val(entity.Remarks);
                cboToBePerformed.SetValue(entity.GCToBePerformed);
                if (entity.GCToBePerformed == Constant.ToBePerformed.SCHEDULLED) {
                    $('#<%=txtPerformDate.ClientID %>').val(entity.PerformedDate);
                    $('#<%=txtPerformTime.ClientID %>').val(entity.PerformedTime);
                }
                else {
                    $('#<%=txtPerformDate.ClientID %>').val(entity.TestOrderDate);
                    $('#<%=txtPerformTime.ClientID %>').val(entity.TestOrderTime);
                }

                var filterExpression = '';
                if ($('#<%=hdnTestOrderID.ClientID %>').val() != '')
                    filterExpression = $('#<%=hdnFilterExpressionItemEdit.ClientID %>').val().replace('{HealthcareServiceUnitID}', cboServiceUnit.GetValue()).replace('{ItemID}', entity.ItemID);
                else
                    filterExpression = $('#<%=hdnFilterExpressionItemNewTransHd.ClientID %>').val().replace('{HealthcareServiceUnitID}', cboServiceUnit.GetValue());

                filterExpression += " AND IsDeleted = 0 AND ISNULL(GCItemStatus,'') != '" + Constant.ItemStatus.ITEM_STATUS_IN_ACTIVE + "'";
                
                ledItem.SetFilterExpression(filterExpression);
                ledItem.SetValue(entity.ItemID);
            }
            else {
                $('#<%=hdnEntryID.ClientID %>').val('');
                $('#<%=hdnItemID.ClientID %>').val('');
                cboDiagnose.SetValue('');
                $('#<%=txtRemarks.ClientID %>').val('');

                var filterExpression = '';
                if ($('#<%=hdnTestOrderID.ClientID %>').val() != '')
                    filterExpression = $('#<%=hdnFilterExpressionItemAdd.ClientID %>').val().replace('{HealthcareServiceUnitID}', cboServiceUnit.GetValue());
                else
                    filterExpression = $('#<%=hdnFilterExpressionItemNewTransHd.ClientID %>').val().replace('{HealthcareServiceUnitID}', cboServiceUnit.GetValue());

                filterExpression += " AND IsDeleted = 0 AND ISNULL(GCItemStatus,'') != '" + Constant.ItemStatus.ITEM_STATUS_IN_ACTIVE + "'";

                ledItem.SetFilterExpression(filterExpression);
                ledItem.SetValue('');
                cbpView.PerformCallback('refresh');
            }            
        }

        function onCboToBePerformedChanged() {
            if (cboToBePerformed.GetValue() != null && cboToBePerformed.GetValue() == Constant.ToBePerformed.SCHEDULLED) {
                $('#<%=txtPerformDate.ClientID %>').removeAttr('readonly');
                $('#<%=txtPerformDate.ClientID %>').datepicker('enable');
                $('#<%=txtPerformTime.ClientID %>').removeAttr('readonly');
            }
            else {
                $('#<%=txtPerformDate.ClientID %>').attr('readonly', 'readonly');
                $('#<%=txtPerformDate.ClientID %>').datepicker('disable');
                $('#<%=txtPerformTime.ClientID %>').attr('readonly', 'readonly');
            }
        }
        //#endregion

        $(function () {
            setDatePicker('<%=txtPerformDate.ClientID %>');

            $('#<%=btnBackTestOrderList.ClientID %>').click(function () {
                if (($('#<%:hdnTestOrderID.ClientID %>').val() != "") && $('#<%:hdnIsProposed.ClientID %>').val() == "0") {
                    var message = "Send your order to Service Unit ?";
                    displayConfirmationMessageBox("SEND ORDER", message, function (result) {
                        if (result) {
                            cbpSendOrder.PerformCallback('sendOrder');
                        }
                        else {
                            showLoadingPanel();
                            document.location = document.referrer;
                        }
                    });
                }
                else {
                    showLoadingPanel();
                    document.location = document.referrer;
                }
            });

            $('#<%=btnOpenTestOrderEntryQuickPicks.ClientID %>').click(function () {
                var serviceUnitID = $('#<%=hdnHealthcareServiceUnitID.ClientID %>').val();
                var labServiceUnitID = $('#<%=hdnLaboratoryServiceUnitID.ClientID %>').val();
                var radiologyServiceUnitID = $('#<%=hdnImagingServiceUnitID.ClientID %>').val();
                var url = '';
                var width = 0;
                var testOrderID = "0";
                var clinicalNotes = '';
                var chiefComplaint = '';
                var param = "";
                var title = 'Quick Picks';

                if ($('#<%=hdnTestOrderID.ClientID %>').val() != '') {
                    testOrderID = $('#<%=hdnTestOrderID.ClientID %>').val();
                }
                if (serviceUnitID == labServiceUnitID) {
                    title = "Order Pemeriksaan Laboratorium - " + $('#<%=hdnPatientInformation.ClientID %>').val();
                    param = "X001^004" + "|" + testOrderID + "|" + clinicalNotes + "|" + chiefComplaint;
                    url = ResolveUrl('~/Program/PatientPage/_PopupEntry/CPOE/TestOrderItemQuickPicksCtl1.ascx');
                    width = 1200;
                }
                else if (serviceUnitID == radiologyServiceUnitID) {
                    title = "Order Pemeriksaan Radiologi - " + $('#<%=hdnPatientInformation.ClientID %>').val();
                    param = "X001^005" + "|" + testOrderID + "|" + clinicalNotes + "|" + chiefComplaint;
                    url = ResolveUrl('~/Program/PatientPage/_PopupEntry/CPOE/TestOrderItemQuickPicksCtl1.ascx');
                    width = 1200;
                }
                else {
                    title = "Order Pemeriksaan Penunjang - " + $('#<%=hdnPatientInformation.ClientID %>').val();
                    param = "X001^007" + "|" + serviceUnitID + "|" + testOrderID + "|" + clinicalNotes + "|" + chiefComplaint;
                    url = ResolveUrl('~/Program/PatientPage/_PopupEntry/CPOE/OtherTestOrderItemQuickPicksCtl1.ascx');
                    width = 1200;
                }

                openUserControlPopup(url, param, title, width, 600);
            });

            $('.imgEdit.imgLink').die('click');
            $('.imgEdit.imgLink').live('click', function () {
                onPatientListEntryEditRecord();
            });

            $('.imgDelete.imgLink').die('click');
            $('.imgDelete.imgLink').live('click', function () {
                onPatientListEntryDeleteRecord();
            });
        });

        $('#<%=btnSendOrder.ClientID %>').click(function () {
            if ($('#<%:hdnTestOrderID.ClientID %>').val() == "") {
                displayErrorMessageBox("SEND ORDER", "There is no order to be sent !");
            }
            else {
                var message = "Send your order to Service Unit ?";
                displayConfirmationMessageBox("SEND ORDER",message, function (result) {
                    if (result) cbpSendOrder.PerformCallback('sendOrder');
                });
            }
        });

        function onLedItemLostFocus(value) {
            $('#<%=hdnItemID.ClientID %>').val(value);
        }

        function onAfterSaveRecord(param) {
            if ($('#<%=hdnTestOrderID.ClientID %>').val() == '') {
                $('#<%=hdnFilterExpressionItemAdd.ClientID %>').val($('#<%=hdnFilterExpressionItemAdd.ClientID %>').val().replace('{TestOrderID}', param));
                $('#<%=hdnFilterExpressionItemEdit.ClientID %>').val($('#<%=hdnFilterExpressionItemEdit.ClientID %>').val().replace('{TestOrderID}', param));

                var paramInfo = param.split('|');
                $('#<%=hdnTestOrderID.ClientID %>').val(paramInfo[0]);
                cboParamedicID.SetEnabled(false);
                cboServiceUnit.SetEnabled(false);
                cboToBePerformed.SetEnabled(false);

                cbpView.PerformCallback('refresh');
            }
            else {
                var paramInfo = param.split('|');
                $('#<%=hdnTestOrderID.ClientID %>').val(paramInfo[0]);
                cboParamedicID.SetEnabled(false);
                cboServiceUnit.SetEnabled(false);
                cboToBePerformed.SetEnabled(false);

                cbpView.PerformCallback('refresh');
            }
        }

        function onCboServiceUnitValueChanged() {
            $('#<%=hdnHealthcareServiceUnitID.ClientID %>').val(cboServiceUnit.GetValue());
        }

        function onBeforeSaveRecord() {
            return ledItem.Validate();
        }

        function onCbpSendOrderEndCallback(s) {
            hideLoadingPanel();
            var param = s.cpResult.split('|');
            if (param[0] == 'sendOrder') {
                if (param[1] == 'success') {
                    showToast('Send Success', 'The test order was successfully sent to Service Unit.');
                    document.location = document.referrer;
                }
                else {
                    showToast('Save Failed', 'Error Message : ' + param[2]);
                }
            }
        }
    </script>
    <input type="hidden" id="hdnFilterExpressionItemNewTransHd" runat="server" value="" />
    <input type="hidden" id="hdnFilterExpressionItemAdd" runat="server" value="" />
    <input type="hidden" id="hdnFilterExpressionItemEdit" runat="server" value="" />
    <input type="hidden" id="hdnTestOrderID" runat="server" value="" />
    <input type="hidden" id="hdnPopupResultID" runat="server" value="0" />
    <input type="hidden" id="hdnIsProposed" runat="server" value="0" />
    <input type="hidden" id="hdnPatientInformation" runat="server" />
    <table cellpadding="2" cellspacing="0">
        <colgroup>
            <col width="150px" />
            <col width="140px" />
            <col width="80px" />
            <col width="150px" />
            <col width="140px" />
            <col width="80px" />
            <col />
        </colgroup>
        <tr>
            <td class="tdLabel"><%=GetLabel("Tanggal ") %> - <%=GetLabel("Jam Order") %></td>
            <td style="padding-right: 1px;width:120px"><asp:TextBox ID="txtTestOrderDate" ReadOnly="true" Width="120px" CssClass="datepicker" runat="server" /></td>
            <td style="width:120px"><asp:TextBox ID="txtTestOrderTime" Width="80px" CssClass="time" ReadOnly="true" runat="server" Style="text-align:center" /></td>
            <td class="tdLabel"><%=GetLabel("Rencana Jadwal") %></td>
            <td style="padding-right: 1px;width:145px"><asp:TextBox ID="txtPerformDate" Width="120px" CssClass="datepicker" runat="server" /></td>
            <td><asp:TextBox ID="txtPerformTime" Width="80px" CssClass="time" runat="server" Style="text-align:center" /></td>
            <td colspan="2">
                <table border="0" cellpadding="0" cellspacing="1" style="width:100%">
                    <tr>
                        <td><asp:CheckBox ID="chkIsUsedRequestTime" Width="160px" runat="server" Text=" Permintaan Jam Khusus" /></td>
                        <td><asp:CheckBox ID="chkIsUsingSpecificItem" Width="180px" runat="server" Text=" Penggunaan Alat Tertentu" /></td>
                        <td><asp:CheckBox ID="chkIsCITO" Width="100px" runat="server" Text=" CITO" /></td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td class="tdLabel"><%=GetLabel("Unit Pelayanan") %></td>
            <td colspan="2">
                <dxe:ASPxComboBox ID="cboServiceUnit" ClientInstanceName="cboServiceUnit" Width="100%" runat="server">
                    <ClientSideEvents ValueChanged="function(s,e){ onCboServiceUnitValueChanged(); }"
                        Init="function(s,e){ onCboServiceUnitValueChanged(); }" />
                </dxe:ASPxComboBox>
            </td>
            <td class="tdLabel"><%=GetLabel("Estimasi Lama Operasi") %></td>
            <td><asp:TextBox ID="txtEstimatedDuration" Width="80px" CssClass="number" runat="server" Style="text-align:center" /> menit</td>
        </tr>
        <tr>
            <td class="tdLabel"><%=GetLabel("Physician") %></td>
            <td colspan="2">
                <dxe:ASPxComboBox ID="cboParamedicID" ClientInstanceName="cboParamedicID" Width="100%" runat="server" />
            </td>
            <td class="tdLabel"><label class="lblLink" id="lblProcedureGroup"><%=GetLabel("Jenis Operasi")%></label></td>
            <td><asp:TextBox ID="txtProcedureGroupCode" Width="100%" runat="server" /></td>
            <td colspan="2"><asp:TextBox ID="txtProcedureGroupName" Width="100%" runat="server" ReadOnly="true"/></td>
        </tr>
    </table>    
</asp:Content>

<asp:Content ID="ctnEntry" ContentPlaceHolderID="plhEntry" runat="server">
    <input type="hidden" value="" id="hdnEntryID" runat="server" />
    <input type="hidden" value="" id="hdnVisitID" runat="server" />
    <input type="hidden" id="hdnHealthcareServiceUnitID" value="" runat="server" />
    <input type="hidden" value="" id="hdnImagingServiceUnitID" runat="server" />
    <input type="hidden" value="" id="hdnLaboratoryServiceUnitID" runat="server" />
    <input type="hidden" value="" id="hdnItemID" runat="server" />
    <input type="hidden" value="" id="hdnID" runat="server" />
    <input type="hidden" id="hdnFilterExpression" runat="server" value="" />
    <table style="width:100%" class="tblEntryDetail">
        <colgroup>
            <col style="width:50%"/>
        </colgroup>
        <tr>
            <td valign="top">
                <table style="width:100%">
                    <colgroup>
                        <col style="width:180px"/>
                    </colgroup>
                    <tr>
                        <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Pemeriksaan/Pelayanan")%></label></td>
                        <td colspan="2">
                            <qis:QISSearchTextBox ID="ledItem" ClientInstanceName="ledItem" runat="server" Width="500px"
                                ValueText="ItemID" FilterExpression="IsDeleted = 0" DisplayText="ItemName1" MethodName="GetvServiceUnitItemList" >
                                <ClientSideEvents ValueChanged="function(s){ onLedItemLostFocus(s.GetValueText()); }" />
                                <Columns>
                                    <qis:QISSearchTextBoxColumn Caption="Item Name" FieldName="ItemName1" Description="i.e. Cholera" Width="300px" />
                                    <qis:QISSearchTextBoxColumn Caption="Item Code" FieldName="ItemCode" Description="i.e. A09" Width="100px" />
                                </Columns>
                            </qis:QISSearchTextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Diagnosa")%></label></td>
                        <td><dxe:ASPxComboBox runat="server" ID="cboDiagnose" ClientInstanceName="cboDiagnose" Width="500px" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel" style="vertical-align: top; padding-top: 5px;"><label class="lblMandatory"><%=GetLabel("Catatan Klinis")%></label></td>
                        <td><asp:TextBox ID="txtRemarks" Width="500px" runat="server" TextMode="MultiLine" /></td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>

<asp:Content ID="ctnList" ContentPlaceHolderID="plhList" runat="server">
    <div style="position: relative;">
        <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
            ShowLoadingPanel="false" OnCallback="cbpView_Callback">
            <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingView').show(); }"
                EndCallback="function(s,e){ onCbpViewEndCallback(s); }" />
            <PanelCollection>
                <dx:PanelContent ID="PanelContent1" runat="server">
                    <asp:Panel runat="server" ID="pnlView" CssClass="pnlContainerGrid" Style="height:300px">
                        <asp:GridView ID="grdView" runat="server" CssClass="grdSelected grdPatientPage" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                            <Columns>
                                <asp:BoundField DataField="ID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="20px">
                                    <ItemTemplate>
                                        <table cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td>
                                                    <img class="imgEdit imgLink"
                                                        title='<%=GetLabel("Edit")%>' src='<%# ResolveUrl("~/Libs/Images/Button/edit.png")%>'
                                                        alt="" />
                                                </td>
                                                <td style="width: 1px">
                                                    &nbsp;
                                                </td>
                                                <td>
                                                    <img class="imgDelete imgLink"
                                                        title='<%=GetLabel("Delete")%>' src='<%# ResolveUrl("~/Libs/Images/Button/delete.png")%>'
                                                        alt="" />
                                                </td>
                                            </tr>
                                        </table>
                                    </ItemTemplate>
                                </asp:TemplateField>  
                                <asp:TemplateField HeaderStyle-CssClass="hiddenColumn" ItemStyle-CssClass="hiddenColumn" >
                                    <ItemTemplate>
                                        <input type="hidden" value="<%#:Eval("ID") %>" bindingfield="ID" />
                                        <input type="hidden" value="<%#:Eval("ItemID") %>" bindingfield="ItemID" />
                                        <input type="hidden" value="<%#:Eval("DiagnoseID") %>" bindingfield="DiagnoseID" />
                                        <input type="hidden" value="<%#:Eval("GCToBePerformed") %>" bindingfield="GCToBePerformed" />
                                        <input type="hidden" value="<%#:Eval("PerformedDateInDatePickerFormat") %>" bindingfield="PerformedDate" />
                                        <input type="hidden" value="<%#:Eval("Remarks") %>" bindingfield="Remarks" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="ItemName1" HeaderText="Item" HeaderStyle-HorizontalAlign="Left" />
                                <asp:BoundField DataField="DiagnoseName" HeaderText="Diagnosis" HeaderStyle-Width="300px" HeaderStyle-HorizontalAlign="Left" />
                                <asp:BoundField DataField="Remarks" HeaderText="Remarks" HeaderStyle-HorizontalAlign="Left" />
                            </Columns>
                            <EmptyDataTemplate>
                                <%=GetLabel("No Data To Display")%>
                            </EmptyDataTemplate>
                        </asp:GridView>
                    </asp:Panel>
                </dx:PanelContent>
            </PanelCollection>
        </dxcp:ASPxCallbackPanel>    
        <dxcp:ASPxCallbackPanel ID="cbpSendOrder" runat="server" Width="100%" ClientInstanceName="cbpSendOrder"
            ShowLoadingPanel="false" OnCallback="cbpSendOrder_Callback">
            <ClientSideEvents BeginCallback="function(s,e) { showLoadingPanel(); }" EndCallback="function(s,e) { onCbpSendOrderEndCallback(s); }" />
        </dxcp:ASPxCallbackPanel>
        <div class="imgLoadingGrdView" id="containerImgLoadingView" >
            <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
        </div>
        <div class="containerPaging">
            <div class="wrapperPaging">
                <div id="paging"></div>
            </div>
        </div> 
    </div>
</asp:Content>
