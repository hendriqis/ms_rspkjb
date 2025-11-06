<%@ Page Title="" Language="C#" MasterPageFile="~/libs/MasterPage/MPMain.master"
    AutoEventWireup="true" CodeBehind="PatientPageList.aspx.cs" Inherits="QIS.Medinfras.Web.Imaging.Program.PatientPageList" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="QIS.Medinfras.Web.CustomControl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"
    Namespace="QIS.Medinfras.Web.CustomControl" TagPrefix="qis" %>
<%@ Register Src="~/Libs/Controls/PatientGrid/GridImagingPatientListCtl.ascx" TagName="ctlGrdPatientResult"
    TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="plhMPMain" runat="server">
    <script type="text/javascript">
        //#region Inisialisasi
        $(function () {
            $('#<%=txtServiceUnitName.ClientID %>').attr("readonly", "readonly");
            setDatePicker('<%=txtTransactionDateFrom.ClientID %>');
            setDatePicker('<%=txtTransactionDateTo.ClientID %>');

            $('#<%=txtTransactionDateFrom.ClientID %>').datepicker('option', 'maxDate', new Date(getDateNow()));
            $('#<%=txtTransactionDateTo.ClientID %>').datepicker('option', 'maxDate', new Date(getDateNow()));

            $('#<%=txtTransactionDateFrom.ClientID %>').change(function () {
                onRefreshGridView();
            });

            $('#<%=txtTransactionDateTo.ClientID %>').change(function () {
                onRefreshGridView();
            });

            //#region ServiceUnit
            function getHealthcareServiceUnitFilterExpression() {
                var filterExpression = "IsUsingRegistration = 1 AND DepartmentID = '" + cboDepartment.GetValue() + "'"; ;
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

        //#region Physician
        function onGetPhysicianFilterExpression() {
            var filterExpression = "";
            var healthcareServiceUnitID = $('#<%=hdnImagingServiceUnitID.ClientID %>').val();
            if (healthcareServiceUnitID != '0')
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
                onRefreshGridView();
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

        function oncboModalityChanged() {
            if (IsValid(null, 'fsPatientList', 'mpPatientList'))
                onRefreshGridView();
        }
        //#endregion
        
        function onCboOrderResultTypeValueChanged() {
            onRefreshGridView();
        }

        function onCboSortByValueChanged() {
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

        function onCboDepartmentValueChanged() {
            $('#<%=hdnServiceUnitID.ClientID %>').val('');
            $('#<%=txtServiceUnitCode.ClientID %>').val('');
            $('#<%=txtServiceUnitName.ClientID %>').val('');
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
    <input type="hidden" value="" id="hdnQuickText" runat="server" />
    <input type="hidden" id="hdnImagingID" runat="server" value="" />
    <input type="hidden" id="hdnLabID" runat="server" value="" />
    <input type="hidden" id="hdnTrigger" runat="server" value="" />
    <input type="hidden" id="hdnImagingServiceUnitID" runat="server" value="" />
    <div style="padding: 15px">
        <input type="hidden" id="hdnFilterExpression" runat="server" value="" />
        <div class="pageTitle">
            <%=GetLabel("Rekam Medis Pasien")%></div>
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
                                        <%=GetLabel("Tanggal Pemeriksaan")%></label>
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
                                        <%=GetLabel("Asal Pasien ")%></label>
                                </td>
                                <td>
                                    <dxe:ASPxComboBox ID="cboDepartment" ClientInstanceName="cboDepartment" Width="100%"
                                        runat="server">
                                        <ClientSideEvents ValueChanged="function(s,e) { onCboDepartmentValueChanged(e); }" />
                                    </dxe:ASPxComboBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblLink" id="lblServiceUnit">
                                        <%=GetLabel("Unit Pelayanan ")%></label>
                                </td>
                                <td>
                                    <input type="hidden" id="hdnServiceUnitID" runat="server" value="" />
                                    <table style="width: 100%" cellpadding="0" cellspacing="0">
                                        <colgroup>
                                            <col style="width: 150px" />
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
                                    <label class="lblNormal">
                                        <%=GetLabel("Modality")%></label>
                                </td>
                                <td>
                                    <dxe:ASPxComboBox ID="cboModality" ClientInstanceName="cboModality" runat="server"
                                        Width="150px">
                                        <ClientSideEvents ValueChanged="function(s,e) { oncboModalityChanged(); }" />
                                    </dxe:ASPxComboBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel"><label class="lblLink" id="lblPhysician"><%=GetLabel("Dokter Pelaksana")%></label></td>
                                <td>
                                    <input type="hidden" id="hdnPhysicianID" runat="server" value="" />
                                    <table style="width:100%" cellpadding="0" cellspacing="0">
                                        <colgroup>
                                            <col style="width:120px"/>
                                            <col style="width:3px"/>
                                            <col/>
                                        </colgroup>
                                        <tr>
                                            <td><asp:TextBox ID="txtPhysicianCode" Width="120px" runat="server" /></td>
                                            <td>&nbsp;</td>
                                            <td><asp:TextBox ID="txtPhysicianName" ReadOnly="true" Width="99%" runat="server" /></td>
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
                                            <qis:QISIntellisenseHint Text="No.RM" FieldName="MedicalNo" />
                                            <qis:QISIntellisenseHint Text="No Bukti" FieldName="TransactionNo" />
                                            <qis:QISIntellisenseHint Text="No Registrasi" FieldName="RegistrationNo" />
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
                                    <dxe:ASPxComboBox ID="cboOrderResultType" ClientInstanceName="cboOrderResultType"
                                        Width="150px" runat="server">
                                        <ClientSideEvents ValueChanged="function(s,e) { onCboOrderResultTypeValueChanged(); }" />
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
                    <uc1:ctlGrdPatientResult runat="server" ID="grdPatientResult" />
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
