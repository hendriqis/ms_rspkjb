<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EpisodeTestOrderCtl1.ascx.cs"
    Inherits="QIS.Medinfras.Web.EMR.Program.EpisodeTestOrderCtl1" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<style type="text/css">
    .highlight
    {
        background-color: #FE5D15;
        color: White;
    }
</style>
<script type="text/javascript" id="dxss_Referralctl">
    $(function () {
        $('#<%=grdView.ClientID %> > tbody > tr:gt(0):not(.trEmpty)').live('click', function () {
            if ($(this).attr('class') != 'selected') {
                $('#<%=grdView.ClientID %> tr.selected').removeClass('selected');
                $('#<%=hdnSelectedID.ClientID %>').val($(this).closest('tr').find('.keyField').html());
                $(this).addClass('selected');
            }
        });
        $('#<%=grdView.ClientID %> tr:eq(1)').click();
    });

    function onBeforeProcess(param) {
        if (!getSelectedItem()) {
            return false;
        }
        else {
            return true;
        }
    }

    function onGetEntryPopupReturnValue() {
        var result = $('#<%=hdnSelectedItem.ClientID %>').val();
        return result;
    }

    $('.chkIsProcessItem input').live('change', function () {
        var $cell = $(this).closest("td");
        var $tr = $cell.closest('tr');
        var isChecked = $(this).is(":checked");
        if ($(this).is(':checked')) {
            $cell.addClass('highlight');
        }
        else {
            $cell.removeClass('highlight');
        }
    });

    $('#chkSelectAll').die('change');
    $('#chkSelectAll').live('change', function () {
        var isChecked = $(this).is(":checked");
        $('.chkIsProcessItem').each(function () {
            $(this).find('input').prop('checked', isChecked);
        });
    });

    function getSelectedItem() {
        var lstSelectedMember = [];
        var count = 0;
        $('.grdView .chkIsProcessItem input:checked').each(function () {
            var $tr = $(this).closest('tr');
            var id = $(this).closest('tr').find('.keyField').html();
            var itemName = $(this).closest('tr').find('.itemName').html();
            var listVisitID = $('#<%=hdnPopupVisitID.ClientID %>').val();

            if ($('#<%=hdnLinkedVisitID.ClientID %>').val() != "" && $('#<%=hdnLinkedVisitID.ClientID %>').val() != "()") {
                listVisitID += ", " + $('#<%=hdnLinkedVisitID.ClientID %>').val().replace("(", "").replace(")", "");
            }

            var selectedItem = "";
            var selectedAtrikel = "";
            var TanggalJamPemeriksaan = "";
            var TanggalJamPemeriksaanBefore = "";

            if ($('#<%=hdnIsResumeMedisCanInsertResultLab.ClientID %>').val() == "1") {
                if ($('#<%=hdnItemType.ClientID %>').val() == "X001^004") {
                    var filterExpression = "VisitID IN (" + listVisitID + ") AND ItemID = " + id + " ORDER BY TransactionDate, TransactionTime, FractionDisplayOrder ASC";
                    Methods.getListObject('GetvLaboratoryResultDtList', filterExpression, function (result) {
                        for (i = 0; i < result.length; i++) {
                            TanggalJamPemeriksaan = result[i].cfTransactionDateTime;

                            if (result[i].cfTransactionDateTime != TanggalJamPemeriksaanBefore) {
                                selectedItem += "\n";
                                selectedItem += "- " + itemName;
                                selectedItem += " Tanggal/Jam Transaksi " + TanggalJamPemeriksaan;
                            }

                            selectedItem += "\n" + "    *" + result[i].FractionName1 + " : " + result[i].cfTestResultValueNew + " " + result[i].MetricUnit;

                            TanggalJamPemeriksaanBefore = TanggalJamPemeriksaan;

                        }
                    });
                } else {
                    var filterExpression = "VisitID IN (" + listVisitID + ") AND ItemID = " + id + " ORDER BY ResultDate, ResultTime, ItemCode ASC";
                    Methods.getListObject('GetvImagingResultDt1List', filterExpression, function (result) {
                        for (i = 0; i < result.length; i++) {
                            TanggalJamPemeriksaan = result[i].cfResultDateTime;

                            if (result[i].cfResultDateTime != TanggalJamPemeriksaanBefore) {
                                selectedItem += "\n";
                                selectedItem += "- " + itemName;
                                selectedItem += " Tanggal/Jam Transaksi " + TanggalJamPemeriksaan;
                            }

//                            selectedItem += "\n" + "    *" + result[i].TestResult1;
                            selectedItem += "\n" + "    *" + result[i].TestResultInString;
                            selectedItem += "\n" + "    *" + result[i].TestResult2;
                            TanggalJamPemeriksaanBefore = TanggalJamPemeriksaan;

                        }
                    });
                }
            }

            lstSelectedMember.push(selectedItem);

            count += 1;
        });

        if (count == 0) {
            var messageBody = "Belum ada item yang dipilih.";
            displayMessageBox('Lookup : Pemeriksaan Penunjang', messageBody);
            return false;
        }
        else {
            $('#<%=hdnSelectedItem.ClientID %>').val(lstSelectedMember.join('|'));
            return true;
        }
    }

    function onCbpViewEndCallback(s) {
        hideLoadingPanel();
    }

    function onAfterProcessPopupEntry(param) {
        if (typeof onAfterLookUpDiagnosticTest == 'function') {
            onAfterLookUpDiagnosticTest(param);
        }
    }
