<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="VaccinationHistoryEntryCtl1.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.VaccinationHistoryEntryCtl1" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="QIS.Medinfras.Web.CustomControl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null" 
    Namespace="QIS.Medinfras.Web.CustomControl" TagPrefix="qis" %>

<script type="text/javascript" id="dxss_vaccinationentryctl1">
    $('#<%=txtSequenceNo.ClientID %>').focus();
    setDatePicker('<%=txtVaccinationDate.ClientID %>');
    $('#<%=txtVaccinationDate.ClientID %>').datepicker('option', 'maxDate', new Date(getDateNow()));

    function onBeforeSaveRecord(errMessage) {
        var dateToday = $('#<%=hdnDateToday.ClientID %>').val();
        var isExternalProvider = $('#<%:chkIsExternalProvider.ClientID %>').is(':checked');

        if ($('#<%=txtVaccinationDate.ClientID %>').val() == '') {
            $('#<%=txtVaccinationDate.ClientID %>').val(dateToday);
        }

        if (isExternalProvider == true) {
            var txtProvider = $('#<%=txtProvider.ClientID %>').val();
            if (txtProvider == '') {
                showToast('Warning', 'Harap Isi Provider');
                return false;
            }
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

    $('#<%=chkIsExternalProvider.ClientID %>').die('change');
    $('#<%=chkIsExternalProvider.ClientID %>').live('change', function () {
        var isChecked = $(this).is(":checked");

        if ($(this).is(':checked')) {
            $('#<%=trProviderInfo.ClientID %>').removeAttr("style");
            $('#<%=trParamedicInfo.ClientID %>').attr("style", "display:none");
            $('#<%=trCurrentVisit.ClientID %>').attr("style", "display:none");
        }
        else {
            $('#<%=trProviderInfo.ClientID %>').attr("style", "display:none");
            $('#<%=trParamedicInfo.ClientID %>').removeAttr("style");
            $('#<%=trCurrentVisit.ClientID %>').removeAttr("style");
        }
    });

    function onLedDrugNameLostFocus(led) {
        var drugID = led.GetValueText();
        if (drugID != '') {
            $('#<%=hdnDrugID.ClientID %>').val(drugID);
            $('#<%=hdnDrugName.ClientID %>').val(led.GetDisplayText());
            var filterExpression = "ItemID = " + drugID;
            Methods.getObject('GetvDrugInfoList', filterExpression, function (result) {
                $('#<%=txtDosingDose.ClientID %>').val('1');
                cboDosingUnit.SetValue(result.GCItemUnit);
                $('#<%=txtDosingDose.ClientID %>').focus();
            });
        }
    }

    function onAfterSaveAddRecordEntryPopup(result) {
        if (typeof onRefreshControl == 'function')
            onRefreshControl();
    }

    function onAfterSaveEditRecordEntryPopup(result) {
        if (typeof onRefreshControl == 'function')
            onRefreshControl();
    }
</script>
<div style="height: 300px">
    <input type="hidden" runat="server" id="hdnPopupID" value="" />
    <input type="hidden" runat="server" id="hdnPopupVisitID" value="" />
    <input type="hidden" runat="server" id="hdnPopupMRN" value="" />
    <input type="hidden" runat="server" id="hdnPopupVaccinationTypeID" value="" />
    <input type="hidden" runat="server" id="hdnDateToday" value="" />
    <input type="hidden" runat="server" id="hdnIsCovid19" value="0" />
    <table class="tblContentArea">
        <colgroup>
            <col style="width: 100%" />
        </colgroup>
        <tr>
            <td style="padding: 5px; vertical-align: top;">
                <table style="width: 100%" class="tblEntryContent">
                    <colgroup>
                        <col style="width: 180px" />
                        <col style="width: 160px" />
                        <col />
                    </colgroup>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Kelompok Vaksinasi")%></label>
                        </td>
                        <td colspan="2">
                            <asp:TextBox runat="server" ID="txtVaccinationType" Width="100%" ReadOnly="true" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblMandatory">
                                <%=GetLabel("Tanggal Vaksinasi")%></label>
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtVaccinationDate" CssClass="datepicker" Width="125px" />
                        </td>
                        <td>
                            <asp:CheckBox ID="chkIsExternalProvider" runat="server" Text=" Dilakukan oleh Fasilitas Kesehatan Lain"/>                        
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblMandatory">
                                <%=GetLabel("Vaksinasi Ke")%></label>
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtSequenceNo" Width="60px" Style="text-align: right" CssClass="number" />
                        </td>
                        <td>
                            <asp:CheckBox ID="chkIsBooster" runat="server" Text = " Booster"/>
                        </td>
                    </tr>
                    <tr id="trCurrentVisit" runat="server">
                        <td class="tdLabel">                       
                        </td>
                        <td colspan="2">
                            <asp:CheckBox ID="chkIsCurrentVisit" runat="server" Text = " Pendataan Tindakan Vaksinasi yang dilakukan pada saat kunjungan" />
                        </td>
                    </tr>
                    <tr id="trParamedicInfo" runat="server">
                        <td class="tdLabel">
                            <label class="lblMandatory">
                                <%=GetLabel("Vaksinator")%></label>
                        </td>
                        <td colspan="2">
                            <dxe:ASPxComboBox ID="cboParamedicID" Width="235px" runat="server" />
                        </td>
                    </tr>
                    <tr id="trProviderInfo" runat="server" style="display:none">
                        <td class="tdLabel">
                            <label class="lblMandatory">
                                <%=GetLabel("Provider")%></label>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtProvider" Width="100%" runat="server" />
                        </td>
                    </tr>
                    <tr id="trItemInfo" runat="server">
                        <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Jenis Vaksin")%></label></td>
                        <td colspan="2">
                            <input type="hidden" value="" id="hdnDrugID" runat="server" />
                            <input type="hidden" value="" id="hdnDrugName" runat="server" />
                            <qis:QISSearchTextBox ID="ledItem" ClientInstanceName="ledItem" runat="server" Width="500px"
                                ValueText="ItemID" FilterExpression="GCItemType = 'X001^002' AND IsDeleted = 0 AND ISNULL(GCItemStatus,'') != 'X181^999'" DisplayText="ItemName1"
                        MethodName="GetvDrugInfoList" >
                                <ClientSideEvents ValueChanged="function(s){ onLedDrugNameLostFocus(s); }" />
                                <Columns>
                                    <qis:QISSearchTextBoxColumn Caption="Nama item" FieldName="ItemName" Description="i.e. Panadol"
                                        Width="400px" />
                                </Columns>
                            </qis:QISSearchTextBox>
                        </td>
                    </tr>
                    <tr id="trCovidVaccination" runat="server" style="display:none">
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
                                <%=GetLabel("Lot/Batch Number")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtBatchNo" Width="150px" runat="server" />
                        </td>
                        <td />
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Rute")%></label>
                        </td>
                        <td colspan="2">
                            <dxe:ASPxComboBox runat="server" ID="cboRoute" ClientInstanceName="cboRoute"
                                Width="150px" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Dosis")%></label></td>
                        <td>
                            <table cellpadding="0" cellspacing="0">
                                <colgroup>
                                    <col style="width:100px"/>
                                    <col style="width:5px"/>
                                    <col style="width:100px"/>
                                </colgroup>
                                <tr>
                                    <td><asp:TextBox ID="txtDosingDose" runat="server" Width="100%" CssClass="number" /></td>
                                    <td>&nbsp;</td>
                                    <td>
                                        <dxe:ASPxComboBox ID="cboDosingUnit" ClientInstanceName="cboDosingUnit" runat="server" Width="100%">
                                            <ClientSideEvents EndCallback="function(s,e){
                                                onCboDosingUnitEndCallback(s);
                                            }" />
                                        </dxe:ASPxComboBox>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Catatan Tambahan")%></label>
                        </td>
                        <td colspan="2" style="vertical-align:top">
                            <asp:TextBox ID="txtRemarks" Width="100%" runat="server" TextMode="MultiLine" Rows="2" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</div>
