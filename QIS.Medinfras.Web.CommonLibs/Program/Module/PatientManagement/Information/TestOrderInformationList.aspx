<%@ Page Title="" Language="C#" MasterPageFile="~/Libs/MasterPage/PatientPage/MPBasePatientPageTrx.master"
    AutoEventWireup="true" CodeBehind="TestOrderInformationList.aspx.cs" Inherits="QIS.Medinfras.Web.CommonLibs.Program.TestOrderInformationList" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Src="~/Libs/Program/Module/PatientManagement/TransactionPage/TransactionPageToolbarCtl.ascx"
    TagName="ToolbarCtl" TagPrefix="uc1" %>
<asp:Content ID="Content5" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div class="menuTitle">
        <%=HttpUtility.HtmlEncode(GetPageTitle())%></div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="plhCustomButtonToolbar" runat="server">
    <li id="btnRefresh" crudmode="R" runat="server">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbrefresh.png")%>' alt="" /><br style="clear: both" />
        <div>
            <%=GetLabel("Refresh")%></div>
    </li>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="plhMPPatientPageNavigationPane"
    runat="server">
    <uc1:ToolbarCtl ID="ctlToolbar" runat="server" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="plhHeader" runat="server">
    <input type="hidden" value="" id="hdnVisitID" runat="server" />
    <input type="hidden" value="" id="hdnHealthcareServiceUnitID" runat="server" />
    <input type="hidden" value="" id="hdnDepartmentID" runat="server" />
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript">
        $(function () {
            $('#<%=grdView.ClientID %> tr:gt(0):not(.trEmpty)').live('click', function () {
                $('#<%=grdView.ClientID %> tr.selected').removeClass('selected');
                $(this).addClass('selected');
                $('#<%=hdnID.ClientID %>').val($(this).find('.keyField').html());
            });
            $('#<%=grdView.ClientID %> tr:eq(1)').click();
        });

        //#region Department
        function onCboDepartmentChanged() {
            $('#<%=hdnServiceUnitOrderID.ClientID %>').val('');
            $('#<%=txtServiceUnitOrderCode.ClientID %>').val('');
            $('#<%=txtServiceUnitOrderName.ClientID %>').val('');
            $('#<%=hdnCboDepartmentID.ClientID %>').val(cboDepartment.GetValue());
            cbpView.PerformCallback('refresh');
        }
        //#endregion

        //#region Service Unit
        function getHealthcareServiceUnitOrderFilterExpression() {
            var filterExpression = "HealthcareID = '" + AppSession.healthcareID + "'"
                                    + " AND DepartmentID = '" + cboDepartment.GetValue() + "'"
                                    + " AND " + $('#<%=hdnFilterServiceUnitID.ClientID %>').val();
            return filterExpression;
        }

        $('#lblServiceUnitOrder.lblLink').live('click', function () {
            openSearchDialog('serviceunitperhealthcare', getHealthcareServiceUnitOrderFilterExpression(), function (value) {
                $('#<%=txtServiceUnitOrderCode.ClientID %>').val(value);
                onTxtServiceUnitOrderCodeChanged(value);
            });
        });

        $('#<%=txtServiceUnitOrderCode.ClientID %>').live('change', function () {
            onTxtServiceUnitOrderCodeChanged($(this).val());
        });

        function onTxtServiceUnitOrderCodeChanged(value) {
            var filterExpression = getHealthcareServiceUnitOrderFilterExpression() + " AND ServiceUnitCode = '" + value + "'";
            Methods.getObject('GetvHealthcareServiceUnitList', filterExpression, function (result) {
                if (result != null) {
                    $('#<%=hdnServiceUnitOrderID.ClientID %>').val(result.HealthcareServiceUnitID);
                    $('#<%=txtServiceUnitOrderCode.ClientID %>').val(result.ServiceUnitCode);
                    $('#<%=txtServiceUnitOrderName.ClientID %>').val(result.ServiceUnitName);
                }
                else {
                    $('#<%=hdnServiceUnitOrderID.ClientID %>').val('');
                    $('#<%=txtServiceUnitOrderCode.ClientID %>').val('');
                    $('#<%=txtServiceUnitOrderName.ClientID %>').val('');
                }
                cbpView.PerformCallback('refresh');
            });
        }
        //#endregion

        $('#<%=btnRefresh.ClientID %>').live('click', function () {
            cbpView.PerformCallback('refresh');
        });

        function onAfterCustomClickSuccess(type) {
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

        function onBeforeRightPanelPrint(code, filterExpression, errMessage) {
            var testOrderID = $('#<%=hdnID.ClientID %>').val();
            if (code == 'PM-00548') {
                filterExpression.text = 'TestOrderID = ' + testOrderID;
            }
            else {
                filterExpression.text = testOrderID;
            }

            return true;
        }

    </script>
    <input type="hidden" value="" id="hdnPageTitle" runat="server" />
    <input type="hidden" value="" id="hdnID" runat="server" />
    <input type="hidden" value="" id="hdnCboDepartmentID" runat="server" />
    <input type="hidden" value="" id="hdnCboServiceUnitID" runat="server" />
    <input type="hidden" value="" id="hdnFilterServiceUnitID" runat="server" />
    <div>
        <table class="tblContentArea" style="width: 100%">
            <colgroup>
                <col style="width: 100px" />
                <col style="width: 100px" />
                <col style="width: 150px" />
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
            <tr>
                <td class="tdLabel">
                    <label class="lblLink lblNormal" id="lblServiceUnitOrder">
                        <%=GetLabel("Unit Pelayanan")%></label>
                </td>
                <input type="hidden" id="hdnServiceUnitOrderID" value="" runat="server" />
                <td>
                    <asp:TextBox ID="txtServiceUnitOrderCode" Width="100%" runat="server" />
                </td>
                <td>
                    <asp:TextBox ID="txtServiceUnitOrderName" ReadOnly="true" Width="250%" runat="server" />
                </td>
                <td>
                    &nbsp;
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
                                        <asp:GridView ID="grdView" runat="server" CssClass="grdSelected grdPatientPage" AutoGenerateColumns="false"
                                            ShowHeaderWhenEmpty="false" EmptyDataRowStyle-CssClass="trEmpty" ShowHeader="true">
                                            <Columns>   
                                                <asp:BoundField DataField="OrderID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                                <asp:BoundField DataField="cfScheduledDateInString" HeaderText="Tanggal" HeaderStyle-Width="60px"
                                                    HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                                <asp:BoundField DataField="ScheduledTime" HeaderText="Jam" HeaderStyle-Width="50px"
                                                    HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                                <asp:BoundField DataField="OrderNo" HeaderText="Nomor Order" HeaderStyle-Width="100px"
                                                    HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left" />
                                                <asp:BoundField DataField="ServiceUnitName" HeaderText="Unit Pelayanan" HeaderStyle-Width="100px"
                                                    HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left" />
                                                <asp:BoundField DataField="ParamedicName" HeaderText="Dokter Order" HeaderStyle-Width="200px"
                                                    HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left" />
                                                <asp:BoundField DataField="CreatedByName" HeaderText="Dibuat Oleh" HeaderStyle-Width="200px"
                                                    HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left" />
                                                <asp:BoundField DataField="Remarks" HeaderText="Catatan" HeaderStyle-Width="200px"
                                                    HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left" />
                                                <asp:BoundField DataField="TransactionStatusWatermark" HeaderText="Status" HeaderStyle-Width="100px"
                                                    HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                            </Columns>
                                            <EmptyDataTemplate>
                                                <%=GetLabel("Tidak ada order penunjang medis untuk pasien ini") %>
                                            </EmptyDataTemplate>
                                        </asp:GridView>
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
