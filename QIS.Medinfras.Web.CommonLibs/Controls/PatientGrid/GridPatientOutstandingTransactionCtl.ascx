<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="GridPatientOutstandingTransactionCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Controls.GridPatientOutstandingTransactionCtl" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<script type="text/javascript" id="dxss_gridreigsteredpatientctlnew">

    //#region Paging
    var pageCount = parseInt('<%=PageCount %>');
    $(function () {
        var gender = $('.hdnPatientGender').val();
        Methods.checkImageError('imgPatientImage', 'patient', gender);
        setPaging($("#paging"), pageCount, function (page) {
            cbpView.PerformCallback('changepage|' + page);
        });
    });

    function onCbpViewEndCallback(s) {
        hideLoadingPanel();
        var gender = $('.hdnPatientGender').val();
        Methods.checkImageError('imgPatientImage', 'patient', gender);
        var param = s.cpResult.split('|');
        if (param[0] == 'refresh') {
            var pageCount = parseInt(param[1]);
            setPaging($("#paging"), pageCount, function (page) {
                cbpView.PerformCallback('changepage|' + page);
            });
        }
    }
    //#endregion

    function refreshGrdRegisteredPatient() {
        cbpView.PerformCallback('refresh');
    }

    $('.lnkDetail').live('click', function () {
        var Id = $(this).closest('tr').find('.hdnVisitID').val();
        var RegNo = $(this).closest('tr').find('.hdnRegistrationNo').val();
        var MedNo = $(this).closest('tr').find('.hdnMedicalNo').val();
        var PatName = $(this).closest('tr').find('.hdnPatientName').val();
        var param = Id + '|' + RegNo + '|' + MedNo + '|' + PatName;
        var url = ResolveUrl("~/Libs/Program/Information/InformationOrderOutstandingCtl.ascx");
        openUserControlPopup(url, param, 'Outstanding Order Detail', 900, 500);
    });

</script>
<dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
    ShowLoadingPanel="false" OnCallback="cbpView_Callback">
    <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ onCbpViewEndCallback(s); }" />
    <PanelCollection>
        <dx:PanelContent ID="PanelContent1" runat="server">
            <asp:Panel runat="server" ID="pnlGridView" Style="width: 100%; margin-left: auto;
                margin-right: auto; position: relative; font-size: 0.95em; height: 365px; overflow-y: scroll;">
                <asp:ListView runat="server" ID="lvwView">
                    <EmptyDataTemplate>
                        <table id="tblView" runat="server" class="grdCollapsible lvwView" cellspacing="0"
                            rules="all">
                            <tr>
                                <th rowspan="2" style="width: 220px; text-align: left">
                                    <div>
                                        <%=GetLabel("DATA KUNJUNGAN")%></div>
                                </th>
                                <th rowspan="2" style="width: 250px; text-align: left">
                                    <div>
                                        <%=GetLabel("DATA PASIEN")%></div>
                                </th>
                                <th colspan="2" style="width: 50px; text-align: center">
                                    <div>
                                        <%=GetLabel("TRANSAKSI")%></div>
                                </th>
                                <th colspan="2" style="width: 50px; text-align: center">
                                    <div>
                                        <%=GetLabel("OUTSTANDING")%></div>
                                </th>
                                <th rowspan="2" style="width: 50px; text-align: right">
                                    <div>
                                        <%=GetLabel("SISA TAGIHAN")%></div>
                                </th>
                            </tr>
                            <tr>
                                <th style="width: 50px; text-align: right">
                                    <div>
                                        <%=GetLabel("SUDAH DIPROSES")%></div>
                                </th>
                                <th style="width: 50px; text-align: right">
                                    <div>
                                        <%=GetLabel("OPEN")%></div>
                                </th>
                                <th style="width: 50px; text-align: right">
                                    <div>
                                        <%=GetLabel("UANG MUKA")%></div>
                                </th>
                                <th style="width: 50px; text-align: right">
                                    <div>
                                        <%=GetLabel("COVERAGE")%></div>
                                </th>
                            </tr>
                            <tr class="trEmpty">
                                <td colspan="8">
                                    <%=GetLabel("Tidak ada pasien yang sedang dalam perawatan")%>
                                </td>
                            </tr>
                        </table>
                    </EmptyDataTemplate>
                    <LayoutTemplate>
                        <table id="tblView" runat="server" class="grdCollapsible lvwView" cellspacing="0"
                            rules="all">
                            <tr>
                                <th rowspan="2" style="width: 130px; text-align: left">
                                    <div>
                                        <%=GetLabel("DATA KUNJUNGAN")%></div>
                                </th>
                                <th rowspan="2" style="width: 250px; text-align: left">
                                    <div>
                                        <%=GetLabel("DATA PASIEN")%></div>
                                </th>
                                <th colspan="2" style="width: 50px; text-align: center">
                                    <div>
                                        <%=GetLabel("TRANSAKSI")%></div>
                                </th>
                                <th colspan="2" style="width: 50px; text-align: center">
                                    <div>
                                        <%=GetLabel("PEMBAYARAN")%></div>
                                </th>
                                <th rowspan="2" style="width: 50px; text-align: right">
                                    <div>
                                        <%=GetLabel("OUTSTANDING")%></div>
                                </th>
                            </tr>
                            <tr>
                                <th style="width: 50px; text-align: right">
                                    <div>
                                        <%=GetLabel("TOTAL TAGIHAN")%></div>
                                </th>
                                <th style="width: 50px; text-align: right">
                                    <div>
                                        <%=GetLabel("DISKON")%></div>
                                </th>
                                <th style="width: 50px; text-align: right">
                                    <div>
                                        <%=GetLabel("UANG MUKA")%></div>
                                </th>
                                <th style="width: 50px; text-align: right">
                                    <div>
                                        <%=GetLabel("PEMBAYARAN")%></div>
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
                                    <b>
                                        <%#: Eval("RegistrationNo") %></b>
                                </div>
                                <div style="float: left">
                                    <%#: Eval("ServiceUnitName")%>,
                                    <%#: Eval("BedCode")%>
                                </div>
                                <br />
