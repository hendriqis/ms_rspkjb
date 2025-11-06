<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AmortizationEntryCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.Finance.Program.AmortizationEntryCtl" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<script type="text/javascript" id="dxss_AmortizationEntryCtl">
    $(function () {
        hideLoadingPanel();

        $('#<%=txtClaimedAmortizationAmount.ClientID %>').trigger('changeValue');
        $('#<%=txtTotalAmortizationAmount.ClientID %>').trigger('changeValue');

        $('.txtAmortizationAmount').each(function () {
            $(this).trigger('changeValue');
        });
    });

    $('.txtAmortizationAmount').live('change', function () {
        $(this).trigger('changeValue');
        calculateTotal();
    });

    function calculateTotal() {
        var lstSelectedAmortizationAmount = 0;
        $('.chkIsSelectedDetail input').each(function () {
            if ($(this).is(':checked')) {
                $tr = $(this).closest('tr');
                var amortizationAmount = parseFloat($tr.find('.txtAmortizationAmount').attr('hiddenVal'));
                lstSelectedAmortizationAmount += amortizationAmount;
            }
        });
        $('#<%=txtTotalAmortizationAmount.ClientID %>').val(lstSelectedAmortizationAmount).trigger('changeValue');
    }

    function onBeforeSaveRecord() {
        $('#<%=hdnSelectedID.ClientID %>').val('');
        $('#<%=hdnSelectedAmortizationAmount.ClientID %>').val('');
        getCheckedMember();
        calculateTotal();

        var claimedAmount = parseFloat($('#<%=txtClaimedAmortizationAmount.ClientID %>').attr('hiddenVal'));
        var totalAmortizationAmount = parseFloat($('#<%=txtTotalAmortizationAmount.ClientID %>').attr('hiddenVal'));

        if (totalAmortizationAmount <= claimedAmount) {
            return true;
        } else {
            showToast('Save Failed', 'Error Message : Nilai Total Amortisasi tidak bisa melebihi Nilai Klaim');
            return false;
        }
    }

    function getCheckedMember() {
        var lstSelectedID = $('#<%=hdnSelectedID.ClientID %>').val().split(',');
        var lstSelectedAmortizationAmount = $('#<%=hdnSelectedAmortizationAmount.ClientID %>').val().split(',');
        $('.chkIsSelectedDetail input').each(function () {
            if ($(this).is(':checked')) {
                $tr = $(this).closest('tr');
                var oID = $tr.find('.keyField').val();
                var oAmortizationAmount = parseFloat($tr.find('.txtAmortizationAmount').attr('hiddenVal'));
                var idx = lstSelectedID.indexOf(oID);
                if (idx < 0) {
                    lstSelectedID.push(oID);
                    lstSelectedAmortizationAmount.push(oAmortizationAmount);
                }
                else {
                    lstSelectedAmortizationAmount[idx] = oAmortizationAmount;
                }
            }
            else {
                var key = $(this).closest('tr').find('.keyField').html().trim();
                var idx = lstSelectedID.indexOf(key);
                if (idx > -1) {
                    lstSelectedID.splice(idx, 1);
                    lstSelectedAmortizationAmount.splice(idx, 1);
                }
            }
        });
        $('#<%=hdnSelectedID.ClientID %>').val(lstSelectedID.join(','));
        $('#<%=hdnSelectedAmortizationAmount.ClientID %>').val(lstSelectedAmortizationAmount.join(','));
    }

    function onCbpProcessDetailEndCallback(s) {
        hideLoadingPanel();

        $('.txtAmortizationAmount').each(function () {
            $(this).trigger('changeValue');
        });
    }

