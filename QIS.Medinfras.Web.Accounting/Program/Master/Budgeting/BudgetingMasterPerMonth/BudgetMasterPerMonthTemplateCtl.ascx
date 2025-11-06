<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="BudgetMasterPerMonthTemplateCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.Accounting.Program.BudgetMasterPerMonthTemplateCtl" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<script type="text/javascript" id="dxss_serviceunithealthcareentryctl">
    function onRefreshGrid() {
        cbpViewPopup.PerformCallback("refresh");
    }

    $('#chkSelectAll').die('change');
    $('#chkSelectAll').live('change', function () {
        var isChecked = $(this).is(":checked");
        $('.chkIsSelected input').each(function () {
            $(this).prop('checked', isChecked);
            $(this).change();
        });
    });

    $('.chkIsSelected input').die('change');
    $('.chkIsSelected input').live('change', function () {
        $tr = $(this).closest('tr');
        if ($(this).is(':checked')) {
            $tr.find('.txtRemaksRCC').removeAttr('readonly');
            $tr.find('.txtBudgetAmount').removeAttr('readonly');
        }
        else {
            $tr.find('.txtRemaksRCC').attr('readonly', 'readonly');
            $tr.find('.txtBudgetAmount').attr('readonly', 'readonly');
        }
    });

    //#region Template Budget
    function onGetGLAccountFilterExpression() {
        var filterExpression = "IsDeleted = 0";
        return filterExpression;
    }

    $('#lblBudgetTemplate.lblLink').live('click', function () {
        openSearchDialog('budgettemplatehd', onGetGLAccountFilterExpression(), function (value) {
            $('#<%=txtBudgetTemplateCode.ClientID %>').val(value);
            ontxtBudgetTemplateCodeChanged(value);
        });
    });

    $('#<%=txtBudgetTemplateCode.ClientID %>').live('change', function () {
        ontxtBudgetTemplateCodeChanged($(this).val());
    });

    function ontxtBudgetTemplateCodeChanged(value) {
        var filterExpression = onGetGLAccountFilterExpression() + " AND BudgetTemplateCode = '" + value + "'";
        Methods.getObject('GetvBudgetTemplateHdList', filterExpression, function (result) {
            if (result != null) {
                $('#<%=hdnBudgetTemplateHdID.ClientID %>').val(result.BudgetTemplateID);
                $('#<%=txtBudgetTemplateCode.ClientID %>').val(result.BudgetTemplateCode);
                $('#<%=txtBudgetTemplateName.ClientID %>').val(result.BudgetTemplateName);
                $('#<%=txtRemarks.ClientID %>').val(result.Remarks);
            }
            else {
                $('#<%=hdnBudgetTemplateHdID.ClientID %>').val('');
                $('#<%=txtBudgetTemplateCode.ClientID %>').val('');
                $('#<%=txtBudgetTemplateName.ClientID %>').val('');
                $('#<%=txtRemarks.ClientID %>').val('');
            }
        });
    }
    //#endregion

    $('#btnRefresh').live('click', function () {
        cbpViewPopup.PerformCallback("refresh");
    });

    function onBeforeSaveRecord(errMessage) {
        var count = 0;
        $('.chkIsSelected input').each(function () {
            if ($(this).is(':checked')) {
                count += 1;
            }
        });
        if (count == 0) {
            errMessage.text = 'Please Select Item First';
            return false;
        }
        return true;
    }

    function onCbpViewPopupEndCallback(s) {
        hideLoadingPanel();
    }
