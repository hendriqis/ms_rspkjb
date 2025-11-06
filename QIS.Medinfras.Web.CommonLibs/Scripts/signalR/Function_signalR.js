$(function () {
    window.hubReady = $.connection.hub.start();

    var shbc = $.connection.BroadcastMessage;
    shbc.client.messageRecieved = function (message) {
        alert("ok");
        notifyOK(message); //ada di MPMain dan MPMainDashboard
    };
 

});
 

function SendNotification(jsonSendNotification) {
    $.connection.hub.start().done(function () {
        var bc = $.connection.BroadcastMessage;
        var message = jsonSendNotification;

        bc.server.send(message);
        ShowSnackbarSuccess("Message Successfully Sent");
    });
}

 