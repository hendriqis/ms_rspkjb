<%@ Page Language="C#" MasterPageFile="~/Libs/MasterPage/MPEntry.master" AutoEventWireup="true" 
    CodeBehind="NurseProblemEntry.aspx.cs" Inherits="QIS.Medinfras.Web.SystemSetup.Program.NurseProblemEntry" %>

<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <input type="hidden" id="hdnID" runat="server" value="" />
    <div class="pageTitle"><%=GetLabel("Nurse Problem")%></div>
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
                        <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Nurse Problem Code")%></label></td>
                        <td><asp:TextBox ID="txtNurseProblemCode" Width="300px" runat="server" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Nurse Problem Name")%></label></td>
                        <td><asp:TextBox ID="txtNursingProblemName" Width="300px" runat="server" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel" style="vertical-align:top"><label><%=GetLabel("Remarks")%></label></td>
                        <td><asp:TextBox ID="txtRemarks" Width="300px" Height="50px" TextMode="MultiLine" runat="server" /></td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>
