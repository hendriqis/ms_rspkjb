<%@ Page Language="C#" MasterPageFile="~/Libs/MasterPage/MPMain.master" AutoEventWireup="true"
    CodeBehind="PatientBillSummaryCombineList.aspx.cs" Inherits="QIS.Medinfras.Web.CommonLibs.Program.PatientBillSummaryCombineList" %>

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
            setDatePicker('<%=txtDateFrom.ClientID %>');
            setDatePicker('<%=txtDateTo.ClientID %>');


            $('#<%=txtDateFrom.ClientID %>').change(function (evt) {
                onRefreshGridView();
            });
            $('#<%=txtDateTo.ClientID %>').change(function (evt) {
                onRefreshGridView();
            });

            $('#lblRefresh.lblLink').click(function (evt) {
                onRefreshGridView();
            });
        });

        //#region Physician
        function onGetPhysicianFilterExpression() {
            var filterExpression = "";
            var healthcareServiceUnitID = $('#<%=hdnHealthcareServiceUnitID.ClientID %>').val();
            if (healthcareServiceUnitID != '')
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
                cbpView.PerformCallback('refresh');
            });
        }
        //#endregion

        //#region ServiceUnit
        function onGetServiceUnitFilterExpression() {
            var filterExpression = "";
            var departmentID = cboDepartment.GetValue();

            if (departmentID == 'ALL') {
                filterExpression = 'IsDeleted = 0';
            }
            else {
                filterExpression = "DepartmentID = '" + departmentID + "' AND IsDeleted = 0";
            }
            return filterExpression;
        }

        $('#lblServiceUnit.lblLink').live('click', function () {
            openSearchDialog('serviceunitperhealthcare', onGetServiceUnitFilterExpression(), function (value) {
                $('#<%=txtServiceUnitCode.ClientID %>').val(value);
                onTxtServiceUnitCodeChanged(value);
            });
        });

        $('#<%=txtServiceUnitCode.ClientID %>').live('change', function () {
            onTxtServiceUnitCodeChanged($(this).val());
        });

        function onTxtServiceUnitCodeChanged(value) {
            var filterExpression = onGetServiceUnitFilterExpression() + " AND ServiceUnitCode = '" + value + "'";
            Methods.getObject('GetvHealthcareServiceUnitList', filterExpression, function (result) {
                if (result != null) {
                    $('#<%=hdnHealthcareServiceUnitID.ClientID %>').val(result.HealthcareServiceUnitID);
                    $('#<%=txtServiceUnitName.ClientID %>').val(result.ServiceUnitName);
                }
                else {
                    $('#<%=txtServiceUnitCode.ClientID %>').val('');
                    $('#<%=hdnHealthcareServiceUnitID.ClientID %>').val('');
                    $('#<%=txtServiceUnitName.ClientID %>').val('');
                }
                cbpView.PerformCallback('refresh');
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

        $(function () {
            $('#<%=txtBarcodeEntry.ClientID %>').keypress(function (e) {
                var keyCode = e.keyCode || e.which;
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

        function onTxtSearchViewSearchClick(s) {
            setTimeout(function () {
                s.SetBlur();
                onRefreshGridView();
                setTimeout(function () {
                    s.SetFocus();
                }, 0);
            }, 0);
        }

        //#region tab
        $(function () {
            $('#ulTabLabResult li').click(function () {
                $('#ulTabLabResult li.selected').removeAttr('class');
                $('.containerInfo').filter(':visible').hide();
                $contentID = $(this).attr('contentid');
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
            var dept = cboDepartment.GetValue();
            openSearchDialog('serviceunitroom', "DepartmentID = '" + dept + "' AND IsDeleted = 0", function (value) {
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

        function onAfterSaveEditRecordEntryPopup() {
        }

        function onAfterPopupControlClosing() {
        }

        function setRightPanelButtonEnabled() {
        }

        function onBeforeLoadRightPanelContent(code) {
        }

        function onCboDepartmentChanged() {
            $('#<%=hdnHealthcareServiceUnitID.ClientID %>').val('');
            $('#<%=txtServiceUnitCode.ClientID %>').val('');
            $('#<%=txtServiceUnitName.ClientID %>').val('');
            cbpView.PerformCallback('refresh');
        }

        function onCboDateFilterChanged() {
            cbpView.PerformCallback('refresh');
        }

        $('#<%=txtDateFrom.ClientID %>').live('change', function () {
            var start = $('#<%=txtDateFrom.ClientID %>').val();
            var end = $('#<%=txtDateTo.ClientID %>').val();

            $('#<%=txtDateFrom.ClientID %>').val(validateDateFromTo(start, end));
        });

        $('#<%=txtDateTo.ClientID %>').live('change', function () {
            var start = $('#<%=txtDateFrom.ClientID %>').val();
            var end = $('#<%=txtDateTo.ClientID %>').val();

            $('#<%=txtDateTo.ClientID %>').val(validateDateToFrom(start, end));
        });

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
        <input type="hidden" value="" id="hdnRoomID" runat="server" />
        <input type="hidden" value="" id="hdnIsControlAdministrationCharges" runat="server" />
        <input type="hidden" value="" id="hdnChargeCodeAdministrationForInstansi" runat="server" />
        <input type="hidden" value="" id="hdnIsControlAdmCost" runat="server" />
        <input type="hidden" value="" id="hdnAdminID" runat="server" />
        <input type="hidden" value="" id="hdnIsControlPatientCardPayment" runat="server" />
        <input type="hidden" value="" id="hdnItemCardFee" runat="server" />
        <input type="hidden" value="" id="hdnServiceUntIDLab" runat="server" />
        <input type="hidden" value="" id="hdnServiceUntIDRadiologi" runat="server" />
        <div class="containerUlTabPage" style="margin-bottom: 3px;">
            <ul class="ulTabPage" id="ulTabLabResult">
                <li contentid="containerTransaction" id="patientTransactionList" runat="server" class="selected">
                    <%= HttpUtility.HtmlEncode(GetMenuCaption())%></li>
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
                            <table class="tblEntryContent">
                                <colgroup>
                                    <col style="width: 100px" />
                                    <col style="width: 145px" />
                                    <col style="width: 10px" />
                                    <col />
                                </colgroup>
                                <tr>
                                    <td class="tdLabel">
                                        <label class="lblNormal">
                                            <%=GetLabel("Filter Tanggal")%></label>
                                    </td>
                                    <td colspan="2">
                                        <dxe:ASPxComboBox ID="cboDateFilter" ClientInstanceName="cboDateFilter" runat="server"
                                            Width="20%">
                                            <ClientSideEvents ValueChanged="function(s,e) { onCboDateFilterChanged(); }" />
                                        </dxe:ASPxComboBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel">
                                        <label class="lblNormal">
                                            <%=GetLabel("Tanggal")%></label>
                                    </td>
                                    <td colspan="2">
                                        <table style="width: 100%" cellpadding="0" cellspacing="0">
                                            <colgroup>
                                                <col style="width: 150px" />
                                                <col style="width: 5px" />
                                                <col />
                                            </colgroup>
                                            <tr>
                                                <td>
                                                    <asp:TextBox runat="server" CssClass="datepicker" ID="txtDateFrom" Width="120px" />
                                                </td>
                                                <td>
                                                    <label class="lblNormal">
                                                        <%=GetLabel(" s/d ")%></label>
                                                </td>
                                                <td>
                                                    <asp:TextBox runat="server" CssClass="datepicker" ID="txtDateTo" Width="120px" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr id="trDepartment" runat="server">
                                    <td class="tdLabel">
                                        <label class="lblNormal">
                                            <%=GetLabel("Department")%></label>
                                    </td>
                                    <td colspan="3">
                                        <dxe:ASPxComboBox ID="cboDepartment" ClientInstanceName="cboDepartment" runat="server"
                                            Width="30%">
                                            <ClientSideEvents ValueChanged="function(s,e) { onCboDepartmentChanged(); }" />
                                        </dxe:ASPxComboBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel">
                                        <label class="lblLink" id="lblServiceUnit">
                                            <%=GetLabel("Unit Pelayanan")%></label>
                                    </td>
                                    <td colspan="2">
                                        <input type="hidden" id="hdnHealthcareServiceUnitID" runat="server" value="" />
                                        <table style="width: 100%" cellpadding="0" cellspacing="0">
                                            <colgroup>
                                                <col style="width: 120px" />
                                                <col style="width: 3px" />
                                                <col />
                                            </colgroup>
                                            <tr>
                                                <td>
                                                    <asp:TextBox ID="txtServiceUnitCode" Width="120px" runat="server" />
                                                </td>
                                                <td>
                                                    &nbsp;
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtServiceUnitName" ReadOnly="true" Width="350px" runat="server" />
                                                </td>
                                            </tr>
                                        </table>
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
                                                    <asp:TextBox ID="txtPhysicianName" ReadOnly="true" Width="350px" runat="server" />
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
                                    <td colspan="3">
                                        <qis:QISIntellisenseTextBox runat="server" ClientInstanceName="txtSearchView" ID="txtSearchView"
                                            Width="560px" Watermark="Search">
                                            <ClientSideEvents SearchClick="function(s){ onTxtSearchViewSearchClick(s); }" />
                                            <IntellisenseHints>
                                                <qis:QISIntellisenseHint Text="Nama Pasien" FieldName="PatientName" />
                                                <qis:QISIntellisenseHint Text="No Registrasi" FieldName="RegistrationNo" />
                                                <qis:QISIntellisenseHint Text="No.RM" FieldName="MedicalNo" />
                                            </IntellisenseHints>
                                        </qis:QISIntellisenseTextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel">
                                        <label class="lblNormal">
                                            <%=GetLabel("No. Rekam Medis")%></label>
                                    </td>
                                    <td colspan="3">
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
        <div style="display: none">
            <dxcp:ASPxCallbackPanel ID="cbpBarcodeEntryProcess" runat="server" Width="100%" ClientInstanceName="cbpBarcodeEntryProcess"
                ShowLoadingPanel="false" OnCallback="cbpBarcodeEntryProcess_Callback">
                <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ onCbpBarcodeEntryProcessEndCallback(s); }" />
            </dxcp:ASPxCallbackPanel>
        </div>
        <script type="text/javascript">
            $(function () {
                txtSearchView.SetText($('#<%=hdnQuickText.ClientID %>').val());
            });
        </script>
    </div>
</asp:Content>
