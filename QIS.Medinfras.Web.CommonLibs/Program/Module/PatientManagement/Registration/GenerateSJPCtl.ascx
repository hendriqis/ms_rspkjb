<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="GenerateSJPCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.GenerateSJPCtl" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<div class="toolbarArea">
    <ul>
        <li id="btnSimpan"style="display: none">
            <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbsave.png")%>' alt="" /><div>
                <%=GetLabel("Simpan")%></div>
        </li>
        <li id="btnMPGenerateSJPCtl"style="display: none">
            <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbprocess.png")%>' alt="" /><div>
                <%=GetLabel("Generate SJP")%></div>
        </li>
        <li id="btnMPCetakSJPCtl"style="display: none">
            <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbprint.png")%>' alt="" /><div>
                <%=GetLabel("Cetak")%></div>
        </li>
        <li id="btnMPUpdateTanggalPulangCtl"style="display: none">
            <img src='<%=ResolveUrl("~/Libs/Images/Icon/close.png")%>' alt="" /><div>
                <%=GetLabel("Tanggal Pulang")%></div>
        </li>
        <li id="btnBack" style="display: none">
            <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbback.png")%>' alt="" /><div>
                <%=GetLabel("Kembali")%></div>
        </li>
        <li id="btnMPHapusSJPCtl"style="display: none">
            <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbvoid.png")%>' alt="" /><div>
                <%=GetLabel("Hapus")%></div>
        </li>
    </ul>
</div>
<script type="text/javascript" id="dxss_patiententryctl">
    setDatePicker('<%=txtTglSJP.ClientID %>');
    setDatePicker('<%=txtTglAsalRujukan.ClientID %>');
    setDatePicker('<%=txtTanggalPulang.ClientID %>');
    $('#<%:txtTglSJP.ClientID %>').datepicker('option', 'maxDate', '0');
    $('#<%:txtTglAsalRujukan.ClientID %>').datepicker('option', 'maxDate', '0');
    $('#<%:txtTanggalPulang.ClientID %>').datepicker('option', 'maxDate', '0');
