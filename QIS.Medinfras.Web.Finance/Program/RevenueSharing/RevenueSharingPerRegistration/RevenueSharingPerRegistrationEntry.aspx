<%@ Page Title="" Language="C#" MasterPageFile="~/libs/MasterPage/MPTrx.master" AutoEventWireup="true"
    CodeBehind="RevenueSharingPerRegistrationEntry.aspx.cs" Inherits="QIS.Medinfras.Web.Finance.Program.RevenueSharingPerRegistrationEntry" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="QIS.Medinfras.Web.CustomControl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"
    Namespace="QIS.Medinfras.Web.CustomControl" TagPrefix="qis" %>
<asp:Content ID="Content2" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div class="menuTitle">
        <%=HttpUtility.HtmlEncode(GetPageTitle())%></div>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript" src='<%= ResolveUrl("~/Libs/Scripts/CustomGridViewList.js")%>'></script>
    <script type="text/javascript">
        $(function () {
            setDatePicker('<%=txtRegistrationDateFrom.ClientID %>');
            setDatePicker('<%=txtRegistrationDateTo.ClientID %>');

            setDatePicker('<%=txtLastPaymentDateFrom.ClientID %>');
            setDatePicker('<%=txtLastPaymentDateTo.ClientID %>');

            setDatePicker('<%=txtRevenueSharingDate.ClientID %>');

            $('#<%=txtRegistrationDateFrom.ClientID %>').datepicker('option', 'maxDate', '0');
            $('#<%=txtRegistrationDateTo.ClientID %>').datepicker('option', 'maxDate', '0');

            $('#<%=txtLastPaymentDateFrom.ClientID %>').datepicker('option', 'maxDate', new Date(getDateNow()));
            $('#<%=txtLastPaymentDateTo.ClientID %>').datepicker('option', 'maxDate', new Date(getDateNow()));

            $('#<%=txtRevenueSharingDate.ClientID %>').datepicker('option', 'maxDate', new Date(getDateNow()));

            var grd = new customGridView();
            grd.init('<%=grdView.ClientID %>', '<%=hdnRegistrationID.ClientID %>', '<%=pnlView.ClientID %>', cbpView, 'paging');

            $('#<%=grdView.ClientID %> > tbody > tr:gt(0):not(.trEmpty)').live('click', function () {
                if ($(this).attr('class') == 'selected') {
                    $('#<%=grdView.ClientID %> tr.selected').removeClass('selected');
                    $(this).addClass('selected');
                    $('#<%=hdnRegistrationID.ClientID %>').val($(this).find('.keyField').html());
                    cbpViewRegDt.PerformCallback('refreshRegDt');
                    cbpPatientCharges.PerformCallback('refreshPatientCharges');
                    cbpViewPatientPayment.PerformCallback('refreshPatientPayment');

                    $('.grdViewPatientCharges .chkIsSelected input').each(function () {
                        $(this).prop('checked', false);
                    });

                    $('#<%=txtTotalBrutoSelected.ClientID %>').val("0").trigger('changeValue');
                    $('#<%=txtTotalAmountSelected.ClientID %>').val("0").trigger('changeValue');

                    $('#containerHdGridAR').append($('#containerDtGridAR'));

                    $('.grdViewARInvoice tr:gt(0)').remove();
                }
            });

            $('#ulTabGrdDetail li').live('click', function () {
                $('#ulTabGrdDetail li.selected').removeAttr('class');
                $('.containerGrdDt').filter(':visible').hide();
                $contentID = $(this).attr('contentid');
                $('#' + $contentID).show();
                $(this).addClass('selected');
            });

            $('.grdViewPatientPayment td.tdExpand').live('click', function () {
                $tr = $(this).parent();
                $trDetail = $(this).parent().next();
                if ($trDetail.attr('class') != 'trDetail') {
                    $('#ulTabGrdDetail li:eq(0)').click();

                    $trCollapse = $('.trDetail');

                    $(this).find('.imgExpandAR').attr('src', '<%= ResolveUrl("~/Libs/Images/down-arrow.png")%>');
                    $newTr = $("<tr><td></td><td colspan='20'></td></tr>").attr('class', 'trDetail');
                    $newTr.insertAfter($tr);
                    $newTr.find('td').last().append($('#containerDtGridAR'));

                    if ($trCollapse != null) {
                        $trCollapse.prev().find('.imgExpandAR').attr('src', '<%= ResolveUrl("~/Libs/Images/right-arrow.png")%>');
                        $trCollapse.remove();
                    }

                    $('.grdViewARInvoice tr:gt(0)').remove();

                    $('#<%=hdnPaymentExpandID.ClientID %>').val($tr.find('.keyField').html().trim());

                    cbpViewARInvoice.PerformCallback('refreshARInvoice');
                }
                else {
                    $(this).find('.imgExpandAR').attr('src', '<%= ResolveUrl("~/Libs/Images/right-arrow.png")%>');
                    $('#containerHdGridAR').append($('#containerDtGridAR'));

                    $('.grdViewARInvoice tr:gt(0)').remove();

                    $trDetail.remove();
                }
            });

        });

        function onCboPaidTypeChanged(s) {
            if (cboPaidType.GetValue() == "2") {
                $('#<%=trLastPaymentDate.ClientID %>').attr('style', '');
                $('#<%=trRegistrationDate.ClientID %>').attr('style', 'display:none');
            } else {
                $('#<%=trLastPaymentDate.ClientID %>').attr('style', 'display:none');
                $('#<%=trRegistrationDate.ClientID %>').attr('style', '');
            }
        }

        //#region Business Partners
        function onGetPayerFilterExpression() {
            var filterExpression = "IsDeleted = 0";
            return filterExpression;
        }

        $('#lblPayer.lblLink').live('click', function () {
            openSearchDialog('payer', onGetPayerFilterExpression(), function (value) {
                $('#<%=txtPayerCode.ClientID %>').val(value);
                onTxtPayerCodeChanged(value);
            });
        });

        $('#<%=txtPayerCode.ClientID %>').live('change', function () {
            onTxtPayerCodeChanged($(this).val());
        });

        function onTxtPayerCodeChanged(value) {
            var filterExpression = onGetPayerFilterExpression() + " AND BusinessPartnerCode = '" + value + "'";
            Methods.getObject('GetBusinessPartnersList', filterExpression, function (result) {
                if (result != null) {
                    $('#<%=hdnPayerID.ClientID %>').val(result.BusinessPartnerID);
                    $('#<%=txtPayerCode.ClientID %>').val(result.BusinessPartnerCode);
                    $('#<%=txtPayerName.ClientID %>').val(result.BusinessPartnerName);
                }
                else {
                    $('#<%=txtPayerCode.ClientID %>').val('');
                    $('#<%=hdnPayerID.ClientID %>').val('');
                    $('#<%=txtPayerName.ClientID %>').val('');
                }
            });
        }
        //#endregion

        function onTxtSearchViewSearchClick(s) {
            setTimeout(function () {
                s.SetBlur();
                $('#<%=hdnFilterExpressionQuickSearch.ClientID %>').val(txtSearchView.GenerateFilterExpression());
                if (IsValid(null, 'fsPatientList', 'mpPatientList')) {
                    cbpView.PerformCallback('refresh');
                }
            }, 0);
        }

        $('#btnRefreshLeftPage').die('click');
        $('#btnRefreshLeftPage').live('click', function () {
            cbpView.PerformCallback('refresh');
        });

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
                if (pageCount > 0) {
                    $('#<%=grdView.ClientID %> tr:eq(1)').click();
                } else {
                    $('#<%=hdnRegistrationID.ClientID %>').val("0");
                    cbpViewRegDt.PerformCallback('refreshRegDt');
                    cbpPatientCharges.PerformCallback('refreshPatientCharges');
                    cbpViewPatientPayment.PerformCallback('refreshPatientPayment');
                }

                setPaging($("#paging"), pageCount, function (page) {
                    cbpView.PerformCallback('changepage|' + page);
                });
            }
            else {
                $('#<%=grdView.ClientID %> tr:eq(1)').click();
            }
        }
        //#endregion

        $('.lblRevenueSharingCode').die('click');
        $('.lblRevenueSharingCode').live('click', function () {
            $tr = $(this).closest('tr');
            var id = $tr.find('.ID').val();
            var dtID = $tr.find('.DtID').val();
            var dtIDSource = $tr.find('.DtIDSource').val();
            var paramedicID = $tr.find('.ParamedicID').val();
            var itemID = $tr.find('.ItemID').val();
            var param = id + "|" + paramedicID + "|" + itemID + "|" + dtIDSource + "|" + dtID;
            var url = ResolveUrl("~/Program/RevenueSharing/RevenueSharingProcess/ChargesEditRevenueSharingCtl.ascx");
            openUserControlPopup(url, param, 'Ubah Kode Jasa Medis', 800, 300);
        });

        $('.lblParamedicName').die('click');
        $('.lblParamedicName').live('click', function () {
            $tr = $(this).closest('tr');
            var id = $tr.find('.ID').val();
            var dtID = $tr.find('.DtID').val();
            var dtIDSource = $tr.find('.DtIDSource').val();
            var paramedicID = $tr.find('.ParamedicID').val();
            var param = id + "|" + paramedicID + "|" + dtIDSource + "|" + dtID;
            var url = ResolveUrl("~/Program/RevenueSharing/RevenueSharingProcess/ChargesEditParamedicIDCtl.ascx");
            openUserControlPopup(url, param, 'Ubah Dokter', 800, 300);
        });

        function onAfterSaveAddRecordEntryPopup() {
            cbpPatientCharges.PerformCallback('refreshPatientCharges');
        }

        function onAfterSaveEditRecordEntryPopup() {
            cbpPatientCharges.PerformCallback('refreshPatientCharges');
        }

        $('#btnProcess').die('click');
        $('#btnProcess').live('click', function () {
            $('#<%=hdnSelectedChargesDtID.ClientID %>').val("");
            $('#<%=hdnSelectedParamedicID.ClientID %>').val("");
            $('#<%=hdnSelectedMember.ClientID %>').val("");

            getCheckedMember();
            if ($('#<%=hdnSelectedMember.ClientID %>').val() == '')
                showToast('Warning', '<%=GetErrorMsgSelectTransactionFirst() %>');
            else
                cbpProcess.PerformCallback('save');
        });

        $('#chkCheckAll').live('click', function () {
            var isChecked = $(this).is(':checked');
            $('.grdViewPatientCharges .chkIsSelected input').each(function () {
                $(this).prop('checked', isChecked);
            });
            calculateTotal();
        });

        $('.grdViewPatientCharges .chkIsSelected input').live('click', function () {
            $('.chkCheckAll input').prop('checked', false);
            calculateTotal();
        });

        function calculateTotal() {
            var totalSelectedBruto = 0;
            var totalSelectedAmount = 0;

            $('.grdViewPatientCharges .chkIsSelected input:checked').each(function () {
                if ($(this).is(':checked')) {
                    $tr = $(this).closest('tr');
                    totalSelectedBruto += parseFloat($tr.find('.TransactionAmount').val());
                    totalSelectedAmount += parseFloat($tr.find('.SharingAmount').val());
                }
            });

            $('#<%=txtTotalBrutoSelected.ClientID %>').val(totalSelectedBruto).trigger('changeValue');
            $('#<%=txtTotalAmountSelected.ClientID %>').val(totalSelectedAmount).trigger('changeValue');
        }

        function getCheckedMember() {
            var lstSelectedChargesDtID = $('#<%=hdnSelectedChargesDtID.ClientID %>').val().split(',');
            var lstSelectedParamedicID = $('#<%=hdnSelectedParamedicID.ClientID %>').val().split(',');
            var lstSelectedMember = $('#<%=hdnSelectedMember.ClientID %>').val().split(',');
            $('.grdViewPatientCharges .chkIsSelected input').each(function () {
                if ($(this).is(':checked')) {
                    var oID = $(this).closest('tr').find('.ID').val();
                    var oDtID = $(this).closest('tr').find('.DtID').val();
                    var oParamedicID = $(this).closest('tr').find('.ParamedicID').val();
                    var oParam = oID + "|" + oParamedicID;
                    if (lstSelectedChargesDtID.indexOf(oDtID) < 0) {
                        lstSelectedChargesDtID.push(oID);
                        lstSelectedParamedicID.push(oParamedicID);
                        lstSelectedMember.push(oParam);
                    }
                }
                else {
                    var oID = $(this).closest('tr').find('.ID').val();
                    var oDtID = $(this).closest('tr').find('.DtID').val();
                    var oParamedicID = $(this).closest('tr').find('.ParamedicID').val();
                    var oParam = oID + "|" + oParamedicID;
                    if (lstSelectedChargesDtID.indexOf(oDtID) > -1) {
                        lstSelectedChargesDtID.splice(lstSelectedChargesDtID.indexOf(oID), 1);
                        lstSelectedParamedicID.splice(lstSelectedParamedicID.indexOf(oParamedicID), 1);
                        lstSelectedMember.splice(lstSelectedChargesDtID.indexOf(oParam), 1);
                    }
                }
            });

            $('#<%=hdnSelectedChargesDtID.ClientID %>').val(lstSelectedChargesDtID.join(','));
            $('#<%=hdnSelectedParamedicID.ClientID %>').val(lstSelectedParamedicID.join(','));
            $('#<%=hdnSelectedMember.ClientID %>').val(lstSelectedMember.join(','));
        }

        $('.imgDeletePRS').live('click', function () {
            var oID = $(this).closest('tr').find('.ID').val();
            var oDtID = $(this).closest('tr').find('.DtID').val();
            var oRSTransactionID = $(this).closest('tr').find('.RSTransactionID').val();
            var oRevenueSharingNo = $(this).closest('tr').find('.RevenueSharingNo').val();

            if (oRSTransactionID != null && oRSTransactionID != "0" && oRSTransactionID != "" && oID != null && oID != "0" && oID != "") {
                var msgConfirm = "Apakah yakin akan menghapus detail transaksi dari nomor transaksi JasMed <b>" + oRevenueSharingNo + "</b> ini ?";

                showToastConfirmation(msgConfirm, function (result) {
                    if (result) {
                        $('#<%=hdnRSTransactionIDForDelete.ClientID %>').val(oRSTransactionID);
                        $('#<%=hdnChargesDtIDForDelete.ClientID %>').val(oID);
                        $('#<%=hdnChargesDtPckgIDForDelete.ClientID %>').val(oDtID);

                        cbpProcess.PerformCallback('deletePRS');
                    }
                });
            } else {
                displayErrorMessageBox('GAGAL', "Tidak ada data transaksi jasmed yg terpilih.");
            }
        });

        function onCbpProcessEndCallback(s) {
            var param = s.cpResult.split('|');
            if (param[0] == 'fail')
                showToast('Process Failed', 'Error Message : ' + param[1]);
            else {
                cbpPatientCharges.PerformCallback('refreshPatientCharges');
            }

            $('#<%=hdnSelectedChargesDtID.ClientID %>').val("");
            $('#<%=hdnSelectedParamedicID.ClientID %>').val("");
            $('#<%=hdnSelectedMember.ClientID %>').val("");

            hideLoadingPanel();
        }

    </script>
    <input type="hidden" id="hdnPageTitle" runat="server" value="" />
    <input type="hidden" id="hdnRegistrationID" runat="server" value="" />
    <input type="hidden" id="hdnFilterExpressionQuickSearch" runat="server" value="" />
    <input type="hidden" id="hdnPaymentExpandID" runat="server" value="" />
    <input type="hidden" id="hdnSelectedMember" value="" runat="server" />
    <input type="hidden" id="hdnSelectedChargesDtID" value="" runat="server" />
    <input type="hidden" id="hdnSelectedParamedicID" value="" runat="server" />
    <input type="hidden" id="hdnRSTransactionIDForDelete" value="" runat="server" />
    <input type="hidden" id="hdnChargesDtIDForDelete" value="" runat="server" />
    <input type="hidden" id="hdnChargesDtPckgIDForDelete" value="" runat="server" />
    <div style="overflow-x: hidden;">
        <table class="tblContentArea">
            <tr>
                <td style="padding: 5px; vertical-align: top; width: 100%">
                    <table class="tblEntryContent" style="width: 100%">
                        <colgroup>
                            <col style="width: 30%" />
                            <col style="width: 70%" />
                        </colgroup>
                        <tr>
                            <td valign="top" align="left">
                                <table style="width: 100%">
                                    <colgroup>
                                        <col style="width: 110px" />
                                        <col style="width: 150px" />
                                        <col style="width: 15px" />
                                        <col style="width: 150px" />
                                        <col />
                                    </colgroup>
                                    <tr>
                                        <td class="tdLabel">
                                            <label class="lblNormal" style="font-size: smaller">
                                                <%=GetLabel("Paid Type")%></label>
                                        </td>
                                        <td colspan="3">
                                            <dxe:ASPxComboBox ID="cboPaidType" ClientInstanceName="cboPaidType" Width="100%"
                                                runat="server">
                                                <ClientSideEvents ValueChanged="function(s){ onCboPaidTypeChanged(s); }" />
                                            </dxe:ASPxComboBox>
                                        </td>
                                    </tr>
                                    <tr id="trRegistrationDate" runat="server" style="display: none">
                                        <td>
                                            <label class="lblNormal" style="font-size: smaller">
                                                <%=GetLabel("RegistrationDate") %></label>
                                        </td>
                                        <td>
                                            <asp:TextBox runat="server" Width="120px" ID="txtRegistrationDateFrom" CssClass="datepicker" />
                                        </td>
                                        <td style="text-align: center; font-size: small">
                                            <%=GetLabel("s/d") %>
                                        </td>
                                        <td>
                                            <asp:TextBox runat="server" Width="120px" ID="txtRegistrationDateTo" CssClass="datepicker" />
                                        </td>
                                    </tr>
                                    <tr id="trLastPaymentDate" runat="server">
                                        <td>
                                            <label class="lblNormal" style="font-size: smaller">
                                                <%=GetLabel("LastPaymentDate") %></label>
                                        </td>
                                        <td>
                                            <asp:TextBox runat="server" Width="120px" ID="txtLastPaymentDateFrom" CssClass="datepicker" />
                                        </td>
                                        <td style="text-align: center; font-size: small">
                                            <%=GetLabel("s/d") %>
                                        </td>
                                        <td>
                                            <asp:TextBox runat="server" Width="120px" ID="txtLastPaymentDateTo" CssClass="datepicker" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <label class="lblNormal" style="font-size: smaller">
                                                <%=GetLabel("Department") %></label>
                                        </td>
                                        <td colspan="3">
                                            <dxe:ASPxComboBox ID="cboDepartment" ClientInstanceName="cboDepartment" Width="50%"
                                                runat="server">
                                            </dxe:ASPxComboBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <label class="lblNormal lblLink" id="lblPayer" style="font-size: smaller">
                                                <%=GetLabel("Business Partner")%></label>
                                        </td>
                                        <td colspan="3">
                                            <table>
                                                <colgroup>
                                                    <col style="width: 100px" />
                                                    <col style="width: 250px" />
                                                    <col />
                                                </colgroup>
                                                <tr>
                                                    <td>
                                                        <input type="hidden" id="hdnPayerID" runat="server" value="" />
                                                        <asp:TextBox ID="txtPayerCode" Width="100%" runat="server" />
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtPayerName" ReadOnly="true" Width="100%" runat="server" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                        </td>
                                        <td>
                                            <asp:CheckBox runat="server" ID="chkIsFilterPayerExclusion" Text=" Is Exclusion?"
                                                Style="font-size: smaller" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <label style="font-size: smaller">
                                                <%=GetLabel("Quick Filter") %></label>
                                        </td>
                                        <td colspan="3">
                                            <qis:QISIntellisenseTextBox runat="server" ClientInstanceName="txtSearchView" ID="txtSearchView"
                                                Width="100%" Watermark="Search">
                                                <ClientSideEvents SearchClick="function(s){ onTxtSearchViewSearchClick(s); }" />
                                                <IntellisenseHints>
                                                    <qis:QISIntellisenseHint Text="Nama Pasien" FieldName="PatientName" />
                                                    <qis:QISIntellisenseHint Text="No RM" FieldName="MedicalNo" />
                                                    <qis:QISIntellisenseHint Text="No Registrasi" FieldName="RegistrationNo" />
                                                </IntellisenseHints>
                                            </qis:QISIntellisenseTextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                        </td>
                                        <td>
                                            <input type="button" id="btnRefreshLeftPage" value="R e f r e s h" class="btnRefresh w3-button w3-blue w3-border w3-border-blue w3-round-large" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="4">
                                            <hr style="padding: 0 0 0 0; margin: 0 0 0 0;" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="4">
                                            <div style="position: relative;" id="divView">
                                                <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
                                                    ShowLoadingPanel="false" OnCallback="cbpView_Callback">
                                                    <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ onCbpViewEndCallback(s); }" />
                                                    <PanelCollection>
                                                        <dx:PanelContent ID="PanelContent1" runat="server">
                                                            <asp:Panel runat="server" ID="pnlView" CssClass="pnlContainerGridProcessList" Style="height: 400px;
                                                                overflow-y: auto">
                                                                <asp:GridView ID="grdView" runat="server" CssClass="grdSelected" AutoGenerateColumns="false"
                                                                    ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                                                    <Columns>
                                                                        <asp:BoundField DataField="RegistrationID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                                                        <asp:TemplateField HeaderStyle-Width="150px" ItemStyle-HorizontalAlign="Left" HeaderStyle-HorizontalAlign="Left">
                                                                            <HeaderTemplate>
                                                                                <%=GetLabel("Data Registrasi") %>
                                                                            </HeaderTemplate>
                                                                            <ItemTemplate>
                                                                                <label style="font-size: large; font-weight: bolder">
                                                                                    <%#:Eval("RegistrationNo") %></label>
                                                                                <label style="font-size: x-small; font-weight: bolder; font-style: italic">
                                                                                    <%#:Eval("LinkedRegistrationNo") %></label>
                                                                                <br />
                                                                                <label style="font-size: smaller">
                                                                                    (<%#:Eval("MedicalNo") %>)</label>
                                                                                <label style="font-size: medium;">
                                                                                    <%#:Eval("PatientName") %></label>
                                                                                <br />
                                                                                <label style="font-size: smaller">
                                                                                    <i>
                                                                                        <%=GetLabel("Status Registrasi : ") %></i><%#:Eval("RegistrationStatus") %></label>
                                                                                <br />
                                                                                <label style='<%# Eval("CountPRS").ToString() == "0" ? "display:none;": "font-size:smaller;" %>'>
                                                                                    <i>
                                                                                        <%=GetLabel("Jumlah Trans Jasmed (PRS) : ") %></i><%#:Eval("CountPRS") %></label>
                                                                            </ItemTemplate>
                                                                        </asp:TemplateField>
                                                                    </Columns>
                                                                    <EmptyDataTemplate>
                                                                        <%=GetLabel("---tidak ada data---")%>
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
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td valign="top" align="left">
                                <table style="width: 100%">
                                    <tr>
                                        <td valign="top" align="left">
                                            <dxcp:ASPxCallbackPanel ID="cbpViewRegDt" runat="server" Width="100%" Height="50%"
                                                ClientInstanceName="cbpViewRegDt" ShowLoadingPanel="false" OnCallback="cbpViewRegDt_Callback">
                                                <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ hideLoadingPanel(); }" />
                                                <PanelCollection>
                                                    <dx:PanelContent ID="PanelContent2" runat="server">
                                                        <asp:Panel runat="server" ID="pnlViewRegDt" CssClass="pnlContainerGridProcessList"
                                                            Style="height: auto">
                                                            <table style="width: 100%">
                                                                <colgroup>
                                                                    <col style="width: 65%" />
                                                                    <col style="width: 35%" />
                                                                </colgroup>
                                                                <tr>
                                                                    <td valign="top" align="left">
                                                                        <table style="width: 100%;">
                                                                            <colgroup>
                                                                                <col style="width: 170px" />
                                                                                <col />
                                                                            </colgroup>
                                                                            <tr>
                                                                                <td style="font-size: smaller; font-style: italic">
                                                                                    <%=GetLabel("No. Registrasi") %>
                                                                                </td>
                                                                                <td>
                                                                                    <label runat="server" id="lblRegistrationNo" style="font-weight: bold" />
                                                                                </td>
                                                                            </tr>
                                                                            <tr id="trLinkRegistration" runat="server">
                                                                                <td style="font-size: smaller; font-style: italic">
                                                                                    <%=GetLabel("No. Registrasi Link") %>
                                                                                </td>
                                                                                <td>
                                                                                    <label runat="server" id="lblLinkedRegistrationNo" />
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td style="font-size: smaller; font-style: italic">
                                                                                    <%=GetLabel("Pasien") %>
                                                                                </td>
                                                                                <td>
                                                                                    <label runat="server" id="lblPatient" style="font-weight: bold" />
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td style="font-size: smaller; font-style: italic">
                                                                                    <%=GetLabel("Tgl/Jam Registrasi s/d Pulang") %>
                                                                                </td>
                                                                                <td>
                                                                                    <label runat="server" id="lblRegistrationDischargeDate" />
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td style="font-size: smaller; font-style: italic">
                                                                                    <%=GetLabel("Unit Registrasi") %>
                                                                                </td>
                                                                                <td>
                                                                                    <label runat="server" id="lblServiceUnitName" />
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td style="font-size: smaller; font-style: italic">
                                                                                    <%=GetLabel("Penjamin Bayar") %>
                                                                                </td>
                                                                                <td>
                                                                                    <label runat="server" id="lblBusinessPartnerName" />
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                    <td valign="top" align="left">
                                                                        <table style="width: 100%">
                                                                            <colgroup>
                                                                                <col style="width: 140px" />
                                                                                <col />
                                                                            </colgroup>
                                                                            <tr>
                                                                                <td style="font-size: smaller; font-style: italic">
                                                                                    <%=GetLabel("Total Transaksi") %>
                                                                                </td>
                                                                                <td align="right">
                                                                                    <label runat="server" id="lblChargesAmount" />
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td style="font-size: smaller; font-style: italic">
                                                                                    <%=GetLabel("Total Pembayaran") %>
                                                                                </td>
                                                                                <td align="right">
                                                                                    <label runat="server" id="lblPaymentAmount" />
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td style="font-size: smaller; font-style: italic">
                                                                                    <%=GetLabel("Total Pengakuan Piutang") %>
                                                                                </td>
                                                                                <td align="right">
                                                                                    <label runat="server" id="lblARInProcessAmount" />
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td style="font-size: smaller; font-style: italic">
                                                                                    <%=GetLabel("Total Tagihan Piutang") %>
                                                                                </td>
                                                                                <td align="right">
                                                                                    <label runat="server" id="lblARInvoiceAmount" />
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td style="font-size: smaller; font-style: italic">
                                                                                    <%=GetLabel("Total Penerimaan Piutang") %>
                                                                                </td>
                                                                                <td align="right">
                                                                                    <label runat="server" id="lblARReceivingAmount" />
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </asp:Panel>
                                                    </dx:PanelContent>
                                                </PanelCollection>
                                            </dxcp:ASPxCallbackPanel>
                                        </td>
                                    </tr>
                                    <tr id="trLine0" runat="server">
                                        <td colspan="2">
                                            <hr style="padding: 0 0 0 0; margin: 0 0 0 0;" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <table>
                                                <colgroup>
                                                    <col style="width: 65%" />
                                                    <col style="width: 35%" />
                                                </colgroup>
                                                <tr>
                                                    <td valign="top">
                                                        <table style="width: 100%">
                                                            <colgroup>
                                                                <col style="width: 200px" />
                                                                <col style="width: 100px" />
                                                                <col style="width: 350px" />
                                                                <col />
                                                            </colgroup>
                                                            <tr>
                                                                <td style="font-size: smaller; font-style: italic">
                                                                    <%=GetLabel("Tanggal Proses Jasa Medis") %>
                                                                </td>
                                                                <td colspan="2">
                                                                    <asp:TextBox runat="server" Width="120px" ID="txtRevenueSharingDate" CssClass="datepicker" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td style="font-size: smaller; font-style: italic">
                                                                    <%=GetLabel("Alokasi Pajak/Cara Bayar") %>
                                                                </td>
                                                                <td colspan="3">
                                                                    <table width="100%" cellspacing="0" cellpadding="0">
                                                                        <tr>
                                                                            <td>
                                                                                <dxe:ASPxComboBox ID="cboReduction" ClientInstanceName="cboReduction" Width="100%"
                                                                                    runat="server" />
                                                                            </td>
                                                                            <td>
                                                                                <dxe:ASPxComboBox ID="cboPaymentMethod" ClientInstanceName="cboPaymentMethod" Width="100%"
                                                                                    runat="server" />
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td style="font-size: smaller; font-style: italic">
                                                                    <%=GetLabel("Grup Pelayanan") %>
                                                                </td>
                                                                <td>
                                                                    <asp:CheckBox runat="server" ID="chkIsFilterClinicGroup" Text=" Is Filter?" />
                                                                </td>
                                                                <td colspan="2">
                                                                    <dxe:ASPxComboBox ID="cboClinicGroup" ClientInstanceName="cboClinicGroup" Width="100%"
                                                                        runat="server" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                    <td valign="top">
                                                        <table>
                                                            <colgroup>
                                                                <col style="width: 150px" />
                                                                <col style="width: 200px" />
                                                                <col />
                                                            </colgroup>
                                                            <tr>
                                                                <td style="font-size: smaller; font-style: italic">
                                                                    <%=GetLabel("Total Nilai Bruto Dipilih") %>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox runat="server" Width="150px" ID="txtTotalBrutoSelected" CssClass="txtCurrency"
                                                                        ReadOnly="true" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td style="font-size: smaller; font-style: italic">
                                                                    <%=GetLabel("Total Nilai JasMed Dipilih") %>
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox runat="server" Width="150px" ID="txtTotalAmountSelected" CssClass="txtCurrency"
                                                                        ReadOnly="true" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                </td>
                                                                <td>
                                                                    <input type="button" id="btnProcess" value="P r o c e s s" class="btnProcess w3-button w3-orange w3-border w3-border-blue w3-round-large" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr id="trLine1" runat="server">
                                        <td colspan="2">
                                            <hr style="padding: 0 0 0 0; margin: 0 0 0 0;" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <dxcp:ASPxCallbackPanel ID="cbpPatientCharges" runat="server" Width="100%" ClientInstanceName="cbpPatientCharges"
                                                ShowLoadingPanel="false" OnCallback="cbpPatientCharges_Callback">
                                                <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ hideLoadingPanel(); }" />
                                                <PanelCollection>
                                                    <dx:PanelContent ID="PanelContent3" runat="server">
                                                        <asp:Panel runat="server" ID="Panel2" CssClass="pnlContainerGrid" Style="height: 300px;
                                                            overflow-y: auto">
                                                            <table class="grdViewPatientCharges grdSelected" cellspacing="0" width="100%" rules="all">
                                                                <tr>
                                                                    <th class="keyField">
                                                                    </th>
                                                                    <th align="center" style="width: 10px">
                                                                        <input type="checkbox" id="chkCheckAll" />
                                                                    </th>
                                                                    <th align="left" style="width: 120px; font-size: smaller">
                                                                        <%=GetLabel("No Registrasi") %>
                                                                    </th>
                                                                    <th align="left" style="font-size: smaller">
                                                                        <%=GetLabel("Info Transaksi") %>
                                                                    </th>
                                                                    <th align="right" style="width: 120px; font-size: smaller">
                                                                        <%=GetLabel("Nilai Bruto") %>
                                                                    </th>
                                                                    <th align="right" style="width: 120px; font-size: smaller">
                                                                        <%=GetLabel("Komponen") %>
                                                                    </th>
                                                                    <th align="right" style="width: 120px; font-size: smaller">
                                                                        <%=GetLabel("Nilai Jasa Medis") %>
                                                                    </th>
                                                                    <th align="center" style="width: 120px; font-size: smaller">
                                                                        <%=GetLabel("No JasMed") %>
                                                                    </th>
                                                                </tr>
                                                                <asp:ListView runat="server" ID="lvwPatientCharges">
                                                                    <EmptyDataTemplate>
                                                                        <tr class="trEmpty">
                                                                            <td colspan="20">
                                                                                <%=GetLabel("---tidak ada data---") %>
                                                                            </td>
                                                                        </tr>
                                                                    </EmptyDataTemplate>
                                                                    <ItemTemplate>
                                                                        <tr>
                                                                            <td class="keyField">
                                                                                <%#: Eval("ID")%>
                                                                                <input type="hidden" class="ID" id="ID" runat="server" value='<%#: Eval("ID")%>' />
                                                                                <input type="hidden" class="DtID" id="DtID" runat="server" value='<%#: Eval("DtID")%>' />
                                                                                <input type="hidden" class="DtIDSource" id="DtIDSource" runat="server" value='<%#: Eval("DtIDSource")%>' />
                                                                                <input type="hidden" class="ItemID" id="ItemID" runat="server" value='<%#: Eval("ItemID")%>' />
                                                                                <input type="hidden" class="ParamedicID" id="ParamedicID" runat="server" value='<%#: Eval("ParamedicID")%>' />
                                                                                <input type="hidden" class="TransactionAmount" id="TransactionAmount" runat="server"
                                                                                    value='<%#: Eval("TransactionAmount")%>' />
                                                                                <input type="hidden" class="SharingAmount" id="SharingAmount" runat="server" value='<%#: Eval("SharingAmount")%>' />
                                                                                <input type="hidden" class="RevenueSharingNo" id="RevenueSharingNo" runat="server"
                                                                                    value='<%#: Eval("RevenueSharingNo")%>' />
                                                                                <input type="hidden" class="RSTransactionID" id="RSTransactionID" runat="server"
                                                                                    value='<%#: Eval("RSTransactionID")%>' />
                                                                            </td>
                                                                            <td align="center">
                                                                                <div <%# Eval("RevenueSharingNo").ToString() == "" || Eval("RevenueSharingNo") == null ?  "" : "style='display:none'" %>>
                                                                                    <asp:CheckBox runat="server" ID="chkIsSelected" CssClass="chkIsSelected" />
                                                                                </div>
                                                                            </td>
                                                                            <td align="left">
                                                                                <div>
                                                                                    <label title="No Registrasi" style="font-size: small">
                                                                                        <%#: Eval("RegistrationNo")%></label></div>
                                                                            </td>
                                                                            <td align="left">
                                                                                <div>
                                                                                    <label title="No Transaksi" style="font-size: small; font-weight: bold">
                                                                                        <%#: Eval("TransactionNo")%></label></div>
                                                                                <div>
                                                                                    <label title="Tgl Transaksi" style="font-size: smaller">
                                                                                        <%#: Eval("cfTransactionDateInString")%></label>
                                                                                    <label title="Jam Transaksi" style="font-size: smaller">
                                                                                        <%#: Eval("TransactionTime")%></label></div>
                                                                                <div>
                                                                                    <label title="Kode Item" style="font-size: smaller; font-weight: bold">
                                                                                        (<%#: Eval("ItemCode")%>)</label>
                                                                                    <label title="Nama Item" style="font-size: small; font-weight: bold">
                                                                                        <%#: Eval("ItemName1")%></label></div>
                                                                                <div>
                                                                                    <label style="font-size: x-small">
                                                                                        <%=GetLabel("Jumlah = ") %></label>
                                                                                    <label title="Jumlah Transaksi" style="font-size: small">
                                                                                        <%#: Eval("ChargedQuantity")%></label></div>
                                                                                <div>
                                                                                    <label title="Kelas Tagih" style="font-size: small">
                                                                                        <%#: Eval("ChargeClassName")%></label></div>
                                                                                <div>
                                                                                    <label class="lblLink lblParamedicName" title="<%#:Eval("ParamedicCode") %>">
                                                                                        <%#: Eval("ParamedicName") %></label></div>
                                                                                <div>
                                                                                    <label class="lblLink lblRevenueSharingCode" title="<%#:Eval("RevenueSharingName") %>">
                                                                                        <%#: Eval("RevenueSharingCode") != "" ? Eval("RevenueSharingCode") : "Pilih JasMed" %></label></div>
                                                                            </td>
                                                                            <td align="right">
                                                                                <div>
                                                                                    <label title="Nilai Transaksi (Bruto)" style="font-size: small">
                                                                                        <%#: Eval("cfTransactionAmountInString")%></label></div>
                                                                            </td>
                                                                            <td align="right">
                                                                                <div>
                                                                                    <label style="font-size: x-small">
                                                                                        <%=GetLabel("Komp1 = ") %></label>
                                                                                    <label title="Komp1" style="font-size: small">
                                                                                        <%#: Eval("cfComponentSharingAmount1InString")%></label></div>
                                                                                <div>
                                                                                    <label style="font-size: x-small">
                                                                                        <%=GetLabel("Komp2 = ") %></label>
                                                                                    <label title="Komp1" style="font-size: small">
                                                                                        <%#: Eval("cfComponentSharingAmount2InString")%></label></div>
                                                                                <div>
                                                                                    <label style="font-size: x-small">
                                                                                        <%=GetLabel("Komp3 = ") %></label>
                                                                                    <label title="Komp1" style="font-size: small">
                                                                                        <%#: Eval("cfComponentSharingAmount3InString")%></label></div>
                                                                            </td>
                                                                            <td align="right">
                                                                                <div>
                                                                                    <label title="Nilai JasMed" style="font-size: small">
                                                                                        <%#: Eval("cfSharingAmountInString")%></label></div>
                                                                            </td>
                                                                            <td align="center">
                                                                                <div>
                                                                                    <label title="No JasMed" style="font-size: small; font-weight: bold">
                                                                                        <%#: Eval("RevenueSharingNo")%></label><br />
                                                                                    <label title="Status JasMed" style="font-size: x-small; font-style: italic">
                                                                                        <%#: Eval("PRSTransactionStatus")%></label>
                                                                                </div>
                                                                                <img class="imgDeletePRS <%#: Eval("PRSGCTransactionStatus").ToString() == "X121^001" ? "imgLink" : "imgDisabled"%>"
                                                                                    title='<%=GetLabel("Delete")%>' src='<%#: Eval("PRSGCTransactionStatus").ToString() == "X121^001" ? ResolveUrl("~/Libs/Images/Button/delete.png") : ""  %>'
                                                                                    style="padding-top: 10px" alt="" />
                                                                            </td>
                                                                        </tr>
                                                                    </ItemTemplate>
                                                                </asp:ListView>
                                                            </table>
                                                        </asp:Panel>
                                                    </dx:PanelContent>
                                                </PanelCollection>
                                            </dxcp:ASPxCallbackPanel>
                                        </td>
                                    </tr>
                                    <tr id="trLine2" runat="server">
                                        <td colspan="2">
                                            <hr style="padding: 0 0 0 0; margin: 0 0 0 0;" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <dxcp:ASPxCallbackPanel ID="cbpViewPatientPayment" runat="server" Width="100%" ClientInstanceName="cbpViewPatientPayment"
                                                ShowLoadingPanel="false" OnCallback="cbpViewPatientPayment_Callback">
                                                <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ hideLoadingPanel(); }" />
                                                <PanelCollection>
                                                    <dx:PanelContent ID="PanelContent4" runat="server">
                                                        <asp:Panel runat="server" ID="Panel3" CssClass="pnlContainerGrid" Style="height: 200px;
                                                            overflow-y: auto">
                                                            <table class="grdViewPatientPayment grdSelected" cellspacing="0" width="100%" rules="all">
                                                                <tr>
                                                                    <th class="keyField">
                                                                    </th>
                                                                    <th align="center" style="width: 30px">
                                                                    </th>
                                                                    <th align="left" style="font-size: smaller">
                                                                        <%=GetLabel("No Bayar") %>
                                                                    </th>
                                                                    <th align="center" style="width: 100px; font-size: smaller">
                                                                        <%=GetLabel("Status Bayar") %>
                                                                    </th>
                                                                    <th align="left" style="width: 120px; font-size: smaller">
                                                                        <%=GetLabel("Jenis Bayar") %>
                                                                    </th>
                                                                    <th align="right" style="width: 150px; font-size: smaller">
                                                                        <%=GetLabel("Nilai Bayar") %>
                                                                    </th>
                                                                    <th align="center" style="width: 150px; font-size: smaller">
                                                                        <%=GetLabel("Info Dibuat") %>
                                                                    </th>
                                                                    <th align="center" style="width: 150px; font-size: smaller">
                                                                        <%=GetLabel("Info Terakhir Diubah") %>
                                                                    </th>
                                                                </tr>
                                                                <asp:ListView runat="server" ID="lvwViewPatientPayment">
                                                                    <EmptyDataTemplate>
                                                                        <tr class="trEmpty">
                                                                            <td colspan="15">
                                                                                <%=GetLabel("---tidak ada data---") %>
                                                                            </td>
                                                                        </tr>
                                                                    </EmptyDataTemplate>
                                                                    <ItemTemplate>
                                                                        <tr>
                                                                            <td class="keyField">
                                                                                <%#: Eval("PaymentID")%>
                                                                            </td>
                                                                            <td align="center" class="tdExpand" valign="middle">
                                                                                <img class="imgExpandAR imgLink" src='<%= ResolveUrl("~/Libs/Images/right-arrow.png")%>'
                                                                                    alt='' <%# Eval("GCPaymentType").ToString() == "X034^003" || Eval("GCPaymentType").ToString() == "X034^004" ?  "" : "style='display:none'" %> />
                                                                            </td>
                                                                            <td align="left">
                                                                                <div>
                                                                                    <label title="No Bayar" style="font-size: small; font-weight: bold">
                                                                                        <%#: Eval("PaymentNo")%></label></div>
                                                                                <div>
                                                                                    <label title="Tgl Bayar" style="font-size: small">
                                                                                        <%#: Eval("PaymentDateTimeInString")%></label></div>
                                                                            </td>
                                                                            <td align="center">
                                                                                <div>
                                                                                    <label title="Status Bayar" style="font-size: small">
                                                                                        <%#: Eval("TransactionStatusWatermark")%></label></div>
                                                                            </td>
                                                                            <td align="left">
                                                                                <div>
                                                                                    <label title="Jenis Bayar" style="font-size: small">
                                                                                        <%#: Eval("PaymentType")%></label></div>
                                                                            </td>
                                                                            <td align="right">
                                                                                <div>
                                                                                    <label title="Nilai Bayar" style="font-size: small">
                                                                                        <%#: Eval("ReceiveAmountInString2")%></label></div>
                                                                            </td>
                                                                            <td align="center">
                                                                                <div>
                                                                                    <label title="Dibuat Oleh" style="font-size: small">
                                                                                        <%#: Eval("CreatedByUser")%></label></div>
                                                                                <div>
                                                                                    <label title="Dibuat Pada" style="font-size: small">
                                                                                        <%#: Eval("cfCreatedDateTimeInString")%></label></div>
                                                                            </td>
                                                                            <td align="center">
                                                                                <div>
                                                                                    <label title="Dibuat Oleh" style="font-size: small">
                                                                                        <%#: Eval("LastUpdatedByUser")%></label></div>
                                                                                <div>
                                                                                    <label title="Dibuat Pada" style="font-size: small">
                                                                                        <%#: Eval("cfLastUpdatedDateInString")%></label></div>
                                                                            </td>
                                                                        </tr>
                                                                    </ItemTemplate>
                                                                </asp:ListView>
                                                            </table>
                                                        </asp:Panel>
                                                    </dx:PanelContent>
                                                </PanelCollection>
                                            </dxcp:ASPxCallbackPanel>
                                        </td>
                                    </tr>
                                    <tr id="trLine3" runat="server">
                                        <td colspan="2">
                                            <hr style="padding: 0 0 0 0; margin: 0 0 0 0;" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
    <div id="containerHdGridAR" style="display: none">
        <div id="containerDtGridAR" class="borderBox" style="width: 100%; padding: 10px 5px;">
            <div class="containerUlTabPage">
                <ul class="ulTabPage" id="ulTabGrdDetail">
                    <li class="selected" contentid="containerARInvoice">
                        <%=GetLabel("AR Information") %></li>
                </ul>
            </div>
            <div style="position: relative;">
                <div id="containerARInvoice" class="containerGrdDt">
                    <dxcp:ASPxCallbackPanel ID="cbpViewARInvoice" runat="server" Width="100%" ClientInstanceName="cbpViewARInvoice"
                        ShowLoadingPanel="false" OnCallback="cbpViewARInvoice_Callback">
                        <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ hideLoadingPanel(); }" />
                        <PanelCollection>
                            <dx:PanelContent ID="PanelContent5" runat="server">
                                <asp:Panel runat="server" ID="Panel4" CssClass="pnlContainerGrid" Style="height: auto">
                                    <table class="grdViewARInvoice grdSelected" cellspacing="0" width="100%" rules="all">
                                        <tr>
                                            <th class="keyField">
                                            </th>
                                            <th align="left" style="width: 250px; font-size: smaller">
                                                <%=GetLabel("Info Invoice") %>
                                            </th>
                                            <th align="right" style="width: 150px; font-size: smaller">
                                                <%=GetLabel("Nilai Klaim") %>
                                            </th>
                                            <th align="right" style="width: 150px; font-size: smaller">
                                                <%=GetLabel("Nilai Bayar") %>
                                            </th>
                                            <th align="left" style="font-size: smaller">
                                                <%=GetLabel("Info Penerimaan Piutang") %>
                                            </th>
                                        </tr>
                                        <asp:ListView runat="server" ID="lvwARInvoice">
                                            <EmptyDataTemplate>
                                                <tr class="trEmpty">
                                                    <td colspan="15">
                                                        <%=GetLabel("---tidak ada data---") %>
                                                    </td>
                                                </tr>
                                            </EmptyDataTemplate>
                                            <ItemTemplate>
                                                <tr>
                                                    <td class="keyField">
                                                        <%#: Eval("ID")%>
                                                    </td>
                                                    <td align="left">
                                                        <div>
                                                            <label title="No Invoice" style="font-size: small; font-weight: bold">
                                                                <%#: Eval("ARInvoiceNo")%></label></div>
                                                        <div>
                                                            <label title="Tgl Invoice" style="font-size: small">
                                                                <%#: Eval("cfARInvoiceDateInString")%></label></div>
                                                        <div>
                                                            <label style="font-size: x-small">
                                                                <%=GetLabel("Status Invoice : ") %></label>
                                                            <label title="Status Invoice" style="font-size: small">
                                                                <%#: Eval("TransactionStatus")%></label></div>
                                                        <div>
                                                            <label title="Instansi" style="font-size: small; font-weight: bold">
                                                                <%#: Eval("BusinessPartnerName")%></label></div>
                                                    </td>
                                                    <td align="right">
                                                        <div>
                                                            <label title="Nilai Klaim" style="font-size: small">
                                                                <%#: Eval("cfARClaimedAmountInString")%></label></div>
                                                    </td>
                                                    <td align="right">
                                                        <div>
                                                            <label title="Nilai Bayar" style="font-size: small">
                                                                <%#: Eval("cfARPaymentAmountInString")%></label></div>
                                                    </td>
                                                    <td align="left">
                                                        <div>
                                                            <label title="Penerimaan Piutang" style="font-size: small">
                                                                <%#: Eval("ARReceivingNo")%></label></div>
                                                    </td>
                                                </tr>
                                            </ItemTemplate>
                                        </asp:ListView>
                                    </table>
                                </asp:Panel>
                            </dx:PanelContent>
                        </PanelCollection>
                    </dxcp:ASPxCallbackPanel>
                </div>
            </div>
        </div>
    </div>
    <dxcp:ASPxCallbackPanel runat="server" ID="cbpProcess" ClientInstanceName="cbpProcess"
        OnCallback="cbpProcess_Callback">
        <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e) { onCbpProcessEndCallback(s); }" />
    </dxcp:ASPxCallbackPanel>
</asp:Content>
