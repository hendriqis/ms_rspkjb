<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MedicalSummaryContent13Ctl.ascx.cs" 
    Inherits="QIS.Medinfras.Web.CommonLibs.Controls.MedicalSummaryContent13Ctl" %>

<script type="text/javascript" id="dxss_MedicalSummaryContent13Ctl">
    $(function () {
        $('#contentDetail13NavPane a').click(function () {
            $('#contentDetail13NavPane a.w3-red').removeClass('w3-red');
            $(this).addClass('w3-red');
            var contentID = $(this).attr('contentID');

            if (contentID != null) {
                showDetailContent(contentID);
            }
        });
    });

    function showDetailContent(contentID) {
        var i, x, tablinks;
        x = document.getElementsByClassName("content13Detail");
        for (i = 0; i < x.length; i++) {
            x[i].style.display = "none";
        }
        document.getElementById(contentID).style.display = "block";
    }

    function openCity(evt, cityName) {
        var i, x, tablinks;
        x = document.getElementsByClassName("content13Detail");
        for (i = 0; i < x.length; i++) {
            x[i].style.display = "none";
        }
        tablinks = document.getElementsByClassName("tablink");
        for (i = 0; i < x.length; i++) {
            tablinks[i].className = tablinks[i].className.replace(" w3-red", "");
        }
        document.getElementById(cityName).style.display = "block";
        evt.currentTarget.className += " w3-red";
    }   
</script>

<div class="w3-border divContent w3-animate-left">
    <table style="margin-top:10px; width:100%" cellpadding="0" cellspacing="0">
        <colgroup>
            <col style="width:130px" />
            <col style="width:10px; text-align: center"/>
            <col />
        </colgroup>
        <tr>
            <td colspan="3" style="width:100%">
                <div id="contentDetail13NavPane" class="w3-bar w3-black">
                    <a href="javascript:void(0)" class="w3-bar-item w3-button tablink w3-red">London</a>
                    <a href="javascript:void(0)" class="w3-bar-item w3-button tablink">Paris</a>
                    <a href="javascript:void(0)" class="w3-bar-item w3-button tablink">Tokyo</a>
                </div>
  
                <div id="London" class="w3-container w3-border content13Detail">
                <h2>London</h2>
                <p>London is the capital city of England.</p>
                </div>

                <div id="Paris" class="w3-container w3-border content13Detail" style="display:none">
                <h2>Paris</h2>
                <p>Paris is the capital of France.</p> 
                </div>

                <div id="Tokyo" class="w3-container w3-border content13Detail" style="display:none">
                <h2>Tokyo</h2>
                <p>Tokyo is the capital of Japan.</p>
                </div>
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

