var KemenKesService = new (function () {
    this.getRekapPasienMasuk = function (functionHandler) {
        showLoadingPanel();
        var loc = ResolveUrl('~/Libs/Service/KemenKesService.asmx/GetRekapPasienMasuk');
        $.ajax({
            type: "POST",
            url: ResolveUrl('~/Libs/Service/KemenKesService.asmx/GetRekapPasienMasuk'),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (msg) {
                hideLoadingPanel();
                functionHandler(msg.d);
            },
            error: function (jqXHR, exception) {
                hideLoadingPanel();
                alert(jqXHR.responseText);
            },
            failure: function (msg) {
                hideLoadingPanel();
                alert(msg);
            }
        });
    };

    this.rekapPasienMasuk = function (tanggal, igd_suspect_l, igd_suspect_p, igd_confirm_l, igd_confirm_p, rj_suspect_l,
            rj_suspect_p, rj_confirm_l, rj_confirm_p, ri_suspect_l, ri_suspect_p, ri_confirm_l, ri_confirm_p, functionHandler) {
        $.ajax({
            type: "POST",
            beforeSend: function (msg) {
                showLoadingPanel();
            },
            url: ResolveUrl('~/Libs/Service/KemenKesService.asmx/PostRekapPasienMasuk'),
            data: "{ tanggal :'" + tanggal + "', igd_suspect_l:'" + igd_suspect_l + "', igd_suspect_p:'" + igd_suspect_p + "', igd_confirm_l:'"
                + igd_confirm_l + "', igd_confirm_p:'" + igd_confirm_p + "', rj_suspect_l:'" + rj_suspect_l + "', rj_suspect_p:'"
                + rj_suspect_p + "', rj_confirm_l:'" + rj_confirm_l + "', rj_confirm_p:'" + rj_confirm_p + "', ri_suspect_l:'" + ri_suspect_l
                + "', ri_suspect_p:'" + ri_suspect_p + "', ri_confirm_l:'" + ri_confirm_l + "', ri_confirm_p:'" + ri_confirm_p + "'}",

            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (msg) {
                hideLoadingPanel();
                functionHandler(msg.d);
            },
            error: function (jqXHR, exception) {
                hideLoadingPanel();
                alert(jqXHR.responseText);
            },
            failure: function (msg) {
                hideLoadingPanel();
                alert(msg);
            }
        });
    };

    this.deleteRekapPasienMasuk = function (tanggal, functionHandler) {
        $.ajax({
            type: "POST",
            beforeSend: function (msg) {
                showLoadingPanel();
            },
            url: ResolveUrl('~/Libs/Service/KemenKesService.asmx/DeleteRekapPasienMasuk'),
            data: "{ tanggal :'" + tanggal + "'}",

            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (msg) {
                hideLoadingPanel();
                functionHandler(msg.d);
            },
            error: function (jqXHR, exception) {
                hideLoadingPanel();
                alert(jqXHR.responseText);
            },
            failure: function (msg) {
                hideLoadingPanel();
                alert(msg);
            }
        });
    };

    this.getPasienDirawatdenganKomorbid = function (functionHandler) {
        showLoadingPanel();
        var loc = ResolveUrl('~/Libs/Service/KemenKesService.asmx/GetPasienDirawatdenganKomorbid');
        $.ajax({
            type: "POST",
            url: ResolveUrl('~/Libs/Service/KemenKesService.asmx/GetPasienDirawatdenganKomorbid'),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (msg) {
                hideLoadingPanel();
                functionHandler(msg.d);
            },
            error: function (jqXHR, exception) {
                hideLoadingPanel();
                alert(jqXHR.responseText);
            },
            failure: function (msg) {
                hideLoadingPanel();
                alert(msg);
            }
        });
    };

    this.pasienDirawatdenganKomorbid = function (tanggal, icu_dengan_ventilator_suspect_l, icu_dengan_ventilator_suspect_p,
                icu_dengan_ventilator_confirm_l, icu_dengan_ventilator_confirm_p, icu_tanpa_ventilator_suspect_l,
                icu_tanpa_ventilator_suspect_p, icu_tanpa_ventilator_confirm_l, icu_tanpa_ventilator_confirm_p,
                icu_tekanan_negatif_dengan_ventilator_suspect_l, icu_tekanan_negatif_dengan_ventilator_suspect_p,
                icu_tekanan_negatif_dengan_ventilator_confim_l, icu_tekanan_negatif_dengan_ventilator_confim_p,
                icu_tekanan_negatif_tanpa_ventilator_suspect_l, icu_tekanan_negatif_tanpa_ventilator_suspect_p,
                icu_tekanan_negatif_tanpa_ventilator_confirm_l, icu_tekanan_negatif_tanpa_ventilator_confirm_p,
                isolasi_tekanan_negatif_suspect_l, isolasi_tekanan_negatif_suspect_p, isolasi_tekanan_negatif_confirm_l, isolasi_tekanan_negatif_confirm_p,
                isolasi_tanpa_tekanan_negatif_suspect_l, isolasi_tanpa_tekanan_negatif_suspect_p, isolasi_tanpa_tekanan_negatif_confirm_l,
                isolasi_tanpa_tekanan_negatif_confirm_p, nicu_khusus_covid_suspect_l, nicu_khusus_covid_suspect_p, nicu_khusus_covid_confirm_l,
                nicu_khusus_covid_confirm_p, picu_khusus_covid_suspect_l, picu_khusus_covid_suspect_p, picu_khusus_covid_confirm_l,
                picu_khusus_covid_confirm_p, functionHandler) {
        $.ajax({
            type: "POST",
            beforeSend: function (msg) {
                showLoadingPanel();
            },
            url: ResolveUrl('~/Libs/Service/KemenKesService.asmx/PostPasienDirawatdenganKomorbid'),
            data: "{ tanggal :'"
                + tanggal + "', icu_dengan_ventilator_suspect_l:'" + icu_dengan_ventilator_suspect_l + "', icu_dengan_ventilator_suspect_p:'" + icu_dengan_ventilator_suspect_p
                + "', icu_dengan_ventilator_confirm_l:'" + icu_dengan_ventilator_confirm_l + "', icu_dengan_ventilator_confirm_p:'" + icu_dengan_ventilator_confirm_p
                + "', icu_tanpa_ventilator_suspect_l:'" + icu_tanpa_ventilator_suspect_l + "', icu_tanpa_ventilator_suspect_p:'" + icu_tanpa_ventilator_suspect_p
                + "', icu_tanpa_ventilator_confirm_l:'" + icu_tanpa_ventilator_confirm_l + "', icu_tanpa_ventilator_confirm_p:'" + icu_tanpa_ventilator_confirm_p
                + "', icu_tekanan_negatif_dengan_ventilator_suspect_l:'" + icu_tekanan_negatif_dengan_ventilator_suspect_l
                + "', icu_tekanan_negatif_dengan_ventilator_suspect_p:'" + icu_tekanan_negatif_dengan_ventilator_suspect_p
                + "', icu_tekanan_negatif_dengan_ventilator_confim_l:'" + icu_tekanan_negatif_dengan_ventilator_confim_l
                + "', icu_tekanan_negatif_dengan_ventilator_confim_p:'" + icu_tekanan_negatif_dengan_ventilator_confim_p
                + "', icu_tekanan_negatif_tanpa_ventilator_suspect_l:'" + icu_tekanan_negatif_tanpa_ventilator_suspect_l
                + "', icu_tekanan_negatif_tanpa_ventilator_suspect_p:'" + icu_tekanan_negatif_tanpa_ventilator_suspect_p
                + "', icu_tekanan_negatif_tanpa_ventilator_confirm_l:'" + icu_tekanan_negatif_tanpa_ventilator_confirm_l
                + "', icu_tekanan_negatif_tanpa_ventilator_confirm_p:'" + icu_tekanan_negatif_tanpa_ventilator_confirm_p
                + "', isolasi_tekanan_negatif_suspect_l:'" + isolasi_tekanan_negatif_suspect_l
                + "', isolasi_tekanan_negatif_suspect_p:'" + isolasi_tekanan_negatif_suspect_p
                + "', isolasi_tekanan_negatif_confirm_l:'" + isolasi_tekanan_negatif_confirm_l
                 + "',isolasi_tekanan_negatif_confirm_p:'" + isolasi_tekanan_negatif_confirm_p
                + "', isolasi_tanpa_tekanan_negatif_suspect_l:'" + isolasi_tanpa_tekanan_negatif_suspect_l
                + "', isolasi_tanpa_tekanan_negatif_suspect_p:'" + isolasi_tanpa_tekanan_negatif_suspect_p
                + "', isolasi_tanpa_tekanan_negatif_confirm_l:'" + isolasi_tanpa_tekanan_negatif_confirm_l
                + "', isolasi_tanpa_tekanan_negatif_confirm_p:'" + isolasi_tanpa_tekanan_negatif_confirm_p
                + "', nicu_khusus_covid_suspect_l:'" + nicu_khusus_covid_suspect_l
                + "', nicu_khusus_covid_suspect_p:'" + nicu_khusus_covid_suspect_p
                + "', nicu_khusus_covid_confirm_l:'" + nicu_khusus_covid_confirm_l
                + "', nicu_khusus_covid_confirm_p:'" + nicu_khusus_covid_confirm_p
                + "', picu_khusus_covid_suspect_l:'" + picu_khusus_covid_suspect_l
                + "', picu_khusus_covid_suspect_p:'" + picu_khusus_covid_suspect_p
                + "', picu_khusus_covid_confirm_l:'" + picu_khusus_covid_confirm_l
                + "', picu_khusus_covid_confirm_p:'" + picu_khusus_covid_confirm_p + "'}",

            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (msg) {
                hideLoadingPanel();
                functionHandler(msg.d);
            },
            error: function (jqXHR, exception) {
                hideLoadingPanel();
                alert(jqXHR.responseText);
            },
            failure: function (msg) {
                hideLoadingPanel();
                alert(msg);
            }
        });
    };

    this.getPasienDirawattanpaKomorbid = function (functionHandler) {
        showLoadingPanel();
        var loc = ResolveUrl('~/Libs/Service/KemenKesService.asmx/GetPasienDirawattanpaKomorbid');
        $.ajax({
            type: "POST",
            url: ResolveUrl('~/Libs/Service/KemenKesService.asmx/GetPasienDirawattanpaKomorbid'),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (msg) {
                hideLoadingPanel();
                functionHandler(msg.d);
            },
            error: function (jqXHR, exception) {
                hideLoadingPanel();
                alert(jqXHR.responseText);
            },
            failure: function (msg) {
                hideLoadingPanel();
                alert(msg);
            }
        });
    };

    this.pasienDirawattanpaKomorbid = function (tanggal, icu_dengan_ventilator_suspect_l, icu_dengan_ventilator_suspect_p,
                icu_dengan_ventilator_confirm_l, icu_dengan_ventilator_confirm_p, icu_tanpa_ventilator_suspect_l,
                icu_tanpa_ventilator_suspect_p, icu_tanpa_ventilator_confirm_l, icu_tanpa_ventilator_confirm_p,
                icu_tekanan_negatif_dengan_ventilator_suspect_l, icu_tekanan_negatif_dengan_ventilator_suspect_p,
                icu_tekanan_negatif_dengan_ventilator_confim_l, icu_tekanan_negatif_dengan_ventilator_confim_p,
                icu_tekanan_negatif_tanpa_ventilator_suspect_l, icu_tekanan_negatif_tanpa_ventilator_suspect_p,
                icu_tekanan_negatif_tanpa_ventilator_confirm_l, icu_tekanan_negatif_tanpa_ventilator_confirm_p,
                isolasi_tekanan_negatif_suspect_l, isolasi_tekanan_negatif_suspect_p, isolasi_tekanan_negatif_confirm_l,isolasi_tekanan_negatif_confirm_p,
                isolasi_tanpa_tekanan_negatif_suspect_l, isolasi_tanpa_tekanan_negatif_suspect_p, isolasi_tanpa_tekanan_negatif_confirm_l,
                isolasi_tanpa_tekanan_negatif_confirm_p, nicu_khusus_covid_suspect_l, nicu_khusus_covid_suspect_p, nicu_khusus_covid_confirm_l,
                nicu_khusus_covid_confirm_p, picu_khusus_covid_suspect_l, picu_khusus_covid_suspect_p, picu_khusus_covid_confirm_l,
                picu_khusus_covid_confirm_p, functionHandler) {
        $.ajax({
            type: "POST",
            beforeSend: function (msg) {
                showLoadingPanel();
            },
            url: ResolveUrl('~/Libs/Service/KemenKesService.asmx/PostPasienDirawattanpaKomorbid'),
            data: "{ tanggal :'"
                + tanggal + "', icu_dengan_ventilator_suspect_l:'" + icu_dengan_ventilator_suspect_l + "', icu_dengan_ventilator_suspect_p:'" + icu_dengan_ventilator_suspect_p
                + "', icu_dengan_ventilator_confirm_l:'" + icu_dengan_ventilator_confirm_l + "', icu_dengan_ventilator_confirm_p:'" + icu_dengan_ventilator_confirm_p
                + "', icu_tanpa_ventilator_suspect_l:'" + icu_tanpa_ventilator_suspect_l + "', icu_tanpa_ventilator_suspect_p:'" + icu_tanpa_ventilator_suspect_p
                + "', icu_tanpa_ventilator_confirm_l:'" + icu_tanpa_ventilator_confirm_l + "', icu_tanpa_ventilator_confirm_p:'" + icu_tanpa_ventilator_confirm_p
                + "', icu_tekanan_negatif_dengan_ventilator_suspect_l:'" + icu_tekanan_negatif_dengan_ventilator_suspect_l
                + "', icu_tekanan_negatif_dengan_ventilator_suspect_p:'" + icu_tekanan_negatif_dengan_ventilator_suspect_p
                + "', icu_tekanan_negatif_dengan_ventilator_confim_l:'" + icu_tekanan_negatif_dengan_ventilator_confim_l
                + "', icu_tekanan_negatif_dengan_ventilator_confim_p:'" + icu_tekanan_negatif_dengan_ventilator_confim_p
                + "', icu_tekanan_negatif_tanpa_ventilator_suspect_l:'" + icu_tekanan_negatif_tanpa_ventilator_suspect_l
                + "', icu_tekanan_negatif_tanpa_ventilator_suspect_p:'" + icu_tekanan_negatif_tanpa_ventilator_suspect_p
                + "', icu_tekanan_negatif_tanpa_ventilator_confirm_l:'" + icu_tekanan_negatif_tanpa_ventilator_confirm_l
                + "', icu_tekanan_negatif_tanpa_ventilator_confirm_p:'" + icu_tekanan_negatif_tanpa_ventilator_confirm_p
                + "', isolasi_tekanan_negatif_suspect_l:'" + isolasi_tekanan_negatif_suspect_l
                + "', isolasi_tekanan_negatif_suspect_p:'" + isolasi_tekanan_negatif_suspect_p
                + "', isolasi_tekanan_negatif_confirm_l:'" + isolasi_tekanan_negatif_confirm_l
                + "', isolasi_tekanan_negatif_confirm_p:'" + isolasi_tekanan_negatif_confirm_p
                + "', isolasi_tanpa_tekanan_negatif_suspect_l:'" + isolasi_tanpa_tekanan_negatif_suspect_l
                + "', isolasi_tanpa_tekanan_negatif_suspect_p:'" + isolasi_tanpa_tekanan_negatif_suspect_p
                + "', isolasi_tanpa_tekanan_negatif_confirm_l:'" + isolasi_tanpa_tekanan_negatif_confirm_l
                + "', isolasi_tanpa_tekanan_negatif_confirm_p:'" + isolasi_tanpa_tekanan_negatif_confirm_p
                + "', nicu_khusus_covid_suspect_l:'" + nicu_khusus_covid_suspect_l
                + "', nicu_khusus_covid_suspect_p:'" + nicu_khusus_covid_suspect_p
                + "', nicu_khusus_covid_confirm_l:'" + nicu_khusus_covid_confirm_l
                + "', nicu_khusus_covid_confirm_p:'" + nicu_khusus_covid_confirm_p
                + "', picu_khusus_covid_suspect_l:'" + picu_khusus_covid_suspect_l
                + "', picu_khusus_covid_suspect_p:'" + picu_khusus_covid_suspect_p
                + "', picu_khusus_covid_confirm_l:'" + picu_khusus_covid_confirm_l
                + "', picu_khusus_covid_confirm_p:'" + picu_khusus_covid_confirm_p + "'}",

            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (msg) {
                hideLoadingPanel();
                functionHandler(msg.d);
            },
            error: function (jqXHR, exception) {
                hideLoadingPanel();
                alert(jqXHR.responseText);
            },
            failure: function (msg) {
                hideLoadingPanel();
                alert(msg);
            }
        });
    };

    this.getPasienKeluar = function (functionHandler) {
        showLoadingPanel();
        var loc = ResolveUrl('~/Libs/Service/KemenKesService.asmx/GetPasienKeluar');
        $.ajax({
            type: "POST",
            url: ResolveUrl('~/Libs/Service/KemenKesService.asmx/GetPasienKeluar'),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (msg) {
                hideLoadingPanel();
                functionHandler(msg.d);
            },
            error: function (jqXHR, exception) {
                hideLoadingPanel();
                alert(jqXHR.responseText);
            },
            failure: function (msg) {
                hideLoadingPanel();
                alert(msg);
            }
        });
    };

    this.pasienKeluar = function (tanggal,
          sembuh,
          discarded,
          meninggal_komorbid,
          meninggal_tanpa_komorbid,
          meninggal_prob_pre_komorbid,
          meninggal_prob_neo_komorbid,
          meninggal_prob_bayi_komorbid,
          meninggal_prob_balita_komorbid,
          meninggal_prob_anak_komorbid,
          meninggal_prob_remaja_komorbid,
          meninggal_prob_dws_komorbid,
          meninggal_prob_lansia_komorbid,
          meninggal_prob_pre_tanpa_komorbid,
          meninggal_prob_neo_tanpa_komorbid,
          meninggal_prob_bayi_tanpa_komorbid,
          meninggal_prob_balita_tanpa_komorbid,
          meninggal_prob_anak_tanpa_komorbid,
          meninggal_prob_remaja_tanpa_komorbid,
          meninggal_prob_dws_tanpa_komorbid,
          meninggal_prob_lansia_tanpa_komorbid,
          meninggal_disarded_komorbid,
          meninggal_discarded_tanpa_komorbid,
          dirujuk,
          isman,
          aps, functionHandler) {
        $.ajax({
            type: "POST",
            beforeSend: function (msg) {
                showLoadingPanel();
            },
            url: ResolveUrl('~/Libs/Service/KemenKesService.asmx/PostPasienKeluar'),
            data: "{ tanggal :'"
                + tanggal + "', sembuh:'" + sembuh + "', discarded:'" + discarded
                + "', meninggal_komorbid:'" + meninggal_komorbid + "', meninggal_tanpa_komorbid:'" + meninggal_tanpa_komorbid
                + "', meninggal_prob_pre_komorbid:'" + meninggal_prob_pre_komorbid + "', meninggal_prob_neo_komorbid:'" + meninggal_prob_neo_komorbid
                + "', meninggal_prob_bayi_komorbid:'" + meninggal_prob_bayi_komorbid + "', meninggal_prob_balita_komorbid:'" + meninggal_prob_balita_komorbid
                + "', meninggal_prob_anak_komorbid:'" + meninggal_prob_anak_komorbid
                + "', meninggal_prob_remaja_komorbid:'" + meninggal_prob_remaja_komorbid
                + "', meninggal_prob_dws_komorbid:'" + meninggal_prob_dws_komorbid
                + "', meninggal_prob_lansia_komorbid:'" + meninggal_prob_lansia_komorbid
                + "', meninggal_prob_pre_tanpa_komorbid:'" + meninggal_prob_pre_tanpa_komorbid
                + "', meninggal_prob_neo_tanpa_komorbid:'" + meninggal_prob_neo_tanpa_komorbid
                + "', meninggal_prob_bayi_tanpa_komorbid:'" + meninggal_prob_bayi_tanpa_komorbid
                + "', meninggal_prob_balita_tanpa_komorbid:'" + meninggal_prob_balita_tanpa_komorbid
                + "', meninggal_prob_anak_tanpa_komorbid:'" + meninggal_prob_anak_tanpa_komorbid
                + "', meninggal_prob_remaja_tanpa_komorbid:'" + meninggal_prob_remaja_tanpa_komorbid
                + "', meninggal_prob_dws_tanpa_komorbid:'" + meninggal_prob_dws_tanpa_komorbid
                + "', meninggal_prob_lansia_tanpa_komorbid:'" + meninggal_prob_lansia_tanpa_komorbid
                + "', meninggal_disarded_komorbid:'" + meninggal_disarded_komorbid 
                + "', meninggal_discarded_tanpa_komorbid:'" + meninggal_discarded_tanpa_komorbid
                + "', dirujuk:'" + dirujuk
                + "', isman:'" + isman
                + "', aps:'" + aps
                + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (msg) {
                hideLoadingPanel();
                functionHandler(msg.d);
            },
            error: function (jqXHR, exception) {
                hideLoadingPanel();
                alert(jqXHR.responseText);
            },
            failure: function (msg) {
                hideLoadingPanel();
                alert(msg);
            }
        });
    };

    this.reinsertSIRANAPReferenceRuangDanTempatTidur = function (functionHandler) {
        showLoadingPanel();
        var loc = ResolveUrl('~/Libs/Service/KemenKesService.asmx/GetRuangDanTempatTidurList');
        $.ajax({
            type: "POST",
            url: ResolveUrl('~/Libs/Service/KemenKesService.asmx/GetRuangDanTempatTidurList'),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (msg) {
                hideLoadingPanel();
                functionHandler(msg.d);
            },
            error: function (jqXHR, exception) {
                hideLoadingPanel();
                alert(jqXHR.responseText);
            },
            failure: function (msg) {
                hideLoadingPanel();
                alert(msg);
            }
        });
    };

    this.reinsertSIRANAPReferenceSDM = function (functionHandler) {
        showLoadingPanel();
        var loc = ResolveUrl('~/Libs/Service/KemenKesService.asmx/GetSDMList');
        $.ajax({
            type: "POST",
            url: ResolveUrl('~/Libs/Service/KemenKesService.asmx/GetSDMList'),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (msg) {
                hideLoadingPanel();
                functionHandler(msg.d);
            },
            error: function (jqXHR, exception) {
                hideLoadingPanel();
                alert(jqXHR.responseText);
            },
            failure: function (msg) {
                hideLoadingPanel();
                alert(msg);
            }
        });
    };

    this.reinsertSIRANAPReferenceAPD = function (functionHandler) {
        showLoadingPanel();
        var loc = ResolveUrl('~/Libs/Service/KemenKesService.asmx/GetAPDList');
        $.ajax({
            type: "POST",
            url: ResolveUrl('~/Libs/Service/KemenKesService.asmx/GetAPDList'),
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (msg) {
                hideLoadingPanel();
                functionHandler(msg.d);
            },
            error: function (jqXHR, exception) {
                hideLoadingPanel();
                alert(jqXHR.responseText);
            },
            failure: function (msg) {
                hideLoadingPanel();
                alert(msg);
            }
        });
    };
})();