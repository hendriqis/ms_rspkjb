<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ProgramReportEntryCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.ProgramReportEntryCtl" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPopupControl" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="QIS.Medinfras.Web.CustomControl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"
    Namespace="QIS.Medinfras.Web.CustomControl" TagPrefix="qis" %>
<script type="text/javascript" id="dxss_ProgramReportEntryCtl1">
    $(function () {
        setDatePicker('<%=txtReportDate.ClientID %>');
        setDatePicker('<%=txtStartDate.ClientID %>');
        setDatePicker('<%=txtEndDate.ClientID %>');

        $('#<%=txtReportDate.ClientID %>').datepicker('option', 'maxDate', new Date(getDateNow()));

        //#region Left Navigation Panel
        $('#leftPageNavPanel ul li').click(function () {
            $('#leftPageNavPanel ul li.selected').removeClass('selected');
            $(this).addClass('selected');
            var contentID = $(this).attr('contentID');

            showContent(contentID);
        });

        function showContent(contentID) {
            var i, x, tablinks;
            x = document.getElementsByClassName("divPageNavPanelContent");
            for (i = 0; i < x.length; i++) {
                x[i].style.display = "none";
            }
            document.getElementById(contentID).style.display = "block";
        }
        //#endregion

        $('#<%=txtEndDate.ClientID %>').change(function () {
            CalculateDuration();
        });
        $('#<%=txtEndDate.ClientID %>').change(function () {
            CalculateDuration();
        });

        $('#<%=txtDuration.ClientID %>').change(function () {
            if ($('#<%=txtDuration.ClientID %>').val() > 0) {
                CalculateEndDate($('#<%=txtDuration.ClientID %>').val());
            } else {
                displayErrorMessageBox('Perhitungan Lama Program', "Lama Program harus lebih besar dari 0");
            }
        });

        $('#leftPageNavPanel ul li').first().click();

        $('#lblPhysicianNoteID').removeClass('lblLink');

    });

    function validateTime(timeValue) {
        var result = true;
        if (timeValue == "" || timeValue.indexOf(":") < 0 || timeValue.length != 5) {
            result = false;
        }
        else {
            var sHours = timeValue.split(':')[0];
            var sMinutes = timeValue.split(':')[1];

            if (sHours == "" || isNaN(sHours) || parseInt(sHours) > 23) {
                result = false;
            }
            else if (parseInt(sHours) == 0)
                sHours = "00";
            else if (sHours < 10)
                sHours = "0" + sHours;

            if (sMinutes == "" || isNaN(sMinutes) || parseInt(sMinutes) > 59) {
                result = false;
            }
            else if (parseInt(sMinutes) == 0)
                sMinutes = "00";
            else if (sMinutes < 10)
                sMinutes = "0" + sMinutes;
        }
        return result;
    }

    function CalculateDuration() {
        var startDateText = $('#<%=txtStartDate.ClientID %>').val();
        var startTimeText = "00:00";

        var endDateText = $('#<%=txtEndDate.ClientID  %>').val();
        var endTimeText = "00:00";

        var startDate = Methods.getDatePickerDate(startDateText);
        var endDate = Methods.getDatePickerDate(endDateText);

        var dateDiff = Methods.calculateDateDifference(startDate, endDate);
        $h1 = parseInt(endTimeText.substring(0, 2), 10);
        $h2 = parseInt(startTimeText.substring(0, 2), 10);

        if (dateDiff.days > 0)
            $h1 = dateDiff.days + $h1;
        $h = $h1 - $h2;

        var duration = $h;

        $('#<%=txtDuration.ClientID %>').val(duration);
    }

    function CalculateEndDate(duration) {
        var startDateText = $('#<%=txtStartDate.ClientID %>').val();
        var startTimeText = "00:00";

        var startDate = Methods.convertToDateTime(startDateText, startTimeText);
        var endDate = Methods.calculateEndDate(startDateText, startTimeText, duration);

        $('#<%=txtEndDate.ClientID %>').val(Methods.dateToDatePickerFormat(endDate));
    }

    function onBeforeSaveRecord(errMessage) {
        var resultFinal = true;
        return resultFinal;
    }

    function onAfterSaveAddRecordEntryPopup(result) {
        if (typeof onRefreshReportGrid == 'function')
            onRefreshReportGrid();
    }

    function onAfterSaveEditRecordEntryPopup(result) {
        if (typeof onRefreshReportGrid == 'function')
            onRefreshReportGrid();
    }
</script>

