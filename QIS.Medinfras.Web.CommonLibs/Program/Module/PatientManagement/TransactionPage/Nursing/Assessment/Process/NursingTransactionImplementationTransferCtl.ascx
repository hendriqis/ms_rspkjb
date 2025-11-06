<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="NursingTransactionImplementationTransferCtl.ascx.cs" 
Inherits="QIS.Medinfras.Web.CommonLibs.Program.NursingTransactionImplementationTransferCtl" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>

<script type="text/javascript">
    var iRowIndex4 = 0;
    setDatePicker('<%=txtJournalDate.ClientID %>');
    $('#<%=txtJournalDate.ClientID %>').datepicker('option', 'maxDate', '0');

    $('#btnCancel').live('click', function () {
        $('#containerEntry').hide();
    });

    $('#btnSave').live('click', function () {
        if (IsValid(null, 'fsTrx', 'mpTrx'))
            cbpProcess.PerformCallback('save');
    });

    $('#<%=hdnID.ClientID %>').live('change', onHdnIDValueChanged);

    $('#<%=grdViewImplementation.ClientID %> tr:gt(0)').live('click', function () {
        iRowIndex4 = $('#<%=grdViewImplementation.ClientID %> tr').index(this);
        $('#<%=grdViewImplementation.ClientID %> tr.selected').removeClass('selected');
        $(this).addClass('selected');
        $('#<%=hdnID.ClientID %>').val($(this).find('.keyField').html());
        $('#<%=hdnActivityDescription.ClientID %>').val($(this).find('.nursingItemText').html());
        onHdnIDValueChanged();
        cbpViewImplementation1.PerformCallback('refresh');

    });

    $('#lblAddData').live('click', function () {
        if ($('#<%=hdnID.ClientID %>').val() != '' && $('#<%=hdnID.ClientID %>').val() != '0') {
            $('#<%=hdnEntryID.ClientID%>').val('');
            $('#<%=txtJournalDate.ClientID%>').val(getDateNowDatePickerFormat());
            $('#<%=txtJournalTime.ClientID%>').val(getTimeNow());
            $('#<%=txtRemarks.ClientID %>').val($('#<%=hdnActivityDescription.ClientID %>').val());
            $('#containerEntry').show();
        }
        else {
            showToast('Failed', 'Pilih intervensi yang bersangkutan terlebih dahulu');
        }
    });

    $('#<%=grdViewImplementation1.ClientID %> .imgDelete.imgLink').live('click', function () {
        $row = $(this).closest('tr').parent().closest('tr');
        showToastConfirmation('Are You Sure Want To Delete?', function (result) {
            if (result) {
                var entity = rowToObject($row);
                $('#<%=hdnEntryID.ClientID %>').val(entity.ID);
                cbpProcess.PerformCallback('delete');
            }
        });
    });



    $('#<%=grdViewImplementation1.ClientID %> .imgEdit.imgLink').live('click', function () {
        $row = $(this).closest('tr').parent().closest('tr');
        var entity = rowToObject($row);

        $('#<%=hdnEntryID.ClientID %>').val(entity.ID);
        $('#<%=txtJournalDate.ClientID%>').val(entity.JournalDate);
        $('#<%=txtJournalTime.ClientID%>').val(entity.JournalTime);
        $('#<%=hdnChargesTransactionID.ClientID %>').val(entity.ChargeTransactionID);
        $('#<%=txtChargesTransactionNo.ClientID %>').val(entity.ChargeTransactionNo);
        $('#<%=chkIsNeedConfirmation.ClientID %>').val(entity.IsNeedVerification);
        $('#<%=txtRemarks.ClientID %>').val(entity.Remarks);
        $('#containerEntry').show();
    });

    function onCbpViewImplementationEndCallback(s) {
        hideLoadingPanel();
        onHdnIDValueChanged();
        if (iRowIndex4 > 0) {
            $("#<%=grdViewImplementation1.ClientID %> tr:eq(" + iRowIndex4 + ")").addClass('selected');
        }
    }

    function onCbpViewImplementation1EndCallback(s) {
        hideLoadingPanel();
        $('#<%=hdnOldID.ClientID %>').val($('#<%=hdnID.ClientID %>').val());

        cbpViewImplementation.PerformCallback('refresh');
    }

    function onCbpProcessEndCallback(s) {
        hideLoadingPanel();
        var param = s.cpResult.split('|');
        if (param[0] == 'save') {
            if (param[1] == 'fail')
                showToast('Save Failed', 'Error Message : ' + param[2]);
            else {
                $('#containerEntry').hide();
                cbpViewImplementation1.PerformCallback('refresh');
                cbpViewImplementation.PerformCallback('refresh');
            }
        }
        else if (param[0] == 'delete') {
            if (param[1] == 'fail')
                showToast('Delete Failed', 'Error Message : ' + param[2]);
            else {
                cbpViewImplementation1.PerformCallback('refresh');
                cbpViewImplementation.PerformCallback('refresh');
            }
        }
    }

    function onHdnIDValueChanged() {
        $('#containerEntry').hide();
        if ($('#<%=hdnID.ClientID %>').val() == '' || $('#<%=hdnID.ClientID %>').val() == '0') {
            $('.tdEntryImplementation').hide();
        }
        else
            $('.tdEntryImplementation').show();
    }

    //#region Transaction No
    $('#lblChargesTransactionNo.lblLink').live('click', function () {
        var filterExpression = "VisitID = " + $('#<%=hdnCurrentVisitID.ClientID %>').val() + " AND GCTransactionStatus NOT IN ('X121^001','X121^999') " + "AND CreatedBy = " + $('#<%=hdnCurrentUserID.ClientID %>').val();
        openSearchDialog('patientchargeshd1', filterExpression, function (value) {
            $('#<%=hdnChargesTransactionID.ClientID %>').val(value);
            onTxtChargesTransactionNoChanged(value);
        });
    });

    $('#<%=txtChargesTransactionNo.ClientID %>').change(function () {
        onTxtChargesTransactionNoChanged($(this).val());
    });

    function onTxtChargesTransactionNoChanged(value) {
        var filterExpression = "TransactionID = " + value;
        Methods.getObject('GetPatientChargesHdList', filterExpression, function (result) {
            if (result != null) {
                $('#<%=txtChargesTransactionNo.ClientID %>').val(result.ChargesTransactionNo);
            }
            else {
                $('#<%=txtChargesTransactionNo.ClientID %>').val('');
            }
        });
    }
    //#endregion   
