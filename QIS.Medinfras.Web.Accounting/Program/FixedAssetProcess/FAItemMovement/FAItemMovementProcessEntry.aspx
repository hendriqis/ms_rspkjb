<%@ Page Title="" Language="C#" MasterPageFile="~/libs/MasterPage/MPTrx2.master"
    AutoEventWireup="true" CodeBehind="FAItemMovementProcessEntry.aspx.cs" Inherits="QIS.Medinfras.Web.Accounting.Program.FAItemMovementProcessEntry" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
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

            if ($('#<%=hdnMovementID.ClientID %>').val() == "" || $('#<%=hdnMovementID.ClientID %>').val() == "0") {
                setDatePicker('<%=txtMovementDate.ClientID %>');
            }

            $('#<%=txtMovementDate.ClientID %>').datepicker('option', 'maxDate', new Date(getDateNow()));

            var pageCount = parseInt($('#<%=hdnPageCount.ClientID %>').val());
            setPaging($("#paging"), pageCount, function (page) {
                cbpView.PerformCallback('changepage|' + page);
            });
        }

        //#region MovementNo
        $('#<%=lblMovementNo.ClientID %>.lblLink').live('click', function () {
            openSearchDialog('faitemmovementhd', "MovementID IS NOT NULL", function (value) {
                $('#<%=txtMovementNo.ClientID %>').val(value);
                ontxtMovementNoChanged(value);
            });
        });

        $('#<%=txtMovementNo.ClientID %>').change(function () {
            ontxtMovementNoChanged($(this).val());
        });

        function ontxtMovementNoChanged(value) {
            onLoadObject(value);
        }
        //#endregion

        function getFALocationFilterExpression() {
            return "IsDeleted = 0";
        }

        //#region FALocationFrom
        $('#<%=lblFALocationFrom.ClientID %>.lblLink').live('click', function () {
            openSearchDialog('falocation', getFALocationFilterExpression(), function (value) {
                $('#<%=txtFALocationCodeFrom.ClientID %>').val(value);
                ontxtFALocationCodeFromChanged(value);
            });
        });

        $('#<%=txtFALocationCodeFrom.ClientID %>').live('change', function () {
            ontxtFALocationCodeFromChanged($(this).val());
        });

        function ontxtFALocationCodeFromChanged(value) {
            var filterExpression = getFALocationFilterExpression() + " AND FALocationCode = '" + value + "'";
            Methods.getObject('GetFALocationList', filterExpression, function (result) {
                if (result != null) {
                    $('#<%=hdnFALocationIDFrom.ClientID %>').val(result.FALocationID);
                    $('#<%=txtFALocationNameFrom.ClientID %>').val(result.FALocationName);
                }
                else {
                    $('#<%=hdnFALocationIDFrom.ClientID %>').val("");
                    $('#<%=txtFALocationCodeFrom.ClientID %>').val("");
                    $('#<%=txtFALocationNameFrom.ClientID %>').val("");
                }
            });
        }
        //#endregion

        //#region FALocationTo
        $('#<%=lblFALocationTo.ClientID %>.lblLink').live('click', function () {
            openSearchDialog('falocation', getFALocationFilterExpression(), function (value) {
                $('#<%=txtFALocationCodeTo.ClientID %>').val(value);
                ontxtFALocationCodeToChanged(value);
            });
        });

        $('#<%=txtFALocationCodeTo.ClientID %>').live('change', function () {
            ontxtFALocationCodeToChanged($(this).val());
        });

        function ontxtFALocationCodeToChanged(value) {
            var filterExpression = getFALocationFilterExpression() + " AND FALocationCode = '" + value + "'";
            Methods.getObject('GetFALocationList', filterExpression, function (result) {
                if (result != null) {
                    $('#<%=hdnFALocationIDTo.ClientID %>').val(result.FALocationID);
                    $('#<%=txtFALocationNameTo.ClientID %>').val(result.FALocationName);
                }
                else {
                    $('#<%=hdnFALocationIDTo.ClientID %>').val("");
                    $('#<%=txtFALocationCodeTo.ClientID %>').val("");
                    $('#<%=txtFALocationNameTo.ClientID %>').val("");
                }
            });
        }
        //#endregion

        $('#btnCancel').live('click', function () {
            $('#containerEntry').hide();
        });

        $('#btnSave').live('click', function (evt) {
            if (IsValid(evt, 'fsTrxPopup', 'mpTrxPopup'))
                cbpProcess.PerformCallback('save');
        });

        $('#lblAddData').live('click', function (evt) {
            if (IsValid(evt, 'fsMPEntry', 'mpEntry')) {
                $('#<%=hdnIsEdit.ClientID %>').val('0');
                $('#<%=hdnID.ClientID %>').val("");
                $('#<%=hdnFAItemID.ClientID %>').val("");
                $('#<%=txtFAItemCode.ClientID %>').val("");
                $('#<%=txtFAItemName.ClientID %>').val("");
                $('#<%=txtReferenceNo.ClientID %>').val("");

                $('#containerEntry').show();
            }
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

        $('#<%=grdView.ClientID %> .imgEdit.imgLink').live('click', function () {
            $row = $(this).closest('tr').parent().closest('tr');
            var entity = rowToObject($row);
            $('#<%=hdnIsEdit.ClientID %>').val('1');
            $('#<%=hdnID.ClientID %>').val(entity.ID);
            $('#<%=hdnFAItemID.ClientID %>').val(entity.FixedAssetID);
            $('#<%=txtFAItemCode.ClientID %>').val(entity.FixedAssetCode);
            $('#<%=txtFAItemName.ClientID %>').val(entity.FixedAssetName);
            $('#<%=txtReferenceNo.ClientID %>').val(entity.ReferenceNo);

            $('#containerEntry').show();
        });
        //#endregion

        //#region FAItem
        function onGetFAItemFilterExpression() {
            var movementID = $('#<%=hdnMovementID.ClientID %>').val();
            var filterFAItem = "IsDeleted = 0 AND GCItemStatus != '" + Constant.ItemStatus.ITEM_STATUS_IN_ACTIVE + "'" + " AND FALocationID = " + $('#<%=hdnFALocationIDFrom.ClientID %>').val();

            if (movementID != 0) {
                filterFAItem += " AND FixedAssetID NOT IN (SELECT fmd.FixedAssetID FROM FAItemMovementDt fmd WITH(NOLOCK) INNER JOIN FAItemMovementHd fmh WITH(NOLOCK) ON fmh.MovementID = fmd.MovementID WHERE fmh.GCTransactionStatus = 'X121^001' AND fmd.IsDeleted = 0 AND fmh.MovementID = " + $('#<%=hdnMovementID.ClientID %>').val() + ")";
            }
            else {
                filterFAItem += " AND FixedAssetID NOT IN (SELECT fmd.FixedAssetID FROM FAItemMovementDt fmd WITH(NOLOCK) INNER JOIN FAItemMovementHd fmh WITH(NOLOCK) ON fmh.MovementID = fmd.MovementID WHERE fmh.GCTransactionStatus = 'X121^001' AND fmd.IsDeleted = 0)";
            }
            return filterFAItem;
        }

        $('#<%=lblFAItem.ClientID %>.lblLink').live('click', function () {
            openSearchDialog('faitem', onGetFAItemFilterExpression(), function (value) {
                $('#<%=txtFAItemCode.ClientID %>').val(value);
                ontxtFAItemCodeChanged(value);
            });
        });

        $('#<%=txtFAItemCode.ClientID %>').live('change', function () {
            ontxtFAItemCodeChanged($(this).val());
        });

        function ontxtFAItemCodeChanged(value) {
            var filterExpression = onGetFAItemFilterExpression() + " AND FixedAssetCode = '" + value + "'";
            Methods.getObject('GetFAItemList', filterExpression, function (result) {
                if (result != null) {
                    $('#<%=hdnFAItemID.ClientID %>').val(result.FixedAssetID);
                    $('#<%=txtFAItemName.ClientID %>').val(result.FixedAssetName);
                }
                else {
                    $('#<%=hdnFAItemID.ClientID %>').val("");
                    $('#<%=txtFAItemCode.ClientID %>').val("");
                    $('#<%=txtFAItemName.ClientID %>').val("");
                }
            });
        }
        //#endregion

        function onAfterSaveRecordDtSuccess(MovementID) {
            if ($('#<%=hdnMovementID.ClientID %>').val() == '0') {
                $('#<%=hdnMovementID.ClientID %>').val(MovementID);
                var filterExpression = 'MovementID = ' + MovementID;
                Methods.getObject('GetFAItemMovementHdList', filterExpression, function (result) {
                    $('#<%=txtMovementNo.ClientID %>').val(result.MovementNo);
                    cbpView.PerformCallback('refresh');
                });
                onAfterCustomSaveSuccess();
            }
            else {
                cbpView.PerformCallback('refresh');
            }
        }

        function onAfterSaveAddRecordEntryPopup(param) {
            onAfterSaveRecordDtSuccess(param);
        }

        function onCbpProcesEndCallback(s) {
            hideLoadingPanel();
            var param = s.cpResult.split('|');
            var MovementID = s.cpOrderID;
            if (param[0] == 'save') {
                if (param[1] == 'fail') {
                    showToast('Save Failed', 'Error Message : ' + param[2]);
                }
                else {
                    onAfterSaveRecordDtSuccess(MovementID);
                    var isEdit = $('#<%=hdnIsEdit.ClientID %>').val();
                    if (isEdit == '1') {
                        $('#containerEntry').hide();
                    } else if (isEdit == '0') {
                        $('#lblAddData').click();
                    }
                    onLoadObject(MovementID);
                }
            }
            else if (param[0] == 'delete') {
                if (param[1] == 'fail') {
                    showToast('Delete Failed', 'Error Message : ' + param[2]);
                }
                else {
                    $('#containerEntry').hide();
                    onLoadObject(MovementID);
                }
            }
            $('#containerEntry').hide();
        }

        //#region Paging
        function onCbpViewEndCallback(s) {
            hideLoadingPanel();

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

        function onBeforeRightPanelPrint(code, filterExpression, errMessage) {
            var movementID = $('#<%=hdnMovementID.ClientID %>').val();
            var GCTransactionStatus = $('#<%=hdnGCTransactionStatus.ClientID%>').val();

            if (movementID == '' || movementID == '0') {
                errMessage.text = 'Harap pilih transaksi terlebih dahulu.';
                return false;
            }
            else {
                if (GCTransactionStatus != Constant.TransactionStatus.OPEN && GCTransactionStatus != Constant.TransactionStatus.VOID) {
                    if (code == 'AC-00003' || code == 'AC-00007' || code == 'AC-00008') {
                        filterExpression.text = "MovementID = " + movementID;
                        return true;
                    }
                } else {
                    errMessage.text = "Transaksi harus Approve terlebih dahulu sebelum proses bisa dilakukan.";
                    return false;
                }
            }
        }

    </script>
    <input type="hidden" value="" id="hdnPageTitle" runat="server" />
    <input type="hidden" value="" id="hdnPrintStatus" runat="server" />
    <input type="hidden" value="" id="hdnMovementID" runat="server" />
    <input type="hidden" value="" id="hdnGCTransactionStatus" runat="server" />
    <input type="hidden" value="" id="hdnPageCount" runat="server" />
    <input type="hidden" value="1" id="hdnIsEditable" runat="server" />
    <input type="hidden" value="0" id="hdnIsEdit" runat="server" />
    <div style="overflow-y: auto; overflow-x: hidden;">
        <table class="tblContentArea">
            <colgroup>
                <col style="width: 50%" />
                <col style="width: 50%" />
            </colgroup>
            <tr>
                <td style="padding: 5px; vertical-align: top">
                    <table class="tblEntryContent" style="width: 100%">
                        <colgroup>
                            <col style="width: 30%" />
                            <col />
                        </colgroup>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblLink" id="lblMovementNo" runat="server">
                                    <%=GetLabel("No. Mutasi")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtMovementNo" Width="100%" ReadOnly="true" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <%=GetLabel("Tanggal Mutasi") %>
                            </td>
                            <td style="padding-right: 1px; width: 145px">
                                <asp:TextBox ID="txtMovementDate" Width="120px" CssClass="datepicker" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblMandatory lblLink" runat="server" id="lblFALocationFrom">
                                    <%=GetLabel("Dari Lokasi")%></label>
                            </td>
                            <td>
                                <input type="hidden" id="hdnFALocationIDFrom" value="" runat="server" />
                                <table style="width: 100%" cellpadding="0" cellspacing="0">
                                    <colgroup>
                                        <col style="width: 30%" />
                                        <col style="width: 3px" />
                                        <col />
                                    </colgroup>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtFALocationCodeFrom" Width="100%" runat="server" />
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtFALocationNameFrom" Width="100%" runat="server" ReadOnly="true" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblMandatory lblLink" runat="server" id="lblFALocationTo">
                                    <%=GetLabel("Kepada Lokasi")%></label>
                            </td>
                            <td>
                                <input type="hidden" id="hdnFALocationIDTo" value="" runat="server" />
                                <table style="width: 100%" cellpadding="0" cellspacing="0">
                                    <colgroup>
                                        <col style="width: 30%" />
                                        <col style="width: 3px" />
                                        <col />
                                    </colgroup>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtFALocationCodeTo" Width="100%" runat="server" />
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtFALocationNameTo" Width="100%" runat="server" ReadOnly="true" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblMandatory">
                                    <%=GetLabel("Jenis Mutasi")%></label>
                            </td>
                            <td>
                                <dxe:ASPxComboBox runat="server" ID="cboMovementType" ClientInstanceName="cboMovementType"
                                    Width="100%">
                                </dxe:ASPxComboBox>
                            </td>
                        </tr>
                    </table>
                </td>
                <td style="padding: 5px; vertical-align: top">
                    <table class="tblEntryContent" style="width: 100%">
                        <colgroup>
                            <col style="width: 30%" />
                            <col />
                        </colgroup>
                        <tr>
                            <td class="tdLabel" style="vertical-align: top">
                                <%=GetLabel("Keterangan") %>
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
                            <%=GetLabel("Edit/Tambah Aset dan Inventaris")%></div>
                        <fieldset id="fsTrxPopup" style="margin: 0">
                            <input type="hidden" value="" id="hdnID" runat="server" />
                            <table style="width: 100%" class="tblEntryDetail">
                                <colgroup>
                                    <col style="width: 150px" />
                                    <col style="width: 200px" />
                                    <col style="width: 5px" />
                                    <col style="width: 500px" />
                                    <col style="width: 200px" />
                                    <col />
                                </colgroup>
                                <tr>
                                    <td class="tdLabel">
                                        <input type="hidden" value="" id="hdnFAItemID" runat="server" />
                                        <label class="lblLink lblMandatory" id="lblFAItem" runat="server">
                                            <%=GetLabel("Aset dan Inventaris")%></label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtFAItemCode" Width="100%" runat="server" />
                                    </td>
                                    <td>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtFAItemName" Width="100%" ReadOnly="true" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel">
                                        <label class="lblNormal" id="lblNoReferensi" runat="server">
                                            <%=GetLabel("No. Referensi")%></label>
                                    </td>
                                    <td>
                                        <asp:TextBox Width="100%" ID="txtReferenceNo" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                    </td>
                                    <td colspan="5">
                                        <table>
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
                    <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
                        ShowLoadingPanel="false" OnCallback="cbpView_Callback">
                        <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ onCbpViewEndCallback(s); }" />
                        <PanelCollection>
                            <dx:PanelContent ID="PanelContent1" runat="server">
                                <asp:Panel runat="server" ID="pnlView" Style="width: 100%; margin-left: auto; margin-right: auto;
                                    position: relative; font-size: 0.95em;">
                                    <asp:GridView ID="grdView" runat="server" CssClass="grdService grdNormal notAllowSelect"
                                        AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                        <Columns>
                                            <asp:BoundField DataField="ID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                            <asp:TemplateField HeaderStyle-Width="80px" ItemStyle-HorizontalAlign="Center">
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
                                                    <input type="hidden" value="<%#:Eval("FixedAssetID") %>" bindingfield="FixedAssetID" />
                                                    <input type="hidden" value="<%#:Eval("FixedAssetCode") %>" bindingfield="FixedAssetCode" />
                                                    <input type="hidden" value="<%#:Eval("FixedAssetName") %>" bindingfield="FixedAssetName" />
                                                    <input type="hidden" value="<%#:Eval("ReferenceNo") %>" bindingfield="ReferenceNo" />
                                                    <input type="hidden" value="<%#:Eval("IsDeleted") %>" bindingfield="IsDeleted" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="FixedAssetCode" HeaderText="Kode Aset dan Inventaris"
                                                HeaderStyle-Width="200px" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                                            <asp:BoundField DataField="FixedAssetName" HeaderText="Nama Aset dan Inventaris"
                                                HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                                            <asp:BoundField DataField="ReferenceNo" HeaderText="No. Referensi" HeaderStyle-Width="200px"
                                                HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                                            <asp:TemplateField HeaderText="Informasi Dibuat" HeaderStyle-Width="150px" HeaderStyle-HorizontalAlign="Center"
                                                ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <table cellpadding="0" cellspacing="0">
                                                        <tr>
                                                            <td class="tdLabel" align="center">
                                                                <label>
                                                                    <%#:Eval("CreatedByName") %></label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="tdLabel" align="center">
                                                                <label>
                                                                    <%#:Eval("cfCreatedDateInString") %></label>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Informasi Diubah" HeaderStyle-Width="150px" HeaderStyle-HorizontalAlign="Center"
                                                ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <table cellpadding="0" cellspacing="0">
                                                        <tr>
                                                            <td class="tdLabel" align="center">
                                                                <label>
                                                                    <%#:Eval("LastUpdatedByName") %></label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="tdLabel" align="center">
                                                                <label>
                                                                    <%#:Eval("cfLastUpdatedDateInString") %></label>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </ItemTemplate>
                                            </asp:TemplateField>
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
                    <div class="containerPaging">
                        <div class="wrapperPaging">
                            <div id="paging">
                            </div>
                        </div>
                    </div>
                    <div style="width: 100%; text-align: center">
                        <span class="lblLink" id="lblAddData">
                            <%= GetLabel("Add Data")%></span>
                    </div>
                    <div>
                        <table width="100%">
                            <tr>
                                <td>
                                    <div style="width: 600px;">
                                        <div class="pageTitle" style="text-align: center">
                                            <%=GetLabel("Informasi")%></div>
                                        <div style="background-color: #EAEAEA;">
                                            <table width="600px" cellpadding="0" cellspacing="0">
                                                <colgroup>
                                                    <col width="150px" />
                                                    <col width="30px" />
                                                </colgroup>
                                                <tr>
                                                    <td align="left">
                                                        <%=GetLabel("Dibuat Oleh") %>
                                                    </td>
                                                    <td align="center">
                                                        :
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
                                                        :
                                                    </td>
                                                    <td>
                                                        <div runat="server" id="divCreatedDate">
                                                        </div>
                                                    </td>
                                                </tr>
                                                <tr id="trApprovedBy" runat="server">
                                                    <td align="left">
                                                        <%=GetLabel("Approved Oleh") %>
                                                    </td>
                                                    <td align="center">
                                                        :
                                                    </td>
                                                    <td>
                                                        <div runat="server" id="divApprovedBy">
                                                        </div>
                                                    </td>
                                                </tr>
                                                <tr id="trApprovedDate" runat="server">
                                                    <td align="left">
                                                        <%=GetLabel("Approved Pada")%>
                                                    </td>
                                                    <td align="center">
                                                        :
                                                    </td>
                                                    <td>
                                                        <div runat="server" id="divApprovedDate">
                                                        </div>
                                                    </td>
                                                </tr>
                                                <tr id="trVoidBy" runat="server">
                                                    <td align="left">
                                                        <%=GetLabel("Void Oleh") %>
                                                    </td>
                                                    <td align="center">
                                                        :
                                                    </td>
                                                    <td>
                                                        <div runat="server" id="divVoidBy">
                                                        </div>
                                                    </td>
                                                </tr>
                                                <tr id="trVoidDate" runat="server">
                                                    <td align="left">
                                                        <%=GetLabel("Void Pada")%>
                                                    </td>
                                                    <td align="center">
                                                        :
                                                    </td>
                                                    <td>
                                                        <div runat="server" id="divVoidDate">
                                                        </div>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td align="left">
                                                        <%=GetLabel("Terakhir Diubah Oleh") %>
                                                    </td>
                                                    <td align="center">
                                                        :
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
                                                        :
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
    </div>
    <dxcp:ASPxCallbackPanel ID="cbpProcess" runat="server" Width="100%" ClientInstanceName="cbpProcess"
        ShowLoadingPanel="false" OnCallback="cbpProcess_Callback">
        <ClientSideEvents BeginCallback="function(s,e) { showLoadingPanel(); }" EndCallback="function(s,e) { onCbpProcesEndCallback(s); }" />
    </dxcp:ASPxCallbackPanel>
</asp:Content>
