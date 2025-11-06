<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PivotOptionCtl.ascx.cs" 
    Inherits="QIS.Medinfras.Web.MedicalRecord.Program.PivotOptionCtl" %>

<%@ Register assembly="DevExpress.Web.ASPxPivotGrid.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" 
    namespace="DevExpress.Web.ASPxPivotGrid" tagprefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register assembly="DevExpress.Web.ASPxPivotGrid.v11.1.Export, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" 
    namespace="DevExpress.Web.ASPxPivotGrid.Export" tagprefix="dx" %>

<table cellpadding="0" cellspacing="0" width="100%">
    <tr>
        <td>
            <table>
                <tr>
                    <td>
                        <strong><%=GetLabel("Export to")%>:</strong>
                    </td>
                    <td>
                        <dxe:ASPxComboBox ID="cboListExportFormat" runat="server" Style="vertical-align: middle"
                            SelectedIndex="0" ValueType="System.String" Width="61px">
                            <Items>
                                <dxe:ListEditItem Text="Pdf" Value="0" />
                                <dxe:ListEditItem Text="Excel" Value="1" />
                            </Items>
                        </dxe:ASPxComboBox>
                    </td>
                    <td>
                        <asp:Button ID="btnSavePivot" OnClick="btnSavePivot_Click" runat="server" Text="Save" />
                    </td>
                </tr>
            </table>
            <table border="0" cellpadding="3" cellspacing="0">
                <tr>
                    <td rowspan="5" valign="top" style="width: 106px">
                        <strong><%=GetLabel("Export Options")%>: </strong>
                    </td>
                    <td style="width: 350px">
                        <asp:CheckBox ID="chkPrintHeadersOnEveryPage" runat="server" Text="Print headers on every page" /><br />
                        <asp:CheckBox ID="chkPrintFilterHeaders" runat="server" Text="Print filter headers" Checked="True" /><br />
                        <asp:CheckBox ID="chkPrintColumnHeaders" runat="server" Text="Print column headers" Checked="True" /><br />
                        <asp:CheckBox ID="chkPrintRowHeaders" runat="server" Text="Print row headers" Checked="True" /><br />
                        <asp:CheckBox ID="checkPrintDataHeaders" runat="server" Text="Print data headers" Checked="True" />
                    </td>
                </tr>
            </table>
        </td>
    </tr>
</table>
<dx:ASPxPivotGridExporter ID="ASPxPivotGridExporter1" runat="server" ASPxPivotGridID="pvView"
    Visible="False" />
<input type="hidden" id="hdnPivotSaveName" runat="server" />