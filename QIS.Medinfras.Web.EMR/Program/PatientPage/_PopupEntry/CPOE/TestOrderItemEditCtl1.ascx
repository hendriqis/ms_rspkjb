<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TestOrderItemEditCtl1.ascx.cs"
    Inherits="QIS.Medinfras.Web.EMR.Program.TestOrderItemEditCtl1" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="QIS.Medinfras.Web.CustomControl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"
    Namespace="QIS.Medinfras.Web.CustomControl" TagPrefix="qis" %>
<script type="text/javascript" id="dxss_testorderdetail1">
    $('#btnEntryPopupSaveDetail').click(function (evt) {
        if (IsValid(evt, 'fsEntryPopup', 'mpEntryPopup'))
            cbpViewPopUpCtl.PerformCallback('save');
        return false;
    });

    $('#btnSaveHeaderTestOrder').live('click', function () {
        cbpViewPopUpCtl.PerformCallback('saveHeader');
        pcRightPanelContent.Hide();
    });

    $('#btnEntryPopupCancelDetail').live('click', function () {
        $('#containerPopupEntryData').hide();
    });

    $('.imgDeleteDetailItem.imgLink').die('click');
    $('.imgDeleteDetailItem.imgLink').live('click', function () {
        $row = $(this).closest('tr');
        $('#<%=hdnID.ClientID %>').val($row.find('.hdnID').val());
        var itemName = $row.find('.hdnItemName').val();
        var message = "Are You Sure Want To Delete This Record <b>" + itemName + "</b> ?";
        showToastConfirmation(message, function (result) {
            if (result) {
                cbpViewPopUpCtl.PerformCallback('delete');
            }
        });
    });

    $('.imgEditDetailItem.imgLink').die('click');
    $('.imgEditDetailItem.imgLink').live('click', function () {
        $row = $(this).closest('tr');
        var itemID = $row.find('.hdnItemID').val();
        var itemName = $row.find('.hdnItemName').val();
        var itemQty = $row.find('.hdnItemQty').val();
        var diagnoseID = $row.find('.hdnDiagnoseID').val();
        var remarks = $row.find('.hdnRemarks').val();
        var isCITO = $row.find('.hdnIsCITO').val();

        $('#<%=hdnID.ClientID %>').val($row.find('.hdnID').val());
        $('#<%=hdnItemID.ClientID %>').val(itemID);
        cboServiceUnitItem.SetValue(itemID);
        $('#<%=txtItemQty.ClientID %>').val(itemQty);
        cboDiagnose.SetValue(diagnoseID);
        $('#<%=txtRemarks.ClientID %>').val(remarks);
        $('#<%:chkIsCITO.ClientID %>').prop("checked", isCITO == 'True');

        $('#containerPopupEntryData').show();
    });

    var pageCount = parseInt('<%=PageCount %>');
    $(function () {
        setPaging($("#paging"), pageCount, function (page) {
            cbpViewPopUpCtl.PerformCallback('changepage|' + page);
        });
    });

    function onLedItemLostFocus(value) {
        $('#<%=hdnItemID.ClientID %>').val(value);
    }

    function oncbpViewPopUpCtlEndCallback(s) {
        var param = s.cpResult.split('|');
        if (param[0] == 'save' || param[0] == 'saveHeader') {
            if (param[1] == 'fail') {
                showToast('Save Failed', 'Error Message : ' + param[2]);
            }
            else {
                $('#containerPopupEntryData').hide();
                cbpViewPopUpCtl.PerformCallback('refresh');
            }
        }
        else if (param[0] == 'delete') {
            if (param[1] == 'fail') {
                showToast('Delete Failed', 'Error Message : ' + param[2]);
            }
            else {
                var pageCount = parseInt(param[2]);
                cbpViewPopUpCtl.PerformCallback('refresh');
            }
        }
        else {
            $('#<%=grdView.ClientID %> tr:eq(1)').click();
        }

        onRefreshLaboratoryGrid();
        onRefreshImagingGrid();
        onRefreshDiagnosticGrid();

        $('#containerPopupEntryData').hide();
        $('#containerImgLoadingViewPopup').hide();
    }

    function onAfterSaveRecordPatientPageEntry(value) {
        if ($('#<%=hdnTestOrderType.ClientID %>').val() == 'LB') {
            if (typeof onRefreshLaboratoryGrid == 'function')
                onRefreshLaboratoryGrid();
        }
        else {
            if (typeof onRefreshImagingGrid == 'function')
                onRefreshImagingGrid();
        }
    }
</script>
<input type="hidden" id="hdnID" runat="server" value="" />
<input type="hidden" id="hdnTestOrderType" runat="server" value="" />
<input type="hidden" id="hdnTestOrderID" runat="server" value="" />
<input type="hidden" id="hdnHealthcareServiceUnitID" runat="server" value="" />
<input type="hidden" id="hdnItemID" runat="server" value="" />
<input type="hidden" value="" id="hdnIsUsingMultiVisitScheduleOrder" runat="server" />
<div>
    <table>
        <tr>
            <td>
                <label class="lblNormal">
                    <%=GetLabel("No Order")%></label>
            </td>
            <td>
                <asp:TextBox ID="txtOrderNo" Width="100%" runat="server" ReadOnly="true" />
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;
            </td>
            <td colspan="2">
                <asp:CheckBox ID="chkIsPathologicalAnatomyTest" Width="100%" runat="server" Text="Pemeriksaan PA" />
            </td>
        </tr>
        <tr id="trMultiVisitScheduleOrder" runat="server" style="display: none">
            <td>
                &nbsp;
            </td>
            <td colspan="2">
                <asp:CheckBox ID="chkIsMultiVisitScheduleOrder" Width="100%" runat="server" Text="Penjadwalan Multi Kunjungan" />
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;
            </td>
            <td>
                <input type="button" id="btnSaveHeaderTestOrder" value='<%= GetLabel("Simpan")%>' class="btnProposeTestOrder w3-btn w3-hover-blue"
                    style="width: 80px; height: 35px" />
            </td>
        </tr>
    </table>
