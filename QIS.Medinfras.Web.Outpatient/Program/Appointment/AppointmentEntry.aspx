<%@ Page Language="C#" MasterPageFile="~/Libs/MasterPage/MPTrx.master" AutoEventWireup="true"
    CodeBehind="AppointmentEntry.aspx.cs" Inherits="QIS.Medinfras.Web.Outpatient.Program.AppointmentEntry" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<asp:Content ID="Content2" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div class="menuTitle">
        <%=HttpUtility.HtmlEncode(GetPageTitle())%></div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="plhCustomButtonToolbar" runat="server">
    <li id="btnAppointmentSave" runat="server" crudmode="R">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbsave.png")%>' alt="" /><div>
            <%=GetLabel("Save")%></div>
    </li>
    <li id="btnAppointmentChangeAppointment" runat="server" crudmode="R">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbset.png")%>' alt="" /><div>
            <%=GetLabel("Change Appointment")%></div>
    </li>
    <li id="btnAppointmentVoid" runat="server" crudmode="R">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbvoid.png")%>' alt="" /><div>
            <%=GetLabel("Void")%></div>
    </li>
    <li id="btnAppointmentProcessRegistration" runat="server" crudmode="R">
        <img src="<%=ResolveUrl("~/Libs/Images/Icon/tbprocess.png") %>" alt="" /><div>
            <%=GetLabel("Registrasi") %></div>
    </li>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript">
        var lastAppointmentID = -1;
        $(function () {

            registerCollapseExpandHandler();
            $('#ctxMenuEdit').click(function () {
                var className = $(this).attr('class');
                if (!(typeof className !== 'undefined' && className !== false))
                    $('#<%=btnAppointmentChangeAppointment.ClientID %>').click();
            });
            $('#ctxMenuVoid:not(.disabled)').click(function () {
                var className = $(this).attr('class');
                if (!(typeof className !== 'undefined' && className !== false))
                    $('#<%=btnAppointmentVoid.ClientID %>').click();
            });

            $('#<%=btnAppointmentSave.ClientID %>').click(function (evt) {
                if ($('.grdAppointment li.selected').length == 0) {
                    showToast('Warning', '<%=GetErrMessageSelectAppointmentFirst() %>');
                }
                else {
                    if ($('#<%=hdnGCAppointmentStatus.ClientID %>').val() != '' && $('#<%=hdnGCAppointmentStatus.ClientID %>').val() != Constant.AppointmentStatus.STARTED) {
                        showToast('Warning', '<%=GetErrMessageCompletedAppointment() %>');
                    }
                    else if (IsValid(evt, 'fsAppointment', 'mpAppointment')) {
                        lastAppointmentID = 0;
                        onCustomButtonClick('save');
                    }
                }
            });
            $('#<%=btnAppointmentProcessRegistration.ClientID %>').click(function () {
                if ($('#<%=hdnAppointmentID.ClientID %>').val() != '') {
                    if ($('#<%=hdnGCAppointmentStatus.ClientID %>').val() != Constant.AppointmentStatus.COMPLETE) {
                        var appointmentID = $('#<%=hdnAppointmentID.ClientID %>').val();
                        var url = ResolveUrl("~/Libs/Program/Module/PatientManagement/Registration/RegistrationEntry.aspx?id=OUTPATIENT&appid=" + appointmentID);
                        showLoadingPanel();
                        window.location.href = url;
                    }
                }
                else showToast('Warning', '<%=GetErrMessageSelectAppointmentFirst() %>');
            });

            $('#<%=btnAppointmentVoid.ClientID %>').click(function () {
                if ($('#<%=hdnAppointmentID.ClientID %>').val() != '') {
                    if ($('#<%=hdnGCAppointmentStatus.ClientID %>').val() != Constant.AppointmentStatus.STARTED) {
                        showToast('Warning', '<%=GetErrMessageCompletedAppointment() %>');
                    }
                    else {
                        showToastConfirmation('Are You Sure Want To Void?', function (result) {
                            if (result) {
                                var id = $('#<%=hdnAppointmentID.ClientID %>').val();
                                var url = ResolveUrl("~/Program/Appointment/AppointmentVoidCtl.ascx");
                                openUserControlPopup(url, id, 'Void Appointment', 800, 350);
                            }
                        });
                    }
                }
                else
                    showToast('Warning', '<%=GetErrMessageSelectAppointmentFirst() %>');
            });

            $('#<%=btnAppointmentChangeAppointment.ClientID %>').click(function () {
                if ($('#<%=hdnAppointmentID.ClientID %>').val() != '') {
                    if ($('#<%=hdnGCAppointmentStatus.ClientID %>').val() != Constant.AppointmentStatus.STARTED) {
                        showToast('Warning', '<%=GetErrMessageCompletedAppointment() %>');
                    }
                    else {
                        var id = $('#<%=hdnAppointmentID.ClientID %>').val();
                        var url = ResolveUrl("~/Program/Appointment/AppointmentChangeDateCtl.ascx");
                        openUserControlPopup(url, id, 'Change Appointment Date', 1000, 650);
                    }
                }
                else
                    showToast('Warning', '<%=GetErrMessageSelectAppointmentFirst() %>');
            });

            $("#calAppointment").datepicker({
                defaultDate: "w",
                changeMonth: true,
                changeYear: true,
                dateFormat: "dd-mm-yy",
                //minDate: "0",
                onSelect: function (dateText, inst) {
                    $('#<%=hdnCalAppointmentSelectedDate.ClientID %>').val(dateText);
                    cbpPhysician.PerformCallback('refresh');
                    $('#<%=txtAppointmentDate.ClientID %>').val(dateText);
                }
            });

            //#region Is New Patient
            $('#<%=chkIsNewPatient.ClientID %>').change(function () {
                if ($(this).is(":checked")) {
                    $('#lblMRN').attr('class', 'lblDisabled');
                    $('#<%=txtMRN.ClientID %>').attr('readonly', 'readonly');
                    $('#<%=txtMRN.ClientID %>').removeClass('error');
                    $('#<%=txtFirstName.ClientID %>').removeAttr('readonly');
                    $('#<%=txtMiddleName.ClientID %>').removeAttr('readonly');
                    $('#<%=txtFamilyName.ClientID %>').removeAttr('readonly');
                    $('#<%=txtAddress.ClientID %>').removeAttr('readonly');
                    cboSalutation.SetEnabled(true);
                }
                else {
                    $('#lblMRN').attr('class', 'lblLink');
                    $('#<%=txtMRN.ClientID %>').removeAttr('readonly');
                    $('#<%=txtFirstName.ClientID %>').attr('readonly', 'readonly');
                    $('#<%=txtMiddleName.ClientID %>').attr('readonly', 'readonly');
                    $('#<%=txtFamilyName.ClientID %>').attr('readonly', 'readonly');
                    $('#<%=txtAddress.ClientID %>').attr('readonly', 'readonly');
                    cboSalutation.SetEnabled(false);
                }

                $('#<%=hdnMRN.ClientID %>').val('');
                $('#<%=txtMRN.ClientID %>').val('');
                cboSalutation.SetValue('');
                $('#<%=txtFirstName.ClientID %>').val('');
                $('#<%=txtMiddleName.ClientID %>').val('');
                $('#<%=txtFamilyName.ClientID %>').val('');
                $('#<%=txtAddress.ClientID %>').val('');
                $('#<%=txtPhoneNo.ClientID %>').val('');
                $('#<%=txtMobilePhone.ClientID %>').val('');
                $('#<%=txtEmail.ClientID %>').val('');
            });
            $('#<%=chkIsNewPatient.ClientID %>').change();
            //#endregion

            $('#<%=grdPhysician.ClientID %> > tbody > tr:gt(0):not(.trDetail):not(.trEmpty)').live('click', function () {
                $('#<%=grdPhysician.ClientID %> > tbody > tr.selected').removeClass('selected');
                $(this).addClass('selected');
                $('#<%=hdnParamedicID.ClientID %>').val($(this).find('.keyField').html());
                $('#<%=txtPhysician.ClientID %>').val($(this).find('.tdParamedicName').html());

                cbpAppointment.PerformCallback();
            });
            $('#<%=grdPhysician.ClientID %> > tbody > tr:eq(1)').click();

            //#region Grd Appointment
            $('.grdAppointment > tbody > tr:gt(0):not(.trDetail):not(.trEmpty) td.tdAppointment li').live('click', function (evt) {
                //if ($('#<%=grdAppointment.ClientID %> > tbody > tr.selected').html() != $(this).html()) {
                $tr = $(this).closest('tr');
                var appointmentID = parseInt($(this).find('.hdnAppointmentID').val());
                if (appointmentID > -2) {
                    $('.grdAppointment li.selected').removeClass('selected');
                    $(this).addClass('selected');
                    var time = '00:00';
                    if ($('#<%=hdnIsWaitingList.ClientID %>').val() == '0') {
                        time = $.trim($tr.find('.tdTime').html());
                    }
                    $('#<%=txtAppointmentHour.ClientID %>').val(time);
                    if (appointmentID > -1) {
                        var GCAppointmentStatus = $(this).find('.hdnGCAppointmentStatus').val();
                        var filterExpression = "AppointmentID = " + appointmentID;
                        Methods.getObject('GetvAppointmentList', filterExpression, function (result) {
                            $('#<%=hdnGCAppointmentStatus.ClientID %>').val(GCAppointmentStatus);
                            $('#<%=hdnAppointmentID.ClientID %>').val(result.AppointmentID);
                            $('#<%=txtAppointmentNo.ClientID %>').val(result.AppointmentNo);
                            $('#<%=txtQueueNo.ClientID %>').val(result.QueueNo);
                            $('#<%=txtAddress.ClientID %>').val(result.StreetName);
                            $('#<%=txtPhoneNo.ClientID %>').val(result.PhoneNo);
                            $('#<%=txtMobilePhone.ClientID %>').val(result.MobilePhoneNo);
                            $('#<%=txtEmail.ClientID %>').val(result.EmailAddress);
                            cboSalutation.SetValue(result.Salutation);
                            $('#<%=txtFirstName.ClientID %>').val(result.FirstName);
                            $('#<%=txtMiddleName.ClientID %>').val(result.MiddleName);
                            $('#<%=txtFamilyName.ClientID %>').val(result.LastName);
                            $('#<%=txtMRN.ClientID %>').val(result.MedicalNo);
                            $('#<%=txtRemarks.ClientID %>').val(result.Notes);
                            $('#<%=hdnRoomID.ClientID %>').val(result.RoomID);
                            $('#<%=txtRoomCode.ClientID %>').val(result.RoomCode);
                            $('#<%=txtRoomName.ClientID %>').val(result.RoomName);
                            $('#<%=txtVisitDuration.ClientID %>').val(result.VisitDuration);
                            $('#<%=hdnVisitTypeID.ClientID %>').val(result.VisitTypeID);
                            $('#<%=txtVisitTypeCode.ClientID %>').val(result.VisitTypeCode);
                            $('#<%=txtVisitTypeName.ClientID %>').val(result.VisitTypeName);

                            $('#<%=chkIsNewPatient.ClientID %>').attr("disabled", true);
                            if (result.MedicalNo == '')
                                $('#<%=chkIsNewPatient.ClientID %>').prop('checked', true);
                            $('#lblMRN').attr('class', 'lblDisabled');
                            $('#<%=txtMRN.ClientID %>').attr('readonly', 'readonly');

                            if (GCAppointmentStatus == Constant.AppointmentStatus.COMPLETE) {
                                $('#<%=txtVisitTypeCode.ClientID %>').attr('readonly', 'readonly');
                                $('#<%=txtRemarks.ClientID %>').attr('readonly', 'readonly');
                                $('#<%=txtPhoneNo.ClientID %>').attr('readonly', 'readonly');
                                $('#<%=txtMobilePhone.ClientID %>').attr('readonly', 'readonly');
                                $('#<%=txtEmail.ClientID %>').attr('readonly', 'readonly');
                            }
                            else {
                                $('#<%=txtVisitTypeCode.ClientID %>').removeAttr('readonly');
                                $('#<%=txtRemarks.ClientID %>').removeAttr('readonly');
                                $('#<%=txtPhoneNo.ClientID %>').removeAttr('readonly');
                                $('#<%=txtMobilePhone.ClientID %>').removeAttr('readonly');
                                $('#<%=txtEmail.ClientID %>').removeAttr('readonly');
                            }
                        });
                    }
                    else if (lastAppointmentID > -1) {
                        $('#<%=hdnGCAppointmentStatus.ClientID %>').val('');
                        $('#<%=hdnAppointmentID.ClientID %>').val('');
                        $('#<%=txtAppointmentNo.ClientID %>').val('');
                        $('#<%=txtQueueNo.ClientID %>').val('');
                        $('#<%=txtAddress.ClientID %>').val('');
                        $('#<%=txtPhoneNo.ClientID %>').val('');
                        $('#<%=txtMobilePhone.ClientID %>').val('');
                        $('#<%=txtEmail.ClientID %>').val('');
                        cboSalutation.SetValue('');
                        $('#<%=txtFirstName.ClientID %>').val('');
                        $('#<%=txtMiddleName.ClientID %>').val('');
                        $('#<%=txtFamilyName.ClientID %>').val('');
                        $('#<%=txtMRN.ClientID %>').val('');
                        $('#<%=txtRemarks.ClientID %>').val('');
                        $('#<%=txtVisitDuration.ClientID %>').val('');
                        $('#<%=hdnVisitTypeID.ClientID %>').val('');
                        $('#<%=txtVisitTypeCode.ClientID %>').val('');
                        $('#<%=txtVisitTypeName.ClientID %>').val('');
                        $('#<%=hdnRoomID.ClientID %>').val('');
                        $('#<%=txtRoomCode.ClientID %>').val('');
                        $('#<%=txtRoomName.ClientID %>').val('');

                        $('#<%=chkIsNewPatient.ClientID %>').prop('checked', false);
                        $('#<%=chkIsNewPatient.ClientID %>').removeAttr('disabled');

                        $('#<%=txtVisitTypeCode.ClientID %>').removeAttr('readonly');
                        $('#<%=txtRemarks.ClientID %>').removeAttr('readonly');
                        $('#<%=txtPhoneNo.ClientID %>').removeAttr('readonly');
                        $('#<%=txtMobilePhone.ClientID %>').removeAttr('readonly');
                        $('#<%=txtEmail.ClientID %>').removeAttr('readonly');

                        $('#<%=txtMRN.ClientID %>').removeAttr('readonly');
                        $('#lblMRN').attr('class', 'lblLink');
                        $('#<%=txtMRN.ClientID %>').removeAttr('readonly');
                    }
                    lastAppointmentID = appointmentID;
                }
                //}
            });

            $('#<%=grdAppointment.ClientID %> tr:gt(0) td.tdExpand').live('click', function () {
                $imgExpand = $(this).find('img');
                if ($imgExpand.is(":visible")) {
                    var id = $(this).parent().find('.keyField').html();

                    $hdnIsExpand = $(this).find('.hdnIsExpand');
                    var isVisible = true;
                    if ($hdnIsExpand.val() == '0') {
                        $(this).find('.imgExpand').attr('src', '<%= ResolveUrl("~/Libs/Images/down-arrow.png")%>');
                        $hdnIsExpand.val('1');
                        isVisible = false;
                    }
                    else {
                        $(this).find('.imgExpand').attr('src', '<%= ResolveUrl("~/Libs/Images/right-arrow.png")%>');
                        $hdnIsExpand.val('0');
                        isVisible = true;
                    }

                    $('#<%=grdAppointment.ClientID %> input[parentid=' + id + ']').each(function () {
                        if (!isVisible)
                            $(this).closest('tr').show('slow');
                        else
                            $(this).closest('tr').hide('fast');
                    });
                }
            });

            $('.grdAppointment > tbody > tr:gt(0):not(.trDetail):not(.trEmpty)').live("contextmenu", function (e) {
                if (e.button === 2) {
                    e.preventDefault();
                    $(this).click();
                    var appointmentID = parseInt($(this).find('.hdnAppointmentID').val());
                    var GCAppointmentStatus = $(this).find('.hdnGCAppointmentStatus').val();
                    if (appointmentID > -2) {
                        if (appointmentID < 0 || GCAppointmentStatus != Constant.AppointmentStatus.STARTED) {
                            $('#ctxMenuEdit').attr('class', 'disabled');
                            $('#ctxMenuVoid').attr('class', 'disabled');
                        }
                        else {
                            $('#ctxMenuEdit').removeAttr('class');
                            $('#ctxMenuVoid').removeAttr('class');
                        }
                        showContextMenu($("#ctxMenuAppointment"), e);
                    }
                }
            });

            $(document).click(function (event) {
                $("#ctxMenuAppointment").hide();
            });

            $(window).blur(function () {
                $("#ctxMenuAppointment").hide();
            });

            //#endregion


        });

        $('#ulTabClinicTransaction li').live('click', function () {
            $('.grdAppointment li.selected').removeClass('selected');
            var name = $(this).attr('contentid');
            if (name == 'containerWaitingList') {
                $('#<%=hdnIsWaitingList.ClientID %>').val('1');
            }
            else $('#<%=hdnIsWaitingList.ClientID %>').val('0');
            $(this).addClass('selected');
            $('#' + name).removeAttr('style');
            $('#ulTabClinicTransaction li').each(function () {
                var tempNameContainer = $(this).attr('contentid');
                if (tempNameContainer != name) {
                    $(this).removeClass('selected');
                    $('#' + tempNameContainer).attr('style', 'display:none');
                }
            });
        });

        function registerGrdAppointmentHandler() {
            var timer = null;
            $('.tdAppointmentInformation').hover(function () {
                $appointmentID = parseInt($(this).closest('tr').find('.hdnAppointmentID').val());
                if ($appointmentID > 0) {
                    $td = $(this);
                    timer = setTimeout(function () {
                        $td.find('.divAppointmentInformationDt').show('fast');
                    }, 300);
                }
            }, function () {
                if ($appointmentID > 0) {
                    if (timer) {
                        clearTimeout(timer);
                    }
                    $(this).find('.divAppointmentInformationDt').hide();
                }
            });


        }

        function onAfterSaveAddRecordEntryPopup() {
            cbpAppointment.PerformCallback();
        }

        function onCbpAppointmentEndCallback(s) {
            var idx = parseInt(s.cpResult);
            if (idx == 0)
                idx = 1;
            $('#<%=hdnIsWaitingList.ClientID %>').val('0');
            $('#<%=grdAppointment.ClientID %> > tbody > tr:eq(' + idx + ') td.tdAppointment li').click();
            hideLoadingPanel();
            registerGrdAppointmentHandler();
            registerCollapseExpandHandler();
        }

        function onAfterCustomClickSuccess(type) {
            if (type == 'save')
                cbpAppointment.PerformCallback();

        }

        //#region Paging
        var pageCount = parseInt('<%=PageCount %>');
        $(function () {
            setPaging($("#pagingPhysican"), pageCount, function (page) {
                cbpPhysician.PerformCallback('changepage|' + page);
            });
        });

        function onCbpPhysicianEndCallback(s) {
            hideLoadingPanel();

            var param = s.cpResult.split('|');
            if (param[0] == 'refresh') {
                var pageCount = parseInt(param[1]);
                if (pageCount > 0)
                    $('#<%=grdPhysician.ClientID %> tr:eq(1)').click();

                setPaging($("#paging"), pageCount, function (page) {
                    cbpPhysician.PerformCallback('changepage|' + page);
                });
            }
            else
                $('#<%=grdPhysician.ClientID %> tr:eq(1)').click();
        }
        //#endregion

        //#region MRN
        $('#lblMRN.lblLink').live('click', function () {
            openSearchDialog('patient', '', function (value) {
                $('#<%=txtMRN.ClientID %>').val(value);
                onTxtMRNChanged(value);
            });
        });
        $('#<%=txtMRN.ClientID %>').live('change', function () {
            onTxtMRNChanged($(this).val());
        });
        function onTxtMRNChanged(value) {
            var filterExpression = "MedicalNo = '" + value + "' AND IsDeleted = 0";
            Methods.getObject('GetvPatientList', filterExpression, function (result) {
                if (result != null) {
                    $('#<%=hdnMRN.ClientID %>').val(result.MRN);
                    cboSalutation.SetValue(result.Salutation);
                    $('#<%=txtFirstName.ClientID %>').val(result.FirstName);
                    $('#<%=txtMiddleName.ClientID %>').val(result.MiddleName);
                    $('#<%=txtFamilyName.ClientID %>').val(result.LastName);
                    $('#<%=txtAddress.ClientID %>').val(result.HomeAddress);
                    $('#<%=txtPhoneNo.ClientID %>').val(result.PhoneNo1);
                    $('#<%=txtMobilePhone.ClientID %>').val(result.MobilePhoneNo1);
                    $('#<%=txtEmail.ClientID %>').val(result.EmailAddress);
                }
                else {
                    $('#<%=hdnMRN.ClientID %>').val('');
                    $('#<%=txtMRN.ClientID %>').val('');
                    cboSalutation.SetValue('');
                    $('#<%=txtFirstName.ClientID %>').val('');
                    $('#<%=txtMiddleName.ClientID %>').val('');
                    $('#<%=txtFamilyName.ClientID %>').val('');
                    $('#<%=txtAddress.ClientID %>').val('');
                    $('#<%=txtPhoneNo.ClientID %>').val('');
                    $('#<%=txtMobilePhone.ClientID %>').val('');
                    $('#<%=txtEmail.ClientID %>').val('');
                }
            });
        }
        //#endregion

        //#region Visit Type
        $('#lblVisitType').live('click', function () {
            var serviceUnitID = cboServiceUnit.GetValue();
            var paramedicID = $('#<%=hdnParamedicID.ClientID %>').val();
            if (paramedicID != '') {
                var filterExpression = "HealthcareServiceUnitID = " + serviceUnitID + " AND ParamedicID = " + paramedicID;
                Methods.getValue("GetParamedicVisitTypeRowCount", filterExpression, function (result) {
                    var filterExpression = '';
                    if (result > 0) {
                        filterExpression = "VisitTypeID IN (SELECT VisitTypeID FROM ParamedicVisitType WHERE HealthcareServiceUnitID = " + serviceUnitID + " AND ParamedicID = " + paramedicID + ")";
                        openSearchDialog('visittype', filterExpression, function (value) {
                            $('#<%=txtVisitTypeCode.ClientID %>').val(value);
                            onTxtVisitTypeCodeChanged(value);
                        });
                    }
                    else {
                        var filterExpression = 'HealthcareServiceUnitID = ' + serviceUnitID;
                        Methods.getObject('GetvHealthcareServiceUnitCustomList', filterExpression, function (result) {
                            if (result.IsHasVisitType)
                                filterExpression = "VisitTypeID IN (SELECT VisitTypeID FROM ServiceUnitVisitType WHERE HealthcareServiceUnitID = " + serviceUnitID + ")";
                            else
                                filterExpression = '';
                            openSearchDialog('visittype', filterExpression, function (value) {
                                $('#<%=txtVisitTypeCode.ClientID %>').val(value);
                                onTxtVisitTypeCodeChanged(value);
                            });
                        });
                    }
                });
            }
            else {
                showToast('Warning', 'Silahkan Pilih Dokter Terlebih Dahulu');            
            }
        });

        $('#<%=txtVisitTypeCode.ClientID %>').live('change', function () {
            onTxtVisitTypeCodeChanged($(this).val());
        });

        function onTxtVisitTypeCodeChanged(value) {
            var filterExpression = '';

            var serviceUnitID = cboServiceUnit.GetValue();
            var paramedicID = $('#<%=hdnParamedicID.ClientID %>').val();
            filterExpression = "HealthcareServiceUnitID = " + serviceUnitID + " AND ParamedicID = " + paramedicID;
            Methods.getValue("GetParamedicVisitTypeRowCount", filterExpression, function (result) {
                var filterExpression = '';
                if (result > 0) {
                    filterExpression += "HealthcareServiceUnitID = " + serviceUnitID + " AND ParamedicID = " + paramedicID + " AND VisitTypeCode = '" + value + "'";
                    Methods.getObject('GetvParamedicVisitTypeList', filterExpression, function (result) {
                        if (result != null) {
                            $('#<%=hdnVisitTypeID.ClientID %>').val(result.VisitTypeID);
                            $('#<%=txtVisitTypeName.ClientID %>').val(result.VisitTypeName);
                            $('#<%=txtVisitDuration.ClientID %>').val(result.VisitDuration);
                        }
                        else {
                            $('#<%=hdnVisitTypeID.ClientID %>').val('');
                            $('#<%=txtVisitTypeCode.ClientID %>').val('');
                            $('#<%=txtVisitTypeName.ClientID %>').val('');
                            $('#<%=txtVisitDuration.ClientID %>').val('');
                        }
                    });
                }
                else {
                    var filterExpression = 'HealthcareServiceUnitID = ' + serviceUnitID;
                    Methods.getObject('GetvHealthcareServiceUnitCustomList', filterExpression, function (result) {
                        if (result.IsHasVisitType) {
                            filterExpression = "HealthcareServiceUnitID = " + serviceUnitID + " AND VisitTypeCode = '" + value + "'";
                            Methods.getObject('GetvServiceUnitVisitTypeList', filterExpression, function (result) {
                                if (result != null) {
                                    $('#<%=hdnVisitTypeID.ClientID %>').val(result.VisitTypeID);
                                    $('#<%=txtVisitTypeName.ClientID %>').val(result.VisitTypeName);
                                    $('#<%=txtVisitDuration.ClientID %>').val(result.VisitDuration);
                                }
                                else {
                                    $('#<%=hdnVisitTypeID.ClientID %>').val('');
                                    $('#<%=txtVisitTypeCode.ClientID %>').val('');
                                    $('#<%=txtVisitTypeName.ClientID %>').val('');
                                    $('#<%=txtVisitDuration.ClientID %>').val('');
                                }
                            });
                        }
                        else {
                            filterExpression = "VisitTypeCode = '" + value + "' AND IsDeleted = 0";
                            Methods.getObject('GetVisitTypeList', filterExpression, function (result) {
                                if (result != null) {
                                    $('#<%=hdnVisitTypeID.ClientID %>').val(result.VisitTypeID);
                                    $('#<%=txtVisitTypeName.ClientID %>').val(result.VisitTypeName);
                                    $('#<%=txtVisitDuration.ClientID %>').val('15');
                                }
                                else {
                                    $('#<%=hdnVisitTypeID.ClientID %>').val('');
                                    $('#<%=txtVisitTypeCode.ClientID %>').val('');
                                    $('#<%=txtVisitTypeName.ClientID %>').val('');
                                    $('#<%=txtVisitDuration.ClientID %>').val('');
                                }
                            });
                        }
                    });
                }
            });
        }
        //#endregion

        function onCboServiceUnitValueChanged() {
            $('#<%=txtServiceUnit.ClientID %>').val(cboServiceUnit.GetText());
            $('#<%:hdnRoomID.ClientID %>').val('');
            $('#<%:txtRoomCode.ClientID %>').val('');
            $('#<%:txtRoomName.ClientID %>').val('');
            $('#<%:txtRoomCode.ClientID %>').trigger('changevalue');
            cbpPhysician.PerformCallback('refresh');
        }

        //#region Room
        function getRoomFilterExpression() {
            var serviceUnitID = cboServiceUnit.GetValue();
            var filterExpression = '';
            if (serviceUnitID != '') {
                filterExpression = "HealthcareServiceUnitID = " + serviceUnitID;
            }
            if (filterExpression != '') filterExpression += " AND ";
            filterExpression += "IsDeleted = 0";
            return filterExpression;
        }

        $('#<%:lblRoom.ClientID %>.lblLink').live('click', function () {
            openSearchDialog('serviceunitroom', getRoomFilterExpression(), function (value) {
                $('#<%:txtRoomCode.ClientID %>').val(value);
                onTxtRoomCodeChanged(value);
            });
        });

        $('#<%:txtRoomCode.ClientID %>').live('change', function () {
            onTxtRoomCodeChanged($(this).val());
        });

        function onTxtRoomCodeChanged(value) {
            var filterExpression = getRoomFilterExpression() + " AND RoomCode = '" + value + "'";
            getRoom(filterExpression);
        }

        function getRoom(_filterExpression) {
            var filterExpression = _filterExpression;
            if (filterExpression == '') filterExpression = getRoomFilterExpression();
            Methods.getListObject('GetvServiceUnitRoomList', filterExpression, function (result) {
                if (result.length == 1) {
                    $('#<%:hdnRoomID.ClientID %>').val(result[0].RoomID);
                    $('#<%:txtRoomCode.ClientID %>').val(result[0].RoomCode);
                    $('#<%:txtRoomName.ClientID %>').val(result[0].RoomName);
                }
                else {
                    $('#<%:hdnRoomID.ClientID %>').val('');
                    $('#<%:txtRoomCode.ClientID %>').val('');
                    $('#<%:txtRoomName.ClientID %>').val('');
                }
            });
        }
        //#endregion
    </script>
    <style type="text/css">
        .tdAppointmentInformation
        {
            position: relative;
            cursor: pointer;
        }
        .tdAppointmentInformation .divAppointmentInformationDt
        {
            z-index: 11;
            font-size: 10px;
            display: none;
            padding: 5px;
            position: absolute;
            top: 15px;
            width: 300px;
            border: 1px solid #AAA;
            text-align: left;
            background-color: White;
        }
        .tdAppointmentInformation .divAppointmentInformationDt td
        {
            color: #000;
        }
        .grdAppointment > tbody > tr > td
        {
            vertical-align: top;
            padding: 0px;
        }
        .tdAppointment
        {
            padding: 0;
            margin: 0;
        }
        .tdAppointment ol
        {
            margin: 0;
            padding: 0;
            width: 100%;
            height: 30px;
            display: table;
            table-layout: fixed;
        }
        .tdAppointment ol li
        {
            display: table-cell;
            border: 1px solid #E3E2E3;
            text-align: center;
        }
        .tdAppointment ol li.selected
        {
            background-color: #F39200;
            color: White;
        }
        .tdTime
        {
            padding: 5px;
        }
    </style>
    <div id="ctxMenuAppointment" class="context-menu">
        <ol>
            <li id="ctxMenuEdit"><a href="#">
                <%=GetLabel("Edit")%></a> </li>
            <li id="ctxMenuVoid"><a href="#">
                <%=GetLabel("Delete")%></a> </li>
        </ol>
    </div>
    <input type="hidden" id="hdnDefaultServiceUnitInterval" runat="server" />
    <input type="hidden" value="" id="hdnPageTitle" runat="server" />
    <table class="tblContentArea">
        <colgroup>
            <col style="width: 50%" />
            <col style="width: 50%" />
        </colgroup>
        <tr>
            <td style="padding: 5px; vertical-align: top; border-right: 1px solid #AAA;">
                <div style="height: 500px; overflow-y: scroll; overflow-x: hidden;">
                    <table style="width: 100%">
                        <colgroup>
                            <col style="width: 100px" />
                            <col />
                        </colgroup>
                        <tr>
                            <td valign="top">
                                <input type="hidden" runat="server" id="hdnCalAppointmentSelectedDate" />
                                <div id="calAppointment">
                                </div>
                            </td>
                            <td valign="top">
                                <dxe:ASPxComboBox ID="cboServiceUnit" ClientInstanceName="cboServiceUnit" Width="100%"
                                    runat="server">
                                    <ClientSideEvents ValueChanged="function(s,e) { onCboServiceUnitValueChanged(s); }" />
                                </dxe:ASPxComboBox>
                                <input type="hidden" id="hdnParamedicID" runat="server" />
                                <div style="position: relative;">
                                    <dxcp:ASPxCallbackPanel ID="cbpPhysician" runat="server" Width="100%" ClientInstanceName="cbpPhysician"
                                        ShowLoadingPanel="false" OnCallback="cbpPhysician_Callback">
                                        <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ onCbpPhysicianEndCallback(s); }" />
                                        <PanelCollection>
                                            <dx:PanelContent ID="PanelContent1" runat="server">
                                                <asp:Panel runat="server" ID="pnlView" CssClass="pnlContainerGrid" Style="height: 200px">
                                                    <asp:GridView ID="grdPhysician" runat="server" CssClass="grdSelected" AutoGenerateColumns="false"
                                                        ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                                        <Columns>
                                                            <asp:BoundField DataField="ParamedicID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                                            <asp:BoundField DataField="ParamedicName" HeaderText="Physician Name" ItemStyle-CssClass="tdParamedicName" />
                                                        </Columns>
                                                        <EmptyDataTemplate>
                                                            <%=GetLabel("No Data To Display")%>
                                                        </EmptyDataTemplate>
                                                    </asp:GridView>
                                                </asp:Panel>
                                            </dx:PanelContent>
                                        </PanelCollection>
                                    </dxcp:ASPxCallbackPanel>
                                    <div class="imgLoadingGrdView" id="containerImgLoadingView">
                                        <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                                    </div>
                                    <div class="containerPaging">
                                        <div class="wrapperPaging">
                                            <div id="pagingPhysican">
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <div style="position: relative;">
                                    <dxcp:ASPxCallbackPanel ID="cbpAppointment" runat="server" Width="100%" ClientInstanceName="cbpAppointment"
                                        ShowLoadingPanel="false" OnCallback="cbpAppointment_Callback">
                                        <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ onCbpAppointmentEndCallback(s); }" />
                                        <PanelCollection>
                                            <dx:PanelContent ID="PanelContent2" runat="server">
                                                <h4 class="h4expanded">
                                                    <%=GetLabel("Appointment Info")%></h4>
                                                <div class="containerTblEntryContent">
                                                    <table>
                                                        <colgroup>
                                                            <col width="50%" />
                                                        </colgroup>
                                                        <tr>
                                                            <td>
                                                                <table>
                                                                    <tr>
                                                                        <td class="tdLabel">
                                                                            <label class="lblNormal">
                                                                                <%=GetLabel("Total Appointment") %></label>
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="txtTotalAppointment" ReadOnly="true" Width="100px" runat="server" />
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td class="tdLabel">
                                                                            <label class="lblNormal">
                                                                                <%=GetLabel("Total Sudah Registrasi") %></label>
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="txtAlreadyRegister" ReadOnly="true" Width="100px" runat="server" />
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td class="tdLabel">
                                                                            <label class="lblNormal">
                                                                                <%=GetLabel("Total Menunggu") %></label>
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="txtPending" ReadOnly="true" Width="100px" runat="server" />
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                            <td style="vertical-align: top">
                                                                <table>
                                                                    <tr id="trMaxAppointment" runat="server">
                                                                        <td class="tdLabel" style="width: 210px">
                                                                            <label class="lblNormal">
                                                                                <%=GetLabel("Maximum Slot Appointment")%></label>
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="txtMaximumAppointment" ReadOnly="true" Width="100px" runat="server" />
                                                                        </td>
                                                                    </tr>
                                                                    <tr id="trMaxWaitingList" runat="server">
                                                                        <td class="tdLabel" style="width: 210px">
                                                                            <label class="lblNormal">
                                                                                <%=GetLabel("Maximum Waiting List") %>
                                                                            </label>
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="txtMaximumWaitingList" ReadOnly="true" Width="100px" runat="server" />
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </div>
                                                <asp:Panel runat="server" ID="Panel1">
                                                    <input type="hidden" runat="server" id="hdnIsWaitingList" value="0" />
                                                    <div class="containerUlTabPage">
                                                        <ul class="ulTabPage" id="ulTabClinicTransaction">
                                                            <li class="selected" contentid="containerAppointment">
                                                                <%=GetLabel("APPOINTMENT") %></li>
                                                            <li contentid="containerWaitingList">
                                                                <%=GetLabel("WAITING LIST") %></li>
                                                        </ul>
                                                    </div>
                                                    <div id="containerAppointment" class="containerAppointment">
                                                        <asp:GridView ID="grdAppointment" runat="server" CssClass="grdSelected grdAppointment"
                                                            AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty"
                                                            OnRowDataBound="grdAppointment_RowDataBound">
                                                            <Columns>
                                                                <asp:BoundField DataField="ID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                                                <asp:TemplateField ItemStyle-CssClass="tdExpand" HeaderStyle-Width="20px">
                                                                    <ItemTemplate>
                                                                        <img class="imgExpand" <%#: Eval("ParentID").ToString() != "-1" ? "style='display:none;'" : "style='cursor:pointer'"%>
                                                                            src='<%= ResolveUrl("~/Libs/Images/down-arrow.png")%>' alt='' />
                                                                        <input type="hidden" parentid='<%#: Eval("ParentID")%>' />
                                                                        <input type="hidden" class="hdnIsExpand" value="1" />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderStyle-Width="70px">
                                                                    <HeaderTemplate>
                                                                        <%=GetLabel("Time") %>
                                                                    </HeaderTemplate>
                                                                    <ItemTemplate>
                                                                        <div class="tdTime">
                                                                            <%#: Eval("Time") %></div>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField ItemStyle-CssClass="tdAppointment">
                                                                    <ItemTemplate>
                                                                        <asp:Repeater ID="rptAppointmentInformation" runat="server">
                                                                            <HeaderTemplate>
                                                                                <ol>
                                                                            </HeaderTemplate>
                                                                            <ItemTemplate>
                                                                                <li>
                                                                                    <div class="tdAppointmentInformation">
                                                                                        <input type="hidden" class="hdnAppointmentID" value='<%#: Eval("AppointmentID")%>' />
                                                                                        <input type="hidden" class="hdnGCAppointmentStatus" value='<%#: Eval("GCAppointmentStatus")%>' />
                                                                                        <img src='<%#ResolveUrl("~/Libs/Images/Button/check.png") %>' height="16px" title="Complete"
                                                                                            <%# Eval("IsAppointmentCompleted").ToString() == "False" ? "style='display:none;float:right'" : "style='float:right'"%> />
                                                                                        <div class="divPatientName">
                                                                                            <%#: Eval("PatientName") %></div>
                                                                                        <div class="divAppointmentInformationDt">
                                                                                            <table>
                                                                                                <tr>
                                                                                                    <td>
                                                                                                        <img src="<%#: Eval("PatientImageUrl") %>" height="60" width="55" alt="" />
                                                                                                    </td>
                                                                                                    <td valign="top">
                                                                                                        <div>
                                                                                                            <%#: Eval("AppointmentNo") %></div>
                                                                                                        <div>
                                                                                                            <b>
                                                                                                                <%#: Eval("PatientName") %></b></div>
                                                                                                        <div>
                                                                                                            <%#: Eval("VisitTypeName") %></div>
                                                                                                        <div>
                                                                                                            <%#: Eval("IsAppointmentCompleted").ToString() == "False" ? "style='display:none;float:right'" : "style='float:right'" %></div>
                                                                                                    </td>
                                                                                                </tr>
                                                                                            </table>
                                                                                        </div>
                                                                                    </div>
                                                                                </li>
                                                                            </ItemTemplate>
                                                                            <FooterTemplate>
                                                                                </ol>
                                                                            </FooterTemplate>
                                                                        </asp:Repeater>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                            </Columns>
                                                            <EmptyDataTemplate>
                                                                <%=GetLabel("No Data To Display")%>
                                                            </EmptyDataTemplate>
                                                        </asp:GridView>
                                                    </div>
                                                    <div id="containerWaitingList" style="display: none" class="containerWaitingList">
                                                        <asp:GridView ID="grdWaitingList" runat="server" CssClass="grdSelected grdAppointment"
                                                            AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty"
                                                            OnRowDataBound="grdWaitingList_RowDataBound">
                                                            <Columns>
                                                                <asp:BoundField DataField="ID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                                                <asp:TemplateField HeaderStyle-Width="70px">
                                                                    <HeaderTemplate>
                                                                        <%=GetLabel("Antrian") %>
                                                                    </HeaderTemplate>
                                                                    <ItemTemplate>
                                                                        <div class="tdTime">
                                                                            <%#: Eval("Queue") %></div>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField ItemStyle-CssClass="tdAppointment">
                                                                    <ItemTemplate>
                                                                        <asp:Repeater ID="rptAppointmentInformation" runat="server">
                                                                            <HeaderTemplate>
                                                                                <ol>
                                                                            </HeaderTemplate>
                                                                            <ItemTemplate>
                                                                                <li>
                                                                                    <div class="tdAppointmentInformation">
                                                                                        <input type="hidden" class="hdnAppointmentID" value='<%#: Eval("AppointmentID")%>' />
                                                                                        <input type="hidden" class="hdnGCAppointmentStatus" value='<%#: Eval("GCAppointmentStatus")%>' />
                                                                                        <img src='<%#ResolveUrl("~/Libs/Images/Button/check.png") %>' height="16px" title="Complete"
                                                                                            <%# Eval("IsAppointmentCompleted").ToString() == "False" ? "style='display:none;float:right'" : "style='float:right'"%> />
                                                                                        <div class="divPatientName">
                                                                                            <%#: Eval("PatientName") %></div>
                                                                                        <div class="divAppointmentInformationDt">
                                                                                            <table>
                                                                                                <tr>
                                                                                                    <td>
                                                                                                        <img src="<%#: Eval("PatientImageUrl") %>" height="60" width="55" alt="" />
                                                                                                    </td>
                                                                                                    <td valign="top">
                                                                                                        <div>
                                                                                                            <%#: Eval("AppointmentNo") %></div>
                                                                                                        <div>
                                                                                                            <b>
                                                                                                                <%#: Eval("PatientName") %></b></div>
                                                                                                        <div>
                                                                                                            <%#: Eval("VisitTypeName") %></div>
                                                                                                    </td>
                                                                                                </tr>
                                                                                            </table>
                                                                                        </div>
                                                                                    </div>
                                                                                </li>
                                                                            </ItemTemplate>
                                                                            <FooterTemplate>
                                                                                </ol>
                                                                            </FooterTemplate>
                                                                        </asp:Repeater>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                            </Columns>
                                                            <EmptyDataTemplate>
                                                                <%=GetLabel("No Data To Display")%>
                                                            </EmptyDataTemplate>
                                                        </asp:GridView>
                                                    </div>
                                                </asp:Panel>
                                            </dx:PanelContent>
                                        </PanelCollection>
                                    </dxcp:ASPxCallbackPanel>
                                </div>
                            </td>
                        </tr>
                    </table>
                </div>
            </td>
            <td style="padding: 5px; vertical-align: top">
                <fieldset id="fsAppointment" style="margin: 0">
                    <input type="hidden" id="hdnAppointmentID" runat="server" />
                    <input type="hidden" id="hdnGCAppointmentStatus" runat="server" />
                    <h4>
                        <%=GetLabel("Visit Information")%></h4>
                    <table class="tblEntryContent" style="width: 100%">
                        <colgroup>
                            <col style="width: 30%" />
                            <col style="width: 140px" />
                            <col style="width: 30px" />
                        </colgroup>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal" id="lblAppointmentNo">
                                    <%=GetLabel("Appointment No")%></label>
                            </td>
                            <td colspan="3">
                                <table>
                                    <tr>
                                        <td><asp:TextBox ID="txtAppointmentNo" ReadOnly="true" Width="300px" runat="server" /></td>
                                        <td>No. Antrian</td>
                                        <td><asp:TextBox ID="txtQueueNo" ReadOnly="true" Width="40px" runat="server" /></td>
                                    </tr>
                                </table>                                
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblMandatory">
                                    <%=GetLabel("Appointment Date / Time")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtAppointmentDate" ReadOnly="true" Width="120px" runat="server"
                                    CssClass="datepicker" />
                            </td>
                            <td style="padding-left: 30px; padding-right: 10px">
                                <%=GetLabel("Hour")%>
                            </td>
                            <td>
                                <asp:TextBox ID="txtAppointmentHour" ReadOnly="true" runat="server" Width="60px"
                                    CssClass="time" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblMandatory">
                                    <%=GetLabel("Clinic")%></label>
                            </td>
                            <td colspan="3">
                                <asp:TextBox ID="txtServiceUnit" Width="300px" runat="server" ReadOnly="true" />
                            </td>
                        </tr>
                        <tr id="trRoom" runat="server">
                            <td class="tdLabel">
                                <label class="lblLink lblNormal" runat="server" id="lblRoom">
                                    <%:GetLabel("Kamar")%></label>
                            </td>
                            <td colspan="3">
                                <input type="hidden" id="hdnRoomID" value="" runat="server" />
                                <table style="width: 100%" cellpadding="0" cellspacing="0">
                                    <colgroup>
                                        <col style="width: 80px" />
                                        <col style="width: 3px" />
                                        <col />
                                    </colgroup>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtRoomCode" Width="100%" runat="server" />
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtRoomName" Width="100%" runat="server" ReadOnly="true" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblMandatory">
                                    <%=GetLabel("Physician")%></label>
                            </td>
                            <td colspan="3">
                                <asp:TextBox ID="txtPhysician" Width="300px" runat="server" ReadOnly="true" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblLink lblMandatory" id="lblVisitType">
                                    <%=GetLabel("Visit Type")%></label>
                            </td>
                            <td colspan="3">
                                <input type="hidden" id="hdnVisitTypeID" value="" runat="server" />
                                <table style="width: 100%" cellpadding="0" cellspacing="0">
                                    <colgroup>
                                        <col style="width: 30%" />
                                        <col style="width: 3px" />
                                        <col />
                                    </colgroup>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtVisitTypeCode" Width="100%" runat="server" />
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtVisitTypeName" ReadOnly="true" Width="100%" runat="server" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Visit Duration")%></label>
                            </td>
                            <td colspan="3">
                                <asp:TextBox ID="txtVisitDuration" Width="100px" runat="server" CssClass="number"
                                    ReadOnly="true" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel" style="vertical-align: top; padding-top: 5px;">
                                <label class="lblNormal">
                                    <%=GetLabel("Remarks")%></label>
                            </td>
                            <td colspan="3">
                                <asp:TextBox ID="txtRemarks" Width="100%" runat="server" TextMode="MultiLine" Rows="2" />
                            </td>
                        </tr>
                    </table>
                    <h4>
                        <%=GetLabel("Patient Information")%></h4>
                    <table class="tblEntryContent" style="width: 100%">
                        <colgroup>
                            <col style="width: 30%" />
                            <col />
                        </colgroup>
                        <tr>
                            <td class="tdLabel">
                            </td>
                            <td>
                                <asp:CheckBox ID="chkIsNewPatient" runat="server" /><%=GetLabel("Is New Patient")%>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblLink" id="lblMRN">
                                    <%=GetLabel("MRN")%></label>
                            </td>
                            <td>
                                <input type="hidden" id="hdnMRN" value="" runat="server" />
                                <asp:TextBox ID="txtMRN" Width="100%" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblMandatory">
                                    <%=GetLabel("Patient Name")%></label>
                            </td>
                            <td>
                                <table style="width: 100%" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td style="width: 11%">
                                            <dxe:ASPxComboBox ID="cboSalutation" ClientInstanceName="cboSalutation" Width="100%"
                                                runat="server" />
                                        </td>
                                        <td style="width: 3px">
                                            &nbsp;
                                        </td>
                                        <td style="width: 25%">
                                            <asp:TextBox ID="txtFirstName" Width="100%" runat="server" />
                                        </td>
                                        <td style="width: 3px">
                                            &nbsp;
                                        </td>
                                        <td style="width: 25%">
                                            <asp:TextBox ID="txtMiddleName" Width="100%" runat="server" />
                                        </td>
                                        <td style="width: 3px">
                                            &nbsp;
                                        </td>
                                        <td style="width: 35%">
                                            <asp:TextBox ID="txtFamilyName" Width="100%" runat="server" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel" style="vertical-align: top; padding-top: 5px;">
                                <label class="lblMandatory">
                                    <%=GetLabel("Address")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtAddress" Width="100%" runat="server" TextMode="MultiLine" Rows="2" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblMandatory">
                                    <%=GetLabel("Phone No")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtPhoneNo" Width="200px" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Mobile Phone")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtMobilePhone" Width="200px" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Email")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtEmail" CssClass="email" Width="200px" runat="server" />
                            </td>
                        </tr>
                    </table>
                </fieldset>
            </td>
        </tr>
    </table>
</asp:Content>
