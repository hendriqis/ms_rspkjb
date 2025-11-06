<%@ Page Title="" Language="C#" MasterPageFile="~/Libs/MasterPage/MPList.master" AutoEventWireup="true" 
    CodeBehind="PurchaseInvoiceInfo.aspx.cs" Inherits="QIS.Medinfras.Web.Finance.Program.PurchaseInvoiceInfo" %>

<%@ Register Assembly="DevExpress.Web.ASPxPivotGrid.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPivotGrid" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.ASPxPivotGrid.v11.1.Export, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPivotGrid.Export" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="QIS.Medinfras.Web.CustomControl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"
    Namespace="QIS.Medinfras.Web.CustomControl" TagPrefix="qis" %>

<asp:Content ID="Content1" ContentPlaceHolderID="plhList" runat="server">
    <script type="text/javascript">
        $(function () {
            setDatePicker('<%=txtDateFrom.ClientID %>');
            setDatePicker('<%=txtDateTo.ClientID %>');

            $('#<%=txtDateFrom.ClientID %>').change(function () {
                cbpView.PerformCallback('refresh');
            });
            $('#<%=txtDateTo.ClientID %>').change(function () {
                cbpView.PerformCallback('refresh');
            });
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

        $('.lblBalanceBEGIN').die('click');
        $('.lblBalanceBEGIN').live('click', function () {
            $tr = $(this).closest('tr');
            var supplierID = $tr.find('.keyField').html();

            var url = ResolveUrl("~/Program/Information/PurchaseInvoiceInfoDetailCtl.ascx");
            openUserControlPopup(url, supplierID, 'Detail Information', 1200, 550);
        });
    </script>
    <input type="hidden" value="" id="hdnFilterExpressionQuickSearch" runat="server" />
    <table width="100%">
        <tr>
            <td>
                <table class="tblEntryContent" style="width:550px;">
                    <colgroup>
                        <col style="width: 150px" />
                        <col style="width: 400px" />
                    </colgroup>
                    <tr>
                        <td><label><%=GetLabel("Tanggal") %></label></td>
                        <td>
                            <table cellpadding="0" cellspacing="0">
                                <tr>
                                    <td><asp:TextBox runat="server" CssClass="datepicker" ID="txtDateFrom" Width="120px" /></td>
                                    <td>&nbsp;</td>
                                    <td><asp:TextBox runat="server" CssClass="datepicker" ID="txtDateTo" Width="120px" /></td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView" ShowLoadingPanel="false" OnCallback="cbpView_Callback">
                    <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }"
                        EndCallback="function(s,e){ onCbpViewEndCallback(s); }" />
                    <PanelCollection>
                        <dx:PanelContent ID="PanelContent1" runat="server">
                            <asp:Panel runat="server" ID="pnlView" Style="width: 100%; margin-left: auto; margin-right: auto;
                                position: relative; font-size: 0.95em;">
                                <asp:GridView ID="grdView" runat="server" CssClass="grdView notAllowSelect"
                                    AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty" >
                                    <Columns>
                                        <asp:BoundField DataField="SupplierID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                        <asp:BoundField DataField="SupplierCode" HeaderText="Kode" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="130px"  />
                                        <asp:BoundField DataField="SupplierName" HeaderText="Name" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                                        <asp:TemplateField HeaderStyle-HorizontalAlign="Right" HeaderStyle-Width="130px" ItemStyle-HorizontalAlign="Right" >
                                            <HeaderTemplate><%=GetLabel("Saldo") %></HeaderTemplate>
                                            <ItemTemplate>
                                                <label <%#:Eval("BalanceBEGIN").ToString() != "0.00" ? "class='lblLink lblBalanceBEGIN'":"" %> ><%#:Eval("BalanceBEGIN","{0:n}") %></label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderStyle-HorizontalAlign="Right" HeaderStyle-Width="130px" ItemStyle-HorizontalAlign="Right" >
                                            <HeaderTemplate><%=GetLabel("Hutang") %></HeaderTemplate>
                                            <ItemTemplate>
                                                <label <%#:Eval("BalanceIN").ToString() != "0.00" ? "class='lblLink lblBalanceIN'":"" %> ><%#:Eval("BalanceIN","{0:n}") %></label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderStyle-HorizontalAlign="Right" HeaderStyle-Width="130px" ItemStyle-HorizontalAlign="Right" >
                                            <HeaderTemplate><%=GetLabel("Pembayaran") %></HeaderTemplate>
                                            <ItemTemplate>
                                                <label <%#:Eval("BalanceOUT").ToString() != "0.00" ? "class='lblLink lblBalanceOUT'":"" %> ><%#:Eval("BalanceOUT","{0:n}") %></label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="BalanceEND" HeaderText="Saldo Akhir" HeaderStyle-HorizontalAlign="Right" HeaderStyle-Width="130px" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:n}" />
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
                <div class="containerPaging">
                    <div class="wrapperPaging">
                        <div id="paging">
                        </div>
                    </div>
                </div>
            </td>
        </tr>
    </table>
</asp:Content>
