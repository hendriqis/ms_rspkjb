<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="GuestEntryReservationCtlX.ascx.cs" 
    Inherits="QIS.Medinfras.Web.Inpatient.Program.GuestEntryReservationCtl" %>

<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>

<script type="text/javascript" id="dxss_guestentryctl">
    setDatePicker('<%=txtDOB.ClientID %>');
    $('#<%=txtDOB.ClientID %>').datepicker('option', 'maxDate', '0');
    //registerCollapseExpandHandler();

    function entityToControl(entity) {
        if (entity != null) {
            setEntryPopupIsAdd(false);
            
            //#region Patient Data
            cboSalutation.SetValue(entity.GCSalutation);
            cboTitle.SetValue(entity.GCTitle);
            $('#<%=txtFirstName.ClientID %>').val(entity.FirstName);
            $('#<%=txtMiddleName.ClientID %>').val(entity.MiddleName);
            $('#<%=txtFamilyName.ClientID %>').val(entity.LastName);

            cboSuffix.SetValue(entity.GCSuffix);
            cboGender.SetValue(entity.GCGender);
            
            $('#<%=txtDOB.ClientID %>').val(Methods.getJSONDateValue(entity.DateOfBirth));
            $('#<%=txtAgeInYear.ClientID %>').val(entity.AgeInYear);
            $('#<%=txtAgeInMonth.ClientID %>').val(entity.AgeInMonth);
            $('#<%=txtAgeInDay.ClientID %>').val(entity.AgeInDay);
            //#endregion

            //#region Patient Address
            $('#<%=txtAddress.ClientID %>').val(entity.StreetName);
            $('#<%=txtCounty.ClientID %>').val(entity.County);
            $('#<%=txtDistrict.ClientID %>').val(entity.District);
            $('#<%=txtCity.ClientID %>').val(entity.City);
            //#endregion

            //#region Patient Contact
            $('#<%=txtTelephoneNo.ClientID %>').val(entity.PhoneNo);
            $('#<%=txtMobilePhone.ClientID %>').val(entity.MobilePhoneNo);
            $('#<%=txtEmail.ClientID %>').val(entity.EmailAddress);
            cboIdentityCardType.SetValue(entity.GCIdentityNoType);
            $('#<%=txtIdentityCardNo.ClientID %>').val(entity.SSN);
            //#endregion
        }
        else {
            setEntryPopupIsAdd(true);
            
            //#region Patient Data
            cboSalutation.SetValue('');
            cboTitle.SetValue('');
            $('#<%=txtFirstName.ClientID %>').val('');
            $('#<%=txtMiddleName.ClientID %>').val('');
            $('#<%=txtFamilyName.ClientID %>').val('');
            cboSuffix.SetValue('');
            cboGender.SetValue('');
            
            $('#<%=txtDOB.ClientID %>').val('');
            $('#<%=txtAgeInYear.ClientID %>').val('');
            $('#<%=txtAgeInMonth.ClientID %>').val('');
            $('#<%=txtAgeInDay.ClientID %>').val('');
            //#endregion

            //#region Patient Address
            $('#<%=txtAddress.ClientID %>').val('');
            $('#<%=txtCounty.ClientID %>').val('');
            $('#<%=txtDistrict.ClientID %>').val('');
            $('#<%=txtCity.ClientID %>').val('');
            //#endregion

            //#region Patient Contact
            $('#<%=txtTelephoneNo.ClientID %>').val('');
            $('#<%=txtMobilePhone.ClientID %>').val('');
            $('#<%=txtEmail.ClientID %>').val('');
            cboIdentityCardType.SetValue('');
            $('#<%=txtIdentityCardNo.ClientID %>').val('');
            //#endregion
        }
    }

    //#region DOB
    $('#<%=txtDOB.ClientID %>').change(function () {
        var age = Methods.getAgeFromDatePickerFormat($(this).val());
        $('#<%=txtAgeInYear.ClientID %>').val(age.years);
        $('#<%=txtAgeInMonth.ClientID %>').val(age.months);
        $('#<%=txtAgeInDay.ClientID %>').val(age.days);
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

    $('#<%=txtDOB.ClientID %>').change();
    //#endregion

    function onBeforeSaveRecord(errMessage) {
        //if (IsValid(null, 'fsEntryPopup', 'mpEntryPopup')) {
        LoadGuest(
            cboSalutation.GetValue(),
            cboTitle.GetValue(),
            $('#<%=txtFirstName.ClientID %>').val(),
            $('#<%=txtMiddleName.ClientID %>').val(),
            $('#<%=txtFamilyName.ClientID %>').val(),
            cboSuffix.GetValue(),
            cboGender.GetValue(),
            $('#<%=txtDOB.ClientID %>').val(),
            $('#<%=txtAddress.ClientID %>').val(),
            $('#<%=txtCounty.ClientID %>').val(),
            $('#<%=txtDistrict.ClientID %>').val(),
            $('#<%=txtCity.ClientID %>').val(),
            $('#<%=txtTelephoneNo.ClientID %>').val(),
            $('#<%=txtMobilePhone.ClientID %>').val(),
            $('#<%=txtEmail.ClientID %>').val(),
            cboIdentityCardType.GetValue(),
            $('#<%=txtIdentityCardNo.ClientID %>').val(),
            cboSuffix.GetText(),
            cboTitle.GetText(),
            $('#<%=txtAgeInYear.ClientID %>').val(),
            $('#<%=txtAgeInMonth.ClientID %>').val(),
            $('#<%=txtAgeInDay.ClientID %>').val(),
            cboGender.GetText()
        );
        return true;
        //}
    }

    function onGetEntryPopupReturnValue() {
        return "";
    }
</script>
<input type="hidden" runat="server" id="hdnGuestID" />
<div style="height:445px;overflow-y:scroll;">
    <table class="tblContentArea">
        <colgroup>
            <col style="width:49%"/>
            <col style="width:3px"/>
            <col style="width:49%"/>
        </colgroup>
        <tr>
            <td style="padding:5px;vertical-align:top;">
                <h4><%=GetLabel("Data Pasien")%></h4>
                <div class="containerTblEntryContent">
                    <table class="tblEntryContent" style="width:100%">
                        <colgroup>
                            <col style="width:35%"/>
                            <col style="width:65%"/>
                        </colgroup>
                        <tr>
                            <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Sapaan")%></label></td>
                            <td><dxe:ASPxComboBox ID="cboSalutation" ClientInstanceName="cboSalutation" Width="100%" runat="server" /></td>
                        </tr>
                        <tr>
                            <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Gelar Depan")%></label></td>
                            <td><dxe:ASPxComboBox ID="cboTitle" ClientInstanceName="cboTitle" Width="100%" runat="server" /></td>
                        </tr>
                        <tr>
                            <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Nama Pasien")%></label></td>
                            <td>
                                <table style="width:100%" cellpadding="0" cellspacing="0">
                                    <colgroup>
                                        <col style="width:49%"/>
                                        <col style="width:3px"/>
                                        <col/>
                                    </colgroup>
                                    <tr>
                                        <td><asp:TextBox ID="txtFirstName" Width="100%" runat="server" /></td>
                                        <td>&nbsp;</td>
                                        <td><asp:TextBox ID="txtMiddleName" Width="100%" runat="server" /></td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Nama Belakang")%></label></td>
                            <td><asp:TextBox ID="txtFamilyName" Width="100%" runat="server" /></td>
                        </tr>
                        <tr>
                            <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Gelar Belakang")%></label></td>
                            <td><dxe:ASPxComboBox ID="cboSuffix" ClientInstanceName="cboSuffix" Width="100%" runat="server" /></td>
                        </tr>
                        <tr>
                            <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Jenis Kelamin")%></label></td>
                            <td><dxe:ASPxComboBox ID="cboGender" ClientInstanceName="cboGender" Width="100%" runat="server" /></td>
                        </tr>
                        <tr>
                            <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Tanggal Lahir")%></label></td>
                            <td><asp:TextBox ID="txtDOB" Width="120px" runat="server" CssClass="datepicker" /></td>
                        </tr>
                        <tr>
                            <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("Umur")%> (yyyy-MM-dd)</label></td>
                            <td>
                                <table style="width:100%" cellpadding="0" cellspacing="0">
                                    <colgroup>
                                        <col style="width:32%"/>
                                        <col style="width:3px"/>
                                        <col style="width:32%"/>
                                        <col style="width:3px"/>
                                        <col style="width:32%"/>
                                    </colgroup>
                                    <tr>
                                        <td><asp:TextBox ID="txtAgeInYear" CssClass="number" Width="100%" runat="server" /></td>
                                        <td>&nbsp;</td>
                                        <td><asp:TextBox ID="txtAgeInMonth" CssClass="number" Width="100%" runat="server" /></td>
                                        <td>&nbsp;</td>
                                        <td><asp:TextBox ID="txtAgeInDay" CssClass="number" Width="100%" runat="server" /></td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </div>
            </td>
            <td>&nbsp;</td>
            <td style="padding:5px;vertical-align:top;">                
                <h4><%=GetLabel("Data Alamat")%></h4>
                <div class="containerTblEntryContent">
                    <table class="tblEntryContent" style="width:100%">
                        <colgroup>
                            <col style="width:35%"/>
                            <col style="width:65%"/>
                        </colgroup>
                        <tr>
                            <td class="tdLabel" style="vertical-align: top; padding-top: 5px;"><label class="lblMandatory"><%=GetLabel("Jalan")%></label></td>
                            <td><asp:TextBox ID="txtAddress" Width="100%" runat="server" TextMode="MultiLine" Rows="2" /></td>
                        </tr>
                        <tr>
                            <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Desa / Kelurahan")%></label></td>
                            <td><asp:TextBox ID="txtCounty" Width="100%" runat="server" /></td>
                        </tr>
                        <tr>
                            <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Kecamatan")%></label></td>
                            <td><asp:TextBox ID="txtDistrict" Width="100%" runat="server" /></td>
                        </tr>
                        <tr>
                            <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Kota")%></label></td>
                            <td><asp:TextBox ID="txtCity" Width="100%" runat="server" /></td>
                        </tr>
                    </table>
                </div>
                
                <h4><%=GetLabel("Data Kontak Pasien")%></h4>
                <div class="containerTblEntryContent">
                    <table class="tblEntryContent" style="width:100%">
                        <colgroup>
                            <col style="width:35%"/>
                            <col style="width:65%"/>
                        </colgroup>
                        <tr>
                            <td class="tdLabel"><label class="lblMandatory"><%=GetLabel("No Telepon")%></label></td>
                            <td><asp:TextBox ID="txtTelephoneNo" Width="100%" runat="server" /></td>
                        </tr>
                        <tr>
                            <td class="tdLabel"><label class="lblNormal"><%=GetLabel("No HP")%></label></td>
                            <td><asp:TextBox ID="txtMobilePhone" Width="100%" runat="server" /></td>
                        </tr>
                        <tr>
                            <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Email")%></label></td>
                            <td><asp:TextBox ID="txtEmail" CssClass="email" Width="100%" runat="server" /></td>
                        </tr>
                        <tr>
                            <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Tipe Kartu Identitas")%></label></td>
                            <td><dxe:ASPxComboBox ID="cboIdentityCardType" ClientInstanceName="cboIdentityCardType" Width="100%" runat="server" /></td>
                        </tr>
                        <tr>
                            <td class="tdLabel"><label class="lblNormal"><%=GetLabel("No Kartu Identitas")%></label></td>
                            <td><asp:TextBox ID="txtIdentityCardNo" Width="100%" runat="server" /></td>
                        </tr>
                    </table>
                </div>
            </td>
        </tr>
    </table>
</div>