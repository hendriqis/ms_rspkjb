<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PrescriptionAlertConfirmCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.EMR.Program.PrescriptionAlertConfirmCtl" %>
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

    $('#chkSelectAll').die('change');
    $('#chkSelectAll').live('change', function () {
        var isChecked = $(this).is(":checked");
        $('.chkIsProcessItem').each(function () {
            $(this).find('input').prop('checked', isChecked);
        });
        $('.txtConfirmationRemarks').each(function () {
            $(this).val($('#<%=txtAlertRemarks.ClientID %>').val());
        });
    });

    $('.chkIsProcessItem input').live('change', function () {
        var $cell = $(this).closest("td");
        var $tr = $cell.closest('tr');
        var isChecked = $(this).is(":checked");
        if ($(this).is(':checked')) {
            $cell.addClass('highlight');
            $tr.find('.txtConfirmationRemarks').val($('#<%=txtAlertRemarks.ClientID %>').val());
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
            var message = "Process the confirmation for medication alert ?";
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
<input type="hidden" runat="server" id="hdnPrescriptionOrderID" value="" />
<input type="hidden" runat="server" id="hdnTransactionDate" value="" />
<div>
    <div>
        <table style="width:100%">
            <colgroup>
                <col width="150px" />
                <col />
            </colgroup>
            <tr>
                <td class="tdLabel" style="vertical-align:top">
                    <label class="lblNormal">
                        <%=GetLabel("Default Remarks")%></label>
                </td>
                <td>
                    <asp:TextBox ID="txtAlertRemarks" Width="100%" runat="server" TextMode="MultiLine" />
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
                                                        <%=GetLabel("There is no medication item should be confirmed")%>
                                                    </td>
                                                </tr>
                                            </table>
                                        </EmptyDataTemplate>
                                        <LayoutTemplate>
                                            <table id="tblView" runat="server" class="grdPrescriptionOrderDt grdSelected" cellspacing="0" rules="all">
                                                <tr>
                                                    <th  align="center" style="width:30px" rowspan="2">
                                                        <input id="chkSelectAll" type="checkbox" />
                                                    </th>
                                                    <th  align="left" rowspan="2"><%=GetLabel("Drug Name")%></th>
                                                    <th colspan="3" align="center" style="width:200px">
                                                        <div>
                                                            <%=GetLabel("Alert Type")%></div>
                                                    </th>
                                                    <th align="left" style="padding: 3px; width: 300px;" rowspan="2">
                                                        <div>
                                                            <%=GetLabel("Confirmation Remarks")%></div>
                                                    </th>
                                                </tr>
                                                <tr>
                                                    <th style="width: 40px;">
                                                        <div style="text-align:center">
                                                            <%=GetLabel("Allergy") %></div>
                                                    </th>
                                                    <th style="width: 40px">
                                                        <div style="text-align:center">
                                                            <%=GetLabel("Adverse Reaction") %></div>
                                                    </th>
                                                    <th style="width: 40px;">
                                                        <div style="text-align:center">
                                                            <%=GetLabel("Duplicate") %></div>
                                                    </th>
                                                </tr>
                                                <tr runat="server" id="itemPlaceholder">
                                                </tr>
                                            </table>
                                        </LayoutTemplate>
                                        <ItemTemplate>
                                            <tr>
                                                <td class="keyField"><%#: Eval("PrescriptionOrderDetailID")%></td>
                                                <td class="hiddenColumn"><%#: Eval("ItemID")%></td>
                                                <td align="center"><asp:CheckBox ID="chkIsProcessItem" runat="server" CssClass="chkIsProcessItem" /></td>
                                                <td class="tdItemName"><label class="lblItemName"><%#: Eval("DrugName")%></label></td>
                                                <td style="text-align: center;width:40px">                                                   
                                                    <asp:CheckBox ID="chkIsAllergyAlert" runat="server" Checked='<%# Eval("IsAllergyAlert")%>' Enabled="false"/>
                                                </td>
                                                <td style="text-align: center;width:40px">                                                   
                                                    <asp:CheckBox ID="chkIsAdverseReactionAlert" runat="server" Checked='<%# Eval("IsAdverseReactionAlert")%>' Enabled="false"/>
                                                </td>
                                                <td style="text-align: center;width:40px">                                                   
                                                    <asp:CheckBox ID="IsDuplicateTheraphyAlert" runat="server" Checked='<%# Eval("IsDuplicateTheraphyAlert")%>' Enabled="false"/>
                                                </td>
                                                <td style="width:200px"><input type="text" class="txtConfirmationRemarks" value="" style="width:100%" rows="3" /></td>
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
