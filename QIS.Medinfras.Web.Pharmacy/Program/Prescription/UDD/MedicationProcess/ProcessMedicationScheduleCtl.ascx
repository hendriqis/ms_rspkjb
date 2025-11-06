<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ProcessMedicationScheduleCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.ProcessMedicationScheduleCtl" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
    <%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<div class="toolbarArea">
    <ul>
        <li id="btnMPEntryProcess">
            <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbProcess.png")%>' alt="" /><div>
                <%=GetLabel("Process")%></div>
        </li>
    </ul>
</div>
<script type="text/javascript" id="dxss_patiententryctl">
    $(function () {
        $('.tblResultInfo').hide();
    });

    $('#chkSelectAll').die('change');
    $('#chkSelectAll').live('change', function () {
        var isChecked = $(this).is(":checked");
        $('.chkIsProcessItem').each(function () {
            $(this).find('input').prop('checked', isChecked);
        });

        if (isChecked) {
            $('.txtMedicationTime').each(function () {
                $(this).val($('#<%=txtDefaultTime.ClientID %>').val());
            });
        }
    });

    function getSelectedCheckbox() {
        var tempSelectedID = "";
        var tempSelectedScheduleID = "";
        var tempSelectedTime = "";
        $('.grdPrescriptionOrderDt .chkIsProcessItem input:checked').each(function () {
            var $tr = $(this).closest('tr');
            var id = $(this).closest('tr').find('.keyField').html();
            var sch = $(this).closest('tr').find('.hiddenColumn').html();
            var time = $tr.find('.txtMedicationTime').val();

            if (tempSelectedID != "") {
                tempSelectedID += ",";
                tempSelectedScheduleID += ",";
                tempSelectedTime += ",";
            }
            tempSelectedID += id;
            tempSelectedScheduleID += sch;
            tempSelectedTime += time;
        });
        if (tempSelectedID != "") {
            $('#<%=hdnSelectedID.ClientID %>').val(tempSelectedID);
            $('#<%=hdnSelectedScheduleID.ClientID %>').val(tempSelectedScheduleID);
            $('#<%=hdnSelectedTime.ClientID %>').val(tempSelectedTime);
            return true;
        }
        else return false;
    }

    setDatePicker('<%=txtMedicationDate.ClientID %>');
    $('#<%=txtMedicationDate.ClientID %>').datepicker('option', 'maxDate', '0');

    $('#<%=txtMedicationDate.ClientID %>').change(function (evt) {
        onRefreshGrid();
    });

    $('#btnMPEntryProcess').click(function () {
        if (!getSelectedCheckbox()) {
            showToast("ERROR", 'Error Message : ' + "Please select the item to be process !");
        }
        else {
            var message = "Process the Medication Order for date <b>" + $('#<%:txtMedicationDate.ClientID %>').val() + "</b> and  <b>Sequence Number " + cboSequence.GetValue() + "</b> ?";
            showToastConfirmation(message, function (result) {
                if (result) cbpPopupProcess.PerformCallback('process');
            });
        }
    });

    function onCbpPopupProcesEndCallback(s) {
        hideLoadingPanel();
        var param = s.cpResult.split('|');
        var retval = s.cpRetval.split('|');
        if (param[0] == 'process') {
            if (param[1] == '0') {
                $('.tblResultInfo').hide();
                showToast("Process Failed", "Error Message : <br/><span style='color:red'>" + param[2] + "</span>");
            }
            else {
                $('.tblResultInfo').show();
                $('#<%=txtReferenceNo.ClientID %>').val(param[3]);
                $('#<%=txtTransactionNo.ClientID %>').val(param[3]);
                showToast('Medication Schedule', param[2]);
                pcRightPanelContent.Hide();
            }
        }
    }

    function onCbpViewEndCallback(s) {
        hideLoadingPanel();
    }

    function onRefreshGrid() {
        cbpView.PerformCallback('refresh');
    }
