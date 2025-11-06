<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="InfoOutstandingPiutangPribadiCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Controls.InfoOutstandingPiutangPribadiCtl" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="QIS.Medinfras.Web.CustomControl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"
    Namespace="QIS.Medinfras.Web.CustomControl" TagPrefix="qis" %>

<script type="text/javascript" id="dxss_InfoOutstandingPiutangPribadiCtl">
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

    function onCboCustomerTypeChanged() {
        $('#<%=hdnCustomerType.ClientID %>').val(cboCustomerType.GetValue());
        cbpView.PerformCallback('refresh');
    }

    function onCboInfoRegistrationServiceUnitValueChanged(s) {
        onRefreshGridViewPopup();
    }

    function onRefreshGridViewPopup() {
        if (IsValid(null, 'fsPatientListPopup', 'mpPatientList'))
            cbpInfoRegistrationView.PerformCallback('refresh');
    }

    function onRefreshGrid() {
            $('#<%=hdnFilterExpressionQuickSearch.ClientID %>').val(txtSearchView.GenerateFilterExpression());
            cbpInfoRegistrationView.PerformCallback('refresh');
    }

    function onTxtSearchViewSearchClick(s) {
        setTimeout(function () {
            s.SetBlur();
            onRefreshGrid();
            setTimeout(function () {
                s.SetFocus();
            }, 0);
        }, 0);
    }

    //#region Paging
    var pageCountAvailable = parseInt('<%=PageCount %>');
    $(function () {
        setPagingDetailItem(pageCountAvailable);
    });

    function setPagingDetailItem(pageCount) {
        if (pageCount > 0)
            $('#<%=lvwView.ClientID %> tr:eq(1)').click();
        setPaging($("#pagingPopup"), pageCount, function (page) {
            cbpInfoRegistrationView.PerformCallback('changepage|' + page);
        }, 8);
    }

    function onCbpInfoRegistrationViewEndCallback(s) {
        hideLoadingPanel();

        var param = s.cpResult.split('|');
        if (param[0] == 'refresh') {
            var pageCount = parseInt(param[1]);
            if (pageCount > 0)
                $('#<%=lvwView.ClientID %> tr:eq(1)').click();

            setPaging($("#pagingPopup"), pageCount, function (page) {
                cbpInfoRegistrationView.PerformCallback('changepage|' + page);
            });
        }
        else
            $('#<%=lvwView.ClientID %> tr:eq(1)').click();
    }
    //#endregion

   
