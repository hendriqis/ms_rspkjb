<%@ Page Title="" Language="C#" MasterPageFile="~/Libs/MasterPage/PatientPage/MPBasePatientPageTrx.master"
    AutoEventWireup="true" CodeBehind="ViewBillDetail.aspx.cs" Inherits="QIS.Medinfras.Web.CommonLibs.Program.ViewBillDetail" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Src="~/Libs/Program/Module/PatientManagement/TransactionDtView/TransactionDtServiceViewCtl.ascx"
    TagName="ServiceCtl" TagPrefix="uc1" %>
<%@ Register Src="~/Libs/Program/Module/PatientManagement/TransactionDtView/TransactionDtProductViewCtl.ascx"
    TagName="DrugMSCtl" TagPrefix="uc1" %>
<%@ Register Src="~/Libs/Program/Module/PatientManagement/PatientBillSummary/PatientBillSummaryToolbarCtl.ascx"
    TagName="ToolbarCtl" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="plhMPPatientPageNavigationPane"
    runat="server">
    <uc1:ToolbarCtl ID="ctlToolbar" runat="server" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="plhCustomButtonToolbar" runat="server">
    <li id="btnProcessGenerateBill" crudmode="R" runat="server">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbapprove.png")%>' alt="" /><br style="clear: both" />
        <div>
            <%=GetLabel("Review")%></div>
    </li>
    <li id="btnVoidGenerateBill" crudmode="R" runat="server">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbvoid.png")%>' alt="" /><br style="clear: both" />
        <div>
            <%=GetLabel("Cancel")%></div>
    </li>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div class="menuTitle">
        <%=HttpUtility.HtmlEncode(GetPageTitle())%></div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="plhHeader" runat="server">
    <input type="hidden" value="" id="hdnDepartmentID" runat="server" />
    <input type="hidden" value="" id="hdnRegistrationID" runat="server" />
    <input type="hidden" value="" id="hdnLinkedRegistrationID" runat="server" />
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript">
        function onLoad() {
            setDatePicker('<%=txtFilterOrderDate.ClientID %>');
            $('#<%=txtFilterOrderDate.ClientID %>').datepicker('option', 'maxDate', '0');

            $('#ulTabPatientBillSummaryDetailAll li').click(function () {
                $('#ulTabPatientBillSummaryDetailAll li.selected').removeAttr('class');
                $('.containerBillSummaryDetailAll').filter(':visible').hide();
                $contentID = $(this).attr('contentid');
                $('#' + $contentID).show();
                $(this).addClass('selected');
            });
        }

        $('#<%=btnProcessGenerateBill.ClientID %>').live('click', function () {
            $('#<%=hdnVerifyCancel.ClientID %>').val('verify');
            callCustomButton();
        });

        $('#<%=btnVoidGenerateBill.ClientID %>').live('click', function () {
            $('#<%=hdnVerifyCancel.ClientID %>').val('cancel');
            callCustomButton();

        });

        function callCustomButton() {
            var lstSelectedValue = "";
            $('.chkIsSelected input:checked').each(function () {
                var $td = $(this).parent().parent();
                var input = $td.find('.hdnKeyField').val();
                if (lstSelectedValue != "") lstSelectedValue += ",";
                lstSelectedValue += input;
            });
            $('#<%=hdnLstSelectedValue.ClientID %>').val(lstSelectedValue);
            if (lstSelectedValue == "")
                showToast('Warning', 'Pilih transaksi yang ingin di verifikasi');
            else
                onCustomButtonClick('generatebill');
        }

        function onAfterCustomClickSuccess(type) {
            cbpView.PerformCallback();
        }

        $('#<%=rblFilterDate.ClientID %>').live('change', function () {
            var value = $(this).find('input:checked').val();
            if (value == 'true') {
                $('#trDate').css('display', '');
            }
            else $('#trDate').css('display', 'none');
            cbpView.PerformCallback();
        });

        $('#<%=txtFilterOrderDate.ClientID %>').live('change', function () {
            cbpView.PerformCallback();
        });

        function onCboServiceUnitChanged() {
            cbpView.PerformCallback();
        }

        $('.chkSelectAll input').live('change', function () {
            var value = $(this).prop("checked");
            //$('.chkIsSelected input').prop("checked", value);
            $('.chkIsSelected input').each(function () {
                $(this).prop("checked", value);
            });
        });

    </script>
    <div>
        <input type="hidden" id="hdnLstSelectedValue" runat="server" value="" />
        <input type="hidden" id="hdnVerifyCancel" runat="server" value="" />
        <table class="tblEntryContent" style="width: 100%">
            <colgroup>
                <col width="200px" />
            </colgroup>
            <tr>
                <td class="tdLabel">
                    <label class="lblNormal">
                        <%=GetLabel("Unit Pelayanan")%></label>
                </td>
                <td>
                    <dxe:ASPxComboBox ID="cboServiceUnit" ClientInstanceName="cboServiceUnit" Width="250px"
                        runat="server">
                        <ClientSideEvents ValueChanged="function(s,e) { onCboServiceUnitChanged(); }" />
                    </dxe:ASPxComboBox>
                </td>
            </tr>
            <tr>
                <td class="tdLabel">
                    <label class="lblNormal">
                        <%=GetLabel("Filter Tanggal") %></label>
                </td>
                <td>
                    <asp:RadioButtonList runat="server" ID="rblFilterDate" RepeatDirection="Horizontal">
                        <asp:ListItem Text="On" Value="true" />
                        <asp:ListItem Text="Off" Value="false" Selected="True" />
                    </asp:RadioButtonList>
                </td>
            </tr>
            <tr id="trDate" style="display: none">
                <td class="tdLabel">
                    <label class="lblNormal">
                        <%=GetLabel("Tanggal") %>
                    </label>
                </td>
                <td>
                    <asp:TextBox ID="txtFilterOrderDate" Width="110px" CssClass="datepicker" runat="server" />
                </td>
            </tr>
        </table>
        <div>
            <table class="tblContentArea">
                <tr>
                    <td>
                        <div>
                            <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
                                ShowLoadingPanel="false" OnCallback="cbpView_Callback">
                                <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e) { hideLoadingPanel(s); onLoad();}" />
                                <PanelCollection>
                                    <dx:PanelContent ID="PanelContent1" runat="server">
                                        <asp:Panel runat="server" ID="pnlService" Style="width: 100%; margin-left: auto;
                                            margin-right: auto; position: relative; font-size: 0.95em;">
                                            <div class="containerUlTabPage">
                                                <ul class="ulTabPage" id="ulTabPatientBillSummaryDetailAll">
                                                    <li class="selected" contentid="containerService">
                                                        <%=GetLabel("Pelayanan") %></li>
                                                    <li contentid="containerDrugMS">
                                                        <%=GetLabel("Obat & Alkes") %></li>
                                                    <li contentid="containerLogistics">
                                                        <%=GetLabel("Barang Umum") %></li>
                                                </ul>
                                            </div>
                                            <div id="containerService" class="containerBillSummaryDetailAll">
                                                <uc1:ServiceCtl ID="ctlService" runat="server" />
                                            </div>
                                            <div id="containerDrugMS" style="display: none" class="containerBillSummaryDetailAll">
                                                <uc1:DrugMSCtl ID="ctlDrugMS" runat="server" />
                                            </div>
                                            <div id="containerLogistics" style="display: none" class="containerBillSummaryDetailAll">
                                                <uc1:DrugMSCtl ID="ctlLogistic" runat="server" />
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
    </div>
</asp:Content>
