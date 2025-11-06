<%@ Page Language="C#" MasterPageFile="~/Libs/MasterPage/MPFrame.Master" AutoEventWireup="true"
    CodeBehind="LaboratoryResultList1.aspx.cs" Inherits="QIS.Medinfras.Web.CommonLibs.Program.LaboratoryResultList1" %>

<%@ Register Assembly="QIS.Medinfras.Web.CustomControl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"
    Namespace="QIS.Medinfras.Web.CustomControl" TagPrefix="qis" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="plhMPFrame" runat="server">
    <script type="text/javascript">
        $('#<%=grdView.ClientID %> tr:gt(0):not(.trEmpty)').live('click', function () {
            if ($(this).attr('class') != 'selected') {
                $('#<%=grdView.ClientID %> tr.selected').removeClass('selected');
                $(this).addClass('selected');
                $('#<%=hdnIDCBCtl.ClientID %>').val($(this).find('.keyField').html());
                $('#<%=txtRemarks.ClientID %>').val($(this).find('.remarks').html().replace(/&nbsp;/gi, " "));
                cbpViewDt.PerformCallback('refresh');
            }
        });

        $('#<%=grdViewTab2.ClientID %> tr:gt(0):not(.trEmpty)').live('click', function () {
            if ($(this).attr('class') != 'selected') {
                $('#<%=grdViewTab2.ClientID %> tr.selected').removeClass('selected');
                $(this).addClass('selected');
                $('#<%=hdnItemGroupCodeCBCtl.ClientID %>').val($(this).find('.keyField').html());
                cbpViewDtTab2.PerformCallback('refresh');
            }
        });

        $('#<%=grdViewDt.ClientID %> tr:gt(0):not(.trEmpty)').live('click', function () {
            if ($(this).attr('class') != 'selected') {
                $('#<%=grdViewDt.ClientID %> tr.selected').removeClass('selected');
                $(this).addClass('selected');
                $('#<%=hdnItemIDCBCtl.ClientID %>').val($(this).find('.keyField').html());
                $('#<%=hdnReferenceNoCBCtl.ClientID %>').val($(this).find('.accessionNo').html());
            }
        });

        $(function () {
            setDatePicker('<%=txtPeriodFrom.ClientID %>');
            setDatePicker('<%=txtPeriodTo.ClientID %>');

            $('#<%=grdView.ClientID %> tr:eq(1)').click();

            $('.lblViewPDF.lblLink').live('click', function (evt) {
                $tr = $(this).parent().closest('tr');
                var textResultValue = $tr.find('.textResultValue').html();
                window.open("data:application/pdf;base64, " + textResultValue, "popupWindow", "width=600, height=600,scrollbars=yes");
            });

            $('.lblViewAttachment.lblLink').live('click', function (evt) {
                var textResultValue = $(this).attr('attachmentValue');
                window.open("data:application/pdf;base64, " + textResultValue, "popupWindow", "width=600, height=600,scrollbars=yes");
            });

            $('#<%=rblItemType.ClientID %> input').change(function () {
                cbpView.PerformCallback('refresh');
                cbpViewDt.PerformCallback('refresh');
            });

            $('#<%=rblItemTypeTab2.ClientID %> input').change(function () {
                cbpViewTab2.PerformCallback('refresh');
            });

            $('#<%:txtPeriodFrom.ClientID %>').live('change', function () {
                cbpViewTab2.PerformCallback('refresh');
            });

            $('#<%:txtPeriodTo.ClientID %>').live('change', function () {
                cbpViewTab2.PerformCallback('refresh');
            });

            //#region Detail Tab
            $('#ulResultDetail li').click(function () {
                $('#ulResultDetail li.selected').removeAttr('class');
                $('.containerOrderDt').filter(':visible').hide();
                $contentID = $(this).attr('contentid');
                $('#' + $contentID).show();
                $(this).addClass('selected');
            });
            //#endregion

            $('.lblCommentDetail.lblLink').die('click');
            $('.lblCommentDetail.lblLink').live('click', function () {
                $row = $(this).parent().closest('tr');
                var entity = rowToObject($row);

                var itemID = entity.ItemID;
                var fractionID = entity.FractionID;
                var chargeTransactionID = entity.ChargeTransactionID;
                var chargesTransactionDetailID = 0;

                var filterExpressionCharges = "TransactionID = '" + chargeTransactionID + "' AND ItemID = '" + itemID + "'";
                Methods.getObject('GetPatientChargesDtList', filterExpressionCharges, function (result) {
                    if (result != null) {
                        chargesTransactionDetailID = result.ID;
                    }
                });

                var filterExpressionOrder = "ChargeTransactionID = '" + chargeTransactionID + "' AND ItemID = '" + itemID + "' AND FractionID = '" + fractionID + "'";
                Methods.getObject('GetvLaboratoryResultDtList', filterExpressionOrder, function (result1) {
                    if (result1 != null) {
                        showToast('Comment', result1.Remarks);
                    }
                    else {
                        var filterExpressionChargesInfo = "ID = '" + chargesTransactionDetailID + "'";
                        Methods.getObject('GetPatientChargesDtInfoList', filterExpressionChargesInfo, function (result2) {
                            if (result2 != null) {
                                showToast('Comment', result2.Remarks);
                            }
                        });
                    }
                });
            });

            $('.lblCommentDetailTab2.lblLink').die('click');
            $('.lblCommentDetailTab2.lblLink').live('click', function () {
                $row = $(this).closest('tr').parent().closest('tr');
                var entity = rowToObject($row);
                var itemID = entity.ItemID;
                var testOrderID = entity.TestOrderID;
                var chargeTransactionID = entity.ChargeTransactionID;
                var chargesTransactionDetailID = 0;

                var filterExpressionCharges = "TransactionID = '" + chargeTransactionID + "' AND ItemID = '" + itemID + "'";
                Methods.getObject('GetPatientChargesDtList', filterExpressionCharges, function (result) {
                    if (result != null) {
                        chargesTransactionDetailID = result.ID;
                    }
                });

                var filterExpressionOrder = "ChargeTransactionID = '" + chargeTransactionID + "' AND ItemID = '" + itemID + "'";
                Methods.getObject('GetvLaboratoryResultDtList', filterExpressionOrder, function (result1) {
                    if (result1 != null) {
                        showToast('Comment', result1.Remarks);
                    }
                    else {
                        var filterExpressionChargesInfo = "ID = '" + chargesTransactionDetailID + "'";
                        Methods.getObject('GetPatientChargesDtInfoList', filterExpressionChargesInfo, function (result2) {
                            if (result2 != null) {
                                showToast('Comment', result2.Remarks);
                            }
                        });
                    }
                });
            });
        });

        function onRefreshControl(filterExpression) {
            $('#<%=hdnFilterExpressionCBCtl.ClientID %>').val(filterExpression);
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

        //#region Paging 2
        function onCbpViewTab2EndCallback(s) {
            $('#containerImgLoadingView').hide();
            var param = s.cpResult.split('|');
            if (param[1] != '0') {
                $('#<%=grdViewTab2.ClientID %> tr:eq(1)').click();
            }
            else {
                $('#<%=hdnItemGroupCodeCBCtl.ClientID %>').val('0');
                cbpViewDtTab2.PerformCallback('refresh');
            }
        }
        //#endregion

        //#region Paging Dt
        function onCbpViewDtEndCallback(s) {
            $('#containerImgLoadingViewDt').hide();

            var param = s.cpResult.split('|');
            if (param[0] == 'refresh') {
                var pageCount = parseInt(param[1]);
                if (pageCount > 0)
                    $('#<%=grdViewDt.ClientID %> tr:eq(1)').click();

                setPaging($("#pagingDt"), pageCount, function (page) {
                    cbpViewDt.PerformCallback('changepage|' + page);
                });
            }
            else
                $('#<%=grdViewDt.ClientID %> tr:eq(1)').click();
        }
        //#endregion

        //#region Paging Dt 2
        function onCbpViewDtTab2EndCallback(s) {
            $('#containerImgLoadingViewDt').hide();
        }
        //#endregion        

        $('.lblReffRange.lblLink').die('click');
        $('.lblReffRange.lblLink').live('click', function () {
            $row = $(this).parent().closest('tr');
            var entity = rowToObject($row);
            showToast('Nilai Referensi', entity.cfReferenceRange);
        });

        $('.lblReffRangeTab2.lblLink').die('click');
        $('.lblReffRangeTab2.lblLink').live('click', function () {
            $row = $(this).parent().closest('tr');
            var entity = rowToObject($row);
            showToast('Nilai Referensi', entity.cfReferenceRange);
        });
    </script>
    <style>
        .fractionInfo
        {
            text-align: left;
            vertical-align: middle;
            font-size: 10pt;
            font-weight: bold;
            font-style: italic;
        }
        
        .resultData
        {
            text-align: right;
            vertical-align: middle;
        }
         .divHiddenResult
        {
            display: none;
        }
    </style>
    <input type="hidden" value="" id="hdnIDCBCtl" runat="server" />
    <input type="hidden" value="" id="hdnItemGroupCodeCBCtl" runat="server" />
    <input type="hidden" value="" id="hdnItemIDCBCtl" runat="server" />
    <input type="hidden" value="" id="hdnItemIDCBCtl2" runat="server" />
    <input type="hidden" value="" id="hdnReferenceNoCBCtl" runat="server" />
    <input type="hidden" value="" id="hdnReferenceNoCBCtl2" runat="server" />
    <input type="hidden" value="" id="hdnViewerUrlCBCtl" runat="server" />
    <input type="hidden" value="" id="hdnDocumentPathCBCtl" runat="server" />
    <input type="hidden" id="hdnMRNCBCtl" runat="server" value="" />
    <input type="hidden" id="hdnVisitIDCBCtl" runat="server" value="" />
    <input type="hidden" id="hdnRegistrationIDCBCtl" runat="server" value="" />
    <input type="hidden" id="hdnIsBridgingToLIS" runat="server" value="" />
    <input type="hidden" id="hdnRISWebViewURL" runat="server" value="" />
    <input type="hidden" id="hdnRISHL7MessageFormat" runat="server" value="" />
    <input type="hidden" id="hdnRISConsumerPassword" runat="server" value="" />
    <input type="hidden" id="hdnHealthcareServiceUnitIDCBCtl" runat="server" value="" />
    <input type="hidden" id="hdnFilterExpressionCBCtl" runat="server" value="" />
    <input type="hidden" id="hdnIsShowResultAfterProposed" runat="server" value="" />
    <table border="0" cellpadding="1" cellspacing="0" style="width: 100%">
        <tr>
            <td>
                <div class="containerUlTabPage" style="margin-bottom: 3px;">
                    <ul class="ulTabPage" id="ulResultDetail">
                        <li class="selected" contentid="panByTransactionNo">
                            <%=GetLabel("Berdasarkan No. Transaksi")%></li>
                        <li contentid="panFraction">
                            <%=GetLabel("Per Kelompok Pemeriksaan")%></li>
                    </ul>
                </div>
            </td>
        </tr>
        <tr>
            <td>
                <div class="containerOrderDt" id="panByTransactionNo">
                    <table style="width: 100%">
                        <colgroup>
                            <col style="width: 400px" />
                            <col />
                        </colgroup>
                        <tr>
                            <td valign="top">
                                <div style="position: relative;">
                                    <table border="0" cellpadding="0" cellspacing="1">
                                        <colgroup>
                                            <col style="width: 100px" />
                                            <col />
                                        </colgroup>
                                        <tr>
                                            <td class="tdLabel">
                                                <label class="lblNormal">
                                                    <%=GetLabel("Filter Transaksi")%></label>
                                            </td>
                                            <td>
                                                <asp:RadioButtonList ID="rblItemType" runat="server" RepeatDirection="Horizontal"
                                                    RepeatLayout="Table">
                                                    <asp:ListItem Text=" Semua" Value="0" Selected="True" />
                                                    <asp:ListItem Text=" Kunjungan/Perawatan saat ini " Value="1" />
                                                </asp:RadioButtonList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2" valign="top">
                                                <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
                                                    ShowLoadingPanel="false" OnCallback="cbpView_Callback">
                                                    <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingView').show(); }"
                                                        EndCallback="function(s,e){ onCbpViewEndCallback(s); }" />
                                                    <PanelCollection>
                                                        <dx:PanelContent ID="PanelContent1" runat="server">
                                                            <asp:Panel runat="server" ID="pnlView" CssClass="pnlContainerGridPatientPage" Height="500px">
                                                                <asp:GridView ID="grdView" runat="server" CssClass="grdSelected grdPatientPage" AutoGenerateColumns="false"
                                                                    ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                                                    <Columns>
                                                                        <asp:BoundField DataField="TransactionID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                                                        <asp:BoundField DataField="Remarks" HeaderStyle-CssClass="hiddenColumn" ItemStyle-CssClass="remarks hiddenColumn" />
                                                                        <asp:TemplateField HeaderStyle-HorizontalAlign="Left">
                                                                            <HeaderTemplate>
                                                                                <div>
                                                                                    <%=GetLabel("Informasi Transaksi") %></div>
                                                                            </HeaderTemplate>
                                                                            <ItemTemplate>
                                                                                <div>
                                                                                    <label style="font-style: italic">
                                                                                        <%=GetLabel("Tgl/Jam Transaksi :") %>
                                                                                    </label>
                                                                                    <label style="font-size: medium">
                                                                                        <%#: Eval("cfTransactionDateInString")%>
                                                                                        |
                                                                                        <%#: Eval("TransactionTime") %>
                                                                                    </label>
                                                                                </div>
                                                                                <div>
                                                                                    <label style="font-style: italic">
                                                                                        <%=GetLabel("No Transaksi :") %>
                                                                                    </label>
                                                                                    <label style="font-size: large; font-weight: bold">
                                                                                        <%#: Eval("TransactionNo")%>
                                                                                    </label>
                                                                                </div>
                                                                                <div>
                                                                                    <label style="font-weight: bold">
                                                                                        <%#: Eval("cfPAReferenceNo")%>
                                                                                    </label>
                                                                                </div>
                                                                                <div>
                                                                                    <label style="font-style: italic">
                                                                                        <%=GetLabel("Tgl/Jam Hasil :") %>
                                                                                    </label>
                                                                                    <label style="font-size: medium">
                                                                                        <%#: Eval("cfResultDateTimeFullCaption")%>
                                                                                    </label>
                                                                                    <div <%# Eval("cfIsHasResultAttachment").ToString() != "True" ? "Style='display:none'":"" %>  >
                                                                                        <label style="font-size: medium" class="lblLink lblViewAttachment" attachmentValue = '<%#: Eval("ResultAttachment")%>'>
                                                                                            View Attachment
                                                                                        </label>
                                                                                    </div>
                                                                                </div>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                    </Columns>
                                                                    <EmptyDataTemplate>
                                                                        <%=GetLabel("Tidak ada data pemeriksaan untuk pasien ini")%>
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
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </td>
                            <td valign="top">
                                <div style="position: relative;">
                                    <table border="0" cellpadding="0" cellspacing="1" style="width: 100%">
                                        <colgroup>
                                            <col style="width: 150px" />
                                            <col />
                                        </colgroup>
                                        <tr>
                                            <td colspan="2" valign="top">
                                                <dxcp:ASPxCallbackPanel ID="cbpViewDt" runat="server" Width="100%" ClientInstanceName="cbpViewDt"
                                                    ShowLoadingPanel="false" OnCallback="cbpViewDt_Callback">
                                                    <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingViewDt').show(); }"
                                                        EndCallback="function(s,e){ onCbpViewDtEndCallback(s); }" />
                                                    <PanelCollection>
                                                        <dx:PanelContent ID="PanelContent2" runat="server">
                                                            <asp:Panel runat="server" ID="Panel1" CssClass="pnlContainerGridPatientPage">
                                                                <asp:GridView ID="grdViewDt" runat="server" CssClass="grdLabResultDt grdPatientPage"
                                                                    AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty"
                                                                    OnSorting="grdViewDt_Sorting" OnRowDataBound="grdViewDt_RowDataBound">
                                                                    <Columns>
                                                                        <asp:BoundField DataField="cfTextValue" HeaderStyle-CssClass="hiddenColumn" ItemStyle-CssClass="textResultValue hiddenColumn" />
                                                                        <asp:BoundField DataField="ItemGroupName1" HeaderText="ItemGroupName1" HeaderStyle-CssClass="hiddenColumn"
                                                                            ItemStyle-CssClass="itemGroupName1 hiddenColumn" SortExpression="ItemGroupName1" />
                                                                        <asp:TemplateField HeaderStyle-HorizontalAlign="Left" ItemStyle-Width="200px" SortExpression="FractionName1">
                                                                            <HeaderTemplate>
                                                                                <div>
                                                                                    <%=GetLabel("Pemeriksaan") %>
                                                                                </div>
                                                                            </HeaderTemplate>
                                                                            <ItemTemplate>
                                                                                <div style="padding-left: 10px">
                                                                                    <div title='<%#: Eval("ItemName1") %>' class='<%#: Eval("IsNormal").ToString() == "False" && Eval("cfIsResultInPDF").ToString() == "False" ?  (Eval("cfIsPanicRange").ToString() == "False" ? "isAbnormalColor" : "isPanicRangeColor blink-critical") : "" %>'>
                                                                                        <%#: Eval("FractionName1") %></div>
                                                                                    <div style="font-style: italic; display: none">
                                                                                        <%#: Eval("ItemName1") %></div>
                                                                                    <input type="hidden" value="<%#:Eval("ChargeTransactionID") %>" bindingfield="ChargeTransactionID" />
                                                                                    <input type="hidden" value="<%#:Eval("cfReferenceRange") %>" bindingfield="cfReferenceRange" />
                                                                                    <input type="hidden" value="<%#:Eval("ItemID") %>" bindingfield="ItemID" />
                                                                                    <input type="hidden" value="<%#:Eval("FractionID") %>" bindingfield="FractionID" />
                                                                                </div>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Hasil" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right"
                                                                            ItemStyle-Width="150px">
                                                                            <ItemTemplate>
                                                                                <div id="divHiddenResult" runat="server" class='<%#: Eval("IsNormal").ToString() == "False" && Eval("cfIsResultInPDF").ToString() == "False" ?  (Eval("cfIsPanicRange").ToString() == "False" ? "isAbnormalColor" : "isPanicRangeColor") : "" %>'
                                                                                    style="text-align: right">
                                                                                    <div class='<%#: Eval("cfIsResultInPDF").ToString() != "False" ? "lblLink lblViewPDF" : "" %>'>
                                                                                        <asp:Literal ID="literalResult" Text='<%# Eval("cfTestResultValueNew") %>' Mode="PassThrough"
                                                                                            runat="server" />
                                                                                    </div>
                                                                                </div>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Satuan" ItemStyle-Width="80px" HeaderStyle-HorizontalAlign="Left"
                                                                            ItemStyle-HorizontalAlign="Left">
                                                                            <ItemTemplate>
                                                                                <div class='<%#: Eval("IsNormal").ToString() == "False" ?  (Eval("cfIsPanicRange").ToString() == "False" ? "isAbnormalColor" : "isPanicRangeColor") : "" %>'>
                                                                                    <%#: Eval("MetricUnit") %></div>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Nilai Referensi" ItemStyle-Width="140px" HeaderStyle-HorizontalAlign="Left"
                                                                            ItemStyle-HorizontalAlign="Left">
                                                                            <ItemTemplate>
                                                                                <div class='<%#: Eval("cfIsReferenceAsLink").ToString() == "True" ? "lblLink lblReffRange" : "" %>'>
                                                                                    <asp:Literal ID="literal" Text='<%# GetISBridgingToLIS() == "True" ? Eval("cfReferenceRangeCustom") : Eval("MetricUnitLabel") %>'
                                                                                        Mode="PassThrough" runat="server" /></div>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Flag" ItemStyle-Width="40px" HeaderStyle-HorizontalAlign="Center"
                                                                            ItemStyle-HorizontalAlign="Center">
                                                                            <ItemTemplate>
                                                                                <div id="divHiddenResultUnit" runat="server" class='<%#: Eval("IsNormal").ToString() == "False" ?  (Eval("cfIsPanicRange").ToString() == "False" ? "isAbnormalColor" : "isPanicRangeColor") : "" %>'
                                                                                    style="text-align: right">
                                                                                    <%#: Eval("ResultFlag") %></div>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Pending" ItemStyle-Width="50px" HeaderStyle-HorizontalAlign="Center"
                                                                            ItemStyle-HorizontalAlign="Center" Visible="True">
                                                                            <ItemTemplate>
                                                                                <div>
                                                                                    <%#: Eval("cfIsPendingResult") %></div>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="Verifikasi" ItemStyle-Width="200px" HeaderStyle-HorizontalAlign="Left"
                                                                            ItemStyle-HorizontalAlign="Left" Visible="False">
                                                                            <ItemTemplate>
                                                                                <div>
                                                                                    <%#: Eval("VerifiedUserName") %></div>
                                                                                <div>
                                                                                    <%#: Eval("VerifiedDateInString") %></div>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField HeaderText="" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                                            Visible="true">
                                                                            <ItemTemplate>
                                                                                <div class='lblCommentDetail'>
                                                                                    <%#: Eval("Remarks") %>
                                                                                </div>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                    </Columns>
                                                                    <EmptyDataTemplate>
                                                                        <span class="blink">
                                                                            <%=GetLabel("Belum ada informasi hasil pemeriksaan untuk transaksi ini") %></span>
                                                                    </EmptyDataTemplate>
                                                                </asp:GridView>
                                                            </asp:Panel>
                                                        </dx:PanelContent>
                                                    </PanelCollection>
                                                </dxcp:ASPxCallbackPanel>
                                                <div class="imgLoadingGrdView" id="containerImgLoadingViewDt">
                                                    <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                                                </div>
                                                <div class="containerPaging">
                                                    <div class="wrapperPaging">
                                                        <div id="pagingDt">
                                                        </div>
                                                    </div>
                                                </div>
                                                <div>
                                                    <table border="0" cellpadding="1" cellspacing="0" width="100%">
                                                        <colgroup>
                                                            <col style="width: 150px" />
                                                            <col />
                                                        </colgroup>
                                                        <tr>
                                                            <td class="tdLabel" valign="top" style="padding-top: 5px;">
                                                                <label class="lblNormal" id="lblResultRemarks">
                                                                    <%=GetLabel("Remarks/Comment") %></label>
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtRemarks" runat="server" Width="100%" TextMode="Multiline" Rows="6"
                                                                    ReadOnly="true" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </td>
                        </tr>
                    </table>
                </div>
                <div class="containerOrderDt" id="panFraction" style='display: none'>
                    <table style="width: 100%">
                        <colgroup>
                            <col style="width: 300px" />
                            <col />
                        </colgroup>
                        <tr>
                            <td colspan="2">
                                <table border="0" cellpadding="0" cellspacing="1" width="100%">
                                    <colgroup>
                                        <col style="width: 150px" />
                                        <col />
                                    </colgroup>
                                    <tr>
                                        <td class="tdLabel">
                                            <label class="lblNormal">
                                                <%=GetLabel("Filter Transaksi")%></label>
                                        </td>
                                        <td>
                                            <asp:RadioButtonList ID="rblItemTypeTab2" runat="server" RepeatDirection="Horizontal"
                                                RepeatLayout="Table">
                                                <asp:ListItem Text=" Semua" Value="0" Selected="True" />
                                                <asp:ListItem Text=" Kunjungan/Perawatan saat ini " Value="1" />
                                            </asp:RadioButtonList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="tdLabel">
                                            <label class="lblNormal">
                                                <%=GetLabel("Periode Tanggal Hasil")%></label>
                                        </td>
                                        <td colspan="2">
                                            <table width="100%" cellspacing="0" cellpadding="0">
                                                <tr>
                                                    <td>
                                                        <table cellpadding="0" cellspacing="0">
                                                            <tr>
                                                                <td>
                                                                    <asp:TextBox runat="server" Width="120px" ID="txtPeriodFrom" CssClass="datepicker" />
                                                                </td>
                                                                <td style="width: 30px; text-align: center">
                                                                    s/d
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox runat="server" Width="120px" ID="txtPeriodTo" CssClass="datepicker" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr style="display: none">
                                        <td>
                                            <label class="lblNormal">
                                                <%=GetLabel("Artikel Pemeriksaan")%></label>
                                        </td>
                                        <td colspan="2">
                                            <table width="100%" cellspacing="0" cellpadding="0">
                                                <tr>
                                                    <td colspan="2">
                                                        <table cellpadding="0" cellspacing="0">
                                                            <tr>
                                                                <td>
                                                                    <asp:TextBox ID="txtFractionSearch" Width="300px" runat="server" />
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
                            <td valign="top">
                                <div style="position: relative;">
                                    <dxcp:ASPxCallbackPanel ID="cbpViewTab2" runat="server" Width="100%" ClientInstanceName="cbpViewTab2"
                                        ShowLoadingPanel="false" OnCallback="cbpViewTab2_Callback">
                                        <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingView').show(); }"
                                            EndCallback="function(s,e){ onCbpViewTab2EndCallback(s); }" />
                                        <PanelCollection>
                                            <dx:PanelContent ID="PanelContent3" runat="server">
                                                <asp:Panel runat="server" ID="Panel2" CssClass="pnlContainerGridPatientPage">
                                                    <asp:GridView ID="grdViewTab2" runat="server" CssClass="grdSelected grdPatientPage"
                                                        AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                                        <Columns>
                                                            <asp:BoundField DataField="ItemGroupCode" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Left">
                                                                <HeaderTemplate>
                                                                    <div>
                                                                        <%=GetLabel("Kelompok Pemeriksaan") %>
                                                                        <div>
                                                                </HeaderTemplate>
                                                                <ItemTemplate>
                                                                    <div>
                                                                        <%#: Eval("ItemGroupName")%></div>
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                        </Columns>
                                                        <EmptyDataTemplate>
                                                            <%=GetLabel("Tidak ada data artikel pemeriksaan untuk pasien ini")%>
                                                        </EmptyDataTemplate>
                                                    </asp:GridView>
                                                </asp:Panel>
                                            </dx:PanelContent>
                                        </PanelCollection>
                                    </dxcp:ASPxCallbackPanel>
                                </div>
                            </td>
                            <td valign="top">
                                <div style="position: relative;">
                                    <dxcp:ASPxCallbackPanel ID="cbpViewDtTab2" runat="server" Width="100%" ClientInstanceName="cbpViewDtTab2"
                                        ShowLoadingPanel="false" OnCallback="cbpViewDtTab2_Callback">
                                        <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingViewDt').show(); }"
                                            EndCallback="function(s,e){ onCbpViewDtTab2EndCallback(s); }" />
                                        <PanelCollection>
                                            <dx:PanelContent ID="PanelContent4" runat="server">
                                                <asp:Panel runat="server" ID="Panel3" CssClass="pnlContainerGridPatientPage">
                                                    <asp:GridView ID="grdViewDt2" runat="server" CssClass="grdSelected grdPatientPage"
                                                        AutoGenerateColumns="false" ShowHeaderWhenEmpty="false" EmptyDataRowStyle-CssClass="trEmpty"
                                                        OnRowDataBound="grdViewDt2_OnRowDataBound">
                                                        <EmptyDataTemplate>
                                                            <%=GetLabel("Tidak ada data artikel pemeriksaan untuk pasien ini")%>
                                                        </EmptyDataTemplate>
                                                    </asp:GridView>
                                                </asp:Panel>
                                            </dx:PanelContent>
                                        </PanelCollection>
                                    </dxcp:ASPxCallbackPanel>
                                </div>
                            </td>
                        </tr>
                    </table>
                </div>
            </td>
        </tr>
    </table>
</asp:Content>
