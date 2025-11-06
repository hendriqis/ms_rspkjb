<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EpisodeSummaryReviewOfSystemCtl.ascx.cs" 
    Inherits="QIS.Medinfras.Web.CommonLibs.Controls.EpisodeSummaryReviewOfSystemCtl" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>

<script type="text/javascript" id="dxss_erpatientstatus1">
    $(function () {
    });

    var pageCount = parseInt('<%=gridROSPageCount %>');
    $(function () {
        setPaging($("#rosPaging"), pageCount, function (page) {
            cbpView.PerformCallback('changepage|' + page);
        });
    });

    function onCbpROSViewEndCallback(s) {
        $('#containerImgLoadingView').hide();

        var param = s.cpResult.split('|');
        if (param[0] == 'refresh') {
            var pageCount = parseInt(param[1]);
            if (pageCount > 0)
                $('#<%=grdROSView.ClientID %> tr:eq(1)').click();

            setPaging($("#rosPaging"), pageCount, function (page) {
                cbpROSView.PerformCallback('changepage|' + page);
            });
        }
        else
            $('#<%=grdROSView.ClientID %> tr:eq(1)').click();
    }
</script>

<input type="hidden" id="hdnLinkedVisitID" value="" runat="server" />
<div>
        <table border="0" cellpadding="1" cellspacing="0" width="99%" style="margin-top: 5px">
            <tr>
                <td>
                    <div style="position: relative;">
                        <dxcp:ASPxCallbackPanel ID="cbpROSView" runat="server" Width="100%" ClientInstanceName="cbpROSView"
                            ShowLoadingPanel="false" OnCallback="cbpROSView_Callback">
                            <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingView').show(); }"
                                EndCallback="function(s,e){ onCbpROSViewEndCallback(s); }" />
                            <PanelCollection>
                                <dx:PanelContent ID="PanelContent4" runat="server">
                                    <asp:Panel runat="server" ID="Panel3" CssClass="pnlContainerGridPatientPage4">
                                        <asp:GridView ID="grdROSView" runat="server" CssClass="grdSelected grdPatientPage"
                                            AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty"
                                            OnRowDataBound="grdROSView_RowDataBound">
                                            <Columns>
                                                <asp:BoundField DataField="ID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                                <asp:TemplateField HeaderStyle-CssClass="hiddenColumn" ItemStyle-CssClass="hiddenColumn">
                                                    <ItemTemplate>
                                                        <input type="hidden" value="<%#:Eval("ID") %>" bindingfield="ID" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left">
                                                    <HeaderTemplate>
                                                        <h3><%=GetLabel("Catatan Pemeriksaan Fisik")%></h3>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <div>
                                                            <b>
                                                                <%#: Eval("ObservationDateInString")%>,
                                                                <%#: Eval("ObservationTime") %>,
                                                                <%#: Eval("ParamedicName") %>
                                                            </b>
                                                        </div>
                                                        <div>
                                                            <asp:Repeater ID="rptReviewOfSystemDt" runat="server">
                                                                <ItemTemplate>
                                                                    <div style="padding-left: 20px; float: left; width: 300px;">
                                                                        <span <%# Eval("IsNormal").ToString() == "False" && Eval("IsNotExamined").ToString() == "False" ? "Style='color:red;font-style:italic'":"" %>>
                                                                            <strong>
                                                                                <%#: DataBinder.Eval(Container.DataItem, "ROSystem") %>
                                                                                : </strong></span>&nbsp; <span <%# Eval("IsNormal").ToString() == "False" && Eval("IsNotExamined").ToString() == "False" ? "Style='color:red;font-style:italic'":"" %>>
                                                                                    <%#: DataBinder.Eval(Container.DataItem, "cfRemarks")%></span>
                                                                    </div>
                                                                </ItemTemplate>
                                                                <FooterTemplate>
                                                                    <br style="clear: both" />
                                                                </FooterTemplate>
                                                            </asp:Repeater>
                                                        </div>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                            <EmptyDataTemplate>
                                                <%=GetLabel("Tidak ada data pemeriksaan fisik untuk pasien ini") %>
                                            </EmptyDataTemplate>
                                        </asp:GridView>
                                    </asp:Panel>
                                </dx:PanelContent>
                            </PanelCollection>
                        </dxcp:ASPxCallbackPanel>
                    </div>
                </td>
            </tr>
        </table>
</div>
<div class="imgLoadingGrdView" id="containerImgLoadingView">
    <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
</div>
<div class="containerPaging">
    <div class="wrapperPaging">
        <div id="rosPaging">
        </div>
    </div>
</div>