<%@ Page Title="" Language="C#" MasterPageFile="~/libs/MasterPage/FixedAssetPage/MPBaseFixedAssetPageTrx.master"
    AutoEventWireup="true" CodeBehind="FAWriteOffEntry.aspx.cs" Inherits="QIS.Medinfras.Web.Accounting.Program.FAWriteOffEntry" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Src="~/Program/FixedAsset/FixedAssetToolbarCtl.ascx" TagName="ToolbarCtl"
    TagPrefix="uc1" %>
<asp:Content ID="Content3" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div class="menuTitle">
        <%=HttpUtility.HtmlEncode(GetPageTitle())%></div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="plhMPPatientPageNavigationPane"
    runat="server">
    <uc1:ToolbarCtl ID="ctlToolbar" runat="server" />
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript">
        function onLoad() {
            $('.txtCurrency').each(function () {
                $(this).trigger('changeValue');
            });

            if ($('#<%=txtFAWriteOffNo.ClientID %>').val() == "") {
            }

            function getFAWriteOffDateFilterExpression(value) {
                var date = value.split('-');
                var fixedAssetID = $('#<%=hdnFixedAssetID.ClientID %>').val();

                var writeOffDateInDatePicker = Methods.getDatePickerDate(value);
                var writeOffDateFormatString = Methods.dateToString(writeOffDateInDatePicker);
                var filterExpression = "DepreciationDate < '" + writeOffDateFormatString + "' AND FixedAssetID = " + fixedAssetID + " ORDER BY FADepreciationID DESC";								

                return filterExpression;
            }

            $('#<%=txtFAWriteOffDate.ClientID %>').change(function () {
                var inputDate = $('#<%=txtFAWriteOffDate.ClientID %>').val();
                if (inputDate != null && inputDate != "") {
                    Methods.getObject('GetvFADepreciationList', getFAWriteOffDateFilterExpression($(this).val()), function (result) {
                        if (result != null) {
                            var nilaiPemusnahan = result.AssetValue;
                            var selisih = (result.ProcurementAmount - nilaiPemusnahan);

                            $('#<%=txtProcurementAmount.ClientID %>').val(result.ProcurementAmount).trigger('changeValue');
                            $('#<%=txtTotalDepreciationAmount.ClientID %>').val(result.TotalDepreciationAmount).trigger('changeValue');
                            $('#<%=txtAssetValue.ClientID %>').val(result.AssetValue).trigger('changeValue');
                            $('#<%=txtWriteOffAmount.ClientID %>').val(nilaiPemusnahan).trigger('changeValue');
                            $('#<%=txtSelisih.ClientID %>').val(selisih).trigger('changeValue');
                        }
                    });
                } else {
                    var today = new Date();
                    var dd = String(today.getDate()).padStart(2, '0');
                    var mm = String(today.getMonth() + 1).padStart(2, '0');
                    var yyyy = today.getFullYear();

                    today = dd + '-' + mm + '-' + yyyy;

                    $('#<%=txtFAWriteOffDate.ClientID %>').val(today);
                }
            });
        }

        function onBeforeRightPanelPrint(code, filterExpression, errMessage) {
            var faWriteOffID = $('#<%=hdnFAWriteOffID.ClientID %>').val();
            var transaction = $('#<%=hdnGCTransactionStatus.ClientID %>').val();

            if (faWriteOffID == '') {
                errMessage.text = 'Please Save Transaction First!';
                return false;
            }
            else {
                if (code == 'AC-00006') {
                    if (transaction == Constant.TransactionStatus.APPROVED || transaction == Constant.TransactionStatus.CLOSED) {
                        filterExpression.text = "FAWriteOffID = " + faWriteOffID;
                        return true;
                    }
                    else {
                        errMessage.text = "Data Doesn't Approved or Closed";
                        return false;
                    }
                }
                else {
                    filterExpression.text = "FAWriteOffID = " + faWriteOffID;
                    return true;
                }
            }

        }
    </script>
    <input type="hidden" id="hdnPageTitle" runat="server" />
    <input type="hidden" id="hdnFAWriteOffID" runat="server" value="" />
    <input type="hidden" id="hdnFAWriteOffNo" runat="server" value="" />
    <input type="hidden" id="hdnFixedAssetID" runat="server" value="" />
    <input type="hidden" id="hdnGCTransactionStatus" runat="server" value="0" />
    <table class="tblContentArea">
        <colgroup>
            <col width="50%" />
            <col />
        </colgroup>
        <tr>
            <td>
                <table width="100%">
                    <colgroup>
                        <col style="width: 30%" />
                    </colgroup>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("No. Pemusnahan") %></label>
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtFAWriteOffNo" Width="200px" ReadOnly="true" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Tanggal Pemusnahan") %></label>
                        </td>
                        <td>
                            <asp:TextBox runat="server" CssClass="datepicker" ID="txtFAWriteOffDate" Width="150px" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Tipe Pemusnahan") %></label>
                        </td>
                        <td>
                            <dxe:ASPxComboBox runat="server" ID="cboAssetWriteOffType" Width="200px" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Cara Penjualan") %></label>
                        </td>
                        <td>
                            <dxe:ASPxComboBox runat="server" ID="cboAssetSalesType" Width="200px" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Nilai Perolehan") %></label>
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtProcurementAmount" Width="200px" CssClass="txtCurrency"
                                ReadOnly="true" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Total Akumulasi Penyusutan") %></label>
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtTotalDepreciationAmount" Width="200px" CssClass="txtCurrency"
                                ReadOnly="true" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Nilai Buku") %></label>
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtAssetValue" Width="200px" CssClass="txtCurrency"
                                ReadOnly="true" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Nilai Pemusnahan") %></label>
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtWriteOffAmount" Width="200px" CssClass="txtCurrency" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Selisih") %></label>
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtSelisih" Width="200px" CssClass="txtCurrency"
                                ReadOnly="true" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel" valign="top">
                            <label class="lblNormal">
                                <%=GetLabel("Keterangan") %></label>
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="txtRemarks" Width="200px" TextMode="MultiLine" Rows="2" />
                        </td>
                    </tr>
                </table>
            </td>
            <td valign="top">
                <div style="margin-left: 200px; padding: 5px; border-color: Red; border-style: solid;
                    border-width: 1px; display: none">
                    <font color="red">PERHATIAN !!</font><br />
                    Data pemusnahan tidak dapat dibatal posting.<br />
                    Harap pastikan data pemusnahan yang Anda masukkan sudah benar.
                </div>
            </td>
        </tr>
    </table>
</asp:Content>
