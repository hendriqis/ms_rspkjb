<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ChargesDtModalityEntry.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.ChargesDtModalityEntry" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<script type="text/javascript" id="dxss_chargesdtmodalityentry">
    $('#lblPatientVisitNoteAddData').die('click');
    $('#lblPatientVisitNoteAddData').live('click', function () {
        $('#<%=hdnID.ClientID %>').val('');
        $('#<%=hdnChargesDtID.ClientID %>').val('');
        $('#<%=hdnItemID.ClientID %>').val('');
        $('#<%=txtItemCode.ClientID %>').val('');
        $('#<%=txtItemName.ClientID %>').val('');
        $('#<%=hdnModalityID.ClientID %>').val('');
        $('#<%=txtModalityCode.ClientID %>').val('');
        $('#<%=txtModalityName.ClientID %>').val('');
        cboModalities.SetValue('');
        $('#<%:lblItem.ClientID %>').attr('class', 'lblLink lblMandatory');
        $('#<%:txtItemCode.ClientID %>').removeAttr('readonly');
        $('#containerModalityEntryData').show();
    });

    $('#btnModalityCtlCancel').click(function () {
        $('#containerModalityEntryData').hide();
    });

    $('#btnModalityCtlSave').click(function (evt) {
        var modalities = cboModalities.GetValue();
        var modalityID = $('#<%=hdnModalityID.ClientID %>').val();
        if (IsValid(evt, 'fsModality', 'mpModality')) {
            var itemID = $('#<%=hdnItemID.ClientID %>').val();
            if (itemID != '') {
                if (modalities == null && modalityID == '') {
                    showToast('Warning', 'Harap Pilih Modality / Modalities Terlebih Dahulu');
                }
                else {
                    cbpModalityCtl.PerformCallback('save');
                }
            }
            else {
                showToast('Warning', 'Harap Pilih Item Terlebih Dahulu');
            }
        }
        else {
            return false;
        }
    });

    $('.imgDeleteCtlModality.imgLink').die('click');
    $('.imgDeleteCtlModality.imgLink').live('click', function () {
        $row = $(this).closest('tr');
        showToastConfirmation('Are You Sure Want To Delete?', function (result) {
            if (result) {
                var entity = rowToObject($row);
                $('#<%=hdnID.ClientID %>').val(entity.ID);
                cbpModalityCtl.PerformCallback('delete');
            }
        });
    });

    $('.imgEditCtlModality.imgLink').die('click')
    $('.imgEditCtlModality.imgLink').live('click', function () {
        $row = $(this).closest('tr');
        var entity = rowToObject($row);
        $('#<%=hdnID.ClientID %>').val(entity.ID);

        $('#<%=hdnChargesDtID.ClientID %>').val(entity.PatientChargesDtID);
        $('#<%=hdnItemID.ClientID %>').val(entity.ItemID);
        $('#<%=txtItemCode.ClientID %>').val(entity.ItemCode);
        $('#<%=txtItemName.ClientID %>').val(entity.ItemName1);
        $('#<%=hdnModalityID.ClientID %>').val(entity.ModalityID);
        $('#<%=txtModalityCode.ClientID %>').val(entity.ModalityCode);
        $('#<%=txtModalityName.ClientID %>').val(entity.ModalityName);
        cboModalities.SetValue(entity.GCModalities);

        $('#<%:lblItem.ClientID %>').attr('class', 'lblDisabled');
        $('#<%:txtItemCode.ClientID %>').attr('readonly', 'readonly');
        $('#containerModalityEntryData').show();
    });

    //#region Modality
    $('#lblModalityID.lblLink').live('click', function () {
        openSearchDialog('modality', '1=1', function (value) {
            $('#<%=txtModalityCode.ClientID %>').val(value);
            onTxtModalityCodeChanged(value);
        });
    });

    $('#<%=txtModalityCode.ClientID %>').live('change', function () {
        onTxtModalityCodeChanged($(this).val());
    });

    function onTxtModalityCodeChanged(value) {
        var filterExpression = "ModalityCode = '" + value + "'";
        Methods.getObject('GetModalityList', filterExpression, function (result) {
            if (result != null) {
                $('#<%=hdnModalityID.ClientID %>').val(result.ModalityID);
                $('#<%=txtModalityName.ClientID %>').val(result.ModalityName);
                cboModalities.SetValue(result.GCModality);
            }
            else {
                $('#<%=hdnModalityID.ClientID %>').val('');
                $('#<%=txtModalityCode.ClientID %>').val('');
                $('#<%=txtModalityName.ClientID %>').val('');
                cboModalities.SetValue('');
            }
        });
    }
    //#endregion

    //#region Item
    function getFilterExpressionItem() {
        var transID = $('#<%=hdnTransactionIDCtl.ClientID %>').val();
        var filter = "TransactionID = '" + transID + "' AND IsDeleted = 0 AND ISNULL(GCTransactionDetailStatus,'') != 'X121^999'";
        return filter;
    }

    $('#<%:lblItem.ClientID %>.lblLink').live('click', function () {
        openSearchDialog('chargesitem', getFilterExpressionItem(), function (value) {
            $('#<%=txtItemCode.ClientID %>').val(value);
            onTxtItemCodeChanged(value);
        });
    });

    $('#<%=txtItemCode.ClientID %>').live('change', function () {
        onTxtItemCodeChanged($(this).val());
    });

    function onTxtItemCodeChanged(value) {
        var filterExpression = getFilterExpressionItem() + " AND ItemCode = '" + value + "'";
        Methods.getObject('GetvPatientChargesDt9List', filterExpression, function (result) {
            if (result != null) {
                $('#<%=hdnChargesDtID.ClientID %>').val(result.ID);
                $('#<%=hdnItemID.ClientID %>').val(result.ItemID);
                $('#<%=txtItemName.ClientID %>').val(result.ItemName1);
            }
            else {
                $('#<%=hdnChargesDtID.ClientID %>').val('');
                $('#<%=hdnItemID.ClientID %>').val('');
                $('#<%=txtItemName.ClientID %>').val('');
            }
        });
    }
    //#endregion

    function onCbpModalityCtlEndCallback(s) {
        var param = s.cpResult.split('|');
        if (param[0] == 'save') {
            if (param[1] == 'fail') {
                showToast('Save Failed', 'Error Message : ' + param[2]);
                $('#containerModalityEntryData').hide();
                cbpModalityCtl.PerformCallback('refresh');
            }
            else {
                var pageCount = parseInt(param[2]);
                setPagingDetailItem(pageCount);
                $('#containerModalityEntryData').hide();
            }
        }
        else if (param[0] == 'delete') {
            if (param[1] == 'fail') {
                showToast('Delete Failed', 'Error Message : ' + param[2]);
                $('#containerModalityEntryData').hide();
                cbpModalityCtl.PerformCallback('refresh');
            }
            else {
                var pageCount = parseInt(param[2]);
                setPagingDetailItem(pageCount);
            }
        }
        else if (param[0] == 'refresh' || param[0] == 'changepage') {
            var pageCount = parseInt(param[1]);
            setPagingDetailItem(pageCount);
        }
        hideLoadingPanel();
    }

    //#region Paging
    var pageCountAvailable = parseInt('<%=PageCount %>');
    $(function () {
        setPagingDetailItem(pageCountAvailable);
    });

    function setPagingDetailItem(pageCount) {
        if (pageCount > 0)
            $('#<%=grdView.ClientID %> tr:eq(1)').click();
        setPaging($("#pagingPopup"), pageCount, function (page) {
            cbpModalityCtl.PerformCallback('changepage|' + page);
        }, 8);
    }
    //#endregion 
