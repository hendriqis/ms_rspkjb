var AplicaresService = new (function () {
    this.createRoom = function (kodeKelas, kodeRuang, namaRuang, jumlahKapasitas, jumlahKosong, jumlahKosongPria, jumlahKosongWanita, jumlahKosongPriaWanita, functionHandler) {
        $.ajax({
            type: "POST",
            beforeSend: function (msg) {
                showLoadingPanel();
            },
            url: ResolveUrl('~/Libs/Service/AplicaresService.asmx/CreateRoom'),
            data: "{ kodeKelas :'" + kodeKelas + "', kodeRuang:'" + kodeRuang + "', namaRuang:'" + namaRuang + "', jumlahKapasitas:'" + jumlahKapasitas + "', jumlahKosong:'" + jumlahKosong + "', jumlahKosongPria:'" + jumlahKosongPria + "', jumlahKosongWanita:'" + jumlahKosongWanita + "', jumlahKosongPriaWanita:'" + jumlahKosongPriaWanita + "'}",
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

    this.createRoom_MedinfrasAPI = function (kodeKelas, kodeRuang, namaRuang, jumlahKapasitas, jumlahKosong, jumlahKosongPria, jumlahKosongWanita, jumlahKosongPriaWanita, functionHandler) {
        $.ajax({
            type: "POST",
            beforeSend: function (msg) {
                showLoadingPanel();
            },
            url: ResolveUrl('~/Libs/Service/AplicaresService.asmx/CreateRoom'),
            data: "{ kodeKelas :'" + kodeKelas + "', kodeRuang:'" + kodeRuang + "', namaRuang:'" + namaRuang + "', jumlahKapasitas:'" + jumlahKapasitas + "', jumlahKosong:'" + jumlahKosong + "', jumlahKosongPria:'" + jumlahKosongPria + "', jumlahKosongWanita:'" + jumlahKosongWanita + "', jumlahKosongPriaWanita:'" + jumlahKosongPriaWanita + "'}",
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

    this.updateRoomStatus = function (kodeKelas, kodeRuang, namaRuang, jumlahKapasitas, jumlahKosong, jumlahKosongPria, jumlahKosongWanita, jumlahKosongPriaWanita, functionHandler) {
        $.ajax({
            type: "POST",
            beforeSend: function (msg) {
                showLoadingPanel();
            },
            url: ResolveUrl('~/Libs/Service/AplicaresService.asmx/UpdateRoomStatus'),
            data: "{ kodeKelas :'" + kodeKelas + "', kodeRuang:'" + kodeRuang + "', namaRuang:'" + namaRuang + "', jumlahKapasitas:'" + jumlahKapasitas + "', jumlahKosong:'" + jumlahKosong + "', jumlahKosongPria:'" + jumlahKosongPria + "', jumlahKosongWanita:'" + jumlahKosongWanita + "', jumlahKosongPriaWanita:'" + jumlahKosongPriaWanita + "'}",
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

    this.updateRoomStatus_MedinfrasAPI = function (kodeKelas, kodeRuang, namaRuang, jumlahKapasitas, jumlahKosong, jumlahKosongPria, jumlahKosongWanita, jumlahKosongPriaWanita, functionHandler) {
        $.ajax({
            type: "POST",
            beforeSend: function (msg) {
                showLoadingPanel();
            },
            url: ResolveUrl('~/Libs/Service/AplicaresService.asmx/UpdateRoomStatus'),
            data: "{ kodeKelas :'" + kodeKelas + "', kodeRuang:'" + kodeRuang + "', namaRuang:'" + namaRuang + "', jumlahKapasitas:'" + jumlahKapasitas + "', jumlahKosong:'" + jumlahKosong + "', jumlahKosongPria:'" + jumlahKosongPria + "', jumlahKosongWanita:'" + jumlahKosongWanita + "', jumlahKosongPriaWanita:'" + jumlahKosongPriaWanita + "'}",
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

    this.deleteClassRoom = function (kodeKelas, kodeRuang, functionHandler) {
        $.ajax({
            type: "POST",
            beforeSend: function (msg) {
                showLoadingPanel();
            },
            url: ResolveUrl('~/Libs/Service/AplicaresService.asmx/DeleteClassRoom'),
            data: "{ kodeKelas :'" + kodeKelas + "', kodeRuang:'" + kodeRuang + "'}",
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

    this.deleteClassRoom_MedinfrasAPI = function (kodeKelas, kodeRuang, functionHandler) {
        $.ajax({
            type: "POST",
            beforeSend: function (msg) {
                showLoadingPanel();
            },
            url: ResolveUrl('~/Libs/Service/AplicaresService.asmx/DeleteClassRoom'),
            data: "{ kodeKelas :'" + kodeKelas + "', kodeRuang:'" + kodeRuang + "'}",
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

    this.updateStatusSendToAplicares = function (HealthcareServiceUnitID, ClassID, functionHandler) {
        $.ajax({
            type: "POST",
            beforeSend: function (msg) {
                showLoadingPanel();
            },
            url: ResolveUrl('~/Libs/Service/AplicaresService.asmx/UpdateStatusSendToAplicares'),
            data: "{ HealthcareServiceUnitID:'" + HealthcareServiceUnitID + "', ClassID:'" + ClassID + "'}",
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

    this.updateStatusDeleteFromAplicares = function (HealthcareServiceUnitID, ClassID, functionHandler) {
        $.ajax({
            type: "POST",
            beforeSend: function (msg) {
                showLoadingPanel();
            },
            url: ResolveUrl('~/Libs/Service/AplicaresService.asmx/UpdateStatusDeleteFromAplicares'),
            data: "{ HealthcareServiceUnitID:'" + HealthcareServiceUnitID + "', ClassID:'" + ClassID + "'}",
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
})();