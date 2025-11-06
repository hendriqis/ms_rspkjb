<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ARInvoicePatientAddRevisionCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.Finance.Program.ARInvoicePatientAddRevisionCtl" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<script type="text/javascript" id="dxss_ARInvoicePatientAddRevisionCtl">
    setDatePicker('<%=txtPeriodFrom.ClientID %>');
    setDatePicker('<%=txtPeriodTo.ClientID %>');

    $('#btnRefresh').live('click', function () {
        cbpProcessDetail.PerformCallback('Refresh');
    });

    function getCheckedMember() {
        var lstSelectedMember = $('#<%=hdnSelectedMember.ClientID %>').val().split(',');
        var lstSelectedMemberPaymentID = $('#<%=hdnSelectedMemberPaymentID.ClientID %>').val().split(',');
        var lstSelectedMemberReferenceNoPrefix = $('#<%=hdnSelectedMemberReferenceNoPrefix.ClientID %>').val().split(',');
        var lstSelectedMemberReferenceNo = $('#<%=hdnSelectedMemberReferenceNo.ClientID %>').val().split(',');
        var result = '';
        $('#<%=grdView.ClientID %> .chkIsSelected input').each(function () {
            if ($(this).is(':checked')) {
                $tr = $(this).closest('tr');
                var key = $tr.find('.keyField').html();
                var paymentID = $tr.find('.paymentID').val();
                var referenceNoPrefix = $tr.find('.txtReferenceNoPrefix').val();
                var referenceNo = $tr.find('.txtReferenceNo').val();
                var idx = lstSelectedMember.indexOf(key);
                if (idx < 0) {
                    lstSelectedMember.push(key);
                    lstSelectedMemberPaymentID.push(paymentID);
                    lstSelectedMemberReferenceNo.push(referenceNo);
                    lstSelectedMemberReferenceNoPrefix.push(referenceNoPrefix);
                }
                else {
                    lstSelectedMemberPaymentID[idx] = paymentID;
                    lstSelectedMemberReferenceNo[idx] = referenceNo;
                    lstSelectedMemberReferenceNoPrefix[idx] = referenceNoPrefix;
                }
            }
            else {
                var key = $(this).closest('tr').find('.keyField').html();
                var idx = lstSelectedMember.indexOf(key);
                if (idx > -1) {
                    lstSelectedMember.splice(idx, 1);
                    lstSelectedMemberPaymentID.splice(idx, 1);
                    lstSelectedMemberReferenceNo.splice(idx, 1);
                    lstSelectedMemberReferenceNoPrefix.splice(idx, 1);
                }
            }
        });
        $('#<%=hdnSelectedMember.ClientID %>').val(lstSelectedMember.join(','));
        $('#<%=hdnSelectedMemberPaymentID.ClientID %>').val(lstSelectedMemberPaymentID.join(','));
        $('#<%=hdnSelectedMemberReferenceNo.ClientID %>').val(lstSelectedMemberReferenceNo.join(','));
        $('#<%=hdnSelectedMemberReferenceNoPrefix.ClientID %>').val(lstSelectedMemberReferenceNoPrefix.join(','));
    }

    $('#chkCheckAll').live('click', function () {
        var isChecked = $(this).is(':checked');
        $('#<%=grdView.ClientID %> .chkIsSelected input').each(function () {
            $(this).prop('checked', isChecked);
        });
    });

    function onBeforeSaveRecord(errMessage) {
        getCheckedMember();
        if ($('#<%=hdnSelectedMember.ClientID %>').val() == '') {
            errMessage.text = 'Please Select Payment First';
            return false;
        }
        return true;
    }

    function onCbpProcessDetailEndCallback(s) {
        hideLoadingPanel();
        getCheckedMember();
    }
