<%@ Page Language="C#" MasterPageFile="~/Libs/MasterPage/PatientPage/MPBasePatientPageTrx.Master"
    AutoEventWireup="true" CodeBehind="PatientBillDetailReprintList2Detail.aspx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.PatientBillDetailReprintList2Detail" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<asp:Content ID="Content3" ContentPlaceHolderID="plhCustomButtonToolbar" runat="server">
    <li id="btnTransactionReprint" runat="server" crudmode="R">
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
        $('#<%=btnTransactionReprint.ClientID %>').live('click', function () {
            showLoadingPanel();
            var reqID = $('#<%=hdnRequestID.ClientID %>').val();
            var url = "~/Libs/Program/Module/PatientManagement/PatientBillDetail/PatientBillDetailReprintList2.aspx?id=" + reqID;
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
            $('#<%=hdnServiceUnitID.ClientID %>').val('');
            $('#<%=txtServiceUnitCode.ClientID %>').val('');
            $('#<%=txtServiceUnitName.ClientID %>').val('');
            $('#<%=hdnCboDepartmentID.ClientID %>').val(cboDepartment.GetValue());
            onRefreshGrdReg();
        }
        //#endregion

        //#region Service Unit
        function getHealthcareServiceUnitFilterExpression() {
            var filterExpression = "HealthcareID = '" + AppSession.healthcareID + "'"
                                    + " AND DepartmentID = '" + cboDepartment.GetValue() + "'"
                                    + " AND " + $('#<%=hdnFilterServiceUnitID.ClientID %>').val();
            return filterExpression;
        }

        $('#lblServiceUnit.lblLink').live('click', function () {
            openSearchDialog('serviceunitperhealthcare', getHealthcareServiceUnitFilterExpression(), function (value) {
                $('#<%=txtServiceUnitCode.ClientID %>').val(value);
                onTxtServiceUnitCodeChanged(value);
            });
        });

        $('#<%=txtServiceUnitCode.ClientID %>').live('change', function () {
            onTxtServiceUnitCodeChanged($(this).val());
        });

        function onTxtServiceUnitCodeChanged(value) {
            var filterExpression = getHealthcareServiceUnitFilterExpression() + " AND ServiceUnitCode = '" + value + "'";
            Methods.getObject('GetvHealthcareServiceUnitList', filterExpression, function (result) {
                if (result != null) {
                    $('#<%=hdnServiceUnitID.ClientID %>').val(result.HealthcareServiceUnitID);
                    $('#<%=txtServiceUnitCode.ClientID %>').val(result.ServiceUnitCode);
                    $('#<%=txtServiceUnitName.ClientID %>').val(result.ServiceUnitName);
                }
                else {
                    $('#<%=hdnServiceUnitID.ClientID %>').val('');
                    $('#<%=txtServiceUnitCode.ClientID %>').val('');
                    $('#<%=txtServiceUnitName.ClientID %>').val('');
                }
                onRefreshGrdReg();
            });
        }
        //#endregion

        $('.lnkTransactionNo').live('click', function () {
            var $tr = $(this).closest('tr');
            var id = $tr.find('.hdnKeyField').val();
            var transactionCode = $tr.find('.hdnTransactionCode').val();
            var prescriptionOrderID = parseInt($tr.find('.hdnPrescriptionOrderID').val());
            var prescriptionReturnOrderID = parseInt($tr.find('.hdnPrescriptionReturnOrderID').val());
            var url = '';

            if (prescriptionOrderID > 0) {
                id = prescriptionOrderID;
                url = ResolveUrl("~/Libs/Program/Module/PatientManagement/PatientBillSummary/GenerateBill/PatientBillSummaryGenerateBillDtPrescriptionCtl.ascx");
            }
            else if (prescriptionReturnOrderID > 0) {
                id = prescriptionReturnOrderID;
                url = ResolveUrl("~/Libs/Program/Module/PatientManagement/PatientBillSummary/GenerateBill/PatientBillSummaryGenerateBillDtPrescriptionReturnCtl.ascx");
            }
            else if (transactionCode == '<%=laboratoryTransactionCode %>')
                url = ResolveUrl("~/Libs/Program/Module/PatientManagement/PatientBillSummary/GenerateBill/PatientBillSummaryGenerateBillDtLaboratoryCtl.ascx");
            else
                url = ResolveUrl("~/Libs/Program/Module/PatientManagement/PatientBillSummary/GenerateBill/PatientBillSummaryGenerateBillDtCtl.ascx");

            openUserControlPopup(url, id, 'Detail Item', 1100, 500);
        });

        $('.imgPrint.imgLink').die('click');
        $('.imgPrint.imgLink').live('click', function () {
            var $tr = $(this).closest('tr');
            var id = $tr.find('.hdnKeyField').val();
            var reportCode = "PM-00201";
            showLoadingPanel();
            openReportViewer(reportCode, id);
        });

        $('.imgPrintA6.imgLink').die('click');
        $('.imgPrintA6.imgLink').live('click', function () {
            var $tr = $(this).closest('tr');
            var id = $tr.find('.hdnKeyField').val();
            var reportCode = "PM-00236";
            showLoadingPanel();
            openReportViewer(reportCode, id);
        });

        $('.imgPrintF4.imgLink').die('click');
        $('.imgPrintF4.imgLink').live('click', function () {
            var $tr = $(this).closest('tr');
            var id = $tr.find('.hdnKeyField').val();
            var reportCode = "PM-00239";
            showLoadingPanel();
            openReportViewer(reportCode, id);
        });

        function onBeforeRightPanelPrint(code, filterExpression, errMessage) {
            var registrationID = $('#<%=hdnRegistrationID.ClientID %>').val();
            var linkedRegisID = $('#<%=hdnLinkedRegistrationID.ClientID %>').val();
            var healthcareServiceUnitID = $('#<%=hdnServiceUnitID.ClientID %>').val();

            if (code == 'PM-00203' || code == 'PM-00204' || code == 'PM-00210' || code == 'PM-00211' || code == 'PM-00212' || code == 'PM-00213') {
                if (linkedRegisID != 0 && linkedRegisID != null) {
                    filterExpression.text = '((RegistrationID = ' + linkedRegisID + ' AND IsChargesTransfered = 1) OR RegistrationID = ' + registrationID + ')';
                } else {
                    filterExpression.text = 'RegistrationID = ' + registrationID;
                }
            } else {
                filterExpression.text = 'RegistrationID = ' + registrationID;
            }

            if (code == 'PM-00224' || code == 'PM-00231' || code == 'PM-002168' || code == 'PM-00262' || code == 'PM-00265' || code == 'PM-00271' ||
                code == 'PM-00264' || code == 'PM-00269' || code == 'PM-00263' || code == 'PM-00267' || code == 'PM-00270' || code == 'PM-00266' ||
                code == 'PM-00268') {
                if (healthcareServiceUnitID != '' && healthcareServiceUnitID != 0) {
                    filterExpression.text = registrationID + ';' + healthcareServiceUnitID;
                    return true;
                } else {
                    errMessage.text = "Maaf, pilih Service Unit terlebih dahulu.";
                    return false;
                }
            }

            return true;
        }

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
                <col style="width: 120px" />
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
                <input type="hidden" id="hdnServiceUnitID" value="" runat="server" />
                <td class="tdLabel">
                    <label class="lblLink lblMandatory" id="lblServiceUnit">
                        <%=GetLabel("Unit Pelayanan")%></label>
                </td>
                <td>
                    <asp:TextBox ID="txtServiceUnitCode" Width="100%" runat="server" />
                </td>
                <td>
                    <asp:TextBox ID="txtServiceUnitName" ReadOnly="true" Width="250%" runat="server" />
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
                                                                    <%= GetLabel("Service Unit")%></div>
                                                                <div>
                                                                    <%= GetLabel("Created By")%></div>
                                                            </div>
                                                        </th>
                                                        <th colspan="3">
                                                            <%=GetLabel("Amount")%>
                                                        </th>
                                                        <th rowspan="2" align="center">
                                                            <div style="padding: 3px; float: center">
                                                                <div>
                                                                    <%= GetLabel("Re-Print")%></div>
                                                            </div>
                                                        </th>
                                                        <th rowspan="2" align="center">
                                                            <div style="padding: 3px; float: center">
                                                                <div>
                                                                    <%= GetLabel("Re-Print (A6)")%></div>
                                                            </div>
                                                        </th>
                                                        <th rowspan="2" align="center">
                                                            <div style="padding: 3px; float: center">
                                                                <div>
                                                                    <%= GetLabel("Re-Print (1/3 F4)")%></div>
                                                            </div>
                                                        </th>
                                                    </tr>
                                                    <tr>
                                                        <th style="width: 150px">
                                                            <div style="text-align: right; padding-right: 3px">
                                                                <%=GetLabel("Payer")%>
                                                            </div>
                                                        </th>
                                                        <th style="width: 150px">
                                                            <div style="text-align: right; padding-right: 3px">
                                                                <%=GetLabel("Patient")%>
                                                            </div>
                                                        </th>
                                                        <th style="width: 150px">
                                                            <div style="text-align: right; padding-right: 3px">
                                                                <%=GetLabel("Total")%>
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
                                                                    <%= GetLabel("Service Unit")%></div>
                                                                <div>
                                                                    <%= GetLabel("Created By")%></div>
                                                            </div>
                                                        </th>
                                                        <th colspan="3">
                                                            <%=GetLabel("Amount")%>
                                                        </th>
                                                        <th rowspan="2" align="center">
                                                            <div style="padding: 3px; float: center">
                                                                <div>
                                                                    <%= GetLabel("Re-Print")%></div>
                                                            </div>
                                                        </th>
                                                        <th rowspan="2" align="center">
                                                            <div style="padding: 3px; float: center">
                                                                <div>
                                                                    <%= GetLabel("Re-Print (A6)")%></div>
                                                            </div>
                                                        </th>
                                                        <th rowspan="2" align="center">
                                                            <div style="padding: 3px; float: center">
                                                                <div>
                                                                    <%= GetLabel("Re-Print (1/3 F4)")%></div>
                                                            </div>
                                                        </th>
                                                    </tr>
                                                    <tr>
                                                        <th style="width: 150px">
                                                            <div style="text-align: right; padding-right: 3px">
                                                                <%=GetLabel("Payer")%>
                                                            </div>
                                                        </th>
                                                        <th style="width: 150px">
                                                            <div style="text-align: right; padding-right: 3px">
                                                                <%=GetLabel("Patient")%>
                                                            </div>
                                                        </th>
                                                        <th style="width: 150px">
                                                            <div style="text-align: right; padding-right: 3px">
                                                                <%=GetLabel("Total")%>
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
                                                        <div style="padding: 3px; float: right; margin-right: 50px; <%#: Eval("IsPendingRecalculated").ToString() == "False" ? "display:none" : ""%>">
                                                            <table>
                                                                <tr>
                                                                    <td>
                                                                        <img height="24" src='<%= ResolveUrl("~/Libs/Images/Button/warning.png")%>' alt='' />
                                                                    </td>
                                                                    <td>
                                                                        <label class="lblInfo">
                                                                            <%=GetLabel("Pending Recalculated") %></label>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </div>
                                                        <div style="padding: 3px; float: left;">
                                                            <input type="hidden" class="hdnTransactionCode" value='<%#: Eval("TransactionCode")%>' />
                                                            <a class="lnkTransactionNo">
                                                                <%#: Eval("TransactionNo")%></a>
                                                            <div>
                                                                <%#: Eval("TransactionDateInString")%></div>
                                                        </div>
                                                    </td>
                                                    <td>
                                                        <div style="padding: 3px; float: left;">
                                                            <div>
                                                                <%#: Eval("ServiceUnitName")%></div>
                                                            <div>
                                                                <%#: Eval("LastUpdatedByUserName")%></div>
                                                        </div>
                                                    </td>
                                                    <td>
                                                        <div style="padding: 3px; text-align: right;">
                                                            <input type="hidden" class="hdnPayerAmount" value='<%#: Eval("TotalPayerAmount")%>' />
                                                            <div>
                                                                <%#: Eval("TotalPayerAmount", "{0:N}")%></div>
                                                        </div>
                                                    </td>
                                                    <td>
                                                        <div style="padding: 3px; text-align: right;">
                                                            <input type="hidden" class="hdnPatientAmount" value='<%#: Eval("TotalPatientAmount")%>' />
                                                            <div>
                                                                <%#: Eval("TotalPatientAmount", "{0:N}")%></div>
                                                        </div>
                                                    </td>
                                                    <td>
                                                        <div style="padding: 3px; text-align: right;">
                                                            <input type="hidden" class="hdnLineAmount" value='<%#: Eval("TotalAmount")%>' />
                                                            <div>
                                                                <%#: Eval("TotalAmount", "{0:N}")%></div>
                                                        </div>
                                                    </td>
                                                    <td>
                                                        <div style="text-align: center">
                                                            <img class="imgPrint imgLink" title='<%=GetLabel("Print")%>' src='<%# ResolveUrl("~/Libs/Images/Button/print.png")%>'
                                                                alt="" style="margin-right: 2px" />
                                                        </div>
                                                    </td>
                                                    <td>
                                                        <input type="hidden" class="hdnKeyField" value="<%#: Eval("TransactionID")%>" />
                                                        <div style="text-align: center">
                                                            <img class="imgPrintA6 imgLink" title='<%=GetLabel("Print")%>' src='<%# ResolveUrl("~/Libs/Images/Button/print.png")%>'
                                                                alt="" style="margin-right: 2px" />
                                                        </div>
                                                    </td>
                                                    <td>
                                                        <input type="hidden" class="hdnKeyField" value="<%#: Eval("TransactionID")%>" />
                                                        <div style="text-align: center">
                                                            <img class="imgPrintF4 imgLink" title='<%=GetLabel("Print")%>' src='<%# ResolveUrl("~/Libs/Images/Button/print.png")%>'
                                                                alt="" style="margin-right: 2px" />
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
