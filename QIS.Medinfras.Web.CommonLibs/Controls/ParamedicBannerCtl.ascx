<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ParamedicBannerCtl.ascx.cs" 
    Inherits="QIS.Medinfras.Web.CommonLibs.Controls.ParamedicBannerCtl" %>

<script type="text/javascript" id="dxss_patientbannerctl">
    $(function () {
        Methods.checkImageError('imgParamedicProfilePicture', 'paramedic');
    });
</script>
<div style="height: 80px;">
    <div style="float:left;margin:5px 0 0 5px;position: relative;">
        <img id="imgParamedicProfilePicture" class="imgParamedicProfilePicture" runat="server" src='' alt="" width="55" />
        <input type="hidden" id="hdnParamedicGender" runat="server" class="hdnParamedicGender" />
    </div>
    <table class="tblPatientBannerInfo" style="margin-left: 50px" cellpadding="0" cellspacing="0" >
        <col style="width:150px"/>
        <col style="width:230px"/>
        <col style="width:200px"/>
        <col style="width:230px"/>
        <col style="width:200px"/>
        <col />
        <tr style="font-size:1.2em">
            <td colspan="4" style="padding-left:20px"><label id="lblParamedicName" runat="server" style="font-weight:bold"></label>
            &nbsp;[<label id="lblParamedicCode" runat="server"></label>]</td>
        </tr>
        <tr>
            <td valign="top" class="tdPatientBannerLabel"><%=GetLabel("Tipe Medis")%></td>
            <td valign="top"><label id="lblParamedicType" runat="server"></label></td>
            <td valign="top" class="tdPatientBannerLabel"><%=GetLabel("Jenis Kelamin")%></td>
            <td valign="top"><label id="lblGender" runat="server"></label></td>            
        </tr>
        <tr>
            <td valign="top" class="tdPatientBannerLabel"><%=GetLabel("Spesialisasi")%></td>
            <td valign="top"><label id="lblSpecialty" runat="server"></label></td>
            <td valign="top" class="tdPatientBannerLabel"><%=GetLabel("Status Kepegawaian")%></td>
            <td valign="top"><label id="lblEmploymentStatus" runat="server"></label></td>
        </tr>
        <tr>
            <td valign="top" class="tdPatientBannerLabel"><%=GetLabel("Inisial")%></td>
            <td valign="top"><label id="lblInitial" runat="server"></label></td>
            <td valign="top" class="tdPatientBannerLabel"><%=GetLabel("Note")%></td>
            <td valign="top"><label id="lblNote" runat="server"></label></td>
        </tr>
    </table>
</div>