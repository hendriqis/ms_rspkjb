<%@ Page Title="" Language="C#" MasterPageFile="~/Libs/MasterPage/PatientPage/MPBasePatientPageTrx.master"
    AutoEventWireup="true" CodeBehind="ResendLaboratoryOrderDetail.aspx.cs" Inherits="QIS.Medinfras.Web.CommonLibs.Program.ResendLaboratoryOrderDetail" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<asp:Content ID="Content3" ContentPlaceHolderID="plhCustomButtonToolbar" runat="server">
    <li id="btnBack" runat="server" crudmode="R">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/back.png")%>' alt="" /><div>
            <%=GetLabel("Back")%></div>
    </li>
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div class="menuTitle">
        <%=HttpUtility.HtmlEncode(GetPageTitle())%></div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="plhHeader" runat="server">
    <input type="hidden" value="" id="hdnRegistrationID" runat="server" />
    <input type="hidden" value="" id="hdnVisitID" runat="server" />
    <input type="hidden" value="" id="hdnLinkedRegistrationID" runat="server" />
    <input type="hidden" value="" id="hdnDepartmentID" runat="server" />
    <input type="hidden" value="" id="hdnHealthcareServiceUnitID" runat="server" />
</asp:Content>
<asp:Content ID="Content6" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript">
        $('#<%=btnBack.ClientID %>').live('click', function () {
            showLoadingPanel();
            var url = "~/Libs/Program/Module/PatientManagement/Process/ResendLaboratoryOrderList.aspx";
            document.location = ResolveUrl(url);
        });

        $(function () {
            //#region Transaction No
            $('#lblTransactionNo.lblLink').live('click', function () {
                var filterExpression = "<%:GetFilterExpression() %>";
                openSearchDialog('patientchargeshd', filterExpression, function (value) {
                    $('#<%=txtTransactionNo.ClientID %>').val(value);
                    onTxtTransactionNoChanged(value);
                });
            });

            $('#<%=txtTransactionNo.ClientID %>').change(function () {
                onTxtTransactionNoChanged($(this).val());
            });

            function onTxtTransactionNoChanged(value) {
                onLoadObject(value);
            }
            //#endregion
        });

        function onBeforeLoadRightPanelContent(code) {
            return $('#<%:hdnTransactionID.ClientID %>').val();
        }

        function onAfterCustomClickSuccess(type) {
            cbpView.PerformCallback();
        }

        function onCbpViewEndCallback(s) {
            hideLoadingPanel();
        }
    </script>
    <input type="hidden" value="" id="hdnPageTitle" runat="server" />
    <input type="hidden" value="" id="hdnID" runat="server" />
    <input type="hidden" value="" id="hdnTransactionID" runat="server" />
    <input type="hidden" value="" id="hdnHsuImaging" runat="server" />
    <input type="hidden" value="" id="hdnHsuLaboratory" runat="server" />
    <div>
        <table class="tblContentArea" style="width: 100%">
            <colgroup>
                <col style="width: 50%" />
                <col style="width: 50%" />
            </colgroup>
            <tr>
                <td style="padding: 5px; vertical-align: top">
                    <table class="tblEntryContent" style="width: 100%">
                        <colgroup>
                            <col style="width: 30%" />
                            <col />
                        </colgroup>
                        <tr>
                            <td class="tdLabel">
                                <div style="position: relative;">
                                    <label class="lblLink lblKey" id="lblTransactionNo">
                                        <%=GetLabel("No. Transaksi")%></label></div>
                            </td>
                            <td>
                                <asp:TextBox ID="txtTransactionNo" Width="150px" ReadOnly="true" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <%=GetLabel("Tanggal") %>
                                -
                                <%=GetLabel("Jam") %>
                            </td>
                            <td>
                                <table cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td style="padding-right: 1px; width: 145px">
                                            <asp:TextBox ID="txtTransactionDate" Width="120px" ReadOnly = "true" CssClass="datepicker" runat="server" />
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtTransactionTime" ReadOnly = "true" Width="80px" CssClass="time" runat="server"
                                                Style="text-align: center" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
                <td style="padding: 5px; vertical-align: top">
                    <table class="tblEntryContent" style="width: 100%">
                        <colgroup>
                            <col style="width: 30%" />
                            <col />
                        </colgroup>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal" id="lblTestOrderInfo">
                                    <%=GetLabel("Informasi Order")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtTestOrderInfo" Width="100%" ReadOnly="true" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal" id="Label2">
                                    <%=GetLabel("Kelompok Tindakan Order")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtProcedureOrderInfo" Width="100%" ReadOnly="true" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label id="Label1" class="lblNormal" runat="server">
                                    <%=GetLabel("No. Referensi")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtReferenceNo" ReadOnly = "true" Width="150px" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Catatan")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtRemarks" ReadOnly = "true" Width="100%" runat="server" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td colspan="2">
                    <div style="padding: 5px; min-height: 150px;">
                        <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
                            ShowLoadingPanel="false" OnCallback="cbpView_Callback">
                            <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e) { onCbpViewEndCallback(s); }" />
                            <PanelCollection>
                                <dx:PanelContent ID="PanelContent1" runat="server">
                                    <asp:Panel runat="server" ID="pnlService" Style="width: 100%; margin-left: auto;
                                        margin-right: auto; position: relative; font-size: 0.95em;">
                                        <asp:ListView ID="lvwService" runat="server">
                                            <LayoutTemplate>
                                                <table id="tblView" runat="server" class="grdService grdNormal notAllowSelect" cellspacing="0"
                                                    rules="all">
                                                    <tr>
                                                        <th rowspan="2">
                                                            <div style="text-align: left; padding-left: 3px">
                                                                <%=GetLabel("Item")%>
                                                            </div>
                                                        </th>
                                                        <th rowspan="2" style="width: 70px">
                                                            <div style="text-align: left; padding-left: 3px">
                                                                <%=GetLabel("Kelas Tagihan")%>
                                                            </div>
                                                        </th>
                                                        <th rowspan="2" style="width: 80px">
                                                            <div style="text-align: right; padding-right: 3px">
                                                                <%=GetLabel("Harga Satuan")%>
                                                            </div>
                                                        </th>
                                                        <th rowspan="2" style="width: 50px">
                                                            <div style="text-align: right; padding-right: 3px">
                                                                <%=GetLabel("Jumlah")%>
                                                            </div>
                                                        </th>
                                                        <th colspan="3">
                                                            <%=GetLabel("HARGA")%>
                                                        </th>
                                                        <th colspan="3">
                                                            <%=GetLabel("TOTAL")%>
                                                        </th>
                                                        <th rowspan="2" style="width: 90px">
                                                            <div style="text-align: right; padding-right: 3px">
                                                                <%=GetLabel("Petugas")%>
                                                            </div>
                                                        </th>