</script>
<div style="height: 450px; overflow-y: auto">
    <input type="hidden" id="hdnARInvoiceDtID" runat="server" value="" />
    <input type="hidden" id="hdnSelectedID" runat="server" value="" />
    <input type="hidden" id="hdnSelectedAmortizationAmount" runat="server" value="" />
    <table class="tblContentArea">
        <tr>
            <td style="padding: 5px; vertical-align: top; width: 100%">
                <table class="tblEntryContent">
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Transaksi Non Operasional")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtTransactionNonOperationalType" Style="text-align: left" ReadOnly="true"
                                Width="250px" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Cost Revenue Sharing")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtCostRevenueSharing" Style="text-align: left" ReadOnly="true"
                                Width="250px" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Periode(bulan) / Tgl Mulai")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtAmortizationPeriodInMonth" Style="text-align: center; width: 50px"
                                ReadOnly="true" runat="server" />
                            <asp:TextBox ID="txtAmortizationFirstDate" Style="text-align: center; width: 100px"
                                ReadOnly="true" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Nilai Klaim")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtClaimedAmortizationAmount" CssClass="txtCurrency" ReadOnly="true" Width="150px"
                                Style="text-align: right" ForeColor="Blue" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Total Amortisasi")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtTotalAmortizationAmount" CssClass="txtCurrency" ReadOnly="true"
                                Width="150px" Style="text-align: right" ForeColor="Green" runat="server" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <div style="position: relative;" id="divView">
                    <dxcp:ASPxCallbackPanel ID="cbpProcessDetail" runat="server" Width="100%" ClientInstanceName="cbpProcessDetail"
                        ShowLoadingPanel="false" OnCallback="cbpProcessDetail_Callback">
                        <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ onCbpProcessDetailEndCallback(s); }" />
                        <PanelCollection>
                            <dx:PanelContent ID="PanelContent1" runat="server">
                                <asp:Panel runat="server" ID="panel1" Style="width: 100%; margin-left: auto; margin-right: auto;
                                    position: relative; font-size: 0.95em;">
                                    <asp:ListView runat="server" ID="lvwView" OnItemDataBound="lvwView_ItemDataBound">
                                        <EmptyDataTemplate>
                                            <table id="tblView" runat="server" class="grdView notAllowSelect" cellspacing="0"
                                                rules="all">
                                                <tr>
                                                    <th style="width: 40px" align="center" id="thSelectAll">
                                                    </th>
                                                    <th align="center" style="width: 150px">
                                                        <%=GetLabel("Tgl Amortisasi")%>
                                                    </th>
                                                    <th align="right">
                                                        <%=GetLabel("Nilai Amortisasi")%>
                                                    </th>
                                                </tr>
                                                <tr class="trEmpty">
                                                    <td colspan="5">
                                                        <%=GetLabel("No Data To Display")%>
                                                    </td>
                                                </tr>
                                            </table>
                                        </EmptyDataTemplate>
                                        <LayoutTemplate>
                                            <table id="tblView" runat="server" class="grdView notAllowSelect" cellspacing="0"
                                                rules="all">
                                                <tr>
                                                    <th style="width: 40px" align="center" id="thSelectAll">
                                                    </th>
                                                    <th align="center" style="width: 150px">
                                                        <%=GetLabel("Tgl Amortisasi")%>
                                                    </th>
                                                    <th align="right">
                                                        <%=GetLabel("Nilai Amortisasi")%>
                                                    </th>
                                                </tr>
                                                <tr runat="server" id="itemPlaceholder">
                                                </tr>
                                            </table>
                                        </LayoutTemplate>
                                        <ItemTemplate>
                                            <tr>
                                                <td align="center">
                                                    <asp:CheckBox ID="chkIsSelectedDetail" runat="server" Enabled="false" CssClass="chkIsSelectedDetail" />
                                                    <input type="hidden" class="keyField" id="keyField" runat="server" value='<%#: Eval("ID")%>' />
                                                    <input type="hidden" class="ARInvoiceDtID" id="ARInvoiceDtID" value="<%#: Eval("ARInvoiceDtID")%>" />
                                                    <input type="hidden" class="AmortizationDate" id="AmortizationDate" value="<%#: Eval("AmortizationDate")%>" />
                                                    <input type="hidden" class="AmortizationAmount" id="AmortizationAmount" value="<%#: Eval("AmortizationAmount")%>" />
                                                </td>
                                                <td align="center">
                                                    <%#: Eval("cfAmortizationDateInString")%>
                                                </td>
                                                <td align="right">
                                                    <asp:TextBox ID="txtAmortizationAmount" Width="80%" runat="server" CssClass="txtAmortizationAmount txtCurrency" />
                                                </td>
                                            </tr>
                                        </ItemTemplate>
                                    </asp:ListView>
                                </asp:Panel>
                            </dx:PanelContent>
                        </PanelCollection>
                    </dxcp:ASPxCallbackPanel>
                    <div class="imgLoadingGrdView" id="divLoading">
                        <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                    </div>
                </div>
            </td>
        </tr>
    </table>
</div>
