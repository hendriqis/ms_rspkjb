<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ParamedicTeamCtl.ascx.cs" 
Inherits="QIS.Medinfras.Web.CommonLibs.Program.ParamedicTeamCtl" %>


<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
  <%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>

    
<script type="text/javascript" id="dxss_serviceunithealthcareentryctl">

    $('#lblEntryPopupAddData').live('click', function () {
        $('#<%=txtParamedicCode.ClientID %>').removeAttr('readonly', 'readonly');
        $('#lblPopupParamedic').attr('class', 'lblLink lblMandatory');

        $('#<%=hdnIsAdd.ClientID %>').val('1');
        $('#<%=txtParamedicCode.ClientID %>').val('');
        $('#<%=txtParamedicName.ClientID %>').val('');
        $('#<%=hdnParamedicID.ClientID %>').val('');

        if ($('#<%=hdnIsPackageItem.ClientID %>').val() == '1') {
            $('#<%=txtItemCode.ClientID %>').removeAttr('readonly', 'readonly');
            $('#lblPopupItem').attr('class', 'lblLink lblMandatory');

            $('#<%=txtItemCode.ClientID %>').val('');
            $('#<%=txtItemName.ClientID %>').val('');
            $('#<%=hdnItemID.ClientID %>').val('');
        }
        cboParamedicRole.SetValue('');
        $('#containerPopupEntryData').show();
    });

    //#region SaveandCancel
    $('#btnEntryPopupSave').click(function (evt) {
        if (IsValid(evt, 'fsEntryPopup', 'mpEntryPopup'))
            
            cbpEntryPopupView.PerformCallback('save');
        return false;
    });
    $('#btnEntryPopupCancel').live('click', function () {
        $('#containerPopupEntryData').hide();
    });
    //#endregion

    //#region Edit Delete
    $('.imgEdit.imgLink').die('click');
    $('.imgEdit.imgLink').live('click', function () {
        $('#<%=txtParamedicCode.ClientID %>').attr('readonly', 'readonly');
        $('#lblPopupParamedic').attr('class', 'lblDisabled');

        $row = $(this).closest('tr').parent().closest('tr');
        var entity = rowToObject($row);

        $('#<%=hdnIsAdd.ClientID %>').val('0');
        $('#<%=hdnID.ClientID %>').val(entity.ID);
        $('#<%=hdnParamedicID.ClientID %>').val(entity.ParamedicID);
        $('#<%=txtParamedicCode.ClientID %>').val(entity.ParamedicCode);
        $('#<%=txtParamedicName.ClientID %>').val(entity.ParamedicName);
        $('#<%=hdnRevenueSharingID.ClientID %>').val(entity.RevenueSharingID);
        cboParamedicRole.SetValue(entity.GCParamedicRole);

        if ($('#<%=hdnIsPackageItem.ClientID %>').val() == '1') {
            $('#<%=txtItemCode.ClientID %>').attr('readonly', 'readonly');
            $('#lblPopupItem').attr('class', 'lblDisabled');

            $('#<%=txtItemCode.ClientID %>').val(entity.ItemCode);
            $('#<%=txtItemName.ClientID %>').val(entity.ItemName1);
            $('#<%=hdnItemID.ClientID %>').val(entity.ItemID);
        }
        $('#containerPopupEntryData').show();
    });

    $('.imgDelete.imgLink').die('click');
    $('.imgDelete.imgLink').live('click', function () {
        $row = $(this).closest('tr').parent().closest('tr');
        showToastConfirmation('Are You Sure Want To Delete?', function (result) {
            if (result) {
                var entity = rowToObject($row);
                $('#<%=hdnParamedicID.ClientID %>').val(entity.ParamedicID);
                $('#<%=hdnItemID.ClientID %>').val(entity.ItemID);
                cbpEntryPopupView.PerformCallback('delete');
            }
        });
    });
    //#endregion

    //#region Item
    function onGetItemPopupFilterExpression() {
        var filterExpression = "<%:OnGetItemFilterExpression() %>";
        return filterExpression;
    }

    $('#lblPopupItem.lblLink').click(function () {
        openSearchDialog('item', onGetItemPopupFilterExpression(), function (value) {
            $('#<%=txtItemCode.ClientID %>').val(value);
            onTxtPopupItemCodeChanged(value);
        });
    });

    $('#<%=txtItemCode.ClientID %>').change(function () {
        onTxtPopupItemCodeChanged($(this).val());
    });

    function onTxtPopupItemCodeChanged(value) {
        var filterExpression = onGetItemPopupFilterExpression() + " AND ItemCode = '" + value + "'";
        Methods.getObject('GetItemMasterList', filterExpression, function (result) {
            if (result != null) {
                $('#<%=hdnItemID.ClientID %>').val(result.ItemID);
                $('#<%=txtItemName.ClientID %>').val(result.ItemName1);

                var today = new Date();
                var date = today.getDate() + '-' + (today.getMonth() + 1) + '-' + today.getFullYear();
                var time = today.getHours() + ":" + today.getMinutes();

                var hdnTransactionDate = $('#<%=hdnTransactionDate.ClientID %>').val() == "" ? date : $('#<%=hdnTransactionDate.ClientID %>').val();
                var hdnTransactionTime = $('#<%=hdnTransactionTime.ClientID %>').val() == "" ? time : $('#<%=hdnTransactionTime.ClientID %>').val();
                var hdnChargesHealthcareServiceUnitID = $('#<%=hdnChargesHealthcareServiceUnitID.ClientID %>').val();

                var paramedicID = $('#<%=hdnParamedicID.ClientID %>').val();
                var paramedicRole = cboParamedicRole.GetValue();
                if (paramedicID != '' && paramedicRole != null && paramedicRole != '') {
                    Methods.getItemRevenueSharing($('#<%=txtItemCode.ClientID %>').val(), $('#<%=hdnParamedicID.ClientID %>').val(), $('#<%=hdnClassID.ClientID %>').val(), paramedicRole, $('#<%=hdnVisitIDCtlPT.ClientID %>').val(), hdnChargesHealthcareServiceUnitID, hdnTransactionDate, hdnTransactionTime, function (result) {
                        if (result != null)
                            $('#<%=hdnRevenueSharingID.ClientID %>').val(result.RevenueSharingID);
                        else
                            $('#<%=hdnRevenueSharingID.ClientID %>').val('');
                    });
                }
            }
            else {
                $('#<%=hdnItemID.ClientID %>').val('');
                $('#<%=txtItemCode.ClientID %>').val('');
                $('#<%=txtItemName.ClientID %>').val('');
                $('#<%=hdnRevenueSharingID.ClientID %>').val('');
            }
        });
    }
    //#endregion

    //#region Paramedic
    $('#lblPopupParamedic.lblLink').click(function () {
        openSearchDialog('paramedic', onGetPhysicianFilterExpression(), function (value) {
            $('#<%=txtParamedicCode.ClientID %>').val(value);
            onTxtPopupParamedicCodeChanged(value);
        });
    });

    $('#<%=txtParamedicCode.ClientID %>').change(function () {
        onTxtPopupParamedicCodeChanged($(this).val());
    });

    function onTxtPopupParamedicCodeChanged(value) {
        var filterExpression = onGetPhysicianFilterExpression() + " AND ParamedicCode = '" + value + "'";
        Methods.getObject('GetvParamedicMasterList', filterExpression, function (result) {
            if (result != null) {
                $('#<%=hdnParamedicID.ClientID %>').val(result.ParamedicID);
                $('#<%=txtParamedicName.ClientID %>').val(result.ParamedicName);

                var today = new Date();
                var date = today.getDate() + '-' + (today.getMonth() + 1) + '-' + today.getFullYear();
                var time = today.getHours() + ":" + today.getMinutes();

                var hdnTransactionDate = $('#<%=hdnTransactionDate.ClientID %>').val() == "" ? date : $('#<%=hdnTransactionDate.ClientID %>').val();
                var hdnTransactionTime = $('#<%=hdnTransactionTime.ClientID %>').val() == "" ? time : $('#<%=hdnTransactionTime.ClientID %>').val();
                var hdnChargesHealthcareServiceUnitID = $('#<%=hdnChargesHealthcareServiceUnitID.ClientID %>').val();

                var itemCode = $('#<%=txtHeaderCode.ClientID %>').val();
                if ($('#<%=txtItemCode.ClientID %>').val() != "") {
                    itemCode = $('#<%=txtItemCode.ClientID %>').val();
                }
                var paramedicRole = cboParamedicRole.GetValue();
                if (itemCode != '' && paramedicRole != null && paramedicRole != '') {
                    Methods.getItemRevenueSharing(itemCode, $('#<%=hdnParamedicID.ClientID %>').val(), $('#<%=hdnClassID.ClientID %>').val(), paramedicRole, $('#<%=hdnVisitIDCtlPT.ClientID %>').val(), hdnChargesHealthcareServiceUnitID, hdnTransactionDate, hdnTransactionTime, function (result) {
                        if (result != null)
                            $('#<%=hdnRevenueSharingID.ClientID %>').val(result.RevenueSharingID);
                        else
                            $('#<%=hdnRevenueSharingID.ClientID %>').val('');
                    });
                }
            }
            else {
                $('#<%=hdnParamedicID.ClientID %>').val('');
                $('#<%=txtParamedicCode.ClientID %>').val('');
                $('#<%=txtParamedicName.ClientID %>').val('');
                $('#<%=hdnRevenueSharingID.ClientID %>').val('');
            }
        });
    }
    //#endregion

    function onCboParamedicRoleChanged() {
        var today = new Date();
        var dayDate = today.getDate() < 10 ? "0" + today.getDate() : today.getDate();
        var monthDate = (today.getMonth() + 1) < 10 ? "0" + (today.getMonth() + 1) : (today.getMonth() + 1);

        var date = dayDate + '-' + monthDate + '-' + today.getFullYear();
        var time = today.getHours() + ":" + today.getMinutes();

        var hdnTransactionDate = $('#<%=hdnTransactionDate.ClientID %>').val() == "" ? date : $('#<%=hdnTransactionDate.ClientID %>').val();
        var hdnTransactionTime = $('#<%=hdnTransactionTime.ClientID %>').val() == "" ? time : $('#<%=hdnTransactionTime.ClientID %>').val();
        var hdnChargesHealthcareServiceUnitID = $('#<%=hdnChargesHealthcareServiceUnitID.ClientID %>').val();

        var paramedicID = $('#<%=hdnParamedicID.ClientID %>').val();
        var itemCode = $('#<%=txtHeaderCode.ClientID %>').val();
        if ($('#<%=txtItemCode.ClientID %>').val() != "") {
            itemCode = $('#<%=txtItemCode.ClientID %>').val();
        }
        var paramedicRole = cboParamedicRole.GetValue();
        if (itemCode != '' && paramedicID != '') {
            Methods.getItemRevenueSharing(itemCode, paramedicID, $('#<%=hdnClassID.ClientID %>').val(), paramedicRole, $('#<%=hdnVisitIDCtlPT.ClientID %>').val(), hdnChargesHealthcareServiceUnitID, hdnTransactionDate, hdnTransactionTime, function (result) {
                if (result != null)
                    $('#<%=hdnRevenueSharingID.ClientID %>').val(result.RevenueSharingID);
                else
                    $('#<%=hdnRevenueSharingID.ClientID %>').val('');
            });
        }
    }

    function onCbpEntryPopupViewEndCallback(s) {
        var param = s.cpResult.split('|');
        if (param[0] == 'save') {
            if (param[1] == 'fail')
                showToast('Save Failed', 'Error Message : ' + param[2]);
            else
                $('#containerPopupEntryData').hide();
        }
        else if (param[0] == 'delete') {
            if (param[1] == 'fail')
                showToast('Delete Failed', 'Error Message : ' + param[2]);
        }
        hideLoadingPanel();
    }


    
