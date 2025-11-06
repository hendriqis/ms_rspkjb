<%@ Page Title="" Language="C#" MasterPageFile="~/libs/MasterPage/MPTrx2.master"
    AutoEventWireup="true" CodeBehind="ReorderPurchaseRequest2.aspx.cs" Inherits="QIS.Medinfras.Web.Inventory.Program.ReorderPurchaseRequest2" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<asp:Content ID="Content3" ContentPlaceHolderID="plhCustomButtonToolbar" runat="server">
    <li id="btnRefresh" crudmode="R" runat="server">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbrefresh.png")%>' alt="" /><br style="clear: both" />
        <div>
            <%=GetLabel("Refresh")%></div>
    </li>
    <li id="btnReorderPurchaseRequestProcess" crudmode="R" runat="server">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbprocess.png")%>' alt="" /><br style="clear: both" />
        <div>
            <%=GetLabel("Process")%></div>
    </li>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div class="menuTitle">
        <%=HttpUtility.HtmlEncode(GetPageTitle())%></div>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript">
        function onLoad() {
            setDatePicker('<%=txtPurchaseRequestDate.ClientID %>');
            $('#<%=txtPurchaseRequestDate.ClientID %>').datepicker('option', 'maxDate', new Date(getDateNow()));

            $('#<%=btnRefresh.ClientID %>').click(function () {
                if (IsValid(null, 'fsMPEntry', 'mpEntry')) {
                    getCheckedMember();
                    if ($('#<%=hdnSelectedMember.ClientID %>').val() != '') {
                        var message = "Ada item-item yang sudah dipilih, apakah proses refresh tetap dilanjutkan ?";
                        showToastConfirmation(message, function (result) {
                            if (result) cbpView.PerformCallback('refresh'); ;
                        });
                    }
                    cbpView.PerformCallback('refresh');
                }
            });

            $('#<%=btnReorderPurchaseRequestProcess.ClientID %>').click(function () {
                if (IsValid(null, 'fsMPEntry', 'mpEntry')) {
                    getCheckedMember();
                    if ($('#<%=hdnSelectedMember.ClientID %>').val() == '') {
                        showToast('Warning', 'Please Select Item First');
                    }
                    else {
                        onCustomButtonClick('process');
                    }
                }
            });

            //#region Location From
            function getLocationFilterExpression() {
                var filterExpression = "<%:filterExpressionLocation %>";
                return filterExpression;
            }

            $('#<%=lblLocation.ClientID %>.lblLink').live('click', function () {
                openSearchDialog('locationroleuser', getLocationFilterExpression(), function (value) {
                    $('#<%=txtLocationCode.ClientID %>').val(value);
                    onTxtLocationCodeChanged(value);
                });
            });

            $('#<%=txtLocationCode.ClientID %>').live('change', function () {
                onTxtLocationCodeChanged($(this).val());
            });

            function onTxtLocationCodeChanged(value) {
                var filterExpression = getLocationFilterExpression() + "LocationCode = '" + value + "'";
                Methods.getObject('GetLocationUserAccessList', filterExpression, function (result) {
                    if (result != null) {
                        $('#<%=hdnLocationIDFrom.ClientID %>').val(result.LocationID);
                        $('#<%=txtLocationName.ClientID %>').val(result.LocationName);

                        filterExpression = "LocationID = " + result.LocationID;
                        Methods.getObject('GetLocationList', filterExpression, function (result) {
                            $('#<%=hdnGCLocationGroupFrom.ClientID %>').val(result.GCLocationGroup);
                        });
                    }
                    else {
                        $('#<%=hdnLocationIDFrom.ClientID %>').val('');
                        $('#<%=txtLocationCode.ClientID %>').val('');
                        $('#<%=txtLocationName.ClientID %>').val('');
                    }
                    cbpView.PerformCallback('refresh');
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
                    $('#<%=txtProductLineCode.ClientID %>').val(value);
                    onTxtProductLineCodeChanged(value);
                });
            });

            $('#<%=txtProductLineCode.ClientID %>').change(function () {
                onTxtProductLineCodeChanged($(this).val());
            });

            function onTxtProductLineCodeChanged(value) {
                var filterExpression = getProductLineFilterExpression() + " AND ProductLineCode = '" + value + "'";
                Methods.getObject('GetProductLineList', filterExpression, function (result) {
                    if (result != null) {
                        $('#<%=hdnProductLineID.ClientID %>').val(result.ProductLineID);
                        $('#<%=txtProductLineName.ClientID %>').val(result.ProductLineName);
                        $('#<%=hdnProductLineItemType.ClientID %>').val(result.GCItemType);
                    }
                    else {
                        $('#<%=hdnProductLineID.ClientID %>').val('');
                        $('#<%=txtProductLineCode.ClientID %>').val('');
                        $('#<%=txtProductLineName.ClientID %>').val('');
                        $('#<%=hdnProductLineItemType.ClientID %>').val('');
                    }
                });
//                cbpView.PerformCallback('refresh');
            }
            //#endregion

