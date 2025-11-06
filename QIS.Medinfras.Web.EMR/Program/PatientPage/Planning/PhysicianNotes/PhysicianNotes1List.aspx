<%@ Page Language="C#" MasterPageFile="~/MasterPage/MPPatientPageListEntry.master" AutoEventWireup="true" 
    CodeBehind="PhysicianNotes1List.aspx.cs" Inherits="QIS.Medinfras.Web.EMR.Program.PhysicianNotes1List" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>

<asp:Content ID="Content2" ContentPlaceHolderID="plhCustomButtonToolbar" runat="server">
    <li id="btnGenerateNote" runat="server" CRUDMode="C" style="display:none"><img src='<%=ResolveUrl("~/Libs/Images/Icon/tbprocess.png")%>' alt="" /><div><%=GetLabel("Generate")%></div></li>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="plhRightToolbar" runat="server">
    <table>
        <tr>
            <td><%=GetLabel("Notes View Type") %></td>
            <td>
                <asp:DropDownList ID="ddlViewType" runat="server">
                    <asp:ListItem Text="All" Value="0" Selected="True" />
                    <asp:ListItem Text="Physician Notes Only" Value="1" />
                    <asp:ListItem Text="Nursing and Other Paramedic Notes Only" Value="2" />
                    <asp:ListItem Text="My Notes Only" Value="3" />
                </asp:DropDownList>
            </td>
        </tr>
    </table>
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

            $('#<%=btnGenerateNote.ClientID %>').click(function () {
                var url = ResolveUrl("~/Program/PatientPage/ProgressNotes/GenerateSOAPNotesCtl.ascx");
                openUserControlPopup(url, "", 'Generate SOAP Notes', 800, 600);
            });

            $('#<%=ddlViewType.ClientID %>').change(function () {
                onPatientListEntryCancelEntryRecord();
                cbpView.PerformCallback('refresh');
            });
        });

        function onCboPhysicianInstructionSourceChanged(s) {
            if (s.GetValue() != null && s.GetValue().indexOf('^01') > -1) {
                $('#lblPhysicianNoteID').addClass('lblLink');
            }
            else {
                $('#lblPhysicianNoteID').removeClass('lblLink');
                $('#<%=hdnPlanningNoteID.ClientID %>').val('');
                $('#<%=txtPatientVisitNoteText.ClientID %>').val('');
            }
        }

        $('#lblPhysicianNoteID.lblLink').live('click', function () {
            var filterExpression = "VisitID = " + $('#<%=hdnVisitID.ClientID %>').val() + " AND GCPatientNoteType IN ('X011^010','X011^011') AND IsEdited = 0";
            openSearchDialog('planningNote', filterExpression, function (value) {
                onTxtPlanningNoteChanged(value);
            });
        });

        function onTxtPlanningNoteChanged(value) {
            var filterExpression = "ID = " + value;
            Methods.getObject('GetPatientVisitNoteList', filterExpression, function (result) {
                if (result != null) {
                    $('#<%=hdnPlanningNoteID.ClientID %>').val(result.ID);
                    $('#<%=txtPatientVisitNoteText.ClientID %>').val(result.NoteText);
                }
                else {
                    $('#<%=hdnPlanningNoteID.ClientID %>').val('');
                    $('#<%=txtPatientVisitNoteText.ClientID %>').val('');
                }
            });
        }

        function onRefreshControl() {
            cbpView.PerformCallback('refresh');
        }

        function onAfterSaveRecordPatientPageEntry() {
            cbpView.PerformCallback('refresh');
        }

        function onCboPhysicianChanged(s) {
            if (s.GetValue() != null) {
                $('#<%=hdnParamedicID.ClientID %>').val(s.GetValue());
            }
            else
                $('#<%=hdnParamedicID.ClientID %>').val('');
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

        function onBeforeEditRecord(entity, errMessage) {
            if (entity.ParamedicID != $('#<%=hdnDefaultParamedicID.ClientID %>').val()) {
                errMessage.text = 'This record is belong to another physician, you are not allowed to edit or delete it';
                return false;
            }
            return true;
        }

        function onBeforeDeleteRecord(entity, errMessage) {
            if (entity.ParamedicID != $('#<%=hdnDefaultParamedicID.ClientID %>').val()) {
                errMessage.text = 'This record is belong to another physician, you are not allowed to edit or delete it';
                return false;
            }
            return true;
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
                $('#<%=txtSubjectiveText.ClientID %>').val(entity.SubjectiveText);
                $('#<%=txtObjectiveText.ClientID %>').val(entity.ObjectiveText);
                $('#<%=txtAssessmentText.ClientID %>').val(entity.AssessmentText);
                $('#<%=txtPlanningText.ClientID %>').val(entity.PlanningText);
                cboPhysicianInstructionSource.SetValue(entity.GCPhysicianInstructionSource);
                $('#<%=hdnPlanningNoteID.ClientID %>').val(entity.LinkedNoteID);
                $('#<%=chkIsNeedConfirmation.ClientID %>').prop('checked', (entity.IsNeedConfirmation == 'True'));
                $('#<%=hdnParamedicID.ClientID %>').val(entity.ConfirmationPhysicianID);
                cboSpecialistPhysician.SetValue(entity.ConfirmationPhysicianID);
            }
            else {
                var currentdate = getDateNowDatePickerFormat();
                var currenttime = getTimeNow(); 
                $('#<%=hdnEntryID.ClientID %>').val('');
                cboPhysician.SetValue($('#<%=hdnDefaultParamedicID.ClientID %>').val());
                $('#<%=txtNoteDate.ClientID %>').val(currentdate);
                $('#<%=txtNoteTime.ClientID %>').val(currenttime);
                $('#<%=txtSubjectiveText.ClientID %>').val('');
                $('#<%=txtObjectiveText.ClientID %>').val('');
                $('#<%=txtAssessmentText.ClientID %>').val('');
                $('#<%=txtPlanningText.ClientID %>').val('');
                cboPhysicianInstructionSource.SetValue('');
                $('#<%=hdnPlanningNoteID.ClientID %>').val('');
                $('#<%=hdnParamedicID.ClientID %>').val('');
                $('#<%=chkIsNeedConfirmation.ClientID %>').prop('checked', false);
                cboSpecialistPhysician.SetValue('');
            }
            $('#<%=txtSubjectiveText.ClientID %>').focus();
        }
        //#endregion
    </script>
