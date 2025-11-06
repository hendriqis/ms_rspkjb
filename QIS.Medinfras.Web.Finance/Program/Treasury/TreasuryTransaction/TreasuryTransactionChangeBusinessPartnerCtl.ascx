<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TreasuryTransactionChangeBusinessPartnerCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.TreasuryTransactionChangeBusinessPartnerCtl" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="QIS.Medinfras.Web.CustomControl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"
    Namespace="QIS.Medinfras.Web.CustomControl" TagPrefix="qis" %>
<script type="text/javascript" id="dxss_TreasuryTransactionChangeBusinessPartnerCtl">

    $('#lblEntryPopupAddData').die('click');
    $('#lblEntryPopupAddData').live('click', function () {

        $('#containerPopupEntryData').show();
    });

    $('#btnEntryPopupCancel').die('click');
    $('#btnEntryPopupCancel').live('click', function () {
        $('#containerPopupEntryData').hide();
    });

    $('#btnEntryPopupSave').die('click');
    $('#btnEntryPopupSave').live('click', function (evt) {
        if (IsValid(evt, 'fsEntryPopup', 'mpEntryPopup')) {
            cbpEntryPopupView.PerformCallback('save');
        }
        else {
            return false;
        }

    });

    $('.imgEdit.imgLink').die('click');
    $('.imgEdit.imgLink').live('click', function () {
        $row = $(this).closest('tr');

        var ID = $row.find('.hdnID').val();
        var customerGroupID = $row.find('.hdnCustomerGroupIDCBP').val();
        var customerGroupCode = $row.find('.hdnCustomerGroupCodeCBP').val();
        var customerGroupName = $row.find('.hdnCustomerGroupNameCBP').val();

        var businessPartnerID = $row.find('.hdnBusinessPartnerIDCBP').val();
        var businessPartnerCode = $row.find('.hdnBusinessPartnerCodeCBP').val();
        var businessPartnerName = $row.find('.hdnBusinessPartnerNameCBP').val();

        $('#<%=hdnID.ClientID %>').val(ID);
        $('#<%=hdnCustomerGroupIDCBP.ClientID %>').val(customerGroupID);
        $('#<%=txtCustomerGroupCodeCBP.ClientID %>').val(customerGroupCode);
        $('#<%=txtCustomerGroupNameCBP.ClientID %>').val(customerGroupName);

        $('#<%=hdnBusinessPartnerIDCBP.ClientID %>').val(businessPartnerID);
        $('#<%=txtBusinessPartnerCodeCBP.ClientID %>').val(businessPartnerCode);
        $('#<%=txtBusinessPartnerNameCBP.ClientID %>').val(businessPartnerName);

        $('#containerPopupEntryData').show();
    });

    function setPagingDetailItem(pageCount) {
        if (pageCount > 0)
            $('.grdPopup tr:eq(1)').click();
        setPaging($("#pagingPopup"), pageCount, function (page) {
            cbpEntryPopupView.PerformCallback('changepage|' + page);
        }, 8);
    }

    function onCbpEntryPopupViewEndCallback(s) {
        var param = s.cpResult.split('|');
        if (param[0] == 'save') {
            if (param[1] == 'fail')
                showToast('Save Failed', 'Error Message : ' + param[2]);
            else {
                $('#lblEntryPopupAddData').click();
                var pageCount = parseInt(param[2]);
                setPagingDetailItem(pageCount);
                $('#containerPopupEntryData').hide();
            }
        }
        else if (param[0] == 'delete') {
            if (param[1] == 'fail')
                showToast('Delete Failed', 'Error Message : ' + param[2]);
            else {
                var pageCount = parseInt(param[2]);
                setPagingDetailItem(pageCount);
            }
        }
        else if (param[0] == 'refresh') {
            var pageCount = parseInt(param[1]);
            setPagingDetailItem(pageCount);
        }
        else {
            $('.grdPopup tr:eq(1)').click();
        }
        hideLoadingPanel();
    }

    function onTxtSearchViewSearchClick(s) {
        setTimeout(function () {
            s.SetBlur();
            onRefreshControl();
            setTimeout(function () {
                s.SetFocus();
            }, 0);
        }, 0);
    }

    function onRefreshControl(filterExpression) {
        $('#<%=hdnFilterExpressionQuickSearch.ClientID %>').val(txtSearchView.GenerateFilterExpression());
        cbpEntryPopupView.PerformCallback('refresh');
    }

    //#region Customer Group
    $('#lblCustomerGroupCBP.lblLink').die('click');
    $('#lblCustomerGroupCBP.lblLink').live('click', function () {
        openSearchDialog('customergroup', 'IsDeleted = 0', function (value) {
            $('#<%=txtCustomerGroupCodeCBP.ClientID %>').val(value);
            ontxtCustomerGroupCodeCBPChanged(value);
        });
    });

    $('#<%=txtCustomerGroupCodeCBP.ClientID %>').live('change', function () {
        var param = $('#<%=txtCustomerGroupCodeCBP.ClientID %>').val();
        ontxtCustomerGroupCodeCBPChanged(param);
    });

    function ontxtCustomerGroupCodeCBPChanged(value) {
        var filterExpression = "CustomerGroupCode = '" + $('#<%=txtCustomerGroupCodeCBP.ClientID %>').val() + "'";
        Methods.getObject('GetCustomerGroupList', filterExpression, function (result) {
            if (result != null) {
                $('#<%=hdnCustomerGroupIDCBP.ClientID %>').val(result.CustomerGroupID);
                $('#<%=txtCustomerGroupNameCBP.ClientID %>').val(result.CustomerGroupName);
            }
            else {
                $('#<%=hdnCustomerGroupIDCBP.ClientID %>').val('');
                $('#<%=txtCustomerGroupCodeCBP.ClientID %>').val('');
                $('#<%=txtCustomerGroupNameCBP.ClientID %>').val('');
            }
        });
    }
    //#endregion

    //#region Business Partner
    function onGetBusinessPartnerFilterExpression() {
        var filterExpression = "IsDeleted = 0";

        if ($('#<%=hdnCustomerGroupIDCBP.ClientID %>').val() != "" && $('#<%=hdnCustomerGroupIDCBP.ClientID %>').val() != "0") {
            filterExpression += " AND BusinessPartnerID IN (SELECT BusinessPartnerID FROM Customer WHERE CustomerGroupID = " + $('#<%=hdnCustomerGroupIDCBP.ClientID %>').val() + ")";
        }

        return filterExpression;
    }

    $('#lblBusinessPartnerCBP.lblLink').die('click');
    $('#lblBusinessPartnerCBP.lblLink').live('click', function () {
        openSearchDialog('businesspartnersakun', onGetBusinessPartnerFilterExpression(), function (value) {
            $('#<%=txtBusinessPartnerCodeCBP.ClientID %>').val(value);
            ontxtBusinessPartnerCodeCBPChanged(value);
        });
    });

    $('#<%=txtBusinessPartnerCodeCBP.ClientID %>').live('change', function () {
        var param = $('#<%=txtBusinessPartnerCodeCBP.ClientID %>').val();
        ontxtBusinessPartnerCodeCBPChanged(param);
    });

    function ontxtBusinessPartnerCodeCBPChanged(value) {
        var filterExpression = onGetBusinessPartnerFilterExpression() + " AND BusinessPartnerCode = '" + $('#<%=txtBusinessPartnerCodeCBP.ClientID %>').val() + "'";
        Methods.getObject('GetBusinessPartnersList', filterExpression, function (result) {
            if (result != null) {
                $('#<%=hdnBusinessPartnerIDCBP.ClientID %>').val(result.BusinessPartnerID);
                $('#<%=txtBusinessPartnerNameCBP.ClientID %>').val(result.BusinessPartnerName);

                if ($('#<%=hdnCustomerGroupIDCBP.ClientID %>').val() == "" || $('#<%=hdnCustomerGroupIDCBP.ClientID %>').val() == "0") {
                    var filterCust = "CustomerGroupID = (SELECT CustomerGroupID FROM Customer WHERE BusinessPartnerID = " + result.BusinessPartnerID + ")";
                    Methods.getObject('GetCustomerGroupList', filterCust, function (resultCust) {
                        if (resultCust != null) {
                            $('#<%=hdnCustomerGroupIDCBP.ClientID %>').val(resultCust.CustomerGroupID);
                            $('#<%=txtCustomerGroupCodeCBP.ClientID %>').val(resultCust.CustomerGroupCode);
                            $('#<%=txtCustomerGroupNameCBP.ClientID %>').val(resultCust.CustomerGroupName);
                        }
                        else {
                            $('#<%=hdnCustomerGroupIDCBP.ClientID %>').val('');
                            $('#<%=txtCustomerGroupCodeCBP.ClientID %>').val('');
                            $('#<%=txtCustomerGroupNameCBP.ClientID %>').val('');
                        }
                    });
                }
            }
            else {
                $('#<%=hdnBusinessPartnerIDCBP.ClientID %>').val('');
                $('#<%=txtBusinessPartnerCodeCBP.ClientID %>').val('');
                $('#<%=txtBusinessPartnerNameCBP.ClientID %>').val('');

                $('#<%=hdnCustomerGroupIDCBP.ClientID %>').val('');
                $('#<%=txtCustomerGroupCodeCBP.ClientID %>').val('');
                $('#<%=txtCustomerGroupNameCBP.ClientID %>').val('');
            }
        });
    }
    //#endregion

    //#region Paging
    var pageCountAvailable = parseInt('<%=PageCount %>');
    $(function () {
        setPagingDetailItem(pageCountAvailable);
    });
    //#endregion

