<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="FetalEntryCtl1.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.FetalEntryCtl1" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<script type="text/javascript" id="dxss_antenatalFormEntryctl">
    $(function () {
     });
</script>
<div style="height: auto">
    <input type="hidden" runat="server" id="hdnID" value="" />
    <input type="hidden" runat="server" id="hdnAntenatalRecordID" value="" />
    <table class="tblEntryContent">
        <colgroup>
            <col />
        </colgroup>
        <tr>
            <td style="vertical-align: top;">
                <div>
                    <table class="tblEntryContent" style="width: 100%">
                        <colgroup>
                            <col style="width: 180px" />
                            <col />
                        </colgroup>
                        <tr>
                            <td class="tdLabel" valign="top">
                                <label class="lblNormal">
                                    <%=GetLabel("Janin Ke-")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtFetusNo" Width="60px" CssClass="number" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel" valign="top">
                                <label class="lblNormal">
                                    <%=GetLabel("Jenis Kelamin")%></label>
                            </td>
                            <td>
                                <dxe:ASPxComboBox runat="server" ID="cboGender" ClientInstanceName="cboGender" Width="250px">
                                </dxe:ASPxComboBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel" valign="top" style="padding-top: 5px;">
                                <%=GetLabel("Catatan Lainnya") %>
                            </td>
                            <td>
                                <asp:TextBox ID="txtRemarks" runat="server" Width="100%" TextMode="Multiline"
                                    Rows="5" />
                            </td>
                        </tr>
                    </table>
                </div>
            </td>
        </tr>
    </table>
</div>
