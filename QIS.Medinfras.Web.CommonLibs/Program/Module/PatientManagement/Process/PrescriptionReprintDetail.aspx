<%@ Page Title="" Language="C#" MasterPageFile="~/Libs/MasterPage/PatientPage/MPBasePatientPageTrx.master"
    AutoEventWireup="true" CodeBehind="PrescriptionReprintDetail.aspx.cs" Inherits="QIS.Medinfras.Web.CommonLibs.Program.PrescriptionReprintDetail" %>

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
    <li id="btnPrintPrescription" runat="server" crudmode="R">
        <img src='<%=ResolveUrl("~/Libs/Images/Icon/tbprint.png")%>' alt="" /><div>
            <%=GetLabel("Print Etiket")%></div>
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
            var url = "~/Libs/Program/Module/PatientManagement/Process/PrescriptionReprintList.aspx";
            document.location = ResolveUrl(url);
        });

        $('#<%=btnPrintPrescription.ClientID %>').live('click', function () {
            var prescriptionOrderID = $('#<%=hdnPrescriptionOrderID.ClientID %>').val();
            var transactionNo = $('#<%=txtTransactionNo.ClientID %>').val();
            var date = $('#<%=txtPrescriptionDate.ClientID %>').val();
            var time = $('#<%=txtPrescriptionTime.ClientID %>').val();
            var prescriptionType = $('#<%=hdnGCPrescriptionType.ClientID %>').val();
            if (prescriptionOrderID != "") {
                var param = prescriptionOrderID + '|' + transactionNo + '|' + date + '|' + time + '|' + prescriptionType;
                var url = ResolveUrl("~/Program/Prescription/PrescriptionEntry/PrintPrescriptionList.ascx");
                openUserControlPopup(url, param, 'Cetak Etiket Obat', 800, 600);
            }
            else showToast('Warning', 'Belum ada transaksi resep yang dientry');
        });

        $(function () {
            //#region Transaction No
            $('#lblPrescriptionNo.lblLink').live('click', function () {
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
            if (code == 'sendOrderToRIS' || code == 'changeDetailRemarks' || code == 'inputSerialNumber' || code == 'inputFixedAsset') {
                return $('#<%:hdnTransactionID.ClientID %>').val();
            }
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
    <input type="hidden" value="" id="hdnPrescriptionOrderID" runat="server" />
    <input type="hidden" value="" id="hdnTransactionID" runat="server" />
    <input type="hidden" value="" id="hdnHsuImaging" runat="server" />
    <input type="hidden" value="" id="hdnHsuLaboratory" runat="server" />
    <input type="hidden" value="" id="hdnDefaultPrescriptionOrderID" runat="server" />
    <input type="hidden" value="" id="hdnDispensaryServiceUnitID" runat="server" />
    <input type="hidden" value="" id="hdnGCPrescriptionType" runat="server" />
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
                                <label class="lblLink" id="lblPrescriptionNo">
                                    <%=GetLabel("No. Resep")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtTransactionNo" Width="150px" ReadOnly="true" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal lblMandatory" />
                                <%=GetLabel("Tanggal ") %>
                                -
                                <%=GetLabel("Jam ") %>
                            </td>
                            <td>
                                <table cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td style="padding-right: 1px; width: 145px">
                                            <asp:TextBox ID="txtPrescriptionDate" ReadOnly="true" Width="120px" CssClass="datepicker"
                                                runat="server" />
                                        </td>
                                        <td style="width: 5px">
                                            &nbsp;
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtPrescriptionTime" ReadOnly="true" Width="80px" CssClass="time"
                                                runat="server" Style="text-align: center" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblMandatory" runat="server" id="lblPhysician">
                                    <%=GetLabel("Dokter ")%></label>
                            </td>
                            <td>
                                <table style="width: 100%" cellpadding="0" cellspacing="0">
                                    <colgroup>
                                        <col style="width: 120px" />
                                        <col style="width: 3px" />
                                        <col />
                                    </colgroup>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtPhysicianCode" ReadOnly="true" Width="100%" runat="server" />
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtPhysicianName" ReadOnly="true" Width="100%" runat="server" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblMandatory">
                                    <%=GetLabel("Jenis Resep")%></label>
                            </td>
                            <td>
                                <dxe:ASPxComboBox ID="cboPrescriptionType" ClientInstanceName="cboPrescriptionType"
                                    Width="233px" runat="server">
                                </dxe:ASPxComboBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblNormal" id="lblReferenceNo">
                                    <%=GetLabel("Nomor Referensi")%></label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtReferenceNo" ReadOnly="true" Width="100%" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel">
                                <label class="lblMandatory">
                                    <%=GetLabel("Jenis Transaksi BPJS")%></label>
                            </td>
                            <td>
                                <dxe:ASPxComboBox ID="cboBPJSTransType" ClientInstanceName="cboBPJSTransType" Width="233px"
                                    runat="server">
                                </dxe:ASPxComboBox>
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
                                <asp:TextBox ID="txtPrescriptionOrderInfo" ReadOnly="true" Width="100%" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tdLabel" valign="top" style="padding-top: 5px;">
                                <label>
                                    <%=GetLabel("Catatan Order")%></label>
                            </td>
                            <td colspan="5">
                                <asp:TextBox ID="txtNotes" ReadOnly="true" Width="100%" TextMode="MultiLine" Height="110px"
                                    runat="server" />
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
                                        <asp:ListView ID="lvwView" runat="server">
                                            <EmptyDataTemplate>
                                                <table id="tblView" runat="server" class="grdNormal notAllowSelect" cellspacing="0"
                                                    rules="all">
                                                    <tr>
                                                        <th rowspan="2" align="left">
                                                            <div>
                                                                <%=GetLabel("Obat")%>
                                                                -
                                                                <%=GetLabel("Kadar")%>
                                                                -
                                                                <%=GetLabel("Bentuk")%></div>
                                                            <div>
                                                                <div style="color: Blue; width: 35px; float: left;">
                                                                    <%=GetLabel("DOSIS")%>
                                                                </div>
                                                                <%=GetLabel("Jumlah")%>
                                                                -
                                                                <%=GetLabel("Rute")%>
                                                                -
                                                                <%=GetLabel("Aturan Pemakaian")%></div>
                                                        </th>
                                                        <th rowspan="2" align="right" style="padding: 3px; width: 80px;">
                                                            <div>
                                                                <%=GetLabel("Harga Satuan")%></div>
                                                        </th>
                                                        <th rowspan="2" align="right" style="padding: 3px; width: 80px;">
                                                            <div>
                                                                <%=GetLabel("Jasa R/")%></div>
                                                        </th>
                                                        <th rowspan="2" align="right" style="padding: 3px; width: 80px;">
                                                            <div>
                                                                <%=GetLabel("Embalase")%></div>
                                                        </th>
                                                        <th rowspan="2" align="right" style="padding: 3px; width: 80px;">
                                                            <div>
                                                                <%=GetLabel("Jumlah")%></div>
                                                        </th>
                                                        <th colspan="3">
                                                            <div>
                                                                <%=GetLabel("TOTAL") %></div>
                                                        </th>
                                                        <th rowspan="2" style="width: 80px">
                                                            <div style="text-align: right; padding-right: 3px">
                                                                <%=GetLabel("Petugas")%>
                                                            </div>
                                                        </th>
                                                    </tr>
                                                    <tr>
                                                        <th style="width: 120px;">
                                                            <div>
                                                                <%=GetLabel("Instansi") %></div>
                                                        </th>
                                                        <th style="width: 120px;">
                                                            <div>
                                                                <%=GetLabel("Pasien") %></div>
                                                        </th>
                                                        <th style="width: 120px;">
                                                            <div>
                                                                <%=GetLabel("Total") %></div>
                                                        </th>
                                                    </tr>
                                                    <tr align="center" style="height: 50px; vertical-align: middle;">
                                                        <td colspan="10">
                                                        </td>
                                                    </tr>
                                                </table>
                                            </EmptyDataTemplate>
                                            <LayoutTemplate>
                                                <table id="tblView" runat="server" class="grdNormal notAllowSelect" cellspacing="0"
                                                    rules="all">
                                                    <tr>
                                                        <th rowspan="2" align="left">
                                                            <div>
                                                                <%=GetLabel("Obat")%>
                                                                -
                                                                <%=GetLabel("Kadar")%>
                                                                -
                                                                <%=GetLabel("Bentuk")%></div>
                                                            <div>
                                                                <div style="color: Blue; width: 35px; float: left;">
                                                                    <%=GetLabel("DOSIS")%></div>
                                                                <%=GetLabel("Jumlah")%>
                                                                -
                                                                <%=GetLabel("Rute")%>
                                                                -
                                                                <%=GetLabel("Aturan Pemakaian")%></div>
                                                        </th>
                                                        <th rowspan="2" align="right" style="padding: 3px; width: 80px;">
                                                            <div>
                                                                <%=GetLabel("Harga Satuan")%></div>
                                                        </th>
                                                        <th rowspan="2" align="right" style="padding: 3px; width: 80px;">
                                                            <div>
                                                                <%=GetLabel("Jasa R/")%></div>
                                                        </th>
                                                        <th rowspan="2" align="right" style="padding: 3px; width: 80px;">
                                                            <div>
                                                                <%=GetLabel("Embalase")%></div>
                                                        </th>
                                                        <th rowspan="2" align="right" style="padding: 3px; width: 80px;">
                                                            <div>
                                                                <%=GetLabel("Jumlah")%></div>
                                                        </th>
                                                        <th colspan="3">
                                                            <div>
                                                                <%=GetLabel("TOTAL") %></div>
                                                        </th>
                                                        <th rowspan="2" style="width: 80px">
                                                            <div style="text-align: right; padding-right: 3px">
                                                                <%=GetLabel("Petugas")%>
                                                            </div>
                                                        </th>
                                                    </tr>
                                                    <tr>
                                                        <th style="width: 100px;">
                                                            <div>
                                                                <%=GetLabel("INSTANSI") %></div>
                                                        </th>
                                                        <th style="width: 100px;">
                                                            <div>
                                                                <%=GetLabel("PASIEN") %></div>
                                                        </th>
                                                        <th style="width: 100px;">
                                                            <div>
                                                                <%=GetLabel("TOTAL") %></div>
                                                        </th>
                                                    </tr>
                                                    <tr runat="server" id="itemPlaceholder">
                                                    </tr>
                                                    <tr class="trFooter">
                                                        <td colspan="5">
                                                            <div style="text-align: right; padding: 0px 3px">
                                                                <%=GetLabel("Total")%>
                                                            </div>
                                                        </td>
                                                        <td>
                                                            <div style="text-align: right; padding: 0px 3px" id="tdTotalAllPayer" runat="server">
                                                                Instansi
                                                            </div>
                                                        </td>
                                                        <td>
                                                            <div style="text-align: right; padding: 0px 3px" id="tdTotalAllPatient" runat="server">
                                                                Pasien
                                                            </div>
                                                        </td>
                                                        <td>
                                                            <div style="text-align: right; padding: 0px 3px" id="tdTotalAll" runat="server">
                                                                Total
                                                            </div>
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
                                                        <input type="hidden" value="<%#:Eval("ID") %>" bindingfield="ID" />
                                                        <input type="hidden" value="<%#:Eval("SignaID") %>" bindingfield="SignaID" />
                                                        <input type="hidden" value="<%#:Eval("SignaLabel") %>" bindingfield="SignaLabel" />
                                                        <input type="hidden" value="<%#:Eval("SignaName1") %>" bindingfield="SignaName1" />
                                                        <input type="hidden" value="<%#:Eval("PrescriptionOrderDetailID") %>" bindingfield="PrescriptionOrderDetailID" />
                                                        <input type="hidden" value="<%#:Eval("GenericName") %>" bindingfield="GenericName" />
                                                        <input type="hidden" value="<%#:Eval("IsRFlag") %>" bindingfield="IsRFlag" />
                                                        <input type="hidden" value="<%#:Eval("IsNonMaster") %>" bindingfield="IsNonMaster" />
                                                        <input type="hidden" value="<%#:Eval("IsCompound") %>" bindingfield="IsCompound" />
                                                        <input type="hidden" value="<%#:Eval("ItemCode") %>" bindingfield="ItemCode" />
                                                        <input type="hidden" value="<%#:Eval("ItemID") %>" bindingfield="ItemID" />
                                                        <input type="hidden" value="<%#:Eval("LocationID") %>" bindingfield="LocationID" />
                                                        <input type="hidden" value="<%#:Eval("GCItemUnit") %>" bindingfield="GCItemUnit" />
                                                        <input type="hidden" value="<%#:Eval("ItemUnit") %>" bindingfield="ItemUnit" />
                                                        <input type="hidden" value="<%#:Eval("GCBaseUnit") %>" bindingfield="GCBaseUnit" />
                                                        <input type="hidden" value="<%#:Eval("DrugName") %>" bindingfield="DrugName" />
                                                        <input type="hidden" value="<%#:Eval("GCCoenamRule") %>" bindingfield="GCCoenamRule" />
                                                        <input type="hidden" value="<%#:Eval("GCDrugForm") %>" bindingfield="GCDrugForm" />
                                                        <input type="hidden" value="<%#:Eval("Dose") %>" bindingfield="Dose" />
                                                        <input type="hidden" value="<%#:Eval("GCDoseUnit") %>" bindingfield="GCDoseUnit" />
                                                        <input type="hidden" value="<%#:Eval("DoseUnit") %>" bindingfield="DoseUnit" />
                                                        <input type="hidden" value="<%#:Eval("GCDosingFrequency") %>" bindingfield="GCDosingFrequency" />
                                                        <input type="hidden" value="<%#:Eval("Frequency") %>" bindingfield="Frequency" />
                                                        <input type="hidden" value="<%#:Eval("NumberOfDosage") %>" bindingfield="NumberOfDosage" />
                                                        <input type="hidden" value="<%#:Eval("GCDosingUnit") %>" bindingfield="GCDosingUnit" />
                                                        <input type="hidden" value="<%#:Eval("DosingDuration") %>" bindingfield="DosingDuration" />
                                                        <input type="hidden" value="<%#:Eval("GCRoute") %>" bindingfield="GCRoute" />
                                                        <input type="hidden" value="<%#:Eval("MedicationPurpose") %>" bindingfield="MedicationPurpose" />
                                                        <input type="hidden" value="<%#:Eval("StartDate") %>" bindingfield="StartDate" />
                                                        <input type="hidden" value="<%#:Eval("StartTime") %>" bindingfield="StartTime" />
                                                        <input type="hidden" value="<%#:Eval("DispenseQty") %>" bindingfield="DispenseQty" />
                                                        <input type="hidden" value="<%#:Eval("MedicationAdministration") %>" bindingfield="MedicationAdministration" />
                                                        <input type="hidden" value="<%#:Eval("cfStartDateInDatePickerFormat") %>" bindingfield="StartDateInDatePickerFormat" />
                                                        <input type="hidden" value="<%#:Eval("TakenQty") %>" bindingfield="TakenQty" />
                                                        <input type="hidden" value="<%#:Eval("ChargedQuantity") %>" bindingfield="ChargedQuantity" />
                                                        <input type="hidden" value="<%#:Eval("ChargeClassID") %>" bindingfield="ChargeClassID" />
                                                        <input type="hidden" value="<%#:Eval("Tariff") %>" bindingfield="Tariff" />
                                                        <input type="hidden" value="<%#:Eval("DiscountAmount") %>" bindingfield="DiscountAmount" />
                                                        <input type="hidden" value="<%#:Eval("PatientAmount") %>" bindingfield="PatientAmount" />
                                                        <input type="hidden" value="<%#:Eval("PayerAmount") %>" bindingfield="PayerAmount" />
                                                        <input type="hidden" value="<%#:Eval("LineAmount") %>" bindingfield="LineAmount" />
                                                        <input type="hidden" value="<%#:Eval("EmbalaceID") %>" bindingfield="EmbalaceID" />
                                                        <input type="hidden" value="<%#:Eval("EmbalaceName") %>" bindingfield="EmbalaceName" />
                                                        <input type="hidden" value="<%#:Eval("EmbalaceCode") %>" bindingfield="EmbalaceCode" />
                                                        <input type="hidden" value="<%#:Eval("EmbalaceQty") %>" bindingfield="EmbalaceQty" />
                                                        <input type="hidden" value="<%#:Eval("EmbalaceAmount") %>" bindingfield="EmbalaceAmount" />
                                                        <input type="hidden" value="<%#:Eval("PrescriptionFeeAmount") %>" bindingfield="PrescriptionFeeAmount" />
                                                        <input type="hidden" value="<%#:Eval("IsMorning") %>" bindingfield="IsMorning" />
                                                        <input type="hidden" value="<%#:Eval("IsNoon") %>" bindingfield="IsNoon" />
                                                        <input type="hidden" value="<%#:Eval("IsEvening") %>" bindingfield="IsEvening" />
                                                        <input type="hidden" value="<%#:Eval("IsNight") %>" bindingfield="IsNight" />
                                                        <input type="hidden" value="<%#:Eval("IsAsRequired") %>" bindingfield="IsAsRequired" />
                                                        <input type="hidden" value="<%#:Eval("IsIMM") %>" bindingfield="IsIMM" />
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <b>
                                                                        <div style="<%# Eval("GCDrugClass").ToString() == "X123^O" ? "color: Red": Eval("GCDrugClass").ToString() == "X123^P" ? "color: Blue" : "color: Black"%>">
                                                                            <%#: Eval("cfItemNameWithNumero")%></div>
                                                                    </b>
                                                                    <div style='<%# Eval("IsCompound").ToString() == "False" ? "display:none;": "white-space: pre-line;font-style:italic; padding-top:5px" %>'>
                                                                        <%#: Eval("cfCompoundDetail")%></div>
                                                                </td>
                                                                <td rowspan="2">
                                                                    &nbsp;
                                                                </td>
                                                                <td rowspan="2">
                                                                    <div>
                                                                        <img src='<%# ResolveUrl("~/Libs/Images/Misc/compound.png")%>' alt="" style='<%# Eval("IsCompound").ToString() == "False" ? "display:none;": "" %> max-width:30px;
                                                                            min-width: 30px; float: left;' /></div>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <div>
                                                                        <div style="color: Blue; width: 35px; float: left;">
                                                                            <%=GetLabel("DOSE")%></div>
                                                                        <%#: Eval("NumberOfDosage")%>
                                                                        <%#: Eval("DosingUnit")%>
                                                                        -
                                                                        <%#: Eval("Route")%>
                                                                        -
                                                                        <%#: Eval("cfDoseFrequency")%>
                                                                    </div>
                                                                    <div>
                                                                        <%#: Eval("MedicationAdministration")%>
                                                                    </div>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                    <td style="padding: 3px;" align="right">
                                                        <div style='<%# Eval("IsCompound").ToString() == "True" ? "display:none;": "" %>'>
                                                            <%#: Eval("Tariff", "{0:N}")%></div>
                                                    </td>
                                                    <td style="padding: 3px;" align="right">
                                                        <%#:Eval("PrescriptionFeeAmount", "{0:N}")%>
                                                    </td>
                                                    <td style="padding: 3px;" align="right">
                                                        <%#: Eval("EmbalaceAmount", "{0:N}")%>
                                                    </td>
                                                    <td style="padding: 3px;" align="right">
                                                        <div>
                                                            <%#: Eval("TakenQty")%>
                                                            <%#: Eval("cfItemUnit")%></div>
                                                    </td>
                                                    <td align="right">
                                                        <%#:Eval("PayerAmount", "{0:N}")%>
                                                    </td>
                                                    <td align="right">
                                                        <%#:Eval("PatientAmount", "{0:N}")%>
                                                    </td>
                                                    <td align="right">
                                                        <%#:Eval("LineAmount", "{0:N}")%>
                                                    </td>
                                                    <td>
                                                        <div style="padding-right: 3px; text-align: right;">
                                                            <div>
                                                                <%#: Eval("ChargesByUserName")%></div>
                                                        </div>
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
