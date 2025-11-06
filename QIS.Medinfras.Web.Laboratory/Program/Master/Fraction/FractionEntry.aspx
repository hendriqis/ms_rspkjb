<%@ Page Language="C#" MasterPageFile="~/Libs/MasterPage/MPEntry.master" AutoEventWireup="true"
    CodeBehind="FractionEntry.aspx.cs" Inherits="QIS.Medinfras.Web.Laboratory.Program.FractionEntry" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<asp:Content ID="Content2" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div class="menuTitle">
        <%=HttpUtility.HtmlEncode(GetPageTitle())%></div>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript">
        function onLoad() {

            $('#<%=txtDisplayColorPicker.ClientID %>').colorPicker();
            $('#<%=txtDisplayColorPicker.ClientID %>').change(function () {
                $('#<%=txtDisplayColor.ClientID %>').val($(this).val());
            });

            $('#<%=txtDisplayColor.ClientID %>').change(function () {
                $('#<%=txtDisplayColorPicker.ClientID %>').val($(this).val());
                $('#<%=txtDisplayColorPicker.ClientID %>').change();
            });

            //#region Parent
            $('#lblParentID.lblLink').click(function () {
                openSearchDialog('fraction', 'IsHeader = 1 AND IsDeleted = 0', function (value) {
                    $('#<%=txtParentCode.ClientID %>').val(value);
                    onTxtParentCodeChanged(value);
                });
            });

            $('#<%=txtParentCode.ClientID %>').change(function () {
                onTxtParentCodeChanged($(this).val());
            });

            function onTxtParentCodeChanged(value) {
                var filterExpression = "FractionCode = '" + value + "' AND IsHeader = 1 AND IsDeleted = 0";
                Methods.getObject('GetFractionList', filterExpression, function (result) {
                    if (result != null) {
                        $('#<%=hdnParentID.ClientID %>').val(result.FractionID);
                        $('#<%=txtParentName.ClientID %>').val(result.FractionName1);
                    }
                    else {
                        $('#<%=hdnParentID.ClientID %>').val('');
                        $('#<%=txtParentCode.ClientID %>').val('');
                        $('#<%=txtParentName.ClientID %>').val('');
                    }
                });
            }
            //#endregion

            //#region FractionGroup
            $('#lblFractionGroup.lblLink').click(function () {
                openSearchDialog('fractiongroup', 'IsDeleted = 0', function (value) {
                    $('#<%=txtFractionGroupCode.ClientID %>').val(value);
                    onTxtFractionGroupCodeChanged(value);
                });
            });

            $('#<%=txtFractionGroupCode.ClientID %>').change(function () {
                onTxtFractionGroupCodeChanged($(this).val());
            });

            function onTxtFractionGroupCodeChanged(value) {
                var filterExpression = "FractionGroupCode = '" + value + "' AND IsDeleted = 0";
                Methods.getObject('GetFractionGroupList', filterExpression, function (result) {
                    if (result != null) {
                        $('#<%=hdnFractionGroupID.ClientID %>').val(result.FractionGroupID);
                        $('#<%=txtFractionGroupName.ClientID %>').val(result.FractionGroupName);
                    }
                    else {
                        $('#<%=hdnFractionGroupID.ClientID %>').val('');
                        $('#<%=txtFractionGroupCode.ClientID %>').val('');
                        $('#<%=txtFractionGroupName.ClientID %>').val('');
                    }
                });
            }
            //#endregion

            //#region Specimen
            $('#lblSpecimen.lblLink').click(function () {
                openSearchDialog('specimen', 'IsDeleted = 0', function (value) {
                    $('#<%=txtSpecimenCode.ClientID %>').val(value);
                    onTxtSpecimenCodeChanged(value);
                });
            });

            $('#<%=txtSpecimenCode.ClientID %>').change(function () {
                onTxtSpecimenCodeChanged($(this).val());
            });

            function onTxtSpecimenCodeChanged(value) {
                var filterExpression = "SpecimenCode = '" + value + "' AND IsDeleted = 0";
                Methods.getObject('GetSpecimenList', filterExpression, function (result) {
                    if (result != null) {
                        $('#<%=hdnSpecimenID.ClientID %>').val(result.SpecimenID);
                        $('#<%=txtSpecimenName.ClientID %>').val(result.SpecimenName);
                    }
                    else {
                        $('#<%=hdnSpecimenID.ClientID %>').val('');
                        $('#<%=txtSpecimenCode.ClientID %>').val('');
                        $('#<%=txtSpecimenName.ClientID %>').val('');
                    }
                });
            }
            //#endregion
        }

        function onCboLabTestResultTypeChanged() {
            var testResult = cboLabTestResultType.GetValue();

            if (testResult == "X002^001") {
                $('#<%:txtDecmalDigits.ClientID %>').attr('readonly', 'readonly');
                $('#<%:chkIsDisplay.ClientID %>').attr('disabled', true);
                $('#<%:txtDisplayColor.ClientID %>').attr('readonly', 'readonly');
                $('#<%:txtDisplayColorPicker.ClientID %>').attr('disabled', true);
                $('#<%:txtChartMinValue.ClientID %>').attr('readonly', 'readonly');
                $('#<%:txtChartMaxValue.ClientID %>').attr('readonly', 'readonly');
            } else if (testResult == "X002^002") {
                $('#<%:txtDecmalDigits.ClientID %>').removeAttr('readonly');
                $('#<%:chkIsDisplay.ClientID %>').removeAttr('disabled');
                $('#<%:txtDisplayColor.ClientID %>').removeAttr('readonly');
                $('#<%:txtDisplayColorPicker.ClientID %>').removeAttr('disabled');
                $('#<%:txtChartMinValue.ClientID %>').removeAttr('readonly');
                $('#<%:txtChartMaxValue.ClientID %>').removeAttr('readonly');
            }
        }
    </script>
    <input type="hidden" id="hdnPageTitle" runat="server" />
    <input type="hidden" id="hdnFractionID" runat="server" value="" />
    <table class="tblContentArea">
        <colgroup>
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
                            <label class="lblMandatory">
                                <%=GetLabel("Kode Artikel")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtFractionCode" Width="100px" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td colspan="2">
                            <asp:CheckBox ID="chkIsHeader" Width="100px" runat="server" Text=" Kode Induk" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblMandatory">
                                <%=GetLabel("Nama Artikel 1")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtFractionName1" Width="100%" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Nama Artikel 2")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtFractionName2" Width="100%" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblLink" id="lblParentID">
                                <%=GetLabel("Kode Induk")%></label>
                        </td>
                        <td>
                            <input type="hidden" value="" runat="server" id="hdnParentID" />
                            <table style="width: 100%" cellpadding="0" cellspacing="0">
                                <colgroup>
                                    <col style="width: 100px" />
                                    <col style="width: 3px" />
                                    <col />
                                </colgroup>
                                <tr>
                                    <td>
                                        <asp:TextBox ID="txtParentCode" Width="100%" runat="server" />
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtParentName" Width="100%" runat="server" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblLink" id="lblFractionGroup">
                                <%=GetLabel("Kelompok Artikel")%></label>
                        </td>
                        <td>
                            <input type="hidden" value="" runat="server" id="hdnFractionGroupID" />
                            <table style="width: 100%" cellpadding="0" cellspacing="0">
                                <colgroup>
                                    <col style="width: 100px" />
                                    <col style="width: 3px" />
                                    <col />
                                </colgroup>
                                <tr>
                                    <td>
                                        <asp:TextBox ID="txtFractionGroupCode" Width="100%" runat="server" />
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtFractionGroupName" Width="100%" runat="server" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblLink lblMandatory" id="lblSpecimen">
                                <%=GetLabel("Jenis Spesimen")%></label>
                        </td>
                        <td>
                            <input type="hidden" value="" runat="server" id="hdnSpecimenID" />
                            <table style="width: 100%" cellpadding="0" cellspacing="0">
                                <colgroup>
                                    <col style="width: 100px" />
                                    <col style="width: 3px" />
                                    <col />
                                </colgroup>
                                <tr>
                                    <td>
                                        <asp:TextBox ID="txtSpecimenCode" Width="100%" runat="server" />
                                    </td>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtSpecimenName" Width="100%" runat="server" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblMandatory">
                                <%=GetLabel("Satuan Metrik")%></label>
                        </td>
                        <td>
                            <dxe:ASPxComboBox ID="cboMetricUnit" Width="103px" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblMandatory">
                                <%=GetLabel("Satuan International")%></label>
                        </td>
                        <td>
                            <dxe:ASPxComboBox ID="cboInternationalUnit" Width="103px" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblMandatory">
                                <%=GetLabel("Faktor Konversi")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtConversion" CssClass="number" Width="100px" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Catatan")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtRemarks" Width="100%" runat="server" TextMode="MultiLine" Rows="2" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td colspan="2">
                            <asp:CheckBox ID="chkIsUsingFooterNote" Width="100%" runat="server" Text=" Menggunakan Catatan Footer" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Catatan Footer")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtFooterNote" Width="100%" runat="server" TextMode="MultiLine" Rows="2" />
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
                            <label class="lblNormal">
                                <%=GetLabel("Urutan Cetak")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtDisplayOrder" CssClass="number" Width="100px" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblMandatory">
                                <%=GetLabel("Jenis Hasil Pemeriksaan")%></label>
                        </td>
                        <td>
                            <dxe:ASPxComboBox ID="cboLabTestResultType" ClientInstanceName="cboLabTestResultType"
                                Width="100px" runat="server">
                                <ClientSideEvents ValueChanged="function(s,e){ onCboLabTestResultTypeChanged(); }" />
                            </dxe:ASPxComboBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Decimal Digits")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtDecmalDigits" CssClass="number" Width="100px" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            <asp:CheckBox ID="chkIsDisplay" Width="300px" runat="server" Text=" Tampilkan di Grafik" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Warna Grafik")%></label>
                        </td>
                        <td>
                            <table cellpadding="0" cellspacing="0">
                                <tr>
                                    <td>
                                        <asp:TextBox ID="txtDisplayColor" CssClass="colorpicker" Width="100px" runat="server" />
                                    </td>
                                    <td style="padding-left: 5px">
                                        <asp:TextBox ID="txtDisplayColorPicker" runat="server" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Nilai Minimum (Grafik)")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtChartMinValue" CssClass="number" Width="100px" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Nilai Maximum (Grafik)")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtChartMaxValue" CssClass="number" Width="100px" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Communication Code")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtCommCode" Width="100px" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Metode Pemeriksaan")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtTestMethod" Width="100px" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            <asp:CheckBox ID="chkIsConfidential" Width="300px" runat="server" Text=" Hasil Rahasia" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>
