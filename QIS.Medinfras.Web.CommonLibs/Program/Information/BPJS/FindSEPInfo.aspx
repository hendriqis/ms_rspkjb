<%@ Page Language="C#" MasterPageFile="~/Libs/MasterPage/MPTrx.master" AutoEventWireup="true" 
    CodeBehind="FindSEPInfo.aspx.cs" Inherits="QIS.Medinfras.Web.CommonLibs.Program.FindSEPInfo" %>

<asp:Content ID="Content3" ContentPlaceHolderID="plhCustomButtonToolbar" runat="server">
    <li id="btnFind" CRUDMode="R" runat="server"><img src='<%=ResolveUrl("~/Libs/Images/Icon/tbsearch.png")%>' alt="" /><br style="clear:both"/><div><%=GetLabel("Search")%></div></li>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div class="menuTitle"><%=GetMenuCaption()%></div>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript">
        var isRegistrationClosed = false;
        function onLoad() {
            $('#<%=btnFind.ClientID %>').click(function () {
                var noSEP = $('#<%=txtNoSEP.ClientID %>').val();
                if (noSEP == '')
                    showToast('Pencarian Data SEP', 'Nomor SEP harus diisi');
                else {
                    if ($('#<%:chkIsINACBG.ClientID %>').is(':checked')) {
                        BPJSService.getSEPInfo2(noSEP, function (result) {
                            GetSepInfoHandler(result);
                        });
                    }
                    else {
                        BPJSService.getSEPInfo1(noSEP, function (result) {
                            GetSepInfoHandler(result);
                        });
                     }
                }
            });
        }

        function GetSepInfoHandler(result) {
            try {
                var resultInfo = result.split('|');
                if (resultInfo[0] == "1") {
                    var obj = jQuery.parseJSON(resultInfo[1]);
                    $('#<%=txtNamaPeserta.ClientID %>').val(obj.response.nama);
                    var tglLahir = obj.response.tglLahir;
                    var arrDate = tglLahir.split("-");
                    var strDate = arrDate[0] + arrDate[1] + arrDate[2]
                    var date = Methods.stringToDate(strDate);
                    $('#<%=txtMedicalNo.ClientID %>').val(obj.response.noMR);
                    $('#<%=txtDOB.ClientID %>').val(Methods.dateToDMY(date));
                    $('#<%=txtJenisPeserta.ClientID %>').val(obj.response.jenisPeserta);
                    $('#<%=txtKelas.ClientID %>').val(obj.response.hakKelas);
                    $('#<%=txtPelayanan.ClientID %>').val(obj.response.jenisPelayanan);
                    
                    if (obj.response.sex == 'P')
                        $('#<%=txtGender.ClientID %>').val('Perempuan');
                    else
                        $('#<%=txtGender.ClientID %>').val('Laki-laki');

                    if (obj.response.poliEksekutif == '0') {
                        $('#<%=chkIsPoliExecutive.ClientID %>').prop("checked", false);
                    }
                    else {
                        $('#<%=chkIsPoliExecutive.ClientID %>').prop("checked", true);
                    }
                    var tglSEP = obj.response.tglSEP;
                    var arrSEPDate = tglSEP.split("-");
                    var strSEPDate = arrSEPDate[0] + arrSEPDate[1] + arrSEPDate[2]
                    var dateSEP = Methods.stringToDate(strSEPDate);
                    $('#<%=txtTglSEP.ClientID %>').val(Methods.dateToDMY(dateSEP));
                    $('#<%=txtPoliTujuan.ClientID %>').val(obj.response.poliTujuan);
                    $('#<%=txtCatatan.ClientID %>').val(obj.response.catatan);
                    $('#<%=txtDiagnoseName.ClientID %>').val(obj.response.diagnosa);
                    $('#<%=txtChargeClass.ClientID %>').val(obj.response.kelasRawat);

                }
                else {
                    showToast('DATA SEP : TIDAK DITEMUKAN', resultInfo[2]);
                    ResetDataSEP();
                }
            }
            catch (err) {
                showToast('DATA SEP : TIDAK DITEMUKAN', err);
                ResetDataSEP();
            }
        }

        function ResetDataSEP() {
            $('#<%=txtNamaPeserta.ClientID %>').val('');
            $('#<%=txtMedicalNo.ClientID %>').val('');
            $('#<%=txtDOB.ClientID %>').val('');
            $('#<%=txtGender.ClientID %>').val('');
            $('#<%=txtJenisPeserta.ClientID %>').val('');
            $('#<%=txtKelas.ClientID %>').val('');
            $('#<%=txtTglSEP.ClientID %>').val('');
            $('#<%=txtPoliTujuan.ClientID %>').val('');
            $('#<%=txtCatatan.ClientID %>').val('');
            $('#<%=txtDiagnoseName.ClientID %>').val('');
            $('#<%=chkIsPoliExecutive.ClientID %>').prop("checked", false);
        }
    </script>
    <input type="hidden" id="hdnDepartmentID" value="" runat="server" />
    <input type="hidden" id="hdnNoSEP" value="" runat="server" />
    <div style="padding: 5px 0;">
    <input type="hidden" runat="server" id="hdnIsAdd" value="" />
    <input type="hidden" runat="server" id="hdnMRN" value="" />
    <input type="hidden" id="hdnRegistrationNo" value="" runat="server" />
    <input type="hidden" runat="server" id="hdnID" value="" />
    <input type="hidden" runat="server" id="hdnKdJenisPeserta" value="" />
    <input type="hidden" runat="server" id="hdnKdKelas" value="" />
    <input type="hidden" runat="server" id="hdnAsalRujukan" value="1" />
    <input type="hidden" runat="server" id="hdnIsBpjsRegistrationCtl" value="" />
    <input type="hidden" runat="server" id="hdnIsPoliExecutive" value="0" />
    <table class="tblContentArea" width="100%">
        <colgroup>
            <col width="50%" />
            <col width="50%" />
        </colgroup>
        <tr>
            <td valign="top">
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
                                <%=GetLabel("No. SEP")%></label>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtNoSEP" runat="server" Width="99%" />
                        </td>
                        <td>
                            <input type="checkbox" id="chkIsINACBG" runat="server" /><label>Integrasi dengan INACBG</label>
                        </td>
                    </tr>
                </table>
            </td>
            <td valign="top">
            </td>
        </tr>
    </table>
    <table class="tblContentArea" width="100%">
        <colgroup>
            <col width="50%" />
            <col width="50%" />
        </colgroup>
        <tr>
            <td valign="top">
                <table style="width:100%">
                    <colgroup>
                        <col width="115px" />
                        <col width="80px" />
                        <col width="90px" />
                        <col />
                    </colgroup>
                    <tr>
                        <td colspan="4">
                            <h4>
                                <%=GetLabel("DATA PESERTA :")%></h4>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Nama")%></label>
                        </td>
                        <td colspan="3">
                            <asp:TextBox ID="txtNamaPeserta" runat="server" Width="99%" ReadOnly="true" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("No. RM")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtMedicalNo" ReadOnly="true" runat="server" Width="100px" />
                        </td>
                        <td>
                            <label class="lblNormal">
                                <%=GetLabel("No. Telepon")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtMobilePhoneNo1" ReadOnly="true" runat="server" Width="100px" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Jenis Kelamin")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtGender" ReadOnly="true" runat="server" Width="100px" />
                        </td>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Tanggal Lahir")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtDOB" Width="100px" runat="server" ReadOnly="true" CssClass="datepicker" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Jenis")%></label>
                        </td>
                        <td colspan="3">
                            <asp:TextBox ID="txtJenisPeserta" ReadOnly="true" Width="99%" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Kelas") %></label>
                        </td>
                        <td colspan="3">
                            <asp:TextBox ID="txtKelas" ReadOnly="true" runat="server" Width="99%" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Faskes/PPK 1")%></label>
                        </td>
                        <td colspan="3">
                            <asp:TextBox ID="txtNamaFaskes" ReadOnly="true" runat="server" Width="99%" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Pelayanan") %></label>
                        </td>
                        <td colspan="3">
                            <input type="hidden" id="hdnKdPelayanan" runat="server" value="" />
                            <asp:TextBox ID="txtPelayanan" ReadOnly="true" runat="server" Width="99%" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            &nbsp;
                        </td>
                        <td>
                            <input type="checkbox" id="chkIsAccident" runat="server" disabled="disabled"/>
                            <label>Kasus KLL</label> 
                        </td>
                        <td colspan="2">
                            <input type="checkbox" id="chkIsCOB" runat="server" disabled="disabled" /><label>Peserta COB</label>
                        </td>
                    </tr>
                    <tr runat="server" style="display: none" id="trAccidentLocationCtl">
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Lokasi Kejadian") %></label>
                        </td>
                        <td colspan="3">
                            <asp:TextBox ID="txtAccidentLocationCtl" runat="server" Width="99%" />
                        </td>
                    </tr>
                    <tr id="trAccidentPayor1" runat="server">
                        <td class="tdLabel" style="vertical-align:top">
                            <label class="lblNormal">
                                <%=GetLabel("Penjamin KLL") %></label>
                        </td>
                        <td colspan="3">
                            <table border="0" cellpadding="0" cellspacing="1">
                                <tr>
                                    <td><asp:CheckBox ID="chkBPJSAccidentPayer1" runat="server" Style="margin-left: 2px" text="Jasa Raharja" Enabled ="false" /></td>
                                    <td><asp:CheckBox ID="chkBPJSAccidentPayer2" runat="server" Style="margin-left: 2px" text="BPJS" Enabled ="false"/></td>
                                    <td><asp:CheckBox ID="chkBPJSAccidentPayer3" runat="server" Style="margin-left: 2px" text="TASPEN" Enabled ="false"/></td>
                                    <td><asp:CheckBox ID="chkBPJSAccidentPayer4" runat="server" Style="margin-left: 2px" text="ASABRI" Enabled ="false"/></td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </td>
            <td valign="top">
                <table style="width: 100%">
                    <colgroup>
                        <col width="125px" />
                        <col width="100px" />
                        <col />
                    </colgroup>
                    <tr>
                        <td colspan="4">
                            <h4>
                                <%=GetLabel("SURAT ELIGIBILITAS PESERTA (SEP) :")%></h4>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Tanggal SEP")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtTglSEP" Width="120px" ReadOnly="true" runat="server" CssClass="datepicker" />
                        </td>
                        <td>
                            <input type="checkbox" id="chkIsPoliExecutive" runat="server" disabled="disabled" /><label>Poli Eksekutif</label>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Kelas Rawat") %></label>
                        </td>
                        <td colspan="4">
                            <input type="hidden" id="hdnKdKelasRawat" runat="server" value="" />
                            <input type="hidden" id="hdnChargeClassID" runat="server" value="" />
                            <asp:TextBox ID="txtChargeClass" ReadOnly="true" runat="server" Width="100%" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Poli Tujuan") %></label>
                        </td>
                        <td colspan="3">
                            <asp:TextBox ID="txtPoliTujuan" ReadOnly="true" runat="server" Width="100%" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal" id="lblDiagnose">
                                <%=GetLabel("Diagnosa")%></label>
                        </td>
                        <td colspan="3">
                            <asp:TextBox ID="txtDiagnoseName" Width="100%" ReadOnly="true" runat="server" />
                        </td>
                    </tr>

                    <tr>
                        <td class="tdLabel">
                            <label>
                                <%=GetLabel("No. Rujukan")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtNoRujukan" ReadOnly="true" runat="server" Width="100%">
                            </asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label>
                                <%=GetLabel("Asal Rujukan")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtKdPpkRujukan" ReadOnly="true" runat="server" Width="100%" />
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtPpkRujukan" ReadOnly="true" runat="server" Width="100%" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Tanggal Rujukan")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtTglRujukan" width="100%" ReadOnly="true" runat="server" CssClass="datepicker" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel" style="vertical-align: top;">
                            <label class="lblNormal">
                                <%=GetLabel("Catatan")%></label>
                        </td>
                        <td colspan="3">
                            <asp:TextBox ID="txtCatatan" Width="100%" ReadOnly="true" runat="server" TextMode="MultiLine" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</div>
</asp:Content>