</asp:Content>

<asp:Content ID="ctnEntry" ContentPlaceHolderID="plhEntry" runat="server">    
    <input type="hidden" value="" id="hdnEntryID" runat="server" />
    <table class="tblEntryDetail">
        <colgroup>
            <col style="width:150px"/>
            <col style="width:150px"/>
            <col style="width:80px"/>
            <col />
            <col style="width:500px"/>
        </colgroup>
        <tr>
            <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Date")%> - <%=GetLabel("Time")%></label></td>
            <td><asp:TextBox ID="txtNoteDate" Width="120px" CssClass="datepicker" runat="server" /></td>
            <td><asp:TextBox ID="txtNoteTime" Width="80px" CssClass="time" runat="server"/></td>
            <td></td>
        </tr>
        <tr>
            <td class="tdLabel" valign="top"><label class="lblMandatory"><%=GetLabel("Physician")%></label></td>
            <td colspan="3"><dxe:ASPxComboBox runat="server" ID="cboPhysician" ClientInstanceName="cboPhysician" Width="300px" /></td>
        </tr>
        <tr>
            <td class="tdLabel" style="vertical-align:top"><label class="lblMandatory"><%=GetLabel("Subjective")%></label></td>
            <td colspan="3"><asp:TextBox ID="txtSubjectiveText" Width="100%" runat="server" TextMode="MultiLine" Rows="6" /></td>
            <td rowspan="6" style="vertical-align:top">
                <table id="tblNotesInstruction" runat="server" border="0" cellpadding="1" cellspacing="0" style="width:100%">
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Specialist Instruction Source")%></label>
                        </td>
                        <td colspan="2">
                            <dxe:ASPxComboBox ID="cboPhysicianInstructionSource" ClientInstanceName="cboPhysicianInstructionSource"
                            runat="server" Width="100%" >
                                <ClientSideEvents SelectedIndexChanged="function(s,e){ onCboPhysicianInstructionSourceChanged(s); }" Init="function(s,e){ onCboPhysicianInstructionSourceChanged(s); }" />
                            </dxe:ASPxComboBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel" style="vertical-align:top">
                            <label class="lblLink" id="lblPhysicianNoteID">
                                <%=GetLabel("Specialist Instruction / Note")%></label>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtPatientVisitNoteText" Width="100%" Height="350px" runat="server" TextMode="MultiLine" ReadOnly="true" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <asp:CheckBox ID="chkIsNeedConfirmation" runat="server" Checked = "false" /> <%:GetLabel("Need Confirmation")%>
                        </td>
                        <td colspan="2">
                            <dxe:ASPxComboBox ID="cboSpecialistPhysician" ClientInstanceName="cboSpecialistPhysician" runat="server" Width="100%" >
                                <ClientSideEvents SelectedIndexChanged="function(s,e){ onCboPhysicianChanged(s); }" Init="function(s,e){ onCboPhysicianChanged(s); }" />
                            </dxe:ASPxComboBox>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td class="tdLabel" valign="top">
                <label class="lblMandatory">
                    <%=GetLabel("Objective")%></label>
            </td>
            <td colspan="3">
                <asp:TextBox ID="txtObjectiveText" runat="server" Width="100%" TextMode="Multiline"
                    Rows="6" />
            </td>
        </tr>
        <tr>
            <td class="tdLabel" valign="top">
                <label class="lblMandatory">
                    <%=GetLabel("Assessment")%></label>
            </td>
            <td colspan="3">
                <asp:TextBox ID="txtAssessmentText" runat="server" Width="100%" TextMode="Multiline"
                    Rows="6" />
            </td>
        </tr>
        <tr>
            <td class="tdLabel" valign="top">
                <label class="lblMandatory">
                    <%=GetLabel("Planning")%></label>
            </td>
            <td colspan="3">
                <asp:TextBox ID="txtPlanningText" runat="server" Width="100%" TextMode="Multiline"
                    Rows="6" />
            </td>
        </tr>
    </table>
