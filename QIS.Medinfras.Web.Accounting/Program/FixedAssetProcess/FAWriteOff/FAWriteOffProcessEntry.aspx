<%@ Page Title="" Language="C#" MasterPageFile="~/libs/MasterPage/MPTrx2.master"
    AutoEventWireup="true" CodeBehind="FAWriteOffProcessEntry.aspx.cs" Inherits="QIS.Medinfras.Web.Accounting.Program.FAWriteOffProcessEntry" %>

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

            if ($('#<%=hdnFAWriteOffHdID.ClientID %>').val() == "" || $('#<%=hdnFAWriteOffHdID.ClientID %>').val() == "0") {
                setDatePicker('<%=txtFAWriteOffHdDate.ClientID %>');
            }

            $('#<%=txtFAWriteOffHdDate.ClientID %>').datepicker('option', 'maxDate', '0');

            var pageCount = parseInt($('#<%=hdnPageCount.ClientID %>').val());
            setPaging($("#paging"), pageCount, function (page) {
                cbpView.PerformCallback('changepage|' + page);
            });
        }

        //#region FAWriteOffHdNo
        $('#<%=lblFAWriteOffNo.ClientID %>.lblLink').live('click', function () {
            openSearchDialog('fawriteoffhd', "FAWriteOffHdID IS NOT NULL", function (value) {
                $('#<%=txtFAWriteOffHdNo.ClientID %>').val(value);
                ontxtFAWriteOffHdNoChanged(value);
            });
        });

        $('#<%=txtFAWriteOffHdNo.ClientID %>').change(function () {
            ontxtFAWriteOffHdNoChanged($(this).val());
        });

        function ontxtFAWriteOffHdNoChanged(value) {
            onLoadObject(value);
        }
        //#endregion

        $('#btnCancel').die('click');
        $('#btnCancel').live('click', function () {
            $('#containerEntry').hide();
        });

        $('#btnSave').die('click');
        $('#btnSave').live('click', function (evt) {
            if (IsValid(evt, 'fsTrxPopup', 'mpTrxPopup')) {
                var assetItem = $('#<%=hdnFAItemID.ClientID %>').val();
                var writeOffType = cboAssetWriteOffType.GetValue();
                var salesType = cboAssetSalesType.GetValue();

                if (assetItem == null || assetItem == "" || assetItem == "0") {
                    displayErrorMessageBox('WARNING', "Harap pilih Aset dan Inventaris terlebih dahulu.");
                } else if (writeOffType == null) {
                    displayErrorMessageBox('WARNING', "Harap pilih Tipe Pemusnahan terlebih dahulu.");
                } else if (salesType == null) {
                    displayErrorMessageBox('WARNING', "Harap pilih Cara Penjualan terlebih dahulu.");
                } else {
                    cbpProcess.PerformCallback('save');
                }
            }
        });

        $('#lblAddData').die('click');
        $('#lblAddData').live('click', function (evt) {
            if (IsValid(evt, 'fsMPEntry', 'mpEntry')) {
                $('#<%=hdnIsEdit.ClientID %>').val('0');
                $('#<%=hdnID.ClientID %>').val("");

                $('#lblFAItem').attr('class', 'lblLink');
                $('#<%=hdnFAItemID.ClientID %>').val("");
                $('#<%=txtFAItemCode.ClientID %>').val("");
                $('#<%=txtFAItemName.ClientID %>').val("");

                cboAssetWriteOffType.SetValue("");
                cboAssetSalesType.SetValue("");

                $('#<%=txtProcurementAmount.ClientID %>').val("0").trigger('changeValue');
                $('#<%=txtTotalDepreciationAmount.ClientID %>').val("0").trigger('changeValue');
                $('#<%=txtAssetValue.ClientID %>').val("0").trigger('changeValue');
                $('#<%=txtWriteOffAmount.ClientID %>').val("0").trigger('changeValue');
                $('#<%=txtSelisih.ClientID %>').val("0").trigger('changeValue');

                $('#<%=txtRemarks.ClientID %>').val("");

                $('#containerEntry').show();
            }
        });

        $('#<%=grdView.ClientID %> .imgDelete.imgLink').die('click');
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

        $('#<%=grdView.ClientID %> .imgEdit.imgLink').die('click');
        $('#<%=grdView.ClientID %> .imgEdit.imgLink').live('click', function () {
            $row = $(this).closest('tr').parent().closest('tr');
            var entity = rowToObject($row);
            $('#<%=hdnIsEdit.ClientID %>').val('1');
            $('#<%=hdnID.ClientID %>').val(entity.ID);

            $('#lblFAItem').attr('class', 'lblDisabled');
            $('#<%=hdnFAItemID.ClientID %>').val(entity.FixedAssetID);
            $('#<%=txtFAItemCode.ClientID %>').val(entity.FixedAssetCode);
            $('#<%=txtFAItemName.ClientID %>').val(entity.FixedAssetName);

            cboAssetWriteOffType.SetValue(entity.GCAssetWriteOffType);
            cboAssetSalesType.SetValue(entity.GCAssetSalesType);

            $('#<%=txtProcurementAmount.ClientID %>').val(entity.AssetValue).trigger('changeValue');
            $('#<%=txtTotalDepreciationAmount.ClientID %>').val(entity.TotalDepreciationAmount).trigger('changeValue');
            $('#<%=txtAssetValue.ClientID %>').val(entity.NilaiBuku).trigger('changeValue');
            $('#<%=txtWriteOffAmount.ClientID %>').val(entity.WriteOffAmount).trigger('changeValue');
            $('#<%=txtSelisih.ClientID %>').val(entity.Selisih).trigger('changeValue');

            $('#<%=txtRemarks.ClientID %>').val(entity.Remarks);

            $('#containerEntry').show();
        });
        //#endregion

        //#region FAItem
        function onGetFAItemFilterExpression() {
            var filterFAItem = "IsDeleted = 0 AND GCItemStatus != '" + Constant.ItemStatus.ITEM_STATUS_IN_ACTIVE + "'";

            if ($('#<%=hdnFAWriteOffHdID.ClientID %>').val() != 0) {
                filterFAItem += " AND FixedAssetID NOT IN (SELECT fmd.FixedAssetID FROM FAWriteOffDt fmd WITH(NOLOCK) INNER JOIN FAWriteOffHd fmh WITH(NOLOCK) ON fmh.FAWriteOffHdID = fmd.FAWriteOffHdID WHERE fmh.GCTransactionStatus != 'X121^999' AND fmd.IsDeleted = 0 AND fmh.FAWriteOffHdID = " + $('#<%=hdnFAWriteOffHdID.ClientID %>').val() + ")";
            }
            else {
                filterFAItem += " AND FixedAssetID NOT IN (SELECT fmd.FixedAssetID FROM FAWriteOffDt fmd WITH(NOLOCK) INNER JOIN FAWriteOffHd fmh WITH(NOLOCK) ON fmh.FAWriteOffHdID = fmd.FAWriteOffHdID WHERE fmh.GCTransactionStatus != 'X121^999' AND fmd.IsDeleted = 0)";
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

        function onAfterSaveRecordDtSuccess(oFAWriteOffHdID) {
            if ($('#<%=hdnFAWriteOffHdID.ClientID %>').val() == '0') {
                $('#<%=hdnFAWriteOffHdID.ClientID %>').val(oFAWriteOffHdID);
                var filterExpression = 'FAWriteOffHdID = ' + oFAWriteOffHdID;
                Methods.getObject('GetFAWriteOffHdList', filterExpression, function (result) {
                    $('#<%=txtFAWriteOffHdNo.ClientID %>').val(result.FAWriteOffHdNo);
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
            var faWriteOffHdID = $('#<%=hdnFAWriteOffHdID.ClientID %>').val();
            var gcTransactionStatus = $('#<%=hdnGCTransactionStatus.ClientID%>').val();

            if (faWriteOffHdID == '' || faWriteOffHdID == '0') {
                errMessage.text = 'Harap pilih transaksi terlebih dahulu.';
                return false;
            } else {
                if (GCTransactionStatus != Constant.TransactionStatus.OPEN && GCTransactionStatus != Constant.TransactionStatus.VOID) {
                } else {
                    errMessage.text = "Transaksi harus Approve terlebih dahulu sebelum proses bisa dilakukan.";
                    return false;
                }
            }

        }

    </script>
    <input type="hidden" value="" id="hdnPageTitle" runat="server" />
    <input type="hidden" value="" id="hdnFAWriteOffHdID" runat="server" />
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
                                <label class="lblLink" id="lblFAWriteOffNo" runat="server">
                                    <%=GetLabel("No. Pemusnahan")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtFAWriteOffHdNo" Width="150px" ReadOnly="true" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <%=GetLabel("Tanggal Pemusnahan") %>
                            </td>
                            <td style="padding-right: 1px; width: 145px">
                                <asp:TextBox ID="txtFAWriteOffHdDate" Width="120px" CssClass="datepicker" runat="server" />
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
                                <asp:TextBox ID="txtRemarksHd" Width="100%" runat="server" TextMode="MultiLine" Rows="2" />
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
                                    <col style="width: 180px" />
                                    <col style="width: 200px" />
                                    <col style="width: 5px" />
                                    <col style="width: 500px" />
                                    <col style="width: 200px" />
                                    <col style="width: 30%" />
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
                                        <label class="lblMandatory">
                                            <%=GetLabel("Tipe Pemusnahan") %></label>
                                    </td>
                                    <td>
                                        <dxe:ASPxComboBox runat="server" ID="cboAssetWriteOffType" ClientInstanceName="cboAssetWriteOffType"
                                            Width="200px" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel">
                                        <label class="lblMandatory">
                                            <%=GetLabel("Cara Penjualan") %></label>
                                    </td>
                                    <td>
                                        <dxe:ASPxComboBox runat="server" ID="cboAssetSalesType" ClientInstanceName="cboAssetSalesType"
                                            Width="200px" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel">
                                        <label class="lblNormal">
                                            <%=GetLabel("Nilai Perolehan") %></label>
                                    </td>
                                    <td>
                                        <asp:TextBox runat="server" ID="txtProcurementAmount" Width="200px" CssClass="txtCurrency"
                                            ReadOnly="true" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel">
                                        <label class="lblNormal">
                                            <%=GetLabel("Total Akumulasi Penyusutan") %></label>
                                    </td>
                                    <td>
                                        <asp:TextBox runat="server" ID="txtTotalDepreciationAmount" Width="200px" CssClass="txtCurrency"
                                            ReadOnly="true" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel">
                                        <label class="lblNormal">
                                            <%=GetLabel("Nilai Buku") %></label>
                                    </td>
                                    <td>
                                        <asp:TextBox runat="server" ID="txtAssetValue" Width="200px" CssClass="txtCurrency"
                                            ReadOnly="true" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel">
                                        <label class="lblMandatory">
                                            <%=GetLabel("Nilai Pemusnahan") %></label>
                                    </td>
                                    <td>
                                        <asp:TextBox runat="server" ID="txtWriteOffAmount" Width="200px" CssClass="txtCurrency" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel">
                                        <label class="lblNormal">
                                            <%=GetLabel("Selisih") %></label>
                                    </td>
                                    <td>
                                        <asp:TextBox runat="server" ID="txtSelisih" Width="200px" CssClass="txtCurrency"
                                            ReadOnly="true" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel" valign="top">
                                        <label class="lblNormal">
                                            <%=GetLabel("Keterangan") %></label>
                                    </td>
                                    <td colspan="3">
                                        <asp:TextBox runat="server" ID="txtRemarks" Width="100%" TextMode="MultiLine" Rows="2" />
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
                                                    <input type="hidden" value="<%#:Eval("GCAssetWriteOffType") %>" bindingfield="GCAssetWriteOffType" />
                                                    <input type="hidden" value="<%#:Eval("AssetWriteOffType") %>" bindingfield="AssetWriteOffType" />
                                                    <input type="hidden" value="<%#:Eval("GCAssetSalesType") %>" bindingfield="GCAssetSalesType" />
                                                    <input type="hidden" value="<%#:Eval("AssetSalesType") %>" bindingfield="AssetSalesType" />
                                                    <input type="hidden" value="<%#:Eval("AssetValue") %>" bindingfield="AssetValue" />
                                                    <input type="hidden" value="<%#:Eval("WriteOffAmount") %>" bindingfield="WriteOffAmount" />
                                                    <input type="hidden" value="<%#:Eval("Selisih") %>" bindingfield="Selisih" />
                                                    <input type="hidden" value="<%#:Eval("TotalDepreciationAmount") %>" bindingfield="TotalDepreciationAmount" />
                                                    <input type="hidden" value="<%#:Eval("NilaiBuku") %>" bindingfield="NilaiBuku" />
                                                    <input type="hidden" value="<%#:Eval("Remarks") %>" bindingfield="Remarks" />
                                                    <input type="hidden" value="<%#:Eval("IsDeleted") %>" bindingfield="IsDeleted" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Aset dan Inventaris" HeaderStyle-Width="200px" HeaderStyle-HorizontalAlign="Center"
                                                ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <table cellpadding="0" cellspacing="0">
                                                        <tr>
                                                            <td class="tdLabel" align="left">
                                                                <label style="font-weight: bold; font-size: medium">
                                                                    <%#:Eval("FixedAssetCode") %></label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="tdLabel" align="left">
                                                                <label style="font-weight: bold">
                                                                    <%#:Eval("FixedAssetName") %></label>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="AssetWriteOffType" HeaderText="Tipe Pemusnahan" HeaderStyle-Width="200px"
                                                HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                                            <asp:BoundField DataField="AssetSalesType" HeaderText="Cara Penjualan" HeaderStyle-Width="200px"
                                                HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                                            <asp:BoundField DataField="cfWriteOffAmountInString" HeaderText="Nilai Pemusnahan"
                                                HeaderStyle-Width="200px" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" />
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
                                                                    <%#:Eval("cfCreatedDateInFullString") %></label>
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
                                                                    <%#:Eval("cfLastUpdatedDateInFullString") %></label>
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