</script>
<input type="hidden" id="hdnSelectedID" runat="server" />
<input type="hidden" id="hdnSelectedMember" runat="server" value="" />
<input type="hidden" id="hdnPopupVisitID" runat="server" value="" />
<input type="hidden" id="hdnLinkedVisitID" runat="server" value="" />
<input type="hidden" id="hdnItemType" runat="server" value="" />
<input type="hidden" id="hdnSelectedItem" runat="server" value="" />
<input type="hidden" id="hdnParamedicID" runat="server" value="" />
<input type="hidden" id="hdnIsResumeMedisCanInsertResultLab" runat="server" value="" />
<div style="height: 440px; overflow-y: auto">
    <table class="tblContentArea">
        <tr>
            <td style="padding: 5px; vertical-align: top">
                <table>
                    <colgroup>
                        <col width="115px" />
                        <col width="100px" />
                        <col width="115px" />
                        <col />
                    </colgroup>
                </table>
                <div>
                    <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
                        ShowLoadingPanel="false" OnCallback="cbpView_Callback">
                        <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ onCbpViewEndCallback(s); }" />
                        <PanelCollection>
                            <dx:PanelContent ID="PanelContent1" runat="server">
                                <asp:Panel runat="server" ID="pnlRujukan" Style="width: 100%; margin-left: auto;
                                    margin-right: auto; position: relative; font-size: 0.95em;">
                                    <asp:GridView ID="grdView" runat="server" CssClass="grdView" AutoGenerateColumns="false">
                                        <Columns>
                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                HeaderStyle-Width="40px">
                                                <HeaderTemplate>
                                                    <input id="chkSelectAll" type="checkbox" />
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chkIsProcessItem" runat="server" CssClass="chkIsProcessItem" AutoPostBack="false" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="ItemID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                            <asp:BoundField DataField="ItemName1" HeaderText="Pemeriksaan" HeaderStyle-CssClass="itemName"
                                                ItemStyle-CssClass="itemName" />
                                        </Columns>
                                    </asp:GridView>
                                    <div class="imgLoadingGrdView" id="containerImgLoadingReferrer">
                                        <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                                    </div>
                                </asp:Panel>
                            </dx:PanelContent>
                        </PanelCollection>
                    </dxcp:ASPxCallbackPanel>
                </div>
            </td>
        </tr>
    </table>
</div>
