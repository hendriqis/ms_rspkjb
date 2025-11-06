<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="RadiotheraphyProgramEntry.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.RadiotheraphyProgramEntry" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<script type="text/javascript" id="dxss_radiotheraphyEntryctl">
    $(function () {
        setDatePicker('<%=txtProgramDate.ClientID %>');
        $('#<%=txtProgramDate.ClientID %>').datepicker('option', 'maxDate', '0');

        //#region Left Navigation Panel
        $('#leftPageNavPanel ul li').click(function () {
            $('#leftPageNavPanel ul li.selected').removeClass('selected');
            $(this).addClass('selected');
            var contentID = $(this).attr('contentID');

            showContent(contentID);
        });

        function showContent(contentID) {
            var i, x, tablinks;
            x = document.getElementsByClassName("divPageNavPanelContent");
            for (i = 0; i < x.length; i++) {
                x[i].style.display = "none";
            }
            document.getElementById(contentID).style.display = "block";
        }
        //#endregion

        $('#leftPageNavPanel ul li').first().click();

        if ($('#<%=hdnCombinationTechniqueCode.ClientID %>').val() != '') {
            var beamTech = $('#<%=hdnCombinationTechniqueCode.ClientID %>').val().split(';');
            for (var i = 0; i < beamTech.length; i++) {
                $('#<%=dlBeamTechnique.ClientID %>').find('.chkBeamTechnique').each(function () {
                    if ($(this).attr('controlID') == beamTech[i]) {
                        $(this).prop('checked', true);
                    }
                });
            }
        }

        if ($('#<%=hdnFormValues.ClientID %>').val() != '') {
            var oldData = $('#<%=hdnFormValues.ClientID %>').val().split(';');
            for (var i = 0; i < oldData.length; ++i) {
                var temp = oldData[i].split('=');
                $('#<%=divFormContent.ClientID %>').find('.ddlForm').each(function () {
                    if ($(this).attr('controlID') == temp[0])
                        $(this).val(temp[1]);
                });
                $('#<%=divFormContent.ClientID %>').find('.chkForm').each(function () {
                    if ($(this).attr('controlID') == temp[0] && temp[1] == "1") {
                        $(this).prop('checked', true);
                    }
                });
                $('#<%=divFormContent.ClientID %>').find('.chkNursingProblem').each(function () {
                    if ($(this).attr('controlID') == temp[0] && temp[1] == "1") {
                        $(this).prop('checked', true);
                    }
                });
                $('#<%=divFormContent.ClientID %>').find('.optForm').each(function () {
                    if ($(this).attr('controlID') == temp[0] && temp[1] == "1") {
                        $(this).prop('checked', true);
                    }
                });
                $('#<%=divFormContent.ClientID %>').find('.txtForm').each(function () {
                    if ($(this).attr('controlID') == temp[0])
                        $(this).val(temp[1]);
                });
            }
        }

        $('#<%=chkIsHasSequence2.ClientID %>').change(function () {
            var isChecked = $(this).is(":checked");
            if (isChecked) {
                ToggleDosageControl("02", "1");
            }
            else {
                ToggleDosageControl("02", "0");
            }
        });

        $('#<%=chkIsHasSequence3.ClientID %>').change(function () {
            var isChecked = $(this).is(":checked");
            if (isChecked) {
                ToggleDosageControl("03", "1");
            }
            else {
                ToggleDosageControl("03", "0");
            }
        });

        function ToggleDosageControl(sequence, mode) {
            switch (sequence) {
                case "02":
                    switch (mode) {
                        case "1":
                            $('#<%=txtNumberOfDosage2.ClientID %>').removeAttr("disabled");
                            $('#<%=txtNumberOfFields2.ClientID %>').removeAttr("disabled");
                            break;
                        default:
                            $('#<%=txtNumberOfDosage2.ClientID %>').attr("disabled", "disabled");
                            $('#<%=txtNumberOfFields2.ClientID %>').attr("disabled", "disabled");
                            break;
                    }
                    break;
                case "03":
                    switch (mode) {
                        case "1":
                            $('#<%=txtNumberOfDosage3.ClientID %>').removeAttr("disabled");
                            $('#<%=txtNumberOfFields3.ClientID %>').removeAttr("disabled");
                            break;
                        default:
                            $('#<%=txtNumberOfDosage3.ClientID %>').attr("disabled", "disabled");
                            $('#<%=txtNumberOfFields3.ClientID %>').attr("disabled", "disabled");
                            break;
                    }
                    break;
                default:
                    $('#<%=txtNumberOfDosage2.ClientID %>').attr("disabled", "disabled");
                    $('#<%=txtNumberOfFields2.ClientID %>').attr("disabled", "disabled");
                    $('#<%=txtNumberOfDosage3.ClientID %>').attr("disabled", "disabled");
                    $('#<%=txtNumberOfFields3.ClientID %>').attr("disabled", "disabled");
                    break;
            };
        }

        $('#<%=txtNumberOfDosage1.ClientID %>').change(function () {
            if ($('#<%=txtNumberOfDosage1.ClientID %>').val() > 0) {
                $('#<%=txtTotalDosage1.ClientID %>').val(CalculateTotalDosis("01"));
            } else {
                displayErrorMessageBox('Total Dosis', "Jumlah Dosis 1 harus lebih besar dari 0");
            }
        });

        $('#<%=txtNumberOfFields1.ClientID %>').change(function () {
            if ($('#<%=txtNumberOfFields1.ClientID %>').val() > 0) {
                $('#<%=txtTotalDosage1.ClientID %>').val(CalculateTotalDosis("01"));
            } else {
                displayErrorMessageBox('Total Dosis', "Jumlah Fraksi 1 harus lebih besar dari 0");
            }
        });

        $('#<%=txtNumberOfDosage2.ClientID %>').change(function () {
            if ($('#<%=txtNumberOfDosage2.ClientID %>').val() > 0) {
                $('#<%=txtTotalDosage2.ClientID %>').val(CalculateTotalDosis("02"));
            } else {
                displayErrorMessageBox('Total Dosis', "Jumlah Dosis 2 harus lebih besar dari 0");
            }
        });

        $('#<%=txtNumberOfFields2.ClientID %>').change(function () {
            if ($('#<%=txtNumberOfFields2.ClientID %>').val() > 0) {
                $('#<%=txtTotalDosage2.ClientID %>').val(CalculateTotalDosis("02"));
            } else {
                displayErrorMessageBox('Total Dosis', "Jumlah Fraksi 2 harus lebih besar dari 0");
            }
        });

        $('#<%=txtNumberOfDosage3.ClientID %>').change(function () {
            if ($('#<%=txtNumberOfDosage3.ClientID %>').val() > 0) {
                $('#<%=txtTotalDosage3.ClientID %>').val(CalculateTotalDosis("01"));
            } else {
                displayErrorMessageBox('Total Dosis', "Jumlah Dosis 3 harus lebih besar dari 0");
            }
        });

        $('#<%=txtNumberOfFields3.ClientID %>').change(function () {
            if ($('#<%=txtNumberOfFields3.ClientID %>').val() > 0) {
                $('#<%=txtTotalDosage3.ClientID %>').val(CalculateTotalDosis("03"));
            } else {
                displayErrorMessageBox('Total Dosis', "Jumlah Fraksi 3 harus lebih besar dari 0");
            }
        });
    });

    function CalculateTotalDosis(sequence) {
        var jumlahDosis = 0;
        var jumlahFraksi = 0;
        var totalDosis = 0;

        switch (sequence) {
            case "01":
                jumlahDosis = $('#<%=txtNumberOfDosage1.ClientID %>').val();
                jumlahFraksi = $('#<%=txtNumberOfFields1.ClientID %>').val();
                break;
            case "02":
                jumlahDosis = $('#<%=txtNumberOfDosage2.ClientID %>').val();
                jumlahFraksi = $('#<%=txtNumberOfFields2.ClientID %>').val();
                break;
            case "03":
                jumlahDosis = $('#<%=txtNumberOfDosage3.ClientID %>').val();
                jumlahFraksi = $('#<%=txtNumberOfFields3.ClientID %>').val();
                break;
            default:
                break;
        }

        if (jumlahFraksi > 0) {
            totalDosis = (jumlahDosis * jumlahFraksi).toFixed(2);
        }
        else {
            totalDosis = 0; 
        }

        return totalDosis;
    }

    function onBeforeSaveRecord() {
//        var values = getFormValues();
        var combinationTechnique = getCombinationTechnique();
        return true;
    }

    function getFormValues() {
        var controlValues = '';
        $('#<%=divFormContent.ClientID %>').find('.ddlForm').each(function () {
            if (controlValues != '')
                controlValues += ";";
            controlValues += $(this).attr('controlID') + '=' + $(this).val();
        });
        $('#<%=divFormContent.ClientID %>').find('.txtForm').each(function () {
            if (controlValues != '')
                controlValues += ";";
            controlValues += $(this).attr('controlID') + '=' + $(this).val();
        });
        $('#<%=divFormContent.ClientID %>').find('.chkForm').each(function () {
            if (controlValues != '')
                controlValues += ";";
            if ($(this).is(':checked'))
                controlValues += $(this).attr('controlID') + '=1';
            else
                controlValues += $(this).attr('controlID') + '=0';
        });
        $('#<%=divFormContent.ClientID %>').find('.chkNursingProblem').each(function () {
            if (controlValues != '')
                controlValues += ";";
            if ($(this).is(':checked'))
                controlValues += $(this).attr('controlID') + '=1';
            else
                controlValues += $(this).attr('controlID') + '=0';
        });
        $('#<%=divFormContent.ClientID %>').find('.optForm').each(function () {
            if (controlValues != '')
                controlValues += ";";
            if ($(this).is(':checked'))
                controlValues += $(this).attr('controlID') + '=1';
            else
                controlValues += $(this).attr('controlID') + '=0';
        });
        $('#<%=hdnFormValues.ClientID %>').val(controlValues);

        return controlValues;
    }

    function getCombinationTechnique() {
        var values = '';
        var name = '';
        $('#<%=dlBeamTechnique.ClientID %>').find('.chkBeamTechnique').each(function () {
            if ($(this).is(':checked')) {
                if (values != '')
                    values += ";";
                if (name != '')
                    name += ";";
                values += $(this).attr('controlID');
                name += $(this).attr('controlName');
            }
        });
        $('#<%=hdnCombinationTechniqueCode.ClientID %>').val(values);
        $('#<%=hdnCombinationTechnique.ClientID %>').val(name);
        return values;     
    }

    function onCboGCBeamTechniqueChanged(s) {
        var cboGCBeamTechnique = s.GetValue();

        if (cboGCBeamTechnique != Constant.BeamTechnique.OTHERS) {
            $('#<%=trCombination.ClientID %>').attr('style', 'display:none');
        }
        else {
            $('#<%=trCombination.ClientID %>').removeAttr('style');
        }
    }

    function onCboGCTherapyTypeChanged(s) {
        var cboGCTherapyType = s.GetValue();

        if (cboGCTherapyType != Constant.RadioTherapyType.BRACHYTHERAPY) {
            $('#<%=trBrachytherapy1.ClientID %>').attr('style', 'display:none');
            $('#<%=trBrachytherapy2.ClientID %>').attr('style', 'display:none');

            $('#<%=trExternal1.ClientID %>').removeAttr('style');
            $('#<%=trVerification.ClientID %>').removeAttr('style');
            $('#<%=trBeamTechnique.ClientID %>').removeAttr('style');
            onCboGCBeamTechniqueChanged(cboGCBeamTechnique);
        }
        else {
            $('#<%=trExternal1.ClientID %>').attr('style', 'display:none');
            $('#<%=trBeamTechnique.ClientID %>').attr('style', 'display:none');
            $('#<%=trCombination.ClientID %>').attr('style', 'display:none');
            $('#<%=trVerification.ClientID %>').attr('style', 'display:none');

            $('#<%=trBrachytherapy1.ClientID %>').removeAttr('style');
            $('#<%=trBrachytherapy2.ClientID %>').removeAttr('style');
        }
    }

    function onAfterSaveAddRecordEntryPopup(result) {
        if (typeof onRefreshControl == 'function')
            onRefreshControl();
    }
    function onAfterSaveEditRecordEntryPopup(result) {
        if (typeof onRefreshControl == 'function')
            onRefreshControl();
    }

