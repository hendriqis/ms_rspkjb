<%@ Page Title="" Language="C#" MasterPageFile="~/libs/MasterPage/MPEntry.master"
    AutoEventWireup="true" CodeBehind="BankEntry.aspx.cs" Inherits="QIS.Medinfras.Web.Finance.Program.BankEntry" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<asp:Content ID="Content2" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div class="menuTitle">
        <%=HttpUtility.HtmlEncode(GetPageTitle())%></div>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript">
        function onLoad() {

            //#region Province
            function onGetSCProvinceFilterExpression() {
                var filterExpression = "<%:OnGetSCProvinceFilterExpression() %>";
                return filterExpression;
            }

            $('#lblProvince.lblLink').click(function () {
                openSearchDialog('stdcode', onGetSCProvinceFilterExpression(), function (value) {
                    $('#<%=txtProvinceCode.ClientID %>').val(value);
                    onTxtProvinceCodeChanged(value);
                });
            });

            $('#<%=txtProvinceCode.ClientID %>').change(function () {
                onTxtProvinceCodeChanged($(this).val());
            });

            function onTxtProvinceCodeChanged(value) {
                var filterExpression = onGetSCProvinceFilterExpression() + " AND StandardCodeID LIKE '%^" + value + "'";
                Methods.getObject('GetStandardCodeList', filterExpression, function (result) {
                    if (result != null)
                        $('#<%=txtProvinceName.ClientID %>').val(result.StandardCodeName);
                    else {
                        $('#<%=txtProvinceCode.ClientID %>').val('');
                        $('#<%=txtProvinceName.ClientID %>').val('');
                    }
                });
            }
            //#endregion

            //#region Zip Code
            $('#lblZipCode.lblLink').click(function () {
                openSearchDialog('zipcodes', 'IsDeleted = 0', function (value) {
                    onTxtZipCodeChanged(value);
                });
            });

            $('#<%=txtZipCode.ClientID %>').change(function () {
                onTxtZipCodeChangedValue($(this).val());
            });

            function onTxtZipCodeChanged(value) {
                var filterExpression = "ID = " + value + " AND IsDeleted = 0";
                Methods.getObject('GetvZipCodesList', filterExpression, function (result) {
                    if (result != null) {
                        $('#<%=hdnZipCode.ClientID %>').val(result.ID);
                        $('#<%=txtZipCode.ClientID %>').val(result.ZipCode);
                        $('#<%=txtCity.ClientID %>').val(result.City);
                        $('#<%=txtCounty.ClientID %>').val(result.County);
                        $('#<%=txtDistrict.ClientID %>').val(result.District);
                        $('#<%=txtCity.ClientID %>').val(result.City);
                        $('#<%=txtProvinceCode.ClientID %>').val(result.GCProvince.split('^')[1]);
                        $('#<%=txtProvinceName.ClientID %>').val(result.Province);
                    }
                    else {
                        $('#<%=hdnZipCode.ClientID %>').val('');
                        $('#<%=txtZipCode.ClientID %>').val('');
                        $('#<%=txtCity.ClientID %>').val('');
                        $('#<%=txtCounty.ClientID %>').val('');
                        $('#<%=txtDistrict.ClientID %>').val('');
                        $('#<%=txtCity.ClientID %>').val('');
                        $('#<%=txtProvinceCode.ClientID %>').val('');
                        $('#<%=txtProvinceName.ClientID %>').val('');
                    }
                });
            }

            function onTxtZipCodeChangedValue(value) {
                var filterExpression = "ZipCode = '" + value + "' AND IsDeleted = 0";
                Methods.getObject('GetvZipCodesList', filterExpression, function (result) {
                    if (result != null) {
                        $('#<%=hdnZipCode.ClientID %>').val(result.ID);
                        $('#<%=txtZipCode.ClientID %>').val(result.ZipCode);
                        $('#<%=txtCity.ClientID %>').val(result.City);
                        $('#<%=txtCounty.ClientID %>').val(result.County);
                        $('#<%=txtDistrict.ClientID %>').val(result.District);
                        $('#<%=txtCity.ClientID %>').val(result.City);
                        $('#<%=txtProvinceCode.ClientID %>').val(result.GCProvince.split('^')[1]);
                        $('#<%=txtProvinceName.ClientID %>').val(result.Province);
                    }
                    else {
                        $('#<%=hdnZipCode.ClientID %>').val('');
                        $('#<%=txtZipCode.ClientID %>').val('');
                        $('#<%=txtCity.ClientID %>').val('');
                        $('#<%=txtCounty.ClientID %>').val('');
                        $('#<%=txtDistrict.ClientID %>').val('');
                        $('#<%=txtCity.ClientID %>').val('');
                        $('#<%=txtProvinceCode.ClientID %>').val('');
                        $('#<%=txtProvinceName.ClientID %>').val('');
                    }
                });
            }
            //#endregion

            $('#<%=chkIsVirtualPayment.ClientID %>').live('change', function () {
                if ($(this).is(':checked')) {
                    $('#<%=trVirtualPayment.ClientID %>').show();
                }
                else {
                    $('#<%=trVirtualPayment.ClientID %>').hide();
                }
            });
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
            <td style="padding: 5px; vertical-align: top;">
                <h4 class="h4expanded">
                    <%=GetLabel("Data BANK")%></h4>
                <div class="containerTblEntryContent">
                    <table class="tblEntryContent" style="width: 100%">
                        <colgroup>
                            <col style="width: 20%" />
                        </colgroup>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblMandatory">
                                    <%=GetLabel("Kode Bank")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtBankCode" Width="100px" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblMandatory">
                                    <%=GetLabel("Nama Bank")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtBankName" Width="300px" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Nama Bank (utk Tampilan)")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtBankNameDisplay" Width="300px" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblMandatory">
                                    <%=GetLabel("No Akun Bank")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtBankAccountNo" Width="300px" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblMandatory">
                                    <%=GetLabel("Nama Akun Bank")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtBankAccountName" Width="300px" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblMandatory">
                                    <%=GetLabel("Rumah Sakit")%></label>
                            </td>
                            <td>
                                <dxe:ASPxComboBox ID="cboHealthcare" Width="300px" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Tipe Bank")%></label>
                            </td>
                            <td>
                                <dxe:ASPxComboBox ID="cboGCBankType" Width="300px" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan=2><asp:CheckBox ID="chkIsVirtualPayment" runat="server" />   Virtual Payment</td>
                        </tr>
                        <tr id="trVirtualPayment" runat=server style="display:none">
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Virtual Payment Channel")%></label>
                            </td>
                            <td>
                                <dxe:ASPxComboBox ID="cboVirtualPaymentChannel" Width="300px" runat="server" />
                            </td>
                        </tr>
                    </table>
                </div>
                <h4 class="h4expanded">
                    <%=GetLabel("Data Alamat BANK")%></h4>
                <div class="containerTblEntryContent">
                    <table class="tblEntryContent" style="width: 100%">
                        <colgroup>
                            <col style="width: 20%" />
                        </colgroup>
                        <tr>
                            <td class="tdLabel" style="vertical-align: top; padding-top: 5px;">
                                <label class="lblMandatory">
                                    <%=GetLabel("Jalan")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtAddress" Width="500px" runat="server" TextMode="MultiLine" Rows="2" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblMandatory lblLink" id="lblZipCode">
                                    <%=GetLabel("Kode Pos")%></label>
                            </td>
                            <td>
                                <table style="width: 500px" cellpadding="0" cellspacing="0">
                                    <colgroup>
                                        <col style="width: 200px" />
                                        <col style="width: 2px" />
                                        <col />
                                    </colgroup>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtZipCode" Width="100px" runat="server" />
                                        </td>
                                        <td>
                                            &nbsp
                                        </td>
                                        <td>
                                            &nbsp
                                        </td>
                                        <td>
                                            <label class="lblNormal">
                                                <%=GetLabel("RT/RW")%></label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtRTData" Width="50px" runat="server" />
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtRWData" Width="50px" runat="server" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Desa / Kabupaten")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtCounty" Width="500px" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Kecamatan")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtDistrict" Width="500px" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblMandatory lblNormal">
                                    <%=GetLabel("Kota")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtCity" Width="500px" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblLink" id="lblProvince">
                                    <%=GetLabel("Provinsi")%></label>
                            </td>
                            <td>
                                <table style="width: 500px" cellpadding="0" cellspacing="0">
                                    <colgroup>
                                        <col style="width: 200px" />
                                        <col style="width: 3px" />
                                        <col />
                                    </colgroup>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtProvinceCode" Width="200px" runat="server" />
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtProvinceName" Width="292px" runat="server" />
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
