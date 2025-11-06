<%@ Page Language="C#" MasterPageFile="~/MasterPage/MPPatientPageList.master" AutoEventWireup="true" 
    CodeBehind="LaboratoryTestResultList.aspx.cs" Inherits="QIS.Medinfras.Web.EMR.Program.LaboratoryTestResultList" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>

<asp:Content ID="Content2" ContentPlaceHolderID="plhCustomButtonToolbar" runat="server">
    <li id="btnMPListImagingTestResult" runat="server" CRUDMode="R"><img src='<%=ResolveUrl("~/Libs/Images/Icon/tbnew.png")%>' alt="" /><div><%=GetLabel("Result")%></div></li>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="plhList" runat="server">
    <script type="text/javascript">
        $(function () {
            $('#<%=grdView.ClientID %> tr:gt(0):not(.trEmpty)').live('click', function () {
                if ($(this).attr('class') != 'selected') {
                    $('#<%=grdView.ClientID %> tr.selected').removeClass('selected');
                    $(this).addClass('selected');
                    $('#<%=hdnID.ClientID %>').val($(this).find('.keyField').html());
                    cbpViewDt.PerformCallback('refresh');
                }
            });
            $('#<%=grdView.ClientID %> tr:eq(1)').click();

            $('#<%=btnMPListImagingTestResult.ClientID %>').click(function () {
                var id = $('#<%=hdnID.ClientID %>').val();
                if (id != '') {
                    var url = ResolveUrl("~/Program/PatientPage/Objective/LaboratoryTestResult/LaboratoryTestResultEntryCtl.ascx");
                    openUserControlPopup(url, id, 'Laboratory Test Result', 1100, 500);
                }
            });
        });

        function onRefreshControl(filterExpression) {
            $('#<%=hdnFilterExpression.ClientID %>').val(filterExpression);
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

        //#region Paging Dt
        function onCbpViewDtEndCallback(s) {
            $('#containerImgLoadingViewDt').hide();

            var param = s.cpResult.split('|');
            if (param[0] == 'refresh') {
                var pageCount = parseInt(param[1]);
                if (pageCount > 0)
                    $('#<%=grdViewDt.ClientID %> tr:eq(1)').click();

                setPaging($("#pagingDt"), pageCount, function (page) {
                    cbpViewDt.PerformCallback('changepage|' + page);
                });
            }
            else
                $('#<%=grdViewDt.ClientID %> tr:eq(1)').click();
        }
        //#endregion
    </script>
    <input type="hidden" value="" id="hdnID" runat="server" />
    <input type="hidden" value="" id="hdnHealthcareServiceUnitID" runat="server" />
    <input type="hidden" value="" id="hdnLinkedVisitID" runat="server" />
    <input type="hidden" id="hdnFilterExpression" runat="server" value="" />
    <table style="width:100%">
        <colgroup>
            <col style="width:30%"/>
            <col style="width:70%"/>
        </colgroup>
        <tr>
            <td valign="top">
                <div style="position: relative;">
                    <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
                        ShowLoadingPanel="false" OnCallback="cbpView_Callback">
                        <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingView').show(); }"
                            EndCallback="function(s,e){ onCbpViewEndCallback(s); }" />
                        <PanelCollection>
                            <dx:PanelContent ID="PanelContent1" runat="server">
                                <asp:Panel runat="server" ID="pnlView" CssClass="pnlContainerGridPatientPage">
                                    <asp:GridView ID="grdView" runat="server" CssClass="grdSelected grdPatientPage" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                        <Columns>
                                            <asp:BoundField DataField="TransactionID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Left">
                                                <HeaderTemplate>
                                                    <div><%=GetLabel("Transaction Date") %> - <%=GetLabel("Time") %></div>
                                                    <div><%=GetLabel("Laboratory No") %></div>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <div><%#: Eval("TransactionDateInString")%> | <%#: Eval("TransactionTime") %></div>
                                                    <div><%#: Eval("TransactionNo")%></div>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <EmptyDataTemplate>
                                            <%=GetLabel("No Laboratory Test Result available")%>
                                        </EmptyDataTemplate>
                                    </asp:GridView>
                                </asp:Panel>
                            </dx:PanelContent>
                        </PanelCollection>
                    </dxcp:ASPxCallbackPanel>    
                    <div class="imgLoadingGrdView" id="containerImgLoadingView" >
                        <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                    </div>
                    <div class="containerPaging">
                        <div class="wrapperPaging">
                            <div id="paging"></div>
                        </div>
                    </div> 
                </div>
            </td>
            <td valign="top">
                <div style="position: relative;">
                    <dxcp:ASPxCallbackPanel ID="cbpViewDt" runat="server" Width="100%" ClientInstanceName="cbpViewDt"
                        ShowLoadingPanel="false" OnCallback="cbpViewDt_Callback">
                        <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingViewDt').show(); }"
                            EndCallback="function(s,e){ onCbpViewDtEndCallback(s); }" />
                        <PanelCollection>
                            <dx:PanelContent ID="PanelContent2" runat="server">
                                <asp:Panel runat="server" ID="Panel1" CssClass="pnlContainerGridPatientPage">
                                    <asp:GridView ID="grdViewDt" runat="server" CssClass="grdSelected grdPatientPage" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                        <Columns>
                                            <asp:BoundField DataField="ItemID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Left">
                                                <HeaderTemplate>
                                                    <div><%=GetLabel("Test Item Name") %></div>
                                                    <div><%=GetLabel("Physician") %></div>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <div><%#: Eval("ItemName1") %></div>
                                                    <div><%#: Eval("ParamedicName")%></div>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <EmptyDataTemplate>
                                            <%=GetLabel("Tidak ada detail pemeriksaan untuk pasien ini") %>
                                        </EmptyDataTemplate>
                                    </asp:GridView>
                                </asp:Panel>
                            </dx:PanelContent>
                        </PanelCollection>
                    </dxcp:ASPxCallbackPanel>    
                    <div class="imgLoadingGrdView" id="containerImgLoadingViewDt" >
                        <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                    </div>
                    <div class="containerPaging">
                        <div class="wrapperPaging">
                            <div id="pagingDt"></div>
                        </div>
                    </div> 
                </div>
            </td>
        </tr>
    </table>
    
</asp:Content>
