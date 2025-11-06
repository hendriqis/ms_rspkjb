<%@ Page Title="" Language="C#" MasterPageFile="~/libs/MasterPage/MPTrx.master" AutoEventWireup="true"
    CodeBehind="AppointmentGenerateListDiagnosticSafe.aspx.cs" Inherits="QIS.Medinfras.Web.CommonLibs.Program.AppointmentGenerateListDiagnosticSafe" %>

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
    <li id="btnPrintLabel" crudmode="R" runat="server">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbprint.png")%>' alt="" /><br style="clear: both" />
        <div>
            <%=GetLabel("Print Label")%></div>
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
//            grd.init('<%=lvwView.ClientID %>', '<%=hdnID.ClientID %>', '<%=pnlView.ClientID %>', cbpView, 'paging');

            setDatePicker('<%=txtFromAppointmentRequestDate.ClientID %>');
            $('#<%=txtFromAppointmentRequestDate.ClientID %>').datepicker('option', '0');
            setDatePicker('<%=txtToAppointmentRequestDate.ClientID %>');
            $('#<%=txtToAppointmentRequestDate.ClientID %>').datepicker('option', '0');

            $(window).keydown(function (event) {
                if (event.keyCode == 13) {
                    event.preventDefault();
                    return false;
                }
            });

            onLoadAppointmentRequest();
        });

        $('#<%=btnRefresh.ClientID %>').click(function () {
            cbpView.PerformCallback('import');
        });

        $('#<%=btnUploadPatientList.ClientID %>').click(function () {
            var departmentID = $('#<%=hdnDepartmentID.ClientID %>').val();
            var param = departmentID + '|';
            var url = ResolveUrl("~/Libs/Program/Module/PatientManagement/Appointment/AppointmentUploadExcelCtl.ascx");
            openUserControlPopup(url, param, 'Upload Patient List', 1200, 500);
        });

        $('#<%=btnPrintLabel.ClientID %>').click(function () {
            getSelected();
            if ($('#<%=hdnParam.ClientID %>').val() != "") {
                cbpView.PerformCallback('print');
            }
            else {
                displayMessageBox("WARNING", "Harap pilih permintaan yang akan dicetak");
            }
        });

        $('#<%=btnVoidProcess.ClientID %>').click(function () {
            getSelected();
            if ($('#<%=hdnParam.ClientID %>').val() != "") {
                cbpView.PerformCallback('void');
            }
            else {
                displayMessageBox("WARNING", "Harap pilih permintaan yang akan diproses");
            }
        });

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

        $('#<%=btnApproveAppointment.ClientID %>').click(function () {
            getSelected();
            if ($('#<%=hdnParam.ClientID %>').val() == '') {
                displayMessageBox("WARNING", "Harap pilih permintaan yang akan diproses");
            }
            else {
                cbpView.PerformCallback('multiple|process|');
            }
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
                onRefreshControl();
                setTimeout(function () {
                    s.SetFocus();
                }, 0);
            }, 0);
        }

        //#region Paging
//        var pageCount = parseInt('<%=PageCount %>');
//        $(function () {
//            setPaging($("#paging"), pageCount, function (page) {
//                cbpView.PerformCallback('changepage|' + page);
//            });
//        });

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
            else if (param[0] == 'multiple') {
                if (param[2] == 'appointment') {
                    if (param[3] == 'success') {
                        cbpView.PerformCallback('refresh');
                        displayMessageBox('SUCCESS', 'Proses pembuatan perjanjian berhasil!');
                    }
                    else {
                        displayMessageBox(' Failed', 'Error Message : ' + param[4]);
                        onRefreshPage();
                    }
                }
                else if (param[2] == 'registration') {
                    if (param[3] == 'success') {
                        cbpView.PerformCallback('refresh');
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
            else if (param[0] == 'fail') {
                displayMessageBox(' Failed', 'Error Message : ' + param[1]);
                cbpView.PerformCallback('refresh');
            }
            else
                $('#<%=lvwView.ClientID %> tr:eq(1)').click();
            setDatePicker('<%=txtFromAppointmentRequestDate.ClientID %>');
            $('#<%=txtFromAppointmentRequestDate.ClientID %>').datepicker('option', '0');
        }
        //#endregion

        function onLoadAppointmentRequest() {
            $('#chkSelectAll').die('change');
            $('#chkSelectAll').live('change', function () {
                var isChecked = $(this).is(":checked");
                $('.chkIsSelected input').each(function () {
                    $(this).prop('checked', isChecked);
                    $(this).change();
                });
            });
        }

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

        $('.lblCustomerType.lblLink').live('click', function () {
            $tr = $(this).closest('tr');
            var gcCustomerType = $tr.find('.hdnGCCustomerType').val();
            var appointmentRequestID = $tr.find('.hdnKey').val();

            var url = ResolveUrl("~/Libs/Program/Module/PatientManagement/Appointment/AppointmentRequestChangePayerCtl.ascx");
            var id = appointmentRequestID + '|' + gcCustomerType;
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
            cbpView.PerformCallback('refresh');
        }

        function onBeforeLoadRightPanelContent(code) {
            if (code == 'infoDokterPraktek') {
                param = $('#<%:hdnDepartmentID.ClientID %>').val();
            }
        }

        function onCboFilterTypeChanged() {
            var type = cboFilterType.GetValue();
//            if (type == "3") {
//                $('#trProcessType').attr("style", "display:none");
//            }
//            else {
//                $('#trProcessType').removeAttr("style");
//            }
        }
    </script>
    <input type="hidden" id="hdnDepartmentID" runat="server" />
    <input type="hidden" value="" id="hdnParam" runat="server" />
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
    <input type="hidden" value="" id="hdnLstParamedicID" runat="server" />
    <input type="hidden" value="" id="hdnLstApmDate" runat="server" />
    <input type="hidden" value="" id="hdnLstCustomerType" runat="server" />
    <input type="hidden" value="" id="hdnTodayDate" runat="server" />
    <input type="hidden" value="" id="hdnIsControlAdministrationCharges" runat="server" />
    <input type="hidden" value="" id="hdnChargeCodeAdministrationForInstansi" runat="server" />
    <input type="hidden" value="" id="hdnIsControlAdmCost" runat="server" />
    <input type="hidden" value="" id="hdnAdminID" runat="server" />
    <input type="hidden" value="" id="hdnIsControlPatientCardPayment" runat="server" />
    <input type="hidden" value="" id="hdnItemCardFee" runat="server" />
    <div style="height: 550px; overflow-y: auto; overflow-x: hidden;">
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
                            <tr>
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
                                                <table id="tblView" runat="server" class="grdMCUOrder grdSelected" cellspacing="0"
                                                    rules="all">
                                                    <tr>
                                                        <th class="keyField">
                                                            &nbsp;
                                                        </th>
                                                         <th style="width: 10px" align="center">
                                                            <input id="chkSelectAll" type="checkbox" />
                                                        </th>
                                                        <th align="left" style="width:200px">
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
                                                        <asp:TextBox ID="txtAppointmentDate" Width="120px" runat="server" CssClass="txtAppointmentDate datepicker" />
                                                    </td> 
                                                    <td>
                                                        <%#: Eval("AppointmentNo")%><br />
                                                        <%#: Eval("cfAppointmentDate")%>
                                                    </td>
                                                    <td>
                                                        <%#: Eval("RegistrationNo")%><br />
                                                        <%#: Eval("cfRegistrationDate")%>
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
</asp:Content>
