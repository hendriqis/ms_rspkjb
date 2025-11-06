<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="HistoryPatientBPJSCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.HistoryPatientBPJSCtl" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<script type="text/javascript" id="dxss_Referralctl">
    $(function () {
        setDatePicker('<%=txtDateFrom.ClientID %>');
        setDatePicker('<%=txtDateTo.ClientID %>');
    });

    $("#btnSearchHistory").on("click", function (e) {
        e.preventDefault();
        if ($('#<%=txtNoPeserta.ClientID %>').val() == '')
            showToast('Nomor Peserta harus diisi!');
        else {
            var noPeserta = $('#<%=txtNoPeserta.ClientID %>').val();
            var dateFrom = Methods.getDatePickerDate($('#<%=txtDateFrom.ClientID %>').val());
            var tglAwal = Methods.dateToYMD(dateFrom);
            var dateTo = Methods.getDatePickerDate($('#<%=txtDateTo.ClientID %>').val());
            var tglAkhir = Methods.dateToYMD(dateTo);

            $('#<%:hdnNoPeserta.ClientID %>').val(noPeserta);
            $('#<%:hdnTglAwal.ClientID %>').val(tglAwal);
            $('#<%:hdnTglAkhir.ClientID %>').val(tglAkhir);

            cbpView.PerformCallback('refresh');
        }
    });

</script>
<input type="hidden" id="hdnNoPeserta" runat="server" />
<input type="hidden" id="hdnTglAwal" runat="server" />
<input type="hidden" id="hdnTglAkhir" runat="server" />
<input type="hidden" id="hdnBPJSVersion" runat="server" />
<div style="height: 440px; overflow-y: auto">
    <div class="pageTitle">
        <%=GetLabel("History Pasien BPJS")%></div>
    <table class="tblContentArea">
        <tr>
            <td style="padding: 5px; vertical-align: top">
                <table>
                    <colgroup>
                        <col width="100px" />
                        <col width="150px" />
                        <col width="150px" />
                        <col />
                    </colgroup>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("No. Kartu")%></label>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="txtNoPeserta" runat="server" ReadOnly="true" Width="150px" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <label>
                                <%=GetLabel("Tanggal Mulai") %></label>
                        </td>
                        <td>
                            <asp:TextBox runat="server" CssClass="datepicker" ID="txtDateFrom" Width="100px" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <label>
                                <%=GetLabel("Tanggal Akhir") %></label>
                        </td>
                        <td>
                            <asp:TextBox runat="server" CssClass="datepicker" ID="txtDateTo" Width="100px" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td>
                            <input type="button" id="btnSearchHistory" value='<%= GetLabel("Histori Pelayanan")%>' />
                        </td>
                    </tr>
                </table>
                <div>
                    <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
                        ShowLoadingPanel="false" OnCallback="cbpView_Callback">
                        <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ hideLoadingPanel(s); }" />
                        <PanelCollection>
                            <dx:PanelContent ID="PanelContent1" runat="server">
                                <asp:Panel runat="server" ID="pnlHistoryBPJSGrdView" Style="width: 100%; margin-left: auto;
                                    margin-right: auto; position: relative; font-size: 0.95em;">
                                    <asp:GridView ID="grdView" runat="server" CssClass="grdView notAllowSelect" AutoGenerateColumns="false">
                                        <Columns>
                                            <asp:BoundField DataField="diagnosa" HeaderText="Diagnosa" HeaderStyle-Width="100px"
                                                HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                                            <asp:BoundField DataField="jnsPelayanan" HeaderText="Jenis Pelayanan" HeaderStyle-Width="100px"
                                                HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                                            <asp:BoundField DataField="kelasRawat" HeaderText="Kelas Rawat" HeaderStyle-Width="100px"
                                                HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                                            <asp:BoundField DataField="namaPeserta" HeaderText="Nama Kelas" HeaderStyle-Width="100px"
                                                HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                                            <asp:BoundField DataField="noKartu" HeaderText="No Kartu" HeaderStyle-Width="100px"
                                                HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                                            <asp:BoundField DataField="noSep" HeaderText="No Sep" HeaderStyle-Width="100px"
                                                HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                                            <asp:BoundField DataField="noRujukan" HeaderText="No Rujukan" HeaderStyle-Width="100px"
                                                HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                                            <asp:BoundField DataField="poli" HeaderText="Poli" HeaderStyle-Width="100px"
                                                HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                                            <asp:BoundField DataField="ppkPelayanan" HeaderText="PPK Pelayanan" HeaderStyle-Width="100px"
                                                HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                                            <asp:BoundField DataField="tglPlgSep" HeaderText="Tgl. Pulang SEP" HeaderStyle-Width="100px"
                                                HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
                                            <asp:BoundField DataField="tglSep" HeaderText="Tgl. SEP" HeaderStyle-Width="100px"
                                                HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left" />
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
    <div style="width: 100%; text-align: right">
        <input type="button" value='<%= GetLabel("Tutup")%>' onclick="pcRightPanelContent.Hide();" />
    </div>
</div>
