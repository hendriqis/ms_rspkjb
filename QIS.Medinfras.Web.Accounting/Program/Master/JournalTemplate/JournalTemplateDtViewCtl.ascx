<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="JournalTemplateDtViewCtl.ascx.cs"
    Inherits="QIS.Medinfras.Web.Accounting.Program.JournalTemplateDtViewCtl" %>
<%@ Register Assembly="DevExpress.Web.ASPxEditors.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxEditors" TagPrefix="dxe" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxCallbackPanel" TagPrefix="dxcp" %>
<%@ Register Assembly="DevExpress.Web.v11.1, Version=11.1.5.0, Culture=neutral, PublicKeyToken=b88d1754d700e49a"
    Namespace="DevExpress.Web.ASPxPanel" TagPrefix="dx" %>
<script type="text/javascript" id="dxss_journaltemplatedtviewctl">
    function onLoad() {
        if (cboJournalTemplateType.GetValue() == Constant.JournalTemplateType.ALOKASI) {
            $('.txtAmount').each(function () {
                $(this).attr('readonly', 'readonly');
            });
        } else {
            $('.txtAmount').each(function () {
                $(this).removeAttr('readonly');
            });
        }
    }

    $('.txtCurrency').each(function () {
        $(this).trigger('changeValue');
    });

    function oncboJournalTemplateTypeChanged() {
        if (cboJournalTemplateType.GetValue() == Constant.JournalTemplateType.ALOKASI) {
            $('#<%=txtTransactionAmount.ClientID %>').removeAttr('readonly');
            $('#<%=txtTransactionAmount.ClientID %>').val("0");
            $('#<%=btnCalculate.ClientID %>').removeAttr('style');

            $('.txtAmount').each(function () {
                $(this).attr('readonly', 'readonly');
            });
        } else {
            $('#<%=txtTransactionAmount.ClientID %>').attr('readonly', 'readonly');
            $('#<%=txtTransactionAmount.ClientID %>').val("0");
            $('#<%=btnCalculate.ClientID %>').attr('style', 'display:none');

            $('.txtAmount').each(function () {
                $(this).removeAttr('readonly');
            });
        }
    }

    $('#<%=btnCalculate.ClientID %>').click(function () {
        if (cboJournalTemplateType.GetValue() == Constant.JournalTemplateType.ALOKASI) {
            var transactionAmount = parseFloat($('#<%=txtTransactionAmount.ClientID %>').attr('hiddenVal'));
            $('.txtAmount').each(function () {
                var percentage = parseFloat($(this).attr('amountpercentage'));
                var amount = transactionAmount * percentage / 100;
                $(this).val(amount).trigger('changeValue');
            });
        }
    });
