<%@ Page Language="C#" MasterPageFile="~/Libs/MasterPage/PatientPage/MPBasePatientPageTrx.Master"
    AutoEventWireup="true" CodeBehind="InformationTransactionPatientDetail.aspx.cs"
    Inherits="QIS.Medinfras.Web.SystemSetup.Program.InformationTransactionPatientDetail" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<asp:Content ID="Content3" ContentPlaceHolderID="plhCustomButtonToolbar" runat="server">
    <li id="btnChangeTransactionStatusBack" runat="server" crudmode="R">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/back.png")%>' alt="" /><div>
            <%=GetLabel("Back")%></div>
    </li>
    <li id="btnRefresh" crudmode="R" runat="server">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbrefresh.png") %>' alt="" /><br style="clear: both" />
        <div>
            <%=GetLabel("Refresh") %></div>
    </li>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div class="menuTitle">
        <%=HttpUtility.HtmlEncode(GetPageTitle())%></div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="plhHeader" runat="server">
    <input type="hidden" value="" id="hdnVisitID" runat="server" />
    <input type="hidden" value="" id="hdnModuleID" runat="server" />
    <input type="hidden" value="" id="hdnHealthcareServiceUnitID" runat="server" />
    <input type="hidden" value="" id="hdnLinkedRegistrationID" runat="server" />
    <input type="hidden" value="" id="hdnDepartmentID" runat="server" />
    <input type="hidden" value="" id="hdnRegistrationID" runat="server" />
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript">
        $('#<%=btnChangeTransactionStatusBack.ClientID %>').live('click', function () {
            showLoadingPanel();
            var reqID = $('#<%=hdnRequestID.ClientID %>').val();
            var url = "~/Libs/Program/Information/InformationTransactionPatientList.aspx?id=" + reqID;
            document.location = ResolveUrl(url);
        });

        function onRefreshControl() {
            cbpView.PerformCallback();
        }

        function onRefreshGrdReg() {
            cbpView.PerformCallback();
        }

        function onCbpViewEndCallback(s) {
            hideLoadingPanel();
        }

        //#region Department
        function onCboDepartmentChanged() {
            $('#<%=hdnCboDepartmentID.ClientID %>').val(cboDepartment.GetValue());
            onRefreshGrdReg();
        }
        //#endregion

        $('#<%=btnRefresh.ClientID %>').click(function () {
            cbpView.PerformCallback();
        });

    </script>
    <input type="hidden" id="hdnPageTitle" runat="server" />
    <input type="hidden" value="" id="hdnRequestID" runat="server" />
    <input type="hidden" value="" id="hdnParam" runat="server" />
    <input type="hidden" value="" id="hdnTransactionHdID" runat="server" />
    <input type="hidden" value="" id="hdnCboDepartmentID" runat="server" />
    <input type="hidden" value="" id="hdnCboServiceUnitID" runat="server" />
    <input type="hidden" value="" id="hdnFilterServiceUnitID" runat="server" />
    <div style="height: 400px; overflow-y: auto;">
        <table class="tblContentArea" style="width: 100%">
            <colgroup>
                <col style="width: 80px" />
            </colgroup>
            <tr id="trDepartment" runat="server">
                <td class="tdLabel">
                    <label class="lblNormal">
                        <%=GetLabel("Department")%></label>
                </td>
                <td colspan="2">
                    <dxe:ASPxComboBox ID="cboDepartment" ClientInstanceName="cboDepartment" runat="server"
                        Width="350px">
                        <ClientSideEvents ValueChanged="function(s,e) { onCboDepartmentChanged(); }" />
                    </dxe:ASPxComboBox>
                </td>
            </tr>
        </table>
        <table class="tblContentArea" style="width: 100%">
            <tr>
                <td>
                    <div style="padding: 5px; min-height: 150px;">
                        <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
                            ShowLoadingPanel="false" OnCallback="cbpView_Callback">
                            <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e) { onCbpViewEndCallback(s); }" />
                            <PanelCollection>
                                <dx:PanelContent ID="PanelContent1" runat="server">
                                    <asp:Panel runat="server" ID="pnlService" Style="width: 100%; margin-left: auto;
                                        margin-right: auto; position: relative; font-size: 0.95em;">
                                        <asp:ListView ID="lvwView" runat="server">
                                            <EmptyDataTemplate>
                                                <table id="tblView" runat="server" class="grdNormal notAllowSelect" cellspacing="0"
                                                    rules="all">
                                                    <tr>
                                                        <th rowspan="2" align="left">
                                                            <div style="padding: 3px; float: left;">
                                                                <div>
                                                                    <%= GetLabel("Transaction No")%></div>
                                                                <div>
                                                                    <%= GetLabel("Transaction Date")%></div>
                                                            </div>
                                                        </th>
                                                        <th rowspan="2" align="left">
                                                            <div style="padding: 3px; float: left;">
                                                                <div>
                                                                    <%= GetLabel("Charges Service Unit Name")%></div>
                                                        </th>
                                                        <th rowspan="2" align="center">
                                                            <div style="padding: 3px; float: center;">
                                                                <div>
                                                                    <%= GetLabel("Transaction Status")%></div>
                                                        </th>
                                                        <th colspan="1" align="left">
                                                            <%=GetLabel("Created By Name")%>
                                                        </th>
                                                        <th colspan="1" align="left">
                                                            <%=GetLabel("Last Updated By Name")%>
                                                        </th>
                                                    </tr>
                                                    <tr>
                                                        <th style="width: 300px">
                                                            <div style="text-align: left; padding-right: 3px">
                                                                <%=GetLabel("Created By Date")%>
                                                            </div>
                                                        </th>
                                                        <th style="width: 300px">
                                                            <div style="text-align: left; padding-right: 3px">
                                                                <%=GetLabel("Last Updated By Date")%>
                                                            </div>
                                                        </th>
                                                    </tr>
                                                    <tr class="trEmpty">
                                                        <td colspan="7">
                                                            <%=GetLabel("No Data To Display") %>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </EmptyDataTemplate>
                                            <LayoutTemplate>
                                                <table id="tblView" runat="server" class="grdNormal notAllowSelect" cellspacing="0"
                                                    rules="all">
                                                    <tr>
                                                        <th rowspan="2" align="left">
                                                            <div style="padding: 3px; float: left;">
                                                                <div>
                                                                    <%= GetLabel("Transaction No")%></div>
                                                                <div>
                                                                    <%= GetLabel("Transaction Date")%></div>
                                                            </div>
                                                        </th>
                                                        <th rowspan="2" align="left">
                                                            <div style="padding: 3px; float: left;">
                                                                <div>
                                                                    <%= GetLabel("Charges Service Unit Name")%></div>
                                                            </div>
                                                        </th>
                                                        <th rowspan="2" align="center">
                                                            <div style="padding: 3px; float: center;">
                                                                <div>
                                                                    <%= GetLabel("Transaction Status")%></div>
                                                        </th>
                                                        <th colspan="1" align="left">
                                                            <%=GetLabel("Created By Name")%>
                                                        </th>
                                                        <th colspan="1" align="left">
                                                            <%=GetLabel("Last Updated By Name")%>
                                                        </th>
                                                    </tr>
                                                    <tr>
                                                        <th style="width: 300px">
                                                            <div style="text-align: left; padding-right: 3px">
                                                                <%=GetLabel("Created By Date Time")%>
                                                            </div>
                                                        </th>
                                                        <th style="width: 300px">
                                                            <div style="text-align: left; padding-right: 3px">
                                                                <%=GetLabel("Last Updated By Date Time")%>
                                                            </div>
                                                        </th>
                                                    </tr>
                                                    <tr runat="server" id="itemPlaceholder">
                                                    </tr>
                                                </table>
                                            </LayoutTemplate>
                                            <ItemTemplate>
                                                <tr>
                                                    <td>
                                                        <div style="padding: 3px; float: left;">
                                                            <div>
                                                                <%#: Eval("TransactionNo")%></div>
                                                            <div>
                                                                <%#: Eval("cfTransactionDateInString")%></div>
                                                        </div>
                                                    </td>
                                                    <td>
                                                        <div style="padding: 3px; float: left;">
                                                            <div>
                                                                <%#: Eval("ServiceUnitName")%></div>
                                                        </div>
                                                    </td>
                                                    <td>
                                                        <div style="padding: 3px; text-align: center;">
                                                            <div>
                                                                <%#: Eval("TransactionStatus")%></div>
                                                        </div>
                                                    </td>
                                                    <td>
                                                        <div style="padding: 3px; text-align: left;">
                                                            <div>
                                                                <%#: Eval("CreatedByName")%></div>
                                                            <div>
                                                                <%#: Eval("cfCreatedDateTimeInString")%></div>
                                                        </div>
                                                    </td>
                                                    <td>
                                                        <div style="padding: 3px; text-align: left;">
                                                            <div>
                                                                <%#: Eval("LastUpdatedByName")%></div>
                                                            <div>
                                                                <%#: Eval("cfLastUpdatedDateTimeInString")%></div>
                                                        </div>
                                                    </td>
                                                </tr>
                                            </ItemTemplate>
                                        </asp:ListView>
                                        <div class="imgLoadingGrdView" id="containerImgLoadingService">
                                            <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                                        </div>
                                    </asp:Panel>
                                </dx:PanelContent>
                            </PanelCollection>
                        </dxcp:ASPxCallbackPanel>
                    </div>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
