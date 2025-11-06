<%@ Page Title="" Language="C#" MasterPageFile="~/Libs/MasterPage/MPList.master" AutoEventWireup="true" 
    CodeBehind="ViewErrorLogList.aspx.cs" Inherits="QIS.Medinfras.Web.SystemSetup.Program.ViewErrorLogList" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="plhList" runat="server">
    <script type="text/javascript" src='<%= ResolveUrl("~/Libs/Scripts/CustomGridViewList.js")%>'></script>
    <script type="text/javascript">
        $(function () {
            setDatePicker('<%=txtLogDate.ClientID %>');
            $('#<%=txtLogDate.ClientID %>').datepicker('option', 'maxDate', '0');

            $('#<%=txtLogDate.ClientID %>').change(function () {
                $('#divErrorDetail').html('');
                cbpView.PerformCallback('refresh');
            });

            $('#<%=grdView.ClientID %> tr:eq(1)').click();
        });

        $('#<%=grdView.ClientID %> > tbody > tr:gt(0):not(.trDetail):not(.trEmpty)').live('click', function () {
            var detail = $(this).find('.keyField').html();
            $('#<%=grdView.ClientID %> tr.selected').removeClass('selected');
            $(this).addClass('selected');
            $('#divErrorDetail').html('');
            $('#divErrorDetail').append(convert(detail));
        });

        var convert = function (convert) {
            return $('<span />', { html: convert }).text();
        }

        function onGetCurrID() {
            return $('#<%=hdnID.ClientID %>').val();
        }

        function onGetFilterExpression() {
            return $('#<%=hdnFilterExpression.ClientID %>').val();
        }

        //#region Paging
        var pageCount = parseInt('<%=PageCount %>');
        var currPage = parseInt('<%=CurrPage %>');
        $(function () {
            setPaging($("#paging"), pageCount, function (page) {
                cbpView.PerformCallback('changepage|' + page);
            }, null, currPage);
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
    </script>
    <input type="hidden" value="" id="hdnID" runat="server" />
    <input type="hidden" id="hdnFilterExpression" runat="server" value="" />
    <div style="position: relative;">
        <table class="tblEntryContent" style="width:60%;">
            <colgroup>
                <col style="width:120px"/>
                <col/>
            </colgroup>
            <tr>
                <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Log Date")%></label></td>
                <td><asp:TextBox ID="txtLogDate" Width="120px" runat="server" CssClass="datepicker" /></td>
            </tr>
        </table>
        <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
            ShowLoadingPanel="false" OnCallback="cbpView_Callback">
            <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }"
                EndCallback="function(s,e){ onCbpViewEndCallback(s); }" />
            <PanelCollection>
                <dx:PanelContent ID="PanelContent1" runat="server">
                    <asp:Panel runat="server" ID="pnlView" CssClass="pnlContainerGrid">
                        <asp:GridView ID="grdView" runat="server" CssClass="grdSelected" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                            <Columns>
                                <asp:BoundField DataField="ErrorDetail" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                <asp:BoundField DataField="Time" HeaderText="Time" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="50px" />
                                <asp:BoundField DataField="ModuleID" HeaderText="Module" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="IPAddress" HeaderText="IP Address" HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="PageUrl" HeaderText="URL" HeaderStyle-Width="650px" />
                                <asp:BoundField DataField="ErrorMessage" HeaderText="Message" />
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
        
        Error detail :
        <div style="background-color: White; padding: 5px; color: Red;">
            <div id="divErrorDetail"></div> 
        </div>
    </div>
</asp:Content>