<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="OrderQuickPicksFromScheduledCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.Pharmacy.Program.OrderQuickPicksFromScheduledCtl" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="QIS.Medinfras.Web.CustomControl, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null"
    Namespace="QIS.Medinfras.Web.CustomControl" TagPrefix="qis" %>
<style type="text/css">
    .trSelectedItem
    {
        background-color: #ecf0f1 !important;
    }
</style>
<script type="text/javascript" id="dxss_OrderQuickPicksFromScheduledCtl">
    //#region Paging
    var pageCount = parseInt('<%=PageCount %>');

    $(function () {
        hideLoadingPanel();

        setPaging($("#pagingPopup"), pageCount, function (page) {
            RefreshGrid(true, page);
        });

        $('#<%=rblVisitPeriod.ClientID %> input').change(function () {
            RefreshGrid(false, 1);
        });

        $('#<%=rblVisitType.ClientID %> input').change(function () {
            RefreshGrid(false, 1);
        });

        $('#<%=grdView.ClientID %> > tbody > tr:gt(0):not(.trEmpty)').live('click', function () {
            if ($(this).attr('class') != 'selected') {
                $('#<%=grdView.ClientID %> tr.selected').removeClass('selected');
                $(this).addClass('selected');
                $('#<%=hdnSelectedPrescriptionOrderID.ClientID %>').val($(this).find('.keyField').html());

                if (typeof (grdPopupViewDt) != 'undefined' && typeof (cbpPopupViewDt) != 'undefined')
                    cbpPopupViewDt.PerformCallback('refresh');
                else
                    window.setTimeout("cbpPopupViewDt.PerformCallback('refresh');", 100);
            }
        });
        $('#<%=grdView.ClientID %> tr:eq(1)').click();
    });

    function RefreshGrid(mode, pageNo) {
        cbpPopup.PerformCallback('changepage|' + pageNo);
    }

    function PromptUserBeforeRefresh(mode, pageNo) {
        displayConfirmationMessageBox('QUICK PICKS : ORDER', 'Ada item yang telah dipilih, aksi anda akan mereset pilihan tersebut, dilanjutkan ?', function (result) {
            if (result) {
                if (mode == false)
                    cbpPopup.PerformCallback('refresh');
                else
                    cbpPopup.PerformCallback('changepage|' + pageNo);
            }
        });
    }

    function onCbpPopupEndCallback(s) {
        hideLoadingPanel();

        var param = s.cpResult.split('|');
        if (param[0] == 'refresh') {
            var pageCount = parseInt(param[1]);
            setPaging($("#pagingPopup"), pageCount, function (page) {
                RefreshGrid(true, page);
            });
            $('#<%=grdView.ClientID %> tr:eq(1)').click();
        }
    }
    //#endregion

    //#region Paging Dt
    function onCbpPopupViewDtEndCallback(s) {
        $('#containerImgLoadingViewDt').hide();

        var param = s.cpResult.split('|');
        if (param[0] == 'refresh') {
            var pageCount = parseInt(param[1]);
            if (pageCount > 0)
                $('#<%=grdPopupViewDt.ClientID %> tr:eq(1)').click();

            setPaging($("#pagingDt1"), pageCount, function (page) {
                cbpViewDt.PerformCallback('changepage|' + page);
            });
        }
        else
            $('#<%=grdPopupViewDt.ClientID %> tr:eq(1)').click();
    }
    //#endregion

    function onAfterSaveRecordPatientPageEntry(value) {
        if (typeof onAfterSaveDetail == 'function')
            onAfterSaveDetail(value);
    }
