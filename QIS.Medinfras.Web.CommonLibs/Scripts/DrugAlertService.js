var DrugAlertService = new (function () {

    this.validateDrugs = function (prescriptionOrderNo, checkType, functionHandler) {
        $.ajax({
            type: "POST",
            beforeSend: function (msg) {
                showLoadingPanel();
            },
            url: ResolveUrl('~/Libs/Service/DrugAlertService.asmx/GetDrugAlertValidation'),
            data: "{ prescriptionOrderNo :'" + prescriptionOrderNo + "', checkType:'" + checkType + "' }",
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
})();