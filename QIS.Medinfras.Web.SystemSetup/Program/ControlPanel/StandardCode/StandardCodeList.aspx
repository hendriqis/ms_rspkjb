<%@ Page Title="" Language="C#" MasterPageFile="~/libs/MasterPage/MPList.master" AutoEventWireup="true" 
CodeBehind="StandardCodeList.aspx.cs" Inherits="QIS.Medinfras.Web.SystemSetup.Program.StandardCodeList" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="plhList" runat="server">
    <script type="text/javascript">
        $(function () {
            $('#<%=grdView.ClientID %> tr:gt(0)').live('click', function () {
                $('#<%=grdView.ClientID %> tr.selected').removeClass('selected');
                $(this).addClass('selected');
                $('#<%=hdnID.ClientID %>').val($(this).find('.keyField').html());
                if ($(this).find('.tdIsEditableByUser').find('input').is(':checked'))
                    $('#<%=hdnIsEditableByUser.ClientID %>').val('1');
                else
                    $('#<%=hdnIsEditableByUser.ClientID %>').val('0');
                cbpView1.PerformCallback('refresh');
            });
            $('#<%=grdView.ClientID %> tr:eq(1)').click();
        });

        $(function () {
            $('#<%=grdView1.ClientID %> tr:gt(0)').live('click', function () {
                $('#<%=grdView1.ClientID %> tr.selected').removeClass('selected');
                $(this).addClass('selected');
                $('#<%=hdnID1.ClientID %>').val($(this).find('.keyField').html());
            });
            $('#<%=grdView1.ClientID %> tr:eq(1)').click();
        });

        $(function () {
            $('#<%=chkIsShowAll.ClientID %>').change(function () {
                cbpView.PerformCallback('refresh');
                $('#<%=grdView1.ClientID %> tr:eq(1)').click();
            });
        });


        function onRefreshControl(filterExpression) {
            $('#<%=hdnFilterExpression.ClientID %>').val(filterExpression);
            cbpView.PerformCallback('refresh');
        }

        //#region Paging grdview
        var pageCount = parseInt('<%=PageCount %>');
        $(function () {
            setPaging($("#paging"), pageCount, function (page) {
                cbpView.PerformCallback('changepage|' + page);
            });
        });

        function onCbpViewEndCallback(s) {
            hideLoadingPanel();

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

        //#region Paging grdview1
        var pageCount = parseInt('<%=PageCount %>');
        $(function () {
            setPaging($("#paging1"), pageCount, function (page) {
                cbpView1.PerformCallback('changepage|' + page);
            });
        });

        function onCbpViewEndCallback1(s) {
            hideLoadingPanel();

            var param = s.cpResult.split('|');
            if (param[0] == 'refresh') {
                var pageCount = parseInt(param[1]);
                if (pageCount > 0)
                    $('#<%=grdView1.ClientID %> tr:eq(1)').click();

                setPaging($("#paging1"), pageCount, function (page) {
                    cbpView1.PerformCallback('changepage|' + page);
                });
            }
            else
                $('#<%=grdView1.ClientID %> tr:eq(1)').click();
        }
        //#endregion
    </script>
    <input type="hidden" value="" id="hdnID" runat="server" />
    <input type="hidden" value="" id="hdnID1" runat="server" />
    <input type="hidden" value="" id="hdnIsEditableByUser" runat="server" />
    <input type="hidden" id="hdnFilterExpression" runat="server" value="" />
    <div style="position: relative;">
        <div align="center">
            <asp:CheckBox ID="chkIsShowAll" runat="server" Text="Show All Standard Code"/>
        </div>
        <table width="100%" cellspacing="5px">
            <colgroup>
                <col width="50%" />
                <col width="50%" />
            </colgroup>
            <tr>
                <td>
                     <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
                        ShowLoadingPanel="false" OnCallback="cbpView_Callback">
                        <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }"
                            EndCallback="function(s,e){ onCbpViewEndCallback(s); }" />
                        <PanelCollection>
                            <dx:PanelContent ID="PanelContent1" runat="server">
                                <asp:Panel runat="server" ID="pnlView" CssClass="pnlContainerGrid">
                                    <asp:GridView ID="grdView" runat="server" CssClass="grdSelected" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                        <Columns>
                                            <asp:BoundField DataField="StandardCodeID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                            <asp:BoundField DataField="StandardCodeID" HeaderText="Standard Code" HeaderStyle-Width="100px" HeaderStyle-HorizontalAlign="Left" />
                                            <asp:BoundField DataField="StandardCodeName" HeaderText="Standard Code Name" HeaderStyle-HorizontalAlign="Left"/>
                                            <asp:CheckBoxField DataField="IsEditableByUser" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="100px" ItemStyle-CssClass="tdIsEditableByUser" HeaderText="Editable By User" />
                                        </Columns>
                                        <EmptyDataTemplate>
                                            <%=GetLabel("No Data To Display")%>
                                        </EmptyDataTemplate>
                                    </asp:GridView>
                                </asp:Panel>
                            </dx:PanelContent>
                        </PanelCollection>
                    </dxcp:ASPxCallbackPanel> 
                    <div class="containerPaging">
                        <div class="wrapperPaging">
                            <div id="paging"></div>
                        </div>
                    </div>   
                </td>
                <td>
                    <dxcp:ASPxCallbackPanel ID="cbpView1" runat="server" Width="100%" ClientInstanceName="cbpView1"
                        ShowLoadingPanel="false" OnCallback="cbpView1_Callback">
                        <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }"
                            EndCallback="function(s,e){ onCbpViewEndCallback1(s); }" />
                        <PanelCollection>
                            <dx:PanelContent ID="PanelContent2" runat="server">
                                <asp:Panel runat="server" ID="Panel1" CssClass="pnlContainerGrid">
                                    <asp:GridView ID="grdView1" runat="server" CssClass="grdSelected" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                        <Columns>
                                            <asp:BoundField DataField="StandardCodeID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                            <asp:BoundField DataField="StandardCodeID" HeaderText="Standard Code ID" HeaderStyle-Width="100px" HeaderStyle-HorizontalAlign="Left" />
                                            <asp:BoundField DataField="StandardCodeName" HeaderText="Standard Code Name" HeaderStyle-HorizontalAlign="Left" />
                                        </Columns>
                                        <EmptyDataTemplate>
                                            <%=GetLabel("No Data To Display")%>
                                        </EmptyDataTemplate>
                                    </asp:GridView>
                                </asp:Panel>
                            </dx:PanelContent>
                        </PanelCollection>
                    </dxcp:ASPxCallbackPanel>  
                    <div class="containerPaging">
                        <div class="wrapperPaging">
                            <div id="paging1"></div>
                        </div>
                    </div>  
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