</script>
<div style="height: 450px; overflow-y: auto; overflow-x: hidden">
    <input type="hidden" id="hdnTemplateID" value="" runat="server" />
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
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Kode Template")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtTemplateCode" ReadOnly="true" Width="300px" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Nama Template")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtTemplateName" ReadOnly="true" Width="300px" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblMandatory">
                                <%=GetLabel("Tipe")%></label>
                        </td>
                        <td colspan="2">
                            <dxe:ASPxComboBox ID="cboJournalTemplateType" ClientInstanceName="cboJournalTemplateType"
                                Width="300px" runat="server">
                                <ClientSideEvents ValueChanged="function(s,e){ oncboJournalTemplateTypeChanged(); }" />
                            </dxe:ASPxComboBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="tdLabel">
                            <label class="lblNormal">
                                <%=GetLabel("Nilai Transaksi")%></label>
                        </td>
                        <td>
                            <asp:TextBox ID="txtTransactionAmount" CssClass="txtCurrency" Width="150px" runat="server" />
                            <input type="button" id="btnCalculate" value="Hitung" runat="server" />
                        </td>
                    </tr>
                </table>
                <dxcp:ASPxCallbackPanel ID="cbpEntryPopupView" runat="server" Width="100%" ClientInstanceName="cbpEntryPopupView"
                    ShowLoadingPanel="false" OnCallback="cbpEntryPopupView_Callback">
                    <ClientSideEvents BeginCallback="function(s,e){ showLoadingPanel(); }" EndCallback="function(s,e){ hideLoadingPanel(); }" />
                    <PanelCollection>
                        <dx:PanelContent ID="PanelContent1" runat="server">
                            <asp:Panel runat="server" ID="pnlEntryPopupGrdView" Style="width: 100%; margin-left: auto;
                                margin-right: auto; position: relative; font-size: 0.95em;">
                                <table style="width: 100%">
                                    <tr>
                                        <td style="width: 50%" valign="top">
                                            <h4 style="text-align: center">
                                                <%=GetLabel("DEBET") %></h4>
                                            <asp:GridView ID="grdViewD" OnItemDataBound="grdViewD_RowDataBound" runat="server"
                                                CssClass="grdView notAllowSelect" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true"
                                                EmptyDataRowStyle-CssClass="trEmpty">
                                                <Columns>
                                                    <asp:TemplateField HeaderText="COA" HeaderStyle-Width="70px">
                                                        <ItemTemplate>
                                                            <div style="font-size: 14px;">
                                                                <%#:Eval("GLAccountNo") %></div>
                                                            <div style="font-size: 10px;">
                                                                <%#:Eval("GLAccountName") %></div>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Segment">
                                                        <ItemTemplate>
                                                            <div style="font-size: 10px;">
                                                                DP:
                                                                <%#:Eval("DepartmentID") %></div>
                                                            <div style="font-size: 10px;">
                                                                SU:
                                                                <%#:Eval("ServiceUnitName") %></div>
                                                            <div style="font-size: 10px;">
                                                                RC:
                                                                <%#:Eval("RevenueCostCenterName") %></div>
                                                            <div style="font-size: 10px;">
                                                                CG:
                                                                <%#:Eval("CustomerGroupName") %></div>
                                                            <div style="font-size: 10px;">
                                                                BP:
                                                                <%#:Eval("BusinessPartnerName") %></div>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="AmountPercentage" HeaderText="Bagian (%)" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right"
                                                        HeaderStyle-Width="50px" />
                                                    <asp:BoundField DataField="Amount" DataFormatString="{0:n2}" HeaderText="Bagian (Rp)" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right"
                                                        HeaderStyle-Width="80px" />
                                                    <asp:TemplateField HeaderText="Jumlah" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right"
                                                        HeaderStyle-Width="150px">
                                                        <ItemTemplate>
                                                            <input type="hidden" value='<%#:Eval("AmountPercentage") %>' class="AmountPercentage"
                                                                id="AmountPercentage" runat="server" />
                                                            <input id="txtAmountD" runat="server" type="text" readonly="readonly" amountpercentage='<%#:Eval("AmountPercentage") %>'
                                                                class="txtAmount txtCurrency" style="width: 100%" value='<%#:Eval("Amount") %>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                                <EmptyDataTemplate>
                                                    <%=GetLabel("No Data To Display")%>
                                                </EmptyDataTemplate>
                                            </asp:GridView>
                                        </td>
                                        <td style="width: 50%" valign="top">
                                            <h4 style="text-align: center">
                                                <%=GetLabel("KREDIT") %></h4>
                                            <asp:GridView ID="grdViewK" OnItemDataBound="grdViewK_RowDataBound" runat="server"
                                                CssClass="grdView notAllowSelect" AutoGenerateColumns="false" ShowHeaderWhenEmpty="true"
                                                EmptyDataRowStyle-CssClass="trEmpty">
                                                <Columns>
                                                    <asp:TemplateField HeaderText="COA" HeaderStyle-Width="70px">
                                                        <ItemTemplate>
                                                            <div style="font-size: 14px;">
                                                                <%#:Eval("GLAccountNo") %></div>
                                                            <div style="font-size: 10px;">
                                                                <%#:Eval("GLAccountName") %></div>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Segment">
                                                        <ItemTemplate>
                                                            <div style="font-size: 10px;">
                                                                DP:
                                                                <%#:Eval("DepartmentID") %></div>
                                                            <div style="font-size: 10px;">
                                                                SU:
                                                                <%#:Eval("ServiceUnitName") %></div>
                                                            <div style="font-size: 10px;">
                                                                RC:
                                                                <%#:Eval("RevenueCostCenterName") %></div>
                                                            <div style="font-size: 10px;">
                                                                CG:
                                                                <%#:Eval("CustomerGroupName") %></div>
                                                            <div style="font-size: 10px;">
                                                                BP:
                                                                <%#:Eval("BusinessPartnerName") %></div>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="AmountPercentage" HeaderText="Bagian (%)" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right"
                                                        HeaderStyle-Width="50px" />
                                                    <asp:BoundField DataField="Amount" DataFormatString="{0:n2}" HeaderText="Bagian (Rp)" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right"
                                                        HeaderStyle-Width="80px" />
                                                    <asp:TemplateField HeaderText="Jumlah" HeaderStyle-HorizontalAlign="Right" ItemStyle-HorizontalAlign="Right"
                                                        HeaderStyle-Width="150px">
                                                        <ItemTemplate>
                                                            <input type="hidden" value='<%#:Eval("AmountPercentage") %>' class="AmountPercentage"
                                                                id="AmountPercentage" runat="server" />
                                                            <input id="txtAmountK" runat="server" type="text" readonly="readonly" amountpercentage='<%#:Eval("AmountPercentage") %>'
                                                                class="txtAmount txtCurrency" style="width: 100%" value='<%#:Eval("Amount") %>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                                <EmptyDataTemplate>
                                                    <%=GetLabel("No Data To Display")%>
                                                </EmptyDataTemplate>
                                            </asp:GridView>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </dx:PanelContent>
                    </PanelCollection>
                </dxcp:ASPxCallbackPanel>
            </td>
        </tr>
    </table>
    <div style="width: 100%; text-align: right">
        <input type="button" value='<%= GetLabel("Close")%>' onclick="pcRightPanelContent.Hide();" />
    </div>
</div>
