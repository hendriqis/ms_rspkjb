<%@ Page Language="C#" MasterPageFile="~/Libs/MasterPage/MPEntry.master" AutoEventWireup="true"
    CodeBehind="PromotionSchemeEntry.aspx.cs" Inherits="QIS.Medinfras.Web.Finance.Program.PromotionSchemeEntry" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<asp:Content ID="Content2" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div class="menuTitle">
        <%=HttpUtility.HtmlEncode(GetPageTitle())%></div>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript">
        function onLoad() {
            setDatePicker('<%=txtStartDate.ClientID %>');
            setDatePicker('<%=txtEndDate.ClientID %>');

            //#region CopyUser
            $('#lblCopyFromPromo.lblLink').click(function () {
                var filterExpression = "GETDATE() BETWEEN StartDate AND EndDate AND IsDeleted = 0";
                openSearchDialog('promotion', filterExpression, function (value) {
                    $('#<%=txtCopyPromotionSchemeCode.ClientID %>').val(value);
                    onTxtCopyPromotionSchemeCodeChanged(value);
                });
            });

            $('#<%=txtCopyPromotionSchemeCode.ClientID %>').change(function () {
                onTxtCopyPromotionSchemeCodeChanged($(this).val());
            });

            function onTxtCopyPromotionSchemeCodeChanged(value) {
                var filterExpression = "PromotionSchemeCode ='" + value + "' AND IsDeleted = 0";
                Methods.getObject('GetPromotionSchemeList', filterExpression, function (result) {
                    if (result != null) {
                        $('#<%=hdnCopyPromotionSchemeID.ClientID %>').val(result.PromotionSchemeID);
                        $('#<%=txtCopyPromotionSchemeCode.ClientID %>').val(result.PromotionSchemeCode);
                        $('#<%=txtCopyPromotionSchemeName.ClientID %>').val(result.PromotionSchemeName);
                    }
                    else {
                        $('#<%=hdnCopyPromotionSchemeID.ClientID %>').val('');
                        $('#<%=txtCopyPromotionSchemeCode.ClientID %>').val('');
                        $('#<%=txtCopyPromotionSchemeName.ClientID %>').val('');
                    }

                });
            }

        }

        $('#<%=txtDiscount.ClientID %>').live('change', function () {
            var value = $(this).val();
            var isPercentage = $('#<%:chkDiscountInPercentage.ClientID %>').is(':checked');
            value = checkMinusDecimalOK(value);
            if (isPercentage == true) {
                if (value > 100) {
                    value = 0;
                }
            }
            $(this).val(value).trigger('changeValue');
        });

        $('#<%=txtStartDate.ClientID %>').live('change', function () {
            var start = $('#<%=txtStartDate.ClientID %>').val();
            var end = $('#<%=txtEndDate.ClientID %>').val();

            $('#<%=txtStartDate.ClientID %>').val(validateDateFromTo(start, end));
        });

        $('#<%=txtEndDate.ClientID %>').live('change', function () {
            var start = $('#<%=txtStartDate.ClientID %>').val();
            var end = $('#<%=txtEndDate.ClientID %>').val();

            $('#<%=txtEndDate.ClientID %>').val(validateDateToFrom(start, end));
        });

        $('#<%=txtMinimumTransaction.ClientID %>').live('change', function () {
            var value = $(this).val();
            $(this).val(checkMinusDecimalOK(value)).trigger('changeValue');

            if ($(this).val() == 0) {
                $('#<%=chkService.ClientID %>').prop('checked', false);
                $('#<%=chkDrugs.ClientID %>').prop('checked', false);
                $('#<%=chkGeneralGoods.ClientID %>').prop('checked', false);
            }
        });

        $('#<%=txtMultiplyQtyRevenueSharing.ClientID %>').live('change', function () {
            var value = $(this).val();
            value = checkMinus(value);

            if (value == 0) {
                value = 1;
            }

            $(this).val(value).trigger('changeValue');
        });

        $('#<%:chkDiscountInPercentage.ClientID %>').live('change', function () {
            $('#<%=txtDiscount.ClientID %>').val(0).trigger('changeValue');
        });

        function onCboPromotionTypeValueChanged(s) {
            $('#<%=txtDiscount.ClientID %>').val(0).trigger('changeValue');
            if (s.GetValue() == 'X415^002') {
                $('#<%:trDiscount.ClientID %>').removeAttr('style');
                $('#<%:trMinAmount.ClientID %>').removeAttr('style');
                $('#<%:trMinFrom.ClientID %>').removeAttr('style');
//                $('#<%:trMultipleRevenueSharing.ClientID %>').attr('style', 'display:none');
//                $('#<%:trPromotionSummary.ClientID %>').removeAttr('style');                              
            }
            else {
                $('#<%:trDiscount.ClientID %>').attr('style', 'display:none');
                $('#<%:trMinAmount.ClientID %>').attr('style', 'display:none');
                $('#<%:trMinFrom.ClientID %>').attr('style', 'display:none');
//                $('#<%:trMultipleRevenueSharing.ClientID %>').removeAttr('style');
//                $('#<%:trPromotionSummary.ClientID %>').attr('style', 'display:none');                  
            }
        }

        $('#<%:chkService.ClientID %>').live('change', function () {
            var value = $('#<%=txtMinimumTransaction.ClientID %>').val();
            if ($(this).is(':checked')) {
                if (value == 0) {
                    showToast('Warning', 'Minimum Transaksi harus lebih besar dari nol.');
                    $('#<%:chkService.ClientID %>').prop('checked', false);
                }
            }
        });

        $('#<%:chkDrugs.ClientID %>').live('change', function () {
            var value = $('#<%=txtMinimumTransaction.ClientID %>').val();
            if ($(this).is(':checked')) {
                if (value == 0) {
                    showToast('Warning', 'Minimum Transaksi harus lebih besar dari nol.');
                    $('#<%:chkDrugs.ClientID %>').prop('checked', false);
                }
            }
        });

        $('#<%:chkGeneralGoods.ClientID %>').live('change', function () {
            var value = $('#<%=txtMinimumTransaction.ClientID %>').val();
            if ($(this).is(':checked')) {
                if (value == 0) {
                    showToast('Warning', 'Minimum Transaksi harus lebih besar dari nol.');
                    $('#<%:chkGeneralGoods.ClientID %>').prop('checked', false);
                }
            }
        });
    </script>
    <input type="hidden" id="hdnPageTitle" runat="server" />
    <input type="hidden" id="hdnID" runat="server" value="" />
    <table class="tblContentArea">
        <colgroup>
            <col style="width: 50%" />
        </colgroup>
        <tr>
            <td style="padding: 5px; vertical-align: top">
                <h4 style="width: 50%">
                    <%=GetLabel("Informasi Promo")%>
                </h4>
                <table class="tblEntryContent" style="width: 50%">
                    <colgroup>
                        <col style="width: 15%" />
                    </colgroup>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblMandatory">
                                <%=GetLabel("Kode Promo")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtPromotionSchemeCode" Width="100px" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblMandatory">
                                <%=GetLabel("Nama Promo")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtPromotionSchemeName" Width="300px" runat="server" />
                        </td>
                    </tr>
                    <tr style='display:none'>
                        <td class="tdLabel">
                            <label class="lblMandatory">
                                <%=GetLabel("Jenis Promo")%></label>
                        </td>
                        <td>
                            <dxe:ASPxComboBox ID="cboPromotionType" ClientInstanceName="cboPromotionType" Width="100%"
                                runat="server">
                                <ClientSideEvents ValueChanged="function(s,e){ onCboPromotionTypeValueChanged(s); }" />
                            </dxe:ASPxComboBox>
                        </td>
                    </tr>
                    <tr id="trDiscount" runat="server" style="display: none">
                        <td class="tdLabel">
                            <label class="lblMandatory">
                                <%=GetLabel("Diskon")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtDiscount" Width="300px" CssClass="number" runat="server" />
                            <asp:CheckBox ID="chkDiscountInPercentage" runat="server" /><%:GetLabel("%")%>
                        </td>
                    </tr>
                    <tr id="trMinAmount" runat="server" style="display: none">
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Minimum Transaksi")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtMinimumTransaction" Width="300px" CssClass="number" runat="server" />
                        </td>
                    </tr>
                    <tr id="trMinFrom" runat="server" style="display: none">
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Minimum Dari")%></label>
                        </td>
                        <td>
                            <asp:CheckBox ID="chkService" runat="server" /><%:GetLabel("Pelayanan")%>
                            <asp:CheckBox ID="chkDrugs" runat="server" /><%:GetLabel("Obat - Obatan (Alkes)")%>
                            <asp:CheckBox ID="chkGeneralGoods" runat="server" /><%:GetLabel("Barang Umum")%>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblMandatory">
                                <%=GetLabel("Tanggal Berlaku")%></label>
                        </td>
                        <td>
                            <table border="0" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td>
                                        <asp:TextBox ID="txtStartDate" Width="120px" runat="server" CssClass="datepicker" />
                                    </td>
                                    <td style="padding-left: 2px; padding-right: 2px">
                                        s/d
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtEndDate" Width="120px" runat="server" CssClass="datepicker" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr id="trMultipleRevenueSharing" runat="server" style="display: none">
                        <td>
                            <label class="lblMandatory">
                                <%=GetLabel("Mutiply Revenue Sharing")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtMultiplyQtyRevenueSharing" Width="120px" runat="server" CssClass="number" />
                        </td>
                    </tr>
                    <tr id="trPromotionSummary" runat="server" style="display: none">
                        <td class="tdLabel" style="vertical-align: top; padding-top: 5px;">
                            <label class="lblNormal">
                                <%=GetLabel("Ringkasan Promo")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtPromotionSummary" Width="100%" Height="100%" runat="server" TextMode="MultiLine" Rows="25" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel" style="vertical-align: top; padding-top: 5px;">
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
            <td style="padding: 5px; vertical-align: top">
                <div id="divCopyPromotion" runat="server" visible="false">
                    <h4 style="width: 50%">
                        <%=GetLabel("Copy Promo")%>
                    </h4>
                    <table class="tblEntryContent" style="width: 50%">
                        <colgroup>
                            <col style="width: 15%" />
                        </colgroup>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblLink" id="lblCopyFromPromo">
                                    <%=GetLabel("Copy Promo")%></label>
                            </td>
                            <td>
                                <input type="hidden" id="hdnCopyPromotionSchemeID" runat="server" value="" />
                                <table style="width: 100%" cellpadding="0" cellspacing="0">
                                    <colgroup>
                                        <col style="width: 25%" />
                                    </colgroup>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtCopyPromotionSchemeCode" Width="100%" runat="server" />
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtCopyPromotionSchemeName" Width="100%" runat="server" ReadOnly="true" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </div>
            </td>
        </tr>
    </table>
</asp:Content>