//    $('.trJenisPelayanan').hide();
    reinitMPButton();

    function reinitMPButton() {
        if ($('#<%=txtNoSJP.ClientID %>').val() != '') {
            $('#btnSimpan').css('display', '');
            $('#btnMPGenerateSJPCtl').css('display', 'none');
            $('#btnMPCetakSJPCtl').css('display', '');
            $('#btnMPHapusSJPCtl').css('display', '');
            $('#btnMPUpdateTanggalPulangCtl').css('display', '');

            $('#btnSearchPeserta').css('display', 'none');
            $('#btnSearchNoRujukan').css('display', 'none');
            $('#<%=txtNoSJP.ClientID %>').attr('readonly', 'readonly');
        }
        else {
            $('#btnSimpan').css('display', 'none');
            $('#btnMPGenerateSJPCtl').css('display', '');
            $('#btnMPCetakSJPCtl').css('display', 'none');
            $('#btnMPHapusSJPCtl').css('display', 'none');
            $('#btnMPUpdateTanggalPulangCtl').css('display', 'none');

            $('#btnSearchPeserta').css('display', '');
            $('#btnSearchNoRujukan').css('display', '');
            $('#<%=txtNoSJP.ClientID %>').removeAttr('readonly');
        }

        if ($('#<%=hdnDepartmentID.ClientID %>').val() != "INPATIENT") {
            $('#btnMPUpdateTanggalPulangCtl').css('display', 'none');
        }
    }

    //#region Eligibilitas Peserta
    $("#btnSearchPeserta").on("click", function (e) {
        e.preventDefault();
        if ($('#<%=hdnIsBridgingToInhealth.ClientID %>').val() != '0') {
            if ($('#<%=txtNoKartuInh.ClientID %>').val() == '')
                showToast('Warning', "Nomor Kartu Inhealth harus diisi!");
            else {
                var token = $('#<%=hdnTokenInhealth.ClientID %>').val();
                var kodeprovider = $('#<%=hdnKodeProviderInhealth.ClientID %>').val();
                var nokainhealth = $('#<%=txtNoKartuInh.ClientID %>').val();
                var date = Methods.getDatePickerDate($('#<%=txtTglSJP.ClientID %>').val());
                var tglpelayanan = Methods.dateToYMD(date);
                var jenispelayanan = cboJenisPelayanan.GetValue();
                var poli = $('#<%=txtKodePoli.ClientID %>').val();
                var username = $('#<%=hdnUsernameLogin.ClientID %>').val();
                if (jenispelayanan != "0") {
                    InhealthService.eligibilitaspeserta_API(nokainhealth, tglpelayanan, jenispelayanan, poli, function (result) {
                        GetPesertaHandler(result);
                    });
                }
                else {
                    showToast('WARNING', 'Harap pilih jenis pelayanan pasien Inhealth');
                }
            }
        }
        else {
            showToast('WARNING', 'Tidak bridging dengan Inhealth');
        }
    });

    function GetPesertaHandler(result) {
        try {
            var resp = result.split('|');
            if (resp[0] == "1") {
                var obj = jQuery.parseJSON(resp[2]);
                var errorCode = obj.ERRORCODE;
                if (errorCode == '00') {
                    $('#<%=hdnCheckStatusPeserta.ClientID %>').val("1");
                    displayMessageBox('STATUS ELIGIBILITAS PESERTA : SUKSES', 'Cek Peserta dengan nama ' + obj.NMPST + ' (' + obj.NOKAPST + ') berhasil.');
                    $('#<%=txtNamaPeserta.ClientID %>').val(obj.NMPST);
                    $('#<%=txtKodeBadanUsaha.ClientID %>').val(obj.KODEBADANUSAHA);
                    $('#<%=txtBadanUsaha.ClientID %>').val(obj.NAMABADANUSAHA);
                    $('#<%=txtNoBPJS.ClientID %>').val(obj.NOKAPSTBPJS);
                    var year = obj.TGLLAHIR.substring(0, 4);
                    var month = obj.TGLLAHIR.substring(5, 7);
                    var day = obj.TGLLAHIR.substring(8, 10);
                    var dob = year + month + day;
                    var date = Methods.stringToDate(dob);
                    $('#<%=txtDOB.ClientID %>').val(Methods.dateToDMY(date)).change();
                    $('#<%=txtKodeProduk.ClientID %>').val(obj.KODEPRODUK);
                    $('#<%=txtNamaProduk.ClientID %>').val(obj.NAMAPRODUK);
                    $('#<%=txtKodeKelasRawat.ClientID %>').val(obj.KODEKELASRAWAT);
                    $('#<%=txtKelasRawat.ClientID %>').val(obj.NAMAKELASRAWAT);
                    $('#<%=txtKodeProvider.ClientID %>').val(obj.KODEPROVIDER);
                    $('#<%=txtNamaProvider.ClientID %>').val(obj.NAMAPROVIDER);
                    $('#<%=txtKodeProviderBPJS.ClientID %>').val(obj.KODEPROVIDERBPJS);
                    $('#<%=txtNamaProviderBPJS.ClientID %>').val(obj.NAMAPROVIDERBPJS);
                    $('#<%=txtProdukCOB.ClientID %>').val(obj.PRODUKCOB);
                    if (obj.FLAGPSTBPJS = "1") {
                        $('#<%:chkIsPatientBPJS.ClientID %>').is(':checked')
                    }


                    var nokainhealth = $('#<%=txtNoKartuInh.ClientID %>').val();
                    var date = Methods.getDatePickerDate($('#<%=txtTglSJP.ClientID %>').val());
                    var tglpelayanan = Methods.dateToYMD(date);
                    var username = $('#<%=hdnUsernameLogin.ClientID %>').val();
                    InhealthService.infobenefit_API(nokainhealth, tglpelayanan, username, function (resultBenefit) {
                    });
                }
                else {
                    displayMessageBox('STATUS ELIGIBILITAS PESERTA : ', obj.ERRORDESC);
                    ResetDataPeserta();
                }
            }
            else {
                displayMessageBox('STATUS ELIGIBILITAS PESERTA : ', resp[1]);
                ResetDataPeserta();
            }
        }
        catch (err) {
            ResetDataPeserta();
        }
    }

    //#endregion

    //#region Simpan
    $('#btnSimpan').click(function () {
        cbpPopupProcess.PerformCallback('save');
    });

    //#region Generate SJP
    $('#btnMPGenerateSJPCtl').click(function () {
        if ($('#<%=hdnIsBridgingToInhealth.ClientID %>').val() != '0') {
            if ($('#<%=txtNoKartuInh.ClientID %>').val() != '') {
                if ($('#<%=hdnCheckStatusPeserta.ClientID %>').val() == "1") {
                    var token = $('#<%=hdnTokenInhealth.ClientID %>').val();
                    var kodeprovider = $('#<%=hdnKodeProviderInhealth.ClientID %>').val();
                    var date = Methods.getDatePickerDate($('#<%=txtTglSJP.ClientID %>').val());
                    var tanggalpelayanan = Methods.dateToYMD(date);
                    var jenispelayanan = cboJenisPelayanan.GetValue();
                    var nokainhealth = $('#<%=txtNoKartuInh.ClientID %>').val();
                    var nomormedicalreport = $('#<%=txtMRN.ClientID %>').val();
                    var nomorasalrujukan = $('#<%=txtNoAsalRujukan.ClientID %>').val();
                    var kodeproviderasalrujukan = $('#<%=txtKodeProviderAsal.ClientID %>').val();
                    var date = Methods.getDatePickerDate($('#<%=txtTglAsalRujukan.ClientID %>').val());
                    var tanggalasalrujukan = Methods.dateToYMD(date);
                    var kodediagnosautama = $('#<%=txtDiagnoseCode.ClientID %>').val();
                    var poli = $('#<%=txtKodePoli.ClientID %>').val();
                    var username = $('#<%=hdnUsername.ClientID %>').val();
                    var informasitambahan = $('#<%=txtInformasiTambahan.ClientID %>').val();
                    var kodediagnosatambahan = $('#<%=txtDiagnoseCodeAdditional.ClientID %>').val();
                    var kecelakaankerja = cboKasusPelayanan.GetValue();
                    var kelasrawat = $('#<%=hdnKelasRawat.ClientID %>').val();
                    var kodejenpelruangrawat = $('#<%=hdnKodeJenpelRanap.ClientID %>').val();
                    var nohp = $('#<%=hdnPatientMobileNo.ClientID %>').val();
                    var email = $('#<%=hdnPatientEmail.ClientID %>').val();
                    var claimidprovider = "";

                    InhealthService.simpansjpV2_API(tanggalpelayanan, jenispelayanan, nokainhealth,
                nomormedicalreport, nomorasalrujukan, kodeproviderasalrujukan, tanggalasalrujukan,
                kodediagnosautama, poli, username, informasitambahan, kodediagnosatambahan,
                kecelakaankerja, kelasrawat, kodejenpelruangrawat, nohp, email, claimidprovider, function (result) {
                    GetGenerateSJPResult(result);
                    cbpPopupProcess.PerformCallback('save');
                    $('#btnMPCetakSJPCtl').css('display', '');
                });
                }
                else {
                    displayMessageBox("SIMPAN SJP : GAGAL", "Harap cek peserta terlebih dulu sebelum penerbitan SJP");
                }
            }
            else {
                showToast('WARNING', 'Nomor Kartu Inhealth harus diisi!');
            }
        }
        else {
            showToast('WARNING', 'Tidak bridging dengan Inhealth');
        }
    });

    function GetGenerateSJPResult(result) {
        try {
            var resp = result.split('|');
            if (resp[0] == "1") {
                var obj = jQuery.parseJSON(resp[2]);
                var errorCode = obj.ERRORCODE;
                if (errorCode == '00') {
                    displayMessageBox('SIMPAN SJP : ', obj.ERRORDESC);
                    $('#<%=txtNoSJP.ClientID %>').val(obj.NOSJP);
                    $('#<%=txtKodeProviderAsal.ClientID %>').val(obj.KDPPKASALRUJUKAN);
                    $('#<%=txtNamaProviderAsal.ClientID %>').val(obj.NMPPKASALRUJUKAN);
                    $('#<%=txtNoAsalRujukan.ClientID %>').val(obj.NOMORRUJUKAN);
                    if (obj.JENISKELAMIN == "L") {
                        $('#<%=txtGender.ClientID %>').val('Male');
                    }
                    else {
                        $('#<%=txtGender.ClientID %>').val('Female');
                    }
                    $('#<%=hdnIdAkomodasi.ClientID %>').val(obj.IDAKOMODASI);
                    $('#<%=txtKelasBPJS.ClientID %>').val(obj.KELASBPJS);
                    $('#<%=txtKodePoli.ClientID %>').val(obj.KDPOLI);
                    $('#<%=txtTipeSJP.ClientID %>').val(obj.TIPESJP);
                    $('#<%=txtTipeCOB.ClientID %>').val(obj.TIPECOB);
                    $('#<%=hdnErrorCode.ClientID %>').val(obj.ERRORCODE);
                }
                else {
                    displayMessageBox('SIMPAN SJP : GAGAL', obj.ERRORDESC);
                    $('#<%=txtNoSJP.ClientID %>').val('');
                }
            }
            else {
                displayMessageBox('SIMPAN SJP : GAGAL', resp[1]);
            }
        }
        catch (err) {
            $('#<%=txtNoSJP.ClientID %>').val('');
        }
    }
    //#endregion

    //#region Hapus SJP
    $('#btnMPHapusSJPCtl').click(function () {
        if ($('#<%=txtNoSJP.ClientID %>').val() != '') {
            $('#btnMPGenerateSJPCtl').css('display', 'none');
            $('#btnMPCetakSJPCtl').css('display', 'none');
            $('#btnMPUpdateTanggalPulangCtl').css('display', 'none');
            $('#btnBack').css('display', '');
            $('.tblKeteranganPelayanan').hide();
            $('.tblDataPeserta').hide();
            $('.tblHapusSJP').show();
            $('#<%=txtAlasanHapus.ClientID %>').val('');
        }
        else {
            showToast('WARNING', 'Harap isi No. SJP yang akan dihapus');
        }
    });

    $('#btnBack').click(function () {
        $('#btnMPGenerateSJPCtl').css('display', '');
        $('#btnMPCetakSJPCtl').css('display', '');
        $('#btnBack').css('display', 'none');
        $('.tblKeteranganPelayanan').show();
        $('.tblDataPeserta').show();
        $('.tblHapusSJP').hide();
        reinitMPButton();
    });

    $('#btnHapusSJP').click(function () {
        if ($('#<%=hdnIsBridgingToInhealth.ClientID %>').val() != '0') {
            if ($('#<%=txtNoSJP.ClientID %>').val() != '') {
                var token = $('#<%=hdnTokenInhealth.ClientID %>').val();
                var kodeprovider = $('#<%=hdnKodeProviderInhealth.ClientID %>').val();
                var nosjp = $('#<%=txtNoSJP.ClientID %>').val();
                var alasanhapus = $('#<%=txtAlasanHapus.ClientID %>').val();
                var userid = $('#<%=hdnUsername.ClientID %>').val();

                InhealthService.hapussjp_API(nosjp, alasanhapus, userid, function (result) {
                    var resp = result.split('|');
                    if (resp[0] == "1") {
                        var obj = jQuery.parseJSON(resp[2]);
                        var errorCode = obj.ERRORCODE;
                        if (errorCode == "00") {
                            displayMessageBox('HAPUS SJP : ', obj.ERRORDESC);
                            $('#btnMPGenerateSJPCtl').css('display', '');
                            $('#btnMPCetakSJPCtl').css('display', '');
                            $('#btnBack').css('display', 'none');
                            $('#btnSearchPeserta').css('display', '');
                            $('.tblKeteranganPelayanan').show();
                            $('.tblDataPeserta').show();
                            $('.tblHapusSJP').hide();
                            ResetDataPeserta();
                            reinitMPButton();

                            cbpPopupProcess.PerformCallback('delete');
                        }
                    }
                    else {
                        displayMessageBox('HAPUS SJP : GAGAL', resp[1]);
                    }
                });
            }
        }
        else {
            showToast('WARNING', 'Tidak bridging dengan Inhealth');
        }
    });

    //#endregion

    //#region Cari SJP
    $('#btnSearchSJP').click(function () {
        if ($('#<%=hdnIsBridgingToInhealth.ClientID %>').val() != '0') {
            if ($('#<%=txtNoSJP.ClientID %>').val() != '') {
                var token = $('#<%=hdnTokenInhealth.ClientID %>').val();
                var kodeprovider = $('#<%=hdnKodeProviderInhealth.ClientID %>').val();
                var nosjp = $('#<%=txtNoSJP.ClientID %>').val()

                InhealthService.infosjp_API(nosjp, function (result) {
                    if (result != '') {
                        var resultArr = result.split('|');
                        if (resultArr[0] == "1") {
                            GetResultInfoSJP(result);
                        }
                        else {
                            ShowSnackbarError(resultArr[1]);
                        }
                    }
                });
            }
            else {
                showToast('WARNING', 'Harap isi nomor SJP.');
            }
        }
        else {
            showToast('WARNING', 'Tidak bridging dengan Inhealth');
        }
    });

    function GetResultInfoSJP(result) {
        try {
            var resp = result.split('|');
            if (resp[0] == "1") {
                var obj = jQuery.parseJSON(resp[2]);
                var errorCode = obj.ERRORCODE;
                if (errorCode == '00') {
                    displayMessageBox('CARI SJP : ', obj.ERRORDESC);
                    $('#<%=txtNoSJP.ClientID %>').val(obj.NOSJP);
                    $('#<%=txtNoKartuInh.ClientID %>').val(obj.NOKAPESERTA);
                    $('#<%=txtNamaPeserta.ClientID %>').val(obj.NAMAPESERTA);
                    $('#<%=txtTglSJP.ClientID %>').val(obj.TGLSJP);
                    $('#<%=txtNoAsalRujukan.ClientID %>').val(obj.NOMORRUJUKAN);
                    $('#<%=txtKodeProviderAsal.ClientID %>').val(obj.KDPPKASALRUJUKAN);
                    $('#<%=txtNamaProviderAsal.ClientID %>').val(obj.NMPPKASALRUJUKAN);
                    if (obj.JENISKELAMIN == "L") {
                        $('#<%=txtGender.ClientID %>').val('Male');
                    }
                    else {
                        $('#<%=txtGender.ClientID %>').val('Female');
                    }
                    $('#<%=txtKelasBPJS.ClientID %>').val(obj.KELASBPJS);
                    $('#<%=txtKodePoli.ClientID %>').val(obj.KDPOLI);
                    $('#<%=txtNamaPoli.ClientID %>').val(obj.NMPOLI);
                    $('#<%=txtDiagnoseCode.ClientID %>').val(obj.KDDIAG);
                    $('#<%=txtDiagnoseName.ClientID %>').val(obj.NMDIAG);
                    $('#<%=txtKodeProduk.ClientID %>').val(obj.PLAN);
                    $('#<%=txtNamaProduk.ClientID %>').val(obj.PLANDESC);
                    $('#<%=txtKodeKelasRawat.ClientID %>').val(obj.KELAS);
                    $('#<%=txtKelasRawat.ClientID %>').val(obj.KELASDESC);
                    $('#<%=txtKodeBadanUsaha.ClientID %>').val(obj.KDBU);
                    $('#<%=txtBadanUsaha.ClientID %>').val(obj.NMBU);
                    $('#<%=txtTipeSJP.ClientID %>').val(obj.TIPESJP);
                    $('#<%=txtTipeCOB.ClientID %>').val(obj.TIPECOB);
                }
                else {
                    displayMessageBox('CARI SJP : GAGAL', obj.ERRORDESC);
                }
            }
            else {
                displayMessageBox('CARI SJP : GAGAL', resp[1]);
            }
        }
        catch (err) {
        }
    }
    //#endregion

    //#region Cetak SJP
    $('#btnMPCetakSJPCtl').click(function () {
        if ($('#<%=hdnIsBridgingToInhealth.ClientID %>').val() != '0') {
            if ($('#<%=txtNoSJP.ClientID %>').val() != '') {
                if ($('#<%=hdnDepartmentID.ClientID %>').val() == "INPATIENT") {
                    openReportViewer('PM-00164', $('#<%=hdnRegistrationID.ClientID %>').val());
                }
                else {
                    openReportViewer('PM-00163', $('#<%=hdnRegistrationID.ClientID %>').val());
                }
            }
            else {
                showToast('WARNING', 'Harap isi nomor SJP.');
            }
        }
        else {
            showToast('WARNING', 'Tidak bridging dengan Inhealth');
        }
    });
    //#endregion

    //#region Update Tanggal Pulang
    $('#btnMPUpdateTanggalPulangCtl').click(function () {
        if ($('#<%=hdnIsBridgingToInhealth.ClientID %>').val() != '0') {
            if ($('#<%=txtNoSJP.ClientID %>').val() != '') {
                if ($('#<%=hdnDepartmentID.ClientID %>').val() == "INPATIENT") {
                    cbpPopupProcess.PerformCallback('update_pulang');
                }
            }
            else {
                showToast('WARNING', 'Harap isi nomor SJP.');
            }
        }
        else {
            showToast('WARNING', 'Tidak bridging dengan Inhealth');
        }
    });
    //#endregion

    //#region Search Dialog

    //#region Diagnose
    $('#lblDiagnose.lblLink').live('click', function () {
        openSearchDialog('diagnose', "IsDeleted = 0", function (value) {
            $('#<%=txtDiagnoseCode.ClientID %>').val(value);
            onTxtDiagnoseCodeChanged(value);
        });
    });

    $('#<%=txtDiagnoseCode.ClientID %>').live('change', function () {
        onTxtDiagnoseCodeChanged($(this).val());
    });

    function onTxtDiagnoseCodeChanged(value) {
        var filterExpression = "DiagnoseID = '" + value + "' AND IsDeleted = 0";
        Methods.getObject('GetDiagnoseList', filterExpression, function (result) {
            if (result != null) {
                $('#<%=txtDiagnoseName.ClientID %>').val(result.DiagnoseName);
            }
            else {
                $('#<%=txtDiagnoseCode.ClientID %>').val('');
                $('#<%=txtDiagnoseName.ClientID %>').val('');
            }
        });
    }
    //#endregion

    //#region Diagnose Additional
    $('#lblDiagnoseAdditional.lblLink').live('click', function () {
        openSearchDialog('diagnose', "IsDeleted = 0", function (value) {
            $('#<%=txtDiagnoseCodeAdditional.ClientID %>').val(value);
            onTxtDiagnoseCodeAdditionalChanged(value);
        });
    });

    $('#<%=txtDiagnoseCodeAdditional.ClientID %>').live('change', function () {
        onTxtDiagnoseCodeAdditionalChanged($(this).val());
    });

    function onTxtDiagnoseCodeAdditionalChanged(value) {
        var filterExpression = "DiagnoseID = '" + value + "' AND IsDeleted = 0";
        Methods.getObject('GetDiagnoseList', filterExpression, function (result) {
            if (result != null) {
                $('#<%=txtDiagnoseNameAdditional.ClientID %>').val(result.DiagnoseName);
            }
            else {
                $('#<%=txtDiagnoseCodeAdditional.ClientID %>').val('');
                $('#<%=txtDiagnoseNameAdditional.ClientID %>').val('');
            }
        });
    }
    //#endregion

    //#region Inhealth Reference Poli
    $('#lblPoli.lblLink').click(function () {
        var filterExpression = "Others LIKE '%True%'";
        openSearchDialog('vinhealthreferencepoli', filterExpression, function (value) {
            $('#<%=txtKodePoli.ClientID %>').val(value);
            onTxtInhealthKodePoliChanged(value);
        });
    });

    $('#<%=txtKodePoli.ClientID %>').change(function () {
        onTxtInhealthKodePoliChanged($(this).val());
    });

    function onTxtInhealthKodePoliChanged(value) {
        var filterExpression = "Others = '" + value + "'";
        Methods.getObject('GetvInhealthReferencePoliList', filterExpression, function (result) {
            if (result != null) {
                $('#<%=txtKodePoli.ClientID %>').val(result.ObjectCode);
                $('#<%=txtNamaPoli.ClientID %>').val(result.ObjectName);
            }
            else {
                $('#<%=txtKodePoli.ClientID %>').val('');
                $('#<%=txtNamaPoli.ClientID %>').val('');
            }
        });
    }
    //#endregion

    //#region Inhealth Reference Provider Rujukan
    $('#lblProviderAsal.lblLink').click(function () {
        var filterExpression = "";
        openSearchDialog('vinhealthreferenceproviderrujukan', filterExpression, function (value) {
            $('#<%=txtKodeProviderAsal.ClientID %>').val(value);
            onTxtInhealthKodeProviderRujukanChanged(value);
        });
    });

    $('#<%=txtKodeProviderAsal.ClientID %>').change(function () {
        onTxtInhealthKodeProviderRujukanChanged($(this).val());
    });

    function onTxtInhealthKodeProviderRujukanChanged(value) {
        var filterExpression = "ObjectCode = '" + value + "'";
        Methods.getObject('GetvInhealthReferenceProviderRujukanList', filterExpression, function (result) {
            if (result != null) {
                $('#<%=txtKodeProviderAsal.ClientID %>').val(result.ObjectCode);
                $('#<%=txtNamaProviderAsal.ClientID %>').val(result.ObjectName);
            }
            else {
                $('#<%=txtKodeProviderAsal.ClientID %>').val('');
                $('#<%=txtNamaProviderAsal.ClientID %>').val('');
            }
        });
    }
    //#endregion

    //#region Kode Akomodasi
    $('#lblAkomodasi.lblLink').live('click', function () {
        openSearchDialog('viteminhealth', "InhealthKodeJenPelRanap IS NOT NULL AND IsInhealthRanapAkomodasi = 1", function (value) {
            $('#<%=txtKodeAkomodasi.ClientID %>').val(value);
            ontxtKodeAkomodasiChanged(value);
        });
    });

    $('#<%=txtKodeAkomodasi.ClientID %>').live('change', function () {
        ontxtKodeAkomodasiChanged($(this).val());
    });

    function ontxtKodeAkomodasiChanged(value) {
        var filterExpression = "ItemCode = '" + value + "'";
        Methods.getObject('GetvItemMasterList', filterExpression, function (result) {
            if (result != null) {
                $('#<%=txtKodeAkomodasi.ClientID %>').val(result.ItemCode);
                $('#<%=txtNamaAkomodasi.ClientID %>').val(result.ItemName1);
                $('#<%=hdnKodeJenpelRanap.ClientID %>').val(result.InhealthKodeJenPelRanap);
                
            }
            else {
                $('#<%=txtKodeAkomodasi.ClientID %>').val('');
                $('#<%=txtNamaAkomodasi.ClientID %>').val('');
                $('#<%=hdnKodeJenpelRanap.ClientID %>').val('');
            }
        });
    }
    //#endregion

    //#endregion

    function ValidateParameter() {
        var message = '';
        if ($('#<%=txtDiagnoseCode.ClientID %>').val() == '') {
            message = message + 'Diagnosa awal harus diisi  \n';
        }
        return message;
    }

    function onCbpPopupProcesEndCallback(s) {
        hideLoadingPanel();
        var param = s.cpResult.split('|');
        if (param[0] == 'save' || param[0] == 'delete') {
            reinitMPButton();
            if (param[1] == 'fail')
                displayMessageBox('Save Failed', 'Error Message : ' + param[2]);
        }
        else if (param[0] == 'update_pulang') {
            reinitMPButton();
            if (param[1] == 'fail') {
                displayMessageBox('Save Failed', 'Error Message : ' + param[2]);
            }
            else {
                displayMessageBox("UPDATE TANGGAL PULANG : SUKSES");
            }                
        }
        else {
            if (param[0] != 'update') {
                pcRightPanelContent.Hide();
            }
            else if (param[0] == 'update') {
                openReportViewer('PM-00128', $('#<%=hdnID.ClientID %>').val());
            }
            var code = $('#hdnRightPanelContentCode').val();
            if (code == '') code = 'generateSEP';
            onAfterSaveRightPanelContent(code, '', '');
        }
    }

    function ResetDataPeserta() {;
        $('#<%=txtNoSJP.ClientID %>').val('');
        $('#<%=txtKodeAkomodasi.ClientID %>').val('');
        $('#<%=txtNamaAkomodasi.ClientID %>').val('');

        if ($('#<%=hdnDepartmentID.ClientID %>').val() == "INPATIENT") {
            $('.trPoli').hide();
            $('#<%=txtKodePoli.ClientID %>').val('UMU');
            $('.tblAkomodasi').show();
            $('.tblTanggalPulang').show();
        }
        else {
            $('#<%=txtNamaAkomodasi.ClientID %>').val('');
        }
    }

    if ($('#<%=hdnDepartmentID.ClientID %>').val() == "INPATIENT") {
        $('.trPoli').hide();
        $('#<%=txtKodePoli.ClientID %>').val('UMU');
        $('.tblAkomodasi').show();
        $('.tblTanggalPulang').show();
    }
    else {
        $('#<%=txtNamaAkomodasi.ClientID %>').val('');
    }

