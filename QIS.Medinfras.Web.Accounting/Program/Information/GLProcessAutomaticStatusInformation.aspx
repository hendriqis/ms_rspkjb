<%@ Page Title="" Language="C#" MasterPageFile="~/Libs/MasterPage/MPTrx2.master"
    AutoEventWireup="true" CodeBehind="GLProcessAutomaticStatusInformation.aspx.cs"
    Inherits="QIS.Medinfras.Web.Accounting.Program.GLProcessAutomaticStatusInformation" %>

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
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                    <div style="height: 500px; overflow-y: auto; overflow-x: auto">
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
                                                <asp:TemplateField HeaderStyle-Width="150px" ItemStyle-Width="150px" HeaderStyle-HorizontalAlign="Left">
                                                    <HeaderTemplate>
                                                        <div style="padding-left: 3px">
                                                            <%=GetLabel("Information")%>
                                                        </div>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <div>
                                                            <label>
                                                                <b><i>
                                                                    <%=GetLabel("TransactionType : ")%></i></b><br />
                                                                <%#: Eval("TransactionName")%></label>
                                                            <br />
                                                        </div>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="Tgl01" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="30px"
                                                    HeaderText="Tgl01" HeaderStyle-HorizontalAlign="Center" />
                                                <asp:BoundField DataField="Tgl02" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="30px"
                                                    HeaderText="Tgl02" HeaderStyle-HorizontalAlign="Center" />
                                                <asp:BoundField DataField="Tgl03" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="30px"
                                                    HeaderText="Tgl03" HeaderStyle-HorizontalAlign="Center" />
                                                <asp:BoundField DataField="Tgl04" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="30px"
                                                    HeaderText="Tgl04" HeaderStyle-HorizontalAlign="Center" />
                                                <asp:BoundField DataField="Tgl05" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="30px"
                                                    HeaderText="Tgl05" HeaderStyle-HorizontalAlign="Center" />
                                                <asp:BoundField DataField="Tgl06" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="30px"
                                                    HeaderText="Tgl06" HeaderStyle-HorizontalAlign="Center" />
                                                <asp:BoundField DataField="Tgl07" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="30px"
                                                    HeaderText="Tgl07" HeaderStyle-HorizontalAlign="Center" />
                                                <asp:BoundField DataField="Tgl08" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="30px"
                                                    HeaderText="Tgl08" HeaderStyle-HorizontalAlign="Center" />
                                                <asp:BoundField DataField="Tgl09" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="30px"
                                                    HeaderText="Tgl09" HeaderStyle-HorizontalAlign="Center" />
                                                <asp:BoundField DataField="Tgl10" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="30px"
                                                    HeaderText="Tgl10" HeaderStyle-HorizontalAlign="Center" />
                                                <asp:BoundField DataField="Tgl11" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="30px"
                                                    HeaderText="Tgl11" HeaderStyle-HorizontalAlign="Center" />
                                                <asp:BoundField DataField="Tgl12" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="30px"
                                                    HeaderText="Tgl12" HeaderStyle-HorizontalAlign="Center" />
                                                <asp:BoundField DataField="Tgl13" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="30px"
                                                    HeaderText="Tgl13" HeaderStyle-HorizontalAlign="Center" />
                                                <asp:BoundField DataField="Tgl14" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="30px"
                                                    HeaderText="Tgl14" HeaderStyle-HorizontalAlign="Center" />
                                                <asp:BoundField DataField="Tgl15" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="30px"
                                                    HeaderText="Tgl15" HeaderStyle-HorizontalAlign="Center" />
                                                <asp:BoundField DataField="Tgl16" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="30px"
                                                    HeaderText="Tgl16" HeaderStyle-HorizontalAlign="Center" />
                                                <asp:BoundField DataField="Tgl17" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="30px"
                                                    HeaderText="Tgl17" HeaderStyle-HorizontalAlign="Center" />
                                                <asp:BoundField DataField="Tgl18" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="30px"
                                                    HeaderText="Tgl18" HeaderStyle-HorizontalAlign="Center" />
                                                <asp:BoundField DataField="Tgl19" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="30px"
                                                    HeaderText="Tgl19" HeaderStyle-HorizontalAlign="Center" />
                                                <asp:BoundField DataField="Tgl20" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="30px"
                                                    HeaderText="Tgl20" HeaderStyle-HorizontalAlign="Center" />
                                                <asp:BoundField DataField="Tgl21" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="30px"
                                                    HeaderText="Tgl21" HeaderStyle-HorizontalAlign="Center" />
                                                <asp:BoundField DataField="Tgl22" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="30px"
                                                    HeaderText="Tgl22" HeaderStyle-HorizontalAlign="Center" />
                                                <asp:BoundField DataField="Tgl23" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="30px"
                                                    HeaderText="Tgl23" HeaderStyle-HorizontalAlign="Center" />
                                                <asp:BoundField DataField="Tgl24" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="30px"
                                                    HeaderText="Tgl24" HeaderStyle-HorizontalAlign="Center" />
                                                <asp:BoundField DataField="Tgl25" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="30px"
                                                    HeaderText="Tgl25" HeaderStyle-HorizontalAlign="Center" />
                                                <asp:BoundField DataField="Tgl26" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="30px"
                                                    HeaderText="Tgl26" HeaderStyle-HorizontalAlign="Center" />
                                                <asp:BoundField DataField="Tgl27" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="30px"
                                                    HeaderText="Tgl27" HeaderStyle-HorizontalAlign="Center" />
                                                <asp:BoundField DataField="Tgl28" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="30px"
                                                    HeaderText="Tgl28" HeaderStyle-HorizontalAlign="Center" />
                                                <asp:BoundField DataField="Tgl29" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="30px"
                                                    HeaderText="Tgl29" HeaderStyle-HorizontalAlign="Center" />
                                                <asp:BoundField DataField="Tgl30" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="30px"
                                                    HeaderText="Tgl30" HeaderStyle-HorizontalAlign="Center" />
                                                <asp:BoundField DataField="Tgl31" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="30px"
                                                    HeaderText="Tgl31" HeaderStyle-HorizontalAlign="Center" />
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
        </table>
    </div>
</asp:Content>
