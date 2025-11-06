<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FADepreciationEntryCtl.ascx.cs" 
    Inherits="QIS.Medinfras.Web.Inventory.Program.FADepreciationEntryCtl" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>

<script type="text/javascript" id="dxss_fadepreciationentryctl">
    $(function () {
        $('#btnProses').click(function () {
            cbpPopupProcess.PerformCallback('process');
        });

    });

    //#region Paging
    var pageCount = parseInt('<%=PageCount %>');
    var currPage = parseInt('<%=CurrPage %>');
    $(function () {
        setPaging($("#paging"), pageCount, function (page) {
            cbpPopupView.PerformCallback('changepage|' + page);
        }, null, currPage);
    });

    function onCbpPopupViewEndCallback(s) {
        hideLoadingPanel();

        var param = s.cpResult.split('|');
        if (param[0] == 'refresh') {
            var pageCount = parseInt(param[1]);
            if (pageCount > 0)
                $('#<%=grdPopupView.ClientID %> tr:eq(1)').click();

            setPaging($("#paging"), pageCount, function (page) {
                cbpPopupView.PerformCallback('changepage|' + page);
            });
        }
        else
            $('#<%=grdPopupView.ClientID %> tr:eq(1)').click();
    }

    function onCbpPopupProcessEndCallback(s) {
        hideLoadingPanel();
        cbpPopupView.PerformCallback('refresh');
    }
    //#endregion

</script>
<input type="hidden" id="hdnFixedAssetID" runat="server" />
<input type="hidden" id="hdnIsProcessDepreciationFromFAAcceptance" runat="server" />
<table class="tblContentArea">
    <colgroup>
        <col style="width:100%"/>
    </colgroup>
    <tr>
        <td>
            <table width="100%">
                <colgroup>
                    <col width="50%"/>
                </colgroup>
                <tr id="trButtonProcess" runat="server">
                    <td align="left"><input type="button" value='<%= GetLabel("Proses Akumulasi Penyusutan")%>' id="btnProses" /></td>
                </tr>
            </table>
        </td>
   </tr>
   <tr>
    <td>
        <table width="100%">
            <tr>
                <td>
                    <div style="position: relative;">
                        <dxcp:ASPxCallbackPanel ID="cbpPopupView" runat="server" Width="100%" ClientInstanceName="cbpPopupView"
                            ShowLoadingPanel="false" OnCallback="cbpPopupView_Callback">
                            <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }"
                                EndCallback="function(s,e){ onCbpPopupViewEndCallback(s); }" />
                            <PanelCollection>
                                <dx:PanelContent ID="PanelContent1" runat="server">
                                    <asp:Panel runat="server" ID="pnlView" CssClass="pnlContainerGrid">
                                        <asp:GridView ID="grdPopupView" runat="server" CssClass="grdSelected" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                            <Columns>
                                                <asp:BoundField DataField="FADepreciationID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                                <asp:BoundField DataField="DepreciationYear" HeaderText="Tahun" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="80px" />
                                                <asp:BoundField DataField="DepreciationPeriodNo" HeaderText="Periode" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="60px" />
                                                <asp:BoundField DataField="DepreciationDateInString" HeaderText="Tgl. Susut" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                                <asp:BoundField DataField="AssetValue" HeaderText="Nilai Buku Awal" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:N2}" HeaderStyle-Width="125px" />
                                                <asp:BoundField DataField="DepreciationAmount" HeaderText="Nilai Susut" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:N2}" HeaderStyle-Width="125px" />
                                                <asp:BoundField DataField="TotalDepreciationAmount" HeaderText="Akumulasi Penyusutan" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:N2}" HeaderStyle-Width="125px" />
                                                <asp:BoundField DataField="NilaiBukuAkhir" HeaderText="Nilai Buku Akhir" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:N2}" HeaderStyle-Width="125px" />
                                            </Columns>
                                            <EmptyDataTemplate>
                                                <%=GetLabel("No Data To Display")%>
                                            </EmptyDataTemplate>
                                        </asp:GridView>
                                    </asp:Panel>
                                </dx:PanelContent>
                            </PanelCollection>
                        </dxcp:ASPxCallbackPanel>    
                        <div class="imgLoadingGrdView" id="containerImgLoadingView" >
                            <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                        </div>
                        <div class="containerPaging">
                            <div class="wrapperPaging">
                                <div id="paging"></div>
                            </div>
                        </div>
                        <dxcp:ASPxCallbackPanel ID="cbpPopupProcess" runat="server" Width="100%" ClientInstanceName="cbpPopupProcess"
                            ShowLoadingPanel="false" OnCallback="cbpPopupProcess_Callback">
                            <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }"
                                EndCallback="function(s,e){ onCbpPopupProcessEndCallback(s); }" />
                        </dxcp:ASPxCallbackPanel>
                    </div>
                </td>
            </tr>
        </table>
    </td>
   </tr>
</table>

