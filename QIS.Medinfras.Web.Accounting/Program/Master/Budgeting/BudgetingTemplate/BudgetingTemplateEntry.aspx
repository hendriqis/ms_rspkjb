<%@ Page Title="" Language="C#" MasterPageFile="~/libs/MasterPage/MPTrx.master" AutoEventWireup="true"
    CodeBehind="BudgetingTemplateEntry.aspx.cs" Inherits="QIS.Medinfras.Web.Accounting.Program.BudgetingTemplateEntry" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<asp:Content ID="Content2" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div class="menuTitle">
        <%=HttpUtility.HtmlEncode(GetPageTitle())%></div>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript">
        function onLoad() {
            if ($('#<%=hdnIsEditable.ClientID %>').val() == '1') {
                $('#lblAddData').show();
            }
            else {
                $('#lblAddData').hide();
            }

            $('#btnCancel').click(function () {
                $('#containerEntry').hide();
            });

            $('#btnSave').click(function (evt) {
                if (IsValid(evt, 'fsTrxPopup', 'mpTrxPopup'))
                    cbpProcess.PerformCallback('save');
            });

        }

        //#region Add, Edit, Delete
        $('#lblAddData').live('click', function (evt) {
            if (IsValid(evt, 'fsMPEntry', 'mpEntry')) {
                $('#<%=hdnID.ClientID %>').val('');

                $('#<%=hdnGLAccountID.ClientID %>').val('');
                $('#<%=txtGLAccountCode.ClientID %>').val('');
                $('#<%=txtGLAccountName.ClientID %>').val('');

                $('#<%=txtRemarksDt.ClientID %>').val('');


                $('#containerEntry').show();
            }
        });

        $('#<%=grdView.ClientID %> .imgEdit.imgLink').live('click', function () {
            $row = $(this).closest('tr').parent().closest('tr');
            var entity = rowToObject($row);
            $('#<%=hdnID.ClientID %>').val(entity.ID);

            $('#<%=hdnGLAccountID.ClientID %>').val(entity.GLAccountID);
            $('#<%=txtGLAccountCode.ClientID %>').val(entity.GLAccountNo);
            $('#<%=txtGLAccountName.ClientID %>').val(entity.GLAccountName);

            $('#<%=txtRemarksDt.ClientID %>').val(entity.Remarks);

            $('#containerEntry').show();
        });

        $('#<%=grdView.ClientID %> .imgDelete.imgLink').live('click', function () {
            $row = $(this).closest('tr').parent().closest('tr');
            showToastConfirmation('Are You Sure Want To Delete?', function (result) {
                if (result) {
                    var entity = rowToObject($row);
                    $('#<%=hdnID.ClientID %>').val(entity.ID);
                    cbpProcess.PerformCallback('delete');
                }
            });
        });
        //#endregion

        //#region Akun
        function onGetGLAccountFilterExpression() {
            var filterExpression = "IsHeader = 0 AND IsDeleted = 0";
            return filterExpression;
        }

        $('#lblGLAccount.lblLink').live('click', function () {
            openSearchDialog('chartofaccount', onGetGLAccountFilterExpression(), function (value) {
                $('#<%=txtGLAccountCode.ClientID %>').val(value);
                ontxtGLAccountCodeChanged(value);
            });
        });

        $('#<%=txtGLAccountCode.ClientID %>').live('change', function () {
            ontxtGLAccountCodeChanged($(this).val());
        });

        function ontxtGLAccountCodeChanged(value) {
            var filterExpression = onGetGLAccountFilterExpression() + " AND GLAccountNo = '" + value + "'";
            Methods.getObject('GetvChartOfAccountList', filterExpression, function (result) {
                if (result != null) {
                    $('#<%=hdnGLAccountID.ClientID %>').val(result.GLAccountID);
                    $('#<%=txtGLAccountName.ClientID %>').val(result.GLAccountName);

                }
                else {
                    $('#<%=hdnGLAccountID.ClientID %>').val('');
                    $('#<%=txtGLAccountCode.ClientID %>').val('');
                    $('#<%=txtGLAccountName.ClientID %>').val('');
                }
            });
        }
        //#endregion

        function onAfterSaveRecordDtSuccess(BudgetTemplateID) {
            $('#<%=hdnBudgetTemplateID.ClientID %>').val(BudgetTemplateID);
            $('#<%=hdnID.ClientID %>').val();

            $('#<%=hdnGLAccountID.ClientID %>').val('');
            $('#<%=txtGLAccountCode.ClientID %>').val('');
            $('#<%=txtGLAccountName.ClientID %>').val('');

            $('#<%=txtRemarksDt.ClientID %>').val('');

            $('#containerEntry').hide();

            cbpView.PerformCallback('refresh');
        }

        var isAfterAdd = false;
        function onCbpProcesEndCallback(s) {
            hideLoadingPanel();
            var param = s.cpResult.split('|');
            if (param[0] == 'save') {
                if (param[1] == 'fail') {
                    showToast('Save Failed', 'Error Message : ' + param[2]);
                }
                else {
                    isAfterAdd = true;
                    var BudgetHdID = s.cpBudgetID;
                    onAfterSaveRecordDtSuccess(BudgetHdID);
                }
            }
            else if (param[0] == 'delete') {
                if (param[1] == 'fail')
                    showToast('Delete Failed', 'Error Message : ' + param[2]);
                else {
                    isAfterAdd = false;
                    cbpView.PerformCallback('refresh');
                }
            }
        }

        function onCbpViewEndCallback(s) {
            hideLoadingPanel();
        }

        //#region Select Budget
        $('#lblSelectBudgetTemplate.lblLink').live('click', function () {
            var filterExpression = "BudgetTemplateID IS NOT NULL AND IsDeleted = 0";
            openSearchDialog('budgettemplatehd', filterExpression, function (value) {
                $('#<%=hdnBudgetTemplateID.ClientID %>').val(value);
                onHdnBudgetTemplateHdIDChanged(value);
            });
        });

        function onHdnBudgetTemplateHdIDChanged(value) {
            onLoadObject(value);
        }
        //#endregion

        $('.lnkDetailRCC a').live('click', function () {
            var isEditable = $('#<%=hdnIsEditable.ClientID %>').val();
            if (isEditable == "1") {
                var id = $(this).closest('tr').find('.keyField').html();
                var url = ResolveUrl("~/Program/Master/Budgeting/BudgetingTemplate/BudgetingTemplateRCCEntryCtl.ascx");
                openUserControlPopup(url, id, 'Budgeting Template Detail - Revenue Cost Center', 900, 500);
            } else {
                displayErrorMessageBox('GAGAL', "Data master ini sudah tidak dapat diubah lagi.");
            }
        });

        function onAfterSaveAddRecordEntryPopup(param) {
            $('#<%=hdnBudgetTemplateID.ClientID %>').val(param);
            cbpView.PerformCallback('refresh');
        }

        function onAfterPopupControlClosing() {
            var BudgetTemplateID = $('#<%=hdnBudgetTemplateID.ClientID %>').val();
            onHdnBudgetTemplateHdIDChanged(BudgetTemplateID);
        }
    </script>
    <input type="hidden" id="hdnPageTitle" runat="server" />
    <input type="hidden" id="hdnIsEditable" runat="server" value="" />
    <input type="hidden" id="hdnBudgetTemplateID" runat="server" value="" />
    <input type="hidden" id="hdnGLAccountID" runat="server" value="" />
    <table class="tblContentArea">
        <colgroup>
            <col style="width: 50%" />
            <col style="width: 50%" />
        </colgroup>
        <tr>
            <td style="padding: 5px; vertical-align: top">
                <table class="tblEntryContent" style="width: 100%">
                    <colgroup>
                        <col style="width: 100px" />
                        <col />
                    </colgroup>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblMandatory lblLink" id="lblSelectBudgetTemplate">
                                <%=GetLabel("Kode Template")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtBudgetTemplateCode" Width="100px" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblMandatory">
                                <%=GetLabel("Nama Template")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtBudgetTemplateName" Width="300px" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel" style="width: 150px; vertical-align: top; padding-top: 5px;">
                            <label class="lblNormal">
                                <%=GetLabel("Keterangan")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtRemarks" Width="100%" runat="server" TextMode="MultiLine" Rows="2" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <div id="containerEntry" style="margin-top: 4px; display: none;">
                    <div class="pageTitle">
                        <%=GetLabel("Edit atau Tambah Data")%></div>
                    <fieldset id="fsTrxPopup" style="margin: 0">
                        <input type="hidden" value="" id="hdnID" runat="server" />
                        <table style="width: 100%" class="tblEntryDetail">
                            <colgroup>
                                <col style="width: 50%" />
                                <col style="width: 50%" />
                            </colgroup>
                            <tr>
                                <td align="left">
                                    <table width="100%">
                                        <colgroup>
                                            <col style="width: 200px;" />
                                            <col style="width: 120px;" />
                                            <col style="width: 150px;" />
                                            <col />
                                        </colgroup>
                                        <tr>
                                            <td>
                                                <label class="lblLink lblMandatory" id="lblGLAccount" style="width: 100%">
                                                    <%=GetLabel("COA")%></label>
                                            </td>
                                            <td>
                                                <asp:TextBox runat="server" ID="txtGLAccountCode" Width="100%" />
                                            </td>
                                            <td>
                                                <asp:TextBox runat="server" ID="txtGLAccountName" Width="100%" ReadOnly="true" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <label class="lblNormal" style="width: 100%">
                                                    <%=GetLabel("Keterangan Detail")%></label>
                                            </td>
                                            <td colspan="2">
                                                <asp:TextBox ID="txtRemarksDt" runat="server" Width="100%" TextMode="MultiLine" Rows="2" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                            </td>
                                            <td colspan="2" style="width: 100%">
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <input type="button" id="btnSave" style="width: 60px;" value='<%= GetLabel("Save")%>' />
                                                        </td>
                                                        <td>
                                                            <input type="button" id="btnCancel" style="width: 60px;" value='<%= GetLabel("Cancel")%>' />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                </div>
                <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
                    ShowLoadingPanel="false" OnCallback="cbpView_Callback">
                    <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ onCbpViewEndCallback(s); }" />
                    <PanelCollection>
                        <dx:PanelContent ID="PanelContent1" runat="server">
                            <asp:Panel runat="server" ID="pnlView" Style="width: 100%; margin-left: auto; margin-right: auto;
                                position: relative;">
                                <input type="hidden" value="0" id="hdnDisplayCount" runat="server" />
                                <asp:GridView ID="grdView" runat="server" CssClass="grdNormal notAllowSelect" AutoGenerateColumns="false"
                                    ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                    <Columns>
                                        <asp:BoundField DataField="ID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                        <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderStyle-Width="50px">
                                            <ItemTemplate>
                                                <table cellpadding="0" cellspacing="0">
                                                    <tr>
                                                        <td>
                                                            <img class="imgEdit <%# IsEditable() == "0" ? "imgDisabled" : "imgLink"%>" title='<%=GetLabel("Edit")%>'
                                                                src='<%# IsEditable() == "0" ? ResolveUrl("~/Libs/Images/Button/edit_disabled.png") : ResolveUrl("~/Libs/Images/Button/edit.png")%>'
                                                                alt="" />
                                                        </td>
                                                        <td style="width: 1px">
                                                            &nbsp;
                                                        </td>
                                                        <td>
                                                            <img class="imgDelete <%# IsEditable() == "0" ? "imgDisabled" : "imgLink"%>" title='<%=GetLabel("Delete")%>'
                                                                src='<%# IsEditable() == "0" ? ResolveUrl("~/Libs/Images/Button/delete_disabled.png") : ResolveUrl("~/Libs/Images/Button/delete.png")%>'
                                                                alt="" />
                                                        </td>
                                                    </tr>
                                                </table>
                                                <input type="hidden" value="<%#:Eval("ID") %>" bindingfield="ID" />
                                                <input type="hidden" value="<%#:Eval("BudgetTemplateID") %>" bindingfield="BudgetTemplateID" />
                                                <input type="hidden" value="<%#:Eval("GLAccountID") %>" bindingfield="GLAccountID" />
                                                <input type="hidden" value="<%#:Eval("GLAccountNo") %>" bindingfield="GLAccountNo" />
                                                <input type="hidden" value="<%#:Eval("GLAccountName") %>" bindingfield="GLAccountName" />
                                                <input type="hidden" value="<%#:Eval("BudgetAmount") %>" bindingfield="BudgetAmount" />
                                                <input type="hidden" value="<%#:Eval("Remarks") %>" bindingfield="Remarks" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderStyle-Width="200px" HeaderStyle-HorizontalAlign="Left">
                                            <HeaderTemplate>
                                                <%=GetLabel("COA")%></HeaderTemplate>
                                            <ItemTemplate>
                                                <div style="font-size: 14px;">
                                                    <%#:Eval("GLAccountNo") %></div>
                                                <div style="font-size: 12px;">
                                                    <%#:Eval("GLAccountName") %></div>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right"
                                            HeaderStyle-Width="200px">
                                            <HeaderTemplate>
                                                <%=GetLabel("Nilai Anggaran") %></HeaderTemplate>
                                            <ItemTemplate>
                                                <%#:Eval("BudgetAmount", "{0:N}")%>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="Remarks" HeaderText="Keterangan" HeaderStyle-HorizontalAlign="Left" />
                                        <asp:TemplateField HeaderStyle-Width="200px" HeaderStyle-HorizontalAlign="Left">
                                            <HeaderTemplate>
                                                <%=GetLabel("Informasi Dibuat")%></HeaderTemplate>
                                            <ItemTemplate>
                                                <div style="font-size: 14px;">
                                                    <%#:Eval("CreatedByName") %></div>
                                                <div style="font-size: 10px;">
                                                    <%#:Eval("cfCreatedDateInString") %></div>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderStyle-Width="200px" HeaderStyle-HorizontalAlign="Left">
                                            <HeaderTemplate>
                                                <%=GetLabel("Informasi Diubah")%></HeaderTemplate>
                                            <ItemTemplate>
                                                <div style="font-size: 14px;">
                                                    <%#:Eval("LastUpdatedByName") %></div>
                                                <div style="font-size: 10px;">
                                                    <%#:Eval("cfLastUpdatedDateInString") %></div>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:HyperLinkField HeaderText="Detail RCC" HeaderStyle-Width="120px" Text="Detail RCC"
                                            ItemStyle-CssClass="lnkDetailRCC" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" />
                                    </Columns>
                                    <EmptyDataTemplate>
                                        <%=GetLabel("No Data To Display")%>
                                    </EmptyDataTemplate>
                                </asp:GridView>
                            </asp:Panel>
                        </dx:PanelContent>
                    </PanelCollection>
                </dxcp:ASPxCallbackPanel>
                <div class="imgLoadingGrdView" id="containerImgLoadingView">
                    <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                </div>
                <div style="width: 100%; text-align: center">
                    <div style="width: 100%; text-align: center">
                        <span class="lblLink" id="lblAddData">
                            <%= GetLabel("Tambah Data")%></span>
                    </div>
                </div>
                <div>
                    <table width="100%">
                        <tr>
                            <td>
                                <div style="width: 450px;">
                                    <div class="lblComponent" style="text-align: left; padding-left: 3px">
                                        <%=GetLabel("Informasi") %></div>
                                    <div style="background-color: #EAEAEA;">
                                        <table width="400px" cellpadding="0" cellspacing="0">
                                            <colgroup>
                                                <col width="150px" />
                                                <col width="10px" />
                                                <col />
                                            </colgroup>
                                            <tr>
                                                <td align="left">
                                                    <%=GetLabel("Dibuat Oleh") %>
                                                </td>
                                                <td align="center">
                                                    <%=GetLabel(":") %>
                                                </td>
                                                <td>
                                                    <div runat="server" id="divCreatedBy">
                                                    </div>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left">
                                                    <%=GetLabel("Dibuat Pada") %>
                                                </td>
                                                <td align="center">
                                                    <%=GetLabel(":") %>
                                                </td>
                                                <td>
                                                    <div runat="server" id="divCreatedDate">
                                                    </div>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left">
                                                    <%=GetLabel("Terakhir Diubah Oleh") %>
                                                </td>
                                                <td align="center">
                                                    <%=GetLabel(":") %>
                                                </td>
                                                <td>
                                                    <div runat="server" id="divLastUpdatedBy">
                                                    </div>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left">
                                                    <%=GetLabel("Terakhir Diubah Pada")%>
                                                </td>
                                                <td align="center">
                                                    <%=GetLabel(":") %>
                                                </td>
                                                <td>
                                                    <div runat="server" id="divLastUpdatedDate">
                                                    </div>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </div>
                            </td>
                        </tr>
                    </table>
                </div>
            </td>
        </tr>
    </table>
    <dxcp:ASPxCallbackPanel ID="cbpProcess" runat="server" Width="100%" ClientInstanceName="cbpProcess"
        ShowLoadingPanel="false" OnCallback="cbpProcess_Callback">
        <ClientSideEvents BeginCallback="function(s,e) { showLoadingPanel(); }" EndCallback="function(s,e) { onCbpProcesEndCallback(s); }" />
    </dxcp:ASPxCallbackPanel>
</asp:Content>
