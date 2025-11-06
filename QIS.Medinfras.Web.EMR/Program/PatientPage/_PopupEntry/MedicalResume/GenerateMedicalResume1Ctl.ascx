<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="GenerateMedicalResume1Ctl.ascx.cs" Inherits="QIS.Medinfras.Web.EMR.Program.PatientPage.GenerateMedicalResume1Ctl" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="QIS.Medinfras.Web.CustomControl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null" 
    Namespace="QIS.Medinfras.Web.CustomControl" TagPrefix="qis" %>

<script type="text/javascript" id="dxss_diagnosisentryctl">
    $(function () {
        setDatePicker('<%=txtMedicalResumeDate.ClientID %>');
        $('#<%=txtMedicalResumeDate.ClientID %>').datepicker('option', 'maxDate', '0');
        $('#<%=txtSubjectiveResumeText.ClientID %>').focus();

        //#region Left Navigation Panel
        $('#leftPageNavPanel ul li').click(function () {
            $('#leftPageNavPanel ul li.selected').removeClass('selected');
            $(this).addClass('selected');
            var contentID = $(this).attr('contentID');

            showContent(contentID);
        });

        function showContent(contentID) {
            var i, x, tablinks;
            x = document.getElementsByClassName("divPageNavPanel1Content");
            for (i = 0; i < x.length; i++) {
                x[i].style.display = "none";
            }
            document.getElementById(contentID).style.display = "block";
        }
        //#endregion

        //#region DRG
        $('#<%=divFormContent1.ClientID %>').html($('#<%=hdnDRGLayout.ClientID %>').val());
        if ($('#<%=hdnDRGValue.ClientID %>').val() != '') {
            var oldData = $('#<%=hdnDRGValue.ClientID %>').val().split(';');
            for (var i = 0; i < oldData.length; ++i) {
                var temp = oldData[i].split('=');
                $('#<%=divFormContent1.ClientID %>').find('.ddlForm').each(function () {
                    if ($(this).attr('controlID') == temp[0])
                        $(this).val(temp[1]);
                });
                $('#<%=divFormContent1.ClientID %>').find('.optForm').each(function () {
                    if ($(this).attr('controlID') == temp[0] && temp[1] == "1") {
                        $(this).prop('checked', true);
                    }
                });
                $('#<%=divFormContent1.ClientID %>').find('.chkForm').each(function () {
                    if ($(this).attr('controlID') == temp[0] && temp[1] == "1") {
                        $(this).prop('checked', true);
                    }
                });
                $('#<%=divFormContent1.ClientID %>').find('.txtForm').each(function () {
                    if ($(this).attr('controlID') == temp[0])
                        $(this).val(temp[1]);
                });
            }
        }
        //#endregion

        $('#leftPageNavPanel ul li').first().click();
    });

    function onBeforeSaveRecord() {
        var rblSelectedValue = $("input[name='<%=rblIsHasSickLetter.UniqueID%>']:radio:checked").val();
        if (rblSelectedValue == '1')
        {
            if ($('#<%=txtNoOfDays.ClientID %>').val() == '' || $('#<%=txtNoOfDays.ClientID %>').val() == '0' || isNaN($('#<%=txtNoOfDays.ClientID %>').val())) {
                displayErrorMessageBox("Resume Medis Rawat Jalan", "Jumlah hari untuk surat keterangan sakit harus diisi dan lebih besar dari 0");
                return false;
            }
            else if (parseInt($('#<%=txtNoOfDays.ClientID %>').val()) <= 0) {
                displayErrorMessageBox("Resume Medis Rawat Jalan", "Jumlah hari untuk surat keterangan sakit harus lebih besar dari 0");
                return false;                
            }
        }

        var values = getDRGFormValues();
        return true;
    }

    //#region Get Form Values
    function getDRGFormValues() {
        var controlValues = '';
        $('#<%=divFormContent1.ClientID %>').find('.ddlForm').each(function () {
            if (controlValues != '')
                controlValues += ";";
            controlValues += $(this).attr('controlID') + '=' + $(this).val();
        });
        $('#<%=divFormContent1.ClientID %>').find('.txtForm').each(function () {
            if (controlValues != '')
                controlValues += ";";
            controlValues += $(this).attr('controlID') + '=' + $(this).val();
        });
        $('#<%=divFormContent1.ClientID %>').find('.optForm').each(function () {
            if (controlValues != '')
                controlValues += ";";
            if ($(this).is(':checked'))
                controlValues += $(this).attr('controlID') + '=1';
            else
                controlValues += $(this).attr('controlID') + '=0';
        });
        $('#<%=divFormContent1.ClientID %>').find('.chkForm').each(function () {
            if (controlValues != '')
                controlValues += ";";
            if ($(this).is(':checked'))
                controlValues += $(this).attr('controlID') + '=1';
            else
                controlValues += $(this).attr('controlID') + '=0';
        });

        $('#<%=hdnDRGValue.ClientID %>').val(controlValues);

        return controlValues;
    }
    //#endregion

    function onAfterSaveRecordPatientPageEntry(value) {
    }
