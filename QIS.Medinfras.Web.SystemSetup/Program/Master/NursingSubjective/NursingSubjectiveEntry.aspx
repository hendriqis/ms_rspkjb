<%@ Page Language="C#" MasterPageFile="~/Libs/MasterPage/MPEntry.master" AutoEventWireup="true" 
    CodeBehind="NursingSubjectiveEntry.aspx.cs" Inherits="QIS.Medinfras.Web.SystemSetup.Program.NursingSubjectiveEntry" %>

<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <input type="hidden" id="hdnID" runat="server" value="" />
    <div class="pageTitle"><%=GetLabel("Nursing Subjective")%></div>
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
                        <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Subjective Code")%></label></td>
                        <td><asp:TextBox ID="txtSubjectiveCode" Width="300px" runat="server" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel" style="vertical-align:top"><label class="lblMandatory"><%=GetLabel("Subjective Text")%></label></td>
                        <td><asp:TextBox ID="txtSubjectiveText" Width="300px" Height="50px" TextMode="MultiLine" runat="server" /></td>
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
