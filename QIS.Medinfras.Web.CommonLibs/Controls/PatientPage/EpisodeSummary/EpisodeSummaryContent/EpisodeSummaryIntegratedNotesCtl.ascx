<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EpisodeSummaryIntegratedNotesCtl.ascx.cs" 
    Inherits="QIS.Medinfras.Web.CommonLibs.Controls.EpisodeSummaryIntegratedNotesCtl" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>

<script type="text/javascript" id="dxss_episodeSummaryIntegratedNotes">
    $(function () {
    });

    var pageCount = parseInt('<%=PageCount %>');
    $(function () {
        setPaging($("#grdPaging"), pageCount, function (page) {
            cbpView.PerformCallback('changepage|' + page);
        });
    });

    function onCbpViewEndCallback(s) {
        $('#containerImgLoadingView').hide();

        var param = s.cpResult.split('|');
        if (param[0] == 'refresh') {
            var pageCount = parseInt(param[1]);
            if (pageCount > 0)
                $('#<%=grdView.ClientID %> tr:eq(1)').click();

            setPaging($("#grdPaging"), pageCount, function (page) {
                cbpView.PerformCallback('changepage|' + page);
            });
        }
        else
            $('#<%=grdView.ClientID %> tr:eq(1)').click();
    }
</script>
<input type="hidden" id="hdnLinkedVisitID" value="" runat="server" />
<div>
    <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
    ShowLoadingPanel="false" OnCallback="cbpView_Callback">
    <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingView').show(); }"
        EndCallback="function(s,e){ onCbpViewEndCallback(s); }" />
    <PanelCollection>
        <dx:PanelContent ID="PanelContent4" runat="server">
            <asp:Panel runat="server" ID="Panel3" CssClass="pnlContainerGridPatientPage4">
                <asp:GridView ID="grdView" runat="server" CssClass="grdSelected grdPatientPage" AutoGenerateColumns="false"
                    ShowHeaderWhenEmpty="false" EmptyDataRowStyle-CssClass="trEmpty" ShowHeader="true">
                    <Columns>
                        <asp:BoundField DataField="cfNoteDate" HeaderText="Tanggal" HeaderStyle-Width="100px"
                            HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField DataField="NoteTime" HeaderText="Jam" HeaderStyle-Width="60px"
                            HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField DataField="cfPPA" HeaderText="PPA" HeaderStyle-Width="50px"
                            HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                        <asp:TemplateField HeaderText="Catatan Terintegrasi" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left">
                            <ItemTemplate>
                                <div>
                                    <span style="color: blue; font-style: italic">
                                        <%#:Eval("ParamedicName") %>
                                    </span>: <span style="font-style: italic">
                                        <%#:Eval("cfLastUpdatedDate") %></span>
                                </div>
                                <div>
                                    <textarea style="padding-left: 10px; border: 0; width: 99%; height: 200px; background-color: transparent"
                                        readonly><%#: DataBinder.Eval(Container.DataItem, "NoteText") %> </textarea>
                                </div>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <EmptyDataTemplate>
                        <%=GetLabel("Tidak ada catatan terintegrasi untuk pasien ini") %>
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
            <div id="grdPaging">
            </div>
        </div>
    </div>
</div>
