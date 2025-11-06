<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="GridPatientReservationCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Controls.GridPatientReservationCtl" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<script type="text/javascript" id="PasienReservasi">
    $('.lvwViewReservasi > tbody > tr:gt(0):not(.trEmpty)').live('click', function () {
        var isFromRegistration = $(this).closest('tr').find('.hdnIsFromRegistration').val();
        if (isFromRegistration == 'True') {
            var id = $(this).closest('tr').find('.hdnReservasiID').val();
            $('#<%=hdnNewReservationID.ClientID %>').val(id)
            showToastConfirmation("Pasien Sudah Mempunyai Nomor Registrasi, Lanjut?", function (resultReservasi) {
                if (resultReservasi) {
                    cbpViewReservasi.PerformCallback('transfer');
                    showToastConfirmation("Pasien Sudah Mempunyai Nomor Registrasi, Lanjut?")
                } else {
                    cbpViewReservasi.PerformCallback('refresh');
                }
            })
        } else {
            var id = $(this).closest('tr').find('.hdnReservasiID').val();
            $('#<%=hdnNewReservationID.ClientID %>').val(id)
            showToastConfirmation("Pasien Sudah Melakukan Reservasi, Lanjut untuk Registrasi?", function (resultReservasi) {
                if (resultReservasi) {
                    __doPostBack('<%=btnOpenTransactionDtReservasi.UniqueID%>', '');
                } else {
                    cbpViewReservasi.PerformCallback('refresh');
                }
            })
        }
    });

    var pageCountReservasi = parseInt('<%=pageCountReservasi %>');
    var currPageReservasi = parseInt('<%=currPageReservasi %>');
    $(function () {
        setPaging($("#pagingReservasi"), pageCountReservasi, function (page) {
            cbpViewReservasi.PerformCallback('changepage|' + page);
        }, null, currPageReservasi);
    });

    function onCbpViewEndCallbackReservasi(s) {
        hideLoadingPanel();

        var param = s.cpResult.split('|');
        if (param[0] == 'refresh') {
            var pageCountReservasi = parseInt(param[1]);
            if (pageCountReservasi > 0)
                $('#<%=lvwViewReservasi.ClientID %> tr:eq(1)').click();

            setPaging($("#pagingReservasi"), pageCountReservasi, function (page) {
                cbpViewReservasi.PerformCallback('changepage|' + page);
            });
        }
        else
            $('#<%=lvwViewReservasi.ClientID %> tr:eq(1)').click();
    }

    function refreshGrdPasienReservasi() {
        cbpViewReservasi.PerformCallback('refresh');
    }

</script>
<div style="display: none">
    <asp:Button ID="btnOpenTransactionDtReservasi" runat="server" UseSubmitBehavior="false"
        OnClientClick="return onBeforeOpenTransactionDtReservasi();" OnClick="btnOpenTransactionDtReservasi_Click" /></div>
