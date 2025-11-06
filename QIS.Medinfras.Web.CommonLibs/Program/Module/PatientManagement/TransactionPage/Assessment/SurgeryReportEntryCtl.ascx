<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SurgeryReportEntryCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.SurgeryReportEntryCtl" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<script type="text/javascript" id="dxss_patientsurgecryctl">
    $(function () {
        setDatePicker('<%=txtStartDate.ClientID %>');
        $('#<%=txtStartDate.ClientID %>').datepicker('option', 'maxDate', '0');
        setDatePicker('<%=txtEndDate.ClientID %>');
        $('#<%=txtEndDate.ClientID %>').datepicker('option', 'maxDate', '0');
    });

    function oncbpProcessSaveEndCallback(s) {
        var param = s.cpResult.split('|');
        if (param[0] == 'save') {
            if (param[1] == 'fail') {
                showToast('Save Failed', 'Error Message : ' + param[2]);
                hideLoadingPanel();
            }
            else {
                hideLoadingPanel();
                pcRightPanelContent.Hide();
                onAfterSaveRecordPatientPageEntry();
            }
        }
        else if (param[0] == 'edit') {
            if (param[1] == 'fail') {
                showToast('Edit Failed', 'Error Message : ' + param[2]);
                hideLoadingPanel();
            }
            else {
                hideLoadingPanel();
                pcRightPanelContent.Hide();
                onAfterSaveRecordPatientPageEntry();
            }
        }
    }

    $('#ulTabOperative li').click(function () {
        $('#ulTabOperative li.selected').removeAttr('class');
        $('.ctOperativeDt').filter(':visible').hide();
        $contentID = $(this).attr('contentid');
        $('#' + $contentID).show();
        $(this).addClass('selected');
        lastContentID = $contentID;
    });

    $('#<%=btnMPEntryPopupSave.ClientID %>').click(function (evt) {
        if (IsValid(evt, 'fsEntryPopup', 'mpEntryPopup')) {
            if ($('#<%:hdnIsAdd.ClientID %>').val() == "1") {
                cbpProcessSave.PerformCallback('save');
            }
            else if ($('#<%:hdnIsAdd.ClientID %>').val() == "0") {
                cbpProcessSave.PerformCallback('edit');
            }
        }
        return false;
    });


    $('#<%=rblBloodDrain.ClientID %>').live('change', function () {
        var value = $(this).find('input:checked').val();
        if (value == '1') {
            $('#<%:txtOtherBloodDrainType.ClientID %>').removeAttr('readonly');
        }
        else {
            $('#<%:txtOtherBloodDrainType.ClientID %>').attr('readonly', 'readonly');
            $('#<%:txtOtherBloodDrainType.ClientID %>').val('');
        }
    });

    $('#<%=rblUsingTampon.ClientID %>').live('change', function () {
        var value = $(this).find('input:checked').val();
        if (value == '1') {
            $('#<%:txtTamponType.ClientID %>').removeAttr('readonly');
        }
        else {
            $('#<%:txtTamponType.ClientID %>').attr('readonly', 'readonly');
            $('#<%:txtTamponType.ClientID %>').val('');
        }
    });

    $('#<%=rblUsingCatheter.ClientID %>').live('change', function () {
        var value = $(this).find('input:checked').val();
        if (value == '1') {
            $('#<%:txtIsCatherterType.ClientID %>').removeAttr('readonly');
        }
        else {
            $('#<%:txtIsCatherterType.ClientID %>').attr('readonly', 'readonly');
            $('#<%:txtIsCatherterType.ClientID %>').val('');
        }
    });

    $('#<%=rblSpecimen.ClientID %>').live('change', function () {
        var value = $(this).find('input:checked').val();
        if (value == '1') {
            cboSpecimenType.removeAttr('readonly');
        }
        else {
            cboSpecimenType.attr('readonly', 'readonly');
        }
    });

    $('#<%=rblUsingImplant.ClientID %>').live('change', function () {
        var value = $(this).find('input:checked').val();
        if (value == '1') {
            $('#<%:txtImplantType.ClientID %>').removeAttr('readonly');
        }
        else {
            $('#<%:txtImplantType.ClientID %>').attr('readonly', 'readonly');
            $('#<%:txtImplantType.ClientID %>').val('')
        }
    });

    $('#<%=rblUsingAntibiotics.ClientID %>').live('change', function () {
        var value = $(this).find('input:checked').val();
        if (value == '1') {
            $('#<%:txtAntibioticsType.ClientID %>').removeAttr('readonly');
            $('#<%:txtAntibiotikHour.ClientID %>').removeAttr('readonly');
        }
        else {
            $('#<%:txtAntibioticsType.ClientID %>').attr('readonly', 'readonly');
            $('#<%:txtAntibiotikHour.ClientID %>').attr('readonly', 'readonly');
            $('#<%:txtAntibioticsType.ClientID %>').val('');
            $('#<%:txtAntibiotikHour.ClientID %>').val('');
        }
    });

    //#region Transaksi
    function getFilterExpression() {
        var filterExpression = "<%:OnGetFilterExpression() %>";
        return filterExpression;
    }

    function getFilterExpressionEdit() {
        var filterExpression = "<%:OnGetFilterExpressionEdit() %>";
        return filterExpression;
    }

    $('#<%:lblItemTransaksi.ClientID %>.lblLink').live('click', function () {
        openSearchDialog('patientchargesdt', getFilterExpression(), function (value) {
            $('#<%:txtItemTransaction.ClientID %>').val(value);
            onTxtItemTransactionNoChanged(value);
        });
    });

    $('#<%:txtItemTransaction.ClientID %>').live('change', function () {
        onTxtItemTransactionNoChangedFromTextBox($(this).val());
    });

    function onTxtItemTransactionNoChangedFromTextBox(value) {
        var filterExpression = getFilterExpressionEdit() + " AND ItemName1 = '" + value + "'";
        Methods.getObject('GetvPatientChargesDt6List', filterExpression, function (result) {
            if (result != null) {
                $('#<%:txtItemTransaction.ClientID %>').val(result.ItemName1);
                $('#<%:hdnTransactionDtID.ClientID %>').val(result.ID);
            }
            else {
                $('#<%:txtItemTransaction.ClientID %>').val('');
                $('#<%:hdnTransactionDtID.ClientID %>').val('');
            }
        });
    }

    function onTxtItemTransactionNoChanged(value) {
        var filterExpression = getFilterExpression() + " AND ID = '" + value + "'";
        Methods.getObject('GetvPatientChargesDt6List', filterExpression, function (result) {
            if (result != null) {
                $('#<%:txtItemTransaction.ClientID %>').val(result.ItemName1);
                $('#<%:hdnTransactionDtID.ClientID %>').val(result.ID);
            }
            else {
                $('#<%:txtItemTransaction.ClientID %>').val('');
                $('#<%:hdnTransactionDtID.ClientID %>').val('');
            }
        });
    }
    //#endregion

    //#region PreOperative
    $('#<%:lblDiagnosePre.ClientID %>.lblLink').live('click', function () {
        var filterExpression = "IsDeleted = 0";
        openSearchDialog('diagnose', filterExpression, function (value) {
            $('#<%:txtDiagnosePreCode.ClientID %>').val(value);
            ontxtDiagnosePreCodeNoChanged(value);
        });
    });

    $('#<%:txtDiagnosePreCode.ClientID %>').live('change', function () {
        ontxtDiagnosePreCodeNoChanged($(this).val());
    });

    function ontxtDiagnosePreCodeNoChanged(value) {
        var filterExpression = "IsDeleted = 0 AND DiagnoseID = '" + value + "'";
        Methods.getObject('GetDiagnoseList', filterExpression, function (result) {
            if (result != null) {
                $('#<%:txtDiagnosePreCode.ClientID %>').val(result.DiagnoseID);
                $('#<%:txtDiagnosePreName.ClientID %>').val(result.DiagnoseName);
                $('#<%:hdnDiagnosePreName.ClientID %>').val(result.DiagnoseName);
            }
            else {
                $('#<%:txtDiagnosePreCode.ClientID %>').val('');
                $('#<%:txtDiagnosePreName.ClientID %>').val('');
                $('#<%:hdnDiagnosePreName.ClientID %>').val('');
            }
        });
    }
    //#endregion

    //#region PostOperative
    $('#<%:lblDiagnosaPost.ClientID %>.lblLink').live('click', function () {
        var filterExpression = "IsDeleted = 0";
        openSearchDialog('diagnose', filterExpression, function (value) {
            $('#<%:txtDiagnosePostCode.ClientID %>').val(value);
            ontxtDiagnosePostCodeNoChanged(value);
        });
    });

    $('#<%:txtDiagnosePostCode.ClientID %>').live('change', function () {
        ontxtDiagnosePostCodeNoChanged($(this).val());
    });

    function ontxtDiagnosePostCodeNoChanged(value) {
        var filterExpression = "IsDeleted = 0 AND DiagnoseID = '" + value + "'";
        Methods.getObject('GetDiagnoseList', filterExpression, function (result) {
            if (result != null) {
                $('#<%:txtDiagnosePostCode.ClientID %>').val(result.DiagnoseID);
                $('#<%:txtDiagnosePostName.ClientID %>').val(result.DiagnoseName);
                $('#<%:hdnDiagnosePostName.ClientID %>').val(result.DiagnoseName);
            }
            else {
                $('#<%:txtDiagnosePostCode.ClientID %>').val('');
                $('#<%:txtDiagnosePostName.ClientID %>').val('');
                $('#<%:hdnDiagnosePostName.ClientID %>').val('');
            }
        });
    }
    //#endregion

    $('#<%=txtHemorrhage.ClientID %>').change(function () {
        $(this).trigger('changeValue');
        var value = parseDecimal($(this).attr('hiddenVal'));
        $('#<%=txtHemorrhage.ClientID %>').val(value).trigger('changeValue');
    });
