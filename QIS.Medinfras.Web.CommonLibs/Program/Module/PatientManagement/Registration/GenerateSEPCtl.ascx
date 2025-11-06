<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="GenerateSEPCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.GenerateSEPCtl" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<div class="toolbarArea">
    <ul>
        <li id="btnMPEntryPopupSave">
            <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbsave.png")%>' alt="" /><div>
                <%=GetLabel("Save")%></div>
        </li>
        <li id="btnMPProposeSEPCtl">
            <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbsendmail.png")%>' alt="" /><div>
                <%=GetLabel("Pengajuan Backdate")%></div>
        </li>
        <li id="btnMPProposeFPCtl">
            <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbsendmail.png")%>' alt="" /><div>
                <%=GetLabel("Pengajuan FP")%></div>
        </li>
        <li id="btnMPGenerateSEPCtl">
            <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbprocess.png")%>' alt="" /><div>
                <%=GetLabel("Pembuatan")%></div>
        </li>
        <li id="btnMPMappingSEPCtl" style="display: none">
            <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbprocess.png")%>' alt="" /><div>
                <%=GetLabel("Mapping SEP")%></div>
        </li>
        <li id="btnMPUpdateSEPCtl">
            <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbedit.png")%>' alt="" /><div>
                <%=GetLabel("Update")%></div>
        </li>
        <li id="btnMPDeleteSEPCtl">
            <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbdelete.png")%>' alt="" /><div>
                <%=GetLabel("Delete")%></div>
        </li>
        <li id="btnMPDischargePatientCtl">
            <img src='<%=ResolveUrl("~/Libs/Images/Icon/close.png")%>' alt="" /><div>
                <%=GetLabel("Tanggal Pulang")%></div>
        </li>
        <li id="btnMPEntryPopupPrintSEP">
            <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbprint.png")%>' alt="" /><div>
                <%=GetLabel("Surat Eligibilitas Peserta")%></div>
        </li>
        <li id="btnMPEntryPopupPrintSuratRujukan">
            <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbprint.png")%>' alt="" /><div>
                <%=GetLabel("Surat Rujukan")%></div>
        </li>
        <li id="btnMPEntryPopupAppointmentSuratRujukan">
            <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbprint.png")%>' alt="" /><div id="divBtnSuratRujukan" runat="server">
            </div>
        </li>
        <li id="btnMPEntryPopupPrintSPRI">
            <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbprint.png")%>' alt="" /><div>
                <%=GetLabel("SPRI")%></div>
        </li>
        <li id="btnBackToSEPContent">
            <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbback.png")%>' alt="" /><div>
                <%=GetLabel("Kembali")%></div>
        </li>
    </ul>
