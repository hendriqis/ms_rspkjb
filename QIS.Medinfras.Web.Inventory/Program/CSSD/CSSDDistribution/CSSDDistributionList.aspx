<%@ Page Title="" Language="C#" MasterPageFile="~/libs/MasterPage/MPTrx.master"
    AutoEventWireup="true" CodeBehind="CSSDDistributionList.aspx.cs" Inherits="QIS.Medinfras.Web.Inventory.Program.CSSDDistributionList" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<asp:Content ID="Content3" ContentPlaceHolderID="plhCustomButtonToolbar" runat="server">
    <li id="btnServiceRequestReturnApprove" crudmode="R" runat="server">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbapprove.png")%>' alt="" /><br style="clear: both" />
        <div>
            <%=GetLabel("Approve")%></div>
    </li>
    <li id="btnServiceRequestReturnVoid" crudmode="R" runat="server">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbvoid.png")%>' alt="" /><br style="clear: both" />
        <div>
            <%=GetLabel("Void")%></div>
    </li>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="plhMenuTitle" runat="server">   
    <div class="menuTitle"><%=HttpUtility.HtmlEncode(GetMenuCaption())%></div>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript" src='<%= ResolveUrl("~/Libs/Scripts/CustomGridViewList.js")%>'></script>
    <script type="text/javascript">
        $(function () {

            $('#<%=btnServiceRequestReturnApprove.ClientID %>').click(function () {
                if ($('.chkIsSelected input:checked').length < 1) {
                    showToast('Warning', 'Please Select Service First');
                }
                else {
                    var param = '';
                    $('.chkIsSelected input:checked').each(function () {
                        var itemDistributionHdID = $(this).closest('tr').find('.keyField').html();
                        if (param != '')
                            param += ',';
                        param += itemDistributionHdID;
                    });
                    $('#<%=hdnParam.ClientID %>').val(param);
                    onCustomButtonClick('approve');
                }
            });

            $('#<%=btnServiceRequestReturnVoid.ClientID %>').click(function () {
                if ($('.chkIsSelected input:checked').length < 1) {
                    showToast('Warning', 'Please Select Service First');
                }
                else {
                    var param = '';
                    $('.chkIsSelected input:checked').each(function () {
                        var itemDistributionHdID = $(this).closest('tr').find('.keyField').html();
                        if (param != '')
                            param += ',';
                        param += itemDistributionHdID;
                    });
                    $('#<%=hdnParam.ClientID %>').val(param);
                    onCustomButtonClick('void');
                }
            });

        });

        function onAfterCustomClickSuccess(type) {
            cbpView.PerformCallback('refresh');
        }

        function onRefreshControl(filterExpression) {
            $('#<%=hdnFilterExpression.ClientID %>').val(filterExpression);
            cbpView.PerformCallback('refresh');
        }

        function onCbpViewEndCallback(s) {
            hideLoadingPanel();
            $('#<%=grdView.ClientID %> tr:eq(1)').click();
        }

        $('.lnkDistribution a').live('click', function () {
            var param = $(this).closest('tr').find('.keyField').html();
            var url = ResolveUrl("~/Program/CSSD/CSSDDistribution/CSSDDistributionDetailListCtl.ascx");
            openUserControlPopup(url, param, 'Distribusi CSSD', 800, 550);
        });

        $('.lnkService a').live('click', function () {
            $row = $(this).closest('tr').parent();
            var entity = rowToObject($row);
            var param = entity.RequestID;
            var url = ResolveUrl("~/Program/CSSD/CSSDDistribution/CSSDServiceDetailListCtl.ascx");
            openUserControlPopup(url, param, 'Service CSSD', 1200, 550);
        });
    </script>
    <input type="hidden" value="" id="hdnParam" runat="server" />
    <input type="hidden" value="" id="hdnID" runat="server" />
    <input type="hidden" id="hdnFilterExpression" runat="server" value="" />
    <div style="position: relative; height: 450px; overflow-y: scroll; overflow-x: hidden">
        <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
            ShowLoadingPanel="false" OnCallback="cbpView_Callback">
            <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ onCbpViewEndCallback(s); }" />
            <PanelCollection>
                <dx:PanelContent ID="PanelContent1" runat="server">
                    <asp:Panel runat="server" ID="pnlView" CssClass="pnlContainerGrid">
                        <asp:GridView ID="grdView" runat="server" CssClass="grdSelected" AutoGenerateColumns="false"
                            ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                            <Columns>
                                <asp:BoundField DataField="DistributionID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                <asp:TemplateField HeaderStyle-Width="0px" ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <input type="hidden" value="<%#:Eval("RequestID") %>" bindingfield="RequestID" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderStyle-Width="40px" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkIsSelected" runat="server" CssClass="chkIsSelected" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:HyperLinkField HeaderText="Request No" DataTextField="RequestNo" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" ItemStyle-CssClass="lnkService" HeaderStyle-Width="150px" />
                                <asp:BoundField DataField="PackageName" HeaderText="CSSD Package" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                                <asp:HyperLinkField HeaderText="Distribution No" DataTextField="DistributionNo" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" ItemStyle-CssClass="lnkDistribution" HeaderStyle-Width="150px" />
                                <asp:BoundField DataField="cfDeliveryDateInString" HeaderText="Distribution Date"
                                    ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="120px" />
                                <asp:BoundField DataField="FromLocationName" HeaderText="Dari Lokasi"  HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                    HeaderStyle-Width="200px" />
                                <asp:BoundField DataField="ToLocationName" HeaderText="Ke Lokasi"  HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                    HeaderStyle-Width="200px" />
                            </Columns>
                            <EmptyDataTemplate>
                                <%=GetLabel("No Data To Display")%>
                            </EmptyDataTemplate>
                        </asp:GridView>
                    </asp:Panel>
                </dx:PanelContent>
            </PanelCollection>
        </dxcp:ASPxCallbackPanel>
        <div class="imgLoadingGrdView" id="containerImgLoadingView">
            <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
        </div>
    </div>
</asp:Content>
