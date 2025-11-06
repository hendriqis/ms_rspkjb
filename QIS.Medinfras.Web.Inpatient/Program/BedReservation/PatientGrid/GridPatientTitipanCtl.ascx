<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="GridPatientTitipanCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Controls.GridPatientTitipanCtl" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<script type="text/javascript" id="PasienTitipan">
    $('.lvwViewTitipan > tbody > tr:gt(0):not(.trEmpty)').live('click', function () {
        var id = $(this).closest('tr').find('.hdnVisitID').val();
        var filterExpression = "VisitID = " + id;
        Methods.getObject('GetvConsultVisit7List', filterExpression, function (result) {
            var ReservationID = result.ReservationID;
            $('#<%=hdnReservationID.ClientID %>').val(result.ReservationID);
            $('#<%=hdnRegistrationID.ClientID %>').val(result.RegistrationID);

            var filterExpressionReservation = "ReservationID = " + ReservationID + " AND GCReservationStatus != '" + Constant.BedReservation.CANCELLED + "'";
            Methods.getObject('GetvBedReservationList', filterExpressionReservation, function (resultReservation) {
                if (resultReservation != null) {
                    showToast("Pasien Sudah Memiliki Reservasi");
                    __doPostBack('<%=btnOpenTransactionDtTitipan.UniqueID%>', '');
                } else {
                    showToastConfirmation("Pasien Belum Reservasi, Buat Reservasi Terlebih Dahulu", function (resultTitipan) {
                        if (resultTitipan) {
                            __doPostBack('<%=btnOpenTransactionDtTitipan.UniqueID%>', '');
                        } else {
                            cbpViewTitipan.PerformCallback('refresh');
                        }
                    })
                }
            })
        })
    });

    var pageCountTitipan = parseInt('<%=pageCountTitipan %>');
    var currPageTitipan = parseInt('<%=currPageTitipan %>');
    $(function () {
        setPaging($("#pagingTitipan"), pageCountTitipan, function (page) {
            cbpViewTitipan.PerformCallback('changepage|' + page);
        }, null, currPageTitipan);
    });

    function onCbpViewEndCallbackTitipan(s) {
        hideLoadingPanel();

        var param = s.cpResult.split('|');
        if (param[0] == 'refresh') {
            var pageCountTitipan = parseInt(param[1]);
            if (pageCountTitipan > 0)
                $('#<%=lvwViewTitipan.ClientID %> tr:eq(1)').click();

            setPaging($("#pagingTitipan"), pageCountTitipan, function (page) {
                cbpViewTitipan.PerformCallback('changepage|' + page);
            });
        }
        else
            $('#<%=lvwViewTitipan.ClientID %> tr:eq(1)').click();
    }

    function refreshGrdPasienTitipan() {
        cbpViewTitipan.PerformCallback('refresh');
    }
</script>
<div style="display: none">
    <asp:Button ID="btnOpenTransactionDtTitipan" runat="server" UseSubmitBehavior="false"
        OnClientClick="return onBeforeOpenTransactionDtTitipan();" OnClick="btnOpenTransactionDtTitipan_Click" /></div>
<input type="hidden" runat="server" id="hdnVisitID" value="" />
<input type="hidden" runat="server" id="hdnReservationID" value="" />
<input type="hidden" runat="server" id="hdnRegistrationID" value="" />
<dxcp:ASPxCallbackPanel ID="cbpViewTitipan" runat="server" Width="100%" ClientInstanceName="cbpViewTitipan"
    ShowLoadingPanel="false" OnCallback="cbpViewTitipan_Callback">
    <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ onCbpViewEndCallbackTitipan(s); }" />
    <PanelCollection>
        <dx:PanelContent ID="PanelContent1" runat="server">
            <asp:Panel runat="server" ID="pnlGridViewOrder" Style="width: 100%; margin-left: auto;
                margin-right: auto; position: relative; font-size: 0.95em; height: 365px; overflow-y: scroll;">
                <asp:ListView runat="server" ID="lvwViewTitipan" OnItemDataBound="lvwViewTitipan_ItemDataBound">
                    <EmptyDataTemplate>
                        <table id="tblView" runat="server" class="grdCollapsible lvwViewTitipan" cellspacing="0"
                            rules="all">
                            <tr>
                                <th style="width: 100px; text-align: left;">
                                    <%=GetLabel("No. Registrasi")%>
                                </th>
                                <th style="width: 100px; text-align: left;">
                                    <%=GetLabel("No. Rekam Medis")%>
                                </th>
                                <th style="width: 200px; text-align: left">
                                    <%=GetLabel("Pasien")%>
                                </th>
                                <th style="width: 100px; text-align: left">
                                    <%=GetLabel("Ruang")%>
                                </th>
                                <th style="width: 100px; text-align: left">
                                    <%=GetLabel("kamar")%>
                                </th>
                                <th style="width: 100px; text-align: left">
                                    <%=GetLabel("Kelas")%>
                                </th>
                                <th style="width: 100px; text-align: left">
                                    <%=GetLabel("Kelas Tagihan")%>
                                </th>
                                <th style="width: 50px; text-align: left">
                                    <%=GetLabel("Bed")%>
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
                        <table id="tblView" runat="server" class="grdCollapsible lvwViewTitipan" cellspacing="0"
                            rules="all">
                            <tr>
                                <th style="width: 100px; text-align: left;">
                                    <%=GetLabel("No. Registrasi")%>
                                </th>
                                <th style="width: 100px; text-align: left;">
                                    <%=GetLabel("No. Rekam Medis")%>
                                </th>
                                <th style="width: 200px; text-align: left">
                                    <%=GetLabel("Pasien")%>
                                </th>
                                <th style="width: 100px; text-align: left">
                                    <%=GetLabel("Ruang")%>
                                </th>
                                <th style="width: 100px; text-align: left">
                                    <%=GetLabel("kamar")%>
                                </th>
                                <th style="width: 100px; text-align: left">
                                    <%=GetLabel("Kelas")%>
                                </th>
                                <th style="width: 100px; text-align: left">
                                    <%=GetLabel("Kelas Tagihan")%>
                                </th>
                                <th style="width: 50px; text-align: left">
                                    <%=GetLabel("Bed")%>
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
                                        <%#: Eval("RegistrationNo")%></div>
                                    <input type="hidden" class="hdnVisitID" value='<%#: Eval("VisitID") %>' />
                                </div>
                            </td>
                            <td>
                                <div>
                                    <div style="float: left">
                                        <%#: Eval("MedicalNo")%></div>
                                </div>
                            </td>
                            <td>
                                <div>
                                    <div style="float: left">
                                        <%#: Eval("PatientName")%></div>
                                </div>
                            </td>
                            <td>
                                <div>
                                    <div style="float: left">
                                        <%#: Eval("ServiceUnitName")%></div>
                                </div>
                            </td>
                            <td>
                                <div>
                                    <div style="float: left">
                                        <%#: Eval("RoomName")%></div>
                                </div>
                            </td>
                            <td>
                                <div>
                                    <div style="float: left">
                                        <%#: Eval("ClassName")%></div>
                                </div>
                            </td>
                            <td>
                                <div>
                                    <div style="float: left">
                                        <%#: Eval("ChargesClassName")%></div>
                                </div>
                            </td>
                            <td>
                                <div>
                                    <div style="float: left">
                                        <%#: Eval("BedCode")%></div>
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
        <div id="pagingTitipan">
        </div>
    </div>
</div>
