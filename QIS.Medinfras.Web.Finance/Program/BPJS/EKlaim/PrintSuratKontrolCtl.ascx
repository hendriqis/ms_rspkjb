<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PrintSuratKontrolCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.Finance.Program.PrintSuratKontrolCtl" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<script type="text/javascript" id="dxss_patientvisitnotesctl">
    function onAfterSaveAddRecordEntryPopup() {
        pcRightPanelContent.Hide();
    }

    $('.imgPrint.lblLink').die('click');
    $('.imgPrint.lblLink').live('click', function () {
        $row = $(this).closest('tr');
        var entity = rowToObject($row);
        var registrationID = $row.find('.hdnRegistrationID').val();
        var reportCode = "PM-00200";
        var filterExpression = registrationID;
        openReportViewer(reportCode, filterExpression);
    });

</script>
    <input type="hidden" runat="server" id="hdnRegistrationIDCtl" value="" />
    <input type="hidden" runat="server" id="hdnNoPeserta" value="" />
    <input type="hidden" runat="server" id="hdnMedicalNo" value="" />
    <input type="hidden" runat="server" id="hdnMRN" value="" />
    <input type="hidden" runat="server" id="hdnPatientName" value="" />
    <input type="hidden" runat="server" id="hdnDOB" value="" />
    <input type="hidden" runat="server" id="hdnGender" value="" />
    <input type="hidden" runat="server" id="hdnoldNoSep" value = "" />
  
<div style="height: 100%; overflow-y: auto;">
    <table class="tblContentArea">
        <tr>
            <td style="padding: 5px; vertical-align: top">
                <table class="tblEntryContent" style="width: 400px">
                    <colgroup>
                            <col style="width: 100px" />
                            <col style="width: 300px" />
                        </colgroup>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("No. RM")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtMRN" Width="160px" runat="server" />
                            </td>
                        </tr>
                        <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Nama Pasien")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtPatientName" Width="100%" runat="server" />
                        </td>
                    </tr>
                </table>

                <dxcp:ASPxCallbackPanel ID="cbpPatientVisitNotes" runat="server" Width="100%" ClientInstanceName="cbpPatientVisitNotes"
                    ShowLoadingPanel="false" OnCallback="cbpPatientVisitNotes_Callback">
                    <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ onPatientVisitNotesEndCallback(s); }" />
                <PanelCollection>
                    <dx:PanelContent ID="PanelContent1" runat="server">
                        <asp:Panel runat="server" ID="pnlPrintSuratKontrolGrdView" Style="width: 100%;
                            margin-left: auto; margin-right: auto; position: relative; font-size: 0.95em;">
                            <asp:GridView ID="grdView" runat="server" CssClass="grdView notAllowSelect"
                                AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                <Columns>
                                    <asp:TemplateField HeaderStyle-Width="8px" ItemStyle-HorizontalAlign="Center">
                                        <ItemTemplate>
                                            <img class="imgPrint lblLink" title='<%=GetLabel("Print")%>' src='<%# ResolveUrl("~/Libs/Images/Button/print.png")%>'
                                                alt="" style="float: center" />
                                            <input type="hidden" value="<%#:Eval("RegistrationID") %>" bindingfield="RegistrationID" />
                                            <input type="hidden" value="<%#:Eval("RegistrationNo") %>" bindingfield="RegistrationNo" />
                                            <input type="hidden" value="<%#:Eval("RegistrationDateInString") %>" bindingfield="RegistrationDateInString" />
                                            <input type="hidden" value="<%#:Eval("NoSuratRencanaKontrolBerikutnya") %>" bindingfield="NoSuratRencanaKontrolBerikutnya" />
                                            <input type="hidden" class="hdnRegistrationID" value="<%#: Eval("RegistrationID")%>" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField HeaderStyle-Width="100px" DataField="NoSEP" HeaderText="No. SEP" HeaderStyle-HorizontalAlign="Left" />
                                    <asp:BoundField HeaderStyle-Width="100px" DataField="RegistrationNo" HeaderText="No. Registrasi" HeaderStyle-HorizontalAlign="Left" />
                                    <asp:BoundField HeaderStyle-Width="80px" DataField="RegistrationDateInString" HeaderText="Tgl. Registrasi" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign = "Center"/>
                                    <asp:BoundField HeaderStyle-Width="150px" DataField="NoSuratRencanaKontrolBerikutnya" HeaderText="No. Kontrol" HeaderStyle-HorizontalAlign="Left" />
                                    <asp:BoundField HeaderStyle-Width="80px" DataField="cfTglRencanaKontrolInString" HeaderText="Tgl. Rencana Kontrol" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign = "Center"/>
                                </Columns>
                                <EmptyDataTemplate>
                                    <%=GetLabel("Data Tidak Tersedia")%>
                                </EmptyDataTemplate>
                            </asp:GridView>
                            <div class="imgLoadingGrdView" id="containerImgLoadingPatientFamily">
                                <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                            </div>
                            </asp:Panel>
                        </dx:PanelContent>
                    </PanelCollection>
                </dxcp:ASPxCallbackPanel>
            </td>
        </tr>
    </table>
</div>
