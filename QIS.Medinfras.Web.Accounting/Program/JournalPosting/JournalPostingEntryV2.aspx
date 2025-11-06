<%@ Page Title="" Language="C#" MasterPageFile="~/libs/MasterPage/MPTrx.master" AutoEventWireup="true"
    CodeBehind="JournalPostingEntryV2.aspx.cs" Inherits="QIS.Medinfras.Web.Accounting.Program.JournalPostingEntryV2" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<asp:Content ID="Content3" ContentPlaceHolderID="plhCustomButtonToolbar" runat="server">
    <li id="btnPosting" runat="server" crudmode="R">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbapprove.png")%>' alt="" /><div>
            <%=GetLabel("Posting")%></div>
    </li>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div class="menuTitle">
        <%=HttpUtility.HtmlEncode(GetPageTitle())%></div>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript">
        $(function () {
            $('#<%=btnPosting.ClientID %>').click(function () {
                showToastConfirmation('Apakah Anda Yakin?', function (result) {
                    if (result) {
                        cbpProcess.PerformCallback('posting');
                    }
                });
            });
        });

        function onCbpProcesEndCallback(s) {
            var param = s.cpResult.split('|');
            if (param[1] == 'fail')
                showToast('Process Failed', 'Error Message : ' + param[2]);
            else
                showToast('Process Success', 'Proses Posting Jurnal Berhasil Dilakukan');

            onLoadObject();
            hideLoadingPanel();
        }
        
    </script>
    <input type="hidden" value="" id="hdnPageTitle" runat="server" />
    <input type="hidden" value="" id="hdnIsFiscalYear" runat="server" />
    <input type="hidden" value="" id="hdnFiscalYearPeriod" runat="server" />
    <input type="hidden" value="" id="hdnFiscalYearStartMonth" runat="server" />
    <input type="hidden" value="" id="hdnPeriodNo" runat="server" />
    <input type="hidden" value="" id="hdnIsAllowPosting" runat="server" />
    <input type="hidden" value="" id="hdnSisaJurnal" runat="server" />
    <table>
        <colgroup>
            <col style="width: 120px" />
            <col />
        </colgroup>
        <tr>
            <td class="tdLabel" style="padding-top: 3px">
                <label class="tdLabel">
                    <%=GetLabel("Periode")%></label>
            </td>
            <td style="padding-top: 3px">
                <asp:TextBox runat="server" ID="txtPeriod" CssClass="datepicker" ReadOnly="true" />
            </td>
        </tr>
        <tr>
            <td class="tdLabel" style="padding-top: 3px">
                <label class="tdLabel">
                    <%=GetLabel("Audit")%></label>
            </td>
            <td style="padding-top: 3px">
                <asp:CheckBox ID="chkIsAuditedJournal" Checked="false" Width="100%" runat="server" Enabled=false/>
            </td>
        </tr>
    </table>
    <div style="padding-top: 30px">
        <table width="50%">
            <tr>
                <td>
                    <div class="lblComponent" style="background-color: #2c3e50; font-size: 18px">
                        <font color="white">
                            <%=GetLabel("Informasi Posting Terakhir") %></font></div>
                    <div style="background-color: #dcdada;">
                        <table width="100%" cellpadding="0" cellspacing="0">
                            <colgroup>
                                <col width="150px" />
                                <col width="25px" />
                                <col />
                            </colgroup>
                            <tr style="padding: 5px">
                                <td>
                                    <%=GetLabel("Posting Terakhir") %>
                                </td>
                                <td align="center">
                                    :
                                </td>
                                <td>
                                    <div runat="server" id="divJournalNo">
                                    </div>
                                </td>
                            </tr>
                            <tr style="padding: 5px">
                                <td>
                                    <%=GetLabel("Periode Posting Terakhir")%>
                                </td>
                                <td align="center">
                                    :
                                </td>
                                <td>
                                    <div runat="server" id="divJournalDate">
                                    </div>
                                </td>
                            </tr>
                            <tr style="outline-style: dotted; outline-width: thin; outline-color: Black; padding: 7px">
                            </tr>
                            <tr style="padding: 5px">
                                <td>
                                    <%=GetLabel("Dibuat Oleh") %>
                                </td>
                                <td align="center">
                                    :
                                </td>
                                <td>
                                    <div runat="server" id="divCreatedBy">
                                    </div>
                                </td>
                            </tr>
                            <tr style="padding: 5px">
                                <td>
                                    <%=GetLabel("Dibuat Pada") %>
                                </td>
                                <td align="center">
                                    :
                                </td>
                                <td>
                                    <div runat="server" id="divCreatedDate">
                                    </div>
                                </td>
                            </tr>
                            <tr style="padding: 5px">
                                <td>
                                    <%=GetLabel("Terakhir Dibuat Oleh") %>
                                </td>
                                <td align="center">
                                    :
                                </td>
                                <td>
                                    <div runat="server" id="divLastUpdatedBy">
                                    </div>
                                </td>
                            </tr>
                            <tr style="padding: 5px">
                                <td>
                                    <%=GetLabel("Terakhir Dibuat Pada")%>
                                </td>
                                <td align="center">
                                    :
                                </td>
                                <td>
                                    <div runat="server" id="divLastUpdatedDate">
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </div>
                </td>
            </tr>
        </table>
    </div>
    <div style="padding-top: 30px">
        <table width="100%">
            <tr>
                <td>
                    <hr style="margin: 10 10 10 10;" />
                    <div class="lblComponent" style="background-color: #fffa9a; font-size: 14px; font-weight: bold">
                        <font color="black">
                            <%=GetLabel("Informasi Detail Jurnal Open / Tidak Seimbang") %></font></div>
                    <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
                        ShowLoadingPanel="false" OnCallback="cbpView_Callback">
                        <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ hideLoadingPanel(); }" />
                        <PanelCollection>
                            <dx:PanelContent ID="PanelContent1" runat="server">
                                <asp:Panel runat="server" ID="pnlView" Style="width: 100%; margin-left: auto; margin-right: auto;
                                    position: relative;">
                                    <input type="hidden" value="0" id="hdnDisplayCount" runat="server" />
                                    <asp:GridView ID="grdView" runat="server" CssClass="grdNormal notAllowSelect" AutoGenerateColumns="false"
                                        ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                        <Columns>
                                            <asp:BoundField DataField="GLTransactionID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                            <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="80px">
                                                <ItemTemplate>
                                                    <input type="hidden" value="<%#:Eval("GLTransactionID") %>" bindingfield="GLTransactionID" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="InfoTypeCaption" HeaderText="Status" ItemStyle-HorizontalAlign="Center"
                                                HeaderStyle-HorizontalAlign="Center" />
                                            <asp:BoundField DataField="JournalNo" HeaderText="No. Jurnal" HeaderStyle-Width="150px"
                                                ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" />
                                            <asp:BoundField DataField="cfJournalDateInString" HeaderText="Tgl. Jurnal" HeaderStyle-Width="150px"
                                                ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" />
                                            <asp:TemplateField ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right"
                                                HeaderStyle-Width="120px">
                                                <HeaderTemplate>
                                                    <%=GetLabel("Debet") %></HeaderTemplate>
                                                <ItemTemplate>
                                                    <%#:Eval("cfDebitAmountInString")%>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right"
                                                HeaderStyle-Width="120px">
                                                <HeaderTemplate>
                                                    <%=GetLabel("Kredit") %></HeaderTemplate>
                                                <ItemTemplate>
                                                    <%#:Eval("cfCreditAmountInString")%>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="TransactionStatus" HeaderText="Status" ItemStyle-HorizontalAlign="Center"
                                                HeaderStyle-HorizontalAlign="Center" />
                                        </Columns>
                                        <EmptyDataTemplate>
                                            <%=GetLabel("No Data To Display")%>
                                        </EmptyDataTemplate>
                                    </asp:GridView>
                                </asp:Panel>
                            </dx:PanelContent>
                        </PanelCollection>
                    </dxcp:ASPxCallbackPanel>
                </td>
            </tr>
        </table>
    </div>
    <div style="padding-top: 30px">
        <table width="100%">
            <tr id="trHutangSupplier" runat="server">
                <td>
                    <hr style="margin: 10 10 10 10;" />
                    <div class="lblComponent" style="background-color: #fffa9a; font-size: 14px; font-weight: bold">
                        <font color="black">
                            <%=GetLabel("Informasi Hutang Supplier") %></font></div>
                    <dxcp:ASPxCallbackPanel ID="cbpViewHutang" runat="server" Width="100%" ClientInstanceName="cbpViewHutang"
                        ShowLoadingPanel="false" OnCallback="cbpViewHutang_Callback">
                        <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ hideLoadingPanel(); }" />
                        <PanelCollection>
                            <dx:PanelContent ID="PanelContent2" runat="server">
                                <asp:Panel runat="server" ID="Panel1" Style="width: 100%; margin-left: auto; margin-right: auto;
                                    position: relative;">
                                    <input type="hidden" value="0" id="Hidden1" runat="server" />
                                    <asp:GridView ID="grdViewHutang" runat="server" CssClass="grdNormal notAllowSelect"
                                        AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                        <Columns>
                                            <asp:BoundField DataField="cfPurchaseInvoiceDateInString" HeaderText="Tanggal Tukar Faktur"
                                                HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" />
                                            <asp:BoundField DataField="PurchaseInvoiceNo" HeaderText="No. Tukar Faktur" HeaderStyle-Width="100px"
                                                ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" />
                                            <asp:BoundField DataField="BusinessPartnerName" HeaderText="Supplier" HeaderStyle-Width="300px"
                                                ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left" />
                                            <asp:BoundField DataField="TransactionStatus" HeaderText="Status" HeaderStyle-Width="100px"
                                                ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" />
                                        </Columns>
                                        <EmptyDataTemplate>
                                            <%=GetLabel("No Data To Display")%>
                                        </EmptyDataTemplate>
                                    </asp:GridView>
                                </asp:Panel>
                            </dx:PanelContent>
                        </PanelCollection>
                    </dxcp:ASPxCallbackPanel>
                </td>
            </tr>
        </table>
    </div>
    <dxcp:ASPxCallbackPanel ID="cbpProcess" runat="server" Width="100%" ClientInstanceName="cbpProcess"
        ShowLoadingPanel="false" OnCallback="cbpProcess_Callback">
        <ClientSideEvents BeginCallback="function(s,e) { showLoadingPanel(); }" EndCallback="function(s,e) { onCbpProcesEndCallback(s); }" />
    </dxcp:ASPxCallbackPanel>
</asp:Content>
