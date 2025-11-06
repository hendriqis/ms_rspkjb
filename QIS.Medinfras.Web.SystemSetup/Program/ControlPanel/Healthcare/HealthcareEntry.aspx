<%@ Page Language="C#" MasterPageFile="~/Libs/MasterPage/MPEntry.master" AutoEventWireup="true"
    CodeBehind="HealthcareEntry.aspx.cs" Inherits="QIS.Medinfras.Web.SystemSetup.Program.HealthcareEntry" %>

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
            setDatePicker('<%:txtLicenseDate.ClientID %>');
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
            $('#lblZipCode.lblLink').live('click', function () {
                openSearchDialog('zipcodes', 'IsDeleted = 0', function (value) {
                    onTxtZipCodeChanged(value);
                });
            });

            $('#<%=txtZipCode.ClientID %>').live('change', function () {
                onTxtZipCodeChangedValue($(this).val());
            });

            function onTxtZipCodeChanged(value) {
                var filterExpression = "ID = '" + value + "' AND IsDeleted = 0";
                Methods.getObject('GetZipCodesList', filterExpression, function (result) {
                    if (result != null) {
                        $('#<%=hdnZipCode.ClientID %>').val(result.ID);
                        $('#<%=txtZipCode.ClientID %>').val(result.ZipCode);
                    }
                    else {
                        $('#<%=hdnZipCode.ClientID %>').val('');
                        $('#<%=txtZipCode.ClientID %>').val('');
                    }
                });
            }

            function onTxtZipCodeChangedValue(value) {
                var filterExpression = "ZipCode = '" + value + "' AND IsDeleted = 0";
                Methods.getObject('GetZipCodesList', filterExpression, function (result) {
                    if (result != null) {
                        $('#<%=hdnZipCode.ClientID %>').val(result.ID);
                        $('#<%=txtZipCode.ClientID %>').val(result.ZipCode);
                    }
                    else {
                        $('#<%=hdnZipCode.ClientID %>').val('');
                        $('#<%=txtZipCode.ClientID %>').val('');
                    }
                });
            }
            //#endregion
        }
    </script>
    <input type="hidden" id="hdnPageTitle" runat="server" />
    <input type="hidden" id="hdnID" runat="server" value="" />
    <table class="tblContentArea">
        <colgroup>
            <col style="width: 50%" />
            <col style="width: 50%" />
        </colgroup>
        <tr>
            <td style="padding: 5px; vertical-align: top">
                <h4>
                    <%=GetLabel("Informasi Umum")%></h4>
                <table class="tblEntryContent" style="width: 100%">
                    <colgroup>
                        <col style="width: 25%" />
                    </colgroup>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblMandatory">
                                <%=GetLabel("Kode Rumah Sakit")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtHealthcareID" Width="150px" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblMandatory">
                                <%=GetLabel("Nama Rumah Sakit")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtHealthcareName" Width="460px" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblMandatory">
                                <%=GetLabel("Nama Singkat")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtShortName" Width="300px" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Inisial")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtInitial" Width="80px" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblMandatory">
                                <%=GetLabel("Regional Rumah Sakit")%></label>
                        </td>
                        <td>
                            <dxe:ASPxComboBox ID="cboOperatingGroup" Width="300px" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblMandatory">
                                <%=GetLabel("Kelas Rumah Sakit")%></label>
                        </td>
                        <td>
                            <dxe:ASPxComboBox ID="cboHealthcareClass" Width="300px" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Kode Tarif INACBG 1")%></label>
                        </td>
                        <td>
                            <dxe:ASPxComboBox ID="cboKdTarifINACBG1" Width="300px" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Kode Tarif INACBG 2")%></label>
                        </td>
                        <td>
                            <dxe:ASPxComboBox ID="cboKdTarifINACBG2" Width="300px" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Nomor NPWP")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtNpwpNo" Width="300px" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblMandatory">
                                <%=GetLabel("Nomor Lisensi")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtLicenseNo" Width="300px" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Kode BPJS")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtBPJSCode" Width="100px" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Perusahaan / Yayasan")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtCompanyName" Width="300px" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblMandatory">
                                <%=GetLabel("Nama Direktur")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtDirectorName" Width="460px" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("No Akunt GL Segment")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtGLAccountSegmentNo" Width="300px" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Nama Logo")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtLogoFileName" Width="300px" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Nama Gambar Login")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtLoginPageImageFileName" Width="300px" runat="server" />
                            <asp:CheckBox ID="chkIsUsingCustomRightPanel" runat="server" />
                            <%=GetLabel("Custom Right Panel")%>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Batas berlaku Lisensi")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtLicenseDate" Width="30%" runat="server" CssClass="datepicker" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Update Terakhir Lisensi")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtLastLicenseDateExpired" Width="30%" runat="server" />
                        </td>
                    </tr>
                </table>
            </td>
            <td style="padding: 5px; vertical-align: top">
                <h4>
                    <%=GetLabel("Alamat")%></h4>
                <table class="tblEntryContent" style="width: 100%">
                    <colgroup>
                        <col style="width: 15%" />
                        <col style="width: 65%" />
                    </colgroup>
                    <tr>
                        <td class="tdLabel" style="vertical-align: top; padding-top: 5px;">
                            <label class="lblMandatory">
                                <%=GetLabel("Jalan")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtAddress" Width="100%" runat="server" TextMode="MultiLine" Rows="2" />
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
                                <%=GetLabel("Telepon")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtTelephoneNo" Width="100%" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Fax No")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtFaxNo" Width="100%" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Email")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtEmail" CssClass="email" Width="100%" runat="server" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>