</div>
<script type="text/javascript" id="dxss_generatesepctl">
    registerCollapseExpandHandler();

    setDatePicker('<%=txtTglSEP.ClientID %>');
    setDatePicker('<%=txtTglRujukan.ClientID %>');
    setDatePicker('<%=txtTglRencanaKunjungan.ClientID %>');
    setDatePicker('<%=txtDischargeDateCtl.ClientID %>');
    setDatePicker('<%=txtDateOfDeath.ClientID %>');
    setDatePicker('<%=txtTglRencanaKontrol.ClientID %>');
    $('#<%:txtTglSEP.ClientID %>').datepicker('option', 'maxDate', new Date(getDateNow()));
    $('#<%:txtTglRujukan.ClientID %>').datepicker('option', 'maxDate', '0');
    $('#<%:txtTglRencanaKunjungan.ClientID %>').datepicker('option', 'maxDate', '100');
    $('#<%:txtDischargeDateCtl.ClientID %>').datepicker('option', 'maxDate', new Date(getDateNow()));
    $('#<%:txtDateOfDeath.ClientID %>').datepicker('option', 'maxDate', '0');
    $('#<%:txtTglRencanaKontrol.ClientID %>').datepicker('option', 'maxDate', '100');

    $('#btnBackToSEPContent').css('display', 'none');
    $("#divGetRencanaKontrol").hide();

    function reinitMPButton() {
        if ($('#<%=txtNoSEP.ClientID %>').val() != '') {
            $('#btnMPProposeSEPCtl').css('display', 'none');
            $('#btnMPProposeFPCtl').css('display', 'none');
            $('#btnMPGenerateSEPCtl').css('display', 'none');
            $('#btnMPUpdateSEPCtl').css('display', '');
            $('#btnMPDeleteSEPCtl').css('display', '');
            $('#btnMPEntryPopupPrintSEP').css('display', '');
            $('#btnMPEntryPopupPrintSuratRujukan').css('display', '');

            $('#btnSearchPeserta').css('display', 'none');
            $('#btnSearchNIK').css('display', 'none');
            $('#btnSearchNoRujukan').css('display', 'none');

            $('#btnMPDischargePatientCtl').css('display', '');

            if ($('#<%=txtNoSPRI.ClientID %>').val() == "") {
                $('#btnInsertSPRI').css('display', '');
                $('#btnUpdateSPRI').css('display', 'none');
                $('#btnDeleteSPRI').css('display', 'none');
                $('#btnMPEntryPopupPrintSPRI').css('display', 'none');
            } else {
                $('#btnInsertSPRI').css('display', 'none');
                $('#btnUpdateSPRI').css('display', '');
                $('#btnDeleteSPRI').css('display', '');
                $('#btnMPEntryPopupPrintSPRI').css('display', '');
            }

            $('#<%=trKontrolBerikutnya.ClientID %>').css('display', '');
            //            $('#<%=txtNoSuratKontrol.ClientID %>').attr("readonly", "readonly");
            //            $('#lblRencanaKontrol').removeClass("lblLink");

            if ($('#<%=txtNoRencanaKontrolBerikutnya.ClientID %>').val() == "") {
                $('#btnInsertRencanaKontrol').css('display', '');
                $('#btnUpdateRencanaKontrol').css('display', 'none');
                $('#btnDeleteRencanaKontrol').css('display', 'none');
                $('#btnMPEntryPopupAppointmentSuratRujukan').css('display', 'none');
            } else {
                $('#btnInsertRencanaKontrol').css('display', 'none');
                $('#btnUpdateRencanaKontrol').css('display', '');
                $('#btnDeleteRencanaKontrol').css('display', '');
                $('#btnMPEntryPopupAppointmentSuratRujukan').css('display', '');
            }

            if ($('#<%=txtNoRujukan2.ClientID %>').val() == "") {
                $('#btnInsertRujukan').css('display', '');
                $('#btnUpdateRujukan').css('display', 'none');
            } else {
                $('#btnInsertRujukan').css('display', 'none');
                $('#btnUpdateRujukan').css('display', '');
            }
        }
        else {
            $('#btnMPProposeSEPCtl').css('display', '');
            $('#btnMPProposeFPCtl').css('display', '');
            $('#btnMPGenerateSEPCtl').css('display', '');
            $('#btnMPUpdateSEPCtl').css('display', 'none');
            $('#btnMPDeleteSEPCtl').css('display', 'none');
            $('#btnMPEntryPopupPrintSEP').css('display', 'none');
            $('#btnMPEntryPopupPrintSuratRujukan').css('display', 'none');

            $('#btnSearchPeserta').css('display', '');
            $('#btnSearchNIK').css('display', '');
            $('#btnSearchNoRujukan').css('display', '');

            $('#btnMPDischargePatientCtl').css('display', 'none');

            if ($('#<%=txtNoSPRI.ClientID %>').val() == "") {
                $('#btnInsertSPRI').css('display', '');
                $('#btnUpdateSPRI').css('display', 'none');
                $('#btnDeleteSPRI').css('display', 'none');
                $('#btnMPEntryPopupPrintSPRI').css('display', 'none');
            } else {
                $('#btnInsertSPRI').css('display', 'none');
                $('#btnUpdateSPRI').css('display', '');
                $('#btnDeleteSPRI').css('display', '');
                $('#btnMPEntryPopupPrintSPRI').css('display', '');
            }

            $('#<%=trKontrolBerikutnya.ClientID %>').css('display', 'none');
            $('#<%=txtNoSuratKontrol.ClientID %>').removeAttr("readonly", "readonly");
            $('#lblRencanaKontrol').addClass("lblLink");

            if ($('#<%=txtNoRencanaKontrolBerikutnya.ClientID %>').val() == "") {
                $('#btnInsertRencanaKontrol').css('display', '');
                $('#btnUpdateRencanaKontrol').css('display', 'none');
                $('#btnDeleteRencanaKontrol').css('display', 'none');
                $('#btnMPEntryPopupAppointmentSuratRujukan').css('display', 'none');
            } else {
                $('#btnInsertRencanaKontrol').css('display', 'none');
                $('#btnUpdateRencanaKontrol').css('display', '');
                $('#btnDeleteRencanaKontrol').css('display', '');
                $('#btnMPEntryPopupAppointmentSuratRujukan').css('display', '');
            }
        }
    }

    $(function () {
        reinitMPButton();

        //#region Province
        function GetSCProvinceFilterExpression() {
            //            var filterExpression = "<%:GetSCProvinceFilterExpression() %>";
            var filterExpression = "GCBPJSObjectType = 'X358^007'";
            return filterExpression;
        }

        $('#<%:lblAccidentProvince.ClientID %>.lblLink').click(function () {
            openSearchDialog('bpjsreference', GetSCProvinceFilterExpression(), function (value) {
                $('#<%=txtKodePropinsi.ClientID %>').val(value);
                onTxtKodePropinsiChanged(value);
            });
        });

        $('#<%=txtKodePropinsi.ClientID %>').change(function () {
            onTxtKodePropinsiChanged($(this).val());
        });

        function onTxtKodePropinsiChanged(value) {
            var filterExpression = GetSCProvinceFilterExpression() + " AND BPJSCode = '" + value + "'";
            Methods.getObject('GetBPJSReferenceList', filterExpression, function (result) {
                if (result != null) {
//                    $('#<%=hdnGCState.ClientID %>').val(result.StandardCodeID);
                    $('#<%=hdnKodePropinsiBPJS.ClientID %>').val(result.BPJSCode);
                    $('#<%=txtNamaPropinsi.ClientID %>').val(result.BPJSName);
                }
                else {
//                    $('#<%=hdnGCState.ClientID %>').val('');
                    $('#<%=hdnKodePropinsiBPJS.ClientID %>').val('');
                    $('#<%=txtNamaPropinsi.ClientID %>').val('');
                }
            });
        }
        //#endregion

        //#region Kota/Kabupaten
        function GetCityFilterExpression() {
            //            var filterExpression = " GCState = '" + $('#<%=hdnGCState.ClientID %>').val() + "'";
            var filterExpression = "GCBPJSObjectType = 'X358^008'";
            return filterExpression;
        }

        $('#<%:lblAccidentCity.ClientID %>.lblLink').click(function () {
//            var state = $('#<%=hdnGCState.ClientID %>').val();
//            if (state != '') {
//                openSearchDialog('bpjsreference', GetCityFilterExpression(), function (value) {
//                    $('#<%=txtKodeKabupaten.ClientID %>').val(value);
//                    onTxtKodeKabupatenChanged(value);
//                });
//            } else {
//                showToast('Warning', 'Pilih Propinsi terlebih dahulu.');
            //            }
            openSearchDialog('bpjsreference', GetCityFilterExpression(), function (value) {
                $('#<%=txtKodeKabupaten.ClientID %>').val(value);
                onTxtKodeKabupatenChanged(value);
            });
        });

        $('#<%=txtKodeKabupaten.ClientID %>').change(function () {
            onTxtKodeKabupatenChanged($(this).val());
        });

        function onTxtKodeKabupatenChanged(value) {
            var filterExpression = GetCityFilterExpression() + " AND BPJSCode = '" + value + "'";
            Methods.getObject('GetBPJSReferenceList', filterExpression, function (result) {
                if (result != null) {
                    $('#<%=hdnKabupatenID.ClientID %>').val(result.BPJSCode);
                    $('#<%=hdnKodeKabupatenBPJS.ClientID %>').val(result.BPJSCode);
                    $('#<%=txtNamaKabupaten.ClientID %>').val(result.BPJSName);

//                    if (result.BPJSReferenceInfo != null) {
//                        var bpjsReferenceInfo = result.BPJSReferenceInfo.split('|');

//                        if (bpjsReferenceInfo[0] != '') {
//                            $('#<%=hdnKodeKabupatenBPJS.ClientID %>').val(bpjsReferenceInfo[0]);
//                        }
//                        else {
//                            $('#<%=hdnKodeKabupatenBPJS.ClientID %>').val(result.KodeKabupaten);
//                        }
//                    }
//                    else {
//                        $('#<%=hdnKodeKabupatenBPJS.ClientID %>').val(result.KodeKabupaten);
//                    }
                }
                else {
                    $('#<%=hdnKabupatenID.ClientID %>').val('0');
                    $('#<%=hdnKodeKabupatenBPJS.ClientID %>').val('');
                    $('#<%=txtKodeKabupaten.ClientID %>').val('');
                    $('#<%=txtNamaKabupaten.ClientID %>').val('');
                }
            });
        }
        //#endregion

        //#region Kecamatan
        function GetDistrictFilterExpression() {
            //            var filterExpression = " KabupatenID = " + $('#<%=hdnKabupatenID.ClientID %>').val();
            var filterExpression = " GCBPJSObjectType = 'X358^009'";
            return filterExpression;
        }

        $('#<%:lblAccidentDistrict.ClientID %>.lblLink').click(function () {
//            var kabupaten = $('#<%=hdnKabupatenID.ClientID %>').val();

//            if (kabupaten != '') {
//                openSearchDialog('kecamatan', GetDistrictFilterExpression(), function (value) {
//                    $('#<%=txtKodeKecamatan.ClientID %>').val(value);
//                    onTxtKodeKecamatanChanged(value);
//                });
//            } else {
//                showToast('Warning', 'Pilih Kabupaten terlebih dahulu.');
            //            }
            openSearchDialog('bpjsreference', GetDistrictFilterExpression(), function (value) {
                $('#<%=txtKodeKecamatan.ClientID %>').val(value);
                onTxtKodeKecamatanChanged(value);
            });
        });

        $('#<%=txtKodeKabupaten.ClientID %>').change(function () {
            onTxtKodeKecamatanChanged($(this).val());
        });

        function onTxtKodeKecamatanChanged(value) {
            var filterExpression = GetDistrictFilterExpression() + " AND BPJSCode = '" + value + "'";
            Methods.getObject('GetBPJSReferenceList', filterExpression, function (result) {
                if (result != null) {
                    $('#<%=hdnKecamatanID.ClientID %>').val(result.BPJSCode); 
                    $('#<%=hdnKodeKecamatanBPJS.ClientID %>').val(result.BPJSCode);
                    $('#<%=txtNamaKecamatan.ClientID %>').val(result.BPJSName);

//                    if (result.BPJSReferenceInfo != null) {
//                        var bpjsReferenceInfo = result.BPJSReferenceInfo.split('|');

//                        if (bpjsReferenceInfo[0] != '') {
//                            $('#<%=hdnKodeKecamatanBPJS.ClientID %>').val(bpjsReferenceInfo[0]);
//                        }
//                        else {
//                            $('#<%=hdnKodeKecamatanBPJS.ClientID %>').val(result.KodeKecamatan);
//                        }
//                    }
//                    else {
//                        $('#<%=hdnKodeKecamatanBPJS.ClientID %>').val(result.KodeKecamatan);
//                    }
                }
                else {
                    $('#<%=hdnKecamatanID.ClientID %>').val('0');
                    $('#<%=hdnKodeKecamatanBPJS.ClientID %>').val('');
                    $('#<%=txtKodeKecamatan.ClientID %>').val('');
                    $('#<%=txtNamaKecamatan.ClientID %>').val('');
                }
            });
        }
        //#endregion

        OnIsAccidentChanged();

        $('.lnkSuratKontrol a').live('click', function () {
            var suratKontrol = $(this).closest('tr').find('.keyField').html();
            $("#divSEPContent").show();
            $("#divGetRencanaKontrol").hide();
            $('#btnMPEntryPopupSave').css('display', '');
            $('#btnMPProposeSEPCtl').css('display', '');
            $('#btnMPProposeFPCtl').css('display', '');
            $('#btnMPGenerateSEPCtl').css('display', '');
            $('#btnBackToSEPContent').css('display', 'none');
            $('#btnGetRencanaKontrol').css('display', '');
            $('#<%=txtNoSuratKontrol.ClientID %>').val(suratKontrol);
            reinitMPButton();
        });
    });

    //#region Referral Number
    $('#<%:lblReferralNo.ClientID %>.lblLink').click(function () {
        alert("Cari Data Rujukan berdasarkan Nomor Kartu");
    });
    //#endregion

    $("#<%:chkIsAccident.ClientID %>").on("change", function (e) {
        OnIsAccidentChanged();
    });

    function OnIsAccidentChanged() {
        if ($('#<%:chkIsAccident.ClientID %>').is(":checked")) {
            $('#<%:trAccidentLocationCtl.ClientID %>').css('display', '');
            $('#<%:trAccidentLocation2.ClientID %>').css('display', '');
            $('#<%:trAccidentLocation3.ClientID %>').css('display', '');
            $('#<%:trAccidentLocation4.ClientID %>').css('display', '');
            $('#<%:trSuplesi.ClientID %>').css('display', '');
            $('#<%:trAccidentPayor1.ClientID %>').css('display', '');
        }
        else {
            $('#<%:trAccidentLocationCtl.ClientID %>').css('display', 'none');
            $('#<%:trAccidentLocation2.ClientID %>').css('display', 'none');
            $('#<%:trAccidentLocation3.ClientID %>').css('display', 'none');
            $('#<%:trAccidentLocation4.ClientID %>').css('display', 'none');
            $('#<%:trSuplesi.ClientID %>').css('display', 'none');
            $('#<%:trAccidentPayor1.ClientID %>').css('display', 'none');
        }
    }

    //#region Search
    $("#btnSearchPeserta").on("click", function (e) {
        e.preventDefault();
        if ($('#<%=txtNoPeserta.ClientID %>').val() == '')
            showToast('Warning', "Nomor Peserta harus diisi!");
        else {
            var noPeserta = $('#<%=txtNoPeserta.ClientID %>').val();
            var date = Methods.getDatePickerDate($('#<%=txtTglSEP.ClientID %>').val());
            var tglSEP = Methods.dateToYMD(date);

            if ($('#<%=hdnIsBridgingBPJSVClaimVersion.ClientID %>').val() == Constant.VersionBridgingBPJSVClaim.v1_0) {
                BPJSService.getPeserta(noPeserta, tglSEP, function (result) {
                    GetPesertaHandler(result);
                });
            } else {
                BPJSService.getPesertaMedinfrasAPI(noPeserta, tglSEP, function (result) {
                    GetPesertaHandler(result);
                });
            }
        }
    });

    $("#btnSearchNIK").on("click", function (e) {
        e.preventDefault();
        if ($('#<%=txtNIK.ClientID %>').val() == '')
            showToast("Warning", "Nomor NIK harus diisi!");
        else {
            var noNIK = $('#<%=txtNIK.ClientID %>').val();
            var date = Methods.getDatePickerDate($('#<%=txtTglSEP.ClientID %>').val());
            var tglSEP = Methods.dateToYMD(date);

            if ($('#<%=hdnIsBridgingBPJSVClaimVersion.ClientID %>').val() == Constant.VersionBridgingBPJSVClaim.v1_0) {
                BPJSService.getPesertaByNIK(noNIK, tglSEP, function (result) {
                    GetPesertaHandler(result);
                });
            } else {
                BPJSService.getPesertaByNIKMedinfrasAPI(noNIK, tglSEP, function (result) {
                    GetPesertaHandler(result);
                });
            }
        }
    });

    $("#btnSearchNoSEP").on("click", function (e) {
        e.preventDefault();
        if ($('#<%=txtNoSEP.ClientID %>').val() == '')
            showToast("Warning", "Nomor SEP harus diisi!");
        else {
            var noSEP = $('#<%=txtNoSEP.ClientID %>').val();
            if ($('#<%=hdnIsBridgingBPJSVClaimVersion.ClientID %>').val() == Constant.VersionBridgingBPJSVClaim.v1_0) {
                BPJSService.getSEPInfo1(noSEP, function (result) {
                    GetSearchSEPHandler(result);
                });
            } else {
                BPJSService.getSEPInfo1MedinfrasAPI(noSEP, function (result) {
                    GetSearchSEPHandler(result);
                });
            }
        }
    });
    //#endregion

    function GetPesertaHandler(result) {
        try {
            var resultInfo = result.split('|');
            if (resultInfo[0] == "1") {
                var obj = jQuery.parseJSON(resultInfo[1]);
                $('#<%=txtNoPeserta.ClientID %>').val(obj.response.peserta.noKartu);
                $('#<%=txtNamaPeserta.ClientID %>').val(obj.response.peserta.nama);
                var tglLahir = obj.response.peserta.tglLahir;
                var tgl = tglLahir.split(" ");
                var arrDate = tgl[0].split("-");
                var strDate = arrDate[0] + arrDate[1] + arrDate[2]
                var date = Methods.stringToDate(strDate);
                $('#<%=txtDOB.ClientID %>').val(Methods.dateToDMY(date));
                $('#<%=txtJenisPeserta.ClientID %>').val(obj.response.peserta.jenisPeserta.keterangan);
                $('#<%=hdnKdJenisPeserta.ClientID %>').val(obj.response.peserta.hakKelas.kode);
                $('#<%=hdnKdKelas.ClientID %>').val(obj.response.peserta.hakKelas.kode);
                $('#<%=txtKelas.ClientID %>').val(obj.response.peserta.hakKelas.kode + ' - ' + obj.response.peserta.hakKelas.keterangan);
                $('#<%=txtNamaFaskes.ClientID %>').val(obj.response.peserta.provUmum.kdProvider + ' - ' + obj.response.peserta.provUmum.nmProvider.trim());
                if (obj.response.peserta.sex == 'P')
                    $('#<%=txtGender.ClientID %>').val('Perempuan');
                else
                    $('#<%=txtGender.ClientID %>').val('Laki-laki');

                if (obj.response.peserta.statusPeserta.kode.trim() != "0") {
                    showToast("Status Peserta", 'Error Message : ' + obj.response.peserta.statusPeserta.keterangan.trim());
                }
            }
            else {
                showToast('BPJS-Bridging', 'Error Message : ' + resultInfo[2]);
                ResetDataPeserta();
            }
        }
        catch (err) {
            showToast('BPJS-Bridging', 'Error Message : ' + err.Description);
            ResetDataPeserta();
        }
    }

    function GetSearchSEPHandler(result) {
        try {
            var noKartu = $('#<%=txtNoPeserta.ClientID %>').val();
            var resultInfo = result.split('|');
            if (resultInfo[0] == "1") {
                var obj = jQuery.parseJSON(resultInfo[1]);
                var noSep = obj.response.noSep;
                var noKartuSEP = obj.response.noKartu;

                if (noKartu != noKartuSEP) {
                    showToast('BPJS-Bridging', 'Error Message : No. Kartu BPJS dari SEP ' + noSep + ' tidak sesuai dengan No. Kartu Peserta');
                }
            }
            else {
                showToast('BPJS-Bridging', 'Error Message : ' + resultInfo[2]);
                ResetDataPeserta();
            }
        }
        catch (err) {
            showToast('BPJS-Bridging', 'Error Message : ' + err.Description);
            ResetDataPeserta();
        }
    }

    function ResetDataPeserta() {
        $('#<%=txtNIK.ClientID %>').val('');
        $('#<%=txtNamaPeserta.ClientID %>').val('');
        $('#<%=txtDOB.ClientID %>').val('');
        $('#<%=txtJenisPeserta.ClientID %>').val('');
        $('#<%=hdnKdJenisPeserta.ClientID %>').val('');
        $('#<%=txtKelas.ClientID %>').val('');
    }

    $("#btnSearchNoRujukan").on("click", function (e) {
        e.preventDefault();
        if ($('#<%=txtNoRujukan.ClientID %>').val() == '') {
            showToast("DATA RUJUKAN", "Nomor Rujukan harus diisi!");
        }
        else {
            var noRujukan = $('#<%=txtNoRujukan.ClientID %>').val();
            var asalRujukanInString = $('#<%=hdnAsalRujukan.ClientID %>').val();
            var asalRujukan = '';

            if (asalRujukanInString = '1') {
                asalRujukan = 'X105^006';
            } else {
                asalRujukan = 'X105^003';
            }

            if ($('#<%=hdnIsBridgingBPJSVClaimVersion.ClientID %>').val() == Constant.VersionBridgingBPJSVClaim.v1_0) {
                BPJSService.getRujukan(noRujukan, asalRujukan, function (result) {
                    try {
                        var resultInfo = result.split('|');
                        if (resultInfo[0] == "1") {
                            var obj = jQuery.parseJSON(resultInfo[1]);

                            var kodeUnit = obj.response.rujukan.poliRujukan.kode;
                            var namaUnit = obj.response.rujukan.poliRujukan.nama;

                            if (kodeUnit != $('#<%=hdnBPJSReferenceInfoKodeUnit.ClientID %>').val()) {
                                showToast("DATA RUJUKAN", 'Poli Rujukan dari Nomor Rujukan ' + noRujukan + ' tidak sesuai dengan pendaftaran pasien ! (' + kodeUnit + ' - ' + namaUnit + ')');
                                $('#<%=txtNoRujukan.ClientID %>').val('')
                            }
                            else {

                                var filterExpressionDiagnose = "BPJSReferenceInfo = '" + obj.response.rujukan.diagnosa.kode + "'";
                                Methods.getObject('GetDiagnoseList', filterExpressionDiagnose, function (resultCheck) {
                                    if (resultCheck != null) {
                                        $('#<%=txtDiagnoseCode.ClientID %>').val(resultCheck.DiagnoseID);
                                        $('#<%=txtDiagnoseName.ClientID %>').val(resultCheck.DiagnoseName);
                                        $('#<%=hdnBPJSDiagnoseCodeCtl.ClientID %>').val(resultCheck.BPJSReferenceInfo);
                                    }
                                    else {
                                        $('#<%=txtDiagnoseCode.ClientID %>').val('');
                                        $('#<%=txtDiagnoseName.ClientID %>').val('');
                                        $('#<%=hdnBPJSDiagnoseCodeCtl.ClientID %>').val('');
                                    }
                                });

                                var tglRujukan = obj.response.rujukan.tglKunjungan;
                                var tgl = tglRujukan.split(" ");
                                var arrDate = tgl[0].split("-");
                                var strDate = arrDate[0] + arrDate[1] + arrDate[2]
                                var date = Methods.stringToDate(strDate);

                                $('#<%=txtTglRujukan.ClientID %>').val(Methods.dateToDMY(date));
                                $('#<%=txtKodePoli.ClientID %>').val(obj.response.rujukan.poliRujukan.kode);
                                $('#<%=txtPoliTujuan.ClientID %>').val(obj.response.rujukan.poliRujukan.nama);
                                $('#<%=txtKdPpkRujukan.ClientID %>').val(obj.response.rujukan.provPerujuk.kode);
                                $('#<%=txtPpkRujukan.ClientID %>').val(obj.response.rujukan.provPerujuk.nama);
                                $('#<%=txtKeluhan.ClientID %>').val(obj.response.rujukan.keluhan);
                            }
                        }
                        else {
                            showToast("DATA RUJUKAN : GAGAL", resultInfo[2]);
                        }
                    } catch (err) {
                        showToast("DATA RUJUKAN : GAGAL", err);
                        $('#<%=txtNoRujukan.ClientID %>').val('')
                        $('#<%=txtTglRujukan.ClientID %>').val('');
                        $('#<%=txtDiagnoseCode.ClientID %>').val('');
                        $('#<%=txtDiagnoseName.ClientID %>').val('');
                        $('#<%=hdnBPJSDiagnoseCodeCtl.ClientID %>').val('');
                        $('#<%=txtPoliTujuan.ClientID %>').val('');
                        $('#<%=txtKodePoli.ClientID %>').val('');
                        $('#<%=txtKdPpkRujukan.ClientID %>').val('');
                        $('#<%=txtPpkRujukan.ClientID %>').val('');
                    }
                });
            } else {
                BPJSService.getRujukanMedinfrasAPI(noRujukan, asalRujukan, function (result) {
                    try {
                        var resultInfo = result.split('|');
                        if (resultInfo[0] == "1") {
                            var obj = jQuery.parseJSON(resultInfo[1]);

                            var kodeUnit = obj.response.rujukan.poliRujukan.kode;
                            var namaUnit = obj.response.rujukan.poliRujukan.nama;

                            if (kodeUnit != $('#<%=hdnBPJSReferenceInfoKodeUnit.ClientID %>').val()) {
                                showToast("DATA RUJUKAN", 'Poli Rujukan dari Nomor Rujukan ' + noRujukan + ' tidak sesuai dengan pendaftaran pasien ! (' + kodeUnit + ' - ' + namaUnit + ')');
                                $('#<%=txtNoRujukan.ClientID %>').val('')
                            }
                            else {

                                var filterExpressionDiagnose = "BPJSReferenceInfo = '" + obj.response.rujukan.diagnosa.kode + "'";
                                Methods.getObject('GetDiagnoseList', filterExpressionDiagnose, function (resultCheck) {
                                    if (resultCheck != null) {
                                        $('#<%=txtDiagnoseCode.ClientID %>').val(resultCheck.DiagnoseID);
                                        $('#<%=txtDiagnoseName.ClientID %>').val(resultCheck.DiagnoseName);
                                        $('#<%=hdnBPJSDiagnoseCodeCtl.ClientID %>').val(resultCheck.BPJSReferenceInfo);
                                    }
                                    else {
                                        $('#<%=txtDiagnoseCode.ClientID %>').val('');
                                        $('#<%=txtDiagnoseName.ClientID %>').val('');
                                        $('#<%=hdnBPJSDiagnoseCodeCtl.ClientID %>').val('');
                                    }
                                });

                                var tglRujukan = obj.response.rujukan.tglKunjungan;
                                var tgl = tglRujukan.split(" ");
                                var arrDate = tgl[0].split("-");
                                var strDate = arrDate[0] + arrDate[1] + arrDate[2]
                                var date = Methods.stringToDate(strDate);

                                $('#<%=txtTglRujukan.ClientID %>').val(Methods.dateToDMY(date));
                                $('#<%=txtKodePoli.ClientID %>').val(obj.response.rujukan.poliRujukan.kode);
                                $('#<%=txtPoliTujuan.ClientID %>').val(obj.response.rujukan.poliRujukan.nama);
                                $('#<%=txtKdPpkRujukan.ClientID %>').val(obj.response.rujukan.provPerujuk.kode);
                                $('#<%=txtPpkRujukan.ClientID %>').val(obj.response.rujukan.provPerujuk.nama);
                                $('#<%=txtKeluhan.ClientID %>').val(obj.response.rujukan.keluhan);
                            }
                        }
                        else {
                            showToast("DATA RUJUKAN : GAGAL", resultInfo[2]);
                        }
                    } catch (err) {
                        showToast("DATA RUJUKAN : GAGAL", err);
                        $('#<%=txtNoRujukan.ClientID %>').val('')
                        $('#<%=txtTglRujukan.ClientID %>').val('');
                        $('#<%=txtDiagnoseCode.ClientID %>').val('');
                        $('#<%=txtDiagnoseName.ClientID %>').val('');
                        $('#<%=hdnBPJSDiagnoseCodeCtl.ClientID %>').val('');
                        $('#<%=txtPoliTujuan.ClientID %>').val('');
                        $('#<%=txtKodePoli.ClientID %>').val('');
                        $('#<%=txtKdPpkRujukan.ClientID %>').val('');
                        $('#<%=txtPpkRujukan.ClientID %>').val('');
                    }
                });
            }
        }
    });

    //#region Dokter Perujuk
    $('#lblDokterPerujuk.lblLink').live('click', function () {
        openSearchDialog('physician', "IsDeleted = 0", function (value) {
            $('#<%=txtDPJPDokterPerujuk.ClientID %>').val(value);
            ontxtDokterPerujukChanged(value);
        });
    });

    $('#<%=txtDPJPDokterPerujuk.ClientID %>').live('change', function () {
        ontxtDokterPerujukChanged($('#<%=txtDPJPDokterPerujuk.ClientID %>').val());
    });

    function ontxtDokterPerujukChanged(value) {
        var filterFix = "ParamedicCode = '" + value + "'";
        Methods.getObject('GetvParamedicMasterList', filterFix, function (result) {
            if (result != null) {
                if (result.BPJSReferenceInfo != null && result.BPJSReferenceInfo != "") {
                    var bpjsReferenceInfo = result.BPJSReferenceInfo.split('|');
                    var kodeDPJPKonsul = bpjsReferenceInfo[0];
                    var namaDPJP = bpjsReferenceInfo[1].split(';');
                    var namaDPJPKonsul = namaDPJP[0];

                    $('#<%=hdnKodePerujuk.ClientID %>').val(kodeDPJPKonsul);
                    $('#<%=txtDPJPDokterPerujuk.ClientID %>').val(kodeDPJPKonsul);
                    $('#<%=txtDokterPerujuk.ClientID %>').val(namaDPJPKonsul);
                }
                else {
                    $('#<%=hdnKodePerujuk.ClientID %>').val('');
                    $('#<%=txtDPJPDokterPerujuk.ClientID %>').val('');
                    $('#<%=txtDokterPerujuk.ClientID %>').val('');
                }
            } else {
                $('#<%=hdnKodePerujuk.ClientID %>').val('');
                $('#<%=txtDPJPDokterPerujuk.ClientID %>').val('');
                $('#<%=txtDokterPerujuk.ClientID %>').val('');
            }
        });
    }
    //#endregion

    //#region Poliklinik
    $('#lblSpesialis.lblLink').live('click', function () {
        openSearchDialog('vklaimpoli', '', function (value) {
            $('#<%=txtKodePoli.ClientID %>').val(value);
            ontxtvKlaimPoliCodeChanged(value);
        });
    });

    $('#<%=txtKodePoli.ClientID %>').live('change', function () {
        ontxtvKlaimPoliCodeChanged($(this).val());
    });

    function ontxtvKlaimPoliCodeChanged(value) {
        var filterExpression = "BPJSCode = '" + value + "'";
        Methods.getObject('GetvBPJSReferencePoliList', filterExpression, function (result) {
            if (result != null) {
                $('#<%=hdnKodePoli.ClientID %>').val(result.BPJSCode);
                $('#<%=txtKodePoli.ClientID %>').val(result.BPJSCode);
                $('#<%=txtPoliTujuan.ClientID %>').val(result.BPJSName);
            }
            else {
                $('#<%=txtKodePoli.ClientID %>').val('');
                $('#<%=txtPoliTujuan.ClientID %>').val('');
            }
        });
    }
    //#endregion

    //#region Poli Rujukan
    $('#lblPoliRujukan.lblLink').live('click', function () {
        openSearchDialog('vklaimpoli', '', function (value) {
            $('#<%=txtKodePoli.ClientID %>').val(value);
            ontxtvKlaimPoliRujukanCodeChanged(value);
        });
    });

    $('#<%=txtKodePoliRujukan.ClientID %>').live('change', function () {
        ontxtvKlaimPoliRujukanCodeChanged($(this).val());
    });

    function ontxtvKlaimPoliRujukanCodeChanged(value) {
        var filterExpression = "BPJSCode = '" + value + "'";
        Methods.getObject('GetvBPJSReferencePoliList', filterExpression, function (result) {
            if (result != null) {
                $('#<%=txtKodePoliRujukan.ClientID %>').val(result.BPJSCode);
                $('#<%=txtNamaPoliRujukan.ClientID %>').val(result.BPJSName);
            }
            else {
                $('#<%=txtKodePoliRujukan.ClientID %>').val('');
                $('#<%=txtNamaPoliRujukan.ClientID %>').val('');
            }
        });
    }
    //#endregion

    //#region SubPoliklinik
    $('#lblSubSpesialis.lblLink').live('click', function () {
        openSearchDialog('vklaimpoli', '', function (value) {
            $('#<%=txtKodeSubSpesialis.ClientID %>').val(value);
            ontxtvKlaimSubPoliCodeChanged(value);
        });
    });

    $('#<%=txtKodeSubSpesialis.ClientID %>').live('change', function () {
        ontxtvKlaimSubPoliCodeChanged($(this).val());
    });

    function ontxtvKlaimSubPoliCodeChanged(value) {
        var filterExpression = "BPJSCode = '" + value + "'";
        Methods.getObject('GetvBPJSReferencePoliList', filterExpression, function (result) {
            if (result != null) {
                $('#<%=txtKodeSubSpesialis.ClientID %>').val(result.BPJSCode);
                $('#<%=txtNamaSubSpesialis.ClientID %>').val(result.BPJSName);
            }
            else {
                $('#<%=txtKodeSubSpesialis.ClientID %>').val('');
                $('#<%=txtNamaSubSpesialis.ClientID %>').val('');
            }
        });
    }
    //#endregion

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
                $('#<%=hdnBPJSDiagnoseCodeCtl.ClientID %>').val(result.BPJSReferenceInfo);
            }
            else {
                $('#<%=txtDiagnoseCode.ClientID %>').val('');
                $('#<%=txtDiagnoseName.ClientID %>').val('');
                $('#<%=hdnBPJSDiagnoseCodeCtl.ClientID %>').val('');
            }
        });
    }
    //#endregion

    //#region Diagnose Rujukan
    $('#lblDiagnosaRujukan.lblLink').live('click', function () {
        openSearchDialog('diagnose', "IsDeleted = 0", function (value) {
            $('#<%=txtKodeDiagnosaRujukan.ClientID %>').val(value);
            onTxtDiagnosaRujukanChanged(value);
        });
    });

    $('#<%=txtKodeDiagnosaRujukan.ClientID %>').live('change', function () {
        onTxtDiagnosaRujukanChanged($(this).val());
    });

    function onTxtDiagnosaRujukanChanged(value) {
        var filterExpression = "DiagnoseID = '" + value + "' AND IsDeleted = 0";
        Methods.getObject('GetDiagnoseList', filterExpression, function (result) {
            if (result != null) {
                $('#<%=txtNamaDiagnosaRujukan.ClientID %>').val(result.DiagnoseName);
                $('#<%=hdnBPJSKodeDiagnosaRujukan.ClientID %>').val(result.BPJSReferenceInfo);
            }
            else {
                $('#<%=txtKodeDiagnosaRujukan.ClientID %>').val('');
                $('#<%=txtNamaDiagnosaRujukan.ClientID %>').val('');
                $('#<%=hdnBPJSKodeDiagnosaRujukan.ClientID %>').val('');
            }
        });
    }
    //#endregion

    //#region Referral Description
    function getReferralDescriptionFilterExpression() {
        var filterExpression = "GCReferrerGroup = '" + cboReferral.GetValue() + "' AND IsDeleted = 0";
        return filterExpression;
    }

    function getReferralParamedicFilterExpression() {
        var filterExpression = "GCParamedicMasterType = '" + Constant.ParamedicType.Physician + "'";
        return filterExpression;
    }

    $('#lblPPKDirujuk.lblLink').live('click', function () {
        openSearchDialog('bpjsreference', "GCBPJSObjectType = 'X358^005'", function (value) {
            $('#<%:txtKodePPKDirujuk.ClientID %>').val(value);
            onTxtReferralDescriptionCodeChanged(value);
        });
    });

    $('#<%:txtKodePPKDirujuk.ClientID %>').live('change', function () {
        onTxtReferralDescriptionCodeChanged($(this).val());
    });

    function onTxtReferralDescriptionCodeChanged(value) {
        var filterExpression = " BPJSCode = '" + value + "'";
        Methods.getObject('GetBPJSReferenceList', filterExpression, function (result) {
            if (result != null) {
                $('#<%:txtKodePPKDirujuk.ClientID %>').val(result.BPJSCode);
                $('#<%:txtNamaPPKDirujuk.ClientID %>').val(result.BPJSName);
            }
            else {
                $('#<%:txtKodePPKDirujuk.ClientID %>').val('');
                $('#<%:txtNamaPPKDirujuk.ClientID %>').val('');
            }
        });
    }
    //#endregion

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
    //#endregion

    //#region Pembiayaan
    $('#lblPembiayaan.lblLink').live('click', function () {
        var filterExpression = "ParentID = '" + Constant.BPJSPayer.X496 + "'";
        openSearchDialog('bpjsPembiayaanPenanggungJawab', filterExpression, function (value) {
            $('#<%=txtPembiayaanCode.ClientID %>').val(value);
            ontxtPembiayaanChanged(value);
        });
    });

    $('#<%=txtPembiayaanCode.ClientID %>').live('change', function () {
        ontxtPembiayaanChanged($(this).val());
    });

    function ontxtPembiayaanChanged(value) {
        var filterExpression = "StandardCodeID = '" + value + "'";
        Methods.getObject('GetStandardCodeList', filterExpression, function (result) {
            if (result != null) {
                $('#<%=txtPembiayaanCode.ClientID %>').val(result.TagProperty);
                $('#<%=txtPembiayaanName.ClientID %>').val(result.StandardCodeName);
            }
            else {
                $('#<%=txtPembiayaanCode.ClientID %>').val('');
                $('#<%=txtPembiayaanName.ClientID %>').val('');
            }
        });
    }
    //#endregion

    //#region Penanggung Jawab
    $('#lblPenanggungJawab.lblLink').live('click', function () {
        var filterExpression = "ParentID = '" + Constant.BPJSPayer.X496 + "'";
        openSearchDialog('bpjsPembiayaanPenanggungJawab', filterExpression, function (value) {
            $('#<%=txtPenanggungJawabCode.ClientID %>').val(value);
            ontxtPenanggungJawabChanged(value);
        });
    });

    $('#<%=txtPenanggungJawabCode.ClientID %>').live('change', function () {
        ontxtPenanggungJawabChanged($(this).val());
    });

    function ontxtPenanggungJawabChanged(value) {
        var filterExpression = "StandardCodeID = '" + value + "'";
        Methods.getObject('GetStandardCodeList', filterExpression, function (result) {
            if (result != null) {
                $('#<%=txtPenanggungJawabCode.ClientID %>').val(result.TagProperty);
                $('#<%=txtPenanggungJawabName.ClientID %>').val(result.StandardCodeName);
            }
            else {
                $('#<%=txtPenanggungJawabCode.ClientID %>').val('');
                $('#<%=txtPenanggungJawabName.ClientID %>').val('');
            }
        });
    }
    //#endregion

    //#region Rencana Kontrol
    $('#lblRencanaKontrol.lblLink').live('click', function () {
        //        var filterExpression = "NoPeserta = '" + $('#<%=txtNoPeserta.ClientID %>').val() + "' AND NoSuratKontrolManual != ''";
        //        openSearchDialog('bpjsNoRencanaKontrol', filterExpression, function (value) {
        //            $('#<%=txtNoSuratKontrol.ClientID %>').val(value);
        //            ontxtNoSuratKontrolChanged(value);
        //        });

        $("#divSEPContent").hide();
        $("#divGetRencanaKontrol").show();
        $('#btnMPEntryPopupSave').css('display', 'none');
        $('#btnMPProposeSEPCtl').css('display', 'none');
        $('#btnMPProposeFPCtl').css('display', 'none');
        $('#btnMPGenerateSEPCtl').css('display', 'none');
        $('#btnBackToSEPContent').css('display', '');
        $('#btnGetRencanaKontrol').css('display', 'none');

    });

    $('#<%=txtNoRencanaKontrolBerikutnya.ClientID %>').live('change', function () {
        //        ontxtNoSuratKontrolChanged($(this).val());
        reinitMPButton();
    });

    function ontxtNoSuratKontrolChanged(value) {
        var filterExpression = "NoSuratKontrolManual = '" + value + "'";
        Methods.getObject('GetvRegistrationBPJSList', filterExpression, function (result) {
            if (result != null) {
                $('#<%=txtNoRencanaKontrolBerikutnya.ClientID %>').val(result.NoSuratKontrolManual);
            }
            else {
                $('#<%=txtNoRencanaKontrolBerikutnya.ClientID %>').val('');
            }
        });
    }
    //#endregion

    $('#btnMPEntryPopupSave').click(function () {
        cbpPopupProcess.PerformCallback('saveandclose');
    });


    //#region Generate SEP
    $('#btnMPGenerateSEPCtl').click(function () {
        var validationMessage = ValidateParameter();
        if (validationMessage != '') {
            alert(validationMessage);
        }

        if ($('#<%=txtNoSEP.ClientID %>').val() == '' && validationMessage == '') {
            if ($('#<%=hdnIsBridgingBPJSVClaimVersion.ClientID %>').val() == Constant.VersionBridgingBPJSVClaim.v1_0) {
                var date = Methods.getDatePickerDate($('#<%=txtTglSEP.ClientID %>').val());
                var tglSEP = Methods.dateToYMD(date);
                date = Methods.getDatePickerDate($('#<%=txtTglRujukan.ClientID %>').val());
                var dateRujukan = Methods.dateToYMD(date);

                var noRujukan = $('#<%=txtNoRujukan.ClientID %>').val();
                var noKartu = $('#<%=txtNoPeserta.ClientID %>').val();
                var diagnosa = $('#<%=hdnBPJSDiagnoseCodeCtl.ClientID %>').val();
                var ppkRujukan = $('#<%=txtKdPpkRujukan.ClientID %>').val();

                var poliTujuan = '';
                var poli = $('#<%=txtKodePoli.ClientID %>').val();
                var subSpesialis = $('#<%=txtKodeSubSpesialis.ClientID %>').val();
                if (subSpesialis == '') {
                    poliTujuan = $('#<%=txtKodePoli.ClientID %>').val();
                } else {
                    poliTujuan = $('#<%=txtKodeSubSpesialis.ClientID %>').val();
                }

                var poliEksekutif = $('#<%:hdnIsPoliExecutive.ClientID %>').val();
                var jnsPelayanan = $('#<%=hdnKdPelayanan.ClientID %>').val();
                var kelasRawat = $('#<%=hdnChargeClassSEP.ClientID %>').val();
                var hakKelas = $('#<%=txtKelas.ClientID %>').val();

                var hakKelasSplit = hakKelas.split(' ');
                var klsRawat = '';
                if (kelasRawat != hakKelasSplit[0]) {
                    if (kelasRawat < hakKelasSplit[0]) {
                        klsRawat = hakKelasSplit[0];
                    } else if (kelasRawat > hakKelasSplit[0]) {
                        klsRawat = $('#<%=hdnChargeClassSEP.ClientID %>').val();
                    } else {
                        klsRawat = $('#<%=hdnChargeClassSEP.ClientID %>').val();
                    }
                } else {
                    klsRawat = $('#<%=hdnChargeClassSEP.ClientID %>').val();
                }

                var catatan = $('#<%=txtNotes.ClientID %>').val();
                var medicalNo = $('#<%=txtMRN.ClientID %>').val();

                var cob = "0";
                if ($('#<%:chkIsCOB.ClientID %>').is(":checked")) {
                    cob = "1";
                }

                var noSKDP = $('#<%:txtNoSuratKontrol.ClientID %>').val();

                var kodeDPJP = "";
                var kodeDokterDPJP = $('#<%:hdnKodeDPJP.ClientID %>').val();
                var kodeDPJPPerujuk = $('#<%:hdnKodePerujuk.ClientID %>').val();
                if (kodeDPJPPerujuk != "" && kodeDPJPPerujuk != null) {
                    kodeDPJP = kodeDPJPPerujuk;
                } else {
                    kodeDPJP = kodeDokterDPJP;
                }

                var katarak = "0";
                if ($('#<%:chkIsCataract.ClientID %>').is(':checked')) {
                    katarak = '1';
                }

                var lakaLantas = "0";
                var lokasiLaka = "";
                var penjamin = "";
                var suplesi = "0";
                if ($('#<%:chkIsSuplesi.ClientID %>').is(':checked')) {
                    suplesi = '1';
                }
                var noSepSuplesi = $('#<%=txtNoSEPSuplesi.ClientID %>').val();
                var kodeKecamatan = $('#<%:hdnKodeKecamatanBPJS.ClientID %>').val();
                var kodeKabupaten = $('#<%:hdnKodeKabupatenBPJS.ClientID %>').val();
                var kodePropinsi = $('#<%:hdnKodePropinsiBPJS.ClientID %>').val();

                if ($('#<%:chkIsAccident.ClientID %>').is(":checked")) {
                    lakaLantas = "1";
                    lokasiLaka = $('#<%=txtAccidentLocationCtl.ClientID %>').val();
                    if ($('#<%:chkBPJSAccidentPayer1.ClientID %>').is(':checked')) {
                        penjamin = '1';
                    }
                    if ($('#<%:chkBPJSAccidentPayer2.ClientID %>').is(':checked')) {
                        if (penjamin != '') {
                            penjamin = penjamin + ',' + '2';
                        }
                        else {
                            penjamin = '2';
                        }
                    }
                    if ($('#<%:chkBPJSAccidentPayer3.ClientID %>').is(':checked')) {
                        if (penjamin != '') {
                            penjamin = penjamin + ',' + '3';
                        }
                        else {
                            penjamin = '3';
                        }
                    }
                    if ($('#<%:chkBPJSAccidentPayer4.ClientID %>').is(':checked')) {
                        if (penjamin != '') {
                            penjamin = penjamin + ',' + '4';
                        }
                        else {
                            penjamin = '4';
                        }
                    }
                }

                var asalRujukan = $('#<%=hdnAsalRujukan.ClientID %>').val();
                var mobilePhoneNo = $('#<%=txtMobilePhoneNo1.ClientID %>').val();

                BPJSService.generateNoSEP(noKartu, tglSEP, dateRujukan, noRujukan, ppkRujukan, jnsPelayanan, catatan, diagnosa,
                                                    poliTujuan, klsRawat, lakaLantas, lokasiLaka, medicalNo, asalRujukan, cob, poliEksekutif, mobilePhoneNo, penjamin,
                                                    katarak, suplesi, noSepSuplesi, kodePropinsi, kodeKabupaten, kodeKecamatan, noSKDP, kodeDPJP, function (result) {
                                                        try {
                                                            var obj = result.split('|');
                                                            if (obj[0] == "1") {
                                                                $('#<%=txtNoSEP.ClientID %>').val(obj[1]);
                                                                $('#<%=txtNoSEP.ClientID %>').attr('readonly', 'readonly');
                                                                cbpPopupProcess.PerformCallback('save');

                                                                if ($('#<%=hdnIsBridgingEklaim.ClientID %>').val() == "1") {
                                                                    cbpPopupProcess.PerformCallback('newClaim');
                                                                }
                                                                showToast('Pembuatan SEP : SUKSES', 'No. SEP : ' + $('#<%=txtNoSEP.ClientID %>').val());
                                                            }
                                                            else {
                                                                showToast('Pembuatan SEP : GAGAL', 'Error Message : ' + obj[2]);
                                                            }

                                                        } catch (err) {
                                                            alert(err);
                                                            $('#<%=txtNoSEP.ClientID %>').val('');
                                                        }
                                                    });
            } else {
                var noKartu = $('#<%=txtNoPeserta.ClientID %>').val();
                Methods.getObject('GetRegistrationList', "RegistrationNo = '" + $('#<%=hdnRegistrationNo.ClientID %>').val() + "'", function (resultReg) {
                    if (resultReg != null) {
                        if (resultReg.RegistrationID > 0) {
                            noKartu = $('#<%=txtNoPeserta.ClientID %>').val() + ';' + resultReg.RegistrationID;
                        }
                    }
                });

                var date = Methods.getDatePickerDate($('#<%=txtTglSEP.ClientID %>').val());
                var tglSEP = Methods.dateToYMD(date);
                var ppkPelayanan = $('#<%=txtKdPpkRujukan.ClientID %>').val();
                var jnsPelayanan = $('#<%=hdnKdPelayanan.ClientID %>').val();
                var hakKelas = $('#<%=txtKelas.ClientID %>').val();
                var hakKelasSplit = hakKelas.split(' ');
                var klsRawatHak = hakKelasSplit[0];
                var kelasRawat = $('#<%=hdnChargeClassSEP.ClientID %>').val();

                var klsRawatNaik = "";
                if (klsRawatHak != kelasRawat) {
                    var klsRawatNaik = $('#<%=hdnKelasNaikBPJS.ClientID %>').val();
                }

                var pembiayaan = $('#<%=txtPembiayaanCode.ClientID %>').val();
                var penanggungJawab = $('#<%=txtPenanggungJawabName.ClientID %>').val();
                var medicalNo = $('#<%=txtMRN.ClientID %>').val();
                var asalRujukan = $('#<%=hdnAsalRujukan.ClientID %>').val();

                date = Methods.getDatePickerDate($('#<%=txtTglRujukan.ClientID %>').val());
                var tglRujukan = Methods.dateToYMD(date);

                var noRujukan = $('#<%=txtNoRujukan.ClientID %>').val();
                var ppkRujukan = $('#<%=txtKdPpkRujukan.ClientID %>').val();
                var catatan = $('#<%=txtNotes.ClientID %>').val();
                var diagnosa = $('#<%=hdnBPJSDiagnoseCodeCtl.ClientID %>').val();
                var poliTujuan = '';
                var poli = $('#<%=txtKodePoli.ClientID %>').val();
                var subSpesialis = $('#<%=txtKodeSubSpesialis.ClientID %>').val();
                if (subSpesialis == '') {
                    poliTujuan = $('#<%=txtKodePoli.ClientID %>').val();
                } else {
                    poliTujuan = $('#<%=txtKodeSubSpesialis.ClientID %>').val();
                }

                var poliEksekutif = $('#<%:hdnIsPoliExecutive.ClientID %>').val();
                var cob = "0";
                if ($('#<%:chkIsCOB.ClientID %>').is(":checked")) {
                    cob = "1";
                }

                var katarak = "0";
                if ($('#<%:chkIsCataract.ClientID %>').is(':checked')) {
                    katarak = '1';
                }

                var lakaLantas = "0";
                var lokasiLaka = "";
                var penjamin = "";
                var suplesi = "0";
                var keterangan = "";
                if ($('#<%:chkIsSuplesi.ClientID %>').is(':checked')) {
                    suplesi = '1';
                }
                var noSepSuplesi = $('#<%=txtNoSEPSuplesi.ClientID %>').val();
                var kodeKecamatan = $('#<%:hdnKodeKecamatanBPJS.ClientID %>').val();
                var kodeKabupaten = $('#<%:hdnKodeKabupatenBPJS.ClientID %>').val();
                var kodePropinsi = $('#<%:hdnKodePropinsiBPJS.ClientID %>').val();

                if ($('#<%:chkIsAccident.ClientID %>').is(":checked")) {
                    lakaLantas = "1";
                    lokasiLaka = $('#<%=txtAccidentLocationCtl.ClientID %>').val();
                    if ($('#<%:chkBPJSAccidentPayer1.ClientID %>').is(':checked')) {
                        penjamin = '1';
                    }
                    if ($('#<%:chkBPJSAccidentPayer2.ClientID %>').is(':checked')) {
                        if (penjamin != '') {
                            penjamin = penjamin + ',' + '2';
                        }
                        else {
                            penjamin = '2';
                        }
                    }
                    if ($('#<%:chkBPJSAccidentPayer3.ClientID %>').is(':checked')) {
                        if (penjamin != '') {
                            penjamin = penjamin + ',' + '3';
                        }
                        else {
                            penjamin = '3';
                        }
                    }
                    if ($('#<%:chkBPJSAccidentPayer4.ClientID %>').is(':checked')) {
                        if (penjamin != '') {
                            penjamin = penjamin + ',' + '4';
                        }
                        else {
                            penjamin = '4';
                        }
                    }
                }

                var tujuanKunj = cboTujuanKunjungan.GetValue();
                if (cboProsedur.GetValue() == null) {
                    var flagProcedure = '';
                } else {
                    var flagProcedure = cboProsedur.GetValue();
                }

                if (cboProsedurPenunjang.GetValue() == null) {
                    var kdPenunjang = '';
                } else {
                    var kdPenunjang = cboProsedurPenunjang.GetValue();
                }

                if (cboAsesmenPelayanan.GetValue() == null) {
                    var assesmentPel = '';
                } else {
                    var assesmentPel = cboAsesmenPelayanan.GetValue();
                }

                var noSKDP

                var noSPRI = $('#<%:txtNoSPRI.ClientID %>').val();
                var noSuratKontrol = $('#<%:txtNoSuratKontrol.ClientID %>').val();
                var noSurat = "";

                if (noSPRI != "") {
                    noSurat = noSPRI;
                } else {
                    noSurat = noSuratKontrol;
                }

                var kodeDokterDPJP = $('#<%:hdnKodeDPJP.ClientID %>').val();
                var kodeDPJPPerujuk = $('#<%:hdnKodePerujuk.ClientID %>').val();
                if (kodeDPJPPerujuk != "" && kodeDPJPPerujuk != null) {
                    kodeDPJP = kodeDPJPPerujuk;
                } else {
                    kodeDPJP = kodeDokterDPJP;
                }

                var dpjpLayan = ''
                if (jnsPelayanan != '1') {
                    dpjpLayan = kodeDPJP;
                }

                var noTelp = $('#<%=txtMobilePhoneNo1.ClientID %>').val();
                var nik = $('#<%=txtNIK.ClientID %>').val();

                BPJSService.generateNoSEPMedinfrasAPI(noKartu, tglSEP, ppkPelayanan, jnsPelayanan, klsRawatHak, klsRawatNaik, pembiayaan, penanggungJawab, medicalNo, asalRujukan,
									tglRujukan, noRujukan, ppkRujukan, catatan, diagnosa, poliTujuan, poliEksekutif, cob, katarak, lakaLantas, keterangan, suplesi,
									noSepSuplesi, kodePropinsi, kodeKabupaten, kodeKecamatan, tujuanKunj, flagProcedure, kdPenunjang, assesmentPel, noSurat,
									kodeDPJP, dpjpLayan, noTelp, nik, function (result) {
									    try {
									        var obj = result.split('|');
									        if (obj[0] == "1") {
									            $('#<%=txtNoSEP.ClientID %>').val(obj[1]);
									            $('#<%=txtNoSEP.ClientID %>').attr('readonly', 'readonly');
									            cbpPopupProcess.PerformCallback('save');
									            showToast('Pembuatan SEP : SUKSES', 'No. SEP : ' + $('#<%=txtNoSEP.ClientID %>').val());
									        }
									        else {
									            showToast('Pembuatan SEP : GAGAL', 'Error Message : ' + obj[2]);
									        }

									    } catch (err) {
									        alert(err);
									        $('#<%=txtNoSEP.ClientID %>').val('');
									    }
									});
            }
        }
    });

    //#endregion

    //#region ProposeSEP
    $('#btnMPProposeSEPCtl').click(function () {
        if ($('#<%=txtNoSEP.ClientID %>').val() == '') {
            var date = Methods.getDatePickerDate($('#<%=txtTglSEP.ClientID %>').val());
            var dateSEP = Methods.dateToYMD(date) + ' ' + $('#<%=txtJamSEP.ClientID %>').val();

            var noKartu = $('#<%=txtNoPeserta.ClientID %>').val();
            var jnsPelayanan = $('#<%=hdnKdPelayanan.ClientID %>').val() + ";1";
            var catatan = $('#<%=txtNotes.ClientID %>').val();

            if (catatan != '') {
                if ($('#<%=hdnIsBridgingBPJSVClaimVersion.ClientID %>').val() == Constant.VersionBridgingBPJSVClaim.v1_0) {
                    BPJSService.pengajuanSEP(noKartu, dateSEP, jnsPelayanan, catatan, function (result) {
                        try {
                            var obj = result.split('|');
                            if (obj[0] == "1") {
                                cbpPopupProcess.PerformCallback('propose');
                                showToast('Pengajuan SEP : SUKSES', 'No. Kartu : ' + $('#<%=txtNoPeserta.ClientID %>').val());
                            }
                            else {
                                showToast('Pengajuan SEP : GAGAL', 'Error Message : ' + obj[2]);
                            }

                        } catch (err) {
                            alert(err);
                        }
                    });
                } else {
                    BPJSService.pengajuanSEPMedinfrasAPI(noKartu, dateSEP, jnsPelayanan, catatan, function (result) {
                        try {
                            var obj = result.split('|');
                            if (obj[0] == "1") {
                                cbpPopupProcess.PerformCallback('propose');
                                showToast('Pengajuan SEP : SUKSES', 'No. Kartu : ' + $('#<%=txtNoPeserta.ClientID %>').val());
                            }
                            else {
                                showToast('Pengajuan SEP : GAGAL', 'Error Message : ' + obj[2]);
                            }

                        } catch (err) {
                            alert(err);
                        }
                    });
                }
            }
            else {
                showToast('Pengajuan SEP : GAGAL', 'Catatan atau Alasan Pengajuan SEP harus diisi!');
            }
        }
    });
    //#endregion

    //#region ProposeFP
    $('#btnMPProposeFPCtl').click(function () {
        if ($('#<%=txtNoSEP.ClientID %>').val() == '') {
            var date = Methods.getDatePickerDate($('#<%=txtTglSEP.ClientID %>').val());
            var dateSEP = Methods.dateToYMD(date) + ' ' + $('#<%=txtJamSEP.ClientID %>').val();

            var noKartu = $('#<%=txtNoPeserta.ClientID %>').val();
            var jnsPelayanan = $('#<%=hdnKdPelayanan.ClientID %>').val() + ";2";
            var catatan = $('#<%=txtNotes.ClientID %>').val();

            if (catatan != '') {
                BPJSService.pengajuanSEPMedinfrasAPI(noKartu, dateSEP, jnsPelayanan, catatan, function (result) {
                    try {
                        var obj = result.split('|');
                        if (obj[0] == "1") {
                            cbpPopupProcess.PerformCallback('propose');
                            showToast('Pengajuan Fingerprint : SUKSES', 'No. Kartu : ' + $('#<%=txtNoPeserta.ClientID %>').val());
                        }
                        else {
                            showToast('Pengajuan Fingerprint : GAGAL', 'Error Message : ' + obj[2]);
                        }

                    } catch (err) {
                        alert(err);
                    }
                });
            }
            else {
                showToast('Pengajuan Fingerprint : GAGAL', 'Catatan atau Alasan Pengajuan Fingerprint harus diisi!');
            }
        }
    });
    //#endregion

    //#region Update SEP
    $('#btnMPUpdateSEPCtl').click(function () {
        if ($('#<%=txtNoSEP.ClientID %>').val() != '') {
            if ($('#<%=hdnIsBridgingBPJSVClaimVersion.ClientID %>').val() == Constant.VersionBridgingBPJSVClaim.v1_0) {
                var noSep = $('#<%=txtNoSEP.ClientID %>').val();
                var date = Methods.getDatePickerDate($('#<%=txtTglSEP.ClientID %>').val());
                var dateSEP = Methods.dateToYMD(date) + ' ' + $('#<%=txtJamSEP.ClientID %>').val();
                date = Methods.getDatePickerDate($('#<%=txtTglRujukan.ClientID %>').val());
                var dateRujukan = Methods.dateToYMD(date) + ' ' + $('#<%=txtJamSEP.ClientID %>').val();

                var noRujukan = $('#<%=txtNoRujukan.ClientID %>').val();
                var noKartu = $('#<%=txtNoPeserta.ClientID %>').val();
                var diagnosa = $('#<%=hdnBPJSDiagnoseCodeCtl.ClientID %>').val();
                var ppkRujukan = $('#<%=txtKdPpkRujukan.ClientID %>').val();
                var poliTujuan = $('#<%=txtKodePoli.ClientID %>').val();
                var poliEksekutif = $('#<%:hdnIsPoliExecutive.ClientID %>').val();
                var jnsPelayanan = $('#<%=hdnKdPelayanan.ClientID %>').val();
                var klsRawat = $('#<%=hdnChargeClassSEP.ClientID %>').val();
                var catatan = $('#<%=txtNotes.ClientID %>').val();
                var medicalNo = $('#<%=txtMRN.ClientID %>').val();
                var cob = "0";
                if ($('#<%:chkIsCOB.ClientID %>').is(":checked")) {
                    cob = "1";
                }
                var noSKDP = $('#<%:hdnNoSKDP.ClientID %>').val();
                var kodeDPJP = $('#<%:hdnKodeDPJP.ClientID %>').val();
                var katarak = "0";
                if ($('#<%:chkIsCataract.ClientID %>').is(':checked')) {
                    katarak = '1';
                }

                var lakaLantas = "0";
                var lokasiLaka = "";
                var penjamin = "";
                var suplesi = "0";
                if ($('#<%:chkIsSuplesi.ClientID %>').is(':checked')) {
                    suplesi = '1';
                }
                var noSepSuplesi = $('#<%=txtNoSEPSuplesi.ClientID %>').val();
                var kodeKecamatan = $('#<%:hdnKodeKecamatanBPJS.ClientID %>').val();
                var kodeKabupaten = $('#<%:hdnKodeKabupatenBPJS.ClientID %>').val();
                var kodePropinsi = $('#<%:hdnKodePropinsiBPJS.ClientID %>').val();
                if ($('#<%:chkIsAccident.ClientID %>').is(":checked")) {
                    lakaLantas = "1";
                    lokasiLaka = $('#<%=txtAccidentLocationCtl.ClientID %>').val();
                    if ($('#<%:chkBPJSAccidentPayer1.ClientID %>').is(':checked')) {
                        penjamin = '1';
                    }
                    if ($('#<%:chkBPJSAccidentPayer2.ClientID %>').is(':checked')) {
                        if (penjamin != '') {
                            penjamin = penjamin + ',' + '2';
                        }
                        else {
                            penjamin = '2';
                        }
                    }
                    if ($('#<%:chkBPJSAccidentPayer3.ClientID %>').is(':checked')) {
                        if (penjamin != '') {
                            penjamin = penjamin + ',' + '3';
                        }
                        else {
                            penjamin = '3';
                        }
                    }
                    if ($('#<%:chkBPJSAccidentPayer4.ClientID %>').is(':checked')) {
                        if (penjamin != '') {
                            penjamin = penjamin + ',' + '4';
                        }
                        else {
                            penjamin = '4';
                        }
                    }
                }
                var asalRujukan = $('#<%=hdnAsalRujukan.ClientID %>').val();
                var mobilePhoneNo = $('#<%=txtMobilePhoneNo1.ClientID %>').val();

                BPJSService.updateNoSEP(noSep, noKartu, dateSEP, dateRujukan, noRujukan, ppkRujukan, jnsPelayanan, catatan, diagnosa, poliTujuan, klsRawat, lakaLantas, lokasiLaka, medicalNo, asalRujukan, cob, poliEksekutif, mobilePhoneNo, penjamin,
                        katarak, suplesi, noSepSuplesi, kodePropinsi, kodeKabupaten, kodeKecamatan, noSKDP, kodeDPJP, function (result) {
                            try {
                                var obj = result.split('|');
                                if (obj[0] == "1") {
                                    $('#<%=txtNoSEP.ClientID %>').val(obj[1]);
                                    $('#<%=txtNoSEP.ClientID %>').attr('readonly', 'readonly');
                                    cbpPopupProcess.PerformCallback('save');
                                    showToast('Update SEP : SUKSES', 'No. SEP : ' + $('#<%=txtNoSEP.ClientID %>').val());
                                }
                                else {
                                    showToast('Update SEP : GAGAL', 'Error Message : ' + obj[2]);
                                }

                            } catch (err) {
                                showToast('Update SEP : GAGAL', err);
                                $('#<%=txtNoSEP.ClientID %>').val('');
                            }
                        });
            } else {
                var date = Methods.getDatePickerDate($('#<%=txtTglSEP.ClientID %>').val());
                var tglSEP = Methods.dateToYMD(date);
                var noSEP = $('#<%=txtNoSEP.ClientID %>').val();
                var hakKelas = $('#<%=txtKelas.ClientID %>').val();
                var hakKelasSplit = hakKelas.split(' ');
                var klsRawatHak = hakKelasSplit[0];
                var klsRawatNaik = "";
                var pembiayaan = "";
                var penanggungJawab = "";
                var medicalNo = $('#<%=txtMRN.ClientID %>').val();
                var catatan = $('#<%=txtNotes.ClientID %>').val();
                var diagnosa = $('#<%=hdnBPJSDiagnoseCodeCtl.ClientID %>').val();

                var poliTujuan = '';
                var poli = $('#<%=txtKodePoli.ClientID %>').val();
                var subSpesialis = $('#<%=txtKodeSubSpesialis.ClientID %>').val();
                if (subSpesialis == '') {
                    poliTujuan = $('#<%=txtKodePoli.ClientID %>').val();
                } else {
                    poliTujuan = $('#<%=txtKodeSubSpesialis.ClientID %>').val();
                }

                var poliEksekutif = $('#<%:hdnIsPoliExecutive.ClientID %>').val();
                var cob = "0";
                if ($('#<%:chkIsCOB.ClientID %>').is(":checked")) {
                    cob = "1";
                }

                var katarak = "0";
                if ($('#<%:chkIsCataract.ClientID %>').is(':checked')) {
                    katarak = '1';
                }

                var lakaLantas = "0";
                var suplesi = "0"
                var keterangan = "";

                if ($('#<%:chkIsSuplesi.ClientID %>').is(':checked')) {
                    suplesi = "1";
                }

                var noSepSuplesi = $('#<%=txtNoSEPSuplesi.ClientID %>').val();
                var kodeKecamatan = $('#<%:hdnKodeKecamatanBPJS.ClientID %>').val();
                var kodeKabupaten = $('#<%:hdnKodeKabupatenBPJS.ClientID %>').val();
                var kodePropinsi = $('#<%:hdnKodePropinsiBPJS.ClientID %>').val();

                if ($('#<%:chkIsAccident.ClientID %>').is(":checked")) {
                    lakaLantas = "1";
                    lokasiLaka = $('#<%=txtAccidentLocationCtl.ClientID %>').val();
                    if ($('#<%:chkBPJSAccidentPayer1.ClientID %>').is(':checked')) {
                        penjamin = '1';
                    }
                    if ($('#<%:chkBPJSAccidentPayer2.ClientID %>').is(':checked')) {
                        if (penjamin != '') {
                            penjamin = penjamin + ',' + '2';
                        }
                        else {
                            penjamin = '2';
                        }
                    }
                    if ($('#<%:chkBPJSAccidentPayer3.ClientID %>').is(':checked')) {
                        if (penjamin != '') {
                            penjamin = penjamin + ',' + '3';
                        }
                        else {
                            penjamin = '3';
                        }
                    }
                    if ($('#<%:chkBPJSAccidentPayer4.ClientID %>').is(':checked')) {
                        if (penjamin != '') {
                            penjamin = penjamin + ',' + '4';
                        }
                        else {
                            penjamin = '4';
                        }
                    }
                }

                var kodeDokterDPJP = $('#<%:hdnKodeDPJP.ClientID %>').val();
                var kodeDPJPPerujuk = $('#<%:hdnKodePerujuk.ClientID %>').val();
                if (kodeDPJPPerujuk != "" && kodeDPJPPerujuk != null) {
                    kodeDPJP = kodeDPJPPerujuk;
                } else {
                    kodeDPJP = kodeDokterDPJP;
                }

                dpjpLayan = kodeDPJP;

                var noTelp = $('#<%=txtMobilePhoneNo1.ClientID %>').val();

                BPJSService.updateNoSEPMedinfrasAPI(tglSEP, noSEP, klsRawatHak, klsRawatNaik, pembiayaan, penanggungJawab,
                            medicalNo, catatan, diagnosa, poliTujuan, poliEksekutif, cob, katarak, lakaLantas, keterangan, suplesi,
                            noSepSuplesi, kodePropinsi, kodeKabupaten, kodeKecamatan, dpjpLayan, noTelp, function (result) {
                                try {
                                    var obj = result.split('|');
                                    if (obj[0] == "1") {
                                        $('#<%=txtNoSEP.ClientID %>').val(obj[1]);
                                        $('#<%=txtNoSEP.ClientID %>').attr('readonly', 'readonly');
                                        cbpPopupProcess.PerformCallback('save');
                                        showToast('Update SEP : SUKSES', 'No. SEP : ' + $('#<%=txtNoSEP.ClientID %>').val());
                                    }
                                    else {
                                        showToast('Update SEP : GAGAL', 'Error Message : ' + obj[2]);
                                    }

                                } catch (err) {
                                    showToast('Update SEP : GAGAL', err);
                                    $('#<%=txtNoSEP.ClientID %>').val('');
                                }
                            });
            }
        }
    });
    //#endregion

    //#region Mapping SEP
    $('#btnMPMappingSEPCtl').click(function () {
        if ($('#<%=txtNoSEP.ClientID %>').val() != '') {
            var noSep = $('#<%=txtNoSEP.ClientID %>').val();
            var noTrans = $('#<%=hdnRegistrationNo.ClientID %>').val();
            BPJSService.mappingBPJS(noSep, noTrans, function (result) {
                try {
                    var obj = jQuery.parseJSON(result);
                    if (obj.metadata.code == "200" && obj.metadata.message == "OK") {
                        showToast('Sukses', 'Mapping SEP dan Registrasi Berhasil di lakukan');
                    }
                    else {
                        showToast('Mapping Gagal', 'Error Message : ' + obj.metadata.message);
                    }
                } catch (err) {
                    alert(err);
                }
            });
        }
    });
    //#endregion

    //#region Delete SEP
    $('#btnMPDeleteSEPCtl').click(function () {
        var message = 'Apakah Anda yakin akan menghapus SEP ' + '<b>' + $('#<%:txtNoSEP.ClientID %>').val() + '</b>' + ' ini ?';
        displayConfirmationMessageBox('DELETE SEP', message, function (result2) {
            if (result2) {
                if ($('#<%=txtNoSEP.ClientID %>').val() != '') {
                    var filterExpression = "RegistrationID = '" + $('#<%=hdnID.ClientID %>').val() + "'";
                    Methods.getObject('GetRegistrationBPJSList', filterExpression, function (result) {
                        if (result != null) {
                            var noSepTemp = result.NoSEP;
                            var noSep = $('#<%=txtNoSEP.ClientID %>').val();
                            if (noSepTemp == noSep) {
                                if ($('#<%=hdnIsBridgingBPJSVClaimVersion.ClientID %>').val() == Constant.VersionBridgingBPJSVClaim.v1_0) {
                                    BPJSService.deleteNoSEP(noSep, function (result) {
                                        try {
                                            var obj = result.split('|');
                                            if (obj[0] == "1") {
                                                showToast('Success', 'Delete No SEP Berhasil dilakukan');
                                                $('#<%=txtNoSEP.ClientID %>').val('');
                                                $('#<%=txtNoSEP.ClientID %>').removeAttr('readonly');
                                                cbpPopupProcess.PerformCallback('delete');
                                            }
                                            else {
                                                showToast('Delete SEP : GAGAL', 'Error Message : ' + obj[2]);
                                            }

                                        } catch (err) {
                                            alert(err);
                                        }
                                    });
                                } else {
                                    BPJSService.deleteNoSEPMedinfrasAPI(noSep, function (result) {
                                        try {
                                            var obj = result.split('|');
                                            if (obj[0] == "1") {
                                                showToast('Success', 'Delete No SEP Berhasil dilakukan');
                                                $('#<%=txtNoSEP.ClientID %>').val('');
                                                $('#<%=txtNoSEP.ClientID %>').removeAttr('readonly');
                                                cbpPopupProcess.PerformCallback('delete');
                                            }
                                            else {
                                                showToast('Delete SEP : GAGAL', 'Error Message : ' + obj[2]);
                                            }

                                        } catch (err) {
                                            alert(err);
                                        }
                                    });
                                }
                            }
                            else {
                                showToast('Warning', 'Anda tidak bisa mengapus SEP pasien registrasi lain.');
                            }
                        }
                        else {
                            showToast('Warning', 'Anda tidak bisa mengapus SEP pasien registrasi lain.');
                        }
                    });
                }
            }
        });
    });
    //#endregion

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
    //#endregion

    //#region SPRI
    $("#btnInsertSPRI").on("click", function (e) {
        e.preventDefault();
        var noKartu = $('#<%=txtNoPeserta.ClientID %>').val();

        var kodeDPJP = "";
        var kodeDokterDPJP = $('#<%:hdnKodeDPJP.ClientID %>').val();
        var kodeDPJPPerujuk = $('#<%:hdnKodePerujuk.ClientID %>').val();
        if (kodeDPJPPerujuk != "" && kodeDPJPPerujuk != null) {
            kodeDPJP = kodeDPJPPerujuk;
        } else {
            kodeDPJP = kodeDokterDPJP;
        }

        var kodeDokter = kodeDPJP;

        var poliTujuan = '';
        var poli = $('#<%=txtKodePoli.ClientID %>').val();
        var subSpesialis = $('#<%=txtKodeSubSpesialis.ClientID %>').val();
        if (subSpesialis == '') {
            poliTujuan = $('#<%=txtKodePoli.ClientID %>').val();
        } else {
            poliTujuan = $('#<%=txtKodeSubSpesialis.ClientID %>').val();
        }
        var date = Methods.getDatePickerDate($('#<%=txtTglRencanaKontrol.ClientID %>').val());
        var tglRencanaKontrol = Methods.dateToYMD(date);

        BPJSService.insertNoSPRIMedinfrasAPI(noKartu, kodeDokter, poliTujuan, tglRencanaKontrol, function (result) {
            try {
                var obj = result.split('|');
                if (obj[0] == "1") {
                    var resultObj = obj[1].split('^');
                    $('#<%=txtNoSPRI.ClientID %>').val(resultObj[0]);
                    cbpPopupProcess.PerformCallback('save');
                    showToast('Pembuatan SPRI : SUKSES', 'No. SPRI : ' + $('#<%=txtNoSPRI.ClientID %>').val());
                }
                else {
                    showToast('Pembuatan SPRI : GAGAL', 'Error Message : ' + obj[2]);
                }
            } catch (err) {
                alert(err);
                $('#<%=txtNoSPRI.ClientID %>').val('');
            }
        });
    });

    $("#btnUpdateSPRI").on("click", function (e) {
        e.preventDefault();
        var noSPRI = $('#<%=txtNoSPRI.ClientID %>').val();

        var kodeDPJP = "";
        var kodeDokterDPJP = $('#<%:hdnKodeDPJP.ClientID %>').val();
        var kodeDPJPPerujuk = $('#<%:hdnKodePerujuk.ClientID %>').val();
        if (kodeDPJPPerujuk != "" && kodeDPJPPerujuk != null) {
            kodeDPJP = kodeDPJPPerujuk;
        } else {
            kodeDPJP = kodeDokterDPJP;
        }

        var kodeDokter = kodeDPJP;

        var poliTujuan = '';
        var poli = $('#<%=txtKodePoli.ClientID %>').val();
        var subSpesialis = $('#<%=txtKodeSubSpesialis.ClientID %>').val();
        if (subSpesialis == '') {
            poliTujuan = $('#<%=txtKodePoli.ClientID %>').val();
        } else {
            poliTujuan = $('#<%=txtKodeSubSpesialis.ClientID %>').val();
        }
        var date = Methods.getDatePickerDate($('#<%=txtTglRencanaKontrol.ClientID %>').val());
        var tglRencanaKontrol = Methods.dateToYMD(date);

        BPJSService.updateNoSPRIMedinfrasAPI(noSPRI, kodeDokter, poliTujuan, tglRencanaKontrol, function (result) {
            try {
                var obj = result.split('|');
                if (obj[0] == "1") {
                    var resultObj = obj[1].split('^');
                    $('#<%=txtNoSPRI.ClientID %>').val(resultObj[0]);
                    cbpPopupProcess.PerformCallback('save');
                    showToast('Update SPRI : SUKSES', 'No. SPRI : ' + $('#<%=txtNoSPRI.ClientID %>').val());
                }
                else {
                    showToast('Update SPRI : GAGAL', 'Error Message : ' + obj[2]);
                }
            } catch (err) {
                alert(err);
                $('#<%=txtNoSPRI.ClientID %>').val('');
            }
        });
    });

    $("#btnDeleteSPRI").on("click", function (e) {
        e.preventDefault();
        var noSPRI = $('#<%=txtNoSPRI.ClientID %>').val();

        BPJSService.deleteRencanaKontrolMedinfrasAPI(noSPRI, function (result) {
            try {
                var obj = result.split('|');
                if (obj[0] == "1") {
                    var resultObj = obj[1].split('^');
                    $('#<%=txtNoSPRI.ClientID %>').val('');
                    cbpPopupProcess.PerformCallback('save');
                    showToast('Hapus SPRI : SUKSES');
                } else {
                    showToast('Hapus SPRI : GAGAL', 'Error Message : ' + obj[2]);
                }
            } catch (err) {
                alert(err);
                $('#<%=txtNoSPRI.ClientID %>').val('');
            }
        });
    });
    //#endregion

    $("#btnBackToSEPContent").on("click", function (e) {
        e.preventDefault();
        $("#divSEPContent").show();
        $("#divGetRencanaKontrol").hide();
        $('#btnMPEntryPopupSave').css('display', '');
        $('#btnMPProposeSEPCtl').css('display', '');
        $('#btnMPProposeFPCtl').css('display', '');
        $('#btnMPGenerateSEPCtl').css('display', '');
        $('#btnBackToSEPContent').css('display', 'none');
        $('#btnGetRencanaKontrol').css('display', '');
    });

    $("#btnSearchRencanaKontrol").on("click", function (e) {
        e.preventDefault();
        var rb = document.getElementById("<%=rbDateFilter.ClientID%>");
        var radio = rb.getElementsByTagName("input");
        for (var i = 0; i < radio.length; i++) {
            if (radio[i].checked) {
                cbpView.PerformCallback("search|" + cboBulanSuratKontrol.GetValue() + "|" + cboTahunSuratKontrol.GetValue() + "|" + radio[i].value);
                break;
            }
        }
    });

    $('#<%=txtNoSPRI.ClientID %>').live('change', function () {
        reinitMPButton();
    });

    function onCbpViewEndCallback(s) {
        hideLoadingPanel();
    }

    //#region Insert Rencana Kontrol
    $("#btnInsertRencanaKontrol").on("click", function (e) {
        e.preventDefault();
        var noSEP = $('#<%=txtNoSEP.ClientID %>').val();

        var kodeDPJP = "";
        var kodeDokterDPJP = $('#<%:hdnKodeDPJP.ClientID %>').val();
        var kodeDPJPPerujuk = $('#<%:hdnKodePerujuk.ClientID %>').val();
        if (kodeDPJPPerujuk != "" && kodeDPJPPerujuk != null) {
            kodeDPJP = kodeDPJPPerujuk;
        } else {
            kodeDPJP = kodeDokterDPJP;
        }

        var kodeDokter = kodeDPJP;

        var poliTujuan = '';
        var poli = $('#<%=txtKodePoli.ClientID %>').val();
        var subSpesialis = $('#<%=txtKodeSubSpesialis.ClientID %>').val();
        if (subSpesialis == '') {
            poliTujuan = $('#<%=txtKodePoli.ClientID %>').val();
        } else {
            poliTujuan = $('#<%=txtKodeSubSpesialis.ClientID %>').val();
        }
        var date = Methods.getDatePickerDate($('#<%=txtTglRencanaKontrol.ClientID %>').val());
        var tglRencanaKontrol = Methods.dateToYMD(date);

        BPJSService.insertRencanaKontrolMedinfrasAPI(noSEP, kodeDokter, poliTujuan, tglRencanaKontrol, function (result) {
            try {
                var obj = result.split('|');
                if (obj[0] == "1") {
                    var resultObj = obj[1].split('^');
                    $('#<%=txtNoRencanaKontrolBerikutnya.ClientID %>').val(resultObj[0]);
                    cbpPopupProcess.PerformCallback('save');
                    showToast('Pembuatan Rencana Kontrol : SUKSES', 'No. Surat Kontrol : ' + $('#<%=txtNoRencanaKontrolBerikutnya.ClientID %>').val());
                }
                else {
                    showToast('Pembuatan Rencana Kontrol : GAGAL', 'Error Message : ' + obj[2]);
                }
            } catch (err) {
                alert(err);
                $('#<%=txtNoRencanaKontrolBerikutnya.ClientID %>').val('');
            }
        });
    });

    $("#btnUpdateRencanaKontrol").on("click", function (e) {
        e.preventDefault();
        var noSuratKontrol = $('#<%=txtNoRencanaKontrolBerikutnya.ClientID %>').val();

        var noSEP = $('#<%=txtNoSEP.ClientID %>').val();

        var kodeDPJP = "";
        var kodeDokterDPJP = $('#<%:hdnKodeDPJP.ClientID %>').val();
        var kodeDPJPPerujuk = $('#<%:hdnKodePerujuk.ClientID %>').val();
        if (kodeDPJPPerujuk != "" && kodeDPJPPerujuk != null) {
            kodeDPJP = kodeDPJPPerujuk;
        } else {
            kodeDPJP = kodeDokterDPJP;
        }

        var kodeDokter = kodeDPJP;

        var poliTujuan = '';
        var poli = $('#<%=txtKodePoli.ClientID %>').val();
        var subSpesialis = $('#<%=txtKodeSubSpesialis.ClientID %>').val();
        if (subSpesialis == '') {
            poliTujuan = $('#<%=txtKodePoli.ClientID %>').val();
        } else {
            poliTujuan = $('#<%=txtKodeSubSpesialis.ClientID %>').val();
        }
        var date = Methods.getDatePickerDate($('#<%=txtTglRencanaKontrol.ClientID %>').val());
        var tglRencanaKontrol = Methods.dateToYMD(date);

        BPJSService.updateRencanaKontrolMedinfrasAPI(noSuratKontrol, noSEP, kodeDokter, poliTujuan, tglRencanaKontrol, function (result) {
            try {
                var obj = result.split('|');
                if (obj[0] == "1") {
                    var resultObj = obj[1].split('^');
                    $('#<%=txtNoRencanaKontrolBerikutnya.ClientID %>').val(resultObj[0]);
                    cbpPopupProcess.PerformCallback('save');
                    showToast('Update Rencana Kontrol : SUKSES', 'No. Surat Kontrol : ' + $('#<%=txtNoRencanaKontrolBerikutnya.ClientID %>').val());
                }
                else {
                    showToast('Update Rencana Kontrol : GAGAL', 'Error Message : ' + obj[2]);
                }
            } catch (err) {
                alert(err);
                $('#<%=txtNoRencanaKontrolBerikutnya.ClientID %>').val('');
            }
        });
    });

    $("#btnDeleteRencanaKontrol").on("click", function (e) {
        e.preventDefault();
        var noSuratKontrol = $('#<%=txtNoRencanaKontrolBerikutnya.ClientID %>').val();

        BPJSService.deleteRencanaKontrolMedinfrasAPI(noSuratKontrol, function (result) {
            try {
                var obj = result.split('|');
                if (obj[0] == "1") {
                    var resultObj = obj[1].split('^');
                    $('#<%=txtNoRencanaKontrolBerikutnya.ClientID %>').val('');
                    cbpPopupProcess.PerformCallback('save');
                    showToast('Hapus Rencana Kontrol : SUKSES');
                } else {
                    showToast('Hapus Rencana Kontrol : GAGAL', 'Error Message : ' + obj[2]);
                }
            } catch (err) {
                alert(err);
                $('#<%=txtNoRencanaKontrolBerikutnya.ClientID %>').val('');
            }
        });
    });
    //#endregion

    //#region Insert Rujukan
    $("#btnInsertRujukan").on("click", function (e) {
        e.preventDefault();
        var noSEP = $('#<%=txtNoSEP.ClientID %>').val();
        var tgl1 = Methods.getDatePickerDate($('#<%=txtTglRujukan.ClientID %>').val());
        var tglRujukan = Methods.dateToYMD(tgl1);
        var tgl2 = Methods.getDatePickerDate($('#<%=txtTglRencanaKunjungan.ClientID %>').val());
        var tglRencanaKunjungan = Methods.dateToYMD(tgl2);
        var ppkRujukan = $('#<%=txtKodePPKDirujuk.ClientID %>').val();
        var jnsPelayanan = cboJenisPelayanan.GetValue();
        var catatan = $('#<%=txtCatatanRujukan.ClientID %>').val();
        var diagRujukan = $('#<%=hdnBPJSKodeDiagnosaRujukan.ClientID %>').val();
        var tipeRujukan = cboTipeRujukan.GetValue();
        var poliRujukan = $('#<%=txtKodePoliRujukan.ClientID %>').val();

        if ($('#<%=txtNoSEP.ClientID %>').val() == '')
            showToast('Warning', "Nomor belum dibuat!");
        else {
            BPJSService.insertRujukanMedinfrasAPI(noSEP, tglRujukan, tglRencanaKunjungan, ppkRujukan, jnsPelayanan, catatan, diagRujukan, tipeRujukan, poliRujukan, function (result) {
                try {
                    var obj = result.split('|');
                    if (obj[0] == "1") {
                        var resultObj = obj[1].split('^');
                        $('#<%=txtNoRujukan2.ClientID %>').val(resultObj[0]);
                        cbpPopupProcess.PerformCallback('save');
                        showToast('Pembuatan Rujukan SUKSES', 'No. Rujukan : ' + $('#<%=txtNoRujukan2.ClientID %>').val());
                    }
                    else {
                        showToast('Pembuatan Rujukan : GAGAL', 'Error Message : ' + obj[2]);
                    }
                } catch (err) {
                    alert(err);
                }
            });
        }
    });


    //#endregion

    //#region Update Rujukan
    $("#btnUpdateRujukan").on("click", function (e) {
        e.preventDefault();
        var noRujukan = $('#<%=txtNoRujukan2.ClientID %>').val();
        var tgl1 = Methods.getDatePickerDate($('#<%=txtTglRujukan.ClientID %>').val());
        var tglRujukan = Methods.dateToYMD(tgl1);
        var tgl2 = Methods.getDatePickerDate($('#<%=txtTglRencanaKunjungan.ClientID %>').val());
        var tglRencanaKunjungan = Methods.dateToYMD(tgl2);
        var ppkRujukan = $('#<%=txtKodePPKDirujuk.ClientID %>').val();
        var jnsPelayanan = cboJenisPelayanan.GetValue();
        var catatan = $('#<%=txtCatatanRujukan.ClientID %>').val();
        var diagRujukan = $('#<%=hdnBPJSKodeDiagnosaRujukan.ClientID %>').val();
        var tipeRujukan = cboTipeRujukan.GetValue();
        var poliRujukan = $('#<%=txtKodePoliRujukan.ClientID %>').val();

        if ($('#<%=txtNoSEP.ClientID %>').val() == '')
            showToast('Warning', "Nomor belum dibuat!");
        else {
            BPJSService.updateRujukanMedinfrasAPI(noRujukan, tglRujukan, tglRencanaKunjungan, ppkRujukan, jnsPelayanan, catatan, diagRujukan, tipeRujukan, poliRujukan, function (result) {
                try {
                    var obj = result.split('|');
                    if (obj[0] == "1") {
                        var resultObj = obj[1].split('^');
                        $('#<%=txtNoRujukan2.ClientID %>').val(resultObj[0]);
                        cbpPopupProcess.PerformCallback('save');
                        showToast('Update Rujukan SUKSES', 'No. Rujukan : ' + $('#<%=txtNoRujukan2.ClientID %>').val());
                    }
                    else {
                        showToast('Update Rujukan : GAGAL', 'Error Message : ' + obj[2]);
                    }
                } catch (err) {
                    alert(err);
                }
            });
        }
    });


    //#endregion

    //#region Finger Print
    $("#btnCekFingerPrint").on("click", function (e) {
        e.preventDefault();
        var noPeserta = $('#<%=txtNoPeserta.ClientID %>').val();
        var tgl1 = Methods.getDatePickerDate($('#<%=txtTglSEP.ClientID %>').val());
        var tglPelayanan = Methods.dateToYMD(tgl1);

        BPJSService.getFingerPrintMedinfrasAPI(noPeserta, tglPelayanan, function (result) {
            try {
                var obj = result.split('|');
                if (obj[0] == "1") {
                    showToast('Finger Print SUKSES', obj[1]);
                }
                else {
                    showToast('Finger Print : GAGAL', 'Error Message : ' + obj[2]);
                }
            } catch (err) {
                alert(err);
            }
        });
    });
    //#endregion

    $('#btnMPEntryPopupPrintSEP').click(function () {
        var isBpjs = $('#<%=hdnIsBpjsRegistrationCtl.ClientID %>').val();
        if (isBpjs == "1") {
            cbpPopupProcess.PerformCallback('update');
        }
        else {
            showToast('Warning', 'Maaf Pasien Ini Belum Melakukan Generate SEP');
        }
    });

    $('#btnMPEntryPopupPrintSuratRujukan').click(function () {
        var isBpjs = $('#<%=hdnIsBpjsRegistrationCtl.ClientID %>').val();
        if (isBpjs == "1") {
            cbpPopupProcess.PerformCallback('printrujukan');
        }
        else {
            showToast('Warning', 'Maaf Pasien Ini Belum Melakukan Generate SEP');
        }
    });

    $('#btnMPEntryPopupAppointmentSuratRujukan').click(function () {
        var surKon = $('#<%=txtNoRencanaKontrolBerikutnya.ClientID %>').val();
        var appointmentID = $('#<%=hdnAppointmentID.ClientID %>').val();
        if (surKon != "") {
            if ($('#<%=hdnHealthcareInitial.ClientID %>').val() == "RSPW") {
                openReportViewer('PM-90044', $('#<%=hdnID.ClientID %>').val());
            }
            else if ($('#<%=hdnHealthcareInitial.ClientID %>').val() == "RSBL" || $('#<%=hdnHealthcareInitial.ClientID %>').val() == "RSSA") {
                if (appointmentID == "") {
                    cbpPopupProcess.PerformCallback('appointment');
                }
                else {
                    showToast('Warning', 'Maaf Pasien Ini Sudah Ada Perjanjian Pasien');
                }
            }
            else { //default all RS
                openReportViewer('PM-90058', $('#<%=hdnID.ClientID %>').val());
            }
        }
        else {
            showToast('Warning', 'Maaf Pasien Ini Belum Dibuatkan Surat Kontrol');
        }
    });

    $('#btnMPEntryPopupPrintSPRI').click(function () {
        var spri = $('#<%=txtNoSPRI.ClientID %>').val();
        if (spri != "") {
            if ($('#<%=hdnHealthcareInitial.ClientID %>').val() == "RSPW") {
                openReportViewer('PM-90045', $('#<%=hdnID.ClientID %>').val());
            }
            else {
                openReportViewer('PM-90059', $('#<%=hdnID.ClientID %>').val());
            }
        }
        else { //default all RS
            showToast('Warning', 'Maaf Pasien Ini Belum Dibuatkan SPRI');
        }
    });

    function ValidateParameter() {
        var message = '';
        //        if ($('#<%=txtNoRujukan.ClientID %>').val() == '') {
        ////            message = message + 'Nomor rujukan harus diisi  \n';
        //        }
        if ($('#<%=txtDiagnoseCode.ClientID %>').val() == '') {
            message = message + 'Diagnosa awal harus diisi  \n';
        }
        if ($('#<%=txtMobilePhoneNo1.ClientID %>').val().length < 8) {
            message = message + 'Nomor telepon tidak valid (lebih kecil dari 8 karakter)';
        }
        return message;
    }

    function onCbpPopupProcesEndCallback(s) {
        hideLoadingPanel();
        var param = s.cpResult.split('|');
        if (param[0] == 'save' || param[0] == 'delete' || param[0] == 'newClaim') {
            reinitMPButton();
            if (param[1] == 'fail')
                showToast('Save Failed', 'Error Message : ' + param[2]);
        }
        else {
            if (param[0] != 'update' && param[0] != 'printrujukan' && param[0] != 'appointment') {
                pcRightPanelContent.Hide();
            }
            else if (param[0] == 'update') {
                if ($('#<%=hdnHealthcareInitial.ClientID %>').val() == "DEMO" || $('#<%=hdnHealthcareInitial.ClientID %>').val() == "RSSES") {
                    openReportViewer('PM-90042', $('#<%=hdnID.ClientID %>').val());
                }
                else if ($('#<%=hdnHealthcareInitial.ClientID %>').val() == "RSRAC" || $('#<%=hdnHealthcareInitial.ClientID %>').val() == "RSRA") {
                    openReportViewer('PM-00216', $('#<%=hdnID.ClientID %>').val());
                } else if ($('#<%=hdnHealthcareInitial.ClientID %>').val() == "rsdo-soba") {
                    openReportViewer('PM-90042', $('#<%=hdnID.ClientID %>').val());
                }else if ($('#<%=hdnHealthcareInitial.ClientID %>').val() == "PHS") {
                    openReportViewer('PM-90069', $('#<%=hdnID.ClientID %>').val());
                } else if ($('#<%=hdnHealthcareInitial.ClientID %>').val() == "RSAJ") {
                    openReportViewer('PM-90071', $('#<%=hdnID.ClientID %>').val());
                } else if ($('#<%=hdnHealthcareInitial.ClientID %>').val() == "RSRTH") {
                    openReportViewer('PM-90072', $('#<%=hdnID.ClientID %>').val());
                }
                else {
                    openReportViewer('PM-00128', $('#<%=hdnID.ClientID %>').val());
                }
            }
            else if (param[0] == 'printrujukan') {
                if ($('#<%=hdnHealthcareInitial.ClientID %>').val() == "RSSES" || $('#<%=hdnHealthcareInitial.ClientID %>').val() == "RSSY") {
                    openReportViewer('PM-00197', $('#<%=hdnID.ClientID %>').val());
                }
                else if ($('#<%=hdnHealthcareInitial.ClientID %>').val() == "RSRTH") {
                    openReportViewer('PM-90078', $('#<%=hdnID.ClientID %>').val());
                } 
                else {
                    alert("Maaf belum tersedia format cetakannya.");
                }
            }
            else if (param[0] == 'appointment') {
                if ($('#<%=hdnHealthcareInitial.ClientID %>').val() == "RSBL" || $('#<%=hdnHealthcareInitial.ClientID %>').val() == "RSSA") {
                    openReportViewer('PM-00200', $('#<%=hdnID.ClientID %>').val());
                } else {
                    alert("Maaf belum tersedia format cetakannya.");
                }
            }

            var code = $('#hdnRightPanelContentCode').val();
            if (code == '') {
                code = 'generateSEP';
            }

            onAfterSaveRightPanelContent(code, '', '');
        }
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
                                <%=GetLabel("No. Kartu")%></label>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtNoPeserta" runat="server" Width="99%" />
                        </td>
                        <td>
                            <input type="button" id="btnSearchPeserta" value='<%= GetLabel("Data Peserta")%>' />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("NIK")%></label>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtNIK" runat="server" Width="99%" />
                        </td>
                        <td>
                            <input type="button" id="btnSearchNIK" value='<%= GetLabel("Data Peserta")%>' />
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
                        <td>
                            <input type="button" id="btnSearchNoSEP" value='<%= GetLabel("Cari SEP")%>' />
                        </td>
                    </tr>
                    <tr style="display: none">
                        <td class="tdLabel">
                            <label>
                                <%=GetLabel("No. SEP RI")%></label>
                        </td>
                        <td colspan="3">
                            <asp:TextBox ID="txtNoSEPRI" runat="server" Width="100%">
                            </asp:TextBox>
                        </td>
                    </tr>
                </table>
            </td>
            <td valign="top">
                <table>
                    <colgroup>
                        <col width="115px" />
                        <col width="150px" />
                        <col width="100px" />
                        <col />
                    </colgroup>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Tanggal SEP")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtTglSEP" Width="120px" runat="server" CssClass="datepicker" />
                        </td>
                        <td>
                            <asp:TextBox ID="txtJamSEP" Width="100px" runat="server" CssClass="time" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Kelas SEP") %></label>
                        </td>
                        <td colspan="2">
                            <input type="hidden" id="hdnChargeClassSEP" runat="server" value="" />
                            <asp:TextBox ID="txtChargeClassSEP" ReadOnly="true" runat="server" Width="100%" />
                        </td>
                    </tr>
                    <tr>
                        <td style="padding-left: 5px">
                            <input type="button" id="btnCekFingerPrint" value='<%= GetLabel("Cek Finger Print")%>' />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <div id="divSEPContent" style="height: 445px; overflow-y: scroll;" top="auto">
        <table class="tblContentArea" width="100%">
            <colgroup>
                <col width="50%" />
                <col width="50%" />
            </colgroup>
            <tr>
                <td style="padding: 5px; vertical-align: top">
                    <h4 class="h4expanded">
                        <%=GetLabel("DATA PESERTA :")%></h4>
                    <div class="containerTblEntryContent">
                        <table class="tblEntryContent" style="width: 100%">
                            <colgroup>
                                <col width="115px" />
                                <col width="80px" />
                                <col width="90px" />
                                <col />
                            </colgroup>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblNormal">
                                        <%=GetLabel("Nama")%></label>
                                </td>
                                <td colspan="3">
                                    <asp:TextBox ID="txtNamaPeserta" runat="server" Width="99%" ReadOnly="true" />
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblNormal">
                                        <%=GetLabel("No. RM")%></label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtMRN" ReadOnly="true" runat="server" Width="100px" />
                                </td>
                                <td>
                                    <label class="lblNormal">
                                        <%=GetLabel("No. Telepon")%></label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtMobilePhoneNo1" runat="server" Width="100px" />
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblNormal">
                                        <%=GetLabel("Jenis Kelamin")%></label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtGender" ReadOnly="true" runat="server" Width="100px" />
                                </td>
                                <td class="tdLabel">
                                    <label class="lblNormal">
                                        <%=GetLabel("Tanggal Lahir")%></label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtDOB" Width="100px" runat="server" ReadOnly="true" CssClass="datepicker" />
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblNormal">
                                        <%=GetLabel("Jenis")%></label>
                                </td>
                                <td colspan="3">
                                    <asp:TextBox ID="txtJenisPeserta" ReadOnly="true" Width="99%" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblNormal">
                                        <%=GetLabel("Kelas") %></label>
                                </td>
                                <td colspan="3">
                                    <asp:TextBox ID="txtKelas" ReadOnly="true" runat="server" Width="99%" />
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblNormal">
                                        <%=GetLabel("Faskes/PPK 1")%></label>
                                </td>
                                <td colspan="3">
                                    <asp:TextBox ID="txtNamaFaskes" ReadOnly="true" runat="server" Width="99%" />
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblNormal">
                                        <%=GetLabel("Pelayanan") %></label>
                                </td>
                                <td colspan="3">
                                    <input type="hidden" id="hdnKdPelayanan" runat="server" value="" />
                                    <asp:TextBox ID="txtPelayanan" ReadOnly="true" runat="server" Width="99%" />
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblLink" id="lblPembiayaan">
                                        <%=GetLabel("Pembiayaan")%></label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtPembiayaanCode" runat="server" Width="100px" />
                                </td>
                                <td colspan="2">
                                    <asp:TextBox ID="txtPembiayaanName" ReadOnly="true" runat="server" Width="160px" />
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblLink" id="lblPenanggungJawab">
                                        <%=GetLabel("Penanggung Jawab")%></label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtPenanggungJawabCode" runat="server" Width="100px" />
                                </td>
                                <td colspan="2">
                                    <asp:TextBox ID="txtPenanggungJawabName" ReadOnly="true" runat="server" Width="160px" />
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    &nbsp;
                                </td>
                                <td>
                                    <input type="checkbox" id="chkIsAccident" runat="server" />
                                    <label>
                                        Kasus KLL</label>
                                </td>
                                <td>
                                    <input type="checkbox" id="chkIsCOB" runat="server" /><label>COB</label>
                                </td>
                                <td>
                                    <asp:CheckBox ID="chkIsCataract" Width="130px" runat="server" Text="Katarak" />
                                </td>
                            </tr>
                            <tr runat="server" style="display: none" id="trAccidentLocationCtl">
                                <td class="tdLabel">
                                    <label class="lblNormal">
                                        <%=GetLabel("Lokasi Kejadian") %></label>
                                </td>
                                <td colspan="3">
                                    <asp:TextBox ID="txtAccidentLocationCtl" runat="server" Width="99%" />
                                </td>
                            </tr>
                            <tr id="trAccidentLocation4" runat="server" style="display: none">
                                <td class="tdLabel">
                                    <label class="lblLink" id="lblAccidentProvince" runat="server">
                                        <%=GetLabel("Propinsi")%></label>
                                </td>
                                <td colspan="3">
                                    <table style="width: 100%" cellpadding="0" cellspacing="0">
                                        <colgroup>
                                            <col width="80px" />
                                            <col width="3px" />
                                            <col />
                                        </colgroup>
                                        <tr>
                                            <td>
                                                <input type="hidden" id="hdnGCState" value="" runat="server" />
                                                <input type="hidden" id="hdnKodePropinsiBPJS" value="" runat="server" />
                                                <asp:TextBox ID="txtKodePropinsi" Width="100%" runat="server" />
                                            </td>
                                            <td />
                                            <td>
                                                <asp:TextBox ID="txtNamaPropinsi" Width="100%" ReadOnly="true" runat="server" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr id="trAccidentLocation3" runat="server" style="display: none">
                                <td class="tdLabel">
                                    <label class="lblLink" id="lblAccidentCity" runat="server">
                                        <%=GetLabel("Kota/Kabupaten")%></label>
                                </td>
                                <td colspan="3">
                                    <table style="width: 100%" cellpadding="0" cellspacing="0">
                                        <colgroup>
                                            <col width="80px" />
                                            <col width="3px" />
                                            <col />
                                        </colgroup>
                                        <tr>
                                            <td>
                                                <input type="hidden" id="hdnKabupatenID" value="" runat="server" />
                                                <input type="hidden" id="hdnKodeKabupatenBPJS" value="" runat="server" />
                                                <asp:TextBox ID="txtKodeKabupaten" Width="100%" runat="server" />
                                            </td>
                                            <td />
                                            <td>
                                                <asp:TextBox ID="txtNamaKabupaten" Width="100%" ReadOnly="true" runat="server" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr id="trAccidentLocation2" runat="server" style="display: none">
                                <td class="tdLabel">
                                    <label class="lblLink" id="lblAccidentDistrict" runat="server">
                                        <%=GetLabel("Kecamatan")%></label>
                                </td>
                                <td colspan="3">
                                    <table style="width: 100%" cellpadding="0" cellspacing="0">
                                        <colgroup>
                                            <col width="80px" />
                                            <col width="3px" />
                                            <col />
                                        </colgroup>
                                        <tr>
                                            <td>
                                                <input type="hidden" id="hdnKecamatanID" value="" runat="server" />
                                                <input type="hidden" id="hdnKodeKecamatanBPJS" value="" runat="server" />
                                                <asp:TextBox ID="txtKodeKecamatan" Width="100%" runat="server" />
                                            </td>
                                            <td />
                                            <td>
                                                <asp:TextBox ID="txtNamaKecamatan" Width="100%" ReadOnly="true" runat="server" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr id="trSuplesi" runat="server" style="display: none">
                                <td class="tdLabel" style="vertical-align: top">
                                    <label class="lblNormal">
                                        <%=GetLabel("No. SEP Suplesi")%></label>
                                </td>
                                <td colspan="3">
                                    <table border="0" cellpadding="0" cellspacing="1">
                                        <tr>
                                            <td>
                                                <asp:TextBox ID="txtNoSEPSuplesi" runat="server" Width="175px" />
                                            </td>
                                            <td>
                                                <asp:CheckBox ID="chkIsSuplesi" runat="server" Style="margin-left: 5px" Text="Suplesi" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr id="trAccidentPayor1" runat="server">
                                <td class="tdLabel" style="vertical-align: top">
                                    <label class="lblNormal">
                                        <%=GetLabel("Penjamin KLL") %></label>
                                </td>
                                <td colspan="3">
                                    <table border="0" cellpadding="0" cellspacing="1">
                                        <tr>
                                            <td>
                                                <asp:CheckBox ID="chkBPJSAccidentPayer1" runat="server" Style="margin-left: 2px"
                                                    Text="Jasa Raharja" />
                                            </td>
                                            <td>
                                                <asp:CheckBox ID="chkBPJSAccidentPayer2" runat="server" Style="margin-left: 2px"
                                                    Text="BPJS" />
                                            </td>
                                            <td>
                                                <asp:CheckBox ID="chkBPJSAccidentPayer3" runat="server" Style="margin-left: 2px"
                                                    Text="TASPEN" />
                                            </td>
                                            <td>
                                                <asp:CheckBox ID="chkBPJSAccidentPayer4" runat="server" Style="margin-left: 2px"
                                                    Text="ASABRI" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </div>
                    <h4 class="h4expanded">
                        <%=GetLabel("PEMBUATAN RUJUKAN PASIEN :")%></h4>
                    <div class="containerTblEntryContent">
                        <table class="tblEntryContent" style="width: 100%">
                            <colgroup>
                                <col width="125px" />
                                <col width="100px" />
                                <col />
                            </colgroup>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblNormal">
                                        <%=GetLabel("No. Rujukan") %></label>
                                </td>
                                <td colspan="4">
                                    <asp:TextBox ID="txtNoRujukan2" runat="server" Width="100%" />
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblNormal">
                                        <%=GetLabel("Tgl. Kunjungan")%></label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtTglRencanaKunjungan" Width="80px" runat="server" CssClass="datepicker" />
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblLink" id="lblPPKDirujuk">
                                        <%=GetLabel("Dirujuk Ke")%></label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtKodePPKDirujuk" Width="100px" runat="server" />
                                </td>
                                <td colspan="2">
                                    <asp:TextBox ID="txtNamaPPKDirujuk" Width="100%" ReadOnly="true" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblNormal" id="Label2">
                                        <%=GetLabel("Jenis Pelayanan") %></label>
                                </td>
                                <td>
                                    <dxe:ASPxComboBox ID="cboJenisPelayanan" ClientInstanceName="cboJenisPelayanan" Width="100%"
                                        runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel" style="vertical-align: top;">
                                    <label class="lblNormal">
                                        <%=GetLabel("Catatan")%></label>
                                </td>
                                <td colspan="2">
                                    <asp:TextBox ID="txtCatatanRujukan" Width="100%" runat="server" TextMode="MultiLine" />
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblLink" id="lblDiagnosaRujukan">
                                        <%=GetLabel("Diagnosa Rujukan")%></label>
                                </td>
                                <td>
                                    <input type="hidden" id="hdnBPJSKodeDiagnosaRujukan" value="" runat="server" />
                                    <asp:TextBox ID="txtKodeDiagnosaRujukan" Width="100px" runat="server" />
                                </td>
                                <td colspan="2">
                                    <asp:TextBox ID="txtNamaDiagnosaRujukan" Width="100%" ReadOnly="true" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblNormal" id="Label3">
                                        <%=GetLabel("Tipe Rujukan") %></label>
                                </td>
                                <td>
                                    <dxe:ASPxComboBox ID="cboTipeRujukan" ClientInstanceName="cboTipeRujukan" Width="100%"
                                        runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblLink" id="lblPoliRujukan">
                                        <%=GetLabel("Poli Rujukan") %></label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtKodePoliRujukan" runat="server" Width="100px" />
                                </td>
                                <td colspan="2">
                                    <asp:TextBox ID="txtNamaPoliRujukan" ReadOnly="true" runat="server" Width="100%" />
                                </td>
                            </tr>
                            <tr>
                                <td style="padding-left: 5px">
                                    <input type="button" id="btnInsertRujukan" value='<%= GetLabel("Insert Rujukan")%>' />
                                </td>
                                <td style="padding-left: 5px">
                                    <input type="button" id="btnUpdateRujukan" value='<%= GetLabel("Update Rujukan")%>' />
                                </td>
                            </tr>
                        </table>
                    </div>
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
                <td style="padding: 5px; vertical-align: top">
                    <h4 class="h4expanded">
                        <%=GetLabel("DATA KUNJUNGAN :")%></h4>
                    <div class="containerTblEntryContent">
                        <table class="tblEntryContent" style="width: 100%">
                            <colgroup>
                                <col width="125px" />
                                <col width="100px" />
                                <col />
                            </colgroup>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblNormal">
                                        <%=GetLabel("Kelas Rawat") %></label>
                                </td>
                                <td colspan="4">
                                    <input type="hidden" id="hdnKdKelasRawat" runat="server" value="" />
                                    <input type="hidden" id="hdnChargeClassID" runat="server" value="" />
                                    <asp:TextBox ID="txtChargeClass" ReadOnly="true" runat="server" Width="100%" />
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblLink" id="lblReferralNo" runat="server">
                                        <%=GetLabel("No. Rujukan")%></label>
                                </td>
                                <td colspan="3">
                                    <table border="0" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td>
                                                <asp:TextBox ID="txtNoRujukan" runat="server" Width="150px">
                                                </asp:TextBox>
                                            </td>
                                            <td style="padding-left: 5px">
                                                <input type="button" id="btnSearchNoRujukan" value='<%= GetLabel("Data Rujukan")%>' />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label>
                                        <%=GetLabel("Asal Rujukan")%></label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtKdPpkRujukan" ReadOnly="true" runat="server" Width="100px" />
                                </td>
                                <td colspan="2">
                                    <asp:TextBox ID="txtPpkRujukan" ReadOnly="true" runat="server" Width="100%" />
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblNormal">
                                        <%=GetLabel("Tanggal Rujukan")%></label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtTglRujukan" Width="80px" runat="server" CssClass="datepicker" />
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblLink" id="lblDokterPerujuk">
                                        <%=GetLabel("Dokter Tujuan")%></label>
                                </td>
                                <td>
                                    <input type="hidden" id="hdnKodePerujuk" runat="server" value="" />
                                    <asp:TextBox ID="txtDPJPDokterPerujuk" runat="server" Width="100px" />
                                </td>
                                <td colspan="2">
                                    <asp:TextBox ID="txtDokterPerujuk" runat="server" Width="100%" />
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblLink" id="lblSpesialis">
                                        <%=GetLabel("Poli Tujuan") %></label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtKodePoli" runat="server" Width="100px" />
                                </td>
                                <td colspan="2">
                                    <asp:TextBox ID="txtPoliTujuan" ReadOnly="true" runat="server" Width="100%" />
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblLink" id="lblSubSpesialis">
                                        <%=GetLabel("Sub Spesialis") %></label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtKodeSubSpesialis" runat="server" Width="100px" />
                                </td>
                                <td colspan="2">
                                    <asp:TextBox ID="txtNamaSubSpesialis" ReadOnly="true" runat="server" Width="100%" />
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblLink" id="lblDiagnose">
                                        <%=GetLabel("Diagnosa")%></label>
                                </td>
                                <td>
                                    <input type="hidden" id="hdnBPJSDiagnoseCodeCtl" value="" runat="server" />
                                    <asp:TextBox ID="txtDiagnoseCode" Width="100px" runat="server" />
                                </td>
                                <td colspan="2">
                                    <asp:TextBox ID="txtDiagnoseName" Width="100%" ReadOnly="true" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel" style="vertical-align: top;">
                                    <label class="lblNormal">
                                        <%=GetLabel("Keluhan")%></label>
                                </td>
                                <td colspan="3">
                                    <asp:TextBox ID="txtKeluhan" Width="100%" runat="server" TextMode="MultiLine" />
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblNormal" id="Label1">
                                        <%=GetLabel("Tujuan Kunjungan") %></label>
                                </td>
                                <td>
                                    <dxe:ASPxComboBox ID="cboTujuanKunjungan" ClientInstanceName="cboTujuanKunjungan"
                                        Width="250%" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblNormal" id="Label4">
                                        <%=GetLabel("Prosedur") %></label>
                                </td>
                                <td>
                                    <dxe:ASPxComboBox ID="cboProsedur" ClientInstanceName="cboProsedur" Width="250%"
                                        runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblNormal" id="Label5">
                                        <%=GetLabel("Prosedur Penunjang") %></label>
                                </td>
                                <td>
                                    <dxe:ASPxComboBox ID="cboProsedurPenunjang" ClientInstanceName="cboProsedurPenunjang"
                                        Width="250%" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblNormal" id="Label6">
                                        <%=GetLabel("Asesmen Pelayanan")%></label>
                                </td>
                                <td>
                                    <dxe:ASPxComboBox ID="cboAsesmenPelayanan" ClientInstanceName="cboAsesmenPelayanan"
                                        Width="250%" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel" style="vertical-align: top;">
                                    <label class="lblNormal">
                                        <%=GetLabel("Catatan")%></label>
                                </td>
                                <td colspan="3">
                                    <asp:TextBox ID="txtNotes" Width="100%" runat="server" TextMode="MultiLine" />
                                </td>
                            </tr>
                        </table>
                    </div>
                    <h4 class="h4expanded">
                        <%=GetLabel("PENGANTAR RAWAT INAP & RENCANA KONTROL:")%></h4>
                    <div class="containerTblEntryContent">
                        <table>
                            <colgroup>
                                <col width="125px" />
                                <col width="100px" />
                                <col />
                            </colgroup>
                            <tr>
                                <td class="tdLabel">
                                    <label>
                                        <%=GetLabel("No. SPRI")%></label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtNoSPRI" runat="server" Width="170px" />
                                </td>
                                <td style="padding-left: 5px">
                                    <input type="button" id="btnInsertSPRI" value='<%= GetLabel("Insert")%>' />
                                </td>
                                <td style="padding-left: 5px">
                                    <input type="button" id="btnUpdateSPRI" value='<%= GetLabel("Update")%>' />
                                </td>
                                <td style="padding-left: 5px">
                                    <input type="button" id="btnDeleteSPRI" value='<%= GetLabel("Delete")%>' />
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblLink" id="lblRencanaKontrol">
                                        <%=GetLabel("No. Rencana Kontrol")%></label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtNoSuratKontrol" runat="server" Width="170px" />
                                </td>
                            </tr>
                            <tr id="trKontrolBerikutnya" runat="server">
                                <td class="tdLabel">
                                    <label class="lblNormal">
                                        <%=GetLabel("No. Rencana Kontrol Berikutnya")%></label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtNoRencanaKontrolBerikutnya" runat="server" Width="170px" />
                                </td>
                                <td style="padding-left: 5px">
                                    <input type="button" id="btnInsertRencanaKontrol" value='<%= GetLabel("Insert")%>' />
                                </td>
                                <td style="padding-left: 5px">
                                    <input type="button" id="btnUpdateRencanaKontrol" value='<%= GetLabel("Update")%>' />
                                </td>
                                <td style="padding-left: 5px">
                                    <input type="button" id="btnDeleteRencanaKontrol" value='<%= GetLabel("Delete")%>' />
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblNormal">
                                        <%=GetLabel("Tgl. Rencana Kontrol")%></label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtTglRencanaKontrol" Width="100px" runat="server" CssClass="datepicker" />
                                </td>
                            </tr>
                        </table>
                    </div>
                </td>
            </tr>
        </table>
    </div>
    <div id="divGetRencanaKontrol" style="height: 100%;">
        <table class="tblContentArea" width="100%">
            <colgroup>
                <col width="100%" />
            </colgroup>
            <tr>
                <td valign="top">
                    <table>
                        <colgroup>
                            <col width="115px" />
                            <col width="150px" />
                            <col width="115px" />
                            <col />
                        </colgroup>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Bulan")%></label>
                            </td>
                            <td colspan="2">
                                <dxe:ASPxComboBox ID="cboBulanSuratKontrol" ClientInstanceName="cboBulanSuratKontrol"
                                    Width="75%" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Tahun")%></label>
                            </td>
                            <td colspan="2">
                                <dxe:ASPxComboBox ID="cboTahunSuratKontrol" ClientInstanceName="cboTahunSuratKontrol"
                                    Width="75%" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label>
                                    <%=GetLabel("Filter Tanggal")%></label>
                            </td>
                            <td colspan="2">
                                <%--<asp:RadioButton ID="rbTglEntri" runat="server" Text="Tgl. Entri" GroupName="dateFilter" Checked=true />  
                                <asp:RadioButton ID="rbTglRencanaKontrol" runat="server" Text="Tgl. Rencana Kontrol" GroupName="dateFilter" />  --%>
                                <asp:RadioButtonList ID="rbDateFilter" runat="server">
                                    <asp:ListItem Text="Tgl. Entry" Value="1" Selected="True"></asp:ListItem>
                                    <asp:ListItem Text="Tgl. Rencana Kontrol" Value="2"></asp:ListItem>
                                </asp:RadioButtonList>
                            </td>
                        </tr>
                        <tr>
                            <td style="padding-left: 5px">
                                <input type="button" id="btnSearchRencanaKontrol" value='<%= GetLabel("Cari")%>' />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
        <div style="height: 100%; overflow-y: scroll">
            <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
                ShowLoadingPanel="false" OnCallback="cbpView_Callback">
                <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ onCbpViewEndCallback(s); }" />
                <PanelCollection>
                    <dx:PanelContent ID="PanelContent1" runat="server">
                        <asp:Panel runat="server" ID="pnlRencanaKontrol" Style="width: 100%; margin-left: auto;
                            margin-right: auto; position: relative; font-size: 0.95em;">
                            <asp:GridView ID="grdView" runat="server" CssClass="grdView grdSelected" AutoGenerateColumns="false">
                                <Columns>
                                    <asp:BoundField DataField="noSuratKontrol" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                    <asp:TemplateField ItemStyle-CssClass="gridColumnLink lnkSuratKontrol" HeaderText="No Surat Kontrol"
                                        HeaderStyle-Width="80px">
                                        <ItemTemplate>
                                            <a>
                                                <%# Eval("noSuratKontrol") %></a>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="jnsPelayanan" HeaderText="Jenis Pelayanan" HeaderStyle-Width="100px"
                                        HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                                    <asp:BoundField DataField="namaJnsKontrol" HeaderText="Jenis Kontrol" HeaderStyle-Width="100px"
                                        HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                                    <asp:BoundField DataField="tglRencanaKontrol" HeaderText="Tgl. Rencana Kontrol" HeaderStyle-Width="100px"
                                        HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                                    <asp:BoundField DataField="namaPoliTujuan" HeaderText="Poli Tujuan" HeaderStyle-Width="150px"
                                        HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                                    <asp:BoundField DataField="kodeDokter" HeaderText="Kode Dokter" HeaderStyle-Width="100px"
                                        HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                                    <asp:BoundField DataField="namaDokter" HeaderText="Nama Dokter" HeaderStyle-HorizontalAlign="Left"
                                        HeaderStyle-Width="150px" ItemStyle-HorizontalAlign="Left" />
                                </Columns>
                            </asp:GridView>
                            <div class="imgLoadingGrdView" id="containerImgLoadingReferrer">
                                <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                            </div>
                        </asp:Panel>
                    </dx:PanelContent>
                </PanelCollection>
            </dxcp:ASPxCallbackPanel>
        </div>
    </div>
    <dxcp:ASPxCallbackPanel ID="cbpPopupProcess" runat="server" Width="100%" ClientInstanceName="cbpPopupProcess"
        ShowLoadingPanel="false" OnCallback="cbpPopupProcess_Callback">
        <ClientSideEvents BeginCallback="function(s,e) { showLoadingPanel(); }" EndCallback="function(s,e) { onCbpPopupProcesEndCallback(s); }" />
    </dxcp:ASPxCallbackPanel>
</div>