</script>

<div style="height:440px; overflow-y:auto">
    <input type="hidden" id="hdnChargesHealthcareServiceUnitID" value="" runat="server" />
    <input type="hidden" id="hdnTransactionDate" value="" runat="server" />
    <input type="hidden" id="hdnTransactionTime" value="" runat="server" />
    <input type="hidden" id="hdnID" value="" runat="server" />
    <input type="hidden" id="hdnIsPackageItem" value="" runat="server" />
    <input type="hidden" id="hdnClassID" value="" runat="server" />
    <input type="hidden" id="hdnMainParamedicID" value="" runat="server" />
    <input type="hidden" id="hdnParentItemID" value="" runat="server" />
    <input type="hidden" id="hdnIsAdd" value="" runat="server" />
    <input type="hidden" id="hdnHealthcareSeriv" value="" runat="server" />
    <input type="hidden" id="hdnVisitIDCtlPT" value="" runat="server" />
    <table class="tblContentArea">
        <colgroup>
            <col style="width:100%"/>
        </colgroup>
        <tr>            
            <td style="padding:5px;vertical-align:top">
                <table class="tblEntryContent" style="width:70%">
                    <colgroup>
                        <col style="width:20%"/>
                        <col/>
                    </colgroup>
                    <tr>
                        <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Pelayanan")%></label></td>
                        <td><asp:TextBox ID="txtHeaderCode" ReadOnly="true" Width="120px" runat="server" /></td>
                        <td><asp:TextBox ID="txtHeaderName" ReadOnly="true" Width="350px" runat="server" /></td>
                    </tr> 
                </table>
                <div id="containerPopupEntryData" style="margin-top:10px;display:none;">
                    <div class="pageTitle"><%=GetLabel("Entry")%></div>
                    <fieldset id="fsEntryPopup" style="margin:0"> 
                        <table class="tblEntryDetail" style="width:100%">
                            <colgroup>
                                <col style="width:150px"/>
                                <col />
                            </colgroup>
                            <tr id="trItemDetail" runat="server">
                                <td class="tdLabel"><label class="lblLink lblMandatory" id="lblPopupItem"><%=GetLabel("Pelayanan")%></label></td> 
                                <td>
                                    <input type="hidden" value="" id="hdnItemID" runat="server" />
                                    <table style="width:100%" cellpadding="0" cellspacing="0">
                                        <colgroup>
                                            <col style="width:30%"/>
                                            <col style="width:3px"/>
                                            <col/>
                                        </colgroup>
                                        <tr>
                                            <td><asp:TextBox ID="txtItemCode" CssClass="required" Width="100%" runat="server" /></td> <!-- Ini customer item code yg waktu mau insert -->
                                            <td>&nbsp;</td>
                                            <td><asp:TextBox ID="txtItemName" ReadOnly="true" CssClass="required" Width="100%" runat="server" /></td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel"><label class="lblLink lblMandatory" id="lblPopupParamedic"><%=GetLabel("Dokter / Paramedis")%></label></td> 
                                <td>
                                    <input type="hidden" value="" id="hdnRevenueSharingID" runat="server" />
                                    <input type="hidden" value="" id="hdnParamedicID" runat="server" />
                                    <table style="width:100%" cellpadding="0" cellspacing="0">
                                        <colgroup>
                                            <col style="width:30%"/>
                                            <col style="width:3px"/>
                                            <col/>
                                        </colgroup>
                                        <tr>
                                            <td><asp:TextBox ID="txtParamedicCode" CssClass="required" Width="100%" runat="server" /></td> <!-- Ini customer item code yg waktu mau insert -->
                                            <td>&nbsp;</td>
                                            <td><asp:TextBox ID="txtParamedicName" ReadOnly="true" CssClass="required" Width="100%" runat="server" /></td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Peranan")%></label></td>
                                <td>
                                    <dxe:ASPxComboBox ID="cboParamedicRole" ClientInstanceName="cboParamedicRole" Width="300px" runat="server">
                                        <ClientSideEvents SelectedIndexChanged="function(s,e){onCboParamedicRoleChanged()}" />
                                    </dxe:ASPxComboBox>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <table>
                                        <tr>
                                            <td>
                                                <input type="button" id="btnEntryPopupSave" value='<%= GetLabel("Save")%>' />
                                            </td>
                                            <td>
                                                <input type="button" id="btnEntryPopupCancel" value='<%= GetLabel("Cancel")%>' />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                </div>

                <dxcp:ASPxCallbackPanel ID="cbpEntryPopupView" runat="server" Width="100%" ClientInstanceName="cbpEntryPopupView"
                    ShowLoadingPanel="false" OnCallback="cbpEntryPopupView_Callback">
                    <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }"
                    EndCallback="function(s,e){ onCbpEntryPopupViewEndCallback(s); }" />
                    <PanelCollection>
                        <dx:PanelContent ID="PanelContent1" runat="server">
                            <asp:Panel runat="server" ID="pnlPatientVisitTransHdGrdView" Style="width: 100%; margin-left: auto; margin-right: auto; position: relative;font-size:0.95em;">
                                <asp:GridView ID="grdView" runat="server" CssClass="grdView notAllowSelect" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                    <Columns>
                                        <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="70px">
                                            <ItemTemplate>
                                               <table cellpadding="0" cellspacing="0">
                                                    <tr>
                                                        <td><img class="imgEdit imgLink" src='<%# ResolveUrl("~/Libs/Images/Button/edit.png")%>' alt="" style="float:left; margin-left:7px" /></td>
                                                        <td style="width:3px">&nbsp;</td>
                                                        <td><img class="imgDelete imgLink" src='<%# ResolveUrl("~/Libs/Images/Button/delete.png")%>' alt="" /></td>
                                                    </tr>
                                                </table>
                                                <input type="hidden" value="<%#:Eval("ID") %>" bindingfield="ID" />
                                                <input type="hidden" value="<%#:Eval("ItemID") %>" bindingfield="ItemID" />
                                                <input type="hidden" value="<%#:Eval("ItemCode") %>" bindingfield="ItemCode" />
                                                <input type="hidden" value="<%#:Eval("ItemName1") %>" bindingfield="ItemName1" />
                                                <input type="hidden" value="<%#:Eval("ParamedicID") %>" bindingfield="ParamedicID" />
                                                <input type="hidden" value="<%#:Eval("ParamedicCode") %>" bindingfield="ParamedicCode" />
                                                <input type="hidden" value="<%#:Eval("ParamedicName") %>" bindingfield="ParamedicName" />
                                                <input type="hidden" value="<%#:Eval("GCParamedicRole") %>" bindingfield="GCParamedicRole" />
                                                <input type="hidden" value="<%#:Eval("RevenueSharingID") %>" bindingfield="RevenueSharingID" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="ItemName1" HeaderText="Pelayanan" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="200px" />
                                        <asp:BoundField DataField="ParamedicName" HeaderText="Nama" ItemStyle-CssClass="tdParamedicName" ItemStyle-HorizontalAlign="Left" />
                                        <asp:BoundField DataField="ParamedicRole" HeaderText="Peranan" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="80px" />
                                    </Columns>
                                    <EmptyDataTemplate>
                                        <%=GetLabel("No Data To Display")%>
                                    </EmptyDataTemplate>
                                </asp:GridView>
                            </asp:Panel>
                        </dx:PanelContent>
                    </PanelCollection>
                </dxcp:ASPxCallbackPanel>
                <div style="width:100%;text-align:center" id="divContainerAddData" runat="server">
                    <span class="lblLink" id="lblEntryPopupAddData"><%= GetLabel("Add Data")%></span>
                </div>
            </td>
        </tr>
    </table>
    <div style="width:100%;text-align:right">
        <input type="button" value='<%= GetLabel("Close")%>' onclick="pcRightPanelContent.Hide();" />
    </div>
</div>
