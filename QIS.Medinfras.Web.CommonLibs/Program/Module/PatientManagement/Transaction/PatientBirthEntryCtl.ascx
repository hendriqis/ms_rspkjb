<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PatientBirthEntryCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.PatientBirthEntryCtl" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<script type="text/javascript" id="dxss_patiententryctl">
    setDatePicker('<%=txtDOBBayi.ClientID %>');
    $('#<%=txtDOBBayi.ClientID %>').datepicker('option', 'maxDate', '0');

    //#region Data Bayi
    $('#<%=chkCopyAlamatIbuKeBayi.ClientID %>').live('change', function () {
        var copy = $(this).is(":checked");
        if (copy) {
            var txtAddressMother = $('#<%=txtAddressMother.ClientID %>').val();
            var txtCountyMother = $('#<%=txtCountyMother.ClientID %>').val();
            var txtDistrictMother = $('#<%=txtDistrictMother.ClientID %>').val();
            var txtCityMother = $('#<%=txtCityMother.ClientID %>').val();
            var txtProvinceCodeMother = $('#<%=txtProvinceCodeMother.ClientID %>').val();
            var txtProvinceNameMother = $('#<%=txtProvinceNameMother.ClientID %>').val();
            var hdnZipCodeMother = $('#<%=hdnZipCodeMother.ClientID %>').val();
            var txtZipCodeMother = $('#<%=txtZipCodeMother.ClientID %>').val();
            var txtTelephoneNoMother = $('#<%=txtTelephoneNoMother.ClientID %>').val();
            $('#<%=txtAddressBaby.ClientID %>').val(txtAddressMother);
            $('#<%=txtCountyBaby.ClientID %>').val(txtCountyMother);
            $('#<%=txtDistrictBaby.ClientID %>').val(txtDistrictMother);
            $('#<%=txtCityBaby.ClientID %>').val(txtCityMother);
            $('#<%=txtProvinceCodeBaby.ClientID %>').val(txtProvinceCodeMother);
            $('#<%=txtProvinceNameBaby.ClientID %>').val(txtProvinceNameMother);
            $('#<%=hdnZipCodeBaby.ClientID %>').val(hdnZipCodeMother);
            $('#<%=txtZipCodeBaby.ClientID %>').val(txtZipCodeMother);
            $('#<%=txtTelephoneNoBaby.ClientID %>').val(txtTelephoneNoMother);
        } else {
            $('#<%=txtBabyMRN.ClientID %>').trigger('change');
        }
    });
    //#endregion

    //#region Data Ayah
    $(function () {
        $('#<%=chkIsFatherHasMRN.ClientID %>').live('change', function () {
            var hasMRN = $(this).is(":checked");
            if (hasMRN) {
                $('#lblMRNAyah').attr('class', 'lblLink');
            }
            else {
                $('#lblMRNAyah').attr('class', 'lblDisabled');
                $('#<%=txtFatherMRN.ClientID %>').val('');
            }
        });

        $('#<%=chkCopyAlamatIbuKeAyah.ClientID %>').live('change', function () {
            var copy = $(this).is(":checked");
            if (copy) {
                var txtAddressMother = $('#<%=txtAddressMother.ClientID %>').val();
                var txtCountyMother = $('#<%=txtCountyMother.ClientID %>').val();
                var txtDistrictMother = $('#<%=txtDistrictMother.ClientID %>').val();
                var txtCityMother = $('#<%=txtCityMother.ClientID %>').val();
                var txtProvinceCodeMother = $('#<%=txtProvinceCodeMother.ClientID %>').val();
                var txtProvinceNameMother = $('#<%=txtProvinceNameMother.ClientID %>').val();
                var hdnZipCodeMother = $('#<%=hdnZipCodeMother.ClientID %>').val();
                var txtZipCodeMother = $('#<%=txtZipCodeMother.ClientID %>').val();
                var txtTelephoneNoMother = $('#<%=txtTelephoneNoMother.ClientID %>').val();
                $('#<%=txtAddressFather.ClientID %>').val(txtAddressMother);
                $('#<%=txtCountyFather.ClientID %>').val(txtCountyMother);
                $('#<%=txtDistrictFather.ClientID %>').val(txtDistrictMother);
                $('#<%=txtCityFather.ClientID %>').val(txtCityMother);
                $('#<%=txtProvinceCodeFather.ClientID %>').val(txtProvinceCodeMother);
                $('#<%=txtProvinceNameFather.ClientID %>').val(txtProvinceNameMother);
                $('#<%=hdnZipCodeFather.ClientID %>').val(hdnZipCodeMother);
                $('#<%=txtZipCodeFather.ClientID %>').val(txtZipCodeMother);
                $('#<%=txtTelephoneNoFather.ClientID %>').val(txtTelephoneNoMother);
            } else {
                $('#<%=txtFatherMRN.ClientID %>').trigger('change');
            }
        });


        $('#lblMRNAyah.lblLink').die('click');
        $('#lblMRNAyah.lblLink').live('click', function () {
            var filterExpression = "GCGender = '0003^M' AND GCSex = '0003^M' AND IsDeleted = 0";
            openSearchDialog('patient', filterExpression, function (value) {
                $('#<%=txtFatherMRN.ClientID %>').val(value);
                onPatientEntryTxtMRNFatherChanged(value);
            });
        });

        $('#<%=txtFatherMRN.ClientID %>').die('change');
        $('#<%=txtFatherMRN.ClientID %>').live('change', function () {
            onPatientEntryTxtMRNFatherChanged($(this).val());
        });

        function onPatientEntryTxtMRNFatherChanged(value) {
            var filterExpression = "MedicalNo = '" + value + "' AND IsDeleted = 0";
            Methods.getObject('GetvPatientList', filterExpression, function (entity) {
                if (entity != null) {
                    $('#<%=hdnFatherMRN.ClientID %>').val(entity.MRN);
                    $('#<%=txtFatherMRN.ClientID %>').val(entity.MedicalNo);

                    //#region Data Ayah
                    cboSalutationFather.SetValue(entity.GCSalutation);
                    cboTitleFather.SetValue(entity.GCTitle);
                    $('#<%=txtFirstNameFather.ClientID %>').val(entity.FirstName);
                    $('#<%=txtMiddleNameFather.ClientID %>').val(entity.MiddleName);
                    $('#<%=txtLastNameFather.ClientID %>').val(entity.LastName);
                    cboSuffixFather.SetValue(entity.GCSuffix);
                    //#endregion

                    //#region Alamat Ayah
                    $('#<%=hdnAddressIDFather.ClientID %>').val(entity.HomeAddressID);
                    $('#<%=txtAddressFather.ClientID %>').val(entity.StreetName);
                    $('#<%=txtCountyFather.ClientID %>').val(entity.County);
                    $('#<%=txtDistrictFather.ClientID %>').val(entity.District);
                    $('#<%=txtCityFather.ClientID %>').val(entity.City);
                    $('#<%=txtProvinceCodeFather.ClientID %>').val(entity.GCState.split('^')[1]);
                    $('#<%=txtProvinceNameFather.ClientID %>').val(entity.State);
                    $('#<%=hdnZipCodeFather.ClientID %>').val(entity.ZipCodeID);
                    $('#<%=txtZipCodeFather.ClientID %>').val(entity.ZipCode);
                    $('#<%=txtTelephoneNoFather.ClientID %>').val(entity.PhoneNo1);
                    //#endregion
                }
                else {
                    setEntryPopupIsAdd(true);
                    $('#<%=hdnFatherMRN.ClientID %>').val('');
                    $('#<%=txtFatherMRN.ClientID %>').val('');

                    //#region Data Ayah
                    cboSalutationFather.SetValue('');
                    cboTitleFather.SetValue('');
                    $('#<%=txtFirstNameFather.ClientID %>').val('');
                    $('#<%=txtMiddleNameFather.ClientID %>').val('');
                    $('#<%=txtLastNameFather.ClientID %>').val('');
                    cboSuffixFather.SetValue('');
                    //#endregion

                    //#region Alamat Ayah
                    $('#<%=hdnAddressIDFather.ClientID %>').val('');
                    $('#<%=txtAddressFather.ClientID %>').val('');
                    $('#<%=txtCountyFather.ClientID %>').val('');
                    $('#<%=txtDistrictFather.ClientID %>').val('');
                    $('#<%=txtCityFather.ClientID %>').val('');
                    $('#<%=txtProvinceCodeFather.ClientID %>').val('');
                    $('#<%=txtProvinceNameFather.ClientID %>').val('');
                    $('#<%=hdnZipCodeFather.ClientID %>').val('');
                    $('#<%=txtZipCodeFather.ClientID %>').val('');
                    $('#<%=txtTelephoneNoFather.ClientID %>').val('');
                    //#endregion
                }
            });
        }

        //#region Province Father
        function onGetSCProvinceFatherFilterExpression() {
            var filterExpression = '<%:OnGetSCProvinceFilterExpression() %>';
            return filterExpression;
        }

        $('#lblProvinceFather.lblLink').die('click');
        $('#lblProvinceFather.lblLink').live('click', function () {
            openSearchDialog('stdcode', onGetSCProvinceFatherFilterExpression(), function (value) {
                $('#<%=txtProvinceCodeFather.ClientID %>').val(value);
                ontxtProvinceCodeFatherChanged(value);
            });
        });

        $('#<%=txtProvinceCodeFather.ClientID %>').die('change');
        $('#<%=txtProvinceCodeFather.ClientID %>').live('change', function () {
            ontxtProvinceCodeFatherChanged($(this).val());
        });

        function ontxtProvinceCodeFatherChanged(value) {
            var filterExpression = onGetSCProvinceFatherFilterExpression() + " AND StandardCodeID LIKE '%^" + value + "'";
            Methods.getObject('GetStandardCodeList', filterExpression, function (result) {
                if (result != null)
                    $('#<%=txtProvinceNameFather.ClientID %>').val(result.StandardCodeName);
                else
                    $('#<%=txtProvinceNameFather.ClientID %>').val('');
            });
        }
        //#endregion

        //#region Zip Code Father
        $('#lblZipCodeFather.lblLink').die('click');
        $('#lblZipCodeFather.lblLink').live('click', function () {
            openSearchDialog('zipcodes', 'IsDeleted = 0', function (value) {
                ontxtZipCodeFatherChanged(value);
            });
        });

        $('#<%=txtZipCodeFather.ClientID %>').die('change');
        $('#<%=txtZipCodeFather.ClientID %>').live('change', function () {
            ontxtZipCodeFatherChangedValue($(this).val());
        });

        function ontxtZipCodeFatherChanged(value) {
            if (value != '') {
                var filterExpression = "ID = '" + value + "' AND IsDeleted = 0";
                Methods.getObject('GetvZipCodesList', filterExpression, function (result) {
                    if (result != null) {
                        $('#<%=hdnZipCodeFather.ClientID %>').val(result.ID);
                        $('#<%=txtZipCodeFather.ClientID %>').val(result.ZipCode);
                        $('#<%=txtCountyFather.ClientID %>').val(result.County);
                        $('#<%=txtDistrictFather.ClientID %>').val(result.District);
                        $('#<%=txtCityFather.ClientID %>').val(result.City);
                        $('#<%=txtProvinceCodeFather.ClientID %>').val(result.GCProvince.split('^')[1]);
                        $('#<%=txtProvinceNameFather.ClientID %>').val(result.Province);
                    }
                    else {
                        $('#<%=hdnZipCodeFather.ClientID %>').val('');
                        $('#<%=txtZipCodeFather.ClientID %>').val('');
                        $('#<%=txtCountyFather.ClientID %>').val('');
                        $('#<%=txtDistrictFather.ClientID %>').val('');
                        $('#<%=txtCityFather.ClientID %>').val('');
                        $('#<%=txtProvinceCodeFather.ClientID %>').val('');
                        $('#<%=txtProvinceNameFather.ClientID %>').val('');
                    }
                });
            }
            else {
                $('#<%=hdnZipCodeFather.ClientID %>').val('');
                $('#<%=txtZipCodeFather.ClientID %>').val('');
                $('#<%=txtCountyFather.ClientID %>').val('');
                $('#<%=txtDistrictFather.ClientID %>').val('');
                $('#<%=txtCityFather.ClientID %>').val('');
                $('#<%=txtProvinceCodeFather.ClientID %>').val('');
                $('#<%=txtProvinceNameFather.ClientID %>').val('');
            }
        }

        function ontxtZipCodeFatherChangedValue(value) {
            if (value != '') {
                var filterExpression = "ZipCode = '" + value + "' AND IsDeleted = 0";
                Methods.getObject('GetvZipCodesList', filterExpression, function (result) {
                    if (result != null) {
                        $('#<%=hdnZipCodeFather.ClientID %>').val(result.ID);
                        $('#<%=txtZipCodeFather.ClientID %>').val(result.ZipCode);
                        $('#<%=txtCountyFather.ClientID %>').val(result.County);
                        $('#<%=txtDistrictFather.ClientID %>').val(result.District);
                        $('#<%=txtCityFather.ClientID %>').val(result.City);
                        $('#<%=txtProvinceCodeFather.ClientID %>').val(result.GCProvince.split('^')[1]);
                        $('#<%=txtProvinceNameFather.ClientID %>').val(result.Province);
                    }
                    else {
                        $('#<%=hdnZipCodeFather.ClientID %>').val('');
                        $('#<%=txtZipCodeFather.ClientID %>').val('');
                        $('#<%=txtCountyFather.ClientID %>').val('');
                        $('#<%=txtDistrictFather.ClientID %>').val('');
                        $('#<%=txtCityFather.ClientID %>').val('');
                        $('#<%=txtProvinceCodeFather.ClientID %>').val('');
                        $('#<%=txtProvinceNameFather.ClientID %>').val('');
                    }
                });
            }
            else {
                $('#<%=hdnZipCodeFather.ClientID %>').val('');
                $('#<%=txtZipCodeFather.ClientID %>').val('');
                $('#<%=txtCountyFather.ClientID %>').val('');
                $('#<%=txtDistrictFather.ClientID %>').val('');
                $('#<%=txtCityFather.ClientID %>').val('');
                $('#<%=txtProvinceCodeFather.ClientID %>').val('');
                $('#<%=txtProvinceNameFather.ClientID %>').val('');
            }
        }
        //#endregion

        //#region Patient Job
        function onGetSCPatientJobFilterExpression() {
            var filterExpression = "<%:OnGetSCPatientJobFilterExpression() %>";
            return filterExpression;
        }

        $('#lblPatientJob.lblLink').live('click', function () {
            openSearchDialog('stdcode', onGetSCPatientJobFilterExpression(), function (value) {
                $('#<%=txtPatientJobCode.ClientID %>').val(value);
                onTxtPatientJobCodeChanged(value);
            });
        });

        $('#<%=txtPatientJobCode.ClientID %>').change(function () {
            onTxtPatientJobCodeChanged($(this).val());
        });

        function onTxtPatientJobCodeChanged(value) {
            var filterExpression = onGetSCPatientJobFilterExpression() + " AND StandardCodeID LIKE '%^" + value + "'";
            Methods.getObject('GetStandardCodeList', filterExpression, function (result) {
                if (result != null)
                    $('#<%=txtPatientJobName.ClientID %>').val(result.StandardCodeName);
                else {
                    $('#<%=txtPatientJobCode.ClientID %>').val('');
                    $('#<%=txtPatientJobName.ClientID %>').val('');
                }
            });
        }
        //#endregion
    });

    //#endregion

    $('#<%=FileUpload1.ClientID %>').live('change', function () {
        readURL(this);
        var fileName = $('#<%=FileUpload1.ClientID %>').val().replace("C:\\fakepath\\", "");

        $('#<%=txtBabyPhoto.ClientID %>').val(fileName);
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
        $('#<%=txtBabyPhoto.ClientID %>').val(fileName);
        $('#<%=hdnFileName.ClientID %>').val(fileName);
    });

