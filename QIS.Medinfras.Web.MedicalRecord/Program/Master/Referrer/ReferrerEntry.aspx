<%@ Page Language="C#" MasterPageFile="~/Libs/MasterPage/MPEntry.master" AutoEventWireup="true"
    CodeBehind="ReferrerEntry.aspx.cs" Inherits="QIS.Medinfras.Web.MedicalRecord.Program.ReferrerEntry" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<asp:Content ID="Content2" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div class="menuTitle">
        <%=HttpUtility.HtmlEncode(GetPageTitle())%></div>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript">
        function onLoad() {

            //#region IsActive
            $('#<%=chkIsActive.ClientID %>').change(function () {
                if ($(this).is(':checked')) {
                    $('#trKeteranganNonAktif').attr('style', "display:none");
                }
                else {
                    $('#trKeteranganNonAktif').removeAttr('style');
                }
            });

            $('#<%=chkIsActive.ClientID %>').trigger("change");
            //#endregion

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
                if (value != '') {
                    var filterExpression = "ID = '" + value + "' AND IsDeleted = 0";
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
            }

            function onTxtZipCodeChangedValue(value) {
                if (value != '') {
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
            }
            //#endregion

            registerCollapseExpandHandler();

            //#region Kode Provider Rujukan Inhealth
            var isBridging = $('#<%=hdnIsBridgingToInhealth.ClientID %>').val();
            if (isBridging == "1") {
                $('#<%:trInhealthCodeReferrer.ClientID %>').removeAttr('style');
            }
            else {
                $('#<%:trInhealthCodeReferrer.ClientID %>').attr('style', 'display:none');
            }

            $('#lblInhealthProviderRujukan.lblLink').click(function () {
                openSearchDialog('vinhealthreferenceproviderrujukan', '', function (value) {
                    $('#<%=txtKodeInhealthProviderRujukan.ClientID %>').val(value);
                    onTxtProviderRujukanCodeChanged(value);
                });
            });

            $('#<%=txtKodeInhealthProviderRujukan.ClientID %>').change(function () {
                onTxtProviderRujukanCodeChanged($(this).val());
            });

            function onTxtProviderRujukanCodeChanged(value) {
                var filterExpression = "ObjectCode = '" + value + "'";
                Methods.getObject('GetvInhealthReferenceProviderRujukanList', filterExpression, function (result) {
                    if (result != null) {
                        $('#<%=txtKodeInhealthProviderRujukan.ClientID %>').val(result.ObjectCode);
                        $('#<%=txtNamaInhealthProviderRujukan.ClientID %>').val(result.ObjectName);
                    }
                    else {
                        $('#<%=txtKodeInhealthProviderRujukan.ClientID %>').val('');
                        $('#<%=txtNamaInhealthProviderRujukan.ClientID %>').val('');
                    }
                });
            }
            //#endregion

        }
    </script>
    <input type="hidden" id="hdnPageTitle" runat="server" />
    <input type="hidden" id="hdnID" runat="server" value="" />
    <input type="hidden" id="hdnIsBridgingToInhealth" runat="server" value="" />
    <table class="tblContentArea">
        <colgroup>
            <col style="width: 50%" />
        </colgroup>
        <tr>
            <td style="padding: 5px; vertical-align: top">
                <h4 class="h4expanded">
                    <%=GetLabel("Informasi Umum")%></h4>
                <div class="containerTblEntryContent">
                    <table class="tblEntryContent" style="width: 100%">
                        <colgroup>
                            <col style="width: 30%" />
                        </colgroup>
                        <tr>
                            <td class="tdLabel">
                                <%=GetLabel("Kode Perujuk")%>
                            </td>
                            <td>
                                <asp:TextBox ID="txtReferrerCode" Width="100px" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <%=GetLabel("Kode External")%>
                            </td>
                            <td>
                                <asp:TextBox ID="txtCommCode" Width="100px" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblMandatory">
                                    <%=GetLabel("Nama Perujuk")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtReferrerName" Width="100%" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Nama Singkat")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtShortName" Width="100%" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Contact Person")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtContactPerson" Width="100%" runat="server" />
                            </td>
                        </tr>
                        <tr id="trInhealthCodeReferrer" runat="server">
                            <td class="tdLabel">
                                <label class="lblLink" id="lblInhealthProviderRujukan">
                                    <%=GetLabel("Referensi Inhealth")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtKodeInhealthProviderRujukan" Width="25%" runat="server" />
                                <asp:TextBox ID="txtNamaInhealthProviderRujukan" Width="72%" runat="server" />
                            </td>
                        </tr>
                    </table>
                </div>
                <h4 class="h4expanded">
                    <%=GetLabel("Informasi Perujuk")%></h4>
                <div class="containerTblEntryContent">
                    <table class="tblEntryContent" style="width: 100%">
                        <colgroup>
                            <col style="width: 30%" />
                        </colgroup>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Rumah Sakit")%></label>
                            </td>
                            <td>
                                <dxe:ASPxComboBox ID="cboHealthcare" Width="100%" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("NPWP")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtVATRegistrationNo" Width="100%" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Termin")%></label>
                            </td>
                            <td>
                                <dxe:ASPxComboBox ID="cboTerm" Width="100%" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblMandatory">
                                    <%=GetLabel("Kelompok Perujuk")%></label>
                            </td>
                            <td>
                                <dxe:ASPxComboBox ID="cboReferrerGroup" Width="100%" runat="server" />
                            </td>
                        </tr>
                    </table>
                </div>
                <h4 class="h4expanded">
                    <%=GetLabel("Status Perujuk")%></h4>
                <div class="containerTblEntryContent">
                    <table class="tblEntryContent" style="width: 100%">
                        <colgroup>
                            <col style="width: 150px" />
                        </colgroup>
                        <tr>
                            <td colspan="2">
                                <asp:CheckBox ID="chkIsTaxable" runat="server" /><%=GetLabel(" PKP")%>
                            </td>
                        </tr>
                        <tr style="display: none">
                            <td colspan="2">
                                <asp:CheckBox ID="chkIsReferralFrom" runat="server" /><%=GetLabel(" Perujuk dari")%>
                            </td>
                        </tr>
                        <tr style="display: none">
                            <td colspan="2">
                                <asp:CheckBox ID="chkIsReferralTo" runat="server" /><%=GetLabel(" Perujuk untuk")%>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:CheckBox ID="chkIsBlacklist" runat="server" /><%=GetLabel(" Blacklist")%>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:CheckBox ID="chkIsActive" runat="server" /><%:GetLabel(" Aktif")%>
                            </td>
                        </tr>
                        <tr id="trKeteranganNonAktif">
                            <td class="tdLabel" style="vertical-align: top">
                                <label class="lblNormal">
                                    <%=GetLabel("Keterangan Non Aktif")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtNotActiveReason" Width="100%" runat="server" TextMode="MultiLine"
                                    Rows="2" />
                            </td>
                        </tr>
                    </table>
                </div>
            </td>
            <td style="padding: 5px; vertical-align: top">
                <h4 class="h4expanded">
                    <%=GetLabel("Alamat")%></h4>
                <div class="containerTblEntryContent">
                    <table class="tblEntryContent" style="width: 100%">
                        <colgroup>
                            <col style="width: 30%" />
                        </colgroup>
                        <tr>
                            <td class="tdLabel" style="vertical-align: top; padding-top: 5px;">
                                <label class="lblMandatory">
                                    <%=GetLabel("Alamat")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtAddress" Width="100%" runat="server" TextMode="MultiLine" Rows="2" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblLink" id="lblZipCode">
                                    <%=GetLabel("Kode Pos")%></label>
                            </td>
                            <td>
                                <input type="hidden" runat="server" id="hdnZipCode" value="" />
                                <asp:TextBox ID="txtZipCode" Width="100%" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Desa / Kelurahan")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtCounty" Width="100%" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Kecamatan")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtDistrict" Width="100%" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Kota")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtCity" Width="100%" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblLink" id="lblProvince">
                                    <%=GetLabel("Provinsi")%></label>
                            </td>
                            <td>
                                <table style="width: 100%" cellpadding="0" cellspacing="0">
                                    <colgroup>
                                        <col style="width: 30%" />
                                        <col style="width: 3px" />
                                        <col />
                                    </colgroup>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtProvinceCode" Width="100%" runat="server" />
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtProvinceName" Width="100%" runat="server" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("No Telepon")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtTelephoneNo" Width="100%" runat="server" />
                            </td>
                        </tr>
                    </table>
                </div>
                <h4 class="h4expanded">
                    <%=GetLabel("Informasi Lain")%></h4>
                <div class="containerTblEntryContent">
                    <table class="tblEntryContent" style="width: 100%">
                        <colgroup>
                            <col style="width: 30%" />
                        </colgroup>
                        <tr>
                            <td class="tdLabel" style="vertical-align: top; padding-top: 5px;">
                                <label class="lblNormal">
                                    <%=GetLabel("Catatan")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtNotes" Width="100%" runat="server" TextMode="MultiLine" Rows="2" />
                            </td>
                        </tr>
                    </table>
                </div>
                <asp:Panel ID="pnlCustomAttribute" runat="server">
                    <h4 class="h4expanded">
                        <%=GetLabel("Atribut")%></h4>
                    <asp:Repeater ID="rptCustomAttribute" runat="server">
                        <HeaderTemplate>
                            <div class="containerTblEntryContent">
                                <table class="tblEntryContent" style="width: 100%">
                                    <colgroup>
                                        <col style="width: 30%" />
                                    </colgroup>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblNormal">
                                        <%#: Eval("Value") %></label>
                                </td>
                                <td>
                                    <input type="hidden" value='<%#: Eval("Code") %>' runat="server" id="hdnTagFieldCode" />
                                    <asp:TextBox ID="txtTagField" Width="300px" runat="server" />
                                </td>
                            </tr>
                        </ItemTemplate>
                        <FooterTemplate>
                            </table> </div>
                        </FooterTemplate>
                    </asp:Repeater>
                </asp:Panel>
            </td>
        </tr>
    </table>
</asp:Content>
