<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PatientBannerCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Controls.PatientBannerCtl" %>
<script type="text/javascript" id="dxss_patientbannerctl">
    $(function () {
        var gender = $('#<%=hdnPatientGender.ClientID %>').val();
        Methods.checkImageError('imgPatientProfilePicture', 'patient', gender);
        Methods.checkImageError('imgPhysician', 'paramedic');

        $('#lblFromRegistrationInfo').click(function (e) {
            var registrationID = $('#<%:hdnRegistrationID.ClientID %>').val();
            var url = ResolveUrl("~/Libs/Program/Information/InfoRegistrationLinkedFromCtl.ascx");
            openUserControlPopup(url, registrationID, 'Informasi Registrasi Asal', 1200, 400);
        });

        $('#lblPatientInfo').click(function (e) {
            var url = ResolveUrl("~/Libs/Controls/PatientInfoCtl.ascx");
            var data = $('#<%=hdnRegistrationID.ClientID %>').val();
            openUserControlPopup(url, data, 'Informasi Detail: Pasien', 1100, 500);
        });

        $('#lblAllergyInfo').click(function (e) {
            var url = ResolveUrl("~/Libs/Controls/PatientAllergyInfoCtl.ascx");
            var data = "";
            openUserControlPopup(url, data, 'Patient Drug Allergy', 800, 480);
        });

        $('#lblShowQRCode').click(function (e) {
            var url = ResolveUrl("~/Libs/Controls/QRCodeCtl.ascx");
            data = ($('#<%=hdnRegistrationNo.ClientID %>').val());
            openUserControlPopup(url, data, 'QR Codes', 500, 500);
        });

        $('#<%=imgPatientProfilePicture.ClientID %>').click(function (e) {
            var url = ResolveUrl("~/Libs/Controls/EditPatientPhotoCtl.ascx");
            data = ($('#<%=hdnMRN.ClientID %>').val());
            openUserControlPopup(url, data, 'Patient Photo', 350, 300);
        });

        $('#<%=imgPatientProfilePicture.ClientID %>').click(function (e) {
            var url = ResolveUrl("~/Libs/Controls/EditPatientPhotoCtl.ascx");
            data = ($('#<%=hdnMRN.ClientID %>').val());
            data1 = ($('#<%=hdnGuestID.ClientID %>').val());
            var filter = "";
            if (data != "0") {
                filter = "1|" + data;
            }
            else {
                filter = "0|" + data1;
            }

            openUserControlPopup(url, filter, 'Patient Photo', 350, 300);
        });

        $('#<%=imgARInvoicePatient.ClientID %>').click(function (e) {
            var url = ResolveUrl("~/Libs/Controls/OutstandingARInvoicePatient.ascx");
            mrn = ($('#<%=hdnMRN.ClientID %>').val());
            medicalNo = ($('#<%=hdnMedicalNo.ClientID %>').val());
            patientName = ($('#<%=hdnPatientName.ClientID %>').val());
            data = mrn + '|' + medicalNo + '|' + patientName;
            openUserControlPopup(url, data, 'Daftar Piutang Pribadi', 800, 500);
        });

        $('#<%=imgCOB.ClientID %>').click(function (e) {
            var url = ResolveUrl("~/Libs/Controls/CoordinationOfBenefitCtl.ascx");
            registrationNo = ($('#<%=hdnRegistrationNo.ClientID %>').val());
            medicalNo = ($('#<%=hdnMedicalNo.ClientID %>').val());
            patientName = ($('#<%=hdnPatientName.ClientID %>').val());
            data = registrationNo + '|' + medicalNo + '|' + patientName;
            openUserControlPopup(url, data, 'Coordination Of Benefit (COB)', 1000, 500);
        });

        $('#<%=imgIsMultipleVisit.ClientID %>').click(function (e) {
            var url = ResolveUrl("~/Libs/Controls/VisitMultipleCtl.ascx");
            data = ($('#<%=hdnRegistrationNo.ClientID %>').val());
            openUserControlPopup(url, data, 'Visit', 800, 500);
        });

        $('#<%=imgIsHasOthersRegActive.ClientID %>').click(function (e) {
            var url = ResolveUrl("~/Libs/Controls/ActiveRegistrationByMRNCtl.ascx");
            oMRN = ($('#<%=hdnMRN.ClientID %>').val());
            oMedicalNo = ($('#<%=hdnMedicalNo.ClientID %>').val());
            oPatientName = ($('#<%=hdnPatientName.ClientID %>').val());
            oRegistrationNo = ($('#<%=hdnRegistrationNo.ClientID %>').val());
            data = oMRN + '|' + oMedicalNo + '|' + oPatientName + '|' + oRegistrationNo;
            openUserControlPopup(url, data, 'Daftar Registrasi Aktif per No RM', 1200, 500);
        });

        $('#<%=imgParamedicTeam.ClientID %>').click(function (e) {
            var url = ResolveUrl("~/Libs/Controls/ParamedicTeamDetailCtl.ascx");
            data = ($('#<%=hdnRegistrationNo.ClientID %>').val());
            openUserControlPopup(url, data, 'Rawat Bersama', 800, 500);
        });

        $('#<%=imgPatientWallet.ClientID %>').click(function (e) {
            var url = ResolveUrl("~/Libs/Controls/PatientDepositBalanceCtl.ascx");
            MRN = ($('#<%=hdnMRN.ClientID %>').val());
            medicalNo = ($('#<%=hdnMedicalNo.ClientID %>').val());
            patientName = ($('#<%=hdnPatientName.ClientID %>').val());
            data = MRN + '|' + medicalNo + '|' + patientName;
            openUserControlPopup(url, data, 'History Deposit Balance', 1200, 500);
        });

        $('#<%=imgIsHasPhysicalLimitation.ClientID %>').click(function (e) {
            var url = ResolveUrl("~/Libs/Controls/IsHasPhysicalLimitationCtl.ascx");
            MRN = ($('#<%=hdnMRN.ClientID %>').val());
            medicalNo = ($('#<%=hdnMedicalNo.ClientID %>').val());
            patientName = ($('#<%=hdnPatientName.ClientID %>').val());
            var registrationID = ($('#<%=hdnRegistrationNo.ClientID %>').val());
            data = MRN + '|' + medicalNo + '|' + patientName + '|' + registrationID;
            openUserControlPopup(url, data, 'Physical Limitation Info', 1000, 200);
        });

        $('#<%=imgIsHasCommunicationRestriction.ClientID %>').click(function (e) {
            var url = ResolveUrl("~/Libs/Controls/IsHasCommunicationRestrictionCtl.ascx");
            MRN = ($('#<%=hdnMRN.ClientID %>').val());
            medicalNo = ($('#<%=hdnMedicalNo.ClientID %>').val());
            patientName = ($('#<%=hdnPatientName.ClientID %>').val());
            var registrationID = ($('#<%=hdnRegistrationNo.ClientID %>').val());
            data = MRN + '|' + medicalNo + '|' + patientName + '|' + registrationID;
            openUserControlPopup(url, data, 'Communication Limitation Info', 1000, 200);
        });

        $('#<%=imgPackageItem.ClientID %>').click(function (e) {
            var url = ResolveUrl("~/Libs/Controls/VisitPackageInformationCtl.ascx");
            data = ($('#<%=hdnMRN.ClientID %>').val());
            openUserControlPopup(url, data, 'Paket Kunjungan', 1200, 500);
        });

        $('#<%=imgRegHasPackageAIO.ClientID %>').click(function (e) {
            var url = ResolveUrl("~/Libs/Controls/ConsultVisitItemPackageBalanceCtl.ascx");
            var oVisitID = ($('#<%=hdnVisitID.ClientID %>').val());
            var oRegistrationID = ($('#<%=hdnRegistrationID.ClientID %>').val());
            var data = oRegistrationID + "|" + oVisitID;
            openUserControlPopup(url, data, 'AIO Package Balance', 1400, 500);
        });

        $('#<%=imgOutstandingOrder.ClientID %>').click(function (e) {
            var oRegistrationID = $('#<%=hdnRegistrationID.ClientID %>').val();
            var url = ResolveUrl("~/Libs/Program/Module/PatientManagement/PatientBillSummary/OutstandingOrderListCtl.ascx");
            openUserControlPopup(url, oRegistrationID, 'Pending/Outstanding Order', 1200, 500);
        });
    });

    $('#btnPayerInfo').live('click', function () {
        var payer = $('#<%:hdnbannerPayerID.ClientID %>').val();
        var contract = $('#<%:hdnbannerContractID.ClientID %>').val();
        var url = ResolveUrl("~/Libs/Program/Information/InformationCustomerPayerCtl.ascx");
        var id = payer + "|" + contract;
        openUserControlPopup(url, id, 'Informasi Instansi', 700, 600);
    });

    function setLblPayerText(value) {
        $('#<%=lblPayer.ClientID %>').html(value);
    }

    function setLblParamedicName(value) {
        $('#<%=lblPhysicianName.ClientID %>').html(value);
    }

    function setLblSpecialtyName(value) {
        $('#<%=lblParamedicType.ClientID %>').html(value);
    }

    function onAfterSavePatientPhoto() {
        var MRN = $('#<%:hdnMRN.ClientID %>').val();
        var filterExpression = 'MRN = ' + MRN;
        hideLoadingPanel();
    }
