<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ProportionalRegistrationInformationTransactionDetailCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.Finance.Program.ProportionalRegistrationInformationTransactionDetailCtl" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<script type="text/javascript" id="dxss_ProportionalRegistrationInformationTransactionDetailCtl">
    function oncbpViewPopUpCtlEndCallback(s) {
        var param = s.cpResult.split('|');
        if (param[0] == 'save') {
            if (param[1] == 'fail') {
                showToast('Save Failed', 'Error Message : ' + param[2]);
                cbpViewPopUpCtl.PerformCallback('refresh');
            }
            else {
                var pageCount = parseInt(param[2]);
                setPagingDetailItem(pageCount);
                $('#containerReferrerEntryDataCtl').hide();
            }
        }
        else if (param[0] == 'delete') {
            if (param[1] == 'fail') {
                showToast('Delete Failed', 'Error Message : ' + param[2]);
                cbpViewPopUpCtl.PerformCallback('refresh');
            }
            else {
                var pageCount = parseInt(param[2]);
                setPagingDetailItem(pageCount);
            }
        }
        else if (param[0] == 'refresh') {
            var pageCount = parseInt(param[1]);
            setPagingDetailItem(pageCount);
        }
        else {
            $('#<%=grdView.ClientID %> tr:eq(1)').click();
        }
        hideLoadingPanel();
    }

    //#region Paging
    var pageCountAvailable = parseInt('<%=PageCount %>');
    $(function () {
        setPagingDetailItem(pageCountAvailable);
    });

    function setPagingDetailItem(pageCount) {
        if (pageCount > 0)
            $('#<%=grdView.ClientID %> tr:eq(1)').click();
        setPaging($("#pagingPopup"), pageCount, function (page) {
            cbpViewPopUpCtl.PerformCallback('changepage|' + page);
        }, 8);
    }
    //#endregion 

    function oncboReferrerSearchCodeValueChanged() {
        onRefreshGridView();
    }

    function onRefreshGridView() {
        cbpViewPopUpCtl.PerformCallback('refresh');
    }

    function onCboStatusValueChanged(s) {
        onRefreshGridView();
    }
</script>
<input type="hidden" value="" id="hdnRegistrationIDCtl" runat="server" />
<div style="height: 100%;">
    <div class="pageTitle">
        <%=GetLabel("Detail Transaksi")%></div>
    <table>
        <tr>
            <td>
                <%=GetLabel("No Registrasi")%>
            </td>
            <td>
                <asp:TextBox ID="txtRegistrationNo" ReadOnly="true" Width="500px" runat="server" />
            </td>
        </tr>
        <tr>
            <td>
                <%=GetLabel("Pasien")%>
            </td>
            <td>
                <asp:TextBox ID="txtPatient" ReadOnly="true" Width="500px" runat="server" />
            </td>
        </tr>
        <tr>
            <td>
                <%=GetLabel("Filter")%>
            </td>
            <td>
                <dxe:ASPxComboBox ID="cboStatus" ClientInstanceName="cboStatus" Width="30%"
                    runat="server">
                    <ClientSideEvents ValueChanged="function(s){ onCboStatusValueChanged(s); }" />
                </dxe:ASPxComboBox>
            </td>
        </tr>
    </table>
    <table class="tblContentArea">
        <tr>
            <td style="padding: 5px; vertical-align: top">
                <div>
                    <dxcp:ASPxCallbackPanel ID="cbpViewPopUpCtl" runat="server" Width="100%" ClientInstanceName="cbpViewPopUpCtl"
                        ShowLoadingPanel="false" OnCallback="cbpViewPopUpCtl_Callback">
                        <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ oncbpViewPopUpCtlEndCallback(s); }" />
                        <PanelCollection>
                            <dx:PanelContent ID="PanelContent1" runat="server">
                                <asp:Panel runat="server" ID="pnlGrdView" Style="width: 100%; margin-left: auto;
                                    margin-right: auto; position: relative; font-size: 0.95em;">
                                    <asp:GridView ID="grdView" runat="server" CssClass="grdView notAllowSelect" AutoGenerateColumns="false">
                                        <Columns>
                                            <asp:TemplateField ItemStyle-VerticalAlign="Top" HeaderStyle-Width="120px">
                                                <HeaderTemplate>
                                                    <div style="text-align: left; padding-left: 3px">
                                                        <%=GetLabel("No Transaksi")%>
                                                    </div>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <div style="padding: 3px">
                                                        <div>
                                                            <%#: Eval("TransactionNo")%><br>
                                                            <b>
                                                                <%#: Eval("ServiceUnitName")%></b></div>
                                                    </div>
                                                </ItemTemplate>
                                                <ItemStyle VerticalAlign="Top"></ItemStyle>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="cfItemName" HeaderText="Nama Item" HeaderStyle-Width="150px"
                                                HeaderStyle-HorizontalAlign="Left" />
                                            <asp:BoundField DataField="cfQty" HeaderText="Jumlah" HeaderStyle-Width="100px" HeaderStyle-HorizontalAlign="Right"
                                                ItemStyle-HorizontalAlign="Right" />
                                            <asp:BoundField DataField="cfTariffComp1" HeaderText="Tariff Comp1" HeaderStyle-Width="100px"
                                                HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" />
                                            <asp:BoundField DataField="cfTariffComp2" HeaderText="Tariff Comp2" HeaderStyle-Width="100px"
                                                HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" />
                                            <asp:BoundField DataField="cfTariffComp3" HeaderText="Tariff Comp3" HeaderStyle-Width="100px"
                                                HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" />
                                            <asp:BoundField DataField="cfTariffComp1Final" HeaderText="Tariff Comp1 Final" HeaderStyle-Width="100px"
                                                HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" />
                                            <asp:BoundField DataField="cfTariffComp2Final" HeaderText="Tariff Comp2 Final" HeaderStyle-Width="100px"
                                                HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" />
                                            <asp:BoundField DataField="cfTariffComp3Final" HeaderText="Tariff Comp3 Final" HeaderStyle-Width="100px"
                                                HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" />
                                        </Columns>
                                    </asp:GridView>
                                    <div class="imgLoadingGrdView" id="containerImgLoadingReferrer">
                                        <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                                    </div>
                                </asp:Panel>
                            </dx:PanelContent>
                        </PanelCollection>
                    </dxcp:ASPxCallbackPanel>
                    <div class="imgLoadingGrdView" id="containerImgLoadingViewPopup">
                        <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                    </div>
                    <div class="containerPaging">
                        <div class="wrapperPaging">
                            <div id="pagingPopup">
                            </div>
                        </div>
                    </div>
                </div>
            </td>
        </tr>
    </table>
    <div style="width: 100%; text-align: right">
        <input type="button" value='<%= GetLabel("Tutup")%>' onclick="pcRightPanelContent.Hide();" />
    </div>
</div>
