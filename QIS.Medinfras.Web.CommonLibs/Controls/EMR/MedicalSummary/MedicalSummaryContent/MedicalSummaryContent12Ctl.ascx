<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MedicalSummaryContent12Ctl.ascx.cs" 
    Inherits="QIS.Medinfras.Web.CommonLibs.Controls.MedicalSummaryContent12Ctl" %>

<script type="text/javascript" id="dxss_MedicalSummaryContent1Ctl">
    $(function () {
    });
</script>

<div class="w3-border divContent w3-animate-left">
    <table style="margin-top:10px; width:100%" cellpadding="0" cellspacing="0">
        <colgroup>
            <col style="width:130px" />
            <col style="width:10px; text-align: center"/>
            <col />
        </colgroup>
        <tr>
            <td colspan="3">
                <center>
	                <h2 class="w3-blue" style="text-shadow:1px 1px 0 #444"><%=GetLabel("PAGE CONTENT #12")%></h2>     
                </center>            
            </td>
        </tr>
        <tr>
            <td style="vertical-align:top">
                <img style="width:110px;height:125px" runat="server" runat="server" id="imgPatientImage" />
            </td>
            <td />
            <td>
                <table border = "0" cellpadding="0" cellspacing="0">
                    <colgroup style="width:160px"/>
                    <colgroup style="width:10px; text-align: center"/>
                    <colgroup />   
                    <tr>
                        <td colspan="3" style="width:100%"><span id="lblPatientName" runat="server" class="w3-sand w3-large" style="font-weight: bold; width:100%"></span></td>
                    </tr> 
                    <tr>
                        <td class="tdLabel"><%=GetLabel("No. Rekam Medis")%></td>
                        <td class="tdLabel"><%=GetLabel(":")%></td>
                        <td><span id="lblMedicalNo" runat="server" style="color:Black"></span></td>
                    </tr>  
                    <tr>
                        <td class="tdLabel"><%=GetLabel("Jenis Kelamin")%></td>
                        <td class="tdLabel"><%=GetLabel(":")%></td>
                        <td><span id="lblGender" runat="server" style="color:Black"></span></td>
                    </tr>                                 
                    <tr>
                        <td class="tdLabel"><%=GetLabel("Tanggal Lahir (Umur)")%></td>
                        <td class="tdLabel"><%=GetLabel(":")%></td>
                        <td><span id="lblDateOfBirth" runat="server" style="color:Black"></span></td>
                    </tr>       
                    <tr>
                        <td class="tdLabel"><%=GetLabel("Tanggal & Jam Registrasi")%></td>
                        <td class="tdLabel"><%=GetLabel(":")%></td>
                        <td><div id="lblRegistrationDateTime" runat="server" style="color:Black"></div></td>
                    </tr>      
                    <tr>
                        <td class="tdLabel"><%=GetLabel("No. Registrasi")%></td>
                        <td class="tdLabel"><%=GetLabel(":")%></td>
                        <td><div id="lblRegistrationNo" runat="server" style="color:Black"></div></td>
                    </tr> 
                    <tr>
                        <td class="tdLabel"><%=GetLabel("DPJP Utama")%></td>
                        <td class="tdLabel"><%=GetLabel(":")%></td>
                        <td><span id="lblPhysician" runat="server" style="color:Black"></span></td>
                    </tr>      
                    <tr>
                        <td class="tdLabel"><%=GetLabel("Pembayar")%></td>
                        <td class="tdLabel"><%=GetLabel(":")%></td>
                        <td><span id="lblPayerInformation" runat="server" style="color:Black"></span></td>
                    </tr>     
                    <tr>
                        <td class="tdLabel"><%=GetLabel("Lokasi Pasien")%></td>
                        <td class="tdLabel"><%=GetLabel(":")%></td>
                        <td><span id="lblPatientLocation" runat="server" style="color:Black"></span></td>
                    </tr>                                                                                                                                                                                                   
                </table>
            </td>
        </tr>
        <tr style="display:none">
            <td class="tdLabel"><%=GetLabel("Diagnosa")%></td>
            <td class="tdLabel"><%=GetLabel(":")%></td>
            <td>
                <textarea id="lblDiagnosis" runat="server" style="border:0; width:100%; height:250px; background-color: transparent" readonly></textarea>
            </td>
        </tr>
    </table>
</div>

