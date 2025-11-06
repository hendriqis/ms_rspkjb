<%@ Page Language="C#" MasterPageFile="~/Libs/MasterPage/MPEntry.master" AutoEventWireup="true" 
    CodeBehind="BodyDiagramEntry.aspx.cs" Inherits="QIS.Medinfras.Web.SystemSetup.Program.BodyDiagramEntry" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>

<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <input type="hidden" id="hdnID" runat="server" value="" />
    <input type="hidden" id="hdnHealthcareID" runat="server" value="" />
    <div class="pageTitle"><%=GetLabel("Body Diagram")%></div>
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
                        <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Diagram Code")%></label></td>
                        <td><asp:TextBox ID="txtDiagramCode" Width="300px" runat="server" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Diagram Name")%></label></td>
                        <td><asp:TextBox ID="txtDiagramName" Width="300px" runat="server" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Diagram Group")%></label></td>
                        <td><dxe:ASPxComboBox runat="server" ID="cboGCBodyDiagramGroup" Width="300px" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Image URL")%></label></td>
                        <td><asp:TextBox ID="txtImageURL" Width="300px" runat="server" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label><%=GetLabel("Remarks")%></label></td>
                        <td><asp:TextBox ID="txtRemarks" Width="300px" runat="server" Height="70px" MaxLength="500" TextMode="MultiLine" /></td>
                    </tr>

                </table>
            </td>
        </tr>
    </table>
</asp:Content>
