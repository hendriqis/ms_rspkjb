<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="viewMCUResultForm.aspx.cs"
    Inherits="QIS.Medinfras.Web.SystemSetup.Program.viewMCUResultForm" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxTimer" TagPrefix="dxt" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPopupControl" TagPrefix="dxpc" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallback" TagPrefix="dxcb" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>MEDINFRAS - Login Page</title>
</head>
<body>
    <form id="form1" runat="server">
    <script src='<%= ResolveUrl("~/Libs/Scripts/jquery/jquery-1.4.3.js")%>' type='text/javascript'></script>
    <script src='<%= ResolveUrl("~/Libs/Scripts/jquery/jquery-1.7.min.js")%>' type='text/javascript'></script>
    <script type="text/javascript">
        
    </script>
    <div>
       <%-- <table>
            <colgroup>
                <col style="width: 5%" />
                <col style="width: 35%" />
                <col style="width: 35%" />
                <col style="width: 25%" />
                <col />
            </colgroup>
            <tr>
                <td>
                    Table
                </td>
                <td>
                    <asp:DropDownList ID="cboTable" runat="server" Width="100%" />
                </td>
                <td>
                    <asp:TextBox ID="txtTableName" runat="server" Width="100%" />
                </td>
                <td>
                    <asp:Button ID="btnGenerate" runat="server" Text="Generate" OnClick="btnGenerate_Click" />
                </td>
            </tr>
            <tr>
                <td>
                    View
                </td>
                <td>
                    <asp:DropDownList ID="cboView" runat="server" Width="100%" />
                </td>
                <td>
                    <asp:TextBox ID="txtViewName" runat="server" Width="100%" />
                </td>
                <td>
                    <asp:Button ID="btnGenerateView" runat="server" Text="Generate" OnClick="btnGenerateView_Click" />
                </td>
            </tr>
            <tr style="display: none">
                <td>
                    Procedure
                </td>
                <td>
                    <asp:DropDownList ID="cboProc" runat="server" Width="100%" />
                </td>
                <td>
                    <asp:TextBox ID="txtProcName" runat="server" Width="100%" />
                </td>
                <td>
                    <asp:Button ID="btnGenerateProc" runat="server" Text="Generate" />
                </td>
            </tr>
        </table>--%>
        <table style="width: 100%">
            <colgroup>
                <col style="width: 50%" />
                <col style="width: 50%" />
            </colgroup>
            <tr>
                <td colspan=2>   <asp:Button ID="btnGenerate" runat="server" Text="Generate" OnClick="btnGenerate_Click" /></td>
            </tr>
            <tr>
                <td style="vertical-align: top">
                    <asp:TextBox ID="txtResult" runat="server" TextMode="MultiLine" Rows="30"
                        Width="100%" />
                </td>
                <td style="vertical-align: top">
                    <asp:TextBox ID="txtParsing" runat="server"   TextMode="MultiLine"
                        Rows="30" Width="100%" />
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
