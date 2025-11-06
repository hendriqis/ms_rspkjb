<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PrescriptionOrderChangesLogCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.Pharmacy.Program.PrescriptionOrderChangesLogCtl" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<script type="text/javascript" id="dxss_patientvisitnotesctl">
    setDatePicker('<%=txtLogDate.ClientID %>');
    $('#<%=txtLogDate.ClientID %>').datepicker('option', 'maxDate', '0');

    $('#lblPatientVisitNoteAddData').die('click');
    $('#lblPatientVisitNoteAddData').live('click', function () {
        $('#<%=hdnLogID.ClientID %>').val("");
        $('#<%=txtNoteText.ClientID %>').val("");
        $('#containerPatientVisitNotesEntryData').show();
    });

    $('#btnPatientVisitNotesCancel').click(function () {
        $('#containerPatientVisitNotesEntryData').hide();
    });

    $('#btnPatientVisitNotesSave').click(function (evt) {
        if (IsValid(evt, 'fsLogNotes', 'mpLogNotes'))
            cbpLogNotes.PerformCallback('save');
        return false;
    });

    $('.imgDeleteCtl.imgLink').die('click');
    $('.imgDeleteCtl.imgLink').live('click', function () {
        $row = $(this).closest('tr');
        showToastConfirmation('Are You Sure Want To Delete?', function (result) {
            if (result) {
                var entity = rowToObject($row);
                $('#<%=hdnLogID.ClientID %>').val(entity.ID);
                cbpLogNotes.PerformCallback('delete');
            }
        });
    });

    $('.imgEditCtl.imgLink').die('click')
    $('.imgEditCtl.imgLink').live('click', function () {
        $row = $(this).closest('tr');
        var entity = rowToObject($row);
        $('#<%=hdnLogID.ClientID %>').val(entity.ID);
        $('#<%=txtNoteText.ClientID %>').val(entity.NoteText);
        
        $('#containerPatientVisitNotesEntryData').show();
    });

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
    <input type="hidden" value="" runat="server" id="hdnPrescriptionOrderID" />
    <input type="hidden" value="" runat="server" id="hdnPrescriptionOrderPhysicianID" />
    <table class="tblContentArea">
        <tr>
            <td style="padding: 5px; vertical-align: top">
                <table class="tblEntryContent" style="width: 400px">
                    <colgroup>
                        <col style="width: 100px" />
                        <col style="width: 300px" />
                    </colgroup>
                </table>
                <table class="tblEntryContent" style="width:100%">
                    <colgroup>
                        <col style="width:150px"/>
                        <col style="width:140px"/>
                        <col style="width:60px"/>
                        <col style="width:120px;"/>
                        <col style="width:100px"/>
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
                        <td></td>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Nama Pasien")%></label>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtPatientName" Width="100%" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblNormal"><%=GetLabel("No. Order Resep")%></label></td>
                        <td colspan="2"><asp:TextBox ID="txtPrescriptionOrderNo" Width="100%" runat="server" ReadOnly="true" /></td> 
                        <td class="tdLabel"><label class="lblNormal" id="lblServiceUnit"><%=GetLabel("Tanggal Order")%></label></td>
                        <td><asp:TextBox ID="txtPrescriptionOrderDate" Width="120px" runat="server" Style="" ReadOnly="true" /></td>
                        <td style="padding-left:10px"><asp:TextBox ID="txtPrescriptionOrderTime"  Width="60px" runat="server" Style="text-align:center" ReadOnly="true" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal" id="Label1"><%=GetLabel("Dokter")%></label>
                        </td>
                        <td colspan="2"> 
                            <asp:TextBox ID="txtParamedic" Width="100%" runat="server" ReadOnly="true"  />
                        </td>
                    </tr>
                </table>
                <div id="containerPatientVisitNotesEntryData" style="margin-top: 10px; display: none;">
                    <div class="pageTitle">
                        <%=GetLabel("Entry")%></div>
                    <input type="hidden" id="hdnLogID" runat="server" value="" />
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
                            <tr style="vertical-align:top">
                                <td class="tdLabel">
                                    <label class="lblMandatory">
                                        <%=GetLabel("Catatan Perubahan")%></label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtNoteText" Width="100%" CssClass="required" runat="server" 
                                    TextMode="Multiline" Rows="2" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <center>
                                        <table>
                                            <tr>
                                                <td>
                                                    <input type="button" id="btnPatientVisitNotesSave" value='<%= GetLabel("Simpan")%>' class="btnPatientVisitNotesSave w3-btn w3-hover-blue" />
                                                </td>
                                                <td>
                                                    <input type="button" id="btnPatientVisitNotesCancel" value='<%= GetLabel("Batal")%>' class="btnPatientVisitNotesCancel w3-btn w3-hover-blue" />
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
                                        <asp:TemplateField HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <img class="imgEditCtl imgLink" src='<%# ResolveUrl("~/Libs/Images/Button/edit.png")%>'
                                                    alt="" style="float: left; margin-left: 7px" />
                                                <img class="imgDeleteCtl imgLink" src='<%# ResolveUrl("~/Libs/Images/Button/delete.png")%>'
                                                    alt="" />
                                                <input type="hidden" value="<%#:Eval("ID") %>" bindingfield="ID" />
                                                <input type="hidden" value="<%#:Eval("NoteText") %>" bindingfield="NoteText" />
                                                <input type="hidden" value="<%#:Eval("cfLogDate") %>" bindingfield="LogDate" />
                                                <input type="hidden" value="<%#:Eval("LogTime") %>" bindingfield="LogTime" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="cfLogDate" HeaderText="Tanggal" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="120px" ItemStyle-HorizontalAlign="Center" />
                                        <asp:BoundField DataField="LogTime" HeaderText="Jam" HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="60px" ItemStyle-HorizontalAlign="Center" />
                                        <asp:BoundField DataField="FullName" HeaderText="User" HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="150px" />
                                        <asp:BoundField DataField="NoteText" HeaderText="Catatan" HeaderStyle-HorizontalAlign="Left" />
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
                    <span class="lblLink" id="lblPatientVisitNoteAddData">
                        <%= GetLabel("Tambah Data")%></span>
                </div>
            </td>
        </tr>
    </table>
</div>
