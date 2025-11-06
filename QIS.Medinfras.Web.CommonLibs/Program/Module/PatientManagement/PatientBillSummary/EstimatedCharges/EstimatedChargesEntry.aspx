<%@ Page Language="C#" MasterPageFile="~/Libs/MasterPage/PatientPage/MPBasePatientPageTrx.Master"
    AutoEventWireup="true" CodeBehind="EstimatedChargesEntry.aspx.cs" Inherits="QIS.Medinfras.Web.CommonLibs.Program.EstimatedChargesEntry" %>

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
    <li id="btnProcess" crudmode="R" runat="server">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbprocess.png")%>' alt="" /><br style="clear: both" />
        <div>
            <%=GetLabel("Process")%></div>
    </li>
    <li id="btnCancel" crudmode="R" runat="server">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbvoid.png")%>' alt="" /><br style="clear: both" />
        <div>
            <%=GetLabel("Batal")%></div>
    </li>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="plhHeader" runat="server">
    <input type="hidden" value="" id="hdnRegistrationID" runat="server" />
    <input type="hidden" value="" id="hdnVisitID" runat="server" />
    <input type="hidden" value="" id="hdnChargesClassID" runat="server" />
    <input type="hidden" value="" id="hdnChargesClassIDSelected" runat="server" />
    <input type="hidden" value="" id="hdnLinkedRegistrationID" runat="server" />
    <input type="hidden" value="" id="hdnDepartmentID" runat="server" />
    <input type="hidden" value="" id="hdnBusinessPartnerID" runat="server" />
    <input type="hidden" value="" id="hdnPageTitle" runat="server" />
    <input type="hidden" id="hdnHealthcareServiceUnitID" runat="server" value="" />
    <input type="hidden" id="hdnImagingServiceUnitID" runat="server" value="" />
    <input type="hidden" id="hdnLaboratoryServiceUnitID" runat="server" value="" />
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="plhEntry" runat="server">
    <script type="text/javascript">
        function onLoad() {
            $('.txtCurrency').each(function () {
                $(this).trigger('changeValue');
            });
        }

        $('#<%=btnCancel.ClientID %>').live('click', function () {
            var transID = $('#<%=hdnTransactionID.ClientID %>').val();
            if (transID != '' && transID != '0') {
                showToastConfirmation("Lanjutkan Proses ? ", function (result) {
                    if (result) {
                        onCustomButtonClick('cancel');
                    }
                });
            }
            else {
                showToast('Warning', 'Harap Pilih no transaksi terlebih dahulu');
            }
        });

        $('#<%=btnProcess.ClientID %>').live('click', function () {
            var transID = $('#<%=hdnTransactionID.ClientID %>').val();
            var classID = $('#<%=hdnChargesClassID.ClientID %>').val();
            var classIDSelected = $('#<%=hdnChargesClassIDSelected.ClientID %>').val();
            if (transID != '' && transID != '0') {
                if (classID == classIDSelected) {
                    showToastConfirmation("Lanjutkan Proses ? ", function (result) {
                        if (result) {
                            onCustomButtonClick('process');
                        }
                    });
                }
                else {
                    showToast('Warning', 'Ada perbedaan kelas antara registrasi dengan transaksi persetujuan biaya');
                }
            }
            else {
                showToast('Warning', 'Harap Pilih no transaksi terlebih dahulu');
            }
        });

        //#region Filter

        //#region Transaction
        function getTransactionFilterExpression() {
            var regID = $('#<%=hdnRegistrationID.ClientID %>').val();
            var filterExpression = "RegistrationID = '" + regID + "' AND GCTransactionStatus = '" + Constant.TransactionStatus.APPROVED + "'";
            return filterExpression;
        }

        $('#lblTransactionNo.lblLink').live('click', function () {
            openSearchDialog('estimatedcharges', getTransactionFilterExpression(), function (value) {
                $('#<%=txtTransactionNo.ClientID %>').val(value);
                ontxtTransactionNoChanged(value);
            });
        });

        $('#<%=txtTransactionNo.ClientID %>').live('change', function () {
            ontxtTransactionNoChanged($(this).val());
        });

        function ontxtTransactionNoChanged(value) {
            var filterExpression = getTransactionFilterExpression() + " AND TransactionNo = '" + value + "'";
            Methods.getObject('GetvEstimatedChargesHdList', filterExpression, function (result) {
                if (result != null) {
                    $('#<%=hdnTransactionID.ClientID %>').val(result.ID);
                    $('#<%=txtTransactionNo.ClientID %>').val(result.TransactionNo);
                    $('#<%=txtRegistrationNo.ClientID %>').val(result.RegistrationNo);
                    $('#<%=txtMedicalNo.ClientID %>').val(result.MedicalNo);
                    $('#<%=txtPatientName.ClientID %>').val(result.PatientName);
                    $('#<%=txtServiceUnitName.ClientID %>').val(result.ServiceUnitName);
                    $('#<%=txtTransactionDate.ClientID %>').val(result.cfTransactionDateInString);
                    $('#<%=hdnChargesClassIDSelected.ClientID %>').val(result.ClassID);
                    $('#<%=txtClassName.ClientID %>').val(result.ClassName);
                    $('#<%=txtCustomerType.ClientID %>').val(result.CustomerTypeName);
                    $('#<%=txtBusinessPartnerCode.ClientID %>').val(result.BusinessPartnerCode);
                    $('#<%=txtBusinessPartnerName.ClientID %>').val(result.BusinessPartnerName);
                    $('#<%=txtCoverageTypeCode.ClientID %>').val(result.CoverageTypeCode);
                    $('#<%=txtCoverageTypeName.ClientID %>').val(result.CoverageTypeName);
                }
                else {
                    $('#<%=hdnTransactionID.ClientID %>').val('');
                    $('#<%=txtTransactionNo.ClientID %>').val('');
                    $('#<%=txtRegistrationNo.ClientID %>').val('');
                    $('#<%=txtMedicalNo.ClientID %>').val('');
                    $('#<%=txtPatientName.ClientID %>').val('');
                    $('#<%=txtServiceUnitName.ClientID %>').val('');
                    $('#<%=txtTransactionDate.ClientID %>').val('');
                    $('#<%=hdnChargesClassIDSelected.ClientID %>').val('');
                    $('#<%=txtClassName.ClientID %>').val('');
                    $('#<%=txtCustomerType.ClientID %>').val('');
                    $('#<%=txtBusinessPartnerCode.ClientID %>').val('');
                    $('#<%=txtBusinessPartnerName.ClientID %>').val('');
                    $('#<%=txtCoverageTypeCode.ClientID %>').val('');
                    $('#<%=txtCoverageTypeName.ClientID %>').val('');
                }
                cbpView.PerformCallback('refresh');
            });
        }
        //#endregion

        $('#btnRefresh').live('click', function () {
            cbpView.PerformCallback('refresh');
        });

        //#endregion

        function onAfterCustomClickSuccess(type) {
            $('#<%=hdnTransactionID.ClientID %>').val('');
            $('#<%=txtTransactionNo.ClientID %>').val('');
            $('#<%=txtRegistrationNo.ClientID %>').val('');
            $('#<%=txtMedicalNo.ClientID %>').val('');
            $('#<%=txtPatientName.ClientID %>').val('');
            $('#<%=txtServiceUnitName.ClientID %>').val('');
            $('#<%=txtTransactionDate.ClientID %>').val('');
            $('#<%=hdnChargesClassIDSelected.ClientID %>').val('');
            $('#<%=txtClassName.ClientID %>').val('');
            $('#<%=txtCustomerType.ClientID %>').val('');
            $('#<%=txtBusinessPartnerCode.ClientID %>').val('');
            $('#<%=txtBusinessPartnerName.ClientID %>').val('');
            $('#<%=txtCoverageTypeCode.ClientID %>').val('');
            $('#<%=txtCoverageTypeName.ClientID %>').val('');
            cbpView.PerformCallback('refresh');
        }
    </script>
    <input type="hidden" value="" id="hdnSelectedTransactionDtID" runat="server" />
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
                            <col style="width: 180px" />
                            <col style="width: 100px" />
                            <col style="width: 350px" />
                            <col />
                        </colgroup>
                        <tr>
                            <td class="tdLabel">
                                <input type="hidden" id="hdnTransactionID" value="" runat="server" />
                                <label class="lblLink lblMandatory" id="lblTransactionNo">
                                    <%=GetLabel("No. Transaksi")%></label>
                            </td>
                            <td colspan="2">
                                <asp:TextBox ID="txtTransactionNo" Width="100%" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label id="lblRegNo" class="lblMandatory" runat="server">
                                    <%=GetLabel("No. Registrasi")%></label>
                            </td>
                            <td colspan="2">
                                <asp:TextBox ID="txtRegistrationNo" Width="100%" ReadOnly="true" CssClass="required"
                                    ValidationGroup="mpEntry" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Pasien")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtMedicalNo" ReadOnly="true" Width="100%" runat="server" />
                            </td>
                            <td>
                                <asp:TextBox ID="txtPatientName" ReadOnly="true" Width="100%" runat="server" />
                            </td>
                            <td>
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal">
                                    <%=GetLabel("Unit Pelayanan")%></label>
                            </td>
                            <td colspan="2">
                                <asp:TextBox ID="txtServiceUnitName" ReadOnly="true" Width="100%" runat="server" />
                            </td>
                        </tr>
                        <tr style='display: none'>
                            <td>
                            </td>
                            <td>
                                <input type="button" id="btnRefresh" value="R e f r e s h" class="btnRefresh w3-button w3-blue w3-border w3-border-blue w3-round-large" />
                            </td>
                        </tr>
                    </table>
                </td>
                <td style="vertical-align: top">
                    <table class="tblContentArea" style="width: 100%; vertical-align: top">
                        <colgroup>
                            <col style="width: 80px" />
                            <col style="width: 100px" />
                            <col style="width: 300px" />
                            <col style="width: 10px" />
                            <col />
                        </colgroup>
                        <tr>
                            <td class="tdLabel">
                                <%=GetLabel("Tanggal") %>
                            </td>
                            <td colspan="2">
                                <asp:TextBox ID="txtTransactionDate" Width="25%" ReadOnly="true" CssClass="datepicker"
                                    runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <%=GetLabel("Kelas Tagihan") %>
                            </td>
                            <td colspan="2">
                                <asp:TextBox ID="txtClassName" Width="100%" ReadOnly="true" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblMandatory">
                                    <%=GetLabel("Pembayar") %>
                                </label>
                            </td>
                            <td colspan="2">
                                <asp:TextBox ID="txtCustomerType" Width="100%" ReadOnly="true" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal" runat="server">
                                    <%:GetLabel("Instansi")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtBusinessPartnerCode" ReadOnly="true" Width="100%" runat="server" />
                            </td>
                            <td>
                                <asp:TextBox ID="txtBusinessPartnerName" ReadOnly="true" Width="100%" runat="server" />
                            </td>
                            <td>
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal" runat="server">
                                    <%:GetLabel("Tipe Jaminan")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtCoverageTypeCode" ReadOnly="true" Width="100%" runat="server" />
                            </td>
                            <td>
                                <asp:TextBox ID="txtCoverageTypeName" ReadOnly="true" Width="100%" runat="server" />
                            </td>
                            <td>
                                &nbsp;
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
                                        <asp:ListView ID="lvwView" runat="server" OnItemDataBound="lvwView_ItemDataBound">
                                            <EmptyDataTemplate>
                                                <table id="tblView" runat="server" class="grdNormal notAllowSelect" cellspacing="0"
                                                    rules="all">
                                                    <tr>
                                                        <th align="left">
                                                            <div style="padding: 3px; float: left">
                                                                <div>
                                                                    <%= GetLabel("Nama Barang")%></div>
                                                            </div>
                                                        </th>
                                                        <th style="width: 30px" align="right">
                                                            <div style="padding: 3px; float: right">
                                                                <div>
                                                                    <%= GetLabel("Quantity")%></div>
                                                            </div>
                                                        </th>
                                                        <th style="width: 120px">
                                                            <div style="text-align: right; padding-right: 3px">
                                                                <%=GetLabel("Harga / satuan")%>
                                                            </div>
                                                        </th>
                                                        <th style="width: 100px">
                                                            <div style="text-align: right; padding-right: 3px">
                                                                <%=GetLabel("Instansi")%>
                                                            </div>
                                                        </th>
                                                        <th style="width: 100px">
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
                                                        <th align="left">
                                                            <div style="padding: 3px; float: left">
                                                                <div>
                                                                    <%= GetLabel("Nama Barang")%></div>
                                                            </div>
                                                        </th>
                                                        <th style="width: 30px" align="right">
                                                            <div style="padding: 3px; float: right">
                                                                <div>
                                                                    <%= GetLabel("Quantity")%></div>
                                                            </div>
                                                        </th>
                                                        <th style="width: 120px">
                                                            <div style="text-align: right; padding-right: 3px">
                                                                <%=GetLabel("Harga / satuan")%>
                                                            </div>
                                                        </th>
                                                        <th style="width: 100px">
                                                            <div style="text-align: right; padding-right: 3px">
                                                                <%=GetLabel("Instansi")%>
                                                            </div>
                                                        </th>
                                                        <th style="width: 100px">
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
                                                <tr>
                                                    <td>
                                                        <input type="hidden" class="keyField" value="<%#: Eval("ID")%>" />
                                                        <input type="hidden" class="TransactionID" value="<%#: Eval("EstimatedChargesHdID")%>" />
                                                        <input type="hidden" class="ItemID" value="<%#: Eval("ItemID")%>" />
                                                        <input type="hidden" class="Tariff" value="<%#: Eval("Tariff")%>" />
                                                        <input type="hidden" class="TariffComp1" value="<%#: Eval("TariffComp1")%>" />
                                                        <input type="hidden" class="TariffComp2" value="<%#: Eval("TariffComp2")%>" />
                                                        <input type="hidden" class="TariffComp3" value="<%#: Eval("TariffComp3")%>" />
                                                        <input type="hidden" class="PayerAmount" value="<%#: Eval("PayerAmount")%>" />
                                                        <input type="hidden" class="PatientAmount" value="<%#: Eval("PatientAmount")%>" />
                                                        <input type="hidden" class="LineAmount" value="<%#: Eval("LineAmount")%>" />
                                                        <input type="hidden" class="ChargedQuantity" value="<%#: Eval("Qty")%>" />
                                                        <div style='display: none'>
                                                            <asp:CheckBox ID="chkIsSelected" CssClass="chkIsSelected" runat="server" />
                                                        </div>
                                                        <div style="padding: 3px; text-align: left;">
                                                            <b>
                                                                <%#: Eval("ItemName1")%></b></div>
                                                        <div style="padding: 3px; text-align: left; font-size: smaller; font-style: italic">
                                                            <%#: Eval("ItemCode")%></div>
                                                    </td>
                                                    <td>
                                                        <div style="padding: 3px; text-align: right;">
                                                            <%#: Eval("Qty")%>
                                                        </div>
                                                    </td>
                                                    <td>
                                                        <div style="padding: 3px; text-align: right;">
                                                            <%#: Eval("Tariff", "{0:N2}")%></div>
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