</script>
<div style="padding: 1px;">
    <input type="hidden" id="hdnSelectedMember" runat="server" value="" />
    <input type="hidden" id="hdnOrderID" runat="server" value="" />
    <input type="hidden" id="hdnTransactionID" runat="server" value="" />
    <input type="hidden" id="hdnRegistrationID" runat="server" value="" />
    <input type="hidden" id="hdnVisitID" runat="server" value="" />
    <input type="hidden" id="hdnSelectedPrescriptionOrderID" runat="server" value="" />
    <input type="hidden" id="hdnChargeClassID" runat="server" value="" />
    <input type="hidden" id="hdnDefaultGCMedicationRoute" runat="server" value="" />
    <input type="hidden" id="hdnParamedicID" runat="server" value="" />
    <input type="hidden" id="hdnDispensaryUnitID" runat="server" value="" />
    <input type="hidden" id="hdnPrescriptionType" runat="server" value="" />
    <input type="hidden" id="hdnPrescriptionDate" value="" runat="server" />
    <input type="hidden" id="hdnPrescriptionTime" value="" runat="server" />
    <input type="hidden" id="hdnParam" runat="server" value="" />
    <input type="hidden" id="hdnLocationID" runat="server" value="" />
    <input type="hidden" id="hdnPrescriptionFeeAmount" runat="server" value="" />
    <input type="hidden" id="hdnIsAutoGenerateReferenceNo" value="0" runat="server" />
    <input type="hidden" id="hdnIsGenerateQueueLabel" value="0" runat="server" />
    <input type="hidden" id="hdnItemQtyWithSpecialQueuePrefix" value="0" runat="server" />
    <input type="hidden" id="hdnIsEndingAmountRoundingTo100" runat="server" value="0" />
    <input type="hidden" id="hdnIsEndingAmountRoundingTo1" runat="server" value="0" />
    <table style="width: 100%">
        <colgroup>
            <col style="width: 40%" />
            <col style="width: 60%" />
        </colgroup>
        <tr>
            <td style="padding: 2px; vertical-align: top">
                <h4>
                    <%=GetLabel("Riwayat Kunjungan Pasien :")%></h4>
                <table cellspacing="0" width="100%">
                    <colgroup>
                        <col style="width: 120px" />
                        <col style="width: 100px" />
                        <col style="width: 250px" />
                        <col />
                    </colgroup>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal" id="lblVisitPeriod">
                                <%=GetLabel("Periode")%></label>
                        </td>
                        <td colspan="2">
                            <asp:RadioButtonList ID="rblVisitPeriod" runat="server" RepeatDirection="Horizontal">
                                <asp:ListItem Text="3 Bulan Terakhir" Value="1" Selected="True" />
                                <asp:ListItem Text="6 Bulan Terakhir" Value="2" />
                                <asp:ListItem Text="Seluruhnya" Value="3" />
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal" id="lblVisitType">
                                <%=GetLabel("Display")%></label>
                        </td>
                        <td colspan="2">
                            <asp:RadioButtonList ID="rblVisitType" runat="server" RepeatDirection="Horizontal">
                                <asp:ListItem Text="All" Value="0" Selected="True" />
                                <asp:ListItem Text="Pasien Dokter" Value="1" />
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                    <tr>
                        <td>
                        </td>
                        <td colspan="2">
                        </td>
                    </tr>
                </table>
                <dxcp:ASPxCallbackPanel ID="cbpPopup" runat="server" Width="100%" ClientInstanceName="cbpPopup"
                    ShowLoadingPanel="false" OnCallback="cbpPopup_Callback">
                    <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel();}" EndCallback="function(s,e){ onCbpPopupEndCallback(s); }" />
                    <PanelCollection>
                        <dx:PanelContent ID="PanelContent1" runat="server" CssClass="pnlContainerGridPatientPage">
                            <asp:Panel runat="server" ID="pnlView" Style="width: 100%; margin-top: 20px; margin-left: auto;
                                margin-right: auto; position: relative; font-size: 0.95em;">
                                <asp:GridView ID="grdView" runat="server" CssClass="grdSelected grdPatientPage" AutoGenerateColumns="false"
                                    ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty" OnRowDataBound="grdView_RowDataBound">
                                    <Columns>
                                        <asp:BoundField DataField="PrescriptionOrderID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                <%=GetLabel("Informasi Resep Asal")%>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <div>
                                                    <label style="font-size: x-small; font-style: italic">
                                                        <%=GetLabel("No. Order Resep : ")%></label>
                                                    <b>
                                                        <%#: Eval("PrescriptionOrderNo")%></b></div>
                                                <div>
                                                    <label style="font-size: x-small; font-style: italic">
                                                        <%=GetLabel("No. Registrasi : ")%></label>
                                                    <%#: Eval("RegistrationNo")%></div>
                                                <div>
                                                    <b>
                                                        <%#: Eval("cfVisitDate")%></b> <span style="color: Blue">
                                                            <%#: Eval("ServiceUnitName")%></span></div>
                                                <div>
                                                    <%#: Eval("ParamedicName")%></div>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <EmptyDataTemplate>
                                        <%=GetLabel("Tidak ada informasi riwayat kunjungan untuk pasien")%>
                                    </EmptyDataTemplate>
                                </asp:GridView>
                            </asp:Panel>
                        </dx:PanelContent>
                    </PanelCollection>
                </dxcp:ASPxCallbackPanel>
                <div class="imgLoadingGrdView" id="containerImgLoadingView1">
                    <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                </div>
                <div class="containerPaging">
                    <div class="wrapperPaging">
                        <div id="pagingPopup">
                        </div>
                    </div>
                </div>
            </td>
            <td style="padding: 2px; vertical-align: top">
                <h4>
                    <%=GetLabel("Detail Item :")%></h4>
                <div style="position: relative;">
                    <dxcp:ASPxCallbackPanel ID="cbpPopupViewDt" runat="server" Width="100%" ClientInstanceName="cbpPopupViewDt"
                        ShowLoadingPanel="false" OnCallback="cbpViewDt_Callback">
                        <ClientSideEvents BeginCallback="function(s,e){$('#containerImgLoadingViewDt').show(); }"
                            EndCallback="function(s,e){ onCbpPopupViewDtEndCallback(s); }" />
                        <PanelCollection>
                            <dx:PanelContent ID="PanelContent2" runat="server">
                                <asp:Panel runat="server" ID="Panel1" CssClass="pnlContainerGridPatientPage">
                                    <asp:GridView ID="grdPopupViewDt" runat="server" CssClass="grdSelected grdPatientPage"
                                        AutoGenerateColumns="false" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                        <Columns>
                                            <asp:TemplateField HeaderStyle-Width="40px" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                                                <HeaderTemplate>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <input type="hidden" class="keyField" value='<%#:Eval("PrescriptionOrderDetailID")%>' />
                                                    <input type="hidden" class="cfOrderInformation" value='<%#:Eval("cfOrderInformation")%>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-HorizontalAlign="Left">
                                                <HeaderTemplate>
                                                    <div>
                                                        <%=GetLabel("Item Name")%></div>
                                                </HeaderTemplate>
                                                <ItemTemplate>
                                                    <div id="divItemName" runat="server" style="font-weight: bold">
                                                        <span class="itemName">
                                                            <%#: Eval("cfMedicationName")%></span></div>
                                                    <div style='<%# Eval("IsCompound").ToString() == "False" ? "display:none;": "white-space: pre-line;font-style:italic; padding-top:5px" %>'>
                                                        <%#: Eval("cfCompoundDetail")%></div>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-Width="90px" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right"
                                                HeaderText="Frekuensi">
                                                <ItemTemplate>
                                                    <input type="hidden" value='<%#:Eval("GCDosingFrequency")%>' class="hdnGCItemUnit" />
                                                    <div>
                                                        <%#:Eval("Frequency")%>
                                                        <%#:Eval("DosingFrequency")%></div>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-Width="90px" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right"
                                                HeaderText="Dosis">
                                                <ItemTemplate>
                                                    <div>
                                                        <%#:Eval("cfNumberOfDosage")%>
                                                        <%#:Eval("DosingUnit")%></div>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-Width="100px" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right"
                                                HeaderText="Jumlah Diorder">
                                                <ItemTemplate>
                                                    <input type="hidden" value='<%#:Eval("GCItemUnit")%>' class="hdnGCItemUnit" />
                                                    <div>
                                                        <%#:Eval("DispenseQty")%>
                                                        <%#:Eval("ItemUnit")%></div>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-Width="100px" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right"
                                                HeaderText="Jumlah Diambil">
                                                <ItemTemplate>
                                                    <input type="hidden" value='<%#:Eval("GCItemUnit")%>' class="hdnGCItemUnit" />
                                                    <div>
                                                        <%#:Eval("TakenQty")%>
                                                        <%#:Eval("ItemUnit")%></div>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-Width="170px" HeaderStyle-HorizontalAlign="Left" ItemStyle-HorizontalAlign="Left"
                                                HeaderText="Special Instruction">
                                                <ItemTemplate>
                                                    <div>
                                                        <%#:Eval("MedicationAdministration")%></div>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <EmptyDataTemplate>
                                            <%=GetLabel("No Data To Display")%>
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
                            <div id="pagingDt1">
                            </div>
                        </div>
                    </div>
                </div>
            </td>
        </tr>
    </table>
</div>
