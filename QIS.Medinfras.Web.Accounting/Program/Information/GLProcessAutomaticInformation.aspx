<%@ Page Language="C#" MasterPageFile="~/libs/MasterPage/MPTrx2.master" AutoEventWireup="true"
    CodeBehind="GLProcessAutomaticInformation.aspx.cs" Inherits="QIS.Medinfras.Web.Accounting.Program.GLProcessAutomaticInformation" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<asp:Content ID="Content3" ContentPlaceHolderID="plhCustomButtonToolbar" runat="server">
    <li id="btnRefresh" runat="server" CRUDMode="R"><img src='<%=ResolveUrl("~/Libs/Images/Icon/tbrefresh.png")%>' alt="" /><div><%=GetLabel("Refresh")%></div></li>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div class="menuTitle">
        <%=HttpUtility.HtmlEncode(GetPageTitle())%></div>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript" src='<%= ResolveUrl("~/Libs/Scripts/CustomGridViewList.js")%>'></script>
    <script type="text/javascript">
        $(function () {
            setDatePicker('<%=txtFromDate.ClientID %>');
            $('#<%=txtFromDate.ClientID %>').datepicker('option', 'maxDate', new Date(getDateNow()));

            setDatePicker('<%=txtToDate.ClientID %>');
            $('#<%=txtToDate.ClientID %>').datepicker('option', 'maxDate', new Date(getDateNow()));

        });

        $('#<%=btnRefresh.ClientID %>').live('click', function () {
            cbpViewSummary.PerformCallback('refresh');
            cbpViewDetail.PerformCallback('refresh');
        });

        function onCbpViewSummaryEndCallback() {
            hideLoadingPanel();
        }

        function onRefreshControl(filterExpression) {
            cbpViewSummary.PerformCallback('refresh');
            cbpViewDetail.PerformCallback('refresh');
        }

    </script>
    <input type="hidden" value="" id="hdnID" runat="server" />
    <input type="hidden" value="" id="hdnLastPostingDate" runat="server" />
    <input type="hidden" value="" id="hdnPageTitle" runat="server" />
    <table class="tblContentArea">
        <tr>
            <td>
                <table>
                    <colgroup>
                        <col style="width: 140px" />
                    </colgroup>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Periode Transaksi")%></label>
                        </td>
                        <td>
                            <table cellpadding="0" cellspacing="0">
                                <colgroup>
                                    <col style="width: 145px" />
                                    <col style="width: 20px" />
                                    <col style="width: 145px" />
                                </colgroup>
                                <tr>
                                    <td>
                                        <asp:TextBox ID="txtFromDate" Width="120px" CssClass="datepicker" runat="server" />
                                    </td>
                                    <td style="padding: 5px">
                                        <%=GetLabel(" s/d ") %>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtToDate" Width="120px" CssClass="datepicker" runat="server" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Filter Status")%></label>
                        </td>
                        <td>
                            <table cellpadding="0" cellspacing="0">
                                <colgroup>
                                    <col style="width: 130px" />
                                </colgroup>
                                <tr>
                                    <td>
                                        <dxe:ASPxComboBox ID="cboStatusJournal" Width="130px" runat="server" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
        </tr>
        <tr style="outline-style: solid; outline-width: thin; outline-color: Black">
        </tr>
        <tr>
        </tr>
    </table>
    <div style="height: 100px; width: auto">
        <h4>
            Summary Process Journal</h4>
        <table>
            <tr>
                <td>
                    <div>
                        <dxcp:ASPxCallbackPanel ID="cbpViewSummary" runat="server" ClientInstanceName="cbpViewSummary"
                            ShowLoadingPanel="false" OnCallback="cbpViewSummary_Callback">
                            <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ onCbpViewSummaryEndCallback(s); }" />
                            <PanelCollection>
                                <dx:PanelContent ID="PanelContent1" runat="server">
                                    <asp:Panel runat="server" ID="pnlViewSummary" CssClass="pnlContainerGrid" Height="60px">
                                        <asp:GridView ID="grdViewSummary" runat="server" CssClass="grdSelected" AutoGenerateColumns="false"
                                            ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                            <Columns>
                                                <asp:BoundField DataField="TotalJournal" ItemStyle-HorizontalAlign="Center" HeaderText="Total Transaksi"
                                                    HeaderStyle-Width="150px" />
                                                <asp:BoundField DataField="TotalJournalBerhasil" ItemStyle-HorizontalAlign="Center"
                                                    HeaderText="Total Berhasil" HeaderStyle-Width="150px" />
                                                <asp:BoundField DataField="TotalJournalGagal" ItemStyle-HorizontalAlign="Center"
                                                    HeaderText="Total Gagal" HeaderStyle-Width="150px" />
                                            </Columns>
                                        </asp:GridView>
                                    </asp:Panel>
                                </dx:PanelContent>
                            </PanelCollection>
                        </dxcp:ASPxCallbackPanel>
                    </div>
                </td>
            </tr>
        </table>
    </div>
    <div style="height: 250px; width: auto">
        <h4>
            Detail Process Journal</h4>
        <table>
            <tr>
                <td>
                    <div style="position: relative;">
                        <dxcp:ASPxCallbackPanel ID="cbpViewDetail" runat="server" ClientInstanceName="cbpViewDetail"
                            ShowLoadingPanel="false" OnCallback="cbpViewDetail_Callback">
                            <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ onCbpViewDetailEndCallback(s); }" />
                            <PanelCollection>
                                <dx:PanelContent ID="PanelContent2" runat="server">
                                    <asp:Panel runat="server" ID="pnlViewDetail" CssClass="pnlContainerGrid" Height="350px">
                                        <asp:GridView ID="grdViewDetail" runat="server" CssClass="grdSelected" AutoGenerateColumns="false"
                                            ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                            <Columns>
                                                <asp:BoundField DataField="ID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                                <asp:BoundField DataField="TransactionName" HeaderText="Sumber Data" />
                                                <asp:BoundField DataField="cfProcessDateTime" ItemStyle-HorizontalAlign="Center"
                                                    HeaderText="Tanggal Proses" HeaderStyle-Width="120px" />
                                                <asp:BoundField DataField="JournalRemarks" HeaderText="Catatan" />
                                                <asp:BoundField DataField="ErrorDescription" HeaderText="Deskripsi" />
                                                <asp:BoundField DataField="CreatedByUsername" ItemStyle-HorizontalAlign="Center"
                                                    HeaderText="Diproses Oleh" HeaderStyle-Width="80px" />
                                                <asp:BoundField DataField="cfCreatedDate" ItemStyle-HorizontalAlign="Center" HeaderText="Diproses Pada"
                                                    HeaderStyle-Width="120px" />
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
    </div>
</asp:Content>
