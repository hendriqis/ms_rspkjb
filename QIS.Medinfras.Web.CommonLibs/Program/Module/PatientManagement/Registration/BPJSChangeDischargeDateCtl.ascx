<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="BPJSChangeDischargeDateCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.BPJSChangeDischargeDateCtl" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<div class="toolbarArea">
    <ul>
        
        <li id="btnMPDischargePatientCtl">
            <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbsave.png")%>' alt="" /><div>
                <%=GetLabel("Tanggal Pulang")%></div>
        </li>
        
        <li id="btnMPEntryPopupAppointmentSuratRujukan" style="display:none;">
            <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbprint.png")%>' alt="" /><div id="divBtnSuratRujukan" runat="server">
            </div>
        </li>
        <li id="btnMPEntryPopupPrintSPRI"  style="display:none;">
            <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbprint.png")%>' alt="" /><div>
                <%=GetLabel("SPRI")%></div>
        </li>
        
    </ul>
</div>
<script type="text/javascript" id="dxss_generatesepctl">
    setDatePicker('<%=txtDischargeDateCtl.ClientID %>');
    setDatePicker('<%=txtDateOfDeath.ClientID %>');
    //#region Discharge Patient
    $('#btnMPDischargePatientCtl').click(function () {
        if ($('#<%=txtNoSEP.ClientID %>').val() != '') {
            var filterExpression = "RegistrationID = '" + $('#<%=hdnID.ClientID %>').val() + "'";
            Methods.getObject('GetRegistrationBPJSList', filterExpression, function (result) {
                if (result != null) {
                    var noSEPTemp = result.NoSEP;
                    var noSEP = $('#<%=txtNoSEP.ClientID %>').val();
                    if (noSEPTemp == noSEP) {
                        if ($('#<%=hdnIsBridgingBPJSVClaimVersion.ClientID %>').val() == Constant.VersionBridgingBPJSVClaim.v1_0) {
                            var date = Methods.getDatePickerDate($('#<%=txtDischargeDateCtl.ClientID %>').val());
                            var tglPulang = Methods.dateToYMD(date) + ' 23 : 59 ';


                            BPJSService.updateTglPlg(noSEP, tglPulang, function (result) {
                                try {
                                    var obj = result.split('|');
                                    if (obj[0] == "1") {
                                        showToast('UPDATE TANGGAL PULANG : SUKSES', 'Update Tanggal Pulang Pasien BPJS dengan No. ' + obj[1] + ' Berhasil');
                                    }
                                    else {
                                        showToast('UPDATE TANGGAL PULANG : GAGAL', 'Error Message : ' + obj[2]);
                                    }

                                } catch (err) {
                                    alert(err);
                                }
                            });
                        } else {
                            var statusPulang = $('#<%=txtStatusPulangCode.ClientID %>').val();
                            var noSuratMeninggal = $('#<%=txtNoSuratMeninggal.ClientID %>').val();
                            var dateMeninggal = Methods.getDatePickerDate($('#<%=txtDateOfDeath.ClientID %>').val());
                            var tglMeninggal = Methods.dateToYMD(dateMeninggal);
                            var datePulang = Methods.getDatePickerDate($('#<%=txtDischargeDateCtl.ClientID %>').val());
                            var tglPulang = Methods.dateToYMD(datePulang);
                            var noLPManual = $('#<%=txtNoLPManual.ClientID %>').val();
                            alert('ok');
                            BPJSService.updateTglPlgAPI(noSEP, statusPulang, noSuratMeninggal, tglMeninggal, tglPulang, noLPManual, function (result) {
                                try {
                                    var obj = result.split('|');
                                    if (obj[0] == "1") {
                                        cbpPopupProcess.PerformCallback('discharge');
                                        showToast('UPDATE TANGGAL PULANG : SUKSES', 'Update Tanggal Pulang Pasien BPJS dengan No. ' + noSEP + ' Berhasil');
                                    }
                                    else {
                                        showToast('UPDATE TANGGAL PULANG : GAGAL', 'Error Message : ' + obj[2]);
                                    }

                                } catch (err) {
                                    alert(err);
                                }
                            });
                        }
                    }
                    else {
                        showToast('Warning', 'Anda tidak bisa memulangkan SEP pasien registrasi lain.');
                    }
                }
                else {
                    showToast('Warning', 'Anda tidak bisa memulangkan SEP pasien registrasi lain.');
                }
            });
        }
        else {
            showToast('Warning', 'Nomor SEP harus terisi untuk bisa melakukan proses ini!');
        }
    });


    function onCbpPopupProcesEndCallback(s) {
        hideLoadingPanel();
        var param = s.cpResult.split('|');
        if (param[0] == 'save' || param[0] == 'delete' || param[0] == 'newClaim') {
            reinitMPButton();
            if (param[1] == 'fail')
                showToast('Save Failed', 'Error Message : ' + param[2]);
        }
        else {
            var code = $('#hdnRightPanelContentCode').val();
            if (code == '') {
                code = 'generateSEP';
            }

            onAfterSaveRightPanelContent(code, '', '');
        }
    }

    //#region Status Pulang
    $('#lblStatusPulang.lblLink').live('click', function () {
        openSearchDialog('vBPJSReferenceCaraKeluar', '', function (value) {
            $('#<%=txtStatusPulangCode.ClientID %>').val(value);
            ontxtvKlaimStatusPulangChanged(value);
        });
    });

    $('#<%=txtStatusPulangCode.ClientID %>').live('change', function () {
        ontxtvKlaimStatusPulangChanged($(this).val());
    });

    function ontxtvKlaimStatusPulangChanged(value) {
        var filterExpression = "BPJSCode = '" + value + "'";
        Methods.getObject('GetvBPJSReferenceCaraKeluarList', filterExpression, function (result) {
            if (result != null) {
                $('#<%=hdnStatusPulang.ClientID %>').val(result.BPJSCode);
                $('#<%=txtStatusPulangCode.ClientID %>').val(result.BPJSCode);
                $('#<%=txtStatusPulangName.ClientID %>').val(result.BPJSName);
            }
            else {
                $('#<%=hdnStatusPulang.ClientID %>').val('');
                $('#<%=txtStatusPulangCode.ClientID %>').val('');
                $('#<%=txtStatusPulangName.ClientID %>').val('');
            }
        });
    }