</script>
<div style="height: 445px; overflow-y: scroll;">
    <input type="hidden" id="hdnBirthRecordID" value="0" runat="server" />
    <input type="hidden" id="hdnDepartmentID" value="0" runat="server" />
    <input type="hidden" id="hdnFileName" value="0" runat="server" />
    <table class="tblEntryContent">
        <colgroup>
            <col style="width: 50%" />
        </colgroup>
        <tr>
            <td style="padding: 5px; vertical-align: top;">
                <h4 class="h4expanded">
                    <%=GetLabel("Data Bayi")%></h4>
                <div class="containerTblEntryContent">
                    <input type="hidden" id="hdnVisitID" value="0" runat="server" />
                    <input type="hidden" id="hdnBabyMRN" value="0" runat="server" />
                    <input type="hidden" id="hdnVisitIDBayi" value="0" runat="server" />
                    <table class="tblEntryContent" style="width: 100%">
                        <colgroup>
                            <col style="width: 30%" />
                        </colgroup>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblLink" id="lblNoRegBayi">
                                    <%=GetLabel("No Pendaftaran Bayi")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtNoRegBayi" Width="100%" runat="server" TabIndex="9" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblMandatory">
                                    <%=GetLabel("No RM Bayi")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtBabyMRN" Width="100%" runat="server" TabIndex="10" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("No. SKL")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtNoSKL" Width="100%" runat="server" TabIndex="10" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Nama Bayi")%></label>
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
                                            <asp:TextBox ID="txtFirstNameBaby" Width="100%" runat="server" />
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtMiddleNameBaby" Width="100%" runat="server" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblMandatory">
                                    <%=GetLabel("Nama Belakang Bayi")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtLastNameBaby" Width="100%" CssClass="required" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblMandatory">
                                    <%=GetLabel("Nama Panggilan Bayi")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtPreferredNameBaby" Width="100%" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblMandatory" id="Label1">
                                    <%=GetLabel("Jenis Kelamin")%></label>
                            </td>
                            <td>
                                <dxe:ASPxComboBox ID="cboGenderBaby" Width="100%" runat="server" TabIndex="16" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Tanggal Lahir")%>
                                    /
                                    <%=GetLabel("Umur Bayi")%></label>
                            </td>
                            <td>
                                <table style="width: 100%" cellpadding="0" cellspacing="0">
                                    <colgroup>
                                        <col style="width: 40%" />
                                        <col style="width: 10%" />
                                        <col style="width: 10%" />
                                        <col />
                                    </colgroup>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtDOBBayi" Width="108px" runat="server" CssClass="datepicker" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Foto Bayi")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtBabyPhoto" ReadOnly="true" Width="100%" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td rowspan="2" style="height: 150px; width: 150px; border: 1px solid ActiveBorder;"
                                align="center">
                                <input type="hidden" id="hdnUploadedFile1" runat="server" value="" />
                                <img src="" runat="server" id="imgPreview" width="170" height="150" />
                            </td>
                            <td>
                                <div style="display: none">
                                    <asp:FileUpload ID="FileUpload1" runat="server" />
                                </div>
                                <table cellpadding="0" cellspacing="0">
                                    <colgroup>
                                        <col style="width: 50%" />
                                        <col style="width: 50%" />
                                    </colgroup>
                                    <tr>
                                        <td>
                                            <input type="button" style="width: 95%" id="btnUploadFile" value="Upload" />
                                        </td>
                                        <td>
                                            <input type="button" style="width: 95%" id="btnDeleteFile" value="Delete" />
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
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <b>
                                    <label class="lblNormal">
                                        <%=GetLabel("DATA ALAMAT BAYI")%></label></b>
                            </td>
                            <td>
                                <asp:CheckBox ID="chkCopyAlamatIbuKeBayi" runat="server" /><%:GetLabel("Salin Data Alamat Ibu")%>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel" style="vertical-align: top; padding-top: 5px;">
                                <input type="hidden" id="hdnAddressIDBaby" runat="server" />
                                <label class="lblMandatory">
                                    <%=GetLabel("Jalan")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtAddressBaby" Width="100%" runat="server" TextMode="MultiLine"
                                    Rows="2" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblLink" id="lblZipCodeBaby">
                                    <%=GetLabel("Kode Pos")%></label>
                            </td>
                            <td>
                                <input type="hidden" runat="server" id="hdnZipCodeBaby" value="" />
                                <asp:TextBox ID="txtZipCodeBaby" Width="100%" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Desa / Kelurahan")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtCountyBaby" Width="100%" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Kecamatan")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtDistrictBaby" Width="100%" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Kota")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtCityBaby" Width="100%" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblLink" id="lblProvinceBaby">
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
                                            <asp:TextBox ID="txtProvinceCodeBaby" Width="100%" runat="server" />
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtProvinceNameBaby" ReadOnly="true" Width="100%" runat="server" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblMandatory">
                                    <%=GetLabel("Telepon")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtTelephoneNoBaby" CssClass="required" Width="100%" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <b>
                                    <label class="lblNormal">
                                        <%=GetLabel("DATA KELAHIRAN BAYI")%></label></b>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblMandatory">
                                    <%=GetLabel("Anak ke")%></label>
                            </td>
                            <td>
                                <table style="width: 100%" cellpadding="0" cellspacing="0">
                                    <colgroup>
                                        <col style="width: 30%" />
                                        <col style="width: 20%" />
                                        <col style="width: 30%" />
                                    </colgroup>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtChildNo" TabIndex="3" Width="80%" runat="server" CssClass="number" />
                                        </td>
                                        <td class="tdLabel">
                                            <label class="lblMandatory">
                                                <%=GetLabel("Jam lahir")%></label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtTimeOfBirth" TabIndex="3" Width="80%" CssClass="time" runat="server"
                                                Style="text-align: center" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblMandatory">
                                    <%=GetLabel("Tempat Lahir")%></label>
                            </td>
                            <td>
                                <dxe:ASPxComboBox ID="cboBornAt" Width="100%" runat="server" TabIndex="4" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblMandatory">
                                    <%=GetLabel("Usia Kehamilan")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtBirthPregnancyAge" Width="80px" runat="server" CssClass="number"
                                    TabIndex="5" />
                                <label>
                                    <%=GetLabel("Minggu")%>
                                </label>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("APGAR Score 1")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtAPGARScore1" Width="100%" CssClass="number" runat="server" TabIndex="6" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("APGAR Score 2")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtAPGARScore2" Width="100%" CssClass="number" runat="server" TabIndex="7" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("APGAR Score 3")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtAPGARScore3" Width="100%" CssClass="number" runat="server" TabIndex="8" />
                            </td>
                        </tr>
                    </table>
                </div>
                <h4 class="h4expanded">
                    <%=GetLabel("Data Kuantitatif")%></h4>
                <div class="containerTblEntryContent">
                    <table class="tblEntryContent" style="width: 100%">
                        <colgroup>
                            <col style="width: 30%" />
                            <col />
                        </colgroup>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblMandatory">
                                    <%=GetLabel("Panjang")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtLength" Width="80px" runat="server" TabIndex="11" CssClass="number" />
                                <label>
                                    <%=GetLabel("cm")%></label>
                            </td>
                            <td class="tdLabel">
                                <label class="lblMandatory">
                                    <%=GetLabel("Berat")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtWeight" Width="80px" runat="server" TabIndex="12" CssClass="number" />
                                <label>
                                    <%=GetLabel("kg")%></label>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Lingkar Kepala")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtHeadCircumference" Width="80px" runat="server" TabIndex="13"
                                    CssClass="number" />
                                <label>
                                    <%=GetLabel("cm")%></label>
                            </td>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Lingkar Dada")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtChestCircumference" Width="80px" runat="server" TabIndex="14"
                                    CssClass="number" />
                                <label>
                                    <%=GetLabel("cm")%></label>
                            </td>
                        </tr>
                    </table>
                </div>
                <h4 class="h4expanded">
                    <%=GetLabel("Data Kualitatif")%></h4>
                <div class="containerTblEntryContent">
                    <table class="tblEntryContent" style="width: 100%">
                        <colgroup>
                            <col style="width: 30%" />
                        </colgroup>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Cara Caesar")%></label>
                            </td>
                            <td>
                                <dxe:ASPxComboBox ID="cboCaesarMethod" Width="100%" runat="server" TabIndex="15" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblMandatory">
                                    <%=GetLabel("Kembar / Tunggal")%></label>
                            </td>
                            <td>
                                <dxe:ASPxComboBox ID="cboTwinSingle" Width="100%" runat="server" TabIndex="16" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblMandatory">
                                    <%=GetLabel("Kondisi Kelahiran")%></label>
                            </td>
                            <td>
                                <dxe:ASPxComboBox ID="cboBornCondition" Width="100%" runat="server" TabIndex="17" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblMandatory">
                                    <%=GetLabel("Cara Melahirkan")%></label>
                            </td>
                            <td>
                                <dxe:ASPxComboBox ID="cboBirthMethod" Width="100%" runat="server" TabIndex="18" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblMandatory">
                                    <%=GetLabel("Komplikasi Kelahiran")%></label>
                            </td>
                            <td>
                                <dxe:ASPxComboBox ID="cboBirthComplication" Width="100%" runat="server" TabIndex="19" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Sebab Kematian")%></label>
                            </td>
                            <td>
                                <dxe:ASPxComboBox ID="cboBirthCOD" Width="100%" runat="server" TabIndex="20" />
                            </td>
                        </tr>
                    </table>
                </div>
            </td>
            <td style="padding: 5px; vertical-align: top">
                <h4 class="h4expanded">
                    <%=GetLabel("Data Ibu")%></h4>
                <div class="containerTblEntryContent">
                    <input type="hidden" id="hdnVisitIDMom" value="0" runat="server" />
                    <input type="hidden" id="hdnVisitIDIbu" value="0" runat="server" />
                    <input type="hidden" id="hdnMotherMRN" value="0" runat="server" />
                    <table class="tblEntryContent" style="width: 100%">
                        <colgroup>
                            <col style="width: 30%" />
                        </colgroup>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblLink" id="lblNoRegIbu">
                                    <%=GetLabel("No Pendaftaran Ibu")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtNoRegIbu" Width="100%" runat="server" TabIndex="9" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblMandatory">
                                    <%=GetLabel("No RM Ibu")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtMotherMRN" Width="100%" runat="server" TabIndex="10" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Sapaan Ibu")%></label>
                            </td>
                            <td>
                                <dxe:ASPxComboBox ID="cboSalutationMother" ClientInstanceName="cboSalutationMother"
                                    Width="100%" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Gelar Depan Ibu")%></label>
                            </td>
                            <td>
                                <dxe:ASPxComboBox ID="cboTitleMother" ClientInstanceName="cboTitleMother" Width="100%"
                                    runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Nama Ibu")%></label>
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
                                            <asp:TextBox ID="txtFirstNameMother" Width="100%" runat="server" />
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtMiddleNameMother" Width="100%" runat="server" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblMandatory">
                                    <%=GetLabel("Nama Belakang Ibu")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtLastNameMother" Width="100%" CssClass="required" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Gelar Belakang Ibu")%></label>
                            </td>
                            <td>
                                <dxe:ASPxComboBox ID="cboSuffixMother" ClientInstanceName="cboSuffixMother" Width="100%"
                                    runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Tanggal Lahir")%>
                                    /
                                    <%=GetLabel("Umur Ibu")%></label>
                            </td>
                            <td>
                                <table style="width: 100%" cellpadding="0" cellspacing="0">
                                    <colgroup>
                                        <col style="width: 40%" />
                                        <col style="width: 10%" />
                                        <col style="width: 10%" />
                                        <col style="width: 10%" />
                                        <col style="width: 10%" />
                                        <col style="width: 10%" />
                                        <col style="width: 10%" />
                                    </colgroup>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtDOBIbu" Width="95%" runat="server" Style="margin-right: 3px;
                                                text-align: center" />
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtAgeYearMom" Width="95%" runat="server" Style="margin-right: 1px;
                                                text-align: center" />
                                        </td>
                                        <td>
                                            <%=GetLabel("Tahun")%>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtAgeMonthMom" Width="95%" runat="server" Style="margin-right: 1px;
                                                text-align: center" />
                                        </td>
                                        <td>
                                            <%=GetLabel("Bulan")%>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtAgeDayMom" Width="95%" runat="server" Style="margin-right: 1px;
                                                text-align: center" />
                                        </td>
                                        <td>
                                            <%=GetLabel("Hari")%>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Tipe Kartu Identitas")%></label>
                            </td>
                            <td>
                                <dxe:ASPxComboBox ID="cboGCIdentityNoType" ClientInstanceName="cboGCIdentityNoType"
                                    Width="100%" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblMandatory">
                                    <%=GetLabel("No Kartu Identitas")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtIdentityNo" Width="100%" CssClass="required" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblMandatory lblLink" id="lblPatientJob">
                                    <%=GetLabel("Pekerjaan")%></label>
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
                                            <asp:TextBox ID="txtPatientJobCode" Width="100%" runat="server" />
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtPatientJobName" Width="100%" ReadOnly="true" runat="server" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <b>
                                    <label class="lblNormal">
                                        <%=GetLabel("DATA ALAMAT IBU")%></label></b>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel" style="vertical-align: top; padding-top: 5px;">
                                <input type="hidden" id="hdnAddressIDMother" runat="server" />
                                <label class="lblMandatory">
                                    <%=GetLabel("Jalan")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtAddressMother" Width="100%" runat="server" TextMode="MultiLine"
                                    Rows="2" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblLink" id="lblZipCodeMother">
                                    <%=GetLabel("Kode Pos")%></label>
                            </td>
                            <td>
                                <input type="hidden" runat="server" id="hdnZipCodeMother" value="" />
                                <asp:TextBox ID="txtZipCodeMother" Width="100%" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Desa / Kelurahan")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtCountyMother" Width="100%" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Kecamatan")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtDistrictMother" Width="100%" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Kota")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtCityMother" Width="100%" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblLink" id="lblProvinceMother">
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
                                            <asp:TextBox ID="txtProvinceCodeMother" Width="100%" runat="server" />
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtProvinceNameMother" ReadOnly="true" Width="100%" runat="server" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblMandatory">
                                    <%=GetLabel("Telepon")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtTelephoneNoMother" CssClass="required" Width="100%" runat="server" />
                            </td>
                        </tr>
                    </table>
                </div>
                <h4 class="h4expanded">
                    <%=GetLabel("Data Ayah")%></h4>
                <div class="containerTblEntryContent">
                    <input type="hidden" id="hdnFatherMRN" value="0" runat="server" />
                    <table class="tblEntryContent" style="width: 100%">
                        <colgroup>
                            <col style="width: 30%" />
                        </colgroup>
                        <tr>
                            <td>
                                <asp:CheckBox ID="chkIsFatherHasMRN" Checked="true" runat="server" /><%:GetLabel("Has Medical No?")%>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblLink" id="lblMRNAyah">
                                    <%=GetLabel("No RM Ayah")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtFatherMRN" Width="100%" runat="server" TabIndex="10" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Sapaan Ayah")%></label>
                            </td>
                            <td>
                                <dxe:ASPxComboBox ID="cboSalutationFather" ClientInstanceName="cboSalutationFather"
                                    Width="100%" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Gelar Depan Ayah")%></label>
                            </td>
                            <td>
                                <dxe:ASPxComboBox ID="cboTitleFather" ClientInstanceName="cboTitleFather" Width="100%"
                                    runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Nama Ayah")%></label>
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
                                            <asp:TextBox ID="txtFirstNameFather" Width="100%" runat="server" />
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtMiddleNameFather" Width="100%" runat="server" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblMandatory">
                                    <%=GetLabel("Nama Belakang Ayah")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtLastNameFather" Width="100%" CssClass="required" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Gelar Belakang Ayah")%></label>
                            </td>
                            <td>
                                <dxe:ASPxComboBox ID="cboSuffixFather" ClientInstanceName="cboSuffixFather" Width="100%"
                                    runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <b>
                                    <label class="lblNormal">
                                        <%=GetLabel("DATA ALAMAT AYAH")%></label></b>
                            </td>
                            <td>
                                <asp:CheckBox ID="chkCopyAlamatIbuKeAyah" runat="server" /><%:GetLabel("Salin Data Alamat Ibu")%>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel" style="vertical-align: top; padding-top: 5px;">
                                <input type="hidden" id="hdnAddressIDFather" runat="server" />
                                <label class="lblMandatory">
                                    <%=GetLabel("Jalan")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtAddressFather" Width="100%" runat="server" TextMode="MultiLine"
                                    Rows="2" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblLink" id="lblZipCodeFather">
                                    <%=GetLabel("Kode Pos")%></label>
                            </td>
                            <td>
                                <input type="hidden" runat="server" id="hdnZipCodeFather" value="" />
                                <asp:TextBox ID="txtZipCodeFather" Width="100%" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Desa / Kelurahan")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtCountyFather" Width="100%" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Kecamatan")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtDistrictFather" Width="100%" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Kota")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtCityFather" Width="100%" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblLink" id="lblProvinceFather">
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
                                            <asp:TextBox ID="txtProvinceCodeFather" Width="100%" runat="server" />
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtProvinceNameFather" ReadOnly="true" Width="100%" runat="server" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblMandatory">
                                    <%=GetLabel("Telepon")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtTelephoneNoFather" CssClass="required" Width="100%" runat="server" />
                            </td>
                        </tr>
                    </table>
                </div>
            </td>
        </tr>
    </table>
</div>
