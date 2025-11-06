<%@ Page Language="C#" MasterPageFile="~/MasterPage/MPPatientPageListEntry.master"
    AutoEventWireup="true" CodeBehind="UnverifyNotesList.aspx.cs" Inherits="QIS.Medinfras.Web.EMR.Program.UnverifyNotesList" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<asp:Content ID="Content2" ContentPlaceHolderID="plhCustomButtonToolbar" runat="server">
    <li id="btnUnverify" runat="server" crudmode="R">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbset.png")%>' alt="" /><div>
            <%=GetLabel("Unverify")%></div>
    </li>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhRightToolbar" runat="server">
    <table>
        <tr>
            <td>
                <%=GetLabel("Notes View Type") %>
            </td>
            <td>
                <asp:DropDownList ID="ddlViewType" runat="server">
                    <asp:ListItem Text="All" Value="0" Selected="True" />
                    <asp:ListItem Text="Physician Notes Only" Value="1" />
                    <asp:ListItem Text="Nursing and Other Paramedic Notes Only" Value="2" />
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

            $('#<%=btnUnverify.ClientID %>').click(function () {
                if (!getSelectedCheckbox()) {
                    showToast("ERROR", 'Error Message : ' + "Please select the item to be process !");
                }
                else {
                    var message = "Are you sure to unverify notes ?";
                    showToastConfirmation(message, function (result) {
                        if (result) cbpCustomProcess.PerformCallback('unverify');
                    });
                }
            });

            $('#<%=ddlViewType.ClientID %>').change(function () {
                cbpView.PerformCallback('refresh');
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
            $('#<%=grdView.ClientID %> .chkIsProcessItem input:checked').each(function () {
                var $tr = $(this).closest('tr');
                var id = $(this).closest('tr').find('.keyField').html();

                if (tempSelectedID != "") {
                    tempSelectedID += ",";
                }
                tempSelectedID += id;
            });
            if (tempSelectedID != "") {
                $('#<%=hdnSelectedID.ClientID %>').val(tempSelectedID);
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

        //#region Entity To Control
        function entityToControl(entity) {
            $('#<%=grdView.ClientID %> tr.selected').removeClass('selected');
            if (entity != null) {
                $('#<%=grdView.ClientID %> tr.focus').addClass('selected');
                $('#<%=hdnEntryID.ClientID %>').val(entity.ID);
            }
            else {
                var currentdate = getDateNowDatePickerFormat();
                var currenttime = getTimeNow();
                $('#<%=hdnEntryID.ClientID %>').val('');
            }
        }
        //#endregion

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
    </script>
</asp:Content>
<asp:Content ID="ctnEntry" ContentPlaceHolderID="plhEntry" runat="server">
    <input type="hidden" value="" id="hdnEntryID" runat="server" />
    <table class="tblEntryDetail">
        <colgroup>
            <col style="width: 150px" />
            <col style="width: 150px" />
            <col />
        </colgroup>
    </table>
</asp:Content>
<asp:Content ID="ctnList" ContentPlaceHolderID="plhList" runat="server">
    <input type="hidden" value="" id="hdnID" runat="server" />
    <input type="hidden" id="hdnFilterExpression" runat="server" value="" />
    <input type="hidden" value="" id="hdnDefaultParamedicID" runat="server" />
    <input type="hidden" value="" id="hdnSelectedID" runat="server" />
    <div style="position: relative;">
        <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
            ShowLoadingPanel="false" OnCallback="cbpView_Callback">
            <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingView').show(); }"
                EndCallback="function(s,e){ onCbpViewEndCallback(s); }" />
            <PanelCollection>
                <dx:PanelContent ID="PanelContent1" runat="server">
                    <asp:Panel runat="server" ID="pnlView" CssClass="pnlContainerGrid" Style="height: 300px">
                        <asp:GridView ID="grdView" runat="server" CssClass="grdSelected grdPatientPage" AutoGenerateColumns="false"
                            ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                            <Columns>
                                <asp:BoundField DataField="ID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                <asp:TemplateField HeaderText="" HeaderStyle-HorizontalAlign="center" HeaderStyle-Width="30px"
                                    ItemStyle-HorizontalAlign="center">
                                    <HeaderTemplate>
                                        <input id="chkSelectAll" type="checkbox" />
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkIsProcessItem" runat="server" CssClass="chkIsProcessItem" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderStyle-CssClass="hiddenColumn" ItemStyle-CssClass="hiddenColumn">
                                    <ItemTemplate>
                                        <input type="hidden" value="<%#:Eval("ID") %>" bindingfield="ID" />
                                        <input type="hidden" value="<%#:Eval("ParamedicID") %>" bindingfield="ParamedicID" />
                                        <input type="hidden" value="<%#:Eval("NoteDate") %>" bindingfield="NoteDate" />
                                        <input type="hidden" value="<%#:Eval("NoteDateInDatePickerFormat") %>" bindingfield="NoteDateInDatePickerFormat" />
                                        <input type="hidden" value="<%#:Eval("NoteTime") %>" bindingfield="NoteTime" />
                                        <input type="hidden" value="<%#:Eval("NoteText") %>" bindingfield="NoteText" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="cfNoteDate" HeaderText="Tanggal " HeaderStyle-Width="100px"
                                    HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="cfNoteDate" />
                                <asp:BoundField DataField="NoteTime" HeaderText="Jam " HeaderStyle-Width="60px" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="Center" ItemStyle-CssClass="noteTime" />
                                <asp:TemplateField HeaderText="PPA" HeaderStyle-Width="40px" HeaderStyle-HorizontalAlign="Center"
                                    ItemStyle-HorizontalAlign="Center">
                                    <ItemTemplate>
                                        <div>
                                            <%#:Eval("cfPPA") %>
                                        </div>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="SOAP" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left">
                                    <ItemTemplate>
                                        <div>
                                            <span style="color: blue; font-style: italic; vertical-align: top">
                                                <%#:Eval("ParamedicName") %>
                                                - <b>
                                                    <%#:Eval("DepartmentID") %>
                                                    (<%#:Eval("ServiceUnitName") %>)
                                                    <%#:Eval("cfParamedicMasterType") %>
                                                </b></span>
                                        </div>
                                        <div style="height: 130px; overflow-y: auto; margin-top: 15px;">
                                            <%#Eval("NoteText").ToString().Replace("\n","<br />")%><br />
                                        </div>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Instruksi" HeaderStyle-Width="350px" HeaderStyle-HorizontalAlign="Left"
                                    ItemStyle-HorizontalAlign="Left">
                                    <ItemTemplate>
                                        <div>
                                            <textarea style="padding-left: 10px; border: 0; width: 99%; height: 200px; background-color: transparent"
                                                readonly><%#: DataBinder.Eval(Container.DataItem, "InstructionText") %> </textarea>
                                        </div>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                    HeaderStyle-Width="150px">
                                    <ItemTemplate>
                                        <div style="color: blue; font-style: italic; vertical-align: top">
                                            <%#:Eval("cfCreatedDate") %>,
                                        </div>
                                        <div>
                                            <b><label id="lblParamedicName" class='<%# Eval("cfIsHasSignature1").ToString() == "True" ? "lblLink lblParamedicName": "lblNormal" %>'>
                                                <%#:Eval("cfCreatedByName") %></label></b>                                            
                                        </div>
                                            <div id="divVerifiedInformation" runat="server" style="margin-top: 10px; text-align: left">
                                                <div style="white-space: normal; overflow: auto; font-weight: bold">
                                                    <span style='color: red;'>Diverifikasi :</span>
                                                    <br />
                                                    <span style='color: Blue;'>
                                                        <%#:Eval("cfVerifiedDateTime") %>, <%#:Eval("VerifiedPhysicianName") %></span>
                                                        <div id="divVerificationRemarks">
                                                            <br />
                                                            <textarea style="border: 0; width: 100%; height: auto; background-color: transparent; font-style:italic "
                                                                readonly><%#: DataBinder.Eval(Container.DataItem, "VerificationRemarks") %> </textarea>
                                                        </div>
                                                </div>
                                            </div>
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
        <div class="imgLoadingGrdView" id="containerImgLoadingView">
            <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
        </div>
        <div class="containerPaging">
            <div class="wrapperPaging">
                <div id="paging">
                </div>
            </div>
        </div>
    </div>
    <dxcp:ASPxCallbackPanel ID="cbpCustomProcess" runat="server" Width="100%" ClientInstanceName="cbpCustomProcess"
        ShowLoadingPanel="false" OnCallback="cbpCustomProcess_Callback">
        <ClientSideEvents BeginCallback="function(s,e) { showLoadingPanel(); }" EndCallback="function(s,e) { onCbpCustomProcesEndCallback(s); }" />
    </dxcp:ASPxCallbackPanel>
</asp:Content>
