<%@ Page Title="" Language="C#" MasterPageFile="~/libs/MasterPage/MPTrx.master" AutoEventWireup="true"
    CodeBehind="AppointmentGenerateListDiagnostic.aspx.cs" Inherits="QIS.Medinfras.Web.CommonLibs.Program.AppointmentGenerateListDiagnostic" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="QIS.Medinfras.Web.CustomControl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"
    Namespace="QIS.Medinfras.Web.CustomControl" TagPrefix="qis" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<asp:Content ID="Content3" ContentPlaceHolderID="plhCustomButtonToolbar" runat="server">
    <li id="btnRefresh" crudmode="R" runat="server">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbrefresh.png")%>' alt="" /><br style="clear: both" />
        <div>
            <%=GetLabel("Refresh")%></div>
    </li>
    <li id="btnUploadPatientList" crudmode="R" runat="server">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbupload.png")%>' alt="" /><br style="clear: both" />
        <div>
            <%=GetLabel("Upload")%></div>
    </li>
    <li id="btnPrintLabel" crudmode="R" runat="server" style="display:none">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbprint.png")%>' alt="" /><br style="clear: both" />
        <div>
            <%=GetLabel("Direct Print")%></div>
    </li>
    <li id="btnApproveAppointment" crudmode="R" runat="server">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbprocess.png")%>' alt="" /><br style="clear: both" />
        <div>
            <%=GetLabel("Proses")%></div>
    </li>
    <li id="btnVoidProcess" crudmode="R" runat="server">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbvoid.png")%>' alt="" /><br style="clear: both" />
        <div>
            <%=GetLabel("Batal")%></div>
    </li>
    <li id="btnChangePhysician" crudmode="R" runat="server">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbchangeparamedic.png")%>' alt="" /><br style="clear: both" />
        <div>
            <%=GetLabel("Ubah Dokter")%></div>
    </li>
    <li id="btnChangePayer" crudmode="R" runat="server">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbchangebusinesspartner.png")%>' alt="" /><br style="clear: both" />
        <div>
            <%=GetLabel("Ubah Penjamin")%></div>
    </li>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div class="menuTitle">
        <%=HttpUtility.HtmlEncode(GetPageTitle())%></div>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript" src='<%= ResolveUrl("~/Libs/Scripts/CustomGridViewList.js")%>'></script>
    <script type="text/javascript">
        $(function () {
            getCboPayerValueCompany();
            $('#<%=lvwView.ClientID %> tr:gt(0)').each(function () {
                $txtAppointmentDate.val('<%=DateTimeNowDatePicker() %>');
            });
            $('#<%=hdnSelectedTab.ClientID %>').val("containerUpload");

            $('.txtAppointmentDate').each(function () {
                setDatePickerElement($(this));
                $(this).datepicker('option', 'minDate', '0');
            });

            $('.txtAppointmentDate').each(function () {
                setDatePickerElement($(this));
                $(this).datepicker('option', 'minDate', '0');
            });

            var grd = new customGridView();

            setDatePicker('<%=txtFromAppointmentRequestDate.ClientID %>');
            $('#<%=txtFromAppointmentRequestDate.ClientID %>').datepicker('option', '0');
            setDatePicker('<%=txtToAppointmentRequestDate.ClientID %>');
            $('#<%=txtToAppointmentRequestDate.ClientID %>').datepicker('option', '0');
            setDatePicker('<%=txtFromAppointmentDateApm.ClientID %>');
            $('#<%=txtFromAppointmentDateApm.ClientID %>').datepicker('option', '0');
            setDatePicker('<%=txtFromAppointmentDateReg.ClientID %>');
            $('#<%=txtFromAppointmentDateReg.ClientID %>').datepicker('option', '0');

            $(window).keydown(function (event) {
                if (event.keyCode == 13) {
                    event.preventDefault();
                    return false;
                }
            });

            onLoadAppointmentRequest();
        });

        function onLoadAppointmentRequest() {
            $('#chkSelectAll').die('change');
            $('#chkSelectAll').live('change', function () {
                var isChecked = $(this).is(":checked");
                $('.chkIsSelected input').each(function () {
                    $(this).prop('checked', isChecked);
                    $(this).change();
                });
            });

            $('#chkSelectAllApm').die('change');
            $('#chkSelectAllApm').live('change', function () {
                var isChecked = $(this).is(":checked");
                $('.chkIsSelectedApm input').each(function () {
                    $(this).prop('checked', isChecked);
                    $(this).change();
                });
            });

            $('#chkSelectAllReg').die('change');
            $('#chkSelectAllReg').live('change', function () {
                var isChecked = $(this).is(":checked");
                $('.chkIsSelectedReg input').each(function () {
                    $(this).prop('checked', isChecked);
                    $(this).change();
                });
            });
        }

        //#region tab
        $(function () {
            $('#ulTabLabResult li').click(function () {
                $('#ulTabLabResult li.selected').removeAttr('class');
                $('.containerInfo').filter(':visible').hide();
                $contentID = $(this).attr('contentid');
                $('#<%=hdnSelectedTab.ClientID %>').val($contentID);
                if ($contentID == "containerUpload") {
                    cbpView.PerformCallback('refresh|');
                    $('#<%=btnApproveAppointment.ClientID %>').removeAttr('style', 'display:none');
                    $('#<%=btnVoidProcess.ClientID %>').removeAttr('style', 'display:none');
                    $('#<%=btnChangePhysician.ClientID %>').removeAttr('style', 'display:none');
                    $('#<%=btnChangePayer.ClientID %>').removeAttr('style', 'display:none');
                    $('#<%=btnPrintLabel.ClientID %>').attr('style', 'display:none');

                    $('#<%=containerUpload.ClientID %>').removeAttr('style', 'display:none');
                    $('#<%=containerAppointment.ClientID %>').attr('style', 'display:none');
                    $('#<%=containerRegistration.ClientID %>').attr('style', 'display:none');
                }
                else if ($contentID == "containerAppointment") {
                    cbpViewApm.PerformCallback('refresh|');
                    $('#<%=btnApproveAppointment.ClientID %>').removeAttr('style', 'display:none');
                    $('#<%=btnVoidProcess.ClientID %>').attr('style', 'display:none');
                    $('#<%=btnChangePhysician.ClientID %>').attr('style', 'display:none');
                    $('#<%=btnChangePayer.ClientID %>').attr('style', 'display:none');
                    $('#<%=btnPrintLabel.ClientID %>').attr('style', 'display:none');

                    $('#<%=containerUpload.ClientID %>').attr('style', 'display:none');
                    $('#<%=containerAppointment.ClientID %>').removeAttr('style', 'display:none');
                    $('#<%=containerRegistration.ClientID %>').attr('style', 'display:none');
                }
                else if ($contentID == "containerRegistration") {
                    cbpViewReg.PerformCallback('refresh|');
                    $('#<%=btnApproveAppointment.ClientID %>').attr('style', 'display:none');
                    $('#<%=btnVoidProcess.ClientID %>').attr('style', 'display:none');
                    $('#<%=btnChangePhysician.ClientID %>').attr('style', 'display:none');
                    $('#<%=btnChangePayer.ClientID %>').attr('style', 'display:none');
                    $('#<%=btnPrintLabel.ClientID %>').removeAttr('style', 'display:none');

                    $('#<%=containerUpload.ClientID %>').attr('style', 'display:none');
                    $('#<%=containerAppointment.ClientID %>').attr('style', 'display:none');
                    $('#<%=containerRegistration.ClientID %>').removeAttr('style', 'display:none');
                }

                $('#' + $contentID).show();
                $(this).addClass('selected');
            });
        });
        //#endregion

        $('#<%=btnRefresh.ClientID %>').click(function () {
            var tab = $('#<%=hdnSelectedTab.ClientID %>').val();
            if (tab == "containerUpload") {
                cbpView.PerformCallback('refresh|');
            }
            else if (tab == "containerAppointment") {
                cbpViewApm.PerformCallback('refresh|');
            }
            else if (tab == "containerRegistration") {
                cbpViewReg.PerformCallback('refresh|');
            }
        });

        $('#<%=btnUploadPatientList.ClientID %>').click(function () {
            var departmentID = $('#<%=hdnDepartmentID.ClientID %>').val();
            var param = departmentID + '|';
            var url = ResolveUrl("~/Libs/Program/Module/PatientManagement/Appointment/AppointmentUploadExcelCtl.ascx");
            openUserControlPopup(url, param, 'Upload Patient List', 1200, 500);
        });

        $('#<%=btnChangePhysician.ClientID %>').click(function () {
            var tab = $('#<%=hdnSelectedTab.ClientID %>').val();
            if (tab == "containerUpload") {
                getSelected();
                var apmReqID = $('#<%=hdnParam.ClientID %>').val();
                if (apmReqID != '') {
                    var filter = "HealthcareServiceUnitID = '<%:GetMCUHealthcareServiceUnitID()%>'";
                    openSearchDialog('serviceUnitParamedicMaster', filter, function (value) {
                        var filterExpression = "ParamedicID = '" + value + "'";
                        Methods.getObject('GetvParamedicMasterList', filterExpression, function (result) {
                            if (result != null) {
                                cbpView.PerformCallback('update|multi|paramedic|' + apmReqID + '|' + result.ParamedicID);
                            }
                        });
                    });
                }
                else {
                    displayMessageBox("WARNING", "Harap pilih permintaan yang akan diubah dokternya");
                }
            }
        });

        $('#<%=btnChangePayer.ClientID %>').click(function () {
            var tab = $('#<%=hdnSelectedTab.ClientID %>').val();
            if (tab == "containerUpload") {
                getSelected();
                var apmReqID = $('#<%=hdnParam.ClientID %>').val();
                if (apmReqID != '') {
                    var url = ResolveUrl("~/Libs/Program/Module/PatientManagement/Appointment/AppointmentRequestChangePayerDiagnosticCtl.ascx");
                    var id = 'multi|' + apmReqID;
                    openUserControlPopup(url, id, 'Ubah Tipe Penjamin', 800, 350);
                }
                else {
                    displayMessageBox("WARNING", "Harap pilih permintaan yang akan diubah penjamin bayarnya");
                }
            }
        });

        //#region get selected member
        function getSelected() {
            var param = '';
            var lstParamedicID = '';
            var lstAppDate = '';
            var lstCustomerType = '';
            var lstCorporateAccountNo = '';
            var lstCorporateAccountName = '';
            $('.chkIsSelected input:checked').each(function () {
                var trxID = $(this).closest('tr').find('.hdnKeyField').val();
                var paramedicID = $(this).closest('tr').find('.hdnParamedicID').val();
                var appDate = $(this).closest('tr').find('.txtAppointmentDate').val();
                var customerType = $(this).closest('tr').find('.hdnGCCustomerType').val();
                var corporateAccountNo = $(this).closest('tr').find('.hdnCorporateAccountNo').val();
                var corporateAccountName = $(this).closest('tr').find('.hdnCorporateAccountName').val();
                if (param != '')
                    param += ',';
                param += trxID;
                if (lstParamedicID != '')
                    lstParamedicID += ',';
                lstParamedicID += paramedicID;
                if (lstAppDate != '')
                    lstAppDate += ',';
                lstAppDate += appDate;
                if (lstCustomerType != '')
                    lstCustomerType += ',';
                lstCustomerType += customerType;
                if (lstCorporateAccountNo != '')
                    lstCorporateAccountNo += ',';
                lstCorporateAccountNo += corporateAccountNo;
                if (lstCorporateAccountName != '')
                    lstCorporateAccountName += ',';
                lstCorporateAccountName += corporateAccountName;
            });
            $('#<%=hdnParam.ClientID %>').val(param);
            $('#<%=hdnLstParamedicID.ClientID %>').val(lstParamedicID);
            $('#<%=hdnLstApmDate.ClientID %>').val(lstAppDate);
            $('#<%=hdnLstCustomerType.ClientID %>').val(lstCustomerType);
        };

        function getSelectedApm() {
            var param = '';
            var lstParamedicID = '';
            var lstAppDate = '';
            var lstCustomerType = '';
            var lstCorporateAccountNo = '';
            var lstCorporateAccountName = '';
            $('.chkIsSelectedApm input:checked').each(function () {
                var trxID = $(this).closest('tr').find('.hdnKeyFieldApm').val();
                var paramedicID = $(this).closest('tr').find('.hdnParamedicIDApm').val();
                var appDate = $(this).closest('tr').find('.txtAppointmentDateApm').val();
                var customerType = $(this).closest('tr').find('.hdnGCCustomerTypeApm').val();
                var corporateAccountNo = $(this).closest('tr').find('.hdnCorporateAccountNoApm').val();
                var corporateAccountName = $(this).closest('tr').find('.hdnCorporateAccountNameApm').val();
                if (param != '')
                    param += ',';
                param += trxID;
                if (lstParamedicID != '')
                    lstParamedicID += ',';
                lstParamedicID += paramedicID;
                if (lstAppDate != '')
                    lstAppDate += ',';
                lstAppDate += appDate;
                if (lstCustomerType != '')
                    lstCustomerType += ',';
                lstCustomerType += customerType;
                if (lstCorporateAccountNo != '')
                    lstCorporateAccountNo += ',';
                lstCorporateAccountNo += corporateAccountNo;
                if (lstCorporateAccountName != '')
                    lstCorporateAccountName += ',';
                lstCorporateAccountName += corporateAccountName;
            });
            $('#<%=hdnParamApm.ClientID %>').val(param);
            $('#<%=hdnLstParamedicIDApm.ClientID %>').val(lstParamedicID);
            $('#<%=hdnLstApmDateApm.ClientID %>').val(lstAppDate);
            $('#<%=hdnLstCustomerTypeApm.ClientID %>').val(lstCustomerType);
        };

        function getSelectedReg() {
            var param = '';
            var lstParamedicID = '';
            var lstAppDate = '';
            var lstCustomerType = '';
            var lstCorporateAccountNo = '';
            var lstCorporateAccountName = '';
            $('.chkIsSelectedReg input:checked').each(function () {
                var trxID = $(this).closest('tr').find('.hdnKeyFieldReg').val();
                var paramedicID = $(this).closest('tr').find('.hdnParamedicIDReg').val();
                var appDate = $(this).closest('tr').find('.txtAppointmentDateReg').val();
                var customerType = $(this).closest('tr').find('.hdnGCCustomerTypeReg').val();
                var corporateAccountNo = $(this).closest('tr').find('.hdnCorporateAccountNoReg').val();
                var corporateAccountName = $(this).closest('tr').find('.hdnCorporateAccountNameReg').val();
                if (param != '')
                    param += ',';
                param += trxID;
                if (lstParamedicID != '')
                    lstParamedicID += ',';
                lstParamedicID += paramedicID;
                if (lstAppDate != '')
                    lstAppDate += ',';
                lstAppDate += appDate;
                if (lstCustomerType != '')
                    lstCustomerType += ',';
                lstCustomerType += customerType;
                if (lstCorporateAccountNo != '')
                    lstCorporateAccountNo += ',';
                lstCorporateAccountNo += corporateAccountNo;
                if (lstCorporateAccountName != '')
                    lstCorporateAccountName += ',';
                lstCorporateAccountName += corporateAccountName;
            });
            $('#<%=hdnParamReg.ClientID %>').val(param);
            $('#<%=hdnLstParamedicIDReg.ClientID %>').val(lstParamedicID);
            $('#<%=hdnLstRegDateReg.ClientID %>').val(lstAppDate);
            $('#<%=hdnLstCustomerTypeReg.ClientID %>').val(lstCustomerType);
        };
        //#endregion

        $('#<%=btnPrintLabel.ClientID %>').click(function () {
            var tab = $('#<%=hdnSelectedTab.ClientID %>').val();
            if (tab == "containerRegistration") {
                var date = $('#<%=txtFromAppointmentDateReg.ClientID %>').val();
                var customerType = cboRegistrationPayerReg.GetValue();
                var businessPartnerID = $('#<%:hdnPayerIDReg.ClientID %>').val();
                var id = date + "|" + customerType + "|" + businessPartnerID + "|" + $('#<%:hdnDepartmentID.ClientID %>').val();
                var url = ResolveUrl("~/Libs/Program/Controls/PrintPatientLabelMCUCtl.ascx");
                openUserControlPopup(url, id, 'Print', 600, 500);
            }
        });

        $('#<%=btnVoidProcess.ClientID %>').click(function () {
            var tab = $('#<%=hdnSelectedTab.ClientID %>').val();
            if (tab == "containerUpload") {
                getSelected();
                if ($('#<%=hdnParam.ClientID %>').val() != "") {
                    cbpView.PerformCallback('void');
                }
                else {
                    displayMessageBox("WARNING", "Harap pilih permintaan yang akan diproses");
                }
            }
            else if (tab == "containerAppointment") {
                getSelectedApm();
                if ($('#<%=hdnParamApm.ClientID %>').val() != "") {
                    cbpViewApm.PerformCallback('void');
                }
                else {
                    displayMessageBox("WARNING", "Harap pilih permintaan yang akan diproses");
                }
            }
            else if (tab == "containerRegistration") {
                displayMessageBox("WARNING", "Tidak bisa proses batal untuk pasien yang sudah diregistrasi menggunakan menu ini");
            }
        });

        $('#<%=btnApproveAppointment.ClientID %>').click(function () {
            var tab = $('#<%=hdnSelectedTab.ClientID %>').val();
            var payer = $('#<%:hdnPayerID.ClientID %>').val();
            var contract = $('#<%:hdnContractID.ClientID %>').val();
            var coverage = $('#<%:hdnCoverageTypeID.ClientID %>').val();

            if (tab == "containerUpload") {
                var validProcess = true;
                var isPersonalPayer = false;
                if (payer != '' && contract != '' && coverage != '') {
                    if (cboRegistrationPayer.GetValue() == "X004^999") {
                        isPersonalPayer = true;
                    }
                    if (!isPersonalPayer) {
                        if (payer == '' && contract == '' && coverage == '') {
                            validProcess = false;
                            displayMessageBox("WARNING", "Harap pilih instansi, kontrak, dan skema penjaminan");
                        }
                    }
                }
                else {
                    validProcess = false;
                    displayMessageBox("WARNING", "Harap pilih instansi, kontrak, dan skema penjaminan");
                }

                if (validProcess) {
                    getSelected();
                    if ($('#<%=hdnParam.ClientID %>').val() == '') {
                        displayMessageBox("WARNING", "Harap pilih permintaan yang akan diproses");
                    }
                    else {
                        cbpView.PerformCallback('multiple|process|');
                    }
                }
            }
            else if (tab == "containerAppointment") {
                getSelectedApm();
                var payerApm = $('#<%:hdnPayerIDApm.ClientID %>').val();
                if (payerApm != '') {
                    if ($('#<%=hdnParamApm.ClientID %>').val() == '') {
                        displayMessageBox("WARNING", "Harap pilih permintaan yang akan diproses");
                    }
                    else {
                        cbpViewApm.PerformCallback('multiple|process|');
                    }
                }
                else {
                    displayMessageBox("WARNING", "Harap pilih instansi, kontrak, dan skema penjaminan");
                }
            }
            else if (tab == "containerRegistration") {
                displayMessageBox("WARNING", "Tidak bisa melanjutkan proses appointment pada pasien yang sudah diregistrasikan");
            }
        });

        function onLoad() {
            
        }

        function onAfterCustomClickSuccess(type) {
            var tab = $('#<%=hdnSelectedTab.ClientID %>').val();
            cbpView.PerformCallback('refresh|');
        }

        function onAfterSaveAddRecordEntryPopup(param) {
            var tab = $('#<%=hdnSelectedTab.ClientID %>').val();
            cbpView.PerformCallback('refresh|');
        }

        function onTxtSearchViewSearchClick(s) {
            setTimeout(function () {
                s.SetBlur();
                onRefreshControl();
                setTimeout(function () {
                    s.SetFocus();
                }, 0);
            }, 0);
        }

        //#region End call back
        function onCbpViewEndCallback(s) {
            hideLoadingPanel();
            var param = s.cpResult.split('|');

            if (param[0] == 'refresh|') {

                var pageCount = parseInt(param[1]);
                if (pageCount > 0)
                    $('#<%=lvwView.ClientID %> tr:eq(1)').click();

                setPaging($("#paging"), pageCount, function (page) {
                    cbpView.PerformCallback('changepage|' + page);
                });
            }
            else if (param[0] == 'createApp') {
                cbpView.PerformCallback('refresh|');
                var id = param[2];
                var url = ResolveUrl("~/Libs/Program/Module/PatientManagement/Appointment/AppointmentGenerateInformationCtl.ascx");
                openUserControlPopup(url, id, 'Appointment Information', 800, 500);
            }
            else if (param[0] == 'multiple') {
                if (param[2] == 'appointment') {
                    if (param[3] == 'success') {
                        cbpView.PerformCallback('refresh|');
                        displayMessageBox('SUCCESS', 'Proses pembuatan perjanjian berhasil!');
                    }
                    else {
                        displayMessageBox(' Failed', 'Error Message : ' + param[4]);
                        onRefreshPage();
                    }
                }
                else if (param[2] == 'registration') {
                    if (param[3] == 'success') {
                        cbpView.PerformCallback('refresh|');
                        displayMessageBox('SUCCESS', 'Proses pembuatan registrasi berhasil!');
                    }
                    else {
                        displayMessageBox(' Failed', 'Error Message : ' + param[4]);
                        onRefreshPage();
                    }
                }
            }
            else if (param[0] == 'void') {
                if (param[1] == "success") {
                    var id = param[2];
                    var url = ResolveUrl("~/Libs/Program/Module/PatientManagement/Appointment/AppointmentVoidDiagnosticCtl.ascx");
                    openUserControlPopup(url, id, 'Void Appointment', 600, 200);
                    onRefreshPage();
                }
                else {
                    displayMessageBox(' Failed', 'Error Message : ' + param[2]);
                }
            }
            else if (param[0] == 'print') {
                if (param[1] == "success") {
                    onRefreshPage();
                }
                else {
                    onRefreshPage();
                    displayMessageBox(' Failed', 'Error Message : ' + param[2]);
                }
            }
            else if (param[0] == 'update') {
                if (param[1] == 'paramedic') {
                    if (param[2] == 'success') {
                        onRefreshPage();
                    }
                    else {
                        displayMessageBox(' Failed', 'Error Message : ' + param[3]);        
                    }
                }
            }
            else if (param[0] == 'fail') {
                displayMessageBox(' Failed', 'Error Message : ' + param[1]);
                onRefreshPage();
            }
            else
                $('#<%=lvwView.ClientID %> tr:eq(1)').click();
            setDatePicker('<%=txtFromAppointmentRequestDate.ClientID %>');
            $('#<%=txtFromAppointmentRequestDate.ClientID %>').datepicker('option', '0');
        }

        function onCbpViewApmEndCallback(s) {
            hideLoadingPanel();
            var param = s.cpResult.split('|');

            if (param[0] == 'refresh|') {

                var pageCount = parseInt(param[1]);
                if (pageCount > 0)
                    $('#<%=lvwViewApm.ClientID %> tr:eq(1)').click();

                setPaging($("#paging"), pageCount, function (page) {
                    cbpViewApm.PerformCallback('changepage|' + page);
                });
            }
            else if (param[0] == 'createApp') {
                cbpViewApm.PerformCallback('refresh|');
                var id = param[2];
                var url = ResolveUrl("~/Libs/Program/Module/PatientManagement/Appointment/AppointmentGenerateInformationCtl.ascx");
                openUserControlPopup(url, id, 'Appointment Information', 800, 500);
            }
            else if (param[0] == 'multiple') {
                if (param[2] == 'appointment') {
                    if (param[3] == 'success') {
                        cbpViewApm.PerformCallback('refresh|');
                        displayMessageBox('SUCCESS', 'Proses pembuatan perjanjian berhasil!');
                    }
                    else {
                        displayMessageBox(' Failed', 'Error Message : ' + param[4]);
                        onRefreshPageApm();
                    }
                }
                else if (param[2] == 'registration') {
                    if (param[3] == 'success') {
                        cbpViewApm.PerformCallback('refresh|');
                        displayMessageBox('SUCCESS', 'Proses pembuatan registrasi berhasil!');
                    }
                    else {
                        displayMessageBox(' Failed', 'Error Message : ' + param[4]);
                        onRefreshPageApm();
                    }
                }
            }
            else if (param[0] == 'void') {
                if (param[1] == "success") {
                    var id = param[2];
                    var url = ResolveUrl("~/Libs/Program/Module/PatientManagement/Appointment/AppointmentVoidDiagnosticCtl.ascx");
                    openUserControlPopup(url, id, 'Void Appointment', 600, 200);
                    onRefreshPageApm();
                }
                else {
                    displayMessageBox(' Failed', 'Error Message : ' + param[2]);
                }
            }
            else if (param[0] == 'print') {
                if (param[1] == "success") {
                    onRefreshPageApm();
                }
                else {
                    onRefreshPageApm();
                    displayMessageBox(' Failed', 'Error Message : ' + param[2]);
                }
            }
            else if (param[0] == 'fail') {
                displayMessageBox(' Failed', 'Error Message : ' + param[1]);
                cbpViewApm.PerformCallback('refresh|');
            }
            else
                $('#<%=lvwViewApm.ClientID %> tr:eq(1)').click();
        }

        function onCbpViewRegEndCallback(s) {
            hideLoadingPanel();
            var param = s.cpResult.split('|');

            if (param[0] == 'refresh|') {

                var pageCount = parseInt(param[1]);
                if (pageCount > 0)
                    $('#<%=lvwViewReg.ClientID %> tr:eq(1)').click();

                setPaging($("#paging"), pageCount, function (page) {
                    cbpViewReg.PerformCallback('changepage|' + page);
                });
            }
            else if (param[0] == 'print') {
                if (param[1] == "success") {
                    onRefreshPageReg();
                }
                else {
                    onRefreshPageReg();
                    displayMessageBox(' Failed', 'Error Message : ' + param[2]);
                }
            }
            else if (param[0] == 'fail') {
                displayMessageBox(' Failed', 'Error Message : ' + param[1]);
                cbpViewReg.PerformCallback('refresh|');
            }
            else
                $('#<%=lvwViewReg.ClientID %> tr:eq(1)').click();
        }
        //#endregion

        $('.lblParamedicName.lblLink').live('click', function () {
            var healthcareServiceUnitID = $(this).closest('tr').find('.hdnHealthareServiceUnitID').val();
            $td = $(this).parent();
            var apmReqID = $(this).closest('tr').find('.hdnKeyField').val();
            var paramedicID = $td.children('.hdnParamedicID').val();
            var healthcareServiceUnitID = $td.children('.hdnHealthcareServiceUnitPerRowID').val();
            var filter = "HealthcareServiceUnitID = '" + healthcareServiceUnitID + "'";
            openSearchDialog('serviceUnitParamedicMaster', filter, function (value) {
                onTxtParamedicChanged(value, $td, apmReqID);
            });
        });

        function onTxtParamedicChanged(value, $td, apmReqID) {
            var filterExpression = "ParamedicID = '" + value + "'";
            Methods.getObject('GetvParamedicMasterList', filterExpression, function (result) {
                if (result != null) {
                    cbpView.PerformCallback('update|single|paramedic|' + apmReqID + '|' + result.ParamedicID);
                }
            });
        }

        $('.lblCustomerType.lblLink').live('click', function () {
            $tr = $(this).closest('tr');
            var gcCustomerType = $tr.find('.hdnGCCustomerType').val();
            var appointmentRequestID = $tr.find('.hdnKey').val();

            var url = ResolveUrl("~/Libs/Program/Module/PatientManagement/Appointment/AppointmentRequestChangePayerDiagnosticCtl.ascx");
            var id = 'single|' + appointmentRequestID;
            openUserControlPopup(url, id, 'Ubah Tipe Penjamin', 800, 350);
        });

        $btnSave = null;
        $('.btnSave').live('click', function () {
            $tr = $(this).closest('td').closest('tr').closest('td').closest('tr');
            var ID = $tr.find('.hdnKey').val();
            var appDate = $tr.find('.txtAppointmentDate').val();
            var paramedicID = $tr.find('.hdnParamedicID').val();
            var gcCustomerType = $tr.find('.hdnGCCustomerType').val();
            $('#<%=hdnParam.ClientID %>').val(ID);
            $('#<%=hdnLstParamedicID.ClientID %>').val(paramedicID);
            $('#<%=hdnLstApmDate.ClientID %>').val(appDate);
            $('#<%=hdnLstCustomerType.ClientID %>').val(gcCustomerType);
            cbpView.PerformCallback('createApp');
        });

        $btnVoid = null;
        $('.btnVoid').live('click', function () {
            $tr = $(this).closest('td').closest('tr').closest('td').closest('tr');
            var id = $tr.find('.hdnKey').val();
            var appDate = $tr.find('.txtAppointmentDate').val();
            var paramedicID = $tr.find('.hdnParamedicID').val();
            var url = ResolveUrl("~/Libs/Program/Module/PatientManagement/Appointment/AppointmentRequestVoidCtl.ascx");
            openUserControlPopup(url, id, 'Void Appointment Request', 800, 350);
        });

        $btnPrint = null;
        $('.btnPrint').live('click', function () {
            $tr = $(this).closest('td').closest('tr').closest('td').closest('tr');
            var id = $tr.find('.hdnKey').val();
            var corporateAccountNo = $tr.find('.hdnCorporateAccountNo').val();
            var corporateAccountName = $tr.find('.hdnCorporateAccountName').val();
            displayMessageBox('PRINT', corporateAccountNo + '<br />' + corporateAccountName);
        });

        function isValidDate(value) {
            var dateWrapper = new Date(value);
            return !isNaN(dateWrapper.getDate());
        }

        function onRefreshPage() {
            cbpView.PerformCallback('refresh|');
        }

        function onRefreshPageApm() {
            cbpViewApm.PerformCallback('refresh|');
        }

        function onRefreshPageReg() {
            cbpViewReg.PerformCallback('refresh|');
        }

        function onBeforeLoadRightPanelContent(code) {
            if (code == 'infoDokterPraktek') {
                param = $('#<%:hdnDepartmentID.ClientID %>').val();
            }
        }

        function onCboFilterTypeChanged() {
            var type = cboFilterType.GetValue();
        }

        $('#btnCustomerContractDocumentInfo').live('click', function () {
            var payer = $('#<%:hdnPayerID.ClientID %>').val();
            var contract = $('#<%:hdnContractID.ClientID %>').val();
            var url = ResolveUrl("~/Libs/Program/Information/InformationCustomerPayerCtl.ascx");
            var id = payer + "|" + contract;
            openUserControlPopup(url, id, 'Informasi Instansi', 700, 600);
        });

        $('#btnPayerNotesDetail').live('click', function () {
            if ($(this).attr('enabled') == null) {
                var id = $('#<%:hdnPayerID.ClientID %>').val();
                var url = ResolveUrl("~/Libs/Program/Module/PatientManagement/Registration/CustomerNotesDetailCtl.ascx");
                openUserControlPopup(url, id, 'Notes', 500, 400);
            }
        });

        //#region Payer Company
        function getPayerCompanyFilterExpression() {
            var filterExpression = "<%:GetCustomerFilterExpression()%> AND GCCustomerType = '" + cboRegistrationPayer.GetValue() + "' AND IsDeleted = 0 AND IsActive = 1 AND IsBlacklist = 0";
            return filterExpression;
        }

        $('#<%:lblPayerCompany.ClientID %>.lblLink').live('click', function () {
            openSearchDialog('payer', getPayerCompanyFilterExpression(), function (value) {
                $('#<%:txtPayerCompanyCode.ClientID %>').val(value);
                onTxtPayerCompanyCodeChanged(value);
            });
        });

        $('#<%:txtPayerCompanyCode.ClientID %>').live('change', function () {
            if ($(this).val() != "") {
                onTxtPayerCompanyCodeChanged($(this).val());
            }
            else {
                $('#<%:hdnPayerID.ClientID %>').val('');
                $('#<%:txtPayerCompanyName.ClientID %>').val('');
                $('#trContract').attr('style', 'display:none');
                $('#trScheme').attr('style', 'display:none');
                $('#<%:hdnCoverageTypeID.ClientID %>').val('');
                $('#<%:txtCoverageTypeCode.ClientID %>').val('');
                $('#<%:txtCoverageTypeName.ClientID %>').val('');
            }
        });

        function getPayerCompanyFilterExpressionApm() {
            var filterExpression = "<%:GetCustomerFilterExpression()%> AND GCCustomerType = '" + cboRegistrationPayerApm.GetValue() + "' AND IsDeleted = 0 AND IsActive = 1 AND IsBlacklist = 0";
            return filterExpression;
        }

        function getPayerCompanyFilterExpressionReg() {
            var filterExpression = "<%:GetCustomerFilterExpression()%> AND GCCustomerType = '" + cboRegistrationPayerReg.GetValue() + "' AND IsDeleted = 0 AND IsActive = 1 AND IsBlacklist = 0";
            return filterExpression;
        }

        $('#<%:lblPayerCompanyApm.ClientID %>.lblLink').live('click', function () {
            openSearchDialog('payer', getPayerCompanyFilterExpressionApm(), function (value) {
                var filterExpression = getPayerCompanyFilterExpressionApm() + " AND BusinessPartnerCode = '" + value + "'";
                Methods.getObject('GetvCustomerList', filterExpression, function (result) {
                    if (result != null) {
                        $('#<%:hdnPayerIDApm.ClientID %>').val(result.BusinessPartnerID);
                        $('#<%:txtPayerCompanyCodeApm.ClientID %>').val(result.BusinessPartnerCode);
                        $('#<%:txtPayerCompanyNameApm.ClientID %>').val(result.BusinessPartnerName);
                    }
                    else {
                        $('#<%:hdnPayerIDApm.ClientID %>').val('');
                        $('#<%:txtPayerCompanyCodeApm.ClientID %>').val('');
                        $('#<%:txtPayerCompanyNameApm.ClientID %>').val('');
                    }
                });
            });
        });

        $('#<%:lblPayerCompanyReg.ClientID %>.lblLink').live('click', function () {
            openSearchDialog('payer', getPayerCompanyFilterExpressionReg(), function (value) {
                var filterExpression = getPayerCompanyFilterExpressionReg() + " AND BusinessPartnerCode = '" + value + "'";
                Methods.getObject('GetvCustomerList', filterExpression, function (result) {
                    if (result != null) {
                        $('#<%:hdnPayerIDReg.ClientID %>').val(result.BusinessPartnerID);
                        $('#<%:txtPayerCompanyCodeReg.ClientID %>').val(result.BusinessPartnerCode);
                        $('#<%:txtPayerCompanyNameReg.ClientID %>').val(result.BusinessPartnerName);
                    }
                    else {
                        $('#<%:hdnPayerIDReg.ClientID %>').val('');
                        $('#<%:txtPayerCompanyCodeReg.ClientID %>').val('');
                        $('#<%:txtPayerCompanyNameReg.ClientID %>').val('');
                    }
                });
            });
        });

        $('#<%:txtPayerCompanyCodeApm.ClientID %>').live('change', function () {
            if ($(this).val() == "") {
                $('#<%:hdnPayerIDApm.ClientID %>').val('');
                $('#<%:txtPayerCompanyCodeApm.ClientID %>').val('');
                $('#<%:txtPayerCompanyNameApm.ClientID %>').val('');
            }

        });

        $('#<%:txtPayerCompanyCodeReg.ClientID %>').live('change', function () {
            if ($(this).val() == "") {
                $('#<%:hdnPayerIDReg.ClientID %>').val('');
                $('#<%:txtPayerCompanyCodeReg.ClientID %>').val('');
                $('#<%:txtPayerCompanyNameReg.ClientID %>').val('');
            }

        });

        function onTxtPayerCompanyCodeChanged(value) {
            var filterExpression = getPayerCompanyFilterExpression() + " AND BusinessPartnerCode = '" + value + "'";
            getPayerCompany(filterExpression);
        }

        function getPayerCompany(_filterExpression) {
            var filterExpression = _filterExpression;
            if (filterExpression == '') filterExpression = getPayerCompanyFilterExpression();

            Methods.getObject('GetvCustomerList', filterExpression, function (result) {
                var messageBlacklistPayer = '<font size="4">' + 'Rekanan Sedang dilakukan Penutupan Layanan Sementara,' + '<br/>' + ' untuk sementara dilakukan sebagai' + '<b>' + ' PASIEN UMUM' + '</b>' + '</font>';
                if (result != null) {
                    $('#<%:hdnIsBlacklistPayer.ClientID %>').val(result.IsBlackList);
                    if ($('#<%:hdnIsBlacklistPayer.ClientID %>').val() == 'false') {
                        $('#<%:hdnPayerID.ClientID %>').val(result.BusinessPartnerID);
                        $('#<%:txtPayerCompanyCode.ClientID %>').val(result.BusinessPartnerCode);
                        $('#<%:txtPayerCompanyName.ClientID %>').val(result.BusinessPartnerName);
                        $('#<%:hdnGCTariffScheme.ClientID %>').val(result.GCTariffScheme);
                        $('#btnPayerNotesDetail').removeAttr('enabled');
                        var filterExpression = getPayerContractFilterExpression();
                        Methods.getValue('GetCustomerContractRowCount', filterExpression, function (result) {
                            if (result == 1) {
                                Methods.getObject('GetvCustomerContractList', filterExpression, function (result) {
                                    if (result != null) {
                                        $('#<%:hdnContractID.ClientID %>').val(result.ContractID);
                                        $('#<%:txtContractNo.ClientID %>').val(result.ContractNo);
                                        $('#<%:hdnContractCoverageCount.ClientID %>').val(result.ContractCoverageCount);
                                        $('#trContract').removeAttr('style', 'display:none');
                                        $('#trScheme').removeAttr('style', 'display:none');
                                        onRefreshPage();
                                    }
                                });
                            }
                            else {
                                $('#<%:hdnContractID.ClientID %>').val('');
                                $('#<%:txtContractNo.ClientID %>').val('');
                                $('#<%:hdnContractCoverageCount.ClientID %>').val('');
                                $('#trContract').attr('style', 'display:none');
                                $('#trScheme').attr('style', 'display:none');
                            }
                        });
                    }
                }
                else {
                    $('#<%:hdnIsBlacklistPayer.ClientID %>').val('0');
                    $('#<%:hdnPayerID.ClientID %>').val('');
                    $('#<%:txtPayerCompanyCode.ClientID %>').val('');
                    $('#<%:txtPayerCompanyName.ClientID %>').val('');
                    $('#btnPayerNotesDetail').attr('enabled', 'false');
                    $('#<%:hdnGCTariffScheme.ClientID %>').val('');

                    $('#<%:hdnContractID.ClientID %>').val('');
                    $('#<%:txtContractNo.ClientID %>').val('');
                    $('#<%:hdnContractCoverageCount.ClientID %>').val('');
                    $('#trContract').attr('style', 'display:none');
                    $('#trScheme').attr('style', 'display:none');
                }
            });
        }

        function getPayerContractFilterExpression() {
            var filterExpression = "BusinessPartnerID = " + $('#<%:hdnPayerID.ClientID %>').val() + " AND EndDate >= CONVERT(DATE,GetDate())  AND IsDeleted = 0";
            return filterExpression;
        }

        $('#<%:lblContract.ClientID %>.lblLink').live('click', function () {
            if ($('#<%:hdnPayerID.ClientID %>').val() != '') {
                openSearchDialog('contract', getPayerContractFilterExpression(), function (value) {
                    $('#<%:txtContractNo.ClientID %>').val(value);
                    onTxtPayerContractNoChanged(value);
                });
            }
        });

        $('#<%:txtContractNo.ClientID %>').live('change', function () {
            if ($('#<%:hdnPayerID.ClientID %>').val() != '')
                onTxtPayerContractNoChanged($(this).val());
            else
                $(this).val('');
        });

        function onTxtPayerContractNoChanged(value) {
            var filterExpression = getPayerContractFilterExpression() + " AND ContractNo = '" + value + "'";
            Methods.getObject('GetvCustomerContractList', filterExpression, function (result) {
                if (result != null) {
                    $('#<%:hdnContractID.ClientID %>').val(result.ContractID);
                    $('#<%:hdnContractCoverageCount.ClientID %>').val(result.ContractCoverageCount);

                    onAfterContractNoChanged();
                }
                else {
                    $('#<%:hdnContractID.ClientID %>').val('');
                    $('#<%:txtContractNo.ClientID %>').val('');
                    $('#<%:hdnContractCoverageCount.ClientID %>').val('');
                }
            });
        }

        function onAfterContractNoChanged() {
            var payerID = $('#<%:hdnPayerID.ClientID %>').val();
            var filterExpression = "CoverageTypeID IN (SELECT CoverageTypeID FROM ContractCoverage WHERE ContractID = " + $('#<%:hdnContractID.ClientID %>').val() + ") AND IsDeleted = 0 AND CoverageTypeID IN (SELECT CoverageTypeID FROM HealthcareCoverageType WHERE HealthcareID = '" + AppSession.healthcareID + "')";
            filterExpression += " AND CoverageTypeID IN (SELECT CoverageTypeID FROM ContractCoverage WHERE ContractID IN (SELECT ContractID FROM CustomerContract WHERE IsDeleted = 0 AND BusinessPartnerID = " + payerID + "))";
            Methods.getObject('GetCoverageTypeList', filterExpression, function (result) {
                if (result != null) {
                    $('#<%:hdnCoverageTypeID.ClientID %>').val(result.CoverageTypeID);
                    $('#<%:txtCoverageTypeCode.ClientID %>').val(result.CoverageTypeCode);
                    $('#<%:txtCoverageTypeName.ClientID %>').val(result.CoverageTypeName);
                }
                else {
                    $('#<%:hdnCoverageTypeID.ClientID %>').val('');
                    $('#<%:txtCoverageTypeCode.ClientID %>').val('');
                    $('#<%:txtCoverageTypeName.ClientID %>').val('');
                }
            });
        }
        //#endregion

        //#region Coverage Type
        function getCoverageTypeFilterExpression() {
            var contractCoverageRowCount = parseInt($('#<%:hdnContractCoverageCount.ClientID %>').val());
            var payerID = parseInt($('#<%:hdnPayerID.ClientID %>').val());

            var filterExpression = '';
            if (contractCoverageRowCount > 0)
                filterExpression = "CoverageTypeID IN (SELECT CoverageTypeID FROM ContractCoverage WHERE ContractID = " + $('#<%:hdnContractID.ClientID %>').val() + ") AND IsDeleted = 0";
            else
                filterExpression = "IsDeleted = 0";

            filterExpression += " AND CoverageTypeID IN (SELECT CoverageTypeID FROM ContractCoverage WHERE ContractID IN (SELECT ContractID FROM CustomerContract WHERE IsDeleted = 0 AND BusinessPartnerID = " + payerID + "))";
            return filterExpression;
        }

        $('#<%:lblCoverageType.ClientID %>.lblLink').live('click', function () {
            if ($('#<%:hdnContractID.ClientID %>').val() != '') {
                openSearchDialog('coveragetype', getCoverageTypeFilterExpression(), function (value) {
                    $('#<%:txtCoverageTypeCode.ClientID %>').val(value);
                    onTxtCoverageTypeCodeChanged(value);
                });
            }
        });

        $('#<%:txtCoverageTypeCode.ClientID %>').live('change', function () {
            if ($('#<%:hdnContractID.ClientID %>').val() != '')
                onTxtCoverageTypeCodeChanged($(this).val());
            else
                $(this).val('');
        });

        function onTxtCoverageTypeCodeChanged(value) {
            var filterExpression = getCoverageTypeFilterExpression() + " AND CoverageTypeCode = '" + value + "'";
            getCoverageType(filterExpression);
        }

        function getCoverageType(_filterExpression) {
            var filterExpression = _filterExpression;
            if (filterExpression == '') filterExpression = getCoverageTypeFilterExpression();
            Methods.getObject('GetCoverageTypeList', filterExpression, function (result) {
                if (result != null) {
                    $('#<%:hdnCoverageTypeID.ClientID %>').val(result.CoverageTypeID);
                    $('#<%:txtCoverageTypeCode.ClientID %>').val(result.CoverageTypeCode);
                    $('#<%:txtCoverageTypeName.ClientID %>').val(result.CoverageTypeName);
                }
                else {
                    $('#<%:hdnCoverageTypeID.ClientID %>').val('');
                    $('#<%:txtCoverageTypeCode.ClientID %>').val('');
                    $('#<%:txtCoverageTypeName.ClientID %>').val('');
                }
            });
        }

        function onCboPayerValueChanged(s) {
            
        }

        function getCboPayerValueCompany() {
            getPayerCompany('');
            $("#trPayer").removeAttr("style", "display:none");
            $('#trContract').attr('style', 'display:none');
            $('#trScheme').attr('style', 'display:none');
            $('#<%:hdnIsBlacklistPayer.ClientID %>').val('0');
            $('#<%:hdnPayerID.ClientID %>').val('');
            $('#<%:txtPayerCompanyCode.ClientID %>').val('');
            $('#<%:txtPayerCompanyName.ClientID %>').val('');
            $('#btnPayerNotesDetail').attr('enabled', 'false');
            $('#<%:hdnGCTariffScheme.ClientID %>').val('');

            $('#<%:hdnContractID.ClientID %>').val('');
            $('#<%:txtContractNo.ClientID %>').val('');
            $('#<%:hdnContractCoverageCount.ClientID %>').val('');
        }

        function onCboPayerValueChangedApm(s) {
            var customer = cboRegistrationPayerApm.GetValue();
            if (customer != "X004^999") {
                $("#trPayerApm").removeAttr("style", "display:none");
            }
            else {
                $("#trPayerApm").attr("style", "display:none");
            }
        }

        function onCboPayerValueChangedReg(s) {
            var customer = cboRegistrationPayerReg.GetValue();
            if (customer != "X004^999") {
                $("#trPayerReg").removeAttr("style", "display:none");
            }
            else {
                $("#trPayerReg").attr("style", "display:none");
            }
        }

        function getCoverageType(_filterExpression) {
            var filterExpression = _filterExpression;
            if (filterExpression == '') filterExpression = getCoverageTypeFilterExpression();
            Methods.getObject('GetCoverageTypeList', filterExpression, function (result) {
                if (result != null) {
                    $('#<%:hdnCoverageTypeID.ClientID %>').val(result.CoverageTypeID);
                    $('#<%:txtCoverageTypeCode.ClientID %>').val(result.CoverageTypeCode);
                    $('#<%:txtCoverageTypeName.ClientID %>').val(result.CoverageTypeName);
                }
                else {
                    $('#<%:hdnCoverageTypeID.ClientID %>').val('');
                    $('#<%:txtCoverageTypeCode.ClientID %>').val('');
                    $('#<%:txtCoverageTypeName.ClientID %>').val('');
                }
            });
        }
        //#endregion
    </script>
    <input type="hidden" id="hdnDepartmentID" runat="server" />
    <input type="hidden" value="" id="hdnID" runat="server" />
    <input type="hidden" value="" id="hdnPageTitle" runat="server" />
    <input type="hidden" value="" id="hdnFilterExpression" runat="server" />
    <input type="hidden" value="" id="hdnQuickText" runat="server" />
    <input type="hidden" value="" id="hdnAppointmentRequestID" runat="server" />
    <input type="hidden" value="" id="hdnAppointmentRequestParamedicID" runat="server" />
    <input type="hidden" value="" id="hdnAppointmentRequestDate" runat="server" />
    <input type="hidden" value="" id="hdnHealthcareServiceUnitImagingID" runat="server" />
    <input type="hidden" value="" id="hdnHealthcareServiceUnitLaboratoryID" runat="server" />
    <input type="hidden" value="" id="hdnIsBridgingToMedinfrasMobileApps" runat="server" />
    <input type="hidden" value="" id="hdnGCCustomerType" runat="server" />
    <input type="hidden" value="" id="hdnMCUClassID" runat="server" />

    <input type="hidden" value="" id="hdnParam" runat="server" />
    <input type="hidden" value="" id="hdnLstParamedicID" runat="server" />
    <input type="hidden" value="" id="hdnLstApmDate" runat="server" />
    <input type="hidden" value="" id="hdnLstCustomerType" runat="server" />

    <input type="hidden" value="" id="hdnParamApm" runat="server" />
    <input type="hidden" value="" id="hdnLstParamedicIDApm" runat="server" />
    <input type="hidden" value="" id="hdnLstApmDateApm" runat="server" />
    <input type="hidden" value="" id="hdnLstCustomerTypeApm" runat="server" />

    <input type="hidden" value="" id="hdnParamReg" runat="server" />
    <input type="hidden" value="" id="hdnLstParamedicIDReg" runat="server" />
    <input type="hidden" value="" id="hdnLstRegDateReg" runat="server" />
    <input type="hidden" value="" id="hdnLstCustomerTypeReg" runat="server" />

    <input type="hidden" value="" id="hdnTodayDate" runat="server" />
    <input type="hidden" value="" id="hdnIsControlAdministrationCharges" runat="server" />
    <input type="hidden" value="" id="hdnChargeCodeAdministrationForInstansi" runat="server" />
    <input type="hidden" value="" id="hdnIsControlAdmCost" runat="server" />
    <input type="hidden" value="" id="hdnAdminID" runat="server" />
    <input type="hidden" value="" id="hdnIsControlPatientCardPayment" runat="server" />
    <input type="hidden" value="" id="hdnItemCardFee" runat="server" />
    <input type="hidden" value="" id="hdnFilterExpressionQuickSearch" runat="server" />
    <input type="hidden" value="" id="hdnSelectedTab" runat="server" />
    <div>
        <asp:Panel ID="panel1" runat="server" DefaultButton="btnDownload">
            <table class="tblEntryContent" style="width: 100%">
                <colgroup>
                    <col style="width: 200px" />
                    <col />
                </colgroup>
                <tr>
                    <td class="tdLabel">
                        <label class="lblNormal">
                            <%=GetLabel("Download Template Excel")%></label>
                    </td>
                    <td>
                        <table cellpadding="0" cellspacing="0">
                            <colgroup>
                                <col style="width: 145px" />
                                <col style="width: 3px" />
                                <col style="width: 145px" />
                            </colgroup>
                            <tr>
                                <td>
                                    <asp:Button ID="btnDownload" runat="server"  Text="Download" OnClick="btnDownload_Click"/>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </asp:Panel>
    </div>
    <div class="containerUlTabPage" style="margin-bottom: 3px;">
        <ul class="ulTabPage" id="ulTabLabResult">
            <li contentid="containerUpload" id="listPatientUpload" runat="server" style="display:none">
                <%=GetLabel("Belum Diproses")%></li>
            <li contentid="containerAppointment" id="listPatientAppointment" runat="server" style="display:none">
                <%=GetLabel("Sudah Perjanjian")%></li>
            <li contentid="containerRegistration" id="listPatientRegistration" runat="server" style="display:none">
                <%=GetLabel("Sudah Registrasi")%></li>
        </ul>
    </div>
    <div id="containerUpload" class="containerInfo" runat="server" style="display:none">
        <div class="pageTitle">
            <%=GetLabel("Daftar Pasien : Belum Diproses")%></div>
        <asp:Panel ID="panel" runat="server" DefaultButton="btnDownload">
            <table class="tblContentArea" id="LstApmRequest">
                <colgroup>
                    <col style="width: 50%" />
                    <col style="width: 50%" />
                </colgroup>
                <tr>
                    <td style="padding: 5px; vertical-align: top">
                        <table class="tblEntryContent" style="width: 100%">
                            <colgroup>
                                <col style="width: 200px" />
                                <col />
                            </colgroup>
                            <tr style="display:none">
                                <td class="tdLabel">
                                    <label class="lblNormal">
                                        <%=GetLabel("Filter Tampilan")%></label>
                                </td>
                                <td>
                                    <dxe:ASPxComboBox ID="cboFilterType" ClientInstanceName="cboFilterType" Width="55%" runat="server">
                                        <ClientSideEvents ValueChanged="function(s,e){
                                                onCboFilterTypeChanged();
                                            }" />
                                    </dxe:ASPxComboBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblNormal">
                                        <%=GetLabel("Tanggal")%></label>
                                </td>
                                <td>
                                    <table cellpadding="0" cellspacing="0">
                                        <colgroup>
                                            <col style="width: 145px" />
                                            <col style="width: 3px" />
                                            <col style="width: 145px" />
                                        </colgroup>
                                        <tr>
                                            <td>
                                                <asp:TextBox ID="txtFromAppointmentRequestDate" Width="120px" CssClass="datepicker"
                                                    runat="server" />
                                            </td>
                                            <td style="display:none">
                                                <%=GetLabel("s/d") %>
                                            </td>
                                            <td style="display:none">
                                                <asp:TextBox ID="txtToAppointmentRequestDate" Width="120px" CssClass="datepicker"
                                                    runat="server" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <hr style="padding: 0 0 0 0; margin: 0 0 0 0;" />
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblNormal">
                                        <%:GetLabel("Pembayar")%></label>
                                </td>
                                <td>
                                    <table cellpadding="0" cellspacing="0">
                                        <colgroup>
                                            <col style="width: 150px" />
                                            <col style="width: 3px" />
                                        </colgroup>
                                        <tr>
                                            <td>
                                                <dxe:ASPxComboBox ID="cboRegistrationPayer" ClientInstanceName="cboRegistrationPayer"
                                                    Width="100%" runat="server">
                                                    <ClientSideEvents ValueChanged="function(s){ onCboPayerValueChanged(s); }" />
                                                </dxe:ASPxComboBox>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr id="trPayer">
                                <td style="width: 30%" class="tdLabel">
                                    <label class="lblLink lblMandatory" runat="server" id="lblPayerCompany">
                                        <%:GetLabel("Instansi")%></label>
                                </td>
                                <td>
                                    <input type="hidden" id="hdnPayerID" value="" runat="server" />
                                    <input type="hidden" id="hdnGCTariffScheme" value="" runat="server" />
                                    <input type="hidden" id="hdnIsBlacklistPayer" value="" runat="server" />
                                    <table style="width: 100%" cellpadding="0" cellspacing="0">
                                        <colgroup>
                                            <col style="width: 80px" />
                                            <col style="width: 3px" />
                                            <col />
                                        </colgroup>
                                        <tr>
                                            <td>
                                                <asp:TextBox ID="txtPayerCompanyCode" Width="100%" runat="server" />
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtPayerCompanyName" Width="100%" runat="server" ReadOnly=true />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr id="trContract" style="display:none">
                                <td style="width: 30%" class="tdLabel">
                                    <label class="lblLink lblMandatory" runat="server" id="lblContract">
                                        <%:GetLabel("Kontrak")%></label>
                                </td>
                                <td>
                                    <table style="width: 100%" cellpadding="0" cellspacing="0">
                                        <colgroup>
                                            <col style="width: 250px" />
                                            <col style="width: 3px" />
                                            <col />
                                        </colgroup>
                                        <tr>
                                            <td>
                                                <input type="hidden" id="hdnContractID" value="" runat="server" />
                                                <input type="hidden" id="hdnContractCoverageCount" value="" runat="server" />
                                                <input type="hidden" id="hdnContractCoverageMemberCount" value="" runat="server" />
                                                <asp:TextBox ID="txtContractNo" Width="100%" runat="server" />
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td>
                                                <input type="button" id="btnPayerNotesDetail" value="..." />
                                            </td>
                                            <td>
                                                <input type="button" id="btnCustomerContractDocumentInfo" value="Informasi Instansi"
                                                    style="width: 100%;" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr id="trScheme" style="display:none">
                                <td style="width: 30%" class="tdLabel">
                                    <label class="lblLink lblMandatory" runat="server" id="lblCoverageType">
                                        <%:GetLabel("Skema Penjaminan")%></label>
                                </td>
                                <td>
                                    <input type="hidden" id="hdnCoverageTypeID" value="" runat="server" />
                                    <table style="width: 100%" cellpadding="0" cellspacing="0">
                                        <colgroup>
                                            <col style="width: 80px" />
                                            <col style="width: 3px" />
                                            <col />
                                        </colgroup>
                                        <tr>
                                            <td>
                                                <asp:TextBox ID="txtCoverageTypeCode" Width="100%" runat="server" />
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtCoverageTypeName" Width="100%" runat="server" ReadOnly="true" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr id="trProcessType">
                                <td class="tdLabel">
                                    <label class="lblNormal">
                                        <%=GetLabel("Proses Menjadi")%></label>
                                </td>
                                <td>
                                    <asp:RadioButtonList ID="rblProcessType" runat="server" RepeatDirection="Vertical">
                                        <asp:ListItem Text="Perjanjian" Value="0" Selected="True" /> 
                                        <asp:ListItem Text="Registrasi" Value="1" />
                                    </asp:RadioButtonList>
                                </td>
                            </tr>
                            <tr style="display:none">
                                <td class="tdLabel">
                                    <label class="lblMandatory lblLink" runat="server" id="lblServiceUnit">
                                        <%=GetLabel("Klinik")%></label>
                                </td>
                                <td>
                                    <input type="hidden" id="hdnHealthcareServiceUnitID" value="" runat="server" />
                                    <table style="width: 100%" cellpadding="0" cellspacing="0">
                                        <colgroup>
                                            <col style="width: 120px" />
                                            <col style="width: 3px" />
                                            <col />
                                        </colgroup>
                                        <tr>
                                            <td>
                                                <asp:TextBox ID="txtServiceUnitCode" Width="100%" runat="server" />
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtServiceUnitName" Width="100%" runat="server" ReadOnly="true" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr style="display:none">
                                <td colspan=2>
                                    <asp:CheckBox ID="chkShowAllAppointmentRequest" runat="server" /><%=GetLabel("  Tampilkan semua permintaan perjanjian")%>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
                            ShowLoadingPanel="false" OnCallback="cbpView_Callback">
                            <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ onCbpViewEndCallback(s); }" />
                            <PanelCollection>
                                <dx:PanelContent ID="PanelContent1" runat="server">
                                    <asp:Panel runat="server" ID="pnlView" CssClass="pnlContainerGrid">
                                        <asp:ListView runat="server" ID="lvwView" OnItemDataBound="lvwView_ItemDataBound">
                                            <EmptyDataTemplate>
                                                <table id="tblView" runat="server" class="grdMCUOrder grdSelected" cellspacing="0"
                                                    rules="all">
                                                    <tr>
                                                        <th class="keyField">
                                                            &nbsp;
                                                        </th>
                                                         <th style="width: 10px" align="center">
                                                            <input id="chkSelectAll" type="checkbox" />
                                                        </th>
                                                        <th align="left">
                                                            <b><%=GetLabel("No Rekam Medis")%></b><br />
                                                            <%=GetLabel("Nama Pasien")%><br />
                                                            <i><%=GetLabel("Informasi Karyawan")%></i>
                                                        </th>
                                                        <th align="left" style="display:none">
                                                            <%=GetLabel("Pasien")%>
                                                        </th>
                                                        <th align="left" style="display:none">
                                                            <%=GetLabel("Klinik")%>
                                                        </th>
                                                        <th align="center">
                                                            <%=GetLabel("Batch No")%>
                                                        </th>
                                                        <th align="left">
                                                            <%=GetLabel("Paket MCU")%>
                                                        </th>
                                                        <th align="left">
                                                            <%=GetLabel("Dokter")%>
                                                        </th>
                                                        <th align="left">
                                                            <%=GetLabel("Penjamin")%>
                                                        </th>
                                                        <th align="center">
                                                            <%=GetLabel("Tanggal ")%>
                                                        </th>
                                                    </tr>
                                                    <tr class="trEmpty">
                                                        <td colspan="20">
                                                            <%=GetLabel("Tidak ada data.")%>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </EmptyDataTemplate>
                                            <LayoutTemplate>
                                                <table id="tblView" runat="server" class="grdMCUOrder grdSelected" cellspacing="0"
                                                    rules="all">
                                                    <tr>
                                                        <th class="keyField">
                                                            &nbsp;
                                                        </th>
                                                         <th style="width: 10px" align="center">
                                                            <input id="chkSelectAll" type="checkbox" />
                                                        </th>
                                                        <th align="left" style="width:300px">
                                                            <b><%=GetLabel("No Rekam Medis")%></b><br />
                                                            <%=GetLabel("Nama Pasien")%><br />
                                                            <i><%=GetLabel("Informasi Karyawan")%></i>
                                                        </th>
                                                        <th align="left" style="display:none">
                                                            <%=GetLabel("Pasien")%>
                                                        </th>
                                                        <th align="left" style="display:none">
                                                            <%=GetLabel("Klinik")%>
                                                        </th>
                                                        <th align="center" style="width:150px">
                                                            <%=GetLabel("Batch No")%>
                                                        </th>
                                                        <th align="left" style="width:150px">
                                                            <%=GetLabel("Paket MCU")%>
                                                        </th>
                                                        <th align="left" style="width:150px">
                                                            <%=GetLabel("Dokter")%>
                                                        </th>
                                                        <th align="left" style="width:100px">
                                                            <%=GetLabel("Penjamin")%>
                                                        </th>
                                                        <th align="center" style="width:150px">
                                                            <%=GetLabel("Tanggal ")%>
                                                        </th>
                                                    </tr>
                                                    <tr runat="server" id="itemPlaceholder">
                                                    </tr>
                                                </table>
                                            </LayoutTemplate>
                                            <ItemTemplate>
                                                <tr>
                                                    <td class="keyField">
                                                        <%#: Eval("AppointmentRequestID")%>
                                                    </td>
                                                    <td align="center">
                                                        <div style="padding: 3px">
                                                            <asp:CheckBox ID="chkIsSelected" CssClass="chkIsSelected" runat="server" />
                                                            <input type="hidden" class="hdnKeyField" value="<%#: Eval("AppointmentRequestID")%>" />
                                                        </div>
                                                    </td>
                                                    <td align="left">
                                                        <input type="hidden" value="" class="hdnKey" id="hdnKey" runat="server" />
                                                        <b><%#: Eval("cfMedicalNo")%></b><br />
                                                        <%#: Eval("cfPatientName")%><br />
                                                        <i>(<%#: Eval("CorporateAccountNo")%>) <%#: Eval("CorporateAccountName")%></i>
                                                    </td>
                                                    <td align="left" style="display:none">
                                                        <%#: Eval("cfPatientName")%>
                                                    </td>
                                                    <td align="left" style="display:none">
                                                        <%#: Eval("ServiceUnitName")%>
                                                    </td>
                                                    <td align="left">
                                                        <%#: Eval("RequestBatchNo")%>
                                                    </td>
                                                    <td align="left">
                                                        <i><%#: Eval("ItemCode")%></i> <br />
                                                        <%#: Eval("ItemName1")%>
                                                    </td>
                                                    <td align="left">
                                                        <input type="hidden" value="" class="hdnHealthcareServiceUnitPerRowID" id="hdnHealthcareServiceUnitPerRowID"
                                                            runat="server" />
                                                        <input type="hidden" value="" class="hdnParamedicID" id="hdnParamedicID" runat="server" />
                                                        <input type="hidden" value="" class="hdnCorporateAccountNo" id="hdnCorporateAccountNo" runat="server" />
                                                        <input type="hidden" value="" class="hdnCorporateAccountName" id="hdnCorporateAccountName" runat="server" />
                                                        <input type="hidden" value="" class="hdnAppointmentID" id="hdnAppointmentID" runat="server" />
                                                        <input type="hidden" value="" class="hdnRegistrationID" id="hdnRegistrationID" runat="server" />
                                                        <label class="lblParamedicName lblLink" runat="server" id="lblParamedicName">
                                                            Pilih Dokter
                                                        </label>
                                                    </td>
                                                    <td align="left">
                                                        <input type="hidden" value="" class="hdnGCCustomerType" id="hdnGCCustomerType" runat="server" />
                                                        <label class="lblCustomerType lblLink" runat="server" id="lblCustomerType">
                                                            Ubah Penjamin
                                                        </label>
                                                    </td>
                                                    <td align="center">
                                                        <asp:TextBox ID="txtAppointmentDate" Width="120px" runat="server" CssClass="txtAppointmentDate datepicker" MaxLength=10 />
                                                    </td> 
                                                    <td align="left" style="display:none">
                                                        <table border="0" cellpadding="2" cellspacing="0">
                                                            <tr>
                                                                <td <%# Eval("AppointmentNo").ToString() != "" ? "Style='display:none'":"" %>>
                                                                    <input type="button" id="btnSave" class="btnSave" value="Simpan" runat="server" />
                                                                </td>
                                                                <td <%# Eval("AppointmentNo").ToString() != "" ? "Style='display:none'":"" %>>
                                                                    <input type="button" id="btnVoid" class="btnVoid" value="Batal" runat="server" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <input type="button" id="btnPrint" class="btnPrint" value="Print" runat="server" />
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
                        <div class="imgLoadingGrdView" id="containerImgLoadingView">
                            <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                        </div>
                    </td>
                </tr>
            </table>
        </asp:Panel>
    </div>
    <div id="containerAppointment" class="containerInfo" runat="server" style="display:none">
        <div class="pageTitle">
            <%=GetLabel("Daftar Pasien : Sudah Appointment")%></div>
        <asp:Panel ID="panel2" runat="server" DefaultButton="btnDownload">
            <table class="tblContentArea" id="Table1">
                <colgroup>
                    <col style="width: 50%" />
                    <col style="width: 50%" />
                </colgroup>
                <tr>
                    <td style="padding: 5px; vertical-align: top">
                        <table class="tblEntryContent" style="width: 100%">
                            <colgroup>
                                <col style="width: 200px" />
                                <col />
                            </colgroup>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblNormal">
                                        <%=GetLabel("Tanggal")%></label>
                                </td>
                                <td>
                                    <table cellpadding="0" cellspacing="0">
                                        <colgroup>
                                            <col style="width: 145px" />
                                            <col style="width: 3px" />
                                            <col style="width: 145px" />
                                        </colgroup>
                                        <tr>
                                            <td>
                                                <asp:TextBox ID="txtFromAppointmentDateApm" Width="120px" CssClass="datepicker"
                                                    runat="server" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblNormal">
                                        <%:GetLabel("Pembayar")%></label>
                                </td>
                                <td>
                                    <table cellpadding="0" cellspacing="0">
                                        <colgroup>
                                            <col style="width: 150px" />
                                            <col style="width: 3px" />
                                        </colgroup>
                                        <tr>
                                            <td>
                                                <dxe:ASPxComboBox ID="cboRegistrationPayerApm" ClientInstanceName="cboRegistrationPayerApm"
                                                    Width="100%" runat="server">
                                                    <ClientSideEvents ValueChanged="function(s){ onCboPayerValueChangedApm(s); }" />
                                                </dxe:ASPxComboBox>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr id="trPayerApm">
                                <td style="width: 30%" class="tdLabel">
                                    <label class="lblLink lblMandatory" runat="server" id="lblPayerCompanyApm">
                                        <%:GetLabel("Instansi")%></label>
                                </td>
                                <td>
                                    <input type="hidden" id="hdnPayerIDApm" value="" runat="server" />
                                    <table style="width: 100%" cellpadding="0" cellspacing="0">
                                        <colgroup>
                                            <col style="width: 80px" />
                                            <col style="width: 3px" />
                                            <col />
                                        </colgroup>
                                        <tr>
                                            <td>
                                                <asp:TextBox ID="txtPayerCompanyCodeApm" Width="100%" runat="server" />
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtPayerCompanyNameApm" Width="100%" runat="server" ReadOnly=true />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <dxcp:ASPxCallbackPanel ID="cbpViewApm" runat="server" Width="100%" ClientInstanceName="cbpViewApm"
                            ShowLoadingPanel="false" OnCallback="cbpViewApm_Callback">
                            <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ onCbpViewApmEndCallback(s); }" />
                            <PanelCollection>
                                <dx:PanelContent ID="PanelContent2" runat="server">
                                    <asp:Panel runat="server" ID="Panel3" CssClass="pnlContainerGrid">
                                        <asp:ListView runat="server" ID="lvwViewApm" OnItemDataBound="lvwViewApm_ItemDataBound">
                                            <EmptyDataTemplate>
                                                <table id="tblView" runat="server" class="grdMCUOrderApm grdSelected" cellspacing="0"
                                                    rules="all">
                                                    <tr>
                                                        <th class="keyField">
                                                            &nbsp;
                                                        </th>
                                                         <th style="width: 10px" align="center">
                                                            <input id="chkSelectAllApm" type="checkbox" />
                                                        </th>
                                                        <th align="left">
                                                            <b><%=GetLabel("No Rekam Medis")%></b><br />
                                                            <%=GetLabel("Nama Pasien")%><br />
                                                            <i><%=GetLabel("Informasi Karyawan")%></i>
                                                        </th>
                                                        <th align="left" style="display:none">
                                                            <%=GetLabel("Pasien")%>
                                                        </th>
                                                        <th align="left" style="display:none">
                                                            <%=GetLabel("Klinik")%>
                                                        </th>
                                                        <th align="left">
                                                            <%=GetLabel("Paket MCU")%>
                                                        </th>
                                                        <th align="left">
                                                            <%=GetLabel("Dokter")%>
                                                        </th>
                                                        <th align="left">
                                                            <%=GetLabel("Penjamin")%>
                                                        </th>
                                                        <th align="center">
                                                            <%=GetLabel("Tanggal ")%>
                                                        </th>
                                                        <th align="left">
                                                            <%=GetLabel("Informasi Appointment ")%>
                                                        </th>
                                                        <th align="left">
                                                            <%=GetLabel("Informasi Registrasi ")%>
                                                        </th>
                                                    </tr>
                                                    <tr class="trEmpty">
                                                        <td colspan="20">
                                                            <%=GetLabel("Tidak ada data.")%>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </EmptyDataTemplate>
                                            <LayoutTemplate>
                                                <table id="tblView" runat="server" class="grdMCUOrderApm grdSelected" cellspacing="0"
                                                    rules="all">
                                                    <tr>
                                                        <th class="keyField">
                                                            &nbsp;
                                                        </th>
                                                         <th style="width: 10px" align="center">
                                                            <input id="chkSelectAllApm" type="checkbox" />
                                                        </th>
                                                        <th align="left" style="width:300px">
                                                            <b><%=GetLabel("No Rekam Medis")%></b><br />
                                                            <%=GetLabel("Nama Pasien")%><br />
                                                            <i><%=GetLabel("Informasi Karyawan")%></i>
                                                        </th>
                                                        <th align="left" style="display:none">
                                                            <%=GetLabel("Pasien")%>
                                                        </th>
                                                        <th align="left" style="display:none">
                                                            <%=GetLabel("Klinik")%>
                                                        </th>
                                                        <th align="left" style="width:150px">
                                                            <%=GetLabel("Paket MCU")%>
                                                        </th>
                                                        <th align="left" style="width:150px">
                                                            <%=GetLabel("Dokter")%>
                                                        </th>
                                                        <th align="left" style="width:100px">
                                                            <%=GetLabel("Penjamin")%>
                                                        </th>
                                                        <th align="center" style="width:150px">
                                                            <%=GetLabel("Tanggal ")%>
                                                        </th>
                                                        <th align="left">
                                                            <%=GetLabel("Informasi Appointment ")%>
                                                        </th>
                                                    </tr>
                                                    <tr runat="server" id="itemPlaceholder">
                                                    </tr>
                                                </table>
                                            </LayoutTemplate>
                                            <ItemTemplate>
                                                <tr>
                                                    <td class="keyField">
                                                        <%#: Eval("AppointmentRequestID")%>
                                                    </td>
                                                    <td align="center">
                                                        <div style="padding: 3px">
                                                            <asp:CheckBox ID="chkIsSelectedApm" CssClass="chkIsSelectedApm" runat="server" />
                                                            <input type="hidden" class="hdnKeyFieldApm" value="<%#: Eval("AppointmentRequestID")%>" />
                                                        </div>
                                                    </td>
                                                    <td align="left">
                                                        <input type="hidden" value="" class="hdnKeyApm" id="hdnKeyApm" runat="server" />
                                                        <b><%#: Eval("cfMedicalNo")%></b><br />
                                                        <%#: Eval("cfPatientName")%><br />
                                                        <i>(<%#: Eval("CorporateAccountNo")%>) <%#: Eval("CorporateAccountName")%></i>
                                                    </td>
                                                    <td align="left" style="display:none">
                                                        <%#: Eval("cfPatientName")%>
                                                    </td>
                                                    <td align="left" style="display:none">
                                                        <%#: Eval("ServiceUnitName")%>
                                                    </td>
                                                    <td align="left">
                                                        <i><%#: Eval("ItemCode")%></i> <br />
                                                        <%#: Eval("ItemName1")%>
                                                    </td>
                                                    <td align="left">
                                                        <%#: Eval("ParamedicName")%>
                                                    </td>
                                                    <td align="left">
                                                        <%#: Eval("BusinessPartnerName")%>
                                                    </td>
                                                    <td align="left" style="display:none">
                                                        <input type="hidden" value="" class="hdnHealthcareServiceUnitPerRowIDApm" id="hdnHealthcareServiceUnitPerRowIDApm"
                                                            runat="server" />
                                                        <input type="hidden" value="" class="hdnParamedicIDApm" id="hdnParamedicIDApm" runat="server" />
                                                        <input type="hidden" value="" class="hdnCorporateAccountNoApm" id="hdnCorporateAccountNoApm" runat="server" />
                                                        <input type="hidden" value="" class="hdnCorporateAccountNameApm" id="hdnCorporateAccountNameApm" runat="server" />
                                                        <input type="hidden" value="" class="hdnAppointmentIDApm" id="hdnAppointmentIDApm" runat="server" />
                                                        <input type="hidden" value="" class="hdnRegistrationIDApm" id="hdnRegistrationIDApm" runat="server" />
                                                        <label class="lblParamedicNameApm lblLink" runat="server" id="lblParamedicNameApm">
                                                            Pilih Dokter
                                                        </label>
                                                    </td>
                                                    <td align="left" style="display:none">
                                                        <input type="hidden" value="" class="hdnGCCustomerTypeApm" id="hdnGCCustomerTypeApm" runat="server" />
                                                        <label class="lblCustomerTypeApm lblLink" runat="server" id="lblCustomerTypeApm">
                                                            Ubah Penjamin
                                                        </label>
                                                    </td>
                                                    <td align="left">
                                                        <%#: Eval("cfAppointmentDate")%>
                                                    </td>
                                                    <td align="center" style="display:none">
                                                        <asp:TextBox ID="txtAppointmentDateApm" Width="120px" runat="server" CssClass="txtAppointmentDateApm datepicker" />
                                                    </td> 
                                                    <td>
                                                        <%#: Eval("AppointmentNo")%><br />
                                                        <%#: Eval("cfAppointmentDate")%>
                                                    </td>
                                                </tr>
                                            </ItemTemplate>
                                        </asp:ListView>
                                    </asp:Panel>
                                </dx:PanelContent>
                            </PanelCollection>
                        </dxcp:ASPxCallbackPanel>
                        <div class="imgLoadingGrdView" id="containerImgLoadingViewApm">
                            <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                        </div>
                    </td>
                </tr>
            </table>
        </asp:Panel>
    </div>
    <div id="containerRegistration" class="containerInfo" runat="server" style="display:none">
        <div class="pageTitle">
            <%=GetLabel("Daftar Pasien : Sudah Registrasi")%></div>
        <asp:Panel ID="panel4" runat="server" DefaultButton="btnDownload">
            <table class="tblContentArea" id="Table2">
                <colgroup>
                    <col style="width: 50%" />
                    <col style="width: 50%" />
                </colgroup>
                <tr>
                    <td style="padding: 5px; vertical-align: top">
                        <table class="tblEntryContent" style="width: 100%">
                            <colgroup>
                                <col style="width: 200px" />
                                <col />
                            </colgroup>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblNormal">
                                        <%=GetLabel("Tanggal")%></label>
                                </td>
                                <td>
                                    <table cellpadding="0" cellspacing="0">
                                        <colgroup>
                                            <col style="width: 145px" />
                                            <col style="width: 3px" />
                                            <col style="width: 145px" />
                                        </colgroup>
                                        <tr>
                                            <td>
                                                <asp:TextBox ID="txtFromAppointmentDateReg" Width="120px" CssClass="datepicker"
                                                    runat="server" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblNormal">
                                        <%:GetLabel("Pembayar")%></label>
                                </td>
                                <td>
                                    <table cellpadding="0" cellspacing="0">
                                        <colgroup>
                                            <col style="width: 150px" />
                                            <col style="width: 3px" />
                                        </colgroup>
                                        <tr>
                                            <td>
                                                <dxe:ASPxComboBox ID="cboRegistrationPayerReg" ClientInstanceName="cboRegistrationPayerReg"
                                                    Width="100%" runat="server">
                                                    <ClientSideEvents ValueChanged="function(s){ onCboPayerValueChangedReg(s); }" />
                                                </dxe:ASPxComboBox>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr id="trPayerReg">
                                <td style="width: 30%" class="tdLabel">
                                    <label class="lblLink lblMandatory" runat="server" id="lblPayerCompanyReg">
                                        <%:GetLabel("Instansi")%></label>
                                </td>
                                <td>
                                    <input type="hidden" id="hdnPayerIDReg" value="" runat="server" />
                                    <table style="width: 100%" cellpadding="0" cellspacing="0">
                                        <colgroup>
                                            <col style="width: 80px" />
                                            <col style="width: 3px" />
                                            <col />
                                        </colgroup>
                                        <tr>
                                            <td>
                                                <asp:TextBox ID="txtPayerCompanyCodeReg" Width="100%" runat="server" />
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtPayerCompanyNameReg" Width="100%" runat="server" ReadOnly=true />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <dxcp:ASPxCallbackPanel ID="cbpViewReg" runat="server" Width="100%" ClientInstanceName="cbpViewReg"
                            ShowLoadingPanel="false" OnCallback="cbpViewReg_Callback">
                            <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ onCbpViewRegEndCallback(s); }" />
                            <PanelCollection>
                                <dx:PanelContent ID="PanelContent3" runat="server">
                                    <asp:Panel runat="server" ID="Panel5" CssClass="pnlContainerGrid">
                                        <asp:ListView runat="server" ID="lvwViewReg" OnItemDataBound="lvwViewReg_ItemDataBound">
                                            <EmptyDataTemplate>
                                                <table id="tblView" runat="server" class="grdMCUOrderReg grdSelected" cellspacing="0"
                                                    rules="all">
                                                    <tr>
                                                        <th class="keyField">
                                                            &nbsp;
                                                        </th>
                                                         <th style="width: 10px;display:none" align="center">
                                                            <input id="chkSelectAllReg" type="checkbox" />
                                                        </th>
                                                        <th align="left">
                                                            <b><%=GetLabel("No Rekam Medis")%></b><br />
                                                            <%=GetLabel("Nama Pasien")%><br />
                                                            <i><%=GetLabel("Informasi Karyawan")%></i>
                                                        </th>
                                                        <th align="left" style="display:none">
                                                            <%=GetLabel("Pasien")%>
                                                        </th>
                                                        <th align="left" style="display:none">
                                                            <%=GetLabel("Klinik")%>
                                                        </th>
                                                        <th align="left">
                                                            <%=GetLabel("Paket MCU")%>
                                                        </th>
                                                        <th align="left">
                                                            <%=GetLabel("Dokter")%>
                                                        </th>
                                                        <th align="left">
                                                            <%=GetLabel("Penjamin")%>
                                                        </th>
                                                        <th align="left">
                                                            <%=GetLabel("Informasi Appointment")%>
                                                        </th>
                                                        <th align="left">
                                                            <%=GetLabel("Informasi Registrasi ")%>
                                                        </th>
                                                    </tr>
                                                    <tr class="trEmpty">
                                                        <td colspan="20">
                                                            <%=GetLabel("Tidak ada data.")%>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </EmptyDataTemplate>
                                            <LayoutTemplate>
                                                <table id="tblView" runat="server" class="grdMCUOrderReg grdSelected" cellspacing="0"
                                                    rules="all">
                                                    <tr>
                                                        <th class="keyField">
                                                            &nbsp;
                                                        </th>
                                                         <th style="width: 10px;display:none" align="center">
                                                            <input id="chkSelectAllReg" type="checkbox" />
                                                        </th>
                                                        <th align="left" style="width:300px">
                                                            <b><%=GetLabel("No Rekam Medis")%></b><br />
                                                            <%=GetLabel("Nama Pasien")%><br />
                                                            <i><%=GetLabel("Informasi Karyawan")%></i>
                                                        </th>
                                                        <th align="left" style="display:none">
                                                            <%=GetLabel("Pasien")%>
                                                        </th>
                                                        <th align="left" style="display:none">
                                                            <%=GetLabel("Klinik")%>
                                                        </th>
                                                        <th align="left" style="width:150px">
                                                            <%=GetLabel("Paket MCU")%>
                                                        </th>
                                                        <th align="left" style="width:150px">
                                                            <%=GetLabel("Dokter")%>
                                                        </th>
                                                        <th align="left" style="width:200px">
                                                            <%=GetLabel("Penjamin")%>
                                                        </th>
                                                        <th align="left">
                                                            <%=GetLabel("Informasi Appointment")%>
                                                        </th>
                                                        <th align="left">
                                                            <%=GetLabel("Informasi Registrasi ")%>
                                                        </th>
                                                    </tr>
                                                    <tr runat="server" id="itemPlaceholder">
                                                    </tr>
                                                </table>
                                            </LayoutTemplate>
                                            <ItemTemplate>
                                                <tr>
                                                    <td class="keyField">
                                                        <%#: Eval("AppointmentRequestID")%>
                                                    </td>
                                                    <td align="center" style="display:none">
                                                        <div style="padding: 3px">
                                                            <asp:CheckBox ID="chkIsSelectedReg" CssClass="chkIsSelectedReg" runat="server" />
                                                            <input type="hidden" class="hdnKeyFieldReg" value="<%#: Eval("AppointmentRequestID")%>" />
                                                        </div>
                                                    </td>
                                                    <td align="left">
                                                        <input type="hidden" value="" class="hdnKeyReg" id="hdnKeyReg" runat="server" />
                                                        <b><%#: Eval("cfMedicalNo")%></b><br />
                                                        <%#: Eval("cfPatientName")%><br />
                                                        <i>(<%#: Eval("CorporateAccountNo")%>) <%#: Eval("CorporateAccountName")%></i>
                                                    </td>
                                                    <td align="left" style="display:none">
                                                        <%#: Eval("cfPatientName")%>
                                                    </td>
                                                    <td align="left" style="display:none">
                                                        <%#: Eval("ServiceUnitName")%>
                                                    </td>
                                                    <td align="left">
                                                        <i><%#: Eval("ItemCode")%></i> <br />
                                                        <%#: Eval("ItemName1")%>
                                                    </td>
                                                    <td align="left">
                                                        <%#: Eval("ParamedicName")%>
                                                    </td>
                                                    <td align="left">
                                                        <%#: Eval("BusinessPartnerName")%>
                                                    </td>
                                                    <td align="left" style="display:none">
                                                        <input type="hidden" value="" class="hdnHealthcareServiceUnitPerRowIDReg" id="hdnHealthcareServiceUnitPerRowIDReg"
                                                            runat="server" />
                                                        <input type="hidden" value="" class="hdnParamedicIDReg" id="hdnParamedicIDReg" runat="server" />
                                                        <input type="hidden" value="" class="hdnCorporateAccountNoReg" id="hdnCorporateAccountNoReg" runat="server" />
                                                        <input type="hidden" value="" class="hdnCorporateAccountNameReg" id="hdnCorporateAccountNameReg" runat="server" />
                                                        <input type="hidden" value="" class="hdnAppointmentIDReg" id="hdnAppointmentIDReg" runat="server" />
                                                        <input type="hidden" value="" class="hdnRegistrationIDReg" id="hdnRegistrationIDReg" runat="server" />
                                                        <label class="lblParamedicNameReg lblLink" runat="server" id="lblParamedicNameReg">
                                                            Pilih Dokter
                                                        </label>
                                                    </td>
                                                    <td align="left" style="display:none">
                                                        <input type="hidden" value="" class="hdnGCCustomerTypeReg" id="hdnGCCustomerTypeReg" runat="server" />
                                                        <label class="lblCustomerTypeReg lblLink" runat="server" id="lblCustomerTypeReg">
                                                            Ubah Penjamin
                                                        </label>
                                                    </td>
                                                    <td align="center" style="display:none">
                                                        <asp:TextBox ID="txtAppointmentDateReg" Width="120px" runat="server" CssClass="txtAppointmentDateReg datepicker" />
                                                    </td> 
                                                    <td>
                                                        <%#: Eval("AppointmentNo")%><br />
                                                        <%#: Eval("cfAppointmentDate")%>
                                                    </td>
                                                    <td>
                                                        <%#: Eval("RegistrationNo")%><br />
                                                        <%#: Eval("cfRegistrationDate")%>
                                                    </td>
                                                </tr>
                                            </ItemTemplate>
                                        </asp:ListView>
                                    </asp:Panel>
                                </dx:PanelContent>
                            </PanelCollection>
                        </dxcp:ASPxCallbackPanel>
                        <div class="imgLoadingGrdView" id="containerImgLoadingViewReg">
                            <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                        </div>
                    </td>
                </tr>
            </table>
        </asp:Panel>
    </div>
</asp:Content>
