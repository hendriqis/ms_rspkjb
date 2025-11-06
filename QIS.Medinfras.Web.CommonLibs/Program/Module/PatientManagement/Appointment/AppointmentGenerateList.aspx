<%@ Page Title="" Language="C#" MasterPageFile="~/libs/MasterPage/MPTrx.master" AutoEventWireup="true"
    CodeBehind="AppointmentGenerateList.aspx.cs" Inherits="QIS.Medinfras.Web.CommonLibs.Program.AppointmentGenerateList" %>

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
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div class="menuTitle">
        <%=HttpUtility.HtmlEncode(GetPageTitle())%></div>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript" src='<%= ResolveUrl("~/Libs/Scripts/CustomGridViewList.js")%>'></script>
    <script type="text/javascript">
        $(function () {
            $('#<%=lvwView.ClientID %> tr:gt(0)').each(function () {
//                $txtAppointmentDate = $(this).find('.txtAppointmentDate');
//                if ($txtAppointmentDate != null) {
//                    setDatePickerElement($txtAppointmentDate);
//                }
//                else {
//                    $txtAppointmentDate.val('<%=DateTimeNowDatePicker() %>');
//                }
                $txtAppointmentDate.val('<%=DateTimeNowDatePicker() %>');
            });

            $('.txtAppointmentDate').each(function () {
                setDatePickerElement($(this));
                $(this).datepicker('option', 'minDate', '0');
            });

            var grd = new customGridView();
            grd.init('<%=lvwView.ClientID %>', '<%=hdnID.ClientID %>', '<%=pnlView.ClientID %>', cbpView, 'paging');

            setDatePicker('<%=txtFromAppointmentRequestDate.ClientID %>');
            setDatePicker('<%=txtToAppointmentRequestDate.ClientID %>');
            $('#<%=txtFromAppointmentRequestDate.ClientID %>').datepicker('option', '0');
            $('#<%=txtToAppointmentRequestDate.ClientID %>').datepicker('option', '0');

            Methods.getObject('GetSettingParameterDtList', "ParameterCode = 'OP0041'", function (result) {
                if (result != null) {
                    $('#<%=hdnIsUsingConfirmationSession.ClientID %>').val(result.ParameterValue);
                }
            });
        });

        $('#<%=btnRefresh.ClientID %>').click(function () {
            $('#<%=hdnFilterExpressionQuickSearch.ClientID %>').val(txtSearchView.GenerateFilterExpression());
            cbpView.PerformCallback('import');
        });

        function onLoad() {
            //#region Location From
            function getServiceUnitFilterExpression() {
                var filterExpression = "<%:GetFilterHealthcareServiceUnit()%>";
                return filterExpression;
            }

            $('#<%=lblServiceUnit.ClientID %>.lblLink').live('click', function () {
                openSearchDialog('serviceunitperhealthcare', getServiceUnitFilterExpression(), function (value) {
                    $('#<%=txtServiceUnitCode.ClientID %>').val(value);
                    ontxtServiceUnitCodeChanged(value);
                });
            });

            $('#<%=txtServiceUnitCode.ClientID %>').live('change', function () {
                ontxtServiceUnitCodeChanged($(this).val());
            });

            function ontxtServiceUnitCodeChanged(value) {
                var filterExpression = getServiceUnitFilterExpression() + " AND ServiceUnitCode = '" + value + "'";
                Methods.getObject('GetvHealthcareServiceUnitList', filterExpression, function (result) {
                    if (result != null) {
                        $('#<%=hdnHealthcareServiceUnitID.ClientID %>').val(result.HealthcareServiceUnitID);
                        $('#<%=txtServiceUnitCode.ClientID %>').val(result.ServiceUnitCode);
                        $('#<%=txtServiceUnitName.ClientID %>').val(result.ServiceUnitName);
                        cbpView.PerformCallback('refresh');
                    }
                    else {
                        $('#<%=hdnHealthcareServiceUnitID.ClientID %>').val('');
                        $('#<%=txtServiceUnitCode.ClientID %>').val('');
                        $('#<%=txtServiceUnitName.ClientID %>').val('');
                        cbpView.PerformCallback('refresh');
                    }
                });
            }
            //#endregion
        }

        function onAfterCustomClickSuccess(type) {
            cbpView.PerformCallback('refresh');
        }

        function onAfterSaveAddRecordEntryPopup(param) {
            cbpView.PerformCallback('refresh');
        }

        function onTxtSearchViewSearchClick(s) {
            setTimeout(function () {
                s.SetBlur();
                $('#<%=hdnFilterExpressionQuickSearch.ClientID %>').val(txtSearchView.GenerateFilterExpression());
                cbpView.PerformCallback('refresh');
            }, 0);
        }

        //#region Paging
        var pageCount = parseInt('<%=PageCount %>');
        $(function () {
            setPaging($("#paging"), pageCount, function (page) {
                cbpView.PerformCallback('changepage|' + page);
            });
        });

        function onCbpViewEndCallback(s) {
            hideLoadingPanel();
            var param = s.cpResult.split('|');

            if (param[0] == 'refresh') {

                var pageCount = parseInt(param[1]);
                if (pageCount > 0)
                    $('#<%=lvwView.ClientID %> tr:eq(1)').click();

                setPaging($("#paging"), pageCount, function (page) {
                    cbpView.PerformCallback('changepage|' + page);
                });
            }
            else if (param[0] == 'createApp') {
                cbpView.PerformCallback('refresh');
                var id = param[2];
                var url = ResolveUrl("~/Libs/Program/Module/PatientManagement/Appointment/AppointmentGenerateInformationCtl.ascx");
                openUserControlPopup(url, id, 'Appointment Information', 800, 500);

            }
            else if (param[0] == 'fail') {
                displayErrorMessageBox('Permintaan Perjanjian', param[1]);
                cbpView.PerformCallback('refresh');
            } else if (param[0] == "showInfo") {
                var id = $('#<%:hdnApmID.ClientID %>').val();
                var url = ResolveUrl("~/Libs/Program/Module/PatientManagement/Appointment/AppointmentGenerateInformationCtl.ascx");
                openUserControlPopup(url, id, 'Appointment Information', 800, 500);
              
            }
            else
                $('#<%=lvwView.ClientID %> tr:eq(1)').click();
        }
        //#endregion

        $('.lblParamedicName.lblLink').live('click', function () {
            var healthcareServiceUnitID = $(this).closest('tr').find('.hdnHealthareServiceUnitID').val();
            $td = $(this).parent();
            var paramedicID = $td.children('.hdnParamedicID').val();
            var healthcareServiceUnitID = $td.children('.hdnHealthcareServiceUnitPerRowID').val();
            var filter = "HealthcareServiceUnitID = '" + healthcareServiceUnitID + "'";
            openSearchDialog('serviceUnitParamedicMaster', filter, function (value) {
                onTxtParamedicChanged(value, $td);
            });
        });

        function onTxtParamedicChanged(value, $td) {
            var filterExpression = "ParamedicID = '" + value + "'";
            Methods.getObject('GetvParamedicMasterList', filterExpression, function (result) {
                if (result != null) {
                    $td.find('.lblParamedicName').html(result.ParamedicName);
                    $td.find('.hdnParamedicID').val(result.ParamedicID);
                }
            });
        }

        function getDayNumber(dateString) {
            var parts = dateString.split("-");
            var year = parseInt(parts[0], 10);
            var month = ('0' + parseInt(parts[1], 10)).slice(-2);
            var day = parseInt(parts[2], 10);
            var selectedDate = new Date(day, month - 1, year); // Kurangi 1 dari month untuk mendapatkan indeks bulan yang benar
            var dayNumber = selectedDate.getDay();

            // Mengubah nomor hari jika 0 menjadi 7 karena di database nomor hari untuk hari Minggu adalah 7
            if (dayNumber == 0) {
                dayNumber = 7;
            }
            return dayNumber;
        }

        $('.lblCustomerType.lblLink').live('click', function () {
            $tr = $(this).closest('tr');
            var gcCustomerType = $tr.find('.hdnGCCustomerType').val();
            var appointmentRequestID = $tr.find('.hdnKey').val();

            var url = ResolveUrl("~/Libs/Program/Module/PatientManagement/Appointment/AppointmentRequestChangePayerCtl.ascx");
            var id = 'single|' + appointmentRequestID;
            openUserControlPopup(url, id, 'Ubah Tipe Penjamin', 800, 350);
        });

        $('.lblViewReferral.lblLink').live('click', function () {
            $tr = $(this).closest('tr');
            var appointmentRequestID = $tr.find('.hdnKey').val();

            var url = ResolveUrl("~/Libs/Program/Module/PatientManagement/Appointment/AppointmentRequestReferralInformationCtl.ascx");
            var id = appointmentRequestID;
            openUserControlPopup(url, id, 'Informasi Rujukan', 300, 300);
        });

        $btnSave = null;
        $('.btnSave').live('click', function () {
            $tr = $(this).closest('tr');
            var ID = $tr.find('.hdnKey').val();
            var appDate = $tr.find('.txtAppointmentDate').val();
            var paramedicID = $tr.find('.hdnParamedicID').val();
            var gcCustomerType = $tr.find('.hdnGCCustomerType').val();
            $('#<%=hdnAppointmentRequestID.ClientID %>').val(ID);
            $('#<%=hdnAppointmentRequestParamedicID.ClientID %>').val(paramedicID);
            $('#<%=hdnAppointmentRequestDate.ClientID %>').val(appDate);
            $('#<%=hdnGCCustomerType.ClientID %>').val(gcCustomerType);
            cbpView.PerformCallback('createApp');
        });

        $('.btnConfirm').live('click', function () {
            $tr = $(this).closest('tr');
            var ID = $tr.find('.hdnKey').val();
            var appDate = $tr.find('.txtAppointmentDate').val();
            var ParamedicID = $tr.find('.hdnParamedicID').val();
            var gcCustomerType = $tr.find('.hdnGCCustomerType').val();
            var hsuID = $tr.find('.hdnHealthcareServiceUnitPerRowID').val();
            var dateParts = appDate.split('-');
            var formattedDate = dateParts[2] + '-' + dateParts[1].padStart(2, '0') + '-' + dateParts[0].padStart(2, '0');
            var dayNumber = getDayNumber(appDate);
            $('#<%=hdnAppointmentRequestID.ClientID %>').val(ID);
            $('#<%=hdnAppointmentRequestParamedicID.ClientID %>').val(ParamedicID);
            $('#<%=hdnAppointmentRequestDate.ClientID %>').val(appDate);
            $('#<%=hdnGCCustomerType.ClientID %>').val(gcCustomerType);
            var param = ID + "|" + appDate + "|" + ParamedicID;

            var filterExpression = "HealthcareServiceUnitID = " + hsuID + " AND ParamedicID = " + ParamedicID + " AND ScheduleDate = '" + formattedDate + "' AND (IsAppointmentByTimeSlot1 = 1 OR IsAppointmentByTimeSlot2 = 1 OR IsAppointmentByTimeSlot3 = 1 OR IsAppointmentByTimeSlot4 = 1 OR IsAppointmentByTimeSlot5 = 1)";
            Methods.getObject("GetParamedicScheduleDateList", filterExpression, function (result) {
                if (result != null) {
                    displayMessageBox('Warning', 'Proses ini hanya untuk jadwal tanpa menggunakan Time Slot, harap lakukan pada menu Perjanjian Pasien dengan cara salin Nomor Permintaan Perjanjian.');
                }
                else {
                    var filterExpression2 = "HealthcareServiceUnitID = " + hsuID + " AND ParamedicID = " + ParamedicID + " AND DayNumber = " + dayNumber + " AND (IsAppointmentByTimeSlot1 = 1 OR IsAppointmentByTimeSlot2 = 1 OR IsAppointmentByTimeSlot3 = 1 OR IsAppointmentByTimeSlot4 = 1 OR IsAppointmentByTimeSlot5 = 1)";
                    Methods.getObject("GetParamedicScheduleList", filterExpression2, function (result2) {
                        if (result2 != null) {
                            displayMessageBox('Warning', 'Proses ini hanya untuk jadwal tanpa menggunakan Time Slot, harap lakukan pada menu Perjanjian Pasien dengan cara salin Nomor Permintaan Perjanjian.');
                        }
                        else {
                            if ($('#<%=hdnIsUsingConfirmationSession.ClientID %>').val() == "1") {
                                var url = ResolveUrl("~/Libs/Program/Module/PatientManagement/Appointment/AppointmentRequestConfirmCtl.ascx");
                                openUserControlPopup(url, param, 'Konfirmasi Appointment Request', 800, 350);
                            }
                            else {
                                cbpView.PerformCallback('createApp');
                            }
                        }
                    });
                }
            });
        });

        $btnVoid = null;
        $('.btnVoid').live('click', function () {
            $tr = $(this).closest('tr');
            var id = $tr.find('.hdnKey').val();
            var appDate = $tr.find('.txtAppointmentDate').val();
            var paramedicID = $tr.find('.hdnParamedicID').val();

            var url = ResolveUrl("~/Libs/Program/Module/PatientManagement/Appointment/AppointmentRequestVoidCtl.ascx");
            openUserControlPopup(url, id, 'Void Appointment Request', 800, 350);
        });

        function isValidDate(value) {
            var dateWrapper = new Date(value);
            return !isNaN(dateWrapper.getDate());
        }

        function onRefreshPage() {
            cbpView.PerformCallback('refresh');
        }

        function onBeforeLoadRightPanelContent(code) {
            if (code == 'infoDokterPraktek') {
                param = $('#<%:hdnDepartmentID.ClientID %>').val();
            }
        }

        function createApmSuccess(id) {
           $('#<%:hdnApmID.ClientID %>').val(id);
           cbpView.PerformCallback('showInfo');
        }
        
        
    </script>
    <input type="hidden" id="hdnApmID" runat="server" />
    <input type="hidden" id="hdnDepartmentID" runat="server" />
    <input type="hidden" value="" id="hdnParam" runat="server" />
    <input type="hidden" value="" id="hdnID" runat="server" />
    <input type="hidden" value="" id="hdnPageTitle" runat="server" />
    <input type="hidden" value="" id="hdnFilterExpression" runat="server" />
    <input type="hidden" value="" id="hdnQuickText" runat="server" />
    
    <input type="hidden" value="" id="hdnFilterExpressionQuickSearch" runat="server" />
    
    <input type="hidden" value="" id="hdnAppointmentRequestID" runat="server" />
    <input type="hidden" value="" id="hdnAppointmentRequestParamedicID" runat="server" />
    <input type="hidden" value="" id="hdnAppointmentRequestDate" runat="server" />
    <input type="hidden" value="" id="hdnHealthcareServiceUnitImagingID" runat="server" />
    <input type="hidden" value="" id="hdnHealthcareServiceUnitLaboratoryID" runat="server" />
    <input type="hidden" value="" id="hdnHealthcareServiceUnitRadioteraphyID" runat="server" />
    <input type="hidden" value="" id="hdnIsBridgingToMedinfrasMobileApps" runat="server" />
    <input type="hidden" value="" id="hdnGCCustomerType" runat="server" />
    <input type="hidden" value="" id="hdnIsUsingConfirmationSession" runat="server" />
    <input type="hidden" value="" id="hdnSubUnit" runat="server" />
    <div style="height: 550px; overflow-y: auto; overflow-x: hidden;">
        <table class="tblContentArea">
            <colgroup>
                <col style="width: 50%" />
                <col style="width: 50%" />
            </colgroup>
            <tr>
                <td style="padding: 5px; vertical-align: top">
                    <table class="tblEntryContent" style="width: 100%">
                        <colgroup>
                            <col style="width: 120px" />
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
                                        <col style="width: 20px" />
                                        <col style="width: 145px" />
                                    </colgroup>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtFromAppointmentRequestDate" Width="120px" CssClass="datepicker"
                                                runat="server" />
                                        </td>
                                        <td>
                                            <%=GetLabel("s/d") %>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtToAppointmentRequestDate" Width="120px" CssClass="datepicker"
                                                runat="server" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblMandatory lblLink" runat="server" id="lblServiceUnit">
                                    <%=GetLabel("Unit Pelayanan")%></label>
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
                          <tr>
                                <td class="tdLabel">
                                    <label>
                                        <%=GetLabel("Quick Filter")%></label>
                                </td>
                                <td>
                                    <qis:QISIntellisenseTextBox runat="server" ClientInstanceName="txtSearchView" ID="txtSearchView"
                                        Width="350px" Watermark="Search">
                                        <ClientSideEvents SearchClick="function(s){ onTxtSearchViewSearchClick(s); }" />
                                        <IntellisenseHints>
                                            <qis:QISIntellisenseHint Text="Nama Pasien" FieldName="PatientName" />
                                            <qis:QISIntellisenseHint Text="No.RM" FieldName="MedicalNo" />
                                        </IntellisenseHints>
                                    </qis:QISIntellisenseTextBox>
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
                                                    <th align="left">
                                                        <%=GetLabel("No. Permintaan Perjanjian")%>
                                                    </th>
                                                    <th align="left">
                                                        <%=GetLabel("No. Rekam Medis")%>
                                                    </th>
                                                    <th align="left">
                                                        <%=GetLabel("Pasien")%>
                                                    </th>
                                                    <th align="left">
                                                        <%=GetLabel("Unit Layanan")%>
                                                    </th>
                                                    <th align="center">
                                                        <%=GetLabel("No Antrian")%>
                                                    </th>
                                                    <th align="left">
                                                        <%=GetLabel("Dokter")%>
                                                    </th>
                                                    <th align="left">
                                                        <%=GetLabel("Penjamin")%>
                                                    </th>
                                                    <th align="center">
                                                        <%=GetLabel("Tanggal")%>
                                                    </th>
                                                    <th align="center">
                                                        <%=GetLabel("Informasi Rujukan")%>
                                                    </th>
                                                    <th>
                                                    </th>
                                                    <th>
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
                                                    <th align="left">
                                                        <%=GetLabel("No. Permintaan Perjanjian")%>
                                                    </th>
                                                    <th align="left">
                                                        <%=GetLabel("No. Rekam Medis")%>
                                                    </th>
                                                    <th align="left">
                                                        <%=GetLabel("Pasien")%>
                                                    </th>
                                                    <th align="left">
                                                        <%=GetLabel("Unit Layanan")%>
                                                    </th>
                                                    <th align="center">
                                                        <%=GetLabel("No Antrian")%>
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
                                                    <th align="center">
                                                        <%=GetLabel("Informasi Rujukan")%>
                                                    </th>
                                                    <th>
                                                    </th>
                                                    <th>
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
                                                <td align="left">
                                                    <%#: Eval("AppointmentRequestNo")%>
                                                </td>
                                                <td align="left">
                                                    <input type="hidden" value="" class="hdnKey" id="hdnKey" runat="server" />
                                                    <%#: Eval("GuestID").ToString() != null && Eval("GuestID").ToString() != "0" ? Eval("GuestNo") : Eval("MedicalNo")%>
                                                </td>
                                                <td align="left">
                                                    <%#: Eval("GuestID").ToString() != null && Eval("GuestID").ToString() != "0" ? Eval("GuestName") : Eval("PatientName")%>
                                                </td>
                                                <td align="left">
                                                    <%#: Eval("ServiceUnitName")%>
                                                </td>
                                                <td align="center">
                                                    <%#: Eval("QueueRequestNo")%>
                                                </td>
                                                <td align="left">
                                                    <input type="hidden" value="" class="hdnHealthcareServiceUnitPerRowID" id="hdnHealthcareServiceUnitPerRowID"
                                                        runat="server" />
                                                    <input type="hidden" value="" class="hdnParamedicID" id="hdnParamedicID" runat="server" />
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
                                                    <asp:TextBox ID="txtAppointmentDate" Width="120px" runat="server" CssClass="txtAppointmentDate datepicker" />
                                                </td>
                                                <td align="center" style="width: 80px">
                                                    <label class="lblViewReferral lblLink" runat="server" id="lblViewReferral">
                                                        View
                                                    </label>
                                                </td>
                                                <td align="center">
                                                    <input type="button" id="btnSave" class="btnSave  w3-btn w3-hover-green" value="Simpan" runat="server" style="display:none;"/>

                                                     <input type="button" id="btnConfirm" class="btnConfirm  w3-btn w3-hover-green" value="Simpan"/>
                                                </td>
                                                <td align="center">
                                                    <input type="button" id="btnVoid" class="btnVoid  w3-btn w3-hover-green" value="Batal" runat="server" />
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
                    <div class="containerPaging">
                        <div class="wrapperPaging">
                            <div id="paging">
                            </div>
                        </div>
                    </div>
                </td>
            </tr>
        </table>
    </div>
    <script type="text/javascript">
        $(function () {
            txtSearchView.SetText($('#<%=hdnQuickText.ClientID %>').val());
        });
    </script>
</asp:Content>
