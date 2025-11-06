<%@ Page Language="C#" MasterPageFile="~/MasterPage/MPPatientPageListEntry.master" AutoEventWireup="true" 
    CodeBehind="PlanningNotesList.aspx.cs" Inherits="QIS.Medinfras.Web.EMR.Program.PlanningNotesList" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>

<asp:Content ID="Content2" ContentPlaceHolderID="plhCustomButtonToolbar" runat="server">
    <li id="btnGenerateNote" runat="server" CRUDMode="C"><img src='<%=ResolveUrl("~/Libs/Images/Icon/tbprocess.png")%>' alt="" /><div><%=GetLabel("Generate")%></div></li>
</asp:Content>


<asp:Content ID="Content3" ContentPlaceHolderID="plhScript" runat="server">
    <script type="text/javascript">
        $(function () {
            $('#<%=grdView.ClientID %> tr:gt(0):not(.trEmpty)').live('click', function () {
                $('#<%=grdView.ClientID %> tr.selected').removeClass('selected');
                $(this).addClass('selected');
                $('#<%=hdnID.ClientID %>').val($(this).find('.keyField').html());
            });
            $('#<%=grdView.ClientID %> tr:eq(1)').click();

            setDatePicker('<%=txtNoteDate.ClientID %>');
            $('#<%=txtNoteDate.ClientID %>').datepicker('option', 'maxDate', '0');
        });

        function onAfterSaveRecordPatientPageEntry() {
            cbpView.PerformCallback('refresh');
        }

        //#region Paging
        var pageCount = parseInt('<%=PageCount %>');
        $(function () {
            setPaging($("#paging"), pageCount, function (page) {
                cbpView.PerformCallback('changepage|' + page);
            });
        });

        function onCbpViewEndCallback(s) {
            $('#containerImgLoadingView').hide();

            var param = s.cpResult.split('|');
            if (param[0] == 'refresh') {
                var pageCount = parseInt(param[1]);
                if (pageCount > 0)
                    $('#<%=grdView.ClientID %> tr:eq(1)').click();

                setPaging($("#paging"), pageCount, function (page) {
                    cbpView.PerformCallback('changepage|' + page);
                });
            }
            else
                $('#<%=grdView.ClientID %> tr:eq(1)').click();
        }
        //#endregion

        function onSelectedRowChanged(value) {
            var idx = $('#<%=grdView.ClientID %> tr').index($('#<%=grdView.ClientID %> tr.selected'));
            idx += value;
            if (idx < 1)
                idx = 1;
            if (idx == $('#<%=grdView.ClientID %> tr').length)
                idx--;
            $('#<%=grdView.ClientID %> tr:eq(' + idx + ')').click();
        }

        function getSelectedRow() {
            return $('#<%=grdView.ClientID %> tr.selected');
        }

        function onButtonCancelClick() {
            $('#<%=grdView.ClientID %> tr.selected').removeClass('selected');
        }

        //#region Entity To Control
        function entityToControl(entity) {
            $('#<%=grdView.ClientID %> tr.selected').removeClass('selected');
            if (entity != null) {
                $('#<%=grdView.ClientID %> tr.focus').addClass('selected');
                $('#<%=hdnEntryID.ClientID %>').val(entity.ID);
                cboPhysician.SetValue(entity.ParamedicID);
                $('#<%=txtNoteDate.ClientID %>').val(entity.NoteDateInDatePickerFormat);
                $('#<%=txtNoteTime.ClientID %>').val(entity.NoteTime);
                $('#<%=txtNoteText.ClientID %>').val(entity.NoteText);
            }
            else {
                $('#<%=hdnEntryID.ClientID %>').val('');
                cboPhysician.SetValue($('#<%=hdnDefaultParamedicID.ClientID %>').val());
                $('#<%=txtNoteText.ClientID %>').val('');
            }
            $('#<%=txtNoteText.ClientID %>').focus();
        }
        //#endregion

        $(function () {
            $('#<%=btnGenerateNote.ClientID %>').click(function () {
//                var queryString = prescriptionID + '|' + date + '|' + time + '|' + physician + '|' + prescriptionType + '|' + dispensaryServiceUnitID + '|' + location + '|' + refillInstruction;

                var url = ResolveUrl("~/Program/PatientPage/Planning/PlanningNotes/GeneratePlanningNotesCtl.ascx");
                openUserControlPopup(url, "", 'Generate Planning Notes', 800, 600);
            });
        });
    </script>
