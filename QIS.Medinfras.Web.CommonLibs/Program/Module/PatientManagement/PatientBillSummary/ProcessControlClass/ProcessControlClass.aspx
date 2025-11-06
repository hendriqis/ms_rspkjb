<%@ Page Language="C#" MasterPageFile="~/Libs/MasterPage/PatientPage/MPBasePatientPageTrx.master"
    AutoEventWireup="true" CodeBehind="ProcessControlClass.aspx.cs" Inherits="QIS.Medinfras.Web.CommonLibs.Program.ProcessControlClass" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPopupControl" TagPrefix="dxpc" %>
<%@ Register Src="~/Libs/Program/Module/PatientManagement/TransactionDtView/TransactionControlClassDtServiceViewCtl.ascx"
    TagName="ServiceCtl" TagPrefix="uc1" %>
<%@ Register Src="~/Libs/Program/Module/PatientManagement/TransactionDtView/TransactionControlClassDtProductViewCtl.ascx"
    TagName="DrugMSCtl" TagPrefix="uc1" %>
<%@ Register Src="~/Libs/Program/Module/PatientManagement/PatientBillSummary/PatientBillSummaryToolbarCtl.ascx"
    TagName="ToolbarCtl" TagPrefix="uc1" %>
<asp:Content ID="Content4" ContentPlaceHolderID="plhMPPatientPageNavigationPane"
    runat="server">
    <uc1:ToolbarCtl ID="ctlToolbar" runat="server" />
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="plhCustomButtonToolbar" runat="server">
    <li id="btnProcessControlClass" runat="server" crudmode="R">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbrecalculate.png")%>' alt="" /><div>
            <%=GetLabel("Process")%></div>
    </li>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div class="menuTitle">
        <%=HttpUtility.HtmlEncode(GetPageTitle())%></div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="plhHeader" runat="server">
    <input type="hidden" value="" id="hdnRegistrationID" runat="server" />
    <input type="hidden" value="" id="hdnLinkedRegistrationID" runat="server" />
    <input type="hidden" value="" id="hdnDepartmentID" runat="server" />
    <input type="hidden" value="" id="hdnPageTitle" runat="server" />
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript" id="dxss_changecontrolclassctl">
        function onLoad() {
            $('#ulTabPatientBillSummaryDetailAll li').click(function () {
                $('#ulTabPatientBillSummaryDetailAll li.selected').removeAttr('class');
                $('.containerBillSummaryDetailAll').filter(':visible').hide();
                $contentID = $(this).attr('contentid');
                $('#' + $contentID).show();
                $(this).addClass('selected');
            });

            $('#<%=btnProcessControlClass.ClientID %>').click(function () {
                userControlType = 'processrecalculate';
                var id = $('#<%=hdnRegistrationID.ClientID %>').val();
                var url = ResolveUrl("~/Libs/Program/Module/PatientManagement/PatientBillSummary/ProcessControlClass/ProcessControlClassCalculation.ascx");
                openUserControlPopup(url, id, 'Recalculate Patient Bill', 1200, 500);
            });
        }

        function onBeforeLoadRightPanelContent(code) {
            return $('#<%:hdnRegistrationID.ClientID %>').val();
        }

        var userControlType = '';
        function onGetEntryPopupReturnValue() {
            return '';
        }

        function onAfterSaveRightPanelContent(code, value, isAdd) {
            if (userControlType == 'updatepayer') {
                var filterExpression = 'RegistrationID = ' + $('#<%=hdnRegistrationID.ClientID %>').val();
                Methods.getObject('GetvRegistrationList', filterExpression, function (result) {
                    setLblPayerText(result.BusinessPartner);
                    cbpProcessControlClass.PerformCallback();
                });
                userControlType = '';
            }
            else {
                cbpProcessControlClass.PerformCallback();
            }
        }

        function oncbpProcessControlClassEndCallback(s) {
            $('#ulTabPatientBillSummaryDetailAll li').click(function () {
                $('#ulTabPatientBillSummaryDetailAll li.selected').removeAttr('class');
                $('.containerBillSummaryDetailAll').filter(':visible').hide();
                $contentID = $(this).attr('contentid');
                $('#' + $contentID).show();
                $(this).addClass('selected');
            });
            hideLoadingPanel();
        }
    </script>
    <style type="text/css">
        .containerBillSummaryDetailAll
        {
            border: 1px solid #EAEAEA;
        }
    </style>
    <div>
        <table class="tblContentArea">
            <tr>
                <td style="padding-top: 5px;">
                    <dxcp:ASPxCallbackPanel ID="cbpProcessControlClass" runat="server" Width="100%"
                        ClientInstanceName="cbpProcessControlClass" ShowLoadingPanel="false" OnCallback="cbpProcessControlClass_Callback">
                        <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ oncbpProcessControlClassEndCallback(s); }" />
                        <PanelCollection>
                            <dx:PanelContent ID="PanelContent1" runat="server">
                                <div id="divWarningPendingRecalculated" runat="server">
                                    <table>
                                        <tr>
                                            <td rowspan="2">
                                                <img height="48" src='<%= ResolveUrl("~/Libs/Images/warning.png")%>' alt='' />
                                            </td>
                                            <td style="vertical-align: top">
                                                <label class="lblWarning">
                                                    <%=GetLabel("Proses Rekalkulasi Transaksi belum dilakukan") %></label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="vertical-align: top">
                                                <label class="lblNormal">
                                                    <%=GetLabel("terjadi perubahan penjamin bayar pasien") %></label>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <div class="containerUlTabPage">
                                    <ul class="ulTabPage" id="ulTabPatientBillSummaryDetailAll">
                                        <li class="selected" contentid="containerService">
                                            <%=GetLabel("Pelayanan") %></li>
                                        <li contentid="containerDrugMS">
                                            <%=GetLabel("Obat & Alkes") %></li>
                                        <li contentid="containerLogistics">
                                            <%=GetLabel("Barang Umum") %></li>
                                        <li contentid="containerLaboratory">
                                            <%=GetLabel("Laboratorium") %></li>
                                        <li contentid="containerImaging">
                                            <%=GetLabel("Radiologi") %></li>
                                        <li contentid="containerMedicalDiagnostic">
                                            <%=GetLabel("Penunjang Medis") %></li>
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
                                <div id="containerLaboratory" style="display: none" class="containerBillSummaryDetailAll">
                                    <uc1:ServiceCtl ID="ctlLaboratory" runat="server" />
                                </div>
                                <div id="containerImaging" style="display: none" class="containerBillSummaryDetailAll">
                                    <uc1:ServiceCtl ID="ctlImaging" runat="server" />
                                </div>
                                <div id="containerMedicalDiagnostic" style="display: none" class="containerBillSummaryDetailAll">
                                    <uc1:ServiceCtl ID="ctlMedicalDiagnostic" runat="server" />
                                </div>
                                <table style="width: 100%" cellpadding="0" cellspacing="0">
                                    <colgroup>
                                        <col style="width: 15%" />
                                        <col style="width: 35%" />
                                        <col style="width: 15%" />
                                        <col style="width: 35%" />
                                    </colgroup>
                                    <tr>
                                        <td>
                                            <div class="lblComponent" style="text-align: left; padding-left: 5px;">
                                                <%=GetLabel("Grand Total") %>
                                                :
                                            </div>
                                        </td>
                                        <td style="text-align: right; padding-right: 10px;">
                                            Rp.
                                            <asp:TextBox ID="txtTotal" ReadOnly="true" CssClass="number" runat="server" Width="200px" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <div class="lblComponent" style="text-align: left; padding-left: 5px;">
                                                <%=GetLabel("Grand Total Instansi") %>
                                                :
                                            </div>
                                        </td>
                                        <td style="text-align: right; padding-right: 10px;">
                                            Rp.
                                            <asp:TextBox ID="txtTotalPayer" ReadOnly="true" CssClass="number" runat="server"
                                                Width="200px" />
                                        </td>
                                        <td>
                                            <div class="lblComponent" style="text-align: left; padding-left: 5px;">
                                                <%=GetLabel("Grand Total Pasien") %>
                                                :
                                            </div>
                                        </td>
                                        <td style="text-align: right; padding-right: 10px;">
                                            Rp.
                                            <asp:TextBox ID="txtTotalPatient" ReadOnly="true" CssClass="number" runat="server"
                                                Width="200px" />
                                        </td>
                                    </tr>
                                </table>
                            </dx:PanelContent>
                        </PanelCollection>
                    </dxcp:ASPxCallbackPanel>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
