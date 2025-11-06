<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ARInvoicePatientAlocationCtlX.ascx.cs" 
    Inherits="QIS.Medinfras.Web.Finance.Program.ARInvoicePatientAlocationCtl" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<script type="text/javascript" id="dxss_alocationarinvoiceentryctl">

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
        var regID = $row.find('.hdnRegistrationID').val();
        var regNo = $row.find('.hdnRegistrationNo').val();
        var patient = $row.find('.hdnPatient').val();
        var paymentID = $row.find('.hdnPaymentID').val();
        var paymentNo = $row.find('.hdnPaymentNo').val();
        var claimedAmount = $row.find('.hdnClaimedAmount').val();
        var paymentAmount = $row.find('.hdnPaymentAmount').val();
        var outstandingAmount = $row.find('.hdnOutstandingAmount').val();

        $('#<%=hdnARDtRegistrationID.ClientID %>').val(regID);
        $('#<%=txtRegistrationNo.ClientID %>').val(regNo);
        $('#<%=txtPatient.ClientID %>').val(patient);
        $('#<%=hdnARDtPaymentID.ClientID %>').val(paymentID); 
        $('#<%=txtPaymentNo.ClientID %>').val(paymentNo);
        $('#<%=hdnClaimedAmountDt.ClientID %>').val(claimedAmount).trigger('changeValue');
        $('#<%=txtClaimedAmount.ClientID %>').val(claimedAmount).trigger('changeValue');
        $('#<%=txtPaymentDtAmount.ClientID %>').val(paymentAmount).trigger('changeValue');
        $('#<%=txtOutstandingAmount.ClientID %>').val(outstandingAmount).trigger('changeValue');

        $('#containerPopupEntryData').show();
    });

    function onCbpEntryPopupViewEndCallback(s) {
        var param = s.cpResult.split('|');
        if (param[0] == 'save') {
            if (param[1] == 'fail') {
                showToast('Save Failed', 'Error Message : ' + param[2]);
            }
            else {
                $('#containerPopupEntryData').hide();
                cbpEntryPopupView.PerformCallback('refresh');
                $('#<%=txtTotalPaymentDt.ClientID %>').val(param[0]);
                $('#<%=txtRemainingPaymentDt.ClientID %>').val(param[1]);
            }
        }
        else if (param[0] == 'delete') {
            if (param[1] == 'fail') {
                showToast('Delete Failed', 'Error Message : ' + param[2]);
            }
        }
        $('#containerImgLoadingViewPopup').hide();
    }
