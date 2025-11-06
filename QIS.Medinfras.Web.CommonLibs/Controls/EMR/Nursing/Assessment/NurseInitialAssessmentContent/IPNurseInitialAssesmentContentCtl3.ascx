<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="IPNurseInitialAssesmentContentCtl3.ascx.cs" 
    Inherits="QIS.Medinfras.Web.CommonLibs.Controls.IPNurseInitialAssesmentContentCtl3" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>

<script type="text/javascript" id="dxss_nurseInitialAssessmentctl3">
    $(function () {
    });

    var pageCount = parseInt('<%=gridVitalSignPageCount %>');
    $(function () {
        setPaging($("#vitalSignPaging"), pageCount, function (page) {
            cbpVitalSignView.PerformCallback('changepage|' + page);
        });
    });

    function onCbpVitalSignViewEndCallback(s) {
        var param = s.cpResult.split('|');
        if (param[0] == 'refresh') {
            var pageCount = parseInt(param[1]);
            if (pageCount > 0)
                $('#<%=grdVitalSignView.ClientID %> tr:eq(1)').click();

            setPaging($("#vitalSignPaging"), pageCount, function (page) {
                cbpVitalSignView.PerformCallback('changepage|' + page);
            });
        }
        else
            $('#<%=grdVitalSignView.ClientID %> tr:eq(1)').click();
    }
</script>

<input type="hidden" id="hdnLinkedVisitID" value="" runat="server" />
<div class="containerTblEntryContent">
    <table border="0" cellpadding="1" cellspacing="0" width="100%" style="margin-top: 5px">
        <colgroup>
            <col width="150px" />
            <col />
        </colgroup>
        <tr>
            <td colspan="2">
                <div>
                    <dxcp:ASPxCallbackPanel ID="cbpVitalSignView" runat="server" Width="100%" ClientInstanceName="cbpVitalSignView"
                        ShowLoadingPanel="false" OnCallback="cbpVitalSignView_Callback">
                        <ClientSideEvents EndCallback="function(s,e){ onCbpVitalSignViewEndCallback(s); }" />
                        <PanelCollection>
                            <dx:PanelContent ID="PanelContent1" runat="server">
                                <asp:Panel runat="server" ID="pnlView" CssClass="pnlContainerGridPatientPage4">
                                    <asp:GridView ID="grdVitalSignView" runat="server" CssClass="grdSelected grdPatientPage"
                                        AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty"
                                        OnRowDataBound="grdVitalSignView_RowDataBound">
                                        <Columns>
                                            <asp:BoundField DataField="ID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Left">
                                                <HeaderTemplate>
                                                    <h3><%=GetLabel("Tanda Vital dan Indikator Lainnya")%></h3> 
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <div>
                                                        <b>
                                                            <%#: Eval("ObservationDateInString")%>,
                                                            <%#: Eval("ObservationTime") %>,
                                                            <%#: Eval("ParamedicName") %>
                                                        </b>
                                                        <br />
                                                        <span style="font-style:italic">
                                                            <%#: Eval("Remarks") %>
                                                        </span>
                                                        <br />
                                                    </div>
                                                    <div>
                                                        <asp:Repeater ID="rptVitalSignDt" runat="server">
                                                            <ItemTemplate>
                                                                <div style="padding-left: 20px; float: left; width: 350px;">
                                                                    <strong>
                                                                        <div style="width: 110px; float: left;" class="labelColumn">
                                                                            <%#: DataBinder.Eval(Container.DataItem, "VitalSignLabel") %></div>
                                                                        <div style="width: 20px; float: left;">
                                                                            :</div>
                                                                    </strong>
                                                                    <div style="float: left;">
                                                                        <%#: DataBinder.Eval(Container.DataItem, "DisplayVitalSignValue") %></div>
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
                                            <%=GetLabel("Tidak ada pemeriksaan tanda vital") %>
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
<div class="containerPaging">
    <div class="wrapperPaging">
        <div id="vitalSignPaging">
        </div>
    </div>
</div>

