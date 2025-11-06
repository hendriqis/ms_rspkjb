<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="GridEKlaimPatientCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.Finance.Program.GridEKlaimPatientCtl" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<script type="text/javascript" id="dxss_grideklaimctl">
    $('.lvwViewEKlaim > tbody > tr:gt(0):not(.trEmpty)').live('click', function () {
        showLoadingPanel();
        $('#<%=hdnRegistrationID.ClientID %>').val($(this).find('.RegistrationID').val());
        $('#<%=hdnNoSEP.ClientID %>').val($(this).find('.NoSEP').val());
        if ($('#<%=hdnNoSEP.ClientID %>').val() != null && $('#<%=hdnNoSEP.ClientID %>').val() != "") {
            __doPostBack('<%=btnOpenEKlaim.UniqueID%>', '');
        } else {
            var id = $(this).find('.RegistrationID').val();
            var url = ResolveUrl("~/Libs/Program/Module/PatientManagement/Registration/GenerateSEPManualCtl.ascx");
            openUserControlPopup(url, id, 'Ubah SEP Manual', 700, 300);
        }
    });

    //#region Paging
    var pageCount = parseInt('<%=PageCount %>');
    $(function () {
        setPaging($("#paging2"), pageCount, function (page) {
            cbpViewEKlaim.PerformCallback('changepage|' + page);
        });
    });

    function onCbpViewEKlaimEndCallback(s) {
        hideLoadingPanel();
        var param = s.cpResult.split('|');
        if (param[0] == 'refresh') {
            var pageCount = parseInt(param[1]);
            setPaging($("#paging2"), pageCount, function (page) {
                cbpViewEKlaim.PerformCallback('changepage|' + page);
            });
        }
    }
    //#endregion

    function refreshGrdEKlaimPatient() {
        cbpViewEKlaim.PerformCallback('refresh');
    }

    function onBeforeOpenEKlaim() {
        return ($('#<%=hdnRegistrationID.ClientID %>').val() != '');
    }
</script>
<div style="display: none">
    <asp:Button ID="btnOpenEKlaim" runat="server" UseSubmitBehavior="false" OnClientClick="return onBeforeOpenEKlaim();"
        OnClick="btnOpenEKlaim_Click" /></div>
