<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="JournalAcrualPicksCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.Accounting.Program.JournalAcrualPicksCtl" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<script type="text/javascript" id="dxss_JournalAcrualPicksCtl">

    //#region Journal No
    $('#lblJournalNoCtlAcc.lblLink').die('click');
    $('#lblJournalNoCtlAcc.lblLink').live('click', function () {
        var filterExpression = "1 = 1";
        openSearchDialog('gltransactionhdcopyacc', filterExpression, function (value) {
            $('#<%=txtJournalNoSelected.ClientID %>').val(value);
            ontxtJournalNoSelectedChanged(value);
        });
    });

    $('#<%=txtJournalNoSelected.ClientID %>').die('change');
    $('#<%=txtJournalNoSelected.ClientID %>').live('change', function () {
        ontxtJournalNoSelectedChanged($(this).val());
    });

    function ontxtJournalNoSelectedChanged(value) {
        if (value != null && value != "") {
            var filterExpression = "JournalNo = '" + value + "' AND GCTransactionStatus != 'X121^999' AND GCItemDetailStatus != 'X121^999' AND IsDeleted = 0";
            Methods.getObject('GetvGLTransactionDtList', filterExpression, function (result) {
                if (result != null) {
                    cbpEntryPopupView.PerformCallback('refresh');
                }
                else {
                    alert("Nomor Jurnal tidak ditemukan.");
                    $('#<%=txtJournalNoSelected.ClientID %>').val("");
                    cbpEntryPopupView.PerformCallback('refresh');
                }
            });
        } else {
            alert("Harap pilih nomor jurnal untuk salin terlebih dahulu.");
            cbpEntryPopupView.PerformCallback('refresh');
        }
    }
    //#endregion

    function onBeforeSaveRecord(param) {
        var paramInput = $('#<%=txtJournalNoSelected.ClientID %>').val();
        if (paramInput != "") {
            return true;
        } else {
            return false;
        }
    }
</script>
<input type="hidden" id="hdnGLTransactionIDCtlAcc" runat="server" />
<input type="hidden" id="hdnGLTransactionIDCtlCopySelected" runat="server" />
<div style="height: 440px; overflow-y: auto; overflow-x: hidden">
    <table style="width: 100%" class="tblContentArea">
        <colgroup>
            <col style="width: 100%" />
        </colgroup>
        <tr>
            <td style="padding: 5px; vertical-align: top">
                <table class="tblEntryContent" style="width: 70%">
                    <colgroup>
                        <col style="width: 120px" />
                        <col style="width: 170px" />
                        <col style="width: 170px" />
                        <col />
                    </colgroup>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblMandatory lblLink" id="lblJournalNoCtlAcc">
                                <%=GetLabel("Nomor Jurnal") %></label>
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtJournalNoSelected" Width="150px" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblMandatory">
                                <%=GetLabel("Jenis Salin")%></label>
                        </td>
                        <td colspan="2">
                            <dxe:ASPxComboBox ID="cboCopyType" ClientInstanceName="cboCopyType" Width="150px"
                                runat="server">
                            </dxe:ASPxComboBox>
                        </td>
                    </tr>
                </table>
                <dxcp:ASPxCallbackPanel ID="cbpEntryPopupView" runat="server" Width="100%" ClientInstanceName="cbpEntryPopupView"
                    ShowLoadingPanel="false" OnCallback="cbpEntryPopupView_Callback">
                    <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ hideLoadingPanel(); }" />
                    <PanelCollection>
                        <dx:PanelContent ID="PanelContent1" runat="server">
                            <asp:Panel runat="server" ID="pnlEntryPopupGrdView" Style="width: 100%; margin-left: auto;
                                margin-right: auto; position: relative; font-size: 0.95em;">
                                <table style="width: 100%">
                                    <tr>
                                        <td style="width: 100%" valign="top">
                                            <asp:GridView ID="grdView" runat="server" CssClass="grdView notAllowSelect" AutoGenerateColumns="false"
                                                ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                                <Columns>
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
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </dx:PanelContent>
                    </PanelCollection>
                </dxcp:ASPxCallbackPanel>
            </td>
        </tr>
    </table>
</div>
