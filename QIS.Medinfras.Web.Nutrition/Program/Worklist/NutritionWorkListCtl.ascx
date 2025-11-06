<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="NutritionWorkListCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.Nutrition.Program.NutritionWorkListCtl" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<script type="text/javascript" id="dxss_drugslogisticsquickpicksctl">
    //#region Paging
    var pageCount = parseInt('<%=PageCount %>');
    $(function () {
        setPaging($("#pagingPopup"), pageCount, function (page) {
            cbpViewPopup.PerformCallback('changepage|' + page);
        });

        $('#btnProcess').click(function () {
            cbpViewPopup.PerformCallback('refresh');
        });
    });

    function onCbpViewPopupEndCallback(s) {
        hideLoadingPanel();

        var param = s.cpResult.split('|');
        if (param[0] == 'refresh') {
            var pageCount = parseInt(param[1]);
            if (pageCount > 0)
                $('#<%=grdView.ClientID %> tr:eq(1)').click();

            setPaging($("#pagingPopup"), pageCount, function (page) {
                cbpViewPopup.PerformCallback('changepage|' + page);
            });
        }
        else
            $('#<%=grdView.ClientID %> tr:eq(1)').click();
    }

    $('#btnRefresh').live('click', function () {
        cbpViewPopup.PerformCallback('refresh');
    });
    //#endregion    
</script>
<div style="padding: 10px;">
    <input type="hidden" id="hdnMealPlanID" value="" runat="server" />
    <input type="hidden" id="hdnGCMealTime" value="" runat="server" />
    <input type="hidden" id="hdnGCMealDay" value="" runat="server" />
    <input type="hidden" id="hdnGCMealStatus" value="" runat="server" />
    <input type="hidden" id="hdnDateParam" value="" runat="server" />
    <input type="hidden" id="hdnIsHasRemarks" value="" runat="server" />
    <table width="100%">
        <tr>
            <td>
                <table>
                    <colgroup>
                        <col style="width: 120px" />
                        <col />
                    </colgroup>
                    <tr>
                        <td class="tdLabel">
                            <%=GetLabel("Tanggal Makan") %>
                        </td>
                        <td>
                            <asp:TextBox ID="txtDate" Width="120px" CssClass="datepicker" ReadOnly="true" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <%=GetLabel("Jadwal Makan") %>
                        </td>
                        <td>
                            <asp:TextBox ID="txtMealTime" Width="120px" runat="server" ReadOnly="true" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <%=GetLabel("Menu Hari Ke-") %>
                        </td>
                        <td>
                            <asp:TextBox ID="txtMealDay" Width="120px" runat="server" ReadOnly="true" CssClass="number" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <%=GetLabel("Service Unit") %>
                        </td>
                        <td>
                            <dxe:ASPxComboBox ID="cboServiceUnit" Width="200px" runat="server" ClientInstanceName="cboServiceUnit">
                            </dxe:ASPxComboBox>
                        </td>
                        <td>
                            <input type="button" id="btnRefresh" title='<%:GetLabel("Ambil Data") %>' value="Ambil Data" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <%=GetLabel("Total Pasien") %>
                        </td>
                        <td>
                            <asp:TextBox ID="txtPatientTotal" Width="50px" runat="server" ReadOnly="true" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
                <dxcp:ASPxCallbackPanel ID="cbpViewPopup" runat="server" Width="100%" ClientInstanceName="cbpViewPopup"
                    ShowLoadingPanel="false" OnCallback="cbpViewPopup_Callback">
                    <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ onCbpViewPopupEndCallback(s); }" />
                    <PanelCollection>
                        <dx:PanelContent ID="PanelContent1" runat="server">
                            <asp:Panel runat="server" ID="pnlView" Style="width: 100%; margin-left: auto; margin-right: auto;
                                position: relative; font-size: 0.95em; max-height: 420px; overflow-y: scroll">
                                <asp:GridView ID="grdView" runat="server" CssClass="grdView notAllowSelect" AutoGenerateColumns="false"
                                    ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                    <Columns>
                                        <asp:BoundField DataField="NutritionOrderNo" ItemStyle-HorizontalAlign="Left" HeaderText="Nomor Order"
                                            HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="150px" />
                                        <asp:BoundField DataField="RegistrationNo" HeaderText="Nomor Registrasi" HeaderStyle-Width="150px"
                                            HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                                        <asp:BoundField DataField="MedicalNo" HeaderText="No. RM" HeaderStyle-Width="70px"
                                            HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                        <asp:BoundField DataField="DiagnoseText" HeaderText="Diagnosa Gizi" HeaderStyle-Width="200px"
                                            HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left" />
                                        <asp:BoundField DataField="DietType" HeaderText="Diet" HeaderStyle-Width="200px"
                                            HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left" />
                                        <asp:BoundField DataField="PatientName" HeaderText="Nama Pasien" HeaderStyle-HorizontalAlign="Left"
                                            ItemStyle-HorizontalAlign="Left" />
                                        <asp:BoundField DataField="ServiceUnitName" HeaderText="Ruang" HeaderStyle-HorizontalAlign="Left"
                                            ItemStyle-HorizontalAlign="Left" />
                                        <asp:BoundField DataField="ClassName" HeaderText="Kelas" HeaderStyle-Width="70px"
                                            HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                                        <asp:BoundField DataField="BedCode" HeaderText="No. TT" HeaderStyle-Width="70px"
                                            ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" />
                                        <asp:BoundField DataField="Remarks" HeaderText="Catatan" ItemStyle-ForeColor="Red" />
                                        <asp:BoundField DataField="Status" HeaderText="Status" HeaderStyle-HorizontalAlign="Center"
                                            ItemStyle-HorizontalAlign="Center" />
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
                <div class="containerPaging" style="display: none">
                    <div class="wrapperPaging">
                        <div id="pagingPopup">
                        </div>
                    </div>
                </div>
            </td>
        </tr>
    </table>
</div>
