<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PharmacyJournalCtl.ascx.cs" Inherits="QIS.Medinfras.Web.Pharmacy.Program.PharmacyJournalCtl" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>

<script type="text/javascript" id="dxss_pharmacyJournalctl">
    $(function () {
        setDatePicker('<%=txtNoteDate.ClientID %>');
        $('#<%=txtNoteDate.ClientID %>').datepicker('option', 'maxDate', '0');

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

        //#region Instruksi Dokter
        $('#lblPhysicianInstructionID.lblLink').click(function () {
            var filterExpression = "VisitID = " + $('#<%=hdnVisitID.ClientID %>').val() + " AND GCInstructionGroup IN ('X139^001','X139^006','X139^007','X139^008')";
            openSearchDialog('physicianInstruction', filterExpression, function (value) {
                $('#<%=hdnPhysicianInstructionID.ClientID %>').val(value);
                onTxtInstructionTextChanged(value);
            });
        });

        function onTxtInstructionTextChanged(value) {
            var filterExpression = "PatientInstructionID = " + value;
            Methods.getObject('GetPatientInstructionList', filterExpression, function (result) {
                if (result != null) {
                    $('#<%=txtInstructionText.ClientID %>').val(result.Description);
                }
                else {
                    $('#<%=txtInstructionText.ClientID %>').val('');
                }
            });
        }
        //#endregion
    });

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
    <input type="hidden" runat="server" id="hdnPhysicianInstructionID" value="" />
    <input type="hidden" runat="server" id="hdnInstructionID" value="" />
    <input type="hidden" runat="server" id="hdnInstructionText" value="" />
    <table class="tblContentArea">
        <colgroup>
            <col style="width: 100%" />
        </colgroup>
        <tr>
            <td style="padding: 5px; vertical-align: top;">
                <table class="tblEntryContent" style="width: 100%">
                    <colgroup>
                        <col style="width: 180px" />
                        <col style="width: 150px" />
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
                        <td class="tdLabel" valign="top" style="padding-top: 5px;">
                            <label class="lblNormal">
                                <%=GetLabel("Catatan Farmasi") %></label>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtNoteText" runat="server" Width="100%" TextMode="Multiline"
                                Height="250px" />
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
                    <tr>
                        <td class="tdLabel" style="vertical-align:top">
                            <label class="lblLink" id="lblPhysicianInstructionID">
                                <%=GetLabel("Instruksi Dokter")%></label>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtInstructionText" Width="100%" Height="50px" runat="server" TextMode="MultiLine" ReadOnly="true" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</div>
