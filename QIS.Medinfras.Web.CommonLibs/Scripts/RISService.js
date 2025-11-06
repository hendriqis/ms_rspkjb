var RISService = new (function () {
    this.SendOrderToRIS = function (testOrderID, functionHandler) {
        $.ajax({
            type: "POST",
            beforeSend: function (msg) {
                showLoadingPanel();
            },
            url: ResolveUrl('~/Libs/Service/RISService.asmx/SendOrderToRIS'),
            data: "{ testOrderID :" + testOrderID + "}",
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