<%--                                                        <th rowspan="2">
                                                            &nbsp;
                                                        </th>--%>
                                                        <th rowspan="2">
                                                            &nbsp;
                                                        </th>
                                                        <th rowspan="2">
                                                            &nbsp;
                                                        </th>
                                                    </tr>
                                                    <tr>
                                                        <th style="width: 80px">
                                                            <div style="text-align: right; padding-right: 3px">
                                                                <%=GetLabel("Harga")%>
                                                            </div>
                                                        </th>
                                                        <th style="width: 90px">
                                                            <div style="text-align: right; padding-right: 3px">
                                                                <%=GetLabel("CITO")%>
                                                            </div>
                                                        </th>
                                                        <th style="width: 80px; display: none">
                                                            <div style="text-align: right; padding-right: 3px">
                                                                <%=GetLabel("Penyulit")%>
                                                            </div>
                                                        </th>
                                                        <th style="width: 80px">
                                                            <div style="text-align: right; padding-right: 3px">
                                                                <%=GetLabel("Diskon")%>
                                                            </div>
                                                        </th>
                                                        <th style="width: 80px">
                                                            <div style="text-align: right; padding-right: 3px">
                                                                <%=GetLabel("Instansi")%>
                                                            </div>
                                                        </th>
                                                        <th style="width: 80px">
                                                            <div style="text-align: right; padding-right: 3px">
                                                                <%=GetLabel("Pasien")%>
                                                            </div>
                                                        </th>
                                                        <th style="width: 80px">
                                                            <div style="text-align: right; padding-right: 3px">
                                                                <%=GetLabel("Total")%>
                                                            </div>
                                                        </th>
                                                    </tr>
                                                    <tr runat="server" id="itemPlaceholder">
                                                    </tr>
                                                    <tr id="Tr1" class="trFooter" runat="server">
                                                        <td colspan="7" align="right" style="padding-right: 3px">
                                                            <%=GetLabel("TOTAL") %>
                                                        </td>
                                                        <td align="right" style="padding-right: 9px" id="tdServiceTotalPayer" class="tdServiceTotalPayer"
                                                            runat="server">
                                                        </td>
                                                        <td align="right" style="padding-right: 9px" id="tdServiceTotalPatient" class="tdServiceTotalPatient"
                                                            runat="server">
                                                        </td>
                                                        <td align="right" style="padding-right: 9px" id="tdServiceTotal" class="tdServiceTotal"
                                                            runat="server">
                                                        </td>
                                                        <td>
                                                            &nbsp;
                                                        </td>
                                                        <td>
                                                            &nbsp;
                                                        </td>
                                                        <td>
                                                            &nbsp;
                                                        </td>
                                                    </tr>
                                                </table>
                                            </LayoutTemplate>
                                            <ItemTemplate>
                                                <tr>
                                                    <td>
                                                        <div style="padding: 3px">
                                                            <input type="hidden" value='<%#: Eval("ID") %>' bindingfield="ID" />
                                                            <input type="hidden" value='<%#: Eval("ItemID") %>' bindingfield="ItemID" />
                                                            <input type="hidden" value='<%#: Eval("ItemCode") %>' bindingfield="ItemCode" />
                                                            <input type="hidden" value='<%#: Eval("ItemName1") %>' bindingfield="ItemName1" />
                                                            <input type="hidden" value='<%#: Eval("RevenueSharingID") %>' bindingfield="RevenueSharingID" />
                                                            <input type="hidden" value='<%#: Eval("ParamedicID") %>' bindingfield="ParamedicID" />
                                                            <input type="hidden" value='<%#: Eval("ParamedicCode") %>' bindingfield="ParamedicCode" />
                                                            <input type="hidden" value='<%#: Eval("ParamedicName") %>' bindingfield="ParamedicName" />
                                                            <input type="hidden" value='<%#: Eval("BusinessPartnerID") %>' bindingfield="TestPartnerID" />
                                                            <input type="hidden" value='<%#: Eval("BusinessPartnerCode") %>' bindingfield="TestPartnerCode" />
                                                            <input type="hidden" value='<%#: Eval("BusinessPartnerName") %>' bindingfield="TestPartnerName" />
                                                            <input type="hidden" value='<%#: Eval("ChargeClassID") %>' bindingfield="ChargeClassID" />
                                                            <input type="hidden" value='<%#: Eval("ChargedQuantity") %>' bindingfield="ChargedQuantity" />
                                                            <input type="hidden" value='<%#: Eval("BaseTariff") %>' bindingfield="BaseTariff" />
                                                            <input type="hidden" value='<%#: Eval("Tariff") %>' bindingfield="Tariff" />
                                                            <input type="hidden" value='<%#: Eval("TariffComp1") %>' bindingfield="TariffComp1" />
                                                            <input type="hidden" value='<%#: Eval("TariffComp2") %>' bindingfield="TariffComp2" />
                                                            <input type="hidden" value='<%#: Eval("TariffComp3") %>' bindingfield="TariffComp3" />
                                                            <input type="hidden" value='<%#: Eval("CostAmount") %>' bindingfield="CostAmount" />
                                                            <input type="hidden" value='<%#: Eval("GrossLineAmount") %>' bindingfield="GrossLineAmount" />
                                                            <input type="hidden" value='<%#: Eval("IsAllowCITO") %>' bindingfield="IsAllowCITO" />
                                                            <input type="hidden" value='<%#: Eval("IsAllowComplication") %>' bindingfield="IsAllowComplication" />
                                                            <input type="hidden" value='<%#: Eval("IsAllowDiscount") %>' bindingfield="IsAllowDiscount" />
                                                            <input type="hidden" value='<%#: Eval("IsAllowDiscountTariffComp1") %>' bindingfield="IsAllowDiscountTariffComp1" />
                                                            <input type="hidden" value='<%#: Eval("IsAllowDiscountTariffComp2") %>' bindingfield="IsAllowDiscountTariffComp2" />
                                                            <input type="hidden" value='<%#: Eval("IsAllowDiscountTariffComp3") %>' bindingfield="IsAllowDiscountTariffComp3" />
                                                            <input type="hidden" value='<%#: Eval("IsAllowVariable") %>' bindingfield="IsAllowVariable" />
                                                            <input type="hidden" value='<%#: Eval("IsAllowVariableTariffComp1") %>' bindingfield="IsAllowVariableTariffComp1" />
                                                            <input type="hidden" value='<%#: Eval("IsAllowVariableTariffComp2") %>' bindingfield="IsAllowVariableTariffComp2" />
                                                            <input type="hidden" value='<%#: Eval("IsAllowVariableTariffComp3") %>' bindingfield="IsAllowVariableTariffComp3" />
                                                            <input type="hidden" value='<%#: Eval("IsAllowUnbilledItem") %>' bindingfield="IsAllowUnbilledItem" />
                                                            <input type="hidden" value='<%#: Eval("IsCITO") %>' bindingfield="IsCITO" />
                                                            <input type="hidden" value='<%#: Eval("IsCITOInPercentage") %>' bindingfield="IsCITOInPercentage" />
                                                            <input type="hidden" value='<%#: Eval("IsComplication") %>' bindingfield="IsComplication" />
                                                            <input type="hidden" value='<%#: Eval("IsComplicationInPercentage") %>' bindingfield="IsComplicationInPercentage" />
                                                            <input type="hidden" value='<%#: Eval("IsVariable") %>' bindingfield="IsVariable" />
                                                            <input type="hidden" value='<%#: Eval("DefaultTariffComp") %>' bindingfield="DefaultTariffComp" />
                                                            <input type="hidden" value='<%#: Eval("IsUnbilledItem") %>' bindingfield="IsUnbilledItem" />
                                                            <input type="hidden" value='<%#: Eval("BaseCITOAmount") %>' bindingfield="BaseCITOAmount" />
                                                            <input type="hidden" value='<%#: Eval("CITOAmount") %>' bindingfield="CITOAmount" />
                                                            <input type="hidden" value='<%#: Eval("CITODiscount") %>' bindingfield="CITODiscount" />
                                                            <input type="hidden" value='<%#: Eval("IsDiscount") %>' bindingfield="IsDiscount" />
                                                            <input type="hidden" value='<%#: Eval("DiscountAmount") %>' bindingfield="DiscountAmount" />
                                                            <input type="hidden" value='<%#: Eval("IsDiscountInPercentageComp1") %>' bindingfield="IsDiscountInPercentageComp1" />
                                                            <input type="hidden" value='<%#: Eval("DiscountPercentageComp1") %>' bindingfield="DiscountPercentageComp1" />
                                                            <input type="hidden" value='<%#: Eval("DiscountComp1") %>' bindingfield="DiscountComp1" />
                                                            <input type="hidden" value='<%#: Eval("IsDiscountInPercentageComp2") %>' bindingfield="IsDiscountInPercentageComp2" />
                                                            <input type="hidden" value='<%#: Eval("DiscountPercentageComp2") %>' bindingfield="DiscountPercentageComp2" />
                                                            <input type="hidden" value='<%#: Eval("DiscountComp2") %>' bindingfield="DiscountComp2" />
                                                            <input type="hidden" value='<%#: Eval("IsDiscountInPercentageComp3") %>' bindingfield="IsDiscountInPercentageComp3" />
                                                            <input type="hidden" value='<%#: Eval("DiscountPercentageComp3") %>' bindingfield="DiscountPercentageComp3" />
                                                            <input type="hidden" value='<%#: Eval("DiscountComp3") %>' bindingfield="DiscountComp3" />
                                                            <input type="hidden" value='<%#: Eval("BaseComplicationAmount") %>' bindingfield="BaseComplicationAmount" />
                                                            <input type="hidden" value='<%#: Eval("ComplicationAmount") %>' bindingfield="ComplicationAmount" />
                                                            <input type="hidden" value='<%#: Eval("PatientAmount") %>' bindingfield="PatientAmount" />
                                                            <input type="hidden" value='<%#: Eval("PayerAmount") %>' bindingfield="PayerAmount" />
                                                            <input type="hidden" value='<%#: Eval("LineAmount") %>' bindingfield="LineAmount" />
                                                            <input type="hidden" value='<%#: Eval("IsSubContractItem") %>' bindingfield="IsSubContractItem" />
                                                            <input type="hidden" value='<%#: Eval("PreconditionNotes") %>' bindingfield="PreconditionNotes" />
                                                            <div>
                                                                <%#: Eval("ItemName1")%></div>
                                                            <div>
                                                                <span style="font-style: italic">
                                                                    <%#: Eval("ItemCode") %></span>, <span style="color: Blue">
                                                                        <%#: Eval("ParamedicName")%></span>
                                                            </div>
                                                            <div <%# Eval("BusinessPartnerName").ToString() != "" ?  "" : "style='display:none'" %>>
                                                                <%#: Eval("BusinessPartnerName")%></div>
                                                        </div>
                                                    </td>
                                                    <td>
                                                        <div style="padding: 3px;">
                                                            <div>
                                                                <%#: Eval("ChargeClassName")%></div>
                                                        </div>
                                                    </td>
                                                    <td>
                                                        <div style="padding: 3px; text-align: right;">
                                                            <div>
                                                                <%#: Eval("Tariff", "{0:N}")%></div>
                                                        </div>
                                                    </td>
                                                    <td>
                                                        <div style="padding: 3px; text-align: right;">
                                                            <div>
                                                                <%#: Eval("ChargedQuantity")%></div>
                                                        </div>
                                                    </td>
                                                    <td>
                                                        <div style="padding: 3px; text-align: right;">
                                                            <div>
                                                                <%#: Eval("GrossLineAmount", "{0:N}")%></div>
                                                        </div>
                                                    </td>
                                                    <td>
                                                        <div style="padding: 3px; text-align: right;">
                                                            <div>
                                                                <%#: Eval("CITOAmount", "{0:N}")%></div>
                                                        </div>
                                                    </td>
                                                    <td style="display: none">
                                                        <div style="padding: 3px; text-align: right;">
                                                            <div>
                                                                <%#: Eval("ComplicationAmount", "{0:N}")%></div>
                                                        </div>
                                                    </td>
                                                    <td>
                                                        <div style="padding: 3px; text-align: right;">
                                                            <div>
                                                                <%#: Eval("DiscountAmount", "{0:N}")%></div>
                                                        </div>
                                                    </td>
                                                    <td>
                                                        <div style="padding: 3px; text-align: right;">
                                                            <div>
                                                                <%#: Eval("PayerAmount", "{0:N}")%></div>
                                                        </div>
                                                    </td>
                                                    <td>
                                                        <div style="padding: 3px; text-align: right;">
                                                            <div>
                                                                <%#: Eval("PatientAmount", "{0:N}")%></div>
                                                        </div>
                                                    </td>
                                                    <td>
                                                        <div style="padding: 3px; text-align: right;">
                                                            <div>
                                                                <%#: Eval("LineAmount", "{0:N}")%></div>
                                                        </div>
                                                    </td>
                                                    <td>
                                                        <div style="padding-right: 3px; text-align: right;">
                                                            <div>
                                                                <%#: Eval("CreatedByFullName")%></div>
                                                            <div>
                                                                <%#: Eval("CreatedDateInString")%></div>
                                                        </div>
                                                    </td>