</script>
<input type="hidden" runat="server" id="hdnSelectedID" value="" />
<input type="hidden" runat="server" id="hdnSelectedScheduleID" value="" />
<input type="hidden" runat="server" id="hdnSelectedTime" value="" />
<input type="hidden" runat="server" id="hdnDispensaryServiceUnitID" value="" />
<input type="hidden" value="0" id="hdnPrescriptionFeeAmount" runat="server" />
<input type="hidden" runat="server" id="hdnLocationID" value="" />
<div>
    <div>
        <table>
            <colgroup>
                <col width="115px" />
                <col width="185px" />
                <col />
            </colgroup>
            <tr>
                <td class="tdLabel">
                    <label class="lblNormal">
                        <%=GetLabel("Tanggal")%></label>
                </td>
                <td>
                    <asp:TextBox ID="txtMedicationDate" runat="server" Width="120px" CssClass="datepicker" />
                </td>
                <td style="vertical-align:top; padding-left:20px;padding-bottom:8px" rowspan="5">
                    <table class="tblResultInfo" width="100%" style="display:none">
                        <colgroup>
                            <col width="115px" />
                            <col width="200px" />
                            <col />
                        </colgroup>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Nomor Referensi")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtReferenceNo" runat="server" Width="100%" ReadOnly="true"/>
                            </td>                                
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Nomor Transaksi")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtTransactionNo" runat="server" Width="100%" ReadOnly="true"/>
                            </td>  
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td class="tdLabel">
                    <label class="lblNormal">
                        <%=GetLabel("Location")%></label>
                </td>
                <td>
                    <dxe:ASPxComboBox ID="cboLocation" ClientInstanceName="cboLocation"
                    runat="server" Width="100%" >
                    </dxe:ASPxComboBox>
                </td>
            </tr>
            <tr>
                <td class="tdLabel">
                    <label class="lblNormal">
                        <%=GetLabel("Sequence No.")%></label>
                </td>
                <td>
                    <dxe:ASPxComboBox ID="cboSequence" ClientInstanceName="cboSequence"
                    runat="server" Width="100%" >
                        <ClientSideEvents ValueChanged="function(s,e) { onRefreshGrid(); }" />
                    </dxe:ASPxComboBox>
                </td>
            </tr>
            <tr>
                <td class="tdLabel">
                    <label class="lblNormal">
                        <%=GetLabel("Default Time")%></label>
                </td>
                <td>
                    <asp:TextBox ID="txtDefaultTime" runat="server" Width="60px" Text="00:00" CssClass="time"/>
                </td>
            </tr>
        </table>
    </div>
    <div style="height:400px; overflow:scroll;overflow-x: hidden;">
        <table class="tblContentArea" width="100%">
            <tr>
                <td>
                </td>
            </tr>
            <tr>
                <td>
                     <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
                        ShowLoadingPanel="false" OnCallback="cbpView_Callback">
                        <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ onCbpViewEndCallback(s); }" />
                        <PanelCollection>
                            <dx:PanelContent ID="PanelContent1" runat="server">
                                <asp:Panel runat="server" ID="pnlView" Style="width: 100%; margin-left: auto; margin-right: auto;
                                    position: relative; font-size: 0.95em;">
                                    <asp:ListView runat="server" ID="lvwView" OnItemDataBound="lvwView_ItemDataBound">
                                        <EmptyDataTemplate>
                                            <table id="tblView" runat="server" class="grdPurchaseRequest grdSelected" cellspacing="0" rules="all">
                                                <tr>
                                                    <th class="keyField" rowspan="2">&nbsp;</th>
                                                    <th  align="left"><%=GetLabel("Drug Name")%></th>
                                                </tr>
                                                <tr class="trEmpty">
                                                    <td colspan="2">
                                                        <%=GetLabel("Tidak ada data penjadwalan obat")%>
                                                    </td>
                                                </tr>
                                            </table>
                                        </EmptyDataTemplate>
                                        <LayoutTemplate>
                                            <table id="tblView" runat="server" class="grdPrescriptionOrderDt grdSelected" cellspacing="0" rules="all">
                                                <tr>
                                                    <th class="keyField" rowspan="2">&nbsp;</th>
                                                    <th class="hiddenColumn" >&nbsp;</th>
                                                    <th  align="center" style="width:30px">
                                                        <input id="chkSelectAll" type="checkbox" />
                                                    </th>
                                                    <th  align="left"><%=GetLabel("Drug Name")%></th>
                                                    <th  align="left"><%=GetLabel("Dosing Quantity")%></th>
                                                    <th  align="left"><%=GetLabel("Route")%></th>
                                                    <th  align="center" style="width:70px"><%=GetLabel("Time")%></th>
                                                </tr>
                                                <tr runat="server" id="itemPlaceholder">
                                                </tr>
                                            </table>
                                        </LayoutTemplate>
                                        <ItemTemplate>
                                            <tr>
                                                <td class="keyField"><%#: Eval("PrescriptionOrderDetailID")%></td>
                                                <td class="hiddenColumn"><%#: Eval("ID")%></td>
                                                <td align="center"><asp:CheckBox ID="chkIsProcessItem" runat="server" CssClass="chkIsProcessItem" /></td>
                                                <td class="tdItemName"><label class="lblItemName"><%#: Eval("DrugName")%></label></td>
                                                <td class="tdDosingQuantity"><label class="lblItemName"><%#: Eval("cfDosingQuantity")%></label></td>
                                                <td class="tdRoute" style="width:100px"><label class="lblRoute"><%#: Eval("Route")%></label></td>
                                                <td><input type="text" class="txtMedicationTime" value="<%#:Eval("MedicationTime") %>" /></td>
                                            </tr>
                                        </ItemTemplate>
                                    </asp:ListView>
                                </asp:Panel>
                            </dx:PanelContent>
                        </PanelCollection>
                    </dxcp:ASPxCallbackPanel>
                </td>
            </tr>
        </table>
        <dxcp:ASPxCallbackPanel ID="cbpPopupProcess" runat="server" Width="100%" ClientInstanceName="cbpPopupProcess"
            ShowLoadingPanel="false" OnCallback="cbpPopupProcess_Callback">
            <ClientSideEvents BeginCallback="function(s,e) { showLoadingPanel(); }" EndCallback="function(s,e) { onCbpPopupProcesEndCallback(s); }" />
        </dxcp:ASPxCallbackPanel>
    </div>
</div>
