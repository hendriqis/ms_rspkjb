var EDCService = new (function () {
    this.SendService = function (Header, VisitInfo, PatienInfo, PaymentCardInfo, TransactionDateFormat, TransactionAmount, RegistrationID, IpToEDC, functionHandler) {
        showLoadingPanel();
        $.ajax({
            type: "POST",
            url: ResolveUrl('~/Libs/Service/EDCService.asmx/CallServiceEdc'),
            data: "{Header:'" + Header + "',VisitInfo:'" + VisitInfo + "',PatienInfo:'" + PatienInfo +  "',PaymentCardInfo:'" + PaymentCardInfo +"',TransactionDateFormat:'" + TransactionDateFormat + "',TransactionAmount:'" + TransactionAmount + "',RegistrationID:'" + RegistrationID + "',IpToEDC:'" + IpToEDC + "' }",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (msg) {
                hideLoadingPanel();
                //alert(msg);
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
    this.SendVoidService = function (Header, VisitInfo, PatienInfo,PaymentCardInfo, TransactionDateFormat, TransactionAmount, RegistrationID, IpToEDC, functionHandler) {
        showLoadingPanel();
        $.ajax({
            type: "POST",
            url: ResolveUrl('~/Libs/Service/EDCService.asmx/CallVoidServiceEDC'),
            data: "{Header:'" + Header + "',VisitInfo:'" + VisitInfo + "',PatienInfo:'" + PatienInfo + "',PaymentCardInfo:'" + PaymentCardInfo +"',TransactionDateFormat:'" + TransactionDateFormat + "',TransactionAmount:'" + TransactionAmount + "',RegistrationID:'" + RegistrationID + "',IpToEDC:'" + IpToEDC + "' }",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (msg) {
                hideLoadingPanel();
                //alert(msg);
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
    this.InsertRequestID = function (functionHandler) {
        showLoadingPanel();
        $.ajax({
            type: "POST",
            url: ResolveUrl('~/Libs/Service/EDCService.asmx/InsertNewRequest'),
            data: "",
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            success: function (msg) {
                hideLoadingPanel();
                //alert(msg);
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