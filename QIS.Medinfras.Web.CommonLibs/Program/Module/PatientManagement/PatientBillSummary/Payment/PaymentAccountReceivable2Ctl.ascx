<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PaymentAccountReceivable2Ctl.ascx.cs"
    Inherits="QIS.Medinfras.Web.CommonLibs.Controls.PaymentAccountReceivable2Ctl" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<script type="text/javascript" id="dxss_patientpayerctl">
    $(function () {
        $('#btnPopupYes').click(function () {
            getCheckedMember()
            if ($('#<%=hdnSelectedMember.ClientID %>').val() == '') {
                showToast('Warning', 'Please Select Item First');
            }
            else {
                cbpProcessPopup.PerformCallback();
            }

            $('#<%=hdnInlineEditingPayerData.ClientID %>').val(grdPaymentARPayer.getTableData());
        });
    });

    //#region chkSelect
    $('#chkSelectAll').die('change');
    $('#chkSelectAll').live('change', function () {
        var isChecked = $(this).is(":checked");
        $('.chkIsSelected input').each(function () {
            $(this).prop('checked', isChecked);
            $(this).change();
        });
    });

    $('.chkIsSelected input').die('change');
    $('.chkIsSelected input').live('change', function () {
        $tr = $(this).closest('tr');
        if ($(this).is(':checked')) {
            $tr.find('.txtPayerAmount').removeAttr('readonly');
        } else {
            $tr.find('.txtPayerAmount').attr('readonly', 'readonly');
        }
    });
    //#endregion

    //#region getCheckedMember
    function getCheckedMember(errMessage) {
        var lstSelectedMember = $('#<%=hdnSelectedMember.ClientID %>').val().split('|');
        var lstPayerAmount = $('#<%=hdnPayerAmount.ClientID %>').val().split('|');
        var lstSumPayerAmount = $('#<%=hdnSumPayerAmount.ClientID %>').val();

        $('.grdService .chkIsSelected input').each(function () {
            if ($(this).is(':checked')) {
                $tr = $(this).closest('tr');
                var key = $tr.find('.keyField').html().trim();
                var payerAmount = $tr.find('.txtPayerAmount').val();

                var idx = lstSelectedMember.indexOf(key);
                if (idx < 0) {
                    lstSelectedMember.push(key);
                    lstPayerAmount.push(payerAmount);
                    lstSumPayerAmount += payerAmount;
                }
                else {
                    lstPayerAmount[idx] = payerAmount;
                }
            }
            else {
                var key = $(this).closest('tr').find('.keyField').html().trim();
                var idx = lstSelectedMember.indexOf(key);
                if (idx > -1) {
                    lstSelectedMember.splice(idx, 1);
                    lstPayerAmount.splice(idx, 1);
                }
            }
        });

        $('#<%=hdnSelectedMember.ClientID %>').val(lstSelectedMember.join('|').trim());
        $('#<%=hdnPayerAmount.ClientID %>').val(lstPayerAmount.join('|'));
    }
    //#endregion

    function onCbpProcessPopupEndCallback(s) {
        var param = s.cpResult.split('|');
        var isCloseRegistration = s.cpIsCloseRegistration;
        if (param[0] == 'fail')
            showToast('Process Failed', 'Error Message : ' + param[1]);
        else {
            showToast('Process Success', s.cpRetval, function () {
                var count = parseInt(s.cpCount);
                for (var i = 0; i < count; ++i)
                    onAfterAddRecordAddRowCount();
                pcRightPanelContent.Hide();
            });
        }

        hideLoadingPanel();
    }
</script>
<input type="hidden" value="" id="hdnSelectedMember" runat="server" />
<input type="hidden" value="" id="hdnPayerAmount" runat="server" />
<input type="hidden" value="" id="hdnSumPayerAmount" runat="server" />
<input type="hidden" id="hdnTotalPatientAmount" value="" runat="server" />
<input type="hidden" id="hdnTotalPayerAmount" value="" runat="server" />
<input type="hidden" id="hdnPatientBillingID" value="" runat="server" />
<input type="hidden" id="hdnRegistrationID" value="" runat="server" />
<input type="hidden" id="hdnDepartmentID" value="" runat="server" />
<input type="hidden" id="hdnCashierGroup" value="" runat="server" />
<input type="hidden" id="hdnShift" value="" runat="server" />
<input type="hidden" value="" id="hdnIsGrouperAmountClaimDefaultZero" runat="server" />
<input type="hidden" value="" id="hdnInlineEditingPayerData" runat="server" />
<tr>
    <td style="padding: 5px; vertical-align: top">
        <table>
            <colgroup>
                <col style="width: 150px" />
                <col />
            </colgroup>
            <tr id="trPatientAmount" runat="server">
                <td class="tdLabel">
                    <label class="lblNormal">
                        <%=GetLabel("Sisa Tagihan Pasien") %></label>
                </td>
                <td>
                    <asp:TextBox ID="txtPatientBillAmount" Width="150px" ReadOnly="true" CssClass="number"
                        runat="server" />
                </td>
            </tr>
            <tr id="trPayerAmount" runat="server">
                <td class="tdLabel">
                    <label class="lblNormal">
                        <%=GetLabel("Sisa Tagihan Instansi") %></label>
                </td>
                <td>
                    <asp:TextBox ID="txtPayerBillAmount" Width="150px" ReadOnly="true" CssClass="number"
                        runat="server" />
                </td>
            </tr>
        </table>
    </td>
