<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="VaccinationCovid19EntryCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.VaccinationCovid19EntryCtl" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<script type="text/javascript" id="dxss_vaccinationcovid19entryctl">
    $('#<%=txtSequenceNo.ClientID %>').focus();
    setDatePicker('<%=txtVaccinationDate.ClientID %>');
    $('#<%=txtVaccinationDate.ClientID %>').datepicker('option', 'maxDate', '0');

    function onBeforeSaveRecordEntryPopup(errMessage) {
        var dateToday = $('#<%=hdnDateToday.ClientID %>').val();

        if ($('#<%=txtVaccinationDate.ClientID %>').val() == '') {
            $('#<%=txtVaccinationDate.ClientID %>').val(dateToday);
        }

        var dateSelected = $('#<%=txtVaccinationDate.ClientID %>').val();
        var vaccinationDateInDatePicker = Methods.getDatePickerDate(dateSelected);

        if (new Date(vaccinationDateInDatePicker).toString() !== 'Invalid Date') {
            var from = dateSelected.split("-");
            var f = new Date(from[2], from[1] - 1, from[0]);

            var to = dateToday.split("-");
            var t = new Date(to[2], to[1] - 1, to[0]);

            if (f > t) {
                showToast('Warning', 'Tanggal Vaksinasi tidak bisa dipilih melewati tanggal hari ini!');
                $('#<%=txtVaccinationDate.ClientID %>').val(dateToday);
                return false;
            }
            else {
                return true;
            }
        }
        else {
            $('#<%=txtVaccinationDate.ClientID %>').val(dateToday);
        }
    }
</script>
<div style="height: 300px">
    <input type="hidden" runat="server" id="hdnID" value="" />
    <input type="hidden" runat="server" id="hdnDateToday" value="" />
    <table class="tblContentArea">
        <colgroup>
            <col style="width: 100%" />
        </colgroup>
        <tr>
            <td style="padding: 5px; vertical-align: top;">
                <table style="width: 460px" class="tblEntryContent">
                    <colgroup>
                        <col style="width: 100%" />
                    </colgroup>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblMandatory">
                                <%=GetLabel("Tanggal Vaksinasi")%></label>
                        </td>
                        <td colspan="5">
                            <asp:TextBox runat="server" ID="txtVaccinationDate" CssClass="datepicker" Width="130px" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblMandatory">
                                <%=GetLabel("Vaksinasi Ke")%></label>
                        </td>
                        <td colspan="5">
                            <asp:TextBox runat="server" ID="txtSequenceNo" Width="60px" Style="text-align: center" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblMandatory">
                                <%=GetLabel("Provider")%></label>
                        </td>
                        <td colspan="5">
                            <asp:TextBox ID="txtProvider" Width="300px" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblMandatory">
                                <%=GetLabel("Jenis Vaksin")%></label>
                        </td>
                        <td colspan="2">
                            <dxe:ASPxComboBox runat="server" ID="cboCovid19Vaccin" ClientInstanceName="cboCovid19Vaccin"
                                Width="150px" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblMandatory">
                                <%=GetLabel("Batch Number")%></label>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtBatchNo" Width="120px" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Catatan")%></label>
                        </td>
                        <td colspan="5">
                            <asp:TextBox ID="txtRemarks" Width="300px" runat="server" TextMode="MultiLine" Rows="2" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</div>
