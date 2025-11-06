<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MPEntryPopupCtl.ascx.cs" 
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.MPEntryPopupCtl" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>

    <input type="hidden" runat="server" id="hdnIsAdd" value="1" />
    <div class="toolbarArea">
        <table border="0" cellpadding="0" cellspacing="0" style="width:100%">
            <tr>
                <td style="width:50%" id="tdToolbarButton">
                    <ul>
                        <li style="display: none" runat="server" id="btnMPEntryPopupNew">
                            <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbnew.png")%>' alt="" /><div>
                                <%=GetLabel("New")%></div>
                        </li>
                        <li style="display: none" runat="server" id="btnMPEntryPopupSave">
                            <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbsave.png")%>' alt="" /><div>
                                <%=GetLabel("Save")%></div>
                        </li>
                    </ul>
                </td>
                <td style="width:50%; text-align: right; padding-left:5px; padding-right: 5px">
                    <div style="font-size: 1.4em">
                        <%=HttpUtility.HtmlEncode(GetPopupTitle())%></div>
                </td>
            </tr>
        </table>
    </div>
    <div style="padding:5px 0;">  
        <script type="text/javascript" id="dxss_mpentrypopupctl">
            window.setEntryPopupIsAdd = function (isAdd) {
                if (isAdd)
                    $('#<%=hdnIsAdd.ClientID %>').val('1');
                else
                    $('#<%=hdnIsAdd.ClientID %>').val('0');
            }
            window.getEntryPopupIsAdd = function () {
                return ($('#<%=hdnIsAdd.ClientID %>').val() == '1');
            }
            
            window.onRefreshCtlControl = function() {
                cbpMPEntryPopupContent.PerformCallback('refresh');
            }

            function addToolbarButton(htmlElement) {
                $("#tdToolbarButton ul").append(htmlElement);
            }

            $('#<%=btnMPEntryPopupSave.ClientID %>').show();

            $('#<%=btnMPEntryPopupSave.ClientID %>').click(function (evt) {
                var errMessage = { text: "", IsConfirm: false, messageConfirm: "" };
                var isAllowSave = true;
                var isAllowSavePatientValidation = "";
                var isValidPatient = false;
                if (typeof onBeforeSaveRecord != 'undefined') {
                    isAllowSave = onBeforeSaveRecord(errMessage);
                }

                if (typeof onBeforeSaveRecordPatientValidation != 'undefined') {
                    isAllowSavePatientValidation = onBeforeSaveRecordPatientValidation(errMessage);
                    Methods.getObject('GetvPatientList', isAllowSavePatientValidation, function (result) {
                        if (result != null) {

                            messageDoublePatient = 'Ditemukan ada pasien dengan data yang sama :<br/>' +
                                                'No.RM : ' + result.MedicalNo + '<br/>Nama Lengkap : ' + result.PatientName + '<br/>NIK : ' + result.SSN + '<br/>Tanggal Lahir : ' + Methods.getJSONDateValue(result.DateOfBirth) + '<br/>Jenis Kelamin : ' + result.Gender + '<br/>Alamat : ' + result.HomeAddress + '<br/>' +
                                                'Apakah ingin melanjutkan proses membuat nomor rekam medis baru ?<br/>';

                            showToastConfirmation(messageDoublePatient, function (result) {
                                if (result) {
                                    if (IsValid(evt, 'fsMPEntryPopup', 'mpEntryPopup'))
                                        cbpMPEntryPopupProcess.PerformCallback('save');
                                }
                            });
                        } else {
                            if (IsValid(evt, 'fsMPEntryPopup', 'mpEntryPopup'))
                                cbpMPEntryPopupProcess.PerformCallback('save');
                        }

                    });
                }

                if (isAllowSavePatientValidation == "") {
                    if (isAllowSave) {
                        if (IsValid(evt, 'fsMPEntryPopup', 'mpEntryPopup'))
                            cbpMPEntryPopupProcess.PerformCallback('save');
                    }
                    else if (errMessage.text != '') {
                        var errMessageSplit = errMessage.text.split('|');
                        if (errMessageSplit[0] == 'showconfirm') {
                            showToastConfirmation(errMessageSplit[1], function (result) {
                                if (result) {
                                    if (IsValid(evt, 'fsMPEntryPopup', 'mpEntryPopup'))
                                        cbpMPEntryPopupProcess.PerformCallback('save');
                                }
                            });
                        }
                        else {
                            displayErrorMessageBox('SAVE', errMessage.text);
                        }
                    }
                }

            });

            $(function () {
                /*if ($('#<%=hdnIsAdd.ClientID %>').val() == '1') {
                $('#btnMPEntryPopupNew').show();
                }*/

                $('#<%=btnMPEntryPopupNew.ClientID %>').click(function () {
                    cbpMPEntryPopupContent.PerformCallback('new');
                });
            });
        </script>
        <dxcp:ASPxCallbackPanel ID="cbpMPEntryPopupContent" runat="server" Width="100%" ClientInstanceName="cbpMPEntryPopupContent"
            ShowLoadingPanel="false" OnCallback="cbpMPEntryPopupContent_Callback">
            <ClientSideEvents BeginCallback="function(s,e){
                showLoadingPanel();
            }" EndCallback="function(s,e){
                setEntryPopupIsAdd(true);
                hideLoadingPanel();
            }" />
            <PanelCollection>
                <dx:PanelContent ID="PanelContent1" runat="server"> 
                    <fieldset id="fsMPEntryPopup">  
                        <asp:Panel ID="pnlEntryPopup" runat="server" />  
                    </fieldset>
                </dx:PanelContent>
            </PanelCollection>
        </dxcp:ASPxCallbackPanel>

        <dxcp:ASPxCallbackPanel ID="cbpMPEntryPopupProcess" runat="server" Width="100%" ClientInstanceName="cbpMPEntryPopupProcess"
            ShowLoadingPanel="false" OnCallback="cbpMPEntryPopupProcess_Callback">
            <ClientSideEvents BeginCallback="function(s,e){
                showLoadingPanel();
            }" EndCallback="function(s,e){
                var result = s.cpResult.split('|');
                if(result[0] == 'saveadd' || result[0] == 'saveedit'){
                    if(result[1] == 'success'){
                        var param = s.cpRetval;
                        if(result[0] == 'saveadd' && typeof onAfterSaveAddRecordEntryPopup != 'undefined')
                            onAfterSaveAddRecordEntryPopup(param);
                        if(result[0] == 'saveedit' && typeof onAfterSaveEditRecordEntryPopup != 'undefined')
                            onAfterSaveEditRecordEntryPopup(param);

                        var isAdd = false;
                        if(result[0] == 'saveadd')
                            isAdd = true;

                        if(result[2] != '') {
                            var messageBody = result[2];
                            displayErrorMessageBox('SAVE', messageBody);
                        }

                        if(typeof onGetEntryPopupReturnValue != 'undefined' && typeof onAfterSaveRightPanelContent != 'undefined')
                            onAfterSaveRightPanelContent($('#hdnRightPanelContentCode').val(), onGetEntryPopupReturnValue(), isAdd);

                        pcRightPanelContent.Hide();
                    }
                    else
                        if(result[2] != '') {
                            var messageBody = result[2];
                            displayErrorMessageBox('SAVE', messageBody);
                        }
                        else
                        {
                            var messageBody = 'An error occured in your process.';
                            displayErrorMessageBox('SAVE', messageBody);
                        }
                }
                hideLoadingPanel();
            }" />
        </dxcp:ASPxCallbackPanel>
    </div>

