<%@ Page Language="C#" MasterPageFile="~/Libs/MasterPage/MPList.master" AutoEventWireup="true" 
    CodeBehind="LocationList.aspx.cs" Inherits="QIS.Medinfras.Web.SystemSetup.Program.LocationList" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>

<asp:Content ID="Content1" ContentPlaceHolderID="plhList" runat="server">
    <script type="text/javascript" src='<%= ResolveUrl("~/Libs/Scripts/CustomGridViewList.js")%>'></script>
    <script type="text/javascript">
        $(function () {
            var grd = new customGridView();
            grd.init('<%=grdView.ClientID %>', '<%=hdnID.ClientID %>', '<%=pnlView.ClientID %>', cbpView, 'paging');
        });

        function onCboHealthcareValueChanged(s) {
            cbpView.PerformCallback('refresh');
        }

        function onRefreshControl(filterExpression) {
            $('#<%=hdnFilterExpression.ClientID %>').val(filterExpression);
            cbpView.PerformCallback('refresh');
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
                else
                    $('#<%=hdnID.ClientID %>').val('');

                setPaging($("#paging"), pageCount, function (page) {
                    cbpView.PerformCallback('changepage|' + page);
                });
            }
            else
                $('#<%=grdView.ClientID %> tr:eq(1)').click();
        }
        //#endregion

        $('.lnkItem a').live('click', function () {
            var id = $(this).closest('tr').find('.keyField').html();
            var url = ResolveUrl("~/Program/Master/Location/LocationItemEntryCtl.ascx");
            openUserControlPopup(url, id, 'Item Balance', 900, 500);
        });
    </script>
    <input type="hidden" value="" id="hdnID" runat="server" />
    <input type="hidden" id="hdnFilterExpression" runat="server" value="" />
    <table cellpadding="0" cellspacing="0">
        <tr>
            <td style="width:100px"><%=GetLabel("Healthcare") %></td>
            <td>
                <dxe:ASPxComboBox ID="cboHealthcare" runat="server" ClientInstanceName="cboHealthcare" Width="150px">
                    <ClientSideEvents ValueChanged="function(s,e){ onCboHealthcareValueChanged(s); }" />
                </dxe:ASPxComboBox>
            </td>
        </tr>
    </table>
    <div style="position: relative;">
        <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
            ShowLoadingPanel="false" OnCallback="cbpView_Callback">
            <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }"
                EndCallback="function(s,e){ onCbpViewEndCallback(s); }" />
            <PanelCollection>
                <dx:PanelContent ID="PanelContent1" runat="server">
                    <asp:Panel runat="server" ID="pnlView" CssClass="pnlContainerGrid">
                        <asp:GridView ID="grdView" runat="server" CssClass="grdSelected" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                            <Columns>
                                <asp:BoundField DataField="LocationID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                <asp:TemplateField HeaderStyle-Width="200px" >
                                    <HeaderTemplate>
                                        <div style="padding-left:3px">
                                            <%=GetLabel("LocationCode")%>
                                        </div>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <div style='margin-left:<%#: Eval("Level") %>0px;'><%#: Eval("LocationCode")%></div>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="LocationName" HeaderText="Location Name" />
                                <asp:BoundField DataField="ShortName" HeaderText="Short Name" HeaderStyle-Width="200px" />
                                <asp:CheckBoxField DataField="IsAvailable" HeaderText="Available Balance" HeaderStyle-Width="120px" ItemStyle-HorizontalAlign="Center" />
                                <asp:CheckBoxField DataField="IsAllowOverIssued" HeaderText="Over Issued" HeaderStyle-Width="120px" ItemStyle-HorizontalAlign="Center" />
                                <asp:CheckBoxField DataField="IsNettable" HeaderText="Nettable" HeaderStyle-Width="120px" ItemStyle-HorizontalAlign="Center" />
                                <asp:CheckBoxField DataField="IsHoldForTransaction" HeaderText="Lock Down" HeaderStyle-Width="120px" ItemStyle-HorizontalAlign="Center" />
                                <asp:TemplateField ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="lnkItem" HeaderText="Item" HeaderStyle-Width="80px">
                                    <ItemTemplate>
                                        <a <%#: Eval("IsHeader").ToString() == "True" ? "style='display:none'" : ""%>>Item</a>
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