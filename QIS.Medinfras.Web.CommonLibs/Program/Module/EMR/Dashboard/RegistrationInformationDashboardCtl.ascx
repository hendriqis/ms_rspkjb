<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RegistrationInformationDashboardCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.RegistrationInformationDashboardCtl" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<script type="text/javascript" id="dxss_registrationinformationdashboardCtl">
    //#region Paging
    var pageCount = parseInt('<%=PageCount %>');
    $(function () {
        setPaging($("#pagingPopup"), pageCount, function (page) {
            cbpPopupView.PerformCallback('changepage|' + page);
        });
    });

    function onCbpPopupViewEndCallback(s) {
        hideLoadingPanel();

        var param = s.cpResult.split('|');
        if (param[0] == 'refresh') {
            var pageCount = parseInt(param[1]);
            setPaging($("#pagingPopup"), pageCount, function (page) {
                cbpPopupView.PerformCallback('changepage|' + page);
            });
        }
    }
    //#endregion

</script>
<input type="hidden" id="hdnID" runat="server" />
<input type="hidden" id="hdnParamedicID" runat="server" />
<input type="hidden" id="hdnHSUID" runat="server" />
<input type="hidden" id="hdnTotal" runat="server" />
<table class="tblContentArea">
    <tr>
        <td style="padding: 5px; vertical-align: top">
            <div>
                <dxcp:ASPxCallbackPanel ID="cbpPopupView" runat="server" Width="100%" ClientInstanceName="cbpPopupView"
                    ShowLoadingPanel="false" OnCallback="cbpPopupView_Callback">
                    <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){onCbpPopupViewEndCallback()}" />
                    <PanelCollection>
                        <dx:PanelContent ID="PanelContent1" runat="server">
                            <asp:Panel runat="server" ID="pnlView" CssClass="pnlContainerGrid" Style="width: 100%; margin-left: auto;
                                    margin-right: auto; position: relative; font-size: 0.95em;">
                                <asp:GridView ID="grdPopupView" runat="server" CssClass="grdSelected notAllowSelect" AutoGenerateColumns="false"
                                    ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                    <Columns>
                                        <asp:BoundField DataField="RegistrationNo" HeaderText="No. Registrasi" ItemStyle-HorizontalAlign="Left"
                                            HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="160px" />
                                        <asp:BoundField DataField="cfRegistrationDateInString" HeaderText="Tanggal Registrasi"
                                            ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="100px" />
                                        <asp:BoundField DataField="MedicalNo" HeaderText="No. RM" ItemStyle-HorizontalAlign="Center"
                                            HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="100px" />
                                        <asp:BoundField DataField="PatientName" HeaderText="Nama Pasien" ItemStyle-HorizontalAlign="Left"
                                            HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="300px" />
                                        <asp:BoundField DataField="cfGender" HeaderText="Jenis Kelamin" ItemStyle-HorizontalAlign="Left"
                                            HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="80px" />
                                        <asp:BoundField DataField="BusinessPartnerName" HeaderText="Nama Penjamin Bayar"
                                            ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="200px" />
                                        <asp:BoundField DataField="RegistrationStatus" HeaderText="Status Registrasi"
                                            ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="100px" />
                                    </Columns>
                                    <EmptyDataTemplate>
                                        <%=GetLabel("No Data To Display")%>
                                    </EmptyDataTemplate>
                                </asp:GridView>
                            </asp:Panel>
                        </dx:PanelContent>
                    </PanelCollection>
                </dxcp:ASPxCallbackPanel>
                <div class="imgLoadingGrdView" id="containerImgLoadingView" style="display: none">
                    <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                </div>
            </div>
        </td>
    </tr>
</table>
