<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PatientDtBannerCtl.ascx.cs" 
    Inherits="QIS.Medinfras.Web.CommonLibs.Controls.PatientDtBannerCtl" %>

<script type="text/javascript" id="dxss_patientbannerctl">
    $(function () {
        var gender = $('#<%=hdnPatientGender.ClientID %>').val();
        Methods.checkImageError('imgPatientProfilePicture', 'patient', gender);
    });
</script>
 <div style="height: 80px;">
    <div style="float:left;margin:5px 0 0 5px;position: relative;">
        <img id="imgPatientProfilePicture" class="imgPatientProfilePicture" runat="server" src='' alt="" width="55" height="65" />
        <input type="hidden" id="hdnPatientGender" runat="server" class="hdnPatientGender" />
    </div>
    <table class="tblPatientBannerInfo" style="margin-left: 50px" cellpadding="0" cellspacing="0" >
        <col style="width:100px"/>
        <col style="width:140px"/>
        <col style="width:100px"/>
        <col style="width:180px"/>
        <col style="width:100px"/>
        <col style="width:240px"/>
        <tr style="font-size:1.2em">
            <td colspan="3" style="padding-left:20px"><label id="lblPatientName" runat="server" style="font-weight:bold"></label></td>
        </tr>
        <tr>
            <td class="tdPatientBannerLabel"><%=GetLabel("MRN")%></td>
            <td><label id="lblMRN" runat="server"></label></td>
            <td class="tdPatientBannerLabel"><%=GetLabel("DOB")%></td>
            <td><label id="lblDOB" runat="server"></label></td>
        </tr>
        <tr>
            <td class="tdPatientBannerLabel"><%=GetLabel("Allergy")%></td>
            <td><label id="lblAllergy" runat="server" style="color:Red;font-weight:bold"></label></td>
            <td class="tdPatientBannerLabel"><%=GetLabel("Age")%></td>
            <td><label id="lblPatientAge" runat="server"></label></td>
        </tr>
        <tr>
            <td class="tdPatientBannerLabel"><%=GetLabel("Note")%></td>
            <td><label id="lblNote" runat="server"></label></td>
            <td class="tdPatientBannerLabel"><%=GetLabel("Gender")%></td>
            <td><label id="lblGender" runat="server"></label></td>
        </tr>
    </table>
</div>