<%@ Page Title="" Language="C#" MasterPageFile="~/Libs/MasterPage/MPList.master" AutoEventWireup="true" 
    CodeBehind="TariffApprovalList.aspx.cs" Inherits="QIS.Medinfras.Web.Finance.Program.TariffApprovalList" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>

<asp:Content ID="Content2" ContentPlaceHolderID="plhCustomButtonToolbar" runat="server">
    <li id="btnTariffApprovalItem" CRUDMode="R" runat="server"><img src='<%=ResolveUrl("~/Libs/Images/Icon/tbnew.png")%>' alt="" /><br style="clear:both"/><div><%=GetLabel("Item")%></div></li>
    <li id="btnTariffApprovalApprove" CRUDMode="R" runat="server"><img src='<%=ResolveUrl("~/Libs/Images/Icon/tbapprove.png")%>' alt="" /><br style="clear:both"/><div><%=GetLabel("Approve")%></div></li>
    <li id="btnTariffApprovalDecline" CRUDMode="R" runat="server"><img src='<%=ResolveUrl("~/Libs/Images/Icon/tbvoid.png")%>' alt="" /><br style="clear:both"/><div><%=GetLabel("Decline")%></div></li>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="plhList" runat="server">
    <script type="text/javascript" src='<%= ResolveUrl("~/Libs/Scripts/CustomGridViewList.js")%>'></script>
    <script type="text/javascript">
        $(function () {
            var grd = new customGridView();
            grd.init('<%=grdView.ClientID %>', '<%=hdnID.ClientID %>', '<%=pnlView.ClientID %>', cbpView, 'paging');

            $('#<%=btnTariffApprovalItem.ClientID %>').click(function () {
                showLoadingPanel();
                var id = $('#<%=hdnID.ClientID %>').val();
                var url = ResolveUrl('~/Program/TariffManagement/TariffApproval/TariffApprovalItemList.aspx?id=' + id);
                document.location = url;
            });

            $('#<%=btnTariffApprovalApprove.ClientID %>').click(function () {
                if ($('.chkIsSelected input:checked').length < 1) {
                    showToast('Warning', 'Please Select Tariff Book First');
                }
                else {
                    var param = '';
                    $('.chkIsSelected input:checked').each(function () {
                        var bookID = $(this).closest('tr').find('.keyField').html();
                        if (param != '')
                            param += '|';
                        param += bookID;
                    });
                    $('#<%=hdnParam.ClientID %>').val(param);
                    onCustomButtonClick('approve');
                }
            });

            $('#<%=btnTariffApprovalDecline.ClientID %>').click(function () {
                if ($('.chkIsSelected input:checked').length < 1) {
                    showToast('Warning', 'Please Select Tariff Book First');
                }
                else {
                    var param = '';
                    $('.chkIsSelected input:checked').each(function () {
                        var bookID = $(this).closest('tr').find('.keyField').html();
                        if (param != '')
                            param += ',';
                        param += bookID;
                    });
                    $('#<%=hdnParam.ClientID %>').val(param);
                    onCustomButtonClick('decline');
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

        //#region Paging
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

        $('.lnkNotes').live('click', function () {
            var id = $(this).closest('tr').find('.keyField').html();
            var url = ResolveUrl("~/Program/TariffManagement/TariffApproval/TariffApprovalNotesCtl.ascx");
            openUserControlPopup(url, id, 'Notes', 500, 400);
        });
    </script>
    <input type="hidden" value="" id="hdnParam" runat="server" />
    <input type="hidden" value="" id="hdnID" runat="server" />
    <input type="hidden" id="hdnFilterExpression" runat="server" value="" />
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
                                <asp:BoundField DataField="BookID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                <asp:TemplateField HeaderStyle-Width="40px" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkIsSelected" runat="server" CssClass="chkIsSelected" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="DocumentNo" HeaderText="No. Buku Tarif"  HeaderStyle-Width="150px" HeaderStyle-HorizontalAlign="Left" />
                                <asp:BoundField DataField="RevisionNo" HeaderText="Nomor Revisi"  HeaderStyle-Width="80px" HeaderStyle-HorizontalAlign="Left" />
                                <asp:BoundField DataField="TariffScheme" HeaderText="Skema Tarif" HeaderStyle-Width="150px" HeaderStyle-HorizontalAlign="Left"/>
                                <%--<asp:BoundField DataField="ItemType" HeaderText="Tarif Untuk" HeaderStyle-HorizontalAlign="Left" />--%>
                                <asp:BoundField DataField="StartingDateInString" HeaderText="Tanggal Berlaku" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="120px" />
                                <asp:TemplateField HeaderStyle-Width="140px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                    <HeaderTemplate>
                                        <%=GetLabel("Jumlah Item")%>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <div><%#: Eval("NumberOfApprovedItems")%> / <%#: Eval("NumberOfItems")%></div>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderStyle-Width="140px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center">
                                    <HeaderTemplate>
                                        <%=GetLabel("Catatan")%>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <a class="lnkNotes" href="javascript:void(0);"><%=GetLabel("Add Notes")%></a>    
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <EmptyDataTemplate>
                                <%=GetLabel("Data Tidak Tersedia")%>
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