//            var pageCount = parseInt($('#<%=hdnPageCount.ClientID %>').val());
//            setPaging($("#paging"), pageCount, function (page) {
//                getCheckedMember();
//                cbpView.PerformCallback('changepage|' + page);
//            });
        }

        $('#chkSelectAll').die('change');
        $('#chkSelectAll').live('change', function () {
            var isChecked = $(this).is(":checked");
            $('.chkIsSelected input').each(function () {
                $(this).prop('checked', isChecked);
                $(this).change();
            });
        });

        $('.chkIsSelected input').live('change', function () {
            $tr = $(this).closest('tr');
            if ($(this).is(':checked')) {
                if ($('#<%=hdnIsPurchaseOrder.ClientID %>').val() == '1') {
                    $tr.find('.txtPurchaseRequest').removeAttr('readonly');
                    $tr.find('.txtPurchaseRequestRI').removeAttr('readonly');
                    $tr.find('.txtPurchaseRequestRJ').removeAttr('readonly');
                    $tr.find('.txtPurchaseRequest').focus();
                } else {
                    $tr.find('.txtPurchaseRequest').removeAttr('readonly');
                    $tr.find('.txtPurchaseRequestRI').attr('readonly', 'readonly');
                    $tr.find('.txtPurchaseRequestRJ').attr('readonly', 'readonly');
                    $tr.find('.txtPurchaseRequestRI').val('0.00');
                    $tr.find('.txtPurchaseRequestRJ').val('0.00');
                    $tr.find('.txtPurchaseRequest').focus();
                }
            }
            else {
                $tr.find('.txtPurchaseRequest').attr('readonly', 'readonly');
                $tr.find('.txtPurchaseRequestRI').attr('readonly', 'readonly');
                $tr.find('.txtPurchaseRequestRJ').attr('readonly', 'readonly');
            }
        });

        function onAfterCustomClickSuccess(type, retval) {
            showToast('Save Success', 'Permintaan Pembelian Berhasil Dibuat Dengan No Permintaan <b>' + retval + '</b>', function () {
                $('#<%=hdnPurchaseRequest.ClientID %>').val('');
                $('#<%=hdnPurchaseRequestRI.ClientID %>').val('');
                $('#<%=hdnPurchaseRequestRJ.ClientID %>').val('');
                $('#<%=hdnSelectedMember.ClientID %>').val('');
                $('#<%=hdnRecommendedQty.ClientID %>').val('');
                cbpView.PerformCallback('refresh');
            });
        }

        function getCheckedMember() {
            var lstSelectedMember = $('#<%=hdnSelectedMember.ClientID %>').val().split('|');
            var lstPurchaseRequest = $('#<%=hdnPurchaseRequest.ClientID %>').val().split('|');
            var lstPurchaseRequestRI = $('#<%=hdnPurchaseRequestRI.ClientID %>').val().split('|');
            var lstPurchaseRequestRJ = $('#<%=hdnPurchaseRequestRJ.ClientID %>').val().split('|');
            var lstRecommendedQty = $('#<%=hdnRecommendedQty.ClientID %>').val().split('|');
            $('#<%=grdView.ClientID %> .chkIsSelected input').each(function () {
                if ($(this).is(':checked')) {
                    var key = $(this).closest('tr').find('.keyField').html();
                    var purchaseRequest = $(this).closest('tr').find('.txtPurchaseRequest').val();
                    var purchaseRequestRI = $(this).closest('tr').find('.txtPurchaseRequestRI').val();
                    var purchaseRequestRJ = $(this).closest('tr').find('.txtPurchaseRequestRJ').val();
                    var recommendQty = $(this).closest('tr').find('.txtRecommendedQty').val();

                    var idx = lstSelectedMember.indexOf(key);
                    if (idx < 0) {
                        lstSelectedMember.push(key);
                        lstPurchaseRequest.push(purchaseRequest);
                        lstPurchaseRequestRI.push(purchaseRequestRI);
                        lstPurchaseRequestRJ.push(purchaseRequestRJ);
                        lstRecommendedQty.push(recommendQty);
                    }
                    else {
                        lstPurchaseRequest[idx] = purchaseRequest;
                        lstPurchaseRequestRI[idx] = purchaseRequestRI;
                        lstPurchaseRequestRJ[idx] = purchaseRequestRJ;
                        lstRecommendedQty[idx] = recommendQty;
                    }
                }
                else {
                    var key = $(this).closest('tr').find('.keyField').html();
                    var purchaseRequest = $(this).closest('tr').find('.txtPurchaseRequest').val();
                    var recommendQty = $(this).closest('tr').find('.txtRecommendedQty').val();
                    var idx = lstSelectedMember.indexOf(key);
                    if (idx > -1) {
                        lstSelectedMember.splice(idx, 1);
                        lstPurchaseRequest.splice(idx, 1);
                        lstPurchaseRequestRI.splice(idx, 1);
                        lstPurchaseRequestRJ.splice(idx, 1);
                        lstRecommendedQty.splice(idx, 1);
                    }
                }
            });
            $('#<%=hdnPurchaseRequest.ClientID %>').val(lstPurchaseRequest.join('|'));
            $('#<%=hdnPurchaseRequestRI.ClientID %>').val(lstPurchaseRequestRI.join('|'));
            $('#<%=hdnPurchaseRequestRJ.ClientID %>').val(lstPurchaseRequestRJ.join('|'));
            $('#<%=hdnSelectedMember.ClientID %>').val(lstSelectedMember.join('|'));
            $('#<%=hdnRecommendedQty.ClientID %>').val(lstRecommendedQty.join('|'));
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
                    getCheckedMember();
                    cbpView.PerformCallback('changepage|' + page);
                });
            }
            else
                $('#<%=grdView.ClientID %> tr:eq(1)').click();
        }
        //#endregion

