<%@ Page Title="" Language="C#" MasterPageFile="~/Libs/MasterPage/PatientPage/MPBasePatientPageTrx.master"
    AutoEventWireup="true" CodeBehind="SurgeryReportList.aspx.cs" Inherits="QIS.Medinfras.Web.CommonLibs.Program.SurgeryReportList" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Src="~/Libs/Program/Module/PatientManagement/TransactionPage/TransactionPageToolbarCtl.ascx"
    TagName="ToolbarCtl" TagPrefix="uc1" %>
<asp:Content ID="Content5" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div class="menuTitle">
        <%=HttpUtility.HtmlEncode(GetPageTitle())%></div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="plhCustomButtonToolbar" runat="server">
    <li id="btnRefresh" crudmode="R" runat="server">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbrefresh.png")%>' alt="" /><br style="clear: both" />
        <div>
            <%=GetLabel("Refresh")%></div>
    </li>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="plhMPPatientPageNavigationPane"
    runat="server">
    <uc1:ToolbarCtl ID="ctlToolbar" runat="server" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="plhHeader" runat="server">
    <input type="hidden" value="" id="hdnVisitID" runat="server" />
    <input type="hidden" value="" id="hdnHealthcareServiceUnitID" runat="server" />
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript">
        $(function () {
            $('.grdSurgery tr:gt(0):not(.trEmpty)').click(function () {
                if ($('.grdSurgery tr').index($(this)) > -1) {
                    $('.grdSurgery tr.selected').removeClass('selected');
                    $(this).addClass('selected');
                    $('#<%=hdnID.ClientID %>').val($(this).find('.hdnKeyField').val());
                }
            });

            $('.grdSurgery tr:eq(1)').click();
        });

        $('#<%=btnRefresh.ClientID %>').live('click', function () {
            cbpView.PerformCallback();
        });

        function onAfterCustomClickSuccess(type) {
            cbpView.PerformCallback();
        }

        function onCbpViewEndCallback(s) {
            hideLoadingPanel();
        }

        function onBeforeRightPanelPrint(code, filterExpression, errMessage) {
            var visitID = $('#<%=hdnVisitID.ClientID %>').val();
            var patientSurgeryID = $('#<%=hdnID.ClientID %>').val();
            if (code == 'PM-00567' || code == 'PM-00577' || code == 'PM-00591' || code == 'PM-00598' || code == 'PM-005671' || code == 'PM-90086' || code == 'PM-90128') {
                filterExpression.text = visitID + '|' + patientSurgeryID;
            }
            else {
                filterExpression.text = patientSurgeryID;
            }

            return true;
        }

    </script>
    <input type="hidden" value="" id="hdnPageTitle" runat="server" />
    <input type="hidden" value="" id="hdnID" runat="server" />
    <div>
        <table class="tblContentArea" style="width: 100%">
            <tr>
                <td>
                    <div style="padding: 5px; min-height: 150px;">
                        <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
                            ShowLoadingPanel="false" OnCallback="cbpView_Callback">
                            <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e) { onCbpViewEndCallback(s); }" />
                            <PanelCollection>
                                <dx:PanelContent ID="PanelContent1" runat="server">
                                    <asp:Panel runat="server" ID="pnlService" Style="width: 100%; margin-left: auto;
                                        margin-right: auto; position: relative; font-size: 0.95em;">
                                        <asp:ListView ID="lvwView" runat="server">
                                            <EmptyDataTemplate>
                                                <table id="tblView" runat="server" class="grdSelected notAllowSelect grdSurgery" cellspacing="0" rules="all" >
                                                    <tr>
                                                        <th style="width: 80px">
                                                            <%= GetLabel("Nomor Order")%>
                                                        </th>
                                                         <th style="width: 60px">
                                                            <%= GetLabel("Tanggal")%>
                                                        </th>
                                                        <th style="width: 20px" align="center">
                                                            <%= GetLabel("Jam")%>
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
                                                        <th style="width: 80px">
                                                                <%= GetLabel("Nomor Order")%>
                                                        </th>
                                                        <th style="width: 60px">
                                                                <%= GetLabel("Tanggal")%>
                                                        </th>
                                                        <th style="width: 20px" align="center">
                                                            <%= GetLabel("Jam")%>
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
                                                    </tr>
                                                    <tr runat="server" id="itemPlaceholder">
                                                    </tr>
                                                </table>
                                            </LayoutTemplate>
                                            <ItemTemplate>
                                                <tr runat="server" id="trItem">
                                             <td align="center">
                                                        <div>
                                                            <%#: Eval("TestOrderNo") %></div>
                                                    </td>
                                                 <td align="center">
                                                        <div>
                                                            <%#: Eval("cfReportDate") %></div>
                                                    </td>
                                                 <td align="center">
                                                        <div>
                                                            <%#: Eval("ReportTime") %></div>
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
                                                </tr>
                                            </ItemTemplate>
                                        </asp:ListView>
                                        <div class="imgLoadingGrdView" id="containerImgLoadingService">
                                            <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                                        </div>
                                    </asp:Panel>
                                </dx:PanelContent>
                            </PanelCollection>
                        </dxcp:ASPxCallbackPanel>
                    </div>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
