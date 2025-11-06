<%@ Page Title="" Language="C#" MasterPageFile="~/libs/MasterPage/MPMain.master" AutoEventWireup="true" 
CodeBehind="ImagingTestResult.aspx.cs" Inherits="QIS.Medinfras.Web.Imaging.Program.WorkList.ImagingTestResult" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Src="~/Libs/Controls/PatientGrid/GridPatientResultCtl.ascx" TagName="ctlGrdPatientResult" TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="plhMPMain" runat="server">
    <script type="text/javascript">
        //#region Inisialisasi
        $(function () {
            $('#<%=txtServiceUnitName.ClientID %>').attr("readonly", "readonly");
            setDatePicker('<%=txtTransactionDate.ClientID %>');

            $('#<%=txtTransactionDate.ClientID %>').datepicker('option', 'maxDate', '0');

            $('#<%=txtTransactionDate.ClientID %>').change(function () {
                if (IsValid(null, 'fsPatientList', 'mpPatientList')) {
                    cbpView.PerformCallback('refresh');
                }
            });

            //#region ServiceUnit
            function getHealthcareServiceUnitFilterExpression() {
                var filterExpression = "DepartmentID = '" + cboPatientFrom.GetValue() + "'"; ;
                return filterExpression;
            }

            $('#lblServiceUnit.lblLink').click(function () {
                openSearchDialog('serviceunitperhealthcare', getHealthcareServiceUnitFilterExpression(), function (value) {
                    $('#<%=txtServiceUnitCode.ClientID %>').val(value);
                    onTxtServiceUnitCodeChanged(value);
                });
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
                    if (IsValid(null, 'fsPatientList', 'mpPatientList'))
                        cbpView.PerformCallback('refresh');
                });
            }
            //#endregion 

        });

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
            cbpView.PerformCallback('refresh');
        }
        //#endregion

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
                if (IsValid(null, 'fsPatientList', 'mpPatientList')) {
                    $('#<%=txtServiceUnitCode.ClientID %>').val('');
                    $('#<%=txtServiceUnitName.ClientID %>').val('');
                    cbpView.PerformCallback('refresh');
                }
            }

           
    </script>
    <div style="padding:15px">
        <input type="hidden" id="hdnFilterExpression" runat="server" value="" />
        <div class="pageTitle"><%=GetLabel("Work List : Hasil Pemeriksaan Pasien")%></div>
        <table class="tblContentArea" style="width:100%">
            <tr>
                <td style="padding:5px;vertical-align:top">
                    <fieldset id="fsPatientList"> 
                    <table class="tblEntryContent" style="width:60%;">
                        <colgroup>
                            <col style="width:25%"/>
                            <col/>
                        </colgroup>
                        <tr>
                            <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Tanggal")%></label></td>
                            <td><asp:TextBox ID="txtTransactionDate" Width="120px" runat="server" CssClass="datepicker"/></td>
                        </tr>
                        <tr>
                            <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Penunjang Medis")%></label></td>
                            <td>
                                <dxe:ASPxComboBox ID="cboMedicSupport" ClientInstanceName="cboMedicSupport" runat="server" Width="350px">
                                    <ClientSideEvents ValueChanged="function(s,e) { oncboMedicSuppport(); }"/>
                                </dxe:ASPxComboBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Nama Pasien")%></label></td>
                            <td><asp:TextBox ID="txtPatientName" Width="200px" runat="server" /></td>
                        </tr>
                                                <tr>
                            <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Asal Pasien")%></label></td>
                            <td>
                                <input type="hidden" id="hdnDepartmentID" value="" runat="server" />
                                <dxe:ASPxComboBox ID="cboPatientFrom" ClientInstanceName="cboPatientFrom" runat="server" Width="200px">
                                    <ClientSideEvents ValueChanged="function(s,e) { onCboPatientFromValueChanged(); }"/>
                                </dxe:ASPxComboBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel"><label class="lblLink lblNormal" id="lblServiceUnit"><%=GetLabel("Service Unit")%></label></td>
                            <td>
                                <input type="hidden" id="hdnServiceUnitID" value="" runat="server" />
                                <table style="width:100%" cellpadding="0" cellspacing="0">
                                    <colgroup>
                                        <col style="width:30%"/>
                                        <col style="width:3px"/>
                                        <col/>
                                    </colgroup>
                                    <tr>
                                        <td><asp:TextBox ID="txtServiceUnitCode" Width="100%" runat="server" /></td>
                                        <td>&nbsp;</td>
                                        <td><asp:TextBox ID="txtServiceUnitName" Width="200px" runat="server" /></td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel"><label class="lblNormal"><%=GetLabel("No Rekam Medis")%></label></td>
                            <td><asp:TextBox ID="txtMedicalRecordNo" Width="200px" runat="server" /></td>
                        </tr>
                    </table>
                    </fieldset>
                    <div style="padding:7px 0 0 3px;font-size:0.95em">
                        <%=GetLabel("Halaman Ini Akan")%> <span class="lblLink" id="lblRefresh">[refresh]</span> <%=GetLabel("Setiap 10 Menit")%>
                    </div>
                    <uc1:ctlGrdPatientResult runat="server" id="grdPatientResult" />
                </td>
            </tr>
            
        </table>
    </div>
</asp:Content>
