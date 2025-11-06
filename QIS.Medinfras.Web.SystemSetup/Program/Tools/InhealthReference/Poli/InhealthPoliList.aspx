<%@ Page Title="" Language="C#" MasterPageFile="~/Libs/MasterPage/MPList.master"
    AutoEventWireup="true" CodeBehind="InhealthPoliList.aspx.cs" Inherits="QIS.Medinfras.Web.SystemSetup.Tools.InhealthPoliList" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<asp:Content ID="Content3" ContentPlaceHolderID="plhCustomButtonToolbarLeft" runat="server">
    <li id="btnImportPoli" runat="server" crudmode="R">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbprocess.png") %>' alt="" />
        <div>
            <%=GetLabel("Import") %></div>
    </li>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhList" runat="server">
    <script type="text/javascript" src='<%= ResolveUrl("~/Libs/Scripts/CustomGridViewList.js")%>'></script>
    <script type="text/javascript">
        $(function () {
            var grd = new customGridView();
            grd.init('<%=grdView.ClientID %>', '<%=hdnID.ClientID %>', '<%=pnlView.ClientID %>', cbpView, 'paging');
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

        //#region Paging
        var pageCount = parseInt('<%=PageCount %>');
        var currPage = parseInt('<%=CurrPage %>');
        $(function () {
            setPaging($("#paging"), pageCount, function (page) {
                cbpView.PerformCallback('changepage|' + page);
            }, null, currPage);
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

        //#region Search Poli
        $('#<%=btnImportPoli.ClientID %>').live('click', function (evt) {
            var isBridgingInhealth = $('#<%=hdnIsBridgingToInhealth.ClientID %>').val();
            if (isBridgingInhealth == "1") {
                var keyword = $('#<%=txtParamPoli.ClientID %>').val();
                SearchInhealthPoli(keyword);
            } else {
                showToast('INFORMATION', 'Status bridging dengan Inhealth sedang nonaktif.');
            }
        });

        function SearchInhealthPoli(keyword) {
//            var token = $('#<%=hdnInhealthToken.ClientID %>').val();
//            var kodeprovider = $('#<%=hdnInhealthKodeProvider.ClientID %>').val();
//            InhealthService.poli(token, kodeprovider, keyword, function (result) {
//                if (result != null) {
//                    showToast('INFORMATION', "Ambil data poli berhasil.");
//                }
//                else {
//                    showToast('INFORMATION', "Tidak ada poli yang diambil.");
//                }
//            });

            var isBridgingToInhealth = $('#<%=hdnIsBridgingToInhealth.ClientID %>').val();
            if (isBridgingToInhealth == "1") {
                InhealthService.poli_API(keyword, function (result) {
                    if (result != null) {
                        var resultArr = result.split('|');
                        if (resultArr[0] == "1") {
                            ShowSnackbarSuccess(resultArr[1]);
                            onRefreshControl("");
                        }
                        else {
                            ShowSnackbarError(resultArr[1]);
                        }
                    }
                    else {
                        ShowSnackbarError(resultArr[1]);
                    }
                });
            }
            else {
                ShowSnackbarError("Tidak Bridging dengan Inhealth");
            }
        }
        //#endregion

    </script>
    <input type="hidden" id="hdnID" runat="server" value="" />
    <input type="hidden" id="hdnFilterExpression" runat="server" value="" />
    <input type="hidden" id="hdnIsBridgingToInhealth" value="" runat="server" />
    <input type="hidden" id="hdnInhealthToken" value="" runat="server" />
    <input type="hidden" id="hdnInhealthKodeProvider" value="" runat="server" />
    <table class="tblContentArea">
        <colgroup>
            <col style="width: 10%" />
            <col style="width: 50%" />
        </colgroup>
        <tr>
            <td class="tdLabel">
                <label>
                    <%=GetLabel("Kode Poli / Nama Poli")%></label>
            </td>
            <td>
                <asp:TextBox runat="server" ID="txtParamPoli" Width="460px" />
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <div style="position: relative;">
                    <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
                        ShowLoadingPanel="false" OnCallback="cbpView_Callback">
                        <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ onCbpViewEndCallback(s); }" />
                        <PanelCollection>
                            <dx:PanelContent ID="PanelContent1" runat="server">
                                <asp:Panel runat="server" ID="pnlView" CssClass="pnlContainerGrid">
                                    <asp:GridView ID="grdView" runat="server" CssClass="grdSelected" AutoGenerateColumns="false"
                                        ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                        <Columns>
                                            <asp:BoundField DataField="ObjectCode" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                            <asp:BoundField DataField="ObjectCode" HeaderText="Kode Poli" HeaderStyle-Width="50px" HeaderStyle-HorizontalAlign="Left" />
                                            <asp:BoundField DataField="ObjectName" HeaderText="Nama Poli" HeaderStyle-Width="150px" HeaderStyle-HorizontalAlign="Left" />
                                            <asp:BoundField DataField="cfStatus" HeaderText="Status" HeaderStyle-Width="25px" HeaderStyle-HorizontalAlign="Left" />
                                            <asp:BoundField DataField="cfCreatedDateInString" HeaderText="Tanggal" HeaderStyle-Width="50px" HeaderStyle-HorizontalAlign="Left" />
                                            <asp:BoundField DataField="CreatedByName" HeaderText="Dibuat Oleh" HeaderStyle-Width="100px" HeaderStyle-HorizontalAlign="Left" />                                        </Columns>
                                        <EmptyDataTemplate>
                                            <%=GetLabel("Data Tidak Tersedia")%>
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
                </div>
            </td>
        </tr>
    </table>
</asp:Content>
