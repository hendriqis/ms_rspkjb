<%@ Page Title="" Language="C#" MasterPageFile="~/libs/MasterPage/MPTrx.master" AutoEventWireup="true"
    CodeBehind="MedicalFolderTrackingList.aspx.cs" Inherits="QIS.Medinfras.Web.MedicalRecord.Program.MedicalFolderTrackingList" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="QIS.Medinfras.Web.CustomControl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"
    Namespace="QIS.Medinfras.Web.CustomControl" TagPrefix="qis" %>
<%@ Register Src="~/Libs/Controls/PatientGrid/CheckGridPatientVisitCtl.ascx" TagName="ctlGrdRegisteredPatient"
    TagPrefix="uc1" %>
<asp:Content ID="Content2" ContentPlaceHolderID="plhCustomButtonToolbar" runat="server">
    <li runat="server" id="btnMPEntryPopupSave">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbprocess.png")%>' alt="" /><div>
            <%=GetLabel("Process")%></div>
    </li>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div class="menuTitle">
        <%=HttpUtility.HtmlEncode(GetPageTitle())%></div>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript">
        function onAfterCustomClickSuccess(type) {
            onRefreshGridView();
        }

        $(function () {
            $('#<%=btnMPEntryPopupSave.ClientID %>').click(function () {
                var param = getCheckedRow();
                if (param == '') {
                    showToast('Warning', 'Please Select Medical Record First');
                }
                else {
                    $('#<%=hdnParam.ClientID %>').val(param);
                    if (IsValid(null, 'fsPatientList', 'mpPatientEntry')) {
                        onCustomButtonClick('save');
                        $('#<%=txtRemarks.ClientID %>').val('');
                    }
                }
            });

            setDatePicker('<%=txtFromRegistrationDate.ClientID %>');
            $('#<%=txtFromRegistrationDate.ClientID %>').datepicker('option', 'maxDate', '0');

            setDatePicker('<%=txtToRegistrationDate.ClientID %>');
            $('#<%=txtToRegistrationDate.ClientID %>').datepicker('option', 'maxDate', '0');

            $('#<%=txtFromRegistrationDate.ClientID %>').change(function (evt) {
                onRefreshGridView();
            });

            $('#<%=txtToRegistrationDate.ClientID %>').change(function (evt) {
                onRefreshGridView();
            });

            setDatePicker('<%=txtDateToDay.ClientID %>');
            $('#<%=txtDateToDay.ClientID %>').datepicker('option', 'maxDate', '0');

            $('#lblRefresh.lblLink').click(function (evt) {
                onRefreshGridView();
            });

            $('#<%=chkIsUsedCurrentDateTime.ClientID %>').change(function () {
                if ($(this).is(':checked')) {
                    $('#<%=txtDateToDay.ClientID %>').attr('readonly', 'readonly');
                    $('#<%=txtTimeToDay.ClientID %>').attr('readonly', 'readonly');
                }
                else {
                    $('#<%=txtDateToDay.ClientID %>').removeAttr('readonly');
                    $('#<%=txtTimeToDay.ClientID %>').removeAttr('readonly');
                }
            });
        });

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

        function GetCurrentDateTime() {
            var currentdate = new Date();
            //            var datetime = "Last Sync: " + currentdate.getDate() + "/"
            //                + (currentdate.getMonth() + 1) + "/"
            //                + currentdate.getFullYear() + " @ "
            //                + currentdate.getHours() + ":"
            //                + currentdate.getMinutes() + ":"
            //                + currentdate.getSeconds();

            $('#<%=txtTimeToDay.ClientID %>').val(currentdate.getHours() + ":" + currentdate.getMinutes());
        }

        function onProcessTypeChanged() {
            onRefreshGridView();
        }

        function onCboPatientFromValueChanged() {
            onRefreshGridView();
        }

        function onCboPatientTypeValueChanged(s) {
            var value = s.GetValue();
            if (s.GetValue() != null && s.GetValue() == '1') {
                cboJenisBerkas.SetValue('X111^001');
                cboJenisBerkas.SetEnabled(false);
            }
            else {
                cboJenisBerkas.SetValue('X111^003');
                cboJenisBerkas.SetEnabled(false);
            }
            onRefreshGridView();
        }

        function onCboPatientStatusValueChanged() {
            onRefreshGridView();
        }

        function onTxtSearchViewSearchClick(s) {
            setTimeout(function () {
                s.SetBlur();
                onRefreshGridView();
            }, 0);
        }

        $('.lblMedicalNo.lblLink').live('click', function () {
            $tr = $(this).closest('tr').closest('tr');
            var id = $tr.find('.keyField').html();
            var visitDate = $('#<%=txtFromRegistrationDate.ClientID %>').val() + ';' + $('#<%=txtToRegistrationDate.ClientID %>').val();
            var param = id + '|' + visitDate;
            var url = ResolveUrl("~/Program/PatientMedicalRecord/MedicalFolderTracking/PatientVisitLogCtl.ascx");
            openUserControlPopup(url, param, 'Daftar Kunjungan Pasien', 700, 500);
        });

        $('.lblRemarks.lblLink').live('click', function () {
            $tr = $(this).closest('tr').closest('tr');
            var id = $tr.find('.keyField').html();
            var visitDate = $('#<%=txtFromRegistrationDate.ClientID %>').val() + ';' + $('#<%=txtToRegistrationDate.ClientID %>').val();
            var param = id + '|' + visitDate;
            var url = ResolveUrl("~/Program/PatientMedicalRecord/MedicalFolderTracking/PatientMRFileLogCtl.ascx");
            openUserControlPopup(url, param, 'Histori Berkas Rekam Medis', 700, 500);
        });

        function onBeforeRightPanelPrint(code, filterExpression, errMessage) {
            var logDate = Methods.DatePickerToDateFormat($('#<%=txtDateToDay.ClientID %>').val());
            //Check Department
            if (cboPatientFrom.GetValue() == null || cboPatientFrom.GetValue().toString() == "") {
                $('#<%=hdnDepartment.ClientID %>').val('%%');
            }
            else {
                $('#<%=hdnDepartment.ClientID %>').val(cboPatientFrom.GetValue().toString());
            }
            //End Check Department

            if (logDate == '' || logDate == '0') {
                errMessage.text = 'Please Select Date First!';
                return false;
            }
            else {
                if (code == 'MR-00001' || code == 'MR-00002' || code == 'MR-00003') {
                    if (code == 'MR-00003') {
                        if ($('#<%=hdnDepartment.ClientID %>').val() != '%%') {
                            filterExpression.text = "ActualVisitDate = '" + logDate + "' AND DepartmentID = '" + $('#<%=hdnDepartment.ClientID %>').val() + "'";
                        }
                        else if ($('#<%=hdnDepartment.ClientID %>').val() == '%%') {
                            filterExpression.text = "ActualVisitDate = '" + logDate + "'";
                        }
                    }
                    else {
                        if ($('#<%=hdnDepartment.ClientID %>').val() != '%%') {
                            filterExpression.text = "LogDate = '" + logDate + "' AND DepartmentID = '" + $('#<%=hdnDepartment.ClientID %>').val() + "'";
                        }
                        else if ($('#<%=hdnDepartment.ClientID %>').val() == '%%') {
                            filterExpression.text = "LogDate = '" + logDate + "'";
                        }
                    }
                }
                else if (code == 'MR-00006') {
                    filterExpression.text = "RegistrationDate = '" + logDate + "'";
                }
                else if (code == 'MR-00007') {
                    filterExpression.text = getFilterExpressionGridCtl();
                }
                return true;
            }
        }
    </script>
    <input type="hidden" value="" id="hdnDepartment" runat="server" />
    <input type="hidden" value="" id="hdnParam" runat="server" />
    <input type="hidden" value="" id="hdnMRN" runat="server" />
    <input type="hidden" value="" id="hdnVisitID" runat="server" />
    <input type="hidden" value="" id="hdnFilterExpressionQuickSearch" runat="server" />
    <input type="hidden" value="" id="hdnPageTitle" runat="server" />
    <table class="tblContentArea" style="width: 100%">
        <tr>
            <td style="padding: 5px; vertical-align: top">
                <fieldset id="fsPatientList">
                    <table>
                        <colgroup>
                            <col style="width: 150px" />
                        </colgroup>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblMandatory">
                                    <%=GetLabel("Status Pasien")%></label>
                            </td>
                            <td>
                                <dxe:ASPxComboBox ID="cboPatientType" Width="100%" ClientInstanceName="cboPatientType"
                                    runat="server">
                                    <ClientSideEvents ValueChanged="function(s,e){ onCboPatientTypeValueChanged(s); }"
                                        Init="function(s,e){ onCboPatientTypeValueChanged(s); }" />
                                </dxe:ASPxComboBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblMandatory">
                                    <%=GetLabel("Proses Berkas")%></label>
                            </td>
                            <td>
                                <dxe:ASPxComboBox ID="cboJenisBerkas" Width="100%" ClientInstanceName="cboJenisBerkas"
                                    runat="server">
                                </dxe:ASPxComboBox>
                            </td>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Asal Pasien")%></label>
                            </td>
                            <td>
                                <dxe:ASPxComboBox ID="cboPatientFrom" ClientInstanceName="cboPatientFrom" runat="server"
                                    Width="100%">
                                    <ClientSideEvents ValueChanged="function(s,e) { onCboPatientFromValueChanged(); }" />
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
                                            <asp:TextBox ID="txtFromRegistrationDate" Width="120px" CssClass="datepicker" runat="server" />
                                        </td>
                                        <td>
                                            <%=GetLabel("s/d") %>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtToRegistrationDate" Width="120px" CssClass="datepicker" runat="server" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td class="tdLabel">
                                <label class="lblMandatory">
                                    <%=GetLabel("Status Pasien")%></label>
                            </td>
                            <td>
                                <dxe:ASPxComboBox ID="cboPatientStatus" Width="100%" ClientInstanceName="cboPatientStatus"
                                    runat="server">
                                    <ClientSideEvents ValueChanged="function(s,e) { onCboPatientStatusValueChanged(); }" />
                                </dxe:ASPxComboBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label>
                                    <%=GetLabel("Quick Filter")%></label>
                            </td>
                            <td>
                                <qis:QISIntellisenseTextBox runat="server" ClientInstanceName="txtSearchView" ID="txtSearchView"
                                    Width="300px" Watermark="Search">
                                    <ClientSideEvents SearchClick="function(s){ onTxtSearchViewSearchClick(s); }" />
                                    <IntellisenseHints>
                                        <qis:QISIntellisenseHint Text="No RM" FieldName="MedicalNo" />
                                        <qis:QISIntellisenseHint Text="Nama Pasien" FieldName="PatientName" />
                                        <qis:QISIntellisenseHint Text="No Registrasi" FieldName="RegistrationNo" />
                                        <qis:QISIntellisenseHint Text="Service Unit" FieldName="ServiceUnitName" />
                                    </IntellisenseHints>
                                </qis:QISIntellisenseTextBox>
                            </td>
                        </tr>
                    </table>
                    <div style="padding: 7px 0 0 3px; font-size: 0.95em">
                        <%=GetLabel("Halaman Ini Akan")%>
                        <span class="lblLink" id="lblRefresh">[refresh]</span>
                        <%=GetLabel("setiap")%>
                        <%=GetRefreshGridInterval() %>
                        <%=GetLabel("menit")%>
                    </div>
                    <hr />
                    <table class="tblEntryContent">
                        <colgroup>
                            <col style="width: 150px" />
                            <col style="width: 150px" />
                            <col style="width: 100px" />
                            <col />
                        </colgroup>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblMandatory">
                                    <%=GetLabel("Pembawa Berkas")%></label>
                            </td>
                            <td colspan="3">
                                <asp:TextBox ID="txtPicker" Width="295px" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <%=GetLabel("Tanggal - Jam")%>
                            </td>
                            <td>
                                <asp:TextBox ID="txtDateToDay" Width="120px" runat="server" CssClass="datepicker"
                                    ReadOnly="true" />
                            </td>
                            <td>
                                <asp:TextBox ID="txtTimeToDay" Width="80px" CssClass="time" runat="server" ReadOnly="true" />
                            </td>
                            <td>
                                <asp:CheckBox ID="chkIsUsedCurrentDateTime" runat="server" Checked="true" /><%:GetLabel("Gunakan Tanggal & Jam Sistem")%>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Keterangan")%></label>
                            </td>
                            <td colspan="3">
                                <asp:TextBox ID="txtRemarks" Width="540px" runat="server" />
                            </td>
                        </tr>
                    </table>
                </fieldset>
                <uc1:ctlGrdRegisteredPatient runat="server" ID="grdRegisteredPatient" />
            </td>
        </tr>
    </table>
</asp:Content>
