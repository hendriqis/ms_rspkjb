<%@ Page Language="C#" MasterPageFile="~/libs/MasterPage/MPTrx.master" AutoEventWireup="true"
    CodeBehind="RekapPasienMasuk.aspx.cs" Inherits="QIS.Medinfras.Web.MedicalRecord.Program.RekapPasienMasuk" %>

<asp:Content ID="Content3" ContentPlaceHolderID="plhCustomButtonToolbar" runat="server">
    <li id="btnRefresh" crudmode="R" runat="server">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbrefresh.png")%>' alt="" /><br style="clear: both" />
        <div>
            <%=GetLabel("Refresh")%></div>
    </li>
    <li id="btnImport" runat="server" crudmode="R">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbprocess.png") %>' alt="" />
        <div>
            <%=GetLabel("GET Data SiRanap") %></div>
    </li>
    <li id="btnSend" crudmode="R" runat="server">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbsendmail.png")%>' alt="" /><br style="clear: both" />
        <div>
            <%=GetLabel("POST")%></div>
    </li>
    <li id="btnDelete" crudmode="R" runat="server">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbdelete.png")%>' alt="" /><br style="clear: both" />
        <div>
            <%=GetLabel("DELETE")%></div>
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
            ResetInfo();
        });

        //#endregion

        //#region btnImport
        $('#<%=btnImport.ClientID %>').live('click', function (evt) {
            var date = Methods.getDatePickerDate($('#<%=txtObservasiDate.ClientID %>').val());
            var tanggal = Methods.dateToYMD(date);

            KemenKesService.getRekapPasienMasuk(function (result) {
                if (result != null) {
                    //showToast('INFORMATION', result);
                    var resp = result.split('|');
                    if (resp[0] == "1") {
                        var jData = JSON.parse(resp[1]);
                        var respData = jData["RekapPasienMasuk"];
                        for (var i = 0; i < respData.length; i++) {
                            if (respData[i]["tanggal"] == tanggal) {
                                //parsing data to form 
                                $('#<%=txtIGDSuspectL.ClientID %>').val(respData[i]["igd_suspect_l"]);
                                $('#<%=txtIGDSuspectP.ClientID %>').val(respData[i]["igd_suspect_p"]);
                                $('#<%=txtIGDConfirmL.ClientID %>').val(respData[i]["igd_confirm_l"]);
                                $('#<%=txtIGDConfirmP.ClientID %>').val(respData[i]["igd_confirm_p"]);
                                $('#<%=txtRJSuspectL.ClientID %>').val(respData[i]["rj_suspect_l"]);
                                $('#<%=txtRJSuspectP.ClientID %>').val(respData[i]["rj_suspect_p"]);
                                $('#<%=txtRJConfirmL.ClientID %>').val(respData[i]["rj_confirm_l"]);
                                $('#<%=txtRJConfirmP.ClientID %>').val(respData[i]["rj_confirm_p"]);
                                $('#<%=txtRISuspectL.ClientID %>').val(respData[i]["ri_suspect_l"]);
                                $('#<%=txtRISuspectP.ClientID %>').val(respData[i]["ri_suspect_p"]);
                                $('#<%=txtRIConfirmL.ClientID %>').val(respData[i]["ri_confirm_l"]);
                                $('#<%=txtRIConfirmP.ClientID %>').val(respData[i]["ri_confirm_p"]);

                                //Show Informasi
                                $("#DataID").text(respData[i]["id"])
                                $("#DataKoders").text(respData[i]["koders"]);
                                $("#DataTanggal").text(respData[i]["tanggal"]);
                                $("#DataTanggalLapor").text(respData[i]["tgl_lapor"]);
                                $("#infoKemkes").show();
                                break;
                            } else {
                                FormReset();
                                ResetInfo();
                            }
                        }

                    } else {
                        showToast('INFORMATION', resp[1]);
                        ResetInfo();
                        FormReset(); 

                    }
                }
            });
        });
        function FormReset() {
            $('#<%=txtIGDSuspectL.ClientID %>').val(0);
            $('#<%=txtIGDSuspectP.ClientID %>').val(0);
            $('#<%=txtIGDConfirmL.ClientID %>').val(0);
            $('#<%=txtIGDConfirmP.ClientID %>').val(0);
            $('#<%=txtRJSuspectL.ClientID %>').val(0);
            $('#<%=txtRJSuspectP.ClientID %>').val(0);
            $('#<%=txtRJConfirmL.ClientID %>').val(0);
            $('#<%=txtRJConfirmP.ClientID %>').val(0);
            $('#<%=txtRISuspectL.ClientID %>').val(0);
            $('#<%=txtRISuspectP.ClientID %>').val(0);
            $('#<%=txtRIConfirmL.ClientID %>').val(0);
            $('#<%=txtRIConfirmP.ClientID %>').val(0);

        }
        function ResetInfo() {
            //Show Info
            $("#DataID").text('')
            $("#DataKoders").text('');
            $("#DataTanggal").text('');
            $("#DataTanggalLapor").text('');
            $("#infoKemkes").hide();
        }
        //#endregion

        //#region btnSend
        $('#<%=btnSend.ClientID%>').live('click', function (evt) {
            var date = Methods.getDatePickerDate($('#<%=txtObservasiDate.ClientID %>').val());
            var tanggal = Methods.dateToYMD(date);
            var igd_suspect_l = $('#<%=txtIGDSuspectL.ClientID %>').val();
            var igd_suspect_p = $('#<%=txtIGDSuspectP.ClientID %>').val();
            var igd_confirm_l = $('#<%=txtIGDConfirmL.ClientID %>').val();
            var igd_confirm_p = $('#<%=txtIGDConfirmP.ClientID %>').val();
            var rj_suspect_l = $('#<%=txtRJSuspectL.ClientID %>').val();
            var rj_suspect_p = $('#<%=txtRJSuspectP.ClientID %>').val();
            var rj_confirm_l = $('#<%=txtRJConfirmL.ClientID %>').val();
            var rj_confirm_p = $('#<%=txtRJConfirmP.ClientID %>').val();
            var ri_suspect_l = $('#<%=txtRISuspectL.ClientID %>').val();
            var ri_suspect_p = $('#<%=txtRISuspectP.ClientID %>').val();
            var ri_confirm_l = $('#<%=txtRIConfirmL.ClientID %>').val();
            var ri_confirm_p = $('#<%=txtRIConfirmP.ClientID %>').val();

            KemenKesService.rekapPasienMasuk(tanggal, igd_suspect_l, igd_suspect_p, igd_confirm_l, igd_confirm_p, rj_suspect_l,
            rj_suspect_p, rj_confirm_l, rj_confirm_p, ri_suspect_l, ri_suspect_p, ri_confirm_l, ri_confirm_p, function (result) {
                if (result != null) {
                    var resp = result.split('|');
                    var jData = JSON.parse(resp[1]);
                    if (resp[0] == "1") {
                        var respData = jData["RekapPasienMasuk"];
                        showToast(respData[0]["message"]);
                    } else {
                        showToast(resp[1]);
                    }

                   //////showToast('INFORMATION', result);
                }
            });
        });
        //#endregion

        //#region btnDelete
        $('#<%=btnDelete.ClientID%>').live('click', function (evt) {
            var date = Methods.getDatePickerDate($('#<%=txtObservasiDate.ClientID %>').val());
            var tanggal = Methods.dateToYMD(date);
            KemenKesService.deleteRekapPasienMasuk(tanggal, function (result) {
                if (result != "") {
                    var resp = result.split('|');
                    var jData = JSON.parse(resp[1]);
                    if (resp[0] == "1") {
                        var respData = jData["RekapPasienMasuk"];
                        showToast(respData[0]["message"]);
                        ResetInfo();
                        FormReset();
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
        <h2>Form  Rekap Pasien Masuk</h2> 
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
                IGD Suspect
            </td>
            <td>
            </td>
            <td>
                <asp:TextBox ID="txtIGDSuspectL" Width="60px" runat="server" CssClass="txtParam txtIGDSuspectL"
                    controlName="IGD Suspect L" />
            </td>
            <td>
                <asp:TextBox ID="txtIGDSuspectP" Width="60px" runat="server" CssClass="txtParam txtIGDSuspectP"
                    controlName="IGD Suspect P" />
            </td>
        </tr>
        <tr>
            <td>
                IGD Confirm
            </td>
            <td>
            </td>
            <td>
                <asp:TextBox ID="txtIGDConfirmL" Width="60px" runat="server" CssClass="txtParam txtIGDConfirmL"
                    controlName="IGD Confirm L" />
            </td>
            <td>
                <asp:TextBox ID="txtIGDConfirmP" Width="60px" runat="server" CssClass="txtParam txtIGDConfirmP"
                    controlName="IGD Confirm P" />
            </td>
        </tr>
        <tr>
            <td>
                Rawat Jalan Suspect
            </td>
            <td>
            </td>
            <td>
                <asp:TextBox ID="txtRJSuspectL" Width="60px" runat="server" CssClass="txtParam txtRJSuspectL"
                    controlName="RJ Suspect L" />
            </td>
            <td>
                <asp:TextBox ID="txtRJSuspectP" Width="60px" runat="server" CssClass="txtParam txtRJSuspectP"
                    controlName="RJ Suspect P" />
            </td>
        </tr>
        <tr>
            <td>
                Rawat Jalan Confirm
            </td>
            <td>
            </td>
            <td>
                <asp:TextBox ID="txtRJConfirmL" Width="60px" runat="server" CssClass="txtParam txtRJConfirmL"
                    controlName="RJ Confirm L" />
            </td>
            <td>
                <asp:TextBox ID="txtRJConfirmP" Width="60px" runat="server" CssClass="txtParam txtRJConfirmP"
                    controlName="RJ Confirm P" />
            </td>
        </tr>
        <tr>
            <td>
                Rawat Inap Suspect
            </td>
            <td>
            </td>
            <td>
                <asp:TextBox ID="txtRISuspectL" Width="60px" runat="server" CssClass="txtParam txtRISuspectL"
                    controlName="RI Suspect L" />
            </td>
            <td>
                <asp:TextBox ID="txtRISuspectP" Width="60px" runat="server" CssClass="txtParam txtRISuspectP"
                    controlName="RI Suspect P" />
            </td>
        </tr>
        <tr>
            <td>
                Rawat Inap Confirm
            </td>
            <td>
            </td>
            <td>
                <asp:TextBox ID="txtRIConfirmL" Width="60px" runat="server" CssClass="txtParam txtRIConfirmL"
                    controlName="RI Confirm L" />
            </td>
            <td>
                <asp:TextBox ID="txtRIConfirmP" Width="60px" runat="server" CssClass="txtParam txtRIConfirmP"
                    controlName="RI Confirm P" />
            </td>
        </tr>
    </table>
</div>
</asp:Content>
