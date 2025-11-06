<%@ Page Language="C#" MasterPageFile="~/Libs/MasterPage/MPEntry.master" AutoEventWireup="true"
    CodeBehind="ProductLineEntry.aspx.cs" Inherits="QIS.Medinfras.Web.CommonLibs.Program.ProductLineEntry" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<asp:Content ID="Content2" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div class="menuTitle">
        <%=HttpUtility.HtmlEncode(GetPageTitle())%></div>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <input type="hidden" id="hdnPageTitle" runat="server" />
    <input type="hidden" id="hdnID" runat="server" value="" />
    <table class="tblContentArea">
        <tr>
            <td style="padding: 5px; vertical-align: top">
                <table class="tblEntryContent" style="width: 80%">
                    <colgroup>
                        <col style="width: 150px" />
                        <col />
                    </colgroup>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblMandatory">
                                <%=GetLabel("Kode Product Line")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtProductLineCode" Width="100px" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblMandatory">
                                <%=GetLabel("Nama Product Line")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtProductLineName" Width="500px" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblMandatory">
                                <%=GetLabel("Tipe Item")%></label>
                        </td>
                        <td>
                            <dxe:ASPxComboBox ID="cboGCItemType" Width="250px" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Persediaan/Stock")%></label>
                        </td>
                        <td>
                            <asp:CheckBox ID="chkIsInventoryItem" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Fixed Asset")%></label>
                        </td>
                        <td>
                            <asp:CheckBox ID="chkIsFixedAsset" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Item Konsinyasi")%></label>
                        </td>
                        <td>
                            <asp:CheckBox ID="chkIsConsigmentItem" runat="server" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>
