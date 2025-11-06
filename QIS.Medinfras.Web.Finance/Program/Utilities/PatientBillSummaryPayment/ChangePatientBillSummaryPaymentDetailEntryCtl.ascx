<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ChangePatientBillSummaryPaymentDetailEntryCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.Finance.Program.ChangePatientBillSummaryPaymentDetailEntryCtl" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<script type="text/javascript" id="dxss_ChangePatientBillSummaryPaymentDetailEntryCtl">
    $('#btnEntryPopupCancel').live('click', function () {
        $('#containerPopupEntryData').hide();
    });

    $('#btnEntryPopupSave').click(function (evt) {
        if (IsValid(evt, 'fsEntryPopup', 'mpEntryPopup'))
            cbpEntryPopupView.PerformCallback('save');
        return false;
    });

    $('.imgEdit.imgLink').die('click');
    $('.imgEdit.imgLink').live('click', function () {
        $row = $(this).closest('tr');
        var paymentDetailID = $row.find('.hdnPaymentDetailID').val();
        var cardType = $row.find('.hdnCardType').val();
        var cardProvider = $row.find('.hdnCardProvider').val();
        var cardNumber = $row.find('.hdnCardNumber').val();

        var digit1 = cardNumber.substring(0, 4);
        var lenghtCardNumber = cardNumber.length;
        var subCardNumber = lenghtCardNumber - 4;
        var digit4 = cardNumber.substring(subCardNumber, lenghtCardNumber);
        var digitDepan = cardNumber.substring(0, 4);
        var digitBelakang = cardNumber.slice(-4);

        var cardHolderName = $row.find('.hdnCardHolderName').val();

        var cardValidThru = $row.find('.hdnCardValidThru').val().split('/');
        var expiredDateMonth = parseInt(cardValidThru[0]);
        var expiredDateYear = 2000 + parseInt(cardValidThru[1]);

        var paymentAmount = $row.find('.hdncfPaymentAmountInString').val();
        var batchNo = $row.find('.hdnBatchNo').val();
        var traceNo = $row.find('.hdnTraceNo').val();
        var referenceNo = $row.find('.hdnReferenceNo').val();
        var approvalCode = $row.find('.hdnApprovalCode').val();
        var terminalID = $row.find('.hdnTerminalID').val();

        $('#<%=hdnPaymentDetailID.ClientID %>').val(paymentDetailID);
        cboCardType.SetValue(cardType);
        cboCardProvider.SetValue(cardProvider);
        $('#<%=txtCardNumber1.ClientID %>').val(digitDepan);
        $('#<%=txtCardNumber4.ClientID %>').val(digitBelakang);
        $('#<%=txtHolderName.ClientID %>').val(cardHolderName);
        cboCardDateMonth.SetValue(expiredDateMonth);
        cboCardDateYear.SetValue(expiredDateYear);
        $('#<%=txtPaymentAmount.ClientID %>').val(paymentAmount);
        $('#<%=txtBatchNo.ClientID %>').val(batchNo);
        $('#<%=txtTraceNo.ClientID %>').val(traceNo);
        $('#<%=txtReferenceNo.ClientID %>').val(referenceNo);
        $('#<%=txtApprovalCode.ClientID %>').val(approvalCode);
        $('#<%=txtTerminalID.ClientID %>').val(terminalID);

        $('#containerPopupEntryData').show();
    });

    function onCbpEntryPopupViewEndCallback(s) {
        var param = s.cpResult.split('|');
        if (param[0] == 'save') {
            if (param[1] == 'fail')
                showToast('Save Failed', 'Error Message : ' + param[2]);
            else
                $('#containerPopupEntryData').hide();
        }
        $('#containerImgLoadingViewPopup').hide();
    }
