<%@ Page Language="C#" MasterPageFile="~/MasterPage/MPPatientPageList.master" AutoEventWireup="true" 
    CodeBehind="ChiefComplaintList.aspx.cs" Inherits="QIS.Medinfras.Web.EMR.Program.ChiefComplaintList" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPopupControl" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" 
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>

<asp:Content ID="Content1" ContentPlaceHolderID="plhList" runat="server">
    <script type="text/javascript">
        $(function () {
            $('#<%=grdView.ClientID %> tr:gt(0):not(.trEmpty)').live('click', function () {
                $('#<%=grdView.ClientID %> tr.selected').removeClass('selected');
                $(this).addClass('selected');
                $('#<%=hdnID.ClientID %>').val($(this).find('.keyField').html());
            });
            $('#<%=grdView.ClientID %> tr:eq(1)').click();
        });

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
    </script>
    <input type="hidden" value="" id="hdnID" runat="server" />
    <input type="hidden" id="hdnFilterExpression" runat="server" value="" />
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
                                <asp:BoundField DataField="ID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                <asp:TemplateField HeaderStyle-HorizontalAlign="Left">
                                    <HeaderTemplate>
                                        <div>
                                            <b>
                                                <span style="float:left;width:130px;"><%=GetLabel("Date")%></span>
                                                <span style="float:left;width:50px;"><%=GetLabel("Time")%></span>
                                                <span style="margin-left:40px"><%=GetLabel("Physician")%></span>
                                            </b>
                                        </div>
                                        <div><%=GetLabel("Chief Complaint")%></div>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <div>
                                            <div>
                                                <b>
                                                    <span style="float:left;width:130px;"><%#: Eval("ObservationDateInString")%></span>
                                                    <span style="float:left;width:50px;"><%#: Eval("ObservationTime")%></span>
                                                    <span style="margin-left:40px"><%#: Eval("ParamedicName")%></span>
                                                </b>
                                            </div>
                                            <div><%#: Eval("ChiefComplaintText")%></div>
                                            <div>
                                                <table cellpadding="0" cellspacing="0">
                                                    <colgroup>
                                                        <col style="width:90px"/>
                                                        <col style="width:400px"/>
                                                        <col style="width:90px"/>
                                                        <col style="width:400px"/>
                                                        <col style="width:90px"/>
                                                        <col style="width:400px"/>
                                                    </colgroup>
                                                    <tr>
                                                        <td><%=GetLabel("Location")%></td>
                                                        <td>: <%#: Eval("Location")%></td>
                                                        <td><%=GetLabel("Quality")%></td>
                                                        <td>: <%#: Eval("DisplayQuality")%></td>
                                                        <td><%=GetLabel("Relieved By")%></td>
                                                        <td>: <%#: Eval("DisplayRelieved")%></td>
                                                    </tr>
                                                    <tr>
                                                        <td><%=GetLabel("Onset")%></td>
                                                        <td>: <%#: Eval("DisplayOnset")%></td>
                                                        <td><%=GetLabel("Severity")%></td>
                                                        <td>: <%#: Eval("DisplaySeverity")%></td>
                                                    </tr>
                                                    <tr>
                                                        <td><%=GetLabel("Provocation")%></td>
                                                        <td>: <%#: Eval("DisplayProvocation")%></td>
                                                        <td><%=GetLabel("Time")%></td>
                                                        <td>: <%#: Eval("DisplayCourse")%></td>
                                                    </tr>
                                                </table>
                                            </div>
                                        </div>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <EmptyDataTemplate>
                                <%=GetLabel("No Data To Display")%>
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
</asp:Content>
