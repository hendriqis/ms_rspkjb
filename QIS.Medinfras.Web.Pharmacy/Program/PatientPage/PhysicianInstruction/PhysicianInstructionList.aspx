<%@ Page Language="C#" MasterPageFile="~/Libs/MasterPage/PatientPage/MPBasePatientPageTrx2.master" AutoEventWireup="true" 
    CodeBehind="PhysicianInstructionList.aspx.cs" Inherits="QIS.Medinfras.Web.Pharmacy.Program.PhysicianInstructionList" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>

<%@ Register Src="~/Libs/Program/Module/PatientManagement/TransactionPage/TransactionPageToolbarCtl.ascx"
    TagName="ToolbarCtl" TagPrefix="uc1" %>
<asp:Content ID="Content4" ContentPlaceHolderID="plhMPPatientPageNavigationPane"
    runat="server">
    <uc1:ToolbarCtl ID="ctlToolbar" runat="server" />
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div style="font-size: 1.4em">
        <%=HttpUtility.HtmlEncode(GetPageTitle())%></div>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhHeader" runat="server">
    <input type="hidden" value="" id="hdnDepartmentID" runat="server" />
    <input type="hidden" value="" id="hdnPageTitle" runat="server" />
    <input type="hidden" value="" id="hdnSubMenuType" runat="server" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="plhCustomButtonToolbar" runat="server">
    <li id="btnRefresh" runat="server" CRUDMode="R"><img src='<%=ResolveUrl("~/Libs/Images/Icon/tbrefresh.png")%>' alt="" /><div><%=GetLabel("Refresh")%></div></li>
    <li id="btnConfirm" runat="server" CRUDMode="R"><img src='<%=ResolveUrl("~/Libs/Images/Icon/tbset.png")%>' alt="" /><div><%=GetLabel("Completed")%></div></li>
    <li id="btnNursingNote" runat="server" CRUDMode="R"><img src='<%=ResolveUrl("~/Libs/Images/Icon/tbnew.png")%>' alt="" /><div><%=GetLabel("Note")%></div></li>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript">
        $(function () {
            $('#<%=grdView.ClientID %> tr:gt(0):not(.trEmpty)').live('click', function () {
                $('#<%=grdView.ClientID %> tr.selected').removeClass('selected');
                $(this).addClass('selected');
                $('#<%=hdnID.ClientID %>').val($(this).find('.keyField').html());
            });
            $('#<%=grdView.ClientID %> tr:eq(1)').click();

            $('#<%=btnRefresh.ClientID %>').click(function () {
                cbpView.PerformCallback('refresh');
            });

            $('#<%=btnConfirm.ClientID %>').click(function () {
                if (!getSelectedCheckbox()) {
                    showToast("ERROR", 'Error Message : ' + "Please select the item to be process !");
                }
                else {
                    var message = "Are you sure to complete the instruction ?";
                    showToastConfirmation(message, function (result) {
                        if (result) cbpCustomProcess.PerformCallback('complete');
                    });
                }
            });

            $('#<%=btnNursingNote.ClientID %>').click(function () {
                if (!getSelectedCheckbox()) {
                    showToast("ERROR", 'Error Message : ' + "Please select the item to be process !");
                }
                else {
                    var selectedID = $('#<%=hdnSelectedID.ClientID %>').val();
                    var selectedText = $('#<%=hdnSelectedText.ClientID %>').val();
                    var param = '0' + '|' + selectedID + '|' + selectedText;
                    openUserControlPopup("~/Program/PatientPage/PharmacyJournal/PharmacyJournalCtl.ascx", param, "Catatan Jurnal Farmasi", 700, 400);
                }
            });
        });

        $('#chkSelectAll').die('change');
        $('#chkSelectAll').live('change', function () {
            var isChecked = $(this).is(":checked");
            $('.chkIsProcessItem').each(function () {
                $(this).find('input').prop('checked', isChecked);
            });
        });

        function getSelectedCheckbox() {
            var tempSelectedID = "";
            var tempSelectedText = "";
            $('#<%=grdView.ClientID %> .chkIsProcessItem input:checked').each(function () {
                var $tr = $(this).closest('tr');
                var id = $(this).closest('tr').find('.keyField').html();
                var text = $(this).closest('tr').find('.description').html();

                if (tempSelectedID != "") {
                    tempSelectedID += ",";
                }
                tempSelectedID += id;

                if (tempSelectedText != "") {
                    tempSelectedText += ",";
                }
                tempSelectedText += text;
            });
            if (tempSelectedID != "") {
                $('#<%=hdnSelectedID.ClientID %>').val(tempSelectedID);
                $('#<%=hdnSelectedText.ClientID %>').val(tempSelectedText);
                return true;
            }
            else return false;
        }

        function onRefreshControl() {
            cbpView.PerformCallback('refresh');
        }

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

        function onCbpCustomProcesEndCallback(s) {
            hideLoadingPanel();
            var param = s.cpResult.split('|');
            var retval = s.cpRetval.split('|');
            if (param[0] == 'process') {
                if (param[1] == '0') {
                    showToast("Process Failed", "Error Message : <br/><span style='color:red'>" + param[2] + "</span>");
                }
                onRefreshControl();
            }
        }

        function onBeforeLoadRightPanelContent(code) {
            if (code == 'patientVisit') {
                return $('#<%:hdnRegistrationID.ClientID %>').val();
            }
        }
    </script>
    <input type="hidden" value="" id="hdnID" runat="server" />
    <input type="hidden" id="hdnFilterExpression" runat="server" value="" />
    <input type="hidden" value="" id="hdnDefaultParamedicID" runat="server" />
    <input type="hidden" value="" id="hdnSelectedID" runat="server" />
    <input type="hidden" value="" id="hdnSelectedText" runat="server" />
    <input type="hidden" value="" id="hdnRegistrationID" runat="server" />
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
                                <asp:TemplateField HeaderText = "" HeaderStyle-HorizontalAlign="center" HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="center">
                                    <HeaderTemplate>
                                        <input id="chkSelectAll" type="checkbox" />
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkIsProcessItem" runat="server" CssClass="chkIsProcessItem" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderStyle-CssClass="hiddenColumn" ItemStyle-CssClass="hiddenColumn" >
                                    <ItemTemplate>
                                        <input type="hidden" value="<%#:Eval("PatientInstructionID") %>" bindingfield="PatientInstructionID" />
                                        <input type="hidden" value="<%#:Eval("PhysicianID") %>" bindingfield="PhysicianID" />
                                        <input type="hidden" value="<%#:Eval("InstructionDate") %>" bindingfield="InstructionDate" />
                                        <input type="hidden" value="<%#:Eval("cfInstructionDatePickerFormat") %>" bindingfield="cfInstructionDatePickerFormat" />
                                        <input type="hidden" value="<%#:Eval("InstructionTime") %>" bindingfield="InstructionTime" />
                                        <input type="hidden" value="<%#:Eval("Description") %>" bindingfield="Description" />
                                        <input type="hidden" value="<%#:Eval("AdditionalText") %>" bindingfield="AdditionalText" />
                                        <input type="hidden" value="<%#:Eval("ExecutedDateTime") %>" bindingfield="ExecutedDateTime" />
                                        <input type="hidden" value="<%#:Eval("ExecutedBy") %>" bindingfield="ExecutedBy" />
                                        <input type="hidden" value="<%#:Eval("cfExecutedByName") %>" bindingfield="ExecutedByName" />
                                        <input type="hidden" value="<%#:Eval("IsCompleted") %>" bindingfield="IsCompleted" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="cfInstructionDate" HeaderText="Date" HeaderStyle-Width="80px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"  />
                                <asp:BoundField DataField="InstructionTime" HeaderText="Date" HeaderStyle-Width="50px" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="InstructionGroup" HeaderText="Instruction Group" HeaderStyle-Width="200px" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                                <asp:BoundField DataField="PhysicianName" HeaderText="Physician" HeaderStyle-Width="250px" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                                <asp:BoundField DataField="Description" HeaderText="Instruction" HeaderStyle-CssClass="description" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" ItemStyle-CssClass="description" />
                            </Columns>
                            <EmptyDataTemplate>
                                <%=GetLabel("No Physician Instruction to display")%>
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
    <dxcp:ASPxCallbackPanel ID="cbpCustomProcess" runat="server" Width="100%" ClientInstanceName="cbpCustomProcess"
        ShowLoadingPanel="false" OnCallback="cbpCustomProcess_Callback">
        <ClientSideEvents BeginCallback="function(s,e) { showLoadingPanel(); }" EndCallback="function(s,e) { onCbpCustomProcesEndCallback(s); }" />
    </dxcp:ASPxCallbackPanel>
</asp:Content>