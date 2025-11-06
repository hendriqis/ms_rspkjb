<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="BankReconciliationEntryCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.Accounting.Program.BankReconciliationEntryCtl" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<script type="text/javascript" id="dxss_BankReconciliationEntryCtl">
    $(function () {
        setDatePicker('<%=txtJournalDateFrom.ClientID %>');
        $('#<%=txtJournalDateFrom.ClientID %>').datepicker('option', 'maxDate', '0');

        setDatePicker('<%=txtJournalDateTo.ClientID %>');
        $('#<%=txtJournalDateTo.ClientID %>').datepicker('option', 'maxDate', '0');
    });

    function onCbpProcessDetailBeginCallback() {
        showLoadingPanel();
        $('#tempContainerGrdDetail').append($('#containerGrdDetail'));

        $('.grdDetail tr:gt(0)').remove();
    }

    function onCbpProcessDetailEndCallback(s) {
        hideLoadingPanel();
        var param = s.cpResult.split('|');
        if (param[0] == 'refresh') {
            var pageCount = parseInt(param[1]);
            if (pageCount > 0)
                $('grdView tr:eq(1)').click();
        }
        $('grdView tr:eq(1)').click();
    }

    $('.grdView td.tdExpand').live('click', function () {
        $tr = $(this).parent();
        $trDetail = $(this).parent().next();
        if ($trDetail.attr('class') != 'trDetail') {
            $trCollapse = $('.trDetail');

            $(this).find('.imgExpand').attr('src', '<%= ResolveUrl("~/Libs/Images/down-arrow.png")%>');
            $newTr = $("<tr><td></td><td colspan='10'></td></tr>").attr('class', 'trDetail');
            $newTr.insertAfter($tr);
            $newTr.find('td').last().append($('#containerGrdDetail'));

            if ($trCollapse != null) {
                $trCollapse.prev().find('.imgExpand').attr('src', '<%= ResolveUrl("~/Libs/Images/right-arrow.png")%>');
                $trCollapse.remove();
            }

            $('.grdDetail tr:gt(0)').remove();
            $('#<%=hdnExpandID.ClientID %>').val($tr.find('.GLTransactionID').val());
            cbpViewDetail.PerformCallback('refresh');
        }
        else {
            $(this).find('.imgExpand').attr('src', '<%= ResolveUrl("~/Libs/Images/right-arrow.png")%>');
            $('#tempContainerGrdDetail').append($('#containerGrdDetail'));

            $('.grdDetail tr:gt(0)').remove();

            $trDetail.remove();
        }
    });

    $('#btnRefresh').live('click', function () {
        $('.chkSelectReconciliationDt').each(function () {
            $chk = $(this).find('input');
            if (!$chk.is(":disabled")) {
                $chk.prop('checked', false);
            }
        });
        cbpProcessDetail.PerformCallback('refresh');
    });

    $('#chkSelectAllReconciliationDt').die('change');
    $('#chkSelectAllReconciliationDt').live('change', function () {
        var isChecked = $(this).is(":checked");
        $('.chkSelectReconciliationDt').each(function () {
            $chk = $(this).find('input');
            if (!$chk.is(":disabled")) {
                $chk.prop('checked', isChecked);
            }
        });
    });

    function getCheckedReconciliationDt() {
        var lstSelectedBankReconciliationDt = $('#<%=hdnSelectedTransactionDtID.ClientID %>').val().split(',');
        $('.chkSelectReconciliationDt input').each(function () {
            if ($(this).is(':checked')) {
                var $tr = $(this).closest('tr');
                var key = $tr.find('.keyField').val();
                var idx = lstSelectedBankReconciliationDt.indexOf(key);
                if (idx < 0) {
                    lstSelectedBankReconciliationDt.push(key);
                }
                else {
                }
            }
            else {
                var key = $(this).closest('tr').find('.keyField').val();
                var idx = lstSelectedBankReconciliationDt.indexOf(key);
                if (idx > -1) {
                    lstSelectedBankReconciliationDt.splice(idx, 1);
                }
            }
        });
        $('#<%=hdnSelectedTransactionDtID.ClientID %>').val(lstSelectedBankReconciliationDt.join(','));
    }

    function onBeforeSaveRecord(errMessage) {
        getCheckedReconciliationDt();
        var hdIDCtl = $('#<%=hdnSelectedTransactionDtID.ClientID %>').val();
        if (hdIDCtl != "" && hdIDCtl != "0") {
            return true;
        } else {
            errMessage.text = 'Silahkan pilih detail jurnal terlebih dahulu';
            return false;
        }
    }

    function onCbpViewEndCallback(s) {
        hideLoadingPanel();
        getCheckedReconciliationDt();
    }
