<%@ Page Language="C#" MasterPageFile="~/libs/MasterPage/MPTrx.master" AutoEventWireup="true"
    CodeBehind="RekapPasienDirawatDenganKomorbid.aspx.cs" Inherits="QIS.Medinfras.Web.MedicalRecord.Program.RekapPasienDirawatDenganKomorbid" %>

<asp:Content ID="Content3" ContentPlaceHolderID="plhCustomButtonToolbar" runat="server">
    <li id="btnRefresh" crudmode="R" runat="server">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbrefresh.png")%>' alt="" /><br style="clear: both" />
        <div>
            <%=GetLabel("Refresh")%></div>
    </li>
    <li id="btnImport" runat="server" crudmode="R">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbprocess.png") %>' alt="" />
        <div>
            <%=GetLabel("GET Data SiRanap")%></div>
    </li>
    <li id="btnSend" crudmode="R" runat="server">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbsendmail.png")%>' alt="" /><br style="clear: both" />
        <div>
            <%=GetLabel("Kirim Data")%></div>
    </li>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div class="menuTitle">
        <%=GetMenuCaption()%></div>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript">
        $(function () {
            setDatePicker('<%=txtObservasiDate.ClientID %>');
        });

        //#region btnRefresh
        $('#<%=btnRefresh.ClientID %>').live('click', function (evt) {
            onCustomButtonClick('refresh');
        });

        //#endregion

        //#region btnImport
        $('#<%=btnImport.ClientID %>').live('click', function (evt) {
            KemenKesService.getPasienDirawatdenganKomorbid(function (result) {
                if (result != null) {

                    var date = Methods.getDatePickerDate($('#<%=txtObservasiDate.ClientID %>').val());
                    var tanggal = Methods.dateToYMD(date);
                    var resp = result.split('|');

                    if (resp[0] == "1") {
                        var jData = JSON.parse(resp[1]);
                        var respData = jData["RekapPasienDirawatKomorbid"];
                        var isDataAvailable = false;
                        for (var i = 0; i < respData.length; i++) {
                            if (respData[i]["tanggal"] == tanggal) {
                                //parsing data to form 
                                $('#<%=txtICUdenganVentilatorSuspectL.ClientID %>').val(respData[i]["icu_dengan_ventilator_suspect_l"]);
                                $('#<%=txtICUdenganVentilatorSuspectP.ClientID %>').val(respData[i]["icu_dengan_ventilator_suspect_p"]);
                                $('#<%=txtICUdenganVentilatorConfirmL.ClientID %>').val(respData[i]["icu_dengan_ventilator_confirm_l"]);
                                $('#<%=txtICUdenganVentilatorConfirmP.ClientID %>').val(respData[i]["icu_dengan_ventilator_confirm_p"]);
                                $('#<%=txtICUtanpaVentilatorSuspectL.ClientID %>').val(respData[i]["icu_tanpa_ventilator_suspect_l"]);
                                $('#<%=txtICUtanpaVentilatorSuspectP.ClientID %>').val(respData[i]["icu_tanpa_ventilator_suspect_p"]);
                                $('#<%=txtICUtanpaVentilatorConfirmL.ClientID %>').val(respData[i]["icu_tanpa_ventilator_confirm_l"]);
                                $('#<%=txtICUtanpaVentilatorConfirmP.ClientID %>').val(respData[i]["icu_tanpa_ventilator_confirm_p"]);
                                $('#<%=txtICUTekananNegatifdenganVentilatorSuspectL.ClientID %>').val(respData[i]["icu_tekanan_negatif_dengan_ventilator_suspect_l"]);
                                $('#<%=txtICUTekananNegatifdenganVentilatorSuspectP.ClientID %>').val(respData[i]["icu_tekanan_negatif_dengan_ventilator_suspect_p"]);
                                $('#<%=txtICUTekananNegatifdenganVentilatorConfirmL.ClientID %>').val(respData[i]["icu_tekanan_negatif_dengan_ventilator_confirm_l"]);
                                $('#<%=txtICUTekananNegatifdenganVentilatorConfirmP.ClientID %>').val(respData[i]["icu_tekanan_negatif_dengan_ventilator_confirm_p"]);
                                $('#<%=txtICUTekananNegatiftanpaVentilatorSuspectL.ClientID %>').val(respData[i]["icu_tekanan_negatif_tanpa_ventilator_suspect_l"]);
                                $('#<%=txtICUTekananNegatiftanpaVentilatorSuspectP.ClientID %>').val(respData[i]["icu_tekanan_negatif_tanpa_ventilator_suspect_p"]);
                                $('#<%=txtICUTekananNegatiftanpaVentilatorConfirmL.ClientID %>').val(respData[i]["icu_tekanan_negatif_tanpa_ventilator_confirm_l"]);
                                $('#<%=txtICUTekananNegatiftanpaVentilatorConfirmP.ClientID %>').val(respData[i]["icu_tekanan_negatif_tanpa_ventilator_confirm_p"]);
                                $('#<%=txtIsolasiTekananNegatifSuspectL.ClientID %>').val(respData[i]["isolasi_tekanan_negatif_suspect_l"]);
                                $('#<%=txtIsolasiTekananNegatifSuspectP.ClientID %>').val(respData[i]["isolasi_tekanan_negatif_suspect_p"]);
                                $('#<%=txtIsolasiTekananNegatifConfirmL.ClientID %>').val(respData[i]["isolasi_tekanan_negatif_confirm_l"]);
                                $('#<%=txtIsolasiTekananNegatifConfirmP.ClientID %>').val(respData[i]["isolasi_tekanan_negatif_confirm_p"]);
                                $('#<%=txtIsolasitanpaTekananNegatifSuspectL.ClientID %>').val(respData[i]["isolasi_tekanan_negatif_confirm_p"]);
                                $('#<%=txtIsolasitanpaTekananNegatifSuspectP.ClientID %>').val(respData[i]["isolasi_tanpa_tekanan_negatif_suspect_l"]);
                                $('#<%=txtIsolasitanpaTekananNegatifConfirmL.ClientID %>').val(respData[i]["isolasi_tanpa_tekanan_negatif_suspect_p"]);
                                $('#<%=txtIsolasitanpaTekananNegatifConfirmP.ClientID %>').val(respData[i]["isolasi_tanpa_tekanan_negatif_confirm_l"]);
                                $('#<%=txtNICUKhususCovidSuspectL.ClientID %>').val(respData[i]["isolasi_tanpa_tekanan_negatif_confirm_p"]);
                                $('#<%=txtNICUKhususCovidSuspectP.ClientID %>').val(respData[i]["nicu_khusus_covid_suspect_l"]);
                                $('#<%=txtNICUKhususCovidConfirmL.ClientID %>').val(respData[i]["nicu_khusus_covid_suspect_p"]);
                                $('#<%=txtNICUKhususCovidConfirmP.ClientID %>').val(respData[i]["nicu_khusus_covid_confirm_l"]);
                                $('#<%=txtPICUKhususCovidSuspectL.ClientID %>').val(respData[i]["nicu_khusus_covid_confirm_p"]);
                                $('#<%=txtPICUKhususCovidSuspectP.ClientID %>').val(respData[i]["picu_khusus_covid_suspect_l"]);
                                $('#<%=txtPICUKhususCovidConfirmL.ClientID %>').val(respData[i]["picu_khusus_covid_confirm_l"]);
                                $('#<%=txtPICUKhususCovidConfirmP.ClientID %>').val(respData[i]["picu_khusus_covid_confirm_p"]);

                                //Show Informasi
                                $("#DataID").text(respData[i]["id"])
                                $("#DataKoders").text(respData[i]["koders"]);
                                $("#DataTanggal").text(respData[i]["tanggal"]);
                                $("#DataTanggalLapor").text(respData[i]["tgl_lapor"]);
                                $("#infoKemkes").show();
                                isDataAvailable = true;
                                break;
                            } else {
                                FormReset();
                                ResetInfo();
                                isDataAvailable = false;
                            }
                        }

                        
                    } else {

                        showToast('INFORMATION', "Tida ada data pada tanggal " + tanggal);
                        ResetInfo();
                        FormReset();
                        isDataAvailable = false;
                    }
                    ////  showToast('INFORMATION', result);
                }
            });
        });
        function ResetInfo() {
            //Show Info
            $("#DataID").text('')
            $("#DataKoders").text('');
            $("#DataTanggal").text('');
            $("#DataTanggalLapor").text('');
            $("#infoKemkes").hide();
        }
        function FormReset() {
            $('#<%=txtICUdenganVentilatorSuspectL.ClientID %>').val(0);
            $('#<%=txtICUdenganVentilatorSuspectP.ClientID %>').val(0);
            $('#<%=txtICUdenganVentilatorConfirmL.ClientID %>').val(0);
            $('#<%=txtICUdenganVentilatorConfirmP.ClientID %>').val(0);
            $('#<%=txtICUtanpaVentilatorSuspectL.ClientID %>').val(0);
            $('#<%=txtICUtanpaVentilatorSuspectP.ClientID %>').val(0);
            $('#<%=txtICUtanpaVentilatorConfirmL.ClientID %>').val(0);
            $('#<%=txtICUtanpaVentilatorConfirmP.ClientID %>').val(0);
            $('#<%=txtICUTekananNegatifdenganVentilatorSuspectL.ClientID %>').val(0);
            $('#<%=txtICUTekananNegatifdenganVentilatorSuspectP.ClientID %>').val(0);
            $('#<%=txtICUTekananNegatifdenganVentilatorConfirmL.ClientID %>').val(0);
            $('#<%=txtICUTekananNegatifdenganVentilatorConfirmP.ClientID %>').val(0);
            $('#<%=txtICUTekananNegatiftanpaVentilatorSuspectL.ClientID %>').val(0);
            $('#<%=txtICUTekananNegatiftanpaVentilatorSuspectP.ClientID %>').val(0);
            $('#<%=txtICUTekananNegatiftanpaVentilatorConfirmL.ClientID %>').val(0);
            $('#<%=txtICUTekananNegatiftanpaVentilatorConfirmP.ClientID %>').val(0);
            $('#<%=txtIsolasiTekananNegatifSuspectL.ClientID %>').val(0);
            $('#<%=txtIsolasiTekananNegatifSuspectP.ClientID %>').val(0);
            $('#<%=txtIsolasiTekananNegatifConfirmL.ClientID %>').val(0);
            $('#<%=txtIsolasiTekananNegatifConfirmP.ClientID %>').val(0);
            $('#<%=txtIsolasitanpaTekananNegatifSuspectL.ClientID %>').val(0);
            $('#<%=txtIsolasitanpaTekananNegatifSuspectP.ClientID %>').val(0);
            $('#<%=txtIsolasitanpaTekananNegatifConfirmL.ClientID %>').val(0);
            $('#<%=txtIsolasitanpaTekananNegatifConfirmP.ClientID %>').val(0);
            $('#<%=txtNICUKhususCovidSuspectL.ClientID %>').val(0);
            $('#<%=txtNICUKhususCovidSuspectP.ClientID %>').val(0);
            $('#<%=txtNICUKhususCovidConfirmL.ClientID %>').val(0);
            $('#<%=txtNICUKhususCovidConfirmP.ClientID %>').val(0);
            $('#<%=txtPICUKhususCovidSuspectL.ClientID %>').val(0);
            $('#<%=txtPICUKhususCovidSuspectP.ClientID %>').val(0);
            $('#<%=txtPICUKhususCovidConfirmL.ClientID %>').val(0);
            $('#<%=txtPICUKhususCovidConfirmP.ClientID %>').val(0);
        }
        //#endregion

        //#region btnSend
        $('#<%=btnSend.ClientID%>').live('click', function (evt) {
            var date = Methods.getDatePickerDate($('#<%=txtObservasiDate.ClientID %>').val());

            var tanggal = Methods.dateToYMD(date);
            var icu_dengan_ventilator_suspect_l = $('#<%=txtICUdenganVentilatorSuspectL.ClientID %>').val();
            var icu_dengan_ventilator_suspect_p = $('#<%=txtICUdenganVentilatorSuspectP.ClientID %>').val();
            var icu_dengan_ventilator_confirm_l = $('#<%=txtICUdenganVentilatorConfirmL.ClientID %>').val();
            var icu_dengan_ventilator_confirm_p = $('#<%=txtICUdenganVentilatorConfirmP.ClientID %>').val();
            var icu_tanpa_ventilator_suspect_l = $('#<%=txtICUtanpaVentilatorSuspectL.ClientID %>').val();
            var icu_tanpa_ventilator_suspect_p = $('#<%=txtICUtanpaVentilatorSuspectP.ClientID %>').val();
            var icu_tanpa_ventilator_confirm_l = $('#<%=txtICUtanpaVentilatorConfirmL.ClientID %>').val();
            var icu_tanpa_ventilator_confirm_p = $('#<%=txtICUtanpaVentilatorConfirmP.ClientID %>').val();
            var icu_tekanan_negatif_dengan_ventilator_suspect_l = $('#<%=txtICUTekananNegatifdenganVentilatorSuspectL.ClientID %>').val();
            var icu_tekanan_negatif_dengan_ventilator_suspect_p = $('#<%=txtICUTekananNegatifdenganVentilatorSuspectP.ClientID %>').val();
            var icu_tekanan_negatif_dengan_ventilator_confim_l = $('#<%=txtICUTekananNegatifdenganVentilatorConfirmL.ClientID %>').val();
            var icu_tekanan_negatif_dengan_ventilator_confim_p = $('#<%=txtICUTekananNegatifdenganVentilatorConfirmP.ClientID %>').val();
            var icu_tekanan_negatif_tanpa_ventilator_suspect_l = $('#<%=txtICUTekananNegatiftanpaVentilatorSuspectL.ClientID %>').val();
            var icu_tekanan_negatif_tanpa_ventilator_suspect_p = $('#<%=txtICUTekananNegatiftanpaVentilatorSuspectP.ClientID %>').val();
            var icu_tekanan_negatif_tanpa_ventilator_confirm_l = $('#<%=txtICUTekananNegatiftanpaVentilatorConfirmL.ClientID %>').val();
            var icu_tekanan_negatif_tanpa_ventilator_confirm_p = $('#<%=txtICUTekananNegatiftanpaVentilatorConfirmP.ClientID %>').val();
            var isolasi_tekanan_negatif_suspect_l = $('#<%=txtIsolasiTekananNegatifSuspectL.ClientID %>').val();
            var isolasi_tekanan_negatif_suspect_p = $('#<%=txtIsolasiTekananNegatifSuspectP.ClientID %>').val();
            var isolasi_tekanan_negatif_confirm_l = $('#<%=txtIsolasiTekananNegatifConfirmL.ClientID %>').val();
            var isolasi_tekanan_negatif_confirm_p = $('#<%=txtIsolasiTekananNegatifConfirmP.ClientID %>').val();
            
            var isolasi_tanpa_tekanan_negatif_suspect_l = $('#<%=txtIsolasitanpaTekananNegatifSuspectL.ClientID %>').val();
            var isolasi_tanpa_tekanan_negatif_suspect_p = $('#<%=txtIsolasitanpaTekananNegatifSuspectP.ClientID %>').val();
            var isolasi_tanpa_tekanan_negatif_confirm_l = $('#<%=txtIsolasitanpaTekananNegatifConfirmL.ClientID %>').val();
            var isolasi_tanpa_tekanan_negatif_confirm_p = $('#<%=txtIsolasitanpaTekananNegatifConfirmP.ClientID %>').val();
            var nicu_khusus_covid_suspect_l = $('#<%=txtNICUKhususCovidSuspectL.ClientID %>').val();
            var nicu_khusus_covid_suspect_p = $('#<%=txtNICUKhususCovidSuspectP.ClientID %>').val();
            var nicu_khusus_covid_confirm_l = $('#<%=txtNICUKhususCovidConfirmL.ClientID %>').val();
            var nicu_khusus_covid_confirm_p = $('#<%=txtNICUKhususCovidConfirmP.ClientID %>').val();
            var picu_khusus_covid_suspect_l = $('#<%=txtPICUKhususCovidSuspectL.ClientID %>').val();
            var picu_khusus_covid_suspect_p = $('#<%=txtPICUKhususCovidSuspectP.ClientID %>').val();
            var picu_khusus_covid_confirm_l = $('#<%=txtPICUKhususCovidConfirmL.ClientID %>').val();
            var picu_khusus_covid_confirm_p = $('#<%=txtPICUKhususCovidConfirmP.ClientID %>').val();

            KemenKesService.pasienDirawatdenganKomorbid(tanggal, icu_dengan_ventilator_suspect_l,icu_dengan_ventilator_suspect_p,icu_dengan_ventilator_confirm_l,icu_dengan_ventilator_confirm_p,
               icu_tanpa_ventilator_suspect_l, icu_tanpa_ventilator_suspect_p,icu_tanpa_ventilator_confirm_l, icu_tanpa_ventilator_confirm_p, 
			   icu_tekanan_negatif_dengan_ventilator_suspect_l, icu_tekanan_negatif_dengan_ventilator_suspect_p, icu_tekanan_negatif_dengan_ventilator_confim_l,
			   icu_tekanan_negatif_dengan_ventilator_confim_p, icu_tekanan_negatif_tanpa_ventilator_suspect_l, icu_tekanan_negatif_tanpa_ventilator_suspect_p,icu_tekanan_negatif_tanpa_ventilator_confirm_l,
               icu_tekanan_negatif_tanpa_ventilator_confirm_p,isolasi_tekanan_negatif_suspect_l,isolasi_tekanan_negatif_suspect_p,
               isolasi_tekanan_negatif_confirm_l, isolasi_tekanan_negatif_confirm_p, isolasi_tanpa_tekanan_negatif_suspect_l, isolasi_tanpa_tekanan_negatif_suspect_p, isolasi_tanpa_tekanan_negatif_confirm_l, isolasi_tanpa_tekanan_negatif_confirm_p, nicu_khusus_covid_suspect_l, nicu_khusus_covid_suspect_p,
               nicu_khusus_covid_confirm_l, nicu_khusus_covid_confirm_p, picu_khusus_covid_suspect_l, picu_khusus_covid_suspect_p,picu_khusus_covid_confirm_l,
               picu_khusus_covid_confirm_p, function (result) {
                   /// alert(result);
                    if (result != null) {
                        //////  showToast('INFORMATION', result);
                        var resp = result.split('|');
                        var jData = JSON.parse(resp[1]);
                        if (resp[0] == "1") {
                            var respData = jData["RekapPasienDirawatKomorbid"];
                            showToast(respData[0]["message"]);
                        } else {
                            showToast(resp[1]);
                        }
                    }
                });
        });
            //#endregion

    </script>
    <table>
        <tr>
            <td>
                <th>
                    Tanggal Observasi
                </th>
            </td>
            <td>
                <asp:TextBox ID="txtObservasiDate" Width="120px" runat="server" CssClass="datepicker" />
            </td>
        </tr>
    </table>
     <div style="width: 25%;margin-bottom: 30px; display:none;" id="infoKemkes">
    <div class="w3-card w3-card w3-white">
     <header class="w3-container w3-blue">
        <h2>Info Data Kemenkes</h2> 
     </header>
      
     <table class="tblInfoTT w3-table w3-bordered w3-card-4">
      <colgroup>
            <col />
            <col style="width: 200px" />
        </colgroup>
        <tr>
            <td>ID</td>
            <td><label id="DataID"></label></td>
        </tr>
         <tr>
            <td>Koders</td>
            <td><label id="DataKoders"></label></td>
        </tr>
        <tr>
            <td>Tanggal</td>
            <td><label id="DataTanggal"></label></td>
        </tr>
         <tr>
            <td>Tanggal Lapor</td>
            <td><label id="DataTanggalLapor"></label></td>
        </tr>
     </table>
    </div>
    </div>
 
    <div class="w3-card w3-card w3-white">
     <header class="w3-container w3-blue">
        <h2>Form Rekap Pasien Dirawat dengan Komorbid</h2> 
     </header>
    <table class="tblInfoTT w3-table w3-bordered w3-card-4 w3-card-white">
        <colgroup>
            <col />
            <col style="width: 80px" />
            <col style="width: 80px" />
            <col style="width: 80px" />
            <col style="width: 80px" />
            <col style="width: 80px" />
            <col style="width: 80px" />
        </colgroup>
        <tr>
            <th>
                Jenis Pasien
            </th>
            <th>
            </th>
            <th colspan="2" class="w3-center">
                Jumlah Pasien
            </th>
        </tr>
        <tr>
            <th>
            </th>
            <th>
            </th>
            <th class="w3-center">
                Laki-laki
            </th>
            <th class="w3-center">
                Perempuan
            </th>
        </tr>
        <tr>
            <td>
                ICU dengan Ventilator Suspect
            </td>
            <td>
            </td>
            <td>
                <asp:TextBox ID="txtICUdenganVentilatorSuspectL" Width="60px" runat="server" CssClass="txtParam txtICUdenganVentilatorSuspectl"
                    controlName="ICU dengan Ventilator Suspect L" />
            </td>
            <td>
                <asp:TextBox ID="txtICUdenganVentilatorSuspectP" Width="60px" runat="server" CssClass="txtParam txtICUdenganVentilatorSuspectP"
                    controlName="ICU dengan Ventilator Suspect P" />
            </td>
        </tr>
        <tr>
            <td>
                ICU dengan Ventilator Confirm
            </td>
            <td>
            </td>
            <td>
                <asp:TextBox ID="txtICUdenganVentilatorConfirmL" Width="60px" runat="server" CssClass="txtParam txtICUdenganVentilatorConfirmL"
                    controlName="ICU dengan Ventilator Confirm L" />
            </td>
            <td>
                <asp:TextBox ID="txtICUdenganVentilatorConfirmP" Width="60px" runat="server" CssClass="txtParam txtICUdenganVentilatorConfirmP"
                    controlName="ICU dengan Ventilator Confirm P" />
            </td>
        </tr>
        <tr>
            <td>
                ICU tanpa Ventilator Suspect
            </td>
            <td>
            </td>
            <td>
                <asp:TextBox ID="txtICUtanpaVentilatorSuspectL" Width="60px" runat="server" CssClass="txtParam txtICUtanpaVentilatorSuspectL"
                    controlName="ICU tanpa Ventilator Suspect L" />
            </td>
            <td>
                <asp:TextBox ID="txtICUtanpaVentilatorSuspectP" Width="60px" runat="server" CssClass="txtParam txtICUtanpaVentilatorSuspectP"
                    controlName="ICU tanpa Ventilator Suspect P" />
            </td>
        </tr>
        <tr>
            <td>
                ICU tanpa Ventilator Confirm
            </td>
            <td>
            </td>
            <td>
                <asp:TextBox ID="txtICUtanpaVentilatorConfirmL" Width="60px" runat="server" CssClass="txtParam txtICUtanpaVentilatorConfirmL"
                    controlName="ICU tanpa Ventilator Confirm L" />
            </td>
            <td>
                <asp:TextBox ID="txtICUtanpaVentilatorConfirmP" Width="60px" runat="server" CssClass="txtParam txtICUtanpaVentilatorConfirmP"
                    controlName="ICU tanpa Ventilator Confirm P" />
            </td>
        </tr>
        <tr>
            <td>
                ICU Tekanan Negatif dengan Ventilator Suspect
            </td>
            <td>
            </td>
            <td>
                <asp:TextBox ID="txtICUTekananNegatifdenganVentilatorSuspectL" Width="60px" runat="server"
                    CssClass="txtParam txtICUTekananNegatifdenganVentilatorSuspectL" controlName="ICU Tekanan Negatif dengan Ventilator Suspect L" />
            </td>
            <td>
                <asp:TextBox ID="txtICUTekananNegatifdenganVentilatorSuspectP" Width="60px" runat="server"
                    CssClass="txtParam txtICUTekananNegatifdenganVentilatorSuspectP" controlName="ICU Tekanan Negatif dengan Ventilator Suspect P" />
            </td>
        </tr>
        <tr>
            <td>
                ICU Tekanan Negatif dengan Ventilator Confirm
            </td>
            <td>
            </td>
            <td>
                <asp:TextBox ID="txtICUTekananNegatifdenganVentilatorConfirmL" Width="60px" runat="server"
                    CssClass="txtParam txtICUTekananNegatifdenganVentilatorConfirmL" controlName="ICU Tekanan Negatif dengan Ventilator Confirm L" />
            </td>
            <td>
                <asp:TextBox ID="txtICUTekananNegatifdenganVentilatorConfirmP" Width="60px" runat="server"
                    CssClass="txtParam txtICUTekananNegatifdenganVentilatorConfirmP" controlName="ICU Tekanan Negatif dengan Ventilator Confirm P" />
            </td>
        </tr>
        <tr>
            <td>
                ICU Tekanan Negatif tanpa Ventilator Suspect
            </td>
            <td>
            </td>
            <td>
                <asp:TextBox ID="txtICUTekananNegatiftanpaVentilatorSuspectL" Width="60px" runat="server"
                    CssClass="txtParam txtICUTekananNegatiftanpaVentilatorSuspectL" controlName="ICU Tekanan Negatif tanpa Ventilator Suspect L" />
            </td>
            <td>
                <asp:TextBox ID="txtICUTekananNegatiftanpaVentilatorSuspectP" Width="60px" runat="server"
                    CssClass="txtParam txtICUTekananNegatiftanpaVentilatorSuspectP" controlName="ICU Tekanan Negatif tanpa Ventilator Suspect P" />
            </td>
        </tr>
        <tr>
            <td>
                ICU Tekanan Negatif tanpa Ventilator Confirm
            </td>
            <td>
            </td>
            <td>
                <asp:TextBox ID="txtICUTekananNegatiftanpaVentilatorConfirmL" Width="60px" runat="server"
                    CssClass="txtParam txtICUTekananNegatiftanpaVentilatorConfirmL" controlName="ICU Tekanan Negatif tanpa Ventilator Confirm L" />
            </td>
            <td>
                <asp:TextBox ID="txtICUTekananNegatiftanpaVentilatorConfirmP" Width="60px" runat="server"
                    CssClass="txtParam txtICUTekananNegatiftanpaVentilatorConfirmP" controlName="ICU Tekanan Negatif tanpa Ventilator Confirm P" />
            </td>
        </tr>
        <tr>
            <td>
                ISOLASI Tekanan Negatif Suspect
            </td>
            <td>
            </td>
            <td>
                <asp:TextBox ID="txtIsolasiTekananNegatifSuspectL" Width="60px" runat="server" CssClass="txtParam txtIsolasiTekananNegatifSuspectL"
                    controlName="ISOLASI Tekanan Negatif Suspect L" />
            </td>
            <td>
                <asp:TextBox ID="txtIsolasiTekananNegatifSuspectP" Width="60px" runat="server" CssClass="txtParam txtIsolasiTekananNegatifSuspectP"
                    controlName="ISOLASI Tekanan Negatif Suspect P" />
            </td>
        </tr>
        <tr>
            <td>
                ISOLASI Tekanan Negatif Confirm
            </td>
            <td>
            </td>
            <td>
                <asp:TextBox ID="txtIsolasiTekananNegatifConfirmL" Width="60px" runat="server" CssClass="txtParam txtIsolasiTekananNegatifConfirmL"
                    controlName="ISOLASI Tekanan Negatif Confirm L" />
            </td>
            <td>
              <asp:TextBox ID="txtIsolasiTekananNegatifConfirmP" Width="60px" runat="server" CssClass="txtParam txtIsolasiTekananNegatifConfirmP"
                    controlName="ISOLASI Tekanan Negatif Confirm P" />
            </td>
        </tr>
        <tr>
            <td>
                ISOLASI tanpa Tekanan Negatif Suspect
            </td>
            <td>
            </td>
            <td>
                <asp:TextBox ID="txtIsolasitanpaTekananNegatifSuspectL" Width="60px" runat="server"
                    CssClass="txtParam txtIsolasitanpaTekananNegatifSuspectL" controlName="ISOLASI tanpa Tekanan Negatif Suspect L" />
            </td>
            <td>
                <asp:TextBox ID="txtIsolasitanpaTekananNegatifSuspectP" Width="60px" runat="server"
                    CssClass="txtParam txtIsolasitanpaTekananNegatifSuspectP" controlName="ISOLASI tanpa Tekanan Negatif Suspect P" />
            </td>
        </tr>
        <tr>
            <td>
                ISOLASI tanpa Tekanan Negatif Confirm
            </td>
            <td>
            </td>
            <td>
                <asp:TextBox ID="txtIsolasitanpaTekananNegatifConfirmL" Width="60px" runat="server"
                    CssClass="txtParam txtIsolasitanpaTekananNegatifConfirmL" controlName="ISOLASI tanpa Tekanan Negatif Confirm L" />
            </td>
            <td>
                <asp:TextBox ID="txtIsolasitanpaTekananNegatifConfirmP" Width="60px" runat="server"
                    CssClass="txtParam txtIsolasitanpaTekananNegatifConfirmP" controlName="ISOLASI tanpa Tekanan Negatif Confirm P" />
            </td>
        </tr>
        <tr>
            <td>
                NICU Khusus Covid Suspect
            </td>
            <td>
            </td>
            <td>
                <asp:TextBox ID="txtNICUKhususCovidSuspectL" Width="60px" runat="server" CssClass="txtParam txtIsolasiKhususCovidSuspectL"
                    controlName="NICU Khusus Covid Suspect L" />
            </td>
            <td>
                <asp:TextBox ID="txtNICUKhususCovidSuspectP" Width="60px" runat="server" CssClass="txtParam txtIsolasiKhususCovidSuspectP"
                    controlName="NICU Khusus Covid Suspect P" />
            </td>
        </tr>
        <tr>
            <td>
                NICU Khusus Covid Confirm
            </td>
            <td>
            </td>
            <td>
                <asp:TextBox ID="txtNICUKhususCovidConfirmL" Width="60px" runat="server" CssClass="txtParam txtNICUKhususCovidConfirmL"
                    controlName="NICU Khusus Covid Confirm L" />
            </td>
            <td>
                <asp:TextBox ID="txtNICUKhususCovidConfirmP" Width="60px" runat="server" CssClass="txtParam txtNICUKhususCovidConfirmP"
                    controlName="NICU Khusus Covid Confirm P" />
            </td>
        </tr>
        <tr>
            <td>
                PICU Khusus Covid Suspect
            </td>
            <td>
            </td>
            <td>
                <asp:TextBox ID="txtPICUKhususCovidSuspectL" Width="60px" runat="server" CssClass="txtParam txtPICUKhususCovidSuspectL"
                    controlName="PICU Khusus Covid Suspect L" />
            </td>
            <td>
                <asp:TextBox ID="txtPICUKhususCovidSuspectP" Width="60px" runat="server" CssClass="txtParam txtPICUKhususCovidSuspectP"
                    controlName="PICU Khusus Covid Suspect P" />
            </td>
        </tr>
        <tr>
            <td>
                PICU Khusus Covid Confirm
            </td>
            <td>
            </td>
            <td>
                <asp:TextBox ID="txtPICUKhususCovidConfirmL" Width="60px" runat="server" CssClass="txtParam txtPICUKhususCovidConfirmL"
                    controlName="PICU Khusus Covid Confirm L" />
            </td>
            <td>
                <asp:TextBox ID="txtPICUKhususCovidConfirmP" Width="60px" runat="server" CssClass="txtParam txtPICUKhususCovidConfirmP"
                    controlName="PICU Khusus Covid Confirm P" />
            </td>
        </tr>
    </table>
    </div>
</asp:Content>
