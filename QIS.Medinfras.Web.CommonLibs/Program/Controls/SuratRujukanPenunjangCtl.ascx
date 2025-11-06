<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SuratRujukanPenunjangCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.SuratRujukanPenunjangCtl" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="LembarKonsultasiCtldxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<script type="text/javascript" id="dxss_medicalconsultation2">

    setDatePicker('<%=txtValueDate.ClientID %>');

    $(function () {
        hideLoadingPanel();
    });

    $('#<%=btnPrint.ClientID %>').click(function (evt) {
        if (IsValid(evt, 'fsTrxPopup', 'mpTrxPopup')) {
            cbpMedicalSickLeave.PerformCallback('Print');
        }
    });

    function onReprintPatientPaymentReceiptSuccess() {
        var testOrderID = $('#<%=hdnTestOrderIDCtl.ClientID %>').val();
        var Notes = $('#<%=txtNotes.ClientID %>').val();
        var SpecimenID = cboSpecimen.GetValue();
        var BusinessPartnerID = cboBussinesPartnerTo.GetValue();
        var reportCode = "LB-00025";
        var filterExpression = testOrderID + "|" + BusinessPartnerID + "|" + SpecimenID + "|" + Notes + "|" + reportCode;

        if (reportCode != "") {
            openReportViewer(reportCode, filterExpression);
        }
        pcRightPanelContent.Hide();
        cbpView.PerformCallback('refresh');
    }
    function onDataOrderDtViewEndCallback(e) { }
    function onCboBussinesPartnerToValueChanged(s) {
        cbpDataOrderDtView.PerformCallback('refresh');
    }
</script>
<div class="toolbarArea">
    <ul id="ulMPListToolbar" runat="server">
        <li id="btnPrint" runat="server">
            <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbprint.png")%>' alt="" /><br style="clear: both" />
            <div>
                <%=GetLabel("Print")%></div>
        </li>
    </ul>
</div>
<div style="padding: 10px;">
    <fieldset id="fsTrxPopup" style="margin: 0">
        <input type="hidden" id="hdnTestOrderIDCtl" runat="server" value="" />
        <input type="hidden" id="hdnTransactionHdIDCtl" runat="server" value="" />
        <input type="hidden" id="txtTransactionNoCtl" runat="server" value="" />
        <input type="hidden" id="hdnVisitIDCtl" runat="server" value="" />
        <input type="hidden" id="hdnTanggal" runat="server" value="" />
        <input type="hidden" id="hdnBusinessPartnerID" runat="server" value="" />
        <table width="100%">
            <tr>
                <td class="tdLabel">
                    <label class="lblMandatory">
                        <%=GetLabel("Tanggal :")%></label>
                </td>
                <td class="tdCustomDate">
                    <asp:TextBox ID="txtValueDate" CssClass="txtValueDate datepicker" runat="server"
                        Width="100px" />
                </td>
            </tr>
            <tr>
                <td class="tdLabel">
                    <label class="lblMandatory">
                        <%=GetLabel("Nama Pasien")%></label>
                </td>
                <td class="tdLabel">
                    <asp:TextBox ID="txtPatientName" runat="server" ReadOnly="true" Width="250px" />
                </td>
            </tr>
            <tr>
                <td class="tdLabel">
                    <label class="lblMandatory">
                        <%=GetLabel("No. RM")%></label>
                </td>
                <td class="tdLabel">
                    <asp:TextBox ID="txtMedicalNo" runat="server" ReadOnly="true" />
                </td>
            </tr>
            <tr>
                <td class="tdLabel">
                    <label class="lblMandatory">
                        <%=GetLabel("Jenis Sample")%></label>
                </td>
                <td>
                    <dxe:ASPxComboBox ID="cboSpecimen" ClientInstanceName="cboSpecimen" runat="server">
                    </dxe:ASPxComboBox>
                </td>
            </tr>
            <tr>
                <td class="tdLabel">
                    <label class="lblMandatory">
                        <%=GetLabel("Dirujuk kepada")%></label>
                </td>
                <td>
                    <dxe:ASPxComboBox ID="cboBussinesPartnerTo" ClientInstanceName="cboBussinesPartnerTo"
                        runat="server">
                        <ClientSideEvents ValueChanged="function(s){ onCboBussinesPartnerToValueChanged(s); }" />
                    </dxe:ASPxComboBox>
                </td>
            </tr>
            <tr>
                <td class="tdLabel">
                    <label class="lblMandatory">
                        <%=GetLabel("Catatan")%></label>
                </td>
                <td>
                    <asp:TextBox ID="txtNotes" Width="250px" CssClass="required" runat="server" TextMode="MultiLine"
                        Rows="10" />
                </td>
            </tr>
        </table>
        <div>
            <dxcp:ASPxCallbackPanel ID="cbpDataOrderDtView" runat="server" Width="100%" ClientInstanceName="cbpDataOrderDtView"
                ShowLoadingPanel="false" OnCallback="cbpDataOrderDtView_Callback">
                <ClientSideEvents EndCallback="function(s,e){ onDataOrderDtViewEndCallback(s); }" />
                <PanelCollection>
                    <dx:PanelContent ID="PanelContent6" runat="server">
                        <asp:Panel runat="server" ID="Panel5" CssClass="pnlContainerGrid" Style="height: 200px">
                            <asp:GridView ID="grdDataOrderDtView" runat="server" CssClass="grdSelected grdPatientPage"
                                AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                <Columns>
                                    <asp:BoundField DataField="ID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                    <asp:TemplateField HeaderStyle-CssClass="hiddenColumn" ItemStyle-CssClass="hiddenColumn">
                                        <ItemTemplate>
                                            <input type="hidden" value="<%#:Eval("ID") %>" bindingfield="ID" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderStyle-HorizontalAlign="Left">
                                        <HeaderTemplate>
                                            <%=GetLabel("Nama Pemeriksaan")%>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <div>
                                                <%#: Eval("ItemName1")%></div>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <EmptyDataTemplate>
                                    <%=GetLabel("Belum ada pemeriksaan untuk pasien ini") %>
                                </EmptyDataTemplate>
                            </asp:GridView>
                        </asp:Panel>
                    </dx:PanelContent>
                </PanelCollection>
            </dxcp:ASPxCallbackPanel>
        </div>
        <div class="imgLoadingGrdView" id="containerImgLoadingView">
            <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
        </div>
        <dxcp:ASPxCallbackPanel ID="cbpMedicalSickLeave" runat="server" Width="100%" ClientInstanceName="cbpMedicalSickLeave"
            ShowLoadingPanel="false" OnCallback="cbpMedicalSickLeave_Callback">
            <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel();}" EndCallback="function(s,e){
                    onReprintPatientPaymentReceiptSuccess();
                    hideLoadingPanel();
                }" />
        </dxcp:ASPxCallbackPanel>
    </fieldset>
</div>
