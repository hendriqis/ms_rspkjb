<%@ Page Language="C#" MasterPageFile="~/Libs/MasterPage/MPEntry.master" AutoEventWireup="true" 
    CodeBehind="SignaEntry.aspx.cs" Inherits="QIS.Medinfras.Web.SystemSetup.Program.SignaEntry" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
    
<asp:Content ID="Content2" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div class="menuTitle"><%=HttpUtility.HtmlEncode(GetPageTitle())%></div>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <input type="hidden" id="hdnPageTitle" runat="server" />
    <input type="hidden" id="hdnID" runat="server" value="" />
    <%--<div class="pageTitle"><%=GetLabel("Aturan Pakai")%></div>--%>
    <table class="tblContentArea" style="width:100%">
        <colgroup>
            <col style="width:200px"/>
            <col />
        </colgroup>
        <tr>
            <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Kode Aturan Pakai")%></label></td>
            <td><asp:TextBox ID="txtSignaLabel" Width="150px" runat="server" /></td>
        </tr>
        <tr>
            <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Keterangan Aturan Pakai #1")%></label></td>
            <td><asp:TextBox ID="txtSignaName1" Width="550px" runat="server" /></td>
        </tr>
        <tr>
            <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Keterangan Aturan Pakai #2")%></label></td>
            <td><asp:TextBox ID="txtSignaName2" Width="550px" runat="server" /></td>
        </tr>
        <tr>
            <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Bentuk/Kemasan")%></label></td>
            <td><dxe:ASPxComboBox ID="cboGCDrugForm" Width="150px" runat="server" /></td>
        </tr>
        <tr>
            <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Frekuensi")%></label></td>
            <td>
                <table cellpadding="0" cellspacing="0">
                    <colgroup>
                        <col style="width:30px" />
                        <col style="width:20px" />
                        <col style="width:100px" />
                        <col style="width:60px" />
                        <col style="width:60px" />
                        <col style="width:5px" />
                        <col style="width:125px" />
                        <col style="width:5px" />
                        <col />
                    </colgroup>
                    <tr>
                        <td><asp:TextBox ID="txtFrequency" Width="100%" CssClass="number" runat="server" /></td>
                        <td>&nbsp X</td>
                        <td><dxe:ASPxComboBox ID="cboGCDosingFrequency" runat="server" Width="100px" /></td>
                        <td class="tdLabel"><label class="lblMandatory">&nbsp<%=GetLabel("Dosis")%></label></td>
                        <td><asp:TextBox ID="txtDose" Width="100%" CssClass="number" runat="server" /></td>
                        <td>&nbsp</td>
                        <td><dxe:ASPxComboBox ID="cboGCDoseUnit" runat="server" Width="125px" /></td>
                        <td>&nbsp</td>
                        <td><dxe:ASPxComboBox runat="server" ID="cboCoenamRule" Width="150px" /></td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Label Dosis")%></label></td>
            <td><asp:TextBox ID="txtDoseInString" Width="100px" runat="server" /></td>
        </tr>
    </table>
</asp:Content>
