<%@ Page Language="C#" MasterPageFile="~/libs/MasterPage/MPTrx.master" AutoEventWireup="true" 
    CodeBehind="DinkesBedInformation1.aspx.cs" Inherits="QIS.Medinfras.Web.MedicalRecord.Program.DinkesBedInformation1" %>

<asp:Content ID="Content3" ContentPlaceHolderID="plhCustomButtonToolbar" runat="server">
    <li id="btnGetData" CRUDMode="R" runat="server" visible="false"><img src='<%=ResolveUrl("~/Libs/Images/Icon/tbprocess.png")%>' alt="" /><br style="clear:both"/><div><%=GetLabel("Pull Data")%></div></li>
    <li id="btnSendToAPI" CRUDMode="R" runat="server"><img src='<%=ResolveUrl("~/Libs/Images/Icon/tbsendmail.png")%>' alt="" /><br style="clear:both"/><div><%=GetLabel("Kirim Data")%></div></li>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div class="menuTitle"><%=GetMenuCaption()%></div>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <style type="text/css">
        .txtKosong { text-align:right; color: Blue;}
        .txtKapasitas { text-align:right; color: Black;}
    </style>
    <script type="text/javascript">

        $('#<%=btnGetData.ClientID %>').click(function () {
              var message = "Fitur ini belum tersedia <br/> Fitur penarikan data dari sistem berdasarkan mapping tempat tidur RS dengan Pengelompokan Tempat Tidur Dinas Kesehatan";
              displayMessageBox("Informasi Tempat Tidur (DINKES)", message);
//            if (errMessage == "") {
//                onCustomButtonClick('send');
//            }
//            else {
//                displayErrorMessageBox("Informasi Tempat Tidur", errMessage);
//            }
        });

        $('#<%=btnSendToAPI.ClientID %>').click(function () {
            var errMessage = ValidateParameter();
            if (errMessage == "") {
                onCustomButtonClick('send');
            }
            else {
                displayErrorMessageBox("Informasi Tempat Tidur", errMessage);
            }
        });

        function ValidateParameter() {
            var message = '';

            $(".txtParam").each(function () {
                var value = $(this).val();
                var controlName = $(this).attr('controlName');
                if (value == "" || isNaN(value))
                    message += ("Parameter " + "<span style='color:red; font-weight:bold; font-style:italic;'>" + controlName + "</span> harus terisi atau minimal 0 <br/>");                
            }); 

            return message;
        }

        function onAfterCustomClickSuccess(type) {
            hideLoadingPanel();
            var message = "Proses pengiriman data informasi tempat tidur kepada Dinas Kesehatan.";
            displayMessageBox("MEDINFRAS", 'PROSES BERHASIL : ' + message);
        }
    </script>

    <table class="tblInfoTT w3-table w3-bordered w3-card-4">
        <colgroup>
            <col />
            <col style="width:80px" />
            <col style="width:80px" />
            <col style="width:80px" />
            <col style="width:80px" />
            <col style="width:80px" />
            <col style="width:80px" />
        </colgroup>
        <tr>
          <th>Jenis Tempat Tidur</th>
          <th colspan="3" class="w3-center">Kapasitas</th>
          <th colspan="3" class="w3-center">Kosong</th>
        </tr>
        <tr>
            <th>Tanggal/Jam Pengiriman Terakhir : <label id="lblLastUpdatedDateTime" runat="server" style="color:Red">dd-mm-yyyy hh:mm:ss</label> </th>
            <th class="w3-center">Total</th>
            <th class="w3-center">Laki-laki</th>
            <th class="w3-center">Perempuan</th>
            <th class="w3-center">Total</th>
            <th class="w3-center">Laki-laki</th>
            <th class="w3-center">Perempuan</th>
        </tr>
        <tr>
            <td>VIP</td>
            <td><asp:TextBox ID="txtKapasitasVIP" Width="60px" runat="server" CssClass="txtParam txtKapasitas" controlName = "Kapasitas VIP" /></td>
            <td></td>
            <td></td>
            <td><asp:TextBox ID="txtKosongVIP" Width="60px" runat="server" CssClass="txtParam txtKosong" controlName = "Kosong VIP"/></td>
            <td></td>
            <td></td>
        </tr>
        <tr>
            <td>Kelas 1</td>
            <td><asp:TextBox ID="txtKapasitasKelas1" Width="60px" runat="server" CssClass="txtParam txtKapasitas" controlName = "Kapasitas Kelas 1" /></td>
            <td><asp:TextBox ID="txtKapasitasKelas1L" Width="60px" runat="server" CssClass="txtParam txtKapasitas" controlName = "Kapasitas Kelas 1 - L"/></td>
            <td><asp:TextBox ID="txtKapasitasKelas1P" Width="60px" runat="server" CssClass="txtParam txtKapasitas" controlName = "Kapasitas Kelas 1 - P"/></td>
            <td><asp:TextBox ID="txtKosongKelas1" Width="60px" runat="server" CssClass="txtParam txtKosong" controlName = "Kosong Kelas 1"/></td>
            <td><asp:TextBox ID="txtKosongKelas1L" Width="60px" runat="server" CssClass="txtParam txtKosong" controlName = "Kosong Kelas 1 - L"/></td>
            <td><asp:TextBox ID="txtKosongKelas1P" Width="60px" runat="server" CssClass="txtParam txtKosong" controlName = "Kosong Kelas 1 - P"/></td>
        </tr>
        <tr>
            <td>Kelas 2</td>
            <td><asp:TextBox ID="txtKapasitasKelas2" Width="60px" runat="server" CssClass="txtParam txtKapasitas" controlName = "Kapasitas Kelas 2"/></td>
            <td><asp:TextBox ID="txtKapasitasKelas2L" Width="60px" runat="server" CssClass="txtParam txtKapasitas" controlName = "Kapasitas Kelas 2 - L"/></td>
            <td><asp:TextBox ID="txtKapasitasKelas2P" Width="60px" runat="server" CssClass="txtParam txtKapasitas" controlName = "Kapasitas Kelas 2 - P"/></td>
            <td><asp:TextBox ID="txtKosongKelas2" Width="60px" runat="server" CssClass="txtParam txtKosong" controlName = "Kosong Kelas 2"/></td>
            <td><asp:TextBox ID="txtKosongKelas2L" Width="60px" runat="server" CssClass="txtParam txtKosong" controlName = "Kapasitas Kelas 2 - L"/></td>
            <td><asp:TextBox ID="txtKosongKelas2P" Width="60px" runat="server" CssClass="txtParam txtKosong" controlName = "Kapasitas Kelas 2 - P"/></td>
        </tr>
        <tr>
            <td>Kelas 3</td>
            <td><asp:TextBox ID="txtKapasitasKelas3" Width="60px" runat="server" CssClass="txtParam txtKapasitas" controlName = "Kapasitas Kelas 3"/></td>
            <td><asp:TextBox ID="txtKapasitasKelas3L" Width="60px" runat="server" CssClass="txtParam txtKapasitas" controlName = "Kapasitas Kelas 3 - L"/></td>
            <td><asp:TextBox ID="txtKapasitasKelas3P" Width="60px" runat="server" CssClass="txtParam txtKapasitas" controlName = "Kapasitas Kelas 3 - P"/></td>
            <td><asp:TextBox ID="txtKosongKelas3" Width="60px" runat="server" CssClass="txtParam txtKosong" controlName = "Kosong Kelas 3"/></td>
            <td><asp:TextBox ID="txtKosongKelas3L" Width="60px" runat="server" CssClass="txtParam txtKosong" controlName = "Kosong Kelas 3 - L"/></td>
            <td><asp:TextBox ID="txtKosongKelas3P" Width="60px" runat="server" CssClass="txtParam txtKosong" controlName = "Kapasitas Kelas 3 - P"/></td>
        </tr>
        <tr>
            <td>HCU</td>
            <td><asp:TextBox ID="txtKapasitasHCU" Width="60px" runat="server" CssClass="txtParam txtKapasitas" controlName = "Kapasitas HCU"/></td>
            <td></td>
            <td></td>
            <td><asp:TextBox ID="txtKosongHCU" Width="60px" runat="server" CssClass="txtParam txtKosong" controlName = "Kosong HCU"/></td>
            <td></td>
            <td></td>
        </tr>
        <tr>
            <td>ICCU</td>
            <td><asp:TextBox ID="txtKapasitasICCU" Width="60px" runat="server" CssClass="txtParam txtKapasitas" controlName = "Kapasitas ICCU"/></td>
            <td></td>
            <td></td>
            <td><asp:TextBox ID="txtKosongICCU" Width="60px" runat="server" CssClass="txtParam txtKosong" controlName = "Kosong HCU"/></td>
            <td></td>
            <td></td>
        </tr>
        <tr>
            <td>ICU : Negatif - Ventilator</td>
            <td><asp:TextBox ID="txtKapasitasICUNegatifVentilator" Width="60px" runat="server" CssClass="txtParam txtKapasitas" controlName = "Kapasitas ICU : Negatif Ventilator"/></td>
            <td></td>
            <td></td>
            <td><asp:TextBox ID="txtKosongICUNegatifVentilator" Width="60px" runat="server" CssClass="txtParam txtKosong" controlName = "Kosong ICU : Negatif Ventilator"/></td>
            <td></td>
            <td></td>
        </tr>
        <tr>
            <td>ICU : Negatif - Tanpa Ventilator</td>
            <td><asp:TextBox ID="txtKapasitasICUNegatifTanpaVentilator" Width="60px" runat="server" CssClass="txtParam txtKapasitas" controlName = "Kapasitas ICU : Negatif Tanpa Ventilator"/></td>
            <td></td>
            <td></td>
            <td><asp:TextBox ID="txtKosongICUNegatifTanpaVentilator" Width="60px" runat="server" CssClass="txtParam txtKosong" controlName = "Kosong ICU : Negatif Tanpa Ventilator"/></td>
            <td></td>
            <td></td>
        </tr>
        <tr>
            <td>ICU : Tanpa Negatif - Ventilator</td>
            <td><asp:TextBox ID="txtKapasitasICUTanpaNegatifVentilator" Width="60px" runat="server" CssClass="txtParam txtKapasitas" controlName = "Kapasitas ICU : Tanpa Negatif Ventilator"/></td>
            <td></td>
            <td></td>
            <td><asp:TextBox ID="txtKosongICUTanpaNegatifVentilator" Width="60px" runat="server" CssClass="txtParam txtKosong" controlName = "Kosong ICU : Tanpa Negatif Ventilator"/></td>
            <td></td>
            <td></td>
        </tr>
        <tr>
            <td>ICU : Tanpa Negatif - Tanpa Ventilator</td>
            <td><asp:TextBox ID="txtKapasitasICUTanpaNegatifTanpaVentilator" Width="60px" runat="server" CssClass="txtParam txtKapasitas" controlName = "Kapasitas ICU : Tanpa Negatif Tanpa Ventilator"/></td>
            <td></td>
            <td></td>
            <td><asp:TextBox ID="txtKosongICUTanpaNegatifTanpaVentilator" Width="60px" runat="server" CssClass="txtParam txtKosong" controlName = "Kosong ICU : Tanpa Negatif Tanpa Ventilator"/></td>
            <td></td>
            <td></td>
        </tr>
        <tr>
            <td>ICU COVID : Negatif - Ventilator</td>
            <td><asp:TextBox ID="txtKapasitasICUCovidNegatifVentilator" Width="60px" runat="server" CssClass="txtParam txtKapasitas" controlName = "Kapasitas ICU COVID : Negatif Ventilator"/></td>
            <td></td>
            <td></td>
            <td><asp:TextBox ID="txtKosongICUCovidNegatifVentilator" Width="60px" runat="server" CssClass="txtParam txtKosong" controlName = "Kosong ICU COVID : Negatif Ventilator"/></td>
            <td></td>
            <td></td>
        </tr>
        <tr>
            <td>ICU COVID : Negatif - Tanpa Ventilator</td>
            <td><asp:TextBox ID="txtKapasitasICUCovidNegatifTanpaVentilator" Width="60px" runat="server" CssClass="txtParam txtKapasitas" controlName = "Kapasitas ICU COVID : Tanpa Negatif Ventilator"/></td>
            <td></td>
            <td></td>
            <td><asp:TextBox ID="txtKosongICUCovidNegatifTanpaVentilator" Width="60px" runat="server" CssClass="txtParam txtKosong" controlName = "Kosong ICU COVID : Tanpa Negatif Ventilator"/></td>
            <td></td>
            <td></td>
        </tr>
        <tr>
            <td>ICU COVID : Tanpa Negatif - Ventilator</td>
            <td><asp:TextBox ID="txtKapasitasICUCovidTanpaNegatifVentilator" Width="60px" runat="server" CssClass="txtParam txtKapasitas" controlName = "Kapasitas ICU COVID : Tanpa Negatif Ventilator"/></td>
            <td></td>
            <td></td>
            <td><asp:TextBox ID="txtKosongICUCovidTanpaNegatifVentilator" Width="60px" runat="server" CssClass="txtParam txtKosong" controlName = "Kapasitas ICU COVID : Tanpa Negatif Ventilator"/></td>
            <td></td>
            <td></td>
        </tr>
        <tr>
            <td>ICU COVID : Tanpa Negatif - Tanpa Ventilator</td>
            <td><asp:TextBox ID="txtKapasitasICUCovidTanpaNegatifTanpaVentilator" Width="60px" runat="server" CssClass="txtParam txtKapasitas" controlName = "Kapasitas ICU COVID : Tanpa Negatif Tanpa Ventilator"/></td>
            <td></td>
            <td></td>
            <td><asp:TextBox ID="txtKosongICUCovidTanpaNegatifTanpaVentilator" Width="60px" runat="server" CssClass="txtParam txtKosong" controlName = "Kapasitas ICU COVID : Tanpa Negatif Tanpa Ventilator"/></td>
            <td></td>
            <td></td>
        </tr>
        <tr>
            <td>Isolasi : Negatif</td>
            <td><asp:TextBox ID="txtKapasitasIsolasiNegatif" Width="60px" runat="server" CssClass="txtParam txtKapasitas" controlName = "Kapasitas Isolasi : Negatif"/></td>
            <td></td>
            <td></td>
            <td><asp:TextBox ID="txtKosongIsolasiNegatif" Width="60px" runat="server" CssClass="txtParam txtKosong" controlName = "Kosong Isolasi : Negatif"/></td>
            <td></td>
            <td></td>
        </tr>
        <tr>
            <td>Isolasi : Tanpa Negatif</td>
            <td><asp:TextBox ID="txtKapasitasIsolasiTanpaNegatif" Width="60px" runat="server" CssClass="txtParam txtKapasitas" controlName = "Kapasitas Isolasi : Tanpa Negatif"/></td>
            <td></td>
            <td></td>
            <td><asp:TextBox ID="txtKosongIsolasiTanpaNegatif" Width="60px" runat="server" CssClass="txtParam txtKosong" controlName = "Kosong Isolasi : Tanpa Negatif"/></td>
            <td></td>
            <td></td>
        </tr>
        <tr>
            <td>NICU COVID</td>
            <td><asp:TextBox ID="txtKapasitasNICUCovid" Width="60px" runat="server" CssClass="txtParam txtKapasitas" controlName = "Kapasitas NICU Covid"/></td>
            <td></td>
            <td></td>
            <td><asp:TextBox ID="txtKosongNICUCovid" Width="60px" runat="server" CssClass="txtParam txtKosong" controlName = "Kosong NICU Covid"/></td>
            <td></td>
            <td></td>
        </tr>
        <tr>
            <td>Perina COVID</td>
            <td><asp:TextBox ID="txtKapasitasPerinaCovid" Width="60px" runat="server" CssClass="txtParam txtKapasitas" controlName = "Kapasitas Perina Covid"/></td>
            <td></td>
            <td></td>
            <td><asp:TextBox ID="txtKosongPerinaCovid" Width="60px" runat="server" CssClass="txtParam txtKosong" controlName = "Kosong Perina Covid"/></td>
            <td></td>
            <td></td>
        </tr>
        <tr>
            <td>PICU COVID</td>
            <td><asp:TextBox ID="txtKapasitasPICUCovid" Width="60px" runat="server" CssClass="txtParam txtKapasitas" controlName = "Kapasitas PICU Covid"/></td>
            <td></td>
            <td></td>
            <td><asp:TextBox ID="txtKosongPICUCovid" Width="60px" runat="server" CssClass="txtParam txtKosong" controlName = "Kosong PICU Covid"/></td>
            <td></td>
            <td></td>
        </tr>
        <tr>
            <td>OK COVID</td>
            <td><asp:TextBox ID="txtKapasitasOKCovid" Width="60px" runat="server" CssClass="txtParam txtKapasitas" controlName = "Kapasitas OK Covid"/></td>
            <td></td>
            <td></td>
            <td><asp:TextBox ID="txtKosongOKCovid" Width="60px" runat="server" CssClass="txtParam txtKosong" controlName = "Kosong OK Covid"/></td>
            <td></td>
            <td></td>
        </tr>
        <tr>
            <td>HD COVID</td>
            <td><asp:TextBox ID="txtKapasitasHDCovid" Width="60px" runat="server" CssClass="txtParam txtKapasitas" controlName = "Kapasitas HD Covid"/></td>
            <td></td>
            <td></td>
            <td><asp:TextBox ID="txtKosongHDCovid" Width="60px" runat="server" CssClass="txtParam txtKosong" controlName = "Kosong HD Covid"/></td>
            <td></td>
            <td></td>
        </tr>
    </table>
</asp:Content>
