<%@ Page Language="C#" MasterPageFile="~/Libs/MasterPage/PatientPage/MPBasePatientPageTrx.master"
    AutoEventWireup="true" CodeBehind="OverLimitConfirmation.aspx.cs" Inherits="QIS.Medinfras.Web.CommonLibs.Program.OverLimitConfirmation" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Src="~/Libs/Program/Module/PatientManagement/PatientBillSummary/PatientBillSummaryToolbarCtl.ascx"
    TagName="ToolbarCtl" TagPrefix="uc1" %>
<asp:Content ID="Content5" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div class="menuTitle">
        <%=HttpUtility.HtmlEncode(GetPageTitle())%></div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="plhMPPatientPageNavigationPane"
    runat="server">
    <uc1:ToolbarCtl ID="ctlToolbar" runat="server" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="plhCustomButtonToolbar" runat="server">
    <li id="btnChangeTransactionStatus" crudmode="R" runat="server">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbprocess.png")%>' alt="" /><br style="clear: both" />
        <div>
            <%=GetLabel("Process")%></div>
    </li>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="plhHeader" runat="server">
    <input type="hidden" value="" id="hdnPageTitle" runat="server" />
    <input type="hidden" value="" id="hdnVisitID" runat="server" />
    <input type="hidden" value="" id="hdnModuleID" runat="server" />
    <input type="hidden" value="" id="hdnHealthcareServiceUnitID" runat="server" />
    <input type="hidden" value="" id="hdnLinkedRegistrationID" runat="server" />
    <input type="hidden" value="" id="hdnDepartmentID" runat="server" />
    <input type="hidden" value="" id="hdnRegistrationID" runat="server" />
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript">
        $(function () {
            onLoadGenerateBill();
        });

        $('#<%=btnChangeTransactionStatus.ClientID %>').live('click', function (evt) {
            if ($('.chkIsSelected input:checked').length < 1) {
                showToast('Warning', '<%=GetErrorMsgSelectTransactionFirst() %>');
            }
            else {
                var param = '';
                $('.chkIsSelected input:checked').each(function () {
                    var trxID = $(this).closest('tr').find('.hdnKeyField').val();
                    if (param != '')
                        param += ',';
                    param += trxID;
                });
                $('#<%=hdnParam.ClientID %>').val(param);
                onCustomButtonClick('confirm');
            }
        });

        function onAfterCustomClickSuccess(type) {
            cbpView.PerformCallback();
        }

        function onLoadGenerateBill() {
            $('.chkIsSelected input').change(function () {
                $('.chkSelectAll input').prop('checked', false);
            });

            $('.chkSelectAll input').change(function () {
                var isChecked = $(this).is(":checked");
                $('.chkIsSelected').each(function () {
                    $(this).find('input').prop('checked', isChecked);
                });
            });
        }

        function onRefreshControl() {
            cbpView.PerformCallback();
        }

        function onCbpViewEndCallback(s) {
            onLoadGenerateBill();
            hideLoadingPanel();
        }

        function onRefreshGrdReg() {
            cbpView.PerformCallback();
        }

        //#region Department
        function onCboDepartmentChanged() {
            $('#<%=hdnServiceUnitOrderID.ClientID %>').val('');
            $('#<%=txtServiceUnitOrderCode.ClientID %>').val('');
            $('#<%=txtServiceUnitOrderName.ClientID %>').val('');
            $('#<%=hdnCboDepartmentID.ClientID %>').val(cboDepartment.GetValue());
            onRefreshGrdReg();
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
                onRefreshGrdReg();
            });
        }
        //#endregion
    
    </script>
    <input type="hidden" value="" id="hdnTotalPatientAmount" runat="server" />
    <input type="hidden" value="" id="hdnTotalPayerAmount" runat="server" />
    <input type="hidden" value="" id="hdnTotalAmount" runat="server" />
    <input type="hidden" value="" id="hdnParam" runat="server" />
    <input type="hidden" value="" id="hdnTransactionHdID" runat="server" />
    <input type="hidden" value="" id="hdnCboDepartmentID" runat="server" />
    <input type="hidden" value="" id="hdnCboServiceUnitID" runat="server" />
    <input type="hidden" id="hdnServiceUnitOrderID" value="" runat="server" />
    <input type="hidden" value="" id="hdnFilterServiceUnitID" runat="server" />
    <div style="height: 435px; overflow-y: auto;">
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
                <td>
                    <asp:TextBox ID="txtServiceUnitOrderCode" Width="100%" runat="server" />
                </td>
                <td>
                    <asp:TextBox ID="txtServiceUnitOrderName" ReadOnly="true" Width="450px" runat="server" />
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
                                        <asp:ListView ID="lvwView" runat="server">
                                            <EmptyDataTemplate>
                                                <table id="tblView" runat="server" class="grdNormal notAllowSelect" cellspacing="0"
                                                    rules="all">
                                                    <tr>
                                                        <th style="width: 40px" rowspan="2">
                                                            <div style="padding: 3px">
                                                                <asp:CheckBox ID="chkSelectAll" runat="server" CssClass="chkSelectAll" />
                                                            </div>
                                                        </th>
                                                        <th rowspan="2" align="left" style="width: 150px">
                                                            <div style="padding: 3px; float: left">
                                                                <div style="font-size: medium">
                                                                    <%= GetLabel("Transaction")%></div>
                                                            </div>
                                                        </th>
                                                        <th rowspan="2" align="left">
                                                            <div style="padding: 3px; float: left">
                                                                <div style="font-size: medium">
                                                                    <%= GetLabel("Item")%></div>
                                                            </div>
                                                        </th>
                                                        <th colspan="3">
                                                            <b>
                                                                <%=GetLabel("Tariff Amount")%></b>
                                                        </th>
                                                        <th colspan="3">
                                                            <b>
                                                                <%=GetLabel("Limit Amount")%></b>
                                                        </th>
                                                    </tr>
                                                    <tr>
                                                        <th style="width: 100px">
                                                            <div style="text-align: right; padding-right: 3px">
                                                                <%=GetLabel("Service")%>
                                                            </div>
                                                        </th>
                                                        <th style="width: 100px">
                                                            <div style="text-align: right; padding-right: 3px">
                                                                <%=GetLabel("Drug & Supplies")%>
                                                            </div>
                                                        </th>
                                                        <th style="width: 100px">
                                                            <div style="text-align: right; padding-right: 3px">
                                                                <%=GetLabel("Logistic")%>
                                                            </div>
                                                        </th>
                                                        <th style="width: 100px">
                                                            <div style="text-align: right; padding-right: 3px">
                                                                <%=GetLabel("Service")%>
                                                            </div>
                                                        </th>
                                                        <th style="width: 100px">
                                                            <div style="text-align: right; padding-right: 3px">
                                                                <%=GetLabel("Drug & Supplies")%>
                                                            </div>
                                                        </th>
                                                        <th style="width: 100px">
                                                            <div style="text-align: right; padding-right: 3px">
                                                                <%=GetLabel("Logistic")%>
                                                            </div>
                                                        </th>
                                                    </tr>
                                                    <tr class="trEmpty">
                                                        <td colspan="10">
                                                            <%=GetLabel("No Data To Display") %>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </EmptyDataTemplate>
                                            <LayoutTemplate>
                                                <table id="tblView" runat="server" class="grdNormal notAllowSelect" cellspacing="0"
                                                    rules="all">
                                                    <tr>
                                                        <th style="width: 40px" rowspan="2">
                                                            <div style="padding: 3px">
                                                                <asp:CheckBox ID="chkSelectAll" runat="server" CssClass="chkSelectAll" />
                                                            </div>
                                                        </th>
                                                        <th rowspan="2" align="left" style="width: 150px">
                                                            <div style="padding: 3px; float: left">
                                                                <div style="font-size: medium">
                                                                    <%= GetLabel("Transaction")%></div>
                                                            </div>
                                                        </th>
                                                        <th rowspan="2" align="left">
                                                            <div style="padding: 3px; float: left">
                                                                <div style="font-size: medium">
                                                                    <%= GetLabel("Item")%></div>
                                                            </div>
                                                        </th>
                                                        <th colspan="3">
                                                            <b>
                                                                <%=GetLabel("Tariff Amount")%></b>
                                                        </th>
                                                        <th colspan="3">
                                                            <b>
                                                                <%=GetLabel("Limit Amount")%></b>
                                                        </th>
                                                    </tr>
                                                    <tr>
                                                        <th style="width: 100px">
                                                            <div style="text-align: right; padding-right: 3px">
                                                                <%=GetLabel("Service")%>
                                                            </div>
                                                        </th>
                                                        <th style="width: 100px">
                                                            <div style="text-align: right; padding-right: 3px">
                                                                <%=GetLabel("Drug & Supplies")%>
                                                            </div>
                                                        </th>
                                                        <th style="width: 100px">
                                                            <div style="text-align: right; padding-right: 3px">
                                                                <%=GetLabel("Logistic")%>
                                                            </div>
                                                        </th>
                                                        <th style="width: 100px">
                                                            <div style="text-align: right; padding-right: 3px">
                                                                <%=GetLabel("Service")%>
                                                            </div>
                                                        </th>
                                                        <th style="width: 100px">
                                                            <div style="text-align: right; padding-right: 3px">
                                                                <%=GetLabel("Drug & Supplies")%>
                                                            </div>
                                                        </th>
                                                        <th style="width: 100px">
                                                            <div style="text-align: right; padding-right: 3px">
                                                                <%=GetLabel("Logistic")%>
                                                            </div>
                                                        </th>
                                                    </tr>
                                                    <tr runat="server" id="itemPlaceholder">
                                                    </tr>
                                                </table>
                                            </LayoutTemplate>
                                            <ItemTemplate>
                                                <tr>
                                                    <td align="center">
                                                        <div <%# Eval("IsConfirmed").ToString() == "True" ? "style='display:none'" : "" %>>
                                                            <asp:CheckBox ID="chkIsSelected" CssClass="chkIsSelected" runat="server" />
                                                            <input type="hidden" class="hdnKeyField" value="<%#: Eval("ID")%>" />
                                                        </div>
                                                    </td>
                                                    <td>
                                                        <div style="padding: 3px; float: left">
                                                            <div>
                                                                <b>
                                                                    <%#: Eval("TransactionNo")%></b></div>
                                                            <div>
                                                                <%#: Eval("cfTransactionDateInString")%></div>
                                                            <div style="font-style: italic">
                                                                <%#: Eval("ServiceUnitName")%></div>
                                                        </div>
                                                    </td>
                                                    <td>
                                                        <div style="padding: 3px; float: left">
                                                            <div>
                                                                <b>
                                                                    <%#: Eval("ItemName1")%></b></div>
                                                            <div style="font-style: italic">
                                                                <%#: Eval("ItemType")%></div>
                                                            <div <%# Eval("IsConfirmed").ToString() == "True" ? "style='color: Blue'" : "style='display:none'" %>>
                                                                <%=GetLabel("Confirmed : ")%>
                                                                <%#: Eval("ConfirmedByName")%>
                                                                <%#: Eval("cfConfirmedDateTimeInString")%></div>
                                                        </div>
                                                    </td>
                                                    <td>
                                                        <div style="padding: 3px; text-align: right; color: Red">
                                                            <div>
                                                                <%#: Eval("TariffServiceUnit", "{0:N2}")%></div>
                                                        </div>
                                                    </td>
                                                    <td>
                                                        <div style="padding: 3px; text-align: right; color: Red">
                                                            <div>
                                                                <%#: Eval("TariffDrugSupplies", "{0:N2}")%></div>
                                                        </div>
                                                    </td>
                                                    <td>
                                                        <div style="padding: 3px; text-align: right; color: Red">
                                                            <div>
                                                                <%#: Eval("TariffLogistic", "{0:N2}")%></div>
                                                        </div>
                                                    </td>
                                                    <td>
                                                        <div style="padding: 3px; text-align: right;">
                                                            <div>
                                                                <%#: Eval("ServiceUnitPrice", "{0:N2}")%></div>
                                                        </div>
                                                    </td>
                                                    <td>
                                                        <div style="padding: 3px; text-align: right;">
                                                            <div>
                                                                <%#: Eval("DrugSuppliesUnitPrice", "{0:N2}")%></div>
                                                        </div>
                                                    </td>
                                                    <td>
                                                        <div style="padding: 3px; text-align: right;">
                                                            <div>
                                                                <%#: Eval("LogisticUnitPrice", "{0:N2}")%></div>
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
