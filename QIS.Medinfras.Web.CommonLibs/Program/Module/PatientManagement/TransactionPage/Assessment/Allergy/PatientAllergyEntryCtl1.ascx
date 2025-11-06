<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PatientAllergyEntryCtl1.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.PatientAllergyEntryCtl1" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<script type="text/javascript" id="dxss_chiefcomplaintentryctl">
    $('#<%=txtAllergenName.ClientID %>').focus();
    setDatePicker('<%=txtObservationDate.ClientID %>');
    $('#<%=txtObservationDate.ClientID %>').datepicker('option', 'maxDate', new Date(getDateNow()));

    function onBeforeSaveRecordEntryPopup(errMessage) {
        var dateToday = $('#<%=hdnDateToday.ClientID %>').val();

        if ($('#<%=txtObservationDate.ClientID %>').val() == '') {
            $('#<%=txtObservationDate.ClientID %>').val(dateToday);
        }

        var dateSelected = $('#<%=txtObservationDate.ClientID %>').val();
        var observationDateInDatePicker = Methods.getDatePickerDate(dateSelected);

        if (new Date(observationDateInDatePicker).toString() !== 'Invalid Date') {
            var from = dateSelected.split("-");
            var f = new Date(from[2], from[1] - 1, from[0]);

            var to = dateToday.split("-");
            var t = new Date(to[2], to[1] - 1, to[0]);

            if (f > t) {
                showToast('Warning', 'Tanggal Log tidak bisa dipilih melewati tanggal hari ini!');
                $('#<%=txtObservationDate.ClientID %>').val(dateToday);
                return false;
            }
            else {
                return true;
            }
        }
        else {
            $('#<%=txtObservationDate.ClientID %>').val(dateToday);
        }
    }
</script>
<div style="height: 300px; overflow-y: scroll;">
    <input type="hidden" runat="server" id="hdnID" value="" />
    <input type="hidden" runat="server" id="hdnIsNutrition" value="" />
    <input type="hidden" runat="server" id="hdnDateToday" value="" />
    <table class="tblContentArea">
        <colgroup>
            <col style="width: 100%" />
        </colgroup>
        <tr>
            <td style="padding:5px;vertical-align:top;">
                <table style="width:460px" class="tblEntryContent">
                <colgroup>
                    <col style="width:100%" />
                </colgroup>
                <tr>
                    <td class="tdLabel">
                        <label class="lblMandatory"><%=GetLabel("Tanggal Log")%></label>
                    </td>
                    <td colspan="5">
                        <asp:TextBox runat="server" ID="txtObservationDate" CssClass="datepicker" Width="130px" />
                    </td>
                </tr>
                <tr>
                    <td class="tdLabel">
                        <label class="lblMandatory"><%=GetLabel("Tipe Alergi")%></label>
                    </td>
                    <td colspan="5">
                        <dxe:ASPxComboBox runat="server" ID="cboAllergenType" ClientInstanceName="cboAllergenType" Width="300px" />
                    </td>
                </tr>
                <tr>
                    <td class="tdLabel">
                        <label class="lblMandatory"><%=GetLabel("Nama Alergi")%></label>
                    </td>
                    <td colspan="5">
                        <asp:TextBox ID="txtAllergenName" Width="300px" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td class="tdLabel">
                        <label class="lblMandatory"><%=GetLabel("Sumber Informasi")%></label>
                    </td>
                    <td colspan = "5">
                        <dxe:ASPxComboBox runat="server" ID="cboFindingSource" ClientInstanceName="cboFindingSource" Width="200px" />
                    </td>
                </tr>
                <tr>
                    <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Alergi Sejak")%></label></td>
                    <td><dxe:ASPxComboBox runat="server" ID="cboYear" ClientInstanceName="cboYear" Width="100%" /></td>
                    <td><dxe:ASPxComboBox runat="server" ID="cboMonth" ClientInstanceName="cboMonth" Width="100%" /></td>
                </tr>
                <tr>
                    <td class="tdLabel">
                        <label class="lblMandatory"><%=GetLabel("Tingkat Alergi")%></label>
                    </td>
                    <td colspan="5">
                        <dxe:ASPxComboBox runat="server" ID="cboSeverity" ClientInstanceName="cboSeverity" Width="300px" />
                    </td>
                </tr>
                <tr>
                    <td class="tdLabel">
                        <label class="lblMandatory"><%=GetLabel("Reaksi Alergi")%></label>
                    </td>
                    <td colspan="5">
                        <asp:TextBox ID="txtReaction" Width="300px" runat="server" />
                    </td>
                </tr>
            </table>
            </td>
        </tr>
    </table>
</div>
