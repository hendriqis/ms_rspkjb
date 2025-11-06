<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="AntenatalRecordFormEntry.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.AntenatalRecordFormEntry" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<script type="text/javascript" id="dxss_antenatalFormEntryctl">
    $(function () {
        setDatePicker('<%=txtLMP.ClientID %>');
        setDatePicker('<%=txtEDB.ClientID %>');
        setDatePicker('<%=txtMembraneDate.ClientID %>');
        setDatePicker('<%=txtColicDate.ClientID %>');
        $('#<%=txtLMP.ClientID %>').datepicker('option', 'maxDate', '0');
        $('#<%=txtEDB.ClientID %>').datepicker('option', 'minDate', '0');
        $('#<%=txtMembraneDate.ClientID %>').datepicker('option', 'maxDate', '0');
        $('#<%=txtColicDate.ClientID %>').datepicker('option', 'maxDate', '0');

        calculateEDB();

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

        $('#leftPageNavPanel ul li').first().click();

        if ($('#<%=hdnFormValues.ClientID %>').val() != '') {
            var oldData = $('#<%=hdnFormValues.ClientID %>').val().split(';');
            for (var i = 0; i < oldData.length; ++i) {
                var temp = oldData[i].split('=');
                $('#<%=divFormContent.ClientID %>').find('.ddlForm').each(function () {
                    if ($(this).attr('controlID') == temp[0])
                        $(this).val(temp[1]);
                });
                $('#<%=divFormContent.ClientID %>').find('.chkForm').each(function () {
                    if ($(this).attr('controlID') == temp[0] && temp[1] == "1") {
                        $(this).prop('checked', true);
                    }
                });
                $('#<%=divFormContent.ClientID %>').find('.chkNursingProblem').each(function () {
                    if ($(this).attr('controlID') == temp[0] && temp[1] == "1") {
                        $(this).prop('checked', true);
                    }
                });
                $('#<%=divFormContent.ClientID %>').find('.optForm').each(function () {
                    if ($(this).attr('controlID') == temp[0] && temp[1] == "1") {
                        $(this).prop('checked', true);
                    }
                });
                $('#<%=divFormContent.ClientID %>').find('.txtForm').each(function () {
                    if ($(this).attr('controlID') == temp[0])
                        $(this).val(temp[1]);
                });
            }
        }
    });

    function onBeforeSaveRecordEntryPopup() {
        var values = getFormValues();
        return true;
    }

    function getFormValues() {
        var controlValues = '';
        $('#<%=divFormContent.ClientID %>').find('.ddlForm').each(function () {
            if (controlValues != '')
                controlValues += ";";
            controlValues += $(this).attr('controlID') + '=' + $(this).val();
        });
        $('#<%=divFormContent.ClientID %>').find('.txtForm').each(function () {
            if (controlValues != '')
                controlValues += ";";
            controlValues += $(this).attr('controlID') + '=' + $(this).val();
        });
        $('#<%=divFormContent.ClientID %>').find('.chkForm').each(function () {
            if (controlValues != '')
                controlValues += ";";
            if ($(this).is(':checked'))
                controlValues += $(this).attr('controlID') + '=1';
            else
                controlValues += $(this).attr('controlID') + '=0';
        });
        $('#<%=divFormContent.ClientID %>').find('.chkNursingProblem').each(function () {
            if (controlValues != '')
                controlValues += ";";
            if ($(this).is(':checked'))
                controlValues += $(this).attr('controlID') + '=1';
            else
                controlValues += $(this).attr('controlID') + '=0';
        });
        $('#<%=divFormContent.ClientID %>').find('.optForm').each(function () {
            if (controlValues != '')
                controlValues += ";";
            if ($(this).is(':checked'))
                controlValues += $(this).attr('controlID') + '=1';
            else
                controlValues += $(this).attr('controlID') + '=0';
        });
        $('#<%=hdnFormValues.ClientID %>').val(controlValues);

        return controlValues;
    }

    $('#<%=rblIsHasInfectiousDisease.ClientID %> input').change(function () {
        if ($(this).val() == "1") {
            $('#<%=trInfectiousInfo.ClientID %>').removeAttr("style");
        }
        else {
            $('#<%=trInfectiousInfo.ClientID %>').attr("style", "display:none");
        }
    });

    function onCboGCInfectiousDiseaseChanged(s) {
        var cboGCInfectiousDisease = s.GetValue();

        if (cboGCInfectiousDisease != Constant.InfectiousDisease.OTHERS) {
            $('#<%=txtOtherInfectiousDisease.ClientID %>').attr('readonly', 'readonly');
            $('#<%=txtOtherInfectiousDisease.ClientID %>').val('');
        }
        else {
            $('#<%=txtOtherInfectiousDisease.ClientID %>').removeAttr('readonly');
        }
    }

    function calculateEDB() {
        //Rumus Naegele : Tanggal HPHT ditambah 7, Bulan dikurang 3, dan tahun ditambah 1
        var fromDate = $('#<%=txtLMP.ClientID %>').val();

        var from = fromDate.split("-");
        var EDB = new Date(parseInt(from[2]) + 1, (parseInt(from[1]) - 1) - 3, parseInt(from[0]) + 7);

        $('#<%=txtEDB.ClientID %>').val(Methods.dateToDatePickerFormat(EDB));
    }

    $('#<%=txtLMP.ClientID %>').die('change');
    $('#<%=txtLMP.ClientID %>').live('change', function (evt) {
        calculateEDB();
    });
