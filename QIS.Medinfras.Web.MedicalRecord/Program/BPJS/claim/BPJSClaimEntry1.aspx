<%@ Page Title="" Language="C#" MasterPageFile="~/libs/MasterPage/PatientPage/MPBasePatientPageList.master"
    AutoEventWireup="true" CodeBehind="BPJSClaimEntry1.aspx.cs" Inherits="QIS.Medinfras.Web.MedicalRecord.Program.BPJSClaimEntry1" %>

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
    <li id="btnProposeClaim" runat="server" crudmode="R">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbprocess.png")%>' alt="" /><div>
            <%=GetLabel("Propose")%></div>
    </li>
    <li id="btnUpdateClaim" runat="server" crudmode="R">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbedit.png")%>' alt="" /><div>
            <%=GetLabel("Edit")%></div>
    </li>
    <li id="btnDeleteClaim" runat="server" crudmode="R">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbdelete.png")%>' alt="" /><div>
            <%=GetLabel("Delete")%></div>
    </li>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhList" runat="server">
    <script type="text/javascript">
        var registrationDateTimeInString = '<%=RegistrationDateTime%>';
        var registrationDateTime = Methods.stringToDateTime(registrationDateTimeInString);
        $(function () {
            $('#<%=btnProposeClaim.ClientID %>').click(function (evt) {
                if (IsValid(evt, 'fsPatientDischarge', 'mpPatientDischarge'))
                    ProsesPengajuanKlaim('propose');

            });
            $('#<%=btnUpdateClaim.ClientID %>').click(function (evt) {
                if (IsValid(evt, 'fsPatientDischarge', 'mpPatientDischarge'))
                    ProsesPengajuanKlaim('edit');
            });
            $('#<%=btnDeleteClaim.ClientID %>').click(function (evt) {
                if (IsValid(evt, 'fsPatientDischarge', 'mpPatientDischarge'))
                    ProsesPengajuanKlaim('delete');
            });
            $('#<%=txtDischargeDate.ClientID %>').change(function () {
                onDischargeDateTimeChange();
            });
            $('#<%=txtDischargeTime.ClientID %>').change(function () {
                onDischargeDateTimeChange();
            });

            onDischargeDateTimeChange();

            InitializeButton();
            registerCollapseExpandHandler();
        });

        function ProsesPengajuanKlaim(mode) {
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
            if ($('#<%=hdnGCBPJSClaimStatus.ClientID %>').val() == Constant.BPJS_CLAIM_STATUS.OPEN) {
                $('#<%=btnProposeClaim.ClientID %>').css('display', '');
                $('#<%=btnUpdateClaim.ClientID %>').css('display', 'none');
                $('#<%=btnDeleteClaim.ClientID %>').css('display', 'none');
            }
            else {
                $('#<%=btnProposeClaim.ClientID %>').css('display', 'none');
                $('#<%=btnUpdateClaim.ClientID %>').css('display', '');
                $('#<%=btnDeleteClaim.ClientID %>').css('display', '');
            }
        }
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
                                    <%=GetLabel("Kelas Perawatan")%></label>
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
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Dirujuk Ke")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtRefferal" runat="server" ReadOnly="true" Width="97%" />
                            </td>
                            <td colspan="3">
                                <asp:TextBox ID="txtRefferalName" runat="server" ReadOnly="true" Width="100%" />
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
                    <h4 class="h4expanded">
                        <%=GetLabel("Parameter Referensi : Lembar Pengajuan Klaim (Mapping kode MEDINFRAS dengan BPJS)")%></h4>
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
                                        <%=GetLabel("Tipe Penjamin")%></label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtKodeTipePenjamin" Width="100%" ReadOnly="true" runat="server"
                                        Style="text-align: left" Text="1" />
                                </td>
                                <td>
                                    <asp:TextBox ID="txtNamaTipePenjamin" Width="100%" ReadOnly="true" runat="server"
                                        Style="text-align: left" Text="JKN" />
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblNormal">
                                        <%=GetLabel("Ruang Rawat")%></label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtRuangRawat" Width="100%" ReadOnly="true" runat="server" Style="text-align: left" />
                                </td>
                                <td>
                                    <asp:TextBox ID="txtNamaRuangRawat" Width="100%" ReadOnly="true" runat="server" Style="text-align: left" />
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblNormal">
                                        <%=GetLabel("Poli")%></label>
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
                                    <label class="lblNormal">
                                        <%=GetLabel("Spesialitik")%></label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtKodeSpesialitik" Width="100%" ReadOnly="true" runat="server"
                                        Style="text-align: left" />
                                </td>
                                <td>
                                    <asp:TextBox ID="txtNamaSpesialitik" Width="100%" ReadOnly="true" runat="server"
                                        Style="text-align: left" />
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblNormal">
                                        <%=GetLabel("DPJP")%></label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtKodeDPJP" Width="100%" ReadOnly="true" runat="server" Style="text-align: left" />
                                </td>
                                <td>
                                    <asp:TextBox ID="txtNamaDPJP" Width="100%" ReadOnly="true" runat="server" Style="text-align: left" />
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblNormal">
                                        <%=GetLabel("Kondisi Pulang")%></label>
                                </td>
                                <td colspan="2">
                                    <dxe:ASPxComboBox ID="cboKondisiPulang" Width="100%" runat="server" ClientInstanceName="cboKondisiPulang" />
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblNormal">
                                        <%=GetLabel("Cara Keluar")%></label>
                                </td>
                                <td colspan="2">
                                    <dxe:ASPxComboBox ID="cboCaraKeluar" Width="100%" runat="server" ClientInstanceName="cboCaraKeluar" />
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblNormal">
                                        <%=GetLabel("Tindak Lanjut")%></label>
                                </td>
                                <td colspan="2">
                                    <dxe:ASPxComboBox ID="cboTindakLanjut" Width="100%" runat="server" ClientInstanceName="cboTindakLanjut" />
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblNormal">
                                        <%=GetLabel("Poli Kontrol")%></label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtKodePoliKontrol" Width="100%" ReadOnly="true" runat="server"
                                        Style="text-align: left" />
                                </td>
                                <td>
                                    <asp:TextBox ID="txtNamaPoliKontrol" Width="100%" ReadOnly="true" runat="server"
                                        Style="text-align: left" />
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblNormal">
                                        <%=GetLabel("Tanggal Kontrol")%></label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtRefferalDate" Width="100%" ReadOnly="true" runat="server" Style="text-align: center" />
                                </td>
                            </tr>
                        </table>
                    </div>
                </td>
            </tr>
        </table>
    </fieldset>
</asp:Content>
