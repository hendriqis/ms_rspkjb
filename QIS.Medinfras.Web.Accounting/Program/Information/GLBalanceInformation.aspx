<%@ Page Title="" Language="C#" MasterPageFile="~/Libs/MasterPage/MPTrx2.master"
    AutoEventWireup="true" CodeBehind="GLBalanceInformation.aspx.cs" Inherits="QIS.Medinfras.Web.Accounting.Program.GLBalanceInformation" %>

<%@ Register Assembly="QIS.Medinfras.Web.CustomControl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"
    Namespace="QIS.Medinfras.Web.CustomControl" TagPrefix="qis" %>
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
<asp:Content ID="Content3" ContentPlaceHolderID="plhCustomButtonToolbar" runat="server">
    <li id="btnRefresh" crudmode="R" runat="server">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbrefresh.png")%>' alt="" /><br style="clear: both" />
        <div>
            <%=GetLabel("Refresh")%></div>
    </li>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div class="menuTitle">
        <%=HttpUtility.HtmlEncode(GetPageTitle())%></div>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript">
        $(function () {
            $('#<%=btnRefresh.ClientID %>').live('click', function () {
                cbpTotal.PerformCallback('refresh');
            });
        });

        $('.lblAccountNo').live('click', function () {
            $tr = $(this).closest('tr');
            var accountID = $tr.find('.keyField').html();

            var url = ResolveUrl('~/Program/Information/GLBalanceInformationCtl.ascx');
            var id = accountID;
            var period = cboYear.GetValue() + '|' + cboMonth.GetValue();
            var status = $('#<%=hdnDataSource.ClientID %>').val();
            var param = id + '|' + period + '|' + status;
            openUserControlPopup(url, param, 'Detail', 1000, 600);
        });

        function onCboStatusValueChanged(s) {
            var value = s.GetValue();
            $('#<%=hdnDataSource.ClientID %>').val(value);
        }

        $(function () {
            $('#btnProcess').click(function () {
                cbpView.PerformCallback('refresh');
            });
        });

        function onCbpViewEndCallback(s) {
            hideLoadingPanel();
        }
    </script>
    <input type="hidden" value="" id="hdnPageTitle" runat="server" />
    <div>
        <table width="100%">
            <colgroup>
                <col width="120px" />
                <col />
            </colgroup>
            <tr>
                <td>
                    <table>
                        <tr id="trPeriode" runat="server">
                            <td class="tdLabel">
                                <label class="tdLabel">
                                    <%=GetLabel("Periode")%></label>
                            </td>
                            <td>
                                <dxe:ASPxComboBox ID="cboYear" Width="120px" ClientInstanceName="cboYear" runat="server" />
                                <input type="hidden" id="hdnSelectedYear" runat="server" />
                            </td>
                            <td>
                                <dxe:ASPxComboBox ID="cboMonth" Width="120px" ClientInstanceName="cboMonth" runat="server" />
                                <input type="hidden" value="" id="hdnSelectedMonth" runat="server" />
                            </td>
                            <td>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="tdLabel">
                                    <%=GetLabel("Filter Status")%></label>
                            </td>
                            <td colspan="2">
                                <dxe:ASPxComboBox ID="cboStatus" ClientInstanceName="cboStatus" Width="245px" runat="server">
                                    <ClientSideEvents ValueChanged="function(s,e){ onCboStatusValueChanged(s); }" />
                                </dxe:ASPxComboBox>
                                <input type="hidden" id="hdnDataSource" runat="server" />
                            </td>
                            <td>
                                <asp:CheckBox runat="server" Text="Hanya COA Detail" ID="chkIsDetailOnly" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <div style="height: 400px; overflow-y: auto;">
                        <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
                            ShowLoadingPanel="false" OnCallback="cbpView_Callback">
                            <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ onCbpViewEndCallback(s); }" />
                            <PanelCollection>
                                <dx:PanelContent ID="PanelContent1" runat="server">
                                    <asp:Panel runat="server" ID="pnlView" Style="width: 100%; margin-left: auto; margin-right: auto;
                                        position: relative; font-size: 0.95em;">
                                        <asp:GridView ID="grdView" runat="server" CssClass="grdView notAllowSelect" AutoGenerateColumns="false"
                                            ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                            <Columns>
                                                <asp:BoundField DataField="GLAccountID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                                <asp:TemplateField HeaderStyle-Width="120px" HeaderStyle-HorizontalAlign="Left">
                                                    <HeaderTemplate>
                                                        <div style="padding-left: 3px">
                                                            <%=GetLabel("COA")%>
                                                        </div>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <div style='margin-left: <%#: Eval("LevelCOA") %>0px;'>
                                                            <label <%#: Eval("IsHeader").ToString() == "0" ? "class='lblLink lblAccountNo'":"" %>>
                                                                <%#: Eval("GLAccountNo")%></label>
                                                        </div>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderStyle-Width="120px" HeaderStyle-HorizontalAlign="Left">
                                                    <HeaderTemplate>
                                                        <div style="padding-left: 3px">
                                                            <%=GetLabel("Nama COA")%>
                                                        </div>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <div style='margin-left: <%#: Eval("LevelCOA") %>0px;'>
                                                            <label <%#: Eval("IsHeader").ToString() == "0" ? "class='lblLink lblAccountNo'":"" %>>
                                                                <%#: Eval("GLAccountName")%></label>
                                                        </div>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="BalanceBEGIN" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:N}"
                                                    HeaderStyle-Width="150px" HeaderText="Saldo Awal" HeaderStyle-HorizontalAlign="Right" />
                                                <asp:BoundField DataField="DEBITAmount" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:N}"
                                                    HeaderStyle-Width="150px" HeaderText="Debet" HeaderStyle-HorizontalAlign="Right" />
                                                <asp:BoundField DataField="CREDITAmount" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:N}"
                                                    HeaderStyle-Width="150px" HeaderText="Kredit" HeaderStyle-HorizontalAlign="Right" />
                                                <asp:BoundField DataField="BalanceEND" ItemStyle-HorizontalAlign="Right" DataFormatString="{0:N}"
                                                    HeaderStyle-Width="150px" HeaderText="Saldo Akhir" HeaderStyle-HorizontalAlign="Right" />
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
                </td>
            </tr>
            <tr>
                <td align="right" colspan="2">
                    <dxcp:ASPxCallbackPanel ID="cbpTotal" runat="server" Width="100%" ClientInstanceName="cbpTotal"
                        ShowLoadingPanel="false" OnCallback="cbpTotal_Callback">
                        <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ cbpView.PerformCallback('refresh'); }" />
                        <PanelCollection>
                            <dx:PanelContent ID="PanelContent2" runat="server">
                                <table>
                                    <colgroup>
                                        <col style="width: 120px;" />
                                        <col style="width: 150px;" />
                                    </colgroup>
                                    <tr>
                                        <td>
                                            <%=GetLabel("Total Debit") %>
                                        </td>
                                        <td>
                                            <asp:TextBox Width="100%" runat="server" ReadOnly="true" CssClass="number" ID="txtTotalBalanceDEBIT" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <%=GetLabel("Total Kredit") %>
                                        </td>
                                        <td>
                                            <asp:TextBox Width="100%" runat="server" ReadOnly="true" CssClass="number" ID="txtTotalBalanceCREDIT" />
                                        </td>
                                    </tr>
                                </table>
                            </dx:PanelContent>
                        </PanelCollection>
                    </dxcp:ASPxCallbackPanel>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
