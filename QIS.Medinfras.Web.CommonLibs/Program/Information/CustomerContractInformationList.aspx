<%@ Page Title="" Language="C#" MasterPageFile="~/Libs/MasterPage/MPTrx2.master"
    AutoEventWireup="true" CodeBehind="CustomerContractInformationList.aspx.cs" Inherits="QIS.Medinfras.Web.CommonLibs.Program.CustomerContractInformationList" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="QIS.Medinfras.Web.CustomControl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"
    Namespace="QIS.Medinfras.Web.CustomControl" TagPrefix="qis" %>
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
            $('#<%=btnRefresh.ClientID %>').click(function () {
                cbpView.PerformCallback('refresh');
            });

            setDatePicker('<%=txtEndDateContract.ClientID %>');
            $('#<%:trContractMonth.ClientID %>').attr('style', 'display:none');

            $('#<%=txtEndDateContract.ClientID %>').datepicker('option', 'maxDate', '2000');

            $('#lblRefresh.lblLink').click(function (evt) {
                cbpView.PerformCallback('refresh');
            });
        });


        $('#<%=rblDataSource.ClientID %>').live('change', function () {
            var displayOption = $('#<%=rblDataSource.ClientID %>').find(":checked").val();
            if (displayOption == "filterMonth") { //Per Bulan
                $('#<%:trContractDate.ClientID %>').attr('style', 'display:none');
                $('#<%:trContractMonth.ClientID %>').removeAttr('style');
//                cbpView.PerformCallback('refresh');
            } else {
                $('#<%:trContractMonth.ClientID %>').attr('style', 'display:none');
                $('#<%:trContractDate.ClientID %>').removeAttr('style');
//                cbpView.PerformCallback('refresh');
            }
        });

//        $('#<%=cboContractMonth.ClientID %>').live('change', function (evt) {
//            cbpView.PerformCallback('refresh');
//        });

        $('#<%=chkEndDate.ClientID %>').die();
        $('#<%=chkEndDate.ClientID %>').live('change', function () {
            if ($(this).is(':checked')) {
                $('#<%=txtEndDateContract.ClientID %>').attr('readonly', 'readonly');
            }
            else $('#<%=txtEndDateContract.ClientID %>').removeAttr('readonly');
//            cbpView.PerformCallback('refresh');
        });