</asp:Content>

<asp:Content ID="ctnEntry" ContentPlaceHolderID="plhEntry" runat="server">    
    <input type="hidden" value="" id="hdnEntryID" runat="server" />
    <table class="tblEntryDetail">
        <colgroup>
            <col style="width:150px"/>
            <col style="width:150px"/>
            <col />
        </colgroup>
        <tr>
            <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Date")%> - <%=GetLabel("Time")%></label></td>
            <td><asp:TextBox ID="txtNoteDate" Width="120px" CssClass="datepicker" runat="server" /></td>
            <td><asp:TextBox ID="txtNoteTime" Width="80px" CssClass="time" runat="server"/></td>
        </tr>
        <tr>
            <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Physician")%></label></td>
            <td colspan="3"><dxe:ASPxComboBox runat="server" ID="cboPhysician" ClientInstanceName="cboPhysician" Width="300px" /></td>
        </tr>
        <tr>
            <td class="tdLabel" style="vertical-align:top"><label class="lblNormal"><%=GetLabel("Planning Note")%></label></td>
            <td colspan="3"><asp:TextBox ID="txtNoteText" Width="750px" runat="server" TextMode="MultiLine" Height="350px" /></td>
        </tr>
    </table>
</asp:Content>

<asp:Content ID="ctnList" ContentPlaceHolderID="plhList" runat="server">
    <input type="hidden" value="" id="hdnID" runat="server" />
    <input type="hidden" id="hdnFilterExpression" runat="server" value="" />
    <input type="hidden" value="" id="hdnDefaultParamedicID" runat="server" />
    <div style="position: relative;">
        <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
            ShowLoadingPanel="false" OnCallback="cbpView_Callback">
            <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingView').show(); }"
                EndCallback="function(s,e){ onCbpViewEndCallback(s); }" />
            <PanelCollection>
                <dx:PanelContent ID="PanelContent1" runat="server">
                    <asp:Panel runat="server" ID="pnlView" CssClass="pnlContainerGrid" Style="height:300px">
                        <asp:GridView ID="grdView" runat="server" CssClass="grdSelected grdPatientPage" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                            <Columns>
                                <asp:BoundField DataField="ID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                <asp:TemplateField HeaderStyle-CssClass="hiddenColumn" ItemStyle-CssClass="hiddenColumn" >
                                    <ItemTemplate>
                                        <input type="hidden" value="<%#:Eval("ID") %>" bindingfield="ID" />
                                        <input type="hidden" value="<%#:Eval("ParamedicID") %>" bindingfield="ParamedicID" />
                                        <input type="hidden" value="<%#:Eval("NoteDate") %>" bindingfield="NoteDate" />
                                        <input type="hidden" value="<%#:Eval("NoteDateInDatePickerFormat") %>" bindingfield="NoteDateInDatePickerFormat" />
                                        <input type="hidden" value="<%#:Eval("NoteTime") %>" bindingfield="NoteTime" />
                                        <input type="hidden" value="<%#:Eval("NoteText") %>" bindingfield="NoteText" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="cfNoteDate" HeaderText="Date" HeaderStyle-Width="100px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="NoteTime" HeaderText="Time" HeaderStyle-Width="60px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"  />
                                <asp:TemplateField HeaderText="Note" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left">
                                    <ItemTemplate>
                                        <div><span style="color:blue; font-style:italic"><%#:Eval("ParamedicName") %> </span> : </div>
                                        <div><asp:TextBox ID="txtNotes" Width="100%" runat="server" TextMode="MultiLine" Height="240px" Text='<%#:Eval("NoteText")%>' style="background-color:transparent;color:Black; border:0 none" ReadOnly="true" /></div>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <EmptyDataTemplate>
                                <%=GetLabel("No record to display")%>
                            </EmptyDataTemplate>
                        </asp:GridView>
                    </asp:Panel>
                </dx:PanelContent>
            </PanelCollection>
        </dxcp:ASPxCallbackPanel>    
        <div class="imgLoadingGrdView" id="containerImgLoadingView" >
            <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
        </div>
        <div class="containerPaging">
            <div class="wrapperPaging">
                <div id="paging"></div>
            </div>
        </div> 
    </div>
</asp:Content>
