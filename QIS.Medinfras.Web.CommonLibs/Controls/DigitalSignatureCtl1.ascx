<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="DigitalSignatureCtl1.ascx.cs" 
    Inherits="QIS.Medinfras.Web.CommonLibs.Controls.DigitalSignatureCtl1" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>

<%@ Register assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a" namespace="DevExpress.Web.ASPxPanel" tagprefix="dx" %>

<script src='<%= ResolveUrl("~/Libs/Scripts/jquery/jquery-1.7.min.js")%>' type='text/javascript'></script>
<script id="dxss_signature1" type="text/javascript">
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

        $('#<%=btnAcceptSignature.ClientID %>').click(function () {
            var userName = $('#<%=hdnSignatureName.ClientID %>').val();
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
                    var param = $('#<%=hdnReferenceID.ClientID %>').val() + "|" + imgData + "|" + $('#<%=hdnSignatureIndex.ClientID %>').val();
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
                if (typeof onRefreshControl == 'function') {
                    onRefreshControl();
                }
                pcRightPanelContent.Hide();
            }
        }
    }
</script>
<style type="text/css">  
</style>

<input type="hidden" id="hdnReferenceID" runat="server" />
<input type="hidden" id="hdnSignatureName" runat="server" />
<input type="hidden" id="hdnSignatureIndex" runat="server" />
<input type="hidden" id="hdnReferenceIDType" runat="server" />
<div id="divBody">
    <div width="500px" height="300px">
        <table class="tblEntryContent" style="width:100%">
            <colgroup>
                <col style="width:175px"/>
                <col style="width:325px"/>
            </colgroup>
            <tr>
                <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Nama")%></label></td>
                <td><asp:TextBox ID="txtSignatureName" ReadOnly="true" Width="100%" runat="server" /></td>
            </tr>  
            <tr>
                <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Tanggal - Waktu")%></label></td>
                <td><asp:TextBox ID="txtNoteDateTime" ReadOnly="true" Width="175px" runat="server" /></td>
            </tr> 
            <tr>
                <td colspan="2">
                    <br />
                </td>
            </tr>
            <tr>                
                <td class="tdLabel" colspan="2"><label class="lblNormal" style="font-style:italic"><%=GetLabel("Silahkan Tanda tangan di area kotak di bawah ini :")%></label></td>
            </tr>
        </table>
        <canvas id="sketchpad" width="400" height="180" style="border:1px solid #d3d3d3; background-color:#ecf0f1"> 
        </canvas>
        <br /> 
        <center>
            <table id="navcontainer" style="width:180px;">
                <tr>
                    <td style="display:none">
                        <!-- Rounded switch -->
                        <label class="switch">
                          <input type="checkbox" id="chkToggleSwitch" runat="server" class="chkToggleSwitch">
                          <span class="slider round"></span>
                        </label> 
                    </td>
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
<dxcp:ASPxCallbackPanel ID="cbpPopupProcess" runat="server" Width="100%" ClientInstanceName="cbpPopupProcess"
    ShowLoadingPanel="false" OnCallback="cbpPopupProcess_Callback">
    <ClientSideEvents BeginCallback="function(s,e) { showLoadingPanel(); }" EndCallback="function(s,e) { onCbpPopupProcesEndCallback(s); }" />
</dxcp:ASPxCallbackPanel>