<%--                                                    <td <%# IsShowSwitchIcon.ToString() == "True" && IsEditable.ToString() == "True" ?  "" : "style='display:none'" %>
                                                        valign="middle">
                                                        <img class="imgServiceSwitch imgLink" title='<%=GetLabel("Switch")%>' src='<%# ResolveUrl("~/Libs/Images/Button/switch.png")%>'
                                                            alt="" />
                                                    </td>--%>
                                                    <td <%# Eval("IsUnitPriceOverLimit").ToString() == "True" ? "" : "style='display:none'" %>
                                                        valign="middle">
                                                        <img src='<%# Eval("IsUnitPriceOverLimit").ToString() == "True" && Eval("IsConfirmed").ToString() == "True" ? ResolveUrl("~/Libs/Images/Status/coverage_ok.png") : ResolveUrl("~/Libs/Images/Status/coverage_warning.png")%>'
                                                            title='<%=GetLabel("ServiceUnitPrice = ")%><%# Eval("cfServiceUnitPriceInString") %><%=GetLabel("\nDrugSuppliesUnitPrice = ")%><%# Eval("cfDrugSuppliesUnitPriceInString") %><%=GetLabel("\nLogisticUnitPrice = ")%><%# Eval("cfLogisticUnitPriceInString") %>'
                                                            alt="" style="width: 30px" />
                                                    </td>
                                                    <td>
                                                        <img class="imgPreconditionNotesCtl imgLink" <%# Eval("PreconditionNotes").ToString() != "" ?  "" : "style='display:none'" %>
                                                            title='<%=GetLabel("Precondition Notes")%>' src='<%# ResolveUrl("~/Libs/Images/Button/info.png")%>'
                                                            alt="" />
                                                    </td>
                                                </tr>
                                            </ItemTemplate>
                                        </asp:ListView>
                                        <div class="imgLoadingGrdView" id="containerImgLoadingService">
                                            <img src='<%= ResolveUrl("~/Libs/Images/loading_small.gif")%>' alt='' />
                                        </div>
                                    </asp:Panel>
                                </dx:PanelContent>
                            </PanelCollection>
                        </dxcp:ASPxCallbackPanel>
                    </div>
                </td>
            </tr>
        </table>
    </div>
</asp:Content>
