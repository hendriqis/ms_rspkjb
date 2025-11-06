<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="InfoContractCustomerCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Controls.InfoContractCustomerCtl" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="QIS.Medinfras.Web.CustomControl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"
    Namespace="QIS.Medinfras.Web.CustomControl" TagPrefix="qis" %>

<script type="text/javascript" id="dxss_InfoContractCustomerCtl">
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
<input type="hidden" id="hdnHealthcareServiceUnitID" runat="server" />
<input type="hidden" value="" id="hdnID" runat="server" />
<input type="hidden" id="hdnFilterExpression" runat="server" value="" />
<input type="hidden" id="hdnCustomerType" runat="server" value="" />
<input type="hidden" value="" id="hdnFilterExpressionQuickSearch" runat="server" />
<div style="position: relative;">
    <div class="pageTitle">
        <%=GetLabel("Information Status Rekanan")%></div>
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
                        <qis:QISIntellisenseHint Text="Kode Instansi" FieldName="BusinessPartnerCode" />
                        <qis:QISIntellisenseHint Text="Nama Instansi" FieldName="BusinessPartnerName" />
                        <qis:QISIntellisenseHint Text="Status" FieldName="IsActive" />
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
                                        <%=GetLabel("Kode Rekanan")%>
                                    </th>
                                    <th align="left">
                                        <%=GetLabel("Nama Rekanan")%>
                                    </th>
                                    <th style="width: 150px" align="center">
                                        <%=GetLabel("Status")%>
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
                                        <%=GetLabel("Kode Rekanan")%>
                                    </th>
                                    <th align="left">
                                        <%=GetLabel("Nama Rekanan")%>
                                    </th>
                                    <th style="width: 150px" align="center">
                                        <%=GetLabel("Status")%>
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
                                        <%#: Eval("BusinessPartnerCode") %></div>
                                </td>
                                <td>
                                    <div>
                                        <%#: Eval("BusinessPartnerName") %></div>
                                </td>
                                <td align="center">
                                    <img class="imgStatus" title='<%#: Eval("cfIsActiveInString") %>' style="width: 20px;
                                        height: 20px" src='<%# Eval("IsActive").ToString() == "False" ? ResolveUrl("~/Libs/Images/Status/cancel.png") : ResolveUrl("~/Libs/Images/Status/done.png")%>'
                                        alt="" />
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