</asp:Content>

<asp:Content ID="ctnList" ContentPlaceHolderID="plhList" runat="server">
    <input type="hidden" value="" id="hdnID" runat="server" />
    <input type="hidden" id="hdnFilterExpression" runat="server" value="" />
    <input type="hidden" value="" id="hdnDefaultParamedicID" runat="server" />
    <input type="hidden" runat="server" id="hdnVisitID" value="" />
    <input type="hidden" runat="server" id="hdnPlanningNoteID" value="" />
    <input type="hidden" runat="server" id="hdnParamedicID" value="" />

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
                                        <input type="hidden" value="<%#:Eval("SubjectiveText") %>" bindingfield="SubjectiveText" />
                                        <input type="hidden" value="<%#:Eval("ObjectiveText") %>" bindingfield="ObjectiveText" />
                                        <input type="hidden" value="<%#:Eval("AssessmentText") %>" bindingfield="AssessmentText" />
                                        <input type="hidden" value="<%#:Eval("PlanningText") %>" bindingfield="PlanningText" />
                                        <input type="hidden" value="<%#:Eval("GCPhysicianInstructionSource") %>" bindingfield="GCPhysicianInstructionSource" />
                                        <input type="hidden" value="<%#:Eval("LinkedNoteID") %>" bindingfield="LinkedNoteID" />
                                        <input type="hidden" value="<%#:Eval("ConfirmationPhysicianID") %>" bindingfield="ConfirmationPhysicianID" />
                                        <input type="hidden" value="<%#:Eval("IsNeedConfirmation") %>" bindingfield="IsNeedConfirmation" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="cfNoteDate" HeaderText="Date" HeaderStyle-Width="100px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="NoteTime" HeaderText="Time" HeaderStyle-Width="60px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"  />
                                <asp:BoundField DataField="cfPPA" HeaderText="PPA" HeaderStyle-Width="30px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"  />
                                <asp:TemplateField HeaderText="Note" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left">
                                    <ItemTemplate>
                                        <div><span style="color:blue; font-style:italic"><%#:Eval("ParamedicName") %> </span> : <span style="font-style:italic"><%#:Eval("cfLastUpdatedDate") %></span> </div>
                                        <div>
                                            <textarea style="padding-left:10px;border:0;width:99%; height:150px ; background-color:transparent" readonly><%#: DataBinder.Eval(Container.DataItem, "NoteText") %> </textarea>
                                        </div>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Remarks" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="150px">
                                    <ItemTemplate>
                                        <div style="color: blue; font-style: italic; vertical-align:top">
                                            <%#:Eval("cfLastUpdatedDate") %>,                                            
                                        </div>
                                        <div>
                                            <b><%#:Eval("cfLastUpdatedByName") %></b>
                                        </div>
                                        <div><img class="imgNeedConfirmation" src='<%# ResolveUrl("~/Libs/Images/Status/warning.png")%>' alt="" style='<%# Eval("cfIsShowWarningIcon").ToString() == "False" ? "display:none;": "" %> max-width:30px; cursor:pointer; min-width: 30px; float: left;' title="Need confirmation" /></div>
                                        <div><img class="imgNeedNotification" src='<%# ResolveUrl("~/Libs/Images/Status/warning.png")%>' alt="" style='<%# Eval("IsNeedNotification").ToString() == "False" ? "display:none;": "" %> max-width:30px; cursor:pointer; min-width: 30px; float: left;' title="Using Notification" /></div>
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