</script>
<div style="height: 100%">
    <input type="hidden" runat="server" id="hdnIsAdd" value="" />
    <input type="hidden" runat="server" id="hdnMRN" value="" />
    <input type="hidden" runat="server" id="hdnRegistrationNo" value="" />
    <input type="hidden" runat="server" id="hdnAppointmentID" value="" />
    <input type="hidden" runat="server" id="hdnVisitID" value="" />
    <input type="hidden" runat="server" id="hdnHealthcareServiceUnitID" value="" />
    <input type="hidden" runat="server" id="hdnParamedicID" value="" />
    <input type="hidden" runat="server" id="hdnVisitTypeID" value="" />
    <input type="hidden" runat="server" id="hdnGCCostumerType" value="" />
    <input type="hidden" runat="server" id="hdnGCTariffSchemePersonal" value="" />
    <input type="hidden" runat="server" id="hdnGCTariffScheme" value="" />
    <input type="hidden" runat="server" id="hdnPayerID" value="" />
    <input type="hidden" runat="server" id="hdnContractID" value="" />
    <input type="hidden" runat="server" id="hdnCoverageTypeID" value="" />
    <input type="hidden" runat="server" id="hdnParticipantNo" value="" />
    <input type="hidden" runat="server" id="hdnIsCoverageLimitPerDay" value="" />
    <input type="hidden" runat="server" id="hdnIsControlClassCare" value="" />
    <input type="hidden" runat="server" id="hdnControlClassCare" value="" />
    <input type="hidden" runat="server" id="hdnEmployeeID" value="" />
    <input type="hidden" runat="server" id="hdnID" value="" />
    <input type="hidden" runat="server" id="hdnKdJenisPeserta" value="" />
    <input type="hidden" runat="server" id="hdnKdKelas" value="" />
    <input type="hidden" runat="server" id="hdnAsalRujukan" value="1" />
    <input type="hidden" runat="server" id="hdnIsBpjsRegistrationCtl" value="" />
    <input type="hidden" runat="server" id="hdnIsPoliExecutive" value="0" />
    <input type="hidden" runat="server" id="hdnBPJSReferenceInfoKodeUnit" value="1" />
    <input type="hidden" runat="server" id="hdnBPJSReferenceInfoNamaUnit" value="1" />
    <input type="hidden" runat="server" id="hdnNoSKDP" value="" />
    <input type="hidden" runat="server" id="hdnNoSKDManual" value="" />
    <input type="hidden" runat="server" id="hdnKodeDPJP" value="" />
    <input type="hidden" runat="server" id="hdnStatusPulang" value="" />
    <input type="hidden" runat="server" id="hdnIsBridgingBPJSVClaimVersion" value="" />
    <input type="hidden" runat="server" id="hdnDepartmentID" value="" />
    <input type="hidden" runat="server" id="hdnKelasNaikBPJS" value="" />
    <input type="hidden" runat="server" id="hdnHealthcareInitial" value="" />
    <input type="hidden" runat="server" id="hdnLstTujuanKunjungan" value="" />
    <input type="hidden" runat="server" id="hdnLstProsedur" value="" />
    <input type="hidden" runat="server" id="hdnLstProsedurPenunjang" value="" />
    <input type="hidden" runat="server" id="hdnLstAsesmenPelayanan" value="" />
    <input type="hidden" runat="server" id="hdnIsUsedReferenceQueueNo" value="" />
    <input type="hidden" runat="server" id="hdnIsCreateAppointmentAfterCreateNoSurkon"
        value="" />
    <input type="hidden" runat="server" id="hdnIsBridgingEklaim" value="" />
    <input type="hidden" runat="server" id="hdnIsSendEKlaimMedicalNo" value="" />
    <input type="hidden" runat="server" id="hdnKodePoli" value="" />
    <table class="tblContentArea" width="100%">
        <colgroup>
            <col width="100%" />
        </colgroup>
        <tr>
            <td valign="top">
                <table>
                    

                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Tanggal SEP")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtTglSEP" Width="120px" runat="server" CssClass="datepicker" ReadOnly />
                        </td>
                        <td>
                            <asp:TextBox ID="txtJamSEP" Width="100px" runat="server" CssClass="time" ReadOnly />
                        </td>
                       
                    </tr>
                     
                    <tr>
                        <td class="tdLabel">
                            <label>
                                <%=GetLabel("No. SEP")%></label>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtNoSEP" runat="server" Width="99%" ReadOnly>
                            </asp:TextBox>
                        </td>
                        
                    </tr>
                     
                </table>
            </td>
             
        </tr>
    </table>
    <div id="divSEPContent" style="height: 445px; overflow-y: scroll;" top="auto">
        <table class="tblContentArea" width="100%">
            <colgroup>
                <col width="100%" />
            </colgroup>
            <tr>
                <td style="padding: 5px; vertical-align: top">
            
                    <h4 class="h4expanded">
                        <%=GetLabel("DATA PASIEN PULANG :")%></h4>
                    <div class="containerTblEntryContent">
                        <table class="tblEntryContent" style="width: 100%">
                            <colgroup>
                                <col width="200px" />
                                <col width="100px" />
                                <col />
                            </colgroup>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblLink" id="lblStatusPulang">
                                        <%=GetLabel("Status Pulang") %></label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtStatusPulangCode" runat="server" Width="100px" />
                                </td>
                                <td colspan="2">
                                    <asp:TextBox ID="txtStatusPulangName" ReadOnly="true" runat="server" Width="160px" />
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblNormal">
                                        <%=GetLabel("Tanggal Pulang")%></label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtDischargeDateCtl" Width="80px" runat="server" CssClass="datepicker" />
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblNormal">
                                        <%=GetLabel("Tanggal Meninggal")%></label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtDateOfDeath" Width="80px" runat="server" CssClass="datepicker" />
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel" style="vertical-align: top">
                                    <label class="lblNormal">
                                        <%=GetLabel("No. Surat Meninggal")%></label>
                                </td>
                                <td colspan="3">
                                    <table border="0" cellpadding="0" cellspacing="1">
                                        <tr>
                                            <td>
                                                <asp:TextBox ID="txtNoSuratMeninggal" runat="server" Width="175px" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel" style="vertical-align: top">
                                    <label class="lblNormal">
                                        <%=GetLabel("No. LP Manual")%></label>
                                </td>
                                <td colspan="3">
                                    <table border="0" cellpadding="0" cellspacing="1">
                                        <tr>
                                            <td>
                                                <asp:TextBox ID="txtNoLPManual" runat="server" Width="175px" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </div>
                </td>
               
            </tr>
        </table>
    </div>
      <dxcp:ASPxCallbackPanel ID="cbpPopupProcess" runat="server" Width="100%" ClientInstanceName="cbpPopupProcess"
        ShowLoadingPanel="false" OnCallback="cbpPopupProcess_Callback">
        <ClientSideEvents BeginCallback="function(s,e) { showLoadingPanel(); }" EndCallback="function(s,e) { onCbpPopupProcesEndCallback(s); }" />
    </dxcp:ASPxCallbackPanel>
</div>
