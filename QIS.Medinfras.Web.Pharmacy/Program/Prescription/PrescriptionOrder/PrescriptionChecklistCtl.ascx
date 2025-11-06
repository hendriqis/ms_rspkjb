<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PrescriptionChecklistCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.Pharmacy.Program.PrescriptionChecklistCtl" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<script type="text/javascript" id="dxss_PrescriptionChecklistCtl">

</script>
<input type="hidden" id="hdnTransactionID" value="" runat="server" />
<input type="hidden" value="" id="hdnOrderID" runat="server" />
<input type="hidden" value="" id="hdnIsReviewPrescriptionMandatoryForProposedTransactionCtl"
    runat="server" />
<table class="tblContentArea">
    <colgroup>
        <col style="width: 100%" />
    </colgroup>
    <tr>
        <td>
            <fieldset id="fsEntryPopup" style="margin: 0">
                <table class="tblEntryContent" cellpadding="0" cellspacing="1">
                    <colgroup>
                        <col style="width: 150px" />
                        <col style="width: 150px" />
                        <col style="width: 5px" />
                        <col style="width: 150px" />
                        <col style="width: 120px" />
                    </colgroup>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal" id="lblTransactionNo">
                                <%=GetLabel("No. Order")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtTransactionNo" Width="150px" ReadOnly="true" runat="server" />
                        </td>
                        <td />
                        <td class="tdLabel">
                            <label class="lblNormal" id="lblTransactionDate">
                                <%=GetLabel("Tanggal ")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtTransactionDate" Width="120px" ReadOnly="true" runat="server"
                                CssClass="datepicker" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal" id="Label1">
                                <%=GetLabel("No. RM")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtMedicalNo" Width="150px" ReadOnly="true" runat="server" />
                        </td>
                        <td />
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal" id="Label2">
                                <%=GetLabel("Pasien")%></label>
                        </td>
                        <td colspan="4">
                            <asp:TextBox ID="txtPatientName" Width="100%" ReadOnly="true" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="5">
                            <hr style="padding: 0 0 0 0; margin: 0 0 0 0;" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="5">
                            <table>
                                <colgroup>
                                    <col style="width: 50%" />
                                    <col style="width: 50%" />
                                </colgroup>
                                <tr>
                                    <td valign="top">
                                        <table>
                                            <colgroup>
                                                <col style="width: 30px" />
                                            </colgroup>
                                            <tr>
                                                <td>
                                                    <asp:CheckBox ID="chkIsCorrectPatient" runat="server" ToolTip="Benar Pasien (Px)" />
                                                </td>
                                                <td style="font-weight: bold;">
                                                    <asp:Label ID="lblIsCorrectPatient" runat="server" ToolTip="Benar Pasien (Px)" Text="Px">Benar Pasien (Px)</asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:CheckBox ID="chkIsCorrectMedication" runat="server" ToolTip="Benar Obat (OB)" />
                                                </td>
                                                <td style="font-weight: bold;">
                                                    <asp:Label ID="lblIsCorrectMedication" runat="server" ToolTip="Benar Obat (OB)" Text="OB">Benar Obat (OB)</asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:CheckBox ID="chkIsCorrectStrength" runat="server" ToolTip="Benar Kekuatan Obat (KE)" />
                                                </td>
                                                <td style="font-weight: bold;">
                                                    <asp:Label ID="lblIsCorrectStrength" runat="server" ToolTip="Benar Kekuatan Obat (KE)"
                                                        Text="KE">Benar Kekuatan Obat (KE)</asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:CheckBox ID="chkIsCorrectFrequency" runat="server" ToolTip="Benar Frekuensi Obat (FRE)" />
                                                </td>
                                                <td style="font-weight: bold;">
                                                    <asp:Label ID="lblIsCorrectFrequency" runat="server" ToolTip="Benar Frekuensi Obat (FRE)"
                                                        Text="FRE">Benar Frekuensi Obat (FRE)</asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:CheckBox ID="chkIsCorrectDosage" runat="server" ToolTip="Benar Dosis (DO)" />
                                                </td>
                                                <td style="font-weight: bold;">
                                                    <asp:Label ID="lblIsCorrectDosage" runat="server" ToolTip="Benar Dosis (DO)" Text="DO">Benar Dosis (DO)</asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:CheckBox ID="chkIsCorrectRoute" runat="server" ToolTip="Benar Rute Pemberian (RP)" />
                                                </td>
                                                <td style="font-weight: bold;">
                                                    <asp:Label ID="lblIsCorrectRoute" runat="server" ToolTip="Benar Rute Pemberian (RP)"
                                                        Text="RP">Benar Rute Pemberian (RP)</asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:CheckBox ID="chkIsHasDrugInteraction" runat="server" ToolTip="Ada tidaknya interaksi obat (IO)" />
                                                </td>
                                                <td style="font-weight: bold;">
                                                    <asp:Label ID="lblIsHasDrugInteraction" runat="server" ToolTip="Ada tidaknya interaksi obat (IO)"
                                                        Text="IO">Ada tidaknya interaksi obat (IO)</asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:CheckBox ID="chkIsHasDuplication" runat="server" ToolTip="Ada tidaknya duplikasi obat (DUP)" />
                                                </td>
                                                <td style="font-weight: bold;">
                                                    <asp:Label ID="lblIsHasDuplication" runat="server" ToolTip="Ada tidaknya duplikasi obat (DUP)"
                                                        Text="DUP">Ada tidaknya duplikasi obat (DUP)</asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:CheckBox ID="chkIsCorrectTimeOfGiving" runat="server" ToolTip="Benar Waktu Pemberian (WP)" />
                                                </td>
                                                <td style="font-weight: bold;">
                                                    <asp:Label ID="lblIsCorrectTimeOfGiving" runat="server" ToolTip="Benar Waktu Pemberian (WP)"
                                                        Text="WP">Benar Waktu Pemberian (WP)</asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:CheckBox ID="chkIsADChecked" runat="server" ToolTip="(AD)" />
                                                </td>
                                                <td style="font-weight: bold;">
                                                    <asp:Label ID="lblIsADChecked" runat="server" ToolTip="(AD)" Text="AD">(AD)</asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:CheckBox ID="chkIsFARChecked" runat="server" ToolTip="(FAR)" />
                                                </td>
                                                <td style="font-weight: bold;">
                                                    <asp:Label ID="lblIsFARChecked" runat="server" ToolTip="(FAR)" Text="FAR">(FAR)</asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:CheckBox ID="chkIsKLNChecked" runat="server" ToolTip="(KLN)" />
                                                </td>
                                                <td style="font-weight: bold;">
                                                    <asp:Label ID="lblIsKLNChecked" runat="server" ToolTip="(KLN)" Text="KLN">(KLN)</asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td valign="top">
                                        <table>
                                            <colgroup>
                                                <col style="width: 150px" />
                                                <col style="width: 350px" />
                                                <col />
                                            </colgroup>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblPrescriptionReviewText" runat="server" ToolTip="Telaah Resep" Text="Telaah Resep (FreeText)" />
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtPrescriptionReviewText" runat="server" TextMode="MultiLine" Height="100px"
                                                        Width="100%" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </fieldset>
        </td>
    </tr>
</table>
