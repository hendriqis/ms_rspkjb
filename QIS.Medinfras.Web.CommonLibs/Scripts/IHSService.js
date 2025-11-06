var IHSService = new (function () {

    this.getIHSNumberByNIK = function (NIK, functionHandler) {
        showLoadingPanel();
        $.ajax({
            type: "POST",
            url: ResolveUrl('~/Libs/Service/IHSService.asmx/GetPatientIHSNumberByNIK'),
            data: "{ NIK:'" + NIK + "'}",
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

    this.getParamedicIHSNumberByNIK = function (NIK, functionHandler) {
        showLoadingPanel();
        $.ajax({
            type: "POST",
            url: ResolveUrl('~/Libs/Service/IHSService.asmx/GetParamedicIHSNumberByNIK'),
            data: "{ NIK:'" + NIK + "'}",
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

    this.createIHSLocationID = function (serviceUnitCode, serviceUnitName, departmentID, functionHandler) {
        showLoadingPanel();
        $.ajax({
            type: "POST",
            url: ResolveUrl('~/Libs/Service/IHSService.asmx/GenerateServiceUnitIHSLocationID'),
            data: "{ serviceUnitCode:'" + serviceUnitCode + "', serviceUnitName:'" + serviceUnitName + "', departmentID:'" + departmentID + "'}",
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

    this.createIHSOrganizationID = function (departmentID, departmentName, functionHandler) {
        showLoadingPanel();
        $.ajax({
            type: "POST",
            url: ResolveUrl('~/Libs/Service/IHSService.asmx/GenerateOrganizationID'),
            data: "{ departmentID:'" + departmentID + "', departmentName:'" + departmentName + "'}",
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

    this.sendObservationIHS = function (ID, registrationID, functionHandler) {
        showLoadingPanel();
        $.ajax({
            type: "POST",
            url: ResolveUrl('~/Libs/Service/IHSService.asmx/SendObservationIHS'),
            data: "{ ID:'" + ID + "', registrationID:'" + registrationID + "'}",
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