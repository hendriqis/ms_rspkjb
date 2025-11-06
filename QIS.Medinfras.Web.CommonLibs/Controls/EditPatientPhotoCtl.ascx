<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EditPatientPhotoCtl.ascx.cs" 
    Inherits="QIS.Medinfras.Web.CommonLibs.Controls.EditPatientPhotoCtl" %>

<script id="dxis_Webcamctl1" src='<%= ResolveUrl("~/Libs/Scripts/JqueryWebcam/webcam.js")%>' type='text/javascript'></script>
<script id="dxss_webcamctl" type="text/javascript">
    $(function () {
        Webcam.set({
            width: 320,
            height: 240,
            image_format: 'jpeg',
            jpeg_quality: 90
        });
        Webcam.attach('#fc');
        $("#imgWrapper").css({ height: "255px",
            width: "240px",
            border: "solid 1px #aaa"
        });

    });
//    $(function () {
//        if (webcam != null) {
//            webcam.set_swf_url('<%= ResolveUrl("~/Libs/Scripts/Webcam/webcam.swf")%>');
//            webcam.set_api_url(document.URL);
//            webcam.set_quality(90); // JPEG quality (1 - 100)
//            webcam.set_shutter_sound(false);


//            $("#imgWrapper").css({ height: "288px",
//                width: "240px",
//                border: "solid 1px #aaa"
//            });

//            $("#fc").html(webcam.get_html(240, 288, 240, 288));
//        }
//    });

    function camReset() {
       // webcam.reset();
        //setCamInstruction("Adjust, snap, then upload", "#666");
        Webcam.resetv1();
        setCamInstruction("Adjust, snap, then upload", "#666");
    }

    function setCamInstruction(msg, c) {
        //$("#upStatus").html(msg).css("color", c);
    }

    $('#imgEditPatientPhotoCapture').click(function () {
        $('#fc').show();
        $('#imgBrowseResult').hide();
        Webcam.snap(function (data_uri) {
            $('#imgBrowseResult').attr('src', data_uri);
            $('#fc').hide();
            $('#imgBrowseResult').show();
        });
       // Webcam.freeze();
    });

    $('#imgEditPatientPhotoClear').click(function () {
        $('#fc').show();
        $('#imgBrowseResult').hide();
        camReset();
    });

    $('#imgEditPatientPhotoSave').click(function () {
        showLoadingPanel();
        if ($("#fc").is(":visible")) {
            var make = $("#make").val();
            var gi = $("#ghimg");
            gi.css("visibility", "visible");
            var MRN = parseInt('<%=MRN %>');
            //            Webcam.upload(ResolveUrl("~/Libs/Service/UploadPatientPhoto.aspx?id=" + MRN), function () {
            //                setTimeout(function () {
            //                    gi.css("visibility", "hidden");
            //                    onAfterSavePatientPhoto();
            //                    pcRightPanelContent.Hide();
            //                }, 0);
            //            });
            Webcam.snap(function (data_uri) {
                var image = data_uri;
                image = image.replace('data:image/png;base64,', '');
                image = image.replace('data:image/jpeg;base64,', '');
                image = image.replace('data:image/gif;base64,', '');
                var isUsingMRN = parseInt('<%=isUsingMRN %>');
                if (isUsingMRN == 1) {
                    var url = '<%= ResolveUrl("~/Libs/Service/UploadService.asmx/UploadPatientPhoto")%>';
                    $.ajax({
                        async: false,
                        type: 'POST',
                        url: url,
                        data: '{ "imageData" : "' + image + '", "MRN" : "' + MRN + '"}',
                        contentType: 'application/json; charset=utf-8',
                        dataType: 'json',
                        error: function (msg) {
                            showToast('Failed', msg.responseText);
                            hideLoadingPanel();
                        },
                        success: function (msg) {
                            success = msg;
                            if (!success)
                                showToast('Failed!', '');
                            else {
                                onAfterSavePatientPhoto();
                                pcRightPanelContent.Hide();
                            }
                            hideLoadingPanel();

                        }
                    });
                }
                else {
                    var url = '<%= ResolveUrl("~/Libs/Service/UploadService.asmx/UploadGuestPhoto")%>';
                    $.ajax({
                        async: false,
                        type: 'POST',
                        url: url,
                        data: '{ "imageData" : "' + image + '", "GuestID" : "' + MRN + '"}',
                        contentType: 'application/json; charset=utf-8',
                        dataType: 'json',
                        error: function (msg) {
                            showToast('Failed', msg.responseText);
                            hideLoadingPanel();
                        },
                        success: function (msg) {
                            success = msg;
                            if (!success)
                                showToast('Failed!', '');
                            else {
                                onAfterSavePatientPhoto();
                                pcRightPanelContent.Hide();
                            }
                            hideLoadingPanel();

                        }
                    });
                }
            });
            hideLoadingPanel();
        }
        else {
            var image = $('#imgBrowseResult').attr('src');
            image = image.replace('data:image/png;base64,', '');
            image = image.replace('data:image/jpeg;base64,', '');
            image = image.replace('data:image/gif;base64,', '');
            var MRN = parseInt('<%=MRN %>');
            var isUsingMRN = parseInt('<%=isUsingMRN %>');
            if (isUsingMRN == 1) {
                var url = '<%= ResolveUrl("~/Libs/Service/UploadService.asmx/UploadPatientPhoto")%>';
                $.ajax({
                    async: false,
                    type: 'POST',
                    url: url,
                    data: '{ "imageData" : "' + image + '", "MRN" : "' + MRN + '"}',
                    contentType: 'application/json; charset=utf-8',
                    dataType: 'json',
                    error: function (msg) {
                        showToast('Failed', msg.responseText);
                        hideLoadingPanel();
                    },
                    success: function (msg) {
                        success = msg;
                        if (!success)
                            showToast('Failed!', '');
                        else {
                            onAfterSavePatientPhoto();
                            pcRightPanelContent.Hide();
                        }
                        hideLoadingPanel();

                    }
                });
            }
            else {
                var url = '<%= ResolveUrl("~/Libs/Service/UploadService.asmx/UploadGuestPhoto")%>';
                $.ajax({
                    async: false,
                    type: 'POST',
                    url: url,
                    data: '{ "imageData" : "' + image + '", "GuestID" : "' + MRN + '"}',
                    contentType: 'application/json; charset=utf-8',
                    dataType: 'json',
                    error: function (msg) {
                        showToast('Failed', msg.responseText);
                        hideLoadingPanel();
                    },
                    success: function (msg) {
                        success = msg;
                        if (!success)
                            showToast('Failed!', '');
                        else {
                            onAfterSavePatientPhoto();
                            pcRightPanelContent.Hide();
                        }
                        hideLoadingPanel();

                    }
                });
            }

        }
    });

    $('#imgEditPatientPhotoBrowse').click(function () {
        document.getElementById('<%= FileUpload1.ClientID %>').click();
    });

    $('#<%=FileUpload1.ClientID %>').change(function () {
        readURL(this);
        if ($('#imgBrowseResult').width() > $('#imgBrowseResult').height())
            $('#imgBrowseResult').width('240px');
        else
            $('#imgBrowseResult').height('288px');

        $('#fc').hide();
        $('#imgBrowseResult').show();
    });

    window.readURL = function (input) {
        if (input.files && input.files[0]) {
            var reader = new FileReader();

            reader.onload = function (e) {
                $('#imgBrowseResult').attr('src', e.target.result);
            }

            reader.readAsDataURL(input.files[0]);
        }
    }
