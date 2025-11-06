<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DoctorFeeEntryCtl1.ascx.cs"
    Inherits="QIS.Medinfras.Web.EMR.Program.DoctorFeeEntryCtl1" %>
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
<style type="text/css">       
    .highlight    {  background-color:#FE5D15; color: White; }
    
    .druglist { font-weight: bold;}
</style>
<script type="text/javascript" id="dxss_patiententryctl">
    $(function () {
    });

    $('.chkIsProcessItem input').live('change', function () {
        var $cell = $(this).closest("td");
        var $tr = $cell.closest('tr');
        var isChecked = $(this).is(":checked");
        if ($(this).is(':checked')) {
            $cell.addClass('highlight');
        }
        else {
            $cell.removeClass('highlight');
        }
    });

    function getSelectedCheckbox() {
        var tempSelectedID = "";
        var tempSelectedRemarks = "";
        $('.grdPrescriptionOrderDt .chkIsProcessItem input:checked').each(function () {
            var $tr = $(this).closest('tr');
            var id = $(this).closest('tr').find('.keyField').html();
            var remarks = $(this).closest('tr').find('.txtConfirmationRemarks').val();

            if (tempSelectedID != "") {
                tempSelectedID += ",";
                tempSelectedRemarks += ",";
            }
            tempSelectedID += id;
            tempSelectedRemarks += remarks;
        });
        if (tempSelectedID != "") {
            $('#<%=hdnSelectedID.ClientID %>').val(tempSelectedID);
            $('#<%=hdnSelectedRemarks.ClientID %>').val(tempSelectedRemarks);
            return true;
        }
        else return false;
    }

    $('#btnMPEntryProcess').click(function () {
        if (!getSelectedCheckbox()) {
            showToast("PROCESS : FAIL", "Error Message : <br/><span style='color:red'>" + "Please select the item to be proceed !"  + "</span>");
        }
        else {
            var message = "Process the doctor fee entry ?";
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
                showToast("PROCESS : FAIL", "Error Message : <br/><span style='color:red'>" + param[2] + "</span>");
            }
            else {
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
<input type="hidden" runat="server" id="hdnSelectedRemarks" value="" />
<input type="hidden" runat="server" id="hdnDispensaryServiceUnitID" value="" />
<input type="hidden" runat="server" id="hdnReferenceNo" value="" />
<input type="hidden" runat="server" id="hdnTransactionNo" value="" />
<input type="hidden" runat="server" id="hdnTransactionID" value="" />
<input type="hidden" runat="server" id="hdnTransactionDate" value="" />
<div>
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
                                                    <th  align="left"><%=GetLabel("Item Name")%></th>
                                                </tr>
                                                <tr class="trEmpty">
                                                    <td colspan="4">
                                                        <%=GetLabel("There is no physician item to be selected")%>
                                                    </td>
                                                </tr>
                                            </table>
                                        </EmptyDataTemplate>
                                        <LayoutTemplate>
                                            <table id="tblView" runat="server" class="grdPrescriptionOrderDt grdSelected" cellspacing="0" rules="all">
                                                <tr>
                                                    <th  align="center" style="width:30px" rowspan="2">
                                                    </th>
                                                    <th  align="left" rowspan="2"><%=GetLabel("Item Name")%></th>
                                                    <th colspan="4" align="center" style="width:200px">
                                                        <div>
                                                            <%=GetLabel("Amount")%></div>
                                                    </th>
                                                </tr>
                                                <tr>
                                                    <th style="width: 40px;">
                                                        <div style="text-align:center">
                                                            <%=GetLabel("Label") %></div>
                                                    </th>
                                                    <th style="width: 40px">
                                                        <div style="text-align:center">
                                                            <%=GetLabel("FOC") %></div>
                                                    </th>
                                                    <th style="width: 40px">
                                                        <div style="text-align:right">
                                                            <%=GetLabel("Disc %") %></div>
                                                    </th>
                                                    <th style="width: 40px;">
                                                        <div style="text-align:right">
                                                            <%=GetLabel("Tariff") %></div>
                                                    </th>
                                                </tr>
                                                <tr runat="server" id="itemPlaceholder">
                                                </tr>
                                            </table>
                                        </LayoutTemplate>
                                        <ItemTemplate>
                                            <tr>
                                                <td class="keyField"><%#: Eval("ItemID")%></td>
                                                <td class="hiddenColumn"><%#: Eval("ItemID")%></td>
                                                <td align="center"><asp:CheckBox ID="chkIsProcessItem" runat="server" CssClass="chkIsProcessItem" /></td>
                                                <td class="tdItemName"><label class="lblItemName"><%#: Eval("ItemName1")%></label></td>
                                                <td class="tdItemName"><label class="lblItemName" style="text-align:center"><%#: Eval("RegistrationReceiptLabel")%></label></td>
                                                <td align="center"><asp:CheckBox ID="chkIsFOC" runat="server" CssClass="chkIsFOC" /></td>
                                                <td style="width:40px"><asp:TextBox runat="server" ID="txtDiscount" CssClass="number" Text="0.00" Width="100%" /></td>
                                                <td style="width:120px"><asp:TextBox runat="server" ID="txtUnitPrice" CssClass="number" Text="0.00" Width="100%" /></td>
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