</script>
<input type="hidden" id="hdnParam" runat="server" />
<input type="hidden" value="" id="hdnID" runat="server" />
<input type="hidden" id="hdnFilterExpression" runat="server" value="" />
<input type="hidden" id="hdnCustomerType" runat="server" value="" />
<input type="hidden" value="" id="hdnFilterExpressionQuickSearch" runat="server" />
<div style="position: relative;">
    <div class="pageTitle">
        <%=GetLabel("Information Outstanding Piutang Pribadi")%></div>
        <table>
         <tr>
           <td class="tdLabel">
                <label><%=GetLabel("Quick Filter")%></label>
            </td>
            <td>
                <qis:QISIntellisenseTextBox runat="server" ClientInstanceName="txtSearchView" ID="txtSearchView"
                    Width="300px" Watermark="Search">
                    <ClientSideEvents SearchClick="function(s){ onTxtSearchViewSearchClick(s); }" />
                    <IntellisenseHints>
                        <qis:QISIntellisenseHint Text="No Registrasi" FieldName="RegistrationNo" />
                        <qis:QISIntellisenseHint Text="Tanggal Registrasi" FieldName="RegistrationDate" />
                        <qis:QISIntellisenseHint Text="No Bayar" FieldName="PaymentNo" />
                        <qis:QISIntellisenseHint Text="Tanggal Bayar" FieldName="PaymentDate" />
                        <qis:QISIntellisenseHint Text="Total Piutang" FieldName="TotalPaymentAmount" />
                        <qis:QISIntellisenseHint Text="Total Lunas" FieldName="TotalReceivingAmount" />
                        <qis:QISIntellisenseHint Text="Tanggal Outstanding" FieldName="" />
                    </IntellisenseHints>
                </qis:QISIntellisenseTextBox>
            </td>
           </tr>
        </table> 
    <dxcp:ASPxCallbackPanel ID="cbpInfoRegistrationView" runat="server" Width="100%"
        ClientInstanceName="cbpInfoRegistrationView" ShowLoadingPanel="false" OnCallback="cbpInfoRegistrationView_Callback">
        <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ onCbpInfoRegistrationViewEndCallback(s); }" />
        <PanelCollection>
            <dx:PanelContent ID="PanelContent1" runat="server">
                <asp:Panel runat="server" ID="pnlView" CssClass="pnlContainerGrid" Style="height: 320px;
                    font-size: 1em;">
                    <asp:ListView runat="server" ID="lvwView">
                        <EmptyDataTemplate>
                            <table id="tblView" runat="server" class="grdCollapsible lvwInfoRegistration" cellspacing="0"
                                rules="all">
                                <tr>
                                    <th style="width: 150px" align="left">
                                        <%=GetLabel("No Registrasi")%>
                                    </th>
                                    <th align="left">
                                        <%=GetLabel("Tanggal Registrasi")%>
                                    </th>
                                    <th style="width: 150px" align="center">
                                        <%=GetLabel("No Bayar")%>
                                    </th>
                                    <th style="width: 150px" align="center">
                                        <%=GetLabel("Total Piutang")%>
                                    </th>
                                    <th style="width: 150px" align="center">
                                        <%=GetLabel("Total Lunas")%>
                                    </th>
                                    <th style="width: 150px" align="center">
                                        <%=GetLabel("Total Outstanding")%>
                                    </th>
                                </tr>
                                <tr class="trEmpty">
                                    <td colspan="6">
                                        <%=GetLabel("No Data To Display")%>
                                    </td>
                                </tr>
                            </table>
                        </EmptyDataTemplate>
                        <LayoutTemplate>
                            <table id="tblView" runat="server" class="grdCollapsible lvwInfoRegistration" cellspacing="0"
                                rules="all">
                                <tr>
                                    <th style="width: 150px" align="left">
                                        <%=GetLabel("No Registrasi")%>
                                    </th>
                                    <th style="width: 100px" align="center">
                                        <%=GetLabel("Tanggal Registrasi")%>
                                    </th>
                                    <th style="width: 300px" align="center">
                                        <%=GetLabel("No bayar")%>
                                    </th>
                                    <th style="width: 100px" align="center">
                                        <%=GetLabel("Tanggal Bayar")%>
                                    </th>
                                    <th style="width: 150px" align="Right">
                                        <%=GetLabel("Total Piutang")%>
                                    </th>
                                    <th style="width: 150px" align="Right">
                                        <%=GetLabel("Total Lunas")%>
                                    </th>
                                    <th style="width: 150px" align="Right">
                                        <%=GetLabel("Total Outstanding")%>
                                    </th>
                                </tr>
                                <tr runat="server" id="itemPlaceholder">
                                </tr>
                            </table>
                        </LayoutTemplate>
                        <ItemTemplate>
                            <tr>
                                <td>
                                    <div>
                                        <%#: Eval("RegistrationNo") %></div>
                                </td>
                                <td style="width: 100px" align="Center">
                                    <div>
                                        <%#: Eval("cfRegistrationDate") %></div>
                                </td>
                                <td>
                                    <div>
                                        <%#: Eval("PaymentNo") %></div>
                                </td>
                             
                                <td style="width: 100px" align="Center">
                                    <div>
                                        <%#: Eval("cfPaymentDate") %></div>
                                </td>
                                <td style="width: 150px" align="Right">
                                    <div>
                                        <%#: Eval("TotalPaymentAmount", "{0:N}") %></div>
                                </td>
                                <td style="width: 150px" align="Right">
                                    <div>
                                        <%#: Eval("TotalReceivingAmount", "{0:N}") %></div>
                                </td>
                                <td style="width: 150px" align="Right">
                                    <div>
                                        <%#: Eval("cfTotalOutstanding", "{0:N}") %></div>
                                </td>
                            </tr>
                        </ItemTemplate>
                    </asp:ListView>
                </asp:Panel>
            </dx:PanelContent>
        </PanelCollection>
    </dxcp:ASPxCallbackPanel>
    <div class="imgLoadingGrdView" id="containerImgLoadingView">
        <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
    </div>
    <div class="containerPaging">
        <div class="wrapperPaging">
            <div id="pagingPopup">
            </div>
        </div>
    </div>
</div>
