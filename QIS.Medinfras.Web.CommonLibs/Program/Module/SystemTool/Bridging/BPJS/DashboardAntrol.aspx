<%@ Page Title="" Language="C#" MasterPageFile="~/Libs/MasterPage/MPTrx2.master"
    AutoEventWireup="true" CodeBehind="DashboardAntrol.aspx.cs" Inherits="QIS.Medinfras.Web.CommonLibs.Program.DashboardAntrol" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="QIS.Medinfras.Web.CustomControl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"
    Namespace="QIS.Medinfras.Web.CustomControl" TagPrefix="qis" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
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
    <script type="text/javascript" src='<%= ResolveUrl("~/Libs/Scripts/CustomGridViewList.js")%>'></script>
    <script type="text/javascript">
        $(function () {
            setDatePicker('<%=txtFromDate.ClientID %>');

            $('#<%=txtFromDate.ClientID %>').datepicker('option', 'maxDate', '0');

            $('#<%=btnRefresh.ClientID %>').click(function () {
                if (IsValid(null, 'fsMPEntry', 'mpEntry')) {
                    onRefreshGrid();
                }
            });

            $('#<%=rbDateFilter.ClientID %> input').change(function () {
                if ($(this).val() == "BULAN") {
                    $('#<%=trTanggal.ClientID %>').attr("style", "display:none");
                    $('#<%=trBulan.ClientID %>').removeAttr("style");
                    $('#<%=trTahun.ClientID %>').removeAttr("style");
                }
                else {
                    $('#<%=trTanggal.ClientID %>').removeAttr("style");
                    $('#<%=trBulan.ClientID %>').attr("style", "display:none");
                    $('#<%=trTahun.ClientID %>').attr("style", "display:none");
                }
            });
        });


        var convert = function (convert) {
            return $('<span />', { html: convert }).text();
        }

        function onGetCurrID() {
            return $('#<%=hdnID.ClientID %>').val();
        }

        function onGetFilterExpression() {
            return $('#<%=hdnFilterExpression.ClientID %>').val();
        }

        function onRefreshGrid() {
            cbpView.PerformCallback('refresh');
        }

        function onCbpViewEndCallback(s) {
            hideLoadingPanel();

            var param = s.cpResult.split('|');
            if (param[0] == 'refresh') {
                var pageCount = parseInt(param[1]);
                if (pageCount > 0) {
                }

                setPaging($("#paging"), pageCount, function (page) {
                    cbpView.PerformCallback('changepage|' + page);
                });
            }
        }
        //#endregion
    </script>
    <input type="hidden" value="" id="hdnModuleID" runat="server" />
    <input type="hidden" value="" id="hdnID" runat="server" />
    <input type="hidden" value="" id="hdnMRN" runat="server" />
    <input type="hidden" value="" id="hdnPageTitle" runat="server" />
    <input type="hidden" id="hdnFilterExpression" runat="server" value="" />
    <div style="position: relative;">
        <table tyle="width:60%;">
            <colgroup>
                <col style="width: 120px" />
                <col />
            </colgroup>
            <tr>
                <td class="tdLabel">
                    <label>
                        <%=GetLabel("Filter Dashboard")%></label>
                </td>
                <td colspan="2">
                    <asp:RadioButtonList ID="rbDateFilter" runat="server">
                        <asp:ListItem Text="Per Tanggal" Value="TANGGAL" Selected="True"></asp:ListItem>
                        <asp:ListItem Text="Per Bulan" Value="BULAN"></asp:ListItem>
                    </asp:RadioButtonList>
                </td>
            </tr>
            <tr id="trTanggal" runat="server" >
                <td class="tdLabel">
                    <label class="lblNormal">
                        <%=GetLabel("Tanggal")%></label>
                </td>
                <td>
                    <table cellpadding="0" cellspacing="0">
                        <tr>
                            <td>
                                <asp:TextBox runat="server" CssClass="datepicker" ID="txtFromDate" Width="120px" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr id="trBulan" runat="server" style="display:none">
                <td class="tdLabel">
                    <label class="lblNormal">
                        <%=GetLabel("Bulan")%></label>
                </td>
                <td colspan="2">
                    <dxe:ASPxComboBox ID="cboBulan" ClientInstanceName="cboBulan"
                        Width="75%" runat="server" />
                </td>
            </tr>
            <tr id="trTahun" runat="server" style="display:none">
                <td class="tdLabel">
                    <label class="lblNormal">
                        <%=GetLabel("Tahun")%></label>
                </td>
                <td colspan="2">
                    <dxe:ASPxComboBox ID="cboTahun" ClientInstanceName="cboTahun"
                        Width="75%" runat="server" />
                </td>
            </tr>
            <tr style="display:none">
                <td class="tdLabel">
                    <label>
                        <%=GetLabel("Waktu")%></label>
                </td>
                <td colspan="2">
                    <asp:RadioButtonList ID="rbTipeWaktu" runat="server">
                        <asp:ListItem Text="RS" Value="rs" Selected="True"></asp:ListItem>
                        <asp:ListItem Text="Server" Value="server"></asp:ListItem>
                    </asp:RadioButtonList>
                </td>
            </tr>
        </table>
        <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
            ShowLoadingPanel="false" OnCallback="cbpView_Callback">
            <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ onCbpViewEndCallback(s); }" />
            <PanelCollection>
                <dx:PanelContent ID="PanelContent1" runat="server">
                    <asp:Panel runat="server" ID="pnlView" CssClass="pnlContainerGrid">
                        <asp:GridView ID="grdView" runat="server" CssClass="grdSelected" AutoGenerateColumns="false"
                            ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                            <Columns>
                                <asp:BoundField DataField="tanggal" HeaderText="Tanggal" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="50px" />
                                <asp:BoundField DataField="kodepoli" HeaderText="Kode Poli" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="50px" />
                                <asp:BoundField DataField="namapoli" HeaderText="Nama Poli" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="50px" />
                                <asp:BoundField DataField="waktu_task1" HeaderText="Waktu Task 1" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="50px" />
                                <asp:BoundField DataField="waktu_task2" HeaderText="Waktu Task 2" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="50px" />
                                <asp:BoundField DataField="waktu_task3" HeaderText="Waktu Task 3" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="50px" />
                                <asp:BoundField DataField="waktu_task4" HeaderText="Waktu Task 4" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="50px" />
                                <asp:BoundField DataField="waktu_task5" HeaderText="Waktu Task 5" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="50px" />
                                <asp:BoundField DataField="waktu_task6" HeaderText="Waktu Task 6" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="50px" />
                                <asp:BoundField DataField="avg_waktu_task1" HeaderText="Rata-Rata Waktu Task 1" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="50px" />
                                <asp:BoundField DataField="avg_waktu_task2" HeaderText="Rata-Rata Waktu Task 2" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="50px" />
                                <asp:BoundField DataField="avg_waktu_task3" HeaderText="Rata-Rata Waktu Task 3" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="50px" />
                                <asp:BoundField DataField="avg_waktu_task4" HeaderText="Rata-Rata Waktu Task 4" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="50px" />
                                <asp:BoundField DataField="avg_waktu_task5" HeaderText="Rata-Rata Waktu Task 5" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="50px" />
                                <asp:BoundField DataField="avg_waktu_task6" HeaderText="Rata-Rata Waktu Task 6" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="50px" />
                            </Columns>
                            <EmptyDataTemplate>
                                <%=GetLabel("Tidak ada record")%>
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
</asp:Content>
