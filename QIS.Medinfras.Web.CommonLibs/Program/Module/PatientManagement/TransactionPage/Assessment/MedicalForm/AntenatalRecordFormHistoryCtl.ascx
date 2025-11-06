<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AntenatalRecordFormHistoryCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.AntenatalRecordFormHistoryCtl" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<input type="hidden" value="" id="hdnAntenatalRecordID" runat="server" />
<input type="hidden" id="hdnFilterExpression" runat="server" value="" />
<table style="width: 100%">
    <colgroup>
        <col style="width: 50%" />
    </colgroup>
    <tr>
        <td valign="top">
            <div style="position: relative;">
                <dxcp:ASPxCallbackPanel ID="cbpViewAntenatalHistory" runat="server" Width="100%" ClientInstanceName="cbpViewAntenatalHistory"
                    ShowLoadingPanel="false" OnCallback="cbpViewAntenatalHistory_Callback">
                    <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingView').show(); }"
                        EndCallback="function(s,e){ oncbpViewAntenatalHistoryEndCallback(s); }" />
                    <PanelCollection>
                        <dx:PanelContent ID="PanelContent1" runat="server">
                            <asp:Panel runat="server" ID="pnlView" CssClass="pnlContainerGridPatientPage">
                                <asp:GridView ID="grdAntenatalRecordHistory" runat="server" CssClass="grdSelected grdPatientPage"
                                    AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                    <Columns>
                                        <asp:BoundField DataField="HistoryID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                        <asp:BoundField DataField="PregnancyNo" HeaderText="Kehamilan Ke-" HeaderStyle-Width="80px"
                                            HeaderStyle-HorizontalAlign="Left" />
                                        <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                            HeaderText="LMP" HeaderStyle-Width="150px" ItemStyle-Width="150px">
                                            <ItemTemplate>
                                                <div>
                                                    <%#: Eval("cfLMP")%>
                                                    <br>
                                                    <%#: Eval("LMPPeriod")%>
                                                    (<%#: Eval("LMPDays")%>
                                                    Days)
                                                </div>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="cfEDB" HeaderText="EDB" HeaderStyle-Width="150px" HeaderStyle-HorizontalAlign="Center"
                                            ItemStyle-HorizontalAlign="Center" />
                                        <asp:BoundField DataField="Gravida" HeaderText="Gravida" HeaderStyle-HorizontalAlign="Center"
                                            ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="60px" />
                                        <asp:BoundField DataField="Para" HeaderText="Para" HeaderStyle-HorizontalAlign="Center"
                                            ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="60px" />
                                        <asp:BoundField DataField="Abortion" HeaderText="Abortion" HeaderStyle-HorizontalAlign="Center"
                                            ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="60px" />
<%--                                        <asp:BoundField DataField="Life" HeaderText="Life" HeaderStyle-HorizontalAlign="Center"
                                            ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="60px" />--%>
<%--                                        <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                            HeaderText="Diubah Oleh" HeaderStyle-Width="150px" ItemStyle-Width="150px">
                                            <ItemTemplate>
                                                <div>
                                                    <%#: Eval("FullName")%>
                                                    <br>
                                                    <%#: Eval("cfCreatedDate")%>
                                                </div>
                                            </ItemTemplate>
                                        </asp:TemplateField>--%>
                                    </Columns>
                                    <EmptyDataTemplate>
                                        <%=GetLabel("Belum ada data") %>
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
<script type="text/javascript" id="dxss_infopurchaseorderctl">
    $(function () {
        $('#<%=grdAntenatalRecordHistory.ClientID %> > tbody > tr:gt(0):not(.trEmpty)').live('click', function () {
            if ($(this).attr('class') != 'selected') {
                $('#<%=grdAntenatalRecordHistory.ClientID %> tr.selected').removeClass('selected');
                $(this).addClass('selected');                
            }
        });
    });

    //#region Paging
    var pageCount = parseInt('<%=PageCount %>');
    $(function () {
        setPaging($("#paging"), pageCount, function (page) {
            cbpViewAntenatalHistory.PerformCallback('changepage|' + page);
        });
    });

    function oncbpViewAntenatalHistoryEndCallback(s) {
        $('#containerImgLoadingView').hide(); ;
        var param = s.cpResult.split('|');
        if (param[0] == 'refresh') {
            var pageCount = parseInt(param[1]);
            if (pageCount > 0)
                $('#<%=grdAntenatalRecordHistory.ClientID %> tr:eq(1)').click();

            setPaging($("#paging"), pageCount, function (page) {
                cbpViewAntenatalHistory.PerformCallback('changepage|' + page);
            });
        }
        else
            $('#<%=grdAntenatalRecordHistory.ClientID %> tr:eq(1)').click();
    }
    //#endregion
</script>
