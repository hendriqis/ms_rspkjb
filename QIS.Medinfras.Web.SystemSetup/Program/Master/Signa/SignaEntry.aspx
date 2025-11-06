<%@ Page Language="C#" MasterPageFile="~/Libs/MasterPage/MPEntry.master" AutoEventWireup="true" 
    CodeBehind="SignaEntry.aspx.cs" Inherits="QIS.Medinfras.Web.SystemSetup.Program.SignaEntry" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>

<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <input type="hidden" id="hdnID" runat="server" value="" />
    <div class="pageTitle"><%=GetLabel("Signa")%></div>
    <table class="tblEntryContent" style="width:70%">
        <colgroup>
            <col style="width:30%"/>
        </colgroup>
        <tr>
            <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Signa Label")%></label></td>
            <td><asp:TextBox ID="txtSignaLabel" Width="300px" runat="server" /></td>
        </tr>
        <tr>
            <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Signa Name 1")%></label></td>
            <td><asp:TextBox ID="txtSignaName1" Width="300px" runat="server" /></td>
        </tr>
        <tr>
            <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Signa Name 2")%></label></td>
            <td><asp:TextBox ID="txtSignaName2" Width="300px" runat="server" /></td>
        </tr>
        <tr>
            <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Drug Form")%></label></td>
            <td><dxe:ASPxComboBox ID="cboGCDrugForm" Width="200px" runat="server" /></td>
        </tr>
        <tr>
            <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Dose")%></label></td>
            <td><asp:TextBox ID="txtDose" Width="100px" CssClass="number" runat="server" /></td>
        </tr>
        <tr>
            <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Dose In String")%></label></td>
            <td><asp:TextBox ID="txtDoseInString" Width="300px" runat="server" /></td>
        </tr>
        <tr>
            <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Dose Unit")%></label></td>
            <td><dxe:ASPxComboBox ID="cboGCDoseUnit" runat="server" Width="200px" /></td>
        </tr>
        <tr>
            <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Dosing Frequency")%></label></td>
            <td><dxe:ASPxComboBox ID="cboGCDosingFrequency" runat="server" Width="200px" /></td>
        </tr>
        <tr>
            <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Frequency")%></label></td>
            <td><asp:TextBox ID="txtFrequency" Width="100px" CssClass="number" runat="server" /></td>
        </tr>
    </table>
</asp:Content>
