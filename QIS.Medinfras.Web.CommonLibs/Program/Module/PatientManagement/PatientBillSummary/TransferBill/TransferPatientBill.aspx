<%@ Page Language="C#" MasterPageFile="~/Libs/MasterPage/PatientPage/MPBasePatientPageTrx.Master" AutoEventWireup="true" 
    CodeBehind="TransferPatientBill.aspx.cs" Inherits="QIS.Medinfras.Web.CommonLibs.Program.TransferPatientBill" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Src="~/Libs/Program/Module/PatientManagement/PatientBillSummary/PatientBillSummaryToolbarCtl.ascx" TagName="ToolbarCtl" TagPrefix="uc1" %>
<%@ Register Src="~/Libs/Program/Module/PatientManagement/TransactionDtView/TransactionDtServiceViewCtl.ascx" TagName="ServiceCtl" TagPrefix="uc1" %>
<%@ Register Src="~/Libs/Program/Module/PatientManagement/TransactionDtView/TransactionDtProductViewCtl.ascx" TagName="DrugMSCtl" TagPrefix="uc1" %>

<asp:Content ID="Content4" ContentPlaceHolderID="plhMPPatientPageNavigationPane" runat="server">   
    <uc1:ToolbarCtl ID="ctlToolbar" runat="server" />
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="plhMenuTitle" runat="server">   
    <div class="menuTitle"><%=HttpUtility.HtmlEncode(GetPageTitle())%></div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="plhCustomButtonToolbar" runat="server">
    <li id="btnProcessGenerateBill" CRUDMode="R" runat="server"><img src='<%=ResolveUrl("~/Libs/Images/Icon/tbprocess.png")%>' alt="" /><br style="clear:both"/><div><%=GetLabel("Process")%></div></li>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="plhHeader" runat="server">   
    <input type="hidden" value="" id="hdnRegistrationID" runat="server" />
    <input type="hidden" value="" id="hdnIsAllowOPEN" runat="server" />
    <input type="hidden" value="" id="hdnLinkedRegistrationID" runat="server" />  
    <input type="hidden" value="" id="hdnDepartmentID" runat="server" />
    <input type="hidden" value="" id="hdnHealthcareServiceUnitID" runat="server" />  
    <input type="hidden" value="" id="hdnPageTitle" runat="server" />
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">   
    <script type="text/javascript">
        $(function () {
            $('#ulTabPatientBillSummaryDetailAll li').click(function () {
                $('#ulTabPatientBillSummaryDetailAll li.selected').removeAttr('class');
                $('.containerBillSummaryDetailAll').filter(':visible').hide();
                $contentID = $(this).attr('contentid');
                $('#' + $contentID).show();
                $(this).addClass('selected');
            });

            $('#<%=btnProcessGenerateBill.ClientID %>').click(function () {
                cbpProcess.PerformCallback();
            });

            $('#lblInfoOutstandingBill').click(function () {
                var id = $('#<%=hdnLinkedRegistrationID.ClientID %>').val();
                var url = ResolveUrl("~/Program/Information/TransactionPatientInformationDetailCtl.ascx");
                openUserControlPopup(url, id, 'Detail Informasi Pasien', 1100, 500);
            });
        });

        function onCbpProcessEndCallback(s) {
            var param = s.cpResult.split('|');
            if (param[0] == 'fail')
                showToast('Transfer Failed', 'Error Message : ' + param[1]);
            else {
                showToast('Transfer Success', '', function () {
                    $('#<%=btnProcessGenerateBill.ClientID %>').click();
                });
            }
            hideLoadingPanel();
        }

    </script> 
     <style type="text/css">
        .containerBillSummaryDetailAll                   { height: 300px;overflow-y:auto; border: 1px solid #EAEAEA; }
    </style>
    <input type="hidden" id="hdnOutstandingCount" runat="server" />
    <div style="height:435px;overflow-y:auto;">
        <div class="pageTitle">
            <div style="font-size: 1.1em"><%=GetLabel("Transfer Tagihan Pasien dari Unit Lain")%></div>
        </div>
        <table class="tblContentArea">
            <tr>
                <td style="padding-top: 5px;">
                    <table id="tblInfoOutstandingBill" runat="server">
                        <tr>
                            <td><img height="24" src='<%= ResolveUrl("~/Libs/Images/Button/warning.png")%>' alt='' /></td>
                            <td><label class="lblInfo" id="lblInfoOutstandingBill"><%=GetLabel("Masih Ada Bill Yang Belum Lunas / Order Yang Belum Direalisasi") %></label></td>
                        </tr>
                    </table>
                    <div class="containerUlTabPage">
                        <ul class="ulTabPage" id="ulTabPatientBillSummaryDetailAll">
                            <li class="selected" contentid="containerService"><%=GetLabel("Pelayanan") %></li>
                            <li contentid="containerDrugMS"><%=GetLabel("Obat & Alkes") %></li>
                            <li contentid="containerLogistics"><%=GetLabel("Barang Umum") %></li>
                            <li contentid="containerLaboratory"><%=GetLabel("Laboratorium") %></li>
                            <li contentid="containerImaging"><%=GetLabel("Radiologi") %></li>
                            <li contentid="containerMedicalDiagnostic"><%=GetLabel("Penunjang Medis") %></li>
                        </ul>
                    </div>

                    <div id="containerService" class="containerBillSummaryDetailAll">
                        <uc1:ServiceCtl ID="ctlService" runat="server" />
                    </div>
                    <div id="containerDrugMS" style="display:none" class="containerBillSummaryDetailAll">
                        <uc1:DrugMSCtl ID="ctlDrugMS" runat="server" />
                    </div>
                    <div id="containerLogistics" style="display:none" class="containerBillSummaryDetailAll">
                        <uc1:DrugMSCtl ID="ctlLogistic" runat="server" />
                    </div>
                    <div id="containerLaboratory" style="display:none" class="containerBillSummaryDetailAll">
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
                </td>
            </tr>
        </table>
    </div>
    <dxcp:ASPxCallbackPanel ID="cbpProcess" runat="server" Width="100%" ClientInstanceName="cbpProcess"
        ShowLoadingPanel="false" OnCallback="cbpProcess_Callback">
            <ClientSideEvents BeginCallback="function(s,e) { showLoadingPanel(); }"
        EndCallback="function(s,e) { onCbpProcessEndCallback(s); }" />
    </dxcp:ASPxCallbackPanel>
</asp:Content>