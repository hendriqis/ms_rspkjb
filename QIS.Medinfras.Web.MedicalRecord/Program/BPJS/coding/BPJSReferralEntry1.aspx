<%@ Page Title="" Language="C#" MasterPageFile="~/libs/MasterPage/PatientPage/MPBasePatientPageList.master"
    AutoEventWireup="true" CodeBehind="BPJSReferralEntry1.aspx.cs" Inherits="QIS.Medinfras.Web.MedicalRecord.Program.BPJSReferralEntry1" %>

<%@ Register Src="~/Program/BPJS/BPJSNavigationPaneCtl.ascx"
    TagName="ToolbarCtl" TagPrefix="uc1" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="QIS.Medinfras.Web.CustomControl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"
    Namespace="QIS.Medinfras.Web.CustomControl" TagPrefix="qis" %>
<asp:Content ID="Content3" ContentPlaceHolderID="plhMPPatientPageNavigationPane"
    runat="server">
    <uc1:ToolbarCtl ID="ctlToolbar" runat="server" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="plhCustomButtonToolbar" runat="server">
    <li id="btnProcessRefferal" runat="server" crudmode="R">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbprocess.png")%>' alt="" /><div>
            <%=GetLabel("Propose")%></div>
    </li>
    <li id="btnUpdateRefferal" runat="server" crudmode="R">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbedit.png")%>' alt="" /><div>
            <%=GetLabel("Edit")%></div>
    </li>
    <li id="btnDeleteReferral" runat="server" crudmode="R">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbdelete.png")%>' alt="" /><div>
            <%=GetLabel("Delete")%></div>
    </li>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhList" runat="server">
    <script type="text/javascript">
        var registrationDateTimeInString = '<%=RegistrationDateTime%>';
        var registrationDateTime = Methods.stringToDateTime(registrationDateTimeInString);
        $(function () {
            $('#<%=btnProcessRefferal.ClientID %>').click(function (evt) {
                if (IsValid(evt, 'fsPatientDischarge', 'mpPatientDischarge'))
                    ProsesRujukan('propose');

            });
            $('#<%=btnUpdateRefferal.ClientID %>').click(function (evt) {
                if (IsValid(evt, 'fsPatientDischarge', 'mpPatientDischarge'))
                    ProsesRujukan('edit');
            });
            $('#<%=btnDeleteReferral.ClientID %>').click(function (evt) {
                if (IsValid(evt, 'fsPatientDischarge', 'mpPatientDischarge'))
                    ProsesRujukan('delete');
            });
            $('#<%=txtDischargeDate.ClientID %>').change(function () {
                onDischargeDateTimeChange();
            });
            $('#<%=txtDischargeTime.ClientID %>').change(function () {
                onDischargeDateTimeChange();
            });

            onDischargeDateTimeChange();

            InitializeButton();

            setDatePicker('<%=txtRefferalDate.ClientID %>');
            $('#<%:txtRefferalDate.ClientID %>').datepicker('option', 'minDate', '0');
        });

        function ProsesRujukan(mode) {
            onCustomButtonClick(mode);
        }

        function dateDiff(date1, date2) {
            var diff = date2 - date1;
            return isNaN(diff) ? NaN : {
                diff: diff,
                ms: Math.floor(diff % 1000),
                s: Math.floor(diff / 1000 % 60),
                m: Math.floor(diff / 60000 % 60),
                h: Math.floor(diff / 3600000 % 24),
                d: Math.floor(diff / 86400000)
            };
        }

        function onDischargeDateTimeChange() {
            var dischargeDate = Methods.getDatePickerDate($('#<%=txtDischargeDate.ClientID %>').val());
            var dischargeTime = $('#<%=txtDischargeTime.ClientID %>').val();
            var dischargeDateTimeInString = Methods.dateToString(dischargeDate) + dischargeTime.replace(':', '');
            var diff = dateDiff(registrationDateTime, Methods.stringToDateTime(dischargeDateTimeInString));
            $('#<%=hdnLOSInDay.ClientID %>').val(diff.d);
            $('#<%=hdnLOSInHour.ClientID %>').val(diff.h);
            $('#<%=hdnLOSInMinute.ClientID %>').val(diff.m);
            $('#<%=txtLengthOfVisit.ClientID %>').val(diff.d + 'dd ' + diff.h + 'hh ' + diff.m + 'mm');
        }

        //        function onAfterCustomClickSuccess(type) {
        //            exitPatientPage();
        //        }

        $('.txtGrouperTariff').change(function () {
            $(this).blur();
            calculateTotal();
        });

        function calculateTotal() {
        }

        function InitializeButton() {
            if ($('#<%=txtRefferalNo.ClientID %>').val() == '') {
                $('#<%=btnProcessRefferal.ClientID %>').css('display', '');
                $('#<%=btnUpdateRefferal.ClientID %>').css('display', 'none');
                $('#<%=btnDeleteReferral.ClientID %>').css('display', 'none');
            }
            else {
                $('#<%=btnProcessRefferal.ClientID %>').css('display', 'none');
                $('#<%=btnUpdateRefferal.ClientID %>').css('display', '');
                $('#<%=btnDeleteReferral.ClientID %>').css('display', '');
            }
        }

        //#region Diagnose
        $('#lblDiagnose.lblLink').live('click', function () {
            openSearchDialog('diagnose', "IsDeleted = 0", function (value) {
                $('#<%=txtDiagnoseCode.ClientID %>').val(value);
                onTxtDiagnoseCodeChanged(value);
            });
        });

        $('#<%=txtDiagnoseCode.ClientID %>').live('change', function () {
            onTxtDiagnoseCodeChanged($(this).val());
        });

        function onTxtDiagnoseCodeChanged(value) {
            var filterExpression = "DiagnoseID = '" + value + "' AND IsDeleted = 0";
            Methods.getObject('GetDiagnoseList', filterExpression, function (result) {
                if (result != null) {
                    $('#<%=txtDiagnoseName.ClientID %>').val(result.DiagnoseName);
                    $('#<%=hdnBPJSDiagnoseCodeCtl.ClientID %>').val(result.INACBGLabel);
                }
                else {
                    $('#<%=txtDiagnoseCode.ClientID %>').val('');
                    $('#<%=txtDiagnoseName.ClientID %>').val('');
                    $('#<%=hdnBPJSDiagnoseCodeCtl.ClientID %>').val('');
                }
            });
        }
        //#endregion

        //#region Poli
        $('#lblPoli.lblLink').live('click', function () {
            openSearchDialog('bpjsreference', "GCBPJSObjectType = 'X294^02'", function (value) {
                $('#<%=txtKodePoli.ClientID %>').val(value);
                onTxtKodePoliChanged(value);
            });
        });

        $('#<%=txtKodePoli.ClientID %>').live('change', function () {
            onTxtKodeChanged($(this).val());
        });

        function onTxtKodePoliChanged(value) {
            var filterExpression = "GCBPJSObjectType = 'X294^02' AND BPJSCode = '" + value + "'";
            Methods.getObject('GetBPJSReferenceList', filterExpression, function (result) {
                if (result != null) {
                    $('#<%=txtNamaPoli.ClientID %>').val(result.BPJSName);
                }
                else {
                    $('#<%=txtNamaPoli.ClientID %>').val('');
                }
            });
        }
        //#endregion
    </script>
    <input type="hidden" id="hdnLOSInDay" runat="server" />
    <input type="hidden" id="hdnLOSInHour" runat="server" />
    <input type="hidden" id="hdnLOSInMinute" runat="server" />
    <input type="hidden" id="hdnJnsRawat" runat="server" />
    <input type="hidden" id="hdnKlsRawat" runat="server" />
    <input type="hidden" id="hdnMainDiagnoseID" runat="server" />
    <input type="hidden" id="hdnMainDiagnoseCode" runat="server" />
    <input type="hidden" id="hdnDiagnosis" runat="server" />
    <input type="hidden" id="hdnProcedures" runat="server" />
    <input type="hidden" id="hdnDepartmentID" runat="server" />
    <input type="hidden" id="hdnGCBPJSClaimStatus" runat="server" />
    <input type="hidden" id="hdnBPJSDiagnoseCodeCtl" value="" runat="server" />
    <fieldset id="fsPatientDischarge">
        <table class="tblEntryContent" style="width: 100%">
            <colgroup>
                <col style="width: 49%" />
                <col style="width: 2%" />
                <col style="width: 49%" />
            </colgroup>
            <tr>
                <td style="vertical-align: top">
                    <table border="0" cellpadding="0" cellspacing="1" width="100%">
                        <colgroup>
                            <col style="width: 155px" />
                            <col style="width: 150px" />
                            <col style="width: 125px" />
                            <col style="width: 150px" />
                            <col />
                        </colgroup>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Nomor Peserta")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtNoPeserta" runat="server" ReadOnly="true" Width="97%" />
                            </td>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Nomor SEP")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtNomorSEP" runat="server" ReadOnly="true" Width="97%" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Jenis Perawatan")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtJenisRawat" runat="server" ReadOnly="true" Width="97%" />
                            </td>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Hak Kelas")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtKelasTanggungan" runat="server" ReadOnly="true" Width="97%" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Tanggal Masuk")%></label>
                            </td>
                            <td>
                                <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                    <colgroup>
                                        <col />
                                        <col width="60px" />
                                    </colgroup>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtVisitDate" Width="100%" ReadOnly="true" runat="server" Style="text-align: center" />
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtVisitTime" Width="60px" ReadOnly="true" runat="server" Style="text-align: center" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Tanggal Keluar")%></label>
                            </td>
                            <td>
                                <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                    <colgroup>
                                        <col />
                                        <col width="60px" />
                                    </colgroup>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtDischargeDate" Width="100%" ReadOnly="true" runat="server" Style="text-align: center" />
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtDischargeTime" Width="60px" ReadOnly="true" runat="server" Style="text-align: center" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td>
                                <asp:TextBox ID="txtLengthOfVisit" ReadOnly="true" Width="100%" runat="server" Visible="false" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Lama Kunjungan (LOS)")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtLOS" ReadOnly="true" Width="105px" runat="server" Style="text-align: right" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblMandatory">
                                    <%=GetLabel("Kondisi Pulang")%></label>
                            </td>
                            <td colspan="2">
                                <dxe:ASPxComboBox ID="cboPatientOutcome" Width="100%" runat="server" ReadOnly="true" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblMandatory">
                                    <%=GetLabel("Cara Keluar")%></label>
                            </td>
                            <td colspan="2">
                                <dxe:ASPxComboBox ID="cboDischargeRoutine" Width="100%" runat="server" ReadOnly="true" />
                            </td>
                        </tr>
                        <tr>
                            <td />
                            <td />
                            <td />
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Diagnosa Utama")%></label>
                            </td>
                            <td colspan="4">
                                <asp:TextBox ID="txtMainDiagnose" runat="server" ReadOnly="true" Width="100%" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel" style="vertical-align: top">
                                <label class="lblNormal">
                                    <%=GetLabel("Diagnosa Tambahan")%></label>
                            </td>
                            <td colspan="4" style="vertical-align: top">
                                <asp:TextBox ID="txtComplication" runat="server" ReadOnly="true" Width="100%" TextMode="MultiLine"
                                    Height="100px" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel" style="vertical-align: top">
                                <label class="lblNormal">
                                    <%=GetLabel("Tindakan")%></label>
                            </td>
                            <td colspan="4" style="vertical-align: top">
                                <asp:TextBox ID="txtProcedures" runat="server" ReadOnly="true" Width="100%" TextMode="MultiLine"
                                    Height="100px" />
                            </td>
                        </tr>
                        <tr style="display: none">
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Tariff Rumah Sakit")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtTariffRS" Width="100%" ReadOnly="true" runat="server" CssClass="txtCurrency" />
                            </td>
                        </tr>
                    </table>
                </td>
                <td />
                <td style="vertical-align: top">
                    <div class="containerTblEntryContent">
                        <table border="0" cellpadding="1" cellspacing="0" width="100%" style="margin-top: 5px">
                            <colgroup>
                                <col width="150px" />
                                <col width="80px" />
                                <col />
                            </colgroup>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblNormal">
                                        <%=GetLabel("No. Rujukan")%></label>
                                </td>
                                <td colspan="2">
                                    <asp:TextBox ID="txtRefferalNo" Width="200px" ReadOnly="true" runat="server"
                                        Style="text-align: left" />
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblNormal">
                                        <%=GetLabel("Tanggal Rujukan")%></label>
                                </td>
                                <td colspan="2">
                                    <asp:TextBox ID="txtRefferalDate" Width="120px" runat="server" Style="text-align: center" />
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblNormal">
                                        <%=GetLabel("Jenis Pelayanan")%></label>
                                </td>
                                <td colspan="2">
                                    <dxe:ASPxComboBox ID="cboJenisPelayanan" Width="200px" runat="server" ClientInstanceName="cboJenisPelayanan" />
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblNormal">
                                        <%=GetLabel("Tipe Rujukan")%></label>
                                </td>
                                <td colspan="2">
                                    <dxe:ASPxComboBox ID="cboRefferalType" Width="200px" runat="server" ClientInstanceName="cboCaraKeluar" />
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblNormal">
                                        <%=GetLabel("Dirujuk Ke")%></label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtRefferal" runat="server" ReadOnly="true" Width="100%" />
                                </td>
                                <td>
                                    <asp:TextBox ID="txtRefferalName" runat="server" ReadOnly="true" Width="100%" />
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblLink" id="lblPoli">
                                        <%=GetLabel("Poli Rujukan")%></label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtKodePoli" Width="100%" ReadOnly="true" runat="server" Style="text-align: left" />
                                </td>
                                <td>
                                    <asp:TextBox ID="txtNamaPoli" Width="100%" ReadOnly="true" runat="server" Style="text-align: left" />
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblLink"  id="lblDiagnose">
                                        <%=GetLabel("Diagnosa Rujukan")%></label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtDiagnoseCode" Width="100%" ReadOnly="true" runat="server" Style="text-align: left" />
                                </td>
                                <td>
                                    <asp:TextBox ID="txtDiagnoseName" Width="100%" ReadOnly="true" runat="server" Style="text-align: left" />
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel" style="vertical-align:top">
                                    <label class="lblNormal">
                                        <%=GetLabel("Catatan Rujukan")%></label>
                                </td>
                                <td colspan="2">
                                    <asp:TextBox ID="txtRemarks" Height="100px" Width="100%" runat="server" TextMode="MultiLine" />
                                </td>
                            </tr>
                        </table>
                    </div>
                </td>
            </tr>
        </table>
    </fieldset>
</asp:Content>
