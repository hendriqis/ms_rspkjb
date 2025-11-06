<%@ Page Language="C#" MasterPageFile="~/libs/MasterPage/MPTrx.master" AutoEventWireup="true"
    CodeBehind="RekapPasienKeluar.aspx.cs" Inherits="QIS.Medinfras.Web.MedicalRecord.Program.RekapPasienKeluar" %>

<asp:Content ID="Content3" ContentPlaceHolderID="plhCustomButtonToolbar" runat="server">
   <li id="btnRefresh" crudmode="R" runat="server">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbrefresh.png")%>' alt="" /><br style="clear: both" />
        <div>
            <%=GetLabel("Refresh")%></div>
    </li>
     <li id="btnImport" runat="server" crudmode="R">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbprocess.png") %>' alt="" />
        <div> <%=GetLabel("GET Data SiRanap")%></div>
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
            KemenKesService.getPasienKeluar(function (result) {
                if (result != null) {
                    var date = Methods.getDatePickerDate($('#<%=txtObservasiDate.ClientID %>').val());
                    var tanggal = Methods.dateToYMD(date);
                    var resp = result.split('|');
                    console.log(resp);
                    if (resp[0] == "1") {
                        var jData = JSON.parse(resp[1]);
                        var respData = jData["RekapPasienKeluar"];
                        var isDataAvailable = false;
                        for (var i = 0; i < respData.length; i++) {
                            if (respData[i]["tanggal"] == tanggal) {
                                //parsing data to form 
                                $('#<%= txtAps.ClientID %>').val(respData[i]["aps"]);
                                $('#<%=txtDirujuk.ClientID %>').val(respData[i]["dirujuk"]);
                                $('#<%=txtDiscarded.ClientID %>').val(respData[i]["discarded"]);
                                $('#<%=txtIsman.ClientID %>').val(respData[i]["isman"]);
                                $('#<%=txtMeninggalDiscardedKomorbid.ClientID %>').val(respData[i]["meninggal_disarded_komorbid"]);
                                $('#<%=txtMeninggalDiscardedTanpaKomorbid.ClientID %>').val(respData[i]["meninggal_discarded_tanpa_komorbid"]);
                                $('#<%=txtMeninggalKomorbid.ClientID %>').val(respData[i]["meninggal_komorbid"]);
                                $('#<%=txtMeninggalProbAnakKomorbid.ClientID %>').val(respData[i]["meninggal_prob_anak_komorbid"]);
                                $('#<%=txtMeninggalProbAnakTanpaKomorbid.ClientID %>').val(respData[i]["meninggal_prob_anak_tanpa_komorbid"]);
                                $('#<%=txtMeninggalProbBalitaKomorbid.ClientID %>').val(respData[i]["meninggal_prob_balita_komorbid"]);
                                $('#<%=txtMeninggalProbBalitaTanpaKomorbid.ClientID %>').val(respData[i]["meninggal_prob_balita_tanpa_komorbid"]);
                                $('#<%=txtMeninggalProbBayiKomorbid.ClientID %>').val(respData[i]["meninggal_prob_bayi_komorbid"]);
                                $('#<%=txtMeninggalProbBayiTanpaKomorbid.ClientID %>').val(respData[i]["meninggal_prob_bayi_Tanpa_komorbid"]);
                                $('#<%=txtmeninggalProbDwsKomorbid.ClientID %>').val(respData[i]["meninggal_prob_dws_komorbid"]);
                                $('#<%=txtMeninggalProbDwsTanpaKomorbid.ClientID %>').val(respData[i]["meninggal_prob_dws_tanpa_komorbid"]);
                                $('#<%=txtMeninggalProbLansiaKomorbid.ClientID %>').val(respData[i]["meninggal_prob_lansia_komorbid"]);
                                $('#<%=txtMeninggalProbLansiaTanpaKomorbid.ClientID %>').val(respData[i]["meninggal_prob_lansia_tanpa_komorbid"]);
                                $('#<%=txtMeninggalProbNeoKomorbid.ClientID %>').val(respData[i]["meninggal_prob_neo_komorbid"]);
                                $('#<%=txtMeninggalProbNeoTanpaKomorbid.ClientID %>').val(respData[i]["meninggal_prob_neo_tanpa_komorbid"]);
                                $('#<%=txtMeninggalProbPrekomorbid.ClientID %>').val(respData[i]["meninggal_prob_pre_komorbid"]);
                                $('#<%=txtSembuh.ClientID %>').val(respData[i]["sembuh"]);
                                $('#<%=txtMeninggalTanpaKomorbid.ClientID %>').val(respData[i]["meninggal_tanpa_komorbid"]);
                                $('#<%=txtMeninggalProbRemajaKomorbid.ClientID %>').val(respData[i]["meninggal_prob_remaja_komorbid"]);
                                $('#<%=txtMeninggalProbRemajaTanpaKomorbid.ClientID %>').val(respData[i]["meninggal_prob_remaja_tanpa_komorbid"]);
                                $('#<%= txtMeninggalProbPreTanpaKomorbid.ClientID %>').val(respData[i]["meninggal_prob_pre_tanpa_komorbid"]);
                                $('#<%= txtMeninggalProbBayiTanpaKomorbid.ClientID %>').val(respData[i]["meninggal_prob_bayi_tanpa_komorbid"]);
                                $('#<%= txtMeninggalDiscardedKomorbid.ClientID %>').val(respData[i]["meninggal_discarded_komorbid"]);


                                //Show Informasi
                                $("#DataID").text(respData[i]["id"])
                                $("#DataKoders").text(respData[i]["koders"]);
                                $("#DataTanggal").text(respData[i]["tanggal"]);
                                $("#DataTanggalLapor").text(respData[i]["tgl_lapor"]);
                                $("#infoKemkes").show();
                                isDataAvailable = true;
                                break;
                            } else {

                                isDataAvailable = false;
                            }
                        }



                    } else {

                        showToast('INFORMATION', "Tidak ada data pada tanggal " + tanggal);
                        isDataAvailable = false;
                        ResetInfo();
                        FormReset(); 
                    }
                    //////showToast('INFORMATION', result);
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
            $('#<%= txtAps.ClientID %>').val(0);
            $('#<%=txtDirujuk.ClientID %>').val(0);
            $('#<%=txtDiscarded.ClientID %>').val(0);
            $('#<%=txtIsman.ClientID %>').val(0);
            $('#<%=txtMeninggalDiscardedKomorbid.ClientID %>').val(0);
            $('#<%=txtMeninggalDiscardedTanpaKomorbid.ClientID %>').val(0);
            $('#<%=txtMeninggalKomorbid.ClientID %>').val(0);
            $('#<%=txtMeninggalProbAnakKomorbid.ClientID %>').val(0);
            $('#<%=txtMeninggalProbAnakTanpaKomorbid.ClientID %>').val(0);
            $('#<%=txtMeninggalProbBalitaKomorbid.ClientID %>').val(0);
            $('#<%=txtMeninggalProbBalitaTanpaKomorbid.ClientID %>').val(0);
            $('#<%=txtMeninggalProbBayiKomorbid.ClientID %>').val(0);
            $('#<%=txtMeninggalProbBayiTanpaKomorbid.ClientID %>').val(0);
            $('#<%=txtmeninggalProbDwsKomorbid.ClientID %>').val(0);
            $('#<%=txtMeninggalProbDwsTanpaKomorbid.ClientID %>').val(0);
            $('#<%=txtMeninggalProbLansiaKomorbid.ClientID %>').val(0);
            $('#<%=txtMeninggalProbLansiaTanpaKomorbid.ClientID %>').val(0);
            $('#<%=txtMeninggalProbNeoKomorbid.ClientID %>').val(0);
            $('#<%=txtMeninggalProbNeoTanpaKomorbid.ClientID %>').val(0);
            $('#<%=txtMeninggalProbPrekomorbid.ClientID %>').val(0);
            $('#<%=txtSembuh.ClientID %>').val(0);
            $('#<%=txtMeninggalTanpaKomorbid.ClientID %>').val(0);
            $('#<%=txtMeninggalProbRemajaKomorbid.ClientID %>').val(0);
            $('#<%=txtMeninggalProbRemajaTanpaKomorbid.ClientID %>').val(0);
            $('#<%= txtMeninggalProbPreTanpaKomorbid.ClientID %>').val(0);
            $('#<%= txtMeninggalProbBayiTanpaKomorbid.ClientID %>').val(0);
            $('#<%= txtMeninggalDiscardedKomorbid.ClientID %>').val(0);
        }
        //#endregion

        //#region btnSend
        $('#<%=btnSend.ClientID%>').live('click', function (evt) {
            var date = Methods.getDatePickerDate($('#<%=txtObservasiDate.ClientID %>').val());
            var tanggal = Methods.dateToYMD(date);
           
            var dirujuk = $('#<%=txtDirujuk.ClientID %>').val();
            var discarded = $('#<%=txtDiscarded.ClientID %>').val();
            var isman  = $('#<%=txtIsman.ClientID %>').val();
            var meninggal_disarded_komorbid = $('#<%=txtMeninggalDiscardedKomorbid.ClientID %>').val();
            var meninggal_discarded_tanpa_komorbid = $('#<%=txtMeninggalDiscardedTanpaKomorbid.ClientID %>').val();
            var meninggal_komorbid = $('#<%=txtMeninggalKomorbid.ClientID %>').val();
            var meninggal_prob_anak_komorbid = $('#<%=txtMeninggalProbAnakKomorbid.ClientID %>').val();
            var meninggal_prob_anak_tanpa_komorbid = $('#<%=txtMeninggalProbAnakTanpaKomorbid.ClientID %>').val();
            var meninggal_prob_balita_komorbid = $('#<%=txtMeninggalProbBalitaKomorbid.ClientID %>').val();
            var meninggal_prob_balita_tanpa_komorbid = $('#<%=txtMeninggalProbBalitaTanpaKomorbid.ClientID %>').val();
            var meninggal_prob_bayi_komorbid = $('#<%=txtMeninggalProbBayiKomorbid.ClientID %>').val();
            var meninggal_prob_bayi_Tanpa_komorbid = $('#<%=txtMeninggalProbBayiTanpaKomorbid.ClientID %>').val();
            var meninggal_prob_dws_komorbid = $('#<%=txtmeninggalProbDwsKomorbid.ClientID %>').val();
            var meninggal_prob_dws_tanpa_komorbid = $('#<%=txtMeninggalProbDwsTanpaKomorbid.ClientID %>').val();
            var meninggal_prob_lansia_komorbid = $('#<%=txtMeninggalProbLansiaKomorbid.ClientID %>').val();
            var meninggal_prob_lansia_tanpa_komorbid = $('#<%=txtMeninggalProbLansiaTanpaKomorbid.ClientID %>').val();
            var meninggal_prob_neo_komorbid = $('#<%=txtMeninggalProbNeoKomorbid.ClientID %>').val();
            var meninggal_prob_neo_tanpa_komorbid = $('#<%=txtMeninggalProbNeoTanpaKomorbid.ClientID %>').val();
            var meninggal_prob_pre_komorbid = $('#<%=txtMeninggalProbPrekomorbid.ClientID %>').val();
            var sembuh = $('#<%=txtSembuh.ClientID %>').val();
            var meninggal_tanpa_komorbid = $('#<%=txtMeninggalTanpaKomorbid.ClientID %>').val();
            var meninggal_prob_remaja_komorbid = $('#<%=txtMeninggalProbRemajaKomorbid.ClientID %>').val();
            var meninggal_prob_remaja_tanpa_komorbid = $('#<%=txtMeninggalProbRemajaTanpaKomorbid.ClientID %>').val();
            var meninggal_prob_pre_tanpa_komorbid = $('#<%= txtMeninggalProbPreTanpaKomorbid.ClientID %>').val();
            var meninggal_prob_bayi_tanpa_komorbid = $('#<%= txtMeninggalProbBayiTanpaKomorbid.ClientID %>').val();
            var meninggal_discarded_komorbid = $('#<%= txtMeninggalDiscardedKomorbid.ClientID %>').val();
            var aps = $('#<%= txtAps.ClientID %>').val();
            KemenKesService.pasienKeluar(tanggal,
                                          sembuh,
                                          discarded,
                                          meninggal_komorbid,
                                          meninggal_tanpa_komorbid,
                                          meninggal_prob_pre_komorbid,
                                          meninggal_prob_neo_komorbid,
                                          meninggal_prob_bayi_komorbid,
                                          meninggal_prob_balita_komorbid,
                                          meninggal_prob_anak_komorbid,
                                          meninggal_prob_remaja_komorbid,
                                          meninggal_prob_dws_komorbid,
                                          meninggal_prob_lansia_komorbid,
                                          meninggal_prob_pre_tanpa_komorbid,
                                          meninggal_prob_neo_tanpa_komorbid,
                                          meninggal_prob_bayi_tanpa_komorbid,
                                          meninggal_prob_balita_tanpa_komorbid,
                                          meninggal_prob_anak_tanpa_komorbid,
                                          meninggal_prob_remaja_tanpa_komorbid,
                                          meninggal_prob_dws_tanpa_komorbid,
                                          meninggal_prob_lansia_tanpa_komorbid,
                                          meninggal_disarded_komorbid,
                                          meninggal_discarded_tanpa_komorbid,
                                          dirujuk,
                                          isman,
                                          aps, function (result) {
                  ///// alert(result);
                   if (result != null) {
                       ///// showToast('INFORMATION', result);

                       if (result != null) {
                           //////  showToast('INFORMATION', result);
                           var resp = result.split('|');
                           var jData = JSON.parse(resp[1]);
                           if (resp[0] == "1") {
                               var respData = jData["RekapPasienKeluar"];
                               showToast(respData[0]["message"]);
                           } else {
                               showToast(resp[1]);
                           }
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
                    Tanggal Pulang
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
     <table class="tblInfoTT w3-table w3-bordered w3-card-4  w3-white">
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
        <h2>Form Rekap Pasien Pulang</h2> 
     </header>
    <table class="tblInfoTT w3-table w3-bordered w3-card-4 w3-white">
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
            <td>
                 Sembuh
            </td>
            <td>
            </td>
            <td>
                <asp:TextBox ID="txtSembuh" Width="60px" runat="server" CssClass="txtParam txtSembuh"
                    controlName="Sembuh" />
            </td>
             
        </tr>
        <tr>
            <td>
             Discarded 
            </td>
            <td>
            </td>
            <td>
                <asp:TextBox ID="txtDiscarded" Width="60px" runat="server" CssClass="txtParam txtDiscarded"
                    controlName="Discarded" />
            </td>
           
        </tr>
        <tr>
            <td>
                Meninggal Komorbid
            </td>
            <td>
            </td>
            <td>
                <asp:TextBox ID="txtMeninggalKomorbid" Width="60px" runat="server" CssClass="txtParam txtMeninggalKomorbid"
                    controlName="txtMeninggalKomorbid" />
            </td>
          
        </tr>
        <tr>
            <td>
                Meninggal Tanpa Komorbid
            </td>
            <td>
            </td>
            <td>
                <asp:TextBox ID="txtMeninggalTanpaKomorbid" Width="60px" runat="server" CssClass="txtParam txtMeninggalTanpaKomorbid"
                    controlName="Meninggal Tanpa Komorbid " />
            </td>
             
        </tr>
        <tr>
            <td>
               Meninggal Prob Pre komorbid
            </td>
            <td>
            </td>
            <td>
                <asp:TextBox ID="txtMeninggalProbPrekomorbid" Width="60px" runat="server"
                    CssClass="txtParam txtMeninggalProbPrekomorbid" controlName="Meninggal Prob Prekomorbid" />
            </td>
            
        </tr>
        <tr>
            <td>
                Meninggal Prob Neo Komorbid
            </td>
            <td>
            </td>
            <td>
                <asp:TextBox ID="txtMeninggalProbNeoKomorbid" Width="60px" runat="server"
                    CssClass="txtParam txtMeninggalProbNeoKomorbid" controlName="Meninggal Prob Neo Komorbid" />
            </td>
             
        </tr>
        <tr>
            <td>
               Meninggal Prob Bayi Komorbid
            </td>
            <td>
            </td>
            <td>
                <asp:TextBox ID="txtMeninggalProbBayiKomorbid" Width="60px" runat="server"
                    CssClass="txtParam txtMeninggalProbBayiKomorbid" controlName="Meninggal Prob Bayi Komorbid" />
            </td>
            
        </tr>
        <tr>
            <td>
               Meninggal Prob Balita Komorbid
            </td>
            <td>
            </td>
            <td>
                <asp:TextBox ID="txtMeninggalProbBalitaKomorbid" Width="60px" runat="server"
                    CssClass="txtParam txtMeninggalProbBalitaKomorbid" controlName="Meninggal Prob Balita Komorbid" />
            </td>
             
        </tr>
        <tr>
            <td>
                Meninggal Prob Anak Komorbid
            </td>
            <td>
            </td>
            <td>
                <asp:TextBox ID="txtMeninggalProbAnakKomorbid" Width="60px" runat="server" CssClass="txtParam txtMeninggalProbAnakKomorbid"
                    controlName=" Meninggal Prob Anak Komorbid " />
            </td>
            
        </tr>
        <tr>
            <td>
               Meninggal Prob Remaja Komorbid
            </td>
            <td>
            </td>
            <td>
                <asp:TextBox ID="txtMeninggalProbRemajaKomorbid" Width="60px" runat="server" CssClass="txtParam txtMeninggalProbRemajaKomorbid"
                    controlName="Meninggal Prob Remaja Komorbid" />
            </td>
            
        </tr>
         <tr>
            <td>
               Meninggal Prob Remaja Tanpa Komorbid
            </td>
            <td>
            </td>
            <td>
                <asp:TextBox ID="txtMeninggalProbRemajaTanpaKomorbid" Width="60px" runat="server" CssClass="txtParam txtMeninggalProbRemajaTanpaKomorbid"
                    controlName="Meninggal Prob Remaja Komorbid" />
            </td>
            
        </tr>
        <tr>
            <td>
                Meninggal Prob Dws Komorbid
            </td>
            <td>
            </td>
            <td>
                <asp:TextBox ID="txtmeninggalProbDwsKomorbid" Width="60px" runat="server"
                    CssClass="txtParam txtmeninggalProbDwsKomorbid" controlName="Meninggal Prob Dws Komorbid" />
            </td>
           
        </tr>
        <tr>
            <td>
                Meninggal Prob Lansia Komorbid
            </td>
            <td>
            </td>
            <td>
                <asp:TextBox ID="txtMeninggalProbLansiaKomorbid" Width="60px" runat="server"
                    CssClass="txtParam txtMeninggalProbLansiaKomorbid" controlName=" Meninggal Prob Lansia Komorbid" />
            </td>
           
        </tr>
        <tr>
            <td>
                Meninggal Prob Pre Tanpa Komorbid
            </td>
            <td>
            </td>
            <td>
                <asp:TextBox ID="txtMeninggalProbPreTanpaKomorbid" Width="60px" runat="server" CssClass="txtParam txtMeninggalProbPreTanpaKomorbid"
                    controlName="Meninggal Prob Pre Tanpa Komorbid" />
            </td>
            
        </tr>
        <tr>
            <td>
                Meninggal Prob Neo Tanpa Komorbid
            </td>
            <td>
            </td>
            <td>
                <asp:TextBox ID="txtMeninggalProbNeoTanpaKomorbid" Width="60px" runat="server" CssClass="txtParam txtMeninggalProbNeoTanpaKomorbid"
                    controlName="Meninggal Prob Neo Tanpa Komorbid" />
            </td>
             
        </tr>
        <tr>
            <td>
               Meninggal Prob Bayi Tanpa Komorbid
            </td>
            <td>
            </td>
            <td>
                <asp:TextBox ID="txtMeninggalProbBayiTanpaKomorbid" Width="60px" runat="server" CssClass="txtParam txtMeninggalProbBayiTanpaKomorbid"
                    controlName="Meninggal Prob Bayi Tanpa Komorbid" />
            </td>
             
        </tr>
        <tr>
            <td>
                Meninggal Prob Balita Tanpa Komorbid
            </td>
            <td>
            </td>
            <td>
                <asp:TextBox ID="txtMeninggalProbBalitaTanpaKomorbid" Width="60px" runat="server" CssClass="txtParam txtMeninggalProbBalitaTanpaKomorbid"
                    controlName="Meninggal Prob Balita Tanpa Komorbid" />
            </td>
             
        </tr>
        <tr>
            <td>
               Meninggal Prob Anak Tanpa Komorbid
            </td>
            <td>
            </td>
            <td>
                <asp:TextBox ID="txtMeninggalProbAnakTanpaKomorbid" Width="60px" runat="server" CssClass="txtParam txtMeninggalProbAnakTanpaKomorbid"
                    controlName="Meninggal Prob Anak Tanpa Komorbid" />
            </td>
            
        </tr>
         <tr>
            <td>
              Meninggal Prob Dws Tanpa Komorbid
            </td>
            <td>
            </td>
            <td>
                <asp:TextBox ID="txtMeninggalProbDwsTanpaKomorbid" Width="60px" runat="server" CssClass="txtParam txtMeninggalProbDwsTanpaKomorbid"
                    controlName="Meninggal Prob Dws Tanpa Komorbid" />
            </td>
             
        </tr>
         <tr>
            <td>
              Meninggal Prob Lansia Tanpa Komorbid
            </td>
            <td>
            </td>
            <td>
                <asp:TextBox ID="txtMeninggalProbLansiaTanpaKomorbid" Width="60px" runat="server" CssClass="txtParam txtMeninggalProbLansiaTanpaKomorbid"
                    controlName="Meninggal Prob Lansia Tanpa Komorbid " />
            </td>
            
        </tr>
        <tr>
            <td>
                Meninggal Discarded Komorbid
            </td>
            <td>
            </td>
            <td>
                <asp:TextBox ID="txtMeninggalDiscardedKomorbid" Width="60px" runat="server" CssClass="txtParam txtMeninggalDiscardedKomorbid"
                    controlName="Meninggal Discarded Komorbid " />
            </td>
           
        </tr>
        <tr>
            <td>
                Meninggal Discarded Tanpa Komorbid
            </td>
            <td>
            </td>
            <td>
                <asp:TextBox ID="txtMeninggalDiscardedTanpaKomorbid" Width="60px" runat="server" CssClass="txtParam txtMeninggalDiscardedTanpaKomorbid"
                    controlName="  Meninggal Discarded Tanpa Komorbid" />
            </td>
          
        </tr>
         <tr>
            <td>
               Dirujuk
            </td>
            <td>
            </td>
            <td>
                <asp:TextBox ID="txtDirujuk" Width="60px" runat="server" CssClass="txtParam txtDirujuk"
                    controlName="Dirujuk" />
            </td>
           
        </tr>
         <tr>
            <td>
                Isman
            </td>
            <td>
            </td>
            <td>
                <asp:TextBox ID="txtIsman" Width="60px" runat="server" CssClass="txtParam txtIsman"
                    controlName="Isman " />
            </td>
          
        </tr>
         <tr>
            <td>
               APS
            </td>
            <td>
            </td>
            <td>
                <asp:TextBox ID="txtAps" Width="60px" runat="server" CssClass="txtParam txtAps"
                    controlName="APS" />
            </td>
          
        </tr>
    </table>
   </div>
</asp:Content>
