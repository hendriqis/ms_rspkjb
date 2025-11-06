var BPJSService = new (function () {

    this.reinsertBPJSReferenceDiagnose = function (keyword, functionHandler) {

        showLoadingPanel();
        var loc = ResolveUrl('~/Libs/Service/BPJSService.asmx/GetReferensiBPJSDiagnosaList');
        $.ajax({
            type: "POST",
            beforeSend: function (msg) {
                showLoadingPanel();
            },
            url: ResolveUrl('~/Libs/Service/BPJSService.asmx/GetReferensiBPJSDiagnosaList'),
            data: "{ keyword:'" + keyword + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (msg) {
                hideLoadingPanel();
                functionHandler(msg.d);
            },
            error: function (jqXHR, exception) {
                hideLoadingPanel();
            },
            failure: function (msg) {
                hideLoadingPanel();
            }
        });
    };


    this.reinsertBPJSReferenceDiagnoseMedinfrasAPI = function (keyword, functionHandler) {

        showLoadingPanel();
        var loc = ResolveUrl('~/Libs/Service/BPJSService.asmx/GetReferensiBPJSDiagnosaList_MEDINFRASAPI');
        $.ajax({
            type: "POST",
            beforeSend: function (msg) {
                showLoadingPanel();
            },
            url: ResolveUrl('~/Libs/Service/BPJSService.asmx/GetReferensiBPJSDiagnosaList_MEDINFRASAPI'),
            data: "{ keyword:'" + keyword + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (msg) {
                hideLoadingPanel();
                functionHandler(msg.d);
            },
            error: function (jqXHR, exception) {
                hideLoadingPanel();
            },
            failure: function (msg) {
                hideLoadingPanel();
            }
        });
    };

    this.reinsertBPJSReferencePoli = function (keyword, functionHandler) {

        showLoadingPanel();
        var loc = ResolveUrl('~/Libs/Service/BPJSService.asmx/GetReferensiBPJSPoliList');
        $.ajax({
            type: "POST",
            beforeSend: function (msg) {
                showLoadingPanel();
            },
            url: ResolveUrl('~/Libs/Service/BPJSService.asmx/GetReferensiBPJSPoliList'),
            data: "{ keyword:'" + keyword + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (msg) {
                hideLoadingPanel();
                functionHandler(msg.d);
            },
            error: function (jqXHR, exception) {
                hideLoadingPanel();
            },
            failure: function (msg) {
                hideLoadingPanel();
            }
        });
    };

    this.reinsertBPJSReferencePoliMedinfrasAPI = function (keyword, functionHandler) {

        showLoadingPanel();
        var loc = ResolveUrl('~/Libs/Service/BPJSService.asmx/GetReferensiBPJSPoliList_MEDINFRASAPI');
        $.ajax({
            type: "POST",
            beforeSend: function (msg) {
                showLoadingPanel();
            },
            url: ResolveUrl('~/Libs/Service/BPJSService.asmx/GetReferensiBPJSPoliList_MEDINFRASAPI'),
            data: "{ keyword:'" + keyword + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (msg) {
                hideLoadingPanel();
                functionHandler(msg.d);
            },
            error: function (jqXHR, exception) {
                hideLoadingPanel();
            },
            failure: function (msg) {
                hideLoadingPanel();
            }
        });
    };

    this.reinsertBPJSReferenceFasilitasKesehatan = function (keyword1, keyword2, functionHandler) {

        showLoadingPanel();
        var loc = ResolveUrl('~/Libs/Service/BPJSService.asmx/GetReferensiBPJSFasilitasKesehatanList');
        $.ajax({
            type: "POST",
            beforeSend: function (msg) {
                showLoadingPanel();
            },
            url: ResolveUrl('~/Libs/Service/BPJSService.asmx/GetReferensiBPJSFasilitasKesehatanList'),
            data: "{ keyword1 :'" + keyword1 + "', keyword2:'" + keyword2 + "' }",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (msg) {
                hideLoadingPanel();
                functionHandler(msg.d);
            },
            error: function (jqXHR, exception) {
                hideLoadingPanel();
            },
            failure: function (msg) {
                hideLoadingPanel();
            }
        });
    };

    this.reinsertBPJSReferenceFasilitasKesehatanMedinfrasAPI = function (keyword1, keyword2, functionHandler) {
        showLoadingPanel();
        var loc = ResolveUrl('~/Libs/Service/BPJSService.asmx/GetReferensiBPJSFasilitasKesehatanList_MEDINFRASAPI');
        $.ajax({
            type: "POST",
            beforeSend: function (msg) {
                showLoadingPanel();
            },
            url: ResolveUrl('~/Libs/Service/BPJSService.asmx/GetReferensiBPJSFasilitasKesehatanList_MEDINFRASAPI'),
            data: "{ keyword1 :'" + keyword1 + "', keyword2:'" + keyword2 + "' }",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (msg) {
                hideLoadingPanel();
                functionHandler(msg.d);
            },
            error: function (jqXHR, exception) {
                hideLoadingPanel();
            },
            failure: function (msg) {
                hideLoadingPanel();
            }
        });
    };

    this.reinsertBPJSReferenceKabupaten = function (keyword, functionHandler) {

        showLoadingPanel();
        var loc = ResolveUrl('~/Libs/Service/BPJSService.asmx/GetReferensiBPJSKabupatenList');
        $.ajax({
            type: "POST",
            beforeSend: function (msg) {
                showLoadingPanel();
            },
            url: ResolveUrl('~/Libs/Service/BPJSService.asmx/GetReferensiBPJSKabupatenList'),
            data: "{ keyword:'" + keyword + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (msg) {
                hideLoadingPanel();
                functionHandler(msg.d);
            },
            error: function (jqXHR, exception) {
                hideLoadingPanel();
            },
            failure: function (msg) {
                hideLoadingPanel();
            }
        });
    };

    this.reinsertBPJSReferenceKabupatenMedinfrasAPI = function (keyword, functionHandler) {

        showLoadingPanel();
        var loc = ResolveUrl('~/Libs/Service/BPJSService.asmx/GetReferensiBPJSKabupatenList_MEDINFRASAPI');
        $.ajax({
            type: "POST",
            beforeSend: function (msg) {
                showLoadingPanel();
            },
            url: ResolveUrl('~/Libs/Service/BPJSService.asmx/GetReferensiBPJSKabupatenList_MEDINFRASAPI'),
            data: "{ keyword:'" + keyword + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (msg) {
                hideLoadingPanel();
                functionHandler(msg.d);
            },
            error: function (jqXHR, exception) {
                hideLoadingPanel();
            },
            failure: function (msg) {
                hideLoadingPanel();
            }
        });
    };

    this.reinsertBPJSReferenceKecamatan = function (keyword, functionHandler) {

        showLoadingPanel();
        var loc = ResolveUrl('~/Libs/Service/BPJSService.asmx/GetReferensiBPJSKecamatanList');
        $.ajax({
            type: "POST",
            beforeSend: function (msg) {
                showLoadingPanel();
            },
            url: ResolveUrl('~/Libs/Service/BPJSService.asmx/GetReferensiBPJSKecamatanList'),
            data: "{ keyword:'" + keyword + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (msg) {
                hideLoadingPanel();
                functionHandler(msg.d);
            },
            error: function (jqXHR, exception) {
                hideLoadingPanel();
            },
            failure: function (msg) {
                hideLoadingPanel();
            }
        });
    };

    this.reinsertBPJSReferenceKecamatanMedinfrasAPI = function (keyword, functionHandler) {

        showLoadingPanel();
        var loc = ResolveUrl('~/Libs/Service/BPJSService.asmx/GetReferensiBPJSKecamatanList_MEDINFRASAPI');
        $.ajax({
            type: "POST",
            beforeSend: function (msg) {
                showLoadingPanel();
            },
            url: ResolveUrl('~/Libs/Service/BPJSService.asmx/GetReferensiBPJSKecamatanList_MEDINFRASAPI'),
            data: "{ keyword:'" + keyword + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (msg) {
                hideLoadingPanel();
                functionHandler(msg.d);
            },
            error: function (jqXHR, exception) {
                hideLoadingPanel();
            },
            failure: function (msg) {
                hideLoadingPanel();
            }
        });
    };

    this.reinsertBPJSReferencePropinsi = function (keyword, functionHandler) {

        showLoadingPanel();
        var loc = ResolveUrl('~/Libs/Service/BPJSService.asmx/GetReferensiBPJSPropinsiList');
        $.ajax({
            type: "POST",
            beforeSend: function (msg) {
                showLoadingPanel();
            },
            url: ResolveUrl('~/Libs/Service/BPJSService.asmx/GetReferensiBPJSPropinsiList'),
            data: "{ keyword:'" + keyword + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (msg) {
                hideLoadingPanel();
                functionHandler(msg.d);
            },
            error: function (jqXHR, exception) {
                hideLoadingPanel();
            },
            failure: function (msg) {
                hideLoadingPanel();
            }
        });
    };

    this.reinsertBPJSReferencePropinsiMedinfrasAPI = function (keyword, functionHandler) {

        showLoadingPanel();
        var loc = ResolveUrl('~/Libs/Service/BPJSService.asmx/GetReferensiBPJSPropinsiList_MEDINFRASAPI');
        $.ajax({
            type: "POST",
            beforeSend: function (msg) {
                showLoadingPanel();
            },
            url: ResolveUrl('~/Libs/Service/BPJSService.asmx/GetReferensiBPJSPropinsiList_MEDINFRASAPI'),
            data: "{ keyword:'" + keyword + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (msg) {
                hideLoadingPanel();
                functionHandler(msg.d);
            },
            error: function (jqXHR, exception) {
                hideLoadingPanel();
            },
            failure: function (msg) {
                hideLoadingPanel();
            }
        });
    };

    this.reinsertBPJSReferenceDokterDPJP = function (keyword1, keyword2, keyword3, functionHandler) {

        showLoadingPanel();
        var loc = ResolveUrl('~/Libs/Service/BPJSService.asmx/GetReferensiBPJSDokterDPJPList');
        $.ajax({
            type: "POST",
            beforeSend: function (msg) {
                showLoadingPanel();
            },
            url: ResolveUrl('~/Libs/Service/BPJSService.asmx/GetReferensiBPJSDokterDPJPList'),
            data: "{ keyword1:'" + keyword1 + "', keyword2:'" + keyword2 + "', keyword3:'" + keyword3 + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (msg) {
                hideLoadingPanel();
                functionHandler(msg.d);
            },
            error: function (jqXHR, exception) {
                hideLoadingPanel();
            },
            failure: function (msg) {
                hideLoadingPanel();
            }
        });
    };


    this.reinsertBPJSReferenceDokterDPJPMedinfrasAPI = function (keyword1, keyword2, keyword3, functionHandler) {
        showLoadingPanel();
        var loc = ResolveUrl('~/Libs/Service/BPJSService.asmx/GetReferensiBPJSDokterDPJPList_MEDINFRASAPI');
        $.ajax({
            type: "POST",
            beforeSend: function (msg) {
                showLoadingPanel();
            },
            url: ResolveUrl('~/Libs/Service/BPJSService.asmx/GetReferensiBPJSDokterDPJPList_MEDINFRASAPI'),
            data: "{ keyword1:'" + keyword1 + "', keyword2:'" + keyword2 + "', keyword3:'" + keyword3 + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (msg) {
                hideLoadingPanel();
                functionHandler(msg.d);
            },
            error: function (jqXHR, exception) {
                hideLoadingPanel();
            },
            failure: function (msg) {
                hideLoadingPanel();
            }
        });
    };

    this.reinsertBPJSReferenceProcedure = function (keyword, functionHandler) {
        showLoadingPanel();
        var loc = ResolveUrl('~/Libs/Service/BPJSService.asmx/GetProcedureList');
        $.ajax({
            type: "POST",
            beforeSend: function (msg) {
                showLoadingPanel();
            },
            url: ResolveUrl('~/Libs/Service/BPJSService.asmx/GetProcedureList'),
            data: "{ keyword:'" + keyword + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (msg) {
                hideLoadingPanel();
                functionHandler(msg.d);
            },
            error: function (jqXHR, exception) {
                alert(jqXHR.responseText);
            },
            failure: function (msg) {
                hideLoadingPanel();
            }
        });
    };

    this.reinsertBPJSReferenceProcedureMedinfrasAPI = function (keyword, functionHandler) {
        showLoadingPanel();
        var loc = ResolveUrl('~/Libs/Service/BPJSService.asmx/GetProcedureList_MEDINFRASAPI');
        $.ajax({
            type: "POST",
            beforeSend: function (msg) {
                showLoadingPanel();
            },
            url: ResolveUrl('~/Libs/Service/BPJSService.asmx/GetProcedureList_MEDINFRASAPI'),
            data: "{ keyword:'" + keyword + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (msg) {
                hideLoadingPanel();
                functionHandler(msg.d);
            },
            error: function (jqXHR, exception) {
                alert(jqXHR.responseText);
            },
            failure: function (msg) {
                hideLoadingPanel();
            }
        });
    };


    this.reinsertBPJSReferenceClassCare = function (keyword, functionHandler) {
        showLoadingPanel();
        var loc = ResolveUrl('~/Libs/Service/BPJSService.asmx/GetClassCareList');
        $.ajax({
            type: "POST",
            beforeSend: function (msg) {
                showLoadingPanel();
            },
            url: ResolveUrl('~/Libs/Service/BPJSService.asmx/GetClassCareList'),
            data: "{ keyword:'" + keyword + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (msg) {
                hideLoadingPanel();
                functionHandler(msg.d);
            },
            error: function (jqXHR, exception) {
                alert(jqXHR.responseText);
            },
            failure: function (msg) {
                hideLoadingPanel();
            }
        });
    };

    this.reinsertBPJSReferenceClassCareMedinfrasAPI = function (keyword, functionHandler) {
        showLoadingPanel();
        var loc = ResolveUrl('~/Libs/Service/BPJSService.asmx/GetClassCareList_MEDINFRASAPI');
        $.ajax({
            type: "POST",
            beforeSend: function (msg) {
                showLoadingPanel();
            },
            url: ResolveUrl('~/Libs/Service/BPJSService.asmx/GetClassCareList_MEDINFRASAPI'),
            data: "{ keyword:'" + keyword + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (msg) {
                hideLoadingPanel();
                functionHandler(msg.d);
            },
            error: function (jqXHR, exception) {
                alert(jqXHR.responseText);
            },
            failure: function (msg) {
                hideLoadingPanel();
            }
        });
    };

    this.reinsertBPJSReferenceParamedic = function (keyword, functionHandler) {
        showLoadingPanel();
        var loc = ResolveUrl('~/Libs/Service/BPJSService.asmx/GetParamedicList');
        $.ajax({
            type: "POST",
            beforeSend: function (msg) {
                showLoadingPanel();
            },
            url: ResolveUrl('~/Libs/Service/BPJSService.asmx/GetParamedicList'),
            data: "{ keyword:'" + keyword + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (msg) {
                hideLoadingPanel();
                functionHandler(msg.d);
            },
            error: function (jqXHR, exception) {
                alert(jqXHR.responseText);
            },
            failure: function (msg) {
                hideLoadingPanel();
            }
        });
    };


    this.reinsertBPJSReferenceParamedicMedinfrasAPI = function (keyword, functionHandler) {
        showLoadingPanel();
        var loc = ResolveUrl('~/Libs/Service/BPJSService.asmx/GetParamedicList_MEDINFRASAPI');
        $.ajax({
            type: "POST",
            beforeSend: function (msg) {
                showLoadingPanel();
            },
            url: ResolveUrl('~/Libs/Service/BPJSService.asmx/GetParamedicList_MEDINFRASAPI'),
            data: "{ keyword:'" + keyword + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (msg) {
                hideLoadingPanel();
                functionHandler(msg.d);
            },
            error: function (jqXHR, exception) {
                alert(jqXHR.responseText);
            },
            failure: function (msg) {
                hideLoadingPanel();
            }
        });
    };

    this.reinsertBPJSReferenceSpeciality = function (keyword, functionHandler) {
        showLoadingPanel();
        var loc = ResolveUrl('~/Libs/Service/BPJSService.asmx/GetSpecialityList');
        $.ajax({
            type: "POST",
            beforeSend: function (msg) {
                showLoadingPanel();
            },
            url: ResolveUrl('~/Libs/Service/BPJSService.asmx/GetSpecialityList'),
            data: "{ keyword:'" + keyword + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (msg) {
                hideLoadingPanel();
                functionHandler(msg.d);
            },
            error: function (jqXHR, exception) {
                alert(jqXHR.responseText);
            },
            failure: function (msg) {
                hideLoadingPanel();
            }
        });
    };

    this.reinsertBPJSReferenceSpecialityMedinfrasAPI = function (keyword, functionHandler) {
        showLoadingPanel();
        var loc = ResolveUrl('~/Libs/Service/BPJSService.asmx/GetSpecialityList_MEDINFRASAPI');
        $.ajax({
            type: "POST",
            beforeSend: function (msg) {
                showLoadingPanel();
            },
            url: ResolveUrl('~/Libs/Service/BPJSService.asmx/GetSpecialityList_MEDINFRASAPI'),
            data: "{ keyword:'" + keyword + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (msg) {
                hideLoadingPanel();
                functionHandler(msg.d);
            },
            error: function (jqXHR, exception) {
                alert(jqXHR.responseText);
            },
            failure: function (msg) {
                hideLoadingPanel();
            }
        });
    };

    this.reinsertBPJSReferenceRuangRawat = function (keyword, functionHandler) {
        showLoadingPanel();
        var loc = ResolveUrl('~/Libs/Service/BPJSService.asmx/GetRuangRawatList');
        $.ajax({
            type: "POST",
            beforeSend: function (msg) {
                showLoadingPanel();
            },
            url: ResolveUrl('~/Libs/Service/BPJSService.asmx/GetRuangRawatList'),
            data: "{ keyword:'" + keyword + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (msg) {
                hideLoadingPanel();
                functionHandler(msg.d);
            },
            error: function (jqXHR, exception) {
                alert(jqXHR.responseText);
            },
            failure: function (msg) {
                hideLoadingPanel();
            }
        });
    };

    this.reinsertBPJSReferenceRuangRawatMedinfrasAPI = function (keyword, functionHandler) {
        showLoadingPanel();
        var loc = ResolveUrl('~/Libs/Service/BPJSService.asmx/GetRuangRawatList_MEDINFRASAPI');
        $.ajax({
            type: "POST",
            beforeSend: function (msg) {
                showLoadingPanel();
            },
            url: ResolveUrl('~/Libs/Service/BPJSService.asmx/GetRuangRawatList_MEDINFRASAPI'),
            data: "{ keyword:'" + keyword + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (msg) {
                hideLoadingPanel();
                functionHandler(msg.d);
            },
            error: function (jqXHR, exception) {
                alert(jqXHR.responseText);
            },
            failure: function (msg) {
                hideLoadingPanel();
            }
        });
    };

    this.reinsertBPJSReferenceCaraKeluar = function (keyword, functionHandler) {
        showLoadingPanel();
        var loc = ResolveUrl('~/Libs/Service/BPJSService.asmx/GetCaraKeluarList');
        $.ajax({
            type: "POST",
            beforeSend: function (msg) {
                showLoadingPanel();
            },
            url: ResolveUrl('~/Libs/Service/BPJSService.asmx/GetCaraKeluarList'),
            data: "{ keyword:'" + keyword + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (msg) {
                hideLoadingPanel();
                functionHandler(msg.d);
            },
            error: function (jqXHR, exception) {
                alert(jqXHR.responseText);
            },
            failure: function (msg) {
                hideLoadingPanel();
            }
        });
    };

    this.reinsertBPJSReferenceCaraKeluarMedinfrasAPI = function (keyword, functionHandler) {
        showLoadingPanel();
        var loc = ResolveUrl('~/Libs/Service/BPJSService.asmx/GetCaraKeluarList_MEDINFRASAPI');
        $.ajax({
            type: "POST",
            beforeSend: function (msg) {
                showLoadingPanel();
            },
            url: ResolveUrl('~/Libs/Service/BPJSService.asmx/GetCaraKeluarList_MEDINFRASAPI'),
            data: "{ keyword:'" + keyword + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (msg) {
                hideLoadingPanel();
                functionHandler(msg.d);
            },
            error: function (jqXHR, exception) {
                alert(jqXHR.responseText);
            },
            failure: function (msg) {
                hideLoadingPanel();
            }
        });
    };

    this.reinsertBPJSReferencePascaPulang = function (keyword, functionHandler) {
        showLoadingPanel();
        var loc = ResolveUrl('~/Libs/Service/BPJSService.asmx/GetPascaPulangList');
        $.ajax({
            type: "POST",
            beforeSend: function (msg) {
                showLoadingPanel();
            },
            url: ResolveUrl('~/Libs/Service/BPJSService.asmx/GetPascaPulangList'),
            data: "{ keyword:'" + keyword + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (msg) {
                hideLoadingPanel();
                functionHandler(msg.d);
            },
            error: function (jqXHR, exception) {
                alert(jqXHR.responseText);
            },
            failure: function (msg) {
                hideLoadingPanel();
            }
        });
    };


    this.reinsertBPJSReferencePascaPulangMedinfrasAPI = function (keyword, functionHandler) {
        showLoadingPanel();
        var loc = ResolveUrl('~/Libs/Service/BPJSService.asmx/GetPascaPulangList_MEDINFRASAPI');
        $.ajax({
            type: "POST",
            beforeSend: function (msg) {
                showLoadingPanel();
            },
            url: ResolveUrl('~/Libs/Service/BPJSService.asmx/GetPascaPulangList_MEDINFRASAPI'),
            data: "{ keyword:'" + keyword + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (msg) {
                hideLoadingPanel();
                functionHandler(msg.d);
            },
            error: function (jqXHR, exception) {
                alert(jqXHR.responseText);
            },
            failure: function (msg) {
                hideLoadingPanel();
            }
        });
    };


    this.getPeserta = function (noPeserta, tglSEP, functionHandler) {
        showLoadingPanel();
        var loc = ResolveUrl('~/Libs/Service/BPJSService.asmx/GetPeserta');
        $.ajax({
            type: "POST",
            url: ResolveUrl('~/Libs/Service/BPJSService.asmx/GetPeserta'),
            data: "{ NoPeserta:'" + noPeserta + "', tglSEP:'" + tglSEP + "'}",
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

    this.getPesertaMedinfrasAPI = function (noPeserta, tglSEP, functionHandler) {
        showLoadingPanel();
        var loc = ResolveUrl('~/Libs/Service/BPJSService.asmx/GetPeserta_MEDINFRASAPI');
        $.ajax({
            type: "POST",
            url: ResolveUrl('~/Libs/Service/BPJSService.asmx/GetPeserta_MEDINFRASAPI'),
            data: "{ NoPeserta:'" + noPeserta + "', tglSEP:'" + tglSEP + "'}",
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


    this.getPesertaByNIK = function (NIK, tglSEP, functionHandler) {
        showLoadingPanel();
        $.ajax({
            type: "POST",
            url: ResolveUrl('~/Libs/Service/BPJSService.asmx/GetPesertaByNIK'),
            data: "{ NIK:'" + NIK + "', tglSEP:'" + tglSEP + "'}",
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

    this.getPesertaByNIKMedinfrasAPI = function (NIK, tglSEP, functionHandler) {
        showLoadingPanel();
        $.ajax({
            type: "POST",
            url: ResolveUrl('~/Libs/Service/BPJSService.asmx/GetPesertaByNIK_MEDINFRASAPI'),
            data: "{ NIK:'" + NIK + "', tglSEP:'" + tglSEP + "'}",
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

    this.getRujukan = function (noRujukan, asalRujukan, functionHandler) {
        showLoadingPanel();
        $.ajax({
            type: "POST",
            url: ResolveUrl('~/Libs/Service/BPJSService.asmx/GetRujukan'),
            data: "{ noRujukan :'" + noRujukan + "', asalRujukan :'" + asalRujukan + "'}",
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

    this.getRujukanMedinfrasAPI = function (noRujukan, asalRujukan, functionHandler) {
        showLoadingPanel();
        $.ajax({
            type: "POST",
            url: ResolveUrl('~/Libs/Service/BPJSService.asmx/GetRujukan_MedinfrasAPI'),
            data: "{ noRujukan :'" + noRujukan + "', asalRujukan :'" + asalRujukan + "'}",
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

    this.getRujukanList = function (noPeserta, asalRujukan, functionHandler) {
        showLoadingPanel();
        $.ajax({
            type: "POST",
            url: ResolveUrl('~/Libs/Service/BPJSService.asmx/GetRujukanListByNoPeserta'),
            data: "{ noPeserta :'" + noPeserta + "', asalRujukan :'" + asalRujukan + "'}",
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

    this.getRujukanListMedinfrasAPI = function (noPeserta, asalRujukan, functionHandler) {
        showLoadingPanel();
        $.ajax({
            type: "POST",
            url: ResolveUrl('~/Libs/Service/BPJSService.asmx/GetRujukanListByNoPeserta_MEDINFRASAPI'),
            data: "{ noPeserta :'" + noPeserta + "', asalRujukan :'" + asalRujukan + "'}",
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

    this.generateNoSEP = function (noKartu, tglSEP, tglRujukan, noRujukan, ppkRujukan, jnsPelayanan, catatan, diagAwal, poliTujuan, klsRawat, lakaLantas, lokasiLaka, noMR, asalRujukan, cob, poliEksekutif, mobilePhoneNo, penjamin, katarak, suplesi, noSepSuplesi, kodePropinsi, kodeKabupaten, kodeKecamatan, noSKDP, kodeDPJP, functionHandler) {
        $.ajax({
            type: "POST",
            beforeSend: function (msg) {
                showLoadingPanel();
            },
            url: ResolveUrl('~/Libs/Service/BPJSService.asmx/GenerateNoSEP'),
            data: "{ noKartu :'" + noKartu + "', tglSEP:'" + tglSEP + "', tglRujukan:'" + tglRujukan + "', noRujukan:'" + noRujukan + "', ppkRujukan:'" + ppkRujukan + "', jnsPelayanan:'" + jnsPelayanan + "', catatan:'" + catatan + "', diagAwal:'" + diagAwal + "', poliTujuan:'" + poliTujuan + "', klsRawat:'" + klsRawat + "', lakaLantas:'" + lakaLantas + "', lokasiLaka:'" + lokasiLaka + "', noMR:'" + noMR + "', asalRujukan:'" + asalRujukan + "', cob:'" + cob + "', poliEksekutif:'" + poliEksekutif + "', mobilePhoneNo:'" + mobilePhoneNo + "', penjamin:'" + penjamin + "', katarak:'" + katarak +
             "', suplesi:'" + suplesi + "', noSepSuplesi:'" + noSepSuplesi + "', kodePropinsi:'" + kodePropinsi + "', kodeKabupaten:'" + kodeKabupaten + "', kodeKecamatan:'" + kodeKecamatan + "', noSKDP:'" + noSKDP + "', kodeDPJP:'" + kodeDPJP + "'}",
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

    this.generateNoSEPMedinfrasAPI = function (noKartu, tglSEP, ppkPelayanan, jnsPelayanan, klsRawatHak, klsRawatNaik, pembiayaan, penanggungJawab, medicalNo, asalRujukan,
									tglRujukan, noRujukan, ppkRujukan, catatan, diagnosa, poliTujuan, poliEksekutif, cob, katarak, lakaLantas, keterangan, suplesi,
									noSepSuplesi, kodePropinsi, kodeKabupaten, kodeKecamatan, tujuanKunj, flagProcedure, kdPenunjang, assesmentPel, noSurat,
									kodeDPJP, dpjpLayan, noTelp, nik, functionHandler) {
        $.ajax({
            type: "POST",
            beforeSend: function (msg) {
                showLoadingPanel();
            },
            url: ResolveUrl('~/Libs/Service/BPJSService.asmx/GenerateNoSEP_MEDINFRASAPI'),
            data: "{ noKartu :'" + noKartu + "', tglSEP:'" + tglSEP + "', ppkPelayanan:'"
                    + ppkPelayanan + "', jnsPelayanan:'" + jnsPelayanan + "', klsRawatHak:'" + klsRawatHak
                    + "', klsRawatNaik:'" + klsRawatNaik + "', pembiayaan:'" + pembiayaan + "', penanggungJawab:'"
                    + penanggungJawab + "', medicalNo:'" + medicalNo + "', asalRujukan:'" + asalRujukan + "', tglRujukan:'"
                    + tglRujukan + "', noRujukan:'" + noRujukan + "', ppkRujukan:'" + ppkRujukan + "', catatan:'"
                    + catatan + "', diagnosa:'" + diagnosa + "', poliTujuan:'" + poliTujuan + "', poliEksekutif:'"
                    + poliEksekutif + "', cob:'" + cob + "', katarak:'" + katarak + "', lakaLantas:'" + lakaLantas
                    + "', keterangan:'" + keterangan + "', suplesi:'" + suplesi + "', noSepSuplesi:'" + noSepSuplesi
                    + "', kodePropinsi:'" + kodePropinsi + "', kodeKabupaten:'" + kodeKabupaten + "',kodeKecamatan:'"
                    + kodeKecamatan + "', tujuanKunj:'" + tujuanKunj + "', flagProcedure:'" + flagProcedure + "',kdPenunjang:'"
                    + kdPenunjang + "', assesmentPel:'" + assesmentPel + "', noSurat:'" + noSurat + "', kodeDPJP:'" + kodeDPJP
                    + "', dpjpLayan:'" + dpjpLayan + "', noTelp:'" + noTelp + "', nik:'" + nik + "'}",
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

    this.pengajuanSEP = function (noKartu, tglSep, jnsPelayanan, keterangan, functionHandler) {
        $.ajax({
            type: "POST",
            beforeSend: function (msg) {
                showLoadingPanel();
            },
            url: ResolveUrl('~/Libs/Service/BPJSService.asmx/PengajuanSEP'),
            data: "{ noKartu :'" + noKartu + "', tglSep:'" + tglSep + "', jnsPelayanan:'" + jnsPelayanan + "', keterangan:'" + keterangan + "'}",
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

    this.pengajuanSEPMedinfrasAPI = function (noKartu, tglSep, jnsPelayanan, keterangan, functionHandler) {
        $.ajax({
            type: "POST",
            beforeSend: function (msg) {
                showLoadingPanel();
            },
            url: ResolveUrl('~/Libs/Service/BPJSService.asmx/PengajuanSEP_MEDINFRASAPI'),
            data: "{ noKartu :'" + noKartu + "', tglSep:'" + tglSep + "', jnsPelayanan:'" + jnsPelayanan + "', keterangan:'" + keterangan + "'}",
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

    this.updateNoSEP = function (noSep, noKartu, tglSEP, tglRujukan, noRujukan, ppkRujukan, jnsPelayanan, catatan, diagAwal, poliTujuan, klsRawat, lakaLantas, lokasiLaka, noMR, asalRujukan, cob, poliEksekutif, mobilePhoneNo, penjamin, katarak, suplesi, noSepSuplesi, kodePropinsi, kodeKabupaten, kodeKecamatan, noSKDP, kodeDPJP, functionHandler) {
        $.ajax({
            type: "POST",
            beforeSend: function (msg) {
                showLoadingPanel();
            },
            url: ResolveUrl('~/Libs/Service/BPJSService.asmx/UpdateNoSEP'),
            data: "{ noSep:'" + noSep + "', noKartu :'" + noKartu + "', tglSEP:'" + tglSEP + "', tglRujukan:'" + tglRujukan + "', noRujukan:'" + noRujukan + "', ppkRujukan:'" + ppkRujukan + "', jnsPelayanan:'" + jnsPelayanan + "', catatan:'" + catatan + "', diagAwal:'" + diagAwal + "', poliTujuan:'" + poliTujuan + "', klsRawat:'" + klsRawat + "', lakaLantas:'" + lakaLantas + "', lokasiLaka:'" + lokasiLaka + "', noMR:'" + noMR + "', asalRujukan:'" + asalRujukan + "', cob:'" + cob + "', poliEksekutif:'" + poliEksekutif + "', mobilePhoneNo:'" + mobilePhoneNo + "', penjamin:'" + penjamin + "', katarak:'" +
            "', suplesi:'" + suplesi + "', noSepSuplesi:'" + noSepSuplesi + "', kodePropinsi:'" + kodePropinsi + "', kodeKabupaten:'" + kodeKabupaten + "', kodeKecamatan:'" + kodeKecamatan + "', noSKDP:'" + noSKDP + "', kodeDPJP:'" + kodeDPJP + "'}",
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

    this.updateNoSEPMedinfrasAPI = function (tglSEP, noSEP, klsRawatHak, klsRawatNaik, pembiayaan, penanggungJawab,
                            medicalNo, catatan, diagnosa, poliTujuan, poliEksekutif, cob, katarak, lakaLantas, keterangan, suplesi,
                            noSepSuplesi, kodePropinsi, kodeKabupaten, kodeKecamatan, dpjpLayan, noTelp, functionHandler) {
        $.ajax({
            type: "POST",
            beforeSend: function (msg) {
                showLoadingPanel();
            },
            url: ResolveUrl('~/Libs/Service/BPJSService.asmx/UpdateNoSEP_MEDINFRASAPI'),
            data: "{ tglSEP:'" + tglSEP + "', noSEP:'" + noSEP + "', klsRawatHak :'" + klsRawatHak + "', klsRawatNaik:'" + klsRawatNaik
                    + "', pembiayaan:'" + pembiayaan + "', penanggungJawab:'" + penanggungJawab + "', medicalNo:'"
                    + medicalNo + "', catatan:'" + catatan + "', diagnosa:'" + diagnosa + "', poliTujuan:'" + poliTujuan
                    + "', poliEksekutif:'" + poliEksekutif + "', cob:'" + cob + "', katarak:'" + katarak + "', lakaLantas:'"
                    + lakaLantas + "', keterangan:'" + keterangan + "', suplesi:'" + suplesi + "', noSepSuplesi:'" + noSepSuplesi
                    + "', kodePropinsi:'" + kodePropinsi + "', kodeKabupaten:'" + kodeKabupaten + "', kodeKecamatan:'" + kodeKecamatan
                    + "', dpjpLayan:'" + dpjpLayan + "', noTelp:'" + noTelp + "'}",
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

    this.deleteNoSEP = function (noSep, functionHandler) {
        $.ajax({
            type: "POST",
            beforeSend: function (msg) {
                showLoadingPanel();
            },
            url: ResolveUrl('~/Libs/Service/BPJSService.asmx/DeleteNoSEP'),
            data: "{ noSep:'" + noSep + "'}",
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
                alert('fail');
                alert(msg);
            }
        });
    };

    this.deleteNoSEPMedinfrasAPI = function (noSep, functionHandler) {
        $.ajax({
            type: "POST",
            beforeSend: function (msg) {
                showLoadingPanel();
            },
            url: ResolveUrl('~/Libs/Service/BPJSService.asmx/DeleteNoSEP_MEDINFRASAPI'),
            data: "{ noSep:'" + noSep + "'}",
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
                alert('fail');
                alert(msg);
            }
        });
    };

    this.getSEPInfo1 = function (noSEP, functionHandler) {
        showLoadingPanel();
        $.ajax({
            type: "POST",
            url: ResolveUrl('~/Libs/Service/BPJSService.asmx/FindSEPInfo'),
            data: "{ noSEP :'" + noSEP + "'}",
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

    this.getSEPInfo1MedinfrasAPI = function (noSEP, functionHandler) {
        showLoadingPanel();
        $.ajax({
            type: "POST",
            url: ResolveUrl('~/Libs/Service/BPJSService.asmx/FindSEPInfo_MEDINFRASAPI'),
            data: "{ noSEP :'" + noSEP + "'}",
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

    this.getSEPInfo2 = function (noSEP, functionHandler) {
        showLoadingPanel();
        $.ajax({
            type: "POST",
            url: ResolveUrl('~/Libs/Service/BPJSService.asmx/FindSEPInaCbgInfo'),
            data: "{ noSEP :'" + noSEP + "'}",
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

    this.mappingBPJS = function (noSep, noTrans, functionHandler) {
        $.ajax({
            type: "POST",
            beforeSend: function (msg) {
                showLoadingPanel();
            },
            url: ResolveUrl('~/Libs/Service/BPJSService.asmx/MappingBPJS'),
            data: "{ noSep:'" + noSep + "', noTrans :'" + noTrans + "'}",
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
                alert('fail');
                alert(msg);
            }
        });
    };

    this.updateTglPlg = function (noSEP, tglPulang, functionHandler) {
        $.ajax({
            type: "POST",
            beforeSend: function (msg) {
                showLoadingPanel();
            },
            url: ResolveUrl('~/Libs/Service/BPJSService.asmx/UpdateTglPlg'),
            data: "{ noSEP:'" + noSEP + "', tglPulang:'" + tglPulang + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (msg) {
                hideLoadingPanel();
                functionHandler(msg.d);
            },
            error: function (jqXHR, exception) {
                console.log(jqXHR.responseText);
                hideLoadingPanel();
                alert(jqXHR.responseText);
            },
            failure: function (msg) {
                hideLoadingPanel();
                alert(msg);
            }
        });
    };

    this.updateTglPlgAPI = function (noSEP, statusPulang, noSuratMeninggal, tglMeninggal, tglPulang, noLPManual, functionHandler) {
        $.ajax({
            type: "POST",
            beforeSend: function (msg) {
                showLoadingPanel();
            },
            url: ResolveUrl('~/Libs/Service/BPJSService.asmx/UpdateTglPlg_MEDINFRASAPI'),
            data: "{ noSEP:'" + noSEP + "', statusPulang:'" + statusPulang + "', noSuratMeninggal:'" + noSuratMeninggal + "', tglMeninggal:'" + tglMeninggal + "', tglPulang:'" + tglPulang + "', noLPManual:'" + noLPManual + "'}",
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

    this.getRujukanByNoRujukan = function (noRujukan, asalRujukan, functionHandler) {
        $.ajax({
            type: "POST",
            beforeSend: function (msg) {
                showLoadingPanel();
            },
            url: ResolveUrl('~/Libs/Service/BPJSService.asmx/GetRujukanByNoRujukan'),
            data: "{ noRujukan:'" + noRujukan + "', asalRujukan:'" + asalRujukan + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (msg) {
                hideLoadingPanel();
                functionHandler(msg.d);
            },
            error: function (msg) {
                hideLoadingPanel();
                alert(msg);
            },
            failure: function (msg) {
                hideLoadingPanel();
                alert(msg);
            }
        });
    };

    this.getRujukanByNoSEP = function (noSEP, functionHandler) {
        $.ajax({
            type: "POST",
            beforeSend: function (msg) {
                showLoadingPanel();
            },
            url: ResolveUrl('~/Libs/Service/BPJSService.asmx/GetRujukanByNoSEP'),
            data: "{ noSep:'" + noSep + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (msg) {
                hideLoadingPanel();
                functionHandler(msg.d);
            },
            error: function (msg) {
                hideLoadingPanel();
                alert(msg);
            },
            failure: function (msg) {
                hideLoadingPanel();
                alert('fail');
                alert(msg);
            }
        });
    };

    this.getHistoryKunjungan = function (noPeserta, functionHandler) {
        $.ajax({
            type: "POST",
            beforeSend: function (msg) {
                showLoadingPanel();
            },
            url: ResolveUrl('~/Libs/Service/BPJSService.asmx/GetHistoryKunjungan'),
            data: "{ NoPeserta:'" + noPeserta + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (msg) {
                hideLoadingPanel();
                functionHandler(msg.d);
            },
            error: function (msg) {
                hideLoadingPanel();
                alert(msg);
            },
            failure: function (msg) {
                hideLoadingPanel();
                alert('fail');
                alert(msg);
            }
        });
    };

    this.getHistoryPelayananPeserta = function (noPeserta, tglAwal, tglAkhir, functionHandler) {
        showLoadingPanel();
        $.ajax({
            type: "POST",
            url: ResolveUrl('~/Libs/Service/BPJSService.asmx/GetHistoryPelayananPeserta'),
            data: "{ noPeserta:'" + noPeserta + "', tglAwal :'" + tglAwal + "', tglAkhir : '" + tglAkhir + "'}",
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

    this.getBPJSReferralInformation = function (keyword, functionHandler) {
        $.ajax({
            type: "POST",
            beforeSend: function (msg) {
                showLoadingPanel();
            },
            url: ResolveUrl('~/Libs/Service/BPJSService.asmx/GetInformasiRujukan'),
            data: "{ Keyword:'" + keyword + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (msg) {
                hideLoadingPanel();
                functionHandler(msg.d);
            },
            error: function (msg) {
                hideLoadingPanel();
                alert(msg);
            },
            failure: function (msg) {
                hideLoadingPanel();
                alert('fail');
                alert(msg);
            }
        });
    };

    this.insertFaskes = function (kodeFaskes, namaFaskes, tipeFaskes, functionHandler) {
        $.ajax({
            type: "POST",
            beforeSend: function (msg) {
                showLoadingPanel();
            },
            url: ResolveUrl('~/Libs/Service/BPJSService.asmx/InsertFaskes'),
            data: "{ kodeFaskes:'" + kodeFaskes + "', namaFaskes :'" + namaFaskes + "', tipeFaskes : '" + tipeFaskes + "'}",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (msg) {
                hideLoadingPanel();
                functionHandler(msg.d);
            },
            error: function (msg) {
                hideLoadingPanel();
                alert(msg);
            },
            failure: function (msg) {
                hideLoadingPanel();
                alert('fail');
                alert(msg);
            }
        });
    };

    this.insertNoSPRIMedinfrasAPI = function (noKartu, kodeDokter, poliKontrol, tglRencanaKontrol, functionHandler) {
        $.ajax({
            type: "POST",
            beforeSend: function (msg) {
                showLoadingPanel();
            },
            url: ResolveUrl('~/Libs/Service/BPJSService.asmx/InsertNoSPRI_MEDINFRASAPI'),
            data: "{ noKartu :'" + noKartu + "', kodeDokter:'" + kodeDokter + "', poliKontrol:'"
                    + poliKontrol + "', tglRencanaKontrol:'" + tglRencanaKontrol + "'}",
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

    this.updateNoSPRIMedinfrasAPI = function (noSPRI, kodeDokter, poliKontrol, tglRencanaKontrol, functionHandler) {
        $.ajax({
            type: "POST",
            beforeSend: function (msg) {
                showLoadingPanel();
            },
            url: ResolveUrl('~/Libs/Service/BPJSService.asmx/UpdateNoSPRI_MEDINFRASAPI'),
            data: "{ noSPRI :'" + noSPRI + "', kodeDokter:'" + kodeDokter + "', poliKontrol:'"
                    + poliKontrol + "', tglRencanaKontrol:'" + tglRencanaKontrol + "'}",
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

    this.getRencanaKontrolByNoPesertaMedinfrasAPI = function (bulan, tahun, noKartu, filter, functionHandler) {
        $.ajax({
            type: "POST",
            beforeSend: function (msg) {
                showLoadingPanel();
            },
            url: ResolveUrl('~/Libs/Service/BPJSService.asmx/GetRencanaKontrolByNoPeserta_MEDINFRASAPI'),
            data: "{ bulan :'" + bulan + "', tahun:'" + tahun + "', noKartu:'" + noKartu + "', filter:'" + filter + "'}",
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

    this.insertRencanaKontrolMedinfrasAPI = function (noSEP, kodeDokter, poliKontrol, tglRencanaKontrol, functionHandler) {
        $.ajax({
            type: "POST",
            beforeSend: function (msg) {
                showLoadingPanel();
            },
            url: ResolveUrl('~/Libs/Service/BPJSService.asmx/InsertRencanaKontrol_MEDINFRASAPI'),
            data: "{ noSEP :'" + noSEP + "', kodeDokter:'" + kodeDokter + "', poliKontrol:'"
                    + poliKontrol + "', tglRencanaKontrol:'" + tglRencanaKontrol + "'}",
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

    this.updateRencanaKontrolMedinfrasAPI = function (noSuratKontrol, noSEP, kodeDokter, poliKontrol, tglRencanaKontrol, functionHandler) {
        $.ajax({
            type: "POST",
            beforeSend: function (msg) {
                showLoadingPanel();
            },
            url: ResolveUrl('~/Libs/Service/BPJSService.asmx/UpdateRencanaKontrol_MEDINFRASAPI'),
            data: "{ noSuratKontrol :'" + noSuratKontrol + "', noSEP :'" + noSEP + "', kodeDokter:'" + kodeDokter + "', poliKontrol:'"
                    + poliKontrol + "', tglRencanaKontrol:'" + tglRencanaKontrol + "'}",
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

    this.deleteRencanaKontrolMedinfrasAPI = function (noSuratKontrol, functionHandler) {
        $.ajax({
            type: "POST",
            beforeSend: function (msg) {
                showLoadingPanel();
            },
            url: ResolveUrl('~/Libs/Service/BPJSService.asmx/DeleteRencanaKontrol_MEDINFRASAPI'),
            data: "{ noSuratKontrol :'" + noSuratKontrol + "'}",
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

    this.insertRujukanMedinfrasAPI = function (noSEP, tglRujukan, tglRencanaKunjungan, ppkRujukan, jnsPelayanan, catatan, diagRujukan, tipeRujukan, poliRujukan, functionHandler) {
        $.ajax({
            type: "POST",
            beforeSend: function (msg) {
                showLoadingPanel();
            },
            url: ResolveUrl('~/Libs/Service/BPJSService.asmx/InsertRujukan_MEDINFRASAPI'),
            data: "{ noSEP :'" + noSEP + "', tglRujukan:'" + tglRujukan
            + "', tglRencanaKunjungan:'" + tglRencanaKunjungan + "', ppkRujukan:'" + ppkRujukan
            + "', jnsPelayanan:'" + jnsPelayanan + "', catatan:'" + catatan + "', diagRujukan:'" + diagRujukan + "', tipeRujukan:'" + tipeRujukan
            + "', poliRujukan:'" + poliRujukan + "'}",
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

    this.updateRujukanMedinfrasAPI = function (noRujukan, tglRujukan, tglRencanaKunjungan, ppkRujukan, jnsPelayanan, catatan, diagRujukan, tipeRujukan, poliRujukan, functionHandler) {
        $.ajax({
            type: "POST",
            beforeSend: function (msg) {
                showLoadingPanel();
            },
            url: ResolveUrl('~/Libs/Service/BPJSService.asmx/UpdateRujukan_MEDINFRASAPI'),
            data: "{ noRujukan :'" + noRujukan + "', tglRujukan:'" + tglRujukan
            + "', tglRencanaKunjungan:'" + tglRencanaKunjungan + "', ppkRujukan:'" + ppkRujukan
            + "', jnsPelayanan:'" + jnsPelayanan + "', catatan:'" + catatan + "', diagRujukan:'" + diagRujukan + "', tipeRujukan:'" + tipeRujukan
            + "', poliRujukan:'" + poliRujukan + "'}",
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

    this.getFingerPrintMedinfrasAPI = function (noPeserta, tglPelayanan, functionHandler) {
        $.ajax({
            type: "POST",
            beforeSend: function (msg) {
                showLoadingPanel();
            },
            url: ResolveUrl('~/Libs/Service/BPJSService.asmx/GetFingerPrint_MEDINFRASAPI'),
            data: "{ noPeserta :'" + noPeserta + "', tglPelayanan:'" + tglPelayanan + "'}",
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

    this.getFingerPrintListMedinfrasAPI = function (tglPelayanan, functionHandler) {
        $.ajax({
            type: "POST",
            beforeSend: function (msg) {
                showLoadingPanel();
            },
            url: ResolveUrl('~/Libs/Service/BPJSService.asmx/GetFingerPrintList_MEDINFRASAPI'),
            data: "{ tglPelayanan:'" + tglPelayanan + "'}",
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