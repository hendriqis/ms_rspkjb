var InhealthService = new (function () {
    this.cekrestriksieprescriptions = function (token, kodeprovider,
                                                kodeobatrs, user, functionHandler) {
        $.ajax({
            type: "POST",
            beforeSend: function (msg) {
                showLoadingPanel();
            },
            url: ResolveUrl('~/Libs/Service/InhealthService.asmx/CekRestriksiEPrescription'),
            data: "{ token :'" + token + "', kodeprovider:'" + kodeprovider
                    + "', kodeobatrs:'" + kodeobatrs + "', user:'" + user + "'}",
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

    this.cekrestriksitransaksi = function (token, kodeprovider,
                                            nosjp, kodeobatrs,
                                            jumlahobat, user, functionHandler) {
        $.ajax({
            type: "POST",
            beforeSend: function (msg) {
                showLoadingPanel();
            },
            url: ResolveUrl('~/Libs/Service/InhealthService.asmx/CekRestriksiTransaksi'),
            data: "{ token :'" + token + "', kodeprovider:'" + kodeprovider
                    + "', nosjp:'" + nosjp + "', kodeobatrs:'" + kodeobatrs
                    + "', jumlahobat:'" + jumlahobat + "', user:'" + user + "'}",
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

    this.ceksjp = function (token, kodeprovider,
                            nokainhealth, tanggalsjp,
                            poli, tkp, functionHandler) {
        $.ajax({
            type: "POST",
            beforeSend: function (msg) {
                showLoadingPanel();
            },
            url: ResolveUrl('~/Libs/Service/InhealthService.asmx/CekSJP'),
            data: "{ token :'" + token + "', kodeprovider:'" + kodeprovider
                + "', nokainhealth:'" + nokainhealth + "', tanggalsjp:'" + tanggalsjp
                + "', poli:'" + poli + "', tkp:'" + tkp + "'}",
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

    this.cetaksjp = function (token, kodeprovider,
                                nosjp, tkp, tipefile, functionHandler) {
        $.ajax({
            type: "POST",
            beforeSend: function (msg) {
                showLoadingPanel();
            },
            url: ResolveUrl('~/Libs/Service/InhealthService.asmx/CetakSJP'),
            data: "{ token :'" + token + "', kodeprovider:'" + kodeprovider
                + "', nosjp:'" + nosjp + "', tkp:'" + tkp + "', tipefile:'" + tipefile + "'}",
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

    this.confirmaktfirstpayor = function (token, kodeprovider,
                                            nosjp, userid, functionHandler) {
        $.ajax({
            type: "POST",
            beforeSend: function (msg) {
                showLoadingPanel();
            },
            url: ResolveUrl('~/Libs/Service/InhealthService.asmx/ConfirmAKTFirstPayor'),
            data: "{ token :'" + token + "', kodeprovider:'" + kodeprovider
                    + "', nosjp:'" + nosjp + "', userid:'" + userid + "'}",
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

    this.eligibilitaspeserta = function (token, kodeprovider,
                                        nokainhealth, tglpelayanan,
                                        jenispelayanan, poli, functionHandler) {
        $.ajax({
            type: "POST",
            beforeSend: function (msg) {
                showLoadingPanel();
            },
            url: ResolveUrl('~/Libs/Service/InhealthService.asmx/EligibilitasPeserta'),
            data: "{ token :'" + token + "', kodeprovider:'" + kodeprovider
                + "', nokainhealth:'" + nokainhealth + "', tglpelayanan:'" + tglpelayanan
                + "', jenispelayanan:'" + jenispelayanan + "', poli:'" + poli + "'}",
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

    this.eligibilitaspeserta_API = function (nokainhealth, tglpelayanan,
                                        jenispelayanan, poli, functionHandler) {
        $.ajax({
            type: "POST",
            beforeSend: function (msg) {
                showLoadingPanel();
            },
            url: ResolveUrl('~/Libs/Service/InhealthService.asmx/EligibilitasPeserta_API'),
            data: "{ nokainhealth:'" + nokainhealth + "', tglpelayanan:'" + tglpelayanan
                + "', jenispelayanan:'" + jenispelayanan + "', poli:'" + poli + "'}",
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

    this.hapusdetailsjp = function (token, kodeprovider,
                                    nosjp, notes, userid, functionHandler) {
        $.ajax({
            type: "POST",
            beforeSend: function (msg) {
                showLoadingPanel();
            },
            url: ResolveUrl('~/Libs/Service/InhealthService.asmx/HapusDetailSJP'),
            data: "{ token :'" + token + "', kodeprovider:'" + kodeprovider
                + "', nosjp:'" + nosjp + "', notes:'" + notes + "', userid:'" + userid + "'}",
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

    this.hapusobatbykodeobatinh = function (token, kodeprovider,
                                            nosjp, noresep,
                                            kodeobat, alasan, user, functionHandler) {
        $.ajax({
            type: "POST",
            beforeSend: function (msg) {
                showLoadingPanel();
            },
            url: ResolveUrl('~/Libs/Service/InhealthService.asmx/HapusObatByKodeObatInh'),
            data: "{ token :'" + token + "', kodeprovider:'" + kodeprovider
                + "', nosjp:'" + nosjp + "', noresep:'" + noresep
                + "', kodeobat:'" + kodeobat + "', alasan:'" + alasan + "', user:'" + user + "'}",
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

    this.hapusobatbykodeobatrs = function (token, kodeprovider,
                                            nosjp, noresep,
                                            kodeobatrs, alasan, user, functionHandler) {
        $.ajax({
            type: "POST",
            beforeSend: function (msg) {
                showLoadingPanel();
            },
            url: ResolveUrl('~/Libs/Service/InhealthService.asmx/HapusObatByKodeObatInh'),
            data: "{ token :'" + token + "', kodeprovider:'" + kodeprovider
                + "', nosjp:'" + nosjp + "', noresep:'" + noresep
                + "', kodeobatrs:'" + kodeobatrs + "', alasan:'" + alasan + "', user:'" + user + "'}",
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

    this.hapussjp = function (token, kodeprovider,
                                nosjp, alasanhapus, userid, functionHandler) {
        $.ajax({
            type: "POST",
            beforeSend: function (msg) {
                showLoadingPanel();
            },
            url: ResolveUrl('~/Libs/Service/InhealthService.asmx/HapusSJP'),
            data: "{ token :'" + token + "', kodeprovider:'" + kodeprovider
                + "', nosjp:'" + nosjp + "', alasanhapus:'" + alasanhapus + "', userid:'" + userid + "'}",
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

    this.hapussjp_API = function (nosjp, alasanhapus, userid, functionHandler) {
        $.ajax({
            type: "POST",
            beforeSend: function (msg) {
                showLoadingPanel();
            },
            url: ResolveUrl('~/Libs/Service/InhealthService.asmx/HapusSJP_API'),
            data: "{ nosjp:'" + nosjp + "', alasanhapus:'" + alasanhapus + "', userid:'" + userid + "'}",
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

    this.hapustindakan = function (token, kodeprovider, nosjp, kodetindakan, tgltindakan, notes, userid, functionHandler) {
        $.ajax({
            type: "POST",
            beforeSend: function (msg) {
                showLoadingPanel();
            },
            url: ResolveUrl('~/Libs/Service/InhealthService.asmx/HapusTindakan'),
            data: "{ token :'" + token + "', kodeprovider:'" + kodeprovider + "', nosjp:'" + nosjp + "', kodetindakan:'" + kodetindakan + "', tgltindakan:'" + tgltindakan + "', notes:'" + notes + "', userid:'" + userid + "'}",
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

    this.infobenefit = function (token, kodeprovider, nokartu, tanggal, user, functionHandler) {
        $.ajax({
            type: "POST",
            beforeSend: function (msg) {
                showLoadingPanel();
            },
            url: ResolveUrl('~/Libs/Service/InhealthService.asmx/InfoBenefit'),
            data: "{ token :'" + token + "', kodeprovider:'" + kodeprovider + "', nokartu:'" + nokartu + "', tanggal:'" + tanggal + "', user:'" + user + "'}",
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
    this.infobenefit_API = function (nokartu, tanggal, user, functionHandler) {
        $.ajax({
            type: "POST",
            beforeSend: function (msg) {
                showLoadingPanel();
            },
            url: ResolveUrl('~/Libs/Service/InhealthService.asmx/InfoBenefit_API'),
            data: "{ nokartu:'" + nokartu + "', tanggal:'" + tanggal + "', user:'" + user + "'}",
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

    this.infosjp = function (token, kodeprovider, nosjp, functionHandler) {
        $.ajax({
            type: "POST",
            beforeSend: function (msg) {
                showLoadingPanel();
            },
            url: ResolveUrl('~/Libs/Service/InhealthService.asmx/InfoSJP'),
            data: "{ token :'" + token + "', kodeprovider:'" + kodeprovider + "', nosjp:'" + nosjp + "'}",
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

    this.infosjp_API = function (nosjp, functionHandler) {
        $.ajax({
            type: "POST",
            beforeSend: function (msg) {
                showLoadingPanel();
            },
            url: ResolveUrl('~/Libs/Service/InhealthService.asmx/InfoSJP_API'),
            data: "{ nosjp:'" + nosjp + "'}",
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

    this.poli = function (token, kodeprovider, keyword, functionHandler) {
        $.ajax({
            type: "POST",
            beforeSend: function (msg) {
                showLoadingPanel();
            },
            url: ResolveUrl('~/Libs/Service/InhealthService.asmx/Poli'),
            data: "{ token :'" + token + "', kodeprovider:'" + kodeprovider + "', keyword:'" + keyword + "'}",
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

    this.poli_API = function (keyword, functionHandler) {
        $.ajax({
            type: "POST",
            beforeSend: function (msg) {
                showLoadingPanel();
            },
            url: ResolveUrl('~/Libs/Service/InhealthService.asmx/Poli_API'),
            data: "{ keyword:'" + keyword + "'}",
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

    this.prosessjptofpk = function (token, kodeprovider, jeniscob, jenispelayanan, listnosjp, namapicprovider, noinvoiceprovider, username, functionHandler) {
        $.ajax({
            type: "POST",
            beforeSend: function (msg) {
                showLoadingPanel();
            },
            url: ResolveUrl('~/Libs/Service/InhealthService.asmx/ProsesSJPToFPK'),
            data: "{ token :'" + token + "', kodeprovider:'" + kodeprovider + "', jeniscob:'" + jeniscob + "', jenispelayanan:'" + jenispelayanan + "', listnosjp:'" + listnosjp + "', namapicprovider:'" + namapicprovider + "', username:'" + username + "'}",
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

    this.providerrujukan = function (token, kodeprovider, keyword, functionHandler) {
        $.ajax({
            type: "POST",
            beforeSend: function (msg) {
                showLoadingPanel();
            },
            url: ResolveUrl('~/Libs/Service/InhealthService.asmx/ProviderRujukan'),
            data: "{ token :'" + token + "', kodeprovider:'" + kodeprovider + "', keyword:'" + keyword + "'}",
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

    this.providerrujukan_API = function (keyword, functionHandler) {
        $.ajax({
            type: "POST",
            beforeSend: function (msg) {
                showLoadingPanel();
            },
            url: ResolveUrl('~/Libs/Service/InhealthService.asmx/ProviderRujukan_API'),
            data: "{ keyword:'" + keyword + "'}",
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

    this.rekaphasilverifikasi = function (token, kodeprovider, nofpk, functionHandler) {
        $.ajax({
            type: "POST",
            beforeSend: function (msg) {
                showLoadingPanel();
            },
            url: ResolveUrl('~/Libs/Service/InhealthService.asmx/RekapHasilVerifikasi'),
            data: "{ token :'" + token + "', kodeprovider:'" + kodeprovider + "', nofpk:'" + nofpk + "'}",
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

    this.simpanbiayainacbgs = function (token, kodeprovider, nosjp, kodeinacbg, biayainacbg, nosep, notes, userid, functionHandler) {
        $.ajax({
            type: "POST",
            beforeSend: function (msg) {
                showLoadingPanel();
            },
            url: ResolveUrl('~/Libs/Service/InhealthService.asmx/SimpanBiayaINACBGS'),
            data: "{ token :'" + token + "', kodeprovider:'" + kodeprovider + "', nosjp:'" + nosjp + "', kodeinacbg:'" + kodeinacbg + "', biayainacbg:'" + biayainacbg + "', nosep:'" + nosep + "', notes:'" + notes + "', userid:'" + userid + "'}",
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

    this.simpanobat = function (token, kodeprovider, nosjp, noresep, tanggalresep, tanggalobat, tipeobat, jenisracikan,
                                kodeobatrs, namaobat, kodedokter, jumlahobat, signa1, signa2, jumlahhari, hdasar,
                                confirmationcode, username, functionHandler) {
        $.ajax({
            type: "POST",
            beforeSend: function (msg) {
                showLoadingPanel();
            },
            url: ResolveUrl('~/Libs/Service/InhealthService.asmx/SimpanObat'),
            data: "{ token :'" + token + "', kodeprovider:'" + kodeprovider + "', nosjp:'" + nosjp + "', noresep:'" + noresep + "', tanggalresep:'" + tanggalresep
                            + "', tanggalobat:'" + tanggalobat + "', tipeobat:'" + tipeobat + "', jenisracikan:'" + jenisracikan +
                            +"', kodeobatrs:'" + kodeobatrs + "', namaobat:'" + namaobat + "', kodedokter:'" + kodedokter +
                            +"', jumlahobat:'" + jumlahobat + "', signa1:'" + signa1 + "', signa2:'" + signa2 +
                            +"', jumlahhari:'" + jumlahhari + "', hdasar:'" + hdasar + "', confirmationcode:'" + confirmationcode + "', username:'" + username + "'}",
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

    this.simpanruangrawat = function (token, kodeprovider, nosjp, tglmasuk, kelasrawat, kodejenispelayanan, byharirawat, functionHandler) {
        $.ajax({
            type: "POST",
            beforeSend: function (msg) {
                showLoadingPanel();
            },
            url: ResolveUrl('~/Libs/Service/InhealthService.asmx/SimpanRuangRawat'),
            data: "{ token :'" + token + "', kodeprovider:'" + kodeprovider + "', nosjp:'" + nosjp + "', tglmasuk:'" + tglmasuk + "', kelasrawat:'" + kelasrawat + "', kodejenispelayanan:'" + kodejenispelayanan + "', byharirawat:'" + byharirawat + "'}",
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

    this.simpansjp = function (token, kodeprovider, tanggalpelayanan, jenispelayanan, nokainhealth,
                                nomormedicalreport, nomorasalrujukan, kodeproviderasalrujukan, tanggalasalrujukan,
                                kodediagnosautama, poli, username, informasitambahan, kodediagnosatambahan, kecelakaankerja,
                                kelasrawat, kodejenpelruangrawat, functionHandler) {
        $.ajax({
            type: "POST",
            beforeSend: function (msg) {
                showLoadingPanel();
            },
            url: ResolveUrl('~/Libs/Service/InhealthService.asmx/SimpanSJP'),
            data: "{ token :'" + token + "', kodeprovider:'" + kodeprovider + "', tanggalpelayanan:'" + tanggalpelayanan
                    + "', jenispelayanan:'" + jenispelayanan + "', nokainhealth:'" + nokainhealth + "', nomormedicalreport:'" + nomormedicalreport
                    + "', nomorasalrujukan:'" + nomorasalrujukan + "', kodeproviderasalrujukan:'" + kodeproviderasalrujukan + "', tanggalasalrujukan:'" + tanggalasalrujukan
                    + "', kodediagnosautama:'" + kodediagnosautama + "', poli:'" + poli + "', username:'" + username
                    + "', informasitambahan:'" + informasitambahan + "', kodediagnosatambahan:'" + kodediagnosatambahan + "', kecelakaankerja:'" + kecelakaankerja
                    + "', kelasrawat:'" + kelasrawat + "', kodejenpelruangrawat:'" + kodejenpelruangrawat + "'}",
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

    this.simpansjp_API = function (tanggalpelayanan, jenispelayanan, nokainhealth,
                                nomormedicalreport, nomorasalrujukan, kodeproviderasalrujukan, tanggalasalrujukan,
                                kodediagnosautama, poli, username, informasitambahan, kodediagnosatambahan, kecelakaankerja,
                                kelasrawat, kodejenpelruangrawat, functionHandler) {
        $.ajax({
            type: "POST",
            beforeSend: function (msg) {
                showLoadingPanel();
            },
            url: ResolveUrl('~/Libs/Service/InhealthService.asmx/SimpanSJP_API'),
            data: "{ tanggalpelayanan:'" + tanggalpelayanan
                    + "', jenispelayanan:'" + jenispelayanan + "', nokainhealth:'" + nokainhealth + "', nomormedicalreport:'" + nomormedicalreport
                    + "', nomorasalrujukan:'" + nomorasalrujukan + "', kodeproviderasalrujukan:'" + kodeproviderasalrujukan + "', tanggalasalrujukan:'" + tanggalasalrujukan
                    + "', kodediagnosautama:'" + kodediagnosautama + "', poli:'" + poli + "', username:'" + username
                    + "', informasitambahan:'" + informasitambahan + "', kodediagnosatambahan:'" + kodediagnosatambahan + "', kecelakaankerja:'" + kecelakaankerja
                    + "', kelasrawat:'" + kelasrawat + "', kodejenpelruangrawat:'" + kodejenpelruangrawat + "'}",
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

    this.simpansjpV2_API = function (tanggalpelayanan, jenispelayanan, nokainhealth,
                                nomormedicalreport, nomorasalrujukan, kodeproviderasalrujukan, tanggalasalrujukan,
                                kodediagnosautama, poli, username, informasitambahan, kodediagnosatambahan, kecelakaankerja,
                                kelasrawat, kodejenpelruangrawat, nohp, email, claimidprovider, functionHandler) {
        $.ajax({
            type: "POST",
            beforeSend: function (msg) {
                showLoadingPanel();
            },
            url: ResolveUrl('~/Libs/Service/InhealthService.asmx/SimpanSJPV2_API'),
            data: "{ tanggalpelayanan:'" + tanggalpelayanan
                    + "', jenispelayanan:'" + jenispelayanan + "', nokainhealth:'" + nokainhealth + "', nomormedicalreport:'" + nomormedicalreport
                    + "', nomorasalrujukan:'" + nomorasalrujukan + "', kodeproviderasalrujukan:'" + kodeproviderasalrujukan + "', tanggalasalrujukan:'" + tanggalasalrujukan
                    + "', kodediagnosautama:'" + kodediagnosautama + "', poli:'" + poli + "', username:'" + username
                    + "', informasitambahan:'" + informasitambahan + "', kodediagnosatambahan:'" + kodediagnosatambahan + "', kecelakaankerja:'" + kecelakaankerja
                    + "', kelasrawat:'" + kelasrawat + "', kodejenpelruangrawat:'" + kodejenpelruangrawat + "', nohp:'" + nohp + "', email:'" + email + "', claimidprovider:'" + claimidprovider 
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

    this.simpantindakan = function (token, kodeprovider, jenispelayanan, nosjp, tglmasukrawat, tanggalpelayanan,
                                    kodetindakan, poli, kodedokter, biayaaju, functionHandler) {
        $.ajax({
            type: "POST",
            beforeSend: function (msg) {
                showLoadingPanel();
            },
            url: ResolveUrl('~/Libs/Service/InhealthService.asmx/SimpanTindakan'),
            data: "{ token :'" + token + "', kodeprovider:'" + kodeprovider + "', jenispelayanan:'" + jenispelayanan + "', nosjp:'" + nosjp
                    + "', tglmasukrawat:'" + tglmasukrawat + "', tanggalpelayanan:'" + tanggalpelayanan + "', kodetindakan:'" + kodetindakan +
                    +"', poli:'" + poli + "', kodedokter:'" + kodedokter + "', biayaaju:'" + biayaaju + "'}",
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

    this.simpantindakanritl = function (token, kodeprovider, jenispelayanan, nosjp, idakomodasi, tglmasukrawat, tanggalpelayanan,
                                    kodetindakan, poli, kodedokter, biayaaju, functionHandler) {
        $.ajax({
            type: "POST",
            beforeSend: function (msg) {
                showLoadingPanel();
            },
            url: ResolveUrl('~/Libs/Service/InhealthService.asmx/SimpanTindakanRITL'),
            data: "{ token :'" + token + "', kodeprovider:'" + kodeprovider + "', jenispelayanan:'" + jenispelayanan + "', nosjp:'" + nosjp + "', idakomodasi:'" + idakomodasi
                    + "', tglmasukrawat:'" + tglmasukrawat + "', tanggalpelayanan:'" + tanggalpelayanan + "', kodetindakan:'" + kodetindakan +
                    +"', poli:'" + poli + "', kodedokter:'" + kodedokter + "', biayaaju:'" + biayaaju + "'}",
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

    this.updatesjp = function (token, kodeprovider, nosjp, nomormedicalreport, nomorasalrujukan,
                                kodeproviderasalrujukan, tanggalasalrujukan, poli, username,
                                informasitambahan, kodediagnosatambahan, kecelakaankerja, functionHandler) {
        $.ajax({
            type: "POST",
            beforeSend: function (msg) {
                showLoadingPanel();
            },
            url: ResolveUrl('~/Libs/Service/InhealthService.asmx/UpdateSJP'),
            data: "{ token :'" + token + "', kodeprovider:'" + kodeprovider + "', nosjp:'" + nosjp + "', nomormedicalreport:'" + nomormedicalreport
                    + "', nomorasalrujukan:'" + nomorasalrujukan + "', kodeproviderasalrujukan:'" + kodeproviderasalrujukan + "', tanggalasalrujukan:'" + tanggalasalrujukan +
                    +"', poli:'" + poli + "', username:'" + username + "', informasitambahan:'" + informasitambahan +
                    +"', kodediagnosatambahan:'" + kodediagnosatambahan + "', kecelakaankerja:'" + kecelakaankerja + "'}",
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

    this.updatetanggalpulang = function (token, kodeprovider, id, nosjp, tglmasuk, tglkeluar, functionHandler) {
        $.ajax({
            type: "POST",
            beforeSend: function (msg) {
                showLoadingPanel();
            },
            url: ResolveUrl('~/Libs/Service/InhealthService.asmx/UpdateSJP'),
            data: "{ token :'" + token + "', kodeprovider:'" + kodeprovider + "', id:'" + id + "', nosjp:'" + nosjp
                    + "', tglmasuk:'" + tglmasuk + "', tglkeluar:'" + tglkeluar + "'}",
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

    this.updatetanggalpulang_API = function (id, nosjp, tglmasuk, tglkeluar, functionHandler) {
        $.ajax({
            type: "POST",
            beforeSend: function (msg) {
                showLoadingPanel();
            },
            url: ResolveUrl('~/Libs/Service/InhealthService.asmx/UpdateTanggalPulang_API'),
            data: "{ id:'" + id + "', nosjp:'" + nosjp
                    + "', tglmasuk:'" + tglmasuk + "', tglkeluar:'" + tglkeluar + "'}",
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

    this.listpraregistrasi_API = function (tanggal, query, functionHandler) {
        $.ajax({
            type: "POST",
            beforeSend: function (msg) {
                showLoadingPanel();
            },
            url: ResolveUrl('~/Libs/Service/InhealthService.asmx/ListPraRegistrasi'),
            data: "{ tanggal:'" + tanggal
                    + "', query:'" + query
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

    this.detailpraregistrasi_API = function (nomorpraregistrasi, functionHandler) {
        $.ajax({
            type: "POST",
            beforeSend: function (msg) {
                showLoadingPanel();
            },
            url: ResolveUrl('~/Libs/Service/InhealthService.asmx/DetailPraRegistrasi'),
            data: "{ nomorpraregistrasi:'" + nomorpraregistrasi
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
})();