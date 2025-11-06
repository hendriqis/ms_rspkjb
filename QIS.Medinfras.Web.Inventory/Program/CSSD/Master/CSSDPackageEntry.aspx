<%@ Page Language="C#" MasterPageFile="~/Libs/MasterPage/MPEntry.master" AutoEventWireup="true"
    CodeBehind="CSSDPackageEntry.aspx.cs" Inherits="QIS.Medinfras.Web.Inventory.Program.CSSDPackageEntry" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<asp:Content ID="Content2" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div class="menuTitle">
        <%=HttpUtility.HtmlEncode(GetPageTitle())%></div>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript">
        function onLoad() {
        }
    </script>
    <input type="hidden" id="hdnPageTitle" runat="server" />
    <input type="hidden" id="hdnID" runat="server" value="" />
    <input type="hidden" id="hdnFileName" runat="server" value="" />
    <table class="tblContentArea">
        <tr>
            <td class="tdLabel" style="width:120px">
                <label class="lblMandatory">
                    <%=GetLabel("Package Code")%></label>
            </td>
            <td>
                <asp:TextBox ID="txtPackageCode" Width="150px" runat="server" />
            </td>
        </tr>
        <tr>
            <td class="tdLabel" style="width:120px">
                <label class="lblMandatory">
                    <%=GetLabel("Package Name")%></label>
            </td>
            <td>
                <asp:TextBox ID="txtPackageName" Width="500px" runat="server" />
            </td>
        </tr>
        <tr>
            <td class="tdLabel" style="width:120px">
                <label class="lblNormal">
                    <%=GetLabel("Remarks")%></label>
            </td>
            <td>
                <asp:TextBox ID="txtRemarks" Width="500px" TextMode="MultiLine" runat="server" />
            </td>
        </tr>
    </table>
</asp:Content>
