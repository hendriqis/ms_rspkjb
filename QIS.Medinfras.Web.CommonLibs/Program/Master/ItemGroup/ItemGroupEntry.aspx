<%@ Page Language="C#" MasterPageFile="~/Libs/MasterPage/MPEntry.master" AutoEventWireup="true"
    CodeBehind="ItemGroupEntry.aspx.cs" Inherits="QIS.Medinfras.Web.CommonLibs.Program.ItemGroupEntry" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<asp:Content ID="Content2" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div class="menuTitle">
        <%=HttpUtility.HtmlEncode(GetPageTitle())%></div>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript">
        function onLoad() {

            //#region Billing Group
            $('#lblBillingGroup.lblLink').click(function () {
                openSearchDialog('billinggroup', 'IsDeleted = 0', function (value) {
                    $('#<%=txtBillingGroupCode.ClientID %>').val(value);
                    onBillingGroupCodeChanged(value);
                });
            });

            $('#<%=txtBillingGroupCode.ClientID %>').change(function () {
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
            $('#lblRevenueSharing.lblLink').click(function () {
                openSearchDialog('revenuesharing', 'IsDeleted = 0', function (value) {
                    $('#<%=txtRevenueSharingCode.ClientID %>').val(value);
                    onTxtRevenueSharingCodeChanged(value);
                });
            });

            $('#<%=txtRevenueSharingCode.ClientID %>').change(function () {
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
            $('#lblGLAccount1.lblLink').click(function () {
                openSearchDialog('chartofaccount', onGetGLAccountFilterExpression(), function (value) {
                    $('#<%=txtGLAccountCode1.ClientID %>').val(value);
                    ontxtGLAccountCode1Changed(value);
                });
            });

            $('#<%=txtGLAccountCode1.ClientID %>').change(function () {
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

            $('#<%=lblSubLedgerDt1.ClientID %>').click(function () {
                if ($('#<%=hdnSearchDialogTypeName1.ClientID %>').val() != '') {
                    openSearchDialog($('#<%=hdnSearchDialogTypeName1.ClientID %>').val(), onGetSubLedgerDt1FilterExpression(), function (value) {
                        $('#<%=txtSubLedgerDtCode1.ClientID %>').val(value);
                        ontxtSubLedgerDtCode1Changed(value);
                    });
                }
            });

            $('#<%=txtSubLedgerDtCode1.ClientID %>').change(function () {
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
            $('#lblGLAccount2.lblLink').click(function () {
                openSearchDialog('chartofaccount', onGetGLAccountFilterExpression(), function (value) {
                    $('#<%=txtGLAccountCode2.ClientID %>').val(value);
                    ontxtGLAccountCode2Changed(value);
                });
            });

            $('#<%=txtGLAccountCode2.ClientID %>').change(function () {
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

            $('#<%=lblSubLedgerDt2.ClientID %>').click(function () {
                if ($('#<%=hdnSearchDialogTypeName2.ClientID %>').val() != '') {
                    openSearchDialog($('#<%=hdnSearchDialogTypeName2.ClientID %>').val(), onGetSubLedgerDt2FilterExpression(), function (value) {
                        $('#<%=txtSubLedgerDtCode2.ClientID %>').val(value);
                        ontxtSubLedgerDtCode2Changed(value);
                    });
                }
            });

            $('#<%=txtSubLedgerDtCode2.ClientID %>').change(function () {
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
            $('#lblGLAccount3.lblLink').click(function () {
                openSearchDialog('chartofaccount', onGetGLAccountFilterExpression(), function (value) {
                    $('#<%=txtGLAccountCode3.ClientID %>').val(value);
                    ontxtGLAccountCode3Changed(value);
                });
            });

            $('#<%=txtGLAccountCode3.ClientID %>').change(function () {
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

            $('#<%=lblSubLedgerDt3.ClientID %>').click(function () {
                if ($('#<%=hdnSearchDialogTypeName3.ClientID %>').val() != '') {
                    openSearchDialog($('#<%=hdnSearchDialogTypeName3.ClientID %>').val(), onGetSubLedgerDt3FilterExpression(), function (value) {
                        $('#<%=txtSubLedgerDtCode3.ClientID %>').val(value);
                        ontxtSubLedgerDtCode3Changed(value);
                    });
                }
            });

            $('#<%=txtSubLedgerDtCode3.ClientID %>').change(function () {
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
            $('#lblGLAccountDiscount.lblLink').click(function () {
                openSearchDialog('chartofaccount', onGetGLAccountFilterExpression(), function (value) {
                    $('#<%=txtGLAccountDiscountCode.ClientID %>').val(value);
                    ontxtGLAccountDiscountCodeChanged(value);
                });
            });

            $('#<%=txtGLAccountDiscountCode.ClientID %>').change(function () {
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

            //#region Product Line
            $('#lblProductLine.lblLink').click(function () {
                openSearchDialog('productline', 'IsDeleted = 0', function (value) {
                    $('#<%=txtProductLineCode.ClientID %>').val(value);
                    onTxtProductLineCodeChanged(value);
                });
            });

            $('#<%=txtProductLineCode.ClientID %>').change(function () {
                onTxtProductLineCodeChanged($(this).val());
            });

            function onTxtProductLineCodeChanged(value) {
                var filterExpression = "ProductLineCode = '" + value + "'";
                Methods.getObject('GetProductLineList', filterExpression, function (result) {
                    if (result != null) {
                        $('#<%=hdnProductLineID.ClientID %>').val(result.ProductLineID);
                        $('#<%=txtProductLineName.ClientID %>').val(result.ProductLineName);
                    }
                    else {
                        $('#<%=hdnProductLineID.ClientID %>').val('');
                        $('#<%=txtProductLineCode.ClientID %>').val('');
                        $('#<%=txtProductLineName.ClientID %>').val('');
                    }
                });
            }
            //#endregion

        }

        function onGetGLAccountFilterExpression() {
            var filterExpression = "IsHeader = 0 AND IsDeleted = 0";
            return filterExpression;
        }
    </script>
    <input type="hidden" id="hdnPageTitle" runat="server" />
    <input type="hidden" id="hdnID" runat="server" value="" />
    <input type="hidden" id="hdnGCItemType" runat="server" value="" />
    <input type="hidden" value="" id="hdnGCSubItemType" runat="server" />
    <input type="hidden" id="hdnItemGroup" runat="server" value="" />
    <input type="hidden" id="hdnSubMenuType" runat="server" value="" />
    <table class="tblContentArea">
        <colgroup>
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
                            <label class="lblMandatory">
                                <%=GetLabel("Kode Kelompok")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtItemGroupCode" Width="100px" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblMandatory">
                                <%=GetLabel("Nama Kelompok #1")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtItemGroupName1" Width="100%" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Nama Kelompok #2")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtItemGroupName2" Width="100%" runat="server" />
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
                                        <asp:TextBox ID="txtBillingGroupName" Width="100%" runat="server" />
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
                                        <asp:TextBox ID="txtRevenueSharingName" Width="100%" runat="server" />
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
                            <asp:TextBox ID="txtPrintOrder" CssClass="number" Width="100px" runat="server" />&nbsp;
                            <asp:CheckBox ID="chkIsHeader" runat="server" /><%=GetLabel("Induk bagi Kelompok Lain")%>
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
                </table>
            </td>
            <td style="padding: 5px; vertical-align: top">
                <table class="tblEntryContent" style="width: 100%">
                    <colgroup>
                        <col style="width: 35%" />
                        <col style="width: 50px" />
                        <col />
                    </colgroup>
                    <tr id="trCITO" runat="server" style="display: none">
                        <td class="tdLabel">
                            <label class="lblMandatory">
                                <%=GetLabel("Nilai CITO")%></label>
                        </td>
                        <td>
                            <asp:CheckBox ID="chkIsCITOInPercentage" runat="server" />
                            %
                        </td>
                        <td>
                            <asp:TextBox ID="txtCITOAmount" CssClass="txtCurrency" Width="100px" runat="server" />
                        </td>
                    </tr>
                    <tr id="trComplicationValue" runat="server" style="display: none">
                        <td class="tdLabel">
                            <label class="lblMandatory">
                                <%=GetLabel("Nilai Penyulit")%></label>
                        </td>
                        <td>
                            <asp:CheckBox ID="chkIsComplicationInPercentage" runat="server" />
                            %
                        </td>
                        <td>
                            <asp:TextBox ID="txtComplicationAmount" CssClass="txtCurrency" Width="100px" runat="server" />
                        </td>
                    </tr>
                    <tr id="trIsPrintWithDoctorName" runat="server" style="display: none">
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Print Nama Dokter")%></label>
                        </td>
                        <td>
                            <asp:CheckBox ID="chkIsPrintWithDoctorName" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3">
                            <hr />
                        </td>
                    </tr>
                    <tr id="trDiscount" runat="server">
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Diskon Terhitung dalam HNA")%></label>
                        </td>
                        <td>
                            <asp:CheckBox ID="chkIsDiscountCalculateHNA" Width="100%" runat="server" />
                        </td>
                    </tr>
                    <tr id="trPPN" runat="server">
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("PPN Terhitung dalam HNA")%></label>
                        </td>
                        <td>
                            <asp:CheckBox ID="chkIsPPNCalculateHNA" Width="100%" runat="server" />
                        </td>
                    </tr>
                    <tr id="tr1" runat="server">
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Perhitungan Harga Jual BPJS")%></label>
                        </td>
                        <td>
                            <asp:CheckBox ID="chkIsBPJS" Width="100%" runat="server" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <hr />
            </td>
        </tr>
        <tr id="trProductLine" runat="server">
            <td style="padding: 5px; vertical-align: top">
                <table class="tblEntryContent" style="width: 100%">
                    <colgroup>
                        <col style="width: 30%" />
                        <col />
                    </colgroup>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblLink lblMandatory" id="lblProductLine">
                                <%=GetLabel("Product Line")%></label>
                        </td>
                        <td>
                            <input type="hidden" id="hdnProductLineID" value="" runat="server" />
                            <table style="width: 100%" cellpadding="0" cellspacing="0">
                                <colgroup>
                                    <col style="width: 100px" />
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
                                        <asp:TextBox ID="txtProductLineName" Width="100%" ReadOnly="true" runat="server" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td style="padding: 5px; vertical-align: top">
                <table class="tblEntryContent" style="width: 100%">
                    <colgroup>
                        <col style="width: 30%" />
                        <col />
                    </colgroup>
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
                                    <col style="width: 100px" />
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
                                    <col style="width: 100px" />
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
                                    <col style="width: 100px" />
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
                            <label class="lblLink" id="lblGLAccountDiscount">
                                <%=GetLabel("COA Diskon")%></label>
                        </td>
                        <td>
                            <input type="hidden" id="hdnGLAccountDiscountID" runat="server" />
                            <table style="width: 100%" cellpadding="0" cellspacing="0">
                                <colgroup>
                                    <col style="width: 100px" />
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
                    <tr style="display: none;">
                        <td class="tdLabel">
                            <label class="lblLink" runat="server" id="lblSubLedgerDt1">
                                <%=GetLabel("Sub Perkiraan 1")%></label>
                        </td>
                        <td>
                            <input type="hidden" id="hdnSubLedgerDtID1" runat="server" />
                            <table style="width: 100%" cellpadding="0" cellspacing="0">
                                <colgroup>
                                    <col style="width: 100px" />
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
                    <tr style="display: none;">
                        <td class="tdLabel">
                            <label class="lblLink" runat="server" id="lblSubLedgerDt2">
                                <%=GetLabel("Sub Perkiraan 2")%></label>
                        </td>
                        <td>
                            <input type="hidden" id="hdnSubLedgerDtID2" runat="server" />
                            <table style="width: 100%" cellpadding="0" cellspacing="0">
                                <colgroup>
                                    <col style="width: 100px" />
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
                    <tr style="display: none;">
                        <td class="tdLabel">
                            <label class="lblLink" runat="server" id="lblSubLedgerDt3">
                                <%=GetLabel("Sub Perkiraan 3")%></label>
                        </td>
                        <td>
                            <input type="hidden" id="hdnSubLedgerDtID3" runat="server" />
                            <table style="width: 100%" cellpadding="0" cellspacing="0">
                                <colgroup>
                                    <col style="width: 100px" />
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
                </table>
            </td>
        </tr>
    </table>
</asp:Content>
