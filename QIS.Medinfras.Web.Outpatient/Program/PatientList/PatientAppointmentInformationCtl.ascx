<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PatientAppointmentInformationCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.Outpatient.Program.PatientAppointmentInformationCtl" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<script type="text/javascript" id="dxss_appointmentInfo">
    //#region Paging
    var pageCountAvailable = parseInt('<%=PageCount %>');
    $(function () {
        setPagingDetailItem(pageCountAvailable);
    });

    function setPagingDetailItem(pageCount) {
        if (pageCount > 0)
            $('#<%=grdView.ClientID %> tr:eq(1)').click();
        setPaging($("#pagingPopup"), pageCount, function (page) {
            cbpViewCtl.PerformCallback('changepage|' + page);
        }, 8);
    }
    //#endregion 

    function oncbpViewCtlEndCallback(s) {
        $('#containerImgLoadingView').hide();
        var param = s.cpResult.split('|');
        if (param[0] == 'refresh') {
            var pageCount = parseInt(param[1]);
            if (pageCount > 0)
                $('#<%=grdView.ClientID %> tr:eq(1)').click();

            setPagingDetailItem($("#pagingPopup"), pageCount, function (page) {
                cbpView.PerformCallback('changepage|' + page);
            });
        }
        else
            $('#<%=grdView.ClientID %> tr:eq(1)').click();
    }
