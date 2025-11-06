<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ChangeSignaCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.Pharmacy.Program.ChangeSignaCtl" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<script type="text/javascript" id="dxss_patientvisitnotesctl">
    $('#btnCancelCtl').click(function () {
        $('#containerEntryDataCtl').hide();
    });

    $('#btnSaveCtl').click(function (evt) {
        if (IsValid(evt, 'fsPatientVisitNotes', 'mpPatientVisitNotes'))
            cbpChangeSigna.PerformCallback('save');
        return false;
    });

    $('.imgEditCtl.imgLink').die('click')
    $('.imgEditCtl.imgLink').live('click', function () {
        $row = $(this).closest('tr');
        var entity = rowToObject($row);
        var prescriptionOrderDetailID = entity.PrescriptionOrderDetailID;
        var transactionDetailID = entity.ID;

        $('#<%=hdnTransactionDetailIDCtl.ClientID %>').val(transactionDetailID);
        $('#<%=hdnPrescriptionDetailIDCtl.ClientID %>').val(prescriptionOrderDetailID);

        if (entity.IsNonMaster == 'True') {
            alert('');
        }
        else if (entity.IsCompound != 'True') {
            $('#<%=txtDrugCodeCtl.ClientID %>').val(entity.ItemCode);
            $('#<%=txtDrugNameCtl.ClientID %>').val(entity.DrugName);
            $('#<%=hdnSignaIDCtl.ClientID %>').val(entity.SignaID);
            $('#<%=txtSignaName1.ClientID %>').val(entity.SignaName1);
            $('#<%=txtSignaLabel.ClientID %>').val(entity.SignaLabel);
        }
        else {
            $('#<%=hdnPrescriptionDetailIDCtl.ClientID %>').val(entity.PrescriptionOrderDetailID);
            openCompoundEntry('edit');
        }


        $('#containerEntryDataCtl').show();
    });

    function onChangeSignaCallback(s) {
        var param = s.cpResult.split('|');
        if (param[0] == 'save') {
            if (param[1] == 'fail')
                showToast('Save Failed', 'Error Message : ' + param[2]);
            else
                $('#containerEntryDataCtl').hide();
        }
        else if (param[0] == 'delete') {
            if (param[1] == 'fail')
                showToast('Delete Failed', 'Error Message : ' + param[2]);
        }
        hideLoadingPanel();
    }

    //#region Signa
    $('#lblSignaCtl.lblLink').click(function () {
        var filterExpression = "IsDeleted = 0";
        openSearchDialog('signa', filterExpression, function (value) {
            $('#<%=txtSignaLabel.ClientID %>').val(value);
            txtSignaLabelChanged(value);
        });
    });

    $('#<%=txtSignaLabel.ClientID %>').change(function () {
        txtSignaLabelChanged($(this).val());
    });

    function txtSignaLabelChanged(value) {
        var filterExpression = "IsDeleted = 0 AND SignaLabel = '" + value + "'";
        Methods.getObject('GetvSignaList', filterExpression, function (result) {
            if (result != null) {
                $('#<%=hdnSignaIDCtl.ClientID %>').val(result.SignaID);
                $('#<%=txtSignaName1.ClientID %>').val(result.SignaName1);
            } else {
                $('#<%=hdnSignaIDCtl.ClientID %>').val('');
                $('#<%=txtSignaLabel.ClientID %>').val('');
                $('#<%=txtSignaName1.ClientID %>').val('');
            }
        });
    }
