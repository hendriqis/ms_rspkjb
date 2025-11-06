<%@ Page Title="" Language="C#" MasterPageFile="~/libs/MasterPage/MPEntry.master" AutoEventWireup="true" 
CodeBehind="ReportConfigurationUserEntry.aspx.cs" Inherits="QIS.Medinfras.Web.SystemSetup.Program.ReportConfigurationUserEntry" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
    
<asp:Content ID="Content2" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div class="menuTitle"><%=HttpUtility.HtmlEncode(GetPageTitle())%></div>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <input type="hidden" id="hdnPageTitle" runat="server" />
    <input type="hidden" id="hdnID" runat="server" value="" />
    <table class="tblContentArea">
        <colgroup>
            <col style="width:50%"/>
        </colgroup>
        <tr>
            <td style="padding:5px;vertical-align:top">
                <table class="tblEntryContent" style="width:50%">
                    <colgroup>
                        <col style="width:15%"/>
                    </colgroup>
                    <tr>
                        <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Report Code")%></label></td>
                        <td><asp:TextBox ID="txtReportCode" Width="100px" runat="server" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Report Title")%></label></td>
                        <td><asp:TextBox ID="txtReportTitle1" Width="300px" runat="server" /></td>
                    </tr>
                    <tr>
                         <td></td>
                        <td><asp:CheckBox runat="server" id="chkIsShowHeader" Text=" Show Header" /></td>
                    </tr>
                    <tr>
                        <td></td>
                        <td><asp:CheckBox runat="server" id="chkIsShowFooter" Text=" Show Footer" /></td>
                    </tr>
                    <tr>
                        <td></td>
                        <td><asp:CheckBox runat="server" id="chkIsUsingPreview" Text=" Is Using Report Viewer" /></td>
                    </tr>
                    <tr>
                        <td></td>
                        <td><asp:CheckBox runat="server" id="chkIsAllowDownloadExcel" Text=" Is Allow Download Excel" /></td>
                    </tr>
                    <tr>
                        <td></td>
                        <td><asp:CheckBox runat="server" id="chkIsAllowDownloadRawExcel" Text=" Is Allow Download Raw Excel" /></td>
                    </tr>
                    <tr>
                        <td></td>
                        <td><asp:CheckBox runat="server" id="chkIsAllowDownloadRawExcelStaging" Text=" Is Allow Download Raw Excel (Staging)" /></td>
                    </tr>
                    <tr>
                        <td></td>
                        <td><asp:CheckBox runat="server" id="chkIsAllowDownloadImageExcel" Text=" Is Allow Download Image Excel" /></td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>
