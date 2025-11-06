<%@ Page Title="" Language="C#" MasterPageFile="~/libs/MasterPage/MPEntry.master" AutoEventWireup="true" 
CodeBehind="NursingIndicatorMasterEntry.aspx.cs" Inherits="QIS.Medinfras.Web.Nursing.Program.NursingIndicatorMasterEntry" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>

<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <input type="hidden" id="hdnID" runat="server" value="" />
    <div class="pageTitle"><%=GetLabel("Kriteria Hasil Asuhan Keperawatan")%></div>
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
                        <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Kode Kriteria")%></label></td>
                        <td><asp:TextBox ID="txtNursingOutcomeCode" Width="150px" runat="server" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Nama Kriteria")%></label></td>
                        <td><asp:TextBox ID="txtNursingOutcomeName" Width="300px" runat="server" /></td>
                    </tr>
                    <tr valign="top">
                        <td class="tdLabel" style="margin-top:2px"><label class="lblMandatory"><%=GetLabel("Skala 1")%></label></td>
                        <td><asp:TextBox ID="txtScale1" Width="300px" runat="server" /></td>
                    </tr>
                    <tr valign="top">
                        <td class="tdLabel" style="margin-top:2px"><label class="lblMandatory"><%=GetLabel("Skala 2")%></label></td>
                        <td><asp:TextBox ID="txtScale2" Width="300px" runat="server" /></td>
                    </tr>
                    <tr valign="top">
                        <td class="tdLabel" style="margin-top:2px"><label class="lblMandatory"><%=GetLabel("Skala 3")%></label></td>
                        <td><asp:TextBox ID="txtScale3" Width="300px" runat="server" /></td>
                    </tr>
                    <tr valign="top">
                        <td class="tdLabel" style="margin-top:2px"><label class="lblMandatory"><%=GetLabel("Skala 4")%></label></td>
                        <td><asp:TextBox ID="txtScale4" Width="300px" runat="server" /></td>
                    </tr>
                    <tr valign="top">
                        <td class="tdLabel" style="margin-top:2px"><label class="lblMandatory"><%=GetLabel("Skala 5")%></label></td>
                        <td><asp:TextBox ID="txtScale5" Width="300px" runat="server" /></td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>
