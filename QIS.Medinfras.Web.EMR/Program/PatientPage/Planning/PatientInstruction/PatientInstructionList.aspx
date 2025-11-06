<%@ Page Language="C#" MasterPageFile="~/MasterPage/MPPatientPageListEntry.master" AutoEventWireup="true" 
    CodeBehind="PatientInstructionList.aspx.cs" Inherits="QIS.Medinfras.Web.EMR.Program.PatientInstructionList" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="plhRightToolbar" runat="server">
    <table cellpadding="0" cellspacing="0" style="width: 100%; display:none">
        <colgroup>
            <col />
        </colgroup>
        <tr>
            <td><%=GetLabel("Instruction Type") %></td>
            <td>
                <asp:DropDownList ID="ddlInstructionTypeFilter" runat="server">
                    <asp:ListItem Text="All" Value="0" Selected="True" />
                    <asp:ListItem Text="Diet & Excercise" Value="001" />
                    <asp:ListItem Text="Wound Care" Value="002" />
                    <asp:ListItem Text="Hygiene" Value="003" />
                    <asp:ListItem Text="Other" Value="999" />
                </asp:DropDownList>
            </td>
            <td>
                <div class="menuTitle">
                    <%=HttpUtility.HtmlEncode(GetPageTitle())%></div>
            </td>
        </tr>
    </table>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="plhCustomButtonToolbar" runat="server">
    <li id="btnOpenPatientInstructionQuickPicks" runat="server" CRUDMode="C" visible="false"><img src='<%=ResolveUrl("~/Libs/Images/Icon/tbnew.png")%>' alt="" /><div><%=GetLabel("Quick Picks")%></div></li>
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

            setDatePicker('<%=txtDate.ClientID %>');
            $('#<%=txtDate.ClientID %>').datepicker('option', 'minDate', '0');
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

        function onRefreshControl(filterExpression) {
            cbpView.PerformCallback('refresh');
        }

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
            cboInstructionType.SetFocus();
            $('#<%=grdView.ClientID %> tr.selected').removeClass('selected');
            if (entity != null) {
                $('#<%=grdView.ClientID %> tr.selected').addClass('selected');
                $('#<%=hdnEntryID.ClientID %>').val(entity.PatientInstructionID);
                $('#<%=hdnIsCompleted.ClientID %>').val(entity.IsCompleted);
                $('#<%=txtDate.ClientID %>').val(entity.cfInstructionDatePickerFormat);
                $('#<%=txtTime.ClientID %>').val(entity.InstructionTime);
                cboPhysician.SetValue(entity.PhysicianID);
                cboInstructionType.SetValue(entity.GCInstructionGroup);
                $('#<%=txtInstruction.ClientID %>').val(entity.Description);
                $('#<%=txtAdditionalText.ClientID %>').val(entity.AdditionalText);
            }
            else {
                $('#<%=hdnEntryID.ClientID %>').val('');
                cboPhysician.SetValue($('#<%=hdnDefaultParamedicID.ClientID %>').val());
                cboInstructionType.SetValue('');
                $('#<%=txtInstruction.ClientID %>').val('');
                $('#<%=txtAdditionalText.ClientID %>').val('');
            }
        }
        //#endregion

        $(function () {
            $('#<%=ddlInstructionTypeFilter.ClientID %>').change(function () {
                onPatientListEntryCancelEntryRecord();
                cbpView.PerformCallback('refresh');
            });

            $('#<%=btnOpenPatientInstructionQuickPicks.ClientID %>').click(function () {
                var url = ResolveUrl("~/Program/PatientPage/Planning/PatientInstruction/PatientInstructionQuickPicksCtl.ascx");
                openUserControlPopup(url, '', 'Quick Picks', 800, 400);
            });
        });
    </script>
</asp:Content>

