<%@ Page Title="" Language="C#" MasterPageFile="~/Libs/MasterPage/MPEntry.Master" AutoEventWireup="true" CodeBehind="Registration.aspx.cs" Inherits="QIS.Medinfras.Web.Module.Program.Outpatient.Registration" %>

<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript">
        $(function () {
            //#region No Registrasi
            $('#lblNoReg.lblLink').live('click', function () {
                openSearchDialog('ri_reg', function (value) {
                    $('#<%=txtNoRegistrasi.ClientID %>').val(value);
                    onTxtNoRegChanged(value);
                });
            });
            $('#<%=txtNoRegistrasi.ClientID %>').live('change', function () {
                onTxtNoRegChanged($(this).val());
            });
            function onTxtNoRegChanged(value) {
                setIsAdd(false);
                $('#lblNoRM').attr('class', 'lblDisabled');
                $('#<%=txtNoRM.ClientID %>').attr('readonly', 'readonly');

                var filterExpression = "noreg = '" + value + "'";
                Methods.getObject('Getvri_regList', filterExpression, function (result) {
                    if (result != null) {
                        $('#<%=txtNoRM.ClientID %>').val(result.norm);
                        $('#<%=txtNama.ClientID %>').val(result.nama);
                        $('#<%=txtMarga.ClientID %>').val(result.marga);
                        $('#<%=txtKdSeks.ClientID %>').val(result.kdseks);
                    }
                    else {
                        $('#<%=txtNoRM.ClientID %>').val('');
                        $('#<%=txtNama.ClientID %>').val('');
                        $('#<%=txtMarga.ClientID %>').val('');
                        $('#<%=txtKdSeks.ClientID %>').val('');
                    }
                });
            }
            //#endregion

            //#region No RM
            $('#lblNoRM.lblLink').live('click', function () {
                openSearchDialog('pasien', function (value) {
                    $('#<%=txtNoRM.ClientID %>').val(value);
                    onTxtNoRMChanged(value);
                });
            });
            $('#<%=txtNoRM.ClientID %>').live('change', function () {
                onTxtNoRMChanged($(this).val());
            });
            function onTxtNoRMChanged(value) {
                var filterExpression = "norm = '" + value + "'";
                Methods.getObject('GetPasienList', filterExpression, function (result) {
                    if (result != null) {
                        $('#<%=txtNama.ClientID %>').val(result.nama);
                        $('#<%=txtMarga.ClientID %>').val(result.marga);
                        $('#<%=txtKdSeks.ClientID %>').val(result.kdseks);
                    }
                    else {
                        $('#<%=txtNama.ClientID %>').val('');
                        $('#<%=txtMarga.ClientID %>').val('');
                        $('#<%=txtKdSeks.ClientID %>').val('');
                    }
                });
            }
            //#endregion
        });
    </script>
    <div class="pageTitle">Pendaftaran Rawat Jalan</div>
    <table class="tblContentArea">
        <colgroup>
            <col style="width:50%"/>
            <col style="width:50%"/>
        </colgroup>
        <tr>
            <td style="padding:5px;">
                <table class="tblEntryContent" style="width:100%">
                    <colgroup>
                        <col style="width:100px"/>
                        <col style="width:180px"/>
                        <col style="width:30px"/>
                    </colgroup>
                    <tr>
                        <td class="tdLabel"><span class="lblNormal">Tanggal / Jam Registrasi</span></td>
                        <td><asp:TextBox ID="txtTanggalRegistrasi" Width="100%" runat="server" Style="text-align:center" /></td>
                        <td style="padding-left:30px;padding-right:10px">Jam</td>
                        <td><asp:TextBox ID="txtJamRegistrasi" runat="server" Width="60px" Style="text-align:center" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><span class="lblLink">Nomor Appointment</span></td>
                        <td><asp:TextBox ID="txtNoAppointment" Width="100%" runat="server" /></td>
                        <td style="padding-left:30px;">Jam</td>
                        <td><asp:TextBox ID="txtJamAppointment" runat="server" Width="60px" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><span class="lblLink" id="lblNoReg">Nomor Registrasi</span></td>
                        <td><asp:TextBox ID="txtNoRegistrasi" Width="100%" runat="server" /></td>
                    </tr>
                </table>
                <h4>Data Pasien</h4>
                <table class="tblEntryContent">
                    <tr>
                        <td class="tdLabel"><span class="lblLink lblMandatory" id="lblNoRM">Nomor Rekam Medis</span></td>
                        <td><asp:TextBox ID="txtNoRM" CssClass="NoRM" Width="100%" runat="server" /></td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><span class="lblNormal">Nama Pasien</span></td>
                        <td>
                            <table style="width:100%" cellpadding="0" cellspacing="0" >
                                <tr>
                                    <td style="width:10%"><asp:TextBox ID="txtGelar" Width="35px" runat="server" Style="padding-right:3px;" /></td>
                                    <td style="width:55%"><asp:TextBox ID="txtNama" Width="95%" runat="server" Style="padding-right:3px;" /></td>
                                    <td style="width:35%"><asp:TextBox ID="txtMarga" Width="100%" runat="server" /></td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><div class="lblNormal">Jenis Kelamin</div></td>
                        <td>
                            <table style="width:100%" cellpadding="0" cellspacing="0" >
                                <colgroup>
                                    <col style="width:40px"/>
                                </colgroup>
                                <tr>
                                    <td><asp:TextBox ID="txtKdSeks" Width="40px" runat="server" Style="margin-right:3px;" /></td>
                                    <td><span style="font-size:16px;color:#000;">L</span>aki-laki / <span style="font-size:16px;color:#000;">P</span>erempuan</td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel"><div class="lblNormal">Tanggal Lahir / Umur</div></td>
                        <td>
                            <table style="width:100%" cellpadding="0" cellspacing="0" >
                                <tr>
                                    <td><asp:TextBox ID="txtTanggalLahir" Width="180px" runat="server" Style="margin-right:3px;" /></td>
                                    <td><asp:TextBox ID="txtUmurTahun" Width="30px" runat="server" Style="margin-right:1px;" /></td>
                                    <td>Tahun</td>
                                    <td><asp:TextBox ID="txtUmurBulan" Width="30px" runat="server" Style="margin-right:1px;" /></td>
                                    <td>Bulan</td>
                                    <td><asp:TextBox ID="txtUmurHari" Width="30px" runat="server" Style="margin-right:1px;" /></td>
                                    <td>Hari</td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </td>
            <td>
            </td>
        </tr>
    </table>   
</asp:Content>
