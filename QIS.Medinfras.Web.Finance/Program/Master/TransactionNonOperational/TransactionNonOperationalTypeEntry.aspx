<%@ Page Title="" Language="C#" MasterPageFile="~/libs/MasterPage/MPEntry.master"
    AutoEventWireup="true" CodeBehind="TransactionNonOperationalTypeEntry.aspx.cs" Inherits="QIS.Medinfras.Web.Finance.Program.TransactionNonOperationalTypeEntry" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<asp:Content ID="Content2" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div class="menuTitle">
        <%=HttpUtility.HtmlEncode(GetPageTitle())%></div>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript">
        function onLoad() {
            //#region Transaction Type
            function onTransactionTypeFilterExpression() {
                var filter = "TableName = 'ARInvoiceHd'";
                return filter;
            }

            $('#lblTransactionType.lblLink').live('click', function () {
                openSearchDialog('transactiontype', onTransactionTypeFilterExpression(), function (value) {
                    $('#<%=txtTransactionCode.ClientID %>').val(value);
                    ontxtTransactionCodeChanged(value);
                });
            });

            $('#<%=txtTransactionCode.ClientID %>').live('change', function () {
                var param = $('#<%=txtTransactionCode.ClientID %>').val();
                ontxtTransactionCodeChanged(param);
            });

            function ontxtTransactionCodeChanged(value) {
                var filterExpression = "TransactionCode = '" + value + "'";
                Methods.getObject('GetTransactionTypeList', filterExpression, function (result) {
                    if (result != null) {
                        $('#<%=hdnTransactionCode.ClientID %>').val(result.TransactionCode);
                        $('#<%=txtTransactionName.ClientID %>').val(result.TransactionName);

                    }
                    else {
                        $('#<%=hdnTransactionCode.ClientID %>').val('');
                        $('#<%=txtTransactionCode.ClientID %>').val('');
                        $('#<%=txtTransactionName.ClientID %>').val('');

                    }
                });
            }
            //#endregion

            //#region COA
            function onGetGLAccountFilterExpression() {
                var filterExpression = "IsHeader = 0 AND IsDeleted = 0";
                return filterExpression;
            }

            $('#lblCOA.lblLink').live('click', function () {
                openSearchDialog('chartofaccount', onGetGLAccountFilterExpression(), function (value) {
                    $('#<%=txtGLAccountNo.ClientID %>').val(value);
                    ontxtGLAccountCodeChanged(value);
                });
            });

            $('#<%=txtGLAccountNo.ClientID %>').live('change', function () {
                ontxtGLAccountCodeChanged($(this).val());
            });

            function ontxtGLAccountCodeChanged(value) {
                var filterExpression = onGetGLAccountFilterExpression() + " AND GLAccountNo = '" + value + "'";
                Methods.getObject('GetvChartOfAccountList', filterExpression, function (result) {
                    if (result != null) {
                        $('#<%=hdnGLAccountID.ClientID %>').val(result.GLAccountID);
                        $('#<%=txtGLAccountName.ClientID %>').val(result.GLAccountName);
                    }
                    else {
                        $('#<%=hdnGLAccountID.ClientID %>').val('');
                        $('#<%=txtGLAccountNo.ClientID %>').val('');
                        $('#<%=txtGLAccountName.ClientID %>').val('');
                    }
                });
            }
            //#endregion

            //#region COA Discount
            function onGetGLAccountDiscountFilterExpression() {
                var filterExpression = "IsHeader = 0 AND IsDeleted = 0";
                return filterExpression;
            }

            $('#lblCOADiscount.lblLink').live('click', function () {
                openSearchDialog('chartofaccount', onGetGLAccountDiscountFilterExpression(), function (value) {
                    $('#<%=txtGLAccountNoDiscount.ClientID %>').val(value);
                    ontxtGLAccountCodeDiscountChanged(value);
                });
            });

            $('#<%=txtGLAccountNoDiscount.ClientID %>').live('change', function () {
                ontxtGLAccountCodeDiscountChanged($(this).val());
            });

            function ontxtGLAccountCodeDiscountChanged(value) {
                var filterExpression = onGetGLAccountDiscountFilterExpression() + " AND GLAccountNo = '" + value + "'";
                Methods.getObject('GetvChartOfAccountList', filterExpression, function (result) {
                    if (result != null) {
                        $('#<%=hdnGLAccountIDDiscount.ClientID %>').val(result.GLAccountID);
                        $('#<%=txtGLAccountNameDiscount.ClientID %>').val(result.GLAccountName);
                    }
                    else {
                        $('#<%=hdnGLAccountIDDiscount.ClientID %>').val('');
                        $('#<%=txtGLAccountNoDiscount.ClientID %>').val('');
                        $('#<%=txtGLAccountNameDiscount.ClientID %>').val('');
                    }
                });
            }
            //#endregion

            //#region COA Amortization
            function onGetGLAccountAmortizationFilterExpression() {
                var filterExpression = "IsHeader = 0 AND IsDeleted = 0";
                return filterExpression;
            }

            $('#lblCOAAmortization.lblLink').live('click', function () {
                openSearchDialog('chartofaccount', onGetGLAccountAmortizationFilterExpression(), function (value) {
                    $('#<%=txtGLAccountNoAmortization.ClientID %>').val(value);
                    ontxtGLAccountCodeAmortizationChanged(value);
                });
            });

            $('#<%=txtGLAccountNoAmortization.ClientID %>').live('change', function () {
                ontxtGLAccountCodeAmortizationChanged($(this).val());
            });

            function ontxtGLAccountCodeAmortizationChanged(value) {
                var filterExpression = onGetGLAccountAmortizationFilterExpression() + " AND GLAccountNo = '" + value + "'";
                Methods.getObject('GetvChartOfAccountList', filterExpression, function (result) {
                    if (result != null) {
                        $('#<%=hdnGLAccountIDAmortization.ClientID %>').val(result.GLAccountID);
                        $('#<%=txtGLAccountNameAmortization.ClientID %>').val(result.GLAccountName);
                    }
                    else {
                        $('#<%=hdnGLAccountIDAmortization.ClientID %>').val('');
                        $('#<%=txtGLAccountNoAmortization.ClientID %>').val('');
                        $('#<%=txtGLAccountNameAmortization.ClientID %>').val('');
                    }
                });
            }
            //#endregion

            //#region Revenue Cost Center
            function onRevenueCostCenterFilterExpression() {
                var filter = "IsDeleted = 0";
                return filter;
            }
            
            $('#lblRevenueCostCenter.lblLink').live('click', function () {
                openSearchDialog('revenuecostcenter', onRevenueCostCenterFilterExpression(), function (value) {
                    $('#<%=txtRevenueCostCenterCode.ClientID %>').val(value);
                    ontxtRevenueCostCenterCodeChanged(value);
                });
            });

            $('#<%=txtRevenueCostCenterCode.ClientID %>').live('change', function () {
                var param = $('#<%=txtRevenueCostCenterCode.ClientID %>').val();
                ontxtRevenueCostCenterCodeChanged(param);
            });

            function ontxtRevenueCostCenterCodeChanged(value) {
                var filterExpression = "RevenueCostCenterCode = '" + $('#<%=txtRevenueCostCenterCode.ClientID %>').val() + "'";
                Methods.getObject('GetRevenueCostCenterList', filterExpression, function (result) {
                    if (result != null) {
                        $('#<%=hdnRevenueCostCenterID.ClientID %>').val(result.RevenueCostCenterID);
                        $('#<%=txtRevenueCostCenterName.ClientID %>').val(result.RevenueCostCenterName);

                    }
                    else {
                        $('#<%=hdnRevenueCostCenterID.ClientID %>').val('');
                        $('#<%=txtRevenueCostCenterCode.ClientID %>').val('');
                        $('#<%=txtRevenueCostCenterName.ClientID %>').val('');

                    }
                });
            }
            //#endregion

            //#region Business Partner
            function onGetBusinessPartnerFilterExpression() {
                var filterExpression = "IsDeleted = 0";
                return filterExpression;
            }

            $('#lblBusinessPartner.lblLink').live('click', function () {
                openSearchDialog('businesspartnersakun', onGetBusinessPartnerFilterExpression(), function (value) {
                    $('#<%=txtBusinessPartnerCode.ClientID %>').val(value);
                    ontxtBusinessPartnerCodeChanged(value);
                });
            });

            $('#<%=txtBusinessPartnerCode.ClientID %>').live('change', function () {
                var param = $('#<%=txtBusinessPartnerCode.ClientID %>').val();
                ontxtBusinessPartnerCodeChanged(param);
            });

            function ontxtBusinessPartnerCodeChanged(value) {
                var filterExpression = onGetBusinessPartnerFilterExpression() + " AND BusinessPartnerCode = '" + $('#<%=txtBusinessPartnerCode.ClientID %>').val() + "'";
                Methods.getObject('GetBusinessPartnersList', filterExpression, function (result) {
                    if (result != null) {
                        $('#<%=hdnBusinessPartnerID.ClientID %>').val(result.BusinessPartnerID);
                        $('#<%=txtBusinessPartnerName.ClientID %>').val(result.BusinessPartnerName);
                    }
                    else {
                        $('#<%=hdnBusinessPartnerID.ClientID %>').val('');
                        $('#<%=txtBusinessPartnerCode.ClientID %>').val('');
                        $('#<%=txtBusinessPartnerName.ClientID %>').val('');
                    }
                });
            }
            //#endregion
        }
    </script>
    <input type="hidden" id="hdnPageTitle" runat="server" />
    <input type="hidden" id="hdnID" runat="server" value="" />
    <input type="hidden" runat="server" id="hdnZipCode" value="" />
    <table class="tblContentArea">
        <colgroup>
            <col style="width: 50%" />
        </colgroup>
        <tr>
            <td style="padding: 5px; vertical-align: top">
                <h4 class="h4expanded">
                    <%=GetLabel("Master Tipe Transaksi Non Operasional")%></h4>
                <div class="containerTblEntryContent">
                    <table class="tblEntryContent" style="width: 100%">
                        <colgroup>
                            <col style="width: 30%" />
                        </colgroup>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblMandatory"><%=GetLabel("Kode Tipe Transaksi")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtTransactionNonOperationalTypeCode" Width="120px" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblMandatory"><%=GetLabel("Nama Tipe Transaksi")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtTransactionNonOperationalTypeName" Width="100%" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblLink lblMandatory" id="lblTransactionType">
                                    <%=GetLabel("Kode Transaksi")%></label>
                            </td>
                            <td>
                                <input type="hidden" id="hdnTransactionCode" runat="server" />
                                <table style="width: 100%" cellpadding="0" cellspacing="0">
                                    <colgroup>
                                        <col style="width: 30%" />
                                        <col style="width: 3px" />
                                        <col />
                                    </colgroup>
                                    <tr>
                                        <td>
                                            <asp:TextBox runat="server" ID="txtTransactionCode" Width="100%" />
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            <asp:TextBox runat="server" ID="txtTransactionName" Width="100%" ReadOnly="true" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblLink lblMandatory" id="lblCOA">
                                    <%=GetLabel("COA Pendapatan")%></label>
                            </td>
                            <td>
                                <input type="hidden" id="hdnGLAccountID" runat="server" />
                                <table style="width: 100%" cellpadding="0" cellspacing="0">
                                    <colgroup>
                                        <col style="width: 30%" />
                                        <col style="width: 3px" />
                                        <col />
                                    </colgroup>
                                    <tr>
                                        <td>
                                            <asp:TextBox runat="server" ID="txtGLAccountNo" Width="100%" />
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            <asp:TextBox runat="server" ID="txtGLAccountName" Width="100%" ReadOnly="true" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblLink" id="lblCOADiscount">
                                    <%=GetLabel("COA Diskon")%></label>
                            </td>
                            <td>
                                <input type="hidden" id="hdnGLAccountIDDiscount" runat="server" />
                                <table style="width: 100%" cellpadding="0" cellspacing="0">
                                    <colgroup>
                                        <col style="width: 30%" />
                                        <col style="width: 3px" />
                                        <col />
                                    </colgroup>
                                    <tr>
                                        <td>
                                            <asp:TextBox runat="server" ID="txtGLAccountNoDiscount" Width="100%" />
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            <asp:TextBox runat="server" ID="txtGLAccountNameDiscount" Width="100%" ReadOnly="true" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblLink" id="lblCOAAmortization">
                                    <%=GetLabel("COA Amortisasi")%></label>
                            </td>
                            <td>
                                <input type="hidden" id="hdnGLAccountIDAmortization" runat="server" />
                                <table style="width: 100%" cellpadding="0" cellspacing="0">
                                    <colgroup>
                                        <col style="width: 30%" />
                                        <col style="width: 3px" />
                                        <col />
                                    </colgroup>
                                    <tr>
                                        <td>
                                            <asp:TextBox runat="server" ID="txtGLAccountNoAmortization" Width="100%" />
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            <asp:TextBox runat="server" ID="txtGLAccountNameAmortization" Width="100%" ReadOnly="true" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblLink lblNormal" id="lblRevenueCostCenter">
                                    <%=GetLabel("Revenue Cost Center")%></label>
                            </td>
                            <td>
                                <input type="hidden" id="hdnRevenueCostCenterID" runat="server" />
                                <table style="width: 100%" cellpadding="0" cellspacing="0">
                                    <colgroup>
                                        <col style="width: 30%" />
                                        <col style="width: 3px" />
                                        <col />
                                    </colgroup>
                                    <tr>
                                        <td>
                                            <asp:TextBox runat="server" ID="txtRevenueCostCenterCode" Width="100%" />
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            <asp:TextBox runat="server" ID="txtRevenueCostCenterName" Width="100%" ReadOnly="true" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblLink lblNormal" id="lblBusinessPartner">
                                    <%=GetLabel("Business Partner")%></label>
                            </td>
                            <td>
                                <input type="hidden" id="hdnBusinessPartnerID" runat="server" />
                                <table style="width: 100%" cellpadding="0" cellspacing="0">
                                    <colgroup>
                                        <col style="width: 30%" />
                                        <col style="width: 3px" />
                                        <col />
                                    </colgroup>
                                    <tr>
                                        <td>
                                            <asp:TextBox runat="server" ID="txtBusinessPartnerCode" Width="100%" />
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            <asp:TextBox runat="server" ID="txtBusinessPartnerName" Width="100%" ReadOnly="true" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </div>
                <td style="padding: 5px; vertical-align: top">
                    <h4 class="h4expanded">
                        <%=GetLabel("Informasi Lainnya")%></h4>
                    <div class="containerTblEntryContent">
                        <table width="100%">
                            <colgroup>
                                <col width="150px" />
                            </colgroup>
                            <tr>
                                <td class="tdLabel" style="vertical-align: top; padding-top: 5px;">
                                    <label class="lblNormal">
                                        <%=GetLabel("Keterangan")%></label>
                                </td>
                                <td colspan="3">
                                    <asp:TextBox ID="txtRemarks" Width="100%" runat="server" TextMode="MultiLine" Rows="2" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                </td>
                                <td>
                                    <asp:CheckBox ID="chkIsUsedPPN" runat="server" /><%:GetLabel("Menggunakan PPN?")%>
                                </td>
                                <td>
                                    <asp:CheckBox ID="chkIsUsedPPH" runat="server" /><%:GetLabel("Menggunakan PPH?")%>
                                </td>
                            </tr>
                        </table>
                    </div>
                </td>
            </td>
        </tr>
    </table>
</asp:Content>
