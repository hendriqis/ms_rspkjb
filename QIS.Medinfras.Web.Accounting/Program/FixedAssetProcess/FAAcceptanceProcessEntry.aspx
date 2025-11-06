<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/libs/MasterPage/MPTrx2.master"
    CodeBehind="FAAcceptanceProcessEntry.aspx.cs" Inherits="QIS.Medinfras.Web.Accounting.Program.FAAcceptanceProcessEntry" %>

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
<asp:Content ID="Content3" ContentPlaceHolderID="plhCustomButtonToolbar" runat="server">
    <li id="btnFAAcceptanceVoid" runat="server" crudmode="R">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbvoid.png")%>' alt="" /><div>
            <%=GetLabel("Void")%></div>
    </li>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript">
        var editedLineAmount = 0;

        function onLoadCurrentRecord() {
            onLoadObject($('#<%=txtFAAcceptanceNo.ClientID %>').val());
        }

        function setCustomToolbarVisibility() {
            var GCTransactionStatus = $('#<%=hdnGCTransactionStatus.ClientID %>').val();
            if (GCTransactionStatus == 'X121^001') {
                $('#<%=btnFAAcceptanceVoid.ClientID %>').show();
            }
            else if (GCTransactionStatus == 'X121^003') {
                $('#<%=btnFAAcceptanceVoid.ClientID %>').hide();
            }
            else if (GCTransactionStatus == 'X121^999' || GCTransactionStatus == '') {
                $('#<%=btnFAAcceptanceVoid.ClientID %>').hide();
            }
        }

        $(function () {
            $('#<%=btnFAAcceptanceVoid.ClientID %>').hide();
        });

        function onLoad() {
            if ($('#<%=hdnIsEditable.ClientID %>').val() == '1') {
                $('#lblQuickPick').show();
            }
            else {
                $('#lblQuickPick').hide();
            }

            if ($('#<%=hdnFAAcceptanceID.ClientID %>').val() == "" || $('#<%=hdnFAAcceptanceID.ClientID %>').val() == "0") {
                setDatePicker('<%=txtAcceptanceDate.ClientID %>');
            }
            $('#<%=txtAcceptanceDate.ClientID %>').datepicker('option', 'maxDate', new Date(getDateNow()));

            //#region FA Acceptance No
            $('#lblFAAcceptanceNo.lblLink').click(function () {
                openSearchDialog('faacceptance', "<%=GetFilterExpression() %>", function (value) {
                    $('#<%=txtFAAcceptanceNo.ClientID %>').val(value);
                    ontxtFAAcceptanceNoChanged(value);
                });
            });

            $('#<%=txtFAAcceptanceNo.ClientID %>').change(function () {
                ontxtFAAcceptanceNoChanged($(this).val());
            });

            function ontxtFAAcceptanceNoChanged(value) {
                onLoadObject(value);
            }
            //#endregion

            //#region Location From
            function getFALocationFilterExpression() {
                var filterExpression = "IsDeleted = 0";
                return filterExpression;
            }

            $('#<%=lblFALocation.ClientID %>.lblLink').live('click', function () {
                openSearchDialog('falocation', getFALocationFilterExpression(), function (value) {
                    $('#<%=txtFALocationCode.ClientID %>').val(value);
                    onTxtFALocationCodeChanged(value);
                });
            });

            $('#<%=txtFALocationCode.ClientID %>').live('change', function () {
                onTxtFALocationCodeChanged($(this).val());
            });

            function onTxtFALocationCodeChanged(value) {
                var filterExpression = getFALocationFilterExpression() + " AND FALocationCode = '" + value + "'";
                Methods.getObject('GetFALocationList', filterExpression, function (result) {
                    if (result != null) {
                        $('#<%=hdnFALocationID.ClientID %>').val(result.FALocationID);
                        $('#<%=txtFALocationName.ClientID %>').val(result.FALocationName);
                    }
                    else {
                        $('#<%=hdnFALocationID.ClientID %>').val('');
                        $('#<%=txtFALocationCode.ClientID %>').val('');
                        $('#<%=txtFALocationName.ClientID %>').val('');
                    }
                });
            }
            //#endregion

            //#region Product Line
            function getProductLineFilterExpression() {
                var filterExpression = "IsDeleted = 0";
                return filterExpression;
            }

            $('#<%=lblProductLine.ClientID %>.lblLink').click(function () {
                openSearchDialog('productlineitemtype', getProductLineFilterExpression(), function (value) {
                    $('#<%=txtAcceptanceProductLineCode.ClientID %>').val(value);
                    onTxtProductLineCodeChanged(value);
                });
            });

            $('#<%=txtAcceptanceProductLineCode.ClientID %>').change(function () {
                onTxtProductLineCodeChanged($(this).val());
            });

            function onTxtProductLineCodeChanged(value) {
                var filterExpression = getProductLineFilterExpression() + " AND ProductLineCode = '" + value + "'";
                Methods.getObject('GetProductLineList', filterExpression, function (result) {
                    if (result != null) {
                        $('#<%=hdnAcceptanceProductLineID.ClientID %>').val(result.ProductLineID);
                        $('#<%=txtAcceptanceProductLineName.ClientID %>').val(result.ProductLineName);
                        $('#<%=hdnAcceptanceProductLineItemType.ClientID %>').val(result.GCItemType);
                    }
                    else {
                        $('#<%=hdnAcceptanceProductLineID.ClientID %>').val('');
                        $('#<%=txtAcceptanceProductLineCode.ClientID %>').val('');
                        $('#<%=txtAcceptanceProductLineName.ClientID %>').val('');
                        $('#<%=hdnAcceptanceProductLineItemType.ClientID %>').val('');
                    }
                });
            }
            //#endregion

            //#region FA Group
            function onGetFAGroupFilterExpression() {
                var filterExpression = "IsDeleted = 0";
                return filterExpression;
            }

            $('#<%=lblFAGroup.ClientID %>.lblLink').live('click', function () {
                openSearchDialog('fagroup', onGetFAGroupFilterExpression(), function (value) {
                    $('#<%=txtFAGroupCode.ClientID %>').val(value);
                    onTxtFAGroupCodeChanged(value);
                });
            });

            $('#<%=txtFAGroupCode.ClientID %>').change(function () {
                onTxtFAGroupCodeChanged($(this).val());
            });

            function onTxtFAGroupCodeChanged(value) {
                var filterExpression = onGetFAGroupFilterExpression() + " AND FAGroupCode = '" + value + "'";
                Methods.getObject('GetFAGroupList', filterExpression, function (result) {
                    if (result != null) {
                        $('#<%=hdnFAGroupID.ClientID %>').val(result.FAGroupID);
                        $('#<%=txtFAGroupName.ClientID %>').val(result.FAGroupName);
                    }
                    else {
                        $('#<%=hdnFAGroupID.ClientID %>').val('');
                        $('#<%=txtFAGroupCode.ClientID %>').val('');
                        $('#<%=txtFAGroupName.ClientID %>').val('');
                    }
                });
            }
            //#endregion

            $('#lblQuickPick').click(function () {
                if (IsValid(null, 'fsMPEntry', 'mpEntry')) {
                    showLoadingPanel();
                    var url = ResolveUrl('~/Program/FixedAssetProcess/FAAcceptanceQuickPicksCtl.ascx');
                    var faAcceptanceID = $('#<%=hdnFAAcceptanceID.ClientID %>').val();
                    var productLineID = $('#<%=hdnAcceptanceProductLineID.ClientID %>').val();
                    var faGroupID = $('#<%=hdnFAGroupID.ClientID %>').val();
                    var faLocationID = $('#<%=hdnFALocationID.ClientID %>').val();
                    var param = faAcceptanceID + '|' + productLineID + '|' + faGroupID + '|' + faLocationID;
                    openUserControlPopup(url, param, 'Quick Picks', 1200, 500);
                }
            });

            $('#btnCancel').click(function () {
                $('#containerEntry').hide();
                cbpView.PerformCallback('refresh');
            });

            $('#btnSave').click(function (evt) {
                if (IsValid(evt, 'fsTrxPopup', 'mpTrxPopup'))
                    cbpProcess.PerformCallback('save');
            });

            var pageCount = parseInt($('#<%=hdnPageCount.ClientID %>').val());
            setPaging($("#paging"), pageCount, function (page) {
                cbpView.PerformCallback('changepage|' + page);
            });

        }

        //#region delete
        $('#<%=grdView.ClientID %> .imgDelete.imgLink').live('click', function () {
            $row = $(this).closest('tr').parent().closest('tr');
            showToastConfirmation('Are You Sure Want To Delete?', function (result) {
                if (result) {
                    var entity = rowToObject($row);
                    $('#<%=hdnEntryID.ClientID %>').val(entity.FAAcceptanceDtID);
                    cbpProcess.PerformCallback('delete');
                }
            });
        });

        //#endregion

        $('#<%=btnFAAcceptanceVoid.ClientID %>').live('click', function (evt) {
            if (IsValid(evt, 'fsMPEntry', 'mpEntry')) {
                var url = ResolveUrl('~/Program/FixedAssetProcess/FAAcceptanceVoidCtl.ascx');
                var faAcceptanceID = $('#<%=hdnFAAcceptanceID.ClientID %>').val();
                openUserControlPopup(url, faAcceptanceID, 'Pembatalan Berita Acara', 400, 230);
            }
        });

        function onAfterCustomClickSuccess(type, retval) {
            onLoadObject(retval);
        }

        function onAfterSaveRecordDtSuccess(FAAcceptanceID) {
            if ($('#<%=hdnFAAcceptanceID.ClientID %>').val() == '0') {
                $('#<%=hdnFAAcceptanceID.ClientID %>').val(FAAcceptanceID);
                var filterExpression = 'FAAcceptanceID = ' + FAAcceptanceID;
                Methods.getObject('GetFAAcceptanceHdList', filterExpression, function (result) {
                    $('#<%=txtFAAcceptanceNo.ClientID %>').val(result.FAAcceptanceNo);
                });
                onAfterCustomSaveSuccess();
            }
            cbpView.PerformCallback('refresh');
        }

        function onAfterSaveAddRecordEntryPopup(param) {
            onAfterSaveRecordDtSuccess(param);
        }

        function onCbpProcesEndCallback(s) {
            hideLoadingPanel();

            var param = s.cpResult.split('|');
            var FAAcceptanceID = s.cpFAAcceptanceID;
            if (param[0] == 'save') {
                if (param[1] == 'fail') {
                    showToast('Save Failed', 'Error Message : ' + param[2]);
                }
                else {
                    onAfterSaveRecordDtSuccess(FAAcceptanceID);
                    var isEdit = $('#<%=hdnIsEdit.ClientID %>').val();
                    if (isEdit == '1') {
                        $('#containerEntry').hide();
                    } else if (isEdit == '0') {
                        $('#lblAddData').click();
                    }
                    onLoadObject(FAAcceptanceID);
                }
            }
            else if (param[0] == 'delete') {
                if (param[1] == 'fail') {
                    showToast('Delete Failed', 'Error Message : ' + param[2]);
                }
                else {
                    $('#containerEntry').hide();
                    onLoadObject(FAAcceptanceID);
                }
            }
        }

        //#region Paging
        function onCbpViewEndCallback(s) {
            hideLoadingPanel();

            var param = s.cpResult.split('|');
            if (param[0] == 'refresh') {
                var pageCount = parseInt(param[1]);
                setPaging($("#paging"), pageCount, function (page) {
                    cbpView.PerformCallback('changepage|' + page);
                });
            }
        }
        //#endregion

        //#region Right Panel
        function onBeforeRightPanelPrint(code, filterExpression, errMessage) {
            var faAcceptanceID = $('#<%=hdnFAAcceptanceID.ClientID %>').val();
            var printStatus = $('#<%=hdnPrintStatus.ClientID %>').val();
            var menuCode = $('#<%=hdnMenuCode.ClientID %>').val();
            if (printStatus == 'true') {
                if (faAcceptanceID == '' || faAcceptanceID == '0') {
                    errMessage.text = 'Please Set Transaction First!';
                    return false;
                }
                else if (code == 'AC-00013') {
                    filterExpression.text = faAcceptanceID + '|' + menuCode;
                    return true;
                }
                else {
                    filterExpression.text = "FAAcceptanceID = " + faAcceptanceID;
                    return true;
                }
            } else {
                errMessage.text = "Data Doesn't Approved or Closed";
                return false;
            }
        }
        //#endregion
    </script>
    <input type="hidden" value="" id="hdnPageTitle" runat="server" />
    <input type="hidden" value="" id="hdnPrintStatus" runat="server" />
    <input type="hidden" value="" id="hdnFAAcceptanceID" runat="server" />
    <input type="hidden" value="" id="hdnPageCount" runat="server" />
    <input type="hidden" value="" id="hdnGCTransactionStatus" runat="server" />
    <input type="hidden" value="1" id="hdnIsEditable" runat="server" />
    <input type="hidden" value="" id="hdnRecordFilterExpression" runat="server" />
    <input type="hidden" value="0" id="hdnIsEdit" runat="server" />
    <input type="hidden" value="0" id="hdnIsUsedProductLine" runat="server" />
    <input type="hidden" value="" id="hdnItemID" runat="server" />
    <input type="hidden" value="0" id="hdnIsProcessDepreciationFromFAAcceptance" runat="server" />
    <input type="hidden" value="0" id="hdnIsApprovedFAAcceptanceReplaceDepreciationStartDate"
        runat="server" />
    <input type="hidden" value="" id="hdnMenuCode" runat="server" />
    <div style="overflow-x: hidden;">
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
                        </colgroup>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblLink" id="lblFAAcceptanceNo">
                                    <%=GetLabel("No. Berita Acara")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtFAAcceptanceNo" Width="150px" ReadOnly="true" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <%=GetLabel("Tanggal Berita Acara") %>
                            </td>
                            <td style="padding-right: 1px; width: 140px">
                                <asp:TextBox ID="txtAcceptanceDate" Width="120px" CssClass="datepicker" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Subjek Berita Acara")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtFAAcceptanceSubject" Width="100%" runat="server" />
                            </td>
                        </tr>
                        <tr id="trProductLine" runat="server" style="display: none">
                            <td>
                                <label class="lblLink lblMandatory" runat="server" id="lblProductLine">
                                    <%=GetLabel("Product Line")%></label>
                            </td>
                            <td>
                                <input type="hidden" id="hdnAcceptanceProductLineID" value="" runat="server" />
                                <input type="hidden" id="hdnAcceptanceProductLineItemType" value="" runat="server" />
                                <table style="width: 100%" cellpadding="0" cellspacing="0">
                                    <colgroup>
                                        <col style="width: 30%" />
                                        <col style="width: 3px" />
                                        <col />
                                    </colgroup>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtAcceptanceProductLineCode" Width="100%" runat="server" />
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtAcceptanceProductLineName" Width="100%" runat="server" ReadOnly="true" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblMandatory lblLink" runat="server" id="lblFAGroup">
                                    <%=GetLabel("Kelompok")%></label>
                            </td>
                            <td>
                                <input type="hidden" id="hdnFAGroupID" runat="server" />
                                <table style="width: 100%" cellpadding="0" cellspacing="0">
                                    <colgroup>
                                        <col style="width: 30%" />
                                        <col style="width: 3px" />
                                        <col />
                                    </colgroup>
                                    <tr>
                                        <td>
                                            <asp:TextBox runat="server" ID="txtFAGroupCode" Width="100%" />
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            <asp:TextBox runat="server" ID="txtFAGroupName" Width="100%" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr id="hdnLocation" runat="server">
                            <td class="tdLabel">
                                <label class="lblMandatory lblLink" runat="server" id="lblFALocation">
                                    <%=GetLabel("Lokasi Aset & Inventaris")%></label>
                            </td>
                            <td>
                                <input type="hidden" id="hdnFALocationID" value="" runat="server" />
                                <table style="width: 100%" cellpadding="0" cellspacing="0">
                                    <colgroup>
                                        <col style="width: 30%" />
                                        <col style="width: 3px" />
                                        <col />
                                    </colgroup>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtFALocationCode" Width="100%" runat="server" />
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtFALocationName" Width="100%" runat="server" ReadOnly="true" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
                <td style="padding: 5px; vertical-align: top">
                    <table class="tblEntryContent" style="width: 100%">
                        <colgroup>
                            <col style="width: 30%" />
                        </colgroup>
                        <tr>
                            <td class="tdLabel" style="padding: 5px; vertical-align: top">
                                <label class="lblNormal">
                                    <%=GetLabel("Catatan")%></label>
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
                    <input type="hidden" value="" id="hdnEntryID" runat="server" />
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
                                            <asp:BoundField DataField="FAAcceptanceDtID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                            <asp:TemplateField HeaderStyle-Width="30px" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <table cellpadding="0" cellspacing="0">
                                                        <tr>
                                                            <td>
                                                                <img class="imgDelete <%# IsEditable() == "0" || Eval("IsAllowEditItem").ToString() == "False" ? "imgDisabled" : "imgLink"%>"
                                                                    title='<%=GetLabel("Delete")%>' src='<%# IsEditable() == "0" || Eval("IsAllowEditItem").ToString() == "False" ? ResolveUrl("~/Libs/Images/Button/delete_disabled.png") : ResolveUrl("~/Libs/Images/Button/delete.png")%>'
                                                                    alt="" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                    <input type="hidden" value="<%#:Eval("FAAcceptanceDtID") %>" bindingfield="FAAcceptanceDtID" />
                                                    <input type="hidden" value="<%#:Eval("FixedAssetID") %>" bindingfield="FixedAssetID" />
                                                    <input type="hidden" value="<%#:Eval("FixedAssetCode") %>" bindingfield="FixedAssetCode" />
                                                    <input type="hidden" value="<%#:Eval("FixedAssetName") %>" bindingfield="FixedAssetName" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-Width="200px">
                                                <HeaderTemplate>
                                                    <%=GetLabel("Kode Aset")%>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <b>
                                                        <%#:Eval("FixedAssetCode")%></b>
                                                    <br />
                                                    <label style="font-size: smaller; font-style: italic">
                                                        <%=GetLabel("Tgl Perolehan = ")%><%#:Eval("cfProcurementDateInString")%>
                                                    </label>
                                                    <br />
                                                    <label style="font-size: smaller; font-style: italic">
                                                        <%#:Eval("cfDepreciationLength")%>
                                                    </label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <HeaderTemplate>
                                                    <%=GetLabel("Nama Aset")%>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <%#:Eval("FixedAssetName")%>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <HeaderTemplate>
                                                    <%=GetLabel("Informasi Penerimaan")%>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <b>
                                                        <%#:Eval("ProcurementNumber")%></b>
                                                    <br />
                                                    <label style="font-size: smaller; font-style: italic">
                                                        <%=GetLabel("Tgl Penerimaan Barang = ")%><%#:Eval("cfProcurementDateInString")%>
                                                    </label>
                                                    <br />
                                                    <label style="font-size: smaller; font-style: italic">
                                                        <%=GetLabel("Catatan Penerimaan Barang (detail) = ")%><%#:Eval("ProcurementRemarksDetail")%>
                                                    </label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right"
                                                HeaderStyle-Width="150px">
                                                <HeaderTemplate>
                                                    <%=GetLabel("Nilai Perolehan")%>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <%#:Eval("ProcurementAmount","{0:n}")%>
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
                        <span class="lblLink" id="lblQuickPick">
                            <%= GetLabel("Quick Picks")%></span>
                    </div>
                </td>
            </tr>
            <tr>
                <td colspan="2">
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
                                                    <col width="180px" />
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
