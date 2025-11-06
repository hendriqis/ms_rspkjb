<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="PaymentBillAllocationEntryCtl.ascx.cs" 
    Inherits="QIS.Medinfras.Web.CommonLibs.Program.PaymentBillAllocationEntryCtl" %>

<script type="text/javascript" id="dxss_paymentbillallocationentry">
    $('.chkPaymentBillAllocationChecked input').change(function () {
        $row = $(this).closest('tr');
        $txt = $row.find('.txtPaymentBillAllocation');
        if ($(this).is(':checked')) {
            $txt.removeAttr('readonly');
            var patientAmount = parseInt($row.find('.hdnPatientAmount').val());
            var remainingAmount = parseInt($('#<%=txtRemainingAmount.ClientID %>').attr('hiddenVal'));
            if (patientAmount > remainingAmount)
                patientAmount = remainingAmount;
            $txt.val(patientAmount).trigger('changeValue');
        }
        else {
            $txt.attr('readonly', 'readonly');
            $txt.val('0').trigger('changeValue');
        }
        $txt.change();
    });

    function onBeforeSaveRecord(errMessage) {
        var result = '';
        var lstBillID = '';
        $('.chkPaymentBillAllocationChecked input:checked').each(function () {
            if (!$(this).is(':disabled')) {
                $row = $(this).closest('tr');
                var value = $row.find('.txtPaymentBillAllocation').attr('hiddenVal');
                var id = $row.find('.hdnKeyField').val();
                if (result != '') {
                    result += '|';
                    lstBillID += ',';
                }
                result += id + ';' + value;
                lstBillID += id;
            }
        });
        $('#<%=hdnResult.ClientID %>').val(result);
        $('#<%=hdnListBillID.ClientID %>').val(lstBillID);
        return true;
    }

    $('.txtCurrency').each(function () {
        $(this).trigger('changeValue');
    });

    $('.txtPaymentBillAllocation').change(function () {
        var value = $(this).val();
        $(this).blur();
        $('#<%=txtPaymentTotal.ClientID %>').focus();
        $row = $(this).closest('tr');
        var newValue = value;
        var patientAmount = parseInt($row.find('.hdnPatientAmount').val());
        if (newValue > patientAmount)
            newValue = patientAmount;
        var remainingAmount = parseInt($('#<%=txtRemainingAmount.ClientID %>').attr('hiddenVal'));
        if (newValue > remainingAmount)
            newValue = remainingAmount;
        if (newValue != value)
            $(this).val(newValue).trigger('changeValue');

        var total = 0;
        $('.txtPaymentBillAllocation').each(function () {
            total += parseInt($(this).attr('hiddenVal'));
        });

        var paymentTotal = parseInt($('#<%=hdnPaymentTotal.ClientID %>').val());
        var remainingAmount = paymentTotal - total;
        $('#<%=txtRemainingAmount.ClientID %>').val(remainingAmount).trigger('changeValue');
    });
</script>

