<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TransactionPageToolbarCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.TransactionPageToolbarCtl" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
 
<div class="pageTitle" style="height: 43px; margin-top: 5px;">
    <input type="hidden" id="hdnVisitID" value="" runat="server" />
    <input type="hidden" id="hdnRegistrationID" value="" runat="server" />
    <input type="hidden" id="hdnDepartmentID" value="" runat="server" />
    <input type="hidden" id="hdnModuleID" value="" runat="server" />
    <input type="hidden" id="hdnParentCode" value="" runat="server" />
    <img class="imgLink" id="imgBackPatientPage" src='<%= ResolveUrl("~/Libs/Images/Icon/back.png")%>'
        alt="" style="float: left; margin-top: 3px;" title="<%=GetLabel("Back")%>" />
    <div style="float: right; margin-top: 3px;">
        <table border="0" cellpadding="0" cellspacing="0">
            <tr>
                <td>
                    <div id="divVisitNote" runat="server">
                        <img class="imgLink button hvr-pulse-grow" id="imgRegistrationNote" src='<%= ResolveUrl("~/Libs/Images/Toolbar/registration_notes.png")%>'
                            alt="" title="<%=GetLabel("Catatan Kunjungan Pasien")%>" width="32" height="32" />
                    </div>
                </td>
                <td>
                    <img class="imgLink" id="imgStartStopServiceTime" src='<%= ResolveUrl("~/Libs/Images/Icon/start.png")%>'
                        alt="" style="float: right; margin-top: 3px; display: none" title="<%=GetLabel("Mulai Pelayanan")%>" />
                </td>
                <td>
                    <div id="divLockTransaction" runat="server" style="float: right; margin-top: 3px;">
                        <img id="imgUnlock" class="imgStatus" src='<%= ResolveUrl("~/Libs/Images/Toolbar/unlockdown.png")%>'
                            title="<%=GetLabel("Transaction Locked")%>" alt="" />
                    </div>
                </td>
            </tr>
        </table>
    </div>
    <div  class="divNavigationPane">
    <asp:Repeater ID="rptHeader" runat="server" OnItemDataBound="rptHeader_ItemDataBound">
        <HeaderTemplate>
            <ul id="ulPatientPageHeader" class="ulNavigationPane">
        </HeaderTemplate>
        <ItemTemplate>
            <li id="liCaption" runat="server" url='<%#: Eval("MenuUrl") %>'>
                <%#: Eval("MenuCaption") %>
            </li>
        </ItemTemplate>
        <FooterTemplate>
            </ul>
        </FooterTemplate>
    </asp:Repeater>
    </div>
</div>
<div class="containerUlTabPage">
    <asp:Repeater ID="rptMenuChild" runat="server" OnItemDataBound="rptMenuChild_ItemDataBound">
        <HeaderTemplate>
            <ul class="ulTabPage" id="ulTabMenuChild">
        </HeaderTemplate>
        <ItemTemplate>
            <li id="liCaption" runat="server" url='<%#: Eval("MenuUrl") %>'>
                <%#: Eval("MenuCaption") %></li>
        </ItemTemplate>
        <FooterTemplate>
            </ul>
        </FooterTemplate>
    </asp:Repeater>
