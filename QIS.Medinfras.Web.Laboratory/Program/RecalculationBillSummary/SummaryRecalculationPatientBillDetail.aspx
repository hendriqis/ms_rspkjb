<%@ Page Language="C#" MasterPageFile="~/Libs/MasterPage/PatientPage/MPBasePatientPageTrx.master" AutoEventWireup="true" 
    CodeBehind="SummaryRecalculationPatientBillDetail.aspx.cs" Inherits="QIS.Medinfras.Web.Laboratory.Program.SummaryRecalculationPatientBillDetail" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPopupControl" TagPrefix="dxpc" %>
<%@ Register Src="~/Libs/Program/Module/PatientManagement/TransactionDtView/TransactionDtServiceViewCtl.ascx" TagName="ServiceCtl" TagPrefix="uc1" %>
<%@ Register Src="~/Libs/Program/Module/PatientManagement/PatientBillSummary/PatientBillSummaryToolbarCtl.ascx" TagName="ToolbarCtl" TagPrefix="uc1" %>
<asp:Content ID="Content4" ContentPlaceHolderID="plhMPPatientPageNavigationPane" runat="server">   
    <uc1:ToolbarCtl ID="ctlToolbar" runat="server" />
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="plhCustomButtonToolbar" runat="server">
    <li id="btnRecalculationPatientBillRecalculate" runat="server" CRUDMode="R"><img src='<%=ResolveUrl("~/Libs/Images/Icon/tbset.png")%>' alt="" /><div><%=GetLabel("Recalculate")%></div></li>
    <li id="btnRecalculationPatientBillUpdatePayer" runat="server" CRUDMode="R"><img src='<%=ResolveUrl("~/Libs/Images/Icon/tbset.png")%>' alt="" /><div><%=GetLabel("Update Payer")%></div></li>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="plhHeader" runat="server">   
    <input type="hidden" value="" id="hdnRegistrationID" runat="server" />
    <input type="hidden" value="" id="hdnDepartmentID" runat="server" />
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">   
    <script type="text/javascript" id="dxss_generatebilldtctl">
        function onLoad() {
            $('#ulTabPatientBillSummaryDetailAll li').click(function () {
                $('#ulTabPatientBillSummaryDetailAll li.selected').removeAttr('class');
                $('.containerBillSummaryDetailAll').filter(':visible').hide();
                $contentID = $(this).attr('contentid');
                $('#' + $contentID).show();
                $(this).addClass('selected');
            });

            $('#<%=btnRecalculationPatientBillRecalculate.ClientID %>').click(function () {
                userControlType = 'processrecalculate';
                var id = $('#<%=hdnRegistrationID.ClientID %>').val();
                var url = ResolveUrl("~/Program/RecalculationBillSummary/SummaryRecalculationPatientBillProcessCtl.ascx");
                openUserControlPopup(url, id, 'Recalcalculate Patient Bill', 1100, 600);
            });

            $('#<%=btnRecalculationPatientBillUpdatePayer.ClientID %>').click(function () {
                userControlType = 'updatepayer';
                var id = $('#<%=hdnRegistrationID.ClientID %>').val() + '|' + $('#<%=hdnDepartmentID.ClientID %>').val();
                var url = ResolveUrl("~/Libs/Program/Module/PatientManagement/PatientBillSummary/RecalculationBill/PatientBillSummaryRecalculationBillUpdatePayerCtl.ascx");
                openUserControlPopup(url, id, 'Update Patient Payer', 700, 400);
            });
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
                    cbpRecalculationPatientBill.PerformCallback();
                });
                userControlType = '';
            }
            else {
                cbpRecalculationPatientBill.PerformCallback();
            }
        }

        function onCbpRecalculationPatientBillEndCallback(s) {
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
        .containerBillSummaryDetailAll                   { height: 300px;overflow-y:auto; border: 1px solid #EAEAEA; }
    </style>
    <div style="height:435px;overflow-y:auto;">
        <div class="pageTitle">
            <div style="font-size: 1.1em"><%=GetLabel("Rekalkulasi Transaksi Pasien")%></div>
        </div>
        <table class="tblContentArea">
            <tr>
                <td style="padding-top: 5px;">
                    <dxcp:ASPxCallbackPanel ID="cbpRecalculationPatientBill" runat="server" Width="100%" ClientInstanceName="cbpRecalculationPatientBill"
                        ShowLoadingPanel="false" OnCallback="cbpRecalculationPatientBill_Callback">
                        <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }"
                            EndCallback="function(s,e){ onCbpRecalculationPatientBillEndCallback(s); }" />
                        <PanelCollection>
                            <dx:PanelContent ID="PanelContent1" runat="server">
                                <div id="divWarningPendingRecalculated" runat="server">
                                    <table>
                                        <tr>
                                            <td><img height="24" src='<%= ResolveUrl("~/Libs/Images/Button/warning.png")%>' alt='' /></td>
                                            <td><label class="lblInfo"><%=GetLabel("Pending Recalculated") %></label></td>
                                        </tr>
                                    </table>
                                </div>
                                <div class="containerUlTabPage">
                                    <ul class="ulTabPage" id="ulTabPatientBillSummaryDetailAll">
                                        <li class="selected" contentid="containerLaboratory"><%=GetLabel("Laboratorium") %></li>
                                        <li contentid="containerImaging"><%=GetLabel("Radiologi") %></li>
                                        <li contentid="containerMedicalDiagnostic"><%=GetLabel("Penunjang Medis") %></li>
                                    </ul>
                                </div>

                                <div id="containerLaboratory" class="containerBillSummaryDetailAll">
                                     <uc1:ServiceCtl ID="ctlLaboratory" runat="server" />
                                </div>
                                <div id="containerImaging" style="display:none" class="containerBillSummaryDetailAll">
                                    <uc1:ServiceCtl ID="ctlImaging" runat="server" />
                                </div>
                                <div id="containerMedicalDiagnostic" style="display:none" class="containerBillSummaryDetailAll">
                                    <uc1:ServiceCtl ID="ctlMedicalDiagnostic" runat="server" />
                                </div>

                                <table style="width:100%" cellpadding="0" cellspacing="0">
                                    <colgroup>
                                        <col style="width:15%"/>
                                        <col style="width:35%"/>
                                        <col style="width:15%"/>
                                        <col style="width:35%"/>
                                    </colgroup>
                                    <tr>
                                        <td><div class="lblComponent" style="text-align: left; padding-left: 5px;"><%=GetLabel("Grand Total") %> : </div></td>
                                        <td style="text-align:right;padding-right: 10px;">
                                            Rp. <asp:TextBox ID="txtTotal" ReadOnly="true" CssClass="number" runat="server" Width="200px" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td><div class="lblComponent" style="text-align: left; padding-left: 5px;"><%=GetLabel("Grand Total Instansi") %> : </div></td>
                                        <td style="text-align:right;padding-right: 10px;">
                                            Rp. <asp:TextBox ID="txtTotalPayer" ReadOnly="true" CssClass="number" runat="server" Width="200px" />
                                        </td>
                                        <td><div class="lblComponent" style="text-align: left; padding-left: 5px;"><%=GetLabel("Grand Total Pasien") %>  : </div></td>
                                        <td style="text-align:right;padding-right: 10px;">
                                            Rp. <asp:TextBox ID="txtTotalPatient" ReadOnly="true" CssClass="number" runat="server" Width="200px" />
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