<input type="hidden" id="hdnPaymentTotal" runat="server" />
<input type="hidden" id="hdnResult" runat="server" />
<input type="hidden" id="hdnPaymentID" runat="server" />
<input type="hidden" id="hdnListBillID" runat="server" />
<table class="tblContentArea">
    <colgroup>
        <col style="width:150px"/>
        <col style="width:150px"/>
        <col />
    </colgroup>
    <tr>
        <td><label><%=GetLabel("No Pembayaran")%></label></td>
        <td><asp:TextBox ID="txtPaymentNo" ReadOnly="true" Width="100%" runat="server" /></td>
        <td>&nbsp;</td>
    </tr>
    <tr>
        <td><label><%=GetLabel("Total Pembayaran")%></label></td>
        <td><asp:TextBox ID="txtPaymentTotal" CssClass="number" ReadOnly="true" Width="110px" runat="server" /></td>
    </tr>
    <tr>
        <td><label><%=GetLabel("Sisa Pembayaran")%></label></td>
        <td><asp:TextBox ID="txtRemainingAmount" CssClass="txtCurrency" ReadOnly="true" Width="110px" runat="server" /></td>
    </tr>
    <tr>
        <td colspan="3"><hr /></td>
    </tr>
    <tr>
        <td colspan="3">
            <asp:ListView ID="lvwView" runat="server" OnItemDataBound="lvwView_ItemDataBound">
                <EmptyDataTemplate>
                    <table id="tblView" runat="server" class="grdNormal notAllowSelect" cellspacing="0" rules="all" >
                        <tr>  
                            <th style="width:40px" rowspan="2">
                            </th>
                            <th rowspan="2" align="left">
                                <div style="padding:3px;float:left;">
                                    <div><%= GetLabel("Billing No")%></div>
                                    <div><%= GetLabel("Date Time")%></div>                                                    
                                </div>
                            </th>
                            <th colspan="2" align="center"><%=GetLabel("Tagihan")%></th>
                        </tr>
                        <tr>
                            <th style="width:70px">
                                <div style="text-align:right;padding-right:3px">
                                    <%=GetLabel("Total")%>
                                </div>
                            </th>
                            <th style="width:70px">
                                <div style="text-align:right;padding-right:3px">
                                    <%=GetLabel("Alokasi")%>
                                </div>
                            </th>
                        </tr>
                        <tr class="trEmpty">
                            <td colspan="5">
                                <%=GetLabel("No Data To Display") %>
                            </td>
                        </tr>
                    </table>
                </EmptyDataTemplate>
                <LayoutTemplate>                                
                    <table id="tblView" runat="server" class="grdNormal notAllowSelect" cellspacing="0" rules="all" >
                        <tr>  
                            <th style="width:40px" rowspan="2">
                            </th>
                            <th rowspan="2" align="left">
                                <div style="padding:3px;float:left;">
                                    <div><%= GetLabel("Billing No")%></div>
                                    <div><%= GetLabel("Date Time")%></div>                                                    
                                </div>
                            </th>
                            <th colspan="2" align="center"><%=GetLabel("Tagihan")%></th>
                        </tr>
                        <tr>
                            <th style="width:100px">
                                <div style="text-align:right;padding-right:3px">
                                    <%=GetLabel("Total")%>
                                </div>
                            </th>
                            <th style="width:100px">
                                <div style="text-align:right;padding-right:3px">
                                    <%=GetLabel("Alokasi")%>
                                </div>
                            </th>
                        </tr>
                        <tr runat="server" id="itemPlaceholder" ></tr>
                    </table>
                </LayoutTemplate>
                <ItemTemplate>
                    <tr>
                        <td align="center">
                            <div style="padding:3px">
                                <asp:CheckBox ID="chkPaymentBillAllocationChecked" CssClass="chkPaymentBillAllocationChecked" runat="server" />
                                <input type="hidden" class="hdnKeyField" value="<%#: Eval("PatientBillingID")%>" />
                            </div>
                        </td>
                        <td>
                            <div style="padding:3px;float:left;">
                                <a class="lnkTransactionNo"><%#: Eval("PatientBillingNo")%></a>
                                <div><%#: Eval("BillingDateInString")%> <%#: Eval("BillingTime")%></div>
                            </div>
                        </td>
                        <td>
                            <div style="padding:3px;text-align:right;">
                                <input type="hidden" class="hdnPatientAmount" value='<%#: Eval("PatientRemainingAmount")%>' />
                                <div id="divPatientRemainingAmount" runat="server"><%#: Eval("PatientRemainingAmount", "{0:N}")%></div>                                                   
                            </div>
                        </td>
                        <td>
                            <div style="padding:3px;text-align:right;">
                                <input type="text" id="txtPaymentBillAllocation" runat="server" value="0" style="width:100%; font-size: 1em;" class="txtCurrency txtPaymentBillAllocation" readonly="readonly" />
                            </div>
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:ListView>
        </td>
    </tr>
</table>