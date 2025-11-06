<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RevenueSharingSummaryEntryAdjTransCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.Finance.Program.RevenueSharingSummaryEntryAdjTransCtl" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<script type="text/javascript" id="dxss_RevenueSharingSummaryEntryAdjTransCtl">

    $(function () {
        setDatePicker('<%=txtPeriodFrom.ClientID %>');
        $('#<%=txtPeriodFrom.ClientID %>').datepicker('option', 'maxDate', '0');

        setDatePicker('<%=txtPeriodTo.ClientID %>');
        $('#<%=txtPeriodTo.ClientID %>').datepicker('option', 'maxDate', '0');
    });

    $('#btnRefresh').live('click', function () {
        getCheckedRSAdjTransaction();
        cbpProcessDetail.PerformCallback('refresh');
    });

    function getCheckedRSAdjTransaction() {
        var lstSelectedRSAdjTransaction = $('#<%=hdnSelectedDtID.ClientID %>').val().split(',');
        $('.chkRSTransaction input').each(function () {
            if ($(this).is(':checked')) {
                var $tr = $(this).closest('tr');
                var key = $tr.find('.keyField').val();
                var idx = lstSelectedRSAdjTransaction.indexOf(key);
                if (idx < 0) {
                    lstSelectedRSAdjTransaction.push(key);
                }
            }
            else {
                var key = $(this).closest('tr').find('.keyField').val();
                var idx = lstSelectedRSAdjTransaction.indexOf(key);
                if (idx > -1) {
                    lstSelectedRSAdjTransaction.splice(idx, 1);
                }
            }
        });
        $('#<%=hdnSelectedDtID.ClientID %>').val(lstSelectedRSAdjTransaction.join(','));
    }

    $('#chkSelectAll').die('change');
    $('#chkSelectAll').live('change', function () {
        var isChecked = $(this).is(":checked");
        $('.chkRSTransaction').each(function () {
            $chk = $(this).find('input');
            if (!$chk.is(":disabled")) {
                $chk.prop('checked', isChecked);
            }
        });
    });

    function onBeforeSaveRecord(errMessage) {
        getCheckedRSAdjTransaction();
        if ($('#<%=hdnSelectedDtID.ClientID %>').val() == '') {
            errMessage.text = 'Please Select Transaction First';
            return false;
        } else {
            return true;
        }
    }

    function onCbpViewEndCallback(s) {
        hideLoadingPanel();
        getCheckedRSAdjTransaction();
    }
</script>
<div style="height: 400px; overflow-y: auto; overflow-x: hidden" id="containerPopup">
    <input type="hidden" id="hdnRSSummaryIDCtl" runat="server" value="" />
    <input type="hidden" id="hdnRSSummaryMaxAmountCtl" runat="server" value="" />
    <input type="hidden" id="hdnIsUsedAdjustmentAmountBRUTO" runat="server" value="" />
    <input type="hidden" id="hdnSelectedDtID" runat="server" value="" />
    <table class="tblContentArea">
        <colgroup>
            <col style="width: 100%" />
        </colgroup>
        <tr>
            <td>
                <table>
                    <colgroup>
                        <col style="width: 200px" />
                        <col style="width: 150px" />
                        <col style="width: 50px" />
                        <col style="width: 150px" />
                        <col />
                    </colgroup>
                    <tr>
                        <td style="width: 150px;">
                            <label class="lblNormal">
                                <%=GetLabel("Periode Transaksi Penyesuaian") %></label>
                        </td>
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
                                                <th style="width: 150px" align="left">
                                                    <%=GetLabel("No Penyesuaian") %>
                                                </th>
                                                <th style="width: 120px" align="center">
                                                    <%=GetLabel("Tanggal Penyesuaian") %>
                                                </th>
                                                <th style="width: 150px" align="left">
                                                    <%=GetLabel("Penambahan / Pengurangan") %>
                                                </th>
                                                <th style="width: 150px" align="left">
                                                    <%=GetLabel("Adjustment Type") %>
                                                </th>
                                                <th style="width: 150px" align="right">
                                                    <%=GetLabel("Ajustment Amount Bruto") %>
                                                </th>
                                                <th style="width: 150px" align="right">
                                                    <%=GetLabel("Ajustment Amount") %>
                                                </th>
                                            </tr>
                                            <tr class="trEmpty">
                                                <td colspan="20">
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
                                                <th style="width: 150px" align="left">
                                                    <%=GetLabel("No Penyesuaian") %>
                                                </th>
                                                <th style="width: 120px" align="center">
                                                    <%=GetLabel("Tanggal Penyesuaian") %>
                                                </th>
                                                <th style="width: 150px" align="left">
                                                    <%=GetLabel("Penambahan / Pengurangan") %>
                                                </th>
                                                <th style="width: 150px" align="left">
                                                    <%=GetLabel("Adjustment Type") %>
                                                </th>
                                                <th style="width: 150px" align="right">
                                                    <%=GetLabel("Ajustment Amount Bruto") %>
                                                </th>
                                                <th style="width: 150px" align="right">
                                                    <%=GetLabel("Ajustment Amount") %>
                                                </th>
                                            </tr>
                                            <tr runat="server" id="itemPlaceholder">
                                            </tr>
                                        </table>
                                    </LayoutTemplate>
                                    <ItemTemplate>
                                        <tr>
                                            <td align="center">
                                                <asp:CheckBox ID="chkRSTransaction" runat="server" CssClass="chkRSTransaction" />
                                                <input type="hidden" class="keyField" id="keyField" runat="server" value='<%#: Eval("ID")%>' />
                                            </td>
                                            <td align="left">
                                                <label class="lblNormal">
                                                    <%#:Eval("RSAdjustmentNo") %></label>
                                            </td>
                                            <td align="center">
                                                <%#:Eval("cfRSAdjustmentDateInString")%>
                                            </td>
                                            <td align="left">
                                                <%#:Eval("RSAdjustmentGroup")%>
                                            </td>
                                            <td align="left">
                                                <%#:Eval("RSAdjustmentType")%>
                                            </td>
                                            <td align="right">
                                                <%#:Eval("AdjustmentAmountBRUTO", "{0:N2}")%>
                                            </td>
                                            <td align="right">
                                                <%#:Eval("AdjustmentAmount", "{0:N2}")%>
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                </asp:ListView>
                            </asp:Panel>
                        </dx:PanelContent>
                    </PanelCollection>
                </dxcp:ASPxCallbackPanel>
                <div class="containerPaging" style="display: none">
                    <div class="wrapperPaging">
                        <div id="pagingPopup">
                        </div>
                    </div>
                </div>
            </td>
        </tr>
    </table>
    <div style="width: 100%; text-align: right">
        <input type="button" value='<%= GetLabel("Close")%>' onclick="pcRightPanelContent.Hide();" />
    </div>
</div>