</script>
<div style="height: auto">
    <input type="hidden" runat="server" id="hdnPopupMRN" value="" />
    <input type="hidden" runat="server" id="hdnPopupVisitID" value="" />
    <input type="hidden" runat="server" id="hdnPopupMedicalNo" value="" />
    <input type="hidden" runat="server" id="hdnPopupPatientName" value="" />
    <input type="hidden" runat="server" id="hdnID" value="" />
    <input type="hidden" runat="server" id="hdnFormGroup" value="" />
    <input type="hidden" runat="server" id="hdnFormType" value="" />
    <input type="hidden" runat="server" id="hdnFormLayout" value="" />
    <input type="hidden" runat="server" id="hdnFormValues" value="" />
    <input type="hidden" runat="server" id="hdnCombinationTechnique" value="" />
    <input type="hidden" runat="server" id="hdnCombinationTechniqueCode" value="" />
    <table class="tblEntryContent">
        <colgroup>
            <col style="width: 25%" />
            <col style="width: 75%" />
        </colgroup>
        <tr>
            <td style="vertical-align: top">
                <div id="leftPageNavPanel" class="w3-border">
                    <ul>
                        <li contentid="divPage1" title="Program Radioterapi" class="w3-hover-red">Program Radioterapi</li>
                        <li contentid="divPage2" title="Catatan Tambahan" class="w3-hover-red">Catatan Tambahan</li>
                     </ul>
                </div>
            </td>
            <td style="vertical-align: top;">
                <div id="divPage1" class="w3-border divPageNavPanelContent w3-animate-left" style="display: none">
                    <table class="tblEntryContent" style="width: 100%">
                        <colgroup>
                            <col style="width: 180px" />
                            <col />
                        </colgroup>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("No. RM / Nama Pasien")%></label>                            
                            </td>
                            <td>
                                <table border="0" cellpadding="1" cellspacing="0">
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtMedicalNo" Width="100px" Enabled="false" runat="server" Style="text-align: center" />
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtPatientName" Width="250px" runat="server" Enabled="false"
                                                Style="text-align: left" />
                                        </td> 
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel" valign="top">
                                <label class="lblMandatory">
                                    <%=GetLabel("Tanggal Program")%></label>
                            </td>
                            <td>
                                <table border="0" cellpadding="1" cellspacing="0">
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtProgramDate" Width="120px" CssClass="datepicker" runat="server" />
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtProgramTime" Width="80px" CssClass="time" runat="server"
                                                Style="text-align: center" />
                                        </td> 
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel" valign="top">
                                <label class="lblNormal">
                                    <%=GetLabel("Diagnosis Pasien")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtDiagnosisInfo" runat="server" TextMode="MultiLine" Rows="5"
                                    Width="100%" ReadOnly="True" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel" valign="top">
                                <label class="lblNormal">
                                    <%=GetLabel("Stadium")%></label>
                            </td>   
                            <td>
                                <asp:TextBox ID="txtStagingInfo" Width="100%" runat="server" Enabled="false"
                                    Style="text-align: left" />
                            </td>                         
                        </tr>
                        <tr>
                            <td class="tdLabel" style="vertical-align:top">
                                <label class="lblMandatory">
                                    <%=GetLabel("Jenis Radioterapi")%></label>
                            </td>
                            <td>
                                <dxe:ASPxComboBox runat="server" ID="cboGCTherapyType" ClientInstanceName="cboGCTherapyType"
                                    Width="50%" ToolTip="Jenis Radioterapi">
                                    <ClientSideEvents ValueChanged="function(s){ onCboGCTherapyTypeChanged(s); }" />
                                </dxe:ASPxComboBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel" style="vertical-align:top">
                                <label class="lblMandatory">
                                    <%=GetLabel("Tujuan Radiasi")%></label>
                            </td>
                            <td>
                                <dxe:ASPxComboBox runat="server" ID="cboGCTherapyPurpose" ClientInstanceName="cboGCTherapyPurpose"
                                    Width="50%" ToolTip="Tujuan Radiasi">
                                </dxe:ASPxComboBox>
                            </td>
                        </tr>
                        <tr id="trExternal1" runat="server">
                            <td class="tdLabel" style="vertical-align:top">
                                <label class="lblMandatory">
                                    <%=GetLabel("Rencana Pengobatan")%></label>
                            </td>
                            <td>
                                <dxe:ASPxComboBox runat="server" ID="cboGCTherapyPlan" ClientInstanceName="cboGCTherapyPlan"
                                    Width="50%" ToolTip="Rencana Pengobatan">
                                </dxe:ASPxComboBox>
                            </td>
                        </tr>
                        <tr id="trBrachytherapy1" runat="server" style="display:none">
                            <td class="tdLabel" style="vertical-align:top">
                                <label class="lblMandatory">
                                    <%=GetLabel("Jenis Brakiterapi")%></label>
                            </td>
                            <td>
                                <dxe:ASPxComboBox runat="server" ID="cboGCBrachyTherapyType" ClientInstanceName="cboGCBrachyTherapyType"
                                    Width="50%" ToolTip="Jenis Brakiterapi">
                                </dxe:ASPxComboBox>
                            </td>
                        </tr>
                        <tr id="trBrachytherapy2" runat="server" style="display:none">
                            <td class="tdLabel" style="vertical-align:top">
                                <label class="lblMandatory">
                                    <%=GetLabel("Aplikator")%></label>
                            </td>
                            <td>
                                <dxe:ASPxComboBox runat="server" ID="cboGCApplicatorType" ClientInstanceName="cboGCApplicatorType"
                                    Width="50%" ToolTip="Aplikator Brakiterapi">
                                </dxe:ASPxComboBox>
                            </td>
                        </tr>
                        <tr id="trBeamTechnique" runat="server">
                            <td class="tdLabel" style="vertical-align:top">
                                <label class="lblNormal">
                                    <%=GetLabel("Teknik Penyinaran")%></label>
                            </td>
                            <td colspan="2">
                                <table border="0" cellpadding="1" cellspacing="0" style="width: 100%">
                                    <colgroup>
                                        <col style="width: 250px" />
                                        <col style="width: 80px" />
                                        <col />
                                    </colgroup>
                                    <tr>
                                        <td colspan="3">
                                            <dxe:ASPxComboBox runat="server" ID="cboGCBeamTechnique" ClientInstanceName="cboGCBeamTechnique"
                                                Width="50%" ToolTip="Teknik Penyinaran">
                                                <ClientSideEvents ValueChanged="function(s){ onCboGCBeamTechniqueChanged(s); }" />
                                            </dxe:ASPxComboBox>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr id="trCombination" runat="server" style="display:none">
                            <td class="tdLabel" style="vertical-align:top">
                                <label class="lblNormal">
                                    <%=GetLabel("Teknik Kombinasi")%></label>
                            </td>
                            <td>
                                <asp:DataList ID="dlBeamTechnique" runat="server" RepeatColumns="3"  
                                    CellSpacing="3" RepeatLayout="Table">  
                                    <ItemTemplate>  
                                        <table class="table">  
                                            <tr>  
                                                <td>
                                                     <input type="checkbox" class="chkBeamTechnique" controlID="<%#Eval("StandardCodeID")%>" controlName="<%#Eval("StandardCodeName") %>" /> <%#Eval("StandardCodeName")%>
                                                </td>  
                                            </tr>  
                                        </table>  
                                    </ItemTemplate>  
                                </asp:DataList>  
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <table border="0" cellpadding="1" cellspacing="0" width="100%">
                                    <colgroup>
                                        <col width="139px" />
                                        <col width="100px" />
                                        <col width="100px" />
                                        <col width="100px" />
                                    </colgroup>
                                    <thead>
                                        <td style="text-align:right"></td>
                                        <td style="text-align:center">Dosis Radiasi</td>
                                        <td style="text-align:center">Jumlah Fraksi</td>
                                        <td style="text-align:center">Total Dosis</td>
                                    </thead>
                                    <tr>
                                        <td style="text-align:right">
                                            01
                                        </td>
                                        <td style="text-align:center">
                                            <asp:TextBox ID="txtNumberOfDosage1" Width="100px" CssClass="number" runat="server" Style="text-align: right" />                                             
                                        </td>
                                        <td style="text-align:center">
                                            <asp:TextBox ID="txtNumberOfFields1" Width="100px" CssClass="number" runat="server" Style="text-align: right" />                                            
                                        </td>
                                        <td style="text-align:center">
                                            <asp:TextBox ID="txtTotalDosage1" Width="100px" CssClass="number" runat="server" Style="text-align: right" ReadOnly="true" />                                             
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align:right">
                                            <asp:CheckBox ID="chkIsHasSequence2" runat="server" Text=" 02" Checked="false" />
                                        </td>
                                        <td style="text-align:center">
                                            <asp:TextBox ID="txtNumberOfDosage2" Width="100px" CssClass="number" runat="server" Style="text-align: right" Enabled="False" />                                             
                                        </td>
                                        <td style="text-align:center">
                                            <asp:TextBox ID="txtNumberOfFields2" Width="100px" CssClass="number" runat="server" Style="text-align: right" Enabled="False" />                                            
                                        </td>
                                        <td style="text-align:center">
                                            <asp:TextBox ID="txtTotalDosage2" Width="100px" CssClass="number" runat="server" Style="text-align: right" ReadOnly="true" />                                             
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align:right">
                                            <asp:CheckBox ID="chkIsHasSequence3" runat="server" Text=" 03" Checked="false" />
                                        </td>
                                        <td style="text-align:center">
                                            <asp:TextBox ID="txtNumberOfDosage3" Width="100px" CssClass="number" runat="server" Style="text-align: right" Enabled="False" />                                             
                                        </td>
                                        <td style="text-align:center">
                                            <asp:TextBox ID="txtNumberOfFields3" Width="100px" CssClass="number" runat="server" Style="text-align: right" Enabled="False" />                                            
                                        </td>
                                        <td style="text-align:center">
                                            <asp:TextBox ID="txtTotalDosage3" Width="100px" CssClass="number" runat="server" Style="text-align: right" ReadOnly="true" />                                             
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr id="trVerification" runat="server">
                            <td class="tdLabel" style="vertical-align:top">
                                <label class="lblMandatory">
                                    <%=GetLabel("Verifikasi")%></label>
                            </td>
                            <td>
                                <dxe:ASPxComboBox runat="server" ID="cboGCVerificationType" ClientInstanceName="cboGCVerificationType"
                                    Width="50%" ToolTip="Verifikasi">
                                </dxe:ASPxComboBox>
                            </td>
                        </tr>
                    </table>
                </div>
                <div id="divPage2" class="w3-border divPageNavPanelContent w3-animate-left" style="display: none">
                    <table class="tblEntryContent" style="width: 100%">
                        <colgroup>
                            <col style="width: 150px" />
                            <col />
                        </colgroup>
                        <tr>
                            <td class="tdLabel" valign="top">
                                <label class="lblNormal">
                                    <%=GetLabel("Catatan Permintaan Radiasi")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtRemarks" runat="server" TextMode="MultiLine" Rows="15"
                                    Width="100%" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <div id="divFormContent" runat="server" style="height: 450px; overflow-y: auto;">
                                </div>
                            </td>
                        </tr>
                    </table>
                </div>
            </td>
        </tr>
    </table>
</div>
