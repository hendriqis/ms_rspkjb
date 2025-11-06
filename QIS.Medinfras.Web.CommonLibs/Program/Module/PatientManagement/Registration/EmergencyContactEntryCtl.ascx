<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="EmergencyContactEntryCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.EmergencyContactEntryCtl" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<script type="text/javascript" id="dxss_patiententryctl">

    setDatePicker('<%=txtDOB.ClientID %>');

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

    $('#<%=txtPatientJobCode.ClientID %>').live('change', function () {
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

    registerCollapseExpandHandler();
    $(function () {
        $('#<%=chkIsFamily.ClientID %>').live('change', function () {
            var isPatient = $(this).is(":checked");
            if (isPatient) {
                $('#<%:lblFamily.ClientID %>').attr('class', 'lblLink');
                $('#<%=txtContactName.ClientID %>').attr('readonly', 'readonly');
                cboRelationship.SetEnabled(false);
            }
            else {
                $('#<%:lblFamily.ClientID %>.lblLink').attr('class', 'lblDisabled');
                $('#<%=txtContactName.ClientID %>').removeAttr('readonly');
                cboRelationship.SetEnabled(true);
                $('#<%=txtFamily.ClientID %>').val('');
                $('#<%=hdnFamilyID.ClientID %>').val('');
                $('#<%=txtContactName.ClientID %>').val('');
                cboRelationship.SetValue('');
                $('#<%=txtTelephoneNo.ClientID %>').val('');
                $('#<%=txtMobilePhone.ClientID %>').val('');
                $('#<%=txtEmail.ClientID %>').val('');
                $('#<%=hdnAddressID.ClientID %>').val('');
                $('#<%=txtAddress.ClientID %>').val('');
                $('#<%=txtCounty.ClientID %>').val('');
                $('#<%=txtDistrict.ClientID %>').val('');
                $('#<%=txtCity.ClientID %>').val('');
                $('#<%=txtProvinceCode.ClientID %>').val('');
                $('#<%=txtProvinceName.ClientID %>').val('');
                $('#<%=hdnZipCode.ClientID %>').val('');
                $('#<%=txtZipCode.ClientID %>').val('');
                $('#<%=txtDOB.ClientID %>').val('');
                $('#<%=txtBirthPlace.ClientID %>').val('');
                $('#<%=txtPatientJobCode.ClientID %>').val('');
                $('#<%=txtPatientJobName.ClientID %>').val('');
            }
        });
    });

    //#region Family
    function onGetEmergencyContactFamilyFilterExpression() {
        var filterExpression = "MRN = " + $('#<%=hdnMRN.ClientID %>').val() + "AND IsDeleted = 0";
        return filterExpression;
    }

    $('#<%:lblFamily.ClientID %>.lblLink').die('click');
    $('#<%:lblFamily.ClientID %>.lblLink').live('click', function () {
        openSearchDialog('patientFamily', onGetEmergencyContactFamilyFilterExpression(), function (value) {
            $('#<%=hdnFamilyID.ClientID %>').val(value);
            onTxtFamilyChanged(value);
        });
    });

    function onTxtFamilyChanged(value) {
        var filterExpression = onGetEmergencyContactFamilyFilterExpression() + " AND FamilyID = " + value;
        Methods.getObject('GetvPatientFamilyList', filterExpression, function (result) {
            if (result != null) {
                $('#<%=txtFamily.ClientID %>').val(result.FamilyMedicalNo);
                $('#<%=txtContactName.ClientID %>').val(result.FullName);
                cboRelationship.SetValue(result.GCFamilyRelation);
                $('#<%=txtTelephoneNo.ClientID %>').val(result.PhoneNo1);
                $('#<%=txtMobilePhone.ClientID %>').val('');
                $('#<%=txtEmail.ClientID %>').val(result.Email);
                $('#<%=hdnAddressID.ClientID %>').val(result.AddressID);
                $('#<%=txtAddress.ClientID %>').val(result.StreetName);
                $('#<%=txtCounty.ClientID %>').val(result.County);
                $('#<%=txtDistrict.ClientID %>').val(result.District);
                $('#<%=txtCity.ClientID %>').val(result.City);
                $('#<%=txtDOB.ClientID %>').val(result.cfDOBInDatePickerFormat);
                $('#<%=txtBirthPlace.ClientID %>').val(result.CityOfBirth);
                $('#<%=txtPatientJobCode.ClientID %>').val(result.cfOccupationID);
                $('#<%=txtPatientJobName.ClientID %>').val(result.Occupation);

                var stateCode = result.GCState.split('^');

                $('#<%=txtProvinceCode.ClientID %>').val(stateCode[1]);
                $('#<%=txtProvinceName.ClientID %>').val(result.State);
                $('#<%=hdnZipCode.ClientID %>').val(result.ZipCodeID);
                $('#<%=txtZipCode.ClientID %>').val(result.ZipCode);

            } else {
                $('#<%=hdnFamilyID.ClientID %>').val('');
                $('#<%=txtFamily.ClientID %>').val('');
                $('#<%=txtContactName.ClientID %>').val('');
                cboRelationship.SetValue('');
                $('#<%=txtTelephoneNo.ClientID %>').val('');
                $('#<%=txtMobilePhone.ClientID %>').val('');
                $('#<%=txtEmail.ClientID %>').val('');
                $('#<%=hdnAddressID.ClientID %>').val('');
                $('#<%=txtAddress.ClientID %>').val('');
                $('#<%=txtCounty.ClientID %>').val('');
                $('#<%=txtDistrict.ClientID %>').val('');
                $('#<%=txtCity.ClientID %>').val('');
                $('#<%=txtProvinceCode.ClientID %>').val('');
                $('#<%=txtProvinceName.ClientID %>').val('');
                $('#<%=hdnZipCode.ClientID %>').val('');
                $('#<%=txtZipCode.ClientID %>').val('');
                $('#<%=txtDOB.ClientID %>').val('');
                $('#<%=txtBirthPlace.ClientID %>').val('');
                $('#<%=txtPatientJobCode.ClientID %>').val('');
                $('#<%=txtPatientJobName.ClientID %>').val('');
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

    $('#<%=txtProvinceCode.ClientID %>').die('change');
    $('#<%=txtProvinceCode.ClientID %>').live('change', function () {
        onTxtProvinceCodeChanged($(this).val());
    });

    function onTxtProvinceCodeChanged(value) {
        var filterExpression = onGetSCProvinceFilterExpression() + " AND StandardCodeID LIKE '%^" + value + "'";
        Methods.getObject('GetStandardCodeList', filterExpression, function (result) {
            if (result != null)
                $('#<%=txtProvinceName.ClientID %>').val(result.StandardCodeName);
            else
                $('#<%=txtProvinceName.ClientID %>').val('');
        });
    }
    //#endregion

    //#region Zip Code
    $('#lblZipCode.lblLink').die('click');
    $('#lblZipCode.lblLink').live('click', function () {
        openSearchDialog('zipcodes', 'IsDeleted = 0', function (value) {
            onTxtZipCodeChanged(value);
        });
    });

    $('#<%=txtZipCode.ClientID %>').die('change');
    $('#<%=txtZipCode.ClientID %>').live('change', function () {
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

</script>
<div style="height: 450px; overflow-x: hidden; overflow-y: scroll;">
    <input type="hidden" runat="server" id="hdnRegistrationID" value="" />
    <input type="hidden" runat="server" id="hdnMRN" value="" />
    <input type="hidden" runat="server" id="hdnFamilyID" value="" />
    <input type="hidden" runat="server" id="hdnAddressID" value="" />
    <input type="hidden" runat="server" id="hdnIsAddressUseZipCode" value="" />

    <table class="tblContentArea">
        <colgroup>
            <col style="width: 150px" />
            <col style="width: 150px" />
            <col style="width: 450px" />
            <col style="width: 200px" />
            <col />
        </colgroup>
        <tr>
            <td class="tdLabel">
                <label class="LblLink" runat="server" id="lblRegistration">
                    <%=GetLabel("No Registrasi")%></label>
            </td>
            <td>
                <asp:TextBox ID="txtRegistrationNoCtlPF" Width="100%" runat="server" ReadOnly="true" />
            </td>
        </tr>
        <tr>
            <td class="tdLabel">
                <label class="LblLink" runat="server" id="lblMRN">
                    <%=GetLabel("Pasien")%></label>
            </td>
            <td colspan="2">
                <asp:TextBox ID="txtPatientCtlPF" Width="100%" runat="server" ReadOnly="true" />
            </td>
        </tr>
    </table>
    <fieldset id="fsEmergencyContact" style="margin: 0">
        <table class="tblContentArea">
            <colgroup>
                <col style="width: 49%" />
                <col style="width: 3px" />
                <col style="width: 49%" />
            </colgroup>
            <tr>
                <td style="padding: 5px; vertical-align: top;">
                    <h4 class="h4expanded">
                        <%=GetLabel("Data")%></h4>
                    <div class="containerTblEntryContent">
                        <table class="tblEntryContent" style="width: 100%">
                            <colgroup>
                                <col style="width: 35%;" />
                                <col style="width: 65%;" />
                            </colgroup>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblDisabled" id="lblFamily" runat="server">
                                        <%=GetLabel("Family")%></label>
                                </td>
                                <td>
                                    <table style="width: 100%" cellpadding="0" cellspacing="0">
                                        <colgroup>
                                            <col style="width: 30%" />
                                            <col style="width: 3px" />
                                            <col style="width: 60px" />
                                            <col />
                                        </colgroup>
                                        <tr>
                                            <td>
                                                <asp:TextBox ID="txtFamily" ReadOnly="true" Width="100%" runat="server" />
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td>
                                                <asp:CheckBox ID="chkIsFamily" runat="server" /><%=GetLabel("Keluarga")%>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblNormal" runat="server" id="lblContactName">
                                        <%=GetLabel("Nama")%></label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtContactName" Width="100%" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblNormal" runat="server" id="Label2">
                                        <%=GetLabel("No Kartu Identitas")%></label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtIdentityCardNo" Width="100%" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <%=GetLabel("Tempat/Tanggal Lahir")%></label>
                                </td>
                                <td>
                                    <table style="width: 100%" cellpadding="0" cellspacing="0">
                                        <colgroup>
                                            <col style="width: 50%" />
                                            <col style="width: 3px" />
                                            <col />
                                        </colgroup>
                                        <tr>
                                            <td>
                                                <asp:TextBox ID="txtBirthPlace" Width="100%" MaxLength="50" runat="server" />
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtDOB" Width="108px" runat="server" CssClass="datepicker" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
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
                                    <label class="lblNormal" runat="server" id="Label1">
                                        <%=GetLabel("Hubungan")%></label>
                                </td>
                                <td>
                                    <dxe:ASPxComboBox ID="cboRelationship" ClientInstanceName="cboRelationship" Width="100%"
                                        runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblMandatory">
                                        <%=GetLabel("Telepon")%></label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtTelephoneNo" Width="100%" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblNormal">
                                        <%=GetLabel("No HP")%></label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtMobilePhone" Width="100%" runat="server" />
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
                                <td class="tdLabel" style="vertical-align: top; padding-top: 5px;">
                                    <label class="lblNormal">
                                        <%=GetLabel("Catatan")%></label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtRemarks" Width="100%" runat="server" TextMode="MultiLine" Rows="2" />
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
                        <%=GetLabel("Data Alamat")%></h4>
                    <div class="containerTblEntryContent" style="width: 100%">
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
                                    <asp:TextBox ID="txtAddress" Width="100%" runat="server" TextMode="MultiLine" Rows="2" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <label class="lblNormal">
                                        <%=GetLabel("RT/RW")%></label>
                                </td>
                                <td>
                                    <table style="width: 50%" cellpadding="0" cellspacing="0">
                                        <colgroup>
                                            <col style="width: 50%" />
                                            <col style="width: 50%" />
                                        </colgroup>
                                        <tr>
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
                                    <label class="lblLink" id="lblProvince" runat="server" >
                                        <%=GetLabel("Provinsi")%></label>
                                </td>
                                <td>
                                    <table style="width: 100%" cellpadding="0" cellspacing="0">
                                        <colgroup>
                                            <col style="width: 30%" />
                                            <col style="width: 3px" />
                                        </colgroup>
                                        <tr>
                                            <td>
                                                <asp:TextBox ID="txtProvinceCode" Width="100%" runat="server" />
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtProvinceName" ReadOnly="true" Width="100%" runat="server" />
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
    </fieldset>
</div>
