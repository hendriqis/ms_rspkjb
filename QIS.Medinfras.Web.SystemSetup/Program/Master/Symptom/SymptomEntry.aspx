<%@ Page Language="C#" MasterPageFile="~/Libs/MasterPage/MPEntry.master" AutoEventWireup="true" 
    CodeBehind="SymptomEntry.aspx.cs" Inherits="QIS.Medinfras.Web.SystemSetup.Program.SymptomEntry" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>

<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <input type="hidden" id="hdnID" runat="server" value="" />
    <div class="pageTitle"><%=GetLabel("Symptom")%></div>
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
                        <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Symptom Code")%></label></td>
                        <td><asp:TextBox ID="txtSymptomCode" Width="300px" runat="server" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Symptom Name")%></label></td>
                        <td><asp:TextBox ID="txtSymptomName" Width="300px" runat="server" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Review Of Systems")%></label></td>
                        <td><dxe:ASPxComboBox runat="server" ID="cboROSystem" Width="300px" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Gender")%></label></td>
                        <td>
                            <asp:RadioButtonList ID="rboGender" runat="server" RepeatDirection="Horizontal">
                                <asp:ListItem Text="Male" Value="M"/>
                                <asp:ListItem Text="Female" Value="F"/>
                                <asp:ListItem Text="Both" Value="B"/>
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>
