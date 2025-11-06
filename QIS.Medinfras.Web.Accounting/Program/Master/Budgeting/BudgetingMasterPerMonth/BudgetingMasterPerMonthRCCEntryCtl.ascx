<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="BudgetingMasterPerMonthRCCEntryCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.Accounting.Program.BudgetingMasterPerMonthRCCEntryCtl" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<script type="text/javascript" id="dxss_BudgetingMasterDtRCCEntryCtl">
    $(function () {
        $('#btnPopupCancel').click(function () {
            $('#trEditEntry').hide();
        });

        $('#btnPopupSave').click(function () {
            cbpPopupProcess.PerformCallback('save');
        });
    });

    //#region Department
    function onGetDepartmentFilterExpression() {
        var filterExpression = "";
        return filterExpression;
    }

    $('#<%=lblDepartment.ClientID %>.lblLink').live('click', function () {
        openSearchDialog('departmentanggaran', onGetDepartmentFilterExpression(), function (value) {
            $('#<%=txtDepartmentID.ClientID %>').val(value);
            ontxtDepartmentIDChanged(value);
        });
    });

    $('#<%=txtDepartmentID.ClientID %>').live('change', function () {
        var param = $('#<%=txtDepartmentID.ClientID %>').val();
        ontxtDepartmentIDChanged(param);
    });

    function ontxtDepartmentIDChanged(value) {
        var filterExpression = onGetDepartmentFilterExpression() + "DepartmentID = '" + value + "'";
        Methods.getObject('GetDepartmentList', filterExpression, function (result) {
            if (result != null) {
                $('#<%=hdnDepartmentID.ClientID %>').val(result.DepartmentID);
                $('#<%=txtDepartmentName.ClientID %>').val(result.DepartmentName);
            }
            else {
                $('#<%=hdnDepartmentID.ClientID %>').val('');
                $('#<%=txtDepartmentID.ClientID %>').val('');
                $('#<%=txtDepartmentName.ClientID %>').val('');
            }
        });
    }
    //#endregion

    //#region Revenue Cost Center
    $('#lblRCC.lblLink').live('click', function () {
        var filter = "IsDeleted = 0";
        openSearchDialog('revenuecostcenter', filter, function (value) {
            $('#<%=txtRCCCode.ClientID %>').val(value);
            ontxtRevenueCostCenterCodeChanged(value);
        });
    });

    $('#<%=txtRCCCode.ClientID %>').live('change', function () {
        var param = $('#<%=txtRCCCode.ClientID %>').val();
        ontxtRevenueCostCenterCodeChanged(param);
    });

    function ontxtRevenueCostCenterCodeChanged(value) {
        var filterExpression = "RevenueCostCenterCode = '" + $('#<%=txtRCCCode.ClientID %>').val() + "'";
        Methods.getObject('GetRevenueCostCenterList', filterExpression, function (result) {
            if (result != null) {
                $('#<%=hdnRCCID.ClientID %>').val(result.RevenueCostCenterID);
                $('#<%=txtRCCName.ClientID %>').val(result.RevenueCostCenterName);

            }
            else {
                $('#<%=hdnRCCID.ClientID %>').val('');
                $('#<%=txtRCCCode.ClientID %>').val('');
                $('#<%=txtRCCName.ClientID %>').val('');

            }
        });
    }
    //#endregion

    $('#lblPopupAddData').die('click');
    $('#lblPopupAddData').live('click', function () {
        $('#<%=hdnID.ClientID %>').val('');

        $('#<%=hdnDepartmentID.ClientID %>').val('');
        $('#<%=txtDepartmentID.ClientID %>').val('');
        $('#<%=txtDepartmentName.ClientID %>').val('');

        $('#<%=hdnRCCID.ClientID %>').val('');
        $('#<%=txtRCCCode.ClientID %>').val('');
        $('#<%=txtRCCName.ClientID %>').val('');

        $('#<%=txtRemarksDtCtl.ClientID %>').val('');

        $('#<%=txtBudgetAmountCtl.ClientID %>').val('0').trigger('changeValue');

        $('#trEditEntry').show();
    });

    $('.imgPopupEdit').die('click');
    $('.imgPopupEdit').live('click', function () {
        $row = $(this).closest('tr').parent().closest('tr');
        var entity = rowToObject($row);

        $('#<%=hdnID.ClientID %>').val(entity.ID);
        $('#<%=hdnBudgetDtIDCtl.ClientID %>').val(entity.BudgetDtID);

        $('#<%=hdnDepartmentID.ClientID %>').val(entity.DepartmentID);
        $('#<%=txtDepartmentID.ClientID %>').val(entity.DepartmentID);
        $('#<%=txtDepartmentName.ClientID %>').val(entity.DepartmentName);

        $('#<%=hdnRCCID.ClientID %>').val(entity.RevenueCostCenterID);
        $('#<%=txtRCCCode.ClientID %>').val(entity.RevenueCostCenterCode);
        $('#<%=txtRCCName.ClientID %>').val(entity.RevenueCostCenterName);

        $('#<%=txtRemarksDtCtl.ClientID %>').val(entity.Remarks);

        $('#<%=txtBudgetAmountCtl.ClientID %>').val(entity.BudgetAmount).trigger('changeValue');

        $('#trEditEntry').show();
    });

    $('.imgPopupDelete').die('click');
    $('.imgPopupDelete').live('click', function () {
        $row = $(this).closest('tr').parent().closest('tr');
        showToastConfirmation('Are You Sure Want To Delete?', function (result) {
            if (result) {
                var entity = rowToObject($row);
                $('#<%=hdnID.ClientID %>').val(entity.ID);
                cbpPopupProcess.PerformCallback('delete');
            }
        });
    });

    function onCbpPopupProcesEndCallback(s) {
        var param = s.cpResult.split('|');
        if (param[0] == 'save') {
            if (param[1] == 'fail')
                showToast('Save Failed', 'Error Message : ' + param[2]);
            else {
                $('#lblPopupAddData').click();
                cbpPopupView.PerformCallback();
            }
        }
        else if (param[0] == 'delete') {
            if (param[1] == 'fail') {
                showToast('Delete Failed', 'Error Message : ' + param[2]);
            }
            cbpPopupView.PerformCallback();
        }
        hideLoadingPanel();
    }
