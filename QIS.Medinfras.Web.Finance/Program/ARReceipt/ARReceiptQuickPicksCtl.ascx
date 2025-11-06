<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ARReceiptQuickPicksCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.Finance.Program.ARReceiptQuickPicksCtl" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<script type="text/javascript" id="dxss_ARReceiptQuickPicksCtl">

    $(function () {
        setDatePicker('<%=txtPeriodFrom.ClientID %>');
        $('#<%=txtPeriodFrom.ClientID %>').datepicker('option', 'maxDate', '0');

        setDatePicker('<%=txtPeriodTo.ClientID %>');
        $('#<%=txtPeriodTo.ClientID %>').datepicker('option', 'maxDate', '0');
    });

    //#region Business Partner
    function getBusinessPartnerFilter() {
        var filterExpression = "GCBusinessPartnerType = 'X017^002' AND IsDeleted = 0 AND IsActive = 1";
        return filterExpression;
    }

    $('#lblBusinessPartnerID.lblLink').live('click', function () {
        openSearchDialog('businesspartners', getBusinessPartnerFilter(), function (value) {
            $('#<%=txtBusinessPartnerCode.ClientID %>').val(value);
            ontxtBusinessPartnerCodeChanged(value);
        });
    });

    $('#<%=txtBusinessPartnerCode.ClientID %>').live('change', function () {
        ontxtBusinessPartnerCodeChanged($(this).val());
    });

    function ontxtBusinessPartnerCodeChanged(value) {
        var filterExpression = getBusinessPartnerFilter() + " AND BusinessPartnerCode = '" + value + "'";
        Methods.getObject('GetBusinessPartnersList', filterExpression, function (result) {
            if (result != null) {
                $('#<%=hdnBusinessPartnerID.ClientID %>').val(result.BusinessPartnerID);
                $('#<%=txtBusinessPartnerName.ClientID %>').val(result.BusinessPartnerName);
            }
            else {
                $('#<%=hdnBusinessPartnerID.ClientID %>').val('');
                $('#<%=txtBusinessPartnerCode.ClientID %>').val('');
                $('#<%=txtBusinessPartnerName.ClientID %>').val('');
            }
        });
    }
    //#endregion

    $('#btnRefresh').live('click', function () {
        getCheckedARInvoice();
        cbpProcessDetail.PerformCallback('refresh');
    });

    function getCheckedARInvoice() {
        var lstSelectedARInvoice = $('#<%=hdnSelectedARInvoiceID.ClientID %>').val().split(',');
        $('.chkARInvoice input').each(function () {
            if ($(this).is(':checked')) {
                var $tr = $(this).closest('tr');
                var key = $tr.find('.keyField').val();
                var idx = lstSelectedARInvoice.indexOf(key);
                if (idx < 0) {
                    lstSelectedARInvoice.push(key);
                }
            }
            else {
                var key = $(this).closest('tr').find('.keyField').val();
                var idx = lstSelectedARInvoice.indexOf(key);
                if (idx > -1) {
                    lstSelectedARInvoice.splice(idx, 1);
                }
            }
        });
        $('#<%=hdnSelectedARInvoiceID.ClientID %>').val(lstSelectedARInvoice.join(','));
    }

    $('#chkSelectAll').die('change');
    $('#chkSelectAll').live('change', function () {
        var isChecked = $(this).is(":checked");
        $('.chkARInvoice').each(function () {
            $chk = $(this).find('input');
            if (!$chk.is(":disabled")) {
                $chk.prop('checked', isChecked);
            }
        });
    });

    function onBeforeSaveRecord(errMessage) {
        getCheckedARInvoice();
        if ($('#<%=hdnSelectedARInvoiceID.ClientID %>').val() == '') {
            errMessage.text = 'Please Select AR Invoice First';
            return false;
        }
        return true;
    }
