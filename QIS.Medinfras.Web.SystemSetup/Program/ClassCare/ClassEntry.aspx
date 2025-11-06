<%@ Page Language="C#" MasterPageFile="~/Libs/MasterPage/MPEntry.master" AutoEventWireup="true" 
    CodeBehind="ClassEntry.aspx.cs" Inherits="QIS.Medinfras.Web.SystemSetup.Program.ClassEntry" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>

<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <input type="hidden" id="hdnID" runat="server" value="" />
    <div class="pageTitle"><%=GetLabel("ClassCare")%></div>
    <table class="tblEntryContent" style="width:70%">
        <colgroup>
            <col style="width:30%"/>
        </colgroup>
        <tr>
            <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Class Code")%></label></td>
            <td><asp:TextBox ID="txtClassCode" Width="300px" runat="server" /></td>
        </tr>
        <tr>
            <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Class Name")%></label></td>
            <td><asp:TextBox ID="txtClassName" Width="300px" runat="server" /></td>
        </tr>
        <tr>
            <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Short Name")%></label></td>
            <td><asp:TextBox ID="txtShortName" Width="150px" runat="server" /></td>
        </tr>
        <tr>
            <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Standard RL Class")%></label></td>
            <td><dxe:ASPxComboBox ID="cboGCClassRL" runat="server" Width="200px" /></td>
        </tr>
        <tr>
            <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Service Markup (%)")%></label></td>
            <td><asp:TextBox ID="txtMarginPercentage1" Width="150px" CssClass="number" runat="server" /></td>
        </tr>
        <tr>
            <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Drugs and Medical Supplies Markup (%)")%></label></td>
            <td><asp:TextBox ID="txtMarginPercentage2" Width="150px" CssClass="number" runat="server" /></td>
        </tr>
        <tr>
            <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Logistic Markup (%)")%></label></td>
            <td><asp:TextBox ID="txtMarginPercentage3" Width="150px" CssClass="number" runat="server" /></td>
        </tr>
        <tr>
            <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Minimum Deposit Amount")%></label></td>
            <td><asp:TextBox ID="txtDepositAmount" Width="200px" CssClass="number" runat="server" /></td>
        </tr>
        <tr>
            <td class="tdLabel"><label class="lblNormal"><%=GetLabel("InPatient Class")%></label></td>
            <td><asp:CheckBox ID="chkIsInPatientClass" runat="server" /></td>
        </tr>
    </table>
</asp:Content>
