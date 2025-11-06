<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="InfoSurgeryReportListCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.MedicalRecord.Program.InfoSurgeryReportListCtl" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="QIS.Medinfras.Web.CustomControl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"
    Namespace="QIS.Medinfras.Web.CustomControl" TagPrefix="qis" %>
<script type="text/javascript" id="dxss_surgeryreportlistctl">

    $('.btnPrintSurgeryReportList.imglink').die('click');
    $('.btnPrintSurgeryReportList.imglink').live('click', function () {
        $row = $(this).closest('tr');
        var entity = rowToObject($row);
        var patientSurgeryID = $row.find('.hdnID').val();
        var visitID = $('#<%=hdnVisitID.ClientID %>').val();
        var reportCode = $('#<%=hdnRptCodeSurgery.ClientID %>').val();
        var filterExpression = visitID + '|' + patientSurgeryID;
        $('#<%=hdnID.ClientID %>').val(patientSurgeryID);

        openReportViewer(reportCode, filterExpression);
    });
</script>
<input type="hidden" value="" id="hdnRptCodeSurgery" runat="server" />
<input type="hidden" value="" id="hdnVisitID" runat="server" />
<input type="hidden" value="" id="hdnHealthcareServiceUnitID" runat="server" />
<input type="hidden" value="" id="hdnPageTitle" runat="server" />
<input type="hidden" value="" id="hdnID" runat="server" />
<div>
    <table class="tblContentArea" style="width: 100%">
        <tr>
            <td>
                <div style="padding: 5px; min-height: 300px;">
                    <dxcp:ASPxCallbackPanel ID="cbpViewSurgey" runat="server" Width="100%" ClientInstanceName="cbpViewSurgey"
                        ShowLoadingPanel="false" OnCallback="cbpViewSurgey_Callback">
                        <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e) { onCbpViewSurgeyEndCallback(s); }" />
                        <PanelCollection>
                            <dx:PanelContent ID="PanelContent1" runat="server">
                                <asp:Panel runat="server" ID="pnlService" Style="width: 100%; margin-left: auto;
                                    margin-right: auto; position: relative; font-size: 0.95em;">
                                    <asp:ListView ID="lvwView" runat="server">
                                        <EmptyDataTemplate>
                                            <table id="tblView" runat="server" class="grdSelected notAllowSelect grdSurgery"
                                                cellspacing="0" rules="all">
                                                <tr>
                                                    <th style="width: 150px" align="left">
                                                        <%= GetLabel("Nomor Order")%>
                                                    </th>
                                                    <th style="width: 120px" align="center">
                                                        <%= GetLabel("Tanggal Order")%>
                                                    </th>
                                                   
                                                    <th style="width: 140px" align="left">
                                                        <%=GetLabel("Dokter")%>
                                                    </th>
                                                    <th style="width: 200px" align="left">
                                                        <%=GetLabel("Pre Diagnosis")%>
                                                    </th>
                                                    <th style="width: 200px" align="left">
                                                        <%=GetLabel("Post Diagnosis")%>
                                                    </th>
                                                    <th style="width: 50px" align="center">
                                                        <%=GetLabel("Print")%>
                                                    </th>
                                                </tr>
                                                <tr class="trEmpty">
                                                    <td colspan="7">
                                                        <%=GetLabel("Tidak ada informasi Laporan Operasi pada saat ini") %>
                                                    </td>
                                                </tr>
                                            </table>
                                        </EmptyDataTemplate>
                                        <LayoutTemplate>
                                            <table id="tblView" runat="server" class="grdSelected notAllowSelect grdSurgery"
                                                cellspacing="0" rules="all">
                                                <tr>
                                                    <th style="width: 150px" align="left">
                                                        <%= GetLabel("Nomor Order")%>
                                                    </th>
                                                    <th style="width: 120px" align="center">
                                                        <%= GetLabel("Tanggal Order")%>
                                                    </th>
                                                   
                                                    <th style="width: 140px" align="left">
                                                        <%=GetLabel("Dokter")%>
                                                    </th>
                                                    <th style="width: 200px" align="left">
                                                        <%=GetLabel("Pre Diagnosis")%>
                                                    </th>
                                                    <th style="width: 200px" align="left">
                                                        <%=GetLabel("Post Diagnosis")%>
                                                    </th>
                                                    <th style="width: 50px" align="center">
                                                        <%=GetLabel("Print")%>
                                                    </th>
                                                </tr>
                                                <tr runat="server" id="itemPlaceholder">
                                                </tr>
                                            </table>
                                        </LayoutTemplate>
                                        <ItemTemplate>
                                            <tr runat="server" id="trItem">
                                                <td align="left" style="width:150px">
                                                    <div>
                                                        <%#: Eval("TestOrderNo") %></div>
                                                </td>
                                                <td align="center" style="width:120px">
                                                    <div>
                                                        <%#: Eval("cfReportDate") %></div>
                                                </td>
                                                
                                                <td align="left">
                                                    <div>
                                                        <input type="hidden" class="hdnKeyField" value="<%#: Eval("PatientSurgeryID")%>" />
                                                        <%#: Eval("ParamedicName") %></div>
                                                </td>
                                                <td align="left">
                                                    <div>
                                                        <%#: Eval("PreOperativeDiagnosisText") %></div>
                                                </td>
                                                <td align="left">
                                                    <div>
                                                        <%#: Eval("PostOperativeDiagnosisText") %></div>
                                                </td>
                                                <td align="center" style="width:50px">
<%--                                                    <div id="ddlPrintSurgeryReportList" runat="server" style='margin-top: 5px; text-align: center;'  >--%>
                                                    <img class="btnPrintSurgeryReportList imglink" title='<%=GetLabel("Laporan Operasi Versi Lengkap")%>' alt="" src='<%# ResolveUrl("~/Libs/Images/Button/print.png")%>' />
                                                      <%--  alt="" style="float: left; margin-right: 2px;" />
                                                        <input type="button" id="btnPrintSurgeryReportList" runat="server" class="btnPrintSurgeryReportList w3-btn w3-hover-blue"
                                                            value="Laporan Operasi" style='width: 100px; background-color: Green; color: White;' />--%>
<%--                                                    </div>--%>
<%--                                                    <input type="hidden" value="<%#:Eval("patientSurgeryID") %>" bindingfield="PatientSurgeryID" />
--%>                                                    <input type="hidden" value="<%#:Eval("VisitID") %>" bindingfield="VisitID" />
                                                    <input type="hidden" class="hdnID" value="<%#: Eval("PatientSurgeryID")%>" />
                                                </td>
                                            </tr>
                                        </ItemTemplate>
                                    </asp:ListView>
                                </asp:Panel>
                            </dx:PanelContent>
                        </PanelCollection>
                    </dxcp:ASPxCallbackPanel>
                </div>
            </td>
        </tr>
    </table>
</div>
