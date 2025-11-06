<%@ Page Language="C#" MasterPageFile="~/Libs/MasterPage/PatientPage/MPBasePatientPageTrx.Master"
    AutoEventWireup="true" CodeBehind="InfoPatientBillSummaryDiscountDetail.aspx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.InfoPatientBillSummaryDiscountDetail" %>

<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Src="~/Libs/Program/Module/PatientManagement/PatientBillSummary/PatientBillSummaryToolbarCtl.ascx"
    TagName="ToolbarCtl" TagPrefix="uc1" %>
<asp:Content ID="Content4" ContentPlaceHolderID="plhMPPatientPageNavigationPane"
    runat="server">
    <uc1:ToolbarCtl ID="ctlToolbar" runat="server" />
</asp:Content>
<asp:Content ID="Content5" ContentPlaceHolderID="plhMenuTitle" runat="server">
    <div class="menuTitle">
        <%=HttpUtility.HtmlEncode(GetPageTitle())%></div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="plhCustomButtonToolbar" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="plhHeader" runat="server">
    <input type="hidden" value="" id="hdnRegistrationID" runat="server" />
    <input type="hidden" value="" id="hdnVisitID" runat="server" />
    <input type="hidden" value="" id="hdnLinkedRegistrationID" runat="server" />
    <input type="hidden" value="" id="hdnDepartmentID" runat="server" />
    <input type="hidden" value="" id="hdnBusinessPartnerID" runat="server" />
    <input type="hidden" value="" id="hdnPageTitle" runat="server" />
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript">
        function onLoad() {
            $('.txtCurrency').each(function () {
                $(this).trigger('changeValue');
            });

        }

        //#region Filter

        //#region Charges Department
        function onCboDepartmentChanged() {
            $('#<%=hdnChargesHealthcareServiceUnitID.ClientID %>').val('');
            $('#<%=txtChargesServiceUnitCode.ClientID %>').val('');
            $('#<%=txtChargesServiceUnitName.ClientID %>').val('');
            $('#<%=hdnCboChargesDepartmentID.ClientID %>').val(cboDepartment.GetValue());
        }
        //#endregion

        //#region Charges Service Unit
        function getChargesHealthcareServiceUnitFilterExpression() {
            var filterExpression = "HealthcareID = '" + AppSession.healthcareID + "'"
                                    + " AND DepartmentID = '" + cboDepartment.GetValue() + "'"
                                    + " AND " + $('#<%=hdnFilterChargesHealthcareServiceUnitID.ClientID %>').val();
            return filterExpression;
        }

        $('#lblChargesHealthcareServiceUnit.lblLink').live('click', function () {
            openSearchDialog('serviceunitperhealthcare', getChargesHealthcareServiceUnitFilterExpression(), function (value) {
                $('#<%=txtChargesServiceUnitCode.ClientID %>').val(value);
                ontxtChargesServiceUnitCodeChanged(value);
            });
        });

        $('#<%=txtChargesServiceUnitCode.ClientID %>').live('change', function () {
            ontxtChargesServiceUnitCodeChanged($(this).val());
        });

        function ontxtChargesServiceUnitCodeChanged(value) {
            var filterExpression = getChargesHealthcareServiceUnitFilterExpression() + " AND ServiceUnitCode = '" + value + "'";
            Methods.getObject('GetvHealthcareServiceUnitList', filterExpression, function (result) {
                if (result != null) {
                    $('#<%=hdnChargesHealthcareServiceUnitID.ClientID %>').val(result.HealthcareServiceUnitID);
                    $('#<%=txtChargesServiceUnitCode.ClientID %>').val(result.ServiceUnitCode);
                    $('#<%=txtChargesServiceUnitName.ClientID %>').val(result.ServiceUnitName);
                }
                else {
                    $('#<%=hdnChargesHealthcareServiceUnitID.ClientID %>').val('');
                    $('#<%=txtChargesServiceUnitCode.ClientID %>').val('');
                    $('#<%=txtChargesServiceUnitName.ClientID %>').val('');
                }
            });
        }
        //#endregion

        //#region Charges Paramedic
        function getChargesParamedicMasterFilterExpression() {
            var registrationID = $('#<%=hdnRegistrationID.ClientID %>').val();
            var filterExpression = "IsDeleted = 0";

            filterExpression += " AND ParamedicID IN (SELECT ParamedicID FROM vPatientChargesDt10 WHERE ((RegistrationID = " + registrationID + " OR (LinkedToRegistrationID = " + registrationID + " AND IsChargesTransfered = 1))" + " AND GCTransactionStatus IN ('X121^001','X121^002') AND GCTransactionDetailStatus IN ('X121^001','X121^002') AND IsDeleted = 0))"

            if ($('#<%=hdnChargesHealthcareServiceUnitID.ClientID %>').val() != "" && $('#<%=hdnChargesHealthcareServiceUnitID.ClientID %>').val() != "0") {
                filterExpression += " AND ParamedicID IN (SELECT ParamedicID FROM ServiceUnitParamedic WHERE HealthcareServiceUnitID = '" + $('#<%=hdnChargesHealthcareServiceUnitID.ClientID %>').val() + "')";
            }

            return filterExpression;
        }

        $('#lblChargesParamedic.lblLink').live('click', function () {
            openSearchDialog('physician', getChargesParamedicMasterFilterExpression(), function (value) {
                $('#<%=txtChargesParamedicCode.ClientID %>').val(value);
                ontxtChargesParamedicCodeChanged(value);
            });
        });

        $('#<%=txtChargesParamedicCode.ClientID %>').live('change', function () {
            ontxtChargesParamedicCodeChanged($(this).val());
        });

        function ontxtChargesParamedicCodeChanged(value) {
            var filterExpression = getChargesParamedicMasterFilterExpression() + " AND ParamedicCode = '" + value + "'";
            Methods.getObject('GetParamedicMasterList', filterExpression, function (result) {
                if (result != null) {
                    $('#<%=hdnChargesParamedicID.ClientID %>').val(result.ParamedicID);
                    $('#<%=txtChargesParamedicCode.ClientID %>').val(result.ParamedicCode);
                    $('#<%=txtChargesParamedicName.ClientID %>').val(result.FullName);
                }
                else {
                    $('#<%=hdnChargesParamedicID.ClientID %>').val('');
                    $('#<%=txtChargesParamedicCode.ClientID %>').val('');
                    $('#<%=txtChargesParamedicName.ClientID %>').val('');
                }
            });
        }
        //#endregion

        //#region Item Type
        function onCboGCItemTypeChanged() {
            $('#<%=hdnCboGCItemType.ClientID %>').val(cboGCItemType.GetValue());
        }
        //#endregion

        $('#btnRefresh').live('click', function () {
            cbpView.PerformCallback('refresh');
        });

        //#endregion

        function onAfterCustomClickSuccess(type) {
            cbpView.PerformCallback('refresh');
        }

    </script>
    <input type="hidden" value="" id="hdnSelectedTransactionDtID" runat="server" />
    <input type="hidden" value="" id="hdnSelectedDiscountAmount" runat="server" />
    <input type="hidden" value="" id="hdnSelectedDiscountComp1" runat="server" />
    <input type="hidden" value="" id="hdnSelectedDiscountComp2" runat="server" />
    <input type="hidden" value="" id="hdnSelectedDiscountComp3" runat="server" />
    <input type="hidden" value="" id="hdnSelectedPayerAmount" runat="server" />
    <input type="hidden" value="" id="hdnSelectedPatientAmount" runat="server" />
    <input type="hidden" value="" id="hdnSelectedLineAmount" runat="server" />
    <input type="hidden" value="" id="hdnCboChargesDepartmentID" runat="server" />
    <input type="hidden" value="" id="hdnCboGCItemType" runat="server" />
    <input type="hidden" value="" id="hdnFilterChargesHealthcareServiceUnitID" runat="server" />
    <div>
        <table class="tblContentArea" style="width: 100%; vertical-align: top">
            <colgroup>
                <col style="width: 50%" />
                <col style="width: 50%" />
            </colgroup>
            <tr>
                <td style="vertical-align: top">
                    <table class="tblContentArea" style="width: 100%; vertical-align: top">
                        <colgroup>
                            <col style="width: 120px" />
                            <col style="width: 100px" />
                            <col style="width: 350px" />
                            <col />
                        </colgroup>
                        <tr id="trDepartment" runat="server">
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Department")%></label>
                            </td>
                            <td colspan="2">
                                <dxe:ASPxComboBox ID="cboDepartment" ClientInstanceName="cboDepartment" runat="server"
                                    Width="250px">
                                    <ClientSideEvents ValueChanged="function(s,e) { onCboDepartmentChanged(); }" />
                                </dxe:ASPxComboBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <input type="hidden" id="hdnChargesHealthcareServiceUnitID" value="" runat="server" />
                                <label class="lblLink lblNormal" id="lblChargesHealthcareServiceUnit">
                                    <%=GetLabel("Unit Pelayanan")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtChargesServiceUnitCode" Width="100%" runat="server" />
                            </td>
                            <td>
                                <asp:TextBox ID="txtChargesServiceUnitName" ReadOnly="true" Width="100%" runat="server" />
                            </td>
                            <td>
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <input type="hidden" id="hdnChargesParamedicID" value="" runat="server" />
                                <label class="lblLink lblNormal" id="lblChargesParamedic">
                                    <%=GetLabel("Dokter/Paramedis")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtChargesParamedicCode" Width="100%" runat="server" />
                            </td>
                            <td>
                                <asp:TextBox ID="txtChargesParamedicName" ReadOnly="true" Width="100%" runat="server" />
                            </td>
                            <td>
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Jenis Item")%></label>
                            </td>
                            <td colspan="2">
                                <dxe:ASPxComboBox ID="cboGCItemType" ClientInstanceName="cboGCItemType" runat="server"
                                    Width="250px">
                                    <ClientSideEvents ValueChanged="function(s,e) { onCboGCItemTypeChanged(); }" />
                                </dxe:ASPxComboBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Item JasMed")%></label>
                            </td>
                            <td colspan="2">
                                <asp:CheckBox ID="chkIsHasRevenueSharing" CssClass="chkIsHasRevenueSharing" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                            </td>
                            <td>
                                <input type="button" id="btnRefresh" value="R e f r e s h" class="btnRefresh w3-button w3-blue w3-border w3-border-blue w3-round-large" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
        <table class="tblContentArea" style="width: 100%">
            <tr>
                <td>
                    <div style="padding: 5px; min-height: 150px;">
                        <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
                            ShowLoadingPanel="false" OnCallback="cbpView_Callback">
                            <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e) { hideLoadingPanel(); }" />
                            <PanelCollection>
                                <dx:PanelContent ID="PanelContent1" runat="server">
                                    <asp:Panel runat="server" ID="pnlService" Style="width: 100%; margin-left: auto;
                                        margin-right: auto; position: relative; font-size: 0.95em;">
                                        <asp:ListView ID="lvwView" runat="server">
                                            <EmptyDataTemplate>
                                                <table id="tblView" runat="server" class="grdNormal notAllowSelect" cellspacing="0"
                                                    rules="all">
                                                    <tr>
                                                        <th style="width: 180px" rowspan="2" align="left">
                                                            <div style="padding: 3px; float: left">
                                                                <div>
                                                                    <%= GetLabel("Transaksi")%></div>
                                                            </div>
                                                        </th>
                                                        <th rowspan="2" align="left">
                                                            <div style="padding: 3px; float: left">
                                                                <div>
                                                                    <%= GetLabel("Item")%></div>
                                                            </div>
                                                        </th>
                                                        <th style="width: 30px" rowspan="2" align="right">
                                                            <div style="padding: 3px; float: right">
                                                                <div>
                                                                    <%= GetLabel("Jumlah")%></div>
                                                            </div>
                                                        </th>
                                                        <th colspan="3">
                                                            <%=GetLabel("HARGA")%>
                                                        </th>
                                                        <th colspan="3">
                                                            <%=GetLabel("TOTAL")%>
                                                        </th>
                                                    </tr>
                                                    <tr>
                                                        <th style="width: 120px">
                                                            <div style="text-align: right; padding-right: 3px">
                                                                <%=GetLabel("Harga")%>
                                                            </div>
                                                        </th>
                                                        <th style="width: 80px">
                                                            <div style="text-align: right; padding-right: 3px">
                                                                <%=GetLabel("CITO")%>
                                                            </div>
                                                        </th>
                                                        <th style="width: 120px">
                                                            <div style="text-align: right; padding-right: 3px">
                                                                <%=GetLabel("Diskon")%>
                                                            </div>
                                                        </th>
                                                        <th style="width: 120px">
                                                            <div style="text-align: right; padding-right: 3px">
                                                                <%=GetLabel("Instansi")%>
                                                            </div>
                                                        </th>
                                                        <th style="width: 120px">
                                                            <div style="text-align: right; padding-right: 3px">
                                                                <%=GetLabel("Pasien")%>
                                                            </div>
                                                        </th>
                                                        <th style="width: 120px">
                                                            <div style="text-align: right; padding-right: 3px">
                                                                <%=GetLabel("Total")%>
                                                            </div>
                                                        </th>
                                                    </tr>
                                                    <tr class="trEmpty">
                                                        <td colspan="25">
                                                            <%=GetLabel("No Data To Display") %>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </EmptyDataTemplate>
                                            <LayoutTemplate>
                                                <table id="tblView" runat="server" class="grdNormal notAllowSelect" cellspacing="0"
                                                    rules="all">
                                                    <tr>
                                                        <th style="width: 180px" rowspan="2" align="left">
                                                            <div style="padding: 3px; float: left">
                                                                <div>
                                                                    <%= GetLabel("Transaksi")%></div>
                                                            </div>
                                                        </th>
                                                        <th rowspan="2" align="left">
                                                            <div style="padding: 3px; float: left">
                                                                <div>
                                                                    <%= GetLabel("Item")%></div>
                                                            </div>
                                                        </th>
                                                        <th style="width: 30px" rowspan="2" align="right">
                                                            <div style="padding: 3px; float: right">
                                                                <div>
                                                                    <%= GetLabel("Jumlah")%></div>
                                                            </div>
                                                        </th>
                                                        <th colspan="3">
                                                            <%=GetLabel("HARGA")%>
                                                        </th>
                                                        <th colspan="3">
                                                            <%=GetLabel("TOTAL")%>
                                                        </th>
                                                    </tr>
                                                    <tr>
                                                        <th style="width: 120px">
                                                            <div style="text-align: right; padding-right: 3px">
                                                                <%=GetLabel("Harga")%>
                                                            </div>
                                                        </th>
                                                        <th style="width: 80px">
                                                            <div style="text-align: right; padding-right: 3px">
                                                                <%=GetLabel("CITO")%>
                                                            </div>
                                                        </th>
                                                        <th style="width: 120px">
                                                            <div style="text-align: right; padding-right: 3px">
                                                                <%=GetLabel("Diskon")%>
                                                            </div>
                                                        </th>
                                                        <th style="width: 120px">
                                                            <div style="text-align: right; padding-right: 3px">
                                                                <%=GetLabel("Instansi")%>
                                                            </div>
                                                        </th>
                                                        <th style="width: 120px">
                                                            <div style="text-align: right; padding-right: 3px">
                                                                <%=GetLabel("Pasien")%>
                                                            </div>
                                                        </th>
                                                        <th style="width: 120px">
                                                            <div style="text-align: right; padding-right: 3px">
                                                                <%=GetLabel("Total")%>
                                                            </div>
                                                        </th>
                                                    </tr>
                                                    <tr runat="server" id="itemPlaceholder">
                                                    </tr>
                                                </table>
                                            </LayoutTemplate>
                                            <ItemTemplate>
                                                <tr style="<%#: Eval("GCTransactionStatus").ToString() == "X121^001" ? "background-color:#FFE4E1" : ""%>">
                                                    <td>
                                                        <div style="padding: 3px; text-align: left;">
                                                            <b>
                                                                <%#: Eval("TransactionNo")%></b></div>
                                                        <div style="padding: 3px; text-align: left; font-size: smaller; font-style: oblique">
                                                            <%#: Eval("TransactionDateInString")%></div>
                                                        <div style="padding: 3px; text-align: left; font-size: smaller; font-style: oblique">
                                                            <%#: Eval("ChargesServiceUnitName")%></div>
                                                        <div style="padding: 3px; text-align: left; font-size: smaller;">
                                                            <%=GetLabel("Status : ")%><%#: Eval("TransactionDetailStatus")%></div>
                                                        <div style="padding: 3px; text-align: left; font-size: smaller;">
                                                            <%=GetLabel("Billing : ")%><%#: Eval("PatientBillingNo")%></div>
                                                    </td>
                                                    <td>
                                                        <div style="padding: 3px; text-align: left;">
                                                            <b>
                                                                <%#: Eval("ItemName1")%></b></div>
                                                        <div style="padding: 3px; text-align: left; font-size: smaller; font-style: italic">
                                                            <%#: Eval("ItemCode")%></div>
                                                        <div style="padding: 3px; text-align: left;">
                                                            <%#: Eval("ParamedicName")%></div>
                                                        <div>
                                                            <img class="imgIsEditPackageItem imgLink" <%# Eval("IsPackageItem").ToString() == "True" ?  "" : "style='display:none'" %>
                                                                title='<%=GetLabel("Info Paket")%>' src='<%# Eval("IsPackageItem").ToString() == "True" ? ResolveUrl("~/Libs/Images/Button/package.png") : ResolveUrl("~/Libs/Images/Button/package_disabled.png")%>'
                                                                alt="" />
                                                        </div>
                                                    </td>
                                                    <td>
                                                        <div style="padding: 3px; text-align: right;">
                                                            <%#: Eval("ChargedQuantity")%>
                                                            <%#: Eval("ItemUnit")%>
                                                        </div>
                                                    </td>
                                                    <td>
                                                        <div style="padding: 3px; text-align: right;">
                                                            <b>
                                                                <%#: Eval("Tariff", "{0:N2}")%></b></div>
                                                        <div style="padding: 3px; text-align: left; font-size: x-small">
                                                            <i>
                                                                <%=GetLabel("c1 : ")%></i><%#: Eval("TariffComp1", "{0:N2}")%></div>
                                                        <div style="padding: 3px; text-align: left; font-size: x-small">
                                                            <i>
                                                                <%=GetLabel("c2 : ")%></i><%#: Eval("TariffComp2", "{0:N2}")%></div>
                                                        <div style="padding: 3px; text-align: left; font-size: x-small">
                                                            <i>
                                                                <%=GetLabel("c3 : ")%></i><%#: Eval("TariffComp3", "{0:N2}")%></div>
                                                    </td>
                                                    <td>
                                                        <div style="padding: 3px; text-align: right;">
                                                            <%#: Eval("CITOAmount", "{0:N2}")%></div>
                                                    </td>
                                                    <td>
                                                        <div style="padding: 3px; text-align: right;">
                                                            <b>
                                                                <%#: Eval("DiscountAmount", "{0:N2}")%></b></div>
                                                        <div style="padding: 3px; text-align: left; font-size: x-small">
                                                            <i>
                                                                <%=GetLabel("c1 : ")%></i><%#: Eval("DiscountComp1", "{0:N2}")%></div>
                                                        <div style="padding: 3px; text-align: left; font-size: x-small">
                                                            <i>
                                                                <%=GetLabel("c2 : ")%></i><%#: Eval("DiscountComp2", "{0:N2}")%></div>
                                                        <div style="padding: 3px; text-align: left; font-size: x-small">
                                                            <i>
                                                                <%=GetLabel("c3 : ")%></i><%#: Eval("DiscountComp3", "{0:N2}")%></div>
                                                    </td>
                                                    <td>
                                                         <div style="padding: 3px; text-align: right;">
                                                            <%#: Eval("PayerAmount", "{0:N2}")%></div>
                                                    </td>
                                                    <td>
                                                       <div style="padding: 3px; text-align: right;">
                                                            <%#: Eval("PatientAmount", "{0:N2}")%></div>
                                                    </td>
                                                    <td>
                                                        <div style="padding: 3px; text-align: right;">
                                                            <%#: Eval("LineAmount", "{0:N2}")%></div>
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
