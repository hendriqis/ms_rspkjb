<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="InfoPatientVentilatorLogCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.InfoPatientVentilatorLogCtl" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<div style="height: 450px; overflow-y: auto">
    <input type="hidden" value="" runat="server" id="hdnRegistrationID" />
    <input type="hidden" value="" runat="server" id="hdnRegistrationNo" />
    <table class="tblContentArea">
        <colgroup>
            <col style="width: 100%" />
        </colgroup>
        <tr>
            <td style="padding: 5px; vertical-align: top">
                <dxcp:ASPxCallbackPanel ID="cbpViewInfoPatientVentilatorLog" runat="server" Width="100%" ClientInstanceName="cbpViewInfoPatientVentilatorLog"
                    ShowLoadingPanel="false">
                    <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingView').show(); }"
                        EndCallback="function(s,e){ onCbpViewInfoPatientVentilatorLogEndCallback(s); }" />
                    <PanelCollection>
                        <dx:PanelContent ID="PanelContent1" runat="server">
                            <asp:Panel runat="server" ID="pnlView" CssClass="pnlContainerGridPatientPage">
                                <asp:ListView runat="server" ID="lvwView">
                                    <EmptyDataTemplate>
                                        <table id="tblView" runat="server" class="grdSelected" cellspacing="0" rules="all">
                                            <tr>
                                                <th style="width: 150px" rowspan="2">
                                                    <%=GetLabel("Unit Perawatan")%>
                                                </th>
                                                <th colspan="2" align="center">
                                                    <%=GetLabel("Pemasangan Ventilator")%>
                                                </th>
                                                <th colspan="2" align="center">
                                                    <%=GetLabel("Pelapasan Ventilator")%>
                                                </th>
                                                <th style="width: 150px" rowspan="2">
                                                    <%=GetLabel("Alasan Pemasangan")%>
                                                </th>
                                                <th style="width: 100px" rowspan="2" align="right">
                                                    <%=GetLabel("Size/Ukuran")%>
                                                </th>
                                            </tr>
                                            <tr>
                                                <th style="width: 170px" align="center">
                                                    <%=GetLabel("Tanggal Jam")%>
                                                </th>
                                                <th style="width: 200px">
                                                    <%=GetLabel("Oleh")%>
                                                </th>
                                                <th style="width: 170px" align="center">
                                                    <%=GetLabel("Tanggal Jam")%>
                                                </th>
                                                <th style="width: 200px">
                                                    <%=GetLabel("Oleh")%>
                                                </th>
                                            </tr>
                                            <tr class="trEmpty">
                                                <td colspan="20">
                                                    <%=GetLabel("No Data To Display")%>
                                                </td>
                                            </tr>
                                        </table>
                                    </EmptyDataTemplate>
                                    <LayoutTemplate>
                                        <table id="tblView" runat="server" class="grdNormal" cellspacing="0" rules="all">
                                            <tr>
                                                <th style="width: 150px" rowspan="2">
                                                    <%=GetLabel("Unit Perawatan")%>
                                                </th>
                                                <th colspan="2" align="center">
                                                    <%=GetLabel("Pemasangan Ventilator")%>
                                                </th>
                                                <th colspan="2" align="center">
                                                    <%=GetLabel("Pelapasan Ventilator")%>
                                                </th>
                                                <th style="width: 150px" rowspan="2">
                                                    <%=GetLabel("Alasan Pemasangan")%>
                                                </th>
                                                <th style="width: 100px" rowspan="2" align="right">
                                                    <%=GetLabel("Size/Ukuran")%>
                                                </th>
                                            </tr>
                                            <tr>
                                                <th style="width: 170px" align="center">
                                                    <%=GetLabel("Tanggal Jam")%>
                                                </th>
                                                <th style="width: 200px">
                                                    <%=GetLabel("Oleh")%>
                                                </th>
                                                <th style="width: 170px" align="center">
                                                    <%=GetLabel("Tanggal Jam")%>
                                                </th>
                                                <th style="width: 200px">
                                                    <%=GetLabel("Oleh")%>
                                                </th>
                                            </tr>
                                            <tr runat="server" id="itemPlaceholder">
                                            </tr>
                                        </table>
                                    </LayoutTemplate>
                                    <ItemTemplate>
                                        <tr>
                                            <td>
                                                <%#: Eval("ServiceUnitName")%>
                                            </td>
                                            <td align="center">
                                                <%#: Eval("cfStartDateTimeInString")%>
                                            </td>
                                            <td>
                                                <%#: Eval("ParamedicName1")%>
                                            </td>
                                            <td align="center">
                                                <%#: Eval("cfEndDateTimeInString")%>
                                            </td>
                                            <td>
                                                <%#: Eval("ParamedicName2")%>
                                            </td>
                                            <td>
                                                <%#: Eval("ETTReason")%>
                                            </td>
                                            <td align="right">
                                                <%#: Eval("cfETTSizeInString")%>
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                </asp:ListView>
                            </asp:Panel>
                        </dx:PanelContent>
                    </PanelCollection>
                </dxcp:ASPxCallbackPanel>
            </td>
        </tr>
    </table>
    <div style="width: 100%; text-align: right">
        <input type="button" value='<%= GetLabel("Tutup")%>' onclick="pcRightPanelContent.Hide();" />
    </div>
</div>
