<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PharmacyJournalNotesCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.Pharmacy.Program.PharmacyJournalNotesCtl" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<script type="text/javascript" id="dxss_patientvisitnotesctl">
    setDatePicker('<%=txtLogDate.ClientID %>');
    $('#<%=txtLogDate.ClientID %>').datepicker('option', 'maxDate', '0');

    $('#lblAddData').die('click');
    $('#lblAddData').live('click', function () {
        $('#<%=hdnID.ClientID %>').val("");
        setDatePicker('<%=txtLogDate.ClientID %>');
        $('#<%=txtLogDate.ClientID %>').datepicker('option', 'maxDate', '0');
        $('#<%=txtNoteText.ClientID %>').val("");
        $('#<%=hdnPhysicianInstructionID.ClientID %>').val("");
        $('#<%=txtInstructionText.ClientID %>').val("");
        $('#containerPatientVisitNotesEntryData').show();
    });

    $('#btnCancel').click(function () {
        $('#containerPatientVisitNotesEntryData').hide();
    });

    $('#btnSave').click(function (evt) {
        if (IsValid(evt, 'fsLogNotes', 'mpLogNotes'))
            cbpLogNotes.PerformCallback('save');
        return false;
    });

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

    //#region Delete Edit
    $('.imgDeleteCtl.imgLink').die('click');
    $('.imgDeleteCtl.imgLink').live('click', function () {
        $row = $(this).closest('tr');
        showToastConfirmation('Are You Sure Want To Delete?', function (result) {
            if (result) {
                var entity = rowToObject($row);
                $('#<%=hdnID.ClientID %>').val(entity.ID);
                cbpLogNotes.PerformCallback('delete');
            }
        });
    });

    $('.imgEditCtl.imgLink').die('click')
    $('.imgEditCtl.imgLink').live('click', function () {
        $row = $(this).closest('tr');
        var entity = rowToObject($row);
        $('#<%=hdnID.ClientID %>').val(entity.ID);
        $('#<%=txtNoteText.ClientID %>').val(entity.NoteText);
        $('#<%=txtLogDate.ClientID %>').val(entity.cfJournalDate2);
        $('#<%=txtLogTime.ClientID %>').val(entity.JournalTime);
        $('#<%=txtNoteText.ClientID %>').val(entity.JournalText);
        $('#<%=hdnPhysicianInstructionID.ClientID %>').val(entity.PatientInstructionID);
        onTxtInstructionTextChanged(entity.PatientInstructionID);

        $('#containerPatientVisitNotesEntryData').show();
    });
    //#endregion

    function onPatientVisitNotesEndCallback(s) {
        var param = s.cpResult.split('|');
        if (param[0] == 'save') {
            if (param[1] == 'fail')
                showToast('Save Failed', 'Error Message : ' + param[2]);
            else
                $('#containerPatientVisitNotesEntryData').hide();
        }
        else if (param[0] == 'delete') {
            if (param[1] == 'fail')
                showToast('Delete Failed', 'Error Message : ' + param[2]);
        }
        hideLoadingPanel();
    }