</script>
<input type="hidden" id="hdnIsAdd" runat="server" value="" />
<input type="hidden" id="hdnDiagnosePreName" runat="server" value="" />
<input type="hidden" id="hdnDiagnosePostName" runat="server" value="" />
<input type="hidden" id="hdnPatientSuregncyID" runat="server" value="" />
<input type="hidden" id="hdnTransactionHdID" runat="server" value="" />
<input type="hidden" id="hdnTransactionDtID" runat="server" value="" />
<input type="hidden" id="hdnHealthcvareServiceUnitOK" runat="server" value="" />
<input type="hidden" id="hdnMRN" runat="server" value="" />
<input type="hidden" id="hdnVisitID" runat="server" value="" />
<div class="toolbarArea">
    <ul>
        <li runat="server" id="btnMPEntryPopupSave">
            <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbsave.png")%>' alt="" /><div>
                <%=GetLabel("Save")%></div>
        </li>
    </ul>
</div>
<table class="tblContentArea">
    <tr>
        <td style="padding: 5px; vertical-align: top">
            <div style="width: 1250px; height: 250px; overflow-y: scroll;">
                <fieldset id="fsEntryPopup" style="margin: 0">
                    <table class="tblEntryContent" style="width: 100%">
                        <colgroup>
                            <col style="width: 40%" />
                            <col style="width: 60%" />
                        </colgroup>
                        <tr>
                            <td style="padding: 5px; vertical-align: top">
                                <table>
                                    <colgroup>
                                        <col style="width: 250px" />
                                        <col />
                                    </colgroup>
                                    <tr>
                                        <td class="tdLabel">
                                            <label>
                                                <%=GetLabel("Waktu Masuk")%></label>
                                        </td>
                                        <td>
                                            <table width="220">
                                                <colgroup>
                                                    <col style="width: 250px" />
                                                    <col />
                                                </colgroup>
                                                <tr>
                                                    <td>
                                                        <asp:textbox id="txtStartDate" width="120px" runat="server" cssclass="datepicker required" />
                                                    </td>
                                                    <td style="padding-left: 5px">
                                                        <asp:textbox id="txtStartHour" cssclass="time required" runat="server" width="60px"
                                                            style="text-align: center" maxlength="5" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tdLabel">
                                            <label>
                                                <%=GetLabel("Waktu Selesai")%></label>
                                        </td>
                                        <td>
                                            <table width="220">
                                                <colgroup>
                                                    <col style="width: 250px" />
                                                    <col />
                                                </colgroup>
                                                <tr>
                                                    <td>
                                                        <asp:textbox id="txtEndDate" width="120px" runat="server" cssclass="datepicker required" />
                                                    </td>
                                                    <td style="padding-left: 5px">
                                                        <asp:textbox id="txtEndHour" cssclass="time required" runat="server" width="60px"
                                                            style="text-align: center" maxlength="5" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tdLabel">
                                            <label>
                                                <%=GetLabel("Sifat Operasi")%></label>
                                        </td>
                                        <td colspan="2">
                                            <asp:radiobuttonlist id="rblEmergency" runat="server" repeatdirection="Horizontal">
                                                <asp:listitem text="Elektif" value="1" selected="True" />
                                                <asp:listitem text="Cito" value="0" />
                                            </asp:radiobuttonlist>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tdLabel">
                                            <label>
                                                <%=GetLabel("Penandaan Lokasi Operasi")%></label>
                                        </td>
                                        <td>
                                            <dxe:ASPxComboBox ID="cboSurgeryMark" ClientInstanceName="cboSurgeryMark" Width="170px"
                                                runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tdLabel">
                                            <label>
                                                <%=GetLabel("Klasifikasi Luka")%></label>
                                        </td>
                                        <td>
                                            <dxe:ASPxComboBox ID="cboWoundType" ClientInstanceName="cboWoundType" Width="170px"
                                                runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tdLabel">
                                            <label>
                                                <%=GetLabel("Jenis Anestesi")%></label>
                                        </td>
                                        <td>
                                            <dxe:ASPxComboBox ID="cboAnestasionType" ClientInstanceName="cboAnestasionType" Width="170px"
                                                runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tdLabel">
                                            <label>
                                                <%=GetLabel("Komplikasi Anestesi")%></label>
                                        </td>
                                        <td>
                                            <dxe:ASPxComboBox ID="cboAnestasionComplication" ClientInstanceName="cboAnestasionComplication"
                                                Width="170px" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tdLabel">
                                            <label>
                                                <%=GetLabel("Sebab Mundurnya Operasi > 60 menit")%></label>
                                        </td>
                                        <td>
                                            <dxe:ASPxComboBox ID="cboSurgeryDelayCause" ClientInstanceName="cboSurgeryDelayCause"
                                                Width="170px" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tdLabel">
                                            <label>
                                                <%=GetLabel("Pencegahan Pasien Jatuh")%></label>
                                        </td>
                                        <td>
                                            <table width="220" border="1" bgcolor="#ffd27a">
                                                <colgroup>
                                                    <col style="width: 250px" />
                                                    <col />
                                                </colgroup>
                                                <tr>
                                                    <td bgcolor="#F5F5F5">
                                                        <asp:checkbox id="chkIsTableWheelLocked" runat="server" /><%:GetLabel("Roda Meja Operasi Terkunci")%>
                                                        <asp:checkbox id="chkIsUseBedBound" runat="server" /><%:GetLabel("Selft Bed Terpasang")%>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tdLabel">
                                            <label id="lblItemTransaksi" class="lblLink lblMandatory" runat="server">
                                                <%=GetLabel("Item")%></label>
                                        </td>
                                        <td>
                                            <asp:textbox id="txtItemTransaction" width="215px" cssclass="required" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td style="padding: 5px; vertical-align: top">
                                <table>
                                    <colgroup>
                                        <col style="width: 45%" />
                                        <col style="width: 55%" />
                                    </colgroup>
                                    <tr>
                                        <td style="padding: 5px; vertical-align: top">
                                            <table>
                                                <colgroup>
                                                    <col style="width: 120px" />
                                                    <col />
                                                </colgroup>
                                                <tr>
                                                    <td class="tdLabel">
                                                        <label id="lblDrain" class="lblNormal" runat="server">
                                                            <%=GetLabel("Drain")%></label>
                                                    </td>
                                                    <td>
                                                        <asp:radiobuttonlist id="rblBloodDrain" runat="server" repeatdirection="horizontal">
                                                            <asp:listitem text="Ya" value="1" selected="True" />
                                                            <asp:listitem text="Tidak" value="0" />
                                                        </asp:radiobuttonlist>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="tdLabel">
                                                        <label id="LblPendarahan" class="lblNormal" runat="server">
                                                            <%=GetLabel("Pendarahan(cc)")%></label>
                                                    </td>
                                                    <td>
                                                        <asp:textbox id="txtHemorrhage" width="120px" cssclass="txtCurrency" runat="server" />
                                                    </td>
                                                    <td>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="tdLabel">
                                                        <label id="lblTampon" class="lblNormal" runat="server">
                                                            <%=GetLabel("Tampon")%></label>
                                                    </td>
                                                    <td>
                                                        <asp:radiobuttonlist id="rblUsingTampon" runat="server" repeatdirection="horizontal">
                                                            <asp:listitem text="Ya" value="1" selected="True" />
                                                            <asp:listitem text="Tidak" value="0" />
                                                        </asp:radiobuttonlist>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="tdLabel">
                                                        <label id="lblCatheter" class="lblNormal" runat="server">
                                                            <%=GetLabel("Catheter")%></label>
                                                    </td>
                                                    <td>
                                                        <asp:radiobuttonlist id="rblUsingCatheter" runat="server" repeatdirection="horizontal">
                                                            <asp:listitem text="Ya" value="1" selected="True" />
                                                            <asp:listitem text="Tidak" value="0" />
                                                        </asp:radiobuttonlist>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="tdLabel">
                                                        <label id="Label1" class="lblNormal" runat="server">
                                                            <%=GetLabel("Catheter From")%></label>
                                                    </td>
                                                    <td>
                                                        <asp:radiobuttonlist id="rblUsingCatheterFromWard" runat="server" repeatdirection="horizontal">
                                                            <asp:listitem text="OK" value="1" selected="True" />
                                                            <asp:listitem text="Ruangan" value="0" />
                                                        </asp:radiobuttonlist>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="tdLabel">
                                                        <label id="lblSpecimen" class="lblNormal" runat="server">
                                                            <%=GetLabel("Specimen")%></label>
                                                    </td>
                                                    <td>
                                                        <asp:radiobuttonlist id="rblSpecimen" runat="server" repeatdirection="horizontal">
                                                            <asp:listitem text="Ya" value="1" selected="True" />
                                                            <asp:listitem text="Tidak" value="0" />
                                                        </asp:radiobuttonlist>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="tdLabel">
                                                        <label id="lblPA" class="lblNormal" runat="server">
                                                            <%=GetLabel("PA")%></label>
                                                    </td>
                                                    <td>
                                                        <asp:radiobuttonlist id="rblTestPA" runat="server" repeatdirection="horizontal">
                                                            <asp:listitem text="Ya" value="1" selected="True" />
                                                            <asp:listitem text="Tidak" value="0" />
                                                        </asp:radiobuttonlist>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="tdLabel">
                                                        <label id="lblKultur" class="lblNormal" runat="server">
                                                            <%=GetLabel("Kultur")%></label>
                                                    </td>
                                                    <td>
                                                        <asp:radiobuttonlist id="rblTestKultur" runat="server" repeatdirection="horizontal">
                                                            <asp:listitem text="Ya" value="1" selected="True" />
                                                            <asp:listitem text="Tidak" value="0" />
                                                        </asp:radiobuttonlist>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="tdLabel">
                                                        <label id="LblPakaiImplant" class="lblNormal" runat="server">
                                                            <%=GetLabel("Pasang Implant")%></label>
                                                    </td>
                                                    <td>
                                                        <asp:radiobuttonlist id="rblUsingImplant" runat="server" repeatdirection="horizontal">
                                                            <asp:listitem text="Ya" value="1" selected="True" />
                                                            <asp:listitem text="Tidak" value="0" />
                                                        </asp:radiobuttonlist>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="tdLabel">
                                                        <label id="lblAntibiotik" class="lblNormal" runat="server">
                                                            <%=GetLabel("Antibiotik Intra")%></label>
                                                    </td>
                                                    <td>
                                                        <asp:radiobuttonlist id="rblUsingAntibiotics" runat="server" repeatdirection="horizontal">
                                                            <asp:listitem text="Ya" value="1" selected="True" />
                                                            <asp:listitem text="Tidak" value="0" />
                                                        </asp:radiobuttonlist>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="tdLabel">
                                                        <label id="lblRawat" class="lblNormal" runat="server">
                                                            <%=GetLabel("Rawat ICU/HDU")%></label>
                                                    </td>
                                                    <td>
                                                        <asp:radiobuttonlist id="rblTransferedToICU" runat="server" repeatdirection="horizontal">
                                                            <asp:listitem text="Ya" value="1" selected="True" />
                                                            <asp:listitem text="Tidak" value="0" />
                                                        </asp:radiobuttonlist>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="tdLabel">
                                                        &nbsp;
                                                    </td>
                                                    <td>
                                                        <table width="180" border="1" bgcolor="#ffd27a">
                                                            <colgroup>
                                                                <col style="width: 180px" />
                                                                <col />
                                                            </colgroup>
                                                            <tr>
                                                                <td bgcolor="#F5F5F5">
                                                                    <asp:radiobuttonlist id="rblSchedulleTransferToICU" runat="server" repeatdirection="vertical">
                                                                        <asp:listitem text="Terencana" value="1" selected="True" />
                                                                        <asp:listitem text="Tidak Terencana" value="0" />
                                                                    </asp:radiobuttonlist>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                </tr>
                                            </table>
                                        </td>
                                        <td style="padding: 5px; vertical-align: top">
                                            <table>
                                                <colgroup>
                                                    <col style="width: 50%" />
                                                    <col style="width: 50%" />
                                                </colgroup>
                                                <tr>
                                                    <td style="padding: 5px; vertical-align: top">
                                                        <table>
                                                            <colgroup>
                                                                <col style="width: 80px" />
                                                                <col style="width: 150px" />
                                                            </colgroup>
                                                            <tr id="trDrain">
                                                                <td style="vertical-align: top">
                                                                    <label id="lblJenisDrain" class="lblNormal" runat="server">
                                                                        <%=GetLabel("Jenis")%></label>
                                                                </td>
                                                                <td>
                                                                    <asp:textbox id="txtOtherBloodDrainType" width="150px" runat="server" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td style="vertical-align: top">
                                                                    &nbsp;
                                                                </td>
                                                            </tr>
                                                            <tr id="trTampon">
                                                                <td style="vertical-align: top">
                                                                    <label id="lblJenisTampon" class="lblNormal" runat="server">
                                                                        <%=GetLabel("Jenis")%></label>
                                                                </td>
                                                                <td>
                                                                    <asp:textbox id="txtTamponType" width="150px" runat="server" />
                                                                </td>
                                                            </tr>
                                                            <tr id="trCatheter">
                                                                <td style="vertical-align: top">
                                                                    <label id="lblCatheterNo" class="lblNormal" runat="server">
                                                                        <%=GetLabel("No")%></label>
                                                                </td>
                                                                <td>
                                                                    <asp:textbox id="txtIsCatherterType" width="150px" runat="server" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    &nbsp;
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                </td>
                                                            </tr>
                                                            <tr id="trSpecimen">
                                                                <td class="tdLabel">
                                                                    <label id="lblSpecimenJenis" class="lblNormal" runat="server">
                                                                        <%=GetLabel("Jenis")%></label>
                                                                </td>
                                                                <td>
                                                                    <dxe:ASPxComboBox ID="cboSpecimenType" ClientInstanceName="cboSpecimenType"
                                                                        Width="150px" runat="server" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    &nbsp;
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    &nbsp;
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    &nbsp;
                                                                </td>
                                                            </tr>
                                                            <tr id="trPasangImplan">
                                                                <td class="tdLabel">
                                                                    <label id="lblPasangImplanJenis" class="lblNormal" runat="server">
                                                                        <%=GetLabel("Jenis")%></label>
                                                                </td>
                                                                <td>
                                                                    <asp:textbox id="txtImplantType" width="150px" runat="server" />
                                                                </td>
                                                            </tr>
                                                            <tr id="trAntibiotik">
                                                                <td class="tdLabel">
                                                                    <label id="lblAntibiotikJenis" class="lblNormal" runat="server">
                                                                        <%=GetLabel("Jenis")%></label>
                                                                </td>
                                                                <td>
                                                                    <asp:textbox id="txtAntibioticsType" width="150px" runat="server" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                    <td style="padding: 5px; vertical-align: top">
                                                        <table>
                                                            <colgroup>
                                                                <col style="width: 80px" />
                                                                <col style="width: 100px" />
                                                            </colgroup>
                                                            <tr>
                                                                <td>
                                                                    &nbsp;
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    &nbsp;
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    &nbsp;
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <tr>
                                                                    <td>
                                                                        &nbsp;
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        &nbsp;
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        &nbsp;
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <tr>
                                                                        <td>
                                                                            &nbsp;
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            &nbsp;
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            &nbsp;
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            &nbsp;
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            &nbsp;
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td class="tdLabel">
                                                                            <label id="lblJam" class="lblNormal" runat="server">
                                                                                <%=GetLabel("Jam (WIB)")%></label>
                                                                        </td>
                                                                        <td>
                                                                            <asp:textbox id="txtAntibiotikHour" cssclass="time" width="50px" runat="server" maxlength="5" />
                                                                        </td>
                                                                    </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
            </div>
        </td>
    </tr>
    <tr>
        <td>
            <div class="containerUlTabPage">
                <ul class="ulTabPage" id="ulTabOperative">
                    <li class="selected" contentid="ctPre">
                        <%=GetLabel("Pre Operative")%></li>
                    <li contentid="ctPost">
                        <%=GetLabel("Post Operative")%></li>
                </ul>
            </div>
            <div id="ctPre" class="ctOperativeDt">
                <table class="tblEntryContent" style="width: 100%">
                    <colgroup>
                        <col style="width: 100px" />
                        <col />
                    </colgroup>
                    <tr>
                        <td class="tdLabel">
                            <label id="lblDiagnosePre" class="lblLink" runat="server">
                                <%=GetLabel("Diagnosa (Pre)")%></label>
                        </td>
                        <td>
                            <table style="width: 100%" cellpadding="0" cellspacing="0">
                                <colgroup>
                                    <col style="width: 1px" />
                                    <col style="width: 5px" />
                                    <col style="width: 100%" />
                                    <col />
                                </colgroup>
                                <tr>
                                    <td>
                                        <asp:textbox id="txtDiagnosePreCode" width="50px" runat="server" />
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                        <asp:textbox id="txtDiagnosePreName" width="940px" readonly="true" runat="server" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label id="lblRemarksPre" class="lblNormal" runat="server">
                                <%=GetLabel("Remarks (Pre)")%></label>
                        </td>
                        <td>
                            <asp:textbox id="txtRemarksPre" width="1000px" runat="server" textmode="MultiLine"
                                rows="8" />
                        </td>
                    </tr>
                </table>
            </div>
            <div id="ctPost" style="display: none" class="ctOperativeDt">
                <table class="tblEntryContent" style="width: 100%">
                    <colgroup>
                        <col style="width: 100px" />
                        <col />
                    </colgroup>
                    <tr>
                        <td class="tdLabel">
                            <label id="lblDiagnosaPost" class="lblLink" runat="server">
                                <%=GetLabel("Diagnosa (Post)")%></label>
                        </td>
                        <td>
                            <table style="width: 100%" cellpadding="0" cellspacing="0">
                                <colgroup>
                                    <col style="width: 1px" />
                                    <col style="width: 5px" />
                                    <col style="width: 100%" />
                                    <col />
                                </colgroup>
                                <tr>
                                    <td>
                                        <asp:textbox id="txtDiagnosePostCode" width="50px" runat="server" />
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                        <asp:textbox id="txtDiagnosePostName" width="940px" readonly="true" runat="server" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label id="lblRemarksPost" class="lblNormal" runat="server">
                                <%=GetLabel("Remarks (Post)")%></label>
                        </td>
                        <td>
                            <asp:textbox id="txtRemarksPost" width="1000px" runat="server" textmode="MultiLine"
                                rows="8" />
                        </td>
                    </tr>
                </table>
            </div>
        </td>
    </tr>
</table>
<div style="display: none">
    <dxcp:ASPxCallbackPanel ID="cbpProcessSave" runat="server" Width="100%" ClientInstanceName="cbpProcessSave"
        ShowLoadingPanel="false" OnCallback="cbpProcessSave_Callback">
        <ClientSideEvents BeginCallback="function(s,e) { showLoadingPanel(); }" EndCallback="function(s,e) { oncbpProcessSaveEndCallback(s); }" />
    </dxcp:ASPxCallbackPanel>
</div>