</script>
<div style="height: 440px; overflow-y: auto">
    <input type="hidden" id="hdnPaymentID" value="" runat="server" />
    <input type="hidden" id="hdnPaymentDetailID" value="" runat="server" />
    <table class="tblContentArea">
        <colgroup>
            <col style="width: 100%" />
        </colgroup>
        <tr>
            <td style="padding: 5px; vertical-align: top">
                <table class="tblEntryContent" style="width: 70%">
                    <colgroup>
                        <col style="width: 160px" />
                        <col />
                    </colgroup>
                </table>
                <div id="containerPopupEntryData" style="margin-top: 10px; display: none;">
                    <input type="hidden" id="hdnHealthcareServiceUnitID" runat="server" value="" />
                    <div class="pageTitle">
                        <%=GetLabel("Entry")%></div>
                    <fieldset id="fsEntryPopup" style="margin: 0">
                        <table class="tblEntryDetail" style="width: 100%">
                            <colgroup>
                                <col style="width: 90px" />
                                <col style="width: 150px" />
                                <col />
                            </colgroup>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblMandatory">
                                        <%=GetLabel("Tipe Kartu")%></label>
                                </td>
                                <td>
                                    <dxe:ASPxComboBox ID="cboCardType" ClientInstanceName="cboCardType" Width="150px"
                                        runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblMandatory">
                                        <%=GetLabel("Bank Penerbit")%></label>
                                </td>
                                <td>
                                    <dxe:ASPxComboBox ID="cboCardProvider" ClientInstanceName="cboCardProvider" Width="150px"
                                        runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblMandatory">
                                        <%=GetLabel("No. Kartu")%></label>
                                </td>
                                <td>
                                    <table style="width: 100%;" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td>
                                                <asp:TextBox ID="txtCardNumber1" Text="XXXX" Width="100%" runat="server" Style="text-align: center" />
                                            </td>
                                            <td style="width: 3px">
                                                &nbsp;
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtCardNumber2" ReadOnly="true" Enabled="false" Text="XXXX" Width="100%"
                                                    runat="server" Style="text-align: center" />
                                            </td>
                                            <td style="width: 3px">
                                                &nbsp;
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtCardNumber3" ReadOnly="true" Enabled="false" Text="XXXX" Width="100%"
                                                    runat="server" Style="text-align: center" />
                                            </td>
                                            <td style="width: 3px">
                                                &nbsp;
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtCardNumber4" Width="100%" runat="server" Style="text-align: center" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblMandatory">
                                        <%=GetLabel("Pemegang Kartu")%></label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtHolderName" Width="100%" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblNormal">
                                        <%=GetLabel("Masa Berlaku")%></label>
                                </td>
                                <td>
                                    <table style="width: 100%;" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td>
                                                <dxe:ASPxComboBox ID="cboCardDateMonth" ClientInstanceName="cboCardDateMonth" Width="100px"
                                                    runat="server" />
                                            </td>
                                            <td style="width: 3px">
                                                &nbsp;
                                            </td>
                                            <td>
                                                <dxe:ASPxComboBox ID="cboCardDateYear" ClientInstanceName="cboCardDateYear" Width="80px"
                                                    runat="server" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblMandatory">
                                        <%=GetLabel("Jumlah")%></label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtPaymentAmount" Width="100%" runat="server" placeholder="isi tanpa titik ataupun koma"
                                        ReadOnly="true" style="text-align:right" />
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblNormal">
                                        <%=GetLabel("Terminal")%></label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtTerminalID" Width="100%" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblMandatory">
                                        <%=GetLabel("No. Batch")%></label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtBatchNo" Width="100%" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label>
                                        <%=GetLabel("No. Trace")%></label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtTraceNo" Width="100%" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblMandatory">
                                        <%=GetLabel("Reference No.")%></label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtReferenceNo" Width="100%" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblMandatory">
                                        <%=GetLabel("Appr Code")%></label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtApprovalCode" Width="100%" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3">
                                    <table>
                                        <tr>
                                            <td>
                                                <input type="button" id="btnEntryPopupSave" value='<%= GetLabel("Simpan")%>' class="w3-btn w3-hover-blue" />
                                            </td>
                                            <td>
                                                <input type="button" id="btnEntryPopupCancel" value='<%= GetLabel("Batal")%>' class="w3-btn w3-hover-blue" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </fieldset>
                </div>
                <dxcp:ASPxCallbackPanel ID="cbpEntryPopupView" runat="server" Width="100%" ClientInstanceName="cbpEntryPopupView"
                    ShowLoadingPanel="false" OnCallback="cbpEntryPopupView_Callback">
                    <ClientSideEvents BeginCallback="function(s,e){ $('#containerImgLoadingViewPopup').show(); }"
                        EndCallback="function(s,e){ onCbpEntryPopupViewEndCallback(s); }" />
                    <PanelCollection>
                        <dx:PanelContent ID="PanelContent1" runat="server">
                            <asp:Panel runat="server" ID="pnlPatientVisitTransHdGrdView" Style="width: 100%;
                                margin-left: auto; margin-right: auto; position: relative; font-size: 0.95em;">
                                <asp:GridView ID="grdView" runat="server" CssClass="grdView notAllowSelect" AutoGenerateColumns="false"
                                    OnRowDataBound="grdView_RowDataBound" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                    <Columns>
                                        <asp:TemplateField HeaderStyle-Width="35px" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <img class="imgEdit <%# Eval("GCPaymentMethod").ToString() == "X035^002" || Eval("GCPaymentMethod").ToString() == "X035^003" ? "imgLink" : "imgDisabled"%>"
                                                    title='<%=GetLabel("Edit")%>' src='<%# Eval("GCPaymentMethod").ToString() == "X035^002" || Eval("GCPaymentMethod").ToString() == "X035^003" ? ResolveUrl("~/Libs/Images/Button/edit.png") : ResolveUrl("~/Libs/Images/Button/edit_disabled.png")%>'
                                                    alt="" style="float: left; margin-left: 7px" />
                                                <input type="hidden" class="hdnPaymentDetailID" value="<%#: Eval("PaymentDetailID")%>" />
                                                <input type="hidden" class="hdnCardType" value="<%#: Eval("GCCardType")%>" />
                                                <input type="hidden" class="hdnCardProvider" value="<%#: Eval("GCCardProvider")%>" />
                                                <input type="hidden" class="hdnCardNumber" value="<%#: Eval("CardNumber")%>" />
                                                <input type="hidden" class="hdnCardHolderName" value="<%#: Eval("CardHolderName")%>" />
                                                <input type="hidden" class="hdnCardValidThru" value="<%#: Eval("CardValidThru")%>" />
                                                <input type="hidden" class="hdnPaymentAmount" value="<%#: Eval("PaymentAmount")%>" />
                                                <input type="hidden" class="hdncfPaymentAmountInString" value="<%#: Eval("cfPaymentAmountInString")%>" />
                                                <input type="hidden" class="hdnReferenceNo" value="<%#: Eval("ReferenceNo")%>" />
                                                <input type="hidden" class="hdnBatchNo" value="<%#: Eval("BatchNo")%>" />
                                                <input type="hidden" class="hdnTraceNo" value="<%#: Eval("TraceNo")%>" />
                                                <input type="hidden" class="hdnApprovalCode" value="<%#: Eval("ApprovalCode")%>" />
                                                <input type="hidden" class="hdnTerminalID" value="<%#: Eval("TerminalID")%>" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="PaymentMethod" ItemStyle-CssClass="tdPaymentMethod" HeaderText="Payment Method"
                                            HeaderStyle-Width="50px" />
                                        <asp:BoundField DataField="CardType" ItemStyle-CssClass="tdCardType" HeaderText="Card Type"
                                            HeaderStyle-Width="30px" />
                                        <asp:BoundField DataField="CardProvider" ItemStyle-CssClass="tdCardProvider" HeaderText="Card Provider"
                                            HeaderStyle-Width="30px" />
                                        <asp:BoundField DataField="CardNumber" ItemStyle-CssClass="tdCardNumber" HeaderText="Card Number"
                                            HeaderStyle-Width="90px" />
                                        <asp:BoundField DataField="CardHolderName" ItemStyle-CssClass="tdCardHolderName"
                                            HeaderText="Card Name" HeaderStyle-Width="120px" />
                                    </Columns>
                                    <EmptyDataTemplate>
                                        <%=GetLabel("Data Tidak Tersedia")%>
                                    </EmptyDataTemplate>
                                </asp:GridView>
                                <div class="imgLoadingGrdView" id="containerImgLoadingViewPopup">
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
