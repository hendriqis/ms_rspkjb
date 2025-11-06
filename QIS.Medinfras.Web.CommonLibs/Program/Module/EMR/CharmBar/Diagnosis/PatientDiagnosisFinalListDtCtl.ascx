<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PatientDiagnosisFinalListDtCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.PatientPage.PatientDiagnosisFinalListDtCtl" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<script type="text/javascript" id="dxss_PatientDiagnosisFinalListDtCtl">
    $(function () {
    });

    //#region Paging
    var pageCountDt = parseInt('<%=PageCountDt %>');
    $(function () {
        setPaging($("#pagingDt"), pageCountDt, function (page) {
            cbpDiagnosisFinalPopupDtView.PerformCallback('changepage|' + page);
        });
    });

    function oncbpDiagnosisFinalPopupDtViewEndCallback(s) {
        $('#containerImgLoadingViewDt').hide();

        var param = s.cpResult.split('|');
        if (param[0] == 'refresh') {
            var pageCount = parseInt(param[1]);
            if (pageCount > 0)
                $('#<%=grdDiagnosisFinalPopupDtView.ClientID %> tr:eq(1)').click();

            setPaging($("#pagingDt"), pageCount, function (page) {
                cbpDiagnosisFinalPopupDtView.PerformCallback('changepage|' + page);
            });
        }
        else
            $('#<%=grdDiagnosisFinalPopupDtView.ClientID %> tr:eq(1)').click();
    }
    //#endregion
</script>
<style type="text/css">
    .containerOrder
    {
        border: 1px solid #EAEAEA;
        padding: 0 5px;
    }
</style>
<input type="hidden" id="hdnDiagnosisIDCBCtl" value="" runat="server" />
<div>
    <table class="tblContentArea">
        <colgroup>
            <col width="70%" />
            <col width="20%" />
            <col width="10%" />
        </colgroup>
        <tr valign="top">
            <td colspan="2">
                <table style="width: 99%">
                    <colgroup>
                        <col style="width: 150px" />
                        <col style="width: 120px" />
                        <col style="width: 40%" />
                        <col style="width: 20%" />
                    </colgroup>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("No.RM | Pasien")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtMedicalNoCBCtl" Width="100%" runat="server" ReadOnly="true" />
                        </td>
                        <td>
                            <asp:TextBox ID="txtPatientNameCBCtl" Width="100%" runat="server" ReadOnly="true" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Diagnosa (RM)")%></label>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtDiagnosisTextCBCtl" Width="100%" runat="server" ReadOnly="true" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td colspan="3">
                <div style="position: relative;">
                    <dxcp:ASPxCallbackPanel ID="cbpDiagnosisFinalPopupDtView" runat="server" Width="99%"
                        ClientInstanceName="cbpDiagnosisFinalPopupDtView" ShowLoadingPanel="false" OnCallback="cbpDiagnosisFinalPopupDtView_Callback">
                        <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingViewDt').show(); }"
                            EndCallback="function(s,e){ oncbpDiagnosisFinalPopupDtViewEndCallback(s); }" />
                        <PanelCollection>
                            <dx:PanelContent ID="PanelDiagnosisDtViewContent1" runat="server">
                                <asp:Panel runat="server" ID="pnlDtView" CssClass="pnlContainerDtGrid" Style="height: 300px; overflow-y:auto">
                                    <asp:GridView ID="grdDiagnosisFinalPopupDtView" runat="server" CssClass="grdSelected"
                                        AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" RowStyle-CssClass="trDraggable"
                                        EmptyDataRowStyle-CssClass="trEmpty">
                                        <Columns>
                                            <asp:BoundField DataField="ID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                            <asp:BoundField DataField="cfFinalDate" ItemStyle-HorizontalAlign="Center" HeaderText="Tgl Diagnosa (RM)"
                                                HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="150px" />
                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Left">
                                                <HeaderTemplate>
                                                    <%=GetLabel("Diagnosa (RM)")%>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <div>
                                                        <span style="color: Blue; font-size: 1.1em">
                                                            <%#: Eval("cfFinalDiagnosisText")%></span> (<b><%#: Eval("FinalDiagnosisID")%></b>)
                                                    </div>
                                                    <div style="font-style: italic">
                                                        <%#: Eval("cfMorphologyInfo")%></div>
                                                    <div>
                                                        <%#: Eval("FinalICDBlockName")%></div>
                                                    <div>
                                                        <b>
                                                            <%#: Eval("DiagnoseType")%></b> -
                                                        <%#: Eval("FinalStatus")%></div>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="350px">
                                                <HeaderTemplate>
                                                    <%=GetLabel("Informasi Kunjungan")%>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <div style="font-weight: bold">
                                                        <%#: Eval("RegistrationNo")%></div>
                                                    <div>
                                                        <%#: Eval("ServiceUnitName")%></div>
                                                    <div style="font-style: italic">
                                                        <%#: Eval("ParamedicName")%></div>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <EmptyDataTemplate>
                                            <%=GetLabel("Tidak ada informasi diagnosa rekam medis untuk pasien ini")%>
                                        </EmptyDataTemplate>
                                    </asp:GridView>
                                </asp:Panel>
                            </dx:PanelContent>
                        </PanelCollection>
                    </dxcp:ASPxCallbackPanel>
                    <div class="imgLoadingGrdView" id="containerImgLoadingViewDt">
                        <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                    </div>
                    <div class="containerPaging">
                        <div class="wrapperPaging">
                            <div id="pagingDt">
                            </div>
                        </div>
                    </div>
                </div>
            </td>
        </tr>
    </table>
</div>
