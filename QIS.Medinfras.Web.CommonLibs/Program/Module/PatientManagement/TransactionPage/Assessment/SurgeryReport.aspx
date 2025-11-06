<%@ Page Title="" Language="C#" MasterPageFile="~/libs/MasterPage/PatientPage/MPBasePatientPageList.master"
    AutoEventWireup="true" CodeBehind="SurgeryReport.aspx.cs" Inherits="QIS.Medinfras.Web.CommonLibs.Program.SurgeryReport" %>

<%@ Register Src="~/Libs/Program/Module/PatientManagement/TransactionPage/TransactionPageToolbarCtl.ascx"
    TagName="ToolbarCtl" TagPrefix="uc1" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<asp:content id="Content3" contentplaceholderid="plhMPPatientPageNavigationPane"
    runat="server">
    <uc1:ToolbarCtl ID="ctlToolbar" runat="server" />
</asp:content>
<asp:content id="Content2" contentplaceholderid="plhMenuTitle" runat="server">
    <div class="menuTitle">
        <%=HttpUtility.HtmlEncode(GetPageTitle())%></div>
</asp:content>
<asp:content id="Content1" contentplaceholderid="plhList" runat="server">
    <script type="text/javascript" id="dxss_erphysicalexamlist">
        $(function () {
            $('#<%=grdView.ClientID %> tr:gt(0):not(.trEmpty)').live('click', function () {
                $('#<%=grdView.ClientID %> tr.selected').removeClass('selected');
                $(this).addClass('selected');
                $('#<%=hdnID.ClientID %>').val($(this).find('.keyField').html());
            });
            $('#<%=grdView.ClientID %> tr:eq(1)').click();
        });

        function onRefreshControl(filterExpression) {
            cbpView.PerformCallback('refresh');
        }

        function onAfterSaveRecordPatientPageEntry() {
            cbpView.PerformCallback('refresh');
        }

        //#region Paging
        var pageCount = parseInt('<%=PageCount %>');
        $(function () {
            setPaging($("#paging"), pageCount, function (page) {
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

                setPaging($("#paging"), pageCount, function (page) {
                    cbpView.PerformCallback('changepage|' + page);
                });
            }
            else
                $('#<%=grdView.ClientID %> tr:eq(1)').click();
        }
        //#endregion

        //#region onBeforeRightPanelPrint
        function onBeforeRightPanelPrint(code, filterExpression, errMessage) {
            var PatientSurgeryID = $('#<%=hdnID.ClientID %>').val();
            if (code == 'MD-00001') {
                filterExpression.text = 'PatientSurgeryID = ' + PatientSurgeryID;
            }
            return true;
        }
        //#endregion
    </script>
    <input type="hidden" value="" id="hdnPageTitle" runat="server" />
    <input type="hidden" value="" id="hdnID" runat="server" />
    <input type="hidden" id="hdnFilterExpression" runat="server" value="" />
    <input type="hidden" value="" id="hdnDepartmentID" runat="server" />
    <input type="hidden" value="" id="hdnMRN" runat="server" />
    <input type="hidden" value="" id="hdnVisitID" runat="server" />
    <div style="position: relative;">
        <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
            ShowLoadingPanel="false" OnCallback="cbpView_Callback">
            <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingView').show(); }"
                EndCallback="function(s,e){ onCbpViewEndCallback(s); }" />
            <PanelCollection>
                <dx:PanelContent ID="PanelContent1" runat="server">
                    <asp:panel runat="server" id="pnlView" cssclass="pnlContainerGridPatientPage">
                        <asp:gridview id="grdView" runat="server" cssclass="grdSelected grdPatientPage" autogeneratecolumns="false"
                            showheaderwhenempty="false" emptydatarowstyle-cssclass="trEmpty" showheader="true">
                            <columns>
                                <asp:BoundField DataField="PatientSurgeryID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                <asp:BoundField DataField="cfStartDateInString" HeaderText="Waktu Mulai" HeaderStyle-Width="50px"
                                    HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="cfEndDateInString" HeaderText="Waktu Selesai" HeaderStyle-Width="50px" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="Duration" HeaderText="Durasi" HeaderStyle-Width="60px" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="Center" />
                                <asp:TemplateField HeaderText="Tindakan Operasi" HeaderStyle-Width="200px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left">
                                    <ItemTemplate>
                                        <div>
                                            <span style="color: blue; font-style: italic; vertical-align:top">
                                                <%#:Eval("ItemName1") %>
                                            </span>
                                        </div>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Diagnosa" HeaderStyle-Width="200px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left">
                                    <ItemTemplate>
                                        <div>
                                            <span style="color: black; vertical-align:top">
                                                <%#:Eval("cfPreDiagnose") %>
                                            </span>
                                        </div>
                                        <div>
                                            <span style="color: black; vertical-align:top">
                                                <%#:Eval("cfPastDiagnose") %>
                                            </span>
                                        </div>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Dibuat Oleh" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="80px">
                                    <ItemTemplate>
                                        <div style="color: blue; font-style: italic; vertical-align:top">
                                            <%#:Eval("cfCreatedDateInString") %>,                                            
                                        </div>
                                        <div>
                                            <b><%#:Eval("CreatedByUserName") %></b>
                                        </div>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </columns>
                            <emptydatatemplate>
                                <%=GetLabel("Tidak ada data operasi untuk pasien ini") %>
                            </emptydatatemplate>
                        </asp:gridview>
                    </asp:panel>
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
</asp:content>