</script>
<div style="padding: 5px 0;">
    <input type="hidden" runat="server" id="hdnIsAdd" value="0" />
    <input type="hidden" runat="server" id="hdnMRN" value="" />
    <input type="hidden" id="hdnRegistrationNo" value="" runat="server" />
    <input type="hidden" runat="server" id="hdnID" value="" />
    <input type="hidden" runat="server" id="hdnKdJenisPeserta" value="" />
    <input type="hidden" runat="server" id="hdnKdKelas" value="" />
    <input type="hidden" runat="server" id="hdnAsalRujukan" value="1" />
    <input type="hidden" runat="server" id="hdnIsBpjsRegistrationCtl" value="" />
    <input type="hidden" runat="server" id="hdnIsPoliExecutive" value="0" />
    <input type="hidden" runat="server" id="hdnBPJSReferenceInfoKodeUnit" value="1" />
    <input type="hidden" runat="server" id="hdnBPJSReferenceInfoNamaUnit" value="1" />
    <input type="hidden" runat="server" id="hdnNoSKDP" value="" />
    <input type="hidden" runat="server" id="hdnKodeDPJP" value="" />
    <input type="hidden" runat="server" id="hdnIsBridgingToInhealth" value="" />
    <input type="hidden" runat="server" id="hdnTokenInhealth" value="" />
    <input type="hidden" runat="server" id="hdnKodeProviderInhealth" value="" />
    <input type="hidden" runat="server" id="hdnUsername" value="" />
    <input type="hidden" runat="server" id="hdnIdAkomodasi" value="" />
    <input type="hidden" runat="server" id="hdnErrorCode" value="" />
    <input type="hidden" runat="server" id="hdnDepartmentID" value="" />
    <input type="hidden" runat="server" id="hdnRegistrationID" value="" />
    <input type="hidden" runat="server" id="hdnKelasRawat" value="" />
    <input type="hidden" runat="server" id="hdnCheckStatusPeserta" value="0" />
    <input type="hidden" runat="server" id="hdnUsernameLogin" value="" />
    <input type="hidden" runat="server" id="hdnKodeJenpelRanap" value="" />
    <input type="hidden" runat="server" id="hdnPatientMobileNo" value="" />
    <input type="hidden" runat="server" id="hdnPatientEmail" value="" />
    <table class="tblKeteranganPelayanan" width="100%">
        <colgroup>
            <col width="50%" />
            <col width="50%" />
        </colgroup>
        <tr>
            <td valign="top">
                <table>
                    <colgroup>
                        <col width="115px" />
                        <col width="100px" />
                        <col width="115px" />
                        <col />
                    </colgroup>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("No. Kartu Peserta")%></label>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtNoKartuInh" runat="server" Width="99%" />
                        </td>
                        <td>
                            <input type="button" id="btnSearchPeserta" value='<%= GetLabel("Cek Peserta")%>' />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label>
                                <%=GetLabel("No. Kartu BPJS")%></label>
                        </td>
                        <td colspan="3">
                            <asp:TextBox ID="txtNoBPJS" runat="server" Width="100%">
                            </asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label>
                                <%=GetLabel("No. SJP")%></label>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtNoSJP" runat="server" Width="99%">
                            </asp:TextBox>
                        </td>
                        <td>
                            <input type="button" id="btnSearchSJP" value='<%= GetLabel("Cari")%>' />
                        </td>
                    </tr>
                </table>
            </td>
            <td valign="top">
                <table>
                    <colgroup>
                        <col width="115px" />
                        <col width="auto" />
                        <col />
                    </colgroup>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Tanggal Pelayanan")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtTglSJP" Width="120px" runat="server" CssClass="datepicker" />
                        </td>
                        <td>
                        </td>
                    </tr>
                    <tr class="trJenisPelayanan">
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Jenis Pelayanan")%></label>
                        </td>
                        <td>
                            <dxe:ASPxComboBox ID="cboJenisPelayanan" ClientInstanceName="cboJenisPelayanan" Width="75%"
                                runat="server">
                                <%--<ClientSideEvents ValueChanged="function(s,e) { onCboResultTypeValueChanged(); }" />--%>
                            </dxe:ASPxComboBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Kasus Pelayanan")%></label>
                        </td>
                        <td>
                            <dxe:ASPxComboBox ID="cboKasusPelayanan" ClientInstanceName="cboKasusPelayanan" Width="75%"
                                runat="server">
                                <%--<ClientSideEvents ValueChanged="function(s,e) { onCboResultTypeValueChanged(); }" />--%>
                            </dxe:ASPxComboBox>
                        </td>
                    </tr>
                    <tr class="trPoli">
                        <td class="tdLabel">
                            <label class="lblLink" id="lblPoli">
                                <%=GetLabel("Poli")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtKodePoli" runat="server" Width="50px">
                            </asp:TextBox>
                            <asp:TextBox ID="txtNamaPoli" runat="server" Width="200px">
                            </asp:TextBox>
                        </td>
                        <td>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <table class="tblDataPeserta" width="100%">
        <colgroup>
            <col width="50%" />
            <col width="50%" />
        </colgroup>
        <tr>
            <td valign="top">
                <table style="width: 100%">
                    <colgroup>
                        <col width="115px" />
                        <col width="80px" />
                        <col width="90px" />
                        <col />
                    </colgroup>
                    <tr>
                        <td colspan="4">
                            <h4>
                                <%=GetLabel("DATA PESERTA :")%></h4>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Nama")%></label>
                        </td>
                        <td colspan="3">
                            <asp:TextBox ID="txtNamaPeserta" runat="server" Width="99%" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("No. RM")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtMRN" runat="server" Width="100px" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Jenis Kelamin")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtGender" runat="server" Width="100px" />
                        </td>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Tanggal Lahir")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtDOB" Width="100px" runat="server" CssClass="datepicker" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Produk")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtKodeProduk" Width="100px" runat="server" />
                        </td>
                        <td colspan="3">
                            <asp:TextBox ID="txtNamaProduk" Width="99%" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Kelas Rawat")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtKodeKelasRawat" Width="100px" runat="server" />
                        </td>
                        <td colspan="3">
                            <asp:TextBox ID="txtKelasRawat" Width="99%" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Kelas BPJS")%></label>
                        </td>
                        <td colspan="3">
                            <asp:TextBox ID="txtKelasBPJS" Width="99%" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Provider")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtKodeProvider" Width="100px" runat="server" />
                        </td>
                        <td colspan="3">
                            <asp:TextBox ID="txtNamaProvider" Width="99%" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Provider BPJS")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtKodeProviderBPJS" Width="100px" runat="server" />
                        </td>
                        <td colspan="3">
                            <asp:TextBox ID="txtNamaProviderBPJS" Width="99%" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Badan Usaha")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtKodeBadanUsaha" Width="100px" runat="server" />
                        </td>
                        <td colspan="3">
                            <asp:TextBox ID="txtBadanUsaha" Width="99%" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Tipe SJP") %></label>
                        </td>
                        <td colspan="3">
                            <asp:TextBox ID="txtTipeSJP" runat="server" Width="99%" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Tipe COB") %></label>
                        </td>
                        <td colspan="3">
                            <asp:TextBox ID="txtTipeCOB" runat="server" Width="99%" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Produk COB") %></label>
                        </td>
                        <td colspan="3">
                            <asp:TextBox ID="txtProdukCOB" runat="server" Width="99%" />
                        </td>
                    </tr>
                    <tr style="display: none">
                        <td>
                            <input type="checkbox" id="chkIsPatientBPJS" runat="server" />
                            <label>
                                Peserta BPJS</label>
                        </td>
                    </tr>
                </table>
            </td>
            <td valign="top">
                <table style="width: 100%">
                    <colgroup>
                        <col width="125px" />
                        <col width="100px" />
                        <col />
                    </colgroup>
                    <tr>
                        <td colspan="4">
                            <h4>
                                <%=GetLabel("KETERANGAN PROVIDER :")%></h4>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label>
                                <%=GetLabel("No. Rujukan")%></label>
                        </td>
                        <td colspan="3">
                            <asp:TextBox ID="txtNoAsalRujukan" runat="server" Width="100%" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblLink" id="lblProviderAsal">
                                <%=GetLabel("Provider Asal")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtKodeProviderAsal" runat="server" Width="100px" />
                        </td>
                        <td colspan="3">
                            <asp:TextBox ID="txtNamaProviderAsal" runat="server" Width="100%" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Tanggal Asal Rujukan")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtTglAsalRujukan" Width="80px" runat="server" CssClass="datepicker" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblLink lblMandatory" id="lblDiagnose">
                                <%=GetLabel("Diagnosa")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtDiagnoseCode" Width="100px" runat="server" />
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtDiagnoseName" Width="100%" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblLink" id="lblDiagnoseAdditional">
                                <%=GetLabel("Diagnosa Tambahan")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtDiagnoseCodeAdditional" Width="100px" runat="server" />
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtDiagnoseNameAdditional" Width="100%" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel" style="vertical-align: top;">
                            <label class="lblNormal">
                                <%=GetLabel("Informasi Tambahan")%></label>
                        </td>
                        <td colspan="3">
                            <asp:TextBox ID="txtInformasiTambahan" Width="100%" runat="server" TextMode="MultiLine" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td valign="top">
                <table style="width: 100%; display: none;" class="tblAkomodasi">
                    <colgroup>
                        <col width="125px" />
                        <col width="100px" />
                        <col />
                    </colgroup>
                    <tr>
                        <td colspan="4">
                            <h4>
                                <%=GetLabel("AKOMODASI :")%></h4>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblLink lblMandatory" id="lblAkomodasi">
                                <%=GetLabel("Kode Akomodasi")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtKodeAkomodasi" Width="100px" runat="server" />
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtNamaAkomodasi" Width="100%" runat="server" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td valign="top">
                <table style="width: 100%; display: none;" class="tblTanggalPulang">
                    <colgroup>
                        <col width="125px" />
                        <col width="100px" />
                        <col />
                    </colgroup>
                    <tr>
                        <td colspan="4">
                            <h4>
                                <%=GetLabel("UPDATE TANGGAL PULANG :")%></h4>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Tanggal Pulang")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtTanggalPulang" Width="100px" runat="server" />
                        </td>
                        <td colspan="2">
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <table class="tblHapusSJP" width="100%" style="display: none">
        <colgroup>
            <col width="100%" />
        </colgroup>
        <tr>
            <td valign="top">
                <table>
                    <colgroup>
                        <col width="10%" />
                        <col width="90%" />
                    </colgroup>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Alasan Hapus")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtAlasanHapus" runat="server" Width="100%" TextMode="MultiLine" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <input type="button" id="btnHapusSJP" value='<%= GetLabel("Hapus SJP")%>' />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <dxcp:ASPxCallbackPanel ID="cbpPopupProcess" runat="server" Width="100%" ClientInstanceName="cbpPopupProcess"
        ShowLoadingPanel="false" OnCallback="cbpPopupProcess_Callback">
        <ClientSideEvents BeginCallback="function(s,e) { showLoadingPanel(); }" EndCallback="function(s,e) { onCbpPopupProcesEndCallback(s); }" />
    </dxcp:ASPxCallbackPanel>
</div>