<input type="hidden" runat="server" id="hdnRegistrationID" value="" />
<input type="hidden" runat="server" id="hdnNoSEP" value="" />
<dxcp:ASPxCallbackPanel ID="cbpViewEKlaim" runat="server" Width="100%" ClientInstanceName="cbpViewEKlaim"
    ShowLoadingPanel="false" OnCallback="cbpViewEKlaim_Callback">
    <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ onCbpViewEKlaimEndCallback(s); }" />
    <PanelCollection>
        <dx:PanelContent ID="PanelContent1" runat="server">
            <asp:Panel runat="server" ID="pnlGridViewEKlaim" Style="width: 100%; margin-left: auto;
                margin-right: auto; position: relative; font-size: 0.95em; height: 350px; overflow-y: scroll;">
                <asp:ListView runat="server" ID="lvwViewEKlaim" OnItemDataBound="lvwViewEKlaim_ItemDataBound">
                    <EmptyDataTemplate>
                        <table id="tblViewEKlaim" runat="server" class="grdView lvwViewEKlaim" cellspacing="0"
                            rules="all">
                            <tr>
                                <th style="width: 50px" align="left">
                                    <%=GetLabel("No. SEP")%>
                                </th>
                                <th style="width: 150px" align="left">
                                    <%=GetLabel("No. Registrasi")%>
                                </th>
                                <th style="width: 120px" align="left">
                                    <%=GetLabel("No. Kartu BPJS")%>
                                </th>
                                <th align="left">
                                    <%=GetLabel("Pasien")%>
                                </th>
                                <th style="width: 100px" align="center">
                                    <%=GetLabel("Tanggal Masuk")%>
                                </th>
                                <th style="width: 100px" align="center">
                                    <%=GetLabel("Tanggal SEP")%>
                                </th>
                                <th style="width: 100px" align="center">
                                    <%=GetLabel("Tanggal Pulang")%>
                                </th>
                                <th align="left">
                                    <%=GetLabel("Informasi Registrasi")%>
                                </th>
                            </tr>
                            <tr class="trEmpty">
                                <td colspan="15">
                                    <%=GetLabel("No data to display.")%>
                                </td>
                            </tr>
                        </table>
                    </EmptyDataTemplate>
                    <LayoutTemplate>
                        <table id="tblViewEKlaim" runat="server" class="grdCollapsible lvwViewEKlaim" cellspacing="0"
                            rules="all">
                            <tr>
                                <th style="width: 50px" align="left">
                                    <%=GetLabel("No. SEP")%>
                                </th>
                                <th style="width: 150px" align="left">
                                    <%=GetLabel("No. Registrasi")%>
                                </th>
                                <th style="width: 120px" align="left">
                                    <%=GetLabel("No. Kartu BPJS")%>
                                </th>
                                <th align="left">
                                    <%=GetLabel("Pasien")%>
                                </th>
                                <th style="width: 90px" align="center">
                                    <%=GetLabel("Tanggal Masuk")%>
                                </th>
                                <th style="width: 90px" align="center">
                                    <%=GetLabel("Tanggal SEP")%>
                                </th>
                                <th style="width: 90px" align="center">
                                    <%=GetLabel("Tanggal Pulang")%>
                                </th>
                                <th align="left">
                                    <%=GetLabel("Informasi Registrasi")%>
                                </th>
                                <th style="width: 30px" align="center">
                                    <%=GetLabel("Dx Klaim")%>
                                </th>
                                <th style="width: 30px" align="center">
                                    <%=GetLabel("Grouper-1")%>
                                </th>
                                <th style="width: 30px" align="center">
                                    <%=GetLabel("Final")%>
                                </th>
                                <th style="width: 30px" align="center">
                                    <%=GetLabel("Sent")%>
                                </th>
                            </tr>
                            <tr runat="server" id="itemPlaceholder">
                            </tr>
                        </table>
                    </LayoutTemplate>
                    <ItemTemplate>
                        <tr>
                            <td align="center">
                                <div>
                                    <input type="hidden" class="RegistrationID" value='<%#: Eval("RegistrationID") %>' />
                                    <input type="hidden" class="NoSEP" value='<%#: Eval("NoSEP") %>' />
                                    <input type="hidden" class="NoPeserta" value='<%#: Eval("NoPeserta") %>' />
                                    <input type="hidden" class="MedicalNo" value='<%#: Eval("MedicalNo") %>' />
                                    <input type="hidden" class="PatientName" value='<%#: Eval("PatientName") %>' />
                                    <b>
                                        <%#: Eval("NoSEP") %></b>
                                    <img class="imgSEP imgLink" title='<%=GetLabel("Tidak Ada No.SEP")%>' src=' <%# ResolveUrl("~/Libs/Images/Status/no_sep.png") %>'
                                        style='<%# Eval("NoSEP").ToString() == "" ? "width:25px": "display:none" %>'
                                        alt="" />
                                    <img title='<%=GetLabel("Pengantar Rawat Inap")%>' src=' <%# ResolveUrl("~/Libs/Images/Status/transfer_to_inpatient.png") %>'
                                        style='<%# Eval("IsTransferredToInpatient").ToString() == "True" ? "width:25px": "display:none" %>'
                                        alt="" />  
                                </div>
                            </td>
                            <td>
                                <div>
                                    <b>
                                        <%#: Eval("RegistrationNo") %></b>
                                </div>
                                <div>
                                    <i>
                                        <%#: Eval("RegistrationStatus") %></i>
                                </div>
                                <div>
                                    <hr style="margin: 0 0 0 0;" />
                                </div>
                                <div style='<%# Eval("LinkedToRegistrationNo").ToString() == "" ? "display:none": "" %>'>
                                    <label style="font-size: xx-small;">
                                        <%=GetLabel("LinkedTo = ")%><%#: Eval("LinkedToRegistrationNo") %></label>
                                </div>
                            </td>
                            <td>
                                <div>
                                    <i>
                                        <%#: Eval("NoPeserta") %></i>
                                </div>
                            </td>
                            <td>
                                <div>
                                    <label style="font-size: x-small; font-style: italic">
                                        <%=GetLabel("No.RM = ")%></label>
                                    <%#: Eval("MedicalNo") %>
                                </div>
                                <div style='<%# Eval("EKlaimMedicalNo").ToString() == "" ? "display:none": "" %>'>
                                    <label style="font-size: xx-small; font-style: italic">
                                        <%=GetLabel("No.RM E-Klaim = ")%></label>
                                    <%#: Eval("EKlaimMedicalNo") %>
                                </div>
                                <div>
                                    <b>
                                        <%#: Eval("PatientName") %></b>
                                </div>
                            </td>
                            <td align="center">
                                <div>
                                    <%#: Eval("cfRegistrationDateInString") %>
                                </div>
                            </td>
                            <td align="center">
                                <div>
                                    <%#: Eval("cfTanggalSEPInString") %>
                                </div>
                            </td>
                            <td align="center">
                                <div>
                                    <%#: Eval("cfDischargeDateInString") %>
                                </div>
                            </td>
                            <td>
                                <div>
                                    <i>
                                        <%#: Eval("DepartmentID") %></i>
                                </div>
                                <div>
                                    <b>
                                        <%#: Eval("ServiceUnitName") %></b>
                                </div>
                                <div>
                                    <i>
                                        <%#: Eval("ParamedicName") %></i>
                                </div>
                            </td>
                            <td align="center">
                                <div id="divHasDiagnoseClaim" runat="server" style="text-align: center; color: blue">
                                </div>
                            </td>
                            <td align="center">
                                <div id="divHasGrouperStage1" runat="server" style="text-align: center; color: blue">
                                </div>
                            </td>
                            <td align="center">
                                <div id="divHasFinalClaim" runat="server" style="text-align: center; color: blue">
                                </div>
                            </td>
                            <td align="center">
                                <div id="divHasSentClaim" runat="server" style="text-align: center; color: blue">
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
        <div id="paging2">
        </div>
    </div>
</div>
