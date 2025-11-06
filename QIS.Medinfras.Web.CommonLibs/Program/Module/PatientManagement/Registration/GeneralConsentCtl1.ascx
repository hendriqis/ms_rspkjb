<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="GeneralConsentCtl1.ascx.cs" 
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.GeneralConsentCtl1" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>

<script type="text/javascript" id="dxss_generalConsentEntryctl">
    setDatePicker('<%=txtConsentDate.ClientID %>');
    $('#<%=txtConsentDate.ClientID %>').datepicker('option', 'maxDate', '0');

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
            var recordID = $(this).attr('recordID');
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

        function GetPatientFamilyFilterExp() {
            var filterExpression = "<%:GetPatientFamilyFilterExp() %>";
            return filterExpression;
        }

        function GetEmergencyContactFilterExp() {
            var filterExpression = "<%:GetEmergencyContactFilterExp() %>";
            return filterExpression;
        }

        $('#<%:lblConsentObjectID.ClientID %>.lblLink').click(function () {
            if ($('#<%=hdnConsentObjectType.ClientID %>').val() == "1") {
                openSearchDialog('patientFamily2', GetPatientFamilyFilterExp(), function (value) {
                    onSelectedPatientFamily(value);
                });
            }
            else {
                openSearchDialog('emergencyContact1', GetEmergencyContactFilterExp(), function (value) {
                    onSelectedEmergencyContact(value);
                });
            }
        });

        function onSelectedPatientFamily(param) {
            var paramInfo = param.split('|');
            $('#<%=hdnFamilyID.ClientID %>').val(paramInfo[0]);
            $('#<%=txtSignature2Name.ClientID %>').val(paramInfo[2]);
            $('#<%=txtRelationship.ClientID %>').val(paramInfo[4]);
        }

        function onSelectedEmergencyContact(param) {
            var paramInfo = param.split('|');
            $('#<%=txtSignature2Name.ClientID %>').val(paramInfo[1]);
            $('#<%=txtRelationship.ClientID %>').val(paramInfo[3]);
        }

        $('#<%=rblIsPatientFamily.ClientID %> input').die('change');
        $('#<%=rblIsPatientFamily.ClientID %> input').live('change', function () {
            $('#<%=hdnConsentObjectType.ClientID %>').val($(this).val());
            if ($(this).val() != "0" && $(this).val() != "") {
                $('#<%=trFamilyInfo.ClientID %>').removeAttr("style");
                $('#<%=hdnFamilyID.ClientID %>').val("0");
                $('#<%=txtSignature2Name.ClientID %>').val("");
                $('#<%=txtRelationship.ClientID %>').val("");
            }
            else {
                $('#<%=trFamilyInfo.ClientID %>').attr("style", "display:none");
                $('#<%=hdnFamilyID.ClientID %>').val("0");
                $('#<%=txtSignature2Name.ClientID %>').val("");
                $('#<%=txtRelationship.ClientID %>').val("");
            }
        });
    });

    //#region Left Navigation Panel
    $('#leftPageNavPanel ul li').click(function () {
        $('#leftPageNavPanel ul li.selected').removeClass('selected');
        $(this).addClass('selected');
        var contentID = $(this).attr('contentID');

        showContent(contentID);
    });

    function showContent(contentID) {
        var i, x, tablinks;
        x = document.getElementsByClassName("divPageNavPanelContent");
        for (i = 0; i < x.length; i++) {
            x[i].style.display = "none";
        }
        document.getElementById(contentID).style.display = "block";
    }
    //#endregion


    function onBeforeSaveRecord(errMessage) {
        return true;
    }

    function onAfterSaveAddRecordEntryPopup(result) {
        if (typeof onAfterSaveRightPanelContent == 'function')
            onAfterSaveRightPanelContent('generalConsent',$('#<%=hdnPopupRegistrationNo.ClientID %>').val(), '');
    }

    function onAfterSaveEditRecordEntryPopup(result) {
        if (typeof onAfterSaveRightPanelContent == 'function')
            onAfterSaveRightPanelContent('generalConsent',$('#<%=hdnPopupRegistrationNo.ClientID %>').val(), '');
    }

    $('#leftPageNavPanel ul li').first().click();

    function onCbpPopupProcesEndCallback(s) {
        hideLoadingPanel();
        var param = s.cpResult.split('|');
        var retval = s.cpRetval.split('|');
        if (param[0] == 'process') {
            if (param[1] == '0') {
                $('.tblResultInfo').hide();
                displayErrorMessageBox("Tanda Tangan Digital", param[2]);
            }
            else {
                if (typeof onRefreshControl == 'function') {
                    onRefreshControl();
                }
            }
        }
    }
