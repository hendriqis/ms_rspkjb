<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="BodyDiagramTakePhotoCtl.ascx.cs" 
    Inherits="QIS.Medinfras.Web.EMR.Program.BodyDiagramTakePhotoCtl" %>

<script id="dxis_webcamctl1" src='<%= ResolveUrl("~/Libs/Scripts/Webcam/webcam.js")%>' type='text/javascript'></script>
<script id="dxss_webcamctl" type="text/javascript">
    $(function () {
        if (webcam != null) {
            webcam.set_swf_url('<%= ResolveUrl("~/Libs/Scripts/Webcam/webcam.swf")%>');
            webcam.set_api_url(document.URL);
            webcam.set_quality(90); // JPEG quality (1 - 100)
            webcam.set_shutter_sound(false);


            $("#imgWrapper").css({ height: "255px",
                width: "240px",
                border: "solid 1px #aaa"
            });

            $("#fc").html(webcam.get_html(240, 255, 282, 300));
        }
    });

    window.initBodyDiagramTakePhoto = function () {
        if (webcam != null) {
            $('#fc').show();
            $('#imgBrowseResult').hide();
            camReset();
        }
    }

    window.camReset = function () {
        webcam.reset();
        //setCamInstruction("Adjust, snap, then upload", "#666");
    }

    function setCamInstruction(msg, c) {
        //$("#upStatus").html(msg).css("color", c);
    }

    window.handleUpload = function () {
        if ($("#fc").is(":visible")) {
            var make = $("#make").val();
            var gi = $("#ghimg");
            gi.css("visibility", "visible");

            webcam.upload('<%= ResolveUrl("~/Program/PatientPage/Objective/BodyDiagram/BodyDiagramUploadWebcam.aspx")%>', function () {
                gi.css("visibility", "hidden");
                pcTakePhoto.Hide();
            });
        }
        else
            pcTakePhoto.Hide();
    }

    $('#imgCapture').click(function () {
        $('#fc').show();
        $('#imgBrowseResult').hide();
        webcam.freeze();
    });

    $('#imgClear').click(function () {
        $('#fc').show();
        $('#imgBrowseResult').hide();
        camReset();
    });

    $('#imgSave').click(function () {
        handleUpload();
    });

    $('#imgBrowse').click(function () {
        document.getElementById('<%= FileUpload1.ClientID %>').click();
    });

    $('#<%=FileUpload1.ClientID %>').change(function () {
        readURL(this);
        if ($('#imgBrowseResult').width() > $('#imgBrowseResult').height())
            $('#imgBrowseResult').width('240px');
        else
            $('#imgBrowseResult').height('255px');

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
<div id="divBody">
    <div style="display:none">
        <asp:FileUpload ID="FileUpload1" runat="server" />
    </div>
    <center>
        <div id="iUploadFrame">
            <img id="imgBrowseResult" src="#" alt="" style="max-width: 240px; max-height: 255px; display:none;" />
            <div id="fc">
                -- Cam Content --
            </div>
            <div id="upStatus" style="padding: 5px 0; color: #666;display:none;">
                Adjust, snap, then upload</div>
             
            <center>
                <table id="navcontainer" style="width:180px;">
                    <tr>
                        <td class="divBtnImage">
                            <img class="imgLink" id="imgBrowse" title="Browse" width="55px" src='<%= ResolveUrl("~/Libs/Images/Toolbar/browseFile.png")%>' alt='' />
                        </td>
                        <td class="divBtnImage">
                            <img class="imgLink" id="imgCapture" title="Capture" width="55px" src='<%= ResolveUrl("~/Libs/Images/Toolbar/cameraCapture.png")%>' alt='' />
                        </td>
                        <td class="divBtnImage">
                            <img class="imgLink" id="imgClear" title="Clear" width="55px" src='<%= ResolveUrl("~/Libs/Images/Toolbar/cameraZoomOut.png")%>' alt='' />
                        </td>
                        <td class="divBtnImage">
                            <img class="imgLink" id="imgSave" title="Save" width="55px" src='<%= ResolveUrl("~/Libs/Images/Toolbar/photoSave.png")%>' alt='' /> 
                        </td>
                    </tr>
                </table>
            </center>
            <div class="progress_beside_inline" id="ghimg">
            </div>
        </div>
    </center>
</div>