<input type="hidden" runat="server" id="hdnNewReservationID" value="" />
<dxcp:ASPxCallbackPanel ID="cbpViewReservasi" runat="server" Width="100%" ClientInstanceName="cbpViewReservasi"
    ShowLoadingPanel="false" OnCallback="cbpViewReservasi_Callback">
    <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ onCbpViewEndCallbackReservasi(s); }" />
    <PanelCollection>
        <dx:PanelContent ID="PanelContent1" runat="server">
            <asp:Panel runat="server" ID="pnlGridViewOrder" Style="width: 100%; margin-left: auto;
                margin-right: auto; position: relative; font-size: 0.95em; height: 365px; overflow-y: scroll;">
                <asp:ListView runat="server" ID="lvwViewReservasi" OnItemDataBound="lvwViewReservasi_ItemDataBound">
                    <EmptyDataTemplate>
                        <table id="tblView" runat="server" class="grdCollapsible lvwViewReservasi" cellspacing="0"
                            rules="all">
                            <tr>
                                <th style="width: 100px; text-align: left;">
                                    <%=GetLabel("No. Reservasi")%>
                                </th>
                                <th style="width: 100px; text-align: left;">
                                    <%=GetLabel("Tgl. Reservasi")%>
                                </th>
                                <th style="width: 200px; text-align: left">
                                    <%=GetLabel("Informasi Pasien")%>
                                </th>
                                <th style="width: 100px; text-align: left">
                                    <%=GetLabel("Kelas Permintaan")%>
                                </th>
                                <th style="width: 100px; text-align: left">
                                    <%=GetLabel("Penjamin Bayar")%>
                                </th>
                                <th style="width: 100px; text-align: left">
                                    <%=GetLabel("Asal Pasien")%>
                                </th>
                            </tr>
                            <tr class="trEmpty">
                                <td colspan="9">
                                    <%=GetLabel("Tidak ada pasien")%>
                                </td>
                            </tr>
                        </table>
                    </EmptyDataTemplate>
                    <LayoutTemplate>
                        <table id="tblView" runat="server" class="grdCollapsible lvwViewReservasi" cellspacing="0"
                            rules="all">
                            <tr>
                                <th style="width: 100px; text-align: left;">
                                    <%=GetLabel("No. Reservasi")%>
                                </th>
                                <th style="width: 100px; text-align: left;">
                                    <%=GetLabel("Tgl. Reservasi")%>
                                </th>
                                <th style="width: 200px; text-align: left">
                                    <%=GetLabel("Informasi Pasien")%>
                                </th>
                                <th style="width: 100px; text-align: left">
                                    <%=GetLabel("Kelas Permintaan")%>
                                </th>
                                <th style="width: 100px; text-align: left">
                                    <%=GetLabel("Penjamin Bayar")%>
                                </th>
                                <th style="width: 100px; text-align: left">
                                    <%=GetLabel("Asal Pasien")%>
                                </th>
                            </tr>
                            <tr runat="server" id="itemPlaceholder">
                            </tr>
                        </table>
                    </LayoutTemplate>
                    <ItemTemplate>
                        <tr>
                            <td>
                                <div>
                                    <div style="float: left">
                                        <%#: Eval("ReservationNo")%></div>
                                    <input type="hidden" class="hdnReservasiID" value='<%#: Eval("ReservationID") %>' />
                                    <input type="hidden" class="hdnIsFromRegistration" value='<%#: Eval("IsFromRegistration") %>' />
                                </div>
                            </td>
                            <td>
                                <div>
                                    <div style="float: left">
                                        <%#: Eval("cfReservationDateInString")%> - <%#: Eval("ReservationTime")%></div>
                                </div>
                            </td>
                            <td>
                                <div>
                                    <div style="float: left">
                                        <%#: Eval("RegistrationNo")%>,
                                            <i>
                                                <%#: Eval("MedicalNo")%>,
                                            </i>
                                        <%#: Eval("PatientName")%>,
                                        <%#: Eval("Gender")%>
                                        </div>
                                </div>
                            </td>
                            <td>
                                <div>
                                    <div style="float: left">
                                        Kelas : <%#: Eval("ClassName")%></div>
                                    <div style="float: left">
                                        Unit Pelayanan : <%#: Eval("ServiceUnitName")%></div>
                                    <div style="float: left">
                                        Kelas Tagihan : <%#: Eval("ChargeClassName")%></div>
                                    <div style="float: left">
                                        Kamar : <%#: Eval("BedCode")%></div>
                                </div>
                            </td>
                            <td>
                                <div>
                                    <div style="float: left">
                                        <%#: Eval("BusinessPartnerName")%></div>
                                </div>
                            </td>
                            <td>
                                <div>
                                    <div style="float: left">
                                        <%#: Eval("PatientOrigin")%></div>
                                </div>
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:ListView>
            </asp:Panel>
        </dx:PanelContent>
    </PanelCollection>
</dxcp:ASPxCallbackPanel>
<div class="imgLoadingGrdView" id="containerImgLoadingView2">
    <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
</div>
<div class="containerPaging">
    <div class="wrapperPaging">
        <div id="pagingReservasi">
        </div>
    </div>
</div>
