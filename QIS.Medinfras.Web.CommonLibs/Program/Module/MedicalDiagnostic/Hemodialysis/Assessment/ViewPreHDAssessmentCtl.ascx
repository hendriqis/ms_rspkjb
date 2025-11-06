<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ViewPreHDAssessmentCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.ViewPreHDAssessmentCtl" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>

<script type="text/javascript" id="dxss_ViewSurgerySignInCtl">
    $(function () {
    });
</script>

<div style="height: auto">
    <input type="hidden" runat="server" id="hdnTestOrderID" value="" />
    <input type="hidden" runat="server" id="hdnID" value="" />
    <input type="hidden" runat="server" id="hdnDivHTML" value="" />
    <input type="hidden" runat="server" id="hdnFormValues" value="" />

    <table class="tblEntryContent" style="width: 100%">
        <colgroup>
            <col style="width: 160px" />
            <col style="width: 120px" />
            <col style="width: 120px" />
            <col />
        </colgroup>
        <tr>
            <td class="tdLabel">
                <label class="lblNormal">
                    <%=GetLabel("Tanggal dan Waktu")%></label>
            </td>
            <td colspan="3">
                <table border="0" cellpadding="0" cellspacing="0">
                    <tr>
                        <td>
                            <asp:TextBox ID="txtAsessmentDate" Width="120px" runat="server" ReadOnly="true" />
                        </td>
                        <td>
                            <asp:TextBox ID="txtAsessmentTime" Width="80px" runat="server" Style="text-align: center" ReadOnly="true" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr id="trServiceUnit" runat="server" style="display:none">
            <td class="tdLabel">
                <label class="lblNormal">
                    <%=GetLabel("Penunjang Medis")%></label>
            </td>
            <td colspan="3">
                <asp:TextBox ID="txtServiceUnitName" Width="150px" runat="server" Style="text-align: center" ReadOnly="true" />
            </td>
        </tr>
        <tr>
            <td class="tdLabel">
                <label class="lblNormal">
                    <%=GetLabel("Perawat")%></label>
            </td>
            <td colspan="3">
                <asp:TextBox ID="txtParamedicName" Width="400px" runat="server" Style="text-align: left" ReadOnly="true" />
            </td>
        </tr>
        <tr>
            <td class="tdLabel">
                <label class="lblNormal">
                    <%=GetLabel("HD Ke")%></label>
            </td>
            <td>
                <asp:TextBox ID="txtHDNo" Width="80px" CssClass="number" runat="server" Style="text-align: right" ReadOnly="true" />
            </td>
            <td class="tdLabel">
                <label class="lblMandatory">
                    <%=GetLabel("HD Pertama Kali")%></label>
            </td>
            <td colspan="2">
                <asp:TextBox ID="txtFirstHDDate" Width="120px" CssClass="datepicker" runat="server" ReadOnly="true" />
            </td>
        </tr>
        <tr>
            <td class="tdLabel">
                <label class="lblMandatory">
                    <%=GetLabel("HFR Ke")%></label>
            </td>
            <td>
                <asp:TextBox ID="txtHFRNo" Width="80px" CssClass="number" runat="server" Style="text-align: right" ReadOnly="true" />
            </td>
            <td class="tdLabel">
                <label class="lblMandatory">
                    <%=GetLabel("HDFMD Ke")%></label>
            </td>
            <td>
                <asp:TextBox ID="txtHDFMDNo" Width="80px" CssClass="number" runat="server" Style="text-align: right" ReadOnly="true" />
            </td>
        </tr>
        <tr>
            <td class="tdLabel">
                <label class="lblMandatory">
                    <%=GetLabel("Hemoperfusion Ke")%></label>
            </td>
            <td>
                <asp:TextBox ID="txtHemoperfusionNo" Width="80px" CssClass="number" runat="server" Style="text-align: right" ReadOnly="true" />
            </td>
            <td class="tdLabel">
                <label class="lblNormal">
                    <%=GetLabel("Nomor Mesin")%></label>
            </td>
            <td colspan="3">
                <asp:TextBox ID="txtMachineNo" Width="80px" runat="server" Style="text-align: left" ReadOnly="true" />
            </td>
        </tr>
        <tr>
            <td class="tdLabel">
                <label class="lblNormal">
                    <%=GetLabel("Jenis Peresepan")%></label>
            </td>
            <td>
                <asp:TextBox ID="txtHDType" Width="100%" runat="server" Style="text-align: left" ReadOnly="true" />
            </td>
            <td class="tdLabel">
                <label class="lblNormal">
                    <%=GetLabel("Metode HD")%></label>
            </td>
            <td>
                <asp:TextBox ID="txtHDMethod" Width="100%" runat="server" Style="text-align: left" ReadOnly="true" />
            </td>
        </tr>
        <tr>
            <td class="tdLabel">
                <label class="lblNormal">
                    <%=GetLabel("Tipe Dialiser")%></label>
            </td>
            <td>
                <asp:TextBox ID="txtMachineType" Width="100%" runat="server" Style="text-align: left" ReadOnly="true" />
            </td>
            <td class="tdLabel">
                <label class="lblNormal">
                    <%=GetLabel("Dialisat")%></label>
            </td>
            <td>
                <table border="0" cellpadding="0" cellspacing="1" width="100%">
                    <tr>
                        <td colspan="2">
                            <table border="0" cellpadding="0" cellspacing="1" width="100%">
                                <tr>
                                    <td>
                                          <asp:TextBox ID="txtDialysate" Width="100%" runat="server" Style="text-align: left" ReadOnly="true" />
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtDialysateRemarks" Width="100%" runat="server" Style="text-align: left" ReadOnly="true" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td class="tdLabel">
                <label class="lblNormal">
                    <%=GetLabel("Reuse Ke")%></label>
            </td>
            <td>
                <asp:TextBox ID="txtReuseNo" Width="80px" CssClass="number" runat="server" Style="text-align: right" ReadOnly="true" />
            </td>
            <td class="tdLabel">
                <label class="lblNormal">
                    <%=GetLabel("Volume Priming/Total Cel Volume")%></label>
            </td>
            <td>
                <asp:TextBox ID="txtVolumePriming" Width="80px" CssClass="number" runat="server" Style="text-align: right" ReadOnly="true"/>
            </td>
        </tr>
        <tr>
            <td class="tdLabel">
                <label class="lblNormal">
                    <%=GetLabel("Durasi")%></label>
            </td>
            <td>
                <asp:TextBox ID="txtHDDuration" Width="80px" CssClass="number" runat="server" Style="text-align: right" ReadOnly="true" /> jam
            </td>
            <td class="tdLabel">
                <label class="lblNormal">
                    <%=GetLabel("Frekuensi")%></label>
            </td>
            <td>
                <asp:TextBox ID="txtHDFrequency" Width="80px" CssClass="number" runat="server" Style="text-align: right" ReadOnly="true"/> x/minggu
            </td>
        </tr>
        <tr>
            <td class="tdLabel">
                <label class="lblNormal">
                    <%=GetLabel("QB")%></label>
            </td>
            <td>
                <asp:TextBox ID="txtQB" Width="80px" CssClass="number" runat="server" Style="text-align: right" ReadOnly="true"/> cc/menit
            </td>
            <td class="tdLabel">
                <label class="lblNormal">
                    <%=GetLabel("QD")%></label>
            </td>
            <td>
                <asp:TextBox ID="txtQD" Width="80px" CssClass="number" runat="server" Style="text-align: right" ReadOnly="true"/> cc/menit
            </td>
        </tr>
        <tr>
            <td class="tdLabel">
                <label class="lblNormal">
                    <%=GetLabel("UF Goal")%></label>
            </td>
            <td>
                <asp:TextBox ID="txtUFGoal" Width="80px" CssClass="number" runat="server" Style="text-align: right" ReadOnly="true"/> cc
            </td>
            <td class="tdLabel">
                <label class="lblNormal">
                    <%=GetLabel("UFR")%></label>
            </td>
            <td>
                <asp:TextBox ID="txtProgProfilingUF" Width="80px" CssClass="number" runat="server" Style="text-align: right" ReadOnly="true"/> ml/menit
            </td>

        </tr>
        <tr>
            <td class="tdLabel">
                <label class="lblNormal">
                    <%=GetLabel("Prog. Profiling Na")%></label>
            </td>
            <td>
                <asp:TextBox ID="txtProgProfilingNa" Width="80px" CssClass="number" runat="server" Style="text-align: right" ReadOnly="true"/>
            </td>
        </tr>
        <tr>
            <td class="tdLabel" valign="top">
                <label class="lblNormal">
                    <%=GetLabel("Catatan Lain-lain")%></label>
            </td>
            <td colspan="3">
                <asp:TextBox ID="txtAdditionalRemarks" runat="server" TextMode="MultiLine" Rows="1"
                    Width="100%" ReadOnly="true"/>
            </td>
        </tr>
        <tr>
            <td colspan="4">
                <asp:RadioButton GroupName="heparization" ID="optIsHeparization" runat="server" Text=" Heparinisasi" Checked="false" Enabled="false" />
            </td>
        </tr>
        <tr>
            <td class="tdLabel">
                <label class="lblNormal">
                    <%=GetLabel("Dosis Awal")%></label>
            </td>
            <td>
                <asp:TextBox ID="txtHeparizationDosageInitiate" Width="60px" CssClass="number" runat="server" Style="text-align: right" ReadOnly="true" /> iu
            </td>
            <td class="tdLabel">
                <label class="lblNormal">
                    <%=GetLabel("Dosis Sirkulasi")%></label>
            </td>
            <td>
                <asp:TextBox ID="txtHeparizationDosageCirculation" Width="60px" CssClass="number" runat="server" Style="text-align: right" ReadOnly="true" /> iu
            </td>
        </tr>
        <tr>
            <td colspan="4">
                <label class="lblNormal" style="font-weight:bold;">
                    <%=GetLabel("Dosis Maintenance :")%></label>
            </td>
        </tr>
        <tr>
            <td class="tdLabel">
                <label class="lblNormal">
                    <%=GetLabel("Continous")%></label>
            </td>
            <td>
                <asp:TextBox ID="txtHeparizationDosageContinues" Width="60px" CssClass="number" runat="server" Style="text-align: right" ReadOnly="true" /> iu/jam
            </td>
            <td class="tdLabel">
                <label class="lblNormal">
                    <%=GetLabel("Intermittent")%></label>
            </td>
            <td>
                <asp:TextBox ID="txtHeparizationDosageIntermitten" Width="60px" CssClass="number" runat="server" Style="text-align: right" ReadOnly="true"/> iu/jam
            </td>
        </tr>
        <tr>
            <td colspan="4">
                <asp:RadioButton GroupName="heparization" ID="optIsWithoutHeparization" runat="server" Text=" Tanpa Heparinisasi" Checked="false" Enabled="false"/>
            </td>
        </tr>             
        <tr>
            <td class="tdLabel" valign="top">
                <label class="lblNormal">
                    <%=GetLabel("Penyebab Tanpa Heparinisasi")%></label>
            </td>
            <td colspan="3">
                <asp:TextBox ID="txtWithoutHeparizationRemarks" runat="server" TextMode="MultiLine" Rows="1"
                    Width="100%" ReadOnly="true" />
            </td>
        </tr> 
        <tr>
            <td valign="top">
            </td>
            <td colspan="3">
                <table border="0" cellpadding="0" cellspacing="1">
                    <tr>
                        <td>
                            <asp:CheckBox ID="chkIsDialysisBleach" runat="server" Text=" Pembilasan NaCl 0.9%" Checked="false" Enabled="false" />
                        </td>
                        <td><asp:TextBox ID="txtDialysisBleach" Width="80px" CssClass="number" runat="server" Style="text-align: right" ReadOnly="true"/></td>
                        <td>
                            <asp:TextBox ID="txtDialysisBleachUnit" Width="80px" CssClass="number" runat="server" Style="text-align: left" ReadOnly="true"/> 
                        </td>
                    </tr>
                </table>
            </td>
        </tr>                                                          
        <tr>
            <td valign="top">
                <asp:RadioButton GroupName="heparization" ID="optIsLMWH" runat="server" Text=" LMWH" Checked="false" Enabled="false" />
            </td>
            <td colspan="3">
                <asp:TextBox ID="txtLMWHRemarks" runat="server" TextMode="MultiLine" Rows="1"
                    Width="100%" ReadOnly="true"/>
            </td>
        </tr>
    </table>
</div>