</script>
<div style="height: 500px; overflow-y: auto; overflow-x: hidden">
    <input type="hidden" id="hdnARInvoiceIDCtl" runat="server" value="" />
    <input type="hidden" id="hdnSelectedMember" runat="server" value="" />
    <input type="hidden" id="hdnSelectedMemberPaymentID" runat="server" value="" />
    <input type="hidden" id="hdnSelectedMemberReferenceNoPrefix" runat="server" value="" />
    <input type="hidden" id="hdnSelectedMemberReferenceNo" runat="server" value="" />
    <table class="tblContentArea">
        <colgroup>
            <col style="width: 50%" />
            <col style="width: 50%" />
        </colgroup>
        <tr>
            <td style="padding: 5px; vertical-align: top;">
                <table class="tblEntryContent" style="width: 100%">
                    <colgroup>
                        <col style="width: 25%" />
                        <col />
                    </colgroup>
                    <tr>
                        <td class="tdLabel">
                            <label>
                                <%=GetLabel("Asal Pasien")%></label>
                        </td>
                        <td style="width: 100%">
                            <table>
                                <colgroup>
                                    <col style="width: 250px" />
                                    <col style="width: 100px" />
                                    <col />
                                </colgroup>
                                <tr>
                                    <td align="left">
                                        <dxe:ASPxComboBox ID="cboDepartment" ClientInstanceName="cboDepartment" runat="server"
                                            Width="100%" />
                                    </td>
                                    <td align="left">
                                        <asp:CheckBox runat="server" ID="chkIsExclusion" Text=" Is Exclusion?" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Status Pendaftaran")%></label>
                        </td>
                        <td>
                            <dxe:ASPxComboBox ID="cboRegistrationStatus" ClientInstanceName="cboRegistrationStatus"
                                Width="250px" runat="server">
                            </dxe:ASPxComboBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal" style="text-decoration: underline">
                                <%=GetLabel("Filter Berdasarkan")%></label>
                        </td>
                        <td>
                            <dxe:ASPxComboBox ID="cboFilterBy" ClientInstanceName="cboFilterBy" Width="250px"
                                runat="server" BackColor="Yellow">
                            </dxe:ASPxComboBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <label class="lblNormal">
                                <%=GetLabel("Periode Transaksi") %></label>
                        </td>
                        <td>
                            <table cellpadding="0" cellspacing="0" width="100%">
                                <colgroup>
                                    <col style="width: 120px" />
                                    <col style="width: 30px" />
                                    <col style="width: 120px" />
                                    <col />
                                </colgroup>
                                <tr>
                                    <td>
                                        <asp:TextBox runat="server" ID="txtPeriodFrom" CssClass="datepicker" />
                                    </td>
                                    <td>
                                        <label>
                                            <%=GetLabel("s/d")%></label>
                                    </td>
                                    <td>
                                        <asp:TextBox runat="server" ID="txtPeriodTo" CssClass="datepicker" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal" style="text-decoration: underline">
                                <%=GetLabel("Urut Berdasarkan")%></label>
                        </td>
                        <td>
                            <dxe:ASPxComboBox ID="cboSortBy" ClientInstanceName="cboSortBy" Width="250px" runat="server"
                                BackColor="Pink">
                            </dxe:ASPxComboBox>
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
        <tr>
            <td style="padding: 5px; vertical-align: top" colspan="2">
                <dxcp:ASPxCallbackPanel ID="cbpProcessDetail" runat="server" Width="100%" ClientInstanceName="cbpProcessDetail"
                    ShowLoadingPanel="false" OnCallback="cbpProcessDetail_Callback">
                    <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ onCbpProcessDetailEndCallback(s); }" />
                    <PanelCollection>
                        <dx:PanelContent ID="PanelContent1" runat="server">
                            <asp:Panel runat="server" ID="pnlEntryPopupGrdView" Style="width: 100%; margin-left: auto;
                                margin-right: auto; position: relative; font-size: 0.95em;">
                                <asp:GridView ID="grdView" runat="server" CssClass="grdSelected" AutoGenerateColumns="false"
                                    ShowHeaderWhenEmpty="true" EmptyDataRowStyle-CssClass="trEmpty" OnRowDataBound="grdView_RowDataBound">
                                    <Columns>
                                        <asp:BoundField DataField="PaymentDetailID" HeaderStyle-CssClass="keyField" ItemStyle-CssClass="keyField" />
                                        <asp:TemplateField ItemStyle-HorizontalAlign="center" HeaderStyle-Width="50px">
                                            <HeaderTemplate>
                                                <input type="checkbox" id="chkCheckAll" style="text-align: center;" />
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <input type="hidden" class="paymentID" value="<%#: Eval("PaymentID")%>" />
                                                <input type="hidden" class="registrationID" value="<%#: Eval("RegistrationID")%>" />
