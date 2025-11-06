<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PartografMonitoringEntryCtl1.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.PartografMonitoringEntryCtl1" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<script type="text/javascript" id="dxss_obstetricHistoryEntryctl">
    setDatePicker('<%=txtMeasurementDate.ClientID %>');
    $('#<%=txtMeasurementDate.ClientID %>').datepicker('option', 'maxDate', '0');

    function onAfterSaveAddRecordEntryPopup(result) {
        if (typeof onRefreshGridPartografDt1 == 'function')
            onRefreshGridPartografDt1();
    }
    function onAfterSaveEditRecordEntryPopup(result) {
        if (typeof onRefreshGridPartografDt1 == 'function')
            onRefreshGridPartografDt1();
    }
</script>
<div style="height: 450px; overflow-y: scroll;">
    <input type="hidden" runat="server" id="hdnID" value="" />
    <input type="hidden" runat="server" id="hdnFetusID" value="" />
    <input type="hidden" value="" id="hdnLMPDate" runat="server" />
    <table>
        <colgroup>
            <col style="width: 50%" />
        </colgroup>
        <tr>
            <td>
                <table style="width: 100%" id="tblEntryContent">
                    <colgroup>
                        <col style="width: 200px" />
                        <col style="width: 150px" />
                        <col />
                    </colgroup>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Janin Ke-")%></label>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtFetusNo" Width="60px" runat="server" ReadOnly="True" CssClass="number"/>
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblMandatory">
                                <%=GetLabel("Tanggal ")%>
                                -
                                <%=GetLabel("Jam Pemeriksaan")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtMeasurementDate" Width="120px" CssClass="datepicker" runat="server" />
                        </td>
                        <td>
                            <asp:TextBox ID="txtMeasurementTime" Width="80px" CssClass="time" runat="server"
                                Style="text-align: center" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Detak Jantung Janin (DJJ)")%></label>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtDJJ" Width="60px" runat="server" CssClass="number" /><%=GetLabel(" /menit")%>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel" valign="top">
                            <label class="lblNormal">
                                <%=GetLabel("Air Ketuban")%></label>
                        </td>
                        <td>
                            <dxe:ASPxComboBox runat="server" ID="cboAirKetuban" ClientInstanceName="cboAirKetuban"
                                Width="100%">
                            </dxe:ASPxComboBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel" valign="top">
                            <label class="lblNormal">
                                <%=GetLabel("Penyusupan")%></label>
                        </td>
                        <td>
                            <dxe:ASPxComboBox runat="server" ID="cboPenyusupan" ClientInstanceName="cboPenyusupan"
                                Width="100%">
                            </dxe:ASPxComboBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel" valign="top">
                            <label class="lblNormal">
                                <%=GetLabel("Pembukaan serviks")%></label>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtPembukaan" Width="60px" CssClass="number" runat="server"/> cm
                        </td>
                    </tr>
                        <tr>
                            <td class="tdLabel">
                                <%=GetLabel("Turunnya Kepala") %>
                            </td>
                            <td colspan="2">
                                <asp:RadioButtonList ID="rblTurunKepala" runat="server" RepeatDirection="Horizontal"
                                    RepeatLayout="Table">
                                    <asp:ListItem Text=" Ya" Value="1" />
                                    <asp:ListItem Text=" Tidak" Value="0" Selected="True" />
                                </asp:RadioButtonList>
                            </td>
                        </tr>
                    <tr>
                        <td class="tdLabel" valign="top">
                            <label class="lblNormal">
                                <%=GetLabel("Kontraksi tiap 10 menit")%></label>
                        </td>
                        <td>
                            <dxe:ASPxComboBox runat="server" ID="cboDurasiKontraksi" ClientInstanceName="cboDurasiKontraksi"
                                Width="100%">
                            </dxe:ASPxComboBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel" valign="top">
                            <label class="lblNormal">
                                <%=GetLabel("Frekuensi Kontraksi")%></label>
                        </td>
                        <td>
                            <dxe:ASPxComboBox runat="server" ID="cboFrekuensiKontraksi" ClientInstanceName="cboFrekuensiKontraksi"
                                Width="100%">
                            </dxe:ASPxComboBox>
                        </td>
                    </tr>   
                </table>
            </td>
        </tr>
    </table>
</div>
