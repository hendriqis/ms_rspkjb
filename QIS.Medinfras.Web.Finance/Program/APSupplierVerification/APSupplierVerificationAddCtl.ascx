<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="APSupplierVerificationAddCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.Finance.Program.APSupplierVerificationAddCtl" %>
<%@ Register Assembly="QIS.Medinfras.Web.CustomControl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"
    Namespace="QIS.Medinfras.Web.CustomControl" TagPrefix="qis" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<script type="text/javascript" id="dxss_apsupplierverificationaddctl">
    $(function () {
        setDatePicker('<%=txtDueDateFrom.ClientID %>');
        setDatePicker('<%=txtDueDateTo.ClientID %>');
    });

    function onBeforeSaveRecord(errMessage) {
        var count = 0;
        getCheckedMember();
        $('.chkIsSelectedCtl input').each(function () {
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

    function getCheckedMember() {
        var lstSelectedMember = [];
        var lstSelectedMemberSupplier = [];
        var lstSelectedAmount = [];
        var result = '';
        $('#<%=grdView.ClientID %> .chkIsSelectedCtl input').each(function () {
            if ($(this).is(':checked')) {
                var key = $(this).closest('tr').find('.keyField').html();
                var supplier = $(this).closest('tr').find('.BusinessPartnerID').val();
                var amount = $(this).closest('tr').find('.txtAmount').val();
                var idx = lstSelectedMember.indexOf(key);
                if (idx < 0) {
                    lstSelectedMember.push(key);
                    lstSelectedMemberSupplier.push(supplier);
                    lstSelectedAmount.push(amount);
                }
            }
        });

        $('#<%=hdnSelectedMember.ClientID %>').val(lstSelectedMember.join(','));
        $('#<%=hdnSelectedMemberSupplier.ClientID %>').val(lstSelectedMemberSupplier.join(','));
        $('#<%=hdnSelectedMemberAmount.ClientID %>').val(lstSelectedAmount.join(','));
    }

    $(function () {
    });

    function onCbpPopupEndCallback(s) {
        hideLoadingPanel();

        var param = s.cpResult.split('|');
        if (param[0] == 'refresh') {
            var pageCount = parseInt(param[1]);
            setPaging($("#pagingPopup"), pageCount, function (page) {
                cbpPopup.PerformCallback('changepage|' + page);
            });
        }
    }

    $('#chkSelectAllCtl').die('change');
    $('#chkSelectAllCtl').live('change', function () {
        var isChecked = $(this).is(":checked");
        $('.chkIsSelectedCtl input').each(function () {
            $(this).prop('checked', isChecked);
            $(this).change();
        });
    });

    $('#<%=grdView.ClientID %> .chkIsSelectedCtl input').die('change');
    $('#<%=grdView.ClientID %> .chkIsSelectedCtl input').live('change', function () {
        $tr = $(this).closest('tr');
        var id = $(this).closest('tr').find('.keyField').html();
        if ($(this).is(':checked')) {
            $tr.find('.txtAmount').removeAttr('readonly');
        }
        else {
            $tr.find('.txtAmount').attr('readonly', 'readonly');
        }
    });

    $('#btnRefresh').live('click', function () {
        $('#<%=hdnFilterExpressionQuickSearchCtl.ClientID %>').val(txtSearchViewCtl.GenerateFilterExpression());
        cbpPopup.PerformCallback('refresh');
    });


    function onRefreshGridCtl() {
        $('#<%=hdnFilterExpressionQuickSearchCtl.ClientID %>').val(txtSearchViewCtl.GenerateFilterExpression());
        getCheckedMember();
        cbpPopup.PerformCallback('refresh');
    }

    function onTxtSearchViewSearchCtlClick(s) {
        setTimeout(function () {
            s.SetBlur();
            onRefreshGridCtl();
        }, 0);
    }
</script>
<div style="padding: 10px;">
    <script id="tmplSelectedTestItem" type="text/x-jquery-tmpl">
    </script>
    <input type="hidden" id="hdnSelectedMember" runat="server" value="" />
    <input type="hidden" id="hdnTransactionID" runat="server" value="" />
    <input type="hidden" id="hdnSelectedMemberSupplier" runat="server" value="" />
    <input type="hidden" id="hdnSelectedMemberAmount" runat="server" value="" />
    <input type="hidden" id="hdnFilterExpressionQuickSearchCtl" value="" runat="server" />
    <table>
        <colgroup>
            <col style="width: 150px" />
            <col style="width: 400px" />
        </colgroup>
        <tr>
            <td class="tdLabel">
                <label class="lblNormal">
                    <%=GetLabel("Tanggal Jatuh Tempo")%></label>
            </td>
            <td>
                <table cellpadding="0" cellspacing="0">
                    <colgroup>
                        <col style="width: 145px" />
                        <col style="width: 3px" />
                        <col style="width: 145px" />
                    </colgroup>
                    <tr>
                        <td>
                            <asp:TextBox ID="txtDueDateFrom" Width="120px" CssClass="datepicker" runat="server" />
                        </td>
                        <td>
                            <%=GetLabel("s/d") %>
                        </td>
                        <td>
                            <asp:TextBox ID="txtDueDateTo" Width="120px" CssClass="datepicker" runat="server" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td class="tdLabel">
                <label class="lblNormal">
                    <%=GetLabel("Quick Search")%></label>
            </td>
            <td>
                <qis:qisintellisensetextbox runat="server" clientinstancename="txtSearchViewCtl"
                    id="txtSearchViewCtl" width="300px" watermark="Search">
                                    <ClientSideEvents SearchClick="function(s){ onTxtSearchViewSearchCtlClick(s); }" />
                                    <IntellisenseHints>
                                        <qis:QISIntellisenseHint Text="PurchaseInvoiceNo" FieldName="PurchaseInvoiceNo" />
                                        <qis:QISIntellisenseHint Text="BusinessPartnerName" FieldName="BusinessPartnerName" />
                                        <qis:QISIntellisenseHint Text="SupplierInvoiceDate(dd-mm-yyyy)" FieldName="SupplierInvoiceDate" />
                                        <qis:QISIntellisenseHint Text="DueDate(dd-mm-yyyy)" FieldName="DueDate" />
                                        <qis:QISIntellisenseHint Text="ProductLineName" FieldName="ProductLineName" />
                                    </IntellisenseHints>
                                </qis:qisintellisensetextbox>
            </td>
        </tr>
        <tr>
            <td>
            </td>
            <td>
                <div id="divRefresh" runat="server" style="float: left; margin-top: 0px;">
                    <input type="button" id="btnRefresh" value="R e f r e s h" class="btnRefresh w3-button w3-blue w3-border w3-border-blue w3-round-large" />
                </div>
            </td>
        </tr>
    </table>
    <table style="width: 100%">
        <colgroup>
            <col style="width: 100%" />
        </colgroup>
        <tr>
            <td style="padding: 5px; vertical-align: top">
                <div>
                    <dxcp:ASPxCallbackPanel ID="cbpPopup" runat="server" Width="100%" ClientInstanceName="cbpPopup"
                        ShowLoadingPanel="false" OnCallback="cbpPopup_Callback">
                        <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel();}" EndCallback="function(s,e){ onCbpPopupEndCallback(s); }" />
                        <PanelCollection>
                            <dx:PanelContent ID="PanelContent1" runat="server">
                                <asp:Panel runat="server" ID="pnlView" Style="width: 100%; margin-left: auto; margin-right: auto;
                                    position: relative; font-size: 0.95em;">
                                    <asp:GridView ID="grdView" runat="server" CssClass="grdView grdNormal notAllowSelect"
                                        AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty"
                                        OnRowDataBound="grdView_RowDataBound">
                                        <Columns>
                                            <asp:BoundField DataField="PurchaseInvoiceID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                            <asp:TemplateField HeaderStyle-Width="40px" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                <HeaderTemplate>
                                                    <input id="chkSelectAllCtl" type="checkbox" />
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chkIsSelectedCtl" runat="server" CssClass="chkIsSelectedCtl" />
                                                    <input type="hidden" class="BusinessPartnerID" id="BusinessPartnerID" runat="server"
                                                        value='<%#: Eval("BusinessPartnerID")%>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-Width="200px" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Center">
                                                <HeaderTemplate>
                                                    <div style="text-align: left; padding-left: 3px">
                                                        <%=GetLabel("No. Tukar Faktur")%>
                                                    </div>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <div>
                                                        <label class="lblPurchaseInvoiceNo">
                                                            <%#: Eval("PurchaseInvoiceNo") %></label></div>
                                                    <div style="font-size: smaller; max-width: 200px;">
                                                        No. Faktur:
                                                        <%#: Eval("PurchaseReceiveReferenceNo") %></div>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="BusinessPartnerName" HeaderText="Supplier" />
                                            <asp:BoundField DataField="PInvoiceDateInString" HeaderText="Tgl. Tukar Faktur" ItemStyle-HorizontalAlign="Center"
                                                HeaderStyle-HorizontalAlign="Center" />
                                            <asp:BoundField DataField="DueDateInString" HeaderText="Tgl. Jatuh Tempo" ItemStyle-HorizontalAlign="Center"
                                                HeaderStyle-HorizontalAlign="Center" />
                                            <asp:BoundField DataField="TotalNetTransactionAmountInString" HeaderText="Total Hutang"
                                                ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right" />
                                            <asp:BoundField DataField="cfPaymentAmount" HeaderText="Terbayar" ItemStyle-HorizontalAlign="Right"
                                                HeaderStyle-HorizontalAlign="Right" />
                                            <asp:BoundField DataField="cfSisaHutang" HeaderText="Sisa Hutang" ItemStyle-HorizontalAlign="Right"
                                                HeaderStyle-HorizontalAlign="Right" />
                                            <asp:TemplateField HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                <HeaderTemplate>
                                                    <div style="text-align: right; padding-left: 3px">
                                                        <%=GetLabel("Pembayaran")%>
                                                    </div>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtAmount" ValidationGroup="mpDrugsQuickPicks" class="txtAmount number min"
                                                        min="0" value="0" ReadOnly="true" Style="width: 100px" runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <EmptyDataTemplate>
                                            <%=GetLabel("No Data To Display")%>
                                        </EmptyDataTemplate>
                                    </asp:GridView>
                                </asp:Panel>
                            </dx:PanelContent>
                        </PanelCollection>
                    </dxcp:ASPxCallbackPanel>
                </div>
            </td>
        </tr>
    </table>
    <div class="imgLoadingGrdView" id="containerImgLoadingView">
        <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
    </div>
</div>