<%--                                                <input type="hidden" class="isBPJS" value="<%#: Eval("IsBPJS")%>" />
                                                <input type="hidden" class="isInhealth" value="<%#: Eval("IsInhealth")%>" />--%>
                                                <asp:CheckBox ID="chkIsSelected" runat="server" CssClass="chkIsSelected" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderStyle-Width="150px" HeaderText="Tgl/No Registrasi" HeaderStyle-HorizontalAlign="Left"
                                            ItemStyle-HorizontalAlign="Left">
                                            <ItemTemplate>
                                                <div>
                                                    <%#: Eval("RegistrationDateInString")%></div>
                                                <div>
                                                    <%#: Eval("RegistrationNo") %>
                                                    <div>
                                                        <%#: Eval("DepartmentID") %>
                                                    </div>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderStyle-Width="150px" HeaderText="Informasi Peserta" HeaderStyle-HorizontalAlign="Left"
                                            ItemStyle-HorizontalAlign="Left">
                                            <ItemTemplate>
<%--                                                <div>
                                                    <%#: Eval("cfInfoNumberCaption")%>
                                                    <%=GetLabel(":") %>
                                                    <%#: Eval("cfInfoNumber")%></div>--%>
                                                <div>
                                                    <%=GetLabel("No.Rujukan : ") %>
                                                    <%#: Eval("ReferralNo")%></div>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderStyle-Width="200px" HeaderText="Pasien" HeaderStyle-HorizontalAlign="Left"
                                            ItemStyle-HorizontalAlign="Left">
                                            <ItemTemplate>
                                                <div>
                                                    <%#: Eval("MedicalNo")%></div>
                                                <div>
                                                    <%#: Eval("PatientName")%></div>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderStyle-Width="150px" HeaderText="Tgl/No Piutang" HeaderStyle-HorizontalAlign="Left"
                                            ItemStyle-HorizontalAlign="Left">
                                            <ItemTemplate>
                                                <div>
                                                    <%#: Eval("PaymentDateInString")%></div>
                                                <div>
                                                    <%#: Eval("PaymentNo")%></div>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="PaymentAmount" HeaderText="Total Piutang" HeaderStyle-HorizontalAlign="Right"
                                            ItemStyle-HorizontalAlign="Right" DataFormatString="{0:N}" HeaderStyle-Width="150px" />
<%--                                        <asp:BoundField DataField="GrouperCodeClaim" HeaderText="Kode Grouper Klaim" HeaderStyle-HorizontalAlign="Center"
                                            HeaderStyle-Width="150px" ItemStyle-HorizontalAlign="Center" />
                                        <asp:BoundField DataField="GrouperAmountClaim" HeaderText="Nilai Grouper Klaim" HeaderStyle-HorizontalAlign="Right"
                                            ItemStyle-HorizontalAlign="Right" DataFormatString="{0:N}" HeaderStyle-Width="150px" />--%>
                                        <asp:TemplateField HeaderStyle-Width="150px" HeaderText="No Referensi" ItemStyle-HorizontalAlign="Center">
                                            <ItemTemplate>
                                                <table style="width: 90%" cellpadding="0" cellspacing="0">
                                                    <colgroup>
                                                        <col style="width: 50px" />
                                                        <col style="width: 5px" />
                                                    </colgroup>
                                                    <tr>
                                                        <td>
                                                            <input type="text" class="txtReferenceNoPrefix" style="width: 100%;" />
                                                        </td>
                                                        <td>
                                                            &nbsp;
                                                        </td>
                                                        <td>
                                                            <input type="text" class="txtReferenceNo" style="width: 100%;" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <EmptyDataTemplate>
                                        <%=GetLabel("Data Tidak Tersedia")%>
                                    </EmptyDataTemplate>
                                </asp:GridView>
                            </asp:Panel>
                        </dx:PanelContent>
                    </PanelCollection>
                </dxcp:ASPxCallbackPanel>
                <div class="containerPaging" style="display: none">
                    <div class="wrapperPaging">
                        <div id="pagingPopup">
                        </div>
                    </div>
                </div>
            </td>
        </tr>
    </table>
    <div style="width: 100%; text-align: right">
        <input type="button" value='<%= GetLabel("Close")%>' onclick="pcRightPanelContent.Hide();" />
    </div>
</div>
