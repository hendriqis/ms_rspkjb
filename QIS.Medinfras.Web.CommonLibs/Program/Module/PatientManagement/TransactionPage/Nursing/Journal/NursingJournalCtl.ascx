<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="NursingJournalCtl.ascx.cs" Inherits="QIS.Medinfras.Web.CommonLibs.Program.NursingJournalCtl" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>

<script type="text/javascript" id="dxss_chiefcomplaintentryctl">
    $(function () {
        setDatePicker('<%=txtNoteDate.ClientID %>');
        $('#<%=txtNoteDate.ClientID %>').datepicker('option', 'maxDate', new Date(getDateNow()));

        $("#<%=txtNoteText.ClientID %>").focus();

        //#region Transaction No
        $('#lblTransactionNo.lblLink').click(function () {
            var filterExpression = "VisitID = " + $('#<%=hdnVisitID.ClientID %>').val() + " AND GCTransactionStatus NOT IN ('X121^001','X121^999') " + "AND CreatedBy = " + $('#<%=hdnUserID.ClientID %>').val();
            openSearchDialog('patientchargeshd1', filterExpression, function (value) {
                $('#<%=hdnTransactionID.ClientID %>').val(value);
                onTxtTransactionNoChanged(value);
            });
        });

        $('#<%=txtTransactionNo.ClientID %>').change(function () {
            onTxtTransactionNoChanged($(this).val());
        });

        function onTxtTransactionNoChanged(value) {
            var filterExpression = "TransactionID = " + value;
            Methods.getObject('GetPatientChargesHdList', filterExpression, function (result) {
                if (result != null) {
                    $('#<%=txtTransactionNo.ClientID %>').val(result.TransactionNo);
                }
                else {
                    $('#<%=txtTransactionNo.ClientID %>').val('');
                }
            });
        }
        //#endregion
    });

    $('#lblNursingJournal.lblLink').live('click', function () {
        if ($('#<%=hdnUserID.ClientID %>').val() != null) {
            var filterExpression = "UserID = " + $('#<%=hdnUserID.ClientID %>').val() + " AND IsDeleted = 0";
            openSearchDialog('paramedictext', filterExpression, function (value) {
                $('#<%=hdnParamedicTemplateTextID.ClientID %>').val(value);
                onSearchParamedicTemplateText(value);
            });
        }
    });

    function onSearchParamedicTemplateText(value) {
        var filterExpression = "TemplateCode = " + "'" + value + "'";
        Methods.getObject('GetvParamedicTextList', filterExpression, function (result) {
            if (result != null) {
                $('#<%=txtNoteText.ClientID %>').val(result.TemplateText);
            }
            else {
                $('#<%=txtNoteText.ClientID %>').val('');
            }
        });
    }

    function onCboPhysicianChanged(s) {
        if (s.GetValue() != null) {
            $('#<%=hdnParamedicID.ClientID %>').val(s.GetValue());
        }
        else
            $('#<%=hdnParamedicID.ClientID %>').val('');
    }
</script>
<style type="text/css">
    #ulVitalSign
    {
        margin: 0;
        padding: 0;
    }
    #ulVitalSign li
    {
        list-style-type: none;
        display: inline-block;
        padding-left: 5px;
        width: 48%;
    }
</style>
<div>
    <input type="hidden" runat="server" id="hdnID" value="" />
    <input type="hidden" runat="server" id="hdnVisitID" value="" />
    <input type="hidden" runat="server" id="hdnPlanningNoteID" value="" />
    <input type="hidden" runat="server" id="hdnParamedicID" value="" />
    <input type="hidden" runat="server" id="hdnUserID" value="" />
    <input type="hidden" runat="server" id="hdnTransactionID" value="" />
    <input type="hidden" runat="server" id="hdnIsAddNew" value="" />
    <input type="hidden" runat="server" id="hdnParamedicTemplateTextID" value="" />
    <table class="tblContentArea">
        <colgroup>
            <col style="width: 100%" />
        </colgroup>
        <tr>
            <td style="padding: 5px; vertical-align: top;">
                <table class="tblEntryContent" style="width: 100%">
                    <colgroup>
                        <col style="width: 180px" />
                        <col style="width: 180px" />
                        <col />
                    </colgroup>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblMandatory">
                                <%=GetLabel("Tanggal ")%>
                                -
                                <%=GetLabel("Jam ")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtNoteDate" Width="120px" CssClass="datepicker" runat="server" />
                        </td>
                        <td>
                            <asp:TextBox ID="txtNoteTime" Width="80px" CssClass="time" runat="server" Style="text-align: center" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblMandatory">
                                <%=GetLabel("Perawat")%></label>
                        </td>
                        <td colspan="2">
                            <dxe:ASPxComboBox ID="cboParamedicID" Width="300px" runat="server" />
                        </td>
                    </tr>
                    <tr style="display:none">
                        <td class="tdLabel">
                            <label class="lblMandatory">
                                <%=GetLabel("Jenis Catatan")%></label>
                        </td>
                        <td colspan="2">
                            <dxe:ASPxComboBox ID="cboNursingJournalType" Width="300px" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel" valign="top" style="padding-top: 5px;">
                            <label class="lblNormal lblLink" id="lblNursingJournal">
                                <%=GetLabel("Catatan Perawat") %></label>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtNoteText" runat="server" Width="100%" TextMode="Multiline"
                                Height="250px" />
                        </td>
                    </tr>
                    <tr>
                        <td></td>
                        <td>
                            <asp:CheckBox ID="chkIsNeedConfirmation" runat="server" Checked = "false" /> <%:GetLabel("Perlu Konfirmasi Perawat")%>
                        </td>
                        <td id="tdchkIsBillingInformation" runat="server">
                            <asp:CheckBox ID="chkIsBillingInformation" runat="server" Checked = "false" /> <%:GetLabel("Notifikasi Penagihan")%>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <div style="position: relative;">
                                <label class="lblLink" id="lblTransactionNo">
                                    <%=GetLabel("No. Transaksi")%></label></div>
                        </td>
                        <td>
                            <asp:TextBox ID="txtTransactionNo" Width="150px" ReadOnly="true" runat="server" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</div>
<div id="divSaveAsNewTemplate" runat="server">
    <table>
        <tr>
            <td></td>
            <td id="tdchkSaveAsNewTemplate" runat="server" colspan="2">
                <asp:CheckBox ID="chkSaveAsNewTemplate" runat="server" Checked = "false" /> <%:GetLabel("Simpan Sebagai Template")%>
            </td>
        </tr>   
    </table>
</div>