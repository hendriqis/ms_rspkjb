<%@ Page Title="" Language="C#" MasterPageFile="~/libs/MasterPage/MPMain.master"
    AutoEventWireup="true" CodeBehind="PatientPageList.aspx.cs" Inherits="QIS.Medinfras.Web.Radiotheraphy.Program.PatientPageList" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="QIS.Medinfras.Web.CustomControl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"
    Namespace="QIS.Medinfras.Web.CustomControl" TagPrefix="qis" %>
<%@ Register Src="~/Libs/Controls/PatientGrid/GridPatientRegOrderCtl1.ascx" TagName="ctlGrdRegOrderPatient"
    TagPrefix="uc1" %>
<%@ Register Src="~/Libs/Controls/PatientGrid/GridPatientRegOrderCtl2.ascx" TagName="ctlGrdRegOrderPatient2"
    TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="plhMPMain" runat="server">
    <script type="text/javascript">
        $(function () {
            $('#ulTabLabResult li').click(function () {
                $('#ulTabLabResult li.selected').removeAttr('class');
                $('.containerOrder').filter(':visible').hide();
                $contentID = $(this).attr('contentid');
                $('#' + $contentID).show();
                $(this).addClass('selected');
            });

            //#region Attr Registrasi
            setDatePicker('<%=txtRealisationDate.ClientID %>');
            $('#<%=txtRealisationDate.ClientID %>').datepicker('option', 'maxDate', '0');
            $('#<%=txtRealisationDate.ClientID %>').change(function () {
                onRefreshGrdReg();
            });
            $('#lblRefresh.lblLink').click(function () {
                onRefreshGrdReg();
            });

            function getHealthcareServiceUnitFilterExpression() {
                var filterExpression = "IsUsingRegistration = 1 AND HealthcareID = '" + AppSession.healthcareID + "' AND DepartmentID = '" + cboPatientFrom.GetValue() + "' AND IsDeleted = 0"; ;
                return filterExpression;
            }

            $('#lblServiceUnit.lblLink').click(function () {
                openSearchDialog('serviceunitperhealthcare', getHealthcareServiceUnitFilterExpression(), function (value) {
                    $('#<%=txtServiceUnitCode.ClientID %>').val(value);
                    onTxtServiceUnitCodeChanged(value);
                });
            });

            $('#<%=txtServiceUnitCode.ClientID %>').change(function () {
                onTxtServiceUnitCodeChanged($(this).val());
            });

            function onTxtServiceUnitCodeChanged(value) {
                var filterExpression = getHealthcareServiceUnitFilterExpression() + " AND ServiceUnitCode = '" + value + "'";
                Methods.getObject('GetvHealthcareServiceUnitList', filterExpression, function (result) {
                    if (result != null) {
                        $('#<%=hdnServiceUnitID.ClientID %>').val(result.HealthcareServiceUnitID);
                        $('#<%=txtServiceUnitCode.ClientID %>').val(result.ServiceUnitCode);
                        $('#<%=txtServiceUnitName.ClientID %>').val(result.ServiceUnitName);
                    }
                    else {
                        $('#<%=hdnServiceUnitID.ClientID %>').val('');
                        $('#<%=txtServiceUnitCode.ClientID %>').val('');
                        $('#<%=txtServiceUnitName.ClientID %>').val('');
                    }
                    onRefreshGrdReg();
                });
            }
            //#endregion
        });

        var interval = parseInt('<%=GetRefreshGridInterval() %>') * 60000;

        //#region Registration

        $('#<%=chkIsPreviousEpisodePatientReg.ClientID %>').die();
        $('#<%=chkIsPreviousEpisodePatientReg.ClientID %>').live('change', function () {
            if ($(this).is(':checked')) {
                $('#<%=txtRealisationDate.ClientID %>').attr('readonly', 'readonly');
            }
            else $('#<%=txtRealisationDate.ClientID %>').removeAttr('readonly');
            onRefreshGrdReg();
        });

        $('#<%=chkIsHasOrder.ClientID %>').die();
        $('#<%=chkIsHasOrder.ClientID %>').live('change', function () {
            if ($(this).is(':checked')) {
                $('#<%:grd.ClientID %>').attr('style', 'display:none');
                $('#<%:grd2.ClientID %>').removeAttr('style');
            }
            else {
                $('#<%:grd2.ClientID %>').attr('style', 'display:none');
                $('#<%:grd.ClientID %>').removeAttr('style');
            }
            onRefreshGrdReg();
        });

        function onCboPatientFromValueChanged() {
            var cboValue = cboPatientFrom.GetValue();
            if (cboValue == Constant.Facility.INPATIENT)
                $('#trRegistrationDate').attr('style', 'display:none');
            else
                $('#trRegistrationDate').removeAttr('style');

            $('#<%=hdnServiceUnitID.ClientID %>').val('');
            $('#<%=txtServiceUnitCode.ClientID %>').val('');
            $('#<%=txtServiceUnitName.ClientID %>').val('');
            onRefreshGrdReg();
        }
        function onCboMedicalDiagnosticValueChanged() {
            onRefreshGrdReg();
        }

        var intervalIDReg = window.setInterval(function () {
            onRefreshGrdReg();
        }, interval);

        function onRefreshGrdReg() {
            var isChecked = $('#<%=chkIsHasOrder.ClientID %>').is(":checked");
            if (IsValid(null, 'fsPatientListReg', 'mpPatientList')) {
                window.clearInterval(intervalIDReg);
                $('#<%=hdnFilterExpressionQuickSearchReg.ClientID %>').val(txtSearchViewReg.GenerateFilterExpression());
                if (isChecked) {
                    refreshGrdRegisteredPatient2();
                }
                else {
                    refreshGrdRegisteredPatient();
                }
                intervalIDReg = window.setInterval(function () {
                    onRefreshGrdReg();
                }, interval);
            }
        }

        function onTxtSearchViewRegSearchClick(s) {
            setTimeout(function () {
                s.SetBlur();
                onRefreshGrdReg();
                setTimeout(function () {
                    s.SetFocus();
                }, 0);
            }, 0);
        }
        //#endregion
    </script>
    <input type="hidden" value="" id="hdnFilterExpressionQuickSearchReg" runat="server" />
    <input type="hidden" value="" id="hdnFilterExpressionQuickSearchOrder" runat="server" />
    <input type="hidden" id="hdnFilterExpression" runat="server" value="" />
    <input type="hidden" id="hdnPageHSUID" runat="server" value="" />
    <div style="padding: 15px">
        <div class="containerUlTabPage" style="margin-bottom: 3px;">
            <ul class="ulTabPage" id="ulTabLabResult">
                <li class="selected" contentid="containerDaftar">
                    <%=GetLabel("Daftar Pasien")%></li>
            </ul>
        </div>
        <div id="containerDaftar" class="containerOrder">
            <div class="pageTitle">
                <%=GetLabel("Pilih Pasien")%></div>
            <fieldset id="fsPatientListReg">
                <table class="tblContentArea" style="width: 100%">
                    <tr>
                        <td style="padding: 5px; vertical-align: top">
                            <table class="tblEntryContent" style="width: 60%;">
                                <colgroup>
                                    <col style="width: 200px" />
                                    <col />
                                </colgroup>
                                <tr>
                                    <td class="tdLabel">
                                        <label class="lblNormal">
                                            <%=GetLabel("Asal Pasien")%></label>
                                    </td>
                                    <td>
                                        <table border="0" cellpadding="1" cellspacing="0" width="100%">
                                            <colgroup>
                                                <col width="200px" />
                                                <col />
                                            </colgroup>
                                            <tr>
                                                <td>
                                                    <dxe:ASPxComboBox ID="cboPatientFrom" ClientInstanceName="cboPatientFrom" runat="server"
                                                        Width="350px">
                                                        <ClientSideEvents ValueChanged="function(s,e) { onCboPatientFromValueChanged(); }" />
                                                    </dxe:ASPxComboBox>
                                                </td>
                                                <td>
                                                    <asp:CheckBox ID="chkIsHasOrder" runat="server" Text=" Yang memiliki order saja" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel">
                                        <label class="lblLink lblNormal" id="lblServiceUnit">
                                            <%=GetLabel("Unit Pelayanan")%></label>
                                    </td>
                                    <td>
                                        <input type="hidden" id="hdnServiceUnitID" value="" runat="server" />
                                        <table style="width: 100%" cellpadding="0" cellspacing="0">
                                            <colgroup>
                                                <col style="width: 100px" />
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
                                                    <asp:TextBox ID="txtServiceUnitName" ReadOnly="true" Width="450px" runat="server" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr id="trRegistrationDate">
                                    <td class="tdLabel">
                                        <label class="lblNormal">
                                            <%=GetLabel("Tanggal")%></label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtRealisationDate" Width="120px" runat="server" CssClass="datepicker" />
                                        <asp:CheckBox ID="chkIsPreviousEpisodePatientReg" runat="server" Text="Abaikan Tanggal" Visible="false" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel">
                                        <label>
                                            <%=GetLabel("Quick Filter")%></label>
                                    </td>
                                    <td>
                                        <qis:QISIntellisenseTextBox runat="server" ClientInstanceName="txtSearchViewReg"
                                            ID="txtSearchViewReg" Width="350px" Watermark="Search">
                                            <ClientSideEvents SearchClick="function(s){ onTxtSearchViewRegSearchClick(s); }" />
                                            <IntellisenseHints>
                                                <qis:QISIntellisenseHint Text="Nama Pasien" FieldName="PatientName" />
                                                <qis:QISIntellisenseHint Text="No Registrasi" FieldName="RegistrationNo" />
                                                <qis:QISIntellisenseHint Text="No.RM" FieldName="MedicalNo" />
                                            </IntellisenseHints>
                                        </qis:QISIntellisenseTextBox>
                                    </td>
                                </tr>
                            </table>
                            <div style="padding: 7px 0 0 3px; font-size: 0.95em">
                                <%=GetLabel("Halaman Ini Akan")%>
                                <span class="lblLink" id="lblRefresh">[refresh]</span>
                                <%=GetLabel("Setiap")%>
                                <%=GetRefreshGridInterval() %>
                                <%=GetLabel("Menit")%>
                            </div>
                            <div id="grd" runat='server'>
                                <uc1:ctlGrdRegOrderPatient runat="server" ID="grdRegisteredPatient" />
                            </div>
                            <div id="grd2" runat='server' style='display:none'>
                                <uc1:ctlGrdRegOrderPatient2 runat="server" ID="grdRegisteredPatient2" />
                            </div>
                        </td>
                    </tr>
                </table>
            </fieldset>
        </div>
    </div>
</asp:Content>

