<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="GridPatientMedRecClaimListCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Controls.GridPatientMedRecClaimListCtl" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<script type="text/javascript" id="dxss_GridPatientMedRecClaimListCtl">
    $('.lvwView2 > tbody > tr:gt(0):not(.trEmpty)').live('click', function () {
        showLoadingPanel();
        $('#<%=hdnVisitID.ClientID %>').val($(this).find('.VisitID').val());
        __doPostBack('<%=btnOpenTransactionDt2.UniqueID%>', '');
    });

    //#region Paging2
    var pageCountRegOrder2 = parseInt('<%=PageCount %>');
    $(function () {
        setPaging($("#paging2"), pageCountRegOrder2, function (page) {
            cbpView2.PerformCallback('changepage|' + page);
        });

        $('#containerDiagnosa').hide();
    });

    function oncbpView2EndCallback(s) {
        hideLoadingPanel();
        var param = s.cpResult.split('|');
        if (param[0] == 'refresh') {
            var pageCount = parseInt(param[1]);
            setPaging($("#paging2"), pageCount, function (page) {
                cbpView2.PerformCallback('changepage|' + page);
            });
        }
    }
    //#endregion

    function refreshGrdRegisteredPatient() {
        cbpView2.PerformCallback('refresh');
    }

    function onBeforeOpenTransactionDt2() {
        return ($('#<%=hdnVisitID.ClientID %>').val() != '');
    }
</script>
<div style="display: none">
    <asp:Button ID="btnOpenTransactionDt2" runat="server" UseSubmitBehavior="false" OnClientClick="return onBeforeOpenTransactionDt2();"
        OnClick="btnOpenTransactionDt2_Click" /></div>
<input type="hidden" runat="server" id="hdnVisitID" value="" />
<dxcp:ASPxCallbackPanel ID="cbpView2" runat="server" Width="100%" ClientInstanceName="cbpView2"
    ShowLoadingPanel="false" OnCallback="cbpView2_Callback">
    <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ oncbpView2EndCallback(s); }" />
    <PanelCollection>
        <dx:PanelContent ID="PanelContent1" runat="server">
            <asp:Panel runat="server" ID="pnlGridView" Style="width: 100%; margin-left: auto;
                margin-right: auto; position: relative; font-size: 0.95em; height: 450px; overflow-y: scroll;">
                <asp:ListView runat="server" ID="lvwViewCount">
                    <EmptyDataTemplate>
                        <table id="tblViewCount" runat="server" cellspacing="0" class="lvwViewCount" rules="all"
                            width="150px" style="float: right; margin-right: 10px">
                            <tr>
                                <th style="width: 150px; margin-right: 5px" align="right">
                                    <%=GetLabel("Jumlah Transaksi")%>
                                </th>
                            </tr>
                            <tr class="trEmpty">
                                <td style="width: 150px; margin-right: 5px; text-align: right">
                                    <%=GetLabel("0")%>
                                </td>
                            </tr>
                        </table>
                    </EmptyDataTemplate>
                    <LayoutTemplate>
                        <table id="tblViewCount" runat="server" cellspacing="0" class="lvwViewCount" rules="all"
                            width="150px" style="float: right; margin-right: 10px">
                            <tr>
                                <th style="width: 150px; margin-right: 5px" align="right">
                                    <%=GetLabel("Jumlah Transaksi")%>
                                </th>
                            </tr>
                            <tr runat="server" id="itemPlaceholder">
                            </tr>
                        </table>
                    </LayoutTemplate>
                    <ItemTemplate>
                        <tr>
                            <td style="width: 150px; text-align: right">
                                <div>
                                    <%#: Eval("TotalRow") %></span>
                                </div>
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:ListView>
                <asp:ListView runat="server" ID="lvwView2" OnItemDataBound="lvwView2_ItemDataBound">
                    <EmptyDataTemplate>
                        <table id="tblView" runat="server" class="grdView lvwView2" cellspacing="0" rules="all">
                            <tr>
                                <th style="width: 100px" align="left">
                                    <%=GetLabel("No RM")%>
                                </th>
                                <th align="left">
                                    <%=GetLabel("Pasien")%>
                                </th>
                                <th style="width: 150px" align="left">
                                    <%=GetLabel("No SEP")%>
                                </th>
                                <th style="width: 150px" align="left">
                                    <%=GetLabel("No Registrasi")%>
                                </th>
                                <th style="width: 100px" align="center">
                                    <%=GetLabel("Tgl Registrasi")%>
                                </th>
                                <th style="width: 250px" align="left">
                                    <%=GetLabel("Unit")%>
                                </th>
                                <th align="center">
                                    <%=GetLabel("P-Dx (ID)")%>
                                </th>
                                <th style="width: 120px" align="right">
                                    <%=GetLabel("Total Transaksi")%>
                                </th>
                            </tr>
                            <tr class="trEmpty">
                                <td colspan="10">
                                    <%=GetLabel("Tidak ada informasi pendaftaran pasien pada tanggal yang dipilih")%>
                                </td>
                            </tr>
                        </table>
                    </EmptyDataTemplate>
                    <LayoutTemplate>
                        <table id="tblView" runat="server" class="grdView lvwView2" cellspacing="0" rules="all">
                            <tr>
                                <th style="width: 100px" align="left">
                                    <%=GetLabel("No RM")%>
                                </th>
                                <th align="left">
                                    <%=GetLabel("Pasien")%>
                                </th>
                                <th style="width: 150px" align="left">
                                    <%=GetLabel("No SEP")%>
                                </th>
                                <th style="width: 150px" align="left">
                                    <%=GetLabel("No Registrasi")%>
                                </th>
                                <th style="width: 100px" align="center">
                                    <%=GetLabel("Tgl Registrasi")%>
                                </th>
                                <th style="width: 250px" align="left">
                                    <%=GetLabel("Unit")%>
                                </th>
                                <th align="center">
                                    <%=GetLabel("P-Dx (ID)")%>
                                </th>
                                <th style="width: 120px" align="right">
                                    <%=GetLabel("Total Transaksi")%>
                                </th>
                            </tr>
                            <tr runat="server" id="itemPlaceholder">
                            </tr>
                        </table>
                    </LayoutTemplate>
                    <ItemTemplate>
                        <tr>
                            <td>
                                <%#: Eval("MedicalNo") %>
                            </td>
                            <td>
                                <%#: Eval("PatientName") %>
                            </td>
                            <td>
                                <b>
                                    <%#: Eval("NoSEP") %></b>
                            </td>
                            <td>
                                <input type="hidden" class="VisitID" value='<%#: Eval("VisitID") %>' />
                                <%#: Eval("RegistrationNo") %>
                            </td>
                            <td align="center">
                                <%#: Eval("cfRegistrationDateInString") %>
                            </td>
                            <td>
                                <%#: Eval("ServiceUnitName") %>
                            </td>
                            <td align="center">
                                <div id="divDiagnosis" runat="server" style="text-align: center; color: blue">
                                </div>
                            </td>
                            <td align="right">
                                <%#: Eval("cfTotalAmountInString") %>
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
        <div id="paging2">
        </div>
    </div>
</div>
