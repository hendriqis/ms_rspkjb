<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="InfoHistoryRegistrationCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Controls.InfoHistoryRegistrationCtl" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<script type="text/javascript" id="dxss_patientvisitctl">
    function onCboInfoRegistrationServiceUnitValueChanged(s) {
        onRefreshGridViewPopup();
    }

    setDatePicker('<%=txtDateFrom.ClientID %>');
    setDatePicker('<%=txtDateTo.ClientID %>');

    $('#<%=txtDateFrom.ClientID %>').change(function (evt) {
        onRefreshGridViewPopup();
    });

    $('#<%=txtDateTo.ClientID %>').change(function (evt) {
        onRefreshGridViewPopup();
    });

    function onCboServiceUnitChanged() {
        onRefreshGridViewPopup();
    }

    function onRefreshGridViewPopup() {
        if (IsValid(null, 'fsPatientListPopup', 'mpPatientList'))
            cbpInfoRegistrationView.PerformCallback('refresh');
    }

    //#region Paging
    var pageCount = parseInt('<%=PageCount %>');
    $(function () {
        setPaging($("#pagingInfoRegistrationView"), pageCount, function (page) {
            cbpInfoRegistrationView.PerformCallback('changepage|' + page);
        });
    });

    function onCbpInfoRegistrationViewEndCallback(s) {
        hideLoadingPanel();

        var param = s.cpResult.split('|');
        if (param[0] == 'refresh') {
            var pageCount = parseInt(param[1]);

            setPaging($("#pagingInfoRegistrationView"), pageCount, function (page) {
                cbpInfoRegistrationView.PerformCallback('changepage|' + page);
            });
        }
    }
    //#endregion
</script>
<input type="hidden" id="hdnMRN" runat="server" />
<div class="pageTitle">
    <%=GetLabel("Registration Information")%></div>
<fieldset id="fsPatientListPopup">
    <table>
        <colgroup>
            <col style="width: 150px" />
            <col style="width: 150px" />
        </colgroup>
        <tr>
            <td class="tdLabel">
                <label class="lblNormal">
                    <%=GetLabel("Tanggal Registrasi")%></label>
            </td>
            <td>
                <asp:TextBox runat="server" CssClass="datepicker" ID="txtDateFrom" Width="120px" />
            </td>
            <td>
            </td>
            <td>
                <asp:TextBox runat="server" CssClass="datepicker" ID="txtDateTo" Width="120px" />
            </td>
        </tr>
    </table>
    <table>
        <colgroup>
            <col style="width: 150px" />
            <col style="width: 150px" />
        </colgroup>
        <tr id="trBusinessPartner" runat="server">
            <td class="tdLabel">
                <label class="lblNormal">
                    <%=GetLabel("Penjamin Bayar")%></label>
            </td>
            <td>
                <dxe:ASPxComboBox ID="cboBusinessPartner" ClientInstanceName="cboBusinessPartner"
                    runat="server" Width="400px">
                    <ClientSideEvents ValueChanged="function(s,e) { onCboServiceUnitChanged(); }" />
                </dxe:ASPxComboBox>
            </td>
        </tr>
    </table>
</fieldset>
<div style="position: relative;">
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
                                    <th style="width: 220px">
                                        <%=GetLabel("No. Registrasi")%>
                                    </th>
                                    <th style="width: 220px">
                                        <%=GetLabel("Tanggal Registrasi")%>
                                    </th>
                                    <th style="width: 220px">
                                        <%=GetLabel("Unit Pelayanan")%>
                                    </th>
                                    <th style="width: 220px">
                                        <%=GetLabel("Dokter")%>
                                    </th>
                                    <th style="width: 220px">
                                        <%=GetLabel("Penjamin Bayar")%>
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
                                    <th style="width: 220px">
                                        <%=GetLabel("No. Registrasi")%>
                                    </th>
                                    <th style="width: 220px">
                                        <%=GetLabel("Tanggal Registrasi")%>
                                    </th>
                                    <th style="width: 220px">
                                        <%=GetLabel("Unit Pelayanan")%>
                                    </th>
                                    <th style="width: 220px">
                                        <%=GetLabel("Dokter")%>
                                    </th>
                                    <th style="width: 220px">
                                        <%=GetLabel("Penjamin Bayar")%>
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
                                        <%#: Eval("RegistrationNo") %>
                                        <input type="hidden" class="hdnRegistrationID" value='<%#: Eval("RegistrationID") %>' />
                                    </div>
                                </td>
                                <td>
                                    <div>
                                        <%#: Eval("cfRegistrationDateInString") %>
                                    </div>
                                </td>
                                <td>
                                    <div>
                                        <%#: Eval("ServiceUnitName") %>
                                    </div>
                                </td>
                                <td>
                                    <div>
                                        <%#: Eval("ParamedicName") %>
                                    </div>
                                </td>
                                <td>
                                    <div>
                                        <%#: Eval("BusinessPartnerName") %>
                                    </div>
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
            <div id="pagingInfoRegistrationView">
            </div>
        </div>
    </div>
</div>