</div>
<script type="text/javascript" id="dxss_transactionpagetoolbarctl">
    function onGetUrlReferrer() {
        if ($('#<%=hdnModuleID.ClientID %>').val() == 'MD' && $('#<%=hdnParentCode.ClientID %>').val() == Constant.MenuCode.MD_PATIENT_PAGE) {
            return ResolveUrl("~/Program/PatientList/MDVisitList.aspx"); 
        }
        else if ($('#<%=hdnModuleID.ClientID %>').val() == 'LB' && $('#<%=hdnParentCode.ClientID %>').val() == Constant.MenuCode.LB_PATIENT_PAGE) {
            return ResolveUrl("~/Program/PatientList/LBVisitList.aspx");
        }
        else if ($('#<%=hdnParentCode.ClientID %>').val() == Constant.MenuCode.ER_PATIENT_PAGE) {
            return ResolveUrl("~/Program/PatientList/ERVisitList.aspx");
        }
        else if ($('#<%=hdnParentCode.ClientID %>').val() == Constant.MenuCode.OP_PATIENT_PAGE) {
            return ResolveUrl("~/Program/PatientList/OPVisitList.aspx");
        }
        else if ($('#<%=hdnParentCode.ClientID %>').val() == Constant.MenuCode.MC_PATIENT_PAGE) {
            return ResolveUrl("~/Program/PatientList/MCVisitList.aspx");
        }
        else if ($('#<%=hdnParentCode.ClientID %>').val() == Constant.MenuCode.FOLLOWUP_PATIENT_ER) {
            return ResolveUrl("~/Program/PatientList/FollowupPatientDischargeList.aspx");
        }
        else if ($('#<%=hdnParentCode.ClientID %>').val() == Constant.MenuCode.FOLLOWUP_PATIENT_IP) {
            return ResolveUrl("~/Program/PatientList/FollowupPatientDischargeList.aspx");
        }
        else if ($('#<%=hdnParentCode.ClientID %>').val() == Constant.MenuCode.FOLLOWUP_PATIENT_MD) {
            return ResolveUrl("~/Program/PatientList/FollowupPatientDischargeList.aspx");
        }
        else if ($('#<%=hdnParentCode.ClientID %>').val() == Constant.MenuCode.FOLLOWUP_PATIENT_OP) {
            return ResolveUrl("~/Program/PatientList/FollowupPatientDischargeList.aspx");
        }
        else if ($('#<%=hdnParentCode.ClientID %>').val() == Constant.MenuCode.EMR_PATIENT_PAGE) {
            return ResolveUrl("~/Program/PatientList/PatientVisitList.aspx");
        }
        else if ($('#<%=hdnParentCode.ClientID %>').val() == Constant.MenuCode.IMAGING_PATIENT_TRANSACTION_PAGE) {
            return ResolveUrl("~/Program/PatientList/RegistrationList.aspx");
        }
        else if ($('#<%=hdnParentCode.ClientID %>').val() == Constant.MenuCode.IMAGING_PATIENT_PAGE) {
            return ResolveUrl("~/Program/PatientList/PatientPageList.aspx");
        }
        else if ($('#<%=hdnParentCode.ClientID %>').val() == Constant.MenuCode.UDD_PRESCRIPTION_PROCESS) {
            return ResolveUrl("~/Program/Prescription/UDD/InpatientList.aspx");
        }
        else if ($('#<%=hdnParentCode.ClientID %>').val() == Constant.MenuCode.NURSING_PATIENT_PAGE) {
            return ResolveUrl("~/Program/Transaction/PatientList/PatientVisitList1.aspx");
        }
        else if ($('#<%=hdnParentCode.ClientID %>').val() == Constant.MenuCode.NURSING_PATIENT_LIST_INPATIENT) {
            return ResolveUrl("~/Program/Transaction/PatientList/InpatientVisitList1.aspx");
        }  
        else if ($('#<%=hdnParentCode.ClientID %>').val() == Constant.MenuCode.NUTRITION_PATIENT_LIST) {
            return ResolveUrl("~/Program/PatientList/VisitList.aspx");
        }
        else if ($('#<%=hdnParentCode.ClientID %>').val() == Constant.MenuCode.PHARMACIST_CLINICAL) {
            return ResolveUrl("~/Program/PatientList/PatientList1.aspx");
        }
        else if ($('#<%=hdnParentCode.ClientID %>').val() == Constant.MenuCode.RADIOTHERAPHY_PATIENT_PAGE) {
            return ResolveUrl("~/Program/PatientList/PatientPageList.aspx");
        }
        else {
            return ResolveUrl("~/Program/PatientList/VisitList.aspx?id=ptp");            
        }
    }

    $(function () {
        $('#imgRegistrationNote.imgLink').click(function () {
            var id = $('#<%=hdnRegistrationID.ClientID %>').val();
            var url = ResolveUrl("~/Libs/Program/Module/PatientManagement/Registration/ViewNotesCtl.ascx");
            openUserControlPopup(url, id, 'Catatan Kunjungan Pasien', 900, 500);
        });
    });

</script>
<div style="display: none">
</div>
