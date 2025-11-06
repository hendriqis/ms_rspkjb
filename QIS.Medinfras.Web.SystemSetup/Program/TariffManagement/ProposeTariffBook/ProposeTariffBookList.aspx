<%@ Page Title="" Language="C#" MasterPageFile="~/Libs/MasterPage/MPList.master" AutoEventWireup="true" 
    CodeBehind="ProposeTariffBookList.aspx.cs" Inherits="QIS.Medinfras.Web.SystemSetup.Program.ProposeTariffBookList" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>

<asp:Content ID="Content2" ContentPlaceHolderID="plhCustomButtonToolbar" runat="server">
    <li id="btnProcessTariffBook" CRUDMode="R" runat="server"><img src='<%=ResolveUrl("~/Libs/Images/Icon/tbnew.png")%>' alt="" /><br style="clear:both"/><div><%=GetLabel("Process")%></div></li>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="plhList" runat="server">
    <script type="text/javascript" src='<%= ResolveUrl("~/Libs/Scripts/CustomGridViewList.js")%>'></script>
    <script type="text/javascript">
        $(function () {
            var grd = new customGridView();
            grd.init('<%=grdView.ClientID %>', '<%=hdnID.ClientID %>', '<%=pnlView.ClientID %>', cbpView, 'paging');

            $('#<%=btnProcessTariffBook.ClientID %>').click(function () {
                if ($('.chkIsProposed input:checked').length < 1) {
                    showToast('Warning', 'Please Select Tariff Book First');
                }
                else {
                    var param = '';
                    $('.chkIsProposed input:checked').each(function () {
                        var bookID = $(this).closest('tr').find('.keyField').html();
                        var clientInstanceName = 'cboApprovedBy' + bookID;
                        var cbo = ASPxClientControl.GetControlCollection().GetByName(clientInstanceName);
                        if (param != '')
                            param += '|';
                        param += bookID + ';' + cbo.GetValue();
                    });
                    $('#<%=hdnParam.ClientID %>').val(param);
                    onCustomButtonClick('processtariffbook');
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
        var lstCbo = [];
        var pageCount = parseInt('<%=PageCount %>');
        $(function () {
            setPaging($("#paging"), pageCount, function (page) {
                cbpView.PerformCallback('changepage|' + page);
            });

            registerControlHandler();
        });

        function registerControlHandler() {
            $('.chkIsProposed input').click(function () {
                var isChecked = $(this).is(":checked");
                var bookID = $(this).closest('tr').find('.keyField').html();
                var clientInstanceName = 'cboApprovedBy' + bookID;
                var cbo = ASPxClientControl.GetControlCollection().GetByName(clientInstanceName);
                if (isChecked) {
                    cbo.SetEnabled(true);
                    cbo.SetSelectedIndex(0);
                }
                else {
                    cbo.SetEnabled(false);
                    cbo.SetSelectedIndex(-1);
                }
            });
        }

        function onCbpViewEndCallback(s) {
            registerControlHandler();

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
                        <asp:GridView ID="grdView" runat="server" CssClass="grdSelected" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty"
                            OnRowDataBound="grdView_RowDataBound">
                            <Columns>
                                <asp:BoundField DataField="BookID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                <asp:TemplateField HeaderStyle-Width="40px" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkIsProposed" runat="server" CssClass="chkIsProposed" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="HealthcareName" HeaderText="Healthcare" HeaderStyle-Width="150px" />
                                <asp:BoundField DataField="TariffScheme" HeaderText="Tariff Scheme" HeaderStyle-Width="150px" />
                                <asp:BoundField DataField="DocumentNo" HeaderText="Document Number" />
                                <asp:BoundField DataField="ItemType" HeaderText="Item Type" HeaderStyle-Width="150px" />
                                <asp:BoundField DataField="RevisionNo" HeaderText="Revision No" ItemStyle-HorizontalAlign="Right" HeaderStyle-Width="90px" />
                                <asp:BoundField DataField="StartingDateInString" HeaderText="Effective Date" ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="120px" />
                                <asp:TemplateField HeaderStyle-Width="140px">
                                    <HeaderTemplate>
                                        <%=GetLabel("Number Of Items")%>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <div><%#: Eval("NumberOfItems")%> / <%#: Eval("NumberOfItemServices")%></div>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderStyle-Width="145px" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                    <HeaderTemplate>
                                        <%=GetLabel("Approved By")%>
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <dxe:ASPxComboBox ID="cboApprovedBy" ClientInstanceName='<%#: "cboApprovedBy" + DataBinder.Eval(Container.DataItem, "BookID")%>' ClientEnabled="false" runat="server" Width="90%" />
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