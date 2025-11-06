<%@ Page Title="" Language="C#" MasterPageFile="~/libs/MasterPage/ParamedicPage/MPBaseParamedicPageTrx.master"
    AutoEventWireup="true" CodeBehind="RevenueSharingEditEntry.aspx.cs" Inherits="QIS.Medinfras.Web.Finance.Program.RevenueSharingEditEntry" %>
    
<%@ Register Assembly="QIS.Medinfras.Web.CustomControl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"
    Namespace="QIS.Medinfras.Web.CustomControl" TagPrefix="qis" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Src="~/Program/RevenueSharing/RevenueSharingToolbarCtl.ascx" TagName="ToolbarCtl"
    TagPrefix="uc1" %>
<asp:Content ID="Content3" ContentPlaceHolderID="plhMPPatientPageNavigationPane"
    runat="server">
    <uc1:ToolbarCtl ID="ctlToolbar" runat="server" />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="plhCustomButtonToolbar" runat="server">
    <li id="btnVoidByReason" runat="server" crudmode="R">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbvoid.png")%>' alt="" /><div>
            <%=GetLabel("Void")%></div>
    </li>
    <li id="btnReOpen" runat="server" crudmode="R">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbvoid.png")%>' alt="" /><div>
            <%=GetLabel("Re-Open")%></div>
    </li>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div style="font-size: 1.4em">
        <%=HttpUtility.HtmlEncode(GetPageTitle())%></div>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript">

        function onLoad() {
            setRightPanelButtonEnabled();

            $('#btnSave').click(function () {
                cbpProcess.PerformCallback('save');
            });

            $('#btnClose').click(function () {
                hideEditForm();
            });

            $('#btnProcessDiscount').click(function () {
                var transactionStatus = $('#<%=hdnGCTransactionStatus.ClientID %>').val();
                if (transactionStatus == 'X121^001') {
                    cbpProcessDiscount.PerformCallback();
                } else {
                    showToast('Process Failed', 'Error Message : Status transaksi jasa medis sudah BUKAN OPEN lagi, tidak dapat proses diskon.');
                }
            });

            var pageCount = parseInt($('#<%=hdnPageCount.ClientID %>').val());
            setPaging($("#paging"), pageCount, function (page) {
                cbpView.PerformCallback('changepage|' + page);
            });

            setCustomToolbarVisibility();
        }

        function setCustomToolbarVisibility() {
            var transactionStatus = $('#<%=hdnGCTransactionStatus.ClientID %>').val();
            var isVoid = $('#<%:hdnIsAllowVoidByReason.ClientID %>').val();
            if (transactionStatus != 'X121^001') {
                $('#<%=btnVoidByReason.ClientID %>').hide();
                if (transactionStatus == 'X121^003') {
                    $('#<%=btnReOpen.ClientID %>').show();
                } else {
                    $('#<%=btnReOpen.ClientID %>').hide();
                }
            }
            else if (transactionStatus == 'X121^001') {
                $('#<%=btnReOpen.ClientID %>').hide();
                if (isVoid == 1) {
                    $('#<%=btnVoidByReason.ClientID %>').show();
                } else {
                    $('#<%=btnVoidByReason.ClientID %>').hide();
                }
            }
        }

        $('#<%=btnVoidByReason.ClientID %>').live('click', function () {
            var transactionStatus = $('#<%=hdnGCTransactionStatus.ClientID %>').val();
            if (transactionStatus == 'X121^001') {
                showDeleteConfirmation(function (data) {
                    var param = 'justvoid;' + data.GCDeleteReason + ';' + data.Reason;
                    onCustomButtonClick(param);
                });
            } else {
                showToast('Process Failed', 'Error Message : Status transaksi jasa medis sudah BUKAN OPEN lagi, tidak dapat proses VOID.');
            }
        });

        $('#<%=btnReOpen.ClientID %>').live('click', function () {
            var transactionStatus = $('#<%=hdnGCTransactionStatus.ClientID %>').val();
            if (transactionStatus == 'X121^003') {
                onCustomButtonClick('reopen');
            } else {
                showToast('Process Failed', 'Error Message : Status transaksi jasa medis sudah BUKAN APPROVED lagi, tidak dapat proses REOPEN.');
            }
        });

        function onAfterCustomClickSuccess(type, retval) {
            onLoadObject(retval);
        }

        function hideEditForm() {
            $('#<%=txtTransactionAmount.ClientID%>').val('');
            $('#<%=txtCCFee.ClientID%>').val('');
            $('#<%=txtDiscountPercentage.ClientID%>').val('');
            $('#<%=txtNett.ClientID%>').val('');
            $tr.find('.tdComponent').each(function () {
                var type = $(this).attr('type').split('^')[1];
                $('.txtCompValue[comptype=' + type + ']').val('');
            });
            $('#containerPopupEntryData').hide();
        }

        $('.txtEditValue').live('change', function () {
            $(this).blur();
            updateSharingAmount();
        });

        function updateSharingAmount() {
            var TransactionAmount = parseFloat($('#<%=txtTransactionAmount.ClientID%>').attr('hiddenVal'));
            $('.txtEditValue').each(function () {
                var pengurang = parseFloat($(this).attr('hiddenVal'));
                TransactionAmount -= pengurang;
            });

            $('#<%=txtNett.ClientID %>').val(TransactionAmount).trigger('changeValue');
            var discountAmount = $('#<%=txtDiscountPercentage.ClientID %>').val() * TransactionAmount / 100;
            $('#<%=txtDiscountAmount.ClientID %>').val(discountAmount).trigger('changeValue');
            $('#<%=txtNettAfterDiscount.ClientID %>').val(TransactionAmount - discountAmount).trigger('changeValue');
        }

        $('.imgDelete.imgLink').live('click', function () {
            $tr = $(this).closest('tr');
            showToastConfirmation('Apakah Anda Yakin?', function (result) {
                if (result) {
                    var entity = rowToObject($tr);
                    $('#<%=hdnTransactionDtID.ClientID %>').val(entity.TransactionDtID);
                    cbpProcess.PerformCallback('delete');
                }
            });
        });

        $('.imgEdit.imgLink').live('click', function () {
            $tr = $(this).closest('tr');
            var entity = rowToObject($tr);

            $('#<%=chkServiceIsVariable.ClientID %>').prop('checked', entity.IsVariable == 'True');
            if (entity.IsVariable == 'True') {
                $('#<%=txtTransactionAmount.ClientID %>').removeAttr('readonly');
                $('#<%=txtNett.ClientID %>').removeAttr('readonly');
            }
            else {
                $('#<%=txtTransactionAmount.ClientID %>').attr('readonly', 'readonly');
                $('#<%=txtNett.ClientID %>').attr('readonly', 'readonly');
            }

            $('#<%=hdnTransactionDtID.ClientID %>').val(entity.TransactionDtID)
            $('#<%=txtTransactionAmount.ClientID%>').val(entity.TransactionAmount).trigger('changeValue');
            $('#<%=txtCCFee.ClientID%>').val(entity.CCFee).trigger('changeValue');
            $('#<%=txtDiscountAmount.ClientID%>').val(entity.DiscountAmount).trigger('changeValue');

            var revenueSharingAmount = parseFloat(entity.RevenueSharingAmount);
            var discountAmount = parseFloat(entity.DiscountAmount);
            var discount = (discountAmount / (revenueSharingAmount + discountAmount) * 100);
            $('#<%=txtDiscountPercentage.ClientID%>').val(discount).trigger('changeValue');
            $('#<%=txtNett.ClientID%>').val(revenueSharingAmount + discountAmount).trigger('changeValue');
            $('#<%=txtNettAfterDiscount.ClientID%>').val(revenueSharingAmount).trigger('changeValue');

            $tr.find('.tdComponent').each(function () {
                var val = $(this).find('.hdnViewComponent').val();
                var type = $(this).attr('type').split('^')[1];
                $('.txtCompValue[comptype=' + type + ']').val(val).trigger('changeValue');
            });

            var TransactionDtID = $(this).closest('tr').find('.keyField').html();
            $('#containerPopupEntryData').show();
        });

        //#region Revenue Sharing No
        function onGetRevenueSharingFilterExpression() {
            var filterExpression = "<%:OnGetRevenueSharingFilterExpression() %>";
            return filterExpression;
        }

        $('#lblRevenueSharingNo.lblLink').live('click', function () {
            openSearchDialog('transrevenuesharinghd', onGetRevenueSharingFilterExpression(), function (value) {
                $('#<%=txtRevenueSharingNo.ClientID %>').val(value);
                ontxtRevenueSharingNoChanged(value);
            });
        });

        $('#<%=txtRevenueSharingNo.ClientID %>').live('change', function () {
            ontxtRevenueSharingNoChanged($(this).val());
        });

        function ontxtRevenueSharingNoChanged(value) {
            var filterExpression = onGetRevenueSharingFilterExpression() + " AND RevenueSharingNo = '" + value + "'";
            Methods.getObject('GetTransRevenueSharingHdList', filterExpression, function (result) {
                if (result != null) {
                    onLoadObject(value);
                }
                else {
                    $('#<%=hdnRSTransactionID.ClientID %>').val('');
                    $('#<%=txtRevenueSharingNo.ClientID %>').val('');
                    $('#<%=txtProcessedDate.ClientID %>').val('');
                }
            });
        }
        //#endregion
        
        $('#<%=txtNett.ClientID %>').live('change', function () {
            $(this).blur();

            var discAmount = parseFloat($('#<%=txtDiscountAmount.ClientID %>').attr('hiddenVal'));

            var TransactionAmount = 0;
            if ($('#<%=chkServiceIsVariable.ClientID %>').is(':checked')) {
                TransactionAmount = parseFloat($('#<%=txtNett.ClientID%>').val().replace('.00', '').split(',').join(''));
            } else {
                TransactionAmount = parseFloat($('#<%=txtNett.ClientID%>').attr('hiddenVal'));
            }

            $('#<%=txtNettAfterDiscount.ClientID%>').val(TransactionAmount - discAmount).trigger('changeValue');
        });

        $('#<%=txtDiscountPercentage.ClientID %>').live('change', function () {
            var TransactionAmount = 0;
            if ($('#<%=chkServiceIsVariable.ClientID %>').is(':checked')) {
                TransactionAmount = parseFloat($('#<%=txtNett.ClientID%>').val().replace('.00', '').split(',').join(''));
            } else {
                TransactionAmount = parseFloat($('#<%=txtNett.ClientID%>').attr('hiddenVal'));
            }

            var discount = $(this).val() * TransactionAmount / 100;
            $('#<%=txtDiscountAmount.ClientID%>').val(discount).trigger('changeValue');

            $('#<%=txtNettAfterDiscount.ClientID%>').val(TransactionAmount - discount).trigger('changeValue');
        });

        $('#<%=txtDiscountAmount.ClientID %>').live('change', function () {
            $(this).blur();

            var val = parseFloat($(this).attr('hiddenVal'));

            var TransactionAmount = 0;
            if ($('#<%=chkServiceIsVariable.ClientID %>').is(':checked')) {
                TransactionAmount = parseFloat($('#<%=txtNett.ClientID%>').val().replace('.00', '').split(',').join(''));
            } else {
                TransactionAmount = parseFloat($('#<%=txtNett.ClientID%>').attr('hiddenVal'));
            }

            var discount = parseFloat(val * 100 / TransactionAmount);

            $('#<%=txtDiscountPercentage.ClientID%>').val(discount);

            $('#<%=txtNettAfterDiscount.ClientID%>').val(TransactionAmount - val).trigger('changeValue');
        });

        function onCbpProcessEndCallback(s) {
            var param = s.cpResult.split('|');
            if (param[0] == 'fail')
                showToast('Process Failed', 'Error Message : ' + param[1]);
            else {
                //showToast('Process Success', 'Proses Edit Honor Dokter Berhasil Dilakukan');
                hideEditForm();
                cbpView.PerformCallback('refresh');
            }

            hideLoadingPanel();
        }

        function onCbpProcessDiscountEndCallback(s) {
            var param = s.cpResult.split('|');
            if (param[0] == 'fail')
                showToast('Process Failed', 'Error Message : ' + param[1]);
            else {
                showToast('Process Success', 'Proses Diskon Honor Dokter Berhasil Dilakukan');
                cbpView.PerformCallback('refresh');
            }

            hideLoadingPanel();
        }

        //#region Paging
        var pageCount = parseInt('<%=PageCount %>');
        $(function () {
            setPaging($("#paging"), pageCount, function (page) {
                cbpView.PerformCallback('changepage|' + page);
            });
        });

        function onCbpViewEndCallback(s) {
            hideLoadingPanel();
            var param = s.cpResult.split('|');
            if (param[0] == 'refresh') {
                var pageCount = parseInt(param[1]);
                $('#<%=txtTotalRSAmount.ClientID %>').val(param[2]);
                $('#<%=txtTotalBrutoAmount.ClientID %>').val(param[3]);
                if (pageCount > 0)
                    $('#grdView tr:eq(2)').click();

                setPaging($("#paging"), pageCount, function (page) {
                    cbpView.PerformCallback('changepage|' + page);
                });
            }
            else
                $('#grdView tr:eq(2)').click();
        }
        //#endregion

        function onRefreshGrid() {
            $('#<%=hdnFilterExpressionQuickSearch.ClientID %>').val(txtSearchView.GenerateFilterExpression());
            cbpView.PerformCallback('refresh');
        }

        function onTxtSearchViewSearchClick(s) {
            setTimeout(function () {
                s.SetBlur();
                onRefreshGrid();
            }, 0);
        }

        $('#<%=chkServiceIsVariable.ClientID %>').live('change', function () {
            if ($(this).is(':checked')) {
                $('#<%=txtTransactionAmount.ClientID %>').removeAttr('readonly');
                $('#<%=txtNett.ClientID %>').removeAttr('readonly');
            }
            else {
                $('#<%=txtTransactionAmount.ClientID %>').attr('readonly', 'readonly');
                $('#<%=txtNett.ClientID %>').attr('readonly', 'readonly');
            }
        });

        function setRightPanelButtonEnabled() {
            var transID = $('#<%=hdnRSTransactionID.ClientID %>').val();
            if (transID != "" && transID != null && transID != "0") {
                $('#btnInfoRevAdj').removeAttr('enabled');
            } else {
                $('#btnInfoRevAdj').attr('enabled', 'false');
            }
        }

        function onBeforeLoadRightPanelContent(code) {
            var transID = $('#<%=hdnRSTransactionID.ClientID %>').val();
            if (code == 'infoAdj') {
                return transID;
            }
        }

        function onBeforeRightPanelPrint(code, filterExpression, errMessage) {
            var transID = $('#<%=hdnRSTransactionID.ClientID %>').val();
            var paramedicID = $('#<%=hdnParamedicID.ClientID %>').val();
            var processedDate = $('#<%=txtProcessedDate.ClientID %>').val();
            if (transID == '' || transID == '0') {
                errMessage.text = 'Please Select Transaction First!';
                return false;
            }
            else if (code == 'FN-00231') {
                filterExpression.text = transID;
                return true;
            }
            else if (code == 'FN-00259') {
                filterExpression.text = processedDate + "|" + paramedicID;
                return true;
            }
            else {
                filterExpression.text = "RSTransactionID = " + transID;
                return true;
            }
        }
    </script>
    <div>
        <input type="hidden" id="hdnPageTitle" runat="server" />
        <input type="hidden" id="hdnParamedicID" runat="server" />
        <input type="hidden" id="hdnPageCount" runat="server" />
        <input type="hidden" id="hdnGCTransactionStatus" runat="server" />
        <input type="hidden" id="hdnIsAllowVoidByReason" runat="server" />
        <input type="hidden" value="" id="hdnFilterExpressionQuickSearch" runat="server" />
        <table class="tblContentArea" style="width: 100%">
            <colgroup>
                <col style="width: 70%" />
                <col style="width: 30%" />
            </colgroup>
            <tr>
                <td valign="top">
                    <table>
                        <colgroup>
                            <col style="width: 180px" />
                            <col style="width: 500px" />
                        </colgroup>
                        <tr>
                            <td>
                                <label class="lblNormal lblLink" id="lblRevenueSharingNo">
                                    <%=GetLabel("Nomor Bukti / Tgl Proses")%></label>
                            </td>
                            <td>
                                <table width="100%" cellspacing="0" cellpadding="0">
                                    <tr>
                                        <td>
                                            <input type="hidden" id="hdnRSTransactionID" runat="server" />
                                            <asp:TextBox runat="server" ID="txtRevenueSharingNo" Width="100%" />
                                        </td>
                                        <td>
                                            <asp:TextBox runat="server" ID="txtProcessedDate" Width="100%" Style="text-align: center"
                                                ReadOnly="true" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label>
                                    <%=GetLabel("Alokasi Pajak / Cara Bayar") %></label>
                            </td>
                            <td>
                                <table width="100%" cellspacing="0" cellpadding="0">
                                    <tr>
                                        <td>
                                            <asp:TextBox runat="server" ID="txtReduction" Enabled="false" Width="100%" />
                                        </td>
                                        <td>
                                            <asp:TextBox runat="server" ID="txtPaymentMethod" Enabled="false" Width="100%" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label>
                                    <%=GetLabel("Grup Pelayanan") %></label>
                            </td>
                            <td>
                                <table width="100%" cellspacing="0" cellpadding="0">
                                    <tr>
                                        <td>
                                            <asp:TextBox runat="server" ID="txtClinicGroup" Enabled="false" Width="100%" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label>
                                    <%=GetLabel("Catatan") %></label>
                            </td>
                            <td>
                                <table width="100%" cellspacing="0" cellpadding="0">
                                    <tr>
                                        <td>
                                            <asp:TextBox runat="server" ID="txtRemarks" Width="100%" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label>
                                    <%=GetLabel("Periode") %></label>
                            </td>
                            <td>
                                <table width="100%" cellspacing="0" cellpadding="0">
                                    <tr>
                                        <td>
                                            <asp:TextBox runat="server" ID="txtPeriodeType" Enabled="false" Width="100%" />
                                        </td>
                                        <td>
                                            <asp:TextBox runat="server" ID="txtPeriode" Enabled="false" Width="100%" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label>
                                    <%=GetLabel("Total Nilai Bruto") %></label>
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtTotalBrutoAmount" Enabled="false" Width="50%"
                                    CssClass="txtCurrency" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label>
                                    <%=GetLabel("Total Nilai JasMed") %></label>
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtTotalRSAmount" Enabled="false" Width="50%" CssClass="txtCurrency" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label>
                                    <%=GetLabel("Quick Filter")%></label>
                            </td>
                            <td>
                                <qis:QISIntellisenseTextBox runat="server" ClientInstanceName="txtSearchView" ID="txtSearchView"
                                    width="250px" Watermark="Search">
                                    <ClientSideEvents SearchClick="function(s){ onTxtSearchViewSearchClick(s); }" />
                                    <IntellisenseHints>
                                        <qis:QISIntellisenseHint Text="No. Registrasi" FieldName="RegistrationNo" />
                                    </IntellisenseHints>
                                </qis:QISIntellisenseTextBox>
                            </td>
                        </tr>
                    </table>
                </td>
                <td valign="top">
                    <table width="100%">
                        <tr>
                            <td colspan="2" style="font-size: large; color: Red; font-style: italic; font-weight: bold">
                                <%=GetLabel("Perhatian")%>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" style="font-size: small; color: Red;">
                                <%=GetLabel("Proses Diskon akan memotong jumlah Jasa Medis Nett setelah melalui proses perhitungan Formula Jasa Medis.")%>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label class="lblNormal" id="lblDiskon">
                                    <%=GetLabel("Diskon Jasa Medis [%]")%></label>
                            </td>
                            <td>
                                <asp:TextBox runat="server" ID="txtDiscount1" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <input type="button" id="btnProcessDiscount" value="Proses Diskon" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <div id="containerPopupEntryData" style="margin-top: 10px; display: none;">
                        <input type="hidden" id="hdnID" runat="server" value="" />
                        <div class="pageTitle">
                            <%=GetLabel("Edit Detail")%></div>
                        <fieldset id="fsEntryPopup" style="margin: 0">
                            <table class="tblEntryDetail" style="width: 100%">
                                <tr>
                                    <td style="width: 150px;">
                                        <label class="lblNormal">
                                            <%=GetLabel("Variable")%></label>
                                    </td>
                                    <td>
                                        <asp:CheckBox ID="chkServiceIsVariable" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 150px;">
                                        <label class="lblNormal">
                                            <%=GetLabel("Nilai Transaksi")%></label>
                                    </td>
                                    <td>
                                        <asp:TextBox runat="server" ID="txtTransactionAmount" CssClass="txtCurrency" />
                                    </td>
                                </tr>
                                <asp:Repeater ID="rptSharingComp" runat="server">
                                    <ItemTemplate>
                                        <tr class="trSharingComp">
                                            <td style="width: 150px;">
                                                <label class="lblNormal">
                                                    <%#:Eval("StandardCodeName")%></label>
                                            </td>
                                            <td>
                                                <input type="hidden" id="hdnGCSharingComponent" runat="server" value='<%#:Eval("StandardCodeID")%>' />
                                                <asp:TextBox runat="server" ID="txtCompValue" comptype='<%#:Eval("cfStandardCodeID")%>'
                                                    CssClass="txtCompValue txtEditValue txtCurrency" />
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                </asp:Repeater>
                                <tr>
                                    <td style="width: 150px;">
                                        <label class="lblNormal">
                                            <%=GetLabel("CC. Fee")%></label>
                                    </td>
                                    <td>
                                        <asp:TextBox runat="server" ID="txtCCFee" CssClass="txtCurrency txtEditValue" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 150px;">
                                        <label class="lblNormal">
                                            <%=GetLabel("Nett")%></label>
                                    </td>
                                    <td>
                                        <asp:TextBox runat="server" ID="txtNett" CssClass="txtCurrency" ReadOnly="true" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 150px;">
                                        <label class="lblNormal">
                                            <%=GetLabel("Diskon %")%></label>
                                    </td>
                                    <td>
                                        <asp:TextBox runat="server" ID="txtDiscountPercentage" CssClass="number" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 150px;">
                                        <label class="lblNormal">
                                            <%=GetLabel("Diskon")%></label>
                                    </td>
                                    <td>
                                        <asp:TextBox runat="server" ID="txtDiscountAmount" CssClass="txtCurrency" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 150px;">
                                        <label class="lblNormal">
                                            <%=GetLabel("Nett After Discount")%></label>
                                    </td>
                                    <td>
                                        <asp:TextBox runat="server" ID="txtNettAfterDiscount" CssClass="txtCurrency" ReadOnly="true" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <input type="button" id="btnSave" value="Save" />&nbsp;<input type="button" id="btnClose"
                                            value="Close" />
                                    </td>
                                </tr>
                            </table>
                        </fieldset>
                    </div>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <input type="hidden" id="hdnIsEditable" runat="server" value="" />
                    <input type="hidden" id="hdnTransactionDtID" runat="server" />
                    <input type="hidden" id="hdnFilterExpression" runat="server" value="" />
                    <div style="position: relative;" id="dView">
                        <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
                            ShowLoadingPanel="false" OnCallback="cbpView_Callback">
                            <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ onCbpViewEndCallback(s); }" />
                            <PanelCollection>
                                <dx:PanelContent ID="PanelContent1" runat="server">
                                    <asp:Panel runat="server" ID="pnlView" CssClass="pnlContainerGrid">
                                        <table class="grdRevenueSharing grdSelected" cellspacing="0" width="100%" rules="all">
                                            <tr>
                                                <th rowspan="2" style="width: 80px">
                                                </th>
                                                <th class="keyField" rowspan="2">
                                                </th>
                                                <th rowspan="2" style="width: 120px;" align="left">
                                                    <%=GetLabel("Info Registrasi")%>
                                                </th>
                                                <th rowspan="2" style="width: 120px" align="left">
                                                    <%=GetLabel("No Transaksi")%>
                                                </th>
                                                <%--<th rowspan="2" style="width: 100px">
                                                    <%=GetLabel("Tanggal Transaksi")%>
                                                </th>--%>
                                                <th rowspan="2" align="left">
                                                    <%=GetLabel("Nama Pasien")%>
                                                </th>
                                                <th rowspan="2" style="width: 120px" align="left">
                                                    <%=GetLabel("Pembayar")%>
                                                </th>
                                                <th rowspan="2" style="width: 90px" align="right">
                                                    <%=GetLabel("Jumlah Transaksi (Bruto)")%>
                                                </th>
                                                <th rowspan="2" style="width: 90px" align="right">
                                                    <%=GetLabel("Kartu Kredit")%>
                                                </th>
                                                <th colspan="<%=GetFormulaTypeCount() %>" align="center">
                                                    <%=GetLabel("Komponen")%>
                                                </th>
                                                <th rowspan="2" style="width: 90px" align="right">
                                                    <%=GetLabel("Diskon") %>
                                                </th>
                                                <th rowspan="2" style="width: 90px" align="right">
                                                    <%=GetLabel("Dokter") %>
                                                </th>
                                                <th rowspan="2" style="width: 70px" align="left">
                                                    <%=GetLabel("Catatan") %>
                                                </th>
                                            </tr>
                                            <asp:Repeater ID="rptFormulaType" runat="server">
                                                <HeaderTemplate>
                                                    <tr>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <th>
                                                        <%#:Eval("StandardCodeName")%>
                                                    </th>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                </FooterTemplate>
                                            </asp:Repeater>
                                            <asp:ListView runat="server" ID="lvwView" OnItemDataBound="lvwView_ItemDataBound">
                                                <EmptyDataTemplate>
                                                    <tr class="trEmpty">
                                                        <td colspan="20">
                                                            <%=GetLabel("No Data To Display")%>
                                                        </td>
                                                    </tr>
                                                </EmptyDataTemplate>
                                                <ItemTemplate>
                                                    <tr>
                                                        <td width="80px" align="center">
                                                            <img class="imgEdit <%# IsEditable() == "0" ? "imgDisabled" : "imgLink"%>" src='<%# IsEditable() == "0" ? ResolveUrl("~/Libs/Images/Button/edit_disabled.png") : ResolveUrl("~/Libs/Images/Button/edit.png")%>'
                                                                alt="" style="float: left; margin-left: 7px" />
                                                            <img class="imgDelete <%# IsEditable() == "0" ? "imgDisabled" : "imgLink"%>" src='<%# IsEditable() == "0" ? ResolveUrl("~/Libs/Images/Button/delete_disabled.png") : ResolveUrl("~/Libs/Images/Button/delete.png")%>'
                                                                alt="" style="float: left; margin-left: 7px" />
                                                        </td>
                                                        <td class="keyField">
                                                            <%#: Eval("RSTransactionID")%>
                                                        </td>
                                                        <td>
                                                            <b>
                                                                <%#:Eval("RegistrationNo") %></b>
                                                            <br />
                                                            <label class="lblNormal" style="font-style: oblique; font-size: smaller">
                                                                <%#:Eval("DepartmentID") %></label>
                                                        </td>
                                                        <td>
                                                            <b>
                                                                <%#:Eval("TransactionNo")%></b>
                                                            <br />
                                                            <label class="lblNormal" style="font-style: oblique; font-size: smaller">
                                                                <%#:Eval("RevenueSharingCode") %></label>
                                                        </td>
                                                        <td>
                                                            <%#:Eval("PatientName") %>
                                                        </td>
                                                        <td>
                                                            <%#:Eval("BusinessPartnerName") %>
                                                        </td>
                                                        <td align="right">
                                                            <input type="hidden" bindingfield="TransactionDtID" value='<%#: Eval("TransactionDtID")%>' />
                                                            <input type="hidden" bindingfield="IsVariable" value='<%#: Eval("IsVariable")%>' />
                                                            <input type="hidden" bindingfield="TransactionAmount" value='<%#: Eval("TransactionAmount")%>' />
                                                            <input type="hidden" bindingfield="CCFee" value='<%#: Eval("CreditCardFeeAmount")%>' />
                                                            <input type="hidden" bindingfield="DiscountAmount" value='<%#: Eval("DiscountAmount")%>' />
                                                            <input type="hidden" bindingfield="RevenueSharingAmount" value='<%#: Eval("RevenueSharingAmount")%>' />
                                                            <%#:Eval("TransactionAmount", "{0:N2}") %>
                                                        </td>
                                                        <td align="right">
                                                            <%#:Eval("CreditCardFeeAmount", "{0:N2}") %>
                                                        </td>
                                                        <asp:Repeater ID="rptSharingComp" runat="server">
                                                            <ItemTemplate>
                                                                <td style="width: 100px" class="tdComponent" type='<%#:Eval("GCSharingComponent") %>'
                                                                    align="right">
                                                                    <%#:Eval("ComponentAmount", "{0:N2}") %>
                                                                    <input type="hidden" class="hdnViewComponent" value='<%#:Eval("ComponentAmount") %>' />
                                                                </td>
                                                            </ItemTemplate>
                                                        </asp:Repeater>
                                                        <td align="right" class="tdDiscountAmount">
                                                            <%#: Eval("DiscountAmount","{0:N2}")%>
                                                        </td>
                                                        <td align="right" class="tdRevenueSharing">
                                                            <%#: Eval("RevenueSharingAmount","{0:N2}")%>
                                                        </td>
                                                        <td>
                                                            <%#: Eval("Remarks")%>
                                                        </td>
                                                    </tr>
                                                </ItemTemplate>
                                            </asp:ListView>
                                        </table>
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
                </td>
            </tr>
        </table>
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
    <dxcp:ASPxCallbackPanel runat="server" ID="cbpProcess" ClientInstanceName="cbpProcess"
        OnCallback="cbpProcess_Callback">
        <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e) { onCbpProcessEndCallback(s); }" />
    </dxcp:ASPxCallbackPanel>
    <dxcp:ASPxCallbackPanel runat="server" ID="cbpProcessDiscount" ClientInstanceName="cbpProcessDiscount"
        OnCallback="cbpProcessDiscount_Callback">
        <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e) { onCbpProcessDiscountEndCallback(s); }" />
    </dxcp:ASPxCallbackPanel>
</asp:Content>
