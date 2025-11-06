<%@ Page Title="" Language="C#" MasterPageFile="~/Libs/MasterPage/MPTrx2.master"
    AutoEventWireup="true" CodeBehind="InterfaceJournalStatusInformation.aspx.cs"
    Inherits="QIS.Medinfras.Web.Accounting.Program.InterfaceJournalStatusInformation" %>

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
                    <div style="height: 450px; overflow-y: auto; overflow-x: auto">
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
                                                            <label>
                                                                <b><i>
                                                                    <%=GetLabel("JournalGroup : ")%></i></b><br />
                                                                <%#: Eval("JournalGroup")%></label>
                                                            <br />
                                                            <label>
                                                                <b><i>
                                                                    <%=GetLabel("JournalType : ")%></i></b><br />
                                                                <%#: Eval("TypeName")%></label>
                                                        </div>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="cfTgl_1_inString" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="30px"
                                                    HeaderText="Tgl_1" HeaderStyle-HorizontalAlign="Center" />
                                                <asp:BoundField DataField="cfTgl_2_inString" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="30px"
                                                    HeaderText="Tgl_2" HeaderStyle-HorizontalAlign="Center" />
                                                <asp:BoundField DataField="cfTgl_3_inString" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="30px"
                                                    HeaderText="Tgl_3" HeaderStyle-HorizontalAlign="Center" />
                                                <asp:BoundField DataField="cfTgl_4_inString" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="30px"
                                                    HeaderText="Tgl_4" HeaderStyle-HorizontalAlign="Center" />
                                                <asp:BoundField DataField="cfTgl_5_inString" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="30px"
                                                    HeaderText="Tgl_5" HeaderStyle-HorizontalAlign="Center" />
                                                <asp:BoundField DataField="cfTgl_6_inString" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="30px"
                                                    HeaderText="Tgl_6" HeaderStyle-HorizontalAlign="Center" />
                                                <asp:BoundField DataField="cfTgl_7_inString" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="30px"
                                                    HeaderText="Tgl_7" HeaderStyle-HorizontalAlign="Center" />
                                                <asp:BoundField DataField="cfTgl_8_inString" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="30px"
                                                    HeaderText="Tgl_8" HeaderStyle-HorizontalAlign="Center" />
                                                <asp:BoundField DataField="cfTgl_9_inString" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="30px"
                                                    HeaderText="Tgl_9" HeaderStyle-HorizontalAlign="Center" />
                                                <asp:BoundField DataField="cfTgl_10_inString" ItemStyle-HorizontalAlign="Center"
                                                    HeaderStyle-Width="30px" HeaderText="Tgl_10" HeaderStyle-HorizontalAlign="Center" />
                                                <asp:BoundField DataField="cfTgl_11_inString" ItemStyle-HorizontalAlign="Center"
                                                    HeaderStyle-Width="30px" HeaderText="Tgl_11" HeaderStyle-HorizontalAlign="Center" />
                                                <asp:BoundField DataField="cfTgl_12_inString" ItemStyle-HorizontalAlign="Center"
                                                    HeaderStyle-Width="30px" HeaderText="Tgl_12" HeaderStyle-HorizontalAlign="Center" />
                                                <asp:BoundField DataField="cfTgl_13_inString" ItemStyle-HorizontalAlign="Center"
                                                    HeaderStyle-Width="30px" HeaderText="Tgl_13" HeaderStyle-HorizontalAlign="Center" />
                                                <asp:BoundField DataField="cfTgl_14_inString" ItemStyle-HorizontalAlign="Center"
                                                    HeaderStyle-Width="30px" HeaderText="Tgl_14" HeaderStyle-HorizontalAlign="Center" />
                                                <asp:BoundField DataField="cfTgl_15_inString" ItemStyle-HorizontalAlign="Center"
                                                    HeaderStyle-Width="30px" HeaderText="Tgl_15" HeaderStyle-HorizontalAlign="Center" />
                                                <asp:BoundField DataField="cfTgl_16_inString" ItemStyle-HorizontalAlign="Center"
                                                    HeaderStyle-Width="30px" HeaderText="Tgl_16" HeaderStyle-HorizontalAlign="Center" />
                                                <asp:BoundField DataField="cfTgl_17_inString" ItemStyle-HorizontalAlign="Center"
                                                    HeaderStyle-Width="30px" HeaderText="Tgl_17" HeaderStyle-HorizontalAlign="Center" />
                                                <asp:BoundField DataField="cfTgl_18_inString" ItemStyle-HorizontalAlign="Center"
                                                    HeaderStyle-Width="30px" HeaderText="Tgl_18" HeaderStyle-HorizontalAlign="Center" />
                                                <asp:BoundField DataField="cfTgl_19_inString" ItemStyle-HorizontalAlign="Center"
                                                    HeaderStyle-Width="30px" HeaderText="Tgl_19" HeaderStyle-HorizontalAlign="Center" />
                                                <asp:BoundField DataField="cfTgl_20_inString" ItemStyle-HorizontalAlign="Center"
                                                    HeaderStyle-Width="30px" HeaderText="Tgl_20" HeaderStyle-HorizontalAlign="Center" />
                                                <asp:BoundField DataField="cfTgl_21_inString" ItemStyle-HorizontalAlign="Center"
                                                    HeaderStyle-Width="30px" HeaderText="Tgl_21" HeaderStyle-HorizontalAlign="Center" />
                                                <asp:BoundField DataField="cfTgl_22_inString" ItemStyle-HorizontalAlign="Center"
                                                    HeaderStyle-Width="30px" HeaderText="Tgl_22" HeaderStyle-HorizontalAlign="Center" />
                                                <asp:BoundField DataField="cfTgl_23_inString" ItemStyle-HorizontalAlign="Center"
                                                    HeaderStyle-Width="30px" HeaderText="Tgl_23" HeaderStyle-HorizontalAlign="Center" />
                                                <asp:BoundField DataField="cfTgl_24_inString" ItemStyle-HorizontalAlign="Center"
                                                    HeaderStyle-Width="30px" HeaderText="Tgl_24" HeaderStyle-HorizontalAlign="Center" />
                                                <asp:BoundField DataField="cfTgl_25_inString" ItemStyle-HorizontalAlign="Center"
                                                    HeaderStyle-Width="30px" HeaderText="Tgl_25" HeaderStyle-HorizontalAlign="Center" />
                                                <asp:BoundField DataField="cfTgl_26_inString" ItemStyle-HorizontalAlign="Center"
                                                    HeaderStyle-Width="30px" HeaderText="Tgl_26" HeaderStyle-HorizontalAlign="Center" />
                                                <asp:BoundField DataField="cfTgl_27_inString" ItemStyle-HorizontalAlign="Center"
                                                    HeaderStyle-Width="30px" HeaderText="Tgl_27" HeaderStyle-HorizontalAlign="Center" />
                                                <asp:BoundField DataField="cfTgl_28_inString" ItemStyle-HorizontalAlign="Center"
                                                    HeaderStyle-Width="30px" HeaderText="Tgl_28" HeaderStyle-HorizontalAlign="Center" />
                                                <asp:BoundField DataField="cfTgl_29_inString" ItemStyle-HorizontalAlign="Center"
                                                    HeaderStyle-Width="30px" HeaderText="Tgl_29" HeaderStyle-HorizontalAlign="Center" />
                                                <asp:BoundField DataField="cfTgl_30_inString" ItemStyle-HorizontalAlign="Center"
                                                    HeaderStyle-Width="30px" HeaderText="Tgl_30" HeaderStyle-HorizontalAlign="Center" />
                                                <asp:BoundField DataField="cfTgl_31_inString" ItemStyle-HorizontalAlign="Center"
                                                    HeaderStyle-Width="30px" HeaderText="Tgl_31" HeaderStyle-HorizontalAlign="Center" />
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
                    <div>
                        <table width="100%">
                            <tr>
                                <td>
                                    <div style="width: 50%;">
                                        <div class="lblComponent" style="text-align: center; padding-left: 3px">
                                            <b>
                                                <%=GetLabel("Status Information") %></div>
                                        </b>
                                        <div style="background-color: #EAEAEA;">
                                            <table width="100%" cellpadding="0" cellspacing="0">
                                                <colgroup>
                                                    <col width="30px" />
                                                    <col width="10px" />
                                                    <col />
                                                </colgroup>
                                                <tr>
                                                    <td align="center">
                                                        <%=GetLabel("0") %>
                                                    </td>
                                                    <td align="center">
                                                        <%=GetLabel(":") %>
                                                    </td>
                                                    <td>
                                                        <div runat="server" id="divStatus0">
                                                        </div>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="center">
                                                        <%=GetLabel("1") %>
                                                    </td>
                                                    <td align="center">
                                                        <%=GetLabel(":") %>
                                                    </td>
                                                    <td>
                                                        <div runat="server" id="divStatus1">
                                                        </div>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="center">
                                                        <%=GetLabel("2") %>
                                                    </td>
                                                    <td align="center">
                                                        <%=GetLabel(":") %>
                                                    </td>
                                                    <td>
                                                        <div runat="server" id="divStatus2">
                                                        </div>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="center">
                                                        <%=GetLabel("3")%>
                                                    </td>
                                                    <td align="center">
                                                        <%=GetLabel(":") %>
                                                    </td>
                                                    <td>
                                                        <div runat="server" id="divStatus3">
                                                        </div>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="center">
                                                        <%=GetLabel("4")%>
                                                    </td>
                                                    <td align="center">
                                                        <%=GetLabel(":") %>
                                                    </td>
                                                    <td>
                                                        <div runat="server" id="divStatus4">
                                                        </div>
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </div>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
