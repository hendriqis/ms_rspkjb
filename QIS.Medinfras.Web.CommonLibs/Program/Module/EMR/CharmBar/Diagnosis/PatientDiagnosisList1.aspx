<%@ Page Language="C#" MasterPageFile="~/Libs/MasterPage/MPFrame.Master" AutoEventWireup="true"
    CodeBehind="PatientDiagnosisList1.aspx.cs" Inherits="QIS.Medinfras.Web.CommonLibs.Program.PatientDiagnosisList1"
    EnableViewState="false" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="plhMPFrame" runat="server">
    <script type="text/javascript">
        function onRefreshControl(filterExpression) {
            cbpPatientDiagnosisNowView.PerformCallback('refresh');
        }

        function oncbpPatientDiagnosisNowViewEndCallback(s) {
            $('#containerImgLoadingView').hide();

            var param = s.cpResult.split('|');
            $('#<%=grdPatientDiagnosisNowView.ClientID %> tr:eq(1)').click();
        }
        //#endregion

        $(function () {
            $('.lblNoOfVisitFinal.lblLink').live('click', function () {
                $tr = $(this).closest('tr').closest('tr');
                var finaldiagnoseID = $tr.find('.keyField').html();
                var finaldiagnoseName = $tr.find('.FinalDiagnosisText').html();
                var url = ResolveUrl("~/libs/Program/Module/EMR/CharmBar/Diagnosis/PatientDiagnosisFinalListDtCtl.ascx");
                openUserControlPopup(url, finaldiagnoseID + '|' + finaldiagnoseName, 'Detail Kunjungan Pasien dengan Diagnosa Rekam Medis', 1100, 350);
            });

            $('.lblNoOfVisitClaim.lblLink').live('click', function () {
                $tr = $(this).closest('tr').closest('tr');
                var claimdiagnoseID = $tr.find('.keyField').html();
                var claimdiagnoseName = $tr.find('.ClaimDiagnosisText').html();
                var url = ResolveUrl("~/libs/Program/Module/EMR/CharmBar/Diagnosis/PatientDiagnosisClaimListDtCtl.ascx");
                openUserControlPopup(url, claimdiagnoseID + '|' + claimdiagnoseName, 'Detail Kunjungan Pasien dengan Diagnosa Klaim', 1100, 350);
            });
        });
    </script>
    <table style="width: 100%; margin-top: 10px;" cellpadding="0" cellspacing="0">
        <colgroup>
            <col style="width: 60%" />
            <col style="width: 40%" />
            <col />
        </colgroup>
        <tr style="height: 600px">
            <td valign="top">
                <div style="text-align: center; font-size: xx-large; font-weight: bolder; background-color: Lime;
                    color: Maroon">
                    <%=GetLabel("Diagnosa Kunjungan/Perawatan Saat Ini")%></div>
                <div style="position: relative;">
                    <dxcp:ASPxCallbackPanel ID="cbpPatientDiagnosisNowView" runat="server" Width="100%"
                        Height="600px" ClientInstanceName="cbpPatientDiagnosisNowView" ShowLoadingPanel="false"
                        OnCallback="cbpPatientDiagnosisNowView_Callback">
                        <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingView').show(); }"
                            EndCallback="function(s,e){ oncbpPatientDiagnosisNowViewEndCallback(s); }" />
                        <PanelCollection>
                            <dx:PanelContent ID="PanelDiagnosisViewContent1" runat="server">
                                <asp:Panel runat="server" ID="pnlView" CssClass="pnlContainerGrid" Height="600px">
                                    <asp:GridView ID="grdPatientDiagnosisNowView" runat="server" CssClass="grdSelected grdPatientPage"
                                        AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                        <Columns>
                                            <asp:BoundField DataField="ID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="30%">
                                                <HeaderTemplate>
                                                    <%=GetLabel("Diagnosa oleh Dokter")%>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <div>
                                                        <%#: Eval("cfDifferentialDateInString")%>,
                                                        <%#: Eval("DifferentialTime")%></div>
                                                    <div>
                                                        <div>
                                                            <span style="color: Blue; font-size: 1.1em">
                                                                <%#: Eval("cfDiagnosisText")%></span> (<b><%#: Eval("DiagnoseID")%></b>)
                                                        </div>
                                                        <div style="font-style: italic">
                                                            <%#: Eval("cfMorphologyInfo")%></div>
                                                        <div>
                                                            <%#: Eval("ICDBlockName")%></div>
                                                        <div>
                                                            <b>
                                                                <%#: Eval("DiagnoseType")%></b> -
                                                            <%#: Eval("DifferentialStatus")%></div>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="35%">
                                                <HeaderTemplate>
                                                    <%=GetLabel("Diagnosa oleh Rekam Medis")%>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <div style='<%# Eval("cfFinalDate") == "" ? "display:none;": "" %>'>
                                                        <div>
                                                            <%#: Eval("cfFinalDate")%>,
                                                            <%#: Eval("FinalTime")%></div>
                                                        <div>
                                                            <span style="color: Blue; font-size: 1.1em">
                                                                <%#: Eval("cfFinalDiagnosisText")%></span> (<b><%#: Eval("FinalDiagnosisID")%></b>)
                                                        </div>
                                                        <div>
                                                            <%#: Eval("cfMorphologyInfo")%></div>
                                                        <div>
                                                            <%#: Eval("FinalICDBlockName")%></div>
                                                        <div>
                                                            <b>
                                                                <%#: Eval("DiagnoseType")%></b> -
                                                            <%#: Eval("FinalStatus")%></div>
                                                    </div>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderStyle-Width="35%">
                                                <HeaderTemplate>
                                                    <%=GetLabel("Diagnosa Klaim")%>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <table>
                                                        <tr>
                                                            <td>
                                                                <div style='<%#: Eval("cfClaimDiagnosisText") == "" ?  "display:none;":""   %>'>
                                                                    <div style='<%# Eval("cfClaimDiagnosisDate") == "" ? "display:none;": "" %>'>
                                                                        <div>
                                                                            <%#: Eval("cfClaimDiagnosisDate")%>,
                                                                            <%#: Eval("ClaimDiagnosisTime")%></div>
                                                                        <div>
                                                                            <span style="color: Blue; font-size: 1.1em">
                                                                                <%#: Eval("cfClaimDiagnosisText")%></span> (<b><%#: Eval("ClaimDiagnosisID")%></b>)
                                                                        </div>
                                                                        <div>
                                                                            <%#: Eval("cfMorphologyInfo")%></div>
                                                                        <div>
                                                                            <%#: Eval("ClaimICDBlockName")%></div>
                                                                        <div>
                                                                            <b>
                                                                                <%#: Eval("DiagnoseTypeClaim")%></b></div>
                                                                        <div>
                                                                            <label>
                                                                                <%=GetLabel("----------------------------------------------------------")%>
                                                                            </label>
                                                                        </div>
                                                                        <div>
                                                                            <label style="font-style: italic">
                                                                                E-Klaim Diagnosa V5
                                                                            </label>
                                                                            <div>
                                                                                <b>
                                                                                    <%#: Eval("cfClaimINACBGLabelName")%></b></div>
                                                                        </div>
                                                                        <div>
                                                                            <label style="font-style: italic">
                                                                                E-Klaim Diagnosa V6
                                                                            </label>
                                                                            <div>
                                                                                <b>
                                                                                    <%#: Eval("cfINACBGINAName")%></b></div>
                                                                        </div>
                                                                    </div>
                                                                </div>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <EmptyDataTemplate>
                                            <%=GetLabel("Tidak ada informasi diagnosa untuk pasien ini")%>
                                        </EmptyDataTemplate>
                                    </asp:GridView>
                                </asp:Panel>
                            </dx:PanelContent>
                        </PanelCollection>
                    </dxcp:ASPxCallbackPanel>
                    <div class="imgLoadingGrdView" id="containerImgLoadingView">
                        <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                    </div>
                </div>
            </td>
            <td valign="top" style="padding-left: 20px; padding-right: 5px;">
                <div style="position: relative; text-align: center; font-size: larger; font-weight: bolder;
                    background-color: Navy; color: White">
                    <%=GetLabel("Riwayat Diagnosa (Rekam Medis)")%></div>
                <div style="position: relative;">
                    <dxcp:ASPxCallbackPanel ID="cbpDiagnosisSummaryFinalView" runat="server" Width="100%"
                        Height="250px" ClientInstanceName="cbpDiagnosisSummaryFinalView" ShowLoadingPanel="false">
                        <PanelCollection>
                            <dx:PanelContent ID="PanelDiagnosisSummaryFinalView" runat="server">
                                <asp:Panel runat="server" ID="Panel1" CssClass="pnlContainerGrid">
                                    <asp:GridView ID="grdDiagnosisSummaryFinalView" runat="server" CssClass="grdSelected grdPatientPage"
                                        AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                        <Columns>
                                            <asp:BoundField DataField="FinalDiagnosisID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                            <asp:BoundField DataField="FinalDiagnosisText" HeaderStyle-CssClass="FinalDiagnosisText"
                                                ItemStyle-CssClass="FinalDiagnosisText" HeaderText="Diagnosa" HeaderStyle-HorizontalAlign="Left" />
                                            <asp:BoundField DataField="FinalDiagnosisID" HeaderStyle-CssClass="FinalDiagnosisID"
                                                ItemStyle-CssClass="FinalDiagnosisID" HeaderText="Kode" HeaderStyle-HorizontalAlign="Left" />
                                            <asp:TemplateField HeaderText="Jumlah Kunjungan" ItemStyle-HorizontalAlign="Center"
                                                HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="70px" HeaderStyle-CssClass="noOfVisit"
                                                ItemStyle-CssClass="noOfVisit">
                                                <ItemTemplate>
                                                    <label class="lblNoOfVisitFinal lblLink">
                                                        <%#:Eval("cfNoOfVisit", "{0:N}")%></label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <EmptyDataTemplate>
                                            <%=GetLabel("Tidak ada informasi diagnosa untuk pasien ini")%>
                                        </EmptyDataTemplate>
                                    </asp:GridView>
                                </asp:Panel>
                            </dx:PanelContent>
                        </PanelCollection>
                    </dxcp:ASPxCallbackPanel>
                </div>
                <div style="position: relative; text-align: center; font-size: larger; font-weight: bolder;
                    background-color: Maroon; color: White">
                    <%=GetLabel("Riwayat Diagnosa (Klaim)")%></div>
                <div style="position: relative;">
                    <dxcp:ASPxCallbackPanel ID="cbpDiagnosisSummaryClaimView" runat="server" Width="100%"
                        Height="250px" ClientInstanceName="cbpDiagnosisSummaryClaimView" ShowLoadingPanel="false">
                        <PanelCollection>
                            <dx:PanelContent ID="PanelDiagnosisSummaryClaimView" runat="server">
                                <asp:Panel runat="server" ID="Panel2" CssClass="pnlContainerGrid">
                                    <asp:GridView ID="grdDiagnosisSummaryClaimView" runat="server" CssClass="grdSelected grdPatientPage"
                                        AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                        <Columns>
                                            <asp:BoundField DataField="ClaimDiagnosisID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                            <asp:BoundField DataField="ClaimDiagnosisText" HeaderStyle-CssClass="ClaimDiagnosisText"
                                                ItemStyle-CssClass="ClaimDiagnosisText" HeaderText="Diagnosa" HeaderStyle-HorizontalAlign="Left" />
                                            <asp:BoundField DataField="ClaimDiagnosisID" HeaderStyle-CssClass="ClaimDiagnosisID"
                                                ItemStyle-CssClass="ClaimdiagnosisID" HeaderText="Kode" HeaderStyle-HorizontalAlign="Left" />
                                            <asp:TemplateField HeaderText="Jumlah Kunjungan" ItemStyle-HorizontalAlign="Center"
                                                HeaderStyle-HorizontalAlign="Center" HeaderStyle-Width="70px" HeaderStyle-CssClass="noOfVisit"
                                                ItemStyle-CssClass="noOfVisit">
                                                <ItemTemplate>
                                                    <label class="lblNoOfVisitClaim lblLink">
                                                        <%#:Eval("cfNoOfVisit", "{0:N}")%></label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <EmptyDataTemplate>
                                            <%=GetLabel("Tidak ada informasi diagnosa untuk pasien ini")%>
                                        </EmptyDataTemplate>
                                    </asp:GridView>
                                </asp:Panel>
                            </dx:PanelContent>
                        </PanelCollection>
                    </dxcp:ASPxCallbackPanel>
                </div>
            </td>
        </tr>
    </table>
</asp:Content>