//        $('#<%=rblROPDynamic.ClientID %>').live('change', function () {
//            cbpView.PerformCallback('refresh');
//        });

        $('#<%=rblItemType.ClientID %>').live('change', function () {
            cbpView.PerformCallback('refresh');
        });

//        $('#<%=rblDisplayOption.ClientID %>').live('change', function () {
////            var displayOption = $('#<%=rblDisplayOption.ClientID %>').find(":checked").val();
////            $('#<%=hdnDisplayOption.ClientID %>').val(displayOption);
////            cbpView.PerformCallback('refresh');
//        });

//        $('#<%=rblByQty.ClientID %>').live('change', function () {
//            cbpView.PerformCallback('refresh');
//        });

        $('#<%=rblByLocation.ClientID %>').live('change', function () {
            cbpView.PerformCallback('refresh');
        });

        $('.lblPurchaseRequestQty.lblLink').live('click', function () {
            $tr = $(this).closest('tr').closest('tr');
            var locationID = $('#<%=hdnLocationIDFrom.ClientID %>').val();
            var itemID = $tr.find('.keyField').html();
            var id = itemID + '|' + locationID;
            var url = ResolveUrl("~/Program/Information/QtyDetailInfo/ItemPurchaseRequestQtyDtCtl.ascx");
            openUserControlPopup(url, id, 'On Purchase Request - Detail', 800, 500);
        });

        $('.lblPurchaseOrderQty.lblLink').live('click', function () {
            $tr = $(this).closest('tr').closest('tr');
            var locationID = $('#<%=hdnLocationIDFrom.ClientID %>').val();
            var itemID = $tr.find('.keyField').html();
            var id = itemID + '|' + locationID;
            var url = ResolveUrl("~/Program/Information/QtyDetailInfo/PurchaseOrderQtyDtCtl.ascx");
            openUserControlPopup(url, id, 'On Purchase Order - Detail', 800, 500);
        });

        $('.txtPurchaseRequest').live('change', function () {
            $tr = $(this).closest('tr').closest('tr');
            var qtyMax = $tr.find('.hdnQtyMax').html();
            var hdnSetVarAllowMax = $('#<%=hdnQtyPRAllowBiggerThanQtyMax.ClientID %>').val();
            if (hdnSetVarAllowMax == "0") {
                $tr.find('.txtPurchaseRequest').attr('max', qtyMax);
                showToast('Warning', 'Maaf, jumlah yang diminta tidak boleh lebih besar dari nilai maksimal sejumlah ' + qtyMax);
                $tr.find('.txtPurchaseRequest').val(qtyMax);
            } else {
                $tr.find('.txtPurchaseRequest').removeAttr('max');
            }

            var qtyRecommed = $tr.find('.hdnRecommendQtyPR').val();
            var hdnSetVarAllowRecommend = $('#<%=hdnQtyPRAllowBiggerThanReccomend.ClientID %>').val();
            if (hdnSetVarAllowRecommend == "0") {
                $tr.find('.txtPurchaseRequest').attr('max', qtyRecommed);
                showToast('Warning', 'Maaf, jumlah yang diminta tidak boleh lebih besar dari nilai rekomendasi sistem sejumlah ' + qtyRecommed);
                $tr.find('.txtPurchaseRequest').val(qtyRecommed);
            } else {
                $tr.find('.txtPurchaseRequest').removeAttr('max');
            }
        });

        $('.txtPurchaseRequestRI').live('change', function () {
            $tr = $(this).closest('tr').closest('tr');
            var qty = parseInt($tr.find('.txtPurchaseRequest').val());
            var qtyRI = parseInt($tr.find('.txtPurchaseRequestRI').val());
            if (qtyRI > qty) {
                showToast('Warning', 'Maaf, jumlah yang diminta tidak boleh lebih besar dari nilai diminta sejumlah ' + qtyRI);
                $tr.find('.txtPurchaseRequestRJ').val(0);
                $tr.find('.txtPurchaseRequestRI').val(0);
            } else {
                var qtyRJ = (qty - qtyRI)
                $tr.find('.txtPurchaseRequestRJ').val(qtyRJ);
            }
        });

        $('.txtPurchaseRequestRJ').live('change', function () {
            $tr = $(this).closest('tr').closest('tr');
            var qty = parseInt($tr.find('.txtPurchaseRequest').val());
            var qtyRJ = parseInt($tr.find('.txtPurchaseRequestRJ').val());
            if (qtyRJ > qty) {
                showToast('Warning', 'Maaf, jumlah yang diminta tidak boleh lebih besar dari nilai diminta sejumlah ' + qtyRJ);
                $tr.find('.txtPurchaseRequestRJ').val(0);
                $tr.find('.txtPurchaseRequestRI').val(0);
            } else {
                var qtyRI = (qty - qtyRJ)
                $tr.find('.txtPurchaseRequestRI').val(qtyRI);
            }
        });

        function onBeforeLoadRightPanelContent(code) {
            var key = '';
            return key
        }
    </script>
    <input type="hidden" value="" id="hdnItemID" runat="server" />
    <input type="hidden" value="" id="hdnIsPurchaseOrder" runat="server" />
    <input type="hidden" value="" id="hdnPageCount" runat="server" />
    <input type="hidden" id="hdnSelectedMember" runat="server" value="" />
    <input type="hidden" id="hdnPurchaseRequest" runat="server" value="" />
    <input type="hidden" id="hdnRecommendedQty" runat="server" value="" />
    <input type="hidden" id="hdnPurchaseRequestRI" runat="server" value="" />
    <input type="hidden" id="hdnPurchaseRequestRJ" runat="server" value="" />
    <input type="hidden" value="" id="hdnPageTitle" runat="server" />
    <input type="hidden" value="1" id="hdnDisplayOption" runat="server" />
    <input type="hidden" value="0" id="hdnSortByQuantityEND" runat="server" />
    <input type="hidden" value="0" id="hdnAutoApprovePR" runat="server" />
    <input type="hidden" value="0" id="hdnIsUsedProductLine" runat="server" />
    <input type="hidden" value="1" id="hdnIsOutstandingPOPRVisible" runat="server" />
    <input type="hidden" value="1" id="hdnQtyPRAllowBiggerThanQtyMax" runat="server" />
    <input type="hidden" value="1" id="hdnQtyPRAllowBiggerThanReccomend" runat="server" />
    <input type="hidden" value="1" id="hdnIM0131" runat="server" />    
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
                                <label class="lblMandatory lblLink" runat="server" id="lblLocation">
                                    <%=GetLabel("Dari Lokasi")%></label>
                            </td>
                            <td>
                                <input type="hidden" id="hdnLocationIDFrom" value="" runat="server" />
                                <input type="hidden" id="hdnGCLocationGroupFrom" value="" runat="server" />
                                <table style="width: 100%" cellpadding="0" cellspacing="0">
                                    <colgroup>
                                        <col style="width: 30%" />
                                        <col style="width: 3px" />
                                    </colgroup>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtLocationCode" Width="100%" runat="server" />
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtLocationName" Width="100%" runat="server" ReadOnly="true" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel" style="vertical-align: top; padding-top: 5px;">
                                <%=GetLabel("Keterangan") %>
                            </td>
                            <td>
                                <asp:TextBox ID="txtNotes" Width="100%" runat="server" TextMode="MultiLine" Rows="3" />
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
                            <td class="tdLabel">
                                <%=GetLabel("Tanggal") %>
                                -
                                <%=GetLabel("Waktu") %>
                            </td>
                            <td>
                                <table cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td style="padding-right: 1px; width: 145px">
                                            <asp:TextBox ID="txtPurchaseRequestDate" Width="120px" CssClass="datepicker" runat="server" />
                                        </td>
                                        <td style="width: 5px">
                                            &nbsp;
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtPurchaseRequestTime" Width="60px" CssClass="time" runat="server"
                                                Style="text-align: center" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr id="trProductLine" runat="server" style="display: none">
                            <td>
                                <label class="lblLink lblMandatory" runat="server" id="lblProductLine">
                                    <%=GetLabel("Product Line")%></label>
                            </td>
                            <td>
                                <input type="hidden" id="hdnProductLineID" value="" runat="server" />
                                <input type="hidden" id="hdnProductLineItemType" value="" runat="server" />
                                <table style="width: 100%" cellpadding="0" cellspacing="0">
                                    <colgroup>
                                        <col style="width: 30%" />
                                        <col style="width: 3px" />
                                        <col />
                                    </colgroup>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtProductLineCode" Width="100%" runat="server" />
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtProductLineName" Width="100%" runat="server" ReadOnly="true" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr id="trItemType" runat="server" style="display: none">
                            <td class="tdLabel">
                                <%=GetLabel("Jenis Item")%>
                            </td>
                            <td>
                                <asp:RadioButtonList ID="rblItemType" runat="server" RepeatDirection="Horizontal">
                                    <asp:ListItem Text="Semua" Value="0" Selected="True" />
                                    <asp:ListItem Text="Obat" Value="2" />
                                    <asp:ListItem Text="Alkes" Value="3" />
                                    <asp:ListItem Text="Barang Umum" Value="8" />
                                </asp:RadioButtonList>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <%=GetLabel("R.O.P")%>
                            </td>
                            <td>
                                <asp:RadioButtonList ID="rblROPDynamic" runat="server" RepeatDirection="Horizontal">
                                    <asp:ListItem Text="Statis" Value="false" Selected="True" />
                                    <asp:ListItem Text="Dinamis" Value="true" />
                                </asp:RadioButtonList>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <%=GetLabel("Display Option")%>
                            </td>
                            <td>
                                <asp:RadioButtonList ID="rblDisplayOption" class runat="server" RepeatDirection="Horizontal">
                                    <asp:ListItem Text="Semua" Value="0" />
                                    <asp:ListItem Text="Rekomendasi Sistem" Value="1" Selected="True" />
                                </asp:RadioButtonList>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <table style="width: 100%" cellpadding="0" cellspacing="0">
                                    <colgroup>
                                        <col style="width: 50%" />
                                        <col style="width: 50%" />
                                    </colgroup>
                                    <tr>
                                        <td>
                                            <table style="width: 100%" cellpadding="0" cellspacing="0">
                                                <colgroup>
                                                    <col style="width: 20%" />
                                                    <col style="width: 3px" />
                                                    <col />
                                                </colgroup>
                                                <tr>
                                                    <td class="tdLabel" style="font-style: oblique; color: Red">
                                                        <%=GetLabel("By Qty")%>
                                                    </td>
                                                    <td>
                                                        <asp:RadioButtonList ID="rblByQty" runat="server" RepeatDirection="Horizontal">
                                                            <asp:ListItem Text="MIN" Value="min" />
                                                            <asp:ListItem Text="MAX" Value="max" Selected="True" />
                                                        </asp:RadioButtonList>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                        <td>
                                            <table style="width: 100%" cellpadding="0" cellspacing="0">
                                                <colgroup>
                                                    <col style="width: 30%" />
                                                    <col style="width: 3px" />
                                                    <col />
                                                </colgroup>
                                                <tr>
                                                    <td class="tdLabel" style="font-style: oblique; color: Red">
                                                        <%=GetLabel("By Location")%>
                                                    </td>
                                                    <td>
                                                        <asp:RadioButtonList ID="rblByLocation" runat="server" RepeatDirection="Horizontal">
                                                            <asp:ListItem Text="On Hand" Value="0" Selected="True"/>
                                                        </asp:RadioButtonList>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
                        ShowLoadingPanel="false" OnCallback="cbpView_Callback">
                        <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ onCbpViewEndCallback(s); }" />
                        <PanelCollection>
                            <dx:PanelContent ID="PanelContent1" runat="server">
                                <asp:Panel runat="server" ID="pnlView" Style="width: 100%; margin-left: auto; margin-right: auto;
                                    position: relative; font-size: 0.95em; height: 650px">
                                    <asp:GridView ID="grdView" runat="server" CssClass="grdService grdNormal notAllowSelect"
                                        AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty"
                                        OnRowDataBound="grdView_RowDataBound">
                                        <Columns>
                                            <asp:BoundField DataField="ItemID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
