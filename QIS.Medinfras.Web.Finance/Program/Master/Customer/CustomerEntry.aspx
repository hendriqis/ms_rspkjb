<%@ Page Language="C#" MasterPageFile="~/Libs/MasterPage/MPEntry.master" AutoEventWireup="true"
    CodeBehind="CustomerEntry.aspx.cs" Inherits="QIS.Medinfras.Web.Finance.Program.CustomerEntry" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<asp:Content ID="Content2" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div class="menuTitle">
        <%=HttpUtility.HtmlEncode(GetPageTitle())%></div>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript">
        function onLoad() {

            //#region Paramedic Master
            function onGetParamedicMasterFilterExpression() {
                var filterExpression = "IsDeleted = 0";
                return filterExpression;
            }

            $('#lblParamedic.lblLink').live('click', function () {
                openSearchDialog('paramedic', onGetParamedicMasterFilterExpression(), function (value) {
                    $('#<%=txtParamedicCode.ClientID %>').val(value);
                    ontxtParamedicCodeChanged(value);
                });
            });

            $('#<%=txtParamedicCode.ClientID %>').live('change', function () {
                ontxtParamedicCodeChanged($(this).val());
            });

            function ontxtParamedicCodeChanged(value) {
                var filterExpression = onGetParamedicMasterFilterExpression() + " AND ParamedicCode = '" + value + "'";
                Methods.getObject('GetParamedicMasterList', filterExpression, function (result) {
                    if (result != null) {
                        $('#<%=hdnParamedicID.ClientID %>').val(result.ParamedicID);
                        $('#<%=txtParamedicName.ClientID %>').val(result.FullName);
                    }
                    else {
                        $('#<%=hdnParamedicID.ClientID %>').val('');
                        $('#<%=txtParamedicCode.ClientID %>').val('');
                        $('#<%=txtParamedicName.ClientID %>').val('');
                    }
                });
            }
            //#endregion

            $('#<%=txtEKlaimPayorID.ClientID %>').live('change', function () {
                if (isNaN($('#<%=txtEKlaimPayorID.ClientID %>').val())) {
                    $('#<%=txtEKlaimPayorID.ClientID %>').val('');
                }
                else if ($('#<%=txtEKlaimPayorID.ClientID %>').val() == '0') {
                    $('#<%=txtEKlaimPayorID.ClientID %>').val('');                
                }
            });

            //#region Customer Bill To
            function onGetCustomerFilterExpression() {
                return "<%=OnGetCustomerFilterExpression() %>";
            }

            $('#lblCustomerBillTo.lblLink').click(function () {
                openSearchDialog('businesspartners', onGetCustomerFilterExpression(), function (value) {
                    $('#<%=txtCustomerBillToCode.ClientID %>').val(value);
                    onTxtCustomerBillToCodeChanged(value);
                });
            });

            $('#<%=txtCustomerBillToCode.ClientID %>').change(function () {
                onTxtCustomerBillToCodeChanged($(this).val());
            });

            function onTxtCustomerBillToCodeChanged(value) {
                var filterExpression = onGetCustomerFilterExpression() + " AND BusinessPartnerCode = '" + value + "'";
                Methods.getObject('GetBusinessPartnersList', filterExpression, function (result) {
                    if (result != null) {
                        $('#<%=hdnCustomerBillToID.ClientID %>').val(result.BusinessPartnerID);
                        $('#<%=txtCustomerBillToName.ClientID %>').val(result.BusinessPartnerName);
                    }
                    else {
                        $('#<%=hdnCustomerBillToID.ClientID %>').val('');
                        $('#<%=txtCustomerBillToCode.ClientID %>').val('');
                        $('#<%=txtCustomerBillToName.ClientID %>').val('');
                    }
                });
            }
            //#endregion

            //#region Customer Group
            $('#lblCustomerGroup.lblLink').click(function () {
                openSearchDialog('customergroup', 'IsDeleted = 0', function (value) {
                    $('#<%=txtCustomerGroupCode.ClientID %>').val(value);
                    onTxtCustomerGroupCodeChanged(value);
                });
            });

            $('#<%=txtCustomerGroupCode.ClientID %>').change(function () {
                onTxtCustomerGroupCodeChanged($(this).val());
            });

            function onTxtCustomerGroupCodeChanged(value) {
                var filterExpression = "CustomerGroupCode = '" + value + "' AND IsDeleted = 0";
                Methods.getObject('GetCustomerGroupList', filterExpression, function (result) {
                    if (result != null) {
                        $('#<%=hdnCustomerGroupID.ClientID %>').val(result.CustomerGroupID);
                        $('#<%=txtCustomerGroupCode.ClientID %>').val(result.CustomerGroupCode);
                        $('#<%=txtCustomerGroupName.ClientID %>').val(result.CustomerGroupName);
                    }
                    else {
                        $('#<%=hdnCustomerGroupID.ClientID %>').val('');
                        $('#<%=txtCustomerGroupCode.ClientID %>').val('');
                        $('#<%=txtCustomerGroupName.ClientID %>').val('');
                    }
                });
            }
            //#endregion

            //#region Customer Line
            $('#lblCustomerLine.lblLink').click(function () {
                openSearchDialog('customerline', 'IsDeleted = 0', function (value) {
                    $('#<%=txtCustomerLineCode.ClientID %>').val(value);
                    onTxtCustomerLineCodeChanged(value);
                });
            });

            $('#<%=txtCustomerLineCode.ClientID %>').change(function () {
                onTxtCustomerLineCodeChanged($(this).val());
            });

            function onTxtCustomerLineCodeChanged(value) {
                var filterExpression = "CustomerLineCode = '" + value + "' AND IsDeleted = 0";
                Methods.getObject('GetCustomerLineList', filterExpression, function (result) {
                    if (result != null) {
                        $('#<%=hdnCustomerLineID.ClientID %>').val(result.CustomerLineID);
                        $('#<%=txtCustomerLineName.ClientID %>').val(result.CustomerLineName);
                    }
                    else {
                        $('#<%=hdnCustomerLineID.ClientID %>').val('');
                        $('#<%=txtCustomerLineCode.ClientID %>').val('');
                        $('#<%=txtCustomerLineName.ClientID %>').val('');
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

            //#region Copy Address
            $('#<%=chkCopyAddress.ClientID %>').live('change', function () {
                if ($(this.checked)) {
                    var address = $('#<%=txtAddress.ClientID %>').val();
                    var county = $('#<%=txtCounty.ClientID %>').val();
                    var district = $('#<%=txtDistrict.ClientID %>').val();
                    var city = $('#<%=txtCity.ClientID %>').val();
                    var provinceCode = $('#<%=txtProvinceCode.ClientID %>').val();
                    var provinceName = $('#<%=txtProvinceName.ClientID %>').val();
                    var zipCodeHdn = $('#<%=hdnZipCode.ClientID %>').val();
                    var zipCode = $('#<%=txtZipCode.ClientID %>').val();
                    var telephoneNo = $('#<%=txtTelephoneNo.ClientID %>').val();


                    $('#<%=txtBillingAddress.ClientID %>').val(address);
                    $('#<%=txtBillingCounty.ClientID %>').val(county);
                    $('#<%=txtBillingDistrict.ClientID %>').val(district);
                    $('#<%=txtBillingCity.ClientID %>').val(city);
                    $('#<%=txtBillingProvinceCode.ClientID %>').val(provinceCode);
                    $('#<%=txtBillingProvinceName.ClientID %>').val(provinceName);
                    $('#<%=hdnBillingZipCode.ClientID %>').val(zipCodeHdn);
                    $('#<%=txtBillingZipCode.ClientID %>').val(zipCode);
                    $('#<%=txtBillingTelephoneNo.ClientID %>').val(telephoneNo);

                }
            });
            //#endregion

            //#region Billing Zip Code
            $('#lblBillingZipCode.lblLink').click(function () {
                openSearchDialog('zipcodes', 'IsDeleted = 0', function (value) {
                    onTxtBillingZipCodeChanged(value);
                });
            });

            $('#<%=txtBillingZipCode.ClientID %>').change(function () {
                onTxtBillingZipCodeChangedValue($(this).val());
            });

            function onTxtBillingZipCodeChanged(value) {
                if (value != '') {
                    var filterExpression = "ID = '" + value + "' AND IsDeleted = 0";
                    Methods.getObject('GetvZipCodesList', filterExpression, function (result) {
                        if (result != null) {
                            $('#<%=hdnBillingZipCode.ClientID %>').val(result.ID);
                            $('#<%=txtBillingZipCode.ClientID %>').val(result.ZipCode);
                            $('#<%=txtBillingCity.ClientID %>').val(result.City);
                            $('#<%=txtBillingCounty.ClientID %>').val(result.County);
                            $('#<%=txtBillingDistrict.ClientID %>').val(result.District);
                            $('#<%=txtBillingCity.ClientID %>').val(result.City);
                            $('#<%=txtBillingProvinceCode.ClientID %>').val(result.GCProvince.split('^')[1]);
                            $('#<%=txtBillingProvinceName.ClientID %>').val(result.Province);
                        }
                        else {
                            $('#<%=hdnBillingZipCode.ClientID %>').val('');
                            $('#<%=txtBillingZipCode.ClientID %>').val('');
                            $('#<%=txtBillingCity.ClientID %>').val('');
                            $('#<%=txtBillingCounty.ClientID %>').val('');
                            $('#<%=txtBillingDistrict.ClientID %>').val('');
                            $('#<%=txtBillingCity.ClientID %>').val('');
                            $('#<%=txtBillingProvinceCode.ClientID %>').val('');
                            $('#<%=txtBillingProvinceName.ClientID %>').val('');
                        }
                    });
                }
                else {
                    $('#<%=hdnBillingZipCode.ClientID %>').val('');
                    $('#<%=txtBillingZipCode.ClientID %>').val('');
                    $('#<%=txtBillingCity.ClientID %>').val('');
                    $('#<%=txtBillingCounty.ClientID %>').val('');
                    $('#<%=txtBillingDistrict.ClientID %>').val('');
                    $('#<%=txtBillingCity.ClientID %>').val('');
                    $('#<%=txtBillingProvinceCode.ClientID %>').val('');
                    $('#<%=txtBillingProvinceName.ClientID %>').val('');
                }
            }

            function onTxtBillingZipCodeChangedValue(value) {
                if (value != '') {
                    var filterExpression = "ZipCode = '" + value + "' AND IsDeleted = 0";
                    Methods.getObject('GetvZipCodesList', filterExpression, function (result) {
                        if (result != null) {
                            $('#<%=hdnBillingZipCode.ClientID %>').val(result.ID);
                            $('#<%=txtBillingZipCode.ClientID %>').val(result.ZipCode);
                            $('#<%=txtBillingCity.ClientID %>').val(result.City);
                            $('#<%=txtBillingCounty.ClientID %>').val(result.County);
                            $('#<%=txtBillingDistrict.ClientID %>').val(result.District);
                            $('#<%=txtBillingCity.ClientID %>').val(result.City);
                            $('#<%=txtBillingProvinceCode.ClientID %>').val(result.GCProvince.split('^')[1]);
                            $('#<%=txtBillingProvinceName.ClientID %>').val(result.Province);
                        }
                        else {
                            $('#<%=hdnBillingZipCode.ClientID %>').val('');
                            $('#<%=txtBillingZipCode.ClientID %>').val('');
                            $('#<%=txtBillingCity.ClientID %>').val('');
                            $('#<%=txtBillingCounty.ClientID %>').val('');
                            $('#<%=txtBillingDistrict.ClientID %>').val('');
                            $('#<%=txtBillingCity.ClientID %>').val('');
                            $('#<%=txtBillingProvinceCode.ClientID %>').val('');
                            $('#<%=txtBillingProvinceName.ClientID %>').val('');
                        }
                    });
                }
                else {
                    $('#<%=hdnBillingZipCode.ClientID %>').val('');
                    $('#<%=txtBillingZipCode.ClientID %>').val('');
                    $('#<%=txtBillingCity.ClientID %>').val('');
                    $('#<%=txtBillingCounty.ClientID %>').val('');
                    $('#<%=txtBillingDistrict.ClientID %>').val('');
                    $('#<%=txtBillingCity.ClientID %>').val('');
                    $('#<%=txtBillingProvinceCode.ClientID %>').val('');
                    $('#<%=txtBillingProvinceName.ClientID %>').val('');
                }
            }
            //#endregion

            //#region Billing Province
            function onGetSCBillingProvinceFilterExpression() {
                var filterExpression = "<%:onGetSCBillingProvinceFilterExpression() %>";
                return filterExpression;
            }

            $('#lblBillingProvince.lblLink').click(function () {
                openSearchDialog('stdcode', onGetSCBillingProvinceFilterExpression(), function (value) {
                    $('#<%=txtBillingProvinceCode.ClientID %>').val(value);
                });
            });

            $('#<%=txtBillingProvinceCode.ClientID %>').change(function () {
                onTxtBillingProvinceCodeChanged($(this).val());
            });

            function onTxtBillingProvinceCodeChanged(value) {
                var filterExpression = onGetSCBillingProvinceFilterExpression() + " AND StandardCodeID LIKE '%^" + value + "'";
                Methods.getObject('GetStandardCodeList', filterExpression, function (result) {
                    if (result != null)
                        $('#<%=txtBillingProvinceName.ClientID %>').val(result.StandardCodeName);
                    else {
                        $('#<%=txtBillingProvinceCode.ClientID %>').val('');
                        $('#<%=txtBillingProvinceName.ClientID %>').val('');
                    }
                });
            }
            //#endregion

            registerCollapseExpandHandler();

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

            //#region IsLateCharge
            $('.txtCurrency').each(function () {
                $(this).trigger('changeValue');
            });

            if ($('#<%=chkIsLateChargeInPercentage.ClientID %>').is(':checked')) {
                $('#<%=txtLateChargeInPercentage.ClientID %>').change();
                $('#<%=txtLateChargeInPercentage.ClientID%>').removeAttr('readonly');
                $('#<%=txtLateChargeInAmount.ClientID%>').attr('readonly', 'readonly');
            } else {
                $('#<%=txtLateChargeInAmount.ClientID %>').change();
                $('#<%=txtLateChargeInPercentage.ClientID%>').attr('readonly', 'readonly');
                $('#<%=txtLateChargeInAmount.ClientID%>').removeAttr('readonly');
            }

            $('#<%=chkIsLateChargeInPercentage.ClientID %>').change(function () {
                if ($('#<%=chkIsLateChargeInPercentage.ClientID %>').is(':checked')) {
                    $('#<%=txtLateChargeInAmount.ClientID%>').val(0);
                    $('#<%=txtLateChargeInAmount.ClientID%>').attr('readonly', 'readonly');
                    $('#<%=txtLateChargeInPercentage.ClientID%>').removeAttr('readonly');
                } else {
                    $('#<%=txtLateChargeInPercentage.ClientID%>').val(0);
                    $('#<%=txtLateChargeInPercentage.ClientID%>').attr('readonly', 'readonly');
                    $('#<%=txtLateChargeInAmount.ClientID%>').removeAttr('readonly');
                }
            });
            //#endregion
        }

        //#region Logo Perusahaan
        $('#<%=FileUpload1.ClientID %>').live('change', function () {
            readURL(this);
            var fileName = $('#<%=FileUpload1.ClientID %>').val();
            $('#<%=txtFileName.ClientID %>').val(fileName);
            $('#<%=hdnFileName.ClientID %>').val(fileName);

            //fileName = fileName.split('.')[0];
            if ($('#<%=imgPreview.ClientID %>').width() > $('#<%=imgPreview.ClientID %>').height())
                $('#<%=imgPreview.ClientID %>').width('150px');
            else
                $('#<%=imgPreview.ClientID %>').height('150px');
        });

        function readURL(input) {
            if (input.files && input.files[0]) {
                var reader = new FileReader();

                reader.onload = function (e) {
                    $('#<%=hdnUploadedFile1.ClientID %>').val(e.target.result);
                    $('#<%=imgPreview.ClientID %>').attr('src', e.target.result);
                }

                reader.readAsDataURL(input.files[0]);
            }
            else {
                $('#<%=imgPreview.ClientID %>').attr('src', input.value);
            }
        }

        $('#btnUploadFile').live('click', function () {
            document.getElementById('<%= FileUpload1.ClientID %>').click();
        });

        $('#btnDeleteFile').live('click', function () {
            readURL(this);
            var fileName = $('#<%=FileUpload1.ClientID %>').val();
            $('#<%=txtFileName.ClientID %>').val(fileName);
            $('#<%=hdnFileName.ClientID %>').val(fileName);
        });
        //#endregion

    </script>
    <input type="hidden" id="hdnPageTitle" runat="server" />
    <input type="hidden" id="hdnID" runat="server" value="" />
    <input type="hidden" id="hdnFileName" runat="server" value="" />
    <input type="hidden" id="hdnOldCustomerLineID" runat="server" />
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
                                <label>
                                    <%=GetLabel("Kode Instansi")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtCustomerCode" Width="200px" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label>
                                    <%=GetLabel("Kode External")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtExternalCode" Width="200px" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblMandatory">
                                    <%=GetLabel("Nama Instansi")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtCustomerName" Width="100%" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Nama Singkat")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtShortName" Width="200px" runat="server" />
                            </td>
                        </tr>
                    </table>
                </div>
                <h4 class="h4expanded">
                    <%=GetLabel("Informasi Contact Person (CP)")%></h4>
                <div class="containerTblEntryContent">
                    <table class="tblEntryContent" style="width: 100%">
                        <colgroup>
                            <col style="width: 30%" />
                        </colgroup>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Nama (CP)")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtContactPersonName" Width="100%" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Mobile Number (CP)")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtContactPersonPhone" Width="100%" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Email (CP)")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtContactPersonEmail" CssClass="email" Width="100%" runat="server" />
                            </td>
                        </tr>
                    </table>
                </div>
                <h4 class="h4expanded">
                    <%=GetLabel("Informasi Instansi")%></h4>
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
                                    <%=GetLabel("Nomor PKP")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtVATRegistrationNo" Width="100%" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Jangka Waktu Pembayaran")%></label>
                            </td>
                            <td>
                                <dxe:ASPxComboBox ID="cboTerm" Width="100%" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblMandatory">
                                    <%=GetLabel("Tipe Instansi")%></label>
                            </td>
                            <td>
                                <dxe:ASPxComboBox ID="cboCustomerType" Width="100%" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <label class="lblNormal lblLink" id="lblParamedic">
                                    <%=GetLabel("Dokter / Paramedis")%></label>
                            </td>
                            <td>
                                <input type="hidden" value="" runat="server" id="hdnParamedicID" />
                                <table style="width: 100%" cellpadding="0" cellspacing="0">
                                    <colgroup>
                                        <col style="width: 100px" />
                                        <col style="width: 3px" />
                                        <col />
                                    </colgroup>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtParamedicCode" Width="100%" runat="server" />
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtParamedicName" Width="100%" runat="server" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblMandatory">
                                    <%=GetLabel("Skema Tarif")%></label>
                            </td>
                            <td>
                                <dxe:ASPxComboBox ID="cboTariffScheme" Width="100%" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblLink" id="lblCustomerBillTo">
                                    <%=GetLabel("Tagihan Instansi")%></label>
                            </td>
                            <td>
                                <input type="hidden" value="" runat="server" id="hdnCustomerBillToID" />
                                <table style="width: 100%" cellpadding="0" cellspacing="0">
                                    <colgroup>
                                        <col style="width: 100px" />
                                        <col style="width: 3px" />
                                        <col />
                                    </colgroup>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtCustomerBillToCode" Width="100%" runat="server" />
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtCustomerBillToName" Width="100%" runat="server" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblLink lblMandatory" id="lblCustomerGroup">
                                    <%=GetLabel("Customer Group")%></label>
                            </td>
                            <td>
                                <input type="hidden" id="hdnCustomerGroupID" runat="server" />
                                <table style="width: 100%" cellpadding="0" cellspacing="0">
                                    <colgroup>
                                        <col style="width: 100px" />
                                        <col style="width: 3px" />
                                        <col />
                                    </colgroup>
                                    <tr>
                                        <td>
                                            <asp:TextBox runat="server" ID="txtCustomerGroupCode" Width="100%" />
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            <asp:TextBox runat="server" ID="txtCustomerGroupName" Width="100%" ReadOnly="true" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblLink lblMandatory" id="lblCustomerLine">
                                    <%=GetLabel("Customer Line")%></label>
                            </td>
                            <td>
                                <input type="hidden" id="hdnCustomerLineID" runat="server" />
                                <table style="width: 100%" cellpadding="0" cellspacing="0">
                                    <colgroup>
                                        <col style="width: 100px" />
                                        <col style="width: 3px" />
                                        <col />
                                    </colgroup>
                                    <tr>
                                        <td>
                                            <asp:TextBox runat="server" ID="txtCustomerLineCode" Width="100%" />
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            <asp:TextBox runat="server" ID="txtCustomerLineName" Width="100%" ReadOnly="true" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Pengelompokan RL")%></label>
                            </td>
                            <td>
                                <dxe:ASPxComboBox ID="cboRLReportGroup" Width="100%" runat="server" />
                            </td>
                        </tr>
                        <tr style="display: none">
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("No Akunt GL Segment")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtGLAccountSegmentNo" Width="100%" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <%=GetLabel("Perjanjian Denda Pembayaran")%></label>
                            </td>
                            <td style="width: 5px">
                                <asp:CheckBox ID="chkIsLateChargeInPercentage" Checked="true" runat="server" />
                                <asp:TextBox CssClass="txtCurrency" ID="txtLateChargeInPercentage" Width="70px" runat="server"
                                    hiddenVal="0" />
                                %
                                <asp:TextBox CssClass="txtCurrency" ID="txtLateChargeInAmount" Width="150px" runat="server"
                                    hiddenVal="0" />
                            </td>
                        </tr>
                    </table>
                </div>
                <h4 class="h4expanded">
                    <%=GetLabel("Status Instansi")%></h4>
                <div class="containerTblEntryContent">
                    <table class="tblEntryContent" style="width: 100%">
                        <colgroup>
                            <col style="width: 50%" />
                        </colgroup>
                        <tr>
                            <td valign="top" style="display: none">
                                <table>
                                    <colgroup>
                                        <col style="width: 10px" />
                                    </colgroup>
                                    <tr>
                                        <td>
                                            <asp:CheckBox ID="chkIsDummy" Width="100%" runat="server" />
                                        </td>
                                        <td>
                                            <%=GetLabel("Dummy")%>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:CheckBox ID="chkIsCreditHold" Width="100%" runat="server" />
                                        </td>
                                        <td>
                                            <%=GetLabel("Pemegang Kredit")%>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:CheckBox ID="chkIsHasContract" Width="100%" runat="server" />
                                        </td>
                                        <td>
                                            <%=GetLabel("Perusahaan Kerjasama")%>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td valign="top">
                                <table>
                                    <colgroup>
                                        <col style="width: 20%" />
                                        <col style="width: 65%" />
                                    </colgroup>
                                    <tr style="display: none">
                                        <td>
                                            <asp:CheckBox ID="chkIsUsingDunningLetter" Width="100%" runat="server" />
                                        </td>
                                        <td>
                                            <%=GetLabel("Using Dunning Letter")%>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <asp:CheckBox ID="chkIsActive" runat="server" /><%:GetLabel("Aktif")%>
                                        </td>
                                    </tr>
                                    <tr id="trKeteranganNonAktif">
                                        <td class="tdLabel" style="vertical-align: top">
                                            <label class="lblNormal">
                                                <%=GetLabel("Keterangan Non Aktif")%></label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtNotActiveReason" Width="100%" runat="server" TextMode="MultiLine" Rows="2" />
                                        </td>
                                    </tr>
                                    <tr style="color: Red">
                                        <td colspan="2">
                                            <asp:CheckBox ID="chkIsBlacklist" runat="server" /><%:GetLabel("Penutupan Layanan Sementara")%>
                                        </td>
                                    </tr>
                                    <tr style="display: none">
                                        <td>
                                            <asp:CheckBox ID="chkIsTaxable" Width="100%" runat="server" />
                                        </td>
                                        <td>
                                            <%=GetLabel("PKP")%>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <asp:CheckBox ID="chkIsControlMember" runat="server" /><%:GetLabel("Control Member")%>
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
                    <%=GetLabel("Logo Perusahaan")%></h4>
                <div class="containerTblEntryContent">
                    <table width="100%">
                        <colgroup>
                            <col width="150px" />
                        </colgroup>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("File Name")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtFileName" ReadOnly="true" Width="100%" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td rowspan="4" style="height: 150px; width: 150px; border: 1px solid ActiveBorder;"
                                align="center">
                                <input type="hidden" id="hdnUploadedFile1" runat="server" value="" />
                                <img src="" runat="server" id="imgPreview" width="150" height="150" />
                            </td>
                            <td>
                                <div style="display: none">
                                    <asp:FileUpload ID="FileUpload1" runat="server" />
                                </div>
                                <table cellpadding="0" cellspacing="0">
                                    <colgroup>
                                        <col style="width: 120px" />
                                        <col style="width: 120px" />
                                    </colgroup>
                                    <tr>
                                        <td>
                                            <input type="button" style="width: 95%" id="btnUploadFile" value="Upload" />
                                        </td>
                                        <td>
                                            <input type="button" style="width: 95%" id="btnDeleteFile" value="Delete" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <%=GetLabel("extension") %>
                                : jpg,jpeg,png.
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <%=GetLabel("Maximum upload size") %>
                                : 10MB
                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp
                            </td>
                        </tr>
                    </table>
                </div>
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
                                    <%=GetLabel("Kelurahan / Desa")%></label>
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
                                        <col style="width: 100px" />
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
                    <%=GetLabel("Alamat Penagihan")%></h4>
                <div class="containerTblEntryContent">
                    <table class="tblEntryContent" style="width: 100%">
                        <colgroup>
                            <col style="width: 30%" />
                        </colgroup>
                        <tr>
                            <td colspan="2">
                                <asp:CheckBox ID="chkCopyAddress" runat="server" /><%:GetLabel("Salin Alamat Instansi")%>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel" style="vertical-align: top; padding-top: 5px;">
                                <label class="lblMandatory">
                                    <%=GetLabel("Jalan")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtBillingAddress" Width="100%" runat="server" TextMode="MultiLine"
                                    Rows="2" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblLink" id="lblBillingZipCode">
                                    <%=GetLabel("Kode Pos")%></label>
                            </td>
                            <td>
                                <input type="hidden" runat="server" id="hdnBillingZipCode" value="" />
                                <asp:TextBox ID="txtBillingZipCode" Width="100%" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Kelurahan / Desa")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtBillingCounty" Width="100%" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Kecamatan")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtBillingDistrict" Width="100%" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Kota")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtBillingCity" Width="100%" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblLink" id="lblBillingProvince">
                                    <%=GetLabel("Provinsi")%></label>
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
                                            <asp:TextBox ID="txtBillingProvinceCode" Width="100%" runat="server" />
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtBillingProvinceName" Width="100%" runat="server" />
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
                                <asp:TextBox ID="txtBillingTelephoneNo" Width="100%" runat="server" />
                            </td>
                        </tr>
                    </table>
                </div>
                <h4 class="h4expanded" style="display: none">
                    <%=GetLabel("Informasi Bank")%></h4>
                <div class="containerTblEntryContent" style="display: none">
                    <table class="tblEntryContent" style="width: 100%">
                        <colgroup>
                            <col style="width: 30%" />
                        </colgroup>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Nama Bank")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtNamaBank" Width="100%" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("No Account Bank")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtNoAccountBank" Width="100%" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("No Account Virtual Bank")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtNoAccountVirtualBank" Width="100%" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Nama Account Bank")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtNamaAccountBank" Width="100%" runat="server" />
                            </td>
                        </tr>
                    </table>
                </div>
                <h4 class="h4expanded">
                    <%=GetLabel("Informasi Lainnya")%></h4>
                <div class="containerTblEntryContent">
                    <table class="tblEntryContent" style="width: 100%">
                        <colgroup>
                            <col style="width: 30%" />
                        </colgroup>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Nama Penerima")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtReceiptName" Width="100%" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Payor ID (E-Klaim)")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtEKlaimPayorID" Width="100%" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Payor Code (E-Klaim)")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtEKlaimPayorCode" Width="100%" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("COB Code (E-Klaim)")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtEKlaimCOBCode" Width="100%" runat="server" />
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