</script>
<div style="height: 400px; overflow-y: auto; overflow-x: hidden" id="containerPopup">
    <input type="hidden" id="hdnBankReconciliationIDEntryCtl" runat="server" value="" />
    <input type="hidden" id="hdnGLAccountIDEntryCtl" runat="server" value="" />
    <input type="hidden" id="hdnSelectedTransactionDtID" runat="server" value="" />
    <input type="hidden" id="hdnExpandID" runat="server" value="" />
    <table class="tblContentArea">
        <colgroup>
            <col style="width: 100%" />
        </colgroup>
        <tr>
            <td>
                <table>
                    <colgroup>
                        <col style="width: 120px" />
                        <col style="width: 150px" />
                        <col style="width: 30px" />
                        <col style="width: 180px" />
                        <col style="width: 150px" />
                    </colgroup>
                    <tr>
                        <td class="tdLabel" valign="middle">
                            <%=GetLabel("Periode Jurnal") %>
                        </td>
                        <td class="tdLabel" valign="middle">
                            <asp:TextBox ID="txtJournalDateFrom" Width="120px" CssClass="datepicker" runat="server" />
                        </td>
                        <td align="center" valign="middle">
                            <%=GetLabel("s/d")%>
                        </td>
                        <td class="tdLabel" valign="middle">
                            <asp:TextBox ID="txtJournalDateTo" Width="120px" CssClass="datepicker" runat="server" />
                        </td>
                        <td valign="middle">
                            <input type="button" id="btnRefresh" value="R e f r e s h" class="btnRefresh w3-button w3-blue w3-border-blue w3-round w3-small w3-padding-small" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td style="padding: 5px; vertical-align: top" colspan="2">
                <div>
                    <dxcp:ASPxCallbackPanel ID="cbpProcessDetail" runat="server" Width="100%" ClientInstanceName="cbpProcessDetail"
                        ShowLoadingPanel="false" OnCallback="cbpProcessDetail_Callback">
                        <ClientSideEvents BeginCallback="function(s,e){ onCbpProcessDetailBeginCallback(); }"
                            EndCallback="function(s,e){ onCbpProcessDetailEndCallback(s); }" />
                        <PanelCollection>
                            <dx:PanelContent ID="PanelContent1" runat="server">
                                <asp:Panel runat="server" ID="pnlEntryPopupGrdView" Style="width: 100%; margin-left: auto;
                                    margin-right: auto; position: relative; font-size: 0.95em;">
                                    <asp:ListView runat="server" ID="lvwView">
                                        <EmptyDataTemplate>
                                            <table id="tblView" runat="server" class="grdView notAllowSelect" cellspacing="0"
                                                rules="all">
                                                <tr>
                                                    <th style="width: 30px" align="center">
                                                        <input id="chkSelectAllReconciliationDt" type="checkbox" />
                                                    </th>
                                                    <th style="width: 20px">
                                                        &nbsp;
                                                    </th>
                                                    <th style="width: 100px" align="center">
                                                        <%=GetLabel("Tanggal Jurnal")%>
                                                    </th>
                                                    <th style="width: 150px">
                                                        <%=GetLabel("No Jurnal")%>
                                                    </th>
                                                    <th>
                                                        <%=GetLabel("COA")%>
                                                    </th>
                                                    <th style="width: 150px" align="right">
                                                        <%=GetLabel("Debit")%>
                                                    </th>
                                                    <th style="width: 150px" align="right">
                                                        <%=GetLabel("Kredit")%>
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
                                                    <th style="width: 30px" align="center">
                                                        <input id="chkSelectAllReconciliationDt" type="checkbox" />
                                                    </th>
                                                    <th style="width: 20px">
                                                        &nbsp;
                                                    </th>
                                                    <th style="width: 100px" align="center">
                                                        <%=GetLabel("Tanggal Jurnal")%>
                                                    </th>
                                                    <th style="width: 150px">
                                                        <%=GetLabel("No Jurnal")%>
                                                    </th>
                                                    <th>
                                                        <%=GetLabel("COA")%>
                                                    </th>
                                                    <th style="width: 150px" align="right">
                                                        <%=GetLabel("Debit")%>
                                                    </th>
                                                    <th style="width: 150px" align="right">
                                                        <%=GetLabel("Kredit")%>
                                                    </th>
                                                </tr>
                                                <tr runat="server" id="itemPlaceholder">
                                                </tr>
                                            </table>
                                        </LayoutTemplate>
                                        <ItemTemplate>
                                            <tr>
                                                <td align="center">
                                                    <asp:CheckBox ID="chkSelectReconciliationDt" runat="server" CssClass="chkSelectReconciliationDt" />
                                                    <input type="hidden" class="keyField" id="keyField" runat="server" value='<%#: Eval("TransactionDtID")%>' />
                                                    <input type="hidden" class="GLTransactionID" value='<%#: Eval("GLTransactionID")%>' />
                                                </td>
                                                <td align="center" class="tdExpand">
                                                    <img class="imgExpand imgLink" src='<%= ResolveUrl("~/Libs/Images/right-arrow.png")%>'
                                                        alt='' />
                                                </td>
                                                <td align="center">
                                                    <%#: Eval("cfJournalDateInString")%>
                                                </td>
                                                <td>
                                                    <b>
                                                        <%#: Eval("JournalNo")%></b>
                                                </td>
                                                <td>
                                                    <div style="font-size: 14px;">
                                                        <%#:Eval("GLAccountNo") %></div>
                                                    <div style="font-size: 12px;">
                                                        <%#:Eval("GLAccountName") %></div>
                                                </td>
                                                <td align="right">
                                                    <%#: Eval("DebitAmount", "{0:N2}")%>
                                                </td>
                                                <td align="right">
                                                    <%#: Eval("CreditAmount", "{0:N2}")%>
                                                </td>
                                            </tr>
                                        </ItemTemplate>
                                    </asp:ListView>
                                </asp:Panel>
                            </dx:PanelContent>
                        </PanelCollection>
                    </dxcp:ASPxCallbackPanel>
                </div>
                <div id="tempContainerGrdDetail" style="display: none">
                    <div id="containerGrdDetail" class="borderBox" style="width: 100%; padding: 10px 5px;">
                        <div style="height: 150px; overflow-y: auto; overflow-x: hidden">
                            <div id="containerGrdDt" class="containerGrdDt">
                                <dxcp:ASPxCallbackPanel ID="cbpViewDetail" runat="server" Width="100%" ClientInstanceName="cbpViewDetail"
                                    ShowLoadingPanel="false" OnCallback="cbpViewDetail_Callback">
                                    <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ hideLoadingPanel(); }" />
                                    <PanelCollection>
                                        <dx:PanelContent ID="PanelContent3" runat="server">
                                            <asp:Panel runat="server" ID="Panel2" Style="width: 100%; margin-left: auto; margin-right: auto">
                                                <asp:GridView ID="grdDetail" runat="server" CssClass="grdView notAllowSelect grdDetail"
                                                    AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                                    <Columns>
                                                        <asp:BoundField DataField="TransactionDtID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                                        <asp:TemplateField HeaderStyle-Width="180px" HeaderStyle-HorizontalAlign="Left">
                                                            <HeaderTemplate>
                                                                <%=GetLabel("COA")%></HeaderTemplate>
                                                            <ItemTemplate>
                                                                <div style="font-size: 14px;">
                                                                    <%#:Eval("GLAccountNo") %></div>
                                                                <div style="font-size: 12px;">
                                                                    <%#:Eval("GLAccountName") %></div>
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
                                                        <asp:TemplateField ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left"
                                                            HeaderStyle-Width="180px">
                                                            <HeaderTemplate>
                                                                <%=GetLabel("Segment") %></HeaderTemplate>
                                                            <ItemTemplate>
                                                                <div style="font-size: 12px;">
                                                                    DP:
                                                                    <%#:Eval("DepartmentID") %></div>
                                                                <div style="font-size: 12px;">
                                                                    SU:
                                                                    <%#:Eval("ServiceUnitName") %></div>
                                                                <div style="font-size: 12px;">
                                                                    RC:
                                                                    <%#:Eval("RevenueCostCenterName") %></div>
                                                                <div style="font-size: 12px;">
                                                                    CG:
                                                                    <%#:Eval("CustomerGroupName") %></div>
                                                                <div style="font-size: 12px;">
                                                                    BP:
                                                                    <%#:Eval("BusinessPartnerName") %></div>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="Remarks" HeaderText="Keterangan Transaksi" HeaderStyle-HorizontalAlign="Left" />
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
                                                        <%=GetLabel("No Data To Display")%>
                                                    </EmptyDataTemplate>
                                                </asp:GridView>
                                            </asp:Panel>
                                        </dx:PanelContent>
                                    </PanelCollection>
                                </dxcp:ASPxCallbackPanel>
                                <div class="imgLoadingGrdView" id="containerImgLoadingViewDetail2">
                                    <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                                </div>
                            </div>
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