</script>
<input type="hidden" value="" id="hdnParam" runat="server" />
<input type="hidden" id="hdnFilterExpression" runat="server" value="" />
<input type="hidden" id="hdnregistrationDate" runat="server" value="" />
<input type="hidden" id="hdnhealthcareServiceUnitID" runat="server" value="" />
<input type="hidden" id="hdnphysicianID" runat="server" value="" />
<table style="width: 100%">
    <tr>
        <td valign="top">
            <div style="position: relative;">
                <table style="width: 50%">
                    <colgroup>
                        <col style="width: 30%" />
                    </colgroup>
                </table>
                <dxcp:ASPxCallbackPanel ID="cbpViewCtl" runat="server" Width="100%" ClientInstanceName="cbpViewCtl"
                    ShowLoadingPanel="false" OnCallback="cbpViewCtl_Callback">
                    <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingView').show(); }"
                        EndCallback="function(s,e){ oncbpViewCtlEndCallback(s); }" />
                    <ClientSideEvents BeginCallback="function(s,e){ $(&#39;#containerImgLoadingView&#39;).show(); }"
                        EndCallback="function(s,e){ oncbpViewCtlEndCallback(s); }"></ClientSideEvents>
                    <PanelCollection>
                        <dx:PanelContent ID="PanelContent2" runat="server">
                            <asp:Panel runat="server" ID="Panel1" CssClass="pnlContainerGridPatientPage">
                                <asp:GridView ID="grdView" runat="server" CssClass="grdSelected grdView" AutoGenerateColumns="false"
                                    ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                    <Columns>
                                        <asp:TemplateField ItemStyle-VerticalAlign="Top" HeaderStyle-Width="5%">
                                            <HeaderTemplate>
                                                <div style="text-align: center; padding-left: 3px">
                                                    <%=GetLabel("No. Antrian")%>
                                                </div>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <div style="text-align: center; padding: 3px">
                                                    <div>
                                                        <%#: Eval("QueueNo")%></div>
                                                </div>
                                            </ItemTemplate>
                                            <HeaderStyle Width="5%"></HeaderStyle>
                                            <ItemStyle VerticalAlign="Top"></ItemStyle>
                                          </asp:TemplateField>
                                          <asp:TemplateField ItemStyle-VerticalAlign="Top" HeaderStyle-Width="10%">
                                            <HeaderTemplate>
                                                <div style="text-align: center; padding-left: 3px">
                                                    <%=GetLabel("Infromasi Kunjungan")%>
                                                </div>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <div style="text-align: center; padding: 3px">
                                                    <div>
                                                        <%#: Eval("ServiceUnitName")%></div>
                                                </div>
                                            </ItemTemplate>
                                            <HeaderStyle Width="10%"></HeaderStyle>
                                            <ItemStyle VerticalAlign="Top"></ItemStyle>
                                        </asp:TemplateField>
                                       <asp:TemplateField ItemStyle-VerticalAlign="Top" HeaderStyle-Width="15%">
                                            <HeaderTemplate>
                                                <div style="text-align: center; padding-left: 3px">
                                                    <%=GetLabel("No. Appointment")%>
                                                </div>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <div style="text-align: center; padding: 3px">
                                                    <div>
                                                        <%#: Eval("AppointmentNo")%></div>
                                                </div>
                                            </ItemTemplate>
                                            <HeaderStyle Width="15%"></HeaderStyle>
                                            <ItemStyle VerticalAlign="Top"></ItemStyle>
                                        </asp:TemplateField>
                                        <asp:TemplateField ItemStyle-VerticalAlign="Top" HeaderStyle-Width="10%">
                                            <HeaderTemplate>
                                                <div style="text-align: center; padding-left: 3px">
                                                    <%=GetLabel("Tanggal Perjanjian")%>
                                                </div>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <div style="text-align: center; padding: 3px">
                                                    <div>
                                                        <%#: Eval("StartDateInString")%></div>
                                                </div>
                                            </ItemTemplate>
                                            <HeaderStyle Width="10%"></HeaderStyle>
                                            <ItemStyle VerticalAlign="Top"></ItemStyle>
                                        </asp:TemplateField>
                                        <asp:TemplateField ItemStyle-VerticalAlign="Top" HeaderStyle-Width="5%">
                                            <HeaderTemplate>
                                                <div style="text-align: center; padding-left: 3px">
                                                    <%=GetLabel("Waktu Perjanjian")%>
                                                </div>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <div style="text-align: center; padding: 3px">
                                                    <div>
                                                        <%#: Eval("StartTime")%></div>
                                                </div>
                                            </ItemTemplate>
                                            <HeaderStyle Width="5%"></HeaderStyle>
                                            <ItemStyle VerticalAlign="Top"></ItemStyle>
                                        </asp:TemplateField>
                                        <asp:TemplateField ItemStyle-VerticalAlign="Top" HeaderStyle-Width="10%">
                                            <HeaderTemplate>
                                                <div style="text-align: center; padding-left: 3px">
                                                    <%=GetLabel("No. RM")%>
                                                </div>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <div style="text-align: center; padding: 3px">
                                                    <div>
                                                        <%#: Eval("MedicalNo")%></div>
                                                </div>
                                            </ItemTemplate>
                                            <HeaderStyle Width="10%"></HeaderStyle>
                                            <ItemStyle VerticalAlign="Top"></ItemStyle>
                                        </asp:TemplateField>
                                        <asp:TemplateField ItemStyle-VerticalAlign="Top" HeaderStyle-Width="20%">
                                            <HeaderTemplate>
                                                <div style="text-align: center; padding-left: 3px">
                                                    <%=GetLabel("Informasi Pasien")%>
                                                </div>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <div style="padding: 3px">
                                                    <div>
                                                        <%#: Eval("cfPatientInformation")%></div>
                                                </div>
                                            </ItemTemplate>
                                            <HeaderStyle Width="20%"></HeaderStyle>
                                            <ItemStyle VerticalAlign="Top"></ItemStyle>
                                        </asp:TemplateField>
                                        <asp:TemplateField ItemStyle-VerticalAlign="Top" HeaderStyle-Width="20%">
                                            <HeaderTemplate>
                                                <div style="text-align: center; padding-left: 3px">
                                                    <%=GetLabel("Informasi Kontak")%>
                                                </div>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <div style="padding: 3px">
                                                    <div>
                                                        <%#: Eval("cfInformasiKontak")%></div>
                                                </div>
                                            </ItemTemplate>
                                            <HeaderStyle Width="20%"></HeaderStyle>
                                            <ItemStyle VerticalAlign="Top"></ItemStyle>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderStyle-Width="10%">
                                        <HeaderTemplate>
                                                <div style="text-align: center; padding-left: 3px">
                                                    <%=GetLabel("Pasien Baru")%>
                                                </div>
                                         </HeaderTemplate>
                                            <ItemTemplate>
                                                    <div style="padding: 3px; text-align: center;">
                                                        <asp:CheckBox ID="chkIsReviewed" runat="server" Checked='<%# Eval("IsNewPatient")%>'
                                                            Enabled="false" />
                                                    </div>
                                            </ItemTemplate>
                                            <HeaderStyle Width="10px"></HeaderStyle>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderStyle-Width="5%">
                                        <HeaderTemplate>
                                                <div style="text-align: left; padding-left: 3px">
                                                    <%=GetLabel("Registrasi")%>
                                                </div>
                                         </HeaderTemplate>
                                            <ItemTemplate>
                                                    <div style="padding: 3px; text-align: center;">
                                                        <asp:CheckBox ID="chkIsRegistration" runat="server" Checked='<%# Eval("cfPatientRegistration")%>'
                                                            Enabled="false" />
                                                    </div>
                                            </ItemTemplate>
                                            <HeaderStyle Width="50px"></HeaderStyle>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderStyle-Width="5%">
                                        <HeaderTemplate>
                                                <div style="text-align: left; padding-left: 3px">
                                                    <%=GetLabel("Batal")%>
                                                </div>
                                         </HeaderTemplate>
                                            <ItemTemplate>
                                                    <div style="padding: 3px; text-align: center;">
                                                        <asp:CheckBox ID="chkDeleteAppointment" runat="server" Checked='<%# Eval("cfDeleteAppointment")%>'
                                                            Enabled="false" />
                                                    </div>
                                            </ItemTemplate>
                                            <HeaderStyle Width="50px"></HeaderStyle>
                                        </asp:TemplateField>
                                    </Columns>
                                    <EmptyDataRowStyle CssClass="trEmpty"></EmptyDataRowStyle>
                                    <EmptyDataTemplate>
                                        <%=GetLabel("No Data To Display") %>
                                    </EmptyDataTemplate>
                                </asp:GridView>
                            </asp:Panel>
                        </dx:PanelContent>
                    </PanelCollection>
                </dxcp:ASPxCallbackPanel>
                <div class="imgLoadingGrdView" id="containerImgLoadingViewDt">
                    <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                </div>
                <div class="containerPaging">
                    <div class="wrapperPaging">
                        <div id="pagingPopup">
                        </div>
                    </div>
                </div>
            </div>
        </td>
    </tr>
</table>
