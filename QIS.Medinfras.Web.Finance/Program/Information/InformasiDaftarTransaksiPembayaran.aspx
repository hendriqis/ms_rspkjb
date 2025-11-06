<%@ Page Title="" Language="C#" MasterPageFile="~/Libs/MasterPage/MPTrx2.master"
    AutoEventWireup="true" CodeBehind="InformasiDaftarTransaksiPembayaran.aspx.cs"
    Inherits="QIS.Medinfras.Web.Finance.Program.InformasiDaftarTransaksiPembayaran" %>

<%@ Register Assembly="QIS.Medinfras.Web.CustomControl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"
    Namespace="QIS.Medinfras.Web.CustomControl" TagPrefix="qis" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<asp:Content ID="Content3" ContentPlaceHolderID="plhCustomButtonToolbar" runat="server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div class="menuTitle">
        <%=HttpUtility.HtmlEncode(GetPageTitle())%></div>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript">
        $(function () {
            setDatePicker('<%=txtPeriodFrom.ClientID %>');
            setDatePicker('<%=txtPeriodTo.ClientID %>');
        });

        //#region Paging
        var pageCount = parseInt('<%=PageCount %>');
        $(function () {
            setPaging($("#paging"), pageCount, function (page) {
                cbpView.PerformCallback('changepage|' + page);
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
        //#endregion

        $('#btnRefresh').live('click', function () {
            cbpView.PerformCallback('refresh');
        });

        function onRefreshGrid() {
            $('#<%=hdnFilterExpressionQuickSearch.ClientID %>').val(txtSearchView.GenerateFilterExpression());
            cbpView.PerformCallback('refresh');
        }

        function onTxtSearchViewSearchClick(s) {
            setTimeout(function () {
                s.SetBlur();
                onRefreshGrid();
            }, 0);
        }

        $('.lblDetail').die('click');
        $('.lblDetail').live('click', function () {
            $tr = $(this).closest('tr');
            var id = $tr.find('.keyField').html();

            var url = ResolveUrl("~/Program/Information/InformasiDaftarTransaksiPembayaranDetailCtl.ascx");
            openUserControlPopup(url, id, 'Informasi Daftar Transaksi vs Pembayaran Detail', 1200, 400);
        });
    </script>
    <div>
        <input type="hidden" value="" id="hdnFilterExpressionQuickSearch" runat="server" />
        <input type="hidden" value="" id="hdnPageTitle" runat="server" />
        <input type="hidden" value="" id="hdnRegistrationID" runat="server" />
        <table class="tblContentArea">
            <colgroup>
                <col style="width: 50%" />
                <col style="width: 50%" />
            </colgroup>
            <tr>
                <td>
                    <table>
                        <colgroup>
                            <col style="width: 200px" />
                            <col style="width: 100px" />
                            <col style="width: 350px" />
                            <col />
                        </colgroup>
                        <tr>
                            <td>
                                <label class="lblNormal">
                                    <%=GetLabel("Department Transaksi") %></label>
                            </td>
                            <td colspan="3">
                                <table width="100%" cellspacing="0" cellpadding="0">
                                    <tr>
                                        <td colspan="3">
                                            <dxe:ASPxComboBox ID="cboDepartment" ClientInstanceName="cboDepartment" Width="90%"
                                                runat="server">
                                            </dxe:ASPxComboBox>
                                        </td>
                                        <td>
                                            <asp:CheckBox runat="server" ID="chkIsFilter" Text="Is Exclusion?" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Periode")%></label>
                            </td>
                            <td colspan="3">
                                <table width="100%" cellspacing="0" cellpadding="0">
                                    <tr>
                                        <td>
                                            <dxe:ASPxComboBox ID="cboPeriodeType" ClientInstanceName="cboPeriodeType" Width="95%"
                                                runat="server" />
                                        </td>
                                        <td>
                                            <table cellpadding="0" cellspacing="0">
                                                <tr>
                                                    <td>
                                                        <asp:TextBox runat="server" Width="100px" ID="txtPeriodFrom" CssClass="datepicker" />
                                                    </td>
                                                    <td style="width: 30px; text-align: center">
                                                        s/d
                                                    </td>
                                                    <td>
                                                        <asp:TextBox runat="server" Width="100px" ID="txtPeriodTo" CssClass="datepicker" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Status Registrasi")%></label>
                            </td>
                            <td colspan="3">
                                <dxe:ASPxComboBox ID="cboRegistrastionStatus" ClientInstanceName="cboRegistrastionStatus"
                                    Width="50%" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                            </td>
                            <td colspan="2" style="padding-top: 10px;">
                                <input type="button" id="btnRefresh" value="R e f r e s h" class="btnRefresh w3-button w3-blue w3-border w3-border-blue w3-round-large" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td colspan="2">
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
                                            <asp:BoundField DataField="RegistrationID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                            <asp:BoundField DataField="RegistrationNo" ItemStyle-HorizontalAlign="Left" HeaderText="Registration No"
                                                HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="100px" />
                                            <asp:BoundField DataField="cfRegistrationDateInString" ItemStyle-HorizontalAlign="Center"
                                                HeaderText="Registration Date" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="70px" />
                                            <asp:BoundField DataField="cfDischargeDateInString" ItemStyle-HorizontalAlign="Center"
                                                HeaderText="Discharge Date" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="70px" />
                                            <asp:BoundField DataField="LinkedRegistrationNo" ItemStyle-HorizontalAlign="Left"
                                                HeaderText="Linked Registration No" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="100px" />
                                            <asp:BoundField DataField="LinkedToRegistrationNo" ItemStyle-HorizontalAlign="Left"
                                                HeaderText="Linked To Registration No" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="100px" />
                                            <asp:BoundField DataField="DepartmentID" ItemStyle-HorizontalAlign="Left" HeaderText="Department"
                                                HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="100px" />
                                            <asp:BoundField DataField="ServiceUnitName" ItemStyle-HorizontalAlign="Left" HeaderText="Service Unit"
                                                HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="130px" />
                                            <asp:BoundField DataField="RegistrationStatus" ItemStyle-HorizontalAlign="Center" HeaderText="Registration Status"
                                                HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="80px" />
                                            <asp:TemplateField ItemStyle-HorizontalAlign="center" HeaderStyle-Width="60px">
                                                <ItemTemplate>
                                                    <input type="hidden" value='<%#:Eval("RegistrationID") %>' class="hdnRegistrationID" />
                                                    <label class="lblLink lblDetail">
                                                        <%=GetLabel("Detail") %></label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <EmptyDataTemplate>
                                            <%=GetLabel("Tidak ada informasi saldo persediaan di lokasi ini")%>
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
    </div>
</asp:Content>