</tr>
<tr>
    <td>
        <dxcp:ASPxCallbackPanel ID="cbpView" runat="server" Width="100%" ClientInstanceName="cbpView"
            ShowLoadingPanel="false" OnCallback="cbpView_Callback">
            <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ onCbpViewEndCallback(s); }" />
            <PanelCollection>
                <dx:PanelContent ID="PanelContent1" runat="server">
                    <asp:Panel runat="server" ID="pnlView" Style="width: 100%; margin-left: auto; margin-right: auto;
                        position: relative; font-size: 0.95em;">
                        <asp:ListView runat="server" ID="lvwView" OnItemDataBound="lvwView_ItemDataBound">
                            <EmptyDataTemplate>
                                <table id="tblView" runat="server" class="grdService grdSelected" cellspacing="0"
                                    rules="all">
                                    <tr>
                                        <th class="keyField" rowspan="2">
                                            &nbsp;
                                        </th>
                                        <th style="width: 20px">
                                            &nbsp;
                                        </th>
                                        <th style="width: 200px" align="left">
                                            <%=GetLabel("Instansi")%>
                                        </th>
                                        <th style="width: 50px" align="right">
                                            <%=GetLabel("Pembayaran")%>
                                        </th>
                                    </tr>
                                    <tr class="trEmpty">
                                        <td colspan="4">
                                            <%=GetLabel("Tidak ada data permintaan pembelian")%>
                                        </td>
                                    </tr>
                                </table>
                            </EmptyDataTemplate>
                            <LayoutTemplate>
                                <table id="tblView" runat="server" class="grdService grdSelected" cellspacing="0"
                                    rules="all">
                                    <tr>
                                        <th class="keyField" rowspan="2">
                                        </th>
                                        <th style="width: 20px" align="center">
                                            <input id="chkSelectAll" type="checkbox" />
                                        </th>
                                        <th style="width: 200px" align="left">
                                            <%=GetLabel("Instansi")%>
                                        </th>
                                        <th style="width: 50px" align="right">
                                            <%=GetLabel("Pembayaran")%>
                                        </th>
                                    </tr>
                                    <tr runat="server" id="itemPlaceholder">
                                    </tr>
                                </table>
                            </LayoutTemplate>
                            <ItemTemplate>
                                <tr>
                                    <td class="keyField">
                                        <%#: Eval("BusinessPartnerID")%>
                                    </td>
                                    <td align="center">
                                        <asp:CheckBox ID="chkIsSelected" runat="server" CssClass="chkIsSelected" />
                                    </td>
                                    <td align="left">
                                        <%#: Eval("BusinessPartnerName")%>
                                    </td>
                                    <td align="right">
                                        <asp:TextBox ID="txtPayerAmount" Width="150px" runat="server" value="0" CssClass="txtCurrency txtPayerAmount"
                                            ReadOnly="true" />
                                    </td>
                                </tr>
                            </ItemTemplate>
                        </asp:ListView>
                    </asp:Panel>
                </dx:PanelContent>
            </PanelCollection>
        </dxcp:ASPxCallbackPanel>
    </td>
</tr>
<div style="margin: 5px;">
    <%=GetLabel("Proses Menjadi Piutang ?") %>
    <input type="button" id="btnPopupYes" value='<%=GetLabel("Ya") %>' />
    <input type="button" value='<%=GetLabel("Tidak") %>' onclick="pcRightPanelContent.Hide();" />
</div>
<dxcp:ASPxCallbackPanel runat="server" ID="cbpProcessPopup" ClientInstanceName="cbpProcessPopup"
    ShowLoadingPanel="false" OnCallback="cbpProcessPopup_Callback">
    <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e) { onCbpProcessPopupEndCallback(s); }" />
</dxcp:ASPxCallbackPanel>
