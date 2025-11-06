<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SearchBPJSReferralCtl2.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.SearchBPJSReferralCtl2" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxFileManager" TagPrefix="dx" %>
<style type="text/css">
    .highlight
    {
        background-color: #FE5D15;
        color: White;
    }
</style>
<script type="text/javascript" id="dxss_Referralctl">
    $(function () {
        $('#<%=grdView.ClientID %> > tbody > tr:gt(0):not(.trEmpty)').live('click', function () {
            if ($(this).attr('class') != 'selected') {
                $('#<%=grdView.ClientID %> tr.selected').removeClass('selected');
                $('#<%=hdnSelectedID.ClientID %>').val($(this).closest('tr').find('.keyField').html());
                $(this).addClass('selected');
            }
        });
        $('#<%=grdView.ClientID %> tr:eq(1)').click();
    });

    function onBeforeProcess(param) {
        if (!getSelectedItem()) {
            return false;
        }
        else {
            return true;
        }
    }

    function onGetEntryPopupReturnValue() {
        var result = $('#<%=hdnSelectedID.ClientID %>').val();
        return result;
    }

    function onAfterProcessPopupEntry(param) {
        $('#hdnRightPanelContentCode').val('getDataRujukan');
        return $('#<%=txtNoPeserta.ClientID %>').val(param);
    }

    $("#btnAmbilRujukan").on("click", function (e) {
        e.preventDefault();
        if ($('#<%=txtNoPeserta.ClientID %>').val() == '')
            showToast("Data Rujukan", "Nomor Kartu Peserta harus diisi!");
        else {
            var noPeserta = $('#<%=txtNoPeserta.ClientID %>').val();
            var asalRujukan = 2;

            if ($('#<%=hdnIsBridgingBPJSVClaimVersion.ClientID %>').val() == Constant.VersionBridgingBPJSVClaim.v1_0) {
                BPJSService.getRujukanList(noPeserta, asalRujukan, function (result) {
                    try {
                        var resultInfo = result.split('|');
                        if (resultInfo[0] == "1") {
                            $('#<%=hdnListRujukan.ClientID %>').val(resultInfo[1]);
                        }
                        else {
                            showToast("Data Rujukan", resultInfo[2]);
                            $('#<%=hdnListRujukan.ClientID %>').val('');
                        }
                        cbpView.PerformCallback();
                    } catch (err) {
                        $('#<%=hdnListRujukan.ClientID %>').val('');
                        showToast("Data Rujukan BPJS", err);
                    }
                });
            } else {
                var asalRujukan = cboReferral.GetValue();
                BPJSService.getRujukanListMedinfrasAPI(noPeserta, asalRujukan, function (result) {
                    try {
                        var resultInfo = result.split('|');
                        if (resultInfo[0] == "1") {
                            $('#<%=hdnListRujukan.ClientID %>').val(resultInfo[1]);
                        }
                        else {
                            showToast("Data Rujukan", resultInfo[2]);
                            $('#<%=hdnListRujukan.ClientID %>').val('');
                        }
                        cbpView.PerformCallback();
                    } catch (err) {
                        $('#<%=hdnListRujukan.ClientID %>').val('');
                        showToast("Data Rujukan BPJS", err);
                    }
                });
            }
        }
    });

    $('#<%=btnAmbilRujukan.ClientID %>').click(function () {
        var test = $('#<%=txtNoPeserta.ClientID %>').val();
        $('#<%=hdnNoPeserta.ClientID %>').val(test);
        cbpView.PerformCallback('save');
    });

    $('.chkIsProcessItem input').live('change', function () {
        var $cell = $(this).closest("td");
        var $tr = $cell.closest('tr');
        var isChecked = $(this).is(":checked");
        if ($(this).is(':checked')) {
            $cell.addClass('highlight');
            //CONTOH TESTING DATA                           //field di RegistrationBPJS
            var TglKunjungan = $tr.find('.hdnTglKunjungan').val();          //2020-07-20                                    TanggalRujukan
            var NoKunjungan = $tr.find('.hdnNoKunjungan').val();            //0153B0400720P00xxxx                           NoRujukan
            var NoKartu = $tr.find('.hdnNoKartu').val();                    //000190157xxxx                                 NoPeserta
            var NamaPeserta = $tr.find('.hdnNamaPeserta').val();            //dani fitrianingsih                            NamaPeserta
            var KodePPK = $tr.find('.hdnKodePPK').val();                    //0153B040                                      KodePPK
            var NamaPPK = $tr.find('.hdnNamaPPK').val();                    //KLINIK ARY FARMA                              NamaPPK
            var KodeKelas = $tr.find('.hdnKodeKelas').val();                //2                                             KodeKelas ?
            var NamaKelas = $tr.find('.hdnNamaKelas').val();                //KELAS II                                      KelasTanggungan / NamaKelasTanggungan ?
            var KodeJenisPeserta = $tr.find('.hdnKodeJenisPeserta').val();  //8                                             KodeJenisPeserta ?
            var NamaJenisPeserta = $tr.find('.hdnNamaJenisPeserta').val();  //ANGGOTA POLRI                                 JenisPeserta
            var Nik = $tr.find('.hdnNik').val();                            //331308540888xxxx                              NIK ?
            var TglLahir = $tr.find('.hdnTglLahir').val();                  //1988-08-14                                    TglLahir ?
            var KodeSex = $tr.find('.hdnKodeSex').val();                    //P                                             KodeSex ?
            var KodeStatusPeserta = $tr.find('.hdnKodeStatusPeserta').val(); //0                                             KodeStatusPeserta ? 
            var NamaStatusPeserta = $tr.find('.hdnNamaStatusPeserta').val(); //AKTIF                                         NamaStatusPeserta ?
            var KodeDiagnosa = $tr.find('.hdnKodeDiagnosa').val();          //O82.9                                         KodeDiagnosa
            var NamaDiagnosa = $tr.find('.hdnNamaDiagnosa').val();          //Delivery by caesarean section, unspecified    NamaDiagnosa
            var KodePerujuk = $tr.find('.hdnKodePerujuk').val();            //0153B040                                      KodeRujukan
            var NamaPerujuk = $tr.find('.hdnNamaPerujuk').val();            //KLINIK ARY FARMA                              NamaRujukan
            var KodePoli = $tr.find('.hdnKodePoli').val();                  //OBG                                           KodePoliklinik
            var NamaPoli = $tr.find('.hdnNamaPoli').val();                  //OBGYN                                         NamaPoliklinik
            var KodePelayanan = $tr.find('.hdnKodePelayanan').val();        //2                                             JenisPelayanan 
            var NamaPelayanan = $tr.find('.hdnNamaPelayanan').val();        //Rawat Jalan                                   NamaPelayanan
            var Keluhan = $tr.find('.hdnKeluhan').val();                    //kehamilan dengan indikasi presbo              Keluhan

            $('#<%=hdnTglKunjungan1.ClientID %>').val(TglKunjungan);
            $('#<%=hdnNoKunjungan1.ClientID %>').val(NoKunjungan);
            $('#<%=hdnNoKartu1.ClientID %>').val(NoKartu);
            $('#<%=hdnNamaPeserta1.ClientID %>').val(NamaPeserta);
            $('#<%=hdnKodePPK1.ClientID %>').val(KodePPK);
            $('#<%=hdnNamaPPK1.ClientID %>').val(NamaPPK);
            $('#<%=hdnKodeKelas1.ClientID %>').val(KodeKelas);
            $('#<%=hdnNamaKelas1.ClientID %>').val(NamaKelas);
            $('#<%=hdnKodeJenisPeserta1.ClientID %>').val(KodeStatusPeserta);
            $('#<%=hdnNamaJenisPeserta1.ClientID %>').val(NamaStatusPeserta);
            $('#<%=hdnNik1.ClientID %>').val(Nik);
            $('#<%=hdnTglLahir1.ClientID %>').val(TglLahir);
            $('#<%=hdnKodeSex1.ClientID %>').val(KodeSex);
            $('#<%=hdnKodeStatusPeserta1.ClientID %>').val(KodeStatusPeserta);
            $('#<%=hdnNamaStatusPeserta1.ClientID %>').val(NamaStatusPeserta);
            $('#<%=hdnKodeDiagnosa1.ClientID %>').val(KodeDiagnosa);
            $('#<%=hdnNamaDiagnosa1.ClientID %>').val(NamaDiagnosa);
            $('#<%=hdnKodePerujuk1.ClientID %>').val(KodePerujuk);
            $('#<%=hdnNamaPerujuk1.ClientID %>').val(NamaPerujuk);
            $('#<%=hdnKodePoli1.ClientID %>').val(KodePoli);
            $('#<%=hdnNamaPoli1.ClientID %>').val(NamaPoli);
            $('#<%=hdnKodePelayanan1.ClientID %>').val(KodePelayanan);
            $('#<%=hdnNamaPelayanan1.ClientID %>').val(NamaPelayanan);
            $('#<%=hdnKeluhan1.ClientID %>').val(Keluhan);
        }
        else {
            $cell.removeClass('highlight');
        }
    });

    function getSelectedItem() {
        var tempSelectedID = "";
        var count = 0;
        $('.grdView .chkIsProcessItem input:checked').each(function () {
            var $tr = $(this).closest('tr');
            var id = $(this).closest('tr').find('.keyField').html();

            if (tempSelectedID != "") {
                tempSelectedID += ",";
            }
            tempSelectedID += id;
            count += 1;
        });
        if (count > 1) {
            showToast("Data Rujukan BPJS", "Hanya boleh pilih 1 (satu) nomor rujukan untuk setiap kunjungan !");
            return false;
        }
        else if (count == 0) {
            showToast("Data Rujukan BPJS", "Belum ada nomor rujukan yang dipilih !");
            return false;
        }
        else {
            return true;
        }
    }

    function onCbpViewEndCallback(s) {
        hideLoadingPanel();
        var param = s.cpResult.split('|');
        if (param[0] == '0') {
            showToast("Data Rujukan BPJS", "Error Message : <br/><span style='color:red'>" + param[1] + "</span>");
        }
    }
