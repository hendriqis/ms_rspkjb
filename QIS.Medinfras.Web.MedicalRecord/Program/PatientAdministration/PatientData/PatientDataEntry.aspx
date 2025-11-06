<%@ Page Language="C#" MasterPageFile="~/libs/MasterPage/MPEntry.master" AutoEventWireup="true"
    CodeBehind="PatientDataEntry.aspx.cs" Inherits="QIS.Medinfras.Web.MedicalRecord.Program.PatientDataEntry" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<asp:Content ID="Content2" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div class="menuTitle">
        <%=HttpUtility.HtmlEncode(GetPageTitle())%></div>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript">

        $(document).ready(function () {
            var hdnIsMobilePhoneNumeric = $('#<%=hdnIsMobilePhoneNumeric.ClientID %>').val();
            if (hdnIsMobilePhoneNumeric == "1") {
                $('#<%=txtMobilePhone1.ClientID %>').TextNumericOnly();
                $('#<%=txtMobilePhone2.ClientID %>').TextNumericOnly();
            }
        });
        function onLoad() {
            setDatePicker('<%=txtDOB.ClientID %>');
            $('#<%=txtDOB.ClientID %>').datepicker('option', 'maxDate', '0');

            //#region DOB
            $('#<%=txtDOB.ClientID %>').change(function () {
                var age = Methods.getAgeFromDatePickerFormat($(this).val());
                $('#<%=txtAgeInYear.ClientID %>').val(age.years);
                $('#<%=txtAgeInMonth.ClientID %>').val(age.months);
                $('#<%=txtAgeInDay.ClientID %>').val(age.days);

                ////                var dateSelected = $('#<%=txtDOB.ClientID %>').val();
                ////                var from = dateSelected.split("-");
                ////                var f = new Date(from[2], from[1] - 1, from[0]);
                //                //untuk ambil data
                //                $('#<%=hdnDOB.ClientID %>').val(from[2] + "-" + from[1] + "-" + from[0]);


                oncboSalutationValueChanged();
            });

            $('#<%=txtAgeInYear.ClientID %>').change(function () {
                getDOBFromAge();
            });

            $('#<%=txtAgeInMonth.ClientID %>').change(function () {
                getDOBFromAge();
            });

            $('#<%=txtAgeInDay.ClientID %>').change(function () {
                getDOBFromAge();
            });

            function getDOBFromAge() {
                var now = Methods.stringToDate('<%=GetTodayDate() %>');
                var ageInYear = parseInt($('#<%=txtAgeInYear.ClientID %>').val());
                var ageInMonth = parseInt($('#<%=txtAgeInMonth.ClientID %>').val());
                var ageInDay = parseInt($('#<%=txtAgeInDay.ClientID %>').val());

                now.setYear(now.getFullYear() - ageInYear);
                now.setMonth(now.getMonth() - ageInMonth);
                now.setDate(now.getDate() - ageInDay);

                var dateStr = Methods.dateToDatePickerFormat(now);
                $('#<%=txtDOB.ClientID %>').val(dateStr);
            }
            //#endregion

            //#region Popup Search
            //#region Ethnic
            function onGetSCEthnicFilterExpression() {
                var filterExpression = "<%:OnGetSCEthnicFilterExpression() %>";
                return filterExpression;
            }

            $('#lblEthnic.lblLink').click(function () {
                openSearchDialog('stdcode', onGetSCEthnicFilterExpression(), function (value) {
                    $('#<%=txtEthnicCode.ClientID %>').val(value);
                    onTxtEthnicCodeChanged(value);
                });
            });

            $('#<%=txtEthnicCode.ClientID %>').change(function () {
                onTxtEthnicCodeChanged($(this).val());
            });

            function onTxtEthnicCodeChanged(value) {
                var filterExpression = onGetSCEthnicFilterExpression() + " AND StandardCodeID LIKE '%^" + value + "'";
                Methods.getObject('GetStandardCodeList', filterExpression, function (result) {
                    if (result != null)
                        $('#<%=txtEthnicName.ClientID %>').val(result.StandardCodeName);
                    else {
                        $('#<%=txtEthnicCode.ClientID %>').val('');
                        $('#<%=txtEthnicName.ClientID %>').val('');
                    }
                });
            }
            //#endregion

            //#region Employee
            function getEmployeeFilterExpression() {
                var filterExpression = "IsDeleted = 0";
                return filterExpression;
            }

            $('#lblEmployee.lblLink').die('click');
            $('#lblEmployee.lblLink').live('click', function () {
                openSearchDialog('employee', getEmployeeFilterExpression(), function (value) {
                    $('#<%=txtEmployeeCode.ClientID %>').val(value);
                    onPatientEntryTxtEmployeeCodeChanged(value);
                });
            });

            $('#<%=txtEmployeeCode.ClientID %>').change(function () {
                onPatientEntryTxtEmployeeCodeChanged($(this).val());
            });

            function onPatientEntryTxtEmployeeCodeChanged(value) {
                var filterExpression = getEmployeeFilterExpression() + " AND EmployeeCode = '" + value + "'";
                Methods.getObject('GetEmployeeList', filterExpression, function (result) {
                    if (result != null) {
                        $('#<%=hdnEmployeeID.ClientID %>').val(result.EmployeeID);
                        $('#<%=txtEmployeeName.ClientID %>').val(result.FullName);
                    }
                    else {
                        $('#<%=hdnEmployeeID.ClientID %>').val('');
                        $('#<%=txtEmployeeCode.ClientID %>').val('');
                        $('#<%=txtEmployeeName.ClientID %>').val('');
                    }
                });
            }
            //#endregion

            //#region Province
            function onGetSCProvinceFilterExpression() {
                var filterExpression = "<%:OnGetSCProvinceFilterExpression() %>";
                return filterExpression;
            }

            $('#<%=lblProvince.ClientID%>.lblLink').live('click', function (evt) {
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

            //#region Office Province
            $('#<%=lblOfficeProvince.ClientID%>.lblLink').live('click', function (evt) {
                openSearchDialog('stdcode', onGetSCProvinceFilterExpression(), function (value) {
                    $('#<%=txtPatientJobOfficeProvinceCode.ClientID %>').val(value);
                    onTxtOfficeProvinceCodeChanged(value);
                });
            });

            $('#<%=txtPatientJobOfficeProvinceCode.ClientID %>').change(function () {
                onTxtOfficeProvinceCodeChanged($(this).val());
            });

            function onTxtOfficeProvinceCodeChanged(value) {
                var filterExpression = onGetSCProvinceFilterExpression() + " AND StandardCodeID LIKE '%^" + value + "'";
                Methods.getObject('GetStandardCodeList', filterExpression, function (result) {
                    if (result != null)
                        $('#<%=txtPatientJobOfficeProvinceName.ClientID %>').val(result.StandardCodeName);
                    else {
                        $('#<%=txtPatientJobOfficeProvinceCode.ClientID %>').val('');
                        $('#<%=txtPatientJobOfficeProvinceName.ClientID %>').val('');
                    }
                });
            }
            //#endregion

            //#region Zip Code
            $('#<%=lblZipCode.ClientID %>').live('click', function (evt) {
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
                            $('#<%=txtMobilePhone1.ClientID %>').val(result.NationPhoneCode);
                        }
                        else {
                            $('#<%=hdnZipCode.ClientID %>').val('');
                            $('#<%=txtZipCode.ClientID %>').val('');
                            $('#<%=txtCity.ClientID %>').val('');
                            $('#<%=txtCounty.ClientID %>').val('');
                            $('#<%=txtDistrict.ClientID %>').val('');
                            $('#<%=txtProvinceCode.ClientID %>').val('');
                            $('#<%=txtProvinceName.ClientID %>').val('');
                            $('#<%=txtMobilePhone1.ClientID %>').val('');
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
                    $('#<%=txtMobilePhone1.ClientID %>').val('');
                }
            }
            //#endregion

            //#region Office Zip Code
            $('#lblOfficeZipCode.lblLink').click(function () {
                openSearchDialog('zipcodes', 'IsDeleted = 0', function (value) {
                    onTxtOfficeZipCodeChanged(value);
                });
            });

            $('#<%=txtPatientJobOfficeZipCode.ClientID %>').change(function () {
                onTxtOfficeZipCodeChangedValue($(this).val());
            });

            function onTxtOfficeZipCodeChanged(value) {
                if (value != '') {
                    var filterExpression = "ID = '" + value + "' AND IsDeleted = 0";
                    Methods.getObject('GetvZipCodesList', filterExpression, function (result) {
                        if (result != null) {
                            $('#<%=hdnOfficeZipCode.ClientID %>').val(result.ID);
                            $('#<%=txtPatientJobOfficeZipCode.ClientID %>').val(result.ZipCode);
                            $('#<%=txtPatientJobOfficeCity.ClientID %>').val(result.City);
                            $('#<%=txtPatientJobOfficeCounty.ClientID %>').val(result.County);
                            $('#<%=txtPatientJobOfficeDistrict.ClientID %>').val(result.District);
                            $('#<%=txtPatientJobOfficeProvinceCode.ClientID %>').val(result.GCProvince.split('^')[1]);
                            $('#<%=txtPatientJobOfficeProvinceName.ClientID %>').val(result.Province);
                        }
                        else {
                            $('#<%=hdnOfficeZipCode.ClientID %>').val('');
                            $('#<%=txtPatientJobOfficeZipCode.ClientID %>').val('');
                            $('#<%=txtPatientJobOfficeCity.ClientID %>').val('');
                            $('#<%=txtPatientJobOfficeCounty.ClientID %>').val('');
                            $('#<%=txtPatientJobOfficeDistrict.ClientID %>').val('');
                            $('#<%=txtPatientJobOfficeProvinceCode.ClientID %>').val('');
                            $('#<%=txtPatientJobOfficeProvinceName.ClientID %>').val('');
                        }
                    });
                }
                else {
                    $('#<%=hdnOfficeZipCode.ClientID %>').val('');
                    $('#<%=txtPatientJobOfficeZipCode.ClientID %>').val('');
                    $('#<%=txtPatientJobOfficeCity.ClientID %>').val('');
                    $('#<%=txtPatientJobOfficeCounty.ClientID %>').val('');
                    $('#<%=txtPatientJobOfficeDistrict.ClientID %>').val('');
                    $('#<%=txtPatientJobOfficeProvinceCode.ClientID %>').val('');
                    $('#<%=txtPatientJobOfficeProvinceName.ClientID %>').val('');
                }
            }

            function onTxtOfficeZipCodeChangedValue(value) {
                if (value != '') {
                    var filterExpression = "ZipCode = '" + value + "' AND IsDeleted = 0";
                    Methods.getObject('GetvZipCodesList', filterExpression, function (result) {
                        if (result != null) {
                            $('#<%=hdnOfficeZipCode.ClientID %>').val(result.ID);
                            $('#<%=txtPatientJobOfficeZipCode.ClientID %>').val(result.ZipCode);
                            $('#<%=txtPatientJobOfficeCity.ClientID %>').val(result.City);
                            $('#<%=txtPatientJobOfficeCounty.ClientID %>').val(result.County);
                            $('#<%=txtPatientJobOfficeDistrict.ClientID %>').val(result.District);
                            $('#<%=txtPatientJobOfficeProvinceCode.ClientID %>').val(result.GCProvince.split('^')[1]);
                            $('#<%=txtPatientJobOfficeProvinceName.ClientID %>').val(result.Province);
                        }
                        else {
                            $('#<%=hdnOfficeZipCode.ClientID %>').val('');
                            $('#<%=txtPatientJobOfficeZipCode.ClientID %>').val('');
                            $('#<%=txtPatientJobOfficeCity.ClientID %>').val('');
                            $('#<%=txtPatientJobOfficeCounty.ClientID %>').val('');
                            $('#<%=txtPatientJobOfficeDistrict.ClientID %>').val('');
                            $('#<%=txtPatientJobOfficeProvinceCode.ClientID %>').val('');
                            $('#<%=txtPatientJobOfficeProvinceName.ClientID %>').val('');
                        }
                    });
                }
                else {
                    $('#<%=hdnOfficeZipCode.ClientID %>').val('');
                    $('#<%=txtPatientJobOfficeZipCode.ClientID %>').val('');
                    $('#<%=txtPatientJobOfficeCity.ClientID %>').val('');
                    $('#<%=txtPatientJobOfficeCounty.ClientID %>').val('');
                    $('#<%=txtPatientJobOfficeDistrict.ClientID %>').val('');
                    $('#<%=txtPatientJobOfficeProvinceCode.ClientID %>').val('');
                    $('#<%=txtPatientJobOfficeProvinceName.ClientID %>').val('');
                }
            }
            //#endregion

            //#region Education
            function onGetSCEducationFilterExpression() {
                var filterExpression = "<%:OnGetSCEducationFilterExpression() %>";
                return filterExpression;
            }

            $('#<%=lblEducation.ClientID %>').live('click', function (evt) {
                openSearchDialog('stdcode', onGetSCEducationFilterExpression(), function (value) {
                    $('#<%=txtEducationCode.ClientID %>').val(value);
                    onTxtEducationCodeChanged(value);
                });
            });

            $('#<%=txtEducationCode.ClientID %>').change(function () {
                onTxtEducationCodeChanged($(this).val());
            });

            function onTxtEducationCodeChanged(value) {
                var filterExpression = onGetSCEducationFilterExpression() + " AND StandardCodeID LIKE '%^" + value + "'";
                Methods.getObject('GetStandardCodeList', filterExpression, function (result) {
                    if (result != null)
                        $('#<%=txtEducationName.ClientID %>').val(result.StandardCodeName);
                    else {
                        $('#<%=txtEducationCode.ClientID %>').val('');
                        $('#<%=txtEducationName.ClientID %>').val('');
                    }
                });
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

            //#region Language
            function onGetSCLanguageFilterExpression() {
                var filterExpression = "<%:OnGetSCLanguageFilterExpression() %>";
                return filterExpression;
            }

            $('#lblLanguage.lblLink').click(function () {
                openSearchDialog('stdcode', onGetSCLanguageFilterExpression(), function (value) {
                    $('#<%=txtlanguageCode.ClientID %>').val(value);
                    onTxtLanguageCodeChanged(value);
                });
            });

            $('#<%=txtlanguageCode.ClientID %>').change(function () {
                onTxtLanguageCodeChanged($(this).val());
            });

            function onTxtLanguageCodeChanged(value) {
                var filterExpression = onGetSCLanguageFilterExpression() + " AND StandardCodeID LIKE '%^" + value + "'";
                Methods.getObject('GetStandardCodeList', filterExpression, function (result) {
                    if (result != null)
                        $('#<%=txtlanguageName.ClientID %>').val(result.StandardCodeName);
                    else {
                        $('#<%=txtlanguageCode.ClientID %>').val('');
                        $('#<%=txtlanguageName.ClientID %>').val('');
                    }
                });
            }
            //#endregion

            //#region Marital Status
            function onGetSCMaritalStatusFilterExpression() {
                var filterExpression = "<%:OnGetSCMaritalStatusFilterExpression() %>";
                return filterExpression;
            }

            $('#lblMaritalStatus.lblLink').click(function () {
                openSearchDialog('stdcode', onGetSCMaritalStatusFilterExpression(), function (value) {
                    $('#<%=txtMaritalStatusCode.ClientID %>').val(value);
                    onTxtMaritalStatusCodeChanged(value);
                });
            });

            $('#<%=txtMaritalStatusCode.ClientID %>').change(function () {
                onTxtMaritalStatusCodeChanged($(this).val());
            });

            function onTxtMaritalStatusCodeChanged(value) {
                var filterExpression = onGetSCMaritalStatusFilterExpression() + " AND StandardCodeID LIKE '%^" + value + "'";
                Methods.getObject('GetStandardCodeList', filterExpression, function (result) {
                    if (result != null)
                        $('#<%=txtMaritalStatusName.ClientID %>').val(result.StandardCodeName);
                    else {
                        $('#<%=txtMaritalStatusCode.ClientID %>').val('');
                        $('#<%=txtMaritalStatusName.ClientID %>').val('');
                    }
                });
            }
            //#endregion

            //#region Patient Category
            function onGetSCPatientCategoryFilterExpression() {
                var filterExpression = "<%:OnGetSCPatientCategoryFilterExpression() %>";
                return filterExpression;
            }

            $('#lblPatientCategory.lblLink').click(function () {
                openSearchDialog('stdcode', onGetSCPatientCategoryFilterExpression(), function (value) {
                    $('#<%=txtPatientCategoryCode.ClientID %>').val(value);
                    onTxtPatientCategoryCodeChanged(value);
                });
            });

            $('#<%=txtPatientCategoryCode.ClientID %>').change(function () {
                onTxtPatientCategoryCodeChanged($(this).val());
            });

            function onTxtPatientCategoryCodeChanged(value) {
                var filterExpression = onGetSCPatientCategoryFilterExpression() + " AND StandardCodeID LIKE '%^" + value + "'";
                Methods.getObject('GetStandardCodeList', filterExpression, function (result) {
                    if (result != null) {
                        $('#<%=txtPatientCategoryName.ClientID %>').val(result.StandardCodeName);

                        if (result.TagProperty != null) {
                            var tempParam = result.TagProperty.split('|');
                            var isEmployee = tempParam[0].substring(tempParam[0].length - 1, tempParam[0].length);
                            if (isEmployee == "1") {
                                $('#<%=txtEmployeeCode.ClientID %>').removeAttr('readonly');
                                $('#lblEmployee').attr('class', 'lblLink');
                                $('#<%=hdnEmployeeID.ClientID %>').val('');
                                $('#<%=txtEmployeeCode.ClientID %>').val('');
                                $('#<%=txtEmployeeName.ClientID %>').val('');
                            } else {
                                $('#lblEmployee').attr('class', 'lblDisabled');
                                $('#<%=hdnEmployeeID.ClientID %>').val('');
                                $('#<%=txtEmployeeCode.ClientID %>').val('');
                                $('#<%=txtEmployeeName.ClientID %>').val('');
                            }
                        }
                        else {
                            $('#lblEmployee').attr('class', 'lblDisabled');
                            $('#<%=hdnEmployeeID.ClientID %>').val('');
                            $('#<%=txtEmployeeCode.ClientID %>').val('');
                            $('#<%=txtEmployeeName.ClientID %>').val('');
                        }
                    }
                    else {
                        $('#<%=txtPatientCategoryCode.ClientID %>').val('');
                        $('#<%=txtPatientCategoryName.ClientID %>').val('');
                    }
                });
            }
            //#endregion

            //#region Payer Company
            function getPayerCompanyFilterExpression() {
                var filterExpression = "GCCustomerType = '" + cboPayer.GetValue() + "' AND IsDeleted = 0 AND IsActive = 1 AND IsBlacklist = 0";
                return filterExpression;
            }

            $('#lblPayerCompany.lblLink').click(function () {
                openSearchDialog('payer', getPayerCompanyFilterExpression(), function (value) {
                    $('#<%=txtPayerCompanyCode.ClientID %>').val(value);
                    onPatientEntryTxtPayerCompanyCodeChanged(value);
                });
            });

            $('#<%=txtPayerCompanyCode.ClientID %>').change(function () {
                onPatientEntryTxtPayerCompanyCodeChanged($(this).val());
            });

            function onPatientEntryTxtPayerCompanyCodeChanged(value) {
                var filterExpression = getPayerCompanyFilterExpression() + " AND BusinessPartnerCode = '" + value + "'";
                Methods.getObject('GetvCustomerList', filterExpression, function (result) {
                    if (result != null) {
                        $('#<%=hdnPayerID.ClientID %>').val(result.BusinessPartnerID);
                        $('#<%=txtPayerCompanyName.ClientID %>').val(result.BusinessPartnerName);
                    }
                    else {
                        $('#<%=hdnPayerID.ClientID %>').val('');
                        $('#<%=txtPayerCompanyCode.ClientID %>').val('');
                        $('#<%=txtPayerCompanyName.ClientID %>').val('');
                    }
                });
            }
            //#endregion

            //#region Patient Job
            function onGetSCPatientJobFilterExpression() {
                var filterExpression = "<%:OnGetSCPatientJobFilterExpression() %>";
                return filterExpression;
            }

            $('#<%=lblPatientJob.ClientID %>').live('click', function (evt) {
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
            //#endregion

            registerCollapseExpandHandler();
        }

        function onCboPayerValueChanged(s) {
            setTblPayerCompanyVisibility();
            $('#<%=hdnPayerID.ClientID %>').val('');
            $('#<%=txtPayerCompanyCode.ClientID %>').val('');
            $('#<%=txtPayerCompanyName.ClientID %>').val('');
        }

        function setTblPayerCompanyVisibility() {
            if (cboPayer.GetValue() == Constant.CustomerType.PERSONAL)
                $('#<%=trPayerCompany.ClientID %>').hide();
            else
                $('#<%=trPayerCompany.ClientID %>').show();
        }

        $('#<%:chkIsVIP.ClientID %>').live('change', function () {
            if ($(this).is(':checked')) {
                $('#<%=tdVIPGroup.ClientID %>').removeAttr('style');
            }
            else {
                $('#<%=tdVIPGroup.ClientID %>').attr('style', 'display:none');
                $('#<%=trVIPGroupOther.ClientID %>').attr('style', 'display:none');
            }
        });

        function oncboVIPGroupChanged() {
            if (cboVIPGroup.GetValue() != 'X289^999') {
                $('#<%=trVIPGroupOther.ClientID %>').attr('style', 'display:none');
                $('#<%=txtOtherVIPGroup.ClientID %>').val('');
            }
            else {
                $('#<%=trVIPGroupOther.ClientID %>').removeAttr('style');
            }
        }

        $('#<%:chkIsBlackList.ClientID %>').live('change', function () {
            if ($(this).is(':checked')) {
                $('#<%=tdGCBlacklistReason.ClientID %>').removeAttr('style');
            }
            else {
                $('#<%=tdGCBlacklistReason.ClientID %>').attr('style', 'display:none');
                $('#<%=trOtherBlackListReason.ClientID %>').attr('style', 'display:none');
            }
        });

        function onCboGCBlacklistReasonValueChanged() {
            var defaultOther = Constant.PatientBlackListReason.OTHER;
            if (cboGCBlacklistReason.GetValue() == defaultOther) {
                $('#<%=trOtherBlackListReason.ClientID %>').removeAttr('style');
            }
            else {
                $('#<%=trOtherBlackListReason.ClientID %>').attr('style', 'display:none');
                $('#<%=txtOtherBlackListReason.ClientID %>').val('');
            }
        }

        $('#<%:chkIsHasCommunicationRestriction.ClientID %>').live('change', function () {
            if ($(this).is(':checked')) {
                $('#<%=tdCommunication.ClientID %>').removeAttr('style');
            }
            else {
                $('#<%=tdCommunication.ClientID %>').attr('style', 'display:none');
            }
        });

        $('#<%:chkIsHasPhysicalLimitation.ClientID %>').live('change', function () {
            if ($(this).is(':checked')) {
                $('#<%=tdPhysicalLimitation.ClientID %>').removeAttr('style');
            }
            else {
                $('#<%=tdPhysicalLimitation.ClientID %>').attr('style', 'display:none');
            }
        });

        function oncboSalutationValueChanged() {
            var ageInYear = parseInt($('#<%=txtAgeInYear.ClientID %>').val());
            var salutation = cboSalutation.GetValue();
            var gender;
            var tagProperty;

            if (cboGender.GetValue() != null) {
                if (cboGender.GetValue() == Constant.Gender.MALE) {
                    gender = 'M';
                    $('#<%=chkIsPregnant.ClientID %>').attr("disabled", true);
                    $('#<%=chkIsPregnant.ClientID %>').prop('checked', false);
                }
                else if (cboGender.GetValue() == Constant.Gender.FEMALE) {
                    gender = 'F';
                    $('#<%=chkIsPregnant.ClientID %>').removeAttr('disabled');
                }
                else if (cboGender.GetValue() == Constant.Gender.UNSPECIFIED) {
                    gender = 'U';
                    $('#<%=chkIsPregnant.ClientID %>').attr("disabled", true);
                    $('#<%=chkIsPregnant.ClientID %>').prop('checked', false);
                }
            }
            else {
                $('#<%=chkIsPregnant.ClientID %>').attr("disabled", true);
                $('#<%=chkIsPregnant.ClientID %>').prop('checked', false);            
            }

            if (salutation != null) {
                var filterExpression = "StandardCodeID = '" + salutation + "'";
                Methods.getObject('GetStandardCodeList', filterExpression, function (result) {
                    if (result != null) {
                        tagProperty = result.TagProperty.split("|");

                        if (tagProperty[1] != 'MF') {
                            if (tagProperty[1] == gender) {
                                if (tagProperty[3] != '~') {
                                    if (ageInYear < parseInt(tagProperty[2]) || ageInYear > parseInt(tagProperty[3])) {
                                        cboSalutation.SetValue('');
                                    }
                                }
                                else if (tagProperty[3] == '~') {
                                    if (ageInYear < parseInt(tagProperty[2])) {
                                        cboSalutation.SetValue('');
                                    }
                                }
                            }
                            else {
                                cboSalutation.SetValue('');
                            }
                        }
                        else {
                            if (tagProperty[3] != '~') {
                                if (ageInYear < parseInt(tagProperty[2]) || ageInYear > parseInt(tagProperty[3])) {
                                    cboSalutation.SetValue('');
                                }
                            }
                            else if (tagProperty[3] == '~') {
                                if (ageInYear < parseInt(tagProperty[2])) {
                                    cboSalutation.SetValue('');
                                }
                            }
                        }
                    }
                    else {
                        cboSalutation.SetValue('');
                    }
                });
            }
        }

        function onBeforeSaveNewRecord(param) {
            var isBlokDoubleRM = $('#<%=hdnIsBlockDoublePatientData.ClientID %>').val();
            var isValidate = true;
            var mrn = $('#<%=hdnMRN.ClientID %>').val();
            var fullName = getFullNameText();
            var dateSelected = $('#<%=txtDOB.ClientID %>').val();
            var from = dateSelected.split("-");
            var f = new Date(from[2], from[1] - 1, from[0]);

            var DOB = from[2] + "-" + from[1] + "-" + from[0];

            var filterExpression = "PatientName='" + fullName.trim() + "' AND DateOfBirth='" + DOB + "' AND GCGender = '" + cboGender.GetValue() + "' AND IsDeleted=0 ";


            var SSN = $('#<%=txtIdentityCardNo.ClientID %>').val();

            if (SSN != "") {
                filterExpression += "  AND SSN ='" + SSN + "'";
            }
            if (mrn != "") {
                filterExpression += "   AND MRN <> '" + mrn + "' ";
            }
            Methods.getObject('GetvPatientList', filterExpression, function (result) {
                if (result != null) {
                    if (isBlokDoubleRM == "1") {
                        messageDoublePatient = 'Ditemukan ada pasien dengan data yang sama :<br/>' +
                                                'No.RM : ' + result.MedicalNo + '<br/>Nama Lengkap : ' + result.PatientName + '<br/> No Kartu Identitas  : ' + result.SSN + '<br/>Tanggal Lahir : ' + Methods.getJSONDateValue(result.DateOfBirth) + '<br/>Jenis Kelamin : ' + result.Gender + '<br/>Alamat : ' + result.HomeAddress + '<br/>';
                        displayMessageBox('Failed', messageDoublePatient);
                        isValidate = false;
                    }
                    else {
                        isValidate = true;
                    }
                }
            });

            return isValidate;
        }

        function onBeforeSaveCloseRecord(param) {
            var isBlokDoubleRM = $('#<%=hdnIsBlockDoublePatientData.ClientID %>').val();
            var isValidate = true;
            var mrn = $('#<%=hdnMRN.ClientID %>').val();
            var fullName = getFullNameText();
            var dateSelected = $('#<%=txtDOB.ClientID %>').val();
            var from = dateSelected.split("-");
            var f = new Date(from[2], from[1] - 1, from[0]);

            var DOB = from[2] + "-" + from[1] + "-" + from[0];

            var filterExpression = "PatientName='" + fullName.trim() + "' AND DateOfBirth='" + DOB + "' AND GCGender = '" + cboGender.GetValue() + "' AND IsDeleted=0 ";


            var SSN = $('#<%=txtIdentityCardNo.ClientID %>').val();

            if (SSN != "") {
                filterExpression += "  AND SSN ='" + SSN + "'";
            }
            if (mrn != "") {
                filterExpression += "   AND MRN <> '" + mrn + "' ";
            }
            Methods.getObject('GetvPatientList', filterExpression, function (result) {
                if (result != null) {
                    if (isBlokDoubleRM == "1") {
                        messageDoublePatient = 'Ditemukan ada pasien dengan data yang sama :<br/>' +
                                                'No.RM : ' + result.MedicalNo + '<br/>Nama Lengkap : ' + result.PatientName + '<br/> No Kartu Identitas  : ' + result.SSN + '<br/>Tanggal Lahir : ' + Methods.getJSONDateValue(result.DateOfBirth) + '<br/>Jenis Kelamin : ' + result.Gender + '<br/>Alamat : ' + result.HomeAddress + '<br/>';
                        displayMessageBox('Failed', messageDoublePatient);
                        isValidate = false;
                    }
                    else {
                        isValidate = true;
                    }
                }
            });
            return isValidate;
        }

        /*
        function onBeforeSaveRecordPatientValidation(param) {
        //            var dateSelected = $('#<%=txtDOB.ClientID %>').val();
        //                        var from = dateSelected.split("-");
        //                        var f = new Date(from[2], from[1] - 1, from[0]);

        //                        var dob = from[2] + "-" + from[1] + "-" + from[0];
        //            /////var dob = Methods.getDatePickerDate($('#<%=txtDOB.ClientID %>').val());
        //           
        //            var fullName = getFullNameText();
        //            var IsExis = true;
        //            var mrn = $('#<%=hdnMRN.ClientID %>').val();
        //          
        //            if(mrn == ""){
        //                 var filterExpression = "PatientName='" + fullName.trim() + "' AND DateOfBirth='" + dob + "' AND GCGender = '" + cboGender.GetValue() + "'";
        //                     result = filterExpression;
        //            }else{
        //                var filterExpression = "PatientName='" + fullName.trim() + "' AND DateOfBirth='" + dob + "' AND GCGender = '" + cboGender.GetValue() + "' AND MRN <> "+mrn+" ";
        //                     result = filterExpression;
        //            }
        //           
        //         
        //            
        //            return result;

        }
        */
        function getFullNameText() {
            var firstName = $('#<%=txtFirstName.ClientID %>').val();
            var middleName = $('#<%=txtMiddleName.ClientID %>').val();
            var familyName = $('#<%=txtFamilyName.ClientID %>').val();
            var cardName = "";

            if (firstName != "" && firstName != null) {
                if (middleName != "" && middleName != null) {
                    cardName = firstName + " " + middleName + " " + familyName;
                } else {
                    cardName = firstName + " " + familyName;
                }
            } else {
                cardName = familyName;
            }

            return cardName;
        }
        function onAfterSaveCloseSuccess() {

        }
        ///#region Guest
        $(document).ready(function () {
            $('#<%=btnGuest.ClientID %>').live('click', function () {
                var filterexpresion = "MRN IS  Null and IsDeleted=0";
                openSearchDialog('guest1', filterexpresion, function (value) {
                    onGuestChanged(value);
                });
            });
            function onGuestChanged(value) {
                var filterExpression = "GuestID = '" + value + "' AND IsDeleted=0";
                Methods.getObject('GetvGuestList', filterExpression, function (result) {
                    if (result != null) {
                        onBidingGuest(result);
                    }
                });
            }

            function onBidingGuest(row) {
                $('#<%=hdnGuestID.ClientID %>').val(row.GuestID);
                $('#<%=txtGuestNo.ClientID %>').val(row.GuestNo);
                $('#<%=txtFirstName.ClientID %>').val(row.FirstName);
                $('#<%=txtMiddleName.ClientID %>').val(row.MiddleName);
                $('#<%=txtFamilyName.ClientID %>').val(row.LastName);

                cboGender.SetValue(row.GCGender);
                cboBloodType.SetValue(row.GCBloodType);
                cboBloodRhesus.SetValue(row.BloodRhesus);
                cboSalutation.SetValue(row.GCSalutation);
                cboTitle.SetValue(row.GCTitle);
                cboSuffix.SetValue(row.GCSuffix);
                cboIdentityCardType.SetValue(row.GCIdentityNoType);
                $('#<%=txtIdentityCardNo.ClientID %>').val(row.SSN);

                $('#<%=txtBirthPlace.ClientID %>').val(row.CityOfBirth);

                $('#<%=txtDOB.ClientID %>').val(row.DateOfBirthInStringDatePickerFormat);
                if (row.DateOfBirthInStringDatePickerFormat != "") {
                    var age = Methods.getAgeFromDatePickerFormat(row.DateOfBirthInStringDatePickerFormat);
                    $('#<%=txtAgeInYear.ClientID %>').val(age.years);
                    $('#<%=txtAgeInMonth.ClientID %>').val(age.months);
                    $('#<%=txtAgeInDay.ClientID %>').val(age.days);
                }

                if (row.GCReligion != "") {
                    var ReligionData = row.GCReligion.split('^');
                    $('#<%=txtReligionCode.ClientID %>').val(ReligionData[1]);
                    $('#<%=txtReligionName.ClientID %>').val(row.Religion);
                }

                if (row.GCEducation != "") {
                    var EducationData = row.GCEducation.split('^');
                    $('#<%=txtEducationCode.ClientID %>').val(EducationData[1]);
                    $('#<%=txtEducationName.ClientID %>').val(row.Education);
                }
                if (row.GCLanguage != "") {
                    var LanguageData = row.GCLanguage.split('^');
                    $('#<%=txtlanguageCode.ClientID %>').val(LanguageData[1]);
                    $('#<%=txtlanguageName.ClientID %>').val(row.Language);
                }
                if (row.GCEthnic != "") {
                    var EthnicData = row.GCEthnic.split('^');
                    $('#<%=txtEthnicCode.ClientID %>').val(EthnicData[1]);
                    $('#<%=txtEthnicName.ClientID %>').val(row.Ethnic);
                }
                if (row.GCMaritalStatus != null) {

                    var MaritalStatusData = row.GCMaritalStatus.split('^');
                    $('#<%=txtMaritalStatusCode.ClientID %>').val(MaritalStatusData[1]);
                    $('#<%=txtMaritalStatusName.ClientID %>').val(row.MaritalStatus);
                }
                if (row.GCNationality != null) {
                    var NationalityData = row.GCNationality.split('^');

                    $('#<%=txtNationalityCode.ClientID %>').val(NationalityData[1]);
                    $('#<%=txtNationalityName.ClientID %>').val(row.Nationality);

                }
                $('#<%=txtAddress.ClientID %>').val(row.StreetName);
                $('#<%=txtCounty.ClientID %>').val(row.County);
                $('#<%=txtDistrict.ClientID %>').val(row.District);
                $('#<%=txtCounty.ClientID %>').val(row.County);
                $('#<%=txtCity.ClientID %>').val(row.City);

                $('#<%=txtTelephoneNo1.ClientID %>').val(row.PhoneNo);
                $('#<%=txtMobilePhone1.ClientID %>').val(row.MobilePhoneNo);
                $('#<%=txtEmail.ClientID %>').val(row.EmailAddress);


            }

            $("#btnGetIHSNumber").on("click", function (e) {
                e.preventDefault();
                if ($('#<%=txtIdentityCardNo.ClientID %>').val() == '')
                    alert("Nomor Identitas Pasien (NIK) harus diisi!");
                else {
                    var NIK = $('#<%=txtIdentityCardNo.ClientID %>').val();
                    try {
                        IHSService.getIHSNumberByNIK(NIK, function (result) {
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
        });
        ///#endregion 
    </script>
    <input type="hidden" id="hdnPageTitle" runat="server" />
    <input type="hidden" runat="server" id="hdnMRN" value="" />
    <input type="hidden" runat="server" id="hdnZipCode" value="" />
    <input type="hidden" runat="server" id="hdnOfficeZipCode" value="" />
    <input type="hidden" id="hdnPayerID" value="" runat="server" />
    <input type="hidden" id="hdnIsBridgingToMedinfrasMobileApps" value="" runat="server" />
    <input type="hidden" runat="server" id="hdnIsBlockDoublePatientData" value="" />
    <input type="hidden" id="hdnDOB" value="" runat="server" />
    <input type="hidden" runat="server" id="hdnGuestID" value="0" />
    <input type="hidden" runat="server" id="hdnIsMobilePhoneNumeric" value="0" />
    <input type="hidden" runat="server" id="hdnIsAddressUseZipCode" value="" />
    <table class="tblContentArea">
        <colgroup>
            <col style="width: 49%" />
            <col style="width: 3px" />
            <col style="width: 49%" />
        </colgroup>
        <tr>
            <td style="padding: 5px; vertical-align: top;">
                <h4 class="h4expanded">
                    <%=GetLabel("Data Pasien")%></h4>
                <div class="containerTblEntryContent">
                    <table class="tblEntryContent" style="width: 100%">
                        <colgroup>
                            <col style="width: 35%" />
                            <col style="width: 65%" />
                        </colgroup>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("No.RM")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtMRN" Width="100%" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label>
                                    <%=GetLabel("No. Pengunjung")%></label>
                            </td>
                            <td>
                                <table border="0" cellpadding="0" cellspacing="1" width="100%">
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtGuestNo" Width="100%" runat="server" />
                                        </td>
                                        <td style="padding-left : 10px">
                                            <input type="button" value="Salin Data Pengunjung" id="btnGuest" runat="server" class="btnGuest w3-btn1 w3-hover-blue" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr id="trInformationMergeMRN" style="display: none" runat="server">
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Gabungan Dari")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtMRNMerge" Width="100%" ReadOnly="true" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("No Peserta BPJS")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtNHSRegistrationNo" Width="100%" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("No RM E-Klaim")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtEKlaimMedicalNo" Width="100%" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("No Register SITB")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtSITBRegisterNo" Width="100%" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal" title="No. Rekam Medis Pasien di Platform SATUSEHAT">
                                    <%=GetLabel("No Peserta Inhealth")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtInhealthParticipantNo" Width="100%" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Tipe Kartu Identitas")%></label>
                            </td>
                            <td>
                                <dxe:ASPxComboBox ID="cboIdentityCardType" Width="100%" runat="server" ClientInstanceName="cboIdentityCardType" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblMandatory lblNormal" id="lblSSN" runat="server">
                                    <%=GetLabel("No Kartu Identitas")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtIdentityCardNo" Width="100%" MaxLength="20" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("No Kartu Keluarga")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtFamilyCardNo" Width="100%" MaxLength="20" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblMandatory lblNormal" id="lblSalutation" runat="server">
                                    <%=GetLabel("Sapaan")%></label>
                            </td>
                            <td>
                                <dxe:ASPxComboBox ID="cboSalutation" ClientInstanceName="cboSalutation" Width="100%"
                                    runat="server">
                                    <ClientSideEvents ValueChanged="function(s,e) { oncboSalutationValueChanged(); }" />
                                </dxe:ASPxComboBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Gelar Depan")%></label>
                            </td>
                            <td>
                                <dxe:ASPxComboBox ID="cboTitle" ClientInstanceName="cboTitle" Width="100%" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblMandatory lblNormal" id="lblPatientName" runat="server">
                                    <%=GetLabel("Nama Pasien")%></label>
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
                                            <asp:TextBox ID="txtFirstName" Width="100%" MaxLength="50" runat="server" />
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtMiddleName" Width="100%" MaxLength="50" runat="server" />
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
                                <asp:TextBox ID="txtFamilyName" Width="100%" MaxLength="50" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Nama Pasien (Khusus)")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtName2" Width="100%" MaxLength="200" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Nama Panggilan")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtPreferredName" Width="100%" MaxLength="35" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Gelar Belakang")%></label>
                            </td>
                            <td>
                                <dxe:ASPxComboBox ID="cboSuffix" ClientInstanceName="cboSuffix" Width="100%" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Jenis Kelamin")%></label>
                            </td>
                            <td>
                                <dxe:ASPxComboBox ID="cboGender" ClientInstanceName="cboGender" Width="100%" runat="server">
                                    <ClientSideEvents ValueChanged="function(s,e) { oncboSalutationValueChanged(); }" />
                                </dxe:ASPxComboBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label id="lblEthnic" class="lblLink">
                                    <%=GetLabel("Suku")%></label>
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
                                            <asp:TextBox ID="txtEthnicCode" Width="100%" runat="server" />
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtEthnicName" Width="100%" runat="server" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Golongan Darah")%></label>
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
                                            <dxe:ASPxComboBox ID="cboBloodType" ClientInstanceName="cboBloodType" Width="100%"
                                                runat="server" />
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            <dxe:ASPxComboBox ID="cboBloodRhesus" ClientInstanceName="cboBloodRhesus" Width="100%"
                                                runat="server" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Tempat Lahir")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtBirthPlace" Width="100%" MaxLength="50" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblMandatory">
                                    <%=GetLabel("Tanggal Lahir")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtDOB" Width="120px" runat="server" CssClass="datepicker" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblMandatory">
                                    <%=GetLabel("Umur")%>
                                    (yyyy-MM-dd)</label>
                            </td>
                            <td>
                                <table style="width: 100%" cellpadding="0" cellspacing="0">
                                    <colgroup>
                                        <col style="width: 32%" />
                                        <col style="width: 3px" />
                                        <col style="width: 32%" />
                                        <col style="width: 3px" />
                                        <col style="width: 32%" />
                                    </colgroup>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtAgeInYear" CssClass="number" Width="100%" runat="server" />
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtAgeInMonth" CssClass="number" Width="100%" runat="server" />
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtAgeInDay" CssClass="number" Width="100%" runat="server" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </div>
                <h4 class="h4expanded">
                    <%=GetLabel("Data Alamat Pasien")%></h4>
                <div class="containerTblEntryContent">
                    <table class="tblEntryContent" style="width: 100%">
                        <colgroup>
                            <col style="width: 35%" />
                            <col style="width: 65%" />
                        </colgroup>
                        <tr>
                            <td class="tdLabel" style="vertical-align: top; padding-top: 5px;">
                                <label class="lblMandatory">
                                    <%=GetLabel("Jalan")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtAddress" Width="100%" MaxLength="500" runat="server" TextMode="MultiLine"
                                    Rows="2" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblMandatory lblLink" id="lblZipCode" runat="server">
                                    <%=GetLabel("Kode Pos")%></label>
                            </td>
                            <td>
                                <table style="width: 100%" cellpadding="0" cellspacing="0">
                                    <colgroup>
                                        <col style="width: 40%" />
                                        <col style="width: 3px" />
                                        <col />
                                    </colgroup>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtZipCode" Width="100%" runat="server" />
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
                                            <asp:TextBox ID="txtRTData" Width="100%" MaxLength="3" runat="server" />
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtRWData" Width="100%" MaxLength="3" runat="server" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Desa / Kelurahan")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtCounty" Width="100%" MaxLength="50" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Kecamatan")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtDistrict" Width="100%" MaxLength="50" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblMandatory lblNormal">
                                    <%=GetLabel("Kota / Kabupaten")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtCity" Width="100%" MaxLength="50" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblLink" id="lblProvince" runat="server">
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
                    <%=GetLabel("Data Kontak Pasien")%></h4>
                <div class="containerTblEntryContent">
                    <table class="tblEntryContent" style="width: 100%">
                        <colgroup>
                            <col style="width: 35%" />
                            <col style="width: 65%" />
                        </colgroup>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblMandatory">
                                    <%=GetLabel("No Telepon 1")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtTelephoneNo1" Width="100%" MaxLength="50" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("No Telepon 2")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtTelephoneNo2" Width="100%" MaxLength="50" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblMandatory">
                                    <%=GetLabel("No HP 1")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtMobilePhone1" Width="100%" MaxLength="20" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("No HP 2")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtMobilePhone2" Width="100%" MaxLength="20" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Email")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtEmail" CssClass="email" Width="100%" MaxLength="50" runat="server" />
                            </td>
                        </tr>
                    </table>
                </div>
                <h4 class="h4expanded">
                    <%=GetLabel("Data Keluarga")%></h4>
                <div class="containerTblEntryContent">
                    <table class="tblEntryContent" style="width: 100%">
                        <colgroup>
                            <col style="width: 35%" />
                            <col style="width: 65%" />
                        </colgroup>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Nama Ayah")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtFatherName" Width="100%" MaxLength="50" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Nama Ibu")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtMotherName" Width="100%" MaxLength="50" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Nama Pasangan")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtSpouseName" Width="100%" MaxLength="20" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Nama Anak")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtChildName" Width="100%" MaxLength="20" runat="server" />
                            </td>
                        </tr>
                    </table>
                </div>
                <div>
                    <table width="100%">
                        <tr>
                            <td>
                                <div style="width: 600px;">
                                    <div class="pageTitle" style="text-align: center">
                                        <%=GetLabel("Informasi")%></div>
                                    <div style="background-color: #EAEAEA;">
                                        <table width="600px" cellpadding="0" cellspacing="0">
                                            <colgroup>
                                                <col width="150px" />
                                                <col width="30px" />
                                            </colgroup>
                                            <tr>
                                                <td align="left">
                                                    <%=GetLabel("Dibuat Oleh") %>
                                                </td>
                                                <td align="center">
                                                    :
                                                </td>
                                                <td>
                                                    <div runat="server" id="divCreatedBy">
                                                    </div>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left">
                                                    <%=GetLabel("Dibuat Pada") %>
                                                </td>
                                                <td align="center">
                                                    :
                                                </td>
                                                <td>
                                                    <div runat="server" id="divCreatedDate">
                                                    </div>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left">
                                                    <%=GetLabel("Terakhir Diubah Oleh") %>
                                                </td>
                                                <td align="center">
                                                    :
                                                </td>
                                                <td>
                                                    <div runat="server" id="divLastUpdatedBy">
                                                    </div>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left">
                                                    <%=GetLabel("Terakhir Diubah Pada")%>
                                                </td>
                                                <td align="center">
                                                    :
                                                </td>
                                                <td>
                                                    <div runat="server" id="divLastUpdatedDate">
                                                    </div>
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </div>
                            </td>
                        </tr>
                    </table>
                </div>
            </td>
            <td>
                &nbsp;
            </td>
            <td style="padding: 5px; vertical-align: top;">
                <h4 class="h4expanded">
                    <%=GetLabel("Data Pembayar")%></h4>
                <div class="containerTblEntryContent">
                    <table class="tblEntryContent" style="width: 100%">
                        <colgroup>
                            <col style="width: 35%" />
                            <col style="width: 65%" />
                        </colgroup>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblMandatory  ">
                                    <%=GetLabel("Instansi")%></label>
                            </td>
                            <td>
                                <dxe:ASPxComboBox ID="cboPayer" ClientInstanceName="cboPayer" Width="100%" runat="server">
                                    <ClientSideEvents ValueChanged="function(s){ onCboPayerValueChanged(s); }" />
                                </dxe:ASPxComboBox>
                            </td>
                        </tr>
                        <tr id="trPayerCompany" runat="server">
                            <td class="tdLabel">
                                <label class="lblLink" id="lblPayerCompany">
                                    <%=GetLabel("Perusahaan")%></label>
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
                                            <asp:TextBox ID="txtPayerCompanyCode" Width="100%" runat="server" />
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtPayerCompanyName" Width="100%" runat="server" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </div>
                <h4 class="h4expanded">
                    <%=GetLabel("Data Pekerjaan Pasien")%></h4>
                <div class="containerTblEntryContent">
                    <table class="tblEntryContent" style="width: 100%">
                        <colgroup>
                            <col style="width: 35%" />
                            <col style="width: 65%" />
                        </colgroup>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblMandatory lblLink" id="lblPatientJob" runat="server">
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
                                            <asp:TextBox ID="txtPatientJobName" Width="100%" runat="server" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Nama Kantor")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtPatientJobOffice" Width="100%" MaxLength="100" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel" style="vertical-align: top; padding-top: 5px;">
                                <label class="lblNormal">
                                    <%=GetLabel("Alamat Kantor")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtPatientJobOfficeAddress" Width="100%" MaxLength="500" runat="server"
                                    TextMode="MultiLine" Rows="2" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblLink" id="lblOfficeZipCode">
                                    <%=GetLabel("Kode Pos [Kantor]")%></label>
                            </td>
                            <td>
                                <table style="width: 100%" cellpadding="0" cellspacing="0">
                                    <colgroup>
                                        <col style="width: 40%" />
                                        <col style="width: 3px" />
                                        <col />
                                    </colgroup>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtPatientJobOfficeZipCode" Width="100%" runat="server" />
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
                                            <asp:TextBox ID="txtRTOfficeData" Width="100%" MaxLength="3" runat="server" />
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtRWOfficeData" Width="100%" MaxLength="3" runat="server" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Desa / Kelurahan [Kantor]")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtPatientJobOfficeCounty" Width="100%" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Kecamatan [Kantor]")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtPatientJobOfficeDistrict" Width="100%" MaxLength="50" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Kota [Kantor]")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtPatientJobOfficeCity" Width="100%" MaxLength="50" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblLink" id="lblOfficeProvince" runat="server">
                                    <%=GetLabel("Provinsi [Kantor]")%></label>
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
                                            <asp:TextBox ID="txtPatientJobOfficeProvinceCode" Width="100%" runat="server" />
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtPatientJobOfficeProvinceName" Width="100%" runat="server" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("No Telepon [Kantor]")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtPatientJobOfficeTelephone" Width="100%" MaxLength="50" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Email [Kantor]")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtPatientJobOfficeEmail" CssClass="email" Width="100%" MaxLength="50"
                                    runat="server" />
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
                                <label class="lblMandatory lblLink" id="lblEducation" runat="server">
                                    <%=GetLabel("Pendidikan")%></label>
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
                                            <asp:TextBox ID="txtEducationCode" Width="100%" runat="server" />
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtEducationName" Width="100%" runat="server" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblMandatory lblLink" id="lblReligion">
                                    <%=GetLabel("Agama")%></label>
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
                                <label class="lblLink" id="lblMaritalStatus">
                                    <%=GetLabel("Status Pernikahan")%></label>
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
                                            <asp:TextBox ID="txtMaritalStatusCode" Width="100%" runat="server" />
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtMaritalStatusName" Width="100%" runat="server" />
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
                                        <col style="width: 30%" />
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
                            <td class="tdLabel">
                                <label class="lblLink" id="lblPatientCategory">
                                    <%=GetLabel("Kategori Pasien")%></label>
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
                                            <asp:TextBox ID="txtPatientCategoryCode" Width="100%" runat="server" />
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtPatientCategoryName" Width="100%" runat="server" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblLink" id="lblEmployee">
                                    <%=GetLabel("Pegawai")%></label>
                            </td>
                            <td>
                                <input type="hidden" runat="server" id="hdnEmployeeID" value="" />
                                <table style="width: 100%" cellpadding="0" cellspacing="0">
                                    <colgroup>
                                        <col style="width: 30%" />
                                        <col style="width: 3px" />
                                        <col />
                                    </colgroup>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtEmployeeCode" Width="100%" runat="server" />
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtEmployeeName" Width="100%" runat="server" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblLink" id="lblLanguage">
                                    <%=GetLabel("Bahasa")%></label>
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
                                            <asp:TextBox ID="txtlanguageCode" Width="100%" runat="server" />
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtlanguageName" Width="100%" runat="server" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </div>
                <h4 class="h4expanded">
                    <%=GetLabel("Status Pasien")%></h4>
                <div class="containerTblEntryContent">
                    <table class="tblEntryContent" style="width: 100%">
                        <colgroup>
                            <col style="width: 35%" />
                            <col style="width: 65%" />
                        </colgroup>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblMandatory">
                                    <%=GetLabel("Status Pasien")%></label>
                            </td>
                            <td>
                                <dxe:ASPxComboBox ID="cboPatientStatus" ClientInstanceName="cboPatientStatus" Width="100%"
                                    runat="server">
                                </dxe:ASPxComboBox>
                            </td>
                        </tr>
                    </table>
                    <table class="tblEntryContent" style="width: 100%">
                        <colgroup>
                            <col style="width: 50%" />
                        </colgroup>
                        <tr>
                            <td valign="top">
                                <table width="100%">
                                    <colgroup>
                                        <col style="width: 10px" />
                                    </colgroup>
                                    <tr>
                                        <td colspan="2" style="font-weight: bold;">
                                            <%=GetLabel("Klinis") %>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:CheckBox ID="chkIsHasInfectious" Width="100%" runat="server" />
                                        </td>
                                        <td>
                                            <%=GetLabel("Has Infectious")%>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:CheckBox ID="chkIsHasAllergy" Width="100%" runat="server" />
                                        </td>
                                        <td>
                                            <%=GetLabel("Has Allergy")%>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:CheckBox ID="chkIsFallRisk" Width="100%" runat="server" />
                                        </td>
                                        <td>
                                            <%=GetLabel("Fall Risk")%>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:CheckBox ID="chkIsAllowResuscitate" Width="100%" runat="server" />
                                        </td>
                                        <td>
                                            <%=GetLabel("DNR")%>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:CheckBox ID="chkIsG6PD" Width="100%" runat="server" />
                                        </td>
                                        <td>
                                            <%=GetLabel("G6PD")%>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:CheckBox ID="chkIsAlive" Width="100%" runat="server" />
                                        </td>
                                        <td>
                                            <%=GetLabel("Hidup")%>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:CheckBox ID="chkIsHasPhysicalLimitation" Width="100%" runat="server" />
                                        </td>
                                        <td>
                                            <%=GetLabel("Keterbatasan Fisik")%>
                                        </td>
                                        <td id="tdPhysicalLimitation" runat="server" style="display: none">
                                            <dxe:ASPxComboBox runat="server" ID="cboPhysicalLimitation" ClientInstanceName="cboPhysicalLimitation"
                                                Width="100px">
                                            </dxe:ASPxComboBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:CheckBox ID="chkIsGeriatricPatient" Width="100%" runat="server" />
                                        </td>
                                        <td>
                                            <%=GetLabel("Pasien Geriatri")%>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:CheckBox ID="chkIsPregnant" Width="100%" runat="server" />
                                        </td>
                                        <td>
                                            <%=GetLabel("Hamil")%>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td valign="top">
                                <table width="100%">
                                    <colgroup>
                                        <col style="width: 10px" />
                                    </colgroup>
                                    <tr>
                                        <td colspan="3" style="font-weight: bold;">
                                            <%=GetLabel("Sosial") %>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:CheckBox ID="chkIsSmoking" Width="100%" runat="server" />
                                        </td>
                                        <td>
                                            <%=GetLabel("Merokok")%>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:CheckBox ID="chkIsIlliteracy" Width="100%" runat="server" />
                                        </td>
                                        <td>
                                            <%=GetLabel("Buta Huruf")%>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:CheckBox ID="chkIsDonor" Width="100%" runat="server" />
                                        </td>
                                        <td>
                                            <%=GetLabel("Donor")%>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:CheckBox ID="chkIsBlackList" Width="100%" runat="server" />
                                        </td>
                                        <td>
                                            <%=GetLabel("Pasien Bermasalah / Penanganan khusus")%>
                                        </td>
                                        <td id="tdGCBlacklistReason" runat="server" style="display: none">
                                            <dxe:ASPxComboBox runat="server" ID="cboGCBlacklistReason" ClientInstanceName="cboGCBlacklistReason"
                                                Width="180px">
                                                <ClientSideEvents ValueChanged="function(s,e){ onCboGCBlacklistReasonValueChanged(); }" />
                                            </dxe:ASPxComboBox>
                                        </td>
                                    </tr>
                                    <tr id="trOtherBlackListReason" runat="server" style="display: none">
                                        <td>
                                        </td>
                                        <td colspan="2">
                                            <asp:TextBox ID="txtOtherBlackListReason" Width="100%" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:CheckBox ID="chkIsVIP" Width="100%" runat="server" />
                                        </td>
                                        <td>
                                            <%=GetLabel("VIP")%>
                                        </td>
                                        <td id="tdVIPGroup" runat="server" style="display: none">
                                            <dxe:ASPxComboBox runat="server" ID="cboVIPGroup" ClientInstanceName="cboVIPGroup"
                                                Width="100px">
                                                <ClientSideEvents ValueChanged="function(s,e){ oncboVIPGroupChanged(); }" />
                                            </dxe:ASPxComboBox>
                                        </td>
                                    </tr>
                                    <tr id="trVIPGroupOther" runat="server" style="display: none">
                                        <td class="tdLabel">
                                            <label class="lblNormal" id="lblOtherReason">
                                                <%=GetLabel("VIP Group (Other) : ")%></label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtOtherVIPGroup" Width="180px" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:CheckBox ID="chkIsHasCommunicationRestriction" Width="100%" runat="server" />
                                        </td>
                                        <td>
                                            <%=GetLabel("Keterbatasan Komunikasi")%>
                                        </td>
                                        <td id="tdCommunication" runat="server" style="display: none">
                                            <dxe:ASPxComboBox runat="server" ID="cboCommunication" ClientInstanceName="cboCommunication"
                                                Width="100px">
                                            </dxe:ASPxComboBox>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </div>
                <h4 class="h4expanded">
                    <%=GetLabel("Informasi Lainnya")%></h4>
                <div class="containerTblEntryContent">
                    <table class="tblEntryContent" style="width: 100%">
                        <colgroup>
                            <col style="width: 35%" />
                            <col style="width: 65%" />
                        </colgroup>
                        <tr>
                            <td class="tdLabel" style="vertical-align: top; padding-top: 5px;">
                                <label class="lblNormal">
                                    <%=GetLabel("No RM Lama")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtOldMedicalNo" Width="100%" runat="server"/>
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
                                <label class="lblNormal">
                                    <%=GetLabel("No Karyawan")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtCorporateAccountNo" Width="100%" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel" style="vertical-align: top; padding-top: 5px;">
                                <label class="lblNormal">
                                    <%=GetLabel("Nama Karyawan")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtCorporateAccountName" Width="100%" runat="server" />
                            </td>
                        </tr>
                    </table>
                </div>
                <asp:Panel ID="pnlCustomAttribute" runat="server">
                    <h4 class="h4expanded">
                        <%=GetLabel("Custom Attribute")%></h4>
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
                <h4 class="h4expanded">
                    <%=GetLabel("Master Patient Index (SatuSEHAT)")%></h4>
                <div class="containerTblEntryContent">
                    <table class="tblEntryContent" style="width: 100%">
                        <colgroup>
                            <col style="width: 35%" />
                            <col style="width: 65%" />
                        </colgroup>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("IHS Number / No. SatuSEHAT")%></label>
                            </td>
                            <td>
                                <table border="0" cellpadding="1" cellspacing="0">
                                    <colgroup>
                                        <col width="200px" />
                                        <col />
                                    </colgroup>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtIHSNumber" Width="100%" runat="server" />                                        
                                        </td>
                                        <td style="padding-left : 10px">
                                            <input type="button" id="btnGetIHSNumber" class="btnIHSNumber w3-btn1 w3-hover-blue" value='<%=GetLabel("IHS Number") %>' title="No. Rekam Medis Pasien di Platform SATUSEHAT" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Pelepasan Informasi : SATUSEHAT")%></label>
                            </td>
                            <td colspan="2">
                                <asp:RadioButtonList ID="rblSATUSEHAT" runat="server" RepeatDirection="Horizontal" Enabled="False">
                                    <asp:ListItem Text=" Setuju" Value="1" />
                                    <asp:ListItem Text=" Tidak Setuju" Value="0"  />
                                </asp:RadioButtonList>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Tanggal Consent")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtSatuSEHATConsentDate" Width="120px" runat="server" Enabled="false" />                                        
                            </td>
                        </tr>
                    </table>                                
                </div>
            </td>
        </tr>
    </table>
    </table>
</asp:Content>
