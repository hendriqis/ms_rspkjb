<%@ Page Language="C#" MasterPageFile="~/Libs/MasterPage/MPTrx.master" AutoEventWireup="true"
    CodeBehind="ChangeTestOrderSurgeryReportEntry.aspx.cs" Inherits="QIS.Medinfras.Web.SystemSetup.Program.ChangeTestOrderSurgeryReportEntry" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<asp:Content ID="Content2" ContentPlaceHolderID="plhHeader" runat="server">
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div class="menuTitle">
        <%=HttpUtility.HtmlEncode(GetPageTitle())%></div>
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="plhCustomButtonToolbar" runat="server">
    <li id="btnProcess" runat="server" crudmode="R">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbprocess.png") %>' alt="" />
        <div>
            <%=GetLabel("Process") %></div>
    </li>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript">
        function onLoad() {
        }

        $('#<%=btnProcess.ClientID %>').live('click', function (evt) {
            var testOrderIDFrom = $('#<%:hdnTestOrderIDFrom.ClientID %>').val();
            var testOrderIDTo = $('#<%:hdnTestOrderIDTo.ClientID %>').val();
            if (testOrderIDFrom != null && testOrderIDFrom != "" && testOrderIDFrom != "0" && testOrderIDTo != null && testOrderIDTo != "" && testOrderIDTo != "0") {
                onCustomButtonClick('process');
            } else {
                displayMessageBox('WARNING', "Harap lengkapi Order Awal dan Order Tujuan terlebih dahulu.");
            }
        });

        //#region Test Order Awal
        $('#lblTestOrderFrom.lblLink').live('click', function () {
            openSearchDialog('patientsurgery', "1 = 1", function (value) {
                $('#<%:txtTestOrderNoFrom.ClientID %>').val(value);
                ontxtTestOrderNoFromChanged(value);
            });
        });

        $('#<%:txtTestOrderNoFrom.ClientID %>').live('change', function () {
            ontxtTestOrderNoFromChanged($(this).val());
        });

        function ontxtTestOrderNoFromChanged(value) {
            var filterExpression = "TestOrderNo = '" + value + "'";
            Methods.getObject('GetvPatientSurgerySearchDialogList', filterExpression, function (result) {
                if (result != null) {
                    $('#<%:hdnPatientSurgeryID.ClientID %>').val(result.PatientSurgeryID);
                    $('#<%:hdnTestOrderIDFrom.ClientID %>').val(result.TestOrderID);
                    $('#<%:txtTestOrderNoFrom.ClientID %>').val(result.TestOrderNo);
                    $('#<%:txtTestOrderDateFrom.ClientID %>').val(result.cfTestOrderDateInString);
                    $('#<%:hdnTestOrderIDTo.ClientID %>').val("");
                    $('#<%:txtTestOrderNoTo.ClientID %>').val("");
                    $('#<%:txtTestOrderDateTo.ClientID %>').val("");
                    $('#<%:hdnVisitID.ClientID %>').val(result.VisitID);
                    $('#<%:hdnRegistrationID.ClientID %>').val(result.RegistrationID);
                    $('#<%:txtRegistrationNo.ClientID %>').val(result.RegistrationNo);
                    $('#<%:txtPatient.ClientID %>').val("(" + result.MedicalNo + ") " + result.PatientName);
                    $('#<%:txtParamedicName.ClientID %>').val(result.ParamedicName);
                    $('#<%:txtReportDate.ClientID %>').val(result.cfReportDateInString);
                    $('#<%:txtReportTime.ClientID %>').val(result.ReportTime);
                    $('#<%:txtStartDate.ClientID %>').val(result.cfStartDateInString);
                    $('#<%:txtStartTime.ClientID %>').val(result.StartTime);
                    $('#<%:txtEndDate.ClientID %>').val(result.cfEndDateInString);
                    $('#<%:txtEndTime.ClientID %>').val(result.EndTime);
                    $('#<%:txtDuration.ClientID %>').val(result.Duration);
                    $('#<%:txtSurgeryNo.ClientID %>').val(result.SurgeryNo);
                }
                else {
                    $('#<%:hdnPatientSurgeryID.ClientID %>').val("");
                    $('#<%:hdnTestOrderIDFrom.ClientID %>').val("");
                    $('#<%:txtTestOrderNoFrom.ClientID %>').val("");
                    $('#<%:txtTestOrderDateFrom.ClientID %>').val("");
                    $('#<%:hdnTestOrderIDTo.ClientID %>').val("");
                    $('#<%:txtTestOrderNoTo.ClientID %>').val("");
                    $('#<%:txtTestOrderDateTo.ClientID %>').val("");
                    $('#<%:hdnVisitID.ClientID %>').val("");
                    $('#<%:hdnRegistrationID.ClientID %>').val("");
                    $('#<%:txtRegistrationNo.ClientID %>').val("");
                    $('#<%:txtPatient.ClientID %>').val("");
                    $('#<%:txtParamedicName.ClientID %>').val("");
                    $('#<%:txtReportDate.ClientID %>').val("");
                    $('#<%:txtReportTime.ClientID %>').val("");
                    $('#<%:txtStartDate.ClientID %>').val("");
                    $('#<%:txtStartTime.ClientID %>').val("");
                    $('#<%:txtEndDate.ClientID %>').val("");
                    $('#<%:txtEndTime.ClientID %>').val("");
                    $('#<%:txtDuration.ClientID %>').val("");
                    $('#<%:txtSurgeryNo.ClientID %>').val("");
                }
            });
        }
        //#endregion

        //#region Test Order Tujuan
        function getTestOrderToFilterExpression() {
            var oVisitID = $('#<%:hdnVisitID.ClientID %>').val();
            var testOrderIDFrom = $('#<%:hdnTestOrderIDFrom.ClientID %>').val();

            var filter = "HealthcareServiceUnitID = (SELECT spd.ParameterValue FROM SettingParameterDt spd WITH(NOLOCK) WHERE spd.HealthcareID = '001' AND spd.ParameterCode = 'MD0006')";
            filter += " AND GCTransactionStatus <> 'X121^999'";
            filter += " AND TestOrderID NOT IN (SELECT ISNULL(a.TestOrderID,0) FROM vPatientSurgerySearchDialog a WITH(NOLOCK))";
            filter += " AND TestOrderID != " + testOrderIDFrom;
            filter += " AND VisitID = " + oVisitID;

            return filter;
        }

        $('#lblTestOrderTo.lblLink').live('click', function () {
            var testOrderIDFrom = $('#<%:hdnTestOrderIDFrom.ClientID %>').val();
            if (testOrderIDFrom != null && testOrderIDFrom != "" && testOrderIDFrom != "0") {
                openSearchDialog('testorderhd', getTestOrderToFilterExpression(), function (value) {
                    $('#<%:txtTestOrderNoTo.ClientID %>').val(value);
                    ontxtTestOrderNoToChanged(value);
                });
            } else {
                displayMessageBox('WARNING', "Harap pilih Order Awal terlebih dahulu.");
            }
        });

        $('#<%:txtTestOrderNoTo.ClientID %>').live('change', function () {
            var testOrderIDFrom = $('#<%:hdnTestOrderIDFrom.ClientID %>').val();
            if (testOrderIDFrom != null && testOrderIDFrom != "" && testOrderIDFrom != "0") {
                ontxtTestOrderNoToChanged($(this).val());
            } else {
                displayMessageBox('WARNING', "Harap pilih Order Awal terlebih dahulu.");
            }
        });

        function ontxtTestOrderNoToChanged(value) {
            var filterExpression = getTestOrderToFilterExpression() + " AND TestOrderNo = '" + value + "'";
            Methods.getObject('GetvTestOrderHdList', filterExpression, function (result) {
                if (result != null) {
                    $('#<%:hdnTestOrderIDTo.ClientID %>').val(result.TestOrderID);
                    $('#<%:txtTestOrderNoTo.ClientID %>').val(result.TestOrderNo);
                    $('#<%:txtTestOrderDateTo.ClientID %>').val(result.TestOrderDateInString);
                }
                else {
                    $('#<%:hdnTestOrderIDTo.ClientID %>').val("");
                    $('#<%:txtTestOrderNoTo.ClientID %>').val("");
                    $('#<%:txtTestOrderDateTo.ClientID %>').val("");
                }
            });
        }
        //#endregion

        function onAfterCustomClickSuccess(type, retval) {
            var orderFrom = $('#<%:txtTestOrderNoFrom.ClientID %>').val();
            var orderTo = $('#<%:txtTestOrderNoTo.ClientID %>').val();

            var textMessage = "Nomor Order <b>" + orderFrom + "</b> berhasil diubah menjadi Nomor Order <b>" + orderTo + "</b>";
            showToast("Ubah Oder Laporan Operasi : ", textMessage);

            $('#<%:hdnPatientSurgeryID.ClientID %>').val("");
            $('#<%:hdnTestOrderIDFrom.ClientID %>').val("");
            $('#<%:txtTestOrderNoFrom.ClientID %>').val("");
            $('#<%:txtTestOrderDateFrom.ClientID %>').val("");
            $('#<%:hdnTestOrderIDTo.ClientID %>').val("");
            $('#<%:txtTestOrderNoTo.ClientID %>').val("");
            $('#<%:txtTestOrderDateTo.ClientID %>').val("");
            $('#<%:hdnVisitID.ClientID %>').val("");
            $('#<%:hdnRegistrationID.ClientID %>').val("");
            $('#<%:txtRegistrationNo.ClientID %>').val("");
            $('#<%:txtPatient.ClientID %>').val("");
            $('#<%:txtParamedicName.ClientID %>').val("");
            $('#<%:txtReportDate.ClientID %>').val("");
            $('#<%:txtReportTime.ClientID %>').val("");
            $('#<%:txtStartDate.ClientID %>').val("");
            $('#<%:txtStartTime.ClientID %>').val("");
            $('#<%:txtEndDate.ClientID %>').val("");
            $('#<%:txtEndTime.ClientID %>').val("");
            $('#<%:txtDuration.ClientID %>').val("");
            $('#<%:txtSurgeryNo.ClientID %>').val("");
        }

    </script>
    <table class="tblContentArea">
        <colgroup>
            <col style="width: 50%" />
            <col style="width: 50%" />
        </colgroup>
        <tr>
            <td style="padding: 5px; vertical-align: top">
                <h4>
                    <%:GetLabel("Order Awal")%></h4>
                <table>
                    <colgroup>
                        <col width="150px" />
                        <col width="150px" />
                        <col />
                    </colgroup>
                    <tr>
                        <td class="tdLabel">
                            <label id="lblTestOrderFrom" class="lblMandatory lblLink">
                                <%:GetLabel("No. Order Awal")%></label>
                        </td>
                        <td colspan="2">
                            <input type="hidden" id="hdnPatientSurgeryID" value="" runat="server" />
                            <input type="hidden" id="hdnTestOrderIDFrom" value="" runat="server" />
                            <asp:TextBox ID="txtTestOrderNoFrom" Width="150px" runat="server" Style="text-align: center" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label id="Label3">
                                <%:GetLabel("Tanggal Order Awal")%></label>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtTestOrderDateFrom" Width="150px" runat="server" ReadOnly="true"
                                Style="text-align: center" />
                        </td>
                    </tr>
                </table>
            </td>
            <td style="padding: 5px; vertical-align: top">
                <h4>
                    <%:GetLabel("Order Tujuan")%></h4>
                <table>
                    <colgroup>
                        <col width="150px" />
                        <col width="150px" />
                        <col />
                    </colgroup>
                    <tr>
                        <td class="tdLabel">
                            <label id="lblTestOrderTo" class="lblMandatory lblLink">
                                <%:GetLabel("No. Order Tujuan")%></label>
                        </td>
                        <td colspan="2">
                            <input type="hidden" id="hdnTestOrderIDTo" value="" runat="server" />
                            <asp:TextBox ID="txtTestOrderNoTo" Width="150px" runat="server" Style="text-align: center" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label id="Label4">
                                <%:GetLabel("Tanggal Order Tujuan")%></label>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtTestOrderDateTo" Width="150px" runat="server" ReadOnly="true"
                                Style="text-align: center" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td style="padding: 5px; vertical-align: top;">
                <h4>
                    <%:GetLabel("Laporan Operasi")%></h4>
                <table class="tblEntryContent" style="width: 100%">
                    <colgroup>
                        <col width="150px" />
                        <col width="150px" />
                        <col width="100px" />
                        <col width="150px" />
                        <col />
                    </colgroup>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("No. Registrasi")%></label>
                        </td>
                        <td colspan="3">
                            <input type="hidden" id="hdnVisitID" value="" runat="server" />
                            <input type="hidden" id="hdnRegistrationID" value="" runat="server" />
                            <asp:TextBox ID="txtRegistrationNo" Width="150px" runat="server" ReadOnly="true" Style="text-align: center" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Pasien")%></label>
                        </td>
                        <td colspan="4">
                            <asp:TextBox ID="txtPatient" Width="550px" runat="server" ReadOnly="true" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Dokter")%></label>
                        </td>
                        <td colspan="4">
                            <asp:TextBox ID="txtParamedicName" Width="550px" runat="server" ReadOnly="true" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Tanggal Laporan")%></label>
                        </td>
                        <td colspan="3">
                            <table border="0" cellpadding="0" cellspacing="0">
                                <colgroup>
                                    <col width="150px" />
                                    <col width="150px" />
                                    <col />
                                </colgroup>
                                <tr>
                                    <td>
                                        <asp:TextBox ID="txtReportDate" Width="120px" runat="server" ReadOnly="true" Style="text-align: center" />
                                    </td>
                                    <td class="tdLabel" style="padding-left: 5px">
                                        <label class="lblNormal">
                                            <%=GetLabel("Jam Laporan")%></label>
                                    </td>
                                    <td style="padding-left: 5px">
                                        <asp:TextBox ID="txtReportTime" Width="80px" CssClass="time" runat="server" Style="text-align: center"
                                            ReadOnly="true" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Tanggal Mulai Operasi")%></label>
                        </td>
                        <td colspan="3">
                            <table border="0" cellpadding="0" cellspacing="0">
                                <colgroup>
                                    <col width="150px" />
                                    <col width="150px" />
                                    <col />
                                </colgroup>
                                <tr>
                                    <td>
                                        <asp:TextBox ID="txtStartDate" Width="120px" runat="server" ReadOnly="true" Style="text-align: center" />
                                    </td>
                                    <td class="tdLabel" style="padding-left: 5px">
                                        <label class="lblNormal">
                                            <%=GetLabel("Jam Mulai Operasi")%></label>
                                    </td>
                                    <td style="padding-left: 5px">
                                        <asp:TextBox ID="txtStartTime" Width="80px" CssClass="time" runat="server" Style="text-align: center"
                                            MaxLength="5" ReadOnly="true" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Tanggal Selesai Operasi")%></label>
                        </td>
                        <td colspan="3">
                            <table border="0" cellpadding="0" cellspacing="0">
                                <colgroup>
                                    <col width="150px" />
                                    <col width="150px" />
                                    <col />
                                </colgroup>
                                <tr>
                                    <td>
                                        <asp:TextBox ID="txtEndDate" Width="120px" runat="server" ReadOnly="true" Style="text-align: center" />
                                    </td>
                                    <td class="tdLabel" style="padding-left: 5px">
                                        <label class="lblNormal">
                                            <%=GetLabel("Jam Selesai Operasi")%></label>
                                    </td>
                                    <td style="padding-left: 5px">
                                        <asp:TextBox ID="txtEndTime" Width="80px" CssClass="time" runat="server" Style="text-align: center"
                                            MaxLength="5" ReadOnly="true" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <%=GetLabel("Lama Operasi") %>
                        </td>
                        <td>
                            <asp:TextBox ID="txtDuration" Width="60px" CssClass="number" runat="server" ReadOnly="true" />
                            menit
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <label class="lblNormal">
                                <%=GetLabel("Operasi Ke-")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtSurgeryNo" Width="50px" runat="server" Style="text-align: right"
                                Enabled="false" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>
