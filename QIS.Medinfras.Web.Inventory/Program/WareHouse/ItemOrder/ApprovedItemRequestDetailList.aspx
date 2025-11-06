<%@ Page Title="" Language="C#" MasterPageFile="~/libs/MasterPage/MPTrx.master" AutoEventWireup="true"
    CodeBehind="ApprovedItemRequestDetailList.aspx.cs" Inherits="QIS.Medinfras.Web.Inventory.Program.ApprovedItemRequestDetailList" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<asp:Content ID="Content3" ContentPlaceHolderID="plhCustomButtonToolbar" runat="server">
    <li id="btnOrderListBack" crudmode="R" runat="server">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/back.png")%>' alt="" /><div>
            <%=GetLabel("Back")%></div>
    </li>
    <li id="btnItemReqHdProcess" crudmode="R" runat="server">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbprocess.png")%>' alt="" /><br style="clear: both" />
        <div>
            <%=GetLabel("Process")%></div>
    </li>
    <li id="btnItemReqHdDecline" crudmode="R" runat="server">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbvoid.png")%>' alt="" /><br style="clear: both" />
        <div>
            <%=GetLabel("Decline")%></div>
    </li>
    <li id="btnItemReqHdClose" crudmode="R" runat="server">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbset.png")%>' alt="" /><br style="clear: both" />
        <div>
            <%=GetLabel("Close")%></div>
    </li>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div class="menuTitle">
        <%=HttpUtility.HtmlEncode(GetMenuCaption())%></div>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript">
        function onLoad() {
            setRightPanelButtonEnabled();

            $('#<%=btnItemReqHdClose.ClientID %>').click(function () {
                getCheckedMember();
                if ($('#<%=hdnDataSave.ClientID %>').val() == '') {
                    showToast('Warning', 'Please Select Item First');
                }
                else {
                    onCustomButtonClick('close');
                }
            });

            $('#<%=btnItemReqHdDecline.ClientID %>').click(function () {
                getCheckedMember();
                if ($('#<%=hdnDataSave.ClientID %>').val() == '') {
                    showToast('Warning', 'Please Select Item First');
                }
                else {
                    onCustomButtonClick('decline');
                }
            });

            $('#<%=btnOrderListBack.ClientID %>').click(function () {
                showLoadingPanel();
                document.location = ResolveUrl('~/Program/Warehouse/ItemOrder/ApprovedItemRequestList.aspx?id=to');
            });

            $('.txtDistribution').change(function () {
                if ($(this).val() != '' && $(this).val() != '0') {
                    $(this).closest('tr').find('.txtConsumption').val('0');
                }
            });

            $('.txtConsumption').change(function () {
                if ($(this).val() != '' && $(this).val() != '0') {
                    $(this).closest('tr').find('.txtDistribution').val('0');
                }
            });
        }

        $('#<%=btnItemReqHdProcess.ClientID %>').live('click', function () {
            if (IsValid(null, 'fsMPEntry', 'mpEntry')) {
                getCheckedMember();
                if ($('#<%=hdnDataSave.ClientID %>').val() == '') {
                    showToast('Warning', 'Please Select Item First');
                }
                else {
                    var oLinkInfoPerIR = "";
                    var filterExpression = "ItemRequestID = " + $('#<%=hdnOrderID.ClientID %>').val();
                    Methods.getObject('GetvItemRequestHdLinkProcessList', filterExpression, function (resultIR) {
                        if (resultIR != null) {
                            oLinkInfoPerIR = resultIR.LinkInfo;

                            showToastConfirmation('Sudah pernah ada transaksi yg diproses sebelumnya di nomor <b>' + oLinkInfoPerIR + '</b>, lanjutkan proses?', function (result) {
                                if (result) {
                                    onCustomButtonClick('approve');
                                } else {
                                    showToast('Informasi', 'Proses dibatalkan');
                                }
                            });
                        } else {
                            onCustomButtonClick('approve');
                        }
                    });
                }
            }
        });

        function onBeforeRightPanelPrint(code, filterExpression, errMessage) {
            var itemRequestID = $('#<%=hdnOrderID.ClientID %>').val();
            filterExpression.text = "ItemRequestID = " + itemRequestID;
            return true;
        }

        $('.chkIsSelected input').live('change', function () {
            $tr = $(this).closest('tr');
            if ($(this).is(':checked')) {
                if ($('#<%=hdnIsAllowItemDistribution.ClientID %>').val() == '1') {
                    if ($tr.find('.txtDistribution').val() != "0" && $tr.find('.txtDistribution').val() != "0.00") {
                        $tr.find('.txtDistribution').removeAttr('readonly');
                    }
                    //                    if (parseInt($tr.find('.hdnPurchaseRequestQty').val()) != parseInt($tr.find('.hdnQuantityDetail').val())) {
                    //                        if ($tr.find('.txtDistribution').val() != "0" && $tr.find('.txtDistribution').val() != "0.00") {
                    //                            $tr.find('.txtDistribution').removeAttr('readonly');
                    //                        }
                    //                    } else {
                    //                        if (parseInt($tr.find('.hdnPurchaseRequestReceivedQty').val()) > 0) {
                    //                            if ($tr.find('.txtDistribution').val() != "0" && $tr.find('.txtDistribution').val() != "0.00") {
                    //                                $tr.find('.txtDistribution').removeAttr('readonly');
                    //                            }
                    //                        }
                    //                    }
                }
                if ($('#<%=hdnIsAllowItemConsumption.ClientID %>').val() == '1') {
                    if ($tr.find('.txtConsumption').val() != "0" && $tr.find('.txtConsumption').val() != "0.00") {
                        $tr.find('.txtConsumption').removeAttr('readonly');
                    }
                    //                    if (parseInt($tr.find('.hdnPurchaseRequestQty').val()) != parseInt($tr.find('.hdnQuantityDetail').val())) {
                    //                        if ($tr.find('.txtConsumption').val() != "0" && $tr.find('.txtConsumption').val() != "0.00") {
                    //                            $tr.find('.txtConsumption').removeAttr('readonly');
                    //                        }
                    //                    } else {
                    //                        if (parseInt($tr.find('.hdnPurchaseRequestReceivedQty').val()) > 0) {
                    //                            if ($tr.find('.txtConsumption').val() != "0" && $tr.find('.txtConsumption').val() != "0.00") {
                    //                                $tr.find('.txtConsumption').removeAttr('readonly');
                    //                            }
                    //                        }
                    //                    }
                }
                if ($('#<%=hdnIsAllowPurchaseRequest.ClientID %>').val() == '1') {
                    var IsPurchaseRequest = $('#<%=hdnIsPurchaseRequest.ClientID %>').val();
                    if (IsPurchaseRequest == "1") {
                        if ($tr.find('.txtPurchaseRequest').val() != "0" && $tr.find('.txtPurchaseRequest').val() != "0.00") {
                            $tr.find('.txtPurchaseRequest').removeAttr('readonly');
                        }
                    } else {
                        $tr.find('.txtPurchaseRequest').attr('readonly', 'readonly');
                    }
                }
                $tr.find('.txtRemark').removeAttr('readonly');
            }
            else {
                $tr.find('.txtDistribution').attr('readonly', 'readonly');
                $tr.find('.txtConsumption').attr('readonly', 'readonly');
                $tr.find('.txtPurchaseRequest').attr('readonly', 'readonly');
                $tr.find('.txtRemark').attr('readonly', 'readonly');
            }
        });

        $('#chkSelectAll').die('change');
        $('#chkSelectAll').live('change', function () {
            var isChecked = $(this).is(":checked");
            $('.chkIsSelected input').each(function () {
                $(this).prop('checked', isChecked);
                $(this).change();
            });
        });

        function onAfterCustomClickSuccess(type, retval) {
            var param = retval.split('|');
            var messageText = '';
            if (param[0] == "failed" || param[0] == "next") {
                if (param[0] == "failed") {
                    showToastConfirmation('Sudah pernah ada distribusi / pemakaian sebelumnya, lanjutkan?', function (result) {
                        if (result) {
                            onCustomButtonClick('approve');
                        } else {
                            showToast('Informasi', 'Proses dibatalkan');
                        }
                    });
                } else {
                    onCustomButtonClick('approve');
                }
                cbpView.PerformCallback('refresh');
            } else if (param[0] == "close") {
                showToast('Close Success', messageText, function () {
                    $('#<%=hdnDataSave.ClientID %>').val('');
                    if (param[0] == '0')
                        $('#<%=btnOrderListBack.ClientID %>').click();
                    cbpView.PerformCallback('refresh');
                });
            } else if (param[0] == "decline") {
                showToast('Decline Success', messageText, function () {
                    $('#<%=hdnDataSave.ClientID %>').val('');
                    if (param[0] == '0')
                        $('#<%=btnOrderListBack.ClientID %>').click();
                    cbpView.PerformCallback('refresh');
                });
            }
            else {
                if (param[2] != '')
                    messageText += 'Permintaan Pembelian Berhasil Dibuat Dengan No Transaksi <b>' + param[2] + '</b>';
                if (param[3] != '') {
                    if (messageText != '')
                        messageText += '<br />';
                    messageText += 'Distribusi Berhasil Dibuat Dengan No Transaksi <b>' + param[3] + '</b>';
                }
                if (param[4] != '') {
                    if (messageText != '')
                        messageText += '<br />';
                    messageText += 'Pemakaian Berhasil Dibuat Dengan No Transaksi <b>' + param[4] + '</b>';
                }
                showToast('Save Success', messageText, function () {
                    $('#<%=hdnDataSave.ClientID %>').val('');
                    if (param[0] == '0')
                        $('#<%=btnOrderListBack.ClientID %>').click();
                    cbpView.PerformCallback('refresh');
                });
            }
        }

        //tambah (untuk validasi ID yang masuk ke hdnSelectedMember tidak boleh doubel)
        Array.prototype.contains = function (obj) {
            var i = this.length;
            while (i--) {
                if (this[i] == obj) {
                    return true;
                }
            }
            return false;
        }
        //selesai

        function getCheckedMember() {
            var param = '';
            $('.grdItemRequest .chkIsSelected input').each(function () {
                if ($(this).is(':checked')) {
                    $tr = $(this).closest('tr');
                    var key = $tr.find('.keyField').html();
                    var itemRequestDtDistribution = $tr.find('.txtDistribution').val();
                    var itemRequestDtConsumption = $tr.find('.txtConsumption').val();
                    var itemRequestDtPR = $tr.find('.txtPurchaseRequest').val();
                    var itemID = $tr.find('.hdnItemID').val();
                    var remarks = $tr.find('.txtRemark').val();

                    if (param == '') {
                        param = '$setData|' + key + '|' + itemRequestDtDistribution + '|' + itemRequestDtConsumption + '|' + itemRequestDtPR + '|' + itemID + '|' + remarks;
                    }
                    else {
                        param += '$setData|' + key + '|' + itemRequestDtDistribution + '|' + itemRequestDtConsumption + '|' + itemRequestDtPR + '|' + itemID + '|' + remarks;
                    }
                    
                }
            });
            $('#<%=hdnDataSave.ClientID %>').val(param);
        }

        //#region Paging
        function onCbpViewEndCallback(s) {
            hideLoadingPanel();

            var param = s.cpResult.split('|');
            if (param[0] == 'refresh') {
                var pageCount = parseInt(param[1]);
                if (pageCount > 0)
                    $('.grdItemRequest tr:eq(1)').click();
            }
            else
                $('.grdItemRequest tr:eq(1)').click();
        }
        //#endregion

        $('.lblAvailableStock.lblLink').live("click", function () {
            $tr = $(this).closest('tr').parent().closest('tr');
            var itemID = $tr.find('.hdnItemID').val();
            var stock = $tr.find('.hdnQuantityEND').val();
            var itemUnit = $tr.find('.hdnItemUnit').val();
            var orderID = $('#<%=hdnOrderID.ClientID %>').val();
            var param = orderID + '|' + itemID + '|' + stock + '|' + itemUnit;
            var url = ResolveUrl("~/Program/Warehouse/ItemOrder/ItemRequestProcessedDtCtl.ascx");
            openUserControlPopup(url, param, 'Detail Penggunaan Item', 800, 500);
        });

        function onBeforeLoadRightPanelContent(code) {
            var requestID = $('#<%:hdnOrderID.ClientID %>').val();

            if (code == "copyItemRequest" || code == "infoDistribution" || code == "infoConsumption" || code == "infoPurchaseRequest") {
                if (requestID == "0" || requestID == "") {
                    showToast('Warning', 'Please Refresh First');
                } else {
                    return requestID;
                }
            }
        }

        function setRightPanelButtonEnabled() {
            var requestID = $('#<%:hdnOrderID.ClientID %>').val();

            if (requestID == "0" || requestID == "") {
                $('#btnInfoDistribution').attr('enabled', 'false');
                $('#btnInfoConsumption').attr('enabled', 'false');
                $('#btnInfoPurchaseRequest').attr('enabled', 'false');
            }
            else {
                var isRequestCopy = $('#<%:hdnIsRequestCopy.ClientID %>').val();
                var isHasCopy = $('#<%:hdnIsHasCopyItemRequest.ClientID %>').val();
                var isHasOutstandingConsumption = $('#<%:hdnIsHasOutstandingConsumption.ClientID %>').val();
                var isHasProcessDistribution = $('#<%:hdnIsHasRequestProcess.ClientID %>').val();

                if (isRequestCopy == "1") { // cek apakah permintaan barang ini merupakan hasil salinan dari permintaan lain sebelumnya
                    $('#btnCopyItemRequest').attr('enabled', 'false');
                } else {
                    if (isHasCopy == "1") { // cek apakah permintaan barang ini sudah dilakukan proses salin
                        $('#btnCopyItemRequest').attr('enabled', 'false');
                    } else {
                        if (isHasOutstandingConsumption == "1") { // cek apakah permintaan ini memiliki jenis permintaan yang bukan distribusi
                            $('#btnCopyItemRequest').attr('enabled', 'false');
                        } else {
                            if (isHasProcessDistribution == "1") { // cek apakah permintaan ini sudah memiliki proses distribusi / pemakaian / minta beli
                                $('#btnCopyItemRequest').attr('enabled', 'false');
                            } else {
                                $('#btnCopyItemRequest').removeAttr('enabled');
                            }
                        }
                    }
                }

                $('#btnInfoDistribution').removeAttr('enabled');
                $('#btnInfoConsumption').removeAttr('enabled');
                $('#btnInfoPurchaseRequest').removeAttr('enabled');
            }
        }
    </script>
    <input type="hidden" value="" id="hdnParamID" runat="server" />
    <input type="hidden" value="" id="hdnOrderID" runat="server" />
    <input type="hidden" value="" id="hdnDefaultGCConsumptionType" runat="server" />
    <input type="hidden" value="" id="hdnIsAllowPurchaseRequest" runat="server" />
    <input type="hidden" value="" id="hdnIsAllowItemConsumption" runat="server" />
    <input type="hidden" value="" id="hdnIsAllowItemDistribution" runat="server" />
    <input type="hidden" value="0" id="hdnIsUsedProductLine" runat="server" />
    <input type="hidden" value="" id="hdnProductLineID" runat="server" />
    <input type="hidden" value="" id="hdnIsPurchaseRequest" runat="server" />
    <input type="hidden" value="0" id="hdnIsRequestCopy" runat="server" />
    <input type="hidden" value="0" id="hdnIsHasCopyItemRequest" runat="server" />
    <input type="hidden" value="0" id="hdnIsHasOutstandingConsumption" runat="server" />
    <input type="hidden" value="0" id="hdnIsHasRequestProcess" runat="server" />
    <input type="hidden" id="hdnDataSave" value="" runat="server" />
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
                                <label class="lblNormal" id="lblOrderNo">
                                    <%=GetLabel("No. Permintaan")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtOrderNo" Width="150px" ReadOnly="true" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal" runat="server" id="lblLocation">
                                    <%=GetLabel("Dari Lokasi")%></label>
                            </td>
                            <td>
                                <input type="hidden" id="hdnLocationIDFrom" value="" runat="server" />
                                <table style="width: 100%" cellpadding="0" cellspacing="0">
                                    <colgroup>
                                        <col style="width: 30%" />
                                        <col style="width: 3px" />
                                        <col />
                                    </colgroup>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtLocationCode" Width="100%" runat="server" ReadOnly="true" />
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
                            <td class="tdLabel">
                                <%=GetLabel("Keterangan") %>
                            </td>
                            <td>
                                <asp:TextBox ID="txtNotes" ReadOnly="true" Width="100%" runat="server" TextMode="MultiLine"
                                    Rows="2" />
                            </td>
                        </tr>
                        <tr>
                            <input type="hidden" id="hdnRegistrationID" value="" runat="server" />
                            <td class="tdLabel">
                                <%=GetLabel("No. Registrasi Pasien")%>
                            </td>
                            <td>
                                <asp:TextBox ID="txtRegistrationNo" Width="100%" ReadOnly="true" runat="server" />
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
                            <td class="tdLabel">
                                <%=GetLabel("Tanggal") %>
                                -
                                <%=GetLabel("Waktu") %>
                            </td>
                            <td>
                                <table cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td style="padding-right: 1px; width: 140px">
                                            <asp:TextBox ID="txtItemOrderDate" Width="120px" CssClass="datepicker" ReadOnly="true"
                                                runat="server" />
                                        </td>
                                        <td style="width: 5px">
                                            &nbsp;
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtItemOrderTime" Width="100px" CssClass="time" runat="server" ReadOnly="true"
                                                Style="text-align: center" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal" runat="server" id="lblLocationTo">
                                    <%=GetLabel("Kepada Lokasi")%></label>
                            </td>
                            <td>
                                <input type="hidden" id="hdnLocationIDTo" value="" runat="server" />
                                <table style="width: 100%" cellpadding="0" cellspacing="0">
                                    <colgroup>
                                        <col style="width: 30%" />
                                        <col style="width: 3px" />
                                        <col />
                                    </colgroup>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtLocationCodeTo" Width="100%" runat="server" ReadOnly="true" />
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtLocationNameTo" Width="100%" runat="server" ReadOnly="true" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal" id="Label1">
                                    <%=GetLabel("Disetujui Oleh")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtApprovedBy" Width="100%" ReadOnly="true" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <%=GetLabel("Disetujui Pada") %>
                            </td>
                            <td>
                                <table cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td style="padding-right: 1px; width: 140px">
                                            <asp:TextBox ID="txtItemOrderApprovedDate" Width="120px" CssClass="datepicker" ReadOnly="true"
                                                runat="server" />
                                        </td>
                                        <td style="width: 5px">
                                            &nbsp;
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtItemOrderApprovedTime" Width="100px" CssClass="time" runat="server"
                                                ReadOnly="true" Style="text-align: center" />
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
                                    position: relative; font-size: 0.95em; height: 800px; overflow-y: scroll;">
                                    <asp:ListView runat="server" ID="lvwView" OnItemDataBound="lvwView_ItemDataBound">
                                        <EmptyDataTemplate>
                                            <table id="tblView" runat="server" class="grdItemRequest grdSelected" cellspacing="0"
                                                rules="all">
                                                <tr>
                                                    <th class="keyField" rowspan="2">
                                                        &nbsp;
                                                    </th>
                                                    <th rowspan="2" style="width: 20px">
                                                        &nbsp;
                                                    </th>
                                                    <th rowspan="2">
                                                        <%=GetLabel("KODE BARANG")%>
                                                    </th>
                                                    <th rowspan="2">
                                                        <%=GetLabel("NAMA BARANG")%>
                                                    </th>
                                                    <th colspan="6">
                                                        <%=GetLabel("JUMLAH BARANG")%>
                                                    </th>
                                                    <th colspan="2">
                                                        <%=GetLabel("SEDANG DIPROSES")%>
                                                    </th>
                                                    <th colspan="3">
                                                        <%=GetLabel("JUMLAH PROSES")%>
                                                    </th>
                                                    <th rowspan="2">
                                                        <%=GetLabel("KETERANGAN")%>
                                                    </th>
                                                </tr>
                                                <tr>
                                                    <th style="width: 110px">
                                                        <%=GetLabel("Diminta")%>
                                                    </th>
                                                    <th style="width: 110px">
                                                        <%=GetLabel("Tersedia")%>
                                                    </th>
                                                    <th style="width: 110px">
                                                        <%=GetLabel("Bisa Digunakan")%>
                                                    </th>
                                                    <th style="width: 110px">
                                                        <%=GetLabel("Pemakaian Rata-Rata")%>
                                                    </th>
                                                    <th style="width: 110px">
                                                        <%=GetLabel("Pemakaian Rata-Rata (Bln Ini)")%>
                                                    </th>
                                                    <th style="width: 110px">
                                                        <%=GetLabel("Minta Beli")%>
                                                    </th>
                                                    <th style="width: 110px">
                                                        <%=GetLabel("Diterima")%>
                                                    </th>
                                                    <th style="width: 150px">
                                                        <%=GetLabel("Distribusi")%>
                                                    </th>
                                                    <th style="width: 150px">
                                                        <%=GetLabel("Pemakaian")%>
                                                    </th>
                                                    <th style="width: 150px">
                                                        <%=GetLabel("Minta Beli")%>
                                                    </th>
                                                </tr>
                                                <tr class="trEmpty">
                                                    <td colspan="15">
                                                        <%=GetLabel("No Data To Display")%>
                                                    </td>
                                                </tr>
                                            </table>
                                        </EmptyDataTemplate>
                                        <LayoutTemplate>
                                            <table id="tblView" runat="server" class="grdItemRequest grdSelected" cellspacing="0"
                                                rules="all">
                                                <tr>
                                                    <th class="keyField" rowspan="2">
                                                    </th>
                                                    <th rowspan="2" style="width: 20px; text-align: center">
                                                        <input id="chkSelectAll" type="checkbox" />
                                                    </th>
                                                    <th rowspan="2" style="width: 100px; text-align: left">
                                                        <%=GetLabel("KODE BARANG")%>
                                                    </th>
                                                    <th rowspan="2" style="text-align: left">
                                                        <%=GetLabel("NAMA BARANG")%>
                                                    </th>
                                                    <th colspan="6">
                                                        <%=GetLabel("JUMLAH BARANG")%>
                                                    </th>
                                                    <th colspan="2">
                                                        <%=GetLabel("SEDANG DIPROSES")%>
                                                    </th>
                                                    <th colspan="3">
                                                        <%=GetLabel("JUMLAH PROSES")%>
                                                    </th>
                                                    <th rowspan="2">
                                                        <%=GetLabel("KETERANGAN")%>
                                                    </th>
                                                </tr>
                                                <tr>
                                                    <th style="width: 100px">
                                                        <%=GetLabel("Saldo Peminta")%>
                                                    </th>
                                                    <th style="width: 100px">
                                                        <%=GetLabel("Diminta")%>
                                                    </th>
                                                    <th style="width: 100px">
                                                        <%=GetLabel("Tersedia")%>
                                                    </th>
                                                    <th style="width: 100px">
                                                        <%=GetLabel("Bisa Digunakan")%>
                                                    </th>
                                                    <th style="width: 110px">
                                                        <%=GetLabel("Pemakaian Rata-Rata")%>
                                                    </th>
                                                    <th style="width: 110px">
                                                        <%=GetLabel("Pemakaian Rata-Rata (Bln Ini)")%>
                                                    </th>
                                                    <th style="width: 100px">
                                                        <%=GetLabel("Minta Beli")%>
                                                    </th>
                                                    <th style="width: 100px">
                                                        <%=GetLabel("Diterima")%>
                                                    </th>
                                                    <th style="width: 140px">
                                                        <%=GetLabel("Distribusi")%>
                                                    </th>
                                                    <th style="width: 140px">
                                                        <%=GetLabel("Pemakaian")%>
                                                    </th>
                                                    <th style="width: 140px">
                                                        <%=GetLabel("Minta Beli")%>
                                                    </th>
                                                </tr>
                                                <tr runat="server" id="itemPlaceholder">
                                                </tr>
                                            </table>
                                        </LayoutTemplate>
                                        <ItemTemplate>
                                            <tr>
                                                <td class="keyField">
                                                    <%#: Eval("ID")%>
                                                </td>
                                                <td align="center">
                                                    <asp:CheckBox ID="chkIsSelected" runat="server" CssClass="chkIsSelected" />
                                                </td>
                                                <td>
                                                    <%#: Eval("ItemCode")%>
                                                </td>
                                                <td>
                                                    <input type="hidden" value='<%#:Eval("ItemID") %>' class="hdnItemID" />
                                                    <input type="hidden" value='<%#:Eval("Quantity") %>' class="hdnQuantityDetail" />
                                                    <input type="hidden" value='<%#:Eval("ItemUnit") %>' class="hdnItemUnit" />
                                                    <input type="hidden" value='<%#:Eval("EndingBalance") %>' class="hdnQuantityEND" />
                                                    <%#: Eval("ItemName1")%>
                                                </td>
                                                <td align="right">
                                                    <table cellpadding="0" cellspacing="0" style="width: 100%">
                                                        <colgroup>
                                                            <col />
                                                            <col style="width: 35px" />
                                                        </colgroup>
                                                        <tr>
                                                            <td class="lblReadOnlyText" align="right">
                                                                <%#: Eval("EndingBalanceFrom")%>
                                                            </td>
                                                            <td>
                                                                &nbsp;<%#: Eval("ItemUnit")%>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                                <td align="right">
                                                    <table cellpadding="0" cellspacing="0" style="width: 100%">
                                                        <colgroup>
                                                            <col />
                                                            <col style="width: 35px" />
                                                        </colgroup>
                                                        <tr>
                                                            <td class="lblReadOnlyText" align="right">
                                                                <%#: Eval("Quantity")%>
                                                            </td>
                                                            <td>
                                                                &nbsp;<%#: Eval("ItemUnit")%>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                                <td align="right">
                                                    <table cellpadding="0" cellspacing="0" style="width: 100%">
                                                        <colgroup>
                                                            <col />
                                                            <col style="width: 35px" />
                                                        </colgroup>
                                                        <tr>
                                                            <td class="lblReadOnlyText" align="right">
                                                                <%#: Eval("EndingBalance")%>
                                                            </td>
                                                            <td>
                                                                &nbsp;<%#: Eval("BaseUnit")%>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                                <td align="right">
                                                    <table cellpadding="0" cellspacing="0" style="width: 100%">
                                                        <colgroup>
                                                            <col />
                                                            <col style="width: 35px" />
                                                        </colgroup>
                                                        <tr>
                                                            <td class="lblReadOnlyText" align="right">
                                                                <label id="lblAvailableStock" runat="server" class="lblAvailableStock lblLink">
                                                                </label>
                                                            </td>
                                                            <td>
                                                                &nbsp<%#: Eval("BaseUnit")%>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                                <td align="right">
                                                    <table cellpadding="0" cellspacing="0" style="width: 100%">
                                                        <colgroup>
                                                            <col />
                                                            <col style="width: 35px" />
                                                        </colgroup>
                                                        <tr>
                                                             <td class="lblReadOnlyText" align="right">
                                                                <%#: Eval("ItemMovementPerDate")%>
                                                            </td>
                                                            <td>
                                                                &nbsp<%#: Eval("BaseUnit")%>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                                <td align="right">
                                                    <table cellpadding="0" cellspacing="0" style="width: 100%">
                                                        <colgroup>
                                                            <col />
                                                            <col style="width: 35px" />
                                                        </colgroup>
                                                        <tr>
                                                             <td class="lblReadOnlyText" align="right">
                                                                <%#: Eval("ItemMovementPerDateThisMonth")%>
                                                            </td>
                                                            <td>
                                                                &nbsp<%#: Eval("BaseUnit")%>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                                <td align="right">
                                                    <table cellpadding="0" cellspacing="0" style="width: 100%">
                                                        <colgroup>
                                                            <col />
                                                            <col style="width: 35px" />
                                                        </colgroup>
                                                        <tr>
                                                            <td class="lblReadOnlyText" align="right">
                                                                <%#: Eval("PurchaseRequestQty")%>
                                                            </td>
                                                            <td>
                                                                &nbsp;<%#: Eval("ItemUnit")%>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                                <td align="right">
                                                    <table cellpadding="0" cellspacing="0" style="width: 100%">
                                                        <colgroup>
                                                            <col />
                                                            <col style="width: 35px" />
                                                        </colgroup>
                                                        <tr>
                                                            <td class="lblReadOnlyText" align="right">
                                                                <%#: Eval("ApprovedDistributionQty")%>
                                                            </td>
                                                            <input type="hidden" value='<%#:Eval("ApprovedDistributionQty") %>' class="hdnPurchaseRequestReceivedQty" />
                                                            <td>
                                                                &nbsp;<%#: Eval("ItemUnit")%>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                                <td align="right">
                                                    <table cellpadding="0" cellspacing="0" style="width: 100%">
                                                        <colgroup>
                                                            <col />
                                                            <col style="width: 60px" />
                                                        </colgroup>
                                                        <tr>
                                                            <td>
                                                                <asp:TextBox ID="txtDistribution" Width="75px" runat="server" value="0" CssClass="number max txtDistribution"
                                                                    ReadOnly="true" />
                                                            </td>
                                                            <td>
                                                                &nbsp;
                                                                <%#: Eval("ItemUnit")%>
                                                                <input type="hidden" class="hdnDistributionQty" value='<%#: Eval("DistributionQty")%>' />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                                <td align="right">
                                                    <table cellpadding="0" cellspacing="0" style="width: 100%">
                                                        <colgroup>
                                                            <col />
                                                            <col style="width: 60px" />
                                                        </colgroup>
                                                        <tr>
                                                            <td>
                                                                <asp:TextBox ID="txtConsumption" Width="75px" runat="server" value="0" CssClass="number max txtConsumption"
                                                                    ReadOnly="true" />
                                                            </td>
                                                            <td>
                                                                &nbsp;
                                                                <%#: Eval("ItemUnit")%>
                                                                <input type="hidden" class="hdnConsumptionQty" value='<%#: Eval("ConsumptionQty")%>' />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                                <td align="right">
                                                    <table cellpadding="0" cellspacing="0" style="width: 100%">
                                                        <colgroup>
                                                            <col />
                                                            <col style="width: 60px" />
                                                        </colgroup>
                                                        <tr>
                                                            <td>
                                                                <asp:TextBox ID="txtPurchaseRequest" Width="75px" runat="server" value="0" CssClass="number txtPurchaseRequest"
                                                                    ReadOnly="true" />
                                                            </td>
                                                            <td>
                                                                &nbsp;
                                                                <%#: Eval("ItemUnit")%>
                                                                <input type="hidden" class="hdnPurchaseRequestQty" value='<%#: Eval("PurchaseRequestQty")%>' />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtRemark" Width="175px" runat="server" CssClass="txtRemark"
                                                        ReadOnly="true" />
                                                </td>
                                            </tr>
                                        </ItemTemplate>
                                    </asp:ListView>
                                </asp:Panel>
                            </dx:PanelContent>
                        </PanelCollection>
                    </dxcp:ASPxCallbackPanel>
                    <div class="imgLoadingGrdView" id="containerImgLoadingView">
                        <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                    </div>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