</script>
<div style="height: 440px; overflow-y: auto">
    <input type="hidden" value="" runat="server" id="hdnVisitID" />
    <input type="hidden" value="" runat="server" id="hdnTransactionIDCtl" />
    <table class="tblContentArea">
        <tr>
            <td style="padding: 5px; vertical-align: top">
                <table class="tblEntryContent" style="width: 400px">
                    <colgroup>
                        <col style="width: 100px" />
                        <col style="width: 300px" />
                    </colgroup>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("No. RM")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtMRN" Width="160px" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Nama Pasien")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtPatientName" Width="100%" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("No Transaksi")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtTransactionNo" Width="100%" runat="server" />
                        </td>
                    </tr>
                </table>
                <div id="containerModalityEntryData" style="margin-top: 10px; display: none;">
                    <div class="pageTitle">
                        <%=GetLabel("Entry")%></div>
                    <input type="hidden" id="hdnID" runat="server" value="" />
                    <fieldset id="fsModality" style="margin: 0">
                        <table class="tblEntryDetail" style="width: 100%">
                            <colgroup>
                                <col style="width: 5%" />
                                <col />
                            </colgroup>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblLink lblMandatory" id="lblItem" runat="server">
                                        <%=GetLabel("Item")%></label>
                                </td>
                                <td>
                                    <input type="hidden" id="hdnChargesDtID" value="" runat="server" />
                                    <input type="hidden" id="hdnItemID" value="" runat="server" />
                                    <table style="width: 100%" cellpadding="0" cellspacing="0">
                                        <colgroup>
                                            <col style="width: 10%" />
                                            <col style="width: 3px" />
                                            <col />
                                        </colgroup>
                                        <tr>
                                            <td>
                                                <asp:TextBox ID="txtItemCode" Width="100%" runat="server" />
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtItemName" Width="100%" ReadOnly="true" runat="server" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblLink" id="lblModalityID">
                                        <%=GetLabel("Modality")%></label>
                                </td>
                                <td>
                                    <input type="hidden" id="hdnModalityID" value="" runat="server" />
                                    <table style="width: 100%" cellpadding="0" cellspacing="0">
                                        <colgroup>
                                            <col style="width: 10%" />
                                            <col style="width: 3px" />
                                            <col />
                                        </colgroup>
                                        <tr>
                                            <td>
                                                <asp:TextBox ID="txtModalityCode" Width="100%" runat="server" />
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtModalityName" Width="100%" ReadOnly="true" runat="server" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td class="tdlabel">
                                    <label class="lblNormal">
                                        <%=GetLabel("Modalities")%></label>
                                </td>
                                <td>
                                    <dxe:ASPxComboBox ID="cboModalities" ClientInstanceName="cboModalities" Width="30%"
                                        runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <table>
                                        <tr>
                                            <td>
                                                <input type="button" id="btnModalityCtlSave" value='<%= GetLabel("Simpan")%>' />
                                            </td>
                                            <td>
                                                <input type="button" id="btnModalityCtlCancel" value='<%= GetLabel("Batal")%>' />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                </div>
                <dxcp:ASPxCallbackPanel ID="cbpModalityCtl" runat="server" Width="100%" ClientInstanceName="cbpModalityCtl"
                    ShowLoadingPanel="false" OnCallback="cbpModalityCtl_Callback">
                    <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ onCbpModalityCtlEndCallback(s); }" />
                    <PanelCollection>
                        <dx:PanelContent ID="PanelContent1" runat="server">
                            <asp:Panel runat="server" ID="pnlPatientVisitTransHdGrdView" Style="width: 100%;
                                margin-left: auto; margin-right: auto; position: relative; font-size: 0.95em;">
                                <asp:GridView ID="grdView" runat="server" CssClass="grdView notAllowSelect" AutoGenerateColumns="false"
                                    ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                    <Columns>
                                        <asp:TemplateField HeaderStyle-Width="40px" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <img class='imgEditCtlModality <%#: Eval("GCTransactionStatus").ToString() != "X121^001" ? "imgDisabled" : "imgLink"%>'
                                                    title='<%=GetLabel("Edit")%>' src='<%# Eval("GCTransactionStatus").ToString() != "X121^001" ? ResolveUrl("~/Libs/Images/Button/edit_disabled.png") : ResolveUrl("~/Libs/Images/Button/edit.png")%>'
                                                    alt="" style="float: left; margin-left: 7px" />
                                                <img class='imgDeleteCtlModality <%#: Eval("GCTransactionStatus").ToString() != "X121^001" ? "imgDisabled" : "imgLink"%>'
                                                    title='<%=GetLabel("Delete")%>' src='<%# Eval("GCTransactionStatus").ToString() != "X121^001" ? ResolveUrl("~/Libs/Images/Button/delete_disabled.png") : ResolveUrl("~/Libs/Images/Button/delete.png")%>'
                                                    alt="" />
                                                <input type="hidden" value="<%#:Eval("ID") %>" bindingfield="ID" />
                                                <input type="hidden" value="<%#:Eval("ItemID") %>" bindingfield="ItemID" />
                                                <input type="hidden" value="<%#:Eval("ItemCode") %>" bindingfield="ItemCode" />
                                                <input type="hidden" value="<%#:Eval("ItemName1") %>" bindingfield="ItemName1" />
                                                <input type="hidden" value="<%#:Eval("ModalityID") %>" bindingfield="ModalityID" />
                                                <input type="hidden" value="<%#:Eval("ModalityCode") %>" bindingfield="ModalityCode" />
                                                <input type="hidden" value="<%#:Eval("ModalityName") %>" bindingfield="ModalityName" />
                                                <input type="hidden" value="<%#:Eval("GCModalities") %>" bindingfield="GCModalities" />
                                                <input type="hidden" value="<%#:Eval("ModalitiesName") %>" bindingfield="ModalitiesName" />
                                                <input type="hidden" value="<%#:Eval("TransactionID") %>" bindingfield="TransactionID" />
                                                <input type="hidden" value="<%#:Eval("PatientChargesDtID") %>" bindingfield="PatientChargesDtID" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField HeaderStyle-Width="200px" DataField="ItemName1" HeaderText="Nama Item"
                                            HeaderStyle-HorizontalAlign="Left" />
                                        <asp:BoundField HeaderStyle-Width="150px" DataField="cfModalityName" HeaderText="Modality"
                                            HeaderStyle-HorizontalAlign="Left" />
                                        <asp:BoundField HeaderStyle-Width="150px" DataField="ModalitiesName" HeaderText="Modalities"
                                            HeaderStyle-HorizontalAlign="Left" />
                                        <asp:TemplateField ItemStyle-VerticalAlign="Top" HeaderStyle-Width="100px">
                                            <HeaderTemplate>
                                                <div style="text-align: left; padding-left: 3px">
                                                    <%=GetLabel("Dibuat Oleh")%>
                                                </div>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <div>
                                                    <%#: Eval("CreatedByFullName")%><br>
                                                    <%#: Eval("cfCreatedDateInString")%></div>
                                            </ItemTemplate>
                                            <ItemStyle VerticalAlign="Top"></ItemStyle>
                                        </asp:TemplateField>
                                    </Columns>
                                    <EmptyDataTemplate>
                                        <%=GetLabel("Data Tidak Tersedia")%>
                                    </EmptyDataTemplate>
                                </asp:GridView>
                                <div class="imgLoadingGrdView" id="containerImgLoadingModalityCtl">
                                    <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                                </div>
                                <div class="containerPaging">
                                    <div class="wrapperPaging">
                                        <div id="pagingPopup">
                                        </div>
                                    </div>
                                </div>
                            </asp:Panel>
                        </dx:PanelContent>
                    </PanelCollection>
                </dxcp:ASPxCallbackPanel>
                <div style="width: 100%; text-align: center" id="divContainerAddData" runat="server">
                    <span class="lblLink" id="lblPatientVisitNoteAddData">
                        <%= GetLabel("Tambah Data")%></span>
                </div>
            </td>
        </tr>
    </table>
    <div style="width: 100%; text-align: right">
        <input type="button" value='<%= GetLabel("Tutup")%>' onclick="pcRightPanelContent.Hide();" />
    </div>
</div>