<%--                                           <asp:BoundField DataField="ItemID" HeaderStyle-CssClass="hiddenColumn" ItemStyle-CssClass="hiddenColumn" />
--%>                                            <asp:TemplateField HeaderStyle-Width="40px" ItemStyle-HorizontalAlign="Center">
                                                <HeaderTemplate>
                                                    <input id="chkSelectAll" type="checkbox" />
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chkIsSelected" runat="server" CssClass="chkIsSelected" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="ItemName1" HeaderText="Nama Item" HeaderStyle-HorizontalAlign="Left" />
                                            <asp:BoundField DataField="ItemUnit" HeaderText="Satuan" HeaderStyle-Width="80px"
                                                HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                                            <asp:BoundField DataField="CustomMinimum2" HeaderText="Minimum" HeaderStyle-Width="60px"
                                                HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" />
                                            <asp:BoundField DataField="CustomMaximum2" HeaderText="Maximum" HeaderStyle-Width="60px"
                                                HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" HeaderStyle-CssClass="hdnQtyMax"
                                                ItemStyle-CssClass="hdnQtyMax" />
                                            <asp:BoundField DataField="CustomAverageQtyInString" HeaderText="Average Usage" HeaderStyle-Width="60px"
                                                HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" />
                                            <asp:BoundField DataField="CustomEndingBalance2" HeaderText="On HAND Location" HeaderStyle-Width="60px"
                                                HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" />
                                            <asp:TemplateField HeaderText="Purchase Request" ItemStyle-HorizontalAlign="Right"
                                                HeaderStyle-HorizontalAlign="Right" HeaderStyle-Width="60px">
                                                <ItemTemplate>
                                                    <label class="lblPurchaseRequestQty lblLink">
                                                        <%#:Eval("CustomPurchaseRequestQtyOnOrder2", "{0:N}")%></label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Purchase Order" ItemStyle-HorizontalAlign="Right"
                                                HeaderStyle-HorizontalAlign="Right" HeaderStyle-Width="60px">
                                                <ItemTemplate>
                                                    <label class="lblPurchaseOrderQty lblLink">
                                                        <%#:Eval("CustomQtyOnOrderPurchaseOrder2", "{0:N}")%></label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-Width="60px" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right"
                                                HeaderText="Reorder Qty">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtRecommendedQty" Width="60px" runat="server" CssClass="number txtRecommendedQty" Min="0"
                                                        ReadOnly="true" Text='<%#: Eval("CustomRecommendedQty")%>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-Width="135px" ItemStyle-HorizontalAlign="Left" HeaderText="DIMINTA (PROSES PR)">
                                                <ItemTemplate>
                                                    <input type="hidden" value="0" class="hdnRecommendQtyPR" id="hdnRecommendQtyPR" runat="server" />
                                                    <asp:TextBox ID="txtPurchaseRequest" Width="45%" runat="server" CssClass="number txtPurchaseRequest"
                                                        ReadOnly="true" />
                                                    &nbsp;
                                                    <%#: Eval("PurchaseUnit")%>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-Width="40px" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right"
                                                HeaderText="Qty RI">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtPurchaseRequestRI" Width="40px" runat="server" CssClass="number txtPurchaseRequestRI"
                                                        ReadOnly="true" Text='<%#: Eval("CustomRecommendedQty")%>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-Width="40px" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right"
                                                HeaderText="Qty RJ">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtPurchaseRequestRJ" Width="40px" runat="server" CssClass="number txtPurchaseRequestRJ"
                                                        ReadOnly="true" Text='<%#: Eval("CustomRecommendedQty")%>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="CustomConversionFactor" HeaderText="Faktor Konversi" HeaderStyle-Width="145px"
                                                HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                                        </Columns>
                                        <EmptyDataTemplate>
                                            <%=GetLabel("Tidak ada data Reorder Permintaan Pembelian untuk Lokasi ini")%>
                                        </EmptyDataTemplate>
                                    </asp:GridView>
                                </asp:Panel>
                            </dx:PanelContent>
                        </PanelCollection>
                    </dxcp:ASPxCallbackPanel>
<%--                    <div class="imgLoadingGrdView" id="containerImgLoadingView">
                        <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                    </div>--%>
<%--                    <div class="containerPaging">
                        <div class="wrapperPaging">
                            <div id="paging">
                            </div>
                        </div>
                    </div>--%>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