</script>
<div style="height: auto">
    <input type="hidden" runat="server" id="hdnID" value="" />
    <input type="hidden" runat="server" id="hdnFormGroup" value="" />
    <input type="hidden" runat="server" id="hdnFormType" value="" />
    <input type="hidden" runat="server" id="hdnFormLayout" value="" />
    <input type="hidden" runat="server" id="hdnFormValues" value="" />
    <input type="hidden" runat="server" id="hdnIsPartograf" value="0" />
    <table class="tblEntryContent">
        <colgroup>
            <col style="width: 20%" />
            <col style="width: 80%" />
        </colgroup>
        <tr>
            <td style="vertical-align: top">
                <div id="leftPageNavPanel" class="w3-border">
                    <ul>
                        <li contentid="divPage1" title="Antenal Record" class="w3-hover-red">Antenal Record</li>
                        <li contentid="divPage2" title="Form Checklist" class="w3-hover-red">Faktor Resiko</li>
                    </ul>
                </div>
            </td>
            <td style="vertical-align: top;">
                <div id="divPage1" class="w3-border divPageNavPanelContent w3-animate-left" style="display: none">
                    <table class="tblEntryContent" style="width: 100%">
                        <colgroup>
                            <col style="width: 180px" />
                            <col />
                        </colgroup>
                        <tr>
                            <td class="tdLabel" valign="top">
                                <label class="lblMandatory">
                                    <%=GetLabel("Kehamilan Ke-")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtPregnancyNo" Width="60px" CssClass="number" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel" valign="top">
                                <label class="lblMandatory">
                                    <%=GetLabel("Menarche")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtMenarche" Width="60px" CssClass="number" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel" valign="top">
                                <label class="lblMandatory">
                                    <%=GetLabel("Menstruasi Terakhir (HPHT)")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtLMP" Width="120px" CssClass="datepicker" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel" valign="top">
                                <label class="lblMandatory">
                                    <%=GetLabel("Lama Menstruasi (hari)")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtLMPDays" Width="60px" CssClass="number" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel" valign="top">
                                <label class="lblNormal">
                                    <%=GetLabel("Periode Menstruasi")%></label>
                            </td>
                            <td>
                                <dxe:ASPxComboBox runat="server" ID="cboLMPPeriod" ClientInstanceName="cboLMPPeriod"
                                    Width="50%">
                                </dxe:ASPxComboBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel" valign="top">
                                <label class="lblMandatory">
                                    <%=GetLabel("Estimasi Tanggal Persalinan")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtEDB" Width="120px" CssClass="datepicker" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel" valign="top">
                                <label class="lblMandatory">
                                    <%=GetLabel("G-P-A")%></label>
                            </td>
                            <td>
                                <table border="0" cellpadding="0" cellspacing="1">
                                    <colgroup>
                                        <col style="width: 60px" />
                                        <col style="width: 5px" />
                                        <col style="width: 60px" />
                                        <col style="width: 5px" />
                                        <col style="width: 60px" />
                                        <col style="width: 5px" />
                                        <col />
                                    </colgroup>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtGravida" Width="60px" CssClass="number" runat="server" />
                                        </td>
                                        <td style="text-align: center">
                                            -
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtPara" Width="60px" CssClass="number" runat="server" />
                                        </td>
                                        <td style="text-align: center">
                                            -
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtAbortion" Width="60px" CssClass="number" runat="server" />
                                        </td>
                                        <td style="text-align: center; display: none">
                                            -
                                        </td>
                                        <td style="display: none">
                                            <asp:TextBox ID="txtLife" Width="60px" CssClass="number" runat="server" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <%=GetLabel("Riwayat Penyakit Infeksi") %>
                            </td>
                            <td colspan="2">
                                <asp:RadioButtonList ID="rblIsHasInfectiousDisease" runat="server" RepeatDirection="Horizontal"
                                    RepeatLayout="Table">
                                    <asp:ListItem Text=" Ya" Value="1" />
                                    <asp:ListItem Text=" Tidak" Value="0" Selected="True" />
                                </asp:RadioButtonList>
                            </td>
                        </tr>
                        <tr id="trInfectiousInfo" runat="server" style="display: none">
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Tipe Penyakit Infeksi")%></label>
                            </td>
                            <td colspan="2">
                                <table border="0" cellpadding="1" cellspacing="0" style="width: 100%">
                                    <colgroup>
                                        <col style="width: 250px" />
                                        <col style="width: 80px" />
                                        <col />
                                    </colgroup>
                                    <tr>
                                        <td>
                                            <dxe:ASPxComboBox runat="server" ID="cboGCInfectiousDisease" ClientInstanceName="cboGCInfectiousDisease"
                                                Width="50%" ToolTip="Tipe Penyakit Infeksi">
                                                <ClientSideEvents ValueChanged="function(s){ onCboGCInfectiousDiseaseChanged(s); }" />
                                            </dxe:ASPxComboBox>
                                            <label class="lblNormal">
                                                <%=GetLabel("Lain-lain : ")%></label>
                                            <asp:TextBox ID="txtOtherInfectiousDisease" CssClass="txtOtherInfectiousDisease"
                                                runat="server" Width="100%" ReadOnly="true" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel" valign="top">
                                <label class="lblNormal">
                                    <%=GetLabel("Catatan Riwayat Menstruasi")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtMenstrualHistory" runat="server" TextMode="MultiLine" Rows="5"
                                    Width="100%" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel" valign="top" style="padding-top: 5px;">
                                <%=GetLabel("Catatan Medis Lainnya") %>
                            </td>
                            <td>
                                <asp:TextBox ID="txtMedicalHistory" runat="server" Width="100%" TextMode="Multiline"
                                    Rows="5" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                            </td>
                            <td colspan="3">
                                <asp:CheckBox ID="chkIsIbuNifas" runat="server" Text="Ibu Nifas mendapat vitamin A"
                                    Checked="false" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                            </td>
                            <td colspan="3">
                                <asp:CheckBox ID="chkIsKortikosteroid" runat="server" Text="Diberikan antenatal kortikosteroid"
                                    Checked="false" />
                            </td>
                        </tr>
                        <tr id="trPartografInfo1" runat="server">
                            <td class="tdLabel">
                                <label class="lblMandatory">
                                    <%=GetLabel("Ketuban Pecah ")%></label>
                            </td>
                            <td>
                                <table border="0" cellpadding="1" cellspacing="0">
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtMembraneDate" Width="120px" CssClass="datepicker" runat="server" />
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtMembraneTime" Width="80px" CssClass="time" runat="server" Style="text-align: center" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr id="trPartografInfo2" runat="server">
                            <td class="tdLabel">
                                <label class="lblMandatory">
                                    <%=GetLabel("Mulai Mules Perut ")%></label>
                            </td>
                            <td>
                                <table border="0" cellpadding="1" cellspacing="0">
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtColicDate" Width="120px" CssClass="datepicker" runat="server" />
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtColicTime" Width="80px" CssClass="time" runat="server" Style="text-align: center" />
                                        </td>
                                    </tr>
                                </table>
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
                            <td colspan="2">
                                <div id="divFormContent" runat="server" style="height: 450px; overflow-y: auto;">
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel" valign="top">
                                <label class="lblNormal">
                                    <%=GetLabel("Faktor Resiko")%></label>
                            </td>
                            <td>
                                <dxe:ASPxComboBox runat="server" ID="cboPregnancyRisk" ClientInstanceName="cboPregnancyRisk"
                                    Width="200px">
                                </dxe:ASPxComboBox>
                            </td>
                        </tr>
                    </table>
                </div>
            </td>
        </tr>
    </table>
</div>
