<%@ Page Title="" Language="C#" MasterPageFile="~/Libs/MasterPage/PatientPage/MPBasePatientPageTrx.master"
    AutoEventWireup="true" CodeBehind="CloseBilling.aspx.cs" Inherits="QIS.Medinfras.Web.CommonLibs.Program.CloseBilling" %>

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
    <li id="btnCloseBilling" crudmode="R" runat="server">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbapprove.png")%>' alt="" /><br style="clear: both" />
        <div>
            <%=GetLabel("Close")%></div>
    </li>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="plhHeader" runat="server">
    <input type="hidden" value="" id="hdnRegistrationID" runat="server" />
    <input type="hidden" value="" id="hdnRegistrationNo" runat="server" />
    <input type="hidden" value="" id="hdnDepartmentID" runat="server" />
    <input type="hidden" value="" id="hdnHealthcareServiceUnitID" runat="server" />
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript">
        $(function () {
            $('#<%=btnCloseBilling.ClientID %>').click(function () {
                if (IsValid(null, 'fsEditReg', 'mpEditReg'))
                    onCustomButtonClick('update');
            });
        });

        function onAfterCustomClickSuccess(type) {
            document.location.reload(true);
        }

        function onAfterPopupControlClosing() {
            document.location.reload(true);
        }
    </script>
    <input type="hidden" value="" id="hdnPageTitle" runat="server" />
    <fieldset id="fsEditReg">
        <table class="tblContentArea" style="width: 600px">
            <colgroup>
                <col style="width: 30%" />
                <col style="width: 70%" />
            </colgroup>
            <tr>
                <td class="tdLabel">
                    <label class="lblNormal">
                        <%=GetLabel("Status Lock")%></label>
                </td>
                <td>
                    <asp:TextBox ID="txtLockStatus" ReadOnly="true" Width="100%" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="tdLabel">
                    <label class="lblNormal">
                        <%=GetLabel("Terakhir Lock Oleh")%></label>
                </td>
                <td>
                    <asp:TextBox ID="txtLockBy" ReadOnly="true" Width="100%" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="tdLabel">
                    <label class="lblNormal">
                        <%=GetLabel("Terakhir Lock Pada")%></label>
                </td>
                <td>
                    <asp:TextBox ID="txtLockDate" ReadOnly="true" Width="100%" runat="server" />
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <hr />
                </td>
            </tr>
            <tr>
                <td class="tdLabel">
                    <label class="lblNormal">
                        <%=GetLabel("Status Billing")%></label>
                </td>
                <td>
                    <asp:TextBox ID="txtBillingStatus" ReadOnly="true" Width="100%" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="tdLabel">
                    <label class="lblNormal">
                        <%=GetLabel("Terakhir Billing Ditutup Oleh")%></label>
                </td>
                <td>
                    <asp:TextBox ID="txtBillingClosedBy" ReadOnly="true" Width="100%" runat="server" />
                </td>
            </tr>
            <tr>
                <td class="tdLabel">
                    <label class="lblNormal">
                        <%=GetLabel("Terakhir Billing Ditutup Pada")%></label>
                </td>
                <td>
                    <asp:TextBox ID="txtBillingClosedDate" ReadOnly="true" Width="100%" runat="server" />
                </td>
            </tr>
        </table>
    </fieldset>
</asp:Content>
