var MedinfrasAPI = new (function () {
    this.getReportTools = function (reportCode, parameterValue, functionHandler) {
       
        showLoadingPanel();
        var URL = ResolveUrl('~/Libs/Service/MedinfrasAPIService.asmx/GetReportTools');
        $.ajax({
            // have to use synchronous here, else returns before data is fetched
            async: false,
            type: 'POST',
            url: ResolveUrl('~/Libs/Service/MedinfrasAPIService.asmx/GetReportTools'),
            contentType: 'application/json; charset=utf-8',
            data: '{ "reportCode" : "' + reportCode + '", "parameterValue" : "' + parameterValue + '"}',
            dataType: 'json',
            error: function (msg) {
                alert(msg.responseText);
            },
            success: function (msg) {
                functionHandler(msg.d);
            }
        });     //end ajax
    };

    this.getGeneralConsentPrint = function (registrationID, functionHandler) {

       /// showLoadingPanel();
         $.ajax({
            // have to use synchronous here, else returns before data is fetched
            async: false,
            type: 'POST',
            url: ResolveUrl('~/Libs/Service/MedinfrasAPIService.asmx/GetGeneralConsentPrint'),
            contentType: 'application/json; charset=utf-8',
            data: '{ "RegistrationID" : ' + registrationID + '}',
            dataType: 'json',
            error: function (msg) {
                alert(msg.responseText);
            },
            success: function (msg) {
                functionHandler(msg.d);
            }
        });     //end ajax
    };
    this.getTemplateDocumentPrint = function (reportCode, paramValue, functionHandler) {

        /// showLoadingPanel();
        $.ajax({
            // have to use synchronous here, else returns before data is fetched
            async: false,
            type: 'POST',
            url: ResolveUrl('~/Libs/Service/MedinfrasAPIService.asmx/GetTemplateDocumentPrint'),
            contentType: 'application/json; charset=utf-8',
            data: '{"ReportCode" : "' + reportCode + '", "ParamValue" : "' + paramValue + '" }',
            dataType: 'json',
            error: function (msg) {
                alert(msg.responseText);
            },
            success: function (msg) {
                functionHandler(msg.d);
            }
        });     //end ajax
    };


})();