</script>

<input type="hidden" id="hdnChargesTransactionID" runat="server" value="" />
<input type="hidden" id="hdnNursingDiagnoseID" runat="server" value="" />
<input type="hidden" value="" id="hdnOldID" runat="server" />
<input type="hidden" value="" id="hdnIsEditable" runat="server" />
<input type="hidden" value="" id="hdnID" runat="server" />
<input type="hidden" value="" id="hdnActivityDescription" runat="server" />
<input type="hidden" runat="server" id="hdnCurrentVisitID" value="" />
<input type="hidden" runat="server" id="hdnCurrentUserID" value="" />

<table width="100%">
    <colgroup>
        <col width="30%" />
        <col />
    </colgroup>
    <tr valign="top">
        <td>
            <dxcp:ASPxCallbackPanel ID="cbpViewImplementation" runat="server" Width="100%" ClientInstanceName="cbpViewImplementation"
                ShowLoadingPanel="false" OnCallback="cbpViewImplementation_Callback">
                <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }"
                    EndCallback="function(s,e){ onCbpViewImplementationEndCallback(s); }" />
                <PanelCollection>
                    <dx:PanelContent ID="PanelContent1" runat="server">
                        <asp:Panel runat="server" ID="pnlView" CssClass="pnlContainerGrid">
                            <asp:GridView ID="grdViewImplementation" runat="server" CssClass="grdSelected" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty" OnRowDataBound="grdViewImplementation_RowDataBound">
                                <Columns>
                                    <asp:BoundField DataField="ID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                    <asp:BoundField DataField="NursingItemText" HeaderText="Aktifitas Intervensi" HeaderStyle-HorizontalAlign="Left"  ItemStyle-CssClass="nursingItemText"/>
                                    <asp:TemplateField HeaderText="Item" HeaderStyle-Width="50px" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <asp:Label runat="server" ID="lblTotalSelected"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <EmptyDataTemplate>
                                    No Data To Display
                                </EmptyDataTemplate>
                            </asp:GridView>
                        </asp:Panel>
                    </dx:PanelContent>
                </PanelCollection>
            </dxcp:ASPxCallbackPanel>    
            <div class="imgLoadingGrdView" id="containerImgLoadingView">
                <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
            </div>
        </td>
        <td>
            <div class="tdEntryImplementation">
                <div id="containerEntry" style="margin-top:4px;display:none;">
                    <div class="pageTitle"><%=GetLabel("Entry Implementasi (Catatan Perawat)")%></div>
                    <fieldset id="fsTrx" style="margin:0"> 
                        <input type="hidden" value="" id="hdnEntryID" runat="server" />
                        <table class="tblEntryDetail" width="50%">
                            <colgroup>
                                <col width="150px" />
                                <col />
                            </colgroup>
                            <tr>
                                <td class="tdLabel"><%=GetLabel("Tanggal") %> / <%=GetLabel("Jam Jurnal") %></td>
                                <td>
                                    <table cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td style="padding-right: 1px"><asp:TextBox ID="txtJournalDate" Width="120px" CssClass="datepicker" runat="server" /></td>
                                            <td style="width:5px">&nbsp;</td>
                                            <td><asp:TextBox ID="txtJournalTime" Width="80px" CssClass="time" runat="server" Style="text-align:center" /></td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td valign="top"><%=GetLabel("Catatan") %></td>
                                <td><asp:TextBox runat="server" Width="100%" Rows="4" id="txtRemarks" TextMode="MultiLine" Wrap="true" /></td>
                            </tr>
                            <tr>
                                <td></td>
                                <td colspan="2">
                                    <asp:CheckBox ID="chkIsNeedConfirmation" runat="server" Checked = "false" /> <%:GetLabel("Perlu Konfirmasi Perawat Lain")%>
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <div style="position: relative;">
                                        <label class="lblLink" id="lblChargesTransactionNo">
                                            <%=GetLabel("No. Transaksi")%></label></div>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtChargesTransactionNo" Width="150px" ReadOnly="true" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3">
                                    <center>
                                        <table>
                                            <tr>
                                                <td>
                                                    <input type="button" id="btnSave" value='<%= GetLabel("Save")%>' class="btnSave w3-btn w3-hover-blue" />
                                                </td>
                                                <td>
                                                    <input type="button" id="btnCancel" value='<%= GetLabel("Cancel")%>' class="btnCancel w3-btn w3-hover-blue" />
                                                </td>
                                            </tr>
                                        </table>
                                    </center>
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                    </div>     
                <input type="hidden" id="hdnFilterExpression" runat="server" value="" />
                <div style="position: relative;">
                    <dxcp:ASPxCallbackPanel ID="cbpViewImplementation1" runat="server" Width="100%" ClientInstanceName="cbpViewImplementation1"
                        ShowLoadingPanel="false" OnCallback="cbpViewImplementation1_Callback">
                        <PanelCollection>
                            <dx:PanelContent ID="PanelContent3" runat="server">
                                <asp:Panel runat="server" ID="Panel1" CssClass="pnlContainerGrid">
                                    <asp:GridView ID="grdViewImplementation1" runat="server" CssClass="grdView notAllowSelect" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                        <Columns>
                                            <asp:BoundField DataField="ID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                            <asp:TemplateField HeaderStyle-Width="60px" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <table cellpadding="0" cellspacing="0" runat="server" ID="tblEditDelete">
                                                        <tr>
                                                            <td>
                                                                <img class="imgEdit" title='<%=GetLabel("Edit")%>' src='<%# ResolveUrl("~/Libs/Images/Button/edit_disabled.png")%>' alt=""/>
                                                            </td>
                                                            <td style="width:1px">&nbsp;</td>
                                                            <td>
                                                                <img class="imgDelete" title='<%=GetLabel("Delete")%>' src='<%# ResolveUrl("~/Libs/Images/Button/delete_disabled.png")%>' alt=""/>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                    <input type="hidden" value="<%#:Eval("ID") %>" bindingfield="ID" />
                                                    <input type="hidden" value="<%#:Eval("JournalDateInStringDatePickerFormat") %>" bindingfield="JournalDate" />
                                                    <input type="hidden" value="<%#:Eval("JournalTime") %>" bindingfield="JournalTime" />
                                                    <input type="hidden" value="<%#:Eval("IsNeedVerification") %>" bindingfield="IsNeedVerification" />
                                                    <input type="hidden" value="<%#:Eval("ChargeTransactionID") %>" bindingfield="ChargeTransactionID" />
                                                    <input type="hidden" value="<%#:Eval("ChargeTransactionNo") %>" bindingfield="ChargeTransactionNo" />
                                                    <input type="hidden" value="<%#:Eval("Remarks") %>" bindingfield="Remarks" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="cfJournalDate" HeaderText="Tanggal" HeaderStyle-Width="100px"
                                                HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                            <asp:BoundField DataField="JournalTime" HeaderText="Jam" HeaderStyle-Width="60px" HeaderStyle-HorizontalAlign="Center"
                                                ItemStyle-HorizontalAlign="Center" />
                                            <asp:TemplateField HeaderText="Catatan" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left">
                                                <ItemTemplate>
                                                    <div>
                                                        <span style="color: blue; font-style: italic; vertical-align:top">
                                                            <%#:Eval("ParamedicName") %> - <b> (<%#:Eval("ServiceUnitName") %>) <span style="color:red"><%#:Eval("cfConfirmationInfo") %></span></b>
                                                        </span>
                                                    </div>
                                                    <div style="height:130px; overflow-y:auto; margin-top:15px;">
                                                        <%#Eval("Remarks").ToString().Replace("\n","<br />")%><br />
                                                    </div>                                                                            
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <EmptyDataTemplate>
                                            <%=GetLabel("Belum ada catatan perawat untuk implementasi aktifitas")%>
                                        </EmptyDataTemplate>
                                    </asp:GridView>
                                    <div style="width:100%;text-align:center">
                                     <%--   <span class="lblLink" id="lblAddData" ><%= GetLabel("Tambah Catatan")%></span>--%>
                                    </div>
                                </asp:Panel>
                            </dx:PanelContent>
                        </PanelCollection>
                    </dxcp:ASPxCallbackPanel>    
                    <dxcp:ASPxCallbackPanel ID="cbpProcess" runat="server" Width="100%" ClientInstanceName="cbpProcess"
                        ShowLoadingPanel="false" OnCallback="cbpProcess_Callback">
                        <ClientSideEvents BeginCallback="function(s,e) { showLoadingPanel(); }"
                            EndCallback="function(s,e) { onCbpProcessEndCallback(s); }" />
                    </dxcp:ASPxCallbackPanel>
                </div>
            </div>
        </td>
    </tr>
</table>