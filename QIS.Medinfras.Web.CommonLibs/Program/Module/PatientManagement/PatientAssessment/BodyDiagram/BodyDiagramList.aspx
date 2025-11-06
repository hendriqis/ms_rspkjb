<%@ Page Title="" Language="C#" MasterPageFile="~/libs/MasterPage/PatientPage/MPBasePatientPageList.master"
    AutoEventWireup="true" CodeBehind="BodyDiagramList.aspx.cs" Inherits="QIS.Medinfras.Web.CommonLibs.Program.BodyDiagramList" %>

<%@ Register Src="~/Libs/Program/Module/PatientManagement/PatientAssessment/PhysicalExaminationToolbarCtl.ascx"
    TagName="ToolbarCtl" TagPrefix="uc1" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<asp:Content ID="Content3" ContentPlaceHolderID="plhMPPatientPageNavigationPane"
    runat="server">
    <uc1:ToolbarCtl ID="ctlToolbar" runat="server" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="plhCustomButtonToolbar" runat="server">
    <li id="btnRecordPrev" runat="server" CRUDMode="R"><img src='<%=ResolveUrl("~/Libs/Images/Icon/tbback.png")%>' alt="" /><div><%=GetLabel("Prev")%></div></li>
    <li id="btnRecordNext" runat="server" CRUDMode="R"><img src='<%=ResolveUrl("~/Libs/Images/Icon/tbnext.png")%>' alt="" /><div><%=GetLabel("Next")%></div></li>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div class="menuTitle">
        <%=HttpUtility.HtmlEncode(GetPageTitle())%></div>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhList" runat="server">
    <script type="text/javascript" id="dxss_erinitialphysicalexam">
        $(function () {
            $('#<%=btnRecordPrev.ClientID %>').click(function () {
                if ($('#<%=hdnPageCount.ClientID %>').val() != '0')
                    cbpView.PerformCallback('prev');
            });
            $('#<%=btnRecordNext.ClientID %>').click(function () {
                if ($('#<%=hdnPageCount.ClientID %>').val() != '0')
                    cbpView.PerformCallback('next');
            });
        });

        function onRefreshControl(filterExpression) {
            if (filterExpression == 'edit')
                cbpView.PerformCallback('edit');
            else
                cbpView.PerformCallback('refresh');
        }

        function onCbpViewEndCallback(s) {
            var param = s.cpResult.split('|');
            if (param[0] == 'count') {
                if (param[1] != '0') {
                    $('#<%=divBodyDiagram.ClientID %>').show();
                    $('#<%=tblEmpty.ClientID %>').hide();
                }
                else {
                    $('#<%=divBodyDiagram.ClientID %>').hide();
                    $('#<%=tblEmpty.ClientID %>').show();
                }

                $('#<%=hdnPageCount.ClientID %>').val(param[1]);
                $('#<%=hdnPageIndex.ClientID %>').val('0');
            }
            else if (param[0] == 'index')
                $('#<%=hdnPageIndex.ClientID %>').val(param[1]);
            hideLoadingPanel();
        }
    </script>
    <style type="text/css">
        .templatePatientBodyDiagram             { padding: 10px; }
        .templatePatientBodyDiagram .containerImage { float:left; display: table-cell; vertical-align:middle; border: 1px solid #AAA; width:300px; height:300px; margin-right: 20px; text-align:center; position:relative; }
        .templatePatientBodyDiagram .containerImage img { max-height:300px; max-width:300px; position:absolute;top:0;bottom:0;left:0;right:0;margin:auto; }
        .templatePatientBodyDiagram .spLabel    { display: inline-block; width: 120px; font-weight:bolder; }
        .templatePatientBodyDiagram .spValue    { margin-left:10px; }
    </style>
    <input type="hidden" value="" id="hdnPageTitle" runat="server" />
    <input type="hidden" id="hdnPageCount" runat="server" value='0' /> 
    <input type="hidden" id="hdnPageIndex" runat="server" value='0' />   
    <input type="hidden" value="" id="hdnDepartmentID" runat="server" />
    <div style="position: relative;" id="divBodyDiagram" runat="server" >
        <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
            ShowLoadingPanel="false" OnCallback="cbpView_Callback">
            <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }"
                EndCallback="function(s,e){ onCbpViewEndCallback(s); }" />
            <PanelCollection>
                <dx:PanelContent ID="PanelContent1" runat="server">
                    <asp:Panel runat="server" ID="pnlView" CssClass="pnlContainerGrid">
                        <div class="templatePatientBodyDiagram">
                            <input type="hidden" id="hdnBodyDiagramID" runat="server" value='' />  
                            <div class="containerImage boxShadow">
                                <img src='' alt="" id="imgBodyDiagram" runat="server" />  
                            </div>
                            <span class="spLabel"><%=GetLabel("Gambar") %></span>:<span class="spValue" id="spnDiagramName" runat="server"></span><br />
                            <span class="spLabel"><%=GetLabel("Catatan") %></span>: <br />
                            <asp:Repeater ID="rptRemarks" runat="server">
                                <HeaderTemplate>
                                    <table>
                                    <colgroup width="20px" />
                                    <colgroup width="2px" />
                                    <colgroup width="15px" />
                                    <colgroup width="2px" />
                                    <colgroup width="60px" />
                                    <colgroup width="2px" />
                                    <colgroup width="*" />
                                    <colgroup width="16px" />
                                    <colgroup width="16px" />               
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <tr>
                                    <td><img alt="" style="width:16px;height:16px" src="<%#: ResolveUrl((string)Eval("SymbolImageUrl"))%>"/></td>
                                    <td>:</td>
                                    <td><%#: DataBinder.Eval(Container.DataItem, "SymbolCode")%></td>
                                    <td>:</td>
                                    <td><%#: DataBinder.Eval(Container.DataItem, "SymbolName")%></td>
                                    <td>:</td>
                                    <td><%#: DataBinder.Eval(Container.DataItem, "Remarks")%></td>
                                    </tr>
                                </ItemTemplate>
                                <FooterTemplate>
                                    </table>
                                </FooterTemplate>
                            </asp:Repeater>
                            <br />
                            <span class="spLabel"><%=GetLabel("Physician") %></span>:<span class="spValue" id="spnParamedicName" runat="server"></span><br />
                            <span class="spLabel"><%=GetLabel("Date/Time")%></span>:<span class="spValue" id="spnObservationDateTime" runat="server"></span><br />
                        </div>
                    </asp:Panel>
                </dx:PanelContent>
            </PanelCollection>
        </dxcp:ASPxCallbackPanel> 
    </div>
    <table id="tblEmpty" style="display:none;width:100%" runat="server">
        <tr class="trEmpty">
            <td align="center" valign="middle"><%=GetLabel("Tidak ada data pemeriksaan dengan penanda gambar") %></td>
        </tr>
    </table> 
</asp:Content>
