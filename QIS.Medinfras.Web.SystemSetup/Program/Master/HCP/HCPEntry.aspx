<%@ Page Language="C#" MasterPageFile="~/Libs/MasterPage/MPEntry.master" AutoEventWireup="true"
    CodeBehind="HCPEntry.aspx.cs" Inherits="QIS.Medinfras.Web.SystemSetup.Program.HCPEntry" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<asp:Content ID="Content2" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div class="menuTitle">
        <%=HttpUtility.HtmlEncode(GetPageTitle())%></div>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript">
        function onLoad() {

            setDatePicker('<%=txtDOB.ClientID %>');
            setDatePicker('<%=txtHiredDate.ClientID %>');
            setDatePicker('<%=txtTerminatedDate.ClientID %>');
            setDatePicker('<%=txtLicenseExpiredDate.ClientID %>');
            setDatePicker('<%=txtNotAvailableUntil.ClientID %>');

            $('#<%:txtDOB.ClientID %>').datepicker('option', 'maxDate', new Date(getDateNow()));

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
                            $('#<%=txtProvinceCode.ClientID %>').val(result.GCProvince.split('^')[1]);
                            $('#<%=txtProvinceName.ClientID %>').val(result.Province);
                        }
                        else {
                            $('#<%=hdnZipCode.ClientID %>').val('');
                            $('#<%=txtZipCode.ClientID %>').val('');
                            $('#<%=txtCity.ClientID %>').val('');
                            $('#<%=txtCounty.ClientID %>').val('');
                            $('#<%=txtDistrict.ClientID %>').val('');
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
                            $('#<%=txtProvinceCode.ClientID %>').val(result.GCProvince.split('^')[1]);
                            $('#<%=txtProvinceName.ClientID %>').val(result.Province);
                        }
                        else {
                            $('#<%=hdnZipCode.ClientID %>').val('');
                            $('#<%=txtZipCode.ClientID %>').val('');
                            $('#<%=txtCity.ClientID %>').val('');
                            $('#<%=txtCounty.ClientID %>').val('');
                            $('#<%=txtDistrict.ClientID %>').val('');
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
                    $('#<%=txtProvinceCode.ClientID %>').val('');
                    $('#<%=txtProvinceName.ClientID %>').val('');
                }
            }
            //#endregion

            //#region Religion
            function onGetSCReligionFilterExpression() {
                var filterExpression = "<%:OnGetSCReligionFilterExpression() %>";
                return filterExpression;
            }

            $('#lblReligion.lblLink').click(function () {
                openSearchDialog('stdcode', onGetSCReligionFilterExpression(), function (value) {
                    $('#<%=txtReligionCode.ClientID %>').val(value);
                    onTxtReligionCodeChanged(value);
                });
            });

            $('#<%=txtReligionCode.ClientID %>').change(function () {
                onTxtReligionCodeChanged($(this).val());
            });

            function onTxtReligionCodeChanged(value) {
                var filterExpression = onGetSCReligionFilterExpression() + " AND StandardCodeID LIKE '%^" + value + "'";
                Methods.getObject('GetStandardCodeList', filterExpression, function (result) {
                    if (result != null)
                        $('#<%=txtReligionName.ClientID %>').val(result.StandardCodeName);
                    else {
                        $('#<%=txtReligionName.ClientID %>').val('');
                        $('#<%=txtReligionCode.ClientID %>').val('');
                    }
                });
            }
            //#endregion

            //#region Nationality
            function onGetSCNationalityFilterExpression() {
                var filterExpression = "<%:OnGetSCNationalityFilterExpression() %>";
                return filterExpression;
            }

            $('#lblNationality.lblLink').click(function () {
                openSearchDialog('stdcode', onGetSCNationalityFilterExpression(), function (value) {
                    $('#<%=txtNationalityCode.ClientID %>').val(value);
                    onTxtNationalityCodeChanged(value);
                });
            });

            $('#<%=txtNationalityCode.ClientID %>').change(function () {
                onTxtNationalityCodeChanged($(this).val());
            });

            function onTxtNationalityCodeChanged(value) {
                var filterExpression = onGetSCNationalityFilterExpression() + " AND StandardCodeID LIKE '%^" + value + "'";
                Methods.getObject('GetStandardCodeList', filterExpression, function (result) {
                    if (result != null)
                        $('#<%=txtNationalityName.ClientID %>').val(result.StandardCodeName);
                    else {
                        $('#<%=txtNationalityCode.ClientID %>').val('');
                        $('#<%=txtNationalityName.ClientID %>').val('');
                    }
                });
            }
            //#endregion

            //#region Referensi VClaimDokterDPJP
            $('#lblVKlaimDokterDPJP.lblLink').click(function () {
                openSearchDialog('vklaimdokterdpjp', '', function (value) {
                    $('#<%=txtVKlaimDokterDPJPCode.ClientID %>').val(value);
                    onTxtDokterDPJPCodeChanged(value);
                });
            });

            $('#<%=txtVKlaimDokterDPJPCode.ClientID %>').change(function () {
                onTxtDokterDPJPCodeChanged($(this).val());
            });

            function onTxtDokterDPJPCodeChanged(value) {
                var filterExpression = "BPJSCode = '" + value + "'";
                Methods.getObject('GetvBPJSReferenceDokterDPJPList', filterExpression, function (result) {
                    if (result != null) {
                        $('#<%=txtVKlaimDokterDPJPName.ClientID %>').val(result.BPJSName);
                        $('#<%=hdnVKlaimDokterDPJPName.ClientID %>').val(result.BPJSName);
                    }
                    else {
                        $('#<%=txtVKlaimDokterDPJPCode.ClientID %>').val('');
                        $('#<%=txtVKlaimDokterDPJPName.ClientID %>').val('');
                        $('#<%=hdnVKlaimDokterDPJPName.ClientID %>').val('');
                    }
                });
            }
            //#endregion

            //#region Referensi HFISDokterDPJP
            $('#lblHFISPhysician.lblLink').click(function () {
                openSearchDialog('vklaimdokterdpjp', '', function (value) {
                    $('#<%=txtBPJSReferenceInfo2Code.ClientID %>').val(value);
                    ontxtBPJSReferenceInfo2CodeChanged(value);
                });
            });

            $('#<%=txtBPJSReferenceInfo2Code.ClientID %>').change(function () {
                ontxtBPJSReferenceInfo2CodeChanged($(this).val());
            });

            function ontxtBPJSReferenceInfo2CodeChanged(value) {
                var filterExpression = "BPJSCode = '" + value + "'";
                Methods.getObject('GetvBPJSReferenceDokterDPJPList', filterExpression, function (result) {
                    if (result != null) {
                        $('#<%=txtBPJSReferenceInfo2Name.ClientID %>').val(result.BPJSName);
                        $('#<%=hdnBPJSReferenceInfo2Name.ClientID %>').val(result.BPJSName);                        
                    }
                    else {
                        $('#<%=txtBPJSReferenceInfo2Code.ClientID %>').val('');
                        $('#<%=txtBPJSReferenceInfo2Name.ClientID %>').val('');
                        $('#<%=hdnBPJSReferenceInfo2Name.ClientID %>').val('');                        
                    }
                });
            }
            //#endregion

            //#region Referensi SpesialisasiBPJS
            $('#lblBPJSSpesialisasi.lblLink').click(function () {
                openSearchDialog('vklaimpoli', '', function (value) {
                    $('#<%=txtKodeSpesialisasi.ClientID %>').val(value);
                    onTxtSpesialisasiDokterDPJPCodeChanged(value);
                });
            });

            $('#<%=txtKodeSpesialisasi.ClientID %>').change(function () {
                onTxtSpesialisasiDokterDPJPCodeChanged($(this).val());
            });

            function onTxtSpesialisasiDokterDPJPCodeChanged(value) {
                var filterExpression = "BPJSCode = '" + value + "'";
                Methods.getObject('GetvBPJSReferencePoliList', filterExpression, function (result) {
                    if (result != null) {
                        $('#<%=txtNamaSpesialisasi.ClientID %>').val(result.BPJSName);
                        $('#<%=hdnBPJSSpesialisasi.ClientID %>').val(result.BPJSName);
                    }
                    else {
                        $('#<%=txtKodeSpesialisasi.ClientID %>').val('');
                        $('#<%=txtNamaSpesialisasi.ClientID %>').val('');
                        $('#<%=hdnBPJSSpesialisasi.ClientID %>').val('');
                    }
                });
            }
            //#endregion

            //#region Referensi SubSpesialisasiBPJS
            $('#lblBPJSSubSpesialisasi.lblLink').click(function () {
                openSearchDialog('vklaimpoli', '', function (value) {
                    $('#<%=txtKodeSubSpesialisasi.ClientID %>').val(value);
                    onTxtSubSpesialisasiDokterDPJPCodeChanged(value);
                });
            });

            $('#<%=txtKodeSubSpesialisasi.ClientID %>').change(function () {
                onTxtSubSpesialisasiDokterDPJPCodeChanged($(this).val());
            });

            function onTxtSubSpesialisasiDokterDPJPCodeChanged(value) {
                var filterExpression = "BPJSCode = '" + value + "'";
                Methods.getObject('GetvBPJSReferencePoliList', filterExpression, function (result) {
                    if (result != null) {
                        $('#<%=txtNamaSubSpesialisasi.ClientID %>').val(result.BPJSName);
                        $('#<%=hdnBPJSSubSpesialisasi.ClientID %>').val(result.BPJSName);
                    }
                    else {
                        $('#<%=txtKodeSubSpesialisasi.ClientID %>').val('');
                        $('#<%=txtNamaSubSpesialisasi.ClientID %>').val('');
                        $('#<%=hdnBPJSSubSpesialisasi.ClientID %>').val('');
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


            $("#btnGetIHSNumber").on("click", function (e) {
                e.preventDefault();
                if ($('#<%=txtNIK.ClientID %>').val() == '')
                    alert("Nomor Identitas (NIK) Dokter/Tenaga Medis harus diisi!");
                else {
                    var NIK = $('#<%=txtNIK.ClientID %>').val();
                    try {
                        IHSService.getParamedicIHSNumberByNIK(NIK, function (result) {
                            GetIHSDataHandler(result);
                        });
                    }
                    catch (err) {
                        displayErrorMessageBox("Integrasi SATUSEHAT", err.Message);
                    }
                }
            });

            function GetIHSDataHandler(result) {
                try {
                    var resultInfo = result.split('|');
                    if (resultInfo[0] == "1") {
                        $('#<%=txtIHSNumber.ClientID %>').val(resultInfo[1]);
                    }
                    else {
                        $('#<%=txtIHSNumber.ClientID %>').val('');
                        displayErrorMessageBox('Integrasi SatuSehat', resultInfo[2]);
                    }
                }
                catch (err) {
                    displayErrorMessageBox('Integrasi SATUSEHAT', 'Error Message : ' + err.Description);
                }
            }

            registerCollapseExpandHandler();

            $('#<%=chkIsAllowWaitingList.ClientID %>').change(function () {
                if ($(this).is(':checked')) {
                    $('#trWaitingList').removeAttr('style');
                }
                else {
                    $('#trWaitingList').attr('style', "display:none");
                }
            });

            $('#<%=chkIsAllowWaitingList.ClientID %>').trigger("change");
        }

    </script>
    <input type="hidden" id="hdnPageTitle" runat="server" />
    <input type="hidden" id="hdnID" runat="server" value="" />
    <input type="hidden" id="hdnIsBridgingToMedinfrasMobileApps" runat="server" value="" />
    <%--<div class="pageTitle"><%=GetLabel("Dokter / Paramedis Rumah Sakit")%></div>--%>
    <table class="tblContentArea">
        <colgroup>
            <col style="width: 50%" />
            <col style="width: 50%" />
        </colgroup>
        <tr>
            <td style="padding: 5px; vertical-align: top">
                <h4 class="h4expanded">
                    <%=GetLabel("Data Personal")%></h4>
                <div class="containerTblEntryContent">
                    <table class="tblEntryContent" style="width: 100%">
                        <colgroup>
                            <col style="width: 35%" />
                            <col style="width: 65%" />
                        </colgroup>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblMandatory">
                                    <%=GetLabel("Kode Dokter / Paramedis")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtParamedicCode" Width="100px" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Gelar Depan")%></label>
                            </td>
                            <td>
                                <dxe:ASPxComboBox ID="cboTitle" Width="105px" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Nama Dokter / Paramedis")%></label>
                            </td>
                            <td>
                                <table style="width: 100%" cellpadding="0" cellspacing="0">
                                    <colgroup>
                                        <col style="width: 49%" />
                                        <col style="width: 3px" />
                                        <col />
                                    </colgroup>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtFirstName" Width="100%" runat="server" />
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtMiddleName" Width="100%" runat="server" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblMandatory">
                                    <%=GetLabel("Nama Belakang")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtFamilyName" Width="100%" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Gelar Belakang")%></label>
                            </td>
                            <td>
                                <dxe:ASPxComboBox ID="cboSuffix" Width="105px" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblMandatory">
                                    <%=GetLabel("Inisial")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtInitial" Width="100px" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblMandatory">
                                    <%=GetLabel("Jenis Kelamin")%></label>
                            </td>
                            <td>
                                <dxe:ASPxComboBox ID="cboGender" Width="105px" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Tempat, Tanggal Lahir")%></label>
                            </td>
                            <td>
                                <table style="width: 100%" cellpadding="0" cellspacing="0">
                                    <colgroup>
                                        <col style="width: 150px" />
                                        <col />
                                    </colgroup>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtCityOfBirth" Width="100%" runat="server" />
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtDOB" Width="120px" runat="server" CssClass="datepicker" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </div>
                <h4 class="h4expanded">
                    <%=GetLabel("Data Dokter / Paramedis")%></h4>
                <div class="containerTblEntryContent">
                    <table class="tblEntryContent" style="width: 100%">
                        <colgroup>
                            <col style="width: 35%" />
                            <col style="width: 65%" />
                        </colgroup>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblMandatory">
                                    <%=GetLabel("Tipe Dokter / Paramedis")%></label>
                            </td>
                            <td>
                                <dxe:ASPxComboBox ID="cboParamedicMasterType" Width="100%" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Spesialisasi")%></label>
                            </td>
                            <td>
                                <dxe:ASPxComboBox ID="cboSpecialty" Width="100%" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Rumah Sakit ")%></label>
                            </td>
                            <td>
                                <dxe:ASPxComboBox ID="cboHealthcare" Width="100%" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblMandatory">
                                    <%=GetLabel("Instalasi")%></label>
                            </td>
                            <td>
                                <dxe:ASPxComboBox ID="cboDepartment" ClientInstanceName="cboDepartment" runat="server"
                                    Width="100%" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblMandatory">
                                    <%=GetLabel("Status Kepegawaian")%></label>
                            </td>
                            <td>
                                <dxe:ASPxComboBox ID="cboEmploymentStatus" Width="100%" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Tangal Mulai Bekerja")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtHiredDate" Width="120px" runat="server" CssClass="datepicker" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Tanggal Berhenti")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtTerminatedDate" Width="120px" runat="server" CssClass="datepicker" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Nomor Lisensi")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtLicenseNo" Width="100%" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Tanggal Berakhir Lisensi")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtLicenseExpiredDate" Width="120px" runat="server" CssClass="datepicker" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblMandatory">
                                    <%=GetLabel("Nomor Registrasi Pajak")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtTaxRegistrationNo" Width="100%" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("NIK") %>                                
                                </label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtNIK" Width="100%" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Nama Sesuai NIK") %>                                
                                </label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtParamedicNameNIK" Width="100%" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Availability")%></label>
                            </td>
                            <td>
                                <asp:CheckBox ID="chkIsAvailable" Width="100%" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Not Available Until")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtNotAvailableUntil" Width="120px" runat="server" CssClass="datepicker" />
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
                                <label class="lblNormal">
                                    <%=GetLabel("Durasi Kunjungan")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtVisitDuration" Width="120px" runat="server" />
                            </td>
                        </tr>
                    </table>
                </div>
                <h4 class="h4expanded">
                    <%=GetLabel("Status Dokter / Paramedis")%></h4>
                <div class="containerTblEntryContent">
                    <table class="tblEntryContent" style="width: 100%">
                        <colgroup>
                            <col style="width: 50%" />
                        </colgroup>
                        <tr>
                            <td valign="top">
                                <table>
                                    <colgroup>
                                        <col style="width: 10px" />
                                    </colgroup>
                                    <tr>
                                        <td>
                                            <asp:CheckBox ID="chkIsSpecialist" Width="100%" runat="server" />
                                        </td>
                                        <td>
                                            <%=GetLabel("Dokter Spesialis")%>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:CheckBox ID="chkIsSubSpecialist" Width="100%" runat="server" />
                                        </td>
                                        <td>
                                            <%=GetLabel("Memiliki Sub Spesialis")%>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:CheckBox ID="chkIsAnesthesist" Width="100%" runat="server" />
                                        </td>
                                        <td>
                                            <%=GetLabel("Anestesi")%>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:CheckBox ID="chkIsRMO" Width="100%" runat="server" />
                                        </td>
                                        <td>
                                            <%=GetLabel("Dokter Jaga (RMO)")%>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:CheckBox ID="chkIsRequestMRFile" Width="100%" runat="server" />
                                        </td>
                                        <td>
                                            <%=GetLabel("Menggunakan Berkas Rekam Medis")%>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td valign="top">
                                <table>
                                    <colgroup>
                                        <col style="width: 10px" />
                                    </colgroup>
                                    <tr>
                                        <td>
                                            <asp:CheckBox ID="chkIsHasPhysicianRole" Width="100%" runat="server" />
                                        </td>
                                        <td>
                                            <%=GetLabel("Bisa dipilih pada saat pendaftaran/appointment")%>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:CheckBox ID="chkIsPrimaryNurse" Width="100%" runat="server" />
                                        </td>
                                        <td>
                                            <%=GetLabel("Perawat Primer / PPJA")%>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:CheckBox ID="chkIsHasRevenueSharing" Width="100%" runat="server" />
                                        </td>
                                        <td>
                                            <%=GetLabel("Perhitungan Jasa Medis")%>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:CheckBox ID="chkIsAppointmentByTimeSlot" Width="100%" runat="server" />
                                        </td>
                                        <td>
                                            <%=GetLabel("Perjanjian menggunakan Time Slot")%>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:CheckBox ID="chkIsAllowPrescribeNarcotics" Width="100%" runat="server" />
                                        </td>
                                        <td>
                                            <%=GetLabel("Pemberian Resep Narkotika/Psikotropika")%>
                                        </td>
                                    </tr>
                                </table>
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
                            <col style="width: 35%" />
                            <col style="width: 65%" />
                        </colgroup>
                        <tr>
                            <td class="tdLabel" style="vertical-align: top; padding-top: 5px;">
                                <label class="lblNormal">
                                    <%=GetLabel("Jalan")%></label>
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
                                    <%=GetLabel("Desa / Kabupaten")%></label>
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
                    </table>
                </div>
                <h4 class="h4expanded">
                    <%=GetLabel("Data Kontak")%></h4>
                <div class="containerTblEntryContent">
                    <table class="tblEntryContent" style="width: 100%">
                        <colgroup>
                            <col style="width: 35%" />
                            <col style="width: 65%" />
                        </colgroup>
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
                                    <%=GetLabel("Nomor Hp")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtMobilePhone" Width="100%" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Nomor Hp")%>
                                    2</label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtMobilePhone2" Width="100%" runat="server" />
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
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Email")%>
                                    2</label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtEmail2" CssClass="email" Width="100%" runat="server" />
                            </td>
                        </tr>
                    </table>
                </div>
                <h4 class="h4expanded">
                    <%=GetLabel("Informasi Bank")%></h4>
                <div class="containerTblEntryContent">
                    <table class="tblEntryContent" style="width: 100%">
                        <colgroup>
                            <col style="width: 35%" />
                        </colgroup>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Nama Bank")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtBankName" Width="100%" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Cabang Bank")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtBankBranch" Width="100%" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("No Rekening Bank")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtBankAccountNo" Width="100%" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("No Akun Virtual Bank")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtVirtualAccountNo" Width="100%" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Nama Rekening Bank")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtBankAccountName" Width="100%" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("No Sandi Kliring")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtBankClearingPassword" Width="100%" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Cara Pembayaran")%></label>
                            </td>
                            <td>
                                <dxe:ASPxComboBox ID="cboPaymentMethod" Width="100%" runat="server" />
                            </td>
                        </tr>
                    </table>
                </div>
                <h4 class="h4expanded">
                    <%=GetLabel("Informasi Tambahan")%></h4>
                <div class="containerTblEntryContent">
                    <table class="tblEntryContent" style="width: 100%">
                        <colgroup>
                            <col style="width: 35%" />
                            <col style="width: 65%" />
                        </colgroup>
                        <tr>
                            <td class="tdLabel">
                                <input type="hidden" runat="server" id="hdnVKlaimDokterDPJPName" value="" />
                                <label class="lblLink" id="lblVKlaimDokterDPJP">
                                    <%=GetLabel("Referensi VClaim")%></label>
                            </td>
                            <td>
                                <table style="width: 100%" cellpadding="0" cellspacing="0">
                                    <colgroup>
                                        <col style="width: 100px" />
                                        <col style="width: 3px" />
                                        <col />
                                    </colgroup>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtVKlaimDokterDPJPCode" Width="100%" runat="server" />
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtVKlaimDokterDPJPName" Width="100%" runat="server" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <input type="hidden" runat="server" id="hdnBPJSReferenceInfo2Name" value="" />
                                <label class="lblNormal lblLink" id="lblHFISPhysician">
                                    <%=GetLabel("Referensi HFIS")%></label>
                            </td>
                            <td>
                                <table style="width: 100%" cellpadding="0" cellspacing="0">
                                    <colgroup>
                                        <col style="width: 100px" />
                                        <col style="width: 3px" />
                                        <col />
                                    </colgroup>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtBPJSReferenceInfo2Code" Width="100%" runat="server" />
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtBPJSReferenceInfo2Name" Width="100%" runat="server" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <input type="hidden" runat="server" id="hdnBPJSSpesialisasi" value="" />
                                <label class="lblNormal lblLink" id="lblBPJSSpesialisasi">
                                    <%=GetLabel("Spesialisasi BPJS")%></label>
                            </td>
                            <td>
                                <table style="width: 100%" cellpadding="0" cellspacing="0">
                                    <colgroup>
                                        <col style="width: 100px" />
                                        <col style="width: 3px" />
                                        <col />
                                    </colgroup>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtKodeSpesialisasi" Width="100%" runat="server" />
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtNamaSpesialisasi" Width="100%" runat="server" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <input type="hidden" runat="server" id="hdnBPJSSubSpesialisasi" value="" />
                                <label class="lblNormal lblLink" id="lblBPJSSubSpesialisasi">
                                    <%=GetLabel("SubSpesialisasi BPJS")%></label>
                            </td>
                            <td>
                                <table style="width: 100%" cellpadding="0" cellspacing="0">
                                    <colgroup>
                                        <col style="width: 100px" />
                                        <col style="width: 3px" />
                                        <col />
                                    </colgroup>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtKodeSubSpesialisasi" Width="100%" runat="server" />
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtNamaSubSpesialisasi" Width="100%" runat="server" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblLink" id="lblReligion">
                                    <%=GetLabel("Agama")%></label>
                            </td>
                            <td>
                                <table style="width: 100%" cellpadding="0" cellspacing="0">
                                    <colgroup>
                                        <col style="width: 100px" />
                                        <col style="width: 3px" />
                                        <col />
                                    </colgroup>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtReligionCode" Width="100%" runat="server" />
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtReligionName" Width="100%" runat="server" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblLink" id="lblNationality">
                                    <%=GetLabel("Kewarganegaraan")%></label>
                            </td>
                            <td>
                                <table style="width: 100%" cellpadding="0" cellspacing="0">
                                    <colgroup>
                                        <col style="width: 100px" />
                                        <col style="width: 3px" />
                                        <col />
                                    </colgroup>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtNationalityCode" Width="100%" runat="server" />
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtNationalityName" Width="100%" runat="server" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label class="lblNormal">
                                    <%=GetLabel("Nama Dokter (E-Klaim)")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtEKlaimParamedicName" Width="100%" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label class="lblNormal">
                                    <%=GetLabel("Nomor Lisensi (E-Klaim)")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtEKlaimParamedicSIP" Width="100%" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label class="lblNormal">
                                    <%=GetLabel("Kode Dokter (Inhealth)")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtInhealthReferenceInfo" Width="100%" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label class="lblNormal">
                                    <%=GetLabel("IHS Number / No. SatuSEHAT") %>
                                </label>
                            </td>
                            <td>
                                <table border="0" cellpadding="1" cellspacing="0">
                                    <colgroup>
                                        <col style="width:150px" />
                                        <col />
                                    </colgroup>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtIHSNumber" Width="100%" runat="server" />
                                        </td>
                                        <td style="padding-left:10px">
                                             <input type="button" id="btnGetIHSNumber" value='<%=GetLabel("IHS Number") %>' title="ID/ Nomor IHS Dokter/Tenaga Medis di Platform SATUSEHAT" class="btnGetIHSNumber w3-btn1 w3-hover-blue" />                                           
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </div>
                <h4 class="h4expanded">
                    <%=GetLabel("Informasi Lain")%></h4>
                <div class="containerTblEntryContent">
                    <table class="tblEntryContent" style="width: 100%">
                        <colgroup>
                            <col style="width: 35%" />
                            <col style="width: 65%" />
                        </colgroup>
                        <tr>
                            <td colspan="2">
                                <asp:CheckBox ID="chkIsAllowWaitingList" runat="server" />&nbsp;<%=GetLabel("Is Allow Waiting List")%>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:CheckBox ID="chkIsDummy" runat="server" />&nbsp;<%=GetLabel("Is Dummy")%>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Maximum Appointment")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtMaxAppointment" Width="100px" runat="server" CssClass="number" />
                            </td>
                        </tr>
                        <tr id="trWaitingList" style="display: none">
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Maximum Waiting List")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtMaximumWaitingList" Width="100px" runat="server" CssClass="number" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel" style="vertical-align: top; padding-top: 5px;">
                                <label class="lblNormal">
                                    <%=GetLabel("Catatan")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtNotes" Width="100%" runat="server" TextMode="MultiLine" Rows="2" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel" style="vertical-align: top; padding-top: 5px;">
                                <label class="lblNormal"><%=GetLabel("Nama File Foto Profile")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtPicName" Width="100%" runat="server"/>
                            </td>
                        </tr>
                    </table>
                </div>
            </td>
        </tr>
    </table>
</asp:Content>
