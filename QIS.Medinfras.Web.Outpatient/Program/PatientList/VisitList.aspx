<%@ Page Language="C#" MasterPageFile="~/Libs/MasterPage/MPMain.master" AutoEventWireup="true"
    CodeBehind="VisitList.aspx.cs" Inherits="QIS.Medinfras.Web.Outpatient.Program.VisitList" %>

<%@ Register Src="~/Libs/Controls/PatientGrid/GridPatientVisitOutpatientCtl.ascx"
    TagName="ctlGrdRegisteredOutpatientPatient" TagPrefix="uc1" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="QIS.Medinfras.Web.CustomControl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"
    Namespace="QIS.Medinfras.Web.CustomControl" TagPrefix="qis" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="plhMPMain" runat="server">
    <script type="text/javascript">
        $(function () {
            setRightPanelButtonEnabled();
            setDatePicker('<%=txtRegistrationDate.ClientID %>');
            setDatePicker('<%=txtRegistrationDatePatientCall.ClientID %>');

            $('#<%=txtRegistrationDate.ClientID %>').datepicker('option', 'maxDate', new Date(getDateNow()));
            $('#<%=txtRegistrationDatePatientCall.ClientID %>').datepicker('option', 'maxDate', new Date(getDateNow()));

            $('#<%=txtRegistrationDate.ClientID %>').change(function (evt) {
                onRefreshGridView();
            });
            $('#<%=txtRegistrationDatePatientCall.ClientID %>').change(function (evt) {
                onRefreshGrdPatientCall();
            });

            $('#lblRefresh.lblLink').click(function (evt) {
                onRefreshGridView();
            });

            $('#lblRefreshPatientCall.lblLink').click(function (evt) {
                onRefreshGrdPatientCall();
            });

            onButtonOutpatientInformationShow();

            $('#imgOutpatientInformation').click(function (evt) {
                var url = ResolveUrl('~/Program/PatientList/PatientAppointmentInformationCtl.ascx');
                var registrationDate = $('#<%=txtRegistrationDate.ClientID %>').val();
                var healthcareServiceUnitID = cboServiceUnit.GetValue();
                var physicianID = $('#<%=hdnPhysicianID.ClientID %>').val();
                var queryString = registrationDate + '|' + healthcareServiceUnitID + '|' + physicianID;
                openUserControlPopup(url, queryString, 'Informasi Appointment', 1300, 600);
            });

            $('#<%=grdView.ClientID %> > tbody > tr:gt(0):not(.trEmpty)').live('click', function () {
                $('#<%=grdView.ClientID %> tr.selected').removeClass('selected');
                $(this).addClass('selected');
                $('#<%=hdnID.ClientID %>').val($(this).find('.keyField').html());
                cbpViewDetail.PerformCallback();
            });

            $('#<%=grdView.ClientID %> tr:eq(1)').click();
        });

        function onButtonOutpatientInformationShow() {
            if (($('#<%=txtRegistrationDate.ClientID %>').val() != "" && $('#<%=hdnPhysicianID.ClientID %>').val() != "" && $('#<%:chkIsPreviousEpisodePatient.ClientID %>').is(':unchecked'))) {
                $('#imgOutpatientInformation').removeAttr('style');
                $('#imgClinicServiceTime').removeAttr('style');
            }
            else {
                $('#imgOutpatientInformation').attr('style', 'display:none');
                $('#imgClinicServiceTime').attr('style', 'display:none');
            }
        }

        //#region Physician
        function onGetPhysicianFilterExpression() {
            var filterExpression = "";
            var healthcareServiceUnitID = cboServiceUnit.GetValue();
            if (healthcareServiceUnitID != '0')
                filterExpression = "ParamedicID IN (SELECT ParamedicID FROM ServiceUnitParamedic WHERE HealthcareServiceUnitID = " + healthcareServiceUnitID + ") AND IsDeleted = 0";
            else
                filterExpression = "IsDeleted = 0";
            return filterExpression;
        }

        $('#lblPhysician.lblLink').live('click', function () {
            openSearchDialog('paramedic', onGetPhysicianFilterExpression(), function (value) {
                $('#<%=txtPhysicianCode.ClientID %>').val(value);
                onTxtPhysicianCodeChanged(value);
            });
        });

        $('#<%=txtPhysicianCode.ClientID %>').live('change', function () {
            onTxtPhysicianCodeChanged($(this).val());
        });

        function onTxtPhysicianCodeChanged(value) {
            var filterExpression = onGetPhysicianFilterExpression() + " AND ParamedicCode = '" + value + "'";
            Methods.getObject('GetvParamedicMasterList', filterExpression, function (result) {
                if (result != null) {
                    $('#<%=hdnPhysicianID.ClientID %>').val(result.ParamedicID);
                    $('#<%=txtPhysicianName.ClientID %>').val(result.ParamedicName);
                }
                else {
                    $('#<%=txtPhysicianCode.ClientID %>').val('');
                    $('#<%=hdnPhysicianID.ClientID %>').val('');
                    $('#<%=txtPhysicianName.ClientID %>').val('');
                }
                onButtonOutpatientInformationShow();
                cbpView.PerformCallback('refresh');
            });
        }
        //#endregion

        //#region Physician Patient Call
        function onGetPhysicianPatientCallFilterExpression() {
            var filterExpressionPatientCall = "";
            var healthcareServiceUnitIDPatientCall = cboClinic.GetValue();
            if (healthcareServiceUnitIDPatientCall != '0')
                filterExpressionPatientCall = "ParamedicID IN (SELECT ParamedicID FROM ServiceUnitParamedic WHERE HealthcareServiceUnitID = " + healthcareServiceUnitIDPatientCall + ") AND IsDeleted = 0";
            else
                filterExpressionPatientCall = "IsDeleted = 0";
            return filterExpressionPatientCall;
        }

        $('#lblPhysicianPatientCall.lblLink').live('click', function () {
            openSearchDialog('paramedic', onGetPhysicianPatientCallFilterExpression(), function (value) {
                $('#<%=txtPhysicianCodePatientCall.ClientID %>').val(value);
                onTxtPhysicianCodePatientCallChanged(value);
            });
        });

        $('#<%=txtPhysicianCodePatientCall.ClientID %>').live('change', function () {
            onTxtPhysicianCodePatientCallChanged($(this).val());
        });

        function onTxtPhysicianCodePatientCallChanged(value) {
            var filterExpressionPatientCall = onGetPhysicianPatientCallFilterExpression() + " AND ParamedicCode = '" + value + "'";
            Methods.getObject('GetvParamedicMasterList', filterExpressionPatientCall, function (result) {
                if (result != null) {
                    $('#<%=hdnPhysicianIDPatientCall.ClientID %>').val(result.ParamedicID);
                    $('#<%=txtPhysicianNamePatientCall.ClientID %>').val(result.ParamedicName);
                    setRightPanelButtonEnabled();
                }
                else {
                    $('#<%=txtPhysicianCodePatientCall.ClientID %>').val('');
                    $('#<%=hdnPhysicianIDPatientCall.ClientID %>').val('');
                    $('#<%=txtPhysicianNamePatientCall.ClientID %>').val('');
                }
                cbpViewPatientCall.PerformCallback('refresh');
            });
        }
        //#endregion

        var interval = parseInt('<%=GetRefreshGridInterval() %>') * 60000;

        var intervalID = window.setInterval(function () {
            onRefreshGridView();
        }, interval);

        function onRefreshGridView() {
            if (IsValid(null, 'fsPatientList', 'mpPatientList')) {
                window.clearInterval(intervalID);
                $('#<%=hdnFilterExpressionQuickSearch.ClientID %>').val(txtSearchView.GenerateFilterExpression());
                refreshGrdRegisteredPatient();
                intervalID = window.setInterval(function () {
                    onRefreshGridView();
                }, interval);
            }
        }

        var intervalIDPatientCall = window.setInterval(function () {
            onRefreshGrdPatientCall();
        }, interval);

        function onRefreshGrdPatientCall() {
            if (IsValid(null, 'fsPatientList', 'mpPatientList')) {
                window.clearInterval(intervalIDPatientCall);
                $('#<%=hdnFilterExpressionQuickSearchPatientCall.ClientID %>').val(txtSearchViewPatientCall.GenerateFilterExpression());
                cbpViewPatientCall.PerformCallback('refresh');
                intervalIDPatientCall = window.setInterval(function () {
                    onRefreshGrdReg();
                }, interval);
            }
        }

        function onCboServiceUnitValueChanged() {
            onRefreshGridView();
        }

        function onCboClinicValueChanged() {
            cbpViewPatientCall.PerformCallback('refresh');
        }

        $(function () {
            $('#<%=txtBarcodeEntry.ClientID %>').keypress(function (e) {
                var keyCode = e.keyCode || e.which;
                //////                if (keyCode == 9 || keyCode == 13) {
                //////                    cbpBarcodeEntryProcess.PerformCallback();
                //////                }
            });

            $('#<%:txtBarcodeEntry.ClientID %>').live('change', function () {
                var mrn = FormatMRN($(this).val());
                $('#<%:hdnSearchBarcodeNoRM.ClientID %>').val(mrn);
                refreshGrdRegisteredPatient();
            });
        });

        function onCbpBarcodeEntryProcessEndCallback(s) {
            if (s.cpUrl != '')
                document.location = s.cpUrl;
            else {
                showToast('Warning', 'No RM Tidak Ditemukan', function () {
                    $('#<%=txtBarcodeEntry.ClientID %>').val('');
                });
                hideLoadingPanel();
            }
        }

        function onCbpViewPatientCallEndCallback(s) {
            hideLoadingPanel();
        }

        function onCbpInfoParamedicScheduleDateViewEndCallback(s) {
            hideLoadingPanel();
        }

        function onCbpPatientCheckedInEndCallback(s) {
            if (s.cpUrl != '')
                document.location = s.cpUrl;
        }

        function onTxtSearchViewSearchClick(s) {
            setTimeout(function () {
                s.SetBlur();
                onRefreshGridView();
                setTimeout(function () {
                    s.SetFocus();
                }, 0);
            }, 0);
        }

        function onTxtSearchViewSearchPatientCallClick(s) {
            setTimeout(function () {
                s.SetBlur();
                onRefreshGrdPatientCall()
                setTimeout(function () {
                    s.SetFocus();
                }, 0);
            }, 0);
        }

        $('#<%=chkIsPreviousEpisodePatient.ClientID %>').die();
        $('#<%=chkIsPreviousEpisodePatient.ClientID %>').live('change', function () {
            if ($(this).is(':checked')) {
                $('#<%=txtRegistrationDate.ClientID %>').attr('readonly', 'readonly');
            }
            else $('#<%=txtRegistrationDate.ClientID %>').removeAttr('readonly');
            onRefreshGridView();
            onButtonOutpatientInformationShow();
        });


        $('#<%=chkIsPreviousEpisodePatientCall.ClientID %>').die();
        $('#<%=chkIsPreviousEpisodePatientCall.ClientID %>').live('change', function () {
            if ($(this).is(':checked')) {
                $('#<%=txtRegistrationDatePatientCall.ClientID %>').attr('readonly', 'readonly');
            }
            else $('#<%=txtRegistrationDatePatientCall.ClientID %>').removeAttr('readonly');
            onRefreshGrdPatientCall();
        });

        $('#<%=chkIsIncludeReopenBilling.ClientID %>').die();
        $('#<%=chkIsIncludeReopenBilling.ClientID %>').live('change', function () {
            onRefreshGridView();
            onButtonOutpatientInformationShow();
        });

        //#region tab
        $(function () {
            $('#ulTabLabResult li').click(function () {
                $('#ulTabLabResult li.selected').removeAttr('class');
                $('.containerInfo').filter(':visible').hide();
                $contentID = $(this).attr('contentid');

                if ($contentID == "containerTransaction") {
                    $('#<%=hdnFilterExpressionQuickSearch.ClientID %>').val("");
                    txtSearchView.SetText("");
                    refreshGrdRegisteredPatient();
                }
                else if ($contentID == "containerPatientCall") {
                    $('#<%=hdnIsPatientCallLoaded.ClientID %>').val("1");
                    $('#<%=hdnFilterExpressionQuickSearchPatientCall.ClientID %>').val("");
                    txtSearchViewPatientCall.SetText("");
                    cbpViewPatientCall.PerformCallback('refresh');
                }
                else if ($contentID == "containerClinicServiceStatus") {
                    $('#<%=hdnIsClinicServiceTimeLoaded.ClientID %>').val("1");
                    cbpInfoParamedicScheduleDateView.PerformCallback('refresh');
                }

                $('#' + $contentID).show();
                $(this).addClass('selected');
            });
        });
        //#endregion

        //#region Room Search Dialog
        $('.lblRoomName.lblLink').live('click', function () {
            $tr = $(this).closest('tr').parent().closest('tr');
            var registrationID = $tr.find('.hdnRegistrationID').val();
            var visitID = $tr.find('.hdnVisitID').val();
            var roomID = $tr.find('.hdnRoomID').val();
            openSearchDialog('serviceunitroom', "DepartmentID = 'OUTPATIENT' AND IsDeleted = 0", function (value) {
                onTxtRoomChanged(value, $tr);
            });
        });

        function onTxtRoomChanged(value, $tr) {
            var filterExpression = "RoomCode = '" + value + "'";
            Methods.getObject('GetvServiceUnitRoomList', filterExpression, function (result) {
                if (result != null) {
                    $tr.find('.hdnRoomCode').html(result.RoomCode);
                    $tr.find('.lblRoomName').html(result.RoomName);
                    $tr.find('.hdnRoomID').html(result.RoomID);
                }
            });
        }
        //#endregion

        $('.btnPatientCall').die('click');
        $('.btnPatientCall').live('click', function () {
            $tr = $(this).closest('tr').parent().closest('tr');
            var registrationID = $tr.find('.hdnRegistrationID').val();
            var visitID = $tr.find('.hdnVisitID').val();
            var roomCode = $tr.find('.hdnRoomCode').val();
            cbpViewPatientCall.PerformCallback('call|' + registrationID + '|' + visitID + '|' + roomCode);
        });

        $('.btnCheckIn').die('click');
        $('.btnCheckIn').live('click', function () {
            $tr = $(this).closest('tr').parent().closest('tr');
            var registrationID = $tr.find('.hdnRegistrationID').val();
            var visitID = $tr.find('.hdnVisitID').val();
            var roomID = $tr.find('.hdnRoomID').val();
            if (roomID == 0) {
                roomID = "";
            }
            $('#<%=hdnRoomID.ClientID %>').val(roomID);
            cbpPatientCheckedIn.PerformCallback('checkin|' + registrationID + '|' + visitID);

        });

        $('.imgStart.imgLink').die('click');
        $('.imgStart.imgLink').live('click', function () {
            $tr = $(this).closest('tr').parent().closest('tr');
            var paramedicID = $tr.find('.hdnParamedicID').val();
            var healthcareServiceUnitID = $tr.find('.hdnHealthcareServiceUnitID').val();
            var departmentID = $tr.find('.hdnDepartmentID').val();
            var tempDate = $tr.find('.hdnTempDate').val();
            var operationalTimeID = $tr.find('.hdnOperationalTimeID').val();
            var id = paramedicID + '|' + healthcareServiceUnitID + '|' + departmentID + '|' + tempDate + '|' + operationalTimeID;
            cbpViewDetail.PerformCallback('start|' + id);
        });

        $('.imgPause.imgLink').die('click');
        $('.imgPause.imgLink').live('click', function () {
            $tr = $(this).closest('tr').parent().closest('tr');
            var paramedicID = $tr.find('.hdnParamedicID').val();
            var healthcareServiceUnitID = $tr.find('.hdnHealthcareServiceUnitID').val();
            var departmentID = $tr.find('.hdnDepartmentID').val();
            var tempDate = $tr.find('.hdnTempDate').val();
            var operationalTimeID = $tr.find('.hdnOperationalTimeID').val();
            var id = paramedicID + '|' + healthcareServiceUnitID + '|' + departmentID + '|' + tempDate + '|' + operationalTimeID;
            var url = ResolveUrl("~/Program/Process/ClinicPauseCtl.ascx");
            openUserControlPopup(url, id, 'Tutup Sementara', 500, 150);
        });

        $('.imgStop.imgLink').die('click');
        $('.imgStop.imgLink').live('click', function () {
            $tr = $(this).closest('tr').parent().closest('tr');
            var paramedicID = $tr.find('.hdnParamedicID').val();
            var healthcareServiceUnitID = $tr.find('.hdnHealthcareServiceUnitID').val();
            var departmentID = $tr.find('.hdnDepartmentID').val();
            var tempDate = $tr.find('.hdnTempDate').val();
            var operationalTimeID = $tr.find('.hdnOperationalTimeID').val();
            var id = paramedicID + '|' + healthcareServiceUnitID + '|' + departmentID + '|' + tempDate + '|' + operationalTimeID;
            cbpViewDetail.PerformCallback('stop|' + id);
        });

        function onAfterSaveEditRecordEntryPopup() {
//            cbpViewDetail.PerformCallback('refresh');
            cbpViewPatientCall.PerformCallback('refresh');
        }

        function onAfterPopupControlClosing() {
//            cbpViewDetail.PerformCallback('refresh');
            cbpViewPatientCall.PerformCallback('refresh');
        }

        function setRightPanelButtonEnabled() {
            var physicianID = $('#<%:hdnPhysicianIDPatientCall.ClientID %>').val();
            var healthcareServiceUnitID = cboClinic.GetValue();
            if (physicianID != '' && healthcareServiceUnitID != null) {
                $('#btnChangeRoom').removeAttr('enabled');
            }
            else {
                $('#btnChangeRoom').attr('enabled', 'false');
            }
        }

        function onBeforeLoadRightPanelContent(code) {
            var param = '';
            var physicianID = $('#<%:hdnPhysicianIDPatientCall.ClientID %>').val();
            var healthcareServiceUnitID = cboClinic.GetValue();
            $('#<%=hdnCboClinicValue.ClientID %>').val(cboClinic.GetValue());
            var registrationDate = $('#<%=txtRegistrationDate.ClientID %>').val();

            if (code == 'changeRoom') {
                param = physicianID + '|' + healthcareServiceUnitID + '|' + registrationDate;
            }

            return param
        }

    </script>
    <div style="padding: 15px">
        <input type="hidden" value="" id="hdnID" runat="server" />
        <input type="hidden" value="" id="hdnFilterExpressionQuickSearch" runat="server" />
        <input type="hidden" value="" id="hdnFilterExpressionQuickSearchPatientCall" runat="server" />
        <input type="hidden" value="" id="hdnSearchBarcodeNoRM" runat="server" />
        <input type="hidden" value="" id="hdnQuickText" runat="server" />
        <input type="hidden" value="" id="hdnQuickTextPatientCall" runat="server" />
        <input type="hidden" value="" id="hdnLstHealthcareServiceUnitID" runat="server" />
        <input type="hidden" value="" id="hdnIsBridgingToGateway" runat="server" />
        <input type="hidden" value="" id="hdnProviderGatewayService" runat="server" />
        <input type="hidden" value="" id="hdnQueryString" runat="server" />
        <input type="hidden" value="" id="hdnIsAutomaticallyCheckedIn" runat="server" />
        <input type="hidden" value="" id="hdnIsUsingPatientCall" runat="server" />
        <input type="hidden" value="" id="hdnIsUsingClinicService" runat="server" />
        <input type="hidden" value="" id="hdnIsUsedReopenBilling" runat="server" />
        <input type="hidden" value="" id="hdnRoomID" runat="server" />
        <input type="hidden" value="" id="hdnIsControlAdministrationCharges" runat="server" />
        <input type="hidden" value="" id="hdnChargeCodeAdministrationForInstansi" runat="server" />
        <input type="hidden" value="" id="hdnIsControlAdmCost" runat="server" />
        <input type="hidden" value="" id="hdnAdminID" runat="server" />
        <input type="hidden" value="" id="hdnIsControlPatientCardPayment" runat="server" />
        <input type="hidden" value="" id="hdnItemCardFee" runat="server" />
        <input type="hidden" value="0" id="hdnIsPatientCallLoaded" runat="server" />
        <input type="hidden" value="0" id="hdnIsClinicServiceTimeLoaded" runat="server" />
        <input type="hidden" value="0" id="hdnCboClinicValue" runat="server" />
        <input type="hidden" value="0" id="hdnIsBridgingWithMedinlink" runat="server" />
        <input type="hidden" id="hdnIsBridgingToMobileJKN" value="0" runat="server" />
        <div class="containerUlTabPage" style="margin-bottom: 3px;">
            <ul class="ulTabPage" id="ulTabLabResult">
                <li contentid="containerTransaction" id="patientTransactionList" runat="server" class="selected">
                    <%= HttpUtility.HtmlEncode(GetMenuCaption())%></li>
                <li contentid="containerPatientCall" id="patientCall" runat="server">
                    <%=GetLabel("Pasien Belum Dilayani")%></li>
                <li contentid="containerClinicServiceStatus" id="clinicServiceStatus" runat="server">
                    <%=GetLabel("Proses Jam Pelayanan Klinik")%></li>
                <%--                <li contentid="containerServiceOrder" id="clinicServiceOrder" runat="server">
                    <%=GetLabel("Order dari Unit Lain")%></li>--%>
            </ul>
        </div>
        <div id="containerTransaction" class="containerInfo">
            <div class="pageTitle">
                <%= HttpUtility.HtmlEncode(GetMenuCaption())%>
                :
                <%=GetLabel("Pilih Pasien")%></div>
            <table class="tblContentArea" style="width: 100%">
                <tr>
                    <td style="padding: 5px; vertical-align: top">
                        <fieldset id="fsPatientList">
                            <table class="tblEntryContent" style="width: 60%;">
                                <colgroup>
                                    <col style="width: 150px" />
                                    <col style="width: 150px" />
                                    <col />
                                </colgroup>
                                <tr>
                                    <td class="tdLabel">
                                        <label class="lblNormal">
                                            <%=GetLabel("Tanggal Registrasi")%></label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtRegistrationDate" Width="120px" runat="server" CssClass="datepicker" />
                                    </td>
                                    <td>
                                        <asp:CheckBox ID="chkIsPreviousEpisodePatient" runat="server" Checked="false" Text="Abaikan Tanggal" />
                                    </td>
                                    <td>
                                        <div style="float: right">
                                            <img class="lblLink" id="imgOutpatientInformation" src='<%= ResolveUrl("~/Libs/Images/Button/package.png")%>'
                                                alt="" title="Informasi Appointment" />
                                        </div>
                                    </td>
                                </tr>
                                <tr id="trFilterReopenBilling" runat="server">
                                    <td>
                                    </td>
                                    <td>
                                    </td>
                                    <td align="left">
                                        <asp:CheckBox ID="chkIsIncludeReopenBilling" runat="server" Checked="false" Text="Khusus Reopen Billing" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel">
                                        <label class="lblNormal">
                                            <%=GetLabel("Klinik")%></label>
                                    </td>
                                    <td colspan="2">
                                        <dxe:ASPxComboBox ID="cboServiceUnit" ClientInstanceName="cboServiceUnit" Width="100%"
                                            runat="server">
                                            <ClientSideEvents ValueChanged="function(s,e) { onCboServiceUnitValueChanged(); }" />
                                        </dxe:ASPxComboBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel">
                                        <label class="lblLink" id="lblPhysician">
                                            <%=GetLabel("Dokter / Tenaga Medis")%></label>
                                    </td>
                                    <td colspan="2">
                                        <input type="hidden" id="hdnPhysicianID" runat="server" value="" />
                                        <table style="width: 100%" cellpadding="0" cellspacing="0">
                                            <colgroup>
                                                <col style="width: 120px" />
                                                <col style="width: 3px" />
                                                <col />
                                            </colgroup>
                                            <tr>
                                                <td>
                                                    <asp:TextBox ID="txtPhysicianCode" Width="120px" runat="server" />
                                                </td>
                                                <td>
                                                    &nbsp;
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtPhysicianName" ReadOnly="true" Width="99%" runat="server" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel">
                                        <label>
                                            <%=GetLabel("Quick Filter")%></label>
                                    </td>
                                    <td colspan="2">
                                        <qis:QISIntellisenseTextBox runat="server" ClientInstanceName="txtSearchView" ID="txtSearchView"
                                            Width="560px" Watermark="Search">
                                            <ClientSideEvents SearchClick="function(s){ onTxtSearchViewSearchClick(s); }" />
                                            <IntellisenseHints>
                                                <qis:QISIntellisenseHint Text="Nama Pasien" FieldName="PatientName" />
                                                <qis:QISIntellisenseHint Text="No Registrasi" FieldName="RegistrationNo" />
                                                <qis:QISIntellisenseHint Text="No.RM" FieldName="MedicalNo" />
                                                <qis:QISIntellisenseHint Text="Penjamin Bayar" FieldName="BusinessPartnerName" />
                                            </IntellisenseHints>
                                        </qis:QISIntellisenseTextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel">
                                        <label class="lblNormal">
                                            <%=GetLabel("No. Rekam Medis")%></label>
                                    </td>
                                    <td colspan="2">
                                        <asp:TextBox ID="txtBarcodeEntry" Width="120px" runat="server" />
                                    </td>
                                </tr>
                            </table>
                        </fieldset>
                        <div style="padding: 7px 0 0 3px; font-size: 0.95em">
                            <%=GetLabel("Halaman Ini Akan")%>
                            <span class="lblLink" id="lblRefresh">[refresh]</span>
                            <%=GetLabel("Setiap")%>
                            <%=GetRefreshGridInterval() %>
                            <%=GetLabel("Menit")%>
                        </div>
                        <uc1:ctlGrdRegisteredOutpatientPatient runat="server" ID="grdRegisteredOutpatientPatient" />
                    </td>
                </tr>
            </table>
        </div>
        <div id="containerPatientCall" class="containerInfo" style="display: none">
            <style type="text/css">
                .ulBed
                {
                    margin: 0;
                    padding: 0;
                }
                .ulBed li
                {
                    display: inline-block;
                    border-radius: 5px;
                    list-style-type: none;
                    width: 275px;
                    height: 135px;
                    margin: 0 3px;
                    padding: 5px;
                }
                
                .ulFooter li
                {
                    display: inline-block;
                    border-radius: 2px;
                    list-style-type: none;
                    min-width: 75px;
                    height: 15px;
                    margin: 0 10px;
                    padding: 5px;
                    font-size: 11px;
                }
                .genderStyle
                {
                    font-size: 11px;
                }
                
                .fontCustom
                {
                    font-size: 12px;
                }
                
                .trGenderM
                {
                    background-color: blue;
                }
                .trGenderF
                {
                    background-color: #FF69B4;
                }
                .liBedStatusU
                {
                    background-color: #A1A4A6;
                }
                .liBedStatusW
                {
                    background-color: #DEEC83;
                }
                .liBedStatusH
                {
                    background-color: #B3A360;
                }
                .liBedStatusI
                {
                    background-color: #F8C299;
                }
                .liBedStatusO
                {
                    background-color: #4ac5e3;
                }
                .liBedStatusCo
                {
                    background-color: #E7B4DE;
                }
                .liBedStatusB
                {
                    background-color: #f1f262;
                }
                .liBedStatusOM
                {
                    background-color: #4ac5e3;
                }
                .liBedStatusOF
                {
                    background-color: #ffbdde;
                }
                
                .ulTab
                {
                    margin: 0;
                    padding: 0;
                }
                .ulTab li
                {
                    list-style-type: none;
                    width: 100px;
                    height: 40px;
                    margin: 0 10px;
                    padding: 5px;
                }
                .TabContent
                {
                    background-color: #F8C299;
                }
            </style>
            <div class="pageTitle">
                <%=GetLabel("Daftar Pasien yang belum dilayani")%></div>
            <table class="tblContentArea" style="width: 100%">
                <tr>
                    <td colspan="2">
                        <div id="divShow1" class="divShow">
                            <table class="tblContentArea" style="width: 100%">
                                <tr>
                                    <td style="padding: 5px; vertical-align: top">
                                        <fieldset id="Fieldset1">
                                            <table class="tblEntryContent" style="width: 60%;">
                                                <tr>
                                                    <td class="tdLabel">
                                                        <label class="lblNormal">
                                                            <%=GetLabel("Tanggal Registrasi")%></label>
                                                    </td>
                                                    <td>
                                                        <table style="width: 100%" cellpadding="0" cellspacing="0">
                                                            <colgroup>
                                                                <col style="width: 145px" />
                                                                <col style="width: 5x" />
                                                                <col />
                                                            </colgroup>
                                                            <tr>
                                                                <td>
                                                                    <asp:TextBox ID="txtRegistrationDatePatientCall" Width="120px" runat="server" CssClass="datepicker" />
                                                                </td>
                                                                <td>
                                                                    &nbsp;
                                                                </td>
                                                                <td>
                                                                    <asp:CheckBox ID="chkIsPreviousEpisodePatientCall" runat="server" Checked="false"
                                                                        Text="Abaikan Tanggal" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr id="trPatientCall" runat="server">
                                                    <td class="tdLabel" style="width: 150px">
                                                        <label class="lblNormal">
                                                            <%=GetLabel("Klinik")%></label>
                                                    </td>
                                                    <td>
                                                        <dxe:ASPxComboBox ID="cboClinic" ClientInstanceName="cboClinic" runat="server" Width="250px">
                                                            <ClientSideEvents ValueChanged="function(s,e) { onCboClinicValueChanged(); }" />
                                                        </dxe:ASPxComboBox>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="tdLabel">
                                                        <label class="lblLink" id="lblPhysicianPatientCall">
                                                            <%=GetLabel("Dokter / Tenaga Medis")%></label>
                                                    </td>
                                                    <td>
                                                        <input type="hidden" id="hdnPhysicianIDPatientCall" runat="server" value="" />
                                                        <table style="width: 100%" cellpadding="0" cellspacing="0">
                                                            <colgroup>
                                                                <col style="width: 120px" />
                                                                <col style="width: 3px" />
                                                                <col />
                                                            </colgroup>
                                                            <tr>
                                                                <td>
                                                                    <asp:TextBox ID="txtPhysicianCodePatientCall" Width="120px" runat="server" />
                                                                </td>
                                                                <td>
                                                                    &nbsp;
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtPhysicianNamePatientCall" ReadOnly="true" Width="150px" runat="server" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="tdLabel">
                                                        <label>
                                                            <%=GetLabel("Quick Filter")%></label>
                                                    </td>
                                                    <td colspan="2">
                                                        <qis:QISIntellisenseTextBox runat="server" ClientInstanceName="txtSearchViewPatientCall"
                                                            ID="txtSearchViewPatientCall" Width="560px" Watermark="Search">
                                                            <ClientSideEvents SearchClick="function(s){ onTxtSearchViewSearchPatientCallClick(s); }" />
                                                            <IntellisenseHints>
                                                                <qis:QISIntellisenseHint Text="Nama Pasien" FieldName="PatientName" />
                                                                <qis:QISIntellisenseHint Text="No Registrasi" FieldName="RegistrationNo" />
                                                                <qis:QISIntellisenseHint Text="No.RM" FieldName="MedicalNo" />
                                                            </IntellisenseHints>
                                                        </qis:QISIntellisenseTextBox>
                                                    </td>
                                                </tr>
                                            </table>
                                        </fieldset>
                                        <div style="padding: 7px 0 0 3px; font-size: 0.95em">
                                            <%=GetLabel("Halaman Ini Akan")%>
                                            <span class="lblLink" id="lblRefreshPatientCall">[refresh]</span>
                                            <%=GetLabel("Setiap")%>
                                            <%=GetRefreshGridInterval() %>
                                            <%=GetLabel("Menit")%>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <dxcp:ASPxCallbackPanel ID="cbpViewPatientCall" runat="server" Width="100%" ClientInstanceName="cbpViewPatientCall"
                                            OnCallback="cbpViewPatientCall_Callback" ShowLoadingPanel="false">
                                            <ClientSideEvents BeginCallback="function(s,e) { showLoadingPanel(); }" EndCallback="function(s,e) { onCbpViewPatientCallEndCallback(); }" />
                                            <PanelCollection>
                                                <dx:PanelContent ID="PanelContent3" runat="server">
                                                    <asp:Panel runat="server" ID="pnlGridView" Style="width: 100%; margin-left: auto;
                                                        margin-right: auto; position: relative; font-size: 0.95em; height: 500px; overflow-y: scroll;">
                                                        <table id="Table1" class="grdSelected grdPatientPage" cellspacing="0" rules="all">
                                                            <asp:ListView ID="lstViewPatientCall" runat="server">
                                                                <EmptyDataTemplate>
                                                                    <table id="tblViewPatientCall" runat="server" class="grdCollapsible lstViewPatientCall"
                                                                        cellspacing="0" rules="all">
                                                                        <tr>
                                                                            <th style="width: 150px" rowspan="2" align="Left">
                                                                                <%=GetLabel("No. Registrasi")%>
                                                                            </th>
                                                                            <th style="width: 450px" rowspan="2" align="Left">
                                                                                <%=GetLabel("Nama Pasien")%>
                                                                            </th>
                                                                            <th style="width: 150px" colspan="6" align="Center">
                                                                                <%=GetLabel("Info Kunjungan")%>
                                                                            </th>
                                                                            <th rowspan="2" align="Left">
                                                                                <%=GetLabel("")%>
                                                                            </th>
                                                                        </tr>
                                                                        <tr>
                                                                            <th style="width: 70px" align="center">
                                                                                <%=GetLabel("Sesi")%>
                                                                            </th>
                                                                            <th style="width: 70px" align="center">
                                                                                <%=GetLabel("Antrian")%>
                                                                            </th>
                                                                            <th style="width: 70px" align="center">
                                                                                <%=GetLabel("No. Tiket")%>
                                                                            </th>
                                                                            <th style="width: 300px" align="Left">
                                                                                <%=GetLabel("Dokter DPJP")%>
                                                                            </th>
                                                                            <th style="width: 300px" align="Left">
                                                                                <%=GetLabel("Klinik")%>
                                                                            </th>
                                                                            <th style="width: 300px" align="Left">
                                                                                <%=GetLabel("Kamar")%>
                                                                            </th>
                                                                        </tr>
                                                                        <tr class="trEmpty">
                                                                            <td colspan="9">
                                                                                <%=GetLabel("Tidak ada data pasien yang belum dilayani") %>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </EmptyDataTemplate>
                                                                <LayoutTemplate>
                                                                    <table id="tblViewPatientCall" runat="server" class="grdCollapsible lstViewPatientCall"
                                                                        cellspacing="0" rules="all">
                                                                        <tr>
                                                                            <th style="width: 150px" rowspan="2" align="Left">
                                                                                <%=GetLabel("No. Registrasi")%>
                                                                            </th>
                                                                            <th style="width: 450px" rowspan="2" align="Left">
                                                                                <%=GetLabel("Nama Pasien")%>
                                                                            </th>
                                                                            <th style="width: 150px" colspan="6" align="Center">
                                                                                <%=GetLabel("Info Kunjungan")%>
                                                                            </th>
                                                                            <th style="width: 200px" rowspan="2" colspan="3" align="center">
                                                                                <%=GetLabel("")%>
                                                                            </th>
                                                                        </tr>
                                                                        <tr>
                                                                            <th style="width: 70px" align="center">
                                                                                <%=GetLabel("Sesi")%>
                                                                            </th>
                                                                            <th style="width: 70px" align="center">
                                                                                <%=GetLabel("Antrian")%>
                                                                            </th>
                                                                            <th style="width: 70px" align="center">
                                                                                <%=GetLabel("No. Tiket")%>
                                                                            </th>
                                                                            <th style="width: 300px" align="center">
                                                                                <%=GetLabel("Dokter DPJP")%>
                                                                            </th>
                                                                            <th style="width: 300px" align="center">
                                                                                <%=GetLabel("Klinik")%>
                                                                            </th>
                                                                            <th style="width: 300px" align="center">
                                                                                <%=GetLabel("Kamar")%>
                                                                            </th>
                                                                        </tr>
                                                                        <tr runat="server" id="itemPlaceholder">
                                                                        </tr>
                                                                    </table>
                                                                </LayoutTemplate>
                                                                <ItemTemplate>
                                                                    <tr>
                                                                        <td>
                                                                            <%#: Eval("RegistrationNo")%>
                                                                        </td>
                                                                        <td align="left">
                                                                            <%#: Eval("PatientName")%>
                                                                        </td>
                                                                        <td align="center">
                                                                            <%#: Eval("Session")%>
                                                                        </td>
                                                                        <td align="center">
                                                                            <%#: Eval("QueueNo")%>
                                                                        </td>
                                                                        <td align="center">
                                                                            <%#: Eval("RegistrationTicketNo")%>
                                                                        </td>
                                                                        <td align="center">
                                                                            <%#: Eval("ParamedicName")%>
                                                                        </td>
                                                                        <td align="center">
                                                                            <%#: Eval("ServiceUnitName")%>
                                                                        </td>
                                                                        <td align="center">
                                                                            <%#: Eval("RoomName")%>
                                                                        </td>
                                                                        <td>
                                                                            <table border="0" cellpadding="0" cellspacing="0">
                                                                                <tr>
                                                                                    <td>
                                                                                        <input type="button" value='Panggil' class="btnPatientCall w3-btn w3-hover-blue"
                                                                                            value="Panggil" style="background-color: Red; color: White; width: 100px;" />
                                                                                    </td>
                                                                                    <td>
                                                                                        <input type="button" value='Check-In' class="btnCheckIn w3-btn w3-hover-blue" value="Check-In"
                                                                                            style="background-color: Green; color: White; width: 100px;" />
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </td>
                                                                        <input type="hidden" value='<%#:Eval("RegistrationID") %>' class="hdnRegistrationID" />
                                                                        <input type="hidden" value='<%#:Eval("VisitID") %>' class="hdnVisitID" />
                                                                        <input type="hidden" value='<%#:Eval("MRN") %>' class="hdnMRN" />
                                                                        <input type="hidden" value='<%#:Eval("HealthcareServiceUnitID") %>' class="hdnHealthcareServiceUnitID" />
                                                                        <input type="hidden" value='<%#:Eval("RoomID") %>' class="hdnRoomID" />
                                                                    </tr>
                                                                </ItemTemplate>
                                                            </asp:ListView>
                                                        </table>
                                                    </asp:Panel>
                                                </dx:PanelContent>
                                            </PanelCollection>
                                        </dxcp:ASPxCallbackPanel>
                                        <div class="imgLoadingGrdView" id="containerImgLoadingView">
                                            <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </td>
                </tr>
            </table>
        </div>
        <div id="containerClinicServiceStatus" class="containerInfo" style="display: none">
            <style type="text/css">
                .tdEmptyScheduleDate
                {
                    background-color: #AAA;
                }
            </style>
            <div class="pageTitle">
                <%=GetLabel("Proses Jam Pelayanan Klinik")%></div>
            <table style="width: 100%">
                <tr>
                    <td style="width: 100px">
                        <label>
                            <%=GetLabel("Tanggal ") %></label>
                    </td>
                    <td>
                        <asp:TextBox ID="txtScheduleDate" Width="120px" runat="server" CssClass="datepicker"
                            ReadOnly="true" />
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <table style="width: 100%">
                            <colgroup>
                                <col style="width: 25%" />
                                <col style="width: 75%" />
                            </colgroup>
                            <tr>
                                <td style="vertical-align: top">
                                    <dxcp:ASPxCallbackPanel ID="cbpInfoParamedicScheduleDateView" runat="server" Width="100%"
                                        ClientInstanceName="cbpInfoParamedicScheduleDateView" ShowLoadingPanel="false"
                                        OnCallback="cbpInfoParamedicScheduleDateView_Callback">
                                        <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ onCbpInfoParamedicScheduleDateViewEndCallback(s); }" />
                                        <PanelCollection>
                                            <dx:PanelContent ID="PanelContent2" runat="server">
                                                <asp:Panel runat="server" ID="Panel1" CssClass="pnlContainerGridProcessList">
                                                    <asp:GridView ID="grdView" runat="server" CssClass="grdSelected" AutoGenerateColumns="false"
                                                        ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                                        <Columns>
                                                            <asp:BoundField DataField="ServiceUnitID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Left">
                                                                <HeaderTemplate>
                                                                    <div>
                                                                        <%=GetLabel("Klinik")%></div>
                                                                    <div>
                                                                        <%=GetLabel("Kode")%></div>
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <table border="0" cellpadding="0" cellspacing="0">
                                                                        <tr>
                                                                            <td>
                                                                                <div>
                                                                                    <%#: Eval("ServiceUnitName")%></div>
                                                                                <div>
                                                                                    <%#: Eval("ServiceUnitCode")%></div>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                        <EmptyDataTemplate>
                                                            <%=GetLabel("Tidak ada unit pelayanan yang tersedia")%>
                                                        </EmptyDataTemplate>
                                                    </asp:GridView>
                                                </asp:Panel>
                                            </dx:PanelContent>
                                        </PanelCollection>
                                    </dxcp:ASPxCallbackPanel>
                                    <div class="imgLoadingGrdView" id="containerImgLoadingView2">
                                        <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                                    </div>
                                </td>
                                <td style="vertical-align: top">
                                    <div style="position: relative;">
                                        <dxcp:ASPxCallbackPanel ID="cbpViewDetail" runat="server" Width="100%" ClientInstanceName="cbpViewDetail"
                                            OnCallback="cbpViewDetail_Callback" ShowLoadingPanel="false">
                                            <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingViewDetail').show(); }"
                                                EndCallback="function(s,e){ $('#containerImgLoadingViewDetail').hide(); }" />
                                            <PanelCollection>
                                                <dx:PanelContent ID="PanelContent4" runat="server">
                                                    <asp:Panel runat="server" ID="PanelDt" CssClass="pnlContainerGridProcessList">
                                                        <asp:ListView runat="server" ID="lvwView">
                                                            <EmptyDataTemplate>
                                                                <table id="tblView" runat="server" class="grdScheduleDt grdSelected" cellspacing="0"
                                                                    rules="all">
                                                                    <tr>
                                                                        <th class="keyField" rowspan="2">
                                                                            &nbsp;
                                                                        </th>
                                                                        <th rowspan="2" align="left">
                                                                            <%=GetLabel("Dokter")%>
                                                                        </th>
                                                                        <th rowspan="2" style="width: 60px">
                                                                            <%=GetLabel("Ruang")%>
                                                                        </th>
                                                                        <th rowspan="2" style="width: 60px">
                                                                            <%=GetLabel("Sesi Praktek")%>
                                                                        </th>
                                                                        <th colspan="2">
                                                                            <%=GetLabel("WAKTU PELAYANAN")%>
                                                                        </th>
                                                                        <th colspan="4" style="width: 60px">
                                                                            <%=GetLabel("STATUS PASIEN")%>
                                                                        </th>
                                                                        <th rowspan="2">
                                                                            <%=GetLabel("STATUS KLINIK")%>
                                                                        </th>
                                                                        <th rowspan="2" style="width: 60px">
                                                                            <%=GetLabel("ACTION")%>
                                                                        </th>
                                                                    </tr>
                                                                    <tr>
                                                                        <th style="width: 50px" align="center">
                                                                            <%=GetLabel("Mulai")%>
                                                                        </th>
                                                                        <th style="width: 50px" align="center">
                                                                            <%=GetLabel("Selesai")%>
                                                                        </th>
                                                                        <th style="width: 40px" align="right">
                                                                            <%=GetLabel("APP")%>
                                                                        </th>
                                                                        <th style="width: 40px" align="right">
                                                                            <%=GetLabel("REG")%>
                                                                        </th>
                                                                        <th style="width: 40px" align="right">
                                                                            <%=GetLabel("OPEN")%>
                                                                        </th>
                                                                        <th style="width: 40px" align="right">
                                                                            <%=GetLabel("VOID")%>
                                                                        </th>
                                                                    </tr>
                                                                    <tr class="trEmpty">
                                                                        <td colspan="25">
                                                                            <%=GetLabel("Tidak ada informasi Jadwal Dokter")%>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </EmptyDataTemplate>
                                                            <LayoutTemplate>
                                                                <table id="tblView" runat="server" class="grdScheduleDt grdSelected" cellspacing="0"
                                                                    rules="all">
                                                                    <tr>
                                                                        <th class="keyField" rowspan="2">
                                                                            &nbsp;
                                                                        </th>
                                                                        <th rowspan="2" align="left">
                                                                            <%=GetLabel("Dokter")%>
                                                                        </th>
                                                                        <th rowspan="2" style="width: 60px">
                                                                            <%=GetLabel("Ruang")%>
                                                                        </th>
                                                                        <th rowspan="2" style="width: 60px">
                                                                            <%=GetLabel("Sesi Praktek")%>
                                                                        </th>
                                                                        <th colspan="2">
                                                                            <%=GetLabel("WAKTU PELAYANAN")%>
                                                                        </th>
                                                                        <th colspan="4" style="width: 60px">
                                                                            <%=GetLabel("STATUS PASIEN")%>
                                                                        </th>
                                                                        <th rowspan="2" style="width: 60px">
                                                                            <%=GetLabel("STATUS KLINIK")%>
                                                                        </th>
                                                                        <th rowspan="2" style="width: 60px">
                                                                            <%=GetLabel("ACTION")%>
                                                                        </th>
                                                                    </tr>
                                                                    <tr>
                                                                        <th style="width: 50px" align="center">
                                                                            <%=GetLabel("Mulai")%>
                                                                        </th>
                                                                        <th style="width: 50px" align="center">
                                                                            <%=GetLabel("Selesai")%>
                                                                        </th>
                                                                        <th style="width: 40px" align="right">
                                                                            <%=GetLabel("APP")%>
                                                                        </th>
                                                                        <th style="width: 40px" align="right">
                                                                            <%=GetLabel("REG")%>
                                                                        </th>
                                                                        <th style="width: 40px" align="right">
                                                                            <%=GetLabel("OPEN")%>
                                                                        </th>
                                                                        <th style="width: 40px" align="right">
                                                                            <%=GetLabel("VOID")%>
                                                                        </th>
                                                                    </tr>
                                                                    <tr runat="server" id="itemPlaceholder">
                                                                    </tr>
                                                                </table>
                                                            </LayoutTemplate>
                                                            <ItemTemplate>
                                                                <tr>
                                                                    <td class="keyField">
                                                                        <%#: Eval("ParamedicID")%>
                                                                    </td>
                                                                    <td style="display: none">
                                                                        <input type="hidden" value='<%#:Eval("ParamedicID") %>' class="hdnParamedicID" />
                                                                        <input type="hidden" value='<%#:Eval("HealthcareServiceUnitID") %>' class="hdnHealthcareServiceUnitID" />
                                                                        <input type="hidden" value='<%#:Eval("DepartmentID") %>' class="hdnDepartmentID" />
                                                                        <input type="hidden" value='<%#:Eval("TempDate") %>' class="hdnTempDate" />
                                                                        <input type="hidden" value='<%#:Eval("OperationalTimeID") %>' class="hdnOperationalTimeID" />
                                                                        <input type="hidden" value='<%#:Eval("RoomID") %>' class="hdnRoomID" />
                                                                        <input type="hidden" value='<%#:Eval("GCClinicStatus") %>' class="hdnGCClinicStatus"
                                                                            id="hdnGCClinicStatus" runat="server" />
                                                                        <input type="hidden" value='<%#:Eval("ClinicStatus") %>' class="hdnClinicStatus" />
                                                                    </td>
                                                                    <td>
                                                                        <img class="imgPhysicianImage" src='<%# Eval("cfPhysicianImageUrl")%>' alt="" height="55px"
                                                                            width="50px" style="float: left; margin-right: 10px;" />
                                                                        <div>
                                                                            <label class="lblDetail">
                                                                                <%#: Eval("ParamedicName")%></label></div>
                                                                    </td>
                                                                    <td>
                                                                        <%#: Eval("RoomName")%>
                                                                    </td>
                                                                    <td align="right">
                                                                        <%#: Eval("Slot")%>
                                                                    </td>
                                                                    <td align="center">
                                                                        <%#: Eval("StartTime")%>
                                                                    </td>
                                                                    <td align="center">
                                                                        <%#: Eval("EndTime")%>
                                                                    </td>
                                                                    <td align="right">
                                                                        <%#: Eval("CountAP")%>
                                                                    </td>
                                                                    <td align="right">
                                                                        <%#: Eval("CountAP_Complete")%>
                                                                    </td>
                                                                    <td align="right">
                                                                        <%#: Eval("CountAP_UnComplete")%>
                                                                    </td>
                                                                    <td align="right">
                                                                        <%#: Eval("CountAP_Void")%>
                                                                    </td>
                                                                    <td align="right">
                                                                        <%#: Eval("ClinicStatus")%>
                                                                    </td>
                                                                    <td align="center">
                                                                        <table cellpadding="1" cellspacing="0">
                                                                            <tr>
                                                                                <td>
                                                                                    <img class="imgStart imgLink" id="imgStart" title='<%=GetLabel("Start")%>' src='<%# ResolveUrl("~/Libs/Images/Status/start.png")%>'
                                                                                        alt="" style="float: left" width="24px" height="24px" />
                                                                                </td>
                                                                                <td style="width: 1px">
                                                                                    <img class="imgPause imgLink" id="imgPause" title='<%=GetLabel("Pause")%>' src='<%# ResolveUrl("~/Libs/Images/Status/pause.png")%>'
                                                                                        alt="" style="float: left" width="24px" height="24px" />
                                                                                </td>
                                                                                <td>
                                                                                    <img class="imgStop imgLink" id="imgStop" title='<%=GetLabel("Stop")%>' src='<%# ResolveUrl("~/Libs/Images/Status/stop_service.png")%>'
                                                                                        alt="" style="float: left" width="24px" height="24px" />
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                </tr>
                                                            </ItemTemplate>
                                                        </asp:ListView>
                                                    </asp:Panel>
                                                </dx:PanelContent>
                                            </PanelCollection>
                                        </dxcp:ASPxCallbackPanel>
                                        <div class="imgLoadingGrdView" id="containerImgLoadingViewDetail">
                                            <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                                        </div>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </div>
        <div style="display: none">
            <dxcp:ASPxCallbackPanel ID="cbpBarcodeEntryProcess" runat="server" Width="100%" ClientInstanceName="cbpBarcodeEntryProcess"
                ShowLoadingPanel="false" OnCallback="cbpBarcodeEntryProcess_Callback">
                <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ onCbpBarcodeEntryProcessEndCallback(s); }" />
            </dxcp:ASPxCallbackPanel>
        </div>
        <div style="display: none">
            <dxcp:ASPxCallbackPanel ID="cbpPatientCheckedIn" runat="server" Width="100%" ClientInstanceName="cbpPatientCheckedIn"
                ShowLoadingPanel="false" OnCallback="cbpPatientCheckedIn_Callback">
                <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ onCbpPatientCheckedInEndCallback(s); }" />
            </dxcp:ASPxCallbackPanel>
        </div>
        <script type="text/javascript">
            $(function () {
                txtSearchView.SetText($('#<%=hdnQuickText.ClientID %>').val());
                txtSearchViewPatientCall.SetText($('#<%=hdnQuickTextPatientCall.ClientID %>').val());
            });
        </script>
    </div>
</asp:Content>
