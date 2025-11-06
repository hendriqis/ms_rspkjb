<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EditMedicationAdministrationCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.EditMedicationAdministrationCtl" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
    <%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<div class="toolbarArea">
    <ul>
        <li id="btnMPEntryProcess">
            <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbSave.png")%>' alt="" /><div>
                <%=GetLabel("Save")%></div>
        </li>
    </ul>
</div>
<script type="text/javascript" id="dxss_patiententryctl">
    $(function () {
        $('#<%=rblIsPatientFamily.ClientID %> input').change(function () {
            if ($(this).val() == "1") {
                $('#<%=trFamilyInfo.ClientID %>').removeAttr("style");
            }
            else {
                $('#<%=trFamilyInfo.ClientID %>').attr("style", "display:none");
            }
        });
    });

    function validateTime(timeValue) {
        var result = true;
        if (timeValue == "" || timeValue.indexOf(":") < 0 || timeValue.length != 5) {
            result = false;
        }
        else {
            var sHours = timeValue.split(':')[0];
            var sMinutes = timeValue.split(':')[1];

            if (sHours == "" || isNaN(sHours) || parseInt(sHours) > 23) {
                result = false;
            }
            else if (parseInt(sHours) == 0)
                sHours = "00";
            else if (sHours < 10)
                sHours = "0" + sHours;

            if (sMinutes == "" || isNaN(sMinutes) || parseInt(sMinutes) > 59) {
                result = false;
            }
            else if (parseInt(sMinutes) == 0)
                sMinutes = "00";
            else if (sMinutes < 10)
                sMinutes = "0" + sMinutes;
        }
        return result;
    }

    $('#btnMPEntryProcess').click(function () {
        var message = "Save the changes for <b>" + $('#<%:txtItemName.ClientID %>').val() + "</b> ?";
        showToastConfirmation(message, function (result) {
            if (result) {
                if (validateTime($('#<%=txtProceedTime.ClientID %>').val())) {
                    cbpPopupProcess.PerformCallback('process');
                }
                else {
                    showToast('Warning', 'Format Waktu yang diinput salah');
                }
            }
        });
    });

    function onCbpPopupProcesEndCallback(s) {
        hideLoadingPanel();
        var param = s.cpResult.split('|');
        var retval = s.cpRetval.split('|');
        if (param[0] == 'process') {
            if (param[1] == '0') {
                showToast("Print Failed", "Error Message : <br/><span style='color:red'>" + param[2] + "</span>");
            }
            else {
                pcRightPanelContent.Hide();
                if (typeof onRefreshDetailList == 'function')
                    onRefreshDetailList();
                else
                    if (typeof onRefreshControl == 'function')
                        onRefreshControl();                    
            }
        }
    }

    function onCbpViewEndCallback(s) {
        hideLoadingPanel();
    }

    function onRefreshGrid() {
        cbpView.PerformCallback('refresh');
    }
</script>
<input type="hidden" runat="server" id="hdnSelectedID" value="" />
<input type="hidden" runat="server" id="hdnSelectedScheduleID" value="" />
<input type="hidden" runat="server" id="hdnSelectedItem" value="" />
<input type="hidden" runat="server" id="hdnDispensaryServiceUnitID" value="" />
<input type="hidden" runat="server" id="hdnMedicationTime" value="" />
<div>
    <div>
        <table width="100%">
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
                    <asp:TextBox ID="txtSequenceNo" runat="server" Width="40px" ReadOnly="true" CssClass="number" />
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
                        <%=GetLabel("Jadwal Pemberian")%></label>
                </td>
                <td>
                    <asp:TextBox ID="txtMedicationTime" runat="server" Width="60px" Text="00:00" CssClass="time" ReadOnly="true" />
                </td>
                <td class="tdLabel">
                    <label class="lblNormal">
                        <%=GetLabel("Jam Diberikan")%></label>
                </td>
                <td>
                    <asp:TextBox ID="txtProceedTime" runat="server" Width="60px" Text="__:__" CssClass="time"/>
                </td>
            </tr>
            <tr>
                <td class="tdLabel">
                    <label class="lblNormal">
                        <%=GetLabel("Dosis")%></label>
                </td>
                <td>
                    <asp:TextBox ID="txtNumberOfDosage" runat="server" Width="40px" CssClass="number" />
                </td>
                <td>
                    <dxe:ASPxComboBox ID="cboDosingUnit" ClientInstanceName="cboDosingUnit" runat="server"
                        Width="100%" />
                </td>
            </tr>
            <tr>
                <td class="tdLabel">
                    <label class="lblNormal">
                        <%=GetLabel("Status Pemberian")%></label>
                </td>
                <td colspan="3">
                    <dxe:ASPxComboBox ID="cboMedicationStatus" ClientInstanceName="cboMedicationStatus"
                    runat="server" Width="100%" >
                    </dxe:ASPxComboBox>
                </td>
            </tr>
            <tr>
                <td class="tdLabel">
                    <label class="lblNormal">
                        <%=GetLabel("Catatan Pemberian")%></label>
                </td>
                <td colspan="3">
                    <asp:TextBox ID="txtOtherMedicationStatus" runat="server" Width="100%"/>
                </td>
            </tr>
            <tr>
                <td class="tdLabel">
                    <label class="lblNormal">
                        <%=GetLabel("Perawat Pemberi")%></label>
                </td>
                <td colspan="3">
                    <dxe:ASPxComboBox ID="cboParamedicID" ClientInstanceName="cboParamedicID"
                    runat="server" Width="100%" >
                    </dxe:ASPxComboBox>
                </td>
            </tr>
            <tr>
                <td class="tdLabel">
                    <asp:CheckBox ID="chkIsNeedConfirmation" runat="server" Checked = "false" /> <%:GetLabel("Konfirmasi Perawat")%>
                </td>
                <td colspan="2">
                    <dxe:ASPxComboBox ID="cboParamedic2" ClientInstanceName="cboParamedic2" runat="server" Width="100%" >
                    </dxe:ASPxComboBox>
                </td>
            </tr>
            <tr>
                <td class="tdLabel">
                    <label class="lblMandatory">
                        <%=GetLabel("Penerima Informasi")%></label>
                </td>
                <td colspan="3">
                    <asp:RadioButtonList ID="rblIsPatientFamily" runat="server" RepeatDirection="Horizontal">
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
                                <asp:TextBox ID="txtPatientFamilyName" CssClass="txtPatientFamilyName" runat="server" Width="100%"  />
                            </td>
                            <td class="tdLabel" style="padding-left:5px">
                                <label class="lblMandatory">
                                    <%=GetLabel("Hubungan")%></label>
                            </td>
                            <td>
                                <dxe:ASPxComboBox runat="server" ID="cboFamilyRelation" ClientInstanceName="cboFamilyRelation"
                                    Width="99%" ToolTip = "Hubungan dengan Pasien" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>  
        </table>
    </div>
    <dxcp:ASPxCallbackPanel ID="cbpPopupProcess" runat="server" Width="100%" ClientInstanceName="cbpPopupProcess"
        ShowLoadingPanel="false" OnCallback="cbpPopupProcess_Callback">
        <ClientSideEvents BeginCallback="function(s,e) { showLoadingPanel(); }" EndCallback="function(s,e) { onCbpPopupProcesEndCallback(s); }" />
    </dxcp:ASPxCallbackPanel>
</div>
