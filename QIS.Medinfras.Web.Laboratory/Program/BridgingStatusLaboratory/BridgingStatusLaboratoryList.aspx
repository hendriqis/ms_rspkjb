<%@ Page Language="C#" MasterPageFile="~/libs/MasterPage/MPMain.master" AutoEventWireup="true"
    CodeBehind="BridgingStatusLaboratoryList.aspx.cs" Inherits="QIS.Medinfras.Web.Laboratory.Program.BridgingStatusLaboratoryList" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="QIS.Medinfras.Web.CustomControl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"
    Namespace="QIS.Medinfras.Web.CustomControl" TagPrefix="qis" %>
<%@ Register Src="~/Libs/Controls/PatientGrid/GridPatientRegOrderCtl.ascx" TagName="ctlGrdRegOrderPatient"
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
            $('#<%=txtRealisationDate.ClientID %>').datepicker('option', 'maxDate', new Date(getDateNow()));
            $('#<%=txtRealisationDate.ClientID %>').change(function () {
                onRefreshGrdReg();
            });
            $('#lblRefresh.lblLink').click(function () {
                onRefreshGrdReg();
            });

            function getHealthcareServiceUnitFilterExpression() {
                var filterExpression = "IsUsingRegistration = 1 AND HealthcareID = '" + AppSession.healthcareID + "' AND DepartmentID = '" + cboPatientFrom.GetValue() + "'"; ;
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

        //#region Registration
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

        function onRefreshGrdReg() {
            if (IsValid(null, 'fsPatientListReg', 'mpPatientList')) {
                $('#<%=hdnFilterExpressionQuickSearchReg.ClientID %>').val(txtSearchViewReg.GenerateFilterExpression());
                refreshGrdRegisteredPatient();
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
    <input type="hidden" id="hdnImagingID" runat="server" value="" />
    <input type="hidden" id="hdnLabID" runat="server" value="" />
    <input type="hidden" id="hdnTrigger" runat="server" value="" />
    <input type="hidden" id="hdnModuleID" runat="server" value="" />
    <div style="padding: 15px">
        <input type="hidden" id="hdnFilterExpression" runat="server" value="" />
        <div class="pageTitle">
            <%=GetLabel("Status Bridging : Laboratorium")%></div>
        <table class="tblContentArea" style="width: 100%">
            <tr>
                <td style="padding: 5px; vertical-align: top">
                    <fieldset id="fsPatientList">
                        <table class="tblEntryContent" style="width: 60%;">
                            <colgroup>
                                <col style="width: 25%" />
                                <col />
                            </colgroup>
                            <tr id="trServiceUnitOrder" runat="server">
                                <td class="tdLabel">
                                    <label class="lblNormal">
                                        <%=GetLabel("Unit Pelayanan")%></label>
                                </td>
                                <td>
                                    <dxe:ASPxComboBox ID="cboMedicalDiagnostic" ClientInstanceName="cboMedicalDiagnostic"
                                        runat="server" Width="350px">
                                        <ClientSideEvents ValueChanged="function(s,e) { onCboMedicalDiagnosticValueChanged(); }" />
                                    </dxe:ASPxComboBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblNormal">
                                        <%=GetLabel("Asal Pasien")%></label>
                                </td>
                                <td>
                                    <dxe:ASPxComboBox ID="cboPatientFrom" ClientInstanceName="cboPatientFrom" runat="server"
                                        Width="350px">
                                        <ClientSideEvents ValueChanged="function(s,e) { onCboPatientFromValueChanged(); }" />
                                    </dxe:ASPxComboBox>
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
                                            <col style="width: 30%" />
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
                                                <asp:TextBox ID="txtServiceUnitName" ReadOnly="true" Width="100%" runat="server" />
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
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label>
                                        <%=GetLabel("Quick Filter")%></label>
                                </td>
                                <td>
                                    <qis:QISIntellisenseTextBox runat="server" ClientInstanceName="txtSearchViewReg"
                                        ID="txtSearchViewReg" Width="300px" Watermark="Search">
                                        <ClientSideEvents SearchClick="function(s){ onTxtSearchViewRegSearchClick(s); }" />
                                        <IntellisenseHints>
                                            <qis:QISIntellisenseHint Text="Nama Pasien" FieldName="PatientName" />
                                            <qis:QISIntellisenseHint Text="No.RM" FieldName="MedicalNo" />
                                            <qis:QISIntellisenseHint Text="No Registrasi" FieldName="RegistrationNo" />
                                        </IntellisenseHints>
                                    </qis:QISIntellisenseTextBox>
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
                    <uc1:ctlGrdRegOrderPatient runat="server" ID="grdRegisteredPatient" />
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
