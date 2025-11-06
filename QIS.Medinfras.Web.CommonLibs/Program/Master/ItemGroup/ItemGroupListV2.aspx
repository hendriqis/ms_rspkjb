<%@ Page Title="" Language="C#" MasterPageFile="~/libs/MasterPage/MPList.master"
    AutoEventWireup="true" CodeBehind="ItemGroupListV2.aspx.cs" Inherits="QIS.Medinfras.Web.CommonLibs.Program.ItemGroupListV2" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="plhList" runat="server">
    <script type="text/javascript" src='<%= ResolveUrl("~/Libs/Scripts/CustomGridViewList.js")%>'></script>
    <script type="text/javascript">
        $(function () {
            var grd = new customGridView();
            grd.init('<%=grdViewIGHD.ClientID %>', '<%=hdnID.ClientID %>', '<%=pnlView.ClientID %>', cbpView, 'paging');

            $('#<%=grdViewIGHD.ClientID %> > tbody > tr:gt(0):not(.trEmpty)').live('click', function () {
                if ($(this).attr('class') == 'selected') {
                    $('#<%=grdViewIGHD.ClientID %> tr.selected').removeClass('selected');
                    $(this).addClass('selected');
                    $('#<%=hdnID.ClientID %>').val($(this).find('.keyField').html());
                    cbpViewDt.PerformCallback('refresh');
                }
            });
            $('#<%=grdViewIGHD.ClientID %> tr:eq(1)').click();

        });

        function onRefreshControl(filterExpression) {
            $('#<%=hdnFilterExpression.ClientID %>').val(filterExpression);
            cbpView.PerformCallback('refresh');
        }

        function onGetCurrID() {
            return $('#<%=hdnID.ClientID %>').val();
        }

        function onGetFilterExpression() {
            return $('#<%=hdnFilterExpression.ClientID %>').val();
        }

        //#region Paging
        function onCbpViewEndCallback(s) {
            hideLoadingPanel();

            var param = s.cpResult.split('|');
            $('#<%=grdViewIGHD.ClientID %> tr:eq(1)').click();
            cbpViewDt.PerformCallback('refresh');
        }
        //#endregion

        //#region Paging Dt
        function onCbpViewDtEndCallback(s) {
            $('#containerImgLoadingViewDt').hide();

            var param = s.cpResult.split('|');
            if (param[0] == 'save') {
                if (param[1] == 'fail') {
                    showToast('Save Failed', 'Error Message : ' + param[2]);
                }
                else {
                    $('#containerPopupEntryData').hide();
                }
                cbpViewDt.PerformCallback('refresh');
            }
            else if (param[0] == 'delete') {
                if (param[1] == 'fail') {
                    showToast('Delete Failed', 'Error Message : ' + param[2]);
                }
                else {
                }
                cbpViewDt.PerformCallback('refresh');
            }

            $('#containerPopupEntryData').hide();
            $('#containerImgLoadingViewPopup').hide();
            $('#containerImgLoadingView').hide();
            $('#containerImgLoadingViewDt').hide();
        }
        //#endregion

        $('.lnkSubGroup a').live('click', function () {
            var id = $(this).closest('tr').find('.keyField').html();
            var url = ResolveUrl("~/Libs/Program/Master/ItemGroup/ItemGroupDetailListCtl.ascx");
            openUserControlPopup(url, id, 'Kelompok Item', 1200, 500);
        });

        //#region Link GLRevenue

        $('#<%=grdViewIGHD.ClientID %> tr:gt(0) td.tdExpand').live('click', function () {
            $tr = $(this).parent();
            $trDetail = $(this).parent().next();
            if ($trDetail.attr('class') != 'trDetail') {
                $trCollapse = $('.trDetail');

                $(this).find('.imgExpand').attr('src', '<%= ResolveUrl("~/Libs/Images/down-arrow.png")%>');

                var a = $('#containerGrdDetail').html();
                $newTr = $("<tr><td></td><td colspan='8'>" + a + "</td></tr>").attr('class', 'trDetail');
                $newTr.insertAfter($tr);

                if ($trCollapse != null) {
                    $trCollapse.prev().find('.imgExpand').attr('src', '<%= ResolveUrl("~/Libs/Images/right-arrow.png")%>');
                    $trCollapse.remove();
                }

                $('#<%=hdnExpandID.ClientID %>').val($tr.find('.keyField').html());
            }
            else {
                $(this).find('.imgExpand').attr('src', '<%= ResolveUrl("~/Libs/Images/right-arrow.png")%>');

                $trDetail.remove();
            }
        });

        $('.lnkItemGroupCOA').live('click', function () {
            var id = $('#<%=hdnExpandID.ClientID %>').val();
            var url = ResolveUrl("~/Libs/Program/Master/ItemGroup/GLRevenue/GLMappingItemGroupEntryCtl.ascx");
            openUserControlPopup(url, id, 'ItemGroup', 1200, 500);
        });

        $('.lnkItemGroupServiceUnitCOA').live('click', function () {
            var id = $('#<%=hdnExpandID.ClientID %>').val();
            var url = ResolveUrl("~/Libs/Program/Master/ItemGroup/GLRevenue/GLMappingItemGroupServiceUnitEntryCtl.ascx");
            openUserControlPopup(url, id, 'ItemGroup & ServiceUnit', 1200, 500);
        });

        $('.lnkItemGroupClassCOA').live('click', function () {
            var id = $('#<%=hdnExpandID.ClientID %>').val();
            var url = ResolveUrl("~/Libs/Program/Master/ItemGroup/GLRevenue/GLMappingItemGroupClassEntryCtl.ascx");
            openUserControlPopup(url, id, 'ItemGroup & Class', 1200, 500);
        });

        $('.lnkItemGroupClassServiceUnitCOA').live('click', function () {
            var id = $('#<%=hdnExpandID.ClientID %>').val();
            var url = ResolveUrl("~/Libs/Program/Master/ItemGroup/GLRevenue/GLRevenueItemGroupClassServiceUnitEntryCtl.ascx");
            openUserControlPopup(url, id, 'ItemGroup, Class & ServiceUnit', 1200, 500);
        });

        $('.lnkItemGroupServiceUnitbySourceUnitCOA').live('click', function () {
            var id = $('#<%=hdnExpandID.ClientID %>').val();
            var url = ResolveUrl("~/Libs/Program/Master/ItemGroup/GLRevenue/GLRevenueItemGroupServiceUnitBySourceUnitCtl.ascx");
            openUserControlPopup(url, id, 'ItemGroup, ServiceUnit & Source', 1200, 500);
        });

        $('.lnkItemGroupServiceUnitbySourceUnitbyClassCOA').live('click', function () {
            var id = $('#<%=hdnExpandID.ClientID %>').val();
            var url = ResolveUrl("~/Libs/Program/Master/ItemGroup/GLRevenue/GLRevenueItemGroupServiceUnitBySourceUnitbyClassCtl.ascx");
            openUserControlPopup(url, id, 'ItemGroup, ServiceUnit, Source & Class', 1200, 500);
        });

        $('.lnkItemGroupServiceUnitbySourceUnitbyClassbyParamedicCOA').live('click', function () {
            var id = $('#<%=hdnExpandID.ClientID %>').val();
            var url = ResolveUrl("~/Libs/Program/Master/ItemGroup/GLRevenue/GLRevenueItemGroupServiceUnitBySourceUnitbyClassbyParamedicCtl.ascx");
            openUserControlPopup(url, id, 'ItemGroup, ServiceUnit, Source, Class & Paramedic', 1200, 500);
        });

        $('.lnkItemGroupServiceUnitbyClassbyParamedicCOA').live('click', function () {
            var id = $('#<%=hdnExpandID.ClientID %>').val();
            var url = ResolveUrl("~/Libs/Program/Master/ItemGroup/GLRevenue/GLRevenueItemGroupServiceUnitbyClassbyParamedicCtl.ascx");
            openUserControlPopup(url, id, 'ItemGroup, ServiceUnit, Class & Paramedic', 1200, 500);
        });

        $('.lnkItemGroupServiceUnitCustomerLineCOA').live('click', function () {
            var id = $('#<%=hdnExpandID.ClientID %>').val();
            var url = ResolveUrl("~/Libs/Program/Master/ItemGroup/GLRevenue/GLRevenueItemGroupServiceUnitCustomerLineCtl.ascx");
            openUserControlPopup(url, id, 'ItemGroup, ServiceUnit & Customer Line', 1200, 500);
        });

        //#endregion

        //#region Link GLRevenue DT

        $('.tdExpandDt').live('click', function () {
            $trDt = $(this).parent();
            $trDetailDt = $(this).parent().next();

            if ($trDetailDt.attr('class') != 'trDetailDt') {
                $trCollapseDt = $('.trDetailDt');

                $(this).find('.imgExpandDt').attr('src', '<%= ResolveUrl("~/Libs/Images/down-arrow.png")%>');

                var a = $('#containerGrdDetailDt').html().replace('display:none', '');
                $newTrDt = $("<tr><td></td><td colspan='8'>" + a + "</td></tr>").attr('class', 'trDetailDt');
                $newTrDt.insertAfter($trDt);

                if ($trCollapseDt != null) {
                    $trCollapseDt.prev().find('.imgExpandDt').attr('src', '<%= ResolveUrl("~/Libs/Images/right-arrow.png")%>');
                    $trCollapseDt.remove();
                }

                $('#<%=hdnExpandIDDt.ClientID %>').val($trDt.find('.keyField').html());
            }
            else {
                $(this).find('.imgExpandDt').attr('src', '<%= ResolveUrl("~/Libs/Images/right-arrow.png")%>');

                $trDetailDt.remove();
            }
        });

        $('.lnkItemGroupCOADT').live('click', function () {
            var id = $('#<%=hdnExpandIDDt.ClientID %>').val();
            var url = ResolveUrl("~/Libs/Program/Master/ItemGroup/GLRevenue/GLMappingItemGroupEntryCtl.ascx");
            openUserControlPopup(url, id, 'ItemGroup', 1200, 500);
        });

        $('.lnkItemGroupServiceUnitCOADT').live('click', function () {
            var id = $('#<%=hdnExpandIDDt.ClientID %>').val();
            var url = ResolveUrl("~/Libs/Program/Master/ItemGroup/GLRevenue/GLMappingItemGroupServiceUnitEntryCtl.ascx");
            openUserControlPopup(url, id, 'ItemGroup & ServiceUnit', 1200, 500);
        });

        $('.lnkItemGroupClassCOADT').live('click', function () {
            var id = $('#<%=hdnExpandIDDt.ClientID %>').val();
            var url = ResolveUrl("~/Libs/Program/Master/ItemGroup/GLRevenue/GLMappingItemGroupClassEntryCtl.ascx");
            openUserControlPopup(url, id, 'ItemGroup & Class', 1200, 500);
        });

        $('.lnkItemGroupClassServiceUnitCOADT').live('click', function () {
            var id = $(this).closest('tr').find('.keyFieldDtDt').html();
            var url = ResolveUrl("~/Libs/Program/Master/ItemGroup/GLRevenue/GLRevenueItemGroupClassServiceUnitEntryCtl.ascx");
            openUserControlPopup(url, id, 'ItemGroup, Class & ServiceUnit', 1200, 500);
        });

        $('.lnkItemGroupServiceUnitbySourceUnitCOADT').live('click', function () {
            var id = $('#<%=hdnExpandIDDt.ClientID %>').val();
            var url = ResolveUrl("~/Libs/Program/Master/ItemGroup/GLRevenue/GLRevenueItemGroupServiceUnitBySourceUnitCtl.ascx");
            openUserControlPopup(url, id, 'ItemGroup, ServiceUnit & Source', 1200, 500);
        });

        $('.lnkItemGroupServiceUnitbySourceUnitbyClassCOADT').live('click', function () {
            var id = $('#<%=hdnExpandIDDt.ClientID %>').val();
            var url = ResolveUrl("~/Libs/Program/Master/ItemGroup/GLRevenue/GLRevenueItemGroupServiceUnitBySourceUnitbyClassCtl.ascx");
            openUserControlPopup(url, id, 'ItemGroup, ServiceUnit, Source & Class', 1200, 500);
        });

        $('.lnkItemGroupServiceUnitbySourceUnitbyClassbyParamedicCOADT').live('click', function () {
            var id = $('#<%=hdnExpandIDDt.ClientID %>').val();
            var url = ResolveUrl("~/Libs/Program/Master/ItemGroup/GLRevenue/GLRevenueItemGroupServiceUnitBySourceUnitbyClassbyParamedicCtl.ascx");
            openUserControlPopup(url, id, 'ItemGroup, ServiceUnit, Source, Class & Paramedic', 1200, 500);
        });

        $('.lnkItemGroupServiceUnitbyClassbyParamedicCOADT').live('click', function () {
            var id = $('#<%=hdnExpandIDDt.ClientID %>').val();
            var url = ResolveUrl("~/Libs/Program/Master/ItemGroup/GLRevenue/GLRevenueItemGroupServiceUnitbyClassbyParamedicCtl.ascx");
            openUserControlPopup(url, id, 'ItemGroup, ServiceUnit, Class & Paramedic', 1200, 500);
        });

        $('.lnkItemGroupServiceUnitCustomerLineCOADT').live('click', function () {
            var id = $('#<%=hdnExpandIDDt.ClientID %>').val();
            var url = ResolveUrl("~/Libs/Program/Master/ItemGroup/GLRevenue/GLRevenueItemGroupServiceUnitCustomerLineCtl.ascx");
            openUserControlPopup(url, id, 'ItemGroup, ServiceUnit & Customer Line', 1200, 500);
        });

        //#endregion

        //#region Sub Item Group

        $('#lblEntryPopupAddData').live('click', function () {
            $('#<%=hdnItemGroupID.ClientID %>').val('');
            $('#<%=txtItemGroupCode.ClientID %>').removeAttr('readonly');
            $('#<%=txtItemGroupCode.ClientID %>').val('');
            $('#<%=txtItemGroupName.ClientID %>').val('');
            $('#<%=txtItemGroupName2.ClientID %>').val('');
            $('#<%=hdnRevenueSharingID.ClientID %>').val('');
            $('#<%=txtRevenueSharingCode.ClientID %>').val('');
            $('#<%=txtRevenueSharingName.ClientID %>').val('');
            $('#<%=txtPrintOrder.ClientID %>').val('');
            $('#<%=txtNotes.ClientID %>').val('');
            $('#<%=hdnProductLineIDCtl.ClientID %>').val('');
            $('#<%=txtProductLineCodeCtl.ClientID %>').val('');
            $('#<%=txtProductLineNameCtl.ClientID %>').val('');
            $('#<%=hdnGLAccountID1.ClientID %>').val('');
            $('#<%=txtGLAccountCode1.ClientID %>').val('');
            $('#<%=txtGLAccountName1.ClientID %>').val('');
            $('#<%=hdnSubLedgerDtID1.ClientID %>').val('');
            $('#<%=txtSubLedgerDtCode1.ClientID %>').val('');
            $('#<%=txtSubLedgerDtName1.ClientID %>').val('');
            $('#<%=hdnGLAccountID2.ClientID %>').val('');
            $('#<%=txtGLAccountCode2.ClientID %>').val('');
            $('#<%=txtGLAccountName2.ClientID %>').val('');
            $('#<%=hdnSubLedgerDtID2.ClientID %>').val('');
            $('#<%=txtSubLedgerDtCode2.ClientID %>').val('');
            $('#<%=txtSubLedgerDtName2.ClientID %>').val('');
            $('#<%=hdnGLAccountID3.ClientID %>').val('');
            $('#<%=txtGLAccountCode3.ClientID %>').val('');
            $('#<%=txtGLAccountName3.ClientID %>').val('');
            $('#<%=hdnSubLedgerDtID3.ClientID %>').val('');
            $('#<%=txtSubLedgerDtCode3.ClientID %>').val('');
            $('#<%=txtSubLedgerDtName3.ClientID %>').val('');
            $('#<%=hdnGLAccountDiscountID.ClientID %>').val('');
            $('#<%=txtGLAccountDiscountCode.ClientID %>').val('');
            $('#<%=txtGLAccountDiscountName.ClientID %>').val('');
            if ($('#<%=hdnGCItemType.ClientID %>').val() == 'X001^001' || $('#<%=hdnGCItemType.ClientID %>').val() == 'X001^004'
                        || $('#<%=hdnGCItemType.ClientID %>').val() == 'X001^005' || $('#<%=hdnGCItemType.ClientID %>').val() == 'X001^006') {

                var filterExpression = "ItemGroupID = " + $('#<%=hdnID.ClientID %>').val();
                Methods.getObject('GetItemGroupMasterList', filterExpression, function (entity) {
                    if (entity != null) {
                        $('#<%=hdnParentCito.ClientID %>').val(entity.IsCitoInPercentage);
                        $('#<%=hdnParentComplication.ClientID %>').val(entity.IsComplicationInPercentage);
                        $('#<%=hdnPrintDoctorName.ClientID %>').val(entity.IsPrintWithDoctorName);
                        $('#<%=hdnParentCitoValue.ClientID %>').val(entity.CitoAmount);
                        $('#<%=hdnParentComplicationValue.ClientID %>').val(entity.ComplicationAmount);
                    }
                    else {
                        $('#<%=hdnParentCito.ClientID %>').val('0');
                        $('#<%=hdnParentComplication.ClientID %>').val('');
                        $('#<%=hdnPrintDoctorName.ClientID %>').val('');
                        $('#<%=hdnParentCitoValue.ClientID %>').val('');
                        $('#<%=hdnParentComplicationValue.ClientID %>').val('');
                    }
                });

                $cito = $('#<%=hdnParentCito.ClientID %>').val();
                $('#<%:chkIsCITOInPercentageCtl.ClientID %>').prop("checked", $cito == 'True');
                $citoValue = $('#<%=hdnParentCitoValue.ClientID %>').val();
                $('#<%=txtCITOAmountCtl.ClientID %>').val($citoValue);
                $complication = $('#<%=hdnParentComplication.ClientID %>').val();
                $('#<%:chkIsComplicationInPercentageCtl.ClientID %>').prop("checked", $complication == 'True');
                $complicationValue = $('#<%=hdnParentComplicationValue.ClientID %>').val();
                $('#<%=txtComplicationAmountCtl.ClientID %>').val($complicationValue);
                $isPrintDoctorName = $('#<%=hdnPrintDoctorName.ClientID %>').val();
                $('#<%:chkIsPrintWithDoctorNameCtl.ClientID %>').prop("checked", $isPrintDoctorName == 'True');
            }
            $('#containerPopupEntryData').show();
        });

        $('#btnEntryPopupCancel').live('click', function () {
            $('#containerPopupEntryData').hide();
        });

        $('#btnEntryPopupSave').live('click', function (evt) {
            if (IsValid(evt, 'fsEntryPopup', 'mpEntryPopup'))
                cbpViewDt.PerformCallback('save');
            return false;
        });

        $('.imgDelete.imgLink').die('click');
        $('.imgDelete.imgLink').live('click', function () {
            if (confirm("Are You Sure Want To Delete This Data?")) {
                $row = $(this).closest('tr');
                var itemGroupID = $row.find('.hdnItemGroupID').val();
                $('#<%=hdnItemGroupID.ClientID %>').val(itemGroupID);

                cbpViewDt.PerformCallback('delete');
            }
        });

        $('.imgEdit.imgLink').die('click');
        $('.imgEdit.imgLink').live('click', function () {
            $row = $(this).closest('tr');
            var itemGroupID = $row.find('.hdnItemGroupID').val();
            var itemGroupCode = $row.find('.hdnItemGroupCode').val();
            var itemGroupName = $row.find('.hdnItemGroupName').val();
            var itemGroupName2 = $row.find('.hdnItemGroupName2').val();
            var billingGroupID = $row.find('.BillingGroupID').val();
            var billingGroupCode = $row.find('.BillingGroupCode').val();
            var billingGroupName = $row.find('.BillingGroupName1').val();
            var revenueSharingID = $row.find('.hdnRevenueSharingID').val();
            var revenueSharingCode = $row.find('.hdnRevenueSharingCode').val();
            var revenueSharingName = $row.find('.hdnRevenueSharingName').val();
            var printOrder = $row.find('.hdnPrintOrder').val();
            var remarks = $row.find('.hdnRemarks').val();
            var ProductLineID = $row.find('.ProductLineID').val();
            var ProductLineCode = $row.find('.ProductLineCode').val();
            var ProductLineName = $row.find('.ProductLineName').val();
            var GLAccountID1 = $row.find('.GLAccountID1').val();
            var GLAccountCode1 = $row.find('.GLAccountCode1').val();
            var GLAccountName1 = $row.find('.GLAccountName1').val();
            var subLedgerDtID1 = $row.find('.SubLedgerDtID1').val();
            var subLedgerDtCode1 = $row.find('.SubLedgerDtCode1').val();
            var subLedgerDtName1 = $row.find('.SubLedgerDtName1').val();
            var GLAccountID2 = $row.find('.GLAccountID2').val();
            var GLAccountCode2 = $row.find('.GLAccountCode2').val();
            var GLAccountName2 = $row.find('.GLAccountName2').val();
            var subLedgerDtID2 = $row.find('.SubLedgerDtID2').val();
            var subLedgerDtCode2 = $row.find('.SubLedgerDtCode2').val();
            var subLedgerDtName2 = $row.find('.SubLedgerDtName2').val();
            var GLAccountID3 = $row.find('.GLAccountID3').val();
            var GLAccountCode3 = $row.find('.GLAccountCode3').val();
            var GLAccountName3 = $row.find('.GLAccountName3').val();
            var subLedgerDtID3 = $row.find('.SubLedgerDtID3').val();
            var subLedgerDtCode3 = $row.find('.SubLedgerDtCode3').val();
            var subLedgerDtName3 = $row.find('.SubLedgerDtName3').val();
            var GLAccountDiscount = $row.find('.GLAccountDiscount').val();
            var GLAccountDiscountNo = $row.find('.GLAccountDiscountNo').val();
            var GLAccountDiscountName = $row.find('.GLAccountDiscountName').val();
            var citoAmount = $row.find('.txtCitoAmount').val();
            var hdnIsCitoInPercentage = $row.find('.hdnIsCitoInPercentage').val();
            var complicationAmount = $row.find('.txtComplicationAmount').val();
            var hdnIsComplicationInPercentage = $row.find('.hdnIsComplicationInPercentage').val();
            var hdnIsPrintWithDoctorName = $row.find('.hdnIsPrintWithDoctorName').val();

            $('#<%=hdnItemGroupID.ClientID %>').val(itemGroupID);
            $('#<%=txtItemGroupCode.ClientID %>').val(itemGroupCode);
            $('#<%=txtItemGroupName.ClientID %>').val(itemGroupName);
            $('#<%=txtItemGroupName2.ClientID %>').val(itemGroupName2);
            $('#<%=hdnBillingGroupID.ClientID %>').val(billingGroupID);
            $('#<%=txtBillingGroupCode.ClientID %>').val(billingGroupCode);
            $('#<%=txtBillingGroupName.ClientID %>').val(billingGroupName);
            $('#<%=hdnRevenueSharingID.ClientID %>').val(revenueSharingID);
            $('#<%=txtRevenueSharingCode.ClientID %>').val(revenueSharingCode);
            $('#<%=txtRevenueSharingName.ClientID %>').val(revenueSharingName);
            $('#<%=txtPrintOrder.ClientID %>').val(printOrder);
            $('#<%=txtNotes.ClientID %>').val(remarks);
            $('#<%=hdnProductLineIDCtl.ClientID %>').val(ProductLineID);
            $('#<%=txtProductLineCodeCtl.ClientID %>').val(ProductLineCode);
            $('#<%=txtProductLineNameCtl.ClientID %>').val(ProductLineName);
            $('#<%=hdnGLAccountID1.ClientID %>').val(GLAccountID1);
            $('#<%=txtGLAccountCode1.ClientID %>').val(GLAccountCode1);
            $('#<%=txtGLAccountName1.ClientID %>').val(GLAccountName1);
            $('#<%=hdnSubLedgerDtID1.ClientID %>').val(subLedgerDtID1);
            $('#<%=txtSubLedgerDtCode1.ClientID %>').val(subLedgerDtCode1);
            $('#<%=txtSubLedgerDtName1.ClientID %>').val(subLedgerDtName1);
            $('#<%=hdnGLAccountID2.ClientID %>').val(GLAccountID2);
            $('#<%=txtGLAccountCode2.ClientID %>').val(GLAccountCode2);
            $('#<%=txtGLAccountName2.ClientID %>').val(GLAccountName2);
            $('#<%=hdnSubLedgerDtID2.ClientID %>').val(subLedgerDtID2);
            $('#<%=txtSubLedgerDtCode2.ClientID %>').val(subLedgerDtCode2);
            $('#<%=txtSubLedgerDtName2.ClientID %>').val(subLedgerDtName2);
            $('#<%=hdnGLAccountID3.ClientID %>').val(GLAccountID3);
            $('#<%=txtGLAccountCode3.ClientID %>').val(GLAccountCode3);
            $('#<%=txtGLAccountName3.ClientID %>').val(GLAccountName3);
            $('#<%=hdnSubLedgerDtID3.ClientID %>').val(subLedgerDtID3);
            $('#<%=txtSubLedgerDtCode3.ClientID %>').val(subLedgerDtCode3);
            $('#<%=txtSubLedgerDtName3.ClientID %>').val(subLedgerDtName3);
            $('#<%=hdnGLAccountDiscountID.ClientID %>').val(GLAccountDiscount);
            $('#<%=txtGLAccountDiscountCode.ClientID %>').val(GLAccountDiscountNo);
            $('#<%=txtGLAccountDiscountName.ClientID %>').val(GLAccountDiscountName);
            $('#<%:chkIsCITOInPercentageCtl.ClientID %>').prop("checked", hdnIsCitoInPercentage == 'True');
            $('#<%=txtCITOAmountCtl.ClientID %>').val(citoAmount);
            if ($('#<%=hdnGCItemType.ClientID %>').val() == 'X001^001') {
                $('#<%:chkIsComplicationInPercentageCtl.ClientID %>').prop("checked", hdnIsComplicationInPercentage == 'True');
                $('#<%=txtComplicationAmountCtl.ClientID %>').val(complicationAmount);
                $('#<%:chkIsPrintWithDoctorNameCtl.ClientID %>').prop("checked", hdnIsPrintWithDoctorName == 'True');
            }

            $('#containerPopupEntryData').show();
        });

        //#region Billing Group
        $('#lblBillingGroup.lblLink').live('click', function () {
            openSearchDialog('billinggroup', 'IsDeleted = 0', function (value) {
                $('#<%=txtBillingGroupCode.ClientID %>').val(value);
                onBillingGroupCodeChanged(value);
            });
        });

        $('#<%=txtBillingGroupCode.ClientID %>').live('change', function () {
            onBillingGroupCodeChanged($(this).val());
        });

        function onBillingGroupCodeChanged(value) {
            var filterExpression = "BillingGroupCode = '" + value + "'";
            Methods.getObject('GetBillingGroupList', filterExpression, function (result) {
                if (result != null) {
                    $('#<%=hdnBillingGroupID.ClientID %>').val(result.BillingGroupID);
                    $('#<%=txtBillingGroupName.ClientID %>').val(result.BillingGroupName1);
                }
                else {
                    $('#<%=hdnBillingGroupID.ClientID %>').val('');
                    $('#<%=txtBillingGroupCode.ClientID %>').val('');
                    $('#<%=txtBillingGroupName.ClientID %>').val('');
                }
            });
        }
        //#endregion

        //#region Revenue Sharing
        $('#lblRevenueSharing.lblLink').live('click', function () {
            openSearchDialog('revenuesharing', 'IsDeleted = 0', function (value) {
                $('#<%=txtRevenueSharingCode.ClientID %>').val(value);
                onTxtRevenueSharingCodeChanged(value);
            });
        });

        $('#<%=txtRevenueSharingCode.ClientID %>').live('change', function () {
            onTxtRevenueSharingCodeChanged($(this).val());
        });

        function onTxtRevenueSharingCodeChanged(value) {
            var filterExpression = "RevenueSharingCode = '" + value + "'";
            Methods.getObject('GetRevenueSharingHdList', filterExpression, function (result) {
                if (result != null) {
                    $('#<%=hdnRevenueSharingID.ClientID %>').val(result.RevenueSharingID);
                    $('#<%=txtRevenueSharingName.ClientID %>').val(result.RevenueSharingName);
                }
                else {
                    $('#<%=hdnRevenueSharingID.ClientID %>').val('');
                    $('#<%=txtRevenueSharingCode.ClientID %>').val('');
                    $('#<%=txtRevenueSharingName.ClientID %>').val('');
                }
            });
        }
        //#endregion

        //#region GL Account 1
        $('#lblGLAccount1.lblLink').live('click', function () {
            openSearchDialog('chartofaccount', onGetGLAccountFilterExpression(), function (value) {
                $('#<%=txtGLAccountCode1.ClientID %>').val(value);
                ontxtGLAccountCode1Changed(value);
            });
        });

        $('#<%=txtGLAccountCode1.ClientID %>').live('change', function () {
            ontxtGLAccountCode1Changed($(this).val());
        });

        function ontxtGLAccountCode1Changed(value) {
            var filterExpression = onGetGLAccountFilterExpression() + " AND GLAccountNo = '" + value + "'";
            Methods.getObject('GetvChartOfAccountList', filterExpression, function (result) {
                if (result != null) {
                    $('#<%=hdnGLAccountID1.ClientID %>').val(result.GLAccountID);
                    $('#<%=txtGLAccountName1.ClientID %>').val(result.GLAccountName);

                    $('#<%=hdnSubLedgerID1.ClientID %>').val(result.SubLedgerID);
                    $('#<%=hdnSearchDialogTypeName1.ClientID %>').val(result.SearchDialogTypeName);
                    $('#<%=hdnFilterExpression1.ClientID %>').val(result.FilterExpression);
                    $('#<%=hdnIDFieldName1.ClientID %>').val(result.IDFieldName);
                    $('#<%=hdnCodeFieldName1.ClientID %>').val(result.CodeFieldName);
                    $('#<%=hdnDisplayFieldName1.ClientID %>').val(result.DisplayFieldName);
                    $('#<%=hdnMethodName1.ClientID %>').val(result.MethodName);
                    onSubLedgerID1Changed();
                }
                else {
                    $('#<%=hdnGLAccountID1.ClientID %>').val('');
                    $('#<%=txtGLAccountCode1.ClientID %>').val('');
                    $('#<%=txtGLAccountName1.ClientID %>').val('');

                    $('#<%=hdnSubLedgerID1.ClientID %>').val('');
                    $('#<%=hdnSearchDialogTypeName1.ClientID %>').val('');
                    $('#<%=hdnFilterExpression1.ClientID %>').val('');
                    $('#<%=hdnIDFieldName1.ClientID %>').val('');
                    $('#<%=hdnCodeFieldName1.ClientID %>').val('');
                    $('#<%=hdnDisplayFieldName1.ClientID %>').val('');
                    $('#<%=hdnMethodName1.ClientID %>').val('');
                }

                $('#<%=hdnSubLedgerDtID1.ClientID %>').val('');
                $('#<%=txtSubLedgerDtCode1.ClientID %>').val('');
                $('#<%=txtSubLedgerDtName1.ClientID %>').val('');
            });
        }

        function onSubLedgerID1Changed() {
            if ($('#<%=hdnSubLedgerID1.ClientID %>').val() == '0' || $('#<%=hdnSubLedgerID1.ClientID %>').val() == '') {
                $('#<%=lblSubLedgerDt1.ClientID %>').attr('class', 'lblDisabled');
                $('#<%=txtSubLedgerDtCode1.ClientID %>').attr('readonly', 'readonly');
            }
            else {
                $('#<%=lblSubLedgerDt1.ClientID %>').attr('class', 'lblLink');
                $('#<%=txtSubLedgerDtCode1.ClientID %>').removeAttr('readonly');
            }
        }
        //#endregion

        //#region Sub Ledger 1
        function onGetSubLedgerDt1FilterExpression() {
            var filterExpression = $('#<%=hdnFilterExpression1.ClientID %>').val().replace('@SubLedgerID', $('#<%=hdnSubLedgerID1.ClientID %>').val());
            return filterExpression;
        }

        $('#<%=lblSubLedgerDt1.ClientID %>').live('click', function () {
            if ($('#<%=hdnSearchDialogTypeName1.ClientID %>').val() != '') {
                openSearchDialog($('#<%=hdnSearchDialogTypeName1.ClientID %>').val(), onGetSubLedgerDt1FilterExpression(), function (value) {
                    $('#<%=txtSubLedgerDtCode1.ClientID %>').val(value);
                    ontxtSubLedgerDtCode1Changed(value);
                });
            }
        });

        $('#<%=txtSubLedgerDtCode1.ClientID %>').live('change', function () {
            ontxtSubLedgerDtCode1Changed($(this).val());
        });

        function ontxtSubLedgerDtCode1Changed(value) {
            if ($('#<%=hdnSearchDialogTypeName1.ClientID %>').val() != '') {
                var filterExpression = onGetSubLedgerDt1FilterExpression() + " AND " + $('#<%=hdnCodeFieldName1.ClientID %>').val() + " = '" + value + "'";
                Methods.getObject($('#<%=hdnMethodName1.ClientID %>').val(), filterExpression, function (result) {
                    if (result != null) {
                        $('#<%=hdnSubLedgerDtID1.ClientID %>').val(result[$('#<%=hdnIDFieldName1.ClientID %>').val()]);
                        $('#<%=txtSubLedgerDtName1.ClientID %>').val(result[$('#<%=hdnDisplayFieldName1.ClientID %>').val()]);
                    }
                    else {
                        $('#<%=hdnSubLedgerDtID1.ClientID %>').val('');
                        $('#<%=txtSubLedgerDtCode1.ClientID %>').val('');
                        $('#<%=txtSubLedgerDtName1.ClientID %>').val('');
                    }
                });
            }
        }
        //#endregion

        //#region GL Account 2
        $('#lblGLAccount2.lblLink').live('click', function () {
            openSearchDialog('chartofaccount', onGetGLAccountFilterExpression(), function (value) {
                $('#<%=txtGLAccountCode2.ClientID %>').val(value);
                ontxtGLAccountCode2Changed(value);
            });
        });

        $('#<%=txtGLAccountCode2.ClientID %>').live('change', function () {
            ontxtGLAccountCode2Changed($(this).val());
        });

        function ontxtGLAccountCode2Changed(value) {
            var filterExpression = onGetGLAccountFilterExpression() + " AND GLAccountNo = '" + value + "'";
            Methods.getObject('GetvChartOfAccountList', filterExpression, function (result) {
                if (result != null) {
                    $('#<%=hdnGLAccountID2.ClientID %>').val(result.GLAccountID);
                    $('#<%=txtGLAccountName2.ClientID %>').val(result.GLAccountName);

                    $('#<%=hdnSubLedgerID2.ClientID %>').val(result.SubLedgerID);
                    $('#<%=hdnSearchDialogTypeName2.ClientID %>').val(result.SearchDialogTypeName);
                    $('#<%=hdnFilterExpression2.ClientID %>').val(result.FilterExpression);
                    $('#<%=hdnIDFieldName2.ClientID %>').val(result.IDFieldName);
                    $('#<%=hdnCodeFieldName2.ClientID %>').val(result.CodeFieldName);
                    $('#<%=hdnDisplayFieldName2.ClientID %>').val(result.DisplayFieldName);
                    $('#<%=hdnMethodName2.ClientID %>').val(result.MethodName);
                    onSubLedgerID2Changed();
                }
                else {
                    $('#<%=hdnGLAccountID2.ClientID %>').val('');
                    $('#<%=txtGLAccountCode2.ClientID %>').val('');
                    $('#<%=txtGLAccountName2.ClientID %>').val('');

                    $('#<%=hdnSubLedgerID2.ClientID %>').val('');
                    $('#<%=hdnSearchDialogTypeName2.ClientID %>').val('');
                    $('#<%=hdnFilterExpression2.ClientID %>').val('');
                    $('#<%=hdnIDFieldName2.ClientID %>').val('');
                    $('#<%=hdnCodeFieldName2.ClientID %>').val('');
                    $('#<%=hdnDisplayFieldName2.ClientID %>').val('');
                    $('#<%=hdnMethodName2.ClientID %>').val('');
                }

                $('#<%=hdnSubLedgerDtID2.ClientID %>').val('');
                $('#<%=txtSubLedgerDtCode2.ClientID %>').val('');
                $('#<%=txtSubLedgerDtName2.ClientID %>').val('');
            });
        }

        function onSubLedgerID2Changed() {
            if ($('#<%=hdnSubLedgerID2.ClientID %>').val() == '0' || $('#<%=hdnSubLedgerID2.ClientID %>').val() == '') {
                $('#<%=lblSubLedgerDt2.ClientID %>').attr('class', 'lblDisabled');
                $('#<%=txtSubLedgerDtCode2.ClientID %>').attr('readonly', 'readonly');
            }
            else {
                $('#<%=lblSubLedgerDt2.ClientID %>').attr('class', 'lblLink');
                $('#<%=txtSubLedgerDtCode2.ClientID %>').removeAttr('readonly');
            }
        }
        //#endregion

        //#region Sub Ledger 2
        function onGetSubLedgerDt2FilterExpression() {
            var filterExpression = $('#<%=hdnFilterExpression2.ClientID %>').val().replace('@SubLedgerID', $('#<%=hdnSubLedgerID2.ClientID %>').val());
            return filterExpression;
        }

        $('#<%=lblSubLedgerDt2.ClientID %>').live('click', function () {
            if ($('#<%=hdnSearchDialogTypeName2.ClientID %>').val() != '') {
                openSearchDialog($('#<%=hdnSearchDialogTypeName2.ClientID %>').val(), onGetSubLedgerDt2FilterExpression(), function (value) {
                    $('#<%=txtSubLedgerDtCode2.ClientID %>').val(value);
                    ontxtSubLedgerDtCode2Changed(value);
                });
            }
        });

        $('#<%=txtSubLedgerDtCode2.ClientID %>').live('change', function () {
            ontxtSubLedgerDtCode2Changed($(this).val());
        });

        function ontxtSubLedgerDtCode2Changed(value) {
            if ($('#<%=hdnSearchDialogTypeName2.ClientID %>').val() != '') {
                var filterExpression = onGetSubLedgerDt2FilterExpression() + " AND " + $('#<%=hdnCodeFieldName2.ClientID %>').val() + " = '" + value + "'";
                Methods.getObject($('#<%=hdnMethodName2.ClientID %>').val(), filterExpression, function (result) {
                    if (result != null) {
                        $('#<%=hdnSubLedgerDtID2.ClientID %>').val(result[$('#<%=hdnIDFieldName2.ClientID %>').val()]);
                        $('#<%=txtSubLedgerDtName2.ClientID %>').val(result[$('#<%=hdnDisplayFieldName2.ClientID %>').val()]);
                    }
                    else {
                        $('#<%=hdnSubLedgerDtID2.ClientID %>').val('');
                        $('#<%=txtSubLedgerDtCode2.ClientID %>').val('');
                        $('#<%=txtSubLedgerDtName2.ClientID %>').val('');
                    }
                });
            }
        }
        //#endregion

        //#region GL Account 3
        $('#lblGLAccount3.lblLink').live('click', function () {
            openSearchDialog('chartofaccount', onGetGLAccountFilterExpression(), function (value) {
                $('#<%=txtGLAccountCode3.ClientID %>').val(value);
                ontxtGLAccountCode3Changed(value);
            });
        });

        $('#<%=txtGLAccountCode3.ClientID %>').live('change', function () {
            ontxtGLAccountCode3Changed($(this).val());
        });

        function ontxtGLAccountCode3Changed(value) {
            var filterExpression = onGetGLAccountFilterExpression() + " AND GLAccountNo = '" + value + "'";
            Methods.getObject('GetvChartOfAccountList', filterExpression, function (result) {
                if (result != null) {
                    $('#<%=hdnGLAccountID3.ClientID %>').val(result.GLAccountID);
                    $('#<%=txtGLAccountName3.ClientID %>').val(result.GLAccountName);

                    $('#<%=hdnSubLedgerID3.ClientID %>').val(result.SubLedgerID);
                    $('#<%=hdnSearchDialogTypeName3.ClientID %>').val(result.SearchDialogTypeName);
                    $('#<%=hdnFilterExpression3.ClientID %>').val(result.FilterExpression);
                    $('#<%=hdnIDFieldName3.ClientID %>').val(result.IDFieldName);
                    $('#<%=hdnCodeFieldName3.ClientID %>').val(result.CodeFieldName);
                    $('#<%=hdnDisplayFieldName3.ClientID %>').val(result.DisplayFieldName);
                    $('#<%=hdnMethodName3.ClientID %>').val(result.MethodName);
                    onSubLedgerID3Changed();
                }
                else {
                    $('#<%=hdnGLAccountID3.ClientID %>').val('');
                    $('#<%=txtGLAccountCode3.ClientID %>').val('');
                    $('#<%=txtGLAccountName3.ClientID %>').val('');

                    $('#<%=hdnSubLedgerID3.ClientID %>').val('');
                    $('#<%=hdnSearchDialogTypeName3.ClientID %>').val('');
                    $('#<%=hdnFilterExpression3.ClientID %>').val('');
                    $('#<%=hdnIDFieldName3.ClientID %>').val('');
                    $('#<%=hdnCodeFieldName3.ClientID %>').val('');
                    $('#<%=hdnDisplayFieldName3.ClientID %>').val('');
                    $('#<%=hdnMethodName3.ClientID %>').val('');
                }

                $('#<%=hdnSubLedgerDtID3.ClientID %>').val('');
                $('#<%=txtSubLedgerDtCode3.ClientID %>').val('');
                $('#<%=txtSubLedgerDtName3.ClientID %>').val('');
            });
        }

        function onSubLedgerID3Changed() {
            if ($('#<%=hdnSubLedgerID3.ClientID %>').val() == '0' || $('#<%=hdnSubLedgerID3.ClientID %>').val() == '') {
                $('#<%=lblSubLedgerDt3.ClientID %>').attr('class', 'lblDisabled');
                $('#<%=txtSubLedgerDtCode3.ClientID %>').attr('readonly', 'readonly');
            }
            else {
                $('#<%=lblSubLedgerDt3.ClientID %>').attr('class', 'lblLink');
                $('#<%=txtSubLedgerDtCode3.ClientID %>').removeAttr('readonly');
            }
        }
        //#endregion

        //#region Sub Ledger 3
        function onGetSubLedgerDt3FilterExpression() {
            var filterExpression = $('#<%=hdnFilterExpression3.ClientID %>').val().replace('@SubLedgerID', $('#<%=hdnSubLedgerID3.ClientID %>').val());
            return filterExpression;
        }

        $('#<%=lblSubLedgerDt3.ClientID %>').live('click', function () {
            if ($('#<%=hdnSearchDialogTypeName3.ClientID %>').val() != '') {
                openSearchDialog($('#<%=hdnSearchDialogTypeName3.ClientID %>').val(), onGetSubLedgerDt3FilterExpression(), function (value) {
                    $('#<%=txtSubLedgerDtCode3.ClientID %>').val(value);
                    ontxtSubLedgerDtCode3Changed(value);
                });
            }
        });

        $('#<%=txtSubLedgerDtCode3.ClientID %>').live('change', function () {
            ontxtSubLedgerDtCode3Changed($(this).val());
        });

        function ontxtSubLedgerDtCode3Changed(value) {
            if ($('#<%=hdnSearchDialogTypeName3.ClientID %>').val() != '') {
                var filterExpression = onGetSubLedgerDt3FilterExpression() + " AND " + $('#<%=hdnCodeFieldName3.ClientID %>').val() + " = '" + value + "'";
                Methods.getObject($('#<%=hdnMethodName3.ClientID %>').val(), filterExpression, function (result) {
                    if (result != null) {
                        $('#<%=hdnSubLedgerDtID3.ClientID %>').val(result[$('#<%=hdnIDFieldName3.ClientID %>').val()]);
                        $('#<%=txtSubLedgerDtName3.ClientID %>').val(result[$('#<%=hdnDisplayFieldName3.ClientID %>').val()]);
                    }
                    else {
                        $('#<%=hdnSubLedgerDtID3.ClientID %>').val('');
                        $('#<%=txtSubLedgerDtCode3.ClientID %>').val('');
                        $('#<%=txtSubLedgerDtName3.ClientID %>').val('');
                    }
                });
            }
        }
        //#endregion

        //#region GL Account Discount
        $('#lblGLAccountDiscount.lblLink').live('click', function () {
            openSearchDialog('chartofaccount', onGetGLAccountFilterExpression(), function (value) {
                $('#<%=txtGLAccountDiscountCode.ClientID %>').val(value);
                ontxtGLAccountDiscountCodeChanged(value);
            });
        });

        $('#<%=txtGLAccountDiscountCode.ClientID %>').live('change', function () {
            ontxtGLAccountDiscountCodeChanged($(this).val());
        });

        function ontxtGLAccountDiscountCodeChanged(value) {
            var filterExpression = onGetGLAccountFilterExpression() + " AND GLAccountNo = '" + value + "'";
            Methods.getObject('GetvChartOfAccountList', filterExpression, function (result) {
                if (result != null) {
                    $('#<%=hdnGLAccountDiscountID.ClientID %>').val(result.GLAccountID);
                    $('#<%=txtGLAccountDiscountName.ClientID %>').val(result.GLAccountName);
                }
                else {
                    $('#<%=hdnGLAccountID1.ClientID %>').val('');
                    $('#<%=txtGLAccountCode1.ClientID %>').val('');
                    $('#<%=txtGLAccountName1.ClientID %>').val('');
                }
            });
        }
        //#endregion

        function onGetGLAccountFilterExpression() {
            var filterExpression = "IsHeader = 0 AND IsDeleted = 0";
            return filterExpression;
        }

        //#endregion

        //#region Product Line
        $('#<%=lblProductLineCtl.ClientID %>').live('click', function () {
            openSearchDialog('productline', 'IsDeleted = 0', function (value) {
                $('#<%=txtProductLineCodeCtl.ClientID %>').val(value);
                ontxtProductLineCodeCtlChanged(value);
            });
        });

        $('#<%=txtProductLineCodeCtl.ClientID %>').live('change', function () {
            ontxtProductLineCodeCtlChanged($(this).val());
        });

        function ontxtProductLineCodeCtlChanged(value) {
            var filterExpression = "ProductLineCode = '" + value + "'";
            Methods.getObject('GetProductLineList', filterExpression, function (result) {
                if (result != null) {
                    $('#<%=hdnProductLineIDCtl.ClientID %>').val(result.ProductLineID);
                    $('#<%=txtProductLineNameCtl.ClientID %>').val(result.ProductLineName);
                }
                else {
                    $('#<%=hdnProductLineIDCtl.ClientID %>').val('');
                    $('#<%=txtProductLineCodeCtl.ClientID %>').val('');
                    $('#<%=txtProductLineNameCtl.ClientID %>').val('');
                }
            });
        }
        //#endregion

    </script>
    <input type="hidden" value="" id="hdnID" runat="server" />
    <input type="hidden" value="" id="hdnGCItemType" runat="server" />
    <input type="hidden" value="" id="hdnGCSubItemType" runat="server" />
    <input type="hidden" value="" id="hdnSubMenuType" runat="server" />
    <input type="hidden" value="" id="hdnQueryItem" runat="server" />
    <input type="hidden" value="" id="hdnFilterExpression" runat="server" />
    <input type="hidden" value="" id="hdnExpandID" runat="server" />
    <input type="hidden" value="" id="hdnExpandIDDt" runat="server" />
    <input type="hidden" value="" id="hdnParentCito" runat="server" />
    <input type="hidden" value="" id="hdnParentCitoValue" runat="server" />
    <input type="hidden" value="" id="hdnParentComplication" runat="server" />
    <input type="hidden" value="" id="hdnParentComplicationValue" runat="server" />
    <input type="hidden" value="" id="hdnPrintDoctorName" runat="server" />
    <div style="position: relative;">
        <table style="width: 100%">
            <colgroup>
                <col style="width: 40%" />
                <col style="width: 60%" />
            </colgroup>
            <tr>
                <td style="vertical-align: top">
                    <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
                        ShowLoadingPanel="false" OnCallback="cbpView_Callback">
                        <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ onCbpViewEndCallback(s); }" />
                        <PanelCollection>
                            <dx:PanelContent ID="PanelContent1" runat="server">
                                <asp:Panel runat="server" ID="pnlView" CssClass="pnlContainerGridProcessList">
                                    <asp:GridView ID="grdViewIGHD" runat="server" CssClass="grdSelected grdPatientPage"
                                        AutoGenerateColumns="false" ShowHeaderWhenEmpty="false" EmptyDataRowStyle-CssClass="trEmpty">
                                        <Columns>
                                            <asp:BoundField DataField="ItemGroupID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                            <asp:TemplateField ItemStyle-Width="20px" ItemStyle-CssClass="tdExpand" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <img class="imgExpand imgLink" src='<%= ResolveUrl("~/Libs/Images/right-arrow.png")%>'
                                                        alt='' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-Width="100px" HeaderStyle-CssClass="gridColumnText"
                                                ItemStyle-CssClass="gridColumnText">
                                                <HeaderTemplate>
                                                    <div style="padding-left: 3px">
                                                        <%=GetLabel("Kode Kelompok")%>
                                                    </div>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <div style='margin-left: <%#: Eval("Level") %>0px;'>
                                                        <%#: Eval("ItemGroupCode") %></div>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="ItemGroupName1" HeaderText="Nama Kelompok Item" HeaderStyle-CssClass="gridColumnText"
                                                ItemStyle-CssClass="gridColumnText" />
                                        </Columns>
                                        <EmptyDataTemplate>
                                            <%=GetLabel("Data tidak tersedia")%>
                                        </EmptyDataTemplate>
                                    </asp:GridView>
                                </asp:Panel>
                            </dx:PanelContent>
                        </PanelCollection>
                    </dxcp:ASPxCallbackPanel>
                    <div class="imgLoadingGrdView" id="containerImgLoadingView">
                        <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                    </div>
                </td>
                <td style="vertical-align: top;">
                    <div id="containerPopupEntryData" style="margin-top: 10px; display: none;">
                        <input type="hidden" id="hdnItemGroupID" runat="server" value="" />
                        <div class="pageTitle">
                            <%=GetLabel("Entry")%></div>
                        <fieldset id="fsEntryPopup" style="margin: 0">
                            <table class="tblEntryDetail" style="width: 100%">
                                <colgroup>
                                    <col style="width: 60%" />
                                    <col style="width: 40%" />
                                </colgroup>
                                <tr>
                                    <td style="padding: 5px; vertical-align: top">
                                        <table style="width: 100%">
                                            <colgroup>
                                                <col style="width: 180px" />
                                            </colgroup>
                                            <tr>
                                                <td class="tdLabel">
                                                    <label class="lblMandatory">
                                                        <%=GetLabel("Kode Kelompok")%></label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtItemGroupCode" Width="120px" CssClass="required" runat="server" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="tdLabel">
                                                    <label class="lblMandatory">
                                                        <%=GetLabel("Nama Kelompok #1")%></label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtItemGroupName" CssClass="required" Width="250px" runat="server" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="tdLabel">
                                                    <label class="lblNormal">
                                                        <%=GetLabel("Nama Kelompok #2")%></label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtItemGroupName2" CssClass="required" Width="250px" runat="server" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="tdLabel">
                                                    <label class="lblLink" id="lblBillingGroup">
                                                        <%=GetLabel("Kelompok Rincian Transaksi")%></label>
                                                </td>
                                                <td>
                                                    <input type="hidden" id="hdnBillingGroupID" value="" runat="server" />
                                                    <table style="width: 100%" cellpadding="0" cellspacing="0">
                                                        <colgroup>
                                                            <col style="width: 100px" />
                                                            <col style="width: 3px" />
                                                            <col />
                                                        </colgroup>
                                                        <tr>
                                                            <td>
                                                                <asp:TextBox ID="txtBillingGroupCode" Width="100%" runat="server" />
                                                            </td>
                                                            <td>
                                                                &nbsp;
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtBillingGroupName" Width="100%" runat="server" ReadOnly="true" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="tdLabel">
                                                    <label class="lblLink" id="lblRevenueSharing">
                                                        <%=GetLabel("Formula Honor Dokter")%></label>
                                                </td>
                                                <td>
                                                    <input type="hidden" id="hdnRevenueSharingID" value="" runat="server" />
                                                    <table style="width: 100%" cellpadding="0" cellspacing="0">
                                                        <colgroup>
                                                            <col style="width: 100px" />
                                                            <col style="width: 3px" />
                                                            <col />
                                                        </colgroup>
                                                        <tr>
                                                            <td>
                                                                <asp:TextBox ID="txtRevenueSharingCode" Width="100%" runat="server" />
                                                            </td>
                                                            <td>
                                                                &nbsp;
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtRevenueSharingName" Width="100%" runat="server" ReadOnly="true" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="tdLabel">
                                                    <label class="lblMandatory">
                                                        <%=GetLabel("Urutan Cetak")%></label>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtPrintOrder" CssClass="number required" Width="100px" runat="server" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="tdLabel">
                                                    <%=GetLabel("Keterangan") %>
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtNotes" Width="100%" runat="server" TextMode="MultiLine" Rows="2" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2">
                                                    <hr />
                                                </td>
                                            </tr>
                                            <tr id="trProductLine" runat="server">
                                                <td class="tdLabel">
                                                    <label class="lblLink lblMandatory" id="lblProductLineCtl" runat="server">
                                                        <%=GetLabel("Product Line")%></label>
                                                </td>
                                                <td>
                                                    <input type="hidden" id="hdnProductLineIDCtl" value="" runat="server" />
                                                    <table style="width: 100%" cellpadding="0" cellspacing="0">
                                                        <colgroup>
                                                            <col style="width: 30%" />
                                                            <col style="width: 3px" />
                                                            <col />
                                                        </colgroup>
                                                        <tr>
                                                            <td>
                                                                <asp:TextBox ID="txtProductLineCodeCtl" Width="100%" runat="server" />
                                                            </td>
                                                            <td>
                                                                &nbsp;
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtProductLineNameCtl" Width="100%" ReadOnly="true" runat="server" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="tdLabel">
                                                    <label class="lblLink" id="lblGLAccount1">
                                                        <%=GetLabel("COA Komponen Tarif 1")%></label>
                                                </td>
                                                <td>
                                                    <input type="hidden" id="hdnGLAccountID1" runat="server" />
                                                    <input type="hidden" id="hdnSubLedgerID1" runat="server" />
                                                    <input type="hidden" id="hdnSearchDialogTypeName1" runat="server" />
                                                    <input type="hidden" id="hdnIDFieldName1" runat="server" />
                                                    <input type="hidden" id="hdnCodeFieldName1" runat="server" />
                                                    <input type="hidden" id="hdnDisplayFieldName1" runat="server" />
                                                    <input type="hidden" id="hdnMethodName1" runat="server" />
                                                    <input type="hidden" id="hdnFilterExpression1" runat="server" />
                                                    <table style="width: 100%" cellpadding="0" cellspacing="0">
                                                        <colgroup>
                                                            <col style="width: 30%" />
                                                            <col style="width: 3px" />
                                                            <col />
                                                        </colgroup>
                                                        <tr>
                                                            <td>
                                                                <asp:TextBox runat="server" ID="txtGLAccountCode1" Width="100%" />
                                                            </td>
                                                            <td>
                                                                &nbsp;
                                                            </td>
                                                            <td>
                                                                <asp:TextBox runat="server" ID="txtGLAccountName1" Width="100%" ReadOnly="true" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="tdLabel">
                                                    <label class="lblDisabled" runat="server" id="lblSubLedgerDt1">
                                                        <%=GetLabel("Sub Perkiraan 1")%></label>
                                                </td>
                                                <td>
                                                    <input type="hidden" id="hdnSubLedgerDtID1" runat="server" />
                                                    <table style="width: 100%" cellpadding="0" cellspacing="0">
                                                        <colgroup>
                                                            <col style="width: 30%" />
                                                            <col style="width: 3px" />
                                                            <col />
                                                        </colgroup>
                                                        <tr>
                                                            <td>
                                                                <asp:TextBox runat="server" ID="txtSubLedgerDtCode1" Width="100%" />
                                                            </td>
                                                            <td>
                                                                &nbsp;
                                                            </td>
                                                            <td>
                                                                <asp:TextBox runat="server" ID="txtSubLedgerDtName1" Width="100%" ReadOnly="true" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="tdLabel">
                                                    <label class="lblLink" id="lblGLAccount2">
                                                        <%=GetLabel("COA Komponen Tarif 2")%></label>
                                                </td>
                                                <td>
                                                    <input type="hidden" id="hdnGLAccountID2" runat="server" />
                                                    <input type="hidden" id="hdnSubLedgerID2" runat="server" />
                                                    <input type="hidden" id="hdnSearchDialogTypeName2" runat="server" />
                                                    <input type="hidden" id="hdnIDFieldName2" runat="server" />
                                                    <input type="hidden" id="hdnCodeFieldName2" runat="server" />
                                                    <input type="hidden" id="hdnDisplayFieldName2" runat="server" />
                                                    <input type="hidden" id="hdnMethodName2" runat="server" />
                                                    <input type="hidden" id="hdnFilterExpression2" runat="server" />
                                                    <table style="width: 100%" cellpadding="0" cellspacing="0">
                                                        <colgroup>
                                                            <col style="width: 30%" />
                                                            <col style="width: 3px" />
                                                            <col />
                                                        </colgroup>
                                                        <tr>
                                                            <td>
                                                                <asp:TextBox runat="server" ID="txtGLAccountCode2" Width="100%" />
                                                            </td>
                                                            <td>
                                                                &nbsp;
                                                            </td>
                                                            <td>
                                                                <asp:TextBox runat="server" ID="txtGLAccountName2" Width="100%" ReadOnly="true" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="tdLabel">
                                                    <label class="lblDisabled" runat="server" id="lblSubLedgerDt2">
                                                        <%=GetLabel("Sub Perkiraan 2")%></label>
                                                </td>
                                                <td>
                                                    <input type="hidden" id="hdnSubLedgerDtID2" runat="server" />
                                                    <table style="width: 100%" cellpadding="0" cellspacing="0">
                                                        <colgroup>
                                                            <col style="width: 30%" />
                                                            <col style="width: 3px" />
                                                            <col />
                                                        </colgroup>
                                                        <tr>
                                                            <td>
                                                                <asp:TextBox runat="server" ID="txtSubLedgerDtCode2" Width="100%" />
                                                            </td>
                                                            <td>
                                                                &nbsp;
                                                            </td>
                                                            <td>
                                                                <asp:TextBox runat="server" ID="txtSubLedgerDtName2" Width="100%" ReadOnly="true" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="tdLabel">
                                                    <label class="lblLink" id="lblGLAccount3">
                                                        <%=GetLabel("COA Komponen Tarif 3")%></label>
                                                </td>
                                                <td>
                                                    <input type="hidden" id="hdnGLAccountID3" runat="server" />
                                                    <input type="hidden" id="hdnSubLedgerID3" runat="server" />
                                                    <input type="hidden" id="hdnSearchDialogTypeName3" runat="server" />
                                                    <input type="hidden" id="hdnIDFieldName3" runat="server" />
                                                    <input type="hidden" id="hdnCodeFieldName3" runat="server" />
                                                    <input type="hidden" id="hdnDisplayFieldName3" runat="server" />
                                                    <input type="hidden" id="hdnMethodName3" runat="server" />
                                                    <input type="hidden" id="hdnFilterExpression3" runat="server" />
                                                    <table style="width: 100%" cellpadding="0" cellspacing="0">
                                                        <colgroup>
                                                            <col style="width: 30%" />
                                                            <col style="width: 3px" />
                                                            <col />
                                                        </colgroup>
                                                        <tr>
                                                            <td>
                                                                <asp:TextBox runat="server" ID="txtGLAccountCode3" Width="100%" />
                                                            </td>
                                                            <td>
                                                                &nbsp;
                                                            </td>
                                                            <td>
                                                                <asp:TextBox runat="server" ID="txtGLAccountName3" Width="100%" ReadOnly="true" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="tdLabel">
                                                    <label class="lblDisabled" runat="server" id="lblSubLedgerDt3">
                                                        <%=GetLabel("Sub Perkiraan 3")%></label>
                                                </td>
                                                <td>
                                                    <input type="hidden" id="hdnSubLedgerDtID3" runat="server" />
                                                    <table style="width: 100%" cellpadding="0" cellspacing="0">
                                                        <colgroup>
                                                            <col style="width: 30%" />
                                                            <col style="width: 3px" />
                                                            <col />
                                                        </colgroup>
                                                        <tr>
                                                            <td>
                                                                <asp:TextBox runat="server" ID="txtSubLedgerDtCode3" Width="100%" />
                                                            </td>
                                                            <td>
                                                                &nbsp;
                                                            </td>
                                                            <td>
                                                                <asp:TextBox runat="server" ID="txtSubLedgerDtName3" Width="100%" ReadOnly="true" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="tdLabel">
                                                    <label class="lblLink" id="lblGLAccountDiscount">
                                                        <%=GetLabel("COA Diskon")%></label>
                                                </td>
                                                <td>
                                                    <input type="hidden" id="hdnGLAccountDiscountID" runat="server" />
                                                    <table style="width: 100%" cellpadding="0" cellspacing="0">
                                                        <colgroup>
                                                            <col style="width: 30%" />
                                                            <col style="width: 3px" />
                                                            <col />
                                                        </colgroup>
                                                        <tr>
                                                            <td>
                                                                <asp:TextBox runat="server" ID="txtGLAccountDiscountCode" Width="100%" />
                                                            </td>
                                                            <td>
                                                                &nbsp;
                                                            </td>
                                                            <td>
                                                                <asp:TextBox runat="server" ID="txtGLAccountDiscountName" Width="100%" ReadOnly="true" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2">
                                                    <table>
                                                        <tr>
                                                            <td>
                                                                <input type="button" id="btnEntryPopupSave" value='<%= GetLabel("Simpan")%>' />
                                                            </td>
                                                            <td>
                                                                <input type="button" id="btnEntryPopupCancel" value='<%= GetLabel("Batal")%>' />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td style="visibility: hidden; vertical-align: top" id="tdItemService" runat="server">
                                        <table style="width: 100%">
                                            <tr>
                                                <td class="tdLabel">
                                                    <label class="lblMandatory">
                                                        <%=GetLabel("Nilai CITO")%></label>
                                                </td>
                                                <td>
                                                    <asp:CheckBox ID="chkIsCITOInPercentageCtl" runat="server" />
                                                    %
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtCITOAmountCtl" Style="text-align: right" Width="100px" runat="server" />
                                                </td>
                                            </tr>
                                            <tr style="display: none">
                                                <td class="tdLabel">
                                                    <label class="lblMandatory">
                                                        <%=GetLabel("Nilai Penyulit")%></label>
                                                </td>
                                                <td>
                                                    <asp:CheckBox ID="chkIsComplicationInPercentageCtl" runat="server" />
                                                    %
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtComplicationAmountCtl" Style="text-align: right" Width="100px"
                                                        runat="server" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="tdLabel">
                                                    <label class="lblNormal">
                                                        <%=GetLabel("Print Nama Dokter")%></label>
                                                </td>
                                                <td>
                                                    <asp:CheckBox ID="chkIsPrintWithDoctorNameCtl" runat="server" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </fieldset>
                    </div>
                    <div style="width: 100%; text-align: center" id="divContainerAddData" runat="server">
                        <span class="lblLink" id="lblEntryPopupAddData">
                            <%= GetLabel("Tambah Data")%></span>
                    </div>
                    <h4>
                        <%=GetLabel("Sub Kelompok Item")%></h4>
                    <dxcp:ASPxCallbackPanel ID="cbpViewDt" runat="server" Width="100%" ClientInstanceName="cbpViewDt"
                        OnCallback="cbpViewDt_Callback" ShowLoadingPanel="false">
                        <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingViewDt').show(); }"
                            EndCallback="function(s,e){ onCbpViewDtEndCallback(s); }" />
                        <PanelCollection>
                            <dx:PanelContent ID="PanelContent3" runat="server">
                                <asp:Panel runat="server" ID="Panel2" CssClass="pnlContainerGridProcessList">
                                    <asp:GridView ID="grdViewIGDT" runat="server" CssClass="grdSelected grdPatientPage"
                                        AutoGenerateColumns="false" ShowHeaderWhenEmpty="false" EmptyDataRowStyle-CssClass="trEmpty">
                                        <Columns>
                                            <asp:BoundField DataField="ItemGroupID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                            <asp:TemplateField HeaderStyle-Width="70px" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <img class="imgEdit imgLink" src='<%# ResolveUrl("~/Libs/Images/Button/edit.png")%>'
                                                        alt="" style="float: left; margin-left: 7px" />
                                                    <img class="imgDelete imgLink" src='<%# ResolveUrl("~/Libs/Images/Button/delete.png")%>'
                                                        alt="" />
                                                    <input type="hidden" class="hdnItemGroupID" value="<%#: Eval("ItemGroupID")%>" />
                                                    <input type="hidden" class="hdnItemGroupCode" value="<%#: Eval("ItemGroupCode")%>" />
                                                    <input type="hidden" class="hdnItemGroupName" value="<%#: Eval("ItemGroupName1")%>" />
                                                    <input type="hidden" class="hdnItemGroupName2" value="<%#: Eval("ItemGroupName2")%>" />
                                                    <input type="hidden" class="hdnPrintOrder" value="<%#: Eval("PrintOrder")%>" />
                                                    <input type="hidden" class="hdnRemarks" value="<%#: Eval("Remarks")%>" />
                                                    <input type="hidden" class="BillingGroupID" value="<%#: Eval("BillingGroupID")%>" />
                                                    <input type="hidden" class="BillingGroupCode" value="<%#: Eval("BillingGroupCode")%>" />
                                                    <input type="hidden" class="BillingGroupName1" value="<%#: Eval("BillingGroupName1")%>" />
                                                    <input type="hidden" class="hdnRevenueSharingID" value="<%#: Eval("RevenueSharingID")%>" />
                                                    <input type="hidden" class="hdnRevenueSharingCode" value="<%#: Eval("RevenueSharingCode")%>" />
                                                    <input type="hidden" class="hdnRevenueSharingName" value="<%#: Eval("RevenueSharingName")%>" />
                                                    <input type="hidden" class="txtCitoAmount" value="<%#: Eval("CitoAmount")%>" />
                                                    <input type="hidden" class="hdnIsCitoInPercentage" value="<%#: Eval("IsCitoInPercentage")%>" />
                                                    <input type="hidden" class="txtComplicationAmount" value="<%#: Eval("ComplicationAmount")%>" />
                                                    <input type="hidden" class="hdnIsComplicationInPercentage" value="<%#: Eval("IsComplicationInPercentage")%>" />
                                                    <input type="hidden" class="hdnIsPrintWithDoctorName" value="<%#: Eval("IsPrintWithDoctorName")%>" />
                                                    <input type="hidden" class="ProductLineID" value="<%#: Eval("ProductLineID")%>" />
                                                    <input type="hidden" class="ProductLineCode" value="<%#: Eval("ProductLineCode")%>" />
                                                    <input type="hidden" class="ProductLineName" value="<%#: Eval("ProductLineName")%>" />
                                                    <input type="hidden" class="GLAccountID1" value="<%#: Eval("GLAccount1")%>" />
                                                    <input type="hidden" class="GLAccountCode1" value="<%#: Eval("GLAccountNo1")%>" />
                                                    <input type="hidden" class="GLAccountName1" value="<%#: Eval("GLAccountName1")%>" />
                                                    <input type="hidden" class="SubLedgerDtID1" value="<%#: Eval("SubLedger1")%>" />
                                                    <input type="hidden" class="SubLedgerDtCode1" value="<%#: Eval("SubLedgerCode1")%>" />
                                                    <input type="hidden" class="SubLedgerDtName1" value="<%#: Eval("SubLedgerName1")%>" />
                                                    <input type="hidden" class="GLAccountID2" value="<%#: Eval("GLAccount2")%>" />
                                                    <input type="hidden" class="GLAccountCode2" value="<%#: Eval("GLAccountNo2")%>" />
                                                    <input type="hidden" class="GLAccountName2" value="<%#: Eval("GLAccountName2")%>" />
                                                    <input type="hidden" class="SubLedgerDtID2" value="<%#: Eval("SubLedger2")%>" />
                                                    <input type="hidden" class="SubLedgerDtCode2" value="<%#: Eval("SubLedgerCode2")%>" />
                                                    <input type="hidden" class="SubLedgerDtName2" value="<%#: Eval("SubLedgerName2")%>" />
                                                    <input type="hidden" class="GLAccountID3" value="<%#: Eval("GLAccount3")%>" />
                                                    <input type="hidden" class="GLAccountCode3" value="<%#: Eval("GLAccountNo3")%>" />
                                                    <input type="hidden" class="GLAccountName3" value="<%#: Eval("GLAccountName3")%>" />
                                                    <input type="hidden" class="SubLedgerDtID3" value="<%#: Eval("SubLedger3")%>" />
                                                    <input type="hidden" class="SubLedgerDtCode3" value="<%#: Eval("SubLedgerCode3")%>" />
                                                    <input type="hidden" class="SubLedgerDtName3" value="<%#: Eval("SubLedgerName3")%>" />
                                                    <input type="hidden" class="GLAccountDiscount" value="<%#: Eval("GLAccountDiscount")%>" />
                                                    <input type="hidden" class="GLAccountDiscountNo" value="<%#: Eval("GLAccountDiscountNo")%>" />
                                                    <input type="hidden" class="GLAccountDiscountName" value="<%#: Eval("GLAccountDiscountName")%>" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField ItemStyle-Width="20px" ItemStyle-CssClass="tdExpandDt" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <img class="imgExpandDt imgLink" src='<%= ResolveUrl("~/Libs/Images/right-arrow.png")%>'
                                                        alt='' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-Width="100px" HeaderStyle-CssClass="gridColumnText"
                                                ItemStyle-CssClass="gridColumnText">
                                                <HeaderTemplate>
                                                    <div style="padding-left: 3px">
                                                        <%=GetLabel("Kode Kelompok")%>
                                                    </div>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <div style='margin-left: <%#: Eval("Level") %>0px;'>
                                                        <%#: Eval("ItemGroupCode") %></div>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="ItemGroupName1" HeaderText="Nama Kelompok Item" HeaderStyle-CssClass="gridColumnText"
                                                ItemStyle-CssClass="gridColumnText" />
                                        </Columns>
                                        <EmptyDataTemplate>
                                            <%=GetLabel("Data tidak tersedia")%>
                                        </EmptyDataTemplate>
                                    </asp:GridView>
                                </asp:Panel>
                            </dx:PanelContent>
                        </PanelCollection>
                    </dxcp:ASPxCallbackPanel>
                    <div class="imgLoadingGrdView" id="containerImgLoadingViewDt">
                        <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                    </div>
                </td>
            </tr>
        </table>
    </div>
    <div id="containerGrdDetail" class="borderBox" style="width: 100%; padding: 10px 5px 10px 5px;
        display: none">
        <table style="width: 90%; border-style: dotted; border-width: thin; background-color: transparent">
            <thead>
                <tr>
                    <th style="text-align: center" colspan="5">
                        <%=GetLabel("Mapping Pendapatan dan Diskon")%>
                    </th>
                </tr>
            </thead>
            <tbody>
                <tr>
                    <td align="center">
                        <label class="lblLink lnkItemGroupCOA">
                            <%=GetLabel("ItemGroup COA")%>
                        </label>
                    </td>
                </tr>
                <tr>
                    <td align="center">
                        <label class="lblLink lnkItemGroupClassCOA">
                            <%=GetLabel("ItemGroup & Class COA")%>
                        </label>
                    </td>
                </tr>
                <tr>
                    <td align="center">
                        <label class="lblLink lnkItemGroupClassServiceUnitCOA">
                            <%=GetLabel("ItemGroup,Class,ServiceUnit COA")%>
                        </label>
                    </td>
                </tr>
                <tr>
                    <td align="center">
                        <label class="lblLink lnkItemGroupServiceUnitCOA">
                            <%=GetLabel("ItemGroup & ServiceUnit COA")%>
                        </label>
                    </td>
                </tr>
                <tr>
                    <td align="center">
                        <label class="lblLink lnkItemGroupServiceUnitCustomerLineCOA">
                            <%=GetLabel("ItemGroup,ServiceUnit,CustomerLine COA")%>
                        </label>
                    </td>
                </tr>
                <tr>
                    <td align="center">
                        <label class="lblLink lnkItemGroupServiceUnitbyClassbyParamedicCOA">
                            <%=GetLabel("ItemGroup,ServiceUnit,byClass,byParamedic COA")%>
                        </label>
                    </td>
                </tr>
                <tr>
                    <td align="center">
                        <label class="lblLink lnkItemGroupServiceUnitbySourceUnitCOA">
                            <%=GetLabel("ItemGroup,ServiceUnit,bySource COA")%>
                        </label>
                    </td>
                </tr>
                <tr>
                    <td align="center">
                        <label class="lblLink lnkItemGroupServiceUnitbySourceUnitbyClassCOA">
                            <%=GetLabel("ItemGroup,ServiceUnit,bySource,byClass COA")%>
                        </label>
                    </td>
                </tr>
                <tr>
                    <td align="center">
                        <label class="lblLink lnkItemGroupServiceUnitbySourceUnitbyClassbyParamedicCOA">
                            <%=GetLabel("ItemGroup,ServiceUnit,bySource,byClass,byParamedic COA")%>
                        </label>
                    </td>
                </tr>
            </tbody>
        </table>
    </div>
    <div id="containerGrdDetailDt" class="borderBox" style="width: 100%; padding: 10px 5px 10px 5px;
        display: none">
        <table style="width: 90%; border-style: dotted; border-width: thin; background-color: transparent">
            <thead>
                <tr>
                    <th style="text-align: center" colspan="5">
                        <%=GetLabel("Mapping Pendapatan dan Diskon")%>
                    </th>
                </tr>
            </thead>
            <tbody>
                <tr>
                    <td align="center">
                        <label class="lblLink lnkItemGroupCOADT">
                            <%=GetLabel("ItemGroup COA")%>
                        </label>
                    </td>
                    <td align="center">
                        <label class="lblLink lnkItemGroupClassCOADT">
                            <%=GetLabel("ItemGroup & Class COA")%>
                        </label>
                    </td>
                </tr>
                <tr>
                    <td align="center">
                        <label class="lblLink lnkItemGroupClassServiceUnitCOADT">
                            <%=GetLabel("ItemGroup,Class,ServiceUnit COA")%>
                        </label>
                    </td>
                    <td align="center">
                        <label class="lblLink lnkItemGroupServiceUnitCOADT">
                            <%=GetLabel("ItemGroup & ServiceUnit COA")%>
                        </label>
                    </td>
                </tr>
                <tr>
                    <td align="center">
                        <label class="lblLink lnkItemGroupServiceUnitCustomerLineCOADT">
                            <%=GetLabel("ItemGroup,ServiceUnit,CustomerLine COA")%>
                        </label>
                    </td>
                    <td align="center">
                        <label class="lblLink lnkItemGroupServiceUnitbyClassbyParamedicCOADT">
                            <%=GetLabel("ItemGroup,ServiceUnit,byClass,byParamedic COA")%>
                        </label>
                    </td>
                </tr>
                <tr>
                    <td align="center">
                        <label class="lblLink lnkItemGroupServiceUnitbySourceUnitCOADT">
                            <%=GetLabel("ItemGroup,ServiceUnit,bySource COA")%>
                        </label>
                    </td>
                    <td align="center">
                        <label class="lblLink lnkItemGroupServiceUnitbySourceUnitbyClassCOADT">
                            <%=GetLabel("ItemGroup,ServiceUnit,bySource,byClass COA")%>
                        </label>
                    </td>
                </tr>
                <tr>
                    <td align="center">
                        <label class="lblLink lnkItemGroupServiceUnitbySourceUnitbyClassbyParamedicCOADT">
                            <%=GetLabel("ItemGroup,ServiceUnit,bySource,byClass,byParamedic COA")%>
                        </label>
                    </td>
                </tr>
            </tbody>
        </table>
    </div>
</asp:Content>