<%--                                <div>
                                    <%#: Eval("ParamedicName")%>
                                </div>--%>
                                <div>
                                    <%#: Eval("BusinessPartnerName")%>
                                </div>
                                <input type="hidden" class="hdnVisitID" value='<%#: Eval("VisitID") %>' />
                                <input type="hidden" class="hdnRegistrationID" value='<%#: Eval("RegistrationID") %>' />
                                <input type="hidden" class="hdnRegistrationNo" value='<%#: Eval("RegistrationNo") %>' />
                                <input type="hidden" class="hdnMedicalNo" value='<%#: Eval("MedicalNo") %>' />
                                <input type="hidden" class="hdnPatientName" value='<%#: Eval("PatientName") %>' />
                                <input type="hidden" class="hdnPatientGender" value='<%#: Eval("GCGender")%>' />
                            </td>
                            <td>
                                <div style="padding: 3px">
                                    <img class="imgPatientImage" src='<%#Eval("PatientImageUrl") %>' alt="" height="55px"
                                        width="40px" style="float: left; margin-right: 10px;" />
                                    <div>
                                        <b>
                                            <%#: Eval("PatientName") %>
                                        </b>
                                    </div>
                                    <table cellpadding="0" cellspacing="0">
                                        <colgroup>
                                            <col style="width: 60px" />
                                            <col style="width: 10px" />
                                            <col style="width: 100px" />
                                            <col style="width: 60px" />
                                            <col style="width: 10px" />
                                            <col style="width: 140px" />
                                        </colgroup>
                                        <tr>
                                            <td style="text-align: right; font-size: 0.9em; font-style: italic">
                                                <%=GetLabel("Nama Panggilan")%>
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td>
                                                <%#: Eval("PatientName")%>
                                            </td>
                                            <td style="text-align: right; font-size: 0.9em; font-style: italic">
                                                <%=GetLabel("No RM")%>
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td>
                                                <%#: Eval("MedicalNo")%>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="text-align: right; font-size: 0.9em; font-style: italic">
                                                <%=GetLabel("Tanggal Lahir")%>
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td>
                                                <%#: Eval("cfDateOfBirthInString")%>
                                            </td>
                                            <td style="text-align: right; font-size: 0.9em; font-style: italic">
                                                <%=GetLabel("Umur")%>
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td>
                                                <%#: Eval("cfPatientAge")%>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </td>
                            <td>
                                <div style="text-align: right">
                                    <%#: Eval("cfTotalTagihanInString")%></div>
                            </td>
                            <td>
                                <div style="text-align: right">
                                    <%#: Eval("cfDiscountAmountInString")%></div>
                            </td>
                            <td>
                                <div style="text-align: right">
                                    <%#: Eval("cfDownPaymentAmountInString")%></div>
                            </td>
                            <td>
                                <div style="text-align: right">
                                    <%#: Eval("cfPaymentAmountInString")%></div>
                            </td>
                            <td>
                                <div style="text-align: right">
                                    <%#: Eval("cfOutstandingAmountInString")%>
                                </div>
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:ListView>
            </asp:Panel>
        </dx:PanelContent>
    </PanelCollection>
</dxcp:ASPxCallbackPanel>
<div class="imgLoadingGrdView" id="containerImgLoadingView">
    <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
</div>
<div class="containerPaging">
    <div class="wrapperPaging">
        <div id="paging">
        </div>
    </div>
</div>
