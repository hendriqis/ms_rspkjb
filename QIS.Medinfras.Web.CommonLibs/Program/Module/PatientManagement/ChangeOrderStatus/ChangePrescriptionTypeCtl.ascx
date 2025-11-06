<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ChangePrescriptionTypeCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.ChangePrescriptionTypeCtl" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<script type="text/javascript" id="dxss_changePrescriptionTypeCtl">
</script>
<input type="hidden" id="hdnTransactionID" value="" runat="server" />
<input type="hidden" id="hdnRegistrationID" value="" runat="server" />
<input type="hidden" id="hdnVisitID" value="" runat="server" />
<input type="hidden" value="" id="hdnPrescriptionID" runat="server" />
<table class="tblContentArea">
    <colgroup>
        <col style="width: 100%" />
    </colgroup>
    <tr>
        <td>
            <table class="tblEntryContent" cellpadding="0" cellspacing="1">
                <tr>
                    <td class="tdLabel">
                        <label class="lblNormal" id="Label3">
                            <%=GetLabel("Nomor Transaksi")%></label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtTransactionNo" Width="100%" ReadOnly="true" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td class="tdLabel">
                        <label class="lblNormal" id="Label1">
                            <%=GetLabel("Jenis Resep Sekarang")%></label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtPrescriptionType" Width="100%" ReadOnly="true" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td class="tdLabel">
                        <label class="lblNormal" id="Label2">
                            <%=GetLabel("Jenis Resep Baru")%></label>
                    </td>
                    <td>
                        <dxe:ASPxComboBox ID="cboPrescriptionType" ClientInstanceName="cboPrescriptionType"
                            runat="server" Width="235px">
                        </dxe:ASPxComboBox>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
</table>