</script>
<div style="height: 450px; overflow-y: auto">
    <input type="hidden" id="hdnARInvoiceID" runat="server" value="" />
    <input type="hidden" id="hdnARDtRegistrationID" runat="server" value="" />
    <input type="hidden" id="hdnARDtPaymentID" runat="server" value="" />
    <input type="hidden" id="hdnClaimedAmountDt" runat="server" value="" />
    <table class="tblContentArea">
        <colgroup>
            <col style="width: 100%" />
        </colgroup>
        <tr>
            <td style="padding: 5px; vertical-align: top">
                <table class="tblEntryContent">
                    <colgroup>
                        <col style="width: 50%" />
                        <col style="width: 50%" />
                    </colgroup>
                    <tr>
                        <td>
                            <table>
                                <colgroup>
                                    <col style="width: 90px" />
                                    <col />
                                </colgroup>
                                <tr>
                                    <td class="tdLabel">
                                        <label class="lblNormal">
                                            <%=GetLabel("No. Invoice")%></label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtARInvoiceNo" Style="text-align: center" ReadOnly="true" Width="140px"
                                            runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel">
                                        <label class="lblNormal">
                                            <%=GetLabel("Nilai Bayar")%></label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtPaymentAmount" Style="text-align: right" ReadOnly="true" Width="140px"
                                            runat="server" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td>
                            <table>
                                <colgroup>
                                    <col style="width: 150px" />
                                    <col />
                                </colgroup>
                                <tr>
                                    <td class="tdLabel">
                                        <label class="lblNormal">
                                            <%=GetLabel("Total Nilai Bayar Detail")%></label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtTotalPaymentDt" Style="text-align: right; color: Blue" ReadOnly="true"
                                            Width="140px" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tdLabel">
                                        <label class="lblNormal">
                                            <%=GetLabel("Total Kurang Bayar Detail")%></label>
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtRemainingPaymentDt" Style="text-align: right; color: Red" ReadOnly="true"
                                            Width="140px" runat="server" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
                <div id="containerPopupEntryData" style="margin-top: 10px; display: none;">
                    <div class="pageTitle">
                        <%=GetLabel("Entry")%></div>
                    <fieldset id="fsEntryPopup" style="margin: 0">
                        <table class="tblEntryDetail" style="width: 100%">
                            <colgroup>
                                <col style="width: 100px" />
                                <col />
                            </colgroup>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblNormal">
                                        <%=GetLabel("No. Registrasi")%></label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtRegistrationNo" ReadOnly="true" Width="120px" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblNormal">
                                        <%=GetLabel("Pasien")%></label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtPatient" ReadOnly="true" Width="200px" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblNormal">
                                        <%=GetLabel("No. Piutang")%></label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtPaymentNo" ReadOnly="true" Width="120px" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblNormal">
                                        <%=GetLabel("Nilai Klaim")%></label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtClaimedAmount" ReadOnly="true" CssClass="txtCurrency" Width="200px"
                                        runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblNormal">
                                        <%=GetLabel("Nilai Bayar")%></label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtPaymentDtAmount" CssClass="txtCurrency" Width="200px" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td class="tdLabel">
                                    <label class="lblNormal">
                                        <%=GetLabel("Nilai Outstanding")%></label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtOutstandingAmount" ReadOnly="true" CssClass="txtCurrency" Width="200px"
                                        runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3">
                                    <table>
                                        <tr>
                                            <td>
                                                <input type="button" id="btnEntryPopupSave" value='<%= GetLabel("Simpan")%>' />
                                            </td>
                                            <td>
                                                <input type="button" id="btnEntryPopupCancel" value='<%= GetLabel("Batal")%>' />
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
                            <asp:Panel runat="server" ID="pnlAlocationARInvoiceDt" Style="width: 100%; margin-left: auto;
                                margin-right: auto; position: relative; font-size: 0.95em;">
                                <asp:GridView ID="grdView" runat="server" CssClass="grdView notAllowSelect" AutoGenerateColumns="false"
                                    OnRowDataBound="grdView_RowDataBound" ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty">
                                    <Columns>
                                        <asp:TemplateField HeaderStyle-Width="40px" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <img class="imgEdit imgLink" src='<%# ResolveUrl("~/Libs/Images/Button/edit.png")%>'
                                                    alt="" />
                                                <input type="hidden" class="hdnRegistrationID" value="<%#: Eval("RegistrationID")%>" />
                                                <input type="hidden" class="hdnRegistrationNo" value="<%#: Eval("RegistrationNo")%>" />
                                                <input type="hidden" class="hdnPatient" value="<%#: Eval("PatientName")%>" />
                                                <input type="hidden" class="hdnPaymentID" value="<%#: Eval("PaymentID")%>" />
                                                <input type="hidden" class="hdnPaymentNo" value="<%#: Eval("PaymentNo")%>" />
                                                <input type="hidden" class="hdnClaimedAmount" value="<%#: Eval("ClaimedAmount")%>" />
                                                <input type="hidden" class="hdnPaymentAmount" value="<%#: Eval("PaymentAmount")%>" />
                                                <input type="hidden" class="hdnOutstandingAmount" value="<%#: Eval("TotalOutstandingPayment")%>" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField HeaderStyle-Width="120px" DataField="RegistrationNo" HeaderText="No. Registrasi" />
                                        <asp:BoundField HeaderStyle-Width="220px" DataField="PatientName" HeaderText="Nama Pasien" />
                                        <asp:BoundField HeaderStyle-Width="120px" DataField="PaymentNo" HeaderText="No. Piutang" />
                                        <asp:BoundField HeaderStyle-Width="180px" DataField="TotalClaimedAmountInString"
                                            HeaderText="Jumlah Klaim" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" />
                                        <asp:BoundField HeaderStyle-Width="180px" DataField="TotalPaymentAmountInString"
                                            HeaderText="Jumlah Bayar" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" />
                                        <asp:BoundField HeaderStyle-Width="180px" DataField="TotalOutstandingPaymentInString"
                                            HeaderText="Jumlah Outstanding" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right" />
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

