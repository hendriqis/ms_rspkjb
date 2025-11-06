<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ConfirmWithNoteCtl.ascx.cs" 
    Inherits="QIS.Medinfras.Web.CommonLibs.Controls.ConfirmWithNoteCtl" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>

<script id="dxis_signature1" src='<%= ResolveUrl("~/Libs/Scripts/jquery/jquery-1.7.min.js")%>' type='text/javascript'></script>
<script id="dxss_signature" type="text/javascript">
    $(function () {
        var canvas = document.getElementById('sketchpad');
        var ctx = canvas.getContext('2d');

        canvas.addEventListener("pointerdown", pointerDown, false);
        canvas.addEventListener("pointerup", pointerUp, false);

        function pointerDown(evt) {
            ctx.beginPath();
            ctx.moveTo(evt.offsetX, evt.offsetY);
            canvas.addEventListener("pointermove", paint, false);
        }

        function pointerUp(evt) {
            canvas.removeEventListener("pointermove", paint);
            paint(evt);
        }

        function paint(evt) {
            ctx.strokeStyle = 'Blue';
            ctx.lineWidth = 2;
            ctx.lineTo(evt.offsetX, evt.offsetY);
            ctx.stroke();
        }

        // Clear the canvas context using the canvas width and height
        function clearCanvas(canvas, ctx) {
            ctx.clearRect(0, 0, canvas.width, canvas.height);
        }

        $('#<%=chkToggleSwitch.ClientID %>').change(function () {
            
        });

        $('#<%=btnAcceptSignature.ClientID %>').click(function () {
            var userName = $('#<%=hdnUserParamedicName.ClientID %>').val();
            var ts = new Date();
            var dateTime = ts.toLocaleDateString() + " " + ts.toLocaleTimeString();
            //var dateTime = "dd/MM/yyyy hh:mm TT";
            var signatureData = userName;
            ctx.font = "12px Arial";
            ctx.fillText(dateTime, 10, 140);
            ctx.fillText(signatureData, 10, 160);
            var imgData = canvas.toDataURL();
            displayConfirmationMessageBox('Tanda Tangan Digital', 'Simpan Tanda Tangan ?', function (result) {
                if (result) {
                    var param = $('#<%=hdnPatientVisitNoteID.ClientID %>').val() + "|" + imgData + "|" + $('#<%=hdnSignatureIndex.ClientID %>').val();
                    cbpPopupProcess.PerformCallback(param);
                }
            });
        });

        $('#<%=btnClearSignature.ClientID %>').click(function () {
            clearCanvas(canvas, ctx);
        });
    });

    function onCbpPopupProcesEndCallback(s) {
        hideLoadingPanel();
        var param = s.cpResult.split('|');
        var retval = s.cpRetval.split('|');
        if (param[0] == 'process') {
            if (param[1] == '0') {
                $('.tblResultInfo').hide();
                displayErrorMessageBox("Tanda Tangan Digital",param[2]);
            }
            else {
                if (typeof onRefreshControl == 'function')
                    onRefreshControl();
                pcRightPanelContent.Hide();
            }
        }
    }

    function onAfterProcessPopupEntry(param) {
        if (typeof onRefreshControl == 'function')
            onRefreshControl();
        pcRightPanelContent.Hide();
    }
</script>
<style type="text/css">
    .divEditPatientPhotoBtnImage    { border: 1px solid #d3d3d3; }
    
    /* The switch - the box around the slider */
    .switch {
      position: relative;
      display: inline-block;
      width: 60px;
      height: 34px;
    }

    /* Hide default HTML checkbox */
    .switch input {
      opacity: 0;
      width: 0;
      height: 0;
    }

    /* The slider */
    .slider {
      position: absolute;
      cursor: pointer;
      top: 0;
      left: 0;
      right: 0;
      bottom: 0;
      background-color: #ccc;
      -webkit-transition: .4s;
      transition: .4s;
    }

    .slider:before {
      position: absolute;
      content: "";
      height: 26px;
      width: 26px;
      left: 4px;
      bottom: 4px;
      background-color: white;
      -webkit-transition: .4s;
      transition: .4s;
    }

    input:checked + .slider 
    {
      background-color: #2196F3;
    }

    input:focus + .slider {
      box-shadow: 0 0 1px #2196F3;
    }

    input:checked + .slider:before {
      -webkit-transform: translateX(26px);
      -ms-transform: translateX(26px);
      transform: translateX(26px);
    }

    /* Rounded sliders */
    .slider.round {
      border-radius: 34px;
    }

    .slider.round:before {
      border-radius: 50%;
    }     
</style>

<input type="hidden" id="hdnPatientVisitNoteID" runat="server" />
<input type="hidden" id="hdnUserParamedicName" runat="server" />
<input type="hidden" id="hdnSignatureIndex" runat="server" />
<div id="divBody">
    <div width="500px" height="auto">
        <table class="tblEntryContent" style="width:100%">
            <colgroup>
                <col style="width:175px"/>
                <col style="width:325px"/>
            </colgroup>
            <tr>
                <td class="tdLabel"><label class="lblNormal"><%=GetLabel("PPA")%></label></td>
                <td><asp:TextBox ID="txtParamedicName" ReadOnly="true" Width="100%" runat="server" /></td>
            </tr>  
            <tr>
                <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Tanggal - Waktu")%></label></td>
                <td><asp:TextBox ID="txtNoteDateTime" ReadOnly="true" Width="175px" runat="server" /></td>
            </tr> 
            <tr>
                <td class="tdLabel" style="vertical-align:top"><label class="lblNormal"><%=GetLabel("Catatan")%></label></td>
                <td>
                    <asp:TextBox ID="txtRemarks" runat="server" Width="100%" TextMode="Multiline"
                        Rows="5" />
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <br />
                </td>
            </tr>
            <tr style="display:none">   
                <td />
                <td>
                    <!-- Rounded switch -->
                    <label class="switch">
                        <input type="checkbox" id="chkToggleSwitch" runat="server" class="chkToggleSwitch">
                        <span class="slider round"></span>
                    </label> 
                </td>                         
            </tr>
        </table>
        <div id="divSignature" style="display:none">
            <label class="lblNormal" style="font-style:italic"><%=GetLabel("Silahkan Tanda tangan di area kotak di bawah ini :")%></label>
            <canvas id="sketchpad" width="400" height="180" style="border: 1px solid #d3d3d3;
                background-color: #ecf0f1"> 
            </canvas>
            <br /> 
            <center>
                <table id="navcontainer" style="width:180px;">
                    <tr>

                        <td class="divEditPatientPhotoBtnImage">
                            <input type="button" id="btnAcceptSignature" runat="server" class="btnAcceptSignature w3-btn w3-hover-blue" value="ACCEPT"
                                style="width: 100px; background-color: Red; color: White;" />
                        </td>
                        <td class="divEditPatientPhotoBtnImage">
                            <input type="button" id="btnClearSignature" runat="server" class="btnClearSignature w3-btn w3-hover-blue"  value="RESET"
                                style="width: 100px; background-color: Red; color: White;" />
                        </td>
                    </tr>
                </table>
            </center>
        </div>
    </div>
</div>
<dxcp:ASPxCallbackPanel ID="cbpPopupProcess" runat="server" Width="100%" ClientInstanceName="cbpPopupProcess"
    ShowLoadingPanel="false" OnCallback="cbpPopupProcess_Callback">
    <ClientSideEvents BeginCallback="function(s,e) { showLoadingPanel(); }" EndCallback="function(s,e) { onCbpPopupProcesEndCallback(s); }" />
</dxcp:ASPxCallbackPanel>

