<%@ Page Title="" Language="C#" MasterPageFile="~/libs/MasterPage/MPEntry.master" AutoEventWireup="true" 
CodeBehind="ReportConfigurationEntry.aspx.cs" Inherits="QIS.Medinfras.Web.SystemSetup.Program.ReportConfigurationEntry" %>

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
                        <col style="width:30%"/>
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
                        <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Report Alternate Title")%></label></td>
                        <td><asp:TextBox ID="txtReportTitle2" Width="300px" runat="server" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Report Reference No")%></label></td>
                        <td><asp:TextBox ID="txtReportReferenceNo" Width="300px" runat="server" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Report Type")%></label></td>
                        <td><dxe:ASPxComboBox ID="cboReportType" Width="300px" runat="server" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Report Class Name")%></label></td>
                        <td><asp:TextBox ID="txtClassName" Width="300px" runat="server" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Data Source Type")%></label></td>
                        <td><dxe:ASPxComboBox ID="cboDataSourceType" Width="300px" runat="server" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Object Type Name")%></label></td>
                        <td><asp:TextBox ID="txtObjectTypeName" Width="300px" runat="server" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Additional Filter Expression")%></label></td>
                        <td><asp:TextBox ID="txtAdditionalFilterExpression" Width="300px" runat="server" /></td>
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
                        <td><asp:CheckBox runat="server" id="chkIsShowParameter" Text=" Show Parameter" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Top Margin") %></label></td>
                        <td><asp:TextBox ID="txtTopMargin" style="text-align:right" Width="120px" CssClass="number" runat="server"  /></td>
                    </tr>
                    <tr>
                        <td></td>
                        <td><asp:CheckBox runat="server" id="chkIsDirectPrint" Text=" Direct Print" /></td>
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