</script>
<input type="hidden" id="hdnBudgetTemplateHdID" value="" runat="server" />
<input type="hidden" id="hdnBudgetTemplateDtRCCID" value="" runat="server" />
<input type="hidden" id="hdnGLAccountID" value="" runat="server" />
<input type="hidden" id="hdnRemarksDt" value="" runat="server" />
<input type="hidden" id="hdnBudgetAmountDt" value="" runat="server" />
<table class="tblContentArea">
    <tr>
        <td style="padding: 5px; vertical-align: top">
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
                                    <label class="lblLink lblMandatory" id="lblBudgetTemplate" style="width: 100%">
                                        <%=GetLabel("Template")%></label>
                                </td>
                                <td>
                                    <asp:TextBox runat="server" ID="txtBudgetTemplateCode" Width="150px" />
                                </td>
                                <td>
                                    <asp:TextBox runat="server" ID="txtBudgetTemplateName" Width="250px" ReadOnly="true" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <label class="lblNormal" style="width: 100%">
                                        <%=GetLabel("Keterangan")%></label>
                                </td>
                                <td colspan="2">
                                    <asp:TextBox ID="txtRemarks" runat="server" Width="100%" TextMode="MultiLine" Rows="2"
                                        ReadOnly="true" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <div id="divRefresh" runat="server" style="float: left; margin-top: 0px;">
                                        <input type="button" id="btnRefresh" value="R e f r e s h" class="btnRefresh w3-button w3-blue w3-border w3-border-blue w3-round-large" />
                                    </div>
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
            <dxcp:ASPxCallbackPanel ID="cbpViewPopup" runat="server" Width="100%" ClientInstanceName="cbpViewPopup"
                ShowLoadingPanel="false" OnCallback="cbpViewPopup_Callback">
                <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ onCbpViewPopupEndCallback(s); }" />
                <PanelCollection>
                    <dx:PanelContent ID="PanelContent1" runat="server">
                        <asp:Panel runat="server" ID="pnlView" Style="width: 100%; margin-left: auto; margin-right: auto;
                            position: relative; font-size: 0.90em; max-height: 300px; overflow-y: scroll;">
                            <asp:GridView ID="grdView" runat="server" CssClass="grdService grdNormal notAllowSelect"
                                AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty"
                                OnRowDataBound="grdView_RowDataBound">
                                <Columns>
                                    <asp:BoundField DataField="BudgetTemplateRCCID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                    <asp:TemplateField HeaderStyle-Width="20px" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                        <HeaderTemplate>
                                            <input id="chkSelectAll" type="checkbox" />
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chkIsSelected" runat="server" CssClass="chkIsSelected" />
                                            <input type="hidden" class="keyField" id="keyField" runat="server" value='<%#: Eval("BudgetTemplateRCCID")%>' />
                                            <input type="hidden" id="hdnGLAccountID" runat="server" value='<%#: Eval("GLAccountID")%>' />
                                            <input type="hidden" id="hdnDepartmentID" runat="server" value='<%#: Eval("DepartmentID")%>' />
                                            <input type="hidden" id="hdnRevenueCostCenterID" runat="server" value='<%#: Eval("RevenueCostCenterID")%>' />
                                            <input type="hidden" id="hdnBudgetTemplateDtID" runat="server" value='<%#: Eval("BudgetTemplateDtID")%>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="GLAccountNo" HeaderText="Kode COA" HeaderStyle-Width="50px"
                                        HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                                    <asp:BoundField DataField="GLAccountName" HeaderText="Nama COA" HeaderStyle-Width="100px"
                                        HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                                    <asp:BoundField DataField="DepartmentName" HeaderText="Department"
                                        HeaderStyle-Width="100px" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                                    <asp:BoundField DataField="RevenueCostCenterName" HeaderText="Revenue Cost Center"
                                        HeaderStyle-Width="100px" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                                    <asp:TemplateField HeaderStyle-Width="100px" HeaderText="Keterangan Detail" ItemStyle-HorizontalAlign="Left"
                                        HeaderStyle-HorizontalAlign="Left">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtRemaksRCC" ReadOnly="true" Width="99%" value="" runat="server"
                                                CssClass="txtRemaksRCC" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderStyle-Width="50px" HeaderText="Nominal" ItemStyle-HorizontalAlign="Center"
                                        HeaderStyle-HorizontalAlign="Right">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtBudgetAmount" ReadOnly="true" Width="99%" runat="server" CssClass="txtCurrency txtBudgetAmount" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <EmptyDataTemplate>
                                    <%=GetLabel("Tidak ada data")%>
                                </EmptyDataTemplate>
                            </asp:GridView>
                        </asp:Panel>
                    </dx:PanelContent>
                </PanelCollection>
            </dxcp:ASPxCallbackPanel>
            <div class="imgLoadingGrdView" id="containerImgLoadingView">
                <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
            </div>
        </td>
    </tr>
</table>