<div>
    <input type="hidden" runat="server" id="hdnID" value="" />
    <input type="hidden" runat="server" id="hdnProgramID" value="" />
    <input type="hidden" runat="server" id="hdnVisitDepartmentID" value="" />
    <input type="hidden" runat="server" id="hdnPopupVisitID" value="" />
    <input type="hidden" runat="server" id="hdnPopupMRN" value="" />
    <input type="hidden" runat="server" id="hdnPopupMedicalNo" value="" />
    <input type="hidden" runat="server" id="hdnPopupTestOrderID" value="" />
    <input type="hidden" runat="server" id="hdnPopupParamedicID" value="" />
    <input type="hidden" runat="server" id="hdnPopupParamedicTeamID" value="" />
    <input type="hidden" runat="server" id="hdnPopupPatientName" value="" />
    <input type="hidden" runat="server" id="hdnTotalFraction" value="" />
    <input type="hidden" runat="server" id="hdnIsAllowFreeTextMode" value="0" />
    <input type="hidden" value="1" id="hdnParamedicTeamProcessMode" runat="server" />
    <input type="hidden" runat="server" id="hdnOrderDtParamedicTeamID" value="" />
    <table class="tblContentArea">
        <colgroup>
            <col style="width: 20%" />
            <col style="width: 80%" />
        </colgroup>
        <tr>
            <td style="vertical-align: top">
                <div id="leftPageNavPanel" class="w3-border">
                    <ul>
                        <li contentid="divPage1" title="Informasi Program" class="w3-hover-red">Informasi Program</li>
                        <li contentid="divPage2" title="Pemeriksaan Klinis Setelah Radiasi" class="w3-hover-red">Pemeriksaan Setelah Radiasi</li>
                        <li contentid="divPage3" title="Toksisitas" class="w3-hover-red">Toksisitas</li>
                        <li contentid="divPage4" title="Rencana Tindak Lanjut" class="w3-hover-red">Rencana Tindak Lanjut</li>
                    </ul>
                </div>
            </td>
            <td style="vertical-align: top;">
                <div id="divPage1" class="w3-border divPageNavPanelContent w3-animate-left" style="display: none">
                    <table class="tblEntryContent" style="width: 100%">
                        <colgroup>
                            <col style="width: 170px" />
                            <col style="width: 150px" />
                            <col />
                        </colgroup>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblMandatory">
                                    <%=GetLabel("Tanggal ")%>
                                    -
                                    <%=GetLabel("Jam Laporan")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtReportDate" Width="120px" CssClass="datepicker" runat="server" />
                            </td>
                            <td>
                                <asp:TextBox ID="txtReportTime" Width="80px" CssClass="time" runat="server" Style="text-align: center" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblMandatory">
                                    <%=GetLabel("Pembuat Laporan")%></label>
                            </td>
                            <td colspan="2">
                                <dxe:ASPxComboBox ID="cboParamedicID" Width="100%" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblMandatory">
                                    <%=GetLabel("Tanggal Mulai")%></label>
                            </td>
                            <td colspan="2">
                                <asp:TextBox ID="txtStartDate" Width="120px" CssClass="datepicker" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Tanggal Akhir")%></label>
                            </td>
                            <td colspan="2">
                                <asp:TextBox ID="txtEndDate" Width="120px" CssClass="datepicker" runat="server" />
                            </td>
                        </tr> 
                        <tr>
                            <td class="tdLabel">
                                <label class="lblMandatory">
                                    <%=GetLabel("Lama Program") %>
                                </label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtDuration" Width="60px" CssClass="number" runat="server" />
                                hari
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel" style="vertical-align:top">
                                <label class="lblNormal">
                                    <%=GetLabel("Total Fraksi")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtFractionNo" Width="60px" CssClass="number" runat="server"/>
                            </td>
                            <td class="tdLabel" style="vertical-align:top">
                                <label class="lblNormal">
                                    <%=GetLabel("Total Dosis")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtTotalDosage" Width="60px" CssClass="number" runat="server"/> Gy
                            </td>
                        </tr> 
                        <tr>
                            <td class="tdLabel" valign="top" style="padding-top: 5px;">
                                <label class="lblNormal">
                                    <%=GetLabel("Informasi Klinis") %></label>
                            </td>
                            <td colspan="3">
                                <asp:TextBox ID="txtMedicalSummary" runat="server" Width="99%" TextMode="Multiline" Height="150px" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel" valign="top" style="padding-top: 5px;">
                                <label class="lblNormal">
                                    <%=GetLabel("Ringkasan Program Radiasi") %></label>
                            </td>
                            <td colspan="3">
                                <asp:TextBox ID="txtProgramSummary" runat="server" Width="99%" TextMode="Multiline" Height="150px" />
                            </td>
                        </tr>
                    </table>
                </div>
                <div id="divPage2" class="w3-border divPageNavPanelContent w3-animate-left" style="display: none">
                    <table class="tblEntryContent" style="width: 100%">
                        <colgroup>
                            <col style="width: 150px" />
                            <col />
                        </colgroup>
                        <tr>
                            <td class="tdLabel" valign="top" style="padding-top: 5px;">
                                <label class="lblNormal">
                                    <%=GetLabel("Pemeriksaan Klinis Setelah Radiasi") %></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtPostProgramMedicalSummary" runat="server" Width="99%" TextMode="Multiline" Height="450px" />
                            </td>
                        </tr>
                    </table>
                </div>
                <div id="divPage3" class="w3-border divPageNavPanelContent w3-animate-left" style="display: none">
                    <table class="tblEntryContent" style="width: 100%">
                        <colgroup>
                            <col style="width: 150px" />
                            <col />
                        </colgroup>
                        <tr>
                            <td class="tdLabel" valign="top" style="padding-top: 5px;">
                                <label class="lblNormal">
                                    <%=GetLabel("Toksisitas") %></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtToxicitySummary" runat="server" Width="99%" TextMode="Multiline" Height="450px" />
                            </td>
                        </tr>
                    </table>
                </div>
                <div id="divPage4" class="w3-border divPageNavPanelContent w3-animate-left" style="display: none">
                    <table class="tblEntryContent" style="width: 100%">
                        <colgroup>
                            <col style="width: 150px" />
                            <col />
                        </colgroup>
                        <tr>
                            <td class="tdLabel" valign="top" style="padding-top: 5px;">
                                <label class="lblNormal">
                                    <%=GetLabel("Rencana Tindak Lanjut") %></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtFollowupSummary" runat="server" Width="99%" TextMode="Multiline" Height="250px" />
                            </td>
                        </tr>
                    </table>
                </div>
            </td>
        </tr>
    </table>
</div>

