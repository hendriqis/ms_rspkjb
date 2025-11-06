<%@ Page Language="C#" MasterPageFile="~/Libs/MasterPage/MPList.master" AutoEventWireup="true"
    CodeBehind="InformasiHutangSupplier.aspx.cs" Inherits="QIS.Medinfras.Web.Finance.Program.InformasiHutangSupplier" %>

<%@ Register Assembly="QIS.Medinfras.Web.CustomControl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"
    Namespace="QIS.Medinfras.Web.CustomControl" TagPrefix="qis" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPopupControl" TagPrefix="dxpc" %>
<asp:Content ID="Content3" ContentPlaceHolderID="plhCustomButtonToolbar" runat="server">
    <li id="btnRefresh" crudmode="R" runat="server">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbrefresh.png")%>' alt="" /><br style="clear: both" />
        <div>
            <%=GetLabel("Refresh")%></div>
    </li>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhList" runat="server">
    <script type="text/javascript">
        $(function () {
            setDatePicker('<%=txtDateFrom.ClientID %>');
            setDatePicker('<%=txtDateTo.ClientID %>');

            $('#<%=btnRefresh.ClientID %>').live('click', function () {
                if (IsValid(null, 'fsMPEntry', 'mpEntry')) {
                    cbpView.PerformCallback('refresh');
                }
            });

            //#region Supplier
            function getSupplierFilterExpression() {
                var filterExpression = "<%:filterExpressionSupplier %>";
                return filterExpression;
            }

            $('#<%=lblSupplier.ClientID %>.lblLink').live('click', function () {
                openSearchDialog('businesspartners', getSupplierFilterExpression(), function (value) {
                    $('#<%=txtSupplierCode.ClientID %>').val(value);
                    onTxtSupplierChanged(value);
                });
            });

            $('#<%=txtSupplierCode.ClientID %>').change(function () {
                onTxtSupplierChanged($(this).val());
            });

            function onTxtSupplierChanged(value) {
                var filterExpression = getSupplierFilterExpression() + " AND BusinessPartnerCode = '" + value + "'";
                Methods.getObject('GetBusinessPartnersList', filterExpression, function (result) {
                    if (result != null) {
                        $('#<%=hdnSupplierID.ClientID %>').val(result.BusinessPartnerID);
                        $('#<%=txtSupplierCode.ClientID %>').val(result.BusinessPartnerCode);
                        $('#<%=txtSupplierName.ClientID %>').val(result.BusinessPartnerName);
                    }
                    else {
                        $('#<%=hdnSupplierID.ClientID %>').val('');
                        $('#<%=txtSupplierCode.ClientID %>').val('');
                        $('#<%=txtSupplierName.ClientID %>').val('');
                        cboTerm.SetValue('');
                    }
                });
            }

            //#endregion
        })

        //#region Paging
        var pageCount = parseInt('<%=PageCount %>');
        $(function () {
            Methods.checkImageError('imgPatientImage', 'patient', 'hdnPatientGender');
            setPaging($("#paging"), pageCount, function (page) {
                cbpView.PerformCallback('changepage|' + page);
            });
        });

        function onCbpViewEndCallback(s) {
            hideLoadingPanel();
            Methods.checkImageError('imgPatientImage', 'patient', 'hdnPatientGender');
            var param = s.cpResult.split('|');
            if (param[0] == 'refresh') {
                var pageCount = parseInt(param[1]);
                setPaging($("#paging"), pageCount, function (page) {
                    cbpView.PerformCallback('changepage|' + page);
                });
            }
        }
        //#endregion

        $('.lblBusinessPartnerName').live('click', function () {
            $tr = $(this).closest('tr');
            var businessPartnerID = $tr.find('.hdnBusinessPartnerID').val();
            var url = ResolveUrl("~/Program/Information/InformasiHutangSupplierCtl.ascx");
            openUserControlPopup(url, businessPartnerID, 'Detail Information', 1200, 550);
        });

        $('.lbl0_30').live('click', function () {
            $tr = $(this).closest('tr');
            var businessPartnerID = $tr.find('.hdnBusinessPartnerID').val();
            var url = ResolveUrl("~/Program/Information/PurchaseInvoiceInformationDtCtl.ascx");
            var param = businessPartnerID + '|' + 0 + '|' + 30;
            openUserControlPopup(url, param, 'Detail Information', 1200, 550);
        });

        $('.lbl30_60').live('click', function () {
            $tr = $(this).closest('tr');
            var businessPartnerID = $tr.find('.hdnBusinessPartnerID').val();
            var url = ResolveUrl("~/Program/Information/PurchaseInvoiceInformationDtCtl.ascx");
            var param = businessPartnerID + '|' + 30 + '|' + 60;
            openUserControlPopup(url, param, 'Detail Information', 1200, 550);
        });

        $('.lbl60_90').live('click', function () {
            $tr = $(this).closest('tr');
            var businessPartnerID = $tr.find('.hdnBusinessPartnerID').val();
            var url = ResolveUrl("~/Program/Information/PurchaseInvoiceInformationDtCtl.ascx");
            var param = businessPartnerID + '|' + 60 + '|' + 90;
            openUserControlPopup(url, param, 'Detail Information', 1200, 550);
        });

        $('.lbl90').live('click', function () {
            $tr = $(this).closest('tr');
            var businessPartnerID = $tr.find('.hdnBusinessPartnerID').val();
            var url = ResolveUrl("~/Program/Information/PurchaseInvoiceInformationDtCtl.ascx");
            var param = businessPartnerID + '|' + 90 + '|' + 0;
            openUserControlPopup(url, param, 'Detail Information', 1200, 550);
        });
    </script>
    <input type="hidden" value="" id="hdnListClassID" runat="server" />
    <input type="hidden" value="" id="hdnListClassName" runat="server" />
    <input type="hidden" value="" id="hdnFilterExpressionQuickSearch" runat="server" />
    <div>
        <table style="width: 100%">
            <tr>
                <td>
                    <table style="width: 50%">
                        <tr>
                            <td>
                                <label>
                                    <%=GetLabel("Tanggal") %></label>
                            </td>
                            <td>
                                <table cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td>
                                            <asp:TextBox runat="server" CssClass="datepicker" ID="txtDateFrom" Width="120px" />
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            <asp:TextBox runat="server" CssClass="datepicker" ID="txtDateTo" Width="120px" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblLink" id="lblSupplier" runat="server">
                                    <%=GetLabel("Supplier")%></label>
                            </td>
                            <td>
                                <input type="hidden" value="" id="hdnSupplierID" runat="server" />
                                <table cellpadding="0" cellspacing="0">
                                    <colgroup>
                                        <col style="width: 30%" />
                                        <col style="width: 3px" />
                                        <col style="width: 250px" />
                                    </colgroup>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtSupplierCode" CssClass="required" ValidationGroup="mpEntry" Width="100%"
                                                runat="server" />
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtSupplierName" ReadOnly="true" Width="100%" runat="server" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td style="padding: 5px; vertical-align: top">
                    <div style="position: relative;">
                        <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
                            ShowLoadingPanel="false" OnCallback="cbpView_Callback">
                            <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ onCbpViewEndCallback(s); }" />
                            <PanelCollection>
                                <dx:PanelContent ID="PanelContent1" runat="server">
                                    <input type="hidden" value="" id="hdnMovementDate" runat="server" />
                                    <asp:Panel runat="server" ID="pnlGridView" CssClass="pnlContainerGrid" Style="width: 100%;
                                        margin-left: auto; margin-right: auto; position: relative; font-size: 0.95em;
                                        height: 380px; overflow-y: auto;">
                                        <asp:ListView runat="server" ID="lvwView">
                                            <EmptyDataTemplate>
                                                <table id="tblView" runat="server" class="grdView notAllowSelect lvwView" cellspacing="0"
                                                    rules="all">
                                                    <tr>
                                                        <th rowspan="2" style="width: 120px">
                                                            <%=GetLabel("Kode Supplier") %>
                                                        </th>
                                                        <th rowspan="2">
                                                            <%=GetLabel("Nama Supplier") %>
                                                        </th>
                                                        <th rowspan="2" style="width: 100px">
                                                            <%=GetLabel("Saldo Awal") %>
                                                        </th>
                                                        <th rowspan="2" style="width: 100px">
                                                            <%=GetLabel("Penambahan")%>
                                                        </th>
                                                        <th rowspan="2" style="width: 100px">
                                                            <%=GetLabel("Pengurangan")%>
                                                        </th>
                                                        <th colspan="5">
                                                            <%=GetLabel("Saldo Akhir")%>
                                                        </th>
                                                    </tr>
                                                    <tr>
                                                        <th style="width: 100px">
                                                            <%=GetLabel("0-30 hari")%>
                                                        </th>
                                                        <th style="width: 100px">
                                                            <%=GetLabel(">30-60 hari")%>
                                                        </th>
                                                        <th style="width: 100px">
                                                            <%=GetLabel(">60-90 hari")%>
                                                        </th>
                                                        <th style="width: 100px">
                                                            <%=GetLabel(">90 hari")%>
                                                        </th>
                                                        <th style="width: 100px">
                                                            <%=GetLabel("Total")%>
                                                        </th>
                                                    </tr>
                                                    <tr class="trEmpty">
                                                        <td colspan="10">
                                                            <%=GetLabel("No Data To Display")%>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </EmptyDataTemplate>
                                            <LayoutTemplate>
                                                <table id="tblView" runat="server" class="grdView notAllowSelect lvwView" cellspacing="0"
                                                    rules="all">
                                                    <tr>
                                                        <th rowspan="2" style="width: 120px">
                                                            <%=GetLabel("Kode Supplier") %>
                                                        </th>
                                                        <th rowspan="2">
                                                            <%=GetLabel("Nama Supplier") %>
                                                        </th>
                                                        <th rowspan="2" style="width: 120px">
                                                            <%=GetLabel("Saldo Awal") %>
                                                        </th>
                                                        <th rowspan="2" style="width: 120px">
                                                            <%=GetLabel("Penambahan")%>
                                                        </th>
                                                        <th rowspan="2" style="width: 120px">
                                                            <%=GetLabel("Pengurangan")%>
                                                        </th>
                                                        <th colspan="5">
                                                            <%=GetLabel("Saldo Akhir")%>
                                                        </th>
                                                    </tr>
                                                    <tr>
                                                        <th style="width: 100px">
                                                            <%=GetLabel("0-30 hari")%>
                                                        </th>
                                                        <th style="width: 100px">
                                                            <%=GetLabel(">30-60 hari")%>
                                                        </th>
                                                        <th style="width: 100px">
                                                            <%=GetLabel(">60-90 hari")%>
                                                        </th>
                                                        <th style="width: 100px">
                                                            <%=GetLabel(">90 hari")%>
                                                        </th>
                                                        <th style="width: 100px">
                                                            <%=GetLabel("Total")%>
                                                        </th>
                                                    </tr>
                                                    <tr runat="server" id="itemPlaceholder">
                                                    </tr>
                                                </table>
                                            </LayoutTemplate>
                                            <ItemTemplate>
                                                <tr>
                                                    <td>
                                                        <%#: Eval("BusinessPartnerCode")%>
                                                    </td>
                                                    <td>
                                                        <input type="hidden" class="hdnBusinessPartnerID" value="<%#: Eval("BusinessPartnerID")%>" />
                                                        <label class="lblLink lblBusinessPartnerName">
                                                            <%#: Eval("BusinessPartnerName")%></label>
                                                    </td>
                                                    <td align="right">
                                                        <%#: Eval("BalanceBegin", "{0:N}")%>
                                                    </td>
                                                    <td align="right">
                                                        <%#: Eval("BalanceIN", "{0:N}")%>
                                                    </td>
                                                    <td align="right">
                                                        <%#: Eval("BalanceOUT", "{0:N}")%>
                                                    </td>
                                                    <td align="right">
                                                        <label <%#:Eval("Days_0_30").ToString() != "0.00" ? "class='lblLink lbl0_30'": "" %>>
                                                            <%#: Eval("Days_0_30","{0:N}")%></label>
                                                    </td>
                                                    <td align="right">
                                                        <label <%#:Eval("Days_30_60").ToString() != "0.00" ? "class='lblLink lbl30_60'": "" %>>
                                                            <%#: Eval("Days_30_60", "{0:N}")%>
                                                    </td>
                                                    <td align="right">
                                                        <label <%#:Eval("Days_60_90").ToString() != "0.00" ? "class='lblLink lbl60_90'": "" %>>
                                                            <%#: Eval("Days_60_90", "{0:N}")%></label>
                                                    </td>
                                                    <td align="right">
                                                        <label <%#:Eval("Days_90").ToString() != "0.00" ? "class='lblLink lbl90'": "" %>>
                                                            <%#: Eval("Days_90", "{0:N}")%></label>
                                                    </td>
                                                    <td align="right">
                                                        <%#: Eval("BalanceEND", "{0:N}")%>
                                                    </td>
                                                </tr>
                                            </ItemTemplate>
                                        </asp:ListView>
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
                    </div>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