</script>
<div style="height: 440px; overflow-y: auto">
    <input type="hidden" value="" runat="server" id="hdnVisitID" />
    <input type="hidden" value="" runat="server" id="hdnHealthcareServiceUnitIDCtl" />
    <input type="hidden" value="" runat="server" id="hdnTransactionID" />
    <input type="hidden" value="" runat="server" id="hdnPrescriptionOrderPhysicianID" />
    <input type="hidden" runat="server" id="hdnPhysicianInstructionID" value="" />
    <input type="hidden" runat="server" id="hdnInstructionID" value="" />
    <input type="hidden" value="" runat="server" id="hdnUserID" />
    <table class="tblContentArea">
        <tr>
            <td style="padding: 5px; vertical-align: top">
                <table class="tblEntryContent" style="width: 400px">
                    <colgroup>
                        <col style="width: 100px" />
                        <col style="width: 300px" />
                    </colgroup>
                </table>
                <table class="tblEntryContent" style="width: 100%">
                    <colgroup>
                        <col style="width: 150px" />
                        <col style="width: 140px" />
                        <col style="width: 60px" />
                        <col style="width: 120px;" />
                        <col style="width: 100px" />
                        <col />
                    </colgroup>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("No. RM")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtMRN" Width="160px" runat="server" />
                        </td>
                        <td>
                        </td>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Nama Pasien")%></label>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtPatientName" Width="100%" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("No. Resep")%></label>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtTransactionNo" Width="100%" runat="server" ReadOnly="true" />
                        </td>
                        <td class="tdLabel">
                            <label class="lblNormal" id="lblServiceUnit">
                                <%=GetLabel("Tanggal Resep")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtTransactionDate" Width="120px" runat="server" Style=""
                                ReadOnly="true" />
                        </td>
                        <td style="padding-left: 10px">
                            <asp:TextBox ID="txtTransactionTime" Width="60px" runat="server" Style="text-align: center"
                                ReadOnly="true" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal" id="Label1">
                                <%=GetLabel("Dokter")%></label>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtParamedic" Width="100%" runat="server" ReadOnly="true" />
                        </td>
                    </tr>
                </table>
                <div id="containerPatientVisitNotesEntryData" style="margin-top: 10px; display: none;">
                    <div class="pageTitle">
                        <%=GetLabel("Entry")%></div>
                    <input type="hidden" id="hdnID" runat="server" value="" />
                    <fieldset id="fsLogNotes" style="margin: 0">
                        <table class="tblEntryDetail" style="width: 100%">
                            <colgroup>
                                <col style="width: 30%" />
                                <col />
                            </colgroup>
                            <tr>
                                <td class="tdLabel">
                                    <%=GetLabel("Tanggal") %>
                                    -
                                    <%=GetLabel("Waktu") %>
                                </td>
                                <td>
                                    <table cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td style="padding-right: 1px; width: 145px">
                                                <asp:TextBox ID="txtLogDate" Width="120px" CssClass="datepicker" runat="server" />
                                            </td>
                                            <td style="width: 5px">
                                                &nbsp;
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtLogTime" Width="100px" CssClass="time" runat="server" Style="text-align: center" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr style="vertical-align: top">
                                <td class="tdLabel">
                                    <label class="lblMandatory">
                                        <%=GetLabel("Catatan Farmasi")%></label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtNoteText" Width="100%" CssClass="required" runat="server" TextMode="Multiline"
                                        Rows="2" />
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel" style="vertical-align: top">
                                    <label class="lblLink" id="lblPhysicianInstructionID">
                                        <%=GetLabel("Instruksi Dokter")%></label>
                                </td>
                                <td colspan="2">
                                    <asp:TextBox ID="txtInstructionText" Width="100%" Height="50px" runat="server" TextMode="MultiLine"
                                        ReadOnly="true" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <center>
                                        <table>
                                            <tr>
                                                <td>
                                                    <input type="button" id="btnSave" value='<%= GetLabel("Simpan")%>' class="btnSave w3-btn w3-hover-blue" />
                                                </td>
                                                <td>
                                                    <input type="button" id="btnCancel" value='<%= GetLabel("Batal")%>' class="btnCancel w3-btn w3-hover-blue" />
                                                </td>
                                            </tr>
                                        </table>
                                    </center>
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                </div>
                <dxcp:ASPxCallbackPanel ID="cbpLogNotes" runat="server" Width="100%" ClientInstanceName="cbpLogNotes"
                    ShowLoadingPanel="false" OnCallback="cbpLogNotes_Callback">
                    <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ onPatientVisitNotesEndCallback(s); }" />
                    <PanelCollection>
                        <dx:PanelContent ID="PanelContent1" runat="server">
                            <asp:Panel runat="server" ID="pnlPatientVisitTransHdGrdView" Style="width: 100%;
                                margin-left: auto; margin-right: auto; position: relative; font-size: 0.95em;">
                                <asp:GridView ID="grdVisitNotes" runat="server" CssClass="grdView notAllowSelect"
                                    AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                    <Columns>
                                        <asp:TemplateField HeaderStyle-Width="50px" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <img class="imgEditCtl imgLink" src='<%# ResolveUrl("~/Libs/Images/Button/edit.png")%>'
                                                    alt="" style="float: left; margin-left: 7px" />
                                                <img class="imgDeleteCtl imgLink" src='<%# ResolveUrl("~/Libs/Images/Button/delete.png")%>'
                                                    alt="" />
                                                <input type="hidden" value="<%#:Eval("ID") %>" bindingfield="ID" />
                                                <input type="hidden" value="<%#:Eval("cfJournalDate2") %>" bindingfield="cfJournalDate2" />
                                                <input type="hidden" value="<%#:Eval("JournalTime") %>" bindingfield="JournalTime" />
                                                <input type="hidden" value="<%#:Eval("JournalText") %>" bindingfield="JournalText" />
                                                <input type="hidden" value="<%#:Eval("PatientInstructionID") %>" bindingfield="PatientInstructionID" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="CfJournalDateTime" HeaderText="Tanggal" HeaderStyle-HorizontalAlign="Center"
                                            HeaderStyle-Width="100px" ItemStyle-HorizontalAlign="Center" />
                                        <asp:BoundField DataField="JournalText" HeaderText="Catatan" HeaderStyle-HorizontalAlign="Left"
                                            HeaderStyle-Width="200px" ItemStyle-HorizontalAlign="Left" />
                                        <asp:BoundField DataField="ChargeTransactionNo" HeaderText="Nomor Transaksi" HeaderStyle-HorizontalAlign="Left"
                                            HeaderStyle-Width="100px" />
                                        <asp:BoundField DataField="CreatedByName" HeaderText="Dibuat Oleh" HeaderStyle-HorizontalAlign="Left"
                                            HeaderStyle-Width="100px" />
                                        <asp:BoundField DataField="LastUpdatedByName" HeaderText="Diubah Oleh" HeaderStyle-HorizontalAlign="Left"
                                            HeaderStyle-Width="100px" />
                                    </Columns>
                                    <EmptyDataTemplate>
                                        <%=GetLabel("Tidak Ada Catatan Perubahan Order Resep")%>
                                    </EmptyDataTemplate>
                                </asp:GridView>
                                <div class="imgLoadingGrdView" id="containerImgLoadingPatientFamily">
                                    <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                                </div>
                            </asp:Panel>
                        </dx:PanelContent>
                    </PanelCollection>
                </dxcp:ASPxCallbackPanel>
                <div style="width: 100%; text-align: center" id="divContainerAddData" runat="server">
                    <span class="lblLink" id="lblAddData">
                        <%= GetLabel("Tambah Data")%></span>
                </div>
            </td>
        </tr>
    </table>
</div>
