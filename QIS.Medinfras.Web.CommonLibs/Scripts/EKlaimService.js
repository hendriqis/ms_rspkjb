var EKlaimService = new (function () {
    this.searchProceduresReinsertBPJSReference = function (procedureSearch, functionHandler) {
        $.ajax({
            type: "POST",
            beforeSend: function (msg) {
                showLoadingPanel();
            },
            url: ResolveUrl('~/Libs/Service/EKlaimService.asmx/SearchProceduresReinsertBPJSReference'),
            data: "{ keyword :'" + procedureSearch + "'}",
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
    this.newClaim = function (nomor_kartu, nomor_sep, nomor_rm, nama_pasien, tgl_lahir, gender, functionHandler) {
        $.ajax({
            type: "POST",
            beforeSend: function (msg) {
                showLoadingPanel();
            },
            url: ResolveUrl('~/Libs/Service/EKlaimService.asmx/NewClaim'),
            data: "{ nomor_kartu :'" + nomor_kartu + "', nomor_sep:'" + nomor_sep + "', nomor_rm:'" + nomor_rm + "', nama_pasien:'" + nama_pasien + "', tgl_lahir:'" + tgl_lahir + "', gender:'" + gender + "'}",
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

    this.updatePatient = function (nomor_kartu, nomor_rm, nama_pasien, tgl_lahir, gender, functionHandler) {
        $.ajax({
            type: "POST",
            beforeSend: function (msg) {
                showLoadingPanel();
            },
            url: ResolveUrl('~/Libs/Service/EKlaimService.asmx/UpdatePatient'),
            data: "{ nomor_kartu :'" + nomor_kartu + "', nomor_rm:'" + nomor_rm + "', nama_pasien:'" + nama_pasien + "', tgl_lahir:'" + tgl_lahir + "', gender:'" + gender + "'}",
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

    this.deletePatient = function (nomor_rm, coder_nik, functionHandler) {
        $.ajax({
            type: "POST",
            beforeSend: function (msg) {
                showLoadingPanel();
            },
            url: ResolveUrl('~/Libs/Service/EKlaimService.asmx/DeletePatient'),
            data: "{ nomor_rm:'" + nomor_rm + "', coder_nik:'" + coder_nik + "'}",
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

    this.setClaimData = function (
                    nomor_sep 
                    ,nomor_kartu 
                    ,tgl_masuk 
                    ,tgl_pulang 
                    ,cara_masuk 
                    ,jenis_rawat
                    ,kelas_rawat
                    ,adl_sub_acute 
                    ,adl_chronic 
                    ,icu_indikator 
                    ,icu_los 
                    ,ventilator_hour 
                    ,use_ind
                    ,start_dttm
                    ,stop_dttm
                    ,upgrade_class_ind 
                    ,upgrade_class_class 
                    ,upgrade_class_los 
                    ,upgrade_class_payor 
                    ,add_payment_pct 
                    ,birth_weight 
                    ,sistole 
                    ,diastole 
                    ,discharge_status 
                    ,diagnosa 
                    ,procedure 
                    ,diagnosa_inagrouper 
                    ,procedure_inagrouper
                    ,prosedur_non_bedah 
                    ,prosedur_bedah 
                    ,konsultasi 
                    ,tenaga_ahli 
                    ,keperawatan 
                    ,penunjang 
                    ,radiologi 
                    ,laboratorium 
                    ,pelayanan_darah 
                    ,rehabilitasi 
                    ,kamar 
                    ,rawat_intensif 
                    ,obat 
                    ,obat_kronis 
                    ,obat_kemoterapi 
                    ,alkes 
                    ,bmhp 
                    ,sewa_alat 
                    ,pemulasaraan_jenazah 
                    ,kantong_jenazah 
                    ,peti_jenazah 
                    ,plastik_erat 
                    ,desinfektan_jenazah 
                    ,mobil_jenazah 
                    ,desinfektan_mobil_jenazah 
                    ,covid19_status_cd 
                    ,nomor_kartu_t 
                    ,episodes 
                    ,covid19_cc_ind 
                    ,covid19_rs_darurat_ind 
                    ,covid19_co_insidense_ind 
                    ,lab_asam_laktat 
                    ,lab_procalcitonin 
                    ,lab_crp 
                    ,lab_kultur 
                    ,lab_d_dimer 
                    ,lab_pt 
                    ,lab_aptt 
                    ,lab_waktu_pendarahan 
                    ,lab_anti_hiv 
                    ,lab_albumin 
                    ,lab_analisa_gas 
                    ,rad_thorax_ap_pa 
                    ,terapi_konvalesen 
                    ,akses_naat 
                    ,isoman_ind 
                    ,bayi_lahir_status_cd 
                    ,dializer_single_use 
                    ,kantong_darah 
                    ,apgar_menit1_appearance 
                    ,apgar_menit1_pulse 
                    ,apgar_menit1_grimace 
                    ,apgar_menit1_activity 
                    ,apgar_menit1_respiration 
                    ,apgar_menit5_appearance 
                    ,apgar_menit5_pulse 
                    ,apgar_menit5_grimace 
                    ,apgar_menit5_activity 
                    ,apgar_menit5_respiration 
                    ,usia_kehamilan 
                    ,gravida 
                    ,partus 
                    ,abortus 
                    ,onset_kontraksi 
                    ,delivery_sequence 
                    ,delivery_method 
                    ,delivery_dttm 
                    ,letak_janin 
                    ,kondisi 
                    ,use_manual 
                    ,use_forcep 
                    ,use_vacuum 
                    ,tarif_poli_eks 
                    ,nama_dokter 
                    ,kode_tarif 
                    ,payor_id 
                    ,payor_cd 
                    ,cob_cd 
                    ,coder_nik
                ) {
 
       
        $.ajax(
        {
            type: "POST",
            beforeSend: function (msg) {
                showLoadingPanel();
            },
            url: ResolveUrl('~/Libs/Service/EKlaimService.asmx/SetClaimData'),
            data: "{ nomor_sep : '" +  nomor_sep
                        + "',nomor_kartu : '" + nomor_kartu
                        + "',tgl_masuk : '" + tgl_masuk
                        + "',tgl_pulang : '" + tgl_pulang
                        + "',cara_masuk : '" + cara_masuk
                        + "',jenis_rawat : '" + jenis_rawat
                        + "',kelas_rawat : '" + kelas_rawat
                        + "',adl_sub_acute : '" + adl_sub_acute
                        + "',adl_chronic : '" + adl_chronic
                        + "',icu_indikator : '" + icu_indikator
                        + "',icu_los : '" + icu_los
                        + "',ventilator_hour : '" + ventilator_hour
                        + "',use_ind : '" + use_ind
                        + "',start_dttm : '" + start_dttm
                        + "',stop_dttm: '" + stop_dttm
                        + "',upgrade_class_ind : '" + upgrade_class_ind
                        + "',upgrade_class_class : '" + upgrade_class_class
                        + "',upgrade_class_los : '" + upgrade_class_los
                        + "',upgrade_class_payor : '" + upgrade_class_payor
                        + "',add_payment_pct : '" + add_payment_pct
                        + "',birth_weight : '" + birth_weight
                        + "',sistole : '" + sistole
                        + "',diastole : '" + diastole
                        + "',discharge_status : '" + discharge_status
                        + "',diagnosa : '" + diagnosa
                        + "',procedure : '" + procedure
                        + "',diagnosa_inagrouper : '" + diagnosa_inagrouper
                        + "',procedure_inagrouper : '" + procedure_inagrouper
                        + "',prosedur_non_bedah : '" + prosedur_non_bedah
                        + "',prosedur_bedah : '" + prosedur_bedah
                        + "',konsultasi : '" + konsultasi
                        + "',tenaga_ahli : '" + tenaga_ahli
                        + "',keperawatan : '" + keperawatan
                        + "',penunjang : '" + penunjang
                        + "',radiologi : '" + radiologi
                        + "',laboratorium : '" + laboratorium
                        + "',pelayanan_darah : '" + pelayanan_darah
                        + "',rehabilitasi : '" + rehabilitasi
                        + "',kamar : '" + kamar
                        + "',rawat_intensif : '" + rawat_intensif
                        + "',obat : '" + obat
                        + "',obat_kronis : '" + obat_kronis
                        + "',obat_kemoterapi : '" + obat_kemoterapi
                        + "',alkes : '" + alkes
                        + "',bmhp : '" + bmhp
                        + "',sewa_alat : '" + sewa_alat
                        + "',pemulasaraan_jenazah : '" + pemulasaraan_jenazah
                        + "',kantong_jenazah : '" + kantong_jenazah
                        + "',peti_jenazah : '" + peti_jenazah
                        + "',plastik_erat : '" + plastik_erat
                        + "',desinfektan_jenazah : '" + desinfektan_jenazah
                        + "',mobil_jenazah : '" + mobil_jenazah
                        + "',desinfektan_mobil_jenazah : '" + desinfektan_mobil_jenazah
                        + "',covid19_status_cd : '" + covid19_status_cd
                        + "',nomor_kartu_t : '" + nomor_kartu_t
                        + "',episodes : '" + episodes
                        + "',covid19_cc_ind : '" + covid19_cc_ind
                        + "',covid19_rs_darurat_ind : '" + covid19_rs_darurat_ind
                        + "',covid19_co_insidense_ind : '" + covid19_co_insidense_ind
                        + "',lab_asam_laktat : '" + lab_asam_laktat
                        + "',lab_procalcitonin : '" + lab_procalcitonin
                        + "',lab_crp : '" + lab_crp
                        + "',lab_kultur : '" + lab_kultur
                        + "',lab_d_dimer : '" + lab_d_dimer
                        + "',lab_pt : '" + lab_pt
                        + "',lab_aptt : '" + lab_aptt
                        + "',lab_waktu_pendarahan : '" + lab_waktu_pendarahan
                        + "',lab_anti_hiv : '" + lab_anti_hiv
                        + "',lab_albumin : '" + lab_albumin
                        + "',lab_analisa_gas : '" + lab_analisa_gas
                        + "',rad_thorax_ap_pa : '" + rad_thorax_ap_pa
                        + "',terapi_konvalesen : '" + terapi_konvalesen
                        + "',akses_naat : '" + akses_naat
                        + "',isoman_ind : '" + isoman_ind
                        + "',bayi_lahir_status_cd : '" + bayi_lahir_status_cd
                        + "',dializer_single_use : '" + dializer_single_use
                        + "',kantong_darah : '" + kantong_darah
                        + "',apgar_menit1_appearance : '" + apgar_menit1_appearance
                        + "',apgar_menit1_pulse : '" + apgar_menit1_pulse
                        + "',apgar_menit1_grimace : '" + apgar_menit1_grimace
                        + "',apgar_menit1_activity : '" + apgar_menit1_activity
                        + "',apgar_menit1_respiration : '" + apgar_menit1_respiration
                        + "',apgar_menit5_appearance : '" + apgar_menit5_appearance
                        + "',apgar_menit5_pulse : '" + apgar_menit5_pulse
                        + "',apgar_menit5_grimace : '" + apgar_menit5_grimace
                        + "',apgar_menit5_activity : '" + apgar_menit5_activity
                        + "',apgar_menit5_respiration : '" + apgar_menit5_respiration
                        + "',usia_kehamilan : '" + usia_kehamilan
                        + "',gravida : '" + gravida
                        + "',partus : '" + partus
                        + "',abortus : '" + abortus
                        + "',onset_kontraksi : '" + onset_kontraksi
                        + "',delivery_sequence : '" + delivery_sequence
                        + "',delivery_method : '" + delivery_method
                        + "',delivery_dttm : '" + delivery_dttm
                        + "',letak_janin : '" + letak_janin
                        + "',kondisi : '" + kondisi
                        + "',use_manual : '" + use_manual
                        + "',use_forcep : '" + use_forcep
                        + "',use_vacuum : '" + use_vacuum
                        + "',tarif_poli_eks : '" + tarif_poli_eks
                        + "',nama_dokter : '" + nama_dokter
                        + "',kode_tarif : '" + kode_tarif
                        + "',payor_id : '" + payor_id
                        + "',payor_cd : '" + payor_cd
                        + "',cob_cd : '" + cob_cd
                        + "',coder_nik : '" + coder_nik 
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

    this.groupingStage1 = function (nomor_sep, functionHandler) {
        $.ajax({
            type: "POST",
            beforeSend: function (msg) {
                showLoadingPanel();
            },
            url: ResolveUrl('~/Libs/Service/EKlaimService.asmx/GroupingStage1'),
            data: "{ nomor_sep:'" + nomor_sep + "'}",
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

    this.groupingStage2 = function (nomor_sep, special_cmg, functionHandler) {
        $.ajax({
            type: "POST",
            beforeSend: function (msg) {
                showLoadingPanel();
            },
            url: ResolveUrl('~/Libs/Service/EKlaimService.asmx/GroupingStage2'),
            data: "{ nomor_sep:'" + nomor_sep + "', special_cmg:'" + special_cmg + "'}",
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

    this.claimFinal = function (nomor_sep, coder_nik, functionHandler) {
        $.ajax({
            type: "POST",
            beforeSend: function (msg) {
                showLoadingPanel();
            },
            url: ResolveUrl('~/Libs/Service/EKlaimService.asmx/ClaimFinal'),
            data: "{ nomor_sep:'" + nomor_sep + "', coder_nik:'" + coder_nik + "'}",
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

    this.reeditClaim = function (nomor_sep, functionHandler) {
        $.ajax({
            type: "POST",
            beforeSend: function (msg) {
                showLoadingPanel();
            },
            url: ResolveUrl('~/Libs/Service/EKlaimService.asmx/ReeditClaim'),
            data: "{ nomor_sep:'" + nomor_sep + "'}",
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

    this.sendClaim = function (start_dt, stop_dt, jenis_rawat, date_type, functionHandler) {
        $.ajax({
            type: "POST",
            beforeSend: function (msg) {
                showLoadingPanel();
            },
            url: ResolveUrl('~/Libs/Service/EKlaimService.asmx/SendClaim'),
            data: "{ start_dt:'" + start_dt + "', stop_dt:'" + stop_dt + "', jenis_rawat:'" + jenis_rawat + "', date_type:'" + date_type + "'}",
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

    this.sendClaimIndividual = function (nomor_sep, functionHandler) {
        $.ajax({
            type: "POST",
            beforeSend: function (msg) {
                showLoadingPanel();
            },
            url: ResolveUrl('~/Libs/Service/EKlaimService.asmx/SendClaimIndividual'),
            data: "{ nomor_sep:'" + nomor_sep + "'}",
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

    this.pullClaim = function (start_dt, stop_dt, jenis_rawat, functionHandler) {
        $.ajax({
            type: "POST",
            beforeSend: function (msg) {
                showLoadingPanel();
            },
            url: ResolveUrl('~/Libs/Service/EKlaimService.asmx/PullClaim'),
            data: "{ start_dt:'" + start_dt + "', stop_dt:'" + stop_dt + "', jenis_rawat:'" + jenis_rawat + "'}",
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

    this.getClaim = function (nomor_sep, functionHandler) {
        $.ajax({
            type: "POST",
            beforeSend: function (msg) {
                showLoadingPanel();
            },
            url: ResolveUrl('~/Libs/Service/EKlaimService.asmx/GetClaim'),
            data: "{ nomor_sep:'" + nomor_sep + "'}",
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

    this.getClaimStatus = function (nomor_sep, functionHandler) {
        $.ajax({
            type: "POST",
            beforeSend: function (msg) {
                showLoadingPanel();
            },
            url: ResolveUrl('~/Libs/Service/EKlaimService.asmx/GetClaimStatus'),
            data: "{ nomor_sep:'" + nomor_sep + "'}",
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

    this.deleteClaim = function (nomor_sep, coder_nik, functionHandler) {
        $.ajax({
            type: "POST",
            beforeSend: function (msg) {
                showLoadingPanel();
            },
            url: ResolveUrl('~/Libs/Service/EKlaimService.asmx/DeleteClaim'),
            data: "{ nomor_sep:'" + nomor_sep + "', coder_nik:'" + coder_nik + "'}",
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

    this.claimPrint = function (nomor_sep, functionHandler) {
        $.ajax({
            type: "POST",
            beforeSend: function (msg) {
                showLoadingPanel();
            },
            url: ResolveUrl('~/Libs/Service/EKlaimService.asmx/ClaimPrint'),
            data: "{ nomor_sep:'" + nomor_sep + "'}",
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

    this.searchDiagnosis = function (keyword, functionHandler) {
        $.ajax({
            type: "POST",
            beforeSend: function (msg) {
                showLoadingPanel();
            },
            url: ResolveUrl('~/Libs/Service/EKlaimService.asmx/SearchDiagnosis'),
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

    this.searchDiagnosisReinsertBPJSReference = function (keyword, functionHandler) {
        $.ajax({
            type: "POST",
            beforeSend: function (msg) {
                showLoadingPanel();
            },
            url: ResolveUrl('~/Libs/Service/EKlaimService.asmx/SearchDiagnosisReinsertBPJSReference'),
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

    this.SearchDiagnosisINAGrouper = function (keyword, functionHandler) {
        $.ajax({
            type: "POST",
            beforeSend: function (msg) {
                showLoadingPanel();
            },
            url: ResolveUrl('~/Libs/Service/EKlaimService.asmx/SearchDiagnosisINAGrouper'),
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

    this.searchDiagnosisINAGrouperReinsertBPJSReference = function (keyword, functionHandler) {
        $.ajax({
            type: "POST",
            beforeSend: function (msg) {
                showLoadingPanel();
            },
            url: ResolveUrl('~/Libs/Service/EKlaimService.asmx/SearchDiagnosisINAGrouperReinsertBPJSReference'),
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

    this.SearchProceduresINAGrouper = function (keyword, functionHandler) {
        $.ajax({
            type: "POST",
            beforeSend: function (msg) {
                showLoadingPanel();
            },
            url: ResolveUrl('~/Libs/Service/EKlaimService.asmx/SearchProceduresINAGrouper'),
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

    this.searchProceduresINAGrouperReinsertBPJSReference = function (keyword, functionHandler) {
        $.ajax({
            type: "POST",
            beforeSend: function (msg) {
                showLoadingPanel();
            },
            url: ResolveUrl('~/Libs/Service/EKlaimService.asmx/SearchProceduresINAGrouperReinsertBPJSReference'),
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

})();