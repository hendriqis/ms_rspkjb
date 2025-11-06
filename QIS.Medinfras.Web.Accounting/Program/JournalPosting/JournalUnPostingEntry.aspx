<%@ Page Title="" Language="C#" MasterPageFile="~/libs/MasterPage/MPTrx.master"
    AutoEventWireup="true" CodeBehind="JournalUnPostingEntry.aspx.cs" Inherits="QIS.Medinfras.Web.Accounting.Program.JournalUnPostingEntry" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<asp:Content ID="Content3" ContentPlaceHolderID="plhCustomButtonToolbar" runat="server">
    <li id="btnUnposting" runat="server" crudmode="R">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbvoid.png")%>' alt="" /><div>
            <%=GetLabel("Un-Posting")%></div>
    </li>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div class="menuTitle">
        <%=HttpUtility.HtmlEncode(GetPageTitle())%></div>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript" src='<%= ResolveUrl("~/Libs/Scripts/CustomGridViewList.js")%>'></script>
    <script type="text/javascript">
        $(function () {
            var grd = new customGridView();
            grd.init('<%=grdView.ClientID %>', '<%=hdnID.ClientID %>', '<%=pnlView.ClientID %>', cbpView, 'paging');

            $('#<%=btnUnposting.ClientID %>').click(function () {
                showToastConfirmation('Apakah Anda Yakin?', function (result) {
                    if (result) {
                        cbpProcess.PerformCallback('unposting');
                    }
                });
            });
        });

        function onCbpProcesEndCallback(s) {
            var param = s.cpResult.split('|');
            if (param[1] == 'fail')
                showToast('Process Failed', 'Error Message : ' + param[2]);
            else
                showToast('Process Success', 'Proses Un-Posting Jurnal Berhasil Dilakukan');

            onLoadObject();
            hideLoadingPanel();
        }

        $('.lnkDetail a').live('click', function () {
            $tr = $(this).closest('tr');
            var id = $tr.find('.keyField').html();
            var url = ResolveUrl("~/Program/Journal/JournalListDtCtl.ascx");
            openUserControlPopup(url, id, 'Detail', 1200, 500);
        });

        function onRefreshControl(filterExpression) {
            cbpView.PerformCallback('refresh');
        }

        //#region Paging
        var pageCount = parseInt('<%=PageCount %>');
        var currPage = parseInt('<%=CurrPage %>');
        $(function () {
            setPaging($("#paging"), pageCount, function (page) {
                cbpView.PerformCallback('changepage|' + page);
            }, null, currPage);
        });

        function onCbpViewEndCallback(s) {
            hideLoadingPanel();

            var param = s.cpResult.split('|');
            if (param[0] == 'refresh') {
                var pageCount = parseInt(param[1]);
                if (pageCount > 0)
                    $('#<%=grdView.ClientID %> tr:eq(1)').click();

                setPaging($("#paging"), pageCount, function (page) {
                    cbpView.PerformCallback('changepage|' + page);
                });
            }
            else
                $('#<%=grdView.ClientID %> tr:eq(1)').click();
        }
        //#endregion
    </script>
    <input type="hidden" id="hdnID" runat="server" value="" />
    <input type="hidden" id="hdnPageCount" runat="server" value="" />
    <input type="hidden" id="hdnIsEditable" runat="server" value="" />
    <input type="hidden" value="" id="hdnPageTitle" runat="server" />
    <div style="position: relative; height:100px">
        <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
            ShowLoadingPanel="false" OnCallback="cbpView_Callback">
            <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ onCbpViewEndCallback(s); }" />
            <PanelCollection>
                <dx:PanelContent ID="PanelContent1" runat="server">
                    <asp:Panel runat="server" ID="pnlView" CssClass="pnlContainerGrid" Height="100px">
                        <asp:GridView ID="grdView" runat="server" CssClass="grdSelected" AutoGenerateColumns="false"
                            ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                            <Columns>
                                <asp:BoundField DataField="GLTransactionID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                <asp:BoundField DataField="JournalNo" HeaderText="No Jurnal" HeaderStyle-Width="150px" />
                                <asp:BoundField DataField="JournalDateInString" ItemStyle-HorizontalAlign="Center"
                                    HeaderText="Tanggal" HeaderStyle-Width="150px" />
                                <asp:BoundField DataField="Remarks" HeaderText="Catatan" />
                                <asp:BoundField DataField="TransactionStatus" HeaderText="Status" ItemStyle-HorizontalAlign="Center"
                                    HeaderStyle-Width="100px" />
                                <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="lnkDetail"
                                    HeaderText="Detail" HeaderStyle-Width="150px">
                                    <ItemTemplate>
                                        <input type="hidden" class="hdnGCTransactionStatus" value='<%#:Eval("GCTransactionStatus") %>'>
                                        <a>Detail</a>
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
        <div class="imgLoadingGrdView" id="containerImgLoadingView">
            <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
        </div>
    </div>
    <div style="padding-top: 30px">
        <table width="50%">
            <tr>
                <td>
                    <div class="lblComponent" style="background-color: #2c3e50; font-size: 18px ">
                        <font color="white"><%=GetLabel("Informasi Un-Posting Terakhir") %></font></div>
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
    <dxcp:ASPxCallbackPanel ID="cbpProcess" runat="server" Width="100%" ClientInstanceName="cbpProcess"
        ShowLoadingPanel="false" OnCallback="cbpProcess_Callback">
        <ClientSideEvents BeginCallback="function(s,e) { showLoadingPanel(); }" EndCallback="function(s,e) { onCbpProcesEndCallback(s); }" />
    </dxcp:ASPxCallbackPanel>
</asp:Content>
