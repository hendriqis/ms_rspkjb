<%@ Page Language="C#" MasterPageFile="~/Libs/MasterPage/MPEntry.master" AutoEventWireup="true" 
    CodeBehind="VisitTypeEntry.aspx.cs" Inherits="QIS.Medinfras.Web.SystemSetup.Program.VisitTypeEntry" %>

<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <input type="hidden" id="hdnID" runat="server" value="" />
    <div class="pageTitle"><%=GetLabel("Visit Type")%></div>
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
                        <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Visit Type Code")%></label></td>
                        <td><asp:TextBox ID="txtVisitTypeCode" Width="300px" runat="server" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Visit Type Name")%></label></td>
                        <td><asp:TextBox ID="txtVisitTypeName" Width="300px" runat="server" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel"></td>
                        <td><asp:CheckBox ID="chkIsSickVisit" Width="200px" runat="server" /><%=GetLabel("Required Diagnosis Entry")%> </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>