</script>
<input type="hidden" id="hdnID" runat="server" value="" />
<input type="hidden" id="hdnBudgetDtIDCtl" runat="server" value="" />
<input type="hidden" id="hdnDepartmentID" runat="server" value="" />
<input type="hidden" id="hdnRCCID" runat="server" value="" />
<table class="tblContentArea">
    <tr id="trEditEntry" style="display: none;">
        <td>
            <table style="width: 100%" class="tblEntryDetail">
                <colgroup>
                    <col style="width: 50%" />
                    <col style="width: 50%" />
                </colgroup>
                <tr>
                    <td align="left">
                        <table width="100%">
                            <colgroup>
                                <col style="width: 200px;" />
                                <col style="width: 120px;" />
                                <col style="width: 150px;" />
                                <col />
                            </colgroup>
                            <tr>
                                <td>
                                    <label class="lblLink lblMandatory" id="lblDepartment" runat="server">
                                        <%=GetLabel("Department")%></label>
                                </td>
                                <td>
                                    <asp:TextBox runat="server" ID="txtDepartmentID" Width="260px" />
                                </td>
                                <td>
                                    <asp:TextBox runat="server" ID="txtDepartmentName" ReadOnly="true" Width="500px" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <label class="lblLink lblMandatory" id="lblRCC" style="width: 100%">
                                        <%=GetLabel("RCC")%></label>
                                </td>
                                <td>
                                    <asp:TextBox runat="server" ID="txtRCCCode" Width="260px" />
                                </td>
                                <td>
                                    <asp:TextBox runat="server" ID="txtRCCName" ReadOnly="true" Width="500px" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <label class="lblNormal" style="width: 100%">
                                        <%=GetLabel("Keterangan Detail")%></label>
                                </td>
                                <td colspan="2">
                                    <asp:TextBox ID="txtRemarksDtCtl" runat="server" Width="100%" TextMode="MultiLine"
                                        Rows="2" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <label class="lblMandatory" style="width: 100%">
                                        <%=GetLabel("Nilai Anggaran")%></label>
                                </td>
                                <td colspan="2">
                                    <asp:TextBox ID="txtBudgetAmountCtl" runat="server" Width="260px" CssClass="txtCurrency" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                </td>
                                <td colspan="2" style="width: 100%">
                                    <table>
                                        <tr>
                                            <td>
                                                <input type="button" id="btnPopupSave" style="width: 60px;" value='<%= GetLabel("Save")%>' />
                                            </td>
                                            <td>
                                                <input type="button" id="btnPopupCancel" style="width: 60px;" value='<%= GetLabel("Cancel")%>' />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
    <tr>
        <td>
            <dxcp:ASPxCallbackPanel ID="cbpPopupView" runat="server" Width="100%" ClientInstanceName="cbpPopupView"
                ShowLoadingPanel="false" OnCallback="cbpPopupView_Callback">
                <PanelCollection>
                    <dx:PanelContent ID="PanelContent1" runat="server">
                        <asp:Panel runat="server" ID="pnlView" Style="width: 100%; margin-left: auto; margin-right: auto;
                            position: relative; font-size: 0.95em;">
                            <asp:GridView ID="grdView" runat="server" CssClass="grdNormal" AutoGenerateColumns="false"
                                ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                <Columns>
                                    <asp:BoundField DataField="ID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                    <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="50px">
                                        <ItemTemplate>
                                            <table cellpadding="0" cellspacing="0">
                                                <tr>
                                                    <td>
                                                        <img class="imgPopupEdit imgLink" title='<%=GetLabel("Edit")%>' src='<%#ResolveUrl("~/Libs/Images/Button/edit.png")%>'
                                                            alt="" />
                                                    </td>
                                                    <td style="width: 1px">
                                                        &nbsp;
                                                    </td>
                                                    <td>
                                                        <img class="imgPopupDelete imgLink" title='<%=GetLabel("Delete")%>' src='<%# ResolveUrl("~/Libs/Images/Button/delete.png")%>'
                                                            alt="" />
                                                    </td>
                                                </tr>
                                            </table>
                                            <input type="hidden" value="<%#:Eval("ID") %>" bindingfield="ID" />
                                            <input type="hidden" value="<%#:Eval("BudgetTransactionDtID") %>" bindingfield="BudgetDtID" />
                                            <input type="hidden" value="<%#:Eval("DepartmentID") %>" bindingfield="DepartmentID" />
                                            <input type="hidden" value="<%#:Eval("DepartmentName") %>" bindingfield="DepartmentName" />
                                            <input type="hidden" value="<%#:Eval("RevenueCostCenterID") %>" bindingfield="RevenueCostCenterID" />
                                            <input type="hidden" value="<%#:Eval("RevenueCostCenterCode") %>" bindingfield="RevenueCostCenterCode" />
                                            <input type="hidden" value="<%#:Eval("RevenueCostCenterName") %>" bindingfield="RevenueCostCenterName" />
                                            <input type="hidden" value="<%#:Eval("Remarks") %>" bindingfield="Remarks" />
                                            <input type="hidden" value="<%#:Eval("BudgetAmount") %>" bindingfield="BudgetAmount" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderStyle-Width="200px" HeaderStyle-HorizontalAlign="Left">
                                        <HeaderTemplate>
                                            <%=GetLabel("Department")%></HeaderTemplate>
                                        <ItemTemplate>
                                            <div style="font-size: 14px;">
                                                <%#:Eval("DepartmentID") %></div>
                                            <div style="font-size: 12px;">
                                                <%#:Eval("DepartmentName") %></div>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderStyle-Width="200px" HeaderStyle-HorizontalAlign="Left">
                                        <HeaderTemplate>
                                            <%=GetLabel("RCC")%></HeaderTemplate>
                                        <ItemTemplate>
                                            <div style="font-size: 14px;">
                                                <%#:Eval("RevenueCostCenterName") %></div>
                                            <div style="font-size: 12px;">
                                                <%#:Eval("RevenueCostCenterCode") %></div>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="Remarks" HeaderText="Keterangan" HeaderStyle-HorizontalAlign="Left" />
                                    <asp:TemplateField ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right"
                                        HeaderStyle-Width="200px">
                                        <HeaderTemplate>
                                            <%=GetLabel("Nilai Anggaran") %></HeaderTemplate>
                                        <ItemTemplate>
                                            <%#:Eval("BudgetAmount", "{0:N}")%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderStyle-Width="200px" HeaderStyle-HorizontalAlign="Left">
                                        <HeaderTemplate>
                                            <%=GetLabel("Informasi Dibuat")%></HeaderTemplate>
                                        <ItemTemplate>
                                            <div style="font-size: 14px;">
                                                <%#:Eval("CreatedByName") %></div>
                                            <div style="font-size: 10px;">
                                                <%#:Eval("cfCreatedDateInString") %></div>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderStyle-Width="200px" HeaderStyle-HorizontalAlign="Left">
                                        <HeaderTemplate>
                                            <%=GetLabel("Informasi Diubah")%></HeaderTemplate>
                                        <ItemTemplate>
                                            <div style="font-size: 14px;">
                                                <%#:Eval("LastUpdatedByName") %></div>
                                            <div style="font-size: 10px;">
                                                <%#:Eval("cfLastUpdatedDateInString") %></div>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <EmptyDataTemplate>
                                    <%=GetLabel("No Data To Display")%>
                                </EmptyDataTemplate>
                            </asp:GridView>
                        </asp:Panel>
                        <div style="width: 100%; text-align: center">
                            <span class="lblLink" id="lblPopupAddData" style="text-align: center">
                                <%= GetLabel("Tambah Data")%></span>
                        </div>
                    </dx:PanelContent>
                </PanelCollection>
            </dxcp:ASPxCallbackPanel>
            <div class="imgLoadingGrdView" id="containerImgLoadingView">
                <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
            </div>
        </td>
    </tr>
    <dxcp:ASPxCallbackPanel ID="cbpPopupProcess" runat="server" Width="100%" ClientInstanceName="cbpPopupProcess"
        ShowLoadingPanel="false" OnCallback="cbpPopupProcess_Callback">
        <ClientSideEvents BeginCallback="function(s,e) { showLoadingPanel(); }" EndCallback="function(s,e) { onCbpPopupProcesEndCallback(s); }" />
    </dxcp:ASPxCallbackPanel>
</table>
