<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MedicationScheduleInfoCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.MedicationScheduleInfoCtl" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
    <%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<script type="text/javascript" id="dxss_patiententryctl">
    $(function () {
    });


    function onCbpViewEndCallback(s) {
        hideLoadingPanel();
    }

    function onRefreshGrid() {
        cbpView.PerformCallback('refresh');
    }
</script>
<input type="hidden" runat="server" id="hdnSelectedID" value="" />
<input type="hidden" runat="server" id="hdnSelectedScheduleID" value="" />
<input type="hidden" runat="server" id="hdnDispensaryServiceUnitID" value="" />
<div>
    <div>
        <table>
            <colgroup>
                <col width="135px" />
                <col width="120px" />
                <col width="115px" />
                <col />
            </colgroup>
            <tr>
                <td class="tdLabel">
                    <label class="lblNormal">
                        <%=GetLabel("Tanggal")%></label>
                </td>
                <td>
                    <asp:TextBox ID="txtMedicationDate" runat="server" Width="120px" ReadOnly="true" CssClass="datepicker" />
                </td>
                <td class="tdLabel">
                    <label class="lblNormal">
                        <%=GetLabel("Sequence No.")%></label>
                </td>
                <td>
                    <asp:TextBox ID="txtSequenceNo" runat="server" Width="100%" ReadOnly="true" CssClass="number" />
                </td>
            </tr>
            <tr>
                <td class="tdLabel">
                    <label class="lblNormal">
                        <%=GetLabel("Jadwal Pemberian")%></label>
                </td>
                <td>
                    <asp:TextBox ID="txtMedicationTime" runat="server" Width="60px" ReadOnly="true" CssClass="time"/>
                </td>
                <td class="tdLabel">
                    <label class="lblNormal">
                        <%=GetLabel("Waktu Pemberian")%></label>
                </td>
                <td>
                    <asp:TextBox ID="txtProceedTime" runat="server" Width="60px" ReadOnly="true" CssClass="time"/>
                </td>
            </tr>
            <tr>
                <td class="tdLabel">
                    <label class="lblNormal">
                        <%=GetLabel("Obat")%></label>
                </td>
                <td colspan="3">
                    <asp:TextBox ID="txtItemName" runat="server" Width="100%" ReadOnly="true"/>
                </td>
            </tr>
            <tr>
                <td class="tdLabel">
                    <label class="lblNormal">
                        <%=GetLabel("Status Pemberian")%></label>
                </td>
                <td colspan="3">
                    <asp:TextBox ID="txtMedicationStatus" runat="server" ReadOnly="true" Width="100%"/>
                </td>
            </tr>
            <tr>
                <td class="tdLabel" style="vertical-align:top">
                    <label class="lblNormal">
                        <%=GetLabel("Catatan Pemberian")%></label>
                </td>
                <td colspan="3">
                    <asp:TextBox ID="txtOtherMedicationStatus" runat="server" ReadOnly="true" Width="100%" TextMode=MultiLine Height="50px"/>
                </td>
            </tr>
            <tr>
                <td class="tdLabel">
                    <label class="lblNormal">
                        <%=GetLabel("Dilakukan oleh")%></label>
                </td>
                <td colspan="3">
                    <asp:TextBox ID="txtParamedicName" runat="server" ReadOnly="true" Width="100%"/>
                </td>
            </tr>
            <tr>
                <td class="tdLabel">
                    <asp:CheckBox ID="chkIsNeedConfirmation" runat="server" Checked = "false" Enabled="false" /> <%:GetLabel("Konfirmasi Perawat")%>
                </td>
                <td colspan="2">
                    <asp:TextBox ID="txtParamedicName2" runat="server" ReadOnly="true" Width="100%"/>
                </td>
                <td>
                    <asp:TextBox ID="txtConfirmationDateTime" runat="server" ReadOnly="true" Width="100%" />
                </td>
            </tr>
            <tr>
                <td class="tdLabel">
                    <label class="lblMandatory">
                        <%=GetLabel("Penerima Informasi")%></label>
                </td>
                <td colspan="3">
                    <asp:RadioButtonList ID="rblIsPatientFamily" runat="server" RepeatDirection="Horizontal" Enabled="false">
                        <asp:ListItem Text="Pasien" Value="0" Selected="True" />
                        <asp:ListItem Text="Keluarga / Lain-lain" Value="1"  />
                    </asp:RadioButtonList>
                </td>
            </tr>
            <tr id="trFamilyInfo" runat="server" style="display: none">
                <td class="tdLabel">
                    <label class="lblMandatory">
                        <%=GetLabel("Nama Penerima")%></label>
                </td>
                <td colspan="3">
                    <table border="0" cellpadding="1" cellspacing="0" style="width:100%">
                        <colgroup>
                            <col style="width:150px"/>
                            <col style="width:100px"/>
                            <col />
                        </colgroup>
                        <tr>
                            <td>
                                <asp:TextBox ID="txtPatientFamilyName" CssClass="txtPatientFamilyName" runat="server" Width="100%" ReadOnly="true"  />
                            </td>
                            <td class="tdLabel" style="padding-left:5px">
                                <label class="lblMandatory">
                                    <%=GetLabel("Hubungan")%></label>
                            </td>
                            <td>
                                 <asp:TextBox ID="txtFamilyRelation" runat="server" ReadOnly="true" Width="100%"/>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>  
            <tr>
                <td class="tdLabel">
                    <label class="lblNormal">
                        <%=GetLabel("Verifikasi Apoteker ")%></label>
                </td>
                <td colspan="3">
                    <table border="0" cellpadding="1" cellspacing="0">
                        <colgroup>
                            <col style="width:55%" />
                            <col style="width:45%" />
                        </colgroup>
                        <tr>
                            <td>
                                <asp:TextBox ID="txtVerifiedPharmacistName" runat="server" ReadOnly="true" Width="100%"/>
                            </td>
                            <td >
                                <asp:TextBox ID="txtVerifiedPharmacistDateTime" runat="server" ReadOnly="true" Width="100%"/>
                            </td>
                        </tr>
                    </table>

                </td>
            </tr>
            <tr>
                <td class="tdLabel">
                    <label class="lblNormal">
                        <%=GetLabel("Verifikasi DPJP  ")%></label>
                </td>
                <td colspan="3">
                    <table border="0" cellpadding="1" cellspacing="0">
                        <colgroup>
                            <col style="width:55%" />
                            <col style="width:45%" />
                        </colgroup>
                        <tr>
                            <td>
                                <asp:TextBox ID="txtVerifiedPhysicianName" runat="server" ReadOnly="true" Width="100%"/>
                            </td>
                            <td >
                                <asp:TextBox ID="txtVerifiedPhysicianDateTime" runat="server" ReadOnly="true" Width="100%"/>
                            </td>
                        </tr>
                    </table>

                </td>
            </tr>
        </table>
    </div>
</div>
