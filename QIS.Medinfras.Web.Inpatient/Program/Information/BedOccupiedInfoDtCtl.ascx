<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="BedOccupiedInfoDtCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.Inpatient.Program.BedOccupiedInfoDtCtl" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>

<script type="text/javascript" id="dxss_BedOccupiedInfoDtCtl">
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
<input type="hidden" id="hdnRoomID" runat="server" />
<input type="hidden" id="hdnClassID" runat="server" />
<table class="tblContentArea">
    <tr>
        <td>
            <table class="tblEntryContent" style="width:70%">
                <colgroup>
                    <col style="width:120px"/>
                    <col/>
                </colgroup>
                <tr>
                    <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Kamar Perawatan")%></label></td>
                    <td><asp:TextBox ID="txtKamar" ReadOnly="true" Width="50%" runat="server" /></td>
                </tr>
                 <tr>
                    <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Kelas")%></label></td>
                    <td><asp:TextBox ID="txtKelas" ReadOnly="true" Width="50%" runat="server" /></td>
                </tr>   
            </table>

            <div style="position: relative;">
                <dxcp:ASPxCallbackPanel ID="cbpPopupView" runat="server" Width="100%" ClientInstanceName="cbpPopupView"
                    ShowLoadingPanel="false" OnCallback="cbpPopupView_Callback">
                    <ClientSideEvents EndCallback="function(s,e){onCbpPopupViewEndCallback()}" />
                    <PanelCollection>
                        <dx:PanelContent ID="PanelContent1" runat="server">
                            <asp:Panel runat="server" ID="pnlView" CssClass="pnlContainerGrid" Style="height:430px; overflow-y: scroll;">
                                <asp:GridView ID="grdPopupView" runat="server" CssClass="grdSelected" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty" >
                                    <Columns>
                                        <asp:BoundField DataField="BedCode" HeaderText="Tempat Tidur" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="151px"  />
                                        <asp:BoundField DataField="RegistrationNo" HeaderText="No. Registrasi" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="160px"  />
                                        <asp:BoundField DataField="cfPasien" HeaderText="Pasien" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left"/>
                                        <asp:BoundField DataField="cfPlanDischargeDate" HeaderText="Rencana Pulang" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="150px"  />
                                        <asp:BoundField DataField="cfPasienTitipan" HeaderText="Titipan" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="150px"  />
                                    </Columns>
                                    <EmptyDataTemplate>
                                        <%=GetLabel("No Data To Display")%>
                                    </EmptyDataTemplate>
                                </asp:GridView>
                            </asp:Panel>
                        </dx:PanelContent>
                    </PanelCollection>
                </dxcp:ASPxCallbackPanel>    
                <div class="imgLoadingGrdView" id="containerImgLoadingView" >
                    <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                </div>
                <div class="containerPaging">
                    <div class="wrapperPaging">
                        <div id="pagingPopup"></div>
                    </div>
                </div> 
            </div>
        </td>
    </tr>
</table>