</script>
<style type="text/css">
    #ulGridInfo
    {
        margin: 0;
        padding: 0;
    }
    #ulGridInfo li
    {
        list-style-type: none;
        display: inline-block;
        margin-right: 0.5px;
         -moz-border-radius: 5px;
        border-radius: 5px;
        vertical-align: top;
    }
    .tdGridInfoHeader
    {
        padding: 0.5px;
        background-color: gray;
        color: white;
        font-style: oblique;
        border: 1px solid gray;
        text-align: center;
        font-size: 10pt;
    }
    .tdGridInfoDetail
    {
        padding: 0.5px;
        font-weight: bold;
        border: 1px solid gray;
        text-align: center;
        font-size: 10pt;
    }
    .tdInfoHeader
    {
        padding: 2px;
        background-color: #CCCCCC;
        color: Black;
        border: 1px solid gray;
        text-align: center;
        font-size: 10pt;
    }
    .tdInfoDetail
    {
        padding: 2px;
        background-color: white;
        color: Black;
        font-weight: bold;
        border: 1px solid gray;
        text-align: center;
        font-size: 10pt;
    }
    .imgPatient:hover
    {
        width: 95%;
        height: 95%;
    }
</style>
<table border="0" cellpadding="0" cellspacing="1" width="100%">
    <tr>
        <td>
            <div style="height: 100px;">
                <input type="hidden" id="hdnbannerPayerID" value="" runat="server" />
                <input type="hidden" id="hdnbannerContractID" value="" runat="server" />
                <input type="hidden" id="hdnDepartmentID" value="" runat="server" />
                <input type="hidden" id="hdnGuestID" value="" runat="server" />
                <div style="float: right;  width: 28%;"">
                    <img id="imgPhysician" runat="server" class="imgPhysician" alt="" width="55" style="float: right; height: 70px;"
                        src="" />
                    <input type="hidden" id="hdnPatientGender" runat="server" class="hdnPatientGender" />
                    <div style="text-align: right; margin-right: 65px;">
                        <div style="font-weight: bold;">
                            <label id="lblPhysicianName" runat="server">
                            </label>
                        </div>
                        <div class="tblPatientBannerInfo" style="font-size: 10pt">
                            <div style="font-style: none; font-size: small;">
                                <label id="lblParamedicLicenseNo" runat="server">
                                </label>
                            </div>
                            <div style="font-size: smaller;">
                                <label id="lblParamedicType" runat="server">
                                </label>
                                <label style="font-weight: bold; color: #1BA1E2;" id="lblGradingInfo" runat="server">
                                </label>
                            </div>
                            <div id="divOutpatientInfoLine1" runat="server">
                                <label style="font-style: italic">
                                    <%=GetServiceUnitLabel()%></label>&nbsp;&nbsp;<label id="lblServiceUnit" runat="server"></label></div>
                            <div id="divOutpatientInfoLine2" runat="server">
                                <label style="font-style: italic">
                                    <%=GetLabel("Jenis Kunjungan")%></label>&nbsp;&nbsp;<label id="lblVisitType" runat="server"></label></div>
                            <div id="divInpatientInfoLine1" runat="server">
                                <label style="font-style: italic">
                                    <%=GetLabel("Kelas Rawat")%>
                                    |
                                    <%=GetLabel("Tagihan")%>
                                </label>
                                &nbsp;&nbsp;
                                <label id="lblInpatientClass" runat="server" style="font-weight: bold;">
                                </label>
                            </div>
                            <div id="divInpatientInfoLine1b" runat="server">
                                <label id="lblInpatientControlClassCaption" runat="server" style="font-style: italic">
                                    <%=GetLabel("Jatah Kelas")%>
                                </label>
                                &nbsp;&nbsp;
                                <label id="lblInpatientControlClass" runat="server">
                                </label>
                            </div>
                            <div id="divHakKelasBPJS" runat="server">
                                <label id="lblInpatientHakKelasCaption" runat="server" style="font-style: italic">
                                    <%=GetLabel("Hak Kelas")%>
                                </label>
                                &nbsp;&nbsp;
                                <label id="lblInpatientHakKelas" runat="server">
                                </label>
                            </div>
                            <div id="divInpatientInfoLine2" runat="server">
                                <label id="lblInpatientWard" runat="server">
                                </label>
                                (<label style="font-weight: bold; color: #1BA1E2;" id="lblInpatientLOS" runat="server"></label>)</div>
                        </div>
                    </div>
                </div>
                <table style="float: left; background-color: White" cellpadding="0" cellspacing="0">
                    <tr>
                        <td valign="top">
                            <div class="divPatientBannerImgInfo" id="divPatientBannerImgInfo" runat="server"
                                style="background-color: Red; height: 70px; width: 12px">
                            </div>
                        </td>
                        <td>
                            <div id="id=divPatientImage">
                                <img id="imgPatientProfilePicture" class="imgLink hvr-glow imgPatient" runat="server"
                                    src='' alt="" style="height: 70px;" />
                            </div>
                        </td>
                        <td style="vertical-align: bottom">
                            <div id="divPatientStatusDNR" class="divPatientStatusDNR" style="background-color: Purple;
                                padding: 0px 1px 0px 1px; font-size: 0.8em; text-align: center; color: white"
                                runat="server">
                                D
                            </div>
                            <div id="divPatientStatusFallRisk" class="divPatientStatusFallRisk" style="background-color: Yellow;
                                padding: 0px 1px 0px 1px; font-size: 0.8em; text-align: center" runat="server">
                                F
                            </div>
                            <div id="divPatientStatusAllergy" class="divPatientStatusAllergy" style="background-color: Red;
                                padding: 0px 1px 0px 1px; font-size: 0.8em; text-align: center; color: white"
                                runat="server">
                                A
                            </div>
                            <div id="divIsGeriatricPatient" class="divIsGeriatricPatient" style="background-color: Blue;
                                padding: 0px 1px 0px 1px; font-size: 0.8em; text-align: center; color: white"
                                runat="server">
                                G
                            </div>
                        </td>
                    </tr>
                </table>
                <table class="tblPatientBannerInfo" style="margin-left: 50px" cellpadding="1" cellspacing="0" width="65%">
                    <col style="width: 8%; vertical-align: top;" />
                    <col style="width: 18%; vertical-align: top;" />
                    <col style="width: 12%; vertical-align: top;" />
                    <col style="width: 20%; vertical-align: top;" />
                    <col style="width: 14%; vertical-align: top;" />
                    <col style="width: 28%; vertical-align: top;" />
                    <tr style="font-size: 1.2em">
                        <td colspan="4" style="padding-left: 10px">
                            <span class="lblLink" id="lblPatientInfo" style="color: Black">
                                <label id="lblPatientName" runat="server" style="font-weight: bold;">
                                </label>
                            </span>
                        </td>
                        <td class="lblRegistrationNo" colspan="2">
                            <input type="hidden" id="hdnVisitID" value="0" runat="server" />
                            <input type="hidden" id="hdnRegistrationID" value="0" runat="server" />
                            <input type="hidden" id="hdnRegistrationNo" value="0" runat="server" />
                            <span class="lblLink" id="lblShowQRCode">
                                <label id="lblRegistrationNo" runat="server" style="font-weight: bold;">
                                </label>
                            </span>
                            <span class="lblLink" id="lblFromRegistrationInfo" style="color: Black">
                              <label id="lblFromRegistrationNo" runat="server" style="color: Red;
                                font-size: smaller; font-style: oblique" />
                            </span>
                        </td>
                       
                    </tr>
                    <tr>
                        <td class="tdPatientBannerLabel">
                            <%=GetLabel("No. RM")%>
                        </td>
                        <td class="tdPatientBannerValue">
                            <input type="hidden" id="hdnMRN" value="" runat="server" />
                            <input type="hidden" id="hdnMedicalNo" value="" runat="server" />
                            <input type="hidden" id="hdnPatientName" value="" runat="server" />
                            <label id="lblMRN" runat="server" style="float: left; font-weight: bold;" />
                            <span id="spnOldMedicalNo" runat="server">
                                <label id="lblOldMRN" runat="server" style="margin-left: 3px; color: Red" />
                            </span>
                        </td>
                        <td class="tdPatientBannerLabel">
                            <%=GetLabel("Tanggal Lahir")%>
                        </td>
                        <td class="tdPatientBannerValue" style="font-weight: bold;">
                            <label id="lblDOB" runat="server">
                            </label>
                        </td>
                        <td class="tdPatientBannerLabel">
                            <%=GetLabel("Tanggal ")%>
                            -
                            <%=GetLabel("Jam ")%>
                        </td>
                        <td class="tdPatientBannerValue" style="font-weight: bold;">
                            <label id="lblDateHour" runat="server">
                            </label>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdPatientBannerLabel">
                            <%=GetLabel("Alergi")%>
                        </td>
                        <td style="font-weight:bold;">
                            <span class="lblLink" id="lblAllergyInfo" style="color: Red">
                                <label id="lblAllergy" runat="server" style="color: Red; font-weight: bold">
                                </label>
                            </span>
                        </td>
                        <td class="tdPatientBannerLabel">
                            <%=GetLabel("Umur - Agama")%>
                        </td>
                        <td class="tdPatientBannerValue"  style="font-weight: bold;">
                            <label id="lblPatientAge" runat="server">
                            </label>
                        </td>
                        <td class="tdPatientBannerLabel">
                            <%=GetLabel("Pengirim")%>
                        </td>
                        <td class="tdPatientBannerValue"  style="font-weight: bold;">
                            <label id="lblReferral" runat="server" >
                            </label>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdPatientBannerLabel">
                            <%=GetLabel("Kategori") %>
                        </td>
                        <td class="tdPatientBannerValue"  style="font-weight: bold;">
                            <label id="lblPatientCategory" runat="server">
                            </label>
                        </td>
                        <td class="tdPatientBannerLabel"  >
                            <%=GetLabel("Jenis Kelamin")%>
                        </td>
                        <td class="tdPatientBannerValue" style="font-weight: bold;">
                            <label id="lblGender" runat="server">
                            </label>
                        </td>
                        <td class="tdPatientBannerLabel">
                            <%=GetLabel("Perawat Pengkaji")%>
                        </td>
                          <td class="tdPatientBannerValue" style="font-weight: bold;">
                            <label id="lblNurseParamedicName" runat="server">
                            </label>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdPatientBannerLabel">
                            <%=GetLabel("Pembayar") %>
                        </td>
                        <td colspan="5">
                            <table cellspacing="0" cellpadding="2" style="vertical-align: middle;">
                                <tr>
                                    <td style="padding: 3px; color: Black; font-weight: bold; border: none; text-align: left;">
                                        <label id="lblPayer" runat="server">
                                        </label>
                                    </td>
                                    <td style="display: none; padding-right: 2px;" id="tdPayerInfobanner" runat="server">
                                        <label id="lblPayerInfo" runat="server">
                                        </label>
                                        <input type="button" class="btn" value="..." id="btnPayerInfo" />
                                    </td>                                                                  
                                </tr>
                            </table>
                        </td>
                    </tr>
                    
                </table>
            </div>
        </td>
    </tr>
    <tr>
        <td>
            <table cellspacing="0" style="width: 100%">
                <tr style="vertical-align: middle; width: 100%">
                    <td style="width: 100%;" colspan="2">
                        
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <table cellspacing="0" style="width: 100%">
                            <colgroup>
                                <col style="width: 95%" />
                                <col style="width: 5%" />
                            </colgroup>
                            <tr>
                                <td align="left">
                                    <asp:Repeater ID="rptGridInfo" runat="server" OnItemDataBound="rptGridInfo_ItemDataBound">
                                        <HeaderTemplate>
                                            <ul id="ulGridInfo">
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <li>
                                                <table cellpadding="0" cellspacing="0">
                                                    <colgroup>
                                                        <col style="width: 60px" />
                                                    </colgroup>
                                                    <tr>
                                                        <td class="tdGridInfoHeader">
                                                            <%#:Eval("Header") %>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td class="tdGridInfoDetail" id="tdDetail" runat="server">
                                                            <label id="lblVitalSignValue" runat="server">
                                                                <%#:Eval("Detail") %></label>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </li>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </td>
                                <td align="right">
                                    <input type="hidden" id="hdnIsInfectiousIconIsAllowDisplay" value="0" runat="server" />
                                    <img id="imgIsHasInfectious" runat="server" width="40" src='' alt='' title="Is Has Infectious" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr style="background-color: #EEEEEE; vertical-align: middle; width: 100%">
                    <td style="width: 40%;">
                        <table style="vertical-align: middle;">
                            <tr>
                                <td class="tdInfoHeader">
                                    <%=GetLabel("No. SEP")%>
                                </td>
                                <td class="tdInfoDetail">
                                    <label id="lblSEPNo" runat="server">
                                    </label>
                                </td>
                                <td>
                                </td>
                                <td class="tdInfoHeader">
                                    <%=GetLabel("No. SJP")%>
                                </td>
                                <td class="tdInfoDetail">
                                    <label id="lblSJPNo" runat="server">
                                    </label>
                                </td>
                                <td>
                                </td>
                                <td class="tdInfoHeader">
                                    <%=GetLabel("No. Rujukan")%>
                                </td>
                                <td class="tdInfoDetail">
                                    <label id="lblReferralNo" runat="server">
                                    </label>
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td style="width: 60%; float: right;">
                        <table>
                            <tr>
                                <td>
                                    <img id="imgIsPregnant" class="imgLink button hvr-pulse-grow" runat="server"
                                        width="35" src='' alt='' title="Pregnant" />
                                </td>
                                <td id="td1" runat="server">
                                    <img id="imgPRB" runat="server" height="40" width="40" src='' alt='' />
                                </td>
                                <td id="tdNeedTranslator" runat="server">
                                    <img id="imgNeedTranslator" runat="server" height="40" width="40" src='' alt='' />
                                </td>
                                <td>
                                    <img id="imgPackageItem" class="imgLink button hvr-pulse-grow" runat="server" height="40" width="40" src=''
                                        alt='' title="Paket Kunjungan" />
                                </td>
                                <td>
                                    <img id="imgIsHasOthersRegActive" class="imgLink button hvr-pulse-grow" runat="server"
                                        width="40" src='' alt='' title="Is Has Other Active Registration" />
                                </td>
                                <td>
                                    <img id="imgParamedicTeam" class="imgLink button hvr-pulse-grow" runat="server" width="40"
                                        src='' alt='' title="Rawat Bersama" />
                                </td>
                                <td>
                                    <img id="imgIsMultipleVisit" class="imgLink button hvr-pulse-grow" runat="server"
                                        width="40" src='' alt='' title="Multiple Visit" />
                                </td>
                                <td>
                                    <img id="imgIsVIP" runat="server" width="40" src='' alt='' title="VIP" />
                                </td>
                                <td>
                                    <img id="imgIsBlacklist" runat="server" width="40" src='' alt='' title="Blacklist" />
                                </td>
                                <td>
                                    <img id="imgIsTerminalPatient" runat="server" width="40" src='' alt='' title="Terminal Patient" />
                                </td>
                                <td>
                                    <img id="imgIsFallRisk" runat="server" width="40" src='' alt='' title="Resiko Jatuh" />
                                </td>
                                <td>
                                    <img id="imgIsPain" runat="server" width="40" src='' alt='' title="Skala Nyeri" />
                                </td>
                                <td>
                                    <img id="imgIsFastTrack" runat="server" width="40" src='' alt='' title="Fast Track" />
                                </td>
                                <td>
                                    <img id="imgIsAlive" runat="server" width="40" src='' alt='' title="RIP" />
                                </td>
                                <td>
                                    <img id="imgIsHasPhysicalLimitation" runat="server" width="40" src='' alt='' title="Keterbatasan Fisik" />
                                </td>
                                <td>
                                    <img id="imgIsHasCommunicationRestriction" runat="server" width="40" src='' alt='' title="Keterbatasan Komunikasi" />
                                </td>
                                <td>
                                    <img id="imgHasPGxTest" runat="server" width="40" src='' alt='' title="Pharmacogenomics Profile" />
                                </td>
                                <td>
                                    <img id="imgIsRAPUH" runat="server" width="40" src='' alt='' title="RAPUH" />
                                </td>
                                <td>
                                    <img id="imgVisitorRestriction" runat="server" width="40" src='' alt='' title="Visitor Restriction" />
                                </td>
                                <td>
                                    <img id="imgARInvoicePatient" class="imgLink button hvr-pulse-grow" runat="server"
                                        width="40" src='' alt='' title="Daftar Piutang Pribadi" />
                                </td>
                                <td>
                                    <img id="imgCOB" class="imgLink button hvr-pulse-grow" runat="server" width="40"
                                        src='' alt='' title="COB" />
                                </td>
                                <td id="tdLegal" runat="server" style="visibility: hidden">
                                    <img id="imgSEPInfo" runat="server" width="40" src='' alt='' title='' />
                                </td>
                                <td id="tdCoverage" runat="server">
                                    <img id="imgCoverage" runat="server" width="40" src='' alt='' />
                                </td>
                                <td>
                                    <img id="imgPatientWallet" class="imgLink button hvr-pulse-grow" runat="server" width="40"
                                        src='' alt='' />
                                </td>
                                <td id="tdRegHasPackageMCU" runat="server">
                                    <img id="imgRegHasPackageMCU" runat="server" width="40" src='' alt='' />
                                </td>
                                <td id="tdRegHasPackageAIO" runat="server">
                                    <img id="imgRegHasPackageAIO" class="imgLink button hvr-pulse-grow" runat="server"
                                        width="40" src='' alt='' />
                                </td>
                                <td id="tdOutstandingOrder" runat="server">
                                    <img id="imgOutstandingOrder" class="imgLink button hvr-pulse-grow" runat="server" width="40" src='' alt='' />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
</table>
