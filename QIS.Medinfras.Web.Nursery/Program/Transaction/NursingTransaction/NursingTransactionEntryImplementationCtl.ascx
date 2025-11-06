<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="NursingTransactionEntryImplementationCtl.ascx.cs" 
Inherits="QIS.Medinfras.Web.Nursing.Program.NursingTransactionEntryImplementationCtl" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>

<script type="text/javascript">    
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

    $('#<%=grdView.ClientID %> tr:gt(0)').live('click', function () {
        $('#<%=grdView.ClientID %> tr.selected').removeClass('selected');
        $(this).addClass('selected');
        $('#<%=hdnID.ClientID %>').val($(this).find('.keyField').html());
        onHdnIDValueChanged();
        cbpViewImplementation1.PerformCallback('refresh');

    });

    $('#lblAddData').live('click', function () {
        if ($('#<%=hdnID.ClientID %>').val() != '' && $('#<%=hdnID.ClientID %>').val() != '0') {
            $('#<%=hdnEntryID.ClientID%>').val('');
            $('#<%=txtJournalDate.ClientID%>').val(getDateNowDatePickerFormat());
            $('#<%=txtJournalTime.ClientID%>').val(getTimeNow());
            $('#<%=txtRemarks.ClientID %>').val('');
            $('#containerEntry').show();
        }
        else {
            showToast('Failed', 'Pilih intervensi yang bersangkutan terlebih dahulu');
        }
    });

    $('#<%=grdView.ClientID %> .imgDelete.imgLink').live('click', function () {
        $row = $(this).closest('tr').parent().closest('tr');
        showToastConfirmation('Are You Sure Want To Delete?', function (result) {
            if (result) {
                var entity = rowToObject($row);
                $('#<%=hdnEntryID.ClientID %>').val(entity.ID);
                cbpProcess.PerformCallback('delete');
            }
        });
    });



    $('#<%=grdView1.ClientID %> .imgEdit.imgLink').live('click', function () {
        $row = $(this).closest('tr').parent().closest('tr');
        var entity = rowToObject($row);

        $('#<%=hdnEntryID.ClientID %>').val(entity.ID);
        $('#<%=txtJournalDate.ClientID%>').val(entity.JournalDate);
        $('#<%=txtJournalTime.ClientID%>').val(entity.JournalTime);
        $('#<%=txtRemarks.ClientID %>').val(entity.Remarks);
        $('#containerEntry').show();
    });

    function onCbpViewImplementationEndCallback(s) {
        hideLoadingPanel();
        onHdnIDValueChanged();
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

   
</script>

<input type="hidden" id="hdnTransactionID" runat="server" value="" />
<input type="hidden" id="hdnNursingDiagnoseID" runat="server" value="" />
<input type="hidden" value="" id="hdnOldID" runat="server" />
<input type="hidden" value="" id="hdnID" runat="server" />

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
                            <asp:GridView ID="grdView" runat="server" CssClass="grdSelected" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty" OnRowDataBound="grdView_RowDataBound">
                                <Columns>
                                    <asp:BoundField DataField="ID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                    <asp:BoundField DataField="NursingItemText" HeaderText="Item Intervensi" />
                                    <asp:TemplateField HeaderText="Jml" HeaderStyle-Width="50px" ItemStyle-HorizontalAlign="Center">
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
                    <div class="pageTitle"><%=GetLabel("Entry Implementasi (Jurnal Keperawatan)")%></div>
                    <fieldset id="fsTrx" style="margin:0"> 
                        <input type="hidden" value="" id="hdnEntryID" runat="server" />
                        <table class="tblEntryDetail" width="50%">
                            <colgroup>
                                <col width="50px" />
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
                                <td><asp:TextBox runat="server" Width="600px" Rows="4" id="txtRemarks" TextMode="MultiLine" Wrap="true" /></td>
                            </tr>
                            <tr>
                                <td>
                                    <table width="100%">
                                        <tr>
                                            <td>
                                                <input type="button" id="btnSave" value='<%= GetLabel("Save")%>' />
                                            </td>
                                            <td>
                                                <input type="button" id="btnCancel" value='<%= GetLabel("Cancel")%>' />
                                            </td>
                                        </tr>
                                    </table>
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
                                    <asp:GridView ID="grdView1" runat="server" CssClass="grdView notAllowSelect" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                        <Columns>
                                            <asp:BoundField DataField="ID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                            <asp:TemplateField HeaderStyle-Width="60px" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <table cellpadding="0" cellspacing="0" runat="server" ID="tblEditDelete">
                                                        <tr>
                                                            <td>
                                                                <img class="imgEdit imgLink" title='<%=GetLabel("Edit")%>' src='<%# ResolveUrl("~/Libs/Images/Button/edit.png")%>' alt="" />
                                                            </td>
                                                            <td style="width:1px">&nbsp;</td>
                                                            <td>
                                                                <img class="imgDelete imgLink" title='<%=GetLabel("Delete")%>' src='<%# ResolveUrl("~/Libs/Images/Button/delete.png")%>' alt="" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                    <input type="hidden" value="<%#:Eval("ID") %>" bindingfield="ID" />
                                                    <input type="hidden" value="<%#:Eval("JournalDateInStringDatePickerFormat") %>" bindingfield="JournalDate" />
                                                    <input type="hidden" value="<%#:Eval("JournalTime") %>" bindingfield="JournalTime" />
                                                    <input type="hidden" value="<%#:Eval("Remarks") %>" bindingfield="Remarks" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="CfJournalDateTime" HeaderText="Tanggal Jurnal" HeaderStyle-Width="130px" ItemStyle-HorizontalAlign="Center" />
                                            <asp:TemplateField HeaderText="Catatan">
                                                <ItemTemplate>
                                                    <asp:TextBox BorderStyle="None" BackColor="Transparent" Rows="4" runat="server" Width="100%" Enabled="false" ID="txtGridRemarks" TextMode="MultiLine" Wrap="true" Text='<%#: Eval("Remarks") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Informasi Jurnal" HeaderStyle-Width="250px">
                                                <ItemTemplate>
                                                    <table width="100%">
                                                        <tr>
                                                            <td><asp:Label ID="Label1" runat="server" Text='<%#: "Dibuat oleh: " + Eval("CreatedByName") + " " + Eval("CreatedDateInString")%>' /></td>
                                                        </tr>
                                                        <tr>
                                                            <td><asp:Label ID="Label2" runat="server" Text='<%#: "Diedit oleh: " + Eval("LastUpdatedByName") + " " + Eval("LastUpdatedDateInString")%>' /></td>
                                                        </tr>
                                                    </table>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <EmptyDataTemplate>
                                            <%=GetLabel("No Data To Display")%>
                                        </EmptyDataTemplate>
                                    </asp:GridView>
                                    <div style="width:100%;text-align:center">
                                        <span class="lblLink" id="lblAddData" ><%= GetLabel("Add Data")%></span>
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