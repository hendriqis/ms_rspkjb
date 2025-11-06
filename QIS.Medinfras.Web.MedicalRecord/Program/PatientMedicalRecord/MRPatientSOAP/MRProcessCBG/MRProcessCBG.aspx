<%@ Page Title="" Language="C#" MasterPageFile="~/libs/MasterPage/PatientPage/MPBasePatientPageList.master"
    AutoEventWireup="true" CodeBehind="MRProcessCBG.aspx.cs" Inherits="QIS.Medinfras.Web.MedicalRecord.Program.MRProcessCBG" %>

<%@ Register Src="~/Program/PatientMedicalRecord/MRPatientSOAP/MRPatientSOAPToolbarCtl.ascx"
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
    <li id="btnSaveGrouper" runat="server" crudmode="R">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbset.png")%>' alt="" /><div>
            <%=GetLabel("Save")%></div>
    </li>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhList" runat="server">
    <script type="text/javascript">
        var registrationDateTimeInString = '<%=RegistrationDateTime%>';
        var registrationDateTime = Methods.stringToDateTime(registrationDateTimeInString);
        $(function () {
            $('#<%=btnSaveGrouper.ClientID %>').click(function (evt) {
                if (IsValid(evt, 'fsPatientDischarge', 'mpPatientDischarge'))
                    onCustomButtonClick('process');
            });
            $('#<%=txtDischargeDate.ClientID %>').change(function () {
                onDischargeDateTimeChange();
            });
            $('#<%=txtDischargeTime.ClientID %>').change(function () {
                onDischargeDateTimeChange();
            });

            onDischargeDateTimeChange();
        });

        $("#btnOnlineGrouper").on("click", function (e) {
            alert('Clicked!');
            e.preventDefault();
            if ($('#<%=txtNomorSEP.ClientID %>').val() == '') {
                var jnsrawat = $('#<%=hdnJnsRawat.ClientID %>').val();
                var klsrawat = $('#<%=hdnKlsRawat.ClientID %>').val();
                var diagnosaCode = $('#<%=hdnDiagnosis.ClientID %>').val();
                var procedureCode = $('#<%=hdnProcedures.ClientID %>').val();

                alert('Clicked!');
                Methods.GetINACBGGrouperTariff(jnsrawat, klsrawat, diagnosaCode, procedureCode, function (result) {
                    try {
                        var obj = jQuery.parseJSON(result);
                        $('#<%=txtGrouperCode.ClientID %>').val(obj.response.GrouperCode);
                        $('#<%=txtGrouperName.ClientID %>').val(obj.response.GrouperName);
                        $('#<%=txtGrouperTariff.ClientID %>').val(obj.response.Tariff);
                    } catch (err) {
                        $('#<%=txtGrouperCode.ClientID %>').val('');
                        $('#<%=txtGrouperName.ClientID %>').val('');
                        $('#<%=txtGrouperTariff.ClientID %>').val('0');
                    }
                });
            }
            else {
                //cbpPopupProcess.PerformCallback('save');
            }
        });

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
            var tariff = parseInt($('#<%=txtGrouperTariff.ClientID %>').attr('hiddenVal'));
            var tariffSA = parseInt($('#<%=txtTariffSA.ClientID %>').attr('hiddenVal'));
            var tariffSP = parseInt($('#<%=txtTariffSP.ClientID %>').attr('hiddenVal'));
            var tariffSD = parseInt($('#<%=txtTariffSD.ClientID %>').attr('hiddenVal'));
            var tariffSI = parseInt($('#<%=txtTariffSI.ClientID %>').attr('hiddenVal'));
            var tariffSR = parseInt($('#<%=txtTariffSR.ClientID %>').attr('hiddenVal'));
            var total = tariff + tariffSA + tariffSP + tariffSD + tariffSI + tariffSR;
            $('#<%=txtTotalTariff.ClientID %>').val(total).trigger('changeValue');
        }

    </script>
    <input type="hidden" id="hdnLOSInDay" runat="server" />
    <input type="hidden" id="hdnLOSInHour" runat="server" />
    <input type="hidden" id="hdnLOSInMinute" runat="server" />
    <input type="hidden" id="hdnJnsRawat" runat="server" />
    <input type="hidden" id="hdnKlsRawat" runat="server" />
    <input type="hidden" id="hdnMainDiagnoseID" runat="server" />
    <input type="hidden" id="hdnDiagnosis" runat="server" />
    <input type="hidden" id="hdnProcedures" runat="server" />
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
                            <col style="width: 125px" />
                            <col style="width: 150px" />
                            <col style="width: 125px" />
                            <col style="width: 150px" />
                            <col />
                        </colgroup>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblMandatory">
                                    <%=GetLabel("Model Pembayaran")%></label>
                            </td>
                            <td>
                                <dxe:ASPxComboBox ID="cboModelBayar" Width="100%" runat="server" />
                            </td>
                        </tr>
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
                                    <%=GetLabel("Cara Pulang")%></label>
                            </td>
                            <td>
                                <dxe:ASPxComboBox ID="cboDischargeCondition" Width="100%" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Surat Rujukan")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtSuratRujukan" runat="server" ReadOnly="true" Width="97%" />
                            </td>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("ADL")%></label>
                            </td>
                            <td>
                                <%--<dxe:ASPxComboBox ID="cboADL" Width="100%" runat="server" />--%>
                                <asp:TextBox ID="txtADLScore" runat="server" Width="100px" />
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
                                <asp:TextBox ID="txtMainDiagnose" runat="server" ReadOnly="true" Width="97%" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel" style="vertical-align: top">
                                <label class="lblNormal">
                                    <%=GetLabel("Diagnosa Tambahan")%></label>
                            </td>
                            <td colspan="4" style="vertical-align: top">
                                <asp:TextBox ID="txtComplication" runat="server" ReadOnly="true" Width="97%" TextMode="MultiLine"
                                    Height="100px" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel" style="vertical-align: top">
                                <label class="lblNormal">
                                    <%=GetLabel("Tindakan")%></label>
                            </td>
                            <td colspan="4" style="vertical-align: top">
                                <asp:TextBox ID="txtProcedures" runat="server" ReadOnly="true" Width="97%" TextMode="MultiLine"
                                    Height="100px" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Tariff RS")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtTariffRS" Width="100%" ReadOnly="true" runat="server" CssClass="txtCurrency" />
                            </td>
                        </tr>
                    </table>
                </td>
                <td />
                <td style="vertical-align: top">
                    <table border="0" cellpadding="0" cellspacing="1" width="100%">
                        <colgroup>
                            <col style="width: 125px" />
                            <col style="width: 150px" />
                            <col style="width: 125px" />
                            <col style="width: 150px" />
                            <col />
                        </colgroup>
                        <tr>
                            <td>
                                <label class="lblNormal" style="font-weight: bold;">
                                    <%=GetLabel("Special CMG :")%></label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label class="lblLink" style="padding-left: 15px">
                                    <%=GetLabel("Sub Acute/Chronic")%></label>
                            </td>
                            <td colspan="4">
                                <table style="width: 100%" cellpadding="0" cellspacing="0">
                                    <colgroup>
                                        <col style="width: 110px" />
                                        <col style="width: 3px" />
                                        <col />
                                    </colgroup>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtSACode" Width="100%" runat="server" />
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtSAName" Width="100%" ReadOnly="true" runat="server" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label class="lblLink" style="padding-left: 15px">
                                    <%=GetLabel("Special Procedure")%></label>
                            </td>
                            <td colspan="4">
                                <table style="width: 100%" cellpadding="0" cellspacing="0">
                                    <colgroup>
                                        <col style="width: 110px" />
                                        <col style="width: 3px" />
                                        <col />
                                    </colgroup>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtProcedureID" Width="100%" runat="server" />
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtProcedureName" Width="100%" ReadOnly="true" runat="server" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label class="lblLink" style="padding-left: 15px">
                                    <%=GetLabel("Special Drugs")%></label>
                            </td>
                            <td colspan="4">
                                <table style="width: 100%" cellpadding="0" cellspacing="0">
                                    <colgroup>
                                        <col style="width: 110px" />
                                        <col style="width: 3px" />
                                        <col />
                                    </colgroup>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtItemCode" Width="100%" runat="server" />
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtItemName" Width="100%" ReadOnly="true" runat="server" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label class="lblLink" style="padding-left: 15px">
                                    <%=GetLabel("Special Investigation")%></label>
                            </td>
                            <td colspan="4">
                                <table style="width: 100%" cellpadding="0" cellspacing="0">
                                    <colgroup>
                                        <col style="width: 110px" />
                                        <col style="width: 3px" />
                                        <col />
                                    </colgroup>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtSICode1" Width="100%" runat="server" />
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtSIName1" Width="100%" ReadOnly="true" runat="server" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label class="lblLink" style="padding-left: 15px">
                                    <%=GetLabel("Special Prosthesis")%></label>
                            </td>
                            <td colspan="4">
                                <table style="width: 100%" cellpadding="0" cellspacing="0">
                                    <colgroup>
                                        <col style="width: 110px" />
                                        <col style="width: 3px" />
                                        <col />
                                    </colgroup>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtProsthesisCode" Width="100%" runat="server" />
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtProsthesisName" Width="100%" ReadOnly="true" runat="server" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="5">
                                <h3>
                                    <label class="lblNormal" style="font-weight: bold; text-decoration: underline">
                                        <%=GetLabel("Proses CBG Grouper :")%></label>
                                </h3>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label class="lblMandatory">
                                    <%=GetLabel("Kode INACBG")%></label>
                            </td>
                            <td colspan="4">
                                <table style="width: 100%" cellpadding="0" cellspacing="0">
                                    <colgroup>
                                        <col style="width: 110px" />
                                        <col style="width: 3px" />
                                        <col />
                                    </colgroup>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtGrouperCode" Width="100%" ReadOnly="true" runat="server" />
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtGrouperName" Width="100%" ReadOnly="true" runat="server" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Tipe CBG")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtCBGType" runat="server" ReadOnly="true" Width="100%" />
                            </td>
                            <td style="padding-left:10px">
                                <input type="button" id="btnOnlineGrouper" value='<%= GetLabel("Online Grouper")%>' />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Tariff")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtGrouperTariff" runat="server" ReadOnly="true" Width="100%" CssClass="txtCurrency" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label class="lblNormal">
                                    <%=GetLabel("Sub Acute/Chronic")%></label>
                            </td>
                            <td>
                                <table style="width: 100%" cellpadding="0" cellspacing="0">
                                    <colgroup>
                                        <col style="width: 70px" />
                                        <col style="width: 3px" />
                                        <col />
                                    </colgroup>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtSACode2" Width="100%" runat="server" ReadOnly="true" Text="None" />
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtTariffSA" Width="100%" ReadOnly="true" runat="server" CssClass="txtCurrency" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label class="lblNormal">
                                    <%=GetLabel("Special Procedures")%></label>
                            </td>
                            <td>
                                <table style="width: 100%" cellpadding="0" cellspacing="0">
                                    <colgroup>
                                        <col style="width: 70px" />
                                        <col style="width: 3px" />
                                        <col />
                                    </colgroup>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtSPCode" Width="100%" runat="server" ReadOnly="true" Text="None" />
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtTariffSP" Width="100%" ReadOnly="true" runat="server" CssClass="txtCurrency" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label class="lblNormal">
                                    <%=GetLabel("Special Prosthesis")%></label>
                            </td>
                            <td>
                                <table style="width: 100%" cellpadding="0" cellspacing="0">
                                    <colgroup>
                                        <col style="width: 70px" />
                                        <col style="width: 3px" />
                                        <col />
                                    </colgroup>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtSRCode2" Width="100%" runat="server" ReadOnly="true" Text="None" />
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtTariffSR" Width="100%" ReadOnly="true" runat="server" CssClass="txtCurrency" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label class="lblNormal">
                                    <%=GetLabel("Special Investigation")%></label>
                            </td>
                            <td>
                                <table style="width: 100%" cellpadding="0" cellspacing="0">
                                    <colgroup>
                                        <col style="width: 70px" />
                                        <col style="width: 3px" />
                                        <col />
                                    </colgroup>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtSICode2" Width="100%" runat="server" ReadOnly="true" Text="None" />
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtTariffSI" Width="100%" ReadOnly="true" runat="server" CssClass="txtCurrency" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label class="lblNormal">
                                    <%=GetLabel("Special Drug")%></label>
                            </td>
                            <td>
                                <table style="width: 100%" cellpadding="0" cellspacing="0">
                                    <colgroup>
                                        <col style="width: 70px" />
                                        <col style="width: 3px" />
                                        <col />
                                    </colgroup>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtSDCode2" Width="100%" runat="server" ReadOnly="true" Text="None" />
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtTariffSD" Width="100%" ReadOnly="true" runat="server" CssClass="txtCurrency" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <br />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Total Tariff")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtTotalTariff" runat="server" ReadOnly="true" Width="100%" CssClass="txtCurrency" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </fieldset>
</asp:Content>