</div>
<div id="containerPopupEntryData" style="margin-top: 10px; display: none;">
    <div class="pageTitle">
        <%=GetLabel("Edit Detail")%></div>
    <fieldset id="fsEntryPopup" style="margin: 0">
        <table class="tblEntryDetail" style="width: 100%">
            <tr>
                <td>
                    <table style="width: 100%">
                        <colgroup>
                            <col style="width: 100px" />
                        </colgroup>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblMandatory">
                                    <%=GetLabel("Item")%></label>
                            </td>
                            <td colspan="2">
                                <dxe:ASPxComboBox ID="cboServiceUnitItem" ClientInstanceName="cboServiceUnitItem"
                                    Width="100%" runat="server">
                                </dxe:ASPxComboBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel" style="vertical-align: top; padding-top: 5px;">
                                <label class="lblNormal">
                                    <%=GetLabel("Jumlah")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtItemQty" Width="100px" runat="server" CssClass="number" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Diagnose")%></label>
                            </td>
                            <td colspan="2">
                                <dxe:ASPxComboBox runat="server" ID="cboDiagnose" ClientInstanceName="cboDiagnose"
                                    Width="100%" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel" style="vertical-align: top; padding-top: 5px;">
                                <label class="lblNormal">
                                    <%=GetLabel("Remarks")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtRemarks" Width="100%" runat="server" TextMode="MultiLine" />
                            </td>
                        </tr>
                        <tr>
                            <td />
                            <td>
                                <asp:CheckBox ID="chkIsCITO" Width="100px" runat="server" Text="CITO" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <center>
                                    <table>
                                        <tr>
                                            <td>
                                                <input type="button" id="btnEntryPopupSaveDetail" value='<%= GetLabel("Simpan")%>' class="btnProposeTestOrder w3-btn w3-hover-blue"
                                                    style="width: 80px; height: 35px" />
                                            </td>
                                            <td>
                                                <input type="button" id="btnEntryPopupCancelDetail" value='<%= GetLabel("Batal")%>' class="btnProposeTestOrder w3-btn w3-hover-blue"
                                                    style="width: 80px; height: 35px" />
                                            </td>
                                        </tr>
                                    </table>
                                </center>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </fieldset>
</div>
<div style="position: relative;">
    <dxcp:ASPxCallbackPanel ID="cbpViewPopUpCtl" runat="server" Width="100%" ClientInstanceName="cbpViewPopUpCtl"
        ShowLoadingPanel="false" OnCallback="cbpViewPopUpCtl_Callback">
        <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingViewPopup').show(); }"
            EndCallback="function(s,e){ oncbpViewPopUpCtlEndCallback(s); }" />
        <PanelCollection>
            <dx:PanelContent ID="PanelContent6" runat="server">
                <asp:Panel runat="server" ID="Panel5" CssClass="pnlContainerGrid" Style="height: 300px">
                    <asp:GridView ID="grdView" runat="server" CssClass="grdSelected grdPatientPage" AutoGenerateColumns="false"
                        ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                        <Columns>
                            <asp:BoundField DataField="ID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                            <asp:TemplateField HeaderStyle-CssClass="hiddenColumn" ItemStyle-CssClass="hiddenColumn">
                                <ItemTemplate>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderStyle-Width="70px" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="20px">
                                <ItemTemplate>
                                    <input type="hidden" class="hdnID" value="<%#: Eval("ID")%>" />
                                    <input type="hidden" class="hdnItemID" value="<%#: Eval("ItemID")%>" />
                                    <input type="hidden" class="hdnItemQty" value="<%#: Eval("ItemQty")%>" />
                                    <input type="hidden" class="hdnDiagnoseID" value="<%#: Eval("DiagnoseID")%>" />
                                    <input type="hidden" class="hdnItemName" value="<%#: Eval("ItemName1")%>" />
                                    <input type="hidden" class="hdnIsCITO" value="<%#:Eval("IsCITO") %>" />
                                    <input type="hidden" class="hdnRemarks" value="<%#:Eval("Remarks") %>" />
                                    <img class="imgEditDetailItem imgLink" title='<%=GetLabel("Edit")%>' src='<%# ResolveUrl("~/Libs/Images/Button/edit.png")%>'
                                        alt="" style="float: left; margin-left: 7px" />
                                    <img class="imgDeleteDetailItem imgLink" title='<%=GetLabel("Delete")%>' src='<%# ResolveUrl("~/Libs/Images/Button/delete.png")%>'
                                        alt="" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderStyle-HorizontalAlign="Left">
                                <HeaderTemplate>
                                    <%=GetLabel("Nama Pemeriksaan")%>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <div>
                                        <%#: Eval("ItemName1")%></div>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderStyle-Width="70px" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right">
                                <HeaderTemplate>
                                    <%=GetLabel("Qty")%>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <div>
                                        <%#: Eval("ItemQty")%></div>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <EmptyDataTemplate>
                            <%=GetLabel("Tidak ada informasi pemeriksaan untuk pasien ini") %>
                        </EmptyDataTemplate>
                    </asp:GridView>
                    <div class="imgLoadingGrdView" id="containerImgLoadingViewPopup">
                        <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                    </div>
                </asp:Panel>
            </dx:PanelContent>
        </PanelCollection>
    </dxcp:ASPxCallbackPanel>
    <div class="containerPaging">
        <div class="wrapperPaging">
            <div id="paging">
            </div>
        </div>
    </div>
</div>
