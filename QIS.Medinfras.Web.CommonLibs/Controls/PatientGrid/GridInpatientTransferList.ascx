<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="GridInpatientTransferList.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Controls.GridInpatientTransferList" %>
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
                                <th rowspan="1" style="width: 13%; text-align: left">
                                    <div>
                                        <%=GetLabel("BED")%></div>
                                </th>
                                <th rowspan="1" style="width: 100px; text-align: left">
                                    <div>
                                        <%=GetLabel("STATUS")%></div>
                                </th>
                                <th rowspan="1" style="width: 150px; text-align: left">
                                    <div>
                                        <%=GetLabel("KELUAR")%></div>
                                </th>
                                <th colspan="1" style="width: 100px; text-align: center">
                                    <div>
                                        <%=GetLabel("MASUK")%></div>
                                </th>
                            </tr>
                            <tr class="trEmpty">
                                <td colspan="8">
                                    <%=GetLabel("Tidak ada informasi pasien pindah")%>
                                </td>
                            </tr>
                        </table>
                    </EmptyDataTemplate>
                    <LayoutTemplate>
                        <table id="tblView" runat="server" class="grdCollapsible lvwView" cellspacing="0"
                            rules="all">
                            <tr>
                                <th rowspan="1" style="width: 13%; text-align: left">
                                    <div>
                                        <%=GetLabel("BED")%></div>
                                </th>
                                <th rowspan="1" style="width: 100px; text-align: left">
                                    <div>
                                        <%=GetLabel("STATUS")%></div>
                                </th>
                                <th rowspan="1" style="width: 150px; text-align: left">
                                    <div>
                                        <%=GetLabel("KELUAR")%></div>
                                </th>
                                <th colspan="1" style="width: 100px; text-align: center">
                                    <div>
                                        <%=GetLabel("MASUK")%></div>
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
                                        <%#: Eval("BedCode") %></b>
                                </div>
                                <input type="hidden" class="hdnVisitID" value='<%#: Eval("VisitID") %>' />
                                <input type="hidden" class="hdnRegistrationNo" value='<%#: Eval("RegistrationNo") %>' />
                                <input type="hidden" class="hdnMedicalNo" value='<%#: Eval("MedicalNo") %>' />
                                <input type="hidden" class="hdnPatientName" value='<%#: Eval("PatientName") %>' />
                                <input type="hidden" class="hdnGender" value='<%#: Eval("Gender")%>' />
                            </td>
                            <td>
                                <div>
                                    <b>
                                        <%#: Eval("BedStatus") %>
                                    </b>
                                </div>
                            </td>
                            <td>
                                <div>
                                    <%#: Eval("cfPatientInformation")%></div>
                            </td>
                            <td>
                                
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