//        $('#<%=txtEndDateContract.ClientID %>').live('change', function (evt) {
//            cbpView.PerformCallback('refresh');
//        });

        //#region Paging
        var pageCountAvailable = parseInt('<%=PageCount %>');
        $(function () {
            setPagingDetailItem(pageCountAvailable);
        });

        function setPagingDetailItem(pageCount) {
            if (pageCount > 0)
                $('#<%=grdView.ClientID %> tr:eq(1)').click();
            setPaging($("#paging"), pageCount, function (page) {
                cbpView.PerformCallback('changepage|' + page);
            }, 8);
        }

        function onCbpViewEndCallback(s) {
            var param = s.cpResult.split('|');
            if (param[0] == 'refresh') {
                var pageCount = parseInt(param[1]);
                setPagingDetailItem(pageCount);
            }
            else {
                $('#<%=grdView.ClientID %> tr:eq(1)').click();
            }
            hideLoadingPanel();
        }
        //#endregion

        $('.lnkContractSummary a').live('click', function () {
            var id = $(this).closest('tr').find('.keyField').html();
            var url = ResolveUrl("~/Libs/Program/Information/CustomerContractSummaryViewCtl.ascx");
            openUserControlPopup(url, id, 'Ringkasan Kontrak', 700, 600);
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

//        function onCboTypeValueChanged() {
//            onRefreshGrid();
//        }

//        function onCboDepartmentValueChanged() {
//            onRefreshGrid();
//        }

//        function onCboContractMonthChanged() {
//            cbpView.PerformCallback('refresh');
//        }

    </script>
    <input type="hidden" id="hdnModuleID" runat="server" value="" />
    <input type="hidden" value="" id="hdnPageTitle" runat="server" />
    <input type="hidden" value="" id="hdnFilterExpressionQuickSearch" runat="server" />
    <table class="tblContentArea" style="width: 100%">
        <tr>
            <table class="tblEntryContent" style="width: 700px;">
                <td style="padding: 5px; vertical-align: top">
                    <table>
                        <colgroup>
                            <col style="width: 150px">
                        </colgroup>
                        <td class="tdLabel">
                            <div style="position: relative;">
                                <label class="lblNormal lblMandatory">
                                    <%:GetLabel("Filter")%></label></div>
                        </td>
                        <td colspan="2">
                            <asp:RadioButtonList ID="rblDataSource" runat="server" RepeatDirection="Horizontal">
                                <asp:ListItem Text="Per Tanggal" Value="filterDate" Selected="True" />
                                <asp:ListItem Text="Per Bulan" Value="filterMonth" />
                            </asp:RadioButtonList>
                        </td>
                        <tr id="trContractDate" runat="server">
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Tanggal Akhir Kontrak")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtEndDateContract" CssClass="datepicker" Width="120px" runat="server" />
                                <asp:CheckBox ID="chkEndDate" runat="server" Checked="false" /><%:GetLabel("Abaikan Tanggal")%>&nbsp;
                            </td>
                        </tr>
                        <tr id="trContractMonth" runat="server">
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Bulan Akhir Kontrak")%></label>
                            </td>
                            <td>
                                <dxe:ASPxComboBox ID="cboContractMonth" ClientInstanceName="cboContractMonth" runat="server"
                                    Width="50px">
                                    <%--<ClientSideEvents ValueChanged="function(s,e) { onCboContractMonthChanged(); }" />--%>
                                </dxe:ASPxComboBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label>
                                    <%=GetLabel("Quick Filter")%></label>
                            </td>
                            <td>
                                <qis:QISIntellisenseTextBox runat="server" ClientInstanceName="txtSearchView" ID="txtSearchView"
                                    Width="300px" Watermark="Search">
                                    <ClientSideEvents SearchClick="function(s){ onTxtSearchViewSearchClick(s); }" />
                                    <IntellisenseHints>
                                        <qis:QISIntellisenseHint Text="Kode Instansi" FieldName="BusinessPartnerCode" />
                                        <qis:QISIntellisenseHint Text="Nama Instansi" FieldName="BusinessPartnerName" />
                                    </IntellisenseHints>
                                </qis:QISIntellisenseTextBox>
                            </td>
                        </tr>
                    </table>
                </td>
            </table>
        </tr>
        <tr>
            <td colspan="2">
                <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
                    ShowLoadingPanel="false" OnCallback="cbpView_Callback">
                    <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ onCbpViewEndCallback(s); }" />
                    <PanelCollection>
                        <dx:PanelContent ID="PanelContent1" runat="server">
                            <asp:Panel runat="server" ID="pnlView" CssClass="pnlContainerGrid" Style="width: 100%;
                                margin-left: auto; margin-right: auto; position: relative; font-size: 0.95em;">
                                <input type="hidden" id="hdnFilterExpression" value="" runat="server" />
                                <asp:GridView ID="grdView" runat="server" CssClass="grdSelected" AutoGenerateColumns="false"
                                    ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                    <Columns>
                                        <asp:BoundField DataField="ContractID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                        <asp:BoundField DataField="BusinessPartnerCode" HeaderText="Kode Instansi" HeaderStyle-Width="50"
                                            HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                                        <asp:BoundField DataField="BusinessPartnerName" HeaderText="Instansi" HeaderStyle-Width="250px"
                                            HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                                        <asp:BoundField DataField="ContractNo" HeaderText="No. Kontrak" HeaderStyle-Width="150px"
                                            HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                                        <asp:BoundField DataField="cfStartDateInString" HeaderText="Tanggal Mulai" HeaderStyle-HorizontalAlign="Center"
                                            ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="50px" />
                                        <asp:BoundField DataField="cfEndDateInString" HeaderText="Tanggal Akhir" HeaderStyle-HorizontalAlign="Center"
                                            ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="50px" />
                                        <asp:HyperLinkField HeaderText="Ringkasan Kontrak" Text="Ringkasan Kontrak" ItemStyle-HorizontalAlign="Center"
                                            ItemStyle-CssClass="lnkContractSummary" HeaderStyle-Width="150px" HeaderStyle-HorizontalAlign="Center" />
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
