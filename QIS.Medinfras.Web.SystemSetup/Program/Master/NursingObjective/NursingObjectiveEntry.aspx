<%@ Page Language="C#" MasterPageFile="~/Libs/MasterPage/MPEntry.master" AutoEventWireup="true" 
    CodeBehind="NursingObjectiveEntry.aspx.cs" Inherits="QIS.Medinfras.Web.SystemSetup.Program.NursingObjectiveEntry" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>

<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <input type="hidden" id="hdnID" runat="server" value="" />
    <div class="pageTitle"><%=GetLabel("Nursing Objective")%></div>
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
                        <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Objective Code")%></label></td>
                        <td><asp:TextBox ID="txtObjectiveCode" Width="300px" runat="server" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel" style="vertical-align:top"><label class="lblMandatory"><%=GetLabel("Objective Text")%></label></td>
                        <td><asp:TextBox ID="txtObjectiveText" Width="300px" Height="50px" TextMode="MultiLine" runat="server" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Objective Data Source")%></label> </td>
                        <td><dxe:ASPxComboBox ID="cboGCObjectiveDataSource" runat="server" Width="200px" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel"></td>
                        <td><asp:CheckBox ID="chkIsEditableByUser" Text="Editable by User" runat="server" /></td>
                    </tr>

                </table>
            </td>
        </tr>
    </table>
</asp:Content>
