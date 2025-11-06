<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PatientBirthCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.PatientBirthCtl" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<script type="text/javascript" id="dxss_patientbirthctl">
</script>
<div style="height: 445px; overflow-y: scroll;">
    <input type="hidden" id="hdnBirthRecordID" value="0" runat="server" />
    <input type="hidden" id="hdnDepartmentID" value="0" runat="server" />
    <table class="tblEntryContent">
        <colgroup>
            <col style="width: 50%" />
        </colgroup>
        <tr>
            <td style="padding: 5px; vertical-align: top;">
                <h4 class="h4expanded">
                    <%=GetLabel("Data Bayi")%></h4>
                <div class="containerTblEntryContent">
                    <input type="hidden" id="hdnVisitID" value="0" runat="server" />
                    <input type="hidden" id="hdnBabyMRN" value="0" runat="server" />
                    <input type="hidden" id="hdnVisitIDBayi" value="0" runat="server" />
                    <table class="tblEntryContent" style="width: 100%">
                        <colgroup>
                            <col style="width: 30%" />
                        </colgroup>
                        <tr>
                            <td class="tdLabel">
                                <b>
                                    <label class="lblNormal">
                                        <%=GetLabel("DATA KELAHIRAN BAYI")%></label></b>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblMandatory">
                                    <%=GetLabel("Anak ke")%></label>
                            </td>
                            <td>
                                <table style="width: 100%" cellpadding="0" cellspacing="0">
                                    <colgroup>
                                        <col style="width: 30%" />
                                        <col style="width: 20%" />
                                        <col style="width: 30%" />
                                    </colgroup>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtChildNo" TabIndex="3" Width="80%" runat="server" CssClass="number" />
                                        </td>
                                        <td class="tdLabel">
                                            <label class="lblMandatory">
                                                <%=GetLabel("Jam lahir")%></label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtTimeOfBirth" TabIndex="3" Width="80px" CssClass="time" runat="server"/>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblMandatory">
                                    <%=GetLabel("Tempat Lahir")%></label>
                            </td>
                            <td>
                                <dxe:ASPxComboBox ID="cboBornAt" Width="100%" runat="server" TabIndex="4" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblMandatory">
                                    <%=GetLabel("Usia Kehamilan")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtBirthPregnancyAge" Width="80px" runat="server" CssClass="number"
                                    TabIndex="5" />
                                <label>
                                    <%=GetLabel("Minggu")%>
                                </label>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("APGAR Score 1")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtAPGARScore1" Width="100%" CssClass="number" runat="server" TabIndex="6" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("APGAR Score 2")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtAPGARScore2" Width="100%" CssClass="number" runat="server" TabIndex="7" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("APGAR Score 3")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtAPGARScore3" Width="100%" CssClass="number" runat="server" TabIndex="8" />
                            </td>
                        </tr>
                    </table>
                </div>
                <h4 class="h4expanded">
                    <%=GetLabel("Data Kuantitatif")%></h4>
                <div class="containerTblEntryContent">
                    <table class="tblEntryContent" style="width: 100%">
                        <colgroup>
                            <col style="width: 30%" />
                            <col />
                        </colgroup>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblMandatory">
                                    <%=GetLabel("Panjang")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtLength" Width="80px" runat="server" TabIndex="11" CssClass="number" />
                                <label>
                                    <%=GetLabel("cm")%></label>
                            </td>
                            <td class="tdLabel">
                                <label class="lblMandatory">
                                    <%=GetLabel("Berat")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtWeight" Width="80px" runat="server" TabIndex="12" CssClass="number" />
                                <label>
                                    <%=GetLabel("Gram")%></label>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Lingkar Kepala")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtHeadCircumference" Width="80px" runat="server" TabIndex="13"
                                    CssClass="number" />
                                <label>
                                    <%=GetLabel("cm")%></label>
                            </td>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Lingkar Dada")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtChestCircumference" Width="80px" runat="server" TabIndex="14"
                                    CssClass="number" />
                                <label>
                                    <%=GetLabel("cm")%></label>
                            </td>
                        </tr>
                    </table>
                </div>
                <h4 class="h4expanded">
                    <%=GetLabel("Data Kualitatif")%></h4>
                <div class="containerTblEntryContent">
                    <table class="tblEntryContent" style="width: 100%">
                        <colgroup>
                            <col style="width: 30%" />
                        </colgroup>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Cara Caesar")%></label>
                            </td>
                            <td>
                                <dxe:ASPxComboBox ID="cboCaesarMethod" Width="100%" runat="server" TabIndex="15" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblMandatory">
                                    <%=GetLabel("Kembar / Tunggal")%></label>
                            </td>
                            <td>
                                <dxe:ASPxComboBox ID="cboTwinSingle" Width="100%" runat="server" TabIndex="16" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblMandatory">
                                    <%=GetLabel("Kondisi Kelahiran")%></label>
                            </td>
                            <td>
                                <dxe:ASPxComboBox ID="cboBornCondition" Width="100%" runat="server" TabIndex="17" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblMandatory">
                                    <%=GetLabel("Cara Melahirkan")%></label>
                            </td>
                            <td>
                                <dxe:ASPxComboBox ID="cboBirthMethod" Width="100%" runat="server" TabIndex="18" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblMandatory">
                                    <%=GetLabel("Komplikasi Kelahiran")%></label>
                            </td>
                            <td>
                                <dxe:ASPxComboBox ID="cboBirthComplication" Width="100%" runat="server" TabIndex="19" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Sebab Kematian")%></label>
                            </td>
                            <td>
                                <dxe:ASPxComboBox ID="cboBirthCOD" Width="100%" runat="server" TabIndex="20" />
                            </td>
                        </tr>
                    </table>
                </div>
            </td>
        </tr>
    </table>
</div>