</script>

<div style="height: 500px; border: 0px">
        <input type="hidden" value="" id="hdnID" runat="server" />
        <input type="hidden" value="" id="hdnDefaultParamedicID" runat="server" />
        <input type="hidden" value="" id="hdnDRGLayout" runat="server" />
        <input type="hidden" value="" id="hdnDRGValue" runat="server" />
        <table style="width:100%">
            <colgroup>
                <col style="width:150px"/>
                <col style="width:150px"/>
                <col style="width:100px"/>
                <col />
            </colgroup>
            <tr>
                <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Tanggal ")%> - <%=GetLabel("Jam ")%></label></td>
                <td><asp:TextBox ID="txtMedicalResumeDate" Width="120px" CssClass="datepicker" runat="server" /></td>
                <td><asp:TextBox ID="txtMedicalResumeTime" Width="80px" CssClass="time" runat="server"/></td>
                <td />
            </tr>
            <tr>
                <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Dokter ")%></label></td>
                <td colspan="3"><dxe:ASPxComboBox runat="server" ID="cboPhysician" ClientInstanceName="cboPhysician" Width="300px" /></td>
            </tr>
            <tr>
                <td class="tdLabel"><label><%=GetLabel("Surat Keterangan Sakit ")%></label></td>
                <td>
                    <asp:RadioButtonList ID="rblIsHasSickLetter" runat="server" RepeatDirection="Horizontal">
                        <asp:ListItem Text=" Tidak" Value="0" Selected="True" />
                        <asp:ListItem Text=" Ya" Value="1"  />
                    </asp:RadioButtonList>
                </td>
                <td><asp:TextBox ID="txtNoOfDays" Width="60px" runat="server" CssClass="number"/></td>
                <td class="tdLabel"><label><%=GetLabel("hari ")%></label></td>
            </tr>
            <tr>
                <td colspan="4">
                    <table class="tblContentArea">
                        <colgroup>
                            <col style="width: 30%" />
                            <col style="width: 70%" />
                        </colgroup>
                        <tr>
                            <td style="vertical-align:top">
                                <div id="leftPageNavPanel" class="w3-border">
                                    <ul>
                                        <li contentID="divPage1" title="Anamnesa/Keluhan Utama Pasien" class="w3-hover-red">Anamnesa/Keluhan Utama Pasien</li>
                                        <li contentID="divPage2" title="Pemeriksaan Fisik" class="w3-hover-red">Pemeriksaan Fisik</li>                         
                                        <li contentID="divPage3" title="Pemeriksaan Penunjang" class="w3-hover-red">Pemeriksaan Penunjang</li>
                                        <li contentID="divPage4" title="Diagnosa" class="w3-hover-red">Diagnosa</li>
                                        <li contentID="divPage5" title="Pengobatan/Terapi" class="w3-hover-red">Pengobatan/Terapi</li>
                                        <li contentID="divPage6" title="Tindakan yang dilakukan" class="w3-hover-red">Tindakan yang dilakukan</li>
                                        <%--<li contentID="divPage7" title="Kondisi/Penyakit berhubungan dengan" class="w3-hover-red">Kondisi/Penyakit berhubungan dengan</li>--%>
                                        <li contentID="divPage8" title="Anjuran Pemeriksaan" class="w3-hover-red">Anjuran Pemeriksaan</li>
                                    </ul>     
                                </div> 
                            </td>
                            <td style="vertical-align:top">
                                <div id="divPage1" class="w3-border divPageNavPanel1Content w3-animate-left" style="display:none"> 
                                    <table cellpadding="0" cellspacing="0" style="width: 99%">
                                        <tr>
                                            <td><asp:TextBox ID="txtSubjectiveResumeText" Width="100%" runat="server" TextMode="MultiLine" Rows="21" BorderWidth="0" /></td>
                                        </tr>
                                    </table>
                                </div>
                                <div id="divPage2" class="w3-border divPageNavPanel1Content w3-animate-left" style="display:none"> 
                                    <table cellpadding="0" cellspacing="0" style="width: 99%">
                                        <tr>
                                            <td><asp:TextBox ID="txtObjectiveResumeText" Width="100%" runat="server" TextMode="MultiLine" Rows="21" BorderWidth="0" /></td>
                                        </tr>
                                    </table>
                                </div>
                                <div id="divPage3" class="w3-border divPageNavPanel1Content w3-animate-left" style="display:none"> 
                                    <table cellpadding="0" cellspacing="0" style="width: 99%">
                                        <tr>
                                            <td><asp:TextBox ID="txtPlanningResumeText" Width="100%" runat="server" TextMode="MultiLine" Rows="21" BorderWidth="0" /></td>
                                        </tr>
                                    </table>
                                </div>
                                <div id="divPage4" class="w3-border divPageNavPanel1Content w3-animate-left" style="display:none"> 
                                    <table cellpadding="0" cellspacing="0" style="width: 99%">
                                        <tr>
                                            <td><asp:TextBox ID="txtAssessmentResumeText" Width="100%" runat="server" TextMode="MultiLine" Rows="21" BorderWidth="0" /></td>
                                        </tr>
                                    </table>
                                </div>
                                <div id="divPage5" class="w3-border divPageNavPanel1Content w3-animate-left" style="display:none"> 
                                    <table cellpadding="0" cellspacing="0" style="width: 99%">
                                        <tr>
                                            <td><asp:TextBox ID="txtMedicationResumeText" Width="100%" runat="server" TextMode="MultiLine" Rows="21" BorderWidth="0" /></td>
                                        </tr>
                                    </table>
                                </div>
                                <div id="divPage6" class="w3-border divPageNavPanel1Content w3-animate-left" style="display:none"> 
                                    <table cellpadding="0" cellspacing="0" style="width: 99%">
                                        <tr>
                                            <td><asp:TextBox ID="txtMedicalResumeText" Width="100%" runat="server" TextMode="MultiLine" Rows="21" BorderWidth="0" /></td>
                                        </tr>
                                    </table>
                                </div>
                                <div id="divPage7" class="w3-border divPageNavPanel1Content w3-animate-left" style="display:none"> 
                                    <div id="divFormContent1" runat="server" style="height: 500px;overflow-y: auto;"></div>
                                </div>
                                <div id="divPage8" class="w3-border divPageNavPanel1Content w3-animate-left" style="display:none"> 
                                    <table cellpadding="0" cellspacing="0" style="width: 99%">
                                        <tr>
                                            <td><asp:TextBox ID="txtInstructionResumeText" Width="100%" runat="server" TextMode="MultiLine" Rows="21" BorderWidth="0" /></td>
                                        </tr>
                                    </table>
                                </div>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
</div>
