<%@ Page Title="" Language="C#" MasterPageFile="~/libs/MasterPage/MPEntry.master" AutoEventWireup="true" CodeBehind="CustomerGroupEntry.aspx.cs" Inherits="QIS.Medinfras.Web.Finance.Program.CustomerGroupEntry" %>
<asp:Content ID="Content2" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div class="menuTitle"><%=HttpUtility.HtmlEncode(GetPageTitle())%></div>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <input type="hidden" id="hdnPageTitle" runat="server" />
    <input type="hidden" id="hdnID" runat="server" value="" />    
    <table class="tblContentArea">
        <colgroup>
            <col style="width:50%"/>
        </colgroup>
        <tr>
            <td style="padding:5px;vertical-align:top">
                <table class="tblEntryContent" style="width:50%">
                    <colgroup>
                        <col style="width:25%"/>
                    </colgroup>
                    <tr>
                        <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Customer Group Code")%></label></td>
                        <td><asp:TextBox ID="txtCustomerGroupCode" Width="150px" runat="server" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Customer Group Name")%></label></td>
                        <td><asp:TextBox ID="txtCustomerGroupName" Width="300px" runat="server" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Remarks")%></label></td>
                        <td><asp:TextBox ID="txtRemarks" Width="300px" runat="server" TextMode="MultiLine" /></td>
                    </tr>                   
                </table>
            </td>
        </tr>
    </table>
</asp:Content>