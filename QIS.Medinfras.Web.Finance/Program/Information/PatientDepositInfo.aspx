<%@ Page Language="C#" MasterPageFile="~/Libs/MasterPage/MPTrx2.master" AutoEventWireup="true"
    CodeBehind="PatientDepositInfo.aspx.cs" Inherits="QIS.Medinfras.Web.Finance.Program.PatientDepositInfo" %>

<%@ Register Assembly="DevExpress.Web.ASPxPivotGrid.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPivotGrid" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.ASPxPivotGrid.v11.1.Export, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPivotGrid.Export" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="QIS.Medinfras.Web.CustomControl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"
    Namespace="QIS.Medinfras.Web.CustomControl" TagPrefix="qis" %>
<asp:Content ID="Content3" ContentPlaceHolderID="plhCustomButtonToolbar" runat="server">
    <li id="btnRefresh" crudmode="R" runat="server">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbrefresh.png")%>' alt="" /><br style="clear: both" />
        <div>
            <%=GetLabel("Refresh")%></div>
    </li>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div class="menuTitle">
        <%=HttpUtility.HtmlEncode(GetPageTitle())%></div>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript" src='<%= ResolveUrl("~/Libs/Scripts/CustomGridViewList.js")%>'></script>
    <script type="text/javascript">
        $(function () {
            $('#<%=btnRefresh.ClientID %>').click(function () {
                if (IsValid(null, 'fsMPEntry', 'mpEntry')) {
                    cbpView.PerformCallback('refresh');
                }
            });

            setDatePicker('<%=txtDateFrom.ClientID %>');
            setDatePicker('<%=txtDateTo.ClientID %>');

            $('#<%=txtDateFrom.ClientID %>').change(function () {
                cbpView.PerformCallback('refresh');
            });
            $('#<%=txtDateTo.ClientID %>').change(function () {
                cbpView.PerformCallback('refresh');
            });
        });

        function onRefreshControl(filterExpression) {
            cbpView.PerformCallback('refresh');
        }

        function onCbpViewEndCallback(s) {
            hideLoadingPanel();
        }

        $('.lblDetail').live('click', function () {
            $tr = $(this).closest('tr');
            var mrn = $tr.find('.keyField').html().trim();
            var dateFrom = $('#<%=txtDateFrom.ClientID %>').val();
            var dateTo = $('#<%=txtDateTo.ClientID %>').val();

            var filterParam = dateFrom + ";" + dateTo + "|" + mrn;

            var url = ResolveUrl("~/Program/Information/PatientDepositInfoDtCtl.ascx");
            openUserControlPopup(url, filterParam, 'Patient Deposit - Detail', 1200, 500);
        });
    </script>
    <input type="hidden" value="" id="hdnPageTitle" runat="server" />
    <div>
        <table class="tblEntryContent" style="width: 650px;">
            <colgroup>
                <col style="width: 120px" />
                <col style="width: 600px" />
            </colgroup>
            <tr>
                <td>
                    <label>
                        <%=GetLabel("Periode") %></label>
                </td>
                <td>
                    <table cellpadding="0" cellspacing="0">
                        <tr>
                            <td>
                                <asp:TextBox runat="server" CssClass="datepicker" ID="txtDateFrom" Width="120px" />
                            </td>
                            <td>
                                &nbsp; s/d &nbsp;
                            </td>
                            <td>
                                <asp:TextBox runat="server" CssClass="datepicker" ID="txtDateTo" Width="120px" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td class="tdLabel">
                    <label>
                        <%=GetLabel("Nama Pasien")%></label>
                </td>
                <td>
                    <asp:TextBox runat="server" ID="txtPatientName" Width="320px" />
                </td>
            </tr>
        </table>
    </div>
    <div style="position: relative; max-height: 450px; overflow-y: auto">
        <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
            ShowLoadingPanel="false" OnCallback="cbpView_Callback">
            <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ onCbpViewEndCallback(s); }" />
            <PanelCollection>
                <dx:PanelContent ID="PanelContent1" runat="server">
                    <asp:Panel runat="server" ID="pnlView" CssClass="pnlContainerGrid">
                        <asp:ListView runat="server" ID="lvwView">
                            <EmptyDataTemplate>
                                <table id="tblView" runat="server" class="grdView lvwView" cellspacing="0" rules="all">
                                    <tr>
                                        <th class="keyField">
                                            &nbsp;
                                        </th>
                                        <th style="width: 150px" align="left">
                                            <%=GetLabel("No. RM")%>
                                        </th>
                                        <th align="left">
                                            <%=GetLabel("Nama Pasien")%>
                                        </th>
                                        <th style="width: 150px" align="right">
                                            <%=GetLabel("Balance BEGIN")%>
                                        </th>
                                        <th style="width: 150px" align="right">
                                            <%=GetLabel("Balance IN")%>
                                        </th>
                                        <th style="width: 150px" align="right">
                                            <%=GetLabel("Balance OUT")%>
                                        </th>
                                        <th style="width: 150px" align="right">
                                            <%=GetLabel("Balance END")%>
                                        </th>
                                    </tr>
                                    <tr class="trEmpty">
                                        <td colspan="8">
                                            <%=GetLabel("No Data To Display")%>
                                        </td>
                                    </tr>
                                </table>
                            </EmptyDataTemplate>
                            <LayoutTemplate>
                                <table id="tblViewOrder" runat="server" class="grdView lvwView" cellspacing="0" rules="all">
                                    <tr>
                                        <th class="keyField">
                                            &nbsp;
                                        </th>
                                        <th style="width: 150px" align="left">
                                            <%=GetLabel("No. RM")%>
                                        </th>
                                        <th align="left">
                                            <%=GetLabel("Nama Pasien")%>
                                        </th>
                                        <th style="width: 150px" align="right">
                                            <%=GetLabel("Balance BEGIN")%>
                                        </th>
                                        <th style="width: 150px" align="right">
                                            <%=GetLabel("Balance IN")%>
                                        </th>
                                        <th style="width: 150px" align="right">
                                            <%=GetLabel("Balance OUT")%>
                                        </th>
                                        <th style="width: 150px" align="right">
                                            <%=GetLabel("Balance END")%>
                                        </th>
                                    </tr>
                                    <tr runat="server" id="itemPlaceholder">
                                    </tr>
                                </table>
                            </LayoutTemplate>
                            <ItemTemplate>
                                <tr>
                                    <td class="keyField">
                                        <%#: Eval("MRN")%>
                                    </td>
                                    <td>
                                        <div>
                                            <%#: Eval("MedicalNo") %>
                                        </div>
                                    </td>
                                    <td>
                                        <label class="lblLink lblDetail">
                                            <%#: Eval("PatientName")%>
                                        </label>
                                    </td>
                                    <td align="right">
                                        <div>
                                            <%#: Eval("cfBalanceBEGIN") %>
                                        </div>
                                    </td>
                                    <td align="right">
                                        <div>
                                            <%#: Eval("cfBalanceIN") %>
                                        </div>
                                    </td>
                                    <td align="right">
                                        <div>
                                            <%#: Eval("cfBalanceOUT") %>
                                        </div>
                                    </td>
                                    <td align="right">
                                        <div>
                                            <%#: Eval("cfBalanceEND") %>
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
    </div>
</asp:Content>
