<%@ Page Title="" Language="C#" MasterPageFile="~/libs/MasterPage/MPEntry.master" AutoEventWireup="true" 
CodeBehind="NursingInterventionEntry.aspx.cs" Inherits="QIS.Medinfras.Web.Nursing.Program.NursingInterventionEntry" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>

<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <input type="hidden" id="hdnID" runat="server" value="" />
    <div class="pageTitle"><%=GetLabel("Intervensi Keperawatan")%></div>
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
                        <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Kode Intervensi")%></label></td>
                        <td><asp:TextBox ID="txtNursingInterventionCode" Width="150px" runat="server" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Nama Intervensi")%></label></td>
                        <td><asp:TextBox ID="txtNursingInterventionName" Width="300px" runat="server" /></td>
                    </tr>
                    <tr valign="top">
                        <td class="tdLabel" style="margin-top:2px"><label class="lblnormal"><%=GetLabel("Definisi")%></label></td>
                        <td><asp:TextBox ID="txtDescription" TextMode="MultiLine" Rows="5" Width="300px" runat="server" /></td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>
