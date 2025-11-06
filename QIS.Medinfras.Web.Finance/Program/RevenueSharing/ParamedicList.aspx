<%@ Page Language="C#" MasterPageFile="~/Libs/MasterPage/MPList.master" AutoEventWireup="true"
    CodeBehind="ParamedicList.aspx.cs" Inherits="QIS.Medinfras.Web.Finance.Program.ParamedicList" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<asp:Content ID="Content2" ContentPlaceHolderID="plhCustomButtonToolbar" runat="server">
    <li id="btnMPListView" runat="server">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbitems.png")%>' alt="" /><div>
            <%=GetLabel("View")%></div>
    </li>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="plhCustomContextMenu" runat="server">
    <li class="list-devider">
        <hr />
    </li>
    <li id="ctxMenuView" runat="server"><a href="#">
        <%=GetLabel("View")%></a> </li>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhList" runat="server">
    <script type="text/javascript" src='<%= ResolveUrl("~/Libs/Scripts/CustomGridViewList.js")%>'></script>
    <script type="text/javascript">
        $(function () {
            Methods.checkImageError('imgParamedicProfilePicture', 'paramedic');

            var grd = new customGridView();
            grd.init('<%=grdView.ClientID %>', '<%=hdnID.ClientID %>', '<%=pnlView.ClientID %>', cbpView, 'paging');

            setDatePicker('<%=txtPeriodFrom.ClientID %>');
            setDatePicker('<%=txtPeriodTo.ClientID %>');
        });

        //#region Paramedic Master
        function onGetParamedicMasterFilterExpression() {
            var filterExpression = "IsDeleted = 0 AND IsHasRevenueSharing = 1";
            return filterExpression;
        }

        $('#lblParamedic.lblLink').live('click', function () {
            openSearchDialog('paramedicRevenueSharing', onGetParamedicMasterFilterExpression(), function (value) {
                $('#<%=txtParamedicCode.ClientID %>').val(value);
                ontxtParamedicCodeChanged(value);
            });
        });

        $('#<%=txtParamedicCode.ClientID %>').live('change', function () {
            ontxtParamedicCodeChanged($(this).val());
        });

        function ontxtParamedicCodeChanged(value) {
            var filterExpression = onGetParamedicMasterFilterExpression() + " AND ParamedicCode = '" + value + "'";
            Methods.getObject('GetParamedicMasterList', filterExpression, function (result) {
                if (result != null) {
                    $('#<%=hdnParamedicID.ClientID %>').val(result.ParamedicID);
                    $('#<%=txtParamedicName.ClientID %>').val(result.FullName);
                }
                else {
                    $('#<%=hdnParamedicID.ClientID %>').val('');
                    $('#<%=txtParamedicCode.ClientID %>').val('');
                    $('#<%=txtParamedicName.ClientID %>').val('');
                }
            });
        }
        //#endregion

        //#region Revenue Sharing
        function onGetRevenueSharingFilterExpression() {
            var filterExpression = "IsDeleted = 0";
            return filterExpression;
        }

        $('#lblRevenueSharing.lblLink').live('click', function () {
            openSearchDialog('revenuesharing', onGetRevenueSharingFilterExpression(), function (value) {
                $('#<%=txtRevenueSharingCode.ClientID %>').val(value);
                ontxtRevenueSharingCodeChanged(value);
            });
        });

        $('#<%=txtRevenueSharingCode.ClientID %>').live('change', function () {
            ontxtRevenueSharingCodeChanged($(this).val());
        });

        function ontxtRevenueSharingCodeChanged(value) {
            var filterExpression = onGetRevenueSharingFilterExpression() + " AND RevenueSharingCode = '" + value + "'";
            Methods.getObject('GetRevenueSharingHdList', filterExpression, function (result) {
                if (result != null) {
                    $('#<%=hdnRevenueSharingID.ClientID %>').val(result.RevenueSharingID);
                    $('#<%=txtRevenueSharingName.ClientID %>').val(result.RevenueSharingName);
                }
                else {
                    $('#<%=hdnRevenueSharingID.ClientID %>').val('');
                    $('#<%=txtRevenueSharingCode.ClientID %>').val('');
                    $('#<%=txtRevenueSharingName.ClientID %>').val('');
                }
            });
        }
        //#endregion

        $('#btnRefresh').live('click', function () {
            cbpView.PerformCallback('refresh');
        });

        function onRefreshControl(filterExpression) {
            $('#<%=hdnFilterExpression.ClientID %>').val(filterExpression);
            cbpView.PerformCallback('refresh');
        }

        function onGetCurrID() {
            return $('#<%=hdnID.ClientID %>').val();
        }

        function onGetFilterExpression() {
            return $('#<%=hdnFilterExpression.ClientID %>').val();
        }

        $('#<%=btnMPListView.ClientID %>').click(function () {
            clickView();
        });

        function clickView() {
            if ($('#<%=grdView.ClientID %> tr.selected').length > 0) {
                showLoadingPanel();
                $('#<%=hdnID.ClientID %>').val($('#<%=grdView.ClientID %> tr.selected').find('.keyField').html());
                __doPostBack('<%=btnOpenTransactionDt.UniqueID%>', '');
            }
        }

        function onCbpViewEndCallback(s) {
            hideLoadingPanel();

            var param = s.cpResult.split('|');
            if (param[0] == 'refresh') {
                $('#<%=grdView.ClientID %> tr:eq(1)').click();
            }
            else {
                $('#<%=grdView.ClientID %> tr:eq(1)').click();
            }

            Methods.checkImageError('imgParamedicProfilePicture', 'paramedic');
        }
    </script>
    <div style="display: none">
        <asp:Button ID="btnOpenTransactionDt" runat="server" UseSubmitBehavior="false" OnClientClick="return onBeforeOpenTransactionDt();"
            OnClick="btnOpenTransactionDt_Click" /></div>
    <input type="hidden" value="" id="hdnID" runat="server" />
    <input type="hidden" id="hdnFilterExpression" runat="server" value="" />
    <table class="tblContentArea" style="width: 100%">
        <colgroup>
            <col style="width: 70%" />
            <col style="width: 30%" />
        </colgroup>
        <tr>
            <td>
                <table>
                    <colgroup>
                        <col style="width: 150px" />
                        <col style="width: 100px" />
                        <col style="width: 350px" />
                        <col />
                    </colgroup>
                    <tr>
                        <td>
                            <label class="lblNormal lblLink" id="lblParamedic">
                                <%=GetLabel("Dokter / Paramedis")%></label>
                        </td>
                        <td>
                            <input type="hidden" id="hdnParamedicID" runat="server" value="" />
                            <asp:TextBox ID="txtParamedicCode" Width="100%" runat="server" />
                        </td>
                        <td>
                            <asp:TextBox ID="txtParamedicName" ReadOnly="true" Width="100%" runat="server" />
                        </td>
                        <td>
                            <asp:CheckBox ID="chkIsExcludeChargesFilter" runat="server" Checked="false" Text="Abaikan Transaksi Pasien" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <label class="lblNormal">
                                <%=GetLabel("Periode") %></label>
                        </td>
                        <td colspan="2">
                            <table cellpadding="0" cellspacing="0">
                                <tr>
                                    <td>
                                        <asp:TextBox runat="server" Width="120px" ID="txtPeriodFrom" CssClass="datepicker" />
                                    </td>
                                    <td style="width: 30px; text-align: center">
                                        s/d
                                    </td>
                                    <td>
                                        <asp:TextBox runat="server" Width="120px" ID="txtPeriodTo" CssClass="datepicker" />
                                    </td>
                                    <td>
                                        <asp:CheckBox ID="chkIsExludeParamDate" runat="server" Checked="false" Text="Abaikan Tanggal" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <label class="lblNormal lblLink" id="lblRevenueSharing">
                                <%=GetLabel("Jasa Medis")%></label>
                        </td>
                        <td>
                            <input type="hidden" id="hdnRevenueSharingID" runat="server" value="" />
                            <asp:TextBox ID="txtRevenueSharingCode" Width="100%" runat="server" />
                        </td>
                        <td>
                            <asp:TextBox ID="txtRevenueSharingName" ReadOnly="true" Width="100%" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td>
                            <input type="button" id="btnRefresh" value="R e f r e s h" class="btnRefresh w3-button w3-blue w3-border w3-border-blue w3-round-large" />
                        </td>
                    </tr>
                </table>
            </td>
            <td>
            </td>
        </tr>
        <tr>
            <td colspan="5">
                <div style="position: relative;">
                    <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
                        ShowLoadingPanel="false" OnCallback="cbpView_Callback">
                        <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ onCbpViewEndCallback(s); }" />
                        <PanelCollection>
                            <dx:PanelContent ID="PanelContent1" runat="server">
                                <asp:Panel runat="server" ID="pnlView" CssClass="pnlContainerGrid" Style="height: 350px;
                                    overflow-y: scroll;">
                                    <asp:GridView ID="grdView" runat="server" CssClass="grdSelected" AutoGenerateColumns="false"
                                        ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                        <Columns>
                                            <asp:BoundField DataField="ParamedicID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                            <asp:TemplateField>
                                                <HeaderTemplate>
                                                    <%=GetLabel("Informasi Dokter / Paramedis")%>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <div style="padding: 3px">
                                                        <img class="imgParamedicProfilePicture" src='<%#Eval("cfPictureFileName") %>' alt=""
                                                            height="55px" style="float: left; margin-right: 10px;" />
                                                        <div>
                                                            <b>
                                                                <%#: Eval("ParamedicName")%></b></div>
                                                        <table cellpadding="0" cellspacing="0">
                                                            <colgroup>
                                                                <col style="width: 150px" />
                                                                <col style="width: 5px" />
                                                                <col style="width: 100px" />
                                                                <col style="width: 200px" />
                                                                <col style="width: 5px" />
                                                                <col style="width: 200px" />
                                                            </colgroup>
                                                            <tr>
                                                                <td align="right">
                                                                    <div style="font-size: 0.9em; font-style: italic">
                                                                        <%=GetLabel("Kode Dokter / Paramedis")%></div>
                                                                    <div style="font-size: 0.9em; font-style: italic">
                                                                        <%=GetLabel("Inisial")%></div>
                                                                </td>
                                                                <td>
                                                                    &nbsp;
                                                                </td>
                                                                <td>
                                                                    <div>
                                                                        <%#: Eval("ParamedicCode")%></div>
                                                                    <div>
                                                                        <%#: Eval("Initial")%></div>
                                                                </td>
                                                                <td align="right">
                                                                    <div style="font-size: 0.9em; font-style: italic">
                                                                        <%=GetLabel("Tipe Dokter / Paramedis")%></div>
                                                                    <div style="font-size: 0.9em; font-style: italic">
                                                                        <%=GetLabel("Spesialisasi")%></div>
                                                                </td>
                                                                <td>
                                                                    &nbsp;
                                                                </td>
                                                                <td>
                                                                    <div>
                                                                        <%#: Eval("ParamedicMasterType")%></div>
                                                                    <div>
                                                                        <%#: Eval("SpecialtyName")%></div>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </div>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:CheckBoxField DataField="IsAvailable" HeaderText="Available" HeaderStyle-Width="80px"
                                                ItemStyle-HorizontalAlign="Center" />
                                            <asp:TemplateField HeaderStyle-Width="100px" HeaderStyle-HorizontalAlign="Center"
                                                ItemStyle-HorizontalAlign="Center">
                                                <HeaderTemplate>
                                                    <%=GetLabel("Jumlah Transaksi JasMed (PRS)")%>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <div style="padding: 3px; text-align:center">
                                                        <%#: Eval("CountPRS")%>
                                                    </div>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-Width="150px" HeaderStyle-HorizontalAlign="Center"
                                                ItemStyle-HorizontalAlign="Center">
                                                <HeaderTemplate>
                                                    <%=GetLabel("Rekap Terakhir (SRS)")%>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <div style="padding: 3px; text-align:center">
                                                        <%#: Eval("LastRSSummaryNo")%>
                                                    </div>
                                                    <div style="padding: 3px; text-align:center">
                                                        <%#: Eval("LastTransactionStatus")%>
                                                    </div>
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
            </td>
        </tr>
    </table>
</asp:Content>
