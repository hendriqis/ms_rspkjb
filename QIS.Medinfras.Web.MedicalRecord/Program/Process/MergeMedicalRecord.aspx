<%@ Page Language="C#" MasterPageFile="~/libs/MasterPage/MPTrx.master" AutoEventWireup="true" 
    CodeBehind="MergeMedicalRecord.aspx.cs" Inherits="QIS.Medinfras.Web.MedicalRecord.Program.MergeMedicalRecord" %>

<asp:Content ID="Content3" ContentPlaceHolderID="plhCustomButtonToolbar" runat="server">
    <li id="btnProcess" CRUDMode="R" runat="server"><img src='<%=ResolveUrl("~/Libs/Images/Icon/tbset.png")%>' alt="" /><br style="clear:both"/><div><%=GetLabel("Proses")%></div></li>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div class="menuTitle"><%=GetMenuCaption()%></div>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript">
        var isRegistrationClosed = false;

        $('#<%=btnProcess.ClientID %>').live('click', function () {
            if ($('#<%:hdnMedicalNo2.ClientID %>').val() == "") {
                showToast("ERROR", 'Error Message : ' + "Nomor Rekam Medis tujuan harus dipilih terlebih dahulu !");
            }
            else {
                var listOpenReg = "";
                var mrn1 = $('#<%:hdnMRN1.ClientID %>').val();
                var mrn2 = $('#<%:hdnMRN2.ClientID %>').val();
                var dob1 = $('#<%:txtDOB1.ClientID %>').val();
                var dob2 = $('#<%:txtDOB2.ClientID %>').val();
                var gender1 = $('#<%:txtGender1.ClientID %>').val();
                var gender2 = $('#<%:txtGender2.ClientID %>').val();

                var filterOpenReg = "MRN = " + mrn1;
                Methods.getListObject('GetvRegistrationOutstandingInfoPerMRNList', filterOpenReg, function (resultOpenReg) {
                    for (i = 0; i < resultOpenReg.length; i++) {
                        listOpenReg += ", " + i;
                    }
                });

                if (mrn1 == mrn2) {
                    showToast('Warning', "Tidak bisa proses penggabungan nomor rekam medis yang sama");
                }
                else if (gender1 != gender2) {
                    showToast('Warning', "Jenis Kelamin pada Nomor Rekam Medis " + $('#<%:hdnMedicalNo1.ClientID %>').val() + " (" + gender1 + ") dengan Nomor Rekam Medis " + $('#<%:hdnMedicalNo2.ClientID %>').val() + " (" + gender2 + ") berbeda.");
                }
                else if (dob1 != dob2) {
                    showToast('Warning', "Tanggal Lahir pada Nomor Rekam Medis " + $('#<%:hdnMedicalNo1.ClientID %>').val() + " (" + dob1 + ") dengan Nomor Rekam Medis " + $('#<%:hdnMedicalNo2.ClientID %>').val() + " (" + dob2 + ") berbeda.");
                }
                else {
                    if (listOpenReg != "") {
                        var message1 = "Nomor Rekam Medis " + $('#<%:hdnMedicalNo1.ClientID %>').val() + " masih memiliki daftar registrasi yang masih belum dipulangkan / masih ada oustanding. Lanjutkan proses ?";
                        showToastConfirmation(message1, function (result1) {
                            if (result1) {
                                var message2 = "Lanjutkan proses penggabungan Nomor Rekam Medis " + $('#<%:hdnMedicalNo1.ClientID %>').val() + " ke " + $('#<%:hdnMedicalNo2.ClientID %>').val() + " ?";
                                showToastConfirmation(message2, function (result2) {
                                    if (result2) onCustomButtonClick('merge');
                                });
                            }
                        });
                    } else {
                        var message2 = "Lanjutkan proses penggabungan Nomor Rekam Medis " + $('#<%:hdnMedicalNo1.ClientID %>').val() + " ke " + $('#<%:hdnMedicalNo2.ClientID %>').val() + " ?";
                        showToastConfirmation(message2, function (result2) {
                            if (result2) onCustomButtonClick('merge');
                        });
                    }
                }
            }
        });

        function initializeControl() {
            $('#<%:hdnMRN1.ClientID %>').val('');
            $('#<%:hdnMRN2.ClientID %>').val('');
            $('#<%:txtMRN1.ClientID %>').val('');
            $('#<%:txtMRN2.ClientID %>').val('');
            $('#<%:txtPatientName1.ClientID %>').val('');
            $('#<%:txtPatientName2.ClientID %>').val('');
            $('#<%:txtSSN1.ClientID %>').val('');
            $('#<%:txtSSN2.ClientID %>').val('');
            $('#<%:txtPreferredName1.ClientID %>').val('');
            $('#<%:txtPreferredName2.ClientID %>').val('');
            $('#<%:txtGender1.ClientID %>').val('');
            $('#<%:txtGender2.ClientID %>').val('');
            $('#<%:txtBirthPlace1.ClientID %>').val('');
            $('#<%:txtBirthPlace2.ClientID %>').val('');
            $('#<%:txtDOB1.ClientID %>').val('');
            $('#<%:txtDOB2.ClientID %>').val('');
            $('#<%:txtAddress1.ClientID %>').val('');
            $('#<%:txtAddress2.ClientID %>').val('');
            $('#<%:txtLastVisit1.ClientID %>').val('');
            $('#<%:txtLastVisit2.ClientID %>').val('');
        }

        //#region MRN
        $('#<%:lblMRN1.ClientID %>.lblLink').live('click', function () {
            openSearchDialog('patient2', '', function (value) {
                $('#<%:txtMRN1.ClientID %>').val(value);
                onTxtMRNChanged(value,'1');
            });
        });

        $('#<%:lblMRN2.ClientID %>.lblLink').live('click', function () {
            openSearchDialog('patient2', '', function (value) {
                $('#<%:txtMRN2.ClientID %>').val(value);
                onTxtMRNChanged(value,'2');
            });
        });

        $('#<%:txtMRN1.ClientID %>').live('change', function () {
            onTxtMRNChanged($(this).val(),'1');
        });

        $('#<%:txtMRN2.ClientID %>').live('change', function () {
            onTxtMRNChanged($(this).val(),'2');
        });

        function onTxtMRNChanged(value, index) {
            var filterExpression = "MRN = " + value + " AND IsDeleted = 0";
            Methods.getObject('GetvPatientList', filterExpression, function (result) {
                SetPatientInformationToControl(result,index);
            });
        }

        function SetPatientInformationToControl(result,index) {
            if (result != null) {
                if (index == '1') {
                    $('#<%:txtMRN1.ClientID %>').val(result.MedicalNo);
                    $('#<%:hdnMedicalNo1.ClientID %>').val(result.MedicalNo);
                    $('#<%:hdnMRN1.ClientID %>').val(result.MRN);
                    $('#<%:txtPatientName1.ClientID %>').val(result.PatientName);
                    if (result.SSN != null && result.SSN != "") {
                        $('#<%:txtSSN1.ClientID %>').val("(" + result.IdentityNoType + ") " + result.SSN);
                    } else {
                        $('#<%:txtSSN1.ClientID %>').val('');
                    }
                    $('#<%:txtPreferredName1.ClientID %>').val(result.PreferredName);
                    $('#<%:txtGender1.ClientID %>').val(result.Gender);
                    $('#<%:txtBirthPlace1.ClientID %>').val('');
                    $('#<%:txtDOB1.ClientID %>').val(result.DateOfBirthInString);
                    $('#<%:txtAddress1.ClientID %>').val(result.HomeAddress);
                    $('#<%:txtLastVisit1.ClientID %>').val(result.cfLastVisitDateInString);
                    $('#<%:txtNoOfVisit1.ClientID %>').val(result.TotalVisit);
                }
                else {
                    $('#<%:txtMRN2.ClientID %>').val(result.MedicalNo);
                    $('#<%:hdnMedicalNo2.ClientID %>').val(result.MedicalNo);
                    $('#<%:hdnMRN2.ClientID %>').val(result.MRN);
                    $('#<%:txtPatientName2.ClientID %>').val(result.PatientName);
                    if (result.SSN != null && result.SSN != "") {
                        $('#<%:txtSSN2.ClientID %>').val("(" + result.IdentityNoType + ") " + result.SSN);
                    } else {
                        $('#<%:txtSSN2.ClientID %>').val('');
                    }
                    $('#<%:txtPreferredName2.ClientID %>').val(result.PreferredName);
                    $('#<%:txtGender2.ClientID %>').val(result.Gender);
                    $('#<%:txtBirthPlace2.ClientID %>').val('');
                    $('#<%:txtDOB2.ClientID %>').val(result.DateOfBirthInString);
                    $('#<%:txtAddress2.ClientID %>').val(result.HomeAddress);
                    $('#<%:txtLastVisit2.ClientID %>').val(result.cfLastVisitDateInString);
                    $('#<%:txtNoOfVisit2.ClientID %>').val(result.TotalVisit);
                }
            }
            else {
                if (index == '1') {
                    $('#<%:hdnMRN1.ClientID %>').val('');
                    $('#<%:hdnMedicalNo1.ClientID %>').val('');
                    $('#<%:txtMRN1.ClientID %>').val('');
                    $('#<%:txtPatientName1.ClientID %>').val('');
                    $('#<%:txtSSN1.ClientID %>').val('');
                    $('#<%:txtPreferredName1.ClientID %>').val('');
                    $('#<%:txtGender1.ClientID %>').val('');
                    $('#<%:txtBirthPlace1.ClientID %>').val('');
                    $('#<%:txtDOB1.ClientID %>').val('');
                    $('#<%:txtAddress1.ClientID %>').val('');
                    $('#<%:txtLastVisit1.ClientID %>').val(result.cfLastVisitDateInString);
                    $('#<%:txtNoOfVisit1.ClientID %>').val('');
                }
                else {
                    $('#<%:hdnMRN2.ClientID %>').val('');
                    $('#<%:hdnMedicalNo2.ClientID %>').val('');
                    $('#<%:txtMRN2.ClientID %>').val('');
                    $('#<%:txtPatientName2.ClientID %>').val('');
                    $('#<%:txtSSN2.ClientID %>').val('');
                    $('#<%:txtPreferredName2.ClientID %>').val('');
                    $('#<%:txtGender2.ClientID %>').val('');
                    $('#<%:txtBirthPlace2.ClientID %>').val('');
                    $('#<%:txtDOB2.ClientID %>').val('');
                    $('#<%:txtAddress2.ClientID %>').val('');
                    $('#<%:txtLastVisit2.ClientID %>').val(result.cfLastVisitDateInString);
                    $('#<%:txtNoOfVisit2.ClientID %>').val('');
                }
            }
        }

        //#endregion

        function onAfterCustomClickSuccess(type) {
            var message = "Proses penggabungan Nomor Rekam Medis " + $('#<%:hdnMedicalNo1.ClientID %>').val() + " ke " + $('#<%:hdnMedicalNo2.ClientID %>').val() + "!";
            showToast("MEDINFRAS", 'PROSES BERHASIL : ' + message);
            initializeControl();
        }
    </script>
    <input type="hidden" id="hdnDepartmentID" value="" runat="server" />
    <input type="hidden" id="hdnRegistrationID" value="" runat="server" />
    <input type="hidden" id="hdnMRN1" value="" runat="server" />
    <input type="hidden" id="hdnMedicalNo1" value="" runat="server" />
    <input type="hidden" id="hdnMRN2" value="" runat="server" />
    <input type="hidden" id="hdnMedicalNo2" value="" runat="server" />
    <table class="tblContentArea">
        <colgroup>
            <col style="width:50%"/>
            <col style="width:50%"/>
        </colgroup>
        <tr>
            <td colspan="2">
                <table style="width:100%">
                    <colgroup width="70px" />
                    <colgroup />
                    <tr>
                        <td>
                            <img src='<%=ResolveUrl("~/Libs/Images/warning.png")%>' alt="" height="65px" width="65px" />
                        </td>
                        <td style="vertical-align:top;">
                            <h4 style="background-color:transparent;color:red;font-weight:bold"><%=GetLabel("PERINGATAN : Proses tidak bisa dibatalkan")%></h4>
                            <%=GetLabel("Data personal dan kunjungan pasien dari Nomor Rekam Medis Asal akan digabungkan ke Nomor Tujuan")%>
                            <br />
                            <%=GetLabel("Pastikan data yang akan digabungkan sudah benar.")%>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td style="padding:5px;vertical-align:top">
                <table class="tblEntryContent" style="width:100%">
                    <colgroup>
                        <col style="width:200px"/>
                        <col style="width:150px"/>
                        <col style="width:150px"/>
                        <col/>
                    </colgroup>
                    <tr>
                        <td colspan="4"><h4><%=GetLabel("DATA PASIEN YANG AKAN DIGABUNGKAN :")%></h4></td>                        
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblLink" runat="server" id="lblMRN1"><%=GetLabel("No.RM")%></label></td>
                        <td><asp:TextBox ID="txtMRN1" Width="120px" runat="server" /></td>
                        <td />
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Nama Pasien")%></label></td>
                        <td colspan="3"><asp:TextBox ID="txtPatientName1" Width="100%" runat="server" ReadOnly="true" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblNormal"><%=GetLabel("No Identitas")%></label></td>
                        <td colspan="3"><asp:TextBox ID="txtSSN1" Width="100%" runat="server" ReadOnly="true" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Nama Panggilan")%></label></td>
                        <td><asp:TextBox ID="txtPreferredName1" Width="100%" runat="server" ReadOnly="true" /></td>
                        <td class="tdLabel" style="text-align:right; padding-right:5px"><label class="lblNormal"><%=GetLabel("Jenis Kelamin")%></label></td>
                        <td><asp:TextBox ID="txtGender1" Width="100%" runat="server" ReadOnly="true" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Tempat Lahir")%></label></td>
                        <td><asp:TextBox ID="txtBirthPlace1" Width="100%" runat="server" ReadOnly="true"/></td>
                        <td class="tdLabel" style="text-align:right; padding-right:5px"><label class="lblNormal"><%=GetLabel("Tanggal Lahir")%></label></td>
                        <td><asp:TextBox ID="txtDOB1" Width="100%" runat="server" ReadOnly="true"/></td>
                    </tr>
                    <tr>
                        <td class="tdLabel" style="vertical-align: top; padding-top: 5px;"><label class="lblNormal"><%=GetLabel("Alamat")%></label></td>
                        <td colspan="3"><asp:TextBox ID="txtAddress1" Width="100%" runat="server" TextMode="MultiLine" Rows="5" ReadOnly="true" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel" style="vertical-align: top; padding-top: 5px;"><label class="lblNormal"><%=GetLabel("Kunjungan Terakhir")%></label></td>
                        <td><asp:TextBox ID="txtLastVisit1" Width="100%" runat="server" ReadOnly="true"/></td>
                        <td class="tdLabel" style="text-align:right; padding-right:5px"><label class="lblNormal"><%=GetLabel("Jumlah Kunjungan")%></label></td>
                        <td><asp:TextBox ID="txtNoOfVisit1" Width="100%" runat="server" ReadOnly="true" /></td>
                    </tr>
                </table>
            </td>
            <td style="padding:5px;vertical-align:top">
                <table class="tblEntryContent" style="width:100%">
                    <colgroup>
                        <col style="width:200px"/>
                        <col style="width:150px"/>
                        <col style="width:150px"/>
                        <col/>
                    </colgroup>
                    <tr>
                        <td colspan="4"><h4><%=GetLabel("AKAN DIGABUNGKAN KE :")%></h4></td>                        
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblLink" runat="server" id="lblMRN2"><%=GetLabel("No.RM")%></label></td>
                        <td><asp:TextBox ID="txtMRN2" Width="120px" runat="server" /></td>
                        <td />
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Nama Pasien")%></label></td>
                        <td colspan="3"><asp:TextBox ID="txtPatientName2" Width="100%" runat="server" ReadOnly="true" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblNormal"><%=GetLabel("No Identitas")%></label></td>
                        <td colspan="3"><asp:TextBox ID="txtSSN2" Width="100%" runat="server" ReadOnly="true" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Nama Panggilan")%></label></td>
                        <td><asp:TextBox ID="txtPreferredName2" Width="100%" runat="server" ReadOnly="true" /></td>
                        <td class="tdLabel" style="text-align:right; padding-right:5px"><label class="lblNormal"><%=GetLabel("Jenis Kelamin")%></label></td>
                        <td><asp:TextBox ID="txtGender2" Width="100%" runat="server" ReadOnly="true" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><label class="lblNormal"><%=GetLabel("Tempat Lahir")%></label></td>
                        <td><asp:TextBox ID="txtBirthPlace2" Width="100%" runat="server" ReadOnly="true"/></td>
                        <td class="tdLabel" style="text-align:right; padding-right:5px"><label class="lblNormal"><%=GetLabel("Tanggal Lahir")%></label></td>
                        <td><asp:TextBox ID="txtDOB2" Width="100%" runat="server" ReadOnly="true"/></td>
                    </tr>
                    <tr>
                        <td class="tdLabel" style="vertical-align: top; padding-top: 5px;"><label class="lblNormal"><%=GetLabel("Alamat")%></label></td>
                        <td colspan="3"><asp:TextBox ID="txtAddress2" Width="100%" runat="server" TextMode="MultiLine" Rows="5" ReadOnly="true" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel" style="vertical-align: top; padding-top: 5px;"><label class="lblNormal"><%=GetLabel("Kunjungan Terakhir")%></label></td>
                        <td><asp:TextBox ID="txtLastVisit2" Width="100%" runat="server" ReadOnly="true"/></td>
                        <td class="tdLabel" style="text-align:right; padding-right:5px"><label class="lblNormal"><%=GetLabel("Jumlah Kunjungan")%></label></td>
                        <td><asp:TextBox ID="txtNoOfVisit2" Width="100%" runat="server" ReadOnly="true" /></td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Content>