</script>
<div style="height: 440px; overflow-y: auto">
    <input type="hidden" value="" id="hdnSignaIDCtl" runat="server" />
    <input type="hidden" value="" runat="server" id="hdnVisitID" />
    <input type="hidden" value="" runat="server" id="hdnPrescriptionOrderIDCtl" />
    <input type="hidden" value="" runat="server" id="hdnTransactionIDCtl" />
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
                                <%=GetLabel("No. Resep")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtPrescriptionNo" Width="160px" runat="server" />
                        </td>
                    </tr>
                </table>
                <div id="containerEntryDataCtl" style="margin-top: 10px; display: none;">
                    <div class="pageTitle">
                        <%=GetLabel("Entry")%></div>
                    <input type="hidden" id="hdnTransactionDetailIDCtl" runat="server" value="" />
                    <input type="hidden" id="hdnPrescriptionDetailIDCtl" runat="server" value="" />
                    <fieldset id="fsPatientVisitNotes" style="margin: 0">
                        <table class="tblEntryDetail" style="width: 100%">
                            <colgroup>
                                <col style="width: 10%" />
                                <col />
                            </colgroup>
                            <tr>
                                <td class="tdLabel">
                                    <%=GetLabel("Obat") %>
                                </td>
                                <td>
                                    <table style="width: 100%" cellpadding="0" cellspacing="0">
                                        <colgroup>
                                            <col style="width: 30%" />
                                            <col style="width: 3px" />
                                            <col />
                                        </colgroup>
                                        <tr>
                                            <td>
                                                <asp:TextBox ID="txtDrugCodeCtl" ReadOnly="true" CssClass="required" Width="100%"
                                                    runat="server" />
                                            </td>

                                            <td>
                                                <asp:TextBox ID="txtDrugNameCtl" ReadOnly="true" CssClass="required" Width="100%"
                                                    runat="server"/>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                               <td class="tdLabel">
                                    <label class="lblNormal lblLink" id="lblSignaCtl">
                                        <%=GetLabel("Signa")%></label>
                                </td>
                                <td>
                                    <table style="width: 100%" cellpadding="0" cellspacing="0">
                                        <colgroup>
                                            <col style="width: 30%" />
                                            <col style="width: 3px" />
                                            <col />
                                        </colgroup>
                                        <tr>
                                             <td>
                                                <asp:TextBox runat="server" ID="txtSignaLabel" Width="100%" />
                                            </td>

                                            <td>
                                                <asp:TextBox runat="server" ID="txtSignaName1" Width="100%" ReadOnly="true" TabIndex="999" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <table>
                                        <tr>
                                            <td>
                                                <input type="button" id="btnSaveCtl" value='<%= GetLabel("Simpan")%>' />
                                            </td>
                                            <td>
                                                <input type="button" id="btnCancelCtl" value='<%= GetLabel("Batal")%>' />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                </div>
                <dxcp:ASPxCallbackPanel ID="cbpChangeSigna" runat="server" Width="100%" ClientInstanceName="cbpChangeSigna"
                    ShowLoadingPanel="false" OnCallback="cbpChangeSigna_Callback">
                    <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ onChangeSignaCallback(s); }" />
                    <PanelCollection>
                        <dx:PanelContent ID="PanelContent1" runat="server">
                            <asp:Panel runat="server" ID="pnlView" CssClass="pnlContainerGrid">
                                <asp:ListView ID="lvwViewCtl" runat="server">
                                    <EmptyDataTemplate>
                                        <table id="tblView" runat="server" class="grdNormal notAllowSelect" cellspacing="0"
                                            rules="all">
                                            <tr>
                                                <th style="width: 70px;">
                                                </th>
                                                <th align="left">
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
                                                <th style="width: 70px;">
                                                </th>
                                                <th align="left">
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
                                            </tr>
                                            <tr runat="server" id="itemPlaceholder">
                                            </tr>
                                        </table>
                                    </LayoutTemplate>
                                    <ItemTemplate>
                                        <tr>
                                            <td>
                                                <img class="imgLink imgEditCtl" title='<%=GetLabel("Edit")%>' src='<%# ResolveUrl("~/Libs/Images/Button/edit.png")%>'
                                                    alt="" style="float: left; margin-right: 2px;" />
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
                                            </td>
                                            <td>
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <b>
                                                                <div style="<%# Eval("GCDrugClass").ToString() == "X123^O" ? "color: Red": Eval("GCDrugClass").ToString() == "X123^P" ? "color: Blue" : "color: Black"%>">
                                                                    <%#: Eval("cfInformationLine1")%></div>
                                                            </b>
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
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </ItemTemplate>
                                </asp:ListView>
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
    <div style="width: 100%; text-align: right">
        <input type="button" value='<%= GetLabel("Tutup")%>' onclick="pcRightPanelContent.Hide();" />
    </div>
</div>
