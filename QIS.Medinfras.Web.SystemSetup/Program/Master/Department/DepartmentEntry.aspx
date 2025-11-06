<%@ Page Language="C#" MasterPageFile="~/Libs/MasterPage/MPEntry.master" AutoEventWireup="true" 
    CodeBehind="DepartmentEntry.aspx.cs" Inherits="QIS.Medinfras.Web.SystemSetup.Program.DepartmentEntry" %>
    
<asp:Content ID="Content2" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div class="menuTitle"><%=HttpUtility.HtmlEncode(GetPageTitle())%></div>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript" id="dxss_departmententry">
        $('#btnGetIHS').live('click', function () {
            var departmentID = $('#<%=txtDepartmentCode.ClientID %>').val();
            var departmentName = $('#<%=txtDepartmentName.ClientID %>').val();
            try {
                IHSService.createIHSOrganizationID(departmentID, departmentName, function (result) {
                    GetIHSDataHandler(result);
                });
            }
            catch (err) {
                displayErrorMessageBox("Integrasi SATUSEHAT", err.Message);
            }
        });

        function GetIHSDataHandler(result) {
            try {
                var resultInfo = result.split('|');
                if (resultInfo[0] == "1") {
                    $('#<%=txtIHSOrganizationID.ClientID %>').val(resultInfo[1]);
                }
                else {
                    $('#<%=txtIHSOrganizationID.ClientID %>').val('');
                    displayErrorMessageBox('Integrasi SatuSehat', resultInfo[2]);
                }
            }
            catch (err) {
                displayErrorMessageBox('Integrasi SATUSEHAT', 'Error Message : ' + err.Description);
            }
        }
    </script>

    <input type="hidden" id="hdnPageTitle" runat="server" />
    <input type="hidden" id="hdnID" runat="server" value="" />
    <%--<div class="pageTitle"><%=GetLabel("Facility")%></div>--%>
    <table class="tblContentArea">
        <colgroup>
            <col style="width:20%"/>
            <col style="width:50%"/>
        </colgroup>
        <tr>
            <td style="padding:5px;vertical-align:top">
                <table class="tblEntryContent" style="width:100%">
                    <colgroup>
                        <col style="width:28%"/>
                    </colgroup>
                    <tr>
                        <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Kode Instalasi")%></label></td>
                        <td><asp:TextBox ID="txtDepartmentCode" Width="300px" runat="server" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Nama Instalasi")%></label></td>
                        <td><asp:TextBox ID="txtDepartmentName" Width="300px" runat="server" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Nama Singkat")%></label></td>
                        <td><asp:TextBox ID="txtShortName" Width="150px" runat="server" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Inisial")%></label></td>
                        <td><asp:TextBox ID="txtInitial" Width="150px" runat="server" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblNormal" title="Kode Lokasi/Organisasi di Platform SATUSEHAT Kemenkes"><%=GetLabel("IHS Organization ID")%></label></td>
                        <td><asp:TextBox ID="txtIHSOrganizationID" Width="300px" runat="server" /></td>
                        <td>
                            <input type="button" id="btnGetIHS" value="Get IHS Organization ID" style="width: 100%;" class="btnGetIHS w3-btn1 w3-hover-blue" />
                        </td>
                    </tr>
                    <tr style="display:none">
                        <td class="tdLabel"><label class="lblNormal"><%=GetLabel("No Akunt GL Segment")%></label></td>
                        <td><asp:TextBox ID="txtGLAccountSegmentNo" Width="150px" runat="server" /></td>
                    </tr>
                </table>
            </td>
            <td style="padding:5px;vertical-align:top">
                <div><asp:CheckBox ID="chkIsActive" runat="server" /> <%=GetLabel("Aktif")%></div>
                <div><asp:CheckBox ID="chkIsAllowPatientRegistration" runat="server" /> <%=GetLabel("Pendaftaran Pasien")%></div>
                <div><asp:CheckBox ID="chkIsAllowPrescriptionOrder" runat="server" /> <%=GetLabel("Pelayanan Order Farmasi (Resep)")%></div>
            </td>
        </tr>
    </table>
</asp:Content>