<asp:Content ID="ctnEntry" ContentPlaceHolderID="plhEntry" runat="server">    
    <input type="hidden" value="" id="hdnEntryID" runat="server" />
    <input type="hidden" value="" id="hdnDefaultParamedicID" runat="server" />
    <input type="hidden" value="" id="hdnIsCompleted" runat="server" />
    <table style="width:100%" class="tblEntryDetail">
        <colgroup>
            <col style="width:150px"/>
            <col style="width:150px"/>
            <col />
        </colgroup>
        <tr>
            <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Physician")%></label></td>
            <td colspan="2"><dxe:ASPxComboBox runat="server" ID="cboPhysician" ClientInstanceName="cboPhysician" Width="400px" /></td>
        </tr>
        <tr>
            <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Date")%> - <%=GetLabel("Time")%></label></td>
            <td><asp:TextBox ID="txtDate" Width="120px" CssClass="datepicker" runat="server" /></td>
            <td><asp:TextBox ID="txtTime" Width="80px" CssClass="time" runat="server" Style="text-align:center" /></td>
        </tr>
        <tr>
            <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Instruction Type")%></label></td>
            <td colspan="2"><dxe:ASPxComboBox runat="server" ID="cboInstructionType" ClientInstanceName="cboInstructionType" Width="400px" /></td>
        </tr>
        <tr>
            <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Instruction")%></label></td>
            <td colspan="2"><asp:TextBox ID="txtInstruction" Width="400px" runat="server" TextMode="Multiline"
                                Rows="5" /></td>
        </tr>
        <tr>
            <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Additional Text")%></label></td>
            <td colspan="2"><asp:TextBox ID="txtAdditionalText" Width="400px" runat="server" /></td>
        </tr>
    </table>
</asp:Content>

<asp:Content ID="ctnList" ContentPlaceHolderID="plhList" runat="server">
    <input type="hidden" value="" id="hdnID" runat="server" />
    <input type="hidden" id="hdnFilterExpression" runat="server" value="" />
    <input type="hidden" value="" id="hdnPageTitle" runat="server" />
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
                                <asp:BoundField DataField="PatientInstructionID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                <asp:TemplateField HeaderStyle-CssClass="hiddenColumn" ItemStyle-CssClass="hiddenColumn" >
                                    <ItemTemplate>
                                        <input type="hidden" value="<%#:Eval("PatientInstructionID") %>" bindingfield="PatientInstructionID" />
                                        <input type="hidden" value="<%#:Eval("cfInstructionDate") %>" bindingfield="cfInstructionDate" />
                                        <input type="hidden" value="<%#:Eval("cfInstructionDatePickerFormat") %>" bindingfield="cfInstructionDatePickerFormat" />
                                        <input type="hidden" value="<%#:Eval("InstructionTime") %>" bindingfield="InstructionTime" />
                                        <input type="hidden" value="<%#:Eval("PhysicianID") %>" bindingfield="PhysicianID" />
                                        <input type="hidden" value="<%#:Eval("PhysicianName") %>" bindingfield="PhysicianName" />
                                        <input type="hidden" value="<%#:Eval("GCInstructionGroup") %>" bindingfield="GCInstructionGroup" />
                                        <input type="hidden" value="<%#:Eval("Description") %>" bindingfield="Description" />
                                        <input type="hidden" value="<%#:Eval("AdditionalText") %>" bindingfield="AdditionalText" />
                                        <input type="hidden" value="<%#:Eval("ExecutedDateTime") %>" bindingfield="ExecutedDateTime" />
                                        <input type="hidden" value="<%#:Eval("ExecutedBy") %>" bindingfield="ExecutedBy" />
                                        <input type="hidden" value="<%#:Eval("ExecutedByName") %>" bindingfield="ExecutedByName" />
                                        <input type="hidden" value="<%#:Eval("IsCompleted") %>" bindingfield="IsCompleted" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="cfInstructionDate" HeaderText="Date" HeaderStyle-Width="80px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"  />
                                <asp:BoundField DataField="InstructionTime" HeaderText="Time" HeaderStyle-Width="50px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="PhysicianName" HeaderText="Physician Name" HeaderStyle-Width="150px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Left" />
                                <asp:BoundField DataField="InstructionGroup" HeaderText="Instruction Group" HeaderStyle-Width="200px" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                                <asp:TemplateField HeaderText="Instruksi" HeaderStyle-Width="350px" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left">
                                    <ItemTemplate>
                                        <div>
                                            <textarea style="padding-left: 10px; border: 0; width: 99%; height: 60px; background-color: transparent"
                                                readonly><%#: DataBinder.Eval(Container.DataItem, "Description") %> </textarea>
                                        </div>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="cfExecutionDateTime" HeaderText="Completed On" HeaderStyle-Width="100px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="ExecutedByName" HeaderText="Completed By" HeaderStyle-Width="200px" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                                <asp:BoundField DataField="IsCompleted" HeaderText="Completed" HeaderStyle-Width="80px" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                            </Columns>
                            <EmptyDataTemplate>
                                <%=GetLabel("No Data To Display")%>
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