</script>
<div style="height: 400px; overflow-y: auto; overflow-x: hidden" id="containerPopup">
    <input type="hidden" id="hdnARReceiptIDCtl" value="" runat="server" />
    <input type="hidden" id="hdnSelectedARInvoiceID" value="" runat="server" />
    <table class="tblContentArea">
        <colgroup>
            <col style="width: 100%" />
        </colgroup>
        <tr>
            <td>
                <table>
                    <colgroup>
                        <col style="width: 150px" />
                        <col />
                    </colgroup>
                    <tr>
                        <td style="width: 150px;">
                            <label class="lblNormal">
                                <%=GetLabel("Tanggal Invoice") %></label>
                        </td>
                        <td>
                            <table>
                                <colgroup>
                                    <col style="width: 150px;" />
                                    <col style="width: 30px;" />
                                    <col style="width: 150px;" />
                                </colgroup>
                                <tr>
                                    <td>
                                        <asp:TextBox runat="server" ID="txtPeriodFrom" CssClass="datepicker" Width="120px" />
                                    </td>
                                    <td align="center">
                                        <%=GetLabel("s/d") %>
                                    </td>
                                    <td>
                                        <asp:TextBox runat="server" ID="txtPeriodTo" CssClass="datepicker" Width="120px" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                        <tr>
                            <td class="tdLabel">
                                <label id="lblBusinessPartnerID" class="lblLink">
                                    <%=GetLabel("Instansi")%></label>
                            </td>
                            <td>
                                <input type="hidden" id="hdnBusinessPartnerID" value="" runat="server" />
                                <table style="width: 100%" cellpadding="0" cellspacing="0">
                                    <colgroup>
                                        <col style="width: 30%" />
                                        <col style="width: 3px" />
                                        <col />
                                    </colgroup>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtBusinessPartnerCode" Width="100%" runat="server" />
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtBusinessPartnerName" Width="100%" runat="server" ReadOnly="true" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    <tr>
                        <td>
                        </td>
                        <td>
                            <input type="button" id="btnRefresh" value="R e f r e s h" class="btnRefresh w3-button w3-blue w3-border w3-border-blue w3-round-large" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td style="padding: 5px; vertical-align: top">
                <dxcp:ASPxCallbackPanel ID="cbpProcessDetail" runat="server" Width="100%" ClientInstanceName="cbpProcessDetail"
                    ShowLoadingPanel="false" OnCallback="cbpProcessDetail_Callback">
                    <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ hideLoadingPanel(); }" />
                    <PanelCollection>
                        <dx:PanelContent ID="PanelContent1" runat="server">
                            <asp:Panel runat="server" ID="pnlEntryPopupGrdView" Style="width: 100%; margin-left: auto;
                                margin-right: auto; position: relative; font-size: 0.95em;">
                                <asp:ListView runat="server" ID="lvwView">
                                    <EmptyDataTemplate>
                                        <table id="tblView" runat="server" class="grdView notAllowSelect" cellspacing="0"
                                            rules="all">
                                            <tr>
                                                <th style="width: 40px" align="center">
                                                    <input id="chkSelectAll" type="checkbox" />
                                                </th>
                                                <th style="width: 120px">
                                                    <%=GetLabel("Jenis Invoice")%>
                                                </th>
                                                <th style="width: 150px">
                                                    <%=GetLabel("No Invoice")%>
                                                </th>
                                                <th style="width: 120px" align="center">
                                                    <%=GetLabel("Tanggal Invoice")%>
                                                </th>
                                                <th>
                                                    <%=GetLabel("Informasi Invoice")%>
                                                </th>
                                                <th style="width: 150px" align="right">
                                                    <%=GetLabel("Nilai Klaim Invoice")%>
                                                </th>
                                            </tr>
                                            <tr class="trEmpty">
                                                <td colspan="10">
                                                    <%=GetLabel("No Data To Display")%>
                                                </td>
                                            </tr>
                                        </table>
                                    </EmptyDataTemplate>
                                    <LayoutTemplate>
                                        <table id="tblView" runat="server" class="grdView notAllowSelect" cellspacing="0"
                                            rules="all">
                                            <tr>
                                                <th style="width: 40px" align="center">
                                                    <input id="chkSelectAll" type="checkbox" />
                                                </th>
                                                <th style="width: 120px">
                                                    <%=GetLabel("Jenis Invoice")%>
                                                </th>
                                                <th style="width: 150px">
                                                    <%=GetLabel("No Invoice")%>
                                                </th>
                                                <th style="width: 120px" align="center">
                                                    <%=GetLabel("Tanggal Invoice")%>
                                                </th>
                                                <th>
                                                    <%=GetLabel("Informasi Invoice")%>
                                                </th>
                                                <th style="width: 150px" align="right">
                                                    <%=GetLabel("Nilai Klaim Invoice")%>
                                                </th>
                                            </tr>
                                            <tr runat="server" id="itemPlaceholder">
                                            </tr>
                                        </table>
                                    </LayoutTemplate>
                                    <ItemTemplate>
                                        <tr>
                                            <td align="center">
                                                <asp:CheckBox ID="chkARInvoice" runat="server" CssClass="chkARInvoice" />
                                                <input type="hidden" class="keyField" id="keyField" runat="server" value='<%#: Eval("ARInvoiceID")%>' />
                                            </td>
                                            <td>
                                                <%#: Eval("cfCaptionARType")%>
                                            </td>
                                            <td>
                                                <%#: Eval("ARInvoiceNo")%>
                                            </td>
                                            <td align="center">
                                                <%#: Eval("ARInvoiceDateInString")%>
                                            </td>
                                            <td>
                                                <%#: Eval("cfARInfo")%>
                                            </td>
                                            <td align="right">
                                                <%#: Eval("TotalClaimedAmountInString")%>
                                            </td>
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
    <div style="width: 100%; text-align: right">
        <input type="button" value='<%= GetLabel("Close")%>' onclick="pcRightPanelContent.Hide();" />
    </div>
</div>
