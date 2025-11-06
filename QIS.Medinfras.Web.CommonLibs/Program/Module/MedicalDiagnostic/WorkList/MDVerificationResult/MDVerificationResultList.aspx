<%@ Page Title="" Language="C#" MasterPageFile="~/libs/MasterPage/MPMain.master"
    AutoEventWireup="true" CodeBehind="MDVerificationResultList.aspx.cs" Inherits="QIS.Medinfras.Web.CommonLibs.Program.MDVerificationResultList" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="QIS.Medinfras.Web.CustomControl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"
    Namespace="QIS.Medinfras.Web.CustomControl" TagPrefix="qis" %>
<%@ Register Src="~/Libs/Controls/PatientGrid/GridImagingResultVerificationCtl.ascx"
    TagName="ctlGrdImagingResultVerification" TagPrefix="uc" %>
<asp:Content ID="Content1" ContentPlaceHolderID="plhMPMain" runat="server">
    <script type="text/javascript">
        $(function () {
            $('#tdOpenRightPanel').attr('style', "display:none");
            $('#<%=txtServiceUnitName.ClientID %>').attr("readonly", "readonly");

            setDatePicker('<%=txtTransactionDateFrom.ClientID %>');
            setDatePicker('<%=txtTransactionDateTo.ClientID %>');

            $('#<%=txtTransactionDateFrom.ClientID %>').datepicker('option', 'maxDate', new Date(getDateNow()));
            $('#<%=txtTransactionDateTo.ClientID %>').datepicker('option', 'maxDate', new Date(getDateNow()));

            //#region ServiceUnit
            function getHealthcareServiceUnitFilterExpression() {
                var filterExpression = "IsUsingRegistration = 1 AND DepartmentID = '" + cboPatientFrom.GetValue() + "'"; ;
                return filterExpression;
            }

            $('#lblServiceUnit.lblLink').click(function () {
                openSearchDialog('serviceunitperhealthcare', getHealthcareServiceUnitFilterExpression(), function (value) {
                    $('#<%=txtServiceUnitCode.ClientID %>').val(value);
                    onTxtServiceUnitCodeChanged(value);
                });
            });

            $('#lblRefresh.lblLink').click(function () {
                onRefreshGridView();
            });

            $('#<%=txtServiceUnitCode.ClientID %>').change(function () {
                if (IsValid(null, 'fsPatientList', 'mpPatientList'))
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
                    onRefreshGridView();
                });
            }
            //#endregion 

        });

        var interval = parseInt('<%=GetRefreshGridInterval() %>') * 60000;
        var intervalID = window.setInterval(function () {
            onRefreshGridView();
        }, interval);

        function onRefreshGridView() {
            if (IsValid(null, 'fsPatientList', 'mpPatientList')) {
                window.clearInterval(intervalID);
                $('#<%=hdnFilterExpressionQuickSearch.ClientID %>').val(txtSearchView.GenerateFilterExpression());
                refreshGrdResultPatient();
                intervalID = window.setInterval(function () {
                    onRefreshGridView();
                }, interval);
            }
        }

        function onCbpViewEndCallback(s) {
            hideLoadingPanel();
            var param = s.cpResult.split('|');
            if (param[0] == 'refresh') {
                var pageCount = parseInt(param[1]);
                setPaging($("#paging"), pageCount, function (page) {
                    cbpView.PerformCallback('changepage|' + page);
                });
            }
        }

        function oncboMedicSuppport() {
            if (IsValid(null, 'fsPatientList', 'mpPatientList'))
                onRefreshGridView();
        }

        //#region Paging
        var pageCount = parseInt('<%=PageCount %>');
        var currPage = parseInt('<%=CurrPage %>');
        $(function () {
            setPaging($("#paging"), pageCount, function (page) {
                cbpView.PerformCallback('changepage|' + page);
            }, null, currPage);
        });
        //#endregion

        function onCboPatientFromValueChanged() {
            $('#<%=hdnServiceUnitID.ClientID %>').val('');
            $('#<%=txtServiceUnitCode.ClientID %>').val('');
            $('#<%=txtServiceUnitName.ClientID %>').val('');
            onRefreshGridView();
        }

        function onCboResultTypeValueChanged(evt) {
            onRefreshGridView();
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
    </script>
    <input type="hidden" value="" id="hdnFilterExpressionQuickSearch" runat="server" />
    <input type="hidden" id="hdnImagingID" runat="server" value="" />
    <input type="hidden" id="hdnLabID" runat="server" value="" />
    <input type="hidden" id="hdnTrigger" runat="server" value="" />
    <div style="padding: 15px">
        <input type="hidden" id="hdnFilterExpression" runat="server" value="" />
        <div class="pageTitle">
            <%=GetLabel("Work List : Verifikasi Hasil Pemeriksaan/Pelayanan")%></div>
        <table class="tblContentArea" style="width: 100%">
            <tr>
                <td style="padding: 5px; vertical-align: top">
                    <fieldset id="fsPatientList">
                        <table class="tblEntryContent" style="width: 60%;">
                            <colgroup>
                                <col style="width: 25%" />
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
                                                <asp:TextBox ID="txtTransactionDateFrom" Width="120px" CssClass="datepicker" runat="server" />
                                            </td>
                                            <td>
                                                <%=GetLabel("s/d") %>
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtTransactionDateTo" Width="120px" CssClass="datepicker" runat="server" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr id="trServiceUnit" runat="server">
                                <td class="tdLabel">
                                    <label class="lblMandatory">
                                        <%=GetLabel("Penunjang Medis")%></label>
                                </td>
                                <td>
                                    <dxe:ASPxComboBox ID="cboMedicSupport" ClientInstanceName="cboMedicSupport" runat="server"
                                        Width="350px">
                                        <ClientSideEvents ValueChanged="function(s,e) { oncboMedicSuppport(); }" />
                                    </dxe:ASPxComboBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblNormal">
                                        <%=GetLabel("Asal Department")%></label>
                                </td>
                                <td>
                                    <input type="hidden" id="hdnDepartmentID" value="" runat="server" />
                                    <dxe:ASPxComboBox ID="cboPatientFrom" ClientInstanceName="cboPatientFrom" runat="server"
                                        Width="300px">
                                        <ClientSideEvents ValueChanged="function(s,e) { onCboPatientFromValueChanged(); }" />
                                    </dxe:ASPxComboBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblLink lblNormal" id="lblServiceUnit">
                                        <%=GetLabel("Asal Unit Pelayanan")%></label>
                                </td>
                                <td>
                                    <input type="hidden" id="hdnServiceUnitID" value="" runat="server" />
                                    <table style="width: 100%" cellpadding="0" cellspacing="0">
                                        <colgroup>
                                            <col style="width: 100px" />
                                            <col style="width: 3px" />
                                            <col style="width: 300px" />
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
                                                <asp:TextBox ID="txtServiceUnitName" Width="100%" runat="server" />
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
                                        Width="300px" Watermark="Search">
                                        <ClientSideEvents SearchClick="function(s){ onTxtSearchViewSearchClick(s); }" />
                                        <IntellisenseHints>
                                            <qis:QISIntellisenseHint Text="No Bukti" FieldName="TransactionNo" />
                                            <qis:QISIntellisenseHint Text="No Registrasi" FieldName="RegistrationNo" />
                                            <qis:QISIntellisenseHint Text="No.RM" FieldName="MedicalNo" />
                                            <qis:QISIntellisenseHint Text="Nama Pasien" FieldName="PatientName" />
                                        </IntellisenseHints>
                                    </qis:QISIntellisenseTextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblNormal">
                                        <%=GetLabel("Tampilan Hasil")%></label>
                                </td>
                                <td>
                                    <dxe:ASPxComboBox ID="cboResultType" ClientInstanceName="cboResultType" Width="300px"
                                        runat="server">
                                        <ClientSideEvents ValueChanged="function(s,e) { onCboResultTypeValueChanged(e); }" />
                                    </dxe:ASPxComboBox>
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
                    <uc:ctlGrdImagingResultVerification runat="server" id="grdVerificationResult" />
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
