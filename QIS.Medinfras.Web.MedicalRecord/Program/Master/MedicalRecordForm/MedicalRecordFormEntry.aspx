<%@ Page Title="" Language="C#" MasterPageFile="~/libs/MasterPage/MPEntry.master" AutoEventWireup="true" 
CodeBehind="MedicalRecordFormEntry.aspx.cs" Inherits="QIS.Medinfras.Web.MedicalRecord.Program.MedicalRecordFormEntry" %>


<asp:Content ID="Content2" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div class="menuTitle"><%=HttpUtility.HtmlEncode(GetPageTitle())%></div>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <input type="hidden" id="hdnPageTitle" runat="server" />
    <input type="hidden" id="hdnID" runat="server" value="" />
    <%--<div class="pageTitle"><%=GetLabel("Formulir Berkas Rekam Medis")%></div>--%>
    <table class="tblContentArea">
        <colgroup>
            <col style="width:50%"/>
        </colgroup>
        <tr>
            <td style="padding:5px;vertical-align:top">
                <table class="tblEntryContent" style="width:50%">
                    <colgroup>
                        <col style="width:25%"/>
                    </colgroup>
                    <tr>
                        <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("No. Formulir RM")%></label></td>
                        <td><asp:TextBox ID="txtFormCode" Width="120px" runat="server" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Nama Formulir RM")%></label></td>
                        <td><asp:TextBox ID="txtFormName" Width="300px" runat="server" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Catatan")%></label></td>
                        <td><asp:TextBox ID="txtRemarks" Width="300px" runat="server" TextMode="MultiLine" Rows="2" /></td>
                    </tr>
                    <tr>
                        <td></td>
                        <td><asp:CheckBox ID="chkIsGeneratedForm" runat="server" /> <%=GetLabel("Form Elektronik")%></td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>