</script>
<input type="hidden" id="hdnListRujukan" runat="server" />
<input type="hidden" id="hdnAsalRujukan" runat="server" />
<input type="hidden" id="hdnNoPeserta" runat="server" />
<input type="hidden" id="hdnRegistrationID" runat="server" />
<input type="hidden" id="hdnSelectedID" runat="server" />
<input type="hidden" id="hdnTglKunjungan1" runat="server" />
<input type="hidden" id="hdnNoKunjungan1" runat="server" />
<input type="hidden" id="hdnNoKartu1" runat="server" />
<input type="hidden" id="hdnNamaPeserta1" runat="server" />
<input type="hidden" id="hdnKodePPK1" runat="server" />
<input type="hidden" id="hdnNamaPPK1" runat="server" />
<input type="hidden" id="hdnKodeKelas1" runat="server" />
<input type="hidden" id="hdnNamaKelas1" runat="server" />
<input type="hidden" id="hdnKodeJenisPeserta1" runat="server" />
<input type="hidden" id="hdnNamaJenisPeserta1" runat="server" />
<input type="hidden" id="hdnNik1" runat="server" />
<input type="hidden" id="hdnTglLahir1" runat="server" />
<input type="hidden" id="hdnKodeSex1" runat="server" />
<input type="hidden" id="hdnKodeStatusPeserta1" runat="server" />
<input type="hidden" id="hdnNamaStatusPeserta1" runat="server" />
<input type="hidden" id="hdnKodeDiagnosa1" runat="server" />
<input type="hidden" id="hdnNamaDiagnosa1" runat="server" />
<input type="hidden" id="hdnKodePerujuk1" runat="server" />
<input type="hidden" id="hdnNamaPerujuk1" runat="server" />
<input type="hidden" id="hdnKodePoli1" runat="server" />
<input type="hidden" id="hdnNamaPoli1" runat="server" />
<input type="hidden" id="hdnKodePelayanan1" runat="server" />
<input type="hidden" id="hdnNamaPelayanan1" runat="server" />
<input type="hidden" id="hdnKeluhan1" runat="server" />
<input type="hidden" runat="server" id="hdnIsBridgingBPJSVClaimVersion" value="" />
<div style="height: 440px; overflow-y: auto">
    <table class="tblContentArea">
        <tr>
            <td style="padding: 5px; vertical-align: top">
                <table>
                    <colgroup>
                        <col width="115px" />
                        <col width="100px" />
                        <col width="115px" />
                        <col />
                    </colgroup>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("No. Kartu")%></label>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtNoPeserta" runat="server" Width="99%" ReadOnly="true" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%:GetLabel("Pilih Rujukan")%></label>
                        </td>
                        <td>
                            <dxe:ASPxComboBox ID="cboReferral" ClientInstanceName="cboReferral" Width="150px"
                                runat="server">
                                <ClientSideEvents ValueChanged="function(s){ onCboReferralValueChanged(s); }" />
                            </dxe:ASPxComboBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <input type="button" id="btnAmbilRujukan" value="Cari Data" style="margin-left: 5px;
                                width: 150px" runat="server" />
                        </td>
                    </tr>
                </table>
                <div>
                    <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
                        ShowLoadingPanel="false" OnCallback="cbpView_Callback">
                        <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ onCbpViewEndCallback(s); }" />
                        <PanelCollection>
                            <dx:PanelContent ID="PanelContent1" runat="server">
                                <asp:Panel runat="server" ID="pnlRujukan" Style="width: 100%; margin-left: auto;
                                    margin-right: auto; position: relative; font-size: 0.95em;">
                                    <asp:GridView ID="grdView" runat="server" CssClass="grdView" AutoGenerateColumns="false">
                                        <Columns>
                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center"
                                                HeaderStyle-Width="40px">
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chkIsProcessItem" runat="server" CssClass="chkIsProcessItem" AutoPostBack="false" />
                                                    <input type="hidden" id="hdnTglKunjungan" class="hdnTglKunjungan" runat="server"
                                                        value='<%#: Eval("tglKunjungan")%>' />
                                                    <input type="hidden" id="hdnNoKunjungan" class="hdnNoKunjungan" runat="server" value='<%#: Eval("noKunjungan")%>' />
                                                    <input type="hidden" id="hdnNoKartu" class="hdnNoKartu" runat="server" value='<%#: Eval("noKartu")%>' />
                                                    <input type="hidden" id="hdnNamaPeserta" class="hdnNamaPeserta" runat="server" value='<%#: Eval("namaPeserta")%>' />
                                                    <input type="hidden" id="hdnKodePPK" class="hdnKodePPK" runat="server" value='<%#: Eval("kodePPK")%>' />
                                                    <input type="hidden" id="hdnNamaPPK" class="hdnNamaPPK" runat="server" value='<%#: Eval("namaPPK")%>' />
                                                    <input type="hidden" id="hdnKodeKelas" class="hdnKodeKelas" runat="server" value='<%#: Eval("kodeKelas")%>' />
                                                    <input type="hidden" id="hdnNamaKelas" class="hdnNamaKelas" runat="server" value='<%#: Eval("namaKelas")%>' />
                                                    <input type="hidden" id="hdnKodeJenisPeserta" class="hdnKodeJenisPeserta" runat="server"
                                                        value='<%#: Eval("kodeJenisPeserta")%>' />
                                                    <input type="hidden" id="hdnNamaJenisPeserta" class="hdnNamaJenisPeserta" runat="server"
                                                        value='<%#: Eval("namaJenisPeserta")%>' />
                                                    <input type="hidden" id="hdnNik" class="hdnNik" runat="server" value='<%#: Eval("nik")%>' />
                                                    <input type="hidden" id="hdnTglLahir" class="hdnTglLahir" runat="server" value='<%#: Eval("tglLahir")%>' />
                                                    <input type="hidden" id="hdnKodeSex" class="hdnKodeSex" runat="server" value='<%#: Eval("kodeSex")%>' />
                                                    <input type="hidden" id="hdnKodeStatusPeserta" class="hdnKodeStatusPeserta" runat="server"
                                                        value='<%#: Eval("kodeStatusPeserta")%>' />
                                                    <input type="hidden" id="hdnNamaStatusPeserta" class="hdnNamaStatusPeserta" runat="server"
                                                        value='<%#: Eval("namaStatusPeserta")%>' />
                                                    <input type="hidden" id="hdnKodeDiagnosa" class="hdnKodeDiagnosa" runat="server"
                                                        value='<%#: Eval("kodeDiagnosa")%>' />
                                                    <input type="hidden" id="hdnNamaDiagnosa" class="hdnNamaDiagnosa" runat="server"
                                                        value='<%#: Eval("namaDiagnosa")%>' />
                                                    <input type="hidden" id="hdnKodePerujuk" class="hdnKodePerujuk" runat="server" value='<%#: Eval("kodePerujuk")%>' />
                                                    <input type="hidden" id="hdnNamaPerujuk" class="hdnNamaPerujuk" runat="server" value='<%#: Eval("namaPerujuk")%>' />
                                                    <input type="hidden" id="hdnKodePoli" class="hdnKodePoli" runat="server" value='<%#: Eval("kodePoli")%>' />
                                                    <input type="hidden" id="hdnNamaPoli" class="hdnNamaPoli" runat="server" value='<%#: Eval("namaPoli")%>' />
                                                    <input type="hidden" id="hdnKodePelayanan" class="hdnKodePelayanan" runat="server"
                                                        value='<%#: Eval("kodePelayanan")%>' />
                                                    <input type="hidden" id="hdnNamaPelayanan" class="hdnNamaPelayanan" runat="server"
                                                        value='<%#: Eval("namaPelayanan")%>' />
                                                    <input type="hidden" id="hdnKeluhan" class="hdnKeluhan" runat="server" value='<%#: Eval("keluhan")%>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="noKunjungan" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                            <asp:BoundField DataField="tglKunjungan" HeaderText="Tanggal" HeaderStyle-Width="100px"
                                                HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                            <asp:BoundField DataField="noKunjungan" HeaderText="No. Kunjungan" HeaderStyle-Width="150px"
                                                HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                                            <asp:BoundField DataField="namaPelayanan" HeaderText="Jenis Pelayanan" HeaderStyle-Width="100px"
                                                HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                                            <asp:BoundField DataField="namaPoli" HeaderText="Poli Rujukan" HeaderStyle-Width="150px"
                                                HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                                            <asp:BoundField DataField="cfDiagnosa" HeaderText="Diagnosa" HeaderStyle-Width="250px"
                                                HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                                            <asp:BoundField DataField="keluhan" HeaderText="Keluhan" HeaderStyle-HorizontalAlign="Left"
                                                ItemStyle-HorizontalAlign="Left" />
                                        </Columns>
                                    </asp:GridView>
                                    <div class="imgLoadingGrdView" id="containerImgLoadingReferrer">
                                        <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                                    </div>
                                </asp:Panel>
                            </dx:PanelContent>
                        </PanelCollection>
                    </dxcp:ASPxCallbackPanel>
                </div>
            </td>
        </tr>
    </table>
</div>
