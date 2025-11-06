<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="UpdateOrderStatusCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.Pharmacy.Program.UpdateOrderStatusCtl" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<style type="text/css">
    .highlight
    {
        background-color: #FE5D15;
        color: White;
    }
</style>
<script type="text/javascript" id="dxss_UpdateOrderStatusCtl">
    $(function () {
    });

    function onBeforeProcess(param) {
        return true;
    }

    function onGetEntryPopupReturnValue() {
        var result = $('#<%=hdnSelectedID.ClientID %>').val();
        return result;
    }

    function onCbpViewEndCallback(s) {
        hideLoadingPanel();
    }
</script>
<input type="hidden" id="hdnPrescriptionOrderID" runat="server" />
<input type="hidden" id="hdnSelectedID" runat="server" />
<div style="height: 440px; overflow-y: auto">
    <table class="tblContentArea">
        <tr>
            <td style="padding: 5px; vertical-align: top">
                <table>
                    <colgroup>
                        <col width="150px" />
                        <col width="150px" />
                        <col width="150px" />
                        <col />
                    </colgroup>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("No. Order")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtTransactionNoCtl" runat="server" Width="99%" ReadOnly="true" />
                        </td>
                        <td class="tdLabel">
                            <label class="lblMandatory">
                                <%=GetLabel("Jenis Resep") %></label>
                        </td>
                        <td>
                            <dxe:ASPxComboBox ID="cboPrescriptionTypeCtl" ClientInstanceName="cboPrescriptionTypeCtl"
                                runat="server" Width="235px" Enabled="false">
                            </dxe:ASPxComboBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Tanggal/Jam Order")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtOrderDateTime" runat="server" Width="99%" ReadOnly="true" />
                        </td>
                        <td class="tdLabel">
                            <label class="lblMandatory" title="Mempengaruhi apakah dokter bisa melakukan reopen order atau tidak.">
                                <%=GetLabel("Status Transaksi Order") %></label>
                        </td>
                        <td>
                            <dxe:ASPxComboBox ID="cboTransactionStatusCtl" ClientInstanceName="cboTransactionStatusCtl"
                                runat="server" Width="235px">
                            </dxe:ASPxComboBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4">
                            <table border="0" cellpadding="1" cellspacing="0">
                                <colgroup>
                                    <col style="width: 30px" />
                                    <col style="width: 40px" />
                                    <col style="width: 30px" />
                                    <col style="width: 40px" />
                                    <col style="width: 30px" />
                                    <col style="width: 40px" />
                                    <col style="width: 30px" />
                                    <col style="width: 40px" />
                                    <col style="width: 30px" />
                                    <col style="width: 40px" />
                                    <col style="width: 30px" />
                                    <col style="width: 40px" />
                                    <col style="width: 30px" />
                                    <col style="width: 40px" />
                                    <col style="width: 30px" />
                                    <col style="width: 40px" />
                                    <col style="width: 30px" />
                                    <col style="width: 40px" />
                                    <col style="width: 30px" />
                                    <col style="width: 40px" />
                                    <col style="width: 30px" />
                                    <col style="width: 40px" />
                                    <col />
                                </colgroup>
                                <tr>
                                    <td>
                                        <asp:CheckBox ID="chkIsCorrectPatientCtl" runat="server" ToolTip="Benar Pasien" />
                                    </td>
                                    <td style="font-weight: bold;">
                                        <asp:Label ID="Label1" runat="server" ToolTip="Benar Pasien" Text="Px">Px</asp:Label>
                                    </td>
                                    <td>
                                        <asp:CheckBox ID="chkIsCorrectMedicationCtl" runat="server" ToolTip="Benar Obat" />
                                    </td>
                                    <td style="font-weight: bold;">
                                        <asp:Label ID="Label2" runat="server" ToolTip="Benar Obat" Text="OB">OB</asp:Label>
                                    </td>
                                    <td>
                                        <asp:CheckBox ID="chkIsCorrectStrengthCtl" runat="server" ToolTip="Benar Kekuatan Obat" />
                                    </td>
                                    <td style="font-weight: bold;">
                                        <asp:Label ID="Label3" runat="server" ToolTip="Benar Kekuatan Obat" Text="KE">KE</asp:Label>
                                    </td>
                                    <td>
                                        <asp:CheckBox ID="chkIsCorrectFrequencyCtl" runat="server" ToolTip="Benar Frekuensi Pemberian" />
                                    </td>
                                    <td style="font-weight: bold;">
                                        <asp:Label ID="Label4" runat="server" ToolTip="Benar Frekuensi Pemberian" Text="FRE">FRE</asp:Label>
                                    </td>
                                    <td>
                                        <asp:CheckBox ID="chkIsCorrectDosageCtl" runat="server" ToolTip="Benar Dosis" />
                                    </td>
                                    <td style="font-weight: bold;">
                                        <asp:Label ID="Label5" runat="server" ToolTip="Benar Dosis" Text="DO">DO</asp:Label>
                                    </td>
                                    <td>
                                        <asp:CheckBox ID="chkIsCorrectRouteCtl" runat="server" ToolTip="Benar Rute Pemberian" />
                                    </td>
                                    <td style="font-weight: bold;">
                                        <asp:Label ID="Label6" runat="server" ToolTip="Benar Rute Pemberian" Text="RP">RP</asp:Label>
                                    </td>
                                    <td>
                                        <asp:CheckBox ID="chkIsHasDrugInteractionCtl" runat="server" ToolTip="ada tidaknya interaksi obat" />
                                    </td>
                                    <td style="font-weight: bold;">
                                        <asp:Label ID="Label7" runat="server" ToolTip="ada tidaknya interaksi obat" Text="IO">IO</asp:Label>
                                    </td>
                                    <td>
                                        <asp:CheckBox ID="chkIsHasDuplicationCtl" runat="server" ToolTip="ada tidaknya duplikasi obat" />
                                    </td>
                                    <td style="font-weight: bold;">
                                        <asp:Label ID="Label8" runat="server" ToolTip="ada tidaknya duplikasi obat" Text="IO">DUP</asp:Label>
                                    </td>
                                    <td>
                                        <asp:CheckBox ID="chkIsADCheckedCtl" runat="server" ToolTip="(AD)" />
                                    </td>
                                    <td style="font-weight: bold;">
                                        <asp:Label ID="Label9" runat="server" ToolTip="(AD)" Text="AD">AD</asp:Label>
                                    </td>
                                    <td>
                                        <asp:CheckBox ID="chkIsFARCheckedCtl" runat="server" ToolTip="(FAR)" />
                                    </td>
                                    <td style="font-weight: bold;">
                                        <asp:Label ID="Label10" runat="server" ToolTip="(FAR)" Text="FAR">FAR</asp:Label>
                                    </td>
                                    <td>
                                        <asp:CheckBox ID="chkIsKLNCheckedCtl" runat="server" ToolTip="(KLN)" />
                                    </td>
                                    <td style="font-weight: bold;">
                                        <asp:Label ID="Label11" runat="server" ToolTip="(KLN)" Text="FAR">KLN</asp:Label>
                                    </td>
                                    <td />
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</div>
