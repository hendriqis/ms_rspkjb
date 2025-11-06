<%@ Page Language="C#" MasterPageFile="~/Libs/MasterPage/MPMain.master" AutoEventWireup="true"
    CodeBehind="LaboratoryVerificationResultList.aspx.cs" Inherits="QIS.Medinfras.Web.Laboratory.Program.LaboratoryVerificationResultList" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="QIS.Medinfras.Web.CustomControl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"
    Namespace="QIS.Medinfras.Web.CustomControl" TagPrefix="qis" %>
<%@ Register Src="~/Program/Worklist/LaboratoryVerificationResult/GridLabResultVerificationCtl.ascx"
    TagName="ctlGrdLabResultVerification" TagPrefix="ctl" %>
<asp:Content ID="Content1" ContentPlaceHolderID="plhMPMain" runat="server">
    <script type="text/javascript">
        $(function () {
            setDatePicker('<%=txtDate.ClientID %>');
            $('#<%=txtDate.ClientID %>').datepicker('option', 'maxDate', new Date(getDateNow()));

            $('#<%=txtDate.ClientID %>').change(function (evt) {
                onRefreshGridView();
            });

            $('#lblRefresh.lblLink').click(function (evt) {
                onRefreshGridView();
            });
        });

        $('#<%=chkIsIgnoreDate.ClientID %>').die();
        $('#<%=chkIsIgnoreDate.ClientID %>').live('change', function () {
            if ($(this).is(':checked')) {
                $('#<%=txtDate.ClientID %>').attr('readonly', 'readonly');
            }
            else $('#<%=txtDate.ClientID %>').removeAttr('readonly');
            onRefreshGridView();
        });

        //#region Service Unit
        $('#lblServiceUnit.lblLink').live('click', function () {
            var DepartmentID = cboDepartment.GetValue();
            var filterExpression = '';
            if (DepartmentID != '')
                filterExpression = "IsUsingRegistration = 1 AND DepartmentID = '" + DepartmentID + "' AND IsDeleted = 0";
            openSearchDialog('serviceunitperhealthcare', filterExpression, function (value) {
                $('#<%=txtServiceUnitCode.ClientID %>').val(value);
                onTxtServiceUnitCodeChanged(value);
            });
        });

        $('#<%=txtServiceUnitCode.ClientID %>').live('change', function () {
            onTxtServiceUnitCodeChanged($(this).val());
        });

        function onTxtServiceUnitCodeChanged(value) {
            var filterExpression = "ServiceUnitCode = '" + value + "'";
            Methods.getObject('GetvHealthcareServiceUnitList', filterExpression, function (result) {
                if (result != null) {
                    $('#<%=hdnServiceUnitID.ClientID %>').val(result.HealthcareServiceUnitID);
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

        function onCboDepartmentValueChanged() {
            $('#<%=hdnServiceUnitID.ClientID %>').val('');
            $('#<%=txtServiceUnitCode.ClientID %>').val('');
            $('#<%=txtServiceUnitName.ClientID %>').val('');
            onRefreshGridView();
        }
        //#endregion

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
    <div style="padding: 15px">
        <input type="hidden" id="hdnFilterExpression" runat="server" value="" />
        <input type="hidden" id="hdnID" runat="server" />
        <div class="pageTitle">
            <%=GetMenuCaption()%>
            :
            <%=GetLabel("Pilih Pasien")%></div>
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
                                        <%=GetLabel("Tanggal Transaksi")%></label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtDate" Width="120px" runat="server" CssClass="datepicker" />&nbsp;<asp:CheckBox
                                        ID="chkIsIgnoreDate" runat="server" Checked="false" /><%:GetLabel("Abaikan Tanggal")%>&nbsp;
                                </td>
                            </tr>
                            <tr id="trPenunjangMedis" runat="server">
                                <td class="tdLabel">
                                    <label class="lblNormal">
                                        <%=GetLabel("Penunjang Medis")%></label>
                                </td>
                                <td>
                                    <dxe:ASPxComboBox ID="cboServiceUnit" ClientInstanceName="cboServiceUnit" Width="100%"
                                        runat="server">
                                        <ClientSideEvents ValueChanged="function(s,e) { refreshGrdResultPatient(); }" />
                                    </dxe:ASPxComboBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblNormal">
                                        <%=GetLabel("Asal Pasien")%></label>
                                </td>
                                <td>
                                    <dxe:ASPxComboBox ID="cboDepartment" ClientInstanceName="cboDepartment" Width="100%"
                                        runat="server">
                                        <ClientSideEvents ValueChanged="function(s,e) { onCboDepartmentValueChanged(); }" />
                                    </dxe:ASPxComboBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblLink" id="lblServiceUnit">
                                        <%=GetLabel("Unit Pelayanan")%></label>
                                </td>
                                <td>
                                    <input type="hidden" id="hdnServiceUnitID" runat="server" value="" />
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
                                            <qis:QISIntellisenseHint Text="Nama Pasien" FieldName="PatientName" />
                                            <qis:QISIntellisenseHint Text="No Bukti" FieldName="TransactionNo" />
                                            <qis:QISIntellisenseHint Text="No Registrasi" FieldName="RegistrationNo" />
                                            <qis:QISIntellisenseHint Text="No.RM" FieldName="MedicalNo" />
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
                                    <dxe:ASPxComboBox ID="cboResultType" ClientInstanceName="cboResultType" Width="150px"
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
                    <ctl:ctlGrdLabResultVerification runat="server" ID="grdLabResultVerification" />
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