</script>
<div style="height: 440px; overflow-y: auto; overflow-x: hidden">
    <input type="hidden" id="hdnGLTransactionID" value="" runat="server" />
    <input type="hidden" id="hdnFilterExpressionQuickSearch" value="" runat="server" />
    <input type="hidden" id="hdnQuickText" value="" runat="server" />
    <table class="tblContentArea">
        <colgroup>
            <col style="width: 100%" />
        </colgroup>
        <tr>
            <td style="padding: 5px; vertical-align: top">
                <table class="tblEntryContent" style="width: 70%">
                    <colgroup>
                        <col style="width: 160px" />
                        <col />
                    </colgroup>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Nomor Voucher")%></label>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtJournalNo" ReadOnly="true" Width="100%" runat="server" />
                        </td>
                    </tr>
                </table>
                <div id="containerPopupEntryData" style="margin-top: 10px; display: none;">
                    <input type="hidden" id="hdnID" runat="server" value="" />
                    <div class="pageTitle">
                        <%=GetLabel("Entry")%></div>
                    <fieldset id="fsEntryPopup" style="margin: 0">
                        <table class="tblEntryDetail" style="width: 100%">
                            <colgroup>
                                <col style="width: 150px" />
                                <col />
                            </colgroup>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblLink" id="lblCustomerGroupCBP">
                                        <%=GetLabel("Customer Group")%></label>
                                </td>
                                <td>
                                    <input type="hidden" id="hdnCustomerGroupIDCBP" runat="server" value="" />
                                    <table style="width: 100%" cellpadding="0" cellspacing="0">
                                        <colgroup>
                                            <col style="width: 100px" />
                                            <col style="width: 3px" />
                                            <col />
                                        </colgroup>
                                        <tr>
                                            <td>
                                                <asp:TextBox runat="server" ID="txtCustomerGroupCodeCBP" Width="100%" />
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td>
                                                <asp:TextBox runat="server" ID="txtCustomerGroupNameCBP" ReadOnly="true" Width="100%" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblLink" id="lblBusinessPartnerCBP">
                                        <%=GetLabel("Business Partner")%></label>
                                </td>
                                <td>
                                    <input type="hidden" id="hdnBusinessPartnerIDCBP" runat="server" />
                                    <table style="width: 100%" cellpadding="0" cellspacing="0">
                                        <colgroup>
                                            <col style="width: 100px" />
                                            <col style="width: 3px" />
                                            <col />
                                        </colgroup>
                                        <tr>
                                            <td>
                                                <asp:TextBox runat="server" ID="txtBusinessPartnerCodeCBP" Width="100%" />
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td>
                                                <asp:TextBox runat="server" ID="txtBusinessPartnerNameCBP" ReadOnly="true" Width="100%" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <table>
                                        <tr>
                                            <td>
                                                <input type="button" id="btnEntryPopupSave" value='<%= GetLabel("Simpan")%>' />
                                            </td>
                                            <td>
                                                <input type="button" id="btnEntryPopupCancel" value='<%= GetLabel("Batal")%>' />
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
                    <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingViewPopup').show(); }"
                        EndCallback="function(s,e){ onCbpEntryPopupViewEndCallback(s); }" />
                    <PanelCollection>
                        <dx:PanelContent ID="PanelContent1" runat="server">
                            <asp:Panel runat="server" ID="pnlEntryPopupGrdView" Style="width: 100%; margin-left: auto;
                                margin-right: auto; position: relative; font-size: 0.95em;">
                                <asp:GridView ID="grdView" runat="server" CssClass="grdView notAllowSelect" AutoGenerateColumns="false"
                                    OnRowDataBound="grdView_RowDataBound" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                    <Columns>
                                        <asp:TemplateField HeaderStyle-Width="50px" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <img class="imgEdit imgLink" src='<%# ResolveUrl("~/Libs/Images/Button/edit.png")%>'
                                                    alt="" style="float: left; margin-left: 7px" />
                                                <input type="hidden" class="hdnID" value="<%#: Eval("TransactionDtID")%>" />
                                                <input type="hidden" class="hdnCustomerGroupIDCBP" value="<%#: Eval("CustomerGroupID")%>" />
                                                <input type="hidden" class="hdnCustomerGroupCodeCBP" value="<%#: Eval("CustomerGroupCode")%>" />
                                                <input type="hidden" class="hdnCustomerGroupNameCBP" value="<%#: Eval("CustomerGroupName")%>" />
                                                <input type="hidden" class="hdnBusinessPartnerIDCBP" value="<%#: Eval("BusinessPartnerID")%>" />
                                                <input type="hidden" class="hdnBusinessPartnerCodeCBP" value="<%#: Eval("BusinessPartnerCode")%>" />
                                                <input type="hidden" class="hdnBusinessPartnerNameCBP" value="<%#: Eval("BusinessPartnerName")%>" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderStyle-Width="130px" HeaderStyle-HorizontalAlign="Left">
                                            <HeaderTemplate>
                                                <%=GetLabel("COA")%></HeaderTemplate>
                                            <ItemTemplate>
                                                <div>
                                                    <%#:Eval("GLAccountNo")%>
                                                    <%#:Eval("GLAccountName")%></div>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderStyle-Width="130px" HeaderStyle-HorizontalAlign="Left" Visible="false">
                                            <HeaderTemplate>
                                                <%=GetLabel("Sub Akun")%></HeaderTemplate>
                                            <ItemTemplate>
                                                <div>
                                                    <%#:Eval("SubLedgerCode")%>
                                                    <%#:Eval("SubLedgerName")%></div>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="cfSegmentNo" HeaderText="Segment" HeaderStyle-Width="150px"
                                            ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" Visible="false" />
                                        <asp:BoundField DataField="cfCustomerGroupBusinessPartner" HeaderText="Customer Group / Business Partner"
                                            HeaderStyle-Width="220px" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" />
                                        <asp:BoundField DataField="Remarks" HeaderText="Keterangan" HeaderStyle-HorizontalAlign="Left" />
                                        <asp:TemplateField ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right"
                                            HeaderStyle-Width="120px">
                                            <HeaderTemplate>
                                                <%=GetLabel("Debet") %></HeaderTemplate>
                                            <ItemTemplate>
                                                <%#:Eval("Position").ToString() == "D" ? Eval("DebitAmount", "{0:N}") : "0"%>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right"
                                            HeaderStyle-Width="120px">
                                            <HeaderTemplate>
                                                <%=GetLabel("Kredit") %></HeaderTemplate>
                                            <ItemTemplate>
                                                <%#:Eval("Position").ToString() == "K" ? Eval("CreditAmount", "{0:N}") : "0"%>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="ReferenceNo" HeaderText="No. Referensi" ItemStyle-HorizontalAlign="Center"
                                            HeaderStyle-Width="130px" />
                                    </Columns>
                                    <EmptyDataTemplate>
                                        <%=GetLabel("Data Tidak Tersedia")%>
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
                        <div id="pagingPopup">
                        </div>
                    </div>
                </div>
            </td>
        </tr>
    </table>
    <div style="width: 100%; text-align: right">
        <input type="button" value='<%= GetLabel("Tutup")%>' onclick="pcRightPanelContent.Hide();" />
    </div>
</div>