</script>
<div>
    <input type="hidden" runat="server" id="hdnPopupRegistrationID" value="" />
    <input type="hidden" runat="server" id="hdnPopupRegistrationNo" value="" />
    <input type="hidden" runat="server" id="hdnPopupMRN" value="" />
    <input type="hidden" runat="server" id="hdnExistingSignature" value="" />
    <input type="hidden" runat="server" id="hdnSignatureName" />
    <input type="hidden" runat="server" id="hdnReferenceID" />
    <input type="hidden" runat="server" id="hdnSignatureIndex" />
    <input type="hidden" runat="server" id="hdnReferenceIDType" />
    <input type="hidden" runat="server" id="hdnConsentObjectType" value="0" />
    <input type="hidden" runat="server" id="hdnFamilyID" value="0" />
    <table class="tblContentArea">
        <colgroup>
            <col style="width: 22%" />
            <col style="width: 78%" />
        </colgroup>
        <tr>
            <td style="vertical-align: top">
                <div id="leftPageNavPanel" class="w3-border">
                    <ul>
                        <li contentID="divPage1" contentIndex="1" title="General Consent" class="w3-hover-red">General Consent</li>
                        <li contentID="divPage2" contentIndex="2" title="Catatan Tambahan" class="w3-hover-red">Catatan Tambahan</li>
                        <li contentID="divPage3" contentIndex="3" title="Tanda Tangan" class="w3-hover-red">Tanda Tangan</li>
                    </ul>
                </div>
                <div>
                    <table class="w3-table-all" style="width: 100%">
                        <tr>
                            <td style="text-align: left" class="w3-blue-grey">
                                <div class=" w3-small">
                                    <%=GetLabel("Tanggal dan Jam :")%></div>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align:center">
                                <asp:TextBox ID="txtConsentDate" Width="120px" CssClass="datepicker" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align:center" ><asp:TextBox ID="txtConsentTime" Width="80px" CssClass="time" runat="server" Style="text-align:center" /></td>
                        </tr>
                        <tr>
                            <td style="text-align: left" class="w3-blue-grey">
                                <div class=" w3-small">
                                    <%=GetLabel("Petugas :")%></div>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left">
                                <div id="lblUserName" runat="server">
                                </div>
                            </td>
                        </tr>
                    </table>
                </div>
            </td>
            <td style="padding:5px;vertical-align:top;">
                <div id="divPage1" class="w3-border divPageNavPanelContent w3-animate-left" style="display: none">
                   <table class="tblEntryContent" style="width:99%">
                        <colgroup>
                            <col />
                        </colgroup>
                        <tr>
                            <td>
                                <table border="0" cellpadding="1" cellspacing="0">
                                    <tr>
                                        <td class="tdLabel">
                                            <label class="lblMandatory">
                                                <%=GetLabel("Pemberi pernyataan :")%></label>
                                        </td>
                                        <td colspan="2">
                                            <asp:RadioButtonList ID="rblIsPatientFamily" runat="server" RepeatDirection="Horizontal">
                                                <asp:ListItem Text=" Pasien" Value="0" Selected="True" />
                                                <asp:ListItem Text=" Keluarga" Value="1"  />
                                                <asp:ListItem Text=" Penanggung Jawab Pasien" Value = "2" />
                                            </asp:RadioButtonList>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr id="trFamilyInfo" runat="server" style="display: none">
                            <td style="padding-left : 20px">
                                <table border="0" cellpadding="1" cellspacing="0">
                                    <colgroup>
                                        <col style="width:200px"/>
                                        <col style="width:150px"/>
                                        <col style="width:150px"/>
                                        <col />
                                    </colgroup>
                                    <tr>
                                        <td class="tdLabel">
                                            <label class="lblMandatory lblLink" id="lblConsentObjectID" runat="server">
                                                <%=GetLabel("Nama")%></label>
                                        </td>
                                        <td colspan="3">
                                            <asp:TextBox ID="txtSignature2Name" CssClass="txtSignature2Name" runat="server" Width="100%" ReadOnly="true"  />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tdLabel">
                                            <label class="lblMandatory">
                                                <%=GetLabel("Hubungan")%></label>
                                        </td>
                                        <td colspan="3">
                                            <asp:TextBox ID="txtRelationship" Width="100%" runat="server" ReadOnly />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>   
                        <tr>
                            <td class="tdLabel">
                                <label>
                                    <%=GetLabel("Bertindak atau memberi pernyataan untuk pasien atas nama :")%></label>
                            </td>
                        </tr>
                        <tr>
                            <td style="padding-left : 20px">
                                <table border="0" cellpadding="1" cellspacing="0">
                                    <colgroup>
                                        <col style="width:200px"/>
                                        <col style="width:150px"/>
                                        <col style="width:150px"/>
                                        <col />
                                    </colgroup>
                                    <tr>
                                        <td class="tdLabel">
                                            <label>
                                                <%=GetLabel("Nama Pasien")%></label>
                                        </td>
                                        <td colspan="3">
                                            <asp:TextBox ID="txtPatientName" Width="100%" runat="server" ReadOnly />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tdLabel">
                                            <label>
                                                <%=GetLabel("No. RM")%></label>
                                        </td>
                                        <td class="tdLabel">
                                            <asp:TextBox ID="txtMedicalNo" Width="120px" runat="server" ReadOnly />                                       
                                        </td>
                                        <td class="tdLabel">
                                            <label>
                                                <%=GetLabel("Tanggal Lahir")%></label>
                                        </td>   
                                        <td>
                                            <asp:TextBox ID="txtDateOfBirth" Width="130px" runat="server" ReadOnly />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tdLabel">
                                            <label>
                                                <%=GetLabel("No. Registrasi")%></label>
                                        </td>
                                        <td colspan="3">
                                            <asp:TextBox ID="txtRegistrationNo" Width="100%" runat="server" ReadOnly />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <table border="0" cellpadding="1" cellspacing="0">
                                    <colgroup>
                                        <col style="width:350px"/>
                                        <col />
                                    </colgroup>
                                    <tr>
                                        <td class="tdLabel">
                                            <label class="lblMandatory">
                                                <%=GetLabel("Ketentuan Pembayaran")%></label>
                                        </td>
                                        <td colspan="2">
                                            <asp:RadioButtonList ID="rblPembayaran" runat="server" RepeatDirection="Horizontal">
                                                <asp:ListItem Text=" Setuju" Value="1" />
                                                <asp:ListItem Text=" Tidak Setuju" Value="0"  />
                                            </asp:RadioButtonList>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <table border="0" cellpadding="1" cellspacing="0">
                                    <colgroup>
                                        <col style="width:350px"/>
                                        <col />
                                    </colgroup>
                                    <tr>
                                        <td class="tdLabel">
                                            <label class="lblMandatory">
                                                <%=GetLabel("Hak dan Kewajiban Pasien")%></label>
                                        </td>
                                        <td colspan="2">
                                            <asp:RadioButtonList ID="rblHKP" runat="server" RepeatDirection="Horizontal">
                                                <asp:ListItem Text=" Setuju" Value="1" />
                                                <asp:ListItem Text=" Tidak Setuju" Value="0"  />
                                            </asp:RadioButtonList>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <table border="0" cellpadding="1" cellspacing="0">
                                    <colgroup>
                                        <col style="width:350px"/>
                                        <col />
                                    </colgroup>
                                    <tr>
                                        <td class="tdLabel">
                                            <label class="lblMandatory">
                                                <%=GetLabel("Tata Tertib RS")%></label>
                                        </td>
                                        <td colspan="2">
                                            <asp:RadioButtonList ID="rblTataTertib" runat="server" RepeatDirection="Horizontal">
                                                <asp:ListItem Text=" Setuju" Value="1" />
                                                <asp:ListItem Text=" Tidak Setuju" Value="0"  />
                                            </asp:RadioButtonList>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <table border="0" cellpadding="1" cellspacing="0">
                                    <colgroup>
                                        <col style="width:350px"/>
                                        <col />
                                    </colgroup>
                                    <tr>
                                        <td class="tdLabel">
                                            <label class="lblMandatory">
                                                <%=GetLabel("Kebutuhan Penterjemah Bahasa")%></label>
                                        </td>
                                        <td colspan="2">
                                            <asp:RadioButtonList ID="rblPenterjemah" runat="server" RepeatDirection="Horizontal">
                                                <asp:ListItem Text=" Ya" Value="1" />
                                                <asp:ListItem Text=" Tidak" Value="0"  />
                                            </asp:RadioButtonList>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <table border="0" cellpadding="1" cellspacing="0">
                                    <colgroup>
                                        <col style="width:350px"/>
                                        <col />
                                    </colgroup>
                                    <tr>
                                        <td class="tdLabel">
                                            <label class="lblMandatory">
                                                <%=GetLabel("Kebutuhan Rohaniawan")%></label>
                                        </td>
                                        <td colspan="2">
                                            <asp:RadioButtonList ID="rblRohaniawan" runat="server" RepeatDirection="Horizontal">
                                                <asp:ListItem Text=" Ya" Value="1" />
                                                <asp:ListItem Text=" Tidak" Value="0"  />
                                            </asp:RadioButtonList>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <table border="0" cellpadding="1" cellspacing="0">
                                    <colgroup>
                                        <col style="width:350px"/>
                                        <col />
                                    </colgroup>
                                    <tr>
                                        <td class="tdLabel">
                                            <label class="lblMandatory">
                                                <%=GetLabel("Pelepasan Informasi : Penjamin Bayar")%></label>
                                        </td>
                                        <td colspan="2">
                                            <asp:RadioButtonList ID="rblPenjamin" runat="server" RepeatDirection="Horizontal">
                                                <asp:ListItem Text=" Setuju" Value="1" />
                                                <asp:ListItem Text=" Tidak Setuju" Value="0"  />
                                            </asp:RadioButtonList>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <table border="0" cellpadding="1" cellspacing="0">
                                    <colgroup>
                                        <col style="width:350px"/>
                                        <col />
                                    </colgroup>
                                    <tr>
                                        <td class="tdLabel">
                                            <label class="lblMandatory">
                                                <%=GetLabel("Pelepasan Informasi : Penelitian/Peserta Didik")%></label>
                                        </td>
                                        <td colspan="2">
                                            <asp:RadioButtonList ID="rblPenelitian" runat="server" RepeatDirection="Horizontal">
                                                <asp:ListItem Text=" Setuju" Value="1" />
                                                <asp:ListItem Text=" Tidak Setuju" Value="0"  />
                                            </asp:RadioButtonList>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <table border="0" cellpadding="1" cellspacing="0">
                                    <colgroup>
                                        <col style="width:350px"/>
                                        <col />
                                    </colgroup>
                                    <tr>
                                        <td class="tdLabel">
                                            <label class="lblMandatory">
                                                <%=GetLabel("Pelepasan Informasi : Anggota Keluarga")%></label>
                                        </td>
                                        <td colspan="2">
                                            <asp:RadioButtonList ID="rblKeluarga" runat="server" RepeatDirection="Horizontal">
                                                <asp:ListItem Text=" Setuju" Value="1" />
                                                <asp:ListItem Text=" Tidak Setuju" Value="0"  />
                                            </asp:RadioButtonList>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <table border="0" cellpadding="1" cellspacing="0">
                                    <colgroup>
                                        <col style="width:350px"/>
                                        <col />
                                    </colgroup>
                                    <tr>
                                        <td class="tdLabel">
                                            <label class="lblMandatory">
                                                <%=GetLabel("Pelepasan Informasi : Fasyankes Rujukan")%></label>
                                        </td>
                                        <td colspan="2">
                                            <asp:RadioButtonList ID="rblRujukan" runat="server" RepeatDirection="Horizontal">
                                                <asp:ListItem Text=" Setuju" Value="1" />
                                                <asp:ListItem Text=" Tidak Setuju" Value="0"  />
                                            </asp:RadioButtonList>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <table border="0" cellpadding="1" cellspacing="0">
                                    <colgroup>
                                        <col style="width:350px"/>
                                        <col />
                                    </colgroup>
                                    <tr>
                                        <td class="tdLabel">
                                            <label class="lblMandatory">
                                                <%=GetLabel("Pelepasan Informasi : SATUSEHAT (KEMENKES RI)")%></label>
                                        </td>
                                        <td colspan="2">
                                            <asp:RadioButtonList ID="rblSATUSEHAT" runat="server" RepeatDirection="Horizontal">
                                                <asp:ListItem Text=" Setuju" Value="1" />
                                                <asp:ListItem Text=" Tidak Setuju" Value="0"  />
                                            </asp:RadioButtonList>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                  </table>             
                </div>
                <div id="divPage2" class="w3-border divPageNavPanelContent w3-animate-left" style="display: none">
                    <table border="0" cellpadding="1" cellspacing="0" width="100%">
                        <colgroup>
                            <col style="width:350px"/>
                            <col />
                        </colgroup>
                        <tr>
                            <td colspan="2">
                                <%=GetLabel("Catatan Tambahan terkait proses pembuatan General Consent") %>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" style="padding-top: 10px">
                                <asp:TextBox ID="txtRemarks" runat="server" Width="98%" TextMode="Multiline"
                                    Rows="20" />
                            </td>
                        </tr>
                    </table>
                </div>
                <div id="divPage3" class="w3-border divPageNavPanelContent w3-animate-left" style="display: none">
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
                        <tr align="center" id="trInfo" runat="server">                
                            <td class="tdLabel" colspan="2"><label class="lblNormal" style="font-style:italic"><%=GetLabel("Silahkan Tanda tangan di area kotak di bawah ini :")%></label></td>
                        </tr>
                        <tr style="text-align:center;" id="trCanvas" runat="server">
                            <td colspan="2">
                                <canvas id="sketchpad" width="400" height="180" style="border:1px solid #d3d3d3; background-color:#ecf0f1"> 
                                </canvas>
                            </td>
                        </tr>
                        <tr style="text-align:center;" id="trImage" runat="server">
                            <td colspan="2">
                                <asp:PlaceHolder ID="plImage" runat="server" />
                            </td>
                        </tr>
                    </table>
                    <br /> 
                    <center>
                        <table id="navcontainer">
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
            </td>
        </tr>
    </table>
</div>
<dxcp:ASPxCallbackPanel ID="cbpPopupProcess" runat="server" Width="100%" ClientInstanceName="cbpPopupProcess"
    ShowLoadingPanel="false" OnCallback="cbpPopupProcess_Callback">
    <ClientSideEvents BeginCallback="function(s,e) { showLoadingPanel(); }" EndCallback="function(s,e) { onCbpPopupProcesEndCallback(s); }" />
</dxcp:ASPxCallbackPanel>