</script>
<style type="text/css">
    .divEditPatientPhotoBtnImage    { border: 1px solid black; }
</style>
<div id="divBody">
    <div style="display:none">
        <asp:FileUpload ID="FileUpload1" runat="server" />
    </div>
    <center>
        <div id="iUploadFrame">
            <img id="imgBrowseResult" src="#" alt="" style="max-width: 240px; max-height: 288px; display:none;" />
            <div id="fc">
                -- Cam Content --
            </div>
            <div id="upStatus" style="padding: 5px 0; color: #666;display:none;">
                Adjust, snap, then upload</div>
             
            <center>
                <table id="navcontainer" style="width:180px;">
                    <tr>
                        <td class="divEditPatientPhotoBtnImage">
                            <img class="imgLink" id="imgEditPatientPhotoBrowse" title="Browse" width="55px" src='<%= ResolveUrl("~/Libs/Images/Toolbar/browseFile.png")%>' alt='' />
                        </td>
                        <td class="divEditPatientPhotoBtnImage">
                            <img class="imgLink" id="imgEditPatientPhotoCapture" title="Capture" width="55px" src='<%= ResolveUrl("~/Libs/Images/Toolbar/cameraCapture.png")%>' alt='' />
                        </td>
                        <td class="divEditPatientPhotoBtnImage">
                            <img class="imgLink" id="imgEditPatientPhotoClear" title="Clear" width="55px" src='<%= ResolveUrl("~/Libs/Images/Toolbar/cameraZoomOut.png")%>' alt='' />
                        </td>
                        <td class="divEditPatientPhotoBtnImage">
                            <img class="imgLink" id="imgEditPatientPhotoSave" title="Save" width="55px" src='<%= ResolveUrl("~/Libs/Images/Toolbar/photoSave.png")%>' alt='' /> 
                        </td>
                    </tr>
                </table>
            </center>
            <div class="progress_beside_inline" id="ghimg">
            </div>
        </div>
    </center>
</div>

